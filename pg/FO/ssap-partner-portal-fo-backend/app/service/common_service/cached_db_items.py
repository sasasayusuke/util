from typing import Dict, List

from app.models.master import MasterSupporterOrganizationModel
from app.models.project import ProjectModel
from app.resources.const import MasterDataType, TtlTime
from cachetools import TTLCache, cached


class CachedDbItems:
    @staticmethod
    @cached(cache=TTLCache(maxsize=1, ttl=TtlTime.DEFAULT))
    def ReturnProjects():
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
    def get_supporter_organizations():
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
