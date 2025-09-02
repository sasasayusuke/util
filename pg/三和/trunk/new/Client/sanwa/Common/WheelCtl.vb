Option Strict On
Option Explicit On

Imports System.Runtime.InteropServices
Imports FarPoint.Win.Spread

''' <summary>
''' Ver.1.00           '2003.09.22 Spread3.0Jでのホイール制御の為作成
''' Ver.1.00           '2024.11.12 Spread17.0Jでのホイール制御の為作成
''' </summary>
Public Module WheelCtl

	'Private Declare Sub mouse_event Lib "user32" (ByVal dwFlags As Integer, ByVal dx As Integer, ByVal dy As Integer, ByVal cButtons As Integer, ByVal dwExtraInfo As Integer)
	' API関数の宣言（DllImportを使用）
	<DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
	Private Sub mouse_event(dwFlags As UInteger, dx As Integer, dy As Integer, cButtons As UInteger, dwExtraInfo As UIntPtr)
	End Sub

	'Private Declare Function SetWindowLong Lib "user32" Alias "SetWindowLongA" (ByVal Hwnd As Integer, ByVal nindex As Integer, ByVal dwNewLong As Integer) As Integer
	<DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
	Private Function SetWindowLong(hwnd As IntPtr, nIndex As Integer, dwNewLong As IntPtr) As IntPtr
	End Function

	'Private Declare Function CallWindowProc Lib "user32" Alias "CallWindowProcA" (ByVal lpPrevWndFunc As Integer, ByVal Hwnd As Integer, ByVal msg As Integer, ByVal wParam As Integer, ByRef lParam As Integer) As Integer
	<DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
	Private Function CallWindowProc(ByVal lpPrevWndFunc As IntPtr, ByVal hwnd As IntPtr, ByVal msg As UInteger, ByVal wParam As IntPtr, ByVallParam As IntPtr) As IntPtr
	End Function

	'Public Declare Function GetWindowLong Lib "user32" Alias "GetWindowLongA" (ByVal Hwnd As Integer, ByVal nindex As Integer) As Integer
	<DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
	Private Function GetWindowLong(hwnd As IntPtr, nIndex As Integer) As IntPtr
	End Function

	Public Const GWL_USERDATA As Integer = (-21)
	Private Const GWL_WNDPROC As Integer = (-4)
	Private Const WM_LBUTTONDOWN As Integer = &H201
	Private Const WM_LBUTTONUP As Integer = &H202
	Private Const WM_MBUTTONDOWN As Integer = &H207
	Private Const WM_MBUTTONUP As Integer = &H208
	Private Const WM_RBUTTONDOWN As Integer = &H204
	Private Const WM_RBUTTONUP As Integer = &H205
	Private Const WM_MOUSEWHEEL As Integer = &H20A

	Dim wm_WheelCtl As FpSpread 'ホイール制御を行うコントロール用
	Dim wm_WheelCnt As Integer 'ホイールの回転量
	Private prevWndProc As IntPtr = IntPtr.Zero

	' デリゲート型の定義
	Private Delegate Function WndProcDelegate(hWnd As IntPtr, uMsg As Integer, wParam As IntPtr, lParam As IntPtr) As IntPtr

	' デリゲートインスタンスとGCHandle
	Private SubClassProcDelegate As WndProcDelegate
	Private DelegateHandle As GCHandle

	Public Sub StartWheel(ByRef ctl As FpSpread, Optional ByRef WheelCnt As Integer = 3)
		wm_WheelCtl = ctl
		wm_WheelCnt = WheelCnt

		' デリゲート作成
		'UPGRADE_WARNING: AddressOf SubClassProc の delegate を追加する 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="E9E157F7-EF0C-4016-87B7-7D7FBBC6EE08"' をクリックしてください。
		SubClassProcDelegate = AddressOf SubClassProc

		' デリゲートをGCHandleでGC対象外にする
		DelegateHandle = GCHandle.Alloc(SubClassProcDelegate)

		'フックの開始
		prevWndProc = SetWindowLong(wm_WheelCtl.Handle, GWL_WNDPROC, Marshal.GetFunctionPointerForDelegate(SubClassProcDelegate))
	End Sub

	Public Sub EndWheel()
		If wm_WheelCtl IsNot Nothing Then
			If wm_WheelCtl.Handle <> prevWndProc Then
				'フックの終了
				SetWindowLong(wm_WheelCtl.Handle, GWL_WNDPROC, GetWindowLong(wm_WheelCtl.Handle, GWL_USERDATA))
			End If
		End If

		' GCHandleの解放
		If DelegateHandle.IsAllocated Then
			DelegateHandle.Free()
		End If

		'コントロールの開放
		'UPGRADE_NOTE: オブジェクト wm_WheelCtl をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		wm_WheelCtl = Nothing
	End Sub

	Public Function SubClassProc(ByVal hWnd As IntPtr, ByVal uMsg As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
		Static Calling As Boolean
		If Not Calling Then
			Calling = True
			Select Case uMsg
				Case WM_MBUTTONUP
				Case WM_MOUSEWHEEL
					If wParam.ToInt32() < 0 Then
						Call vbWheelDown()
						uMsg = 0
					Else
						Call vbWheelUp()
						uMsg = 0
					End If
			End Select
			Calling = False
		End If
		Return CallWindowProc(prevWndProc, hWnd, CUInt(uMsg), wParam, lParam)
	End Function

	Public Sub vbWheelUp()
		With wm_WheelCtl
			'UPGRADE_WARNING: オブジェクト wm_WheelCtl.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト wm_WheelCtl.TopRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'.Row = .TopRow
			.ActiveSheet.ActiveRowIndex = .GetViewportTopRow(0)
			'UPGRADE_WARNING: オブジェクト wm_WheelCtl.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'If .Row - wm_WheelCnt <= 1 Then
			If .ActiveSheet.ActiveRowIndex - wm_WheelCnt <= 1 Then
				'UPGRADE_WARNING: オブジェクト wm_WheelCtl.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'.Row = 1
				.ActiveSheet.ActiveRowIndex = 0
			Else
				'UPGRADE_WARNING: オブジェクト wm_WheelCtl.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'.Row = .Row - wm_WheelCnt
				.ActiveSheet.ActiveRowIndex = .ActiveSheet.ActiveRowIndex - wm_WheelCnt
			End If
			'UPGRADE_WARNING: オブジェクト wm_WheelCtl.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'.Action = FPSpreadADO.ActionConstants.ActionGotoCell
			.ActiveSheet.SetActiveCell(.ActiveSheet.ActiveRowIndex, .ActiveSheet.ActiveColumnIndex)
		End With
	End Sub

	Public Sub vbWheelDown()
		With wm_WheelCtl
			'UPGRADE_WARNING: オブジェクト wm_WheelCtl.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト wm_WheelCtl.TopRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'.Row = .TopRow
			.ActiveSheet.ActiveRowIndex = .GetViewportTopRow(0)
			'UPGRADE_WARNING: オブジェクト wm_WheelCtl.MaxRows の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト wm_WheelCtl.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'If .Row + wm_WheelCnt > .MaxRows Then
			If .ActiveSheet.ActiveRowIndex + wm_WheelCnt > .ActiveSheet.RowCount Then
				'UPGRADE_WARNING: オブジェクト wm_WheelCtl.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト wm_WheelCtl.MaxRows の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'.Row = .MaxRows
				.ActiveSheet.ActiveRowIndex = .ActiveSheet.RowCount
			Else
				'UPGRADE_WARNING: オブジェクト wm_WheelCtl.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'.Row = .Row + wm_WheelCnt
				.ActiveSheet.ActiveRowIndex = .ActiveSheet.ActiveRowIndex + wm_WheelCnt
			End If
			'UPGRADE_WARNING: オブジェクト wm_WheelCtl.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'.Action = FPSpreadADO.ActionConstants.ActionGotoCell
			.ActiveSheet.SetActiveCell(.ActiveSheet.ActiveRowIndex, .ActiveSheet.ActiveColumnIndex)
		End With
	End Sub

End Module
