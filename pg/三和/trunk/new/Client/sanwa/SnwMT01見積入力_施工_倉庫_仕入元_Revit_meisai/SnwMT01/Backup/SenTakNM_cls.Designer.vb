<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class SentakNM_cls
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
	Public WithEvents tx_検索ID As System.Windows.Forms.TextBox
	Public WithEvents SelListVw As System.Windows.Forms.ListView
	Public WithEvents cmdOk As System.Windows.Forms.Button
	Public WithEvents cmdCan As System.Windows.Forms.Button
	Public WithEvents cmdFind As System.Windows.Forms.Button
	Public WithEvents tx_検索名 As System.Windows.Forms.TextBox
	Public WithEvents lbGuide As System.Windows.Forms.Label
	Public WithEvents lbListCount As System.Windows.Forms.Label
	Public WithEvents lb_該当件数 As System.Windows.Forms.Label
	Public WithEvents lb_検索ID As System.Windows.Forms.Label
	Public WithEvents lb_検索名 As System.Windows.Forms.Label
	'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
	'Windows フォーム デザイナを使って変更できます。
	'コード エディタを使用して、変更しないでください。
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(SentakNM_cls))
		Me.components = New System.ComponentModel.Container()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
		Me.tx_検索ID = New System.Windows.Forms.TextBox
		Me.SelListVw = New System.Windows.Forms.ListView
		Me.cmdOk = New System.Windows.Forms.Button
		Me.cmdCan = New System.Windows.Forms.Button
		Me.cmdFind = New System.Windows.Forms.Button
		Me.tx_検索名 = New System.Windows.Forms.TextBox
		Me.lbGuide = New System.Windows.Forms.Label
		Me.lbListCount = New System.Windows.Forms.Label
		Me.lb_該当件数 = New System.Windows.Forms.Label
		Me.lb_検索ID = New System.Windows.Forms.Label
		Me.lb_検索名 = New System.Windows.Forms.Label
		Me.SuspendLayout()
		Me.ToolTip1.Active = True
		Me.Text = "Form1"
		Me.ClientSize = New System.Drawing.Size(486, 407)
		Me.Location = New System.Drawing.Point(4, 30)
		Me.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.Icon = CType(resources.GetObject("SentakNM_cls.Icon"), System.Drawing.Icon)
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
		Me.Name = "SentakNM_cls"
		Me.tx_検索ID.AutoSize = False
		Me.tx_検索ID.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_検索ID.Size = New System.Drawing.Size(67, 19)
		Me.tx_検索ID.Location = New System.Drawing.Point(99, 45)
		Me.tx_検索ID.TabIndex = 0
		Me.tx_検索ID.Text = ""
		Me.tx_検索ID.Maxlength = 12
		Me.tx_検索ID.AcceptsReturn = True
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
		Me.SelListVw.Size = New System.Drawing.Size(351, 278)
		Me.SelListVw.Location = New System.Drawing.Point(8, 89)
		Me.SelListVw.TabIndex = 3
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
		Me.cmdOk.Location = New System.Drawing.Point(309, 376)
		Me.cmdOk.TabIndex = 4
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
		Me.cmdCan.Location = New System.Drawing.Point(392, 376)
		Me.cmdCan.TabIndex = 5
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
		Me.cmdFind.Location = New System.Drawing.Point(383, 63)
		Me.cmdFind.TabIndex = 2
		Me.cmdFind.BackColor = System.Drawing.SystemColors.Control
		Me.cmdFind.CausesValidation = True
		Me.cmdFind.Enabled = True
		Me.cmdFind.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdFind.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdFind.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdFind.TabStop = True
		Me.cmdFind.Name = "cmdFind"
		Me.tx_検索名.AutoSize = False
		Me.tx_検索名.Size = New System.Drawing.Size(133, 19)
		Me.tx_検索名.Location = New System.Drawing.Point(99, 64)
		Me.tx_検索名.TabIndex = 1
		Me.tx_検索名.Text = ""
		Me.tx_検索名.Maxlength = 30
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
		Me.lb_検索ID.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me.lb_検索ID.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me.lb_検索ID.Text = "検索ID"
		Me.lb_検索ID.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.lb_検索ID.ForeColor = System.Drawing.Color.White
		Me.lb_検索ID.Size = New System.Drawing.Size(89, 19)
		Me.lb_検索ID.Location = New System.Drawing.Point(10, 45)
		Me.lb_検索ID.TabIndex = 7
		Me.lb_検索ID.Enabled = True
		Me.lb_検索ID.Cursor = System.Windows.Forms.Cursors.Default
		Me.lb_検索ID.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lb_検索ID.UseMnemonic = True
		Me.lb_検索ID.Visible = True
		Me.lb_検索ID.AutoSize = False
		Me.lb_検索ID.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lb_検索ID.Name = "lb_検索ID"
		Me.lb_検索名.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me.lb_検索名.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me.lb_検索名.Text = "検索名"
		Me.lb_検索名.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.lb_検索名.ForeColor = System.Drawing.Color.White
		Me.lb_検索名.Size = New System.Drawing.Size(89, 19)
		Me.lb_検索名.Location = New System.Drawing.Point(10, 64)
		Me.lb_検索名.TabIndex = 6
		Me.lb_検索名.Enabled = True
		Me.lb_検索名.Cursor = System.Windows.Forms.Cursors.Default
		Me.lb_検索名.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lb_検索名.UseMnemonic = True
		Me.lb_検索名.Visible = True
		Me.lb_検索名.AutoSize = False
		Me.lb_検索名.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lb_検索名.Name = "lb_検索名"
		Me.Controls.Add(tx_検索ID)
		Me.Controls.Add(SelListVw)
		Me.Controls.Add(cmdOk)
		Me.Controls.Add(cmdCan)
		Me.Controls.Add(cmdFind)
		Me.Controls.Add(tx_検索名)
		Me.Controls.Add(lbGuide)
		Me.Controls.Add(lbListCount)
		Me.Controls.Add(lb_該当件数)
		Me.Controls.Add(lb_検索ID)
		Me.Controls.Add(lb_検索名)
		Me.ResumeLayout(False)
		Me.PerformLayout()
	End Sub
#End Region 
End Class