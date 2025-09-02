Option Strict Off
Option Explicit On

Imports FarPoint.Win.Spread
Imports Microsoft.Office.Interop.Excel

''' <summary>
''' --------------------------------------------------------------------
'''   ユーザー名           株式会社三和商研
'''   業務名               積算データ管理システム
'''   部門名               見積部門
'''   プログラム名         仕分員数リスト出力画面（ＥＸＣＥＬ）
'''   作成会社             テクノウェア株式会社
'''   作成日               2003/06/16
'''   作成者               kawamura
''' --------------------------------------------------------------------
'''    UPDATE
'''        2011/03/14  oosawa      マジックナンバーで持っているのでずれていた
''' --------------------------------------------------------------------
''' </summary>
Friend Class SnwMT02F03
	Inherits System.Windows.Forms.Form

	'Private Const Col仕分数1 As Integer = 49
	Private Const Col仕分数1 As Integer = SnwMT01B01.Col仕分数1 '2018/05/03 ADD

	Const ProfileKey As String = "仕分員数リスト出力"
	Const DefaultTemplateFile As String = "Template_仕分員数リスト.xlsx"
	Dim GXLSPath As String '出力パス

	Dim ReturnF As Boolean 'リターンキー時（確定時）True
	Dim SelectF As Boolean
	Dim PreviousControl As System.Windows.Forms.Control 'コントロールの戻りを制御
	Dim sql As String '抽出ＳＱＬセット
	'Dim rs(0 To 1)      As adodb.Recordset      '抽出レコードセット

	Dim ArySpd(,) As Object 'ｽﾌﾟﾚｯﾄﾞの配列

	Dim pParentForm As SnwMT02F00
	Dim pMituNo As Integer
	Dim pMituNM As String

	Dim pSpd As FpSpread

	Private Sub SnwMT02F03_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Me.Load

		'フォームを画面の中央に配置
		Me.Top = VB6Conv.TwipsToPixelsY((VB6Conv.PixelsToTwipsY(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height) - VB6Conv.PixelsToTwipsY(Me.Height)) \ 2)
		Me.Left = VB6Conv.TwipsToPixelsX((VB6Conv.PixelsToTwipsX(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width) - VB6Conv.PixelsToTwipsX(Me.Width)) \ 2)

		rf_見積番号.Text = pMituNo.ToString("#")

		'出力先情報セット
		'UPGRADE_WARNING: オブジェクト GetIni() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		GXLSPath = GetIni("Path", ProfileKey, INIFile)
		'UPGRADE_WARNING: TextBox プロパティ txDir.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
		txDir.Text = CompactPathEx(GXLSPath, txDir.MaxLength)

		''    Set rs(0) = New adodb.Recordset
		''    Set rs(1) = New adodb.Recordset
	End Sub

	Private Sub SnwMT02F03_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
		' Graphics オブジェクトを取得
		Dim g As Graphics = e.Graphics

		' DPIを取得
		Dim dpiX As Single = g.DpiX ' 水平方向のDPI
		Dim dpiY As Single = g.DpiY ' 垂直方向のDPI

		' 1ポイントをピクセルに変換
		Dim lineWidth As Single = 1 * (dpiX / 72)

		Dim pen As New Pen(Color.Black, lineWidth)

		' 線を描く、見積番号
		g.DrawLine(pen, 12, 26, 150, 26)

		' リソースを解放
		pen.Dispose()
	End Sub

	Private Sub SnwMT02F03_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
		Dim Cancel As Boolean = e.Cancel
		Dim UnloadMode As System.Windows.Forms.CloseReason = e.CloseReason
		'    ReleaseRs rs(0)
		'    ReleaseRs rs(1)
		pParentForm.Enabled = True
		pParentForm.Activate()
		'UPGRADE_NOTE: オブジェクト pParentForm をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		pParentForm = Nothing
		e.Cancel = Cancel
		Me.Dispose()
	End Sub

	Private Sub Cb中止_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Cb中止.Click
		Me.Close()
	End Sub

	Private Sub CbXLS_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CbXLS.Enter
		If Item_Check((CbXLS.TabIndex)) = False Then
			Exit Sub
		End If
	End Sub

	Private Sub CbXLS_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CbXLS.Click
		Dim st As Integer
		Dim XLSPath As String
		XLSPath = ""
		If txDir.Text = vbNullString Then
			'        CriticalAlarm "ファイルを指定して下さい。"
			CriticalAlarm("フォルダを指定して下さい。")
			Cb変更.Focus()
			Exit Sub
		End If

		If MsgBoxResult.No = YesNo("仕分員数リスト（仕分レベル毎）をExcelBookファイルに出力します。" & vbCrLf & vbCrLf & "※既にファイルが存在している場合はすべて上書きされます。") Then Exit Sub
		st = 0
		If Download() = False Then
			st = -1
			Inform("該当データ無し")
			CbXLS.Focus()
		Else
			'フォルダ名セット(指定フォルダ＋現場名＋オペ日)
			If Strings.Right(GXLSPath, 1) <> "\" Then
				XLSPath = GXLSPath & "\"
			End If
			'        GXLSPath = CutOutPath(txDir) & "\" & HD_現場名 & Format$(Date, "yyyymmdd")
			'        XLSPath = XLSPath & HD_現場名 & Format$(Date, "yyyymmdd")
			'2004/03/29 ADD
			XLSPath = XLSPath & HD_見積件名 & Today.ToString("yyyyMMdd")
			'フォルダ作成
			st = MKFolder(XLSPath)
			If st <> 0 Then
				Exit Sub
			End If
			'EXCELファイル出力
			st = 仕分員数リスト出力(XLSPath)
		End If
		If st = 0 Then
			Inform("出力を完了しました。")
			Me.Close()
		End If
	End Sub

	Private Sub Cb変更_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Cb変更.Click
		Dim Buf As String

		If Item_Check((Cb変更.TabIndex)) = False Then Exit Sub
		'    If OpenSaveDialog(GXLSPath, ProfileKey, "xls") = True Then
		'---2003.09.26.DEL-------
		'    If OpenSaveDialog(GXLSPath, "仕分", "xls") = True Then
		'        WriteIni "Path", ProfileKey, GXLSPath, INIFile
		'    End If
		'    txDir = CompactPathEx(GXLSPath, txDir.MaxLength)
		'---2003.09.26.ADD-------
		Buf = Y_GetFolder(Me.Handle, "フォルダを作成する場所を選択して下さい。", "")
		If Buf <> vbNullString Then
			GXLSPath = Buf
			WriteIni("Path", ProfileKey, GXLSPath, INIFile)
		End If
		'UPGRADE_WARNING: TextBox プロパティ txDir.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
		txDir.Text = CompactPathEx(GXLSPath, txDir.MaxLength)
		'------------------------
	End Sub

	Private Sub Tx_FromNo_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Tx_FromNo.Enter
		PreviousControl = Me.ActiveControl
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "開始仕分番号を入力して下さい。"
	End Sub

	Private Sub Tx_FromNo_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Tx_FromNo.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
	End Sub

	Private Sub Tx_ToNo_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Tx_ToNo.Enter
		PreviousControl = Me.ActiveControl
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "終了仕分番号を入力して下さい。"
	End Sub

	Private Sub Tx_ToNo_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Tx_ToNo.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
	End Sub

	Private Function Item_Check(ByRef ItemNo As Integer) As Boolean
		Dim bufName As String '名前取得用バッファ
		Dim Chk_ID As String 'ﾁｪｯｸ用ワーク

		On Error GoTo Item_Check_Err
		Item_Check = False

		Item_Check = True

		Exit Function
