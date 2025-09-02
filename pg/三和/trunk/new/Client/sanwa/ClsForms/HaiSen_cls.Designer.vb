<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class HaiSen_cls
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使って変更できます。
    'コード エディタを使用して、変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(HaiSen_cls))
        Me.CmdOk = New System.Windows.Forms.Button()
        Me.CmdCan = New System.Windows.Forms.Button()
        Me.CmdFind = New System.Windows.Forms.Button()
        Me.tx_検索ID = New ExText.ExTextBox()
        Me.tx_検索名 = New ExText.ExTextBox()
        Me.tx_検索カナ = New ExText.ExTextBox()
        Me._lb_項目_2 = New System.Windows.Forms.Label()
        Me._lb_項目_1 = New System.Windows.Forms.Label()
        Me.lbGuide = New System.Windows.Forms.Label()
        Me.lbListCount = New System.Windows.Forms.Label()
        Me.lb_該当件数 = New System.Windows.Forms.Label()
        Me._lb_項目_0 = New System.Windows.Forms.Label()
        Me.SelListVw = New SortableListView()
        Me.SuspendLayout()
        '
        'CmdOk
        '
        Me.CmdOk.BackColor = System.Drawing.SystemColors.Control
        Me.CmdOk.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmdOk.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CmdOk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdOk.Location = New System.Drawing.Point(277, 401)
        Me.CmdOk.Name = "CmdOk"
        Me.CmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdOk.Size = New System.Drawing.Size(92, 25)
        Me.CmdOk.TabIndex = 5
        Me.CmdOk.Text = "ＯＫ(&O)"
        Me.CmdOk.UseVisualStyleBackColor = False
        '
        'CmdCan
        '
        Me.CmdCan.BackColor = System.Drawing.SystemColors.Control
        Me.CmdCan.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmdCan.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CmdCan.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CmdCan.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdCan.Location = New System.Drawing.Point(376, 401)
        Me.CmdCan.Name = "CmdCan"
        Me.CmdCan.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdCan.Size = New System.Drawing.Size(92, 25)
        Me.CmdCan.TabIndex = 6
        Me.CmdCan.Text = "ｷｬﾝｾﾙ(&C)"
        Me.CmdCan.UseVisualStyleBackColor = False
        '
        'CmdFind
        '
        Me.CmdFind.BackColor = System.Drawing.SystemColors.Control
        Me.CmdFind.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmdFind.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CmdFind.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdFind.Location = New System.Drawing.Point(383, 80)
        Me.CmdFind.Name = "CmdFind"
        Me.CmdFind.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdFind.Size = New System.Drawing.Size(83, 25)
        Me.CmdFind.TabIndex = 3
        Me.CmdFind.Text = "検索(&F)"
        Me.CmdFind.UseVisualStyleBackColor = False
        '
        'tx_検索ID
        '
        Me.tx_検索ID.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_検索ID.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_検索ID.CanForwardSetFocus = False
        Me.tx_検索ID.CanNextSetFocus = True
        Me.tx_検索ID.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_検索ID.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_検索ID.EditMode = False
        Me.tx_検索ID.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_検索ID.IMEMode = System.Windows.Forms.ImeMode.Disable
        Me.tx_検索ID.LengthType = ExText.ExTextBox.LenType.ByteType
        Me.tx_検索ID.Location = New System.Drawing.Point(99, 45)
        Me.tx_検索ID.MaxLength = 2
        Me.tx_検索ID.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_検索ID.Name = "tx_検索ID"
        Me.tx_検索ID.OldValue = "WW"
        Me.tx_検索ID.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_検索ID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_検索ID.SelectText = True
        Me.tx_検索ID.SelLength = 0
        Me.tx_検索ID.SelStart = 0
        Me.tx_検索ID.SelText = ""
        Me.tx_検索ID.Size = New System.Drawing.Size(37, 22)
        Me.tx_検索ID.TabIndex = 0
        Me.tx_検索ID.Text = "WW"
        '
        'tx_検索名
        '
        Me.tx_検索名.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_検索名.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_検索名.CanForwardSetFocus = True
        Me.tx_検索名.CanNextSetFocus = True
        Me.tx_検索名.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_検索名.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_検索名.EditMode = True
        Me.tx_検索名.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_検索名.IMEMode = System.Windows.Forms.ImeMode.Hiragana
        Me.tx_検索名.LengthType = ExText.ExTextBox.LenType.ByteType
        Me.tx_検索名.Location = New System.Drawing.Point(99, 68)
        Me.tx_検索名.MaxLength = 28
        Me.tx_検索名.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_検索名.Name = "tx_検索名"
        Me.tx_検索名.OldValue = "WWWWWWWWWWWWWWWWWWWWWWWWWWWW"
        Me.tx_検索名.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_検索名.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_検索名.SelectText = True
        Me.tx_検索名.SelLength = 0
        Me.tx_検索名.SelStart = 0
        Me.tx_検索名.SelText = ""
        Me.tx_検索名.Size = New System.Drawing.Size(227, 22)
        Me.tx_検索名.TabIndex = 1
        Me.tx_検索名.Text = "WWWWWWWWWWWWWWWWWWWWWWWWWWWW"
        '
        'tx_検索カナ
        '
        Me.tx_検索カナ.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_検索カナ.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_検索カナ.CanForwardSetFocus = True
        Me.tx_検索カナ.CanNextSetFocus = True
        Me.tx_検索カナ.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_検索カナ.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_検索カナ.EditMode = True
        Me.tx_検索カナ.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_検索カナ.IMEMode = System.Windows.Forms.ImeMode.KatakanaHalf
        Me.tx_検索カナ.LengthType = ExText.ExTextBox.LenType.ByteType
        Me.tx_検索カナ.Location = New System.Drawing.Point(99, 91)
        Me.tx_検索カナ.MaxLength = 5
        Me.tx_検索カナ.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_検索カナ.Name = "tx_検索カナ"
        Me.tx_検索カナ.OldValue = "WWWWW"
        Me.tx_検索カナ.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_検索カナ.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_検索カナ.SelectText = True
        Me.tx_検索カナ.SelLength = 0
        Me.tx_検索カナ.SelStart = 0
        Me.tx_検索カナ.SelText = ""
        Me.tx_検索カナ.Size = New System.Drawing.Size(60, 22)
        Me.tx_検索カナ.TabIndex = 2
        Me.tx_検索カナ.Text = "WWWWW"
        '
        '_lb_項目_2
        '
        Me._lb_項目_2.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_2.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_2.ForeColor = System.Drawing.Color.White
        Me._lb_項目_2.Location = New System.Drawing.Point(10, 91)
        Me._lb_項目_2.Name = "_lb_項目_2"
        Me._lb_項目_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_2.Size = New System.Drawing.Size(89, 22)
        Me._lb_項目_2.TabIndex = 12
        Me._lb_項目_2.Text = "フリガナ"
        Me._lb_項目_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        '_lb_項目_1
        '
        Me._lb_項目_1.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_1.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_1.ForeColor = System.Drawing.Color.White
        Me._lb_項目_1.Location = New System.Drawing.Point(10, 68)
        Me._lb_項目_1.Name = "_lb_項目_1"
        Me._lb_項目_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_1.Size = New System.Drawing.Size(89, 22)
        Me._lb_項目_1.TabIndex = 11
        Me._lb_項目_1.Text = "配送先名1"
        Me._lb_項目_1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lbGuide
        '
        Me.lbGuide.BackColor = System.Drawing.SystemColors.Control
        Me.lbGuide.Cursor = System.Windows.Forms.Cursors.Default
        Me.lbGuide.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lbGuide.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lbGuide.Location = New System.Drawing.Point(11, 25)
        Me.lbGuide.Name = "lbGuide"
        Me.lbGuide.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lbGuide.Size = New System.Drawing.Size(314, 17)
        Me.lbGuide.TabIndex = 10
        Me.lbGuide.Text = "[↑][↓]で選択、[Enter]で決定 ／[Esc]で中止"
        '
        'lbListCount
        '
        Me.lbListCount.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.lbListCount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lbListCount.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lbListCount.ForeColor = System.Drawing.Color.Yellow
        Me.lbListCount.Location = New System.Drawing.Point(79, 5)
        Me.lbListCount.Name = "lbListCount"
        Me.lbListCount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lbListCount.Size = New System.Drawing.Size(89, 17)
        Me.lbListCount.TabIndex = 9
        Me.lbListCount.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lb_該当件数
        '
        Me.lb_該当件数.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.lb_該当件数.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_該当件数.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_該当件数.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_該当件数.Location = New System.Drawing.Point(11, 5)
        Me.lb_該当件数.Name = "lb_該当件数"
        Me.lb_該当件数.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_該当件数.Size = New System.Drawing.Size(69, 17)
        Me.lb_該当件数.TabIndex = 8
        Me.lb_該当件数.Text = "該当件数"
        Me.lb_該当件数.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_0
        '
        Me._lb_項目_0.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_0.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_0.ForeColor = System.Drawing.Color.White
        Me._lb_項目_0.Location = New System.Drawing.Point(10, 45)
        Me._lb_項目_0.Name = "_lb_項目_0"
        Me._lb_項目_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_0.Size = New System.Drawing.Size(89, 22)
        Me._lb_項目_0.TabIndex = 7
        Me._lb_項目_0.Text = "配送先CD"
        Me._lb_項目_0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'SelListVw
        '
        Me.SelListVw.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SelListVw.BackColor = System.Drawing.Color.FromArgb(CType(CType(198, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.SelListVw.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.SelListVw.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.SelListVw.ForeColor = System.Drawing.SystemColors.WindowText
        Me.SelListVw.FullRowSelect = True
        Me.SelListVw.HideSelection = False
        Me.SelListVw.Location = New System.Drawing.Point(8, 119)
        Me.SelListVw.Name = "SelListVw"
        Me.SelListVw.Size = New System.Drawing.Size(458, 274)
        Me.SelListVw.SortStyle = SortableListView.SortStyles.SortDefault
        Me.SelListVw.TabIndex = 4
        Me.SelListVw.UseCompatibleStateImageBehavior = False
        Me.SelListVw.View = System.Windows.Forms.View.Details
        '
        'HaiSen_cls
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.CmdCan
        Me.ClientSize = New System.Drawing.Size(486, 431)
        Me.Controls.Add(Me.SelListVw)
        Me.Controls.Add(Me.CmdOk)
        Me.Controls.Add(Me.CmdCan)
        Me.Controls.Add(Me.CmdFind)
        Me.Controls.Add(Me.tx_検索ID)
        Me.Controls.Add(Me.tx_検索名)
        Me.Controls.Add(Me.tx_検索カナ)
        Me.Controls.Add(Me._lb_項目_2)
        Me.Controls.Add(Me._lb_項目_1)
        Me.Controls.Add(Me.lbGuide)
        Me.Controls.Add(Me.lbListCount)
        Me.Controls.Add(Me.lb_該当件数)
        Me.Controls.Add(Me._lb_項目_0)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "HaiSen_cls"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "配送先選択"
        Me.ResumeLayout(False)

    End Sub
    'Private components As System.ComponentModel.IContainer
    Friend WithEvents SelListVw As SortableListView
    Friend WithEvents CmdOk As System.Windows.Forms.Button
    Friend WithEvents CmdCan As System.Windows.Forms.Button
    Friend WithEvents CmdFind As System.Windows.Forms.Button
    Public ToolTip1 As System.Windows.Forms.ToolTip
    Public WithEvents tx_検索ID As ExText.ExTextBox
    Public WithEvents tx_検索名 As ExText.ExTextBox
    Public WithEvents tx_検索カナ As ExText.ExTextBox
    Public WithEvents _lb_項目_2 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_1 As System.Windows.Forms.Label
    Public WithEvents lbGuide As System.Windows.Forms.Label
    Public WithEvents lbListCount As System.Windows.Forms.Label
    Public WithEvents lb_該当件数 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_0 As System.Windows.Forms.Label
End Class