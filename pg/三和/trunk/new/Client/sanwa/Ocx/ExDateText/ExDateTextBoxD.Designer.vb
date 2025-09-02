<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ExDateTextBoxD
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
        Me.ExDateTextD = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'ExDateTextD
        '
        Me.ExDateTextD.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.ExDateTextD.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!)
        Me.ExDateTextD.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.ExDateTextD.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.ExDateTextD.Location = New System.Drawing.Point(6, 8)
        Me.ExDateTextD.MaxLength = 2
        Me.ExDateTextD.Multiline = True
        Me.ExDateTextD.Name = "ExDateTextD"
        Me.ExDateTextD.Size = New System.Drawing.Size(27, 18)
        Me.ExDateTextD.TabIndex = 0
        Me.ExDateTextD.Text = "DD"
        Me.ExDateTextD.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ExDateTextBoxD
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.ExDateTextD)
        Me.Name = "ExDateTextBoxD"
        Me.Size = New System.Drawing.Size(45, 34)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ExDateTextD As TextBox
    Friend WithEvents ToolTip1 As ToolTip
End Class