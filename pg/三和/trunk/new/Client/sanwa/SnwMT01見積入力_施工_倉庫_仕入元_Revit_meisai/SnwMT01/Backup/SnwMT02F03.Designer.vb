<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class SnwMT02F03
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
	Public WithEvents cb変更 As System.Windows.Forms.Button
	Public WithEvents txDir As System.Windows.Forms.TextBox
	Public WithEvents Picture1 As System.Windows.Forms.Panel
	Public WithEvents cbXLS As System.Windows.Forms.Button
	Public WithEvents cb中止 As System.Windows.Forms.Button
	Public WithEvents _sb_Msg_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents sb_Msg As System.Windows.Forms.StatusStrip
	Public WithEvents tx_FromNo As System.Windows.Forms.TextBox
	Public WithEvents tx_ToNo As System.Windows.Forms.TextBox
	Public WithEvents _lin_Under_0 As Microsoft.VisualBasic.PowerPacks.LineShape
	Public WithEvents rf_見積番号 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_2 As System.Windows.Forms.Label
	Public WithEvents lb_番号指定 As System.Windows.Forms.Label
	Public WithEvents lb_kara As System.Windows.Forms.Label
	Public WithEvents _lb_項目_5 As System.Windows.Forms.Label
	Public WithEvents _Shape1_2 As Microsoft.VisualBasic.PowerPacks.RectangleShape
	Public WithEvents _Shape1_3 As Microsoft.VisualBasic.PowerPacks.RectangleShape
	Public WithEvents _Shape1_0 As Microsoft.VisualBasic.PowerPacks.RectangleShape
	Public WithEvents _Shape1_1 As Microsoft.VisualBasic.PowerPacks.RectangleShape
	Public WithEvents lb_項目 As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents lin_Under As LineShapeArray
	Public WithEvents Shape1 As RectangleShapeArray
	Public WithEvents ShapeContainer1 As Microsoft.VisualBasic.PowerPacks.ShapeContainer
	'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
	'Windows フォーム デザイナを使って変更できます。
	'コード エディタを使用して、変更しないでください。
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(SnwMT02F03))
		Me.components = New System.ComponentModel.Container()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
		Me.ShapeContainer1 = New Microsoft.VisualBasic.PowerPacks.ShapeContainer
		Me.cb変更 = New System.Windows.Forms.Button
		Me.Picture1 = New System.Windows.Forms.Panel
		Me.txDir = New System.Windows.Forms.TextBox
		Me.cbXLS = New System.Windows.Forms.Button
		Me.cb中止 = New System.Windows.Forms.Button
		Me.sb_Msg = New System.Windows.Forms.StatusStrip
		Me._sb_Msg_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
		Me.tx_FromNo = New System.Windows.Forms.TextBox
		Me.tx_ToNo = New System.Windows.Forms.TextBox
		Me._lin_Under_0 = New Microsoft.VisualBasic.PowerPacks.LineShape
		Me.rf_見積番号 = New System.Windows.Forms.Label
		Me._lb_項目_2 = New System.Windows.Forms.Label
		Me.lb_番号指定 = New System.Windows.Forms.Label
		Me.lb_kara = New System.Windows.Forms.Label
		Me._lb_項目_5 = New System.Windows.Forms.Label
		Me._Shape1_2 = New Microsoft.VisualBasic.PowerPacks.RectangleShape
		Me._Shape1_3 = New Microsoft.VisualBasic.PowerPacks.RectangleShape
		Me._Shape1_0 = New Microsoft.VisualBasic.PowerPacks.RectangleShape
		Me._Shape1_1 = New Microsoft.VisualBasic.PowerPacks.RectangleShape
		Me.lb_項目 = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(components)
		Me.lin_Under = New LineShapeArray(components)
		Me.Shape1 = New RectangleShapeArray(components)
		Me.Picture1.SuspendLayout()
		Me.sb_Msg.SuspendLayout()
		Me.SuspendLayout()
		Me.ToolTip1.Active = True
		CType(Me.lb_項目, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lin_Under, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Shape1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.Text = "員数シート出力"
		Me.ClientSize = New System.Drawing.Size(456, 243)
		Me.Location = New System.Drawing.Point(3, 22)
		Me.Icon = CType(resources.GetObject("SnwMT02F03.Icon"), System.Drawing.Icon)
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
		Me.Name = "SnwMT02F03"
		Me.cb変更.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cb変更.Text = "フォルダ(&D)"
		Me.cb変更.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.cb変更.Size = New System.Drawing.Size(74, 22)
		Me.cb変更.Location = New System.Drawing.Point(324, 149)
		Me.cb変更.TabIndex = 4
		Me.cb変更.BackColor = System.Drawing.SystemColors.Control
		Me.cb変更.CausesValidation = True
		Me.cb変更.Enabled = True
		Me.cb変更.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cb変更.Cursor = System.Windows.Forms.Cursors.Default
		Me.cb変更.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cb変更.TabStop = True
		Me.cb変更.Name = "cb変更"
		Me.Picture1.Enabled = False
		Me.Picture1.ForeColor = System.Drawing.SystemColors.WindowText
		Me.Picture1.Size = New System.Drawing.Size(249, 19)
		Me.Picture1.Location = New System.Drawing.Point(50, 149)
		Me.Picture1.TabIndex = 7
		Me.Picture1.Dock = System.Windows.Forms.DockStyle.None
		Me.Picture1.BackColor = System.Drawing.SystemColors.Control
		Me.Picture1.CausesValidation = True
		Me.Picture1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Picture1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Picture1.TabStop = True
		Me.Picture1.Visible = True
		Me.Picture1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Picture1.Name = "Picture1"
		Me.txDir.AutoSize = False
		Me.txDir.BackColor = System.Drawing.SystemColors.Control
		Me.txDir.Size = New System.Drawing.Size(245, 19)
		Me.txDir.Location = New System.Drawing.Point(0, 0)
		Me.txDir.Maxlength = 40
		Me.txDir.TabIndex = 8
		Me.txDir.AcceptsReturn = True
		Me.txDir.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txDir.CausesValidation = True
		Me.txDir.Enabled = True
		Me.txDir.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txDir.HideSelection = True
		Me.txDir.ReadOnly = False
		Me.txDir.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txDir.MultiLine = False
		Me.txDir.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txDir.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txDir.TabStop = True
		Me.txDir.Visible = True
		Me.txDir.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.txDir.Name = "txDir"
		Me.cbXLS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cbXLS.Text = "出力(&O)"
		Me.cbXLS.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.cbXLS.Size = New System.Drawing.Size(72, 22)
		Me.cbXLS.Location = New System.Drawing.Point(284, 192)
		Me.cbXLS.TabIndex = 2
		Me.cbXLS.BackColor = System.Drawing.SystemColors.Control
		Me.cbXLS.CausesValidation = True
		Me.cbXLS.Enabled = True
		Me.cbXLS.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cbXLS.Cursor = System.Windows.Forms.Cursors.Default
		Me.cbXLS.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cbXLS.TabStop = True
		Me.cbXLS.Name = "cbXLS"
		Me.cb中止.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.CancelButton = Me.cb中止
		Me.cb中止.Text = "ｷｬﾝｾﾙ"
		Me.cb中止.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.cb中止.Size = New System.Drawing.Size(74, 22)
		Me.cb中止.Location = New System.Drawing.Point(364, 192)
		Me.cb中止.TabIndex = 3
		Me.cb中止.BackColor = System.Drawing.SystemColors.Control
		Me.cb中止.CausesValidation = True
		Me.cb中止.Enabled = True
		Me.cb中止.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cb中止.Cursor = System.Windows.Forms.Cursors.Default
		Me.cb中止.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cb中止.TabStop = True
		Me.cb中止.Name = "cb中止"
		Me.sb_Msg.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.sb_Msg.Size = New System.Drawing.Size(456, 20)
		Me.sb_Msg.Location = New System.Drawing.Point(0, 223)
		Me.sb_Msg.TabIndex = 5
		Me.sb_Msg.Name = "sb_Msg"
		Me._sb_Msg_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
		Me._sb_Msg_Panel1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
		Me._sb_Msg_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._sb_Msg_Panel1.Size = New System.Drawing.Size(455, 20)
		Me._sb_Msg_Panel1.Spring = True
		Me._sb_Msg_Panel1.AutoSize = True
		Me._sb_Msg_Panel1.BorderSides = CType(System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom, System.Windows.Forms.ToolStripStatusLabelBorderSides)
		Me._sb_Msg_Panel1.Margin = New System.Windows.Forms.Padding(0)
		Me._sb_Msg_Panel1.AutoSize = False
		Me.tx_FromNo.AutoSize = False
		Me.tx_FromNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_FromNo.Size = New System.Drawing.Size(33, 19)
		Me.tx_FromNo.Location = New System.Drawing.Point(116, 67)
		Me.tx_FromNo.TabIndex = 0
		Me.tx_FromNo.Text = ""
		Me.tx_FromNo.Maxlength = 2
		Me.tx_FromNo.AcceptsReturn = True
		Me.tx_FromNo.BackColor = System.Drawing.SystemColors.Window
		Me.tx_FromNo.CausesValidation = True
		Me.tx_FromNo.Enabled = True
		Me.tx_FromNo.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_FromNo.HideSelection = True
		Me.tx_FromNo.ReadOnly = False
		Me.tx_FromNo.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_FromNo.MultiLine = False
		Me.tx_FromNo.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_FromNo.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_FromNo.TabStop = True
		Me.tx_FromNo.Visible = True
		Me.tx_FromNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_FromNo.Name = "tx_FromNo"
		Me.tx_ToNo.AutoSize = False
		Me.tx_ToNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_ToNo.Size = New System.Drawing.Size(33, 19)
		Me.tx_ToNo.Location = New System.Drawing.Point(179, 67)
		Me.tx_ToNo.TabIndex = 1
		Me.tx_ToNo.Text = ""
		Me.tx_ToNo.Maxlength = 2
		Me.tx_ToNo.AcceptsReturn = True
		Me.tx_ToNo.BackColor = System.Drawing.SystemColors.Window
		Me.tx_ToNo.CausesValidation = True
		Me.tx_ToNo.Enabled = True
		Me.tx_ToNo.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_ToNo.HideSelection = True
		Me.tx_ToNo.ReadOnly = False
		Me.tx_ToNo.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_ToNo.MultiLine = False
		Me.tx_ToNo.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_ToNo.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_ToNo.TabStop = True
		Me.tx_ToNo.Visible = True
		Me.tx_ToNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_ToNo.Name = "tx_ToNo"
		Me._lin_Under_0.X1 = 21
		Me._lin_Under_0.X2 = 235
		Me._lin_Under_0.Y1 = 28
		Me._lin_Under_0.Y2 = 28
		Me._lin_Under_0.BorderColor = System.Drawing.SystemColors.WindowText
		Me._lin_Under_0.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid
		Me._lin_Under_0.BorderWidth = 1
		Me._lin_Under_0.Visible = True
		Me._lin_Under_0.Name = "_lin_Under_0"
		Me.rf_見積番号.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.rf_見積番号.Text = "21755009"
		Me.rf_見積番号.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.rf_見積番号.Size = New System.Drawing.Size(67, 13)
		Me.rf_見積番号.Location = New System.Drawing.Point(117, 12)
		Me.rf_見積番号.TabIndex = 12
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
		Me._lb_項目_2.Text = "見積№"
		Me._lb_項目_2.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_2.Size = New System.Drawing.Size(77, 13)
		Me._lb_項目_2.Location = New System.Drawing.Point(25, 12)
		Me._lb_項目_2.TabIndex = 11
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
		Me.lb_番号指定.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me.lb_番号指定.Text = "【範囲指定】"
		Me.lb_番号指定.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 11.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.lb_番号指定.ForeColor = System.Drawing.Color.Black
		Me.lb_番号指定.Size = New System.Drawing.Size(85, 17)
		Me.lb_番号指定.Location = New System.Drawing.Point(17, 38)
		Me.lb_番号指定.TabIndex = 10
		Me.lb_番号指定.BackColor = System.Drawing.SystemColors.Control
		Me.lb_番号指定.Enabled = True
		Me.lb_番号指定.Cursor = System.Windows.Forms.Cursors.Default
		Me.lb_番号指定.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lb_番号指定.UseMnemonic = True
		Me.lb_番号指定.Visible = True
		Me.lb_番号指定.AutoSize = False
		Me.lb_番号指定.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lb_番号指定.Name = "lb_番号指定"
		Me.lb_kara.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me.lb_kara.Text = "～"
		Me.lb_kara.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 11.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.lb_kara.Size = New System.Drawing.Size(20, 17)
		Me.lb_kara.Location = New System.Drawing.Point(156, 70)
		Me.lb_kara.TabIndex = 9
		Me.lb_kara.BackColor = System.Drawing.SystemColors.Control
		Me.lb_kara.Enabled = True
		Me.lb_kara.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lb_kara.Cursor = System.Windows.Forms.Cursors.Default
		Me.lb_kara.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lb_kara.UseMnemonic = True
		Me.lb_kara.Visible = True
		Me.lb_kara.AutoSize = False
		Me.lb_kara.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lb_kara.Name = "lb_kara"
		Me._lb_項目_5.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_項目_5.Text = "【出力先】"
		Me._lb_項目_5.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 11.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_5.ForeColor = System.Drawing.Color.Black
		Me._lb_項目_5.Size = New System.Drawing.Size(89, 21)
		Me._lb_項目_5.Location = New System.Drawing.Point(15, 120)
		Me._lb_項目_5.TabIndex = 6
		Me._lb_項目_5.BackColor = System.Drawing.SystemColors.Control
		Me._lb_項目_5.Enabled = True
		Me._lb_項目_5.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_5.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_5.UseMnemonic = True
		Me._lb_項目_5.Visible = True
		Me._lb_項目_5.AutoSize = False
		Me._lb_項目_5.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_項目_5.Name = "_lb_項目_5"
		Me._Shape1_2.Size = New System.Drawing.Size(427, 53)
		Me._Shape1_2.Location = New System.Drawing.Point(9, 128)
		Me._Shape1_2.BackColor = System.Drawing.SystemColors.Window
		Me._Shape1_2.BackStyle = Microsoft.VisualBasic.PowerPacks.BackStyle.Transparent
		Me._Shape1_2.BorderColor = System.Drawing.SystemColors.WindowText
		Me._Shape1_2.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid
		Me._Shape1_2.BorderWidth = 1
		Me._Shape1_2.FillColor = System.Drawing.Color.Black
		Me._Shape1_2.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Transparent
		Me._Shape1_2.Visible = True
		Me._Shape1_2.Name = "_Shape1_2"
		Me._Shape1_3.BorderColor = System.Drawing.SystemColors.Window
		Me._Shape1_3.BorderWidth = 2
		Me._Shape1_3.Size = New System.Drawing.Size(427, 53)
		Me._Shape1_3.Location = New System.Drawing.Point(10, 129)
		Me._Shape1_3.BackColor = System.Drawing.SystemColors.Window
		Me._Shape1_3.BackStyle = Microsoft.VisualBasic.PowerPacks.BackStyle.Transparent
		Me._Shape1_3.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid
		Me._Shape1_3.FillColor = System.Drawing.Color.Black
		Me._Shape1_3.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Transparent
		Me._Shape1_3.Visible = True
		Me._Shape1_3.Name = "_Shape1_3"
		Me._Shape1_0.Size = New System.Drawing.Size(427, 58)
		Me._Shape1_0.Location = New System.Drawing.Point(9, 47)
		Me._Shape1_0.BackColor = System.Drawing.SystemColors.Window
		Me._Shape1_0.BackStyle = Microsoft.VisualBasic.PowerPacks.BackStyle.Transparent
		Me._Shape1_0.BorderColor = System.Drawing.SystemColors.WindowText
		Me._Shape1_0.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid
		Me._Shape1_0.BorderWidth = 1
		Me._Shape1_0.FillColor = System.Drawing.Color.Black
		Me._Shape1_0.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Transparent
		Me._Shape1_0.Visible = True
		Me._Shape1_0.Name = "_Shape1_0"
		Me._Shape1_1.BorderColor = System.Drawing.SystemColors.Window
		Me._Shape1_1.BorderWidth = 2
		Me._Shape1_1.Size = New System.Drawing.Size(427, 58)
		Me._Shape1_1.Location = New System.Drawing.Point(10, 48)
		Me._Shape1_1.BackColor = System.Drawing.SystemColors.Window
		Me._Shape1_1.BackStyle = Microsoft.VisualBasic.PowerPacks.BackStyle.Transparent
		Me._Shape1_1.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid
		Me._Shape1_1.FillColor = System.Drawing.Color.Black
		Me._Shape1_1.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Transparent
		Me._Shape1_1.Visible = True
		Me._Shape1_1.Name = "_Shape1_1"
		Me.Controls.Add(cb変更)
		Me.Controls.Add(Picture1)
		Me.Controls.Add(cbXLS)
		Me.Controls.Add(cb中止)
		Me.Controls.Add(sb_Msg)
		Me.Controls.Add(tx_FromNo)
		Me.Controls.Add(tx_ToNo)
		Me.ShapeContainer1.Shapes.Add(_lin_Under_0)
		Me.Controls.Add(rf_見積番号)
		Me.Controls.Add(_lb_項目_2)
		Me.Controls.Add(lb_番号指定)
		Me.Controls.Add(lb_kara)
		Me.Controls.Add(_lb_項目_5)
		Me.ShapeContainer1.Shapes.Add(_Shape1_2)
		Me.ShapeContainer1.Shapes.Add(_Shape1_3)
		Me.ShapeContainer1.Shapes.Add(_Shape1_0)
		Me.ShapeContainer1.Shapes.Add(_Shape1_1)
		Me.Controls.Add(ShapeContainer1)
		Me.Picture1.Controls.Add(txDir)
		Me.sb_Msg.Items.AddRange(New System.Windows.Forms.ToolStripItem(){Me._sb_Msg_Panel1})
		Me.lb_項目.SetIndex(_lb_項目_2, CType(2, Short))
		Me.lb_項目.SetIndex(_lb_項目_5, CType(5, Short))
		Me.lin_Under.SetIndex(_lin_Under_0, CType(0, Short))
		Me.Shape1.SetIndex(_Shape1_2, CType(2, Short))
		Me.Shape1.SetIndex(_Shape1_3, CType(3, Short))
		Me.Shape1.SetIndex(_Shape1_0, CType(0, Short))
		Me.Shape1.SetIndex(_Shape1_1, CType(1, Short))
		CType(Me.Shape1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lin_Under, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lb_項目, System.ComponentModel.ISupportInitialize).EndInit()
		Me.Picture1.ResumeLayout(False)
		Me.sb_Msg.ResumeLayout(False)
		Me.ResumeLayout(False)
		Me.PerformLayout()
	End Sub
#End Region 
End Class