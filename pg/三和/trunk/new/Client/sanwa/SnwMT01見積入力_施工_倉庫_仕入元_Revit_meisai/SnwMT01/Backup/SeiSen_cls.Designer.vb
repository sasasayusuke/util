<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class SeiSen_cls
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
	Public WithEvents SelListVw As System.Windows.Forms.ListView
	Public WithEvents cmdOk As System.Windows.Forms.Button
	Public WithEvents cmdCan As System.Windows.Forms.Button
	Public WithEvents cmdFind As System.Windows.Forms.Button
	Public WithEvents tx_検索製品 As System.Windows.Forms.TextBox
	Public WithEvents tx_検索名称 As System.Windows.Forms.TextBox
	Public WithEvents tx_検索仕入先 As System.Windows.Forms.TextBox
	Public WithEvents tx_検索仕様 As System.Windows.Forms.TextBox
	Public WithEvents tx_W As System.Windows.Forms.TextBox
	Public WithEvents tx_D As System.Windows.Forms.TextBox
	Public WithEvents tx_H As System.Windows.Forms.TextBox
	Public WithEvents tx_D2 As System.Windows.Forms.TextBox
	Public WithEvents tx_D1 As System.Windows.Forms.TextBox
	Public WithEvents tx_H2 As System.Windows.Forms.TextBox
	Public WithEvents tx_H1 As System.Windows.Forms.TextBox
	Public WithEvents _lb_項目_10 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_9 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_7 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_8 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_6 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_5 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_4 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_1 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_2 As System.Windows.Forms.Label
	Public WithEvents lbGuide As System.Windows.Forms.Label
	Public WithEvents lbListCount As System.Windows.Forms.Label
	Public WithEvents lb_該当件数 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_3 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_0 As System.Windows.Forms.Label
	Public WithEvents lb_項目 As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
	'Windows フォーム デザイナを使って変更できます。
	'コード エディタを使用して、変更しないでください。
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(SeiSen_cls))
		Me.components = New System.ComponentModel.Container()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
		Me.SelListVw = New System.Windows.Forms.ListView
		Me.cmdOk = New System.Windows.Forms.Button
		Me.cmdCan = New System.Windows.Forms.Button
		Me.cmdFind = New System.Windows.Forms.Button
		Me.tx_検索製品 = New System.Windows.Forms.TextBox
		Me.tx_検索名称 = New System.Windows.Forms.TextBox
		Me.tx_検索仕入先 = New System.Windows.Forms.TextBox
		Me.tx_検索仕様 = New System.Windows.Forms.TextBox
		Me.tx_W = New System.Windows.Forms.TextBox
		Me.tx_D = New System.Windows.Forms.TextBox
		Me.tx_H = New System.Windows.Forms.TextBox
		Me.tx_D2 = New System.Windows.Forms.TextBox
		Me.tx_D1 = New System.Windows.Forms.TextBox
		Me.tx_H2 = New System.Windows.Forms.TextBox
		Me.tx_H1 = New System.Windows.Forms.TextBox
		Me._lb_項目_10 = New System.Windows.Forms.Label
		Me._lb_項目_9 = New System.Windows.Forms.Label
		Me._lb_項目_7 = New System.Windows.Forms.Label
		Me._lb_項目_8 = New System.Windows.Forms.Label
		Me._lb_項目_6 = New System.Windows.Forms.Label
		Me._lb_項目_5 = New System.Windows.Forms.Label
		Me._lb_項目_4 = New System.Windows.Forms.Label
		Me._lb_項目_1 = New System.Windows.Forms.Label
		Me._lb_項目_2 = New System.Windows.Forms.Label
		Me.lbGuide = New System.Windows.Forms.Label
		Me.lbListCount = New System.Windows.Forms.Label
		Me.lb_該当件数 = New System.Windows.Forms.Label
		Me._lb_項目_3 = New System.Windows.Forms.Label
		Me._lb_項目_0 = New System.Windows.Forms.Label
		Me.lb_項目 = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(components)
		Me.SuspendLayout()
		Me.ToolTip1.Active = True
		CType(Me.lb_項目, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.Text = "製品選択"
		Me.ClientSize = New System.Drawing.Size(804, 495)
		Me.Location = New System.Drawing.Point(4, 23)
		Me.Icon = CType(resources.GetObject("SeiSen_cls.Icon"), System.Drawing.Icon)
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.ControlBox = True
		Me.Enabled = True
		Me.KeyPreview = False
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.HelpButton = False
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.Name = "SeiSen_cls"
		Me.SelListVw.Size = New System.Drawing.Size(687, 316)
		Me.SelListVw.Location = New System.Drawing.Point(12, 148)
		Me.SelListVw.TabIndex = 12
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
		Me.cmdOk.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOk.Text = "ＯＫ(&O)"
		Me.cmdOk.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.cmdOk.Size = New System.Drawing.Size(83, 22)
		Me.cmdOk.Location = New System.Drawing.Point(533, 471)
		Me.cmdOk.TabIndex = 13
		Me.cmdOk.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOk.CausesValidation = True
		Me.cmdOk.Enabled = True
		Me.cmdOk.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOk.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOk.TabStop = True
		Me.cmdOk.Name = "cmdOk"
		Me.cmdCan.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.CancelButton = Me.cmdCan
		Me.cmdCan.Text = "ｷｬﾝｾﾙ(&C)"
		Me.cmdCan.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.cmdCan.Size = New System.Drawing.Size(83, 22)
		Me.cmdCan.Location = New System.Drawing.Point(616, 471)
		Me.cmdCan.TabIndex = 14
		Me.cmdCan.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCan.CausesValidation = True
		Me.cmdCan.Enabled = True
		Me.cmdCan.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCan.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCan.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCan.TabStop = True
		Me.cmdCan.Name = "cmdCan"
		Me.cmdFind.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdFind.Text = "検索(&F)"
		Me.cmdFind.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.cmdFind.Size = New System.Drawing.Size(83, 22)
		Me.cmdFind.Location = New System.Drawing.Point(711, 116)
		Me.cmdFind.TabIndex = 11
		Me.cmdFind.BackColor = System.Drawing.SystemColors.Control
		Me.cmdFind.CausesValidation = True
		Me.cmdFind.Enabled = True
		Me.cmdFind.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdFind.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdFind.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdFind.TabStop = True
		Me.cmdFind.Name = "cmdFind"
		Me.tx_検索製品.AutoSize = False
		Me.tx_検索製品.Size = New System.Drawing.Size(93, 19)
		Me.tx_検索製品.Location = New System.Drawing.Point(99, 45)
		Me.tx_検索製品.TabIndex = 0
		Me.tx_検索製品.Text = ""
		Me.tx_検索製品.Maxlength = 7
		Me.tx_検索製品.AcceptsReturn = True
		Me.tx_検索製品.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_検索製品.BackColor = System.Drawing.SystemColors.Window
		Me.tx_検索製品.CausesValidation = True
		Me.tx_検索製品.Enabled = True
		Me.tx_検索製品.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_検索製品.HideSelection = True
		Me.tx_検索製品.ReadOnly = False
		Me.tx_検索製品.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_検索製品.MultiLine = False
		Me.tx_検索製品.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_検索製品.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_検索製品.TabStop = True
		Me.tx_検索製品.Visible = True
		Me.tx_検索製品.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_検索製品.Name = "tx_検索製品"
		Me.tx_検索名称.AutoSize = False
		Me.tx_検索名称.Size = New System.Drawing.Size(267, 19)
		Me.tx_検索名称.Location = New System.Drawing.Point(99, 83)
		Me.tx_検索名称.TabIndex = 2
		Me.tx_検索名称.Text = ""
		Me.tx_検索名称.Maxlength = 40
		Me.tx_検索名称.AcceptsReturn = True
		Me.tx_検索名称.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_検索名称.BackColor = System.Drawing.SystemColors.Window
		Me.tx_検索名称.CausesValidation = True
		Me.tx_検索名称.Enabled = True
		Me.tx_検索名称.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_検索名称.HideSelection = True
		Me.tx_検索名称.ReadOnly = False
		Me.tx_検索名称.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_検索名称.MultiLine = False
		Me.tx_検索名称.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_検索名称.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_検索名称.TabStop = True
		Me.tx_検索名称.Visible = True
		Me.tx_検索名称.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_検索名称.Name = "tx_検索名称"
		Me.tx_検索仕入先.AutoSize = False
		Me.tx_検索仕入先.Size = New System.Drawing.Size(67, 19)
		Me.tx_検索仕入先.Location = New System.Drawing.Point(99, 102)
		Me.tx_検索仕入先.TabIndex = 3
		Me.tx_検索仕入先.Text = ""
		Me.tx_検索仕入先.Maxlength = 4
		Me.tx_検索仕入先.AcceptsReturn = True
		Me.tx_検索仕入先.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_検索仕入先.BackColor = System.Drawing.SystemColors.Window
		Me.tx_検索仕入先.CausesValidation = True
		Me.tx_検索仕入先.Enabled = True
		Me.tx_検索仕入先.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_検索仕入先.HideSelection = True
		Me.tx_検索仕入先.ReadOnly = False
		Me.tx_検索仕入先.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_検索仕入先.MultiLine = False
		Me.tx_検索仕入先.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_検索仕入先.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_検索仕入先.TabStop = True
		Me.tx_検索仕入先.Visible = True
		Me.tx_検索仕入先.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_検索仕入先.Name = "tx_検索仕入先"
		Me.tx_検索仕様.AutoSize = False
		Me.tx_検索仕様.Size = New System.Drawing.Size(79, 19)
		Me.tx_検索仕様.Location = New System.Drawing.Point(99, 64)
		Me.tx_検索仕様.TabIndex = 1
		Me.tx_検索仕様.Text = ""
		Me.tx_検索仕様.Maxlength = 7
		Me.tx_検索仕様.AcceptsReturn = True
		Me.tx_検索仕様.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_検索仕様.BackColor = System.Drawing.SystemColors.Window
		Me.tx_検索仕様.CausesValidation = True
		Me.tx_検索仕様.Enabled = True
		Me.tx_検索仕様.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_検索仕様.HideSelection = True
		Me.tx_検索仕様.ReadOnly = False
		Me.tx_検索仕様.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_検索仕様.MultiLine = False
		Me.tx_検索仕様.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_検索仕様.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_検索仕様.TabStop = True
		Me.tx_検索仕様.Visible = True
		Me.tx_検索仕様.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_検索仕様.Name = "tx_検索仕様"
		Me.tx_W.AutoSize = False
		Me.tx_W.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_W.Size = New System.Drawing.Size(52, 19)
		Me.tx_W.Location = New System.Drawing.Point(481, 45)
		Me.tx_W.TabIndex = 4
		Me.tx_W.Text = "9999"
		Me.tx_W.Maxlength = 4
		Me.tx_W.AcceptsReturn = True
		Me.tx_W.BackColor = System.Drawing.SystemColors.Window
		Me.tx_W.CausesValidation = True
		Me.tx_W.Enabled = True
		Me.tx_W.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_W.HideSelection = True
		Me.tx_W.ReadOnly = False
		Me.tx_W.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_W.MultiLine = False
		Me.tx_W.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_W.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_W.TabStop = True
		Me.tx_W.Visible = True
		Me.tx_W.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_W.Name = "tx_W"
		Me.tx_D.AutoSize = False
		Me.tx_D.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_D.Size = New System.Drawing.Size(52, 19)
		Me.tx_D.Location = New System.Drawing.Point(481, 64)
		Me.tx_D.TabIndex = 5
		Me.tx_D.Text = "9999"
		Me.tx_D.Maxlength = 4
		Me.tx_D.AcceptsReturn = True
		Me.tx_D.BackColor = System.Drawing.SystemColors.Window
		Me.tx_D.CausesValidation = True
		Me.tx_D.Enabled = True
		Me.tx_D.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_D.HideSelection = True
		Me.tx_D.ReadOnly = False
		Me.tx_D.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_D.MultiLine = False
		Me.tx_D.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_D.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_D.TabStop = True
		Me.tx_D.Visible = True
		Me.tx_D.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_D.Name = "tx_D"
		Me.tx_H.AutoSize = False
		Me.tx_H.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_H.Size = New System.Drawing.Size(52, 19)
		Me.tx_H.Location = New System.Drawing.Point(481, 83)
		Me.tx_H.TabIndex = 6
		Me.tx_H.Text = "9999"
		Me.tx_H.Maxlength = 4
		Me.tx_H.AcceptsReturn = True
		Me.tx_H.BackColor = System.Drawing.SystemColors.Window
		Me.tx_H.CausesValidation = True
		Me.tx_H.Enabled = True
		Me.tx_H.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_H.HideSelection = True
		Me.tx_H.ReadOnly = False
		Me.tx_H.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_H.MultiLine = False
		Me.tx_H.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_H.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_H.TabStop = True
		Me.tx_H.Visible = True
		Me.tx_H.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_H.Name = "tx_H"
		Me.tx_D2.AutoSize = False
		Me.tx_D2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_D2.Size = New System.Drawing.Size(52, 19)
		Me.tx_D2.Location = New System.Drawing.Point(641, 64)
		Me.tx_D2.TabIndex = 8
		Me.tx_D2.Text = "9999"
		Me.tx_D2.Maxlength = 4
		Me.tx_D2.AcceptsReturn = True
		Me.tx_D2.BackColor = System.Drawing.SystemColors.Window
		Me.tx_D2.CausesValidation = True
		Me.tx_D2.Enabled = True
		Me.tx_D2.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_D2.HideSelection = True
		Me.tx_D2.ReadOnly = False
		Me.tx_D2.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_D2.MultiLine = False
		Me.tx_D2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_D2.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_D2.TabStop = True
		Me.tx_D2.Visible = True
		Me.tx_D2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_D2.Name = "tx_D2"
		Me.tx_D1.AutoSize = False
		Me.tx_D1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_D1.Size = New System.Drawing.Size(52, 19)
		Me.tx_D1.Location = New System.Drawing.Point(641, 45)
		Me.tx_D1.TabIndex = 7
		Me.tx_D1.Text = "9999"
		Me.tx_D1.Maxlength = 4
		Me.tx_D1.AcceptsReturn = True
		Me.tx_D1.BackColor = System.Drawing.SystemColors.Window
		Me.tx_D1.CausesValidation = True
		Me.tx_D1.Enabled = True
		Me.tx_D1.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_D1.HideSelection = True
		Me.tx_D1.ReadOnly = False
		Me.tx_D1.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_D1.MultiLine = False
		Me.tx_D1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_D1.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_D1.TabStop = True
		Me.tx_D1.Visible = True
		Me.tx_D1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_D1.Name = "tx_D1"
		Me.tx_H2.AutoSize = False
		Me.tx_H2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_H2.Size = New System.Drawing.Size(52, 19)
		Me.tx_H2.Location = New System.Drawing.Point(641, 102)
		Me.tx_H2.TabIndex = 10
		Me.tx_H2.Text = "9999"
		Me.tx_H2.Maxlength = 4
		Me.tx_H2.AcceptsReturn = True
		Me.tx_H2.BackColor = System.Drawing.SystemColors.Window
		Me.tx_H2.CausesValidation = True
		Me.tx_H2.Enabled = True
		Me.tx_H2.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_H2.HideSelection = True
		Me.tx_H2.ReadOnly = False
		Me.tx_H2.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_H2.MultiLine = False
		Me.tx_H2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_H2.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_H2.TabStop = True
		Me.tx_H2.Visible = True
		Me.tx_H2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_H2.Name = "tx_H2"
		Me.tx_H1.AutoSize = False
		Me.tx_H1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_H1.Size = New System.Drawing.Size(52, 19)
		Me.tx_H1.Location = New System.Drawing.Point(641, 83)
		Me.tx_H1.TabIndex = 9
		Me.tx_H1.Text = "9999"
		Me.tx_H1.Maxlength = 4
		Me.tx_H1.AcceptsReturn = True
		Me.tx_H1.BackColor = System.Drawing.SystemColors.Window
		Me.tx_H1.CausesValidation = True
		Me.tx_H1.Enabled = True
		Me.tx_H1.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_H1.HideSelection = True
		Me.tx_H1.ReadOnly = False
		Me.tx_H1.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_H1.MultiLine = False
		Me.tx_H1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_H1.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_H1.TabStop = True
		Me.tx_H1.Visible = True
		Me.tx_H1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_H1.Name = "tx_H1"
		Me._lb_項目_10.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_項目_10.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me._lb_項目_10.Text = "Ｈ１"
		Me._lb_項目_10.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_10.ForeColor = System.Drawing.Color.White
		Me._lb_項目_10.Size = New System.Drawing.Size(89, 19)
		Me._lb_項目_10.Location = New System.Drawing.Point(552, 83)
		Me._lb_項目_10.TabIndex = 28
		Me._lb_項目_10.Enabled = True
		Me._lb_項目_10.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_10.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_10.UseMnemonic = True
		Me._lb_項目_10.Visible = True
		Me._lb_項目_10.AutoSize = False
		Me._lb_項目_10.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._lb_項目_10.Name = "_lb_項目_10"
		Me._lb_項目_9.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_項目_9.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me._lb_項目_9.Text = "Ｈ２"
		Me._lb_項目_9.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_9.ForeColor = System.Drawing.Color.White
		Me._lb_項目_9.Size = New System.Drawing.Size(89, 19)
		Me._lb_項目_9.Location = New System.Drawing.Point(552, 102)
		Me._lb_項目_9.TabIndex = 27
		Me._lb_項目_9.Enabled = True
		Me._lb_項目_9.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_9.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_9.UseMnemonic = True
		Me._lb_項目_9.Visible = True
		Me._lb_項目_9.AutoSize = False
		Me._lb_項目_9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._lb_項目_9.Name = "_lb_項目_9"
		Me._lb_項目_7.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_項目_7.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me._lb_項目_7.Text = "Ｄ１"
		Me._lb_項目_7.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_7.ForeColor = System.Drawing.Color.White
		Me._lb_項目_7.Size = New System.Drawing.Size(89, 19)
		Me._lb_項目_7.Location = New System.Drawing.Point(552, 45)
		Me._lb_項目_7.TabIndex = 26
		Me._lb_項目_7.Enabled = True
		Me._lb_項目_7.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_7.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_7.UseMnemonic = True
		Me._lb_項目_7.Visible = True
		Me._lb_項目_7.AutoSize = False
		Me._lb_項目_7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._lb_項目_7.Name = "_lb_項目_7"
		Me._lb_項目_8.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_項目_8.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me._lb_項目_8.Text = "Ｄ２"
		Me._lb_項目_8.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_8.ForeColor = System.Drawing.Color.White
		Me._lb_項目_8.Size = New System.Drawing.Size(89, 19)
		Me._lb_項目_8.Location = New System.Drawing.Point(552, 64)
		Me._lb_項目_8.TabIndex = 25
		Me._lb_項目_8.Enabled = True
		Me._lb_項目_8.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_8.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_8.UseMnemonic = True
		Me._lb_項目_8.Visible = True
		Me._lb_項目_8.AutoSize = False
		Me._lb_項目_8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._lb_項目_8.Name = "_lb_項目_8"
		Me._lb_項目_6.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_項目_6.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me._lb_項目_6.Text = "Ｈ"
		Me._lb_項目_6.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_6.ForeColor = System.Drawing.Color.White
		Me._lb_項目_6.Size = New System.Drawing.Size(89, 19)
		Me._lb_項目_6.Location = New System.Drawing.Point(392, 83)
		Me._lb_項目_6.TabIndex = 24
		Me._lb_項目_6.Enabled = True
		Me._lb_項目_6.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_6.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_6.UseMnemonic = True
		Me._lb_項目_6.Visible = True
		Me._lb_項目_6.AutoSize = False
		Me._lb_項目_6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._lb_項目_6.Name = "_lb_項目_6"
		Me._lb_項目_5.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_項目_5.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me._lb_項目_5.Text = "Ｄ"
		Me._lb_項目_5.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_5.ForeColor = System.Drawing.Color.White
		Me._lb_項目_5.Size = New System.Drawing.Size(89, 19)
		Me._lb_項目_5.Location = New System.Drawing.Point(392, 64)
		Me._lb_項目_5.TabIndex = 23
		Me._lb_項目_5.Enabled = True
		Me._lb_項目_5.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_5.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_5.UseMnemonic = True
		Me._lb_項目_5.Visible = True
		Me._lb_項目_5.AutoSize = False
		Me._lb_項目_5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._lb_項目_5.Name = "_lb_項目_5"
		Me._lb_項目_4.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_項目_4.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me._lb_項目_4.Text = "Ｗ"
		Me._lb_項目_4.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_4.ForeColor = System.Drawing.Color.White
		Me._lb_項目_4.Size = New System.Drawing.Size(89, 19)
		Me._lb_項目_4.Location = New System.Drawing.Point(392, 45)
		Me._lb_項目_4.TabIndex = 22
		Me._lb_項目_4.Enabled = True
		Me._lb_項目_4.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_4.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_4.UseMnemonic = True
		Me._lb_項目_4.Visible = True
		Me._lb_項目_4.AutoSize = False
		Me._lb_項目_4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._lb_項目_4.Name = "_lb_項目_4"
		Me._lb_項目_1.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_項目_1.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me._lb_項目_1.Text = "仕様NO"
		Me._lb_項目_1.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_1.ForeColor = System.Drawing.Color.White
		Me._lb_項目_1.Size = New System.Drawing.Size(89, 19)
		Me._lb_項目_1.Location = New System.Drawing.Point(10, 64)
		Me._lb_項目_1.TabIndex = 21
		Me._lb_項目_1.Enabled = True
		Me._lb_項目_1.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_1.UseMnemonic = True
		Me._lb_項目_1.Visible = True
		Me._lb_項目_1.AutoSize = False
		Me._lb_項目_1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._lb_項目_1.Name = "_lb_項目_1"
		Me._lb_項目_2.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_項目_2.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me._lb_項目_2.Text = "漢字名称"
		Me._lb_項目_2.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_2.ForeColor = System.Drawing.Color.White
		Me._lb_項目_2.Size = New System.Drawing.Size(89, 19)
		Me._lb_項目_2.Location = New System.Drawing.Point(10, 83)
		Me._lb_項目_2.TabIndex = 20
		Me._lb_項目_2.Enabled = True
		Me._lb_項目_2.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_2.UseMnemonic = True
		Me._lb_項目_2.Visible = True
		Me._lb_項目_2.AutoSize = False
		Me._lb_項目_2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._lb_項目_2.Name = "_lb_項目_2"
		Me.lbGuide.Text = "[↑][↓]で選択、[Enter]で決定 ／[Esc]で中止"
		Me.lbGuide.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.lbGuide.Size = New System.Drawing.Size(297, 17)
		Me.lbGuide.Location = New System.Drawing.Point(11, 25)
		Me.lbGuide.TabIndex = 19
		Me.lbGuide.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lbGuide.BackColor = System.Drawing.SystemColors.Control
		Me.lbGuide.Enabled = True
		Me.lbGuide.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lbGuide.Cursor = System.Windows.Forms.Cursors.Default
		Me.lbGuide.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lbGuide.UseMnemonic = True
		Me.lbGuide.Visible = True
		Me.lbGuide.AutoSize = False
		Me.lbGuide.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lbGuide.Name = "lbGuide"
		Me.lbListCount.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lbListCount.BackColor = System.Drawing.Color.FromARGB(0, 0, 128)
		Me.lbListCount.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.lbListCount.ForeColor = System.Drawing.Color.Yellow
		Me.lbListCount.Size = New System.Drawing.Size(89, 17)
		Me.lbListCount.Location = New System.Drawing.Point(79, 5)
		Me.lbListCount.TabIndex = 18
		Me.lbListCount.Enabled = True
		Me.lbListCount.Cursor = System.Windows.Forms.Cursors.Default
		Me.lbListCount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lbListCount.UseMnemonic = True
		Me.lbListCount.Visible = True
		Me.lbListCount.AutoSize = False
		Me.lbListCount.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lbListCount.Name = "lbListCount"
		Me.lb_該当件数.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me.lb_該当件数.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me.lb_該当件数.Text = "該当件数"
		Me.lb_該当件数.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.lb_該当件数.Size = New System.Drawing.Size(69, 17)
		Me.lb_該当件数.Location = New System.Drawing.Point(11, 5)
		Me.lb_該当件数.TabIndex = 17
		Me.lb_該当件数.Enabled = True
		Me.lb_該当件数.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lb_該当件数.Cursor = System.Windows.Forms.Cursors.Default
		Me.lb_該当件数.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lb_該当件数.UseMnemonic = True
		Me.lb_該当件数.Visible = True
		Me.lb_該当件数.AutoSize = False
		Me.lb_該当件数.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lb_該当件数.Name = "lb_該当件数"
		Me._lb_項目_3.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_項目_3.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me._lb_項目_3.Text = "主仕入先"
		Me._lb_項目_3.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_3.ForeColor = System.Drawing.Color.White
		Me._lb_項目_3.Size = New System.Drawing.Size(89, 19)
		Me._lb_項目_3.Location = New System.Drawing.Point(10, 102)
		Me._lb_項目_3.TabIndex = 16
		Me._lb_項目_3.Enabled = True
		Me._lb_項目_3.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_3.UseMnemonic = True
		Me._lb_項目_3.Visible = True
		Me._lb_項目_3.AutoSize = False
		Me._lb_項目_3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._lb_項目_3.Name = "_lb_項目_3"
		Me._lb_項目_0.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_項目_0.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me._lb_項目_0.Text = "製品NO"
		Me._lb_項目_0.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_0.ForeColor = System.Drawing.Color.White
		Me._lb_項目_0.Size = New System.Drawing.Size(89, 19)
		Me._lb_項目_0.Location = New System.Drawing.Point(10, 45)
		Me._lb_項目_0.TabIndex = 15
		Me._lb_項目_0.Enabled = True
		Me._lb_項目_0.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_0.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_0.UseMnemonic = True
		Me._lb_項目_0.Visible = True
		Me._lb_項目_0.AutoSize = False
		Me._lb_項目_0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._lb_項目_0.Name = "_lb_項目_0"
		Me.Controls.Add(SelListVw)
		Me.Controls.Add(cmdOk)
		Me.Controls.Add(cmdCan)
		Me.Controls.Add(cmdFind)
		Me.Controls.Add(tx_検索製品)
		Me.Controls.Add(tx_検索名称)
		Me.Controls.Add(tx_検索仕入先)
		Me.Controls.Add(tx_検索仕様)
		Me.Controls.Add(tx_W)
		Me.Controls.Add(tx_D)
		Me.Controls.Add(tx_H)
		Me.Controls.Add(tx_D2)
		Me.Controls.Add(tx_D1)
		Me.Controls.Add(tx_H2)
		Me.Controls.Add(tx_H1)
		Me.Controls.Add(_lb_項目_10)
		Me.Controls.Add(_lb_項目_9)
		Me.Controls.Add(_lb_項目_7)
		Me.Controls.Add(_lb_項目_8)
		Me.Controls.Add(_lb_項目_6)
		Me.Controls.Add(_lb_項目_5)
		Me.Controls.Add(_lb_項目_4)
		Me.Controls.Add(_lb_項目_1)
		Me.Controls.Add(_lb_項目_2)
		Me.Controls.Add(lbGuide)
		Me.Controls.Add(lbListCount)
		Me.Controls.Add(lb_該当件数)
		Me.Controls.Add(_lb_項目_3)
		Me.Controls.Add(_lb_項目_0)
		Me.lb_項目.SetIndex(_lb_項目_10, CType(10, Short))
		Me.lb_項目.SetIndex(_lb_項目_9, CType(9, Short))
		Me.lb_項目.SetIndex(_lb_項目_7, CType(7, Short))
		Me.lb_項目.SetIndex(_lb_項目_8, CType(8, Short))
		Me.lb_項目.SetIndex(_lb_項目_6, CType(6, Short))
		Me.lb_項目.SetIndex(_lb_項目_5, CType(5, Short))
		Me.lb_項目.SetIndex(_lb_項目_4, CType(4, Short))
		Me.lb_項目.SetIndex(_lb_項目_1, CType(1, Short))
		Me.lb_項目.SetIndex(_lb_項目_2, CType(2, Short))
		Me.lb_項目.SetIndex(_lb_項目_3, CType(3, Short))
		Me.lb_項目.SetIndex(_lb_項目_0, CType(0, Short))
		CType(Me.lb_項目, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()
	End Sub
#End Region 
End Class