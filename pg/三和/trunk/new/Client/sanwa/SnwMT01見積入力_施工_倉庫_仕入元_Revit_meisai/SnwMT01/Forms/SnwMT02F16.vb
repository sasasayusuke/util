Imports FarPoint.Win.Spread

''' <summary>
''' --------------------------------------------------------------------
'''   ユーザー名           株式会社三和商研
'''   業務名               積算データ管理システム
'''   部門名               見積部門
'''   プログラム名         員数入力処理（入出庫日指定）
'''   作成会社             テクノウェア株式会社
'''   作成日               2018/05/03
'''   作成者               kawamura
''' --------------------------------------------------------------------
''' 
'''    UPDATE
'''        2018/05/03  oosawa      新設
'''        2020/09/16  oosawa      入庫日・出庫日の切替
''' -------------------------------------------------------------------- 
''' </summary>
Friend Class SnwMT02F16
	Inherits System.Windows.Forms.Form

	'ｽﾌﾟﾚｯﾄﾞのクラス
	'UPGRADE_NOTE: clsSPD は clsSPD_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
	Dim clsSPD As clsSPD
	Dim clsSPD_P As clsSPD

	'保存ﾁｪｯｸ用ワーク
	Private Structure WkTBL
		Dim WKMeisyo As String
		Dim WKRyaku As String
		Dim WKNouki As String
		Dim WKJikan As String
	End Structure

	'変数
	Dim HoldCD As Object
	Dim HCol As Integer
	Dim HRow As Integer

	Dim MeWidth, MeHeight As Integer '起動時のフォームサイズHold用
	Dim MeHeightLimit As Integer
	Dim LvHeightLimit As Integer
	Dim WkHeight As Integer 'リストビューからファンクションキー間の高さHold用


	Dim WkTBLS() As WkTBL
	Dim tbls(,) As Object

	Dim pParentForm As SnwMT02F00
	Dim pMituNo As Integer
	Dim pSetCol As Integer

	'2017/03/10 ADD↑
	Private Const Colp仕入先CD As Integer = 0
	Private Const Colp仕入先名 As Integer = 1
	Private Const Colp送り先CD As Integer = 2
	Private Const Colp送り先名 As Integer = 3
	Private Const Colp元入出庫日 As Integer = 4
	Private Const Colp新入出庫日 As Integer = 5

	Dim PreviousControl As System.Windows.Forms.Control 'コントロールの戻りを制御

	Dim m_Col入出庫日 As Integer

	'入庫日・出庫日の切替
	Public WriteOnly Property Col入出庫日() As String
		Set(ByVal Value As String)
			'Dim fpSpd As Object
			'UPGRADE_WARNING: オブジェクト NullToZero(vData) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If NullToZero(Value) = "入庫日" Then
				m_Col入出庫日 = Col入庫日
				Me.Text = "入庫日設定"
				'セルを選択します。
				'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'fpSpd.Col = 5
				'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'fpSpd.Row = 0
				'列ヘッダのテキストを設定します。
				'UPGRADE_WARNING: オブジェクト fpSpd.Text の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'fpSpd.Text = "元入庫日"
				'FpSpd.ActiveSheet.SetText(0, 5, "元入庫日")
				FpSpd.ActiveSheet.Cells(0, 4).Column.Label = "元入庫日"
				'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'FpSpd.Col = 6
				'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'FpSpd.Row = 0
				'列ヘッダのテキストを設定します。
				'UPGRADE_WARNING: オブジェクト fpSpd.Text の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'FpSpd.Text = "入庫日"
				'FpSpd.ActiveSheet.SetText(0, 6, "入庫日")
				FpSpd.ActiveSheet.Cells(0, 5).Column.Label = "入庫日"

				'        fpspd.
			Else
				m_Col入出庫日 = Col出庫日
				Me.Text = "出庫日設定"
				'セルを選択します。
				'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'fpSpd.Col = 5
				'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'fpSpd.Row = 0
				'列ヘッダのテキストを設定します。
				'UPGRADE_WARNING: オブジェクト fpSpd.Text の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'fpSpd.Text = "元出庫日"
				'FpSpd.ActiveSheet.SetText(0, 5, "元出庫日")
				FpSpd.ActiveSheet.Cells(0, 4).Column.Label = "元出庫日"
				'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'FpSpd.Col = 6
				'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'fpSpd.Row = 0
				'列ヘッダのテキストを設定します。
				'UPGRADE_WARNING: オブジェクト fpSpd.Text の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'fpSpd.Text = "出庫日"
				'FpSpd.ActiveSheet.SetText(0, 6, "出庫日")
				FpSpd.ActiveSheet.Cells(0, 5).Column.Label = "出庫日"
			End If
		End Set
	End Property

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

	Private Sub SnwMT02F16_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
		' Graphics オブジェクトを取得
		Dim g As Graphics = e.Graphics

		' DPIを取得
		Dim dpiX As Single = g.DpiX ' 水平方向のDPI
		Dim dpiY As Single = g.DpiY ' 垂直方向のDPI

		' 1ポイントをピクセルに変換
		Dim lineWidth As Single = 1 * (dpiX / 72)

		Dim pen As New Pen(Color.Black, lineWidth)

		' 線を描く、見積番号
		'g.DrawLine(pen, 12, 30, 180, 30)

		' リソースを解放
		pen.Dispose()
	End Sub

	Private Sub SnwMT02F16_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Me.Load
		'Dim fpSpd As Object
		On Error GoTo SysErr_Form_Load

		'フォームを画面の中央に配置
		Me.Top = VB6Conv.TwipsToPixelsY((VB6Conv.PixelsToTwipsY(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height) - VB6Conv.PixelsToTwipsY(Me.Height)) \ 2)
		Me.Left = VB6Conv.TwipsToPixelsX((VB6Conv.PixelsToTwipsX(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width) - VB6Conv.PixelsToTwipsX(Me.Width)) \ 2)

		'SPREAD設定
		clsSPD = New clsSPD
		clsSPD.CtlSpd = FpSpd

		clsSPD_P = New clsSPD
		clsSPD_P.CtlSpd = CType(pParentForm.Controls("fpSpd"), FarPoint.Win.Spread.FpSpread)

		'スプレッドの設定をする。
		Call FpSpd_Initialize()

		'リサイズ用初期値設定（幅・高さ）
		'MeWidth = VB6Conv.PixelsToTwipsX(Me.Width)
		'UPGRADE_WARNING: オブジェクト fpSpd.Height の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト fpSpd.Top の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'WkHeight = FpSpd.Top + FpSpd.Height
		'WkHeight = VB6Conv.PixelsToTwipsY(sb_Msg.Top) - WkHeight
		'UPGRADE_WARNING: オブジェクト fpSpd.Height の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'MeHeight = VB6Conv.PixelsToTwipsY(Me.Height) - FpSpd.Height
		'UPGRADE_WARNING: オブジェクト fpSpd.Font の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'LvHeightLimit = FpSpd.Font.Size * (34 + 24)
		'MeHeightLimit = MeHeight + LvHeightLimit


		'    rf_見積番号 = Format$(pMituNo, "#")

		If GetMitsumoriM() = False Then
			'Call Unload(Me)
		End If

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

	Private Sub SnwMT02F16_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
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
			'fpSpd.Action = FPSpreadADO.ActionConstants.ActionActiveCell
			FpSpd.ActiveSheet.SetActiveCell(FpSpd.ActiveSheet.ActiveRowIndex, FpSpd.ActiveSheet.ActiveColumnIndex)
			'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			FpSpd.Focus()
			Exit Sub
		End If

		'1行目の日付を以降にもセット

		Dim buff As String
		With FpSpd
			buff = clsSPD.GetTextEX(Colp新入出庫日, 0) '一行目の値を取得

			'UPGRADE_WARNING: オブジェクト fpSpd.ReDraw の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'.ReDraw = False
			.SuspendLayout()
			'If FpSpd.ActiveSheet.NonEmptyRowCount > 1 Then
			If clsSPD.GetLastNonEmptyRowEx + 1 > 1 Then
				'UPGRADE_WARNING: オブジェクト fpSpd.DataRowCnt の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'For i = 0 To FpSpd.ActiveSheet.NonEmptyRowCount - 1
				For i = 0 To clsSPD.GetLastNonEmptyRowEx
					If clsSPD.GetTextEX(Colp新入出庫日, i + 1) = "" Then
						'空欄のみセット
						'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ActiveSheet.SetText(i + 1, Colp新入出庫日, buff)
					End If
				Next
			End If
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
		'    If Upload_Chk = False Then
		'        PreviousControl.SetFocus
		'        Exit Sub
		'    End If
		If MsgBoxResult.Yes = YesNo("保存します。", Me.Text) Then
			System.Windows.Forms.Application.DoEvents()

			If Upload() Then
				'            Call SetHeader
				Me.Close()
			Else
				PreviousControl.Focus()
			End If
		Else
			PreviousControl.Focus()
		End If
	End Sub

	Private Sub FpSpd_Initialize()
		'Dim fpSpd As Object
		With FpSpd
			'UPGRADE_WARNING: オブジェクト fpSpd.ReDraw の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'.ReDraw = False
			.SuspendLayout()
			'UPGRADE_WARNING: オブジェクト fpSpd.MaxRows の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.ActiveSheet.RowCount = 2000

			.ActiveSheet.FrozenRowCount = 1
			.ActiveSheet.FrozenColumnCount = 6

			'UPGRADE_WARNING: オブジェクト fpSpd.EditModePermanent の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.EditModePermanent = True '常時入力モードを維持するかどうかを設定します。
			.EditModeReplace = True
			'セル
			'UPGRADE_WARNING: オブジェクト fpSpd.UnitType の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'NOTE SS .UnitType = FPSpreadADO.UnitTypeConstants.UnitTypeTwips
			'列の幅を設定する。
			'UPGRADE_WARNING: オブジェクト fpSpd.RowHeight の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'.RowHeight(-1) = 250
			'TODO SS .ActiveSheet.SetRowHeight(0, 250) 'twip → ピクセル単位に変換 250/14.4→17.36
			'グリッド線の表示形式を設定します。
			'UPGRADE_WARNING: オブジェクト fpSpd.GridSolid の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'TODO SS .GridSolid グリッド線
			'NOTE SS .GridSolid = True ' 線種（点線）
			'セルの背景色をグリッド線の下に表示します。
			'UPGRADE_WARNING: オブジェクト fpSpd.BackColorStyle の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'NOTE SS .BackColorStyle = FPSpreadADO.BackColorStyleConstants.BackColorStyleUnderGrid

			'UPGRADE_WARNING: オブジェクト fpSpd.ColsFrozen の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'.ColsFrozen = Colp元入出庫日
			.ActiveSheet.FrozenColumnCount = Colp元入出庫日
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'.Col = 1
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'.Row = 1
			'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'.Action = FPSpreadADO.ActionConstants.ActionActiveCell
			.ActiveSheet.SetActiveCell(0, 0)

			.ResumeLayout(True)
		End With
		FpSpd.Select()
	End Sub

	'UPGRADE_WARNING: イベント SnwMT02F16.Resize は、フォームが初期化されたときに発生します。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"' をクリックしてください。
	Private Sub SnwMT02F16_Resize(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Me.Resize
		'Dim fpSpd As Object

		If Me.WindowState <> System.Windows.Forms.FormWindowState.Minimized Then

			'フォーム最小（幅）制御
			If VB6Conv.PixelsToTwipsX(Me.Width) < MeWidth Then
				'Me.Width = VB6Conv.TwipsToPixelsX(MeWidth)
			End If
			'フォーム最小（高さ）制御・リストビューの高さをフォームの高さに比例
			If VB6Conv.PixelsToTwipsY(Me.Height) < MeHeightLimit Then
				'Me.Height = VB6Conv.TwipsToPixelsY(MeHeightLimit)
			End If
			'リストビューの高さ・ボタン位置
			'        fpSpd.Width = Me.ScaleWidth - (fpSpd.Left * 2)  '2012/09/13 ADD
			'UPGRADE_WARNING: オブジェクト fpSpd.Height の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.Top の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'WkHeight = VB6Conv.PixelsToTwipsY(sb_Msg.Top) - (FpSpd.Top + FpSpd.Height)
			'UPGRADE_WARNING: オブジェクト fpSpd.Height の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.Top の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'FpSpd.Height = VB6Conv.PixelsToTwipsY(Me.ClientRectangle.Height) - FpSpd.Top - (WkHeight) - VB6Conv.PixelsToTwipsY(sb_Msg.Height)

			'検索項目移動
			'UPGRADE_WARNING: オブジェクト fpSpd.Height の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.Top の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'[cbNouki].Top = VB6Conv.TwipsToPixelsY(FpSpd.Top + FpSpd.Height + 165)
			'UPGRADE_WARNING: オブジェクト fpSpd.Height の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.Top の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'[cbUpload].Top = VB6Conv.TwipsToPixelsY(FpSpd.Top + FpSpd.Height + 165)
			'UPGRADE_WARNING: オブジェクト fpSpd.Height の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.Top の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'[cbClose].Top = VB6Conv.TwipsToPixelsY(FpSpd.Top + FpSpd.Height + 165)

		End If
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
			If .ActiveSheet.ActiveColumnIndex = .ActiveSheet.RowCount Then
				If e.AdvanceNext = True Then
					'UPGRADE_WARNING: オブジェクト fpSpd.MaxCols の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					If .ActiveSheet.ActiveRowIndex <> .ActiveSheet.ColumnCount Then
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'.Col = .ActiveSheet.ActiveColumnIndex + 1
						'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'.Row = 1
						'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'.Action = FPSpreadADO.ActionConstants.ActionActiveCell
						'UPGRADE_WARNING: オブジェクト fpSpd.TopRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ActiveSheet.SetActiveCell(0, .ActiveSheet.ActiveColumnIndex + 1)
						'.TopRow = 1
						.SetViewportTopRow(0, 1)
					End If
				End If
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			ElseIf FpSpd.ActiveSheet.ActiveRowIndex = 1 Then
				If e.AdvanceNext = False Then
					'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					If .ActiveSheet.ActiveColumnIndex <> 1 Then
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

		Dim Row As Integer = fpSpd.ActiveSheet.ActiveRowIndex
		Dim Col As Integer = fpSpd.ActiveSheet.ActiveColumnIndex

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
		Dim getdata As Object 'Cell取り出し用
		'Dim getryak As Object 'Cell取り出し用

		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		If Me.ActiveControl.Name = "CbClose" Then Exit Sub


		With FpSpd
			'入力された情報を取得
			'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'check = .ActiveSheet.GetText(Col, Row, getdata)
			getdata = .ActiveSheet.GetText(e.Row, e.Column)
			check = True

			Select Case e.Column
				Case 0
					'値が変わったら
					If check Then
						'                    If getdata <> HoldCD Then
						'                        '略称セット(名称の８バイト)
						'                        .SetText 2, Row, AnsiLeftB(getdata, 8)
						'                        '2014/04/22 ADD↓
						'                        If clsSPD.GetTextEX(3, Row) = "" Then
						'                            .SetText 3, Row, HD_納期S
						'                        End If
						'                        '2014/04/22 ADD↑
						'                    End If
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
					'                Call ImmOpenModeSet(Me.Hwnd, ZENHIRA)
				Case Else
			End Select

			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'.Col = Col
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'.Row = 0
			'UPGRADE_WARNING: オブジェクト fpSpd.Text の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'Buf = .Text
			Buf = .ActiveSheet.Cells(0, Col).Column.Label
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

	Private Sub InitialItems()
		'シートのクリア
		Call clsSPD.SprClearText()
	End Sub

	'  Private Sub SetSPDFromgSiwakeTBL()
	'  '    Dim RecArry() As Variant
	'      Dim i As Integer, j As Integer
	'  '    Dim a As String
	'  
	'      'シートのクリア
	'      Call clsSPD.SprClearText
	'  
	'  '    If Not IsEmpty(gSiwakeTBL) Then
	'  '        For i = 0 To UBound(gSiwakeTBL, 2)
	'  '            For j = 0 To UBound(gSiwakeTBL)
	'  '                Select Case j + 1
	'  '                    Case 3
	'  '                        fpSpd.Col = j + 1
	'  '                        fpSpd.Row = i + 1
	'  '                        fpSpd.SetText j + 1, i + 1, Format$(gSiwakeTBL(j, i), "yyyy/mm/dd")
	'  '                    Case Else
	'  '                        fpSpd.SetText j + 1, i + 1, Trim$("" & gSiwakeTBL(j, i))
	'  '                End Select
	'  '            Next
	'  '        Next
	'  '    End If
	'      fpSpd.Col = 1
	'      fpSpd.Row = 1
	'      fpSpd.Action = ActionActiveCell
	'  End Sub
	'  
	'  Private Function Upload_Chk() As Boolean
	'      Dim i As Integer
	'      Dim j As Integer
	'      Dim wMaxRow As Integer
	'      Dim getdata As Variant
	'      Dim check As Boolean
	'  
	'      If fpSpd.DataRowCnt = 0 Then
	'          CriticalAlarm "明細がありません。"
	'          fpSpd.Col = fpSpd.ActiveRow
	'          fpSpd.Row = fpSpd.ActiveRow
	'          fpSpd.Action = ActionActiveCell
	'          fpSpd.SetFocus
	'          Exit Function
	'      End If
	'  
	'  '    ReDim WkTBLS(1 To fpSpd.DataRowCnt)
	'  
	'      ReDim tbls(1 To fpSpd.DataRowCnt, 1 To 4) As Variant '(1 to 30,1 to 4)
	'      ReDim tbls(1 To fpSpd.DataRowCnt, 1 To fpSpd.MaxCols) As Variant '(1 to 30,1 to 4)'2016/03/07 ADD
	'  
	'      fpSpd.GetArray 1, 1, tbls
	'  
	'      '未入力最終明細サーチ
	'      For wMaxRow = UBound(tbls, 1) To LBound(tbls, 1) Step -1
	'          If IsCheckNull(tbls(wMaxRow, 1)) = False Then Exit For                '名称
	'          If IsCheckNull(tbls(wMaxRow, 2)) = False Then Exit For                '略称
	'          If IsCheckNull(tbls(wMaxRow, 3)) = False Then Exit For                '納期
	'          If IsCheckNull(tbls(wMaxRow, 4)) = False Then Exit For                '時間
	'          If IsCheckNull(tbls(wMaxRow, 5)) = False Then Exit For                '部門 2016/03/07 ADD
	'  
	'      Next
	'  
	'      With fpSpd
	'          fpSpd.GetArray 1, 1, tbls
	'          .ReDraw = False
	'  
	'  '        For i = 1 To UBound(tbls)
	'          For i = 1 To wMaxRow
	'  '            If tbls(i, 1) = vbNullString And _
	'  '                tbls(i, 2) = vbNullString And _
	'  '                tbls(i, 3) = vbNullString And _
	'  '                tbls(i, 4) = vbNullString Then
	'  '                Debug.Print "aa" & i
	'  '            Else
	'                  If tbls(i, 1) = vbNullString Then
	'                      CriticalAlarm i & "行目の名称を入力して下さい。"
	'                      '行の位置付け
	'                      .Col = 1
	'                      .Row = i
	'                      .Action = ActionActiveCell
	'                      .SetFocus
	'                      '行の色を変える
	'                      .Col = -1
	'                      .Row = i
	'                      .BackColor = &HC0C0FF
	'                      .SetFocus
	'                      Exit Function
	'                  End If
	'  
	'                  If tbls(i, 2) = vbNullString Then
	'                      CriticalAlarm i & "行目の略称を入力して下さい。"
	'                      '行の位置付け
	'                      .Col = 1
	'                      .Row = i
	'                      .Action = ActionActiveCell
	'                      .SetFocus
	'                      '行の色を変える
	'                      .Col = -1
	'                      .Row = i
	'                      .BackColor = &HC0C0FF
	'                      .SetFocus
	'                      Exit Function
	'                  Else
	'                  If WkTBLS(i).WKMeisyo = vbNullString Then
	'                      CriticalAlarm i & "行目の名称を入力して下さい。"
	'                      '行の位置付け
	'                      .Col = 1
	'                      .Row = i
	'                      .Action = ActionActiveCell
	'                      .SetFocus
	'                      '行の色を変える
	'                      .Col = -1
	'                      .Row = i
	'                      .BackColor = &HC0C0FF
	'                      .SetFocus
	'                      Exit Function
	'                  End If
	'                  End If
	'  
	'              '納期・時間は未入力可能にする。
	'              'ただし、受注区分が「確定」の時はチェックする。[SnwMT02F02]の[Upload_Chk]で行う。
	'  '                If IsDate(tbls(i, 3)) = False Then
	'  '                    CriticalAlarm i & "行目の納期が不正です。"
	'  '                    '行の位置付け
	'  '                    .Col = 1
	'  '                    .Row = i
	'  '                    .Action = ActionActiveCell
	'  '                    .SetFocus
	'  '                    '行の色を変える
	'  '                    .Col = -1
	'  '                    .Row = i
	'  '                    .BackColor = &HC0C0FF
	'  '                    .SetFocus
	'  '                    Exit Function
	'  '                End If
	'  '
	'  '                If IsDate(tbls(i, 4)) = False Then
	'  '                    CriticalAlarm i & "行目の時間が不正です。"
	'  '                    '行の位置付け
	'  '                    .Col = 1
	'  '                    .Row = i
	'  '                    .Action = ActionActiveCell
	'  '                    .SetFocus
	'  '                    '行の色を変える
	'  '                    .Col = -1
	'  '                    .Row = i
	'  '                    .BackColor = &HC0C0FF
	'  '                    .SetFocus
	'  '                    Exit Function
	'  '                End If
	'  '            End If
	'          Next
	'  
	'          .ReDraw = True
	'      End With
	'  
	'      Upload_Chk = True
	'  
	'  End Function

	Private Function Upload() As Boolean
		'Dim fpSpd As Object
		Dim rs As ADODB.Recordset
		Dim sql As String
		Dim i As Integer
		Dim j As Integer

		On Error GoTo Trans_err

		Upload = False
		'マウスポインターを砂時計にする
		HourGlass(True)

		Dim spfpd As clsSPD
		spfpd = New clsSPD
		clsSPD.CtlSpd = FpSpd

		'UPGRADE_WARNING: 配列 tbls の下限が 1,1 から 0,0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"' をクリックしてください。
		'UPGRADE_ISSUE: As Variant が ReDim tbls(1 To fpSpd.DataRowCnt, 1 To fpSpd.MaxCols) ステートメントから削除されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="19AFCB41-AA8E-4E6B-A441-A3E802E5FD64"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト fpSpd.MaxCols の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト fpSpd.DataRowCnt の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		ReDim tbls(FpSpd.ActiveSheet.NonEmptyRowCount, FpSpd.ActiveSheet.ColumnCount)

		'UPGRADE_WARNING: オブジェクト fpSpd.GetArray の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'tbls = FpSpd.ActiveSheet.GetArray(0, 0, FpSpd.ActiveSheet.RowCount, FpSpd.ActiveSheet.ColumnCount)
		tbls = FpSpd.ActiveSheet.GetArray(0, 0, FpSpd.ActiveSheet.NonEmptyRowCount, FpSpd.ActiveSheet.ColumnCount)

		'UPGRADE_WARNING: オブジェクト pParentForm.Controls().ReDraw の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'CType(pParentForm.Controls("fpSpd"), Object).ReDraw = True

		For i = 0 To UBound(tbls)
			'UPGRADE_WARNING: オブジェクト pParentForm.Controls(fpSpd).DataRowCnt の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'For j = 1 To CType(pParentForm.Controls("fpSpd"), Object).DataRowCnt
			For j = 0 To CType(pParentForm.Controls("fpSpd"), Object).ActiveSheet.NonEmptyRowCount - 1
				'UPGRADE_WARNING: オブジェクト tbls(i, Colp元入出庫日) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト tbls(i, Colp送り先CD) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト tbls(i, Colp仕入先CD) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				If clsSPD_P.GetTextEX(Col仕入先CD, j) = tbls(i, Colp仕入先CD) And clsSPD_P.GetTextEX(Col送り先CD, j) = tbls(i, Colp送り先CD) And clsSPD_P.GetTextEX(m_Col入出庫日, j) = If(tbls(i, Colp元入出庫日) Is Nothing, "", CType(tbls(i, Colp元入出庫日), Date).ToString("yy/MM/dd")) Then
					'UPGRADE_WARNING: オブジェクト SpcToNull(clsSPD_P.GetTextEX(Col発注数, j), 0) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					If SpcToNull(clsSPD_P.GetTextEX(Col発注数, j), 0) <> 0 Then '2018/06/02 ADD
						'UPGRADE_WARNING: オブジェクト pParentForm.Controls().SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'CType(pParentForm.Controls("fpSpd"), Object).SetText(j, m_Col入出庫日, tbls(i, Colp新入出庫日))
						CType(pParentForm.Controls("fpSpd"), Object).ActiveSheet.SetText(j, m_Col入出庫日, tbls(i, Colp新入出庫日))
						'            fpSpd.SetText Col入出庫日, j, tbls(4, i)
					End If
				End If

			Next
		Next

		Upload = True

Trans_Correct:
		On Error GoTo 0

		HourGlass(False)
		'UPGRADE_WARNING: オブジェクト pParentForm.Controls().ReDraw の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'CType(pParentForm.Controls("fpSpd"), Object).ReDraw = False

		'UPGRADE_WARNING: オブジェクト pParentForm.Controls().Item の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'CType(pParentForm.Controls("LcbFunc"), Object).Item(10).Text = "ﾁｪｯｸ"
		pParentForm.LcbFunc(10).Text = "ﾁｪｯｸ"

		Exit Function


Trans_err: '---エラー時
		MsgBox(Err.Number & " " & Err.Description)
		Resume Trans_Correct
	End Function

	Private Function GetMitsumoriM() As Boolean
		'Dim rs As ADODB.Recordset
		Dim Criteria As String
		Dim i As Integer
		Dim wMaxRow As Integer
		Dim grs As ADODB.Recordset
		GetMitsumoriM = False

		Dim WkTBLS(,) As Object
		'UPGRADE_ISSUE: Control fpSpd は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		With pParentForm.FpSpd
			'UPGRADE_ISSUE: Control fpSpd は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
			If .ActiveSheet.NonEmptyRowCount = 0 Then
				CriticalAlarm("明細がありません。")
				Exit Function
			End If

			'UPGRADE_WARNING: 配列 WkTBLS の下限が 1,1 から 0,0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"' をクリックしてください。
			'UPGRADE_ISSUE: Control fpSpd は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
			ReDim WkTBLS(.ActiveSheet.NonEmptyRowCount, .ActiveSheet.ColumnCount)
			'        Dim WkTBLSs(1 To .DataRowCnt, 1 To .MaxCols)
			'スプレッド上のデータを配列に
			'UPGRADE_ISSUE: Control fpSpd は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
			WkTBLS = .ActiveSheet.GetArray(0, 0, .ActiveSheet.NonEmptyRowCount, .ActiveSheet.ColumnCount)

			'未入力最終明細サーチ
			For wMaxRow = UBound(WkTBLS, 1) To LBound(WkTBLS, 1) Step -1
				If IsCheckNull(WkTBLS(wMaxRow, ColPC区分)) = False Then Exit For 'PC区分
				If IsCheckNull(WkTBLS(wMaxRow, Col製品NO)) = False Then Exit For '製品NO
				If IsCheckNull(WkTBLS(wMaxRow, Col仕様NO)) = False Then Exit For '仕様NO
				If IsCheckNull(WkTBLS(wMaxRow, Col名称)) = False Then Exit For '名称
				If IsCheckNull(WkTBLS(wMaxRow, Col発注数)) = False Then Exit For '発注数
				'        If IsCheckNull(WkTBLS(wMaxRow, 24)) = False Then Exit For               '仕入先CD
				'        If IsCheckNull(WkTBLS(wMaxRow, 25)) = False Then Exit For               '配送先CD
			Next

			'保存用テーブル作成
			grs = CreateTable()

			For i = 0 To wMaxRow
				'        Select Case SpcToNull(WkTBLS(i, Col見積区分))
				'            Case "A", "C", "S"
				'                '取り込まない
				'            Case Else
				'UPGRADE_WARNING: オブジェクト SpcToNull(WkTBLS(i, m_Col入出庫日)) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト SpcToNull(WkTBLS(i, Col送り先CD)) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト SpcToNull(WkTBLS(i, Col仕入先CD)) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				If Not (SpcToNull(WkTBLS(i, Col仕入先CD)) = "" And SpcToNull(WkTBLS(i, Col送り先CD)) = "" And SpcToNull(WkTBLS(i, m_Col入出庫日)) Is Nothing) Then
					'UPGRADE_WARNING: オブジェクト SpcToNull(WkTBLS(i, Col発注数), 0) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					If SpcToNull(WkTBLS(i, Col発注数), 0) <> 0 Then '2018/06/02 ADD

						'ループして存在のチェックを行なう
						'UPGRADE_WARNING: オブジェクト SpcToNull(WkTBLS(i, m_Col入出庫日)) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト SpcToNull(WkTBLS(i, Col送り先CD)) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						Criteria = "仕入先CD = '" & SpcToNull(WkTBLS(i, Col仕入先CD)) & "' AND 配送先CD = '" & SpcToNull(WkTBLS(i, Col送り先CD)) & "' AND 旧入出庫日 = '" & SpcToNull(WkTBLS(i, m_Col入出庫日)) & "'"
						'            If IsNull(SpcToNull(WkTBLS(i, Col入出庫日))) Then
						'                criteria = "旧入出庫日 is NULL"
						'            Else
						'                criteria = "旧入出庫日 = '" & SpcToNull(WkTBLS(i, Col入出庫日)) & "'"
						'            End If
						'            criteria = "Key = '" & SpcToNull(WkTBLS(i, Col仕入先CD)) & "-" & SpcToNull(WkTBLS(i, Col送り先CD)) & "-" & SpcToNull(WkTBLS(i, Col入出庫日))

						'            grs.MoveFirst
						'            grs.Find criteria
						grs.Filter = Criteria

						If grs.RecordCount = 0 Then
							grs.AddNew()

							'UPGRADE_WARNING: オブジェクト SpcToNull(WkTBLS(i, m_Col入出庫日)) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト SpcToNull(WkTBLS(i, Col送り先CD)) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							grs.Fields("Key").Value = SpcToNull(WkTBLS(i, Col仕入先CD)) & "-" & SpcToNull(WkTBLS(i, Col送り先CD)) & "-" & SpcToNull(WkTBLS(i, m_Col入出庫日))

							'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							grs.Fields("仕入先CD").Value = SpcToNull(WkTBLS(i, Col仕入先CD), "")
							'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							grs.Fields("仕入先略称").Value = SpcToNull(WkTBLS(i, Col仕入先名), "")
							'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							grs.Fields("配送先CD").Value = SpcToNull(WkTBLS(i, Col送り先CD), "")
							'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							grs.Fields("配送先略称").Value = SpcToNull(WkTBLS(i, Col送り先名), "")
							'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'grs.Fields("旧入出庫日").Value = SpcToNull(WkTBLS(i, m_Col入出庫日), "")
							grs.Fields("旧入出庫日").Value = If(SpcToNull(WkTBLS(i, m_Col入出庫日), "") = "", "", CType(WkTBLS(i, m_Col入出庫日), DateTime).ToString("yyyy/MM/dd"))
							'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'grs.Fields("新入出庫日").Value = SpcToNull(WkTBLS(i, m_Col入出庫日), "")
							grs.Fields("新入出庫日").Value = If(SpcToNull(WkTBLS(i, m_Col入出庫日), "") = "", "", CType(WkTBLS(i, m_Col入出庫日), DateTime).ToString("yyyy/MM/dd"))

							grs.Update()

						End If
					End If
				End If
				'            End Select
			Next
			grs.Filter = ""
		End With

		If grs.EOF Then
			CriticalAlarm("明細がありません。")
			Exit Function
		End If

		i = 0
		With FpSpd
			grs.MoveFirst()
			grs.Sort = "仕入先CD,配送先CD,旧入出庫日"

			'UPGRADE_WARNING: オブジェクト fpSpd.ReDraw の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'.ReDraw = False
			.SuspendLayout()

			Do Until grs.EOF

				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ActiveSheet.SetText(i, Colp仕入先CD, grs.Fields("仕入先CD").Value)
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ActiveSheet.SetText(i, Colp仕入先名, grs.Fields("仕入先略称").Value)
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ActiveSheet.SetText(i, Colp送り先CD, grs.Fields("配送先CD").Value)
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ActiveSheet.SetText(i, Colp送り先名, grs.Fields("配送先略称").Value)
				'.SetText Colp元入出庫日, i, grs![旧入出庫日]
				'.SetText Colp新入出庫日, i, grs![新入出庫日]
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				If (grs.Fields("旧入出庫日").Value).ToString = "" Then
					.ActiveSheet.SetText(i, Colp元入出庫日, "")
				Else
					.ActiveSheet.SetText(i, Colp元入出庫日, CType(grs.Fields("旧入出庫日").Value, DateTime).ToString("yy/MM/dd"))
				End If
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				If (grs.Fields("新入出庫日").Value).ToString = "" Then
					.ActiveSheet.SetText(i, Colp新入出庫日, "")
				Else
					.ActiveSheet.SetText(i, Colp新入出庫日, CType(grs.Fields("新入出庫日").Value, DateTime).ToString("yy/MM/dd"))
				End If

				grs.MoveNext()
				i += 1
			Loop


			'UPGRADE_WARNING: オブジェクト fpSpd.MaxRows の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.ActiveSheet.RowCount = i
			'UPGRADE_WARNING: オブジェクト fpSpd.ReDraw の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'.ReDraw = True
			.ResumeLayout(True)

		End With

		'    grs.Close
		'    Set grs = Nothing
		ReleaseRs(grs)

		'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'fpSpd.ActiveSheet.ActiveColumnIndex = Colp新入出庫日
		'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'fpSpd.ActiveSheet.ActiveRowIndex = 0
		'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'fpSpd.Action = FPSpreadADO.ActionConstants.ActionActiveCell
		FpSpd.ActiveSheet.SetActiveCell(0, Colp新入出庫日)
	End Function

	Private Function CreateTable() As ADODB.Recordset
		Dim rs As ADODB.Recordset

		On Error GoTo CreateTable_Err

		'If CreateTable IsNot Nothing Then
		'	CreateTable.Close()
		'	'UPGRADE_NOTE: オブジェクト CreateTable をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		'	CreateTable = Nothing
		'End If

		'レコードセットを作成する
		rs = New ADODB.Recordset
		rs.CursorLocation = ADODB.CursorLocationEnum.adUseClient
		With rs
			'.Fields.Append "伝票番号", adInteger
			'.Fields.Append "納品日", adDate

			.Fields.Append("key", ADODB.DataTypeEnum.adVarChar, 255)

			.Fields.Append("仕入先CD", ADODB.DataTypeEnum.adVarChar, 4)
			.Fields.Append("仕入先略称", ADODB.DataTypeEnum.adVarChar, 20)
			.Fields.Append("配送先CD", ADODB.DataTypeEnum.adVarChar, 4)
			.Fields.Append("配送先略称", ADODB.DataTypeEnum.adVarChar, 20)
			.Fields.Append("旧入出庫日", ADODB.DataTypeEnum.adVarChar, 10)
			.Fields.Append("新入出庫日", ADODB.DataTypeEnum.adVarChar, 10)

			.CursorType = ADODB.CursorTypeEnum.adOpenStatic
			.LockType = ADODB.LockTypeEnum.adLockOptimistic

			.Open()
		End With

		'If CreateTable IsNot Nothing Then
		'	CreateTable.Close()
		'	'UPGRADE_NOTE: オブジェクト CreateTable をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		'	CreateTable = Nothing
		'End If

		Return rs

CreateTable_Err:
		MsgBox(Err.Number & " " & Err.Description)
		Call HourGlass(False)
	End Function

End Class