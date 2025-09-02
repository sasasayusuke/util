<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.ExNmTextBox1 = New ExNmText.ExNmTextBox()
        Me.ExDateTextBoxD1 = New ExDateText.ExDateTextBoxD()
        Me.ExDateTextBoxM1 = New ExDateText.ExDateTextBoxM()
        Me.ExDateTextBoxY1 = New ExDateText.ExDateTextBoxY()
        Me.ExTextBox1 = New ExText.ExTextBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(66, 59)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(51, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "ExText"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(66, 161)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(72, 15)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "ExNmText"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(66, 268)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(81, 15)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "ExDateText"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(465, 330)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(132, 39)
        Me.Button1.TabIndex = 8
        Me.Button1.Text = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'ExNmTextBox1
        '
        Me.ExNmTextBox1.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.ExNmTextBox1.BackColor = System.Drawing.SystemColors.Window
        Me.ExNmTextBox1.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.ExNmTextBox1.CanForwardSetFocus = True
        Me.ExNmTextBox1.CanNextSetFocus = True
        Me.ExNmTextBox1.DecimalPlace = CType(0, Short)
        Me.ExNmTextBox1.EditMode = False
        Me.ExNmTextBox1.FocusBackColor = System.Drawing.SystemColors.Window
        Me.ExNmTextBox1.FormatType = "#,###"
        Me.ExNmTextBox1.FormatTypeNega = "#,###"
        Me.ExNmTextBox1.FormatTypeNull = "#"
        Me.ExNmTextBox1.FormatTypeZero = "#"
        Me.ExNmTextBox1.InputMinus = True
        Me.ExNmTextBox1.InputPlus = True
        Me.ExNmTextBox1.InputZero = False
        Me.ExNmTextBox1.Location = New System.Drawing.Point(198, 160)
        Me.ExNmTextBox1.MaxLength = 12
        Me.ExNmTextBox1.Name = "ExNmTextBox1"
        Me.ExNmTextBox1.OldValue = "1,234,567"
        Me.ExNmTextBox1.SelectText = True
        Me.ExNmTextBox1.SelLength = 0
        Me.ExNmTextBox1.SelStart = 0
        Me.ExNmTextBox1.SelText = ""
        Me.ExNmTextBox1.Size = New System.Drawing.Size(201, 22)
        Me.ExNmTextBox1.TabIndex = 4
        Me.ExNmTextBox1.Text = "1,234,567"
        '
        'ExDateTextBoxD1
        '
        Me.ExDateTextBoxD1.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.ExDateTextBoxD1.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.実線
        Me.ExDateTextBoxD1.CanForwardSetFocus = True
        Me.ExDateTextBoxD1.CanNextSetFocus = True
        Me.ExDateTextBoxD1.EditMode = True
        Me.ExDateTextBoxD1.FocusBackColor = System.Drawing.SystemColors.Window
        Me.ExDateTextBoxD1.Location = New System.Drawing.Point(337, 264)
        Me.ExDateTextBoxD1.MaxLength = 2
        Me.ExDateTextBoxD1.Name = "ExDateTextBoxD1"
        Me.ExDateTextBoxD1.OldValue = "31"
        Me.ExDateTextBoxD1.SelectText = True
        Me.ExDateTextBoxD1.SelLength = 0
        Me.ExDateTextBoxD1.SelStart = 0
        Me.ExDateTextBoxD1.SelText = ""
        Me.ExDateTextBoxD1.Size = New System.Drawing.Size(45, 22)
        Me.ExDateTextBoxD1.TabIndex = 7
        Me.ExDateTextBoxD1.Text = "31"
        '
        'ExDateTextBoxM1
        '
        Me.ExDateTextBoxM1.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.ExDateTextBoxM1.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.実線
        Me.ExDateTextBoxM1.CanForwardSetFocus = True
        Me.ExDateTextBoxM1.CanNextSetFocus = True
        Me.ExDateTextBoxM1.EditMode = True
        Me.ExDateTextBoxM1.FocusBackColor = System.Drawing.SystemColors.Window
        Me.ExDateTextBoxM1.Location = New System.Drawing.Point(268, 264)
        Me.ExDateTextBoxM1.MaxLength = 2
        Me.ExDateTextBoxM1.Name = "ExDateTextBoxM1"
        Me.ExDateTextBoxM1.OldValue = "12"
        Me.ExDateTextBoxM1.SelectText = True
        Me.ExDateTextBoxM1.SelLength = 0
        Me.ExDateTextBoxM1.SelStart = 0
        Me.ExDateTextBoxM1.SelText = ""
        Me.ExDateTextBoxM1.Size = New System.Drawing.Size(45, 22)
        Me.ExDateTextBoxM1.TabIndex = 6
        Me.ExDateTextBoxM1.Text = "12"
        '
        'ExDateTextBoxY1
        '
        Me.ExDateTextBoxY1.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.ExDateTextBoxY1.BackColor = System.Drawing.SystemColors.Window
        Me.ExDateTextBoxY1.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.実線
        Me.ExDateTextBoxY1.CanForwardSetFocus = True
        Me.ExDateTextBoxY1.CanNextSetFocus = True
        Me.ExDateTextBoxY1.EditMode = True
        Me.ExDateTextBoxY1.FocusBackColor = System.Drawing.SystemColors.Window
        Me.ExDateTextBoxY1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ExDateTextBoxY1.Location = New System.Drawing.Point(200, 264)
        Me.ExDateTextBoxY1.MaxLength = 4
        Me.ExDateTextBoxY1.Name = "ExDateTextBoxY1"
        Me.ExDateTextBoxY1.OldValue = "2025"
        Me.ExDateTextBoxY1.SelectText = True
        Me.ExDateTextBoxY1.SelLength = 0
        Me.ExDateTextBoxY1.SelStart = 0
        Me.ExDateTextBoxY1.SelText = ""
        Me.ExDateTextBoxY1.Size = New System.Drawing.Size(45, 22)
        Me.ExDateTextBoxY1.TabIndex = 5
        Me.ExDateTextBoxY1.Text = "2025"
        '
        'ExTextBox1
        '
        Me.ExTextBox1.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.ExTextBox1.BackColor = System.Drawing.SystemColors.Window
        Me.ExTextBox1.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.ExTextBox1.CanForwardSetFocus = True
        Me.ExTextBox1.CanNextSetFocus = True
        Me.ExTextBox1.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.ExTextBox1.EditMode = False
        Me.ExTextBox1.FocusBackColor = System.Drawing.Color.Empty
        Me.ExTextBox1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ExTextBox1.IMEMode = System.Windows.Forms.ImeMode.NoControl
        Me.ExTextBox1.IMEMode = System.Windows.Forms.ImeMode.NoControl
        Me.ExTextBox1.LengthType = ExText.ExTextBox.LenType.ByteType
        Me.ExTextBox1.Location = New System.Drawing.Point(200, 59)
        Me.ExTextBox1.MaxLength = 12
        Me.ExTextBox1.MousePointer = System.Windows.Forms.Cursors.Default
        Me.ExTextBox1.Name = "ExTextBox1"
        Me.ExTextBox1.OldValue = "AAA"
        Me.ExTextBox1.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.ExTextBox1.SelectText = True
        Me.ExTextBox1.SelLength = 0
        Me.ExTextBox1.SelStart = 0
        Me.ExTextBox1.SelText = ""
        Me.ExTextBox1.Size = New System.Drawing.Size(190, 22)
        Me.ExTextBox1.TabIndex = 3
        Me.ExTextBox1.Text = "AAA"
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(465, 398)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(132, 39)
        Me.Button2.TabIndex = 9
        Me.Button2.Text = "Button2"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(681, 481)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.ExDateTextBoxD1)
        Me.Controls.Add(Me.ExDateTextBoxM1)
        Me.Controls.Add(Me.ExDateTextBoxY1)
        Me.Controls.Add(Me.ExNmTextBox1)
        Me.Controls.Add(Me.ExTextBox1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents ExTextBox1 As ExText.ExTextBox
    Friend WithEvents ExNmTextBox1 As ExNmText.ExNmTextBox
    Friend WithEvents ExDateTextBoxY1 As ExDateText.ExDateTextBoxY
    Friend WithEvents ExDateTextBoxM1 As ExDateText.ExDateTextBoxM
    Friend WithEvents ExDateTextBoxD1 As ExDateText.ExDateTextBoxD
    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
End Class
