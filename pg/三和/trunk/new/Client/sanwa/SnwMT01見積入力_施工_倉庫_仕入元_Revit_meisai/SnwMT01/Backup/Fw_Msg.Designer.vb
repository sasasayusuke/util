<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class Fw_Msg
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
	Public WithEvents cbAbort As System.Windows.Forms.Button
	Public WithEvents ProgBar As System.Windows.Forms.ProgressBar
	Public WithEvents lbl_Status As System.Windows.Forms.Label
	Public WithEvents lbl_Wait As System.Windows.Forms.Label
	'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
	'Windows フォーム デザイナを使って変更できます。
	'コード エディタを使用して、変更しないでください。
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(Fw_Msg))
		Me.components = New System.ComponentModel.Container()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
		Me.cbAbort = New System.Windows.Forms.Button
		Me.ProgBar = New System.Windows.Forms.ProgressBar
		Me.lbl_Status = New System.Windows.Forms.Label
		Me.lbl_Wait = New System.Windows.Forms.Label
		Me.SuspendLayout()
		Me.ToolTip1.Active = True
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.ClientSize = New System.Drawing.Size(367, 110)
		Me.Location = New System.Drawing.Point(3, 22)
		Me.Font = New System.Drawing.Font("ＭＳ ゴシック", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.Icon = CType(resources.GetObject("Fw_Msg.Icon"), System.Drawing.Icon)
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
		Me.Name = "Fw_Msg"
		Me.cbAbort.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cbAbort.Text = "中止"
		Me.cbAbort.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.cbAbort.Size = New System.Drawing.Size(83, 24)
		Me.cbAbort.Location = New System.Drawing.Point(262, 80)
		Me.cbAbort.TabIndex = 2
		Me.cbAbort.BackColor = System.Drawing.SystemColors.Control
		Me.cbAbort.CausesValidation = True
		Me.cbAbort.Enabled = True
		Me.cbAbort.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cbAbort.Cursor = System.Windows.Forms.Cursors.Default
		Me.cbAbort.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cbAbort.TabStop = True
		Me.cbAbort.Name = "cbAbort"
		Me.ProgBar.Size = New System.Drawing.Size(335, 12)
		Me.ProgBar.Location = New System.Drawing.Point(17, 61)
		Me.ProgBar.TabIndex = 1
		Me.ProgBar.Cursor = System.Windows.Forms.Cursors.WaitCursor
		Me.ProgBar.Name = "ProgBar"
		Me.lbl_Status.Text = "集計中です。"
		Me.lbl_Status.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.lbl_Status.ForeColor = System.Drawing.Color.Black
		Me.lbl_Status.Size = New System.Drawing.Size(336, 18)
		Me.lbl_Status.Location = New System.Drawing.Point(19, 39)
		Me.lbl_Status.TabIndex = 3
		Me.lbl_Status.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lbl_Status.BackColor = System.Drawing.SystemColors.Control
		Me.lbl_Status.Enabled = True
		Me.lbl_Status.Cursor = System.Windows.Forms.Cursors.Default
		Me.lbl_Status.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lbl_Status.UseMnemonic = True
		Me.lbl_Status.Visible = True
		Me.lbl_Status.AutoSize = False
		Me.lbl_Status.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lbl_Status.Name = "lbl_Status"
		Me.lbl_Wait.Text = "しばらくお待ち下さい。"
		Me.lbl_Wait.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.lbl_Wait.ForeColor = System.Drawing.Color.Black
		Me.lbl_Wait.Size = New System.Drawing.Size(336, 18)
		Me.lbl_Wait.Location = New System.Drawing.Point(19, 16)
		Me.lbl_Wait.TabIndex = 0
		Me.lbl_Wait.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lbl_Wait.BackColor = System.Drawing.SystemColors.Control
		Me.lbl_Wait.Enabled = True
		Me.lbl_Wait.Cursor = System.Windows.Forms.Cursors.Default
		Me.lbl_Wait.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lbl_Wait.UseMnemonic = True
		Me.lbl_Wait.Visible = True
		Me.lbl_Wait.AutoSize = False
		Me.lbl_Wait.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lbl_Wait.Name = "lbl_Wait"
		Me.Controls.Add(cbAbort)
		Me.Controls.Add(ProgBar)
		Me.Controls.Add(lbl_Status)
		Me.Controls.Add(lbl_Wait)
		Me.ResumeLayout(False)
		Me.PerformLayout()
	End Sub
#End Region 
End Class