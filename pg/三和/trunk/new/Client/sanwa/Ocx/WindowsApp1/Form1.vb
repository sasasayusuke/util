Public Class Form1

	Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
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

	Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
		Form2.Show()
	End Sub

	Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
		Debug.WriteLine("ExTextBox1:" & ExTextBox1.Enabled & "," & ExTextBox1.Locked)
		MessageBox.Show(ExTextBox1.Text, "ExText")

		Debug.WriteLine("ExNmTextBox1:" & ExNmTextBox1.Enabled & "," & ExNmTextBox1.Locked)
		MessageBox.Show(ExNmTextBox1.Text, "ExNmText")

		Debug.WriteLine("ExDateTextBoxY1:" & ExDateTextBoxY1.Enabled & "," & ExDateTextBoxY1.Locked)
		Debug.WriteLine("ExDateTextBoxM1:" & ExDateTextBoxM1.Enabled & "," & ExDateTextBoxM1.Locked)
		Debug.WriteLine("ExDateTextBoxD1:" & ExDateTextBoxD1.Enabled & "," & ExDateTextBoxD1.Locked)
		MessageBox.Show(ExDateTextBoxY1.Text, "ExDateTextY")
		MessageBox.Show(ExDateTextBoxM1.Text, "ExDateTextM")
		MessageBox.Show(ExDateTextBoxD1.Text, "ExDateTextD")
	End Sub

	Private Sub ExTextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles ExTextBox1.KeyPress
		Debug.Print("ExTextBox1_KeyPress:" & ExTextBox1.Text)
		Debug.Print("ExTextBox1_KeyPress:" & ExTextBox1.OldValue)
		Debug.Print("ExTextBox1_KeyPress:" & ExTextBox1.SelText & "," & ExTextBox1.SelLength & "," & ExTextBox1.SelectText)
	End Sub

	Private Sub ExTextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles ExTextBox1.KeyDown
		Debug.Print("ExTextBox1_KeyDown :" & ExTextBox1.Text)
		Debug.Print("ExTextBox1_KeyDown :" & ExTextBox1.OldValue)
		Debug.Print("ExTextBox1_KeyDown :" & ExTextBox1.SelText & "," & ExTextBox1.SelLength & "," & ExTextBox1.SelectText)
	End Sub

	Private Sub ExTextBox1_TextChange() Handles ExTextBox1.TextChange
		Debug.Print("ExTextBox1_TextChange:" & ExTextBox1.Text)
		Debug.Print("ExTextBox1_TextChange:" & ExTextBox1.OldValue)
		Debug.Print("ExTextBox1_TextChange:" & ExTextBox1.SelText & "," & ExTextBox1.SelLength & "," & ExTextBox1.SelectText)
	End Sub

	Private Sub ExTextBox1_Enter(sender As Object, e As EventArgs) Handles ExTextBox1.Enter
		Debug.Print("ExTextBox1_Enter   :" & ExTextBox1.Text)
		Debug.Print("ExTextBox1_Enter   :" & ExTextBox1.OldValue)
		Debug.Print("ExTextBox1_Enter   :" & ExTextBox1.SelText & "," & ExTextBox1.SelLength & "," & ExTextBox1.SelectText)
	End Sub

	Private Sub ExTextBox1_Leave(sender As Object, e As EventArgs) Handles ExTextBox1.Leave
		Debug.Print("ExTextBox1_Leave   :" & ExTextBox1.Text)
		Debug.Print("ExTextBox1_Leave   :" & ExTextBox1.OldValue)
		Debug.Print("ExTextBox1_Leave   :" & ExTextBox1.SelText & "," & ExTextBox1.SelLength & "," & ExTextBox1.SelectText)
	End Sub

	Private Sub ExTextBox1_GotFocus(sender As Object, e As EventArgs) Handles ExTextBox1.GotFocus
		Debug.Print("ExTextBox1_GotFocus:" & ExTextBox1.Text)
		Debug.Print("ExTextBox1_GotFocus:" & ExTextBox1.OldValue)
		Debug.Print("ExTextBox1_GotFocus:" & ExTextBox1.SelText & "," & ExTextBox1.SelLength & "," & ExTextBox1.SelectText)
	End Sub

	Private Sub ExTextBox1_LostFocus(sender As Object, e As EventArgs) Handles ExTextBox1.LostFocus
		Debug.Print("ExTextBox1_LostFocus:" & ExTextBox1.Text)
		Debug.Print("ExTextBox1_LostFocus:" & ExTextBox1.OldValue)
		Debug.Print("ExTextBox1_LostFocus:" & ExTextBox1.SelText & "," & ExTextBox1.SelLength & "," & ExTextBox1.SelectText)
	End Sub

	Private Sub ExNmTextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles ExNmTextBox1.KeyPress
		Debug.Print("ExNmTextBox1_KeyPress:" & ExNmTextBox1.Text)
		Debug.Print("ExNmTextBox1_KeyPress:" & ExNmTextBox1.OldValue)
		Debug.Print("ExNmTextBox1_KeyPress:" & ExNmTextBox1.SelText & "," & ExNmTextBox1.SelLength & "," & ExNmTextBox1.SelectText)
	End Sub

	Private Sub ExNmTextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles ExNmTextBox1.KeyDown
		Debug.Print("ExNmTextBox1_KeyDown :" & ExNmTextBox1.Text)
		Debug.Print("ExNmTextBox1_KeyDown :" & ExNmTextBox1.OldValue)
		Debug.Print("ExNmTextBox1_KeyDown :" & ExNmTextBox1.SelText & "," & ExNmTextBox1.SelLength & "," & ExNmTextBox1.SelectText)
	End Sub

	Private Sub ExNmTextBox1_TextChange() Handles ExNmTextBox1.TextChange
		Debug.Print("ExNmTextBox1_TextChange:" & ExNmTextBox1.Text)
		Debug.Print("ExNmTextBox1_TextChange:" & ExNmTextBox1.OldValue)
		Debug.Print("ExNmTextBox1_TextChange:" & ExNmTextBox1.SelText & "," & ExNmTextBox1.SelLength & "," & ExNmTextBox1.SelectText)
	End Sub

	Private Sub ExNmTextBox1_Enter(sender As Object, e As EventArgs) Handles ExNmTextBox1.Enter
		Debug.Print("ExNmTextBox1_Enter   :" & ExNmTextBox1.Text)
		Debug.Print("ExNmTextBox1_Enter   :" & ExNmTextBox1.OldValue)
		Debug.Print("ExNmTextBox1_Enter   :" & ExNmTextBox1.SelText & "," & ExNmTextBox1.SelLength & "," & ExNmTextBox1.SelectText)
	End Sub

	Private Sub ExNmTextBox1_Leave(sender As Object, e As EventArgs) Handles ExNmTextBox1.Leave
		Debug.Print("ExNmTextBox1_Leave   :" & ExNmTextBox1.Text)
		Debug.Print("ExNmTextBox1_Leave   :" & ExNmTextBox1.OldValue)
		Debug.Print("ExNmTextBox1_Leave   :" & ExNmTextBox1.SelText & "," & ExNmTextBox1.SelLength & "," & ExNmTextBox1.SelectText)
	End Sub

	Private Sub ExNmTextBox1_GotFocus(sender As Object, e As EventArgs) Handles ExNmTextBox1.GotFocus
		Debug.Print("ExNmTextBox1_GotFocus:" & ExNmTextBox1.Text)
		Debug.Print("ExNmTextBox1_GotFocus:" & ExNmTextBox1.OldValue)
		Debug.Print("ExNmTextBox1_GotFocus:" & ExNmTextBox1.SelText & "," & ExNmTextBox1.SelLength & "," & ExNmTextBox1.SelectText)
	End Sub

	Private Sub ExNmTextBox1_LostFocus(sender As Object, e As EventArgs) Handles ExNmTextBox1.LostFocus
		Debug.Print("ExNmTextBox1_LostFocus:" & ExNmTextBox1.Text)
		Debug.Print("ExNmTextBox1_LostFocus:" & ExNmTextBox1.OldValue)
		Debug.Print("ExNmTextBox1_LostFocus:" & ExNmTextBox1.SelText & "," & ExNmTextBox1.SelLength & "," & ExNmTextBox1.SelectText)
	End Sub

	Private Sub ExDateTextBoxY1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles ExDateTextBoxY1.KeyPress
		Debug.Print("ExDateTextBoxY1_KeyPress:" & ExDateTextBoxY1.Text)
		Debug.Print("ExDateTextBoxY1_KeyPress:" & ExDateTextBoxY1.OldValue)
		Debug.Print("ExDateTextBoxY1_KeyPress:" & ExDateTextBoxY1.SelText & "," & ExDateTextBoxY1.SelLength & "," & ExDateTextBoxY1.SelectText)
	End Sub

	Private Sub ExDateTextBoxY1_KeyDown(sender As Object, e As KeyEventArgs) Handles ExDateTextBoxY1.KeyDown
		Debug.Print("ExDateTextBoxY1_KeyDown :" & ExDateTextBoxY1.Text)
		Debug.Print("ExDateTextBoxY1_KeyDown :" & ExDateTextBoxY1.OldValue)
		Debug.Print("ExDateTextBoxY1_KeyDown :" & ExDateTextBoxY1.SelText & "," & ExDateTextBoxY1.SelLength & "," & ExDateTextBoxY1.SelectText)
	End Sub

	Private Sub ExDateTextBoxY1_TextChange() Handles ExDateTextBoxY1.TextChange
		Debug.Print("ExDateTextBoxY1_TextChange:" & ExDateTextBoxY1.Text)
		Debug.Print("ExDateTextBoxY1_TextChange:" & ExDateTextBoxY1.OldValue)
		Debug.Print("ExDateTextBoxY1_TextChange:" & ExDateTextBoxY1.SelText & "," & ExDateTextBoxY1.SelLength & "," & ExDateTextBoxY1.SelectText)
	End Sub

	Private Sub ExDateTextBoxY1_Enter(sender As Object, e As EventArgs) Handles ExDateTextBoxY1.Enter
		Debug.Print("ExDateTextBoxY1_Enter   :" & ExDateTextBoxY1.Text)
		Debug.Print("ExDateTextBoxY1_Enter   :" & ExDateTextBoxY1.OldValue)
		Debug.Print("ExDateTextBoxY1_Enter   :" & ExDateTextBoxY1.SelText & "," & ExDateTextBoxY1.SelLength & "," & ExDateTextBoxY1.SelectText)
	End Sub

	Private Sub ExDateTextBoxY1_Leave(sender As Object, e As EventArgs) Handles ExDateTextBoxY1.Leave
		Debug.Print("ExDateTextBoxY1_Leave   :" & ExDateTextBoxY1.Text)
		Debug.Print("ExDateTextBoxY1_Leave   :" & ExDateTextBoxY1.OldValue)
		Debug.Print("ExDateTextBoxY1_Leave   :" & ExDateTextBoxY1.SelText & "," & ExDateTextBoxY1.SelLength & "," & ExDateTextBoxY1.SelectText)
	End Sub

	Private Sub ExDateTextBoxY1_GotFocus(sender As Object, e As EventArgs) Handles ExDateTextBoxY1.GotFocus
		Debug.Print("ExDateTextBoxY1_GotFocus:" & ExDateTextBoxY1.Text)
		Debug.Print("ExDateTextBoxY1_GotFocus:" & ExDateTextBoxY1.OldValue)
		Debug.Print("ExDateTextBoxY1_GotFocus:" & ExDateTextBoxY1.SelText & "," & ExDateTextBoxY1.SelLength & "," & ExDateTextBoxY1.SelectText)
	End Sub

	Private Sub ExDateTextBoxY1_LostFocus(sender As Object, e As EventArgs) Handles ExDateTextBoxY1.LostFocus
		Debug.Print("ExDateTextBoxY1_LostFocus:" & ExDateTextBoxY1.Text)
		Debug.Print("ExDateTextBoxY1_LostFocus:" & ExDateTextBoxY1.OldValue)
		Debug.Print("ExDateTextBoxY1_LostFocus:" & ExDateTextBoxY1.SelText & "," & ExDateTextBoxY1.SelLength & "," & ExDateTextBoxY1.SelectText)
	End Sub

End Class
