Option Strict Off
Option Explicit On

''' <summary>
''' --------------------------------------------------------------------
'''   ユーザー名           株式会社三和商研
'''   業務名               積算データ管理システム
'''   部門名               見積部門
'''   プログラム名         レビットデータ取込画面（ＴＸＴ）
'''   作成会社
'''   作成日               2018/04/10
'''   作成者               oosawa
''' --------------------------------------------------------------------
'''    UPDATE
'''        2018/04/10  oosawa      新設
''' --------------------------------------------------------------------
''' </summary>
Friend Class SnwMT02F15
	Inherits System.Windows.Forms.Form

	Const ProfileKey As String = "レビット取込"
	Dim GXLSPath As String '出力パス

	Dim pParentForm As SnwMT02F00
	Dim pMituNo As Integer

	Dim wMituNo As Integer
	Dim werrCnt As Integer
	Dim wSiwaGyo As Integer
	Dim wTempData As Object

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

	Private Sub SnwMT02F15_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Me.Load
		'フォームを画面の中央に配置
		Me.Top = VB6Conv.TwipsToPixelsY((VB6Conv.PixelsToTwipsY(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height) - VB6Conv.PixelsToTwipsY(Me.Height)) \ 2)
		Me.Left = VB6Conv.TwipsToPixelsX((VB6Conv.PixelsToTwipsX(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width) - VB6Conv.PixelsToTwipsX(Me.Width)) \ 2)

		''    rf_見積番号 = Format$(pMituNo, "#")

		'入力先情報セット
		'UPGRADE_WARNING: オブジェクト GetIni() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		GXLSPath = GetIni("Path", ProfileKey, INIFile)
		'UPGRADE_WARNING: TextBox プロパティ txDir.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
		txDir.Text = CompactPathEx(GXLSPath, txDir.MaxLength)
	End Sub

	Private Sub SnwMT02F15_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
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

	Private Function Item_Check(ByRef ItemNo As Integer) As Boolean

		On Error GoTo Item_Check_Err
		Item_Check = False

		'見積番号のチェック
		If ItemNo > [tx_仕分番号].TabIndex Then
			'UPGRADE_WARNING: オブジェクト NullToZero(tx_仕分番号.Text, 0) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If NullToZero(([tx_仕分番号].Text), 0) = 0 Then
				CriticalAlarm("仕分番号を入力して下さい。")
				[tx_仕分番号].Focus()
				Exit Function
			End If
			'UPGRADE_WARNING: オブジェクト NullToZero(tx_仕分番号.Text) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If NullToZero(([tx_仕分番号].Text)) < 1 Or NullToZero(([tx_仕分番号].Text)) > 31 Then
				CriticalAlarm("仕分番号が範囲外です。")
				[tx_仕分番号].Focus()
				Exit Function
			End If
		End If

		Item_Check = True

		Exit Function
