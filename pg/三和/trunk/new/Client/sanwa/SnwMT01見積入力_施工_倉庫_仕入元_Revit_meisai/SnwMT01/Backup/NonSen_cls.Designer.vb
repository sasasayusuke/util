<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class NonSen_cls
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
	Public WithEvents tx_検索ID As System.Windows.Forms.TextBox
	Public WithEvents tx_検索名 As System.Windows.Forms.TextBox
	Public WithEvents tx_検索カナ As System.Windows.Forms.TextBox
	Public WithEvents tx_得意先CD As System.Windows.Forms.TextBox
	Public WithEvents _lb_項目_3 As System.Windows.Forms.Label
	Public WithEvents rf_得意先名 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_2 As System.Windows.Forms.Label
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
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(NonSen_cls))
		Me.components = New System.ComponentModel.Container()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
		Me.SelListVw = New System.Windows.Forms.ListView
		Me.cmdOk = New System.Windows.Forms.Button
		Me.cmdCan = New System.Windows.Forms.Button
		Me.cmdFind = New System.Windows.Forms.Button
		Me.tx_検索ID = New System.Windows.Forms.TextBox
		Me.tx_検索名 = New System.Windows.Forms.TextBox
		Me.tx_検索カナ = New System.Windows.Forms.TextBox
		Me.tx_得意先CD = New System.Windows.Forms.TextBox
		Me._lb_項目_3 = New System.Windows.Forms.Label
		Me.rf_得意先名 = New System.Windows.Forms.Label
		Me._lb_項目_2 = New System.Windows.Forms.Label
		Me._lb_項目_1 = New System.Windows.Forms.Label
		Me.lbGuide = New System.Windows.Forms.Label
		Me.lbListCount = New System.Windows.Forms.Label
		Me.lb_該当件数 = New System.Windows.Forms.Label
		Me._lb_項目_0 = New System.Windows.Forms.Label
		Me.lb_項目 = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(components)
		Me.SuspendLayout()
		Me.ToolTip1.Active = True
		CType(Me.lb_項目, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.Text = "納入先選択"
		Me.ClientSize = New System.Drawing.Size(486, 450)
		Me.Location = New System.Drawing.Point(4, 23)
		Me.Icon = CType(resources.GetObject("NonSen_cls.Icon"), System.Drawing.Icon)
		Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation
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
		Me.Name = "NonSen_cls"
		Me.SelListVw.Size = New System.Drawing.Size(357, 285)
		Me.SelListVw.Location = New System.Drawing.Point(8, 127)
		Me.SelListVw.TabIndex = 4
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
		Me.cmdOk.Location = New System.Drawing.Point(309, 420)
		Me.cmdOk.TabIndex = 5
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
		Me.cmdCan.Location = New System.Drawing.Point(392, 420)
		Me.cmdCan.TabIndex = 6
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
		Me.cmdFind.Location = New System.Drawing.Point(383, 99)
		Me.cmdFind.TabIndex = 3
		Me.cmdFind.BackColor = System.Drawing.SystemColors.Control
		Me.cmdFind.CausesValidation = True
		Me.cmdFind.Enabled = True
		Me.cmdFind.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdFind.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdFind.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdFind.TabStop = True
		Me.cmdFind.Name = "cmdFind"
		Me.tx_検索ID.AutoSize = False
		Me.tx_検索ID.Size = New System.Drawing.Size(47, 19)
		Me.tx_検索ID.Location = New System.Drawing.Point(99, 64)
		Me.tx_検索ID.TabIndex = 0
		Me.tx_検索ID.Text = "WWWW"
		Me.tx_検索ID.Maxlength = 4
		Me.tx_検索ID.AcceptsReturn = True
		Me.tx_検索ID.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_検索ID.BackColor = System.Drawing.SystemColors.Window
		Me.tx_検索ID.CausesValidation = True
		Me.tx_検索ID.Enabled = True
		Me.tx_検索ID.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_検索ID.HideSelection = True
		Me.tx_検索ID.ReadOnly = False
		Me.tx_検索ID.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_検索ID.MultiLine = False
		Me.tx_検索ID.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_検索ID.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_検索ID.TabStop = True
		Me.tx_検索ID.Visible = True
		Me.tx_検索ID.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_検索ID.Name = "tx_検索ID"
		Me.tx_検索名.AutoSize = False
		Me.tx_検索名.Size = New System.Drawing.Size(227, 19)
		Me.tx_検索名.Location = New System.Drawing.Point(99, 83)
		Me.tx_検索名.TabIndex = 1
		Me.tx_検索名.Text = "WWWWWWWWWWWWWWWWWWWWWWWWWWWW"
		Me.tx_検索名.Maxlength = 28
		Me.tx_検索名.AcceptsReturn = True
		Me.tx_検索名.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_検索名.BackColor = System.Drawing.SystemColors.Window
		Me.tx_検索名.CausesValidation = True
		Me.tx_検索名.Enabled = True
		Me.tx_検索名.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_検索名.HideSelection = True
		Me.tx_検索名.ReadOnly = False
		Me.tx_検索名.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_検索名.MultiLine = False
		Me.tx_検索名.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_検索名.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_検索名.TabStop = True
		Me.tx_検索名.Visible = True
		Me.tx_検索名.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_検索名.Name = "tx_検索名"
		Me.tx_検索カナ.AutoSize = False
		Me.tx_検索カナ.Size = New System.Drawing.Size(60, 19)
		Me.tx_検索カナ.Location = New System.Drawing.Point(99, 102)
		Me.tx_検索カナ.TabIndex = 2
		Me.tx_検索カナ.Text = "WWWWW"
		Me.tx_検索カナ.Maxlength = 5
		Me.tx_検索カナ.AcceptsReturn = True
		Me.tx_検索カナ.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_検索カナ.BackColor = System.Drawing.SystemColors.Window
		Me.tx_検索カナ.CausesValidation = True
		Me.tx_検索カナ.Enabled = True
		Me.tx_検索カナ.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_検索カナ.HideSelection = True
		Me.tx_検索カナ.ReadOnly = False
		Me.tx_検索カナ.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_検索カナ.MultiLine = False
		Me.tx_検索カナ.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_検索カナ.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_検索カナ.TabStop = True
		Me.tx_検索カナ.Visible = True
		Me.tx_検索カナ.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_検索カナ.Name = "tx_検索カナ"
		Me.tx_得意先CD.AutoSize = False
		Me.tx_得意先CD.Size = New System.Drawing.Size(47, 19)
		Me.tx_得意先CD.Location = New System.Drawing.Point(99, 45)
		Me.tx_得意先CD.TabIndex = 13
		Me.tx_得意先CD.Text = "1234"
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
		Me._lb_項目_3.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_項目_3.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me._lb_項目_3.Text = "得意先"
		Me._lb_項目_3.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_3.ForeColor = System.Drawing.Color.White
		Me._lb_項目_3.Size = New System.Drawing.Size(89, 19)
		Me._lb_項目_3.Location = New System.Drawing.Point(10, 45)
		Me._lb_項目_3.TabIndex = 15
		Me._lb_項目_3.Enabled = True
		Me._lb_項目_3.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_3.UseMnemonic = True
		Me._lb_項目_3.Visible = True
		Me._lb_項目_3.AutoSize = False
		Me._lb_項目_3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._lb_項目_3.Name = "_lb_項目_3"
		Me.rf_得意先名.BackColor = System.Drawing.Color.FromARGB(224, 224, 224)
		Me.rf_得意先名.Text = "１２３４５６７８９０１２３４"
		Me.rf_得意先名.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.rf_得意先名.Size = New System.Drawing.Size(220, 19)
		Me.rf_得意先名.Location = New System.Drawing.Point(146, 45)
		Me.rf_得意先名.TabIndex = 14
		Me.rf_得意先名.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.rf_得意先名.Enabled = True
		Me.rf_得意先名.ForeColor = System.Drawing.SystemColors.ControlText
		Me.rf_得意先名.Cursor = System.Windows.Forms.Cursors.Default
		Me.rf_得意先名.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.rf_得意先名.UseMnemonic = True
		Me.rf_得意先名.Visible = True
		Me.rf_得意先名.AutoSize = False
		Me.rf_得意先名.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.rf_得意先名.Name = "rf_得意先名"
		Me._lb_項目_2.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_項目_2.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me._lb_項目_2.Text = "フリガナ"
		Me._lb_項目_2.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_2.ForeColor = System.Drawing.Color.White
		Me._lb_項目_2.Size = New System.Drawing.Size(89, 19)
		Me._lb_項目_2.Location = New System.Drawing.Point(10, 102)
		Me._lb_項目_2.TabIndex = 12
		Me._lb_項目_2.Enabled = True
		Me._lb_項目_2.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_2.UseMnemonic = True
		Me._lb_項目_2.Visible = True
		Me._lb_項目_2.AutoSize = False
		Me._lb_項目_2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._lb_項目_2.Name = "_lb_項目_2"
		Me._lb_項目_1.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_項目_1.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me._lb_項目_1.Text = "納入先名1"
		Me._lb_項目_1.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_1.ForeColor = System.Drawing.Color.White
		Me._lb_項目_1.Size = New System.Drawing.Size(89, 19)
		Me._lb_項目_1.Location = New System.Drawing.Point(10, 83)
		Me._lb_項目_1.TabIndex = 11
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
		Me.lbGuide.TabIndex = 10
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
		Me.lbListCount.TabIndex = 9
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
		Me.lb_該当件数.TabIndex = 8
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
		Me._lb_項目_0.Text = "納入先CD"
		Me._lb_項目_0.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_0.ForeColor = System.Drawing.Color.White
		Me._lb_項目_0.Size = New System.Drawing.Size(89, 19)
		Me._lb_項目_0.Location = New System.Drawing.Point(10, 64)
		Me._lb_項目_0.TabIndex = 7
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
		Me.Controls.Add(tx_検索ID)
		Me.Controls.Add(tx_検索名)
		Me.Controls.Add(tx_検索カナ)
		Me.Controls.Add(tx_得意先CD)
		Me.Controls.Add(_lb_項目_3)
		Me.Controls.Add(rf_得意先名)
		Me.Controls.Add(_lb_項目_2)
		Me.Controls.Add(_lb_項目_1)
		Me.Controls.Add(lbGuide)
		Me.Controls.Add(lbListCount)
		Me.Controls.Add(lb_該当件数)
		Me.Controls.Add(_lb_項目_0)
		Me.lb_項目.SetIndex(_lb_項目_3, CType(3, Short))
		Me.lb_項目.SetIndex(_lb_項目_2, CType(2, Short))
		Me.lb_項目.SetIndex(_lb_項目_1, CType(1, Short))
		Me.lb_項目.SetIndex(_lb_項目_0, CType(0, Short))
		CType(Me.lb_項目, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()
	End Sub
#End Region 
End Class