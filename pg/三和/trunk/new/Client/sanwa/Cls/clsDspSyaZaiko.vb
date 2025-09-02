Option Strict Off
Option Explicit On

''' <summary>
''' 社内在庫マスタクラス
''' 2009/09/17  oosawa  新規作成
''' 2009/10/05  oosawa  スカラー値を返すように変更
''' 2009/11/26  oosawa  納期が空の場合はゼロで返す
''' </summary>
Friend Class clsDspSyaZaiko

	'担当者CD
	Private m_担当者CD As Short
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
	'適正在庫数
	Private m_適正在庫数 As Decimal
	'予備在庫数
	Private m_予備在庫数 As Decimal


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
			[W] = m_W.ToString("#")
		End Get
	End Property

	'//////////////////////////////////////
	'   D
	'//////////////////////////////////////
	Public ReadOnly Property D() As String
		Get
			[D] = m_D.ToString("#")
		End Get
	End Property

	'//////////////////////////////////////
	'   H
	'//////////////////////////////////////
	Public ReadOnly Property H() As String
		Get
			[H] = m_H.ToString("#")
		End Get
	End Property

	'//////////////////////////////////////
	'   D1
	'//////////////////////////////////////
	Public ReadOnly Property D1() As String
		Get
			[D1] = m_D1.ToString("#")
		End Get
	End Property

	'//////////////////////////////////////
	'   D2
	'//////////////////////////////////////
	Public ReadOnly Property D2() As String
		Get
			[D2] = m_D2.ToString("#")
		End Get
	End Property

	'//////////////////////////////////////
	'   H1
	'//////////////////////////////////////
	Public ReadOnly Property H1() As String
		Get
			[H1] = m_H1.ToString("#")
		End Get
	End Property

	'//////////////////////////////////////
	'   H2
	'//////////////////////////////////////
	Public ReadOnly Property H2() As String
		Get
			[H2] = m_H2.ToString("#")
		End Get
	End Property

	'//////////////////////////////////////
	'   適正在庫数
	'//////////////////////////////////////
	Public ReadOnly Property 適正在庫数() As Decimal
		Get
			[適正在庫数] = m_適正在庫数
		End Get
	End Property

	'//////////////////////////////////////
	'   予備在庫数
	'//////////////////////////////////////
	Public ReadOnly Property 予備在庫数() As Decimal
		Get
			[予備在庫数] = m_予備在庫数
		End Get
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

		m_適正在庫数 = 0
		m_予備在庫数 = 0

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
		If IsDBNull(m_製品NO) Or Trim(m_製品NO) = vbNullString Then
			GetbyID = False
			Exit Function
		End If

		'マウスポインターを砂時計にする
		Call HourGlass(True)

		sql = "SELECT ZK.担当者CD, ZK.製品NO, ZK.仕様NO, "
		sql = sql & " SE.漢字名称,SE.W,SE.D,SE.H,SE.D1,SE.D2,SE.H1,SE.H2, "
		sql = sql & " ZK.適正在庫数,ZK.予備在庫数 "
		sql = sql & " FROM TM社内在庫_担当者別 AS ZK"
		sql = sql & " INNER JOIN TM製品 AS SE "
		sql = sql & " ON ZK.製品NO = SE.製品NO "
		sql = sql & " AND ZK.仕様NO = SE.仕様NO "
		sql = sql & " LEFT JOIN TM仕入先 AS SI ON SE.主仕入先CD = SI.仕入先CD"
		sql = sql & " WHERE ZK.担当者CD = " & SQLString(Trim(CStr(m_担当者CD)))
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

				m_適正在庫数 = .Fields("適正在庫数").Value
				m_予備在庫数 = .Fields("予備在庫数").Value
			End If
		End With

		Call ReleaseRs(rs)

		Call HourGlass(False)
		Exit Function
