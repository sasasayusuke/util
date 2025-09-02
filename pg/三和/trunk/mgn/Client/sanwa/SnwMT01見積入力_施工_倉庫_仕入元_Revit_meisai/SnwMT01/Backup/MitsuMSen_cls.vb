Option Strict Off
Option Explicit On
Friend Class MitsuMSen_cls
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
	
	'Dim pwhr_ZaikKB         As String   '在庫区分の抽出式   2009/09/11 ADD
	'Dim psel_SQL         As String
	'Dim pSel_PKey        As String
	'Dim pSel_Caption     As String
	'Dim Find_ID           As String
	'Dim Find_Name         As String
	'Dim Find_kubn         As String
	Dim CLKFLG As Boolean 'リストビュー制御用
	
	'    Dim FieldCnt As Integer
	'    Dim FieldName() As Variant
	'    Dim FieldSize() As Integer
	'    Dim NameLen() As Integer
	'    Dim List_ColWidth() As Integer
	
	Dim PreviousControl As System.Windows.Forms.Control 'コントロールの戻りを制御
	Dim sql As String
	Dim rs As ADODB.Recordset
	
	Dim MeWidth, MeHeight As Integer
	Dim wkWidth As Short
	Dim WidthSa As Short
	Dim WkHeight As Integer
	Dim LvHeightLimit, MeHeightLimit As Integer
	Dim SelectF As Boolean
	'値返還用
	Private m_Parent As clsMitsumoriM
	Private m_DialogResult As Boolean
	Private m_DialogResultCode As String
	
	'クラス
	Private cSiiresaki As clsSiiresaki
	Private cTokuisaki As clsTokuisaki
	Private cNonyusaki As clsNonyusaki
	
	'//////////////////////////////////////
	'   呼び出し元のクラス
	'//////////////////////////////////////
	'UPGRADE_NOTE: Parent は Parent_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
	Public WriteOnly Property Parent_Renamed() As clsMitsumoriM
		Set(ByVal Value As clsMitsumoriM)
			m_Parent = Value
		End Set
	End Property
	
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
	'
	'Public Property Get DialogResultCode2() As String
	'    DialogResultCode2 = m_DialogResultCode2
	'End Property
	
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
	
	Private Sub MitsuMSen_cls_FormClosing(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
		Dim Cancel As Boolean = eventArgs.Cancel
		Dim UnloadMode As System.Windows.Forms.CloseReason = eventArgs.CloseReason
		'    Set ResultCodeSetControl1 = Nothing
		'    Set ResultCodeSetControl2 = Nothing
		'UPGRADE_NOTE: オブジェクト SeiSen_cls をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		SeiSen_cls = Nothing
		eventArgs.Cancel = Cancel
	End Sub
	
	'UPGRADE_WARNING: イベント MitsuMSen_cls.Resize は、フォームが初期化されたときに発生します。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"' をクリックしてください。
	Private Sub MitsuMSen_cls_Resize(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Resize
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
			SelListVw.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Me.ClientRectangle.Width) - (VB6.PixelsToTwipsX(SelListVw.Left) * 2)) '2012/09/13 ADD
			SelListVw.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.ClientRectangle.Height) - VB6.PixelsToTwipsY(SelListVw.Top) - (WkHeight * 2) - VB6.PixelsToTwipsY(cmdOk.Height))
			cmdOk.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(SelListVw.Height) + VB6.PixelsToTwipsY(SelListVw.Top) + WkHeight)
			cmdCan.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(SelListVw.Height) + VB6.PixelsToTwipsY(SelListVw.Top) + WkHeight)
		End If
	End Sub
	
	Private Sub MitsuMSen_cls_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		Dim i, j As Short
		Dim List_ColLen As Short
		
		On Error GoTo Form_Load_Err
		
		'クラス生成
		cSiiresaki = New clsSiiresaki
		cTokuisaki = New clsTokuisaki
		cNonyusaki = New clsNonyusaki
		
		'リストヘッダー項目セット
		With SelListVw
			.Columns.Clear()
			.Columns.Add("", "見積明細連番", CInt(VB6.TwipsToPixelsX(0)))
			.Columns.Add("", "見積番号", CInt(VB6.TwipsToPixelsX(0)))
			.Columns.Add("", "見積日付", CInt(VB6.TwipsToPixelsX(0))) '2020/09/16 ADD
			.Columns.Add("", "見積件名", CInt(VB6.TwipsToPixelsX(0)))
			.Columns.Add("", "行番号", CInt(VB6.TwipsToPixelsX(0)))
			.Columns.Add("", "製品NO", CInt(VB6.TwipsToPixelsX(1500)))
			.Columns.Add("", "仕様NO", CInt(VB6.TwipsToPixelsX(1000)))
			.Columns.Add("", "ﾍﾞｰｽ色", CInt(VB6.TwipsToPixelsX(1000)))
			.Columns.Add("", "漢字名称", CInt(VB6.TwipsToPixelsX(4600)))
			.Columns.Add("", "Ｗ", CInt(VB6.TwipsToPixelsX(600)))
			.Columns.Add("", "Ｄ", CInt(VB6.TwipsToPixelsX(600)))
			.Columns.Add("", "Ｈ", CInt(VB6.TwipsToPixelsX(600)))
			.Columns.Add("", "Ｄ１", CInt(VB6.TwipsToPixelsX(600)))
			.Columns.Add("", "Ｄ２", CInt(VB6.TwipsToPixelsX(600)))
			.Columns.Add("", "Ｈ１", CInt(VB6.TwipsToPixelsX(600)))
			.Columns.Add("", "Ｈ２", CInt(VB6.TwipsToPixelsX(600)))
			.Columns.Add("", "備考", CInt(VB6.TwipsToPixelsX(600)))
			.Columns.Add("見積数量", CInt(VB6.TwipsToPixelsX(1300)), System.Windows.Forms.HorizontalAlignment.Right)
			.Columns.Add("", "単位", CInt(VB6.TwipsToPixelsX(600)))
			.Columns.Add("定価", CInt(VB6.TwipsToPixelsX(1300)), System.Windows.Forms.HorizontalAlignment.Right)
			.Columns.Add("仕入単価", CInt(VB6.TwipsToPixelsX(1300)), System.Windows.Forms.HorizontalAlignment.Right)
			.Columns.Add("売上単価", CInt(VB6.TwipsToPixelsX(1300)), System.Windows.Forms.HorizontalAlignment.Right)
			.Columns.Add("", "売上税区分", CInt(VB6.TwipsToPixelsX(0)))
			.Columns.Add("", "", CInt(VB6.TwipsToPixelsX(600)))
			.Columns.Add("", "仕入業者名", CInt(VB6.TwipsToPixelsX(1395)))
			.Columns.Add("", "", CInt(VB6.TwipsToPixelsX(600)))
			.Columns.Add("", "出荷元名", CInt(VB6.TwipsToPixelsX(1395)))
			.Columns.Add("", "", CInt(VB6.TwipsToPixelsX(600)))
			.Columns.Add("", "送り先名", CInt(VB6.TwipsToPixelsX(1395)))
			
		End With
		
		SelListVw.Width = VB6.TwipsToPixelsX(13800 + 330) 'スクロールバー分プラス
		
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
		
		'    ResultCodeSetControl1.Tag = ""
		'    ResultCodeSetControl2.Tag = ""
		
		'桁セット-------------------
		'UPGRADE_WARNING: TextBox プロパティ tx_検索製品.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
		tx_検索製品.Maxlength = SeiIDLength
		'UPGRADE_WARNING: TextBox プロパティ tx_検索仕様.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
		tx_検索仕様.Maxlength = ShiyoIDLength
		'------------------------------------
		
		
		Exit Sub
