from abc import ABC, abstractmethod
from datetime import datetime
from typing import Dict, Any

class BaseService(ABC):
    """サービスの抽象基底クラス"""

    @abstractmethod
    def display(self, request, session) -> Dict[str, Any]:
        """印刷処理を行うメソッド"""
        pass

    @abstractmethod
    def execute(self, request, session) -> Dict[str, Any]:
        """実行処理を行うメソッド"""
        pass

