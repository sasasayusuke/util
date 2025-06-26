from typing import List
from app.auth.jwt import JWTBearer
from app.schemas.base import OKResponse
from app.schemas.master import GetNpfProjectIdResponse
from app.service.master_karten_service import MasterKartenService
from fastapi import APIRouter, Depends, Path, Query, status
from app.auth.auth import AuthUser, get_current_user_factory

from app.resources.const import UserRoleType
from app.schemas.master_karten import (
    GetMasterKartenByIdResponse,
    GetMasterKartenQuery,
    GetMasterKartenResponse,
    GetMasterKartenSelectBoxResponse,
    PostMasterKartenRequest,
    PostMasterKartenResponse,
    PutMasterKartenByIdRequest,
    PutMasterKartenByIdResponse,
)
from fastapi.security import HTTPAuthorizationCredentials

router = APIRouter(prefix="/master-karten")


@router.get(
    "",
    tags=["MasterKarten"],
    status_code=status.HTTP_200_OK,
    response_model=GetMasterKartenResponse,
    description="""
検索結果に一致するマスターカルテの一覧を取得します。<br>
PFのデータを検索し、NPFのデータを取得します。ただし現段階ではPPにある項目に関してPPのデータを優先して返却します。

## PP利用テーブル
| テーブル名 | ファセット| CRUD | 備考 |
| ----- | ----- | ----- | ----- |
| 一般ユーザーテーブル(PP) | 一般ユーザー情報  | R |  |
| 案件テーブル(PP) | 案件情報  | R |  |

## 利用PF API
[GetProjects API](https://platform-dev-api-docs.s3.ap-northeast-1.amazonaws.com/api.html#tag/Project)

## 処理定義
* 権限制御をPPで行い、パラメータに追加
* 検索項目をパラメーターに追加し、NPF APIのfieldに必要な項目のみ指定し、リクエストを行う
* NPFGetProjectsAPIで検索を行い、該当の案件ID一覧を取得
* 該当の案件一覧をPPの案件テーブルから別途取得
* 各項目をフロントエンドに返す

# その他
* ソートは行わない
* 検索条件はANDのみ適用
* FOでは対象案件の絞り込みを行う
* お客様名のサジェストはPPのサジェストAPIを使用（PP側にある案件のみを表示させる必要があるため）
* NPF案件IDはマスターカルテ詳細ページのリンクに利用
* PP案件IDはその他のリンクに利用
* 各検索項目は"lineup=A%20B%20C"のように%20を挟むことで複数の項目をAnd指定することが可能
""",
)
def get_master_karten(
    query_params: GetMasterKartenQuery = Depends(),
    category: List[str] = Query([], alias="category[]"),
    industry_segment: List[str] = Query([], alias="industrySegment[]"),
    lineup: List[str] = Query([], alias="lineup[]"),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SUPPORTER.key,
                UserRoleType.CUSTOMER.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
    authorization: HTTPAuthorizationCredentials = Depends(JWTBearer()),
):
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
        all=query_params.all,
        current_user=current_user,
        authorization=authorization,
    )


@router.post(
    "",
    tags=["MasterKarten"],
    status_code=status.HTTP_200_OK,
    response_model=PostMasterKartenResponse,
    description="""
マスターカルテを新規作成します。（PP独自案件のみ呼ばれる想定）

## 利用PF API
- [CreateProgram](https://platform-dev-api-docs.s3.ap-northeast-1.amazonaws.com/api.html#tag/Program/paths/~1program/post)
## PP利用テーブル
| テーブル名 | ファセット| CRUD | 備考 |
| ----- | ----- | ----- | ----- |
| 一般ユーザーテーブル(PP) | 一般ユーザー情報  | R | 権限制御のために利用 |

# 処理定義
* 権限制御をPPで行う

# その他
- NPFで保存するプログラムレコードは当期支援・次期支援の二種類が存在
- 同画面内で案件カルテ取更新PIを叩く
""",
)
def create_master_karten(
    body: PostMasterKartenRequest,
    is_current_program: bool = Query(
        True, description="現在のプログラムかどうか", alias="isCurrentProgram"
    ),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SUPPORTER.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
    authorization: HTTPAuthorizationCredentials = Depends(JWTBearer()),
):
    return MasterKartenService.create_master_karten(
        is_current_program=is_current_program,
        body=body,
        current_user=current_user,
        authorization=authorization,
    )


