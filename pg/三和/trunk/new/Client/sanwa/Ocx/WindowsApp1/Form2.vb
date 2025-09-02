Public Class Form2
    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles Me.Load
		Call FormInitialize()
	End Sub

	Private Sub FormInitialize()
		Call SetupBlank()
	End Sub

	'空白セット
	Private Sub SetupBlank()
		ClearControls(Me)
	End Sub

	Private Sub ClearControls(ByVal parent As Control)
		Dim ctl As System.Windows.Forms.Control
		Try
			For Each ctl In parent.Controls
				If TypeOf ctl Is TextBox Then
					DirectCast(ctl, TextBox).Text = ""
				End If
				If TypeOf ctl Is System.Windows.Forms.Label Then
					If ctl.Name Like "rf_*" Then
						ctl.Text = vbNullString
					End If
				End If
				If TypeOf ctl Is ExText.ExTextBox Then
					ctl.Text = vbNullString
				End If
				If TypeOf ctl Is ExNmText.ExNmTextBox Then
					ctl.Text = vbNullString
				End If
				If TypeOf ctl Is ExDateText.ExDateTextBoxY Then
					ctl.Text = vbNullString
				End If
				If TypeOf ctl Is ExDateText.ExDateTextBoxM Then
					ctl.Text = vbNullString
				End If
				If TypeOf ctl Is ExDateText.ExDateTextBoxD Then
					ctl.Text = vbNullString
				End If

				' 子コントロールがある場合、再帰的に処理
				If ctl.HasChildren Then
					ClearControls(ctl)
				End If
			Next ctl
		Catch ex As Exception
			MessageBox.Show("エラー発生: " & ex.Message)
		End Try
	End Sub

End Class