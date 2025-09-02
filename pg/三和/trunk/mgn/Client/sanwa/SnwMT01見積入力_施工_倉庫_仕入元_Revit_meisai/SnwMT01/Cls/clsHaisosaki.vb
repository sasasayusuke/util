Option Strict Off
Option Explicit On
Friend Class clsHaisosaki
	'///////////////////////////
	'得意先マスタクラス
	'///////////////////////////
	
	'変数
	Private m_配送先CD As String
	Private m_配送先名1 As String
	Private m_配送先名2 As String
	Private m_略称 As String
	Private m_フリガナ As String
	Private m_郵便番号 As String
	Private m_住所1 As String
	Private m_住所2 As String
	Private m_電話番号 As String
	Private m_FAX番号 As String
	Private m_配送先担当者名 As String
	Private m_メモ As String
	
	Private m_初期登録日 As Date
	Private m_登録変更日 As Date
	
	
	Private m_MaxLength As Integer '桁数
	Private m_EOF As Boolean '存在確認
	
	Private m_Error As Short 'エラー
	
	'//////////////////////////////////////
	'   配送先CD
	'//////////////////////////////////////
	
	Public Property 配送先CD() As String
		Get
			[配送先CD] = m_配送先CD
		End Get
		Set(ByVal Value As String)
			m_配送先CD = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   配送先名1
	'//////////////////////////////////////
	
	Public Property 配送先名1() As String
		Get
			[配送先名1] = m_配送先名1
		End Get
		Set(ByVal Value As String)
			m_配送先名1 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   配送先名2
	'//////////////////////////////////////
	
	Public Property 配送先名2() As String
		Get
			[配送先名2] = m_配送先名2
		End Get
		Set(ByVal Value As String)
			m_配送先名2 = Value
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
	'   配送先担当者名
	'//////////////////////////////////////
	
	Public Property 配送先担当者名() As String
		Get
			[配送先担当者名] = m_配送先担当者名
		End Get
		Set(ByVal Value As String)
			m_配送先担当者名 = Value
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
		m_MaxLength = 2
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
		m_配送先CD = vbNullString
		m_配送先名1 = vbNullString
		m_配送先名2 = vbNullString
		m_略称 = vbNullString
		m_フリガナ = vbNullString
		m_郵便番号 = vbNullString
		m_住所1 = vbNullString
		m_住所2 = vbNullString
		m_電話番号 = vbNullString
		m_FAX番号 = vbNullString
		m_配送先担当者名 = vbNullString
		m_メモ = vbNullString
		
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
		sql = sql & " FROM TM配送先"
		sql = sql & " WHERE 配送先CD = '" & SQLString((Me.配送先CD)) & "'"
		
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
				
				Me.配送先CD = .Fields("配送先CD").Value
				Me.配送先名1 = .Fields("配送先名1").Value
				Me.配送先名2 = .Fields("配送先名2").Value
				Me.略称 = .Fields("略称").Value
				Me.フリガナ = .Fields("フリガナ").Value
				Me.郵便番号 = .Fields("郵便番号").Value
				Me.住所1 = .Fields("住所1").Value
				Me.住所2 = .Fields("住所2").Value
				Me.電話番号 = .Fields("電話番号").Value
				Me.FAX番号 = .Fields("FAX番号").Value
				Me.配送先担当者名 = .Fields("配送先担当者名").Value
				Me.メモ = .Fields("メモ").Value
				
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
		sql = "SELECT *"
		sql = sql & " FROM TM配送先"
		
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
	
	''''//////////////////////////////////////
	''''   レコードを読み込むメソッド
	''''//////////////////////////////////////
	'''Public Function GetRsByID(ByVal 開始CD As Variant, ByVal 終了cd As Variant) As ADODB.Recordset
	'''    Dim rs As ADODB.Recordset
	'''    Dim sql As String
	'''    Dim whr As String
	'''
	'''    On Error GoTo GetRs_Err
	'''
	'''    'マウスポインターを砂時計にする
	'''    'Call HourGlass(True)
	'''
	'''    '-------抽出条件セット
	'''    whr = SQLIntRange("部門CD", 開始CD, 終了cd)
	'''    If whr <> vbNullString Then
	'''        whr = " WHERE " & whr & ""
	'''    Else
	'''        whr = ""
	'''    End If
	'''
	'''    'SQL生成
	'''    sql = "SELECT  部門CD, 部門名"
	'''    sql = sql & " FROM TM部門"
	'''    sql = sql & " " & whr & " "
	'''    sql = sql & " ORDER BY 部門CD"
	'''
	'''    'DBクローズするのに必要
	'''    Cn.CursorLocation = adUseClient
	'''
	'''    'SQL実行
	'''    Set GetRsByID = OpenRs(sql, Cn, adOpenForwardOnly, adLockReadOnly)
	'''    Set GetAllRs.ActiveConnection = Nothing
	'''
	'''    'DBクローズするのに必要
	'''    Cn.CursorLocation = adUseServer
	''''    Call ReleaseRs(rs)
	'''
	'''    'Call HourGlass(False)
	'''    Exit Function
	'''GetRs_Err:
	'''    'Call HourGlass(False)
	'''    'エラーの生成
	'''    Err.Raise Err.Number, , Err.Description
	'''
	'''End Function
	
	'//////////////////////////////////////
	'   選択画面
	'//////////////////////////////////////
	Public Function ShowDialog() As Boolean
		Dim fSentak As HaiSen_cls
		
		fSentak = New HaiSen_cls
		
		With fSentak
			.ShowDialog()
			If .DialogResult_Renamed Then
				Me.配送先CD = .DialogResultCode
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
		
		sql = "SELECT *"
		sql = sql & " FROM TM配送先"
		sql = sql & " WHERE 配送先CD = '" & SQLString((Me.配送先CD)) & "'"
		
		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
		
		With rs
			Select Case .EOF
				Case True
					.AddNew()
					.Fields("配送先CD").Value = Me.配送先CD
					
					.Fields("初期登録日").Value = Today
				Case False
			End Select
			
			.Fields("配送先名1").Value = Me.配送先名1
			.Fields("配送先名2").Value = Me.配送先名2
			.Fields("略称").Value = Me.略称
			.Fields("フリガナ").Value = Me.フリガナ
			.Fields("郵便番号").Value = Me.郵便番号
			.Fields("住所1").Value = Me.住所1
			.Fields("住所2").Value = Me.住所2
			.Fields("電話番号").Value = Me.電話番号
			.Fields("FAX番号").Value = Me.FAX番号
			.Fields("配送先担当者名").Value = Me.配送先担当者名
			.Fields("メモ").Value = Me.メモ
			
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
		sql = sql & " FROM TM配送先"
		sql = sql & " WHERE 配送先CD = '" & SQLString((Me.配送先CD)) & "'"
		
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
		cmd.CommandText = "usp_ChkDelFor配送先"
		cmd.CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
		
		' それぞれのパラメータの値を指定する
		With cmd.Parameters
			.Item(1).Value = Me.配送先CD
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