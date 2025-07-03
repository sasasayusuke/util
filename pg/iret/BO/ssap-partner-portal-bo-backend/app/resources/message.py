class Message:
    """メッセージ管理クラス"""

    class General:
        """基本メッセージ"""

        HELLO = "Go Serverless v1.0! Your function executed successfully!"

    class Error:
        """エラーメッセージ"""

        NO_PERMISSION_TO_ACCESS_RESOURCE = "No permission to access the resource."
        SIGN_IN_REQUIRED = "Sign-in required."

    class ImportCheckError:
        """CSVファイルインポート事前チェック時のエラーメッセージ"""

        REQUIRED_ERROR = "{column_name}を入力してください。"
        FORMAT_ERROR = "{column_name}は「{format_value}」形式で入力してください。"
        VALUE_ERROR = "{column_name}は{value}を入力してください。"
        NOT_EXIST_ERROR = "{column_name}が存在しません。"
        DISABLED_ERROR = "{column_name}に無効なユーザーが設定されています。"
        EMAIL_FORMAT_ERROR = "{column_name}には正しい形式のメールアドレスを入力してください。"
        DATETIME_ERROR = "{column_name}には正しい日時を入力してください。"
        DATE_ERROR = "{column_name}には正しい日付を入力してください。"

    class ImportExecuteError:
        """CSVファイルインポート取込時のエラーメッセージ"""

        EXECUTE_ERROR = "取込処理でエラーが発生しました。"
