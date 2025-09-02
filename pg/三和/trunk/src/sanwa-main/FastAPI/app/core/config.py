import os
import yaml
from pydantic_settings import BaseSettings
import pythoncom
import win32com.client as win32

class Settings(BaseSettings):

    APP_NAME: str = "Sanwa App"

    ENVIRONMENT: str = os.getenv("ENVIRONMENT")
    APP_ROOT: str = os.getenv("APP_ROOT")

    # Excel設定
    EXCEL_LICENSE: bool = True
    EXCEL_VERSION: str = ""
    EXCEL_BUILD: str = ""
    EXCEL_PATH: str = ""

    WKHTMLTOPDF_PATH: str = os.getenv("WKHTMLTOPDF_PATH")

    # Pleasanter設定
    PLEASANTER_HOST: str = os.getenv("PLEASANTER_HOST")
    PLEASANTER_API_KEY: str = os.getenv("PLEASANTER_API_KEY")
    PLEASANTER_API_VERSION: float = 1.1
    PLEASANTER_INFO: dict = {}

    # データベース設定
    DB_HOST: str = os.getenv("DB_HOST")
    DB_PORT: int = int(os.getenv("DB_PORT"))
    DB_NAME: str = os.getenv("DB_NAME")
    DB_USER: str = os.getenv("DB_USER")
    DB_PASSWORD: str = os.getenv("DB_PASSWORD")

    DB_URL: str = f"mssql+pymssql://{DB_USER}:{DB_PASSWORD}@{DB_HOST}:{DB_PORT}/{DB_NAME}"


    def get_env(self):
        return {
            "ENVIRONMENT": self.ENVIRONMENT,
            "APP_ROOT": self.APP_ROOT,
            "PLEASANTER_HOST": self.PLEASANTER_HOST,
            "PLEASANTER_API_KEY": "******",
            "PLEASANTER_API_VERSION": self.PLEASANTER_API_VERSION,
            "PLEASANTER_INFO": self.PLEASANTER_INFO,
            "DB_HOST": self.DB_HOST,
            "DB_PORT": self.DB_PORT,
            "DB_NAME": self.DB_NAME,
            "DB_USER": self.DB_USER,
            "DB_PASSWORD": "******",
            "EXCEL_LICENSE": self.EXCEL_LICENSE,
            "EXCEL_VERSION": self.EXCEL_VERSION,
            "EXCEL_BUILD": self.EXCEL_BUILD,
            "EXCEL_PATH": self.EXCEL_PATH,
            "WKHTMLTOPDF_PATH": self.WKHTMLTOPDF_PATH,
        }

    def initialize_excel_info(self):
        """Excelの情報を初期化する"""
        try:
            pythoncom.CoInitialize()
            try:
                excel = win32.Dispatch("Excel.Application")
                self.EXCEL_VERSION = excel.Version
                self.EXCEL_BUILD = excel.Build
                self.EXCEL_PATH = excel.Path

            except Exception as e:
                settings.EXCEL_LICENSE = False

            finally:
                if 'excel' in locals():
                    pass
                    # excel.Quit()
        finally:
            pythoncom.CoUninitialize()

    def initialize_pleasanter_info(self):
        """Pleasanterの情報を初期化する"""

        # 環境に対応するYAMLファイルを読み込む
        yaml_path = os.path.join(self.APP_ROOT, 'public', 'pleasanter', '00_SiteSettingTool', f'pleasanter_setting.{self.ENVIRONMENT}.yaml')

        try:
            with open(yaml_path, 'r', encoding='utf-8') as f:
                self.PLEASANTER_INFO = yaml.safe_load(f)

        except FileNotFoundError:
            logger.error(f"設定ファイルが見つかりません: {yaml_path} または {txt_path}")

        except Exception as e:
            logger.error(f"アセット読み込みエラー: {str(e)}")

# インスタンスを生成
settings = Settings()

