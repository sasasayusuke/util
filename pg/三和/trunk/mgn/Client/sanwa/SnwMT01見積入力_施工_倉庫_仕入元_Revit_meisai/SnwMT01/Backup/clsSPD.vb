Option Strict Off
Option Explicit On
Friend Class clsSPD
	'              '2014/11/11 LineSwap2追加 セルブロックでの入替用
	'              '2014/11/13 Search追加　全セル内検索用
	'              '2017/03/30 colSwap 列の入替（LineSwap2パクリ版）
	
	
	'ﾌﾟﾛﾊﾟﾃｨ値を保持するためのﾛｰｶﾙ変数。
	'''Private fpSpd As Control 'ﾛｰｶﾙ ｺﾋﾟｰ
	Private fpSpd As AxFPSpreadADO.AxfpSpread 'ﾛｰｶﾙ ｺﾋﾟｰ
	Private HCol As Integer
	Private HRow As Integer
	
	'UPGRADE_NOTE: Class_Terminate は Class_Terminate_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
	Private Sub Class_Terminate_Renamed()
		'UPGRADE_NOTE: オブジェクト fpSpd をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		fpSpd = Nothing
	End Sub
	Protected Overrides Sub Finalize()
		Class_Terminate_Renamed()
		MyBase.Finalize()
	End Sub
	
	
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
	
	Public Function LineInsert(Optional ByRef iRow As Integer = 0) As Short
		'行の挿入
		With fpSpd
			'初期値設定
			If iRow = 0 Then
				iRow = .ActiveRow
			End If
			
			'''        If SpreadErr = False Then
			If .DataRowCnt <> .MaxRows Then
				.ReDraw = False
				.col = 1
				.Row = iRow
				.Action = FPSpreadADO.ActionConstants.ActionInsertRow
				.ReDraw = True
				'''            HoldCD = vbNullString
			Else
				'''            Inform "これ以上、挿入できません。"
				'''            MsgBox "これ以上、挿入できません。", vbInformation, .Parent.Name
				LineInsert = -1
				'''            DoEvents
			End If
			'''        End If
			.Focus()
		End With
	End Function
	
	Public Sub LineDelete(Optional ByRef iRow As Integer = 0)
		'行の削除
		With fpSpd
			'初期値設定
			If iRow = 0 Then
				iRow = .ActiveRow
			End If
			
			'''        If SpreadErr = False Then
			.ReDraw = False
			.col = 1
			.Row = iRow
			'''            .SetText 1, .Row, vbNullString
			.Action = FPSpreadADO.ActionConstants.ActionDeleteRow
			.ReDraw = True
			'''        End If
			.Focus()
		End With
	End Sub
	
	Public Sub LineSwap(Optional ByRef Dest As Short = 1)
		'行の入れ替え
		With fpSpd
			'        Debug.Print "1:" & fpSpd.DataRowCnt
			If (.ActiveRow + Dest >= 1) And (.ActiveRow + Dest <= .MaxRows) Then
				.ReDraw = False
				.col = 1
				.Col2 = .MaxCols
				'DataRowCntの同期
				If Dest < 0 Then
					.Row = .ActiveRow + Dest
					.Row2 = .Row
					.DestRow = .ActiveRow
				Else
					.Row = .ActiveRow
					.Row2 = .ActiveRow
					.DestRow = .ActiveRow + Dest
				End If
				.DestCol = 1
				.Action = FPSpreadADO.ActionConstants.ActionSwapRange
				'            Debug.Print "2:" & fpSpd.DataRowCnt
				'DataRowCntの同期
				''''            .ClipValue = .ClipValue
				'''            .Col = 1
				'''            .Row = .MaxRows
				.MaxRows = .MaxRows + 1
				.col = 1
				.Row = .MaxRows
				.Value = CStr(1)
				.MaxRows = .MaxRows - 1
				
				'''            .Value = 1
				''''''''''''''            .Value = .Value
				'UPGRADE_NOTE: Refresh は CtlRefresh にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
				.CtlRefresh()
				'            Debug.Print "3:" & fpSpd.DataRowCnt
				'カーソルを移動位置に移動
				.col = .ActiveCol
				.Row = .ActiveRow + Dest
				.Action = FPSpreadADO.ActionConstants.ActionActiveCell
				'''            .Col2 = 1
				'''            .Row2 = .ActiveRow
				'''            .ClipValue = .ClipValue
				.Value = .Value
				'UPGRADE_NOTE: Refresh は CtlRefresh にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
				.CtlRefresh()
				.ReDraw = True
				'''            Debug.Print "SPDLineSwap:ok"
			End If
			'        Debug.Print "4:" & fpSpd.DataRowCnt
			.Focus()
		End With
	End Sub
	
	'2014/11/11 ADD↓
	Public Sub LineSwap2(Optional ByRef MOVEROW As Integer = 1)
		Dim ROWS As Integer
		
		ROWS = fpSpd.SelBlockRow2 - fpSpd.SelBlockRow + 1
		
		'行の入れ替え
		With fpSpd
			'        Debug.Print "1:" & fpSpd.DataRowCnt
			If (.ActiveRow + MOVEROW >= 1) And (.ActiveRow + MOVEROW + (ROWS - 1) <= .MaxRows) Then
				.ReDraw = False
				.col = 1
				.Col2 = .MaxCols
				'DataRowCntの同期
				If MOVEROW < 0 Then
					.Row = .ActiveRow
					.Row2 = .Row + ROWS - 1
					.DestRow = .ActiveRow + MOVEROW
				Else
					.Row = .ActiveRow
					.Row2 = .Row + ROWS - 1
					.DestRow = .ActiveRow + MOVEROW
				End If
				.DestCol = 1
				.Action = FPSpreadADO.ActionConstants.ActionSwapRange
				'            Debug.Print "2:" & fpSpd.DataRowCnt
				'DataRowCntの同期
				''''            .ClipValue = .ClipValue
				'''            .Col = 1
				'''            .Row = .MaxRows
				.MaxRows = .MaxRows + 1
				.col = 1
				.Row = .MaxRows
				.Value = CStr(1)
				.MaxRows = .MaxRows - 1
				
				'''            .Value = 1
				''''''''''''''            .Value = .Value
				'UPGRADE_NOTE: Refresh は CtlRefresh にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
				.CtlRefresh()
				'            Debug.Print "3:" & fpSpd.DataRowCnt
				'カーソルを移動位置に移動
				.col = .ActiveCol
				.Row = .ActiveRow + MOVEROW
				.Row2 = .Row + ROWS - 1
				.BlockMode = True
				If .IsBlockSelected = True Then
					.Action = FPSpreadADO.ActionConstants.ActionSelectBlock
				Else
					.Action = FPSpreadADO.ActionConstants.ActionActiveCell
				End If
				.BlockMode = False
				'''            .Col2 = 1
				'''            .Row2 = .ActiveRow
				'''            .ClipValue = .ClipValue
				.Value = .Value
				'UPGRADE_NOTE: Refresh は CtlRefresh にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
				.CtlRefresh()
				.ReDraw = True
				'''            Debug.Print "SPDLineSwap:ok"
			End If
			'        Debug.Print "4:" & fpSpd.DataRowCnt
			.Focus()
		End With
	End Sub
	'2014/11/11 ADD↑
	
	'2017/03/30 ADD↓
	Public Sub colSwap(ByRef minCol As Integer, ByRef maxCol As Integer, Optional ByRef MOVECOL As Integer = 1)
		Dim COLS As Integer
		
		COLS = fpSpd.SelBlockCol2 - fpSpd.SelBlockCol + 1
		
		'列の入れ替え
		With fpSpd
			'        Debug.Print "1:" & fpSpd.DataRowCnt
			If (.ActiveCol + MOVECOL >= minCol) And (.ActiveCol + MOVECOL + (COLS - 1) <= maxCol) Then
				.ReDraw = False
				.Row = 1
				.Row2 = .MaxRows
				'DataRowCntの同期
				If MOVECOL < 0 Then
					.col = .ActiveCol
					.Col2 = .col + COLS - 1
					.DestCol = .ActiveCol + MOVECOL
				Else
					.col = .ActiveCol
					.Col2 = .col + COLS - 1
					.DestCol = .ActiveCol + MOVECOL
				End If
				.DestRow = 1
				.Action = FPSpreadADO.ActionConstants.ActionSwapRange
				'            Debug.Print "2:" & fpSpd.DataRowCnt
				'DataRowCntの同期
				''''            .ClipValue = .ClipValue
				'''            .Col = 1
				'''            .Row = .MaxRows
				.MaxCols = .MaxCols + 1
				.Row = 1
				.col = .MaxCols
				.Value = CStr(1)
				.MaxCols = .MaxCols - 1
				
				'''            .Value = 1
				''''''''''''''            .Value = .Value
				'UPGRADE_NOTE: Refresh は CtlRefresh にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
				.CtlRefresh()
				'            Debug.Print "3:" & fpSpd.DataRowCnt
				'カーソルを移動位置に移動
				.Row = .ActiveRow
				.col = .ActiveCol + MOVECOL
				.Col2 = .col + COLS - 1
				.BlockMode = True
				If .IsBlockSelected = True Then
					.Action = FPSpreadADO.ActionConstants.ActionSelectBlock
				Else
					.Action = FPSpreadADO.ActionConstants.ActionActiveCell
				End If
				.BlockMode = False
				'''            .Col2 = 1
				'''            .Row2 = .ActiveRow
				'''            .ClipValue = .ClipValue
				.Value = .Value
				'UPGRADE_NOTE: Refresh は CtlRefresh にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
				.CtlRefresh()
				.ReDraw = True
				'''            Debug.Print "SPDLineSwap:ok"
			End If
			'        Debug.Print "4:" & fpSpd.DataRowCnt
			.Focus()
		End With
	End Sub
	'2017/03/30 ADD↑
	
	Public Sub LineBColorChange(ByRef lColor As Integer, Optional ByRef iRow As Short = 0)
		'行の色を変える
		With fpSpd
			'初期値設定
			If iRow = 0 Then
				iRow = .ActiveRow 'ｱｸﾃｨﾌﾞ行を指定
			End If
			
			.col = 1
			.Row = iRow
			.Col2 = .MaxCols
			.Row2 = iRow
			.BlockMode = True
			''        .BackColor = &HFFFFC0
			.BackColor = System.Drawing.ColorTranslator.FromOle(lColor)
			.BlockMode = False
		End With
	End Sub
	
	Public Sub LineCopyHold()
		'複写する為の行情報の保持
		With fpSpd
			HCol = 1
			HRow = .ActiveRow
			.Focus()
		End With
	End Sub
	
	Public Sub LinePaste()
		'コピーした行の複写
		With fpSpd
			.ReDraw = False
			.col = HCol
			.Row = HRow
			.Col2 = .MaxCols
			.Row2 = HRow
			.DestCol = 1
			.DestRow = .ActiveRow
			.Action = FPSpreadADO.ActionConstants.ActionCopyRange
			'DataRowCntの同期
			.Value = .Value
			.Focus()
			.ReDraw = True
		End With
	End Sub
	'''
	'''Public Sub SetAryItems(Ary() As Variant)
	'''    With fpSpd
	'''        .SetArray 1, 1, Ary()
	'''        .Col = 1
	'''        .Row = 1
	'''        .Action = ActionActiveCell
	'''    End With
	'''End Sub
	'''''
	'''''Public Function sprVAV2VA(InAr As Variant, Optional FirstDimensionIsCol As Boolean = True) As Variant()
	''''''バリアント変数 InAr が保持している配列を バリアント配列で返します
	''''''
	''''''引数 FirstDimensionIsCol は、バリアント変数 InAr が保持している配列を(Col,Row)あるいは(Row,Col)の
	''''''どちらで返すかを指定します
	'''''
	'''''    Dim InAr() As Variant
	'''''    Dim OutAr() As Variant
	'''''    Dim Row As Long, Col As Long
	'''''    Dim Dimension As Integer
	'''''
	'''''    If (VarType(InAr) And vbArray) Then
	'''''        Err.Clear
	'''''        On Error Resume Next
	'''''        Row = UBound(InAr, 2)
	'''''        If Err Then
	'''''            Dimension = 1
	'''''        Else
	'''''            Dimension = 2
	'''''        End If
	'''''        On Error GoTo 0
	'''''        If Dimension = 1 Then
	'''''            If FirstDimensionIsCol = True Then
	'''''                ReDim OutAr(0 To 0, LBound(InAr) To UBound(InAr))
	'''''                For Col = LBound(InAr) To UBound(InAr)
	'''''                    OutAr(0, Col) = InAr(Col)
	'''''                Next
	'''''            Else
	'''''                ReDim OutAr(LBound(InAr) To UBound(InAr), 0 To 0)
	'''''                For Col = LBound(InAr) To UBound(InAr)
	'''''                    OutAr(Col, 0) = InAr(Col)
	'''''                Next
	'''''            End If
	'''''        Else
	'''''            If FirstDimensionIsCol = True Then
	'''''                ReDim OutAr(LBound(InAr, 2) To UBound(InAr, 2), LBound(InAr, 1) To UBound(InAr, 1))
	'''''                For Row = LBound(InAr, 2) To UBound(InAr, 2)
	'''''                    For Col = LBound(InAr, 1) To UBound(InAr, 1)
	'''''                        OutAr(Row, Col) = InAr(Col, Row)
	'''''                    Next
	'''''                Next
	'''''            Else
	'''''                ReDim OutAr(LBound(InAr, 1) To UBound(InAr, 1), LBound(InAr, 2) To UBound(InAr, 2))
	'''''                For Row = LBound(InAr, 2) To UBound(InAr, 2)
	'''''                    For Col = LBound(InAr, 1) To UBound(InAr, 1)
	'''''                        OutAr(Col, Row) = InAr(Col, Row)
	'''''                    Next
	'''''                Next
	'''''            End If
	'''''        End If
	'''''    End If
	'''''    sprVAV2VA = OutAr()
	'''''End Function
	
	Public Sub sprClearText()
		'スプレッドシートの値をクリアする（ブロック指定）
		With fpSpd
			.ReDraw = False
			.Row = 1 ' セルブロックを設定
			.col = 1
			.Row2 = .MaxRows
			.Col2 = .MaxCols
			.BlockMode = True ' セルブロックを有効に設定
			.Action = FPSpreadADO.ActionConstants.ActionClearText ' 値とセル型の設定を消去
			.BlockMode = False ' セルブロックを無効に設定
			.ReDraw = True
		End With
	End Sub
	
	Public Function SearchCol(ByVal lCol As Integer, ByVal lRowStart As Integer, ByVal lRowEnd As Integer, ByVal Text As String) As Integer
		
		Dim WkTBLS As Object
		Dim i As Integer
		
		SearchCol = -1
		
		'UPGRADE_WARNING: 配列 WkTBLS の下限が 1,1 から 0,0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"' をクリックしてください。
		'UPGRADE_ISSUE: As Variant が ReDim WkTBLS(1 To fpSpd.DataRowCnt, 1 To fpSpd.MaxCols) ステートメントから削除されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="19AFCB41-AA8E-4E6B-A441-A3E802E5FD64"' をクリックしてください。
		ReDim WkTBLS(fpSpd.DataRowCnt, fpSpd.MaxCols)
		
		fpSpd.GetArray(1, 1, WkTBLS)
		
		If lRowEnd = -1 Then
			'-1の場合最大行
			lRowEnd = fpSpd.DataRowCnt
		End If
		
		
		
		For i = lRowStart To lRowEnd
			'UPGRADE_WARNING: オブジェクト WkTBLS() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If Trim(WkTBLS(i, lCol)) = Trim(Text) Then
				SearchCol = i
				Exit For
			End If
		Next 
		
	End Function
	
	'2014/11/13 ADD↓
	Public Function Search(ByVal lRowStart As Integer, ByVal lColStart As Integer, ByVal Text As String) As Integer
		
		Dim WkTBLS As Object
		Dim i As Integer
		Dim j As Integer
		Dim X As Integer
		
		Search = -1
		
		'UPGRADE_WARNING: 配列 WkTBLS の下限が 1,1 から 0,0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"' をクリックしてください。
		'UPGRADE_ISSUE: As Variant が ReDim WkTBLS(1 To fpSpd.DataRowCnt, 1 To fpSpd.MaxCols) ステートメントから削除されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="19AFCB41-AA8E-4E6B-A441-A3E802E5FD64"' をクリックしてください。
		ReDim WkTBLS(fpSpd.DataRowCnt, fpSpd.MaxCols)
		
		fpSpd.GetArray(1, 1, WkTBLS)
		
		'    If lRowEnd = -1 Then
		'        '-1の場合最大行
		'        lRowEnd = fpSpd.DataRowCnt
		'    End If
		Dim LoopFLG As Boolean
		
		
		
		For j = 1 To fpSpd.MaxCols
			For i = 1 To fpSpd.DataRowCnt
				fpSpd.col = j
				fpSpd.Row = i
				'            Debug.Print i, j
				''            If fpSpd.Lock = False Then
				If (i > lRowStart And j = lColStart) Or (j > lColStart) Then
					'UPGRADE_WARNING: オブジェクト WkTBLS() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					If Trim(WkTBLS(i, j)) Like "*" & Trim(Text) & "*" Then
						Search = i
						fpSpd.col = j
						fpSpd.Row = i
						
						'''                        '指定セルがロックされていたら
						'''                        'その行のロックされていないセルを探す
						'''                        For x = j - 1 To 0 Step -1
						'''                            If fpSpd.Lock = True Then
						'''                                fpSpd.Col = x
						'''                            Else
						'''                                Exit For
						'''                            End If
						'''                        Next
						
						LoopFLG = True
						Exit For
					End If
				End If
				''            End If
			Next 
			If LoopFLG = True Then Exit For
		Next 
		
	End Function
	'2014/11/13 ADD↑
	
	Public Function GetTextEX(ByVal iCol As Integer, ByVal iRow As Integer, Optional ByRef bRet As Boolean = False) As String
		'スプレットシートの値をゲット
		'標準関数:GetTextの拡張版
		'戻り値をセルのテキストにしたバージョン
		Dim vText As Object
		
		If fpSpd.GetText(iCol, iRow, vText) Then
			'UPGRADE_WARNING: オブジェクト vText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			GetTextEX = vText
			bRet = True
		Else
			GetTextEX = vbNullString
		End If
		
	End Function
	
	Public Sub RowBackColorSet2(ByRef Mode As String, ByRef iRow As Integer)
		'----------------------------------
		'   UPDATE      2019/08/10  移設
		'----------------------------------
		With fpSpd
			Select Case Mode
				Case ""
					'行の背景色を初期値にする
					'                .BlockMode = True
					'                    .Col = 1
					'                    .Col2 = .MaxCols
					'                    .Row = wRow
					'                    .Row2 = wRow
					.col = -1
					.Row = iRow
					.BackColor = System.Drawing.ColorTranslator.FromOle(&HFFFFFF)
					'                .BlockMode = False
				Case "U"
					'行の背景色を黄色にする
					.BlockMode = True
					.col = 1
					.Col2 = .MaxCols
					.Row = iRow
					.Row2 = iRow
					.BackColor = System.Drawing.ColorTranslator.FromOle(&H80FFFF)
					.BlockMode = False
				Case "R"
					'行の背景色を赤色にする
					.BlockMode = True
					.col = 1
					.Col2 = .MaxCols
					.Row = iRow
					.Row2 = iRow
					'                    .BackColor = RGB(255, 150, 150)
					'                    .BackColor = RGB(230, 184, 183)
					.BackColor = System.Drawing.ColorTranslator.FromOle(RGB(242, 220, 219))
					.BlockMode = False
				Case "H"
					'行の背景色を灰色にする
					.BlockMode = True
					.col = 1
					.Col2 = .MaxCols
					.Row = iRow
					.Row2 = iRow
					.BackColor = System.Drawing.ColorTranslator.FromOle(RGB(217, 217, 217))
					.BlockMode = False
				Case "B"
					'行の背景色を青色にする
					.BlockMode = True
					.col = 1
					.Col2 = .MaxCols
					.Row = iRow
					.Row2 = iRow
					'                    .BackColor = RGB(150, 150, 255)
					'                    .BackColor = RGB(184, 204, 228)
					.BackColor = System.Drawing.ColorTranslator.FromOle(RGB(220, 230, 241))
					.BlockMode = False
			End Select
		End With
	End Sub
End Class