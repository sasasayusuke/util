from typing import List
from app.auth.jwt import JWTBearer

from fastapi import APIRouter, Depends, Path, status

from app.auth.auth import get_current_user_factory
from app.resources.const import UserRoleType
from app.schemas.customer import (
    CreateCustomerRequest,
    CreateCustomerResponse,
    DeleteCustomerByIdQuery,
    DeleteCustomerByIdResponse,
    GetCustomerByIdResponse,
    GetCustomersQuery,
    GetCustomersResponse,
    ImportCustomersRequest,
    ImportCustomersResponse,
    SuggestCustomersQuery,
    SuggestCustomersResponse,
    UpdateCustomerByIdQuery,
    UpdateCustomerByIdRequest,
    UpdateCustomerByIdResponse,
)
from app.service.customer_service import CustomerService
from fastapi.security import HTTPAuthorizationCredentials

router = APIRouter()


@router.post(
    "/customers",
    tags=["Customer"],
    description="取引先を一件作成します",
    response_model=CreateCustomerResponse,
    status_code=status.HTTP_200_OK,
)
def create_customer(
    body: CreateCustomerRequest,
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SURVEY_OPS.key,
                UserRoleType.MAN_HOUR_OPS.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
    authorization: HTTPAuthorizationCredentials = Depends(JWTBearer()),
) -> CreateCustomerResponse:
    """Post /customers 取引先新規個別登録API

    Args:
        body (CreateCustomerRequest): 登録内容
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        CreateCustomerResponse: 登録結果
    """
    return CustomerService.create_customer(
        item=body, current_user=current_user, authorization=authorization
    )


@router.post(
    "/customers/import",
    tags=["Customer"],
    description="顧客のCSVデータをエラーチェックまたは取り込み実行します",
    response_model=ImportCustomersResponse,
    status_code=status.HTTP_200_OK,
)
def import_customer(
    body: ImportCustomersRequest,
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SURVEY_OPS.key,
                UserRoleType.MAN_HOUR_OPS.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
) -> ImportCustomersResponse:
    """Post /customers/import 取引先一括登録API

    Args:
        body (CreateCustomerRequest): 登録内容
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        ImportCustomersResponse: 登録結果
    """
    return CustomerService.import_customer(item=body, current_user=current_user)


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
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SURVEY_OPS.key,
                UserRoleType.MAN_HOUR_OPS.key,
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
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SURVEY_OPS.key,
                UserRoleType.MAN_HOUR_OPS.key,
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
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SURVEY_OPS.key,
                UserRoleType.MAN_HOUR_OPS.key,
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


@router.put(
    "/customers/{customer_id}",
    tags=["Customer"],
    description="取引先をIDで一意に更新します。",
    response_model=UpdateCustomerByIdResponse,
    status_code=status.HTTP_200_OK,
)
def update_customer_by_id(
    body: UpdateCustomerByIdRequest,
    query_params: UpdateCustomerByIdQuery = Depends(),
    customer_id: str = Path(
        ..., title="顧客ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    ),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SURVEY_OPS.key,
                UserRoleType.MAN_HOUR_OPS.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
) -> UpdateCustomerByIdResponse:
    """Put /customers/{customer_id} 取引先更新API

    Args:
        body (UpdateCustomerByIdRequest): 更新内容
        version: ロックキー（楽観ロック制御） クエリパラメータで指定(>=1)
        customer_id (str): 取引先ID パスパラメータで指定
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        GetCustomerByIdResponse: 取得結果
    """
    return CustomerService.update_customer_by_id(
        item=body,
        version=query_params.version,
        customer_id=customer_id,
        current_user=current_user,
    )


@router.delete(
    "/customers/{customer_id}",
    tags=["Customer"],
    description="取引先をIDで一意に削除します。 取引先に紐づく情報案件または一般ユーザーがある場合削除は不可。",
    response_model=DeleteCustomerByIdResponse,
    status_code=status.HTTP_200_OK,
)
def delete_customer_by_id(
    query_params: DeleteCustomerByIdQuery = Depends(),
    customer_id: str = Path(
        ..., title="顧客ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    ),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SURVEY_OPS.key,
                UserRoleType.MAN_HOUR_OPS.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
) -> DeleteCustomerByIdResponse:
    """Delete /customers/{customer_id} 取引先削除API

    Args:
        version: ロックキー（楽観ロック制御） クエリパラメータで指定(>=1)
        customer_id (str): 取引先ID パスパラメータで指定
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        DeleteCustomerByIdResponse: 削除結果
    """
    return CustomerService.delete_customer_by_id(
        version=query_params.version, customer_id=customer_id, current_user=current_user
    )
