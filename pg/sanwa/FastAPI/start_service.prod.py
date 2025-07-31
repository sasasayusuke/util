from start_service import run_fastapi

# 本番環境設定
production_config = {
    "ENVIRONMENT": "product",
    "APP_ROOT": r"C:\SDT\FastAPI",
    "PLEASANTER_HOST": "192.168.0.7",
    "PLEASANTER_API_KEY": "e66695678fd2e310b17a79a86632f841a83a97f4c95d39774921a839ef1d36dd184715250c60a54897f42f875a4967afb49b77dc24a69fb3ee4d7fde7db210ce",
    "DB_HOST": "192.168.0.7",
    "DB_PORT": "1433",
    "DB_NAME": "SanwaSDB",
    "DB_USER": "sa",
    "DB_PASSWORD": "yjy0354847577",
}




# 本番環境で実行
run_fastapi(production_config)