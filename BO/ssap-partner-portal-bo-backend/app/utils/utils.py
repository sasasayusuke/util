from distutils.util import strtobool


class Utils:
    @staticmethod
    def is_int(target: str) -> bool:
        """数値チェック"""
        try:
            int(target)
        except ValueError:
            return False
        else:
            return True

    @staticmethod
    def is_float(target: str) -> bool:
        """数値チェック"""
        try:
            float(target)
        except ValueError:
            return False
        else:
            return True

    @staticmethod
    def is_bool(target: str) -> bool:
        """真理値チェック"""
        try:
            strtobool(target)
        except ValueError:
            return False
        else:
            return True
