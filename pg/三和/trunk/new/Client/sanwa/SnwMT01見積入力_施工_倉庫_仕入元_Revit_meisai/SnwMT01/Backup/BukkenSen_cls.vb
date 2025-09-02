Option Strict Off
Option Explicit On
Friend Class BukkenSen_cls
	Inherits System.Windows.Forms.Form
	'
	'--------------------------------------------------------------------
	'  ユーザー名           株式会社 三和商研
	'  業務名               販売管理システム
	'  部門名               見積部門
	'  プログラム名         見積共通選択処理
	'  作成会社             テクノウェア株式会社
	'  作成日               2003/04/22
	'  作成者               oosawa
	'--------------------------------------------------------------------
	'   UPDATE
	'       2003/12/04  kawamura  合計金額に出精値引を含める
	'                             物件種別の区分追加
	'       2004/01/17  oosawa　　見積日付・見積番号の降順に並べる
	'                             idcの開放
	'--------------------------------------------------------------------
	'
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
	
	'--Formで使用する変数--
	'コントロールの戻りを制御
	'''''Dim PreviousControl As Control
	'各項目でEnterKeyが押されたかのﾁｪｯｸﾌﾗｸﾞ
	Dim ReturnF As Boolean
	'起動時のフォームサイズHold用
	Dim MeWidth As Integer
	Dim MeHeight As Integer
	Dim wkWidth As Short
	Dim WidthSa As Short
	Dim WkHeight As Integer
	Dim LvHeightLimit, MeHeightLimit As Integer
	
	'ボタン２重起動防止フラグ(cbFunc_Clickで使用)
	Dim CLK2F As Boolean
	
	'リストビュー制御用(ListViewで使用)
	Dim CLKFLG As Boolean
	
	Dim psel_SQL As String
	Dim pSel_PKey As String
	Dim pSel_Caption As String
	Dim Find_ID As String
	Dim Find_Name As String
	
	Dim sql As String
	Dim rs As ADODB.Recordset
	''''Dim ResultCodeSetControl As Control     '選択したコードの送り先をセットする。
	'日付用ワーク
	'UPGRADE_WARNING: 配列を New で宣言することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC9D3AE5-6B95-4B43-91C7-28276302A5E8"' をクリックしてください。
	Dim idc(1) As New iDate
	'クラス
	Private cTanto As clsTanto
	Private cTokuisaki As clsTokuisaki
	Private cNonyusaki As clsNonyusaki
	
	'値返還用
	Private m_DialogResult As Boolean
	Private m_DialogResultCode As String
	
	'UPGRADE_NOTE: DialogResult は DialogResult_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
	Public ReadOnly Property DialogResult_Renamed() As Boolean
		Get
			DialogResult_Renamed = m_DialogResult
		End Get
	End Property
	
	Public ReadOnly Property DialogResultCode() As String
		Get
			DialogResultCode = m_DialogResultCode
		End Get
	End Property
	
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
	
	Private Sub BukkenSen_cls_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		''    Dim i As Integer, j As Integer
		
		On Error GoTo Form_Load_Err
		
		'リストヘッダー項目セット
		With SelListVw
			.Columns.Clear()
			.Columns.Add("", "物件番号", CInt(VB6.TwipsToPixelsX(1150)))
			.Columns.Add("登録日付", CInt(VB6.TwipsToPixelsX(1150)), System.Windows.Forms.HorizontalAlignment.Center)
			.Columns.Add("", "得意先", CInt(VB6.TwipsToPixelsX(1000)))
			.Columns.Add("", "納入先", CInt(VB6.TwipsToPixelsX(1000)))
			.Columns.Add("", "物件名", CInt(VB6.TwipsToPixelsX(4500)))
			.Columns.Add("更新日付", CInt(VB6.TwipsToPixelsX(1700)), System.Windows.Forms.HorizontalAlignment.Center)
		End With
		
		'リストビューのヘッダを平面にする
		Call SetFlatHeader(SelListVw)
		
		wkWidth = (VB6.PixelsToTwipsX(Me.SelListVw.Left) * 2) + VB6.PixelsToTwipsX(SelListVw.Width) 'wkフォーム幅セット
		WidthSa = VB6.PixelsToTwipsX(Me.Width) - VB6.PixelsToTwipsX(Me.ClientRectangle.Width) 'フォームWidthの内外値差
		
		If wkWidth < 11070 Then
			wkWidth = 11070
		End If
		
		[cmdFind].Left = VB6.TwipsToPixelsX(wkWidth - (VB6.PixelsToTwipsX(SelListVw.Left) + VB6.PixelsToTwipsX([cmdFind].Width))) '検索ボタン位置決定
		cmdOk.Left = VB6.TwipsToPixelsX(wkWidth - (VB6.PixelsToTwipsX(SelListVw.Left) + VB6.PixelsToTwipsX(cmdOk.Width) + VB6.PixelsToTwipsX(cmdCan.Width))) 'ＯＫボタン位置決定
		cmdCan.Left = VB6.TwipsToPixelsX(wkWidth - (VB6.PixelsToTwipsX(SelListVw.Left) + VB6.PixelsToTwipsX(cmdCan.Width))) 'キャンセルボタン位置決定
		'UPGRADE_ISSUE: Form プロパティ BukkenSen_cls.ScaleWidth はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Me.ScaleWidth = wkWidth 'フォーム幅決定(ScaleWidth)
		Me.Width = VB6.TwipsToPixelsX(wkWidth + WidthSa) 'フォーム幅決定(Width)
		
		'クラス生成
		cTanto = New clsTanto
		cTokuisaki = New clsTokuisaki
		cNonyusaki = New clsNonyusaki
		
		'日付セット
		idc(0).SetupA(Me, "s物件登録日", 0)
		idc(1).SetupA(Me, "e物件登録日", 0)
		'フォームを画面の中央に配置
		Me.Top = VB6.TwipsToPixelsY((VB6.PixelsToTwipsY(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height) - VB6.PixelsToTwipsY(Me.Height)) \ 2)
		Me.Left = VB6.TwipsToPixelsX((VB6.PixelsToTwipsX(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width) - VB6.PixelsToTwipsX(Me.Width)) \ 2)
		'項目クリア
		Call SetupBlank()
		
		MeWidth = VB6.PixelsToTwipsX(Me.Width)
		MeHeight = VB6.PixelsToTwipsY(Me.Height) - VB6.PixelsToTwipsY(SelListVw.Height)
		LvHeightLimit = SelListVw.Font.SizeInPoints * (34 + 24) '(列見出し + 明細一行分）
		MeHeightLimit = MeHeight + LvHeightLimit
		
		Exit Sub
