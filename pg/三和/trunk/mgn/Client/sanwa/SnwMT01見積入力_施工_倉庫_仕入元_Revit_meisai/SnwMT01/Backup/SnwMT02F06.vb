Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.PowerPacks
Friend Class SnwMT02F06
	Inherits System.Windows.Forms.Form
	'
	'--------------------------------------------------------------------
	'  ユーザー名           株式会社三和商研
	'  業務名　　　　　　　　積算データ管理システム
	'  部門名               マスタ部門
	'  プログラム名         員数入力処理（顧客テンプレート選択）
	'  作成会社             テクノウェア株式会社
	'  作成日               2003/07/02
	'  作成者               kawamura
	'--------------------------------------------------------------------
	'
	Dim ReturnF As Boolean 'リターンキー時（確定時）True
	Dim SelectF As Boolean '選択画面制御用
	Dim Loaded As Boolean 'FormLoad成功
	Dim PreviousControl As System.Windows.Forms.Control 'コントロールの戻りを制御
	Dim ModeF As Short '処理区分   1：登録 2：修正
	Dim SpreadErr As Boolean
	
	Dim MeWidth, MeHeight As Integer '起動時のフォームサイズHold用
	Dim MeHeightLimit As Integer
	Dim LvHeightLimit As Integer
	Dim WkHeight As Integer 'リストビューからファンクションキー間の高さHold用
	
	'ボタン２重起動防止フラグ
	Dim CLK2F As Boolean '２重実行制御用
	
	'ｽﾌﾟﾚｯﾄﾞのクラス
	'UPGRADE_NOTE: clsSPD は clsSPD_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
	Dim clsSPD_Renamed As clsSPD
	
	'保存ﾁｪｯｸ用ワーク
	Private Structure WkTBL
		Dim WKCheck As Short
		Dim WKPCKUBN As String
		Dim WKSEIHNO As String
		Dim WKSIYONO As String
		Dim WKNAME As String
		Dim WKW As Short
		Dim WKD As Short
		Dim WKH As Short
		Dim WKD1 As Short
		Dim WKD2 As Short
		Dim WKSIRECD As String
	End Structure
	
	Dim WkTBLS() As WkTBL
	
	Dim Uploaded As Boolean
	Dim pParentForm As System.Windows.Forms.Form
	Dim pTOKUCD As String
	Dim pTOKUNM As String
	
	'''選択画面チェックデータの受け取り
	''Public SelData As Variant
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
	
	Private Sub cbFunc_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cbFunc.Click
		Dim Index As Short = cbFunc.GetIndex(eventSender)
		If cbFunc(Index).Text = vbNullString Then
			PreviousControl.Focus()
			Exit Sub
		End If
		'ボタンを押されたのが２回目ならば抜ける
		If CLK2F = True Then
			Exit Sub
		End If
		'ボタン２重起動防止フラグのセット
		CLK2F = True
		cbFunc(Index).TabStop = True
		Select Case Index
			Case 1
			Case 2
			Case 3
			Case 4
			Case 5
			Case 6
			Case 7
			Case 8
			Case 9
			Case 10
				If MsgBoxResult.Yes = NoYes("現在の編集内容を破棄します。", Me.Text) Then
					System.Windows.Forms.Application.DoEvents()
					[tx_テンプレート名].Focus()
				Else
					PreviousControl.Focus()
				End If
			Case 11
				If Item_Check(cbFunc(Index).TabIndex) = True Then
					''                If Upload_Chk Then
					''                    cbFunc(Index).Caption = "登録"
					''                    PreviousControl.SetFocus
					''                End If
					''            End If
					''            Else
					If MsgBoxResult.Yes = YesNo("指定のデータを員数シート画面に貼り付けます。", Me.Text) Then
						System.Windows.Forms.Application.DoEvents()
						''                    cbFunc(Index).Enabled = False
						
						'''''                    If Upload Then
						If Upload_Kok Then
							'''''                        Call FormInitialize
							'''''                        [tx_テンプレート名].SetFocus
							
							Uploaded = True
							Me.Close()
							CLK2F = False
							Exit Sub
							
							''                    If ResultCallF = False Then         '2002/01/15
							''                        Call FormInitialize
							''                        [tx_得意先CD].SetFocus
							''                    Else
							''                        Loaded = False
							''                        cbClose = True
							''                        Exit Sub
							''                    End If
						Else
							PreviousControl.Focus()
						End If
					Else
						PreviousControl.Focus()
					End If
				End If
			Case 12
				Me.Close()
				CLK2F = False
				Exit Sub
		End Select
		cbFunc(Index).TabStop = False
		'ボタン２重起動防止フラグの初期化
		CLK2F = False
	End Sub
	
	Private Sub SnwMT02F06_FormClosing(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
		Dim Cancel As Boolean = eventArgs.Cancel
		Dim UnloadMode As System.Windows.Forms.CloseReason = eventArgs.CloseReason
		If Loaded Then
			'UPGRADE_NOTE: オブジェクト pParentForm をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
			pParentForm = Nothing
			
			If Uploaded Then
				''            pParentForm.ClearMode (True)
				''            pParentForm.Visible = True
				''            pParentForm.tx_得意先CD.SetFocus
				Exit Sub
			End If
			If MsgBoxResult.Yes = NoYes("現在の処理を終了します。") Then
				''            pParentForm.ClearMode (True)
				''            pParentForm.Visible = True
				''            pParentForm.tx_得意先CD.SetFocus
			Else
				PreviousControl.Focus()
				Cancel = True
			End If
		End If
		''    If Loaded Then
		''        cbClose.TabStop = True
		''        If vbYes = NoYes("現在の処理を終了します。") Then
		''        Else
		''            PreviousControl.SetFocus
		''            cbClose.TabStop = False
		''            Cancel = True
		''        End If
		''    End If
		eventArgs.Cancel = Cancel
	End Sub
	
	'UPGRADE_WARNING: イベント SnwMT02F06.Resize は、フォームが初期化されたときに発生します。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"' をクリックしてください。
	Private Sub SnwMT02F06_Resize(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Resize
		Dim [fpSpd] As Object
		If Me.WindowState <> System.Windows.Forms.FormWindowState.Minimized Then
			'見出しライン制御
			ln_over(0).X2 = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Me.Width))
			ln_over(1).X2 = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Me.Width))
			'フォーム最小（幅）制御
			If VB6.PixelsToTwipsX(Me.Width) < MeWidth Then
				Me.Width = VB6.TwipsToPixelsX(MeWidth)
			End If
			'フォーム最小（高さ）制御
			If VB6.PixelsToTwipsY(Me.Height) < MeHeight Then
				Me.Height = VB6.TwipsToPixelsY(MeHeightLimit)
			End If
			'UPGRADE_WARNING: オブジェクト fpSpd.Height の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[fpSpd].Height = VB6.PixelsToTwipsY(Me.Height) - MeHeight
		End If
	End Sub
	
	Private Sub SnwMT02F06_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		Dim [fpSpd] As Object
		Loaded = False
		
		'クラス生成
		cKokyakutmp = New clsKokyakutmp
		
		'SPREAD設定
		clsSPD_Renamed = New clsSPD
		clsSPD_Renamed.CtlSpd = [fpSpd]
		
		'フォームを画面の中央に配置
		Me.Top = VB6.TwipsToPixelsY((VB6.PixelsToTwipsY(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height) - VB6.PixelsToTwipsY(Me.Height)) \ 2)
		Me.Left = VB6.TwipsToPixelsX((VB6.PixelsToTwipsX(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width) - VB6.PixelsToTwipsX(Me.Width)) \ 2)
		
		'空白セット
		Call FormInitialize()
		'得意先セット
		[rf_得意先CD].Text = pTOKUCD
		rf_得意先名.Text = pTOKUNM
		
		'リサイズ用初期値設定（幅・高さ）
		MeWidth = VB6.PixelsToTwipsX(Me.Width)
		'UPGRADE_WARNING: オブジェクト fpSpd.Height の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト fpSpd.Top の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		WkHeight = [fpSpd].Top + [fpSpd].Height
		WkHeight = VB6.PixelsToTwipsY(picFunction.Top) - WkHeight
		'UPGRADE_WARNING: オブジェクト fpSpd.Height の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		MeHeight = VB6.PixelsToTwipsY(Me.Height) - [fpSpd].Height
		''    LvHeightLimit = fpSpd.Font.Size * (34 + 24)
		'UPGRADE_WARNING: オブジェクト fpSpd.Font の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		LvHeightLimit = [fpSpd].Font.Size * ((34 + 24) * 2) '一行分?
		MeHeightLimit = MeHeight + LvHeightLimit
		
		Loaded = True
		Uploaded = False
	End Sub
	
	Private Sub SnwMT02F06_KeyDown(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
		Dim KeyCode As Short = eventArgs.KeyCode
		Dim Shift As Short = eventArgs.KeyData \ &H10000
		Dim ctl As System.Windows.Forms.Button
		
		On Error GoTo Form_KeyDown_Err
		Select Case KeyCode
			Case System.Windows.Forms.Keys.Escape
				KeyCode = 0
				Exit Sub
			Case System.Windows.Forms.Keys.F1 To System.Windows.Forms.Keys.F12
				On Error Resume Next
				ctl = CType(Me.Controls("cbFunc"), Object)(KeyCode - System.Windows.Forms.Keys.F1 + 1)
				If Err.Number = 0 Then
					If ctl.Text <> vbNullString Then
						If ctl.Enabled = True Then
							ctl.Focus()
							If Err.Number = 0 Then
								Call SendReturnKey()
							End If
						End If
					End If
				End If
				'UPGRADE_NOTE: オブジェクト ctl をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
				ctl = Nothing
				KeyCode = 0
				On Error GoTo 0
				Exit Sub
		End Select
		
		Exit Sub
Form_KeyDown_Err: 
		MsgBox(Err.Number & " " & Err.Description)
	End Sub
	
	Private Sub SnwMT02F06_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
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
		''    Call ModeIndicate(Null)
		Call Set_Spread()
		cmdChkOn.Enabled = False
		cmdChkOff.Enabled = False
	End Sub
	
	Private Sub SetupBlank()
		Dim ctl As System.Windows.Forms.Control
		
		On Error Resume Next
		
		For	Each ctl In Me.Controls
			'UPGRADE_WARNING: TypeOf に新しい動作が指定されています。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
			If TypeOf ctl Is System.Windows.Forms.Label Then
				If ctl.Name Like "rf_*" Then
					ctl.Text = vbNullString
				End If
			End If
			'UPGRADE_WARNING: TypeOf に新しい動作が指定されています。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
			If TypeOf ctl Is AxExText.AxExTextBox Then
				ctl.Text = vbNullString
			End If
			'UPGRADE_WARNING: TypeOf に新しい動作が指定されています。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
			If TypeOf ctl Is AxExNmText.AxExNmTextBox Then
				ctl.Text = vbNullString
			End If
		Next ctl
		
		On Error GoTo 0
	End Sub
	
	Private Sub HldBlank()
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HTOKUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HTOKUCD = System.DBNull.Value
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HTEMPNM の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HTEMPNM = System.DBNull.Value
	End Sub
	
	Private Sub ModeIndicate(ByVal Mode As Object)
		''    If IsNull(Mode) Then
		''        [rf_処理区分].Caption = vbNullString
		''        [rf_処理区分].BackColor = &HE0E0E0
		''        ModeF = 0
		''    ElseIf Mode = 0 Then
		''        [rf_処理区分].Caption = "≪登 録≫"
		''        [rf_処理区分].BackColor = &HC0FFFF
		''        ModeF = 1
		''    ElseIf Mode = 1 Then
		''        [rf_処理区分].Caption = "≪修 正≫"
		''        [rf_処理区分].BackColor = &HC0FFC0
		''        ModeF = 2
		''    Else
		''        [rf_処理区分].Caption = "≪修 正≫"
		''        [rf_処理区分].BackColor = &HC0FFC0
		''        ModeF = 2
		''    End If
	End Sub
	
	Private Sub Set_Spread()
		Dim [fpSpd] As Object
		With [fpSpd]
			''        .MaxCols = 12
			'UPGRADE_WARNING: オブジェクト fpSpd.MaxRows の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.MaxRows = 300 '2007/11/17 UPD
			'UPGRADE_WARNING: オブジェクト fpSpd.EditModePermanent の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.EditModePermanent = True '常時入力モードを維持するかどうかを設定します。
			''        .ColsFrozen = 4
			'セル
			'UPGRADE_WARNING: オブジェクト fpSpd.UnitType の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.UnitType = FPSpreadADO.UnitTypeConstants.UnitTypeTwips
			'UPGRADE_WARNING: オブジェクト fpSpd.RowHeight の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.RowHeight(-1) = 250
			'シートのクリア
			Call clsSPD_Renamed.sprClearText()
			'セルの位置付け
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = 1
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row = 1
			'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Action = FPSpreadADO.ActionConstants.ActionActiveCell
		End With
	End Sub
	
	'UPGRADE_NOTE: Name は Name_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
	Private Sub SetUpFuncs(ByRef Name_Renamed As String)
		'ボタン名の変更
		Select Case Name_Renamed
			Case "tx_テンプレート名"
				cbFunc(1).Text = ""
				cbFunc(2).Text = ""
				cbFunc(3).Text = ""
				cbFunc(4).Text = ""
				cbFunc(5).Text = ""
				cbFunc(6).Text = ""
				cbFunc(7).Text = ""
				cbFunc(8).Text = ""
				cbFunc(9).Text = ""
				cbFunc(10).Text = ""
				cbFunc(11).Text = ""
			Case Else
				cbFunc(1).Text = ""
				cbFunc(2).Text = ""
				cbFunc(3).Text = ""
				cbFunc(4).Text = ""
				cbFunc(5).Text = ""
				cbFunc(6).Text = ""
				cbFunc(7).Text = ""
				cbFunc(8).Text = ""
				cbFunc(9).Text = ""
				cbFunc(10).Text = "中止"
				cbFunc(11).Text = "確定"
		End Select
	End Sub
	
	Private Sub cbTabEnd_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cbTabEnd.Enter
		If Item_Check((cbTabEnd.TabIndex)) Then
			cbFunc(11).Focus()
			cbFunc(11).PerformClick()
		End If
	End Sub
	
	'UPGRADE_ISSUE: PictureBox イベント picFunction.GotFocus はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="ABD9AF39-7E24-4AFF-AD8D-3675C1AA3054"' をクリックしてください。
	Private Sub picFunction_GotFocus()
		PreviousControl.Focus()
	End Sub
	
	Private Sub cmdChkOn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdChkOn.Click
		Dim [fpSpd] As Object
		Dim i, j As Short
		
		'UPGRADE_WARNING: オブジェクト fpSpd.DataRowCnt の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If [fpSpd].DataRowCnt = 0 Then
			Inform("対象データがありません。")
			Exit Sub
		End If
		
		j = GetSerchMax
		
		''    For i = 1 To fpSpd.DataRowCnt
		For i = 1 To j
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[fpSpd].Col = 1
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[fpSpd].Row = i
			'UPGRADE_WARNING: オブジェクト fpSpd.Text の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[fpSpd].Text = 1
		Next 
		
		'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		[fpSpd].SetFocus()
	End Sub
	
	Private Sub cmdChkOff_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdChkOff.Click
		Dim [fpSpd] As Object
		Dim i As Short
		
		'UPGRADE_WARNING: オブジェクト fpSpd.DataRowCnt の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If [fpSpd].DataRowCnt = 0 Then
			Exit Sub
		End If
		
		'UPGRADE_WARNING: オブジェクト fpSpd.DataRowCnt の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		For i = 1 To [fpSpd].DataRowCnt
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[fpSpd].Col = 1
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[fpSpd].Row = i
			'UPGRADE_WARNING: オブジェクト fpSpd.Text の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[fpSpd].Text = 0
		Next 
		
		'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		[fpSpd].SetFocus()
	End Sub
	
	Private Sub tx_テンプレート名_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_テンプレート名.Enter
		If SelectF = False Then
			'入力ﾁｪｯｸ
			If Item_Check(([tx_テンプレート名].TabIndex)) = False Then
				Exit Sub
			End If
			PreviousControl = Me.ActiveControl
			''        Call FormInitialize
		End If
		SelectF = False
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(1).Text = "テンプレート名を入力して下さい。　選択画面：Space"
	End Sub
	
	Private Sub tx_テンプレート名_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_テンプレート名.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		If KeyAscii = Asc(" ") Or KeyAscii = Asc("　") And ([tx_テンプレート名].SelectionStart = 0 And [tx_テンプレート名].SelectionLength = Len([tx_テンプレート名].Text)) Then
			KeyAscii = 0
			'---参照画面表示
			SelectF = True
			cKokyakutmp.得意先CD = [rf_得意先CD].Text
			If cKokyakutmp.ShowDialog = True Then
				[tx_テンプレート名].Text = cKokyakutmp.テンプレート名
				ReturnF = True
				[tx_テンプレート名].Focus()
			Else
				[tx_テンプレート名].Focus()
			End If
		End If
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
	
	Private Sub tx_テンプレート名_RtnKeyDown(ByRef KeyCode As Short, ByRef Shift As Short, ByRef Cancel As Boolean)
		If [tx_テンプレート名].Text = "" Then
			KeyCode = 0
			'---参照画面表示
			SelectF = True
			cKokyakutmp.得意先CD = [rf_得意先CD].Text
			If cKokyakutmp.ShowDialog = True Then
				[tx_テンプレート名].Text = cKokyakutmp.テンプレート名
				[tx_テンプレート名].Focus()
			Else
				[tx_テンプレート名].Focus()
			End If
		End If
		
		ReturnF = True
	End Sub
	
	Private Sub tx_テンプレート名_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_テンプレート名.Leave
		''    [sb_Msg].Panels(1).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_テンプレート名.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_テンプレート名].Undo()
		End If
		ReturnF = False
	End Sub
	
	Private Sub fpSpd_GotFocus()
		Dim [fpSpd] As Object
		'入力ﾁｪｯｸ
		'UPGRADE_WARNING: オブジェクト fpSpd.TabIndex の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If Item_Check([fpSpd].TabIndex) = False Then
			Exit Sub
		End If
		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'ホイールコントロール制御開始
		'UPGRADE_WARNING: オブジェクト fpSpd の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Call StartWheel([fpSpd])
	End Sub
	
	Private Sub fpSpd_LostFocus()
		'ホイールコントロール制御解除
		Call EndWheel()
	End Sub
	
	Private Sub fpSpd_Advance(ByVal AdvanceNext As Boolean)
		Dim [fpSpd] As Object
		''    Debug.Print "fpSpd_Advance = " & AdvanceNext
		'UPGRADE_WARNING: オブジェクト fpSpd.MaxRows の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If [fpSpd].ActiveRow = [fpSpd].MaxRows Then
			If AdvanceNext = True Then
				cbTabEnd.Focus()
			End If
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		ElseIf [fpSpd].ActiveRow = 1 Then 
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If [fpSpd].ActiveCol = 1 Then
				If AdvanceNext = False Then
					[tx_テンプレート名].Focus()
				End If
			End If
		End If
	End Sub
	
	Private Sub fpSpd_EditMode(ByVal Col As Integer, ByVal Row As Integer, ByVal Mode As Short, ByVal ChangeMade As Boolean)
		Dim [fpSpd] As Object
		''    Debug.Print "fpSpd_EditMode = " & Col & ":" & Row & ":" & Mode & ":" & ChangeMade
		
		If Mode = 0 Then Exit Sub 'フォーカスがないならば
		If ChangeMade = True Then Exit Sub
		
		'行の色を変える
		With [fpSpd]
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = -1
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row = .ActiveRow
			'UPGRADE_WARNING: オブジェクト fpSpd.BackColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.BackColor = &HFFFFC0
		End With
		
		''    Debug.Print "EditMode col & Row & HoldCD = " & Col & ":" & Row & ":" & HoldCD
	End Sub
	
	Private Sub fpSpd_LeaveCell(ByVal Col As Integer, ByVal Row As Integer, ByVal NewCol As Integer, ByVal NewRow As Integer, ByRef Cancel As Boolean)
		Dim fpSpd As Object
		
		SpreadErr = False
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		If Me.ActiveControl.Name = "cbClose" Then Exit Sub
		
		'行の色を変える
		With [fpSpd]
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = -1
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row = Row
			'UPGRADE_WARNING: オブジェクト fpSpd.BackColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.BackColor = &HFFFFFFFF
			If NewRow > 0 Then
				'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Col = -1
				'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Row = NewRow
				'UPGRADE_WARNING: オブジェクト fpSpd.BackColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.BackColor = &HFFFFC0
			End If
		End With
		
	End Sub
	
	Private Function Item_Check(ByRef ItemNo As Short) As Short
		Dim fpSpd As Object
		Dim bufName As String '名前取得用バッファ
		Dim buf2 As String
		Dim bufCur As Decimal
		Dim Chk_ID As String 'ﾁｪｯｸ用ワーク
		
		On Error GoTo Item_Check_Err
		Item_Check = False
		
		'キー項目「テンプレート名」のﾁｪｯｸ
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HTEMPNM の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If ItemNo > tx_テンプレート名.TabIndex And ([tx_テンプレート名].Text <> HTEMPNM Or IsDbNull(HTEMPNM)) Then
			If IsCheckText([tx_テンプレート名]) = False Then
				CriticalAlarm("テンプレート名が未入力です。")
				'UPGRADE_WARNING: オブジェクト tx_テンプレート名.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				[tx_テンプレート名].Undo()
				[tx_テンプレート名].Focus()
				Exit Function
			End If
			
			'--- データ表示
			'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト HTEMPNM の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If ([tx_テンプレート名].Text <> HTEMPNM Or IsDbNull(HTEMPNM)) Then
				If Download(1, [rf_得意先CD].Text, [tx_テンプレート名].Text) = -1 Then
					CriticalAlarm("指定の顧客テンプレートは未登録です。")
					'UPGRADE_WARNING: オブジェクト tx_テンプレート名.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					[tx_テンプレート名].Undo()
					[tx_テンプレート名].Focus()
					Exit Function
				End If
				
				cmdChkOn.Enabled = True
				cmdChkOff.Enabled = True
				
			End If
			
			'--- 入力値をワークへ格納
			'UPGRADE_WARNING: オブジェクト HTEMPNM の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			HTEMPNM = [tx_テンプレート名].Text
		End If
		
		'-----再読み込み用のクリア
		'UPGRADE_WARNING: オブジェクト fpSpd.TabIndex の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If ItemNo < [fpSpd].TabIndex Then
			[tx_テンプレート名].Text = vbNullString
			'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト HTEMPNM の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			HTEMPNM = System.DBNull.Value
			Call clsSPD_Renamed.sprClearText()
			cmdChkOn.Enabled = False
			cmdChkOff.Enabled = False
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
				IDName = NullToZero(.Fields("得意先名1"), vbNullString)
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
		Dim [fpSpd] As Object
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
				''            InitDate = Date
				''''            Call InitialItems
				Download = -1
			Else
				''''            InitDate = ![初期登録日]
				Call SetupItems(rs)
				If Mode = 2 Then '選択画面より選択時
					'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetFocus()
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
		Dim [fpSpd] As Object
		Dim RecArry() As Object
		Dim i, j As Short
		
		'シートのクリア
		Call clsSPD_Renamed.sprClearText()
		
		'UPGRADE_WARNING: Array に新しい動作が指定されています。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト fpSpd.MaxRows の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト rs.GetRows() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		RecArry = rs.GetRows([fpSpd].MaxRows,  , New Object(){"PC区分", "製品NO", "仕様NO", "ベース色", "漢字名称", "W", "D", "H", "D1", "D2", "H1", "H2", "単位名", "仕入先CD", "マスタ"})
		
		For i = 0 To UBound(RecArry, 2)
			For j = 0 To UBound(RecArry)
				Select Case j + 1
					Case 6 To 12
						'UPGRADE_WARNING: オブジェクト RecArry() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						[fpSpd].SetText(j + 2, i + 1, IIf(Trim("" & RecArry(j, i)) = "0", "", Trim("" & RecArry(j, i))))
					Case Else
						'UPGRADE_WARNING: オブジェクト RecArry() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						[fpSpd].SetText(j + 2, i + 1, Trim("" & RecArry(j, i)))
				End Select
			Next 
		Next 
		
		'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		[fpSpd].Col = 1
		'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		[fpSpd].Row = 1
		'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		[fpSpd].Action = FPSpreadADO.ActionConstants.ActionActiveCell
		
		'HOLDセット
		'UPGRADE_WARNING: オブジェクト HTOKUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HTOKUCD = [rf_得意先CD].Text
		'UPGRADE_WARNING: オブジェクト HTEMPNM の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HTEMPNM = [tx_テンプレート名].Text
	End Sub
	
	Private Function GetSerchMax() As Short
		Dim [fpSpd] As Object
		Dim i As Short
		Dim j As Short
		Dim getdata As Object
		Dim check As Boolean
		
		GetSerchMax = 0
		
		'UPGRADE_WARNING: オブジェクト fpSpd.DataRowCnt の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If [fpSpd].DataRowCnt = 0 Then
			CriticalAlarm("明細がありません。")
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[fpSpd].Col = [fpSpd].ActiveRow
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[fpSpd].Row = [fpSpd].ActiveRow
			'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[fpSpd].Action = FPSpreadADO.ActionConstants.ActionActiveCell
			'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[fpSpd].SetFocus()
			Exit Function
		End If
		
		'UPGRADE_WARNING: 配列 WkTBLS の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト fpSpd.DataRowCnt の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		ReDim WkTBLS([fpSpd].DataRowCnt)
		
		With [fpSpd]
			'ワークにホールド
			'UPGRADE_WARNING: オブジェクト fpSpd.DataRowCnt の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			For i = 1 To .DataRowCnt
				'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				check = .GetText(1, i, getdata) 'チェック
				If check Then
					'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					WkTBLS(i).WKCheck = getdata
				End If
				'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				check = .GetText(2, i, getdata) 'PC区分
				If check Then
					'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					WkTBLS(i).WKPCKUBN = getdata
				End If
				'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				check = .GetText(3, i, getdata) '製品NO
				If check Then
					'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					WkTBLS(i).WKSEIHNO = getdata
				End If
				'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				check = .GetText(4, i, getdata) '仕様NO
				If check Then
					'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					WkTBLS(i).WKSIYONO = getdata
				End If
				'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				check = .GetText(6, i, getdata) '漢字名称
				If check Then
					'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					WkTBLS(i).WKNAME = getdata
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
		Dim [fpSpd] As Object
		Dim getdata As Object
		Dim check As Boolean
		Dim CheckData As Object
		Dim ResData As Object
		Dim i, j As Short
		Dim wRow As Short
		
		Upload_Kok = False
		
		wRow = GetSerchMax
		
		''    For i = 1 To fpSpd.DataRowCnt
		For i = 1 To wRow
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[fpSpd].Col = 1
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[fpSpd].Row = i
			'UPGRADE_WARNING: オブジェクト fpSpd.Text の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If [fpSpd].Text <> "" Then
				'UPGRADE_WARNING: オブジェクト fpSpd.Text の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				If [fpSpd].Text = 1 Or Trim([fpSpd].Text) = "1" Then
					j = j + 1
				End If
			End If
		Next 
		
		If j = 0 Then
			Inform("対象データがありません。")
			Exit Function
		End If
		
		'UPGRADE_WARNING: オブジェクト fpSpd.MaxCols の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		ReDim ResData(j - 1, [fpSpd].MaxCols - 1)
		j = 0
		
		With [fpSpd]
			''        For i = 1 To UBound(ResData, 1)
			'UPGRADE_WARNING: オブジェクト fpSpd.MaxRows の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			For i = 1 To .MaxRows
				'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Col = 1
				'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Row = i
				'UPGRADE_WARNING: オブジェクト fpSpd.Text の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				If .Text = "1" Then
					'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					check = .GetText(2, i, getdata) 'PC区分
					If check Then
						'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト ResData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						ResData(j, 0) = getdata
					End If
					'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					check = .GetText(3, i, getdata) '製品NO
					If check Then
						'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト ResData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						ResData(j, 1) = getdata
					End If
					'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					check = .GetText(4, i, getdata) '仕様NO
					If check Then
						'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト ResData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						ResData(j, 2) = getdata
					End If
					'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					check = .GetText(5, i, getdata) 'ベース色
					If check Then
						'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト ResData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						ResData(j, 3) = getdata
					End If
					'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					check = .GetText(6, i, getdata) '名称
					If check Then
						'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト ResData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						ResData(j, 4) = getdata
					End If
					'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					check = .GetText(7, i, getdata) 'W
					If check Then
						'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト ResData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						ResData(j, 5) = getdata
					End If
					'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					check = .GetText(8, i, getdata) 'D
					If check Then
						'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト ResData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						ResData(j, 6) = getdata
					End If
					'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					check = .GetText(9, i, getdata) 'H
					If check Then
						'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト ResData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						ResData(j, 7) = getdata
					End If
					'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					check = .GetText(10, i, getdata) 'D1
					If check Then
						'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト ResData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						ResData(j, 8) = getdata
					End If
					'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					check = .GetText(11, i, getdata) 'D2
					If check Then
						'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト ResData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						ResData(j, 9) = getdata
					End If
					'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					check = .GetText(12, i, getdata) 'H1
					If check Then
						'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト ResData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						ResData(j, 10) = getdata
					End If
					'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					check = .GetText(13, i, getdata) 'H2
					If check Then
						'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト ResData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						ResData(j, 11) = getdata
					End If
					'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					check = .GetText(14, i, getdata) '単位
					If check Then
						'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト ResData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						ResData(j, 12) = getdata
					End If
					'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					check = .GetText(15, i, getdata) '仕入先CD
					If check Then
						'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト ResData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						ResData(j, 13) = getdata
					End If
					
					j = j + 1
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
	Public Function SelSetup(ByRef TOKUCD As String, ByRef TOKUNM As String) As Object
		
		pTOKUCD = TOKUCD
		pTOKUNM = TOKUNM
		
	End Function
End Class