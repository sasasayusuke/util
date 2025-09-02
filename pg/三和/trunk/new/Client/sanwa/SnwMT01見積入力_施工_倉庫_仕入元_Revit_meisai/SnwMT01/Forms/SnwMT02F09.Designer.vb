<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SnwMT02F09

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

	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents CdOK As System.Windows.Forms.Button
	Public WithEvents rf_得意先名1 As System.Windows.Forms.Label
	Public WithEvents rf_得意先名2 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_0 As System.Windows.Forms.Label
	Public WithEvents rf_担当者名 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_8 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_7 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_6 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_5 As System.Windows.Forms.Label
	Public WithEvents rf_製品名 As System.Windows.Forms.Label
	Public WithEvents rf_仕様NO As System.Windows.Forms.Label
	Public WithEvents rf_製品NO As System.Windows.Forms.Label
    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使って変更できます。
    'コード エディタを使用して、変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SnwMT02F09))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.CdOK = New System.Windows.Forms.Button()
        Me.rf_得意先名1 = New System.Windows.Forms.Label()
        Me.rf_得意先名2 = New System.Windows.Forms.Label()
        Me._lb_項目_0 = New System.Windows.Forms.Label()
        Me.rf_担当者名 = New System.Windows.Forms.Label()
        Me._lb_項目_8 = New System.Windows.Forms.Label()
        Me._lb_項目_7 = New System.Windows.Forms.Label()
        Me._lb_項目_6 = New System.Windows.Forms.Label()
        Me._lb_項目_5 = New System.Windows.Forms.Label()
        Me.rf_製品名 = New System.Windows.Forms.Label()
        Me.rf_仕様NO = New System.Windows.Forms.Label()
        Me.rf_製品NO = New System.Windows.Forms.Label()
        Me.FpSpd = New FarPoint.Win.Spread.FpSpread(FarPoint.Win.Spread.LegacyBehaviors.None, resources.GetObject("resource1"))
        CType(Me.FpSpd, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'CdOK
        '
        Me.CdOK.BackColor = System.Drawing.SystemColors.Control
        Me.CdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.CdOK.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CdOK.Location = New System.Drawing.Point(640, 396)
        Me.CdOK.Name = "CdOK"
        Me.CdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CdOK.Size = New System.Drawing.Size(74, 22)
        Me.CdOK.TabIndex = 0
        Me.CdOK.Text = "OK"
        Me.CdOK.UseVisualStyleBackColor = False
        '
        'rf_得意先名1
        '
        Me.rf_得意先名1.BackColor = System.Drawing.SystemColors.Control
        Me.rf_得意先名1.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_得意先名1.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_得意先名1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_得意先名1.Location = New System.Drawing.Point(104, 27)
        Me.rf_得意先名1.Name = "rf_得意先名1"
        Me.rf_得意先名1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_得意先名1.Size = New System.Drawing.Size(307, 19)
        Me.rf_得意先名1.TabIndex = 12
        Me.rf_得意先名1.Text = "ＷＷＷＷＷＷＷＷＷＷＷＷＷＷＷＷＷＷＷＷ"
        '
        'rf_得意先名2
        '
        Me.rf_得意先名2.BackColor = System.Drawing.SystemColors.Control
        Me.rf_得意先名2.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_得意先名2.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_得意先名2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_得意先名2.Location = New System.Drawing.Point(411, 27)
        Me.rf_得意先名2.Name = "rf_得意先名2"
        Me.rf_得意先名2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_得意先名2.Size = New System.Drawing.Size(307, 19)
        Me.rf_得意先名2.TabIndex = 11
        Me.rf_得意先名2.Text = "ＷＷＷＷＷＷＷＷＷＷＷＷＷＷＷＷＷＷＷＷ"
        '
        '_lb_項目_0
        '
        Me._lb_項目_0.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_0.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_0.ForeColor = System.Drawing.Color.White
        Me._lb_項目_0.Location = New System.Drawing.Point(8, 27)
        Me._lb_項目_0.Name = "_lb_項目_0"
        Me._lb_項目_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_0.Size = New System.Drawing.Size(89, 19)
        Me._lb_項目_0.TabIndex = 10
        Me._lb_項目_0.Text = "得意先"
        Me._lb_項目_0.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'rf_担当者名
        '
        Me.rf_担当者名.BackColor = System.Drawing.SystemColors.Control
        Me.rf_担当者名.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_担当者名.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_担当者名.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_担当者名.Location = New System.Drawing.Point(104, 8)
        Me.rf_担当者名.Name = "rf_担当者名"
        Me.rf_担当者名.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_担当者名.Size = New System.Drawing.Size(111, 19)
        Me.rf_担当者名.TabIndex = 9
        Me.rf_担当者名.Text = "wwwwwww"
        '
        '_lb_項目_8
        '
        Me._lb_項目_8.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_8.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_8.ForeColor = System.Drawing.Color.White
        Me._lb_項目_8.Location = New System.Drawing.Point(8, 85)
        Me._lb_項目_8.Name = "_lb_項目_8"
        Me._lb_項目_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_8.Size = New System.Drawing.Size(89, 19)
        Me._lb_項目_8.TabIndex = 7
        Me._lb_項目_8.Text = "製品名"
        Me._lb_項目_8.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_7
        '
        Me._lb_項目_7.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_7.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_7.ForeColor = System.Drawing.Color.White
        Me._lb_項目_7.Location = New System.Drawing.Point(8, 66)
        Me._lb_項目_7.Name = "_lb_項目_7"
        Me._lb_項目_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_7.Size = New System.Drawing.Size(89, 19)
        Me._lb_項目_7.TabIndex = 6
        Me._lb_項目_7.Text = "仕様№"
        Me._lb_項目_7.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_6
        '
        Me._lb_項目_6.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_6.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_6.ForeColor = System.Drawing.Color.White
        Me._lb_項目_6.Location = New System.Drawing.Point(8, 47)
        Me._lb_項目_6.Name = "_lb_項目_6"
        Me._lb_項目_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_6.Size = New System.Drawing.Size(89, 19)
        Me._lb_項目_6.TabIndex = 5
        Me._lb_項目_6.Text = "製品№"
        Me._lb_項目_6.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_5
        '
        Me._lb_項目_5.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_5.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_5.ForeColor = System.Drawing.Color.White
        Me._lb_項目_5.Location = New System.Drawing.Point(8, 8)
        Me._lb_項目_5.Name = "_lb_項目_5"
        Me._lb_項目_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_5.Size = New System.Drawing.Size(89, 19)
        Me._lb_項目_5.TabIndex = 4
        Me._lb_項目_5.Text = "担当者"
        Me._lb_項目_5.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'rf_製品名
        '
        Me.rf_製品名.BackColor = System.Drawing.SystemColors.Control
        Me.rf_製品名.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_製品名.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_製品名.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_製品名.Location = New System.Drawing.Point(104, 85)
        Me.rf_製品名.Name = "rf_製品名"
        Me.rf_製品名.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_製品名.Size = New System.Drawing.Size(307, 19)
        Me.rf_製品名.TabIndex = 3
        Me.rf_製品名.Text = "ＷＷＷＷＷＷＷＷＷＷＷＷＷＷＷＷＷＷＷＷ"
        '
        'rf_仕様NO
        '
        Me.rf_仕様NO.BackColor = System.Drawing.SystemColors.Control
        Me.rf_仕様NO.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_仕様NO.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_仕様NO.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_仕様NO.Location = New System.Drawing.Point(104, 66)
        Me.rf_仕様NO.Name = "rf_仕様NO"
        Me.rf_仕様NO.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_仕様NO.Size = New System.Drawing.Size(67, 19)
        Me.rf_仕様NO.TabIndex = 2
        Me.rf_仕様NO.Text = "wwwwwww"
        '
        'rf_製品NO
        '
        Me.rf_製品NO.BackColor = System.Drawing.SystemColors.Control
        Me.rf_製品NO.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_製品NO.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_製品NO.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_製品NO.Location = New System.Drawing.Point(104, 47)
        Me.rf_製品NO.Name = "rf_製品NO"
        Me.rf_製品NO.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_製品NO.Size = New System.Drawing.Size(67, 19)
        Me.rf_製品NO.TabIndex = 1
        Me.rf_製品NO.Text = "wwwwwww"
        '
        'FpSpd
        '
        Me.FpSpd.AccessibleDescription = "FpSpd, Sheet1, Row 0, Column 0"
        Me.FpSpd.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!)
        Me.FpSpd.Location = New System.Drawing.Point(8, 107)
        Me.FpSpd.Name = "FpSpd"
        Me.FpSpd.Size = New System.Drawing.Size(723, 276)
        Me.FpSpd.TabIndex = 13
        '
        'SnwMT02F09
        '
        Me.AcceptButton = Me.CdOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(736, 427)
        Me.Controls.Add(Me.FpSpd)
        Me.Controls.Add(Me.CdOK)
        Me.Controls.Add(Me.rf_得意先名1)
        Me.Controls.Add(Me.rf_得意先名2)
        Me.Controls.Add(Me._lb_項目_0)
        Me.Controls.Add(Me.rf_担当者名)
        Me.Controls.Add(Me._lb_項目_8)
        Me.Controls.Add(Me._lb_項目_7)
        Me.Controls.Add(Me._lb_項目_6)
        Me.Controls.Add(Me._lb_項目_5)
        Me.Controls.Add(Me.rf_製品名)
        Me.Controls.Add(Me.rf_仕様NO)
        Me.Controls.Add(Me.rf_製品NO)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(184, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SnwMT02F09"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "客先在庫履歴表示"
        CType(Me.FpSpd, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents FpSpd As FarPoint.Win.Spread.FpSpread
    Friend WithEvents FpSpd_Sheet1 As FarPoint.Win.Spread.SheetView

End Class