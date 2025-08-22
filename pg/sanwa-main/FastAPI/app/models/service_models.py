from pydantic import BaseModel, validator
from typing import Dict, Any, Union, List, Set
from datetime import datetime
from app.services.service_factory import SERVICE_INFO
from app.core.exceptions import ValidationError

class ServiceRequest(BaseModel):
    """サービスリクエストモデル"""
    category: str
    title: str
    button: str
    user: str
    opentime: datetime
    params: Dict[str, Any]

    @validator('category')
    def validate_category(cls, v: str) -> str:
        """カテゴリのバリデーション - 存在チェック"""
        if v not in SERVICE_INFO:
            valid_categories = list(SERVICE_INFO.keys())
            raise ValidationError(
                f"無効なカテゴリです: {v}\n"
                f"有効なカテゴリ: {', '.join(valid_categories)}"
            )
        return v

    @validator('title')
    def validate_title(cls, v: str, values: Dict) -> str:
        """タイトルのバリデーション - カテゴリに対するタイトルの存在チェック"""
        category = values.get('category')
        if not category:
            raise ValidationError("カテゴリが指定されていません。")

        if v not in SERVICE_INFO.get(category, {}):
            valid_titles = list(SERVICE_INFO[category].keys())
            raise ValidationError(
                f"カテゴリ {category} に対して無効なタイトルです: {v}\n"
                f"有効なタイトル: {', '.join(valid_titles)}"
            )
        return v

    @validator('button')
    def validate_button(cls, v: str, values: Dict) -> str:
        """ボタンのバリデーション - カテゴリ/タイトルに対して有効なボタンかチェック"""
        category = values.get('category')
        title = values.get('title')
        if not category or not title:
            raise ValidationError("カテゴリおよびタイトルが指定されていません。")

        service_config = SERVICE_INFO.get(category, {}).get(title, {})
        valid_buttons = list(service_config.get('buttons', {}).keys())
        if v not in valid_buttons:
            raise ValidationError(
            f"サービス {category}/{title} に対して無効なボタンです: {v}\n"
                f"利用可能なボタン: {', '.join(valid_buttons)}"
            )
        return v