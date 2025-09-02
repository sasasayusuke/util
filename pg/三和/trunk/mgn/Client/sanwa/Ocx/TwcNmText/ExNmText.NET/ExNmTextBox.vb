Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
<System.Runtime.InteropServices.ProgId("ExNmTextBox_NET.ExNmTextBox")> Public Class ExNmTextBox
	Inherits System.Windows.Forms.UserControl
	Public Event FormatTypeChange()
	Public Event EditModeChange()
	Public Event SelStartChange()
	Public Event CanForwardSetFocusChange()
	Public Event SelTextChange()
	Public Event ForeColorChange()
	Public Event BorderStyleChange()
	Public Event DecimalPlaceChange()
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
	Public Event InputPlusChange()
	Public Event SelLengthChange()
	Public Event FontChange()
	Public Event InputMinusChange()
	Public Event BackColorChange()
	Public Event InputZeroChange()
	'UPDATE----------------------------------------------------------------------------------------
	'       2002/08/23      UNDO時FORMAT編集する。
	'
	'----------------------------------------------------------------------------------------------
	
	'UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
	Private Declare Function SendMessage Lib "user32"  Alias "SendMessageA"(ByVal Hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer, ByRef lParam As Any) As Integer
	''Private Const EM_CANUNDO = &HC6
	''Private Const EM_UNDO = &HC7
	''Private Const EM_EMPTYUNDOBUFFER = &HCD
	''Private Const EM_SETREADONLY = &HCF
	Private Const WM_KEYDOWN As Integer = &H100
	''Private Const EM_LIMITTEXT = &HC5
	'' ウィンドウへメッセージをポストするAPI
	'UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
	Private Declare Function PostMessage Lib "user32"  Alias "PostMessageA"(ByVal Hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer, ByRef lParam As Any) As Integer
	''Private Const WM_KEYDOWN = &H100
	''Private Const WM_KEYUP = &H101
	''Private Const VK_TAB = &H9
	'キーを送るAPI
	Private Declare Sub keybd_event Lib "user32" (ByVal bVk As Byte, ByVal bScan As Byte, ByVal dwFlags As Integer, ByVal dwExtraInfo As Integer)
	Private Const KEYEVENTF_KEYUP As Integer = &H2
	
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
	Private hMaxLength As Integer 'カンマ編集、小数点などを考慮したMaxLength
	
	Private pInputZero As Boolean
	Private pInputPlus As Boolean
	Private pInputMinus As Boolean
	Private pDecimalPlace As Short
	Private pFormatType As String
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
	''Public Event GotFocus()
	''Public Event LostFocus()
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
	Public Event SpcKeyPress(ByVal Sender As System.Object, ByVal e As SpcKeyPressEventArgs)
	
	Public Sub Undo()
		'    Dim lret As Long
		
		'    lret = SendMessage(ExNmText.Hwnd, EM_CANUNDO, 0, 0)
		'    If lret <> 0 Then
		'''        ExNmText.Text = vUNDOBUF                             '2002/08/23 DEL
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
	Private Sub ExNmText_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ExNmText.TextChanged
		'    ExNmText.Text = ChgFormat(ExNmText.text, pFormatType)
		RaiseEvent Change(Me, Nothing)
	End Sub
	
	Private Sub ExNmText_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ExNmText.Click
		RaiseEvent Click(Me, Nothing)
	End Sub
	
	Private Sub ExNmText_DoubleClick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ExNmText.DoubleClick
		RaiseEvent DblClick(Me, Nothing)
	End Sub
	
	Private Sub ExNmText_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ExNmText.Enter
		'    hBackColor = ExNmText.BackColor
		ExNmText.BackColor = pFocusBackColor
		
		On Error Resume Next
		If Me.Hwnd <> MyParentName Then
			'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			vUNDOBUF = ExNmText.Text
		End If
		If Err.Number <> 0 Then
			'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			vUNDOBUF = ExNmText.Text
		End If
		Err.Clear()
		On Error GoTo 0
		
		If pInputZero Then
			ExNmText.Text = VB6.Format(ExNmText.Text, "0" & IIf(pDecimalPlace <> 0, "." & New String("0", pDecimalPlace), ""))
		Else
			If ExNmText.Text <> vbNullString Then
				If CDbl(ExNmText.Text) <> 0 Then
					ExNmText.Text = VB6.Format(ExNmText.Text, "0" & IIf(pDecimalPlace <> 0, "." & New String("0", pDecimalPlace), ""))
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
		ExNmText.Maxlength = hMaxLength
		''
		''    RaiseEvent GotFocus
	End Sub
	
	Private Sub ExNmText_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ExNmText.Leave
		'UPGRADE_WARNING: TextBox プロパティ ExNmText.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
		ExNmText.Maxlength = 0
		
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
		ExNmText.Text = ChgFormat((ExNmText.Text), pFormatType)
		ExNmText.BackColor = hBackColor
		
		On Error Resume Next
		'UPGRADE_WARNING: Control プロパティ UserControl.Parent は、新しい動作をもつ UserControl.FindForm にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="DFCDE711-9694-47D7-9C50-45A99CD8E91E"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト UserControl.Parent.ActiveControl の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_ISSUE: Control Hwnd は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		MyParentName = MyBase.FindForm.ActiveControl.Hwnd
		Err.Clear()
		On Error GoTo 0
		''    Call EmptyUndoBuffer
		''    RaiseEvent LostFocus
	End Sub
	
	Private Sub ExNmText_KeyDown(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyEventArgs) Handles ExNmText.KeyDown
		Dim KeyCode As Short = eventArgs.KeyCode
		Dim Shift As Short = eventArgs.KeyData \ &H10000
		Dim Rtn_Cancel As Boolean
		
		If Shift = 0 Then
			Select Case KeyCode
				Case System.Windows.Forms.Keys.Return
					RaiseEvent RtnKeyDown(Me, New RtnKeyDownEventArgs(KeyCode, Shift, Rtn_Cancel))
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
		RaiseEvent KeyDown(Me, New KeyDownEventArgs(KeyCode, Shift))
		
		If KeyCode = System.Windows.Forms.Keys.Return And Rtn_Cancel = False Then
			If pCanNextSetFocus = True Then
				KeyCode = 0
				Call NextSetFocus()
			Else
				KeyCode = 0
			End If
		ElseIf KeyCode = System.Windows.Forms.Keys.Down Then 
			If pCanNextSetFocus = True And CtlCursorCondition(ExNmText) = -1 Then
				KeyCode = 0
				Call NextSetFocus()
			Else
				KeyCode = 0
			End If
		ElseIf KeyCode = System.Windows.Forms.Keys.Up Then 
			If pCanForwardSetFocus = True And CtlCursorCondition(ExNmText) = -1 Then
				KeyCode = 0
				Call ForwardSetFocus()
			Else
				KeyCode = 0
			End If
		ElseIf KeyCode = System.Windows.Forms.Keys.Insert Then 
			'入力モードにする
			KeyCode = 0
			If CtlCursorCondition(ExNmText) = -1 Then
				ExNmText.SelectionStart = Len(ExNmText.Text)
			Else
				ExNmText.SelectionStart = 0
				ExNmText.SelectionLength = Len(ExNmText.Text)
			End If
		ElseIf KeyCode = System.Windows.Forms.Keys.Left Then 
			If pEditMode = False Then
				If pCanForwardSetFocus = True And CtlCursorCondition(ExNmText) = -1 Then
					KeyCode = 0
					Call ForwardSetFocus()
				End If
			End If
		ElseIf KeyCode = System.Windows.Forms.Keys.Right Then 
			If pEditMode = False Then
				If pCanNextSetFocus = True And CtlCursorCondition(ExNmText) = -1 Then
					KeyCode = 0
					Call NextSetFocus()
				End If
			End If
		End If
	End Sub
	
	Private Sub ExNmText_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles ExNmText.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		Const Numbers As String = "-0123456789." ' 入力許可文字
		Dim strText As String
		Dim Spc_Cancel As Boolean
		
		If KeyAscii = System.Windows.Forms.Keys.Return Then KeyAscii = 0
		
		'数字検査をする
		If KeyAscii <> 8 Then ' バックスペースは例外
			Select Case KeyAscii
				Case System.Windows.Forms.Keys.Space
					RaiseEvent SpcKeyPress(Me, New SpcKeyPressEventArgs(KeyAscii, Spc_Cancel))
					KeyAscii = 0
					GoTo EventExitSub
			End Select
			
			If InStr(Numbers, Chr(KeyAscii)) = 0 Then
				KeyAscii = 0 ' 入力を無効にする
				GoTo EventExitSub
			ElseIf InStr(".", Chr(KeyAscii)) <> 0 And (pDecimalPlace = 0) Then 
				KeyAscii = 0
				GoTo EventExitSub
			End If
			If pInputMinus = False Then 'マイナス入力不可の場合
				If Chr(KeyAscii) = "-" Then
					KeyAscii = 0
				End If
			End If
			'ﾏｲﾅｽを入力された場合                  '2002/12/10  ADD
			If Chr(KeyAscii) = "-" Then
				KeyAscii = 0
				If InStr(ExNmText.Text, "-") = 0 Then
					If pDecimalPlace = 0 Then
						'UPGRADE_WARNING: TextBox プロパティ ExNmText.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
						If Len(ExNmText.Text) < ExNmText.Maxlength Then
							ExNmText.Text = "-" & ExNmText.Text
						End If
					Else
						If InStr(ExNmText.Text, ".") = 0 Then
							'UPGRADE_WARNING: TextBox プロパティ ExNmText.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
							If Len(ExNmText.Text) < (ExNmText.Maxlength - pDecimalPlace) - 1 Then
								ExNmText.Text = "-" & ExNmText.Text
							End If
						Else
							'UPGRADE_WARNING: TextBox プロパティ ExNmText.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
							If InStr(ExNmText.Text, ".") - 1 < (ExNmText.Maxlength - pDecimalPlace) - 1 Then
								ExNmText.Text = "-" & ExNmText.Text
							End If
						End If
					End If
				Else
					ExNmText.Text = Mid(ExNmText.Text, InStr(ExNmText.Text, "-") + 1)
				End If
				Me.SelStart = Len(Me.Text)
			End If '-------------------------
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
			strText = InsStrToTextBox(ExNmText, KeyAscii)
			'''    '末尾に何かしらの数字を付加
			strText = strText & IIf(pInputZero = True, "0", "1")
			'小数点、マイナスの場合ゼロを内部的につける
			'        If Not IsNumeric(strText) Then
			'            strText = strText & "0"
			'        End If
			
			'数字以外になりそうなキー入力は排除
			If isNumericMatch(strText) = False Then
				KeyAscii = 0
				GoTo EventExitSub
			End If
			
			Select Case Len(ExNmText.SelectedText)
				Case 0
					If pDecimalPlace = 0 Then
						'UPGRADE_WARNING: TextBox プロパティ ExNmText.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
						If Len(strText) > ExNmText.Maxlength + 1 Then
							KeyAscii = 0
						End If
					Else
						If InStr(strText, ".") = 0 Then
							'UPGRADE_WARNING: TextBox プロパティ ExNmText.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
							If Len(strText) >= (ExNmText.Maxlength - pDecimalPlace) + 1 Then
								KeyAscii = 0
							End If
						Else
							If ExNmText.SelectionStart < InStr(strText, ".") Then '小数点より左側（整数部分）
								'UPGRADE_WARNING: TextBox プロパティ ExNmText.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
								If InStr(strText, ".") > (ExNmText.Maxlength - pDecimalPlace) Then
									KeyAscii = 0
								End If
							Else
								If (Len(strText) - InStr(strText, ".")) > pDecimalPlace + 1 Then
									KeyAscii = 0
								Else
									If Chr(KeyAscii) = "-" Then
										KeyAscii = 0
									End If
								End If
							End If
						End If
					End If
				Case Is < Len(ExNmText.Text)
					If InStr(ExNmText.SelectedText, ".") <> 0 Then
						KeyAscii = 0
					End If
			End Select
			
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
		
		RaiseEvent KeyPress(Me, New KeyPressEventArgs(KeyAscii))
