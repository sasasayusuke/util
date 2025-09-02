Option Strict On
Option Explicit On

Imports System.Globalization
Imports System.Runtime.InteropServices

''' <summary>
''' Ver.1.00           '?
''' Ver.1.01           '2003.10.07 GetFirstDateOfScope･GetLastDateOfScope修正
''' </summary>

Public Module StdLib_Date

	'*******************************************************************************************
	'関数名     開始日付算出
	'機能       日付と締日で開始日付を判断する
	'引数       wshime         '得意先締日
	'           wdate          '入力された年月
	'例
	'           1999/04    or 20 = 99/03/21
	'           1999/04    or 99 = 99/04/01
	'           1999/04/20 or 20 = 99/03/21
	'           1999/04/21 or 20 = 99/04/21
	'
	'メモ       得意先の締日は何でも入力可能な為このルーチンは使えない
	'
	'参照       dateadd("d",1,dateadd("m",-1,"2003/02/28"))…03/01/29 ×
	'           dateadd("m",-1,dateadd("d",1,"2003/02/28"))…03/02/01 ○
	'*******************************************************************************************
	Public Function GetFirstDateOfScope(wdate As Date, wshime As Integer, Optional ByVal optDefault As String = "") As String
		Dim BufDate As String

		Select Case wshime
			Case 0
				Return wdate.ToString("yyyy/MM/dd")
			Case 99
				Return New DateTime(wdate.Year, wdate.Month, 1).ToString("yyyy/MM/dd")
			Case Else
				If wdate.Day > wshime Then
					BufDate = New DateTime(wdate.Year, wdate.Month, wshime).ToString("yyyy/MM/dd")
					If Date.TryParse(BufDate, Nothing) Then
						Return DateAdd("d", 1, BufDate).ToString("yyyy/MM/dd")
					Else
						Return optDefault
					End If
				Else
					BufDate = New DateTime(wdate.Year, wdate.Month, wshime).ToString("yyyy/MM/dd")
					If Date.TryParse(BufDate, Nothing) Then
						Return DateAdd("m", -1, DateAdd("d", 1, BufDate)).ToString("yyyy/MM/dd")
					Else
						Return optDefault
					End If
				End If
		End Select

	End Function

	'*******************************************************************************************
	'関数名     終了日付算出
	'機能       日付と締日で終了日付を判断する
	'引数       wshime         '得意先締日
	'           wdate          '入力された年月
	'例
	'           1999/04     or 20 = 1999/4/20
	'           1999/04     or 99 = 1999/4/30
	'           1999/04/20  or 20 = 1999/4/20
	'           1999/04/20  or 99 = 1999/4/30
	'
	'メモ       得意先の締日は何でも入力可能な為このルーチンは使えない
	'*******************************************************************************************
	Public Function GetLastDateOfScope(wdate As Date, wshime As Integer, Optional ByVal optDefault As String = "") As String
		Dim BufDate As String

		Select Case wshime
			Case 0
				Return wdate.ToString("yyyy/MM/dd")
			Case 99
				Return DateAdd("d", -1, New DateTime(wdate.Year, wdate.Month, 1).AddMonths(1)).ToString("yyyy/MM/dd")
			Case Else
				If wdate.Day > wshime Then
					BufDate = New DateTime(wdate.Year, wdate.Month, wshime).ToString("yyyy/MM/dd")
					If Date.TryParse(BufDate, Nothing) Then
						Return DateAdd("m", 1, BufDate).ToString("yyyy/MM/dd")
					Else
						Return optDefault
					End If
				Else
					BufDate = New DateTime(wdate.Year, wdate.Month, wshime).ToString("yyyy/MM/dd")
					If Date.TryParse(BufDate, Nothing) Then
						Return BufDate
					Else
						Return optDefault
					End If
				End If
		End Select

	End Function

	'*******************************************************************************************
	'           日付項目を日付型に変換する
	'*******************************************************************************************
	' 書式          OutputDateYMDFromFlatText(sStr,[YearLen],)
	'  値           sStr    ：日付項目
	'               YearLen ：日付項目の年の桁数
	'               MonthLen：日付項目の月の桁数
	'
	'  戻り値       /がついた形での日付
	'               日付にならない場合、""を返す
	'*******************************************************************************************
	Public Function OutputDateYMDFromFlatText(sStr As String, Optional YearLen As Integer = 4, Optional MonthLen As Integer = 2) As String
	    Dim Buf As String

	    If sStr.Length > 8 Then Return ""
	    If sStr.Length = 0 Then Return ""
	    If sStr = "" Then Return ""

	    Try
	        Buf = sStr.Substring(0, YearLen) & "/" & sStr.Substring(YearLen, MonthLen) & "/" & sStr.Substring(YearLen + MonthLen)
	    Catch ex As Exception
	        Return ""
	    End Try

	    If Date.TryParse(Buf, Nothing) Then
	        Return Buf
	    Else
	        Return ""
	    End If
	End Function

End Module
