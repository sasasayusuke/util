from typing import Dict, List

from cachetools import TTLCache, cached

from app.models.customer import CustomerModel
from app.models.master import MasterSupporterOrganizationModel
from app.models.project import ProjectModel
from app.models.survey_master import SurveyMasterModel
from app.models.user import UserModel
from app.resources.const import MasterDataType, TtlTime


class CachedDbItems:
    @staticmethod
    @cached(cache=TTLCache(maxsize=1, ttl=TtlTime.DEFAULT))
    def ReturnServiceTypes():
        """サービスタイプIDとサービスタイプ名の一覧を取得し配列として返却する関数

        Args:

        Returns:
            service_types: List[Dict(id: int, name: str)]
        """
        service_types: List = []
        service_type_iter = (
            MasterSupporterOrganizationModel.data_type_order_index.query(
                hash_key=MasterDataType.MASTER_SERVICE_TYPE.value,
                attributes_to_get=["id", "name"],
            )
        )
        for item in service_type_iter:
            service_types.append(dict(id=item.id, name=item.name))

        return service_types

    @staticmethod
    @cached(cache=TTLCache(maxsize=1, ttl=TtlTime.DEFAULT))
    def ReturnSupporterOrganizations():
        """支援者組織IDと支援者名の一覧を取得し配列として返却する関数

        Args:

        Returns:
            supporter_organizations: List[Dict(id: int, name: str)]
        """
        supporter_organizations: List = []
        supporter_organization_iter = (
            MasterSupporterOrganizationModel.data_type_order_index.query(
                hash_key=MasterDataType.MASTER_SUPPORTER_ORGANIZATION.value,
                attributes_to_get=["id", "value"],
            )
        )
        for item in supporter_organization_iter:
            supporter_organizations.append(dict(id=item.id, name=item.value))

        return supporter_organizations

    @staticmethod
    @cached(cache=TTLCache(maxsize=1, ttl=TtlTime.DEFAULT))
    def get_projects():
        """案件情報一覧を取得し配列として返却する関数

        Args:

        Returns:
            projects: List[Dict()]
        """
        project_iter = ProjectModel.scan()
        projects: List = []

        for project in project_iter:
            current_items: Dict = {}
            for k in project.attribute_values:
                current_items[k] = project.attribute_values[k]

            projects.append(current_items)

        return projects

    @staticmethod
    @cached(cache=TTLCache(maxsize=1, ttl=TtlTime.DEFAULT))
    def get_customers():
        """顧客ID, 顧客名の一覧を取得し配列として返却する関数

        Args:

        Returns:
            customers: List[Dict(id: int, name: str)]
        """
        customers: List = []
        customer_iter = CustomerModel.scan(attributes_to_get=["id", "name"])
        for item in customer_iter:
            customers.append(dict(id=item.id, name=item.name))

        return customers

    @staticmethod
    @cached(cache=TTLCache(maxsize=1, ttl=TtlTime.DEFAULT))
    def get_survey_masters():
        """アンケートマスタID, アンケートマスタ名の一覧を取得し配列として返却する関数

        Args:

        Returns:
            survey_masters: List[Dict(id: int, name: str)]
        """
        survey_masters: List = []
        survey_master_iter = SurveyMasterModel.scan()
        for item in survey_master_iter:
            survey_masters.append(
                dict(
                    id=item.id,
                    revision=item.revision,
                    name=item.name,
                    questions=item.questions,
                )
            )

        return survey_masters

    @staticmethod
    @cached(cache=TTLCache(maxsize=1, ttl=TtlTime.DEFAULT))
    def get_users():
        """一般ユーザーID, 一般ユーザー名と支援者組織の一覧を取得し配列として返却する関数

        Args:

        Returns:
            users: List[Dict(id: int, name: str), supporter_organization_id: List[str]]
        """
        users: List = []
        user_iter = UserModel.scan(
            attributes_to_get=["id", "name", "supporter_organization_id"]
        )
        for item in user_iter:
            users.append(
                dict(
                    id=item.id,
                    name=item.name,
                    supporter_organization_id=item.supporter_organization_id,
                )
            )

        return users
