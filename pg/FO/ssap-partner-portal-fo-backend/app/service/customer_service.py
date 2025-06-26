from typing import List

from app.auth.auth import AuthUser
from app.core.common_logging import CustomLogger
from app.models.customer import CustomerModel
from app.resources.const import (DataType, GetCustomersSortType,
                                 SuggestCustomersSortType)
from app.schemas.customer import (CustomerInfoForGetCustomers,
                                  GetCustomerByIdResponse,
                                  GetCustomersResponse,
                                  SuggestCustomersResponse)
from app.service.common_service.pagination import EmptyPage, Paginator
from app.service.common_service.user_info import get_update_user_name
from fastapi import HTTPException, status
from pynamodb.exceptions import DoesNotExist

logger = CustomLogger.get_logger()


class CustomerService:
    @staticmethod
    def suggest_customers(
        sort: SuggestCustomersSortType, current_user: AuthUser
    ) -> SuggestCustomersResponse:
        """取引先のサジェスト用データを取得します

        Args:
            sort (SuggestCustomersSortType): ソート （'name:asc'）
            current_user (Behavior, optional): 認証済みのユーザー

        Returns:
            SuggestCustomersResponse: 取得結果
        """
        if sort == SuggestCustomersSortType.NAME_ASC:
            # name昇順で取得
            customer_iter = CustomerModel.data_type_name_index.query(
                hash_key=DataType.CUSTOMER, scan_index_forward=True
            )
            suggest_list: List[SuggestCustomersResponse] = []
            for customer in customer_iter:
                suggest_list.append(
                    SuggestCustomersResponse(**customer.attribute_values)
                )
            return suggest_list
        else:
            # 呼出し元でsort項目チェック済の為、以下は発生しない想定
            raise HTTPException(status_code=status.HTTP_500_INTERNAL_SERVER_ERROR)

    @staticmethod
    def get_customer_by_id(customer_id: str, current_user: AuthUser):
        """取引先IDを指定して取引先情報を取得する

        Args:
            customer_id (str): 取引先ID
            current_user (Behavior, optional): 認証済みのユーザー
        Raises:
            HTTPException: 404 Not found

        Returns:
            GetCustomerByIdResponse: 取得結果
        """
        try:
            customer = CustomerModel.get(
                hash_key=customer_id, range_key=DataType.CUSTOMER
            )
        except DoesNotExist:
            logger.warning(f"GetCustomerById not found. customer_id: {customer_id}")
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="Not found"
            )

        user_name: str = get_update_user_name(customer.update_id)

        return GetCustomerByIdResponse(
            update_user_name=user_name, **customer.attribute_values
        )

    @staticmethod
    def get_customers(
        name: str,
        sort: GetCustomersSortType,
        limit: int,
        offset_page: int,
        current_user: AuthUser,
    ) -> GetCustomersResponse:
        """案件を検索し、案件一覧を取得する
        Args:
            name (str): 案件名
            sort (GetCustomersSortType): ソート
            limit (int): 最大取得件数
            offset_page (int): リストの中で何ページ目を取得するか
            current_user (Behavior, optional): 認証済みのユーザー
        Returns:
            GetCustomersResponse: 取得結果
        """
        # GSIソート順(name)
        scan_index_forward = None
        if sort == GetCustomersSortType.NAME_ASC:
            scan_index_forward = True
        elif sort == GetCustomersSortType.NAME_DESC:
            scan_index_forward = False

        # 抽出条件
        range_key_condition = None
        # 抽出条件: 案件名
        if name:
            range_key_condition &= CustomerModel.name == name

        # 取引先情報取得
        result_list: List[CustomerModel] = list(
            CustomerModel.data_type_name_index.query(
                hash_key=DataType.CUSTOMER,
                range_key_condition=range_key_condition,
                scan_index_forward=scan_index_forward,
            )
        )

        customer_info_list: List[CustomerInfoForGetCustomers] = [
            CustomerInfoForGetCustomers(**customer.attribute_values)
            for customer in result_list
        ]

        # ページネーション
        try:
            p = Paginator(customer_info_list, limit)
            current_page = p.page(offset_page).object_list
        except EmptyPage:
            logger.warning("GetCustomers not found.")
            raise HTTPException(status_code=status.HTTP_404_NOT_FOUND)

        return GetCustomersResponse(
            offset_page=offset_page,
            total=len(customer_info_list),
            customers=current_page,
        )
