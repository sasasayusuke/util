Option Strict Off
Option Explicit On
Imports System.Runtime.InteropServices

''' <summary>
''' Ver.1.00           '2003.09.25 OptionButtonのカーソル制御の為作成
''' </summary>
Friend Class OpSubCls

	'UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
	'Private Declare Function PostMessage Lib "user32"  Alias "PostMessageA"(ByVal Hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer, ByRef lParam As Any) As Integer

	'PostMessage 
	<DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
	Private Shared Function PostMessage(ByVal hwnd As IntPtr, ByVal wMsg As UInteger, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As Boolean
	End Function

	Private Const WM_KEYDOWN As Integer = &H100
	'Private Const WM_KEYUP = &H101
	Private Const VK_TAB As Integer = &H9

	'Private Declare Sub keybd_event Lib "user32" (ByVal bVk As Byte, ByVal bScan As Byte, ByVal dwFlags As Integer, ByVal dwExtraInfo As Integer)

	'keybd_event 
	<DllImport("user32.dll", SetLastError:=True)>
	Private Shared Sub keybd_event(ByVal bVk As Byte, ByVal bScan As Byte, ByVal dwFlags As UInteger, ByVal dwExtraInfo As UIntPtr)
	End Sub

	Private Const KEYEVENTF_KEYUP As Integer = &H2

	'  'Private Declare Function SendMessage Lib "user32" Alias "SendMessageA" (ByVal Hwnd As Long, ByVal wMsg As Long, ByVal wParam As Long, lParam As Any) As Long
	'Private Declare Function SetWindowLong Lib "user32" Alias "SetWindowLongA" (ByVal Hwnd As Integer, ByVal nindex As Integer, ByVal dwNewLong As Integer) As Integer
	<DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
	Private Shared Function SetWindowLong(ByVal hwnd As IntPtr, ByVal nIndex As Integer, ByVal dwNewLong As IntPtr) As IntPtr
	End Function

	'Private Declare Function CallWindowProc Lib "user32" Alias "CallWindowProcA" (ByVal lpPrevWndFunc As Integer, ByVal Hwnd As Integer, ByVal msg As Integer, ByVal wParam As Integer, ByRef lParam As Integer) As Integer
	<DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
	Private Shared Function CallWindowProc(ByVal lpPrevWndFunc As IntPtr, ByVal hwnd As IntPtr, ByVal msg As UInteger, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
	End Function

	Private Const GWL_WNDPROC As Short = (-4)

	Dim wm_OptionCtl As System.Windows.Forms.Control 'ホイール制御を行うコントロール用

	Private lpOrg As IntPtr = IntPtr.Zero '古いｳｨﾝﾄﾞｳﾌﾟﾛｼｰｼﾞｬのｱﾄﾞﾚｽが入る。


	' デリゲート型の定義
	Private Delegate Function WndProcDelegate(ByVal hwnd As IntPtr, ByVal msg As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr

	' デリゲートインスタンスとGCHandle
	Private SubClassDelegate As WndProcDelegate
	Private DelegateHandle As GCHandle

	Public Sub StartOption(ByRef ctl As System.Windows.Forms.Control)
		wm_OptionCtl = ctl

		' デリゲート作成
		'UPGRADE_WARNING: AddressOf SubClassProc の delegate を追加する 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="E9E157F7-EF0C-4016-87B7-7D7FBBC6EE08"' をクリックしてください。
		SubClassDelegate = AddressOf SubClassProc

		' デリゲートをGCHandleでGC対象外にする
		DelegateHandle = GCHandle.Alloc(SubClassDelegate)

		'フックの開始
		lpOrg = SetWindowLong(wm_OptionCtl.Handle, GWL_WNDPROC, Marshal.GetFunctionPointerForDelegate(SubClassDelegate))
	End Sub

	Public Sub EndOption()
		If lpOrg <> IntPtr.Zero Then
			'フックの終了
			SetWindowLong(wm_OptionCtl.Handle, GWL_WNDPROC, lpOrg)
			lpOrg = IntPtr.Zero
		End If

		' GCHandleの解放
		If DelegateHandle.IsAllocated Then
			DelegateHandle.Free()
		End If

		'コントロールの開放
		'UPGRADE_NOTE: オブジェクト wm_OptionCtl をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		wm_OptionCtl = Nothing
	End Sub

	Public Function SubClassProc(ByVal hwndx As IntPtr, ByVal uMsg As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
		Static Calling As Boolean
		If Not Calling Then
			Calling = True
			'        Debug.Print Hex(hwndx), Hex(uMsg), Hex(wParam), Hex(lpOrg)
			'        Debug.Print "hwndx:[" & hwndx & "] uMsg:[" & uMsg & "] wParam:[" & wParam & "]"

			Select Case uMsg
				Case WM_KEYDOWN
					Select Case wParam
						Case System.Windows.Forms.Keys.Up
							Call keybd_event(System.Windows.Forms.Keys.ShiftKey, 0, 0, UIntPtr.Zero)
							Call keybd_event(System.Windows.Forms.Keys.Tab, 0, 0, UIntPtr.Zero)
							Call keybd_event(System.Windows.Forms.Keys.ShiftKey, 0, KEYEVENTF_KEYUP, UIntPtr.Zero)
							Call keybd_event(System.Windows.Forms.Keys.Tab, 0, KEYEVENTF_KEYUP, UIntPtr.Zero)
							'                        Debug.Print "WM_KEYUP"
						Case System.Windows.Forms.Keys.Down, System.Windows.Forms.Keys.Return
							Call PostMessage(wm_OptionCtl.Handle, WM_KEYDOWN, System.Windows.Forms.Keys.Tab, IntPtr.Zero)
							'                        uMsg = 0
							'                        wParam = 0
							'                        Debug.Print "WM_KEYdown"
					End Select
			End Select

			Calling = False
		End If
		SubClassProc = CallWindowProc(lpOrg, hwndx, CUInt(uMsg), wParam, lParam)
	End Function

End Class