Form_Load_Err: 
		MsgBox(Err.Number & " " & Err.Description)
		Exit Sub
	End Sub
	
	'UPGRADE_WARNING: イベント BukkenSen_cls.Resize は、フォームが初期化されたときに発生します。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"' をクリックしてください。
	Private Sub BukkenSen_cls_Resize(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Resize
		
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
	
	Private Sub BukkenSen_cls_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
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
	
	Private Sub BukkenSen_cls_FormClosing(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
		Dim Cancel As Boolean = eventArgs.Cancel
		Dim UnloadMode As System.Windows.Forms.CloseReason = eventArgs.CloseReason
		'UPGRADE_NOTE: オブジェクト idc() をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		idc(0) = Nothing
		'UPGRADE_NOTE: オブジェクト idc() をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		idc(1) = Nothing
		eventArgs.Cancel = Cancel
	End Sub
	
	Private Sub SetupBlank()
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
			If TypeOf ctl Is AxExDateText.AxExDateTextBoxY Then
				ctl.Text = vbNullString
			End If
			'UPGRADE_WARNING: TypeOf に新しい動作が指定されています。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
			If TypeOf ctl Is AxExDateText.AxExDateTextBoxM Then
				ctl.Text = vbNullString
			End If
			'UPGRADE_WARNING: TypeOf に新しい動作が指定されています。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
			If TypeOf ctl Is AxExDateText.AxExDateTextBoxD Then
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
	End Sub
	
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
			'        SelectF = True
			If cTokuisaki.ShowDialog = True Then
				[tx_得意先CD].Text = cTokuisaki.得意先CD
				ReturnF = True
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
	
	Private Sub tx_得意先CD_RtnKeyDown(ByRef KeyCode As Short, ByRef Shift As Short, ByRef Cancel As Boolean)
		ReturnF = True
	End Sub
	
	Private Sub tx_得意先CD_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_得意先CD.Leave
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_得意先CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_得意先CD].Undo()
		End If
		ReturnF = False
	End Sub
	
	Private Sub tx_納入先CD_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_納入先CD.Enter
		If Item_Check(([tx_納入先CD].TabIndex)) = False Then
			Exit Sub
		End If
		'    Set PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_納入先CD_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_納入先CD.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		If KeyAscii = Asc(" ") And ([tx_納入先CD].SelectionStart = 0 And [tx_納入先CD].SelectionLength = Len([tx_納入先CD].Text)) Then
			KeyAscii = 0
			'---参照画面表示
			cNonyusaki.得意先CD = [tx_得意先CD].Text
			If cNonyusaki.ShowDialog = True Then
				[tx_納入先CD].Text = cNonyusaki.納入先CD
				ReturnF = True
				[tx_納入先CD].Focus()
			Else
				[tx_納入先CD].Focus()
			End If
		End If
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
	
	Private Sub tx_納入先CD_RtnKeyDown(ByRef KeyCode As Short, ByRef Shift As Short, ByRef Cancel As Boolean)
		ReturnF = True
	End Sub
	
	Private Sub tx_納入先CD_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_納入先CD.Leave
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_納入先CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_納入先CD].Undo()
		End If
		ReturnF = False
	End Sub
	'2009/12/02 ADD↓
	Private Sub tx_担当者CD_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_担当者CD.Enter
		If Item_Check(([tx_担当者CD].TabIndex)) = False Then
			Exit Sub
		End If
		'    Set PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_担当者CD_SpcKeyPress(ByRef KeyAscii As Short, ByRef Cancel As Boolean)
		If KeyAscii = Asc(" ") And ([tx_担当者CD].SelectionStart = 0 And [tx_担当者CD].SelectionLength = Len([tx_担当者CD].Text)) Then
			KeyAscii = 0
			'---参照画面表示
			If cTanto.ShowDialog = True Then
				[tx_担当者CD].Text = cTanto.担当者CD
				ReturnF = True
				[tx_担当者CD].Focus()
			Else
				[tx_担当者CD].Focus()
			End If
		End If
	End Sub
	
	Private Sub tx_担当者CD_RtnKeyDown(ByRef KeyCode As Short, ByRef Shift As Short, ByRef Cancel As Boolean)
		ReturnF = True
	End Sub
	
	Private Sub tx_担当者CD_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_担当者CD.Leave
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_担当者CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_担当者CD].Undo()
		End If
		ReturnF = False
	End Sub
	'2009/12/02 ADD↑
	
	Private Sub tx_s物件番号_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_s物件番号.Enter
		If Item_Check(([tx_s物件番号].TabIndex)) = False Then
			Exit Sub
		End If
		[tx_s物件番号].Text = Trim([tx_s物件番号].Text)
		'    Set PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_s物件番号_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_s物件番号.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		Const Numbers As String = "0123456789" ' 入力許可文字
		
		If KeyAscii = System.Windows.Forms.Keys.Return Then KeyAscii = 0
		
		'数字検査をする
		If KeyAscii <> 8 Then ' バックスペースは例外
			If InStr(Numbers, Chr(KeyAscii)) = 0 Then
				KeyAscii = 0 ' 入力を無効にする
				GoTo EventExitSub
			End If
		End If
