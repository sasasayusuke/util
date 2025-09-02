Option Strict Off
Option Explicit On
Friend Class clsDspKyakuZaiko
	'///////////////////////////
	'客先在庫マスタクラス
	'///////////////////////////
	'
	'--------------------------------------------------------------------
	'UPDATE
	'       2009/09/17  oosawa  新規作成
	'       2009/10/05  oosawa  スカラー値を返すように変更
	'       2009/11/26  oosawa  納期が空の場合はゼロで返す
	'--------------------------------------------------------------------
	
	'担当者CD
	Private m_担当者CD As Short
	'得意先CD
	Private m_得意先CD As String
	'製品NO
	Private m_製品NO As String
	'仕様NO
	Private m_仕様NO As String
	'漢字名称
	Private m_漢字名称 As String
	'W
	Private m_W As Short
	'D
	Private m_D As Short
	'H
	Private m_H As Short
	'D1
	Private m_D1 As Short
	'D2
	Private m_D2 As Short
	'H1
	Private m_H1 As Short
	'H2
	Private m_H2 As Short
	'仕入先CD
	Private m_仕入先CD As String
	
	'合計在庫数
	Private m_合計在庫数 As Decimal
	
	'//////////////////////////////////////
	'   担当者CD
	'//////////////////////////////////////
	
	Public Property 担当者CD() As Short
		Get
			[担当者CD] = m_担当者CD
		End Get
		Set(ByVal Value As Short)
			m_担当者CD = Value
		End Set
	End Property
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
	'   製品NO
	'//////////////////////////////////////
	
	Public Property 製品NO() As String
		Get
			[製品NO] = m_製品NO
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
			[仕様NO] = m_仕様NO
		End Get
		Set(ByVal Value As String)
			m_仕様NO = Value
		End Set
	End Property
	'//////////////////////////////////////
	'   漢字名称
	'//////////////////////////////////////
	Public ReadOnly Property 漢字名称() As String
		Get
			[漢字名称] = m_漢字名称
		End Get
	End Property
	'//////////////////////////////////////
	'   W
	'//////////////////////////////////////
	Public ReadOnly Property W() As String
		Get
			[W] = VB6.Format(m_W, "#")
		End Get
	End Property
	'//////////////////////////////////////
	'   D
	'//////////////////////////////////////
	Public ReadOnly Property D() As String
		Get
			[D] = VB6.Format(m_D, "#")
		End Get
	End Property
	'//////////////////////////////////////
	'   H
	'//////////////////////////////////////
	Public ReadOnly Property H() As String
		Get
			[H] = VB6.Format(m_H, "#")
		End Get
	End Property
	'//////////////////////////////////////
	'   D1
	'//////////////////////////////////////
	Public ReadOnly Property D1() As String
		Get
			[D1] = VB6.Format(m_D1, "#")
		End Get
	End Property
	'//////////////////////////////////////
	'   D2
	'//////////////////////////////////////
	Public ReadOnly Property D2() As String
		Get
			[D2] = VB6.Format(m_D2, "#")
		End Get
	End Property
	'//////////////////////////////////////
	'   H1
	'//////////////////////////////////////
	Public ReadOnly Property H1() As String
		Get
			[H1] = VB6.Format(m_H1, "#")
		End Get
	End Property
	'//////////////////////////////////////
	'   H2
	'//////////////////////////////////////
	Public ReadOnly Property H2() As String
		Get
			[H2] = VB6.Format(m_H2, "#")
		End Get
	End Property
	'//////////////////////////////////////
	'   仕入先CD
	'//////////////////////////////////////
	
	Public Property 仕入先CD() As String
		Get
			[仕入先CD] = m_仕入先CD
		End Get
		Set(ByVal Value As String)
			m_仕入先CD = Value
		End Set
	End Property
	'//////////////////////////////////////
	'   合計在庫数
	'//////////////////////////////////////
	Public ReadOnly Property 合計在庫数() As Decimal
		Get
			合計在庫数 = m_合計在庫数
		End Get
	End Property
	
	'//////////////////////////////////////
	'   変数の初期化
	'//////////////////////////////////////
	'UPGRADE_NOTE: Class_Initialize は Class_Initialize_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
	Private Sub Class_Initialize_Renamed()
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
		m_担当者CD = 0
		m_製品NO = vbNullString
		m_仕様NO = vbNullString
		m_漢字名称 = vbNullString
		m_W = 0
		m_D = 0
		m_H = 0
		m_D1 = 0
		m_D2 = 0
		m_H1 = 0
		m_H2 = 0
		m_仕入先CD = vbNullString
		
		m_合計在庫数 = 0
		
	End Sub
	
	'//////////////////////////////////////
	'   在庫情報取得
	'//////////////////////////////////////
	Public Function GetbyID() As Boolean
		Dim rs As ADODB.Recordset
		Dim sql As String
		
		'    On Error GoTo GetByID_Err
		
		'IDが設定されてない場合
		'UPGRADE_NOTE: IsMissing() は IsNothing() に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="8AE1CB93-37AB-439A-A4FF-BE3B6760BB23"' をクリックしてください。
		If IsNothing(m_製品NO) Then
			GetbyID = False
			Exit Function
		End If
		
		'IDになにも入ってないチェックをしたほうがいい？
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		If IsDbNull(m_製品NO) Or Trim(m_製品NO) = vbNullString Then
			GetbyID = False
			Exit Function
		End If
		
		'マウスポインターを砂時計にする
		Call HourGlass(True)
		
		sql = "SELECT ZK.担当者CD, ZK.得意先CD, ZK.製品NO, ZK.仕様NO, "
		sql = sql & " SE.漢字名称,SE.W,SE.D,SE.H,SE.D1,SE.D2,SE.H1,SE.H2, "
		sql = sql & " SI.仕入先CD "
		sql = sql & " FROM TM客先在庫_担当者別 AS ZK"
		sql = sql & " INNER JOIN TM製品 AS SE "
		sql = sql & " ON ZK.製品NO = SE.製品NO "
		sql = sql & " AND ZK.仕様NO = SE.仕様NO "
		sql = sql & " LEFT JOIN TM仕入先 AS SI ON SE.主仕入先CD = SI.仕入先CD"
		sql = sql & " WHERE ZK.担当者CD = " & SQLString(Trim(CStr(m_担当者CD)))
		sql = sql & " AND ZK.得意先CD = '" & SQLString(Trim(m_得意先CD)) & "'"
		sql = sql & " AND ZK.製品NO = '" & SQLString(Trim(m_製品NO)) & "'"
		sql = sql & " AND ZK.仕様NO = '" & SQLString(Trim(m_仕様NO)) & "'"
		
		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
		
		With rs
			If .EOF Then
				GetbyID = False
				'            m_EOF = True
			Else
				GetbyID = True
				'            m_EOF = False
				m_担当者CD = .Fields("担当者CD").Value
				m_得意先CD = Trim(.Fields("得意先CD").Value)
				m_製品NO = Trim(.Fields("製品NO").Value)
				m_仕様NO = Trim(.Fields("仕様NO").Value)
				
				m_漢字名称 = Trim(.Fields("漢字名称").Value)
				m_W = .Fields("W").Value
				m_D = .Fields("D").Value
				m_H = .Fields("H").Value
				m_D1 = .Fields("D1").Value
				m_D2 = .Fields("D2").Value
				m_H1 = .Fields("H1").Value
				m_H2 = .Fields("H2").Value
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				m_仕入先CD = NullToZero(.Fields("仕入先CD"), "")
			End If
		End With
		
		Call ReleaseRs(rs)
		
		Call HourGlass(False)
		Exit Function
