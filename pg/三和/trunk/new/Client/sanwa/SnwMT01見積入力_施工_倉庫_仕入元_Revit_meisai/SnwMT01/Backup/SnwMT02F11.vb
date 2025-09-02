Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.PowerPacks
Friend Class SnwMT02F11
	Inherits System.Windows.Forms.Form
	
	Dim ResultCodeSetControl As AxFPSpreadADO.AxfpSpread '選択したコードの送り先をセットする。
	
	Dim ResultCodeGenka As Decimal '原価
	Dim ResultCodeBaika As Decimal '売価
	
	'選択したコードを送るコントロールをセット
	WriteOnly Property ResCodeCTL() As System.Windows.Forms.Control
		Set(ByVal Value As System.Windows.Forms.Control)
			ResultCodeSetControl = Value
		End Set
	End Property
	'原価をセット
	WriteOnly Property ResCodeGenka() As Decimal
		Set(ByVal Value As Decimal)
			ResultCodeGenka = Value
		End Set
	End Property
	'売価をセット
	WriteOnly Property ResCodeBaika() As Decimal
		Set(ByVal Value As Decimal)
			ResultCodeBaika = Value
		End Set
	End Property
	
	Private Sub SnwMT02F11_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		rf_原価.Text = VB6.Format(SpcToNull(ResultCodeGenka, 0), "#,##0.00")
		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		rf_売価.Text = VB6.Format(SpcToNull(ResultCodeBaika, 0), "#,##0.00")
		If rf_原価.Text = "0.00" Then
			tx_掛率.Text = CStr(0)
		Else
			tx_掛率.Text = VB6.Format(ISRound((CDbl(rf_売価.Text) / CDbl(rf_原価.Text)) * 100, 2), "#,##0.00")
		End If
		'''    rf_売価 = Format$((rf_原価 * tx_掛率) / 100, "#,##0.00")
	End Sub
	
	Private Sub cb中止_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cb中止.Click
		Me.Close()
	End Sub
	
	Private Sub cdOK_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cdOK.Click
		If Len("" & rf_売価.Text) > 13 Then
			Inform("桁がオーバーフローしました。")
			''        PreviousControl.Undo
			''        PreviousControl.SetFocus
			Exit Sub
		End If
		
		ResultCodeSetControl.Text = rf_売価.Text
		
		Me.Close()
	End Sub
	
	Private Sub Ob_条件_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Ob_条件.Enter
		Dim Index As Short = Ob_条件.GetIndex(eventSender)
		Call StartOption([Ob_条件](Index))
	End Sub
	
	'UPGRADE_WARNING: イベント Ob_条件.CheckedChanged は、フォームが初期化されたときに発生します。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"' をクリックしてください。
	Private Sub Ob_条件_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Ob_条件.CheckedChanged
		If eventSender.Checked Then
			Dim Index As Short = Ob_条件.GetIndex(eventSender)
			
			If ([Ob_条件](0).Checked = True) Then
				[lb_項目](5).Text = "掛  率"
				If CDec(rf_原価.Text) = 0 Then '2004/03/05
				Else
					tx_掛率.Text = VB6.Format(ISRound((CDbl(rf_売価.Text) / CDbl(rf_原価.Text)) * 100, 2), "#,##0.00")
				End If
			Else
				[lb_項目](5).Text = "原価率"
				If CDec(rf_売価.Text) = 0 Then '2004/03/05
				Else
					tx_掛率.Text = VB6.Format(ISRound((CDbl(rf_原価.Text) / CDbl(rf_売価.Text)) * 100, 2), "#,##0.00")
				End If
			End If
			
		End If
	End Sub
	
	Private Sub Ob_条件_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Ob_条件.Leave
		Dim Index As Short = Ob_条件.GetIndex(eventSender)
		Call EndOption()
	End Sub
	
	'UPGRADE_ISSUE: PictureBox イベント pic_印刷条件.GotFocus はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="ABD9AF39-7E24-4AFF-AD8D-3675C1AA3054"' をクリックしてください。
	Private Sub pic_印刷条件_GotFocus()
		If ([Ob_条件](0).Checked = True) Then
			[Ob_条件](0).Focus()
		ElseIf ([Ob_条件](1).Checked = True) Then 
			[Ob_条件](1).Focus()
		End If
	End Sub
	
	Private Sub tx_掛率_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_掛率.Leave
		Select Case True
			Case [Ob_条件](0).Checked
				rf_売価.Text = VB6.Format((CDbl(rf_原価.Text) * CDbl(tx_掛率.Text)) / 100, "#,##0.00")
			Case [Ob_条件](1).Checked
				If CDec(tx_掛率.Text) = 0 Then '2004/04/01
				Else
					rf_売価.Text = VB6.Format((CDbl(rf_原価.Text) / CDbl(tx_掛率.Text)) * 100, "#,##0.00")
				End If
		End Select
		
	End Sub
End Class