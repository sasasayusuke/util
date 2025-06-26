import copy
import json

from typing import List, Tuple
from app.core.common_logging import CustomLogger
from app.core.config import get_app_settings
from app.models.admin import AdminModel
from app.models.customer import CustomerModel
from app.models.project import ProjectModel
from app.models.project_survey import ProjectSurveyModel
from app.models.survey_master import SurveyMasterModel
from app.models.user import UserModel
from app.resources.const import (
    DataType,
    FoAppUrl,
    MasterKartenCustomerCategory,
    MasterKartenProgramType,
    MasterKartenUpdateUserType,
    MailType,
    SurveyType,
    UserRoleType,
    SurveyQuestionsSummaryType,
)
from app.schemas.master import GetNpfProjectIdResponse
from app.schemas.master_karten import (
    CompanyDepartment,
    CurrentProgram,
    FundamentalInformation,
    GetMasterKartenByIdResponse,
    GetMasterKartenResponse,
    GetMasterKartenSelectBoxResponse,
    MasterKartenInfoForGetMasterKarten,
    NextProgram,
    Others,
    PostMasterKartenRequest,
    PostMasterKartenResponse,
    PutMasterKartenByIdRequest,
    PutMasterKartenByIdResponse,
    Result,
    SatisfactionEvaluation,
    SelectBoxItem,
    UsageHistory,
)
from app.schemas.platform import (
    CreateCurrentProgram,
    CreateNextProgram,
    UpdateCurrentProgram,
    UpdateNextProgram,
)
from app.service.karten_service import KartenService
from app.utils.format import conv_blank
from app.utils.platform import PlatformApiOperator
from app.utils.aws.sqs import SqsHelper
from fastapi import HTTPException, status
from fastapi.security import HTTPAuthorizationCredentials
from pynamodb.exceptions import DoesNotExist
from pynamodb.expressions.condition import size
import re

logger = CustomLogger.get_logger()


