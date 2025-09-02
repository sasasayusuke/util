<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form2
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
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
        Me.tx_s見積日Y = New ExDateText.ExDateTextBoxY()
        Me.tx_s見積日M = New ExDateText.ExDateTextBoxM()
        Me.tx_s見積日D = New ExDateText.ExDateTextBoxD()
        Me.tx_e見積日Y = New ExDateText.ExDateTextBoxY()
        Me.tx_e見積日M = New ExDateText.ExDateTextBoxM()
        Me.tx_e見積日D = New ExDateText.ExDateTextBoxD()
        Me.lb_e日 = New System.Windows.Forms.Label()
        Me.lb_e月 = New System.Windows.Forms.Label()
        Me.lb_e年 = New System.Windows.Forms.Label()
        Me.lb_s日 = New System.Windows.Forms.Label()
        Me.lb_s月 = New System.Windows.Forms.Label()
        Me.lb_s年 = New System.Windows.Forms.Label()
        Me._lb_見積日 = New System.Windows.Forms.Label()
        Me._lb_s見積日 = New System.Windows.Forms.Label()
        Me._lb_見積日_kara = New System.Windows.Forms.Label()
        Me._lb_e見積日 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'tx_s見積日Y
        '
        Me.tx_s見積日Y.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_s見積日Y.BackColor = System.Drawing.SystemColors.Window
        Me.tx_s見積日Y.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.実線
        Me.tx_s見積日Y.CanForwardSetFocus = True
        Me.tx_s見積日Y.CanNextSetFocus = True
        Me.tx_s見積日Y.EditMode = True
        Me.tx_s見積日Y.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_s見積日Y.ForeColor = System.Drawing.SystemColors.WindowText
        Me.tx_s見積日Y.Location = New System.Drawing.Point(241, 178)
        Me.tx_s見積日Y.MaxLength = 4
        Me.tx_s見積日Y.Name = "tx_s見積日Y"
        Me.tx_s見積日Y.OldValue = "2024"
        Me.tx_s見積日Y.SelectText = True
        Me.tx_s見積日Y.SelLength = 0
        Me.tx_s見積日Y.SelStart = 0
        Me.tx_s見積日Y.SelText = ""
        Me.tx_s見積日Y.Size = New System.Drawing.Size(42, 22)
        Me.tx_s見積日Y.TabIndex = 90
        '
        'tx_s見積日M
        '
        Me.tx_s見積日M.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_s見積日M.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.実線
        Me.tx_s見積日M.CanForwardSetFocus = True
        Me.tx_s見積日M.CanNextSetFocus = True
        Me.tx_s見積日M.EditMode = True
        Me.tx_s見積日M.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_s見積日M.Location = New System.Drawing.Point(302, 178)
        Me.tx_s見積日M.MaxLength = 2
        Me.tx_s見積日M.Name = "tx_s見積日M"
        Me.tx_s見積日M.OldValue = "12"
        Me.tx_s見積日M.SelectText = True
        Me.tx_s見積日M.SelLength = 0
        Me.tx_s見積日M.SelStart = 0
        Me.tx_s見積日M.SelText = ""
        Me.tx_s見積日M.Size = New System.Drawing.Size(26, 22)
        Me.tx_s見積日M.TabIndex = 91
        '
        'tx_s見積日D
        '
        Me.tx_s見積日D.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_s見積日D.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.実線
        Me.tx_s見積日D.CanForwardSetFocus = True
        Me.tx_s見積日D.CanNextSetFocus = True
        Me.tx_s見積日D.EditMode = True
        Me.tx_s見積日D.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_s見積日D.Location = New System.Drawing.Point(347, 178)
        Me.tx_s見積日D.MaxLength = 2
        Me.tx_s見積日D.Name = "tx_s見積日D"
        Me.tx_s見積日D.OldValue = "31"
        Me.tx_s見積日D.SelectText = True
        Me.tx_s見積日D.SelLength = 0
        Me.tx_s見積日D.SelStart = 0
        Me.tx_s見積日D.SelText = ""
        Me.tx_s見積日D.Size = New System.Drawing.Size(27, 22)
        Me.tx_s見積日D.TabIndex = 92
        '
        'tx_e見積日Y
        '
        Me.tx_e見積日Y.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_e見積日Y.BackColor = System.Drawing.SystemColors.Window
        Me.tx_e見積日Y.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.実線
        Me.tx_e見積日Y.CanForwardSetFocus = True
        Me.tx_e見積日Y.CanNextSetFocus = True
        Me.tx_e見積日Y.EditMode = True
        Me.tx_e見積日Y.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_e見積日Y.ForeColor = System.Drawing.SystemColors.WindowText
        Me.tx_e見積日Y.Location = New System.Drawing.Point(450, 178)
        Me.tx_e見積日Y.MaxLength = 4
        Me.tx_e見積日Y.Name = "tx_e見積日Y"
        Me.tx_e見積日Y.OldValue = ""
        Me.tx_e見積日Y.SelectText = True
        Me.tx_e見積日Y.SelLength = 0
        Me.tx_e見積日Y.SelStart = 0
        Me.tx_e見積日Y.SelText = ""
        Me.tx_e見積日Y.Size = New System.Drawing.Size(42, 22)
        Me.tx_e見積日Y.TabIndex = 93
        '
        'tx_e見積日M
        '
        Me.tx_e見積日M.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_e見積日M.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.実線
        Me.tx_e見積日M.CanForwardSetFocus = True
        Me.tx_e見積日M.CanNextSetFocus = True
        Me.tx_e見積日M.EditMode = True
        Me.tx_e見積日M.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_e見積日M.Location = New System.Drawing.Point(512, 178)
        Me.tx_e見積日M.MaxLength = 2
        Me.tx_e見積日M.Name = "tx_e見積日M"
        Me.tx_e見積日M.OldValue = ""
        Me.tx_e見積日M.SelectText = True
        Me.tx_e見積日M.SelLength = 0
        Me.tx_e見積日M.SelStart = 0
        Me.tx_e見積日M.SelText = ""
        Me.tx_e見積日M.Size = New System.Drawing.Size(28, 22)
        Me.tx_e見積日M.TabIndex = 94
        '
        'tx_e見積日D
        '
        Me.tx_e見積日D.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_e見積日D.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.実線
        Me.tx_e見積日D.CanForwardSetFocus = True
        Me.tx_e見積日D.CanNextSetFocus = True
        Me.tx_e見積日D.EditMode = True
        Me.tx_e見積日D.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_e見積日D.Location = New System.Drawing.Point(566, 178)
        Me.tx_e見積日D.MaxLength = 2
        Me.tx_e見積日D.Name = "tx_e見積日D"
        Me.tx_e見積日D.OldValue = ""
        Me.tx_e見積日D.SelectText = True
        Me.tx_e見積日D.SelLength = 0
        Me.tx_e見積日D.SelStart = 0
        Me.tx_e見積日D.SelText = ""
        Me.tx_e見積日D.Size = New System.Drawing.Size(28, 22)
        Me.tx_e見積日D.TabIndex = 95
        '
        'lb_e日
        '
        Me.lb_e日.BackColor = System.Drawing.Color.Transparent
        Me.lb_e日.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_e日.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_e日.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_e日.Location = New System.Drawing.Point(604, 180)
        Me.lb_e日.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lb_e日.Name = "lb_e日"
        Me.lb_e日.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_e日.Size = New System.Drawing.Size(16, 14)
        Me.lb_e日.TabIndex = 103
        Me.lb_e日.Text = "日"
        Me.lb_e日.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_e月
        '
        Me.lb_e月.BackColor = System.Drawing.Color.Transparent
        Me.lb_e月.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_e月.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_e月.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_e月.Location = New System.Drawing.Point(548, 181)
        Me.lb_e月.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lb_e月.Name = "lb_e月"
        Me.lb_e月.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_e月.Size = New System.Drawing.Size(16, 14)
        Me.lb_e月.TabIndex = 102
        Me.lb_e月.Text = "月"
        Me.lb_e月.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_e年
        '
        Me.lb_e年.BackColor = System.Drawing.Color.Transparent
        Me.lb_e年.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_e年.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_e年.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_e年.Location = New System.Drawing.Point(497, 181)
        Me.lb_e年.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lb_e年.Name = "lb_e年"
        Me.lb_e年.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_e年.Size = New System.Drawing.Size(16, 14)
        Me.lb_e年.TabIndex = 101
        Me.lb_e年.Text = "年"
        Me.lb_e年.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_s日
        '
        Me.lb_s日.BackColor = System.Drawing.Color.Transparent
        Me.lb_s日.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_s日.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_s日.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_s日.Location = New System.Drawing.Point(385, 180)
        Me.lb_s日.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lb_s日.Name = "lb_s日"
        Me.lb_s日.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_s日.Size = New System.Drawing.Size(16, 14)
        Me.lb_s日.TabIndex = 100
        Me.lb_s日.Text = "日"
        Me.lb_s日.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_s月
        '
        Me.lb_s月.BackColor = System.Drawing.Color.Transparent
        Me.lb_s月.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_s月.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_s月.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_s月.Location = New System.Drawing.Point(334, 180)
        Me.lb_s月.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lb_s月.Name = "lb_s月"
        Me.lb_s月.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_s月.Size = New System.Drawing.Size(16, 14)
        Me.lb_s月.TabIndex = 99
        Me.lb_s月.Text = "月"
        Me.lb_s月.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_s年
        '
        Me.lb_s年.BackColor = System.Drawing.Color.Transparent
        Me.lb_s年.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_s年.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_s年.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_s年.Location = New System.Drawing.Point(290, 179)
        Me.lb_s年.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lb_s年.Name = "lb_s年"
        Me.lb_s年.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_s年.Size = New System.Drawing.Size(16, 14)
        Me.lb_s年.TabIndex = 98
        Me.lb_s年.Text = "年"
        Me.lb_s年.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_見積日
        '
        Me._lb_見積日.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_見積日.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_見積日.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_見積日.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_見積日.ForeColor = System.Drawing.Color.White
        Me._lb_見積日.Location = New System.Drawing.Point(147, 178)
        Me._lb_見積日.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_見積日.Name = "_lb_見積日"
        Me._lb_見積日.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_見積日.Size = New System.Drawing.Size(91, 19)
        Me._lb_見積日.TabIndex = 104
        Me._lb_見積日.Text = "見積日"
        Me._lb_見積日.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_s見積日
        '
        Me._lb_s見積日.BackColor = System.Drawing.SystemColors.Window
        Me._lb_s見積日.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_s見積日.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_s見積日.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_s見積日.Location = New System.Drawing.Point(240, 178)
        Me._lb_s見積日.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_s見積日.Name = "_lb_s見積日"
        Me._lb_s見積日.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_s見積日.Size = New System.Drawing.Size(158, 20)
        Me._lb_s見積日.TabIndex = 97
        '
        '_lb_見積日_kara
        '
        Me._lb_見積日_kara.BackColor = System.Drawing.SystemColors.Control
        Me._lb_見積日_kara.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_見積日_kara.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_見積日_kara.Location = New System.Drawing.Point(395, 181)
        Me._lb_見積日_kara.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_見積日_kara.Name = "_lb_見積日_kara"
        Me._lb_見積日_kara.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_見積日_kara.Size = New System.Drawing.Size(46, 18)
        Me._lb_見積日_kara.TabIndex = 96
        Me._lb_見積日_kara.Text = "～"
        Me._lb_見積日_kara.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_e見積日
        '
        Me._lb_e見積日.BackColor = System.Drawing.SystemColors.Window
        Me._lb_e見積日.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_e見積日.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_e見積日.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_e見積日.Location = New System.Drawing.Point(450, 178)
        Me._lb_e見積日.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_e見積日.Name = "_lb_e見積日"
        Me._lb_e見積日.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_e見積日.Size = New System.Drawing.Size(169, 20)
        Me._lb_e見積日.TabIndex = 105
        '
        'Form2
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.tx_s見積日Y)
        Me.Controls.Add(Me.tx_s見積日M)
        Me.Controls.Add(Me.tx_s見積日D)
        Me.Controls.Add(Me.tx_e見積日Y)
        Me.Controls.Add(Me.tx_e見積日M)
        Me.Controls.Add(Me.tx_e見積日D)
        Me.Controls.Add(Me._lb_見積日)
        Me.Controls.Add(Me.lb_e日)
        Me.Controls.Add(Me.lb_e月)
        Me.Controls.Add(Me.lb_e年)
        Me.Controls.Add(Me.lb_s日)
        Me.Controls.Add(Me.lb_s月)
        Me.Controls.Add(Me.lb_s年)
        Me.Controls.Add(Me._lb_s見積日)
        Me.Controls.Add(Me._lb_見積日_kara)
        Me.Controls.Add(Me._lb_e見積日)
        Me.Name = "Form2"
        Me.Text = "Form2"
        Me.ResumeLayout(False)

    End Sub

    Public WithEvents tx_s見積日Y As ExDateText.ExDateTextBoxY
    Public WithEvents tx_s見積日M As ExDateText.ExDateTextBoxM
    Public WithEvents tx_s見積日D As ExDateText.ExDateTextBoxD
    Public WithEvents tx_e見積日Y As ExDateText.ExDateTextBoxY
    Public WithEvents tx_e見積日M As ExDateText.ExDateTextBoxM
    Public WithEvents tx_e見積日D As ExDateText.ExDateTextBoxD
    Public WithEvents _lb_見積日 As Label
    Public WithEvents lb_e日 As Label
    Public WithEvents lb_e月 As Label
    Public WithEvents lb_e年 As Label
    Public WithEvents lb_s日 As Label
    Public WithEvents lb_s月 As Label
    Public WithEvents lb_s年 As Label
    Public WithEvents _lb_s見積日 As Label
    Public WithEvents _lb_見積日_kara As Label
    Public WithEvents _lb_e見積日 As Label
End Class
