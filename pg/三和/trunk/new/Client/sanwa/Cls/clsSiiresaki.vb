Option Strict Off
Option Explicit On

''' <summary>
''' 仕入先マスタクラス
''' 2023/09/26  oosawa      インボイス登録番号追加
''' </summary>
Public Class clsSiiresaki

	'変数
	Private m_仕入先CD As String
	Private m_仕入先名1 As String
	Private m_仕入先名2 As String
	Private m_略称 As String
	Private m_フリガナ As String
	Private m_郵便番号 As String
	Private m_住所1 As String
	Private m_住所2 As String
	Private m_電話番号 As String
	Private m_FAX番号 As String
	Private m_仕入先担当者名 As String
	Private m_担当者CD As Short
	Private m_締日 As Short
	Private m_決算月 As Short

	Private m_税集計区分 As Short
	Private m_仕入端数 As Short

	Private m_消費税端数 As Short
	Private m_支払サイクル As Short
	Private m_支払予定日 As Short
	Private m_支払区分CD As Short
	Private m_支払方法 As Short
	Private m_支払様式 As Short
	Private m_導入支払金額 As Decimal
	Private m_導入買掛金額 As Decimal
	Private m_メモ As String

	'Private m_初期登録日 As Date
	'Private m_登録変更日 As Date
	Private m_最終使用日 As Object
	Private m_インボイス登録番号 As String '2023/09/26 ADD

	Private m_MaxLength As Integer '桁数
	Private m_EOF As Boolean '存在確認

	'Private m_Error As Integer 'エラー

	'//////////////////////////////////////
	'   仕入先CD
	'//////////////////////////////////////
	Public Property 仕入先CD() As String
		Get
			[仕入先CD] = m_仕入先CD
		End Get
		Set(ByVal Value As String)
			'    m_仕入先CD = vData'2014/03/31 DEL

			'2014/03/31 ADD
			If ISInt(Value) Then
				m_仕入先CD = CType(Value, Integer).ToString(New String("0"c, m_MaxLength))
			End If
		End Set
	End Property

	'//////////////////////////////////////
	'   仕入先名1
	'//////////////////////////////////////
	Public Property 仕入先名1() As String
		Get
			[仕入先名1] = m_仕入先名1
		End Get
		Set(ByVal Value As String)
			m_仕入先名1 = Value
		End Set
	End Property

	'//////////////////////////////////////
	'   仕入先名2
	'//////////////////////////////////////
	Public Property 仕入先名2() As String
		Get
			[仕入先名2] = m_仕入先名2
		End Get
		Set(ByVal Value As String)
			m_仕入先名2 = Value
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
	'   仕入先担当者名
	'//////////////////////////////////////
	Public Property 仕入先担当者名() As String
		Get
			[仕入先担当者名] = m_仕入先担当者名
		End Get
		Set(ByVal Value As String)
			m_仕入先担当者名 = Value
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
	'   仕入端数
	'//////////////////////////////////////
	Public Property 仕入端数() As Short
		Get
			[仕入端数] = m_仕入端数
		End Get
		Set(ByVal Value As Short)
			m_仕入端数 = Value
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
	'   支払サイクル
	'//////////////////////////////////////
	Public Property 支払サイクル() As Short
		Get
			[支払サイクル] = m_支払サイクル
		End Get
		Set(ByVal Value As Short)
			m_支払サイクル = Value
		End Set
	End Property

	'//////////////////////////////////////
	'   支払予定日
	'//////////////////////////////////////
	Public Property 支払予定日() As Short
		Get
			[支払予定日] = m_支払予定日
		End Get
		Set(ByVal Value As Short)
			m_支払予定日 = Value
		End Set
	End Property

	'//////////////////////////////////////
	'   支払区分CD
	'//////////////////////////////////////
	Public Property 支払区分CD() As Short
		Get
			[支払区分CD] = m_支払区分CD
		End Get
		Set(ByVal Value As Short)
			m_支払区分CD = Value
		End Set
	End Property

	'//////////////////////////////////////
	'   支払方法
	'//////////////////////////////////////
	Public Property 支払方法() As Short
		Get
			[支払方法] = m_支払方法
		End Get
		Set(ByVal Value As Short)
			m_支払方法 = Value
		End Set
	End Property

	'//////////////////////////////////////
	'   支払様式
	'//////////////////////////////////////
	Public Property 支払様式() As Short
		Get
			[支払様式] = m_支払様式
		End Get
		Set(ByVal Value As Short)
			m_支払様式 = Value
		End Set
	End Property

	'//////////////////////////////////////
	'   導入支払金額
	'//////////////////////////////////////
	Public Property 導入支払金額() As Decimal
		Get
			[導入支払金額] = m_導入支払金額
		End Get
		Set(ByVal Value As Decimal)
			m_導入支払金額 = Value
		End Set
	End Property

	'//////////////////////////////////////
	'   導入買掛金額
	'//////////////////////////////////////
	Public Property 導入買掛金額() As Decimal
		Get
			[導入買掛金額] = m_導入買掛金額
		End Get
		Set(ByVal Value As Decimal)
			m_導入買掛金額 = Value
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

	'2023/09/26 ADD↓
	'//////////////////////////////////////
	'   インボイス登録番号
	'//////////////////////////////////////
	Public Property インボイス登録番号() As String
		Get
			[インボイス登録番号] = m_インボイス登録番号
		End Get
		Set(ByVal Value As String)
			m_インボイス登録番号 = Value
		End Set
	End Property
	'2023/09/26 ADD↑

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
		m_仕入先CD = vbNullString
		m_仕入先名1 = vbNullString
		m_仕入先名2 = vbNullString
		m_略称 = vbNullString
		m_フリガナ = vbNullString
		m_郵便番号 = vbNullString
		m_住所1 = vbNullString
		m_住所2 = vbNullString
		m_電話番号 = vbNullString
		m_FAX番号 = vbNullString
		m_仕入先担当者名 = vbNullString
		m_担当者CD = 0
		m_締日 = 0
		m_決算月 = 0

		m_税集計区分 = 0
		m_仕入端数 = 0

		m_消費税端数 = 0
		m_支払サイクル = 0
		m_支払予定日 = 0
		m_支払区分CD = 0
		m_支払方法 = 0
		m_支払様式 = 0
		m_導入支払金額 = 0
		m_導入買掛金額 = 0
		m_メモ = vbNullString

		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト m_最終使用日 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		m_最終使用日 = System.DBNull.Value

		m_インボイス登録番号 = vbNullString '2023/09/26 ADD

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
		sql = sql & " FROM TM仕入先"
		sql = sql & " WHERE 仕入先CD = '" & SQLString((Me.仕入先CD)) & "'"

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

				Me.仕入先CD = .Fields("仕入先CD").Value
				Me.仕入先名1 = .Fields("仕入先名1").Value
				Me.仕入先名2 = .Fields("仕入先名2").Value
				Me.略称 = .Fields("略称").Value
				Me.フリガナ = .Fields("フリガナ").Value
				Me.郵便番号 = .Fields("郵便番号").Value
				Me.住所1 = .Fields("住所1").Value
				Me.住所2 = .Fields("住所2").Value
				Me.電話番号 = .Fields("電話番号").Value
				Me.FAX番号 = .Fields("FAX番号").Value
				Me.仕入先担当者名 = .Fields("仕入先担当者名").Value
				Me.担当者CD = .Fields("担当者CD").Value
				Me.締日 = .Fields("締日").Value
				Me.決算月 = .Fields("決算月").Value

				Me.税集計区分 = .Fields("税集計区分").Value
				Me.仕入端数 = .Fields("仕入端数").Value

				Me.消費税端数 = .Fields("消費税端数").Value
				Me.支払サイクル = .Fields("支払サイクル").Value
				Me.支払予定日 = .Fields("支払予定日").Value
				Me.支払区分CD = .Fields("支払区分CD").Value
				Me.支払方法 = .Fields("支払方法").Value
				Me.支払様式 = .Fields("支払様式").Value
				Me.導入支払金額 = .Fields("導入支払金額").Value
				Me.導入買掛金額 = .Fields("導入買掛金額").Value
				Me.メモ = .Fields("メモ").Value

				Me.最終使用日 = .Fields("最終使用日").Value

				Me.インボイス登録番号 = .Fields("インボイス登録番号").Value '2023/09/26 ADD
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
		sql = sql & " FROM TM仕入先"

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
		whr = SQLIntRange("仕入先CD", 開始CD, 終了cd)
		If whr <> vbNullString Then
			whr = " WHERE " & whr & ""
		Else
			whr = ""
		End If

		'SQL生成
		sql = "SELECT  *"
		sql = sql & " FROM TM仕入先"
		sql = sql & " " & whr & " "
		sql = sql & " ORDER BY 仕入先CD"

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
		Dim fSentak As SirSen_cls
		fSentak = New SirSen_cls

		ShowDialog = False

		With fSentak
			'        Call .SelSetup("仕入先選択" _
			''            , "SELECT 仕入先ID,仕入先店舗ID,仕入先名1,仕入先名2 FROM TM仕入先" _
			''            , "仕入先" _
			''            , Null, Null, Null, Null)
			'       Call .SelSetup( _
			''                        "SELECT 部門CD,部門名 FROM TM部門", _
			''                        "部門CD", "部門名", "", "部門CD", "部門選択", _
			''                        1050, 2235)
			.ShowDialog()
			If .DialogResult_Renamed Then
				Me.仕入先CD = .DialogResultCode
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

		'マウスポインターを砂時計にする
		'Call HourGlass(True)

		On Error GoTo Trans_err

		sql = "SELECT  *"
		sql = sql & " FROM TM仕入先"
		sql = sql & " WHERE 仕入先CD = '" & SQLString((Me.仕入先CD)) & "'"

		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)

		With rs
			Select Case .EOF
				Case True
					.AddNew()
					.Fields("仕入先CD").Value = Me.仕入先CD

					.Fields("初期登録日").Value = Today
				Case False
			End Select

			.Fields("仕入先名1").Value = Me.仕入先名1
			.Fields("仕入先名2").Value = Me.仕入先名2
			.Fields("略称").Value = Me.略称
			.Fields("フリガナ").Value = Me.フリガナ
			.Fields("郵便番号").Value = Me.郵便番号
			.Fields("住所1").Value = Me.住所1
			.Fields("住所2").Value = Me.住所2
			.Fields("電話番号").Value = Me.電話番号
			.Fields("FAX番号").Value = Me.FAX番号
			.Fields("仕入先担当者名").Value = Me.仕入先担当者名
			.Fields("担当者CD").Value = Me.担当者CD
			.Fields("締日").Value = Me.締日
			.Fields("決算月").Value = Me.決算月

			.Fields("税集計区分").Value = Me.税集計区分
			.Fields("仕入端数").Value = Me.仕入端数

			.Fields("消費税端数").Value = Me.消費税端数
			.Fields("支払サイクル").Value = Me.支払サイクル
			.Fields("支払予定日").Value = Me.支払予定日
			.Fields("支払区分CD").Value = Me.支払区分CD
			.Fields("支払方法").Value = Me.支払方法
			.Fields("支払様式").Value = Me.支払様式
			.Fields("導入支払金額").Value = Me.導入支払金額
			.Fields("導入買掛金額").Value = Me.導入買掛金額
			.Fields("メモ").Value = Me.メモ

			'UPGRADE_WARNING: オブジェクト Me.最終使用日 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Fields("最終使用日").Value = Me.最終使用日

			.Fields("インボイス登録番号").Value = Me.インボイス登録番号 '2023/09/26 ADD

			.Fields("登録変更日").Value = Today

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
		sql = sql & " FROM TM仕入先"
		sql = sql & " WHERE 仕入先CD = '" & SQLString((Me.仕入先CD)) & "'"

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
		cmd.CommandText = "usp_ChkDelFor仕入先"
		cmd.CommandType = ADODB.CommandTypeEnum.adCmdStoredProc

		' それぞれのパラメータの値を指定する
		With cmd.Parameters
			.Item(1).Value = Me.仕入先CD
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