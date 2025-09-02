Option Strict Off
Option Explicit On
Friend Class clsSirTanka
	'///////////////////////////
	'仕入先単価マスタクラス
	'///////////////////////////
	'2015/06/11 oosawa      新設
	
	'変数
	Private m_仕入先cls As clsSiiresaki '仕入先クラス
	Private m_製品cls As clsSeihin '製品クラス
	
	'Private m_仕入先CD          As String
	''Private m_PC区分            As String
	'Private m_製品NO            As String
	'Private m_仕様NO            As String
	Private m_仕入単価 As Decimal
	Private m_旧仕入単価 As Decimal
	Private m_初期登録日 As Object
	
	Private m_MaxLength As Integer '桁数
	Private m_EOF As Boolean '存在確認
	
	Private m_Error As Short 'エラー
	
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
	
	''''//////////////////////////////////////
	''''   PC区分
	''''//////////////////////////////////////
	'''Public Property Let PC区分(ByVal vData As String)
	'''        m_PC区分 = vData
	'''End Property
	'''
	'''Public Property Get PC区分() As String
	'''    PC区分 = Trim$(m_PC区分)
	'''End Property
	
	'//////////////////////////////////////
	'   製品NO
	'//////////////////////////////////////
	
	Public Property 製品NO() As String
		Get
			[製品NO] = Trim(Me.製品cls.製品NO)
		End Get
		Set(ByVal Value As String)
			Me.製品cls.製品NO = Value
		End Set
	End Property
	'//////////////////////////////////////
	'   仕様NO
	'//////////////////////////////////////
	
	Public Property 仕様NO() As String
		Get
			[仕様NO] = Trim(Me.製品cls.仕様NO)
		End Get
		Set(ByVal Value As String)
			Me.製品cls.仕様NO = Value
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
	
	'//////////////////////////////////////
	'   製品クラス
	'//////////////////////////////////////
	
	Public Property 製品cls() As clsSeihin
		Get
			製品cls = m_製品cls
		End Get
		Set(ByVal Value As clsSeihin)
			m_製品cls = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   仕入単価
	'//////////////////////////////////////
	
	Public Property 仕入単価() As Decimal
		Get
			[仕入単価] = m_仕入単価
		End Get
		Set(ByVal Value As Decimal)
			m_仕入単価 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   旧仕入単価
	'//////////////////////////////////////
	
	Public Property 旧仕入単価() As Decimal
		Get
			[旧仕入単価] = m_旧仕入単価
		End Get
		Set(ByVal Value As Decimal)
			m_旧仕入単価 = Value
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
		'仕入先クラス生成
		m_仕入先cls = New clsSiiresaki
		'製品クラス生成
		m_製品cls = New clsSeihin
		
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
		'UPGRADE_NOTE: オブジェクト m_仕入先cls をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		m_仕入先cls = Nothing '仕入先クラス
		'UPGRADE_NOTE: オブジェクト m_製品cls をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		m_製品cls = Nothing '製品クラス
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
		m_仕入先cls.Initialize()
		
		m_製品cls.Initialize()
		m_製品cls.isDo在庫管理 = clsSeihin.Type在庫管理.全て
		m_製品cls.isDo廃盤表示 = clsSeihin.Type廃盤表示.廃盤表示しない
		
		m_仕入単価 = 0
		
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
		sql = sql & " FROM TM仕入単価"
		sql = sql & " WHERE 仕入先CD = '" & SQLString((Me.仕入先CD)) & "'"
		sql = sql & " AND 製品NO = '" & SQLString((Me.製品NO)) & "'"
		sql = sql & " AND 仕様NO = '" & SQLString((Me.仕様NO)) & "'"
		
		'SQL実行
		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
		
		With rs
			If .EOF Then
				GetbyID = False
				m_EOF = True
				
				m_仕入単価 = 0
			Else
				GetbyID = True
				m_EOF = False
				
				Me.仕入単価 = .Fields("仕入単価").Value
				Me.旧仕入単価 = .Fields("旧仕入単価").Value
				'UPGRADE_WARNING: オブジェクト m_初期登録日 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				m_初期登録日 = .Fields("初期登録日").Value
				
				'仕入先M
				Me.仕入先cls.Initialize()
				Me.仕入先cls.仕入先CD = .Fields("仕入先CD").Value
				Me.仕入先cls.GetbyID()
				
				'製品M
				Me.製品cls.Initialize()
				Me.製品cls.製品NO = .Fields("製品NO").Value
				Me.製品cls.仕様NO = .Fields("仕様NO").Value
				Me.製品cls.GetbyID()
				
				
				
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
		sql = sql & " FROM TM仕入単価"
		sql = sql & " ORDER BY 仕入先CD,製品NO,仕様NO"
		
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
	Public Function GetAllRsBySirCD(ByRef 仕入先CD As String) As ADODB.Recordset
		Dim rs As ADODB.Recordset
		Dim sql As String
		
		On Error GoTo GetRs_Err
		
		'マウスポインターを砂時計にする
		'Call HourGlass(True)
		
		'SQL生成
		sql = "SELECT ST.*,SE.漢字名称,"
		sql = sql & "製品サイズ=(CASE WHEN SE.W = 0 THEN '' ELSE 'W=' + LTRIM(STR(SE.W) + ' ') END"
		sql = sql & " + CASE WHEN SE.D = 0 THEN '' ELSE 'D=' + LTRIM(STR(SE.D) + ' ') END"
		sql = sql & " + CASE WHEN SE.H = 0 THEN '' ELSE 'H=' + LTRIM(STR(SE.H) + ' ') END"
		sql = sql & " + CASE WHEN SE.D1 = 0 THEN '' ELSE 'D1=' + LTRIM(STR(SE.D1) + ' ') END"
		sql = sql & " + CASE WHEN SE.D2 = 0 THEN '' ELSE 'D2=' + LTRIM(STR(SE.D2) + ' ') END"
		sql = sql & " + CASE WHEN SE.H1 = 0 THEN '' ELSE 'H1=' + LTRIM(STR(SE.H1) + ' ') END"
		sql = sql & " + CASE WHEN SE.H2 = 0 THEN '' ELSE 'H2=' + LTRIM(STR(SE.H2) + ' ') END) "
		sql = sql & " FROM TM仕入単価 AS ST"
		sql = sql & " LEFT JOIN TM製品 AS SE"
		sql = sql & " ON ST.製品NO = SE.製品NO AND ST.仕様NO = SE.仕様NO"
		
		sql = sql & " WHERE ST.仕入先CD = '" & SQLString([仕入先CD]) & "'"
		sql = sql & "ORDER BY ST.仕入先CD,ST.製品NO,ST.仕様NO"
		
		'DBクローズするのに必要
		Cn.CursorLocation = ADODB.CursorLocationEnum.adUseClient
		
		'SQL実行
		GetAllRsBySirCD = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
		'UPGRADE_NOTE: オブジェクト GetAllRsBySirCD.ActiveConnection をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		GetAllRsBySirCD.ActiveConnection = Nothing
		
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
	Public Function GetRsByID(ByVal 開始CD As Object, ByVal 終了CD As Object) As ADODB.Recordset
		Dim rs As ADODB.Recordset
		Dim sql As String
		Dim whr As String
		
		On Error GoTo GetRs_Err
		
		'マウスポインターを砂時計にする
		'Call HourGlass(True)
		
		'-------抽出条件セット
		whr = SQLStringRange("SR.仕入先CD", 開始CD, 終了CD)
		If whr <> vbNullString Then
			whr = " WHERE " & whr & " "
		End If
		
		'SQL生成
		sql = "SELECT TN.仕入先CD, SR.仕入先名1, TN.製品NO, TN.仕様NO,"
		sql = sql & " SE.漢字名称,"
		sql = sql & " SE.W,SE.D,SE.H,SE.D1,SE.D2,SE.H1,SE.H2, "
		sql = sql & " ISNULL(TN.仕入単価,0) AS 仕入単価,"
		sql = sql & " ISNULL(TN.旧仕入単価,0) AS 旧仕入単価"
		sql = sql & " FROM TM仕入単価 AS TN"
		sql = sql & " LEFT JOIN TM仕入先 AS SR ON TN.仕入先CD = SR.仕入先CD"
		sql = sql & " LEFT JOIN TM製品 AS SE ON TN.製品NO = SE.製品NO AND TN.仕様NO = SE.仕様NO"
		sql = sql & whr
		sql = sql & " ORDER BY TN.仕入先CD, TN.製品NO, TN.仕様NO"
		
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
		sql = sql & " FROM TM仕入単価"
		sql = sql & " WHERE 仕入先CD = '" & SQLString((Me.仕入先CD)) & "'"
		sql = sql & " AND 製品NO = '" & SQLString((Me.製品NO)) & "'"
		sql = sql & " AND 仕様NO = '" & SQLString((Me.仕様NO)) & "'"
		
		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
		
		With rs
			Select Case .EOF
				Case True
					.AddNew()
					.Fields("仕入先CD").Value = Me.仕入先CD
					.Fields("製品NO").Value = Me.製品NO
					.Fields("仕様NO").Value = Me.仕様NO
					
					.Fields("初期登録日").Value = Today
				Case False
			End Select
			
			.Fields("仕入単価").Value = Me.仕入単価
			.Fields("旧仕入単価").Value = Me.旧仕入単価
			
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
		sql = sql & " FROM TM仕入単価"
		sql = sql & " WHERE 仕入先CD = '" & SQLString((Me.仕入先CD)) & "'"
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
	Public Function PurgeBySir(ByRef 仕入先CD As String) As Boolean
		Dim sql As String
		
		PurgeBySir = False
		
		On Error GoTo Trans_err
		
		'マウスポインターを砂時計にする
		'Call HourGlass(True)
		
		'SQL生成
		sql = "DELETE "
		sql = sql & " FROM TM仕入単価"
		sql = sql & " WHERE 仕入先CD = '" & SQLString([仕入先CD]) & "'"
		
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