EventExitSub: 
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
	
	Private Sub tx_s物件番号_RtnKeyDown(ByRef KeyCode As Short, ByRef Shift As Short, ByRef Cancel As Boolean)
		ReturnF = True
	End Sub
	
	Private Sub tx_s物件番号_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_s物件番号.Leave
		Dim Buf As String
		
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_s物件番号.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_s物件番号].Undo()
		End If
		ReturnF = False
		
		'UPGRADE_WARNING: TextBox プロパティ tx_s物件番号.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
		Buf = New String(" ", [tx_s物件番号].Maxlength)
		Buf = RSet(Trim([tx_s物件番号].Text), Len(Buf))
		[tx_s物件番号].Text = Buf
	End Sub
	
	Private Sub tx_e物件番号_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_e物件番号.Enter
		If Item_Check(([tx_e物件番号].TabIndex)) = False Then
			Exit Sub
		End If
		[tx_e物件番号].Text = Trim([tx_e物件番号].Text)
		'    Set PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_e物件番号_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_e物件番号.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		Const Numbers As String = "0123456789" ' 入力許可文字
		
		If KeyAscii = System.Windows.Forms.Keys.Return Then KeyAscii = 0
		
		'数字検査をする
		If KeyAscii <> 8 Then ' バックスペースは例外
			If InStr(Numbers, Chr(KeyAscii)) = 0 Then
				KeyAscii = 0 ' 入力を無効にする
				GoTo EventExitSub
			End If
		End If
