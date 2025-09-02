<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SnwMT02F14

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
	Public WithEvents CbNewImport As System.Windows.Forms.Button
	Public WithEvents Cb変更 As System.Windows.Forms.Button
	Public WithEvents txDir As System.Windows.Forms.TextBox
	Public WithEvents Picture1 As System.Windows.Forms.Panel
	Public WithEvents Cb中止 As System.Windows.Forms.Button
	Public WithEvents sb_Msg_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents sb_Msg As System.Windows.Forms.StatusStrip
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents lb_項目_3 As System.Windows.Forms.Label
    Public WithEvents rf_取込行数 As System.Windows.Forms.Label
    Public WithEvents lb_項目_4 As System.Windows.Forms.Label

    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使って変更できます。
    'コード エディタを使用して、変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SnwMT02F14))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.CbNewImport = New System.Windows.Forms.Button()
        Me.Cb変更 = New System.Windows.Forms.Button()
        Me.Picture1 = New System.Windows.Forms.Panel()
        Me.txDir = New System.Windows.Forms.TextBox()
        Me.Cb中止 = New System.Windows.Forms.Button()
        Me.sb_Msg = New System.Windows.Forms.StatusStrip()
        Me.sb_Msg_Panel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lb_項目_3 = New System.Windows.Forms.Label()
        Me.rf_取込行数 = New System.Windows.Forms.Label()
        Me.lb_項目_4 = New System.Windows.Forms.Label()
        Me.Picture1.SuspendLayout()
        Me.sb_Msg.SuspendLayout()
        Me.SuspendLayout()
        '
        'CbNewImport
        '
        Me.CbNewImport.BackColor = System.Drawing.SystemColors.Control
        Me.CbNewImport.Cursor = System.Windows.Forms.Cursors.Default
        Me.CbNewImport.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CbNewImport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CbNewImport.Location = New System.Drawing.Point(316, 181)
        Me.CbNewImport.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.CbNewImport.Name = "CbNewImport"
        Me.CbNewImport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CbNewImport.Size = New System.Drawing.Size(72, 25)
        Me.CbNewImport.TabIndex = 0
        Me.CbNewImport.Text = "取込(&I)"
        Me.CbNewImport.UseVisualStyleBackColor = False
        '
        'Cb変更
        '
        Me.Cb変更.BackColor = System.Drawing.SystemColors.Control
        Me.Cb変更.Cursor = System.Windows.Forms.Cursors.Default
        Me.Cb変更.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Cb変更.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Cb変更.Location = New System.Drawing.Point(403, 44)
        Me.Cb変更.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Cb変更.Name = "Cb変更"
        Me.Cb変更.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Cb変更.Size = New System.Drawing.Size(67, 25)
        Me.Cb変更.TabIndex = 8
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
        Me.Picture1.Location = New System.Drawing.Point(16, 44)
        Me.Picture1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Picture1.Name = "Picture1"
        Me.Picture1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Picture1.Size = New System.Drawing.Size(384, 19)
        Me.Picture1.TabIndex = 6
        Me.Picture1.TabStop = True
        '
        'txDir
        '
        Me.txDir.AcceptsReturn = True
        Me.txDir.BackColor = System.Drawing.SystemColors.Control
        Me.txDir.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txDir.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txDir.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txDir.Location = New System.Drawing.Point(0, 0)
        Me.txDir.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txDir.MaxLength = 60
        Me.txDir.Name = "txDir"
        Me.txDir.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txDir.Size = New System.Drawing.Size(420, 15)
        Me.txDir.TabIndex = 7
        '
        'Cb中止
        '
        Me.Cb中止.BackColor = System.Drawing.SystemColors.Control
        Me.Cb中止.Cursor = System.Windows.Forms.Cursors.Default
        Me.Cb中止.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cb中止.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Cb中止.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Cb中止.Location = New System.Drawing.Point(396, 181)
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
        Me.sb_Msg.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.sb_Msg.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.sb_Msg_Panel1})
        Me.sb_Msg.Location = New System.Drawing.Point(0, 269)
        Me.sb_Msg.Name = "sb_Msg"
        Me.sb_Msg.Padding = New System.Windows.Forms.Padding(1, 0, 13, 0)
        Me.sb_Msg.Size = New System.Drawing.Size(483, 22)
        Me.sb_Msg.TabIndex = 2
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
        Me.sb_Msg_Panel1.Size = New System.Drawing.Size(469, 22)
        Me.sb_Msg_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(12, 80)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(245, 174)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "取込ファイル仕様　しまむらWeb処理履歴一覧" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  8519_XX_SHINCHOKU_XXXX.CSV" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "※出荷済を取り込みます。" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "XX部分" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "01：しま" &
    "むら" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "02：アベイル" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "04：バースディ" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "05：シャンブル" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "06：ディバロ" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'lb_項目_3
        '
        Me.lb_項目_3.BackColor = System.Drawing.SystemColors.Control
        Me.lb_項目_3.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_項目_3.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_項目_3.ForeColor = System.Drawing.Color.Black
        Me.lb_項目_3.Location = New System.Drawing.Point(15, 12)
        Me.lb_項目_3.Name = "lb_項目_3"
        Me.lb_項目_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_項目_3.Size = New System.Drawing.Size(89, 21)
        Me.lb_項目_3.TabIndex = 3
        Me.lb_項目_3.Text = "【入力先】"
        Me.lb_項目_3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'rf_取込行数
        '
        Me.rf_取込行数.BackColor = System.Drawing.SystemColors.Control
        Me.rf_取込行数.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_取込行数.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_取込行数.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_取込行数.Location = New System.Drawing.Point(419, 88)
        Me.rf_取込行数.Name = "rf_取込行数"
        Me.rf_取込行数.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_取込行数.Size = New System.Drawing.Size(44, 18)
        Me.rf_取込行数.TabIndex = 5
        Me.rf_取込行数.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lb_項目_4
        '
        Me.lb_項目_4.BackColor = System.Drawing.SystemColors.Control
        Me.lb_項目_4.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_項目_4.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_項目_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_項目_4.Location = New System.Drawing.Point(325, 88)
        Me.lb_項目_4.Name = "lb_項目_4"
        Me.lb_項目_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_項目_4.Size = New System.Drawing.Size(81, 18)
        Me.lb_項目_4.TabIndex = 4
        Me.lb_項目_4.Text = "取込行数"
        '
        'SnwMT02F14
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.Cb中止
        Me.ClientSize = New System.Drawing.Size(483, 291)
        Me.Controls.Add(Me.CbNewImport)
        Me.Controls.Add(Me.Cb変更)
        Me.Controls.Add(Me.Picture1)
        Me.Controls.Add(Me.Cb中止)
        Me.Controls.Add(Me.sb_Msg)
        Me.Controls.Add(Me.Label1)
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
        Me.Name = "SnwMT02F14"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "しまむら消耗品取込"
        Me.Picture1.ResumeLayout(False)
        Me.Picture1.PerformLayout()
        Me.sb_Msg.ResumeLayout(False)
        Me.sb_Msg.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

End Class