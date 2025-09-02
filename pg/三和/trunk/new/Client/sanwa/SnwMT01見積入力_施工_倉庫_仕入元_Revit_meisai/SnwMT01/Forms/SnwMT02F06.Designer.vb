<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SnwMT02F06

	'Form は、コンポーネント一覧に後処理を実行するために dispose をオーバーライドします。
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

	'Windows フォーム デザイナで必要です。
	Private components As System.ComponentModel.IContainer

	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents CmdChkOff As System.Windows.Forms.Button
	Public WithEvents CmdChkOn As System.Windows.Forms.Button
	Public WithEvents tx_Dummy3 As System.Windows.Forms.TextBox
	Public WithEvents tx_Dummy2 As System.Windows.Forms.TextBox
	Public WithEvents tx_Dummy1 As System.Windows.Forms.TextBox
	Public WithEvents cbTabEnd As System.Windows.Forms.Button
	Public WithEvents PicFunction As System.Windows.Forms.Panel
	Public WithEvents sb_Msg_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents sb_Msg_Panel2 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents sb_Msg_Panel3 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents sb_Msg As System.Windows.Forms.StatusStrip
	Public WithEvents Tx_テンプレート名 As ExText.ExTextBox
	Public WithEvents rf_得意先CD As System.Windows.Forms.Label
	Public WithEvents rf_得意先名 As System.Windows.Forms.Label
	Public WithEvents lb_項目_4 As System.Windows.Forms.Label
	Public WithEvents lb_項目_0 As System.Windows.Forms.Label
    Public WithEvents lb_cnt As System.Windows.Forms.Label
    Public WithEvents lb_項目_28 As System.Windows.Forms.Label
    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使って変更できます。
    'コード エディタを使用して、変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SnwMT02F06))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.CmdChkOff = New System.Windows.Forms.Button()
        Me.CmdChkOn = New System.Windows.Forms.Button()
        Me.tx_Dummy3 = New System.Windows.Forms.TextBox()
        Me.tx_Dummy2 = New System.Windows.Forms.TextBox()
        Me.tx_Dummy1 = New System.Windows.Forms.TextBox()
        Me.PicFunction = New System.Windows.Forms.Panel()
        Me.cbTabEnd = New System.Windows.Forms.Button()
        Me.sb_Msg = New System.Windows.Forms.StatusStrip()
        Me.sb_Msg_Panel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.sb_Msg_Panel2 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.sb_Msg_Panel3 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.Tx_テンプレート名 = New ExText.ExTextBox()
        Me.rf_得意先CD = New System.Windows.Forms.Label()
        Me.rf_得意先名 = New System.Windows.Forms.Label()
        Me.lb_項目_4 = New System.Windows.Forms.Label()
        Me.lb_項目_0 = New System.Windows.Forms.Label()
        Me.lb_cnt = New System.Windows.Forms.Label()
        Me.lb_項目_28 = New System.Windows.Forms.Label()
        Me.FpSpd = New FarPoint.Win.Spread.FpSpread(FarPoint.Win.Spread.LegacyBehaviors.None, resources.GetObject("resource1"))
        Me.FpSpd_Sheet1 = Me.FpSpd.GetSheet(0)
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.PicFunction.SuspendLayout()
        Me.sb_Msg.SuspendLayout()
        CType(Me.FpSpd, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'CmdChkOff
        '
        Me.CmdChkOff.BackColor = System.Drawing.SystemColors.Control
        Me.CmdChkOff.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmdChkOff.Enabled = False
        Me.CmdChkOff.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CmdChkOff.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdChkOff.Location = New System.Drawing.Point(695, 38)
        Me.CmdChkOff.Name = "CmdChkOff"
        Me.CmdChkOff.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdChkOff.Size = New System.Drawing.Size(83, 23)
        Me.CmdChkOff.TabIndex = 39
        Me.CmdChkOff.Text = "ALL解除"
        Me.CmdChkOff.UseVisualStyleBackColor = False
        '
        'CmdChkOn
        '
        Me.CmdChkOn.BackColor = System.Drawing.SystemColors.Control
        Me.CmdChkOn.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmdChkOn.Enabled = False
        Me.CmdChkOn.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CmdChkOn.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdChkOn.Location = New System.Drawing.Point(612, 38)
        Me.CmdChkOn.Name = "CmdChkOn"
        Me.CmdChkOn.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdChkOn.Size = New System.Drawing.Size(83, 23)
        Me.CmdChkOn.TabIndex = 38
        Me.CmdChkOn.Text = "ALL選択"
        Me.CmdChkOn.UseVisualStyleBackColor = False
        '
        'tx_Dummy3
        '
        Me.tx_Dummy3.AcceptsReturn = True
        Me.tx_Dummy3.BackColor = System.Drawing.SystemColors.Window
        Me.tx_Dummy3.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_Dummy3.Enabled = False
        Me.tx_Dummy3.ForeColor = System.Drawing.SystemColors.WindowText
        Me.tx_Dummy3.Location = New System.Drawing.Point(866, 46)
        Me.tx_Dummy3.MaxLength = 0
        Me.tx_Dummy3.Name = "tx_Dummy3"
        Me.tx_Dummy3.ReadOnly = True
        Me.tx_Dummy3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_Dummy3.Size = New System.Drawing.Size(42, 22)
        Me.tx_Dummy3.TabIndex = 35
        Me.tx_Dummy3.Text = "Dummy3"
        Me.tx_Dummy3.Visible = False
        '
        'tx_Dummy2
        '
        Me.tx_Dummy2.AcceptsReturn = True
        Me.tx_Dummy2.BackColor = System.Drawing.SystemColors.Window
        Me.tx_Dummy2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_Dummy2.Enabled = False
        Me.tx_Dummy2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.tx_Dummy2.Location = New System.Drawing.Point(866, 25)
        Me.tx_Dummy2.MaxLength = 0
        Me.tx_Dummy2.Name = "tx_Dummy2"
        Me.tx_Dummy2.ReadOnly = True
        Me.tx_Dummy2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_Dummy2.Size = New System.Drawing.Size(42, 22)
        Me.tx_Dummy2.TabIndex = 34
        Me.tx_Dummy2.Text = "Dummy2"
        Me.tx_Dummy2.Visible = False
        '
        'tx_Dummy1
        '
        Me.tx_Dummy1.AcceptsReturn = True
        Me.tx_Dummy1.BackColor = System.Drawing.SystemColors.Window
        Me.tx_Dummy1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_Dummy1.Enabled = False
        Me.tx_Dummy1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.tx_Dummy1.Location = New System.Drawing.Point(866, 4)
        Me.tx_Dummy1.MaxLength = 0
        Me.tx_Dummy1.Name = "tx_Dummy1"
        Me.tx_Dummy1.ReadOnly = True
        Me.tx_Dummy1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_Dummy1.Size = New System.Drawing.Size(42, 22)
        Me.tx_Dummy1.TabIndex = 33
        Me.tx_Dummy1.Text = "Dummy1"
        Me.tx_Dummy1.Visible = False
        '
        'PicFunction
        '
        Me.PicFunction.BackColor = System.Drawing.SystemColors.Control
        Me.PicFunction.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PicFunction.Controls.Add(Me.cbTabEnd)
        Me.PicFunction.Cursor = System.Windows.Forms.Cursors.Default
        Me.PicFunction.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PicFunction.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.PicFunction.ForeColor = System.Drawing.SystemColors.ControlText
        Me.PicFunction.Location = New System.Drawing.Point(0, 553)
        Me.PicFunction.Name = "PicFunction"
        Me.PicFunction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.PicFunction.Size = New System.Drawing.Size(1524, 39)
        Me.PicFunction.TabIndex = 13
        '
        'cbTabEnd
        '
        Me.cbTabEnd.BackColor = System.Drawing.SystemColors.Control
        Me.cbTabEnd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cbTabEnd.Enabled = False
        Me.cbTabEnd.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cbTabEnd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cbTabEnd.Location = New System.Drawing.Point(864, 16)
        Me.cbTabEnd.Name = "cbTabEnd"
        Me.cbTabEnd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cbTabEnd.Size = New System.Drawing.Size(41, 9)
        Me.cbTabEnd.TabIndex = 2
        Me.cbTabEnd.UseVisualStyleBackColor = False
        Me.cbTabEnd.Visible = False
        '
        'sb_Msg
        '
        Me.sb_Msg.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.sb_Msg.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.sb_Msg.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.sb_Msg_Panel1, Me.sb_Msg_Panel2, Me.sb_Msg_Panel3})
        Me.sb_Msg.Location = New System.Drawing.Point(0, 592)
        Me.sb_Msg.Name = "sb_Msg"
        Me.sb_Msg.Size = New System.Drawing.Size(1524, 22)
        Me.sb_Msg.TabIndex = 24
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
        Me.sb_Msg_Panel1.Size = New System.Drawing.Size(1364, 22)
        Me.sb_Msg_Panel1.Spring = True
        Me.sb_Msg_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'sb_Msg_Panel2
        '
        Me.sb_Msg_Panel2.AutoSize = False
        Me.sb_Msg_Panel2.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.sb_Msg_Panel2.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me.sb_Msg_Panel2.Margin = New System.Windows.Forms.Padding(0)
        Me.sb_Msg_Panel2.Name = "sb_Msg_Panel2"
        Me.sb_Msg_Panel2.Size = New System.Drawing.Size(96, 22)
        Me.sb_Msg_Panel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.sb_Msg_Panel2.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        '
        'sb_Msg_Panel3
        '
        Me.sb_Msg_Panel3.AutoSize = False
        Me.sb_Msg_Panel3.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.sb_Msg_Panel3.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me.sb_Msg_Panel3.Margin = New System.Windows.Forms.Padding(0)
        Me.sb_Msg_Panel3.Name = "sb_Msg_Panel3"
        Me.sb_Msg_Panel3.Size = New System.Drawing.Size(49, 22)
        Me.sb_Msg_Panel3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.sb_Msg_Panel3.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        '
        'Tx_テンプレート名
        '
        Me.Tx_テンプレート名.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.Tx_テンプレート名.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.Tx_テンプレート名.CanForwardSetFocus = True
        Me.Tx_テンプレート名.CanNextSetFocus = True
        Me.Tx_テンプレート名.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.Tx_テンプレート名.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Tx_テンプレート名.EditMode = True
        Me.Tx_テンプレート名.FocusBackColor = System.Drawing.SystemColors.Window
        Me.Tx_テンプレート名.IMEMode = System.Windows.Forms.ImeMode.Hiragana
        Me.Tx_テンプレート名.LengthType = ExText.ExTextBox.LenType.ByteType
        Me.Tx_テンプレート名.Location = New System.Drawing.Point(152, 38)
        Me.Tx_テンプレート名.MaxLength = 40
        Me.Tx_テンプレート名.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.Tx_テンプレート名.Name = "Tx_テンプレート名"
        Me.Tx_テンプレート名.OldValue = "1234567890123456789012345678901234567890"
        Me.Tx_テンプレート名.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.Tx_テンプレート名.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Tx_テンプレート名.SelectText = True
        Me.Tx_テンプレート名.SelLength = 0
        Me.Tx_テンプレート名.SelStart = 0
        Me.Tx_テンプレート名.SelText = ""
        Me.Tx_テンプレート名.Size = New System.Drawing.Size(320, 22)
        Me.Tx_テンプレート名.TabIndex = 0
        Me.Tx_テンプレート名.Text = "1234567890123456789012345678901234567890"
        '
        'rf_得意先CD
        '
        Me.rf_得意先CD.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.rf_得意先CD.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.rf_得意先CD.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_得意先CD.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_得意先CD.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_得意先CD.Location = New System.Drawing.Point(152, 19)
        Me.rf_得意先CD.Name = "rf_得意先CD"
        Me.rf_得意先CD.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_得意先CD.Size = New System.Drawing.Size(52, 19)
        Me.rf_得意先CD.TabIndex = 37
        Me.rf_得意先CD.Text = "1234"
        '
        'rf_得意先名
        '
        Me.rf_得意先名.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.rf_得意先名.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.rf_得意先名.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_得意先名.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_得意先名.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_得意先名.Location = New System.Drawing.Point(204, 19)
        Me.rf_得意先名.Name = "rf_得意先名"
        Me.rf_得意先名.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_得意先名.Size = New System.Drawing.Size(384, 19)
        Me.rf_得意先名.TabIndex = 36
        Me.rf_得意先名.Text = "１２３４５６７８９０１２３４ １２３４５６７８９０１２３４"
        '
        'lb_項目_4
        '
        Me.lb_項目_4.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.lb_項目_4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lb_項目_4.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_項目_4.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_項目_4.ForeColor = System.Drawing.Color.White
        Me.lb_項目_4.Location = New System.Drawing.Point(35, 39)
        Me.lb_項目_4.Name = "lb_項目_4"
        Me.lb_項目_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_項目_4.Size = New System.Drawing.Size(117, 20)
        Me.lb_項目_4.TabIndex = 32
        Me.lb_項目_4.Text = "テンプレート名"
        Me.lb_項目_4.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_項目_0
        '
        Me.lb_項目_0.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.lb_項目_0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lb_項目_0.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_項目_0.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_項目_0.ForeColor = System.Drawing.Color.White
        Me.lb_項目_0.Location = New System.Drawing.Point(35, 19)
        Me.lb_項目_0.Name = "lb_項目_0"
        Me.lb_項目_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_項目_0.Size = New System.Drawing.Size(117, 20)
        Me.lb_項目_0.TabIndex = 31
        Me.lb_項目_0.Text = "得意先CD"
        Me.lb_項目_0.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_cnt
        '
        Me.lb_cnt.BackColor = System.Drawing.SystemColors.Control
        Me.lb_cnt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lb_cnt.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_cnt.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_cnt.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_cnt.Location = New System.Drawing.Point(918, 46)
        Me.lb_cnt.Name = "lb_cnt"
        Me.lb_cnt.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_cnt.Size = New System.Drawing.Size(67, 19)
        Me.lb_cnt.TabIndex = 26
        Me.lb_cnt.Text = "1000"
        Me.lb_cnt.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.lb_cnt.Visible = False
        '
        'lb_項目_28
        '
        Me.lb_項目_28.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.lb_項目_28.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lb_項目_28.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_項目_28.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_項目_28.ForeColor = System.Drawing.Color.White
        Me.lb_項目_28.Location = New System.Drawing.Point(918, 27)
        Me.lb_項目_28.Name = "lb_項目_28"
        Me.lb_項目_28.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_項目_28.Size = New System.Drawing.Size(67, 19)
        Me.lb_項目_28.TabIndex = 25
        Me.lb_項目_28.Text = "登録件数"
        Me.lb_項目_28.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.lb_項目_28.Visible = False
        '
        'FpSpd
        '
        Me.FpSpd.AccessibleDescription = "FpSpd, Sheet1, Row 0, Column 0"
        Me.FpSpd.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!)
        Me.FpSpd.Location = New System.Drawing.Point(12, 66)
        Me.FpSpd.Name = "FpSpd"
        Me.FpSpd.Size = New System.Drawing.Size(1049, 481)
        Me.FpSpd.TabIndex = 1
        '
        'SnwMT02F06
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(1524, 614)
        Me.Controls.Add(Me.FpSpd)
        Me.Controls.Add(Me.CmdChkOff)
        Me.Controls.Add(Me.CmdChkOn)
        Me.Controls.Add(Me.tx_Dummy3)
        Me.Controls.Add(Me.tx_Dummy2)
        Me.Controls.Add(Me.tx_Dummy1)
        Me.Controls.Add(Me.PicFunction)
        Me.Controls.Add(Me.sb_Msg)
        Me.Controls.Add(Me.Tx_テンプレート名)
        Me.Controls.Add(Me.rf_得意先CD)
        Me.Controls.Add(Me.rf_得意先名)
        Me.Controls.Add(Me.lb_項目_4)
        Me.Controls.Add(Me.lb_項目_0)
        Me.Controls.Add(Me.lb_cnt)
        Me.Controls.Add(Me.lb_項目_28)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(11, 11)
        Me.Name = "SnwMT02F06"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "顧客テンプレート選択"
        Me.PicFunction.ResumeLayout(False)
        Me.sb_Msg.ResumeLayout(False)
        Me.sb_Msg.PerformLayout()
        CType(Me.FpSpd, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents FpSpd As FarPoint.Win.Spread.FpSpread
    Friend WithEvents Timer1 As Timer
    Friend WithEvents FpSpd_Sheet1 As FarPoint.Win.Spread.SheetView
End Class