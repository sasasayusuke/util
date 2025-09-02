Option Strict Off
Option Explicit On

''' <summary>
''' 得意先マスタクラス
''' </summary>
Friend Class clsKokyakutmp

	'変数

	''    [得意先CD] [varchar](4) NOT NULL,
	''    [テンプレート名] [varchar](40) NOT NULL,
	''    [行NO] [int] NOT NULL,
	''    [PC区分] [varchar](1) NOT NULL,
	''    [製品NO] [char](7) NOT NULL,
	''    [仕様NO] [char](7) NOT NULL,
	''    [ベース色] [char](7) NOT NULL,
	''    [漢字名称] [varchar](40) NOT NULL,
	''    [W] [int] NOT NULL,
	''    [D] [int] NOT NULL,
	''    [H] [int] NOT NULL,
	''    [D1] [int] NOT NULL,
	''    [D2] [int] NOT NULL,
	''    [H1] [int] NOT NULL,
	''    [H2] [int] NOT NULL,
	''    [単位名] [varchar](6) NOT NULL,
	''    [仕入先CD] [varchar](4) NOT NULL,
	''    [製品区分] [smallint] NULL,
	''    [初期登録日] [datetime] NULL,
	''    [登録変更日] [datetime] NULL,


	Private m_得意先CD As String
	Private m_テンプレート名 As String
	''Private m_納入先名1         As String
	''Private m_納入先名2         As String
	''Private m_略称              As String
	''Private m_フリガナ          As String
	''Private m_郵便番号          As String
	''Private m_住所1             As String
	''Private m_住所2             As String
	''Private m_電話番号          As String
	''Private m_FAX番号           As String
	''Private m_納入先担当者名    As String
	''Private m_メモ              As String
	''
	''Private m_初期登録日        As Date
	''Private m_登録変更日        As Date
	''Private m_最終使用日        As Variant


	Private m_MaxLength As Integer '桁数
	Private m_EOF As Boolean '存在確認

	'Private m_Error As Integer 'エラー

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
	'   テンプレート名
	'//////////////////////////////////////
	Public Property テンプレート名() As String
		Get
			[テンプレート名] = m_テンプレート名
		End Get
		Set(ByVal Value As String)
			m_テンプレート名 = Value
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
		m_MaxLength = 40
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
		m_テンプレート名 = vbNullString

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
		sql = sql & " FROM TM納入先"
		sql = sql & " WHERE 得意先CD = '" & SQLString((Me.得意先CD)) & "'"
		sql = sql & " AND テンプレート名 = '" & SQLString((Me.テンプレート名)) & "'"

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
				Me.テンプレート名 = .Fields("テンプレート名").Value
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
	'   選択画面
	'//////////////////////////////////////
	Public Function ShowDialog() As Boolean
		Dim fSentak As SelKokTmp_cls
		fSentak = New SelKokTmp_cls

		ShowDialog = False

		With fSentak
			.[得意先CD] = Me.得意先CD
			.ShowDialog()
			If .DialogResult_Renamed Then
				Me.テンプレート名 = .DialogResultCode
				ShowDialog = True
			End If
		End With

		'UPGRADE_NOTE: オブジェクト fSentak をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		fSentak = Nothing

	End Function
End Class