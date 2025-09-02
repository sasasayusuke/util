Option Explicit On

Imports System.Runtime.InteropServices

''' <summary>
''' 
''' </summary>
Public Class SelKokTmp_cls

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

	'Dim pTOKUCD     As String
	'Dim pTOKUNM     As String
	Dim CLKFLG As Boolean 'リストビュー制御用

	Dim PreviousControl As System.Windows.Forms.Control 'コントロールの戻りを制御
	Dim sql As String
	Dim rs As ADODB.Recordset
	Dim ResultCodeSetControl As System.Windows.Forms.Control '選択したコードの送り先をセットする。

	Dim MeWidth, MeHeight As Integer
	Dim wkWidth As Short
	Dim WidthSa As Short
	Dim WkHeight As Integer
	Dim LvHeightLimit, MeHeightLimit As Integer
	Dim SelectF As Boolean

	'クラス
	Private cTokuisaki As clsTokuisaki

	Private m_得意先CD As String

	'値返還用
	Public m_DialogResult As Boolean
	Public m_DialogResultCode As String

	'UPGRADE_NOTE: DialogResult は DialogResult_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
	Public ReadOnly Property DialogResult_Renamed() As Boolean
		Get
			Return m_DialogResult
		End Get
	End Property

	Public ReadOnly Property DialogResultCode() As String
		Get
			Return m_DialogResultCode
		End Get
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

	Private Sub SelKokTmp_cls_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
		Dim Cancel As Boolean = e.Cancel
		Dim UnloadMode As System.Windows.Forms.CloseReason = e.CloseReason
		'    Set ResultCodeSetControl = Nothing
		'UPGRADE_NOTE: オブジェクト SelKokTmp_cls をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		'Me = Nothing
		'Me.Dispose()
		e.Cancel = Cancel
	End Sub

	'UPGRADE_WARNING: イベント SelKokTmp_cls.Resize は、フォームが初期化されたときに発生します。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"' をクリックしてください。
	Private Sub SelKokTmp_cls_Resize(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Resize
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

	Private Sub SelKokTmp_cls_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Me.Load
		'Dim i, j As Integer
		'Dim List_ColLen As Integer

		On Error GoTo Form_Load_Err

		'リストヘッダー項目セット
		With SelListVw
			.Columns.Clear()
			.Columns.Add("", "テンプレート名", CInt(VB6Conv.TwipsToPixelsX(4600)))
		End With

		SelListVw.Width = VB6Conv.TwipsToPixelsX(4600 + 330) 'スクロールバー分プラス

		'得意先セット
		''    tx_得意先CD = pTOKUCD
		''    rf_得意先名 = pTOKUNM
		'クラス生成
		cTokuisaki = New clsTokuisaki

		'得意先セット
		[tx_得意先CD].Text = Me.得意先CD
		If Me.得意先CD = "" Then
			[rf_得意先名].Text = ""
		Else
			With cTokuisaki
				.Initialize()
				.得意先CD = Me.得意先CD
				If .GetbyID = False Then
					rf_得意先名.Text = ""
				Else
					[rf_得意先名].Text = .得意先名1
				End If
			End With
		End If
		'リストビューのヘッダを平面にする
		Call SetFlatHeader(SelListVw)

		wkWidth = (VB6Conv.PixelsToTwipsX(Me.SelListVw.Left) * 2) + VB6Conv.PixelsToTwipsX(SelListVw.Width) 'wkフォーム幅セット
		WidthSa = VB6Conv.PixelsToTwipsX(Me.Width) - VB6Conv.PixelsToTwipsX(Me.ClientRectangle.Width) 'フォームWidthの内外値差

		If wkWidth < 7200 Then
			wkWidth = 7200
		End If

		[cmdFind].Left = VB6Conv.TwipsToPixelsX(wkWidth - (VB6Conv.PixelsToTwipsX(SelListVw.Left) + VB6Conv.PixelsToTwipsX([cmdFind].Width))) '検索ボタン位置決定
		CmdOk.Left = VB6Conv.TwipsToPixelsX(wkWidth - (VB6Conv.PixelsToTwipsX(SelListVw.Left) + VB6Conv.PixelsToTwipsX(CmdOk.Width) + VB6Conv.PixelsToTwipsX(CmdCan.Width))) 'ＯＫボタン位置決定
		CmdCan.Left = VB6Conv.TwipsToPixelsX(wkWidth - (VB6Conv.PixelsToTwipsX(SelListVw.Left) + VB6Conv.PixelsToTwipsX(CmdCan.Width))) 'キャンセルボタン位置決定
		Me.Width = VB6Conv.TwipsToPixelsX(wkWidth + WidthSa) 'フォーム幅決定(Width)

		'フォームを画面の中央に配置
		Me.Top = VB6Conv.TwipsToPixelsY((VB6Conv.PixelsToTwipsY(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height) - VB6Conv.PixelsToTwipsY(Me.Height)) \ 2)
		Me.Left = VB6Conv.TwipsToPixelsX((VB6Conv.PixelsToTwipsX(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width) - VB6Conv.PixelsToTwipsX(Me.Width)) \ 2)
		'項目クリア
		[tx_検索名称].Text = vbNullString

		MeWidth = VB6Conv.PixelsToTwipsX(Me.Width)
		MeHeight = VB6Conv.PixelsToTwipsY(Me.Height) - VB6Conv.PixelsToTwipsY(SelListVw.Height)
		LvHeightLimit = SelListVw.Font.SizeInPoints * (34 + 24) '(列見出し + 明細一行分）
		MeHeightLimit = MeHeight + LvHeightLimit

		'    ResultCodeSetControl.Tag = ""

		Me.KeyPreview = True

		Exit Sub
Form_Load_Err:
		MsgBox(Err.Number & " " & Err.Description)
		Exit Sub
	End Sub

	Private Sub SelKokTmp_cls_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
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

	Private Sub tx_検索名称_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_検索名称.Enter
		Item_Check(([tx_検索名称].TabIndex))
		PreviousControl = Me.ActiveControl
	End Sub

	Private Sub CmdFind_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdFind.Enter
		Item_Check(([cmdFind].TabIndex))
	End Sub

	Private Sub CmdFind_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdFind.Click
		On Error GoTo CmdFind_Click_Err

		If Not Download() Then
			CheckAlarm("該当データがありません。")
			lbListCount.Text = ""
			[tx_検索名称].Focus()
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
		On Error GoTo CmdOK_Click_Err
		'--- リスト選択
		SetResultValues()

		Exit Sub
CmdOK_Click_Err:
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
			[cmdFind].Focus()
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
					[tx_検索名称].Focus()
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
			' ''                If Not (ResultCodeSetControl Is Nothing) Then
			' ''                    With ResultCodeSetControl
			' ''                        .Text = Trim$(SelListVw.ListItems(SelListVw.SelectedItem.Index))
			' ''                        .Tag = "True"
			' ''                    End With
			' ''                End If
			m_DialogResult = True
			'UPGRADE_WARNING: コレクション SelListVw.ListItems の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
			m_DialogResultCode = Trim(SelListVw.Items.Item(SelListVw.FocusedItem.Index).Text)
		End If
		Me.Close()
	End Sub

	Private Function Item_Check(ByRef ItemNo As Integer) As Boolean

		On Error GoTo Item_Check_Err

		''    'tx_検索得意先のチェック
		''    If ItemNo > [tx_検索得意先].TabIndex Then
		''        '--- 入力値をワークへ格納
		''        If ISInt([tx_検索得意先].Text) Then
		''            [tx_検索得意先].Text = Format([tx_検索得意先].Text, String(tx_検索得意先.MaxLength, "0"))
		''        End If
		''    End If

		Exit Function
Item_Check_Err:
		MsgBox(Err.Number & " " & Err.Description)
	End Function

	Private Function Download() As Boolean
		Dim itmX As System.Windows.Forms.ListViewItem
		Dim itmFound As System.Windows.Forms.ListViewItem ' FoundItem 変数。
		Dim FindListText As String
		'Dim Buf As String '文字成型用
		'Dim i As Integer
		Dim whr As String

		Download = False
		Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

		sql = "SELECT テンプレート名 " & "FROM TM顧客テンプレート "

		whr = "得意先CD = '" & [tx_得意先CD].Text & "' And 行NO = 1"

		If [tx_検索名称].Text <> vbNullString Then
			If whr <> "" Then
				whr = whr & " and "
			End If
			whr = whr & " テンプレート名 LIKE '%" & SQLString([tx_検索名称].Text) & "%'"
		End If

		If whr <> vbNullString Then
			sql = sql & " WHERE " & whr
		End If

		sql = sql & " ORDER BY テンプレート名"

		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockReadOnly)

		LockWindowUpdate(Me.Handle)

		SelListVw.Items.Clear()

		Do Until rs.EOF
			itmX = SelListVw.Items.Add(CStr(rs.Fields(0).Value) & " No", CStr(rs.Fields(0).Value), "")
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
		FindListText = Space(1) & CStr([tx_検索名称].Text)
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

	'' 
	'' '------------------------------------------
	'' '   選択画面セットアップ
	'' '       ctl         :選択コードを送るコントロールをセット
	'' '       TOKUID      :得意先ID
	'' '       TOKUNM      :得意先名
	'' '------------------------------------------
	'' Public Function SelSetup(ctl As Control, TOKUCD As String, TOKUNM As String)
	'' 
	''     Set ResultCodeSetControl = ctl
	''     pTOKUCD = TOKUCD
	''     pTOKUNM = TOKUNM
	'' 
	'' End Function
End Class