Option Strict Off
Option Explicit On
Friend Class clsMitumoriH
	'///////////////////////////
	'TD見積クラス（仮）
	'///////////////////////////
	'   2018/10/06  oosawa      isU区分追加
	
	
	'変数
	'見積番号
	Private m_見積番号 As Integer
	
	Private m_担当者CD As Short
	Private m_見積日付 As Object
	Private m_見積件名 As String
	Private m_得意先CD As String
	
	Private m_得意先名1 As String
	Private m_得意先名2 As String
	Private m_得TEL As String
	Private m_得FAX As String
	Private m_得担当者 As String
	Private m_納得意先CD As String
	
	Private m_納入先CD As String
	Private m_納入先名1 As String
	Private m_納入先名2 As String
	Private m_納郵便番号 As String
	Private m_納住所1 As String
	Private m_納住所2 As String
	Private m_納TEL As String
	Private m_納FAX As String
	
	Private m_納担当者 As String
	Private m_納期S As Object
	Private m_納期E As Object
	Private m_備考 As String
	
	Private m_規模金額 As Decimal
	Private m_OPEN日 As Object
	Private m_物件種別 As Short
	Private m_現場名 As String
	Private m_支払条件 As Short
	Private m_支払条件他 As String
	Private m_納期表示 As Short
	Private m_納期表示他 As String
	Private m_見積日出力 As Short
	Private m_有効期限 As Short
	Private m_受注区分 As Short
	Private m_受注日付 As Object
	Private m_大小口区分 As Short
	Private m_出精値引 As Decimal
	''''修正チェック用
	Private m_売上日付 As Object
	Private m_仕入日付 As Object
	'''
	Private m_合計金額 As Decimal
	Private m_原価合計 As Decimal
	Private m_原価率 As Decimal
	Private m_外税額 As Decimal
	'''
	Private m_税集計区分 As Short
	Private m_売上端数 As Short
	Private m_消費税端数 As Short
	'''
	Private m_得意先別見積番号 As Object
	Private m_見積区分 As Short
	
	
	'見積区分
	Private m_isDsp見積区分 As enum見積区分
	'見積区分の種類
	Public Enum enum見積区分 '区分にあわせた数字
		仮見積 = 0
		本見積 = 1
		全て = 2
	End Enum
	
	'社内伝含む・含まない
	Private m_isDsp社内伝 As enum社内伝
	'在庫管理の種類
	Public Enum enum社内伝
		全て = 0
		社内伝以外 = 1
		社内伝のみ = 2
	End Enum
	
	
	Private m_MaxLength As Integer '桁数
	Private m_EOF As Boolean '存在確認
	Private m_Error As Short 'エラー
	
	Private m_Datas() As Object '選択配列
	'//////////////////////////////////////
	'   見積番号
	'//////////////////////////////////////
	
	Public Property 見積番号() As Integer
		Get
			見積番号 = m_見積番号
		End Get
		Set(ByVal Value As Integer)
			m_見積番号 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   担当者CD
	'//////////////////////////////////////
	
	Public Property 担当者CD() As Short
		Get
			[担当者CD] = m_担当者CD
		End Get
		Set(ByVal Value As Short)
			m_担当者CD = Value
		End Set
	End Property
	'//////////////////////////////////////
	'   見積日付
	'//////////////////////////////////////
	
	Public Property 見積日付() As Object
		Get
			'UPGRADE_WARNING: オブジェクト m_見積日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト 見積日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[見積日付] = m_見積日付
		End Get
		Set(ByVal Value As Object)
			'UPGRADE_WARNING: オブジェクト vData の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト m_見積日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			m_見積日付 = Value
		End Set
	End Property
	'//////////////////////////////////////
	'   見積件名
	'//////////////////////////////////////
	
	Public Property 見積件名() As String
		Get
			[見積件名] = m_見積件名
		End Get
		Set(ByVal Value As String)
			m_見積件名 = Value
		End Set
	End Property
	'//////////////////////////////////////
	'   得意先CD
	'//////////////////////////////////////
	
	Public Property 得意先CD() As String
		Get
			[得意先CD] = m_得意先CD
		End Get
		Set(ByVal Value As String)
			m_得意先CD = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   得意先名1
	'//////////////////////////////////////
	
	Public Property 得意先名1() As String
		Get
			[得意先名1] = m_得意先名1
		End Get
		Set(ByVal Value As String)
			m_得意先名1 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   得意先名2
	'//////////////////////////////////////
	
	Public Property 得意先名2() As String
		Get
			[得意先名2] = m_得意先名2
		End Get
		Set(ByVal Value As String)
			m_得意先名2 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   得TEL
	'//////////////////////////////////////
	
	Public Property 得TEL() As String
		Get
			[得TEL] = m_得TEL
		End Get
		Set(ByVal Value As String)
			m_得TEL = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   得FAX
	'//////////////////////////////////////
	
	Public Property 得FAX() As String
		Get
			[得FAX] = m_得FAX
		End Get
		Set(ByVal Value As String)
			m_得FAX = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   得担当者
	'//////////////////////////////////////
	
	Public Property 得担当者() As String
		Get
			得担当者 = m_得担当者
		End Get
		Set(ByVal Value As String)
			m_得担当者 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   納得意先CD
	'//////////////////////////////////////
	
	Public Property 納得意先CD() As String
		Get
			納得意先CD = m_納得意先CD
		End Get
		Set(ByVal Value As String)
			m_納得意先CD = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   納入先CD
	'//////////////////////////////////////
	
	Public Property 納入先CD() As String
		Get
			[納入先CD] = m_納入先CD
		End Get
		Set(ByVal Value As String)
			m_納入先CD = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   納入先名1
	'//////////////////////////////////////
	
	Public Property 納入先名1() As String
		Get
			[納入先名1] = m_納入先名1
		End Get
		Set(ByVal Value As String)
			m_納入先名1 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   納入先名2
	'//////////////////////////////////////
	
	Public Property 納入先名2() As String
		Get
			[納入先名2] = m_納入先名2
		End Get
		Set(ByVal Value As String)
			m_納入先名2 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   納郵便番号
	'//////////////////////////////////////
	
	Public Property 納郵便番号() As String
		Get
			納郵便番号 = m_納郵便番号
		End Get
		Set(ByVal Value As String)
			m_納郵便番号 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   納住所1
	'//////////////////////////////////////
	
	Public Property 納住所1() As String
		Get
			納住所1 = m_納住所1
		End Get
		Set(ByVal Value As String)
			m_納住所1 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   納住所2
	'//////////////////////////////////////
	
	Public Property 納住所2() As String
		Get
			納住所2 = m_納住所2
		End Get
		Set(ByVal Value As String)
			m_納住所2 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   納TEL
	'//////////////////////////////////////
	
	Public Property 納TEL() As String
		Get
			[納TEL] = m_納TEL
		End Get
		Set(ByVal Value As String)
			m_納TEL = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   納FAX
	'//////////////////////////////////////
	
	Public Property 納FAX() As String
		Get
			[納FAX] = m_納FAX
		End Get
		Set(ByVal Value As String)
			m_納FAX = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   納担当者
	'//////////////////////////////////////
	
	Public Property 納担当者() As String
		Get
			納担当者 = m_納担当者
		End Get
		Set(ByVal Value As String)
			m_納担当者 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   納期S
	'//////////////////////////////////////
	
	Public Property 納期S() As Object
		Get
			'UPGRADE_WARNING: オブジェクト m_納期S の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト 納期S の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[納期S] = m_納期S
		End Get
		Set(ByVal Value As Object)
			'UPGRADE_WARNING: オブジェクト vData の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト m_納期S の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			m_納期S = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   納期E
	'//////////////////////////////////////
	
	Public Property 納期E() As Object
		Get
			'UPGRADE_WARNING: オブジェクト m_納期E の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト 納期E の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[納期E] = m_納期E
		End Get
		Set(ByVal Value As Object)
			'UPGRADE_WARNING: オブジェクト vData の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト m_納期E の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			m_納期E = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   備考
	'//////////////////////////////////////
	
	Public Property 備考() As String
		Get
			[備考] = m_備考
		End Get
		Set(ByVal Value As String)
			m_備考 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   規模金額
	'//////////////////////////////////////
	
	Public Property 規模金額() As Decimal
		Get
			規模金額 = m_規模金額
		End Get
		Set(ByVal Value As Decimal)
			m_規模金額 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   OPEN日
	'//////////////////////////////////////
	
	Public Property OPEN日() As Object
		Get
			'UPGRADE_WARNING: オブジェクト m_OPEN日 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト OPEN日 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			OPEN日 = m_OPEN日
		End Get
		Set(ByVal Value As Object)
			'UPGRADE_WARNING: オブジェクト vData の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト m_OPEN日 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			m_OPEN日 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   物件種別
	'//////////////////////////////////////
	
	Public Property 物件種別() As Short
		Get
			[物件種別] = m_物件種別
		End Get
		Set(ByVal Value As Short)
			m_物件種別 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   現場名
	'//////////////////////////////////////
	
	Public Property 現場名() As String
		Get
			[現場名] = m_現場名
		End Get
		Set(ByVal Value As String)
			m_現場名 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   支払条件
	'//////////////////////////////////////
	
	Public Property 支払条件() As Short
		Get
			[支払条件] = m_支払条件
		End Get
		Set(ByVal Value As Short)
			m_支払条件 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   支払条件他
	'//////////////////////////////////////
	
	Public Property 支払条件他() As String
		Get
			支払条件他 = m_支払条件他
		End Get
		Set(ByVal Value As String)
			m_支払条件他 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   納期表示
	'//////////////////////////////////////
	
	Public Property 納期表示() As Short
		Get
			[納期表示] = m_納期表示
		End Get
		Set(ByVal Value As Short)
			m_納期表示 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   納期表示他
	'//////////////////////////////////////
	
	Public Property 納期表示他() As String
		Get
			納期表示他 = m_納期表示他
		End Get
		Set(ByVal Value As String)
			m_納期表示他 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   見積日出力
	'//////////////////////////////////////
	
	Public Property 見積日出力() As Short
		Get
			[見積日出力] = m_見積日出力
		End Get
		Set(ByVal Value As Short)
			m_見積日出力 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   有効期限
	'//////////////////////////////////////
	
	Public Property 有効期限() As Short
		Get
			[有効期限] = m_有効期限
		End Get
		Set(ByVal Value As Short)
			m_有効期限 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   受注区分
	'//////////////////////////////////////
	
	Public Property 受注区分() As Short
		Get
			[受注区分] = m_受注区分
		End Get
		Set(ByVal Value As Short)
			m_受注区分 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   受注日付
	'//////////////////////////////////////
	
	Public Property 受注日付() As Object
		Get
			'UPGRADE_WARNING: オブジェクト m_受注日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト 受注日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[受注日付] = m_受注日付
		End Get
		Set(ByVal Value As Object)
			'UPGRADE_WARNING: オブジェクト vData の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト m_受注日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			m_受注日付 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   大小口区分
	'//////////////////////////////////////
	
	Public Property 大小口区分() As Short
		Get
			[大小口区分] = m_大小口区分
		End Get
		Set(ByVal Value As Short)
			m_大小口区分 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   出精値引
	'//////////////////////////////////////
	
	Public Property 出精値引() As Decimal
		Get
			[出精値引] = m_出精値引
		End Get
		Set(ByVal Value As Decimal)
			m_出精値引 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   売上日付
	'//////////////////////////////////////
	
	Public Property 売上日付() As Object
		Get
			'UPGRADE_WARNING: オブジェクト m_売上日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト 売上日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			売上日付 = m_売上日付
		End Get
		Set(ByVal Value As Object)
			'UPGRADE_WARNING: オブジェクト vData の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト m_売上日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			m_売上日付 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   仕入日付
	'//////////////////////////////////////
	
	Public Property 仕入日付() As Object
		Get
			'UPGRADE_WARNING: オブジェクト m_仕入日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト 仕入日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			仕入日付 = m_仕入日付
		End Get
		Set(ByVal Value As Object)
			'UPGRADE_WARNING: オブジェクト vData の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト m_仕入日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			m_仕入日付 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   合計金額
	'//////////////////////////////////////
	
	Public Property 合計金額() As Decimal
		Get
			[合計金額] = m_合計金額
		End Get
		Set(ByVal Value As Decimal)
			m_合計金額 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   原価合計
	'//////////////////////////////////////
	
	Public Property 原価合計() As Decimal
		Get
			[原価合計] = m_原価合計
		End Get
		Set(ByVal Value As Decimal)
			m_原価合計 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   原価率
	'//////////////////////////////////////
	
	Public Property 原価率() As Decimal
		Get
			[原価率] = m_原価率
		End Get
		Set(ByVal Value As Decimal)
			m_原価率 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   外税額
	'//////////////////////////////////////
	
	Public Property 外税額() As Decimal
		Get
			[外税額] = m_外税額
		End Get
		Set(ByVal Value As Decimal)
			m_外税額 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   税集計区分
	'//////////////////////////////////////
	
	Public Property 税集計区分() As Short
		Get
			[税集計区分] = m_税集計区分
		End Get
		Set(ByVal Value As Short)
			m_税集計区分 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   売上端数
	'//////////////////////////////////////
	
	Public Property 売上端数() As Short
		Get
			[売上端数] = m_売上端数
		End Get
		Set(ByVal Value As Short)
			m_売上端数 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   消費税端数
	'//////////////////////////////////////
	
	Public Property 消費税端数() As Short
		Get
			[消費税端数] = m_消費税端数
		End Get
		Set(ByVal Value As Short)
			m_消費税端数 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   得意先別見積番号
	'//////////////////////////////////////
	
	Public Property 得意先別見積番号() As Object
		Get
			'UPGRADE_WARNING: オブジェクト m_得意先別見積番号 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト 得意先別見積番号 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[得意先別見積番号] = m_得意先別見積番号
		End Get
		Set(ByVal Value As Object)
			'UPGRADE_WARNING: オブジェクト vData の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト m_得意先別見積番号 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			m_得意先別見積番号 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   見積区分
	'//////////////////////////////////////
	
	Public Property 見積区分() As Short
		Get
			[見積区分] = m_見積区分
		End Get
		Set(ByVal Value As Short)
			m_見積区分 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   見積区分の判断
	'//////////////////////////////////////
	Public WriteOnly Property isDsp見積区分() As enum見積区分
		Set(ByVal Value As enum見積区分)
			m_isDsp見積区分 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   見積区分の判断
	'//////////////////////////////////////
	Public WriteOnly Property isDsp社内伝() As enum社内伝
		Set(ByVal Value As enum社内伝)
			m_isDsp社内伝 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   存在確認
	'//////////////////////////////////////
	'UPGRADE_NOTE: EOF は EOF_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
	Public ReadOnly Property EOF_Renamed() As Boolean
		Get
			EOF_Renamed = m_EOF
		End Get
	End Property
	
	'//////////////////////////////////////
	'   変数の初期化
	'//////////////////////////////////////
	'UPGRADE_NOTE: Class_Initialize は Class_Initialize_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
	Private Sub Class_Initialize_Renamed()
		'初期化
		'    m_Read = True
		'    m_GetMode = True
		
		'初期化
		Call Initialize()
		
		'生成時の初期値
		m_isDsp見積区分 = enum見積区分.全て
		m_isDsp社内伝 = enum社内伝.全て
	End Sub
	Public Sub New()
		MyBase.New()
		Class_Initialize_Renamed()
	End Sub
	
	'UPGRADE_NOTE: Class_Terminate は Class_Terminate_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
	Private Sub Class_Terminate_Renamed()
		'    DBTanto = Nothing
	End Sub
	Protected Overrides Sub Finalize()
		Class_Terminate_Renamed()
		MyBase.Finalize()
	End Sub
	
	'//////////////////////////////////////
	'   クリアメソッド
	'//////////////////////////////////////
	Public Sub Initialize()
		'初期化
		m_見積番号 = 0
		m_担当者CD = 0
		'UPGRADE_WARNING: オブジェクト m_見積日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		m_見積日付 = vbNullString
		m_見積件名 = vbNullString
		m_得意先CD = vbNullString
		m_得意先名1 = vbNullString
		m_得意先名2 = vbNullString
		m_得TEL = vbNullString
		m_得FAX = vbNullString
		m_得担当者 = vbNullString
		m_納得意先CD = vbNullString
		m_納入先CD = vbNullString
		m_納入先名1 = vbNullString
		m_納入先名2 = vbNullString
		m_納郵便番号 = vbNullString
		m_納住所1 = vbNullString
		m_納住所2 = vbNullString
		m_納TEL = vbNullString
		m_納FAX = vbNullString
		m_納担当者 = vbNullString
		'UPGRADE_WARNING: オブジェクト m_納期S の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		m_納期S = vbNullString
		'UPGRADE_WARNING: オブジェクト m_納期E の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		m_納期E = vbNullString
		m_備考 = vbNullString
		m_規模金額 = 0
		'UPGRADE_WARNING: オブジェクト m_OPEN日 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		m_OPEN日 = vbNullString
		m_物件種別 = 0
		m_現場名 = vbNullString
		m_支払条件 = 0
		m_支払条件他 = vbNullString
		m_納期表示 = 0
		m_納期表示他 = vbNullString
		m_見積日出力 = 0
		m_有効期限 = 0
		m_受注区分 = 0
		'UPGRADE_WARNING: オブジェクト m_受注日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		m_受注日付 = vbNullString
		m_大小口区分 = 0
		m_出精値引 = 0
		'UPGRADE_WARNING: オブジェクト m_売上日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		m_売上日付 = vbNullString
		'UPGRADE_WARNING: オブジェクト m_仕入日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		m_仕入日付 = vbNullString
		m_合計金額 = 0
		m_原価合計 = 0
		m_原価率 = 0
		m_外税額 = 0
		m_税集計区分 = 0
		m_売上端数 = 0
		m_消費税端数 = 0
		'UPGRADE_WARNING: オブジェクト m_得意先別見積番号 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		m_得意先別見積番号 = vbNullString
		m_見積区分 = 0
		
	End Sub
	
	'//////////////////////////////////////
	'   選択画面
	'//////////////////////////////////////
	Public Function ShowDialog() As Boolean
		Dim fSentak As SentakMitumori_cls
		
		fSentak = New SentakMitumori_cls
		'呼び出し先から呼び出し元を使用する場合は自分自身を渡してあげなければならない
		fSentak.Parent_Renamed = Me
		
		With fSentak
			.ShowDialog()
			If .DialogResult_Renamed Then
				
				Me.見積番号 = .DialogResultCode
				ShowDialog = True
			End If
		End With
		
		'UPGRADE_NOTE: オブジェクト fSentak をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		fSentak = Nothing
		
	End Function
	
	'//////////////////////////////////////
	'   データを読み込むメソッド
	'//////////////////////////////////////
	Public Function GetbyID() As Boolean
		Dim rs As ADODB.Recordset
		Dim sql As String
		
		On Error GoTo GetbyID_Err
		
		'マウスポインターを砂時計にする
		'Call HourGlass(True)
		
		'必要か？↓
		''''    '見積データ使用チェック
		''''    If LockData("見積番号", Me.見積番号) = False Then
		''''        Exit Function
		''''    End If
		'必要か？↑
		
		'SQL生成
		sql = "SELECT * FROM TD見積"
		sql = sql & " WHERE 見積番号 = " & Me.見積番号
		
		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockReadOnly)
		With rs
			If .EOF Then
				ReleaseRs(rs)
				Exit Function
			Else
				
				Select Case m_isDsp見積区分
					Case enum見積区分.仮見積
						If .Fields("見積区分").Value = enum見積区分.本見積 Then
							CriticalAlarm("指定の見積番号は本見積ではありません。")
							Exit Function
						End If
					Case enum見積区分.本見積
						If .Fields("見積区分").Value = enum見積区分.仮見積 Then
							CriticalAlarm("指定の見積番号は仮見積ではありません。")
							Exit Function
						End If
					Case enum見積区分.全て
				End Select
				
				Select Case m_isDsp社内伝
					Case enum社内伝.社内伝以外
						If .Fields("得意先CD").Value = "9999" Then
							CriticalAlarm("社内在庫分です。")
							Exit Function
						End If
					Case enum社内伝.社内伝のみ
						If .Fields("得意先CD").Value <> "9999" Then
							CriticalAlarm("社内在庫以外です。")
							Exit Function
						End If
				End Select
				
				Me.見積番号 = Me.見積番号
				Me.担当者CD = .Fields("担当者CD").Value
				Me.見積日付 = .Fields("見積日付")
				Me.見積件名 = .Fields("見積件名").Value
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Me.得意先CD = NullToZero(.Fields("得意先CD"), "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Me.得意先名1 = NullToZero(.Fields("得意先名1"), "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Me.得意先名2 = NullToZero(.Fields("得意先名2"), "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Me.得TEL = NullToZero(.Fields("得TEL"), "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Me.得FAX = NullToZero(.Fields("得FAX"), "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Me.得担当者 = NullToZero(.Fields("得意先担当者"), "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Me.納得意先CD = NullToZero(.Fields("納入得意先CD"), "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Me.納入先CD = NullToZero(.Fields("納入先CD"), "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Me.納入先名1 = NullToZero(.Fields("納入先名1"), "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Me.納入先名2 = NullToZero(.Fields("納入先名2"), "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Me.納郵便番号 = NullToZero(.Fields("郵便番号"), "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Me.納住所1 = NullToZero(.Fields("住所1"), "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Me.納住所2 = NullToZero(.Fields("住所2"), "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Me.納TEL = NullToZero(.Fields("納TEL"), "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Me.納FAX = NullToZero(.Fields("納FAX"), "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Me.納担当者 = NullToZero(.Fields("納入先担当者"), "")
				Me.納期S = .Fields("納期S")
				Me.納期E = .Fields("納期E")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Me.備考 = NullToZero(.Fields("備考"), "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Me.規模金額 = NullToZero(.Fields("物件規模金額"), "")
				Me.OPEN日 = .Fields("オープン日")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Me.物件種別 = NullToZero(.Fields("物件種別"), "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Me.現場名 = NullToZero(.Fields("現場名"), "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Me.支払条件 = NullToZero(.Fields("支払条件"), "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Me.支払条件他 = NullToZero(.Fields("支払条件その他"), "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Me.納期表示 = NullToZero(.Fields("納期表示"), "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Me.納期表示他 = NullToZero(.Fields("納期その他"), "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Me.見積日出力 = NullToZero(.Fields("見積日出力"), "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Me.有効期限 = NullToZero(.Fields("有効期限"), "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Me.受注区分 = NullToZero(.Fields("受注区分"), "")
				Me.受注日付 = .Fields("受注日付")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Me.大小口区分 = NullToZero(.Fields("大小口区分"), "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Me.出精値引 = NullToZero(.Fields("出精値引"), "")
				
				Me.税集計区分 = .Fields("税集計区分").Value
				Me.売上端数 = .Fields("売上端数").Value
				Me.消費税端数 = .Fields("消費税端数").Value
				
				Me.合計金額 = .Fields("合計金額").Value
				Me.原価合計 = .Fields("原価合計").Value
				Me.原価率 = .Fields("原価率").Value
				Me.外税額 = .Fields("外税額").Value
				
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Me.得意先別見積番号 = NullToZero(.Fields("得意先別見積番号"), "")
				
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Me.見積区分 = NullToZero(.Fields("見積区分"), "")
			End If
		End With
		
		Call ReleaseRs(rs)
		
		'Call HourGlass(False)
		GetbyID = True
		Exit Function
GetbyID_Err: 
		'Call HourGlass(False)
		'エラーの生成
		Err.Raise(Err.Number,  , Err.Description)
	End Function
	
	'//////////////////////////////////////
	'   レコードを読み込むメソッド 選択画面で使用
	'//////////////////////////////////////
	Public Function GetRs(ByVal p得意先CD As String, ByVal p納入先CD As String, ByVal ps見積番号 As String, ByVal pe見積番号 As String, ByVal p見積件名 As String, ByVal ps見積金額 As String, ByVal pe見積金額 As String, ByVal ps見積日付 As String, ByVal pe見積日付 As String) As ADODB.Recordset
		Dim rs As ADODB.Recordset
		Dim sql As String
		Dim whr As String
		
		On Error GoTo GetRs_Err
		
		
		'SQL生成
		sql = "SELECT MT.見積番号, MT.納期S, MT.物件種別, MT.得意先CD, MT.見積件名," & "MT.合計金額 + MT.出精値引 AS 合計金額, MT.見積書出力日,MT.注文発行日, " & "売上状況=(CASE WHEN MT.売上日付 IS NULL THEN '' ELSE '売上済' END)" & " FROM TD見積 AS MT"
		
		'得意先
		If Trim(p得意先CD) <> "" Then
			whr = whr & "得意先CD LIKE '" & SQLString(p得意先CD) & "%'"
		End If
		'納入先
		If Trim(p納入先CD) <> "" Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & "納入先CD Like '" & SQLString(p納入先CD) & "%'"
		End If
		'見積番号
		If Trim(ps見積番号) <> "" Or Trim(pe見積番号) <> "" Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & SQLIntRange("MT.見積番号", Trim(ps見積番号), Trim(pe見積番号),  , False)
		End If
		'見積件名
		If Trim(p見積件名) <> "" Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & "見積件名 Like '%" & SQLString(p見積件名) & "%'"
		End If
		'見積金額
		If Trim(ps見積金額) <> "" Or Trim(pe見積金額) <> "" Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & SQLCurRange("MT.合計金額 + MT.出精値引", Trim(ps見積金額), Trim(pe見積金額),  , False)
		End If
		'見積出力日
		If Trim(ps見積日付) <> vbNullString Or Trim(pe見積日付) <> vbNullString Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & SQLDateRange("MT.見積日付", ps見積日付, pe見積日付, DBType, False)
		End If
		
		Select Case m_isDsp見積区分
			Case enum見積区分.仮見積, enum見積区分.本見積
				If whr <> "" Then
					whr = whr & " and "
				End If
				whr = whr & " MT.見積区分 = " & m_isDsp見積区分
			Case enum見積区分.全て
		End Select
		
		Select Case m_isDsp社内伝
			Case enum社内伝.社内伝以外
				If whr <> "" Then
					whr = whr & " and "
				End If
				whr = whr & " MT.得意先CD <> '9999' "
			Case enum社内伝.社内伝のみ
				If whr <> "" Then
					whr = whr & " and "
				End If
				whr = whr & " MT.得意先CD = '9999' "
			Case enum社内伝.全て
		End Select
		
		If whr <> vbNullString Then
			sql = sql & " WHERE " & whr
		End If
		
		sql = sql & " ORDER BY MT.見積日付 DESC, MT.見積番号"
		
		'DBクローズするのに必要
		Cn.CursorLocation = ADODB.CursorLocationEnum.adUseClient
		
		'SQL実行
		GetRs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
		'UPGRADE_NOTE: オブジェクト GetRs.ActiveConnection をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		GetRs.ActiveConnection = Nothing
		
		'DBクローズするのに必要
		Cn.CursorLocation = ADODB.CursorLocationEnum.adUseServer
		
		Exit Function
GetRs_Err: 
		'エラーの生成
		Err.Raise(Err.Number,  , Err.Description)
	End Function
	
	'2018/10/06 ADD↓
	'//////////////////////////////////////
	'   U区分存在チェック
	'   ありはTRUE
	'//////////////////////////////////////
	Public Function isU区分(ByRef U区分 As String) As Boolean
		Dim rs As ADODB.Recordset
		Dim sql As String
		
		On Error GoTo isU区分_Err
		
		'マウスポインターを砂時計にする
		'Call HourGlass(True)
		
		'SQL生成
		sql = "SELECT COUNT(U区分) AS 区分カウント"
		sql = sql & " FROM TD見積シートM"
		sql = sql & " WHERE 見積番号 = " & Me.見積番号
		sql = sql & " AND U区分 = '" & U区分 & "'"
		sql = sql & " AND 見積数量 <> 0"
		
		'SQL実行
		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
		
		With rs
			If .EOF Then
				isU区分 = False
			Else
				If .Fields("区分カウント").Value = 0 Then
					isU区分 = False
				Else
					isU区分 = True
				End If
			End If
		End With
		
		Call ReleaseRs(rs)
		
		'Call HourGlass(False)
		Exit Function
isU区分_Err: 
		'Call HourGlass(False)
		'エラーの生成
		Err.Raise(Err.Number,  , Err.Description)
	End Function
	'2018/10/06 ADD↑
End Class