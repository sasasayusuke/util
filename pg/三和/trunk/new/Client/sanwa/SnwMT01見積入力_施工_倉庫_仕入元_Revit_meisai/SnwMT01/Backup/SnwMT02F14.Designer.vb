<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class SnwMT02F14
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
	Public WithEvents cbNewImport As System.Windows.Forms.Button
	Public WithEvents cb変更 As System.Windows.Forms.Button
	Public WithEvents txDir As System.Windows.Forms.TextBox
	Public WithEvents Picture1 As System.Windows.Forms.Panel
	Public WithEvents cb中止 As System.Windows.Forms.Button
	Public WithEvents _sb_Msg_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents sb_Msg As System.Windows.Forms.StatusStrip
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_3 As System.Windows.Forms.Label
	Public WithEvents _Shape1_0 As Microsoft.VisualBasic.PowerPacks.RectangleShape
	Public WithEvents _lin_Under_2 As Microsoft.VisualBasic.PowerPacks.LineShape
	Public WithEvents rf_取込行数 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_4 As System.Windows.Forms.Label
	Public WithEvents _Shape1_1 As Microsoft.VisualBasic.PowerPacks.RectangleShape
	Public WithEvents lb_項目 As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents lin_Under As LineShapeArray
	Public WithEvents Shape1 As RectangleShapeArray
	Public WithEvents ShapeContainer1 As Microsoft.VisualBasic.PowerPacks.ShapeContainer
	'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
	'Windows フォーム デザイナを使って変更できます。
	'コード エディタを使用して、変更しないでください。
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(SnwMT02F14))
		Me.components = New System.ComponentModel.Container()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
		Me.ShapeContainer1 = New Microsoft.VisualBasic.PowerPacks.ShapeContainer
		Me.cbNewImport = New System.Windows.Forms.Button
		Me.cb変更 = New System.Windows.Forms.Button
		Me.Picture1 = New System.Windows.Forms.Panel
		Me.txDir = New System.Windows.Forms.TextBox
		Me.cb中止 = New System.Windows.Forms.Button
		Me.sb_Msg = New System.Windows.Forms.StatusStrip
		Me._sb_Msg_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
		Me.Label1 = New System.Windows.Forms.Label
		Me._lb_項目_3 = New System.Windows.Forms.Label
		Me._Shape1_0 = New Microsoft.VisualBasic.PowerPacks.RectangleShape
		Me._lin_Under_2 = New Microsoft.VisualBasic.PowerPacks.LineShape
		Me.rf_取込行数 = New System.Windows.Forms.Label
		Me._lb_項目_4 = New System.Windows.Forms.Label
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
		Me.Text = "しまむら消耗品取込"
		Me.ClientSize = New System.Drawing.Size(480, 225)
		Me.Location = New System.Drawing.Point(3, 22)
		Me.Icon = CType(resources.GetObject("SnwMT02F14.Icon"), System.Drawing.Icon)
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
		Me.Name = "SnwMT02F14"
		Me.cbNewImport.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cbNewImport.Text = "取込(&I)"
		Me.cbNewImport.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.cbNewImport.Size = New System.Drawing.Size(72, 22)
		Me.cbNewImport.Location = New System.Drawing.Point(316, 181)
		Me.cbNewImport.TabIndex = 0
		Me.cbNewImport.BackColor = System.Drawing.SystemColors.Control
		Me.cbNewImport.CausesValidation = True
		Me.cbNewImport.Enabled = True
		Me.cbNewImport.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cbNewImport.Cursor = System.Windows.Forms.Cursors.Default
		Me.cbNewImport.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cbNewImport.TabStop = True
		Me.cbNewImport.Name = "cbNewImport"
		Me.cb変更.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cb変更.Text = "変更(&D)"
		Me.cb変更.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.cb変更.Size = New System.Drawing.Size(60, 22)
		Me.cb変更.Location = New System.Drawing.Point(403, 44)
		Me.cb変更.TabIndex = 8
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
		Me.Picture1.Size = New System.Drawing.Size(384, 19)
		Me.Picture1.Location = New System.Drawing.Point(16, 44)
		Me.Picture1.TabIndex = 6
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
		Me.txDir.Size = New System.Drawing.Size(420, 19)
		Me.txDir.Location = New System.Drawing.Point(0, 0)
		Me.txDir.Maxlength = 60
		Me.txDir.TabIndex = 7
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
		Me.cb中止.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.CancelButton = Me.cb中止
		Me.cb中止.Text = "ｷｬﾝｾﾙ"
		Me.cb中止.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.cb中止.Size = New System.Drawing.Size(74, 22)
		Me.cb中止.Location = New System.Drawing.Point(396, 181)
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
		Me.sb_Msg.Size = New System.Drawing.Size(480, 20)
		Me.sb_Msg.Location = New System.Drawing.Point(0, 205)
		Me.sb_Msg.TabIndex = 2
		Me.sb_Msg.Name = "sb_Msg"
		Me._sb_Msg_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
		Me._sb_Msg_Panel1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
		Me._sb_Msg_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._sb_Msg_Panel1.Size = New System.Drawing.Size(479, 20)
		Me._sb_Msg_Panel1.Spring = True
		Me._sb_Msg_Panel1.AutoSize = True
		Me._sb_Msg_Panel1.BorderSides = CType(System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom, System.Windows.Forms.ToolStripStatusLabelBorderSides)
		Me._sb_Msg_Panel1.Margin = New System.Windows.Forms.Padding(0)
		Me._sb_Msg_Panel1.AutoSize = False
		Me.Label1.Text = "取込ファイル仕様　しまむらWeb処理履歴一覧" & Chr(13) & Chr(10) & "  8519_XX_SHINCHOKU_XXXX.CSV" & Chr(13) & Chr(10) & "※出荷済を取り込みます。" & Chr(13) & Chr(10) & "" & Chr(13) & Chr(10) & "XX部分" & Chr(13) & Chr(10) & "01：しまむら" & Chr(13) & Chr(10) & "02：アベイル" & Chr(13) & Chr(10) & "04：バースディ" & Chr(13) & Chr(10) & "05：シャンブル" & Chr(13) & Chr(10) & "06：ディバロ" & Chr(13) & Chr(10)
		Me.Label1.Size = New System.Drawing.Size(245, 125)
		Me.Label1.Location = New System.Drawing.Point(12, 80)
		Me.Label1.TabIndex = 9
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label1.BackColor = System.Drawing.SystemColors.Control
		Me.Label1.Enabled = True
		Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label1.UseMnemonic = True
		Me.Label1.Visible = True
		Me.Label1.AutoSize = False
		Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label1.Name = "Label1"
		Me._lb_項目_3.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_項目_3.Text = "【入力先】"
		Me._lb_項目_3.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 11.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_3.ForeColor = System.Drawing.Color.Black
		Me._lb_項目_3.Size = New System.Drawing.Size(89, 21)
		Me._lb_項目_3.Location = New System.Drawing.Point(15, 12)
		Me._lb_項目_3.TabIndex = 3
		Me._lb_項目_3.BackColor = System.Drawing.SystemColors.Control
		Me._lb_項目_3.Enabled = True
		Me._lb_項目_3.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_3.UseMnemonic = True
		Me._lb_項目_3.Visible = True
		Me._lb_項目_3.AutoSize = False
		Me._lb_項目_3.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_項目_3.Name = "_lb_項目_3"
		Me._Shape1_0.BorderColor = System.Drawing.SystemColors.ControlDark
		Me._Shape1_0.Size = New System.Drawing.Size(462, 58)
		Me._Shape1_0.Location = New System.Drawing.Point(8, 20)
		Me._Shape1_0.BackColor = System.Drawing.SystemColors.Window
		Me._Shape1_0.BackStyle = Microsoft.VisualBasic.PowerPacks.BackStyle.Transparent
		Me._Shape1_0.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid
		Me._Shape1_0.BorderWidth = 1
		Me._Shape1_0.FillColor = System.Drawing.Color.Black
		Me._Shape1_0.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Transparent
		Me._Shape1_0.Visible = True
		Me._Shape1_0.Name = "_Shape1_0"
		Me._lin_Under_2.X1 = 323
		Me._lin_Under_2.X2 = 466
		Me._lin_Under_2.Y1 = 104
		Me._lin_Under_2.Y2 = 104
		Me._lin_Under_2.BorderColor = System.Drawing.SystemColors.WindowText
		Me._lin_Under_2.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid
		Me._lin_Under_2.BorderWidth = 1
		Me._lin_Under_2.Visible = True
		Me._lin_Under_2.Name = "_lin_Under_2"
		Me.rf_取込行数.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.rf_取込行数.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.rf_取込行数.Size = New System.Drawing.Size(44, 17)
		Me.rf_取込行数.Location = New System.Drawing.Point(419, 88)
		Me.rf_取込行数.TabIndex = 5
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
		Me._lb_項目_4.Size = New System.Drawing.Size(81, 17)
		Me._lb_項目_4.Location = New System.Drawing.Point(326, 87)
		Me._lb_項目_4.TabIndex = 4
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
		Me._Shape1_1.BorderColor = System.Drawing.SystemColors.Window
		Me._Shape1_1.BorderWidth = 2
		Me._Shape1_1.Size = New System.Drawing.Size(462, 58)
		Me._Shape1_1.Location = New System.Drawing.Point(9, 21)
		Me._Shape1_1.BackColor = System.Drawing.SystemColors.Window
		Me._Shape1_1.BackStyle = Microsoft.VisualBasic.PowerPacks.BackStyle.Transparent
		Me._Shape1_1.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid
		Me._Shape1_1.FillColor = System.Drawing.Color.Black
		Me._Shape1_1.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Transparent
		Me._Shape1_1.Visible = True
		Me._Shape1_1.Name = "_Shape1_1"
		Me.Controls.Add(cbNewImport)
		Me.Controls.Add(cb変更)
		Me.Controls.Add(Picture1)
		Me.Controls.Add(cb中止)
		Me.Controls.Add(sb_Msg)
		Me.Controls.Add(Label1)
		Me.Controls.Add(_lb_項目_3)
		Me.ShapeContainer1.Shapes.Add(_Shape1_0)
		Me.ShapeContainer1.Shapes.Add(_lin_Under_2)
		Me.Controls.Add(rf_取込行数)
		Me.Controls.Add(_lb_項目_4)
		Me.ShapeContainer1.Shapes.Add(_Shape1_1)
		Me.Controls.Add(ShapeContainer1)
		Me.Picture1.Controls.Add(txDir)
		Me.sb_Msg.Items.AddRange(New System.Windows.Forms.ToolStripItem(){Me._sb_Msg_Panel1})
		Me.lb_項目.SetIndex(_lb_項目_3, CType(3, Short))
		Me.lb_項目.SetIndex(_lb_項目_4, CType(4, Short))
		Me.lin_Under.SetIndex(_lin_Under_2, CType(2, Short))
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