// Pleasanter用URL
const HOME_URL 				= "http://192.168.10.54"

//　API用URL
const BASE_URL 				= `${HOME_URL}:8000`
const RENDER_FORM_URL		= `${BASE_URL}/render-form`;
const STORED_PROCEDURE_URL	= `${BASE_URL}/execute-stored-procedure`;
const SQL_URL 				= `${BASE_URL}/execute-sql`;
const SERVICE_URL 			= `${BASE_URL}/execute-service`;
const LOCK_URL 				= `${BASE_URL}/execute-lock`;
const UNLOCK_URL 			= `${BASE_URL}/execute-unlock`;
const GET_TAX_URL           = `${BASE_URL}/execute-get-tax`;

// マスターテーブルIDの定義
const MASTER_TABLES = {
	'物件入力': 3064532,
	'ヤオコー製品変換マスタ': 1841975,
	'三研実績入力': 1841976,
	'仕入先マスタ': 1841977,
	'会社マスタ': 1841978,
	'担当者マスタ': 3064531,
	'科目マスタ': 1841980,
	'税率マスタ': 1841981,
	'請求金額マスタ': 1841982,
	'配送先マスタ': 1841983,
	'ウエルシア物件内容マスタ': 1841984,
	'売上仕入単価マスタ': 1841985,
	'売掛金額マスタ': 1841986,
	'得意先マスタ': 1841987,
	'支払金額マスタ': 1841988,
	'棚番マスタ': 1841989,
	'納入先マスタ': 1841990,
	'補助科目マスタ': 1841991,
	'郵便番号マスタ': 1480503,
	'製品マスタ': 1841992,
	'買掛金額マスタ': 1841993,
	'工事担当マスタ': 1841994,
	'業種按分マスタ': 1841995,
	'三和サービス実績入力': 1841996,
	'ウエルシア物件区分マスタ': 1841997,
	'人数按分マスタ': 3066123,
	'権限設定': 3065964,
	'部署マスタ': 3064556,
	'入金区分マスタ': 2190740,
	'支払区分マスタ': 2190749,
};
// 入力画面IDの定義
const INPUT_GUIS = {
	'仕入明細入力': 3064555,
	'売上明細入力': 3065886
};

// 値からキーを取得する関数
const getTableName = (value) => {
	return Object.entries({ ...MASTER_TABLES, ...INPUT_GUIS }).find(([_, val]) => val === parseInt(value, 10))?.[0] || null;
};

