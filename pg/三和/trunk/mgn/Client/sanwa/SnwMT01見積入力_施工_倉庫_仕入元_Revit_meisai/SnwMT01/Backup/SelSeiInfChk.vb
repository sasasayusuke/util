Option Strict Off
Option Explicit On
Friend Class SelSeiInfChk
	Inherits System.Windows.Forms.Form
	'   2015/01/10  oosawa      廃盤FLG追加での修正
	'   2015/06/19  oosawa      仕様NOを範囲指定に変更
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
	
	Dim pParentForm As System.Windows.Forms.Form
	Dim pSelMode As Short
	Dim CLKFLG As Boolean 'リストビュー制御用
	Dim PreviousControl As System.Windows.Forms.Control 'コントロールの戻りを制御
	
	Dim sql As String
	Dim rs As ADODB.Recordset
	Dim ResultCodeSetControl1 As System.Windows.Forms.Control '選択したコードの送り先をセットする。
	Dim ResultCodeSetControl2 As System.Windows.Forms.Control '選択したコードの送り先をセットする。
	Dim ResultCodeSetControl3 As System.Windows.Forms.Control '選択したコードの送り先をセットする。
	Dim MeWidth, MeHeight As Integer
	Dim wkWidth As Short
	Dim WidthSa As Short
	Dim WkHeight As Integer
	Dim LvHeightLimit, MeHeightLimit As Integer
	
	'クラス
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
	
	Private Sub SelSeiInfChk_FormClosing(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
		Dim Cancel As Boolean = eventArgs.Cancel
		Dim UnloadMode As System.Windows.Forms.CloseReason = eventArgs.CloseReason
		'UPGRADE_NOTE: オブジェクト SelSeiInfChk をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		Me = Nothing
		eventArgs.Cancel = Cancel
	End Sub
	
	'UPGRADE_WARNING: イベント SelSeiInfChk.Resize は、フォームが初期化されたときに発生します。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"' をクリックしてください。
	Private Sub SelSeiInfChk_Resize(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Resize
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
			cmdChkOn.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(SelListVw.Height) + VB6.PixelsToTwipsY(SelListVw.Top) + WkHeight)
			cmdChkOff.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(SelListVw.Height) + VB6.PixelsToTwipsY(SelListVw.Top) + WkHeight)
		End If
	End Sub
	
	Private Sub SelSeiInfChk_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		Dim i As Short
		
		On Error GoTo Form_Load_Err
		
		'クラス生成
		cSiiresaki = New clsSiiresaki
		
		'リストヘッダー項目セット
		With SelListVw
			.Columns.Clear()
			.Columns.Add("", "", CInt(VB6.TwipsToPixelsX(270)))
			.Columns.Add("", "PC", CInt(VB6.TwipsToPixelsX(400)))
			.Columns.Add("", "製品NO", CInt(VB6.TwipsToPixelsX(1400)))
			.Columns.Add("", "仕様NO", CInt(VB6.TwipsToPixelsX(1200)))
			.Columns.Add("", "漢字名称", CInt(VB6.TwipsToPixelsX(4000)))
			.Columns.Add("", "Ｗ", CInt(VB6.TwipsToPixelsX(750)))
			.Columns.Add("", "Ｄ", CInt(VB6.TwipsToPixelsX(750)))
			.Columns.Add("", "Ｈ", CInt(VB6.TwipsToPixelsX(750)))
			.Columns.Add("", "D1/径", CInt(VB6.TwipsToPixelsX(750)))
			.Columns.Add("", "D2/Ｔ", CInt(VB6.TwipsToPixelsX(750)))
			.Columns.Add("", "H1", CInt(VB6.TwipsToPixelsX(750)))
			.Columns.Add("", "H2", CInt(VB6.TwipsToPixelsX(750)))
			.Columns.Add("", "仕入先名", CInt(VB6.TwipsToPixelsX(1450)))
			.Columns.Add("", "マスタ", CInt(VB6.TwipsToPixelsX(1000)))
		End With
		
		SelListVw.Width = VB6.TwipsToPixelsX(14970 + 330) 'スクロールバー分プラス
		
		'リストビューのヘッダを平面にする
		Call SetFlatHeader(SelListVw)
		
		wkWidth = (VB6.PixelsToTwipsX(Me.SelListVw.Left) * 2) + VB6.PixelsToTwipsX(SelListVw.Width) 'wkフォーム幅セット
		WidthSa = VB6.PixelsToTwipsX(Me.Width) - VB6.PixelsToTwipsX(Me.ClientRectangle.Width) 'フォームWidthの内外値差
		
		If wkWidth < 4940 Then
			wkWidth = 4940
		End If
		
		'桁セット-------------------
		'UPGRADE_WARNING: TextBox プロパティ tx_F1検索製品.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
		[tx_F1検索製品].Maxlength = SeiIDLength
		'UPGRADE_WARNING: TextBox プロパティ tx_F1検索仕様.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
		tx_F1検索仕様.Maxlength = ShiyoIDLength
		'UPGRADE_WARNING: TextBox プロパティ tx_F1検索仕様e.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
		tx_F1検索仕様e.Maxlength = ShiyoIDLength
		'------------------------------------
		
		[cmdFind].Left = VB6.TwipsToPixelsX(wkWidth - (VB6.PixelsToTwipsX(SelListVw.Left) + VB6.PixelsToTwipsX([cmdFind].Width))) '検索ボタン位置決定
		cmdOk.Left = VB6.TwipsToPixelsX(wkWidth - (VB6.PixelsToTwipsX(SelListVw.Left) + VB6.PixelsToTwipsX(cmdOk.Width) + VB6.PixelsToTwipsX(cmdCan.Width))) 'ＯＫボタン位置決定
		cmdCan.Left = VB6.TwipsToPixelsX(wkWidth - (VB6.PixelsToTwipsX(SelListVw.Left) + VB6.PixelsToTwipsX(cmdCan.Width))) 'キャンセルボタン位置決定
		cmdChkOn.Left = SelListVw.Left 'ＡＬＬ選択ボタン位置決定
		cmdChkOff.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(SelListVw.Left) + VB6.PixelsToTwipsX(cmdChkOn.Width)) 'ＡＬＬ解除ボタン位置決定
		Me.Width = VB6.TwipsToPixelsX(wkWidth + WidthSa) 'フォーム幅決定(Width)
		
		'フォームを画面の中央に配置
		Me.Top = VB6.TwipsToPixelsY((VB6.PixelsToTwipsY(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height) - VB6.PixelsToTwipsY(Me.Height)) \ 2)
		Me.Left = VB6.TwipsToPixelsX((VB6.PixelsToTwipsX(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width) - VB6.PixelsToTwipsX(Me.Width)) \ 2)
		'項目クリア
		Call SetupBlank()
		
		'位置重ね
		With PicFind
			For i = 0 To .UBound
				.Item(i).Top = VB6.TwipsToPixelsY(1020)
				.Item(i).Left = VB6.TwipsToPixelsX(180)
			Next 
		End With
		
		MeWidth = VB6.PixelsToTwipsX(Me.Width)
		MeHeight = VB6.PixelsToTwipsY(Me.Height) - VB6.PixelsToTwipsY(SelListVw.Height)
		LvHeightLimit = SelListVw.Font.SizeInPoints * (34 + 24) '(列見出し + 明細一行分）
		MeHeightLimit = MeHeight + LvHeightLimit
		
		'検索対象のロック
		If pSelMode = 1 Then
			ck_Find(2).Enabled = False
			ck_Find(3).Enabled = False
			lb_Find(2).Enabled = False
			lb_Find(3).Enabled = False
		Else
			ck_Find(2).Enabled = True
			ck_Find(3).Enabled = True
			lb_Find(2).Enabled = True
			lb_Find(3).Enabled = True
		End If
		'検索対象セット(既定値：製品)
		ck_Find(0).CheckState = System.Windows.Forms.CheckState.Checked
		
		Exit Sub
