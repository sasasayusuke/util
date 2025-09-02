<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class SnwMT02F02
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
	Public WithEvents cbNouki As System.Windows.Forms.Button
	Public WithEvents cbClose As System.Windows.Forms.Button
	Public WithEvents cbUpload As System.Windows.Forms.Button
	Public WithEvents _sb_Msg_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents sb_Msg As System.Windows.Forms.StatusStrip
	Public WithEvents tx_掛率 As System.Windows.Forms.TextBox
	Public WithEvents tx_見積番号 As System.Windows.Forms.TextBox
	Public WithEvents _lb_項目_3 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_0 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_1 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_2 As System.Windows.Forms.Label
	Public WithEvents rf_見積番号 As System.Windows.Forms.Label
	Public WithEvents _lin_Under_0 As Microsoft.VisualBasic.PowerPacks.LineShape
	Public WithEvents lb_項目 As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents lin_Under As LineShapeArray
	Public WithEvents ShapeContainer1 As Microsoft.VisualBasic.PowerPacks.ShapeContainer
	'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
	'Windows フォーム デザイナを使って変更できます。
	'コード エディタを使用して、変更しないでください。
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(SnwMT02F02))
		Me.components = New System.ComponentModel.Container()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
		Me.ShapeContainer1 = New Microsoft.VisualBasic.PowerPacks.ShapeContainer
		Me.cbGET = New System.Windows.Forms.Button
		Me.cbNouki = New System.Windows.Forms.Button
		Me.cbClose = New System.Windows.Forms.Button
		Me.cbUpload = New System.Windows.Forms.Button
		Me.sb_Msg = New System.Windows.Forms.StatusStrip
		Me._sb_Msg_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
		Me.tx_掛率 = New System.Windows.Forms.TextBox
		Me.tx_見積番号 = New System.Windows.Forms.TextBox
		Me._lb_項目_3 = New System.Windows.Forms.Label
		Me._lb_項目_0 = New System.Windows.Forms.Label
		Me._lb_項目_1 = New System.Windows.Forms.Label
		Me._lb_項目_2 = New System.Windows.Forms.Label
		Me.rf_見積番号 = New System.Windows.Forms.Label
		Me._lin_Under_0 = New Microsoft.VisualBasic.PowerPacks.LineShape
		Me.lb_項目 = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(components)
		Me.lin_Under = New LineShapeArray(components)
		Me.sb_Msg.SuspendLayout()
		Me.SuspendLayout()
		Me.ToolTip1.Active = True
		CType(Me.lb_項目, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lin_Under, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.Text = "仕分レベル設定"
		Me.ClientSize = New System.Drawing.Size(642, 391)
		Me.Location = New System.Drawing.Point(3, 22)
		Me.Icon = CType(resources.GetObject("SnwMT02F02.Icon"), System.Drawing.Icon)
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
		Me.Name = "SnwMT02F02"
		Me.cbGET.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cbGET.Text = "取込(&I)"
		Me.cbGET.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.cbGET.Size = New System.Drawing.Size(72, 22)
		Me.cbGET.Location = New System.Drawing.Point(536, 12)
		Me.cbGET.TabIndex = 12
		Me.cbGET.BackColor = System.Drawing.SystemColors.Control
		Me.cbGET.CausesValidation = True
		Me.cbGET.Enabled = True
		Me.cbGET.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cbGET.Cursor = System.Windows.Forms.Cursors.Default
		Me.cbGET.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cbGET.TabStop = True
		Me.cbGET.Name = "cbGET"
		Me.cbNouki.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cbNouki.Text = "納期一括"
		Me.cbNouki.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.cbNouki.Size = New System.Drawing.Size(72, 22)
		Me.cbNouki.Location = New System.Drawing.Point(16, 338)
		Me.cbNouki.TabIndex = 9
		Me.cbNouki.BackColor = System.Drawing.SystemColors.Control
		Me.cbNouki.CausesValidation = True
		Me.cbNouki.Enabled = True
		Me.cbNouki.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cbNouki.Cursor = System.Windows.Forms.Cursors.Default
		Me.cbNouki.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cbNouki.TabStop = True
		Me.cbNouki.Name = "cbNouki"
		Me.cbClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.CancelButton = Me.cbClose
		Me.cbClose.Text = "閉じる"
		Me.cbClose.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.cbClose.Size = New System.Drawing.Size(74, 22)
		Me.cbClose.Location = New System.Drawing.Point(494, 338)
		Me.cbClose.TabIndex = 2
		Me.cbClose.BackColor = System.Drawing.SystemColors.Control
		Me.cbClose.CausesValidation = True
		Me.cbClose.Enabled = True
		Me.cbClose.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cbClose.Cursor = System.Windows.Forms.Cursors.Default
		Me.cbClose.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cbClose.TabStop = True
		Me.cbClose.Name = "cbClose"
		Me.cbUpload.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cbUpload.Text = "登録(&S)"
		Me.cbUpload.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.cbUpload.Size = New System.Drawing.Size(72, 22)
		Me.cbUpload.Location = New System.Drawing.Point(414, 338)
		Me.cbUpload.TabIndex = 1
		Me.cbUpload.BackColor = System.Drawing.SystemColors.Control
		Me.cbUpload.CausesValidation = True
		Me.cbUpload.Enabled = True
		Me.cbUpload.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cbUpload.Cursor = System.Windows.Forms.Cursors.Default
		Me.cbUpload.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cbUpload.TabStop = True
		Me.cbUpload.Name = "cbUpload"
		Me.sb_Msg.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.sb_Msg.Size = New System.Drawing.Size(642, 20)
		Me.sb_Msg.Location = New System.Drawing.Point(0, 371)
		Me.sb_Msg.TabIndex = 3
		Me.sb_Msg.Name = "sb_Msg"
		Me._sb_Msg_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
		Me._sb_Msg_Panel1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
		Me._sb_Msg_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._sb_Msg_Panel1.Size = New System.Drawing.Size(642, 20)
		Me._sb_Msg_Panel1.Spring = True
		Me._sb_Msg_Panel1.AutoSize = True
		Me._sb_Msg_Panel1.BorderSides = CType(System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom, System.Windows.Forms.ToolStripStatusLabelBorderSides)
		Me._sb_Msg_Panel1.Margin = New System.Windows.Forms.Padding(0)
		Me._sb_Msg_Panel1.AutoSize = False
		Me.tx_掛率.AutoSize = False
		Me.tx_掛率.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_掛率.Size = New System.Drawing.Size(40, 19)
		Me.tx_掛率.Location = New System.Drawing.Point(117, 43)
		Me.tx_掛率.TabIndex = 6
		Me.tx_掛率.Text = "888"
		Me.tx_掛率.Maxlength = 3
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
		Me.tx_見積番号.AutoSize = False
		Me.tx_見積番号.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_見積番号.Size = New System.Drawing.Size(63, 19)
		Me.tx_見積番号.Location = New System.Drawing.Point(470, 12)
		Me.tx_見積番号.TabIndex = 10
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
		Me._lb_項目_3.Text = "見積№"
		Me._lb_項目_3.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_3.Size = New System.Drawing.Size(59, 13)
		Me._lb_項目_3.Location = New System.Drawing.Point(404, 16)
		Me._lb_項目_3.TabIndex = 11
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
		Me._lb_項目_0.BackColor = System.Drawing.Color.Transparent
		Me._lb_項目_0.Text = "定価の掛率"
		Me._lb_項目_0.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_0.ForeColor = System.Drawing.SystemColors.WindowText
		Me._lb_項目_0.Size = New System.Drawing.Size(77, 13)
		Me._lb_項目_0.Location = New System.Drawing.Point(28, 46)
		Me._lb_項目_0.TabIndex = 8
		Me._lb_項目_0.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lb_項目_0.Enabled = True
		Me._lb_項目_0.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_0.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_0.UseMnemonic = True
		Me._lb_項目_0.Visible = True
		Me._lb_項目_0.AutoSize = False
		Me._lb_項目_0.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_項目_0.Name = "_lb_項目_0"
		Me._lb_項目_1.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_項目_1.Text = "％"
		Me._lb_項目_1.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 11.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_1.Size = New System.Drawing.Size(20, 17)
		Me._lb_項目_1.Location = New System.Drawing.Point(161, 45)
		Me._lb_項目_1.TabIndex = 7
		Me._lb_項目_1.BackColor = System.Drawing.Color.Transparent
		Me._lb_項目_1.Enabled = True
		Me._lb_項目_1.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_項目_1.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_1.UseMnemonic = True
		Me._lb_項目_1.Visible = True
		Me._lb_項目_1.AutoSize = False
		Me._lb_項目_1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_項目_1.Name = "_lb_項目_1"
		Me._lb_項目_2.Text = "見積№"
		Me._lb_項目_2.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_2.Size = New System.Drawing.Size(77, 13)
		Me._lb_項目_2.Location = New System.Drawing.Point(28, 12)
		Me._lb_項目_2.TabIndex = 5
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
		Me.rf_見積番号.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.rf_見積番号.Text = "21755009"
		Me.rf_見積番号.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.rf_見積番号.Size = New System.Drawing.Size(67, 13)
		Me.rf_見積番号.Location = New System.Drawing.Point(120, 12)
		Me.rf_見積番号.TabIndex = 4
		Me.rf_見積番号.BackColor = System.Drawing.SystemColors.Control
		Me.rf_見積番号.Enabled = True
		Me.rf_見積番号.ForeColor = System.Drawing.SystemColors.ControlText
		Me.rf_見積番号.Cursor = System.Windows.Forms.Cursors.Default
		Me.rf_見積番号.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.rf_見積番号.UseMnemonic = True
		Me.rf_見積番号.Visible = True
		Me.rf_見積番号.AutoSize = False
		Me.rf_見積番号.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.rf_見積番号.Name = "rf_見積番号"
		Me._lin_Under_0.X1 = 24
		Me._lin_Under_0.X2 = 238
		Me._lin_Under_0.Y1 = 28
		Me._lin_Under_0.Y2 = 28
		Me._lin_Under_0.BorderColor = System.Drawing.SystemColors.WindowText
		Me._lin_Under_0.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid
		Me._lin_Under_0.BorderWidth = 1
		Me._lin_Under_0.Visible = True
		Me._lin_Under_0.Name = "_lin_Under_0"
		Me.Controls.Add(cbGET)
		Me.Controls.Add(cbNouki)
		Me.Controls.Add(cbClose)
		Me.Controls.Add(cbUpload)
		Me.Controls.Add(sb_Msg)
		Me.Controls.Add(tx_掛率)
		Me.Controls.Add(tx_見積番号)
		Me.Controls.Add(_lb_項目_3)
		Me.Controls.Add(_lb_項目_0)
		Me.Controls.Add(_lb_項目_1)
		Me.Controls.Add(_lb_項目_2)
		Me.Controls.Add(rf_見積番号)
		Me.ShapeContainer1.Shapes.Add(_lin_Under_0)
		Me.Controls.Add(ShapeContainer1)
		Me.sb_Msg.Items.AddRange(New System.Windows.Forms.ToolStripItem(){Me._sb_Msg_Panel1})
		Me.lb_項目.SetIndex(_lb_項目_3, CType(3, Short))
		Me.lb_項目.SetIndex(_lb_項目_0, CType(0, Short))
		Me.lb_項目.SetIndex(_lb_項目_1, CType(1, Short))
		Me.lb_項目.SetIndex(_lb_項目_2, CType(2, Short))
		Me.lin_Under.SetIndex(_lin_Under_0, CType(0, Short))
		CType(Me.lin_Under, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lb_項目, System.ComponentModel.ISupportInitialize).EndInit()
		Me.sb_Msg.ResumeLayout(False)
		Me.ResumeLayout(False)
		Me.PerformLayout()
	End Sub
#End Region 
End Class