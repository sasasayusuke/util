Option Strict Off
Option Explicit On
Module Win32API_GetInfo
	'Ver.1.00           '2003.04.09
	
	' コンピュータ名を取得する関数の宣言
	Declare Function GetComputerName Lib "kernel32.dll"  Alias "GetComputerNameA"(ByVal lpBuffer As String, ByRef nSize As Integer) As Integer
	
	' コンピュータ名の長さを示す定数の宣言
	Public Const MAX_COMPUTERNAME_LENGTH As Short = 127
	
	' コンピュータ名を取得するバッファの長さを示す定数の宣言
	Public Const COMPUTERNAMBUFFER_LENGTH As Integer = MAX_COMPUTERNAME_LENGTH + 1
	
	'INIファイルより読み込み
	Public Function GetPCName() As String
		Dim strComputerNameBuffer As New VB6.FixedLengthString(COMPUTERNAMBUFFER_LENGTH)
		Dim lngComputerNameLength As Integer
		Dim lngWin32apiResultCode As Integer
		
		' コンピュータ名の長さを設定
		lngComputerNameLength = Len(strComputerNameBuffer.Value)
		' コンピュータ名を取得
		lngWin32apiResultCode = GetComputerName(strComputerNameBuffer.Value, lngComputerNameLength)
		' コンピュータ名を表示
		GetPCName = Left(strComputerNameBuffer.Value, InStr(strComputerNameBuffer.Value, vbNullChar) - 1)
	End Function
End Module