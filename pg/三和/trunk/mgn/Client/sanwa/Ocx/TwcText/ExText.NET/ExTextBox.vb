Option Strict Off
Option Explicit On
<System.Runtime.InteropServices.ProgId("ExTextBox_NET.ExTextBox")> Public Class ExTextBox
	Inherits System.Windows.Forms.UserControl
	Public Event IMEModeChange()
	Public Event EditModeChange()
	Public Event CharacterSizeChange()
	Public Event CanForwardSetFocusChange()
	Public Event SelTextChange()
	Public Event OLEDropModeChange()
	Public Event MousePointerChange()
	Public Event ForeColorChange()
	Public Event BorderStyleChange()
	Public Event OldValueChange()
	Public Event AlignmentChange()
	Public Event TextChange()
	Public Event AppearanceChange()
	Public Event LockedChange()
	Public Event FocusBackColorChange()
	Public Event MouseIconChange()
	Public Event CanNextSetFocusChange()
	Public Event PasswordCharChange()
	Public Event SelectTextChange()
	Public Event LengthTypeChange()
	Public Event SelStartChange()
	Public Event MaxLengthChange()
	Public Event EnabledChange()
	Public Event OLEDragModeChange()
	Public Event SelLengthChange()
	Public Event FontChange()
	Public Event BackColorChange()
	
	'UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
	Private Declare Function SendMessage Lib "user32"  Alias "SendMessageA"(ByVal Hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer, ByRef lParam As Any) As Integer
	''Private Const EM_CANUNDO = &HC6
	''Private Const EM_UNDO = &HC7
	''Private Const EM_EMPTYUNDOBUFFER = &HCD
	''Private Const EM_SETREADONLY = &HCF
	Private Const WM_KEYDOWN As Integer = &H100
	Private Const EM_LIMITTEXT As Integer = &HC5
	''Private Const EM_GETMODIFY = &HB8
	''Private Const EM_SETMODIFY = &HB9
	
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
	Enum IMEModeType
		なし = 0
		オン = 1
		オフ = 2
		オフ固定 = 3
		全角ひらがな = 4
		全角カタカナ = 5
		半角カタカナ = 6
		全角英数 = 7
	End Enum
	Enum CharSize
		なし = 0
		大文字 = 1
		小文字 = 2
	End Enum
	Enum LenType
		UnicodeType = 0
		ByteType = 1
	End Enum
	Enum AppearanceType
		フラット = 0
		立体 = 1
	End Enum
	Enum BorderStyleType
		なし = 0
		実線 = 1
	End Enum
	
	'プロパティ変数
	Private pFocusBackColor As System.Drawing.Color
	Private hBackColor As System.Drawing.Color
	Private pCharSize As CharSize
	Private pCanNextSetFocus As Boolean
	Private pCanForwardSetFocus As Boolean
	Private pSelectText As Boolean
	Private pMaxLength As Integer
	Private pLengthType As LenType
	Private pEditMode As Boolean
	
	'変数
	Private fGotFocus As Boolean
	Private fClicking As Boolean
	Private vUNDOBUF As Object
	'Private fUNDOBUF  As Boolean
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
	''''Public Event LinkClose()
	''''Public Event LinkError(LinkErr As Integer)
	''''Public Event LinkNotify()
	''''Public Event LinkOpen(Cancel As Integer)
	Public Event OLECompleteDrag(ByVal Sender As System.Object, ByVal e As OLECompleteDragEventArgs)
	Public Event OLEDragDrop(ByVal Sender As System.Object, ByVal e As OLEDragDropEventArgs)
	Public Event OLEDragOver(ByVal Sender As System.Object, ByVal e As OLEDragOverEventArgs)
	Public Event OLEGiveFeedback(ByVal Sender As System.Object, ByVal e As OLEGiveFeedbackEventArgs)
	Public Event OLESetData(ByVal Sender As System.Object, ByVal e As OLESetDataEventArgs)
	Public Event OLEStartDrag(ByVal Sender As System.Object, ByVal e As OLEStartDragEventArgs)
	
	Public Event RtnKeyDown(ByVal Sender As System.Object, ByVal e As RtnKeyDownEventArgs)
	Public Event SpcKeyPress(ByVal Sender As System.Object, ByVal e As SpcKeyPressEventArgs)
	
	Public Sub Undo()
		Dim lret As Integer
		
		'    lret = SendMessage(ExText.Hwnd, EM_CANUNDO, 0, 0)
		'    If lret <> 0 Then
		'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		ExText.Text = vUNDOBUF
		'    End If
		'    Call SendMessage(ExText.Hwnd, EM_UNDO, 0, ByVal 0&)
		'    Call EmptyUndoBuffer
	End Sub
	
	Public Sub EmptyUndoBuffer()
		'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		vUNDOBUF = ExText.Text
		'    Call SendMessage(ExText.Hwnd, EM_EMPTYUNDOBUFFER, 0, ByVal 0&)
	End Sub
	
	'UPGRADE_WARNING: イベント ExText.TextChanged は、フォームが初期化されたときに発生します。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"' をクリックしてください。
	Private Sub ExText_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ExText.TextChanged
		RaiseEvent Change(Me, Nothing)
	End Sub
	
	Private Sub ExText_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ExText.Click
		RaiseEvent Click(Me, Nothing)
	End Sub
	
	Private Sub ExText_DoubleClick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ExText.DoubleClick
		RaiseEvent DblClick(Me, Nothing)
	End Sub
	
	Private Sub ExText_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ExText.Enter
		'    hBackColor = ExText.BackColor
		ExText.BackColor = pFocusBackColor
		
		''''    DoEvents
		On Error Resume Next
		If Me.Hwnd <> MyParentName Then
			''        If UserControl.Parent.ActiveControl.Hwnd <> MyParentName Then
			'        If UserControl.Parent.ActiveControl.Name <> MyParentName Then
			'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			vUNDOBUF = ExText.Text
		End If
		If Err.Number <> 0 Then
			'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			vUNDOBUF = ExText.Text
		End If
		Err.Clear()
		On Error GoTo 0
		
		If pSelectText Then
			If fClicking = False Then
				ExText.SelectionStart = 0
				ExText.SelectionLength = Len(ExText.Text)
			End If
		End If
	End Sub
	
	Private Sub ExText_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ExText.Leave
		ExText.BackColor = hBackColor
		On Error Resume Next
		'UPGRADE_WARNING: Control プロパティ UserControl.Parent は、新しい動作をもつ UserControl.FindForm にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="DFCDE711-9694-47D7-9C50-45A99CD8E91E"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト UserControl.Parent.ActiveControl の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_ISSUE: Control Hwnd は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		MyParentName = MyBase.FindForm.ActiveControl.Hwnd
		Err.Clear()
		On Error GoTo 0
		''    Call EmptyUndoBuffer
	End Sub
	
	Private Sub ExText_KeyDown(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyEventArgs) Handles ExText.KeyDown
		Dim KeyCode As Short = eventArgs.KeyCode
		Dim Shift As Short = eventArgs.KeyData \ &H10000
		Dim Rtn_Cancel As Boolean
		
		If Shift = 0 Then
			Select Case KeyCode
				Case System.Windows.Forms.Keys.Return
					RaiseEvent RtnKeyDown(Me, New RtnKeyDownEventArgs(KeyCode, Shift, Rtn_Cancel))
					'            Case vbKeySpace
					'                RaiseEvent SpcKeyDown(KeyCode, Shift, Spc_Cancel)
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
			If pCanNextSetFocus = True And CtlCursorCondition(ExText) = -1 Then
				KeyCode = 0
				Call NextSetFocus()
			Else
				KeyCode = 0
			End If
		ElseIf KeyCode = System.Windows.Forms.Keys.Up Then 
			If pCanForwardSetFocus = True And CtlCursorCondition(ExText) = -1 Then
				KeyCode = 0
				Call ForwardSetFocus()
			Else
				KeyCode = 0
			End If
		ElseIf KeyCode = System.Windows.Forms.Keys.Insert Then 
			'入力モードにする
			KeyCode = 0
			If CtlCursorCondition(ExText) = -1 Then
				ExText.SelectionStart = Len(ExText.Text)
			Else
				ExText.SelectionStart = 0
				ExText.SelectionLength = Len(ExText.Text)
			End If
		ElseIf KeyCode = System.Windows.Forms.Keys.Left Then 
			If pEditMode = False Then
				If pCanForwardSetFocus = True And CtlCursorCondition(ExText) = -1 Then
					KeyCode = 0
					Call ForwardSetFocus()
				End If
			End If
		ElseIf KeyCode = System.Windows.Forms.Keys.Right Then 
			If pEditMode = False Then
				If pCanNextSetFocus = True And CtlCursorCondition(ExText) = -1 Then
					KeyCode = 0
					Call NextSetFocus()
				End If
			End If
		End If
	End Sub
	
	Private Sub ExText_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles ExText.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		Dim Spc_Cancel As Boolean
		
		If KeyAscii = System.Windows.Forms.Keys.Return Then KeyAscii = 0
		''    If KeyAscii = vbKeyReturn Then
		''        If pCanNextSetFocus = True Then
		''            KeyAscii = 0
		''            NextSetFocus
		''        Else
		''            KeyAscii = 0
		''        End If
		''    End If
		Select Case KeyAscii
			Case System.Windows.Forms.Keys.Space
				RaiseEvent SpcKeyPress(Me, New SpcKeyPressEventArgs(KeyAscii, Spc_Cancel))
		End Select
		
		Select Case pCharSize
			Case 1
				KeyAscii = Asc(UCase(Chr(KeyAscii)))
			Case 2
				KeyAscii = Asc(LCase(Chr(KeyAscii)))
		End Select
		
		RaiseEvent KeyPress(Me, New KeyPressEventArgs(KeyAscii))
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
	
	Private Sub ExText_KeyUp(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyEventArgs) Handles ExText.KeyUp
		Dim KeyCode As Short = eventArgs.KeyCode
		Dim Shift As Short = eventArgs.KeyData \ &H10000
		RaiseEvent KeyUp(Me, New KeyUpEventArgs(KeyCode, Shift))
	End Sub
	
	Private Sub ExText_MouseDown(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles ExText.MouseDown
		Dim Button As Short = eventArgs.Button \ &H100000
		Dim Shift As Short = System.Windows.Forms.Control.ModifierKeys \ &H10000
		Dim X As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim Y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		RaiseEvent MouseDown(Me, New MouseDownEventArgs(Button, Shift, X, Y))
		fClicking = True
	End Sub
	
	Private Sub ExText_MouseMove(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles ExText.MouseMove
		Dim Button As Short = eventArgs.Button \ &H100000
		Dim Shift As Short = System.Windows.Forms.Control.ModifierKeys \ &H10000
		Dim X As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim Y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		RaiseEvent MouseMove(Me, New MouseMoveEventArgs(Button, Shift, X, Y))
	End Sub
	
	Private Sub ExText_MouseUp(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles ExText.MouseUp
		Dim Button As Short = eventArgs.Button \ &H100000
		Dim Shift As Short = System.Windows.Forms.Control.ModifierKeys \ &H10000
		Dim X As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim Y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		RaiseEvent MouseUp(Me, New MouseUpEventArgs(Button, Shift, X, Y))
		fClicking = False
	End Sub
	
	'UPGRADE_ISSUE: TextBox イベント ExText.OLECompleteDrag はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="ABD9AF39-7E24-4AFF-AD8D-3675C1AA3054"' をクリックしてください。
	Private Sub ExText_OLECompleteDrag(ByRef Effect As Integer) '2002/10/24 ADD
		RaiseEvent OLECompleteDrag(Me, New OLECompleteDragEventArgs(Effect))
	End Sub
	
	'UPGRADE_ISSUE: VBRUN.DataObject 型 はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6B85A2A7-FE9F-4FBE-AA0C-CF11AC86A305"' をクリックしてください。
	'UPGRADE_ISSUE: TextBox イベント ExText.OLEDragDrop はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="ABD9AF39-7E24-4AFF-AD8D-3675C1AA3054"' をクリックしてください。
	Private Sub ExText_OLEDragDrop(ByRef Data As Object, ByRef Effect As Integer, ByRef Button As Short, ByRef Shift As Short, ByRef X As Single, ByRef Y As Single) '2002/10/24 ADD
		RaiseEvent OLEDragDrop(Me, New OLEDragDropEventArgs(Data, Effect, Button, Shift, X, Y))
	End Sub
	
	'UPGRADE_ISSUE: VBRUN.DataObject 型 はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6B85A2A7-FE9F-4FBE-AA0C-CF11AC86A305"' をクリックしてください。
	'UPGRADE_ISSUE: TextBox イベント ExText.OLEDragOver はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="ABD9AF39-7E24-4AFF-AD8D-3675C1AA3054"' をクリックしてください。
	Private Sub ExText_OLEDragOver(ByRef Data As Object, ByRef Effect As Integer, ByRef Button As Short, ByRef Shift As Short, ByRef X As Single, ByRef Y As Single, ByRef State As Short) '2002/10/24 ADD
		RaiseEvent OLEDragOver(Me, New OLEDragOverEventArgs(Data, Effect, Button, Shift, X, Y, State))
	End Sub
	
	'UPGRADE_ISSUE: TextBox イベント ExText.OLEGiveFeedback はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="ABD9AF39-7E24-4AFF-AD8D-3675C1AA3054"' をクリックしてください。
	Private Sub ExText_OLEGiveFeedback(ByRef Effect As Integer, ByRef DefaultCursors As Boolean) '2002/10/24 ADD
		RaiseEvent OLEGiveFeedback(Me, New OLEGiveFeedbackEventArgs(Effect, DefaultCursors))
	End Sub
	
	'UPGRADE_ISSUE: VBRUN.DataObject 型 はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6B85A2A7-FE9F-4FBE-AA0C-CF11AC86A305"' をクリックしてください。
	'UPGRADE_ISSUE: TextBox イベント ExText.OLESetData はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="ABD9AF39-7E24-4AFF-AD8D-3675C1AA3054"' をクリックしてください。
	Private Sub ExText_OLESetData(ByRef Data As Object, ByRef DataFormat As Short) '2002/10/24 ADD
		RaiseEvent OLESetData(Me, New OLESetDataEventArgs(Data, DataFormat))
	End Sub
	
	'UPGRADE_ISSUE: VBRUN.DataObject 型 はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6B85A2A7-FE9F-4FBE-AA0C-CF11AC86A305"' をクリックしてください。
	'UPGRADE_ISSUE: TextBox イベント ExText.OLEStartDrag はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="ABD9AF39-7E24-4AFF-AD8D-3675C1AA3054"' をクリックしてください。
	Private Sub ExText_OLEStartDrag(ByRef Data As Object, ByRef AllowedEffects As Integer) '2002/10/24 ADD
		RaiseEvent OLEStartDrag(Me, New OLEStartDragEventArgs(Data, AllowedEffects))
	End Sub
	
	Private Sub ExTextBox_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Enter
		fGotFocus = True
		'''    DoEvents
		On Error Resume Next
		If Me.Hwnd <> MyParentName Then
			'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			vUNDOBUF = ExText.Text
		End If
		If Err.Number <> 0 Then
			'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			vUNDOBUF = ExText.Text
		End If
		Err.Clear()
		On Error GoTo 0
		'''''''    ExText.SetFocus
	End Sub
	
	Private Sub ExTextBox_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Leave
		fGotFocus = False
		fClicking = False
	End Sub
	
	Private Sub ExTextBox_GotFocus(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.GotFocus
		fGotFocus = True
		'    ExText.SetFocus
	End Sub
	
	Private Sub ExTextBox_LostFocus(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.LostFocus
		fGotFocus = False
		fClicking = False
	End Sub
	
	Private Sub UserControl_Terminate()
		'終了時にIMEModeを0-なしにしないと
		'WinMEでimm32.dllがこける。
		ExText.IMEMode = System.Windows.Forms.ImeMode.NoControl
	End Sub
	
	Private Sub ExTextBox_Resize(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Resize
		With ExText
			.Top = 0
			.Left = 0
			.Width = MyBase.Width
			'UPGRADE_ISSUE: UserControl プロパティ ExTextBox.Extender はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト Extender.Width の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			Extender.Width = VB6.PixelsToTwipsX(.Width) '2002/08/15 ADD
			.Height = MyBase.Height
			'UPGRADE_ISSUE: UserControl プロパティ ExTextBox.Extender はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト Extender.Height の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			Extender.Height = VB6.PixelsToTwipsY(.Height) '2002/08/15 ADD
			'''''        UserControl.Height = .Height   '2002/08/15 DEL
			''        .Height = Extender.Height
			''        .Width = Extender.Width
		End With
	End Sub
	
	'UPGRADE_ISSUE: UserControl イベント UserControl.InitProperties はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="ABD9AF39-7E24-4AFF-AD8D-3675C1AA3054"' をクリックしてください。
	Private Sub UserControl_InitProperties()
		'UPGRADE_ISSUE: AmbientProperties プロパティ Ambient.DisplayName はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Me.Text = Ambient.DisplayName
		Me.FocusBackColor = System.Drawing.SystemColors.Window
		Me.ForeColor = System.Drawing.SystemColors.WindowText
		Me.BackColor = System.Drawing.SystemColors.Window
		'UPGRADE_ISSUE: AmbientProperties プロパティ Ambient.Font はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Me.Font = Ambient.Font
		pCanNextSetFocus = True
		pCanForwardSetFocus = True
		pSelectText = True
		pEditMode = True
		Me.Appearance = AppearanceType.立体 '01/12/20
		Me.BorderStyle = BorderStyleType.実線 '01/12/20
	End Sub
	
	'UPGRADE_ISSUE: VBRUN.PropertyBag 型 はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6B85A2A7-FE9F-4FBE-AA0C-CF11AC86A305"' をクリックしてください。
	'UPGRADE_WARNING: UserControl イベント ReadProperties はサポートされていません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="92F3B58C-F772-4151-BE90-09F4A232AEAD"' をクリックしてください。
	Private Sub UserControl_ReadProperties(ByRef PropBag As Object)
		'UPGRADE_ISSUE: AmbientProperties プロパティ Ambient.DisplayName はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		ExText.Text = PropBag.ReadProperty("Text", Ambient.DisplayName)
		''    UserControl.Enabled = PropBag.ReadProperty("Enabled", True)
		''    ExText.Locked = PropBag.ReadProperty("Locked", False)
		''''    ExText.Alignment = PropBag.ReadProperty("Alignment", 0)
		''''    pMultiLine = PropBag.ReadProperty("MultiLine", False)
		''    ExText.MaxLength = PropBag.ReadProperty("MaxLength", 0)
		'''    ExText.SelStart = PropBag.ReadProperty("SelStart", 0)
		'''    ExText.SelLength = PropBag.ReadProperty("SelLength", 0)
		'''    ExText.SelText = PropBag.ReadProperty("SelText", "")
		''    ExText.ForeColor = PropBag.ReadProperty("ForeColor", vbWindowText)
		''    ExText.BackColor = PropBag.ReadProperty("BackColor", vbWindowBackground)
		''    ExText.IMEMode = PropBag.ReadProperty("IMEMode", 0)
		''    Set Font = PropBag.ReadProperty("Font", Ambient.Font)
		
		'    Me.Text = PropBag.ReadProperty("Text", Ambient.DisplayName)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		pLengthType = PropBag.ReadProperty("LengthType", pLengthType)
		
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Me.Enabled = PropBag.ReadProperty("Enabled", True)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Me.Locked = PropBag.ReadProperty("Locked", False)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Me.MaxLength = PropBag.ReadProperty("MaxLength", 0)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Me.ForeColor = System.Drawing.ColorTranslator.FromOle(PropBag.ReadProperty("ForeColor", System.Drawing.ColorTranslator.ToOle(System.Drawing.SystemColors.WindowText)))
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Me.BackColor = System.Drawing.ColorTranslator.FromOle(PropBag.ReadProperty("BackColor", System.Drawing.ColorTranslator.ToOle(System.Drawing.SystemColors.Window)))
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Me.IMEMode = PropBag.ReadProperty("IMEMode", 0)
		'UPGRADE_ISSUE: AmbientProperties プロパティ Ambient.Font はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Me.Font = PropBag.ReadProperty("Font", Ambient.Font)
		
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		pFocusBackColor = System.Drawing.ColorTranslator.FromOle(PropBag.ReadProperty("FocusBackColor", System.Drawing.ColorTranslator.ToOle(System.Drawing.SystemColors.Window)))
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		pCharSize = PropBag.ReadProperty("CharacterSize", pCharSize)
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
		ExText.TextAlign = PropBag.ReadProperty("Alignment", 0)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_ISSUE: TextBox プロパティ ExText.Appearance はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		ExText.Appearance = PropBag.ReadProperty("Appearance", 1)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		ExText.BorderStyle = PropBag.ReadProperty("BorderStyle", 1)
		
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		vUNDOBUF = PropBag.ReadProperty("OldValue", vUNDOBUF)
		
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_ISSUE: TextBox プロパティ ExText.OLEDragMode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		ExText.OLEDragMode = PropBag.ReadProperty("OLEDragMode", 0) '2002/10/24 ADD
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_ISSUE: TextBox プロパティ ExText.OLEDropMode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		ExText.OLEDropMode = PropBag.ReadProperty("OLEDropMode", 0) '2002/10/24 ADD
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		ExText.PasswordChar = PropBag.ReadProperty("PasswordChar", vbNullString) '2002/10/24 ADD
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_ISSUE: TextBox プロパティ ExText.MousePointer はカスタム マウスポインタをサポートしません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="45116EAB-7060-405E-8ABE-9DBB40DC2E86"' をクリックしてください。
		ExText.Cursor = PropBag.ReadProperty("MousePointer", 0) '2002/10/24 ADD
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Me.MouseIcon = PropBag.ReadProperty("MouseIcon", Nothing) '2002/10/24 ADD
	End Sub
	
	'UPGRADE_ISSUE: VBRUN.PropertyBag 型 はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6B85A2A7-FE9F-4FBE-AA0C-CF11AC86A305"' をクリックしてください。
	'UPGRADE_WARNING: UserControl イベント WriteProperties はサポートされていません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="92F3B58C-F772-4151-BE90-09F4A232AEAD"' をクリックしてください。
	Private Sub UserControl_WriteProperties(ByRef PropBag As Object)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("Text", ExText.Text)
		''    Call PropBag.WriteProperty("Enabled", UserControl.Enabled, True)
		''    Call PropBag.WriteProperty("Locked", ExText.Locked, False)
		''''    Call PropBag.WriteProperty("Alignment", ExText.Alignment, 0)
		''''    Call PropBag.WriteProperty("MultiLine", ExText.MultiLine, False)
		''    Call PropBag.WriteProperty("MaxLength", ExText.MaxLength, 0)
		'''    Call PropBag.WriteProperty("SelStart", ExText.SelStart, 0)
		'''    Call PropBag.WriteProperty("SelLength", ExText.SelLength, 0)
		'''    Call PropBag.WriteProperty("SelText", ExText.SelText, "")
		''    Call PropBag.WriteProperty("ForeColor", ExText.ForeColor)
		''    Call PropBag.WriteProperty("BackColor", ExText.BackColor)
		''    Call PropBag.WriteProperty("IMEMode", ExText.IMEMode)
		''    Call PropBag.WriteProperty("Font", Font)
		
		'    Call PropBag.WriteProperty("Text", Me.Text)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("Enabled", Me.Enabled, True)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("Locked", Me.Locked, False)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("MaxLength", Me.MaxLength, 0)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("ForeColor", System.Drawing.ColorTranslator.ToOle(Me.ForeColor))
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("BackColor", System.Drawing.ColorTranslator.ToOle(Me.BackColor))
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("IMEMode", Me.IMEMode)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("Font", Me.Font)
		
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("FocusBackColor", pFocusBackColor)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("CharacterSize", pCharSize)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("CanNextSetFocus", pCanNextSetFocus)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("CanForwardSetFocus", pCanForwardSetFocus)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("SelectText", pSelectText)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("LengthType", pLengthType)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("EditMode", pEditMode)
		
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("Alignment", ExText.TextAlign, 0)
		'UPGRADE_ISSUE: TextBox プロパティ ExText.Appearance はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("Appearance", ExText.Appearance, 1)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("BorderStyle", ExText.BorderStyle, 1)
		
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("OldValue", vUNDOBUF)
		
		'UPGRADE_ISSUE: TextBox プロパティ ExText.OLEDragMode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("OLEDragMode", ExText.OLEDragMode, 0) '2002/10/24 ADD
		'UPGRADE_ISSUE: TextBox プロパティ ExText.OLEDropMode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("OLEDropMode", ExText.OLEDropMode, 0) '2002/10/24 ADD
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("PasswordChar", ExText.PasswordChar, vbNullString) '2002/10/24 ADD
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("MousePointer", ExText.Cursor, 0) '2002/10/24 ADD
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("MouseIcon", MouseIcon, Nothing) '2002/10/24 ADD
	End Sub
	
	
	Public Overrides Property Text() As String
		Get
			'UPGRADE_ISSUE: 定数 vbUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			Text = StrConv(StrConv(ExText.Text(), vbFromUnicode), vbUnicode)
			''    On Error Resume Next
			''    If ExText.Text() = vbNullString Then
			''        Text = vbNullString
			''    Else
			''        Text = ExText.Text()
			''    End If
			''    If Err <> 0 Then
			''        MsgBox Err.Number & " " & Err.Description
			''    End If
		End Get
		Set(ByVal Value As String)
			ExText.Text = Value
			On Error Resume Next '01/09/05   01/11/20
			If Me.Hwnd <> MyParentName Then
				'        If UserControl.Parent.ActiveControl.Hwnd <> MyParentName Then
				'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				vUNDOBUF = ExText.Text
			End If
			If Err.Number <> 0 Then
				'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				vUNDOBUF = ExText.Text
			End If
			Err.Clear()
			On Error GoTo 0
			RaiseEvent TextChange()
		End Set
	End Property
	
	
	Public Shadows Property Enabled() As Boolean
		Get
			'    Enabled = ExText.Enabled
			Enabled = MyBase.Enabled
			'    Enabled = Me.Enabled
		End Get
		Set(ByVal Value As Boolean)
			'    ExText.Enabled = New_Enabled
			MyBase.Enabled = Value
			'    Me.Enabled = New_Enabled
			RaiseEvent EnabledChange()
		End Set
	End Property
	
	
	Public Property Locked() As Boolean
		Get
			Locked = ExText.ReadOnly
		End Get
		Set(ByVal Value As Boolean)
			'    SendMessage m_hWnd, EM_SETREADONLY, NewProp, ByVal 0&
			ExText.ReadOnly = Value
			RaiseEvent LockedChange()
		End Set
	End Property
	
	
	Public Property Alignment() As System.Drawing.ContentAlignment
		Get
			Alignment = ExText.TextAlign
		End Get
		Set(ByVal Value As System.Drawing.ContentAlignment)
			ExText.TextAlign = Value
			RaiseEvent AlignmentChange()
		End Set
	End Property
	
	
	Public Property Appearance() As AppearanceType
		Get
			'UPGRADE_ISSUE: TextBox プロパティ ExText.Appearance はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
			Appearance = ExText.Appearance
		End Get
		Set(ByVal Value As AppearanceType)
			'UPGRADE_ISSUE: TextBox プロパティ ExText.Appearance はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
			ExText.Appearance = Value
			RaiseEvent AppearanceChange()
		End Set
	End Property
	
	
	Public Shadows Property BorderStyle() As BorderStyleType
		Get
			BorderStyle = ExText.BorderStyle
		End Get
		Set(ByVal Value As BorderStyleType)
			ExText.BorderStyle = Value
			RaiseEvent BorderStyleChange()
		End Set
	End Property
	
	'設定不可
	''Public Property Get MultiLine() As Boolean
	''    MultiLine = pMultiLine
	''End Property
	''
	''Public Property Let MultiLine(ByVal New_MultiLine As Boolean)
	''    pMultiLine = New_MultiLine
	''    PropertyChanged ("MultiLine")
	''End Property
	
	
	Public Property MaxLength() As Integer
		Get
			'    MaxLength = ExText.MaxLength
			MaxLength = pMaxLength
		End Get
		Set(ByVal Value As Integer)
			If pLengthType = LenType.UnicodeType Then
				'UPGRADE_WARNING: TextBox プロパティ ExText.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				ExText.Maxlength = Value
			Else
				'UPGRADE_WARNING: TextBox プロパティ ExText.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				ExText.Maxlength = 0
				Call SendMessage(ExText.Handle.ToInt32, EM_LIMITTEXT, Value, 0)
			End If
			pMaxLength = Value
			RaiseEvent MaxLengthChange()
		End Set
	End Property
	
	
	Public Property SelStart() As Integer
		Get
			SelStart = ExText.SelectionStart
		End Get
		Set(ByVal Value As Integer)
			If Not DesignMode = False Then Err.Raise(382)
			ExText.SelectionStart = Value
			RaiseEvent SelStartChange()
		End Set
	End Property
	
	
	Public Property SelLength() As Integer
		Get
			SelLength = ExText.SelectionLength
		End Get
		Set(ByVal Value As Integer)
			If Not DesignMode = False Then Err.Raise(382)
			ExText.SelectionLength = Value
			RaiseEvent SelLengthChange()
		End Set
	End Property
	
	
	Public Property SelText() As String
		Get
			SelText = ExText.SelectedText
		End Get
		Set(ByVal Value As String)
			If Not DesignMode = False Then Err.Raise(382)
			ExText.SelectedText = Value
			RaiseEvent SelTextChange()
		End Set
	End Property
	
	
	Public Overrides Property ForeColor() As System.Drawing.Color
		Get
			ForeColor = ExText.ForeColor
		End Get
		Set(ByVal Value As System.Drawing.Color)
			ExText.ForeColor = Value
			RaiseEvent ForeColorChange()
		End Set
	End Property
	
	
	Public Overrides Property BackColor() As System.Drawing.Color
		Get
			BackColor = ExText.BackColor
		End Get
		Set(ByVal Value As System.Drawing.Color)
			ExText.BackColor = Value
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
	
	
	Public Shadows Property IMEMode() As IMEModeType
		Get
			'UPGRADE_WARNING: オブジェクト ExText.IMEMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			IMEMode = ExText.IMEMode
		End Get
		Set(ByVal Value As IMEModeType)
			ExText.IMEMode = Value
			RaiseEvent IMEModeChange()
		End Set
	End Property
	
	
	Public Overrides Property Font() As System.Drawing.Font
		Get
			Font = ExText.Font
		End Get
		Set(ByVal Value As System.Drawing.Font)
			'''    Dim BufLogFont As LOGFONT
			'''    Dim lngNewFontHandle As Long
			'''    Dim MyByte() As Byte, i As Integer
			
			'''    With BufLogFont
			'''        .lfHeight = -(New_Font.Size * 20 / Screen.TwipsPerPixelX)
			'''        .lfCharSet = New_Font.Charset
			'''        .lfItalic = New_Font.Italic
			'''        .lfUnderline = New_Font.Underline
			'''        .lfStrikeOut = New_Font.Strikethrough
			'''        If New_Font.Weight = 0 Then
			'''            If New_Font.Bold = True Then .lfWeight = 700
			'''            If New_Font.Bold = False Then .lfWeight = 400
			'''        Else
			'''            .lfWeight = New_Font.Weight
			'''        End If
			'''        MyByte() = StrConv(New_Font.Name, vbFromUnicode)
			'''        For i = 0 To UBound(MyByte)
			'''            .lfFaceName(i) = MyByte(i)
			'''        Next i
			'''        .lfEscapement = 0
			'''        .lfOrientation = 0
			'''    End With
			ExText.Font = Value
			'''''''    Call UserControl_Resize          '2002/10/24 DEL
			'    lngNewFontHandle = CreateFontIndirect(BufLogFont)
			'    SendMessage ExText.hwnd, WM_SETFONT, lngNewFontHandle, ByVal 1
			RaiseEvent FontChange()
		End Set
	End Property
	
	
	Public Property CharacterSize() As CharSize
		Get
			CharacterSize = pCharSize
		End Get
		Set(ByVal Value As CharSize)
			pCharSize = Value
			RaiseEvent CharacterSizeChange()
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
			Hwnd = ExText.Handle.ToInt32
		End Get
	End Property
	
	
	Public Property LengthType() As LenType
		Get
			LengthType = pLengthType
		End Get
		Set(ByVal Value As LenType)
			pLengthType = Value
			RaiseEvent LengthTypeChange()
		End Set
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
	
	
	'UPGRADE_ISSUE: VBRUN.OLEDragConstants 型 はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6B85A2A7-FE9F-4FBE-AA0C-CF11AC86A305"' をクリックしてください。
	'UPGRADE_ISSUE: VBRUN.OLEDragConstants 型 はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6B85A2A7-FE9F-4FBE-AA0C-CF11AC86A305"' をクリックしてください。
	Public Property OLEDragMode() As Object
		Get '2002/10/24 ADD
			'UPGRADE_ISSUE: TextBox プロパティ ExText.OLEDragMode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
			OLEDragMode = ExText.OLEDragMode()
		End Get
		Set(ByVal Value As Object) '2002/10/24 ADD
			'UPGRADE_ISSUE: TextBox プロパティ ExText.OLEDragMode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
			ExText.OLEDragMode = Value
			RaiseEvent OLEDragModeChange()
		End Set
	End Property
	
	
	'UPGRADE_ISSUE: VBRUN.OLEDropConstants 型 はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6B85A2A7-FE9F-4FBE-AA0C-CF11AC86A305"' をクリックしてください。
	'UPGRADE_ISSUE: VBRUN.OLEDropConstants 型 はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6B85A2A7-FE9F-4FBE-AA0C-CF11AC86A305"' をクリックしてください。
	Public Property OLEDropMode() As Object
		Get '2002/10/24 ADD
			'UPGRADE_ISSUE: TextBox プロパティ ExText.OLEDropMode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
			OLEDropMode = ExText.OLEDropMode()
		End Get
		Set(ByVal Value As Object) '2002/10/24 ADD
			'UPGRADE_ISSUE: TextBox プロパティ ExText.OLEDropMode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
			ExText.OLEDropMode = Value
			RaiseEvent OLEDropModeChange()
		End Set
	End Property
	
	
	Public Property PasswordChar() As String
		Get '2002/10/24 ADD
			PasswordChar = CStr(ExText.PasswordChar())
		End Get
		Set(ByVal Value As String) '2002/10/24 ADD
			'    If LenB(New_PasswordChar) > 1 Then Err.Raise 380
			ExText.PasswordChar = CChar(Value)
			RaiseEvent PasswordCharChange()
		End Set
	End Property
	
	
	Public Property MousePointer() As System.Windows.Forms.Cursor
		Get '2002/10/24 ADD
			MousePointer = ExText.Cursor
		End Get
		Set(ByVal Value As System.Windows.Forms.Cursor) '2002/10/24 ADD
			'UPGRADE_ISSUE: TextBox プロパティ ExText.MousePointer はカスタム マウスポインタをサポートしません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="45116EAB-7060-405E-8ABE-9DBB40DC2E86"' をクリックしてください。
			ExText.Cursor = Value
			RaiseEvent MousePointerChange()
		End Set
	End Property
	
	
	Public Property MouseIcon() As System.Drawing.Image
		Get '2002/10/24 ADD
			'UPGRADE_ISSUE: TextBox プロパティ ExText.MouseIcon はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
			MouseIcon = ExText.MouseIcon
		End Get
		Set(ByVal Value As System.Drawing.Image) '2002/10/24 ADD
			'UPGRADE_ISSUE: TextBox プロパティ ExText.MouseIcon はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
			ExText.MouseIcon = Value
			RaiseEvent MouseIconChange()
		End Set
	End Property
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
			Call PostMessage(ExText.Handle.ToInt32, WM_KEYDOWN, System.Windows.Forms.Keys.Tab, 0)
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
	
	Public Sub OLEDrag() '2002/10/24 ADD
		'UPGRADE_ISSUE: TextBox メソッド ExText.OLEDrag はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		ExText.OLEDrag()
	End Sub
End Class