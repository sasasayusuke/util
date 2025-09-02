<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SnwMT02F03

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
	Public WithEvents Cb変更 As System.Windows.Forms.Button
	Public WithEvents txDir As System.Windows.Forms.TextBox
    Public WithEvents CbXLS As System.Windows.Forms.Button
    Public WithEvents Cb中止 As System.Windows.Forms.Button
    Public WithEvents Tx_FromNo As ExNmText.ExNmTextBox
    Public WithEvents Tx_ToNo As ExNmText.ExNmTextBox
    Public WithEvents rf_見積番号 As System.Windows.Forms.Label
    Public WithEvents lb_項目_2 As System.Windows.Forms.Label
    Public WithEvents lb_kara As System.Windows.Forms.Label

    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使って変更できます。
    'コード エディタを使用して、変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SnwMT02F03))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Cb変更 = New System.Windows.Forms.Button()
        Me.txDir = New System.Windows.Forms.TextBox()
        Me.CbXLS = New System.Windows.Forms.Button()
        Me.Cb中止 = New System.Windows.Forms.Button()
        Me.Tx_FromNo = New ExNmText.ExNmTextBox()
        Me.Tx_ToNo = New ExNmText.ExNmTextBox()
        Me.rf_見積番号 = New System.Windows.Forms.Label()
        Me.lb_項目_2 = New System.Windows.Forms.Label()
        Me.lb_kara = New System.Windows.Forms.Label()
        Me.lb_番号指定 = New System.Windows.Forms.GroupBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.sb_Msg_Panel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.sb_Msg = New System.Windows.Forms.StatusStrip()
        Me.lb_番号指定.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.sb_Msg.SuspendLayout()
        Me.SuspendLayout()
        '
        'Cb変更
        '
        Me.Cb変更.BackColor = System.Drawing.SystemColors.Control
        Me.Cb変更.Cursor = System.Windows.Forms.Cursors.Default
        Me.Cb変更.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Cb変更.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Cb変更.Location = New System.Drawing.Point(388, 39)
        Me.Cb変更.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Cb変更.Name = "Cb変更"
        Me.Cb変更.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Cb変更.Size = New System.Drawing.Size(75, 25)
        Me.Cb変更.TabIndex = 4
        Me.Cb変更.Text = "フォルダ(&D)"
        Me.Cb変更.UseVisualStyleBackColor = False
        '
        'txDir
        '
        Me.txDir.AcceptsReturn = True
        Me.txDir.BackColor = System.Drawing.SystemColors.Control
        Me.txDir.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txDir.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txDir.Font = New System.Drawing.Font("MS UI Gothic", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txDir.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txDir.Location = New System.Drawing.Point(37, 39)
        Me.txDir.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txDir.MaxLength = 40
        Me.txDir.Name = "txDir"
        Me.txDir.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txDir.Size = New System.Drawing.Size(309, 17)
        Me.txDir.TabIndex = 8
        '
        'CbXLS
        '
        Me.CbXLS.BackColor = System.Drawing.SystemColors.Control
        Me.CbXLS.Cursor = System.Windows.Forms.Cursors.Default
        Me.CbXLS.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CbXLS.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CbXLS.Location = New System.Drawing.Point(349, 196)
        Me.CbXLS.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.CbXLS.Name = "CbXLS"
        Me.CbXLS.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CbXLS.Size = New System.Drawing.Size(72, 25)
        Me.CbXLS.TabIndex = 2
        Me.CbXLS.Text = "出力(&O)"
        Me.CbXLS.UseVisualStyleBackColor = False
        '
        'Cb中止
        '
        Me.Cb中止.BackColor = System.Drawing.SystemColors.Control
        Me.Cb中止.Cursor = System.Windows.Forms.Cursors.Default
        Me.Cb中止.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cb中止.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Cb中止.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Cb中止.Location = New System.Drawing.Point(429, 196)
        Me.Cb中止.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Cb中止.Name = "Cb中止"
        Me.Cb中止.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Cb中止.Size = New System.Drawing.Size(75, 25)
        Me.Cb中止.TabIndex = 3
        Me.Cb中止.Text = "ｷｬﾝｾﾙ"
        Me.Cb中止.UseVisualStyleBackColor = False
        '
        'Tx_FromNo
        '
        Me.Tx_FromNo.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.Tx_FromNo.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.Tx_FromNo.CanForwardSetFocus = True
        Me.Tx_FromNo.CanNextSetFocus = True
        Me.Tx_FromNo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Tx_FromNo.DecimalPlace = CType(0, Short)
        Me.Tx_FromNo.EditMode = True
        Me.Tx_FromNo.FocusBackColor = System.Drawing.SystemColors.Window
        Me.Tx_FromNo.FormatType = "#"
        Me.Tx_FromNo.FormatTypeNega = ""
        Me.Tx_FromNo.FormatTypeNull = ""
        Me.Tx_FromNo.FormatTypeZero = ""
        Me.Tx_FromNo.InputMinus = False
        Me.Tx_FromNo.InputPlus = True
        Me.Tx_FromNo.InputZero = False
        Me.Tx_FromNo.Location = New System.Drawing.Point(83, 28)
        Me.Tx_FromNo.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Tx_FromNo.MaxLength = 2
        Me.Tx_FromNo.Name = "Tx_FromNo"
        Me.Tx_FromNo.OldValue = "ExNmTextBox"
        Me.Tx_FromNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Tx_FromNo.SelectText = True
        Me.Tx_FromNo.SelLength = 0
        Me.Tx_FromNo.SelStart = 0
        Me.Tx_FromNo.SelText = ""
        Me.Tx_FromNo.Size = New System.Drawing.Size(39, 22)
        Me.Tx_FromNo.TabIndex = 0
        '
        'Tx_ToNo
        '
        Me.Tx_ToNo.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.Tx_ToNo.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.Tx_ToNo.CanForwardSetFocus = True
        Me.Tx_ToNo.CanNextSetFocus = True
        Me.Tx_ToNo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Tx_ToNo.DecimalPlace = CType(0, Short)
        Me.Tx_ToNo.EditMode = True
        Me.Tx_ToNo.FocusBackColor = System.Drawing.SystemColors.Window
        Me.Tx_ToNo.FormatType = "#"
        Me.Tx_ToNo.FormatTypeNega = ""
        Me.Tx_ToNo.FormatTypeNull = ""
        Me.Tx_ToNo.FormatTypeZero = ""
        Me.Tx_ToNo.InputMinus = False
        Me.Tx_ToNo.InputPlus = True
        Me.Tx_ToNo.InputZero = False
        Me.Tx_ToNo.Location = New System.Drawing.Point(171, 26)
        Me.Tx_ToNo.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Tx_ToNo.MaxLength = 2
        Me.Tx_ToNo.Name = "Tx_ToNo"
        Me.Tx_ToNo.OldValue = "ExNmTextBox"
        Me.Tx_ToNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Tx_ToNo.SelectText = True
        Me.Tx_ToNo.SelLength = 0
        Me.Tx_ToNo.SelStart = 0
        Me.Tx_ToNo.SelText = ""
        Me.Tx_ToNo.Size = New System.Drawing.Size(40, 22)
        Me.Tx_ToNo.TabIndex = 1
        '
        'rf_見積番号
        '
        Me.rf_見積番号.BackColor = System.Drawing.SystemColors.Control
        Me.rf_見積番号.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_見積番号.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_見積番号.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_見積番号.Location = New System.Drawing.Point(117, 12)
        Me.rf_見積番号.Name = "rf_見積番号"
        Me.rf_見積番号.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_見積番号.Size = New System.Drawing.Size(67, 14)
        Me.rf_見積番号.TabIndex = 12
        Me.rf_見積番号.Text = "21755009"
        Me.rf_見積番号.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lb_項目_2
        '
        Me.lb_項目_2.BackColor = System.Drawing.SystemColors.Control
        Me.lb_項目_2.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_項目_2.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_項目_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_項目_2.Location = New System.Drawing.Point(25, 12)
        Me.lb_項目_2.Name = "lb_項目_2"
        Me.lb_項目_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_項目_2.Size = New System.Drawing.Size(77, 18)
        Me.lb_項目_2.TabIndex = 11
        Me.lb_項目_2.Text = "見積№"
        '
        'lb_kara
        '
        Me.lb_kara.BackColor = System.Drawing.SystemColors.Control
        Me.lb_kara.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_kara.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_kara.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_kara.Location = New System.Drawing.Point(135, 29)
        Me.lb_kara.Name = "lb_kara"
        Me.lb_kara.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_kara.Size = New System.Drawing.Size(20, 18)
        Me.lb_kara.TabIndex = 9
        Me.lb_kara.Text = "～"
        Me.lb_kara.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_番号指定
        '
        Me.lb_番号指定.Controls.Add(Me.Tx_FromNo)
        Me.lb_番号指定.Controls.Add(Me.Tx_ToNo)
        Me.lb_番号指定.Controls.Add(Me.lb_kara)
        Me.lb_番号指定.Font = New System.Drawing.Font("MS UI Gothic", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_番号指定.Location = New System.Drawing.Point(13, 41)
        Me.lb_番号指定.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.lb_番号指定.Name = "lb_番号指定"
        Me.lb_番号指定.Padding = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.lb_番号指定.Size = New System.Drawing.Size(491, 62)
        Me.lb_番号指定.TabIndex = 13
        Me.lb_番号指定.TabStop = False
        Me.lb_番号指定.Text = "【範囲指定】"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.txDir)
        Me.GroupBox1.Controls.Add(Me.Cb変更)
        Me.GroupBox1.Font = New System.Drawing.Font("MS UI Gothic", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(13, 110)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.GroupBox1.Size = New System.Drawing.Size(491, 76)
        Me.GroupBox1.TabIndex = 14
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "【出力先】"
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
        Me.sb_Msg_Panel1.Size = New System.Drawing.Size(350, 24)
        Me.sb_Msg_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'sb_Msg
        '
        Me.sb_Msg.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.sb_Msg.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.sb_Msg.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.sb_Msg_Panel1})
        Me.sb_Msg.Location = New System.Drawing.Point(0, 245)
        Me.sb_Msg.Name = "sb_Msg"
        Me.sb_Msg.Padding = New System.Windows.Forms.Padding(1, 0, 13, 0)
        Me.sb_Msg.Size = New System.Drawing.Size(535, 24)
        Me.sb_Msg.TabIndex = 5
        '
        'SnwMT02F03
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.Cb中止
        Me.ClientSize = New System.Drawing.Size(535, 269)
        Me.Controls.Add(Me.lb_番号指定)
        Me.Controls.Add(Me.CbXLS)
        Me.Controls.Add(Me.Cb中止)
        Me.Controls.Add(Me.sb_Msg)
        Me.Controls.Add(Me.rf_見積番号)
        Me.Controls.Add(Me.lb_項目_2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(3, 22)
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SnwMT02F03"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "員数シート出力"
        Me.lb_番号指定.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.sb_Msg.ResumeLayout(False)
        Me.sb_Msg.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lb_番号指定 As GroupBox
    Friend WithEvents GroupBox1 As GroupBox
    Public WithEvents sb_Msg_Panel1 As ToolStripStatusLabel
    Public WithEvents sb_Msg As StatusStrip

End Class
