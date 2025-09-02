<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class SnwMT02F08
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
	Public WithEvents cdOK As System.Windows.Forms.Button
	Public WithEvents rf_担当者名 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_8 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_7 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_6 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_5 As System.Windows.Forms.Label
	Public WithEvents rf_製品名 As System.Windows.Forms.Label
	Public WithEvents rf_仕様NO As System.Windows.Forms.Label
	Public WithEvents rf_製品NO As System.Windows.Forms.Label
	Public WithEvents lb_項目 As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
	'Windows フォーム デザイナを使って変更できます。
	'コード エディタを使用して、変更しないでください。
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(SnwMT02F08))
		Me.components = New System.ComponentModel.Container()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
		Me.cdOK = New System.Windows.Forms.Button
		Me.rf_担当者名 = New System.Windows.Forms.Label
		Me._lb_項目_8 = New System.Windows.Forms.Label
		Me._lb_項目_7 = New System.Windows.Forms.Label
		Me._lb_項目_6 = New System.Windows.Forms.Label
		Me._lb_項目_5 = New System.Windows.Forms.Label
		Me.rf_製品名 = New System.Windows.Forms.Label
		Me.rf_仕様NO = New System.Windows.Forms.Label
		Me.rf_製品NO = New System.Windows.Forms.Label
		Me.lb_項目 = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(components)
		Me.SuspendLayout()
		Me.ToolTip1.Active = True
		CType(Me.lb_項目, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.Text = "社内在庫履歴表示"
		Me.ClientSize = New System.Drawing.Size(926, 397)
		Me.Location = New System.Drawing.Point(184, 22)
		Me.Icon = CType(resources.GetObject("SnwMT02F08.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ControlBox = True
		Me.Enabled = True
		Me.KeyPreview = False
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.HelpButton = False
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.Name = "SnwMT02F08"
		Me.cdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cdOK.Text = "OK"
		Me.AcceptButton = Me.cdOK
		Me.cdOK.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.cdOK.Size = New System.Drawing.Size(74, 22)
		Me.cdOK.Location = New System.Drawing.Point(832, 368)
		Me.cdOK.TabIndex = 0
		Me.cdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cdOK.CausesValidation = True
		Me.cdOK.Enabled = True
		Me.cdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cdOK.TabStop = True
		Me.cdOK.Name = "cdOK"
		Me.rf_担当者名.Text = "wwwwwww"
		Me.rf_担当者名.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.rf_担当者名.Size = New System.Drawing.Size(111, 19)
		Me.rf_担当者名.Location = New System.Drawing.Point(104, 8)
		Me.rf_担当者名.TabIndex = 8
		Me.rf_担当者名.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.rf_担当者名.BackColor = System.Drawing.SystemColors.Control
		Me.rf_担当者名.Enabled = True
		Me.rf_担当者名.ForeColor = System.Drawing.SystemColors.ControlText
		Me.rf_担当者名.Cursor = System.Windows.Forms.Cursors.Default
		Me.rf_担当者名.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.rf_担当者名.UseMnemonic = True
		Me.rf_担当者名.Visible = True
		Me.rf_担当者名.AutoSize = False
		Me.rf_担当者名.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.rf_担当者名.Name = "rf_担当者名"
		Me._lb_項目_8.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_項目_8.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me._lb_項目_8.Text = "製品名"
		Me._lb_項目_8.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_8.ForeColor = System.Drawing.Color.White
		Me._lb_項目_8.Size = New System.Drawing.Size(89, 19)
		Me._lb_項目_8.Location = New System.Drawing.Point(8, 65)
		Me._lb_項目_8.TabIndex = 7
		Me._lb_項目_8.Enabled = True
		Me._lb_項目_8.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_8.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_8.UseMnemonic = True
		Me._lb_項目_8.Visible = True
		Me._lb_項目_8.AutoSize = False
		Me._lb_項目_8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._lb_項目_8.Name = "_lb_項目_8"
		Me._lb_項目_7.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_項目_7.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me._lb_項目_7.Text = "仕様№"
		Me._lb_項目_7.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_7.ForeColor = System.Drawing.Color.White
		Me._lb_項目_7.Size = New System.Drawing.Size(89, 19)
		Me._lb_項目_7.Location = New System.Drawing.Point(8, 46)
		Me._lb_項目_7.TabIndex = 6
		Me._lb_項目_7.Enabled = True
		Me._lb_項目_7.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_7.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_7.UseMnemonic = True
		Me._lb_項目_7.Visible = True
		Me._lb_項目_7.AutoSize = False
		Me._lb_項目_7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._lb_項目_7.Name = "_lb_項目_7"
		Me._lb_項目_6.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_項目_6.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me._lb_項目_6.Text = "製品№"
		Me._lb_項目_6.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_6.ForeColor = System.Drawing.Color.White
		Me._lb_項目_6.Size = New System.Drawing.Size(89, 19)
		Me._lb_項目_6.Location = New System.Drawing.Point(8, 27)
		Me._lb_項目_6.TabIndex = 5
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
		Me._lb_項目_5.Text = "担当者"
		Me._lb_項目_5.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_5.ForeColor = System.Drawing.Color.White
		Me._lb_項目_5.Size = New System.Drawing.Size(89, 19)
		Me._lb_項目_5.Location = New System.Drawing.Point(8, 8)
		Me._lb_項目_5.TabIndex = 4
		Me._lb_項目_5.Enabled = True
		Me._lb_項目_5.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_5.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_5.UseMnemonic = True
		Me._lb_項目_5.Visible = True
		Me._lb_項目_5.AutoSize = False
		Me._lb_項目_5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._lb_項目_5.Name = "_lb_項目_5"
		Me.rf_製品名.Text = "ＷＷＷＷＷＷＷＷＷＷＷＷＷＷＷＷＷＷＷＷ"
		Me.rf_製品名.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.rf_製品名.Size = New System.Drawing.Size(307, 19)
		Me.rf_製品名.Location = New System.Drawing.Point(104, 65)
		Me.rf_製品名.TabIndex = 3
		Me.rf_製品名.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.rf_製品名.BackColor = System.Drawing.SystemColors.Control
		Me.rf_製品名.Enabled = True
		Me.rf_製品名.ForeColor = System.Drawing.SystemColors.ControlText
		Me.rf_製品名.Cursor = System.Windows.Forms.Cursors.Default
		Me.rf_製品名.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.rf_製品名.UseMnemonic = True
		Me.rf_製品名.Visible = True
		Me.rf_製品名.AutoSize = False
		Me.rf_製品名.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.rf_製品名.Name = "rf_製品名"
		Me.rf_仕様NO.Text = "wwwwwww"
		Me.rf_仕様NO.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.rf_仕様NO.Size = New System.Drawing.Size(67, 19)
		Me.rf_仕様NO.Location = New System.Drawing.Point(104, 46)
		Me.rf_仕様NO.TabIndex = 2
		Me.rf_仕様NO.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.rf_仕様NO.BackColor = System.Drawing.SystemColors.Control
		Me.rf_仕様NO.Enabled = True
		Me.rf_仕様NO.ForeColor = System.Drawing.SystemColors.ControlText
		Me.rf_仕様NO.Cursor = System.Windows.Forms.Cursors.Default
		Me.rf_仕様NO.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.rf_仕様NO.UseMnemonic = True
		Me.rf_仕様NO.Visible = True
		Me.rf_仕様NO.AutoSize = False
		Me.rf_仕様NO.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.rf_仕様NO.Name = "rf_仕様NO"
		Me.rf_製品NO.Text = "wwwwwww"
		Me.rf_製品NO.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.rf_製品NO.Size = New System.Drawing.Size(67, 19)
		Me.rf_製品NO.Location = New System.Drawing.Point(104, 27)
		Me.rf_製品NO.TabIndex = 1
		Me.rf_製品NO.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.rf_製品NO.BackColor = System.Drawing.SystemColors.Control
		Me.rf_製品NO.Enabled = True
		Me.rf_製品NO.ForeColor = System.Drawing.SystemColors.ControlText
		Me.rf_製品NO.Cursor = System.Windows.Forms.Cursors.Default
		Me.rf_製品NO.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.rf_製品NO.UseMnemonic = True
		Me.rf_製品NO.Visible = True
		Me.rf_製品NO.AutoSize = False
		Me.rf_製品NO.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.rf_製品NO.Name = "rf_製品NO"
		Me.Controls.Add(cdOK)
		Me.Controls.Add(rf_担当者名)
		Me.Controls.Add(_lb_項目_8)
		Me.Controls.Add(_lb_項目_7)
		Me.Controls.Add(_lb_項目_6)
		Me.Controls.Add(_lb_項目_5)
		Me.Controls.Add(rf_製品名)
		Me.Controls.Add(rf_仕様NO)
		Me.Controls.Add(rf_製品NO)
		Me.lb_項目.SetIndex(_lb_項目_8, CType(8, Short))
		Me.lb_項目.SetIndex(_lb_項目_7, CType(7, Short))
		Me.lb_項目.SetIndex(_lb_項目_6, CType(6, Short))
		Me.lb_項目.SetIndex(_lb_項目_5, CType(5, Short))
		CType(Me.lb_項目, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()
	End Sub
#End Region 
End Class