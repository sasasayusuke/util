Option Strict Off
Option Explicit On
Friend Class clsLoginControl
	'///////////////////////////
	'ログイン制御クラス
	'///////////////////////////
	'Ver.1.00           '2018.03.26     新設
	
	'変数
	Private m_LoginName As String
	Private m_AppTitle As String
	
	Private m_AppEnabled As Boolean
	Private m_AppUpDel As Boolean
	
	
	Private m_MaxLength As Integer '桁数
	Private m_EOF As Boolean '存在確認
	
	Private m_Error As Short 'エラー
	
	'//////////////////////////////////////
	'   LoginName
	'//////////////////////////////////////
	
	Public Property LoginName() As String
		Get
			[LoginName] = m_LoginName
		End Get
		Set(ByVal Value As String)
			m_LoginName = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   AppTitle
	'//////////////////////////////////////
	
	Public Property AppTitle() As String
		Get
			[AppTitle] = m_AppTitle
		End Get
		Set(ByVal Value As String)
			m_AppTitle = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   AppEnabled
	'//////////////////////////////////////
	
	Public Property AppEnabled() As Boolean
		Get
			[AppEnabled] = m_AppEnabled
		End Get
		Set(ByVal Value As Boolean)
			m_AppEnabled = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   AppUpDel
	'//////////////////////////////////////
	
	Public Property AppUpDel() As Boolean
		Get
			[AppUpDel] = m_AppUpDel
		End Get
		Set(ByVal Value As Boolean)
			m_AppUpDel = Value
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
	'UPGRADE_NOTE: eof は eof_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
	Public ReadOnly Property eof_Renamed() As Boolean
		Get
			eof_Renamed = m_EOF
		End Get
	End Property
	
	'//////////////////////////////////////
	'   変数の初期化
	'//////////////////////////////////////
	'UPGRADE_NOTE: Class_Initialize は Class_Initialize_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
	Private Sub Class_Initialize_Renamed()
		'初期化
		m_MaxLength = 10
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
		m_LoginName = New String(" ", m_MaxLength + 1)
		m_AppTitle = New String(" ", m_MaxLength + 1)
		m_AppEnabled = False
		
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
		sql = sql & " FROM TMログイン制御"
		sql = sql & " WHERE LoginName = '" & SQLString((Me.LoginName)) & "'"
		sql = sql & " AND AppTitle = '" & SQLString((Me.AppTitle)) & "'"
		
		'SQL実行
		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
		
		With rs
			If .eof Then
				GetbyID = False
				m_EOF = True
				
				'            Me.LoginName = ""
				Me.AppEnabled = False
				Me.AppUpDel = False
			Else
				GetbyID = True
				m_EOF = False
				
				Me.LoginName = .Fields("LoginName").Value
				Me.AppTitle = .Fields("AppTitle").Value
				Me.AppEnabled = .Fields("AppEnabled").Value
				Me.AppUpDel = .Fields("AppUpDel").Value
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
	'   Upload
	'//////////////////////////////////////
	Public Function Upload() As Boolean
		Dim rs As ADODB.Recordset
		Dim sql As String
		
		'マウスポインターを砂時計にする
		'Call HourGlass(True)
		
		On Error GoTo Trans_err
		
		'SQL生成
		sql = "SELECT *"
		sql = sql & " FROM TMログイン制御"
		sql = sql & " WHERE LoginName = '" & SQLString((Me.LoginName)) & "'"
		sql = sql & " AND AppTitle = '" & SQLString((Me.AppTitle)) & "'"
		
		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
		
		With rs
			Select Case .eof
				Case True
					.AddNew()
					.Fields("LoginName").Value = Me.LoginName
					.Fields("AppTitle").Value = Me.AppTitle
					
					.Fields("初期登録日").Value = Today
				Case False
			End Select
			
			.Fields("AppEnabled").Value = Me.AppEnabled
			.Fields("AppUpDel").Value = Me.AppUpDel
			
			.Fields("登録変更日").Value = Now 'Date
			
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
End Class