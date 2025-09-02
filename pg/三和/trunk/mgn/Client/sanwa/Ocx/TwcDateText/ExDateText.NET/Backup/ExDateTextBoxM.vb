Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
<System.Runtime.InteropServices.ProgId("ExDateTextBoxM_NET.ExDateTextBoxM")> Public Class ExDateTextBoxM
	Inherits System.Windows.Forms.UserControl
	Public Event EditModeChange()
	Public Event SelStartChange()
	Public Event CanForwardSetFocusChange()
	Public Event SelTextChange()
	Public Event ForeColorChange()
	Public Event BorderStyleChange()
	Public Event OldValueChange()
	Public Event AlignmentChange()
	Public Event TextChange()
	Public Event AppearanceChange()
	Public Event LockedChange()
	Public Event FocusBackColorChange()
	Public Event CanNextSetFocusChange()
	Public Event SelectTextChange()
	Public Event MaxLengthChange()
	Public Event EnabledChange()
	Public Event SelLengthChange()
	Public Event FontChange()
	Public Event BackColorChange()
	
	'UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
	Private Declare Function SendMessage Lib "user32"  Alias "SendMessageA"(ByVal Hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer, ByRef lParam As Any) As Integer
	''Private Const EM_CANUNDO = &HC6         'UNDO可能か
	''Private Const EM_UNDO = &HC7            'UNDOする
	''Private Const EM_EMPTYUNDOBUFFER = &HCD
	Private Const WM_KEYDOWN As Integer = &H100
	''Private Const EM_GETMODIFY = &HB8       '内容が変更されたかどうか
	
	'' ウィンドウへメッセージをポストするAPI
	'UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
	Private Declare Function PostMessage Lib "user32"  Alias "PostMessageA"(ByVal Hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer, ByRef lParam As Any) As Integer
	''Private Const WM_KEYDOWN = &H100
	''Private Const WM_KEYUP = &H101
	''Private Const VK_TAB = &H9
	'キーを送るAPI
	Private Declare Sub keybd_event Lib "user32" (ByVal bVk As Byte, ByVal bScan As Byte, ByVal dwFlags As Integer, ByVal dwExtraInfo As Integer)
	Private Const KEYEVENTF_KEYUP As Integer = &H2
	
	'列挙型
	'Enum AppearanceType
	'    フラット = 0
	'    立体 = 1
	'End Enum
	'Enum BorderStyleType
	'    なし = 0
	'    実線 = 1
	'End Enum
	
	'プロパティ変数
	Private pFocusBackColor As System.Drawing.Color
	Private hBackColor As System.Drawing.Color
	Private pCanNextSetFocus As Boolean
	Private pCanForwardSetFocus As Boolean
	Private pSelectText As Boolean
	Private pEditMode As Boolean
	
	'変数
	Private fGotFocus As Boolean
	Private fClicking As Boolean
	Private vUNDOBUF As Object
	Private MyParentName As Integer
	
	'イベント
	Public Event Change(ByVal Sender As System.Object, ByVal e As System.EventArgs)
	Public Shadows Event Click(ByVal Sender As System.Object, ByVal e As System.EventArgs)
	Public Event DblClick(ByVal Sender As System.Object, ByVal e As System.EventArgs)
	Public Shadows Event KeyDown(ByVal Sender As System.Object, ByVal e As KeyDownEventArgs)
	Public Shadows Event KeyPress(ByVal Sender As System.Object, ByVal e As KeyPressEventArgs)
	Public Shadows Event KeyUp(ByVal Sender As System.Object, ByVal e As KeyUpEventArgs)
	Public Shadows Event MouseDown(ByVal Sender As System.Object, ByVal e As MouseDownEventArgs)
	Public Shadows Event MouseMove(ByVal Sender As System.Object, ByVal e As MouseMoveEventArgs)
	Public Shadows Event MouseUp(ByVal Sender As System.Object, ByVal e As MouseUpEventArgs)
	Public Event OLECompleteDrag(ByVal Sender As System.Object, ByVal e As OLECompleteDragEventArgs)
	Public Event OLEDragDrop(ByVal Sender As System.Object, ByVal e As OLEDragDropEventArgs)
	Public Event OLEDragOver(ByVal Sender As System.Object, ByVal e As OLEDragOverEventArgs)
	Public Event OLEGiveFeedback(ByVal Sender As System.Object, ByVal e As OLEGiveFeedbackEventArgs)
	Public Event OLESetData(ByVal Sender As System.Object, ByVal e As OLESetDataEventArgs)
	Public Event OLEStartDrag(ByVal Sender As System.Object, ByVal e As OLEStartDragEventArgs)
	
	Public Event RtnKeyDown(ByVal Sender As System.Object, ByVal e As RtnKeyDownEventArgs)
	
	Public Sub Undo()
		'    Dim lret As Long
		
		'    lret = SendMessage(ExDateTextM.Hwnd, EM_CANUNDO, 0, 0)
		'    If lret <> 0 Then
		'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		ExDateTextM.Text = vUNDOBUF
		'    End If
		'    Call SendMessage(ExDateTextM.Hwnd, EM_UNDO, 0, ByVal 0&)
		'    Call EmptyUndoBuffer
	End Sub
	
	Public Sub EmptyUndoBuffer()
		'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		vUNDOBUF = ExDateTextM.Text
		'    Call SendMessage(ExDateTextM.Hwnd, EM_EMPTYUNDOBUFFER, 0, ByVal 0&)
	End Sub
	
	'UPGRADE_WARNING: イベント ExDateTextM.TextChanged は、フォームが初期化されたときに発生します。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"' をクリックしてください。
	Private Sub ExDateTextM_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ExDateTextM.TextChanged
		RaiseEvent Change(Me, Nothing)
	End Sub
	
	Private Sub ExDateTextM_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ExDateTextM.Click
		RaiseEvent Click(Me, Nothing)
	End Sub
	
	Private Sub ExDateTextM_DoubleClick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ExDateTextM.DoubleClick
		RaiseEvent DblClick(Me, Nothing)
	End Sub
	
	Private Sub ExDateTextM_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ExDateTextM.Enter
		ExDateTextM.BackColor = pFocusBackColor
		
		On Error Resume Next
		If Me.Hwnd <> MyParentName Then
			'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			vUNDOBUF = ExDateTextM.Text
		End If
		If Err.Number <> 0 Then
			'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			vUNDOBUF = ExDateTextM.Text
		End If
		Err.Clear()
		On Error GoTo 0
		
		If pSelectText Then
			If fClicking = False Then
				ExDateTextM.SelectionStart = 0
				ExDateTextM.SelectionLength = Len(ExDateTextM.Text)
			End If
		End If
	End Sub
	
	Private Sub ExDateTextM_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ExDateTextM.Leave
		ExDateTextM.BackColor = hBackColor
		On Error Resume Next
		'UPGRADE_WARNING: Control プロパティ UserControl.Parent は、新しい動作をもつ UserControl.FindForm にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="DFCDE711-9694-47D7-9C50-45A99CD8E91E"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト UserControl.Parent.ActiveControl の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_ISSUE: Control Hwnd は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		MyParentName = MyBase.FindForm.ActiveControl.Hwnd
		Err.Clear()
		On Error GoTo 0
	End Sub
	
	Private Sub ExDateTextM_KeyDown(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyEventArgs) Handles ExDateTextM.KeyDown
		Dim KeyCode As Short = eventArgs.KeyCode
		Dim Shift As Short = eventArgs.KeyData \ &H10000
		Dim Rtn_Cancel As Boolean
		''    If Shift = 0 Then
		''        Select Case KeyCode
		''            Case vbKeyReturn
		''                RaiseEvent RtnKeyDown
		''            Case vbKeySpace
		''                RaiseEvent SpcKeyDown
		''            Case vbKeyDelete, vbKeyBack
		''                If Len(ExDateTextM.Text) <> 0 Then
		''                    If Len(ExDateTextM.Text) <> Len(ExDateTextM.SelText) Then
		''                        ExDateTextM.SelStart = Len(ExDateTextM.Text) - 1
		''                        ExDateTextM.SelLength = Len(ExDateTextM.Text)
		''                    End If
		''                End If
		''        End Select
		''    End If
		If Shift = 0 Then
			Select Case KeyCode
				Case System.Windows.Forms.Keys.Return
					RaiseEvent RtnKeyDown(Me, New RtnKeyDownEventArgs(KeyCode, Shift, Rtn_Cancel))
			End Select
		End If
		RaiseEvent KeyDown(Me, New KeyDownEventArgs(KeyCode, Shift))
		
		If KeyCode = System.Windows.Forms.Keys.Return And Rtn_Cancel = False Then
			If pCanNextSetFocus = True Then
				KeyCode = 0
				Call NextSetFocus()
			Else
				KeyCode = 0
			End If
		ElseIf KeyCode = System.Windows.Forms.Keys.Down Then 
			If pCanNextSetFocus = True And CtlCursorCondition(ExDateTextM) = -1 Then
				KeyCode = 0
				Call NextSetFocus()
			Else
				KeyCode = 0
			End If
		ElseIf KeyCode = System.Windows.Forms.Keys.Up Then 
			If pCanForwardSetFocus = True And CtlCursorCondition(ExDateTextM) = -1 Then
				KeyCode = 0
				Call ForwardSetFocus()
			Else
				KeyCode = 0
			End If
		ElseIf KeyCode = System.Windows.Forms.Keys.Insert Then 
			'入力モードにする
			KeyCode = 0
			If CtlCursorCondition(ExDateTextM) = -1 Then
				ExDateTextM.SelectionStart = Len(ExDateTextM.Text)
			Else
				ExDateTextM.SelectionStart = 0
				ExDateTextM.SelectionLength = Len(ExDateTextM.Text)
			End If
		ElseIf KeyCode = System.Windows.Forms.Keys.Left Then 
			If pEditMode = False Then
				If pCanForwardSetFocus = True And CtlCursorCondition(ExDateTextM) = -1 Then
					KeyCode = 0
					Call ForwardSetFocus()
				End If
			End If
		ElseIf KeyCode = System.Windows.Forms.Keys.Right Then 
			If pEditMode = False Then
				If pCanNextSetFocus = True And CtlCursorCondition(ExDateTextM) = -1 Then
					KeyCode = 0
					Call NextSetFocus()
				End If
			End If
		End If
	End Sub
	
	Private Sub ExDateTextM_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles ExDateTextM.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		Const Numbers As String = "0123456789" ' 入力許可文字
		Dim strText As String
		
		If KeyAscii = System.Windows.Forms.Keys.Return Then KeyAscii = 0
		
		'数字検査をする
		If KeyAscii <> 8 Then ' バックスペースは例外
			If InStr(Numbers, Chr(KeyAscii)) = 0 Then
				KeyAscii = 0 ' 入力を無効にする
				GoTo EventExitSub
			End If
			strText = InsStrToTextBox(ExDateTextM, Chr(KeyAscii))
			If InStr(strText, "0") = 1 Then
				KeyAscii = 0
			End If
			If Len(strText) <= 2 Then
				If CDbl(strText) < 0 Or CDbl(strText) > 12 Then
					KeyAscii = 0
				End If
			Else
				KeyAscii = 0
			End If
		End If
		
		RaiseEvent KeyPress(Me, New KeyPressEventArgs(KeyAscii))
