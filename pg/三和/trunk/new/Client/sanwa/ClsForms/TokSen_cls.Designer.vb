<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TokSen_cls
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(TokSen_cls))
        Me.CmdOk = New System.Windows.Forms.Button()
        Me.SelListVw = New SortableListView()
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
        Me.SuspendLayout()
        '
        'CmdOk
        '
        Me.CmdOk.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CmdOk.Location = New System.Drawing.Point(321, 426)
        Me.CmdOk.Margin = New System.Windows.Forms.Padding(4, 2, 4, 2)
        Me.CmdOk.Name = "CmdOk"
        Me.CmdOk.Size = New System.Drawing.Size(92, 25)
        Me.CmdOk.TabIndex = 11
        Me.CmdOk.Text = "ＯＫ(&O)"
        Me.CmdOk.UseVisualStyleBackColor = True
        '
        'SelListVw
        '
        Me.SelListVw.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SelListVw.BackColor = System.Drawing.Color.FromArgb(CType(CType(198, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.SelListVw.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.SelListVw.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!)
        Me.SelListVw.FullRowSelect = True
        Me.SelListVw.HideSelection = False
        Me.SelListVw.Location = New System.Drawing.Point(8, 124)
        Me.SelListVw.Margin = New System.Windows.Forms.Padding(4, 2, 4, 2)
        Me.SelListVw.Name = "SelListVw"
        Me.SelListVw.Size = New System.Drawing.Size(507, 282)
        Me.SelListVw.SortStyle = SortableListView.SortStyles.SortDefault
        Me.SelListVw.TabIndex = 10
        Me.SelListVw.UseCompatibleStateImageBehavior = False
        Me.SelListVw.View = System.Windows.Forms.View.Details
        '
        'CmdCan
        '
        Me.CmdCan.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CmdCan.Location = New System.Drawing.Point(421, 426)
        Me.CmdCan.Margin = New System.Windows.Forms.Padding(4, 2, 4, 2)
        Me.CmdCan.Name = "CmdCan"
        Me.CmdCan.Size = New System.Drawing.Size(92, 25)
        Me.CmdCan.TabIndex = 12
        Me.CmdCan.Text = "ｷｬﾝｾﾙ(&C)"
        Me.CmdCan.UseVisualStyleBackColor = True
        '
        'CmdFind
        '
        Me.CmdFind.Location = New System.Drawing.Point(419, 82)
        Me.CmdFind.Margin = New System.Windows.Forms.Padding(4, 2, 4, 2)
        Me.CmdFind.Name = "CmdFind"
        Me.CmdFind.Size = New System.Drawing.Size(83, 25)
        Me.CmdFind.TabIndex = 9
        Me.CmdFind.Text = "検索(&F)"
        Me.CmdFind.UseVisualStyleBackColor = True
        '
        'tx_検索ID
        '
        Me.tx_検索ID.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_検索ID.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_検索ID.CanForwardSetFocus = True
        Me.tx_検索ID.CanNextSetFocus = True
        Me.tx_検索ID.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_検索ID.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_検索ID.EditMode = True
        Me.tx_検索ID.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_検索ID.IMEMode = System.Windows.Forms.ImeMode.Disable
        Me.tx_検索ID.LengthType = ExText.ExTextBox.LenType.ByteType
        Me.tx_検索ID.Location = New System.Drawing.Point(97, 47)
        Me.tx_検索ID.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.tx_検索ID.MaxLength = 4
        Me.tx_検索ID.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_検索ID.Name = "tx_検索ID"
        Me.tx_検索ID.OldValue = "WWWW"
        Me.tx_検索ID.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_検索ID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_検索ID.SelectText = True
        Me.tx_検索ID.SelLength = 0
        Me.tx_検索ID.SelStart = 0
        Me.tx_検索ID.SelText = ""
        Me.tx_検索ID.Size = New System.Drawing.Size(60, 22)
        Me.tx_検索ID.TabIndex = 4
        Me.tx_検索ID.Text = "WWWW"
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
        Me.tx_検索名.Location = New System.Drawing.Point(97, 68)
        Me.tx_検索名.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
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
        Me.tx_検索名.Size = New System.Drawing.Size(232, 22)
        Me.tx_検索名.TabIndex = 6
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
        Me.tx_検索カナ.Location = New System.Drawing.Point(97, 89)
        Me.tx_検索カナ.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
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
        Me.tx_検索カナ.TabIndex = 8
        Me.tx_検索カナ.Text = "WWWWW"
        '
        '_lb_項目_2
        '
        Me._lb_項目_2.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_2.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_2.ForeColor = System.Drawing.Color.White
        Me._lb_項目_2.Location = New System.Drawing.Point(8, 89)
        Me._lb_項目_2.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_項目_2.Name = "_lb_項目_2"
        Me._lb_項目_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_2.Size = New System.Drawing.Size(89, 20)
        Me._lb_項目_2.TabIndex = 7
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
        Me._lb_項目_1.Location = New System.Drawing.Point(8, 68)
        Me._lb_項目_1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_項目_1.Name = "_lb_項目_1"
        Me._lb_項目_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_1.Size = New System.Drawing.Size(89, 20)
        Me._lb_項目_1.TabIndex = 5
        Me._lb_項目_1.Text = "得意先名1"
        Me._lb_項目_1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lbGuide
        '
        Me.lbGuide.BackColor = System.Drawing.SystemColors.Control
        Me.lbGuide.Cursor = System.Windows.Forms.Cursors.Default
        Me.lbGuide.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lbGuide.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lbGuide.Location = New System.Drawing.Point(12, 28)
        Me.lbGuide.Name = "lbGuide"
        Me.lbGuide.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lbGuide.Size = New System.Drawing.Size(317, 18)
        Me.lbGuide.TabIndex = 2
        Me.lbGuide.Text = "[↑][↓]で選択、[Enter]で決定 ／[Esc]で中止"
        '
        'lbListCount
        '
        Me.lbListCount.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.lbListCount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lbListCount.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lbListCount.ForeColor = System.Drawing.Color.Yellow
        Me.lbListCount.Location = New System.Drawing.Point(77, 8)
        Me.lbListCount.Name = "lbListCount"
        Me.lbListCount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lbListCount.Size = New System.Drawing.Size(89, 18)
        Me.lbListCount.TabIndex = 1
        Me.lbListCount.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lb_該当件数
        '
        Me.lb_該当件数.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.lb_該当件数.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_該当件数.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_該当件数.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_該当件数.Location = New System.Drawing.Point(9, 8)
        Me.lb_該当件数.Name = "lb_該当件数"
        Me.lb_該当件数.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_該当件数.Size = New System.Drawing.Size(69, 18)
        Me.lb_該当件数.TabIndex = 0
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
        Me._lb_項目_0.Location = New System.Drawing.Point(8, 47)
        Me._lb_項目_0.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_項目_0.Name = "_lb_項目_0"
        Me._lb_項目_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_0.Size = New System.Drawing.Size(89, 20)
        Me._lb_項目_0.TabIndex = 3
        Me._lb_項目_0.Text = "得意先CD"
        Me._lb_項目_0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TokSen_cls
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(528, 466)
        Me.Controls.Add(Me.tx_検索ID)
        Me.Controls.Add(Me.tx_検索名)
        Me.Controls.Add(Me.tx_検索カナ)
        Me.Controls.Add(Me._lb_項目_2)
        Me.Controls.Add(Me._lb_項目_1)
        Me.Controls.Add(Me.lbGuide)
        Me.Controls.Add(Me.lbListCount)
        Me.Controls.Add(Me.lb_該当件数)
        Me.Controls.Add(Me._lb_項目_0)
        Me.Controls.Add(Me.CmdFind)
        Me.Controls.Add(Me.CmdCan)
        Me.Controls.Add(Me.CmdOk)
        Me.Controls.Add(Me.SelListVw)
        Me.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4, 2, 4, 2)
        Me.Name = "TokSen_cls"
        Me.Text = "得意先選択"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SelListVw As SortableListView
    Friend WithEvents CmdOk As System.Windows.Forms.Button
    Friend WithEvents CmdCan As Button
    Friend WithEvents CmdFind As Button
    Public WithEvents tx_検索ID As ExText.ExTextBox
    Public WithEvents tx_検索名 As ExText.ExTextBox
    Public WithEvents tx_検索カナ As ExText.ExTextBox
    Public WithEvents _lb_項目_2 As Label
    Public WithEvents _lb_項目_1 As Label
    Public WithEvents lbGuide As Label
    Public WithEvents lbListCount As Label
    Public WithEvents lb_該当件数 As Label
    Public WithEvents _lb_項目_0 As Label
End Class