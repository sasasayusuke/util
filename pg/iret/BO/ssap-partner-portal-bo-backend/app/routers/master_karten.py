from typing import List

from fastapi import APIRouter, Depends, Path, Query, status
from fastapi.security import HTTPAuthorizationCredentials

from app.auth.auth import get_current_user_factory
from app.auth.jwt import JWTBearer
from app.resources.const import UserRoleType
from app.schemas.master_karten import (
    GetMasterKartenByIdResponse,
    GetMasterKartenQuery,
    GetMasterKartenResponse,
    GetMasterKartenSelectBoxResponse,
    GetNpfProjectIdResponse,
)
from app.service.master_karten_service import MasterKartenService

router = APIRouter()


@router.get(
    "/master-karten",
    tags=["MasterKarten"],
    description="マスターカルテの一覧を取得します。",
    response_model=GetMasterKartenResponse,
    status_code=status.HTTP_200_OK,
)
def get_master_karten(
    query_params: GetMasterKartenQuery = Depends(),
    category: List[str] = Query([], alias="category[]"),
    industry_segment: List[str] = Query([], alias="industrySegment[]"),
    lineup: List[str] = Query([], alias="lineup[]"),
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
) -> GetMasterKartenResponse:
    """Get /master-karten マスターカルテ一覧取得API

    Args:
        query_params(GetMasterKartenQuery):クエリパラメーター
        current_user (AdminModel): API実行ユーザー

    Returns:
        GetMasterKartenResponse: 取得結果
    """
    return MasterKartenService.get_master_karten(
        offset_page=query_params.offset_page,
        limit=query_params.limit,
        customer_id=query_params.customer_id,
        support_date_from=query_params.support_date_from,
        support_date_to=query_params.support_date_to,
        is_current_program=query_params.is_current_program,
        category=category,
        industry_segment=industry_segment,
        department_name=query_params.department_name,
        current_situation=query_params.current_situation,
        issue=query_params.issue,
        customer_success=query_params.customer_success,
        lineup=lineup,
        required_personal_skill=query_params.required_personal_skill,
        required_partner=query_params.required_partner,
        strength=query_params.strength,
        current_user=current_user,
        authorization=authorization,
    )


@router.get(
    "/master-karten/select-box",
    tags=["MasterKarten"],
    response_model=List[GetMasterKartenSelectBoxResponse],
    description="マスターカルテで使用するセレクトボックスのリストを取得します。",
    status_code=status.HTTP_200_OK,
)
def get_master_karten_select_box(
    _=Depends(
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
) -> GetMasterKartenSelectBoxResponse:
    """Get /master-karten/select-box マスターカルテ一覧セレクトボックス候補取得API

    Args:
        current_user (Behavior, optional): 認証済みのユーザー

        Returns:
            GetMasterKartenSelectBoxResponse: 取得結果
    """
    return MasterKartenService.get_master_karten_select_box(authorization=authorization)


@router.get(
    "/master-karten/{npf_project_id}",
    tags=["MasterKarten"],
    response_model=GetMasterKartenByIdResponse,
    description="マスターカルテをIDで取得します。",
    status_code=status.HTTP_200_OK,
)
def get_master_karten_by_id(
    npf_project_id: str = Path(..., description="NPFプロジェクトID"),
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
) -> GetMasterKartenByIdResponse:

    """Get /master-karten/{npf_project_id} マスターカルテ情報詳細取得API

    Args:
        npf_project_id (str, optional): NPFプロジェクトID

    Returns:
        GetMasterKartenByIdResponse: 取得結果
    """
    return MasterKartenService.get_master_karten_by_id(
        npf_project_id=npf_project_id,
        current_user=current_user,
        authorization=authorization,
    )


@router.get(
    "/master-karten/npf-id/{pp_project_id}",
    tags=["MasterKarten"],
    description="PP案件IDからNPF案件IDを取得します。",
    response_model=GetNpfProjectIdResponse,
    status_code=status.HTTP_200_OK,
)
def get_npf_project_id(
    pp_project_id: str = Path(..., description="PP案件ID"),
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
) -> GetNpfProjectIdResponse:
    """Get /masters/npf-id/{pp_project_id} NPF案件ID取得API

    Returns:
        str: NPF案件ID
    """
    return MasterKartenService.get_npf_project_id(
        pp_project_id=pp_project_id,
        current_user=current_user,
        authorization=authorization,
    )
