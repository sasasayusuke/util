<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ExTextBox
    Inherits System.Windows.Forms.UserControl

    'UserControl はコンポーネント一覧をクリーンアップするために dispose をオーバーライドします。
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

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.ExText = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'ExText
        '
        Me.ExText.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        Me.ExText.BackColor = System.Drawing.SystemColors.Window
        Me.ExText.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.ExText.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ExText.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.ExText.Location = New System.Drawing.Point(8, 8)
        Me.ExText.MaxLength = 0
        Me.ExText.Name = "ExText"
        Me.ExText.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ExText.Size = New System.Drawing.Size(185, 22)
        Me.ExText.TabIndex = 0
        '
        'ExTextBox
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.ExText)
        Me.Name = "ExTextBox"
        Me.Size = New System.Drawing.Size(201, 35)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ExText As TextBox
    Friend WithEvents ToolTip1 As ToolTip
End Class