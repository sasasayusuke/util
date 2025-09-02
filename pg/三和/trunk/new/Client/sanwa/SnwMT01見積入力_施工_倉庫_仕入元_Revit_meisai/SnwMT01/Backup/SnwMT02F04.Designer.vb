<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class SnwMT02F04
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
	Public WithEvents _lin_Under_2 As Microsoft.VisualBasic.PowerPacks.LineShape
	Public WithEvents rf_取込行数 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_4 As System.Windows.Forms.Label
	Public WithEvents _lin_Under_1 As Microsoft.VisualBasic.PowerPacks.LineShape
	Public WithEvents _lin_Under_0 As Microsoft.VisualBasic.PowerPacks.LineShape
	Public WithEvents rf_取込仕分 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_2 As System.Windows.Forms.Label
	Public WithEvents rf_見積番号 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_1 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_0 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_3 As System.Windows.Forms.Label
	Public WithEvents _Shape1_2 As Microsoft.VisualBasic.PowerPacks.RectangleShape
	Public WithEvents _Shape1_3 As Microsoft.VisualBasic.PowerPacks.RectangleShape
	Public WithEvents lb_項目 As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents lin_Under As LineShapeArray
	Public WithEvents Shape1 As RectangleShapeArray
	Public WithEvents ShapeContainer1 As Microsoft.VisualBasic.PowerPacks.ShapeContainer
	'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
	'Windows フォーム デザイナを使って変更できます。
	'コード エディタを使用して、変更しないでください。
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(SnwMT02F04))
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
		Me._lin_Under_2 = New Microsoft.VisualBasic.PowerPacks.LineShape
		Me.rf_取込行数 = New System.Windows.Forms.Label
		Me._lb_項目_4 = New System.Windows.Forms.Label
		Me._lin_Under_1 = New Microsoft.VisualBasic.PowerPacks.LineShape
		Me._lin_Under_0 = New Microsoft.VisualBasic.PowerPacks.LineShape
		Me.rf_取込仕分 = New System.Windows.Forms.Label
		Me._lb_項目_2 = New System.Windows.Forms.Label
		Me.rf_見積番号 = New System.Windows.Forms.Label
		Me._lb_項目_1 = New System.Windows.Forms.Label
		Me._lb_項目_0 = New System.Windows.Forms.Label
		Me._lb_項目_3 = New System.Windows.Forms.Label
		Me._Shape1_2 = New Microsoft.VisualBasic.PowerPacks.RectangleShape
		Me._Shape1_3 = New Microsoft.VisualBasic.PowerPacks.RectangleShape
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
		Me.Text = "員数シート取込"
		Me.ClientSize = New System.Drawing.Size(458, 191)
		Me.Location = New System.Drawing.Point(3, 22)
		Me.Icon = CType(resources.GetObject("SnwMT02F04.Icon"), System.Drawing.Icon)
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
		Me.Name = "SnwMT02F04"
		Me.cb変更.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cb変更.Text = "変更(&D)"
		Me.cb変更.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.cb変更.Size = New System.Drawing.Size(60, 22)
		Me.cb変更.Location = New System.Drawing.Point(324, 41)
		Me.cb変更.TabIndex = 2
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
		Me.Picture1.Location = New System.Drawing.Point(50, 41)
		Me.Picture1.TabIndex = 5
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
		Me.txDir.TabIndex = 6
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
		Me.cbXLS.Text = "取込(&I)"
		Me.cbXLS.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.cbXLS.Size = New System.Drawing.Size(72, 22)
		Me.cbXLS.Location = New System.Drawing.Point(284, 89)
		Me.cbXLS.TabIndex = 0
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
		Me.cb中止.Location = New System.Drawing.Point(364, 89)
		Me.cb中止.TabIndex = 1
		Me.cb中止.BackColor = System.Drawing.SystemColors.Control
		Me.cb中止.CausesValidation = True
		Me.cb中止.Enabled = True
		Me.cb中止.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cb中止.Cursor = System.Windows.Forms.Cursors.Default
		Me.cb中止.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cb中止.TabStop = True
		Me.cb中止.Name = "cb中止"
		Me.sb_Msg.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.sb_Msg.Size = New System.Drawing.Size(458, 20)
		Me.sb_Msg.Location = New System.Drawing.Point(0, 171)
		Me.sb_Msg.TabIndex = 3
		Me.sb_Msg.Name = "sb_Msg"
		Me._sb_Msg_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
		Me._sb_Msg_Panel1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
		Me._sb_Msg_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._sb_Msg_Panel1.Size = New System.Drawing.Size(457, 20)
		Me._sb_Msg_Panel1.Spring = True
		Me._sb_Msg_Panel1.AutoSize = True
		Me._sb_Msg_Panel1.BorderSides = CType(System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom, System.Windows.Forms.ToolStripStatusLabelBorderSides)
		Me._sb_Msg_Panel1.Margin = New System.Windows.Forms.Padding(0)
		Me._sb_Msg_Panel1.AutoSize = False
		Me._lin_Under_2.X1 = 291
		Me._lin_Under_2.X2 = 434
		Me._lin_Under_2.Y1 = 164
		Me._lin_Under_2.Y2 = 164
		Me._lin_Under_2.BorderColor = System.Drawing.SystemColors.WindowText
		Me._lin_Under_2.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid
		Me._lin_Under_2.BorderWidth = 1
		Me._lin_Under_2.Visible = True
		Me._lin_Under_2.Name = "_lin_Under_2"
		Me.rf_取込行数.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.rf_取込行数.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.rf_取込行数.Size = New System.Drawing.Size(40, 17)
		Me.rf_取込行数.Location = New System.Drawing.Point(375, 148)
		Me.rf_取込行数.TabIndex = 13
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
		Me._lb_項目_4.Location = New System.Drawing.Point(294, 147)
		Me._lb_項目_4.TabIndex = 12
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
		Me._lin_Under_1.X1 = 125
		Me._lin_Under_1.X2 = 289
		Me._lin_Under_1.Y1 = 164
		Me._lin_Under_1.Y2 = 164
		Me._lin_Under_1.BorderColor = System.Drawing.SystemColors.WindowText
		Me._lin_Under_1.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid
		Me._lin_Under_1.BorderWidth = 1
		Me._lin_Under_1.Visible = True
		Me._lin_Under_1.Name = "_lin_Under_1"
		Me._lin_Under_0.X1 = 125
		Me._lin_Under_0.X2 = 289
		Me._lin_Under_0.Y1 = 140
		Me._lin_Under_0.Y2 = 140
		Me._lin_Under_0.BorderColor = System.Drawing.SystemColors.WindowText
		Me._lin_Under_0.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid
		Me._lin_Under_0.BorderWidth = 1
		Me._lin_Under_0.Visible = True
		Me._lin_Under_0.Name = "_lin_Under_0"
		Me.rf_取込仕分.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.rf_取込仕分.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.rf_取込仕分.Size = New System.Drawing.Size(67, 17)
		Me.rf_取込仕分.Location = New System.Drawing.Point(209, 147)
		Me.rf_取込仕分.TabIndex = 11
		Me.rf_取込仕分.BackColor = System.Drawing.SystemColors.Control
		Me.rf_取込仕分.Enabled = True
		Me.rf_取込仕分.ForeColor = System.Drawing.SystemColors.ControlText
		Me.rf_取込仕分.Cursor = System.Windows.Forms.Cursors.Default
		Me.rf_取込仕分.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.rf_取込仕分.UseMnemonic = True
		Me.rf_取込仕分.Visible = True
		Me.rf_取込仕分.AutoSize = False
		Me.rf_取込仕分.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.rf_取込仕分.Name = "rf_取込仕分"
		Me._lb_項目_2.Text = "取込仕分"
		Me._lb_項目_2.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_2.Size = New System.Drawing.Size(77, 17)
		Me._lb_項目_2.Location = New System.Drawing.Point(128, 147)
		Me._lb_項目_2.TabIndex = 10
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
		Me.rf_見積番号.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.rf_見積番号.Size = New System.Drawing.Size(67, 17)
		Me.rf_見積番号.Location = New System.Drawing.Point(209, 123)
		Me.rf_見積番号.TabIndex = 9
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
		Me._lb_項目_1.Text = "見積№"
		Me._lb_項目_1.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_1.Size = New System.Drawing.Size(77, 17)
		Me._lb_項目_1.Location = New System.Drawing.Point(128, 123)
		Me._lb_項目_1.TabIndex = 8
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
		Me._lb_項目_0.Text = "取込シート情報"
		Me._lb_項目_0.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_0.Size = New System.Drawing.Size(105, 17)
		Me._lb_項目_0.Location = New System.Drawing.Point(15, 123)
		Me._lb_項目_0.TabIndex = 7
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
		Me._lb_項目_3.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_項目_3.Text = "【入力先】"
		Me._lb_項目_3.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 11.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_3.ForeColor = System.Drawing.Color.Black
		Me._lb_項目_3.Size = New System.Drawing.Size(89, 21)
		Me._lb_項目_3.Location = New System.Drawing.Point(15, 12)
		Me._lb_項目_3.TabIndex = 4
		Me._lb_項目_3.BackColor = System.Drawing.SystemColors.Control
		Me._lb_項目_3.Enabled = True
		Me._lb_項目_3.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_3.UseMnemonic = True
		Me._lb_項目_3.Visible = True
		Me._lb_項目_3.AutoSize = False
		Me._lb_項目_3.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_項目_3.Name = "_lb_項目_3"
		Me._Shape1_2.Size = New System.Drawing.Size(427, 53)
		Me._Shape1_2.Location = New System.Drawing.Point(9, 20)
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
		Me._Shape1_3.Location = New System.Drawing.Point(10, 21)
		Me._Shape1_3.BackColor = System.Drawing.SystemColors.Window
		Me._Shape1_3.BackStyle = Microsoft.VisualBasic.PowerPacks.BackStyle.Transparent
		Me._Shape1_3.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid
		Me._Shape1_3.FillColor = System.Drawing.Color.Black
		Me._Shape1_3.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Transparent
		Me._Shape1_3.Visible = True
		Me._Shape1_3.Name = "_Shape1_3"
		Me.Controls.Add(cb変更)
		Me.Controls.Add(Picture1)
		Me.Controls.Add(cbXLS)
		Me.Controls.Add(cb中止)
		Me.Controls.Add(sb_Msg)
		Me.ShapeContainer1.Shapes.Add(_lin_Under_2)
		Me.Controls.Add(rf_取込行数)
		Me.Controls.Add(_lb_項目_4)
		Me.ShapeContainer1.Shapes.Add(_lin_Under_1)
		Me.ShapeContainer1.Shapes.Add(_lin_Under_0)
		Me.Controls.Add(rf_取込仕分)
		Me.Controls.Add(_lb_項目_2)
		Me.Controls.Add(rf_見積番号)
		Me.Controls.Add(_lb_項目_1)
		Me.Controls.Add(_lb_項目_0)
		Me.Controls.Add(_lb_項目_3)
		Me.ShapeContainer1.Shapes.Add(_Shape1_2)
		Me.ShapeContainer1.Shapes.Add(_Shape1_3)
		Me.Controls.Add(ShapeContainer1)
		Me.Picture1.Controls.Add(txDir)
		Me.sb_Msg.Items.AddRange(New System.Windows.Forms.ToolStripItem(){Me._sb_Msg_Panel1})
		Me.lb_項目.SetIndex(_lb_項目_4, CType(4, Short))
		Me.lb_項目.SetIndex(_lb_項目_2, CType(2, Short))
		Me.lb_項目.SetIndex(_lb_項目_1, CType(1, Short))
		Me.lb_項目.SetIndex(_lb_項目_0, CType(0, Short))
		Me.lb_項目.SetIndex(_lb_項目_3, CType(3, Short))
		Me.lin_Under.SetIndex(_lin_Under_2, CType(2, Short))
		Me.lin_Under.SetIndex(_lin_Under_1, CType(1, Short))
		Me.lin_Under.SetIndex(_lin_Under_0, CType(0, Short))
		Me.Shape1.SetIndex(_Shape1_2, CType(2, Short))
		Me.Shape1.SetIndex(_Shape1_3, CType(3, Short))
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