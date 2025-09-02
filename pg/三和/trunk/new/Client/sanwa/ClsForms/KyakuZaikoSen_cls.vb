Option Explicit On

Imports System.Runtime.InteropServices

''' <summary>
''' 
''' </summary>
Public Class KyakuZaikoSen_cls

	'フォームの再描画用のAPI定義
	'Private Declare Function LockWindowUpdate Lib "user32" (ByVal Hwnd As Integer) As Integer
	'リストビュー設定用のAPI定義
	'Private Declare Function GetWindowLong Lib "user32"  Alias "GetWindowLongA"(ByVal Hwnd As Integer, ByVal nlndex As Integer) As Integer
	'Private Declare Function SetWindowLong Lib "user32"  Alias "SetWindowLongA"(ByVal Hwnd As Integer, ByVal nindex As Integer, ByVal dwNewLong As Integer) As Integer
	'UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
	'Private Declare Function SendMessage Lib "user32"  Alias "SendMessageA"(ByVal Hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer, ByRef lParam As Any) As Integer

	' フォームの再描画用のAPI定義
	<DllImport("user32.dll", SetLastError:=True)>
	Private Shared Function LockWindowUpdate(ByVal hwndLock As IntPtr) As Boolean
	End Function

	' リストビュー設定用のAPI定義
	<DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
	Private Shared Function GetWindowLong(ByVal hwnd As IntPtr, ByVal nIndex As Integer) As Integer
	End Function

	<DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
	Private Shared Function SetWindowLong(ByVal hwnd As IntPtr, ByVal nIndex As Integer, ByVal dwNewLong As Integer) As Integer
	End Function

	<DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
	Private Shared Function SendMessage(ByVal hwnd As IntPtr, ByVal msg As UInteger, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
	End Function

	'リストビュー設定用の定数
	Private Const GWL_STYLE As Short = (-16)
	Private Const LVM_FIRST As Integer = &H1000
	Private Const LVM_GETHEADER As UInteger = (LVM_FIRST + 31)
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
	Public m_DialogResult As Boolean
	Public m_DialogResultCode1 As String
	Public m_DialogResultCode2 As String

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

	'  '------------------------------------------
	'  '   選択画面セットアップ
	'  '       Title       :選択画面のCaption
	'  '       ResCtl      :選択コードを送るコントロールをセット
	'  '       sql         :リストセット用レコードソース
	'  '       Mode        :直送先選択
	'  '       FieldsWidth :sqlで指定した項目分のリスト幅をセット(NULLにすると自動)
	'  '------------------------------------------
	'  Public Function SelSetup(Title As String, sql As String _
	'  '                        , Mode As String _
	'  '                        , ParamArray FieldsWidth())
	'  
	'      Dim i As Integer
	'  
	'  '''    pSel_FormTitle = Title
	'  '''    psel_SQL = sql
	'  '''    pSel_Mode = Mode
	'  '''    ReDim List_ColWidth(0 To UBound(FieldsWidth))
	'  '''
	'  '''    For i = 0 To UBound(FieldsWidth)
	'  '''        List_ColWidth(i) = FieldsWidth(i)
	'  '''    Next
	'  
	'  End Function

	'UPGRADE_NOTE: DialogResult は DialogResult_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
	Public ReadOnly Property DialogResult_Renamed() As Boolean
		Get
			Return m_DialogResult
		End Get
	End Property

	Public ReadOnly Property DialogResultCode1() As String
		Get
			Return m_DialogResultCode1
		End Get
	End Property

	Public ReadOnly Property DialogResultCode2() As String
		Get
			Return m_DialogResultCode2
		End Get
	End Property

	Private Sub SetFlatHeader(ByRef Target As System.Windows.Forms.ListView)
		'指定されたリストビューのヘッダを平面（フラット）にする
		Dim lngHeader As IntPtr
		Dim lngStyle As Integer
		Dim lngAPIReVal As Integer

		'ヘッダーのハンドルを取得
		lngHeader = SendMessage(Target.Handle, LVM_GETHEADER, IntPtr.Zero, IntPtr.Zero)
		'ヘッダのウィンドウスタイルを取得
		lngStyle = GetWindowLong(lngHeader, GWL_STYLE)
		'ヘッダをフラットスタイルに設定
		lngAPIReVal = SetWindowLong(lngHeader, GWL_STYLE, lngStyle Xor HDS_BUTTONS)
	End Sub

	Private Sub KyakuZaikoSen_cls_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
		Dim Cancel As Boolean = e.Cancel
		Dim UnloadMode As System.Windows.Forms.CloseReason = e.CloseReason
		'    Set ResultCodeSetControl1 = Nothing
		'    Set ResultCodeSetControl2 = Nothing
		'UPGRADE_NOTE: オブジェクト KyakuZaikoSen_cls をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		'Me = Nothing
		Me.Dispose()
		e.Cancel = Cancel
	End Sub

	'UPGRADE_WARNING: イベント KyakuZaikoSen_cls.Resize は、フォームが初期化されたときに発生します。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"' をクリックしてください。
	Private Sub KyakuZaikoSen_cls_Resize(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Me.Resize
		If Me.WindowState <> System.Windows.Forms.FormWindowState.Minimized Then
			'フォーム最小（幅）制御
			If VB6Conv.PixelsToTwipsX(Me.Width) < MeWidth Then
				Me.Width = VB6Conv.TwipsToPixelsX(MeWidth)
			End If
			'フォーム最小（高さ）制御・リストビューの高さをフォームの高さに比例
			If VB6Conv.PixelsToTwipsY(Me.Height) < MeHeightLimit Then
				Me.Height = VB6Conv.TwipsToPixelsY(MeHeightLimit)
			End If
			'リストビューの高さ・ボタン位置
			WkHeight = VB6Conv.PixelsToTwipsY(SelListVw.Top) + VB6Conv.PixelsToTwipsY(SelListVw.Height)
			WkHeight = VB6Conv.PixelsToTwipsY(CmdOk.Top) - WkHeight
			SelListVw.Width = VB6Conv.TwipsToPixelsX(VB6Conv.PixelsToTwipsX(Me.ClientRectangle.Width) - (VB6Conv.PixelsToTwipsX(SelListVw.Left) * 2)) '2012/09/13 ADD
			SelListVw.Height = VB6Conv.TwipsToPixelsY(VB6Conv.PixelsToTwipsY(Me.ClientRectangle.Height) - VB6Conv.PixelsToTwipsY(SelListVw.Top) - (WkHeight * 2) - VB6Conv.PixelsToTwipsY(CmdOk.Height))
			CmdOk.Top = VB6Conv.TwipsToPixelsY(VB6Conv.PixelsToTwipsY(SelListVw.Height) + VB6Conv.PixelsToTwipsY(SelListVw.Top) + WkHeight)
			CmdCan.Top = VB6Conv.TwipsToPixelsY(VB6Conv.PixelsToTwipsY(SelListVw.Height) + VB6Conv.PixelsToTwipsY(SelListVw.Top) + WkHeight)
		End If
	End Sub

	Private Sub KyakuZaikoSen_cls_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Me.Load
		'Dim i, j As Integer
		'Dim List_ColLen As Integer

		On Error GoTo Form_Load_Err

		'クラス生成
		cSiiresaki = New clsSiiresaki

		'リストヘッダー項目セット
		With SelListVw
			.Columns.Clear()
			.Columns.Add("", "製品NO", CInt(VB6Conv.TwipsToPixelsX(1000)))
			.Columns.Add("", "仕様NO", CInt(VB6Conv.TwipsToPixelsX(1000)))
			.Columns.Add("", "漢字名称", CInt(VB6Conv.TwipsToPixelsX(4600)))
			.Columns.Add("", "Ｗ", CInt(VB6Conv.TwipsToPixelsX(750)))
			.Columns.Add("", "Ｄ", CInt(VB6Conv.TwipsToPixelsX(750)))
			.Columns.Add("", "Ｈ", CInt(VB6Conv.TwipsToPixelsX(750)))
			.Columns.Add("", "Ｄ１", CInt(VB6Conv.TwipsToPixelsX(750)))
			.Columns.Add("", "Ｄ２", CInt(VB6Conv.TwipsToPixelsX(750)))
			.Columns.Add("", "Ｈ１", CInt(VB6Conv.TwipsToPixelsX(750)))
			.Columns.Add("", "Ｈ２", CInt(VB6Conv.TwipsToPixelsX(750)))
			.Columns.Add("", "仕入先名", CInt(VB6Conv.TwipsToPixelsX(1450)))
		End With

		SelListVw.Width = VB6Conv.TwipsToPixelsX(13300 + 330) 'スクロールバー分プラス

		'リストビューのヘッダを平面にする
		Call SetFlatHeader(SelListVw)

		wkWidth = (VB6Conv.PixelsToTwipsX(Me.SelListVw.Left) * 2) + VB6Conv.PixelsToTwipsX(SelListVw.Width) 'wkフォーム幅セット
		WidthSa = VB6Conv.PixelsToTwipsX(Me.Width) - VB6Conv.PixelsToTwipsX(Me.ClientRectangle.Width) 'フォームWidthの内外値差

		If wkWidth < 4940 Then
			wkWidth = 4940
		End If

		[CmdFind].Left = VB6Conv.TwipsToPixelsX(wkWidth - (VB6Conv.PixelsToTwipsX(SelListVw.Left) + VB6Conv.PixelsToTwipsX([CmdFind].Width))) '検索ボタン位置決定
		CmdOk.Left = VB6Conv.TwipsToPixelsX(wkWidth - (VB6Conv.PixelsToTwipsX(SelListVw.Left) + VB6Conv.PixelsToTwipsX(CmdOk.Width) + VB6Conv.PixelsToTwipsX(CmdCan.Width))) 'ＯＫボタン位置決定
		CmdCan.Left = VB6Conv.TwipsToPixelsX(wkWidth - (VB6Conv.PixelsToTwipsX(SelListVw.Left) + VB6Conv.PixelsToTwipsX(CmdCan.Width))) 'キャンセルボタン位置決定
		Me.Width = VB6Conv.TwipsToPixelsX(wkWidth + WidthSa) 'フォーム幅決定(Width)

		'フォームを画面の中央に配置
		Me.Top = VB6Conv.TwipsToPixelsY((VB6Conv.PixelsToTwipsY(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height) - VB6Conv.PixelsToTwipsY(Me.Height)) \ 2)
		Me.Left = VB6Conv.TwipsToPixelsX((VB6Conv.PixelsToTwipsX(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width) - VB6Conv.PixelsToTwipsX(Me.Width)) \ 2)
		'項目クリア
		Call SetupBlank()

		MeWidth = VB6Conv.PixelsToTwipsX(Me.Width)
		MeHeight = VB6Conv.PixelsToTwipsY(Me.Height) - VB6Conv.PixelsToTwipsY(SelListVw.Height)
		LvHeightLimit = SelListVw.Font.SizeInPoints * (34 + 24) '(列見出し + 明細一行分）
		MeHeightLimit = MeHeight + LvHeightLimit

		''    ResultCodeSetControl1.Tag = ""
		''    ResultCodeSetControl2.Tag = ""

		Me.KeyPreview = True

		Exit Sub
Form_Load_Err:
		MsgBox(Err.Number & " " & Err.Description)
		Exit Sub
	End Sub

	Private Sub KyakuZaikoSen_cls_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
		Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
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
		Try
			ClearControls(Me)
			SelListVw.Items.Clear()
		Catch ex As Exception
			MessageBox.Show(Err.Number.ToString() & " " & Err.GetException().Message)
		End Try
	End Sub

	Private Sub ClearControls(ByVal parent As Control)
		For Each ctl As Control In parent.Controls
			'UPGRADE_WARNING: TypeOf に新しい動作が指定されています。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
			If TypeOf ctl Is System.Windows.Forms.TextBox Then
				ctl.Text = vbNullString
			End If
			'UPGRADE_WARNING: TypeOf に新しい動作が指定されています。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
			If TypeOf ctl Is ExText.ExTextBox Then
				ctl.Text = vbNullString
			End If
			'UPGRADE_WARNING: TypeOf に新しい動作が指定されています。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
			If TypeOf ctl Is ExNmText.ExNmTextBox Then
				ctl.Text = vbNullString
			End If
			'UPGRADE_WARNING: TypeOf に新しい動作が指定されています。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
			If TypeOf ctl Is System.Windows.Forms.Label Then
				If ctl.Name Like "rf_*" Then
					ctl.Text = vbNullString
				End If
			End If

			' 子コントロールがある場合、再帰的に処理
			If ctl.HasChildren Then
			    ClearControls(ctl)
			End If
		Next
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
		Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
		If KeyAscii = Asc(" ") And ([tx_検索仕入先].SelStart = 0 And [tx_検索仕入先].SelLength = Len([tx_検索仕入先].Text)) Then
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

			'        KeyAscii = 0
			'        '---参照画面表示
			'        SelectF = True
			'        Set SirSen.ResCodeCTL = tx_検索仕入先
			'        SirSen.Show vbModal, Me
			'        If [tx_検索仕入先].Tag <> "" Then
			' ''            ReturnF = True
			'            [tx_検索仕入先].SetFocus
			'        Else
			'            [tx_検索仕入先].SetFocus
			'        End If
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

	Private Sub CmdFind_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdFind.Enter
		Item_Check(([CmdFind].TabIndex))
	End Sub

	Private Sub CmdFind_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdFind.Click
		On Error GoTo CmdFind_Click_Err

		If Not Download() Then
			CheckAlarm("該当データがありません。")
			lbListCount.Text = ""
			tx_検索製品.Focus()
			Exit Sub
		End If

		'検索ＯＫ処理
		CLKFLG = True
		SelListVw.Focus()

		Exit Sub
CmdFind_Click_Err:
		Me.Cursor = System.Windows.Forms.Cursors.Default
		MsgBox(Err.Number & " " & Err.Description)
		Exit Sub
	End Sub

	Private Sub CmdOk_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdOk.Click
		On Error GoTo CmdOk_Click_Err
		'--- リスト選択
		SetResultValues()

		Exit Sub
CmdOk_Click_Err:
		MsgBox(Err.Number & " " & Err.Description)
		Exit Sub
	End Sub

	Private Sub CmdCan_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdCan.Click
		On Error GoTo CmdCan_Click_Err
		Me.Close()
		Exit Sub
CmdCan_Click_Err:
		MsgBox(Err.Number & " " & Err.Description)
		Exit Sub
	End Sub

	Private Sub SelListVw_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles SelListVw.Enter
		If SelListVw.Items.Count = 0 Then
			[CmdFind].Focus()
		Else
			If SelListVw.FocusedItem IsNot Nothing Then
				SelListVw.FocusedItem.Selected = True
			Else
				SelListVw.Items(0).Selected = True
			End If
		End If
	End Sub

	Private Sub SelListVw_DoubleClick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles SelListVw.DoubleClick
		CLKFLG = True
		SetResultValues()
	End Sub

	'UPGRADE_ISSUE: MSComctlLib.ListView イベント SelListVw.ItemClick はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="ABD9AF39-7E24-4AFF-AD8D-3675C1AA3054"' をクリックしてください。
	'Private Sub SelListVw_ItemClick(ByVal Item As System.Windows.Forms.ListViewItem)
	'	CLKFLG = True
	'End Sub

	Private Sub SelListVw_MouseClick(sender As Object, e As MouseEventArgs) Handles SelListVw.MouseClick
		CLKFLG = True
	End Sub

	Private Sub SelListVw_MouseDown(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles SelListVw.MouseDown
		Dim Button As Integer = eventArgs.Button \ &H100000
		Dim Shift As Integer = System.Windows.Forms.Control.ModifierKeys \ &H10000
		Dim X As Single = VB6Conv.PixelsToTwipsX(eventArgs.X)
		Dim Y As Single = VB6Conv.PixelsToTwipsY(eventArgs.Y)
		CLKFLG = False
	End Sub

	Private Sub SelListVw_KeyDown(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyEventArgs) Handles SelListVw.KeyDown
		Dim KeyCode As Integer = eventArgs.KeyCode
		Dim Shift As Integer = eventArgs.KeyData \ &H10000
		Select Case KeyCode
			Case System.Windows.Forms.Keys.Return
				KeyCode = 0
				CLKFLG = True
				SetResultValues()
			Case System.Windows.Forms.Keys.Up
				If SelListVw.FocusedItem.Index = 0 Then
					KeyCode = 0
					[tx_H2].Focus()
				End If
		End Select
	End Sub

	Private Sub SelListVw_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles SelListVw.KeyPress
		Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
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

	Private Function Item_Check(ByRef ItemNo As Integer) As Boolean

		On Error GoTo Item_Check_Err

		' ''    'キー項目「検索製品」のチェック
		' ''    If ItemNo > [tx_検索製品].TabIndex Then
		' ''        '--- 入力値をワークへ格納
		' ''        If ISInt([tx_検索製品].Text) Then
		' ''            [tx_検索製品].Text = Format([tx_検索製品].Text, String(tx_検索製品.MaxLength, "0"))
		' ''        End If
		' ''    End If
		' ''
		' ''    'キー項目「ID」のチェック
		' ''    If ItemNo > [tx_検索仕様].TabIndex Then
		' ''        '--- 入力値をワークへ格納
		' ''        If ISInt([tx_検索仕様].Text) Then
		' ''            [tx_検索仕様].Text = Format([tx_検索仕様].Text, String(tx_検索仕様.MaxLength, "0"))
		' ''        End If
		' ''    End If
		' ''
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
				[tx_検索仕入先].Text = CType([tx_検索仕入先].Text, Integer).ToString(New String("0"c, [tx_検索仕入先].MaxLength))
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
		'Dim i As Integer
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
			whr = whr & " SE.製品NO like '" & SQLString(tx_検索製品.Text) & "%'"
		End If
		If tx_検索仕様.Text <> vbNullString Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & " SE.仕様NO LIKE '" & SQLString(tx_検索仕様.Text) & "%'"
		End If
		If tx_検索名称.Text <> vbNullString Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & " SE.漢字名称 LIKE '%" & SQLString(tx_検索名称.Text) & "%'"
		End If
		If [tx_検索仕入先].Text <> vbNullString Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & " 主仕入先CD = " & SQLQuoteString([tx_検索仕入先].Text)
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

		LockWindowUpdate(Me.Handle)

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
			itmX.SubItems.Add(CType(NullToZero((rs.Fields(3).Value), ""), Integer).ToString("#"))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(CType(NullToZero((rs.Fields(4).Value), ""), Integer).ToString("#"))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(CType(NullToZero((rs.Fields(5).Value), ""), Integer).ToString("#"))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(CType(NullToZero((rs.Fields(6).Value), ""), Integer).ToString("#"))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(CType(NullToZero((rs.Fields(7).Value), ""), Integer).ToString("#"))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(CType(NullToZero((rs.Fields(8).Value), ""), Integer).ToString("#"))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(CType(NullToZero((rs.Fields(9).Value), ""), Integer).ToString("#"))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			itmX.SubItems.Add(CStr(NullToZero((rs.Fields(10).Value), "")))
			rs.MoveNext()
		Loop
		ReleaseRs(rs)
		'UPGRADE_NOTE: オブジェクト itmX をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		itmX = Nothing

		'UPGRADE_ISSUE: MSComctlLib.ListView プロパティ SelListVw.SortKey はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'NOTE SS SelListVw.SortKey = 0 (意味は0番目のカラムに相当)
		SelListVw.SortStyle = SortableListView.SortStyles.SortSelectedColumn
		SelListVw.Sort()
		SelListVw.Refresh()

		'選択されているテキストを保持する
		FindListText = Space(1) & CStr(tx_検索製品.Text)
		'NOTE SS itmFound = SelListVw.FindItemWithText(FindListText, True, 0, MSComctlLib.ListFindItemHowConstants.lvwWhole)
		itmFound = SelListVw.FindItemWithText(FindListText)

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
			lbListCount.Text = SelListVw.Items.Count.ToString("#,##0")
		Else
			Exit Function
		End If

		Download = True
	End Function
End Class