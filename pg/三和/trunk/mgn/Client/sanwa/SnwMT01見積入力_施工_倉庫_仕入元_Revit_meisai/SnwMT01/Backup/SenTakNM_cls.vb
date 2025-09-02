Option Strict Off
Option Explicit On
Friend Class SentakNM_cls
	Inherits System.Windows.Forms.Form
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
	
	'プロパティ変数
	Dim psel_SQL As String
	Dim pSel_PKey As String
	Dim pSel_Caption As String
	Dim pFind_ID As String
	Dim pFind_Name As String
	Dim pFindBase_Name As String
	
	Dim FieldCnt As Short
	Dim FieldName() As Object
	Dim FieldSize() As Short
	Dim NameLen() As Short
	Dim List_ColWidth() As Object
	
	Dim sql As String
	Dim rs As ADODB.Recordset
	Dim ResultCodeSetControl As System.Windows.Forms.Control '選択したコードの送り先をセットする。
	Dim CLKFLG As Boolean 'リストビュー制御用
	Dim MeWidth, MeHeight As Integer
	Dim wkWidth As Short
	Dim WidthSa As Short
	Dim WkHeight As Integer
	Dim LvHeightLimit, MeHeightLimit As Integer
	
	Private m_DialogResult As Boolean
	Private m_DialogResultCode As Integer
	
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
	
	Private Sub SentakNM_cls_FormClosing(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
		Dim Cancel As Boolean = eventArgs.Cancel
		Dim UnloadMode As System.Windows.Forms.CloseReason = eventArgs.CloseReason
		'    Set ResultCodeSetControl = Nothing
		'UPGRADE_NOTE: オブジェクト SentakNM_cls をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		Me = Nothing
		eventArgs.Cancel = Cancel
	End Sub
	
	'UPGRADE_WARNING: イベント SentakNM_cls.Resize は、フォームが初期化されたときに発生します。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"' をクリックしてください。
	Private Sub SentakNM_cls_Resize(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Resize
		
		If Me.WindowState <> System.Windows.Forms.FormWindowState.Minimized Then
			'フォーム最小（幅）制御
			If VB6.PixelsToTwipsX(Me.Width) < MeWidth Then
				Me.Width = VB6.TwipsToPixelsX(MeWidth)
			End If
			'フォーム最小（高さ）制御・リストビューの高さをフォームの高さに比例
			If VB6.PixelsToTwipsY(Me.Height) < MeHeightLimit Then
				Me.Height = VB6.TwipsToPixelsY(MeHeightLimit)
			End If
			'リストビューの高さ・ボタン位置
			WkHeight = VB6.PixelsToTwipsY(SelListVw.Top) + VB6.PixelsToTwipsY(SelListVw.Height)
			WkHeight = VB6.PixelsToTwipsY(cmdOk.Top) - WkHeight
			SelListVw.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.ClientRectangle.Height) - VB6.PixelsToTwipsY(SelListVw.Top) - (WkHeight * 2) - VB6.PixelsToTwipsY(cmdOk.Height))
			cmdOk.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(SelListVw.Height) + VB6.PixelsToTwipsY(SelListVw.Top) + WkHeight)
			cmdCan.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(SelListVw.Height) + VB6.PixelsToTwipsY(SelListVw.Top) + WkHeight)
		End If
	End Sub
	
	Private Sub SentakNM_cls_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		Dim i, j As Short
		Dim List_ColLen As Short
		
		On Error GoTo Form_Load_Err
		
		'表示項目セット
		If InStr(UCase(psel_SQL), " WHERE ") <> 0 Then
			sql = psel_SQL & " and " & pSel_PKey & " is null"
		Else
			sql = psel_SQL & " WHERE " & pSel_PKey & " is null"
		End If
		
		rs = OpenRs(sql, Cn)
		
		FieldCnt = rs.Fields.Count - 1
		ReDim FieldName(FieldCnt)
		ReDim FieldSize(FieldCnt)
		ReDim NameLen(FieldCnt)
		
		For i = 0 To FieldCnt
			'UPGRADE_WARNING: オブジェクト FieldName(i) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			FieldName(i) = rs.Fields(i).Name
			FieldSize(i) = rs.Fields(i).DefinedSize
			NameLen(i) = AnsiLenB(rs.Fields(i).Name)
			'表示項目（幅）セット
			'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
			If IsDbNull(List_ColWidth(i)) Then
				If NameLen(i) > FieldSize(i) Then
					'前後の余裕で+200
					'UPGRADE_ISSUE: Form メソッド SentakNM_cls.TextWidth はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト List_ColWidth(i) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					List_ColWidth(i) = Me.TextWidth(New String(" ", NameLen(i))) + 200
				Else
					'UPGRADE_ISSUE: Form メソッド SentakNM_cls.TextWidth はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト List_ColWidth(i) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					List_ColWidth(i) = Me.TextWidth(New String(" ", FieldSize(i))) + 200
				End If
			End If
		Next 
		ReleaseRs(rs)
		
		'標題(固定)
		Me.Text = pSel_Caption
		'リストヘッダー項目セット
		With SelListVw
			.Columns.Clear()
			For i = 0 To FieldCnt
				.Columns.Add("", FieldName(i), CInt(VB6.TwipsToPixelsX(List_ColWidth(i))))
			Next 
		End With
		
		'リスト幅セット
		SelListVw.Width = 0
		For i = 0 To FieldCnt
			'UPGRADE_WARNING: オブジェクト List_ColWidth() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			SelListVw.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(SelListVw.Width) + List_ColWidth(i))
		Next 
		SelListVw.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(SelListVw.Width) + 330) 'スクロールバー分プラス
		
		'リストビューのヘッダを平面にする
		Call SetFlatHeader(SelListVw)
		
		wkWidth = (VB6.PixelsToTwipsX(Me.SelListVw.Left) * 2) + VB6.PixelsToTwipsX(SelListVw.Width) 'wkフォーム幅セット
		WidthSa = VB6.PixelsToTwipsX(Me.Width) - VB6.PixelsToTwipsX(Me.ClientRectangle.Width) 'フォームWidthの内外値差
		
		If wkWidth < 4940 Then
			wkWidth = 4940
		End If
		
		[cmdFind].Left = VB6.TwipsToPixelsX(wkWidth - (VB6.PixelsToTwipsX(SelListVw.Left) + VB6.PixelsToTwipsX([cmdFind].Width))) '検索ボタン位置決定
		cmdOk.Left = VB6.TwipsToPixelsX(wkWidth - (VB6.PixelsToTwipsX(SelListVw.Left) + VB6.PixelsToTwipsX(cmdOk.Width) + VB6.PixelsToTwipsX(cmdCan.Width))) 'ＯＫボタン位置決定
		cmdCan.Left = VB6.TwipsToPixelsX(wkWidth - (VB6.PixelsToTwipsX(SelListVw.Left) + VB6.PixelsToTwipsX(cmdCan.Width))) 'キャンセルボタン位置決定
		'    Me.ScaleWidth = wkWidth                                                 'フォーム幅決定(ScaleWidth)
		Me.Width = VB6.TwipsToPixelsX(wkWidth + WidthSa) 'フォーム幅決定(Width)
		
		'フォームを画面の中央に配置
		Me.Top = VB6.TwipsToPixelsY((VB6.PixelsToTwipsY(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height) - VB6.PixelsToTwipsY(Me.Height)) \ 2)
		Me.Left = VB6.TwipsToPixelsX((VB6.PixelsToTwipsX(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width) - VB6.PixelsToTwipsX(Me.Width)) \ 2)
		'項目クリア
		Call SetupBlank()
		
		MeWidth = VB6.PixelsToTwipsX(Me.Width)
		MeHeight = VB6.PixelsToTwipsY(Me.Height) - VB6.PixelsToTwipsY(SelListVw.Height)
		LvHeightLimit = SelListVw.Font.SizeInPoints * (34 + 24) '(列見出し + 明細一行分）
		MeHeightLimit = MeHeight + LvHeightLimit
		
		'    ResultCodeSetControl.Tag = ""
		
		Exit Sub
