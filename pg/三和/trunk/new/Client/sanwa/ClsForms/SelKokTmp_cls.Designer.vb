<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SelKokTmp_cls
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

    'ƒƒ‚: ˆÈ‰º‚ÌƒvƒƒV[ƒWƒƒ‚Í Windows ƒtƒH[ƒ€ ƒfƒUƒCƒi‚Å•K—v‚Å‚·B
    'Windows ƒtƒH[ƒ€ ƒfƒUƒCƒi‚ğg‚Á‚Ä•ÏX‚Å‚«‚Ü‚·B
    'ƒR[ƒh ƒGƒfƒBƒ^‚ğg—p‚µ‚ÄA•ÏX‚µ‚È‚¢‚Å‚­‚¾‚³‚¢B
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SelKokTmp_cls))
        Me.SelListVw = New SortableListView()
        Me.CmdOk = New System.Windows.Forms.Button()
        Me.CmdCan = New System.Windows.Forms.Button()
        Me.CmdFind = New System.Windows.Forms.Button()
        Me.tx_ŒŸõ–¼Ì = New ExText.ExTextBox()
        Me.tx_“¾ˆÓæCD = New ExText.ExTextBox()
        Me.rf_“¾ˆÓæ–¼ = New System.Windows.Forms.Label()
        Me._lb_€–Ú_2 = New System.Windows.Forms.Label()
        Me.lbGuide = New System.Windows.Forms.Label()
        Me.lbListCount = New System.Windows.Forms.Label()
        Me.lb_ŠY“–Œ” = New System.Windows.Forms.Label()
        Me._lb_€–Ú_3 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'SelListVw
        '
        Me.SelListVw.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SelListVw.BackColor = System.Drawing.Color.FromArgb(CType(CType(198, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.SelListVw.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.SelListVw.Font = New System.Drawing.Font("‚l‚r ƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.SelListVw.ForeColor = System.Drawing.SystemColors.WindowText
        Me.SelListVw.FullRowSelect = True
        Me.SelListVw.HideSelection = False
        Me.SelListVw.Location = New System.Drawing.Point(8, 92)
        Me.SelListVw.Name = "SelListVw"
        Me.SelListVw.Size = New System.Drawing.Size(351, 300)
        Me.SelListVw.SortStyle = SortableListView.SortStyles.SortDefault
        Me.SelListVw.TabIndex = 3
        Me.SelListVw.UseCompatibleStateImageBehavior = False
        Me.SelListVw.View = System.Windows.Forms.View.Details
        '
        'CmdOk
        '
        Me.CmdOk.BackColor = System.Drawing.SystemColors.Control
        Me.CmdOk.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmdOk.Font = New System.Drawing.Font("‚l‚r ƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CmdOk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdOk.Location = New System.Drawing.Point(270, 400)
        Me.CmdOk.Name = "CmdOk"
        Me.CmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdOk.Size = New System.Drawing.Size(92, 25)
        Me.CmdOk.TabIndex = 4
        Me.CmdOk.Text = "‚n‚j(&O)"
        Me.CmdOk.UseVisualStyleBackColor = False
        '
        'CmdCan
        '
        Me.CmdCan.BackColor = System.Drawing.SystemColors.Control
        Me.CmdCan.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmdCan.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CmdCan.Font = New System.Drawing.Font("‚l‚r ƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CmdCan.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdCan.Location = New System.Drawing.Point(372, 400)
        Me.CmdCan.Name = "CmdCan"
        Me.CmdCan.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdCan.Size = New System.Drawing.Size(92, 25)
        Me.CmdCan.TabIndex = 5
        Me.CmdCan.Text = "·¬İ¾Ù(&C)"
        Me.CmdCan.UseVisualStyleBackColor = False
        '
        'CmdFind
        '
        Me.CmdFind.BackColor = System.Drawing.SystemColors.Control
        Me.CmdFind.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmdFind.Font = New System.Drawing.Font("‚l‚r ƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CmdFind.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdFind.Location = New System.Drawing.Point(384, 60)
        Me.CmdFind.Name = "CmdFind"
        Me.CmdFind.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdFind.Size = New System.Drawing.Size(83, 25)
        Me.CmdFind.TabIndex = 2
        Me.CmdFind.Text = "ŒŸõ(&F)"
        Me.CmdFind.UseVisualStyleBackColor = False
        '
        'tx_ŒŸõ–¼Ì
        '
        Me.tx_ŒŸõ–¼Ì.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_ŒŸõ–¼Ì.BorderStyle = ExText.ExTextBox.BorderStyleType.Àü
        Me.tx_ŒŸõ–¼Ì.CanForwardSetFocus = True
        Me.tx_ŒŸõ–¼Ì.CanNextSetFocus = True
        Me.tx_ŒŸõ–¼Ì.CharacterSize = ExText.ExTextBox.CharSize.‚È‚µ
        Me.tx_ŒŸõ–¼Ì.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_ŒŸõ–¼Ì.EditMode = True
        Me.tx_ŒŸõ–¼Ì.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_ŒŸõ–¼Ì.IMEMode = System.Windows.Forms.ImeMode.Hiragana
        Me.tx_ŒŸõ–¼Ì.LengthType = ExText.ExTextBox.LenType.ByteType
        Me.tx_ŒŸõ–¼Ì.Location = New System.Drawing.Point(109, 64)
        Me.tx_ŒŸõ–¼Ì.MaxLength = 40
        Me.tx_ŒŸõ–¼Ì.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_ŒŸõ–¼Ì.Name = "tx_ŒŸõ–¼Ì"
        Me.tx_ŒŸõ–¼Ì.OldValue = "ExTextBox"
        Me.tx_ŒŸõ–¼Ì.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_ŒŸõ–¼Ì.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_ŒŸõ–¼Ì.SelectText = True
        Me.tx_ŒŸõ–¼Ì.SelLength = 0
        Me.tx_ŒŸõ–¼Ì.SelStart = 0
        Me.tx_ŒŸõ–¼Ì.SelText = ""
        Me.tx_ŒŸõ–¼Ì.Size = New System.Drawing.Size(267, 22)
        Me.tx_ŒŸõ–¼Ì.TabIndex = 1
        Me.tx_ŒŸõ–¼Ì.Text = "ExTextBox"
        '
        'tx_“¾ˆÓæCD
        '
        Me.tx_“¾ˆÓæCD.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_“¾ˆÓæCD.BorderStyle = ExText.ExTextBox.BorderStyleType.Àü
        Me.tx_“¾ˆÓæCD.CanForwardSetFocus = True
        Me.tx_“¾ˆÓæCD.CanNextSetFocus = True
        Me.tx_“¾ˆÓæCD.CharacterSize = ExText.ExTextBox.CharSize.‚È‚µ
        Me.tx_“¾ˆÓæCD.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_“¾ˆÓæCD.EditMode = True
        Me.tx_“¾ˆÓæCD.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_“¾ˆÓæCD.IMEMode = System.Windows.Forms.ImeMode.Disable
        Me.tx_“¾ˆÓæCD.LengthType = ExText.ExTextBox.LenType.ByteType
        Me.tx_“¾ˆÓæCD.Location = New System.Drawing.Point(109, 44)
        Me.tx_“¾ˆÓæCD.MaxLength = 4
        Me.tx_“¾ˆÓæCD.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_“¾ˆÓæCD.Name = "tx_“¾ˆÓæCD"
        Me.tx_“¾ˆÓæCD.OldValue = "1234"
        Me.tx_“¾ˆÓæCD.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_“¾ˆÓæCD.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_“¾ˆÓæCD.SelectText = True
        Me.tx_“¾ˆÓæCD.SelLength = 0
        Me.tx_“¾ˆÓæCD.SelStart = 0
        Me.tx_“¾ˆÓæCD.SelText = ""
        Me.tx_“¾ˆÓæCD.Size = New System.Drawing.Size(47, 22)
        Me.tx_“¾ˆÓæCD.TabIndex = 0
        Me.tx_“¾ˆÓæCD.Text = "1234"
        '
        'rf_“¾ˆÓæ–¼
        '
        Me.rf_“¾ˆÓæ–¼.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.rf_“¾ˆÓæ–¼.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.rf_“¾ˆÓæ–¼.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_“¾ˆÓæ–¼.Font = New System.Drawing.Font("‚l‚r ƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_“¾ˆÓæ–¼.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_“¾ˆÓæ–¼.Location = New System.Drawing.Point(156, 44)
        Me.rf_“¾ˆÓæ–¼.Name = "rf_“¾ˆÓæ–¼"
        Me.rf_“¾ˆÓæ–¼.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_“¾ˆÓæ–¼.Size = New System.Drawing.Size(220, 19)
        Me.rf_“¾ˆÓæ–¼.TabIndex = 11
        Me.rf_“¾ˆÓæ–¼.Text = "‚P‚Q‚R‚S‚T‚U‚V‚W‚X‚O‚P‚Q‚R‚S"
        '
        '_lb_€–Ú_2
        '
        Me._lb_€–Ú_2.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_€–Ú_2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_€–Ú_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_€–Ú_2.Font = New System.Drawing.Font("‚l‚r ‚oƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_€–Ú_2.ForeColor = System.Drawing.Color.White
        Me._lb_€–Ú_2.Location = New System.Drawing.Point(10, 64)
        Me._lb_€–Ú_2.Name = "_lb_€–Ú_2"
        Me._lb_€–Ú_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_€–Ú_2.Size = New System.Drawing.Size(99, 19)
        Me._lb_€–Ú_2.TabIndex = 10
        Me._lb_€–Ú_2.Text = "ƒeƒ“ƒvƒŒ[ƒg–¼"
        Me._lb_€–Ú_2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lbGuide
        '
        Me.lbGuide.BackColor = System.Drawing.SystemColors.Control
        Me.lbGuide.Cursor = System.Windows.Forms.Cursors.Default
        Me.lbGuide.Font = New System.Drawing.Font("‚l‚r ‚oƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lbGuide.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lbGuide.Location = New System.Drawing.Point(11, 25)
        Me.lbGuide.Name = "lbGuide"
        Me.lbGuide.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lbGuide.Size = New System.Drawing.Size(305, 17)
        Me.lbGuide.TabIndex = 9
        Me.lbGuide.Text = "[ª][«]‚Å‘I‘ğA[Enter]‚ÅŒˆ’è ^[Esc]‚Å’†~"
        '
        'lbListCount
        '
        Me.lbListCount.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.lbListCount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lbListCount.Font = New System.Drawing.Font("‚l‚r ‚oƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lbListCount.ForeColor = System.Drawing.Color.Yellow
        Me.lbListCount.Location = New System.Drawing.Point(79, 5)
        Me.lbListCount.Name = "lbListCount"
        Me.lbListCount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lbListCount.Size = New System.Drawing.Size(89, 17)
        Me.lbListCount.TabIndex = 8
        Me.lbListCount.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lb_ŠY“–Œ”
        '
        Me.lb_ŠY“–Œ”.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.lb_ŠY“–Œ”.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_ŠY“–Œ”.Font = New System.Drawing.Font("‚l‚r ‚oƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_ŠY“–Œ”.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_ŠY“–Œ”.Location = New System.Drawing.Point(11, 5)
        Me.lb_ŠY“–Œ”.Name = "lb_ŠY“–Œ”"
        Me.lb_ŠY“–Œ”.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_ŠY“–Œ”.Size = New System.Drawing.Size(69, 17)
        Me.lb_ŠY“–Œ”.TabIndex = 7
        Me.lb_ŠY“–Œ”.Text = "ŠY“–Œ”"
        Me.lb_ŠY“–Œ”.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_€–Ú_3
        '
        Me._lb_€–Ú_3.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_€–Ú_3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_€–Ú_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_€–Ú_3.Font = New System.Drawing.Font("‚l‚r ‚oƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_€–Ú_3.ForeColor = System.Drawing.Color.White
        Me._lb_€–Ú_3.Location = New System.Drawing.Point(10, 44)
        Me._lb_€–Ú_3.Name = "_lb_€–Ú_3"
        Me._lb_€–Ú_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_€–Ú_3.Size = New System.Drawing.Size(99, 19)
        Me._lb_€–Ú_3.TabIndex = 6
        Me._lb_€–Ú_3.Text = "“¾ˆÓæ"
        Me._lb_€–Ú_3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'SelKokTmp_cls
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.CmdCan
        Me.ClientSize = New System.Drawing.Size(472, 432)
        Me.Controls.Add(Me.SelListVw)
        Me.Controls.Add(Me.CmdOk)
        Me.Controls.Add(Me.CmdCan)
        Me.Controls.Add(Me.CmdFind)
        Me.Controls.Add(Me.tx_ŒŸõ–¼Ì)
        Me.Controls.Add(Me.tx_“¾ˆÓæCD)
        Me.Controls.Add(Me.rf_“¾ˆÓæ–¼)
        Me.Controls.Add(Me._lb_€–Ú_2)
        Me.Controls.Add(Me.lbGuide)
        Me.Controls.Add(Me.lbListCount)
        Me.Controls.Add(Me.lb_ŠY“–Œ”)
        Me.Controls.Add(Me._lb_€–Ú_3)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "SelKokTmp_cls"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ŒÚ‹qƒeƒ“ƒvƒŒ[ƒg‘I‘ğ"
        Me.ResumeLayout(False)

    End Sub
    'Public ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents SelListVw As SortableListView
    Public WithEvents CmdOk As System.Windows.Forms.Button
    Public WithEvents CmdCan As System.Windows.Forms.Button
    Public WithEvents CmdFind As System.Windows.Forms.Button
    Public WithEvents tx_ŒŸõ–¼Ì As ExText.ExTextBox
    Public WithEvents tx_“¾ˆÓæCD As ExText.ExTextBox
    Public WithEvents rf_“¾ˆÓæ–¼ As System.Windows.Forms.Label
    Public WithEvents _lb_€–Ú_2 As System.Windows.Forms.Label
    Public WithEvents lbGuide As System.Windows.Forms.Label
    Public WithEvents lbListCount As System.Windows.Forms.Label
    Public WithEvents lb_ŠY“–Œ” As System.Windows.Forms.Label
    Public WithEvents _lb_€–Ú_3 As System.Windows.Forms.Label
End Class