Item_Check_Err:
		MsgBox(Err.Number & " " & Err.Description)
	End Function

	Private Function Download() As Boolean
		Dim whr As String

		Download = False

		'マウスポインターを砂時計にする
		HourGlass(True)

		'親スプレッドに明細がない場合
		If pSpd.ActiveSheet.NonEmptyRowCount = 0 Then
			''        CriticalAlarm "明細がありません。"
			HourGlass(False)
			Exit Function
		End If


		With pSpd
			'UPGRADE_WARNING: 配列 ArySpd の下限が 1,1 から 0,0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"' をクリックしてください。
			'UPGRADE_ISSUE: As Variant が ReDim ArySpd(1 To pSpd.DataRowCnt, 1 To pSpd.MaxCols) ステートメントから削除されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="19AFCB41-AA8E-4E6B-A441-A3E802E5FD64"' をクリックしてください。
			ReDim ArySpd(.ActiveSheet.NonEmptyRowCount, .ActiveSheet.ColumnCount) '(1 to 30,1 to 4)
			ArySpd = .ActiveSheet.GetArray(0, 0, .ActiveSheet.RowCount, .ActiveSheet.ColumnCount)
		End With
		'    Erase gSiwakeTBL()
		'    ReDim gSiwakeTBL(0 To UBound(tbls, 2) - 1, 0 To UBound(tbls) - 1)
		'
		'    For i = 1 To UBound(tbls)
		'        For j = 1 To UBound(tbls, 2)
		'            gSiwakeTBL(j - 1, i - 1) = tbls(i, j)
		'        Next
		'    Next


		''    '---見積シート明細情報
		''    SQL = "SELECT SP区分, PC区分, 製品NO, 仕様NO, ベース色, 漢字名称, W, D, H, D1, D2, " _
		''        & "仕入単価, 売上単価, 見積数量, 単位名, 売上金額, 仕入先CD, 配送先CD, " _
		''        & "社内在庫数 , 客先在庫数, 転用数, 総数量 " _
		''        & "FROM TD見積シートM " _
		''        & "WHERE 見積番号 = " & pMituNo _
		''        & " ORDER BY 行番号"
		''
		''    Set rs(0) = OpenRs(SQL, Cn, adOpenStatic, adLockReadOnly)
		''
		''    With rs(0)
		''        If .EOF Then
		''            ReleaseRs rs(0)
		''            HourGlass False
		''            Exit Function
		''        End If
		''    End With
		''
		''    '抽出条件セット
		''    whr = SQLIntRange("仕分番号", tx_FromNo, tx_ToNo, DBType, True)
		''
		''    '--見積シート内訳名称情報
		''    SQL = "SELECT U.見積番号, U.仕分番号, U.行番号, U.総数量, UM.名称, CM.件数" _
		''        & " FROM" _
		''        & " (SELECT 見積番号, 行番号, 仕分番号, SUM(数量) AS 総数量" _
		''        & " FROM TD見積シート内訳" _
		''        & " WHERE 見積番号 = " & pMituNo & whr _
		''        & " GROUP BY 見積番号, 行番号, 仕分番号" _
		''        & " ) AS U" _
		''        & " LEFT JOIN" _
		''        & " (SELECT 見積番号, 仕分番号, 名称" _
		''        & " FROM TD見積シート内訳名称" _
		''        & " WHERE 見積番号 = " & pMituNo & whr _
		''        & " ) AS UM" _
		''        & " ON U.見積番号 = UM.見積番号 AND U.仕分番号 = UM.仕分番号" _
		''        & " LEFT JOIN" _
		''        & " (SELECT 見積番号, COUNT(*) as 件数" _
		''        & " FROM" _
		''        & " (SELECT 見積番号, 仕分番号" _
		''        & " FROM TD見積シート内訳" _
		''        & " WHERE 見積番号 = " & pMituNo & whr _
		''        & " GROUP BY 見積番号, 仕分番号" _
		''        & " ) AS D" _
		''        & " GROUP BY 見積番号" _
		''        & " ) AS CM" _
		''        & " ON U.見積番号 = CM.見積番号" _
		''        & " ORDER BY U.仕分番号, U.行番号"
		''
		''    Set rs(1) = OpenRs(SQL, Cn, adOpenStatic, adLockReadOnly)
		''
		''    With rs(1)
		''        If .EOF Then
		''            ReleaseRs rs(1)
		''        Else
		''            Download = True
		''        End If
		''    End With

		Download = True

		On Error GoTo 0
		HourGlass(False)
	End Function

	Private Function 仕分員数リスト出力(ByRef OutFile As String) As Integer
		Dim st As Integer
		'UPGRADE_NOTE: xl は xl_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
		Dim xl_Renamed As Object 'Excel.Application
		Dim TemplateFile As String
		Dim OriginalCalcMode As Integer 'XlCalculation
		Dim wkb As Workbook = Nothing 'Excel.Workbook
		Dim twkb As Workbook 'Excel.Workbook
		Dim twks As Worksheet 'Excel.Worksheet
		Dim wks As Worksheet 'Excel.Worksheet
		Dim wksMe() As Worksheet 'Excel.Worksheet

		Dim Core As Range 'Excel.Range
		Dim CoreRows, CoreColumns As Integer
		Dim PrintTitleRows, PrintTitleColumns As String

		Dim RecCnt As Integer
		Dim i, j As Integer
		Dim r As Integer
		Dim LCNT As Integer
		Dim BaseOutFile As String
		Dim BookFileNM As String
		Dim Siwake As Integer
		'仕分番号の変数
		Dim FromNO As Integer
		Dim ToNO As Integer
		Dim SwapNO As Integer

		If Tx_FromNo.Text = vbNullString Then
			FromNO = 1
		Else
			FromNO = CInt(Tx_FromNo.Text)
		End If

		If Tx_ToNo.Text = vbNullString Then
			ToNO = 30
		Else
			ToNO = CInt(Tx_ToNo.Text)
		End If

		If FromNO > ToNO Then
			SwapNO = FromNO
			FromNO = ToNO
			ToNO = SwapNO
		End If

		'    BaseOutFile = CutOutPath(OutFile) & "\仕分" & pMituNo & "_"
		BaseOutFile = OutFile & "\"

		'処理メッセージ準備
		System.Windows.Forms.Application.DoEvents()
		'マウスポインターを砂時計にする
		Me.Enabled = False
		HourGlass(True)
		'メッセージを表示
		Fw_Msg.Text = Me.Text
		'VB6.ShowForm(Fw_Msg, VB6.FormShowConstants.Modeless, Me)
		Fw_Msg.Show(Me)
		Fw_Msg.Refresh()

		'戻り値をリセットします
		st = 0
		'*---処理メッセージ表示
		Fw_Msg.StsCaption = "作成中..."

		'テンプレートのファイル名を準備します
		'UPGRADE_WARNING: オブジェクト GetIni() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		TemplateFile = GetIni("Template", ProfileKey, INIFile)
		If TemplateFile = "" Then
			TemplateFile = AppPath(DefaultTemplateFile)
		End If

		'Excel.Application クラスオブジェクトを開きます
		xl_Renamed = xlOpen()
		If xl_Renamed Is Nothing Then
			st = -1
			GoTo exit_proc
		End If
		'Templateのブックを開きます
		wkb = xlOpenBook(xl_Renamed, TemplateFile)
		If wkb Is Nothing Then
			st = -2
			GoTo exit_proc
		End If
		'自動再計算を禁止します
		'UPGRADE_WARNING: オブジェクト xl_Renamed.Calculation の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		OriginalCalcMode = xl_Renamed.Calculation
		'UPGRADE_WARNING: オブジェクト xl_Renamed.Calculation の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		xl_Renamed.Calculation = -4135 'xlCalculationManual

		'テンプレートのシートを変数にセットします
		'UPGRADE_WARNING: オブジェクト wkb.Worksheets の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		twks = wkb.Worksheets(1)

		'中止ボタンの確認
		System.Windows.Forms.Application.DoEvents()
		If Fw_Msg.AbortDoing = True Then GoTo Abort

		'xl.Visible = True

		On Error GoTo on_err 'エラートラップを開始します

		'見積ヘッダー情報をセットします
		'UPGRADE_WARNING: オブジェクト xl_Renamed.Range の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'xl_Renamed.Range("見積番号") = pMituNo
		twks.Range("H2").Value = pMituNo
		'UPGRADE_WARNING: オブジェクト xl_Renamed.Range の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'xl_Renamed.Range("見積名称") = pMituNM
		twks.Range("K2").Value = pMituNM
		'作成日をセットします
		'UPGRADE_WARNING: オブジェクト xl_Renamed.Range の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'xl_Renamed.Range("作成日") = "作成日： " & Today.ToString("yyyy/MM/dd")
		twks.Range("M1").Value = "作成日： " & Today.ToString("yyyy/MM/dd")

		'*---処理メッセージ表示
		Fw_Msg.StsCaption = "データを抽出しています。"

		'表の雛形セル範囲を変数にセットします
		'UPGRADE_WARNING: オブジェクト xl_Renamed.Range の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Core = xl_Renamed.Range("CoreCells")
		'UPGRADE_WARNING: オブジェクト Core.ROWS の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		CoreRows = Core.ROWS.Count
		'UPGRADE_WARNING: オブジェクト Core.Columns の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		CoreColumns = Core.Columns.Count

		'印刷タイトル範囲を設定します
		With twks
			'        PrintTitleRows = .Range(.Range("Print_Titles").Address).EntireRow.Address
			'        PrintTitleColumns = .Range("$A$1:" & SumCore.Cells(1, 1).address).EntireColumn.address
		End With

		'レコードカウント取得
		'    With rs(0)
		'        RecCnt = .RecordCount
		RecCnt = UBound(ArySpd)
		'    End With

		'プログレスバーの値セット
		Fw_Msg.ProgValue = 0
		Fw_Msg.ProgMax = RecCnt

		'タイトル行の設定(明細開始行)
		LCNT = 6
		'CoreColumns = 16384

		With twks
			'UPGRADE_WARNING: オブジェクト twks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト twks.Range の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'.Range(.Cells(LCNT + 1, 1).Address, .Cells(LCNT + SPMaxRow - 2, CoreColumns).Address).EntireRow.Insert()
			.Range(.Cells(LCNT + 1, 1).Address, .Cells(LCNT + SPMaxRow - 2, CoreColumns).Address).EntireRow.Insert
			'セル内容を行コピーします
			'UPGRADE_WARNING: オブジェクト twks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト twks.Range の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'.Range(.Cells(LCNT, 1).Address, .Cells(LCNT + SPMaxRow - 1, CoreColumns).Address).filldown()
			.Range(.Cells(LCNT, 1).Address, .Cells(LCNT + SPMaxRow - 1, CoreColumns).Address).FillDown
			'        If RecCnt > 2 Then
			'            '不足している行を挿入します
			'            .Range(.Cells(LCNT + 1, 1).Address, .Cells(LCNT + RecCnt - 2, CoreColumns).Address).EntireRow.Insert
			'            'セル内容を行コピーします
			'            .Range(.Cells(LCNT, 1).Address, .Cells(LCNT + RecCnt - 1, CoreColumns).Address).filldown
			'        ElseIf RecCnt = 2 Then
			'             'セル内容を行コピーします
			'            .Range(.Cells(LCNT, 1).Address, .Cells(LCNT + RecCnt - 1, CoreColumns).Address).filldown
			'        ElseIf RecCnt < 2 Then
			'            'ダミー行が残ってしまうので削除しておきます
			'            .Range(.Cells(LCNT + 1, 1).Address, .Cells(LCNT + 1, CoreColumns).Address).EntireRow.Delete
			'        End If
		End With

		'読み込みカウント
		r = 0
		'*---処理メッセージ表示
		Fw_Msg.StsCaption = "員数シート情報をセットしています。"

		'集計値をセルにセットします
		With twks
			For i = 0 To UBound(ArySpd)
				'中止ボタンの確認
				System.Windows.Forms.Application.DoEvents()
				If Fw_Msg.AbortDoing = True Then GoTo Abort

				'員数シート情報セット
				'UPGRADE_WARNING: オブジェクト twks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト ArySpd() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Cells(r + LCNT, 1) = ArySpd(i, Col見積区分) '見積区分
				'2016/06/22 ADD↓
				'UPGRADE_WARNING: オブジェクト twks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト ArySpd() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Cells(r + LCNT, 2) = ArySpd(i, ColSP区分) 'SP区分
				'UPGRADE_WARNING: オブジェクト twks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト ArySpd() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Cells(r + LCNT, 3) = ArySpd(i, ColPC区分) 'PC区分
				'UPGRADE_WARNING: オブジェクト twks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト ArySpd() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Cells(r + LCNT, 4) = ArySpd(i, Col製品NO) '製品NO
				'UPGRADE_WARNING: オブジェクト twks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト ArySpd() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Cells(r + LCNT, 5) = ArySpd(i, Col仕様NO) '仕様NO
				'UPGRADE_WARNING: オブジェクト twks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト ArySpd() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Cells(r + LCNT, 6) = ArySpd(i, Colベース色) 'ベース色
				'UPGRADE_WARNING: オブジェクト twks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト ArySpd() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Cells(r + LCNT, 7) = ArySpd(i, Col他社伝票番号) '他社伝票番号
				'UPGRADE_WARNING: オブジェクト twks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト ArySpd() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Cells(r + LCNT, 8) = ArySpd(i, Col名称) '漢字名称
				'UPGRADE_WARNING: オブジェクト twks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト ArySpd() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Cells(r + LCNT, 9) = If(ArySpd(i, ColW) = "", "", ArySpd(i, ColW)) 'W
				'UPGRADE_WARNING: オブジェクト twks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト ArySpd() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Cells(r + LCNT, 10) = If(ArySpd(i, ColD) = "", "", ArySpd(i, ColD)) 'H
				'UPGRADE_WARNING: オブジェクト twks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト ArySpd() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Cells(r + LCNT, 11) = If(ArySpd(i, ColH) = "", "", ArySpd(i, ColH)) 'D
				'UPGRADE_WARNING: オブジェクト twks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト ArySpd() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Cells(r + LCNT, 12) = If(ArySpd(i, ColD1) = "", "", ArySpd(i, ColD1)) 'D1
				'UPGRADE_WARNING: オブジェクト twks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト ArySpd() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Cells(r + LCNT, 13) = If(ArySpd(i, ColD2) = "", "", ArySpd(i, ColD2)) 'D2
				'UPGRADE_WARNING: オブジェクト twks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト ArySpd() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Cells(r + LCNT, 14) = If(ArySpd(i, ColH1) = "", "", ArySpd(i, ColH1)) 'H1
				'UPGRADE_WARNING: オブジェクト twks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト ArySpd() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Cells(r + LCNT, 15) = If(ArySpd(i, ColH2) = "", "", ArySpd(i, ColH2)) 'H2
				'2016/06/22 ADD↑
				'2016/06/22 DEL↓
				'            .Cells(r + LCNT, 2) = ArySpd(i, 5)      'SP区分
				'            .Cells(r + LCNT, 3) = ArySpd(i, 6)      'PC区分
				'            .Cells(r + LCNT, 4) = ArySpd(i, 7)      '製品NO
				'            .Cells(r + LCNT, 5) = ArySpd(i, 8)      '仕様NO
				'            .Cells(r + LCNT, 6) = ArySpd(i, 9)      'ベース色
				'            .Cells(r + LCNT, 7) = ArySpd(i, 3)      '他社伝票番号
				'            .Cells(r + LCNT, 8) = ArySpd(i, 10)      '漢字名称
				'            .Cells(r + LCNT, 9) = IIf(ArySpd(i, 11) = 0, "", ArySpd(i, 11))       'W
				'            .Cells(r + LCNT, 10) = IIf(ArySpd(i, 12) = 0, "", ArySpd(i, 12))       'H
				'            .Cells(r + LCNT, 11) = IIf(ArySpd(i, 13) = 0, "", ArySpd(i, 13))     'D
				'            .Cells(r + LCNT, 12) = IIf(ArySpd(i, 14) = 0, "", ArySpd(i, 14))    'D1
				'            .Cells(r + LCNT, 13) = IIf(ArySpd(i, 15) = 0, "", ArySpd(i, 15))    'D2
				'            .Cells(r + LCNT, 14) = IIf(ArySpd(i, 16) = 0, "", ArySpd(i, 16))    'H1
				'            .Cells(r + LCNT, 15) = IIf(ArySpd(i, 17) = 0, "", ArySpd(i, 17))    'H2
				'2016/06/22 DEL↑
				'-------------------
				'---'2005/10/14.ADD U区分追加 ※以下の項目はプラス１(Record側のみ)
				'            .Cells(r + LCNT, 15) = IIf(ArySpd(i, 18) = 0, "", ArySpd(i, 18))    '仕入単価
				'            .Cells(r + LCNT, 16) = IIf(ArySpd(i, 20) = 0, "", ArySpd(i, 20))    '売上単価
				'            .Cells(r + LCNT, 17) = IIf(ArySpd(i, 23) = 0, "", ArySpd(i, 23))    '見積数量
				'            .Cells(r + LCNT, 18) = ArySpd(i, 24)                                '単位名
				'            .Cells(r + LCNT, 19) = IIf(ArySpd(i, 25) = 0, "", ArySpd(i, 25))    '売上金額
				'            .Cells(r + LCNT, 20) = ArySpd(i, 29)                                '仕入先CD
				'            .Cells(r + LCNT, 21) = ArySpd(i, 30)                                '仕入先名
				'            .Cells(r + LCNT, 22) = ArySpd(i, 31)                                '配送先CD
				'            .Cells(r + LCNT, 23) = ArySpd(i, 32)                                '配送先名
				'            .Cells(r + LCNT, 24) = IIf(ArySpd(i, 33) = 0, "", ArySpd(i, 33))    '社内在庫数
				'            .Cells(r + LCNT, 25) = IIf(ArySpd(i, 34) = 0, "", ArySpd(i, 34))    '客先在庫数
				'            .Cells(r + LCNT, 26) = IIf(ArySpd(i, 35) = 0, "", ArySpd(i, 35))    '転用数
				'2016/06/22 ADD↓
				'UPGRADE_WARNING: オブジェクト twks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト ArySpd() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Cells(r + LCNT, 16) = If(SpcToNull(ArySpd(i, Col原価)) = "", "", CDbl(ArySpd(i, Col原価))) '仕入単価
				'UPGRADE_WARNING: オブジェクト twks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト ArySpd() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Cells(r + LCNT, 17) = If(SpcToNull(ArySpd(i, Col売価)) = "", "", CDbl(ArySpd(i, Col売価))) '売上単価
				'UPGRADE_WARNING: オブジェクト twks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト ArySpd() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Cells(r + LCNT, 18) = If(NullToZero(ArySpd(i, Col見積数量)) = 0, "", CDbl(ArySpd(i, Col見積数量))) '見積数量
				'UPGRADE_WARNING: オブジェクト twks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト ArySpd() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Cells(r + LCNT, 19) = ArySpd(i, Col単位) '単位名
				'UPGRADE_WARNING: オブジェクト twks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト ArySpd() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Cells(r + LCNT, 20) = If(SpcToNull(ArySpd(i, Col金額)) = "", "", CDbl(ArySpd(i, Col金額))) '売上金額
				'UPGRADE_WARNING: オブジェクト twks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト ArySpd() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Cells(r + LCNT, 21) = ArySpd(i, Col仕入先CD) '仕入先CD
				'UPGRADE_WARNING: オブジェクト twks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト ArySpd() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Cells(r + LCNT, 22) = ArySpd(i, Col仕入先名) '仕入先名
				'UPGRADE_WARNING: オブジェクト twks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト ArySpd() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Cells(r + LCNT, 23) = ArySpd(i, Col送り先CD) '配送先CD
				'UPGRADE_WARNING: オブジェクト twks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト ArySpd() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Cells(r + LCNT, 24) = ArySpd(i, Col送り先名) '配送先名
				'UPGRADE_WARNING: オブジェクト twks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト ArySpd() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Cells(r + LCNT, 25) = If(NullToZero(ArySpd(i, Col社内在庫)) = 0, "", CDbl(ArySpd(i, Col社内在庫))) '社内在庫数
				'UPGRADE_WARNING: オブジェクト twks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト ArySpd() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Cells(r + LCNT, 26) = If(NullToZero(ArySpd(i, Col客先在庫)) = 0, "", CDbl(ArySpd(i, Col客先在庫))) '客先在庫数 '2011/03/14 ADD
				'UPGRADE_WARNING: オブジェクト twks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト ArySpd() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Cells(r + LCNT, 27) = If(NullToZero(ArySpd(i, Col転用)) = 0, "", CDbl(ArySpd(i, Col転用))) '転用数     '2011/03/14 ADD
				'2016/06/22 ADD↑
				'2016/06/22 DEL↓
				'            .Cells(r + LCNT, 16) = IIf(ArySpd(i, 21) = 0, "", ArySpd(i, 21))    '仕入単価
				'            .Cells(r + LCNT, 17) = IIf(ArySpd(i, 23) = 0, "", ArySpd(i, 23))    '売上単価
				'            .Cells(r + LCNT, 18) = IIf(ArySpd(i, 26) = 0, "", ArySpd(i, 26))    '見積数量
				'            .Cells(r + LCNT, 19) = ArySpd(i, 27)                                '単位名
				'            .Cells(r + LCNT, 20) = IIf(ArySpd(i, 28) = 0, "", ArySpd(i, 28))    '売上金額
				'            .Cells(r + LCNT, 21) = ArySpd(i, 32)                                '仕入先CD
				'            .Cells(r + LCNT, 22) = ArySpd(i, 33)                                '仕入先名
				'            .Cells(r + LCNT, 23) = ArySpd(i, 34)                                '配送先CD
				'            .Cells(r + LCNT, 24) = ArySpd(i, 35)                                '配送先名
				'            .Cells(r + LCNT, 25) = IIf(ArySpd(i, 36) = 0, "", ArySpd(i, 36))    '社内在庫数
				''            .Cells(r + LCNT, 25) = IIf(ArySpd(i, 35) = 0, "", ArySpd(i, 35))    '客先在庫数
				''            .Cells(r + LCNT, 26) = IIf(ArySpd(i, 36) = 0, "", ArySpd(i, 36))    '転用数
				'            .Cells(r + LCNT, 26) = IIf(ArySpd(i, 38) = 0, "", ArySpd(i, 38))    '客先在庫数 '2011/03/14 ADD
				'            .Cells(r + LCNT, 27) = IIf(ArySpd(i, 40) = 0, "", ArySpd(i, 40))    '転用数     '2011/03/14 ADD
				'2016/06/22 DEL↑

				r += 1
				'プログレスバーの値セット
				Fw_Msg.ProgValue = r

			Next
		End With

		Fw_Msg.ProgValue = RecCnt

		'プログレスバーの値セット
		Fw_Msg.ProgValue = 0
		'    Fw_Msg.ProgMax = rs(1)![件数]
		Fw_Msg.ProgMax = ToNO - FromNO + 1

		'    i = 0
		'仕分名称毎にBookを追加・保存します。
		For i = FromNO - 1 To ToNO - 1

			'*---処理メッセージ表示
			Fw_Msg.StsCaption = "仕分情報をセットしています。"
			'中止ボタンの確認
			System.Windows.Forms.Application.DoEvents()
			If Fw_Msg.AbortDoing = True Then GoTo Abort

			'        Siwake = rs(1)![仕分番号]
			'        i = i + 1

			'シートを新しいBOOKにコピーします
			'UPGRADE_WARNING: オブジェクト twks.Copy の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			twks.Copy()
			'新しいBOOKを変数にセットします
			'UPGRADE_WARNING: オブジェクト xl_Renamed.Workbooks の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			twkb = xl_Renamed.Workbooks(xl_Renamed.Workbooks.Count)
			'新しいBOOKのシートを変数にセットします
			'UPGRADE_WARNING: オブジェクト twkb.Worksheets の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			wks = twkb.Worksheets(twkb.Worksheets.Count)

			'追加したシートの設定
			'UPGRADE_WARNING: IsEmpty は、IsNothing にアップグレードされ、新しい動作が指定されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
			If Not IsNothing(gSiwakeTBL) Then
				If UBound(gSiwakeTBL, 2) >= i Then
					'UPGRADE_WARNING: オブジェクト gSiwakeTBL(0, i) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					If gSiwakeTBL(0, i) = vbNullString Then
						'UPGRADE_WARNING: オブジェクト wks.Name の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						wks.Name = CStr(i + 1)
						BookFileNM = CStr(i + 1)
						'UPGRADE_WARNING: オブジェクト xl_Renamed.Range の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						wks.Range("仕分レベル").Value = CStr(i + 1)
						'UPGRADE_WARNING: オブジェクト xl_Renamed.Range の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						wks.Range("仕分名称").Value = CStr(i + 1)
					Else
						'UPGRADE_WARNING: オブジェクト wks.Name の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						wks.Name = NullToZero(gSiwakeTBL(0, i), " ")
						'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						BookFileNM = NullToZero(gSiwakeTBL(0, i), " ")
						'UPGRADE_WARNING: オブジェクト xl_Renamed.Range の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						wks.Range("仕分レベル").Value = CStr(i + 1)
						'UPGRADE_WARNING: オブジェクト xl_Renamed.Range の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						wks.Range("仕分名称").Value = NullToZero(gSiwakeTBL(0, i), " ")
					End If
				Else
					'UPGRADE_WARNING: オブジェクト wks.Name の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					wks.Name = CStr(i + 1)
					BookFileNM = CStr(i + 1)
					'UPGRADE_WARNING: オブジェクト xl_Renamed.Range の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					wks.Range("仕分レベル").Value = CStr(i + 1)
					'UPGRADE_WARNING: オブジェクト xl_Renamed.Range の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					wks.Range("仕分名称").Value = CStr(i + 1)
				End If
			Else
				'UPGRADE_WARNING: オブジェクト wks.Name の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wks.Name = CStr(i + 1)
				BookFileNM = CStr(i + 1)
				'UPGRADE_WARNING: オブジェクト xl_Renamed.Range の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wks.Range("仕分レベル").Value = CStr(i + 1)
				'UPGRADE_WARNING: オブジェクト xl_Renamed.Range の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wks.Range("仕分名称").Value = CStr(i + 1)
			End If
			'総仕分数をセット
			For j = 1 To UBound(ArySpd)


				'            wks.Cells(j + LCNT - 1, 28) = ArySpd(j, 38 + i)
				'            wks.Cells(j + LCNT - 1, 28) = ArySpd(j, 39 + i)         '2005/10/14
				'UPGRADE_WARNING: オブジェクト wks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト ArySpd() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wks.Cells(j + LCNT - 1, 29) = ArySpd(j - 1, Col仕分数1 + i) '2011/03/14 ADD

			Next

			''        'BOOKファイル名セット
			''        BookFileNM = Format$(Str(i + 1), "00")
			'タイトル行をセット
			'        wks.PageSetup.PrintTitleRows = PrintTitleRows
			'テンプレートシートへの名前参照のごみが残らないように予め定義名を削除しておきます
			'UPGRADE_WARNING: オブジェクト twkb.names の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'For j = twkb.names.Count To 1 Step -1
			'	'UPGRADE_WARNING: オブジェクト twkb.names の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'	If InStr(twkb.names(j).Name, "Print_Titles") = 0 Then
			'		'UPGRADE_WARNING: オブジェクト twkb.names の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'		twkb.names(j).Delete()
			'	End If
			'Next
			For Each nameObj As Name In twkb.Names
				If Not nameObj.Name.Contains("Print_Titles") Then
					nameObj.Delete()
				End If
			Next

			'*---処理メッセージ表示
			Fw_Msg.StsCaption = "保存しています。"

			'再計算
			'UPGRADE_WARNING: オブジェクト xl_Renamed.Calculate の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			xl_Renamed.Calculate()
			'UPGRADE_WARNING: オブジェクト xl_Renamed.Calculation の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			xl_Renamed.Calculation = OriginalCalcMode '再計算モードを復旧します
			'最初のシートを選択しておきます
			'UPGRADE_WARNING: オブジェクト twkb.Worksheets の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			twkb.Worksheets(1).Activate()

			'名前を付けて保存します
			'UPGRADE_WARNING: オブジェクト twkb.SaveAs の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'twkb.SaveAs(BaseOutFile & BookFileNM & ".xls", -4143) 'xlWorkbookNormal
			twkb.SaveAs(BaseOutFile & BookFileNM & ".xlsx", 51) 'xlWorkbookNormal

			xlCloseBook(twkb) 'ワークブックを閉じます

			'プログレスバーの値セット
			Fw_Msg.ProgValue = i + 1 - FromNO

		Next
		''    Do
		''
		''        '中止ボタンの確認
		''        DoEvents
		''        If Fw_Msg.AbortDoing = True Then GoTo Abort
		''
		''        Siwake = rs(1)![仕分番号]
		''        i = i + 1
		''
		''        'シートを新しいBOOKにコピーします
		''        twks.Copy
		''        '新しいBOOKを変数にセットします
		''        Set twkb = xl.Workbooks(xl.Workbooks.Count)
		''        '新しいBOOKのシートを変数にセットします
		''        Set wks = twkb.Worksheets(twkb.Worksheets.Count)
		''
		''        '追加したシートの設定
		''        If rs(1)![名称] = vbNullString Then
		''            wks.Name = Str(rs(1)![仕分番号])
		''            xl.Range("仕分レベル") = Str(rs(1)![仕分番号])
		''            xl.Range("仕分名称") = Str(rs(1)![仕分番号])
		''        Else
		''            wks.Name = NullToZero(rs(1)![名称], " ")
		''            xl.Range("仕分レベル") = Str(rs(1)![仕分番号])
		''            xl.Range("仕分名称") = NullToZero(rs(1)![名称], " ")
		''        End If
		''
		''        '総仕分数をセット
		''        Do
		''
		''            wks.Cells(rs(1)![行番号] + LCNT - 1, 23) = rs(1)![総数量]
		''
		''            rs(1).MoveNext
		''        If rs(1).EOF Then Exit Do
		''        Loop While (rs(1)![仕分番号] = Siwake)
		''
		''        'BOOKファイル名セット
		''        BookFileNM = Format$(Str(Siwake), "00")
		''        'タイトル行をセット
		''        wks.PageSetup.PrintTitleRows = PrintTitleRows
		''        'テンプレートシートへの名前参照のごみが残らないように予め定義名を削除しておきます
		''        For j = twkb.names.Count To 1 Step -1
		''            If InStr(twkb.names(j).Name, "Print_Titles") = 0 Then
		''                twkb.names(j).Delete
		''            End If
		''        Next
		''
		''        '再計算
		''        xl.Calculate
		''        xl.Calculation = OriginalCalcMode   '再計算モードを復旧します
		''        '最初のシートを選択しておきます
		''        twkb.Worksheets(1).Activate
		''
		''        '名前を付けて保存します
		''        twkb.SaveAs BaseOutFile & BookFileNM & ".xls", -4143       'xlWorkbookNormal
		''
		''        xlCloseBook twkb     'ワークブックを閉じます
		''
		''        'プログレスバーの値セット
		''        Fw_Msg.ProgValue = i
		''
		''    Loop Until rs(1).EOF

		''    Fw_Msg.ProgValue = rs(1).RecordCount
		Fw_Msg.ProgValue = ToNO - FromNO + 1
		'*---処理メッセージ表示
		Fw_Msg.StsCaption = "終了しています。"

		GoTo exit_proc
