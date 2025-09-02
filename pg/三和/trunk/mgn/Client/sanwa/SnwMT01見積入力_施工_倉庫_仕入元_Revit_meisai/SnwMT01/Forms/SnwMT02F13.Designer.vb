<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class SnwMT02F13
#Region "Windows フォーム デザイナによって生成されたコード "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
		'この呼び出しは、Windows フォーム デザイナで必要です。
		InitializeComponent()
	End Sub
	'Form は、コンポーネント一覧に後処理を実行するために dispose をオーバーライドします。
	<System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			If Not components Is Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Windows フォーム デザイナで必要です。
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents tx_原価率上限 As System.Windows.Forms.TextBox
	Public WithEvents cb中止 As System.Windows.Forms.Button
	Public WithEvents cdOK As System.Windows.Forms.Button
	Public WithEvents tx_原価率下限 As System.Windows.Forms.TextBox
	Public WithEvents _lb_項目_1 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_0 As System.Windows.Forms.Label
	Public WithEvents lb_項目 As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
	'Windows フォーム デザイナを使って変更できます。
	'コード エディタを使用して、変更しないでください。
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(SnwMT02F13))
		Me.components = New System.ComponentModel.Container()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
		Me.tx_原価率上限 = New System.Windows.Forms.TextBox
		Me.cb中止 = New System.Windows.Forms.Button
		Me.cdOK = New System.Windows.Forms.Button
		Me.tx_原価率下限 = New System.Windows.Forms.TextBox
		Me._lb_項目_1 = New System.Windows.Forms.Label
		Me._lb_項目_0 = New System.Windows.Forms.Label
		Me.lb_項目 = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(components)
		Me.SuspendLayout()
		Me.ToolTip1.Active = True
		CType(Me.lb_項目, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.Text = "原価割れ設定"
		Me.ClientSize = New System.Drawing.Size(262, 128)
		Me.Location = New System.Drawing.Point(184, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ControlBox = True
		Me.Enabled = True
		Me.KeyPreview = False
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.HelpButton = False
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.Name = "SnwMT02F13"
		Me.tx_原価率上限.AutoSize = False
		Me.tx_原価率上限.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_原価率上限.Size = New System.Drawing.Size(57, 19)
		Me.tx_原価率上限.Location = New System.Drawing.Point(28, 16)
		Me.tx_原価率上限.TabIndex = 0
		Me.tx_原価率上限.Text = "999.99"
		Me.tx_原価率上限.Maxlength = 6
		Me.tx_原価率上限.AcceptsReturn = True
		Me.tx_原価率上限.BackColor = System.Drawing.SystemColors.Window
		Me.tx_原価率上限.CausesValidation = True
		Me.tx_原価率上限.Enabled = True
		Me.tx_原価率上限.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_原価率上限.HideSelection = True
		Me.tx_原価率上限.ReadOnly = False
		Me.tx_原価率上限.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_原価率上限.MultiLine = False
		Me.tx_原価率上限.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_原価率上限.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_原価率上限.TabStop = True
		Me.tx_原価率上限.Visible = True
		Me.tx_原価率上限.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_原価率上限.Name = "tx_原価率上限"
		Me.cb中止.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.CancelButton = Me.cb中止
		Me.cb中止.Text = "ｷｬﾝｾﾙ"
		Me.cb中止.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.cb中止.Size = New System.Drawing.Size(74, 22)
		Me.cb中止.Location = New System.Drawing.Point(180, 92)
		Me.cb中止.TabIndex = 3
		Me.cb中止.BackColor = System.Drawing.SystemColors.Control
		Me.cb中止.CausesValidation = True
		Me.cb中止.Enabled = True
		Me.cb中止.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cb中止.Cursor = System.Windows.Forms.Cursors.Default
		Me.cb中止.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cb中止.TabStop = True
		Me.cb中止.Name = "cb中止"
		Me.cdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cdOK.Text = "OK"
		Me.cdOK.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.cdOK.Size = New System.Drawing.Size(74, 22)
		Me.cdOK.Location = New System.Drawing.Point(96, 92)
		Me.cdOK.TabIndex = 2
		Me.cdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cdOK.CausesValidation = True
		Me.cdOK.Enabled = True
		Me.cdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cdOK.TabStop = True
		Me.cdOK.Name = "cdOK"
		Me.tx_原価率下限.AutoSize = False
		Me.tx_原価率下限.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_原価率下限.Size = New System.Drawing.Size(57, 19)
		Me.tx_原価率下限.Location = New System.Drawing.Point(28, 52)
		Me.tx_原価率下限.TabIndex = 1
		Me.tx_原価率下限.Text = "999.99"
		Me.tx_原価率下限.Maxlength = 6
		Me.tx_原価率下限.AcceptsReturn = True
		Me.tx_原価率下限.BackColor = System.Drawing.SystemColors.Window
		Me.tx_原価率下限.CausesValidation = True
		Me.tx_原価率下限.Enabled = True
		Me.tx_原価率下限.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_原価率下限.HideSelection = True
		Me.tx_原価率下限.ReadOnly = False
		Me.tx_原価率下限.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_原価率下限.MultiLine = False
		Me.tx_原価率下限.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_原価率下限.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_原価率下限.TabStop = True
		Me.tx_原価率下限.Visible = True
		Me.tx_原価率下限.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_原価率下限.Name = "tx_原価率下限"
		Me._lb_項目_1.Text = "％以下はエラーとする。"
		Me._lb_項目_1.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_1.Size = New System.Drawing.Size(143, 13)
		Me._lb_項目_1.Location = New System.Drawing.Point(92, 56)
		Me._lb_項目_1.TabIndex = 5
		Me._lb_項目_1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lb_項目_1.BackColor = System.Drawing.SystemColors.Control
		Me._lb_項目_1.Enabled = True
		Me._lb_項目_1.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_項目_1.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_1.UseMnemonic = True
		Me._lb_項目_1.Visible = True
		Me._lb_項目_1.AutoSize = False
		Me._lb_項目_1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_項目_1.Name = "_lb_項目_1"
		Me._lb_項目_0.Text = "％以上はエラーとする。"
		Me._lb_項目_0.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_0.Size = New System.Drawing.Size(139, 13)
		Me._lb_項目_0.Location = New System.Drawing.Point(92, 20)
		Me._lb_項目_0.TabIndex = 4
		Me._lb_項目_0.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lb_項目_0.BackColor = System.Drawing.SystemColors.Control
		Me._lb_項目_0.Enabled = True
		Me._lb_項目_0.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_項目_0.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_0.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_0.UseMnemonic = True
		Me._lb_項目_0.Visible = True
		Me._lb_項目_0.AutoSize = False
		Me._lb_項目_0.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_項目_0.Name = "_lb_項目_0"
		Me.Controls.Add(tx_原価率上限)
		Me.Controls.Add(cb中止)
		Me.Controls.Add(cdOK)
		Me.Controls.Add(tx_原価率下限)
		Me.Controls.Add(_lb_項目_1)
		Me.Controls.Add(_lb_項目_0)
		Me.lb_項目.SetIndex(_lb_項目_1, CType(1, Short))
		Me.lb_項目.SetIndex(_lb_項目_0, CType(0, Short))
		CType(Me.lb_項目, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()
	End Sub
#End Region 
End Class