@router.get(
    "/select-box",
    tags=["MasterKarten"],
    status_code=status.HTTP_200_OK,
    response_model=List[GetMasterKartenSelectBoxResponse],
    description="""
マスターカルテからラインナップ・顧客セグメント・業界セグメントのセレクトボックスを取得する。

## 利用PF API
[GetCodeMaster API](https://platform-dev-api-docs.s3.ap-northeast-1.amazonaws.com/api.html#tag/Search/paths/~1params/get)

## 処理定義
GetCodeMaster APIを実行し、取得したデータをフロントエンドに返す。
""",
)
def get_master_karten_select_box(
    _=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SUPPORTER.key,
                UserRoleType.CUSTOMER.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
    authorization: HTTPAuthorizationCredentials = Depends(JWTBearer()),
):
    return MasterKartenService.get_master_karten_select_box(authorization=authorization)


@router.get(
    "/{npf_project_id}",
    tags=["MasterKarten"],
    status_code=status.HTTP_200_OK,
    response_model=GetMasterKartenByIdResponse,
    description="""
指定したマスターカルテを取得します。

## 利用PF API
- [GetProjectById](https://platform-dev-api-docs.s3.ap-northeast-1.amazonaws.com/api.html#tag/Project/paths/~1projects/get)
- [GetProjects](https://platform-dev-api-docs.s3.ap-northeast-1.amazonaws.com/api.html#tag/Project/paths/~1projects/get)
## PP利用テーブル
| テーブル名 | ファセット| CRUD | 備考 |
| ----- | ----- | ----- | ----- |
| 一般ユーザーテーブル(PP) | 一般ユーザー情報  | R | 権限制御のために利用 |

## 処理定義
- 権限制御をPPで行う
- 商談IDを元にNPF APIにリクエストを行う
- NPF APIから該当マスターカルテ各項目を取得
- 一般ユーザーテーブルを利用し、最終更新ID(メールアドレス)から最終更新者名を取得
- 各項目をフロントエンドに返す

# その他
- NPFで保存するプログラムレコードは当期支援・次期支援の二種類が存在
- 同画面内で案件カルテ取得APIを叩く
""",
)
def get_master_karten_detail(
    npf_project_id: str = Path(..., description="NPF案件ID"),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SUPPORTER.key,
                UserRoleType.CUSTOMER.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
    authorization: HTTPAuthorizationCredentials = Depends(JWTBearer()),
):
    return MasterKartenService.get_master_karten_by_id(
        current_user=current_user,
        npf_project_id=npf_project_id,
        authorization=authorization,
    )


@router.put(
    "/{npf_program_id}",
    tags=["MasterKarten"],
    status_code=status.HTTP_200_OK,
    response_model=PutMasterKartenByIdResponse,
    description="""
プログラムIDを利用して指定したマスターカルテを更新します。

## 利用PF API
- [UpdateProgramById](https://platform-dev-api-docs.s3.ap-northeast-1.amazonaws.com/api.html#tag/Program/paths/~1programs~1%7BprogramId%7D/put)
## PP利用テーブル
| テーブル名 | ファセット| CRUD | 備考 |
| ----- | ----- | ----- | ----- |
| 一般ユーザーテーブル(PP) | 一般ユーザー情報  | R | 権限制御のために利用 |

# 処理定義
* 権限制御をPPで行う
* PF案件IDを元にNPF APIに更新リクエストを行う
* 各項目をフロントエンドに返す

# その他
- NPFで保存するプログラムレコードは当期支援・次期支援の二種類が存在
""",
)
def update_master_karten_detail(
    body: PutMasterKartenByIdRequest,
    npf_program_id: str = Path(..., description="NPFプログラムID"),
    is_current_program: bool = Query(
        True, description="現在のプログラムかどうか", alias="isCurrentProgram"
    ),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SUPPORTER.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
    authorization: HTTPAuthorizationCredentials = Depends(JWTBearer()),
):
    return MasterKartenService.update_master_karten_program(
        is_current_program=is_current_program,
        current_user=current_user,
        npf_program_id=npf_program_id,
        body=body,
        authorization=authorization,
    )


@router.get(
    "/npf-id/{pp_project_id}",
    tags=["MasterKarten"],
    description="PP案件IDからNPF案件IDを取得します。",
    response_model=GetNpfProjectIdResponse,
    status_code=status.HTTP_200_OK,
)
def get_npf_project_id(
    pp_project_id: str = Path(..., description="PP案件ID"),
    current_user: AuthUser = Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SUPPORTER.key,
                UserRoleType.SALES.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.CUSTOMER.key,
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
