Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.PowerPacks
Friend Class SnwMT02F04
	Inherits System.Windows.Forms.Form
	'
	'--------------------------------------------------------------------
	'  ユーザー名           株式会社三和商研
	'  業務名　　　　　　　　積算データ管理システム
	'  部門名               見積部門
	'  プログラム名         員数シート取込画面（ＥＸＣＥＬ）
	'  作成会社             テクノウェア株式会社
	'  作成日               2003/06/16
	'  作成者               kawamura
	'--------------------------------------------------------------------
	'
	Const ProfileKey As String = "仕分員数リスト入力"
	Dim GXLSPath As String '出力パス
	
	Dim pParentForm As System.Windows.Forms.Form
	Dim pMituNo As Integer
	
	Dim wMituNo As Integer
	Dim wSiwaNo As Integer
	Dim wSiwaGyo As Short
	Dim wTempData As Object
	
	Private Sub SnwMT02F04_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		'フォームを画面の中央に配置
		Me.Top = VB6.TwipsToPixelsY((VB6.PixelsToTwipsY(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height) - VB6.PixelsToTwipsY(Me.Height)) \ 2)
		Me.Left = VB6.TwipsToPixelsX((VB6.PixelsToTwipsX(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width) - VB6.PixelsToTwipsX(Me.Width)) \ 2)
		
		'''''    rf_見積番号 = Format$(pMituNo, "#")
		
		'入力先情報セット
		'UPGRADE_WARNING: オブジェクト GetIni() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		GXLSPath = GetIni("Path", ProfileKey, INIFile)
		'UPGRADE_WARNING: TextBox プロパティ txDir.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
		txDir.Text = CompactPathEx(GXLSPath, txDir.Maxlength)
	End Sub
	
	Private Sub SnwMT02F04_FormClosing(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
		Dim Cancel As Boolean = eventArgs.Cancel
		Dim UnloadMode As System.Windows.Forms.CloseReason = eventArgs.CloseReason
		''    ReleaseRs rs(0)
		''    ReleaseRs rs(1)
		pParentForm.Enabled = True
		pParentForm.Activate()
		'UPGRADE_NOTE: オブジェクト pParentForm をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		pParentForm = Nothing
		eventArgs.Cancel = Cancel
	End Sub
	
	Private Sub cb中止_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cb中止.Click
		Me.Close()
	End Sub
	
	Private Sub cbXLS_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cbXLS.Click
		Dim st As Integer
		
		If txDir.Text = vbNullString Then
			CriticalAlarm("ファイルを指定して下さい。")
			cb変更.Focus()
			Exit Sub
		End If
		
		If MsgBoxResult.No = YesNo("指定のファイルを員数シートに取込みます。") Then Exit Sub
		'2004/02/05 ADD
		If FileOverWriteCheck(GXLSPath,  , True) <> 0 Then Exit Sub 'オーバーライトをチェックします
		st = 0
		st = 員数シート取込(GXLSPath)
		If st = 0 Then
			rf_取込仕分.Text = CStr(wSiwaNo)
			rf_取込行数.Text = CStr(wSiwaGyo)
			If MsgBoxResult.Yes = Question("終了しました。適用しますか？" & vbCrLf & "", MsgBoxResult.Yes) Then
				'UPGRADE_ISSUE: Control SiwaNo は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
				pParentForm.SiwaNo = wSiwaNo
				'UPGRADE_ISSUE: Control SiwaGyo は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
				pParentForm.SiwaGyo = wSiwaGyo
				'UPGRADE_ISSUE: Control TempData は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト wTempData の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				pParentForm.TempData = wTempData
				'親のメソッドで処理をする。
				'UPGRADE_ISSUE: Control SetImportData は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
				pParentForm.SetImportData()
				Call Set取込FLG(GXLSPath)
				Me.Close()
				Exit Sub
			End If
		Else
			cb変更.Focus()
			Exit Sub
		End If
	End Sub
	
	Private Sub cb変更_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cb変更.Click
		''    If OpenFileDialog(GXLSPath, ProfileKey, "xls") = True Then
		If OpenFileDialog(GXLSPath, "仕分", "xls") = True Then
			WriteIni("Path", ProfileKey, GXLSPath, INIFile)
		End If
		'UPGRADE_WARNING: TextBox プロパティ txDir.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
		txDir.Text = CompactPathEx(GXLSPath, txDir.Maxlength)
		''    rf_コメント.Visible = False
		rf_取込仕分.Text = vbNullString
		rf_取込行数.Text = vbNullString
	End Sub
	
	Private Function Set取込FLG(ByRef InFile As String) As Integer
		'UPGRADE_NOTE: xl は xl_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
		Dim xl_Renamed As Object 'Excel.Application
		Dim wkb As Object 'Excel.Workbook
		Dim wks As Object 'Excel.Worksheet
		
		'Excel.Application クラスオブジェクトを開きます
		xl_Renamed = xlOpen()
		If xl_Renamed Is Nothing Then
			GoTo Set取込FLG_Exit
		End If
		'Templateのブックを開きます
		wkb = xlOpenBook(xl_Renamed, InFile, False)
		If wkb Is Nothing Then
			GoTo Set取込FLG_Exit
		End If
		
		'テンプレートのシートを変数にセットします
		'UPGRADE_WARNING: オブジェクト wkb.Worksheets の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		wks = wkb.Worksheets(1)
		
		'取込フラグをセットする。
		'UPGRADE_WARNING: オブジェクト wks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		wks.Cells(1, 1) = 1
		
		'名前を付けて保存します
		'UPGRADE_WARNING: オブジェクト wkb.SaveAs の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		wkb.SaveAs(InFile, -4143) 'xlWorkbookNormal
		
		If Not xl_Renamed Is Nothing Then
			'UPGRADE_NOTE: オブジェクト wks をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
			wks = Nothing
			xlCloseBook(wkb) 'ワークブックを閉じます
			xlClose(xl_Renamed) 'Excel.Applicationオブジェクトを開放します
		End If
		
Set取込FLG_Exit: 
		Exit Function
		
Set取込FLG_Err: 
		MsgBox(Err.Number & " " & Err.Description)
	End Function
	
	Private Function 員数シート取込(ByRef InFile As String) As Integer
		Dim st As Integer
		'UPGRADE_NOTE: xl は xl_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
		Dim xl_Renamed As Object 'Excel.Application
		Dim wkb As Object 'Excel.Workbook
		Dim wks As Object 'Excel.Worksheet
		
		Dim Core As Object 'Excel.Range
		Dim CoreRows, CoreColumns As Short
		
		Dim RecCnt As Integer
		Dim i As Short
		Dim r As Short
		Dim LCNT As Short
		Dim MaxRows As Integer
		Dim w2FLG As String '2004/02/05 ADD
		
		'処理メッセージ準備
		System.Windows.Forms.Application.DoEvents()
		'マウスポインターを砂時計にする
		Me.Enabled = False
		HourGlass(True)
		'メッセージを表示
		Fw_Msg.Text = Me.Text
		VB6.ShowForm(Fw_Msg, VB6.FormShowConstants.Modeless, Me)
		Fw_Msg.Refresh()
		
		'戻り値をリセットします
		st = 0
		'*---処理メッセージ表示
		Fw_Msg.StsCaption = "ファイルを開いています..."
		
		'Excel.Application クラスオブジェクトを開きます
		xl_Renamed = xlOpen()
		If xl_Renamed Is Nothing Then
			st = -1
			GoTo exit_proc
		End If
		'Templateのブックを開きます
		wkb = xlOpenBook(xl_Renamed, InFile)
		If wkb Is Nothing Then
			st = -2
			GoTo exit_proc
		End If
		
		'テンプレートのシートを変数にセットします
		'UPGRADE_WARNING: オブジェクト wkb.Worksheets の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		wks = wkb.Worksheets(1)
		'最初のシートを選択しておきます
		'UPGRADE_WARNING: オブジェクト wkb.Worksheets の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		wkb.Worksheets(1).Activate()
		
		'中止ボタンの確認
		System.Windows.Forms.Application.DoEvents()
		If Fw_Msg.AbortDoing = True Then GoTo Abort
		
		''xl.Visible = True
		
		On Error GoTo on_err 'エラートラップを開始します
		
		'２重取込のチェックをします。                       '2004/02/05 ADD
		'UPGRADE_WARNING: オブジェクト wks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		w2FLG = wks.Cells(1, 1)
		'シート情報チェック
		If w2FLG = "1" Then
			If MsgBoxResult.No = MsgBox("指定されたシートは既に取り込まれています。" & vbCrLf & vbCrLf & "取り込みを行なってもよろしいですか？", MsgBoxStyle.Question + MsgBoxStyle.YesNo, Me.Text) Then
				st = -2
				GoTo exit_proc
			End If
		End If
		
		'見積情報を取得します。
		'    wMituNo = wks.Cells(2, 7)
		Fw_Msg.StsCaption = "見積番号チェック" '2022/11/11 ADD
		'UPGRADE_WARNING: オブジェクト wks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		wMituNo = wks.Cells(2, 8) '2015/02/04 ADD
		rf_見積番号.Text = VB6.Format(wMituNo, "#")
		'シート情報チェック
		If pMituNo <> wMituNo Then
			If MsgBoxResult.No = MsgBox("指定されたシートは現在修正中のシートと違います。" & vbCrLf & vbCrLf & "取り込みを行なってもよろしいですか？", MsgBoxStyle.Question + MsgBoxStyle.YesNo, Me.Text) Then
				st = -2
				GoTo exit_proc
			End If
		End If
		
		Fw_Msg.StsCaption = "仕分レベルチェック"
		'UPGRADE_WARNING: オブジェクト wks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		wSiwaNo = wks.Cells(3, 8) 'wks.Cells(3, 7)'2015/02/04 ADD
		wSiwaGyo = 0
		
		'タイトル行の設定
		LCNT = 6
		
		'プログレスバーの値セット
		Fw_Msg.ProgValue = 0
		Fw_Msg.ProgMax = SPMaxRow
		
		'データ明細部の取得
		'UPGRADE_WARNING: 配列 wTempData の下限が 1,1 から 0,0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"' をクリックしてください。
		ReDim wTempData(SPMaxRow, 27)
		
		'*---処理メッセージ表示
		Fw_Msg.StsCaption = "検索中。。。"
		'読み込みカウント
		r = 0
		'最終行を探します。
		With wks
			For i = LBound(wTempData, 1) To UBound(wTempData, 1)
				
				'中止ボタンの確認
				System.Windows.Forms.Application.DoEvents()
				If Fw_Msg.AbortDoing = True Then GoTo Abort
				
				'UPGRADE_WARNING: オブジェクト wks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				If Trim(StrConv(.Cells(r + LCNT, 1), VbStrConv.UpperCase)) = vbNullString And Trim(StrConv(.Cells(r + LCNT, 3), VbStrConv.UpperCase)) = vbNullString And Trim(StrConv(.Cells(r + LCNT, 4), VbStrConv.UpperCase)) = vbNullString And Trim(StrConv(.Cells(r + LCNT, 5), VbStrConv.UpperCase)) = vbNullString And Trim(.Cells(r + LCNT, 8)) = vbNullString Then
					Exit For
				End If
				r = r + 1
				'プログレスバーの値セット
				Fw_Msg.ProgValue = r
			Next 
		End With
		'一度プログレスバーを最大にする
		Fw_Msg.ProgValue = Fw_Msg.ProgMax
		
		'プログレスバーの値セット
		Fw_Msg.ProgValue = 0
		Fw_Msg.ProgMax = r
		
		'最大行数での変数の初期化
		'UPGRADE_WARNING: 配列 wTempData の下限が 1,1 から 0,0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"' をクリックしてください。
		ReDim wTempData(r, 28)
		
		'*---処理メッセージ表示
		Fw_Msg.StsCaption = "取込中。。。"
		'読み込みカウント
		r = 0
		'集計値をセルにセットします
		With wks
			For i = LBound(wTempData, 1) To UBound(wTempData, 1)
				
				'中止ボタンの確認
				System.Windows.Forms.Application.DoEvents()
				If Fw_Msg.AbortDoing = True Then GoTo Abort
				
				Fw_Msg.StsCaption = "取込中。。。" & i & "行目"
				
				'員数シート情報取込
				'2004/01/22 UPD
				'UPGRADE_WARNING: オブジェクト wks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト AnsiLeftB() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト wTempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wTempData(i, 1) = Trim(StrConv(AnsiLeftB(.Cells(r + LCNT, 1), 1), VbStrConv.UpperCase)) '見積区分
				'UPGRADE_WARNING: オブジェクト wks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト wTempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wTempData(i, 2) = Trim(StrConv(.Cells(r + LCNT, 2), VbStrConv.UpperCase)) 'SP区分
				'UPGRADE_WARNING: オブジェクト wks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト AnsiLeftB() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト wTempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wTempData(i, 3) = Trim(StrConv(AnsiLeftB(.Cells(r + LCNT, 3), 1), VbStrConv.UpperCase)) 'PC区分
				'UPGRADE_WARNING: オブジェクト wks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト AnsiLeftB() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト wTempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wTempData(i, 4) = Trim(StrConv(AnsiLeftB(.Cells(r + LCNT, 4), 7), VbStrConv.UpperCase)) '製品NO
				'UPGRADE_WARNING: オブジェクト wks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト AnsiLeftB() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト wTempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wTempData(i, 5) = Trim(StrConv(AnsiLeftB(.Cells(r + LCNT, 5), 7), VbStrConv.UpperCase)) '仕様NO
				'UPGRADE_WARNING: オブジェクト wks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト AnsiLeftB() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト wTempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wTempData(i, 6) = Trim(StrConv(AnsiLeftB(.Cells(r + LCNT, 6), 7), VbStrConv.UpperCase)) 'ベース色
				'2015/02/04 ADD↓
				'UPGRADE_WARNING: オブジェクト wks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト AnsiLeftB() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト wTempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wTempData(i, 7) = Trim(StrConv(AnsiLeftB(.Cells(r + LCNT, 7), 10), VbStrConv.UpperCase)) '他社伝票番号
				'UPGRADE_WARNING: オブジェクト wks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト AnsiLeftB() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト wTempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wTempData(i, 8) = Trim(AnsiLeftB(.Cells(r + LCNT, 8), 40)) '名称
				'UPGRADE_WARNING: オブジェクト wks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト wTempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wTempData(i, 9) = IIf(.Cells(r + LCNT, 9) = 0, "", AnsiLeftB(.Cells(r + LCNT, 9), 4)) 'W
				'UPGRADE_WARNING: オブジェクト wks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト wTempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wTempData(i, 10) = IIf(.Cells(r + LCNT, 10) = 0, "", AnsiLeftB(.Cells(r + LCNT, 10), 4)) 'D
				'UPGRADE_WARNING: オブジェクト wks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト wTempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wTempData(i, 11) = IIf(.Cells(r + LCNT, 11) = 0, "", AnsiLeftB(.Cells(r + LCNT, 11), 4)) 'H
				'UPGRADE_WARNING: オブジェクト wks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト wTempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wTempData(i, 12) = IIf(.Cells(r + LCNT, 12) = 0, "", AnsiLeftB(.Cells(r + LCNT, 12), 4)) 'D1
				'UPGRADE_WARNING: オブジェクト wks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト wTempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wTempData(i, 13) = IIf(.Cells(r + LCNT, 13) = 0, "", AnsiLeftB(.Cells(r + LCNT, 13), 4)) 'D2
				'UPGRADE_WARNING: オブジェクト wks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト wTempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wTempData(i, 14) = IIf(.Cells(r + LCNT, 14) = 0, "", AnsiLeftB(.Cells(r + LCNT, 14), 4)) 'H1
				'UPGRADE_WARNING: オブジェクト wks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト wTempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wTempData(i, 15) = IIf(.Cells(r + LCNT, 15) = 0, "", AnsiLeftB(.Cells(r + LCNT, 15), 4)) 'H2
				'UPGRADE_WARNING: オブジェクト wks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト wTempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wTempData(i, 28) = NullToZero(.Cells(r + LCNT, 28), 0) '合計
				'UPGRADE_WARNING: オブジェクト wks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト AnsiLeftB() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト wTempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wTempData(i, 19) = Trim(AnsiLeftB(.Cells(r + LCNT, 19), 6)) '単位
				'2015/02/04 ADD↑
				'2015/02/04 DEL↓
				''''''            wTempData(i, 7) = Trim$(AnsiLeftB(.Cells(r + LCNT, 7), 40))                       '名称
				''''''            wTempData(i, 8) = IIf(.Cells(r + LCNT, 8) = 0, "", AnsiLeftB(.Cells(r + LCNT, 8), 4))    'W
				''''''            wTempData(i, 9) = IIf(.Cells(r + LCNT, 9) = 0, "", AnsiLeftB(.Cells(r + LCNT, 9), 4))    'D
				''''''            wTempData(i, 10) = IIf(.Cells(r + LCNT, 10) = 0, "", AnsiLeftB(.Cells(r + LCNT, 10), 4))   'H
				''''''            wTempData(i, 11) = IIf(.Cells(r + LCNT, 11) = 0, "", AnsiLeftB(.Cells(r + LCNT, 11), 4))  'D1
				''''''            wTempData(i, 12) = IIf(.Cells(r + LCNT, 12) = 0, "", AnsiLeftB(.Cells(r + LCNT, 12), 4))  'D2
				''''''            wTempData(i, 13) = IIf(.Cells(r + LCNT, 13) = 0, "", AnsiLeftB(.Cells(r + LCNT, 13), 4))  'H1
				''''''            wTempData(i, 14) = IIf(.Cells(r + LCNT, 14) = 0, "", AnsiLeftB(.Cells(r + LCNT, 14), 4))  'H2
				''''''''            wTempData(i, 14) = NullToZero(.Cells(r + LCNT, 14), 0)               '原価
				''''''''            wTempData(i, 15) = NullToZero(.Cells(r + LCNT, 15), 0)               '売価
				''''''''            wTempData(i, 16) = NullToZero(.Cells(r + LCNT, 16), 0)               '数量
				''''''            wTempData(i, 18) = Trim$(AnsiLeftB(.Cells(r + LCNT, 18), 6))           '単位
				''''''''            wTempData(i, 18) = NullToZero(.Cells(r + LCNT, 18), 0)               '金額
				''''''''            wTempData(i, 19) = Trim$(StrConv(.Cells(r + LCNT, 19), vbUpperCase)) '仕入先
				''''''''            wTempData(i, 20) = Trim$(StrConv(.Cells(r + LCNT, 20), vbUpperCase)) '仕入先
				''''''''            wTempData(i, 21) = Trim$(StrConv(.Cells(r + LCNT, 21), vbUpperCase)) '送り先
				''''''''            wTempData(i, 22) = Trim$(StrConv(.Cells(r + LCNT, 22), vbUpperCase)) '送り先
				''''''''            wTempData(i, 23) = NullToZero(.Cells(r + LCNT, 23), 0)               '社内在庫
				''''''''            wTempData(i, 24) = NullToZero(.Cells(r + LCNT, 24), 0)               '客先在庫
				''''''''            wTempData(i, 25) = NullToZero(.Cells(r + LCNT, 25), 0)               '転用
				''''''            wTempData(i, 27) = NullToZero(.Cells(r + LCNT, 27), 0)               '合計
				'2015/02/04 DEL↑
				
				r = r + 1
				'プログレスバーの値セット
				Fw_Msg.ProgValue = r
			Next 
			
		End With
		
		'中止ボタンの確認
		System.Windows.Forms.Application.DoEvents()
		If Fw_Msg.AbortDoing = True Then GoTo Abort
		
		'プログレスバーを最大にして終了
		Fw_Msg.ProgValue = Fw_Msg.ProgMax
		
		'テンプレートのシートを削除します
		'UPGRADE_NOTE: オブジェクト Core をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		Core = Nothing
		'UPGRADE_NOTE: オブジェクト wks をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		wks = Nothing
		'最初のシートを選択しておきます
		'UPGRADE_WARNING: オブジェクト wkb.Worksheets の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		wkb.Worksheets(1).Activate()
		
		GoTo exit_proc
Abort: 
		'*---処理メッセージ表示
		Fw_Msg.StsCaption = "中止しています．．．"
		st = -1
exit_proc: 
		On Error GoTo 0 'エラートラップを解除します
		
		'UPGRADE_NOTE: オブジェクト Core をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		Core = Nothing
		'UPGRADE_NOTE: オブジェクト wks をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		wks = Nothing
		If Not xl_Renamed Is Nothing Then
			xlCloseBook(wkb) 'ワークブックを閉じます
			xlClose(xl_Renamed) 'Excel.Applicationオブジェクトを開放します
		End If
		
		員数シート取込 = st
		'取り込み件数のセット
		wSiwaGyo = r
		
		'処理メッセージの終了
		Me.Enabled = True
		Me.Activate()
		'メッセージを閉じる
		Fw_Msg.Hide()
		Fw_Msg.Refresh()
		'UPGRADE_NOTE: オブジェクト Fw_Msg をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		Fw_Msg = Nothing
		HourGlass(False)
		
		Exit Function
on_err: 
		st = Err.Number
		CriticalAlarm(GetErrorDetails(Cn))
		Resume exit_proc
	End Function
	
	'選択したコードを送るコントロールをセット
	WriteOnly Property ResParentForm() As System.Windows.Forms.Form
		Set(ByVal Value As System.Windows.Forms.Form)
			pParentForm = Value
		End Set
	End Property
	
	'表示項目をセット
	WriteOnly Property MituNo() As Integer
		Set(ByVal Value As Integer)
			pMituNo = Value
		End Set
	End Property
End Class