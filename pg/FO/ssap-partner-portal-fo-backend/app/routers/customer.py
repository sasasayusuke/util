from typing import List

from app.auth.auth import get_current_user_factory
from app.resources.const import UserRoleType
from app.schemas.customer import (
    GetCustomerByIdResponse,
    GetCustomersQuery,
    GetCustomersResponse,
    SuggestCustomersQuery,
    SuggestCustomersResponse,
)
from app.service.customer_service import CustomerService
from fastapi import APIRouter, Depends, Path, status

router = APIRouter()


@router.get(
    "/customers/suggest",
    tags=["Customer"],
    description="取引先のサジェスト用情報を取得します。 フロントでサジェストするためのリストを取得。",
    response_model=List[SuggestCustomersResponse],
    status_code=status.HTTP_200_OK,
)
def suggest_customers(
    query_params: SuggestCustomersQuery = Depends(),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SUPPORTER.key,
                UserRoleType.SALES.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
) -> List[SuggestCustomersResponse]:
    """Get /customers/suggest 取引先サジェスト用データ取得API

    Args:
        sort (SuggestCustomersSortType): ソート クエリパラメータで指定（'name:asc'）
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        SuggestCustomersResponse: 取得結果
    """
    return CustomerService.suggest_customers(
        sort=query_params.sort, current_user=current_user
    )


@router.get(
    "/customers/{customer_id}",
    tags=["Customer"],
    description="取引先をIDで一意に取得します。",
    response_model=GetCustomerByIdResponse,
    status_code=status.HTTP_200_OK,
)
def get_customer_by_id(
    customer_id: str = Path(
        ..., title="顧客ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    ),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SUPPORTER.key,
                UserRoleType.SALES.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
) -> GetCustomerByIdResponse:
    """Get /customers/{customer_id} 取引先詳細取得API

    Args:
        customer_id (str): 取引先ID パスパラメータで指定
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        GetCustomerByIdResponse: 取得結果
    """
    return CustomerService.get_customer_by_id(
        customer_id=customer_id, current_user=current_user
    )


@router.get(
    "/customers",
    tags=["Customer"],
    description="取引先の検索・一覧を取得します。 ",
    response_model=GetCustomersResponse,
    status_code=status.HTTP_200_OK,
)
def get_customers(
    query_params: GetCustomersQuery = Depends(),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SUPPORTER.key,
                UserRoleType.SALES.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
) -> GetCustomersResponse:
    """Get /customers 取引先一覧取得API
    Args:
        name (str): 取引先名
        sort (GetCustomersSortType): ソート
        limit (int): 最大取得件数 Default:Constの規定値(DefaultPageItemCount.limit)
        offsetPage (int): リストの中で何ページ目を取得するか Default:1
        current_user (Behavior, optional): 認証済みのユーザー
    Returns:
        GetCustomersResponse: 取得結果
    """
    return CustomerService.get_customers(
        name=query_params.name,
        sort=query_params.sort,
        limit=query_params.limit,
        offset_page=query_params.offset_page,
        current_user=current_user,
    )
