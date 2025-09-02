<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class SnwMT02F11
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
	Public WithEvents _Ob_条件_0 As System.Windows.Forms.RadioButton
	Public WithEvents _Ob_条件_1 As System.Windows.Forms.RadioButton
	Public WithEvents pic_印刷条件 As System.Windows.Forms.Panel
	Public WithEvents tx_掛率 As System.Windows.Forms.TextBox
	Public WithEvents cb中止 As System.Windows.Forms.Button
	Public WithEvents cdOK As System.Windows.Forms.Button
	Public WithEvents _lb_項目_0 As System.Windows.Forms.Label
	Public WithEvents _lin_Under_0 As Microsoft.VisualBasic.PowerPacks.LineShape
	Public WithEvents _lb_項目_5 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_2 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_3 As System.Windows.Forms.Label
	Public WithEvents rf_売価 As System.Windows.Forms.Label
	Public WithEvents rf_原価 As System.Windows.Forms.Label
	Public WithEvents Ob_条件 As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
	Public WithEvents lb_項目 As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents lin_Under As LineShapeArray
	Public WithEvents ShapeContainer1 As Microsoft.VisualBasic.PowerPacks.ShapeContainer
	'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
	'Windows フォーム デザイナを使って変更できます。
	'コード エディタを使用して、変更しないでください。
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(SnwMT02F11))
		Me.components = New System.ComponentModel.Container()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
		Me.ShapeContainer1 = New Microsoft.VisualBasic.PowerPacks.ShapeContainer
		Me.pic_印刷条件 = New System.Windows.Forms.Panel
		Me._Ob_条件_0 = New System.Windows.Forms.RadioButton
		Me._Ob_条件_1 = New System.Windows.Forms.RadioButton
		Me.tx_掛率 = New System.Windows.Forms.TextBox
		Me.cb中止 = New System.Windows.Forms.Button
		Me.cdOK = New System.Windows.Forms.Button
		Me._lb_項目_0 = New System.Windows.Forms.Label
		Me._lin_Under_0 = New Microsoft.VisualBasic.PowerPacks.LineShape
		Me._lb_項目_5 = New System.Windows.Forms.Label
		Me._lb_項目_2 = New System.Windows.Forms.Label
		Me._lb_項目_3 = New System.Windows.Forms.Label
		Me.rf_売価 = New System.Windows.Forms.Label
		Me.rf_原価 = New System.Windows.Forms.Label
		Me.Ob_条件 = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(components)
		Me.lb_項目 = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(components)
		Me.lin_Under = New LineShapeArray(components)
		Me.pic_印刷条件.SuspendLayout()
		Me.SuspendLayout()
		Me.ToolTip1.Active = True
		CType(Me.Ob_条件, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lb_項目, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lin_Under, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.Text = "売価変更"
		Me.ClientSize = New System.Drawing.Size(262, 173)
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
		Me.Name = "SnwMT02F11"
		Me.pic_印刷条件.ForeColor = System.Drawing.SystemColors.WindowText
		Me.pic_印刷条件.Size = New System.Drawing.Size(183, 24)
		Me.pic_印刷条件.Location = New System.Drawing.Point(40, 12)
		Me.pic_印刷条件.TabIndex = 0
		Me.pic_印刷条件.Dock = System.Windows.Forms.DockStyle.None
		Me.pic_印刷条件.BackColor = System.Drawing.SystemColors.Control
		Me.pic_印刷条件.CausesValidation = True
		Me.pic_印刷条件.Enabled = True
		Me.pic_印刷条件.Cursor = System.Windows.Forms.Cursors.Default
		Me.pic_印刷条件.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.pic_印刷条件.TabStop = True
		Me.pic_印刷条件.Visible = True
		Me.pic_印刷条件.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.pic_印刷条件.Name = "pic_印刷条件"
		Me._Ob_条件_0.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._Ob_条件_0.Text = "掛率"
		Me._Ob_条件_0.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._Ob_条件_0.Size = New System.Drawing.Size(72, 20)
		Me._Ob_条件_0.Location = New System.Drawing.Point(0, 0)
		Me._Ob_条件_0.TabIndex = 1
		Me._Ob_条件_0.Checked = True
		Me._Ob_条件_0.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._Ob_条件_0.BackColor = System.Drawing.SystemColors.Control
		Me._Ob_条件_0.CausesValidation = True
		Me._Ob_条件_0.Enabled = True
		Me._Ob_条件_0.ForeColor = System.Drawing.SystemColors.ControlText
		Me._Ob_条件_0.Cursor = System.Windows.Forms.Cursors.Default
		Me._Ob_条件_0.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._Ob_条件_0.Appearance = System.Windows.Forms.Appearance.Normal
		Me._Ob_条件_0.TabStop = True
		Me._Ob_条件_0.Visible = True
		Me._Ob_条件_0.Name = "_Ob_条件_0"
		Me._Ob_条件_1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._Ob_条件_1.Text = "原価率"
		Me._Ob_条件_1.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._Ob_条件_1.Size = New System.Drawing.Size(76, 20)
		Me._Ob_条件_1.Location = New System.Drawing.Point(88, 0)
		Me._Ob_条件_1.TabIndex = 2
		Me._Ob_条件_1.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._Ob_条件_1.BackColor = System.Drawing.SystemColors.Control
		Me._Ob_条件_1.CausesValidation = True
		Me._Ob_条件_1.Enabled = True
		Me._Ob_条件_1.ForeColor = System.Drawing.SystemColors.ControlText
		Me._Ob_条件_1.Cursor = System.Windows.Forms.Cursors.Default
		Me._Ob_条件_1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._Ob_条件_1.Appearance = System.Windows.Forms.Appearance.Normal
		Me._Ob_条件_1.TabStop = True
		Me._Ob_条件_1.Checked = False
		Me._Ob_条件_1.Visible = True
		Me._Ob_条件_1.Name = "_Ob_条件_1"
		Me.tx_掛率.AutoSize = False
		Me.tx_掛率.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_掛率.Size = New System.Drawing.Size(57, 19)
		Me.tx_掛率.Location = New System.Drawing.Point(140, 72)
		Me.tx_掛率.TabIndex = 3
		Me.tx_掛率.Text = "999.99"
		Me.tx_掛率.Maxlength = 6
		Me.tx_掛率.AcceptsReturn = True
		Me.tx_掛率.BackColor = System.Drawing.SystemColors.Window
		Me.tx_掛率.CausesValidation = True
		Me.tx_掛率.Enabled = True
		Me.tx_掛率.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_掛率.HideSelection = True
		Me.tx_掛率.ReadOnly = False
		Me.tx_掛率.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_掛率.MultiLine = False
		Me.tx_掛率.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_掛率.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_掛率.TabStop = True
		Me.tx_掛率.Visible = True
		Me.tx_掛率.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_掛率.Name = "tx_掛率"
		Me.cb中止.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.CancelButton = Me.cb中止
		Me.cb中止.Text = "ｷｬﾝｾﾙ"
		Me.cb中止.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.cb中止.Size = New System.Drawing.Size(74, 22)
		Me.cb中止.Location = New System.Drawing.Point(180, 140)
		Me.cb中止.TabIndex = 5
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
		Me.cdOK.Location = New System.Drawing.Point(96, 140)
		Me.cdOK.TabIndex = 4
		Me.cdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cdOK.CausesValidation = True
		Me.cdOK.Enabled = True
		Me.cdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cdOK.TabStop = True
		Me.cdOK.Name = "cdOK"
		Me._lb_項目_0.Text = "％"
		Me._lb_項目_0.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_0.Size = New System.Drawing.Size(19, 13)
		Me._lb_項目_0.Location = New System.Drawing.Point(204, 76)
		Me._lb_項目_0.TabIndex = 11
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
		Me._lin_Under_0.X1 = 28
		Me._lin_Under_0.X2 = 224
		Me._lin_Under_0.Y1 = 96
		Me._lin_Under_0.Y2 = 96
		Me._lin_Under_0.BorderColor = System.Drawing.SystemColors.WindowText
		Me._lin_Under_0.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid
		Me._lin_Under_0.BorderWidth = 1
		Me._lin_Under_0.Visible = True
		Me._lin_Under_0.Name = "_lin_Under_0"
		Me._lb_項目_5.Text = "掛  率"
		Me._lb_項目_5.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_5.Size = New System.Drawing.Size(47, 13)
		Me._lb_項目_5.Location = New System.Drawing.Point(40, 76)
		Me._lb_項目_5.TabIndex = 10
		Me._lb_項目_5.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lb_項目_5.BackColor = System.Drawing.SystemColors.Control
		Me._lb_項目_5.Enabled = True
		Me._lb_項目_5.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_項目_5.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_5.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_5.UseMnemonic = True
		Me._lb_項目_5.Visible = True
		Me._lb_項目_5.AutoSize = False
		Me._lb_項目_5.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_項目_5.Name = "_lb_項目_5"
		Me._lb_項目_2.Text = "原  価"
		Me._lb_項目_2.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_2.Size = New System.Drawing.Size(47, 13)
		Me._lb_項目_2.Location = New System.Drawing.Point(40, 56)
		Me._lb_項目_2.TabIndex = 9
		Me._lb_項目_2.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lb_項目_2.BackColor = System.Drawing.SystemColors.Control
		Me._lb_項目_2.Enabled = True
		Me._lb_項目_2.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_項目_2.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_2.UseMnemonic = True
		Me._lb_項目_2.Visible = True
		Me._lb_項目_2.AutoSize = False
		Me._lb_項目_2.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_項目_2.Name = "_lb_項目_2"
		Me._lb_項目_3.Text = "売  価"
		Me._lb_項目_3.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_3.Size = New System.Drawing.Size(47, 13)
		Me._lb_項目_3.Location = New System.Drawing.Point(40, 108)
		Me._lb_項目_3.TabIndex = 8
		Me._lb_項目_3.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lb_項目_3.BackColor = System.Drawing.SystemColors.Control
		Me._lb_項目_3.Enabled = True
		Me._lb_項目_3.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_項目_3.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_3.UseMnemonic = True
		Me._lb_項目_3.Visible = True
		Me._lb_項目_3.AutoSize = False
		Me._lb_項目_3.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_項目_3.Name = "_lb_項目_3"
		Me.rf_売価.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.rf_売価.BackColor = System.Drawing.SystemColors.Window
		Me.rf_売価.Text = "99,999,999.99"
		Me.rf_売価.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.rf_売価.Size = New System.Drawing.Size(99, 19)
		Me.rf_売価.Location = New System.Drawing.Point(98, 104)
		Me.rf_売価.TabIndex = 7
		Me.rf_売価.Enabled = True
		Me.rf_売価.ForeColor = System.Drawing.SystemColors.ControlText
		Me.rf_売価.Cursor = System.Windows.Forms.Cursors.Default
		Me.rf_売価.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.rf_売価.UseMnemonic = True
		Me.rf_売価.Visible = True
		Me.rf_売価.AutoSize = False
		Me.rf_売価.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.rf_売価.Name = "rf_売価"
		Me.rf_原価.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.rf_原価.BackColor = System.Drawing.SystemColors.Window
		Me.rf_原価.Text = "99,999,999.99"
		Me.rf_原価.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.rf_原価.Size = New System.Drawing.Size(99, 19)
		Me.rf_原価.Location = New System.Drawing.Point(98, 52)
		Me.rf_原価.TabIndex = 6
		Me.rf_原価.Enabled = True
		Me.rf_原価.ForeColor = System.Drawing.SystemColors.ControlText
		Me.rf_原価.Cursor = System.Windows.Forms.Cursors.Default
		Me.rf_原価.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.rf_原価.UseMnemonic = True
		Me.rf_原価.Visible = True
		Me.rf_原価.AutoSize = False
		Me.rf_原価.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.rf_原価.Name = "rf_原価"
		Me.Controls.Add(pic_印刷条件)
		Me.Controls.Add(tx_掛率)
		Me.Controls.Add(cb中止)
		Me.Controls.Add(cdOK)
		Me.Controls.Add(_lb_項目_0)
		Me.ShapeContainer1.Shapes.Add(_lin_Under_0)
		Me.Controls.Add(_lb_項目_5)
		Me.Controls.Add(_lb_項目_2)
		Me.Controls.Add(_lb_項目_3)
		Me.Controls.Add(rf_売価)
		Me.Controls.Add(rf_原価)
		Me.Controls.Add(ShapeContainer1)
		Me.pic_印刷条件.Controls.Add(_Ob_条件_0)
		Me.pic_印刷条件.Controls.Add(_Ob_条件_1)
		Me.Ob_条件.SetIndex(_Ob_条件_0, CType(0, Short))
		Me.Ob_条件.SetIndex(_Ob_条件_1, CType(1, Short))
		Me.lb_項目.SetIndex(_lb_項目_0, CType(0, Short))
		Me.lb_項目.SetIndex(_lb_項目_5, CType(5, Short))
		Me.lb_項目.SetIndex(_lb_項目_2, CType(2, Short))
		Me.lb_項目.SetIndex(_lb_項目_3, CType(3, Short))
		Me.lin_Under.SetIndex(_lin_Under_0, CType(0, Short))
		CType(Me.lin_Under, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lb_項目, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Ob_条件, System.ComponentModel.ISupportInitialize).EndInit()
		Me.pic_印刷条件.ResumeLayout(False)
		Me.ResumeLayout(False)
		Me.PerformLayout()
	End Sub
#End Region 
End Class