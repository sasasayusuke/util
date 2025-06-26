import copy
import uuid
from datetime import datetime
from typing import List, Tuple

from dateutil.relativedelta import relativedelta
from fastapi import HTTPException, status
from pydantic import BaseModel
from pynamodb.exceptions import DoesNotExist
from pytz import timezone

from app.core.common_logging import CustomLogger
from app.models.master import MasterServiceManagerModel
from app.models.project import ProjectModel
from app.models.project_karte import ProjectKarteModel
from app.models.project_survey import (
    PointsAttribute,
    ProjectSurveyModel,
    SupporterUserAttribute,
)
from app.models.survey_master import SurveyMasterModel
from app.models.user import UserModel
from app.resources.const import (
    DataType,
    MasterDataType,
    ScheduleTiming,
    SurveyResponseDate,
    SurveyType,
    SurveyTypeExcludingPP,
    UserRoleType,
)
from app.schemas.schedule import (
    BulkCreateSurveyScheduleRequest,
    BulkCreateSurveyScheduleResponse,
    BulkUpdateSurveyScheduleRequest,
    BulkUpdateSurveyScheduleResponse,
    CreateSupportSchedulesRequest,
    CreateSupportSchedulesResponse,
    CreateSurveySchedulesRequest,
    CreateSurveySchedulesResponse,
    DeleteScheduleIdDateResponse,
    DeleteSupportSchedulesByIdDateQuery,
    DeleteSurveySchedulesByIdDateQuery,
    GetSupportSchedulesByIdResponse,
    GetSurveySchedulesByIdResponse,
    ProjectSupportScheduleInfo,
    ProjectSurveyScheduleInfo,
    PutScheduleIdDateResponse,
    PutSupportScheduleIdDateRequest,
    PutSupportScheduleIdDateResponse,
    PutSupportSchedulesByIdDateQuery,
    PutSurveyScheduleIdDateQuery,
    PutSurveyScheduleIdDateRequest,
)
from app.service.common_service.cached_db_items import CachedDbItems
from app.service.common_service.check_permission import (
    is_assigned_to_project,
)
from app.service.project_service import ProjectService
from app.utils.date import (
    get_after_business_day,
    get_before_business_day,
    get_completion_request_date,
    get_continuous_business_date_for_each_month,
    get_continuous_date_for_each_month,
    get_continuous_date_for_each_week,
    get_datetime_now,
    get_first_business_day_of_the_next_month,
    get_last_business_date_of_month,
    get_last_date_of_month,
    get_pair_business_date_for_once,
)

logger = CustomLogger.get_logger()


class SurveySchedule(BaseModel):
    survey_request_date: str
    response_limit_date: str = None


class BulkCreateSurveySchedule(BaseModel):
    id: str
    survey_request_date: str
    response_limit_date: str = None
    type: SurveyType


