Option Strict Off
Option Explicit On

Imports System.DirectoryServices.AccountManagement
Imports System.Management
Imports System.Runtime.InteropServices
Imports System.Text

''' <summary>
''' ---------------------------------------------------------
''' Ver.1.00           '2002.04.17
''' Ver.1.01           '2003.09.30  ShellExecuteEXを追加
'''                                 CloseHandleを追加
'''                                 WaitForSingleObjectを追加
''' Ver.1.02           '2004.10.28  GetIniにフラグを新設（AppPathを付けるor付けない)
''' Ver.1.03           '2005.02.03  GetIniの変数を1024→2048へ
''' Ver.1.04           '2014.08.25  GetIniの変数を2048→4096へ
''' Ver.1.05           '2020.04.04  GetFullName(表示名取得)を追加
''' ---------------------------------------------------------
''' </summary>
Public Module Win32API

	'----------[Window操作]
	<StructLayout(LayoutKind.Sequential)>
	Public Structure RECT
		Public Left As Integer
		Public Top As Integer
		Public Right As Integer
		Public Bottom As Integer
	End Structure

	'API宣言
	<DllImport("user32.dll", SetLastError:=True)>
	Public Function AdjustWindowRect(ByRef lpRect As RECT, ByVal dwStyle As Integer, ByVal bMenu As Integer) As Integer
	End Function

	<DllImport("user32.dll", SetLastError:=True)>
	Public Function MoveWindow(ByVal hwnd As IntPtr, ByVal X As Integer, ByVal Y As Integer, ByVal nWidth As Integer, ByVal nHeight As Integer, ByVal bRepaint As Integer) As Integer
	End Function

	<DllImport("user32.dll", SetLastError:=True)>
	Public Function GetWindowRect(ByVal hwnd As IntPtr, ByRef lpRect As RECT) As Integer
	End Function

	<DllImport("user32.dll", SetLastError:=True)>
	Public Function GetDesktopWindow() As IntPtr
	End Function

	<DllImport("kernel32.dll", SetLastError:=True)>
	Public Sub Sleep(ByVal dwMilliseconds As Integer)
	End Sub

	<DllImport("user32.dll", SetLastError:=True)>
	Public Sub keybd_event(ByVal bVk As Byte, ByVal bScan As Byte, ByVal dwFlags As Integer, ByVal dwExtraInfo As UIntPtr)
	End Sub

	<DllImport("advapi32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
	Public Function GetUserName(ByVal lpBuffer As StringBuilder, ByRef nSize As Integer) As Integer
	End Function

	<DllImport("kernel32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
	Public Function GetPrivateProfileString(ByVal lpAppName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As System.Text.StringBuilder, ByVal nSize As Integer, ByVal lpFileName As String) As Integer
	End Function

	<DllImport("kernel32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
	Public Function WritePrivateProfileString(ByVal lpAppName As String, ByVal lpKeyName As String, ByVal lpString As String, ByVal lpFileName As String) As Boolean
	End Function

	<DllImport("user32.dll", SetLastError:=True)>
	Public Function SetWindowPos(ByVal hwnd As IntPtr, ByVal hWndInsertAfter As IntPtr, ByVal X As Integer, ByVal Y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal uFlags As Integer) As Boolean
	End Function

	<DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
	Public Function FindWindow(ByVal lpClassName As String, ByVal lpWindowName As String) As IntPtr
	End Function

	<DllImport("user32.dll", SetLastError:=True)>
	Public Function GetLastActivePopup(ByVal hWndOwnder As IntPtr) As IntPtr
	End Function

	<DllImport("user32.dll", SetLastError:=True)>
	Public Function SetForegroundWindow(ByVal hwnd As IntPtr) As Integer
	End Function

	<DllImport("user32.dll", SetLastError:=True)>
	Public Function ShowWindow(ByVal hwnd As IntPtr, ByVal nCmdShow As Integer) As Integer
	End Function

	<DllImport("shell32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
	Public Function ShellExecute(ByVal hwnd As IntPtr, ByVal lpOperation As String, ByVal lpFile As String, ByVal lpParameters As String, ByVal lpDirectory As String, ByVal nShowCmd As Integer) As IntPtr
	End Function

	<DllImport("shell32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
	Public Function ShellExecuteEx(ByRef lpExecInfo As SHELLEXECUTEINFO) As Integer
	End Function

	<DllImport("kernel32.dll", SetLastError:=True)>
	Public Function CloseHandle(ByVal hObject As IntPtr) As Integer
	End Function

	<DllImport("kernel32.dll", SetLastError:=True)>
	Public Function WaitForSingleObject(ByVal hHandle As IntPtr, ByVal dwMilliseconds As Integer) As Integer
	End Function

	<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)>
	Public Structure SHELLEXECUTEINFO
		Public cbSize As Integer
		Public fMask As Integer
		Public hwnd As IntPtr
		<MarshalAs(UnmanagedType.LPTStr)> Public lpVerb As String
		<MarshalAs(UnmanagedType.LPTStr)> Public lpFile As String
		<MarshalAs(UnmanagedType.LPTStr)> Public lpParameters As String
		<MarshalAs(UnmanagedType.LPTStr)> Public lpDirectory As String
		Public nShow As Integer
		Public hInstApp As IntPtr
		' Optional fields
		Public lpIDList As IntPtr
		<MarshalAs(UnmanagedType.LPTStr)> Public lpClass As String
		Public hkeyClass As IntPtr
		Public dwHotKey As Integer
		Public hIcon As IntPtr
		Public hProcess As IntPtr
	End Structure

	'定数
	Public Const SW_SHOWNORMAL As Integer = 1
	Public Const SW_SHOWMINIMIZED As Integer = 2
	Public Const SW_SHOWMAXIMIZED As Integer = 3

	Public Const KEYEVENTF_KEYUP As Integer = &H2

	Public Const HWND_TOP As Integer = 0
	Public Const HWND_BOTTOM As Integer = 1
	Public Const HWND_TOPMOST As Integer = -1
	Public Const HWND_NOTOPMOST As Integer = -2

	Public Const SWP_SHOWWINDOW As Integer = &H40
	Public Const SWP_NOSIZE As Integer = &H1
	Public Const SWP_NOMOVE As Integer = &H2

	Public Const INFINITE As Integer = &HFFFF
	Public Const STATUS_WAIT_0 As Integer = 0
	Public Const STATUS_TIMEOUT As Integer = &H102
	Public Const STATUS_ABANDONED_WAIT_0 As Integer = &H80

	Public Const WAIT_TIMEOUT As Integer = &H102

	Public Const PROCESS_QUERY_INFORMATION As Integer = &H400
	Public Const STILL_ACTIVE As Integer = &H103

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

	Public Const ERROR_FILE_NOT_FOUND As Integer = 2
	Public Const ERROR_PATH_NOT_FOUND As Integer = 3
	Public Const ERROR_DDE_FAIL As Integer = 1156
	Public Const ERROR_NO_ASSOCIATION As Integer = 1155
	Public Const ERROR_ACCESS_DENIED As Integer = 5
	Public Const ERROR_DLL_NOT_FOUND As Integer = 1157
	Public Const ERROR_CANCELLED As Integer = 1223
	Public Const ERROR_NOT_ENOUGH_MEMORY As Integer = 8
	Public Const ERROR_SHARING_VIOLATION As Integer = 32

	'動作環境定義ファイル名
	Public INIFile As String

	''' <summary>
	''' INIファイルより読み込み
	''' </summary>
	''' <param name="Section"></param>
	''' <param name="Key"></param>
	''' <param name="INIFile"></param>
	''' <param name="AddAppPath"></param>
	''' <returns></returns>
	Public Function GetIni(ByVal Section As String, ByVal Key As String, ByVal INIFile As String, Optional ByVal AddAppPath As Boolean = True) As String
		Dim sb As New StringBuilder(4096)
		Dim ret As Integer

		If AddAppPath Then
			ret = GetPrivateProfileString(Section, Key, "", sb, sb.Capacity, AppPath(INIFile))
		Else
			ret = GetPrivateProfileString(Section, Key, "", sb, sb.Capacity, INIFile)
		End If
		Return UniAsczToStr(sb.ToString())
	End Function

	''' <summary>
	''' INIファイルに書き込み
	''' </summary>
	''' <param name="Section"></param>
	''' <param name="Key"></param>
	''' <param name="Str"></param>
	''' <param name="INIFile"></param>
	''' <returns></returns>
	Public Function WriteIni(ByVal Section As String, ByVal Key As String, ByVal Str As String, ByVal INIFile As String) As Boolean
		Return WritePrivateProfileString(Section, Key, Str, AppPath(INIFile))
	End Function

	''' <summary>
	''' Returnキー送信
	''' </summary>
	Public Sub SendReturnKey()
		keybd_event(CByte(Keys.Return), 0, 0, UIntPtr.Zero)
		keybd_event(CByte(Keys.Return), 0, KEYEVENTF_KEYUP, UIntPtr.Zero)
	End Sub

	''' <summary>
	''' TABキー送信
	''' </summary>
	Public Sub SendTabKey()
		keybd_event(CByte(Keys.Tab), 0, 0, UIntPtr.Zero)
		keybd_event(CByte(Keys.Tab), 0, KEYEVENTF_KEYUP, UIntPtr.Zero)
	End Sub

	''' <summary>
	''' Shift+TABキー送信
	''' </summary>
	Public Sub SendSHTabKey()
		keybd_event(CByte(Keys.ShiftKey), 0, 0, UIntPtr.Zero)
		keybd_event(CByte(Keys.Tab), 0, 0, UIntPtr.Zero)
		keybd_event(CByte(Keys.ShiftKey), 0, KEYEVENTF_KEYUP, UIntPtr.Zero)
		keybd_event(CByte(Keys.Tab), 0, KEYEVENTF_KEYUP, UIntPtr.Zero)
	End Sub

	''' <summary>
	''' NULL文字を除去する関数
	''' </summary>
	''' <param name="Buf"></param>
	''' <returns></returns>
	Public Function UniAsczToStr(ByVal Buf As String) As String
		Dim i As Integer = Buf.IndexOf(Chr(0))
		If i >= 0 Then
			Return Buf.Substring(0, i)
		Else
			Return Buf
		End If
	End Function

	''' <summary>
	''' フォームを常に手前に表示
	''' </summary>
	''' <param name="frmObject"></param>
	''' <param name="Optn"></param>
	''' <returns></returns>
	Public Function SetFormTop(ByVal frmObject As Form, ByVal Optn As Integer) As Boolean
		Dim objHwnd As IntPtr = frmObject.Handle

		If Optn = 0 Then
			Return SetWindowPos(objHwnd, New IntPtr(HWND_NOTOPMOST), 0, 0, 0, 0, SWP_SHOWWINDOW Or SWP_NOMOVE Or SWP_NOSIZE)
		Else
			Return SetWindowPos(objHwnd, New IntPtr(HWND_TOPMOST), 0, 0, 0, 0, SWP_SHOWWINDOW Or SWP_NOMOVE Or SWP_NOSIZE)
		End If
	End Function

	''' <summary>
	''' ユーザー名を取得
	''' </summary>
	''' <returns></returns>
	Public Function GetUName() As String
		Dim UName As New StringBuilder(255)
		GetUserName(UName, CInt(UName.Capacity))
		Return UName.ToString().TrimEnd(Chr(0))
	End Function

	''' <summary>
	''' ユーザーのフルネームを取得
	''' </summary>
	''' <returns></returns>
	Public Function GetFullName() As String
		Dim fullName As String

		' Active Directory 環境で取得
		fullName = GetADFullName()
		If fullName <> "取得不可" AndAlso fullName <> "取得エラー" Then
			Return fullName
		End If

		' ローカルアカウントとして取得
		fullName = GetLocalFullName()
		If fullName <> "取得不可" AndAlso fullName <> "取得エラー" Then
			Return fullName
		End If

		' NetWorkアカウントとして取得
		fullName = GetNetworkFullName()
		Return fullName
	End Function

	''' <summary>
	''' ドメインコンテキストからユーザー情報を取得
	''' </summary>
	''' <returns></returns>
	Public Function GetADFullName() As String
		Dim fullName As String = "取得不可"

		Try
			' 現在のドメインコンテキストからユーザー情報を取得
			Using context As New PrincipalContext(ContextType.Domain)
				Dim userName As String = Environment.UserName
				Using user As UserPrincipal = UserPrincipal.FindByIdentity(context, userName)
					If user IsNot Nothing AndAlso Not String.IsNullOrEmpty(user.DisplayName) Then
						fullName = user.DisplayName
					End If
				End Using
			End Using
		Catch ex As Exception
			fullName = "取得エラー"
		End Try

		Return fullName
	End Function

	''' <summary>
	''' WMI 経由で Win32_UserAccount からユーザー情報を取得
	''' </summary>
	''' <returns></returns>
	Public Function GetLocalFullName() As String
		Dim fullName As String = "取得不可"

		Try
			Dim userName As String = Environment.UserName

			' WMI 経由で Win32_UserAccount からフルネームを取得
			Dim searcher As New ManagementObjectSearcher("SELECT * FROM Win32_UserAccount WHERE Name = '" & userName & "' AND LocalAccount = TRUE")

			For Each obj As ManagementObject In searcher.Get()
				If obj("FullName") IsNot Nothing AndAlso obj("FullName").ToString().Trim() <> "" Then
					fullName = obj("FullName").ToString()
					Exit For
				End If
			Next
		Catch ex As Exception
			fullName = "取得エラー"
		End Try

		Return fullName
	End Function

	''' <summary>
	''' NetWorkアカウントからユーザー情報を取得
	''' </summary>
	''' <returns></returns>
	Public Function GetNetworkFullName() As String
		Dim FullName As String = "取得不可"

		Try
			Dim Locator As Object = CreateObject("WbemScripting.SWbemLocator")
			Dim Service As Object = Locator.ConnectServer()
			Dim QfeSet As Object = Service.ExecQuery("Select * From Win32_NetworkLoginProfile")

			Dim UserName As String = CreateObject("WScript.Network").UserName

			For Each Qfe As Object In QfeSet
				If Qfe.Name.Contains(UserName) Then
					If Not IsDBNull(Qfe.FullName) AndAlso Qfe.FullName.ToString().Trim() <> "" Then
						FullName = Qfe.FullName
					End If
				End If
			Next
		Catch ex As Exception
			FullName = "取得エラー"
		End Try

		Return FullName
	End Function

End Module
