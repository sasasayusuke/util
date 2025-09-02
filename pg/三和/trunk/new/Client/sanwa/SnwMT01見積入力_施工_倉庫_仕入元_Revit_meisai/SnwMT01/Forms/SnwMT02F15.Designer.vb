<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SnwMT02F15

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
	Public WithEvents CbTenyo As System.Windows.Forms.Button
	Public WithEvents Cb変更 As System.Windows.Forms.Button
	Public WithEvents txDir As System.Windows.Forms.TextBox
	Public WithEvents Picture1 As System.Windows.Forms.Panel
	Public WithEvents CbXLS As System.Windows.Forms.Button
	Public WithEvents Cb中止 As System.Windows.Forms.Button
	Public WithEvents sb_Msg_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents sb_Msg As System.Windows.Forms.StatusStrip
	Public WithEvents tx_仕分番号 As ExNmText.ExNmTextBox
	Public WithEvents lb_項目_2 As System.Windows.Forms.Label
    Public WithEvents lb_項目_1 As System.Windows.Forms.Label
    Public WithEvents rf_製品なし数 As System.Windows.Forms.Label
    Public WithEvents rf_取込行数 As System.Windows.Forms.Label
    Public WithEvents lb_項目_4 As System.Windows.Forms.Label
	Public WithEvents lb_項目_0 As System.Windows.Forms.Label
	Public WithEvents lb_項目_3 As System.Windows.Forms.Label
    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使って変更できます。
    'コード エディタを使用して、変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SnwMT02F15))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.CbTenyo = New System.Windows.Forms.Button()
        Me.Cb変更 = New System.Windows.Forms.Button()
        Me.Picture1 = New System.Windows.Forms.Panel()
        Me.txDir = New System.Windows.Forms.TextBox()
        Me.CbXLS = New System.Windows.Forms.Button()
        Me.Cb中止 = New System.Windows.Forms.Button()
        Me.sb_Msg = New System.Windows.Forms.StatusStrip()
        Me.sb_Msg_Panel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tx_仕分番号 = New ExNmText.ExNmTextBox()
        Me.lb_項目_2 = New System.Windows.Forms.Label()
        Me.lb_項目_1 = New System.Windows.Forms.Label()
        Me.rf_製品なし数 = New System.Windows.Forms.Label()
        Me.rf_取込行数 = New System.Windows.Forms.Label()
        Me.lb_項目_4 = New System.Windows.Forms.Label()
        Me.lb_項目_0 = New System.Windows.Forms.Label()
        Me.lb_項目_3 = New System.Windows.Forms.Label()
        Me.Picture1.SuspendLayout()
        Me.sb_Msg.SuspendLayout()
        Me.SuspendLayout()
        '
        'CbTenyo
        '
        Me.CbTenyo.BackColor = System.Drawing.SystemColors.Control
        Me.CbTenyo.Cursor = System.Windows.Forms.Cursors.Default
        Me.CbTenyo.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CbTenyo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CbTenyo.Location = New System.Drawing.Point(284, 188)
        Me.CbTenyo.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.CbTenyo.Name = "CbTenyo"
        Me.CbTenyo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CbTenyo.Size = New System.Drawing.Size(72, 24)
        Me.CbTenyo.TabIndex = 14
        Me.CbTenyo.Text = "転用(&T)"
        Me.CbTenyo.UseVisualStyleBackColor = False
        '
        'Cb変更
        '
        Me.Cb変更.BackColor = System.Drawing.SystemColors.Control
        Me.Cb変更.Cursor = System.Windows.Forms.Cursors.Default
        Me.Cb変更.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Cb変更.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Cb変更.Location = New System.Drawing.Point(364, 109)
        Me.Cb変更.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Cb変更.Name = "Cb変更"
        Me.Cb変更.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Cb変更.Size = New System.Drawing.Size(71, 24)
        Me.Cb変更.TabIndex = 3
        Me.Cb変更.Text = "変更(&D)"
        Me.Cb変更.UseVisualStyleBackColor = False
        '
        'Picture1
        '
        Me.Picture1.BackColor = System.Drawing.SystemColors.Control
        Me.Picture1.Controls.Add(Me.txDir)
        Me.Picture1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Picture1.Enabled = False
        Me.Picture1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Picture1.Location = New System.Drawing.Point(51, 109)
        Me.Picture1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Picture1.Name = "Picture1"
        Me.Picture1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Picture1.Size = New System.Drawing.Size(301, 22)
        Me.Picture1.TabIndex = 6
        Me.Picture1.TabStop = True
        '
        'txDir
        '
        Me.txDir.AcceptsReturn = True
        Me.txDir.BackColor = System.Drawing.SystemColors.Control
        Me.txDir.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txDir.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txDir.Font = New System.Drawing.Font("MS UI Gothic", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txDir.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txDir.Location = New System.Drawing.Point(0, 0)
        Me.txDir.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txDir.MaxLength = 40
        Me.txDir.Name = "txDir"
        Me.txDir.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txDir.Size = New System.Drawing.Size(248, 17)
        Me.txDir.TabIndex = 7
        '
        'CbXLS
        '
        Me.CbXLS.BackColor = System.Drawing.SystemColors.Control
        Me.CbXLS.Cursor = System.Windows.Forms.Cursors.Default
        Me.CbXLS.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CbXLS.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CbXLS.Location = New System.Drawing.Point(284, 158)
        Me.CbXLS.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.CbXLS.Name = "CbXLS"
        Me.CbXLS.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CbXLS.Size = New System.Drawing.Size(72, 24)
        Me.CbXLS.TabIndex = 1
        Me.CbXLS.Text = "取込(&I)"
        Me.CbXLS.UseVisualStyleBackColor = False
        '
        'Cb中止
        '
        Me.Cb中止.BackColor = System.Drawing.SystemColors.Control
        Me.Cb中止.Cursor = System.Windows.Forms.Cursors.Default
        Me.Cb中止.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cb中止.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Cb中止.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Cb中止.Location = New System.Drawing.Point(364, 158)
        Me.Cb中止.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Cb中止.Name = "Cb中止"
        Me.Cb中止.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Cb中止.Size = New System.Drawing.Size(75, 24)
        Me.Cb中止.TabIndex = 2
        Me.Cb中止.Text = "ｷｬﾝｾﾙ"
        Me.Cb中止.UseVisualStyleBackColor = False
        '
        'sb_Msg
        '
        Me.sb_Msg.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.sb_Msg.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.sb_Msg_Panel1})
        Me.sb_Msg.Location = New System.Drawing.Point(0, 234)
        Me.sb_Msg.Name = "sb_Msg"
        Me.sb_Msg.Padding = New System.Windows.Forms.Padding(1, 0, 13, 0)
        Me.sb_Msg.Size = New System.Drawing.Size(459, 22)
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
        Me.sb_Msg_Panel1.Size = New System.Drawing.Size(457, 20)
        Me.sb_Msg_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tx_仕分番号
        '
        Me.tx_仕分番号.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_仕分番号.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_仕分番号.CanForwardSetFocus = True
        Me.tx_仕分番号.CanNextSetFocus = True
        Me.tx_仕分番号.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_仕分番号.DecimalPlace = CType(0, Short)
        Me.tx_仕分番号.EditMode = True
        Me.tx_仕分番号.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_仕分番号.FormatType = "#"
        Me.tx_仕分番号.FormatTypeNega = ""
        Me.tx_仕分番号.FormatTypeNull = ""
        Me.tx_仕分番号.FormatTypeZero = ""
        Me.tx_仕分番号.InputMinus = True
        Me.tx_仕分番号.InputPlus = True
        Me.tx_仕分番号.InputZero = True
        Me.tx_仕分番号.Location = New System.Drawing.Point(147, 24)
        Me.tx_仕分番号.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.tx_仕分番号.MaxLength = 2
        Me.tx_仕分番号.Name = "tx_仕分番号"
        Me.tx_仕分番号.OldValue = "ExNmTextBox"
        Me.tx_仕分番号.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_仕分番号.SelectText = True
        Me.tx_仕分番号.SelLength = 0
        Me.tx_仕分番号.SelStart = 0
        Me.tx_仕分番号.SelText = ""
        Me.tx_仕分番号.Size = New System.Drawing.Size(33, 22)
        Me.tx_仕分番号.TabIndex = 0
        '
        'lb_項目_2
        '
        Me.lb_項目_2.BackColor = System.Drawing.SystemColors.Control
        Me.lb_項目_2.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_項目_2.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_項目_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_項目_2.Location = New System.Drawing.Point(52, 28)
        Me.lb_項目_2.Name = "lb_項目_2"
        Me.lb_項目_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_項目_2.Size = New System.Drawing.Size(95, 21)
        Me.lb_項目_2.TabIndex = 13
        Me.lb_項目_2.Text = "取込仕分番号"
        '
        'lb_項目_1
        '
        Me.lb_項目_1.BackColor = System.Drawing.SystemColors.Control
        Me.lb_項目_1.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_項目_1.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_項目_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_項目_1.Location = New System.Drawing.Point(139, 188)
        Me.lb_項目_1.Name = "lb_項目_1"
        Me.lb_項目_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_項目_1.Size = New System.Drawing.Size(77, 18)
        Me.lb_項目_1.TabIndex = 12
        Me.lb_項目_1.Text = "製品なし数"
        '
        'rf_製品なし数
        '
        Me.rf_製品なし数.BackColor = System.Drawing.SystemColors.Control
        Me.rf_製品なし数.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_製品なし数.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_製品なし数.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_製品なし数.Location = New System.Drawing.Point(220, 189)
        Me.rf_製品なし数.Name = "rf_製品なし数"
        Me.rf_製品なし数.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_製品なし数.Size = New System.Drawing.Size(40, 18)
        Me.rf_製品なし数.TabIndex = 11
        Me.rf_製品なし数.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'rf_取込行数
        '
        Me.rf_取込行数.BackColor = System.Drawing.SystemColors.Control
        Me.rf_取込行数.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_取込行数.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_取込行数.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_取込行数.Location = New System.Drawing.Point(219, 160)
        Me.rf_取込行数.Name = "rf_取込行数"
        Me.rf_取込行数.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_取込行数.Size = New System.Drawing.Size(40, 18)
        Me.rf_取込行数.TabIndex = 10
        Me.rf_取込行数.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lb_項目_4
        '
        Me.lb_項目_4.BackColor = System.Drawing.SystemColors.Control
        Me.lb_項目_4.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_項目_4.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_項目_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_項目_4.Location = New System.Drawing.Point(139, 159)
        Me.lb_項目_4.Name = "lb_項目_4"
        Me.lb_項目_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_項目_4.Size = New System.Drawing.Size(77, 18)
        Me.lb_項目_4.TabIndex = 9
        Me.lb_項目_4.Text = "取込行数"
        '
        'lb_項目_0
        '
        Me.lb_項目_0.BackColor = System.Drawing.SystemColors.Control
        Me.lb_項目_0.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_項目_0.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_項目_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_項目_0.Location = New System.Drawing.Point(15, 159)
        Me.lb_項目_0.Name = "lb_項目_0"
        Me.lb_項目_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_項目_0.Size = New System.Drawing.Size(105, 18)
        Me.lb_項目_0.TabIndex = 8
        Me.lb_項目_0.Text = "レビット取込情報"
        '
        'lb_項目_3
        '
        Me.lb_項目_3.BackColor = System.Drawing.SystemColors.Control
        Me.lb_項目_3.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_項目_3.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_項目_3.ForeColor = System.Drawing.Color.Black
        Me.lb_項目_3.Location = New System.Drawing.Point(15, 80)
        Me.lb_項目_3.Name = "lb_項目_3"
        Me.lb_項目_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_項目_3.Size = New System.Drawing.Size(89, 21)
        Me.lb_項目_3.TabIndex = 5
        Me.lb_項目_3.Text = "【入力先】"
        Me.lb_項目_3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'SnwMT02F15
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.Cb中止
        Me.ClientSize = New System.Drawing.Size(459, 256)
        Me.Controls.Add(Me.CbTenyo)
        Me.Controls.Add(Me.Cb変更)
        Me.Controls.Add(Me.Picture1)
        Me.Controls.Add(Me.CbXLS)
        Me.Controls.Add(Me.Cb中止)
        Me.Controls.Add(Me.sb_Msg)
        Me.Controls.Add(Me.tx_仕分番号)
        Me.Controls.Add(Me.lb_項目_2)
        Me.Controls.Add(Me.lb_項目_1)
        Me.Controls.Add(Me.rf_製品なし数)
        Me.Controls.Add(Me.rf_取込行数)
        Me.Controls.Add(Me.lb_項目_4)
        Me.Controls.Add(Me.lb_項目_0)
        Me.Controls.Add(Me.lb_項目_3)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(3, 22)
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SnwMT02F15"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "レビット取込"
        Me.Picture1.ResumeLayout(False)
        Me.Picture1.PerformLayout()
        Me.sb_Msg.ResumeLayout(False)
        Me.sb_Msg.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

End Class