Form_Load_Err: 
		MsgBox(Err.Number & " " & Err.Description)
		Exit Sub
	End Sub
	
	Private Sub SentakNM_cls_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
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
	
	Private Sub SetupBlank()
		On Error GoTo SetupBlank_Err
		
		'各項目のクリア
		lb_検索ID.Text = pFind_ID
		'2015/06/11 ADD↓
		'UPGRADE_ISSUE: Form メソッド SentakNM_cls.TextWidth はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		If VB6.PixelsToTwipsX(lb_検索ID.Width) < Me.TextWidth(lb_検索ID.Text) Then
			'UPGRADE_ISSUE: Form メソッド SentakNM_cls.TextWidth はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
			lb_検索ID.Width = VB6.TwipsToPixelsX(Me.TextWidth(lb_検索ID.Text))
			[tx_検索ID].Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(lb_検索ID.Width) + VB6.PixelsToTwipsX(lb_検索ID.Left))
		End If
		'2015/06/11 ADD↑
		
		lb_検索名.Text = pFind_Name
		'2015/06/11 ADD↓
		'UPGRADE_ISSUE: Form メソッド SentakNM_cls.TextWidth はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		If VB6.PixelsToTwipsX(lb_検索名.Width) < Me.TextWidth(lb_検索名.Text) Then
			'UPGRADE_ISSUE: Form メソッド SentakNM_cls.TextWidth はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
			lb_検索名.Width = VB6.TwipsToPixelsX(Me.TextWidth(lb_検索名.Text))
			[tx_検索名].Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(lb_検索名.Width) + VB6.PixelsToTwipsX(lb_検索名.Left))
		End If
		'2015/06/11 ADD↑
		
		
		'    tx_検索ID.MaxLength = ResultCodeSetControl.MaxLength
		[tx_検索ID].Text = ""
		[tx_検索名].Text = ""
		SelListVw.Items.Clear()
		
		Exit Sub
