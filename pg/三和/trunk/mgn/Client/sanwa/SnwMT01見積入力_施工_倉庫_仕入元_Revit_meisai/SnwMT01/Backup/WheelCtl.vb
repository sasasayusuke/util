Option Strict Off
Option Explicit On
Module WheelCtl
	'Ver.1.00           '2003.09.22 Spread3.0Jでのホイール制御の為作成
	
	Private Declare Sub mouse_event Lib "user32" (ByVal dwFlags As Integer, ByVal dx As Integer, ByVal dy As Integer, ByVal cButtons As Integer, ByVal dwExtraInfo As Integer)
	Private Declare Function SetWindowLong Lib "user32"  Alias "SetWindowLongA"(ByVal Hwnd As Integer, ByVal nindex As Integer, ByVal dwNewLong As Integer) As Integer
	
	Private Declare Function CallWindowProc Lib "user32"  Alias "CallWindowProcA"(ByVal lpPrevWndFunc As Integer, ByVal Hwnd As Integer, ByVal msg As Integer, ByVal wParam As Integer, ByRef lParam As Integer) As Integer
	Public Declare Function GetWindowLong Lib "user32"  Alias "GetWindowLongA"(ByVal Hwnd As Integer, ByVal nindex As Integer) As Integer
	
	Public Const GWL_USERDATA As Short = (-21)
	Private Const GWL_WNDPROC As Short = (-4)
	
	Private Const WM_LBUTTONDOWN As Integer = &H201
	Private Const WM_LBUTTONUP As Integer = &H202
	Private Const WM_MBUTTONDOWN As Integer = &H207
	Private Const WM_MBUTTONUP As Integer = &H208
	Private Const WM_RBUTTONDOWN As Integer = &H204
	Private Const WM_RBUTTONUP As Integer = &H205
	Private Const WM_MOUSEWHEEL As Integer = &H20A
	
	Dim wm_WheelCtl As System.Windows.Forms.Control 'ホイール制御を行うコントロール用
	Dim wm_WheelCnt As Short 'ホイールの回転量
	
	Public Sub StartWheel(ByRef ctl As System.Windows.Forms.Control, Optional ByRef WheelCnt As Short = 3)
		wm_WheelCtl = ctl
		wm_WheelCnt = WheelCnt
		'フックの開始
		'UPGRADE_WARNING: AddressOf SubClassProc の delegate を追加する 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="E9E157F7-EF0C-4016-87B7-7D7FBBC6EE08"' をクリックしてください。
		SetWindowLong(wm_WheelCtl.Handle.ToInt32, GWL_USERDATA, SetWindowLong(wm_WheelCtl.Handle.ToInt32, GWL_WNDPROC, AddressOf SubClassProc))
	End Sub
	
	Public Sub EndWheel()
		If Not wm_WheelCtl Is Nothing Then
			If wm_WheelCtl.Handle.ToInt32 <> 0 Then
				'フックの終了
				SetWindowLong(wm_WheelCtl.Handle.ToInt32, GWL_WNDPROC, GetWindowLong(wm_WheelCtl.Handle.ToInt32, GWL_USERDATA))
			End If
		End If
		'コントロールの開放
		'UPGRADE_NOTE: オブジェクト wm_WheelCtl をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		wm_WheelCtl = Nothing
	End Sub
	
	Public Function SubClassProc(ByVal hwndx As Integer, ByVal uMsg As Integer, ByVal wParam As Integer, ByRef lParam As Integer) As Integer
		Static Calling As Boolean
		If Not Calling Then
			Calling = True
			Select Case uMsg
				Case WM_MBUTTONUP
				Case WM_MOUSEWHEEL
					If wParam < 0 Then
						Call vbWheelDown()
						uMsg = 0
					Else
						Call vbWheelUp()
						uMsg = 0
					End If
			End Select
			Calling = False
		End If
		SubClassProc = CallWindowProc(GetWindowLong(hwndx, GWL_USERDATA), hwndx, uMsg, wParam, lParam)
	End Function
	
	Public Sub vbWheelUp()
		With wm_WheelCtl
			'UPGRADE_WARNING: オブジェクト wm_WheelCtl.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト wm_WheelCtl.TopRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row = .TopRow
			'UPGRADE_WARNING: オブジェクト wm_WheelCtl.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If .Row - wm_WheelCnt <= 1 Then
				'UPGRADE_WARNING: オブジェクト wm_WheelCtl.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Row = 1
			Else
				'UPGRADE_WARNING: オブジェクト wm_WheelCtl.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Row = .Row - wm_WheelCnt
			End If
			'UPGRADE_WARNING: オブジェクト wm_WheelCtl.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Action = FPSpreadADO.ActionConstants.ActionGotoCell
		End With
	End Sub
	
	Public Sub vbWheelDown()
		With wm_WheelCtl
			'UPGRADE_WARNING: オブジェクト wm_WheelCtl.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト wm_WheelCtl.TopRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row = .TopRow
			'UPGRADE_WARNING: オブジェクト wm_WheelCtl.MaxRows の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト wm_WheelCtl.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If .Row + wm_WheelCnt > .MaxRows Then
				'UPGRADE_WARNING: オブジェクト wm_WheelCtl.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト wm_WheelCtl.MaxRows の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Row = .MaxRows
			Else
				'UPGRADE_WARNING: オブジェクト wm_WheelCtl.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Row = .Row + wm_WheelCnt
			End If
			'UPGRADE_WARNING: オブジェクト wm_WheelCtl.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Action = FPSpreadADO.ActionConstants.ActionGotoCell
		End With
	End Sub
End Module