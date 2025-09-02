<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Fw_Msg
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
   
	'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
	'Windows フォーム デザイナを使って変更できます。
	'コード エディタを使用して、変更しないでください。
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Fw_Msg))
        Me.CbAbort = New System.Windows.Forms.Button()
        Me.ProgBar = New System.Windows.Forms.ProgressBar()
        Me.lbl_Status = New System.Windows.Forms.Label()
        Me.lbl_Wait = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'CbAbort
        '
        Me.CbAbort.BackColor = System.Drawing.SystemColors.Control
        Me.CbAbort.Cursor = System.Windows.Forms.Cursors.Default
        Me.CbAbort.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CbAbort.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CbAbort.Location = New System.Drawing.Point(262, 80)
        Me.CbAbort.Name = "CbAbort"
        Me.CbAbort.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CbAbort.Size = New System.Drawing.Size(83, 24)
        Me.CbAbort.TabIndex = 2
        Me.CbAbort.Text = "中止"
        Me.CbAbort.UseVisualStyleBackColor = False
        '
        'ProgBar
        '
        Me.ProgBar.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Me.ProgBar.Location = New System.Drawing.Point(17, 61)
        Me.ProgBar.Name = "ProgBar"
        Me.ProgBar.Size = New System.Drawing.Size(335, 12)
        Me.ProgBar.TabIndex = 1
        '
        'lbl_Status
        '
        Me.lbl_Status.BackColor = System.Drawing.SystemColors.Control
        Me.lbl_Status.Cursor = System.Windows.Forms.Cursors.Default
        Me.lbl_Status.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lbl_Status.ForeColor = System.Drawing.Color.Black
        Me.lbl_Status.Location = New System.Drawing.Point(19, 39)
        Me.lbl_Status.Name = "lbl_Status"
        Me.lbl_Status.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lbl_Status.Size = New System.Drawing.Size(336, 18)
        Me.lbl_Status.TabIndex = 3
        Me.lbl_Status.Text = "集計中です。"
        '
        'lbl_Wait
        '
        Me.lbl_Wait.BackColor = System.Drawing.SystemColors.Control
        Me.lbl_Wait.Cursor = System.Windows.Forms.Cursors.Default
        Me.lbl_Wait.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lbl_Wait.ForeColor = System.Drawing.Color.Black
        Me.lbl_Wait.Location = New System.Drawing.Point(19, 16)
        Me.lbl_Wait.Name = "lbl_Wait"
        Me.lbl_Wait.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lbl_Wait.Size = New System.Drawing.Size(336, 18)
        Me.lbl_Wait.TabIndex = 0
        Me.lbl_Wait.Text = "しばらくお待ち下さい。"
        '
        'Fw_Msg
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(10.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(367, 110)
        Me.Controls.Add(Me.CbAbort)
        Me.Controls.Add(Me.ProgBar)
        Me.Controls.Add(Me.lbl_Status)
        Me.Controls.Add(Me.lbl_Wait)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Fw_Msg"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.ResumeLayout(False)

    End Sub
    Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents CbAbort As System.Windows.Forms.Button
	Public WithEvents ProgBar As System.Windows.Forms.ProgressBar
	Public WithEvents lbl_Status As System.Windows.Forms.Label
	Public WithEvents lbl_Wait As System.Windows.Forms.Label

End Class