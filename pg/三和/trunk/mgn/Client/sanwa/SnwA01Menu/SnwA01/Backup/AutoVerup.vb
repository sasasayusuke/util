Option Strict Off
Option Explicit On
Module AutoVerup
	'Ver.1.01           '2018.04.02     更新時のメッセージを削除
	
	Const ForWriting As Short = 2
	
	'自動バージョンアップｉｎｉファイル用
	'機能
	'   サーバー上に配布された最新バージョンにiniファイルを入れ替える
	'   ファイルが最新かどうかは、ファイルの日付を比較する
	'引数
	'   sSetUpFolderUNC : 最新バージョンが入っているフォルダのUNCパス
	'   sFileName       : バージョンアップするファイル名（ファイル名のみ）
	Public Function AutoVersionUpIni(ByRef sSetUpFolderUNC As String, Optional ByRef sFileName As String = "") As Boolean
		Dim sSource As String '新バージョンのファイル
		Dim sExeName As String '実行ファイル名
		Dim sExeFullPath As String
		Dim isModified As Boolean
		Dim ExeIsModified As Boolean
		Dim st As Short
		
		'指定されたフォルダが存在するか
		On Error Resume Next
		st = 0
		st = GetAttr(sSetUpFolderUNC)
		st = Err.Number
		On Error GoTo 0
		If st <> 0 Then
			Exit Function
		End If
		
		'Iniファイルのコピー判定
		sExeName = sFileName
		sSource = sSetUpFolderUNC & "\" & sExeName
		sExeFullPath = AppPath & sExeName
		ExeIsModified = NewCopyFile(sSource, sExeFullPath)
		isModified = isModified Or ExeIsModified
		'ファイルコピーされていたら
		If isModified Then
			AutoVersionUpIni = True
		End If
	End Function
	
	'自動バージョンアップ
	'機能
	'   サーバー上に配布された最新バージョンにExeファイルを入れ替えた後
	'   プログラムを再起動する。同時に他のファイルもコピー可能
	'   ファイルが最新かどうかは、ファイルの日付を比較する
	'引数
	'   sSetUpFolderUNC : 最新バージョンが入っているフォルダのUNCパス
	'   sFileName       : バージョンアップするファイル名リスト（ファイル名のみ）
	'                     ,区切りで複数指定可
	Public Function AutoVersionUp(ByRef sSetUpFolderUNC As String, Optional ByRef sFileName As String = "") As Boolean
		Dim FSO As Object 'FileSystemObject
		Dim wsh As Object
		Dim ts As Object 'TextStream
		Dim sSource As String '新バージョンのファイル
		Dim sDest As String 'ファイルのコピー先
		Dim sScript As String '再起動のスクリプトファイル名
		Dim sExeName As String '実行ファイル名
		Dim sExeFullPath As String
		Dim sFileArray() As String
		Dim isModified As Boolean
		Dim ExeIsModified As Boolean
		Dim i As Short
		Dim st As Short
		
		'指定されたフォルダが存在するか
		On Error Resume Next
		st = 0
		st = GetAttr(sSetUpFolderUNC)
		st = Err.Number
		On Error GoTo 0
		If st <> 0 Then
			Exit Function
		End If
		
		'ファイルリストにあるファイルをコピー
		'ファイルリストにあるファイルを新しければ無条件にコピーする
		''    sFileArray = Split(sFileName, vbTab)
		sFileArray = Split(sFileName, ",")
		isModified = False
		For i = LBound(sFileArray) To UBound(sFileArray)
			sSource = sSetUpFolderUNC & "\" & sFileArray(i)
			sDest = AppPath & sFileArray(i)
			isModified = isModified Or NewCopyFile(sSource, sDest)
		Next i
		'Exeファイルのコピー判定
		'UPGRADE_WARNING: App プロパティ App.EXEName には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
		sExeName = My.Application.Info.AssemblyName & ".exe"
		sSource = sSetUpFolderUNC & "\" & sExeName
		sExeFullPath = AppPath & sExeName
		ExeIsModified = NewCopyFile(sSource, sExeFullPath, False) '実際にはコピーしない
		isModified = isModified Or ExeIsModified
		'ファイルコピーされていたらプログラムを再起動
		If isModified Then
			sScript = AppPath & "update.vbs"
			'再起動するスクリプトファイルを作成する
			''        Set FSO = New FileSystemObject
			FSO = CreateObject("scripting.FileSystemObject")
			'UPGRADE_WARNING: オブジェクト FSO.OpenTextFile の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			ts = FSO.OpenTextFile(sScript, ForWriting, True)
			'UPGRADE_WARNING: オブジェクト ts.WriteLine の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			ts.WriteLine("Dim fso,wsh")
			'UPGRADE_WARNING: オブジェクト ts.WriteLine の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			ts.WriteLine("Set FSO = CreateObject(""Scripting.FileSystemObject"")")
			'UPGRADE_WARNING: オブジェクト ts.WriteLine の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			ts.WriteLine()
			''        ts.WriteLine "MsgBox ""最新版がリリースされました。最新版にアップデートします。"", vbInformation"'2018/04/02 DEL
			'Exeファイルをコピーするスクリプト生成
			If ExeIsModified Then
				'UPGRADE_WARNING: オブジェクト ts.WriteLine の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				ts.WriteLine("FSO.CopyFile """ & sSource & """, """ & sExeFullPath & """")
			End If
			'再起動スクリプト生成
			'UPGRADE_WARNING: オブジェクト ts.WriteLine の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			ts.WriteLine("Set wsh = CreateObject(""WScript.Shell"")")
			'UPGRADE_WARNING: オブジェクト ts.WriteLine の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			ts.WriteLine("wsh.Run """"""" & sExeFullPath & """""""")
			'UPGRADE_WARNING: オブジェクト ts.WriteLine の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			ts.WriteLine("FSO.DeleteFile """ & AppPath & "update.vbs""")
			'UPGRADE_WARNING: オブジェクト ts.Close の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			ts.Close()
			'UPGRADE_NOTE: オブジェクト ts をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
			ts = Nothing
			'UPGRADE_NOTE: オブジェクト FSO をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
			FSO = Nothing
			wsh = CreateObject("WScript.Shell")
			'スクリプトファイルを実行する
			'UPGRADE_WARNING: オブジェクト wsh.Run の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			wsh.Run("""" & sScript & """")
			'UPGRADE_NOTE: オブジェクト wsh をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
			wsh = Nothing
			AutoVersionUp = True
			''        End
		End If
	End Function
	
	Public Function GetFileNode(ByRef sFullPath As String) As String
		Dim p As Object
		
		'UPGRADE_WARNING: オブジェクト p の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		p = InStrRev(sFullPath, "\")
		'UPGRADE_WARNING: オブジェクト p の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If p = 0 Then
			'UPGRADE_WARNING: オブジェクト p の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			p = InStrRev(sFullPath, ":")
			'UPGRADE_WARNING: オブジェクト p の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If p = 0 Then
				GetFileNode = sFullPath
			Else
				'UPGRADE_WARNING: オブジェクト p の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				GetFileNode = Mid(sFullPath, p + 1)
			End If
		Else
			'UPGRADE_WARNING: オブジェクト p の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			GetFileNode = Mid(sFullPath, p + 1)
		End If
	End Function
	
	'ファイルが新しければコピー
	'   sSource sDest ともフルパス指定 sDestにもファイル名部分が必要
	'   bExecute をFalseにすると実際にはコピーは行わない
	'戻り値　コピーした場合はTrue してない場合はFalse
	Private Function NewCopyFile(ByRef sSource As String, ByRef sDest As String, Optional ByRef bExecute As Boolean = True) As Boolean
		Dim FSO As Object 'FileSystemObject
		Dim fileSource As Object 'File
		Dim fileDest As Object 'File
		Dim retValue As Boolean
		
		'    Set FSO = New FileSystemObject
		FSO = CreateObject("scripting.FileSystemObject")
		'UPGRADE_WARNING: オブジェクト FSO.FileExists の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If FSO.FileExists(sSource) Then
			'UPGRADE_WARNING: オブジェクト FSO.GetFile の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fileSource = FSO.GetFile(sSource)
		Else
			'元のファイルがないのでコピーできない
			'        MsgBox "元のファイルがないのでこぴーでけへんだ"
			NewCopyFile = False
			Exit Function
		End If
		'UPGRADE_WARNING: オブジェクト FSO.FileExists の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If FSO.FileExists(sDest) Then
			'UPGRADE_WARNING: オブジェクト FSO.GetFile の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fileDest = FSO.GetFile(sDest)
			'ファイルが新しければコピー
			'UPGRADE_WARNING: オブジェクト fileDest.DateLastModified の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fileSource.DateLastModified の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If fileSource.DateLastModified > fileDest.DateLastModified Then
				'            MsgBox "新しかったのでコピーした"
				'UPGRADE_WARNING: オブジェクト fileSource.Copy の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				If bExecute Then fileSource.Copy(sDest, True)
				retValue = True
			Else
				'            MsgBox "古かったのでコピーしてない"
				retValue = False
			End If
		Else
			'送り先のファイルがない場合もコピー
			'        MsgBox "送り先ファイルがなかったのでコピーした"
			'UPGRADE_WARNING: オブジェクト fileSource.Copy の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If bExecute Then fileSource.Copy(sDest, True)
			retValue = True
		End If
		'UPGRADE_NOTE: オブジェクト fileSource をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		fileSource = Nothing
		'UPGRADE_NOTE: オブジェクト fileDest をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		fileDest = Nothing
		'UPGRADE_NOTE: オブジェクト FSO をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		FSO = Nothing
		NewCopyFile = retValue
	End Function
End Module