Form_Load_Err: 
		MsgBox(Err.Number & " " & Err.Description)
		Exit Sub
	End Sub
	
	Private Sub SelSeiInfChk_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
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
		Dim ctl As System.Windows.Forms.Control
		
		On Error Resume Next
		For	Each ctl In Me.Controls
			'UPGRADE_WARNING: TypeOf に新しい動作が指定されています。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
			If TypeOf ctl Is System.Windows.Forms.TextBox Then
				ctl.Text = vbNullString
			End If
			'UPGRADE_WARNING: TypeOf に新しい動作が指定されています。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
			If TypeOf ctl Is AxExText.AxExTextBox Then
				ctl.Text = vbNullString
			End If
			'UPGRADE_WARNING: TypeOf に新しい動作が指定されています。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
			If TypeOf ctl Is AxExNmText.AxExNmTextBox Then
				ctl.Text = vbNullString
			End If
			'UPGRADE_WARNING: TypeOf に新しい動作が指定されています。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
			If TypeOf ctl Is System.Windows.Forms.Label Then
				If ctl.Name Like "rf_*" Then
					ctl.Text = vbNullString
				End If
			End If
		Next ctl
		'UPGRADE_NOTE: オブジェクト ctl をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		ctl = Nothing
		SelListVw.Items.Clear()
		
		On Error GoTo 0
		Exit Sub
