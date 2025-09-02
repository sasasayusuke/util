import uno
from com.sun.star.beans import PropertyValue
import os
import sys
from pathlib import Path
import subprocess
import time
import psutil
import socket

def kill_existing_soffice():
    """既存のsofficeプロセスを完全に終了する"""
    killed = False
    for proc in psutil.process_iter(['name', 'pid']):
        try:
            pname = proc.info['name'].lower()
            if 'soffice' in pname or 'libreoffice' in pname:
                proc = psutil.Process(proc.info['pid'])
                proc.terminate()
                proc.wait(timeout=3)  # プロセスの終了を待つ
                killed = True
        except (psutil.NoSuchProcess, psutil.AccessDenied, psutil.TimeoutExpired):
            continue

    if killed:
        time.sleep(3)  # 完全な終了を待つ

def start_libreoffice_service():
    """LibreOfficeをサービスモードで起動する"""
    # まず既存のプロセスをクリーンアップ
    kill_existing_soffice()

    # LibreOfficeの実行パスを探す
    possible_paths = [
        r"C:\Program Files\LibreOffice\program\soffice.exe",
        r"C:\Program Files (x86)\LibreOffice\program\soffice.exe",
    ]

    soffice_path = None
    for path in possible_paths:
        if os.path.exists(path):
            soffice_path = path
            break

    if not soffice_path:
        raise FileNotFoundError("LibreOfficeが見つかりません")

    print(f"LibreOfficeを起動します: {soffice_path}")

    # 一時プロファイルディレクトリを作成
    temp_profile_dir = os.path.join(os.environ.get('TEMP', ''), f'libreoffice_profile_{os.getpid()}')
    os.makedirs(temp_profile_dir, exist_ok=True)

    # パスの正規化
    profile_url = temp_profile_dir.replace('\\', '/')

    # サービスモードで起動（新しいプロファイルを使用）
    cmd = [
        soffice_path,
        '-env:UserInstallation=file:///' + profile_url,
        '--headless',
        '--nologo',
        '--nofirststartwizard',
        '--norestore',
        '--invisible',
        '--accept=socket,host=localhost,port=2002;urp;StarOffice.ServiceManager'
    ]

    try:
        process = subprocess.Popen(
            cmd,
            stdout=subprocess.PIPE,
            stderr=subprocess.PIPE,
            universal_newlines=True
        )

        # 起動待機
        time.sleep(5)  # 待機時間を増やす

        if process.poll() is not None:
            stdout, stderr = process.communicate()
            raise RuntimeError(f"LibreOffice起動失敗\nSTDOUT: {stdout}\nSTDERR: {stderr}")

        return process, temp_profile_dir

    except Exception as e:
        raise RuntimeError(f"LibreOffice起動エラー: {str(e)}")

def connect_to_libreoffice(max_attempts=3, delay=3):
    """LibreOfficeへの接続を確立する"""
    for attempt in range(max_attempts):
        try:
            localContext = uno.getComponentContext()
            resolver = localContext.ServiceManager.createInstanceWithContext(
                "com.sun.star.bridge.UnoUrlResolver", localContext)

            # 接続文字列を変更
            ctx = resolver.resolve(
                "uno:socket,host=localhost,port=2002;urp;StarOffice.ServiceManager"
            )

            smgr = ctx.ServiceManager
            desktop = smgr.createInstanceWithContext("com.sun.star.frame.Desktop", ctx)

            if desktop:
                print("LibreOfficeへの接続に成功しました")
                return desktop

        except Exception as e:
            print(f"接続試行 {attempt + 1}/{max_attempts} 失敗: {str(e)}")
            if attempt < max_attempts - 1:
                print(f"{delay}秒後に再試行します...")
                time.sleep(delay)
            else:
                raise ConnectionError(f"LibreOffice接続失敗（{max_attempts}回試行）: {str(e)}")

def save_with_password(input_file, output_dir, output_file_name, password):
    """Excelファイルにパスワードを設定して保存する"""
    if not os.path.exists(input_file):
        raise FileNotFoundError(f"入力ファイルが見つかりません: {input_file}")

    if not os.path.exists(output_dir):
        raise NotADirectoryError(f"出力ディレクトリが存在しません: {output_dir}")

    if not password or len(password) < 4:
        raise ValueError("パスワードは4文字以上必要です")

    libreoffice_process = None
    temp_profile_dir = None

    try:
        print("LibreOfficeサービスを開始します...")
        libreoffice_process, temp_profile_dir = start_libreoffice_service()

        desktop = connect_to_libreoffice(max_attempts=5, delay=3)

        input_url = uno.systemPathToFileUrl(str(Path(input_file).absolute()))
        output_path = Path(output_dir) / output_file_name
        output_url = uno.systemPathToFileUrl(str(output_path.absolute()))

        print(f"ファイルを開いています: {input_file}")
        document = desktop.loadComponentFromURL(input_url, "_blank", 0, ())

        if not document:
            raise RuntimeError("ドキュメントを開けませんでした")

        try:
            print("パスワード保護を設定中...")
            properties = [
                PropertyValue("FilterName", 0, "Calc MS Excel 2007 XML", 0),
                PropertyValue("Password", 0, password, 0),
            ]
            document.storeAsURL(output_url, tuple(properties))
            print(f"ファイルを保存しました: {output_path}")
            return True

        finally:
            document.close(True)
            print("ドキュメントを閉じました")

    except Exception as e:
        print(f"エラーが発生しました: {str(e)}", file=sys.stderr)
        return False

    finally:
        if libreoffice_process:
            print("LibreOfficeサービスを終了します...")
            libreoffice_process.terminate()
            try:
                libreoffice_process.wait(timeout=5)
            except subprocess.TimeoutExpired:
                libreoffice_process.kill()
            print("LibreOfficeサービスを終了しました")

        # 一時プロファイルディレクトリの削除
        if temp_profile_dir and os.path.exists(temp_profile_dir):
            try:
                import shutil
                shutil.rmtree(temp_profile_dir, ignore_errors=True)
            except:
                pass

def main():
    """メイン処理"""
    input_file = r"C:\Users\NT-210174\Desktop\old\ex.xlsx"
    output_dir = r"C:\Users\NT-210174\Desktop"
    output_file_name = "ex_protected.xlsx"
    password = "mypassword"

    try:
        success = save_with_password(input_file, output_dir, output_file_name, password)
        if not success:
            sys.exit(1)
    except Exception as e:
        print(f"致命的なエラーが発生しました: {e}", file=sys.stderr)
        sys.exit(1)

if __name__ == "__main__":
    main()