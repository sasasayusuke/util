Option Strict On
Option Explicit On

Imports System.Runtime.InteropServices
Imports System.Text

''' <summary>
''' Ver.1.00           '2003.04.09
''' </summary>
Public Module Win32API_GetInfo

	' コンピュータ名を取得する関数の宣言
	<DllImport("kernel32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
	Public Function GetComputerName(ByVal lpBuffer As StringBuilder, ByRef nSize As Integer) As Boolean
	End Function

	' コンピュータ名の長さを示す定数の宣言
	Public Const MAX_COMPUTERNAME_LENGTH As Integer = 15

	''' <summary>
	''' コンピュータ名を取得する関数
	''' </summary>
	''' <returns></returns>
	Public Function GetPCName() As String
		Dim strComputerNameBuffer As New StringBuilder(MAX_COMPUTERNAME_LENGTH + 1)
		Dim lngComputerNameLength As Integer = strComputerNameBuffer.Capacity
		Dim lngWin32apiResultCode As Boolean

		' コンピュータ名を取得
		lngWin32apiResultCode = GetComputerName(strComputerNameBuffer, lngComputerNameLength)

		If lngWin32apiResultCode Then
			' コンピュータ名を返す
			Return strComputerNameBuffer.ToString()
		Else
			' エラーが発生した場合は空の文字列を返す
			Return String.Empty
		End If
	End Function

End Module