Abort:
		'*---処理メッセージ表示
		Fw_Msg.StsCaption = "中止しています．．．"
		st = -1
exit_proc:
		On Error GoTo 0 'エラートラップを解除します
		Cn.CursorLocation = ADODB.CursorLocationEnum.adUseServer
		'    ReleaseRs rs(0)
		'    ReleaseRs rs(1)

		'UPGRADE_NOTE: オブジェクト Core をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		Core = Nothing
		'UPGRADE_NOTE: オブジェクト twks をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		twks = Nothing
		'UPGRADE_NOTE: オブジェクト wks をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		wks = Nothing
		If xl_Renamed IsNot Nothing Then
			If OriginalCalcMode <> 0 Then
				'UPGRADE_WARNING: オブジェクト xl_Renamed.Calculation の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				xl_Renamed.Calculation = OriginalCalcMode '再計算モードを復旧します
			End If
			If wkb IsNot Nothing Then
				xlCloseBook(wkb) 'ワークブックを閉じます
				xlClose(xl_Renamed) 'Excel.Applicationオブジェクトを開放します
			End If
		End If

		仕分員数リスト出力 = st

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

	'表示項目をセット
	WriteOnly Property MituNM() As String
		Set(ByVal Value As String)
			pMituNM = Value
		End Set
	End Property

	'員数入力画面のスプレッドをセット
	WriteOnly Property OwnerSpd() As Object
		Set(ByVal Value As Object)
			pSpd = Value
		End Set
	End Property

End Class