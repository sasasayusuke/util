Option Strict Off
Option Explicit On

''' <summary>
''' 部署クラス
''' </summary>
Public Class clsBusyo

	'変数
	Private m_部署CD As String

	Private m_Name As String

	Private m_MaxLength As Integer '桁数
	Private m_EOF As Boolean '存在確認

	'Private m_Error As Integer 'エラー

	'//////////////////////////////////////
	'   部署CD
	'//////////////////////////////////////
	Public Property 部署CD() As String
		Get
			[部署CD] = m_部署CD
		End Get
		Set(ByVal Value As String)
			m_部署CD = Value
		End Set
	End Property

	'//////////////////////////////////////
	'   部署名
	'//////////////////////////////////////
	Public Property 部署名() As String
		Get
			[部署名] = m_Name
		End Get
		Set(ByVal Value As String)
			m_Name = Value
		End Set
	End Property

	'//////////////////////////////////////
	'   MaxLength
	'//////////////////////////////////////
	Public Property MaxLength() As Integer
		Get
			MaxLength = m_MaxLength
		End Get
		Set(ByVal Value As Integer)
			m_MaxLength = Value
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
		m_部署CD = New String(" "c, m_MaxLength + 1)
		m_Name = vbNullString

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
		sql = sql & " FROM TM部署"
		sql = sql & " WHERE 部署CD = " & Me.部署CD

		'SQL実行
		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)

		With rs
			If .EOF Then
				GetbyID = False
				m_EOF = True

				m_部署CD = ""
				m_Name = ""
			Else
				GetbyID = True
				m_EOF = False

				m_部署CD = .Fields("部署CD").Value
				m_Name = .Fields("部署名").Value
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
		sql = "SELECT  部署CD, 部署名"
		sql = sql & " FROM TM部署"

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
		whr = SQLIntRange("部署CD", 開始CD, 終了cd)
		If whr <> vbNullString Then
			whr = " WHERE " & whr & ""
		Else
			whr = ""
		End If

		'SQL生成
		sql = "SELECT  部署CD, 部署名"
		sql = sql & " FROM TM部署"
		sql = sql & " " & whr & " "
		sql = sql & " ORDER BY 部署CD"

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
		Dim fSentak As SentakNM_cls
		fSentak = New SentakNM_cls

		ShowDialog = False

		With fSentak
			Call .SelSetup("SELECT 部署CD,部署名 FROM TM部署", "部署CD", "部署名", "", "部署CD", "部署選択", 1050, 2235)
			.ShowDialog()
			If .DialogResult_Renamed Then
				Me.部署CD = CStr(.DialogResultCode)
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

		sql = "SELECT  部署CD, 部署名"
		sql = sql & " ,初期登録日, 登録変更日"
		sql = sql & " FROM TM部署"
		sql = sql & " WHERE 部署CD = " & Me.部署CD

		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)

		With rs
			Select Case .EOF
				Case True
					.AddNew()
					.Fields("部署CD").Value = Me.部署CD

					.Fields("初期登録日").Value = Today
				Case False
			End Select

			.Fields("部署名").Value = Me.部署名
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
		sql = sql & " FROM TM部署"
		sql = sql & " WHERE 部署CD = " & Me.部署CD

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
		cmd.CommandText = "usp_ChkDelFor部署"
		cmd.CommandType = ADODB.CommandTypeEnum.adCmdStoredProc

		' それぞれのパラメータの値を指定する
		With cmd.Parameters
			.Item(1).Value = Me.部署CD
		End With

		cmd.Execute()

		If (cmd.Parameters(0).Value = 0) Then
			PurgeChk = False
		Else
			PurgeChk = True
			''        CriticalAlarm (cmd(3))
			'エラーの生成
			Err.Raise(cmd.Parameters(3).Value,  , cmd.Parameters(4))
		End If

		'UPGRADE_NOTE: オブジェクト cmd をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		cmd = Nothing

		'Call HourGlass(False)
	End Function

End Class