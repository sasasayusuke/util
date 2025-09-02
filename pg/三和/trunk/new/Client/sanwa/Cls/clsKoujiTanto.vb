Option Strict Off
Option Explicit On

''' <summary>
''' 工事担当クラス
''' </summary>
Friend Class clsKoujiTanto

	'変数
	Private m_工事担当CD As String

	Private m_Name As String

	Private m_Order As Short '順序

	Private m_MaxLength As Integer '桁数
	Private m_EOF As Boolean '存在確認

	'Private m_Error As Integer 'エラー

	'//////////////////////////////////////
	'   工事担当CD
	'//////////////////////////////////////
	Public Property 工事担当CD() As String
		Get
			[工事担当CD] = m_工事担当CD
		End Get
		Set(ByVal Value As String)
			m_工事担当CD = Value
		End Set
	End Property

	'//////////////////////////////////////
	'   工事担当名
	'//////////////////////////////////////
	Public Property 工事担当名() As String
		Get
			[工事担当名] = m_Name
		End Get
		Set(ByVal Value As String)
			m_Name = Value
		End Set
	End Property

	'//////////////////////////////////////
	'   順序
	'//////////////////////////////////////
	Public Property 順序() As Short
		Get
			[順序] = m_Order
		End Get
		Set(ByVal Value As Short)
			m_Order = Value
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
		m_工事担当CD = New String(" "c, m_MaxLength + 1)
		m_Name = vbNullString

	End Sub

	'//////////////////////////////////////
	'   データを読み込むメソッド
	'//////////////////////////////////////
	Public Function GetbyID() As Boolean
		Dim rs As ADODB.Recordset
		Dim sql As String

		If Me.工事担当CD = "" Then
			GetbyID = False
			m_工事担当CD = ""
			m_Name = ""
			Exit Function
		End If

		On Error GoTo GetbyID_Err

		'マウスポインターを砂時計にする
		'Call HourGlass(True)

		'SQL生成
		sql = "SELECT *"
		sql = sql & " FROM TM工事担当"
		sql = sql & " WHERE 工事担当CD = " & Me.工事担当CD

		'SQL実行
		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)

		With rs
			If .EOF Then
				GetbyID = False
				m_EOF = True

				m_工事担当CD = ""
				m_Name = ""
			Else
				GetbyID = True
				m_EOF = False

				m_工事担当CD = .Fields("工事担当CD").Value
				m_Name = .Fields("工事担当名").Value
				m_Order = .Fields("順序").Value
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
		sql = "SELECT  工事担当CD, 工事担当名, 順序"
		sql = sql & " FROM TM工事担当"

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
		whr = SQLIntRange("工事担当CD", 開始CD, 終了cd)
		If whr <> vbNullString Then
			whr = " WHERE " & whr & ""
		Else
			whr = ""
		End If

		'SQL生成
		sql = "SELECT  工事担当CD, 工事担当名"
		sql = sql & " FROM TM工事担当"
		sql = sql & " " & whr & " "
		sql = sql & " ORDER BY 工事担当CD"

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
			Call .SelSetup("SELECT 工事担当CD,工事担当名 FROM TM工事担当", "工事担当CD", "工事担当名", "", "工事担当CD", "工事担当選択", 1050, 2235)
			.ShowDialog()
			If .DialogResult_Renamed Then
				Me.工事担当CD = CStr(.DialogResultCode)
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

		sql = "SELECT  工事担当CD, 工事担当名, 順序"
		sql = sql & " ,初期登録日, 登録変更日"
		sql = sql & " FROM TM工事担当"
		sql = sql & " WHERE 工事担当CD = " & Me.工事担当CD

		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)

		With rs
			Select Case .EOF
				Case True
					.AddNew()
					.Fields("工事担当CD").Value = Me.工事担当CD

					.Fields("初期登録日").Value = Today
				Case False
			End Select

			.Fields("工事担当名").Value = Me.工事担当名

			.Fields("順序").Value = Me.順序

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
		sql = sql & " FROM TM工事担当"
		sql = sql & " WHERE 工事担当CD = " & Me.工事担当CD

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

		'      'Call HourGlass(True)
		'  
		'      ' コマンドを実行する接続先を指定する
		'      cmd.ActiveConnection = Cn
		'      cmd.CommandText = "usp_ChkDelFor工事担当"
		'      cmd.CommandType = adCmdStoredProc
		'  
		'      ' それぞれのパラメータの値を指定する
		'      With cmd.Parameters
		'          .Item(1).Value = Me.工事担当CD
		'      End With
		'  
		'      cmd.Execute
		'  
		'      If (cmd(0) = 0) Then
		'          PurgeChk = False
		'      Else
		'          PurgeChk = True
		'  ''        CriticalAlarm (cmd(3))
		'          'エラーの生成
		'          Err.Raise cmd(3), , cmd(4)
		'      End If
		'  
		'      Set cmd = Nothing
		'  
		'      'Call HourGlass(False)
	End Function
End Class