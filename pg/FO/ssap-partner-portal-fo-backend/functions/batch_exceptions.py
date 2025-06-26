class ExitHandler(Exception):
    """処理の途中でハンドラーを終了させる場合に使用するException"""

    def __init__(self, value=None) -> None:
        super().__init__()
        self.value = value
