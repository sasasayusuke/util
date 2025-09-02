<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SnwMT02F13

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
	Public WithEvents tx_原価率上限 As ExNmText.ExNmTextBox
	Public WithEvents Cb中止 As System.Windows.Forms.Button
	Public WithEvents CdOK As System.Windows.Forms.Button
	Public WithEvents tx_原価率下限 As ExNmText.ExNmTextBox
	Public WithEvents _lb_項目_1 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_0 As System.Windows.Forms.Label
    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使って変更できます。
    'コード エディタを使用して、変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SnwMT02F13))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.tx_原価率上限 = New ExNmText.ExNmTextBox()
        Me.Cb中止 = New System.Windows.Forms.Button()
        Me.CdOK = New System.Windows.Forms.Button()
        Me.tx_原価率下限 = New ExNmText.ExNmTextBox()
        Me._lb_項目_1 = New System.Windows.Forms.Label()
        Me._lb_項目_0 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'tx_原価率上限
        '
        Me.tx_原価率上限.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_原価率上限.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_原価率上限.CanForwardSetFocus = True
        Me.tx_原価率上限.CanNextSetFocus = True
        Me.tx_原価率上限.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_原価率上限.DecimalPlace = CType(2, Short)
        Me.tx_原価率上限.EditMode = True
        Me.tx_原価率上限.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_原価率上限.FormatType = "##0.00"
        Me.tx_原価率上限.FormatTypeNega = ""
        Me.tx_原価率上限.FormatTypeNull = ""
        Me.tx_原価率上限.FormatTypeZero = ""
        Me.tx_原価率上限.InputMinus = False
        Me.tx_原価率上限.InputPlus = True
        Me.tx_原価率上限.InputZero = False
        Me.tx_原価率上限.Location = New System.Drawing.Point(28, 16)
        Me.tx_原価率上限.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.tx_原価率上限.MaxLength = 6
        Me.tx_原価率上限.Name = "tx_原価率上限"
        Me.tx_原価率上限.OldValue = "999.99"
        Me.tx_原価率上限.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_原価率上限.SelectText = True
        Me.tx_原価率上限.SelLength = 0
        Me.tx_原価率上限.SelStart = 0
        Me.tx_原価率上限.SelText = ""
        Me.tx_原価率上限.Size = New System.Drawing.Size(57, 22)
        Me.tx_原価率上限.TabIndex = 0
        Me.tx_原価率上限.Text = "999.99"
        '
        'Cb中止
        '
        Me.Cb中止.BackColor = System.Drawing.SystemColors.Control
        Me.Cb中止.Cursor = System.Windows.Forms.Cursors.Default
        Me.Cb中止.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cb中止.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Cb中止.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Cb中止.Location = New System.Drawing.Point(180, 92)
        Me.Cb中止.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Cb中止.Name = "Cb中止"
        Me.Cb中止.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Cb中止.Size = New System.Drawing.Size(75, 24)
        Me.Cb中止.TabIndex = 3
        Me.Cb中止.Text = "ｷｬﾝｾﾙ"
        Me.Cb中止.UseVisualStyleBackColor = False
        '
        'CdOK
        '
        Me.CdOK.BackColor = System.Drawing.SystemColors.Control
        Me.CdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.CdOK.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CdOK.Location = New System.Drawing.Point(96, 92)
        Me.CdOK.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.CdOK.Name = "CdOK"
        Me.CdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CdOK.Size = New System.Drawing.Size(75, 24)
        Me.CdOK.TabIndex = 2
        Me.CdOK.Text = "OK"
        Me.CdOK.UseVisualStyleBackColor = False
        '
        'tx_原価率下限
        '
        Me.tx_原価率下限.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_原価率下限.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_原価率下限.CanForwardSetFocus = True
        Me.tx_原価率下限.CanNextSetFocus = True
        Me.tx_原価率下限.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_原価率下限.DecimalPlace = CType(2, Short)
        Me.tx_原価率下限.EditMode = True
        Me.tx_原価率下限.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_原価率下限.FormatType = "##0.00"
        Me.tx_原価率下限.FormatTypeNega = ""
        Me.tx_原価率下限.FormatTypeNull = ""
        Me.tx_原価率下限.FormatTypeZero = ""
        Me.tx_原価率下限.InputMinus = False
        Me.tx_原価率下限.InputPlus = True
        Me.tx_原価率下限.InputZero = False
        Me.tx_原価率下限.Location = New System.Drawing.Point(28, 52)
        Me.tx_原価率下限.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.tx_原価率下限.MaxLength = 6
        Me.tx_原価率下限.Name = "tx_原価率下限"
        Me.tx_原価率下限.OldValue = "999.99"
        Me.tx_原価率下限.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_原価率下限.SelectText = True
        Me.tx_原価率下限.SelLength = 0
        Me.tx_原価率下限.SelStart = 0
        Me.tx_原価率下限.SelText = ""
        Me.tx_原価率下限.Size = New System.Drawing.Size(57, 22)
        Me.tx_原価率下限.TabIndex = 1
        Me.tx_原価率下限.Text = "999.99"
        '
        '_lb_項目_1
        '
        Me._lb_項目_1.BackColor = System.Drawing.SystemColors.Control
        Me._lb_項目_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_1.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_項目_1.Location = New System.Drawing.Point(92, 56)
        Me._lb_項目_1.Name = "_lb_項目_1"
        Me._lb_項目_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_1.Size = New System.Drawing.Size(195, 15)
        Me._lb_項目_1.TabIndex = 5
        Me._lb_項目_1.Text = "％以下はエラーとする。"
        '
        '_lb_項目_0
        '
        Me._lb_項目_0.BackColor = System.Drawing.SystemColors.Control
        Me._lb_項目_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_0.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_項目_0.Location = New System.Drawing.Point(92, 20)
        Me._lb_項目_0.Name = "_lb_項目_0"
        Me._lb_項目_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_0.Size = New System.Drawing.Size(195, 15)
        Me._lb_項目_0.TabIndex = 4
        Me._lb_項目_0.Text = "％以上はエラーとする。"
        '
        'SnwMT02F13
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.Cb中止
        Me.ClientSize = New System.Drawing.Size(331, 135)
        Me.Controls.Add(Me.tx_原価率上限)
        Me.Controls.Add(Me.Cb中止)
        Me.Controls.Add(Me.CdOK)
        Me.Controls.Add(Me.tx_原価率下限)
        Me.Controls.Add(Me._lb_項目_1)
        Me.Controls.Add(Me._lb_項目_0)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(184, 22)
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SnwMT02F13"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "原価割れ設定"
        Me.ResumeLayout(False)

    End Sub

End Class