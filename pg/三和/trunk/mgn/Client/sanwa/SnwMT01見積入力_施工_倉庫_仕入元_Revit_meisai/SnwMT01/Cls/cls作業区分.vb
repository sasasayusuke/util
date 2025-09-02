Option Strict Off
Option Explicit On
Friend Class cls作業区分
	'///////////////////////////
	'作業区分クラス (員数入力で使用)
	'///////////////////////////
	
	'変数
	Private m_作業区分CD As String
	
	Private m_Name As String
	
	Private m_MaxLength As Integer '桁数
	Private m_EOF As Boolean '存在確認
	
	Private m_Error As Short 'エラー
	
	'//////////////////////////////////////
	'   作業区分CD
	'//////////////////////////////////////
	
	Public Property 作業区分CD() As String
		Get
			作業区分CD = m_作業区分CD
		End Get
		Set(ByVal Value As String)
			m_作業区分CD = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   作業区分名
	'//////////////////////////////////////
	
	Public Property 作業区分名() As String
		Get
			作業区分名 = m_Name
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
		m_MaxLength = 1
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
		m_作業区分CD = New String(" ", m_MaxLength + 1)
		m_Name = vbNullString
		
	End Sub
	
	'//////////////////////////////////////
	'   データを読み込むメソッド
	'//////////////////////////////////////
	Public Function GetbyID() As Boolean
		
		On Error GoTo GetbyID_Err
		
		'マウスポインターを砂時計にする
		'Call HourGlass(True)
		
		Select Case Me.作業区分CD
			Case ""
				GetbyID = True
				m_Name = vbNullString
			Case "1"
				GetbyID = True
				''            m_Name = "施工"
				m_Name = "工事" '2022/09/02 ADD
			Case "2"
				GetbyID = True
				m_Name = "コール"
			Case "3"
				GetbyID = True
				m_Name = "内装"
			Case Else
				GetbyID = False
				m_Name = vbNullString
		End Select
		Exit Function
GetbyID_Err: 
		'Call HourGlass(False)
		'エラーの生成
		Err.Raise(Err.Number,  , Err.Description)
	End Function
End Class