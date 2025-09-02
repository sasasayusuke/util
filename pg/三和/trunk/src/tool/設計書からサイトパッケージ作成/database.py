import os
from sqlalchemy import create_engine, text
from sqlalchemy.orm import sessionmaker, scoped_session
from sqlalchemy.ext.declarative import declarative_base
from threading import Lock

DB_HOST = "192.168.10.54"
DB_PORT = "1433"
DB_USER = "sa"
DB_PASSWORD = "1qaz!QAZ"
DB_SANWA_PLEASANTER_NAME = "Implem.Pleasanter"

class DatabaseManager:
    def __init__(self):
        self.username = DB_USER
        self.password = DB_PASSWORD
        self.host = DB_HOST
        self.port = DB_PORT
        self.database = DB_SANWA_PLEASANTER_NAME
        self.dialect = "mssql"
        self.driver = "pymssql"
        self.charset_type = "utf8"
        self.engine = None
        self.session = None
        self.base = declarative_base()

    def initialize(self):
        self.db_url = self._construct_db_url()
        print(f"Connecting to: {self.db_url}")  # デバッグ用出力
        self.engine = create_engine(self.db_url, echo=True)
        self.session = scoped_session(sessionmaker(autocommit=False, autoflush=False, bind=self.engine))

    def get_db(self):
        if self.session is None:
            raise Exception("session が初期化されていません。initialize() を呼び出してください")
        return self.session()

    def _construct_db_url(self):
        return f"{self.dialect}+{self.driver}://{self.username}:{self.password}@{self.host}:{self.port}/{self.database}?charset={self.charset_type}"

class DatabaseManagerFactory:
    _instance = None
    _lock = Lock()
    pleasanter_db: DatabaseManager

    def __new__(cls):
        with cls._lock:
            if cls._instance is None:
                cls._instance = super(DatabaseManagerFactory, cls).__new__(cls)
                cls._instance.pleasanter_db = DatabaseManager()
                cls._instance.pleasanter_db.initialize()
        return cls._instance

    @classmethod
    def get_instance(cls):
        if cls._instance is None:
            cls._instance = cls()
        return cls._instance

    def get_pleasanter_db(self):
        return self.pleasanter_db.get_db()

def create_view(db_session, view_name, view_query):
    try:
        drop_view_query = f"IF OBJECT_ID('{view_name}', 'V') IS NOT NULL DROP VIEW {view_name}"
        db_session.execute(text(drop_view_query))

        create_view_query = f"CREATE VIEW {view_name} AS {view_query}"
        db_session.execute(text(create_view_query))

        db_session.commit()
    except Exception as e:
        db_session.rollback()
        # TODO ログに出るようにする
        # raise Exception(f"ビューの作成中にエラーが発生しました: {str(e)}")
        print(f"ビューの作成中にエラーが発生しました: {str(e)}")
