Option Strict Off
Option Explicit On
Module OutText
	'ADODB VERSION
	'Ver.1.00           '2002.04.17
	'Ver.1.01           '2003/01/14 FileOverWriteCheck:開いているファイルのチェックを追加
	'Ver.1.02           '2003/01/14 FileOverWriteCheck:CompelOverWriteを付ける
	'                                                   強制的に上書きをしてしまうようにする
	'Ver.1.03           '2003/10/17 NumberingFileName:ファイルが存在していたらナンバリングしてファイル名を返すを追加
	'                                                   （未使用）デバッグしていない
	'Ver.1.04           '2004/07/09 FileOverWriteCheck:圧縮ファイルでの書き込みに対応(2048)
	'Ver.1.05           '2004/11/02 OutputTextFileFromRecordset:(FileAppend)追加書きに対応
	'2004/11/02 FileWriteFieldsをPublicに変更
	'Ver.1.06           '2006/08/11 テキストファイルに項目名をセットするかのフラグ(ItemNameRow)を追加---浅井建築
	'Ver.1.06           '2009/02/04 FileOverWriteCheckの特殊属性でのエラーメッセージに属性を表示するように変更
	'Ver.1.10           '2011/09/02 OutputTextFileFromRecordset・FileWriteFields
	'                                   除外する項目を指定するプロパティ「AryExcludeFields」を追加
	
	
	'----------------------------------------------------------
	'レコードセットをテキストファイルにダンプする
	'引数
	'       rs          ADODB.Recordset
	'       FileName    出力先ファイル名（フルパス）
	'       Separator   区切り文字（省略時','）
	'       FileAppend  アペンドするかしないか
	'戻値
	'       0           正常終了
	'       -1          上書き確認時のユーザーによる中止
	'       (Other)     Err.Numberを返す
	'----------------------------------------------------------
	Public Function OutputTextFileFromRecordset(ByRef rs As ADODB.Recordset, ByRef FileName As String, Optional ByRef Separator As String = ",", Optional ByRef FileAppend As Boolean = False, Optional ByRef ItemNameRow As Boolean = True, Optional ByRef AryExcludeFields As Object = Nothing) As Integer '2011/09/02 ADD
		'Public Function OutputTextFileFromRecordset(rs As adodb.Recordset, FileName As String, _
		''                                            Optional Separator As String = ",", _
		''                                            Optional FileAppend As Boolean = False, _
		''                                            Optional ItemNameRow As Boolean = True) As Long     '2006/08/11
		'Public Function OutputTextFileFromRecordset(rs As adodb.Recordset, FileName As String, _
		'Optional Separator As String = ",", _
		'Optional FileAppend As Boolean = False) As Long     '2004/11/02
		'Public Function OutputTextFileFromRecordset(rs As adodb.Recordset, FileName As String, _
		'Optional Separator As String = ",") As Long
		Dim st As Integer
		Dim ErMsg As String
		Dim fp, OutFp As Short
		Dim tmp As String
		Dim In_Tmp As String
		Dim Out_Tmp As String
		Dim i As Integer
		Dim CopyFieldsCount As Short
		
		ErMsg = ""
		OutFp = 0
		
		On Error Resume Next
		st = rs.Fields.Count
		st = Err.Number
		On Error GoTo 0
		If st <> 0 Then
			ErMsg = "出力ソースのレコードセットが無効です。"
			GoTo ExitProc
		End If
		
		'上書きのチェック
		If FileAppend = False Then '2004/11/02
			st = FileOverWriteCheck(FileName)
			If st <> 0 Then GoTo ExitProc
		End If
		
		fp = FreeFile
		On Error Resume Next
		'------------------------------------------------------2004/11/02
		'''    Open FileName For Output As fp
		If FileAppend = False Then '2004/11/02
			FileOpen(fp, FileName, OpenMode.Output, , OpenShare.LockReadWrite)
		Else
			'UPGRADE_WARNING: Dir に新しい動作が指定されています。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
			If Dir(FileName) <> "" Then
				FileOpen(fp, FileName, OpenMode.Input, , OpenShare.LockReadWrite)
				If EOF(fp) Then
					FileClose(fp)
					fp = FreeFile
					FileOpen(fp, FileName, OpenMode.Output, , OpenShare.LockReadWrite)
				Else
					FileClose(fp)
					fp = FreeFile
					FileOpen(fp, FileName, OpenMode.Append, , OpenShare.LockReadWrite)
				End If
			Else
				FileOpen(fp, FileName, OpenMode.Output, , OpenShare.LockReadWrite)
			End If
		End If
		'------------------------------------------------------2004/11/02
		st = Err.Number
		On Error GoTo 0
		If st <> 0 Then
			ErMsg = "ファイルを作成できません。"
			GoTo ExitProc
		End If
		
		OutFp = fp
		
		On Error GoTo OnErr
		With rs
			'列名行を出力
			If ItemNameRow = True Then '2006/08/11 ADD
				'''            Print #fp, FileWriteFields(rs.Fields, Separator, True)                   '2011/09/02 DEL
				PrintLine(fp, FileWriteFields((rs.Fields), Separator, True, AryExcludeFields)) '2011/09/02 ADD
				If rs.CursorType <> ADODB.CursorTypeEnum.adOpenForwardOnly Then
					If Not (rs.BOF And rs.EOF) Then
						'レコードセットの先頭行へ移動します
						rs.MoveFirst()
					End If
				End If
			End If '2006/08/11 ADD
			'データ行を出力
			Do Until .EOF
				'''            Print #fp, FileWriteFields(rs.Fields, Separator)                         '2011/09/02 DEL
				PrintLine(fp, FileWriteFields((rs.Fields), Separator,  , AryExcludeFields)) '2011/09/02 ADD
				.MoveNext()
			Loop 
		End With
		On Error GoTo 0
		
		Inform("テキストファイルを作成しました。")
		
ExitProc: 
		If OutFp <> 0 Then FileClose(OutFp)
		If ErMsg <> "" Then
			CriticalAlarm(ErMsg, "テキストファイル出力")
		End If
		OutputTextFileFromRecordset = st
		Exit Function
		
OnErr: 
		st = Err.Number
		ErMsg = "(" & CStr(st) & ")" & Err.Description
		Resume ExitProc
	End Function
	
	''Private Function FileWriteFields(flds As ADODB.Fields, Separator As String, Optional ItemNameRow As Boolean = False) As String
	''Public Function FileWriteFields(flds As adodb.Fields, Separator As String, Optional ItemNameRow As Boolean = False) As String
	Public Function FileWriteFields(ByRef flds As ADODB.Fields, ByRef Separator As String, Optional ByRef ItemNameRow As Boolean = False, Optional ByRef AryExcludeFields As Object = Nothing) As String '2011/09/02 ADD
		Dim Outs As String
		Dim fld As ADODB.Field
		Outs = ""
		
		If Not IsArray(AryExcludeFields) Then
			'AryExcludeFieldsが指定されてない場合
			'空欄の配列を作成する
			'UPGRADE_WARNING: Array に新しい動作が指定されています。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト AryExcludeFields の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			AryExcludeFields = New Object(){}
		End If
		
		For	Each fld In flds
			If Not (IsFindArray(AryExcludeFields, (fld.Name))) Then '2011/09/02 ADD
				'除外項目でないならば
				If Outs <> "" Then Outs = Outs & Separator
				If ItemNameRow Then
					Outs = Outs & fld.Name
				Else
					'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
					If Not IsDbNull(fld.Value) Then
						Select Case fld.Type
							Case ADODB.DataTypeEnum.adChar, ADODB.DataTypeEnum.adBSTR, ADODB.DataTypeEnum.adVarWChar, ADODB.DataTypeEnum.adWChar, ADODB.DataTypeEnum.adVarChar
								Outs = Outs & """" & fld.Value & """"
							Case ADODB.DataTypeEnum.adDate, ADODB.DataTypeEnum.adDBDate, ADODB.DataTypeEnum.adDBTimeStamp
								Outs = Outs & VB6.Format(fld.Value, "yyyy/mm/dd")
							Case ADODB.DataTypeEnum.adCurrency, ADODB.DataTypeEnum.adDecimal
								''''                        Outs = Outs & Format$(fld.Value, "\\#0.00")
								Outs = Outs & VB6.Format(fld.Value, "#0.00") '2004/11/02
							Case Else
								Outs = Outs & CStr(fld.Value)
						End Select
					End If
				End If
			End If '2011/09/02 ADD
		Next fld
		FileWriteFields = Outs
	End Function
	
	'配列内に指定項目が存在するか
	Public Function IsFindArray(ByRef ArrayItems As Object, ByRef SearchItem As Object) As Boolean
		Dim AryItem As Object
		IsFindArray = False
		
		For	Each AryItem In ArrayItems
			'UPGRADE_WARNING: オブジェクト SearchItem の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト AryItem の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If AryItem = SearchItem Then
				IsFindArray = True
				Exit For
			End If
		Next AryItem
	End Function
	
	'ファイル出力時の上書きチェックを行います
	''Public Function FileOverWriteCheck(FileName As String, Optional NoOverWriteWarning As Boolean = False) As Long
	Public Function FileOverWriteCheck(ByRef FileName As String, Optional ByRef NoOverWriteWarning As Boolean = False, Optional ByRef CompelOverWrite As Boolean = False) As Integer
		
		'NoOverWriteWarning 上書きしない。
		'CompelOverWrite    無条件上書きにする。
		
		Dim st As Integer
		Dim tmp As Object
		
		'ファイルの存在をチェックします
		'UPGRADE_WARNING: オブジェクト tmp の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		tmp = CutOutPath(FileName)
		'UPGRADE_WARNING: オブジェクト tmp の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If tmp = "" Then
			FileName = AppPath & FileName
		Else
			'パスのチェック
			st = 0
			On Error Resume Next
			'UPGRADE_WARNING: オブジェクト tmp の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			st = GetAttr(tmp)
			st = Err.Number
			On Error GoTo 0
			If st <> 0 Then
				CheckAlarm("指定のファイルパスにアクセスできません。" & vbCrLf & "無効、またはアクセス権限のないパスが指定されているか" & vbCrLf & "または無効なデバイスです。")
				FileOverWriteCheck = st
				Exit Function
			End If
		End If
		On Error Resume Next
		'''''    tmp = GetAttr(FileName) And Not vbArchive                 '2004/07/09 ADD
		'UPGRADE_WARNING: オブジェクト tmp の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		tmp = GetAttr(FileName) And Not FileAttribute.Archive And Not 2048 '2048:圧縮ファイル
		st = Err.Number
		On Error GoTo 0
		If st <> 0 Then
			'ファイルが存在していないのでチェックＯＫ
			st = 0
		Else
			'ファイルが存在しているので開いているかチェックします   '2003/01/14 ADD
			On Error Resume Next
			Rename(FileName, FileName)
			If Err.Number <> 0 Then
				CheckAlarm("ファイルは使用中です。")
				st = -6
			Else '2003/01/14 ADD
				'ファイルは存在しているのでアトリビュートをチェックします
				If tmp And (FileAttribute.System Or FileAttribute.Hidden Or FileAttribute.Volume) Then
					CheckAlarm("システム属性のファイルには上書きできません。")
					st = -3
				ElseIf tmp And FileAttribute.Directory Then 
					CheckAlarm("出力ファイル名と同名のフォルダが存在しています。")
					st = -4
				ElseIf tmp And FileAttribute.ReadOnly Then 
					CheckAlarm("既存のファイルが読み取り専用のため上書きできません。")
					st = -5
					'UPGRADE_WARNING: オブジェクト tmp の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				ElseIf tmp <> 0 Then 
					'UPGRADE_WARNING: オブジェクト tmp の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					CheckAlarm("指定のファイルは特殊属性のため上書きできません。" & vbCrLf & "属性:" & tmp) '2009/02/06 ADD
					st = -7 '2004/07/09 ADD
				Else
					If CompelOverWrite = False Then '2003/01/14 ADD
						If NoOverWriteWarning = False Then
							If MsgBoxResult.No = YesNo("既存のファイルは置き換えられます。") Then
								st = -2 'オペレータによるキャンセル
							End If
						Else
							st = -1
						End If
					End If
				End If
			End If
		End If
		FileOverWriteCheck = st
	End Function
	
	Public Function NumberingFileName(ByRef FullName As String) As String
		'----------------------------------------------------------
		'ファイルが存在していたらナンバリングして新規作成するファイル名を返す
		'引数
		'       rs          ADODB.Recordset
		'       FileName    出力先ファイル名（フルパス）
		'       Separator   区切り文字（省略時','）
		'戻値
		'       0           正常終了
		'       -1          上書き確認時のユーザーによる中止
		'       (Other)     Err.Numberを返す
		'----------------------------------------------------------
		Dim i As Short
		Dim FilePath, FileName, FileExtension As String
		Dim st As Integer
		
		i = 0
		NumberingFileName = FullName
		
		Do 
			'存在チェック
			st = FileOverWriteCheck(NumberingFileName, True)
			
			If st = -1 Then
				i = i + 1
				Call SeparateFilename(FullName, FilePath, FileName, FileExtension)
				
				NumberingFileName = FilePath & "\" & FileName & "(" & i & ")" & "." & FileExtension
			End If
		Loop While st = -1
	End Function
End Module