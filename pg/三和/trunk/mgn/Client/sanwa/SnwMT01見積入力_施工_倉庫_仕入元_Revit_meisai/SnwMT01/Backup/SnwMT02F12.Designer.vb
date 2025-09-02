<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class SnwMT02F12
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
	Public WithEvents cbGET As System.Windows.Forms.Button
	Public WithEvents cb中止 As System.Windows.Forms.Button
	Public WithEvents _sb_Msg_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents sb_Msg As System.Windows.Forms.StatusStrip
	Public WithEvents tx_見積番号 As System.Windows.Forms.TextBox
	Public WithEvents tx_s行 As System.Windows.Forms.TextBox
	Public WithEvents tx_e行 As System.Windows.Forms.TextBox
	Public WithEvents _lb_項目_0 As System.Windows.Forms.Label
	Public WithEvents _lb_kara_1 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_3 As System.Windows.Forms.Label
	Public WithEvents _lin_Under_2 As Microsoft.VisualBasic.PowerPacks.LineShape
	Public WithEvents rf_取込行数 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_4 As System.Windows.Forms.Label
	Public WithEvents lb_kara As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents lb_項目 As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents lin_Under As LineShapeArray
	Public WithEvents ShapeContainer1 As Microsoft.VisualBasic.PowerPacks.ShapeContainer
	'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
	'Windows フォーム デザイナを使って変更できます。
	'コード エディタを使用して、変更しないでください。
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(SnwMT02F12))
		Me.components = New System.ComponentModel.Container()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
		Me.ShapeContainer1 = New Microsoft.VisualBasic.PowerPacks.ShapeContainer
		Me.cbGET = New System.Windows.Forms.Button
		Me.cb中止 = New System.Windows.Forms.Button
		Me.sb_Msg = New System.Windows.Forms.StatusStrip
		Me._sb_Msg_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
		Me.tx_見積番号 = New System.Windows.Forms.TextBox
		Me.tx_s行 = New System.Windows.Forms.TextBox
		Me.tx_e行 = New System.Windows.Forms.TextBox
		Me._lb_項目_0 = New System.Windows.Forms.Label
		Me._lb_kara_1 = New System.Windows.Forms.Label
		Me._lb_項目_3 = New System.Windows.Forms.Label
		Me._lin_Under_2 = New Microsoft.VisualBasic.PowerPacks.LineShape
		Me.rf_取込行数 = New System.Windows.Forms.Label
		Me._lb_項目_4 = New System.Windows.Forms.Label
		Me.lb_kara = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(components)
		Me.lb_項目 = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(components)
		Me.lin_Under = New LineShapeArray(components)
		Me.sb_Msg.SuspendLayout()
		Me.SuspendLayout()
		Me.ToolTip1.Active = True
		CType(Me.lb_kara, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lb_項目, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lin_Under, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.Text = "員数取込"
		Me.ClientSize = New System.Drawing.Size(292, 161)
		Me.Location = New System.Drawing.Point(3, 22)
		Me.Icon = CType(resources.GetObject("SnwMT02F12.Icon"), System.Drawing.Icon)
		Me.KeyPreview = True
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ControlBox = True
		Me.Enabled = True
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.HelpButton = False
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.Name = "SnwMT02F12"
		Me.cbGET.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cbGET.Text = "取込(&I)"
		Me.cbGET.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.cbGET.Size = New System.Drawing.Size(72, 22)
		Me.cbGET.Location = New System.Drawing.Point(124, 113)
		Me.cbGET.TabIndex = 3
		Me.cbGET.BackColor = System.Drawing.SystemColors.Control
		Me.cbGET.CausesValidation = True
		Me.cbGET.Enabled = True
		Me.cbGET.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cbGET.Cursor = System.Windows.Forms.Cursors.Default
		Me.cbGET.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cbGET.TabStop = True
		Me.cbGET.Name = "cbGET"
		Me.cb中止.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.CancelButton = Me.cb中止
		Me.cb中止.Text = "ｷｬﾝｾﾙ"
		Me.cb中止.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.cb中止.Size = New System.Drawing.Size(74, 22)
		Me.cb中止.Location = New System.Drawing.Point(204, 113)
		Me.cb中止.TabIndex = 4
		Me.cb中止.BackColor = System.Drawing.SystemColors.Control
		Me.cb中止.CausesValidation = True
		Me.cb中止.Enabled = True
		Me.cb中止.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cb中止.Cursor = System.Windows.Forms.Cursors.Default
		Me.cb中止.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cb中止.TabStop = True
		Me.cb中止.Name = "cb中止"
		Me.sb_Msg.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.sb_Msg.Size = New System.Drawing.Size(292, 20)
		Me.sb_Msg.Location = New System.Drawing.Point(0, 141)
		Me.sb_Msg.TabIndex = 5
		Me.sb_Msg.Name = "sb_Msg"
		Me._sb_Msg_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
		Me._sb_Msg_Panel1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
		Me._sb_Msg_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._sb_Msg_Panel1.Size = New System.Drawing.Size(291, 20)
		Me._sb_Msg_Panel1.Spring = True
		Me._sb_Msg_Panel1.AutoSize = True
		Me._sb_Msg_Panel1.BorderSides = CType(System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom, System.Windows.Forms.ToolStripStatusLabelBorderSides)
		Me._sb_Msg_Panel1.Margin = New System.Windows.Forms.Padding(0)
		Me._sb_Msg_Panel1.AutoSize = False
		Me.tx_見積番号.AutoSize = False
		Me.tx_見積番号.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_見積番号.Size = New System.Drawing.Size(63, 19)
		Me.tx_見積番号.Location = New System.Drawing.Point(108, 28)
		Me.tx_見積番号.TabIndex = 0
		Me.tx_見積番号.Text = ""
		Me.tx_見積番号.Maxlength = 7
		Me.tx_見積番号.AcceptsReturn = True
		Me.tx_見積番号.BackColor = System.Drawing.SystemColors.Window
		Me.tx_見積番号.CausesValidation = True
		Me.tx_見積番号.Enabled = True
		Me.tx_見積番号.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_見積番号.HideSelection = True
		Me.tx_見積番号.ReadOnly = False
		Me.tx_見積番号.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_見積番号.MultiLine = False
		Me.tx_見積番号.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_見積番号.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_見積番号.TabStop = True
		Me.tx_見積番号.Visible = True
		Me.tx_見積番号.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_見積番号.Name = "tx_見積番号"
		Me.tx_s行.AutoSize = False
		Me.tx_s行.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_s行.Size = New System.Drawing.Size(36, 19)
		Me.tx_s行.Location = New System.Drawing.Point(108, 52)
		Me.tx_s行.TabIndex = 1
		Me.tx_s行.Text = ""
		Me.tx_s行.Maxlength = 4
		Me.tx_s行.AcceptsReturn = True
		Me.tx_s行.BackColor = System.Drawing.SystemColors.Window
		Me.tx_s行.CausesValidation = True
		Me.tx_s行.Enabled = True
		Me.tx_s行.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_s行.HideSelection = True
		Me.tx_s行.ReadOnly = False
		Me.tx_s行.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_s行.MultiLine = False
		Me.tx_s行.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_s行.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_s行.TabStop = True
		Me.tx_s行.Visible = True
		Me.tx_s行.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_s行.Name = "tx_s行"
		Me.tx_e行.AutoSize = False
		Me.tx_e行.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_e行.Size = New System.Drawing.Size(36, 19)
		Me.tx_e行.Location = New System.Drawing.Point(161, 52)
		Me.tx_e行.TabIndex = 2
		Me.tx_e行.Text = ""
		Me.tx_e行.Maxlength = 4
		Me.tx_e行.AcceptsReturn = True
		Me.tx_e行.BackColor = System.Drawing.SystemColors.Window
		Me.tx_e行.CausesValidation = True
		Me.tx_e行.Enabled = True
		Me.tx_e行.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_e行.HideSelection = True
		Me.tx_e行.ReadOnly = False
		Me.tx_e行.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_e行.MultiLine = False
		Me.tx_e行.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_e行.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_e行.TabStop = True
		Me.tx_e行.Visible = True
		Me.tx_e行.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_e行.Name = "tx_e行"
		Me._lb_項目_0.Text = "行"
		Me._lb_項目_0.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_0.Size = New System.Drawing.Size(35, 13)
		Me._lb_項目_0.Location = New System.Drawing.Point(66, 54)
		Me._lb_項目_0.TabIndex = 10
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
		Me._lb_kara_1.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_kara_1.Text = "～"
		Me._lb_kara_1.Size = New System.Drawing.Size(17, 12)
		Me._lb_kara_1.Location = New System.Drawing.Point(144, 56)
		Me._lb_kara_1.TabIndex = 9
		Me._lb_kara_1.BackColor = System.Drawing.SystemColors.Control
		Me._lb_kara_1.Enabled = True
		Me._lb_kara_1.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_kara_1.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_kara_1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_kara_1.UseMnemonic = True
		Me._lb_kara_1.Visible = True
		Me._lb_kara_1.AutoSize = False
		Me._lb_kara_1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_kara_1.Name = "_lb_kara_1"
		Me._lb_項目_3.Text = "見積№"
		Me._lb_項目_3.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_3.Size = New System.Drawing.Size(59, 13)
		Me._lb_項目_3.Location = New System.Drawing.Point(42, 32)
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
		Me._lin_Under_2.X1 = 39
		Me._lin_Under_2.X2 = 182
		Me._lin_Under_2.Y1 = 100
		Me._lin_Under_2.Y2 = 100
		Me._lin_Under_2.BorderColor = System.Drawing.SystemColors.WindowText
		Me._lin_Under_2.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid
		Me._lin_Under_2.BorderWidth = 1
		Me._lin_Under_2.Visible = True
		Me._lin_Under_2.Name = "_lin_Under_2"
		Me.rf_取込行数.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.rf_取込行数.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.rf_取込行数.Size = New System.Drawing.Size(40, 17)
		Me.rf_取込行数.Location = New System.Drawing.Point(128, 84)
		Me.rf_取込行数.TabIndex = 7
		Me.rf_取込行数.BackColor = System.Drawing.SystemColors.Control
		Me.rf_取込行数.Enabled = True
		Me.rf_取込行数.ForeColor = System.Drawing.SystemColors.ControlText
		Me.rf_取込行数.Cursor = System.Windows.Forms.Cursors.Default
		Me.rf_取込行数.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.rf_取込行数.UseMnemonic = True
		Me.rf_取込行数.Visible = True
		Me.rf_取込行数.AutoSize = False
		Me.rf_取込行数.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.rf_取込行数.Name = "rf_取込行数"
		Me._lb_項目_4.Text = "取込行数"
		Me._lb_項目_4.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_4.Size = New System.Drawing.Size(77, 17)
		Me._lb_項目_4.Location = New System.Drawing.Point(42, 83)
		Me._lb_項目_4.TabIndex = 6
		Me._lb_項目_4.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lb_項目_4.BackColor = System.Drawing.SystemColors.Control
		Me._lb_項目_4.Enabled = True
		Me._lb_項目_4.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_項目_4.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_4.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_4.UseMnemonic = True
		Me._lb_項目_4.Visible = True
		Me._lb_項目_4.AutoSize = False
		Me._lb_項目_4.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_項目_4.Name = "_lb_項目_4"
		Me.Controls.Add(cbGET)
		Me.Controls.Add(cb中止)
		Me.Controls.Add(sb_Msg)
		Me.Controls.Add(tx_見積番号)
		Me.Controls.Add(tx_s行)
		Me.Controls.Add(tx_e行)
		Me.Controls.Add(_lb_項目_0)
		Me.Controls.Add(_lb_kara_1)
		Me.Controls.Add(_lb_項目_3)
		Me.ShapeContainer1.Shapes.Add(_lin_Under_2)
		Me.Controls.Add(rf_取込行数)
		Me.Controls.Add(_lb_項目_4)
		Me.Controls.Add(ShapeContainer1)
		Me.sb_Msg.Items.AddRange(New System.Windows.Forms.ToolStripItem(){Me._sb_Msg_Panel1})
		Me.lb_kara.SetIndex(_lb_kara_1, CType(1, Short))
		Me.lb_項目.SetIndex(_lb_項目_0, CType(0, Short))
		Me.lb_項目.SetIndex(_lb_項目_3, CType(3, Short))
		Me.lb_項目.SetIndex(_lb_項目_4, CType(4, Short))
		Me.lin_Under.SetIndex(_lin_Under_2, CType(2, Short))
		CType(Me.lin_Under, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lb_項目, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lb_kara, System.ComponentModel.ISupportInitialize).EndInit()
		Me.sb_Msg.ResumeLayout(False)
		Me.ResumeLayout(False)
		Me.PerformLayout()
	End Sub
#End Region 
End Class