GetbyID_Err:
		Call HourGlass(False)
		MsgBox(Err.Number & " " & Err.Description)
	End Function
	'  '
	'  ''//////////////////////////////////////
	'  ''   選択画面
	'  ''//////////////////////////////////////
	'  'Public Function ShowDialog() As Boolean
	'  '    Dim fSentak As SyaZaikoSen_cls
	'  '
	'  '    Set fSentak = New SyaZaikoSen_cls
	'  '
	'  '    With fSentak
	'  '        .Show vbModal
	'  '        If .DialogResult Then
	'  '''''
	'  '    'ここでIDに入れるとItem_checkでチェックが効かない。
	'  '    '旧情報と新情報のチェックの仕方を変えないと...！！！
	'  '    'Undoの考え方も？？？
	'  ''''            Me.SelectedID = .DialogResultCode
	'  '''            Me.ID = .DialogResultCode
	'  '''            m_ID = .DialogResultCode
	'  '            Me.製品NO = .DialogResultCode1
	'  '            Me.仕様NO = .DialogResultCode1
	'  '            ShowDialog = True
	'  '        End If
	'  '    End With
	'  '
	'  '    Set fSentak = Nothing
	'  '
	'  'End Function

	'  Public Function GetZaikoInfo(m指定日付 As Date, m担当者CD As Integer, _
	'  '                                m製品NO As String, m仕様NO As String, m見積番号 As Long) As Boolean
	'      Dim cmd As New adodb.Command
	'      Dim prm As adodb.Parameter
	'      Dim rs As adodb.Recordset
	'  
	'      'マウスポインターを砂時計にする
	'      HourGlass True
	'  
	'      '---コマンドパラメータ設定
	'      ' コマンドを実行する接続先を指定する
	'      cmd.ActiveConnection = Cn
	'      cmd.CommandTimeout = 0
	'      cmd.CommandText = "usp_担当者別社内在庫抽出"
	'      cmd.CommandType = adCmdStoredProc
	'  
	'      ' それぞれのパラメータの値を指定する
	'      With cmd.Parameters
	'  
	'          .Item("@i指定日付").Value = m指定日付
	'  
	'          .Item("@i担当者CD").Value = m担当者CD
	'  
	'          .Item("@i製品NO").Value = IIf(m製品NO = "", Null, m製品NO)
	'          .Item("@i仕様NO").Value = IIf(m仕様NO = "", Null, m仕様NO)
	'  
	'          .Item("@i見積番号").Value = m見積番号
	'      End With
	'  
	'      Cn.CursorLocation = adUseClient             'MoveLastを使用する場合
	'  
	'      Set rs = New adodb.Recordset
	'      '---コマンド実行
	'      Set rs = cmd.Execute
	'  
	'      Cn.CursorLocation = adUseServer
	'  
	'      m_合計在庫数 = 0
	'  
	'      If rs.State <> 0 Then
	'          If rs.EOF Then
	'              GetZaikoInfo = False
	'          Else
	'              GetZaikoInfo = True
	'              m_適正在庫数 = rs![適正在庫数]
	'              m_合計在庫数 = rs![合計在庫数]
	'          End If
	'      Else
	'          GetZaikoInfo = False
	'          CriticalAlarm (cmd("@RetST") & " : " & cmd("@RetMsg"))
	'      End If
	'  
	'      Set cmd = Nothing
	'  
	'      On Error GoTo 0
	'      ReleaseRs rs
	'      Call HourGlass(False)
	'      Exit Function
	'  
	'  Download_Err:
	'      CriticalAlarm Err.Number & " " & Err.Description
	'      Set cmd = Nothing
	'      ReleaseRs rs
	'      Call HourGlass(False)
	'  End Function

	'スカラ値を戻すパターン
	Public Function GetZaikoInfo(ByRef m指定日付 As Object, ByRef m担当者CD As Short, ByRef m製品NO As String, ByRef m仕様NO As String, ByRef m見積番号 As Integer) As Boolean
		' 'Public Function GetZaikoInfo(m指定日付 As Date, m担当者CD As Integer, _
		' ''                                m製品NO As String, m仕様NO As String, m見積番号 As Long) As Boolean
		Dim cmd As New ADODB.Command
		Dim prm As ADODB.Parameter
		Dim rs As ADODB.Recordset

		m_合計在庫数 = 0
		m_適正在庫数 = 0

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
		cmd.CommandText = "usp_担当者別社内在庫抽出"
		cmd.CommandType = ADODB.CommandTypeEnum.adCmdStoredProc

		' それぞれのパラメータの値を指定する
		With cmd.Parameters

			'UPGRADE_WARNING: オブジェクト m指定日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@i指定日付").Value = m指定日付

			.Item("@i担当者CD").Value = m担当者CD

			'  '        .Item("@i製品NO").Value = IIf(m製品NO = "", Null, m製品NO)
			'  '        .Item("@i仕様NO").Value = IIf(m仕様NO = "", Null, m仕様NO)
			.Item("@i製品NO").Value = Trim(m製品NO) '2013/03/08 ADD
			.Item("@i仕様NO").Value = Trim(m仕様NO) '2013/03/08 ADD

			.Item("@i見積番号").Value = m見積番号
		End With

		m_合計在庫数 = 0
		m_適正在庫数 = 0

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
				m_適正在庫数 = cmd.Parameters("@Ret適正在庫数").Value
				GetZaikoInfo = True
			End If
		End If

		'UPGRADE_NOTE: オブジェクト cmd をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		cmd = Nothing

		On Error GoTo 0
		'ReleaseRs(rs)
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