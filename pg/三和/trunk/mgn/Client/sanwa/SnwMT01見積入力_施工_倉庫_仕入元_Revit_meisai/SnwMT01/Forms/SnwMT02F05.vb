Option Strict Off
Option Explicit On
Friend Class SnwMT02F05
	Inherits System.Windows.Forms.Form
	
	'--------------------------------------------------------------------
	'   UPDATE
	'       2018/06/20  oosawa      見積数量がゼロのデータはださない
	'--------------------------------------------------------------------
	
	'フォームの再描画用のAPI定義
	Private Declare Function LockWindowUpdate Lib "user32" (ByVal Hwnd As Integer) As Integer
	'リストビュー設定用のAPI定義
	Private Declare Function GetWindowLong Lib "user32"  Alias "GetWindowLongA"(ByVal Hwnd As Integer, ByVal nlndex As Integer) As Integer
	Private Declare Function SetWindowLong Lib "user32"  Alias "SetWindowLongA"(ByVal Hwnd As Integer, ByVal nindex As Integer, ByVal dwNewLong As Integer) As Integer
	'UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
	Private Declare Function SendMessage Lib "user32"  Alias "SendMessageA"(ByVal Hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer, ByRef lParam As Any) As Integer
	
	'リストビュー設定用の定数
	Private Const GWL_STYLE As Short = (-16)
	Private Const LVM_FIRST As Integer = &H1000
	Private Const LVM_GETHEADER As Decimal = (LVM_FIRST + 31)
	'ヘッダのスタイル
	Private Const HDS_BUTTONS As Integer = &H2
	
	Dim pGetKBN As Short
	Dim pPCKUBN As String
	Dim pSEIHNO As String
	Dim pSIYONO As String
	
	'2009/07/28 ADD↓
	Dim pTOKUCD As String
	Dim pSIRCD As String
	'2009/07/28 ADD↑
	
	Dim CLKFLG As Boolean 'リストビュー制御用
	Dim sql As String
	Dim rs As ADODB.Recordset
	Dim ResultCodeSetControl As System.Windows.Forms.Control '選択したコードの送り先をセットする。
	Dim MeWidth, MeHeight As Integer
	Dim wkWidth As Short
	Dim WidthSa As Short
	Dim WkHeight As Integer
	Dim LvHeightLimit, MeHeightLimit As Integer
	Dim SelectF As Boolean
	
	Dim idc As New iDate '2012/10/3 ADD
	
	'クラス
	Private cTokuisaki As clsTokuisaki
	Private cSiiresaki As clsSiiresaki
	
	Private Sub SetFlatHeader(ByRef Target As System.Windows.Forms.ListView)
		'指定されたリストビューのヘッダを平面（フラット）にする
		Dim lngHeader As Integer
		Dim lngStyle As Integer
		Dim lngAPIReVal As Integer
		
		'ヘッダーのハンドルを取得
		lngHeader = SendMessage(Target.Handle.ToInt32, LVM_GETHEADER, 0, 0)
		'ヘッダのウィンドウスタイルを取得
		lngStyle = GetWindowLong(lngHeader, GWL_STYLE)
		'ヘッダをフラットスタイルに設定
		lngAPIReVal = SetWindowLong(lngHeader, GWL_STYLE, lngStyle Xor HDS_BUTTONS)
	End Sub
	
	Private Sub SnwMT02F05_FormClosing(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
		Dim Cancel As Boolean = eventArgs.Cancel
		Dim UnloadMode As System.Windows.Forms.CloseReason = eventArgs.CloseReason
		'UPGRADE_NOTE: オブジェクト ResultCodeSetControl をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		ResultCodeSetControl = Nothing
		'UPGRADE_NOTE: オブジェクト cTokuisaki をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		cTokuisaki = Nothing
		'UPGRADE_NOTE: オブジェクト cSiiresaki をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		cSiiresaki = Nothing
		eventArgs.Cancel = Cancel
	End Sub
	
	'UPGRADE_WARNING: イベント SnwMT02F05.Resize は、フォームが初期化されたときに発生します。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"' をクリックしてください。
	Private Sub SnwMT02F05_Resize(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Resize
		
		''    If Me.WindowState <> vbMinimized Then
		''        'フォーム最小（幅）制御
		''        If Me.Width < MeWidth Then
		''            Me.Width = MeWidth
		''        End If
		''        'フォーム最小（高さ）制御・リストビューの高さをフォームの高さに比例
		''        If Me.Height < MeHeightLimit Then
		''            Me.Height = MeHeightLimit
		''        End If
		''        'リストビューの高さ・ボタン位置
		''        WkHeight = SelListVw.Top + SelListVw.Height
		''        WkHeight = cmdOk.Top - WkHeight
		''        SelListVw.Height = Me.ScaleHeight - SelListVw.Top - (WkHeight * 2) - cmdOk.Height
		''        cmdOk.Top = SelListVw.Height + SelListVw.Top + WkHeight
		''''        cmdCan.Top = SelListVw.Height + SelListVw.Top + WkHeight
		''    End If
	End Sub
	
	Private Sub SnwMT02F05_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		Dim i, j As Short
		Dim List_ColLen As Short
		
		On Error GoTo Form_Load_Err
		
		'リストヘッダー項目セット
		With SelListVw
			.Columns.Clear()
			.Columns.Add("", "  見積日付", CInt(VB6.TwipsToPixelsX(1300)))
			.Columns.Add("見積番号", CInt(VB6.TwipsToPixelsX(1300)), System.Windows.Forms.HorizontalAlignment.Right)
			.Columns.Add("行番号", CInt(VB6.TwipsToPixelsX(900)), System.Windows.Forms.HorizontalAlignment.Right)
			.Columns.Add("", "見積件名", CInt(VB6.TwipsToPixelsX(4000))) '2006/11/02 ADD
			.Columns.Add("", "名称", CInt(VB6.TwipsToPixelsX(4000)))
			.Columns.Add("", "サイズ", CInt(VB6.TwipsToPixelsX(3000)))
			.Columns.Add("     原価", CInt(VB6.TwipsToPixelsX(1500)), System.Windows.Forms.HorizontalAlignment.Right)
			.Columns.Add("     売価", CInt(VB6.TwipsToPixelsX(1500)), System.Windows.Forms.HorizontalAlignment.Right)
			.Columns.Add("     数量", CInt(VB6.TwipsToPixelsX(1500)), System.Windows.Forms.HorizontalAlignment.Right)
			.Columns.Add("", "単位", CInt(VB6.TwipsToPixelsX(850)))
			.Columns.Add("", "仕入先", CInt(VB6.TwipsToPixelsX(1395))) '2006/11/02 ADD
			'2012/07/02 ADD↓
			.Columns.Add("     原価", CInt(VB6.TwipsToPixelsX(1500)), System.Windows.Forms.HorizontalAlignment.Right)
			.Columns.Add("     売価", CInt(VB6.TwipsToPixelsX(1500)), System.Windows.Forms.HorizontalAlignment.Right)
			'2012/07/02 ADD↑
			''''        .ColumnHeaders(4).Alignment = lvwColumnRight
			''''        .ColumnHeaders(5).Alignment = lvwColumnRight
			''''        .ColumnHeaders(6).Alignment = lvwColumnRight
		End With
		
		'    SelListVw.Width = 11400 + 330     'スクロールバー分プラス
		'    SelListVw.Width = 13450 + 330     'スクロールバー分プラス
		SelListVw.Width = VB6.TwipsToPixelsX(14400 + 330) 'スクロールバー分プラス
		
		'リストビューのヘッダを平面にする
		Call SetFlatHeader(SelListVw)
		
		'得意先CDの桁セット-------------------
		'UPGRADE_WARNING: TextBox プロパティ tx_得意先CD.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
		[tx_得意先CD].Maxlength = TokuIDLength
		'UPGRADE_WARNING: TextBox プロパティ tx_仕入先CD.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
		[tx_仕入先CD].Maxlength = TokuIDLength
		'------------------------------------
		
		wkWidth = (VB6.PixelsToTwipsX(Me.SelListVw.Left) * 2) + VB6.PixelsToTwipsX(SelListVw.Width) 'wkフォーム幅セット
		WidthSa = VB6.PixelsToTwipsX(Me.Width) - VB6.PixelsToTwipsX(Me.ClientRectangle.Width) 'フォームWidthの内外値差
		
		If wkWidth < 4940 Then
			wkWidth = 4940
		End If
		
		cmdOk.Left = VB6.TwipsToPixelsX(wkWidth - (VB6.PixelsToTwipsX(SelListVw.Left) + VB6.PixelsToTwipsX(cmdOk.Width) + VB6.PixelsToTwipsX([cmdCan].Width))) 'ＯＫボタン位置決定
		[cmdCan].Left = VB6.TwipsToPixelsX(wkWidth - (VB6.PixelsToTwipsX(SelListVw.Left) + VB6.PixelsToTwipsX([cmdCan].Width))) 'キャンセルボタン位置決定
		'UPGRADE_ISSUE: Form プロパティ SnwMT02F05.ScaleWidth はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Me.ScaleWidth = wkWidth 'フォーム幅決定(ScaleWidth)
		Me.Width = VB6.TwipsToPixelsX(wkWidth + WidthSa) 'フォーム幅決定(Width)
		
		'フォームを画面の中央に配置
		Me.Top = VB6.TwipsToPixelsY((VB6.PixelsToTwipsY(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height) - VB6.PixelsToTwipsY(Me.Height)) \ 2)
		Me.Left = VB6.TwipsToPixelsX((VB6.PixelsToTwipsX(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width) - VB6.PixelsToTwipsX(Me.Width)) \ 2)
		
		'クラス生成
		cTokuisaki = New clsTokuisaki
		cSiiresaki = New clsSiiresaki
		
		[tx_得意先CD].Text = pTOKUCD
		[tx_仕入先CD].Text = pSIRCD
		idc.SetupA(Me, "見積日付", 0) '2012/10/03 ADD
		idc.Text = ""
		
		ResultCodeSetControl.Tag = ""
		Call Download()
		
		Exit Sub
Form_Load_Err: 
		MsgBox(Err.Number & " " & Err.Description)
		Exit Sub
	End Sub
	
	Private Sub SnwMT02F05_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		On Error GoTo Form_KeyPress_Err
		Select Case KeyAscii
			Case System.Windows.Forms.Keys.Escape
				Me.Close()
			Case System.Windows.Forms.Keys.Return
				'ビープ音消去用
				'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
				If Me.ActiveControl.Name <> "SelListVw" Then
					KeyAscii = 0
				End If
		End Select
		GoTo EventExitSub
Form_KeyPress_Err: 
		MsgBox(Err.Number & " " & Err.Description)
		GoTo EventExitSub
EventExitSub: 
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOK.Click
		On Error GoTo cmdOk_Click_Err
		'--- リスト選択
		SetResultValues()
		Me.Close()
		
		Exit Sub
cmdOk_Click_Err: 
		MsgBox(Err.Number & " " & Err.Description)
		Exit Sub
	End Sub
	
	Private Sub cmdOk_KeyDown(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyEventArgs) Handles cmdOk.KeyDown
		Dim KeyCode As Short = eventArgs.KeyCode
		Dim Shift As Short = eventArgs.KeyData \ &H10000
		Select Case KeyCode
			Case System.Windows.Forms.Keys.Down, System.Windows.Forms.Keys.Right
				KeyCode = 0
		End Select
	End Sub
	
	Private Sub cmdCan_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCan.Click
		On Error GoTo cmdCan_Click_Err
		Me.Close()
		Exit Sub
cmdCan_Click_Err: 
		MsgBox(Err.Number & " " & Err.Description)
		Exit Sub
	End Sub
	
	Private Sub SelListVw_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles SelListVw.Enter
		If SelListVw.Items.Count = 0 Then
			[cmdCan].Focus()
		Else
			SelListVw.FocusedItem.Selected = True
		End If
	End Sub
	
	Private Sub SelListVw_DoubleClick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles SelListVw.DoubleClick
		CLKFLG = True
		SetResultValues()
	End Sub
	
	'UPGRADE_ISSUE: MSComctlLib.ListView イベント SelListVw.ItemClick はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="ABD9AF39-7E24-4AFF-AD8D-3675C1AA3054"' をクリックしてください。
	Private Sub SelListVw_ItemClick(ByVal Item As System.Windows.Forms.ListViewItem)
		CLKFLG = True
	End Sub
	
	Private Sub SelListVw_MouseDown(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles SelListVw.MouseDown
		Dim Button As Short = eventArgs.Button \ &H100000
		Dim Shift As Short = System.Windows.Forms.Control.ModifierKeys \ &H10000
		Dim X As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim Y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		CLKFLG = False
	End Sub
	
	Private Sub SelListVw_KeyDown(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyEventArgs) Handles SelListVw.KeyDown
		Dim KeyCode As Short = eventArgs.KeyCode
		Dim Shift As Short = eventArgs.KeyData \ &H10000
		Select Case KeyCode
			Case System.Windows.Forms.Keys.Return
				KeyCode = 0
				CLKFLG = True
				SetResultValues()
			Case System.Windows.Forms.Keys.Up
				If SelListVw.FocusedItem.Index = 1 Then
					KeyCode = 0
				End If
			Case System.Windows.Forms.Keys.Down
				If SelListVw.FocusedItem.Index = SelListVw.Items.Count Then
					cmdOk.Focus()
				End If
		End Select
	End Sub
	
	''Private Sub SelListVw_KeyPress(KeyAscii As Integer)
	''    Dim itmFound As ListItem   ' FoundItem 変数。
	''
	''    Set itmFound = SelListVw.FindItem(Space(1) & Chr$(KeyAscii), lvwText, , lvwPartial)
	''    KeyAscii = 0
	''   ' EnsureVisible メソッドを使用して
	''   ' コントロールをスクロールし、ListItem を選択します。
	''   If Not (itmFound Is Nothing) Then
	''       itmFound.EnsureVisible ' リスト ビュー コントロールをスクロールして、検出された ListItem を表示します。
	''       itmFound.Selected = True   ' ListItem を選択します。
	''      ' コントロールにフォーカスを返し、選択した内容を表示します。
	''       SelListVw.SetFocus
	''   End If
	''   Set itmFound = Nothing
	''End Sub
	
	Private Sub SetResultValues()
		If Not CLKFLG Then Exit Sub
		If SelListVw.Items.Count <> 0 Then
			If SelListVw.FocusedItem.Selected = False Then Exit Sub
			If Not (ResultCodeSetControl Is Nothing) Then
				With ResultCodeSetControl
					Select Case pGetKBN
						Case 1
							''                                .Text = Trim$(SelListVw.SelectedItem.SubItems(3))     '2006/11/15 DEL
							'''                                .Text = Trim$(SelListVw.SelectedItem.SubItems(4))       '2006/11/15 ADD
							'UPGRADE_WARNING: コレクション SelListVw.SelectedItem の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
							.Text = Trim(SelListVw.FocusedItem.SubItems(6).Text) '2009/07/28 ADD
							.Tag = "True"
						Case 2
							''                                .Text = Trim$(SelListVw.SelectedItem.SubItems(4))     '2006/11/15 DEL
							'''                                .Text = Trim$(SelListVw.SelectedItem.SubItems(5))       '2006/11/15 ADD
							'UPGRADE_WARNING: コレクション SelListVw.SelectedItem の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
							.Text = Trim(SelListVw.FocusedItem.SubItems(7).Text) '2009/07/28 ADD
							.Tag = "True"
					End Select
				End With
			End If
		End If
		Me.Close()
	End Sub
	
	Private Function Item_Check(ByRef ItemNo As Short) As Short
		
		On Error GoTo Item_Check_Err
		
		'キー項目「得意先CD」のチェック
		If ItemNo > [tx_得意先CD].TabIndex Then
			If ISInt(([tx_得意先CD].Text)) Then
				'UPGRADE_WARNING: TextBox プロパティ tx_得意先CD.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				[tx_得意先CD].Text = VB6.Format([tx_得意先CD].Text, New String("0", [tx_得意先CD].Maxlength))
			End If
		End If
		
		'キー項目「仕入先CD」のチェック
		If ItemNo > [tx_仕入先CD].TabIndex Then
			If ISInt(([tx_仕入先CD].Text)) Then
				'UPGRADE_WARNING: TextBox プロパティ tx_仕入先CD.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				[tx_仕入先CD].Text = VB6.Format([tx_仕入先CD].Text, New String("0", [tx_仕入先CD].Maxlength))
			End If
		End If
		
		Exit Function
Item_Check_Err: 
		MsgBox(Err.Number & " " & Err.Description)
	End Function
	
	Private Function Download() As Boolean
		Dim itmX As System.Windows.Forms.ListViewItem
		Dim itmFound As System.Windows.Forms.ListViewItem ' FoundItem 変数。
		Dim FindListText As String
		Dim Buf As String '文字成型用
		Dim i As Short
		Dim whr As String
		Dim WSize As String
		
		Download = False
		HourGlass(True)
		'2003/12/06 ADD
		'''    SQL = "SELECT TOP 5 " _
		'& "MIN(MH.見積番号) AS 見積番号, " _
		'& "CONVERT(CHAR(10),MH.見積日付,111) AS 見積日付, " _
		'& "MIN(MM.漢字名称) AS 漢字名称, MM.仕入単価, MM.売上単価, " _
		'& "MM.見積数量, MM.単位名, " _
		'& "MM.W,MM.D,MM.H,MM.D1,MM.D2,MM.H1,MM.H2 " _
		'& "FROM TD見積 AS MH " _
		'& "INNER JOIN TD見積シートM AS MM ON MH.見積番号 = MM.見積番号 " _
		'& "WHERE MM.PC区分 = '" & SQLString(pPCKUBN) & "' " _
		'& "AND MM.製品NO = '" & SQLString(pSEIHNO) & "' " _
		'& "AND MM.仕様NO = '" & SQLString(pSIYONO) & "' " _
		'& "AND NOT(MM.PC区分 = '' " _
		'& "AND MM.製品NO = '' " _
		'& "AND MM.仕様NO = '') " _
		'& "AND MH.得意先CD = '" & SQLString(HD_得意先CD) & "' " _
		'& "GROUP BY CONVERT(CHAR(10),MH.見積日付,111),MM.W,MM.D,MM.H,MM.D1,MM.D2,MM.H1,MM.H2," _
		'& "MM.仕入単価, MM.売上単価, MM.見積数量, MM.単位名 " _
		'& "ORDER BY MH.見積日付 DESC"
		
		'2006/11/02 ADD
		'    SQL = "SELECT TOP 100"
		'    SQL = SQL & " MIN(MH.見積番号) AS 見積番号,"
		'    SQL = SQL & "CONVERT(CHAR(10),MH.見積日付,111) AS 見積日付,"
		'    SQL = SQL & "MIN(MM.漢字名称) AS 漢字名称, MM.仕入単価, MM.売上単価,"
		'    SQL = SQL & "MM.見積数量, MM.単位名, "
		'    SQL = SQL & "MM.W,MM.D,MM.H,MM.D1,MM.D2,MM.H1,MM.H2,"
		'    SQL = SQL & "MH.見積件名 , MM.仕入先名"
		'    SQL = SQL & " FROM TD見積 AS MH"
		'    SQL = SQL & " INNER JOIN TD見積シートM AS MM ON MH.見積番号 = MM.見積番号"
		'    SQL = SQL & " WHERE MH.得意先CD = '" & SQLString(HD_得意先CD) & "' "
		'    SQL = SQL & " AND MM.PC区分 = '" & SQLString(pPCKUBN) & "' "
		'    SQL = SQL & " AND MM.製品NO = '" & SQLString(pSEIHNO) & "' "
		'    SQL = SQL & " AND MM.仕様NO = '" & SQLString(pSIYONO) & "' "
		'    SQL = SQL & " AND NOT(MM.PC区分 = '' AND MM.製品NO = '' AND MM.仕様NO = '') "
		''''    SQL = SQL & " AND MM.仕入単価 <> 0"                              '2007/03/09 DEL
		''''    SQL = SQL & " AND MM.売上単価 <> 0"                              '2007/03/09 DEL
		'    SQL = SQL & " AND NOT(MM.仕入単価 = 0 AND MM.売上単価 = 0)"         '2007/03/09 ADD
		'    SQL = SQL & " GROUP BY CONVERT(CHAR(10),MH.見積日付,111),MH.見積件名 , MM.仕入先名,"
		'    SQL = SQL & "MM.W,MM.D,MM.H,MM.D1,MM.D2,MM.H1,MM.H2,"
		'    SQL = SQL & "MM.仕入単価, MM.売上単価, MM.見積数量, MM.単位名 "
		'    SQL = SQL & " ORDER BY MH.見積日付 DESC"
		
		Dim bTimeout As Integer '2009/11/16 ADD
		bTimeout = Cn.CommandTimeout '2009/11/16 ADD
		Cn.CommandTimeout = 0 '2009/11/16 ADD
		
		'''2012/07/02 ADD↓
		sql = "SELECT TOP 100"
		sql = sql & " MH.見積番号,"
		sql = sql & "CONVERT(CHAR(10),MH.見積日付,111) AS 見積日付,"
		sql = sql & "MM.漢字名称, MM.仕入単価, MM.売上単価,"
		sql = sql & "MM.見積数量, MM.単位名, "
		sql = sql & "MM.W,MM.D,MM.H,MM.D1,MM.D2,MM.H1,MM.H2,"
		sql = sql & "MH.見積件名 , MM.仕入先名"
		sql = sql & " ,MM.行番号" '2009/07/28 ADD
		sql = sql & " ,SH.仕入単価 AS 仕入単価2,UH.売上単価 AS 売上単価2"
		sql = sql & " FROM TD見積 AS MH"
		sql = sql & "     INNER JOIN TD見積シートM AS MM"
		sql = sql & "         ON MH.見積番号 = MM.見積番号"
		
		'2014/12/17 ADD↓
		sql = sql & "         LEFT JOIN (SELECT UM.見積明細連番,UM.売上単価"
		sql = sql & "                        FROM TD売上明細v2 AS UM"
		sql = sql & "                    ) AS UH"
		sql = sql & "                    ON MM.見積明細連番 = UH.見積明細連番"
		sql = sql & "         LEFT JOIN (SELECT SM.見積明細連番,SM.仕入単価"
		sql = sql & "                        FROM TD仕入明細内訳 AS SM"
		sql = sql & "                    ) AS SH"
		sql = sql & "                    ON MM.見積明細連番 = SH.見積明細連番"
		'2014/12/17 ADD↑
		
		'2014/12/17 DEL↓
		'''    sql = sql & "     LEFT JOIN (SELECT UH.見積番号, UMU.見積行番号, UMU.売上単価"
		'''    sql = sql & "                 FROM TD売上  AS UH"
		'''    sql = sql & "                 INNER JOIN TD売上明細内訳 AS UMU"
		'''    sql = sql & "                     ON UH.売上番号 = UMU.売上番号"
		'''    sql = sql & "             ) AS UH"
		'''    sql = sql & "             ON UH.見積番号 = MH.見積番号"
		'''    sql = sql & "             AND UH.見積行番号 = MM.行番号"
		'''    sql = sql & "     LEFT JOIN (SELECT SH.見積番号, SHU.見積行番号, SHU.仕入単価"
		'''    sql = sql & "                 FROM TD仕入 AS SH"
		'''    sql = sql & "                 INNER JOIN TD仕入明細内訳 AS SHU"
		'''    sql = sql & "                     ON SH.仕入番号 = SHU.仕入番号"
		'''    sql = sql & "             ) AS SH"
		'''    sql = sql & "             ON SH.見積番号 = MH.見積番号"
		'''    sql = sql & "             AND SH.見積行番号 = MM.行番号"
		'2014/12/17 DEL↑
		
		sql = sql & " WHERE "
		sql = sql & " MM.PC区分 = '" & SQLString(pPCKUBN) & "' "
		sql = sql & " AND MM.製品NO = '" & SQLString(pSEIHNO) & "' "
		sql = sql & " AND MM.仕様NO = '" & SQLString(pSIYONO) & "' "
		sql = sql & " AND MM.見積数量 <> 0" '2018/06/20 ADD
		
		'''2012/07/02 ADD↑
		'''2012/07/02 DEL↓
		'''    sql = "SELECT TOP 100"
		'''    sql = sql & " MH.見積番号,"
		'''    sql = sql & "CONVERT(CHAR(10),MH.見積日付,111) AS 見積日付,"
		'''    sql = sql & "MM.漢字名称, MM.仕入単価, MM.売上単価,"
		'''    sql = sql & "MM.見積数量, MM.単位名, "
		'''    sql = sql & "MM.W,MM.D,MM.H,MM.D1,MM.D2,MM.H1,MM.H2,"
		'''    sql = sql & "MH.見積件名 , MM.仕入先名"
		'''    sql = sql & " ,MM.行番号"   '2009/07/28 ADD
		'''    sql = sql & " FROM TD見積 AS MH"
		'''    sql = sql & " INNER JOIN TD見積シートM AS MM ON MH.見積番号 = MM.見積番号"
		'''    sql = sql & " WHERE "
		'''    sql = sql & " MM.PC区分 = '" & SQLString(pPCKUBN) & "' "
		'''    sql = sql & " AND MM.製品NO = '" & SQLString(pSEIHNO) & "' "
		'''    sql = sql & " AND MM.仕様NO = '" & SQLString(pSIYONO) & "' "
		'''2012/07/02 DEL↑
		'2012/10/03 ADD↓
		If IsCheckNull((idc.Text)) = True Then
		Else
			sql = sql & " AND MH.見積日付 <= '" & idc.Text & "'" '2012/10/3 ADD
		End If
		'2012/10/03 ADD↑
		
		If IsCheckNull(([tx_得意先CD].Text)) = True Then
		Else
			sql = sql & " AND MH.得意先CD = '" & SQLString(([tx_得意先CD].Text)) & "'"
		End If
		If IsCheckNull(([tx_仕入先CD].Text)) = True Then
		Else
			sql = sql & " AND MM.仕入先CD = '" & SQLString(([tx_仕入先CD].Text)) & "'"
		End If
		
		'2016/03/10 ADD↓
		'製品Z204拓の制御
		'UPGRADE_WARNING: オブジェクト GetIni(Disp, VisibleTAKU, INIFile) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If GetIni("Disp", "VisibleTAKU", INIFile) = "FALSE" Then
			If sql <> "" Then
				sql = sql & " and "
			End If
			sql = sql & "NOT(MM.製品NO = 'Z204')"
		End If
		'2016/03/10 ADD↑
		
		
		sql = sql & " AND NOT(MM.PC区分 = '' AND MM.製品NO = '' AND MM.仕様NO = '') "
		sql = sql & " AND NOT(MM.仕入単価 = 0 AND MM.売上単価 = 0)" '2007/03/09 ADD
		sql = sql & " ORDER BY MH.見積日付 DESC,MH.見積番号,MM.行番号"
		
		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockReadOnly)
		
		Cn.CommandTimeout = bTimeout '2009/11/16 ADD
		
		If rs.EOF Then
			HourGlass(False)
			SelListVw.Items.Clear()
			ReleaseRs(rs)
			Exit Function
			''    Else
			''        rs.MoveLast
			''''        lbListCount.Caption = Format$(rs.RecordCount, "#,##0")
			''        rs.MoveFirst
		End If
		
		LockWindowUpdate(Me.Handle.ToInt32)
		
		SelListVw.Items.Clear()
		
		Do Until rs.EOF
			'---見積日付
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			Buf = " " & CStr(VB6.Format(NullToZero((rs.Fields("見積日付").Value), ""), "yyyy/mm/dd"))
			''        Set itmX = SelListVw.ListItems.Add(, CStr(rs(1).Value) & CStr(rs(3).Value) _
			'''                    & CStr(rs(4).Value) & CStr(rs(5).Value) & CStr(rs(6).Value) & " ID", buf)
			itmX = SelListVw.Items.Add(Buf)
			'---見積番号
			itmX.SubItems.Add(rs.Fields("見積番号").Value) '2009/07/28 ADD
			'---見積番号
			itmX.SubItems.Add(rs.Fields("行番号").Value) '2009/07/28 ADD
			'---見積件名
			itmX.SubItems.Add(rs.Fields("見積件名").Value) '2006/11/02 ADD
			'---漢字名称
			itmX.SubItems.Add(rs.Fields("漢字名称").Value)
			'---サイズ
			WSize = ""
			If (rs.Fields("W").Value = 0) And (rs.Fields("D").Value = 0) And (rs.Fields("H").Value = 0) And (rs.Fields("D1").Value = 0) And (rs.Fields("D2").Value = 0) And (rs.Fields("H1").Value = 0) And (rs.Fields("H2").Value = 0) Then
				
				itmX.SubItems.Add(" ")
			Else
				If (rs.Fields("W").Value <> 0) Then
					WSize = rs.Fields("W").Value & "W"
				End If
				
				If (rs.Fields("D").Value <> 0) Then
					If (WSize <> "") Then
						WSize = WSize & "×" & rs.Fields("D").Value & "D"
					Else
						WSize = rs.Fields("D").Value & "D"
					End If
				Else
					If (rs.Fields("D1").Value = 0) And (rs.Fields("D2").Value = 0) Then
					Else
						If (WSize <> "") Then
							WSize = WSize & "×" & rs.Fields("D1").Value & "D" & "/" & rs.Fields("D2").Value & "D"
						Else
							WSize = rs.Fields("D1").Value & "D" & "/" & rs.Fields("D2").Value & "D"
						End If
					End If
				End If
				
				If (rs.Fields("H").Value <> 0) Then
					If (WSize <> "") Then
						WSize = WSize & "×" & rs.Fields("H").Value & "H"
					Else
						WSize = rs.Fields("H").Value & "H"
					End If
				Else
					If (rs.Fields("H1").Value = 0) And (rs.Fields("H2").Value = 0) Then
					Else
						If (WSize <> "") Then
							WSize = WSize & "×" & rs.Fields("H1").Value & "H" & "/" & rs.Fields("H2").Value & "H"
						Else
							WSize = rs.Fields("H1").Value & "H" & "/" & rs.Fields("H2").Value & "H"
						End If
					End If
				End If
				
				itmX.SubItems.Add(WSize)
				'            itmX.ListSubItems.Add , , "1000W×2000D/3000D×4000H/5000H"
			End If
			'---仕入単価
			itmX.SubItems.Add(CStr(VB6.Format(rs.Fields("仕入単価").Value, "#,##0.00;-#,##0.00")))
			''        itmX.ListSubItems.Add , , CStr(Format$(rs(3).Value, "#,##0;-#,##0"))
			'---売上単価
			itmX.SubItems.Add(CStr(VB6.Format(rs.Fields("売上単価").Value, "#,##0.00;-#,##0.00")))
			''        itmX.ListSubItems.Add , , CStr(Format$(rs(4).Value, "#,##0;-#,##0"))
			'---数量
			itmX.SubItems.Add(CStr(VB6.Format(rs.Fields("見積数量").Value, "#,##0.00;-#,##0.00")))
			'---単位
			itmX.SubItems.Add(CStr(rs.Fields("単位名").Value))
			'---仕入先名
			itmX.SubItems.Add(rs.Fields("仕入先名").Value) '2006/11/02 ADD
			
			'2012/07/02 ADD↓
			'---仕入単価
			itmX.SubItems.Add(CStr(VB6.Format(rs.Fields("仕入単価2").Value, "#,##0.00;-#,##0.00")))
			'---売上単価
			itmX.SubItems.Add(CStr(VB6.Format(rs.Fields("売上単価2").Value, "#,##0.00;-#,##0.00")))
			'2012/07/02 ADD↑
			rs.MoveNext()
		Loop 
		ReleaseRs(rs)
		'UPGRADE_NOTE: オブジェクト itmX をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		itmX = Nothing
		
		''    SelListVw.SortKey = 0
		''    SelListVw.Sorted = True
		SelListVw.Refresh()
		
		''    '選択されているテキストを保持する
		''    FindListText = Space$(1) & CStr(tx_検索ID)
		''    Set itmFound = SelListVw.FindItem(FindListText, lvwText, , lvwWhole)
		
		' EnsureVisible メソッドを使用して
		' コントロールをスクロールし、ListItem を選択します。
		If Not (itmFound Is Nothing) Then
			'UPGRADE_WARNING: MSComctlLib.ListItem メソッド itmFound.EnsureVisible には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
			itmFound.EnsureVisible() ' リスト ビュー コントロールをスクロールして、検出された ListItem を表示します。
			itmFound.Selected = True ' ListItem を選択します。
		End If
		'UPGRADE_NOTE: オブジェクト itmFound をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		itmFound = Nothing
		
		LockWindowUpdate(0)
		HourGlass(False)
		
		Download = True
	End Function
	
	'------------------------------------------
	'   前回単価参照画面セットアップ
	'       PCKUBN      :PC区分
	'       SEIHNO      :製品NO
	'       SIYONO      :仕様NO
	'------------------------------------------
	'''Public Function SelSetup(GetKBN As Integer, ctl As Control, PCKUBN As String, SEIHNO As String, SIYONO As String)
	Public Function SelSetup(ByRef GetKBN As Short, ByRef ctl As System.Windows.Forms.Control, ByRef PCKUBN As String, ByRef SEIHNO As String, ByRef SIYONO As String, ByRef TOKUCD As String, ByRef SirCD As String) As Object
		
		ResultCodeSetControl = ctl
		pGetKBN = GetKBN
		pPCKUBN = PCKUBN
		pSEIHNO = SEIHNO
		pSIYONO = SIYONO
		
		'2009/07/28 ADD↓
		pTOKUCD = TOKUCD
		pSIRCD = SirCD
		'2009/07/28 ADD↑
	End Function
	
	Private Sub tx_得意先CD_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_得意先CD.Enter
		If Item_Check(([tx_得意先CD].TabIndex)) = False Then
			Exit Sub
		End If
		'    Set PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_得意先CD_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_得意先CD.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		If KeyAscii = Asc(" ") And ([tx_得意先CD].SelectionStart = 0 And [tx_得意先CD].SelectionLength = Len([tx_得意先CD].Text)) Then
			KeyAscii = 0
			'---参照画面表示
			If cTokuisaki.ShowDialog = True Then
				[tx_得意先CD].Text = cTokuisaki.得意先CD
				[tx_得意先CD].Focus()
			Else
				[tx_得意先CD].Focus()
			End If
		End If
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
	
	Private Sub tx_仕入先CD_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_仕入先CD.Enter
		If Item_Check(([tx_仕入先CD].TabIndex)) = False Then
			Exit Sub
		End If
		'    Set PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_仕入先CD_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_仕入先CD.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		If KeyAscii = Asc(" ") And ([tx_仕入先CD].SelectionStart = 0 And [tx_仕入先CD].SelectionLength = Len([tx_仕入先CD].Text)) Then
			KeyAscii = 0
			'---参照画面表示
			If cSiiresaki.ShowDialog = True Then
				[tx_仕入先CD].Text = cSiiresaki.仕入先CD
				[tx_仕入先CD].Focus()
			Else
				[tx_仕入先CD].Focus()
			End If
		End If
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
	
	'2012/10/3 ADD↓
	Private Sub tx_見積日付Y_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_見積日付Y.Enter
		If Item_Check(([tx_見積日付Y].TabIndex)) = False Then
			Exit Sub
		End If
	End Sub
	
	Private Sub tx_見積日付Y_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_見積日付Y.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		If KeyAscii <> 8 Then ' バックスペースは例外
			'UPGRADE_WARNING: TextBox プロパティ tx_見積日付Y.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
			'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
			If LenB(StrConv([tx_見積日付Y].Text & Chr(KeyAscii), vbFromUnicode)) - LenB(StrConv([tx_見積日付Y].SelectedText, vbFromUnicode)) = [tx_見積日付Y].Maxlength Then
				'            ReturnF = True
				[tx_見積日付Y].Focus()
			End If
		End If
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
	
	Private Sub tx_見積日付M_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_見積日付M.Enter
		If Item_Check(([tx_見積日付M].TabIndex)) = False Then
			Exit Sub
		End If
	End Sub
	
	Private Sub tx_見積日付M_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_見積日付M.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		If KeyAscii <> 8 Then ' バックスペースは例外
			'UPGRADE_WARNING: TextBox プロパティ tx_見積日付M.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
			'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
			If LenB(StrConv([tx_見積日付M].Text & Chr(KeyAscii), vbFromUnicode)) - LenB(StrConv([tx_見積日付M].SelectedText, vbFromUnicode)) = [tx_見積日付M].Maxlength Then
				If KeyAscii <> 0 Then '入力キーがエラーでないならば
					'                ReturnF = True
					[tx_見積日付M].Focus()
				End If
			Else
				Select Case Chr(KeyAscii)
					Case CStr(2) To CStr(9)
						'                    ReturnF = True
						[tx_見積日付M].Focus()
				End Select
			End If
		End If
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
	
	Private Sub tx_見積日付D_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_見積日付D.Enter
		If Item_Check(([tx_見積日付D].TabIndex)) = False Then
			Exit Sub
		End If
	End Sub
	
	Private Sub tx_見積日付D_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_見積日付D.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		If KeyAscii <> 8 Then ' バックスペースは例外
			'UPGRADE_WARNING: TextBox プロパティ tx_見積日付D.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
			'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
			If LenB(StrConv([tx_見積日付D].Text & Chr(KeyAscii), vbFromUnicode)) - LenB(StrConv([tx_見積日付D].SelectedText, vbFromUnicode)) = [tx_見積日付D].Maxlength Then
				If KeyAscii <> 0 Then '入力キーがエラーでないならば
					'                ReturnF = True
					[tx_見積日付D].Focus()
				End If
			Else
				Select Case Chr(KeyAscii)
					Case CStr(4) To CStr(9)
						'                    ReturnF = True
						[tx_見積日付D].Focus()
				End Select
			End If
		End If
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
	'2012/10/3 ADD↑
	
	Private Sub cmdFind_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdFind.Enter
		If Item_Check(([cmdFind].TabIndex)) = False Then
			Exit Sub
		End If
	End Sub
	
	Private Sub cmdFind_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdFind.Click
		On Error GoTo cmdFind_Click_Err
		
		If Not Download Then
			CheckAlarm("該当データがありません。")
			[tx_得意先CD].Focus()
			Exit Sub
		End If
		
		'検索ＯＫ処理
		CLKFLG = True
		SelListVw.Focus()
		
		Exit Sub
cmdFind_Click_Err: 
		Me.Cursor = System.Windows.Forms.Cursors.Default
		MsgBox(Err.Number & " " & Err.Description)
		Exit Sub
	End Sub
End Class