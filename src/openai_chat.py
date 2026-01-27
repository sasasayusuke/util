"""OpenAI APIを使った対話型チャットスクリプト"""

import os
from dotenv import load_dotenv
from openai import OpenAI

load_dotenv()  # .envファイルを読み込む

DEFAULT_MODEL = "gpt-4o-mini"


def get_model() -> str:
    """環境変数AI_MODELからモデル名を取得する。"""
    return os.environ.get("AI_MODEL", DEFAULT_MODEL)


def get_client() -> OpenAI:
    """OpenAIクライアントを取得する。環境変数からAPIキーを読み込む。"""
    api_key = os.environ.get("OPENAI_API_KEY")
    if not api_key:
        raise ValueError(
            "OPENAI_API_KEY環境変数が設定されていません。\n"
            "設定例: export OPENAI_API_KEY='your-api-key'"
        )
    return OpenAI(api_key=api_key)


def chat(
    client: OpenAI,
    messages: list[dict[str, str]],
    model: str | None = None,
) -> str:
    """メッセージを送信してレスポンスを取得する。"""
    if model is None:
        model = get_model()
    response = client.chat.completions.create(
        model=model,
        messages=messages,  # type: ignore
    )
    return response.choices[0].message.content or ""


def interactive_chat(model: str | None = None) -> None:
    """対話型チャットを開始する。"""
    client = get_client()
    messages: list[dict[str, str]] = []
    if model is None:
        model = get_model()

    print(f"OpenAI Chat ({model})")
    print("終了するには 'quit' または 'exit' と入力してください。")
    print("-" * 50)

    while True:
        try:
            user_input = input("\nYou: ").strip()
        except (KeyboardInterrupt, EOFError):
            print("\n終了します。")
            break

        if not user_input:
            continue

        if user_input.lower() in ("quit", "exit"):
            print("終了します。")
            break

        messages.append({"role": "user", "content": user_input})

        try:
            response = chat(client, messages, model)
            print(f"\nAssistant: {response}")
            messages.append({"role": "assistant", "content": response})
        except Exception as e:
            print(f"\nエラーが発生しました: {e}")
            messages.pop()  # 失敗したメッセージを削除


def single_query(prompt: str, model: str | None = None) -> str:
    """単発のクエリを実行する。"""
    client = get_client()
    messages = [{"role": "user", "content": prompt}]
    return chat(client, messages, model)


if __name__ == "__main__":
    import sys

    if len(sys.argv) > 1:
        # コマンドライン引数があれば単発クエリとして実行
        query = " ".join(sys.argv[1:])
        print(single_query(query))
    else:
        # 引数がなければ対話モード
        interactive_chat()
