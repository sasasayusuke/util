Option Strict Off
Option Explicit On
Friend Class clsBukken
	'///////////////////////////
	'物件情報データクラス
	'///////////////////////////
	'2015/11/26 oosawa      売上合計・出精値引合計・原価合計・原価率の項目を追加
	
	
	'変数
	Private m_物件番号 As Integer
	Private m_物件登録日付 As Object
	Private m_物件名 As String
	Private m_物件略称 As String
	
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
	
	
	Private m_担当者CD As Short
	Private m_部署CD As Short
	
	Private m_見積予定日付 As Object
	Private m_受注予定日付 As Object
	Private m_仕入予定日付 As Object
	Private m_売上予定日付 As Object
	Private m_検収予定日付 As Object
	Private m_請求予定日付 As Object
	
	'2015/11/26 ADD↓
	Private m_出精値引計 As Decimal
	Private m_合計金額計 As Decimal
	Private m_原価金額合計 As Decimal
	Private m_原価率計 As Decimal
	'2015/11/26 ADD↑
	
	'2022/10/20↓
	Private m_工事担当CD As Object
	Private m_税集計区分 As Object
	Private m_集計CD As Object
	
	Private m_予定開始納期 As Object
	Private m_予定終了納期 As Object
	Private m_予定オープン日 As Object
	Private m_予定物件種別 As Short
	Private m_予定受付日付 As Object
	Private m_予定完工日付 As Object
	Private m_予定請求予定日付 As Object
	'2022/10/20↑
	
	
	'Private m_初期登録日        As Variant 'Date
	'Private m_登録変更日        As Date
	
	Private m_MaxLength As Integer '桁数
	Private m_EOF As Boolean '存在確認
	
	Private m_Error As Short 'エラー
	
	'//////////////////////////////////////
	'   物件番号
	'//////////////////////////////////////
	
	Public Property 物件番号() As Integer
		Get
			[物件番号] = m_物件番号
		End Get
		Set(ByVal Value As Integer)
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			m_物件番号 = NullToZero(Value)
		End Set
	End Property
	
	'//////////////////////////////////////
	'   物件登録日付
	'//////////////////////////////////////
	
	Public Property 物件登録日付() As Object
		Get
			'UPGRADE_WARNING: オブジェクト m_物件登録日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト 物件登録日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[物件登録日付] = m_物件登録日付
		End Get
		Set(ByVal Value As Object)
			'UPGRADE_WARNING: オブジェクト vData の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト m_物件登録日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			m_物件登録日付 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   物件名
	'//////////////////////////////////////
	
	Public Property 物件名() As String
		Get
			[物件名] = m_物件名
		End Get
		Set(ByVal Value As String)
			m_物件名 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   物件略称
	'//////////////////////////////////////
	
	Public Property 物件略称() As String
		Get
			[物件略称] = m_物件略称
		End Get
		Set(ByVal Value As String)
			m_物件略称 = Value
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
	'   部署CD
	'//////////////////////////////////////
	
	Public Property 部署CD() As Short
		Get
			[部署CD] = m_部署CD
		End Get
		Set(ByVal Value As Short)
			m_部署CD = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   見積予定日付
	'//////////////////////////////////////
	
	Public Property 見積予定日付() As Object
		Get
			'UPGRADE_WARNING: オブジェクト m_見積予定日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト 見積予定日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[見積予定日付] = m_見積予定日付
		End Get
		Set(ByVal Value As Object)
			'UPGRADE_WARNING: オブジェクト vData の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト m_見積予定日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			m_見積予定日付 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   受注予定日付
	'//////////////////////////////////////
	
	Public Property 受注予定日付() As Object
		Get
			'UPGRADE_WARNING: オブジェクト m_受注予定日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト 受注予定日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[受注予定日付] = m_受注予定日付
		End Get
		Set(ByVal Value As Object)
			'UPGRADE_WARNING: オブジェクト vData の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト m_受注予定日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			m_受注予定日付 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   仕入予定日付
	'//////////////////////////////////////
	
	Public Property 仕入予定日付() As Object
		Get
			'UPGRADE_WARNING: オブジェクト m_仕入予定日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト 仕入予定日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[仕入予定日付] = m_仕入予定日付
		End Get
		Set(ByVal Value As Object)
			'UPGRADE_WARNING: オブジェクト vData の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト m_仕入予定日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			m_仕入予定日付 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   売上予定日付
	'//////////////////////////////////////
	
	Public Property 売上予定日付() As Object
		Get
			'UPGRADE_WARNING: オブジェクト m_売上予定日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト 売上予定日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[売上予定日付] = m_売上予定日付
		End Get
		Set(ByVal Value As Object)
			'UPGRADE_WARNING: オブジェクト vData の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト m_売上予定日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			m_売上予定日付 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   検収予定日付
	'//////////////////////////////////////
	
	Public Property 検収予定日付() As Object
		Get
			'UPGRADE_WARNING: オブジェクト m_検収予定日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト 検収予定日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[検収予定日付] = m_検収予定日付
		End Get
		Set(ByVal Value As Object)
			'UPGRADE_WARNING: オブジェクト vData の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト m_検収予定日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			m_検収予定日付 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   請求予定日付
	'//////////////////////////////////////
	
	Public Property 請求予定日付() As Object
		Get
			'UPGRADE_WARNING: オブジェクト m_請求予定日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト 請求予定日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[請求予定日付] = m_請求予定日付
		End Get
		Set(ByVal Value As Object)
			'UPGRADE_WARNING: オブジェクト vData の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト m_請求予定日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			m_請求予定日付 = Value
		End Set
	End Property
	
	'2015/11/26 ADD↓
	'//////////////////////////////////////
	'   出精値引計
	'//////////////////////////////////////
	
	Public Property 出精値引計() As Decimal
		Get
			[出精値引計] = m_出精値引計
		End Get
		Set(ByVal Value As Decimal)
			m_出精値引計 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   合計金額計
	'//////////////////////////////////////
	
	Public Property 合計金額計() As Decimal
		Get
			[合計金額計] = m_合計金額計
		End Get
		Set(ByVal Value As Decimal)
			m_合計金額計 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   原価金額合計
	'//////////////////////////////////////
	
	Public Property 原価金額合計() As Decimal
		Get
			[原価金額合計] = m_原価金額合計
		End Get
		Set(ByVal Value As Decimal)
			m_原価金額合計 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   原価率計
	'//////////////////////////////////////
	
	Public Property 原価率計() As Decimal
		Get
			原価率計 = m_原価率計
		End Get
		Set(ByVal Value As Decimal)
			m_原価率計 = Value
		End Set
	End Property
	'2015/11/26 ADD↑
	
	'2022/10/20 ADD↓
	'//////////////////////////////////////
	'   工事担当CD
	'//////////////////////////////////////
	
	Public Property 工事担当CD() As Short
		Get
			'UPGRADE_WARNING: オブジェクト m_工事担当CD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[工事担当CD] = m_工事担当CD
		End Get
		Set(ByVal Value As Short)
			'UPGRADE_WARNING: オブジェクト m_工事担当CD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			m_工事担当CD = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   税集計区分
	'//////////////////////////////////////
	
	Public Property 税集計区分() As Short
		Get
			'UPGRADE_WARNING: オブジェクト m_税集計区分 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[税集計区分] = m_税集計区分
		End Get
		Set(ByVal Value As Short)
			'UPGRADE_WARNING: オブジェクト m_税集計区分 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			m_税集計区分 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   集計CD
	'//////////////////////////////////////
	
	Public Property 集計CD() As String
		Get
			'UPGRADE_WARNING: オブジェクト m_集計CD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[集計CD] = m_集計CD
		End Get
		Set(ByVal Value As String)
			'UPGRADE_WARNING: オブジェクト m_集計CD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			m_集計CD = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   予定開始納期
	'//////////////////////////////////////
	
	Public Property 予定開始納期() As Object
		Get
			'UPGRADE_WARNING: オブジェクト m_予定開始納期 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト 予定開始納期 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			予定開始納期 = m_予定開始納期
		End Get
		Set(ByVal Value As Object)
			'UPGRADE_WARNING: オブジェクト vData の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト m_予定開始納期 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			m_予定開始納期 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   予定終了納期
	'//////////////////////////////////////
	
	Public Property 予定終了納期() As Object
		Get
			'UPGRADE_WARNING: オブジェクト m_予定終了納期 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト 予定終了納期 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			予定終了納期 = m_予定終了納期
		End Get
		Set(ByVal Value As Object)
			'UPGRADE_WARNING: オブジェクト vData の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト m_予定終了納期 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			m_予定終了納期 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   予定オープン日
	'//////////////////////////////////////
	
	Public Property 予定オープン日() As Object
		Get
			'UPGRADE_WARNING: オブジェクト m_予定オープン日 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト 予定オープン日 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[予定オープン日] = m_予定オープン日
		End Get
		Set(ByVal Value As Object)
			'UPGRADE_WARNING: オブジェクト vData の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト m_予定オープン日 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			m_予定オープン日 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   予定物件種別
	'//////////////////////////////////////
	
	Public Property 予定物件種別() As Short
		Get
			[予定物件種別] = m_予定物件種別
		End Get
		Set(ByVal Value As Short)
			m_予定物件種別 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   予定受付日付
	'//////////////////////////////////////
	
	Public Property 予定受付日付() As Object
		Get
			'UPGRADE_WARNING: オブジェクト m_予定受付日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト 予定受付日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[予定受付日付] = m_予定受付日付
		End Get
		Set(ByVal Value As Object)
			'UPGRADE_WARNING: オブジェクト vData の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト m_予定受付日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			m_予定受付日付 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   予定完工日付
	'//////////////////////////////////////
	
	Public Property 予定完工日付() As Object
		Get
			'UPGRADE_WARNING: オブジェクト m_予定完工日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト 予定完工日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[予定完工日付] = m_予定完工日付
		End Get
		Set(ByVal Value As Object)
			'UPGRADE_WARNING: オブジェクト vData の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト m_予定完工日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			m_予定完工日付 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   予定請求予定日付
	'//////////////////////////////////////
	
	Public Property 予定請求予定日付() As Object
		Get
			'UPGRADE_WARNING: オブジェクト m_予定請求予定日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト 予定請求予定日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[予定請求予定日付] = m_予定請求予定日付
		End Get
		Set(ByVal Value As Object)
			'UPGRADE_WARNING: オブジェクト vData の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト m_予定請求予定日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			m_予定請求予定日付 = Value
		End Set
	End Property
	'2022/10/20 ADD↑
	
	'//////////////////////////////////////
	'   MaxLength
	'//////////////////////////////////////
	Public ReadOnly Property MaxLength() As Integer
		Get
			MaxLength = m_MaxLength
		End Get
	End Property
	
	'''Public Property Let MaxLength(ByVal NewValue As Long)
	'''    m_MaxLength = NewValue
	'''End Property
	
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
		m_MaxLength = 7
		'初期化
		Call Initialize()
		
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
		m_物件番号 = 0
		
		'UPGRADE_WARNING: オブジェクト m_物件登録日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		m_物件登録日付 = Today
		m_物件名 = ""
		m_物件略称 = ""
		
		m_得意先CD = ""
		m_得意先名1 = ""
		m_得意先名2 = ""
		m_得TEL = ""
		m_得FAX = ""
		m_得担当者 = ""
		
		m_納得意先CD = ""
		m_納入先CD = ""
		m_納入先名1 = ""
		m_納入先名2 = ""
		m_納郵便番号 = ""
		m_納住所1 = ""
		m_納住所2 = ""
		m_納TEL = ""
		m_納FAX = ""
		m_納担当者 = ""
		
		m_担当者CD = 0
		m_部署CD = 0
		
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト m_見積予定日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		m_見積予定日付 = System.DBNull.Value
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト m_受注予定日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		m_受注予定日付 = System.DBNull.Value
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト m_仕入予定日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		m_仕入予定日付 = System.DBNull.Value
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト m_売上予定日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		m_売上予定日付 = System.DBNull.Value
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト m_検収予定日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		m_検収予定日付 = System.DBNull.Value
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト m_請求予定日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		m_請求予定日付 = System.DBNull.Value
		
		'2022/10/20 ADD↓
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト m_予定開始納期 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		m_予定開始納期 = System.DBNull.Value
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト m_予定終了納期 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		m_予定終了納期 = System.DBNull.Value
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト m_予定オープン日 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		m_予定オープン日 = System.DBNull.Value
		m_予定物件種別 = 0
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト m_予定受付日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		m_予定受付日付 = System.DBNull.Value
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト m_予定完工日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		m_予定完工日付 = System.DBNull.Value
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト m_予定請求予定日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		m_予定請求予定日付 = System.DBNull.Value
		'2022/10/20 ADD↑
		
	End Sub
	
	'//////////////////////////////////////
	'   データを読み込むメソッド
	'//////////////////////////////////////
	Public Function GetbyID() As Boolean
		Dim rs As ADODB.Recordset
		Dim sql As String
		
		On Error GoTo GetbyID_Err
		
		'マウスポインターを砂時計にする
		'Call HourGlass(True)
		
		'SQL生成
		sql = "SELECT *"
		sql = sql & " FROM TD物件情報"
		sql = sql & " WHERE 物件番号 = " & SQLString((Me.物件番号))
		
		'SQL実行
		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
		
		With rs
			If .EOF Then
				GetbyID = False
				m_EOF = True
				
				Me.Initialize()
				
			Else
				GetbyID = True
				m_EOF = False
				
				Me.物件番号 = .Fields("物件番号").Value
				Me.物件登録日付 = .Fields("物件登録日付")
				Me.物件名 = .Fields("物件名").Value
				Me.物件略称 = .Fields("物件略称").Value
				
				Me.得意先CD = .Fields("得意先CD").Value
				Me.得意先名1 = .Fields("得意先名1").Value
				Me.得意先名2 = .Fields("得意先名2").Value
				Me.得TEL = .Fields("得TEL").Value
				Me.得FAX = .Fields("得FAX").Value
				Me.得担当者 = .Fields("得意先担当者").Value
				
				Me.納得意先CD = .Fields("納入得意先CD").Value
				Me.納入先CD = .Fields("納入先CD").Value
				Me.納入先名1 = .Fields("納入先名1").Value
				Me.納入先名2 = .Fields("納入先名2").Value
				Me.納郵便番号 = .Fields("郵便番号").Value
				Me.納住所1 = .Fields("住所1").Value
				Me.納住所2 = .Fields("住所2").Value
				Me.納TEL = .Fields("納TEL").Value
				Me.納FAX = .Fields("納FAX").Value
				Me.納担当者 = .Fields("納入先担当者").Value
				
				Me.担当者CD = .Fields("担当者CD").Value
				Me.部署CD = .Fields("部署CD").Value
				
				Me.見積予定日付 = .Fields("見積予定日付")
				Me.受注予定日付 = .Fields("受注予定日付")
				Me.仕入予定日付 = .Fields("仕入予定日付")
				Me.売上予定日付 = .Fields("売上予定日付")
				Me.検収予定日付 = .Fields("検収予定日付")
				Me.請求予定日付 = .Fields("請求予定日付")
				
				'2022/10/20 ADD↓
				Me.工事担当CD = .Fields("工事担当CD").Value
				Me.税集計区分 = .Fields("税集計区分").Value
				Me.集計CD = .Fields("集計CD").Value
				
				Me.予定開始納期 = .Fields("予定納期S")
				Me.予定終了納期 = .Fields("予定納期E")
				Me.予定オープン日 = .Fields("予定オープン日")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Me.予定物件種別 = NullToZero(.Fields("予定物件種別"))
				Me.予定受付日付 = .Fields("予定受付日付")
				Me.予定完工日付 = .Fields("予定完工日付")
				Me.予定請求予定日付 = .Fields("予定請求予定日付")
				'2022/10/20 ADD↑
				
			End If
		End With
		
		Call ReleaseRs(rs)
		
		'値引合計・売上合計・原価合計の算出
		Call Get合計関係()
		
		'Call HourGlass(False)
		Exit Function
GetbyID_Err: 
		'Call HourGlass(False)
		'エラーの生成
		Err.Raise(Err.Number,  , Err.Description)
	End Function
	
	'//////////////////////////////////////
	'   レコードを読み込むメソッド
	'//////////////////////////////////////
	Public Function GetAllRs() As ADODB.Recordset
		Dim rs As ADODB.Recordset
		Dim sql As String
		
		On Error GoTo GetRs_Err
		
		'マウスポインターを砂時計にする
		'Call HourGlass(True)
		
		'SQL生成
		sql = "SELECT  *"
		sql = sql & " FROM TD物件情報"
		sql = sql & " ORDER BY 物件番号"
		
		'DBクローズするのに必要
		Cn.CursorLocation = ADODB.CursorLocationEnum.adUseClient
		
		'SQL実行
		GetAllRs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
		'UPGRADE_NOTE: オブジェクト GetAllRs.ActiveConnection をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		GetAllRs.ActiveConnection = Nothing
		
		'DBクローズするのに必要
		Cn.CursorLocation = ADODB.CursorLocationEnum.adUseServer
		'    Call ReleaseRs(rs)
		
		'Call HourGlass(False)
		Exit Function
GetRs_Err: 
		'Call HourGlass(False)
		'エラーの生成
		Err.Raise(Err.Number,  , Err.Description)
		
	End Function
	
	'//////////////////////////////////////
	'   選択画面
	'//////////////////////////////////////
	Public Function ShowDialog() As Boolean
		Dim fSentak As BukkenSen_cls
		
		fSentak = New BukkenSen_cls
		
		With fSentak
			.ShowDialog()
			If .DialogResult_Renamed Then
				Me.物件番号 = CInt(.DialogResultCode)
				ShowDialog = True
			End If
		End With
		
		'UPGRADE_NOTE: オブジェクト fSentak をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		fSentak = Nothing
		
	End Function
	
	'//////////////////////////////////////
	'   Upload
	'//////////////////////////////////////
	Public Function Upload() As Boolean
		Dim rs As ADODB.Recordset
		Dim sql As String
		Dim BukkenNo As Integer '物件番号保持
		
		'マウスポインターを砂時計にする
		'Call HourGlass(True)
		
		On Error GoTo Trans_err
		
		BukkenNo = Me.物件番号
		
		sql = "SELECT  *"
		sql = sql & " FROM TD物件情報"
		sql = sql & " WHERE 物件番号 = " & SQLString((Me.物件番号))
		
		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
		
		With rs
			Select Case .EOF
				Case True
					.AddNew()
					'                ![物件番号] = Me.物件番号
					'新№獲得
					'                ![物件番号] = GetCounter("物件番号")
					BukkenNo = GetCounter("物件番号")
					.Fields("物件番号").Value = BukkenNo
					
					.Fields("初期登録日").Value = Now
				Case False
			End Select
			
			'UPGRADE_WARNING: オブジェクト Me.物件登録日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Fields("物件登録日付").Value = Me.物件登録日付
			.Fields("物件名").Value = Me.物件名
			.Fields("物件略称").Value = Me.物件略称
			
			.Fields("得意先CD").Value = Me.得意先CD
			.Fields("得意先名1").Value = Me.得意先名1
			.Fields("得意先名2").Value = Me.得意先名2
			.Fields("得TEL").Value = Me.得TEL
			.Fields("得FAX").Value = Me.得FAX
			.Fields("得意先担当者").Value = Me.得担当者
			
			.Fields("納入得意先CD").Value = Me.納得意先CD
			.Fields("納入先CD").Value = Me.納入先CD
			.Fields("納入先名1").Value = Me.納入先名1
			.Fields("納入先名2").Value = Me.納入先名2
			.Fields("郵便番号").Value = Me.納郵便番号
			.Fields("住所1").Value = Me.納住所1
			.Fields("住所2").Value = Me.納住所2
			.Fields("納TEL").Value = Me.納TEL
			.Fields("納FAX").Value = Me.納FAX
			.Fields("納入先担当者").Value = Me.納担当者
			
			.Fields("担当者CD").Value = Me.担当者CD
			.Fields("部署CD").Value = Me.部署CD
			
			'UPGRADE_WARNING: オブジェクト Me.見積予定日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Fields("見積予定日付").Value = Me.見積予定日付
			'UPGRADE_WARNING: オブジェクト Me.受注予定日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Fields("受注予定日付").Value = Me.受注予定日付
			'UPGRADE_WARNING: オブジェクト Me.仕入予定日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Fields("仕入予定日付").Value = Me.仕入予定日付
			'UPGRADE_WARNING: オブジェクト Me.売上予定日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Fields("売上予定日付").Value = Me.売上予定日付
			'UPGRADE_WARNING: オブジェクト Me.検収予定日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Fields("検収予定日付").Value = Me.検収予定日付
			'UPGRADE_WARNING: オブジェクト Me.請求予定日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Fields("請求予定日付").Value = Me.請求予定日付
			
			.Fields("登録変更日").Value = Now
			
			
			'2022/10/20 ADD↓
			.Fields("工事担当CD").Value = Me.工事担当CD
			.Fields("税集計区分").Value = Me.税集計区分
			.Fields("集計CD").Value = Me.集計CD
			
			'UPGRADE_WARNING: オブジェクト Me.予定開始納期 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Fields("予定納期S").Value = Me.予定開始納期
			'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Fields("予定納期E").Value = SpcToNull((Me.予定終了納期))
			'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Fields("予定オープン日").Value = SpcToNull((Me.予定オープン日))
			.Fields("予定物件種別").Value = Me.予定物件種別
			'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Fields("予定受付日付").Value = SpcToNull((Me.予定受付日付))
			'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Fields("予定完工日付").Value = SpcToNull((Me.予定完工日付))
			'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Fields("予定請求予定日付").Value = SpcToNull((Me.予定請求予定日付))
			'2022/10/20 ADD↑
			
			
			
			.Update()
		End With
		
		Call ReleaseRs(rs)
		
		Me.物件番号 = BukkenNo '新規の時も物件番号が返せるように保持。失敗したときにセットしないようにこの位置。
		Upload = True
		
Trans_Correct: 
		On Error GoTo 0
		
		'Call HourGlass(False)
		Exit Function
		
Trans_err: '---エラー時
		'Call HourGlass(False)
		'エラーの生成
		Err.Raise(Err.Number,  , Err.Description)
		Resume Trans_Correct
	End Function
	
	'//////////////////////////////////////
	'   Purge
	'//////////////////////////////////////
	Public Function Purge() As Boolean
		Dim sql As String
		
		Purge = False
		
		On Error GoTo Trans_err
		
		'マウスポインターを砂時計にする
		'Call HourGlass(True)
		
		'---他データ使用状況チェック
		If Me.PurgeChk() Then
			'Call HourGlass(False)
			Exit Function
		End If
		
		'SQL生成
		sql = "DELETE "
		sql = sql & " FROM TD物件情報"
		sql = sql & " WHERE 物件番号 = " & SQLString((Me.物件番号))
		
		Cn.Execute(sql)
		
		Purge = True
		
Trans_Correct: 
		On Error GoTo 0
		
		'Call HourGlass(False)
		Exit Function
		
Trans_err: '---エラー時
		'Call HourGlass(False)
		'エラーの生成
		Err.Raise(Err.Number,  , Err.Description)
		Resume Trans_Correct
	End Function
	
	'//////////////////////////////////////
	'   PurgeChk
	'//////////////////////////////////////
	Friend Function PurgeChk() As Boolean
		Dim cmd As New ADODB.Command
		
		PurgeChk = False
		
		'Call HourGlass(True)
		
		' コマンドを実行する接続先を指定する
		cmd.let_ActiveConnection(Cn)
		cmd.CommandText = "usp_ChkDelFor物件情報"
		cmd.CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
		
		' それぞれのパラメータの値を指定する
		With cmd.Parameters
			.Item(1).Value = Me.物件番号
		End With
		
		cmd.Execute()
		
		If (cmd.Parameters(0).Value = 0) Then
			PurgeChk = False
		Else
			PurgeChk = True
			CriticalAlarm((cmd.Parameters(3).Value))
		End If
		
		'UPGRADE_NOTE: オブジェクト cmd をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		cmd = Nothing
		
		'Call HourGlass(False)
	End Function
	
	'2015/11/26 ADD↓
	Private Function Get合計関係() As Boolean
		Dim sql As String
		Dim rs As ADODB.Recordset
		
		On Error GoTo GetbyID_Err
		
		'マウスポインターを砂時計にする
		'Call HourGlass(True)
		
		'SQL生成
		sql = "SELECT 物件番号, SUM(出精値引) AS 出精値引計, SUM(合計金額) AS 合計金額計, SUM(原価合計) AS 原価金額合計"
		sql = sql & " FROM TD見積"
		sql = sql & " WHERE 物件番号 = " & SQLString((Me.物件番号))
		sql = sql & " GROUP BY 物件番号"
		sql = sql & " ORDER BY 物件番号"
		
		'SQL実行
		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
		
		With rs
			If .EOF Then
				Get合計関係 = False
				
				Me.出精値引計 = 0
				Me.合計金額計 = 0
				Me.原価金額合計 = 0
				Me.原価率計 = 0
				
			Else
				Get合計関係 = True
				
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Me.出精値引計 = NullToZero(.Fields("出精値引計"))
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Me.合計金額計 = NullToZero(.Fields("合計金額計"))
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Me.原価金額合計 = NullToZero(.Fields("原価金額合計"))
				
				'UPGRADE_WARNING: オブジェクト NullToZero(rs!出精値引計) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト NullToZero(rs!合計金額計) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				If NullToZero(.Fields("合計金額計")) + NullToZero(.Fields("出精値引計")) = 0 Then
					Me.原価率計 = 0
				Else
					'UPGRADE_WARNING: オブジェクト NullToZero(rs!出精値引計) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト NullToZero(rs!合計金額計) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					Me.原価率計 = System.Math.Round(NullToZero(.Fields("原価金額合計")) / (NullToZero(.Fields("合計金額計")) + NullToZero(.Fields("出精値引計"))) * 100, 2)
				End If
			End If
		End With
		
		Call ReleaseRs(rs)
		
		'Call HourGlass(False)
		Exit Function
GetbyID_Err: 
		'Call HourGlass(False)
		'エラーの生成
		Err.Raise(Err.Number,  , Err.Description)
	End Function
	'2015/11/26 ADD↑
End Class