Item_Check_Err:
		MsgBox(Err.Number & " " & Err.Description)
	End Function

	Private Sub Cb中止_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Cb中止.Click
		Me.Close()
	End Sub

	Private Sub CbXLS_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CbXLS.Click
		Dim st As Integer

		If Item_Check((CbXLS.TabIndex)) = False Then
			Exit Sub
		End If

		If txDir.Text = vbNullString Then
			CriticalAlarm("ファイルを指定して下さい。")
			Cb変更.Focus()
			Exit Sub
		End If

		If MsgBoxResult.No = YesNo("指定のファイルを員数シートに上書きします。") Then Exit Sub
		'2004/02/05 ADD
		If FileOverWriteCheck(GXLSPath,  , True) <> 0 Then Exit Sub 'オーバーライトをチェックします
		st = 0
		'    st = 員数シート取込(GXLSPath)
		'    If st = 0 Then
		If ImportFixdTextToRsNew(GXLSPath, False) = True Then
			rf_製品なし数.Text = CStr(werrCnt)
			rf_取込行数.Text = CStr(wSiwaGyo)
			If MsgBoxResult.Yes = Question("終了しました。適用しますか？" & vbCrLf & "", MsgBoxResult.Yes) Then
				'            pParentForm.SiwaNo = wSiwaNo
				'UPGRADE_ISSUE: Control SiwaGyo は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
				pParentForm.SiwaGyo = wSiwaGyo
				'UPGRADE_ISSUE: Control TempData は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト wTempData の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				pParentForm.TempData = wTempData
				'UPGRADE_ISSUE: Control REVIT_SIWANO は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				pParentForm.REVIT_SIWANO = NullToZero(([tx_仕分番号].Text))
				'親のメソッドで処理をする。
				'UPGRADE_ISSUE: Control SetImportData_Revit は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
				pParentForm.SetImportData_Revit()
				'            Call Set取込FLG(GXLSPath)
				Me.Close()
				Exit Sub
			End If
		Else
			Cb変更.Focus()
			Exit Sub
		End If
	End Sub

	Private Sub CbTenyo_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CbTenyo.Click
		Dim st As Integer

		'    If Item_Check([cbTenyo].TabIndex) = False Then
		'        Exit Sub
		'    End If
		If [tx_仕分番号].Text <> "" Then
			CriticalAlarm("仕分番号が入力されています。")
			[tx_仕分番号].Focus()
			Exit Sub
		End If

		If txDir.Text = vbNullString Then
			CriticalAlarm("ファイルを指定して下さい。")
			Cb変更.Focus()
			Exit Sub
		End If

		If MsgBoxResult.No = YesNo("指定のファイルを員数シートの転用に上書きします。") Then Exit Sub
		'2004/02/05 ADD
		If FileOverWriteCheck(GXLSPath,  , True) <> 0 Then Exit Sub 'オーバーライトをチェックします
		st = 0
		'    st = 員数シート取込(GXLSPath)
		'    If st = 0 Then
		If ImportFixdTextToRsNew(GXLSPath, True) = True Then
			rf_製品なし数.Text = CStr(werrCnt)
			rf_取込行数.Text = CStr(wSiwaGyo)
			If MsgBoxResult.Yes = Question("終了しました。転用を適用しますか？" & vbCrLf & "", MsgBoxResult.Yes) Then
				'            pParentForm.SiwaNo = wSiwaNo
				'UPGRADE_ISSUE: Control SiwaGyo は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
				pParentForm.SiwaGyo = wSiwaGyo
				'UPGRADE_ISSUE: Control TempData は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト wTempData の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				pParentForm.TempData = wTempData
				'UPGRADE_ISSUE: Control REVIT_SIWANO は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				pParentForm.REVIT_SIWANO = NullToZero(([tx_仕分番号].Text))
				'親のメソッドで処理をする。
				'UPGRADE_ISSUE: Control SetImportData_Revit は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
				pParentForm.SetImportData_Revit()
				'            Call Set取込FLG(GXLSPath)
				Me.Close()
				Exit Sub
			End If
		Else
			Cb変更.Focus()
			Exit Sub
		End If
	End Sub

	Private Sub Cb変更_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Cb変更.Click
		'    If OpenFileDialog(GXLSPath, ProfileKey, "xls") = True Then
		If ShowOpenFileDialog(GXLSPath, ProfileKey, "txt") = True Then
			WriteIni("Path", ProfileKey, GXLSPath, INIFile)
		End If
		'UPGRADE_WARNING: TextBox プロパティ txDir.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
		txDir.Text = CompactPathEx(GXLSPath, txDir.MaxLength)
		'    rf_コメント.Visible = False
		'    rf_取込仕分.Caption = vbNullString
		rf_取込行数.Text = vbNullString
	End Sub

	Private Function ImportFixdTextToRsNew(ByRef FilePath As String, Optional ByRef tenyoF As Boolean = False) As Boolean
		Dim cCsvReader_UNI As clsCSVReader_UNI
		cCsvReader_UNI = New clsCSVReader_UNI

		ImportFixdTextToRsNew = False

		' 指定した CSV ファイルを開く
		If cCsvReader_UNI.OpenStream(FilePath) = False Then
			CriticalAlarm(("指定したファイルが存在しません。"))
			Exit Function
		End If


		' 最初の行をヘッダとして読み込む
		'    Call cCsvReader.ReadHeader

		' CSV ファイルの中身をすべて取得する
		Dim cTable As Collection
		cTable = cCsvReader_UNI.ReadToEnd()


		Dim i As Integer
		Dim j As Integer

		j = 0
		werrCnt = 0

		'-------------------------
		'いらないデータの削除
		'-------------------------
		For i = cTable.Count() To 1 Step -1
			'UPGRADE_WARNING: オブジェクト cTable(i)(1) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If cTable.Item(i)(1) <> Chr(&H1A) And cTable.Item(i)(1) <> "" Then
			Else
				'EOFは削除
				cTable.Remove((i))
			End If
		Next

		'最大行数での変数の初期化
		'UPGRADE_WARNING: 配列 wTempData の下限が 1,1 から 0,0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"' をクリックしてください。
		ReDim wTempData(cTable.Count(), 30)

		'データ読み込み
		With cTable
			For i = 1 To cTable.Count()
				j += 1

				'UPGRADE_WARNING: オブジェクト cTable(j)(1) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				If cTable.Item(j)(1) & "" = "" Then
					werrCnt += 1
				End If

				'員数シート情報取込
				'UPGRADE_WARNING: オブジェクト wTempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wTempData(i, 1) = 0 '見積区分
				'UPGRADE_WARNING: オブジェクト wTempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wTempData(i, 2) = "" 'SP区分
				'UPGRADE_WARNING: オブジェクト wTempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wTempData(i, 3) = "" 'PC区分
				'UPGRADE_WARNING: オブジェクト cTable()() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト wTempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wTempData(i, 4) = cTable.Item(j)(1) '製品NO
				'UPGRADE_WARNING: オブジェクト cTable()() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト wTempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wTempData(i, 5) = cTable.Item(j)(2) '仕様NO
				'UPGRADE_WARNING: オブジェクト wTempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wTempData(i, 6) = "" 'ベース色
				'UPGRADE_WARNING: オブジェクト wTempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wTempData(i, 7) = "" '他社伝票番号
				'UPGRADE_WARNING: オブジェクト cTable()() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト AnsiLeftB() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト wTempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wTempData(i, 8) = Trim(AnsiLeftB(cTable.Item(j)(3), 40)) '名称
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト wTempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wTempData(i, 9) = NullToZero(cTable.Item(j)(4)) 'W
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト wTempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wTempData(i, 10) = NullToZero(cTable.Item(j)(5)) 'D
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト wTempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wTempData(i, 11) = NullToZero(cTable.Item(j)(6)) 'H
				'UPGRADE_WARNING: オブジェクト wTempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wTempData(i, 12) = 0 'D1
				'UPGRADE_WARNING: オブジェクト wTempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wTempData(i, 13) = 0 'D2
				'UPGRADE_WARNING: オブジェクト wTempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wTempData(i, 14) = 0 'H1
				'UPGRADE_WARNING: オブジェクト wTempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wTempData(i, 15) = 0 'H2
				'UPGRADE_WARNING: オブジェクト cTable()() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト AnsiLeftB() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト wTempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wTempData(i, 19) = Trim(AnsiLeftB(cTable.Item(j)(8), 6)) '単位
				'UPGRADE_WARNING: オブジェクト cTable(j)(11) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				If cTable.Item(j)(11) = "" Then
					'プラス
					If tenyoF = False Then
						'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト wTempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						wTempData(i, 28) = NullToZero(cTable.Item(j)(7), 0) '数量
					Else
						'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト wTempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						wTempData(i, 26) = NullToZero(cTable.Item(j)(7), 0) '転用数
					End If
				Else
					'マイナス
					If tenyoF = False Then
						'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト wTempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						wTempData(i, 28) = NullToZero(cTable.Item(j)(7), 0) * -1 '数量
					Else
						'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト wTempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						wTempData(i, 26) = NullToZero(cTable.Item(j)(7), 0) * -1 '転用数
					End If
				End If
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト wTempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wTempData(i, 29) = NullToZero(([tx_仕分番号].Text)) '仕分レベル

				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト wTempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wTempData(i, 30) = NullToZero(cTable.Item(j)(13), "") '追番R（S_ｴﾘｱ名） 2019/12/12 ADD
			Next

		End With

		'カウント
		wSiwaGyo = cTable.Count()


		'２重取込できないように
		'ファイル名の変更
		Rename(FilePath, FilePath & "xxx")


		ImportFixdTextToRsNew = True
	End Function
End Class