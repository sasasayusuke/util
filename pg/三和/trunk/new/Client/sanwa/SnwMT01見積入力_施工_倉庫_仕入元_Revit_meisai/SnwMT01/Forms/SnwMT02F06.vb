Option Strict Off
Option Explicit On

Imports FarPoint.Win.Spread

''' <summary>
''' --------------------------------------------------------------------
'''   ユーザー名           株式会社三和商研
'''   業務名               積算データ管理システム
'''   部門名               マスタ部門
'''   プログラム名         員数入力処理（顧客テンプレート選択）
'''   作成会社             テクノウェア株式会社
'''   作成日               2003/07/02
'''   作成者               kawamura
''' --------------------------------------------------------------------
''' </summary>
Friend Class SnwMT02F06
	Inherits System.Windows.Forms.Form

	Dim ReturnF As Boolean 'リターンキー時（確定時）True
	Dim SelectF As Boolean '選択画面制御用
	Dim Loaded As Boolean 'FormLoad成功
	Dim PreviousControl As System.Windows.Forms.Control 'コントロールの戻りを制御
	Dim ModeF As Integer '処理区分   1：登録 2：修正
	Dim SpreadErr As Boolean

	Dim MeWidth, MeHeight As Integer '起動時のフォームサイズHold用
	Dim MeHeightLimit As Integer
	Dim LvHeightLimit As Integer
	Dim WkHeight As Integer 'リストビューからファンクションキー間の高さHold用

	'ボタン２重起動防止フラグ(CbFunc_Clickで使用)
	Dim CLK2F As Boolean '２重実行制御用

	'ｽﾌﾟﾚｯﾄﾞのクラス
	'UPGRADE_NOTE: clsSPD は clsSPD_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
	Dim clsSPD As clsSPD

	'保存ﾁｪｯｸ用ワーク
	Private Structure WkTBL
		Dim WKCheck As Boolean
		Dim WKPCKUBN As String
		Dim WKSEIHNO As String
		Dim WKSIYONO As String
		Dim WKNAME As String
		Dim WKW As Integer
		Dim WKD As Integer
		Dim WKH As Integer
		Dim WKD1 As Integer
		Dim WKD2 As Integer
		Dim WKSIRECD As String
	End Structure

	Dim WkTBLS() As WkTBL

	Dim Uploaded As Boolean
	Dim pParentForm As SnwMT02F00
	Dim pTOKUCD As String
	Dim pTOKUNM As String

	'選択画面チェックデータの受け取り
	'Public SelData As Variant
	'Excel取込データの受け取り
	Public MaxRows As Integer
	Public TOKUCD As String
	Public TOKUNM As String
	Public TEMPNM As String
	Public TempData As Object

	'データHold用ワーク
	Dim HTOKUCD As Object 'HoldID
	Dim HTEMPNM As Object 'HoldID
	Dim InitDate As Date '初期登録日保存用
	'クラス
	Private cKokyakutmp As clsKokyakutmp

	'List配列CbFuncのButton
	Private LcbFunc As New List(Of Button)

	Private Sub CbFunc_Click(sender As Object, e As EventArgs)
		Dim btn As Button = CType(sender, Button)
		Dim Index As Integer = CType(btn.Tag, Integer) - 1
		'Dim Index As Integer = cbFunc.GetIndex(eventSender)
		If LcbFunc(Index).Text = vbNullString Then
			PreviousControl.Focus()
			Exit Sub
		End If
		'ボタンを押されたのが２回目ならば抜ける
		If CLK2F = True Then
			Exit Sub
		End If
		'ボタン２重起動防止フラグのセット
		CLK2F = True
		LcbFunc(Index).TabStop = True
		Select Case Index
			Case 0
			Case 1
			Case 2
			Case 3
			Case 4
			Case 5
			Case 6
			Case 7
			Case 8
			Case 9
				If MsgBoxResult.Yes = NoYes("現在の編集内容を破棄します。", Me.Text) Then
					System.Windows.Forms.Application.DoEvents()
					Tx_テンプレート名.Focus()
				Else
					PreviousControl.Focus()
				End If
			Case 10
				If Item_Check(LcbFunc(Index).TabIndex) = True Then
					'                If Upload_Chk Then
					'                    cbFunc(Index).Caption = "登録"
					'                    PreviousControl.SetFocus
					'                End If
					'            End If
					'            Else
					If MsgBoxResult.Yes = YesNo("指定のデータを員数シート画面に貼り付けます。", Me.Text) Then
						System.Windows.Forms.Application.DoEvents()
						'                    cbFunc(Index).Enabled = False

						''                    If Upload Then
						If Upload_Kok() Then
							''                        Call FormInitialize
							''                        [tx_テンプレート名].SetFocus

							Uploaded = True
							Me.Close()
							CLK2F = False
							Exit Sub

							'                    If ResultCallF = False Then         '2002/01/15
							'                        Call FormInitialize
							'                        [tx_得意先CD].SetFocus
							'                    Else
							'                        Loaded = False
							'                        cbClose = True
							'                        Exit Sub
							'                    End If
						Else
							PreviousControl.Focus()
						End If
					Else
						PreviousControl.Focus()
					End If
				End If
			Case 11
				Me.Close()
				CLK2F = False
				Exit Sub
		End Select
		LcbFunc(Index).TabStop = False
		'ボタン２重起動防止フラグの初期化
		CLK2F = False
	End Sub

	Private Sub SnwMT02F06_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
		Dim Cancel As Boolean = e.Cancel
		Dim UnloadMode As System.Windows.Forms.CloseReason = e.CloseReason
		If Loaded Then
			'UPGRADE_NOTE: オブジェクト pParentForm をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
			pParentForm = Nothing

			If Uploaded Then
				'            pParentForm.ClearMode (True)
				'            pParentForm.Visible = True
				'            pParentForm.tx_得意先CD.SetFocus
				Exit Sub
			End If
			If MsgBoxResult.Yes = NoYes("現在の処理を終了します。") Then
				'            pParentForm.ClearMode (True)
				'            pParentForm.Visible = True
				'            pParentForm.tx_得意先CD.SetFocus
			Else
				PreviousControl.Focus()
				Cancel = True
			End If
		End If
		'    If Loaded Then
		'        cbClose.TabStop = True
		'        If vbYes = NoYes("現在の処理を終了します。") Then
		'        Else
		'            PreviousControl.SetFocus
		'            cbClose.TabStop = False
		'            Cancel = True
		'        End If
		'    End If
		e.Cancel = Cancel
		Me.Dispose()
	End Sub

	'UPGRADE_WARNING: イベント SnwMT02F06.Resize は、フォームが初期化されたときに発生します。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"' をクリックしてください。
	Private Sub SnwMT02F06_Resize(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Me.Resize
		'Dim fpSpd As Object
		If Me.WindowState <> System.Windows.Forms.FormWindowState.Minimized Then
			'見出しライン制御
			'TODO SS ln_over(0).X2 = VB6Conv.TwipsToPixelsX(VB6Conv.PixelsToTwipsX(Me.Width))
			'TODO SS ln_over(1).X2 = VB6Conv.TwipsToPixelsX(VB6Conv.PixelsToTwipsX(Me.Width))
			'フォーム最小（幅）制御
			If VB6Conv.PixelsToTwipsX(Me.Width) < MeWidth Then
				Me.Width = VB6Conv.TwipsToPixelsX(MeWidth)
			End If
			'フォーム最小（高さ）制御
			If VB6Conv.PixelsToTwipsY(Me.Height) < MeHeight Then
				Me.Height = VB6Conv.TwipsToPixelsY(MeHeightLimit)
			End If
			'UPGRADE_WARNING: オブジェクト fpSpd.Height の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'FpSpd.Height = VB6Conv.PixelsToTwipsY(Me.Height) - MeHeight

			' フォームの幅と高さに基づいてSpreadのサイズを変更
			FpSpd.Width = Me.ClientSize.Width - 30
			FpSpd.Height = Me.ClientSize.Height - 140

		End If
	End Sub

	Private Sub SnwMT02F06_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Me.Load
		'Dim fpSpd As Object
		Loaded = False

		'クラス生成
		cKokyakutmp = New clsKokyakutmp

		'SPREAD設定
		clsSPD = New clsSPD
		clsSPD.CtlSpd = FpSpd

		'フォームを画面の中央に配置
		Me.Top = VB6Conv.TwipsToPixelsY((VB6Conv.PixelsToTwipsY(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height) - VB6Conv.PixelsToTwipsY(Me.Height)) \ 2)
		Me.Left = VB6Conv.TwipsToPixelsX((VB6Conv.PixelsToTwipsX(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width) - VB6Conv.PixelsToTwipsX(Me.Width)) \ 2)

		'空白セット
		Call FormInitialize()
		'得意先セット
		[rf_得意先CD].Text = pTOKUCD
		rf_得意先名.Text = pTOKUNM

		'リサイズ用初期値設定（幅・高さ）
		MeWidth = VB6Conv.PixelsToTwipsX(Me.Width)
		'UPGRADE_WARNING: オブジェクト fpSpd.Height の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト fpSpd.Top の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		WkHeight = FpSpd.Top + FpSpd.Height
		WkHeight = VB6Conv.PixelsToTwipsY(PicFunction.Top) - WkHeight
		'UPGRADE_WARNING: オブジェクト fpSpd.Height の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		MeHeight = VB6Conv.PixelsToTwipsY(Me.Height) - FpSpd.Height
		'    LvHeightLimit = fpSpd.Font.Size * (34 + 24)
		'UPGRADE_WARNING: オブジェクト fpSpd.Font の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		LvHeightLimit = SnwMT02F00.FpSpd.Font.Size * ((34 + 24) * 2) '一行分?
		MeHeightLimit = MeHeight + LvHeightLimit

		' フォームの幅と高さに基づいてSpreadのサイズを変更
		FpSpd.Width = Me.ClientSize.Width - 30
		FpSpd.Height = Me.ClientSize.Height - 140

		sb_Msg_Panel2.Text = DateTime.Now.ToString("yyyy/MM/dd")
		sb_Msg_Panel3.Text = DateTime.Now.ToString("HH:mm")
		' タイマーの間隔を設定 (10秒ごとに更新)
		Timer1.Interval = 10000
		Timer1.Start() ' タイマーを開始

		' フォームでキー入力を受け取れるようにする
		Me.KeyPreview = True

		'ロード時にラベル・ボタンを動的に配置する
		AddPicFunction()

		Loaded = True
		Uploaded = False
	End Sub

	Private Sub AddPicFunction()

		' ラベル・ボタンを配置するPanelを指定
		Dim targetPanel As Panel = PicFunction

		' 生成数を指定
		Dim genLabelCount As Integer = 12
		Dim genLabelWidth As Integer = 65
		Dim genLabelHeight As Integer = 12
		Dim genLabelY As Integer = 1
		Dim genButtonCount As Integer = 12
		Dim genButtonWidth As Integer = 65
		Dim genButtonHeight As Integer = 18
		Dim genButtonY As Integer = 12

		Dim margin As Integer = 7               ' PFキー4つ置きのボタン間の余白

		Dim i As Integer

		For i = 0 To genLabelCount - 1

			' 新しいラベルを作成
			Dim lbl As New Label()

			' ラベルのプロパティを設定
			lbl.Tag = i + 1                     ' IndexをTagプロパティに格納
			lbl.Name = "LbFunc" & i + 1

			' ラベルをPanelに追加
			targetPanel.Controls.Add(lbl)

			lbl.Size = New Size(genLabelWidth, genLabelHeight)
			lbl.Location = New Point((i * genLabelWidth) + 1 + ((i \ 4) * margin), genLabelY) ' ラベルの位置を計算

			lbl.Text = "F" & i + 1
		Next

		For i = 0 To genButtonCount - 1

			' 新しいボタンを作成
			Dim btn As New Button()

			' ボタンのプロパティを設定
			btn.Tag = i + 1                     ' IndexをTagプロパティに格納
			btn.TabIndex = i + 1 + 2            ' TabIndexをプロパティに格納
			btn.Name = "CbFunc" & i + 1
			btn.Font = New Font("MS Gothic", 7.8, FontStyle.Regular)
			btn.FlatStyle = FlatStyle.Standard

			btn.Size = New Size(genButtonWidth, genButtonHeight)
			btn.Location = New Point((i * genButtonWidth) + 1 + ((i \ 4) * margin), genButtonY) ' ボタンの位置を計算

			'Debug.WriteLine($"Location X:{(i * genButtonWidth) + 1 + ((i \ 4) * margin)}")

			Select Case i
				Case 10
					btn.Text = "確定"
				Case 11
					btn.Text = "終了"
				Case Else
					btn.Text = ""
			End Select

			' ボタンにクリックイベントを追加
			AddHandler btn.Click, AddressOf CbFunc_Click

			' ボタンをPanelに追加
			targetPanel.Controls.Add(btn)

			' リストにボタンを追加
			LcbFunc.Add(btn)

		Next

	End Sub

	Private Sub SnwMT02F06_KeyDown(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
		Dim KeyCode As Integer = eventArgs.KeyCode
		Dim Shift As Integer = eventArgs.KeyData \ &H10000
		Dim ctl As System.Windows.Forms.Button

		On Error GoTo Form_KeyDown_Err
		Select Case KeyCode
			Case System.Windows.Forms.Keys.Escape
				KeyCode = 0
				Exit Sub
			Case System.Windows.Forms.Keys.F1 To System.Windows.Forms.Keys.F12
				On Error Resume Next
				'ctl = CType(Me.Controls("cbFunc"), Object)(KeyCode - System.Windows.Forms.Keys.F1 + 1)
				'If Err.Number = 0 Then
				'	If ctl.Text <> vbNullString Then
				'		If ctl.Enabled = True Then
				'			ctl.Focus()
				'			If Err.Number = 0 Then
				'				Call SendReturnKey()
				'			End If
				'		End If
				'	End If
				'End If
				'UPGRADE_NOTE: オブジェクト ctl をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
				'ctl = Nothing
				Select Case KeyCode
					Case Keys.F1
						LcbFunc_Action(0)
					Case Keys.F2
						LcbFunc_Action(1)
					Case Keys.F3
						LcbFunc_Action(2)
					Case Keys.F4
						LcbFunc_Action(3)
					Case Keys.F5
						LcbFunc_Action(4)
					Case Keys.F6
						LcbFunc_Action(5)
					Case Keys.F7
						LcbFunc_Action(6)
					Case Keys.F8
						LcbFunc_Action(7)
					Case Keys.F9
						LcbFunc_Action(8)
					Case Keys.F10
						LcbFunc_Action(9)
					Case Keys.F11
						LcbFunc_Action(10)
					Case Keys.F12
						LcbFunc_Action(11)
				End Select
				KeyCode = 0
				On Error GoTo 0
				Exit Sub
		End Select

		Exit Sub
Form_KeyDown_Err:
		MsgBox(Err.Number & " " & Err.Description)
	End Sub

	' 指定したインデックスのボタン動作を実行
	Private Sub LcbFunc_Action(index As Integer)
		If index >= 0 AndAlso index < LcbFunc.Count Then
			' ボタンのクリックイベントを呼び出す
			LcbFunc(index).PerformClick()
		End If
	End Sub

	Private Sub SnwMT02F06_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
		Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
		'ビープ音消去用
		Select Case KeyAscii
			Case System.Windows.Forms.Keys.Return
				KeyAscii = 0
		End Select
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub

	Private Sub FormInitialize()
		Call SetupBlank()
		Call HldBlank()
		'    Call ModeIndicate(Null)
		'スプレッドの設定をする。
		Call FpSpd_Initialize()
		CmdChkOn.Enabled = False
		CmdChkOff.Enabled = False
	End Sub

	Private Sub SetupBlank()
		Try
			ClearControls(Me)
		Catch ex As Exception
		End Try
	End Sub

	Private Sub ClearControls(ByVal parent As Control)
		For Each ctl As Control In parent.Controls
			If TypeOf ctl Is TextBox Then
				DirectCast(ctl, TextBox).Text = ""
			End If
			'UPGRADE_WARNING: TypeOf に新しい動作が指定されています。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
			If TypeOf ctl Is System.Windows.Forms.Label Then
				If ctl.Name Like "rf_*" Then
					ctl.Text = vbNullString
				End If
			End If
			'UPGRADE_WARNING: TypeOf に新しい動作が指定されています。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
			If TypeOf ctl Is ExText.ExTextBox Then
				ctl.Text = vbNullString
			End If
			'UPGRADE_WARNING: TypeOf に新しい動作が指定されています。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
			If TypeOf ctl Is ExNmText.ExNmTextBox Then
				ctl.Text = vbNullString
			End If

			' 子コントロールがある場合、再帰的に処理
			If ctl.HasChildren Then
				ClearControls(ctl)
			End If
		Next
	End Sub

	Private Sub HldBlank()
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HTOKUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HTOKUCD = System.DBNull.Value
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HTEMPNM の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HTEMPNM = System.DBNull.Value
	End Sub

	Private Sub ModeIndicate(ByVal Mode As Integer)
		'    If IsNull(Mode) Then
		'        [rf_処理区分].Caption = vbNullString
		'        [rf_処理区分].BackColor = &HE0E0E0
		'        ModeF = 0
		'    ElseIf Mode = 0 Then
		'        [rf_処理区分].Caption = "≪登 録≫"
		'        [rf_処理区分].BackColor = &HC0FFFF
		'        ModeF = 1
		'    ElseIf Mode = 1 Then
		'        [rf_処理区分].Caption = "≪修 正≫"
		'        [rf_処理区分].BackColor = &HC0FFC0
		'        ModeF = 2
		'    Else
		'        [rf_処理区分].Caption = "≪修 正≫"
		'        [rf_処理区分].BackColor = &HC0FFC0
		'        ModeF = 2
		'    End If
	End Sub

	Private Sub FpSpd_Initialize()
		'Dim fpSpd As Object
		With FpSpd
			'        .MaxCols = 12
			'UPGRADE_WARNING: オブジェクト fpSpd.MaxRows の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.ActiveSheet.RowCount = 300 '2007/11/17 UPD
			'UPGRADE_WARNING: オブジェクト fpSpd.EditModePermanent の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.EditModePermanent = True '常時入力モードを維持するかどうかを設定します。
			.EditModeReplace = True
			'        .ColsFrozen = 4
			'セル
			'UPGRADE_WARNING: オブジェクト fpSpd.UnitType の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'NOTE SS .UnitType 該当なし
			'NOTE SS .UnitType = FPSpreadADO.UnitTypeConstants.UnitTypeTwips
			'UPGRADE_WARNING: オブジェクト fpSpd.RowHeight の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'.RowHeight(-1) = 250
			'TODO SS .ActiveSheet.SetRowHeight(0, 250) 'twip → ピクセル単位に変換 250/14.4→17.36
			'シートのクリア
			Call clsSPD.SprClearText()
			'セルの位置付け
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'.Col = 1
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'.Row = 1
			'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'.Action = FPSpreadADO.ActionConstants.ActionActiveCell
			.ActiveSheet.SetActiveCell(0, 0)
		End With
		'FpSpd.Select()
	End Sub

	'UPGRADE_NOTE: Name は Name_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
	Private Sub SetUpFuncs(ByRef Name_Renamed As String)
		'ボタン名の変更
		Select Case Name_Renamed
			Case "Tx_テンプレート名"
				LcbFunc(0).Text = ""
				LcbFunc(1).Text = ""
				LcbFunc(2).Text = ""
				LcbFunc(3).Text = ""
				LcbFunc(4).Text = ""
				LcbFunc(5).Text = ""
				LcbFunc(6).Text = ""
				LcbFunc(7).Text = ""
				LcbFunc(8).Text = ""
				LcbFunc(9).Text = ""
				LcbFunc(10).Text = ""
			Case Else
				LcbFunc(0).Text = ""
				LcbFunc(1).Text = ""
				LcbFunc(2).Text = ""
				LcbFunc(3).Text = ""
				LcbFunc(4).Text = ""
				LcbFunc(5).Text = ""
				LcbFunc(6).Text = ""
				LcbFunc(7).Text = ""
				LcbFunc(8).Text = ""
				LcbFunc(9).Text = "中止"
				LcbFunc(10).Text = "確定"
		End Select
	End Sub

	Private Sub CbTabEnd_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs)
		If Item_Check((cbTabEnd.TabIndex)) Then
			LcbFunc(10).Focus()
			LcbFunc(10).PerformClick()
		End If
	End Sub

	'UPGRADE_ISSUE: PictureBox イベント picFunction.GotFocus はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="ABD9AF39-7E24-4AFF-AD8D-3675C1AA3054"' をクリックしてください。
	Private Sub PicFunction_Enter(sender As Object, e As EventArgs) Handles PicFunction.Enter
		PreviousControl.Focus()
	End Sub

	Private Sub CmdChkOn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdChkOn.Click
		'Dim fpSpd As Object
		Dim i, j As Integer

		'UPGRADE_WARNING: オブジェクト fpSpd.DataRowCnt の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'If FpSpd.ActiveSheet.NonEmptyRowCount = 0 Then
		If clsSPD.GetLastNonEmptyRowEx + 1 = 0 Then
			Inform("対象データがありません。")
			Exit Sub
		End If

		j = GetSerchMax()

		'    For i = 1 To fpSpd.DataRowCnt
		For i = 0 To j
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'fpSpd.Col = 1
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'fpSpd.Row = i
			'UPGRADE_WARNING: オブジェクト fpSpd.Text の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'FpSpd.Text = 1
			FpSpd.ActiveSheet.SetText(i, 0, 1)
		Next

		'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		FpSpd.Focus()
	End Sub

	Private Sub CmdChkOff_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdChkOff.Click
		'Dim fpSpd As Object
		Dim i As Integer

		'UPGRADE_WARNING: オブジェクト fpSpd.DataRowCnt の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'If FpSpd.ActiveSheet.NonEmptyRowCount = 0 Then
		If clsSPD.GetLastNonEmptyRowEx + 1 = 0 Then
			Exit Sub
		End If

		'UPGRADE_WARNING: オブジェクト fpSpd.DataRowCnt の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'For i = 0 To FpSpd.ActiveSheet.NonEmptyRowCount - 1
		For i = 0 To clsSPD.GetLastNonEmptyRowEx
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'FpSpd.Col = 1
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'FpSpd.Row = i
			'UPGRADE_WARNING: オブジェクト fpSpd.Text の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'FpSpd.Text = 0
			FpSpd.ActiveSheet.SetText(i, 0, 0)
		Next

		'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		FpSpd.Focus()
	End Sub

	Private Sub Tx_テンプレート名_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Tx_テンプレート名.Enter
		If SelectF = False Then
			'入力ﾁｪｯｸ
			If Item_Check((Tx_テンプレート名.TabIndex)) = False Then
				Exit Sub
			End If
			PreviousControl = Me.ActiveControl
			'        Call FormInitialize
		End If
		SelectF = False
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "テンプレート名を入力して下さい。　選択画面：Space"
	End Sub

	Private Sub Tx_テンプレート名_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles Tx_テンプレート名.KeyPress
		Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
		If KeyAscii = Asc(" ") Or KeyAscii = Asc("　") And (Tx_テンプレート名.SelStart = 0 And Tx_テンプレート名.SelLength = Len(Tx_テンプレート名.Text)) Then
			KeyAscii = 0
			'---参照画面表示
			SelectF = True
			cKokyakutmp.得意先CD = [rf_得意先CD].Text
			If cKokyakutmp.ShowDialog = True Then
				Tx_テンプレート名.Text = cKokyakutmp.テンプレート名
				ReturnF = True
				Tx_テンプレート名.NextSetFocus()
			Else
				Tx_テンプレート名.Focus()
			End If
		End If
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub

	'Private Sub Tx_テンプレート名_KeyDown(sender As Object, e As KeyEventArgs) Handles Tx_テンプレート名.KeyDown
	'	If Tx_テンプレート名.Text = "" Then
	'		'KeyCode = 0
	'		'---参照画面表示
	'		SelectF = True
	'		cKokyakutmp.得意先CD = [rf_得意先CD].Text
	'		If cKokyakutmp.ShowDialog = True Then
	'			Tx_テンプレート名.Text = cKokyakutmp.テンプレート名
	'			Tx_テンプレート名.NextSetFocus()
	'		Else
	'			Tx_テンプレート名.Focus()
	'		End If
	'	End If
	'
	'	ReturnF = True
	'End Sub

	Private Sub Tx_テンプレート名_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Tx_テンプレート名.Leave
		'    [sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_テンプレート名.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			Tx_テンプレート名.Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub FpSpd_GotFocus(sender As Object, e As EventArgs) Handles FpSpd.GotFocus
		'Dim fpSpd As Object
		'入力ﾁｪｯｸ
		'UPGRADE_WARNING: オブジェクト fpSpd.TabIndex の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If Item_Check(FpSpd.TabIndex) = False Then
			Exit Sub
		End If
		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
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
		'    Debug.Print "fpSpd_Advance = " & AdvanceNext
		'UPGRADE_WARNING: オブジェクト fpSpd.MaxRows の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If FpSpd.ActiveSheet.ActiveRowIndex = FpSpd.ActiveSheet.RowCount Then
			If e.AdvanceNext = True Then
				cbTabEnd.Focus()
			End If
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		ElseIf FpSpd.ActiveSheet.ActiveRowIndex = 1 Then
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If FpSpd.ActiveSheet.ActiveColumnIndex = 1 Then
				If e.AdvanceNext = False Then
					Tx_テンプレート名.Focus()
				End If
			End If
		End If
	End Sub

	Private Sub FpSpd_EditModeStarting(sender As Object, e As FarPoint.Win.Spread.EditModeStartingEventArgs) Handles FpSpd.EditModeStarting
		'Dim fpSpd As Object
		'    Debug.Print "fpSpd_EditMode = " & Col & ":" & Row & ":" & Mode & ":" & ChangeMade

		'If Mode = 0 Then Exit Sub 'フォーカスがないならば
		'If ChangeMade = True Then Exit Sub

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

		'    Debug.Print "EditMode col & Row & HoldCD = " & Col & ":" & Row & ":" & HoldCD
	End Sub

	Private Sub FpSpd_LeaveCell(sender As Object, e As LeaveCellEventArgs) Handles FpSpd.LeaveCell
		'Dim fpSpd As Object

		SpreadErr = False
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		If Me.ActiveControl.Name = "CbClose" Then Exit Sub

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

	End Sub

	Private Function Item_Check(ByRef ItemNo As Integer) As Boolean
		'Dim fpSpd As Object
		Dim bufName As String '名前取得用バッファ
		Dim buf2 As String
		Dim bufCur As Decimal
		Dim Chk_ID As String 'ﾁｪｯｸ用ワーク

		On Error GoTo Item_Check_Err
		Item_Check = False

		'キー項目「テンプレート名」のﾁｪｯｸ
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HTEMPNM の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'If ItemNo > tx_テンプレート名.TabIndex And ([tx_テンプレート名].Text <> HTEMPNM Or IsDBNull(HTEMPNM)) Then
		Dim StrHTEMPNM As String = If(IsDBNull(HTEMPNM), String.Empty, HTEMPNM.ToString())
		If ItemNo > Tx_テンプレート名.TabIndex And (Tx_テンプレート名.Text <> StrHTEMPNM) Then
			If IsCheckText(Tx_テンプレート名) = False Then
				CriticalAlarm("テンプレート名が未入力です。")
				'UPGRADE_WARNING: オブジェクト tx_テンプレート名.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Tx_テンプレート名.Undo()
				Tx_テンプレート名.Focus()
				Exit Function
			End If

			'--- データ表示
			'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト HTEMPNM の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'If ([tx_テンプレート名].Text <> HTEMPNM Or IsDBNull(HTEMPNM)) Then
			If (Tx_テンプレート名.Text <> StrHTEMPNM) Then
				If Download(1, [rf_得意先CD].Text, Tx_テンプレート名.Text) = -1 Then
					CriticalAlarm("指定の顧客テンプレートは未登録です。")
					'UPGRADE_WARNING: オブジェクト tx_テンプレート名.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					Tx_テンプレート名.Undo()
					Tx_テンプレート名.Focus()
					Exit Function
				End If

				CmdChkOn.Enabled = True
				CmdChkOff.Enabled = True

			End If

			'--- 入力値をワークへ格納
			'UPGRADE_WARNING: オブジェクト HTEMPNM の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			HTEMPNM = Tx_テンプレート名.Text
		End If

		'-----再読み込み用のクリア
		'UPGRADE_WARNING: オブジェクト fpSpd.TabIndex の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If ItemNo < FpSpd.TabIndex Then
			Tx_テンプレート名.Text = vbNullString
			'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト HTEMPNM の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			HTEMPNM = System.DBNull.Value
			Call clsSPD.SprClearText()
			CmdChkOn.Enabled = False
			CmdChkOff.Enabled = False
		End If

		Item_Check = True

		Exit Function
Item_Check_Err:
		MsgBox(Err.Number & " " & Err.Description)
	End Function

	Private Function Get得意先DB(ByVal ID As String, ByRef IDName As String) As Short
		Dim rs As ADODB.Recordset
		Dim sql As String

		On Error GoTo Get得意先DB_Err
		'マウスポインターを砂時計にする
		HourGlass(True)

		sql = "SELECT 得意先名1 FROM TM得意先 " & "WHERE 得意先CD = '" & SQLString(Trim(ID)) & "'"
		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)

		With rs
			If .EOF Then
				IDName = "未登録"
				Get得意先DB = -1
			Else
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				IDName = NullToZero(.Fields("得意先名1").Value, vbNullString)
				Get得意先DB = 0
			End If
		End With
		ReleaseRs(rs)

		HourGlass(False)
		Exit Function
Get得意先DB_Err:
		MsgBox(Err.Number & " " & Err.Description)
		HourGlass(False)
	End Function

	Private Function Download(ByRef Mode As Short, ByRef TOKUCD As String, ByRef TEMPNM As String) As Short
		'Dim fpSpd As Object
		Dim rs As ADODB.Recordset
		Dim sql As String

		On Error GoTo Download_Err
		'マウスポインターを砂時計にする
		HourGlass(True)

		Download = 0

		'---顧客テンプレートＭ存在ﾁｪｯｸ
		'    SQL = "SELECT KT.行NO, KT.PC区分, KT.製品NO, KT.仕様NO, " _
		'& "名称=(COALESCE(SE.漢字名称,HI.品群名称,UN.ユニット名,PC.漢字名称)), " _
		'& "SE.ベース色, " _
		'& "KT.W, KT.D, KT.H, KT.D1, KT.D2, " _
		'& "KT.H1, KT.H2, " _
		'& "単位=(COALESCE(SE.単位名,PC.単位名)), " _
		'& "マスタ=(CASE WHEN KT.製品区分 = 0 THEN '製品' WHEN KT.製品区分 = 1 THEN '品群' WHEN KT.製品区分 = 2 THEN 'ユニット' WHEN KT.製品区分 = 3 THEN 'ＰＣ' END), " _
		'& "KT.仕入先CD, " _
		'& "KT.初期登録日 " _
		'& "FROM TM顧客テンプレート AS KT " _
		'& "LEFT JOIN TM製品 AS SE " _
		'& "ON KT.製品区分 = 0 AND KT.製品NO = SE.製品NO AND KT.仕様NO = SE.仕様NO " _
		'& "LEFT JOIN TM品群 AS HI " _
		'& "ON KT.製品区分 = 1 AND KT.製品NO = HI.品群NO " _
		'& "LEFT JOIN TMユニット AS UN " _
		'& "ON KT.製品区分 = 2 AND KT.製品NO = UN.ユニットNO " _
		'& "LEFT JOIN TMPC AS PC " _
		'& "ON KT.製品区分 = 3 AND KT.PC区分 = PC.PC区分 AND KT.製品NO = PC.製品NO " _
		'& "WHERE 得意先CD = '" & SQLString(Trim$(TOKUCD)) & "'" _
		'& "AND テンプレート名 = '" & SQLString(Trim$(TEMPNM)) & "'" _
		'& "ORDER BY 得意先CD, テンプレート名, 行NO"

		sql = "SELECT KT.行NO, KT.PC区分, KT.製品NO, KT.仕様NO, " & "KT.漢字名称, KT.ベース色, " & "KT.W, KT.D, KT.H, KT.D1, KT.D2, KT.H1, KT.H2, " & "KT.単位名, KT.仕入先CD, " & "マスタ=(CASE WHEN KT.製品区分 = 0 THEN '製品' WHEN KT.製品区分 = 1 THEN '品群' WHEN KT.製品区分 = 2 THEN 'ユニット' WHEN KT.製品区分 = 3 THEN 'ＰＣ' END), " & "KT.製品区分 " & "FROM TM顧客テンプレート AS KT " & "WHERE 得意先CD = '" & SQLString(Trim(TOKUCD)) & "'" & "AND テンプレート名 = '" & SQLString(Trim(TEMPNM)) & "'" & "ORDER BY 得意先CD, テンプレート名, 行NO"

		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)

		With rs
			If .EOF Then
				'            InitDate = Date
				'            Call InitialItems
				Download = -1
			Else
				'            InitDate = ![初期登録日]
				Call SetupItems(rs)
				If Mode = 2 Then '選択画面より選択時
					'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					FpSpd.Focus()
				End If
				Download = 0
			End If
		End With
		Call ReleaseRs(rs)

		HourGlass(False)
		Exit Function
Download_Err:
		MsgBox(Err.Number & " " & Err.Description)
		HourGlass(False)
	End Function

	Private Sub SetupItems(ByRef rs As ADODB.Recordset)
		'Dim fpSpd As Object
		Dim RecArry(,) As Object
		Dim i, j As Integer

		'シートのクリア
		Call clsSPD.SprClearText()

		'UPGRADE_WARNING: Array に新しい動作が指定されています。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト fpSpd.MaxRows の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト rs.GetRows() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		RecArry = rs.GetRows(FpSpd.ActiveSheet.RowCount,  , New Object() {"PC区分", "製品NO", "仕様NO", "ベース色", "漢字名称", "W", "D", "H", "D1", "D2", "H1", "H2", "単位名", "仕入先CD", "マスタ"})

		For i = 0 To UBound(RecArry, 2)
			For j = 0 To UBound(RecArry)
				Select Case j + 1
					Case 6 To 12
						'UPGRADE_WARNING: オブジェクト RecArry() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						FpSpd.ActiveSheet.SetText(i, j + 1, If(Trim("" & RecArry(j, i)) = "0", "", Trim("" & RecArry(j, i))))
					Case Else
						'UPGRADE_WARNING: オブジェクト RecArry() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						FpSpd.ActiveSheet.SetText(i, j + 1, Trim("" & RecArry(j, i)))
				End Select
			Next
		Next

		'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'fpSpd.Col = 1
		'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'FpSpd.Row = 1
		'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'FpSpd.Action = FPSpreadADO.ActionConstants.ActionActiveCell
		FpSpd.ActiveSheet.SetActiveCell(0, 0)

		'HOLDセット
		'UPGRADE_WARNING: オブジェクト HTOKUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HTOKUCD = [rf_得意先CD].Text
		'UPGRADE_WARNING: オブジェクト HTEMPNM の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HTEMPNM = Tx_テンプレート名.Text
	End Sub

	Private Function GetSerchMax() As Integer
		'Dim fpSpd As Object
		Dim i As Integer
		'Dim j As Integer
		Dim getdata As Object
		Dim check As Boolean

		GetSerchMax = 0

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
			Exit Function
		End If

		'UPGRADE_WARNING: 配列 WkTBLS の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト fpSpd.DataRowCnt の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'ReDim WkTBLS(FpSpd.ActiveSheet.NonEmptyRowCount - 1)
		ReDim WkTBLS(clsSPD.GetLastNonEmptyRowEx)

		With FpSpd
			'ワークにホールド
			'UPGRADE_WARNING: オブジェクト fpSpd.DataRowCnt の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'For i = 0 To .ActiveSheet.NonEmptyRowCount - 1
			For i = 0 To clsSPD.GetLastNonEmptyRowEx
				'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				getdata = .ActiveSheet.GetText(i, 0) 'チェック
				If getdata.ToString().Trim() = "" Then
					check = False
				Else
					check = True
				End If
				If check Then
					'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					WkTBLS(i).WKCheck = getdata
				Else
					WkTBLS(i).WKCheck = False
				End If
				'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				getdata = .ActiveSheet.GetText(i, 1) 'PC区分
				If getdata.ToString().Trim() = "" Then
					check = False
				Else
					check = True
				End If
				If check Then
					'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					WkTBLS(i).WKPCKUBN = getdata
				Else
					WkTBLS(i).WKPCKUBN = ""
				End If
				'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				getdata = .ActiveSheet.GetText(i, 2) '製品NO
				If getdata.ToString().Trim() = "" Then
					check = False
				Else
					check = True
				End If
				If check Then
					'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					WkTBLS(i).WKSEIHNO = getdata
				Else
					WkTBLS(i).WKSEIHNO = ""
				End If
				'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				getdata = .ActiveSheet.GetText(i, 3) '仕様NO
				If getdata.ToString().Trim() = "" Then
					check = False
				Else
					check = True
				End If
				If check Then
					'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					WkTBLS(i).WKSIYONO = getdata
				Else
					WkTBLS(i).WKSIYONO = ""
				End If
				'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				getdata = .ActiveSheet.GetText(i, 5) '漢字名称
				If getdata.ToString().Trim() = "" Then
					check = False
				Else
					check = True
				End If
				If check Then
					'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					WkTBLS(i).WKNAME = getdata
				Else
					WkTBLS(i).WKNAME = ""
				End If
			Next

			'明細ﾁｪｯｸ
			For i = UBound(WkTBLS) To LBound(WkTBLS) Step -1
				If WkTBLS(i).WKPCKUBN <> vbNullString Then Exit For
				If WkTBLS(i).WKSEIHNO <> vbNullString Then Exit For
				If WkTBLS(i).WKSIYONO <> vbNullString Then Exit For
				If WkTBLS(i).WKNAME <> vbNullString Then Exit For
			Next

		End With

		GetSerchMax = i

	End Function

	Private Function Upload_Kok() As Boolean
		'Dim fpSpd As Object
		Dim getdata As Object
		Dim check As Boolean
		'Dim CheckData As Object
		Dim ResData(,) As Object
		Dim i, j As Integer
		Dim wRow As Integer

		Upload_Kok = False

		wRow = GetSerchMax()
		j = 0
		'    For i = 1 To fpSpd.DataRowCnt
		For i = 0 To wRow
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'fpSpd.Col = 1
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'fpSpd.Row = i
			'UPGRADE_WARNING: オブジェクト fpSpd.Text の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If FpSpd.ActiveSheet.GetText(i, 0) <> "" Then
				'UPGRADE_WARNING: オブジェクト fpSpd.Text の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'If FpSpd.ActiveSheet.GetText(i, 0) = 1 Or Trim(FpSpd.ActiveSheet.GetText(i, 0)) = "1" Then
				If FpSpd.ActiveSheet.GetText(i, 0) = "True" Or FpSpd.ActiveSheet.GetText(i, 0) = "1" Then
					j += 1
				End If
			End If
		Next

		If j = 0 Then
			Inform("対象データがありません。")
			Exit Function
		End If

		'UPGRADE_WARNING: オブジェクト fpSpd.MaxCols の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		ReDim ResData(j - 1, FpSpd.ActiveSheet.ColumnCount - 1)
		j = 0

		With FpSpd
			'        For i = 1 To UBound(ResData, 1)
			'UPGRADE_WARNING: オブジェクト fpSpd.MaxRows の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'For i = 0 To .ActiveSheet.NonEmptyRowCount - 1
			For i = 0 To clsSPD.GetLastNonEmptyRowEx
				'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'.Col = 1
				'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'.Row = i
				'UPGRADE_WARNING: オブジェクト fpSpd.Text の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				If .ActiveSheet.GetText(i, 0) = "True" Or .ActiveSheet.GetText(i, 0) = "1" Then
					'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					getdata = .ActiveSheet.GetText(i, 1) 'PC区分
					If getdata.ToString().Trim() = "" Then
						check = False
					Else
						check = True
					End If
					If check Then
						'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト ResData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						ResData(j, 0) = getdata
					Else
						ResData(j, 0) = ""
					End If
					'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					getdata = .ActiveSheet.GetText(i, 2) '製品NO
					If getdata.ToString().Trim() = "" Then
						check = False
					Else
						check = True
					End If
					If check Then
						'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト ResData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						ResData(j, 1) = getdata
					Else
						ResData(j, 1) = ""
					End If
					'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					getdata = .ActiveSheet.GetText(i, 3) '仕様NO
					If getdata.ToString().Trim() = "" Then
						check = False
					Else
						check = True
					End If
					If check Then
						'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト ResData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						ResData(j, 2) = getdata
					Else
						ResData(j, 2) = Space(7)
					End If
					'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					getdata = .ActiveSheet.GetText(i, 4) 'ベース色
					If getdata.ToString().Trim() = "" Then
						check = False
					Else
						check = True
					End If
					If check Then
						'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト ResData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						ResData(j, 3) = getdata
					Else
						ResData(j, 3) = ""
					End If
					'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					getdata = .ActiveSheet.GetText(i, 5) '名称
					If getdata.ToString().Trim() = "" Then
						check = False
					Else
						check = True
					End If
					If check Then
						'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト ResData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						ResData(j, 4) = getdata
					Else
						ResData(j, 4) = ""
					End If
					'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					getdata = .ActiveSheet.GetText(i, 6) 'W
					If getdata.ToString().Trim() = "" Then
						check = False
					Else
						check = True
					End If
					If check Then
						'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト ResData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						ResData(j, 5) = getdata
					Else
						ResData(j, 5) = ""
					End If
					'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					getdata = .ActiveSheet.GetText(i, 7) 'D
					If getdata.ToString().Trim() = "" Then
						check = False
					Else
						check = True
					End If
					If check Then
						'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト ResData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						ResData(j, 6) = getdata
					Else
						ResData(j, 6) = ""
					End If
					'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					getdata = .ActiveSheet.GetText(i, 8) 'H
					If getdata.ToString().Trim() = "" Then
						check = False
					Else
						check = True
					End If
					If check Then
						'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト ResData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						ResData(j, 7) = getdata
					Else
						ResData(j, 7) = ""
					End If
					'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					getdata = .ActiveSheet.GetText(i, 9) 'D1
					If getdata.ToString().Trim() = "" Then
						check = False
					Else
						check = True
					End If
					If check Then
						'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト ResData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						ResData(j, 8) = getdata
					Else
						ResData(j, 8) = ""
					End If
					'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					getdata = .ActiveSheet.GetText(i, 10) 'D2
					If getdata.ToString().Trim() = "" Then
						check = False
					Else
						check = True
					End If
					If check Then
						'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト ResData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						ResData(j, 9) = getdata
					Else
						ResData(j, 9) = ""
					End If
					'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					getdata = .ActiveSheet.GetText(i, 11) 'H1
					If getdata.ToString().Trim() = "" Then
						check = False
					Else
						check = True
					End If
					If check Then
						'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト ResData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						ResData(j, 10) = getdata
					Else
						ResData(j, 10) = ""
					End If
					'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					getdata = .ActiveSheet.GetText(i, 12) 'H2
					If getdata.ToString().Trim() = "" Then
						check = False
					Else
						check = True
					End If
					If check Then
						'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト ResData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						ResData(j, 11) = getdata
					Else
						ResData(j, 11) = ""
					End If
					'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					getdata = .ActiveSheet.GetText(i, 13) '単位
					If getdata.ToString().Trim() = "" Then
						check = False
					Else
						check = True
					End If
					If check Then
						'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト ResData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						ResData(j, 12) = getdata
					Else
						ResData(j, 12) = ""
					End If
					'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					getdata = .ActiveSheet.GetText(i, 14) '仕入先CD
					If getdata.ToString().Trim() = "" Then
						check = False
					Else
						check = True
					End If
					If check Then
						'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト ResData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						ResData(j, 13) = getdata
					Else
						ResData(j, 13) = ""
					End If

					j += 1
				End If
			Next
		End With

		'呼び元フォームに選択データをセット（注：変数は必須  Public SelData As Variant ）
		'UPGRADE_ISSUE: Control SelData は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト ResData の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		pParentForm.SelData = ResData

		Upload_Kok = True
	End Function

	'選択したコードを送るコントロールをセット
	WriteOnly Property ResParentForm() As System.Windows.Forms.Form
		Set(ByVal Value As System.Windows.Forms.Form)
			pParentForm = Value
		End Set
	End Property

	'------------------------------------------
	'   選択画面セットアップ
	'       ctl         :選択コードを送るコントロールをセット
	'       TOKUID      :得意先ID
	'       TOKUNM      :得意先名
	'------------------------------------------
	Public Sub SelSetup(ByRef TOKUCD As String, ByRef TOKUNM As String)

		pTOKUCD = TOKUCD
		pTOKUNM = TOKUNM

	End Sub

End Class