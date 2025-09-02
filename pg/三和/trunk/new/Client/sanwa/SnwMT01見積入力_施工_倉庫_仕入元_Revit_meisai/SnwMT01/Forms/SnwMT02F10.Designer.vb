<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SnwMT02F10

	'Form は、コンポーネント一覧に後処理を実行するために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

	'Windows フォーム デザイナで必要です。
	Private components As System.ComponentModel.IContainer

	'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
	'Windows フォーム デザイナを使って変更できます。
	'コード エディタを使用して、変更しないでください。
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SnwMT02F10))
        Me.ChkFunction = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.CbOK = New System.Windows.Forms.Button()
        Me.CbCANCEL = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'ChkFunction
        '
        Me.ChkFunction.Location = New System.Drawing.Point(20, 20)
        Me.ChkFunction.Name = "ChkFunction"
        Me.ChkFunction.Size = New System.Drawing.Size(510, 404)
        Me.ChkFunction.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(51, 444)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(232, 15)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "※ 表示する項目をチェックして下さい。"
        '
        'CbOK
        '
        Me.CbOK.Location = New System.Drawing.Point(319, 463)
        Me.CbOK.Name = "CbOK"
        Me.CbOK.Size = New System.Drawing.Size(91, 27)
        Me.CbOK.TabIndex = 2
        Me.CbOK.Text = "OK"
        Me.CbOK.UseVisualStyleBackColor = True
        '
        'CbCANCEL
        '
        Me.CbCANCEL.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CbCANCEL.Location = New System.Drawing.Point(417, 463)
        Me.CbCANCEL.Name = "CbCANCEL"
        Me.CbCANCEL.Size = New System.Drawing.Size(91, 27)
        Me.CbCANCEL.TabIndex = 3
        Me.CbCANCEL.Text = "ｷｬﾝｾﾙ"
        Me.CbCANCEL.UseVisualStyleBackColor = True
        '
        'SnwMT02F10
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.CbCANCEL
        Me.ClientSize = New System.Drawing.Size(542, 510)
        Me.Controls.Add(Me.CbCANCEL)
        Me.Controls.Add(Me.CbOK)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ChkFunction)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SnwMT02F10"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "表示項目設定"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ChkFunction As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents CbOK As Button
    Friend WithEvents CbCANCEL As Button
End Class