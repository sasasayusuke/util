import json
from datetime import datetime

import pytest
from fastapi.testclient import TestClient
from pynamodb.exceptions import DoesNotExist
from pytz import timezone

from app.main import app
from app.models.project import (
    GrossProfitAttribute,
    ProjectModel,
    SalesforceMainCustomerAttribute,
)
from app.models.user import UserModel
from app.resources.const import DataType
from app.schemas.master_karten import (
    GetMasterKartenByIdResponse,
    CurrentProgram,
    FundamentalInformation,
    NextProgram,
    Result,
    CompanyDepartment,
    Others,
)
from app.service.master_karten_service import MasterKartenService
from app.utils.aws.sqs import SqsHelper
from app.utils.platform import PlatformApiOperator

client = TestClient(app)


class TestCreateMasterKarten:
    @pytest.mark.usefixtures("auth_sales_user")
    def test_ok_current_program(self, mocker):
        mock = mocker.patch.object(ProjectModel, "get")
        mock.return_value = ProjectModel(
            id="346820d3-618b-4ddb-bde9-030eb6441630",
            data_type=DataType.PROJECT,
            salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
            salesforce_opportunity_id="a03d1661-cdab-494c-818e-d4845088269b",
            salesforce_update_at=timezone("Asia/Tokyo").localize(
                datetime.strptime(
                    "2020/10/23 03:21",
                    "%Y/%m/%d %H:%M",
                )
            ),
            customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
            name="サンプルプロジェクト",
            customer_name="ソニーグループ株式会社",
            service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
            create_new=True,
            continued=True,
            main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
            contract_date="2021/01/30",
            phase="プラン提示(D)",
            customer_success="DXの実現",
            support_date_from="2021/01/30",
            support_date_to="2021/02/28",
            profit=GrossProfitAttribute(
                monthly=[
                    400000,
                    400000,
                    400000,
                    400000,
                    400000,
                    400000,
                    400000,
                    400000,
                    400000,
                    400000,
                    400000,
                    400000,
                ],
                quarterly=[1200000, 1200000, 1200000, 1200000],
                half=[2400000, 2400000],
                year=4800000,
            ),
            total_contract_time=200,
            main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
            salesforce_main_customer=SalesforceMainCustomerAttribute(
                name="山田太郎",
                email="yamada@example.com",
                organization_name="IST",
                job="部長",
            ),
            customer_user_ids=set(list(["c9b67094-cdab-494c-818e-d4845088269b"])),
            main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
            supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
            salesforce_main_supporter_user_name="山田太郎",
            supporter_user_ids=set(list(["c9b67094-cdab-494c-818e-d4845088269b"])),
            salesforce_supporter_user_names=["山田太郎", "山田次郎"],
            is_count_man_hour=True,
            is_karte_remind=True,
            contract_type="有償",
            is_secret=False,
            create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
            create_at="2022-05-23T16:34:21.523000+0000",
            update_id="a9b67094-cdab-494c-818e-d4845088269b",
            update_at="2022-05-23T16:34:21.523000+0000",
            version=1,
        )

        pf_api_mock = mocker.patch.object(PlatformApiOperator, "create_program")
        pf_api_mock.return_value = (200, {"message": "ok"})

        pf_get_by_id_mock = mocker.patch.object(MasterKartenService, "get_master_karten_by_id")
        get_master_karten_by_id_response_top_dict = {
            "npfProjectId": "100",
            "ppProjectId": "886a3144-9650-4a34-8a23-3b02f3b9aeac",
            "supporterOrganizationId": "7ac8bddf-88da-46c9-a504-a03d1661ad58",
            "service": "string",
            "project": "サンプルプロジェクト",
            "client": "ソニーグループ株式会社",
            "supportDateFrom": "2021/01/30",
            "supportDateTo": "2022/02/30",
        }
        fundamental_information_dict = {
            "presidentPolicy": "string",
            "kpi": "string",
            "toBeThreeYears": "string",
            "currentSituation": "string",
            "issue": "string",
            "request": "string",
            "customerSuccess": "DXの実現",
            "customerSuccessReuse": True,
            "schedule": "string",
            "lineup": "string",
            "supportContents": "string",
            "requiredPersonalSkill": "string",
            "requiredPartner": "string",
            "supplementHumanResourceToSap": "string",
            "currentCustomerProfile": "string",
            "wantAcquireCustomerProfile": "string",
            "ourStrength": "string",
            "aspiration": "string",
            "usageHistory": []
        }
        result_dict = {
            "customerSuccessResult": "string",
            "customerSuccessResultFactor": "string",
            "nextSupportContent": "string",
            "supportIssue": "string",
            "supportSuccessFactor": "string",
            "surveyCustomerAssessment": "string",
            "surveySsapAssessment": "string",
            "surveyId": "11111449-5d63-42d4-ae1b-f0faf65a7076",
            "satisfactionEvaluation": [
                {
                    "isAnswer": False,
                    "title": "5：とても満足"
                },
                {
                    "isAnswer": True,
                    "title": "4：満足"
                },
                {
                    "isAnswer": False,
                    "title": "：どちらともいえない"
                },
                {
                    "isAnswer": False,
                    "title": "：不満"
                },
                {
                    "isAnswer": False,
                    "title": "1：とても不満"
                }
            ],
            "isDisclosure": True,
        }
        company_department_dict = {
            "customerName": "ソニーグループ株式会社",
            "customerUrl": "http://sample.com",
            "category": "string",
            "establishment": "2020-10-23",
            "employee": "10000",
            "capitalStock": "10000",
            "businessSummary": "事業概要",
            "industrySegment": "電気機器",
            "departmentId": "100",
            "departmentName": "IT戦略事業部",
        }
        others_dict = {
            "mission": "string",
            "numberOfPeople": "0",
            "manager": "string",
            "commercializationSkill": "string",
            "existPartners": "string",
            "supportOrder": "string",
            "existEvaluation": "string",
            "existAudition": "string",
            "existIdeation": "string",
            "existIdeaReview": "string",
            "budget": "string",
            "humanResource": "string",
            "idea": "string",
            "theme": "string",
            "client": "string",
            "clientIssue": "string",
            "solution": "string",
            "originality": "string",
            "mvp": "string",
            "tam": "string",
            "sam": "string",
            "isRightTime": "string",
            "roadMap": "string"
        }
        pf_get_by_id_mock.return_value = GetMasterKartenByIdResponse(
            **get_master_karten_by_id_response_top_dict,
            current_program=CurrentProgram(
                id="100",
                version=1,
                fundamental_information=FundamentalInformation(**fundamental_information_dict),
                result=Result(**result_dict),
                company_department=CompanyDepartment(**company_department_dict),
                others=Others(**others_dict),
                last_update_datetime="2023-01-01 00:00:00",
                last_update_by="ソニー太郎",
            ),
            next_program=NextProgram(
                id="200",
                version=1,
                is_customer_public=False,
                fundamental_information=FundamentalInformation(**fundamental_information_dict),
                others=Others(**others_dict),
                last_update_datetime="2023-01-01 00:00:00",
                last_update_by="abs@abc.co.jp"
            ),
        )

        user_mock = mocker.patch.object(UserModel, "scan")
        user_mock.return_value = iter([])

        mocker.patch.object(SqsHelper, "send_message_to_queue")

        body = {
            "npfProjectId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            "ppProjectId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            "currentProgram": {
                "version": 0,
                "result": {
                    "customerSuccessResult": "string",
                    "customerSuccessResultFactor": "string",
                    "nextSupportContent": "string",
                    "supportIssue": "string",
                    "suportSuccessFactor": "string",
                    "surveyCustomerAssessment": "string",
                    "surveySsapAssessment": "string",
                },
            },
            "isNotifyUpdateMasterKarte" : True,
        }

        response = client.post(
            "/api/master-karten?isCurrentProgram=true",
            json=body,
        )
        assert response.status_code == 200

    @pytest.mark.usefixtures("auth_sales_user")
    def test_ok_next_program(self, mocker):
        mock = mocker.patch.object(ProjectModel, "get")
        mock.return_value = ProjectModel(
            id="346820d3-618b-4ddb-bde9-030eb6441630",
            data_type=DataType.PROJECT,
            salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
            salesforce_opportunity_id="a03d1661-cdab-494c-818e-d4845088269b",
            salesforce_update_at=timezone("Asia/Tokyo").localize(
                datetime.strptime(
                    "2020/10/23 03:21",
                    "%Y/%m/%d %H:%M",
                )
            ),
            customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
            name="サンプルプロジェクト",
            customer_name="ソニーグループ株式会社",
            service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
            create_new=True,
            continued=True,
            main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
            contract_date="2021/01/30",
            phase="プラン提示(D)",
            customer_success="DXの実現",
            support_date_from="2021/01/30",
            support_date_to="2021/02/28",
            profit=GrossProfitAttribute(
                monthly=[
                    400000,
                    400000,
                    400000,
                    400000,
                    400000,
                    400000,
                    400000,
                    400000,
                    400000,
                    400000,
                    400000,
                    400000,
                ],
                quarterly=[1200000, 1200000, 1200000, 1200000],
                half=[2400000, 2400000],
                year=4800000,
            ),
            total_contract_time=200,
            main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
            salesforce_main_customer=SalesforceMainCustomerAttribute(
                name="山田太郎",
                email="yamada@example.com",
                organization_name="IST",
                job="部長",
            ),
            customer_user_ids=set(list(["c9b67094-cdab-494c-818e-d4845088269b"])),
            main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
            supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
            salesforce_main_supporter_user_name="山田太郎",
            supporter_user_ids=set(list(["c9b67094-cdab-494c-818e-d4845088269b"])),
            salesforce_supporter_user_names=["山田太郎", "山田次郎"],
            is_count_man_hour=True,
            is_karte_remind=True,
            contract_type="有償",
            is_secret=False,
            create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
            create_at="2022-05-23T16:34:21.523000+0000",
            update_id="a9b67094-cdab-494c-818e-d4845088269b",
            update_at="2022-05-23T16:34:21.523000+0000",
            version=1,
        )

        body = {
            "npfProjectId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            "ppProjectId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            "nextProgram": {
                "version": 1,
                "isCustomerPublic": False,
                "fundamentalInformation": {
                    "presidentPolicy": "string",
                    "kpi": "string",
                    "toBeThreeYears": "string",
                    "currentSituation": "string",
                    "issue": "string",
                    "request": "string",
                    "customerSuccess": "string",
                    "customerSuccessReuse": True,
                    "schedule": "string",
                    "lineup": "string",
                    "supportContents": "string",
                    "requiredPersonalSkill": "string",
                    "requiredPartner": "string",
                    "supplementHumanResourceToSap": "string",
                    "currentCustomerProfile": "string",
                    "wantAcquireCustomerProfile": "string",
                    "ourStrength": "string",
                    "aspiration": "string",
                },
                "others": {
                    "mission": "string",
                    "numberOfPeople": "string",
                    "manager": "string",
                    "commercializationSkill": "string",
                    "existPartners": "string",
                    "supportOrder": "string",
                    "existEvaluation": "string",
                    "existAudition": "string",
                    "existIdeation": "string",
                    "existIdeaReview": "string",
                    "budget": "string",
                    "humanResource": "string",
                    "idea": "string",
                    "theme": "string",
                    "client": "string",
                    "clientIssue": "string",
                    "solution": "string",
                    "originality": "string",
                    "mvp": "string",
                    "tam": "string",
                    "sam": "string",
                    "isRightTime": "string",
                    "roadMap": "string",
                },
            },
            "isNotifyUpdateMasterKarte" : True,
        }

        pf_api_mock = mocker.patch.object(PlatformApiOperator, "create_program")
        pf_api_mock.return_value = (200, {"message": "OK"})

        pf_get_by_id_mock = mocker.patch.object(MasterKartenService, "get_master_karten_by_id")
        get_master_karten_by_id_response_top_dict = {
            "npfProjectId": "100",
            "ppProjectId": "886a3144-9650-4a34-8a23-3b02f3b9aeac",
            "supporterOrganizationId": "7ac8bddf-88da-46c9-a504-a03d1661ad58",
            "service": "string",
            "project": "サンプルプロジェクト",
            "client": "ソニーグループ株式会社",
            "supportDateFrom": "2021/01/30",
            "supportDateTo": "2022/02/30",
        }
        fundamental_information_dict = {
            "presidentPolicy": "string",
            "kpi": "string",
            "toBeThreeYears": "string",
            "currentSituation": "string",
            "issue": "string",
            "request": "string",
            "customerSuccess": "DXの実現",
            "customerSuccessReuse": True,
            "schedule": "string",
            "lineup": "string",
            "supportContents": "string",
            "requiredPersonalSkill": "string",
            "requiredPartner": "string",
            "supplementHumanResourceToSap": "string",
            "currentCustomerProfile": "string",
            "wantAcquireCustomerProfile": "string",
            "ourStrength": "string",
            "aspiration": "string",
            "usageHistory": []
        }
        result_dict = {
            "customerSuccessResult": "string",
            "customerSuccessResultFactor": "string",
            "nextSupportContent": "string",
            "supportIssue": "string",
            "supportSuccessFactor": "string",
            "surveyCustomerAssessment": "string",
            "surveySsapAssessment": "string",
            "surveyId": "11111449-5d63-42d4-ae1b-f0faf65a7076",
            "satisfactionEvaluation": [
                {
                    "isAnswer": False,
                    "title": "5：とても満足"
                },
                {
                    "isAnswer": True,
                    "title": "4：満足"
                },
                {
                    "isAnswer": False,
                    "title": "：どちらともいえない"
                },
                {
                    "isAnswer": False,
                    "title": "：不満"
                },
                {
                    "isAnswer": False,
                    "title": "1：とても不満"
                }
            ],
            "isDisclosure": True,
        }
        company_department_dict = {
            "customerName": "ソニーグループ株式会社",
            "customerUrl": "http://sample.com",
            "category": "string",
            "establishment": "2020-10-23",
            "employee": "10000",
            "capitalStock": "10000",
            "businessSummary": "事業概要",
            "industrySegment": "電気機器",
            "departmentId": "100",
            "departmentName": "IT戦略事業部",
        }
        others_dict = {
            "mission": "string",
            "numberOfPeople": "0",
            "manager": "string",
            "commercializationSkill": "string",
            "existPartners": "string",
            "supportOrder": "string",
            "existEvaluation": "string",
            "existAudition": "string",
            "existIdeation": "string",
            "existIdeaReview": "string",
            "budget": "string",
            "humanResource": "string",
            "idea": "string",
            "theme": "string",
            "client": "string",
            "clientIssue": "string",
            "solution": "string",
            "originality": "string",
            "mvp": "string",
            "tam": "string",
            "sam": "string",
            "isRightTime": "string",
            "roadMap": "string"
        }
        pf_get_by_id_mock.return_value = GetMasterKartenByIdResponse(
            **get_master_karten_by_id_response_top_dict,
            current_program=CurrentProgram(
                id="100",
                version=1,
                fundamental_information=FundamentalInformation(**fundamental_information_dict),
                result=Result(**result_dict),
                company_department=CompanyDepartment(**company_department_dict),
                others=Others(**others_dict),
                last_update_datetime="2023-01-01 00:00:00",
                last_update_by="ソニー太郎",
            ),
            next_program=NextProgram(
                id="200",
                version=1,
                is_customer_public=False,
                fundamental_information=FundamentalInformation(**fundamental_information_dict),
                others=Others(**others_dict),
                last_update_datetime="2023-01-01 00:00:00",
                last_update_by="abs@abc.co.jp"
            ),
        )

        user_mock = mocker.patch.object(UserModel, "scan")
        user_mock.return_value = iter([])

        mocker.patch.object(SqsHelper, "send_message_to_queue")

        response = client.post(
            "/api/master-karten?isCurrentProgram=false",
            json=body,
        )
        assert response.status_code == 200

    @pytest.mark.usefixtures("auth_sales_user")
    def test_no_authorized_to_update_program(self, mocker):
        mocker.patch.object(ProjectModel, "get")
        mocker.return_value = ProjectModel(
            id="efba46d3-1d56-428b-ac6d-a51e263b54a8",
            data_type=DataType.PROJECT,
            salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
            salesforce_opportunity_id="a03d1661-cdab-494c-818e-d4845088269b",
            salesforce_update_at=timezone("Asia/Tokyo").localize(
                datetime.strptime(
                    "2020/10/23 03:21",
                    "%Y/%m/%d %H:%M",
                )
            ),
            customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
            name="サンプルプロジェクト",
            customer_name="ソニーグループ株式会社",
            service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
            create_new=True,
            continued=True,
            main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
            contract_date="2021/01/30",
            phase="プラン提示(D)",
            customer_success="DXの実現",
            support_date_from="2021/01/30",
            support_date_to="2021/02/28",
            profit=GrossProfitAttribute(
                monthly=[
                    400000,
                    400000,
                    400000,
                    400000,
                    400000,
                    400000,
                    400000,
                    400000,
                    400000,
                    400000,
                    400000,
                    400000,
                ],
                quarterly=[1200000, 1200000, 1200000, 1200000],
                half=[2400000, 2400000],
                year=4800000,
            ),
            total_contract_time=200,
            main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
            salesforce_main_customer=SalesforceMainCustomerAttribute(
                name="山田太郎",
                email="yamada@example.com",
                organization_name="IST",
                job="部長",
            ),
            customer_user_ids=set(list(["c9b67094-cdab-494c-818e-d4845088269b"])),
            main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
            supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
            salesforce_main_supporter_user_name="山田太郎",
            supporter_user_ids=set(list(["c9b67094-cdab-494c-818e-d4845088269b"])),
            salesforce_supporter_user_names=["山田太郎", "山田次郎"],
            is_count_man_hour=True,
            is_karte_remind=True,
            contract_type="有償",
            is_secret=False,
            create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
            create_at="2022-05-23T16:34:21.523000+0000",
            update_id="a9b67094-cdab-494c-818e-d4845088269b",
            update_at="2022-05-23T16:34:21.523000+0000",
            version=1,
        )

        body = {
            "npfProjectId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            "ppProjectId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            "nextProgram": {
                "version": 1,
                "isCustomerPublic": False,
                "fundamentalInformation": {
                    "presidentPolicy": "string",
                    "kpi": "string",
                    "toBeThreeYears": "string",
                    "currentSituation": "string",
                    "issue": "string",
                    "request": "string",
                    "customerSuccess": "string",
                    "customerSuccessReuse": True,
                    "schedule": "string",
                    "lineup": "string",
                    "supportContents": "string",
                    "requiredPersonalSkill": "string",
                    "requiredPartner": "string",
                    "ourStrength": "string",
                    "aspiration": "string",
                },
                "others": {
                    "mission": "string",
                    "numberOfPeople": "string",
                    "manager": "string",
                    "commercialization": "string",
                    "existPartners": "string",
                    "supportOrder": "string",
                    "existEvaluation": "string",
                    "existAudition": "string",
                    "existIdeation": "string",
                    "existIdeaReview": "string",
                    "budget": "string",
                    "humanResource": "string",
                    "idea": "string",
                    "theme": "string",
                    "client": "string",
                    "clientIssue": "string",
                    "solution": "string",
                    "originality": "string",
                    "mvp": "string",
                    "tam": "string",
                    "sam": "string",
                    "isRightTime": "string",
                    "roadMap": "string",
                },
            },
            "isNotifyUpdateMasterKarte" : False,
        }

        response = client.post(
            "/api/master-karten?isCurrentProgram=false",
            json=body,
        )
        assert response.status_code == 403

    @pytest.mark.usefixtures("auth_sales_user")
    def test_project_not_found(self, mocker):
        mock = mocker.patch.object(ProjectModel, "get")
        mock.side_effect = DoesNotExist()

        body = {
            "npfProjectId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            "ppProjectId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            "nextProgram": {
                "version": 1,
                "isCustomerPublic": False,
                "fundamentalInformation": {
                    "presidentPolicy": "string",
                    "kpi": "string",
                    "toBeThreeYears": "string",
                    "currentSituation": "string",
                    "issue": "string",
                    "request": "string",
                    "customerSuccess": "string",
                    "customerSuccessReuse": True,
                    "schedule": "string",
                    "lineup": "string",
                    "supportContents": "string",
                    "requiredPersonalSkill": "string",
                    "requiredPartner": "string",
                    "ourStrength": "string",
                    "aspiration": "string",
                },
                "others": {
                    "mission": "string",
                    "numberOfPeople": "string",
                    "manager": "string",
                    "commercialization": "string",
                    "existPartners": "string",
                    "supportOrder": "string",
                    "existEvaluation": "string",
                    "existAudition": "string",
                    "existIdeation": "string",
                    "existIdeaReview": "string",
                    "budget": "string",
                    "humanResource": "string",
                    "idea": "string",
                    "theme": "string",
                    "client": "string",
                    "clientIssue": "string",
                    "solution": "string",
                    "originality": "string",
                    "mvp": "string",
                    "tam": "string",
                    "sam": "string",
                    "isRightTime": "string",
                    "roadMap": "string",
                },
            },
            "isNotifyUpdateMasterKarte" : False,
        }

        response = client.post(
            "/api/master-karten?isCurrentProgram=false",
            json=body,
        )
        assert response.status_code == 404

    @pytest.mark.parametrize(
        "body",
        [
            #####################
            # ppProjectId
            #####################
            # 必須チェック
            {
                # "ppProjectId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                "currentProgram": {
                    "version": 0,
                    "result": {
                        "projectResult": "string",
                        "projectSuccessFactor": "string",
                        "supportResult": "string",
                        "ssapAssessment": "string",
                        "assessmentCause": "string",
                    },
                },
                "isNotifyUpdateMasterKarte" : True,
            },
            #####################
            # currentProgram & nextProgram
            #####################
            # 必須チェック
            {
                "ppProjectId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                # "currentProgram": {
                #     "version": 0,
                #     "result": {
                #         "projectResult": "string",
                #         "projectSuccessFactor": "string",
                #         "supportResult": "string",
                #         "ssapAssessment": "string",
                #         "assessmentCause": "string",
                #     },
                # },
                "isNotifyUpdateMasterKarte" : True,
            },
            ###############################
            # isNotifyUpdateMasterKarte
            ###############################
            # 必須チェック
            {
                "ppProjectId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                "currentProgram": {
                    "version": 0,
                    "result": {
                        "projectResult": "string",
                        "projectSuccessFactor": "string",
                        "supportResult": "string",
                        "ssapAssessment": "string",
                        "assessmentCause": "string",
                    },
                },
                # "isNotifyUpdateMasterKarte" : True,
            },
        ],
    )
    @pytest.mark.usefixtures("auth_sales_user")
    def test_validation(self, body):
        response = client.post(
            "/api/master-karten?isCurrentProgram=true",
            json=body,
        )

        assert response.status_code == 422