GetbyID_Err: 
		Call HourGlass(False)
		MsgBox(Err.Number & " " & Err.Description)
	End Function
	
	'//////////////////////////////////////
	'   選択画面
	'//////////////////////////////////////
	Public Function ShowDialog() As Boolean
		Dim fSentak As KyakuZaikoSen_cls
		
		fSentak = New KyakuZaikoSen_cls
		
		With fSentak
			.[担当者CD] = CShort(SQLString(Trim(CStr(m_担当者CD))))
			.[得意先CD] = SQLString(Trim(m_得意先CD))
			
			.ShowDialog()
			If .DialogResult_Renamed Then
				''''
				'ここでIDに入れるとItem_checkでチェックが効かない。
				'旧情報と新情報のチェックの仕方を変えないと...！！！
				'Undoの考え方も？？？
				'''            Me.SelectedID = .DialogResultCode
				''            Me.ID = .DialogResultCode
				''            m_ID = .DialogResultCode
				Me.製品NO = .DialogResultCode1
				Me.仕様NO = .DialogResultCode2
				ShowDialog = True
			End If
		End With
		
		'UPGRADE_NOTE: オブジェクト fSentak をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		fSentak = Nothing
		
	End Function
	''''
	''''Public Function GetZaikoInfo(m指定日付 As Date, m担当者CD As Integer, m得意先CD As String, _
	'''''                                m製品NO As String, m仕様NO As String, m見積番号 As Long) As Boolean
	''''    Dim cmd As New adodb.Command
	''''    Dim prm As adodb.Parameter
	''''    Dim rs As adodb.Recordset
	''''
	''''    'マウスポインターを砂時計にする
	''''    HourGlass True
	''''
	''''    '---コマンドパラメータ設定
	''''    ' コマンドを実行する接続先を指定する
	''''    cmd.ActiveConnection = Cn
	''''    cmd.CommandTimeout = 0
	''''    cmd.CommandText = "usp_担当者別客先在庫抽出"
	''''    cmd.CommandType = adCmdStoredProc
	''''
	''''    ' それぞれのパラメータの値を指定する
	''''    With cmd.Parameters
	''''
	''''        .Item("@i指定日付").Value = m指定日付
	''''
	''''        .Item("@i担当者CD").Value = m担当者CD
	''''        .Item("@i得意先CD").Value = m得意先CD
	''''
	''''        .Item("@i製品NO").Value = IIf(m製品NO = "", Null, m製品NO)
	''''        .Item("@i仕様NO").Value = IIf(m仕様NO = "", Null, m仕様NO)
	''''
	''''        .Item("@i見積番号").Value = m見積番号
	''''
	''''    End With
	''''
	''''    Cn.CursorLocation = adUseClient             'MoveLastを使用する場合
	''''
	''''    Set rs = New adodb.Recordset
	''''    '---コマンド実行
	''''    Set rs = cmd.Execute
	''''
	''''    Cn.CursorLocation = adUseServer
	''''
	''''    m_合計在庫数 = 0
	''''
	''''    If rs.State <> 0 Then
	''''        If rs.EOF Then
	''''            GetZaikoInfo = False
	''''        Else
	''''            GetZaikoInfo = True
	''''            m_合計在庫数 = rs![合計在庫数]
	''''        End If
	''''    Else
	''''        GetZaikoInfo = False
	''''        CriticalAlarm (cmd("@RetST") & " : " & cmd("@RetMsg"))
	''''    End If
	''''
	''''    Set cmd = Nothing
	''''
	''''    On Error GoTo 0
	''''    ReleaseRs rs
	''''    Call HourGlass(False)
	''''    Exit Function
	''''
	''''Download_Err:
	''''    CriticalAlarm Err.Number & " " & Err.Description
	''''    Set cmd = Nothing
	''''    ReleaseRs rs
	''''    Call HourGlass(False)
	''''End Function
	
	Public Function GetZaikoInfo(ByRef m指定日付 As Object, ByRef m担当者CD As Short, ByRef m得意先CD As String, ByRef m製品NO As String, ByRef m仕様NO As String, ByRef m見積番号 As Integer) As Boolean
		''Public Function GetZaikoInfo(m指定日付 As Date, m担当者CD As Integer, m得意先CD As String, _
		'''                                m製品NO As String, m仕様NO As String, m見積番号 As Long) As Boolean
		Dim cmd As New ADODB.Command
		Dim prm As ADODB.Parameter
		Dim rs As ADODB.Recordset
		
		m_合計在庫数 = 0
		
		'2009/11/26 ADD↓
		'UPGRADE_WARNING: オブジェクト NullToZero(m指定日付, ) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If NullToZero(m指定日付, "") = "" Then
			GetZaikoInfo = False
			Exit Function
		End If
		'2009/11/26 ADD↑
		
		'マウスポインターを砂時計にする
		HourGlass(True)
		
		'---コマンドパラメータ設定
		' コマンドを実行する接続先を指定する
		cmd.let_ActiveConnection(Cn)
		cmd.CommandTimeout = 0
		cmd.CommandText = "usp_担当者別客先在庫抽出"
		cmd.CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
		
		' それぞれのパラメータの値を指定する
		With cmd.Parameters
			
			'UPGRADE_WARNING: オブジェクト m指定日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@i指定日付").Value = m指定日付
			
			.Item("@i担当者CD").Value = m担当者CD
			.Item("@i得意先CD").Value = m得意先CD
			
			'''        .Item("@i製品NO").Value = IIf(m製品NO = "", Null, m製品NO)
			'''        .Item("@i仕様NO").Value = IIf(m仕様NO = "", Null, m仕様NO)
			.Item("@i製品NO").Value = Trim(m製品NO) '2013/03/08 ADD
			.Item("@i仕様NO").Value = Trim(m仕様NO) '2013/03/08 ADD
			
			.Item("@i見積番号").Value = m見積番号
			
		End With
		
		m_合計在庫数 = 0
		
		cmd.Execute()
		
		If cmd.State <> 0 Then
			'エラー
			GetZaikoInfo = False
			CriticalAlarm((cmd.Parameters("@RetST").Value & " : " & cmd.Parameters("@RetMsg").Value))
		Else
			If cmd.Parameters("@Ret未管理FLG").Value <> 0 Then
				'在庫未管理
				GetZaikoInfo = False
			Else
				m_合計在庫数 = cmd.Parameters("@Ret合計在庫数").Value
				GetZaikoInfo = True
			End If
		End If
		
		'UPGRADE_NOTE: オブジェクト cmd をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		cmd = Nothing
		
		On Error GoTo 0
		ReleaseRs(rs)
		Call HourGlass(False)
		Exit Function
		
Download_Err: 
		CriticalAlarm(Err.Number & " " & Err.Description)
		'UPGRADE_NOTE: オブジェクト cmd をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		cmd = Nothing
		ReleaseRs(rs)
		Call HourGlass(False)
	End Function
End Class