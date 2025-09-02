
# 環境変数の設定
$env:ENVIRONMENT = "product"

$env:APP_ROOT = "C:\SDT\FastAPI"

$env:PLEASANTER_HOST = "192.168.0.9"

$env:PLEASANTER_API_KEY = "e66695678fd2e310b17a79a86632f841a83a97f4c95d39774921a839ef1d36dd184715250c60a54897f42f875a4967afb49b77dc24a69fb3ee4d7fde7db210ce"

$env:DB_HOST = "192.168.0.9"

$env:DB_PORT = "1433"

$env:DB_NAME = "SanwaSDB"

$env:DB_USER = "sa"

$env:DB_PASSWORD = "yjy0354847577"

# wkhtmltopdf のパス
$env:WKHTMLTOPDF_PATH = "C:\Program Files\wkhtmltopdf\bin\wkhtmltopdf.exe"

# パス設定
$logPath = ".\log\log_start.txt"
$reqPath = ".\requirements.txt"
$certPath = ".\certs"

# SSL証明書のパス設定
$enableHttps = $false
$sslCertFile = "$certPath\live\example.com\fullchain.pem"
$sslKeyFile = "$certPath\live\example.com\privkey.pem"
$sslParams = if ($enableHttps) {
    "--ssl-certfile '$sslCertFile' --ssl-keyfile '$sslKeyFile'"
} else {
    ""
}

# ポート設定
$port = if ($enableHttps) { "443" } else { "8000" }

# 起動時刻を取得
$startTime = Get-Date -Format "yyyy-MM-dd HH:mm:ss"

# FastAPIプロジェクトディレクトリに移動
Set-Location -Path $env:APP_ROOT

# ログファイルの初期化
if (Test-Path $logPath) {
    Remove-Item -Force $logPath
}
New-Item -Path $logPath -ItemType File | Out-Null

# 起動時刻の記録
$message = "サービス起動時刻: $startTime"
Write-Host $message
Add-Content -Path $logPath -Value $message

# 環境設定の表示
$message = "ENVIRONMENT is: $env:ENVIRONMENT"
Write-Host $message
Add-Content -Path $logPath -Value $message

$message = "HTTPS is: $(if ($enableHttps) { 'enabled' } else { 'disabled' })"
Write-Host $message
Add-Content -Path $logPath -Value $message

# 仮想環境の作成と基本パッケージのインストール
$message = "仮想環境をセットアップ中..."
Write-Host $message
Add-Content -Path $logPath -Value $message
if (Test-Path ".\venv") {
    Remove-Item -Recurse -Force ".\venv"
}
try {
    python -m pip install --upgrade pip
    python -m venv venv
    .\venv\Scripts\Activate.ps1
} catch {
    $message = "仮想環境の作成またはアクティベート時にエラーが発生しました: $_"
    Write-Host $message
    Add-Content -Path $logPath -Value $message
    exit 1
}

# pipを再度アップグレード
$message = "pipをアップグレードしています..."
Write-Host $message
Add-Content -Path $logPath -Value $message
try {
    python -m pip install --upgrade pip
} catch {
    $message = "pipのアップグレードに失敗しました: $_"
    Write-Host $message
    Add-Content -Path $logPath -Value $message
    exit 1
}

# 環境モードに応じた処理
if ($env:ENVIRONMENT -eq 'development') {
    try {
        $message = "開発環境用の基本パッケージをインストールしています..."
        Write-Host $message
        Add-Content -Path $logPath -Value $message

        python -m pip install wheel
        python -m pip install debugpy
    } catch {
        $message = "処理中にエラーが発生しました: $_"
        Write-Host $message
        Add-Content -Path $logPath -Value $message
        exit 1
    }
} elseif ($env:ENVIRONMENT -eq 'product') {
} else {
    $message = "不明なENVIRONMENTが設定されています: $env:ENVIRONMENT。アプリケーションを終了します。"
    Write-Host $message
    Add-Content -Path $logPath -Value $message
    exit 1
}

# HTTPS使用に応じた処理
if ($enableHttps) {
    try {
        $message = "Certbotをインストールしています......"
        Write-Host $message
        Add-Content -Path $logPath -Value $message

        python -m pip install certbot

        # Certbotで証明書を更新
        $message = "CertbotでSSL証明書を更新しています..."
        Write-Host $message
        Add-Content -Path $logPath -Value $message

        certbot renew --quiet --config-dir $certPath

        if ($LASTEXITCODE -ne 0) {
            throw "CertbotのSSL証明書更新に失敗しました。"
        }

        $message = "CertbotのSSL証明書更新が成功しました。保存先: $certPath"
        Write-Host $message
        Add-Content -Path $logPath -Value $message
    } catch {
        $message = "処理中にエラーが発生しました: $_"
        Write-Host $message
        Add-Content -Path $logPath -Value $message
        exit 1
    }
}

# requirements.txtからパッケージをインストール
if (Test-Path -Path $reqPath) {
    $message = "requirements.txtからパッケージをインストールしています..."
    Write-Host $message
    Add-Content -Path $logPath -Value $message
    try {
        python -m pip install -r $reqPath
    } catch {
        $message = "requirements.txtのインストールに失敗しました: $_"
        Write-Host $message
        Add-Content -Path $logPath -Value $message
        exit 1
    }
} else {
    $message = "requirements.txtが見つかりません: $reqPath"
    Write-Host $message
    Add-Content -Path $logPath -Value $message
    exit 1
}

# Pythonバージョンとインストール済みパッケージをログに記録
$message = "Pythonバージョンを記録中..."
Write-Host $message
Add-Content -Path $logPath -Value $message
$pythonVersion = python --version
$pythonVersion | Out-File -FilePath $logPath -Append -Encoding UTF8

# pip listを記録
$message = "インストール済みのpipパッケージを記録中..."
Write-Host $message
Add-Content -Path $logPath -Value $message
$pipList = python -m pip list
$pipList | Out-File -FilePath $logPath -Append -Encoding UTF8

Write-Host "終了するには Ctrl+C を押してください"

# FastAPIの起動
if ($env:ENVIRONMENT -eq 'development') {
    $message = "開発モードでアプリケーションを起動しています..."
    Write-Host $message
    Add-Content -Path $logPath -Value $message
    try {
        python -m debugpy --listen 0.0.0.0:5678 -m uvicorn app.main:app --host 0.0.0.0 --port $port --reload $sslParams
    } catch {
        $message = "開発モードでのアプリケーション起動中にエラーが発生しました: $_"
        Write-Host $message
        Add-Content -Path $logPath -Value $message
        exit 1
    }
} elseif ($env:ENVIRONMENT -eq 'product') {
    $message = "本番モードでアプリケーションを起動しています..."
    Write-Host $message
    Add-Content -Path $logPath -Value $message
    try {
        python -m uvicorn app.main:app --host 0.0.0.0 --port $port --workers 9 $sslParams
    } catch {
        $message = "本番モードでのアプリケーション起動中にエラーが発生しました: $_"
        Write-Host $message
        Add-Content -Path $logPath -Value $message
        exit 1
    }
}