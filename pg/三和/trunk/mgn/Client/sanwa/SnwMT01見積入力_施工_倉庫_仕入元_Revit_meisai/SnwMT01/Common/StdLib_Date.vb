Option Strict Off
Option Explicit On
Module StdLib_Date
	'Ver.1.00           '?
	'Ver.1.01           '2003.10.07 GetFirstDateOfScope･GetLastDateOfScope修正
	
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
	'UPGRADE_NOTE: Default は Default_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
	Public Function GetFirstDateOfScope(ByRef wdate As Date, ByRef wshime As Short, Optional ByRef Default_Renamed As String = vbNullString) As String
		Dim BufDate As String
		
		Select Case wshime
			Case 0
				GetFirstDateOfScope = CStr(wdate)
			Case 99
				'            開始日付算出 = wdate
				GetFirstDateOfScope = VB6.Format(wdate, "yyyy/mm") & "/01"
			Case Else
				If DatePart(Microsoft.VisualBasic.DateInterval.Day, wdate) > wshime Then
					BufDate = VB6.Format(wdate, "yyyy/mm") & "/" & wshime
					If IsDate(BufDate) Then
						GetFirstDateOfScope = VB6.Format(DateAdd(Microsoft.VisualBasic.DateInterval.Day, 1, CDate(BufDate)), "yyyy\/mm\/dd")
					Else
						GetFirstDateOfScope = Default_Renamed
					End If
				Else
					BufDate = VB6.Format(wdate, "yyyy/mm") & "/" & wshime
					If IsDate(BufDate) Then
						GetFirstDateOfScope = CStr(DateAdd(Microsoft.VisualBasic.DateInterval.Month, -1, DateAdd(Microsoft.VisualBasic.DateInterval.Day, 1, CDate(BufDate))))
					Else
						GetFirstDateOfScope = Default_Renamed
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
	'UPGRADE_NOTE: Default は Default_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
	Public Function GetLastDateOfScope(ByRef wdate As Date, ByRef wshime As Short, Optional ByRef Default_Renamed As String = vbNullString) As String
		Dim BufDate As String
		
		Select Case wshime
			Case 0
				GetLastDateOfScope = CStr(wdate)
			Case 99
				GetLastDateOfScope = CStr(DateAdd(Microsoft.VisualBasic.DateInterval.Day, -1, CDate(VB6.Format(DateAdd(Microsoft.VisualBasic.DateInterval.Month, 1, wdate), "yyyy/mm") & "/01")))
			Case Else
				If DatePart(Microsoft.VisualBasic.DateInterval.Day, wdate) > wshime Then
					BufDate = VB6.Format(wdate, "yyyy/mm") & "/" & wshime
					If IsDate(BufDate) Then
						GetLastDateOfScope = CStr(DateAdd(Microsoft.VisualBasic.DateInterval.Month, 1, CDate(BufDate)))
					Else
						GetLastDateOfScope = Default_Renamed
					End If
				Else
					BufDate = VB6.Format(wdate, "yyyy/mm") & "/" & wshime
					If IsDate(BufDate) Then
						GetLastDateOfScope = BufDate
					Else
						GetLastDateOfScope = Default_Renamed
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
	Public Function OutputDateYMDFromFlatText(ByRef sStr As String, Optional ByRef YearLen As Short = 4, Optional ByRef MonthLen As Short = 2) As String
		Dim Buf As String
		
		If AnsiLenB(sStr) > 8 Then Exit Function
		If AnsiLenB(sStr) = 0 Then Exit Function
		If sStr = vbNullString Then Exit Function
		
		'UPGRADE_WARNING: オブジェクト AnsiMidB(sStr, YearLen + MonthLen + 1) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト AnsiMidB(sStr, YearLen + 1, MonthLen) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト AnsiMidB() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Buf = AnsiMidB(sStr, 1, YearLen) & "/" & AnsiMidB(sStr, YearLen + 1, MonthLen) & "/" & AnsiMidB(sStr, YearLen + MonthLen + 1)
		
		If IsDate(Buf) Then
			OutputDateYMDFromFlatText = Buf
		Else
			OutputDateYMDFromFlatText = vbNullString
		End If
	End Function
End Module