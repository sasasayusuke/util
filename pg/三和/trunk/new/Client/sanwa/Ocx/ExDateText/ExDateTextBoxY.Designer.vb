<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ExDateTextBoxY
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
        Me.ExDateTextY = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'ExDateTextY
        '
        Me.ExDateTextY.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.ExDateTextY.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!)
        Me.ExDateTextY.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.ExDateTextY.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.ExDateTextY.Location = New System.Drawing.Point(4, 8)
        Me.ExDateTextY.MaxLength = 4
        Me.ExDateTextY.Multiline = True
        Me.ExDateTextY.Name = "ExDateTextY"
        Me.ExDateTextY.Size = New System.Drawing.Size(42, 18)
        Me.ExDateTextY.TabIndex = 0
        Me.ExDateTextY.Text = "YYYY"
        Me.ExDateTextY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ExDateTextBoxY
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.ExDateTextY)
        Me.Name = "ExDateTextBoxY"
        Me.Size = New System.Drawing.Size(45, 34)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ExDateTextY As TextBox
    Friend WithEvents ToolTip1 As ToolTip
End Class