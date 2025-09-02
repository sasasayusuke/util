<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SnwMT02F07

	'Form ‚ÍAƒRƒ“ƒ|[ƒlƒ“ƒgˆê——‚ÉŒãˆ—‚ğÀs‚·‚é‚½‚ß‚É dispose ‚ğƒI[ƒo[ƒ‰ƒCƒh‚µ‚Ü‚·B
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

	'Windows ƒtƒH[ƒ€ ƒfƒUƒCƒi‚Å•K—v‚Å‚·B
	Private components As System.ComponentModel.IContainer

	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents CbClose As System.Windows.Forms.Button
	Public WithEvents CbCopy As System.Windows.Forms.Button
	Public WithEvents sb_Msg_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents sb_Msg As System.Windows.Forms.StatusStrip
	Public WithEvents tx_“¾ˆÓæCD As ExText.ExTextBox
	Public WithEvents rf_“¾ˆÓæ–¼1 As System.Windows.Forms.Label
	Public WithEvents rf_“¾ˆÓæ–¼2 As System.Windows.Forms.Label
    Public WithEvents lb_€–Ú_3 As System.Windows.Forms.Label
    Public WithEvents rf_Œ©ÏŒ–¼ As System.Windows.Forms.Label
    Public WithEvents lb_€–Ú_1 As System.Windows.Forms.Label
	Public WithEvents lb_€–Ú_2 As System.Windows.Forms.Label
	Public WithEvents rf_Œ©Ï”Ô† As System.Windows.Forms.Label
    Public WithEvents lb_€–Ú_0 As System.Windows.Forms.Label
    'ƒƒ‚: ˆÈ‰º‚ÌƒvƒƒV[ƒWƒƒ‚Í Windows ƒtƒH[ƒ€ ƒfƒUƒCƒi‚Å•K—v‚Å‚·B
    'Windows ƒtƒH[ƒ€ ƒfƒUƒCƒi‚ğg‚Á‚Ä•ÏX‚Å‚«‚Ü‚·B
    'ƒR[ƒh ƒGƒfƒBƒ^‚ğg—p‚µ‚ÄA•ÏX‚µ‚È‚¢‚Å‚­‚¾‚³‚¢B
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SnwMT02F07))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.CbClose = New System.Windows.Forms.Button()
        Me.CbCopy = New System.Windows.Forms.Button()
        Me.sb_Msg = New System.Windows.Forms.StatusStrip()
        Me.sb_Msg_Panel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tx_“¾ˆÓæCD = New ExText.ExTextBox()
        Me.rf_“¾ˆÓæ–¼1 = New System.Windows.Forms.Label()
        Me.rf_“¾ˆÓæ–¼2 = New System.Windows.Forms.Label()
        Me.lb_€–Ú_3 = New System.Windows.Forms.Label()
        Me.rf_Œ©ÏŒ–¼ = New System.Windows.Forms.Label()
        Me.lb_€–Ú_1 = New System.Windows.Forms.Label()
        Me.lb_€–Ú_2 = New System.Windows.Forms.Label()
        Me.rf_Œ©Ï”Ô† = New System.Windows.Forms.Label()
        Me.lb_€–Ú_0 = New System.Windows.Forms.Label()
        Me.sb_Msg.SuspendLayout()
        Me.SuspendLayout()
        '
        'CbClose
        '
        Me.CbClose.BackColor = System.Drawing.SystemColors.Control
        Me.CbClose.Cursor = System.Windows.Forms.Cursors.Default
        Me.CbClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CbClose.Font = New System.Drawing.Font("‚l‚r ‚oƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CbClose.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CbClose.Location = New System.Drawing.Point(380, 182)
        Me.CbClose.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.CbClose.Name = "CbClose"
        Me.CbClose.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CbClose.Size = New System.Drawing.Size(79, 25)
        Me.CbClose.TabIndex = 2
        Me.CbClose.Text = "•Â‚¶‚é"
        Me.CbClose.UseVisualStyleBackColor = False
        '
        'CbCopy
        '
        Me.CbCopy.BackColor = System.Drawing.SystemColors.Control
        Me.CbCopy.Cursor = System.Windows.Forms.Cursors.Default
        Me.CbCopy.Font = New System.Drawing.Font("‚l‚r ‚oƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CbCopy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CbCopy.Location = New System.Drawing.Point(284, 182)
        Me.CbCopy.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.CbCopy.Name = "CbCopy"
        Me.CbCopy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CbCopy.Size = New System.Drawing.Size(84, 25)
        Me.CbCopy.TabIndex = 1
        Me.CbCopy.Text = "ƒRƒs[(&C)"
        Me.CbCopy.UseVisualStyleBackColor = False
        '
        'sb_Msg
        '
        Me.sb_Msg.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.sb_Msg.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.sb_Msg_Panel1})
        Me.sb_Msg.Location = New System.Drawing.Point(0, 234)
        Me.sb_Msg.Name = "sb_Msg"
        Me.sb_Msg.Padding = New System.Windows.Forms.Padding(1, 0, 13, 0)
        Me.sb_Msg.Size = New System.Drawing.Size(511, 22)
        Me.sb_Msg.TabIndex = 4
        '
        'sb_Msg_Panel1
        '
        Me.sb_Msg_Panel1.AutoSize = False
        Me.sb_Msg_Panel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.sb_Msg_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me.sb_Msg_Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me.sb_Msg_Panel1.Name = "sb_Msg_Panel1"
        Me.sb_Msg_Panel1.Size = New System.Drawing.Size(497, 22)
        Me.sb_Msg_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tx_“¾ˆÓæCD
        '
        Me.tx_“¾ˆÓæCD.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_“¾ˆÓæCD.BorderStyle = ExText.ExTextBox.BorderStyleType.Àü
        Me.tx_“¾ˆÓæCD.CanForwardSetFocus = True
        Me.tx_“¾ˆÓæCD.CanNextSetFocus = True
        Me.tx_“¾ˆÓæCD.CharacterSize = ExText.ExTextBox.CharSize.‚È‚µ
        Me.tx_“¾ˆÓæCD.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_“¾ˆÓæCD.EditMode = True
        Me.tx_“¾ˆÓæCD.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_“¾ˆÓæCD.IMEMode = System.Windows.Forms.ImeMode.NoControl
        Me.tx_“¾ˆÓæCD.LengthType = ExText.ExTextBox.LenType.ByteType
        Me.tx_“¾ˆÓæCD.Location = New System.Drawing.Point(117, 124)
        Me.tx_“¾ˆÓæCD.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.tx_“¾ˆÓæCD.MaxLength = 4
        Me.tx_“¾ˆÓæCD.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_“¾ˆÓæCD.Name = "tx_“¾ˆÓæCD"
        Me.tx_“¾ˆÓæCD.OldValue = "WWWW"
        Me.tx_“¾ˆÓæCD.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_“¾ˆÓæCD.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_“¾ˆÓæCD.SelectText = True
        Me.tx_“¾ˆÓæCD.SelLength = 0
        Me.tx_“¾ˆÓæCD.SelStart = 0
        Me.tx_“¾ˆÓæCD.SelText = ""
        Me.tx_“¾ˆÓæCD.Size = New System.Drawing.Size(41, 22)
        Me.tx_“¾ˆÓæCD.TabIndex = 0
        Me.tx_“¾ˆÓæCD.Text = "WWWW"
        '
        'rf_“¾ˆÓæ–¼1
        '
        Me.rf_“¾ˆÓæ–¼1.BackColor = System.Drawing.Color.Transparent
        Me.rf_“¾ˆÓæ–¼1.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_“¾ˆÓæ–¼1.Font = New System.Drawing.Font("‚l‚r ƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_“¾ˆÓæ–¼1.ForeColor = System.Drawing.Color.Black
        Me.rf_“¾ˆÓæ–¼1.Location = New System.Drawing.Point(176, 112)
        Me.rf_“¾ˆÓæ–¼1.Name = "rf_“¾ˆÓæ–¼1"
        Me.rf_“¾ˆÓæ–¼1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_“¾ˆÓæ–¼1.Size = New System.Drawing.Size(272, 20)
        Me.rf_“¾ˆÓæ–¼1.TabIndex = 11
        Me.rf_“¾ˆÓæ–¼1.Text = "‚P‚Q‚R‚S‚T‚U‚V‚W‚X‚O‚P‚Q‚R‚S"
        '
        'rf_“¾ˆÓæ–¼2
        '
        Me.rf_“¾ˆÓæ–¼2.BackColor = System.Drawing.Color.Transparent
        Me.rf_“¾ˆÓæ–¼2.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_“¾ˆÓæ–¼2.Font = New System.Drawing.Font("‚l‚r ƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_“¾ˆÓæ–¼2.ForeColor = System.Drawing.Color.Black
        Me.rf_“¾ˆÓæ–¼2.Location = New System.Drawing.Point(176, 131)
        Me.rf_“¾ˆÓæ–¼2.Name = "rf_“¾ˆÓæ–¼2"
        Me.rf_“¾ˆÓæ–¼2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_“¾ˆÓæ–¼2.Size = New System.Drawing.Size(272, 20)
        Me.rf_“¾ˆÓæ–¼2.TabIndex = 10
        Me.rf_“¾ˆÓæ–¼2.Text = "‚P‚Q‚R‚S‚T‚U‚V‚W‚X‚O‚P‚Q‚R‚S"
        '
        'lb_€–Ú_3
        '
        Me.lb_€–Ú_3.BackColor = System.Drawing.SystemColors.Control
        Me.lb_€–Ú_3.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_€–Ú_3.Font = New System.Drawing.Font("‚l‚r ‚oƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_€–Ú_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_€–Ú_3.Location = New System.Drawing.Point(13, 128)
        Me.lb_€–Ú_3.Name = "lb_€–Ú_3"
        Me.lb_€–Ú_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_€–Ú_3.Size = New System.Drawing.Size(93, 14)
        Me.lb_€–Ú_3.TabIndex = 9
        Me.lb_€–Ú_3.Text = "“¾ˆÓæw’è"
        '
        'rf_Œ©ÏŒ–¼
        '
        Me.rf_Œ©ÏŒ–¼.BackColor = System.Drawing.SystemColors.Control
        Me.rf_Œ©ÏŒ–¼.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_Œ©ÏŒ–¼.Font = New System.Drawing.Font("‚l‚r ‚oƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_Œ©ÏŒ–¼.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_Œ©ÏŒ–¼.Location = New System.Drawing.Point(115, 84)
        Me.rf_Œ©ÏŒ–¼.Name = "rf_Œ©ÏŒ–¼"
        Me.rf_Œ©ÏŒ–¼.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_Œ©ÏŒ–¼.Size = New System.Drawing.Size(333, 15)
        Me.rf_Œ©ÏŒ–¼.TabIndex = 8
        Me.rf_Œ©ÏŒ–¼.Text = "‚P‚Q‚R‚S‚T‚U‚V‚W‚X‚O‚P‚Q‚R‚S‚T‚U‚V‚W‚X‚O‚P‚Q‚R‚S‚T‚U‚V‚W‚X‚O"
        '
        'lb_€–Ú_1
        '
        Me.lb_€–Ú_1.BackColor = System.Drawing.SystemColors.Control
        Me.lb_€–Ú_1.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_€–Ú_1.Font = New System.Drawing.Font("‚l‚r ‚oƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_€–Ú_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_€–Ú_1.Location = New System.Drawing.Point(15, 85)
        Me.lb_€–Ú_1.Name = "lb_€–Ú_1"
        Me.lb_€–Ú_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_€–Ú_1.Size = New System.Drawing.Size(83, 14)
        Me.lb_€–Ú_1.TabIndex = 7
        Me.lb_€–Ú_1.Text = "Œ©ÏŒ–¼"
        '
        'lb_€–Ú_2
        '
        Me.lb_€–Ú_2.BackColor = System.Drawing.SystemColors.Control
        Me.lb_€–Ú_2.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_€–Ú_2.Font = New System.Drawing.Font("‚l‚r ‚oƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_€–Ú_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_€–Ú_2.Location = New System.Drawing.Point(16, 48)
        Me.lb_€–Ú_2.Name = "lb_€–Ú_2"
        Me.lb_€–Ú_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_€–Ú_2.Size = New System.Drawing.Size(59, 15)
        Me.lb_€–Ú_2.TabIndex = 6
        Me.lb_€–Ú_2.Text = "Œ©Ï‡‚"
        '
        'rf_Œ©Ï”Ô†
        '
        Me.rf_Œ©Ï”Ô†.BackColor = System.Drawing.SystemColors.Control
        Me.rf_Œ©Ï”Ô†.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_Œ©Ï”Ô†.Font = New System.Drawing.Font("‚l‚r ‚oƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_Œ©Ï”Ô†.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_Œ©Ï”Ô†.Location = New System.Drawing.Point(111, 48)
        Me.rf_Œ©Ï”Ô†.Name = "rf_Œ©Ï”Ô†"
        Me.rf_Œ©Ï”Ô†.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_Œ©Ï”Ô†.Size = New System.Drawing.Size(67, 16)
        Me.rf_Œ©Ï”Ô†.TabIndex = 5
        Me.rf_Œ©Ï”Ô†.Text = "21755009"
        Me.rf_Œ©Ï”Ô†.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lb_€–Ú_0
        '
        Me.lb_€–Ú_0.BackColor = System.Drawing.SystemColors.Control
        Me.lb_€–Ú_0.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_€–Ú_0.Font = New System.Drawing.Font("‚l‚r ‚oƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_€–Ú_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lb_€–Ú_0.Location = New System.Drawing.Point(16, 16)
        Me.lb_€–Ú_0.Name = "lb_€–Ú_0"
        Me.lb_€–Ú_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_€–Ú_0.Size = New System.Drawing.Size(60, 14)
        Me.lb_€–Ú_0.TabIndex = 3
        Me.lb_€–Ú_0.Text = "ƒRƒs[Œ³"
        '
        'SnwMT02F07
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.CbClose
        Me.ClientSize = New System.Drawing.Size(511, 256)
        Me.Controls.Add(Me.CbClose)
        Me.Controls.Add(Me.CbCopy)
        Me.Controls.Add(Me.sb_Msg)
        Me.Controls.Add(Me.tx_“¾ˆÓæCD)
        Me.Controls.Add(Me.rf_“¾ˆÓæ–¼1)
        Me.Controls.Add(Me.rf_“¾ˆÓæ–¼2)
        Me.Controls.Add(Me.lb_€–Ú_3)
        Me.Controls.Add(Me.rf_Œ©ÏŒ–¼)
        Me.Controls.Add(Me.lb_€–Ú_1)
        Me.Controls.Add(Me.lb_€–Ú_2)
        Me.Controls.Add(Me.rf_Œ©Ï”Ô†)
        Me.Controls.Add(Me.lb_€–Ú_0)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(3, 22)
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SnwMT02F07"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Œ©ÏƒRƒs["
        Me.sb_Msg.ResumeLayout(False)
        Me.sb_Msg.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

End Class