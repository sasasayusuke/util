Option Strict Off
Option Explicit On
Friend Class clsTanto
	'///////////////////////////
	'担当者クラス
	'///////////////////////////
	'2014/06/05 oosawa      メールアドレス新設
	'2022/09/27 oosawa      問い合わせ先　読み込みのみ追加
	
	'変数
	Private m_担当者CD As String
	
	Private m_Name As String
	Private m_Order As Short '順序   2012/11/07 ADD
	Private m_MailAddress As String '2014/06/05 ADD
	
	Private m_toiawase As String '問合せ先 2022/09/27
	
	
	Private m_MaxLength As Integer '桁数
	Private m_EOF As Boolean '存在確認
	
	Private m_Error As Short 'エラー
	
	Private m_部署cls As clsBusyo '部署クラス
	
	'//////////////////////////////////////
	'   担当者CD
	'//////////////////////////////////////
	
	Public Property 担当者CD() As String
		Get
			[担当者CD] = m_担当者CD
		End Get
		Set(ByVal Value As String)
			m_担当者CD = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   担当者名
	'//////////////////////////////////////
	
	Public Property 担当者名() As String
		Get
			[担当者名] = m_Name
		End Get
		Set(ByVal Value As String)
			m_Name = Value
		End Set
	End Property
	'2012/11/07 ADD↓
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
	'2012/11/07 ADD↑
	'2012/12/12 ADD↓
	'//////////////////////////////////////
	'   部署クラス
	'//////////////////////////////////////
	
	Public Property 部署cls() As clsBusyo
		Get
			部署cls = m_部署cls
		End Get
		Set(ByVal Value As clsBusyo)
			m_部署cls = Value
		End Set
	End Property
	'2012/11/07 ADD↑
	'2014/06/05 ADD↓
	'//////////////////////////////////////
	'   メールアドレス
	'//////////////////////////////////////
	
	Public Property メールアドレス() As String
		Get
			[メールアドレス] = m_MailAddress
		End Get
		Set(ByVal Value As String)
			m_MailAddress = Value
		End Set
	End Property
	'2014/06/05 ADD↑
	'2022/09/27 ADD↓
	'//////////////////////////////////////
	'   問い合わせ先
	'//////////////////////////////////////
	
	Public Property 問い合わせ先() As String
		Get
			[問い合わせ先] = m_toiawase
		End Get
		Set(ByVal Value As String)
			m_toiawase = Value
		End Set
	End Property
	'2022/09/27 ADD↑
	
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
		'部署クラス生成
		m_部署cls = New clsBusyo '部署クラス
		
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
		'UPGRADE_NOTE: オブジェクト m_部署cls をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		m_部署cls = Nothing
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
		m_担当者CD = New String(" ", m_MaxLength + 1)
		m_Name = vbNullString
		
		m_部署cls.Initialize()
		
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
		sql = sql & " FROM TM担当者"
		sql = sql & " WHERE 担当者CD = " & Me.担当者CD
		
		'SQL実行
		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
		
		With rs
			If .EOF Then
				GetbyID = False
				m_EOF = True
				
				m_担当者CD = ""
				m_Name = ""
			Else
				GetbyID = True
				m_EOF = False
				
				m_担当者CD = .Fields("担当者CD").Value
				m_Name = .Fields("担当者名").Value
				m_Order = .Fields("順序").Value '2012/11/07 ADD
				Me.メールアドレス = .Fields("メールアドレス").Value '2014/06/05 ADD
				
				Me.問い合わせ先 = .Fields("問い合わせ先").Value '2022/09/27 ADD
				
				'2012/12/15 ADD
				m_部署cls.Initialize()
				m_部署cls.部署CD = .Fields("部署CD").Value
				Call m_部署cls.GetbyID()
				'2012/12/15 ADD
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
		''    sql = "SELECT  担当者CD, 担当者名, 順序"
		''    sql = sql & " FROM TM担当者"
		
		sql = "SELECT TN.担当者CD, TN.担当者名, TN.部署CD, BS.部署名, TN.順序, TN.初期登録日, TN.登録変更日"
		sql = sql & ",TN.メールアドレス" '2014/06/05 ADD
		sql = sql & ",TN.問い合わせ先" '2022/12/16 ADD
		sql = sql & " FROM TM担当者 AS TN"
		sql = sql & " LEFT JOIN TM部署 AS BS"
		sql = sql & " ON TN.部署CD = BS.部署CD"
		
		
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
		whr = SQLIntRange("TN.担当者CD", 開始CD, 終了cd)
		If whr <> vbNullString Then
			whr = " WHERE " & whr & ""
		Else
			whr = ""
		End If
		
		'SQL生成
		'    sql = "SELECT  担当者CD, 担当者名,順序"
		'    sql = sql & " FROM TM担当者"
		'    sql = sql & " " & whr & " "
		'    sql = sql & " ORDER BY 担当者CD"
		
		sql = "SELECT TN.担当者CD, TN.担当者名, TN.初期登録日, TN.登録変更日, TN.順序, TN.部署CD, BS.部署名"
		sql = sql & ",TN.メールアドレス" '2014/06/05 ADD
		sql = sql & ",TN.問い合わせ先" '2022/12/16 ADD
		sql = sql & " FROM TM担当者 AS TN"
		sql = sql & " LEFT JOIN TM部署 AS BS"
		sql = sql & " ON TN.部署CD = BS.部署CD"
		sql = sql & " " & whr & " "
		sql = sql & " ORDER BY 担当者CD"
		
		
		'DBクローズするのに必要
		Cn.CursorLocation = ADODB.CursorLocationEnum.adUseClient
		
		'SQL実行
		GetRsByID = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
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
		Dim fSentak As SentakNM_cls
		
		fSentak = New SentakNM_cls
		
		With fSentak
			Call .SelSetup("SELECT 担当者CD,担当者名 FROM TM担当者", "担当者CD", "担当者名", "", "担当者CD", "担当者選択", 1050, 2235)
			.tx_検索名.IMEMode = ExText.IMEModeType.全角ひらがな
			.ShowDialog()
			If .DialogResult_Renamed Then
				Me.担当者CD = CStr(.DialogResultCode)
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
		
		'    sql = "SELECT  担当者CD, 担当者名,順序"
		sql = "SELECT  担当者CD, 担当者名,順序,部署CD"
		sql = sql & " ,初期登録日, 登録変更日"
		sql = sql & ",メールアドレス" '2014/06/05 ADD
		sql = sql & ",問い合わせ先" '2022/12/16 ADD
		sql = sql & " FROM TM担当者"
		sql = sql & " WHERE 担当者CD = " & Me.担当者CD
		
		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
		
		With rs
			Select Case .EOF
				Case True
					.AddNew()
					.Fields("担当者CD").Value = Me.担当者CD
					
					.Fields("初期登録日").Value = Today
				Case False
			End Select
			
			.Fields("担当者名").Value = Me.担当者名
			.Fields("登録変更日").Value = Today
			
			.Fields("順序").Value = Me.順序 '2012/11/07 ADD
			'![部署CD] = m_部署cls.部署CD
			.Fields("部署CD").Value = Me.部署cls.部署CD
			
			.Fields("メールアドレス").Value = Me.メールアドレス '2014/06/05 ADD
			
			.Fields("問い合わせ先").Value = Me.問い合わせ先 '2022/12/16 ADD
			
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
		sql = sql & " FROM TM担当者"
		sql = sql & " WHERE 担当者CD = " & Me.担当者CD
		
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
		cmd.CommandText = "usp_ChkDelFor担当者"
		cmd.CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
		
		' それぞれのパラメータの値を指定する
		With cmd.Parameters
			.Item(1).Value = Me.担当者CD
		End With
		
		cmd.Execute()
		
		If (cmd.Parameters(0).Value = 0) Then
			PurgeChk = False
		Else
			PurgeChk = True
			'''        CriticalAlarm (cmd(3))
			'エラーの生成
			Err.Raise(cmd.Parameters(3).Value,  , cmd.Parameters(4))
		End If
		
		'UPGRADE_NOTE: オブジェクト cmd をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		cmd = Nothing
		
		'Call HourGlass(False)
	End Function
End Class