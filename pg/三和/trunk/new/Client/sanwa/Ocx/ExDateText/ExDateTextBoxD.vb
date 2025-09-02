Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Runtime.InteropServices

<ToolboxItem(True)>
Public Class ExDateTextBoxD
	Inherits System.Windows.Forms.UserControl
'	Implements ISupportInitialize

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

	'UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
	'Private Declare Function SendMessage Lib "user32"  Alias "SendMessageA"(ByVal Hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer, ByRef lParam As Any) As Integer
	<DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
	Private Shared Function SendMessage(ByVal hwnd As IntPtr, ByVal msg As UInteger, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
	End Function

	''Private Const EM_CANUNDO = &HC6         'UNDO可能か
	''Private Const EM_UNDO = &HC7            'UNDOする
	''Private Const EM_GETMODIFY = &HB8       '内容が変更されたかどうか
	''Private Const EM_EMPTYUNDOBUFFER = &HCD
	Private Const WM_KEYDOWN As Integer = &H100

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
	Private vUNDOBUF As String
	Private MyParentName As IntPtr
	Private Loaded As Boolean

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
	'Public Event OLECompleteDrag(ByVal Sender As System.Object, ByVal e As OLECompleteDragEventArgs)
	'Public Event OLEDragDrop(ByVal Sender As System.Object, ByVal e As OLEDragDropEventArgs)
	'Public Event OLEDragOver(ByVal Sender As System.Object, ByVal e As OLEDragOverEventArgs)
	'Public Event OLEGiveFeedback(ByVal Sender As System.Object, ByVal e As OLEGiveFeedbackEventArgs)
	'Public Event OLESetData(ByVal Sender As System.Object, ByVal e As OLESetDataEventArgs)
	'Public Event OLEStartDrag(ByVal Sender As System.Object, ByVal e As OLEStartDragEventArgs)

	'Public Event RtnKeyDownVB6(ByVal Sender As System.Object, ByVal e As RtnKeyDownEventArgs)

	' 標準の KeyPressEventArgs に基づくオーバーライド
	'Protected Overrides Sub OnKeyPress(e As System.Windows.Forms.KeyPressEventArgs)
	'	' Raise カスタムイベント（必要な場合のみ）
	'	RaiseEvent KeyPressVB6(Me, New KeyPressEventArgs(AscW(e.KeyChar)))
	'
	'	MyBase.OnKeyPress(e) ' 標準の KeyPress イベントがこれで自動的に発生する
	'End Sub

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
	Private _Enabled As Boolean
	Private _Locked As Boolean
	Private _MaxLength As Integer
	Private _ForeColor As String
	Private _BackColor As String
	Private _Format As Integer
	Private _Font As Integer
	Private _FocusBackColor As String
	Private _CanNextSetFocus As Boolean
	Private _CanForwardSetFocus As Boolean
	Private _SelectText As Boolean
	Private _EditMode As Boolean
	Private _Alignment As String
	Private _Appearance As String
	Private _BorderStyle As String
	Private _OldValue As String

	Private initializing As Boolean = True

	Public Sub Undo()
		'    Dim lret As Integer

		'    lret = SendMessage(ExDateTextD.Hwnd, EM_CANUNDO, 0, 0)
		'    If lret <> 0 Then
		'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		ExDateTextD.Text = vUNDOBUF.ToString
		'    End If
		'    Call SendMessage(ExDateTextD.Hwnd, EM_UNDO, 0, ByVal 0&)
		'    Call EmptyUndoBuffer
	End Sub

	Public Sub EmptyUndoBuffer()
		'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		vUNDOBUF = ExDateTextD.Text
		'    Call SendMessage(ExDateTextD.Hwnd, EM_EMPTYUNDOBUFFER, 0, ByVal 0&)
	End Sub

	'UPGRADE_WARNING: イベント ExDateTextD.TextChanged は、フォームが初期化されたときに発生します。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"' をクリックしてください。
	Private Sub ExDateTextD_TextChanged(sender As Object, e As EventArgs) Handles ExDateTextD.TextChanged

		If Loaded = False Then Exit Sub

		Dim tb As TextBox = DirectCast(sender, TextBox)
		If tb.Text.Length >= tb.MaxLength Then
			Dim nextCtrl = Me.FindForm().GetNextControl(Me, True)
			If nextCtrl IsNot Nothing Then
				nextCtrl.Focus()
			End If
		End If
		RaiseEvent Change(Me, Nothing)
	End Sub

	Private Sub ExDateTextD_Click(sender As Object, e As EventArgs) Handles ExDateTextD.Click
		RaiseEvent Click(Me, Nothing)
	End Sub

	Private Sub ExDateTextD_DoubleClick(sender As Object, e As EventArgs) Handles ExDateTextD.DoubleClick
		RaiseEvent DblClick(Me, Nothing)
	End Sub

	Private Sub ExDateTextD_Enter(sender As Object, e As EventArgs) Handles ExDateTextD.Enter
		ExDateTextD.BackColor = pFocusBackColor

		Try
			If Me.Hwnd <> MyParentName Then
				'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				vUNDOBUF = ExDateTextD.Text
			End If
			If Err.Number <> 0 Then
				'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				vUNDOBUF = ExDateTextD.Text
			End If
		Catch ex As Exception
			Err.Clear()
		End Try

		If pSelectText Then
			If fClicking = False Then
				ExDateTextD.SelectionStart = 0
				ExDateTextD.SelectionLength = Len(ExDateTextD.Text)
			End If
		End If
	End Sub

	Private Sub ExDateTextD_Leave(sender As Object, e As EventArgs) Handles ExDateTextD.Leave
		ExDateTextD.BackColor = hBackColor
		'On Error Resume Next
		'UPGRADE_WARNING: Control プロパティ UserControl.Parent は、新しい動作をもつ UserControl.FindForm にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="DFCDE711-9694-47D7-9C50-45A99CD8E91E"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト UserControl.Parent.ActiveControl の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_ISSUE: Control Hwnd は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		'MyParentName = MyBase.FindForm.ActiveControl.Hwnd
		'Err.Clear()
		'On Error GoTo 0
		Try
			Dim parentControl = Me.FindForm
			Dim activeControl = parentControl.ActiveControl
			Dim hwnd As IntPtr = If(activeControl IsNot Nothing, activeControl.Handle, IntPtr.Zero)
			MyParentName = hwnd
			Err.Clear()
		Catch ex As Exception
			' 必要に応じてエラーハンドリングを実装
		End Try
	End Sub

	Private Sub ExDateTextD_KeyDown(sender As Object, e As KeyEventArgs) Handles ExDateTextD.KeyDown
		Dim KeyCode As Integer = e.KeyCode
		Dim Shift As Integer = e.KeyData \ &H10000
		Dim Rtn_Cancel As Boolean
		''    If Shift = 0 Then
		''        Select Case KeyCode
		''            Case vbKeyReturn
		''                RaiseEvent RtnKeyDown
		''            Case vbKeySpace
		''                RaiseEvent SpcKeyDown
		''            Case vbKeyDelete, vbKeyBack
		''                If Len(ExDateTextD.Text) <> 0 Then
		''                    If Len(ExDateTextD.Text) <> Len(ExDateTextD.SelText) Then
		''                        ExDateTextD.SelStart = Len(ExDateTextD.Text) - 1
		''                        ExDateTextD.SelLength = Len(ExDateTextD.Text)
		''                    End If
		''                End If
		''        End Select
		''    End If
		If Shift = 0 Then
			Select Case KeyCode
				Case System.Windows.Forms.Keys.Return
					RaiseEvent KeyDown(Me, e)
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
			If pCanNextSetFocus = True And CtlCursorCondition(ExDateTextD) = -1 Then
				e.SuppressKeyPress = True
				Call NextSetFocus()
			Else
				e.Handled = True
			End If
		ElseIf KeyCode = System.Windows.Forms.Keys.Up Then
			If pCanForwardSetFocus = True And CtlCursorCondition(ExDateTextD) = -1 Then
				e.SuppressKeyPress = True
				Call ForwardSetFocus()
			Else
				e.Handled = True
			End If
		ElseIf KeyCode = System.Windows.Forms.Keys.Insert Then
			'入力モードにする
			e.SuppressKeyPress = True
			If CtlCursorCondition(ExDateTextD) = -1 Then
				ExDateTextD.SelectionStart = Len(ExDateTextD.Text)
			Else
				ExDateTextD.SelectionStart = 0
				ExDateTextD.SelectionLength = Len(ExDateTextD.Text)
			End If
		ElseIf KeyCode = System.Windows.Forms.Keys.Left Then
			If pEditMode = False Then
				If pCanForwardSetFocus = True And CtlCursorCondition(ExDateTextD) = -1 Then
					e.SuppressKeyPress = True
					Call ForwardSetFocus()
				End If
			End If
		ElseIf KeyCode = System.Windows.Forms.Keys.Right Then
			If pEditMode = False Then
				If pCanNextSetFocus = True And CtlCursorCondition(ExDateTextD) = -1 Then
					e.SuppressKeyPress = True
					Call NextSetFocus()
				End If
			End If
		End If
	End Sub

	Private Sub ExDateTextD_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles ExDateTextD.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		Const Numbers As String = "0123456789" ' 入力許可文字
		Dim strText As String
		Dim isValidKey As Boolean = True

		' Enterキーを無効にする
		If KeyAscii = System.Windows.Forms.Keys.Return Then KeyAscii = 0

		'数字検査をする
		If KeyAscii <> 8 Then ' バックスペースは例外
			If InStr(Numbers, Chr(KeyAscii)) = 0 Then
				KeyAscii = 0 ' 入力を無効にする
				isValidKey = False
			Else
				strText = InsStrToTextBox(ExDateTextD, Chr(KeyAscii))
				If InStr(strText, "0") = 1 Then
					KeyAscii = 0
				End If
				If Len(strText) <= 2 Then
					If CDbl(strText) < 0 Or CDbl(strText) > 31 Then
						KeyAscii = 0
					End If
				Else
					KeyAscii = 0
				End If
			End If
		End If

		' 有効なキーのみ RaiseEvent を通す
		If isValidKey Then
			'RaiseEvent KeyPressVB6(Me, New KeyPressEventArgs(KeyAscii))
			RaiseEvent KeyPress(Me, e)
		End If

		' キー入力の処理
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub ExDateTextD_KeyUp(sender As Object, e As KeyEventArgs) Handles ExDateTextD.KeyUp
		Dim KeyCode As Integer = e.KeyCode
		Dim Shift As Integer = e.KeyData \ &H10000
		'RaiseEvent KeyUp(Me, e)
	End Sub

	Private Sub ExDateTextD_MouseDown(sender As Object, e As MouseEventArgs) Handles ExDateTextD.MouseDown
		Dim Button As Integer = e.Button \ &H100000
		Dim Shift As Integer = System.Windows.Forms.Control.ModifierKeys \ &H10000
		'Dim X As Single = VB6Conv.PixelsToTwipsX(eventArgs.X)
		'Dim Y As Single = VB6Conv.PixelsToTwipsY(eventArgs.Y)
		Dim X As Single = e.X
		Dim Y As Single = e.Y
		'RaiseEvent MouseDown(Me, e)
		fClicking = True
	End Sub

	Private Sub ExDateTextD_MouseMove(sender As Object, e As MouseEventArgs) Handles ExDateTextD.MouseMove
		Dim Button As Integer = e.Button \ &H100000
		Dim Shift As Integer = System.Windows.Forms.Control.ModifierKeys \ &H10000
		'Dim X As Single = VB6Conv.PixelsToTwipsX(eventArgs.X)
		'Dim Y As Single = VB6Conv.PixelsToTwipsY(eventArgs.Y)
		Dim X As Single = e.X
		Dim Y As Single = e.Y
		'RaiseEvent MouseMove(Me, e)
	End Sub

	Private Sub ExDateTextD_MouseUp(sender As Object, e As MouseEventArgs) Handles ExDateTextD.MouseUp
		Dim Button As Integer = e.Button \ &H100000
		Dim Shift As Integer = System.Windows.Forms.Control.ModifierKeys \ &H10000
		'Dim X As Single = VB6Conv.PixelsToTwipsX(eventArgs.X)
		'Dim Y As Single = VB6Conv.PixelsToTwipsY(eventArgs.Y)
		Dim X As Single = e.X
		Dim Y As Single = e.Y
		'RaiseEvent MouseUp(Me, e)
		fClicking = False
	End Sub

	Private Sub ExDateTextBoxD_Enter(sender As Object, e As EventArgs) Handles MyBase.Enter
		fGotFocus = True
		Try

			If Me.Hwnd <> MyParentName Then
				'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				vUNDOBUF = ExDateTextD.Text
			End If
			If Err.Number <> 0 Then
				'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				vUNDOBUF = ExDateTextD.Text
			End If
		Catch ex As Exception
			Err.Clear()
		End Try
	End Sub

	Private Sub ExDateTextBoxD_Leave(sender As Object, e As EventArgs) Handles MyBase.Leave
		fGotFocus = False
		fClicking = False
	End Sub

	'UPGRADE_ISSUE: UserControl イベント UserControl.Terminate はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="ABD9AF39-7E24-4AFF-AD8D-3675C1AA3054"' をクリックしてください。
	Protected Overrides Sub OnHandleDestroyed(e As EventArgs)
		' コントロール破棄時の処理
		'終了時にIMEModeを0-なしにしないと
		'WinMEでimm32.dllがこける。
		ExDateTextD.ImeMode = System.Windows.Forms.ImeMode.NoControl
	End Sub

	Private Sub ExDateTextBoxD_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
		With ExDateTextD
			.Top = 0
			.Left = 0
			.Width = MyBase.Width
			'UPGRADE_ISSUE: UserControl プロパティ ExDateTextBoxD.Extender はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト Extender.Width の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'Me.Width = VB6Conv.PixelsToTwipsX(.Width) '2002/08/15 ADD
			Me.Width = .Width
			.Height = MyBase.Height
			'UPGRADE_ISSUE: UserControl プロパティ ExDateTextBoxD.Extender はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト Extender.Height の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'Me.Height = VB6Conv.PixelsToTwipsY(.Height) '2002/08/15 ADD
			Me.Height = .Height
			'''''        UserControl.Height = .Height   '2002/08/15 DEL
			'        .Height = Extender.Height
			'        .Width = Extender.Width
		End With
	End Sub

	'UPGRADE_ISSUE: UserControl イベント UserControl.InitProperties はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="ABD9AF39-7E24-4AFF-AD8D-3675C1AA3054"' をクリックしてください。
	Public Sub New()
		InitializeComponent()

		If Not Me.DesignMode Then
			'    Text = ""
			Me.FocusBackColor = System.Drawing.SystemColors.Window
			Me.ForeColor = System.Drawing.SystemColors.WindowText
			Me.BackColor = System.Drawing.SystemColors.Window
			Me.MaxLength = 2
			'UPGRADE_ISSUE: AmbientProperties プロパティ Ambient.Font はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
			'Me.Font = Ambient.Font
			'Me.Appearance = ExDateTextBoxY.AppearanceType.立体 '01/12/20
			Me.BorderStyle = ExDateTextBoxY.BorderStyleType.実線 '01/12/20
		End If

		pCanNextSetFocus = True
		pCanForwardSetFocus = True
		pSelectText = True
		pEditMode = True

	End Sub

	'UPGRADE_ISSUE: VBRUN.PropertyBag 型 はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6B85A2A7-FE9F-4FBE-AA0C-CF11AC86A305"' をクリックしてください。
	'UPGRADE_WARNING: UserControl イベント ReadProperties はサポートされていません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="92F3B58C-F772-4151-BE90-09F4A232AEAD"' をクリックしてください。
	'Protected Overrides Sub OnHandleCreated(ByVal e As EventArgs)
	'    MyBase.OnHandleCreated(e)
    '
	'	'UPGRADE_ISSUE: AmbientProperties プロパティ Ambient.DisplayName はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
	'	'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
	'	'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
	'	_DisplayName = My.Settings.D_DisplayName
	'	ExDateTextD.Text = _DisplayName ' Ambient.DisplayName
    '
	'	Me.Enabled = My.Settings.D_Enabled
	'	Me.Locked = My.Settings.D_Locked
	'	Me.MaxLength = My.Settings.D_MaxLength
	'	Me.ForeColor = My.Settings.D_ForeColor
	'	Me.BackColor = My.Settings.D_BackColor
	'	Me.Font = My.Settings.D_Font
    '
	'	pFocusBackColor = My.Settings.D_FocusBackColor
	'	pCanNextSetFocus = My.Settings.D_CanNextSetFocus
	'	pCanForwardSetFocus = My.Settings.D_CanForwardSetFocus
	'	pSelectText = My.Settings.D_SelectText
	'	pEditMode = My.Settings.D_EditMode
    '
	'	'ExDateTextD.Alignment = My.Settings.D_Alignment
	'	'ExDateTextD.Appearance = My.Settings.D_Appearance
	'	ExDateTextD.BorderStyle = My.Settings.D_BorderStyle
    '
	'	vUNDOBUF = My.Settings.D_OldValue
	'End Sub

	'UPGRADE_ISSUE: VBRUN.PropertyBag 型 はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6B85A2A7-FE9F-4FBE-AA0C-CF11AC86A305"' をクリックしてください。
	'UPGRADE_WARNING: UserControl イベント WriteProperties はサポートされていません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="92F3B58C-F772-4151-BE90-09F4A232AEAD"' をクリックしてください。

	' ISupportInitialize の実装
	'Public Sub BeginInit() Implements ISupportInitialize.BeginInit
	'    initializing = True
	'End Sub
    '
	'Public Sub EndInit() Implements ISupportInitialize.EndInit
	'    initializing = False
	'    SaveProperties()
	'End Sub
    '
	'' プロパティを保存する処理
	'Private Sub SaveProperties()
    '
	'	My.Settings.D_DisplayName = _DisplayName
	'	My.Settings.D_Enabled = Me.Enabled
	'	My.Settings.D_Locked = Me.Locked
	'	My.Settings.D_MaxLength = Me.MaxLength
	'	My.Settings.D_ForeColor = Me.ForeColor
	'	My.Settings.D_BackColor = Me.BackColor
	'	My.Settings.D_Font = Me.Font
    '
	'	My.Settings.D_FocusBackColor = pFocusBackColor
	'	My.Settings.D_CanNextSetFocus = pCanNextSetFocus
	'	My.Settings.D_CanForwardSetFocus = pCanForwardSetFocus
	'	My.Settings.D_SelectText = pSelectText
	'	My.Settings.D_EditMode = pEditMode
    '
	'	'My.Settings.D_Alignment = ExDateTextD.Alignment
	'	'My.Settings.D_Appearance = ExDateTextD.Appearance
	'	My.Settings.D_BorderStyle = ExDateTextD.BorderStyle
    '
	'	My.Settings.D_OldValue = vUNDOBUF
    '
	'	My.Settings.Save()
	'End Sub


	' ''Public Property Get CanUndo() As Boolean
	' ''    Dim fUndo As Boolean
	' ''
	' ''    fUndo = SendMessage(ExDateTextD.Hwnd, EM_CANUNDO, 0, ByVal 0&)
	' ''
	' ''    If fUndo Then CanUndo = True Else CanUndo = False
	' ''
	' ''End Property

	<Browsable(True)> <Description("コントロールに含まれる文字を設定します。")>
	<DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
	Public Overrides Property Text() As String
		Get
			Text = ExDateTextD.Text
		End Get
		Set(ByVal Value As String)
			If ExDateTextD IsNot Nothing Then
				ExDateTextD.Text = Value
			End If
			Try  '01/09/05
				If Me.Hwnd <> MyParentName Then
					'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					vUNDOBUF = ExDateTextD.Text
				End If
			Catch ex As Exception
				Try
					'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					vUNDOBUF = ExDateTextD.Text
				Catch innerEx As Exception
				End Try
			End Try
			RaiseEvent TextChange()
		End Set
	End Property

	<Browsable(True)> <Description("コントロール内のテキストの配置を設定します。")>
	Public Property Alignment() As HorizontalAlignment
		Get
			Alignment = ExDateTextD.TextAlign
		End Get
		Set(ByVal Value As HorizontalAlignment)
			Select Case Value
				Case 0
					ExDateTextD.TextAlign = HorizontalAlignment.Left
				Case 1
					ExDateTextD.TextAlign = HorizontalAlignment.Right
				Case 2
					ExDateTextD.TextAlign = HorizontalAlignment.Center
				Case Else
					ExDateTextD.TextAlign = HorizontalAlignment.Left
			End Select
			RaiseEvent AlignmentChange()
		End Set
	End Property

	'<Description("オブジェクトが、実行時に立体的に表示されるかどうかを設定します。")>
	'Public Property Appearance() As ExDateTextBoxY.AppearanceType
	'	Get
	'		'UPGRADE_ISSUE: TextBox プロパティ ExDateTextD.Appearance はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
	'		Appearance = ExDateTextD.Appearance
	'	End Get
	'	Set(ByVal Value As ExDateTextBoxY.AppearanceType)
	'		'UPGRADE_ISSUE: TextBox プロパティ ExDateTextD.Appearance はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
	'		ExDateTextD.Appearance = Value
	'		RaiseEvent AppearanceChange()
	'	End Set
	'End Property

	<Browsable(True)> <Description("オブジェクトの境界線のスタイルを設定します。")>
	Public Shadows Property BorderStyle() As ExDateTextBoxY.BorderStyleType
		Get
			BorderStyle = ExDateTextD.BorderStyle
		End Get
		Set(ByVal Value As ExDateTextBoxY.BorderStyleType)
			ExDateTextD.BorderStyle = Value
			RaiseEvent BorderStyleChange()
		End Set
	End Property

	Public Shadows Property Enabled() As Boolean
		Get
			' ''    Enabled = ExDateTextD.Enabled
			Enabled = MyBase.Enabled
		End Get
		Set(ByVal Value As Boolean)
			' ''    ExDateTextD.Enabled = New_Enabled
			MyBase.Enabled = Value
			RaiseEvent EnabledChange()
		End Set
	End Property

	Public Property Locked() As Boolean
		Get
			Locked = ExDateTextD.ReadOnly
		End Get
		Set(ByVal Value As Boolean)
			'    SendMessage m_hWnd, EM_SETREADONLY, NewProp, ByVal 0&
			ExDateTextD.ReadOnly = Value
			RaiseEvent LockedChange()
		End Set
	End Property

	<Browsable(True)> <Description("文字の最大数を設定します。")>
	Public Property MaxLength() As Integer
		Get
			'UPGRADE_WARNING: TextBox プロパティ ExDateTextD.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
			MaxLength = ExDateTextD.Maxlength
		End Get
		Set(ByVal Value As Integer)
			'UPGRADE_WARNING: TextBox プロパティ ExDateTextD.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
			ExDateTextD.Maxlength = Value
			RaiseEvent MaxLengthChange()
		End Set
	End Property

	<Browsable(True)>
	Public Property SelStart() As Integer
		Get
			SelStart = ExDateTextD.SelectionStart
		End Get
		Set(ByVal Value As Integer)
			If LicenseManager.UsageMode = LicenseUsageMode.Designtime Then Err.Raise(382)
			ExDateTextD.SelectionStart = Value
			RaiseEvent SelStartChange()
		End Set
	End Property

	<Browsable(True)>
	Public Property SelLength() As Integer
		Get
			SelLength = ExDateTextD.SelectionLength
		End Get
		Set(ByVal Value As Integer)
			If LicenseManager.UsageMode = LicenseUsageMode.Designtime Then Err.Raise(382)
			ExDateTextD.SelectionLength = Value
			RaiseEvent SelLengthChange()
		End Set
	End Property

	<Browsable(True)>
	Public Property SelText() As String
		Get
			SelText = ExDateTextD.SelectedText
		End Get
		Set(ByVal Value As String)
			If LicenseManager.UsageMode = LicenseUsageMode.Designtime Then Err.Raise(382)
			ExDateTextD.SelectedText = Value
			RaiseEvent SelTextChange()
		End Set
	End Property

	<Browsable(True)> <Description("オブジェクト内の文字色を設定します。")>
	Public Overrides Property ForeColor() As System.Drawing.Color
		Get
			ForeColor = ExDateTextD.ForeColor
		End Get
		Set(ByVal Value As System.Drawing.Color)
			ExDateTextD.ForeColor = Value
			RaiseEvent ForeColorChange()
		End Set
	End Property

	<Browsable(True)> <Description("オブジェクト内の文字で使用する背景色を設定します。")>
	Public Overrides Property BackColor() As System.Drawing.Color
		Get
			BackColor = ExDateTextD.BackColor
		End Get
		Set(ByVal Value As System.Drawing.Color)
			ExDateTextD.BackColor = Value
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

	Public Overrides Property Font() As System.Drawing.Font
		Get
			Return MyBase.Font
		End Get
		Set(ByVal Value As System.Drawing.Font)
			ExDateTextD.Font = Value
			Call ExDateTextBoxD_Resize(Me, New System.EventArgs())
			RaiseEvent FontChange()
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
			Hwnd = ExDateTextD.Handle
		End Get
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

	'テキストボックスの現在の位置に文字列を埋め込む
	Private Function InsStrToTextBox(ByRef TxText As System.Windows.Forms.TextBox, ByVal sInsStr As String) As String
		With TxText
			'        If sInsStr = "-" Then
			'            InsStrToTextBox = sInsStr & Left$(.Text, .SelStart) & Right$(.Text, Len(.Text) - .SelStart - .SelLength)
			'        Else
			'InsStrToTextBox = VB6Conv.Left(.Text, .SelectionStart) & sInsStr & VB6Conv.Right(.Text, Len(.Text) - .SelectionStart - .SelectionLength)
			Return .Text.Substring(0, .SelectionStart) & sInsStr &
			   .Text.Substring(.SelectionStart + .SelectionLength)
			'        End If
		End With
	End Function

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
				Call PostMessage(ExDateTextD.Handle, WM_KEYDOWN, System.Windows.Forms.Keys.Tab, 0)
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

	Private Sub ExDateTextBoxD_Load(sender As Object, e As EventArgs) Handles Me.Load
		Loaded = True
	End Sub
End Class