EventExitSub: 
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
	
	Private Sub ExNmText_KeyUp(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyEventArgs) Handles ExNmText.KeyUp
		Dim KeyCode As Short = eventArgs.KeyCode
		Dim Shift As Short = eventArgs.KeyData \ &H10000
		RaiseEvent KeyUp(Me, New KeyUpEventArgs(KeyCode, Shift))
	End Sub
	
	Private Sub ExNmText_MouseDown(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles ExNmText.MouseDown
		Dim Button As Short = eventArgs.Button \ &H100000
		Dim Shift As Short = System.Windows.Forms.Control.ModifierKeys \ &H10000
		Dim X As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim Y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		RaiseEvent MouseDown(Me, New MouseDownEventArgs(Button, Shift, X, Y))
		fClicking = True
	End Sub
	
	Private Sub ExNmText_MouseMove(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles ExNmText.MouseMove
		Dim Button As Short = eventArgs.Button \ &H100000
		Dim Shift As Short = System.Windows.Forms.Control.ModifierKeys \ &H10000
		Dim X As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim Y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		RaiseEvent MouseMove(Me, New MouseMoveEventArgs(Button, Shift, X, Y))
	End Sub
	
	Private Sub ExNmText_MouseUp(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles ExNmText.MouseUp
		Dim Button As Short = eventArgs.Button \ &H100000
		Dim Shift As Short = System.Windows.Forms.Control.ModifierKeys \ &H10000
		Dim X As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim Y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		RaiseEvent MouseUp(Me, New MouseUpEventArgs(Button, Shift, X, Y))
		fClicking = False
	End Sub
	
	Private Sub ExNmTextBox_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Enter
		fGotFocus = True
		On Error Resume Next
		If Me.Hwnd <> MyParentName Then
			'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			vUNDOBUF = ExNmText.Text
		End If
		If Err.Number <> 0 Then
			'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			vUNDOBUF = ExNmText.Text
		End If
		Err.Clear()
		On Error GoTo 0
	End Sub
	
	Private Sub ExNmTextBox_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Leave
		fGotFocus = False
		fClicking = False
	End Sub
	
	Private Sub ExNmTextBox_GotFocus(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.GotFocus
		fGotFocus = True
		'    ExText.SetFocus
	End Sub
	
	Private Sub ExNmTextBox_LostFocus(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.LostFocus
		fGotFocus = False
	End Sub
	
	Private Sub UserControl_Terminate()
		'終了時にIMEModeを0-なしにしないと
		'WinMEでimm32.dllがこける。
		ExNmText.IMEMode = System.Windows.Forms.ImeMode.NoControl
	End Sub
	
	Private Sub ExNmTextBox_Resize(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Resize
		With ExNmText
			.Top = 0
			.Left = 0
			.Width = MyBase.Width
			'UPGRADE_ISSUE: UserControl プロパティ ExNmTextBox.Extender はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト Extender.Width の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			Extender.Width = VB6.PixelsToTwipsX(.Width) '2002/08/15 ADD
			.Height = MyBase.Height
			'UPGRADE_ISSUE: UserControl プロパティ ExNmTextBox.Extender はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
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
		Me.MaxLength = 12
		'UPGRADE_ISSUE: AmbientProperties プロパティ Ambient.Font はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Me.Font = Ambient.Font
		pInputZero = True
		pInputPlus = True
		pInputMinus = True
		pDecimalPlace = 0
		pFormatType = ""
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
		'    ExNmText.Text = PropBag.ReadProperty("Text", Text)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Me.Text = PropBag.ReadProperty("Text", Text)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Me.Enabled = PropBag.ReadProperty("Enabled", True)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Me.Locked = PropBag.ReadProperty("Locked", False)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Me.MaxLength = PropBag.ReadProperty("MaxLength", 12)
		'    hMaxLength = PropBag.ReadProperty("hMaxLength", 12)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Me.ForeColor = System.Drawing.ColorTranslator.FromOle(PropBag.ReadProperty("ForeColor", System.Drawing.ColorTranslator.ToOle(System.Drawing.SystemColors.WindowText)))
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Me.BackColor = System.Drawing.ColorTranslator.FromOle(PropBag.ReadProperty("BackColor", System.Drawing.ColorTranslator.ToOle(System.Drawing.SystemColors.Window)))
		'    ExNmText.IMEMode = PropBag.ReadProperty("IMEMode", 0)
		'UPGRADE_ISSUE: AmbientProperties プロパティ Ambient.Font はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Me.Font = PropBag.ReadProperty("Font", Ambient.Font)
		
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		pFocusBackColor = System.Drawing.ColorTranslator.FromOle(PropBag.ReadProperty("FocusBackColor", System.Drawing.ColorTranslator.ToOle(System.Drawing.SystemColors.Window)))
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		pInputZero = PropBag.ReadProperty("InputZero", pInputZero)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		pInputPlus = PropBag.ReadProperty("InputPlus", pInputPlus)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		pInputMinus = PropBag.ReadProperty("InputMinus", pInputMinus)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		pDecimalPlace = PropBag.ReadProperty("DecimalPlace", pDecimalPlace)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		pFormatType = PropBag.ReadProperty("Format", pFormatType)
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
		ExNmText.TextAlign = PropBag.ReadProperty("Alignment", 0)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_ISSUE: TextBox プロパティ ExNmText.Appearance はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		ExNmText.Appearance = PropBag.ReadProperty("Appearance", 1)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		ExNmText.BorderStyle = PropBag.ReadProperty("BorderStyle", 1)
		
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.ReadProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト PropBag.ReadProperty() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		vUNDOBUF = PropBag.ReadProperty("OldValue", vUNDOBUF)
	End Sub
	
	'UPGRADE_ISSUE: VBRUN.PropertyBag 型 はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6B85A2A7-FE9F-4FBE-AA0C-CF11AC86A305"' をクリックしてください。
	'UPGRADE_WARNING: UserControl イベント WriteProperties はサポートされていません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="92F3B58C-F772-4151-BE90-09F4A232AEAD"' をクリックしてください。
	Private Sub UserControl_WriteProperties(ByRef PropBag As Object)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("Text", ExNmText.Text)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("Enabled", Me.Enabled, True)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("Locked", Me.Locked, False)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("MaxLength", Me.MaxLength)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("hMaxLength", hMaxLength)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("ForeColor", System.Drawing.ColorTranslator.ToOle(Me.ForeColor))
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("BackColor", System.Drawing.ColorTranslator.ToOle(Me.BackColor))
		'    Call PropBag.WriteProperty("IMEMode", ExNmText.IMEMode)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("Font", Me.Font)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("FocusBackColor", pFocusBackColor)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("InputZero", pInputZero)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("InputPlus", pInputPlus)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("InputMinus", pInputMinus)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("DecimalPlace", pDecimalPlace)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("Format", pFormatType)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("CanNextSetFocus", pCanNextSetFocus)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("CanForwardSetFocus", pCanForwardSetFocus)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("SelectText", pSelectText)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("EditMode", pEditMode)
		
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("Alignment", ExNmText.TextAlign, 0)
		'UPGRADE_ISSUE: TextBox プロパティ ExNmText.Appearance はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("Appearance", ExNmText.Appearance, 1)
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("BorderStyle", ExNmText.BorderStyle, 1)
		
		'UPGRADE_ISSUE: PropertyBag メソッド PropBag.WriteProperty はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
		Call PropBag.WriteProperty("OldValue", vUNDOBUF)
	End Sub
	
	
	Public Overrides Property Text() As String
		Get
			''    Text = ExNmText.Text
			'UPGRADE_ISSUE: 定数 vbUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			Text = StrConv(StrConv(ExNmText.Text(), vbFromUnicode), vbUnicode)
		End Get
		Set(ByVal Value As String)
			Dim ParentHwnd As Integer
			
			'UPGRADE_WARNING: TextBox プロパティ ExNmText.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
			ExNmText.Maxlength = 0 '01/08/28
			
			On Error Resume Next '01/09/05
			If Me.Hwnd <> MyParentName Then
				'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				vUNDOBUF = Value
			End If
			If Err.Number <> 0 Then
				'UPGRADE_WARNING: オブジェクト vUNDOBUF の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				vUNDOBUF = Value
			End If
			Err.Clear()
			On Error GoTo 0
			
			On Error Resume Next '02/01/15
			'UPGRADE_WARNING: Control プロパティ UserControl.Parent は、新しい動作をもつ UserControl.FindForm にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="DFCDE711-9694-47D7-9C50-45A99CD8E91E"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト UserControl.Parent.ActiveControl の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_ISSUE: Control Hwnd は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
			ParentHwnd = MyBase.FindForm.ActiveControl.Hwnd
			If Err.Number Then
				ExNmText.Text = ChgFormat(Value, pFormatType)
			Else
				If Me.Hwnd = ParentHwnd Then
					If pInputZero Then
						ExNmText.Text = VB6.Format(Value, "0" & IIf(pDecimalPlace <> 0, "." & New String("0", pDecimalPlace), ""))
					Else
						If Value <> vbNullString Then
							If CDbl(Value) <> 0 Then
								ExNmText.Text = VB6.Format(Value, "0" & IIf(pDecimalPlace <> 0, "." & New String("0", pDecimalPlace), ""))
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
			End If
			On Error GoTo 0
			
			'UPGRADE_WARNING: TextBox プロパティ ExNmText.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
			ExNmText.Maxlength = hMaxLength
			
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
	
	
	Public Property Alignment() As System.Drawing.ContentAlignment
		Get
			Alignment = ExNmText.TextAlign
		End Get
		Set(ByVal Value As System.Drawing.ContentAlignment)
			ExNmText.TextAlign = Value
			RaiseEvent AlignmentChange()
		End Set
	End Property
	
	
	Public Property Appearance() As AppearanceType
		Get
			'UPGRADE_ISSUE: TextBox プロパティ ExNmText.Appearance はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
			Appearance = ExNmText.Appearance
		End Get
		Set(ByVal Value As AppearanceType)
			'UPGRADE_ISSUE: TextBox プロパティ ExNmText.Appearance はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
			ExNmText.Appearance = Value
			RaiseEvent AppearanceChange()
		End Set
	End Property
	
	
	Public Shadows Property BorderStyle() As BorderStyleType
		Get
			BorderStyle = ExNmText.BorderStyle
		End Get
		Set(ByVal Value As BorderStyleType)
			ExNmText.BorderStyle = Value
			RaiseEvent BorderStyleChange()
		End Set
	End Property
	
	
	Public Property MaxLength() As Integer
		Get
			'フォーカスを持っていない場合、ZEROなのでHOLDの方を返す。
			'フォーカスを持っている場合は、同じ値のはず！
			'UPGRADE_WARNING: TextBox プロパティ ExNmText.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
			If ExNmText.Maxlength = 0 Then '2002/01/21
				MaxLength = hMaxLength
			Else
				'UPGRADE_WARNING: TextBox プロパティ ExNmText.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				MaxLength = ExNmText.Maxlength
			End If
		End Get
		Set(ByVal Value As Integer)
			'UPGRADE_WARNING: TextBox プロパティ ExNmText.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
			ExNmText.Maxlength = Value
			hMaxLength = Value
			RaiseEvent MaxLengthChange()
		End Set
	End Property
	
	
	Public Property SelStart() As Integer
		Get
			SelStart = ExNmText.SelectionStart
		End Get
		Set(ByVal Value As Integer)
			If Not DesignMode = False Then Err.Raise(382)
			ExNmText.SelectionStart = Value
			RaiseEvent SelStartChange()
		End Set
	End Property
	
	
	Public Property SelLength() As Integer
		Get
			SelLength = ExNmText.SelectionLength
		End Get
		Set(ByVal Value As Integer)
			If Not DesignMode = False Then Err.Raise(382)
			ExNmText.SelectionLength = Value
			RaiseEvent SelLengthChange()
		End Set
	End Property
	
	
	Public Property SelText() As String
		Get
			SelText = ExNmText.SelectedText
		End Get
		Set(ByVal Value As String)
			If Not DesignMode = False Then Err.Raise(382)
			ExNmText.SelectedText = Value
			RaiseEvent SelTextChange()
		End Set
	End Property
	
	
	Public Overrides Property ForeColor() As System.Drawing.Color
		Get
			ForeColor = ExNmText.ForeColor
		End Get
		Set(ByVal Value As System.Drawing.Color)
			ExNmText.ForeColor = Value
			RaiseEvent ForeColorChange()
		End Set
	End Property
	
	
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
	
	
	Public Property FocusBackColor() As System.Drawing.Color
		Get
			FocusBackColor = pFocusBackColor
		End Get
		Set(ByVal Value As System.Drawing.Color)
			pFocusBackColor = Value
			RaiseEvent FocusBackColorChange()
		End Set
	End Property
	'
	'Public Property Get IMEMode() As IMEModeType
	'    IMEMode = ExNmText.IMEMode
	'End Property
	'
	'Public Property Let IMEMode(ByVal New_IMEMode As IMEModeType)
	'    ExNmText.IMEMode() = New_IMEMode
	'    PropertyChanged ("IMEMode")
	'End Property
	
	
	Public Overrides Property Font() As System.Drawing.Font
		Get
			Font = ExNmText.Font
		End Get
		Set(ByVal Value As System.Drawing.Font)
			ExNmText.Font = Value
			RaiseEvent FontChange()
		End Set
	End Property
	
	
	Public Property InputZero() As Boolean
		Get
			InputZero = pInputZero
		End Get
		Set(ByVal Value As Boolean)
			pInputZero = Value
			RaiseEvent InputZeroChange()
		End Set
	End Property
	
	
	Public Property InputPlus() As Boolean
		Get
			InputPlus = pInputPlus
		End Get
		Set(ByVal Value As Boolean)
			pInputPlus = Value
			RaiseEvent InputPlusChange()
		End Set
	End Property
	
	
	Public Property InputMinus() As Boolean
		Get
			InputMinus = pInputMinus
		End Get
		Set(ByVal Value As Boolean)
			pInputMinus = Value
			RaiseEvent InputMinusChange()
		End Set
	End Property
	
	
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
	
	
	Public Property FormatType() As String
		Get
			FormatType = pFormatType
		End Get
		Set(ByVal Value As String)
			pFormatType = Value
			'''    ExNmText.Text = ChgFormat(ExNmText.Text, pFormatType)
			RaiseEvent FormatTypeChange()
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
			Hwnd = ExNmText.Handle.ToInt32
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
	'''''''Private Function InsStrToTextBox(ByRef TxText As TextBox, ByVal sInsStr As String) As String
	Private Function InsStrToTextBox(ByRef TxText As System.Windows.Forms.TextBox, ByVal sInsStr As Integer) As String
		'UPGRADE_NOTE: str は str_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
		Dim str_Renamed As String
		If sInsStr = 0 Then
			InsStrToTextBox = TxText.Text
			Exit Function 'KeyAsciiが０なら何もしない
		End If
		
		str_Renamed = Chr(sInsStr)
		With TxText
			'        If sInsStr = "-" Then
			'            InsStrToTextBox = sInsStr & Left$(.Text, .SelStart) & Right$(.Text, Len(.Text) - .SelStart - .SelLength)
			'        Else
			''''            InsStrToTextBox = Left$(.Text, .SelStart) & sInsStr & Right$(.Text, Len(.Text) - .SelStart - .SelLength)
			InsStrToTextBox = VB.Left(.Text, .SelectionStart) & str_Renamed & VB.Right(.Text, Len(.Text) - .SelectionStart - .SelectionLength)
			'        End If
		End With
	End Function
	
	'文字の内容が各条件に合っているか？
	Private Function isNumericMatch(ByVal Expression As Object) As Boolean
		Dim lBuf As Integer
		Dim dBuf As Double
		
		On Error GoTo Err_isNumericMatch
		'数字か？
		If IsNumeric(Expression) = False Then
			isNumericMatch = False
			Exit Function
		End If
		'文字から数値へ変換
		'UPGRADE_WARNING: オブジェクト Expression の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		dBuf = CDbl(Expression)
		'負数の条件に適合しているか？
		If pInputMinus = False Then
			If dBuf < 0 Then
				isNumericMatch = False
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
						isNumericMatch = False
						Exit Function
					End If
				End If
			End If
		End If
		'正数の条件に適合しているか？
		If pInputPlus = False Then
			If dBuf > 0 Then
				isNumericMatch = False
				Exit Function
			End If
		End If
		'''整数の条件に適合しているか？
		''    If bInteger = False Then
		''        lBuf = CLng(dBuf)
		''        If CDbl(lBuf) <> dBuf Then
		''            isNumericMatch = False
		''            Exit Function
		''        End If
		''    End If
		''''''''    Dim DecPos As Integer
		''''''''    Dim DecNum As Integer
		''''''''
		''''''''    DecPos = InStr(Expression, ".")
		''''''''''    DecPos = InStr(dBuf, ".")
		''''''''    If DecPos <> 0 Then
		''''''''        DecNum = Len(Expression) - DecPos
		''''''''''        DecNum = Len(CStr(dBuf)) - DecPos
		''''''''        If pDecimalPlace < DecNum Then
		''''''''            isNumericMatch = False
		''''''''            Exit Function
		''''''''        End If
		''''''''    End If
		
		isNumericMatch = True
		Exit Function
		'エラー処理
Err_isNumericMatch: 
		isNumericMatch = False
	End Function
	
	Private Function ChgFormat(ByRef tx_Text As String, ByRef FormatType As String) As String
		'''    Dim aryFormat() As String
		'''    Dim i As Long
		'''
		'''    Call StrSplit(FormatType, ";", aryFormat, 4)
		'''
		'''    If IsNull(tx_Text) Then
		'''        i = 3
		'''        tx_Text = "0"
		'''    ElseIf tx_Text = "" Then
		'''        i = 3
		'''        tx_Text = "0"
		'''    ElseIf CDbl(tx_Text) > 0 Then
		'''        i = 0
		'''    ElseIf CDbl(tx_Text) < 0 Then
		'''        i = 1
		'''    Else
		'''        i = 2
		'''    End If
		'''    If aryFormat(i) <> "" Then
		If Not IsNumeric(tx_Text) Then
			tx_Text = ""
		End If
		
		If Len(tx_Text) > hMaxLength Then
			'''        tx_Text = Left$(tx_Text, hMaxLength)     2001/12/14
			tx_Text = VB6.Format(tx_Text, "")
		End If
		
		If FormatType <> "" Then
			'''        ChgFormat = Format(tx_Text, aryFormat(i))
			ChgFormat = VB6.Format(tx_Text, FormatType)
		Else
			ChgFormat = tx_Text
		End If
		If VB.Right(ChgFormat, 1) = "." Then '一番左が小数点ならば
			ChgFormat = VB.Left(ChgFormat, Len(ChgFormat) - 1)
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
				lngMaxArray = lngMaxArray + MAX_ARRAY '配列要素最大数をMAX_ARRAY増やす
				ReDim Preserve strArray(lngMaxArray)
			End If
			p2 = InStr(p1, strExpression, strDelimiter, lngCompare) '区切り文字を検索しその位置を返す
			If p2 Then '区切り文字が存在した場合
				strArray(lngCnt) = Mid(strExpression, p1, p2 - p1) '検索開始点から 区切り文字位置 - 1 までの文字列をを配列に代入
				p1 = p2 + lngDivLen '次回の検索開始点を設定
				lngCnt = lngCnt + 1 '次回のため項目数を一つ増やす
			Else '区切り文字が存在しない場合
				If lngCnt Then '最後の要素である場合
					strArray(lngCnt) = VB.Right(strExpression, lngStrLen - p1 + 1) '文字列の最後から検索開始点までの文字列を配列に代入
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
			Call PostMessage(ExNmText.Handle.ToInt32, WM_KEYDOWN, System.Windows.Forms.Keys.Tab, 0)
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