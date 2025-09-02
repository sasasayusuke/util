<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class SnwMT02F05
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
	Public WithEvents cmdFind As System.Windows.Forms.Button
	Public WithEvents cmdCan As System.Windows.Forms.Button
	Public WithEvents cmdOk As System.Windows.Forms.Button
	Public WithEvents SelListVw As System.Windows.Forms.ListView
	Public WithEvents tx_得意先CD As System.Windows.Forms.TextBox
	Public WithEvents tx_仕入先CD As System.Windows.Forms.TextBox
	Public WithEvents tx_見積日付D As System.Windows.Forms.TextBox
	Public WithEvents tx_見積日付M As System.Windows.Forms.TextBox
	Public WithEvents tx_見積日付Y As System.Windows.Forms.TextBox
	Public WithEvents _lb_項目_1 As System.Windows.Forms.Label
	Public WithEvents _lb_年_3 As System.Windows.Forms.Label
	Public WithEvents _lb_月_3 As System.Windows.Forms.Label
	Public WithEvents _lb_日_3 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_0 As System.Windows.Forms.Label
	Public WithEvents rf_名称1 As System.Windows.Forms.Label
	Public WithEvents rf_名称2 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_12 As System.Windows.Forms.Label
	Public WithEvents lb_見積日付 As System.Windows.Forms.Label
	Public WithEvents lb_月 As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents lb_項目 As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents lb_日 As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents lb_年 As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
	'Windows フォーム デザイナを使って変更できます。
	'コード エディタを使用して、変更しないでください。
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(SnwMT02F05))
		Me.components = New System.ComponentModel.Container()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
		Me.cmdFind = New System.Windows.Forms.Button
		Me.cmdCan = New System.Windows.Forms.Button
		Me.cmdOk = New System.Windows.Forms.Button
		Me.SelListVw = New System.Windows.Forms.ListView
		Me.tx_得意先CD = New System.Windows.Forms.TextBox
		Me.tx_仕入先CD = New System.Windows.Forms.TextBox
		Me.tx_見積日付D = New System.Windows.Forms.TextBox
		Me.tx_見積日付M = New System.Windows.Forms.TextBox
		Me.tx_見積日付Y = New System.Windows.Forms.TextBox
		Me._lb_項目_1 = New System.Windows.Forms.Label
		Me._lb_年_3 = New System.Windows.Forms.Label
		Me._lb_月_3 = New System.Windows.Forms.Label
		Me._lb_日_3 = New System.Windows.Forms.Label
		Me._lb_項目_0 = New System.Windows.Forms.Label
		Me.rf_名称1 = New System.Windows.Forms.Label
		Me.rf_名称2 = New System.Windows.Forms.Label
		Me._lb_項目_12 = New System.Windows.Forms.Label
		Me.lb_見積日付 = New System.Windows.Forms.Label
		Me.lb_月 = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(components)
		Me.lb_項目 = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(components)
		Me.lb_日 = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(components)
		Me.lb_年 = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(components)
		Me.SuspendLayout()
		Me.ToolTip1.Active = True
		CType(Me.lb_月, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lb_項目, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lb_日, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lb_年, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.Text = "前回単価参照"
		Me.ClientSize = New System.Drawing.Size(549, 414)
		Me.Location = New System.Drawing.Point(3, 22)
		Me.Icon = CType(resources.GetObject("SnwMT02F05.Icon"), System.Drawing.Icon)
		Me.KeyPreview = True
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ControlBox = True
		Me.Enabled = True
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.HelpButton = False
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.Name = "SnwMT02F05"
		Me.cmdFind.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdFind.Text = "検索(&F)"
		Me.cmdFind.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.cmdFind.Size = New System.Drawing.Size(83, 22)
		Me.cmdFind.Location = New System.Drawing.Point(428, 48)
		Me.cmdFind.TabIndex = 5
		Me.cmdFind.BackColor = System.Drawing.SystemColors.Control
		Me.cmdFind.CausesValidation = True
		Me.cmdFind.Enabled = True
		Me.cmdFind.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdFind.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdFind.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdFind.TabStop = True
		Me.cmdFind.Name = "cmdFind"
		Me.cmdCan.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.CancelButton = Me.cmdCan
		Me.cmdCan.Text = "ｷｬﾝｾﾙ(&C)"
		Me.cmdCan.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.cmdCan.Size = New System.Drawing.Size(83, 22)
		Me.cmdCan.Location = New System.Drawing.Point(254, 384)
		Me.cmdCan.TabIndex = 8
		Me.cmdCan.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCan.CausesValidation = True
		Me.cmdCan.Enabled = True
		Me.cmdCan.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCan.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCan.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCan.TabStop = True
		Me.cmdCan.Name = "cmdCan"
		Me.cmdOk.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOk.Text = "ＯＫ(&O)"
		Me.cmdOk.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.cmdOk.Size = New System.Drawing.Size(83, 22)
		Me.cmdOk.Location = New System.Drawing.Point(171, 384)
		Me.cmdOk.TabIndex = 7
		Me.cmdOk.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOk.CausesValidation = True
		Me.cmdOk.Enabled = True
		Me.cmdOk.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOk.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOk.TabStop = True
		Me.cmdOk.Name = "cmdOk"
		Me.SelListVw.Size = New System.Drawing.Size(329, 290)
		Me.SelListVw.Location = New System.Drawing.Point(8, 84)
		Me.SelListVw.TabIndex = 6
		Me.SelListVw.View = System.Windows.Forms.View.Details
		Me.SelListVw.LabelEdit = False
		Me.SelListVw.LabelWrap = True
		Me.SelListVw.HideSelection = True
		Me.SelListVw.FullRowSelect = True
		Me.SelListVw.ForeColor = System.Drawing.SystemColors.WindowText
		Me.SelListVw.BackColor = System.Drawing.Color.FromARGB(198, 255, 255)
		Me.SelListVw.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.SelListVw.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.SelListVw.Name = "SelListVw"
		Me.tx_得意先CD.AutoSize = False
		Me.tx_得意先CD.Size = New System.Drawing.Size(50, 20)
		Me.tx_得意先CD.Location = New System.Drawing.Point(88, 8)
		Me.tx_得意先CD.TabIndex = 0
		Me.tx_得意先CD.Text = "8888"
		Me.tx_得意先CD.Maxlength = 4
		Me.tx_得意先CD.AcceptsReturn = True
		Me.tx_得意先CD.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_得意先CD.BackColor = System.Drawing.SystemColors.Window
		Me.tx_得意先CD.CausesValidation = True
		Me.tx_得意先CD.Enabled = True
		Me.tx_得意先CD.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_得意先CD.HideSelection = True
		Me.tx_得意先CD.ReadOnly = False
		Me.tx_得意先CD.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_得意先CD.MultiLine = False
		Me.tx_得意先CD.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_得意先CD.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_得意先CD.TabStop = True
		Me.tx_得意先CD.Visible = True
		Me.tx_得意先CD.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_得意先CD.Name = "tx_得意先CD"
		Me.tx_仕入先CD.AutoSize = False
		Me.tx_仕入先CD.Size = New System.Drawing.Size(50, 20)
		Me.tx_仕入先CD.Location = New System.Drawing.Point(88, 28)
		Me.tx_仕入先CD.TabIndex = 1
		Me.tx_仕入先CD.Text = "8888"
		Me.tx_仕入先CD.Maxlength = 4
		Me.tx_仕入先CD.AcceptsReturn = True
		Me.tx_仕入先CD.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_仕入先CD.BackColor = System.Drawing.SystemColors.Window
		Me.tx_仕入先CD.CausesValidation = True
		Me.tx_仕入先CD.Enabled = True
		Me.tx_仕入先CD.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_仕入先CD.HideSelection = True
		Me.tx_仕入先CD.ReadOnly = False
		Me.tx_仕入先CD.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_仕入先CD.MultiLine = False
		Me.tx_仕入先CD.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_仕入先CD.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_仕入先CD.TabStop = True
		Me.tx_仕入先CD.Visible = True
		Me.tx_仕入先CD.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_仕入先CD.Name = "tx_仕入先CD"
		Me.tx_見積日付D.AutoSize = False
		Me.tx_見積日付D.Size = New System.Drawing.Size(16, 13)
		Me.tx_見積日付D.Location = New System.Drawing.Point(180, 51)
		Me.tx_見積日付D.TabIndex = 4
		Me.tx_見積日付D.Text = "88"
		Me.tx_見積日付D.Maxlength = 2
		Me.tx_見積日付D.AcceptsReturn = True
		Me.tx_見積日付D.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_見積日付D.BackColor = System.Drawing.SystemColors.Window
		Me.tx_見積日付D.CausesValidation = True
		Me.tx_見積日付D.Enabled = True
		Me.tx_見積日付D.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_見積日付D.HideSelection = True
		Me.tx_見積日付D.ReadOnly = False
		Me.tx_見積日付D.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_見積日付D.MultiLine = False
		Me.tx_見積日付D.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_見積日付D.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_見積日付D.TabStop = True
		Me.tx_見積日付D.Visible = True
		Me.tx_見積日付D.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_見積日付D.Name = "tx_見積日付D"
		Me.tx_見積日付M.AutoSize = False
		Me.tx_見積日付M.Size = New System.Drawing.Size(16, 13)
		Me.tx_見積日付M.Location = New System.Drawing.Point(145, 51)
		Me.tx_見積日付M.TabIndex = 3
		Me.tx_見積日付M.Text = "88"
		Me.tx_見積日付M.Maxlength = 2
		Me.tx_見積日付M.AcceptsReturn = True
		Me.tx_見積日付M.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_見積日付M.BackColor = System.Drawing.SystemColors.Window
		Me.tx_見積日付M.CausesValidation = True
		Me.tx_見積日付M.Enabled = True
		Me.tx_見積日付M.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_見積日付M.HideSelection = True
		Me.tx_見積日付M.ReadOnly = False
		Me.tx_見積日付M.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_見積日付M.MultiLine = False
		Me.tx_見積日付M.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_見積日付M.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_見積日付M.TabStop = True
		Me.tx_見積日付M.Visible = True
		Me.tx_見積日付M.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_見積日付M.Name = "tx_見積日付M"
		Me.tx_見積日付Y.AutoSize = False
		Me.tx_見積日付Y.Size = New System.Drawing.Size(33, 13)
		Me.tx_見積日付Y.Location = New System.Drawing.Point(94, 51)
		Me.tx_見積日付Y.TabIndex = 2
		Me.tx_見積日付Y.Text = "8888"
		Me.tx_見積日付Y.Maxlength = 4
		Me.tx_見積日付Y.AcceptsReturn = True
		Me.tx_見積日付Y.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_見積日付Y.BackColor = System.Drawing.SystemColors.Window
		Me.tx_見積日付Y.CausesValidation = True
		Me.tx_見積日付Y.Enabled = True
		Me.tx_見積日付Y.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_見積日付Y.HideSelection = True
		Me.tx_見積日付Y.ReadOnly = False
		Me.tx_見積日付Y.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_見積日付Y.MultiLine = False
		Me.tx_見積日付Y.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_見積日付Y.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_見積日付Y.TabStop = True
		Me.tx_見積日付Y.Visible = True
		Me.tx_見積日付Y.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_見積日付Y.Name = "tx_見積日付Y"
		Me._lb_項目_1.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_項目_1.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me._lb_項目_1.Text = "見積日付"
		Me._lb_項目_1.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_1.ForeColor = System.Drawing.Color.White
		Me._lb_項目_1.Size = New System.Drawing.Size(80, 20)
		Me._lb_項目_1.Location = New System.Drawing.Point(8, 48)
		Me._lb_項目_1.TabIndex = 17
		Me._lb_項目_1.Enabled = True
		Me._lb_項目_1.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_1.UseMnemonic = True
		Me._lb_項目_1.Visible = True
		Me._lb_項目_1.AutoSize = False
		Me._lb_項目_1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._lb_項目_1.Name = "_lb_項目_1"
		Me._lb_年_3.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_年_3.BackColor = System.Drawing.Color.Transparent
		Me._lb_年_3.Text = "年"
		Me._lb_年_3.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_年_3.Size = New System.Drawing.Size(17, 15)
		Me._lb_年_3.Location = New System.Drawing.Point(125, 51)
		Me._lb_年_3.TabIndex = 15
		Me._lb_年_3.Enabled = True
		Me._lb_年_3.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_年_3.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_年_3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_年_3.UseMnemonic = True
		Me._lb_年_3.Visible = True
		Me._lb_年_3.AutoSize = False
		Me._lb_年_3.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_年_3.Name = "_lb_年_3"
		Me._lb_月_3.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_月_3.BackColor = System.Drawing.Color.Transparent
		Me._lb_月_3.Text = "月"
		Me._lb_月_3.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_月_3.Size = New System.Drawing.Size(12, 15)
		Me._lb_月_3.Location = New System.Drawing.Point(162, 51)
		Me._lb_月_3.TabIndex = 14
		Me._lb_月_3.Enabled = True
		Me._lb_月_3.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_月_3.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_月_3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_月_3.UseMnemonic = True
		Me._lb_月_3.Visible = True
		Me._lb_月_3.AutoSize = False
		Me._lb_月_3.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_月_3.Name = "_lb_月_3"
		Me._lb_日_3.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_日_3.BackColor = System.Drawing.Color.Transparent
		Me._lb_日_3.Text = "日"
		Me._lb_日_3.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_日_3.Size = New System.Drawing.Size(12, 15)
		Me._lb_日_3.Location = New System.Drawing.Point(198, 51)
		Me._lb_日_3.TabIndex = 13
		Me._lb_日_3.Enabled = True
		Me._lb_日_3.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_日_3.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_日_3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_日_3.UseMnemonic = True
		Me._lb_日_3.Visible = True
		Me._lb_日_3.AutoSize = False
		Me._lb_日_3.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_日_3.Name = "_lb_日_3"
		Me._lb_項目_0.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_項目_0.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me._lb_項目_0.Text = "得意先"
		Me._lb_項目_0.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_0.ForeColor = System.Drawing.Color.White
		Me._lb_項目_0.Size = New System.Drawing.Size(80, 20)
		Me._lb_項目_0.Location = New System.Drawing.Point(8, 8)
		Me._lb_項目_0.TabIndex = 12
		Me._lb_項目_0.Enabled = True
		Me._lb_項目_0.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_0.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_0.UseMnemonic = True
		Me._lb_項目_0.Visible = True
		Me._lb_項目_0.AutoSize = False
		Me._lb_項目_0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._lb_項目_0.Name = "_lb_項目_0"
		Me.rf_名称1.BackColor = System.Drawing.Color.FromARGB(224, 224, 224)
		Me.rf_名称1.Text = "１２３４５６７８９０１２３４"
		Me.rf_名称1.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.rf_名称1.ForeColor = System.Drawing.Color.Black
		Me.rf_名称1.Size = New System.Drawing.Size(236, 20)
		Me.rf_名称1.Location = New System.Drawing.Point(138, 8)
		Me.rf_名称1.TabIndex = 11
		Me.rf_名称1.Visible = False
		Me.rf_名称1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.rf_名称1.Enabled = True
		Me.rf_名称1.Cursor = System.Windows.Forms.Cursors.Default
		Me.rf_名称1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.rf_名称1.UseMnemonic = True
		Me.rf_名称1.AutoSize = False
		Me.rf_名称1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.rf_名称1.Name = "rf_名称1"
		Me.rf_名称2.BackColor = System.Drawing.Color.FromARGB(224, 224, 224)
		Me.rf_名称2.Text = "１２３４５６７８９０１２３４"
		Me.rf_名称2.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.rf_名称2.ForeColor = System.Drawing.Color.Black
		Me.rf_名称2.Size = New System.Drawing.Size(236, 20)
		Me.rf_名称2.Location = New System.Drawing.Point(138, 28)
		Me.rf_名称2.TabIndex = 10
		Me.rf_名称2.Visible = False
		Me.rf_名称2.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.rf_名称2.Enabled = True
		Me.rf_名称2.Cursor = System.Windows.Forms.Cursors.Default
		Me.rf_名称2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.rf_名称2.UseMnemonic = True
		Me.rf_名称2.AutoSize = False
		Me.rf_名称2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.rf_名称2.Name = "rf_名称2"
		Me._lb_項目_12.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_項目_12.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me._lb_項目_12.Text = "仕入先"
		Me._lb_項目_12.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_12.ForeColor = System.Drawing.Color.White
		Me._lb_項目_12.Size = New System.Drawing.Size(80, 20)
		Me._lb_項目_12.Location = New System.Drawing.Point(8, 28)
		Me._lb_項目_12.TabIndex = 9
		Me._lb_項目_12.Enabled = True
		Me._lb_項目_12.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_12.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_12.UseMnemonic = True
		Me._lb_項目_12.Visible = True
		Me._lb_項目_12.AutoSize = False
		Me._lb_項目_12.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._lb_項目_12.Name = "_lb_項目_12"
		Me.lb_見積日付.BackColor = System.Drawing.SystemColors.Window
		Me.lb_見積日付.Size = New System.Drawing.Size(127, 19)
		Me.lb_見積日付.Location = New System.Drawing.Point(88, 48)
		Me.lb_見積日付.TabIndex = 16
		Me.lb_見積日付.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lb_見積日付.Enabled = True
		Me.lb_見積日付.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lb_見積日付.Cursor = System.Windows.Forms.Cursors.Default
		Me.lb_見積日付.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lb_見積日付.UseMnemonic = True
		Me.lb_見積日付.Visible = True
		Me.lb_見積日付.AutoSize = False
		Me.lb_見積日付.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lb_見積日付.Name = "lb_見積日付"
		Me.Controls.Add(cmdFind)
		Me.Controls.Add(cmdCan)
		Me.Controls.Add(cmdOk)
		Me.Controls.Add(SelListVw)
		Me.Controls.Add(tx_得意先CD)
		Me.Controls.Add(tx_仕入先CD)
		Me.Controls.Add(tx_見積日付D)
		Me.Controls.Add(tx_見積日付M)
		Me.Controls.Add(tx_見積日付Y)
		Me.Controls.Add(_lb_項目_1)
		Me.Controls.Add(_lb_年_3)
		Me.Controls.Add(_lb_月_3)
		Me.Controls.Add(_lb_日_3)
		Me.Controls.Add(_lb_項目_0)
		Me.Controls.Add(rf_名称1)
		Me.Controls.Add(rf_名称2)
		Me.Controls.Add(_lb_項目_12)
		Me.Controls.Add(lb_見積日付)
		Me.lb_月.SetIndex(_lb_月_3, CType(3, Short))
		Me.lb_項目.SetIndex(_lb_項目_1, CType(1, Short))
		Me.lb_項目.SetIndex(_lb_項目_0, CType(0, Short))
		Me.lb_項目.SetIndex(_lb_項目_12, CType(12, Short))
		Me.lb_日.SetIndex(_lb_日_3, CType(3, Short))
		Me.lb_年.SetIndex(_lb_年_3, CType(3, Short))
		CType(Me.lb_年, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lb_日, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lb_項目, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lb_月, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()
	End Sub
#End Region 
End Class