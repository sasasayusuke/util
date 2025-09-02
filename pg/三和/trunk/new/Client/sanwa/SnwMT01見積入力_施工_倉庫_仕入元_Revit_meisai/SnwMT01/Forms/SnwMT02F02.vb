Option Strict Off
Option Explicit On

Imports FarPoint.Win.Spread
Imports GrapeCity.Spreadsheet.Win

''' <summary>
''' --------------------------------------------------------------------
'''   ユーザー名           株式会社三和商研
'''   業務名               積算データ管理システム
'''   部門名               見積部門
'''   プログラム名         員数入力処理（仕分レベル設定処理）
'''   作成会社             テクノウェア株式会社
'''   作成日               2003/06/13
'''   作成者               kawamura
''' --------------------------------------------------------------------
''' 
'''    UPDATE
'''        2011/11/29  oosawa      (F02)時間がスペースならば00:00をセット
'''        2013/09/02  oosawa      仕分レベル設定に日付一括を追加
'''        2014/04/22  oosawa      仕分レベルの納期に前画面の納期Sをセット（空白の場合）
'''        2015/10/07  oosawa      仕分レベルの桁数を20→30へ変更
'''        2020/06/24  oosawa      仕分レベル取込追加
''' --------------------------------------------------------------------
''' </summary>
Friend Class SnwMT02F02
	Inherits System.Windows.Forms.Form

	'ｽﾌﾟﾚｯﾄﾞのクラス
	'UPGRADE_NOTE: clsSPD は clsSPD_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
	Dim clsSPD As clsSPD

	'保存ﾁｪｯｸ用ワーク
	Private Structure WkTBL
		Dim WKMeisyo As String
		Dim WKRyaku As String
		Dim WKNouki As String
		Dim WKJikan As String
	End Structure

	'変数
	Dim HoldCD As String
	Dim HCol As Integer
	Dim HRow As Integer

	'ｽﾌﾟﾚｯﾄﾞｼｰﾄの定数
	Private Const Col仕分名称 As Integer = 0
	Private Const Col仕分略称 As Integer = 1
	Private Const Col納期 As Integer = 2
	Private Const Col時間 As Integer = 3
	Private Const Col部門 As Integer = 4

	Dim WkTBLS() As WkTBL
	Dim tbls(,) As Object

	Dim pParentForm As SnwMT02F00
	Dim pMituNo As Integer
	Dim pSetCol As Integer

	Dim PreviousControl As System.Windows.Forms.Control 'コントロールの戻りを制御

	'各項目でEnterKeyが押されたかのﾁｪｯｸﾌﾗｸﾞ
	Dim ReturnF As Boolean

	Dim grs As ADODB.Recordset

	Private Sub SnwMT02F02_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Me.Load
		'Dim fpSpd As Object

		On Error GoTo SysErr_Form_Load

		'フォームを画面の中央に配置
		Me.Top = VB6Conv.TwipsToPixelsY((VB6Conv.PixelsToTwipsY(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height) - VB6Conv.PixelsToTwipsY(Me.Height)) \ 2)
		Me.Left = VB6Conv.TwipsToPixelsX((VB6Conv.PixelsToTwipsX(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width) - VB6Conv.PixelsToTwipsX(Me.Width)) \ 2)

		'SPREAD設定
		clsSPD = New clsSPD
		'		clsSPD.CtlSpd = pParentForm.fpSpd
		clsSPD.CtlSpd = FpSpd

		'スプレッドの設定をする。
		Call FpSpd_Initialize()

		rf_見積番号.Text = pMituNo.ToString("#")
		Call SetSPDFromgSiwakeTBL()

		' フォームでキー入力を受け取れるようにする
		Me.KeyPreview = True

		' アクティブセルを１列右に移動します。
		Dim im As FarPoint.Win.Spread.InputMap
		im = FpSpd.GetInputMap(FarPoint.Win.Spread.InputMapMode.WhenAncestorOfFocused)
		im.Put(New FarPoint.Win.Spread.Keystroke(Keys.Enter, Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToNextColumn)
		im = FpSpd.GetInputMap(FarPoint.Win.Spread.InputMapMode.WhenFocused)
		im.Put(New FarPoint.Win.Spread.Keystroke(Keys.Enter, Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToNextColumn)

		Exit Sub
SysErr_Form_Load:
		MsgBox(Err.Number & " " & Err.Description)
	End Sub

	Private Sub SnwMT02F02_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
		Dim Cancel As Boolean = e.Cancel
		Dim UnloadMode As System.Windows.Forms.CloseReason = e.CloseReason
		'UPGRADE_NOTE: オブジェクト pParentForm をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		pParentForm = Nothing
		e.Cancel = Cancel
		Me.Dispose()
	End Sub

	Private Sub CbClose_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CbClose.Click
		Me.Close()
	End Sub

	Private Sub CbClose_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CbClose.Enter
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		sb_Msg.Items.Item(0).Text = ""
	End Sub

	'2013/09/02 ADD↓
	Private Sub CbNouki_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CbNouki.Enter
		If Item_Check((CbNouki.TabIndex)) = False Then
			Exit Sub
		End If
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		sb_Msg.Items.Item(0).Text = ""
	End Sub

	Private Sub CbNouki_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CbNouki.Click
		'Dim fpSpd As Object
		Dim i As Integer

		'UPGRADE_WARNING: オブジェクト fpSpd.DataRowCnt の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'If FpSpd.ActiveSheet.NonEmptyRowCount = 0 Then
		If clsSPD.GetLastNonEmptyRowEx + 1 = 0 Then
			CriticalAlarm("明細がありません。")
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'FpSpd.Col = FpSpd.ActiveSheet.ActiveRowIndex
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'FpSpd.Row = FpSpd.ActiveSheet.ActiveRowIndex
			'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'FpSpd.Action = FPSpreadADO.ActionConstants.ActionActiveCell
			FpSpd.ActiveSheet.SetActiveCell(FpSpd.ActiveSheet.ActiveRowIndex, FpSpd.ActiveSheet.ActiveColumnIndex)
			'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			FpSpd.Focus()
			Exit Sub
		End If

		'1行目の日付を以降にもセット

		Dim buff As String
		With FpSpd
			buff = clsSPD.GetTextEX(Col納期, 0) '一行目の値を取得

			'UPGRADE_WARNING: オブジェクト fpSpd.ReDraw の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'.ReDraw = False
			.SuspendLayout()
			'UPGRADE_WARNING: オブジェクト fpSpd.DataRowCnt の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'For i = 1 To FpSpd.ActiveSheet.NonEmptyRowCount - 1
			For i = 1 To clsSPD.GetLastNonEmptyRowEx
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ActiveSheet.SetText(i, Col納期, buff)
			Next
			'UPGRADE_WARNING: オブジェクト fpSpd.ReDraw の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'.ReDraw = True
			.ResumeLayout(True)
		End With
		PreviousControl.Focus()
	End Sub
	'2013/09/02 ADD↑

	Private Sub CbUpload_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CbUpload.Enter
		If Item_Check((CbUpload.TabIndex)) = False Then
			Exit Sub
		End If
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		sb_Msg.Items.Item(0).Text = ""
	End Sub

	Private Sub CbUpload_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CbUpload.Click
		If Upload_Chk() = False Then
			PreviousControl.Focus()
			Exit Sub
		End If
		If MsgBoxResult.Yes = YesNo("保存します。", Me.Text) Then
			System.Windows.Forms.Application.DoEvents()

			If Upload() Then
				Call SetHeader()
				Me.Close()
			Else
				PreviousControl.Focus()
			End If
		Else
			PreviousControl.Focus()
		End If
	End Sub

	'2020/06/24 ADD↓
	Private Sub CbGET_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cbGET.Click
		Dim RecCnt As Integer
		Dim wMsg As String
		wMsg = ""

		If Item_Check(([cbGET].TabIndex)) = False Then
			Exit Sub
		End If

		If MsgBoxResult.No = YesNo("指定の仕分レベルを上書きします。") Then Exit Sub

		RecCnt = 仕分レベル取込(CInt(NullToZero(Tx_見積番号.Text, 0)))
		Select Case RecCnt
			Case -1
				CriticalAlarm("該当データ無し")
			Case Else
				'            rf_取込行数.Caption = RecCnt
				'            If RecCnt + pActRow > 30 Then
				'                wMsg = vbCrLf & "※全ての行を取り込めません。" & vbCrLf & vbCrLf
				'            Else
				'                wMsg = ""
				'            End If
				If MsgBoxResult.Yes = Question("仕分レベルを上書きします。" & wMsg & "適用しますか？" & vbCrLf & "", MsgBoxResult.Yes) Then
					'親のメソッドで処理をする。
					DspGetData(grs, 0) 'pActRow
					'UPGRADE_NOTE: オブジェクト grs をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
					grs = Nothing
					'                Unload Me
					Exit Sub
				End If
		End Select

	End Sub

	Private Function 仕分レベル取込(ByRef MituNo As Integer) As Integer
		Dim rs As ADODB.Recordset
		Dim sql As String
		Dim wMeisai As Object
		Dim wLine As Integer
		Dim i As Integer

		On Error GoTo Download_Err
		'マウスポインターを砂時計にする
		HourGlass(True)

		仕分レベル取込 = 0

		'---見積明細セット
		sql = "SELECT "
		sql = sql & " MSUM.見積番号, MSUM.仕分番号, MSUM.名称, MSUM.略称, MSUM.他社部門CD"
		sql = sql & " FROM TD見積シート内訳名称 AS MSUM"
		sql = sql & " WHERE MSUM.見積番号 = " & MituNo
		sql = sql & " ORDER BY MSUM.仕分番号"

		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly) 'RecordCountを取る為

		With rs
			If .EOF Then
				仕分レベル取込 = -1
			Else
				仕分レベル取込 = rs.RecordCount
			End If
		End With

		If 仕分レベル取込 > 0 Then

			grs = rs

		End If

		HourGlass(False)
		Exit Function
Download_Err:
		Call ReleaseRs(rs)
		MsgBox(Err.Number & " " & Err.Description)
		HourGlass(False)
	End Function

	Private Sub DspGetData(ByRef rs As ADODB.Recordset, ByRef DspRow As Integer)
		'Dim fpSpd As Object
		'指定の行に指定の仕分レベルを表示する
		'Dim X As Integer
		Dim wLine As Integer

		'2015/10/26 ADD
		Dim cSeihin As clsSeihin
		cSeihin = New clsSeihin

		HourGlass(True)

		'表示行セット
		wLine = DspRow
		'    i = 1

		'UPGRADE_WARNING: オブジェクト fpSpd.AutoCalc の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		FpSpd.ActiveSheet.AutoCalculation = False

		With FpSpd
			'シートのクリア
			Call clsSPD.SprClearText()

			'UPGRADE_WARNING: オブジェクト fpSpd.ReDraw の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'.ReDraw = False
			.SuspendLayout()

			Do Until rs.EOF

				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				FpSpd.ActiveSheet.SetText(wLine, Col仕分名称, Trim(rs.Fields("名称").Value & ""))
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				FpSpd.ActiveSheet.SetText(wLine, Col仕分略称, Trim(rs.Fields("略称").Value & ""))
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				FpSpd.ActiveSheet.SetText(wLine, Col部門, Trim(rs.Fields("他社部門CD").Value & ""))

				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				FpSpd.ActiveSheet.SetText(wLine, Col納期, HD_納期S)

				rs.MoveNext()

				'表示行カウント
				wLine += 1
			Loop
			'UPGRADE_WARNING: オブジェクト fpSpd.ReDraw の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'.ReDraw = True
			.ResumeLayout(True)

		End With

		ReleaseRs(rs)

		'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'pParentForm.fpSpd.Col = FpSpd.ActiveSheet.ActiveColumnIndex '2003/11/25 ADD
		'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'pParentForm.fpSpd.Row = FpSpd.ActiveSheet.ActiveRowIndex
		'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'pParentForm.fpSpd.Action = FPSpreadADO.ActionConstants.ActionActiveCell
		FpSpd.ActiveSheet.SetActiveCell(pParentForm.FpSpd.ActiveSheet.ActiveRowIndex, pParentForm.FpSpd.ActiveSheet.ActiveColumnIndex)
		'UPGRADE_WARNING: オブジェクト fpSpd.AutoCalc の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		FpSpd.ActiveSheet.AutoCalculation = True

		HourGlass(False)
	End Sub
	'2020/06/24 ADD↑

	Private Sub FpSpd_Initialize()
		'Dim fpSpd As Object

		With FpSpd
			'UPGRADE_WARNING: オブジェクト fpSpd.EditModePermanent の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.EditModePermanent = True '常時入力モードを維持するかどうかを設定します。
			.EditModeReplace = True
		End With

		FpSpd.SuspendLayout()

		With FpSpd
			.ActiveSheet.Protect = True

			'UPGRADE_WARNING: オブジェクト fpSpd.ReDraw の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'.ReDraw = False
			'UPGRADE_WARNING: オブジェクト fpSpd.MaxRows の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.ActiveSheet.RowCount = 30
			.ActiveSheet.ColumnCount = 5

			.ActiveSheet.FrozenRowCount = 0
			.ActiveSheet.FrozenColumnCount = 5

			.ActiveSheet.Columns(0, FpSpd.ActiveSheet.Columns.Count - 1).Locked = False

			.ActiveSheet.Columns(Col仕分名称).CellType = New FarPoint.Win.Spread.CellType.TextCellType With {
			   .AcceptsArrowKeys = FarPoint.Win.SuperEdit.AcceptsArrowKeys.None,
			   .MaxLength = 40
			}
			.ActiveSheet.Columns(Col仕分名称).ImeMode = System.Windows.Forms.ImeMode.NoControl
			.ActiveSheet.Columns(Col仕分略称).CellType = New FarPoint.Win.Spread.CellType.TextCellType With {
			   .AcceptsArrowKeys = FarPoint.Win.SuperEdit.AcceptsArrowKeys.None,
			   .MaxLength = 8
			}
			.ActiveSheet.Columns(Col仕分略称).ImeMode = System.Windows.Forms.ImeMode.NoControl
			.ActiveSheet.Columns(Col納期).CellType = New FarPoint.Win.Spread.CellType.DateTimeCellType With {
			   .AcceptsArrowKeys = FarPoint.Win.SuperEdit.AcceptsArrowKeys.None,
			   .DateTimeFormat = FarPoint.Win.Spread.CellType.DateTimeFormat.UserDefined,
			   .UserDefinedFormat = "yy/MM/dd",
			   .DateSeparator = "/",
			   .MinimumDate = "1990/01/01",
			   .MaximumDate = "2100/12/31"
			}
			.ActiveSheet.Columns(Col納期).ImeMode = System.Windows.Forms.ImeMode.NoControl
			.ActiveSheet.Columns(Col時間).CellType = New FarPoint.Win.Spread.CellType.DateTimeCellType With {
			   .AcceptsArrowKeys = FarPoint.Win.SuperEdit.AcceptsArrowKeys.None,
			   .DateTimeFormat = FarPoint.Win.Spread.CellType.DateTimeFormat.UserDefined,
			   .UserDefinedFormat = "HH:mm",
			   .TimeSeparator = ":"
			}
			.ActiveSheet.Columns(Col時間).ImeMode = System.Windows.Forms.ImeMode.NoControl
			.ActiveSheet.Columns(Col部門).CellType = New FarPoint.Win.Spread.CellType.TextCellType With {
			   .AcceptsArrowKeys = FarPoint.Win.SuperEdit.AcceptsArrowKeys.None,
			   .MaxLength = 4
			}
			.ActiveSheet.Columns(Col部門).ImeMode = System.Windows.Forms.ImeMode.Alpha

			'セル
			'UPGRADE_WARNING: オブジェクト fpSpd.UnitType の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'NOTE SS .UnitType = FPSpreadADO.UnitTypeConstants.UnitTypeTwips
			'列の幅を設定する。
			'UPGRADE_WARNING: オブジェクト fpSpd.RowHeight の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'.RowHeight(-1) = 250
			'TODO SS .ActiveSheet.SetRowHeight(0, 250) 'twip → ピクセル単位に変換 250/14.4→17.36
			'シートのクリア
			'Call clsSPD.SprClearText()
			'グリッド線の表示形式を設定します。
			'UPGRADE_WARNING: オブジェクト fpSpd.GridSolid の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'TODO SS .GridSolid グリッド線
			'NOTE SS .GridSolid = True ' 線種（点線）
			'セルの背景色をグリッド線の下に表示します。
			'UPGRADE_WARNING: オブジェクト fpSpd.BackColorStyle の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'NOTE SS .BackColorStyle = FPSpreadADO.BackColorStyleConstants.BackColorStyleUnderGrid

			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'.Col = 1
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'.Row = 1
			'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'.Action = FPSpreadADO.ActionConstants.ActionActiveCell
			.ActiveSheet.SetActiveCell(0, 0)

			'行ヘッダのロック表示解除
			.ActiveSheet.ColumnHeader.Rows(0).Locked = False
			'列ヘッダのロック表示解除
			.ActiveSheet.RowHeader.Columns(0).Locked = False

		End With

		FpSpd.ResumeLayout(True)
		FpSpd.Select()
	End Sub

	Private Sub FpSpd_GotFocus(sender As Object, e As EventArgs) Handles FpSpd.GotFocus
		'Dim fpSpd As Object
		PreviousControl = Me.ActiveControl
		'スプレッドコメント表示
		'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Call Comment_spd(FpSpd.ActiveSheet.ActiveColumnIndex, FpSpd.ActiveSheet.ActiveRowIndex)
		'ホイールコントロール制御開始
		'UPGRADE_WARNING: オブジェクト fpSpd の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'Call StartWheel(FpSpd)
	End Sub

	Private Sub FpSpd_LostFocus(sender As Object, e As EventArgs) Handles FpSpd.LostFocus
		'ホイールコントロール制御解除
		Call EndWheel()
	End Sub

	Private Sub FpSpd_Advance(sender As Object, e As FarPoint.Win.Spread.AdvanceEventArgs) Handles FpSpd.Advance
		'Dim fpSpd As Object
		With FpSpd
			'UPGRADE_WARNING: オブジェクト fpSpd.MaxRows の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If .ActiveSheet.ActiveRowIndex = .ActiveSheet.RowCount Then
				If e.AdvanceNext = True Then
					'UPGRADE_WARNING: オブジェクト fpSpd.MaxCols の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					If .ActiveSheet.ActiveColumnIndex <> .ActiveSheet.ColumnCount Then
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'.Col = .ActiveSheet.ActiveColumnIndex + 1
						'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'.Row = 1
						'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'.Action = FPSpreadADO.ActionConstants.ActionActiveCell
						.ActiveSheet.SetActiveCell(0, .ActiveSheet.ActiveColumnIndex + 1)
						'UPGRADE_WARNING: オブジェクト fpSpd.TopRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'fpSpd.TopRow = 1
						.SetViewportTopRow(.ActiveSheet.ActiveRowIndex, 1)
					End If
				End If
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			ElseIf FpSpd.ActiveSheet.ActiveColumnIndex = 1 Then
				If e.AdvanceNext = False Then
					'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					If .ActiveSheet.ActiveRowIndex <> 1 Then
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'.Col = .ActiveSheet.ActiveColumnIndex
						'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト fpSpd.MaxRows の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'.Row = .ActiveSheet.ActiveRowIndex
						'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'.Action = FPSpreadADO.ActionConstants.ActionActiveCell
						.ActiveSheet.SetActiveCell(.ActiveSheet.ActiveRowIndex, .ActiveSheet.ActiveColumnIndex)
					End If
				End If
			End If
		End With
	End Sub

	Private Sub FpSpd_EditModeStarting(sender As Object, e As FarPoint.Win.Spread.EditModeStartingEventArgs) Handles FpSpd.EditModeStarting

		'Dim fpSpd As Object
		'    Debug.Print "fpSpd_EditMode = " & Col & ":" & Row & ":" & Mode & ":" & ChangeMade
		'Dim check As Boolean

		'If Mode = 0 Then Exit Sub 'フォーカスがないならば
		'If ChangeMade = True Then Exit Sub

		Dim Row As Integer = FpSpd.ActiveSheet.ActiveRowIndex
		Dim Col As Integer = FpSpd.ActiveSheet.ActiveColumnIndex

		'行の色を変える
		With FpSpd
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'.Col = -1
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'.Row = .ActiveSheet.ActiveRowIndex
			'UPGRADE_WARNING: オブジェクト fpSpd.BackColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.ActiveSheet.Rows(.ActiveSheet.ActiveRowIndex).BackColor = System.Drawing.ColorTranslator.FromOle(&HFFFFC0)
		End With

		If HCol = Col And HRow = Row Then Exit Sub

		'        Debug.Print "hold:" & " col:" & Col & " row:" & Row
		'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HoldCD = FpSpd.ActiveSheet.GetText(Row, Col)

		HCol = Col
		HRow = Row
		'    Debug.Print "EditMode col & Row & HoldCD = " & Col & ":" & Row & ":" & HoldCD
	End Sub

	Private Sub FpSpd_LeaveCell(sender As Object, e As LeaveCellEventArgs) Handles FpSpd.LeaveCell
		'Dim fpSpd As Object
		Dim check As Boolean 'Cell取り出しチェックフラグ
		Dim getdata As String 'Cell取り出し用
		'Dim getryak As Object 'Cell取り出し用

		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		If Me.ActiveControl.Name = "CbClose" Then Exit Sub

		With FpSpd
			'入力された情報を取得
			'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			getdata = .ActiveSheet.GetText(e.Row, e.Column)
			If getdata.ToString().Trim() = "" Then
				check = False
			Else
				check = True
			End If

			Select Case e.Column
				Case 0
					'値が変わったら
					If check Then
						'UPGRADE_WARNING: オブジェクト HoldCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						If getdata <> HoldCD Then
							'略称セット(名称の８バイト)
							'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.ActiveSheet.SetText(e.Row, Col仕分略称, AnsiLeftB(getdata, 8))
							'2014/04/22 ADD↓
							If clsSPD.GetTextEX(Col納期, e.Row) = "" Then
								'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.ActiveSheet.SetText(e.Row, Col納期, HD_納期S)
							End If
							'2014/04/22 ADD↑
						End If
					End If
			End Select
		End With

		'行の色を変える
		With FpSpd
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'.Col = -1
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'.Row = Row
			'UPGRADE_WARNING: オブジェクト fpSpd.BackColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.ActiveSheet.Rows(e.Row).BackColor = System.Drawing.ColorTranslator.FromOle(&HFFFFFF)
			If e.NewRow >= 0 Then
				'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'.Col = -1
				'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'.Row = NewRow
				'UPGRADE_WARNING: オブジェクト fpSpd.BackColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ActiveSheet.Rows(e.NewRow).BackColor = System.Drawing.ColorTranslator.FromOle(&HFFFFC0)
			End If
		End With

		'スプレッドコメント表示
		Call Comment_spd(e.NewColumn, e.NewRow)
	End Sub

	Private Sub Comment_spd(ByRef Col As Integer, ByRef Row As Integer)
		'Dim fpSpd As Object
		'IMEモードを「オフ」にする
		Call ImmOpenModeSet(Me.Handle)

		'スプレッドのコメントをだす
		Dim Buf As String

		With FpSpd
			Select Case Col
				Case -1
				Case 1, 2
					'IMEモードを「全角ひらがな」にする
					Call ImmOpenModeSet(Me.Handle, Win32API_IME.ConversionMODE.ZENHIRA)
				Case Else
			End Select

			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'.Col = Col
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'.Row = 0
			'UPGRADE_WARNING: オブジェクト fpSpd.Text の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'Buf = .Text
			Buf = FpSpd.ActiveSheet.Cells(0, Col).Column.Label
			'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
			sb_Msg.Items.Item(0).Text = Buf & "を入力して下さい。"
		End With
	End Sub

	Private Function Item_Check(ByRef ItemNo As Integer) As Boolean

		On Error GoTo Item_Check_Err
		Item_Check = False


		Item_Check = True

		Exit Function
Item_Check_Err:
		MsgBox(Err.Number & " " & Err.Description)
	End Function

	''Private Function Download(MituNo As Integer) As Integer
	''    Dim rs As adodb.Recordset, SQL As String
	''    Dim wMeisai As Variant
	''    Dim wLine As Integer
	''    Dim i As Integer
	''
	''    On Error GoTo Download_Err
	''    'マウスポインターを砂時計にする
	''    HourGlass True
	''
	''    Download = 0
	''
	''    '---見積明細内訳名称セット
	''    SQL = "SELECT * FROM TD見積シート内訳名称 " _
	''        & "WHERE 見積番号 = " & MituNo
	''
	''    Set rs = OpenRs(SQL, Cn, adOpenForwardOnly, adLockReadOnly)
	''
	''    With rs
	''        If .EOF Then
	''            Call InitialItems
	''        Else
	''            Call SetupItems(rs)
	''        End If
	''    End With
	''
	''    Call ReleaseRs(rs)
	''
	''    HourGlass False
	''    Exit Function
	''Download_Err:
	''    Call ReleaseRs(rs)
	''    MsgBox Err.Number & " " & Err.Description
	''    HourGlass False
	''End Function

	Private Sub InitialItems()
		'シートのクリア
		Call clsSPD.SprClearText()
	End Sub

	Private Sub SetSPDFromgSiwakeTBL()
		'    Dim RecArry() As Variant
		Dim i, j As Integer
		'    Dim a As String

		'シートのクリア
		Call clsSPD.SprClearText()

		'    RecArry = rs.GetRows(fpSpd.MaxRows, , Array("名称", "略称", "納期", "時間"))
		'UPGRADE_WARNING: IsEmpty は、IsNothing にアップグレードされ、新しい動作が指定されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
		If Not IsNothing(gSiwakeTBL) Then
			For i = 0 To UBound(gSiwakeTBL, 2)
				For j = 0 To UBound(gSiwakeTBL)
					Select Case j + 1
						Case 3
							'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'fpSpd.Col = j + 1
							'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'FpSpd.Row = i + 1
							'UPGRADE_WARNING: オブジェクト gSiwakeTBL() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							If IsDBNull(gSiwakeTBL(j, i)) Then
								FpSpd.ActiveSheet.SetText(i, j, "")
							Else
								FpSpd.ActiveSheet.SetText(i, j, CType(gSiwakeTBL(j, i), DateTime).ToString("yyyy/MM/dd"))
							End If
						Case Else
							'UPGRADE_WARNING: オブジェクト gSiwakeTBL() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							FpSpd.ActiveSheet.SetText(i, j, Trim("" & gSiwakeTBL(j, i)))
					End Select
				Next
			Next
		End If
		'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'fpSpd.Col = 1
		'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'fpSpd.Row = 1
		'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'fpSpd.Action = FPSpreadADO.ActionConstants.ActionActiveCell
		FpSpd.ActiveSheet.SetActiveCell(0, 0)
	End Sub

	Private Function Upload_Chk() As Boolean
		'Dim fpSpd As Object
		Dim i As Integer
		'Dim j As Integer
		Dim wMaxRow As Integer
		'Dim getdata As Object
		'Dim check As Boolean

		Upload_Chk = False

		'UPGRADE_WARNING: オブジェクト fpSpd.DataRowCnt の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'If FpSpd.ActiveSheet.NonEmptyRowCount = 0 Then
		If clsSPD.GetLastNonEmptyRowEx + 1 = 0 Then
			CriticalAlarm("明細がありません。")
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'fpSpd.Col = fpSpd.ActiveRow
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'FpSpd.Row = fpSpd.ActiveRow
			'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'FpSpd.Action = FPSpreadADO.ActionConstants.ActionActiveCell
			FpSpd.ActiveSheet.SetActiveCell(FpSpd.ActiveSheet.ActiveRowIndex, FpSpd.ActiveSheet.ActiveColumnIndex)
			'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'FpSpd.Focus()
			Exit Function
		End If

		'    ReDim WkTBLS(1 To fpSpd.DataRowCnt)

		'    ReDim tbls(1 To fpSpd.DataRowCnt, 1 To 4) As Variant '(1 to 30,1 to 4)
		'UPGRADE_WARNING: 配列 tbls の下限が 1,1 から 0,0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"' をクリックしてください。
		'UPGRADE_ISSUE: As Variant が ReDim tbls(1 To fpSpd.DataRowCnt, 1 To fpSpd.MaxCols) ステートメントから削除されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="19AFCB41-AA8E-4E6B-A441-A3E802E5FD64"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト fpSpd.MaxCols の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト fpSpd.DataRowCnt の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'ReDim tbls(FpSpd.ActiveSheet.NonEmptyRowCount, FpSpd.ActiveSheet.ColumnCount) '(1 to 30,1 to 4)'2016/03/07 ADD
		ReDim tbls(clsSPD.GetLastNonEmptyRowEx + 1, FpSpd.ActiveSheet.ColumnCount) '(1 to 30,1 to 4)'2016/03/07 ADD

		'UPGRADE_WARNING: オブジェクト fpSpd.GetArray の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'tbls = FpSpd.ActiveSheet.GetArray(0, 0, FpSpd.ActiveSheet.NonEmptyRowCount, FpSpd.ActiveSheet.ColumnCount)
		tbls = FpSpd.ActiveSheet.GetArray(0, 0, clsSPD.GetLastNonEmptyRowEx + 1, FpSpd.ActiveSheet.ColumnCount)

		'未入力最終明細サーチ
		For wMaxRow = UBound(tbls, 1) To LBound(tbls, 1) Step -1
			If IsCheckNull(tbls(wMaxRow, Col仕分名称)) = False Then Exit For '名称
			If IsCheckNull(tbls(wMaxRow, Col仕分略称)) = False Then Exit For '略称
			If IsCheckNull(tbls(wMaxRow, Col納期)) = False Then Exit For '納期
			If IsCheckNull(tbls(wMaxRow, Col時間)) = False Then Exit For '時間
			If IsCheckNull(tbls(wMaxRow, Col部門)) = False Then Exit For '部門 2016/03/07 ADD
		Next

		With FpSpd
			'        fpSpd.GetArray 1, 1, tbls
			'UPGRADE_WARNING: オブジェクト fpSpd.ReDraw の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'.ReDraw = False
			.SuspendLayout()

			''        For i = 1 To UBound(tbls)
			For i = 0 To wMaxRow
				''            If tbls(i, 1) = vbNullString And _
				''                tbls(i, 2) = vbNullString And _
				''                tbls(i, 3) = vbNullString And _
				''                tbls(i, 4) = vbNullString Then
				''                Debug.Print "aa" & i
				''            Else
				'UPGRADE_WARNING: オブジェクト tbls(i, 1) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				If tbls(i, 0) = vbNullString Then
					CriticalAlarm(i + 1 & "行目の名称を入力して下さい。")
					'行の位置付け
					'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'.Col = 1
					'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'.Row = i
					'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'.Action = FPSpreadADO.ActionConstants.ActionActiveCell
					.ActiveSheet.SetActiveCell(i, 0)
					'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Focus()
					'行の色を変える
					'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'.Col = -1
					'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'.Row = i
					'UPGRADE_WARNING: オブジェクト fpSpd.BackColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.ActiveSheet.Rows(i).BackColor = System.Drawing.ColorTranslator.FromOle(&HC0C0FF)
					'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Focus()

					.ResumeLayout(True)

					Exit Function
				End If

				'UPGRADE_WARNING: オブジェクト tbls(i, 2) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				If tbls(i, 1) = vbNullString Then
					CriticalAlarm(i + 1 & "行目の略称を入力して下さい。")
					'行の位置付け
					'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'.Col = 1
					'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'.Row = i
					'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'.Action = FPSpreadADO.ActionConstants.ActionActiveCell
					.ActiveSheet.SetActiveCell(i, 0)
					'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Focus()
					'行の色を変える
					'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'.Col = -1
					'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'.Row = i
					'UPGRADE_WARNING: オブジェクト fpSpd.BackColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.ActiveSheet.Rows(i).BackColor = System.Drawing.ColorTranslator.FromOle(&HC0C0FF)
					'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Focus()

					.ResumeLayout(True)

					Exit Function
					'                Else
					'                If WkTBLS(i).WKMeisyo = vbNullString Then
					'                    CriticalAlarm i & "行目の名称を入力して下さい。"
					'                    '行の位置付け
					'                    .Col = 1
					'                    .Row = i
					'                    .Action = ActionActiveCell
					'                    .SetFocus
					'                    '行の色を変える
					'                    .Col = -1
					'                    .Row = i
					'                    .BackColor = &HC0C0FF
					'                    .SetFocus
					'                    Exit Function
					'                End If
				End If

				'納期・時間は未入力可能にする。
				'ただし、受注区分が「確定」の時はチェックする。[SnwMT02F02]の[Upload_Chk]で行う。
				''                If IsDate(tbls(i, 3)) = False Then
				''                    CriticalAlarm i & "行目の納期が不正です。"
				''                    '行の位置付け
				''                    .Col = 1
				''                    .Row = i
				''                    .Action = ActionActiveCell
				''                    .SetFocus
				''                    '行の色を変える
				''                    .Col = -1
				''                    .Row = i
				''                    .BackColor = &HC0C0FF
				''                    .SetFocus
				''                    Exit Function
				''                End If
				''
				''                If IsDate(tbls(i, 4)) = False Then
				''                    CriticalAlarm i & "行目の時間が不正です。"
				''                    '行の位置付け
				''                    .Col = 1
				''                    .Row = i
				''                    .Action = ActionActiveCell
				''                    .SetFocus
				''                    '行の色を変える
				''                    .Col = -1
				''                    .Row = i
				''                    .BackColor = &HC0C0FF
				''                    .SetFocus
				''                    Exit Function
				''                End If
				''            End If
			Next

			'UPGRADE_WARNING: オブジェクト fpSpd.ReDraw の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'.ReDraw = True
			.ResumeLayout(True)
		End With

		Upload_Chk = True

	End Function

	Private Function Upload() As Boolean
		Dim rs As ADODB.Recordset
		Dim sql As String
		Dim i As Integer
		Dim j As Integer

		Upload = False
		'マウスポインターを砂時計にする
		HourGlass(True)

		'    Erase gSiwakeTBL
		ReDim gSiwakeTBL(UBound(tbls, 2), UBound(tbls))

		For i = 0 To UBound(tbls)
			For j = 0 To UBound(tbls, 2)
				'UPGRADE_WARNING: オブジェクト tbls(i, j) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト gSiwakeTBL() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'gSiwakeTBL(j - 1, i - 1) = tbls(i, j)
				gSiwakeTBL(j, i) = tbls(i, j)
				'2011/11/29 ADD↓
				'時間がスペースならば00:00をセット
				'UPGRADE_WARNING: オブジェクト tbls(i, 4) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				If j = 3 AndAlso String.IsNullOrEmpty(tbls(i, 3)) Then
					'UPGRADE_WARNING: オブジェクト gSiwakeTBL() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'gSiwakeTBL(j - 1, i - 1) = "00:00"
					'gSiwakeTBL(j, i) = "00:00"
					gSiwakeTBL(j, i) = DateTime.ParseExact("00:00", "HH:mm", Nothing)
				End If
				'2011/11/29 ADD↑
			Next
		Next

		'      Cn.BeginTrans   '---トランザクションの開始
		'      On Error GoTo Trans_err
		'  
		'      SQL = "DELETE FROM TD見積シート内訳名称 " _
		'          & "WHERE 見積番号 = " & pMituNo
		'      Cn.Execute SQL
		'  
		'      For i = 1 To UBound(WkTBLS)
		'  '        If WkTBLS(i).WKMeisyo <> vbNullString Then
		'  '
		'              SQL = "INSERT INTO TD見積シート内訳名称 " _
		'                  & "(見積番号, 仕分番号, 名称, 略称, 納期, 時間) " _
		'                  & "VALUES " _
		'                  & "(" _
		'                  & pMituNo & "," _
		'                  & i & "," _
		'                  & "'" & SQLString(WkTBLS(i).WKMeisyo) & "'," _
		'                  & "'" & SQLString(WkTBLS(i).WKRyaku) & "'," _
		'                  & IIf(WkTBLS(i).WKNouki = vbNullString, "Null", SQLDate(WkTBLS(i).WKNouki, DBType)) & "," _
		'                  & IIf(WkTBLS(i).WKJikan = vbNullString, "Null", "'" & SQLString(WkTBLS(i).WKJikan) & "'") _
		'                  & ")"
		'  
		'              Cn.Execute SQL
		'  '        End If
		'      Next
		'  
		'      Cn.CommitTrans  '---トランザクションをコミットする
		Upload = True

Trans_Correct:
		On Error GoTo 0

		HourGlass(False)
		Exit Function

Trans_err: '---エラー時
		MsgBox(Err.Number & " " & Err.Description)
		Cn.RollbackTrans() 'トランザクションを破棄する
		Resume Trans_Correct
	End Function

	Private Sub SetHeader()
		Dim i As Integer

		With CType(pParentForm.Controls("fpSpd"), FarPoint.Win.Spread.FpSpread)
			For i = 0 To UBound(tbls)
				'UPGRADE_WARNING: オブジェクト tbls(i, 1) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				If tbls(i, 0) = vbNullString Then
					'            Exit For
					'UPGRADE_WARNING: オブジェクト pParentForm.Controls().Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'.Row = 0
					'UPGRADE_WARNING: オブジェクト pParentForm.Controls().Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'.Col = i + pSetCol - 1
					'                .Text = WkTBLS(i).WKRyaku
					'.Text = CStr(i)
					.ActiveSheet.SetColumnLabel(0, i + pSetCol, CStr(i + 1))
				Else
					'UPGRADE_WARNING: オブジェクト pParentForm.Controls().Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'.Row = 0
					'UPGRADE_WARNING: オブジェクト pParentForm.Controls().Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'.Col = i + pSetCol - 1
					'UPGRADE_WARNING: オブジェクト tbls(i, 2) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'.Text = tbls(i, 2)
					.ActiveSheet.SetColumnLabel(0, i + pSetCol, tbls(i, 1))
				End If
			Next
		End With
		'UPGRADE_WARNING: オブジェクト pParentForm.Controls().Item の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'CType(pParentForm.Controls("LcbFunc"), Object).Item(10).Text = "ﾁｪｯｸ"
		pParentForm.LcbFunc(10).Text = "ﾁｪｯｸ"
		'With pParentForm.Controls("fpSpd")
		'    For i = 1 To UBound(WkTBLS)
		'        If WkTBLS(i).WKMeisyo = vbNullString Then
		'        '   Exit For
		'            .Row = 0
		'            .Col = i + pSetCol - 1
		'        '   .Text = WkTBLS(i).WKRyaku
		'            .Text = i
		'        Else
		'            .Row = 0
		'            .Col = i + pSetCol - 1
		'            .Text = WkTBLS(i).WKRyaku
		'        End If
		'    Next
		'End With
	End Sub

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

	'項目をセット
	WriteOnly Property SetCol() As Integer
		Set(ByVal Value As Integer)
			pSetCol = Value
		End Set
	End Property

	'2020/06/23 ADD↓
	Private Sub Tx_見積番号_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Tx_見積番号.Enter
		If Item_Check((Tx_見積番号.TabIndex)) = False Then
			Exit Sub
		End If

		'    sb_Msg.Panels(1).Text = "見積番号を入力して下さい。　選択画面：Space"
		PreviousControl = Me.ActiveControl
	End Sub

	Private Sub Tx_見積番号_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles Tx_見積番号.KeyPress
		Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
		'2008/01/23 ADD↓
		If KeyAscii = Asc(" ") And (Tx_見積番号.SelStart = 0 And Tx_見積番号.SelLength = Len(Tx_見積番号.Text)) Then
			KeyAscii = 0
			'---参照画面表示
			'        SelectF = True
			SnwMT03F00S.ResCodeCTL = Tx_見積番号
			'VB6.ShowForm(SnwMT03F00S, VB6.FormShowConstants.Modal, Me)
			SnwMT03F00S.ShowDialog(Me)
			If Tx_見積番号.Tag <> "" Then
				ReturnF = True
				Tx_見積番号.NextSetFocus()
			Else
				Tx_見積番号.Focus()
			End If
		End If
		'2008/01/23 ADD↑
	End Sub

	Private Sub Tx_見積番号_KeyDown(sender As Object, e As KeyEventArgs) Handles Tx_見積番号.KeyDown
		ReturnF = True
	End Sub

	Private Sub Tx_見積番号_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Tx_見積番号.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		sb_Msg.Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_見積番号.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			Tx_見積番号.Undo()
		End If
		ReturnF = False
	End Sub
	'2020/06/23 ADD↑

End Class