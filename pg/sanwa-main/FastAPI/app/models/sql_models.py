from pydantic import BaseModel
from typing import List, Dict
import datetime

class SQLQueryRequest(BaseModel):
    query: str

class SQLQueryResponse(BaseModel):
    message: str
    count: int
    results: List[Dict]

class StoredQueryRequest(BaseModel):
    storedname: str
    params: Dict
    output_params: Dict

class StoredQueryResponse(BaseModel):
    message: str
    count: int
    results: List[List[Dict]]
    output_values: Dict

class lockQueryRequest(BaseModel):
    iDataName:str
    iNumber:int
    iDataCode:str
    iCheck:int
    iPCName:str

class unlockQueryRequest(BaseModel):
    iDataName:str
    iNumber:int
    iDataCode:str
    iPCName:str

class getTaxQueryRequest(BaseModel):
    iDate:datetime.datetime