SetupBlank_Err: 
		MsgBox(Err.Number & " " & Err.Description)
		Exit Sub
	End Sub
	
	Private Sub tx_検索名_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_検索名.Enter
		Item_Check(([tx_検索名].TabIndex))
	End Sub
	
	Private Sub cmdFind_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdFind.Enter
		Item_Check(([cmdFind].TabIndex))
	End Sub
	
	Private Sub cmdFind_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdFind.Click
		On Error GoTo cmdFind_Click_Err
		
		If Not Download Then
			CheckAlarm("該当データがありません。")
			lbListCount.Text = ""
			[tx_検索ID].Focus()
			Exit Sub
		End If
		
		'検索ＯＫ処理
		CLKFLG = True
		SelListVw.Focus()
		
		Exit Sub
cmdFind_Click_Err: 
		HourGlass(False)
		MsgBox(Err.Number & " " & Err.Description)
		Exit Sub
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOK.Click
		On Error GoTo cmdOk_Click_Err
		'--- リスト選択
		SetResultValues()
		
		Exit Sub
cmdOk_Click_Err: 
		MsgBox(Err.Number & " " & Err.Description)
		Exit Sub
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
			[cmdFind].Focus()
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
					[tx_検索名].Focus()
				End If
		End Select
	End Sub
	
	Private Sub SelListVw_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles SelListVw.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		Dim itmFound As System.Windows.Forms.ListViewItem ' FoundItem 変数。
		
		itmFound = SelListVw.FindItemWithText(Space(1) & Chr(KeyAscii), True, 0, True)
		KeyAscii = 0
		' EnsureVisible メソッドを使用して
		' コントロールをスクロールし、ListItem を選択します。
		If Not (itmFound Is Nothing) Then
			'UPGRADE_WARNING: MSComctlLib.ListItem メソッド itmFound.EnsureVisible には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
			itmFound.EnsureVisible() ' リスト ビュー コントロールをスクロールして、検出された ListItem を表示します。
			itmFound.Selected = True ' ListItem を選択します。
			' コントロールにフォーカスを返し、選択した内容を表示します。
			SelListVw.Focus()
		End If
		'UPGRADE_NOTE: オブジェクト itmFound をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		itmFound = Nothing
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
	
	Private Sub SetResultValues()
		If Not CLKFLG Then Exit Sub
		If SelListVw.Items.Count <> 0 Then
			If SelListVw.FocusedItem.Selected = False Then Exit Sub
			'''                If Not (ResultCodeSetControl Is Nothing) Then
			'''                    With ResultCodeSetControl
			'''                        .Text = Trim$(SelListVw.ListItems(SelListVw.SelectedItem.Index))
			'''                        .Tag = "True"
			'''                    End With
			'''                End If
			m_DialogResult = True
			'UPGRADE_WARNING: コレクション SelListVw.ListItems の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
			m_DialogResultCode = CInt(Trim(SelListVw.Items.Item(SelListVw.FocusedItem.Index).Text))
		End If
		Me.Close()
	End Sub
	
	Private Function Item_Check(ByRef ItemNo As Short) As Short
		
		On Error GoTo Item_Check_Err
		
		'キー項目「ID」のチェック
		If ItemNo > [tx_検索ID].TabIndex Then
			'--- 入力値をワークへ格納
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
		
		Download = False
		HourGlass(True)
		
		If IsCheckNull([tx_検索ID]) = False Or IsCheckNull([tx_検索名]) = False Then
			If IsCheckNull([tx_検索ID]) = False Then
				whr = pFind_ID & " >= " & [tx_検索ID].Text
				If IsCheckNull([tx_検索名]) = False Then
					whr = whr & " AND " & pFind_Name & " LIKE '%" & SQLString([tx_検索名]) & "%'"
				End If
			Else
				If IsCheckNull([tx_検索名]) = False Then
					If pFindBase_Name = vbNullString Then
						whr = pFind_Name & " LIKE '%" & SQLString([tx_検索名]) & "%'"
					Else
						whr = pFindBase_Name & " LIKE '%" & SQLString([tx_検索名]) & "%'"
					End If
				End If
			End If
		End If
		
		sql = psel_SQL
		
		If whr <> vbNullString Then
			If InStr(UCase(psel_SQL), " WHERE ") <> 0 Then
				sql = sql & " and " & whr
			Else
				sql = sql & " WHERE " & whr
			End If
		End If
		
		sql = sql & " ORDER BY " & pFind_ID
		
		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
		
		LockWindowUpdate(Me.Handle.ToInt32)
		
		SelListVw.Items.Clear()
		
		Do Until rs.EOF
			''''        Buf = String(ResultCodeSetControl.MaxLength, "0") & "0"      'ひとつ余計にする
			Buf = New String(" ", 8) & "0" 'ひとつ余計にする       'Long幅
			
			Select Case rs.Fields(0).Type
				Case ADODB.DataTypeEnum.adBSTR, ADODB.DataTypeEnum.adChar, ADODB.DataTypeEnum.adVarChar, ADODB.DataTypeEnum.adVarWChar, ADODB.DataTypeEnum.adWChar
					Buf = Space(1) & CStr(rs.Fields(0).Value)
				Case Else
					Buf = RSet(CStr(rs.Fields(0).Value), Len(Buf))
			End Select
			itmX = SelListVw.Items.Add(CStr(rs.Fields(0).Value) & " ID", Buf, "")
			For i = 1 To FieldCnt
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				itmX.SubItems.Add(CStr(NullToZero((rs.Fields(i).Value), "")))
			Next 
			rs.MoveNext()
		Loop 
		ReleaseRs(rs)
		'UPGRADE_NOTE: オブジェクト itmX をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		itmX = Nothing
		
		'UPGRADE_ISSUE: MSComctlLib.ListView プロパティ SelListVw.SortKey はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		SelListVw.SortKey = 0
		SelListVw.Sort()
		SelListVw.Refresh()
		
		'選択されているテキストを保持する
		FindListText = CStr([tx_検索ID].Text)
		itmFound = SelListVw.FindItemWithText(FindListText, True, 0, MSComctlLib.ListFindItemHowConstants.lvwWhole)
		
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
		
		'データ件数セット
		If SelListVw.Items.Count <> 0 Then
			lbListCount.Text = VB6.Format(SelListVw.Items.Count, "#,##0")
		Else
			Exit Function
		End If
		
		Download = True
	End Function
	''''
	'''''選択したコードを送るコントロールをセット
	''''Property Set ResCodeCTL(ByRef ctl As Control)
	''''    Set ResultCodeSetControl = ctl
	''''End Property
	''''
	'''''指定したPrimarryKeyをセット
	''''Property Let Sel_PKey(ByVal New_Sel_PKey As String)
	''''    pSel_PKey = New_Sel_PKey
	''''End Property
	''''
	'''''指定したSQLをセット
	''''Property Let Sel_SQL(ByVal New_Sel_SQL As String)
	''''    psel_SQL = New_Sel_SQL
	''''End Property
	''''
	'''''指定した標題をセット
	''''Property Let Sel_Caption(ByVal New_Sel_Caption As String)
	''''    pSel_Caption = New_Sel_Caption
	''''End Property
	''''
	'''''------------------------------------------
	'''''   選択画面セットアップ
	'''''       ctl         :選択コードを送るコントロールをセット
	'''''       sql         :リストセット用レコードソース
	'''''       FindID      :検索ID
	'''''       FindNM      :検索名
	'''''       FindBaseNM  :検索名をアリアス(AS)した時の基本となるフィールド名
	'''''       Pkey        :PrimaryKey
	'''''       Title       :選択画面のCaption
	'''''       FormatType  :Formatする(True)しない(False)
	'''''       FieldsWidth :sqlで指定した項目分のリスト幅をセット(NULLにすると自動)
	'''''------------------------------------------
	''''Public Function SelSetup(ctl As Control, sql As String, FindID As String, FindNM As String _
	'''''                        , FindBaseNM As String, Pkey As String, Title As String _
	'''''                        , ParamArray FieldsWidth())
	''''
	''''    Dim i As Integer
	''''
	''''    Set ResultCodeSetControl = ctl
	''''    psel_SQL = sql
	''''    pSel_PKey = Pkey
	''''    pSel_Caption = Title
	''''
	''''    pFind_ID = FindID
	''''    pFind_Name = FindNM
	''''    pFindBase_Name = FindBaseNM
	''''    ReDim List_ColWidth(0 To UBound(FieldsWidth))
	''''
	''''    For i = 0 To UBound(FieldsWidth)
	''''        List_ColWidth(i) = FieldsWidth(i)
	''''    Next
	''''
	''''End Function
	
	'UPGRADE_WARNING: ParamArray FieldsWidth が ByRef から ByVal に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="93C6A0DC-8C99-429A-8696-35FC4DCEFCCC"' をクリックしてください。
	Public Function SelSetup(ByRef sql As String, ByRef FindID As String, ByRef FindNM As String, ByRef FindBaseNM As String, ByRef Pkey As String, ByRef Title As String, ParamArray ByVal FieldsWidth() As Object) As Object
		
		Dim i As Short
		
		'''    Set ResultCodeSetControl = ctl
		psel_SQL = sql
		pSel_PKey = Pkey
		pSel_Caption = Title
		
		pFind_ID = FindID
		pFind_Name = FindNM
		pFindBase_Name = FindBaseNM
		ReDim List_ColWidth(UBound(FieldsWidth))
		
		For i = 0 To UBound(FieldsWidth)
			'UPGRADE_WARNING: オブジェクト FieldsWidth() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト List_ColWidth(i) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			List_ColWidth(i) = FieldsWidth(i)
		Next 
		
	End Function
	
	'UPGRADE_NOTE: DialogResult は DialogResult_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
	Public ReadOnly Property DialogResult_Renamed() As Boolean
		Get
			DialogResult_Renamed = m_DialogResult
		End Get
	End Property
	
	Public ReadOnly Property DialogResultCode() As Integer
		Get
			DialogResultCode = m_DialogResultCode
		End Get
	End Property
End Class