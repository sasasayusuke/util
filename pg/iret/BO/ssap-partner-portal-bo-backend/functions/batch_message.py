class Message:
    """メッセージ管理クラス"""

    class AutomaticLinkCheckError:
        """自動連携処理のバリデーションエラーメッセージ"""

        REQUIRED_ERROR = "{column_name}を入力してください。"
        FORMAT_ERROR = "「{format_value}」形式で入力してください。"
        VALUE_ERROR = "{value}を入力してください。"
        NOT_EXIST_ERROR = "{column_name}が存在しません。"
        DISABLED_ERROR = "無効なユーザーが設定されています。"
        EMAIL_FORMAT_ERROR = "正しい形式のメールアドレスを入力してください。"
        DATETIME_ERROR = "正しい日時を入力してください。"
        DATE_ERROR = "正しい日付を入力してください。"
        DATA_LENGTH_OVER_ERROR = "{value}文字以内で入力してください。"

    class ImportExecuteError:
        """自動連携処理のエラーメッセージ"""

        EXECUTE_ERROR = "取引先・案件情報自動連携処理で内部処理エラーが発生しました。"
