Option Strict Off
Option Explicit On
Friend Class clsDates
	'///////////////////////////
	'Datesクラス
	'///////////////////////////
	'Ver.1.01           '2013.11.26     更新日付をDateからNowに変更
	
	
	' コンピュータ名を取得する関数の宣言
	Private Declare Function GetComputerName Lib "kernel32.dll"  Alias "GetComputerNameA"(ByVal lpBuffer As String, ByRef nSize As Integer) As Integer
	
	' コンピュータ名の長さを示す定数の宣言
	Private Const MAX_COMPUTERNAME_LENGTH As Short = 127
	
	' コンピュータ名を取得するバッファの長さを示す定数の宣言
	Private Const COMPUTERNAMBUFFER_LENGTH As Integer = MAX_COMPUTERNAME_LENGTH + 1
	
	
	'変数
	Private m_DateID As String
	
	Private m_更新日付 As Date
	
	Private m_MaxLength As Integer '桁数
	Private m_EOF As Boolean '存在確認
	
	Private m_Error As Short 'エラー
	
	'INIファイルより読み込み
	Public Function GetPCName() As String
		Dim strComputerNameBuffer As New VB6.FixedLengthString(COMPUTERNAMBUFFER_LENGTH)
		Dim lngComputerNameLength As Integer
		Dim lngWin32apiResultCode As Integer
		
		' コンピュータ名の長さを設定
		lngComputerNameLength = Len(strComputerNameBuffer.Value)
		' コンピュータ名を取得
		lngWin32apiResultCode = GetComputerName(strComputerNameBuffer.Value, lngComputerNameLength)
		' コンピュータ名を表示
		GetPCName = Left(strComputerNameBuffer.Value, InStr(strComputerNameBuffer.Value, vbNullChar) - 1)
	End Function
	
	'//////////////////////////////////////
	'   DateID
	'//////////////////////////////////////
	
	Public Property DateID() As String
		Get
			[DateID] = m_DateID
		End Get
		Set(ByVal Value As String)
			m_DateID = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   更新日付
	'//////////////////////////////////////
	
	Public Property 更新日付() As Date
		Get
			[更新日付] = m_更新日付
		End Get
		Set(ByVal Value As Date)
			m_更新日付 = Value
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
		m_DateID = New String(" ", m_MaxLength + 1)
		m_更新日付 = CDate("1990/01/01")
		
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
		sql = sql & " FROM TMDates"
		sql = sql & " WHERE DateID = '" & SQLString((Me.DateID)) & "'"
		
		'SQL実行
		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
		
		With rs
			If .eof Then
				GetbyID = False
				m_EOF = True
				
				'            Me.DateID = ""
				Me.更新日付 = CDate("1990/01/01")
			Else
				GetbyID = True
				m_EOF = False
				
				Me.DateID = .Fields("DateID").Value
				Me.更新日付 = .Fields("更新日付").Value
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
		sql = sql & " FROM TMDates"
		sql = sql & " WHERE DateID = '" & SQLString((Me.DateID)) & "'"
		
		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
		
		With rs
			Select Case .eof
				Case True
					.AddNew()
					.Fields("DateID").Value = Me.DateID
					
					.Fields("初期登録日").Value = Today
				Case False
			End Select
			
			.Fields("更新日付").Value = Me.更新日付
			.Fields("UserID").Value = Me.GetPCName
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
	
	'//////////////////////////////////////
	'   Upload
	'   更新対象より指定日付が後の場合更新する
	'//////////////////////////////////////
	Public Function Upload_After() As Boolean
		Dim rs As ADODB.Recordset
		Dim sql As String
		
		'マウスポインターを砂時計にする
		'Call HourGlass(True)
		
		On Error GoTo Trans_err
		
		'SQL生成
		sql = "SELECT *"
		sql = sql & " FROM TMDates"
		sql = sql & " WHERE DateID = '" & SQLString((Me.DateID)) & "'"
		sql = sql & " AND 更新日付 < " & SQLDate((Me.更新日付), DBType)
		
		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
		
		With rs
			If Not .eof Then
				
				.Fields("更新日付").Value = Me.更新日付
				.Fields("UserID").Value = Me.GetPCName
				.Fields("登録変更日").Value = Now 'Date
				
				.Update()
			End If
		End With
		
		Call ReleaseRs(rs)
		
		Upload_After = True
		
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