class MasterKartenService:
    @staticmethod
    def get_master_karten(
        offset_page: int,
        limit: int,
        customer_id: str,
        support_date_from: str,
        support_date_to: str,
        is_current_program: bool,
        category: list,
        industry_segment: list,
        department_name: str,
        current_situation: str,
        issue: str,
        customer_success: str,
        lineup: list,
        required_personal_skill: str,
        required_partner: str,
        strength: str,
        all: bool,
        current_user: UserModel,
        authorization: HTTPAuthorizationCredentials,
    ) -> GetMasterKartenResponse:
        """マスターカルテを検索し、マスターカルテ一覧を取得する

        Args:
            offsetPage（int）:リストの中で何ページ目を取得するか
            limit（int）:最大取得件数
            customerId（str）:お客様(取引先)名
            supportDateFrom（str）:支援開始日
            supportDateTo（str）:支援終了日
            isCurrentProgram（bool）:当期支援か次期支援か。
            customerSegment（list）:顧客セグメント
            industrySegment（list）:業界セグメント
            departmentName（str）:部署名
            currentSituation（str）:現状
            issue（str）:課題
            customerSuccess（str）:カスタマーサクセス
            lineup（list）:ラインナップ
            requiredPersonalSkill（str）:お客様に不足している人的リソース
            requiredPartner（str）:お客様に紹介したい企業や産業
            strength（str）:自社の強み
            all（bool）:全案件かどうか
            current_user (Behavior, optional): 認証済みのユーザー
            authorization (HTTPAuthorizationCredentials): トークン情報

        Returns:
            GetMasterKartenResponse: 取得結果
        """
        platform_api_operator = PlatformApiOperator(jwt_token=authorization.jwt_token)

        pf_params = {}
        salesforce_opportunity_id_list = []
        pp_project_id_list = []

        if current_user.role == UserRoleType.CUSTOMER.key:
            # お客様の場合は参加案件のみアクセス可
            if current_user.project_ids:
                for project_id in current_user.project_ids:
                    project = ProjectModel.get(
                        hash_key=project_id, range_key=DataType.PROJECT
                    )
                    if project.salesforce_opportunity_id:
                        salesforce_opportunity_id_list.append(
                            project.salesforce_opportunity_id
                        )
                    else:
                        pp_project_id_list.append(project.id)
            else:
                # どの案件にも所属しておらずアクセス可能な案件がないためPFのAPI実行前にレスポンス
                return GetMasterKartenResponse(
                    offset_page=offset_page, total=0, karten=[]
                )
        elif current_user.role in [UserRoleType.SALES.key, UserRoleType.SUPPORTER.key]:
            if all:
                if current_user.project_ids:
                    # 営業担当者の場合,「非所属非公開案件」以外アクセス可能
                    project_list = list(
                        ProjectModel.data_type_name_index.query(
                            hash_key=DataType.PROJECT,
                            filter_condition=MasterKartenService.split_project_ids_for_is_in_filter_condition(
                                current_user.project_ids
                            ),
                        )
                    )

                    for project in project_list:
                        # SF商談IDが存在しない場合は案件IDを追加
                        if project.salesforce_opportunity_id:
                            salesforce_opportunity_id_list.append(
                                project.salesforce_opportunity_id
                            )
                        else:
                            pp_project_id_list.append(project.id)
                # 公開案件
                pf_params["publishableProject"] = True
            else:
                if current_user.project_ids:
                    for project_id in current_user.project_ids:
                        project = ProjectModel.get(
                            hash_key=project_id, range_key=DataType.PROJECT
                        )
                        if project.salesforce_opportunity_id:
                            salesforce_opportunity_id_list.append(
                                project.salesforce_opportunity_id
                            )
                        else:
                            pp_project_id_list.append(project.id)
                else:
                    # どの案件にも所属しておらずアクセス可能な案件がないためPFのAPI実行前にレスポンス
                    return GetMasterKartenResponse(
                        offset_page=offset_page, total=0, karten=[]
                    )
        elif current_user.role in [UserRoleType.SALES_MGR.key, UserRoleType.BUSINESS_MGR.key, UserRoleType.SUPPORTER_MGR.key]:
            if not all:
                if current_user.project_ids:
                    for project_id in current_user.project_ids:
                        project = ProjectModel.get(
                            hash_key=project_id, range_key=DataType.PROJECT
                        )
                        if project.salesforce_opportunity_id:
                            salesforce_opportunity_id_list.append(
                                project.salesforce_opportunity_id
                            )
                        else:
                            pp_project_id_list.append(project.id)
                else:
                    # どの案件にも所属しておらずアクセス可能な案件がないためPFのAPI実行前にレスポンス
                    return GetMasterKartenResponse(
                        offset_page=offset_page, total=0, karten=[]
                    )

        if len(salesforce_opportunity_id_list) != 0:
            # SF商談ID
            pf_params[
                "salesforceOpportunityId"
            ] = MasterKartenService.make_pf_request_param(
                salesforce_opportunity_id_list
            )
        if len(pp_project_id_list) != 0:
            # PP案件ID
            pf_params[
                "partnerPortalProjectId"
            ] = MasterKartenService.make_pf_request_param(pp_project_id_list)

        # お客様IDが指定されている場合、取引先IDからSF取引先IDを取得
        if customer_id:
            result = CustomerModel.get(
                hash_key=customer_id, range_key=DataType.CUSTOMER
            )
            if result.salesforce_customer_id:
                pf_params["salesforceCustomerId"] = result.salesforce_customer_id
            else:
                # PP独自顧客
                pf_params["partnerPortalCustomerId"] = result.id

        # 当期支援か次期支援か
        if is_current_program:
            pf_params["targetProgram"] = MasterKartenProgramType.CURRENT
        else:
            pf_params["targetProgram"] = MasterKartenProgramType.NEXT
        # 支援開始日
        if support_date_from:
            fromDate = support_date_from.replace("/", "")
            pf_params["fromDate"] = fromDate
        # 支援終了日
        if support_date_to:
            toDate = support_date_to.replace("/", "")
            pf_params["toDate"] = toDate
        # 部署名
        if department_name:
            pf_params["departmentName"] = department_name.replace("'", "")
        # 顧客セグメント
        if category:
            pf_params["category"] = MasterKartenService.make_pf_request_param(
                category
            )
        # 業界セグメント
        if industry_segment:
            pf_params["industrySegment"] = MasterKartenService.make_pf_request_param(
                industry_segment
            )
        # お客様に不足している人的リソース
        if required_personal_skill:
            pf_params["needSkill"] = required_personal_skill.replace("'", "")
        # お客様に紹介したい企業や産業
        if required_partner:
            pf_params["needPartner"] = required_partner.replace("'", "")
        # ラインナップ
        if lineup:
            pf_params["lineup"] = MasterKartenService.make_pf_request_param(lineup)
        # 案件の現状
        if current_situation:
            pf_params["currentState"] = current_situation.replace("'", "")
        # 案件の課題
        if issue:
            pf_params["issue"] = issue.replace("'", "")
        # カスタマーサクセス
        if customer_success:
            pf_params["customerSuccess"] = customer_success.replace("'", "")
        # 自社の強み
        if strength:
            pf_params["ourStrength"] = strength.replace("'", "")
        # リストの中で何ページ目を取得するか
        if offset_page:
            pf_params["offsetPage"] = offset_page
        # 最大取得件数
        if limit:
            pf_params["limit"] = limit

        # PPに案件として取り込まれているものを指定
        pf_params["csvImported"] = True

        # PF案件一覧取得APIの呼び出し
        status_code, pf_projects = platform_api_operator.get_projects(params=pf_params)
        logger.info(f"platform getProjects statusCode: {status_code}")
        if status_code != 200:
            logger.info(
                "platform getProjects response:" + json.dumps(pf_projects)
            )
            raise HTTPException(
                status_code=status_code,
                detail=json.dumps(pf_projects),
            )

        karten_list: List[MasterKartenInfoForGetMasterKarten] = []

        for pf_project_item in pf_projects["projects"]:
            pp_project: ProjectModel = None
            if pf_project_item["project"]["salesforceOpportunityId"]:
                # SF商談IDが一致するPP案件情報を取得
                result = list(
                    ProjectModel.data_type_name_index.query(
                        hash_key=DataType.PROJECT,
                        filter_condition=ProjectModel.salesforce_opportunity_id
                        == pf_project_item["project"]["salesforceOpportunityId"],
                    )
                )
                for project_info in result:
                    pp_project = project_info
            else:
                # SF商談IDがない場合はPP案件IDを利用して、PP案件情報を取得
                pp_project = ProjectModel.get(
                    hash_key=pf_project_item["project"]["partnerPortalProjectId"],
                    range_key=DataType.PROJECT,
                )

            # カルテ詳細へのアクセス権があるか
            if pp_project:
                is_accessible_karten = KartenService.is_visible_karte(
                    current_user, pp_project.id
                )
            else:
                is_accessible_karten = False

            # マスターカルテ詳細へのアクセス権があるか
            if pp_project:
                is_accessible_master_karten = MasterKartenService.is_accessible_master_karten_detail(
                    current_user, pp_project
                )
            else:
                is_accessible_master_karten = False

            if pp_project:
                # レスポンス編集
                karten_list.append(
                    MasterKartenInfoForGetMasterKarten(
                        npf_project_id=pf_project_item["project"]["id"],
                        pp_project_id=pp_project.id,
                        service_name=pf_project_item["project"]["serviceType"],
                        project=pp_project.name,
                        client=pp_project.customer_name,
                        support_date_from=pp_project.support_date_from,
                        support_date_to=pp_project.support_date_to,
                        is_accessible_karten=is_accessible_karten,
                        is_accessible_master_karten=is_accessible_master_karten,
                    )
                )

        return GetMasterKartenResponse(
            offset_page=pf_projects["offsetPage"],
            total=pf_projects["total"],
            karten=karten_list,
        )

    @staticmethod
    def get_master_karten_by_id(
        current_user: UserModel,
        npf_project_id: str,
        authorization: HTTPAuthorizationCredentials,
    ):
        """マスターカルテの詳細を取得します。

        Args:
            current_user (UserModel): API実行ユーザー
            npf_project_id (str): PFの案件ID
            authorization (HTTPAuthorizationCredentials): トークン情報

        Returns:
            GetMasterKartenByIdResponse: マスターカルテ詳細
        """
        platform_api_operator = PlatformApiOperator(jwt_token=authorization.jwt_token)

        # PF案件詳細情報取得APIの呼び出し
        status_code, pf_response = platform_api_operator.get_project_by_pf_id(
            npf_project_id
        )
        logger.info(f"platform getProjectByPfId statusCode: {status_code}")
        if status_code != 200:
            logger.info("platform getProjectByPfId response:" + json.dumps(pf_response))
            raise HTTPException(
                status_code=status_code,
                detail=pf_response["detail"],
            )
        # 細かな権限チェックのためNPFのデータからPPの案件情報を取得
        if pf_response.get("project", {}).get("partnerPortalProjectId"):
            # 通常案件はPP案件IDが取れないのでSF商談IDでPPの案件テーブルからデータを取得
            try:
                pp_project = ProjectModel.get(
                    hash_key=str(pf_response["project"]["partnerPortalProjectId"]),
                    range_key=DataType.PROJECT,
                )
            except DoesNotExist:
                raise HTTPException(
                    status_code=status.HTTP_404_NOT_FOUND, detail="Not Found"
                )
        else:
            try:
                pp_project = next(
                    ProjectModel.data_type_name_index.query(
                        hash_key=DataType.PROJECT,
                        filter_condition=ProjectModel.salesforce_opportunity_id
                        == pf_response["project"]["salesforceOpportunityId"],
                    )
                )
            except StopIteration:
                raise HTTPException(
                    status_code=status.HTTP_404_NOT_FOUND, detail="Not Found"
                )

        if not MasterKartenService.is_accessible_master_karten_detail(
            current_user, pp_project
        ):
            raise HTTPException(
                status_code=status.HTTP_403_FORBIDDEN, detail="Forbidden"
            )

        # プログラム利用履歴の取得
        usage_histories = MasterKartenService.get_usage_histories(
            pf_project_detail=pf_response,
            platform_api_operator=platform_api_operator,
            current_user=current_user,
        )

        response = GetMasterKartenByIdResponse(
            npf_project_id=pf_response["project"]["id"],
            pp_project_id=pp_project.id,
            supporter_organization_id=pp_project.supporter_organization_id,
            service=pf_response["project"]["serviceType"],
            project=pp_project.name,
            client=pp_project.customer_name,
            support_date_from=pf_response["project"]["supportDateFrom"],
            support_date_to=pf_response["project"]["supportDateTo"],
            current_program=MasterKartenService.get_current_program(
                pf_project_detail=pf_response,
                pp_project_model=pp_project,
                usage_histories=usage_histories,
                current_user=current_user,
            ),
            next_program=MasterKartenService.get_next_program(
                pf_project_detail=pf_response,
                usage_histories=usage_histories,
                current_user=current_user,
            ),
        )
        return response

    @staticmethod
    def is_accessible_master_karten_detail(
        current_user: UserModel, project: ProjectModel
    ) -> bool:
        """マスターカルテの詳細がアクセス可能かどうかを判定します。
            1.制限なし(アクセス可)
              ・営業責任者、事業者責任者
            2.支援者、営業担当者
              所属していない非公開案件：アクセス不可
              上記以外：アクセス可
            3.支援者責任者
              所属課以外の非公開案件：アクセス不可
              上記以外：アクセス可
            3.顧客
              自身の案件：アクセス可
              上記以外：アクセス不可

        Args:
            current_user (UserModel): API実行ユーザー
            project (ProjectModel): PPの案件情報

        Returns:
            bool: アクセス可能かどうか
        """
        if current_user.role == UserRoleType.SALES_MGR.key or current_user.role == UserRoleType.BUSINESS_MGR.key:
            return True
        elif current_user.role == UserRoleType.CUSTOMER.key:
            # 担当案件のみ
            if project.id in current_user.project_ids:
                return True
        elif current_user.role in [UserRoleType.SUPPORTER.key, UserRoleType.SALES.key]:
            # 担当案件 + 非担当の公開案件
            if (
                current_user.project_ids and project.id in current_user.project_ids
            ) or not project.is_secret:
                return True
        elif current_user.role == UserRoleType.SUPPORTER_MGR.key:
            # 担当案件 + 非担当の公開案件
            if (
                current_user.project_ids and project.id in current_user.project_ids
            ) or not project.is_secret:
                return True

            # 所属課の案件
            if (
                project.supporter_organization_id
                in current_user.supporter_organization_id
            ):
                return True
        return False

    @staticmethod
    def is_updatable_master_karte_role(
        current_user: UserModel, project: ProjectModel
    ) -> bool:
        """マスターカルテの更新が可能な権限かどうかを判定します。

        Args:
            current_user (UserModel): API実行ユーザー
            project (ProjectModel): PPの案件情報

        Returns:
            bool: 判定結果
        """
        if current_user.role in [
            UserRoleType.SUPPORTER.key,
            UserRoleType.SUPPORTER_MGR.key,
            UserRoleType.SALES.key,
            UserRoleType.SALES_MGR.key,
            UserRoleType.BUSINESS_MGR.key,
        ]:
            # 担当案件のみ更新が可能
            if project.id in current_user.project_ids:
                return True
        return False

    @staticmethod
    def is_accessible_master_karten_item(
        current_user: UserModel, project: ProjectModel
    ) -> bool:
        """アクセス可能なマスターカルテの項目かどうかを判定します。

        Args:
            current_user (UserModel): API実行ユーザー
            project (ProjectModel): PPの案件情報

        Returns:
            bool: アクセス可能なマスターカルテの項目かどうか
        """
        if current_user.role in [UserRoleType.SALES.key, UserRoleType.SUPPORTER.key]:
            # 非担当の公開案件
            if not current_user.project_ids or (
                project.id not in current_user.project_ids and not project.is_secret
            ):
                return False
        elif current_user.role == UserRoleType.SUPPORTER_MGR.key:
            # 非担当の公開案件 + 非所属課の公開案件
            if (
                not current_user.project_ids
                or (
                    project.id not in current_user.project_ids and not project.is_secret
                )
            ) and (
                project.supporter_organization_id
                not in current_user.supporter_organization_id
                and project.is_secret
            ):
                return False

        return True

    @staticmethod
    def get_completion_survey_data(
        pp_project_id: str, current_user: UserModel
    ) -> Tuple[SatisfactionEvaluation, str]:
        """修了アンケート満足度のレスポンス組み立てます。

        Args:
            pp_project_id (str): PPの案件ID

        Raises:
            HTTPException: _description_

        Returns:
            satisfaction_evaluation (SatisfactionEvaluation): 修了アンケートの満足度評価情報
            completion_survey_id: 修了アンケートのID
        """
        # 回答済みの修了アンケートのみ取得
        filter_condition = (
            ProjectSurveyModel.actual_survey_response_datetime.exists()
            | (size(ProjectSurveyModel.actual_survey_response_datetime) > 0)
        ) & (ProjectSurveyModel.survey_type == SurveyType.COMPLETION)

        if current_user.role == UserRoleType.SUPPORTER.key:
            filter_condition &= (ProjectSurveyModel.is_disclosure == True)  # NOQA

        completion_surveys = list(
            ProjectSurveyModel.project_id_summary_month_index.query(
                hash_key=pp_project_id, filter_condition=filter_condition
            )
        )

        if len(completion_surveys) > 0:
            # 回答日が最新のものを1件
            completion_surveys = sorted(
                completion_surveys,
                reverse=True,
                key=lambda x: x.actual_survey_response_datetime,
            )
            completion_survey = completion_surveys[0]
        else:
            return []

        # 修了アンケート満足度のレスポンス組み立て
        try:
            survey_master = SurveyMasterModel.get(
                hash_key=completion_survey.survey_master_id,
                range_key=completion_survey.survey_master_revision,
            )
        except DoesNotExist:
            return []

        satisfaction_evaluation = []
        master_satisfaction_choices = [
            question
            for question in survey_master.questions
            if question.summary_type == SurveyQuestionsSummaryType.SATISFACTION.value
        ]
        # 有効な総合満足度設問のidを取得
        valid_master_satisfaction_choices_id = None
        for choice in master_satisfaction_choices:
            if not choice.disabled:
                valid_master_satisfaction_choices_id = choice.id

        satisfaction_answer = [
            answer
            for answer in completion_survey.answers
            if valid_master_satisfaction_choices_id is not None if answer.id == valid_master_satisfaction_choices_id
        ]

        for question in master_satisfaction_choices[0].choices[0].group:
            if len(satisfaction_answer) != 0:
                satisfaction_evaluation.append(
                    SatisfactionEvaluation(
                        title=re.sub(r"\[{2}\d+\]{2}", "", question.title),
                        is_answer=question.id == list(satisfaction_answer[0].choice_ids)[0],
                    )
                )

        return satisfaction_evaluation, completion_survey.id, completion_survey.is_disclosure

    @staticmethod
    def get_usage_histories(
        pf_project_detail: dict,
        platform_api_operator: PlatformApiOperator,
        current_user: UserModel,
    ) -> UsageHistory:
        """顧客に紐づく案件のプログラム利用履歴を取得します。

        Args:
            pf_project_detail (dict): PFの案件詳細情報
            platform_api_operator (PlatformApiOperator): PFのAPIオペレーター

        Returns:
            UsageHistory: プログラム利用履歴情報
        """
        params = {
            "limit": 50,
            "offsetPage": 1
        }
        if pf_project_detail.get("customer", {}).get("salesforceCustomerId"):
            params["salesforceCustomerId"] = pf_project_detail["customer"][
                "salesforceCustomerId"
            ]
        else:
            params["partnerPortalCustomerId"] = pf_project_detail["customer"][
                "partnerPortalCustomerId"
            ]

        # PF案件一覧取得APIの呼び出し
        total_project_cnt = None
        retrieved_cnt = 0
        pf_project_list = []
        while total_project_cnt is None or len(pf_project_list) < retrieved_cnt:
            status_code, projects = platform_api_operator.get_projects(params=params)
            logger.info(f"platform getProjects statusCode: {status_code}")
            if status_code != 200:
                logger.info(
                    "platform getProjects response:" + json.dumps(projects)
                )
                raise HTTPException(
                    status_code=status_code,
                    detail=json.dumps(projects),
                )
            pf_project_list.extend(projects["projects"])
            retrieved_cnt = projects["total"]
            params["offsetPage"] += 1

            if total_project_cnt is None:
                total_project_cnt = projects["total"]

        # TODO: レスポンスの整形
        usage_histories = []
        if projects["total"] > 0:
            for project in pf_project_list:
                # NOTE: 案件名はPPの情報を使う。お客様に紐づく案件はそこまで多くない想定なので1件ずつ取得する
                pp_project: ProjectModel = None
                if project.get("project", {}).get("partnerPortalProjectId"):
                    # 通常案件はPP案件IDが取れないのでSF商談IDでPPの案件テーブルからデータを取得
                    try:
                        pp_project = ProjectModel.get(
                            hash_key=project["project"]["partnerPortalProjectId"],
                            range_key=DataType.PROJECT,
                        )
                    except DoesNotExist:
                        logger.info(
                            f"ProjectModel not found. project_id: {project['project']['id']}"
                        )
                        continue
                else:
                    try:
                        pp_project = next(
                            iter(
                                ProjectModel.data_type_name_index.query(
                                    hash_key=DataType.PROJECT,
                                    filter_condition=ProjectModel.salesforce_opportunity_id
                                    == project["project"]["salesforceOpportunityId"],
                                )
                            )
                        )

                    except StopIteration:
                        continue

                # アクセスしている案件のプログラム利用履歴は対象外
                if project["project"]["id"] == pf_project_detail["project"]["id"]:
                    continue

                # 権限で見れない案件は対象外
                if not MasterKartenService.is_accessible_master_karten_detail(
                    current_user=current_user, project=pp_project
                ):
                    continue

                usage_histories.append(
                    UsageHistory(
                        service_type=project["project"]["serviceType"],
                        project_name=pp_project.name,
                        npf_project_id=project["project"]["id"],
                    )
                )
        return usage_histories

    @staticmethod
    def get_current_program(
        pf_project_detail: dict,
        pp_project_model: ProjectModel,
        usage_histories: UsageHistory,
        current_user: UserModel,
    ) -> CurrentProgram:
        """当期支援の詳細を取得します。
        Args:
            pf_project_detail (dict): PFの案件詳細情報
            pp_project_model (ProjectModel): PPの案件情報
            usage_histories (UsageHistory): プログラム利用履歴情報
            current_user (UserModel): API実行ユーザー

        Returns:
            CurrentProgram: 当期支援情報
        """
        customer_info = pf_project_detail["customer"]
        department_info = pf_project_detail["department"]

        current_program = [
            program
            for program in pf_project_detail["programs"]
            if program["isCurrent"] is True
        ]

        if len(current_program) == 0:
            return CurrentProgram()
        else:
            current_program = current_program[0]

        # 権限によって値をセットするかしないかを制御
        if current_user.role == UserRoleType.CUSTOMER.key:
            # 顧客の場合基本情報のみ
            result = Result()
            company_department = CompanyDepartment()
            fundamental_information = FundamentalInformation(
                current_situation=current_program["presentState"],
                issue=current_program["issue"],
                request=current_program["request"],
                customer_success=current_program["customerSuccess"],
                schedule=current_program["schedule"],
                lineup=current_program["lineup"],
                support_contents=current_program["supportContents"],
                required_personal_skill=current_program["needPersonalSkill"],
                required_partner=current_program["needPartner"],
                our_strength=current_program["ourStrengths"],
                aspiration=current_program["aspiration"],
                usage_history=usage_histories,
            )
            others = Others()
        else:
            # 権限によってセットする値を制御
            if not MasterKartenService.is_accessible_master_karten_item(
                current_user, pp_project_model
            ):
                # ref: マスターカルテ項目定義書
                # 案件結果・実績情報は以下項目以外閲覧不可
                # ・カスタマーサクセス結果
                # ・カスタマーサクセス達成／未達要因要因
                # ・次期支援内容
                # ・支援で生じた課題
                # ・解決できた要因/解決できなかった要因
                result = Result(
                    customer_success_result=current_program["customerSuccessResult"],
                    customer_success_result_factor=current_program[
                        "customerSuccessResultFactor"
                    ],
                    next_support_content=current_program["nextSupportContent"],
                    support_issue=current_program["supportIssue"],
                    support_success_factor=current_program["supportSuccessFactor"],
                )
            else:
                survey_info = MasterKartenService.get_completion_survey_data(
                    pp_project_model.id, current_user
                )
                result = Result(
                    customer_success_result=current_program["customerSuccessResult"],
                    customer_success_result_factor=current_program[
                        "customerSuccessResultFactor"
                    ],
                    next_support_content=current_program["nextSupportContent"],
                    support_issue=current_program["supportIssue"],
                    support_success_factor=current_program["supportSuccessFactor"],
                    survey_customer_assessment=current_program["surveyCustomerSelfAssessment"],
                    survey_ssap_assessment=current_program["surveySsapAssessment"],
                    survey_id="" if not survey_info else survey_info[1],
                    satisfaction_evaluation=[] if not survey_info else survey_info[0],
                    is_disclosure=False if not survey_info else survey_info[2],
                )
            fundamental_information = FundamentalInformation(
                president_policy=current_program["presidentPolicy"],
                kpi=current_program["kpi"],
                to_be_three_years=current_program["threeYearsToBe"],
                current_situation=current_program["presentState"],
                issue=current_program["issue"],
                request=current_program["request"],
                customer_success=current_program["customerSuccess"],
                customer_success_reuse=current_program["customerSuccessReuse"],
                schedule=current_program["schedule"],
                lineup=current_program["lineup"],
                support_contents=current_program["supportContents"],
                required_personal_skill=current_program["needPersonalSkill"],
                required_partner=current_program["needPartner"],
                our_strength=current_program["ourStrengths"],
                aspiration=current_program["aspiration"],
                usage_history=usage_histories,
            )
            company_department = CompanyDepartment(
                customer_name=pp_project_model.customer_name,
                customer_url=customer_info["url"],
                category=customer_info["category"],
                establishment=customer_info["establishment"],
                employee=customer_info["employees"],
                capital_stock=customer_info["capitalStock"],
                business_summary=customer_info["businessSummary"],
                industry_segment=customer_info["industrySegment"],
                department_id=department_info["id"],
                department_name=department_info["departmentName"],
            )
            others = Others(
                mission=current_program["mission"],
                number_of_people=current_program["persons"],
                manager=current_program["manager"],
                commercialization_skill=current_program["commercializationSkill"],
                exist_partners=current_program["existPartners"],
                support_order=current_program["supportOrder"],
                exist_evaluation=current_program["existEvaluation"],
                exist_audition=current_program["existAudition"],
                exist_ideation=current_program["existIdeation"],
                exist_idea_review=current_program["existIdeaReview"],
                budget=current_program["budget"],
                human_resource=current_program["humanResource"],
                idea=current_program["idea"],
                theme=current_program["theme"],
                client=current_program["client"],
                client_issue=current_program["clientIssue"],
                solution=current_program["solution"],
                originality=current_program["originality"],
                mvp=current_program["mvp"],
                tam=current_program["tam"],
                sam=current_program["sam"],
                is_right_time=current_program["isRightTime"],
                road_map=current_program["loadMap"],
            )

        # 最終更新者名の取得
        if (
            current_program["lastUpdater"]
            and current_program["lastUpdater"] == MasterKartenUpdateUserType.SALESFORCE
        ):
            # Salseforceが最終更新の場合
            last_update_by = MasterKartenUpdateUserType.SALESFORCE
        else:
            # PPに存在ない場合、PF案件詳細取得APIのlastUpdateByの値をそのまま利用
            try:
                users = UserModel.data_type_email_index.query(
                    hash_key=DataType.USER,
                    range_key_condition=UserModel.email
                    == current_program["lastUpdater"],
                )
                user: UserModel = next(users)
                last_update_by = user.name
            except StopIteration:
                try:
                    users = AdminModel.data_type_email_index.query(
                        hash_key=DataType.ADMIN,
                        range_key_condition=AdminModel.email
                        == current_program["lastUpdater"],
                    )
                    user: AdminModel = next(users)
                    last_update_by = user.name
                except StopIteration:
                    last_update_by = ""

        current_program = CurrentProgram(
            id=current_program["id"],
            version=current_program["version"],
            fundamental_information=fundamental_information,
            result=result,
            company_department=company_department,
            others=others,
            last_update_datetime=current_program["updatedAt"]
            if current_program["updatedAt"]
            else current_program["insertedAt"],
            last_update_by=last_update_by,
        )
        return current_program

    @staticmethod
    def get_next_program(
        pf_project_detail: dict, usage_histories: UsageHistory, current_user: UserModel
    ) -> NextProgram:
        """次期支援情報の詳細を取得します。

        Args:
            pf_response (dict): _description_
            usage_histories (UsageHistory): _description_
            current_user (UserModel): _description_

        Returns:
            NextProgram: 次期支援情報
        """
        next_program = [
            program
            for program in pf_project_detail["programs"]
            if program["isCurrent"] is False
        ]
        if len(next_program) == 0:
            return NextProgram()
        else:
            next_program = next_program[0]

        if current_user.role == UserRoleType.CUSTOMER.key:
            fundamental_information = FundamentalInformation(
                current_situation=next_program["presentState"],
                issue=next_program["issue"],
                request=next_program["request"],
                customer_success=next_program["customerSuccess"],
                schedule=next_program["schedule"],
                lineup=next_program["lineup"],
                support_contents=next_program["supportContents"],
                required_personal_skill=next_program["needPersonalSkill"],
                required_partner=next_program["needPartner"],
                our_strength=next_program["ourStrengths"],
                aspiration=next_program["aspiration"],
                usage_history=usage_histories,
            )
            others = Others()
        else:
            fundamental_information = FundamentalInformation(
                president_policy=next_program["presidentPolicy"],
                kpi=next_program["kpi"],
                to_be_three_years=next_program["threeYearsToBe"],
                current_situation=next_program["presentState"],
                issue=next_program["issue"],
                request=next_program["request"],
                customer_success=next_program["customerSuccess"],
                customer_success_reuse=next_program["customerSuccessReuse"],
                schedule=next_program["schedule"],
                lineup=next_program["lineup"],
                support_contents=next_program["supportContents"],
                required_personal_skill=next_program["needPersonalSkill"],
                required_partner=next_program["needPartner"],
                supplement_human_resource_to_sap=next_program[
                    "supplementHumanResourceToSap"
                ],
                current_customer_profile=next_program["currentCustomerProfile"],
                want_acquire_customer_profile=next_program[
                    "wantAcquireCustomerProfile"
                ],
                our_strength=next_program["ourStrengths"],
                aspiration=next_program["aspiration"],
                usage_history=usage_histories,
            )
            others = Others(
                mission=next_program["mission"],
                number_of_people=next_program["persons"],
                manager=next_program["manager"],
                commercialization_skill=next_program["commercializationSkill"],
                exist_partners=next_program["existPartners"],
                support_order=next_program["supportOrder"],
                exist_evaluation=next_program["existEvaluation"],
                exist_audition=next_program["existAudition"],
                exist_ideation=next_program["existIdeation"],
                exist_idea_review=next_program["existIdeaReview"],
                budget=next_program["budget"],
                human_resource=next_program["humanResource"],
                idea=next_program["idea"],
                theme=next_program["theme"],
                client=next_program["client"],
                client_issue=next_program["clientIssue"],
                solution=next_program["solution"],
                originality=next_program["originality"],
                mvp=next_program["mvp"],
                tam=next_program["tam"],
                sam=next_program["sam"],
                is_right_time=next_program["isRightTime"],
                road_map=next_program["loadMap"],
            )

        # 最終更新者名の取得
        # PPに存在ない場合、PF案件詳細取得APIのlastUpdateByの値をそのまま利用
        try:
            users = UserModel.data_type_email_index.query(
                hash_key=DataType.USER,
                range_key_condition=UserModel.email
                == next_program["lastUpdater"],
            )
            user: UserModel = next(users)
            last_update_by = user.name
        except StopIteration:
            last_update_by = next_program["lastUpdater"]

        next_program = NextProgram(
            id=next_program["id"],
            version=next_program["version"],
            is_customer_public=next_program[
                "isPublishable"
            ],  # TODO: PF側の仕様書にまだ明記されてないので後で修正
            fundamental_information=fundamental_information,
            others=others,
            last_update_datetime=next_program["updatedAt"] if next_program["updatedAt"] else next_program["insertedAt"],
            last_update_by=last_update_by
        )
        return next_program

    @staticmethod
    def get_master_karten_select_box(
        authorization: HTTPAuthorizationCredentials,
    ) -> List[GetMasterKartenSelectBoxResponse]:
        """マスターカルテで使用するセレクトボックスのリストを取得します。

        Args:
            authorization (HTTPAuthorizationCredentials): 認証情報

        Returns:
            List[GetMasterKartenSelectBoxResponse]: マスターカルテで使用するセレクトボックスのリスト
        """
        platform_api_operator = PlatformApiOperator(jwt_token=authorization.jwt_token)

        # PF検索情報取得APIの呼び出し
        status_code, code_master_list = platform_api_operator.get_code_master()
        logger.info(f"platform getCodeMaster statusCode: {status_code}")
        if status_code != 200:
            logger.info(
                "platform getCodeMaster response:"
                + json.dumps(code_master_list)
            )
            raise HTTPException(
                status_code=status_code,
                detail=json.dumps(code_master_list)
            )

        # レスポンスの組み立て
        response: List[GetMasterKartenSelectBoxResponse] = []
        for code_master in code_master_list:
            items: List[SelectBoxItem] = []
            for item in code_master["items"]:
                items.append(
                    SelectBoxItem(
                        label=item["label"],
                        value=item["value"],
                    )
                )

            response.append(
                GetMasterKartenSelectBoxResponse(
                    name=code_master["name"],
                    items=items,
                )
            )
        return response

    @staticmethod
    def update_master_karten_program(
        npf_program_id: str,
        is_current_program: bool,
        body: PutMasterKartenByIdRequest,
        current_user: UserModel,
        authorization: HTTPAuthorizationCredentials,
    ) -> PutMasterKartenByIdResponse:
        """マスターカルテのプログラムを更新します。
        今回・次期支援それぞれで更新内容を分ける（今回・次回同時に更新されることはない）

        Args:
            npf_program_id (str): PFのプログラムID
            is_current_program (bool): 当期支援か否か
            body (PutMasterKartenByIdRequest): リクエストボディ
            current_user (UserModel): API実行ユーザー
            authorization (HTTPAuthorizationCredentials): 認証情報

        Returns:
            PutMasterKartenByIdResponse: 更新結果
        """
        # 権限チェック
        try:
            project = ProjectModel.get(
                hash_key=body.pp_project_id,
                range_key=DataType.PROJECT,
            )
        except DoesNotExist:
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="Not Found"
            )

        if not MasterKartenService.is_updatable_master_karte_role(
            current_user=current_user, project=project
        ):
            # 権限チェック
            raise HTTPException(
                status_code=status.HTTP_403_FORBIDDEN, detail="Forbidden"
            )
        plat_form_api_operator = PlatformApiOperator(jwt_token=authorization.jwt_token)

        # 今回・次期支援それぞれで更新内容を分ける（今回・次回同時に更新されることはない）
        if is_current_program:
            pf_request_body = UpdateCurrentProgram(
                project_id=body.npf_project_id,
                version=body.current_program.version,
                is_current=True,
                customer_success_result=body.current_program.result.customer_success_result,
                customer_success_result_factor=body.current_program.result.customer_success_result_factor,
                next_support_content=body.current_program.result.next_support_content,
                support_issue=body.current_program.result.support_issue,
                support_success_factor=body.current_program.result.support_success_factor,
                survey_customer_self_assessment=body.current_program.result.survey_customer_assessment,
                survey_ssap_assessment=body.current_program.result.survey_ssap_assessment,
            )
        else:
            pf_request_body = UpdateNextProgram(
                project_id=body.npf_project_id,
                version=body.next_program.version,
                is_current=False,
                is_publishable=body.next_program.is_customer_public,
                president_policy=body.next_program.fundamental_information.president_policy,
                kpi=body.next_program.fundamental_information.kpi,
                three_years_to_be=body.next_program.fundamental_information.to_be_three_years,
                present_state=body.next_program.fundamental_information.current_situation,
                issue=body.next_program.fundamental_information.issue,
                request=body.next_program.fundamental_information.request,
                customer_success=body.next_program.fundamental_information.customer_success,
                customer_success_reuse=body.next_program.fundamental_information.customer_success_reuse,
                schedule=body.next_program.fundamental_information.schedule,
                lineup=body.next_program.fundamental_information.lineup,
                support_contents=body.next_program.fundamental_information.support_contents,
                need_personal_skill=body.next_program.fundamental_information.required_personal_skill,
                need_partner=body.next_program.fundamental_information.required_partner,
                supplement_human_resource_to_sap=body.next_program.fundamental_information.supplement_human_resource_to_sap,
                current_customer_profile=body.next_program.fundamental_information.current_customer_profile,
                want_acquire_customer_profile=body.next_program.fundamental_information.want_acquire_customer_profile,
                our_strengths=body.next_program.fundamental_information.our_strength,
                aspiration=body.next_program.fundamental_information.aspiration,
                mission=body.next_program.others.mission,
                persons=body.next_program.others.number_of_people,
                manager=body.next_program.others.manager,
                commercialization_skill=body.next_program.others.commercialization_skill,
                exist_partners=body.next_program.others.exist_partners,
                support_order=body.next_program.others.support_order,
                exist_evaluation=body.next_program.others.exist_evaluation,
                exist_audition=body.next_program.others.exist_audition,
                exist_ideation=body.next_program.others.exist_ideation,
                exist_idea_review=body.next_program.others.exist_idea_review,
                budget=body.next_program.others.budget,
                human_resource=body.next_program.others.human_resource,
                idea=body.next_program.others.idea,
                theme=body.next_program.others.theme,
                client=body.next_program.others.client,
                client_issue=body.next_program.others.client_issue,
                solution=body.next_program.others.solution,
                originality=body.next_program.others.originality,
                mvp=body.next_program.others.mvp,
                tam=body.next_program.others.tam,
                sam=body.next_program.others.sam,
                is_right_time=body.next_program.others.is_right_time,
                load_map=body.next_program.others.road_map,
            )

        status_code, pf_response = plat_form_api_operator.update_program_by_id(
            npf_program_id, pf_request_body.dict()
        )

        if status_code != 200:
            raise HTTPException(
                status_code=status_code,
                detail=pf_response["detail"],
            )

        response: GetMasterKartenByIdResponse = (
            MasterKartenService.get_master_karten_by_id(
                current_user=current_user,
                authorization=authorization,
                npf_project_id=body.npf_project_id,
            )
        )

        # メール通知
        # 更新通知チェックボックスをONにして保存したとき
        if body.is_notify_update_master_karte:
            user_filter_condition = UserModel.disabled == False  # NOQA
            user_list = list(UserModel.scan(filter_condition=user_filter_condition))
            MasterKartenService.send_update_master_karte_mail(
                is_current_program=is_current_program,
                master_karte_info=response,
                project=project,
                user_list=user_list,
                current_user=current_user,
            )

        return response

    @staticmethod
    def create_master_karten(
        is_current_program: bool,
        body: PostMasterKartenRequest,
        current_user: UserModel,
        authorization: HTTPAuthorizationCredentials,
    ) -> PostMasterKartenResponse:
        """マスターカルテのプログラムを作成します。
        今回・次期支援それぞれでプログラムを作成する（今回・次回同時に作成されることはない）

        Args:
            is_current_program (bool): 当期支援か否か
            body (PutMasterKartenByIdRequest): リクエストボディ
            current_user (UserModel): API実行ユーザー
            authorization (HTTPAuthorizationCredentials): 認証情報

        Returns:
            PostMasterKartenResponse: 実行結果
        """
        # 権限チェック
        try:
            project = ProjectModel.get(
                hash_key=body.pp_project_id,
                range_key=DataType.PROJECT,
            )
        except DoesNotExist:
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="Not Found"
            )

        if not MasterKartenService.is_updatable_master_karte_role(
            current_user=current_user, project=project
        ):
            # 権限チェック
            raise HTTPException(
                status_code=status.HTTP_403_FORBIDDEN, detail="Forbidden"
            )
        plat_form_api_operator = PlatformApiOperator(jwt_token=authorization.jwt_token)

        # 今回・次期支援それぞれで作成内容を分ける（今回・次回同時に作成されることはない）
        if is_current_program:
            pf_request_body = CreateCurrentProgram(
                is_current=True,
                customer_success_result=body.current_program.result.customer_success_result,
                customer_success_result_factor=body.current_program.result.customer_success_result_factor,
                next_support_content=body.current_program.result.next_support_content,
                support_issue=body.current_program.result.support_issue,
                support_success_factor=body.current_program.result.support_success_factor,
                survey_customer_self_assessment=body.current_program.result.survey_customer_assessment,
                survey_ssap_assessment=body.current_program.result.survey_ssap_assessment,
            )
        else:
            pf_request_body = CreateNextProgram(
                project_id=body.npf_project_id,
                is_current=False,
                is_publishable=body.next_program.is_customer_public,
                president_policy=body.next_program.fundamental_information.president_policy,
                kpi=body.next_program.fundamental_information.kpi,
                three_years_to_be=body.next_program.fundamental_information.to_be_three_years,
                present_state=body.next_program.fundamental_information.current_situation,
                issue=body.next_program.fundamental_information.issue,
                request=body.next_program.fundamental_information.request,
                customer_success=body.next_program.fundamental_information.customer_success,
                customer_success_reuse=body.next_program.fundamental_information.customer_success_reuse,
                schedule=body.next_program.fundamental_information.schedule,
                lineup=body.next_program.fundamental_information.lineup,
                support_contents=body.next_program.fundamental_information.support_contents,
                need_personal_skill=body.next_program.fundamental_information.required_personal_skill,
                need_partner=body.next_program.fundamental_information.required_partner,
                supplement_human_resource_to_sap=body.next_program.fundamental_information.supplement_human_resource_to_sap,
                current_customer_profile=body.next_program.fundamental_information.current_customer_profile,
                want_acquire_customer_profile=body.next_program.fundamental_information.want_acquire_customer_profile,
                our_strengths=body.next_program.fundamental_information.our_strength,
                aspiration=body.next_program.fundamental_information.aspiration,
                mission=body.next_program.others.mission,
                persons=body.next_program.others.number_of_people,
                manager=body.next_program.others.manager,
                commercialization_skill=body.next_program.others.commercialization_skill,
                exist_partners=body.next_program.others.exist_partners,
                support_order=body.next_program.others.support_order,
                exist_evaluation=body.next_program.others.exist_evaluation,
                exist_audition=body.next_program.others.exist_audition,
                exist_ideation=body.next_program.others.exist_ideation,
                exist_idea_review=body.next_program.others.exist_idea_review,
                budget=body.next_program.others.budget,
                human_resource=body.next_program.others.human_resource,
                idea=body.next_program.others.idea,
                theme=body.next_program.others.theme,
                client=body.next_program.others.client,
                client_issue=body.next_program.others.client_issue,
                solution=body.next_program.others.solution,
                originality=body.next_program.others.originality,
                mvp=body.next_program.others.mvp,
                tam=body.next_program.others.tam,
                sam=body.next_program.others.sam,
                is_right_time=body.next_program.others.is_right_time,
                load_map=body.next_program.others.road_map,
            )

        status_code, pf_response = plat_form_api_operator.create_program(
            pf_request_body.dict()
        )

        if status_code != 200:
            raise HTTPException(
                status_code=status_code,
                detail=pf_response["detail"],
            )

        response: GetMasterKartenByIdResponse = (
            MasterKartenService.get_master_karten_by_id(
                current_user=current_user,
                authorization=authorization,
                npf_project_id=body.npf_project_id,
            )
        )

        # メール通知
        # 更新通知チェックボックスをONにして保存したとき
        if body.is_notify_update_master_karte:
            user_filter_condition = UserModel.disabled == False  # NOQA
            user_list = list(UserModel.scan(filter_condition=user_filter_condition))
            MasterKartenService.send_update_master_karte_mail(
                is_current_program=is_current_program,
                master_karte_info=response,
                project=project,
                user_list=user_list,
                current_user=current_user,
            )

        return response

    @staticmethod
    def make_pf_request_param(items: list):
        """PF APIのAND指定リクエストパラメーターを作成"""
        param = ""
        for item in items:
            param += item + "+"
        return param.rstrip("+")

    @staticmethod
    def get_npf_project_id(
        pp_project_id: str,
        current_user: UserModel,
        authorization: HTTPAuthorizationCredentials,
    ) -> GetNpfProjectIdResponse:
        """
        PP案件IDからNPF案件IDを取得する

        Args:
            pp_project_id (str): PP案件ID
            current_user (AuthUser): 認証済みのユーザー
            authorization (HTTPAuthorizationCredentials): 認証情報

        Raises:
            HTTPException: 404 Not Found

        Returns:
            GetNpfProjectIdResponse: 取得結果
        """
        try:
            project = ProjectModel.get(
                hash_key=pp_project_id, range_key=DataType.PROJECT
            )
        except DoesNotExist:
            raise HTTPException(status_code=status.HTTP_404_NOT_FOUND)

        platform_api_operator = PlatformApiOperator(jwt_token=authorization.jwt_token)
        if project.salesforce_opportunity_id:
            params = {"salesforceOpportunityId": project.salesforce_opportunity_id}
        else:
            # PP独自案件
            params = {"partnerPortalProjectId": project.id}

        status_code, response = platform_api_operator.get_projects(params=params)
        if status_code != status.HTTP_200_OK:
            raise HTTPException(status_code=status_code, detail=json.dumps(response))

        return GetNpfProjectIdResponse(
            npf_project_id=response["projects"][0]["project"]["id"]
        )

    @staticmethod
    def split_project_ids_for_is_in_filter_condition(project_ids: set) -> str:
        """
        ProjectModel.id.is_in()の引数に渡すために、project_idsを100件ずつに分割する
        DynamoDBの制限事項により、100件以上の場合はエラーになるため

        Args:
            project_ids (set): 案件IDのset
        Returns:
            str: ProjectModel.id.is_in()の引数に渡す文字列
        """
        max = 100
        project_id_list = list(copy.deepcopy(project_ids))
        splited_project_id_list = [
            project_id_list[i : i + max] for i in range(0, len(project_id_list), max)
        ]
        for i, project_ids in enumerate(splited_project_id_list):
            if i == 0:
                filter_condition = ProjectModel.id.is_in(*project_ids)
            else:
                filter_condition |= ProjectModel.id.is_in(*project_ids)

        return filter_condition

    @staticmethod
    def send_mail(
        template: MailType,
        to_addr_list: list[str],
        cc_addr_list: list[str],
        payload: dict,
    ):
        """メール送信

        Args:
            template (MailType): メールテンプレート名
            to_addr_list (List[str]): メール宛先（TO）
            cc_addr_list (List[str]): メール宛先（CC）
            payload (dict): ペイロード
        """
        queue_name = get_app_settings().sqs_email_queue_name
        message_body = {
            "template": template,
            "to": to_addr_list,
            "cc": cc_addr_list,
            "payload": payload,
        }
        sqs_message_body = json.dumps(message_body)
        SqsHelper().send_message_to_queue(
            queue_name=queue_name, message_body=sqs_message_body
        )

    @staticmethod
    def send_update_master_karte_mail(
        is_current_program: bool,
        master_karte_info: GetMasterKartenByIdResponse,
        project: ProjectModel,
        user_list: list[UserModel],
        current_user: UserModel,
    ):
        """
        マスターカルテ登録／更新時のメール通知を送信します

        Args:
            is_current_program (bool): 当期支援か否か
            master_karte_info (GetMasterKartenByIdResponse): マスターカルテ情報
            project (ProjectModel): 案件情報
            user_list (list[UserModel]): ユーザ情報リスト
            current_user (UserModel): 実行ユーザ情報
        """
        # 送信先の編集
        # TO：事業者責任者（全員）、営業責任者（全員）、支援者責任者（全員）
        # CC: 案件に設定されているプロデューサー、案件に設定されているアクセラレータ（全員）、案件に設定された商談所有者
        to_email_list: list[str] = []
        cc_email_list: list[str] = []
        for user_info in user_list:
            if user_info.role in [
                UserRoleType.BUSINESS_MGR.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER_MGR.key,
            ]:
                to_email_list.append(user_info.email)

            if (
                user_info.id == project.main_supporter_user_id
                or (
                    project.supporter_user_ids
                    and user_info.id in project.supporter_user_ids
                )
                or user_info.id == project.main_sales_user_id
            ):
                cc_email_list.append(user_info.email)

        # 重複を排除
        to_email_list = list(set(to_email_list))
        cc_email_list = list(set(cc_email_list))

        cp_fundamental_information = None
        cp_result = None
        cp_company_department = None
        np_fundamental_information = None
        if master_karte_info.current_program:
            cp_fundamental_information = (
                master_karte_info.current_program.fundamental_information
            )
            cp_result = master_karte_info.current_program.result
            cp_company_department = master_karte_info.current_program.company_department
        if master_karte_info.next_program:
            np_fundamental_information = (
                master_karte_info.next_program.fundamental_information
            )

        # マスターカルテ更新者の苗字を取得
        master_karte_update_username = ""
        if current_user.name:
            temp_user_name_split = current_user.name.split()
            if temp_user_name_split:
                master_karte_update_username = temp_user_name_split[0]

        # 部署名
        mail_department_name = (
            cp_company_department.department_name
            if (
                cp_company_department.category
                == MasterKartenCustomerCategory.SONY_GROUP
            )
            and cp_company_department.department_name
            else ""
        )

        fo_site_url = get_app_settings().fo_site_url
        fo_master_karte_detail_url = fo_site_url + FoAppUrl.MASTER_KARTE_DETAIL.format(
            npfProjectId=master_karte_info.npf_project_id
        )

        if is_current_program:
            # マスターカルテ当期支援結果通知
            # 登録／更新後のメール通知のため、当期支援の情報（master_karte_info.current_program）が存在する前提
            fo_master_karte_detail_url = (
                fo_master_karte_detail_url
                + FoAppUrl.MASTER_KARTE_DETAIL_QUERY_PARAM.format(isCurrent="true")
            )
            payload = {
                "project_id": master_karte_info.npf_project_id,
                "project_name": project.name,
                "update_user_name": master_karte_update_username,
                "customer_name": project.customer_name,
                "cp_customer_success": cp_fundamental_information.customer_success,
                "cp_customer_success_result": cp_result.customer_success_result,
                "cp_customer_success_result_factor": cp_result.customer_success_result_factor,
                "cp_support_issue": cp_result.support_issue,
                "cp_support_success_factor": cp_result.support_success_factor,
                "cp_survey_ssap_assessment": cp_result.survey_ssap_assessment,
                "cp_next_support_content": cp_result.next_support_content,
                "cp_president_policy": cp_fundamental_information.president_policy,
                "cp_to_be_three_years": cp_fundamental_information.to_be_three_years,
                "cp_issue": cp_fundamental_information.issue,
                "cp_request": cp_fundamental_information.request,
                "cp_schedule": cp_fundamental_information.schedule,
                "cp_support_contents": cp_fundamental_information.support_contents,
                "company_dept_customer_name": cp_company_department.customer_name,
                "company_dept_customer_url": cp_company_department.customer_url,
                "company_dept_category": cp_company_department.category,
                "company_dept_establishment": cp_company_department.establishment,
                "company_dept_employee": cp_company_department.employee,
                "company_dept_capital_stock": cp_company_department.capital_stock,
                "company_dept_business_summary": cp_company_department.business_summary,
                "company_dept_industry_segment": cp_company_department.industry_segment,
                "company_dept_department_name": mail_department_name,
                "fo_master_karte_URL": fo_master_karte_detail_url,
            }
            # Noneの項目を空文字に変換
            converted_payload = {k: conv_blank(v) for k, v in payload.items()}
            MasterKartenService.send_mail(
                template=MailType.UPDATE_MASTER_KARTE_CURRENT_PROGRAM,
                to_addr_list=to_email_list,
                cc_addr_list=cc_email_list,
                payload=converted_payload,
            )
        else:
            # マスターカルテ次期支援内容通知
            # 登録／更新後のメール通知のため、以下の情報が存在する前提
            # ・当期支援の情報（master_karte_info.current_program）
            # ・次期支援の情報（master_karte_info.next_program）
            fo_master_karte_detail_url = (
                fo_master_karte_detail_url
                + FoAppUrl.MASTER_KARTE_DETAIL_QUERY_PARAM.format(isCurrent="false")
            )
            payload = {
                "project_id": master_karte_info.npf_project_id,
                "project_name": project.name,
                "update_user_name": master_karte_update_username,
                "customer_name": project.customer_name,
                "np_president_policy": np_fundamental_information.president_policy,
                "np_to_be_three_years": np_fundamental_information.to_be_three_years,
                "np_current_situation": np_fundamental_information.current_situation,
                "np_issue": np_fundamental_information.issue,
                "np_request": np_fundamental_information.request,
                "np_customer_success": np_fundamental_information.customer_success,
                "np_support_contents": np_fundamental_information.support_contents,
                "np_lineup": np_fundamental_information.lineup,
                "np_schedule": np_fundamental_information.schedule,
                "np_required_personal_skill": np_fundamental_information.required_personal_skill,
                "np_required_partner": np_fundamental_information.required_partner,
                "np_supplement_human_resource_to_sap": np_fundamental_information.supplement_human_resource_to_sap,
                "np_our_strength": np_fundamental_information.our_strength,
                "np_current_customer_profile": np_fundamental_information.current_customer_profile,
                "np_want_acquire_customer_profile": np_fundamental_information.want_acquire_customer_profile,
                "np_aspiration": np_fundamental_information.aspiration,
                "cp_president_policy": cp_fundamental_information.president_policy,
                "cp_to_be_three_years": cp_fundamental_information.to_be_three_years,
                "cp_current_situation": cp_fundamental_information.current_situation,
                "cp_issue": cp_fundamental_information.issue,
                "cp_request": cp_fundamental_information.request,
                "cp_customer_success": cp_fundamental_information.customer_success,
                "cp_support_contents": cp_fundamental_information.support_contents,
                "cp_lineup": cp_fundamental_information.lineup,
                "cp_schedule": cp_fundamental_information.schedule,
                "company_dept_customer_name": cp_company_department.customer_name,
                "company_dept_customer_url": cp_company_department.customer_url,
                "company_dept_category": cp_company_department.category,
                "company_dept_establishment": cp_company_department.establishment,
                "company_dept_employee": cp_company_department.employee,
                "company_dept_capital_stock": cp_company_department.capital_stock,
                "company_dept_business_summary": cp_company_department.business_summary,
                "company_dept_industry_segment": cp_company_department.industry_segment,
                "company_dept_department_name": mail_department_name,
                "fo_master_karte_URL": fo_master_karte_detail_url,
            }
            # Noneの項目を空文字に変換
            converted_payload = {k: conv_blank(v) for k, v in payload.items()}
            MasterKartenService.send_mail(
                template=MailType.UPDATE_MASTER_KARTE_NEXT_PROGRAM,
                to_addr_list=to_email_list,
                cc_addr_list=cc_email_list,
                payload=converted_payload,
            )
