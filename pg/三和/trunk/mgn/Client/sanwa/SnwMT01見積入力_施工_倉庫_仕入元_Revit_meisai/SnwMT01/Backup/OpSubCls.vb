Option Strict Off
Option Explicit On
Module OpSubCls
	'Ver.1.00           '2003.09.25 OptionButtonのカーソル制御の為作成
	
	'UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
	Private Declare Function PostMessage Lib "user32"  Alias "PostMessageA"(ByVal Hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer, ByRef lParam As Any) As Integer
	Private Const WM_KEYDOWN As Integer = &H100
	'Private Const WM_KEYUP = &H101
	Private Const VK_TAB As Integer = &H9
	
	Private Declare Sub keybd_event Lib "user32" (ByVal bVk As Byte, ByVal bScan As Byte, ByVal dwFlags As Integer, ByVal dwExtraInfo As Integer)
	Private Const KEYEVENTF_KEYUP As Integer = &H2
	'''Private Declare Function SendMessage Lib "user32" Alias "SendMessageA" (ByVal Hwnd As Long, ByVal wMsg As Long, ByVal wParam As Long, lParam As Any) As Long
	Private Declare Function SetWindowLong Lib "user32"  Alias "SetWindowLongA"(ByVal Hwnd As Integer, ByVal nindex As Integer, ByVal dwNewLong As Integer) As Integer
	
	Private Declare Function CallWindowProc Lib "user32"  Alias "CallWindowProcA"(ByVal lpPrevWndFunc As Integer, ByVal Hwnd As Integer, ByVal msg As Integer, ByVal wParam As Integer, ByRef lParam As Integer) As Integer
	
	Private Const GWL_WNDPROC As Short = (-4)
	
	Dim wm_OptionCtl As System.Windows.Forms.Control 'ホイール制御を行うコントロール用
	
	Private lpOrg As Integer '古いｳｨﾝﾄﾞｳﾌﾟﾛｼｰｼﾞｬのｱﾄﾞﾚｽが入る。
	
	Public Sub StartOption(ByRef ctl As System.Windows.Forms.Control)
		wm_OptionCtl = ctl
		'フックの開始
		'UPGRADE_WARNING: AddressOf SubClassProc の delegate を追加する 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="E9E157F7-EF0C-4016-87B7-7D7FBBC6EE08"' をクリックしてください。
		lpOrg = SetWindowLong(wm_OptionCtl.Handle.ToInt32, GWL_WNDPROC, AddressOf SubClassProc)
	End Sub
	
	Public Sub EndOption()
		If lpOrg <> 0 Then
			'フックの終了
			SetWindowLong(wm_OptionCtl.Handle.ToInt32, GWL_WNDPROC, lpOrg)
			lpOrg = 0
		End If
		'コントロールの開放
		'UPGRADE_NOTE: オブジェクト wm_OptionCtl をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		wm_OptionCtl = Nothing
	End Sub
	
	Public Function SubClassProc(ByVal hwndx As Integer, ByVal uMsg As Integer, ByVal wParam As Integer, ByRef lParam As Integer) As Integer
		Static Calling As Boolean
		If Not Calling Then
			Calling = True
			'''''        Debug.Print Hex(hwndx), Hex(uMsg), Hex(wParam), Hex(lpOrg)
			'        Debug.Print "hwndx:[" & hwndx & "] uMsg:[" & uMsg & "] wParam:[" & wParam & "]"
			
			Select Case uMsg
				Case WM_KEYDOWN
					Select Case wParam
						Case System.Windows.Forms.Keys.Up
							Call keybd_event(System.Windows.Forms.Keys.ShiftKey, 0, 0, 0)
							Call keybd_event(System.Windows.Forms.Keys.Tab, 0, 0, 0)
							Call keybd_event(System.Windows.Forms.Keys.ShiftKey, 0, KEYEVENTF_KEYUP, 0)
							Call keybd_event(System.Windows.Forms.Keys.Tab, 0, KEYEVENTF_KEYUP, 0)
							'                        Debug.Print "WM_KEYUP"
						Case System.Windows.Forms.Keys.Down, System.Windows.Forms.Keys.Return
							Call PostMessage(wm_OptionCtl.Handle.ToInt32, WM_KEYDOWN, System.Windows.Forms.Keys.Tab, 0)
							''                        uMsg = 0
							''                        wParam = 0
							'                        Debug.Print "WM_KEYdown"
					End Select
			End Select
			
			Calling = False
		End If
		SubClassProc = CallWindowProc(lpOrg, hwndx, uMsg, wParam, lParam)
	End Function
End Module