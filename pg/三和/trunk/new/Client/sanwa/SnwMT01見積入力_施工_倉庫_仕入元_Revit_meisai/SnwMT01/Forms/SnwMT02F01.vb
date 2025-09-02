Option Strict Off
Option Explicit On

''' <summary>
''' --------------------------------------------------------------------
'''   ユーザー名           株式会社三和商研
'''   業務名               積算データ管理システム
'''   部門名               見積部門
'''   プログラム名         員数入力処理（定価の掛率設定）
'''   作成会社             テクノウェア株式会社
'''   作成日               2003/06/13
'''   作成者               kawamura
''' --------------------------------------------------------------------
''' </summary>
Friend Class SnwMT02F01
	Inherits System.Windows.Forms.Form

	Dim pParentForm As SnwMT02F00
	Dim pMituNo As Integer
	Dim pSetCol As Integer

	Dim PreviousControl As System.Windows.Forms.Control 'コントロールの戻りを制御

	Private Sub SnwMT02F01_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Me.Load
		On Error GoTo SysErr_Form_Load

		'フォームを画面の中央に配置
		Me.Top = VB6Conv.TwipsToPixelsY((VB6Conv.PixelsToTwipsY(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height) - VB6Conv.PixelsToTwipsY(Me.Height)) \ 2)
		Me.Left = VB6Conv.TwipsToPixelsX((VB6Conv.PixelsToTwipsX(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width) - VB6Conv.PixelsToTwipsX(Me.Width)) \ 2)

		rf_見積番号.Text = pMituNo.ToString("#")
		Tx_掛率.Text = CStr(0)

		Exit Sub
SysErr_Form_Load:
		MsgBox(Err.Number & " " & Err.Description)
	End Sub

	Private Sub SnwMT02F01_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
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

	Private Sub Cb原価計算_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Cb原価計算.Enter
		If Item_Check((Cb原価計算.TabIndex)) = False Then
			Exit Sub
		End If
	End Sub

	Private Sub Cb原価計算_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Cb原価計算.Click
		If MsgBox("指定された掛率で原価価を計算します。", MsgBoxStyle.Question + MsgBoxStyle.OkCancel, Me.Text) = MsgBoxResult.Ok Then
			System.Windows.Forms.Application.DoEvents()
			'UPGRADE_ISSUE: Control DspGenka は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
			Call pParentForm.DspGenka(Decimal.Parse(Tx_掛率.Text))
			Me.Close()
		End If
	End Sub

	Private Sub CbCalc_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CbCalc.Enter
		If Item_Check((CbCalc.TabIndex)) = False Then
			Exit Sub
		End If
	End Sub

	Private Sub CbCalc_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CbCalc.Click
		If MsgBox("指定された掛率で売価を計算します。", MsgBoxStyle.Question + MsgBoxStyle.OkCancel, Me.Text) = MsgBoxResult.Ok Then
			System.Windows.Forms.Application.DoEvents()
			'UPGRADE_ISSUE: Control DspBaika は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
			Call pParentForm.DspBaika(Decimal.Parse(Tx_掛率.Text))
			Me.Close()
		End If
	End Sub

	Private Sub Tx_掛率_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Tx_掛率.Enter
		PreviousControl = Me.ActiveControl
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "掛率を入力して下さい。"
	End Sub

	Private Sub Tx_掛率_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Tx_掛率.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
	End Sub

	Private Function Item_Check(ByRef ItemNo As Integer) As Boolean

		On Error GoTo Item_Check_Err
		Item_Check = False

		'掛率のチェック
		'    If ItemNo > [tx_掛率].TabIndex Then
		'        If NullToZero([tx_掛率].Text, 0) = 0 Then
		'            CriticalAlarm "掛率を入力して下さい。"
		'            [tx_掛率].SetFocus
		'            Exit Function
		'        End If
		'    End If

		Item_Check = True

		Exit Function
Item_Check_Err:
		MsgBox(Err.Number & " " & Err.Description)
	End Function

	Private Sub Download()
		'nop
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

End Class