EventExitSub: 
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
	
	Private Sub tx_e物件番号_RtnKeyDown(ByRef KeyCode As Short, ByRef Shift As Short, ByRef Cancel As Boolean)
		ReturnF = True
	End Sub
	
	Private Sub tx_e物件番号_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_e物件番号.Leave
		Dim Buf As String
		
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_e物件番号.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_e物件番号].Undo()
		End If
		ReturnF = False
		
		'UPGRADE_WARNING: TextBox プロパティ tx_e物件番号.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
		Buf = New String(" ", [tx_e物件番号].Maxlength)
		Buf = RSet(Trim([tx_e物件番号].Text), Len(Buf))
		[tx_e物件番号].Text = Buf
	End Sub
	
	Private Sub tx_物件名_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_物件名.Enter
		If Item_Check(([tx_物件名].TabIndex)) = False Then
			Exit Sub
		End If
		'    Set PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_物件名_RtnKeyDown(ByRef KeyCode As Short, ByRef Shift As Short, ByRef Cancel As Boolean)
		ReturnF = True
	End Sub
	
	Private Sub tx_物件名_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_物件名.Leave
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_物件名.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_物件名].Undo()
		End If
		ReturnF = False
	End Sub
	
	Private Sub tx_s物件登録日Y_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_s物件登録日Y.Enter
		'入力チェック
		If Item_Check(([tx_s物件登録日Y].TabIndex)) = False Then
			Exit Sub
		End If
		'    Set PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_s物件登録日Y_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_s物件登録日Y.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		If KeyAscii <> 8 Then ' バックスペースは例外
			'UPGRADE_WARNING: TextBox プロパティ tx_s物件登録日Y.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
			'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
			If LenB(StrConv([tx_s物件登録日Y].Text & Chr(KeyAscii), vbFromUnicode)) - LenB(StrConv([tx_s物件登録日Y].SelectedText, vbFromUnicode)) = [tx_s物件登録日Y].Maxlength Then
				ReturnF = True
				[tx_s物件登録日Y].Focus()
			End If
		End If
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
	
	Private Sub tx_s物件登録日Y_RtnKeyDown(ByRef KeyCode As Short, ByRef Shift As Short, ByRef Cancel As Boolean)
		ReturnF = True
	End Sub
	
	Private Sub tx_s物件登録日Y_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_s物件登録日Y.Leave
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_s物件登録日Y.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_s物件登録日Y].Undo()
		End If
		ReturnF = False
	End Sub
	
	Private Sub tx_s物件登録日M_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_s物件登録日M.Enter
		'入力チェック
		If Item_Check(([tx_s物件登録日M].TabIndex)) = False Then
			Exit Sub
		End If
		'    Set PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_s物件登録日M_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_s物件登録日M.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		If KeyAscii <> 8 Then ' バックスペースは例外
			'UPGRADE_WARNING: TextBox プロパティ tx_s物件登録日M.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
			'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
			If LenB(StrConv([tx_s物件登録日M].Text & Chr(KeyAscii), vbFromUnicode)) - LenB(StrConv([tx_s物件登録日M].SelectedText, vbFromUnicode)) = [tx_s物件登録日M].Maxlength Then
				If KeyAscii <> 0 Then
					ReturnF = True
					[tx_s物件登録日M].Focus()
				End If
			Else
				Select Case Chr(KeyAscii)
					Case CStr(2) To CStr(9)
						ReturnF = True
						[tx_s物件登録日M].Focus()
				End Select
			End If
		End If
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
	
	Private Sub tx_s物件登録日M_RtnKeyDown(ByRef KeyCode As Short, ByRef Shift As Short, ByRef Cancel As Boolean)
		ReturnF = True
	End Sub
	
	Private Sub tx_s物件登録日M_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_s物件登録日M.Leave
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_s物件登録日M.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_s物件登録日M].Undo()
		End If
		ReturnF = False
	End Sub
	
	Private Sub tx_s物件登録日D_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_s物件登録日D.Enter
		'入力チェック
		If Item_Check(([tx_s物件登録日D].TabIndex)) = False Then
			Exit Sub
		End If
		'    Set PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_s物件登録日D_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_s物件登録日D.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		If KeyAscii <> 8 Then ' バックスペースは例外
			'UPGRADE_WARNING: TextBox プロパティ tx_s物件登録日D.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
			'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
			If LenB(StrConv([tx_s物件登録日D].Text & Chr(KeyAscii), vbFromUnicode)) - LenB(StrConv([tx_s物件登録日D].SelectedText, vbFromUnicode)) = [tx_s物件登録日D].Maxlength Then
				If KeyAscii <> 0 Then '入力キーがエラーでないならば
					ReturnF = True
					[tx_s物件登録日D].Focus()
				End If
			Else
				Select Case Chr(KeyAscii)
					Case CStr(4) To CStr(9)
						ReturnF = True
						[tx_s物件登録日D].Focus()
				End Select
			End If
		End If
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
	
	Private Sub tx_s物件登録日D_RtnKeyDown(ByRef KeyCode As Short, ByRef Shift As Short, ByRef Cancel As Boolean)
		ReturnF = True
	End Sub
	
	Private Sub tx_s物件登録日D_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_s物件登録日D.Leave
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_s物件登録日D.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_s物件登録日D].Undo()
		End If
		ReturnF = False
	End Sub
	
	Private Sub tx_e物件登録日Y_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_e物件登録日Y.Enter
		'入力チェック
		If Item_Check(([tx_e物件登録日Y].TabIndex)) = False Then
			Exit Sub
		End If
		'    Set PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_e物件登録日Y_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_e物件登録日Y.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		If KeyAscii <> 8 Then ' バックスペースは例外
			'UPGRADE_WARNING: TextBox プロパティ tx_e物件登録日Y.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
			'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
			If LenB(StrConv([tx_e物件登録日Y].Text & Chr(KeyAscii), vbFromUnicode)) - LenB(StrConv([tx_e物件登録日Y].SelectedText, vbFromUnicode)) = [tx_e物件登録日Y].Maxlength Then
				ReturnF = True
				[tx_e物件登録日Y].Focus()
			End If
		End If
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
	
	Private Sub tx_e物件登録日Y_RtnKeyDown(ByRef KeyCode As Short, ByRef Shift As Short, ByRef Cancel As Boolean)
		ReturnF = True
	End Sub
	
	Private Sub tx_e物件登録日Y_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_e物件登録日Y.Leave
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_e物件登録日Y.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_e物件登録日Y].Undo()
		End If
		ReturnF = False
	End Sub
	
	Private Sub tx_e物件登録日M_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_e物件登録日M.Enter
		'入力チェック
		If Item_Check(([tx_e物件登録日M].TabIndex)) = False Then
			Exit Sub
		End If
		'    Set PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_e物件登録日M_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_e物件登録日M.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		If KeyAscii <> 8 Then ' バックスペースは例外
			'UPGRADE_WARNING: TextBox プロパティ tx_e物件登録日M.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
			'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
			If LenB(StrConv([tx_e物件登録日M].Text & Chr(KeyAscii), vbFromUnicode)) - LenB(StrConv([tx_e物件登録日M].SelectedText, vbFromUnicode)) = [tx_e物件登録日M].Maxlength Then
				If KeyAscii <> 0 Then
					ReturnF = True
					[tx_e物件登録日M].Focus()
				End If
			Else
				Select Case Chr(KeyAscii)
					Case CStr(2) To CStr(9)
						ReturnF = True
						[tx_e物件登録日M].Focus()
				End Select
			End If
		End If
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
	
	Private Sub tx_e物件登録日M_RtnKeyDown(ByRef KeyCode As Short, ByRef Shift As Short, ByRef Cancel As Boolean)
		ReturnF = True
	End Sub
	
	Private Sub tx_e物件登録日M_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_e物件登録日M.Leave
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_e物件登録日M.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_e物件登録日M].Undo()
		End If
		ReturnF = False
	End Sub
	
	Private Sub tx_e物件登録日D_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_e物件登録日D.Enter
		'入力チェック
		If Item_Check(([tx_e物件登録日D].TabIndex)) = False Then
			Exit Sub
		End If
		'    Set PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_e物件登録日D_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_e物件登録日D.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		If KeyAscii <> 8 Then ' バックスペースは例外
			'UPGRADE_WARNING: TextBox プロパティ tx_e物件登録日D.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
			'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
			If LenB(StrConv([tx_e物件登録日D].Text & Chr(KeyAscii), vbFromUnicode)) - LenB(StrConv([tx_e物件登録日D].SelectedText, vbFromUnicode)) = [tx_e物件登録日D].Maxlength Then
				If KeyAscii <> 0 Then '入力キーがエラーでないならば
					ReturnF = True
					[tx_e物件登録日D].Focus()
				End If
			Else
				Select Case Chr(KeyAscii)
					Case CStr(4) To CStr(9)
						ReturnF = True
						[tx_e物件登録日D].Focus()
				End Select
			End If
		End If
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
	
	Private Sub tx_e物件登録日D_RtnKeyDown(ByRef KeyCode As Short, ByRef Shift As Short, ByRef Cancel As Boolean)
		ReturnF = True
	End Sub
	
	Private Sub tx_e物件登録日D_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_e物件登録日D.Leave
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_e物件登録日D.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_e物件登録日D].Undo()
		End If
		ReturnF = False
	End Sub
	
	Private Sub cmdFind_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdFind.Enter
		Item_Check(([cmdFind].TabIndex))
	End Sub
	
	Private Sub cmdFind_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdFind.Click
		''    On Error GoTo cmdFind_Click_Err
		
		If Not Download Then
			CheckAlarm("該当データがありません。")
			lbListCount.Text = ""
			[tx_得意先CD].Focus()
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
					[tx_e物件登録日D].Focus()
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
			
			m_DialogResult = True
			'UPGRADE_WARNING: コレクション SelListVw.ListItems の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
			m_DialogResultCode = Trim(SelListVw.Items.Item(SelListVw.FocusedItem.Index).Text)
		End If
		Me.Close()
	End Sub
	'''''
	''''''選択したコードを送るコントロールをセット
	'''''Property Set ResCodeCTL(ByRef ctl As Control)
	'''''    Set ResultCodeSetControl = ctl
	'''''End Property
	
	Private Function Item_Check(ByRef ItemNo As Short) As Short
		Dim Chk_ID As String
		
		On Error GoTo Item_Check_Err
		
		Item_Check = False
		
		'キー項目「得意先CD」のチェック
		If ItemNo > [tx_得意先CD].TabIndex Then
			If ISInt(([tx_得意先CD].Text)) Then
				'UPGRADE_WARNING: TextBox プロパティ tx_得意先CD.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				[tx_得意先CD].Text = VB6.Format([tx_得意先CD].Text, New String("0", [tx_得意先CD].Maxlength))
			End If
		End If
		
		'キー項目「納入先CD」のチェック
		If ItemNo > [tx_納入先CD].TabIndex Then
			If ISInt(([tx_納入先CD].Text)) Then
				'UPGRADE_WARNING: TextBox プロパティ tx_納入先CD.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				[tx_納入先CD].Text = VB6.Format([tx_納入先CD].Text, New String("0", [tx_納入先CD].Maxlength))
			End If
		End If
		
		'開始見積日付のチェック
		If ItemNo > idc(0).TabIndex Then
			If Not idc(0).IsAllNull Then
				If IsDate(idc(0).Text) = False Then
					CriticalAlarm("開始見積最終出力日が不正です。")
					'UPGRADE_WARNING: オブジェクト idc().ErrorPart.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					idc(0).ErrorPart.Undo()
					idc(0).ErrorPart.Focus()
					Exit Function
				End If
			End If
		End If
		
		'終了見積日付のチェック
		If ItemNo > idc(1).TabIndex Then
			If Not idc(1).IsAllNull Then
				If IsDate(idc(1).Text) = False Then
					CriticalAlarm("終了見積最終出力日が不正です。")
					'UPGRADE_WARNING: オブジェクト idc().ErrorPart.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					idc(1).ErrorPart.Undo()
					idc(1).ErrorPart.Focus()
					Exit Function
				End If
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
		Dim ret As String '種別変換用
		
		Download = False
		HourGlass(True)
		
		sql = "SELECT  物件番号, 物件登録日付, 物件名, 物件略称,"
		sql = sql & "  得意先CD, 納入得意先CD, 納入先CD,"
		sql = sql & "  担当者CD, 部署CD,"
		'    sql = sql & "  見積予定日付 , 受付日付, 仕入予定日付, 完工日付, 請求予定日付,"
		sql = sql & "  登録変更日"
		sql = sql & "  FROM TD物件情報 AS BK"
		
		'得意先
		If IsCheckText([tx_得意先CD]) = True Then
			whr = whr & "得意先CD LIKE '" & SQLString(([tx_得意先CD].Text)) & "%'"
		End If
		'納入先
		If IsCheckText([tx_納入先CD]) = True Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & "納入先CD Like '" & SQLString(([tx_納入先CD].Text)) & "%'"
		End If
		'担当者
		If IsCheckText([tx_担当者CD]) = True Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & "担当者CD Like '%" & SQLString(([tx_担当者CD].Text)) & "%'"
		End If
		'物件番号
		If IsCheckText([tx_s物件番号]) = True Or IsCheckText([tx_e物件番号]) = True Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & SQLIntRange("BK.物件番号", Trim([tx_s物件番号].Text), Trim([tx_e物件番号].Text),  , False)
		End If
		'物件名
		If IsCheckText([tx_物件名]) = True Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & "物件名 Like '%" & SQLString(([tx_物件名].Text)) & "%'"
		End If
		If idc(0).Text <> vbNullString Or idc(1).Text <> vbNullString Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & SQLDateRange("BK.物件登録日付", idc(0).Text, idc(1).Text, DBType, False)
		End If
		
		If whr <> "" Then
			whr = " WHERE " & whr
		End If
		
		sql = sql & whr & " ORDER BY BK.物件登録日付 DESC, BK.物件番号 DESC"
		
		
		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
		
		LockWindowUpdate(Me.Handle.ToInt32)
		
		SelListVw.Items.Clear()
		
		Do Until rs.EOF
			Buf = New String("0", 7) & "0" 'ひとつ余計にする
			If ISInt((rs.Fields(0).Value)) Then
				Buf = RSet(CStr(rs.Fields(0).Value), Len(Buf))
			Else
				Buf = Space(1) & CStr(rs.Fields("物件番号").Value)
			End If
			itmX = SelListVw.Items.Add(Buf)
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(CStr(VB6.Format(NullToZero((rs.Fields("物件登録日付").Value), ""), "yy/mm/dd"))) '2007/02/16 ADD
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(CStr(NullToZero((rs.Fields("得意先CD").Value), "")))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(CStr(NullToZero((rs.Fields("納入先CD").Value), "")))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(CStr(NullToZero((rs.Fields("物件名").Value), "")))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(CStr(VB6.Format(NullToZero((rs.Fields("登録変更日").Value), ""), "yy/mm/dd hh:mm"))) '2012/10/26 ADD
			rs.MoveNext()
		Loop 
		ReleaseRs(rs)
		'UPGRADE_NOTE: オブジェクト itmX をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		itmX = Nothing
		
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
End Class