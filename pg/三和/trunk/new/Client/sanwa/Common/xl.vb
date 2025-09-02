Option Strict On
Option Explicit On

Imports Microsoft.Office.Interop.Excel
''' <summary>
''' Ver.1.01           '2004.02.05     xlOpenBookに読取専用属性をセットできるようにする。
''' Ver.1.02           '2014.08.09     xlOpenBookにリンクの更新方法を指定
''' Ver.1.03           '2015/02/18     画面更新しない(EXCEL2013で2010以前に比べ処理が遅くなった為追加)
'''                                    ただし、動きが見えなくなる。デバッグ時Trueにする事。
''' </summary>
Public Module xl

	'______________________________________________________________
	'関数名     xlOpen
	'機能       Excelオブジェクト生成
	'引数       なし
	'戻り値     Excel.Application オブジェクト
	'           異常終了時には Nothing を返す
	'______________________________________________________________

	''' <summary>
	''' Excelオブジェクト生成
	''' </summary>
	''' <returns>Excel.Application オブジェクト異常終了時には Nothing を返す</returns>
	Public Function xlOpen() As Application
		On Error Resume Next
		xlOpen = New Application()
		'xlOpen = CreateObject("Excel.Application")
		'xlOpen = New Application()
		If Err.Number <> 0 Then
			On Error GoTo 0
			CriticalAlarm("Excelを開けません。")
			'UPGRADE_NOTE: オブジェクト xlOpen をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
			xlOpen = Nothing
		Else
			On Error GoTo 0
			'UPGRADE_WARNING: オブジェクト xlOpen.Visible の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			xlOpen.Visible = False 'Excelオブジェクト非表示
			'UPGRADE_WARNING: オブジェクト xlOpen.DisplayAlerts の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			xlOpen.DisplayAlerts = False 'ダイアログ非表示
			'UPGRADE_WARNING: オブジェクト xlOpen.ScreenUpdating の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			xlOpen.ScreenUpdating = False '画面更新しない   '2015/02/18 ADD
		End If
	End Function

	'______________________________________________________________
	'関数名     xlClose
	'機能       Excelオブジェクト解放
	'引数       Excel.Application オブジェクト
	'______________________________________________________________

	Public Sub xlClose(ByRef xlApp As Application)
		'Excel解放
		If Not xlApp Is Nothing Then
			On Error Resume Next
			'UPGRADE_WARNING: オブジェクト xlApp.Quit の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			xlApp.Quit()
			On Error GoTo 0
		End If
		'UPGRADE_NOTE: オブジェクト xlApp をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		xlApp = Nothing 'オブジェクト解放
	End Sub

	'______________________________________________________________
	'関数名     xlOpenBook
	'機能       ExcelBookOpen
	'引数       Book名
	'戻り値     開いたブックの Excel.Workbook オブジェクト
	'           異常終了時には Nothing を返す
	'______________________________________________________________

	'Public Function xlOpenBook(xl As Object, BookName As String, Optional RaadOnly As Boolean = True) As Object
	Public Function xlOpenBook(ByRef xl As Application, ByRef BookName As String, Optional ByRef bReadOnly As Boolean = True) As Object
		Dim st As Integer
		On Error Resume Next
		'    xl.Workbooks.Open BookName, ReadOnly:=True '2004/02/04 DEL
		''    xl.Workbooks.Open BookName, ReadOnly:=bReadOnly
		'UPGRADE_WARNING: オブジェクト xl.Workbooks の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		xl.Workbooks.Open(BookName, ReadOnly:=bReadOnly, UpdateLinks:=0) '2014/08/09 ADD
		st = Err.Number
		On Error GoTo 0
		If st = 0 Then
			'UPGRADE_WARNING: オブジェクト xl.Workbooks の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			xlOpenBook = xl.Workbooks(xl.Workbooks.Count)
		Else
			'UPGRADE_NOTE: オブジェクト xlOpenBook をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
			xlOpenBook = Nothing
			CriticalAlarm("ブックファイルを開けません。" & vbCrLf & vbCrLf & "'" & BookName & "'")
		End If
	End Function

	'______________________________________________________________
	'関数名     xlCloseBook
	'機能       ExcelBookClose
	'引数       Excel.Workbook オブジェクト
	'______________________________________________________________
	Public Sub xlCloseBook(ByRef xlBook As Workbook)
		'保存しないでクローズ
		On Error Resume Next
		'UPGRADE_WARNING: オブジェクト xlBook.Close の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		xlBook.Close(SaveChanges:=False)
		'UPGRADE_NOTE: オブジェクト xlBook をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		xlBook = Nothing
		On Error GoTo 0
	End Sub

	''' <summary>
	''' 
	''' </summary>
	''' <param name="src"></param>
	''' <param name="dst"></param>
	''' <returns></returns>
	Public Function xlCopyRange(ByRef src As Range, ByRef dst As Range) As Range
		Dim ws As Worksheet = TryCast(dst.Parent, Worksheet) ' Parent を Worksheet にキャスト
		If ws Is Nothing Then
			Throw New InvalidCastException("dst.Parent is not a Worksheet.")
		End If
		'Range src を Range dst の位置にコピーし、dstの左上のセル位置から srcと同じサイズの Rangeを返します
		'UPGRADE_WARNING: オブジェクト dst.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト src.Copy の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		src.Copy(DirectCast(dst.Cells(1, 1), Range))
		'UPGRADE_WARNING: オブジェクト src.Columns の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト src.Rows の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト dst.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト dst.Parent の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Return ws.Range(DirectCast(dst.Cells(1, 1), Range), DirectCast(dst.Cells(src.Rows.Count, src.Columns.Count), Range))
	End Function

End Module