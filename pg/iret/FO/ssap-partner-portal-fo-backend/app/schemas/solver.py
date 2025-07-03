from datetime import datetime
from typing import List

from app.schemas.base import CustomBaseModel
from app.resources.const import (
    GetSolversSortType,
)

from fastapi import HTTPException, Query
from fastapi import status as api_status
from pydantic import Field, root_validator


class PatchSolverStatusByIdQuery(CustomBaseModel):
    """個人ソルバー変更クエリクラス"""

    version: int = Query(..., title="ロックキー（楽観ロック制御）", example=1)


class GetSolversQuery(CustomBaseModel):
    """個人ソルバー一覧取得クエリクラス"""

    id: str = Query(
        ..., title="法人ソルバーID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    # service_typeが期待する値
    # 全て:all, ソルバー候補:solver_candidate, 個人ソルバー: solver, 認定済みソルバー: certificated_solver
    solver_type: str = Query(None, title="ソルバー種別", example="solver")
    name: str = Query(None, title="個人ソルバー名", example="山田太郎")
    # sexが期待する値
    # 全て:all, 男性:man, 女性:woman, 未設定:not_set
    sex: str = Query(None, title="性別", example="man")
    # certification_statusが期待する値
    # 全て:all, 申請前:before, 申請中:during
    certification_status: str = Query(
        None, title="個人ソルバー申請状況", example="during"
    )
    # operating_statusが期待する値
    # 全て:all, 未稼働:not_working, 稼働中:working, 休止中:inactive
    operating_status: str = Query(None, title="稼働状況", example="working")
    sort: GetSolversSortType = Query(GetSolversSortType.CREATE_AT_DESC, title="ソート")
    offset_page: int = Query(1, title="リストの中で何ページ目を取得するか", example=1)
    limit: int = Query(None, title="最大取得件数", example=20)


class SolverInfoForGetSolvers(CustomBaseModel):
    """GetSolversResponse.solversのList要素"""

    id: str = Field(
        ..., title="個人ソルバーID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    name: str = Field(..., title="個人ソルバー名", example="山田太郎")
    corporate_id: str = Field(
        None, title="法人内ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    sex: str = Field(None, title="性別", example="man")
    age: int = Field(None, title="年齢", example=50)
    birth_day: str = Field(None, title="生年月日", example="1974/10/23")
    specialized_themes: str = Field(
        None, title="専門テーマ", example="デジタルマーケティング"
    )
    operating_status: str = Field(None, title="稼働状況", example="working")
    provided_operating_rate: int = Field(None, title="提供稼働率（今月）", example=10)
    provided_operating_rate_next: int = Field(
        None, title="提供稼働率（来月）", example=20
    )
    operation_prospects_month_after_next: str = Field(
        None, title="再来月以降の稼働見込み", example="再来月以降の稼働あり"
    )
    price_per_person_month: int = Field(None, title="人月単価（上限）", example=20000)
    price_per_person_month_lower: int = Field(
        None, title="人月単価（下限）", example=10000
    )
    hourly_rate: int = Field(None, title="時間単価（上限）", example=20000)
    hourly_rate_lower: int = Field(None, title="時間単価（下限）", example=10000)
    registration_status: str = Field(None, title="登録状態", example="new")
    price_and_operating_rate_update_at: datetime = Field(
        None, title="稼働率・単価最終更新日時", example="2020-10-23T03:21:39.356872Z"
    )
    version: int = Field(None, title="ロックキー（楽観ロック制御）", example=1)
    is_solver: bool = Field(False, title="個人ソルバーか", example="True")


class GetSolversResponse(CustomBaseModel):
    """個人ソルバー一覧取得レスポンスクラス"""

    offset_page: int = Field(None, title="リストの総数内で何ページ目か", example=1)
    total: int = Field(..., title="件数", example=10, ge=0)
    solvers: List[SolverInfoForGetSolvers] = Field(..., title="個人ソルバー情報")


class SolverApplications(CustomBaseModel):
    """案件情報"""

    id: str = Field(None, title="ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")
    name: str = Field(None, title="案件名", example="ソルバー案件")
    project_code: str = Field(None, title="案件コード")


class IssueMap50(CustomBaseModel):
    """課題マップ50"""

    id: str = Field(None, title="ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")
    category: str = Field(None, title="カテゴリ", example="人材育成")
    name: str = Field(
        None, title="課題名", example="新規事業開発を担うリーダーが不足している"
    )


class FacePhoto(CustomBaseModel):
    """個人ソルバー画像"""

    file_name: str = Field(None, title="ファイル名", example="ファイルA")
    # path: 任意. 空文字を登録を防ぐため、空文字チェック(1文字以上)
    path: str = Field(
        None, title="パス", example="http://www.example.com", min_length=1
    )


class Resume(CustomBaseModel):
    """添付資料"""

    file_name: str = Field(None, title="ファイル名", example="ファイルA")
    # path: 任意. 空文字を登録を防ぐため、空文字チェック(1文字以上)
    path: str = Field(
        None, title="パス", example="http://www.example.com", min_length=1
    )


class Screening(CustomBaseModel):
    """スクリーニング項目"""

    evaluation: bool = Field(None, title="評価", example=False)
    evidence: str = Field(None, title="エビデンス", example="特になし")


class GetSolverByIdResponse(CustomBaseModel):
    """個人ソルバー情報取得レスポンスクラス"""

    id: str = Field(
        ..., title="個人ソルバーID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    name: str = Field(..., title="個人ソルバー名", example="山田太郎")
    name_kana: str = Field(None, title="個人ソルバーふりがな", example="ヤマダタロウ")
    solver_applications: List[SolverApplications] = Field(None, title="案件情報")
    title: str = Field(None, title="役職", example="リーダー")
    email: str = Field(None, title="連絡先メールアドレス", example="yamada@example.com")
    phone: str = Field(None, title="電話番号", example="00-0000-0000")
    issue_map50: List[str] = Field(None, title="課題マップ50", example=[
        "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
        "99cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    ])
    corporate_id: str = Field(
        None, title="法人内ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    sex: str = Field(..., title="性別", example="man")
    birth_day: str = Field(None, title="生年月日", example="2020/10/23")
    operating_status: str = Field(None, title="稼働状況", example="working")
    face_photo: FacePhoto = Field(None, title="個人ソルバー画像")
    resume: List[Resume] = Field(None, title="添付資料")
    academic_background: str = Field(None, title="学歴", example="大学卒")
    work_history: str = Field(None, title="職歴", example="コンサル10年目")
    is_consulting_firm: bool = Field(
        ..., title="コンサルティングファーム経験有無", example=True
    )
    specialized_themes: str = Field(
        None, title="専門テーマ", example="デジタルマーケティング"
    )
    main_achievements: str = Field(None, title="主な実績", example="特になし")
    provided_operating_rate: int = Field(None, title="提供稼働率（今月）", example=10)
    provided_operating_rate_next: int = Field(
        None, title="提供稼働率（来月）", example=20
    )
    operation_prospects_month_after_next: str = Field(
        None, title="再来月以降の稼働見込み", example="再来月以降の稼働あり"
    )
    price_per_person_month: int = Field(None, title="人月単価（上限）", example=20000)
    price_per_person_month_lower: int = Field(
        None, title="人月単価（下限）", example=10000
    )
    hourly_rate: int = Field(None, title="時間単価（上限）", example=20000)
    hourly_rate_lower: int = Field(None, title="時間単価（下限）", example=10000)
    english_level: str = Field(None, title="英語レベル", example="reading_and_writing")
    tsi_areas: List[str] = Field(
        None, title="東証33業種経験/対応可能領域", example=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"]
    )
    screening_1: Screening = Field(None, title="スクリーニング項目1")
    screening_2: Screening = Field(None, title="スクリーニング項目2")
    screening_3: Screening = Field(None, title="スクリーニング項目3")
    screening_4: Screening = Field(None, title="スクリーニング項目4")
    screening_5: Screening = Field(None, title="スクリーニング項目5")
    screening_6: Screening = Field(None, title="スクリーニング項目6")
    screening_7: Screening = Field(None, title="スクリーニング項目7")
    screening_8: Screening = Field(None, title="スクリーニング項目8")
    criteria_1: str = Field(
        None, title="クライテリア項目1エビデンス", example="特になし"
    )
    criteria_2: str = Field(
        None, title="クライテリア項目2エビデンス", example="特になし"
    )
    criteria_3: str = Field(
        None, title="クライテリア項目3エビデンス", example="特になし"
    )
    criteria_4: str = Field(
        None, title="クライテリア項目4エビデンス", example="特になし"
    )
    criteria_5: str = Field(
        None, title="クライテリア項目5エビデンス", example="特になし"
    )
    criteria_6: str = Field(
        None, title="クライテリア項目6エビデンス", example="特になし"
    )
    criteria_7: str = Field(
        None, title="クライテリア項目7エビデンス", example="特になし"
    )
    criteria_8: str = Field(
        None, title="クライテリア項目8エビデンス", example="特になし"
    )
    notes: str = Field(None, title="備考", example="特になし")
    is_solver: bool = Field(False, title="個人ソルバーか", example="True")
    registration_status: str = Field(..., title="登録状態", example="new")
    create_id: str = Field(
        ..., title="登録者ID", example="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7"
    )
    create_user_name: str = Field(None, title="登録者名", example="ソニー太郎")
    create_at: datetime = Field(
        ..., title="登録日時", example="2020-10-23T03:21:39.356872Z"
    )
    price_and_operating_rate_update_at: datetime = Field(
        None, title="稼働率・単価最終更新日時", example="2020-10-23T03:21:39.356872Z"
    )
    price_and_operating_rate_update_by: str = Field(
        None, title="稼働率・単価最終更新者", example="ソニー太郎"
    )
    update_id: str = Field(
        None, title="最終更新者ID", example="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7"
    )
    update_user_name: str = Field(None, title="最終更新者名", example="ソニー太郎")
    update_at: datetime = Field(
        None, title="最終更新日時", example="2020-10-23T03:21:39.356872Z"
    )
    version: int = Field(None, title="ロックキー（楽観ロック制御）", example=1)
    file_key_id: str = Field(..., title="S3のファイルを特定するためのID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")


class SolverInfoForCreateSolver(CustomBaseModel):
    """CreateSolverRequest.solvers_infoのList要素"""

    id: str = Field(None, title="ソルバーID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")
    mode: str = Field(..., title="モード", example="create_candidate")
    is_registered_solver: bool = Field(..., title="既存ソルバーか", example=False)
    name: str = Field(..., title="個人ソルバー名", example="山田太郎")
    name_kana: str = Field(None, title="個人ソルバーふりがな", example="ヤマダタロウ")
    solver_application_id: str = Field(None, title="案件ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")
    solver_application_name: str = Field(None, title="案件名", example="ソルバー案件")
    title: str = Field(None, title="役職", example="リーダー")
    email: str = Field(None, title="連絡先メールアドレス", example="yamada@example.com")
    phone: str = Field(None, title="電話番号", example="00-0000-0000")
    issue_map50: List[str] = Field(None, title="課題マップ50", example=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"])
    corporate_id: str = Field(
        ..., title="法人内ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    sex: str = Field(..., title="性別", example="man")
    birth_day: str = Field(None, title="生年月日", example="2020/10/23")
    operating_status: str = Field(None, title="稼働状況", example="working")
    face_photo: FacePhoto = Field(None, title="個人ソルバー画像")
    resume: List[Resume] = Field(None, title="添付資料")
    academic_background: str = Field(None, title="学歴", example="大学卒")
    work_history: str = Field(None, title="職歴", example="コンサル10年目")
    is_consulting_firm: bool = Field(
        ..., title="コンサルティングファーム経験有無", example=True
    )
    specialized_themes: str = Field(
        None, title="専門テーマ", example="デジタルマーケティング"
    )
    main_achievements: str = Field(None, title="主な実績", example="特になし")
    provided_operating_rate: int = Field(None, title="提供稼働率（今月）", example=10)
    provided_operating_rate_next: int = Field(
        None, title="提供稼働率（来月）", example=20
    )
    operation_prospects_month_after_next: str = Field(
        None, title="再来月以降の稼働見込み", example="再来月以降の稼働あり"
    )
    price_per_person_month: int = Field(None, title="人月単価（上限）", example=20000)
    price_per_person_month_lower: int = Field(
        None, title="人月単価（下限）", example=10000
    )
    hourly_rate: int = Field(None, title="時間単価（上限）", example=20000)
    hourly_rate_lower: int = Field(None, title="時間単価（下限）", example=10000)
    english_level: str = Field(None, title="英語レベル", example="reading_and_writing")
    tsi_areas: List[str] = Field(
        None, title="東証33業種経験/対応可能領域", example=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"]
    )
    screening_1: Screening = Field(None, title="スクリーニング項目1")
    screening_2: Screening = Field(None, title="スクリーニング項目2")
    screening_3: Screening = Field(None, title="スクリーニング項目3")
    screening_4: Screening = Field(None, title="スクリーニング項目4")
    screening_5: Screening = Field(None, title="スクリーニング項目5")
    screening_6: Screening = Field(None, title="スクリーニング項目6")
    screening_7: Screening = Field(None, title="スクリーニング項目7")
    screening_8: Screening = Field(None, title="スクリーニング項目8")
    criteria_1: str = Field(
        None, title="クライテリア項目1エビデンス", example="特になし"
    )
    criteria_2: str = Field(
        None, title="クライテリア項目2エビデンス", example="特になし"
    )
    criteria_3: str = Field(
        None, title="クライテリア項目3エビデンス", example="特になし"
    )
    criteria_4: str = Field(
        None, title="クライテリア項目4エビデンス", example="特になし"
    )
    criteria_5: str = Field(
        None, title="クライテリア項目5エビデンス", example="特になし"
    )
    criteria_6: str = Field(
        None, title="クライテリア項目6エビデンス", example="特になし"
    )
    criteria_7: str = Field(
        None, title="クライテリア項目7エビデンス", example="特になし"
    )
    criteria_8: str = Field(
        None, title="クライテリア項目8エビデンス", example="特になし"
    )
    notes: str = Field(None, title="備考", example="特になし")
    registration_status: str = Field(..., title="登録状態", example="new")
    file_key_id: str = Field(..., title="S3のファイルを特定するためのID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")

    @root_validator
    def validate_required(cls, v):
        mode = v.get("mode")
        name_kana = v.get("name_kana")
        solver_application_id = v.get("solver_application_id")
        solver_application_name = v.get("solver_application_name")
        title = v.get("title")
        issue_map50 = v.get("issue_map50")
        birth_day = v.get("birth_day")
        operating_status = v.get("operating_status")
        face_photo = v.get("face_photo")
        academic_background = v.get("academic_background")
        work_history = v.get("work_history")
        specialized_themes = v.get("specialized_themes")
        main_achievements = v.get("main_achievements")
        provided_operating_rate = v.get("provided_operating_rate")
        provided_operating_rate_next = v.get("provided_operating_rate_next")
        operation_prospects_month_after_next = v.get("operation_prospects_month_after_next")
        price_per_person_month = v.get("price_per_person_month")
        price_per_person_month_lower = v.get("price_per_person_month_lower")
        hourly_rate = v.get("hourly_rate")
        hourly_rate_lower = v.get("hourly_rate_lower")
        english_level = v.get("english_level")
        tsi_areas = v.get("tsi_areas")
        screening_1 = v.get("screening_1")
        screening_2 = v.get("screening_2")
        screening_3 = v.get("screening_3")
        screening_4 = v.get("screening_4")
        screening_5 = v.get("screening_5")
        screening_6 = v.get("screening_6")
        screening_7 = v.get("screening_7")
        screening_8 = v.get("screening_8")
        criteria_1 = v.get("criteria_1")
        criteria_2 = v.get("criteria_2")
        criteria_3 = v.get("criteria_3")
        criteria_4 = v.get("criteria_4")
        criteria_5 = v.get("criteria_5")
        criteria_6 = v.get("criteria_6")
        criteria_7 = v.get("criteria_7")
        criteria_8 = v.get("criteria_8")
        registration_status = v.get("registration_status")

        # 新規ソルバー候補申請の場合
        if mode == "create_candidate":
            if not name_kana:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="`name_kana` is required.",
                )

            if not solver_application_id:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="`solver_application_id` is required.",
                )

            if not solver_application_name:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="`solver_application_name` is required.",
                )

            if not birth_day:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="`birth_day` is required.",
                )

            if not work_history:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="`work_history` is required.",
                )

            if not specialized_themes:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="`specialized_themes` is required.",
                )

            if not main_achievements:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="`main_achievements` is required.",
                )
        # 新規個人ソルバー登録申請にて申請するボタンを押下した場合
        if mode == "create_solver" and registration_status == "saved":
            if not name_kana:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="`name_kana` is required.",
                )

            if not title:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="`title` is required.",
                )

            if not issue_map50:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="`issue_map50` is required.",
                )

            if not birth_day:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="`birth_day` is required.",
                )

            if not operating_status:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="`operating_status` is required.",
                )

            if not face_photo:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="`face_photo` is required.",
                )

            if not academic_background:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="`academic_background` is required.",
                )

            if not work_history:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="`work_history` is required.",
                )

            if not specialized_themes:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="`specialized_themes` is required.",
                )

            if not main_achievements:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="`main_achievements` is required.",
                )

            # 提供稼働率・人月単価・時間単価の場合は0を許容するため、is Noneを用いて必須チェックを行う
            if provided_operating_rate is None:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="`provided_operating_rate` is required.",
                )

            if provided_operating_rate_next is None:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="`provided_operating_rate_next` is required.",
                )

            if not operation_prospects_month_after_next:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="`operation_prospects_month_after_next` is required.",
                )

            if price_per_person_month is None:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="`price_per_person_month` is required.",
                )

            if price_per_person_month_lower is None:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="`price_per_person_month_lower` is required.",
                )

            if hourly_rate is None:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="`hourly_rate` is required.",
                )

            if hourly_rate_lower is None:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="`hourly_rate_lower` is required.",
                )

            if not english_level:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="`english_level` is required.",
                )

            if not tsi_areas:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="`tsi_areas` is required.",
                )

            if screening_1.evaluation and not screening_1.evidence:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="if `screening_1.evaluation` is True, `screening_1.evidence` is required.",
                )

            if screening_2.evaluation and not screening_2.evidence:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="if `screening_2.evaluation` is True, `screening_2.evidence` is required.",
                )

            if screening_3.evaluation and not screening_3.evidence:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="if `screening_3.evaluation` is True, `screening_3.evidence` is required.",
                )

            if screening_4.evaluation and not screening_4.evidence:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="if `screening_4.evaluation` is True, `screening_4.evidence` is required.",
                )

            if screening_5.evaluation and not screening_5.evidence:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="if `screening_5.evaluation` is True, `screening_5.evidence` is required.",
                )

            if screening_6.evaluation and not screening_6.evidence:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="if `screening_6.evaluation` is True, `screening_6.evidence` is required.",
                )

            if screening_7.evaluation and not screening_7.evidence:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="if `screening_7.evaluation` is True, `screening_7.evidence` is required.",
                )

            if screening_8.evaluation and not screening_8.evidence:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="if `screening_8.evaluation` is True, `screening_8.evidence` is required.",
                )

            if not criteria_1:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="`criteria_1` is required.",
                )

            if not criteria_2:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="`criteria_2` is required.",
                )

            if not criteria_3:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="`criteria_3` is required.",
                )

            if not criteria_4:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="`criteria_4` is required.",
                )

            if not criteria_5:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="`criteria_5` is required.",
                )

            if not criteria_6:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="`criteria_6` is required.",
                )

            if not criteria_7:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="`criteria_7` is required.",
                )

            if not criteria_8:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="`criteria_8` is required.",
                )

        return v

    @root_validator
    def validate_price_per_person_month(cls, v):
        price_per_person_month = v.get("price_per_person_month")
        price_per_person_month_lower = v.get("price_per_person_month_lower")
        if price_per_person_month and price_per_person_month_lower:
            if price_per_person_month < price_per_person_month_lower:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="pricePerPersonMonth is greater than or equal to pricePerPersonMonthLower.",
                )

        return v

    @root_validator
    def validate_hourly_rate(cls, v):
        hourly_rate = v.get("hourly_rate")
        hourly_rate_lower = v.get("hourly_rate_lower")
        if hourly_rate and hourly_rate_lower:
            if hourly_rate < hourly_rate_lower:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="hourlyRate is greater than or equal to hourlyRateLower.",
                )

        return v


