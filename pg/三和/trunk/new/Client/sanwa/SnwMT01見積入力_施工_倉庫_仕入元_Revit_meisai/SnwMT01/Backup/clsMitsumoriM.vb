Option Strict Off
Option Explicit On
Friend Class clsMitsumoriM
	'   2018/05/03  oosawa      新設
	'   2018/11/03  oosawa      明細備考追加
	'///////////////////////////
	'TD見積シートMクラス
	'///////////////////////////
	
	'変数
	'明細連番
	Private m_見積明細連番 As Integer
	Private m_見積番号 As Integer
	Private m_行番号 As Integer
	Private m_SP区分 As String
	Private m_PC区分 As String
	'製品NO
	Private m_製品NO As String
	'仕様NO
	Private m_仕様NO As String
	'漢字名称
	Private m_漢字名称 As String
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
	'定価
	Private m_定価 As Decimal
	'仕入単価
	Private m_仕入単価 As Decimal
	'売上単価
	Private m_売上単価 As Decimal
	'売上税区分
	Private m_売上税区分 As Short
	''グループ
	'Private m_グループ As String
	'仕入先CD
	Private m_仕入先CD As String
	Private m_仕入先名 As String
	'仕入業者CD
	Private m_仕入業者CD As String
	Private m_仕入業者名 As String
	'配送先CD
	Private m_配送先CD As String
	Private m_配送先名 As String
	
	Private m_作業区分CD As String
	
	Private m_製品区分 As String
	
	Private m_明細備考 As String '2018/11/03 ADD
	
	''''''仕入税区分
	'''''Private m_作業区分CD As Integer
	''''''在庫区分
	'''''Private m_在庫区分 As Integer
	'''''
	''''''在庫管理する・しない
	'''''Private m_在庫管理 As Type在庫管理
	''''''在庫管理の種類
	'''''Public Enum Type在庫管理
	'''''    在庫管理する
	'''''    在庫管理しない
	'''''    全て
	'''''End Enum
	''''''JANCODE
	'''''Private m_JANCODE   As String   '2014/06/16 ADD
	
	''''''廃盤FLG
	'''''Private m_廃盤FLG As Integer    '2015/01/10 ADD
	'''''Private m_廃盤表示 As Type廃盤表示   '2015/01/10 ADD
	''''''在庫管理の種類
	'''''Public Enum Type廃盤表示
	'''''    廃盤表示する
	'''''    廃盤表示しない
	'''''End Enum
	'''''
	''''''ロケーション
	'''''Private m_ロケーション   As String   '2015/01/29 ADD
	'''''
	'''''Private m_費用区分  As Integer  '2015/12/23 ADD
	'''''
	''''''入出庫用コード
	'''''Private m_入出庫用CD    As String   '2018/03/27 ADD
	
	
	Private m_MaxLength As Integer '桁数
	Private m_EOF As Boolean '存在確認
	Private m_Error As Short 'エラー
	
	'Private m_Datas() As Variant    '選択配列
	''//////////////////////////////////////
	''   製品連番配列
	''//////////////////////////////////////
	'Public Property Let Array製品(ByVal vData As Variant)
	'        m_Datas = vData
	'End Property
	'
	'Public Property Get Array製品() As Variant
	'    Array製品 = m_Datas
	'End Property
	'
	'//////////////////////////////////////
	'   見積明細連番
	'//////////////////////////////////////
	
	Public Property 見積明細連番() As Integer
		Get
			[見積明細連番] = m_見積明細連番
		End Get
		Set(ByVal Value As Integer)
			m_見積明細連番 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   見積番号
	'//////////////////////////////////////
	
	Public Property 見積番号() As Integer
		Get
			[見積番号] = m_見積番号
		End Get
		Set(ByVal Value As Integer)
			m_見積番号 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   行番号
	'//////////////////////////////////////
	
	Public Property 行番号() As Integer
		Get
			[行番号] = m_行番号
		End Get
		Set(ByVal Value As Integer)
			m_行番号 = Value
		End Set
	End Property
	
	'//////////////////////////////////////
	'   SP区分
	'//////////////////////////////////////
	
	Public Property SP区分() As String
		Get
			[SP区分] = Trim(m_SP区分)
		End Get
		Set(ByVal Value As String)
			m_SP区分 = Value
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
	'''''//////////////////////////////////////
	'''''   カナ名称
	'''''//////////////////////////////////////
	''''Public Property Let カナ名称(ByVal vData As String)
	''''        m_カナ名称 = vData
	''''End Property
	''''Public Property Get カナ名称() As String
	''''    カナ名称 = m_カナ名称
	''''End Property
	'//////////////////////////////////////
	'   ベース色
	'//////////////////////////////////////
	Public Property ベース色() As String
		Get
			[ベース色] = Trim(m_ベース色)
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
	''''//////////////////////////////////////
	''''   グループ
	''''//////////////////////////////////////
	'''Public Property Let グループ(ByVal vData As String)
	'''    m_グループ = vData
	'''End Property
	'''Public Property Get グループ() As String
	'''    グループ = m_グループ
	'''End Property
	'//////////////////////////////////////
	'   仕入業者CD
	'//////////////////////////////////////
	Public Property 仕入業者CD() As String
		Get
			[仕入業者CD] = m_仕入業者CD
		End Get
		Set(ByVal Value As String)
			m_仕入業者CD = Value
		End Set
	End Property
	'//////////////////////////////////////
	'   仕入業者名
	'//////////////////////////////////////
	Public Property 仕入業者名() As String
		Get
			[仕入業者名] = m_仕入業者名
		End Get
		Set(ByVal Value As String)
			m_仕入業者名 = Value
		End Set
	End Property
	'//////////////////////////////////////
	'   仕入先CD（出荷元）
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
	'   仕入先名
	'//////////////////////////////////////
	Public Property 仕入先名() As String
		Get
			[仕入先名] = m_仕入先名
		End Get
		Set(ByVal Value As String)
			m_仕入先名 = Value
		End Set
	End Property '//////////////////////////////////////
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
	'   配送先名
	'//////////////////////////////////////
	Public Property 配送先名() As String
		Get
			[配送先名] = m_配送先名
		End Get
		Set(ByVal Value As String)
			m_配送先名 = Value
		End Set
	End Property '//////////////////////////////////////
	'   作業区分CD
	'//////////////////////////////////////
	
	Public Property 作業区分CD() As String
		Get
			[作業区分CD] = VB6.Format(m_作業区分CD, "#")
		End Get
		Set(ByVal Value As String)
			m_作業区分CD = Value
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
	'   製品区分
	'//////////////////////////////////////
	Public Property 製品区分() As String
		Get
			[製品区分] = m_製品区分
		End Get
		Set(ByVal Value As String)
			m_製品区分 = Value
		End Set
	End Property
	'//////////////////////////////////////
	'   明細備考
	'//////////////////////////////////////
	Public Property 明細備考() As String
		Get
			[明細備考] = m_明細備考
		End Get
		Set(ByVal Value As String)
			m_明細備考 = Value
		End Set
	End Property
	''//////////////////////////////////////
	''   仕入税区分
	''//////////////////////////////////////
	'Public Property Let 仕入税区分(ByVal vData As Integer)
	'    m_仕入税区分 = vData
	'End Property
	'Public Property Get 仕入税区分() As Integer
	'    仕入税区分 = m_仕入税区分
	'End Property
	'//////////////////////////////////////
	'   在庫区分
	'//////////////////////////////////////
	'Public Property Let 在庫区分(ByVal vData As Integer)
	'    m_在庫区分 = vData
	'End Property
	'Public Property Get 在庫区分() As Integer
	'    在庫区分 = m_在庫区分
	'End Property
	''//////////////////////////////////////
	''   在庫管理するか:0しないか:1
	''//////////////////////////////////////
	'Public Property Let isDo在庫管理(ByVal vData As Type在庫管理)
	'    m_在庫管理 = vData
	'End Property
	
	''2014/06/06 ADD↓
	''//////////////////////////////////////
	''   JANCODE
	''//////////////////////////////////////
	'Public Property Let JANCODE(ByVal vData As String)
	'    m_JANCODE = vData
	'End Property
	'Public Property Get JANCODE() As String
	'    JANCODE = m_JANCODE
	'End Property
	''2014/06/06 ADD↑
	'
	''2015/01/10 ADD↓
	''//////////////////////////////////////
	''   廃盤FLG 0:廃盤でない 以外：廃盤
	''//////////////////////////////////////
	'Public Property Let 廃盤FLG(ByVal vData As Integer)
	'    m_廃盤FLG = vData
	'End Property
	'Public Property Get 廃盤FLG() As Integer
	'    廃盤FLG = m_廃盤FLG
	'End Property
	''//////////////////////////////////////
	''   廃盤表示するか:0しないか:1
	''//////////////////////////////////////
	'Public Property Let isDo廃盤表示(ByVal vData As Type廃盤表示)
	'    m_廃盤表示 = vData
	'End Property
	''2015/01/10 ADD↑
	
	''2015/01/29 ADD↓
	''//////////////////////////////////////
	''   ロケーション
	''//////////////////////////////////////
	'Public Property Let ロケーション(ByVal vData As String)
	'    m_ロケーション = vData
	'End Property
	'Public Property Get ロケーション() As String
	'    ロケーション = m_ロケーション
	'End Property
	''2015/01/29 ADD↑
	'
	''2015/12/23 ADD↓
	''2015/01/10 ADD↓
	''//////////////////////////////////////
	''   費用区分 0:部材費 1：労務費
	''//////////////////////////////////////
	'Public Property Let 費用区分(ByVal vData As Integer)
	'    m_費用区分 = vData
	'End Property
	'Public Property Get 費用区分() As Integer
	'    費用区分 = m_費用区分
	'End Property
	''2015/12/23 ADD↑
	'
	''2018/03/27 ADD↓
	''//////////////////////////////////////
	''   入出庫用コード
	''//////////////////////////////////////
	'Public Property Let 入出庫用CD(ByVal vData As String)
	'    m_入出庫用CD = vData
	'End Property
	'Public Property Get 入出庫用CD() As String
	'    入出庫用CD = m_入出庫用CD
	'End Property
	''2018/03/27 ADD↑
	
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
		'    m_カナ名称 = vbNullString
		m_ベース色 = vbNullString
		m_W = 0
		m_D = 0
		m_H = 0
		m_D1 = 0
		m_D2 = 0
		m_H1 = 0
		m_H2 = 0
		
		m_単位名 = vbNullString
		'    m_グループ = vbNullString
		m_仕入先CD = vbNullString
		m_定価 = 0
		m_売上単価 = 0
		m_仕入単価 = 0
		m_売上税区分 = 0
		
		m_明細備考 = vbNullString
		'    m_仕入税区分 = 0
		'    m_在庫区分 = 0
		
		'    m_JANCODE = vbNullString    '2014/06/16 ADD
		
		'    m_廃盤FLG = 0   '2015/01/10 ADD
		'    m_廃盤表示 = 1
		
		'    m_ロケーション = vbNullString    '2015/01/29 ADD
		
		'    m_費用区分 = 0  '2015/12/23 ADD
		
		'    m_入出庫用CD = vbNullString    '2018/03/27 ADD
		
	End Sub
	
	'//////////////////////////////////////
	'   選択画面
	'//////////////////////////////////////
	Public Function ShowDialog() As Boolean
		Dim fSentak As MitsuMSen_cls
		
		fSentak = New MitsuMSen_cls
		
		With fSentak
			.Parent_Renamed = Me
			.ShowDialog()
			If .DialogResult_Renamed Then
				Me.見積明細連番 = CInt(.DialogResultCode)
				
				If Me.GetbyID = True Then
					ShowDialog = True
				Else
					ShowDialog = False
				End If
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
		sql = "SELECT  見積明細連番,見積番号,"
		sql = sql & " 行番号,"
		sql = sql & " SP区分,PC区分,製品NO,仕様NO,"
		sql = sql & " ベース色,漢字名称,"
		sql = sql & " W,D,H,D1,D2,H1,H2,"
		'    sql = sql & " エラー内容,"
		'    sql = sql & " 見積数量,"
		sql = sql & " 単位名,"
		sql = sql & " 定価,仕入単価,仕入率,売上単価,売上率,"
		'    sql = sql & " U区分,M区分,"
		'    sql = sql & " 仕入金額,売上金額,"
		sql = sql & " 売上税区分,"
		'    sql = sql & " 消費税額,"
		sql = sql & " 仕入先CD,仕入先名,配送先CD,配送先名,"
		'    sql = sql & " 社内在庫数,客先在庫数,"
		'    sql = sql & " 転用数,発注数,発注調整数,総数量,"
		sql = sql & " 製品区分,"
		sql = sql & " 見積区分,"
		'    sql = sql & " 他社伝票番号,他社納品日付,"
		'    sql = sql & " 施工対象見積番号,クレーム明細区分,"
		sql = sql & " 作業区分CD , 仕入業者CD, 仕入業者名,"
		sql = sql & " 明細備考,"
		sql = sql & " 初期登録日,登録変更日"
		sql = sql & " FROM TD見積シートM"
		sql = sql & " WHERE 見積明細連番 = " & Me.見積明細連番
		
		'SQL実行
		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
		
		With rs
			If .eof Then
				GetbyID = False
				m_EOF = True
				
				Me.Initialize()
			Else
				GetbyID = True
				m_EOF = False
				
				Me.見積明細連番 = .Fields("見積明細連番").Value
				Me.見積番号 = .Fields("見積番号").Value
				Me.行番号 = .Fields("行番号").Value
				Me.SP区分 = .Fields("SP区分").Value
				Me.PC区分 = .Fields("PC区分").Value
				
				Me.製品NO = .Fields("製品NO").Value
				Me.仕様NO = .Fields("仕様NO").Value
				Me.漢字名称 = .Fields("漢字名称").Value
				'            Me.カナ名称 = ![カナ名称]
				Me.ベース色 = .Fields("ベース色").Value
				Me.W = .Fields("W").Value
				Me.D = .Fields("D").Value
				Me.H = .Fields("H").Value
				Me.D1 = .Fields("D1").Value
				Me.D2 = .Fields("D2").Value
				Me.H1 = .Fields("H1").Value
				Me.H2 = .Fields("H2").Value
				Me.単位名 = .Fields("単位名").Value
				'            Me.グループ = ![グループ]
				Me.仕入業者CD = .Fields("仕入業者CD").Value
				Me.仕入業者名 = .Fields("仕入業者名").Value
				Me.仕入先CD = .Fields("仕入先CD").Value
				Me.仕入先名 = .Fields("仕入先名").Value
				Me.配送先CD = .Fields("配送先CD").Value
				Me.配送先名 = .Fields("配送先名").Value
				Me.定価 = .Fields("定価").Value
				Me.売上単価 = .Fields("売上単価").Value
				Me.仕入単価 = .Fields("仕入単価").Value
				Me.売上税区分 = .Fields("売上税区分").Value
				'            Me.仕入税区分 = ![仕入税区分]
				Me.作業区分CD = .Fields("作業区分CD").Value
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Me.製品区分 = NullToZero(.Fields("製品区分"), "")
				
				Me.明細備考 = .Fields("明細備考").Value '2018/11/03 ADD
				
				'            Me.JANCODE = ![JANCODE] '2014/06/16 ADD
				'            Me.ロケーション = ![ロケーション] '2015/01/29 ADD
				'
				'            Me.費用区分 = ![費用区分] '2015/12/23 ADD
				'
				'            Me.入出庫用CD = ![入出庫用CD] '2018/03/27 ADD
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
	''
	'''//////////////////////////////////////
	'''   レコードを読み込むメソッド 選択画面で使用
	'''//////////////////////////////////////
	''Public Function GetRs(ByVal p製品NO As String, ByVal p仕様NO As String, ByVal p漢字名称 As String, ByVal p主仕入先CD As String, _
	'''                        ByVal pW As String, ByVal pD As String, ByVal pH As String, _
	'''                        ByVal pD1 As String, ByVal pD2 As String, ByVal pH1 As String, ByVal pH2 As String) As ADODB.Recordset
	''    Dim rs As ADODB.Recordset
	''    Dim sql As String
	''    Dim whr As String
	''
	''    On Error GoTo GetRs_Err
	''
	''    'SQL生成
	''    sql = "SELECT SE.製品NO, SE.仕様NO, SE.漢字名称,SE.W,SE.D,SE.H,SE.D1,SE.D2,SE.H1,SE.H2,SI.略称 "
	''    sql = sql & "FROM TM製品 AS SE "
	''    sql = sql & "LEFT JOIN TM仕入先 AS SI ON SE.主仕入先CD = SI.仕入先CD"
	''
	''    If p製品NO <> vbNullString Then
	''        whr = whr & " SE.製品NO like '" & SQLString(p製品NO) & "%'"
	''    End If
	''    If p仕様NO <> vbNullString Then
	''        If whr <> "" Then
	''            whr = whr & " and "
	''        End If
	''        whr = whr & " SE.仕様NO LIKE '" & SQLString(p仕様NO) & "%'"
	''    End If
	''    If p漢字名称 <> vbNullString Then
	''        If whr <> "" Then
	''            whr = whr & " and "
	''        End If
	''        whr = whr & " SE.漢字名称 LIKE '%" & SQLString(p漢字名称) & "%'"
	''    End If
	''    If p主仕入先CD <> vbNullString Then
	''        If whr <> "" Then
	''            whr = whr & " and "
	''        End If
	''        whr = whr & " 主仕入先CD = " & SQLQuoteString(p主仕入先CD)
	''    End If
	''
	''    If pW <> vbNullString Then
	''        If whr <> "" Then
	''            whr = whr & " and "
	''        End If
	''        whr = whr & " SE.W = " & pW
	''    End If
	''
	''    If pD <> vbNullString Then
	''        If whr <> "" Then
	''            whr = whr & " and "
	''        End If
	''        whr = whr & " SE.D = " & pD
	''    End If
	''    If pH <> vbNullString Then
	''        If whr <> "" Then
	''            whr = whr & " and "
	''        End If
	''        whr = whr & " SE.H = " & pH
	''    End If
	''    If pD1 <> vbNullString Then
	''        If whr <> "" Then
	''            whr = whr & " and "
	''        End If
	''        whr = whr & " SE.D1 = " & pD1
	''    End If
	''    If pD2 <> vbNullString Then
	''        If whr <> "" Then
	''            whr = whr & " and "
	''        End If
	''        whr = whr & " SE.D2 = " & pD2
	''    End If
	''    If pH1 <> vbNullString Then
	''        If whr <> "" Then
	''            whr = whr & " and "
	''        End If
	''        whr = whr & " SE.H1 = " & pH1
	''    End If
	''    If pH2 <> vbNullString Then
	''        If whr <> "" Then
	''            whr = whr & " and "
	''        End If
	''        whr = whr & " SE.H2 = " & pH2
	''    End If
	''
	''    Select Case m_在庫管理
	''        Case Type在庫管理.在庫管理する, Type在庫管理.在庫管理しない
	''            If whr <> "" Then
	''                whr = whr & " and "
	''            End If
	''            whr = whr & " SE.在庫区分 = " & m_在庫管理
	''        Case Type在庫管理.全て
	''    End Select
	''
	''    '2015/01/10 ADD↓
	''    Select Case m_廃盤表示
	''        Case 0 '表示する
	''        Case Else   '表示しない
	''            If whr <> "" Then
	''                whr = whr & " AND "
	''            End If
	''            whr = whr & " SE.廃盤FLG = 0"
	''    End Select
	''    '2015/01/10 ADD↑
	''
	''    If whr <> vbNullString Then
	''        sql = sql & " WHERE " & whr
	''    End If
	''
	''    sql = sql & " ORDER BY 製品NO, 仕様NO"
	''
	''    'DBクローズするのに必要
	''    Cn.CursorLocation = adUseClient
	''
	''    'SQL実行
	''    Set GetRs = OpenRs(sql, Cn, adOpenForwardOnly, adLockReadOnly)
	''    Set GetRs.ActiveConnection = Nothing
	''
	''    'DBクローズするのに必要
	''    Cn.CursorLocation = adUseServer
	''
	''    Exit Function
	''GetRs_Err:
	''    'エラーの生成
	''    Err.Raise Err.Number, , Err.Description
	''End Function
	''''''
	'''''''2014/06/16 ADD↓
	'''''''//////////////////////////////////////
	'''''''   Upload
	'''''''//////////////////////////////////////
	''''''Public Function Upload() As Boolean
	''''''    Dim rs As ADODB.Recordset, sql As String
	''''''
	''''''    'マウスポインターを砂時計にする
	''''''    'Call HourGlass(True)
	''''''
	''''''    On Error GoTo Trans_err
	''''''
	''''''    'SQL生成
	''''''    sql = "SELECT *"
	''''''    sql = sql & " FROM TM製品"
	''''''    sql = sql & " WHERE 製品NO = '" & SQLString(Me.製品NO) & "'"
	''''''    sql = sql & " AND 仕様NO = '" & SQLString(Me.仕様NO) & "'"
	''''''
	''''''    Set rs = OpenRs(sql, Cn, adOpenKeyset, adLockOptimistic)
	''''''
	''''''    With rs
	''''''        Select Case .eof
	''''''            Case True
	''''''                .AddNew
	''''''                ![製品NO] = Me.製品NO
	''''''                ![仕様NO] = Me.仕様NO
	''''''
	''''''''                ![入出庫用CD] = Right$("0000000000000000" & GetCounter("製品入出庫用CD"), 16) '2018/03/27 ADD
	''''''
	''''''                ![初期登録日] = Date
	''''''            Case False
	''''''        End Select
	''''''
	''''''        ![漢字名称] = Me.漢字名称
	''''''        ![カナ名称] = Me.カナ名称
	''''''        ![ベース色] = Me.ベース色
	''''''        ![W] = NullToZero(Me.W)
	''''''        ![D] = NullToZero(Me.D)
	''''''        ![H] = NullToZero(Me.H)
	''''''        ![D1] = NullToZero(Me.D1)
	''''''        ![D2] = NullToZero(Me.D2)
	''''''        ![H1] = NullToZero(Me.H1)
	''''''        ![H2] = NullToZero(Me.H2)
	''''''        ![単位名] = Me.単位名
	''''''        ![グループ] = Me.グループ
	''''''        ![主仕入先CD] = Me.主仕入先CD
	''''''        ![定価] = Me.定価
	''''''        ![売上単価] = Me.売上単価
	''''''        ![仕入単価] = Me.仕入単価
	''''''        ![売上税区分] = Me.売上税区分
	''''''        ![仕入税区分] = Me.仕入税区分
	''''''        ![在庫区分] = Me.在庫区分
	''''''
	''''''        ![JANCODE] = Me.JANCODE '2014/06/16 ADD
	''''''
	''''''        ![廃盤FLG] = Me.廃盤FLG '2015/01/10 ADD
	''''''
	''''''        ![ロケーション] = Me.ロケーション '2015/01/29 ADD
	''''''
	''''''        ![費用区分] = Me.費用区分 '2015/12/23 ADD
	''''''
	''''''''        ![入出庫用CD] = Me.入出庫用CD '2018/03/27 ADD
	''''''
	''''''        ![登録変更日] = Date
	''''''
	''''''        '廃盤復活の場合も連番が付いてないとき連番を付ける
	''''''        If Me.廃盤FLG = 0 And Me.入出庫用CD = "" Then
	''''''            ![入出庫用CD] = Right$("0000000000000000" & GetCounter("製品入出庫用CD"), 16) '2018/03/27 ADD
	''''''        End If
	''''''
	''''''        .Update
	''''''    End With
	''''''
	''''''    Call ReleaseRs(rs)
	''''''
	''''''    Upload = True
	''''''
	''''''Trans_Correct:
	''''''    On Error GoTo 0
	''''''    'Call HourGlass(False)
	''''''    Exit Function
	''''''
	''''''Trans_err:  '---エラー時
	''''''    'Call HourGlass(False)
	''''''    'エラーの生成
	''''''    Err.Raise Err.Number, , Err.Description
	''''''    Resume Trans_Correct
	''''''End Function
	''''''
	''''''
	'''''''//////////////////////////////////////
	'''''''   Purge
	'''''''//////////////////////////////////////
	''''''Public Function Purge() As Boolean
	''''''    Dim sql As String
	''''''
	''''''    Purge = False
	''''''
	''''''    On Error GoTo Trans_err
	''''''
	''''''    'マウスポインターを砂時計にする
	''''''    'Call HourGlass(True)
	''''''
	''''''    '---他データ使用状況チェック
	''''''    If Me.PurgeChk() Then
	''''''        'Call HourGlass(False)
	''''''        Exit Function
	''''''    End If
	''''''
	''''''    'SQL生成
	''''''    sql = "DELETE "
	''''''    sql = sql & " FROM TM製品"
	''''''    sql = sql & " WHERE 製品NO = '" & SQLString(Me.製品NO) & "' "
	''''''    sql = sql & " AND 仕様NO = '" & SQLString(Me.仕様NO) & "'"
	''''''
	''''''
	''''''    Cn.Execute sql
	''''''
	''''''    Purge = True
	''''''
	''''''Trans_Correct:
	''''''    On Error GoTo 0
	''''''
	''''''    'Call HourGlass(False)
	''''''    Exit Function
	''''''
	''''''Trans_err:  '---エラー時
	''''''    'Call HourGlass(False)
	''''''    'エラーの生成
	''''''    Err.Raise Err.Number, , Err.Description
	''''''    Resume Trans_Correct
	''''''End Function
	''''''
	'''''''//////////////////////////////////////
	'''''''   PurgeChk
	'''''''//////////////////////////////////////
	''''''Friend Function PurgeChk() As Boolean
	''''''    Dim cmd As New ADODB.Command
	''''''
	''''''    PurgeChk = False
	''''''
	''''''    'Call HourGlass(True)
	''''''
	''''''    ' コマンドを実行する接続先を指定する
	''''''    cmd.ActiveConnection = Cn
	''''''    cmd.CommandText = "usp_ChkDelFor製品"
	''''''    cmd.CommandType = adCmdStoredProc
	''''''
	''''''    ' それぞれのパラメータの値を指定する
	''''''    With cmd.Parameters
	''''''        .Item(1).Value = Me.製品NO
	''''''        .Item(2).Value = Me.仕様NO
	''''''    End With
	''''''
	''''''    cmd.Execute
	''''''
	''''''    If (cmd(0) = 0) Then
	''''''        PurgeChk = False
	''''''    Else
	''''''        PurgeChk = True
	''''''        'CriticalAlarm (cmd(3))
	''''''        'エラーの生成
	''''''        Err.Raise cmd(3), , cmd(4)
	''''''
	''''''    End If
	''''''
	''''''    Set cmd = Nothing
	''''''
	''''''    'Call HourGlass(False)
	''''''End Function
	'''''''2014/06/16 ADD↑
	''''''
	''''''Public Function Get売上税区分名(ID As String) As String
	''''''    Select Case ID
	''''''        Case 0
	''''''            Get売上税区分名 = "外税"
	''''''        Case 1
	''''''            Get売上税区分名 = "内税"
	''''''        Case 2
	''''''            Get売上税区分名 = "非課税"
	''''''    End Select
	''''''End Function
	''''''
	''''''Friend Function Get仕入税区分名(ID As String) As String
	''''''    Select Case ID
	''''''        Case 0
	''''''            Get仕入税区分名 = "外税"
	''''''        Case 1
	''''''            Get仕入税区分名 = "内税"
	''''''        Case 2
	''''''            Get仕入税区分名 = "非課税"
	''''''    End Select
	''''''End Function
End Class