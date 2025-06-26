import pytest
from app.auth.jwt import JWTAuthorizationCredentials, JWTBearer
from app.models.user import CognitoIdIndex, UserModel
from app.resources.const import DataType, UserRoleType


@pytest.fixture
def auth_user(mocker):
    """認証済みユーザのモック化を行う"""

    # #######################################
    # テスト用データ
    # #######################################
    user = UserModel(
        id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
        data_type=DataType.USER,
        name="山田太郎",
        email="taro.yamada@example.com",
        job="部長",
        solver_corporation_id=None,
        supporter_organization_id=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
        organization_name="IST",
        cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
        role="supporter_mgr",
    )

    # #######################################
    # モック作成
    # #######################################

    user_model = [user]
    mock = mocker.patch.object(CognitoIdIndex, "query")
    mock.return_value = iter(user_model)

    credentials = JWTAuthorizationCredentials(
        jwt_token="",
        header={},
        claims={
            "sub": "itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
            "cognito:username": "taro.yamada@example.com",
            "email": "taro.yamada@example.com",
        },
        signature="",
        message="",
    )
    mock = mocker.patch.object(JWTBearer, "__call__")
    mock.return_value = credentials


@pytest.fixture
def auth_customer_user(mocker):
    """認証済みユーザのモック化を行う"""

    # #######################################
    # テスト用データ
    # #######################################
    user = UserModel(
        id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
        data_type=DataType.USER,
        name="山田太郎",
        email="taro.yamada@example.com",
        customer_id="106a3144-9650-4a34-8a23-3b02f3b9aeac",
        customer_name="取引先株式会社",
        job="部長",
        solver_corporation_id=None,
        organization_name="コンサルティング事業部",
        project_ids=["886a3144-9650-4a34-8a23-3b02f3b9aeac"],
        cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
        agreed=True,
        role=UserRoleType.CUSTOMER.key,
        version=1,
    )

    # #######################################
    # モック作成
    # #######################################

    user_model = [user]
    mock = mocker.patch.object(CognitoIdIndex, "query")
    mock.return_value = iter(user_model)

    credentials = JWTAuthorizationCredentials(
        jwt_token="",
        header={},
        claims={
            "sub": "itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
            "cognito:username": "taro.yamada@example.com",
            "email": "taro.yamada@example.com",
        },
        signature="",
        message="",
    )
    mock = mocker.patch.object(JWTBearer, "__call__")
    mock.return_value = credentials


@pytest.fixture
def auth_supporter_user(mocker):
    """認証済みユーザのモック化を行う"""

    # #######################################
    # テスト用データ
    # #######################################
    user = UserModel(
        id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
        data_type=DataType.USER,
        name="山田太郎",
        email="taro.yamada@example.com",
        company="テスト株式会社",
        solver_corporation_id=None,
        supporter_organization_id=["556a3144-9650-4a34-8a23-3b02f3b9aeac"],
        is_input_man_hour=True,
        project_ids=[
            "886a3144-9650-4a34-8a23-3b02f3b9aeac",
            "126a3144-9650-4a34-8a23-3b02f3b9aeac",
            "236a3144-9650-4a34-8a23-3b02f3b9aeac",
        ],
        cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
        agreed=True,
        role=UserRoleType.SUPPORTER.key,
        version=1,
    )

    # #######################################
    # モック作成
    # #######################################

    user_model = [user]
    mock = mocker.patch.object(CognitoIdIndex, "query")
    mock.return_value = iter(user_model)

    credentials = JWTAuthorizationCredentials(
        jwt_token="",
        header={},
        claims={
            "sub": "itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
            "cognito:username": "taro.yamada@example.com",
            "email": "taro.yamada@example.com",
        },
        signature="",
        message="",
    )
    mock = mocker.patch.object(JWTBearer, "__call__")
    mock.return_value = credentials


@pytest.fixture
def auth_supporter_mgr_user(mocker):
    """認証済みユーザのモック化を行う"""

    # #######################################
    # テスト用データ
    # #######################################
    user = UserModel(
        id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
        data_type=DataType.USER,
        name="山田太郎",
        email="taro.yamada@example.com",
        company="テスト株式会社",
        solver_corporation_id=None,
        supporter_organization_id=["556a3144-9650-4a34-8a23-3b02f3b9aeac"],
        is_input_man_hour=True,
        project_ids=[
            "886a3144-9650-4a34-8a23-3b02f3b9aeac",
            "126a3144-9650-4a34-8a23-3b02f3b9aeac",
            "236a3144-9650-4a34-8a23-3b02f3b9aeac",
        ],
        cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
        agreed=True,
        role=UserRoleType.SUPPORTER_MGR.key,
        version=1,
    )

    # #######################################
    # モック作成
    # #######################################

    user_model = [user]
    mock = mocker.patch.object(CognitoIdIndex, "query")
    mock.return_value = iter(user_model)

    credentials = JWTAuthorizationCredentials(
        jwt_token="",
        header={},
        claims={
            "sub": "itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
            "cognito:username": "taro.yamada@example.com",
            "email": "taro.yamada@example.com",
        },
        signature="",
        message="",
    )
    mock = mocker.patch.object(JWTBearer, "__call__")
    mock.return_value = credentials


