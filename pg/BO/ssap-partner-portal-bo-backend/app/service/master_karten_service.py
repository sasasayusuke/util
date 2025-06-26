import json
import re
from typing import List, Union, Tuple
from app.core.common_logging import CustomLogger

from fastapi import HTTPException, status
from fastapi.security import HTTPAuthorizationCredentials
from pynamodb.exceptions import DoesNotExist
from pynamodb.expressions.condition import size

from app.models.admin import AdminModel
from app.models.customer import CustomerModel
from app.models.project import ProjectModel
from app.models.project_survey import ProjectSurveyModel
from app.models.survey_master import SurveyMasterModel
from app.models.user import UserModel
from app.resources.const import (
    DataType,
    MasterKartenProgramType,
    MasterKartenUpdateUserType,
    SurveyQuestionsSummaryType,
    SurveyType,
    UserRoleType,
)
from app.schemas.master_karten import (
    CompanyDepartment,
    CurrentProgram,
    FundamentalInformation,
    GetMasterKartenByIdResponse,
    GetMasterKartenResponse,
    GetMasterKartenSelectBoxResponse,
    GetNpfProjectIdResponse,
    MasterKartenInfoForGetMasterKarten,
    NextProgram,
    Others,
    Result,
    SatisfactionEvaluation,
    SelectBoxItems,
    UsageHistory,
)
from app.service.karten_service import KartenService
from app.utils.platform import PlatformApiOperator

logger = CustomLogger.get_logger()


