Option Strict Off
Option Explicit On
Imports FarPoint.Win.Spread

''' <summary>
''' '2014/11/11 LineSwap2追加 セルブロックでの入替用
''' '2014/11/13 Search追加　全セル内検索用
''' '2017/03/30 ColSwap 列の入替（LineSwap2パクリ版） 
''' </summary>
Friend Class clsSPD
	Implements IDisposable

	'ﾌﾟﾛﾊﾟﾃｨ値を保持するためのﾛｰｶﾙ変数。
	Private fpSpd As FpSpread 'ﾛｰｶﾙ ｺﾋﾟｰ

	Private HCol As Integer
	Private HRow As Integer

	Private disposedValue As Boolean

	''' <summary>
	''' プロパティ宣言
	''' </summary>
	''' <returns></returns>
	Public Property CtlSpd() As System.Windows.Forms.Control
		Get
			'ﾌﾟﾛﾊﾟﾃｨの値を取得するときに、代入式の右辺で使用します。
			'Syntax: Debug.Print X.Spread
			CtlSpd = fpSpd
		End Get
		Set(ByVal Value As System.Windows.Forms.Control)
			'ﾌﾟﾛﾊﾟﾃｨにｵﾌﾞｼﾞｪｸﾄを代入するときに、Set ｽﾃｰﾄﾒﾝﾄの左辺で使用します。
			'Syntax: Set x.Spread = Form1
			fpSpd = Value
		End Set
	End Property

	''' <summary>
	''' 行の挿入
	''' </summary>
	''' <param name="MaxRow"></param>
	''' <param name="iRow"></param>
	''' <returns>
	''' 戻り値:
	''' ・0  - 正常
	''' ・-1 - 上限
	''' </returns>
	Public Function LineInsert(MaxRow As Integer, Optional ByRef iRow As Integer = 0) As Integer
		Dim bk_formula(,) As String
		Dim bk_numberFormat(,) As String
		Dim i As Integer
		Dim j As Integer

		LineInsert = 0

		With fpSpd.ActiveSheet
			'初期値設定
			If iRow = 0 Then
				iRow = .ActiveRowIndex
			End If

			HCol = 0
			HRow = .ActiveRowIndex

			'最終行
			If .ActiveRowIndex + 1 = MaxRow Then
				LineInsert = -1
				Exit Function
			End If
			
			'最終行の製品Noに入力されているか？
			If .GetText(MaxRow - 1, 8) <> "" Then
				LineInsert = -1
				Exit Function
			End If
			
			'最終行の見積区分に入力されているか？
			If .GetText(MaxRow - 1, 1) <> "" Then
				LineInsert = -1
				Exit Function
			End If
		End With

		Dim workbook = fpSpd.AsWorkbook()

		' 配列の確保
		ReDim bk_formula(fpSpd.ActiveSheet.ActiveRowIndex, fpSpd.ActiveSheet.ColumnCount - 1)
		ReDim bk_numberFormat(fpSpd.ActiveSheet.ActiveRowIndex, fpSpd.ActiveSheet.ColumnCount - 1)

		fpSpd.SuspendLayout()

		' 自動再計算の停止
		fpSpd.ActiveSheet.AutoCalculation = False

		With workbook
			' 数式の保存
			For i = fpSpd.ActiveSheet.ActiveRowIndex To fpSpd.ActiveSheet.ActiveRowIndex
				For j = 0 To .ActiveSheet.ColumnCount - 1
					bk_formula(i, j) = workbook.ActiveSheet.Cells(i, j).Formula
					bk_numberFormat(i, j) = workbook.ActiveSheet.Cells(i, j).NumberFormat
				Next
			Next

			fpSpd.ActiveSheet.AddRows(iRow, 1)
			fpSpd.ActiveSheet.Rows(iRow).Resizable = False

			With fpSpd
				.ActiveSheet.CopyRange(HRow, HCol, .ActiveSheet.ActiveRowIndex, 0, 1, .ActiveSheet.ColumnCount, True)
			End With

			' 数式の復元
			For i = fpSpd.ActiveSheet.ActiveRowIndex To fpSpd.ActiveSheet.ActiveRowIndex
				For j = 0 To .ActiveSheet.ColumnCount - 1
					'Debug.WriteLine($"CellType: {fpSpd.ActiveSheet.GetCellType(i, j)}")
					Select Case True
						Case TypeOf fpSpd.ActiveSheet.GetCellType(i, j) Is GrapeCity.Win.Spread.InputMan.CellType.GcTextBoxCellType
						Case TypeOf fpSpd.ActiveSheet.GetCellType(i, j) Is GrapeCity.Win.Spread.InputMan.CellType.GcDateTimeCellType
						Case Else
							.ActiveSheet.Cells(i, j).Formula = bk_formula(i, j)
							.ActiveSheet.Cells(i, j).NumberFormat = bk_numberFormat(i, j)
					End Select
				Next
			Next
		End With

		fpSpd.ActiveSheet.RowCount = MaxRow

		' 自動再計算の再開
		fpSpd.ActiveSheet.AutoCalculation = True
		fpSpd.ResumeLayout(True)

		fpSpd.Focus()

	End Function

	''' <summary>
	''' 行の削除
	''' </summary>
	''' <param name="iRow"></param>
	Public Sub LineDelete(Optional ByRef iRow As Integer = 0)
		Dim bk_formula(,) As String
		Dim bk_numberFormat(,) As String
		Dim i As Integer
		Dim j As Integer

		With fpSpd.ActiveSheet
			'初期値設定
			If iRow = 0 Then
				iRow = .ActiveRowIndex
			End If

		End With

		Dim workbook = fpSpd.AsWorkbook()

		' 配列の確保
		ReDim bk_formula(fpSpd.ActiveSheet.RowCount - 1, fpSpd.ActiveSheet.ColumnCount - 1)
		ReDim bk_numberFormat(fpSpd.ActiveSheet.RowCount - 1, fpSpd.ActiveSheet.ColumnCount - 1)

		With workbook
			' 数式の保存
			For i = fpSpd.ActiveSheet.RowCount - 1 To fpSpd.ActiveSheet.RowCount - 1
				For j = 0 To .ActiveSheet.ColumnCount - 1
					bk_formula(i, j) = workbook.ActiveSheet.Cells(i, j).Formula
					bk_numberFormat(i, j) = workbook.ActiveSheet.Cells(i, j).NumberFormat
				Next
			Next

			.ActiveSheet.RemoveRows(iRow, 1)

			fpSpd.ActiveSheet.AddRows(fpSpd.ActiveSheet.RowCount, 1)
			fpSpd.ActiveSheet.Rows(fpSpd.ActiveSheet.RowCount - 1).Resizable = False

			HCol = 0
			HRow = fpSpd.ActiveSheet.RowCount + 1

			With fpSpd
				.ActiveSheet.CopyRange(HRow, HCol, .ActiveSheet.ActiveRowIndex, 0, 1, .ActiveSheet.ColumnCount, True)
				.ActiveSheet.Rows(.ActiveSheet.RowCount - 1).Height = 23
			End With

			' 数式の復元
			For i = fpSpd.ActiveSheet.RowCount - 1 To fpSpd.ActiveSheet.RowCount - 1
				For j = 0 To .ActiveSheet.ColumnCount - 1
					'Debug.WriteLine($"CellType: {fpSpd.ActiveSheet.GetCellType(i, j)}")
					Select Case True
						Case TypeOf fpSpd.ActiveSheet.GetCellType(i, j) Is GrapeCity.Win.Spread.InputMan.CellType.GcTextBoxCellType
						Case TypeOf fpSpd.ActiveSheet.GetCellType(i, j) Is GrapeCity.Win.Spread.InputMan.CellType.GcDateTimeCellType
						Case Else
							.ActiveSheet.Cells(i, j).Formula = bk_formula(i, j)
							.ActiveSheet.Cells(i, j).NumberFormat = bk_numberFormat(i, j)
					End Select
				Next
			Next
		End With

		fpSpd.Focus()
	End Sub

	''' <summary>
	''' 行の入れ替え(未使用部品)
	''' </summary>
	''' <param name="Dest"></param>
	Public Sub LineSwap(Optional ByRef Dest As Integer = 0)

		Dim Destination As Integer

		fpSpd.SuspendLayout()

		With fpSpd
			'NOTE SS			'        Debug.Print "1:" & fpSpd.DataRowCnt
			'NOTE SS			If (.ActiveRow + Dest >= 1) And (.ActiveRow + Dest <= .MaxRows) Then
			'NOTE SS				.ReDraw = False
			'NOTE SS				.col = 1
			'NOTE SS				.Col2 = .MaxCols
			'NOTE SS				'DataRowCntの同期
			'NOTE SS				If Dest < 0 Then
			'NOTE SS					.Row = .ActiveRow + Dest
			'NOTE SS					.Row2 = .Row
			'NOTE SS					.DestRow = .ActiveRow
			'NOTE SS				Else
			'NOTE SS					.Row = .ActiveRow
			'NOTE SS					.Row2 = .ActiveRow
			'NOTE SS					.DestRow = .ActiveRow + Dest
			'NOTE SS				End If
			'NOTE SS				.DestCol = 1
			'NOTE SS				.Action = FPSpreadADO.ActionConstants.ActionSwapRange
			'NOTE SS				'            Debug.Print "2:" & fpSpd.DataRowCnt
			'NOTE SS				'DataRowCntの同期
			'NOTE SS				'  '            .ClipValue = .ClipValue
			'NOTE SS				'  ''            .Col = 1
			'NOTE SS				'  ''            .Row = .MaxRows
			'NOTE SS				.MaxRows = .MaxRows + 1
			'NOTE SS				.col = 1
			'NOTE SS				.Row = .MaxRows
			'NOTE SS				.Value = CStr(1)
			'NOTE SS				.MaxRows = .MaxRows - 1
			'NOTE SS
			'NOTE SS				'  ''            .Value = 1
			'NOTE SS				'  '''''''''''''            .Value = .Value
			'NOTE SS				'UPGRADE_NOTE: Refresh は CtlRefresh にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
			'NOTE SS				.CtlRefresh()
			'NOTE SS				'            Debug.Print "3:" & fpSpd.DataRowCnt
			'NOTE SS				'カーソルを移動位置に移動
			'NOTE SS				.col = .ActiveCol
			'NOTE SS				.Row = .ActiveRow + Dest
			'NOTE SS				.Action = FPSpreadADO.ActionConstants.ActionActiveCell
			'NOTE SS				'  ''            .Col2 = 1
			'NOTE SS				'  ''            .Row2 = .ActiveRow
			'NOTE SS				'  ''            .ClipValue = .ClipValue
			'NOTE SS				.Value = .Value
			'NOTE SS				'UPGRADE_NOTE: Refresh は CtlRefresh にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
			'NOTE SS				.CtlRefresh()
			'NOTE SS				.ReDraw = True
			'NOTE SS				'  ''            Debug.Print "SPDLineSwap:ok"
			'NOTE SS			End If
			'NOTE SS			'        Debug.Print "4:" & fpSpd.DataRowCnt

			If Dest < 0 Then
				Destination = .ActiveSheet.ActiveRowIndex - 1
			Else
				Destination = .ActiveSheet.ActiveRowIndex + 1
			End If
			If Destination < 0 Then
				Destination = 0
				If .ActiveSheet.ActiveRowIndex = 0 Then
					Exit Sub
				End If
			ElseIf Destination > .ActiveSheet.RowCount Then
				Destination = .ActiveSheet.RowCount
				If .ActiveSheet.ActiveRowIndex = .ActiveSheet.RowCount - 1 Then
					Exit Sub
				End If
			End If

			'fromRow    :交換元のセル範囲の左上隅のセルの行インデックス
			'fromColumn :交換元のセル範囲の左上隅のセルの列インデックス
			'toRow      :交換先範囲の左上隅のセルの行インデックス
			'toColumn   :交換先範囲の左上隅のセルの列インデックス
			'rowCount   :セル範囲の行数
			'columnCount:セル範囲の列数
			'dataOnly   :データのみを交換するか、データと書式を共に交換するかを表すブール値
			.ActiveSheet.SwapRange(.ActiveSheet.ActiveRowIndex, 0, Destination, 0, 1, .ActiveSheet.ColumnCount, False)

			'アクティブセルの行 移動
			If Dest < 0 Then
				.ActiveSheet.SetActiveCell(.ActiveSheet.ActiveRowIndex - 1, .ActiveSheet.ActiveColumnIndex)
			Else
				.ActiveSheet.SetActiveCell(.ActiveSheet.ActiveRowIndex + 1, .ActiveSheet.ActiveColumnIndex)
			End If

			.Focus()
		End With
		fpSpd.ResumeLayout(True)
	End Sub

	'2014/11/11 ADD↓
	''' <summary>
	''' 行の入れ替え
	''' </summary>
	''' <param name="MOVEROW"></param>
	Public Sub LineSwap2(Optional ByRef MOVEROW As Integer = 0)
		Dim Rows As Integer
		Rows = fpSpd.ActiveSheet.Models.Selection.LeadRow - fpSpd.ActiveSheet.Models.Selection.AnchorRow + 1

		Dim StartRows As Integer
		StartRows = fpSpd.ActiveSheet.Models.Selection.AnchorRow

		Dim EndRows As Integer
		EndRows = fpSpd.ActiveSheet.Models.Selection.AnchorRow + Rows - 1

		Dim ActiveRows As Integer
		If MOVEROW < 0 Then
			ActiveRows = fpSpd.ActiveSheet.Models.Selection.AnchorRow - 1
		Else
			ActiveRows = fpSpd.ActiveSheet.Models.Selection.AnchorRow + 1
		End If

		Dim i As Integer
		Dim j As Integer

		Try
			If MOVEROW < 0 Then
				j = fpSpd.ActiveSheet.Models.Selection.AnchorRow - 1
			Else
				j = fpSpd.ActiveSheet.Models.Selection.LeadRow + 1
			End If
			If MOVEROW < 0 Then
				For i = StartRows To EndRows
					'fromRow    :交換元のセル範囲の左上隅のセルの行インデックス
					'fromColumn :交換元のセル範囲の左上隅のセルの列インデックス
					'toRow      :交換先範囲の左上隅のセルの行インデックス
					'toColumn   :交換先範囲の左上隅のセルの列インデックス
					'rowCount   :セル範囲の行数
					'columnCount:セル範囲の列数
					'dataOnly   :データのみを交換するか、データと書式を共に交換するかを表すブール値
					fpSpd.ActiveSheet.SwapRange(i, 0, j, 0, 1, fpSpd.ActiveSheet.ColumnCount, False)
					j += 1
				Next
			Else
				For i = EndRows To StartRows Step -1
					'fromRow    :交換元のセル範囲の左上隅のセルの行インデックス
					'fromColumn :交換元のセル範囲の左上隅のセルの列インデックス
					'toRow      :交換先範囲の左上隅のセルの行インデックス
					'toColumn   :交換先範囲の左上隅のセルの列インデックス
					'rowCount   :セル範囲の行数
					'columnCount:セル範囲の列数
					'dataOnly   :データのみを交換するか、データと書式を共に交換するかを表すブール値
					fpSpd.ActiveSheet.SwapRange(i, 0, j, 0, 1, fpSpd.ActiveSheet.ColumnCount, False)
					j -= 1
				Next
			End If

		Catch ex As InvalidOperationException
			Debug.WriteLine($"Exception：{ex}")
		Catch ex As Exception
			Debug.WriteLine($"Exception：{ex}")
		End Try

		'アクティブセルの行 移動
		fpSpd.ActiveSheet.SetActiveCell(ActiveRows, 0)
		'選択範囲
		fpSpd.ActiveSheet.ClearSelection()
		fpSpd.ActiveSheet.AddSelection(ActiveRows, 0, Rows, fpSpd.ActiveSheet.ColumnCount)
	End Sub
	'2014/11/11 ADD↑

	'2017/03/30 ADD↓
	''' <summary>
	''' 列の入れ替え
	''' </summary>
	''' <param name="minCol"></param>
	''' <param name="maxCol"></param>
	''' <param name="MOVECOL"></param>
	Public Sub ColSwap(ByRef minCol As Integer, ByRef maxCol As Integer, Optional ByRef MOVECOL As Integer = 0)
		'NOTE SS Dim COLS As Integer
		'NOTE SS COLS = fpSpd.SelBlockCol2 - fpSpd.SelBlockCol + 1
		Dim Dest As Integer

		fpSpd.SuspendLayout()

		With fpSpd
			'NOTE SS			'        Debug.Print "1:" & fpSpd.DataRowCnt
			'NOTE SS			If (.ActiveCol + MOVECOL >= minCol) And (.ActiveCol + MOVECOL + (COLS - 1) <= maxCol) Then
			'NOTE SS				.ReDraw = False
			'NOTE SS				.Row = 1
			'NOTE SS				.Row2 = .MaxRows
			'NOTE SS				'DataRowCntの同期
			'NOTE SS				If MOVECOL < 0 Then
			'NOTE SS					.col = .ActiveCol
			'NOTE SS					.Col2 = .col + COLS - 1
			'NOTE SS					.DestCol = .ActiveCol + MOVECOL
			'NOTE SS				Else
			'NOTE SS					.col = .ActiveCol
			'NOTE SS					.Col2 = .col + COLS - 1
			'NOTE SS					.DestCol = .ActiveCol + MOVECOL
			'NOTE SS				End If
			'NOTE SS				.DestRow = 1
			'NOTE SS				.Action = FPSpreadADO.ActionConstants.ActionSwapRange
			'NOTE SS				'            Debug.Print "2:" & fpSpd.DataRowCnt
			'NOTE SS				'DataRowCntの同期
			'NOTE SS				'  ''            .ClipValue = .ClipValue
			'NOTE SS				'  '            .Col = 1
			'NOTE SS				'  '            .Row = .MaxRows
			'NOTE SS				.MaxCols = .MaxCols + 1
			'NOTE SS				.Row = 1
			'NOTE SS				.col = .MaxCols
			'NOTE SS				.Value = CStr(1)
			'NOTE SS				.MaxCols = .MaxCols - 1
			'NOTE SS
			'NOTE SS				'  '            .Value = 1
			'NOTE SS				'  '''''''''''''            .Value = .Value
			'NOTE SS				'UPGRADE_NOTE: Refresh は CtlRefresh にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
			'NOTE SS				.CtlRefresh()
			'NOTE SS				'            Debug.Print "3:" & fpSpd.DataRowCnt
			'NOTE SS				'カーソルを移動位置に移動
			'NOTE SS				.Row = .ActiveRow
			'NOTE SS				.col = .ActiveCol + MOVECOL
			'NOTE SS				.Col2 = .col + COLS - 1
			'NOTE SS				.BlockMode = True
			'NOTE SS				If .IsBlockSelected = True Then
			'NOTE SS					.Action = FPSpreadADO.ActionConstants.ActionSelectBlock
			'NOTE SS				Else
			'NOTE SS					.Action = FPSpreadADO.ActionConstants.ActionActiveCell
			'NOTE SS				End If
			'NOTE SS				.BlockMode = False
			'NOTE SS				'  ''            .Col2 = 1
			'NOTE SS				'  ''            .Row2 = .ActiveRow
			'NOTE SS				'  ''            .ClipValue = .ClipValue
			'NOTE SS				.Value = .Value
			'NOTE SS				'UPGRADE_NOTE: Refresh は CtlRefresh にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
			'NOTE SS				.CtlRefresh()
			'NOTE SS				.ReDraw = True
			'NOTE SS				'  ''            Debug.Print "SPDLineSwap:ok"
			'NOTE SS			End If
			'NOTE SS			'        Debug.Print "4:" & fpSpd.DataRowCnt

			If MOVECOL < 0 Then
				Dest = .ActiveSheet.ActiveColumnIndex - 1
			Else
				Dest = .ActiveSheet.ActiveColumnIndex + 1
			End If
			If Dest < 0 Then
				Dest = 0
				If .ActiveSheet.ActiveColumnIndex = 0 Then
					Exit Sub
				End If
			ElseIf Dest > .ActiveSheet.ColumnCount Then
				Dest = .ActiveSheet.ColumnCount
				If .ActiveSheet.ActiveRowIndex = .ActiveSheet.ColumnCount - 1 Then
					Exit Sub
				End If
			End If

			'fromRow    :交換元のセル範囲の左上隅のセルの行インデックス
			'fromColumn :交換元のセル範囲の左上隅のセルの列インデックス
			'toRow      :交換先範囲の左上隅のセルの行インデックス
			'toColumn   :交換先範囲の左上隅のセルの列インデックス
			'rowCount   :セル範囲の行数
			'columnCount:セル範囲の列数
			'dataOnly   :データのみを交換するか、データと書式を共に交換するかを表すブール値
			.ActiveSheet.SwapRange(0, .ActiveSheet.ActiveColumnIndex, 0, Dest, .ActiveSheet.RowCount, 1, False)

			'アクティブセルの行 移動
			If MOVECOL < 0 Then
				.ActiveSheet.SetActiveCell(.ActiveSheet.ActiveRowIndex, .ActiveSheet.ActiveColumnIndex - 1)
			Else
				.ActiveSheet.SetActiveCell(.ActiveSheet.ActiveRowIndex, .ActiveSheet.ActiveColumnIndex + 1)
			End If

			.Focus()
		End With
		fpSpd.ResumeLayout(True)
	End Sub
	'2017/03/30 ADD↑

	''' <summary>
	''' 行の色を変える(未使用部品)
	''' </summary>
	''' <param name="lColor"></param>
	''' <param name="iRow"></param>
	Public Sub LineBColorChange(ByRef lColor As Long, Optional ByRef iRow As Integer = 0)
		With fpSpd
			'初期値設定
			If iRow = 0 Then
				'NOTE SS iRow = .ActiveRow 'ｱｸﾃｨﾌﾞ行を指定
				iRow = .ActiveSheet.ActiveRowIndex
			End If
			.ActiveSheet.Rows(iRow).BackColor = System.Drawing.ColorTranslator.FromOle(lColor)
		End With
	End Sub

	''' <summary>
	''' 複写する為の行情報の保持
	''' </summary>
	Public Sub LineCopyHold()
		With fpSpd
			'NOTE SS			HCol = 1
			HCol = 0
			'NOTE SS 	HRow = .ActiveRow
			HRow = .ActiveSheet.ActiveRowIndex
			.Focus()
			'Debug.Print("The Hold of HCol is: " & HCol.ToString & "," & .ActiveSheet.ColumnCount.ToString)
			'Debug.Print("The Hold of HRow is: " & HRow.ToString & "," & .ActiveSheet.ActiveRowIndex.ToString)
		End With
	End Sub

	''' <summary>
	''' コピーした行の複写
	''' </summary>
	Public Sub LinePaste()
		fpSpd.SuspendLayout()
		With fpSpd
			'NOTE SS			.ReDraw = False
			'NOTE SS			.col = HCol
			'NOTE SS			.Row = HRow
			'NOTE SS			.Col2 = .MaxCols
			'NOTE SS			.Row2 = HRow
			'NOTE SS			.DestCol = 1
			'NOTE SS			.DestRow = .ActiveRow
			'NOTE SS			.Action = FPSpreadADO.ActionConstants.ActionCopyRange
			'DataRowCntの同期
			'NOTE SS			.Value = .Value
			'NOTE SS			.Focus()
			'NOTE SS			.ReDraw = True

			'fromRow    :コピーする範囲の左上隅のセルの行インデックス
			'fromColumn :コピーする範囲の左上隅のセルの列インデックス
			'toRow      :コピー先の左上隅のセルの行インデックス
			'toColumn   :コピー先の左上隅のセルの列インデックス
			'rowCount   :範囲の行数
			'columnCount:範囲の列数
			'dataOnly   :データのみを交換するか、データと書式を共に交換するかを表すブール値
			.ActiveSheet.CopyRange(HRow, HCol, .ActiveSheet.ActiveRowIndex, 0, 1, .ActiveSheet.ColumnCount, False)

			'Debug.Print("The Paste of HRow is: " & HRow.ToString & "," & .ActiveSheet.ActiveRowIndex.ToString)
			'Debug.Print("The Paste of HCol is: " & HCol.ToString & "," & .ActiveSheet.ColumnCount.ToString)
		End With
		fpSpd.ResumeLayout(True)
	End Sub

	'  Public Sub SetAryItems(Ary() As Variant)
	'      With fpSpd
	'          .SetArray 1, 1, Ary()
	'          .Col = 1
	'          .Row = 1
	'          .Action = ActionActiveCell
	'      End With
	'  End Sub
	'  
	'  Public Function sprVAV2VA(InAr As Variant, Optional FirstDimensionIsCol As Boolean = True) As Variant()
	'  'バリアント変数 InAr が保持している配列を バリアント配列で返します
	'  '
	'  '引数 FirstDimensionIsCol は、バリアント変数 InAr が保持している配列を(Col,Row)あるいは(Row,Col)の
	'  'どちらで返すかを指定します
	'  
	'      Dim InAr() As Variant
	'      Dim OutAr() As Variant
	'      Dim Row As Long, Col As Long
	'      Dim Dimension As Integer
	'  
	'      If (VarType(InAr) And vbArray) Then
	'          Err.Clear
	'          On Error Resume Next
	'          Row = UBound(InAr, 2)
	'          If Err Then
	'              Dimension = 1
	'          Else
	'              Dimension = 2
	'          End If
	'          On Error GoTo 0
	'          If Dimension = 1 Then
	'              If FirstDimensionIsCol = True Then
	'                  ReDim OutAr(0 To 0, LBound(InAr) To UBound(InAr))
	'                  For Col = LBound(InAr) To UBound(InAr)
	'                      OutAr(0, Col) = InAr(Col)
	'                  Next
	'              Else
	'                  ReDim OutAr(LBound(InAr) To UBound(InAr), 0 To 0)
	'                  For Col = LBound(InAr) To UBound(InAr)
	'                      OutAr(Col, 0) = InAr(Col)
	'                  Next
	'              End If
	'          Else
	'              If FirstDimensionIsCol = True Then
	'                  ReDim OutAr(LBound(InAr, 2) To UBound(InAr, 2), LBound(InAr, 1) To UBound(InAr, 1))
	'                  For Row = LBound(InAr, 2) To UBound(InAr, 2)
	'                      For Col = LBound(InAr, 1) To UBound(InAr, 1)
	'                          OutAr(Row, Col) = InAr(Col, Row)
	'                      Next
	'                  Next
	'              Else
	'                  ReDim OutAr(LBound(InAr, 1) To UBound(InAr, 1), LBound(InAr, 2) To UBound(InAr, 2))
	'                  For Row = LBound(InAr, 2) To UBound(InAr, 2)
	'                      For Col = LBound(InAr, 1) To UBound(InAr, 1)
	'                          OutAr(Col, Row) = InAr(Col, Row)
	'                      Next
	'                  Next
	'              End If
	'          End If
	'      End If
	'      sprVAV2VA = OutAr()
	'  End Function

	''' <summary>
	''' スプレッドシートの値をクリアする（ブロック指定）
	''' </summary>
	Public Sub SprClearText()

		With fpSpd
			'NOTE SS			.ReDraw = False
			'NOTE SS			.Row = 1 ' セルブロックを設定
			'NOTE SS			.col = 1
			'NOTE SS			.Row2 = .MaxRows
			'NOTE SS			.Col2 = .MaxCols
			'NOTE SS			.BlockMode = True ' セルブロックを有効に設定
			'NOTE SS			.Action = FPSpreadADO.ActionConstants.ActionClearText ' 値とセル型の設定を消去
			'NOTE SS			.BlockMode = False ' セルブロックを無効に設定
			'NOTE SS			.ReDraw = True

			Dim dataModel As New FarPoint.Win.Spread.Model.DefaultSheetDataModel(.ActiveSheet.RowCount, .ActiveSheet.ColumnCount)
			.ActiveSheet.Models.Data = dataModel

			'指定したセル範囲からすべてのデータを削除します。
			'Row        : クリアする範囲の開始行インデックス
			'Column     : クリアする範囲の開始列インデックス
			'rowCount   : クリアする行数
			'columnCount: クリアする列数
			dataModel.ClearData(0, 0, .ActiveSheet.RowCount, .ActiveSheet.ColumnCount)

			dataModel = Nothing

			' 全行の背景色を設定
			For i As Integer = 0 To .ActiveSheet.RowCount - 1
				.ActiveSheet.Rows(i).BackColor = System.Drawing.ColorTranslator.FromOle(&HFFFFFF)
			Next
		End With
	End Sub

	''' <summary>
	''' セル内検索 (未使用部品)
	''' </summary>
	''' <param name="lCol"></param>
	''' <param name="lRowStart"></param>
	''' <param name="lRowEnd"></param>
	''' <param name="Text"></param>
	''' <returns></returns>
	Public Function SearchCol(ByVal lCol As Integer, ByVal lRowStart As Integer, ByVal lRowEnd As Integer, ByVal Text As String) As Integer

		Dim WkTBLS(,) As Object
		Dim i As Integer

		SearchCol = -1

		'UPGRADE_WARNING: 配列 WkTBLS の下限が 1,1 から 0,0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"' をクリックしてください。
		'UPGRADE_ISSUE: As Variant が ReDim WkTBLS(1 To fpSpd.DataRowCnt, 1 To fpSpd.MaxCols) ステートメントから削除されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="19AFCB41-AA8E-4E6B-A441-A3E802E5FD64"' をクリックしてください。

		ReDim WkTBLS(fpSpd.ActiveSheet.RowCount, fpSpd.ActiveSheet.ColumnCount)

		WkTBLS = fpSpd.ActiveSheet.GetArray(0, 0, fpSpd.ActiveSheet.RowCount, fpSpd.ActiveSheet.ColumnCount)

		If lRowEnd = -1 Then
			'-1の場合最大行
			lRowEnd = fpSpd.ActiveSheet.RowCount
		End If

		For i = lRowStart To lRowEnd
			'UPGRADE_WARNING: オブジェクト WkTBLS() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If Trim(WkTBLS(i, lCol)) = Trim(Text) Then
				SearchCol = i
				'検索行を先頭に表示する
				fpSpd.SetViewportTopRow(0, i)
				Exit For
			End If
		Next

	End Function

	'2014/11/13 ADD↓
	''' <summary>
	''' 全セル内検索
	''' </summary>
	''' <param name="lRowStart"></param>
	''' <param name="lColStart"></param>
	''' <param name="Text"></param>
	''' <returns></returns>
	Public Function Search(ByVal lRowStart As Integer, ByVal lColStart As Integer, ByVal Text As String) As Integer

		Dim WkTBLS(,) As Object
		Dim i As Integer
		Dim j As Integer
		'Dim X As Integer

		Search = -1

		'UPGRADE_WARNING: 配列 WkTBLS の下限が 1,1 から 0,0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"' をクリックしてください。
		'UPGRADE_ISSUE: As Variant が ReDim WkTBLS(1 To fpSpd.DataRowCnt, 1 To fpSpd.MaxCols) ステートメントから削除されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="19AFCB41-AA8E-4E6B-A441-A3E802E5FD64"' をクリックしてください。
		'NOTE SS		ReDim WkTBLS(fpSpd.DataRowCnt, fpSpd.MaxCols)

		'NOTE SS		fpSpd.GetArray(1, 1, WkTBLS)

		'    If lRowEnd = -1 Then
		'        '-1の場合最大行
		'        lRowEnd = fpSpd.DataRowCnt
		'    End If
		'NOTE SS        Dim LoopFLG As Boolean

		'NOTE SS		For j = 1 To fpSpd.MaxCols
		'NOTE SS			For i = 1 To fpSpd.DataRowCnt
		'NOTE SS				fpSpd.col = j
		'NOTE SS				fpSpd.Row = i
		'NOTE SS				'            Debug.Print i, j
		'NOTE SS				''            If fpSpd.Lock = False Then
		'NOTE SS				If (i > lRowStart And j = lColStart) Or (j > lColStart) Then
		'NOTE SS					'UPGRADE_WARNING: オブジェクト WkTBLS() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'NOTE SS					If Trim(WkTBLS(i, j)) Like "*" & Trim(Text) & "*" Then
		'NOTE SS						Search = i
		'NOTE SS						fpSpd.col = j
		'NOTE SS						fpSpd.Row = i
		'NOTE SS
		'NOTE SS						' ''                        '指定セルがロックされていたら
		'NOTE SS						' ''                        'その行のロックされていないセルを探す
		'NOTE SS						' ''                        For x = j - 1 To 0 Step -1
		'NOTE SS						' ''                            If fpSpd.Lock = True Then
		'NOTE SS						' ''                                fpSpd.Col = x
		'NOTE SS						' ''                            Else
		'NOTE SS						' ''                                Exit For
		'NOTE SS						' ''                            End If
		'NOTE SS						' ''                        Next
		'NOTE SS
		'NOTE SS						LoopFLG = True
		'NOTE SS						Exit For
		'NOTE SS					End If
		'NOTE SS				End If
		'NOTE SS				''            End If
		'NOTE SS			Next
		'NOTE SS			If LoopFLG = True Then Exit For
		'NOTE SS		Next

		ReDim WkTBLS(fpSpd.ActiveSheet.RowCount, fpSpd.ActiveSheet.ColumnCount)

		'NOTE SS		fpSpd.GetArray(1, 1, WkTBLS)
		WkTBLS = fpSpd.ActiveSheet.GetArray(0, 0, fpSpd.ActiveSheet.RowCount, fpSpd.ActiveSheet.ColumnCount)

		Dim LoopFLG As Boolean

		For j = 0 To fpSpd.ActiveSheet.ColumnCount - 1
			For i = 0 To fpSpd.ActiveSheet.RowCount - 1
				fpSpd.ActiveSheet.SetActiveCell(i, j)
				'Debug.Print("The Col for j is: " & j.ToString & "The Row for i is: " & i.ToString)

				If (i > lRowStart And j = lColStart) Or (j > lColStart) Then
					If Trim(WkTBLS(i, j)) Like "*" & Trim(Text) & "*" Then
						Search = i
						fpSpd.ActiveSheet.SetActiveCell(i, j)
						'検索行を先頭に表示する
						fpSpd.SetViewportTopRow(0, i)
						LoopFLG = True
						Exit For
					End If
				End If
			Next
			If LoopFLG = True Then Exit For
		Next

	End Function
	'2014/11/13 ADD↑

	''' <summary>
	''' セルの値取得
	''' 標準関数:GetTextの拡張版
	''' 戻り値をセルのテキストにしたバージョン
	''' </summary>
	''' <param name="iCol"></param>
	''' <param name="iRow"></param>
	''' <param name="bRet"></param>
	''' <returns></returns>
	Public Function GetTextEX(ByVal iCol As Integer, ByVal iRow As Integer, Optional ByRef bRet As Boolean = False) As String

		'NOTE SS パラメータ順序の変更
		'NOTE SS If fpSpd.ActiveSheet.GetText(iCol, iRow, vText) Then
		'row   :セルの行インデックスを示す整数値。
		'column:セルの列インデックスを示す整数値。
		GetTextEX = fpSpd.ActiveSheet.GetText(iRow, iCol)
		bRet = True

	End Function

	''' <summary>
	''' 行背景色設定
	''' ----------------------------------
	'''    UPDATE      2019/08/10  移設
	''' ----------------------------------
	''' </summary>
	''' <param name="Mode"></param>
	''' <param name="iRow"></param>
	Public Sub RowBackColorSet2(ByRef Mode As String, ByRef iRow As Integer)

		With fpSpd
			Select Case Mode
				Case ""
					'行の背景色を初期値にする
					.ActiveSheet.Rows(iRow).BackColor = System.Drawing.ColorTranslator.FromOle(&HFFFFFF)
				Case "U"
					'行の背景色を黄色にする
					.ActiveSheet.Rows(iRow).BackColor = System.Drawing.ColorTranslator.FromOle(&H80FFFF)
				Case "R"
					'行の背景色を赤色にする
					'                    .BackColor = RGB(255, 150, 150)
					'                    .BackColor = RGB(230, 184, 183)
					.ActiveSheet.Rows(iRow).BackColor = System.Drawing.ColorTranslator.FromOle(RGB(242, 220, 219))
				Case "H"
					'行の背景色を灰色にする
					.ActiveSheet.Rows(iRow).BackColor = System.Drawing.ColorTranslator.FromOle(RGB(217, 217, 217))
				Case "B"
					'行の背景色を青色にする
					'                    .BackColor = RGB(150, 150, 255)
					'                    .BackColor = RGB(184, 204, 228)
					.ActiveSheet.Rows(iRow).BackColor = System.Drawing.ColorTranslator.FromOle(RGB(220, 230, 241))
			End Select
		End With
	End Sub

	'2025/08/01 ADD↓
	''' <summary>
	''' 最後の行番号を取得するメソッド
	''' </summary>
	''' <param name="Mode">
	'''     Other  : Full Check
	'''     MT02F00: 員数入力
	''' </param>
	''' <returns>最終行:Integer</returns>
    Public Function GetLastNonEmptyRowEx(Optional ByVal Mode As String = "Other") As Integer

        Dim retValue = -1
        Dim WkTBLS(,) As Object

        Dim excludeCols() As Integer = {28, 44, 46, 48, 49, 50, 51}

        ReDim WkTBLS(FpSpd.ActiveSheet.NonEmptyRowCount, FpSpd.ActiveSheet.ColumnCount)
        WkTBLS = FpSpd.ActiveSheet.GetArray(0, 0, FpSpd.ActiveSheet.RowCount, FpSpd.ActiveSheet.ColumnCount)

        '未入力最終行サーチ
        For wMaxRow = UBound(WkTBLS, 1) To LBound(WkTBLS, 1) Step -1
            For wMaxCol = UBound(WkTBLS, 2) To LBound(WkTBLS, 2) Step -1
                ' 員数入力 の場合、数式カラムを除く
                If (Mode <> "MT02F00") Or (Mode = "MT02F00" AndAlso Not excludeCols.Contains(wMaxCol)) Then
					If Not String.IsNullOrEmpty(WkTBLS(wMaxRow, wMaxCol)) Then
						Debug.WriteLine($"GetLastNonEmptyRowEx: {Mode} ,MaxRow : {wMaxRow}")
						retValue = wMaxRow
						Return retValue
					End If
				End If
            Next
        Next

        Return retValue
	End Function

	''' <summary>
	''' 最後の列番号を取得するメソッド
	''' </summary>
	''' <returns>最終列:Integer</returns>
    Public Function GetLastNonEmptyColEx() As Integer

        Dim retValue = -1
        Dim WkTBLS(,) As Object

        ReDim WkTBLS(FpSpd.ActiveSheet.NonEmptyRowCount, FpSpd.ActiveSheet.ColumnCount)
        WkTBLS = FpSpd.ActiveSheet.GetArray(0, 0, FpSpd.ActiveSheet.RowCount, FpSpd.ActiveSheet.ColumnCount)

        '未入力最終列サーチ
        For wMaxCol = UBound(WkTBLS, 2) To LBound(WkTBLS, 2) Step -1
            For wMaxRow = UBound(WkTBLS, 1) To LBound(WkTBLS, 1) Step -1
                If Not String.IsNullOrEmpty(WkTBLS(wMaxRow, wMaxCol)) Then
                    retValue = wMaxCol
                    Return retValue
                End If
            Next
        Next

        Return retValue
	End Function
	'2025/08/01 ADD↑

	''' <summary>
	''' IDisposable.Dispose の実装
	''' </summary>
	Public Sub Dispose() Implements IDisposable.Dispose
		' 明示的にリソースを解放
		Dispose(disposing:=True)
		' ファイナライザを呼ばないように抑制
		GC.SuppressFinalize(Me)
	End Sub

	''' <summary>
	''' Dispose本体 (Overridable にして派生クラスで上書き可能にする)
	''' </summary>
	''' <param name="disposing"></param>
	Protected Overridable Sub Dispose(disposing As Boolean)
		If Not disposedValue Then
			If disposing Then
				' NOTE: マネージド状態を破棄します (マネージド オブジェクト)
			End If

			' アンマネージド リソース (アンマネージド オブジェクト) の解放
			fpSpd = Nothing
			' 破棄済みフラグ
			disposedValue = True
		End If
	End Sub

	''' <summary>
	''' デストラクタ (Finalize)
	''' </summary>
	Protected Overrides Sub Finalize()
		Dispose(False)
	End Sub

End Class
