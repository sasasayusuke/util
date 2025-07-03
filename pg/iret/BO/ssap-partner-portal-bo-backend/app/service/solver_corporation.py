from typing import List

from fastapi import HTTPException, status

from app.core.common_logging import CustomLogger
from app.models.admin import AdminModel
from app.models.solver_corporation import SolverCorporationModel
from app.resources.const import DataType
from app.resources.const import (
    SuggestSolverCorporationsSortType,
)
from app.schemas.solver_corporation import (
    SuggestSolverCorporationsResponse,
)

logger = CustomLogger.get_logger()


class SolversCorporationService:

    @staticmethod
    def suggest_solver_corporations(
        sort: SuggestSolverCorporationsSortType, disabled: bool, current_user: AdminModel
    ) -> SuggestSolverCorporationsResponse:
        """法人名のサジェスト用データを取得します

        Args:
            sort (SuggestSolverCorporationsSortType): ソート （'name:asc'）
            current_user (Behavior, optional): 認証済みのユーザー

        Returns:
            SuggestSolverCorporationsResponse: 取得結果
        """
        # クエリ条件を指定
        filter_condition = None

        # 無効化されたソルバー法人は取得しない
        if not disabled:
            filter_condition &= SolverCorporationModel.disabled == disabled

        if sort == SuggestSolverCorporationsSortType.NAME_ASC:
            # name昇順で取得
            solver_corporation_iter = SolverCorporationModel.data_type_name_index.query(
                hash_key=DataType.SOLVER_CORPORATION,
                filter_condition=filter_condition,
                scan_index_forward=True,
            )

            # クエリ結果をリスト変数へ格納
            suggest_list: List[SuggestSolverCorporationsResponse] = []
            for solver_corporation in solver_corporation_iter:
                suggest_list.append(
                    SuggestSolverCorporationsResponse(**solver_corporation.attribute_values)
                )
            return suggest_list
        else:
            # 呼出し元でsort項目チェック済の為、以下は発生しない想定
            raise HTTPException(status_code=status.HTTP_500_INTERNAL_SERVER_ERROR)
