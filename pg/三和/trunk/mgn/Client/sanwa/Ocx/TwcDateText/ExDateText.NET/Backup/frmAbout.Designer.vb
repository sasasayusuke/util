<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmAbout
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
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdSysInfo As System.Windows.Forms.Button
	Public WithEvents _Image_2 As System.Windows.Forms.PictureBox
	Public WithEvents _Image_1 As System.Windows.Forms.PictureBox
	Public WithEvents _Image_0 As System.Windows.Forms.PictureBox
	Public WithEvents lblCopyLight As System.Windows.Forms.Label
	Public WithEvents _Line1_1 As Microsoft.VisualBasic.PowerPacks.LineShape
	Public WithEvents lblDescription As System.Windows.Forms.Label
	Public WithEvents lblTitle As System.Windows.Forms.Label
	Public WithEvents _Line1_0 As Microsoft.VisualBasic.PowerPacks.LineShape
	Public WithEvents lblVersion As System.Windows.Forms.Label
	Public WithEvents lblDisclaimer As System.Windows.Forms.Label
	Public WithEvents Image As Microsoft.VisualBasic.Compatibility.VB6.PictureBoxArray
	Public WithEvents Line1 As LineShapeArray
	Public WithEvents ShapeContainer1 As Microsoft.VisualBasic.PowerPacks.ShapeContainer
	'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
	'Windows フォーム デザイナを使って変更できます。
	'コード エディタを使用して、変更しないでください。
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmAbout))
		Me.components = New System.ComponentModel.Container()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
		Me.ShapeContainer1 = New Microsoft.VisualBasic.PowerPacks.ShapeContainer
		Me.cmdOK = New System.Windows.Forms.Button
		Me.cmdSysInfo = New System.Windows.Forms.Button
		Me._Image_2 = New System.Windows.Forms.PictureBox
		Me._Image_1 = New System.Windows.Forms.PictureBox
		Me._Image_0 = New System.Windows.Forms.PictureBox
		Me.lblCopyLight = New System.Windows.Forms.Label
		Me._Line1_1 = New Microsoft.VisualBasic.PowerPacks.LineShape
		Me.lblDescription = New System.Windows.Forms.Label
		Me.lblTitle = New System.Windows.Forms.Label
		Me._Line1_0 = New Microsoft.VisualBasic.PowerPacks.LineShape
		Me.lblVersion = New System.Windows.Forms.Label
		Me.lblDisclaimer = New System.Windows.Forms.Label
		Me.Image = New Microsoft.VisualBasic.Compatibility.VB6.PictureBoxArray
		Me.Line1 = New LineShapeArray(components)
		Me.SuspendLayout()
		Me.ToolTip1.Active = True
		CType(Me.Line1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
		Me.Text = "TWC ExDateText  ﾊﾞｰｼﾞｮﾝ情報"
		Me.ClientSize = New System.Drawing.Size(382, 237)
		Me.Location = New System.Drawing.Point(156, 126)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ControlBox = True
		Me.Enabled = True
		Me.KeyPreview = False
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.HelpButton = False
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.Name = "frmAbout"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.CancelButton = Me.cmdOK
		Me.cmdOK.Text = "OK"
		Me.AcceptButton = Me.cmdOK
		Me.cmdOK.Size = New System.Drawing.Size(92, 23)
		Me.cmdOK.Location = New System.Drawing.Point(284, 175)
		Me.cmdOK.TabIndex = 0
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Enabled = True
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.TabStop = True
		Me.cmdOK.Name = "cmdOK"
		Me.cmdSysInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdSysInfo.Text = "ｼｽﾃﾑ情報(&S)..."
		Me.cmdSysInfo.Size = New System.Drawing.Size(92, 23)
		Me.cmdSysInfo.Location = New System.Drawing.Point(284, 205)
		Me.cmdSysInfo.TabIndex = 1
		Me.cmdSysInfo.BackColor = System.Drawing.SystemColors.Control
		Me.cmdSysInfo.CausesValidation = True
		Me.cmdSysInfo.Enabled = True
		Me.cmdSysInfo.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdSysInfo.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdSysInfo.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdSysInfo.TabStop = True
		Me.cmdSysInfo.Name = "cmdSysInfo"
		Me._Image_2.Size = New System.Drawing.Size(20, 20)
		Me._Image_2.Location = New System.Drawing.Point(44, 12)
		Me._Image_2.Image = CType(resources.GetObject("_Image_2.Image"), System.Drawing.Image)
		Me._Image_2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
		Me._Image_2.Enabled = True
		Me._Image_2.Cursor = System.Windows.Forms.Cursors.Default
		Me._Image_2.Visible = True
		Me._Image_2.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._Image_2.Name = "_Image_2"
		Me._Image_1.Size = New System.Drawing.Size(20, 20)
		Me._Image_1.Location = New System.Drawing.Point(24, 12)
		Me._Image_1.Image = CType(resources.GetObject("_Image_1.Image"), System.Drawing.Image)
		Me._Image_1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
		Me._Image_1.Enabled = True
		Me._Image_1.Cursor = System.Windows.Forms.Cursors.Default
		Me._Image_1.Visible = True
		Me._Image_1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._Image_1.Name = "_Image_1"
		Me._Image_0.Size = New System.Drawing.Size(20, 20)
		Me._Image_0.Location = New System.Drawing.Point(4, 12)
		Me._Image_0.Image = CType(resources.GetObject("_Image_0.Image"), System.Drawing.Image)
		Me._Image_0.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
		Me._Image_0.Enabled = True
		Me._Image_0.Cursor = System.Windows.Forms.Cursors.Default
		Me._Image_0.Visible = True
		Me._Image_0.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._Image_0.Name = "_Image_0"
		Me.lblCopyLight.Text = "ｱﾌﾟﾘｹｰｼｮﾝ ｺﾋﾟｰﾗｲﾄ"
		Me.lblCopyLight.ForeColor = System.Drawing.Color.Black
		Me.lblCopyLight.Size = New System.Drawing.Size(259, 16)
		Me.lblCopyLight.Location = New System.Drawing.Point(68, 60)
		Me.lblCopyLight.TabIndex = 6
		Me.lblCopyLight.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCopyLight.BackColor = System.Drawing.SystemColors.Control
		Me.lblCopyLight.Enabled = True
		Me.lblCopyLight.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCopyLight.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCopyLight.UseMnemonic = True
		Me.lblCopyLight.Visible = True
		Me.lblCopyLight.AutoSize = False
		Me.lblCopyLight.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCopyLight.Name = "lblCopyLight"
		Me._Line1_1.BorderColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me._Line1_1.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid
		Me._Line1_1.X1 = 6
		Me._Line1_1.X2 = 377
		Me._Line1_1.Y1 = 163
		Me._Line1_1.Y2 = 163
		Me._Line1_1.BorderWidth = 1
		Me._Line1_1.Visible = True
		Me._Line1_1.Name = "_Line1_1"
		Me.lblDescription.Text = "ｱﾌﾟﾘｹｰｼｮﾝの説明"
		Me.lblDescription.ForeColor = System.Drawing.Color.Black
		Me.lblDescription.Size = New System.Drawing.Size(259, 70)
		Me.lblDescription.Location = New System.Drawing.Point(68, 83)
		Me.lblDescription.TabIndex = 2
		Me.lblDescription.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDescription.BackColor = System.Drawing.SystemColors.Control
		Me.lblDescription.Enabled = True
		Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDescription.UseMnemonic = True
		Me.lblDescription.Visible = True
		Me.lblDescription.AutoSize = False
		Me.lblDescription.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDescription.Name = "lblDescription"
		Me.lblTitle.Text = "ｱﾌﾟﾘｹｰｼｮﾝ ﾀｲﾄﾙ"
		Me.lblTitle.ForeColor = System.Drawing.Color.Black
		Me.lblTitle.Size = New System.Drawing.Size(259, 16)
		Me.lblTitle.Location = New System.Drawing.Point(68, 16)
		Me.lblTitle.TabIndex = 4
		Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblTitle.BackColor = System.Drawing.SystemColors.Control
		Me.lblTitle.Enabled = True
		Me.lblTitle.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblTitle.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblTitle.UseMnemonic = True
		Me.lblTitle.Visible = True
		Me.lblTitle.AutoSize = False
		Me.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblTitle.Name = "lblTitle"
		Me._Line1_0.BorderColor = System.Drawing.Color.White
		Me._Line1_0.BorderWidth = 2
		Me._Line1_0.X1 = 7
		Me._Line1_0.X2 = 377
		Me._Line1_0.Y1 = 164
		Me._Line1_0.Y2 = 164
		Me._Line1_0.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid
		Me._Line1_0.Visible = True
		Me._Line1_0.Name = "_Line1_0"
		Me.lblVersion.Text = "ﾊﾞｰｼﾞｮﾝ"
		Me.lblVersion.Size = New System.Drawing.Size(259, 15)
		Me.lblVersion.Location = New System.Drawing.Point(68, 36)
		Me.lblVersion.TabIndex = 5
		Me.lblVersion.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblVersion.BackColor = System.Drawing.SystemColors.Control
		Me.lblVersion.Enabled = True
		Me.lblVersion.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblVersion.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblVersion.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblVersion.UseMnemonic = True
		Me.lblVersion.Visible = True
		Me.lblVersion.AutoSize = False
		Me.lblVersion.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblVersion.Name = "lblVersion"
		Me.lblDisclaimer.Text = "警告:"
		Me.lblDisclaimer.ForeColor = System.Drawing.Color.Black
		Me.lblDisclaimer.Size = New System.Drawing.Size(258, 55)
		Me.lblDisclaimer.Location = New System.Drawing.Point(17, 175)
		Me.lblDisclaimer.TabIndex = 3
		Me.lblDisclaimer.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDisclaimer.BackColor = System.Drawing.SystemColors.Control
		Me.lblDisclaimer.Enabled = True
		Me.lblDisclaimer.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDisclaimer.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDisclaimer.UseMnemonic = True
		Me.lblDisclaimer.Visible = True
		Me.lblDisclaimer.AutoSize = False
		Me.lblDisclaimer.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDisclaimer.Name = "lblDisclaimer"
		Me.Controls.Add(cmdOK)
		Me.Controls.Add(cmdSysInfo)
		Me.Controls.Add(_Image_2)
		Me.Controls.Add(_Image_1)
		Me.Controls.Add(_Image_0)
		Me.Controls.Add(lblCopyLight)
		Me.ShapeContainer1.Shapes.Add(_Line1_1)
		Me.Controls.Add(lblDescription)
		Me.Controls.Add(lblTitle)
		Me.ShapeContainer1.Shapes.Add(_Line1_0)
		Me.Controls.Add(lblVersion)
		Me.Controls.Add(lblDisclaimer)
		Me.Controls.Add(ShapeContainer1)
		Me.Image.SetIndex(_Image_2, CType(2, Short))
		Me.Image.SetIndex(_Image_1, CType(1, Short))
		Me.Image.SetIndex(_Image_0, CType(0, Short))
		Me.Line1.SetIndex(_Line1_1, CType(1, Short))
		Me.Line1.SetIndex(_Line1_0, CType(0, Short))
		CType(Me.Line1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()
	End Sub
#End Region 
End Class