class MasterKartenService:
    """マスターカルテサービスクラス"""

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
        current_user: AdminModel,
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
            category（list）:顧客セグメント
            industrySegment（list）:業界セグメント
            departmentName（str）:部署名
            currentSituation（str）:現状
            issue（str）:課題
            customerSuccess（str）:カスタマーサクセス
            lineup（list）:ラインナップ
            requiredPersonalSkill（str）:お客様に不足している人的リソース
            requiredPartner（str）:お客様に紹介したい企業や産業
            strength（str）:自社の強み
            current_user (Behavior, optional): 認証済みのユーザー
            authorization (HTTPAuthorizationCredentials): トークン情報

        Returns:
            GetMasterKartenResponse: 取得結果
        """
        platform_api_operator = PlatformApiOperator(jwt_token=authorization.jwt_token)

        pf_params = {}
        salesforce_opportunity_id_list = []
        pp_project_id_list = []

        no_liimt_role = {
            UserRoleType.SALES_MGR.key,
            UserRoleType.SURVEY_OPS.key,
            UserRoleType.MAN_HOUR_OPS.key,
            UserRoleType.SYSTEM_ADMIN.key,
        }

        if len(no_liimt_role & current_user.roles) == 0:
            # 制限ありの場合（営業担当者）
            # ログインユーザーが営業担当者のみのロールを保持している場合
            if (
                UserRoleType.SALES.key in current_user.roles
                and len(current_user.roles) == 1
            ):
                # メールアドレスが一致する一般ユーザ情報を取得
                range_key_condition = UserModel.email == current_user.email
                filter_condition = UserModel.disabled == False
                user_info_list: List[UserModel] = list(
                    UserModel.data_type_email_index.query(
                        hash_key=DataType.USER,
                        range_key_condition=range_key_condition,
                        filter_condition=filter_condition,
                    )
                )
                # 取得結果は0件または1件のため、リストの先頭を取得
                user_info = user_info_list[0] if user_info_list else None
                # 担当案件
                if user_info and user_info.project_ids:
                    # 一般ユーザー情報が存在、かつ担当案件が存在する場合
                    project_list = list(
                        ProjectModel.data_type_name_index.query(
                            hash_key=DataType.PROJECT,
                            filter_condition=ProjectModel.id.is_in(
                                *user_info.project_ids
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
                # PP独自案件
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
                "platform getProjects response:" + pf_projects["detail"]["message"]
            )
            raise HTTPException(
                status_code=status_code,
                detail=pf_projects["detail"]["message"],
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
    def make_pf_request_param(items: list):
        """PF APIのAND指定リクエストパラメーターを作成"""
        param = ""
        for item in items:
            param += item + "+"
        return param.rstrip("+")

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
                + code_master_list["detail"]["message"]
            )
            raise HTTPException(
                status_code=status_code,
                detail=code_master_list["detail"]["message"],
            )

        # レスポンスの組み立て
        response: List[GetMasterKartenSelectBoxResponse] = []
        for code_master in code_master_list:
            items: List[SelectBoxItems] = []
            for item in code_master["items"]:
                items.append(
                    SelectBoxItems(
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
    def get_master_karten_by_id(
        npf_project_id,
        current_user: AdminModel,
        authorization: HTTPAuthorizationCredentials,
    ) -> Union[str, None, HTTPException]:
        """マスターカルテから指定した案件を一意に取得します。

        Args:
            npf_project_id (str): PFの案件ID
            current_user (Behavior, optional): 認証済みのユーザー
            authorization (HTTPAuthorizationCredentials): トークン情報

        Returns:
            GetMasterKartenByIdResponse: マスターカルテ詳細情報 取得結果
        """
        platform_api_operator = PlatformApiOperator(jwt_token=authorization.jwt_token)

        # PF案件詳細情報取得APIの呼び出し
        status_code, pf_response = platform_api_operator.get_project_by_pf_id(
            npf_project_id
        )
        logger.info(f"platform getProjectByPfId statusCode: {status_code}")

        if status_code != 200:
            logger.info(
                "platform getProjectByPfId response:" + pf_response["detail"]["message"]
            )
            raise HTTPException(
                status_code=status_code,
                detail=pf_response["detail"]["message"],
            )
        # 細かな権限チェックのためNPFのデータからPPの案件情報を取得
        if pf_response.get("project", {}).get("partnerPortalProjectId"):
            # 通常案件はPP案件IDが取れないのでSF商談IDでPPの案件テーブルからデータを取得
            try:
                pp_project = ProjectModel.get(
                    hash_key=pf_response["project"]["partnerPortalProjectId"],
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
            current_user=current_user
        )

        response = GetMasterKartenByIdResponse(
            master_karte_id=pf_response["programs"][0]["id"],
            pp_project_id=pp_project.id,
            supporter_organization_id=pp_project.supporter_organization_id,
            service=pf_response["project"]["serviceType"],
            project=pp_project.name,
            client=pp_project.customer_name,
            support_date_from=pp_project.support_date_from,
            support_date_to=pp_project.support_date_to,
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
        current_user: AdminModel, project: ProjectModel
    ):
        """マスターカルテ詳細へアクセス可能か判定.
          ユーザ情報は管理ユーザ情報から取得.
          また、営業担当者で、案件に所属しているか判定する際は一般ユーザ情報を使用する.
            1.制限なし(アクセス可)
              ・営業責任者
              ・アンケート事務局
              ・稼働率調査事務局
              ・システム管理者
              ・事業者責任者
            2.支援者責任者
              「自身の課以外の非公開案件」：アクセス不可
              上記以外：アクセス可
            3.営業担当者
              「所属していない非公開案件」：アクセス不可
              上記以外：アクセス可
        Args:
            current_user (AdminModel): ログインユーザ情報
            project (ProjectModel): 対象案件情報
        Returns:
            bool: True(アクセス可能)、False(アクセス不可)
        """

        # メールアドレスが一致する一般ユーザ情報を取得
        range_key_condition = UserModel.email == current_user.email
        filter_condition = UserModel.disabled == False
        user_info_list: List[UserModel] = list(
            UserModel.data_type_email_index.query(
                hash_key=DataType.USER,
                range_key_condition=range_key_condition,
                filter_condition=filter_condition,
            )
        )
        # 取得結果は0件または1件のため、リストの先頭を取得
        user_info = user_info_list[0] if user_info_list else None

        for role in current_user.roles:
            if role in [
                UserRoleType.SALES_MGR.key,
                UserRoleType.SURVEY_OPS.key,
                UserRoleType.MAN_HOUR_OPS.key,
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.BUSINESS_MGR.key,
            ]:
                return True
            elif role == UserRoleType.SUPPORTER_MGR.key:
                if not project.supporter_organization_id and project.is_secret:
                    # 自身の課以外の非公開案件
                    # (支援者組織IDがNoneまたは空の場合も自身の課以外とする)
                    return False
                elif (
                    project.supporter_organization_id
                    not in current_user.supporter_organization_id
                    and project.is_secret
                ):
                    # 自身の課以外の非公開案件
                    return False
                return True
            elif role == UserRoleType.SALES.key:
                if user_info:
                    # 一般ユーザ情報が存在する場合
                    if (
                        not user_info.project_ids
                        or project.id not in user_info.project_ids
                    ) and project.is_secret:
                        # 所属していない非公開案件
                        return False
                else:
                    # 一般ユーザ情報が存在しない場合
                    if project.is_secret:
                        # 所属していない非公開案件
                        # (一般ユーザ情報が存在しない場合も所属していないとみなす)
                        return False
                return True
        return False

    @staticmethod
    def get_completion_survey_data(
        pp_project_id: str,
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
            if choice.disabled == False:
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
                        survey_id=completion_survey.id,
                        title=re.sub(r"\[{2}\d+\]{2}", "", question.title),
                        is_answer=question.id == list(satisfaction_answer[0].choice_ids)[0],
                    )
                )
        return satisfaction_evaluation, completion_survey.id

    @staticmethod
    def get_usage_histories(
        pf_project_detail: dict, platform_api_operator: PlatformApiOperator, current_user: AdminModel
    ) -> UsageHistory:
        """顧客に紐づく案件のプログラム利用履歴を取得します。

        Args:
            pf_response (dict): PFの案件詳細情報
            platform_api_operator (PlatformApiOperator): _description_

        Returns:
            usage_histories（UsageHistory）: プログラム利用履歴
        """
        params = {
            "limit": 50,
            "offsetPage": 1
        }
        if pf_project_detail.get("customer", {}).get("salesforceCustomerId"):
            # SF取引先IDが存在しない場合、PP取引先IDを利用
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
                logger.info("platform getProjects response:" + json.dumps(projects))
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

                # ログインユーザーのロールが支援者責任者のみの場合
                if len(current_user.roles) == 1 and UserRoleType.SUPPORTER_MGR.key in current_user.roles:
                    # 所属課案件／その他公開案件のみアクセス可
                    if not pp_project.is_secret:
                        pass
                    elif (
                        pp_project.supporter_organization_id
                        and pp_project.supporter_organization_id in current_user.supporter_organization_id
                    ):
                        pass
                    else:
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
        current_user: AdminModel,
    ) -> CurrentProgram:
        """当期支援の詳細を取得します。
        Args:
            pf_project_detail (dict): PFの案件詳細情報
            pp_project_model (ProjectModel): PPの案件情報
            usage_histories (UsageHistory): プログラム利用履歴情報
            current_user (AdminModel): API実行ユーザー

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
            return None
        else:
            current_program = current_program[0]

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
            our_strengths=current_program["ourStrengths"],
            aspiration=current_program["aspiration"],
            usage_history=usage_histories,
        )
        survey_info = MasterKartenService.get_completion_survey_data(
            pp_project_model.id
        )
        result = Result(
            customer_success_result=current_program["customerSuccessResult"],
            customer_success_result_factor=current_program["customerSuccessResultFactor"],
            next_support_content=current_program["nextSupportContent"],
            support_issue=current_program["supportIssue"],
            support_success_factor=current_program["supportSuccessFactor"],
            survey_customer_assessment=current_program["surveyCustomerSelfAssessment"],
            survey_ssap_assessment=current_program["surveySsapAssessment"],
            survey_id="" if not survey_info else survey_info[1],
            satisfaction_evaluation=[] if not survey_info else survey_info[0],
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
            version=current_program["version"],
            fundamental_information=fundamental_information,
            result=result,
            company_department=company_department,
            others=others,
            last_update_datetime=current_program["updatedAt"] if current_program["updatedAt"] else current_program["insertedAt"],
            last_update_by=last_update_by,
        )
        return current_program

    @staticmethod
    def get_next_program(
        pf_project_detail: dict, usage_histories: UsageHistory, current_user: AdminModel
    ) -> NextProgram:
        """次期支援情報の詳細を取得します。

        Args:
            pf_response (dict): PFの案件詳細情報
            usage_histories (UsageHistory): プログラム利用履歴情報
            current_user (AdminModel): API実行ユーザー

        Returns:
            NextProgram: 次期支援情報
        """
        next_program = [
            program
            for program in pf_project_detail["programs"]
            if program["isCurrent"] is False
        ]

        if len(next_program) == 0:
            return None
        else:
            next_program = next_program[0]

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
            supplement_human_resource_to_sap=next_program["supplementHumanResourceToSap"],
            current_customer_profile=next_program["currentCustomerProfile"],
            want_acquire_customer_profile=next_program["wantAcquireCustomerProfile"],
            our_strengths=next_program["ourStrengths"],
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
                range_key_condition=UserModel.email == next_program["lastUpdater"],
            )
            user: UserModel = next(users)
            last_update_by = user.name
        except StopIteration:
            last_update_by = next_program["lastUpdater"]

        next_program = NextProgram(
            is_customer_public=next_program["isPublishable"],
            version=next_program["version"],
            fundamental_information=fundamental_information,
            others=others,
            last_update_datetime=next_program["updatedAt"] if next_program["updatedAt"] else next_program["insertedAt"],
            last_update_by=last_update_by,
        )
        return next_program

    @staticmethod
    def get_npf_project_id(
        pp_project_id: str,
        current_user: AdminModel,
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
