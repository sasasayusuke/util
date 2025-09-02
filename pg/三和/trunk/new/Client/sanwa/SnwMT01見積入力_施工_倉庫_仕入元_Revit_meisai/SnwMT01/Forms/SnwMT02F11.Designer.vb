<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SnwMT02F11

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
	Public WithEvents Pic_印刷条件 As System.Windows.Forms.Panel
    Public WithEvents tx_掛率 As System.Windows.Forms.TextBox
    Public WithEvents Cb中止 As System.Windows.Forms.Button
    Public WithEvents CdOK As System.Windows.Forms.Button
    Public WithEvents lb_項目_0 As System.Windows.Forms.Label
    Public WithEvents lb_項目_5 As System.Windows.Forms.Label
    Public WithEvents lb_項目_2 As System.Windows.Forms.Label
    Public WithEvents lb_項目_3 As System.Windows.Forms.Label
    Public WithEvents rf_売価 As System.Windows.Forms.Label
    Public WithEvents rf_原価 As System.Windows.Forms.Label
	'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
	'Windows フォーム デザイナを使って変更できます。
	'コード エディタを使用して、変更しないでください。
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SnwMT02F11))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Pic_印刷条件 = New System.Windows.Forms.Panel()
        Me.tx_掛率 = New System.Windows.Forms.TextBox()
        Me.Cb中止 = New System.Windows.Forms.Button()
        Me.CdOK = New System.Windows.Forms.Button()
        Me.lb_項目_0 = New System.Windows.Forms.Label()
        Me.lb_項目_5 = New System.Windows.Forms.Label()
        Me.lb_項目_2 = New System.Windows.Forms.Label()
        Me.lb_項目_3 = New System.Windows.Forms.Label()
        Me.rf_売価 = New System.Windows.Forms.Label()
        Me.rf_原価 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Pic_印刷条件
        '
        Me.Pic_印刷条件.BackColor = System.Drawing.SystemColors.Control
        Me.Pic_印刷条件.Cursor = System.Windows.Forms.Cursors.Default
        Me.Pic_印刷条件.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Pic_印刷条件.Location = New System.Drawing.Point(40, 8)
        Me.Pic_印刷条件.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Pic_印刷条件.Name = "Pic_印刷条件"
        Me.Pic_印刷条件.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Pic_印刷条件.Size = New System.Drawing.Size(183, 28)
        Me.Pic_印刷条件.TabIndex = 0
        Me.Pic_印刷条件.TabStop = True
        '
        'tx_掛率
        '
        Me.tx_掛率.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tx_掛率.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_掛率.Location = New System.Drawing.Point(139, 78)
        Me.tx_掛率.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.tx_掛率.MaxLength = 6
        Me.tx_掛率.Name = "tx_掛率"
        Me.tx_掛率.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_掛率.Size = New System.Drawing.Size(57, 22)
        Me.tx_掛率.TabIndex = 3
        Me.tx_掛率.Text = "999.99"
        Me.tx_掛率.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Cb中止
        '
        Me.Cb中止.BackColor = System.Drawing.SystemColors.Control
        Me.Cb中止.Cursor = System.Windows.Forms.Cursors.Default
        Me.Cb中止.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cb中止.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Cb中止.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Cb中止.Location = New System.Drawing.Point(180, 140)
        Me.Cb中止.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Cb中止.Name = "Cb中止"
        Me.Cb中止.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Cb中止.Size = New System.Drawing.Size(75, 24)
        Me.Cb中止.TabIndex = 5
        Me.Cb中止.Text = "ｷｬﾝｾﾙ"
        Me.Cb中止.UseVisualStyleBackColor = False
        '
        'CdOK
        '
        Me.CdOK.BackColor = System.Drawing.SystemColors.Control
        Me.CdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.CdOK.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CdOK.Location = New System.Drawing.Point(96, 140)
        Me.CdOK.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.CdOK.Name = "CdOK"
        Me.CdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CdOK.Size = New System.Drawing.Size(75, 24)
        Me.CdOK.TabIndex = 4
        Me.CdOK.Text = "OK"
        Me.CdOK.UseVisualStyleBackColor = False
        '
        'lb_項目_0
        '
        Me.lb_項目_0.BackColor = System.Drawing.SystemColors.Control
        Me.lb_項目_0.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_項目_0.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_項目_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_項目_0.Location = New System.Drawing.Point(203, 81)
        Me.lb_項目_0.Name = "lb_項目_0"
        Me.lb_項目_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_項目_0.Size = New System.Drawing.Size(19, 12)
        Me.lb_項目_0.TabIndex = 11
        Me.lb_項目_0.Text = "％"
        '
        'lb_項目_5
        '
        Me.lb_項目_5.BackColor = System.Drawing.SystemColors.Control
        Me.lb_項目_5.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_項目_5.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_項目_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_項目_5.Location = New System.Drawing.Point(35, 81)
        Me.lb_項目_5.Name = "lb_項目_5"
        Me.lb_項目_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_項目_5.Size = New System.Drawing.Size(64, 15)
        Me.lb_項目_5.TabIndex = 10
        Me.lb_項目_5.Text = "掛  率"
        '
        'lb_項目_2
        '
        Me.lb_項目_2.BackColor = System.Drawing.SystemColors.Control
        Me.lb_項目_2.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_項目_2.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_項目_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_項目_2.Location = New System.Drawing.Point(35, 52)
        Me.lb_項目_2.Name = "lb_項目_2"
        Me.lb_項目_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_項目_2.Size = New System.Drawing.Size(59, 21)
        Me.lb_項目_2.TabIndex = 9
        Me.lb_項目_2.Text = "原  価"
        '
        'lb_項目_3
        '
        Me.lb_項目_3.BackColor = System.Drawing.SystemColors.Control
        Me.lb_項目_3.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_項目_3.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_項目_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_項目_3.Location = New System.Drawing.Point(35, 108)
        Me.lb_項目_3.Name = "lb_項目_3"
        Me.lb_項目_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_項目_3.Size = New System.Drawing.Size(59, 15)
        Me.lb_項目_3.TabIndex = 8
        Me.lb_項目_3.Text = "売  価"
        '
        'rf_売価
        '
        Me.rf_売価.BackColor = System.Drawing.SystemColors.Window
        Me.rf_売価.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.rf_売価.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_売価.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_売価.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_売価.Location = New System.Drawing.Point(99, 104)
        Me.rf_売価.Name = "rf_売価"
        Me.rf_売価.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_売価.Size = New System.Drawing.Size(124, 19)
        Me.rf_売価.TabIndex = 7
        Me.rf_売価.Text = "99,999,999.99"
        Me.rf_売価.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'rf_原価
        '
        Me.rf_原価.BackColor = System.Drawing.SystemColors.Window
        Me.rf_原価.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.rf_原価.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_原価.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_原価.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_原価.Location = New System.Drawing.Point(99, 52)
        Me.rf_原価.Name = "rf_原価"
        Me.rf_原価.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_原価.Size = New System.Drawing.Size(124, 22)
        Me.rf_原価.TabIndex = 6
        Me.rf_原価.Text = "99,999,999.99"
        Me.rf_原価.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'SnwMT02F11
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.Cb中止
        Me.ClientSize = New System.Drawing.Size(261, 172)
        Me.Controls.Add(Me.Pic_印刷条件)
        Me.Controls.Add(Me.tx_掛率)
        Me.Controls.Add(Me.Cb中止)
        Me.Controls.Add(Me.CdOK)
        Me.Controls.Add(Me.lb_項目_0)
        Me.Controls.Add(Me.lb_項目_5)
        Me.Controls.Add(Me.lb_項目_2)
        Me.Controls.Add(Me.lb_項目_3)
        Me.Controls.Add(Me.rf_売価)
        Me.Controls.Add(Me.rf_原価)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(184, 22)
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SnwMT02F11"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "売価変更"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

End Class