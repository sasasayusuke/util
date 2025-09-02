<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SnwMT02F12

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
	Public WithEvents CbGET As System.Windows.Forms.Button
	Public WithEvents Cb中止 As System.Windows.Forms.Button
	Public WithEvents sb_Msg_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents sb_Msg As System.Windows.Forms.StatusStrip
	Public WithEvents tx_見積番号 As ExNmText.ExNmTextBox
	Public WithEvents tx_s行 As ExNmText.ExNmTextBox
	Public WithEvents tx_e行 As ExNmText.ExNmTextBox
	Public WithEvents lb_項目_0 As System.Windows.Forms.Label
	Public WithEvents lb_kara_1 As System.Windows.Forms.Label
	Public WithEvents lb_項目_3 As System.Windows.Forms.Label
    Public WithEvents rf_取込行数 As System.Windows.Forms.Label
    Public WithEvents lb_項目_4 As System.Windows.Forms.Label
    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使って変更できます。
    'コード エディタを使用して、変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SnwMT02F12))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.CbGET = New System.Windows.Forms.Button()
        Me.Cb中止 = New System.Windows.Forms.Button()
        Me.sb_Msg = New System.Windows.Forms.StatusStrip()
        Me.sb_Msg_Panel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tx_見積番号 = New ExNmText.ExNmTextBox()
        Me.tx_s行 = New ExNmText.ExNmTextBox()
        Me.tx_e行 = New ExNmText.ExNmTextBox()
        Me.lb_項目_0 = New System.Windows.Forms.Label()
        Me.lb_kara_1 = New System.Windows.Forms.Label()
        Me.lb_項目_3 = New System.Windows.Forms.Label()
        Me.rf_取込行数 = New System.Windows.Forms.Label()
        Me.lb_項目_4 = New System.Windows.Forms.Label()
        Me.sb_Msg.SuspendLayout()
        Me.SuspendLayout()
        '
        'CbGET
        '
        Me.CbGET.BackColor = System.Drawing.SystemColors.Control
        Me.CbGET.Cursor = System.Windows.Forms.Cursors.Default
        Me.CbGET.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CbGET.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CbGET.Location = New System.Drawing.Point(139, 112)
        Me.CbGET.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.CbGET.Name = "CbGET"
        Me.CbGET.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CbGET.Size = New System.Drawing.Size(72, 25)
        Me.CbGET.TabIndex = 3
        Me.CbGET.Text = "取込(&I)"
        Me.CbGET.UseVisualStyleBackColor = False
        '
        'Cb中止
        '
        Me.Cb中止.BackColor = System.Drawing.SystemColors.Control
        Me.Cb中止.Cursor = System.Windows.Forms.Cursors.Default
        Me.Cb中止.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cb中止.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Cb中止.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Cb中止.Location = New System.Drawing.Point(219, 112)
        Me.Cb中止.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Cb中止.Name = "Cb中止"
        Me.Cb中止.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Cb中止.Size = New System.Drawing.Size(75, 25)
        Me.Cb中止.TabIndex = 4
        Me.Cb中止.Text = "ｷｬﾝｾﾙ"
        Me.Cb中止.UseVisualStyleBackColor = False
        '
        'sb_Msg
        '
        Me.sb_Msg.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.sb_Msg.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.sb_Msg_Panel1})
        Me.sb_Msg.Location = New System.Drawing.Point(0, 156)
        Me.sb_Msg.Name = "sb_Msg"
        Me.sb_Msg.Padding = New System.Windows.Forms.Padding(1, 0, 13, 0)
        Me.sb_Msg.Size = New System.Drawing.Size(343, 22)
        Me.sb_Msg.TabIndex = 5
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
        Me.sb_Msg_Panel1.Size = New System.Drawing.Size(246, 22)
        Me.sb_Msg_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tx_見積番号
        '
        Me.tx_見積番号.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_見積番号.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_見積番号.CanForwardSetFocus = True
        Me.tx_見積番号.CanNextSetFocus = True
        Me.tx_見積番号.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_見積番号.DecimalPlace = CType(0, Short)
        Me.tx_見積番号.EditMode = True
        Me.tx_見積番号.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_見積番号.FormatType = ""
        Me.tx_見積番号.FormatTypeNega = ""
        Me.tx_見積番号.FormatTypeNull = ""
        Me.tx_見積番号.FormatTypeZero = ""
        Me.tx_見積番号.InputMinus = False
        Me.tx_見積番号.InputPlus = True
        Me.tx_見積番号.InputZero = False
        Me.tx_見積番号.Location = New System.Drawing.Point(108, 28)
        Me.tx_見積番号.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.tx_見積番号.MaxLength = 7
        Me.tx_見積番号.Name = "tx_見積番号"
        Me.tx_見積番号.OldValue = "ExNmTextBox"
        Me.tx_見積番号.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_見積番号.SelectText = True
        Me.tx_見積番号.SelLength = 0
        Me.tx_見積番号.SelStart = 0
        Me.tx_見積番号.SelText = ""
        Me.tx_見積番号.Size = New System.Drawing.Size(63, 22)
        Me.tx_見積番号.TabIndex = 0
        '
        'tx_s行
        '
        Me.tx_s行.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_s行.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_s行.CanForwardSetFocus = True
        Me.tx_s行.CanNextSetFocus = True
        Me.tx_s行.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_s行.DecimalPlace = CType(0, Short)
        Me.tx_s行.EditMode = True
        Me.tx_s行.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_s行.FormatType = "#"
        Me.tx_s行.FormatTypeNega = ""
        Me.tx_s行.FormatTypeNull = "#"
        Me.tx_s行.FormatTypeZero = "#"
        Me.tx_s行.InputMinus = False
        Me.tx_s行.InputPlus = True
        Me.tx_s行.InputZero = False
        Me.tx_s行.Location = New System.Drawing.Point(108, 52)
        Me.tx_s行.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.tx_s行.MaxLength = 4
        Me.tx_s行.Name = "tx_s行"
        Me.tx_s行.OldValue = "ExNmTextBox"
        Me.tx_s行.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_s行.SelectText = True
        Me.tx_s行.SelLength = 0
        Me.tx_s行.SelStart = 0
        Me.tx_s行.SelText = ""
        Me.tx_s行.Size = New System.Drawing.Size(36, 22)
        Me.tx_s行.TabIndex = 1
        '
        'tx_e行
        '
        Me.tx_e行.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_e行.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_e行.CanForwardSetFocus = True
        Me.tx_e行.CanNextSetFocus = True
        Me.tx_e行.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_e行.DecimalPlace = CType(0, Short)
        Me.tx_e行.EditMode = True
        Me.tx_e行.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_e行.FormatType = "#"
        Me.tx_e行.FormatTypeNega = ""
        Me.tx_e行.FormatTypeNull = "#"
        Me.tx_e行.FormatTypeZero = "#"
        Me.tx_e行.InputMinus = False
        Me.tx_e行.InputPlus = True
        Me.tx_e行.InputZero = False
        Me.tx_e行.Location = New System.Drawing.Point(161, 52)
        Me.tx_e行.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.tx_e行.MaxLength = 4
        Me.tx_e行.Name = "tx_e行"
        Me.tx_e行.OldValue = "ExNmTextBox"
        Me.tx_e行.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_e行.SelectText = True
        Me.tx_e行.SelLength = 0
        Me.tx_e行.SelStart = 0
        Me.tx_e行.SelText = ""
        Me.tx_e行.Size = New System.Drawing.Size(36, 22)
        Me.tx_e行.TabIndex = 2
        '
        'lb_項目_0
        '
        Me.lb_項目_0.BackColor = System.Drawing.SystemColors.Control
        Me.lb_項目_0.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_項目_0.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_項目_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_項目_0.Location = New System.Drawing.Point(67, 54)
        Me.lb_項目_0.Name = "lb_項目_0"
        Me.lb_項目_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_項目_0.Size = New System.Drawing.Size(35, 18)
        Me.lb_項目_0.TabIndex = 10
        Me.lb_項目_0.Text = "行"
        '
        'lb_kara_1
        '
        Me.lb_kara_1.BackColor = System.Drawing.SystemColors.Control
        Me.lb_kara_1.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_kara_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_kara_1.Location = New System.Drawing.Point(144, 56)
        Me.lb_kara_1.Name = "lb_kara_1"
        Me.lb_kara_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_kara_1.Size = New System.Drawing.Size(17, 12)
        Me.lb_kara_1.TabIndex = 9
        Me.lb_kara_1.Text = "～"
        Me.lb_kara_1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_項目_3
        '
        Me.lb_項目_3.BackColor = System.Drawing.SystemColors.Control
        Me.lb_項目_3.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_項目_3.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_項目_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_項目_3.Location = New System.Drawing.Point(43, 32)
        Me.lb_項目_3.Name = "lb_項目_3"
        Me.lb_項目_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_項目_3.Size = New System.Drawing.Size(59, 15)
        Me.lb_項目_3.TabIndex = 8
        Me.lb_項目_3.Text = "見積№"
        '
        'rf_取込行数
        '
        Me.rf_取込行数.BackColor = System.Drawing.SystemColors.Control
        Me.rf_取込行数.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_取込行数.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_取込行数.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_取込行数.Location = New System.Drawing.Point(128, 84)
        Me.rf_取込行数.Name = "rf_取込行数"
        Me.rf_取込行数.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_取込行数.Size = New System.Drawing.Size(40, 18)
        Me.rf_取込行数.TabIndex = 7
        Me.rf_取込行数.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lb_項目_4
        '
        Me.lb_項目_4.BackColor = System.Drawing.SystemColors.Control
        Me.lb_項目_4.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_項目_4.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_項目_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_項目_4.Location = New System.Drawing.Point(43, 82)
        Me.lb_項目_4.Name = "lb_項目_4"
        Me.lb_項目_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_項目_4.Size = New System.Drawing.Size(77, 18)
        Me.lb_項目_4.TabIndex = 6
        Me.lb_項目_4.Text = "取込行数"
        '
        'SnwMT02F12
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.Cb中止
        Me.ClientSize = New System.Drawing.Size(343, 178)
        Me.Controls.Add(Me.CbGET)
        Me.Controls.Add(Me.Cb中止)
        Me.Controls.Add(Me.sb_Msg)
        Me.Controls.Add(Me.tx_見積番号)
        Me.Controls.Add(Me.tx_s行)
        Me.Controls.Add(Me.tx_e行)
        Me.Controls.Add(Me.lb_項目_0)
        Me.Controls.Add(Me.lb_kara_1)
        Me.Controls.Add(Me.lb_項目_3)
        Me.Controls.Add(Me.rf_取込行数)
        Me.Controls.Add(Me.lb_項目_4)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(3, 22)
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SnwMT02F12"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "員数取込"
        Me.sb_Msg.ResumeLayout(False)
        Me.sb_Msg.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

End Class