EventExitSub: 
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
	
	Private Sub ExDateTextM_KeyUp(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyEventArgs) Handles ExDateTextM.KeyUp
		Dim KeyCode As Short = eventArgs.KeyCode
		Dim Shift As Short = eventArgs.KeyData \ &H10000
		RaiseEvent KeyUp(Me, New KeyUpEventArgs(KeyCode, Shift))
	End Sub
	
	Private Sub ExDateTextM_MouseDown(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles ExDateTextM.MouseDown
		Dim Button As Short = eventArgs.Button \ &H100000
		Dim Shift As Short = System.Windows.Forms.Control.ModifierKeys \ &H10000
		Dim X As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim Y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		RaiseEvent MouseDown(Me, New MouseDownEventArgs(Button, Shift, X, Y))
		fClicking = True
	End Sub
	
	Private Sub ExDateTextM_MouseMove(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles ExDateTextM.MouseMove
		Dim Button As Short = eventArgs.Button \ &H100000
		Dim Shift As Short = System.Windows.Forms.Control.ModifierKeys \ &H10000
		Dim X As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim Y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		RaiseEvent MouseMove(Me, New MouseMoveEventArgs(Button, Shift, X, Y))
	End Sub
	
	Private Sub ExDateTextM_MouseUp(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles ExDateTextM.MouseUp
		Dim Button As Short = eventArgs.Button \ &H100000
		Dim Shift As Short = System.Windows.Forms.Control.ModifierKeys \ &H10000
		Dim X As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim Y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		RaiseEvent MouseUp(Me, New MouseUpEventArgs(Button, Shift, X, Y))
		fClicking = False
	End Sub
	
	Private Sub ExDateTextBoxM_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Enter
		fGotFocus = True
		On Error Resume Next
		If Me.Hwnd <> MyParentName Then
			'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			vUNDOBUF = ExDateTextM.Text
		End If
		If Err.Number <> 0 Then
			'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			vUNDOBUF = ExDateTextM.Text
		End If
		Err.Clear()
		On Error GoTo 0
	End Sub
	
	Private Sub ExDateTextBoxM_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Leave
		fGotFocus = False
		fClicking = False
	End Sub
	
	Private Sub ExDateTextBoxM_GotFocus(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.GotFocus
		fGotFocus = True
	End Sub
	
	Private Sub ExDateTextBoxM_LostFocus(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.LostFocus
		fGotFocus = False
	End Sub
	
	Private Sub UserControl_Terminate()
		'終了時にIMEModeを0-なしにしないと
		'WinMEでimm32.dllがこける。
		ExDateTextM.IMEMode = System.Windows.Forms.ImeMode.NoControl
	End Sub
	
	Private Sub ExDateTextBoxM_Resize(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Resize
		With ExDateTextM
			.Top = 0
			.Left = 0
			.Width = MyBase.Width
			'UPGRADE_ISSUE: UserControl プロパティ ExDateTextBoxM.Extender はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト Extender.Width の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			Extender.Width = VB6.PixelsToTwipsX(.Width) '2002/08/15 ADD
			.Height = MyBase.Height
			'UPGRADE_ISSUE: UserControl プロパティ ExDateTextBoxM.Extender はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト Extender.Height の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			Extender.Height = VB6.PixelsToTwipsY(.Height) '2002/08/15 ADD
			'''''        UserControl.Height = .Height   '2002/08/15 DEL
			'        .Height = Extender.Height
			'        .Width = Extender.Width
		End With
	End Sub
	
	'UPGRADE_ISSUE: UserControl イベント UserControl.InitProperties はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="ABD9AF39-7E24-4AFF-AD8D-3675C1AA3054"' をクリックしてください。
	Private Sub UserControl_InitProperties()
		'    Text = ""
		Me.FocusBackColor = System.Drawing.SystemColors.Window
		Me.ForeColor = System.Drawing.SystemColors.WindowText
		Me.BackColor = System.Drawing.SystemColors.Window
		Me.MaxLength = 2
		'UPGRADE_ISSUE: AmbientProperties プロパティ Ambient.Font はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Me.Font = Ambient.Font
		pCanNextSetFocus = True
		pCanForwardSetFocus = True
		pSelectText = True
		pEditMode = True
		Me.Appearance = ExDateTextBoxY.AppearanceType.立体 '01/12/20
		Me.BorderStyle = ExDateTextBoxY.BorderStyleType.実線 '01/12/20
	End Sub
	
	'UPGRADE_ISSUE: VBRUN.PropertyBag 型 はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6B85A2A7-FE9F-4FBE-AA0C-CF11AC86A305"' をクリックしてください。
	'UPGRADE_WARNING: UserControl イベント ReadProperties はサポートされていません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="92F3B58C-F772-4151-BE90-09F4A232AEAD"' をクリックしてください。
	Private Sub UserControl_ReadProperties(ByRef PropBag As Object)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		ExDateTextM.Text = PropBag.ReadProperty("Text", Text)
		'    ExDateTextM.Text = PropBag.ReadProperty("Appearance", 1)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Me.Enabled = PropBag.ReadProperty("Enabled", True)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Me.Locked = PropBag.ReadProperty("Locked", False)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Me.MaxLength = PropBag.ReadProperty("MaxLength", 2)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Me.ForeColor = System.Drawing.ColorTranslator.FromOle(PropBag.ReadProperty("ForeColor", System.Drawing.ColorTranslator.ToOle(System.Drawing.SystemColors.WindowText)))
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Me.BackColor = System.Drawing.ColorTranslator.FromOle(PropBag.ReadProperty("BackColor", System.Drawing.ColorTranslator.ToOle(System.Drawing.SystemColors.Window)))
		'    ExDateTextM.IMEMode = PropBag.ReadProperty("IMEMode", 0)
		'UPGRADE_ISSUE: AmbientProperties プロパティ Ambient.Font はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Me.Font = PropBag.ReadProperty("Font", Ambient.Font)
		
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		pFocusBackColor = System.Drawing.ColorTranslator.FromOle(PropBag.ReadProperty("FocusBackColor", System.Drawing.ColorTranslator.ToOle(System.Drawing.SystemColors.Window)))
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		pCanNextSetFocus = PropBag.ReadProperty("CanNextSetFocus", pCanNextSetFocus)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		pCanForwardSetFocus = PropBag.ReadProperty("CanForwardSetFocus", pCanForwardSetFocus)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		pSelectText = PropBag.ReadProperty("SelectText", pSelectText)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		pEditMode = PropBag.ReadProperty("EditMode", pEditMode)
		
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		ExDateTextM.TextAlign = PropBag.ReadProperty("Alignment", 0)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_ISSUE: TextBox プロパティ ExDateTextM.Appearance はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		ExDateTextM.Appearance = PropBag.ReadProperty("Appearance", 1)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		ExDateTextM.BorderStyle = PropBag.ReadProperty("BorderStyle", 1)
		
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		vUNDOBUF = PropBag.ReadProperty("OldValue", vUNDOBUF)
	End Sub
	
	'UPGRADE_ISSUE: VBRUN.PropertyBag 型 はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6B85A2A7-FE9F-4FBE-AA0C-CF11AC86A305"' をクリックしてください。
	'UPGRADE_WARNING: UserControl イベント WriteProperties はサポートされていません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="92F3B58C-F772-4151-BE90-09F4A232AEAD"' をクリックしてください。
	Private Sub UserControl_WriteProperties(ByRef PropBag As Object)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("Text", ExDateTextM.Text)
		'    Call PropBag.WriteProperty("Text", ExDateTextM.Appearance)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("Enabled", Me.Enabled, True)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("Locked", Me.Locked, False)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("MaxLength", Me.MaxLength)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("ForeColor", System.Drawing.ColorTranslator.ToOle(Me.ForeColor))
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("BackColor", System.Drawing.ColorTranslator.ToOle(Me.BackColor))
		'    Call PropBag.WriteProperty("IMEMode", ExDateTextM.IMEMode)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("Font", Me.Font)
		
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("FocusBackColor", pFocusBackColor)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("CanNextSetFocus", pCanNextSetFocus)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("CanForwardSetFocus", pCanForwardSetFocus)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("SelectText", pSelectText)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("EditMode", pEditMode)
		
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("Alignment", ExDateTextM.TextAlign, 0)
		'UPGRADE_ISSUE: TextBox プロパティ ExDateTextM.Appearance はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("Appearance", ExDateTextM.Appearance, 1)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("BorderStyle", ExDateTextM.BorderStyle, 1)
		
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("OldValue", vUNDOBUF)
	End Sub
	
	'''Public Property Get CanUndo() As Boolean
	'''    Dim fUndo As Boolean
	'''
	'''    fUndo = SendMessage(ExDateTextM.Hwnd, EM_CANUNDO, 0, ByVal 0&)
	'''
	'''    If fUndo Then CanUndo = True Else CanUndo = False
	'''
	'''End Property
	
	
	Public Overrides Property Text() As String
		Get
			Text = ExDateTextM.Text
		End Get
		Set(ByVal Value As String)
			ExDateTextM.Text = Value
			On Error Resume Next '01/09/05
			If Me.Hwnd <> MyParentName Then
				'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				vUNDOBUF = ExDateTextM.Text
			End If
			If Err.Number <> 0 Then
				'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				vUNDOBUF = ExDateTextM.Text
			End If
			Err.Clear()
			On Error GoTo 0
			RaiseEvent TextChange()
		End Set
	End Property
	
	
	Public Property Alignment() As System.Drawing.ContentAlignment
		Get
			Alignment = ExDateTextM.TextAlign
		End Get
		Set(ByVal Value As System.Drawing.ContentAlignment)
			ExDateTextM.TextAlign = Value
			RaiseEvent AlignmentChange()
		End Set
	End Property
	
	
	Public Property Appearance() As ExDateTextBoxY.AppearanceType
		Get
			'UPGRADE_ISSUE: TextBox プロパティ ExDateTextM.Appearance はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
			Appearance = ExDateTextM.Appearance
		End Get
		Set(ByVal Value As ExDateTextBoxY.AppearanceType)
			'UPGRADE_ISSUE: TextBox プロパティ ExDateTextM.Appearance はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
			ExDateTextM.Appearance = Value
			RaiseEvent AppearanceChange()
		End Set
	End Property
	
	
	Public Shadows Property BorderStyle() As ExDateTextBoxY.BorderStyleType
		Get
			BorderStyle = ExDateTextM.BorderStyle
		End Get
		Set(ByVal Value As ExDateTextBoxY.BorderStyleType)
			ExDateTextM.BorderStyle = Value
			RaiseEvent BorderStyleChange()
		End Set
	End Property
	
	
	Public Shadows Property Enabled() As Boolean
		Get
			'''    Enabled = ExDateTextM.Enabled
			Enabled = MyBase.Enabled
		End Get
		Set(ByVal Value As Boolean)
			'''    ExDateTextM.Enabled = New_Enabled
			MyBase.Enabled = Value
			RaiseEvent EnabledChange()
		End Set
	End Property
	
	
	Public Property Locked() As Boolean
		Get
			Locked = ExDateTextM.ReadOnly
		End Get
		Set(ByVal Value As Boolean)
			'    SendMessage m_hWnd, EM_SETREADONLY, NewProp, ByVal 0&
			ExDateTextM.ReadOnly = Value
			RaiseEvent LockedChange()
		End Set
	End Property
	
	
	Public Property MaxLength() As Integer
		Get
			'UPGRADE_WARNING: TextBox プロパティ ExDateTextM.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
			MaxLength = ExDateTextM.Maxlength
		End Get
		Set(ByVal Value As Integer)
			'UPGRADE_WARNING: TextBox プロパティ ExDateTextM.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
			ExDateTextM.Maxlength = Value
			RaiseEvent MaxLengthChange()
		End Set
	End Property
	
	
	Public Property SelStart() As Integer
		Get
			SelStart = ExDateTextM.SelectionStart
		End Get
		Set(ByVal Value As Integer)
			If Not DesignMode = False Then Err.Raise(382)
			ExDateTextM.SelectionStart = Value
			RaiseEvent SelStartChange()
		End Set
	End Property
	
	
	Public Property SelLength() As Integer
		Get
			SelLength = ExDateTextM.SelectionLength
		End Get
		Set(ByVal Value As Integer)
			If Not DesignMode = False Then Err.Raise(382)
			ExDateTextM.SelectionLength = Value
			RaiseEvent SelLengthChange()
		End Set
	End Property
	
	
	Public Property SelText() As String
		Get
			SelText = ExDateTextM.SelectedText
		End Get
		Set(ByVal Value As String)
			If Not DesignMode = False Then Err.Raise(382)
			ExDateTextM.SelectedText = Value
			RaiseEvent SelTextChange()
		End Set
	End Property
	
	
	Public Overrides Property ForeColor() As System.Drawing.Color
		Get
			ForeColor = ExDateTextM.ForeColor
		End Get
		Set(ByVal Value As System.Drawing.Color)
			ExDateTextM.ForeColor = Value
			RaiseEvent ForeColorChange()
		End Set
	End Property
	
	
	Public Overrides Property BackColor() As System.Drawing.Color
		Get
			BackColor = ExDateTextM.BackColor
		End Get
		Set(ByVal Value As System.Drawing.Color)
			ExDateTextM.BackColor = Value
			hBackColor = Value
			RaiseEvent BackColorChange()
		End Set
	End Property
	
	
	Public Property FocusBackColor() As System.Drawing.Color
		Get
			FocusBackColor = pFocusBackColor
		End Get
		Set(ByVal Value As System.Drawing.Color)
			pFocusBackColor = Value
			RaiseEvent FocusBackColorChange()
		End Set
	End Property
	
	
	Public Overrides Property Font() As System.Drawing.Font
		Get
			Font = ExDateTextM.Font
		End Get
		Set(ByVal Value As System.Drawing.Font)
			ExDateTextM.Font = Value
			Call ExDateTextBoxM_Resize(Me, New System.EventArgs())
			RaiseEvent FontChange()
		End Set
	End Property
	'テキストを選択状態にする。
	
	Public Property SelectText() As Boolean
		Get
			SelectText = pSelectText
		End Get
		Set(ByVal Value As Boolean)
			pSelectText = Value
			RaiseEvent SelectTextChange()
		End Set
	End Property
	'次にセットフォーカスするか
	
	Public Property CanNextSetFocus() As Boolean
		Get
			CanNextSetFocus = pCanNextSetFocus
		End Get
		Set(ByVal Value As Boolean)
			pCanNextSetFocus = Value
			RaiseEvent CanNextSetFocusChange()
		End Set
	End Property
	'前にセットフォーカスするか
	
	Public Property CanForwardSetFocus() As Boolean
		Get
			CanForwardSetFocus = pCanForwardSetFocus
		End Get
		Set(ByVal Value As Boolean)
			pCanForwardSetFocus = Value
			RaiseEvent CanForwardSetFocusChange()
		End Set
	End Property
	'ハンドル
	Public ReadOnly Property Hwnd() As Integer
		Get
			Hwnd = ExDateTextM.Handle.ToInt32
		End Get
	End Property
	'矢印キーでコントロール移動するか
	
	Public Property EditMode() As Boolean
		Get
			EditMode = pEditMode
		End Get
		Set(ByVal Value As Boolean)
			pEditMode = Value
			RaiseEvent EditModeChange()
		End Set
	End Property
	
	
	Public Property OldValue() As Object
		Get
			'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト OldValue の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			OldValue = vUNDOBUF
		End Get
		Set(ByVal Value As Object)
			'UPGRADE_WARNING: オブジェクト New_OldValue の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			vUNDOBUF = Value
			RaiseEvent OldValueChange()
		End Set
	End Property
	
	'テキストボックスの現在の位置に文字列を埋め込む
	Private Function InsStrToTextBox(ByRef TxText As System.Windows.Forms.TextBox, ByVal sInsStr As String) As String
		With TxText
			'        If sInsStr = "-" Then
			'            InsStrToTextBox = sInsStr & Left$(.Text, .SelStart) & Right$(.Text, Len(.Text) - .SelStart - .SelLength)
			'        Else
			InsStrToTextBox = VB.Left(.Text, .SelectionStart) & sInsStr & VB.Right(.Text, Len(.Text) - .SelectionStart - .SelectionLength)
			'        End If
		End With
	End Function
	
	'メソッド
	Public Sub NextSetFocus()
		On Error GoTo NextSetFocus_Err
		
		''    Lb_Cancel = False
		''    RaiseEvent EnterKeyDown(Lb_Cancel)
		''
		''    If Lb_Cancel = True Then Exit Sub
		''
		System.Windows.Forms.Application.DoEvents()
		
		'既にｶｰｿﾙが他へ遷移していたら処理しない
		If fGotFocus = True Then
			'        Call SendMessage(ExText.hWnd, WM_KEYDOWN, vbKeyTab, ByVal 0&)
			Call PostMessage(ExDateTextM.Handle.ToInt32, WM_KEYDOWN, System.Windows.Forms.Keys.Tab, 0)
			'        PostMessage ExText.Hwnd, WM_KEYDOWN, VK_TAB, 1
			'        PostMessage ExText.Hwnd, WM_KEYUP, VK_TAB, 1
			'        SendKeys "{TAB}"
		End If
		'    If UserControl.Enabled = True And pText.Enabled = True And pText.Visible = True Then
		'        SendKeys "{TAB}"
		'    End If
		
NextSetFocus_Err: 
	End Sub
	
	Public Sub ForwardSetFocus()
		On Error GoTo ForwardSetFocus_Err
		
		System.Windows.Forms.Application.DoEvents()
		
		'既にｶｰｿﾙが他へ遷移していたら処理しない
		If fGotFocus = True Then
			Call keybd_event(System.Windows.Forms.Keys.ShiftKey, 0, 0, 0)
			Call keybd_event(System.Windows.Forms.Keys.Tab, 0, 0, 0)
			Call keybd_event(System.Windows.Forms.Keys.ShiftKey, 0, KEYEVENTF_KEYUP, 0)
			Call keybd_event(System.Windows.Forms.Keys.Tab, 0, KEYEVENTF_KEYUP, 0)
		End If
ForwardSetFocus_Err: 
	End Sub
	
	'コントロールのカーソル状態を検査
	Private Function CtlCursorCondition(Optional ByRef CheckControl As Object = Nothing) As Short
		Dim ctl As System.Windows.Forms.Control
		Dim TextResult As Object
		
		'UPGRADE_NOTE: IsMissing() は IsNothing() に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="8AE1CB93-37AB-439A-A4FF-BE3B6760BB23"' をクリックしてください。
		If IsNothing(CheckControl) Then
			ctl = Me
		Else
			ctl = CheckControl
		End If
		With ctl
			On Error Resume Next
			'UPGRADE_WARNING: オブジェクト ctl.SelText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト TextResult の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			TextResult = ctl.SelText
			'--- エラーが発生していなければ
			If Err.Number = 0 Then
				On Error GoTo 0
				'UPGRADE_WARNING: オブジェクト ctl.SelText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				If Len(.SelText) = Len(.Text) Then
					CtlCursorCondition = -1 'selected all
					'UPGRADE_WARNING: オブジェクト ctl.SelLength の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト ctl.SelStart の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				ElseIf .SelStart = 0 And .SelLength <= 1 Then 
					CtlCursorCondition = 2 'left side
					'UPGRADE_WARNING: オブジェクト ctl.SelLength の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト ctl.SelStart の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				ElseIf .SelStart = Len(.Text) And .SelLength = 0 Then 
					CtlCursorCondition = 1 'right side
					'UPGRADE_WARNING: オブジェクト ctl.SelLength の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト ctl.SelStart の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				ElseIf .SelStart = Len(.Text) - 1 And .SelLength = 1 Then 
					CtlCursorCondition = 1 'right side
				End If
			Else
				On Error GoTo 0
				CtlCursorCondition = -1
			End If
		End With
	End Function
	
	Public Sub ShowAdoutBox()
		frmAbout.ShowDialog()
		frmAbout.Close()
		'UPGRADE_NOTE: オブジェクト frmAbout をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		frmAbout = Nothing
	End Sub
End Class