class CreateSolverRequest(CustomBaseModel):
    """新規ソルバー候補申請・新規個人ソルバー申請リクエストクラス"""

    solvers_info: List[SolverInfoForCreateSolver] = Field(..., title="登録するソルバーの情報")


class DeleteSolverByIdQuery(CustomBaseModel):
    """個人ソルバー削除クエリクラス"""

    version: int = Field(None, title="ロックキー（楽観ロック制御）", example=1)


class SolverUtilizationRate(CustomBaseModel):
    id: str = Field(
        ..., title="個人ソルバーID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    name: str = Field(
        ..., title="個人ソルバー名", example="ソルバー太郎"
    )
    provided_operating_rate: int = Field(None, title="提供稼働率（今月）", example=50)
    provided_operating_rate_next: int = Field(
        None, title="提供稼働率（来月）", example=100
    )
    operation_prospects_month_after_next: str = Field(
        None, title="再来月以降の稼働見込み", example="再来月以降の稼働あり"
    )
    price_per_person_month: int = Field(None, title="人月単価（上限）", example=1000000)
    price_per_person_month_lower: int = Field(
        None, title="人月単価（下限）", example=800000
    )
    hourly_rate: int = Field(None, title="時間単価（上限）", example=100000)
    hourly_rate_lower: int = Field(None, title="時間単価（下限）", example=80000)

    @root_validator
    def validate_price_per_person(cls, v):
        price_per_person_month = v.get("price_per_person_month")
        price_per_person_month_lower = v.get("price_per_person_month_lower")
        if price_per_person_month and price_per_person_month_lower:
            if price_per_person_month < price_per_person_month_lower:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="pricePerPersonMonth is greater than or equal to pricePerPersonMonthLower.",
                )

        return v

    @root_validator
    def validate_hourly_rate(cls, v):
        hourly_rate = v.get("hourly_rate")
        hourly_rate_lower = v.get("hourly_rate_lower")
        if hourly_rate and hourly_rate_lower:
            if hourly_rate < hourly_rate_lower:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="hourlyRate is greater than or equal to hourlyRateLower.",
                )

        return v


