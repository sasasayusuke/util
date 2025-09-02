Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Runtime.InteropServices

<ToolboxItem(True)>
Public Class ExNmTextBox
	Inherits System.Windows.Forms.UserControl
'	Implements ISupportInitialize

	Public Event FormatTypeChange()
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
	Public Event DecimalPlaceChange()
	Public Event InputPlusChange()
	Public Event InputMinusChange()
	Public Event InputZeroChange()

	'UPDATE----------------------------------------------------------------------------------------
	'       2002/08/23      UNDO時FORMAT編集する。
	'
	'----------------------------------------------------------------------------------------------

	'UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
	'Private Declare Function SendMessage Lib "user32"  Alias "SendMessageA"(ByVal Hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer, ByRef lParam As Any) As Integer
	<DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
	Private Shared Function SendMessage(ByVal hwnd As IntPtr, ByVal msg As UInteger, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
	End Function

	''Private Const EM_LIMITTEXT = &HC5
	''Private Const EM_CANUNDO = &HC6
	''Private Const EM_UNDO = &HC7
	''Private Const EM_EMPTYUNDOBUFFER = &HCD
	''Private Const EM_SETREADONLY = &HCF
	Private Const WM_KEYDOWN As Integer = &H100

	'' ウィンドウへメッセージをポストするAPI
	'UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
	'Private Declare Function PostMessage Lib "user32" Alias "PostMessageA"(ByVal Hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer, ByRef lParam As Any) As Integer
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
	Private hMaxLength As Integer 'カンマ編集、小数点などを考慮したMaxLength

	Private pInputZero As Boolean
	Private pInputPlus As Boolean
	Private pInputMinus As Boolean
	Private pDecimalPlace As Integer
	Private pFormatType As String
	Private pFormatTypeNega As String
	Private pFormatTypeZero As String
	Private pFormatTypeNull As String
	Private pCanNextSetFocus As Boolean
	Private pCanForwardSetFocus As Boolean
	Private pSelectText As Boolean
	Private pEditMode As Boolean

	'変数
	Private fGotFocus As Boolean
	Private fClicking As Boolean
	Private vUNDOBUF As String
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
	''Public Event GotFocus()
	''Public Event LostFocus()
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
	Private _Enabled As Boolean
	Private _Locked As Boolean
	Private _MaxLength As Integer
	Private _ForeColor As String
	Private _BackColor As String
	Private _Font As Integer
	Private _FocusBackColor As String
	Private _InputZero As Boolean
	Private _InputPlus As Boolean
	Private _InputMinus As Boolean
	Private _DecimalPlace As Boolean
	Private _Format As String
	Private _FormatNega As String
	Private _FormatZero As String
	Private _FormatNull As String
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

		'    lret = SendMessage(ExNmText.Hwnd, EM_CANUNDO, 0, 0)
		'    If lret <> 0 Then
		' ''        ExNmText.Text = vUNDOBUF                             '2002/08/23 DEL
		'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		ExNmText.Text = ChgFormat(CStr(vUNDOBUF), pFormatType) '2002/08/23 ADD
		'    End If
		'    Call SendMessage(ExNmText.Hwnd, EM_UNDO, 0, ByVal 0&)
		'    Call EmptyUndoBuffer
	End Sub

	Public Sub EmptyUndoBuffer()
		'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		vUNDOBUF = ExNmText.Text
		'    Call SendMessage(ExNmText.Hwnd, EM_EMPTYUNDOBUFFER, 0, ByVal 0&)
	End Sub

	'UPGRADE_WARNING: イベント ExNmText.TextChanged は、フォームが初期化されたときに発生します。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"' をクリックしてください。
	Private Sub ExNmText_TextChanged(sender As Object, e As EventArgs) Handles ExNmText.TextChanged
		'    ExNmText.Text = ChgFormat(ExNmText.Text, pFormatType)
		RaiseEvent Change(Me, Nothing)
	End Sub

	Private Sub ExNmText_Click(sender As Object, e As EventArgs) Handles ExNmText.Click
		RaiseEvent Click(Me, Nothing)
	End Sub

	Private Sub ExNmText_DoubleClick(sender As Object, e As EventArgs) Handles ExNmText.DoubleClick
		RaiseEvent DblClick(Me, Nothing)
	End Sub

	Private Sub ExNmText_Enter(sender As Object, e As EventArgs) Handles ExNmText.Enter
		'    hBackColor = ExNmText.BackColor
		ExNmText.BackColor = pFocusBackColor

		Try
			If Me.Hwnd <> MyParentName Then
				'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				vUNDOBUF = ExNmText.Text
			End If
			If Err.Number <> 0 Then
				'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				vUNDOBUF = ExNmText.Text
			End If
		Catch ex As Exception
			Err.Clear()
		End Try

		If pInputZero Then
			If ExNmText.Text <> vbNullString Then
				If CDbl(ExNmText.Text) <> 0 Then
					ExNmText.Text = Format(CDbl(ExNmText.Text), "0" & If(pDecimalPlace <> 0, "." & New String("0"c, pDecimalPlace), ""))
				Else
					ExNmText.Text = "0"
				End If
			End If
		Else
			If ExNmText.Text <> vbNullString Then
				If CDbl(ExNmText.Text) <> 0 Then
					ExNmText.Text = Format(CDbl(ExNmText.Text), "0" & If(pDecimalPlace <> 0, "." & New String("0"c, pDecimalPlace), ""))
				Else
					ExNmText.Text = vbNullString
				End If
			End If
		End If

		If pSelectText Then
			If fClicking = False Then
				ExNmText.SelectionStart = 0
				ExNmText.SelectionLength = Len(ExNmText.Text)
			End If
		Else
			If fClicking = False Then '2002/12/10 ADD
				ExNmText.SelectionStart = Len(Me.Text)
			End If '--------------
		End If
		'UPGRADE_WARNING: TextBox プロパティ ExNmText.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
		ExNmText.MaxLength = hMaxLength
		''
		''    RaiseEvent GotFocus
	End Sub

	Private Sub ExNmText_Leave(sender As Object, e As EventArgs) Handles ExNmText.Leave
		'UPGRADE_WARNING: TextBox プロパティ ExNmText.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
		ExNmText.MaxLength = 0

		''    If Not IsNumeric(ExNmText.Text) Then  'ChgFormatに埋め込み
		''        ExNmText.Text = ""
		''    End If
		If pInputZero = True Then
			If ExNmText.Text = "" Then
				ExNmText.Text = "0"
			End If
		Else
			If ExNmText.Text <> "" Then
				If CDbl(ExNmText.Text) = 0 Then
					ExNmText.Text = vbNullString
				End If
			End If
		End If
		ExNmText.Text = ChgFormat(ExNmText.Text, pFormatType)
		ExNmText.BackColor = hBackColor

		'On Error Resume Next
		'UPGRADE_WARNING: Control プロパティ UserControl.Parent は、新しい動作をもつ UserControl.FindForm にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="DFCDE711-9694-47D7-9C50-45A99CD8E91E"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト UserControl.Parent.ActiveControl の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_ISSUE: Control Hwnd は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		'MyParentName = UserControl.FindForm.ActiveControl.Hwnd
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

	Private Sub ExNmText_KeyDown(sender As Object, e As KeyEventArgs) Handles ExNmText.KeyDown
		Dim KeyCode As Integer = e.KeyCode
		Dim Shift As Integer = e.KeyData \ &H10000
		Dim Rtn_Cancel As Boolean

		If Shift = 0 Then
			Select Case KeyCode
				Case System.Windows.Forms.Keys.Return
					RaiseEvent KeyDown(Me, e)
					''            Case vbKeySpace
					''                RaiseEvent SpcKeyDown
				Case System.Windows.Forms.Keys.Delete, System.Windows.Forms.Keys.Back
					If Len(ExNmText.Text) <> 0 Then
						'                    If Len(ExNmText.SelText) = 0 Or InStr(ExNmText.SelText, ".") <> 0 Then
						'                    If Len(ExNmText.Text) <> Len(ExNmText.SelText) And InStr(ExNmText.SelText, ".") <> 0 Then
						If Len(ExNmText.Text) <> Len(ExNmText.SelectedText) Then
							ExNmText.SelectionStart = Len(ExNmText.Text) - 1
							ExNmText.SelectionLength = Len(ExNmText.Text)
						End If
					End If
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
			If pCanNextSetFocus = True And CtlCursorCondition(ExNmText) = -1 Then
				e.SuppressKeyPress = True
				Call NextSetFocus()
			Else
				e.Handled = True
			End If
		ElseIf KeyCode = System.Windows.Forms.Keys.Up Then
			If pCanForwardSetFocus = True And CtlCursorCondition(ExNmText) = -1 Then
				e.SuppressKeyPress = True
				Call ForwardSetFocus()
			Else
				e.Handled = True
			End If
		ElseIf KeyCode = System.Windows.Forms.Keys.Insert Then
			'入力モードにする
			e.SuppressKeyPress = True
			If CtlCursorCondition(ExNmText) = -1 Then
				ExNmText.SelectionStart = Len(ExNmText.Text)
			Else
				ExNmText.SelectionStart = 0
				ExNmText.SelectionLength = Len(ExNmText.Text)
			End If
		ElseIf KeyCode = System.Windows.Forms.Keys.Left Then
			If pEditMode = False Then
				If pCanForwardSetFocus = True And CtlCursorCondition(ExNmText) = -1 Then
					e.SuppressKeyPress = True
					Call ForwardSetFocus()
				End If
			End If
		ElseIf KeyCode = System.Windows.Forms.Keys.Right Then
			If pEditMode = False Then
				If pCanNextSetFocus = True And CtlCursorCondition(ExNmText) = -1 Then
					e.SuppressKeyPress = True
					Call NextSetFocus()
				End If
			End If
		End If
	End Sub

	Private Sub ExNmText_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles ExNmText.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		Const Numbers As String = "-0123456789." ' 入力許可文字
		Dim strText As String
		'Dim Spc_Cancel As Boolean
		Dim isValidKey As Boolean = True

		If KeyAscii = System.Windows.Forms.Keys.Return Then KeyAscii = 0

		'数字検査をする
		If KeyAscii <> 8 Then ' バックスペースは例外
			If KeyAscii = System.Windows.Forms.Keys.Space Then
				RaiseEvent KeyPress(Me, e)
				e.Handled = True
			Else
				If InStr(Numbers, Chr(KeyAscii)) = 0 Then
					e.Handled = True ' 入力を無効にする
					isValidKey = False
				Else
					If InStr(".", Chr(KeyAscii)) <> 0 And (pDecimalPlace = 0) Then
						e.Handled = True
						isValidKey = False
					Else
						If pInputMinus = False Then 'マイナス入力不可の場合
							If Chr(KeyAscii) = "-" Then
								e.Handled = True
							End If
						End If
						'ﾏｲﾅｽを入力された場合                  '2002/12/10  ADD
						If Chr(KeyAscii) = "-" Then
							e.Handled = True
							If InStr(ExNmText.Text, "-") = 0 Then
								If pDecimalPlace = 0 Then
									'UPGRADE_WARNING: TextBox プロパティ ExNmText.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
									If Len(ExNmText.Text) < ExNmText.MaxLength Then
										ExNmText.Text = "-" & ExNmText.Text
									End If
								Else
									If InStr(ExNmText.Text, ".") = 0 Then
										'UPGRADE_WARNING: TextBox プロパティ ExNmText.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
										If Len(ExNmText.Text) < (ExNmText.MaxLength - pDecimalPlace) - 1 Then
											ExNmText.Text = "-" & ExNmText.Text
										End If
									Else
										'UPGRADE_WARNING: TextBox プロパティ ExNmText.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
										If InStr(ExNmText.Text, ".") - 1 < (ExNmText.MaxLength - pDecimalPlace) - 1 Then
											ExNmText.Text = "-" & ExNmText.Text
										End If
									End If
								End If
							Else
								ExNmText.Text = Mid(ExNmText.Text, InStr(ExNmText.Text, "-") + 1, Len(ExNmText.Text) - InStr(ExNmText.Text, "-") + 1)
							End If
							Me.SelStart = Len(Me.Text)
						End If '-------------------------
					End If
				End If
				'マイナスの処理                             2002/1/30
				'''''''''        If Chr$(KeyAscii) = "-" Then
				'''''''''            If pInputMinus = False Then             'マイナス入力不可の場合
				'''''''''                KeyAscii = 0
				'''''''''            Else
				'''''''''                Select Case Len(Me)
				'''''''''                    Case Me.MaxLength
				'''''''''                        If InStr(Me, "-") <> 0 Then
				'''''''''                            ExNmText = ExNmText * -1
				'''''''''                            Me.SelStart = Len(Me)
				'''''''''                            KeyAscii = 0
				'''''''''                        Else
				'''''''''                            KeyAscii = 0
				'''''''''                        End If
				'''''''''                    Case Is < Me.MaxLength
				'''''''''                        If Me = vbNullString Then
				'''''''''                            ExNmText = "-0"
				'''''''''                            KeyAscii = 0
				'''''''''                        Else
				'''''''''                            If Me = "-0" Then
				'''''''''                                ExNmText = "0"
				'''''''''                                KeyAscii = 0
				'''''''''                            Else
				'''''''''                                If Me = 0 Then
				'''''''''                                    ExNmText = "-0"
				'''''''''                                    KeyAscii = 0
				'''''''''                                Else
				'''''''''                                    If InStr(ExNmText, "-") = 0 Then
				'''''''''                                        ExNmText = "-" & ExNmText
				'''''''''                                    Else
				'''''''''                                        ExNmText = Mid$(ExNmText, InStr(ExNmText, "-") + 1)
				'''''''''                                    End If
				''''''''''                                    Me = ExNmText * -1
				'''''''''                                    KeyAscii = 0
				'''''''''                                End If
				'''''''''                            End If
				'''''''''                        End If
				'''''''''                        Me.SelStart = Len(Me)
				'''''''''                End Select
				'''''''''            End If
				'''''''''        End If
				'''''''''''        strText = InsStrToTextBox(ExNmText, Chr$(KeyAscii))  2002/12/10
				If isValidKey Then
					strText = InsStrToTextBox(ExNmText, KeyAscii)
					' ''    '末尾に何かしらの数字を付加
					strText += IIf(pInputZero = True, "0", "1")
					'小数点、マイナスの場合ゼロを内部的につける
					'        If Not IsNumeric(strText) Then
					'            strText = strText & "0"
					'        End If

					'数字以外になりそうなキー入力は排除
					If IsNumericMatch(strText) = False Then
						e.Handled = True
						isValidKey = False
					End If

					If isValidKey Then
						Select Case Len(ExNmText.SelectedText)
							Case 0
								If pDecimalPlace = 0 Then
									'UPGRADE_WARNING: TextBox プロパティ ExNmText.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
									If Len(strText) > ExNmText.MaxLength + 1 Then
										e.Handled = True
									End If
								Else
									If InStr(strText, ".") = 0 Then
										'UPGRADE_WARNING: TextBox プロパティ ExNmText.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
										If Len(strText) >= (ExNmText.MaxLength - pDecimalPlace) + 1 Then
											e.Handled = True
										End If
									Else
										If ExNmText.SelectionStart < InStr(strText, ".") Then '小数点より左側（整数部分）
											'UPGRADE_WARNING: TextBox プロパティ ExNmText.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
											If InStr(strText, ".") > (ExNmText.MaxLength - pDecimalPlace) Then
												e.Handled = True
											End If
										Else
											If (Len(strText) - InStr(strText, ".")) > pDecimalPlace + 1 Then
												e.Handled = True
											Else
												If Chr(KeyAscii) = "-" Then
													e.Handled = True
												End If
											End If
										End If
									End If
								End If
							Case Is < Len(ExNmText.Text)
								If InStr(ExNmText.SelectedText, ".") <> 0 Then
									e.Handled = True
								End If
						End Select

					End If
				End If
			End If

			''''        If Len(ExNmText.SelText) <> Len(ExNmText.Text) Then     '全選択されていない場合
			'''''        If Len(ExNmText.SelText) = 0 Or InStr(ExNmText.SelText, ".") <> 0 Then
			''''''            If InStr(strText, ".") = 0 Then
			''''            If pDecimalPlace = 0 Then
			''''                If Len(strText) > (ExNmText.MaxLength - pDecimalPlace) Then
			''''                    KeyAscii = 0
			''''                End If
			''''            Else
			''''                If InStr(strText, ".") = 0 Then
			''''                    If Len(strText) >= (ExNmText.MaxLength - pDecimalPlace) Then
			''''                        KeyAscii = 0
			''''                    End If
			''''                Else
			''''                    If SelStart < InStr(strText, ".") Then   '小数点より左側（整数部分）
			''''                        If InStr(ExNmText.SelText, ".") <> 0 Then
			''''                            If InStr(strText, ".") > (ExNmText.MaxLength - pDecimalPlace) Then
			''''                                KeyAscii = 0
			''''                            End If
			''''                        End If
			''''                    Else
			''''                        If (Len(strText) - InStr(strText, ".")) > pDecimalPlace Then
			''''                            KeyAscii = 0
			''''                        End If
			''''                    End If
			''''                End If
			''''            End If
			''''        Else
			''''''            If InStr(ExNmText.SelText, ".") <> 0 Then
			''''''                KeyAscii = 0
			''''''            End If
			''''        End If
			'        ExNmText.Text = ChgFormat(strText, pFormatType)
			'        ExNmText.SelStart = Len(ExNmText.Text)
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

	Private Sub ExNmText_KeyUp(sender As Object, e As KeyEventArgs) Handles ExNmText.KeyUp
		Dim KeyCode As Integer = e.KeyCode
		Dim Shift As Integer = e.KeyData \ &H10000
		'RaiseEvent KeyUp(Me, e)
	End Sub

	Private Sub ExNmText_MouseDown(sender As Object, e As MouseEventArgs) Handles ExNmText.MouseDown
		Dim Button As Integer = e.Button \ &H100000
		Dim Shift As Integer = System.Windows.Forms.Control.ModifierKeys \ &H10000
		'Dim X As Single = VB6Conv.PixelsToTwipsX(eventArgs.X)
		'Dim Y As Single = VB6Conv.PixelsToTwipsY(eventArgs.Y)
		Dim X As Single = e.X
		Dim Y As Single = e.Y
		'RaiseEvent MouseDown(Me, e)
		fClicking = True
	End Sub

	Private Sub ExNmText_MouseMove(sender As Object, e As MouseEventArgs) Handles ExNmText.MouseMove
		Dim Button As Integer = e.Button \ &H100000
		Dim Shift As Integer = System.Windows.Forms.Control.ModifierKeys \ &H10000
		'Dim X As Single = VB6Conv.PixelsToTwipsX(eventArgs.X)
		'Dim Y As Single = VB6Conv.PixelsToTwipsY(eventArgs.Y)
		Dim X As Single = e.X
		Dim Y As Single = e.Y
		'RaiseEvent MouseMove(Me, e)
	End Sub

	Private Sub ExNmText_MouseUp(sender As Object, e As MouseEventArgs) Handles ExNmText.MouseUp
		Dim Button As Integer = e.Button \ &H100000
		Dim Shift As Integer = System.Windows.Forms.Control.ModifierKeys \ &H10000
		'Dim X As Single = VB6Conv.PixelsToTwipsX(eventArgs.X)
		'Dim Y As Single = VB6Conv.PixelsToTwipsY(eventArgs.Y)
		Dim X As Single = e.X
		Dim Y As Single = e.Y
		'RaiseEvent MouseUp(Me, e)
		fClicking = False
	End Sub

	Private Sub ExNmTextBox_Enter(sender As Object, e As EventArgs) Handles MyBase.Enter
		fGotFocus = True
		Try
			If Me.Hwnd <> MyParentName Then
				'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				vUNDOBUF = ExNmText.Text
			End If
			If Err.Number <> 0 Then
				'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				vUNDOBUF = ExNmText.Text
			End If
		Catch ex As Exception
			Err.Clear()
		End Try
	End Sub

	Private Sub ExNmTextBox_Leave(sender As Object, e As EventArgs) Handles MyBase.Leave
		fGotFocus = False
		fClicking = False
	End Sub

	'UPGRADE_ISSUE: UserControl イベント UserControl.Terminate はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="ABD9AF39-7E24-4AFF-AD8D-3675C1AA3054"' をクリックしてください。
	Protected Overrides Sub OnHandleDestroyed(e As EventArgs)
		' コントロール破棄時の処理
		'終了時にIMEModeを0-なしにしないと
		'WinMEでimm32.dllがこける。
		ExNmText.ImeMode = System.Windows.Forms.ImeMode.NoControl
	End Sub

	Private Sub ExNmTextBox_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
		With ExNmText
			.Top = 0
			.Left = 0
			.Width = MyBase.Width
			'UPGRADE_ISSUE: UserControl プロパティ ExNmTextBox.Extender はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト Extender.Width の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'Me.Width = VB6Conv.PixelsToTwipsX(.Width) '2002/08/15 ADD
			Me.Width = .Width
			.Height = MyBase.Height
			'UPGRADE_ISSUE: UserControl プロパティ ExNmTextBox.Extender はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト Extender.Height の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'Me.Height = VB6Conv.PixelsToTwipsY(.Height) '2002/08/15 ADD
			Me.Height = .Height
			'        UserControl.Height = .Height   '2002/08/15 DEL
			'        .Height = Extender.Height
			'        .Width = Extender.Width
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
			Me.MaxLength = 12
			'UPGRADE_ISSUE: AmbientProperties プロパティ Ambient.Font はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
			'Me.Font = Ambient.Font
			'Me.Appearance = AppearanceType.立体 '01/12/20
			Me.BorderStyle = BorderStyleType.実線 '01/12/20
		End If

		pInputZero = True
		pInputPlus = True
		pInputMinus = True
		pDecimalPlace = 0
		pFormatType = ""
		pFormatTypeNega = ""
		pFormatTypeZero = ""
		pFormatTypeNull = ""
		pCanNextSetFocus = True
		pCanForwardSetFocus = True
		pSelectText = True
		pEditMode = True

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
	'	ExNmText.Text = _DisplayName ' Ambient.DisplayName
	'
	'	Me.Enabled = My.Settings.Enabled
	'	Me.Locked = My.Settings.Locked
	'	Me.MaxLength = My.Settings.MaxLength
	'	Me.ForeColor = My.Settings.ForeColor
	'	Me.BackColor = My.Settings.BackColor
	'	Me.Font = My.Settings.Font
	'
	'	pFocusBackColor = My.Settings.FocusBackColor
	'	pInputZero = My.Settings.InputZero
	'	pInputPlus = My.Settings.InputPlus
	'	pInputMinus = My.Settings.InputMinus
	'	pDecimalPlace = My.Settings.DecimalPlace
	'	pFormatType = My.Settings.Format
	'	pFormatTypeNega = My.Settings.FormatNega
	'	pFormatTypeZero = My.Settings.FormatZero
	'	pFormatTypeNull = My.Settings.FormatNull
	'	pCanNextSetFocus = My.Settings.CanNextSetFocus
	'	pCanForwardSetFocus = My.Settings.CanForwardSetFocus
	'	pSelectText = My.Settings.SelectText
	'	pEditMode = My.Settings.EditMode
	'
	'	ExNmText.TextAlign = My.Settings.Alignment
	'	'ExNmText.Appearance = My.Settings.Appearance
	'	ExNmText.BorderStyle = My.Settings.BorderStyle
	'
	'	vUNDOBUF = My.Settings.OldValue
	'End Sub

	'UPGRADE_ISSUE: VBRUN.PropertyBag 型 はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6B85A2A7-FE9F-4FBE-AA0C-CF11AC86A305"' をクリックしてください。
	'UPGRADE_WARNING: UserControl イベント WriteProperties はサポートされていません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="92F3B58C-F772-4151-BE90-09F4A232AEAD"' をクリックしてください。

	' ISupportInitialize の初期化の開始
	'Public Sub BeginInit() Implements ISupportInitialize.BeginInit
	'	initializing = True
	'End Sub
	'
	'' ISupportInitialize の初期化の終了
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
	'	My.Settings.Font = Me.Font
	'
	'	My.Settings.FocusBackColor = pFocusBackColor
	'	My.Settings.InputZero = pInputZero
	'	My.Settings.InputMinus = pInputPlus
	'	My.Settings.DecimalPlace = pDecimalPlace
	'	My.Settings.Format = pFormatType
	'	My.Settings.FormatNega = pFormatTypeNega
	'	My.Settings.FormatZero = pFormatTypeZero
	'	My.Settings.FormatNull = pFormatTypeNull
	'	My.Settings.CanNextSetFocus = pCanNextSetFocus
	'	My.Settings.CanForwardSetFocus = pCanForwardSetFocus
	'	My.Settings.SelectText = pSelectText
	'	My.Settings.EditMode = pEditMode
	'
	'	My.Settings.Alignment = ExNmText.TextAlign
	'	'My.Settings.Appearance = ExNmText.Appearance
	'	My.Settings.BorderStyle = ExNmText.BorderStyle
	'
	'	My.Settings.OldValue = vUNDOBUF
	'
	'	My.Settings.Save()
	'End Sub

	<Browsable(True)> <Description("コントロールに含まれる文字を設定します。")>
	<DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
	Public Overrides Property Text() As String
		Get
			''    Text = ExNmText.Text
			'UPGRADE_ISSUE: 定数 vbUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			'Text = StrConv(StrConv(ExNmText.Text(), vbFromUnicode), vbUnicode)
			Text = ExNmText.Text()
		End Get
		Set(ByVal Value As String)
			Dim ParentHwnd As IntPtr

			'UPGRADE_WARNING: TextBox プロパティ ExNmText.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
			ExNmText.MaxLength = 0 '01/08/28

			'On Error Resume Next '01/09/05
			Try
				If Me.Hwnd <> MyParentName Then
					'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					vUNDOBUF = Value
				End If
			Catch ex As Exception
				Try
					'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					vUNDOBUF = Value
				Catch innerEx As Exception
				End Try
			End Try

			Try
				'UPGRADE_WARNING: Control プロパティ UserControl.Parent は、新しい動作をもつ UserControl.FindForm にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="DFCDE711-9694-47D7-9C50-45A99CD8E91E"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト UserControl.Parent.ActiveControl の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_ISSUE: Control Hwnd は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
				Dim parentControl = Me.FindForm
				If parentControl IsNot Nothing Then
					ParentHwnd = If(parentControl.ActiveControl IsNot Nothing, parentControl.ActiveControl.Handle, IntPtr.Zero)
					Dim activeControl = parentControl.ActiveControl
					Dim hwnd As IntPtr = If(activeControl IsNot Nothing, activeControl.Handle, IntPtr.Zero)
					MyParentName = hwnd
				End If
			Catch ex As Exception
			End Try

			Try
				If Me.Hwnd = ParentHwnd Then
					If pInputZero Then
						ExNmText.Text = Format(ExNmText.Text, "0" & If(pDecimalPlace <> 0, "." & New String("0"c, pDecimalPlace), ""))
					Else
						If Value <> vbNullString Then
							If CDbl(Value) <> 0 Then
								ExNmText.Text = Format(CDbl(ExNmText.Text), "0" & If(pDecimalPlace <> 0, "." & New String("0"c, pDecimalPlace), ""))
							Else
								ExNmText.Text = vbNullString
							End If
						Else
							ExNmText.Text = ChgFormat(Value, pFormatType)
						End If
					End If
				Else
					ExNmText.Text = ChgFormat(Value, pFormatType)
				End If
			Catch ex As Exception
				ExNmText.Text = ChgFormat(Value, pFormatType)
			End Try

			'UPGRADE_WARNING: TextBox プロパティ ExNmText.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
			ExNmText.MaxLength = hMaxLength

			RaiseEvent TextChange()
		End Set
	End Property


	Public Shadows Property Enabled() As Boolean
		Get
			'    Enabled = ExNmText.Enabled
			Enabled = MyBase.Enabled
		End Get
		Set(ByVal Value As Boolean)
			'    ExNmText.Enabled = New_Enabled
			MyBase.Enabled = Value
			RaiseEvent EnabledChange()
		End Set
	End Property


	Public Property Locked() As Boolean
		Get
			Locked = ExNmText.ReadOnly
		End Get
		Set(ByVal Value As Boolean)
			'    SendMessage m_hWnd, EM_SETREADONLY, NewProp, ByVal 0&
			ExNmText.ReadOnly = Value
			RaiseEvent LockedChange()
		End Set
	End Property

	<Browsable(True)> <Description("コントロール内のテキストの配置を設定します。")>
	Public Property Alignment() As HorizontalAlignment
		Get
			Alignment = ExNmText.TextAlign
		End Get
		Set(ByVal Value As HorizontalAlignment)
			Select Case Value
				Case 0
					ExNmText.TextAlign = HorizontalAlignment.Left
				Case 1
					ExNmText.TextAlign = HorizontalAlignment.Right
				Case 2
					ExNmText.TextAlign = HorizontalAlignment.Center
				Case Else
					ExNmText.TextAlign = HorizontalAlignment.Left
			End Select
			RaiseEvent AlignmentChange()
		End Set
	End Property

	'<Description("オブジェクトが、実行時に立体的に表示されるかどうかを設定します。")>
	'Public Property Appearance() As AppearanceType
	'    Get
	'        'UPGRADE_ISSUE: TextBox プロパティ ExNmText.Appearance はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
	'        'Appearance = ExNmText.Appearance
	'    End Get
	'    Set(ByVal Value As AppearanceType)
	'        'UPGRADE_ISSUE: TextBox プロパティ ExNmText.Appearance はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
	'        'ExNmText.Appearance = Value
	'        RaiseEvent AppearanceChange()
	'    End Set
	'End Property

	<Browsable(True)> <Description("オブジェクトの境界線のスタイルを設定します。")>
	Public Shadows Property BorderStyle() As BorderStyleType
		Get
			BorderStyle = ExNmText.BorderStyle
		End Get
		Set(ByVal Value As BorderStyleType)
			ExNmText.BorderStyle = Value
			RaiseEvent BorderStyleChange()
		End Set
	End Property


	<Browsable(True)> <Description("文字の最大数を設定します。小数点を含む場合及び、マイナス入力可能の場合、それぞれ+1する事。")>
	Public Property MaxLength() As Integer
		Get
			'フォーカスを持っていない場合、ZEROなのでHOLDの方を返す。
			'フォーカスを持っている場合は、同じ値のはず！
			'UPGRADE_WARNING: TextBox プロパティ ExNmText.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
			If ExNmText.MaxLength = 0 Then '2002/01/21
				MaxLength = hMaxLength
			Else
				'UPGRADE_WARNING: TextBox プロパティ ExNmText.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				MaxLength = ExNmText.MaxLength
			End If
		End Get
		Set(ByVal Value As Integer)
			'UPGRADE_WARNING: TextBox プロパティ ExNmText.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
			ExNmText.MaxLength = Value
			hMaxLength = Value
			RaiseEvent MaxLengthChange()
		End Set
	End Property

	<Browsable(True)>
	Public Property SelStart() As Integer
		Get
			SelStart = ExNmText.SelectionStart
		End Get
		Set(ByVal Value As Integer)
			If LicenseManager.UsageMode = LicenseUsageMode.Designtime Then Err.Raise(382)
			ExNmText.SelectionStart = Value
			RaiseEvent SelStartChange()
		End Set
	End Property

	<Browsable(True)>
	Public Property SelLength() As Integer
		Get
			SelLength = ExNmText.SelectionLength
		End Get
		Set(ByVal Value As Integer)
			If LicenseManager.UsageMode = LicenseUsageMode.Designtime Then Err.Raise(382)
			ExNmText.SelectionLength = Value
			RaiseEvent SelLengthChange()
		End Set
	End Property

	<Browsable(True)>
	Public Property SelText() As String
		Get
			SelText = ExNmText.SelectedText
		End Get
		Set(ByVal Value As String)
			If LicenseManager.UsageMode = LicenseUsageMode.Designtime Then Err.Raise(382)
			ExNmText.SelectedText = Value
			RaiseEvent SelTextChange()
		End Set
	End Property

	<Browsable(True)> <Description("オブジェクト内の文字色を設定します。")>
	Public Overrides Property ForeColor() As System.Drawing.Color
		Get
			ForeColor = ExNmText.ForeColor
		End Get
		Set(ByVal Value As System.Drawing.Color)
			ExNmText.ForeColor = Value
			RaiseEvent ForeColorChange()
		End Set
	End Property

	<Browsable(True)> <Description("オブジェクト内の文字で使用する背景色を設定します。")>
	Public Overrides Property BackColor() As System.Drawing.Color
		Get
			BackColor = ExNmText.BackColor
		End Get
		Set(ByVal Value As System.Drawing.Color)
			ExNmText.BackColor = Value
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


	'Public Shadows Property IMEMode() As IMEModeType
	'	Get
	'		IMEMode = ExNmText.ImeMode
	'	End Get
	'	Set(ByVal Value As IMEModeType)
	'		ExNmText.ImeMode = Value
	'		RaiseEvent IMEModeChange()
	'	End Set
	'End Property


	Public Overrides Property Font() As System.Drawing.Font
		Get
			Return MyBase.Font
		End Get
		Set(ByVal Value As System.Drawing.Font)
			ExNmText.Font = Value
			RaiseEvent FontChange()
		End Set
	End Property

	<Browsable(True)> <Description("ゼロ入力可能かどうかを設定します。")>
	Public Property InputZero() As Boolean
		Get
			InputZero = pInputZero
		End Get
		Set(ByVal Value As Boolean)
			pInputZero = Value
			RaiseEvent InputZeroChange()
		End Set
	End Property

	<Browsable(True)> <Description("プラス入力可能かどうかを設定します。")>
	Public Property InputPlus() As Boolean
		Get
			InputPlus = pInputPlus
		End Get
		Set(ByVal Value As Boolean)
			pInputPlus = Value
			RaiseEvent InputPlusChange()
		End Set
	End Property

	<Browsable(True)> <Description("マイナス入力可能かどうかを設定します。")>
	Public Property InputMinus() As Boolean
		Get
			InputMinus = pInputMinus
		End Get
		Set(ByVal Value As Boolean)
			pInputMinus = Value
			RaiseEvent InputMinusChange()
		End Set
	End Property


	<Browsable(True)> <Description("小数点の桁数を入力します。")>
	Public Property DecimalPlace() As Short
		Get
			DecimalPlace = pDecimalPlace
		End Get
		Set(ByVal Value As Short)
			If Value < 0 Then Err.Raise(380)
			pDecimalPlace = Value
			RaiseEvent DecimalPlaceChange()
		End Set
	End Property


	<Browsable(True)> <Description("書式を設定します。")>
	Public Property FormatType() As String
		Get
			FormatType = pFormatType
		End Get
		Set(ByVal Value As String)
			pFormatType = Value
			RaiseEvent FormatTypeChange()
		End Set
	End Property

	<Browsable(True)> <Description("書式を設定します。負数")>
	Public Property FormatTypeNega() As String
		Get
			FormatTypeNega = pFormatTypeNega
		End Get
		Set(ByVal Value As String)
			pFormatTypeNega = Value
			RaiseEvent FormatTypeChange()
		End Set
	End Property

	<Browsable(True)> <Description("書式を設定します。Zero")>
	Public Property FormatTypeZero() As String
		Get
			FormatTypeZero = pFormatTypeZero
		End Get
		Set(ByVal Value As String)
			pFormatTypeZero = Value
			RaiseEvent FormatTypeChange()
		End Set
	End Property

	<Browsable(True)> <Description("書式を設定します。Null")>
	Public Property FormatTypeNull() As String
		Get
			FormatTypeNull = pFormatTypeNull
		End Get
		Set(ByVal Value As String)
			pFormatTypeNull = Value
			RaiseEvent FormatTypeChange()
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
			Hwnd = ExNmText.Handle
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
	'''''''Private Function InsStrToTextBox(ByRef TxText As TextBox, ByVal sInsStr As String) As String
	Private Function InsStrToTextBox(ByRef TxText As System.Windows.Forms.TextBox, ByVal sInsStr As Integer) As String
		'UPGRADE_NOTE: str は str_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
		Dim str_Renamed As String
		If sInsStr = 0 Then
			InsStrToTextBox = TxText.Text
			Exit Function 'KeyAsciiが０なら何もしない
		End If

		str_Renamed = ChrW(sInsStr)
		With TxText
			' If sInsStr = "-" Then
			'  InsStrToTextBox = sInsStr & Left$(.Text, .SelStart) & Right$(.Text, Len(.Text) - .SelStart - .SelLength)
			' Else
			'  InsStrToTextBox = Left$(.Text, .SelStart) & sInsStr & Right$(.Text, Len(.Text) - .SelStart - .SelLength)
			'  InsStrToTextBox = VB6Conv.Left(.Text, .SelectionStart) & str_Renamed & VB6Conv.Right(.Text, Len(.Text) - .SelectionStart - .SelectionLength)
			Return .Text.Substring(0, .SelectionStart) & str_Renamed &
			   .Text.Substring(.SelectionStart + .SelectionLength)
			' End If
		End With
	End Function

	'文字の内容が各条件に合っているか？
	Private Function IsNumericMatch(ByVal Expression As Object) As Boolean
		'Dim lBuf As Integer
		Dim dBuf As Double

		Try
			'数字か？
			If IsNumeric(Expression) = False Then
				IsNumericMatch = False
				Exit Function
			End If
			'文字から数値へ変換
			'UPGRADE_WARNING: オブジェクト Expression の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			dBuf = CDbl(Expression)
			'負数の条件に適合しているか？
			If pInputMinus = False Then
				If dBuf < 0 Then
					IsNumericMatch = False
					Exit Function
				End If
			End If
			'ゼロの条件に適合しているか？
			If pInputZero = False Then
				If dBuf = 0 Then
					'UPGRADE_WARNING: オブジェクト Expression の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					If InStr(Expression, "-") = 0 Then
						'UPGRADE_WARNING: オブジェクト Expression の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						If InStr(Expression, ".") = 0 Then
							IsNumericMatch = False
							Exit Function
						End If
					End If
				End If
			End If
			'正数の条件に適合しているか？
			If pInputPlus = False Then
				If dBuf > 0 Then
					IsNumericMatch = False
					Exit Function
				End If
			End If
			' ''整数の条件に適合しているか？
			''    If bInteger = False Then
			''        lBuf = CLng(dBuf)
			''        If CDbl(lBuf) <> dBuf Then
			''            isNumericMatch = False
			''            Exit Function
			''        End If
			''    End If
			' '''''''    Dim DecPos As Integer
			' '''''''    Dim DecNum As Integer
			' '''''''
			' '''''''    DecPos = InStr(Expression, ".")
			' '''''''''    DecPos = InStr(dBuf, ".")
			' '''''''    If DecPos <> 0 Then
			' '''''''        DecNum = Len(Expression) - DecPos
			' '''''''''        DecNum = Len(CStr(dBuf)) - DecPos
			' '''''''        If pDecimalPlace < DecNum Then
			' '''''''            isNumericMatch = False
			' '''''''            Exit Function
			' '''''''        End If
			' '''''''    End If

			IsNumericMatch = True
			Exit Function
		Catch ex As Exception
			'エラー処理
			IsNumericMatch = False
		End Try
	End Function

	Private Function ChgFormat(ByRef tx_Text As String, ByRef FormatType As String) As String
		' ''    Dim aryFormat() As String
		' ''    Dim i As Long
		' ''
		' ''    Call StrSplit(FormatType, ";", aryFormat, 4)
		' ''
		' ''    If IsNull(tx_Text) Then
		' ''        i = 3
		' ''        tx_Text = "0"
		' ''    ElseIf tx_Text = "" Then
		' ''        i = 3
		' ''        tx_Text = "0"
		' ''    ElseIf CDbl(tx_Text) > 0 Then
		' ''        i = 0
		' ''    ElseIf CDbl(tx_Text) < 0 Then
		' ''        i = 1
		' ''    Else
		' ''        i = 2
		' ''    End If
		' ''    If aryFormat(i) <> "" Then
		If Not IsNumeric(tx_Text) Then
			tx_Text = ""
		End If

		If Len(tx_Text) > hMaxLength Then
			' ''        tx_Text = Left$(tx_Text, hMaxLength)     2001/12/14
			tx_Text = Convert.ToString(tx_Text)
		End If
		Select Case True
			Case tx_Text = ""
				If FormatTypeNull <> "" Then
					If tx_Text <> "" Then
						ChgFormat = Format(CDbl(tx_Text), FormatTypeNull)
					Else
						ChgFormat = tx_Text
					End If
				Else
					ChgFormat = tx_Text
				End If
			Case CDbl(tx_Text) = 0
				If FormatTypeZero <> "" Then
					If tx_Text <> "" Then
						ChgFormat = Format(CDbl(tx_Text), FormatTypeZero)
					Else
						ChgFormat = tx_Text
					End If
				Else
					ChgFormat = tx_Text
				End If
			Case CDbl(tx_Text) > 0
				If FormatType <> "" Then
					If tx_Text <> "" Then
						ChgFormat = Format(CDbl(tx_Text), FormatType)
					Else
						ChgFormat = tx_Text
					End If
				Else
					ChgFormat = tx_Text
				End If
			Case Else
				If FormatTypeNega <> "" Then
					If tx_Text <> "" Then
						ChgFormat = Format(CDbl(tx_Text), FormatTypeNega)
					Else
						ChgFormat = tx_Text
					End If
				Else
					ChgFormat = tx_Text
				End If
		End Select
		If Not String.IsNullOrEmpty(ChgFormat) Then
			If ChgFormat.Substring(ChgFormat.Length - 1) = "." Then '一番左が小数点ならば
				'ChgFormat = VB6Conv.Left(ChgFormat, Len(ChgFormat) - 1)
				ChgFormat = ChgFormat.Substring(0, ChgFormat.Length - 1)
			End If
		End If

	End Function

	Public Function StrSplit(ByRef strExpression As String, ByRef strDelimiter As String, ByRef strArray() As String, Optional ByRef MAX_ARRAY As Integer = 4, Optional ByVal lngCompare As Integer = 0) As Integer

		Dim p1 As Integer 'InStr関数用検索開始位置
		Dim p2 As Integer 'InStr関数用文字検出位置
		Dim lngStrLen As Integer '検索される文字列のサイズ
		Dim lngDivLen As Integer '区切り文字のサイズ
		Dim lngCnt As Integer '項目数(=配列要素数)をあらわすカウンタ
		Dim lngMaxArray As Integer '配列要素の最大数

		lngStrLen = Len(strExpression) '元の文字列 strExpression の文字数を取得
		lngDivLen = Len(strDelimiter) '区切り文字 strDelimiter の文字数を取得

		If lngStrLen = 0 Then '引数 strExpression に空の文字列を渡した場合
			StrSplit = 0 '0 を返す
			ReDim strArray(0)
			strArray(0) = ""
			Exit Function
		ElseIf lngDivLen = 0 Then  '引数 strDelimiter に空の文字列を渡した場合
			ReDim strArray(0)
			strArray(0) = strExpression '引数 strExpression を単一要素の配列として返す
			StrSplit = 1
			Exit Function
		End If

		lngMaxArray = MAX_ARRAY
		ReDim strArray(lngMaxArray) '配列最大要素数を初期値にセット
		p1 = 1 '初期検索開始点を設定

		Do  '区切り文字が検出されなくなるまでループ
			If lngCnt > lngMaxArray Then '項目数が配列要素最大数を超えてしまった場合
				lngMaxArray += lngMaxArray + MAX_ARRAY '配列要素最大数をMAX_ARRAY増やす
				ReDim Preserve strArray(lngMaxArray)
			End If
			p2 = InStr(p1, strExpression, strDelimiter, lngCompare) '区切り文字を検索しその位置を返す
			If p2 Then '区切り文字が存在した場合
				strArray(lngCnt) = Mid(strExpression, p1, p2 - p1) '検索開始点から 区切り文字位置 - 1 までの文字列をを配列に代入
				p1 = p2 + lngDivLen '次回の検索開始点を設定
				lngCnt += lngCnt + 1 '次回のため項目数を一つ増やす
			Else '区切り文字が存在しない場合
				If lngCnt Then '最後の要素である場合
					'strArray(lngCnt) = VB6Conv.Right(strExpression, lngStrLen - p1 + 1) '文字列の最後から検索開始点までの文字列を配列に代入
					strArray(lngCnt) = strExpression.Substring(p1, lngStrLen - p1 + 1) '文字列の最後から検索開始点までの文字列を配列に代入
				Else '区切り文字が全く存在しない場合
					strArray(lngCnt) = strExpression
					'                Erase strArray                                                     '配列を消去
					'                StrSplit = 0                                                       '0 を返す
					'                Exit Function
				End If
			End If
		Loop Until p2 = 0

		ReDim Preserve strArray(lngCnt) '配列の余分な要素を削る
		StrSplit = lngCnt + 1 '配列の個数を返す

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
				Call PostMessage(ExNmText.Handle, WM_KEYDOWN, System.Windows.Forms.Keys.Tab, 0)
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