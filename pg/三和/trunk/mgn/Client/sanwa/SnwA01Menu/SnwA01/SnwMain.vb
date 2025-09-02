Option Strict Off
Option Explicit On
Module SnwMain
	
	'UPGRADE_WARNING: Sub Main() が完了したときにアプリケーションは終了します。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="E08DDC71-66BA-424F-A612-80AF11498FF8"' をクリックしてください。
	Public Sub Main()
		Dim SvEXEPath As String
		Dim PGS As String
		Dim UPDATES As String
		
		'UPGRADE_WARNING: Dir に新しい動作が指定されています。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
		If Len(Dir(AppPath("UPDATE.ini"))) = 0 Then
			Err.Raise(vbObjectError + 1,  , "ＩＮＩファイルがありません。")
			Exit Sub
		End If
		
		'サーバーの最新EXEが入っているフォルダを指定する。
		'UPGRADE_WARNING: オブジェクト GetIni() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		SvEXEPath = GetIni("Common", "SvEXEPath", "UPDATE.ini")
		If SvEXEPath = vbNullString Then
			Call WriteIni("Common", "SvEXEPath", DefSvEXEPath, "UPDATE.ini")
			SvEXEPath = DefSvEXEPath
		End If
		
		'UPDATE.iniの最新チェック
		'バージョンアップチェック
		Call AutoVersionUpIni(SvEXEPath, "UPDATE.ini")
		
		'UPDATEするファイル名を取得する。
		'UPGRADE_WARNING: オブジェクト GetIni() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		UPDATES = GetIni("UPDATES", "UPDATES", "UPDATE.ini")
		If UPDATES = vbNullString Then
			Err.Raise(vbObjectError + 1,  , "ファイル名がありません。")
			Exit Sub
		End If
		
		'バージョンアップチェック
		If AutoVersionUp(SvEXEPath, UPDATES) = False Then
			'--------------------------------------------------
			'ｱﾌﾟﾘｹｰｼｮﾝの初期化を行う
			'--------------------------------------------------
			If ApplicationInit = False Then
				Exit Sub
			End If
			
			'UPGRADE_ISSUE: Load ステートメント はサポートされていません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="B530EFF2-3132-48F8-B8BC-D88AF543D321"' をクリックしてください。
			Load(SnwA01F00)
			
			'メインフォームの表示
			SnwA01F00.Show()
		End If
		
	End Sub
End Module