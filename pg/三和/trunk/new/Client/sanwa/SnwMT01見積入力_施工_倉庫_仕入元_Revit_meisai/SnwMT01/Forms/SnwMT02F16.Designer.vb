<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SnwMT02F16

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
	Public WithEvents CbNouki As System.Windows.Forms.Button
	Public WithEvents CbClose As System.Windows.Forms.Button
	Public WithEvents CbUpload As System.Windows.Forms.Button
	Public WithEvents sb_Msg_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents sb_Msg As System.Windows.Forms.StatusStrip
	Public WithEvents tx_Š|—¦ As ExNmText.ExNmTextBox
	Public WithEvents lb_€–Ú_0 As System.Windows.Forms.Label
	Public WithEvents lb_€–Ú_1 As System.Windows.Forms.Label
	Public WithEvents lb_€–Ú_2 As System.Windows.Forms.Label
	Public WithEvents rf_Œ©Ï”Ô† As System.Windows.Forms.Label
    'ƒƒ‚: ˆÈ‰º‚ÌƒvƒƒV[ƒWƒƒ‚Í Windows ƒtƒH[ƒ€ ƒfƒUƒCƒi‚Å•K—v‚Å‚·B
    'Windows ƒtƒH[ƒ€ ƒfƒUƒCƒi‚ğg‚Á‚Ä•ÏX‚Å‚«‚Ü‚·B
    'ƒR[ƒh ƒGƒfƒBƒ^‚ğg—p‚µ‚ÄA•ÏX‚µ‚È‚¢‚Å‚­‚¾‚³‚¢B
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SnwMT02F16))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.CbNouki = New System.Windows.Forms.Button()
        Me.CbClose = New System.Windows.Forms.Button()
        Me.CbUpload = New System.Windows.Forms.Button()
        Me.sb_Msg = New System.Windows.Forms.StatusStrip()
        Me.sb_Msg_Panel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tx_Š|—¦ = New ExNmText.ExNmTextBox()
        Me.lb_€–Ú_0 = New System.Windows.Forms.Label()
        Me.lb_€–Ú_1 = New System.Windows.Forms.Label()
        Me.lb_€–Ú_2 = New System.Windows.Forms.Label()
        Me.rf_Œ©Ï”Ô† = New System.Windows.Forms.Label()
        Me.FpSpd = New FarPoint.Win.Spread.FpSpread(FarPoint.Win.Spread.LegacyBehaviors.None, resources.GetObject("resource1"))
        Me.FpSpd_Sheet1 = Me.FpSpd.GetSheet(0)
        Me.sb_Msg.SuspendLayout()
        CType(Me.FpSpd, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'CbNouki
        '
        Me.CbNouki.BackColor = System.Drawing.SystemColors.Control
        Me.CbNouki.Cursor = System.Windows.Forms.Cursors.Default
        Me.CbNouki.Font = New System.Drawing.Font("‚l‚r ‚oƒSƒVƒbƒN", 9.0!)
        Me.CbNouki.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CbNouki.Location = New System.Drawing.Point(14, 410)
        Me.CbNouki.Name = "CbNouki"
        Me.CbNouki.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CbNouki.Size = New System.Drawing.Size(82, 24)
        Me.CbNouki.TabIndex = 9
        Me.CbNouki.Text = "ˆêŠ‡"
        Me.CbNouki.UseVisualStyleBackColor = False
        '
        'CbClose
        '
        Me.CbClose.BackColor = System.Drawing.SystemColors.Control
        Me.CbClose.Cursor = System.Windows.Forms.Cursors.Default
        Me.CbClose.DialogResult = System.Windows.Forms.DialogResult.Abort
        Me.CbClose.Font = New System.Drawing.Font("‚l‚r ‚oƒSƒVƒbƒN", 9.0!)
        Me.CbClose.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CbClose.Location = New System.Drawing.Point(523, 412)
        Me.CbClose.Name = "CbClose"
        Me.CbClose.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CbClose.Size = New System.Drawing.Size(82, 25)
        Me.CbClose.TabIndex = 2
        Me.CbClose.Text = "•Â‚¶‚é"
        Me.CbClose.UseVisualStyleBackColor = False
        '
        'CbUpload
        '
        Me.CbUpload.BackColor = System.Drawing.SystemColors.Control
        Me.CbUpload.Cursor = System.Windows.Forms.Cursors.Default
        Me.CbUpload.Font = New System.Drawing.Font("‚l‚r ‚oƒSƒVƒbƒN", 9.0!)
        Me.CbUpload.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CbUpload.Location = New System.Drawing.Point(427, 412)
        Me.CbUpload.Name = "CbUpload"
        Me.CbUpload.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CbUpload.Size = New System.Drawing.Size(82, 25)
        Me.CbUpload.TabIndex = 1
        Me.CbUpload.Text = "“o˜^(&S)"
        Me.CbUpload.UseVisualStyleBackColor = False
        '
        'sb_Msg
        '
        Me.sb_Msg.Font = New System.Drawing.Font("MS UI Gothic", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.sb_Msg.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.sb_Msg.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.sb_Msg_Panel1})
        Me.sb_Msg.Location = New System.Drawing.Point(0, 455)
        Me.sb_Msg.Name = "sb_Msg"
        Me.sb_Msg.Size = New System.Drawing.Size(696, 24)
        Me.sb_Msg.TabIndex = 3
        '
        'sb_Msg_Panel1
        '
        Me.sb_Msg_Panel1.AutoSize = False
        Me.sb_Msg_Panel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.sb_Msg_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me.sb_Msg_Panel1.Font = New System.Drawing.Font("MS UI Gothic", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.sb_Msg_Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me.sb_Msg_Panel1.Name = "sb_Msg_Panel1"
        Me.sb_Msg_Panel1.Size = New System.Drawing.Size(450, 24)
        Me.sb_Msg_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.sb_Msg_Panel1.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        '
        'tx_Š|—¦
        '
        Me.tx_Š|—¦.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_Š|—¦.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.Àü
        Me.tx_Š|—¦.CanForwardSetFocus = True
        Me.tx_Š|—¦.CanNextSetFocus = True
        Me.tx_Š|—¦.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_Š|—¦.DecimalPlace = CType(0, Short)
        Me.tx_Š|—¦.EditMode = True
        Me.tx_Š|—¦.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_Š|—¦.FormatType = ""
        Me.tx_Š|—¦.FormatTypeNega = ""
        Me.tx_Š|—¦.FormatTypeNull = ""
        Me.tx_Š|—¦.FormatTypeZero = ""
        Me.tx_Š|—¦.InputMinus = True
        Me.tx_Š|—¦.InputPlus = True
        Me.tx_Š|—¦.InputZero = True
        Me.tx_Š|—¦.Location = New System.Drawing.Point(123, 43)
        Me.tx_Š|—¦.MaxLength = 3
        Me.tx_Š|—¦.Name = "tx_Š|—¦"
        Me.tx_Š|—¦.OldValue = "888"
        Me.tx_Š|—¦.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_Š|—¦.SelectText = True
        Me.tx_Š|—¦.SelLength = 0
        Me.tx_Š|—¦.SelStart = 0
        Me.tx_Š|—¦.SelText = ""
        Me.tx_Š|—¦.Size = New System.Drawing.Size(40, 22)
        Me.tx_Š|—¦.TabIndex = 6
        Me.tx_Š|—¦.Text = "888"
        '
        'lb_€–Ú_0
        '
        Me.lb_€–Ú_0.BackColor = System.Drawing.Color.Transparent
        Me.lb_€–Ú_0.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_€–Ú_0.Font = New System.Drawing.Font("‚l‚r ‚oƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_€–Ú_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lb_€–Ú_0.Location = New System.Drawing.Point(28, 46)
        Me.lb_€–Ú_0.Name = "lb_€–Ú_0"
        Me.lb_€–Ú_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_€–Ú_0.Size = New System.Drawing.Size(87, 15)
        Me.lb_€–Ú_0.TabIndex = 8
        Me.lb_€–Ú_0.Text = "’è‰¿‚ÌŠ|—¦"
        '
        'lb_€–Ú_1
        '
        Me.lb_€–Ú_1.BackColor = System.Drawing.Color.Transparent
        Me.lb_€–Ú_1.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_€–Ú_1.Font = New System.Drawing.Font("‚l‚r ‚oƒSƒVƒbƒN", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_€–Ú_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_€–Ú_1.Location = New System.Drawing.Point(167, 45)
        Me.lb_€–Ú_1.Name = "lb_€–Ú_1"
        Me.lb_€–Ú_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_€–Ú_1.Size = New System.Drawing.Size(20, 17)
        Me.lb_€–Ú_1.TabIndex = 7
        Me.lb_€–Ú_1.Text = "“"
        Me.lb_€–Ú_1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_€–Ú_2
        '
        Me.lb_€–Ú_2.BackColor = System.Drawing.SystemColors.Control
        Me.lb_€–Ú_2.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_€–Ú_2.Font = New System.Drawing.Font("‚l‚r ‚oƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lb_€–Ú_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_€–Ú_2.Location = New System.Drawing.Point(28, 12)
        Me.lb_€–Ú_2.Name = "lb_€–Ú_2"
        Me.lb_€–Ú_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_€–Ú_2.Size = New System.Drawing.Size(77, 16)
        Me.lb_€–Ú_2.TabIndex = 5
        Me.lb_€–Ú_2.Text = "Œ©Ï‡‚"
        Me.lb_€–Ú_2.Visible = False
        '
        'rf_Œ©Ï”Ô†
        '
        Me.rf_Œ©Ï”Ô†.BackColor = System.Drawing.SystemColors.Control
        Me.rf_Œ©Ï”Ô†.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_Œ©Ï”Ô†.Font = New System.Drawing.Font("‚l‚r ‚oƒSƒVƒbƒN", 9.0!)
        Me.rf_Œ©Ï”Ô†.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_Œ©Ï”Ô†.Location = New System.Drawing.Point(120, 12)
        Me.rf_Œ©Ï”Ô†.Name = "rf_Œ©Ï”Ô†"
        Me.rf_Œ©Ï”Ô†.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_Œ©Ï”Ô†.Size = New System.Drawing.Size(67, 15)
        Me.rf_Œ©Ï”Ô†.TabIndex = 4
        Me.rf_Œ©Ï”Ô†.Text = "21755009"
        Me.rf_Œ©Ï”Ô†.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.rf_Œ©Ï”Ô†.Visible = False
        '
        'FpSpd
        '
        Me.FpSpd.AccessibleDescription = "FpSpd, Sheet1, Row 0, Column 0"
        Me.FpSpd.Font = New System.Drawing.Font("‚l‚r ‚oƒSƒVƒbƒN", 9.0!)
        Me.FpSpd.Location = New System.Drawing.Point(12, 43)
        Me.FpSpd.Name = "FpSpd"
        Me.FpSpd.Size = New System.Drawing.Size(660, 348)
        Me.FpSpd.TabIndex = 11
        '
        'SnwMT02F16
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.CbClose
        Me.ClientSize = New System.Drawing.Size(696, 479)
        Me.Controls.Add(Me.FpSpd)
        Me.Controls.Add(Me.CbNouki)
        Me.Controls.Add(Me.CbClose)
        Me.Controls.Add(Me.CbUpload)
        Me.Controls.Add(Me.sb_Msg)
        Me.Controls.Add(Me.tx_Š|—¦)
        Me.Controls.Add(Me.lb_€–Ú_0)
        Me.Controls.Add(Me.lb_€–Ú_1)
        Me.Controls.Add(Me.lb_€–Ú_2)
        Me.Controls.Add(Me.rf_Œ©Ï”Ô†)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "SnwMT02F16"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "“üoŒÉ“úİ’è"
        Me.sb_Msg.ResumeLayout(False)
        Me.sb_Msg.PerformLayout()
        CType(Me.FpSpd, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents FpSpd As FarPoint.Win.Spread.FpSpread
    Friend WithEvents FpSpd_Sheet1 As FarPoint.Win.Spread.SheetView
End Class