SetupBlank_Err: 
		MsgBox(Err.Number & " " & Err.Description)
		Exit Sub
	End Sub
	
	'UPGRADE_WARNING: イベント ck_Find.CheckStateChanged は、フォームが初期化されたときに発生します。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"' をクリックしてください。
	Private Sub ck_Find_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ck_Find.CheckStateChanged
		Dim Index As Short = ck_Find.GetIndex(eventSender)
		If SelListVw.Items.Count <> 0 Then
			SelListVw.Items.Clear()
			lbListCount.Text = ""
		End If
		
		If ck_Find(Index).CheckState = 1 Then
			PicFind(Index).BringToFront()
			Call PicLock(Index)
		Else
			PreviousControl.Focus()
		End If
	End Sub
	
	Private Sub lb_Find_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles lb_Find.Click
		Dim Index As Short = lb_Find.GetIndex(eventSender)
		PicFind(Index).BringToFront()
		Call PicLock(Index)
	End Sub
	
	'UPGRADE_ISSUE: PictureBox イベント PicFind.GotFocus はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="ABD9AF39-7E24-4AFF-AD8D-3675C1AA3054"' をクリックしてください。
	Private Sub PicFind_GotFocus(ByRef Index As Short)
		Call CtlFocus(Index)
	End Sub
	
	Private Sub SSTab1_DblClick()
		
	End Sub
	
	Private Sub tx_F1検索製品_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_F1検索製品.Enter
		Item_Check(([tx_F1検索製品].TabIndex))
		PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_F1検索仕様_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_F1検索仕様.Enter
		Item_Check((tx_F1検索仕様.TabIndex))
		PreviousControl = Me.ActiveControl
	End Sub
	
	'2015/06/19 ADD↓
	Private Sub tx_F1検索仕様e_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_F1検索仕様e.Enter
		Item_Check((tx_F1検索仕様e.TabIndex))
		PreviousControl = Me.ActiveControl
	End Sub
	'2015/06/19 ADD↑
	
	Private Sub tx_F1検索名称_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_F1検索名称.Enter
		Item_Check((tx_F1検索名称.TabIndex))
		PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_F1検索仕入先_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_F1検索仕入先.Enter
		Item_Check(([tx_F1検索仕入先].TabIndex))
		PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_F1検索仕入先_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_F1検索仕入先.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		If KeyAscii = Asc(" ") And ([tx_F1検索仕入先].SelectionStart = 0 And [tx_F1検索仕入先].SelectionLength = Len([tx_F1検索仕入先].Text)) Then
			KeyAscii = 0
			'---参照画面表示
			'        SelectF = True
			If cSiiresaki.ShowDialog = True Then
				[tx_F1検索仕入先].Text = cSiiresaki.仕入先CD
				'            ReturnF = True
				[tx_F1検索仕入先].Focus()
			Else
				[tx_F1検索仕入先].Focus()
			End If
			'''        KeyAscii = 0
			'''        '---参照画面表示
			'''        Set SirSen.ResCodeCTL = tx_F1検索仕入先
			'''        SirSen.Show vbModal, Me
			'''        If [tx_F1検索仕入先].Tag <> "" Then
			'''            [tx_F1検索仕入先].SetFocus
			'''        Else
			'''            [tx_F1検索仕入先].SetFocus
			'''        End If
		End If
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
	
	Private Sub tx_F1検索W_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_F1検索W.Enter
		Item_Check((tx_F1検索W.TabIndex))
		PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_F1検索D_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_F1検索D.Enter
		Item_Check((tx_F1検索D.TabIndex))
		PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_F1検索H_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_F1検索H.Enter
		Item_Check((tx_F1検索H.TabIndex))
		PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_F1検索D1_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_F1検索D1.Enter
		Item_Check((tx_F1検索D1.TabIndex))
		PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_F1検索D2_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_F1検索D2.Enter
		Item_Check((tx_F1検索D2.TabIndex))
		PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_F1検索H1_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_F1検索H1.Enter
		Item_Check((tx_F1検索H1.TabIndex))
		PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_F1検索H2_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_F1検索H2.Enter
		Item_Check(([tx_F1検索H2].TabIndex))
		PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_F2検索品群_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_F2検索品群.Enter
		Item_Check(([tx_F2検索品群].TabIndex))
		PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_F2検索名称_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_F2検索名称.Enter
		Item_Check(([tx_F2検索名称].TabIndex))
		PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_F3検索ユニット_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_F3検索ユニット.Enter
		Item_Check(([tx_F3検索ユニット].TabIndex))
		PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_F3検索名称_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_F3検索名称.Enter
		Item_Check(([tx_F3検索名称].TabIndex))
		PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_F4検索PC区分_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_F4検索PC区分.Enter
		Item_Check(([tx_F4検索PC区分].TabIndex))
		PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_F4検索製品_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_F4検索製品.Enter
		Item_Check((tx_F4検索製品.TabIndex))
		PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_F4検索名称_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_F4検索名称.Enter
		Item_Check((tx_F4検索名称.TabIndex))
		PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_F4検索仕入先_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_F4検索仕入先.Enter
		Item_Check(([tx_F4検索仕入先].TabIndex))
		PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_F4検索仕入先_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_F4検索仕入先.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		If KeyAscii = Asc(" ") And ([tx_F4検索仕入先].SelectionStart = 0 And [tx_F4検索仕入先].SelectionLength = Len([tx_F4検索仕入先].Text)) Then
			KeyAscii = 0
			'---参照画面表示
			'        SelectF = True
			If cSiiresaki.ShowDialog = True Then
				[tx_F4検索仕入先].Text = cSiiresaki.仕入先CD
				'            ReturnF = True
				[tx_F4検索仕入先].Focus()
			Else
				[tx_F4検索仕入先].Focus()
			End If
			'''        KeyAscii = 0
			'''        '---参照画面表示
			'''        Set SirSen.ResCodeCTL = tx_F4検索仕入先
			'''        SirSen.Show vbModal, Me
			'''        If [tx_F4検索仕入先].Tag <> "" Then
			'''            [tx_F4検索仕入先].SetFocus
			'''        Else
			'''            [tx_F4検索仕入先].SetFocus
			'''        End If
		End If
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
	
	Private Sub tx_F4検索W_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_F4検索W.Enter
		Item_Check((tx_F4検索W.TabIndex))
		PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_F4検索D_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_F4検索D.Enter
		Item_Check((tx_F4検索D.TabIndex))
		PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_F4検索H_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_F4検索H.Enter
		Item_Check((tx_F4検索H.TabIndex))
		PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_F4検索径_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_F4検索径.Enter
		Item_Check((tx_F4検索径.TabIndex))
		PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_F4検索T_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_F4検索T.Enter
		Item_Check(([tx_F4検索T].TabIndex))
		PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub cmdFind_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdFind.Enter
		Item_Check(([cmdFind].TabIndex))
	End Sub
	
	Private Sub cmdFind_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdFind.Click
		On Error GoTo cmdFind_Click_Err
		
		'検索対象のﾁｪｯｸ
		If ck_Find(0).CheckState = 0 And ck_Find(1).CheckState = 0 And ck_Find(2).CheckState = 0 And ck_Find(3).CheckState = 0 Then
			CriticalAlarm("検索対象を選択して下さい。")
			PreviousControl.Focus()
			Exit Sub
		End If
		
		If Not Download Then
			CheckAlarm("該当データがありません。")
			lbListCount.Text = ""
			PreviousControl.Focus()
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
	
	Private Sub cmdChkOn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdChkOn.Click
		Dim i As Short
		
		If SelListVw.Items.Count = 0 Then
			Inform("対象データがありません。")
			Exit Sub
		End If
		
		For i = 1 To SelListVw.Items.Count
			'UPGRADE_WARNING: コレクション SelListVw.ListItems の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
			SelListVw.Items.Item(i).Checked = True
		Next 
	End Sub
	
	Private Sub cmdChkOff_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdChkOff.Click
		Dim i As Short
		
		If SelListVw.Items.Count = 0 Then
			Exit Sub
		End If
		
		For i = 1 To SelListVw.Items.Count
			'UPGRADE_WARNING: コレクション SelListVw.ListItems の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
			SelListVw.Items.Item(i).Checked = False
		Next 
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOK.Click
		Dim ResData As Object
		Dim i, j As Integer
		
		On Error GoTo cmdOk_Click_Err
		'--- リスト選択
		For i = 1 To SelListVw.Items.Count
			'UPGRADE_WARNING: コレクション SelListVw.ListItems の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
			If SelListVw.Items.Item(i).Checked = True Then
				j = j + 1
			End If
		Next 
		
		If j = 0 Then
			Inform("対象データがありません。")
			Exit Sub
		End If
		
		ReDim ResData(j - 1, 3 - 1)
		j = 0
		
		For i = 1 To SelListVw.Items.Count
			'UPGRADE_WARNING: コレクション SelListVw.ListItems の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
			If SelListVw.Items.Item(i).Checked = True Then
				'UPGRADE_WARNING: コレクション SelListVw.ListItems の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
				'UPGRADE_WARNING: コレクション SelListVw.ListItems.Item().ListSubItems の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト ResData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				ResData(j, 0) = Trim(SelListVw.Items.Item(i).SubItems.Item(1).Text)
				'UPGRADE_WARNING: コレクション SelListVw.ListItems の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
				'UPGRADE_WARNING: コレクション SelListVw.ListItems.Item().ListSubItems の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト ResData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				ResData(j, 1) = Trim(SelListVw.Items.Item(i).SubItems.Item(2).Text)
				'UPGRADE_WARNING: コレクション SelListVw.ListItems の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
				'UPGRADE_WARNING: コレクション SelListVw.ListItems.Item().ListSubItems の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト ResData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				ResData(j, 2) = Trim(SelListVw.Items.Item(i).SubItems.Item(3).Text)
				j = j + 1
			End If
		Next 
		'呼び元フォームに選択データをセット（注：変数は必須  Public SelData As Variant ）
		'UPGRADE_ISSUE: Control SelData は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト ResData の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		pParentForm.SelData = ResData
		
		Me.Close()
		
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
	
	Private Sub SelListVw_KeyDown(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyEventArgs) Handles SelListVw.KeyDown
		Dim KeyCode As Short = eventArgs.KeyCode
		Dim Shift As Short = eventArgs.KeyData \ &H10000
		Select Case KeyCode
			''        Case vbKeyReturn
			''            KeyCode = 0
			''            CLKFLG = True
			''            SetResultValues
			Case System.Windows.Forms.Keys.Up
				If SelListVw.FocusedItem.Index = 1 Then
					KeyCode = 0
					PreviousControl.Focus()
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
	
	Private Function Item_Check(ByRef ItemNo As Short) As Boolean
		
		On Error GoTo Item_Check_Err
		Item_Check = False
		
		'検索仕入先のチェック
		If ItemNo > [tx_F1検索仕入先].TabIndex Then
			'--- 入力値をワークへ格納
			If ISInt(([tx_F1検索仕入先].Text)) Then
				'UPGRADE_WARNING: TextBox プロパティ tx_F1検索仕入先.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				[tx_F1検索仕入先].Text = VB6.Format([tx_F1検索仕入先].Text, New String("0", [tx_F1検索仕入先].Maxlength))
			End If
		End If
		
		'検索仕入先のチェック
		If ItemNo > [tx_F4検索仕入先].TabIndex Then
			'--- 入力値をワークへ格納
			If ISInt(([tx_F4検索仕入先].Text)) Then
				'UPGRADE_WARNING: TextBox プロパティ tx_F4検索仕入先.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				[tx_F4検索仕入先].Text = VB6.Format([tx_F4検索仕入先].Text, New String("0", [tx_F4検索仕入先].Maxlength))
			End If
		End If
		
		Item_Check = True
		
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
		Dim whrbuf As String
		
		Download = False
		HourGlass(True)
		
		sql = "SELECT SJ.PC区分, SJ.製品NO, SJ.仕様NO, " & "名称=(COALESCE(SE.漢字名称,HI.品群名称,UN.ユニット名,PC.漢字名称)), " & " COALESCE (SE.W, PC.W) AS W, " & " COALESCE (SE.D, PC.D) AS D, " & " COALESCE (SE.H, PC.H) AS H, " & " COALESCE (SE.D1,PC.径) AS D1, " & " COALESCE (SE.D2,PC.T) AS D2, " & " SE.H1 AS H1, " & " SE.H2 AS H2, " & "仕入先名=(COALESCE(SI.略称,PI.略称)), " & "マスタ=(CASE WHEN SJ.製品区分 = 0 THEN '製品' WHEN SJ.製品区分 = 1 THEN '品群' WHEN SJ.製品区分 = 2 THEN 'ユニット' WHEN SJ.製品区分 = 3 THEN 'ＰＣ' END) " & "FROM TM製品情報 AS SJ " & "LEFT JOIN TM製品 AS SE " & "ON SJ.製品区分 = 0 AND SJ.製品NO = SE.製品NO AND SJ.仕様NO = SE.仕様NO " & "LEFT JOIN TM仕入先 AS SI " & "ON SE.主仕入先CD = SI.仕入先CD " & "LEFT JOIN TM品群 AS HI " & "ON SJ.製品区分 = 1 AND SJ.製品NO = HI.品群NO " & "LEFT JOIN TMユニット AS UN " & "ON SJ.製品区分 = 2 AND SJ.製品NO = UN.ユニットNO " & "LEFT JOIN TMPC AS PC " & "ON SJ.製品区分 = 3 AND SJ.PC区分 = PC.PC区分 AND SJ.製品NO = PC.製品NO " & "LEFT JOIN TM仕入先 AS PI " & "ON PC.主仕入先CD = PI.仕入先CD "
		
		'製品抽出条件セット
		'''    whr = whr & "(" & ck_Find(0).Value & "=1 AND SJ.製品区分 = 0"
		'廃盤は出さないように変更
		whr = whr & "(" & ck_Find(0).CheckState & "=1 AND SJ.製品区分 = 0 AND SE.廃盤FLG = 0" '2015/01/10 ADD
		whrbuf = SetWhereSEIH
		If whrbuf <> vbNullString Then
			If whr <> "" Then
				whr = whr & " AND "
			End If
		End If
		whr = whr & whrbuf
		whr = whr & ")"
		
		'品群抽出条件セット
		If whr <> "" Then
			whr = whr & " OR "
		End If
		whr = whr & "(" & ck_Find(1).CheckState & "=1 AND SJ.製品区分 = 1"
		whrbuf = SetWhereHING
		If whrbuf <> vbNullString Then
			If whr <> "" Then
				whr = whr & " AND "
			End If
		End If
		whr = whr & whrbuf
		whr = whr & ")"
		
		'ユニット抽出条件セット
		If whr <> "" Then
			whr = whr & " OR "
		End If
		whr = whr & "(" & ck_Find(2).CheckState & "=1 AND SJ.製品区分 = 2"
		whrbuf = SetWhereUNIT
		If whrbuf <> vbNullString Then
			If whr <> "" Then
				whr = whr & " AND "
			End If
		End If
		whr = whr & whrbuf
		whr = whr & ")"
		
		'ＰＣ抽出条件セット
		If whr <> "" Then
			whr = whr & " OR "
		End If
		whr = whr & "(" & ck_Find(3).CheckState & "=1 AND SJ.製品区分 = 3"
		whrbuf = SetWherePC
		If whrbuf <> vbNullString Then
			If whr <> "" Then
				whr = whr & " AND "
			End If
		End If
		whr = whr & whrbuf
		whr = whr & ")"
		
		If whr <> vbNullString Then
			sql = sql & " WHERE " & whr
		End If
		
		sql = sql & " ORDER BY SJ.PC区分, SJ.製品NO, SJ.仕様NO"
		
		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
		
		LockWindowUpdate(Me.Handle.ToInt32)
		
		SelListVw.Items.Clear()
		
		Do Until rs.EOF
			Buf = Space(1) & CStr(rs.Fields(0).Value)
			itmX = SelListVw.Items.Add(CStr(rs.Fields(0).Value) & CStr(rs.Fields(1).Value) & CStr(rs.Fields(2).Value) & " No", Buf, "")
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(CStr(NullToZero(Buf, "")))
			Buf = New String("0", 7) & "0" 'ひとつ余計にする
			If ISInt((rs.Fields(0).Value)) Then
				Buf = RSet(CStr(rs.Fields(1).Value), Len(Buf))
			Else
				Buf = Space(1) & CStr(rs.Fields(1).Value)
			End If
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(CStr(NullToZero(Buf, "")))
			Buf = New String("0", 7) & "0" 'ひとつ余計にする
			If ISInt((rs.Fields(2).Value)) Then
				Buf = RSet(CStr(rs.Fields(2).Value), Len(Buf))
			Else
				Buf = Space(1) & CStr(rs.Fields(2).Value)
			End If
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(CStr(NullToZero(Buf, "")))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(CStr(NullToZero((rs.Fields(3).Value), "")))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(VB6.Format(NullToZero((rs.Fields(4).Value), ""), "#"))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(VB6.Format(NullToZero((rs.Fields(5).Value), ""), "#"))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(VB6.Format(NullToZero((rs.Fields(6).Value), ""), "#"))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(VB6.Format(NullToZero((rs.Fields(7).Value), ""), "#"))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(VB6.Format(NullToZero((rs.Fields(8).Value), ""), "#"))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(VB6.Format(NullToZero((rs.Fields(9).Value), ""), "#"))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(VB6.Format(NullToZero((rs.Fields(10).Value), ""), "#"))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(CStr(NullToZero((rs.Fields(11).Value), "")))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(CStr(NullToZero((rs.Fields(12).Value), "")))
			rs.MoveNext()
		Loop 
		ReleaseRs(rs)
		'UPGRADE_NOTE: オブジェクト itmX をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		itmX = Nothing
		
		'UPGRADE_ISSUE: MSComctlLib.ListView プロパティ SelListVw.SortKey はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		SelListVw.SortKey = 0
		SelListVw.Sort()
		SelListVw.Refresh()
		
		'''''    '選択されているテキストを保持する
		'''''    FindListText = Space$(1) & CStr(tx_F1検索製品)
		'''''    Set itmFound = SelListVw.FindItem(FindListText, lvwText, , lvwWhole)
		'''''
		'''''    ' EnsureVisible メソッドを使用して
		'''''    ' コントロールをスクロールし、ListItem を選択します。
		'''''    If Not (itmFound Is Nothing) Then
		'''''        itmFound.EnsureVisible ' リスト ビュー コントロールをスクロールして、検出された ListItem を表示します。
		'''''        itmFound.Selected = True   ' ListItem を選択します。
		'''''    End If
		'''''    Set itmFound = Nothing
		
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
	
	'---選択モードをセット
	'--- 0:すべて
	'--- 1:製品・品群のみ検索
	WriteOnly Property SelMode() As Short
		Set(ByVal Value As Short)
			pSelMode = Value
		End Set
	End Property
	
	'選択したコードを送るコントロールをセット
	WriteOnly Property ResParentForm() As System.Windows.Forms.Form
		Set(ByVal Value As System.Windows.Forms.Form)
			'仕様NO
			pParentForm = Value
		End Set
	End Property
	
	Private Function SetWhereSEIH() As String
		SetWhereSEIH = ""
		'製品検索
		If [tx_F1検索製品].Text <> vbNullString Then
			If SetWhereSEIH <> "" Then
				SetWhereSEIH = SetWhereSEIH & " AND "
			End If
			SetWhereSEIH = SetWhereSEIH & " SJ.製品NO Like '" & SQLString([tx_F1検索製品]) & "%'"
		End If
		'2015/06/19 DEL↓
		'''    If tx_F1検索仕様 <> vbNullString Then
		'''        If SetWhereSEIH <> "" Then
		'''            SetWhereSEIH = SetWhereSEIH & " AND "
		'''        End If
		'''        SetWhereSEIH = SetWhereSEIH & " SJ.仕様NO Like '" & SQLString(tx_F1検索仕様) & "%'"
		'''    End If
		'2015/06/19 DEL↑
		'2015/06/19 ADD↓
		'仕様検索
		If IsCheckText(tx_F1検索仕様) = True Or IsCheckText(tx_F1検索仕様e) = True Then
			If SetWhereSEIH <> "" Then
				SetWhereSEIH = SetWhereSEIH & " and "
			End If
			SetWhereSEIH = SetWhereSEIH & SQLStringRange("SJ.仕様NO", Trim(tx_F1検索仕様.Text), Trim(tx_F1検索仕様e.Text),  , False)
		End If
		'2015/06/19 ADD↑
		
		If tx_F1検索名称.Text <> vbNullString Then
			If SetWhereSEIH <> "" Then
				SetWhereSEIH = SetWhereSEIH & " AND "
			End If
			SetWhereSEIH = SetWhereSEIH & " SE.漢字名称 LIKE '%" & SQLString(tx_F1検索名称) & "%'"
		End If
		If [tx_F1検索仕入先].Text <> vbNullString Then
			If SetWhereSEIH <> "" Then
				SetWhereSEIH = SetWhereSEIH & " AND "
			End If
			SetWhereSEIH = SetWhereSEIH & " SE.主仕入先CD = " & SQLQuoteString([tx_F1検索仕入先])
		End If
		
		If tx_F1検索W.Text <> vbNullString Then
			If SetWhereSEIH <> "" Then
				SetWhereSEIH = SetWhereSEIH & " and "
			End If
			SetWhereSEIH = SetWhereSEIH & " SE.W = " & tx_F1検索W.Text
		End If
		If tx_F1検索D.Text <> vbNullString Then
			If SetWhereSEIH <> "" Then
				SetWhereSEIH = SetWhereSEIH & " and "
			End If
			SetWhereSEIH = SetWhereSEIH & " SE.D = " & tx_F1検索D.Text
		End If
		If tx_F1検索H.Text <> vbNullString Then
			If SetWhereSEIH <> "" Then
				SetWhereSEIH = SetWhereSEIH & " and "
			End If
			SetWhereSEIH = SetWhereSEIH & " SE.H = " & tx_F1検索H.Text
		End If
		If tx_F1検索D1.Text <> vbNullString Then
			If SetWhereSEIH <> "" Then
				SetWhereSEIH = SetWhereSEIH & " and "
			End If
			SetWhereSEIH = SetWhereSEIH & " SE.D1 = " & tx_F1検索D1.Text
		End If
		If tx_F1検索D2.Text <> vbNullString Then
			If SetWhereSEIH <> "" Then
				SetWhereSEIH = SetWhereSEIH & " and "
			End If
			SetWhereSEIH = SetWhereSEIH & " SE.D2 = " & tx_F1検索D2.Text
		End If
		If tx_F1検索H1.Text <> vbNullString Then
			If SetWhereSEIH <> "" Then
				SetWhereSEIH = SetWhereSEIH & " and "
			End If
			SetWhereSEIH = SetWhereSEIH & " SE.H1 = " & tx_F1検索H1.Text
		End If
		If [tx_F1検索H2].Text <> vbNullString Then
			If SetWhereSEIH <> "" Then
				SetWhereSEIH = SetWhereSEIH & " and "
			End If
			SetWhereSEIH = SetWhereSEIH & " SE.H2 = " & [tx_F1検索H2].Text
		End If
	End Function
	
	Private Function SetWhereHING() As String
		SetWhereHING = ""
		'品群検索
		If [tx_F2検索品群].Text <> vbNullString Then
			If SetWhereHING <> "" Then
				SetWhereHING = SetWhereHING & " AND "
			End If
			SetWhereHING = SetWhereHING & " SJ.製品NO LIKE '" & SQLString([tx_F2検索品群]) & "%'"
		End If
		If [tx_F2検索名称].Text <> vbNullString Then
			If SetWhereHING <> "" Then
				SetWhereHING = SetWhereHING & " AND "
			End If
			SetWhereHING = SetWhereHING & " 品群名称 LIKE '%" & SQLString([tx_F2検索名称]) & "%'"
		End If
	End Function
	
	Private Function SetWhereUNIT() As String
		SetWhereUNIT = ""
		'ユニット検索
		If [tx_F3検索ユニット].Text <> vbNullString Then
			If SetWhereUNIT <> "" Then
				SetWhereUNIT = SetWhereUNIT & " AND "
			End If
			SetWhereUNIT = SetWhereUNIT & " SJ.製品NO LIKE '" & SQLString([tx_F3検索ユニット]) & "%'"
		End If
		If [tx_F3検索名称].Text <> vbNullString Then
			If SetWhereUNIT <> "" Then
				SetWhereUNIT = SetWhereUNIT & " AND "
			End If
			SetWhereUNIT = SetWhereUNIT & " ユニット名 LIKE '%" & SQLString([tx_F3検索名称]) & "%'"
		End If
	End Function
	
	Private Function SetWherePC() As String
		SetWherePC = ""
		'ＰＣ検索
		If [tx_F4検索PC区分].Text <> vbNullString Then
			If SetWherePC <> "" Then
				SetWherePC = SetWherePC & " AND "
			End If
			SetWherePC = SetWherePC & " SJ.PC区分 = " & SQLQuoteString([tx_F4検索PC区分])
		End If
		If tx_F4検索製品.Text <> vbNullString Then
			If SetWherePC <> "" Then
				SetWherePC = SetWherePC & " AND "
			End If
			SetWherePC = SetWherePC & " SJ.製品NO Like '" & SQLString(tx_F4検索製品) & "%'"
		End If
		If tx_F4検索名称.Text <> vbNullString Then
			If SetWherePC <> "" Then
				SetWherePC = SetWherePC & " AND "
			End If
			SetWherePC = SetWherePC & " PC.漢字名称 LIKE '%" & SQLString(tx_F4検索名称) & "%'"
		End If
		If [tx_F4検索仕入先].Text <> vbNullString Then
			If SetWherePC <> "" Then
				SetWherePC = SetWherePC & " AND "
			End If
			SetWherePC = SetWherePC & " PC.主仕入先CD = " & SQLQuoteString([tx_F4検索仕入先])
		End If
		
		If tx_F4検索W.Text <> vbNullString Then
			If SetWherePC <> "" Then
				SetWherePC = SetWherePC & " and "
			End If
			SetWherePC = SetWherePC & " PC.W = " & tx_F4検索W.Text
		End If
		If tx_F4検索D.Text <> vbNullString Then
			If SetWherePC <> "" Then
				SetWherePC = SetWherePC & " and "
			End If
			SetWherePC = SetWherePC & " PC.D = " & tx_F4検索D.Text
		End If
		If tx_F4検索H.Text <> vbNullString Then
			If SetWherePC <> "" Then
				SetWherePC = SetWherePC & " and "
			End If
			SetWherePC = SetWherePC & " PC.H = " & tx_F4検索H.Text
		End If
		If tx_F4検索径.Text <> vbNullString Then
			If SetWherePC <> "" Then
				SetWherePC = SetWherePC & " and "
			End If
			SetWherePC = SetWherePC & " PC.径 = " & tx_F4検索径.Text
		End If
		If [tx_F4検索T].Text <> vbNullString Then
			If SetWherePC <> "" Then
				SetWherePC = SetWherePC & " and "
			End If
			SetWherePC = SetWherePC & " PC.T = " & [tx_F4検索T].Text
		End If
	End Function
	
	Private Sub PicLock(ByRef Index As Short)
		On Error Resume Next
		Select Case Index
			Case 0
				PicFind(0).Enabled = True
				PicFind(1).Enabled = False
				PicFind(2).Enabled = False
				PicFind(3).Enabled = False
				lb_Find(0).Font = VB6.FontChangeBold(lb_Find(0).Font, True)
				lb_Find(1).Font = VB6.FontChangeBold(lb_Find(1).Font, False)
				lb_Find(2).Font = VB6.FontChangeBold(lb_Find(2).Font, False)
				lb_Find(3).Font = VB6.FontChangeBold(lb_Find(3).Font, False)
				[tx_F1検索製品].Focus()
			Case 1
				PicFind(0).Enabled = False
				PicFind(1).Enabled = True
				PicFind(2).Enabled = False
				PicFind(3).Enabled = False
				lb_Find(0).Font = VB6.FontChangeBold(lb_Find(0).Font, False)
				lb_Find(1).Font = VB6.FontChangeBold(lb_Find(1).Font, True)
				lb_Find(2).Font = VB6.FontChangeBold(lb_Find(2).Font, False)
				lb_Find(3).Font = VB6.FontChangeBold(lb_Find(3).Font, False)
				[tx_F2検索品群].Focus()
			Case 2
				PicFind(0).Enabled = False
				PicFind(1).Enabled = False
				PicFind(2).Enabled = True
				PicFind(3).Enabled = False
				lb_Find(0).Font = VB6.FontChangeBold(lb_Find(0).Font, False)
				lb_Find(1).Font = VB6.FontChangeBold(lb_Find(1).Font, False)
				lb_Find(2).Font = VB6.FontChangeBold(lb_Find(2).Font, True)
				lb_Find(3).Font = VB6.FontChangeBold(lb_Find(3).Font, False)
				[tx_F3検索ユニット].Focus()
			Case 3
				PicFind(0).Enabled = False
				PicFind(1).Enabled = False
				PicFind(2).Enabled = False
				PicFind(3).Enabled = True
				lb_Find(0).Font = VB6.FontChangeBold(lb_Find(0).Font, False)
				lb_Find(1).Font = VB6.FontChangeBold(lb_Find(1).Font, False)
				lb_Find(2).Font = VB6.FontChangeBold(lb_Find(2).Font, False)
				lb_Find(3).Font = VB6.FontChangeBold(lb_Find(3).Font, True)
				[tx_F4検索PC区分].Focus()
		End Select
		On Error GoTo 0
	End Sub
	
	Private Sub CtlFocus(ByRef Index As Short)
		On Error Resume Next
		Select Case Index
			Case 0
				[tx_F1検索H2].Focus()
			Case 1
				[tx_F2検索名称].Focus()
			Case 2
				[tx_F3検索名称].Focus()
			Case 3
				[tx_F4検索T].Focus()
		End Select
		On Error GoTo 0
	End Sub
End Class