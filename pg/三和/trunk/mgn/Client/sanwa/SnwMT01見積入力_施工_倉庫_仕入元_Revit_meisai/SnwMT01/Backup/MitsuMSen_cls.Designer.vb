<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class MitsuMSen_cls
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
	Public WithEvents ck_NotZero As System.Windows.Forms.CheckBox
	Public WithEvents SelListVw As System.Windows.Forms.ListView
	Public WithEvents cmdOk As System.Windows.Forms.Button
	Public WithEvents cmdCan As System.Windows.Forms.Button
	Public WithEvents cmdFind As System.Windows.Forms.Button
	Public WithEvents tx_検索製品 As System.Windows.Forms.TextBox
	Public WithEvents tx_検索名称 As System.Windows.Forms.TextBox
	Public WithEvents tx_検索仕様 As System.Windows.Forms.TextBox
	Public WithEvents tx_W As System.Windows.Forms.TextBox
	Public WithEvents tx_D As System.Windows.Forms.TextBox
	Public WithEvents tx_H As System.Windows.Forms.TextBox
	Public WithEvents tx_得意先CD As System.Windows.Forms.TextBox
	Public WithEvents tx_仕入先CD As System.Windows.Forms.TextBox
	Public WithEvents tx_納入先CD As System.Windows.Forms.TextBox
	Public WithEvents tx_見積件名 As System.Windows.Forms.TextBox
	Public WithEvents tx_仕入業者CD As System.Windows.Forms.TextBox
	Public WithEvents _lb_項目_9 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_7 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_2 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_8 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_3 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_12 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_11 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_6 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_5 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_4 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_1 As System.Windows.Forms.Label
	Public WithEvents lbGuide As System.Windows.Forms.Label
	Public WithEvents lbListCount As System.Windows.Forms.Label
	Public WithEvents lb_該当件数 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_0 As System.Windows.Forms.Label
	Public WithEvents lb_項目 As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
	'Windows フォーム デザイナを使って変更できます。
	'コード エディタを使用して、変更しないでください。
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MitsuMSen_cls))
		Me.components = New System.ComponentModel.Container()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
		Me.ck_NotZero = New System.Windows.Forms.CheckBox
		Me.SelListVw = New System.Windows.Forms.ListView
		Me.cmdOk = New System.Windows.Forms.Button
		Me.cmdCan = New System.Windows.Forms.Button
		Me.cmdFind = New System.Windows.Forms.Button
		Me.tx_検索製品 = New System.Windows.Forms.TextBox
		Me.tx_検索名称 = New System.Windows.Forms.TextBox
		Me.tx_検索仕様 = New System.Windows.Forms.TextBox
		Me.tx_W = New System.Windows.Forms.TextBox
		Me.tx_D = New System.Windows.Forms.TextBox
		Me.tx_H = New System.Windows.Forms.TextBox
		Me.tx_得意先CD = New System.Windows.Forms.TextBox
		Me.tx_仕入先CD = New System.Windows.Forms.TextBox
		Me.tx_納入先CD = New System.Windows.Forms.TextBox
		Me.tx_見積件名 = New System.Windows.Forms.TextBox
		Me.tx_仕入業者CD = New System.Windows.Forms.TextBox
		Me._lb_項目_9 = New System.Windows.Forms.Label
		Me._lb_項目_7 = New System.Windows.Forms.Label
		Me._lb_項目_2 = New System.Windows.Forms.Label
		Me._lb_項目_8 = New System.Windows.Forms.Label
		Me._lb_項目_3 = New System.Windows.Forms.Label
		Me._lb_項目_12 = New System.Windows.Forms.Label
		Me._lb_項目_11 = New System.Windows.Forms.Label
		Me._lb_項目_6 = New System.Windows.Forms.Label
		Me._lb_項目_5 = New System.Windows.Forms.Label
		Me._lb_項目_4 = New System.Windows.Forms.Label
		Me._lb_項目_1 = New System.Windows.Forms.Label
		Me.lbGuide = New System.Windows.Forms.Label
		Me.lbListCount = New System.Windows.Forms.Label
		Me.lb_該当件数 = New System.Windows.Forms.Label
		Me._lb_項目_0 = New System.Windows.Forms.Label
		Me.lb_項目 = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(components)
		Me.SuspendLayout()
		Me.ToolTip1.Active = True
		CType(Me.lb_項目, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.Text = "見積明細検索"
		Me.ClientSize = New System.Drawing.Size(804, 495)
		Me.Location = New System.Drawing.Point(4, 23)
		Me.Icon = CType(resources.GetObject("MitsuMSen_cls.Icon"), System.Drawing.Icon)
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
		Me.Name = "MitsuMSen_cls"
		Me.ck_NotZero.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.ck_NotZero.ForeColor = System.Drawing.Color.Black
		Me.ck_NotZero.Size = New System.Drawing.Size(17, 19)
		Me.ck_NotZero.Location = New System.Drawing.Point(664, 104)
		Me.ck_NotZero.TabIndex = 11
		Me.ck_NotZero.CheckState = System.Windows.Forms.CheckState.Checked
		Me.ck_NotZero.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.ck_NotZero.FlatStyle = System.Windows.Forms.FlatStyle.Standard
		Me.ck_NotZero.BackColor = System.Drawing.SystemColors.Control
		Me.ck_NotZero.Text = ""
		Me.ck_NotZero.CausesValidation = True
		Me.ck_NotZero.Enabled = True
		Me.ck_NotZero.Cursor = System.Windows.Forms.Cursors.Default
		Me.ck_NotZero.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ck_NotZero.Appearance = System.Windows.Forms.Appearance.Normal
		Me.ck_NotZero.TabStop = True
		Me.ck_NotZero.Visible = True
		Me.ck_NotZero.Name = "ck_NotZero"
		Me.SelListVw.Size = New System.Drawing.Size(687, 316)
		Me.SelListVw.Location = New System.Drawing.Point(12, 148)
		Me.SelListVw.TabIndex = 13
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
		Me.cmdOk.TabIndex = 14
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
		Me.cmdCan.TabIndex = 15
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
		Me.cmdFind.TabIndex = 12
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
		Me.tx_検索製品.Location = New System.Drawing.Point(263, 64)
		Me.tx_検索製品.TabIndex = 5
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
		Me.tx_検索名称.Location = New System.Drawing.Point(263, 102)
		Me.tx_検索名称.TabIndex = 7
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
		Me.tx_検索仕様.AutoSize = False
		Me.tx_検索仕様.Size = New System.Drawing.Size(79, 19)
		Me.tx_検索仕様.Location = New System.Drawing.Point(263, 83)
		Me.tx_検索仕様.TabIndex = 6
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
		Me.tx_W.Location = New System.Drawing.Point(645, 45)
		Me.tx_W.TabIndex = 8
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
		Me.tx_D.Location = New System.Drawing.Point(645, 64)
		Me.tx_D.TabIndex = 9
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
		Me.tx_H.Location = New System.Drawing.Point(645, 83)
		Me.tx_H.TabIndex = 10
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
		Me.tx_得意先CD.AutoSize = False
		Me.tx_得意先CD.Size = New System.Drawing.Size(50, 20)
		Me.tx_得意先CD.Location = New System.Drawing.Point(92, 45)
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
		Me.tx_仕入先CD.Location = New System.Drawing.Point(92, 104)
		Me.tx_仕入先CD.TabIndex = 3
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
		Me.tx_納入先CD.AutoSize = False
		Me.tx_納入先CD.Size = New System.Drawing.Size(50, 19)
		Me.tx_納入先CD.Location = New System.Drawing.Point(92, 65)
		Me.tx_納入先CD.TabIndex = 1
		Me.tx_納入先CD.Text = "1234"
		Me.tx_納入先CD.Maxlength = 4
		Me.tx_納入先CD.AcceptsReturn = True
		Me.tx_納入先CD.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_納入先CD.BackColor = System.Drawing.SystemColors.Window
		Me.tx_納入先CD.CausesValidation = True
		Me.tx_納入先CD.Enabled = True
		Me.tx_納入先CD.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_納入先CD.HideSelection = True
		Me.tx_納入先CD.ReadOnly = False
		Me.tx_納入先CD.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_納入先CD.MultiLine = False
		Me.tx_納入先CD.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_納入先CD.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_納入先CD.TabStop = True
		Me.tx_納入先CD.Visible = True
		Me.tx_納入先CD.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_納入先CD.Name = "tx_納入先CD"
		Me.tx_見積件名.AutoSize = False
		Me.tx_見積件名.Size = New System.Drawing.Size(288, 19)
		Me.tx_見積件名.Location = New System.Drawing.Point(263, 45)
		Me.tx_見積件名.TabIndex = 4
		Me.tx_見積件名.Text = "あいうえおあいうえおあいうえおあいうえお"
		Me.tx_見積件名.AcceptsReturn = True
		Me.tx_見積件名.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_見積件名.BackColor = System.Drawing.SystemColors.Window
		Me.tx_見積件名.CausesValidation = True
		Me.tx_見積件名.Enabled = True
		Me.tx_見積件名.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_見積件名.HideSelection = True
		Me.tx_見積件名.ReadOnly = False
		Me.tx_見積件名.Maxlength = 0
		Me.tx_見積件名.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_見積件名.MultiLine = False
		Me.tx_見積件名.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_見積件名.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_見積件名.TabStop = True
		Me.tx_見積件名.Visible = True
		Me.tx_見積件名.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_見積件名.Name = "tx_見積件名"
		Me.tx_仕入業者CD.AutoSize = False
		Me.tx_仕入業者CD.Size = New System.Drawing.Size(50, 20)
		Me.tx_仕入業者CD.Location = New System.Drawing.Point(92, 84)
		Me.tx_仕入業者CD.TabIndex = 2
		Me.tx_仕入業者CD.Text = "8888"
		Me.tx_仕入業者CD.Maxlength = 4
		Me.tx_仕入業者CD.AcceptsReturn = True
		Me.tx_仕入業者CD.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_仕入業者CD.BackColor = System.Drawing.SystemColors.Window
		Me.tx_仕入業者CD.CausesValidation = True
		Me.tx_仕入業者CD.Enabled = True
		Me.tx_仕入業者CD.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_仕入業者CD.HideSelection = True
		Me.tx_仕入業者CD.ReadOnly = False
		Me.tx_仕入業者CD.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_仕入業者CD.MultiLine = False
		Me.tx_仕入業者CD.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_仕入業者CD.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_仕入業者CD.TabStop = True
		Me.tx_仕入業者CD.Visible = True
		Me.tx_仕入業者CD.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_仕入業者CD.Name = "tx_仕入業者CD"
		Me._lb_項目_9.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_項目_9.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me._lb_項目_9.Text = "数量0以外"
		Me._lb_項目_9.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_9.ForeColor = System.Drawing.Color.White
		Me._lb_項目_9.Size = New System.Drawing.Size(89, 19)
		Me._lb_項目_9.Location = New System.Drawing.Point(556, 102)
		Me._lb_項目_9.TabIndex = 30
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
		Me._lb_項目_7.Text = "見積件名"
		Me._lb_項目_7.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_7.ForeColor = System.Drawing.Color.White
		Me._lb_項目_7.Size = New System.Drawing.Size(89, 19)
		Me._lb_項目_7.Location = New System.Drawing.Point(174, 45)
		Me._lb_項目_7.TabIndex = 29
		Me._lb_項目_7.Enabled = True
		Me._lb_項目_7.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_7.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_7.UseMnemonic = True
		Me._lb_項目_7.Visible = True
		Me._lb_項目_7.AutoSize = False
		Me._lb_項目_7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._lb_項目_7.Name = "_lb_項目_7"
		Me._lb_項目_2.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_項目_2.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me._lb_項目_2.Text = "漢字名称"
		Me._lb_項目_2.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_2.ForeColor = System.Drawing.Color.White
		Me._lb_項目_2.Size = New System.Drawing.Size(89, 19)
		Me._lb_項目_2.Location = New System.Drawing.Point(174, 102)
		Me._lb_項目_2.TabIndex = 28
		Me._lb_項目_2.Enabled = True
		Me._lb_項目_2.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_2.UseMnemonic = True
		Me._lb_項目_2.Visible = True
		Me._lb_項目_2.AutoSize = False
		Me._lb_項目_2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._lb_項目_2.Name = "_lb_項目_2"
		Me._lb_項目_8.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_項目_8.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me._lb_項目_8.Text = "仕入業者"
		Me._lb_項目_8.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_8.ForeColor = System.Drawing.Color.White
		Me._lb_項目_8.Size = New System.Drawing.Size(80, 20)
		Me._lb_項目_8.Location = New System.Drawing.Point(12, 84)
		Me._lb_項目_8.TabIndex = 27
		Me._lb_項目_8.Enabled = True
		Me._lb_項目_8.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_8.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_8.UseMnemonic = True
		Me._lb_項目_8.Visible = True
		Me._lb_項目_8.AutoSize = False
		Me._lb_項目_8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._lb_項目_8.Name = "_lb_項目_8"
		Me._lb_項目_3.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_項目_3.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me._lb_項目_3.Text = "納入先"
		Me._lb_項目_3.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_3.ForeColor = System.Drawing.Color.White
		Me._lb_項目_3.Size = New System.Drawing.Size(80, 19)
		Me._lb_項目_3.Location = New System.Drawing.Point(12, 65)
		Me._lb_項目_3.TabIndex = 26
		Me._lb_項目_3.Enabled = True
		Me._lb_項目_3.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_3.UseMnemonic = True
		Me._lb_項目_3.Visible = True
		Me._lb_項目_3.AutoSize = False
		Me._lb_項目_3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._lb_項目_3.Name = "_lb_項目_3"
		Me._lb_項目_12.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_項目_12.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me._lb_項目_12.Text = "出荷元"
		Me._lb_項目_12.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_12.ForeColor = System.Drawing.Color.White
		Me._lb_項目_12.Size = New System.Drawing.Size(80, 20)
		Me._lb_項目_12.Location = New System.Drawing.Point(12, 104)
		Me._lb_項目_12.TabIndex = 25
		Me._lb_項目_12.Enabled = True
		Me._lb_項目_12.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_12.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_12.UseMnemonic = True
		Me._lb_項目_12.Visible = True
		Me._lb_項目_12.AutoSize = False
		Me._lb_項目_12.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._lb_項目_12.Name = "_lb_項目_12"
		Me._lb_項目_11.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_項目_11.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me._lb_項目_11.Text = "得意先"
		Me._lb_項目_11.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_11.ForeColor = System.Drawing.Color.White
		Me._lb_項目_11.Size = New System.Drawing.Size(80, 20)
		Me._lb_項目_11.Location = New System.Drawing.Point(12, 45)
		Me._lb_項目_11.TabIndex = 24
		Me._lb_項目_11.Enabled = True
		Me._lb_項目_11.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_11.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_11.UseMnemonic = True
		Me._lb_項目_11.Visible = True
		Me._lb_項目_11.AutoSize = False
		Me._lb_項目_11.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._lb_項目_11.Name = "_lb_項目_11"
		Me._lb_項目_6.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_項目_6.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me._lb_項目_6.Text = "Ｈ"
		Me._lb_項目_6.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_6.ForeColor = System.Drawing.Color.White
		Me._lb_項目_6.Size = New System.Drawing.Size(89, 19)
		Me._lb_項目_6.Location = New System.Drawing.Point(556, 83)
		Me._lb_項目_6.TabIndex = 23
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
		Me._lb_項目_5.Location = New System.Drawing.Point(556, 64)
		Me._lb_項目_5.TabIndex = 22
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
		Me._lb_項目_4.Location = New System.Drawing.Point(556, 45)
		Me._lb_項目_4.TabIndex = 21
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
		Me._lb_項目_1.Location = New System.Drawing.Point(174, 83)
		Me._lb_項目_1.TabIndex = 20
		Me._lb_項目_1.Enabled = True
		Me._lb_項目_1.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_1.UseMnemonic = True
		Me._lb_項目_1.Visible = True
		Me._lb_項目_1.AutoSize = False
		Me._lb_項目_1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._lb_項目_1.Name = "_lb_項目_1"
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
		Me._lb_項目_0.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_項目_0.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me._lb_項目_0.Text = "製品NO"
		Me._lb_項目_0.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_0.ForeColor = System.Drawing.Color.White
		Me._lb_項目_0.Size = New System.Drawing.Size(89, 19)
		Me._lb_項目_0.Location = New System.Drawing.Point(174, 64)
		Me._lb_項目_0.TabIndex = 16
		Me._lb_項目_0.Enabled = True
		Me._lb_項目_0.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_0.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_0.UseMnemonic = True
		Me._lb_項目_0.Visible = True
		Me._lb_項目_0.AutoSize = False
		Me._lb_項目_0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._lb_項目_0.Name = "_lb_項目_0"
		Me.Controls.Add(ck_NotZero)
		Me.Controls.Add(SelListVw)
		Me.Controls.Add(cmdOk)
		Me.Controls.Add(cmdCan)
		Me.Controls.Add(cmdFind)
		Me.Controls.Add(tx_検索製品)
		Me.Controls.Add(tx_検索名称)
		Me.Controls.Add(tx_検索仕様)
		Me.Controls.Add(tx_W)
		Me.Controls.Add(tx_D)
		Me.Controls.Add(tx_H)
		Me.Controls.Add(tx_得意先CD)
		Me.Controls.Add(tx_仕入先CD)
		Me.Controls.Add(tx_納入先CD)
		Me.Controls.Add(tx_見積件名)
		Me.Controls.Add(tx_仕入業者CD)
		Me.Controls.Add(_lb_項目_9)
		Me.Controls.Add(_lb_項目_7)
		Me.Controls.Add(_lb_項目_2)
		Me.Controls.Add(_lb_項目_8)
		Me.Controls.Add(_lb_項目_3)
		Me.Controls.Add(_lb_項目_12)
		Me.Controls.Add(_lb_項目_11)
		Me.Controls.Add(_lb_項目_6)
		Me.Controls.Add(_lb_項目_5)
		Me.Controls.Add(_lb_項目_4)
		Me.Controls.Add(_lb_項目_1)
		Me.Controls.Add(lbGuide)
		Me.Controls.Add(lbListCount)
		Me.Controls.Add(lb_該当件数)
		Me.Controls.Add(_lb_項目_0)
		Me.lb_項目.SetIndex(_lb_項目_9, CType(9, Short))
		Me.lb_項目.SetIndex(_lb_項目_7, CType(7, Short))
		Me.lb_項目.SetIndex(_lb_項目_2, CType(2, Short))
		Me.lb_項目.SetIndex(_lb_項目_8, CType(8, Short))
		Me.lb_項目.SetIndex(_lb_項目_3, CType(3, Short))
		Me.lb_項目.SetIndex(_lb_項目_12, CType(12, Short))
		Me.lb_項目.SetIndex(_lb_項目_11, CType(11, Short))
		Me.lb_項目.SetIndex(_lb_項目_6, CType(6, Short))
		Me.lb_項目.SetIndex(_lb_項目_5, CType(5, Short))
		Me.lb_項目.SetIndex(_lb_項目_4, CType(4, Short))
		Me.lb_項目.SetIndex(_lb_項目_1, CType(1, Short))
		Me.lb_項目.SetIndex(_lb_項目_0, CType(0, Short))
		CType(Me.lb_項目, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()
	End Sub
#End Region 
End Class