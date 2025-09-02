Option Strict Off
Option Explicit On
Friend Class clsSeihin
	'   2015/01/10  oosawa      廃盤FLG追加
	'   2015/01/29  oosawa      ロケーション追加
	'   2017/03/27  oosawa      入出庫用コード追加
	'   2023/07/28  oosawa      取組区分名追加
	'///////////////////////////
	'TM製品クラス
	'///////////////////////////
	
	'変数
	'製品NO
	Private m_製品NO As String
	'仕様NO
	Private m_仕様NO As String
	'漢字名称
	Private m_漢字名称 As String
	'カナ名称
	Private m_カナ名称 As String
	'ベース色
	Private m_ベース色 As String
	'W
	Private m_W As Integer
	'D
	Private m_D As Integer
	'H
	Private m_H As Integer
	'D1
	Private m_D1 As Integer
	'D2
	Private m_D2 As Integer
	'H1
	Private m_H1 As Integer
	'H2
	Private m_H2 As Integer
	
	'単位名
	Private m_単位名 As String
	'グループ
	Private m_グループ As String
	'主仕入先CD
	Private m_主仕入先CD As String
	'定価
	Private m_定価 As Decimal
	'売上単価
	Private m_売上単価 As Decimal
	'仕入単価
	Private m_仕入単価 As Decimal
	'売上税区分
	Private m_売上税区分 As Short
	'仕入税区分
	Private m_仕入税区分 As Short
	'在庫区分
	Private m_在庫区分 As Short
	
	'在庫管理する・しない
	Private m_在庫管理 As Type在庫管理
	'在庫管理の種類
	Public Enum Type在庫管理
		在庫管理する
		在庫管理しない
		全て
	End Enum
	
	'JANCODE
	Private m_JANCODE As String '2014/06/16 ADD
	
	'廃盤FLG
	Private m_廃盤FLG As Short '2015/01/10 ADD
	Private m_廃盤表示 As Type廃盤表示 '2015/01/10 ADD
	'在庫管理の種類
	Public Enum Type廃盤表示
		廃盤表示する
		廃盤表示しない
	End Enum
	
	'ロケーション
	Private m_ロケーション As String '2015/01/29 ADD
	
	Private m_費用区分 As Short '2015/12/23 ADD
	
	'入出庫用コード
	Private m_入出庫用CD As String '2018/03/27 ADD
	
	'サイズ変更区分
	Private m_サイズ変更区分 As Short '2020/10/10 ADD
	
	Private m_取組区分 As Short '2023/07/28 ADD
	Private m_取組終了年月 As Object '2023/7/28 ADD
	
	Private m_MaxLength As Integer '桁数
	Private m_EOF As Boolean '存在確認
	Private m_Error As Short 'エラー
	
	Private m_Datas() As Object '選択配列
	'//////////////////////////////////////
	'   製品連番配列
	'//////////////////////////////////////
	
	Public Property Array製品() As Object
		Get
			'UPGRADE_WARNING: オブジェクト Array製品 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			Array製品 = VB6.CopyArray(m_Datas)
		End Get
		Set(ByVal Value As Object)
			'UPGRADE_WARNING: オブジェクト vData の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			m_Datas = Value
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
	'   漢字名称
	'//////////////////////////////////////
	Public Property 漢字名称() As String
		Get
			[漢字名称] = m_漢字名称
		End Get
		Set(ByVal Value As String)
			m_漢字名称 = Value
		End Set
	End Property
	'//////////////////////////////////////
	'   カナ名称
	'//////////////////////////////////////
	Public Property カナ名称() As String
		Get
			[カナ名称] = m_カナ名称
		End Get
		Set(ByVal Value As String)
			m_カナ名称 = Value
		End Set
	End Property
	'//////////////////////////////////////
	'   ベース色
	'//////////////////////////////////////
	Public Property ベース色() As String
		Get
			[ベース色] = m_ベース色
		End Get
		Set(ByVal Value As String)
			m_ベース色 = Value
		End Set
	End Property
	'//////////////////////////////////////
	'   W
	'//////////////////////////////////////
	Public Property W() As String
		Get
			[W] = VB6.Format(m_W, "#")
		End Get
		Set(ByVal Value As String)
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			m_W = NullToZero(Value)
		End Set
	End Property
	'//////////////////////////////////////
	'   D
	'//////////////////////////////////////
	Public Property D() As String
		Get
			[D] = VB6.Format(m_D, "#")
		End Get
		Set(ByVal Value As String)
			m_D = CInt(Value)
		End Set
	End Property
	'//////////////////////////////////////
	'   H
	'//////////////////////////////////////
	Public Property H() As String
		Get
			[H] = VB6.Format(m_H, "#")
		End Get
		Set(ByVal Value As String)
			m_H = CInt(Value)
		End Set
	End Property
	'//////////////////////////////////////
	'   D1
	'//////////////////////////////////////
	Public Property D1() As String
		Get
			[D1] = VB6.Format(m_D1, "#")
		End Get
		Set(ByVal Value As String)
			m_D1 = CInt(Value)
		End Set
	End Property
	'//////////////////////////////////////
	'   D2
	'//////////////////////////////////////
	Public Property D2() As String
		Get
			[D2] = VB6.Format(m_D2, "#")
		End Get
		Set(ByVal Value As String)
			m_D2 = CInt(Value)
		End Set
	End Property
	'//////////////////////////////////////
	'   H1
	'//////////////////////////////////////
	Public Property H1() As String
		Get
			[H1] = VB6.Format(m_H1, "#")
		End Get
		Set(ByVal Value As String)
			m_H1 = CInt(Value)
		End Set
	End Property
	'//////////////////////////////////////
	'   H2
	'//////////////////////////////////////
	Public Property H2() As String
		Get
			[H2] = VB6.Format(m_H2, "#")
		End Get
		Set(ByVal Value As String)
			m_H2 = CInt(Value)
		End Set
	End Property
	'//////////////////////////////////////
	'   単位名
	'//////////////////////////////////////
	Public Property 単位名() As String
		Get
			[単位名] = m_単位名
		End Get
		Set(ByVal Value As String)
			m_単位名 = Value
		End Set
	End Property
	'//////////////////////////////////////
	'   グループ
	'//////////////////////////////////////
	Public Property グループ() As String
		Get
			[グループ] = m_グループ
		End Get
		Set(ByVal Value As String)
			m_グループ = Value
		End Set
	End Property
	'//////////////////////////////////////
	'   主仕入先CD
	'//////////////////////////////////////
	Public Property 主仕入先CD() As String
		Get
			[主仕入先CD] = m_主仕入先CD
		End Get
		Set(ByVal Value As String)
			m_主仕入先CD = Value
		End Set
	End Property
	'//////////////////////////////////////
	'   売上単価
	'//////////////////////////////////////
	Public Property 売上単価() As Decimal
		Get
			[売上単価] = m_売上単価
		End Get
		Set(ByVal Value As Decimal)
			m_売上単価 = Value
		End Set
	End Property
	'//////////////////////////////////////
	'   定価
	'//////////////////////////////////////
	Public Property 定価() As Decimal
		Get
			[定価] = m_定価
		End Get
		Set(ByVal Value As Decimal)
			m_定価 = Value
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
	'   売上税区分
	'//////////////////////////////////////
	
	Public Property 売上税区分() As Short
		Get
			[売上税区分] = m_売上税区分
		End Get
		Set(ByVal Value As Short)
			m_売上税区分 = Value
		End Set
	End Property
	'//////////////////////////////////////
	'   仕入税区分
	'//////////////////////////////////////
	Public Property 仕入税区分() As Short
		Get
			[仕入税区分] = m_仕入税区分
		End Get
		Set(ByVal Value As Short)
			m_仕入税区分 = Value
		End Set
	End Property
	'//////////////////////////////////////
	'   在庫区分
	'//////////////////////////////////////
	Public Property 在庫区分() As Short
		Get
			[在庫区分] = m_在庫区分
		End Get
		Set(ByVal Value As Short)
			m_在庫区分 = Value
		End Set
	End Property
	'//////////////////////////////////////
	'   在庫管理するか:0しないか:1
	'   検索条件
	'//////////////////////////////////////
	Public WriteOnly Property isDo在庫管理() As Type在庫管理
		Set(ByVal Value As Type在庫管理)
			m_在庫管理 = Value
		End Set
	End Property
	
	'2014/06/06 ADD↓
	'//////////////////////////////////////
	'   JANCODE
	'//////////////////////////////////////
	Public Property JANCODE() As String
		Get
			[JANCODE] = m_JANCODE
		End Get
		Set(ByVal Value As String)
			m_JANCODE = Value
		End Set
	End Property
	'2014/06/06 ADD↑
	
	'2015/01/10 ADD↓
	'//////////////////////////////////////
	'   廃盤FLG 0:廃盤でない 以外：廃盤
	'//////////////////////////////////////
	Public Property 廃盤FLG() As Short
		Get
			[廃盤FLG] = m_廃盤FLG
		End Get
		Set(ByVal Value As Short)
			m_廃盤FLG = Value
		End Set
	End Property
	'//////////////////////////////////////
	'   廃盤表示するか:0しないか:1
	'//////////////////////////////////////
	Public WriteOnly Property isDo廃盤表示() As Type廃盤表示
		Set(ByVal Value As Type廃盤表示)
			m_廃盤表示 = Value
		End Set
	End Property
	'2015/01/10 ADD↑
	
	'2015/01/29 ADD↓
	'//////////////////////////////////////
	'   ロケーション
	'//////////////////////////////////////
	Public Property ロケーション() As String
		Get
			[ロケーション] = m_ロケーション
		End Get
		Set(ByVal Value As String)
			m_ロケーション = Value
		End Set
	End Property
	'2015/01/29 ADD↑
	
	'2015/12/23 ADD↓
	'2015/01/10 ADD↓
	'//////////////////////////////////////
	'   費用区分 0:部材費 1：労務費
	'//////////////////////////////////////
	Public Property 費用区分() As Short
		Get
			[費用区分] = m_費用区分
		End Get
		Set(ByVal Value As Short)
			m_費用区分 = Value
		End Set
	End Property
	'2015/12/23 ADD↑
	
	'2018/03/27 ADD↓
	'//////////////////////////////////////
	'   入出庫用コード
	'//////////////////////////////////////
	Public Property 入出庫用CD() As String
		Get
			[入出庫用CD] = m_入出庫用CD
		End Get
		Set(ByVal Value As String)
			m_入出庫用CD = Value
		End Set
	End Property
	'2018/03/27 ADD↑
	
	'2020/10/10 ADD↓
	'//////////////////////////////////////
	'   サイズ変更区分
	'//////////////////////////////////////
	Public Property サイズ変更区分() As Short
		Get
			[サイズ変更区分] = m_サイズ変更区分
		End Get
		Set(ByVal Value As Short)
			m_サイズ変更区分 = Value
		End Set
	End Property
	'2020/10/10 ADD↑
	
	'2023/07/28 ADD↓
	'//////////////////////////////////////
	'   取組区分
	'//////////////////////////////////////
	Public Property 取組区分() As Short
		Get
			[取組区分] = m_取組区分
		End Get
		Set(ByVal Value As Short)
			m_取組区分 = Value
		End Set
	End Property
	'//////////////////////////////////////
	'   取組終了年月
	'//////////////////////////////////////
	Public Property 取組終了年月() As Object
		Get
			'UPGRADE_WARNING: オブジェクト m_取組終了年月 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト 取組終了年月 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[取組終了年月] = m_取組終了年月
		End Get
		Set(ByVal Value As Object)
			'UPGRADE_WARNING: オブジェクト vData の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト m_取組終了年月 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			m_取組終了年月 = Value
		End Set
	End Property
	
	'2023/07/28 ADD↑
	
	''''//////////////////////////////////////
	''''   選択配列
	''''//////////////////////////////////////
	'''Public Property Set Datas(ByVal vData As Variant)
	'''        m_Datas = vData
	'''End Property
	'''Public Property Get Datas() As Variant()
	'''    Datas = m_Datas()
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
		'    m_Read = True
		'    m_GetMode = True
		
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
		m_製品NO = vbNullString
		m_仕様NO = vbNullString
		m_漢字名称 = vbNullString
		m_カナ名称 = vbNullString
		m_ベース色 = vbNullString
		m_W = 0
		m_D = 0
		m_H = 0
		m_D1 = 0
		m_D2 = 0
		m_H1 = 0
		m_H2 = 0
		
		m_単位名 = vbNullString
		m_グループ = vbNullString
		m_主仕入先CD = vbNullString
		m_定価 = 0
		m_売上単価 = 0
		m_仕入単価 = 0
		m_売上税区分 = 0
		m_仕入税区分 = 0
		m_在庫区分 = 0
		
		m_JANCODE = vbNullString '2014/06/16 ADD
		
		m_廃盤FLG = 0 '2015/01/10 ADD
		m_廃盤表示 = 1
		
		m_ロケーション = vbNullString '2015/01/29 ADD
		
		m_費用区分 = 0 '2015/12/23 ADD
		
		m_入出庫用CD = vbNullString '2018/03/27 ADD
		
		m_サイズ変更区分 = 0 '2020/10/10 ADD
		
	End Sub
	
	'//////////////////////////////////////
	'   選択画面
	'//////////////////////////////////////
	Public Function ShowDialog() As Boolean
		Dim fSentak As SeiSen_cls
		
		fSentak = New SeiSen_cls
		
		With fSentak
			.Parent_Renamed = Me
			.ShowDialog()
			If .DialogResult_Renamed Then
				
				
				'''            Me.m_製品NO = .DialogResultCodes
				Me.製品NO = .DialogResultCode1
				Me.仕様NO = .DialogResultCode2
				ShowDialog = True
			End If
		End With
		
		'UPGRADE_NOTE: オブジェクト fSentak をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		fSentak = Nothing
		
	End Function
	
	
	'//////////////////////////////////////
	'   複数選択画面
	'//////////////////////////////////////
	Public Function ShowDialogArray() As Boolean
		Dim fSentak As SeiSenArray_cls
		
		fSentak = New SeiSenArray_cls
		
		With fSentak
			.Parent_Renamed = Me
			.ShowDialog()
			If .DialogResult_Renamed Then
				
				
				'''            Me.棚割用製品連番 = .DialogResultCodes
				'UPGRADE_WARNING: オブジェクト fSentak.DialogResultCodes の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Me.Array製品 = .DialogResultCodes
				ShowDialogArray = True
			End If
		End With
		
		'UPGRADE_NOTE: オブジェクト fSentak をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		fSentak = Nothing
		
	End Function
	
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
		sql = sql & " FROM TM製品"
		sql = sql & " WHERE 製品NO = '" & SQLString((Me.製品NO)) & "'"
		sql = sql & " AND 仕様NO = '" & SQLString((Me.仕様NO)) & "' COLLATE Japanese_Bin"
		
		Select Case m_在庫管理
			Case Type在庫管理.在庫管理する, Type在庫管理.在庫管理しない
				sql = sql & " AND 在庫区分 = " & m_在庫管理
			Case Type在庫管理.全て
		End Select
		
		'2015/01/10 ADD↓
		Select Case m_廃盤表示
			Case 0 '表示する
			Case Else '表示しない
				sql = sql & " AND 廃盤FLG = 0"
		End Select
		'2015/01/10 ADD↑
		
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
				
				Me.製品NO = .Fields("製品NO").Value
				Me.仕様NO = .Fields("仕様NO").Value
				Me.漢字名称 = .Fields("漢字名称").Value
				Me.カナ名称 = .Fields("カナ名称").Value
				Me.ベース色 = .Fields("ベース色").Value
				Me.W = .Fields("W").Value
				Me.D = .Fields("D").Value
				Me.H = .Fields("H").Value
				Me.D1 = .Fields("D1").Value
				Me.D2 = .Fields("D2").Value
				Me.H1 = .Fields("H1").Value
				Me.H2 = .Fields("H2").Value
				Me.単位名 = .Fields("単位名").Value
				Me.グループ = .Fields("グループ").Value
				Me.主仕入先CD = .Fields("主仕入先CD").Value
				Me.定価 = .Fields("定価").Value
				Me.売上単価 = .Fields("売上単価").Value
				Me.仕入単価 = .Fields("仕入単価").Value
				Me.売上税区分 = .Fields("売上税区分").Value
				Me.仕入税区分 = .Fields("仕入税区分").Value
				Me.在庫区分 = .Fields("在庫区分").Value
				
				Me.JANCODE = .Fields("JANCODE").Value '2014/06/16 ADD
				Me.ロケーション = .Fields("ロケーション").Value '2015/01/29 ADD
				
				Me.費用区分 = .Fields("費用区分").Value '2015/12/23 ADD
				
				Me.入出庫用CD = .Fields("入出庫用CD").Value '2018/03/27 ADD
				
				Me.サイズ変更区分 = .Fields("サイズ変更区分").Value '2020/10/10 ADD
				
				Me.取組区分 = .Fields("取組区分").Value '2023/07/28 ADD
				Me.取組終了年月 = .Fields("取組終了年月") '2023/07/28 ADD
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
	'   レコードを読み込むメソッド 選択画面で使用
	'//////////////////////////////////////
	Public Function GetRs(ByVal p製品NO As String, ByVal p仕様NO As String, ByVal p漢字名称 As String, ByVal p主仕入先CD As String, ByVal pW As String, ByVal pD As String, ByVal pH As String, ByVal pD1 As String, ByVal pD2 As String, ByVal pH1 As String, ByVal pH2 As String) As ADODB.Recordset
		Dim rs As ADODB.Recordset
		Dim sql As String
		Dim whr As String
		
		On Error GoTo GetRs_Err
		
		'SQL生成
		sql = "SELECT SE.製品NO, SE.仕様NO, SE.漢字名称,SE.W,SE.D,SE.H,SE.D1,SE.D2,SE.H1,SE.H2,SI.略称 "
		sql = sql & "FROM TM製品 AS SE "
		sql = sql & "LEFT JOIN TM仕入先 AS SI ON SE.主仕入先CD = SI.仕入先CD"
		
		If p製品NO <> vbNullString Then
			whr = whr & " SE.製品NO like '" & SQLString(p製品NO) & "%'"
		End If
		If p仕様NO <> vbNullString Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & " SE.仕様NO LIKE '" & SQLString(p仕様NO) & "%'"
		End If
		If p漢字名称 <> vbNullString Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & " SE.漢字名称 LIKE '%" & SQLString(p漢字名称) & "%'"
		End If
		If p主仕入先CD <> vbNullString Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & " 主仕入先CD = " & SQLQuoteString(p主仕入先CD)
		End If
		
		If pW <> vbNullString Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & " SE.W = " & pW
		End If
		
		If pD <> vbNullString Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & " SE.D = " & pD
		End If
		If pH <> vbNullString Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & " SE.H = " & pH
		End If
		If pD1 <> vbNullString Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & " SE.D1 = " & pD1
		End If
		If pD2 <> vbNullString Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & " SE.D2 = " & pD2
		End If
		If pH1 <> vbNullString Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & " SE.H1 = " & pH1
		End If
		If pH2 <> vbNullString Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & " SE.H2 = " & pH2
		End If
		
		Select Case m_在庫管理
			Case Type在庫管理.在庫管理する, Type在庫管理.在庫管理しない
				If whr <> "" Then
					whr = whr & " and "
				End If
				whr = whr & " SE.在庫区分 = " & m_在庫管理
			Case Type在庫管理.全て
		End Select
		
		'2015/01/10 ADD↓
		Select Case m_廃盤表示
			Case 0 '表示する
			Case Else '表示しない
				If whr <> "" Then
					whr = whr & " AND "
				End If
				whr = whr & " SE.廃盤FLG = 0"
		End Select
		'2015/01/10 ADD↑
		
		If whr <> vbNullString Then
			sql = sql & " WHERE " & whr
		End If
		
		sql = sql & " ORDER BY 製品NO, 仕様NO"
		
		'DBクローズするのに必要
		Cn.CursorLocation = ADODB.CursorLocationEnum.adUseClient
		
		'SQL実行
		GetRs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
		'UPGRADE_NOTE: オブジェクト GetRs.ActiveConnection をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		GetRs.ActiveConnection = Nothing
		
		'DBクローズするのに必要
		Cn.CursorLocation = ADODB.CursorLocationEnum.adUseServer
		
		Exit Function
