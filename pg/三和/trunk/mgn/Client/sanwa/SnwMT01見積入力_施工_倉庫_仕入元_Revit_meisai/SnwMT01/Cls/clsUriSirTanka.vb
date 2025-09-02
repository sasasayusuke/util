Option Strict Off
Option Explicit On
Friend Class clsUriSirTanka
	'///////////////////////////
	'売上仕入単価マスタクラス
	'///////////////////////////
	'2015/09/25 oosawa      新設
	
	'変数
	Private m_得意先cls As clsTokuisaki '得意先クラス
	Private m_仕入先cls As clsSiiresaki '仕入先クラス
	'Private m_製品cls As clsSeihin        '製品クラス
	
	'Private m_仕入先CD          As String
	Private m_PC区分 As String
	Private m_製品NO As String
	Private m_仕様NO As String
	Private m_大口売上単価 As Decimal
	Private m_大口仕入単価 As Decimal
	Private m_小口売上単価 As Decimal
	Private m_小口仕入単価 As Decimal
	Private m_初期登録日 As Object
	
	Private m_MaxLength As Integer '桁数
	Private m_EOF As Boolean '存在確認
	
	Private m_Error As Short 'エラー
	
	'//////////////////////////////////////
	'   得意先CD
	'//////////////////////////////////////
	
	Public Property 得意先CD() As String
		Get
			[得意先CD] = Me.得意先cls.得意先CD
		End Get
		Set(ByVal Value As String)
			Me.得意先cls.得意先CD = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   仕入先CD
	'//////////////////////////////////////
	
	Public Property 仕入先CD() As String
		Get
			[仕入先CD] = Me.仕入先cls.仕入先CD
		End Get
		Set(ByVal Value As String)
			Me.仕入先cls.仕入先CD = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   PC区分
	'//////////////////////////////////////
	
	Public Property PC区分() As String
		Get
			[PC区分] = Trim(m_PC区分)
		End Get
		Set(ByVal Value As String)
			m_PC区分 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   製品NO
	'//////////////////////////////////////
	
	Public Property 製品NO() As String
		Get
			[製品NO] = Trim(m_製品NO)
		End Get
		Set(ByVal Value As String)
			m_製品NO = Value
		End Set
	End Property
	'//////////////////////////////////////
	'   仕様NO
	'//////////////////////////////////////
	
	Public Property 仕様NO() As String
		Get
			[仕様NO] = Trim(m_仕様NO)
		End Get
		Set(ByVal Value As String)
			m_仕様NO = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   得意先クラス
	'//////////////////////////////////////
	
	Public Property 得意先cls() As clsTokuisaki
		Get
			得意先cls = m_得意先cls
		End Get
		Set(ByVal Value As clsTokuisaki)
			m_得意先cls = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   仕入先クラス
	'//////////////////////////////////////
	
	Public Property 仕入先cls() As clsSiiresaki
		Get
			仕入先cls = m_仕入先cls
		End Get
		Set(ByVal Value As clsSiiresaki)
			m_仕入先cls = Value
		End Set
	End Property
	
	''''//////////////////////////////////////
	''''   製品クラス
	''''//////////////////////////////////////
	'''Public Property Set 製品cls(ByVal vData As clsSeihin)
	'''    Set m_製品cls = vData
	'''End Property
	'''
	'''Public Property Get 製品cls() As clsSeihin
	'''    Set 製品cls = m_製品cls
	'''End Property
	
	'//////////////////////////////////////
	'   大口売上単価
	'//////////////////////////////////////
	
	Public Property 大口売上単価() As Decimal
		Get
			[大口売上単価] = m_大口売上単価
		End Get
		Set(ByVal Value As Decimal)
			m_大口売上単価 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   大口仕入単価
	'//////////////////////////////////////
	
	Public Property 大口仕入単価() As Decimal
		Get
			[大口仕入単価] = m_大口仕入単価
		End Get
		Set(ByVal Value As Decimal)
			m_大口仕入単価 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   小口売上単価
	'//////////////////////////////////////
	
	Public Property 小口売上単価() As Decimal
		Get
			[小口売上単価] = m_小口売上単価
		End Get
		Set(ByVal Value As Decimal)
			m_小口売上単価 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   小口仕入単価
	'//////////////////////////////////////
	
	Public Property 小口仕入単価() As Decimal
		Get
			[小口仕入単価] = m_小口仕入単価
		End Get
		Set(ByVal Value As Decimal)
			m_小口仕入単価 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   初期登録日
	'//////////////////////////////////////
	Public ReadOnly Property 初期登録日() As Object
		Get
			'UPGRADE_WARNING: オブジェクト m_初期登録日 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト 初期登録日 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[初期登録日] = m_初期登録日
		End Get
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
		'得意先クラス生成
		m_得意先cls = New clsTokuisaki
		'仕入先クラス生成
		m_仕入先cls = New clsSiiresaki
		'製品クラス生成
		'    Set m_製品cls = New clsSeihin
		
		'初期化
		m_MaxLength = 100
		'初期化
		Call Initialize()
		
	End Sub
	Public Sub New()
		MyBase.New()
		Class_Initialize_Renamed()
	End Sub
	
	'UPGRADE_NOTE: Class_Terminate は Class_Terminate_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
	Private Sub Class_Terminate_Renamed()
		'nop
		'UPGRADE_NOTE: オブジェクト m_得意先cls をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		m_得意先cls = Nothing '得意先クラス
		'UPGRADE_NOTE: オブジェクト m_仕入先cls をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		m_仕入先cls = Nothing '仕入先クラス
		'    Set m_製品cls = Nothing     '製品クラス
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
		m_得意先cls.Initialize()
		m_仕入先cls.Initialize()
		
		'    m_製品cls.Initialize
		'    m_製品cls.isDo在庫管理 = Type在庫管理.全て
		'    m_製品cls.isDo廃盤表示 = 廃盤表示しない
		
		m_大口売上単価 = 0
		m_小口売上単価 = 0
		m_大口仕入単価 = 0
		m_小口仕入単価 = 0
		
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト m_初期登録日 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		m_初期登録日 = System.DBNull.Value
		
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
		sql = sql & " FROM TM売上仕入単価"
		sql = sql & " WHERE 得意先CD = '" & SQLString((Me.得意先CD)) & "'"
		sql = sql & " AND 仕入先CD = '" & SQLString((Me.仕入先CD)) & "'"
		sql = sql & " AND PC区分 = '" & SQLString((Me.PC区分)) & "'"
		sql = sql & " AND 製品NO = '" & SQLString((Me.製品NO)) & "'"
		sql = sql & " AND 仕様NO = '" & SQLString((Me.仕様NO)) & "'"
		
		'SQL実行
		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
		
		With rs
			If .EOF Then
				GetbyID = False
				m_EOF = True
				
				Me.大口売上単価 = 0
				Me.小口売上単価 = 0
				Me.大口仕入単価 = 0
				Me.小口仕入単価 = 0
			Else
				GetbyID = True
				m_EOF = False
				
				Me.大口売上単価 = .Fields("大口売上単価").Value
				Me.小口売上単価 = .Fields("小口売上単価").Value
				Me.大口仕入単価 = .Fields("大口仕入単価").Value
				Me.小口仕入単価 = .Fields("小口仕入単価").Value
				'UPGRADE_WARNING: オブジェクト m_初期登録日 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				m_初期登録日 = .Fields("初期登録日").Value
				
				'得意先M
				Me.得意先cls.Initialize()
				Me.得意先cls.得意先CD = .Fields("得意先CD").Value
				Me.得意先cls.GetbyID()
				'仕入先M
				Me.仕入先cls.Initialize()
				Me.仕入先cls.仕入先CD = .Fields("仕入先CD").Value
				Me.仕入先cls.GetbyID()
				
				'''            '製品M
				'''            Me.製品cls.Initialize
				'''            Me.製品cls.製品NO = ![製品NO]
				'''            Me.製品cls.仕様NO = ![仕様NO]
				'''            Me.製品cls.GetbyID
				
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
	'''''
	''''''//////////////////////////////////////
	''''''   データを読み込むメソッド
	''''''//////////////////////////////////////
	'''''Public Function GetbyID4Tanka(i大小口区分 As Integer) As Boolean
	'''''    Dim rs As ADODB.Recordset
	'''''    Dim sql As String
	'''''
	'''''    On Error GoTo GetbyID4Tanka_Err
	'''''
	'''''    'マウスポインターを砂時計にする
	'''''    'Call HourGlass(True)
	'''''
	'''''    'SQL生成
	'''''    sql = "SELECT *"
	'''''    sql = sql & " FROM TM売上仕入単価"
	'''''    sql = sql & " WHERE 得意先CD = '" & SQLString(Me.得意先CD) & "'"
	'''''    sql = sql & " AND 仕入先CD = '" & SQLString(Me.仕入先CD) & "'"
	'''''    sql = sql & " AND PC区分 = '" & SQLString(Me.PC区分) & "'"
	'''''    sql = sql & " AND 製品NO = '" & SQLString(Me.製品NO) & "'"
	'''''    sql = sql & " AND 仕様NO = '" & SQLString(Me.仕様NO) & "'"
	'''''
	'''''    'SQL実行
	'''''    Set rs = OpenRs(sql, Cn, adOpenForwardOnly, adLockReadOnly)
	'''''
	'''''    With rs
	'''''        If .EOF Then
	'''''            GetbyID4Tanka = False
	'''''            m_EOF = True
	'''''
	'''''            Me.大口売上単価 = 0
	'''''            Me.小口売上単価 = 0
	'''''            Me.大口仕入単価 = 0
	'''''            Me.小口仕入単価 = 0
	'''''        Else
	'''''            GetbyID4Tanka = True
	'''''            m_EOF = False
	'''''
	'''''            If [tx_大小口区分].Text = 0 Then
	'''''                Genka = NullToZero(![大口仕入単価], 0)
	'''''                大小口売上単価 = NullToZero(![大口売上単価], 0)
	'''''            Else
	'''''                Genka = NullToZero(![小口仕入単価], 0)
	'''''                Baika = NullToZero(![小口売上単価], 0)
	'''''            End If
	'''''
	'''''            Me.大口売上単価 = ![大口売上単価]
	'''''            Me.小口売上単価 = ![小口売上単価]
	'''''            Me.大口仕入単価 = ![大口仕入単価]
	'''''            Me.小口仕入単価 = ![小口仕入単価]
	'''''            m_初期登録日 = ![初期登録日]
	'''''
	'''''''            '得意先M
	'''''''            Me.得意先cls.Initialize
	'''''''            Me.得意先cls.得意先CD = ![得意先CD]
	'''''''            Me.得意先cls.GetbyID
	'''''''            '仕入先M
	'''''''            Me.仕入先cls.Initialize
	'''''''            Me.仕入先cls.仕入先CD = ![仕入先CD]
	'''''''            Me.仕入先cls.GetbyID
	'''''''
	'''''
	'''''        End If
	'''''    End With
	'''''
	'''''    Call ReleaseRs(rs)
	'''''
	'''''    'Call HourGlass(False)
	'''''    Exit Function
	'''''GetbyID4Tanka_Err:
	'''''    'Call HourGlass(False)
	'''''    'エラーの生成
	'''''    Err.Raise Err.Number, , Err.Description
	'''''End Function
	
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
		sql = sql & " FROM TM売上仕入単価"
		sql = sql & " ORDER BY 得意先CD,仕入先CD,製品NO,仕様NO"
		
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
	Public Function GetAllRsByTokuSirCD(ByVal s得意先CD As String, ByVal s仕入先CD As String) As ADODB.Recordset
		Dim rs As ADODB.Recordset
		Dim sql As String
		
		On Error GoTo GetRs_Err
		
		'マウスポインターを砂時計にする
		'Call HourGlass(True)
		
		'SQL生成
		sql = "SELECT TN.得意先CD, TN.PC区分, TN.製品NO, TN.仕様NO,"
		sql = sql & " 名称=COALESCE(SE.漢字名称,PC.漢字名称),"
		sql = sql & " ISNULL(TN.大口売上単価,0), ISNULL(TN.大口仕入単価,0),"
		sql = sql & " ISNULL(TN.小口売上単価,0), ISNULL(TN.小口仕入単価,0),"
		sql = sql & " 製品サイズ=(CASE WHEN SE.W = 0 THEN '' ELSE 'W=' + LTRIM(STR(SE.W) + ' ') END"
		sql = sql & " + CASE WHEN SE.D = 0 THEN '' ELSE 'D=' + LTRIM(STR(SE.D) + ' ') END"
		sql = sql & " + CASE WHEN SE.H = 0 THEN '' ELSE 'H=' + LTRIM(STR(SE.H) + ' ') END"
		sql = sql & " + CASE WHEN SE.D1 = 0 THEN '' ELSE 'D1=' + LTRIM(STR(SE.D1) + ' ') END"
		sql = sql & " + CASE WHEN SE.D2 = 0 THEN '' ELSE 'D2=' + LTRIM(STR(SE.D2) + ' ') END"
		sql = sql & " + CASE WHEN SE.H1 = 0 THEN '' ELSE 'H1=' + LTRIM(STR(SE.H1) + ' ') END"
		sql = sql & " + CASE WHEN SE.H2 = 0 THEN '' ELSE 'H2=' + LTRIM(STR(SE.H2) + ' ') END), "
		sql = sql & " PCサイズ=(CASE WHEN PC.W = 0 THEN '' ELSE 'W=' + LTRIM(STR(PC.W) + ' ') END"
		sql = sql & " + CASE WHEN PC.D = 0 THEN '' ELSE 'D=' + LTRIM(STR(PC.D) + ' ') END"
		sql = sql & " + CASE WHEN PC.H = 0 THEN '' ELSE 'H=' + LTRIM(STR(PC.H) + ' ') END"
		sql = sql & " + CASE WHEN PC.径 = 0 THEN '' ELSE '径=' + LTRIM(STR(PC.径) + ' ') END"
		sql = sql & " + CASE WHEN PC.T = 0 THEN '' ELSE 'T=' + LTRIM(STR(PC.T) + ' ') END) "
		sql = sql & " FROM TM売上仕入単価 AS TN"
		sql = sql & " LEFT JOIN TM製品情報 AS SJ"
		sql = sql & " ON TN.PC区分 = SJ.PC区分 AND TN.製品NO = SJ.製品NO AND TN.仕様NO = SJ.仕様NO"
		sql = sql & " LEFT JOIN TM製品 AS SE"
		sql = sql & " ON SJ.PC区分 = '' AND SJ.製品NO = SE.製品NO AND SJ.仕様NO = SE.仕様NO"
		sql = sql & " LEFT JOIN TMPC AS PC"
		sql = sql & " ON SJ.PC区分 = PC.PC区分 AND SJ.製品NO = PC.製品NO"
		
		sql = sql & " WHERE TN.得意先CD = '" & SQLString(s得意先CD) & "'"
		sql = sql & " AND TN.仕入先CD = '" & SQLString(s仕入先CD) & "'"
		sql = sql & " AND SE.廃盤FLG = 0"
		sql = sql & " ORDER BY TN.PC区分, TN.製品NO, TN.仕様NO"
		
		
		'DBクローズするのに必要
		Cn.CursorLocation = ADODB.CursorLocationEnum.adUseClient
		
		'SQL実行
		GetAllRsByTokuSirCD = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
		'UPGRADE_NOTE: オブジェクト GetAllRsByTokuSirCD.ActiveConnection をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		GetAllRsByTokuSirCD.ActiveConnection = Nothing
		
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
	Public Function GetRsByID(ByVal s得意先CD As Object, ByVal e得意先CD As Object, ByVal s仕入先CD As Object, ByVal e仕入先CD As Object) As ADODB.Recordset
		Dim rs As ADODB.Recordset
		Dim sql As String
		Dim whr As String
		
		On Error GoTo GetRs_Err
		
		'マウスポインターを砂時計にする
		'Call HourGlass(True)
		
		'-------抽出条件セット
		If IsCheckNull(s得意先CD) = False Or IsCheckNull(e得意先CD) = False Then
			If whr <> "" Then
				whr = whr & " AND "
			End If
			whr = whr & SQLStringRange("TK.得意先CD", s得意先CD, e得意先CD)
		End If
		If IsCheckNull(s仕入先CD) = False Or IsCheckNull(e仕入先CD) = False Then
			If whr <> "" Then
				whr = whr & " AND "
			End If
			whr = whr & SQLStringRange("SR.仕入先CD", s仕入先CD, e仕入先CD)
		End If
		
		If whr <> "" Then
			whr = whr & " AND "
		End If
		whr = whr & "SE.廃盤FLG = 0 "
		
		
		If whr <> "" Then
			whr = " WHERE " & whr
		End If
		
		'SQL生成
		sql = "SELECT TN.得意先CD, TK.得意先名1, TN.仕入先CD,SR.仕入先名1,"
		sql = sql & " TN.PC区分, TN.製品NO, TN.仕様NO,"
		sql = sql & " 漢字名称=ISNULL(COALESCE(SE.漢字名称,PC.漢字名称),''),"
		sql = sql & " SE.W,SE.D,SE.H,SE.D1,SE.D2,SE.H1,SE.H2, "
		sql = sql & " ISNULL(TN.大口売上単価,0) AS 大口売上単価, ISNULL(TN.大口仕入単価,0) AS 大口仕入単価,"
		sql = sql & " ISNULL(TN.小口売上単価,0) AS 小口売上単価, ISNULL(TN.小口仕入単価,0) AS 小口仕入単価"
		sql = sql & " FROM TM売上仕入単価 AS TN"
		sql = sql & " LEFT JOIN TM得意先 AS TK ON TN.得意先CD = TK.得意先CD"
		sql = sql & " LEFT JOIN TM仕入先 AS SR ON TN.仕入先CD = SR.仕入先CD"
		sql = sql & " LEFT JOIN TM製品情報 AS SJ ON TN.PC区分 = SJ.PC区分 AND TN.製品NO = SJ.製品NO AND TN.仕様NO = SJ.仕様NO"
		sql = sql & " LEFT JOIN TM製品 AS SE ON SJ.PC区分 = '' AND SJ.製品NO = SE.製品NO AND SJ.仕様NO = SE.仕様NO"
		sql = sql & " LEFT JOIN TMPC AS PC ON SJ.PC区分 = PC.PC区分 AND SJ.製品NO = PC.製品NO"
		sql = sql & whr
		sql = sql & " ORDER BY TN.得意先CD, TN.仕入先CD,TN.PC区分, TN.製品NO, TN.仕様NO"
		
		
		'DBクローズするのに必要
		Cn.CursorLocation = ADODB.CursorLocationEnum.adUseClient
		
		'SQL実行
		GetRsByID = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
		'UPGRADE_NOTE: オブジェクト GetRsByID.ActiveConnection をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
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
	''''
	'''''//////////////////////////////////////
	'''''   選択画面
	'''''//////////////////////////////////////
	''''Public Function ShowDialog() As Boolean
	''''    Dim fSentak As SentakNM_cls
	''''
	''''    Set fSentak = New SentakNM_cls
	''''
	''''    With fSentak
	''''        Call .SelSetup( _
	'''''                        "SELECT ウエルシア物件内容CD,ウエルシア物件内容名 FROM TMウエルシア物件内容", _
	'''''                        "ウエルシア物件内容CD", "ウエルシア物件内容名", "", "ウエルシア物件内容CD", "ウエルシア物件内容選択", _
	'''''                        1050, 5000)
	''''        .tx_検索名.IMEMode = 全角ひらがな
	''''        .Show vbModal
	''''        If .DialogResult Then
	''''            Me.ウエルシア物件内容CD = .DialogResultCode
	''''            ShowDialog = True
	''''        End If
	''''    End With
	''''
	''''    Set fSentak = Nothing
	''''
	''''End Function
	
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
		sql = sql & " FROM TM売上仕入単価"
		sql = sql & " WHERE 得意先CD = '" & SQLString((Me.得意先CD)) & "'"
		sql = sql & " AND 仕入先CD = '" & SQLString((Me.仕入先CD)) & "'"
		sql = sql & " AND PC区分 = '" & SQLString((Me.PC区分)) & "'"
		sql = sql & " AND 製品NO = '" & SQLString((Me.製品NO)) & "'"
		sql = sql & " AND 仕様NO = '" & SQLString((Me.仕様NO)) & "'"
		
		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
		
		With rs
			Select Case .EOF
				Case True
					.AddNew()
					.Fields("得意先CD").Value = Me.得意先CD
					.Fields("仕入先CD").Value = Me.仕入先CD
					.Fields("PC区分").Value = Me.PC区分
					.Fields("製品NO").Value = Me.製品NO
					.Fields("仕様NO").Value = Me.仕様NO
					
					.Fields("初期登録日").Value = Today
				Case False
			End Select
			
			.Fields("大口売上単価").Value = Me.大口売上単価
			.Fields("大口仕入単価").Value = Me.大口仕入単価
			.Fields("小口売上単価").Value = Me.小口売上単価
			.Fields("小口仕入単価").Value = Me.小口仕入単価
			
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
		sql = sql & " FROM TM売上仕入単価"
		sql = sql & " WHERE 得意先CD = '" & SQLString((Me.得意先CD)) & "'"
		sql = sql & " AND 仕入先CD = '" & SQLString((Me.仕入先CD)) & "'"
		sql = sql & " AND PC区分 = '" & SQLString((Me.PC区分)) & "'"
		sql = sql & " AND 製品NO = '" & SQLString((Me.製品NO)) & "'"
		sql = sql & " AND 仕様NO = '" & SQLString((Me.仕様NO)) & "'"
		
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
		
		'    ' コマンドを実行する接続先を指定する
		'    cmd.ActiveConnection = Cn
		'    cmd.CommandText = "usp_ChkDelFor仕入単価"
		'    cmd.CommandType = adCmdStoredProc
		'
		'    ' それぞれのパラメータの値を指定する
		'    With cmd.Parameters
		'        .Item(1).Value = Me.ウエルシア物件内容CD
		'    End With
		'
		'    cmd.Execute
		'
		'    If (cmd(0) = 0) Then
		'        PurgeChk = False
		'    Else
		'        PurgeChk = True
		''''        CriticalAlarm (cmd(3))
		'        'エラーの生成
		'        Err.Raise cmd("@RetST"), , cmd("@RetMsg")
		'    End If
		'
		'    Set cmd = Nothing
		
	End Function
	
	'//////////////////////////////////////
	'   PurgeBySir
	'//////////////////////////////////////
	Public Function PurgeBySir(ByVal s得意先CD As String, ByVal s仕入先CD As String) As Boolean
		Dim sql As String
		
		PurgeBySir = False
		
		On Error GoTo Trans_err
		
		'マウスポインターを砂時計にする
		'Call HourGlass(True)
		
		'SQL生成
		sql = "DELETE "
		sql = sql & " FROM TM売上仕入単価"
		sql = sql & " WHERE 得意先CD = '" & SQLString(s得意先CD) & "'"
		sql = sql & " AND 仕入先CD = '" & SQLString(s仕入先CD) & "'"
		
		Cn.Execute(sql)
		
		PurgeBySir = True
		
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