#!/usr/bin/env python
import os
import sys
import subprocess
import logging
import time
import signal
from datetime import datetime
from pathlib import Path

def run_fastapi(env_config=None):
    """
    FastAPIアプリケーションをセットアップして起動します

    """
    # 環境変数のデフォルト設定
    default_config = {
		# 環境変数の選択
        # "ENVIRONMENT": "local",
        "ENVIRONMENT": "development",
		# "ENVIRONMENT": "test-development-backup",
		# "ENVIRONMENT": "product",

        "APP_ROOT": r"C:\SVN\三和\trunk\src\sanwa-main\FastAPI",
        "PLEASANTER_HOST": "192.168.10.54",
        "PLEASANTER_API_KEY": "e66695678fd2e310b17a79a86632f841a83a97f4c95d39774921a839ef1d36dd184715250c60a54897f42f875a4967afb49b77dc24a69fb3ee4d7fde7db210ce",
        "DB_HOST": "192.168.10.54",
        "DB_PORT": "1433",

        "DB_USER": "sa",
        "DB_PASSWORD": "1qaz!QAZ",

		# DB名の選択
        # "DB_NAME": "SanwaSDB",
		"DB_NAME": "SanwaSDB_241016",
		# "DB_NAME": "SanwaOrg",


        "WKHTMLTOPDF_PATH": r"C:\Program Files\wkhtmltopdf\bin\wkhtmltopdf.exe"

    }

    # 指定された設定があれば、デフォルト設定を上書き
    if env_config:
        default_config.update(env_config)

    # 環境変数を設定
    for key, value in default_config.items():
        os.environ[key] = value

    # パス設定
    log_dir = Path("./log")
    log_path = log_dir / f"startup_{datetime.now().strftime('%Y%m%d')}.log"
    req_path = Path("./requirements.txt")

    # ポート設定
    port = "8000"

    # CPUコア数に基づいてワーカー数を設定
    import multiprocessing
    cpu_count = multiprocessing.cpu_count()

    # アプリケーションのディレクトリに移動
    os.chdir(os.environ["APP_ROOT"])

    # ログディレクトリの確認
    log_dir.mkdir(exist_ok=True)

    # ログ設定
    logging.basicConfig(
        level=logging.INFO,
        format="%(asctime)s - %(levelname)s - %(message)s",
        handlers=[
            logging.FileHandler(log_path),
            logging.StreamHandler()
        ]
    )

    # 起動時刻の記録
    logging.info(f"サービス起動時刻: {datetime.now().strftime('%Y-%m-%d %H:%M:%S')}")
    logging.info(f"起動モード: {os.environ['ENVIRONMENT']}")

    try:
        # 仮想環境のセットアップ
        venv_path = Path("./venv")

        # 既存の仮想環境を削除（PowerShellスクリプトと同じ動作）
        if venv_path.exists():
            logging.info("既存の仮想環境を削除します...")
            import shutil
            shutil.rmtree(venv_path)

        # 仮想環境の作成
        logging.info("仮想環境を新規作成しています...")
        subprocess.run([sys.executable, "-m", "pip", "install", "--upgrade", "pip"], check=True)
        subprocess.run([sys.executable, "-m", "venv", "venv"], check=True)

        # 仮想環境のPythonパスを取得
        python_path = str(venv_path / "Scripts" / "python.exe") if sys.platform == "win32" else str(venv_path / "bin" / "python")

        # 環境に基づいてパッケージをインストール
        if os.environ["ENVIRONMENT"] in ['development', 'local', 'test-development-backup']:
            logging.info("開発環境用の基本パッケージをインストールしています...")
            subprocess.run([python_path, "-m", "pip", "install", "wheel"], check=True)
            subprocess.run([python_path, "-m", "pip", "install", "debugpy"], check=True)
        elif os.environ["ENVIRONMENT"] in ['product']:
            # 本番環境では特別なパッケージは不要
            pass
        else:
            logging.error(f"不明なENVIRONMENTが設定されています: {os.environ['ENVIRONMENT']}。アプリケーションを終了します。")
            sys.exit(1)

        # requirements.txtからパッケージをインストール
        if req_path.exists():
            logging.info("requirements.txtからパッケージをインストールしています...")
            subprocess.run([python_path, "-m", "pip", "install", "-r", str(req_path)], check=True)
        else:
            logging.error(f"requirements.txtが見つかりません: {req_path}")
            sys.exit(1)

        # Pythonバージョンをログに記録
        logging.info("Pythonバージョンを記録中...")
        python_version = subprocess.check_output([python_path, "--version"], text=True)
        logging.info(python_version)

        # pip listをログに記録
        logging.info("インストール済みのpipパッケージを記録中...")
        pip_list = subprocess.check_output([python_path, "-m", "pip", "list"], text=True)
        logging.info(pip_list)

        print("終了するには Ctrl+C を押してください")

        # FastAPIの起動
        if os.environ["ENVIRONMENT"] in ['development', 'local', 'test-development-backup']:
            logging.info("開発モードでアプリケーションを起動しています...")
            cmd = [
                python_path, "-m", "debugpy", "--listen", "0.0.0.0:5678",
                "-m", "uvicorn", "app.main:app",
                "--host", "0.0.0.0",
                "--port", port,
                "--reload",
                "--timeout-keep-alive", "120"
            ]
            subprocess.run(cmd, check=True)

        elif os.environ["ENVIRONMENT"] in ['product']:
            workers = min(16, cpu_count * 2)  # CPUコア数の2倍か16の小さい方

            logging.info(f"本番モードでアプリケーションを起動しています... (ワーカー数: {workers})")

            # 自動再起動機能付きの実行
            restart_count = 0
            max_restarts = 5

            def handle_sigterm(sig, frame):
                logging.info(f"シグナル {sig} を受信しました。アプリケーションを終了します。")
                sys.exit(0)

            signal.signal(signal.SIGTERM, handle_sigterm)
            signal.signal(signal.SIGINT, handle_sigterm)

            while True:
                start_time = datetime.now()
                logging.info("本番モードでアプリケーションを起動しています...")

                cmd = [
                    python_path, "-m", "uvicorn", "app.main:app",
                    "--host", "0.0.0.0",
                    "--port", port,
                    "--workers", str(workers),
                    "--timeout-keep-alive", "120"
                ]

                process = subprocess.Popen(cmd)
                return_code = process.wait()

                end_time = datetime.now()
                run_duration = (end_time - start_time).total_seconds()

                # 短時間で終了した場合の処理
                if run_duration < 60:
                    restart_count += 1
                    if restart_count >= max_restarts:
                        logging.error(f"{max_restarts}回連続で短時間終了しました。問題を調査してください。")
                        time.sleep(300)
                        restart_count = 0
                    else:
                        logging.warning(f"アプリケーションが短時間で終了しました（コード: {return_code}）。再起動します... ({restart_count}/{max_restarts})")
                        time.sleep(5)
                else:
                    restart_count = 0
                    logging.warning(f"アプリケーションが終了しました（コード: {return_code}）。再起動します...")
                    time.sleep(5)
    except Exception as e:
        logging.exception(f"致命的なエラーが発生しました: {e}")
        sys.exit(1)


if __name__ == "__main__":
    # 基本設定で実行
    run_fastapi()