Option Strict Off
Option Explicit On

''' <summary>
''' 得意先マスタクラス
''' </summary>
Public Class clsTokuisaki

	'変数
	Private m_得意先CD As String
	Private m_得意先名1 As String
	Private m_得意先名2 As String
	Private m_略称 As String
	Private m_フリガナ As String
	Private m_郵便番号 As String
	Private m_住所1 As String
	Private m_住所2 As String
	Private m_電話番号 As String
	Private m_FAX番号 As String
	Private m_得意先担当者名 As String
	Private m_担当者CD As Short
	Private m_締日 As Short
	Private m_決算月 As Short

	Private m_税集計区分 As Short
	Private m_売上端数 As Short

	Private m_消費税端数 As Short
	Private m_入金サイクル As Short
	Private m_入金予定日 As Short
	Private m_入金区分CD As Short
	Private m_回収方法 As Short
	Private m_請求様式 As Short
	Private m_導入請求金額 As Decimal
	Private m_導入売掛金額 As Decimal
	Private m_メモ As String

	Private m_初期登録日 As Object 'Date
	'Private m_登録変更日 As Date
	Private m_最終使用日 As Object

	Private m_集計CD As String '2013/10/31 ADD

	Private m_チームCD As String '2014/09/18 ADD

	Private m_MaxLength As Integer '桁数
	Private m_EOF As Boolean '存在確認

	'Private m_Error As Integer 'エラー

	' '''税集計区分の種類
	' ''Public Enum Type税集計区分
	' ''    伝票単位 = 0
	' ''    請求単位 = 1
	' ''    税対象外 = 3
	' ''End Enum

	'2016/04/07 ADD↓
	Public Function Get税集計区分名(ByRef ID As String) As String
		Select Case ID
			Case CStr(0)
				Get税集計区分名 = "伝票単位"
			Case CStr(1)
				Get税集計区分名 = "請求単位"
				'2016/04/07 ADD↓
			Case CStr(3)
				Get税集計区分名 = "税対象外"
				'2016/04/07 ADD↑
			Case Else
				Get税集計区分名 = ""
		End Select
	End Function
	'2016/04/07 ADD↑

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
	'   略称
	'//////////////////////////////////////
	Public Property 略称() As String
		Get
			[略称] = m_略称
		End Get
		Set(ByVal Value As String)
			m_略称 = Value
		End Set
	End Property

	'//////////////////////////////////////
	'   フリガナ
	'//////////////////////////////////////
	Public Property フリガナ() As String
		Get
			[フリガナ] = m_フリガナ
		End Get
		Set(ByVal Value As String)
			m_フリガナ = Value
		End Set
	End Property

	'//////////////////////////////////////
	'   郵便番号
	'//////////////////////////////////////
	Public Property 郵便番号() As String
		Get
			[郵便番号] = m_郵便番号
		End Get
		Set(ByVal Value As String)
			m_郵便番号 = Value
		End Set
	End Property

	'//////////////////////////////////////
	'   住所1
	'//////////////////////////////////////
	Public Property 住所1() As String
		Get
			[住所1] = m_住所1
		End Get
		Set(ByVal Value As String)
			m_住所1 = Value
		End Set
	End Property

	'//////////////////////////////////////
	'   住所2
	'//////////////////////////////////////
	Public Property 住所2() As String
		Get
			[住所2] = m_住所2
		End Get
		Set(ByVal Value As String)
			m_住所2 = Value
		End Set
	End Property

	'//////////////////////////////////////
	'   電話番号
	'//////////////////////////////////////
	Public Property 電話番号() As String
		Get
			[電話番号] = m_電話番号
		End Get
		Set(ByVal Value As String)
			m_電話番号 = Value
		End Set
	End Property

	'//////////////////////////////////////
	'   FAX番号
	'//////////////////////////////////////
	Public Property FAX番号() As String
		Get
			[FAX番号] = m_FAX番号
		End Get
		Set(ByVal Value As String)
			m_FAX番号 = Value
		End Set
	End Property

	'//////////////////////////////////////
	'   得意先担当者名
	'//////////////////////////////////////
	Public Property 得意先担当者名() As String
		Get
			[得意先担当者名] = m_得意先担当者名
		End Get
		Set(ByVal Value As String)
			m_得意先担当者名 = Value
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
	'   締日
	'//////////////////////////////////////
	Public Property 締日() As Short
		Get
			[締日] = m_締日
		End Get
		Set(ByVal Value As Short)
			m_締日 = Value
		End Set
	End Property

	'//////////////////////////////////////
	'   決算月
	'//////////////////////////////////////
	Public Property 決算月() As Short
		Get
			[決算月] = m_決算月
		End Get
		Set(ByVal Value As Short)
			m_決算月 = Value
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
	'   入金サイクル
	'//////////////////////////////////////
	Public Property 入金サイクル() As Short
		Get
			[入金サイクル] = m_入金サイクル
		End Get
		Set(ByVal Value As Short)
			m_入金サイクル = Value
		End Set
	End Property

	'//////////////////////////////////////
	'   入金予定日
	'//////////////////////////////////////
	Public Property 入金予定日() As Short
		Get
			[入金予定日] = m_入金予定日
		End Get
		Set(ByVal Value As Short)
			m_入金予定日 = Value
		End Set
	End Property

	'//////////////////////////////////////
	'   入金区分CD
	'//////////////////////////////////////
	Public Property 入金区分CD() As Short
		Get
			[入金区分CD] = m_入金区分CD
		End Get
		Set(ByVal Value As Short)
			m_入金区分CD = Value
		End Set
	End Property

	'//////////////////////////////////////
	'   回収方法
	'//////////////////////////////////////
	Public Property 回収方法() As Short
		Get
			[回収方法] = m_回収方法
		End Get
		Set(ByVal Value As Short)
			m_回収方法 = Value
		End Set
	End Property

	'//////////////////////////////////////
	'   請求様式
	'//////////////////////////////////////
	Public Property 請求様式() As Short
		Get
			[請求様式] = m_請求様式
		End Get
		Set(ByVal Value As Short)
			m_請求様式 = Value
		End Set
	End Property

	'//////////////////////////////////////
	'   導入請求金額
	'//////////////////////////////////////
	Public Property 導入請求金額() As Decimal
		Get
			[導入請求金額] = m_導入請求金額
		End Get
		Set(ByVal Value As Decimal)
			m_導入請求金額 = Value
		End Set
	End Property

	'//////////////////////////////////////
	'   導入売掛金額
	'//////////////////////////////////////
	Public Property 導入売掛金額() As Decimal
		Get
			[導入売掛金額] = m_導入売掛金額
		End Get
		Set(ByVal Value As Decimal)
			m_導入売掛金額 = Value
		End Set
	End Property

	'//////////////////////////////////////
	'   メモ
	'//////////////////////////////////////
	Public Property メモ() As String
		Get
			[メモ] = m_メモ
		End Get
		Set(ByVal Value As String)
			m_メモ = Value
		End Set
	End Property

	'//////////////////////////////////////
	'   最終使用日
	'//////////////////////////////////////
	Public Property 最終使用日() As Object
		Get
			'UPGRADE_WARNING: オブジェクト m_最終使用日 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト 最終使用日 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[最終使用日] = m_最終使用日
		End Get
		Set(ByVal Value As Object)
			'UPGRADE_WARNING: オブジェクト vData の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト m_最終使用日 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			m_最終使用日 = Value
		End Set
	End Property

	'2013/10/31 ADD↓
	'//////////////////////////////////////
	'   初期登録日
	'//////////////////////////////////////
	Public Property 初期登録日() As Object
		Get
			'UPGRADE_WARNING: オブジェクト m_初期登録日 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト 初期登録日 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[初期登録日] = m_初期登録日
		End Get
		Set(ByVal Value As Object)
			'UPGRADE_WARNING: オブジェクト vData の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト m_初期登録日 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			m_初期登録日 = Value
		End Set
	End Property

	'//////////////////////////////////////
	'   集計CD
	'//////////////////////////////////////
	Public Property 集計CD() As String
		Get
			[集計CD] = m_集計CD
		End Get
		Set(ByVal Value As String)
			m_集計CD = Value
		End Set
	End Property
	'2013/10/31 ADD↑

	'2014/09/18 ADD↓
	'//////////////////////////////////////
	'   チームCD
	'//////////////////////////////////////
	Public Property チームCD() As Short
		Get
			[チームCD] = CShort(m_チームCD)
		End Get
		Set(ByVal Value As Short)
			m_チームCD = CStr(Value)
		End Set
	End Property

	'2014/09/18 ADD↑

	'//////////////////////////////////////
	'   MaxLength
	'//////////////////////////////////////
	Public ReadOnly Property MaxLength() As Integer
		Get
			MaxLength = m_MaxLength
		End Get
	End Property

	' ''Public Property Let MaxLength(ByVal NewValue As Long)
	' ''    m_MaxLength = NewValue
	' ''End Property

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
		m_MaxLength = 4
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
		'初期値を1桁多く設定する
		''    m_部門CD = String(m_MaxLength + 1, " ")
		m_得意先CD = vbNullString
		m_得意先名1 = vbNullString
		m_得意先名2 = vbNullString
		m_略称 = vbNullString
		m_フリガナ = vbNullString
		m_郵便番号 = vbNullString
		m_住所1 = vbNullString
		m_住所2 = vbNullString
		m_電話番号 = vbNullString
		m_FAX番号 = vbNullString
		m_得意先担当者名 = vbNullString
		m_担当者CD = 0
		m_締日 = 0
		m_決算月 = 0

		m_税集計区分 = 0
		m_売上端数 = 0

		m_消費税端数 = 0
		m_入金サイクル = 0
		m_入金予定日 = 0
		m_入金区分CD = 0
		m_回収方法 = 0
		m_請求様式 = 0
		m_導入請求金額 = 0
		m_導入売掛金額 = 0
		m_メモ = vbNullString

		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト m_最終使用日 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		m_最終使用日 = System.DBNull.Value

		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト m_初期登録日 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		m_初期登録日 = System.DBNull.Value '2013/10/31 ADD

		m_チームCD = CStr(0) '2014/09/18 ADD

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
		sql = sql & " FROM TM得意先"
		sql = sql & " WHERE 得意先CD = '" & SQLString((Me.得意先CD)) & "'"

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

				Me.得意先CD = .Fields("得意先CD").Value
				Me.得意先名1 = .Fields("得意先名1").Value
				Me.得意先名2 = .Fields("得意先名2").Value
				Me.略称 = .Fields("略称").Value
				Me.フリガナ = .Fields("フリガナ").Value
				Me.郵便番号 = .Fields("郵便番号").Value
				Me.住所1 = .Fields("住所1").Value
				Me.住所2 = .Fields("住所2").Value
				Me.電話番号 = .Fields("電話番号").Value
				Me.FAX番号 = .Fields("FAX番号").Value
				Me.得意先担当者名 = .Fields("得意先担当者名").Value
				Me.担当者CD = .Fields("担当者CD").Value
				Me.締日 = .Fields("締日").Value
				Me.決算月 = .Fields("決算月").Value

				Me.税集計区分 = .Fields("税集計区分").Value
				Me.売上端数 = .Fields("売上端数").Value

				Me.消費税端数 = .Fields("消費税端数").Value
				Me.入金サイクル = .Fields("入金サイクル").Value
				Me.入金予定日 = .Fields("入金予定日").Value
				Me.入金区分CD = .Fields("入金区分CD").Value
				Me.回収方法 = .Fields("回収方法").Value
				Me.請求様式 = .Fields("請求様式").Value
				Me.導入請求金額 = .Fields("導入請求金額").Value
				Me.導入売掛金額 = .Fields("導入売掛金額").Value
				Me.メモ = .Fields("メモ").Value

				Me.最終使用日 = .Fields("最終使用日").Value

				Me.初期登録日 = .Fields("初期登録日").Value '2013/10/31 ADD
				Me.集計CD = .Fields("集計CD").Value '2013/10/31 ADD
				Me.チームCD = .Fields("チームCD").Value '2014/09/18 ADD
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
		sql = sql & " FROM TM得意先"

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
	'   レコードを読み込むメソッド
	'//////////////////////////////////////
	Public Function GetRsByID(ByVal 開始CD As Object, ByVal 終了cd As Object) As ADODB.Recordset
		Dim rs As ADODB.Recordset
		Dim sql As String
		Dim whr As String

		On Error GoTo GetRs_Err

		'マウスポインターを砂時計にする
		'Call HourGlass(True)

		'-------抽出条件セット
		whr = SQLIntRange("得意先CD", 開始CD, 終了cd)
		If whr <> vbNullString Then
			whr = " WHERE " & whr & ""
		Else
			whr = ""
		End If

		'SQL生成
		sql = "SELECT  *"
		sql = sql & " FROM TM得意先"
		sql = sql & " " & whr & " "
		sql = sql & " ORDER BY 得意先CD"

		'DBクローズするのに必要
		Cn.CursorLocation = ADODB.CursorLocationEnum.adUseClient

		'SQL実行
		GetRsByID = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
		'UPGRADE_NOTE: オブジェクト GetAllRs.ActiveConnection をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		GetRsByID.ActiveConnection = Nothing

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
		Dim fSentak As TokSen_cls
		fSentak = New TokSen_cls

		ShowDialog = False

		With fSentak
			'        Call .SelSetup("得意先選択" _
			''            , "SELECT 得意先ID,得意先店舗ID,得意先名1,得意先名2 FROM TM得意先" _
			''            , "得意先" _
			''            , Null, Null, Null, Null)
			'       Call .SelSetup( _
			''                        "SELECT 部門CD,部門名 FROM TM部門", _
			''                        "部門CD", "部門名", "", "部門CD", "部門選択", _
			''                        1050, 2235)
			.ShowDialog()
			If .DialogResult_Renamed Then
				Me.得意先CD = .DialogResultCode
				ShowDialog = True
			End If
		End With

		fSentak.Dispose()
		'UPGRADE_NOTE: オブジェクト fSentak をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		fSentak = Nothing

	End Function

	'//////////////////////////////////////
	'   Upload
	'//////////////////////////////////////
	Public Function Upload() As Boolean
		Dim rs As ADODB.Recordset
		Dim sql As String

		'マウスポインターを砂時計にする
		'Call HourGlass(True)

		On Error GoTo Trans_err

		sql = "SELECT  *"
		sql = sql & " FROM TM得意先"
		sql = sql & " WHERE 得意先CD = '" & SQLString((Me.得意先CD)) & "'"

		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)

		With rs
			Select Case .EOF
				Case True
					.AddNew()
					.Fields("得意先CD").Value = Me.得意先CD

					.Fields("初期登録日").Value = Today
				Case False
			End Select

			.Fields("得意先名1").Value = Me.得意先名1
			.Fields("得意先名2").Value = Me.得意先名2
			.Fields("略称").Value = Me.略称
			.Fields("フリガナ").Value = Me.フリガナ
			.Fields("郵便番号").Value = Me.郵便番号
			.Fields("住所1").Value = Me.住所1
			.Fields("住所2").Value = Me.住所2
			.Fields("電話番号").Value = Me.電話番号
			.Fields("FAX番号").Value = Me.FAX番号
			.Fields("得意先担当者名").Value = Me.得意先担当者名
			.Fields("担当者CD").Value = Me.担当者CD
			.Fields("締日").Value = Me.締日
			.Fields("決算月").Value = Me.決算月

			.Fields("税集計区分").Value = Me.税集計区分
			.Fields("売上端数").Value = Me.売上端数

			.Fields("消費税端数").Value = Me.消費税端数
			.Fields("入金サイクル").Value = Me.入金サイクル
			.Fields("入金予定日").Value = Me.入金予定日
			.Fields("入金区分CD").Value = Me.入金区分CD
			.Fields("回収方法").Value = Me.回収方法
			.Fields("請求様式").Value = Me.請求様式
			.Fields("導入請求金額").Value = Me.導入請求金額
			.Fields("導入売掛金額").Value = Me.導入売掛金額
			.Fields("メモ").Value = Me.メモ

			'UPGRADE_WARNING: オブジェクト Me.最終使用日 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Fields("最終使用日").Value = Me.最終使用日

			.Fields("登録変更日").Value = Today

			.Fields("集計CD").Value = Me.集計CD

			.Fields("チームCD").Value = Me.チームCD '2014/09/18 ADD

			.Update()
		End With

		Call ReleaseRs(rs)

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
		sql = sql & " FROM TM得意先"
		sql = sql & " WHERE 得意先CD = '" & SQLString((Me.得意先CD)) & "'"

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
		cmd.CommandText = "usp_ChkDelFor得意先"
		cmd.CommandType = ADODB.CommandTypeEnum.adCmdStoredProc

		' それぞれのパラメータの値を指定する
		With cmd.Parameters
			.Item(1).Value = Me.得意先CD
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

End Class