class UpdateSolverUtilizationRateRequest(CustomBaseModel):
    """ソルバーの稼働率を更新するリクエストクラス"""

    utilization_rate: List[SolverUtilizationRate]


class UpdateSolverUtilizationRateResponse(CustomBaseModel):
    """ソルバーの稼働率を更新するレスポンスクラス"""

    deleted: List[str] = Field(None, title="削除済みのユーザーリスト", example=["ソルバー太郎"])


class UpdateSolverUtilizationRateQuery(CustomBaseModel):
    """個人ソルバー変更クエリクラス"""

    version: int = Query(..., title="ロックキー（楽観ロック制御）", example=1)


class UpdateSolverByIdQuery(CustomBaseModel):
    """ソルバー更新クエリクラス"""

    version: int = Query(..., title="ロックキー（楽観ロック制御）", example=1)


class UpdateSolverByIdRequest(CustomBaseModel):
    """ソルバー更新リクエストクラス"""

    mode: str = Field(..., title="モード", example="update_solver")
    name: str = Field(None, title="個人ソルバー名", example="山田太郎")
    name_kana: str = Field(None, title="個人ソルバーふりがな", example="ヤマダタロウ")
    solver_application_id: str = Field(None, title="案件ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")
    solver_application_name: str = Field(None, title="案件名", example="ソルバー案件")
    delete_solver_application_ids: List[str] = Field(None, title="削除案件IDリスト", example=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d", "99cbe2ed-f44c-4a1c-9408-c67b0ca9990a"])
    title: str = Field(None, title="役職", example="リーダー")
    email: str = Field(None, title="連絡先メールアドレス", example="yamada@example.com")
    phone: str = Field(None, title="電話番号", example="00-0000-0000")
    issue_map50: List[str] = Field(
        None,
        title="課題マップ50",
        example=[
            "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            "99cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
        ],
    )
    corporate_id: str = Field(
        ..., title="法人内ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    sex: str = Field(..., title="性別", example="man")
    birth_day: str = Field(None, title="生年月日", example="2020/10/23")
    operating_status: str = Field(None, title="稼働状況", example="working")
    face_photo: FacePhoto = Field(None, title="個人ソルバー画像")
    resume: List[Resume] = Field(None, title="添付資料")
    academic_background: str = Field(None, title="学歴", example="大学卒")
    work_history: str = Field(None, title="職歴", example="コンサル10年目")
    is_consulting_firm: bool = Field(
        ..., title="コンサルティングファーム経験有無", example=True
    )
    specialized_themes: str = Field(
        None, title="専門テーマ", example="デジタルマーケティング"
    )
    main_achievements: str = Field(None, title="主な実績", example="特になし")
    provided_operating_rate: int = Field(0, title="提供稼働率（今月）", example=10)
    provided_operating_rate_next: int = Field(
        0, title="提供稼働率（来月）", example=20
    )
    operation_prospects_month_after_next: str = Field(
        None, title="再来月以降の稼働見込み", example="再来月以降の稼働あり"
    )
    price_per_person_month: int = Field(0, title="人月単価（上限）", example=20000)
    price_per_person_month_lower: int = Field(
        0, title="人月単価（下限）", example=10000
    )
    hourly_rate: int = Field(0, title="時間単価（上限）", example=20000)
    hourly_rate_lower: int = Field(0, title="時間単価（下限）", example=10000)
    english_level: str = Field(None, title="英語レベル", example="reading_and_writing")
    tsi_areas: List[str] = Field(
        None, title="東証33業種経験/対応可能領域", example=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"]
    )
    screening_1: Screening = Field(None, title="スクリーニング項目1")
    screening_2: Screening = Field(None, title="スクリーニング項目2")
    screening_3: Screening = Field(None, title="スクリーニング項目3")
    screening_4: Screening = Field(None, title="スクリーニング項目4")
    screening_5: Screening = Field(None, title="スクリーニング項目5")
    screening_6: Screening = Field(None, title="スクリーニング項目6")
    screening_7: Screening = Field(None, title="スクリーニング項目7")
    screening_8: Screening = Field(None, title="スクリーニング項目8")
    criteria_1: str = Field(
        None, title="クライテリア項目1エビデンス", example="特になし"
    )
    criteria_2: str = Field(
        None, title="クライテリア項目2エビデンス", example="特になし"
    )
    criteria_3: str = Field(
        None, title="クライテリア項目3エビデンス", example="特になし"
    )
    criteria_4: str = Field(
        None, title="クライテリア項目4エビデンス", example="特になし"
    )
    criteria_5: str = Field(
        None, title="クライテリア項目5エビデンス", example="特になし"
    )
    criteria_6: str = Field(
        None, title="クライテリア項目6エビデンス", example="特になし"
    )
    criteria_7: str = Field(
        None, title="クライテリア項目7エビデンス", example="特になし"
    )
    criteria_8: str = Field(
        None, title="クライテリア項目8エビデンス", example="特になし"
    )
    notes: str = Field(None, title="備考", example="特になし")
    is_solver: bool = Field(..., title="個人ソルバーか", example=True)
    registration_status: str = Field(..., title="登録状態", example="certificated")

    @root_validator
    def validate_for_update_solver(cls, v):
        mode = v.get("mode")
        title = v.get("title")
        issue_map50 = v.get("issue_map50")
        operating_status = v.get("operating_status")
        face_photo = v.get("face_photo")
        academic_background = v.get("academic_background")
        main_achievements = v.get("main_achievements")
        provided_operating_rate = v.get("provided_operating_rate")
        provided_operating_rate_next = v.get("provided_operating_rate_next")
        operation_prospects_month_after_next = v.get("operation_prospects_month_after_next")
        price_per_person_month = v.get("price_per_person_month")
        price_per_person_month_lower = v.get("price_per_person_month_lower")
        hourly_rate = v.get("hourly_rate")
        hourly_rate_lower = v.get("hourly_rate_lower")
        english_level = v.get("english_level")
        tsi_areas = v.get("tsi_areas")
        registration_status = v.get("registration_status")
        name = v.get("name")
        name_kana = v.get("name_kana")
        birth_day = v.get("birth_day")
        work_history = v.get("work_history")
        specialized_themes = v.get("specialized_themes")

        # 個人ソルバー登録申請（一時保存）以外の場合
        if mode != "temporary_save_solver":
            if not name:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="name is required.",
                )
            if not name_kana:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="name_kana is required.",
                )
            if not birth_day:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="birth_day is required.",
                )
            if not work_history:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="work_history is required.",
                )
            if not specialized_themes:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="specialized_themes is required.",
                )

        # 個人ソルバー登録申請または個人ソルバー更新の場合
        if (mode == "register_solver" and registration_status == "saved") or mode == "update_solver":
            if not title:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="title is required.",
                )
            if not issue_map50:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="issue_map50 is required.",
                )
            if not operating_status:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="operating_status is required.",
                )
            if not face_photo:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="face_photo is required.",
                )
            if not academic_background:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="academic_background is required.",
                )
            if not main_achievements:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="main_achievements is required.",
                )
            # 提供稼働率・人月単価・時間単価の場合は0を許容するため、is Noneを用いて必須チェックを行う
            if provided_operating_rate is None:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="provided_operating_rate is required.",
                )
            if provided_operating_rate_next is None:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="provided_operating_rate_next is required.",
                )
            if not operation_prospects_month_after_next:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="operation_prospects_month_after_next is required.",
                )
            if price_per_person_month is None:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="price_per_person_month is required.",
                )
            if price_per_person_month_lower is None:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="price_per_person_month_lower is required.",
                )
            if hourly_rate is None:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="hourly_rate is required.",
                )
            if hourly_rate_lower is None:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="hourly_rate_lower is required.",
                )
            if not english_level:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="english_level is required.",
                )
            if not tsi_areas:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="tsi_areas is required.",
                )

            # スクリーニング項目、クライテリア項目の検証
            cls._valid_screening_and_criteria(v)

        if mode == "update_candidate" and registration_status == "saved":
            # スクリーニング項目、クライテリア項目の検証
            cls._valid_screening_and_criteria(v)

        return v

    @classmethod
    def _valid_screening_and_criteria(cls, v):
        # スクリーニング項目、クライテリア項目の検証
        for i in range(1, 9):
            screening = v.get(f"screening_{i}")
            if screening.evaluation and not screening.evidence:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail=f"if `screening_{i}.evaluation` is True, `screening_{i}.evidence` is required.",
                )

            criteria = v.get(f"criteria_{i}")
            if not criteria:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail=f"criteria_{i} is required.",
                )