class SchedulesService:
    @staticmethod
    def is_visible_project(user: UserModel, project_id: str) -> bool:
        """案件情報がアクセス可能か判定.
                1.所属案件(アクセス可)
                ・支援者
                ・支援者責任者
                ・営業担当者
                2.アクセス不可(API呼出権限で制御)
                ・顧客
                ・営業責任者
                3.制限なし
                ・事業者責任者

        Args:
            project_id (案件ID)
        Returns:
            bool: True(アクセス可能)、False(アクセス不可)
        """
        project = ProjectModel.get(hash_key=project_id, range_key=DataType.PROJECT)

        if user.role in [
            UserRoleType.SUPPORTER.key,
            UserRoleType.SUPPORTER_MGR.key,
        ]:
            if project.main_supporter_user_id != user.id and (
                not project.supporter_user_ids
                or (user.id not in project.supporter_user_ids)
            ):
                # 所属していない案件
                return False

            return True

        elif user.role == UserRoleType.SALES.key:
            if project.main_sales_user_id != user.id:
                # 所属していない案件
                return False

            return True

        elif user.role == UserRoleType.BUSINESS_MGR.key:
            return True

    def is_visible_project_get_by_id(user: UserModel, project_id: str) -> bool:
        """案件情報がアクセス可能か判定.
                1.所属案件(アクセス可)
                ・顧客
                2.所属していない非公開案件を除く全案件(アクセス可)
                ・支援者
                ・営業担当者
                3.制限なし
                ・営業責任者
                ・事業者責任者
                4. 非所属非公開案件を除く全案件(アクセス可)
                ・支援者責任者

        Args:
            project_id (案件ID)
        Returns:
            bool: True(アクセス可能)、False(アクセス不可)
        """
        project = ProjectModel.get(hash_key=project_id, range_key=DataType.PROJECT)

        if user.role == UserRoleType.SUPPORTER.key:
            # 支援者の場合
            if (
                project.main_supporter_user_id != user.id
                and (
                    not project.supporter_user_ids
                    or (user.id not in project.supporter_user_ids)
                )
                and project.is_secret
            ):
                # 所属していない非公開案件
                return False

            return True

        elif user.role == UserRoleType.SALES.key:
            if project.main_sales_user_id != user.id and project.is_secret:
                # 所属していない非公開案件
                return False

            return True

        elif user.role == UserRoleType.CUSTOMER.key:
            for project_itr in user.project_ids:
                if project_itr == project.id:
                    # 参加している案件
                    return True

            # 参加していない案件
            return False

        elif (
            user.role == UserRoleType.SALES_MGR.key
            or user.role == UserRoleType.BUSINESS_MGR.key
        ):
            return True

        elif user.role == UserRoleType.SUPPORTER_MGR.key:
            # 支援者責任者の場合
            if (
                project.main_supporter_user_id
                and project.main_supporter_user_id == user.id
                or (
                    project.supporter_user_ids
                    and (user.id in project.supporter_user_ids)
                )
            ):
                # 担当案件
                return True
            else:
                if project.supporter_organization_id in user.supporter_organization_id:
                    # 非担当所属課案件
                    return True
                else:
                    # 非担当非所属課案件
                    if project.is_secret:
                        # 非公開案件
                        return False
                    else:
                        # 公開案件
                        return True

    @staticmethod
    def get_support_schedules_during_period(
        support_start_date: str,
        support_end_date: str,
        support_date: str,
        timing: ScheduleTiming,
    ) -> List[str]:
        """支援期間中の支援スケジュールを取得します。

        Args:
            support_start_date (str): 支援開始日
            support_end_date (str): 支援終了日
            date (str): 支援日（20xx/xx/xx）
            timing (ScheduleTiming): 支援スケジュール作成のタイミング（毎月・毎週・1回）

        Returns:
            List: 支援スケジュール
        """
        now = get_datetime_now()
        # 開催日時の判定（既に支援開始されてる場合は、現在日時から計算）
        if now.strftime("%Y/%m/%d") > support_start_date:
            start_date = now
        else:
            start_date = datetime.strptime(support_start_date, "%Y/%m/%d")

        if timing == ScheduleTiming.MONTHLY:
            return get_continuous_date_for_each_month(
                start_date=start_date,
                end_date=datetime.strptime(support_end_date, "%Y/%m/%d"),
                offset_day=int(support_date),
            )

        elif timing == ScheduleTiming.WEEKLY:
            return get_continuous_date_for_each_week(
                start_date=start_date,
                end_date=datetime.strptime(support_end_date, "%Y/%m/%d"),
                day_of_week_index=int(support_date),
            )

        else:
            return [support_date]

    @staticmethod
    def create_support_schedules(
        item: CreateSupportSchedulesRequest, project_id: str, current_user: UserModel
    ) -> CreateSupportSchedulesResponse:
        """案件の支援スケジュールを指定期間もしくは指定日で作成します。

        Args:
            item (CreateSchedulesRequest): 登録内容
            project_id (str): 案件ID
            current_user (UserModel): 認証済みユーザー

        Raises:
            HTTPException: 404

        Returns:
            CreateSchedulesResponse: 登録結果
        """

        # 権限チェック
        if not SchedulesService.is_visible_project(current_user, project_id):
            logger.warning("CreateSchedules forbidden.")
            raise HTTPException(
                status_code=status.HTTP_403_FORBIDDEN, detail="Forbidden"
            )

        # 案件存在チェック
        try:
            project = ProjectModel.get(hash_key=project_id, range_key=DataType.PROJECT)
        except DoesNotExist:
            logger.info(f"Not found project_id from Project table: {project_id}")
            raise HTTPException(status_code=status.HTTP_404_NOT_FOUND)

        additional_schedules = SchedulesService.get_support_schedules_during_period(
            support_start_date=project.support_date_from,
            support_end_date=project.support_date_to,
            support_date=item.support_date,
            timing=item.timing,
        )

        update_datetime = datetime.now()
        # カルテを空の状態で支援スケジュール分作成
        with ProjectKarteModel.batch_write() as project_karte_batch:
            for additional_schedule in additional_schedules:
                karte_id = str(uuid.uuid4())
                karte = ProjectKarteModel(
                    karte_id=karte_id,
                    project_id=project.id,
                    customer_id=project.customer_id,
                    date=additional_schedule,
                    start_datetime=(additional_schedule + " " + item.start_time),
                    start_time=item.start_time,
                    end_time=item.end_time,
                    create_at=update_datetime,
                    update_at=update_datetime,
                    version=1,
                )
                project_karte_batch.save(karte)

        return CreateSupportSchedulesResponse()

    @staticmethod
    def get_support_schedules_by_id(
        project_id: str, current_user: UserModel
    ) -> GetSupportSchedulesByIdResponse:
        """案件IDを指定して支援スケジュール情報を取得する

        Args:
            project_id (str): 案件ID
            current_user (AuthUser): 認証済みユーザー

        Raises:
            HTTPException: 403 Forbidden

        Returns:
            GetSupportSchedulesByIdResponse: 取得結果
        """

        schedule_list: List[ProjectSupportScheduleInfo] = []

        karte_list: List[ProjectKarteModel] = list(
            ProjectKarteModel.project_id_start_datetime_index.query(hash_key=project_id)
        )

        project = ProjectModel.get(hash_key=project_id, range_key=DataType.PROJECT)

        # 更新者がいない(draft_supporter_idがNULL)を考慮
        item_update_user: str = ""
        for karte in karte_list:
            if karte.draft_supporter_id is not None:
                # 支援者名取得
                user_info = UserModel
                try:
                    user_info = UserModel.get(
                        hash_key=karte.draft_supporter_id,
                        range_key=DataType.USER,
                    )
                    item_update_user = user_info.name
                except DoesNotExist:
                    item_update_user = ""
            else:
                item_update_user = ""

            schedule_list.append(
                ProjectSupportScheduleInfo(
                    year_month=datetime.strptime(karte.date, "%Y/%m/%d").strftime(
                        "%Y/%m"
                    ),
                    support_date=karte.date,
                    support_start_time=karte.start_time,
                    support_end_time=karte.end_time,
                    completed=not karte.is_draft,
                    karte_id=karte.karte_id,
                    is_accessible_karte_detail=SchedulesService.is_accessible_karte_detail(
                        current_user, project
                    ),
                    last_update_datetime=karte.last_update_datetime,
                    update_user=item_update_user,
                    version=karte.version,
                )
            )

        # スケジュールがないときは 空リストをレスポンスとする
        return GetSupportSchedulesByIdResponse(
            project_id=project_id,
            project_schedules=sorted(
                schedule_list, key=lambda x: (x.support_date, x.support_start_time)
            ),
        )

    @staticmethod
    def is_accessible_karte_detail(
        current_user: UserModel, project: ProjectModel
    ) -> bool:
        """カルテ詳細を表示可能か判定する

        Args:
            karte (ProjectKarteModel): カルテ情報
            current_user(AdminModel): ログインユーザー情報
        Returns:
            bool: True(表示可能)、False(表示不可)
        """
        if (
            current_user.role == UserRoleType.SALES_MGR.key
            or current_user.role == UserRoleType.BUSINESS_MGR.key
            or current_user.role == UserRoleType.SUPPORTER_MGR.key
        ):
            return True

        elif current_user.role in [
            UserRoleType.SALES.key,
            UserRoleType.SUPPORTER.key,
        ]:
            return is_assigned_to_project(current_user.project_ids, project.id)

        return False

    @staticmethod
    def update_support_schedules_by_id_qnd_date(
        item: PutSupportScheduleIdDateRequest,
        karte_id: str,
        query_params: PutSupportSchedulesByIdDateQuery,
        current_user: UserModel,
    ) -> PutSupportScheduleIdDateResponse:
        """カルテIDを指定して支援スケジュール情報を更新する

        Args:
            body (PutSupportScheduleIdDateRequest): 更新内容
            karte_id (str): カルテID パスパラメータで指定
            PutSupportSchedulesByIdDateQuery (schedule_id, version):  クエリパラメータで指定
            current_user (Behavior, optional): 認証済みのユーザー

        Raises:
            HTTPException: 404 Not Found
            HTTPException: 409 Conflict

        Returns:
            PutSupportScheduleIdDateResponse: 更新結果
        """

        # カルテ取得
        try:
            karte = ProjectKarteModel.get(hash_key=karte_id)
        except DoesNotExist:
            logger.warning(
                f"UpdateSupportSchedule karte not found. karte_id: {karte_id}"
            )
            raise HTTPException(status_code=status.HTTP_404_NOT_FOUND)

        # 権限チェック
        if not SchedulesService.is_visible_project(current_user, karte.project_id):
            logger.warning(
                "PutSupportScheduleIdDate forbidden.current_user:{current_user.id} karte_id: {karte_id} project_id: {karte.project_id}"
            )
            raise HTTPException(
                status_code=status.HTTP_403_FORBIDDEN, detail="Forbidden"
            )

        version = query_params.version
        # 排他チェック
        if version != karte.version:
            logger.warning(
                f"PutSupportScheduleIdDate conflict. request_ver:{version} project_ver: {karte.version}"
            )
            raise HTTPException(status_code=status.HTTP_409_CONFLICT, detail="Conflict")

        # 更新したい支援「日」取得
        item_date_split = item.support_date.split("/")
        update_year_month = item_date_split[0] + "/" + item_date_split[1].zfill(2)
        update_day = int(item_date_split[2])
        # 更新する時間を取得
        update_support_start_time = item.support_start_time
        update_support_end_time = item.support_end_time

        # カルテを更新する
        karte_date: str = update_year_month + "/" + str(update_day).zfill(2)
        karte.update(
            actions=[
                ProjectKarteModel.date.set(karte_date),
                ProjectKarteModel.start_time.set(update_support_start_time),
                ProjectKarteModel.end_time.set(update_support_end_time),
                ProjectKarteModel.start_datetime.set(
                    karte_date + " " + update_support_start_time
                ),
                ProjectKarteModel.update_id.set(current_user.id),
                ProjectKarteModel.update_at.set(datetime.now()),
            ]
        )

        return PutSupportScheduleIdDateResponse(message="OK")

    @staticmethod
    def delete_support_schedules_by_id_date(
        query_params: DeleteSupportSchedulesByIdDateQuery,
        current_user: UserModel,
    ) -> DeleteScheduleIdDateResponse:
        """Delete /schedules/support 「支援」スケジュール削除API

        Args:
            query_params (DeleteSupportSchedulesByIdDateQuery): クエリパラメータ
            current_user (Behavior, optional): 認証済みのユーザー

        Raises:
            HTTPException: 403 Forbidden
            HTTPException: 404 Not Found
            HTTPException: 409 Conflict

        Returns:
            DeleteScheduleIdDateResponse: 更新結果
        """

        karte_id = query_params.karte_id
        # カルテ取得
        try:
            karte = ProjectKarteModel.get(hash_key=karte_id)
        except DoesNotExist:
            logger.warning(
                f"DeleteSupportSchedule karte not found. karte_id: {karte_id}"
            )
            raise HTTPException(status_code=status.HTTP_404_NOT_FOUND)

        # 権限チェック
        if not SchedulesService.is_visible_project(current_user, karte.project_id):
            logger.info(
                "DeleteScheduleIdDate forbidden. current_user:{current_user.id} karte_id: {karte_id} project_id: {karte.project_id}"
            )
            raise HTTPException(
                status_code=status.HTTP_403_FORBIDDEN, detail="Forbidden"
            )

        # 排他チェック
        if query_params.version != karte.version:
            logger.info(
                f"DeleteScheduleIdDate conflict. request_ver:{query_params.version} karte_ver: {karte.version}"
            )
            raise HTTPException(status_code=status.HTTP_409_CONFLICT, detail="Conflict")

        # 案件カルテを削除
        karte.delete()

        return DeleteScheduleIdDateResponse(message="OK")

    @staticmethod
    def create_survey_schedules(
        project_id: str,
        item: CreateSurveySchedulesRequest,
        current_user: UserModel,
    ) -> CreateSurveySchedulesResponse:
        """支援期間内のアンケートスケジュールを作成する

        Args:
            project_id (str): 案件ID
            item (CreateSurveySchedulesRequest): 登録内容
            current_user (UserModel): 認証済みユーザー

        Raises:
            HTTPException: _description_
        """

        # 権限チェック
        if not SchedulesService.is_visible_project(current_user, project_id):
            logger.info("CreateSurveySchedules forbidden")
            raise HTTPException(
                status_code=status.HTTP_403_FORBIDDEN, detail="Forbidden"
            )

        project = ProjectModel.get_project(project_id=project_id)

        survey_schedules: List[SurveySchedule] = (
            SchedulesService.get_survey_schedules_during_period(
                survey_type=item.survey_type,
                support_start_date=project.support_date_from,
                support_end_date=project.support_date_to,
                request_date=item.request_date,
                timing=item.timing,
                limit_date=item.limit_date,
            )
        )

        user_id_name_map: dict = {}
        search_user_id_list: List = []
        if project.main_supporter_user_id:
            search_user_id_list.append(project.main_supporter_user_id)
        if project.main_customer_user_id:
            search_user_id_list.append(project.main_customer_user_id)
        if project.main_sales_user_id:
            search_user_id_list.append(project.main_sales_user_id)
        if project.supporter_user_ids:
            for user_id in project.supporter_user_ids:
                search_user_id_list.append(user_id)
        user_item_keys = [(id, DataType.USER) for id in search_user_id_list]
        for user in UserModel.batch_get(user_item_keys):
            user_id_name_map[user.id] = user.name

        # アンケートテーブルに所属課を登録する
        db_main_supporter_user: SupporterUserAttribute
        if project.main_supporter_user_id:
            main_supporter_user = UserModel.get_user_by_id(
                project.main_supporter_user_id
            )
            main_supporter_user_organization_id: str = ""
            list_supporter_user_organization_name: List = []
            main_supporter_user_organization_name: str = ""

            main_supporter_user_organization_id = ";".join(
                main_supporter_user.supporter_organization_id
            )
            for (
                supporter_organization_id_itr
            ) in main_supporter_user.supporter_organization_id:
                list_supporter_user_organization_name.append(
                    ProjectService.get_supporter_organization_name(
                        supporter_organization_id_itr
                    )
                )
            main_supporter_user_organization_name = ";".join(
                list_supporter_user_organization_name
            )

            db_main_supporter_user = SupporterUserAttribute(
                id=project.main_supporter_user_id,
                name=user_id_name_map.get(project.main_supporter_user_id, ""),
                organization_id=main_supporter_user_organization_id,
                organization_name=main_supporter_user_organization_name,
            )
        else:
            db_main_supporter_user = None

        if item.survey_type == SurveyTypeExcludingPP.QUICK.value:
            latest_survey_master: SurveyMasterModel = (
                SurveyMasterModel.get_latest_survey_masters(
                    survey_type=item.survey_type, survey_master_id=item.survey_master_id
                )
            )
        else:
            latest_survey_master: SurveyMasterModel = (
                SurveyMasterModel.get_latest_survey_masters(
                    survey_type=item.survey_type
                )
            )

        service_types: List[dict] = CachedDbItems.ReturnServiceTypes()
        service_type_name = ""
        for service_type_dict in service_types:
            if service_type_dict["id"] == project.service_type:
                service_type_name = service_type_dict["name"]
                break

        # support_users List を保存用に作成 -> ユーザーid,name のみ入れて一括作成する
        support_users_list: List[SupporterUserAttribute] = []
        if project.supporter_user_ids:
            for user_ids_itr in project.supporter_user_ids:
                # 支援者組織
                user_ids_itr_supporter_user = UserModel.get_user_by_id(user_ids_itr)
                user_ids_itr_organization_id: str = ""
                list_ser_ids_itr_organization_name: List = []
                user_ids_itr_organization_name: str = ""

                user_ids_itr_organization_id = ";".join(
                    user_ids_itr_supporter_user.supporter_organization_id
                )
                for (
                    supporter_organization_id_itr
                ) in user_ids_itr_supporter_user.supporter_organization_id:
                    list_ser_ids_itr_organization_name.append(
                        ProjectService.get_supporter_organization_name(
                            supporter_organization_id_itr
                        )
                    )
                user_ids_itr_organization_name = ";".join(
                    list_ser_ids_itr_organization_name
                )

                support_users_list.append(
                    SupporterUserAttribute(
                        id=user_ids_itr,
                        name=user_id_name_map.get(user_ids_itr, ""),
                        organization_id=copy.deepcopy(user_ids_itr_organization_id),
                        organization_name=copy.deepcopy(user_ids_itr_organization_name),
                    )
                )
        else:
            # アクセラレータが登録されていない時はNoneでDB保存処理を行う
            support_users_list = None

        service_manager_id = None
        service_manager_name = None
        if project.supporter_organization_id:
            try:
                filter_condition_master_service_manager = None
                filter_condition_master_service_manager &= (
                    MasterServiceManagerModel.supporter_organization_id
                    == project.supporter_organization_id
                )
                master_service_manager: MasterServiceManagerModel = next(
                    MasterServiceManagerModel.data_type_index.query(
                        hash_key=MasterDataType.SERVICE_MANAGER.value,
                        filter_condition=filter_condition_master_service_manager,
                    )
                )
                service_manager_id = master_service_manager.id
                service_manager_name = master_service_manager.name
            except StopIteration:
                pass

        # 案件アンケートの登録
        # 一括登録ではないため、スケジュールグループID（survey_group_id）は登録しない
        with ProjectSurveyModel.batch_write() as project_survey_batch:
            for survey_schedule in survey_schedules:
                project_survey_batch.save(
                    ProjectSurveyModel(
                        id=str(uuid.uuid4()),
                        data_type=DataType.SURVEY,
                        survey_master_id=latest_survey_master.id,
                        survey_master_revision=latest_survey_master.revision,
                        survey_type=item.survey_type,
                        project_id=project_id,
                        project_name=project.name,
                        customer_success=project.customer_success,
                        supporter_organization_id=project.supporter_organization_id,
                        supporter_organization_name=ProjectService.get_supporter_organization_name(
                            project.supporter_organization_id
                        ),
                        support_date_from=project.support_date_from,
                        support_date_to=project.support_date_to,
                        main_supporter_user=db_main_supporter_user,
                        supporter_users=support_users_list,
                        sales_user_id=project.main_sales_user_id,
                        service_type_id=project.service_type,
                        service_type_name=service_type_name,
                        service_manager_id=service_manager_id,
                        service_manager_name=service_manager_name,
                        answer_user_id=project.main_customer_user_id,
                        answer_user_name=user_id_name_map.get(
                            project.main_customer_user_id, ""
                        ),
                        customer_id=project.customer_id,
                        customer_name=project.customer_name,
                        points=PointsAttribute(
                            satisfaction=0,
                            continuation=0,
                            recommended=0,
                            sales=0,
                            survey_satisfaction=0,
                            man_hour_satisfaction=0,
                            karte_satisfaction=0,
                            master_karte_satisfaction=0,
                        ),
                        summary_month=survey_schedule.survey_request_date.rsplit(
                            "/", 1
                        )[0],
                        plan_survey_request_datetime=SchedulesService.make_request_datetime(
                            survey_schedule.survey_request_date
                        ),
                        plan_survey_response_datetime=SchedulesService.make_request_datetime(
                            survey_schedule.response_limit_date
                        ),
                        # NPFから取得したもの
                        this_month_type=project.this_month_type,
                        no_send_reason=project.no_send_reason,
                        create_id=current_user.id,
                        create_at=datetime.now(),
                        update_at=datetime.now(),
                        version=1,
                    )
                )

        return CreateSurveySchedulesResponse()

    @staticmethod
    def get_survey_schedules_by_id(
        project_id: str, current_user: UserModel
    ) -> GetSurveySchedulesByIdResponse:
        """案件IDを指定してアンケートスケジュール情報を取得する

        Args:
        project_id (str): 案件ID パスパラメータで指定
        current_user (Behavior, optional): 認証済みのユーザー

        Raises:
            HTTPException: 403 Forbidden

        Returns:
            GetSurveySchedulesByIdResponse: 取得結果
        """

        schedule_list: List[ProjectSurveyScheduleInfo] = []

        # 案件アンケート取得
        survey_list: List[ProjectSurveyModel] = list(
            ProjectSurveyModel.project_id_summary_month_index.query(hash_key=project_id)
        )

        for survey in survey_list:
            survey_group_id_by_survey = (
                survey.survey_group_id if survey.survey_group_id else ""
            )

            send_date = (
                datetime.strptime(
                    survey.plan_survey_request_datetime, "%Y/%m/%d %H:%M"
                ).strftime("%Y/%m/%d")
                if survey.plan_survey_request_datetime
                else ""
            )
            survey_limit_date = (
                datetime.strptime(
                    survey.plan_survey_response_datetime, "%Y/%m/%d %H:%M"
                ).strftime("%Y/%m/%d")
                if survey.plan_survey_response_datetime
                else ""
            )
            schedule_list.append(
                ProjectSurveyScheduleInfo(
                    schedule_group_id=survey_group_id_by_survey,
                    survey_id=survey.id,
                    send_date=send_date,
                    survey_name=survey.survey_type,
                    survey_limit_date=survey_limit_date,
                    version=survey.version,
                )
            )

        # グループid がないときは、空文字をレスポンスとする
        return_survey_group_id = ""
        try:
            project = ProjectModel.get(hash_key=project_id, range_key=DataType.PROJECT)
        except DoesNotExist:
            logger.warning(
                f"GetSurveySchedulesById project not found. project_id: {project_id}"
            )
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="project not found"
            )
        if project.survey_group_id is not None:
            return_survey_group_id = project.survey_group_id

        # スケジュールがないときは 空リストをレスポンスとする
        return GetSurveySchedulesByIdResponse(
            project_id=project_id,
            survey_group_id=return_survey_group_id,
            project_schedules=sorted(schedule_list, key=lambda x: x.send_date),
        )

    @staticmethod
    def update_survey_schedules_by_id_and_date(
        item: PutSurveyScheduleIdDateRequest,
        query_params: PutSurveyScheduleIdDateQuery,
        current_user: UserModel,
    ) -> PutScheduleIdDateResponse:
        """Put /schedules/survey 「アンケート」スケジュール更新API

        Args:
            body (PutSurveyScheduleIdDateRequest): 更新内容
            survey_id (str): アンケートID クエリパラメータで指定
            version (int): ロックキー（楽観ロック制御） クエリパラメータで指定(>=1)
            current_user (Behavior, optional): 認証済みのユーザー

        Raises:
            HTTPException: 400 Bad Request
            HTTPException: 403 Forbidden
            HTTPException: 404 Not Found
            HTTPException: 409 Conflict

        Returns:
            PutScheduleIdDateResponse: 更新結果
        """

        survey_id = query_params.survey_id
        # 案件アンケート取得
        try:
            survey = ProjectSurveyModel.get(
                hash_key=survey_id, range_key=DataType.SURVEY
            )
        except DoesNotExist:
            logger.warning(
                f"UpdateSurveySchedule survey not found. survey_id: {survey_id}"
            )
            raise HTTPException(status_code=status.HTTP_404_NOT_FOUND)

        # 権限チェック
        if not SchedulesService.is_visible_project(current_user, survey.project_id):
            logger.warning(
                "PutScheduleIdDate forbidden. current_user:{current_user.id} project_id: {survey.project_id}"
            )
            raise HTTPException(
                status_code=status.HTTP_403_FORBIDDEN, detail="Forbidden"
            )

        # 排他チェック
        if query_params.version != survey.version:
            logger.warning(
                f"PutScheduleIdDate conflict. request_ver:{query_params.version} project_ver: {survey.version}"
            )
            raise HTTPException(status_code=status.HTTP_409_CONFLICT, detail="Conflict")

        # 更新可能なアンケートスケジュールか
        validate_datetime = datetime.strptime(
            survey.plan_survey_request_datetime, "%Y/%m/%d %H:%M"
        )
        if not SchedulesService.is_change_schedule(
            validate_datetime.strftime("%Y/%m"), validate_datetime.strftime("%d")
        ):
            logger.warning("UpdateSurveySchedule bad request.")
            raise HTTPException(status_code=status.HTTP_400_BAD_REQUEST)

        now = get_datetime_now()
        try:
            # yyyy/mm/dd形式
            datetime.strptime(item.send_date, "%Y/%m/%d")
            # 更新したい送信「日」取得
            item_date_split = item.send_date.split("/")
            update_year_month = item_date_split[0] + "/" + item_date_split[1].zfill(2)
            update_day = int(item_date_split[2])
            # 回答期限日
            update_limit_date: str = SchedulesService.get_limit_date(
                item.send_date, item.survey_limit_date
            )
        except ValueError:
            # 回答期限日から○営業日前:-30～-1
            # 以下の組み合わせはschemasのバリデーションで指定不可
            # ・requestDate(アンケート送信日時)の「回答期限日から○営業日前」とlimitDate(回答期限日)の(送信日から)「〇営業日後」の同時指定は不可
            # ・requestDate(アンケート送信日時)の「回答期限日から○営業日前」とlimitDate(回答期限日)の「なし」の同時指定は不可
            try:
                (
                    survey_request_date_list,
                    survey_limit_date_list,
                ) = get_pair_business_date_for_once(
                    current_datetime=now,
                    ref_day=int(item.survey_limit_date),
                    offset_day=int(item.send_date),
                )
                # 更新したい送信「日」取得
                item_date_split = survey_request_date_list[0].split("/")
                update_year_month = (
                    item_date_split[0] + "/" + item_date_split[1].zfill(2)
                )
                update_day = int(item_date_split[2])
                # 回答期限日
                update_limit_date: str = survey_limit_date_list[0]

            except ValueError:
                raise HTTPException(
                    status_code=status.HTTP_400_BAD_REQUEST,
                    detail="You cannot set a send date that is earlier than the current date.",
                )

        # 案件アンケートテーブルの送信予定日、回答予定日時（回答期限） 更新
        # なお、案件アンケートの一括登録時のスケジュールグループID（survey_group_id）はクリア

        # 送信予定日作成
        plan_request_datetime = SchedulesService.make_request_datetime(
            update_year_month + "/" + str(update_day)
        )

        # 回答期限日なしの場合
        if update_limit_date == "":
            survey.update(
                actions=[
                    ProjectSurveyModel.plan_survey_request_datetime.set(
                        plan_request_datetime
                    ),
                    ProjectSurveyModel.summary_month.set(update_year_month),
                    ProjectSurveyModel.plan_survey_response_datetime.remove(),
                    ProjectSurveyModel.survey_group_id.remove(),
                    ProjectSurveyModel.update_at.set(datetime.now()),
                    ProjectSurveyModel.update_id.set(current_user.id),
                ]
            )
        else:
            survey.update(
                actions=[
                    ProjectSurveyModel.plan_survey_request_datetime.set(
                        plan_request_datetime
                    ),
                    ProjectSurveyModel.summary_month.set(update_year_month),
                    ProjectSurveyModel.plan_survey_response_datetime.set(
                        SchedulesService.make_request_datetime(update_limit_date)
                    ),
                    ProjectSurveyModel.survey_group_id.remove(),
                    ProjectSurveyModel.update_at.set(datetime.now()),
                    ProjectSurveyModel.update_id.set(current_user.id),
                ]
            )

        # 案件情報のsurvey_group_idに紐づく案件アンケートが存在しない場合、案件情報のsurvey_group_idを削除
        SchedulesService.check_and_delete_survey_group_id_of_project(
            project_id=survey.project_id, current_user_id=current_user.id
        )

        return PutScheduleIdDateResponse(message="OK")

    @staticmethod
    def delete_survey_schedules_by_id_date(
        query_params: DeleteSurveySchedulesByIdDateQuery,
        current_user: UserModel,
    ) -> DeleteScheduleIdDateResponse:
        """Delete /schedules/survey 「アンケート」スケジュール削除API

        Args:
            survey_id (str): アンケートID クエリパラメータで指定
            version (int): ロックキー（楽観ロック制御） クエリパラメータで指定(>=1)
            current_user (Behavior, optional): 認証済みのユーザー

        Raises:
            HTTPException: 400 Bad Request
            HTTPException: 403 Forbidden
            HTTPException: 404 Not Found
            HTTPException: 409 Conflict

        Returns:
            DeleteScheduleIdDateResponse: 更新結果
        """

        survey_id = query_params.survey_id
        # 案件アンケート取得
        try:
            survey = ProjectSurveyModel.get(
                hash_key=survey_id, range_key=DataType.SURVEY
            )
        except DoesNotExist:
            logger.warning(
                f"DeleteSurveySchedule survey not found. survey_id: {survey_id}"
            )
            raise HTTPException(status_code=status.HTTP_404_NOT_FOUND)

        # 権限チェック
        if not SchedulesService.is_visible_project(current_user, survey.project_id):
            logger.warning(
                "DeleteScheduleIdDate forbidden.  current_user:{current_user.id} project_id: {survey.project_id}"
            )
            raise HTTPException(
                status_code=status.HTTP_403_FORBIDDEN, detail="Forbidden"
            )

        # 排他チェック
        if query_params.version != survey.version:
            logger.warning(
                f"DeleteScheduleIdDate conflict. request_ver:{query_params.version} project_ver: {survey.version}"
            )
            raise HTTPException(status_code=status.HTTP_409_CONFLICT, detail="Conflict")

        # 削除可能なアンケートスケジュールか
        validate_datetime = datetime.strptime(
            survey.plan_survey_request_datetime, "%Y/%m/%d %H:%M"
        )
        if not SchedulesService.is_change_schedule(
            validate_datetime.strftime("%Y/%m"), validate_datetime.strftime("%d")
        ):
            logger.warning("DeleteSurveySchedule bad request.")
            raise HTTPException(status_code=status.HTTP_400_BAD_REQUEST)

        else:
            # 案件アンケートを削除
            survey.delete()

            # 案件情報のsurvey_group_idに紐づく案件アンケートが存在しない場合、案件情報のsurvey_group_idを削除
            SchedulesService.check_and_delete_survey_group_id_of_project(
                project_id=survey.project_id, current_user_id=current_user.id
            )

            return DeleteScheduleIdDateResponse(message="OK")

    @staticmethod
    def get_limit_date(send_date: str, survey_limit_date: str) -> str:
        """送信日と回答期限(0～99, 101～130)をもとに回答期限日yyy/mm/ddを返す
        Args:
            send_date:str 送信日 yyyy/mm/dd
            survey_limit_date:int 0-99, 101-130
        Returns:
            str: 回答期限日 yyyy/mm/dd
        """

        # 更新したい送信「日」取得
        item_date_split = send_date.split("/")

        # 回答期限日
        update_limit_date: str
        # 回答期限日 月末最終営業日
        if survey_limit_date == 99:
            update_limit_date = get_last_business_date_of_month(
                year=int(item_date_split[0]), month=int(item_date_split[1])
            ).strftime("%Y/%m/%d")

        # 回答期限日 なし
        elif survey_limit_date == 0:
            update_limit_date = ""

        # 回答期限日 送信日から○営業日後に指定
        elif 1 <= survey_limit_date <= 30:
            send_date_datetime = datetime.strptime(send_date, "%Y/%m/%d")
            update_limit_date = get_after_business_day(
                send_date_datetime, survey_limit_date
            ).strftime("%Y/%m/%d")

        # 回答期限日 翌月月初○営業日:101～130(営業日に+100した数値)
        else:
            temp_offset: int = survey_limit_date - 101
            update_limit_date = get_first_business_day_of_the_next_month(
                year=int(item_date_split[0]),
                month=int(item_date_split[1]),
                offset=temp_offset,
            ).strftime("%Y/%m/%d")

        return update_limit_date

    @staticmethod
    def bulk_create_surveys(
        survey_list: List[BulkCreateSurveySchedule],
        survey_type: SurveyType,
        group_id: str,
        project: ProjectModel,
        current_user: UserModel,
    ) -> str:
        """アンケートを一括登録する

         Args:
            survey_list List[BulkCreateSurveySchedule]: 登録アンケート情報,
            survey_type SurveyType: アンケートタイプ,
            group_id str: スケジュールgroup_id,
            project ProjectModel: 案件情報,
            current_user AuthUser: 認証済みのユーザー,
            version (int): ロックキー（楽観ロック制御） クエリパラメータで指定(>=1)
            current_user (Behavior, optional): 認証済みのユーザー

        Returns:
            str: 結果
        """

        user_id_name_map: dict = {}
        search_user_id_list: List = []
        if project.main_supporter_user_id is not None:
            search_user_id_list.append(project.main_supporter_user_id)
        if project.main_customer_user_id is not None:
            search_user_id_list.append(project.main_customer_user_id)
        if project.main_sales_user_id is not None:
            search_user_id_list.append(project.main_sales_user_id)
        if project.supporter_user_ids is not None:
            for user_id in project.supporter_user_ids:
                search_user_id_list.append(user_id)
        user_item_keys = [(id, DataType.USER) for id in search_user_id_list]
        for user in UserModel.batch_get(user_item_keys):
            user_id_name_map[user.id] = user.name

        # アンケートテーブルに所属課を登録する
        db_main_supporter_user: SupporterUserAttribute
        if project.main_supporter_user_id is not None:
            main_supporter_user = UserModel.get_user_by_id(
                project.main_supporter_user_id
            )
            main_supporter_user_organization_id: str = ""
            list_supporter_user_organization_name: List = []
            main_supporter_user_organization_name: str = ""

            main_supporter_user_organization_id = ";".join(
                main_supporter_user.supporter_organization_id
            )
            for (
                supporter_organization_id_itr
            ) in main_supporter_user.supporter_organization_id:
                list_supporter_user_organization_name.append(
                    ProjectService.get_supporter_organization_name(
                        supporter_organization_id_itr
                    )
                )
            main_supporter_user_organization_name = ";".join(
                list_supporter_user_organization_name
            )

            db_main_supporter_user = SupporterUserAttribute(
                id=project.main_supporter_user_id,
                name=user_id_name_map.get(project.main_supporter_user_id, ""),
                organization_id=main_supporter_user_organization_id,
                organization_name=main_supporter_user_organization_name,
            )
        else:
            db_main_supporter_user = None

        latest_survey_master: SurveyMasterModel = (
            SurveyMasterModel.get_latest_survey_masters(survey_type)
        )

        service_types: List[dict] = CachedDbItems.ReturnServiceTypes()
        service_type_name = ""
        for service_type_dict in service_types:
            if service_type_dict["id"] == project.service_type:
                service_type_name = service_type_dict["name"]
                break

        # support_users List を保存用に作成 -> ユーザーid,name のみ入れて一括作成する
        support_users_list: List[SupporterUserAttribute] = []
        if project.supporter_user_ids is not None:
            for user_ids_itr in project.supporter_user_ids:
                # 支援者組織
                user_ids_itr_supporter_user = UserModel.get_user_by_id(user_ids_itr)
                user_ids_itr_organization_id: str = ""
                list_ser_ids_itr_organization_name: List = []
                user_ids_itr_organization_name: str = ""

                user_ids_itr_organization_id = ";".join(
                    user_ids_itr_supporter_user.supporter_organization_id
                )
                for (
                    supporter_organization_id_itr
                ) in user_ids_itr_supporter_user.supporter_organization_id:
                    list_ser_ids_itr_organization_name.append(
                        ProjectService.get_supporter_organization_name(
                            supporter_organization_id_itr
                        )
                    )
                user_ids_itr_organization_name = ";".join(
                    list_ser_ids_itr_organization_name
                )

                support_users_list.append(
                    SupporterUserAttribute(
                        id=user_ids_itr,
                        name=user_id_name_map.get(user_ids_itr, ""),
                        organization_id=copy.deepcopy(user_ids_itr_organization_id),
                        organization_name=copy.deepcopy(user_ids_itr_organization_name),
                    )
                )
        else:
            # アクセラレータが登録されていない時はNoneでDB保存処理を行う
            support_users_list = None

        service_manager_id = None
        service_manager_name = None
        if project.supporter_organization_id:
            try:
                filter_condition_master_service_manager = None
                filter_condition_master_service_manager &= (
                    MasterServiceManagerModel.supporter_organization_id
                    == project.supporter_organization_id
                )
                master_service_manager: MasterServiceManagerModel = next(
                    MasterServiceManagerModel.data_type_index.query(
                        hash_key=MasterDataType.SERVICE_MANAGER.value,
                        filter_condition=filter_condition_master_service_manager,
                    )
                )
                service_manager_id = master_service_manager.id
                service_manager_name = master_service_manager.name
            except StopIteration:
                pass

        with ProjectSurveyModel.batch_write() as project_survey_batch:
            for survey_info in survey_list:
                project_survey_model = ProjectSurveyModel(
                    id=survey_info.id,
                    data_type=DataType.SURVEY,
                    survey_master_id=latest_survey_master.id,
                    survey_master_revision=latest_survey_master.revision,
                    survey_type=survey_type,
                    project_id=project.id,
                    project_name=project.name,
                    customer_success=project.customer_success,
                    supporter_organization_id=project.supporter_organization_id,
                    supporter_organization_name=ProjectService.get_supporter_organization_name(
                        project.supporter_organization_id
                    ),
                    support_date_from=project.support_date_from,
                    support_date_to=project.support_date_to,
                    main_supporter_user=db_main_supporter_user,
                    supporter_users=support_users_list,
                    sales_user_id=project.main_sales_user_id,
                    service_type_id=project.service_type,
                    service_type_name=service_type_name,
                    service_manager_id=service_manager_id,
                    service_manager_name=service_manager_name,
                    answer_user_id=project.main_customer_user_id,
                    answer_user_name=user_id_name_map.get(
                        project.main_customer_user_id, ""
                    ),
                    customer_id=project.customer_id,
                    customer_name=project.customer_name,
                    points=PointsAttribute(
                        satisfaction=0,
                        continuation=0,
                        recommended=0,
                        sales=0,
                        survey_satisfaction=0,
                        man_hour_satisfaction=0,
                        karte_satisfaction=0,
                        master_karte_satisfaction=0,
                    ),
                    summary_month=survey_info.survey_request_date.rsplit("/", 1)[0],
                    plan_survey_request_datetime=SchedulesService.make_request_datetime(
                        survey_info.survey_request_date
                    ),
                    plan_survey_response_datetime=SchedulesService.make_request_datetime(
                        survey_info.response_limit_date
                    ),
                    # NPFから取得したもの
                    this_month_type=project.this_month_type,
                    no_send_reason=project.no_send_reason,
                    survey_group_id=group_id,
                    create_id=current_user.id,
                    create_at=datetime.now(),
                    update_at=datetime.now(),
                    version=1,
                )
                project_survey_batch.save(project_survey_model)

        return "OK"

    @staticmethod
    def make_request_datetime(request_date_str: str) -> str:
        """request_datetime(yyyy/mm/dd 9:00)作成 ProjectSurveyテーブル保存用

        Args:
            request_date_str(str): yyy/mm/dd
        Returns:
            str: yyyy/mm/dd 9:00
        """
        if request_date_str is None or len(request_date_str) == 0:
            # 回答期限なしをDBに登録するため、Noneを作成
            return None
        else:
            datetime_request = datetime.strptime(request_date_str, "%Y/%m/%d")
            datetime_request = datetime_request.replace(hour=9, minute=0, second=0)

            return datetime_request.strftime("%Y/%m/%d %H:%M")

    @staticmethod
    def bulk_create_survey_schedules(
        item: BulkCreateSurveyScheduleRequest,
        project_id: str,
        current_user: UserModel,
    ) -> BulkCreateSurveyScheduleResponse:
        """Post /schedules/survey/bulk/{project_id} アンケートスケジュール一括登録API

        Args:
            body (BulkCreateSurveyScheduleRequest): 一括登録内容
            project_id (str): 案件ID パスパラメータで指定
            current_user (Behavior, optional): 認証済みのユーザー

        Raises:
            HTTPException: 403 Forbidden
            HTTPException: 404 Not Found

        Returns:
            BulkCreateSurveyScheduleResponse: 更新結果
        """

        # 権限チェック
        if not SchedulesService.is_visible_project(current_user, project_id):
            logger.info("BulkCreateSurveySchedule forbidden.")
            raise HTTPException(
                status_code=status.HTTP_403_FORBIDDEN, detail="Forbidden"
            )

        try:
            project = ProjectModel.get_project(project_id=project_id)
        except DoesNotExist:
            logger.info(f"BulkCreateSurveySchedule not found. project_id: {project_id}")
            raise HTTPException(status_code=status.HTTP_404_NOT_FOUND)

        # 支援期間終了月にサービスアンケートは作成しない
        support_date_to_datetime = datetime.strptime(
            project.support_date_to, "%Y/%m/%d"
        )
        # 1ヵ月前を取得
        support_end_before_month = support_date_to_datetime - relativedelta(months=1)
        service_survey_end = get_last_date_of_month(
            support_end_before_month.year, support_end_before_month.month
        )

        now = get_datetime_now()
        # 開催日時の判定（既に支援開始されてる場合は、現在日時から計算）
        if now.strftime("%Y/%m/%d") > project.support_date_from:
            create_start_date = now
        else:
            create_start_date = datetime.strptime(project.support_date_from, "%Y/%m/%d")

        # サービスアンケート
        service_survey_limit_date_list = []
        if -30 <= int(item.service.request_date) <= -1:
            # 回答期限日から○営業日前:-30～-1
            # 以下の組み合わせはschemasのバリデーションで指定不可
            # ・requestDate(アンケート送信日時)の「回答期限日から○営業日前」とlimitDate(回答期限日)の(送信日から)「〇営業日後」の同時指定は不可
            # ・requestDate(アンケート送信日時)の「回答期限日から○営業日前」とlimitDate(回答期限日)の「なし」の同時指定は不可
            # service_survey_endがdate型のため、datetime型に変換して渡す
            (
                service_survey_date_list,
                service_survey_limit_date_list,
            ) = get_continuous_business_date_for_each_month(
                start_date=create_start_date,
                end_date=datetime(
                    year=service_survey_end.year,
                    month=service_survey_end.month,
                    day=service_survey_end.day,
                ),
                ref_day=item.service.limit_date,
                current_datetime=now,
                offset_day=item.service.request_date,
            )
        else:
            # 毎月○日:1-31
            service_survey_date_list = get_continuous_date_for_each_month(
                create_start_date,
                service_survey_end,
                item.service.request_date,
            )

        service_survey_list: List[BulkCreateSurveySchedule] = []
        if service_survey_limit_date_list:
            for date_item, limit_date_item in zip(
                service_survey_date_list, service_survey_limit_date_list
            ):
                service_survey_list.append(
                    BulkCreateSurveySchedule(
                        id=str(uuid.uuid4()),
                        survey_request_date=date_item,
                        response_limit_date=limit_date_item,
                        type=SurveyType.SERVICE,
                    )
                )
        else:
            for service_survey_date_itr in service_survey_date_list:
                limit_date = SchedulesService.get_limit_date(
                    service_survey_date_itr, item.service.limit_date
                )
                service_survey_list.append(
                    BulkCreateSurveySchedule(
                        id=str(uuid.uuid4()),
                        survey_request_date=service_survey_date_itr,
                        response_limit_date=limit_date,
                        type=SurveyType.SERVICE,
                    )
                )

        # サービス・修了アンケート共通
        group_id = str(uuid.uuid4())

        SchedulesService.bulk_create_surveys(
            service_survey_list,
            SurveyType.SERVICE,
            group_id,
            project,
            current_user,
        )

        # 修了アンケート
        limit_date = ""
        completion_survey_list: List[BulkCreateSurveySchedule] = []
        split_to_date = project.support_date_to.split("/")
        if -30 <= int(item.completion.request_date) <= -1:
            # 回答期限日から○営業日前:-30～-1の場合
            # 期限日の算出
            ref_datetime: datetime = None
            if item.completion.limit_date == 99:
                # 月末最終営業日
                ref_datetime = get_last_business_date_of_month(
                    year=int(split_to_date[0]), month=int(split_to_date[1])
                )
            else:
                # 翌月月初○営業日: 101～130
                # 月初1営業日はoffsetを0とするため、101を減算
                temp_offset: int = item.completion.limit_date - 101
                ref_datetime = get_first_business_day_of_the_next_month(
                    year=int(split_to_date[0]),
                    month=int(split_to_date[1]),
                    offset=temp_offset,
                )
            limit_date = ref_datetime.strftime("%Y/%m/%d")

            # 送信日の算出
            if item.completion.request_date < 0:
                completion_request_date = get_before_business_day(
                    ref_datetime, item.completion.request_date * -1
                ).strftime("%Y/%m/%d")
            else:
                completion_request_date = get_after_business_day(
                    ref_datetime, item.completion.request_date
                ).strftime("%Y/%m/%d")

        else:
            # 毎月○日:1-31の場合
            completion_request_date = get_completion_request_date(
                int(split_to_date[0]),
                int(split_to_date[1]),
                item.completion.request_date,
            ).strftime("%Y/%m/%d")

        # 現在日より過去のアンケートスケジュールは作成しない
        if not (now.strftime("%Y/%m/%d") > completion_request_date):
            if not limit_date:
                limit_date = SchedulesService.get_limit_date(
                    completion_request_date, item.completion.limit_date
                )

            completion_survey_list.append(
                BulkCreateSurveySchedule(
                    id=str(uuid.uuid4()),
                    survey_request_date=completion_request_date,
                    response_limit_date=limit_date,
                    type=SurveyType.COMPLETION,
                )
            )

            SchedulesService.bulk_create_surveys(
                completion_survey_list,
                SurveyType.COMPLETION,
                group_id,
                project,
                current_user,
            )

            # 案件テーブルにスケジュールグループid登録
            project.update(
                actions=[
                    ProjectModel.update_id.set(current_user.id),
                    ProjectModel.update_at.set(datetime.now()),
                    ProjectModel.survey_group_id.set(group_id),
                ]
            )

        return BulkCreateSurveyScheduleResponse(message="OK")

    @staticmethod
    def is_change_schedule(year_month: str, day: str) -> bool:
        """削除・更新が可能なアンケートスケジュールか

            削除・更新不可 送信日＜=現在日 送信日が現在日時よりも過去
            削除・更新可   送信日＞現在日  送信日が現在日時よりも未来
        Args:
            year_month:str 送信予定年月 yyyy/mm
            day:str 送信日
        Returns:
            true: 削除・変更可能
            false: 削除・変更不可能
        """

        target_date_str = year_month + ("/" + day.zfill(2))
        # 比較用にdatetime型へ
        today = get_datetime_now()
        target_date = datetime.strptime(target_date_str, "%Y/%m/%d")
        # time zone設定
        time_zone_info = timezone("Asia/Tokyo")
        target_date = target_date.replace(tzinfo=time_zone_info)

        return target_date.date() > today.date()

    @staticmethod
    def bulk_update_survey_schedules(
        item: BulkUpdateSurveyScheduleRequest,
        project_id: str,
        current_user: UserModel,
    ) -> BulkUpdateSurveyScheduleResponse:
        """Put /schedules/survey/bulk/{project_id} アンケートスケジュール一括更新API

        Args:
            body (BulkUpdateSurveyScheduleRequest): 一括更新内容
            project_id (str): 案件ID パスパラメータで指定
            current_user (Behavior, optional): 認証済みのユーザー

        Raises:
            HTTPException: 400 Bad Request
            HTTPException: 403 Forbidden
            HTTPException: 404 Not Found

        Returns:
            BulkUpdateSurveyScheduleResponse: 更新結果
        """

        # 権限チェック
        if not SchedulesService.is_visible_project(current_user, project_id):
            logger.info("BulkUpdateSurveySchedule forbidden")
            raise HTTPException(
                status_code=status.HTTP_403_FORBIDDEN, detail="Forbidden"
            )

        try:
            project = ProjectModel.get_project(project_id=project_id)
        except DoesNotExist:
            logger.info(f"BulkUpdateSurveySchedule not found. project_id: {project_id}")
            raise HTTPException(status_code=status.HTTP_404_NOT_FOUND)

        # project_id にグループidが保存されていない
        if (project.survey_group_id is None) or (not project.survey_group_id):
            logger.info(
                "BulkUpdateSurveySchedule bad request. The project doesn't have survey_group_id."
            )
            raise HTTPException(status_code=status.HTTP_400_BAD_REQUEST)

        # 案件アンケート取得
        # survey_group_idを条件に取得する
        filter_condition = None
        filter_condition &= (
            ProjectSurveyModel.survey_group_id == project.survey_group_id
        )
        survey_list: List[ProjectSurveyModel] = list(
            ProjectSurveyModel.project_id_summary_month_index.query(
                hash_key=project_id, filter_condition=filter_condition
            )
        )
        for survey in survey_list:
            # 更新可能なアンケートスケジュールか
            validate_datetime = datetime.strptime(
                survey.plan_survey_request_datetime, "%Y/%m/%d %H:%M"
            )
            if SchedulesService.is_change_schedule(
                validate_datetime.strftime("%Y/%m"),
                validate_datetime.strftime("%d"),
            ):
                year_month = validate_datetime.strftime("%Y/%m")
                split_to_date = year_month.split("/")
                if survey.survey_type == SurveyType.SERVICE:
                    if -30 <= int(item.service.request_date) <= -1:
                        # 回答期限日から○営業日前:-30～-1
                        try:
                            (
                                service_survey_request_date_list,
                                service_survey_limit_date_list,
                            ) = get_pair_business_date_for_once(
                                current_datetime=datetime(
                                    year=int(split_to_date[0]),
                                    month=int(split_to_date[1]),
                                    day=1,
                                ),
                                ref_day=int(item.service.limit_date),
                                offset_day=int(item.service.request_date),
                            )
                        except ValueError:
                            continue
                        request_date = (
                            service_survey_request_date_list[0]
                            if service_survey_request_date_list
                            else ""
                        )
                        limit_date = (
                            service_survey_limit_date_list[0]
                            if service_survey_limit_date_list
                            else ""
                        )
                    else:
                        # 毎月○日:1-31
                        request_date = get_completion_request_date(
                            int(split_to_date[0]),
                            int(split_to_date[1]),
                            item.service.request_date,
                        ).strftime("%Y/%m/%d")
                        limit_date = SchedulesService.get_limit_date(
                            request_date,
                            item.service.limit_date,
                        )
                elif survey.survey_type == SurveyType.COMPLETION:
                    if -30 <= int(item.completion.request_date) <= -1:
                        # 回答期限日から○営業日前:-30～-1
                        try:
                            (
                                completion_survey_request_date_list,
                                completion_survey_limit_date_list,
                            ) = get_pair_business_date_for_once(
                                current_datetime=datetime(
                                    year=int(split_to_date[0]),
                                    month=int(split_to_date[1]),
                                    day=1,
                                ),
                                ref_day=int(item.completion.limit_date),
                                offset_day=int(item.completion.request_date),
                            )
                        except ValueError:
                            continue
                        request_date = (
                            completion_survey_request_date_list[0]
                            if completion_survey_request_date_list
                            else ""
                        )
                        limit_date = (
                            completion_survey_limit_date_list[0]
                            if completion_survey_limit_date_list
                            else ""
                        )
                    else:
                        # 毎月○日:1-31
                        request_date = get_completion_request_date(
                            int(split_to_date[0]),
                            int(split_to_date[1]),
                            item.completion.request_date,
                        ).strftime("%Y/%m/%d")
                        limit_date = SchedulesService.get_limit_date(
                            request_date,
                            item.completion.limit_date,
                        )

                # アンケート更新
                item_date_split = request_date.split("/")
                update_year_month = (
                    item_date_split[0] + "/" + item_date_split[1].zfill(2)
                )
                plan_request_datetime = SchedulesService.make_request_datetime(
                    request_date
                )
                if limit_date != "":
                    plan_limit_datetime = SchedulesService.make_request_datetime(
                        limit_date
                    )
                else:
                    plan_limit_datetime = ""
                # 回答期限日なし -> アンケートテーブルでremove()処理をする
                if plan_limit_datetime != "":
                    survey.update(
                        actions=[
                            ProjectSurveyModel.summary_month.set(update_year_month),
                            ProjectSurveyModel.plan_survey_request_datetime.set(
                                plan_request_datetime
                            ),
                            ProjectSurveyModel.plan_survey_response_datetime.set(
                                plan_limit_datetime
                            ),
                        ]
                    )
                else:
                    survey.update(
                        actions=[
                            ProjectSurveyModel.summary_month.set(update_year_month),
                            ProjectSurveyModel.plan_survey_request_datetime.set(
                                plan_request_datetime
                            ),
                            ProjectSurveyModel.plan_survey_response_datetime.remove(),
                        ]
                    )

        return BulkUpdateSurveyScheduleResponse(message="OK")

    @staticmethod
    def get_survey_schedules_during_period(
        survey_type: SurveyType,
        support_start_date: str,
        support_end_date: str,
        request_date: str,
        timing: ScheduleTiming,
        limit_date: int,
    ) -> List[SurveySchedule]:
        """アンケートの日時情報を取得します。

        Args:
            survey_type (SurveyType): アンケート種別
            support_start_date (str): 支援開始日時
            support_end_date (str): 支援終了日時
            request_date (Any): アンケート送信日時
            timing (ScheduleTiming): タイミング種別
            limit_date (int): 回答期限日 0: なし, 1~31: ○日後, 99: 月末最終日, 101～130: 翌月月初○営業日(営業日に+100した数値)

        Returns:
            _type_: _description_
        """
        now = get_datetime_now()
        # 開催日時の判定（既に支援開始されてる場合は、現在日時から計算）
        if now.strftime("%Y/%m/%d") > support_start_date:
            start_date = now
        else:
            start_date = datetime.strptime(support_start_date, "%Y/%m/%d")

        survey_limit_date_list = []
        # クイックアンケートのみ週・月単位で指定が可能（複数スケジュール）、サービス・修了に関しては1回のスケジュールのみ登録が可能
        if survey_type == SurveyType.QUICK:
            if timing == ScheduleTiming.MONTHLY:
                if -30 <= int(request_date) <= -1:
                    # 回答期限日から○営業日前:-30～-1
                    # 以下の組み合わせはschemasのバリデーションで指定不可
                    # ・requestDate(アンケート送信日時)の「回答期限日から○営業日前」とlimitDate(回答期限日)の(送信日から)「〇営業日後」の同時指定は不可
                    # ・requestDate(アンケート送信日時)の「回答期限日から○営業日前」とlimitDate(回答期限日)の「なし」の同時指定は不可
                    (
                        survey_request_date_list,
                        survey_limit_date_list,
                    ) = get_continuous_business_date_for_each_month(
                        start_date=start_date,
                        end_date=datetime.strptime(support_end_date, "%Y/%m/%d"),
                        ref_day=int(limit_date),
                        current_datetime=now,
                        offset_day=int(request_date),
                    )
                else:
                    # 毎月○日:1-31
                    survey_request_date_list = get_continuous_date_for_each_month(
                        start_date=start_date,
                        end_date=datetime.strptime(support_end_date, "%Y/%m/%d"),
                        offset_day=int(request_date),
                    )
            elif timing == ScheduleTiming.WEEKLY:
                survey_request_date_list = get_continuous_date_for_each_week(
                    start_date=start_date,
                    end_date=datetime.strptime(support_end_date, "%Y/%m/%d"),
                    day_of_week_index=int(request_date),
                )
            else:
                try:
                    # yyyy/mm/dd形式
                    datetime.strptime(request_date, "%Y/%m/%d")
                    survey_request_date_list = [request_date]
                except ValueError:
                    # 回答期限日から○営業日前:-30～-1
                    # 以下の組み合わせはschemasのバリデーションで指定不可
                    # ・requestDate(アンケート送信日時)の「回答期限日から○営業日前」とlimitDate(回答期限日)の(送信日から)「〇営業日後」の同時指定は不可
                    # ・requestDate(アンケート送信日時)の「回答期限日から○営業日前」とlimitDate(回答期限日)の「なし」の同時指定は不可
                    try:
                        (
                            survey_request_date_list,
                            survey_limit_date_list,
                        ) = get_pair_business_date_for_once(
                            current_datetime=now,
                            ref_day=int(limit_date),
                            offset_day=int(request_date),
                        )
                    except ValueError:
                        raise HTTPException(
                            status_code=status.HTTP_400_BAD_REQUEST,
                            detail="You cannot set a send date that is earlier than the current date.",
                        )
        else:
            try:
                # yyyy/mm/dd形式
                datetime.strptime(request_date, "%Y/%m/%d")
                survey_request_date_list = [request_date]
            except ValueError:
                # 回答期限日から○営業日前:-30～-1
                # 以下の組み合わせはschemasのバリデーションで指定不可
                # ・requestDate(アンケート送信日時)の「回答期限日から○営業日前」とlimitDate(回答期限日)の(送信日から)「〇営業日後」の同時指定は不可
                # ・requestDate(アンケート送信日時)の「回答期限日から○営業日前」とlimitDate(回答期限日)の「なし」の同時指定は不可
                try:
                    (
                        survey_request_date_list,
                        survey_limit_date_list,
                    ) = get_pair_business_date_for_once(
                        current_datetime=now,
                        ref_day=int(limit_date),
                        offset_day=int(request_date),
                    )
                except ValueError:
                    raise HTTPException(
                        status_code=status.HTTP_400_BAD_REQUEST,
                        detail="You cannot set a send date that is earlier than the current date.",
                    )

        if survey_limit_date_list:
            # 回答期限日算出済みの場合
            ret_list = []
            for req_item, limit_item in zip(
                survey_request_date_list, survey_limit_date_list
            ):
                ret_list.append(
                    SurveySchedule(
                        survey_request_date=req_item,
                        response_limit_date=limit_item,
                    )
                )
            return ret_list

        return SchedulesService.get_survey_information_date(
            survey_request_date=survey_request_date_list,
            limit_day=int(limit_date),
        )

    @staticmethod
    def get_survey_information_date(
        survey_request_date: List,
        limit_day: int = None,
    ) -> List[SurveySchedule]:
        """アンケートの日付け情報を取得します

        アンケート送信日からリクエストの内容に基づいて回答期限日を取得する

        Args:
            survey_request_date (List): アンケート送信日
            limit_date (int): アンケート送信日 0: なし、1~30: ○日後, 99: 月末最終日

        Returns:
            List[Tuple[str, str]]: _description_
        """
        survey_date_info: List[Tuple[str, str]] = []
        for request_date in survey_request_date:
            limit_date = None
            request_date = datetime.strptime(request_date, "%Y/%m/%d")
            if limit_day == int(SurveyResponseDate.MONTH_END):
                # 月末最終営業日
                limit_date = get_last_business_date_of_month(
                    year=request_date.year, month=request_date.month
                ).strftime("%Y/%m/%d")
            elif 1 <= limit_day <= 30:
                # 送信日から○営業日後に指定
                limit_date = get_after_business_day(request_date, limit_day).strftime(
                    "%Y/%m/%d"
                )
            elif 101 <= limit_day <= 130:
                # 翌月月初○営業日:101～130(営業日に+100した数値)
                temp_offset: int = limit_day - 101
                limit_date = get_first_business_day_of_the_next_month(
                    year=request_date.year,
                    month=request_date.month,
                    offset=temp_offset,
                ).strftime("%Y/%m/%d")

            survey_date_info.append(
                SurveySchedule(
                    survey_request_date=request_date.strftime("%Y/%m/%d"),
                    response_limit_date=limit_date,
                )
            )

        return survey_date_info

    @staticmethod
    def check_and_delete_survey_group_id_of_project(
        project_id: str, current_user_id: str
    ) -> None:
        """案件情報のsurvey_group_idに紐づく案件アンケートが存在しない場合、案件情報のsurvey_group_idを削除.

        Args:
            project_id (str): 案件ID
            current_user_id (str): ユーザID

        Raises:
            HTTPException: 404 Not Found
        """
        # 案件情報の取得
        try:
            project: ProjectModel = ProjectModel.get(
                hash_key=project_id, range_key=DataType.PROJECT
            )
        except DoesNotExist:
            logger.warning(
                f"check_and_delete_survey_group_id_of_project project not found. project_id: {project_id}"
            )
            raise HTTPException(status_code=status.HTTP_404_NOT_FOUND)

        if project.survey_group_id:
            # 案件に紐づく案件アンケートの survey_group_id 存在チェック
            filter_condition = None
            filter_condition &= (
                ProjectSurveyModel.survey_group_id == project.survey_group_id
            )
            survey_list: List[ProjectSurveyModel] = list(
                ProjectSurveyModel.project_id_summary_month_index.query(
                    hash_key=project_id, filter_condition=filter_condition
                )
            )

            # 案件アンケートにsurvey_group_idが存在しない場合
            if not survey_list:
                # 案件の survey_group_id を削除
                project.update(
                    actions=[
                        ProjectModel.survey_group_id.remove(),
                        ProjectModel.update_at.set(datetime.now()),
                        ProjectModel.update_id.set(current_user_id),
                    ]
                )
