from pydantic import BaseModel
from typing import Optional, Dict, Any

class RecordCreateRequest(BaseModel):
    pleasanter_table_name: str
    new_data: Dict[str, Any]

class RecordUpdateRequest(BaseModel):
    pleasanter_table_name: str
    old_data: Dict[str, Any]
    new_data: Dict[str, Any]

class RecordDeleteRequest(BaseModel):
    pleasanter_table_name: str
    old_data: Dict[str, Any]
