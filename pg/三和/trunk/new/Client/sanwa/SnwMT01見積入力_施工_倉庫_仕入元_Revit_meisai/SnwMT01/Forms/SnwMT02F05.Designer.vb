<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SnwMT02F05

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
	Public WithEvents CmdFind As System.Windows.Forms.Button
	Public WithEvents CmdCan As System.Windows.Forms.Button
	Public WithEvents CmdOk As System.Windows.Forms.Button
	Public WithEvents SelListVw As System.Windows.Forms.ListView
	Public WithEvents tx_得意先CD As ExText.ExTextBox
	Public WithEvents tx_仕入先CD As ExText.ExTextBox
	Public WithEvents tx_見積日付D As ExDateText.ExDateTextBoxD
	Public WithEvents tx_見積日付M As ExDateText.ExDateTextBoxM
	Public WithEvents tx_見積日付Y As ExDateText.ExDateTextBoxY
	Public WithEvents _lb_項目_1 As System.Windows.Forms.Label
	Public WithEvents _lb_年_3 As System.Windows.Forms.Label
	Public WithEvents _lb_月_3 As System.Windows.Forms.Label
	Public WithEvents _lb_日_3 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_0 As System.Windows.Forms.Label
	Public WithEvents rf_名称1 As System.Windows.Forms.Label
	Public WithEvents rf_名称2 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_12 As System.Windows.Forms.Label
	Public WithEvents lb_見積日付 As System.Windows.Forms.Label

    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使って変更できます。
    'コード エディタを使用して、変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SnwMT02F05))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.CmdFind = New System.Windows.Forms.Button()
        Me.CmdCan = New System.Windows.Forms.Button()
        Me.CmdOk = New System.Windows.Forms.Button()
        Me.SelListVw = New System.Windows.Forms.ListView()
        Me.tx_得意先CD = New ExText.ExTextBox()
        Me.tx_仕入先CD = New ExText.ExTextBox()
        Me.tx_見積日付D = New ExDateText.ExDateTextBoxD()
        Me.tx_見積日付M = New ExDateText.ExDateTextBoxM()
        Me.tx_見積日付Y = New ExDateText.ExDateTextBoxY()
        Me._lb_項目_1 = New System.Windows.Forms.Label()
        Me._lb_年_3 = New System.Windows.Forms.Label()
        Me._lb_月_3 = New System.Windows.Forms.Label()
        Me._lb_日_3 = New System.Windows.Forms.Label()
        Me._lb_項目_0 = New System.Windows.Forms.Label()
        Me.rf_名称1 = New System.Windows.Forms.Label()
        Me.rf_名称2 = New System.Windows.Forms.Label()
        Me._lb_項目_12 = New System.Windows.Forms.Label()
        Me.lb_見積日付 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'CmdFind
        '
        Me.CmdFind.BackColor = System.Drawing.SystemColors.Control
        Me.CmdFind.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmdFind.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CmdFind.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdFind.Location = New System.Drawing.Point(428, 48)
        Me.CmdFind.Name = "CmdFind"
        Me.CmdFind.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdFind.Size = New System.Drawing.Size(83, 25)
        Me.CmdFind.TabIndex = 5
        Me.CmdFind.Text = "検索(&F)"
        Me.CmdFind.UseVisualStyleBackColor = False
        '
        'CmdCan
        '
        Me.CmdCan.BackColor = System.Drawing.SystemColors.Control
        Me.CmdCan.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmdCan.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CmdCan.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CmdCan.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdCan.Location = New System.Drawing.Point(244, 384)
        Me.CmdCan.Name = "CmdCan"
        Me.CmdCan.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdCan.Size = New System.Drawing.Size(92, 22)
        Me.CmdCan.TabIndex = 8
        Me.CmdCan.Text = "ｷｬﾝｾﾙ(&C)"
        Me.CmdCan.UseVisualStyleBackColor = False
        '
        'CmdOk
        '
        Me.CmdOk.BackColor = System.Drawing.SystemColors.Control
        Me.CmdOk.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmdOk.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CmdOk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdOk.Location = New System.Drawing.Point(145, 384)
        Me.CmdOk.Name = "CmdOk"
        Me.CmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdOk.Size = New System.Drawing.Size(92, 22)
        Me.CmdOk.TabIndex = 7
        Me.CmdOk.Text = "ＯＫ(&O)"
        Me.CmdOk.UseVisualStyleBackColor = False
        '
        'SelListVw
        '
        Me.SelListVw.BackColor = System.Drawing.Color.FromArgb(CType(CType(198, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.SelListVw.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.SelListVw.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.SelListVw.ForeColor = System.Drawing.SystemColors.WindowText
        Me.SelListVw.FullRowSelect = True
        Me.SelListVw.HideSelection = False
        Me.SelListVw.Location = New System.Drawing.Point(8, 84)
        Me.SelListVw.Name = "SelListVw"
        Me.SelListVw.Size = New System.Drawing.Size(329, 290)
        Me.SelListVw.TabIndex = 6
        Me.SelListVw.UseCompatibleStateImageBehavior = False
        Me.SelListVw.View = System.Windows.Forms.View.Details
        '
        'tx_得意先CD
        '
        Me.tx_得意先CD.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_得意先CD.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_得意先CD.CanForwardSetFocus = True
        Me.tx_得意先CD.CanNextSetFocus = True
        Me.tx_得意先CD.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_得意先CD.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_得意先CD.EditMode = True
        Me.tx_得意先CD.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_得意先CD.IMEMode = System.Windows.Forms.ImeMode.NoControl
        Me.tx_得意先CD.LengthType = ExText.ExTextBox.LenType.ByteType
        Me.tx_得意先CD.Location = New System.Drawing.Point(88, 8)
        Me.tx_得意先CD.MaxLength = 4
        Me.tx_得意先CD.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_得意先CD.Name = "tx_得意先CD"
        Me.tx_得意先CD.OldValue = "8888"
        Me.tx_得意先CD.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_得意先CD.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_得意先CD.SelectText = True
        Me.tx_得意先CD.SelLength = 0
        Me.tx_得意先CD.SelStart = 0
        Me.tx_得意先CD.SelText = ""
        Me.tx_得意先CD.Size = New System.Drawing.Size(50, 22)
        Me.tx_得意先CD.TabIndex = 0
        Me.tx_得意先CD.Text = "8888"
        '
        'tx_仕入先CD
        '
        Me.tx_仕入先CD.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_仕入先CD.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_仕入先CD.CanForwardSetFocus = True
        Me.tx_仕入先CD.CanNextSetFocus = True
        Me.tx_仕入先CD.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_仕入先CD.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_仕入先CD.EditMode = True
        Me.tx_仕入先CD.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_仕入先CD.IMEMode = System.Windows.Forms.ImeMode.NoControl
        Me.tx_仕入先CD.LengthType = ExText.ExTextBox.LenType.ByteType
        Me.tx_仕入先CD.Location = New System.Drawing.Point(88, 28)
        Me.tx_仕入先CD.MaxLength = 4
        Me.tx_仕入先CD.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_仕入先CD.Name = "tx_仕入先CD"
        Me.tx_仕入先CD.OldValue = "8888"
        Me.tx_仕入先CD.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_仕入先CD.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_仕入先CD.SelectText = True
        Me.tx_仕入先CD.SelLength = 0
        Me.tx_仕入先CD.SelStart = 0
        Me.tx_仕入先CD.SelText = ""
        Me.tx_仕入先CD.Size = New System.Drawing.Size(50, 22)
        Me.tx_仕入先CD.TabIndex = 1
        Me.tx_仕入先CD.Text = "8888"
        '
        'tx_見積日付D
        '
        Me.tx_見積日付D.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_見積日付D.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_見積日付D.CanForwardSetFocus = True
        Me.tx_見積日付D.CanNextSetFocus = True
        Me.tx_見積日付D.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_見積日付D.EditMode = True
        Me.tx_見積日付D.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_見積日付D.Location = New System.Drawing.Point(176, 49)
        Me.tx_見積日付D.MaxLength = 2
        Me.tx_見積日付D.Name = "tx_見積日付D"
        Me.tx_見積日付D.OldValue = "88"
        Me.tx_見積日付D.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_見積日付D.SelectText = True
        Me.tx_見積日付D.SelLength = 0
        Me.tx_見積日付D.SelStart = 0
        Me.tx_見積日付D.SelText = ""
        Me.tx_見積日付D.Size = New System.Drawing.Size(20, 16)
        Me.tx_見積日付D.TabIndex = 4
        Me.tx_見積日付D.Text = "88"
        '
        'tx_見積日付M
        '
        Me.tx_見積日付M.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_見積日付M.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_見積日付M.CanForwardSetFocus = True
        Me.tx_見積日付M.CanNextSetFocus = True
        Me.tx_見積日付M.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_見積日付M.EditMode = True
        Me.tx_見積日付M.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_見積日付M.Location = New System.Drawing.Point(139, 49)
        Me.tx_見積日付M.MaxLength = 2
        Me.tx_見積日付M.Name = "tx_見積日付M"
        Me.tx_見積日付M.OldValue = "88"
        Me.tx_見積日付M.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_見積日付M.SelectText = True
        Me.tx_見積日付M.SelLength = 0
        Me.tx_見積日付M.SelStart = 0
        Me.tx_見積日付M.SelText = ""
        Me.tx_見積日付M.Size = New System.Drawing.Size(20, 16)
        Me.tx_見積日付M.TabIndex = 3
        Me.tx_見積日付M.Text = "88"
        '
        'tx_見積日付Y
        '
        Me.tx_見積日付Y.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_見積日付Y.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_見積日付Y.CanForwardSetFocus = True
        Me.tx_見積日付Y.CanNextSetFocus = True
        Me.tx_見積日付Y.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_見積日付Y.EditMode = True
        Me.tx_見積日付Y.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_見積日付Y.Location = New System.Drawing.Point(89, 49)
        Me.tx_見積日付Y.MaxLength = 4
        Me.tx_見積日付Y.Name = "tx_見積日付Y"
        Me.tx_見積日付Y.OldValue = "8888"
        Me.tx_見積日付Y.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_見積日付Y.SelectText = True
        Me.tx_見積日付Y.SelLength = 0
        Me.tx_見積日付Y.SelStart = 0
        Me.tx_見積日付Y.SelText = ""
        Me.tx_見積日付Y.Size = New System.Drawing.Size(32, 16)
        Me.tx_見積日付Y.TabIndex = 2
        Me.tx_見積日付Y.Text = "8888"
        '
        '_lb_項目_1
        '
        Me._lb_項目_1.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_1.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_1.ForeColor = System.Drawing.Color.White
        Me._lb_項目_1.Location = New System.Drawing.Point(8, 48)
        Me._lb_項目_1.Name = "_lb_項目_1"
        Me._lb_項目_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_1.Size = New System.Drawing.Size(80, 20)
        Me._lb_項目_1.TabIndex = 17
        Me._lb_項目_1.Text = "見積日付"
        Me._lb_項目_1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_年_3
        '
        Me._lb_年_3.BackColor = System.Drawing.SystemColors.Window
        Me._lb_年_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_年_3.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_年_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_年_3.Location = New System.Drawing.Point(125, 51)
        Me._lb_年_3.Name = "_lb_年_3"
        Me._lb_年_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_年_3.Size = New System.Drawing.Size(17, 15)
        Me._lb_年_3.TabIndex = 15
        Me._lb_年_3.Text = "年"
        Me._lb_年_3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_月_3
        '
        Me._lb_月_3.BackColor = System.Drawing.SystemColors.Window
        Me._lb_月_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_月_3.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_月_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_月_3.Location = New System.Drawing.Point(162, 51)
        Me._lb_月_3.Name = "_lb_月_3"
        Me._lb_月_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_月_3.Size = New System.Drawing.Size(12, 15)
        Me._lb_月_3.TabIndex = 14
        Me._lb_月_3.Text = "月"
        Me._lb_月_3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_日_3
        '
        Me._lb_日_3.BackColor = System.Drawing.SystemColors.Window
        Me._lb_日_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_日_3.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_日_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_日_3.Location = New System.Drawing.Point(204, 51)
        Me._lb_日_3.Name = "_lb_日_3"
        Me._lb_日_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_日_3.Size = New System.Drawing.Size(12, 15)
        Me._lb_日_3.TabIndex = 13
        Me._lb_日_3.Text = "日"
        Me._lb_日_3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_0
        '
        Me._lb_項目_0.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_0.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_0.ForeColor = System.Drawing.Color.White
        Me._lb_項目_0.Location = New System.Drawing.Point(8, 8)
        Me._lb_項目_0.Name = "_lb_項目_0"
        Me._lb_項目_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_0.Size = New System.Drawing.Size(80, 20)
        Me._lb_項目_0.TabIndex = 12
        Me._lb_項目_0.Text = "得意先"
        Me._lb_項目_0.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'rf_名称1
        '
        Me.rf_名称1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.rf_名称1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.rf_名称1.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_名称1.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_名称1.ForeColor = System.Drawing.Color.Black
        Me.rf_名称1.Location = New System.Drawing.Point(138, 8)
        Me.rf_名称1.Name = "rf_名称1"
        Me.rf_名称1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_名称1.Size = New System.Drawing.Size(236, 20)
        Me.rf_名称1.TabIndex = 11
        Me.rf_名称1.Text = "１２３４５６７８９０１２３４"
        Me.rf_名称1.Visible = False
        '
        'rf_名称2
        '
        Me.rf_名称2.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.rf_名称2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.rf_名称2.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_名称2.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_名称2.ForeColor = System.Drawing.Color.Black
        Me.rf_名称2.Location = New System.Drawing.Point(138, 28)
        Me.rf_名称2.Name = "rf_名称2"
        Me.rf_名称2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_名称2.Size = New System.Drawing.Size(236, 20)
        Me.rf_名称2.TabIndex = 10
        Me.rf_名称2.Text = "１２３４５６７８９０１２３４"
        Me.rf_名称2.Visible = False
        '
        '_lb_項目_12
        '
        Me._lb_項目_12.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_12.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_12.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_12.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_12.ForeColor = System.Drawing.Color.White
        Me._lb_項目_12.Location = New System.Drawing.Point(8, 28)
        Me._lb_項目_12.Name = "_lb_項目_12"
        Me._lb_項目_12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_12.Size = New System.Drawing.Size(80, 20)
        Me._lb_項目_12.TabIndex = 9
        Me._lb_項目_12.Text = "仕入先"
        Me._lb_項目_12.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_見積日付
        '
        Me.lb_見積日付.BackColor = System.Drawing.SystemColors.Window
        Me.lb_見積日付.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lb_見積日付.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_見積日付.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_見積日付.Location = New System.Drawing.Point(88, 48)
        Me.lb_見積日付.Name = "lb_見積日付"
        Me.lb_見積日付.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_見積日付.Size = New System.Drawing.Size(133, 22)
        Me.lb_見積日付.TabIndex = 16
        '
        'SnwMT02F05
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.CmdCan
        Me.ClientSize = New System.Drawing.Size(549, 414)
        Me.Controls.Add(Me.CmdFind)
        Me.Controls.Add(Me.CmdCan)
        Me.Controls.Add(Me.CmdOk)
        Me.Controls.Add(Me.SelListVw)
        Me.Controls.Add(Me.tx_得意先CD)
        Me.Controls.Add(Me.tx_仕入先CD)
        Me.Controls.Add(Me.tx_見積日付D)
        Me.Controls.Add(Me.tx_見積日付M)
        Me.Controls.Add(Me.tx_見積日付Y)
        Me.Controls.Add(Me._lb_項目_1)
        Me.Controls.Add(Me._lb_年_3)
        Me.Controls.Add(Me._lb_月_3)
        Me.Controls.Add(Me._lb_日_3)
        Me.Controls.Add(Me._lb_項目_0)
        Me.Controls.Add(Me.rf_名称1)
        Me.Controls.Add(Me.rf_名称2)
        Me.Controls.Add(Me._lb_項目_12)
        Me.Controls.Add(Me.lb_見積日付)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SnwMT02F05"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.Text = "前回単価参照"
        Me.ResumeLayout(False)

    End Sub

End Class