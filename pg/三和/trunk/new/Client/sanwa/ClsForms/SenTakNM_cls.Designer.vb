<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SentakNM_cls
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

    'ƒƒ‚: ˆÈ‰º‚ÌƒvƒƒV[ƒWƒƒ‚Í Windows ƒtƒH[ƒ€ ƒfƒUƒCƒi‚Å•K—v‚Å‚·B
    'Windows ƒtƒH[ƒ€ ƒfƒUƒCƒi‚ğg‚Á‚Ä•ÏX‚Å‚«‚Ü‚·B
    'ƒR[ƒh ƒGƒfƒBƒ^‚ğg—p‚µ‚ÄA•ÏX‚µ‚È‚¢‚Å‚­‚¾‚³‚¢B
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SentakNM_cls))
        Me.tx_ŒŸõID = New ExText.ExTextBox()
        Me.SelListVw = New SortableListView()
        Me.CmdOk = New System.Windows.Forms.Button()
        Me.CmdCan = New System.Windows.Forms.Button()
        Me.CmdFind = New System.Windows.Forms.Button()
        Me.tx_ŒŸõ–¼ = New ExText.ExTextBox()
        Me.lbGuide = New System.Windows.Forms.Label()
        Me.lbListCount = New System.Windows.Forms.Label()
        Me.lb_ŠY“–Œ” = New System.Windows.Forms.Label()
        Me.lb_ŒŸõID = New System.Windows.Forms.Label()
        Me.lb_ŒŸõ–¼ = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'tx_ŒŸõID
        '
        Me.tx_ŒŸõID.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_ŒŸõID.BorderStyle = ExText.ExTextBox.BorderStyleType.Àü
        Me.tx_ŒŸõID.CanForwardSetFocus = True
        Me.tx_ŒŸõID.CanNextSetFocus = True
        Me.tx_ŒŸõID.CharacterSize = ExText.ExTextBox.CharSize.‚È‚µ
        Me.tx_ŒŸõID.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_ŒŸõID.EditMode = True
        Me.tx_ŒŸõID.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_ŒŸõID.IMEMode = System.Windows.Forms.ImeMode.Disable
        Me.tx_ŒŸõID.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_ŒŸõID.Location = New System.Drawing.Point(99, 45)
        Me.tx_ŒŸõID.MaxLength = 30
        Me.tx_ŒŸõID.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_ŒŸõID.Name = "tx_ŒŸõID"
        Me.tx_ŒŸõID.OldValue = "ExTextBox"
        Me.tx_ŒŸõID.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_ŒŸõID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_ŒŸõID.SelectText = True
        Me.tx_ŒŸõID.SelLength = 0
        Me.tx_ŒŸõID.SelStart = 0
        Me.tx_ŒŸõID.SelText = ""
        Me.tx_ŒŸõID.Size = New System.Drawing.Size(67, 22)
        Me.tx_ŒŸõID.TabIndex = 4
        Me.tx_ŒŸõID.Text = "ExTextBox"
        '
        'SelListVw
        '
        Me.SelListVw.BackColor = System.Drawing.Color.FromArgb(CType(CType(198, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.SelListVw.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.SelListVw.Font = New System.Drawing.Font("‚l‚r ƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.SelListVw.ForeColor = System.Drawing.SystemColors.WindowText
        Me.SelListVw.FullRowSelect = True
        Me.SelListVw.HideSelection = False
        Me.SelListVw.Location = New System.Drawing.Point(8, 95)
        Me.SelListVw.Name = "SelListVw"
        Me.SelListVw.Size = New System.Drawing.Size(467, 272)
        Me.SelListVw.SortStyle = SortableListView.SortStyles.SortDefault
        Me.SelListVw.TabIndex = 8
        Me.SelListVw.UseCompatibleStateImageBehavior = False
        Me.SelListVw.View = System.Windows.Forms.View.Details
        '
        'CmdOk
        '
        Me.CmdOk.BackColor = System.Drawing.SystemColors.Control
        Me.CmdOk.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmdOk.Font = New System.Drawing.Font("‚l‚r ƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CmdOk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdOk.Location = New System.Drawing.Point(284, 376)
        Me.CmdOk.Name = "CmdOk"
        Me.CmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdOk.Size = New System.Drawing.Size(92, 25)
        Me.CmdOk.TabIndex = 9
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
        Me.CmdCan.Location = New System.Drawing.Point(385, 376)
        Me.CmdCan.Name = "CmdCan"
        Me.CmdCan.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdCan.Size = New System.Drawing.Size(92, 24)
        Me.CmdCan.TabIndex = 10
        Me.CmdCan.Text = "·¬İ¾Ù(&C)"
        Me.CmdCan.UseVisualStyleBackColor = False
        '
        'CmdFind
        '
        Me.CmdFind.BackColor = System.Drawing.SystemColors.Control
        Me.CmdFind.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmdFind.Font = New System.Drawing.Font("‚l‚r ƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CmdFind.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdFind.Location = New System.Drawing.Point(383, 63)
        Me.CmdFind.Name = "CmdFind"
        Me.CmdFind.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdFind.Size = New System.Drawing.Size(83, 25)
        Me.CmdFind.TabIndex = 7
        Me.CmdFind.Text = "ŒŸõ(&F)"
        Me.CmdFind.UseVisualStyleBackColor = False
        '
        'tx_ŒŸõ–¼
        '
        Me.tx_ŒŸõ–¼.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_ŒŸõ–¼.BorderStyle = ExText.ExTextBox.BorderStyleType.Àü
        Me.tx_ŒŸõ–¼.CanForwardSetFocus = True
        Me.tx_ŒŸõ–¼.CanNextSetFocus = True
        Me.tx_ŒŸõ–¼.CharacterSize = ExText.ExTextBox.CharSize.‚È‚µ
        Me.tx_ŒŸõ–¼.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_ŒŸõ–¼.EditMode = True
        Me.tx_ŒŸõ–¼.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_ŒŸõ–¼.IMEMode = System.Windows.Forms.ImeMode.Hiragana
        Me.tx_ŒŸõ–¼.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_ŒŸõ–¼.Location = New System.Drawing.Point(99, 67)
        Me.tx_ŒŸõ–¼.MaxLength = 30
        Me.tx_ŒŸõ–¼.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_ŒŸõ–¼.Name = "tx_ŒŸõ–¼"
        Me.tx_ŒŸõ–¼.OldValue = "ExTextBox"
        Me.tx_ŒŸõ–¼.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_ŒŸõ–¼.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_ŒŸõ–¼.SelectText = True
        Me.tx_ŒŸõ–¼.SelLength = 0
        Me.tx_ŒŸõ–¼.SelStart = 0
        Me.tx_ŒŸõ–¼.SelText = ""
        Me.tx_ŒŸõ–¼.Size = New System.Drawing.Size(133, 22)
        Me.tx_ŒŸõ–¼.TabIndex = 6
        Me.tx_ŒŸõ–¼.Text = "ExTextBox"
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
        Me.lbGuide.Size = New System.Drawing.Size(313, 17)
        Me.lbGuide.TabIndex = 2
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
        Me.lbListCount.TabIndex = 1
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
        Me.lb_ŠY“–Œ”.TabIndex = 0
        Me.lb_ŠY“–Œ”.Text = "ŠY“–Œ”"
        Me.lb_ŠY“–Œ”.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_ŒŸõID
        '
        Me.lb_ŒŸõID.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.lb_ŒŸõID.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lb_ŒŸõID.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_ŒŸõID.Font = New System.Drawing.Font("‚l‚r ‚oƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_ŒŸõID.ForeColor = System.Drawing.Color.White
        Me.lb_ŒŸõID.Location = New System.Drawing.Point(10, 45)
        Me.lb_ŒŸõID.Name = "lb_ŒŸõID"
        Me.lb_ŒŸõID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_ŒŸõID.Size = New System.Drawing.Size(89, 20)
        Me.lb_ŒŸõID.TabIndex = 3
        Me.lb_ŒŸõID.Text = "ŒŸõID"
        Me.lb_ŒŸõID.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_ŒŸõ–¼
        '
        Me.lb_ŒŸõ–¼.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.lb_ŒŸõ–¼.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lb_ŒŸõ–¼.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_ŒŸõ–¼.Font = New System.Drawing.Font("‚l‚r ‚oƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_ŒŸõ–¼.ForeColor = System.Drawing.Color.White
        Me.lb_ŒŸõ–¼.Location = New System.Drawing.Point(10, 67)
        Me.lb_ŒŸõ–¼.Name = "lb_ŒŸõ–¼"
        Me.lb_ŒŸõ–¼.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_ŒŸõ–¼.Size = New System.Drawing.Size(89, 20)
        Me.lb_ŒŸõ–¼.TabIndex = 5
        Me.lb_ŒŸõ–¼.Text = "ŒŸõ–¼"
        Me.lb_ŒŸõ–¼.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'SentakNM_cls
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.CmdCan
        Me.ClientSize = New System.Drawing.Size(486, 407)
        Me.Controls.Add(Me.tx_ŒŸõID)
        Me.Controls.Add(Me.SelListVw)
        Me.Controls.Add(Me.CmdOk)
        Me.Controls.Add(Me.CmdCan)
        Me.Controls.Add(Me.CmdFind)
        Me.Controls.Add(Me.tx_ŒŸõ–¼)
        Me.Controls.Add(Me.lbGuide)
        Me.Controls.Add(Me.lbListCount)
        Me.Controls.Add(Me.lb_ŠY“–Œ”)
        Me.Controls.Add(Me.lb_ŒŸõID)
        Me.Controls.Add(Me.lb_ŒŸõ–¼)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("‚l‚r ƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(4, 30)
        Me.Name = "SentakNM_cls"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Form1"
        Me.ResumeLayout(False)

    End Sub
    'Private components As System.ComponentModel.IContainer
    'Public ToolTip1 As System.Windows.Forms.ToolTip
    Public WithEvents tx_ŒŸõID As ExText.ExTextBox
    Public WithEvents SelListVw As SortableListView
    Public WithEvents CmdOk As System.Windows.Forms.Button
    Public WithEvents CmdCan As System.Windows.Forms.Button
    Public WithEvents CmdFind As System.Windows.Forms.Button
    Public WithEvents tx_ŒŸõ–¼ As ExText.ExTextBox
    Public WithEvents lbGuide As System.Windows.Forms.Label
    Public WithEvents lbListCount As System.Windows.Forms.Label
    Public WithEvents lb_ŠY“–Œ” As System.Windows.Forms.Label
    Public WithEvents lb_ŒŸõID As System.Windows.Forms.Label
    Public WithEvents lb_ŒŸõ–¼ As System.Windows.Forms.Label
End Class