Form_Load_Err: 
		MsgBox(Err.Number & " " & Err.Description)
		Exit Sub
	End Sub
	
	Private Sub MitsuMSen_cls_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
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
	
	Private Sub tx_得意先CD_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_得意先CD.Enter
		Item_Check(([tx_得意先CD].TabIndex))
		PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_得意先CD_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_得意先CD.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		If KeyAscii = Asc(" ") And ([tx_得意先CD].SelectionStart = 0 And [tx_得意先CD].SelectionLength = Len([tx_得意先CD].Text)) Then
			KeyAscii = 0
			'---参照画面表示
			'        SelectF = True
			If cTokuisaki.ShowDialog = True Then
				[tx_得意先CD].Text = cTokuisaki.得意先CD
				'            ReturnF = True
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
	
	Private Sub tx_納入先CD_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_納入先CD.Enter
		Item_Check(([tx_納入先CD].TabIndex))
		PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_納入先CD_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_納入先CD.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		If KeyAscii = Asc(" ") And ([tx_納入先CD].SelectionStart = 0 And [tx_納入先CD].SelectionLength = Len([tx_納入先CD].Text)) Then
			KeyAscii = 0
			'---参照画面表示
			cNonyusaki.得意先CD = [tx_得意先CD].Text
			If cNonyusaki.ShowDialog = True Then
				[tx_納入先CD].Text = cNonyusaki.納入先CD
				'            ReturnF = True
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
	
	Private Sub tx_見積件名_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_見積件名.Enter
		Item_Check((tx_見積件名.TabIndex))
		PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_検索製品_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_検索製品.Enter
		Item_Check((tx_検索製品.TabIndex))
		PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_検索仕様_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_検索仕様.Enter
		'    If PreviousControl.Name = "tx_検索製品" Then
		Item_Check((tx_検索仕様.TabIndex))
		'    End If
		PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_検索名称_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_検索名称.Enter
		Item_Check((tx_検索名称.TabIndex))
		PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_仕入業者CD_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_仕入業者CD.Enter
		Item_Check(([tx_仕入業者CD].TabIndex))
		PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_仕入業者CD_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_仕入業者CD.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		If KeyAscii = Asc(" ") And ([tx_仕入業者CD].SelectionStart = 0 And [tx_仕入業者CD].SelectionLength = Len([tx_仕入業者CD].Text)) Then
			KeyAscii = 0
			'---参照画面表示
			SelectF = True
			If cSiiresaki.ShowDialog = True Then
				[tx_仕入業者CD].Text = cSiiresaki.仕入先CD
				'            ReturnF = True
				[tx_仕入業者CD].Focus()
			Else
				[tx_仕入業者CD].Focus()
			End If
		End If
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
	
	Private Sub tx_仕入先CD_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_仕入先CD.Enter
		Item_Check(([tx_仕入先CD].TabIndex))
		PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_仕入先CD_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_仕入先CD.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		If KeyAscii = Asc(" ") And ([tx_仕入先CD].SelectionStart = 0 And [tx_仕入先CD].SelectionLength = Len([tx_仕入先CD].Text)) Then
			KeyAscii = 0
			'---参照画面表示
			SelectF = True
			If cSiiresaki.ShowDialog = True Then
				[tx_仕入先CD].Text = cSiiresaki.仕入先CD
				'            ReturnF = True
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
	
	Private Sub tx_W_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_W.Enter
		Item_Check((tx_W.TabIndex))
		PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_D_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_D.Enter
		Item_Check((tx_D.TabIndex))
		PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_H_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_H.Enter
		Item_Check(([tx_H].TabIndex))
		PreviousControl = Me.ActiveControl
	End Sub
	'''
	'''Private Sub tx_D1_GotFocus()
	'''    Item_Check (tx_D1.TabIndex)
	'''    Set PreviousControl = Me.ActiveControl
	'''End Sub
	'''
	'''Private Sub tx_D2_GotFocus()
	'''    Item_Check (tx_D2.TabIndex)
	'''    Set PreviousControl = Me.ActiveControl
	'''End Sub
	'''
	'''Private Sub tx_H1_GotFocus()
	'''    Item_Check (tx_H1.TabIndex)
	'''    Set PreviousControl = Me.ActiveControl
	'''End Sub
	'''
	'''Private Sub tx_H2_GotFocus()
	'''    Item_Check (tx_H2.TabIndex)
	'''    Set PreviousControl = Me.ActiveControl
	'''End Sub
	
	Private Sub ck_NotZero_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ck_NotZero.Enter
		Item_Check((ck_NotZero.TabIndex))
		PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub ck_NotZero_KeyDown(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyEventArgs) Handles ck_NotZero.KeyDown
		Dim KeyCode As Short = eventArgs.KeyCode
		Dim Shift As Short = eventArgs.KeyData \ &H10000
		If KeyCode = System.Windows.Forms.Keys.Return Then
			[cmdFind].Focus()
		End If
	End Sub
	
	Private Sub cmdFind_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdFind.Enter
		Item_Check(([cmdFind].TabIndex))
	End Sub
	
	Private Sub cmdFind_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdFind.Click
		On Error GoTo cmdFind_Click_Err
		
		If Not Download Then
			CheckAlarm("該当データがありません。")
			lbListCount.Text = ""
			tx_検索製品.Focus()
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
					[tx_H].Focus()
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
			'''                If Not (ResultCodeSetControl1 Is Nothing) Then
			'''                    With ResultCodeSetControl1
			'''''                        .Text = Trim$(SelListVw.SelectedItem.SubItems(0))
			'''                        .Text = Trim$(SelListVw.ListItems(SelListVw.SelectedItem.Index))
			'''                        .Tag = "True"
			'''                    End With
			'''                    With ResultCodeSetControl2
			'''                        .Text = Trim$(SelListVw.SelectedItem.SubItems(1))
			'''''                        .Text = Trim$(SelListVw.ListItems(SelListVw.SelectedItem.Index))
			'''                        .Tag = "True"
			'''                    End With
			'''                End If
			m_DialogResult = True
			'UPGRADE_WARNING: コレクション SelListVw.ListItems の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
			m_Parent.製品NO = Trim(SelListVw.Items.Item(SelListVw.FocusedItem.Index).Text)
			'UPGRADE_WARNING: コレクション SelListVw.ListItems の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
			m_DialogResultCode = Trim(SelListVw.Items.Item(SelListVw.FocusedItem.Index).Text)
		End If
		Me.Close()
	End Sub
	
	Private Function Item_Check(ByRef ItemNo As Short) As Short
		
		On Error GoTo Item_Check_Err
		
		'''    'キー項目「検索製品」のチェック
		'''    If ItemNo > [tx_検索製品].TabIndex Then
		'''        '--- 入力値をワークへ格納
		'''        If ISInt([tx_検索製品].Text) Then
		'''            [tx_検索製品].Text = Format([tx_検索製品].Text, String(tx_検索製品.MaxLength, "0"))
		'''        End If
		'''    End If
		'''
		'''    'キー項目「ID」のチェック
		'''    If ItemNo > [tx_検索仕様].TabIndex Then
		'''        '--- 入力値をワークへ格納
		'''        If ISInt([tx_検索仕様].Text) Then
		'''            [tx_検索仕様].Text = Format([tx_検索仕様].Text, String(tx_検索仕様.MaxLength, "0"))
		'''        End If
		'''    End If
		'''
		'    '終了IDのチェック
		'    If ItemNo > [tx_検索製品].TabIndex Then
		'        If IsCheckText([tx_検索製品]) = False Then
		'            [tx_検索仕様].Text = vbNullString
		'            [tx_検索仕様].SetFocus
		'        End If
		'    End If
		'tx_得意先CDのチェック
		If ItemNo > [tx_得意先CD].TabIndex Then
			'--- 入力値をワークへ格納
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
		
		'tx_仕入業者CDのチェック
		If ItemNo > [tx_仕入業者CD].TabIndex Then
			'--- 入力値をワークへ格納
			If ISInt(([tx_仕入業者CD].Text)) Then
				'UPGRADE_WARNING: TextBox プロパティ tx_仕入業者CD.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				[tx_仕入業者CD].Text = VB6.Format([tx_仕入業者CD].Text, New String("0", [tx_仕入業者CD].Maxlength))
			End If
		End If
		
		'tx_仕入先CDのチェック
		If ItemNo > [tx_仕入先CD].TabIndex Then
			'--- 入力値をワークへ格納
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
		
		Download = False
		HourGlass(True)
		
		sql = "SELECT TOP 1000 MSM.見積明細連番,MSM.見積番号,MH.見積件名,MSM.行番号,MH.見積日付,"
		sql = sql & " MSM.製品NO, MSM.仕様NO, MSM.ベース色, MSM.漢字名称, MSM.W, MSM.D, MSM.H, MSM.D1, MSM.D2, MSM.H1,MSM.明細備考,"
		sql = sql & " MSM.H2, MSM.見積数量, MSM.単位名, MSM.定価, MSM.仕入単価, MSM.売上単価, MSM.U区分, MSM.M区分, MSM.売上税区分,"
		sql = sql & " MSM.仕入先CD, MSM.仕入先名, MSM.配送先CD, MSM.配送先名, MSM.製品区分, MSM.見積区分, MSM.作業区分CD,"
		sql = sql & " MSM.仕入業者CD , MSM.仕入業者名, MH.得意先CD"
		sql = sql & " FROM TD見積シートM AS MSM"
		sql = sql & " INNER JOIN TD見積 AS MH"
		sql = sql & " ON MSM.見積番号 = MH.見積番号"
		
		'得意先
		If IsCheckText([tx_得意先CD]) = True Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & "MH.得意先CD LIKE '" & SQLString([tx_得意先CD]) & "%'"
		End If
		'納入先
		If IsCheckText([tx_納入先CD]) = True Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & "MH.納入先CD Like '" & SQLString([tx_納入先CD]) & "%'"
		End If
		'見積件名
		If IsCheckText(tx_見積件名) = True Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & "MH.見積件名 Like '%" & SQLString(tx_見積件名) & "%'"
		End If
		'仕入業者
		If IsCheckText([tx_仕入業者CD]) = True Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & "MSM.仕入業者CD Like '" & SQLString([tx_仕入業者CD]) & "%'"
		End If
		'仕入先
		If IsCheckText([tx_仕入先CD]) = True Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & "MSM.仕入先CD Like '" & SQLString([tx_仕入先CD]) & "%'"
		End If
		'製品NO
		If IsCheckText(tx_検索製品) = True Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & "MSM.製品NO Like '" & SQLString(tx_検索製品) & "%'"
		End If
		'仕様NO
		If IsCheckText(tx_検索仕様) = True Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & "MSM.仕様NO Like '" & SQLString(tx_検索仕様) & "%'"
		End If
		'漢字名称
		If IsCheckText(tx_検索名称) = True Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & "MSM.漢字名称 Like '%" & SQLString(tx_検索名称) & "%'"
		End If
		'W
		If IsCheckText(tx_W) = True Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & "MSM.W Like '%" & SQLString(tx_W) & "%'"
		End If
		'D
		If IsCheckText(tx_D) = True Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & "MSM.D Like '%" & SQLString(tx_D) & "%'"
		End If
		'H
		If IsCheckText([tx_H]) = True Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & "MSM.H Like '%" & SQLString([tx_H]) & "%'"
		End If
		
		'見積数量
		If ck_NotZero.CheckState = 1 Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & "MSM.見積数量 <> 0"
		End If
		
		
		
		If whr <> "" Then
			whr = " WHERE " & whr
		End If
		
		sql = sql & whr & " ORDER BY MH.見積日付 DESC, MH.見積番号 DESC, MSM.行番号 ASC"
		
		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
		
		
		LockWindowUpdate(Me.Handle.ToInt32)
		
		SelListVw.Items.Clear()
		
		Do Until rs.EOF
			'        buf = String(7, "0") & "0"              'ひとつ余計にする
			'        buf = Space$(1) & CStr(rs("製品NO").Value)
			itmX = SelListVw.Items.Add(rs.Fields("見積明細連番").Value)
			'        buf = String(7, "0") & "0"              'ひとつ余計にする
			'            buf = Space$(1) & CStr(rs(1).Value)
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(CStr(NullToZero((rs.Fields("見積番号").Value), "#")))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(CStr(VB6.Format(NullToZero((rs.Fields("見積日付").Value), ""), "yy/mm/dd"))) '2020/09/19 ADD
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(CStr(NullToZero((rs.Fields("見積件名").Value), "")))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(CStr(NullToZero((rs.Fields("行番号").Value), "#")))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(CStr(NullToZero((rs.Fields("製品NO").Value), "")))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(CStr(NullToZero((rs.Fields("仕様NO").Value), "")))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(CStr(NullToZero((rs.Fields("ベース色").Value), "")))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(CStr(NullToZero((rs.Fields("漢字名称").Value), "")))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(VB6.Format(NullToZero((rs.Fields("W").Value), ""), "#"))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(VB6.Format(NullToZero((rs.Fields("D").Value), ""), "#"))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(VB6.Format(NullToZero((rs.Fields("H").Value), ""), "#"))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(VB6.Format(NullToZero((rs.Fields("D1").Value), ""), "#"))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(VB6.Format(NullToZero((rs.Fields("D2").Value), ""), "#"))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(VB6.Format(NullToZero((rs.Fields("H1").Value), ""), "#"))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(VB6.Format(NullToZero((rs.Fields("H2").Value), ""), "#"))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(CStr(NullToZero((rs.Fields("明細備考").Value), "")))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(VB6.Format(NullToZero((rs.Fields("見積数量").Value), ""), "#,##0"))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(CStr(NullToZero((rs.Fields("単位名").Value), "")))
			
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(VB6.Format(NullToZero((rs.Fields("定価").Value), ""), "#,##0"))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(VB6.Format(NullToZero((rs.Fields("仕入単価").Value), ""), "#,##0"))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(VB6.Format(NullToZero((rs.Fields("売上単価").Value), ""), "#,##0"))
			'        itmX.ListSubItems.Add , , Format$(NullToZero(rs("U区分").Value, ""), "#")
			'        itmX.ListSubItems.Add , , Format$(NullToZero(rs("M区分").Value, ""), "#")
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(VB6.Format(NullToZero((rs.Fields("売上税区分").Value), ""), "#"))
			
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(VB6.Format(NullToZero((rs.Fields("仕入業者CD").Value), ""), ""))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(VB6.Format(NullToZero((rs.Fields("仕入業者名").Value), ""), ""))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(VB6.Format(NullToZero((rs.Fields("仕入先CD").Value), ""), ""))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(VB6.Format(NullToZero((rs.Fields("仕入先名").Value), ""), ""))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(VB6.Format(NullToZero((rs.Fields("配送先CD").Value), ""), ""))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(VB6.Format(NullToZero((rs.Fields("配送先名").Value), ""), ""))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(VB6.Format(NullToZero((rs.Fields("作業区分CD").Value), ""), "#"))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(VB6.Format(NullToZero((rs.Fields("製品区分").Value), ""), "#"))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(VB6.Format(NullToZero((rs.Fields("見積区分").Value), ""), "#"))
			rs.MoveNext()
		Loop 
		ReleaseRs(rs)
		'UPGRADE_NOTE: オブジェクト itmX をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		itmX = Nothing
		
		'    SelListVw.SortKey = 0
		'    SelListVw.Sorted = True
		'    SelListVw.Refresh
		
		'    '選択されているテキストを保持する
		'    FindListText = Space$(1) & CStr(tx_検索製品)
		'    Set itmFound = SelListVw.FindItem(FindListText, lvwText, , lvwWhole)
		'
		'    ' EnsureVisible メソッドを使用して
		'    ' コントロールをスクロールし、ListItem を選択します。
		'    If Not (itmFound Is Nothing) Then
		'        itmFound.EnsureVisible ' リスト ビュー コントロールをスクロールして、検出された ListItem を表示します。
		'        itmFound.Selected = True   ' ListItem を選択します。
		'    End If
		'    Set itmFound = Nothing
		
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
	''''''''
	'''''''''選択したコードを送るコントロールをセット
	''''''''Property Set ResCodeCTL1(ByRef ctl As Control)
	''''''''    Set ResultCodeSetControl1 = ctl
	''''''''End Property
	''''''''
	'''''''''選択したコードを送るコントロールをセット
	''''''''Property Set ResCodeCTL2(ByRef ctl As Control)
	''''''''    Set ResultCodeSetControl2 = ctl
	''''''''End Property
	''''''''
	'''''''''--------------------------------------------
	'''''''''在庫区分の抽出式をセット   '2009/09/11 ADD
	'''''''''   在庫区分=0
	'''''''''   在庫区分=1
	'''''''''   ""(指定なし)
	'''''''''--------------------------------------------
	''''''''Property Let whr_ZaikKB(ByVal New_whr_ZaikKB As String)
	''''''''    pwhr_ZaikKB = New_whr_ZaikKB
	''''''''End Property
	'''
	''''指定したPrimarryKeyをセット
	'''Property Let Sel_PKey(ByVal New_Sel_PKey As String)
	'''    pSel_PKey = New_Sel_PKey
	'''End Property
	'''
	''''指定したSQLをセット
	'''Property Let Sel_SQL(ByVal New_Sel_SQL As String)
	'''    psel_SQL = New_Sel_SQL
	'''End Property
	'''
	''''指定した標題をセット
	'''Property Let Sel_Caption(ByVal New_Sel_Caption As String)
	'''    pSel_Caption = New_Sel_Caption
	'''End Property
End Class