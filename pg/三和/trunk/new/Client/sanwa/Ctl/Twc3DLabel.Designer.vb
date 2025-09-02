<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Twc3DLabel
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
        Me.SuspendLayout()

        ' LabelA
        Me.LabelA.BackColor = Color.Transparent
        Me.LabelA.Text = "Label1"
        Me.LabelA.Location = New Point(555, 150)
        Me.LabelA.Size = New Size(2085, 360)
        Me.LabelA.TabIndex = 0

        ' LineTop
        Me.LineTop.BorderStyle = BorderStyle.FixedSingle
        Me.LineTop.BackColor = Color.White
        Me.LineTop.Location = New Point(60, 60)
        Me.LineTop.Size = New Size(360, 1)

        ' LineLeft
        Me.LineLeft.BorderStyle = BorderStyle.FixedSingle
        Me.LineLeft.BackColor = Color.White
        Me.LineLeft.Location = New Point(60, 60)
        Me.LineLeft.Size = New Size(1, 480)

        ' ShapeA
        Me.ShapeA.BackColor = Color.Gray
        Me.ShapeA.Location = New Point(60, 60)
        Me.ShapeA.Size = New Size(375, 495)
        Me.ShapeA.BorderStyle = BorderStyle.FixedSingle

        ' Twc3DLabel
        Me.Controls.Add(Me.LabelA)
        Me.Controls.Add(Me.LineTop)
        Me.Controls.Add(Me.LineLeft)
        Me.Controls.Add(Me.ShapeA)
        Me.Size = New Size(4035, 3045)
        Me.ResumeLayout(False)

        '
        'Twc3DLabel
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Name = "Twc3DLabel"
        Me.Size = New System.Drawing.Size(801, 464)
        Me.ResumeLayout(False)

    End Sub

End Class