@pytest.fixture
def auth_sales_user(mocker):
    """認証済みユーザのモック化を行う"""

    # #######################################
    # テスト用データ
    # #######################################
    user = UserModel(
        id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
        data_type=DataType.USER,
        name="山田太郎",
        email="taro.yamada@example.com",
        company="テスト株式会社",
        solver_corporation_id=None,
        is_input_man_hour=True,
        project_ids=[
            "886a3144-9650-4a34-8a23-3b02f3b9aeac",
            "346820d3-618b-4ddb-bde9-030eb6441630",
            "126a3144-9650-4a34-8a23-3b02f3b9aeac"
        ],
        cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
        agreed=True,
        role=UserRoleType.SALES.key,
        version=1,
    )

    # #######################################
    # モック作成
    # #######################################

    user_model = [user]
    mock = mocker.patch.object(CognitoIdIndex, "query")
    mock.return_value = iter(user_model)

    credentials = JWTAuthorizationCredentials(
        jwt_token="",
        header={},
        claims={
            "sub": "itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
            "cognito:username": "taro.yamada@example.com",
            "email": "taro.yamada@example.com",
        },
        signature="",
        message="",
    )
    mock = mocker.patch.object(JWTBearer, "__call__")
    mock.return_value = credentials


@pytest.fixture
def auth_sales_mgr_user(mocker):
    """認証済みユーザのモック化を行う"""

    # #######################################
    # テスト用データ
    # #######################################
    user = UserModel(
        id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
        data_type=DataType.USER,
        name="山田太郎",
        email="taro.yamada@example.com",
        company="テスト株式会社",
        solver_corporation_id=None,
        is_input_man_hour=True,
        project_ids=["886a3144-9650-4a34-8a23-3b02f3b9aeac"],
        cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
        agreed=True,
        role=UserRoleType.SALES_MGR.key,
        version=1,
    )

    # #######################################
    # モック作成
    # #######################################

    user_model = [user]
    mock = mocker.patch.object(CognitoIdIndex, "query")
    mock.return_value = iter(user_model)

    credentials = JWTAuthorizationCredentials(
        jwt_token="",
        header={},
        claims={
            "sub": "itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
            "cognito:username": "taro.yamada@example.com",
            "email": "taro.yamada@example.com",
        },
        signature="",
        message="",
    )
    mock = mocker.patch.object(JWTBearer, "__call__")
    mock.return_value = credentials


@pytest.fixture
def auth_apt_user(mocker):
    """認証済みユーザのモック化を行う"""

    # #######################################
    # テスト用データ
    # #######################################
    user = UserModel(
        id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
        data_type=DataType.USER,
        name="山田太郎",
        email="taro.yamada@example.com",
        company="テスト株式会社",
        solver_corporation_id=None,
        is_input_man_hour=True,
        project_ids=[],
        cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
        agreed=True,
        role=UserRoleType.APT.key,
        version=1,
    )

    # #######################################
    # モック作成
    # #######################################

    user_model = [user]
    mock = mocker.patch.object(CognitoIdIndex, "query")
    mock.return_value = iter(user_model)

    credentials = JWTAuthorizationCredentials(
        jwt_token="",
        header={},
        claims={
            "sub": "itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
            "cognito:username": "taro.yamada@example.com",
            "email": "taro.yamada@example.com",
        },
        signature="",
        message="",
    )
    mock = mocker.patch.object(JWTBearer, "__call__")
    mock.return_value = credentials


@pytest.fixture
def auth_solver_staff_user(mocker):
    """認証済みユーザのモック化を行う"""

    # #######################################
    # テスト用データ
    # #######################################
    user = UserModel(
        id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
        data_type=DataType.USER,
        name="山田太郎",
        email="taro.yamada@example.com",
        company="テスト株式会社",
        solver_corporation_id="906a3144-9650-4a34-8a23-3b02f3b9a999",
        is_input_man_hour=True,
        project_ids=[],
        cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
        agreed=True,
        role=UserRoleType.SOLVER_STAFF.key,
        version=1,
    )

    # #######################################
    # モック作成
    # #######################################

    user_model = [user]
    mock = mocker.patch.object(CognitoIdIndex, "query")
    mock.return_value = iter(user_model)

    credentials = JWTAuthorizationCredentials(
        jwt_token="",
        header={},
        claims={
            "sub": "itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
            "cognito:username": "taro.yamada@example.com",
            "email": "taro.yamada@example.com",
        },
        signature="",
        message="",
    )
    mock = mocker.patch.object(JWTBearer, "__call__")
    mock.return_value = credentials
