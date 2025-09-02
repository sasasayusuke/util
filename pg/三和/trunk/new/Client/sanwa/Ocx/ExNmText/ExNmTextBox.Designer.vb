<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ExNmTextBox
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
        Me.ExNmText = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'ExNmText
        '
        Me.ExNmText.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ExNmText.Size = New System.Drawing.Size(185, 22)
		Me.ExNmText.Location = New System.Drawing.Point(8, 6)
		Me.ExNmText.TabIndex = 0
        Me.ExNmText.Name = "ExNmText"
        '
        'ExNmTextBox
        '
        Me.Size = New System.Drawing.Size(201, 35)
		Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.Name = "ExNmTextBox"
		Me.Controls.Add(Me.ExNmText)
		Me.ResumeLayout(False)
		Me.PerformLayout()

    End Sub

    Friend WithEvents ExNmText As TextBox
    Friend WithEvents ToolTip1 As ToolTip
End Class