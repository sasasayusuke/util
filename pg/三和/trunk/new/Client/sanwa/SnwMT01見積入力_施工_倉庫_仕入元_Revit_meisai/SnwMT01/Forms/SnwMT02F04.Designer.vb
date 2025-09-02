<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SnwMT02F04

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
    Public WithEvents CbXLS As System.Windows.Forms.Button
    Public WithEvents Cb中止 As System.Windows.Forms.Button
    Public WithEvents sb_Msg_Panel1 As System.Windows.Forms.ToolStripStatusLabel
    Public WithEvents sb_Msg As System.Windows.Forms.StatusStrip
    Public WithEvents rf_取込行数 As System.Windows.Forms.Label
    Public WithEvents lb_項目_4 As System.Windows.Forms.Label
    Public WithEvents rf_取込仕分 As System.Windows.Forms.Label
    Public WithEvents lb_項目_2 As System.Windows.Forms.Label
    Public WithEvents rf_見積番号 As System.Windows.Forms.Label
    Public WithEvents lb_項目_1 As System.Windows.Forms.Label
    Public WithEvents lb_項目_0 As System.Windows.Forms.Label
    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使って変更できます。
    'コード エディタを使用して、変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SnwMT02F04))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.CbXLS = New System.Windows.Forms.Button()
        Me.Cb中止 = New System.Windows.Forms.Button()
        Me.sb_Msg = New System.Windows.Forms.StatusStrip()
        Me.sb_Msg_Panel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.rf_取込行数 = New System.Windows.Forms.Label()
        Me.lb_項目_4 = New System.Windows.Forms.Label()
        Me.rf_取込仕分 = New System.Windows.Forms.Label()
        Me.lb_項目_2 = New System.Windows.Forms.Label()
        Me.rf_見積番号 = New System.Windows.Forms.Label()
        Me.lb_項目_1 = New System.Windows.Forms.Label()
        Me.lb_項目_0 = New System.Windows.Forms.Label()
        Me.Cb変更 = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.txDir = New System.Windows.Forms.TextBox()
        Me.sb_Msg.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'CbXLS
        '
        Me.CbXLS.BackColor = System.Drawing.SystemColors.Control
        Me.CbXLS.Cursor = System.Windows.Forms.Cursors.Default
        Me.CbXLS.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CbXLS.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CbXLS.Location = New System.Drawing.Point(288, 88)
        Me.CbXLS.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.CbXLS.Name = "CbXLS"
        Me.CbXLS.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CbXLS.Size = New System.Drawing.Size(72, 25)
        Me.CbXLS.TabIndex = 0
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
        Me.Cb中止.Location = New System.Drawing.Point(368, 88)
        Me.Cb中止.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Cb中止.Name = "Cb中止"
        Me.Cb中止.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Cb中止.Size = New System.Drawing.Size(75, 25)
        Me.Cb中止.TabIndex = 1
        Me.Cb中止.Text = "ｷｬﾝｾﾙ"
        Me.Cb中止.UseVisualStyleBackColor = False
        '
        'sb_Msg
        '
        Me.sb_Msg.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.sb_Msg.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.sb_Msg.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.sb_Msg_Panel1})
        Me.sb_Msg.Location = New System.Drawing.Point(0, 194)
        Me.sb_Msg.Name = "sb_Msg"
        Me.sb_Msg.Padding = New System.Windows.Forms.Padding(1, 0, 13, 0)
        Me.sb_Msg.Size = New System.Drawing.Size(501, 24)
        Me.sb_Msg.TabIndex = 3
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
        Me.sb_Msg_Panel1.Size = New System.Drawing.Size(487, 24)
        Me.sb_Msg_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'rf_取込行数
        '
        Me.rf_取込行数.BackColor = System.Drawing.SystemColors.Control
        Me.rf_取込行数.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_取込行数.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_取込行数.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_取込行数.Location = New System.Drawing.Point(375, 148)
        Me.rf_取込行数.Name = "rf_取込行数"
        Me.rf_取込行数.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_取込行数.Size = New System.Drawing.Size(40, 18)
        Me.rf_取込行数.TabIndex = 13
        Me.rf_取込行数.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lb_項目_4
        '
        Me.lb_項目_4.BackColor = System.Drawing.SystemColors.Control
        Me.lb_項目_4.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_項目_4.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_項目_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_項目_4.Location = New System.Drawing.Point(293, 148)
        Me.lb_項目_4.Name = "lb_項目_4"
        Me.lb_項目_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_項目_4.Size = New System.Drawing.Size(77, 18)
        Me.lb_項目_4.TabIndex = 12
        Me.lb_項目_4.Text = "取込行数"
        '
        'rf_取込仕分
        '
        Me.rf_取込仕分.BackColor = System.Drawing.SystemColors.Control
        Me.rf_取込仕分.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_取込仕分.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_取込仕分.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_取込仕分.Location = New System.Drawing.Point(209, 148)
        Me.rf_取込仕分.Name = "rf_取込仕分"
        Me.rf_取込仕分.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_取込仕分.Size = New System.Drawing.Size(67, 18)
        Me.rf_取込仕分.TabIndex = 11
        Me.rf_取込仕分.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lb_項目_2
        '
        Me.lb_項目_2.BackColor = System.Drawing.SystemColors.Control
        Me.lb_項目_2.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_項目_2.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_項目_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_項目_2.Location = New System.Drawing.Point(128, 148)
        Me.lb_項目_2.Name = "lb_項目_2"
        Me.lb_項目_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_項目_2.Size = New System.Drawing.Size(77, 18)
        Me.lb_項目_2.TabIndex = 10
        Me.lb_項目_2.Text = "取込仕分"
        '
        'rf_見積番号
        '
        Me.rf_見積番号.BackColor = System.Drawing.SystemColors.Control
        Me.rf_見積番号.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_見積番号.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_見積番号.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_見積番号.Location = New System.Drawing.Point(209, 121)
        Me.rf_見積番号.Name = "rf_見積番号"
        Me.rf_見積番号.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_見積番号.Size = New System.Drawing.Size(67, 18)
        Me.rf_見積番号.TabIndex = 9
        Me.rf_見積番号.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lb_項目_1
        '
        Me.lb_項目_1.BackColor = System.Drawing.SystemColors.Control
        Me.lb_項目_1.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_項目_1.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_項目_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_項目_1.Location = New System.Drawing.Point(128, 122)
        Me.lb_項目_1.Name = "lb_項目_1"
        Me.lb_項目_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_項目_1.Size = New System.Drawing.Size(77, 18)
        Me.lb_項目_1.TabIndex = 8
        Me.lb_項目_1.Text = "見積№"
        '
        'lb_項目_0
        '
        Me.lb_項目_0.BackColor = System.Drawing.SystemColors.Control
        Me.lb_項目_0.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_項目_0.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_項目_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_項目_0.Location = New System.Drawing.Point(15, 122)
        Me.lb_項目_0.Name = "lb_項目_0"
        Me.lb_項目_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_項目_0.Size = New System.Drawing.Size(105, 18)
        Me.lb_項目_0.TabIndex = 7
        Me.lb_項目_0.Text = "取込シート情報"
        '
        'Cb変更
        '
        Me.Cb変更.BackColor = System.Drawing.SystemColors.Control
        Me.Cb変更.Cursor = System.Windows.Forms.Cursors.Default
        Me.Cb変更.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Cb変更.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Cb変更.Location = New System.Drawing.Point(356, 23)
        Me.Cb変更.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Cb変更.Name = "Cb変更"
        Me.Cb変更.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Cb変更.Size = New System.Drawing.Size(71, 25)
        Me.Cb変更.TabIndex = 2
        Me.Cb変更.Text = "変更(&D)"
        Me.Cb変更.UseVisualStyleBackColor = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.txDir)
        Me.GroupBox1.Controls.Add(Me.Cb変更)
        Me.GroupBox1.Font = New System.Drawing.Font("MS UI Gothic", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.GroupBox1.Size = New System.Drawing.Size(468, 62)
        Me.GroupBox1.TabIndex = 14
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "【入力先】"
        '
        'txDir
        '
        Me.txDir.AcceptsReturn = True
        Me.txDir.BackColor = System.Drawing.SystemColors.Control
        Me.txDir.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txDir.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txDir.Font = New System.Drawing.Font("MS UI Gothic", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txDir.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txDir.Location = New System.Drawing.Point(35, 29)
        Me.txDir.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txDir.MaxLength = 40
        Me.txDir.Name = "txDir"
        Me.txDir.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txDir.Size = New System.Drawing.Size(299, 15)
        Me.txDir.TabIndex = 6
        '
        'SnwMT02F04
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.Cb中止
        Me.ClientSize = New System.Drawing.Size(501, 218)
        Me.Controls.Add(Me.CbXLS)
        Me.Controls.Add(Me.Cb中止)
        Me.Controls.Add(Me.sb_Msg)
        Me.Controls.Add(Me.rf_取込行数)
        Me.Controls.Add(Me.lb_項目_4)
        Me.Controls.Add(Me.rf_取込仕分)
        Me.Controls.Add(Me.lb_項目_2)
        Me.Controls.Add(Me.rf_見積番号)
        Me.Controls.Add(Me.lb_項目_1)
        Me.Controls.Add(Me.lb_項目_0)
        Me.Controls.Add(Me.GroupBox1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(3, 22)
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SnwMT02F04"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "員数シート取込"
        Me.sb_Msg.ResumeLayout(False)
        Me.sb_Msg.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Public WithEvents Cb変更 As Button
    Friend WithEvents GroupBox1 As GroupBox
    Public WithEvents txDir As TextBox
End Class
