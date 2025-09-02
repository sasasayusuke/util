Option Strict Off
Option Explicit On
Friend Class KyakuZaikoSen_cls
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
	
	Dim pwhr_ZaikKB As String '在庫区分の抽出式   2009/09/11 ADD
	'Dim psel_SQL         As String
	'Dim pSel_PKey        As String
	'Dim pSel_Caption     As String
	'Dim Find_ID           As String
	'Dim Find_Name         As String
	'Dim Find_kubn         As String
	Dim CLKFLG As Boolean 'リストビュー制御用
	''テーブル構成情報
	'    Dim FieldCnt As Integer
	'    Dim FieldName() As Variant
	'    Dim FieldSize() As Integer
	'    Dim NameLen() As Integer
	'    Dim List_ColWidth() As Integer
	'
	Dim PreviousControl As System.Windows.Forms.Control 'コントロールの戻りを制御
	Dim sql As String
	Dim rs As ADODB.Recordset
	'Dim ResultCodeSetControl1 As Control     '選択したコードの送り先をセットする。
	'Dim ResultCodeSetControl2 As Control     '選択したコードの送り先をセットする。
	Dim MeWidth, MeHeight As Integer
	Dim wkWidth As Short
	Dim WidthSa As Short
	Dim WkHeight As Integer
	Dim LvHeightLimit, MeHeightLimit As Integer
	Dim SelectF As Boolean
	
	'クラス
	Private cSiiresaki As clsSiiresaki
	
	'値返還用
	Private m_DialogResult As Boolean
	Private m_DialogResultCode1 As String
	Private m_DialogResultCode2 As String
	
	'担当者CD
	Private m_担当者CD As Short
	'得意先CD
	Private m_得意先CD As String
	
	'//////////////////////////////////////
	'   担当者CD
	'//////////////////////////////////////
	
	Public Property 担当者CD() As Short
		Get
			担当者CD = m_担当者CD
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
			得意先CD = m_得意先CD
		End Get
		Set(ByVal Value As String)
			m_得意先CD = Value
		End Set
	End Property
	
	''''------------------------------------------
	''''   選択画面セットアップ
	''''       Title       :選択画面のCaption
	''''       ResCtl      :選択コードを送るコントロールをセット
	''''       sql         :リストセット用レコードソース
	''''       Mode        :直送先選択
	''''       FieldsWidth :sqlで指定した項目分のリスト幅をセット(NULLにすると自動)
	''''------------------------------------------
	'''Public Function SelSetup(Title As String, sql As String _
	''''                        , Mode As String _
	''''                        , ParamArray FieldsWidth())
	'''
	'''    Dim i As Integer
	'''
	''''''    pSel_FormTitle = Title
	''''''    psel_SQL = sql
	''''''    pSel_Mode = Mode
	''''''    ReDim List_ColWidth(0 To UBound(FieldsWidth))
	''''''
	''''''    For i = 0 To UBound(FieldsWidth)
	''''''        List_ColWidth(i) = FieldsWidth(i)
	''''''    Next
	'''
	'''End Function
	
	'UPGRADE_NOTE: DialogResult は DialogResult_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
	Public ReadOnly Property DialogResult_Renamed() As Boolean
		Get
			DialogResult_Renamed = m_DialogResult
		End Get
	End Property
	
	Public ReadOnly Property DialogResultCode1() As String
		Get
			DialogResultCode1 = m_DialogResultCode1
		End Get
	End Property
	
	Public ReadOnly Property DialogResultCode2() As String
		Get
			DialogResultCode2 = m_DialogResultCode2
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
	
	Private Sub KyakuZaikoSen_cls_FormClosing(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
		Dim Cancel As Boolean = eventArgs.Cancel
		Dim UnloadMode As System.Windows.Forms.CloseReason = eventArgs.CloseReason
		'    Set ResultCodeSetControl1 = Nothing
		'    Set ResultCodeSetControl2 = Nothing
		'UPGRADE_NOTE: オブジェクト KyakuZaikoSen_cls をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		Me = Nothing
		eventArgs.Cancel = Cancel
	End Sub
	
	'UPGRADE_WARNING: イベント KyakuZaikoSen_cls.Resize は、フォームが初期化されたときに発生します。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"' をクリックしてください。
	Private Sub KyakuZaikoSen_cls_Resize(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Resize
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
	
	Private Sub KyakuZaikoSen_cls_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		Dim i, j As Short
		Dim List_ColLen As Short
		
		On Error GoTo Form_Load_Err
		
		'クラス生成
		cSiiresaki = New clsSiiresaki
		
		'リストヘッダー項目セット
		With SelListVw
			.Columns.Clear()
			.Columns.Add("", "製品NO", CInt(VB6.TwipsToPixelsX(1000)))
			.Columns.Add("", "仕様NO", CInt(VB6.TwipsToPixelsX(1000)))
			.Columns.Add("", "漢字名称", CInt(VB6.TwipsToPixelsX(4600)))
			.Columns.Add("", "Ｗ", CInt(VB6.TwipsToPixelsX(750)))
			.Columns.Add("", "Ｄ", CInt(VB6.TwipsToPixelsX(750)))
			.Columns.Add("", "Ｈ", CInt(VB6.TwipsToPixelsX(750)))
			.Columns.Add("", "Ｄ１", CInt(VB6.TwipsToPixelsX(750)))
			.Columns.Add("", "Ｄ２", CInt(VB6.TwipsToPixelsX(750)))
			.Columns.Add("", "Ｈ１", CInt(VB6.TwipsToPixelsX(750)))
			.Columns.Add("", "Ｈ２", CInt(VB6.TwipsToPixelsX(750)))
			.Columns.Add("", "仕入先名", CInt(VB6.TwipsToPixelsX(1450)))
		End With
		
		SelListVw.Width = VB6.TwipsToPixelsX(13300 + 330) 'スクロールバー分プラス
		
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
		
		''    ResultCodeSetControl1.Tag = ""
		''    ResultCodeSetControl2.Tag = ""
		
		Exit Sub
Form_Load_Err: 
		MsgBox(Err.Number & " " & Err.Description)
		Exit Sub
	End Sub
	
	Private Sub KyakuZaikoSen_cls_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
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
	
	Private Sub tx_検索仕入先_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_検索仕入先.Enter
		Item_Check(([tx_検索仕入先].TabIndex))
		PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_検索仕入先_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_検索仕入先.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		If KeyAscii = Asc(" ") And ([tx_検索仕入先].SelectionStart = 0 And [tx_検索仕入先].SelectionLength = Len([tx_検索仕入先].Text)) Then
			KeyAscii = 0
			'---参照画面表示
			SelectF = True
			If cSiiresaki.ShowDialog = True Then
				[tx_検索仕入先].Text = cSiiresaki.仕入先CD
				'            ReturnF = True
				[tx_検索仕入先].Focus()
			Else
				[tx_検索仕入先].Focus()
			End If
			
			'''        KeyAscii = 0
			'''        '---参照画面表示
			'''        SelectF = True
			'''        Set SirSen.ResCodeCTL = tx_検索仕入先
			'''        SirSen.Show vbModal, Me
			'''        If [tx_検索仕入先].Tag <> "" Then
			'''''            ReturnF = True
			'''            [tx_検索仕入先].SetFocus
			'''        Else
			'''            [tx_検索仕入先].SetFocus
			'''        End If
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
		Item_Check((tx_H.TabIndex))
		PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_D1_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_D1.Enter
		Item_Check((tx_D1.TabIndex))
		PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_D2_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_D2.Enter
		Item_Check((tx_D2.TabIndex))
		PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_H1_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_H1.Enter
		Item_Check((tx_H1.TabIndex))
		PreviousControl = Me.ActiveControl
	End Sub
	
	Private Sub tx_H2_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_H2.Enter
		Item_Check(([tx_H2].TabIndex))
		PreviousControl = Me.ActiveControl
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
					[tx_H2].Focus()
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
			m_DialogResultCode1 = Trim(SelListVw.Items.Item(SelListVw.FocusedItem.Index).Text)
			'UPGRADE_WARNING: コレクション SelListVw.ListItems の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
			'UPGRADE_WARNING: コレクション SelListVw.ListItems().ListSubItems の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
			m_DialogResultCode2 = Trim(SelListVw.Items.Item(SelListVw.FocusedItem.Index).SubItems.Item(1).Text)
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
		
		'tx_検索仕入先のチェック
		If ItemNo > [tx_検索仕入先].TabIndex Then
			'--- 入力値をワークへ格納
			If ISInt(([tx_検索仕入先].Text)) Then
				'UPGRADE_WARNING: TextBox プロパティ tx_検索仕入先.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				[tx_検索仕入先].Text = VB6.Format([tx_検索仕入先].Text, New String("0", [tx_検索仕入先].Maxlength))
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
		Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
		
		sql = "SELECT SE.製品NO, SE.仕様NO, SE.漢字名称,SE.W,SE.D,SE.H,SE.D1,SE.D2,SE.H1,SE.H2,SI.略称 "
		sql = sql & " FROM TM客先在庫_担当者別 AS ZK"
		sql = sql & " INNER JOIN TM製品 AS SE "
		sql = sql & " ON ZK.製品NO = SE.製品NO "
		sql = sql & " AND ZK.仕様NO = SE.仕様NO "
		sql = sql & " LEFT JOIN TM仕入先 AS SI ON SE.主仕入先CD = SI.仕入先CD"
		'    sql = sql & " WHERE ZK.担当者CD = " & SQLString(Trim$(m_担当者CD))
		'    sql = sql & " AND ZK.得意先CD = '" & SQLString(Trim$(m_得意先CD)) & "'"
		
		whr = "ZK.担当者CD = " & SQLString(Trim(CStr(m_担当者CD))) & " AND ZK.得意先CD = '" & SQLString(Trim(m_得意先CD)) & "'"
		
		If tx_検索製品.Text <> vbNullString Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & " SE.製品NO like '" & SQLString(tx_検索製品) & "%'"
		End If
		If tx_検索仕様.Text <> vbNullString Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & " SE.仕様NO LIKE '" & SQLString(tx_検索仕様) & "%'"
		End If
		If tx_検索名称.Text <> vbNullString Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & " SE.漢字名称 LIKE '%" & SQLString(tx_検索名称) & "%'"
		End If
		If [tx_検索仕入先].Text <> vbNullString Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & " 主仕入先CD = " & SQLQuoteString([tx_検索仕入先])
		End If
		
		If tx_W.Text <> vbNullString Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & " SE.W = " & tx_W.Text
		End If
		
		If tx_D.Text <> vbNullString Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & " SE.D = " & tx_D.Text
		End If
		If tx_H.Text <> vbNullString Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & " SE.H = " & tx_H.Text
		End If
		If tx_D1.Text <> vbNullString Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & " SE.D1 = " & tx_D1.Text
		End If
		If tx_D2.Text <> vbNullString Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & " SE.D2 = " & tx_D2.Text
		End If
		If tx_H1.Text <> vbNullString Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & " SE.H1 = " & tx_H1.Text
		End If
		If [tx_H2].Text <> vbNullString Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & " SE.H2 = " & [tx_H2].Text
		End If
		
		If whr <> vbNullString Then
			sql = sql & " WHERE " & whr
		End If
		
		'---2009/09/11 ADD
		''    If pwhr_ZaikKB <> vbNullString Then
		''        If whr = vbNullString Then
		''            sql = sql & " WHERE " & pwhr_ZaikKB
		''        Else
		''            sql = sql & " AND " & pwhr_ZaikKB
		''        End If
		''    End If
		
		sql = sql & " ORDER BY ZK.製品NO, ZK.仕様NO"
		
		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
		
		LockWindowUpdate(Me.Handle.ToInt32)
		
		SelListVw.Items.Clear()
		
		Do Until rs.EOF
			Buf = New String("0", 7) & "0" 'ひとつ余計にする
			Buf = Space(1) & CStr(rs.Fields(0).Value)
			itmX = SelListVw.Items.Add(Buf)
			Buf = New String("0", 7) & "0" 'ひとつ余計にする
			Buf = Space(1) & CStr(rs.Fields(1).Value)
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(CStr(NullToZero((rs.Fields(1).Value), "")))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(CStr(NullToZero((rs.Fields(2).Value), "")))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(VB6.Format(NullToZero((rs.Fields(3).Value), ""), "#"))
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
			itmX.SubItems.Add(CStr(NullToZero((rs.Fields(10).Value), "")))
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
		FindListText = Space(1) & CStr(tx_検索製品.Text)
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
		Me.Cursor = System.Windows.Forms.Cursors.Default
		
		'データ件数セット
		If SelListVw.Items.Count <> 0 Then
			lbListCount.Text = VB6.Format(SelListVw.Items.Count, "#,##0")
		Else
			Exit Function
		End If
		
		Download = True
	End Function
End Class