GetRs_Err: 
		'エラーの生成
		Err.Raise(Err.Number,  , Err.Description)
	End Function
	
	'2014/06/16 ADD↓
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
		sql = sql & " FROM TM製品"
		sql = sql & " WHERE 製品NO = '" & SQLString((Me.製品NO)) & "'"
		sql = sql & " AND 仕様NO = '" & SQLString((Me.仕様NO)) & "'"
		
		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
		
		With rs
			Select Case .EOF
				Case True
					.AddNew()
					.Fields("製品NO").Value = Me.製品NO
					.Fields("仕様NO").Value = Me.仕様NO
					
					''                ![入出庫用CD] = Right$("0000000000000000" & GetCounter("製品入出庫用CD"), 16) '2018/03/27 ADD
					
					.Fields("初期登録日").Value = Today
				Case False
			End Select
			
			.Fields("漢字名称").Value = Me.漢字名称
			.Fields("カナ名称").Value = Me.カナ名称
			.Fields("ベース色").Value = Me.ベース色
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Fields("W").Value = NullToZero((Me.W))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Fields("D").Value = NullToZero((Me.D))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Fields("H").Value = NullToZero((Me.H))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Fields("D1").Value = NullToZero((Me.D1))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Fields("D2").Value = NullToZero((Me.D2))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Fields("H1").Value = NullToZero((Me.H1))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Fields("H2").Value = NullToZero((Me.H2))
			.Fields("単位名").Value = Me.単位名
			.Fields("グループ").Value = Me.グループ
			.Fields("主仕入先CD").Value = Me.主仕入先CD
			.Fields("定価").Value = Me.定価
			.Fields("売上単価").Value = Me.売上単価
			.Fields("仕入単価").Value = Me.仕入単価
			.Fields("売上税区分").Value = Me.売上税区分
			.Fields("仕入税区分").Value = Me.仕入税区分
			.Fields("在庫区分").Value = Me.在庫区分
			
			.Fields("JANCODE").Value = Me.JANCODE '2014/06/16 ADD
			
			.Fields("廃盤FLG").Value = Me.廃盤FLG '2015/01/10 ADD
			
			.Fields("ロケーション").Value = Me.ロケーション '2015/01/29 ADD
			
			.Fields("費用区分").Value = Me.費用区分 '2015/12/23 ADD
			
			''        ![入出庫用CD] = Me.入出庫用CD '2018/03/27 ADD
			
			.Fields("登録変更日").Value = Today
			
			'廃盤復活の場合も連番が付いてないとき連番を付ける
			If Me.廃盤FLG = 0 And Me.入出庫用CD = "" Then
				.Fields("入出庫用CD").Value = Right("0000000000000000" & GetCounter("製品入出庫用CD"), 16) '2018/03/27 ADD
			End If
			
			.Fields("サイズ変更区分").Value = Me.サイズ変更区分 '2020/10/10 ADD
			
			.Fields("サイズ変更区分").Value = Me.サイズ変更区分 '2020/10/10 ADD
			
			.Fields("取組区分").Value = Me.取組区分 '2023/07/28 ADD
			'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Fields("取組終了年月").Value = SpcToNull((Me.取組終了年月)) '2023/07/28 ADD
			
			
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
		sql = sql & " FROM TM製品"
		sql = sql & " WHERE 製品NO = '" & SQLString((Me.製品NO)) & "' "
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
		
		'Call HourGlass(True)
		
		' コマンドを実行する接続先を指定する
		cmd.let_ActiveConnection(Cn)
		cmd.CommandText = "usp_ChkDelFor製品"
		cmd.CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
		
		' それぞれのパラメータの値を指定する
		With cmd.Parameters
			.Item(1).Value = Me.製品NO
			.Item(2).Value = Me.仕様NO
		End With
		
		cmd.Execute()
		
		If (cmd.Parameters(0).Value = 0) Then
			PurgeChk = False
		Else
			PurgeChk = True
			'CriticalAlarm (cmd(3))
			'エラーの生成
			Err.Raise(cmd.Parameters(3).Value,  , cmd.Parameters(4))
			
		End If
		
		'UPGRADE_NOTE: オブジェクト cmd をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		cmd = Nothing
		
		'Call HourGlass(False)
	End Function
	'2014/06/16 ADD↑
	
	Public Function Get売上税区分名(ByRef ID As String) As String
		Select Case ID
			Case CStr(0)
				Get売上税区分名 = "外税"
			Case CStr(1)
				Get売上税区分名 = "内税"
			Case CStr(2)
				Get売上税区分名 = "非課税"
		End Select
	End Function
	
	Friend Function Get仕入税区分名(ByRef ID As String) As String
		Select Case ID
			Case CStr(0)
				Get仕入税区分名 = "外税"
			Case CStr(1)
				Get仕入税区分名 = "内税"
			Case CStr(2)
				Get仕入税区分名 = "非課税"
		End Select
	End Function
	
	Public Function Getサイズ変更区分名(ByRef ID As String) As String
		Select Case ID
			Case CStr(0)
				Getサイズ変更区分名 = "不可"
			Case CStr(1)
				Getサイズ変更区分名 = "可"
		End Select
	End Function
	
	'2023/07/28 ADD
	Public Function Get取組区分名(ByRef ID As String) As String
		Select Case ID
			Case CStr(0)
				Get取組区分名 = "一般製品"
			Case CStr(1)
				Get取組区分名 = "取組製品"
		End Select
	End Function
End Class