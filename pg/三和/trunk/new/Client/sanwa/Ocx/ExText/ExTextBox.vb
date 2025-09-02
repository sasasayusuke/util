Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Runtime.InteropServices

<ToolboxItem(True)>
Public Class ExTextBox
	Inherits System.Windows.Forms.UserControl
'	Implements ISupportInitialize

	Public Event IMEModeChange()
	Public Event EditModeChange()
	Public Event CanForwardSetFocusChange()
	Public Event CanNextSetFocusChange()
	Public Event ForeColorChange()
	Public Event BackColorChange()
	Public Event BorderStyleChange()
	Public Event FocusBackColorChange()
	Public Event OldValueChange()
	Public Event AlignmentChange()
	Public Event TextChange()
	Public Event AppearanceChange()
	Public Event EnabledChange()
	Public Event LockedChange()
	Public Event SelectTextChange()
	Public Event SelStartChange()
	Public Event SelTextChange()
	Public Event SelLengthChange()
	Public Event MaxLengthChange()
	Public Event FontChange()
	Public Event OLEDragModeChange()
	Public Event OLEDropModeChange()
	Public Event CharacterSizeChange()
	Public Event LengthTypeChange()
	Public Event MouseIconChange()
	Public Event MousePointerChange()
	Public Event PasswordCharChange()


	'UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
	'Private Declare Function SendMessage Lib "user32"  Alias "SendMessageA"(ByVal Hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer, ByRef lParam As Any) As Integer
	<DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
	Private Shared Function SendMessage(ByVal hwnd As IntPtr, ByVal msg As UInteger, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
	End Function

	Private Const EM_LIMITTEXT As Integer = &HC5
	''Private Const EM_CANUNDO = &HC6
	''Private Const EM_UNDO = &HC7
	''Private Const EM_EMPTYUNDOBUFFER = &HCD
	''Private Const EM_SETREADONLY = &HCF
	Private Const WM_KEYDOWN As Integer = &H100
	''Private Const EM_GETMODIFY = &HB8
	''Private Const EM_SETMODIFY = &HB9

	'' ウィンドウへメッセージをポストするAPI
	'UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
	'Private Declare Function PostMessage Lib "user32" Alias "PostMessageA" (ByVal Hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer, ByRef lParam As Any) As Integer
	<DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
	Private Shared Function PostMessage(ByVal hwnd As IntPtr, ByVal wMsg As UInteger, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As Boolean
	End Function

	''Private Const WM_KEYDOWN = &H100
	''Private Const WM_KEYUP = &H101
	''Private Const VK_TAB = &H9
	'キーを送るAPI
	'Private Declare Sub keybd_event Lib "user32" (ByVal bVk As Byte, ByVal bScan As Byte, ByVal dwFlags As Integer, ByVal dwExtraInfo As Integer)
	<DllImport("user32.dll", SetLastError:=True)>
	Private Shared Sub keybd_event(ByVal bVk As Byte, ByVal bScan As Byte, ByVal dwFlags As UInteger, ByVal dwExtraInfo As UIntPtr)
	End Sub

	Private Const KEYEVENTF_KEYUP As Integer = &H2

	'列挙型
	''' <summary>
	''' IMEModeType
	''' </summary>
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

	''' <summary>
	''' CharSize
	''' </summary>
	Enum CharSize
		なし = 0
		大文字 = 1
		小文字 = 2
	End Enum

	''' <summary>
	''' LenType
	''' </summary>
	Enum LenType
		UnicodeType = 0
		ByteType = 1
	End Enum

	''' <summary>
	''' AppearanceType
	''' </summary>
	Enum AppearanceType
		フラット = 0
		立体 = 1
	End Enum

	''' <summary>
	''' BorderStyleType
	''' </summary>
	Enum BorderStyleType
		なし = 0
		実線 = 1
	End Enum

	'プロパティ変数
	Private pFocusBackColor As System.Drawing.Color
	Private hBackColor As System.Drawing.Color
	Private pMaxLength As Integer
	Private pLengthType As LenType
	Private pCharSize As CharSize
	Private pCanNextSetFocus As Boolean
	Private pCanForwardSetFocus As Boolean
	Private pSelectText As Boolean
	Private pEditMode As Boolean

	'変数
	Private fGotFocus As Boolean
	Private fClicking As Boolean
	Private vUNDOBUF As String
	'Private fUNDOBUF  As Boolean
	Private MyParentName As IntPtr

	'公開イベント
	Public Shadows Event KeyPress As KeyPressEventHandler
	Public Shadows Event KeyDown As KeyEventHandler

	'互換イベント
	Public Event Change(ByVal Sender As System.Object, ByVal e As System.EventArgs)
	Public Shadows Event Click(ByVal Sender As System.Object, ByVal e As System.EventArgs)
	Public Event DblClick(ByVal Sender As System.Object, ByVal e As System.EventArgs)
	'Public Shadows Event KeyDownVB6(ByVal Sender As System.Object, ByVal e As KeyDownEventArgs)
	'Public Shadows Event KeyPressVB6(ByVal Sender As System.Object, ByVal e As KeyPressEventArgs)
	'Public Shadows Event KeyUpVB6(ByVal Sender As System.Object, ByVal e As KeyUpEventArgs)
	'Public Shadows Event MouseDownVB6(ByVal Sender As System.Object, ByVal e As MouseDownEventArgs)
	'Public Shadows Event MouseMoveVB6(ByVal Sender As System.Object, ByVal e As MouseMoveEventArgs)
	'Public Shadows Event MouseUpVB6(ByVal Sender As System.Object, ByVal e As MouseUpEventArgs)
	''''Public Event LinkClose()
	''''Public Event LinkError(LinkErr As Integer)
	''''Public Event LinkNotify()
	''''Public Event LinkOpen(Cancel As Integer)
	'Public Event OLECompleteDrag(ByVal Sender As System.Object, ByVal e As OLECompleteDragEventArgs)
	'Public Event OLEDragDrop(ByVal Sender As System.Object, ByVal e As OLEDragDropEventArgs)
	'Public Event OLEDragOver(ByVal Sender As System.Object, ByVal e As OLEDragOverEventArgs)
	'Public Event OLEGiveFeedback(ByVal Sender As System.Object, ByVal e As OLEGiveFeedbackEventArgs)
	'Public Event OLESetData(ByVal Sender As System.Object, ByVal e As OLESetDataEventArgs)
	'Public Event OLEStartDrag(ByVal Sender As System.Object, ByVal e As OLEStartDragEventArgs)

	'Public Event RtnKeyDownVB6(ByVal Sender As System.Object, ByVal e As RtnKeyDownEventArgs)
	'Public Event SpcKeyPressVB6(ByVal Sender As System.Object, ByVal e As SpcKeyPressEventArgs)

	' 標準の KeyPressEventArgs に基づくオーバーライド
	'Protected Overrides Sub OnKeyPress(e As System.Windows.Forms.KeyPressEventArgs)
	'	' Raise カスタムイベント（必要な場合のみ）
	'	RaiseEvent KeyPressVB6(Me, New KeyPressEventArgs(AscW(e.KeyChar)))
	'
	'	MyBase.OnKeyPress(e) ' 標準の KeyPress イベントがこれで自動的に発生する
	'End Sub

	' スペースキーの押下イベント
	Public NotInheritable Class SpcKeyPressEventArgs
		Inherits EventArgs

		Public Property KeyAscii As Integer
		Public Property Cancel As Boolean

		Public Sub New(keyAscii As Integer, cancel As Boolean)
			Me.KeyAscii = keyAscii
			Me.Cancel = cancel
		End Sub
	End Class

	' リターンキーのダウンイベント
	Public NotInheritable Class RtnKeyDownEventArgs
		Inherits EventArgs

		Public Property KeyCode As Integer
		Public Property Shift As Integer
		Public Property Cancel As Boolean

		Public Sub New(keyCode As Integer, shift As Integer, cancel As Boolean)
			Me.KeyCode = keyCode
			Me.Shift = shift
			Me.Cancel = cancel
		End Sub
	End Class

	' ドラッグ開始イベント
	Public NotInheritable Class OLEStartDragEventArgs
		Inherits EventArgs

		Public Property Data As IDataObject
		Public Property AllowedEffects As DragDropEffects

		Public Sub New(data As IDataObject, allowedEffects As DragDropEffects)
			Me.Data = data
			Me.AllowedEffects = allowedEffects
		End Sub
	End Class

	' データ設定イベント
	Public NotInheritable Class OLESetDataEventArgs
		Inherits EventArgs

		Public Property Data As IDataObject
		Public Property DataFormat As Short

		Public Sub New(data As IDataObject, dataFormat As Short)
			Me.Data = data
			Me.DataFormat = dataFormat
		End Sub
	End Class

	' フィードバックイベント
	Public NotInheritable Class OLEGiveFeedbackEventArgs
		Inherits EventArgs

		Public Property Effect As DragDropEffects
		Public Property DefaultCursors As Boolean

		Public Sub New(effect As DragDropEffects, defaultCursors As Boolean)
			Me.Effect = effect
			Me.DefaultCursors = defaultCursors
		End Sub
	End Class

	' ドラッグオーバーイベント
	Public NotInheritable Class OLEDragOverEventArgs
		Inherits EventArgs

		Public Property Data As IDataObject
		Public Property Effect As DragDropEffects
		Public Property Button As Integer
		Public Property Shift As Integer
		Public Property X As Single
		Public Property Y As Single
		Public Property State As Short

		Public Sub New(data As IDataObject, effect As DragDropEffects, button As Integer, shift As Integer, x As Single, y As Single, state As Short)
			Me.Data = data
			Me.Effect = effect
			Me.Button = button
			Me.Shift = shift
			Me.X = x
			Me.Y = y
			Me.State = state
		End Sub
	End Class

	' ドラッグドロップイベント
	Public NotInheritable Class OLEDragDropEventArgs
		Inherits EventArgs

		Public Property Data As IDataObject
		Public Property Effect As DragDropEffects
		Public Property Button As Integer
		Public Property Shift As Integer
		Public Property X As Single
		Public Property Y As Single

		Public Sub New(data As IDataObject, effect As DragDropEffects, button As Integer, shift As Integer, x As Single, y As Single)
			Me.Data = data
			Me.Effect = effect
			Me.Button = button
			Me.Shift = shift
			Me.X = x
			Me.Y = y
		End Sub
	End Class

	' ドラッグ完了イベント
	Public NotInheritable Class OLECompleteDragEventArgs
		Inherits EventArgs

		Public Property Effect As DragDropEffects

		Public Sub New(effect As DragDropEffects)
			Me.Effect = effect
		End Sub
	End Class

	' マウス系イベント（Up, Down, Move）
	Public NotInheritable Class MouseUpEventArgs
		Inherits EventArgs

		Public Property Button As Integer
		Public Property Shift As Integer
		Public Property X As Single
		Public Property Y As Single

		Public Sub New(button As Integer, shift As Integer, x As Single, y As Single)
			Me.Button = button
			Me.Shift = shift
			Me.X = x
			Me.Y = y
		End Sub
	End Class

	Public NotInheritable Class MouseMoveEventArgs
		Inherits EventArgs

		Public Property Button As Integer
		Public Property Shift As Integer
		Public Property X As Single
		Public Property Y As Single

		Public Sub New(button As Integer, shift As Integer, x As Single, y As Single)
			Me.Button = button
			Me.Shift = shift
			Me.X = x
			Me.Y = y
		End Sub
	End Class

	Public NotInheritable Class MouseDownEventArgs
		Inherits EventArgs

		Public Property Button As Integer
		Public Property Shift As Integer
		Public Property X As Single
		Public Property Y As Single

		Public Sub New(button As Integer, shift As Integer, x As Single, y As Single)
			Me.Button = button
			Me.Shift = shift
			Me.X = x
			Me.Y = y
		End Sub
	End Class

	' キーイベント（Down, Up, Press）
	Public NotInheritable Class KeyDownEventArgs
		Inherits EventArgs

		Public Property KeyCode As Integer
		Public Property Shift As Integer

		Public Sub New(keyCode As Integer, shift As Integer)
			Me.KeyCode = keyCode
			Me.Shift = shift
		End Sub
	End Class

	Public NotInheritable Class KeyUpEventArgs
		Inherits EventArgs

		Public Property KeyCode As Integer
		Public Property Shift As Integer

		Public Sub New(keyCode As Integer, shift As Integer)
			Me.KeyCode = keyCode
			Me.Shift = shift
		End Sub
	End Class

	Public NotInheritable Class KeyPressEventArgs
		Inherits EventArgs

		Public Property KeyAscii As Integer

		Public Sub New(keyAscii As Integer)
			Me.KeyAscii = keyAscii
		End Sub
	End Class

	Private _DisplayName As String = Me.Name
	Private _LengthType As Integer
	Private _Enabled As Boolean
	Private _Locked As Boolean
	Private _MaxLength As Integer
	Private _ForeColor As String
	Private _BackColor As String
	Private _IMEMode As Integer
	Private _Font As Integer
	Private _FocusBackColor As String
	Private _CharacterSize As Integer
	Private _CanNextSetFocus As Boolean
	Private _CanForwardSetFocus As Boolean
	Private _SelectText As Boolean
	Private _EditMode As Boolean
	Private _Alignment As String
	Private _Appearance As String
	Private _BorderStyle As String
	Private _OldValue As String
	Private _OLEDragMode As String
	Private _OLEDropMode As String
	Private _PasswordChar As String
	Private _MousePointer As String
	Private _MouseIcon As String

	Private initializing As Boolean = True

	Public Sub Undo()
		'    Dim lret As Integer

		'    lret = SendMessage(ExText.Hwnd, EM_CANUNDO, 0, 0)
		'    If lret <> 0 Then
		'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		ExText.Text = vUNDOBUF.ToString
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
	Private Sub ExText_TextChanged(sender As Object, e As EventArgs) Handles ExText.TextChanged
		RaiseEvent Change(Me, Nothing)
	End Sub

	Private Sub ExText_Click(sender As Object, e As EventArgs) Handles ExText.Click
		RaiseEvent Click(Me, Nothing)
	End Sub

	Private Sub ExText_DoubleClick(sender As Object, e As EventArgs) Handles ExText.DoubleClick
		RaiseEvent DblClick(Me, Nothing)
	End Sub

	Private Sub ExText_Enter(sender As Object, e As EventArgs) Handles ExText.Enter
		'    hBackColor = ExText.BackColor
		ExText.BackColor = pFocusBackColor

		''''    DoEvents
		Try
			If Me.Hwnd <> MyParentName Then
				''        If UserControl.Parent.ActiveControl.Hwnd <> MyParentName Then
				'    If UserControl.Parent.ActiveControl.Name <> MyParentName Then
				'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				vUNDOBUF = ExText.Text
			End If
			If Err.Number <> 0 Then
				'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				vUNDOBUF = ExText.Text
			End If
		Catch ex As Exception
			Err.Clear()
		End Try

		If pSelectText Then
			If fClicking = False Then
				ExText.SelectionStart = 0
				ExText.SelectionLength = Len(ExText.Text)
			End If
		End If
	End Sub

	Private Sub ExText_Leave(sender As Object, e As EventArgs) Handles ExText.Leave
		ExText.BackColor = hBackColor
		'UPGRADE_WARNING: Control プロパティ UserControl.Parent は、新しい動作をもつ UserControl.FindForm にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="DFCDE711-9694-47D7-9C50-45A99CD8E91E"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト UserControl.Parent.ActiveControl の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_ISSUE: Control Hwnd は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		'MyParentName = UserControl.FindForm.ActiveControl.Hwnd
		Try
			Dim parentControl = Me.FindForm
			Dim activeControl = parentControl.ActiveControl
			Dim hwnd As IntPtr = If(activeControl IsNot Nothing, activeControl.Handle, IntPtr.Zero)
			MyParentName = hwnd
			Err.Clear()
		Catch ex As Exception
			Err.Clear()
		End Try
	End Sub

	Private Sub ExText_KeyDown(sender As Object, e As KeyEventArgs) Handles ExText.KeyDown
		Dim KeyCode As Integer = e.KeyCode
		Dim Shift As Integer = e.KeyData \ &H10000
		Dim Rtn_Cancel As Boolean

		If Shift = 0 Then
			Select Case KeyCode
				Case System.Windows.Forms.Keys.Return
					RaiseEvent KeyDown(Me, e)
					'            Case vbKeySpace
					'                RaiseEvent SpcKeyDown(KeyCode, Shift, Spc_Cancel)
			End Select
		End If
		'RaiseEvent KeyDownVB6(Me, New KeyDownEventArgs(KeyCode, Shift))
		RaiseEvent KeyDown(Me, e)

		If KeyCode = System.Windows.Forms.Keys.Return And Rtn_Cancel = False Then
			If pCanNextSetFocus = True Then
				e.SuppressKeyPress = True
				Call NextSetFocus()
			Else
				e.Handled = True
			End If
		ElseIf KeyCode = System.Windows.Forms.Keys.Down Then
			If pCanNextSetFocus = True And CtlCursorCondition(ExText) = -1 Then
				e.SuppressKeyPress = True
				Call NextSetFocus()
			Else
				e.Handled = True
			End If
		ElseIf KeyCode = System.Windows.Forms.Keys.Up Then
			If pCanForwardSetFocus = True And CtlCursorCondition(ExText) = -1 Then
				e.SuppressKeyPress = True
				Call ForwardSetFocus()
			Else
				e.Handled = True
			End If
		ElseIf KeyCode = System.Windows.Forms.Keys.Insert Then
			'入力モードにする
			e.SuppressKeyPress = True
			If CtlCursorCondition(ExText) = -1 Then
				ExText.SelectionStart = Len(ExText.Text)
			Else
				ExText.SelectionStart = 0
				ExText.SelectionLength = Len(ExText.Text)
			End If
		ElseIf KeyCode = System.Windows.Forms.Keys.Left Then
			If pEditMode = False Then
				If pCanForwardSetFocus = True And CtlCursorCondition(ExText) = -1 Then
					e.SuppressKeyPress = True
					Call ForwardSetFocus()
				End If
			End If
		ElseIf KeyCode = System.Windows.Forms.Keys.Right Then
			If pEditMode = False Then
				If pCanNextSetFocus = True And CtlCursorCondition(ExText) = -1 Then
					e.SuppressKeyPress = True
					Call NextSetFocus()
				End If
			End If
		End If
	End Sub

	Private Sub ExText_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles ExText.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		'Dim Spc_Cancel As Boolean

		If KeyAscii = System.Windows.Forms.Keys.Return Then KeyAscii = 0
		''    If KeyAscii = vbKeyReturn Then
		''        If pCanNextSetFocus = True Then
		''            KeyAscii = 0
		''            NextSetFocus
		''        Else
		''            KeyAscii = 0
		''        End If
		''    End If
		'Select Case KeyAscii
		'	Case System.Windows.Forms.Keys.Space
		'		RaiseEvent SpcKeyPressVB6(Me, New SpcKeyPressEventArgs(KeyAscii, Spc_Cancel))
		'		KeyAscii = 0
		'End Select

		Select Case pCharSize
			Case 1
				KeyAscii = Asc(UCase(Chr(KeyAscii)))
			Case 2
				KeyAscii = Asc(LCase(Chr(KeyAscii)))
		End Select

		'RaiseEvent KeyPressVB6(Me, New KeyPressEventArgs(KeyAscii))
		RaiseEvent KeyPress(Me, e)

		' キー入力の処理
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub ExText_KeyUp(sender As Object, e As KeyEventArgs) Handles ExText.KeyUp
		Dim KeyCode As Integer = e.KeyCode
		Dim Shift As Integer = e.KeyData \ &H10000
		'RaiseEvent KeyUp(Me, e)
	End Sub

	Private Sub ExText_MouseDown(sender As Object, e As MouseEventArgs) Handles ExText.MouseDown
		Dim Button As Integer = e.Button \ &H100000
		Dim Shift As Integer = System.Windows.Forms.Control.ModifierKeys \ &H10000
		'Dim X As Single = VB6Conv.PixelsToTwipsX(eventArgs.X)
		'Dim Y As Single = VB6Conv.PixelsToTwipsY(eventArgs.Y)
		Dim X As Single = e.X
		Dim Y As Single = e.Y
		'RaiseEvent MouseDown(Me, e)
		fClicking = True
	End Sub

	Private Sub ExText_MouseMove(sender As Object, e As MouseEventArgs) Handles ExText.MouseMove
		Dim Button As Integer = e.Button \ &H100000
		Dim Shift As Integer = System.Windows.Forms.Control.ModifierKeys \ &H10000
		'Dim X As Single = VB6Conv.PixelsToTwipsX(eventArgs.X)
		'Dim Y As Single = VB6Conv.PixelsToTwipsY(eventArgs.Y)
		Dim X As Single = e.X
		Dim Y As Single = e.Y
		'RaiseEvent MouseMove(Me, e)
	End Sub

	Private Sub ExText_MouseUp(sender As Object, e As MouseEventArgs) Handles ExText.MouseUp
		Dim Button As Integer = e.Button \ &H100000
		Dim Shift As Integer = System.Windows.Forms.Control.ModifierKeys \ &H10000
		'Dim X As Single = VB6Conv.PixelsToTwipsX(eventArgs.X)
		'Dim Y As Single = VB6Conv.PixelsToTwipsY(eventArgs.Y)
		Dim X As Single = e.X
		Dim Y As Single = e.Y
		'RaiseEvent MouseUp(Me, e)
		fClicking = False
	End Sub

	'UPGRADE_ISSUE: TextBox イベント ExText.OLECompleteDrag はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="ABD9AF39-7E24-4AFF-AD8D-3675C1AA3054"' をクリックしてください。
	'Private Sub ExText_OLECompleteDrag(ByRef Effect As Integer) '2002/10/24 ADD
	'	RaiseEvent OLECompleteDrag(Me, New OLECompleteDragEventArgs(Effect))
	'End Sub

	'UPGRADE_ISSUE: VBRUN.DataObject 型 はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6B85A2A7-FE9F-4FBE-AA0C-CF11AC86A305"' をクリックしてください。
	'UPGRADE_ISSUE: TextBox イベント ExText.OLEDragDrop はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="ABD9AF39-7E24-4AFF-AD8D-3675C1AA3054"' をクリックしてください。
	'Private Sub ExText_OLEDragDrop(ByRef Data As Object, ByRef Effect As Integer, ByRef Button As Integer, ByRef Shift As Integer, ByRef X As Single, ByRef Y As Single) '2002/10/24 ADD
	'	RaiseEvent OLEDragDrop(Me, New OLEDragDropEventArgs(Data, Effect, Button, Shift, X, Y))
	'End Sub

	'UPGRADE_ISSUE: VBRUN.DataObject 型 はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6B85A2A7-FE9F-4FBE-AA0C-CF11AC86A305"' をクリックしてください。
	'UPGRADE_ISSUE: TextBox イベント ExText.OLEDragOver はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="ABD9AF39-7E24-4AFF-AD8D-3675C1AA3054"' をクリックしてください。
	'Private Sub ExText_OLEDragOver(ByRef Data As Object, ByRef Effect As Integer, ByRef Button As Integer, ByRef Shift As Integer, ByRef X As Single, ByRef Y As Single, ByRef State As Short) '2002/10/24 ADD
	'	RaiseEvent OLEDragOver(Me, New OLEDragOverEventArgs(Data, Effect, Button, Shift, X, Y, State))
	'End Sub

	'UPGRADE_ISSUE: TextBox イベント ExText.OLEGiveFeedback はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="ABD9AF39-7E24-4AFF-AD8D-3675C1AA3054"' をクリックしてください。
	'Private Sub ExText_OLEGiveFeedback(ByRef Effect As Integer, ByRef DefaultCursors As Boolean) '2002/10/24 ADD
	'	RaiseEvent OLEGiveFeedback(Me, New OLEGiveFeedbackEventArgs(Effect, DefaultCursors))
	'End Sub

	'UPGRADE_ISSUE: VBRUN.DataObject 型 はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6B85A2A7-FE9F-4FBE-AA0C-CF11AC86A305"' をクリックしてください。
	'UPGRADE_ISSUE: TextBox イベント ExText.OLESetData はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="ABD9AF39-7E24-4AFF-AD8D-3675C1AA3054"' をクリックしてください。
	'Private Sub ExText_OLESetData(ByRef Data As Object, ByRef DataFormat As Short) '2002/10/24 ADD
	'	RaiseEvent OLESetData(Me, New OLESetDataEventArgs(Data, DataFormat))
	'End Sub

	'UPGRADE_ISSUE: VBRUN.DataObject 型 はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6B85A2A7-FE9F-4FBE-AA0C-CF11AC86A305"' をクリックしてください。
	'UPGRADE_ISSUE: TextBox イベント ExText.OLEStartDrag はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="ABD9AF39-7E24-4AFF-AD8D-3675C1AA3054"' をクリックしてください。
	'Private Sub ExText_OLEStartDrag(ByRef Data As Object, ByRef AllowedEffects As Integer) '2002/10/24 ADD
	'	RaiseEvent OLEStartDrag(Me, New OLEStartDragEventArgs(Data, AllowedEffects))
	'End Sub

	Private Sub ExTextBox_Enter(sender As Object, e As EventArgs) Handles MyBase.Enter
		fGotFocus = True
		Try
			' ''    DoEvents
			If Me.Hwnd <> MyParentName Then
				'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				vUNDOBUF = ExText.Text
			End If
			If Err.Number <> 0 Then
				'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				vUNDOBUF = ExText.Text
			End If
			'''''''    ExText.SetFocus
		Catch ex As Exception
			Err.Clear()
		End Try
	End Sub

	Private Sub ExTextBox_Leave(sender As Object, e As EventArgs) Handles MyBase.Leave
		fGotFocus = False
		fClicking = False
	End Sub

	'UPGRADE_ISSUE: UserControl イベント UserControl.Terminate はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="ABD9AF39-7E24-4AFF-AD8D-3675C1AA3054"' をクリックしてください。
	Protected Overrides Sub OnHandleDestroyed(e As EventArgs)
		' コントロール破棄時の処理
		'終了時にIMEModeを0-なしにしないと
		'WinMEでimm32.dllがこける。
		ExText.ImeMode = System.Windows.Forms.ImeMode.NoControl
	End Sub

	Private Sub ExTextBox_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
		If ExText Is Nothing Then Exit Sub ' ExText が未初期化なら何もしない

		With ExText
			.Top = 0
			.Left = 0
			.Width = MyBase.Width
			'UPGRADE_ISSUE: UserControl プロパティ ExTextBox.Extender はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト Extender.Width の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'Me.Width = VB6Conv.PixelsToTwipsX(.Width) '2002/08/15 ADD
			Me.Width = .Width
			.Height = MyBase.Height
			'UPGRADE_ISSUE: UserControl プロパティ ExTextBox.Extender はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト Extender.Height の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'Me.Height = VB6Conv.PixelsToTwipsY(.Height) '2002/08/15 ADD
			Me.Height = .Height
			'''''        UserControl.Height = .Height   '2002/08/15 DEL
			''        .Height = Extender.Height
			''        .Width = Extender.Width
		End With
	End Sub

	'UPGRADE_ISSUE: UserControl イベント UserControl.InitProperties はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="ABD9AF39-7E24-4AFF-AD8D-3675C1AA3054"' をクリックしてください。
	Public Sub New()
		InitializeComponent()

		If Not Me.DesignMode Then
			'UPGRADE_ISSUE: AmbientProperties プロパティ Ambient.DisplayName はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
			Me.Text = Me.Name
			Me.FocusBackColor = System.Drawing.SystemColors.Window
			Me.ForeColor = System.Drawing.SystemColors.WindowText
			Me.BackColor = System.Drawing.SystemColors.Window
			'UPGRADE_ISSUE: AmbientProperties プロパティ Ambient.Font はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
			'Me.Font = Ambient.Font
			'Me.Appearance = AppearanceType.立体 '01/12/20
			Me.BorderStyle = BorderStyleType.実線 '01/12/20
		End If

		pCanNextSetFocus = True
		pCanForwardSetFocus = True
		pSelectText = True
		pEditMode = True
		pLengthType = LenType.ByteType

	End Sub

	'UPGRADE_ISSUE: VBRUN.PropertyBag 型 はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6B85A2A7-FE9F-4FBE-AA0C-CF11AC86A305"' をクリックしてください。
	'UPGRADE_WARNING: UserControl イベント ReadProperties はサポートされていません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="92F3B58C-F772-4151-BE90-09F4A232AEAD"' をクリックしてください。
	'Protected Overrides Sub OnHandleCreated(ByVal e As EventArgs)
	'	MyBase.OnHandleCreated(e)
    '
	'	'UPGRADE_ISSUE: AmbientProperties プロパティ Ambient.DisplayName はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
	'	'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
	'	'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
	'	_DisplayName = My.Settings.DisplayName
	'	ExText.Text = _DisplayName ' Ambient.DisplayName
    '
	'	pLengthType = My.Settings.LengthType
    '
	'	Me.Enabled = My.Settings.Enabled
	'	Me.Locked = My.Settings.Locked
	'	Me.MaxLength = My.Settings.MaxLength
	'	Me.ForeColor = My.Settings.ForeColor
	'	Me.BackColor = My.Settings.BackColor
	'	Me.IMEMode = My.Settings.IMEMode
	'	Me.Font = My.Settings.Font
    '
	'	pFocusBackColor = My.Settings.FocusBackColor
	'	pCharSize = My.Settings.CharacterSize
	'	pCanNextSetFocus = My.Settings.CanNextSetFocus
	'	pCanForwardSetFocus = My.Settings.CanForwardSetFocus
	'	pSelectText = My.Settings.SelectText
	'	pEditMode = My.Settings.EditMode
    '
	'	'ExText.Alignment = My.Settings.Alignment
	'	'ExText.Appearance = My.Settings.Appearance
	'	ExText.BorderStyle = My.Settings.BorderStyle
    '
	'	vUNDOBUF = My.Settings.OldValue
    '
	'	'ExText.OLEDragMode = My.Settings.OLEDragMode
	'	'ExText.OLEDropMode = My.Settings.OLEDropMode
	'	ExText.PasswordChar = My.Settings.PasswordChar
	'	'ExText.MousePointer = My.Settings.MousePointer
	'	'ExText.MouseIcon = My.Settings.MouseIcon
    '
	'End Sub

	'UPGRADE_ISSUE: VBRUN.PropertyBag 型 はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6B85A2A7-FE9F-4FBE-AA0C-CF11AC86A305"' をクリックしてください。
	'UPGRADE_WARNING: UserControl イベント WriteProperties はサポートされていません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="92F3B58C-F772-4151-BE90-09F4A232AEAD"' をクリックしてください。

	' ISupportInitialize の初期化の開始
	'Public Sub BeginInit() Implements ISupportInitialize.BeginInit
	'	initializing = True
	'End Sub
    '
	'Public Sub EndInit() Implements ISupportInitialize.EndInit
	'	initializing = False
	'	SaveProperties()
	'End Sub
    '
	'' プロパティを保存する処理
	'Private Sub SaveProperties()
    '
	'	My.Settings.DisplayName = _DisplayName
	'	My.Settings.Enabled = Me.Enabled
	'	My.Settings.Locked = Me.Locked
	'	My.Settings.MaxLength = Me.MaxLength
	'	My.Settings.ForeColor = Me.ForeColor
	'	My.Settings.BackColor = Me.BackColor
	'	My.Settings.IMEMode = Me.IMEMode
	'	My.Settings.Font = Me.Font
    '
	'	My.Settings.FocusBackColor = pFocusBackColor
	'	My.Settings.CharacterSize = pCharSize
	'	My.Settings.CanNextSetFocus = pCanNextSetFocus
	'	My.Settings.CanForwardSetFocus = pCanForwardSetFocus
	'	My.Settings.SelectText = pSelectText
	'	My.Settings.LengthType = pLengthType
	'	My.Settings.EditMode = pEditMode
    '
	'	'My.Settings.Alignment = ExText.Alignment
	'	'My.Settings.Appearance = ExText.Appearance
	'	My.Settings.BorderStyle = ExText.BorderStyle
    '
	'	My.Settings.OldValue = vUNDOBUF
    '
	'	'My.Settings.OLEDragMode = ExText.OLEDragMode
	'	'My.Settings.OLEDropMode = ExText.OLEDropMode
	'	My.Settings.PasswordChar = ExText.PasswordChar
	'	'My.Settings.MousePointer = ExText.MousePointer
	'	'My.Settings.MouseIcon = ExText.MouseIcon
    '
	'	My.Settings.Save()
	'End Sub

	<Browsable(True)> <Description("コントロールに含まれる文字を設定します。")>
	<DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
	Public Overrides Property Text() As String
		Get
			'UPGRADE_ISSUE: 定数 vbUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			'Text = StrConv(StrConv(ExText.Text(), vbFromUnicode), vbUnicode)
			Text = ExText.Text()
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
			If ExText IsNot Nothing Then
				ExText.Text = Value
			End If
			Try  '01/09/05
				If Me.Hwnd <> MyParentName Then
					'        If UserControl.Parent.ActiveControl.Hwnd <> MyParentName Then
					'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					vUNDOBUF = ExText.Text
				End If
			Catch ex As Exception
				Try
					'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					vUNDOBUF = ExText.Text
				Catch innerEx As Exception
				End Try
			End Try
			RaiseEvent TextChange()
		End Set
	End Property


	Public Shadows Property Enabled() As Boolean
		Get
			'    Enabled = ExText.Enabled
			Enabled = MyBase.Enabled
			'	Enabled = Me.Enabled
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

	<Browsable(True)> <Description("コントロール内のテキストの配置を設定します。")>
	Public Property Alignment() As HorizontalAlignment
		Get
			Alignment = ExText.TextAlign
		End Get
		Set(ByVal Value As HorizontalAlignment)
			Select Case Value
				Case 0
					ExText.TextAlign = HorizontalAlignment.Left
				Case 1
					ExText.TextAlign = HorizontalAlignment.Right
				Case 2
					ExText.TextAlign = HorizontalAlignment.Center
				Case Else
					ExText.TextAlign = HorizontalAlignment.Left
			End Select
			RaiseEvent AlignmentChange()
		End Set
	End Property

	'<Description("オブジェクトが、実行時に立体的に表示されるかどうかを設定します。")>
	'Public Property Appearance() As AppearanceType
	'    Get
	'        'UPGRADE_ISSUE: TextBox プロパティ ExText.Appearance はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
	'        Appearance = ExText.Appearance
	'    End Get
	'    Set(ByVal Value As AppearanceType)
	'        'UPGRADE_ISSUE: TextBox プロパティ ExText.Appearance はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
	'        ExText.Appearance = Value
	'        RaiseEvent AppearanceChange()
	'    End Set
	'End Property

	<Browsable(True)> <Description("オブジェクトの境界線のスタイルを設定します。")>
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


	<Browsable(True)> <Description("文字の最大数を設定します。")>
	Public Property MaxLength() As Integer
		Get
			MaxLength = ExText.MaxLength
			'MaxLength = pMaxLength
		End Get
		Set(ByVal Value As Integer)
			If pLengthType = LenType.UnicodeType Then
				'UPGRADE_WARNING: TextBox プロパティ ExText.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				ExText.MaxLength = Value
			Else
				'UPGRADE_WARNING: TextBox プロパティ ExText.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				ExText.MaxLength = 0
				Call SendMessage(ExText.Handle, EM_LIMITTEXT, Value, 0)
			End If
			pMaxLength = Value
			RaiseEvent MaxLengthChange()
		End Set
	End Property

	<Browsable(True)>
	Public Property SelStart() As Integer
		Get
			SelStart = ExText.SelectionStart
		End Get
		Set(ByVal Value As Integer)
			If LicenseManager.UsageMode = LicenseUsageMode.Designtime Then Err.Raise(382)
			ExText.SelectionStart = Value
			RaiseEvent SelStartChange()
		End Set
	End Property

	<Browsable(True)>
	Public Property SelLength() As Integer
		Get
			SelLength = ExText.SelectionLength
		End Get
		Set(ByVal Value As Integer)
			If LicenseManager.UsageMode = LicenseUsageMode.Designtime Then Err.Raise(382)
			ExText.SelectionLength = Value
			RaiseEvent SelLengthChange()
		End Set
	End Property

	<Browsable(True)>
	Public Property SelText() As String
		Get
			SelText = ExText.SelectedText
		End Get
		Set(ByVal Value As String)
			If LicenseManager.UsageMode = LicenseUsageMode.Designtime Then Err.Raise(382)
			ExText.SelectedText = Value
			RaiseEvent SelTextChange()
		End Set
	End Property

	<Browsable(True)> <Description("オブジェクト内の文字色を設定します。")>
	Public Overrides Property ForeColor() As System.Drawing.Color
		Get
			ForeColor = ExText.ForeColor
		End Get
		Set(ByVal Value As System.Drawing.Color)
			ExText.ForeColor = Value
			RaiseEvent ForeColorChange()
		End Set
	End Property

	<Browsable(True)> <Description("オブジェクト内の文字で使用する背景色を設定します。")>
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

	<Browsable(True)> <Description("フォーカスのある場合の背景色を設定します。")>
	Public Property FocusBackColor() As System.Drawing.Color
		Get
			FocusBackColor = pFocusBackColor
		End Get
		Set(ByVal Value As System.Drawing.Color)
			pFocusBackColor = Value
			RaiseEvent FocusBackColorChange()
		End Set
	End Property


	Public Shadows Property IMEMode() As ImeMode
		Get
			'UPGRADE_WARNING: オブジェクト ExText.IMEMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			IMEMode = ConvertToImeMode(ExText.ImeMode)
		End Get
		Set(ByVal Value As ImeMode)
			ExText.ImeMode = ConvertFromImeMode(Value)
			RaiseEvent IMEModeChange()
		End Set
	End Property

	Private Function ConvertToImeMode(value As ImeMode) As IMEModeType
		Select Case value
			Case ImeMode.On : Return IMEModeType.オン
			Case ImeMode.Off : Return IMEModeType.オフ
			Case ImeMode.Disable : Return IMEModeType.オフ固定
			Case ImeMode.Hiragana : Return IMEModeType.全角ひらがな
			Case ImeMode.Katakana : Return IMEModeType.全角カタカナ
			Case ImeMode.KatakanaHalf : Return IMEModeType.半角カタカナ
			Case ImeMode.AlphaFull : Return IMEModeType.全角英数
			Case Else : Return IMEModeType.なし
		End Select
	End Function

	' IMEModeType から .NET の ImeMode へ変換
	Private Function ConvertFromImeMode(mode As IMEModeType) As ImeMode
		Select Case mode
			Case IMEModeType.なし
				Return ImeMode.NoControl
			Case IMEModeType.オン
				Return ImeMode.On
			Case IMEModeType.オフ
				Return ImeMode.Off
			Case IMEModeType.オフ固定
				Return ImeMode.Disable
			Case IMEModeType.全角ひらがな
				Return ImeMode.Hiragana
			Case IMEModeType.全角カタカナ
				Return ImeMode.Katakana
			Case IMEModeType.半角カタカナ
				Return ImeMode.KatakanaHalf
			Case IMEModeType.全角英数
				Return ImeMode.AlphaFull
			Case Else
				Return ImeMode.NoControl
		End Select
	End Function

	Public Overrides Property Font() As System.Drawing.Font
		Get
			'Debug.Print("Get Font() called")
			Return MyBase.Font
		End Get
		Set(ByVal Value As System.Drawing.Font)
			' ''    Dim BufLogFont As LOGFONT
			' ''    Dim lngNewFontHandle As Long
			' ''    Dim MyByte() As Byte, i As Integer

			' ''    With BufLogFont
			' ''        .lfHeight = -(New_Font.Size * 20 / Screen.TwipsPerPixelX)
			' ''        .lfCharSet = New_Font.Charset
			' ''        .lfItalic = New_Font.Italic
			' ''        .lfUnderline = New_Font.Underline
			' ''        .lfStrikeOut = New_Font.Strikethrough
			' ''        If New_Font.Weight = 0 Then
			' ''            If New_Font.Bold = True Then .lfWeight = 700
			' ''            If New_Font.Bold = False Then .lfWeight = 400
			' ''        Else
			' ''            .lfWeight = New_Font.Weight
			' ''        End If
			' ''        MyByte() = StrConv(New_Font.Name, vbFromUnicode)
			' ''        For i = 0 To UBound(MyByte)
			' ''            .lfFaceName(i) = MyByte(i)
			' ''        Next i
			' ''        .lfEscapement = 0
			' ''        .lfOrientation = 0
			' ''    End With
			'Debug.Print("Set Font() called")
			ExText.Font = Value
			'''''''    Call UserControl_Resize          '2002/10/24 DEL
			'    lngNewFontHandle = CreateFontIndirect(BufLogFont)
			'    SendMessage ExText.hwnd, WM_SETFONT, lngNewFontHandle, ByVal 1
			RaiseEvent FontChange()
		End Set
	End Property

	<Browsable(True)> <Description("入力文字を大文字、小文字に変換する。")>
	Public Property CharacterSize() As CharSize
		Get
			CharacterSize = pCharSize
		End Get
		Set(ByVal Value As CharSize)
			pCharSize = Value
			RaiseEvent CharacterSizeChange()
		End Set
	End Property

	<Browsable(True)> <Description("テキストを選択状態にする。")>
	Public Property SelectText() As Boolean
		Get
			SelectText = pSelectText
		End Get
		Set(ByVal Value As Boolean)
			pSelectText = Value
			RaiseEvent SelectTextChange()
		End Set
	End Property

	<Browsable(True)> <Description("次にセットフォーカスする。")>
	Public Property CanNextSetFocus() As Boolean
		Get
			CanNextSetFocus = pCanNextSetFocus
		End Get
		Set(ByVal Value As Boolean)
			pCanNextSetFocus = Value
			RaiseEvent CanNextSetFocusChange()
		End Set
	End Property

	<Browsable(True)> <Description("前にセットフォーカスする。")>
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
	Public ReadOnly Property Hwnd() As IntPtr
		Get
			Hwnd = ExText.Handle
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

	<Browsable(True)> <Description("矢印キーでコントロール移動する。")>
	Public Property EditMode() As Boolean
		Get
			EditMode = pEditMode
		End Get
		Set(ByVal Value As Boolean)
			pEditMode = Value
			RaiseEvent EditModeChange()
		End Set
	End Property

	<Browsable(True)>
	Public Property OldValue() As String
		Get
			'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト OldValue の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			OldValue = vUNDOBUF
		End Get
		Set(ByVal Value As String)
			'UPGRADE_WARNING: オブジェクト New_OldValue の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			vUNDOBUF = Value
			RaiseEvent OldValueChange()
		End Set
	End Property


	'UPGRADE_ISSUE: VBRUN.OLEDragConstants 型 はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6B85A2A7-FE9F-4FBE-AA0C-CF11AC86A305"' をクリックしてください。
	'UPGRADE_ISSUE: VBRUN.OLEDragConstants 型 はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6B85A2A7-FE9F-4FBE-AA0C-CF11AC86A305"' をクリックしてください。
	'<Browsable(True)> <Description("このオブジェクトが OLE ドラッグ アンド ドロップのソースとして動作するかどうか、またはこのプロセスが自動的に開始されるか、プログラムの制御によって開始されるかどうかを設定します。値の取得も可能です。")>
	'Public Property OLEDragMode() As Object
	'	Get '2002/10/24 ADD
	'		'UPGRADE_ISSUE: TextBox プロパティ ExText.OLEDragMode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
	'		OLEDragMode = ExText.OLEDragMode()
	'	End Get
	'	Set(ByVal Value As Object) '2002/10/24 ADD
	'		'UPGRADE_ISSUE: TextBox プロパティ ExText.OLEDragMode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
	'		ExText.OLEDragMode = Value
	'		RaiseEvent OLEDragModeChange()
	'	End Set
	'End Property


	'UPGRADE_ISSUE: VBRUN.OLEDropConstants 型 はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6B85A2A7-FE9F-4FBE-AA0C-CF11AC86A305"' をクリックしてください。
	'UPGRADE_ISSUE: VBRUN.OLEDropConstants 型 はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6B85A2A7-FE9F-4FBE-AA0C-CF11AC86A305"' をクリックしてください。
	'<Browsable(True)> <Description("このコントロールが OLE ドロップ ターゲットとして動作するかどうかを設定します。値の取得も可能です。")>
	'Public Property OLEDropMode() As Object
	'	Get '2002/10/24 ADD
	'		'UPGRADE_ISSUE: TextBox プロパティ ExText.OLEDropMode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
	'		OLEDropMode = ExText.OLEDropMode()
	'	End Get
	'	Set(ByVal Value As Object) '2002/10/24 ADD
	'		'UPGRADE_ISSUE: TextBox プロパティ ExText.OLEDropMode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
	'		ExText.OLEDropMode = Value
	'		RaiseEvent OLEDropModeChange()
	'	End Set
	'End Property


	'<Browsable(True)> <Description("指定したコントロールをソースとして、OLE ドラッグ アンド ドロップ イベントを開始します。")>
	'Public Sub OLEDrag() '2002/10/24 ADD
	'	'UPGRADE_ISSUE: TextBox メソッド ExText.OLEDrag はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
	'	ExText.OLEDrag()
	'End Sub

	<Browsable(True)> <Description("コントロールに、入力された文字を表示するか、あるいはプレースホルダを表示するかどうかを設定します。値の取得も可能です。")>
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

	<Browsable(True)> <Description("オブジェクト上にマウス ポインタが置かれたときに表示される、マウス ポインタの種類を設定します。値の取得も可能です。")>
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

	'<Browsable(True)> <Description("カスタム マウス アイコンを設定します。")>
	'Public Property MouseIcon() As System.Drawing.Image
	'	Get '2002/10/24 ADD
	'		'UPGRADE_ISSUE: TextBox プロパティ ExText.MouseIcon はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
	'		MouseIcon = ExText.MouseIcon
	'	End Get
	'	Set(ByVal Value As System.Drawing.Image) '2002/10/24 ADD
	'		'UPGRADE_ISSUE: TextBox プロパティ ExText.MouseIcon はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
	'		ExText.MouseIcon = Value
	'		RaiseEvent MouseIconChange()
	'	End Set
	'End Property

	'メソッド
	Public Sub NextSetFocus()
		Try
			''    Lb_Cancel = False
			''    RaiseEvent EnterKeyDown(Lb_Cancel)
			''
			''    If Lb_Cancel = True Then Exit Sub
			''
			System.Windows.Forms.Application.DoEvents()

			'既にｶｰｿﾙが他へ遷移していたら処理しない
			If fGotFocus = True Then
				'        Call SendMessage(ExText.hWnd, WM_KEYDOWN, vbKeyTab, ByVal 0&)
				Call PostMessage(ExText.Handle, WM_KEYDOWN, System.Windows.Forms.Keys.Tab, 0)
				'        PostMessage ExText.Hwnd, WM_KEYDOWN, VK_TAB, 1
				'        PostMessage ExText.Hwnd, WM_KEYUP, VK_TAB, 1
				'        SendKeys "{TAB}"
			End If
			'    If UserControl.Enabled = True And pText.Enabled = True And pText.Visible = True Then
			'        SendKeys "{TAB}"
			'    End If
		Catch ex As Exception
		End Try
	End Sub

	Public Sub ForwardSetFocus()
		Try
			System.Windows.Forms.Application.DoEvents()

			'既にｶｰｿﾙが他へ遷移していたら処理しない
			If fGotFocus = True Then
				Call keybd_event(CByte(System.Windows.Forms.Keys.ShiftKey), 0, 0, 0)
				Call keybd_event(CByte(System.Windows.Forms.Keys.Tab), 0, 0, 0)
				Call keybd_event(CByte(System.Windows.Forms.Keys.ShiftKey), 0, KEYEVENTF_KEYUP, 0)
				Call keybd_event(CByte(System.Windows.Forms.Keys.Tab), 0, KEYEVENTF_KEYUP, 0)
			End If
		Catch ex As Exception
		End Try
	End Sub

	'コントロールのカーソル状態を検査
	Private Function CtlCursorCondition(Optional ByRef CheckControl As Object = Nothing) As Short
		Dim ctl As System.Windows.Forms.Control = If(CheckControl, Me)
		CtlCursorCondition = 0
		If TypeOf ctl Is TextBoxBase Then
			Dim textControl = DirectCast(ctl, TextBoxBase)

			Try
				' 選択テキストを確認
				Dim selectedText As String = textControl.SelectedText

				' 全選択されている場合
				If selectedText.Length = textControl.Text.Length Then
					Return -1 ' 全選択
				End If

				' 左端にカーソルがある場合
				If textControl.SelectionStart = 0 AndAlso textControl.SelectionLength <= 1 Then
					Return 2 ' 左端
				End If

				' 右端にカーソルがある場合
				If textControl.SelectionStart = textControl.Text.Length AndAlso textControl.SelectionLength = 0 Then
					Return 1 ' 右端
				End If

				' 右端の直前にカーソルがあり、1文字選択されている場合
				If textControl.SelectionStart = textControl.Text.Length - 1 AndAlso textControl.SelectionLength = 1 Then
					Return 1 ' 右端
				End If
			Catch ex As Exception
				' 例外が発生した場合、-1 を返す
				Return -1
			End Try
		Else
			' TextBoxBase 以外のコントロールの場合
			Return -1
		End If

		'Dim TextResult As Object
		''UPGRADE_NOTE: IsMissing() は IsNothing() に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="8AE1CB93-37AB-439A-A4FF-BE3B6760BB23"' をクリックしてください。
		'If IsNothing(CheckControl) Then
		'	ctl = Me
		'Else
		'	ctl = CheckControl
		'End If
		'With ctl
		'	'On Error Resume Next
		'	'UPGRADE_WARNING: オブジェクト ctl.SelText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'	'UPGRADE_WARNING: オブジェクト TextResult の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'	TextResult = ctl.SelectedText
		'	'--- エラーが発生していなければ
		'	If Err.Number = 0 Then
		'		'On Error GoTo 0
		'		'UPGRADE_WARNING: オブジェクト ctl.SelText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'		If Len(.SelText) = Len(.Text) Then
		'			CtlCursorCondition = -1 'selected all
		'			'UPGRADE_WARNING: オブジェクト ctl.SelLength の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'			'UPGRADE_WARNING: オブジェクト ctl.SelStart の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'		ElseIf .SelStart = 0 And .SelLength <= 1 Then 
		'			CtlCursorCondition = 2 'left side
		'			'UPGRADE_WARNING: オブジェクト ctl.SelLength の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'			'UPGRADE_WARNING: オブジェクト ctl.SelStart の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'		ElseIf .SelStart = Len(.Text) And .SelLength = 0 Then 
		'			CtlCursorCondition = 1 'right side
		'			'UPGRADE_WARNING: オブジェクト ctl.SelLength の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'			'UPGRADE_WARNING: オブジェクト ctl.SelStart の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'		ElseIf .SelStart = Len(.Text) - 1 And .SelLength = 1 Then 
		'			CtlCursorCondition = 1 'right side
		'		End If
		'	Else
		'		'On Error GoTo 0
		'		CtlCursorCondition = -1
		'	End If
		'End With
	End Function

	Public Sub ShowAdoutBox()
		FrmAbout.ShowDialog()
		FrmAbout.Close()
		'UPGRADE_NOTE: オブジェクト frmAbout をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		FrmAbout = Nothing
	End Sub

End Class