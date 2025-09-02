Option Strict Off
Option Explicit On
Module Win32API
	'---------------------------------------------------------
	'Ver.1.00           '2002.04.17
	'Ver.1.01           '2003.09.30  ShellExecuteEXを追加
	'                                CloseHandleを追加
	'                                WaitForSingleObjectを追加
	'Ver.1.02           '2004.10.28  GetIniにフラグを新設（AppPathを付けるor付けない)
	'Ver.1.03           '2005.02.03  GetIniの変数を1024→2048へ
	'Ver.1.04           '2014.08.25  GetIniの変数を2048→4096へ
	'Ver.1.05           '2020.04.04  GetFullName(表示名取得)を追加
	'---------------------------------------------------------
	
	'----------[Window操作]
	Structure RECT
		'UPGRADE_NOTE: Left は Left_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
		Dim Left_Renamed As Integer
		Dim Top As Integer
		'UPGRADE_NOTE: Right は Right_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
		Dim Right_Renamed As Integer
		Dim Bottom As Integer
	End Structure
	
	'UPGRADE_WARNING: 構造体 RECT に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
	Declare Function AdjustWindowRect Lib "user32" (ByRef lpRect As RECT, ByVal dwStyle As Integer, ByVal bMenu As Integer) As Integer
	
	Declare Function MoveWindow Lib "user32" (ByVal Hwnd As Integer, ByVal X As Integer, ByVal Y As Integer, ByVal nWidth As Integer, ByVal nHeight As Integer, ByVal bRepaint As Integer) As Integer
	'UPGRADE_WARNING: 構造体 RECT に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
	Declare Function GetWindowRect Lib "user32" (ByVal Hwnd As Integer, ByRef lpRect As RECT) As Integer
	Declare Function GetDesktopWindow Lib "user32" () As Integer
	
	Public Const SW_SHOWNORMAL As Short = 1
	Public Const SW_SHOWMINIMIZED As Short = 2
	Public Const SW_SHOWMAXIMIZED As Short = 3
	
	'Declare Function ShowWindow Lib "user32" (ByVal hwnd As Long, ByVal nCmdShow As Long) As Long
	
	'Sleep
	Declare Sub Sleep Lib "kernel32" (ByVal dwMilliseconds As Integer)
	
	
	'リターンキー制御
	'キーボードイベントでタブキーを送る
	Public Declare Sub keybd_event Lib "user32" (ByVal bVk As Byte, ByVal bScan As Byte, ByVal dwFlags As Integer, ByVal dwExtraInfo As Integer)
	
	Public Const KEYEVENTF_KEYUP As Integer = &H2
	
	'ログオンユーザー名
	Declare Function GetUserName Lib "advapi32.dll"  Alias "GetUserNameA"(ByVal lpBuffer As String, ByRef nSize As Integer) As Integer
	
	
	'動作環境定義ファイル名
	Public INIFile As String
	
	'ＩＮＩファイルより読み込み(API)
	'UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
	Declare Function GetPrivateProfileString Lib "kernel32"  Alias "GetPrivateProfileStringA"(ByVal lpApplicationName As String, ByVal lpKeyName As Any, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Integer, ByVal lpFileName As String) As Integer
	
	'ＩＮＩファイルに書き込み(API)
	'UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
	'UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
	Declare Function WritePrivateProfileString Lib "kernel32"  Alias "WritePrivateProfileStringA"(ByVal lpApplicationName As String, ByVal lpKeyName As Any, ByVal lpString As Any, ByVal lpFileName As String) As Integer
	
	'Windowの位置やサイズ、表示を設定
	Public Declare Function SetWindowPos Lib "user32" (ByVal Hwnd As Integer, ByVal hWndInsertAfter As Integer, ByVal X As Integer, ByVal Y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal wFlags As Integer) As Integer
	
	Public Const HWND_TOP As Short = 0 '手前にセット
	Public Const HWND_BOTTOM As Short = 1 '後ろにセット
	Public Const HWND_TOPMOST As Short = -1 '常に手前にセット
	Public Const HWND_NOTOPMOST As Short = -2 '常に手前、解除
	
	Public Const SWP_SHOWWINDOW As Integer = &H40 '表示
	Public Const SWP_NOSIZE As Integer = &H1 'サイズ設定なし
	Public Const SWP_NOMOVE As Integer = &H2 '位置設定なし
	
	'指定されたクラス名とウィンドウ名を持つトップレベルウィンドウを探す。
	Public Declare Function FindWindow Lib "user32"  Alias "FindWindowA"(ByVal lpClassName As String, ByVal lpWindowName As String) As Integer
	'指定されたウィンドウが所有するポップアップウィンドウのうち、
	'直前にアクティブであったウィンドウを調べる。
	Public Declare Function GetLastActivePopup Lib "user32" (ByVal hWndOwnder As Integer) As Integer
	'指定されたウィンドウを作成したスレッドをフォアグラウンドにし、そのウィンドウをアクティブにする。
	Public Declare Function SetForegroundWindow Lib "user32" (ByVal Hwnd As Integer) As Integer
	'指定されたウィンドウの表示状態を設定する。
	Public Declare Function ShowWindow Lib "user32" (ByVal Hwnd As Integer, ByVal nCmdShow As Integer) As Integer
	'Public Const SW_SHOWNORMAL = 1
	Public Const SW_MAXIMAIZE As Short = 3
	Public Const SW_SHOW As Short = 5
	Public Const SW_RESTORE As Short = 9
	
	Public Declare Function ShellExecute Lib "shell32.dll"  Alias "ShellExecuteA"(ByVal Hwnd As Integer, ByVal lpOperation As String, ByVal lpFile As String, ByVal lpParameters As String, ByVal lpDirectory As String, ByVal nShowCmd As Integer) As Integer
	
	'ShellExecuteEX
	Public Const SEE_MASK_CLASSKEY As Integer = &H3
	Public Const SEE_MASK_CLASSNAME As Integer = &H1
	Public Const SEE_MASK_CONNECTNETDRV As Integer = &H80
	Public Const SEE_MASK_DOENVSUBST As Integer = &H200
	Public Const SEE_MASK_FLAG_DDEWAIT As Integer = &H100
	Public Const SEE_MASK_FLAG_NO_UI As Integer = &H400
	Public Const SEE_MASK_HOTKEY As Integer = &H20
	Public Const SEE_MASK_ICON As Integer = &H10
	Public Const SEE_MASK_IDLIST As Integer = &H4
	Public Const SEE_MASK_INVOKEIDLIST As Integer = &HC
	Public Const SEE_MASK_NOCLOSEPROCESS As Integer = &H40
	
	Public Structure SHELLEXECUTEINFO
		Dim cbSize As Integer
		Dim fMask As Integer
		Dim Hwnd As Integer
		Dim lpVerb As String
		Dim lpFile As String
		Dim lpParameters As String
		Dim lpDirectory As String
		Dim nShow As Integer
		Dim hInstApp As Integer
		'  Optional fields
		Dim lpIDList As Integer
		Dim lpClass As String
		Dim hkeyClass As Integer
		Dim dwHotKey As Integer
		Dim hIcon As Integer
		Dim hProcess As Integer
	End Structure
	
	'UPGRADE_WARNING: 構造体 SHELLEXECUTEINFO に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
	Public Declare Function ShellExecuteEx Lib "shell32.dll"  Alias "ShellExecuteExA"(ByRef lpExecInfo As SHELLEXECUTEINFO) As Integer
	
	Public Const ERROR_FILE_NOT_FOUND As Short = 2
	Public Const ERROR_PATH_NOT_FOUND As Short = 3
	Public Const ERROR_DDE_FAIL As Short = 1156
	Public Const ERROR_NO_ASSOCIATION As Short = 1155
	Public Const ERROR_ACCESS_DENIED As Short = 5
	Public Const ERROR_DLL_NOT_FOUND As Short = 1157
	Public Const ERROR_CANCELLED As Short = 1223
	Public Const ERROR_NOT_ENOUGH_MEMORY As Short = 8
	Public Const ERROR_SHARING_VIOLATION As Short = 32
	
	Public Declare Function CloseHandle Lib "kernel32" (ByVal hObject As Integer) As Integer
	
	'起動したアプリを待つ
	Public Declare Function WaitForSingleObject Lib "kernel32" (ByVal hHandle As Integer, ByVal dwMilliseconds As Integer) As Integer
	
	Public Const INFINITE As Integer = &HFFFF
	Public Const STATUS_WAIT_0 As Short = 0
	Public Const STATUS_TIMEOUT As Integer = &H102
	Public Const STATUS_ABANDONED_WAIT_0 As Integer = &H80
	
	Public Const WAIT_TIMEOUT As Integer = &H102
	
	'定数
	Const PROCESS_QUERY_INFORMATION As Integer = &H400
	Const STILL_ACTIVE As Integer = &H103
	
	'INIファイルより読み込み
	Public Function GetIni(ByRef Section As String, ByRef Key As String, ByRef INIFile As String, Optional ByRef AddAppPath As Boolean = True) As Object
		'''''Public Function GetIni(Section As String, Key As String, INIFile As String)
		
		'    Dim w As String * 2048
		Dim W As New VB6.FixedLengthString(4096)
		Dim ret As Integer
		
		If AddAppPath = True Then '2004/10/28
			'アプリケーションのパスをくっつける
			'''        ret = GetPrivateProfileString(Section, Key, vbNullString, w, 2048, AppPath(INIFile))
			ret = GetPrivateProfileString(Section, Key, vbNullString, W.Value, 4096, AppPath(INIFile))
		Else
			'フルパス指定
			'''        ret = GetPrivateProfileString(Section, Key, vbNullString, w, 2048, INIFile)
			ret = GetPrivateProfileString(Section, Key, vbNullString, W.Value, 4096, INIFile)
		End If
		GetIni = UniAsczToStr(W.Value)
		
	End Function
	
	'INIファイルに書き込み
	'UPGRADE_NOTE: Str は Str_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
	Public Function WriteIni(ByRef Section As String, ByRef Key As String, ByRef Str_Renamed As String, ByRef INIFile As String) As Object
		
		Dim ret As Integer
		
		ret = WritePrivateProfileString(Section, Key, Str_Renamed, AppPath(INIFile))
		
	End Function
	
	'sendkeys {Return}の変わりのAPI
	Public Sub SendReturnKey()
		
		Call keybd_event(System.Windows.Forms.Keys.Return, 0, 0, 0)
		Call keybd_event(System.Windows.Forms.Keys.Return, 0, KEYEVENTF_KEYUP, 0)
		
	End Sub
	'sendkeys {TAB}の変わりのAPI
	Public Sub SendTabKey()
		
		Call keybd_event(System.Windows.Forms.Keys.Tab, 0, 0, 0)
		Call keybd_event(System.Windows.Forms.Keys.Tab, 0, KEYEVENTF_KEYUP, 0)
		
	End Sub
	
	'sendkeys {+TAB}の変わりのAPI
	Public Sub SendSHTabKey()
		
		Call keybd_event(System.Windows.Forms.Keys.ShiftKey, 0, 0, 0)
		Call keybd_event(System.Windows.Forms.Keys.Tab, 0, 0, 0)
		Call keybd_event(System.Windows.Forms.Keys.ShiftKey, 0, KEYEVENTF_KEYUP, 0)
		Call keybd_event(System.Windows.Forms.Keys.Tab, 0, KEYEVENTF_KEYUP, 0)
		
	End Sub
	
	'***********************************************************
	'機能  ：引数 bufの文字列中の NULLコードを検索し、NULLコードを
	'       除いた文字列を返す
	'引数  ： Buf = NULLコードを含む文字列
	'戻り値：NULLコードを除いた文字列
	'***********************************************************
	Public Function UniAsczToStr(ByRef Buf As String) As String
		Dim i As Integer
		
		i = InStr(Buf, vbNullChar)
		If (i <> 0) Then
			UniAsczToStr = Left(Buf, i - 1)
		Else
			UniAsczToStr = Buf
		End If
		
	End Function
	
	'-----------------------------------------------------------------------------------------------------
	'    lRet = Set_FormTop(Form1, 1)
	'
	Public Function SetFormTop(ByRef frmObject As System.Windows.Forms.Form, ByRef Optn As Short) As Integer
		
		Dim objHwnd As Integer
		
		objHwnd = frmObject.Handle.ToInt32
		
		If Optn = 0 Then
			'常に手前に表示を解除
			SetFormTop = SetWindowPos(objHwnd, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_SHOWWINDOW Or SWP_NOMOVE Or SWP_NOSIZE)
		Else
			'常に手前に表示にセット
			SetFormTop = SetWindowPos(objHwnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_SHOWWINDOW Or SWP_NOMOVE Or SWP_NOSIZE)
		End If
		
	End Function
	
	Public Function GetUName() As String
		'ユーザー名を取得する関数
		Dim UName As String
		
		UName = Space(255)
		
		Call GetUserName(UName, Len(UName))
		
		GetUName = Left(UName, InStr(1, UName, Chr(0)) - 1)
		
	End Function
	
	Public Function GetFullName() As String
		'ユーザのフルネーム（表示名）を取得
		Dim Locator As Object
		Dim Service As Object
		Dim QfeSet As Object
		Dim Qfe As Object
		
		Dim UserName As String
		Dim FullName As String
		
		'初期値
		FullName = "取得不可"
		
		Locator = CreateObject("WbemScripting.SWbemLocator")
		'UPGRADE_WARNING: オブジェクト Locator.ConnectServer の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Service = Locator.ConnectServer
		'UPGRADE_WARNING: オブジェクト Service.ExecQuery の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		QfeSet = Service.ExecQuery("Select * From Win32_NetworkLoginProfile")
		
		'UPGRADE_WARNING: オブジェクト CreateObject().UserName の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		UserName = CreateObject("WScript.Network").UserName
		
		For	Each Qfe In QfeSet
			'UPGRADE_WARNING: オブジェクト Qfe.Name の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If 0 < InStr(Qfe.Name, UserName) Then
				'UPGRADE_WARNING: オブジェクト Qfe.FullName の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				FullName = Qfe.FullName
			End If
		Next Qfe
		
		GetFullName = FullName
	End Function
End Module