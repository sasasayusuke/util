<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class BukkenSen_cls
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(BukkenSen_cls))
        Me.CmdOk = New System.Windows.Forms.Button()
        Me.SelListVw = New SortableListView()
        Me.CmdCan = New System.Windows.Forms.Button()
        Me.CmdFind = New System.Windows.Forms.Button()
        Me.tx_”[“üæCD = New ExText.ExTextBox()
        Me.tx_“¾ˆÓæCD = New ExText.ExTextBox()
        Me.tx_s•¨Œ“o˜^“úD = New ExDateText.ExDateTextBoxD()
        Me.tx_s•¨Œ“o˜^“úM = New ExDateText.ExDateTextBoxM()
        Me.tx_s•¨Œ“o˜^“úY = New ExDateText.ExDateTextBoxY()
        Me.tx_e•¨Œ“o˜^“úD = New ExDateText.ExDateTextBoxD()
        Me.tx_e•¨Œ“o˜^“úM = New ExDateText.ExDateTextBoxM()
        Me.tx_e•¨Œ“o˜^“úY = New ExDateText.ExDateTextBoxY()
        Me.tx_•¨Œ–¼ = New ExText.ExTextBox()
        Me.tx_’S“–ÒCD = New ExNmText.ExNmTextBox()
        Me.tx_s•¨Œ”Ô† = New ExText.ExTextBox()
        Me.tx_e•¨Œ”Ô† = New ExText.ExTextBox()
        Me._lb_€–Ú_3 = New System.Windows.Forms.Label()
        Me._lb_kara_2 = New System.Windows.Forms.Label()
        Me._lb_€–Ú_2 = New System.Windows.Forms.Label()
        Me._lb_€–Ú_7 = New System.Windows.Forms.Label()
        Me._lb_€–Ú_6 = New System.Windows.Forms.Label()
        Me._lb_€–Ú_4 = New System.Windows.Forms.Label()
        Me._lb_€–Ú_5 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lb_“ú = New System.Windows.Forms.Label()
        Me.lb_Œ = New System.Windows.Forms.Label()
        Me.lb_”N = New System.Windows.Forms.Label()
        Me._lb_”„ã“ú_0 = New System.Windows.Forms.Label()
        Me._lb_kara_0 = New System.Windows.Forms.Label()
        Me.lbListCount = New System.Windows.Forms.Label()
        Me.lb_ŠY“–Œ” = New System.Windows.Forms.Label()
        Me._lb_”„ã“ú_1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'CmdOk
        '
        Me.CmdOk.BackColor = System.Drawing.SystemColors.Control
        Me.CmdOk.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmdOk.Font = New System.Drawing.Font("‚l‚r ƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CmdOk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdOk.Location = New System.Drawing.Point(540, 512)
        Me.CmdOk.Name = "CmdOk"
        Me.CmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdOk.Size = New System.Drawing.Size(92, 25)
        Me.CmdOk.TabIndex = 29
        Me.CmdOk.Text = "‚n‚j(&O)"
        Me.CmdOk.UseVisualStyleBackColor = False
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
        Me.SelListVw.Location = New System.Drawing.Point(4, 174)
        Me.SelListVw.Name = "SelListVw"
        Me.SelListVw.Size = New System.Drawing.Size(731, 332)
        Me.SelListVw.SortStyle = SortableListView.SortStyles.SortDefault
        Me.SelListVw.TabIndex = 13
        Me.SelListVw.UseCompatibleStateImageBehavior = False
        Me.SelListVw.View = System.Windows.Forms.View.Details
        '
        'CmdCan
        '
        Me.CmdCan.BackColor = System.Drawing.SystemColors.Control
        Me.CmdCan.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmdCan.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CmdCan.Font = New System.Drawing.Font("‚l‚r ƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CmdCan.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdCan.Location = New System.Drawing.Point(642, 512)
        Me.CmdCan.Name = "CmdCan"
        Me.CmdCan.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdCan.Size = New System.Drawing.Size(92, 25)
        Me.CmdCan.TabIndex = 30
        Me.CmdCan.Text = "·¬İ¾Ù(&C)"
        Me.CmdCan.UseVisualStyleBackColor = False
        '
        'CmdFind
        '
        Me.CmdFind.BackColor = System.Drawing.SystemColors.Control
        Me.CmdFind.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmdFind.Font = New System.Drawing.Font("‚l‚r ƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CmdFind.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdFind.Location = New System.Drawing.Point(440, 120)
        Me.CmdFind.Name = "CmdFind"
        Me.CmdFind.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdFind.Size = New System.Drawing.Size(83, 25)
        Me.CmdFind.TabIndex = 12
        Me.CmdFind.Text = "ŒŸõ(&F)"
        Me.CmdFind.UseVisualStyleBackColor = False
        '
        'tx_”[“üæCD
        '
        Me.tx_”[“üæCD.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_”[“üæCD.BorderStyle = ExText.ExTextBox.BorderStyleType.Àü
        Me.tx_”[“üæCD.CanForwardSetFocus = True
        Me.tx_”[“üæCD.CanNextSetFocus = True
        Me.tx_”[“üæCD.CharacterSize = ExText.ExTextBox.CharSize.‚È‚µ
        Me.tx_”[“üæCD.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_”[“üæCD.EditMode = False
        Me.tx_”[“üæCD.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_”[“üæCD.IMEMode = System.Windows.Forms.ImeMode.Disable
        Me.tx_”[“üæCD.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_”[“üæCD.Location = New System.Drawing.Point(125, 51)
        Me.tx_”[“üæCD.MaxLength = 4
        Me.tx_”[“üæCD.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_”[“üæCD.Name = "tx_”[“üæCD"
        Me.tx_”[“üæCD.OldValue = "1234"
        Me.tx_”[“üæCD.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_”[“üæCD.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_”[“üæCD.SelectText = False
        Me.tx_”[“üæCD.SelLength = 0
        Me.tx_”[“üæCD.SelStart = 0
        Me.tx_”[“üæCD.SelText = ""
        Me.tx_”[“üæCD.Size = New System.Drawing.Size(43, 22)
        Me.tx_”[“üæCD.TabIndex = 1
        Me.tx_”[“üæCD.Text = "1234"
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
        Me.tx_“¾ˆÓæCD.Location = New System.Drawing.Point(125, 28)
        Me.tx_“¾ˆÓæCD.MaxLength = 4
        Me.tx_“¾ˆÓæCD.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_“¾ˆÓæCD.Name = "tx_“¾ˆÓæCD"
        Me.tx_“¾ˆÓæCD.OldValue = "1234567"
        Me.tx_“¾ˆÓæCD.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_“¾ˆÓæCD.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_“¾ˆÓæCD.SelectText = True
        Me.tx_“¾ˆÓæCD.SelLength = 0
        Me.tx_“¾ˆÓæCD.SelStart = 0
        Me.tx_“¾ˆÓæCD.SelText = ""
        Me.tx_“¾ˆÓæCD.Size = New System.Drawing.Size(64, 22)
        Me.tx_“¾ˆÓæCD.TabIndex = 0
        Me.tx_“¾ˆÓæCD.Text = "1234567"
        '
        'tx_s•¨Œ“o˜^“úD
        '
        Me.tx_s•¨Œ“o˜^“úD.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_s•¨Œ“o˜^“úD.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.‚È‚µ
        Me.tx_s•¨Œ“o˜^“úD.CanForwardSetFocus = True
        Me.tx_s•¨Œ“o˜^“úD.CanNextSetFocus = True
        Me.tx_s•¨Œ“o˜^“úD.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_s•¨Œ“o˜^“úD.EditMode = True
        Me.tx_s•¨Œ“o˜^“úD.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_s•¨Œ“o˜^“úD.Location = New System.Drawing.Point(229, 146)
        Me.tx_s•¨Œ“o˜^“úD.MaxLength = 2
        Me.tx_s•¨Œ“o˜^“úD.Name = "tx_s•¨Œ“o˜^“úD"
        Me.tx_s•¨Œ“o˜^“úD.OldValue = "88"
        Me.tx_s•¨Œ“o˜^“úD.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_s•¨Œ“o˜^“úD.SelectText = True
        Me.tx_s•¨Œ“o˜^“úD.SelLength = 0
        Me.tx_s•¨Œ“o˜^“úD.SelStart = 0
        Me.tx_s•¨Œ“o˜^“úD.SelText = ""
        Me.tx_s•¨Œ“o˜^“úD.Size = New System.Drawing.Size(22, 16)
        Me.tx_s•¨Œ“o˜^“úD.TabIndex = 8
        Me.tx_s•¨Œ“o˜^“úD.Text = "88"
        '
        'tx_s•¨Œ“o˜^“úM
        '
        Me.tx_s•¨Œ“o˜^“úM.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_s•¨Œ“o˜^“úM.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.‚È‚µ
        Me.tx_s•¨Œ“o˜^“úM.CanForwardSetFocus = True
        Me.tx_s•¨Œ“o˜^“úM.CanNextSetFocus = True
        Me.tx_s•¨Œ“o˜^“úM.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_s•¨Œ“o˜^“úM.EditMode = True
        Me.tx_s•¨Œ“o˜^“úM.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_s•¨Œ“o˜^“úM.Location = New System.Drawing.Point(184, 146)
        Me.tx_s•¨Œ“o˜^“úM.MaxLength = 2
        Me.tx_s•¨Œ“o˜^“úM.Name = "tx_s•¨Œ“o˜^“úM"
        Me.tx_s•¨Œ“o˜^“úM.OldValue = "88"
        Me.tx_s•¨Œ“o˜^“úM.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_s•¨Œ“o˜^“úM.SelectText = True
        Me.tx_s•¨Œ“o˜^“úM.SelLength = 0
        Me.tx_s•¨Œ“o˜^“úM.SelStart = 0
        Me.tx_s•¨Œ“o˜^“úM.SelText = ""
        Me.tx_s•¨Œ“o˜^“úM.Size = New System.Drawing.Size(26, 16)
        Me.tx_s•¨Œ“o˜^“úM.TabIndex = 7
        Me.tx_s•¨Œ“o˜^“úM.Text = "88"
        '
        'tx_s•¨Œ“o˜^“úY
        '
        Me.tx_s•¨Œ“o˜^“úY.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_s•¨Œ“o˜^“úY.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.‚È‚µ
        Me.tx_s•¨Œ“o˜^“úY.CanForwardSetFocus = True
        Me.tx_s•¨Œ“o˜^“úY.CanNextSetFocus = True
        Me.tx_s•¨Œ“o˜^“úY.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_s•¨Œ“o˜^“úY.EditMode = True
        Me.tx_s•¨Œ“o˜^“úY.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_s•¨Œ“o˜^“úY.Location = New System.Drawing.Point(126, 146)
        Me.tx_s•¨Œ“o˜^“úY.MaxLength = 4
        Me.tx_s•¨Œ“o˜^“úY.Name = "tx_s•¨Œ“o˜^“úY"
        Me.tx_s•¨Œ“o˜^“úY.OldValue = "8888"
        Me.tx_s•¨Œ“o˜^“úY.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_s•¨Œ“o˜^“úY.SelectText = True
        Me.tx_s•¨Œ“o˜^“úY.SelLength = 0
        Me.tx_s•¨Œ“o˜^“úY.SelStart = 0
        Me.tx_s•¨Œ“o˜^“úY.SelText = ""
        Me.tx_s•¨Œ“o˜^“úY.Size = New System.Drawing.Size(40, 16)
        Me.tx_s•¨Œ“o˜^“úY.TabIndex = 6
        Me.tx_s•¨Œ“o˜^“úY.Text = "8888"
        '
        'tx_e•¨Œ“o˜^“úD
        '
        Me.tx_e•¨Œ“o˜^“úD.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_e•¨Œ“o˜^“úD.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.‚È‚µ
        Me.tx_e•¨Œ“o˜^“úD.CanForwardSetFocus = True
        Me.tx_e•¨Œ“o˜^“úD.CanNextSetFocus = True
        Me.tx_e•¨Œ“o˜^“úD.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_e•¨Œ“o˜^“úD.EditMode = True
        Me.tx_e•¨Œ“o˜^“úD.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_e•¨Œ“o˜^“úD.Location = New System.Drawing.Point(403, 146)
        Me.tx_e•¨Œ“o˜^“úD.MaxLength = 2
        Me.tx_e•¨Œ“o˜^“úD.Name = "tx_e•¨Œ“o˜^“úD"
        Me.tx_e•¨Œ“o˜^“úD.OldValue = "88"
        Me.tx_e•¨Œ“o˜^“úD.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_e•¨Œ“o˜^“úD.SelectText = True
        Me.tx_e•¨Œ“o˜^“úD.SelLength = 0
        Me.tx_e•¨Œ“o˜^“úD.SelStart = 0
        Me.tx_e•¨Œ“o˜^“úD.SelText = ""
        Me.tx_e•¨Œ“o˜^“úD.Size = New System.Drawing.Size(22, 16)
        Me.tx_e•¨Œ“o˜^“úD.TabIndex = 11
        Me.tx_e•¨Œ“o˜^“úD.Text = "88"
        '
        'tx_e•¨Œ“o˜^“úM
        '
        Me.tx_e•¨Œ“o˜^“úM.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_e•¨Œ“o˜^“úM.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.‚È‚µ
        Me.tx_e•¨Œ“o˜^“úM.CanForwardSetFocus = True
        Me.tx_e•¨Œ“o˜^“úM.CanNextSetFocus = True
        Me.tx_e•¨Œ“o˜^“úM.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_e•¨Œ“o˜^“úM.EditMode = True
        Me.tx_e•¨Œ“o˜^“úM.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_e•¨Œ“o˜^“úM.Location = New System.Drawing.Point(357, 146)
        Me.tx_e•¨Œ“o˜^“úM.MaxLength = 2
        Me.tx_e•¨Œ“o˜^“úM.Name = "tx_e•¨Œ“o˜^“úM"
        Me.tx_e•¨Œ“o˜^“úM.OldValue = "88"
        Me.tx_e•¨Œ“o˜^“úM.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_e•¨Œ“o˜^“úM.SelectText = True
        Me.tx_e•¨Œ“o˜^“úM.SelLength = 0
        Me.tx_e•¨Œ“o˜^“úM.SelStart = 0
        Me.tx_e•¨Œ“o˜^“úM.SelText = ""
        Me.tx_e•¨Œ“o˜^“úM.Size = New System.Drawing.Size(26, 16)
        Me.tx_e•¨Œ“o˜^“úM.TabIndex = 10
        Me.tx_e•¨Œ“o˜^“úM.Text = "88"
        '
        'tx_e•¨Œ“o˜^“úY
        '
        Me.tx_e•¨Œ“o˜^“úY.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_e•¨Œ“o˜^“úY.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.‚È‚µ
        Me.tx_e•¨Œ“o˜^“úY.CanForwardSetFocus = True
        Me.tx_e•¨Œ“o˜^“úY.CanNextSetFocus = True
        Me.tx_e•¨Œ“o˜^“úY.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_e•¨Œ“o˜^“úY.EditMode = True
        Me.tx_e•¨Œ“o˜^“úY.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_e•¨Œ“o˜^“úY.Location = New System.Drawing.Point(303, 146)
        Me.tx_e•¨Œ“o˜^“úY.MaxLength = 4
        Me.tx_e•¨Œ“o˜^“úY.Name = "tx_e•¨Œ“o˜^“úY"
        Me.tx_e•¨Œ“o˜^“úY.OldValue = "8888"
        Me.tx_e•¨Œ“o˜^“úY.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_e•¨Œ“o˜^“úY.SelectText = True
        Me.tx_e•¨Œ“o˜^“úY.SelLength = 0
        Me.tx_e•¨Œ“o˜^“úY.SelStart = 0
        Me.tx_e•¨Œ“o˜^“úY.SelText = ""
        Me.tx_e•¨Œ“o˜^“úY.Size = New System.Drawing.Size(40, 16)
        Me.tx_e•¨Œ“o˜^“úY.TabIndex = 9
        Me.tx_e•¨Œ“o˜^“úY.Text = "8888"
        '
        'tx_•¨Œ–¼
        '
        Me.tx_•¨Œ–¼.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_•¨Œ–¼.BorderStyle = ExText.ExTextBox.BorderStyleType.Àü
        Me.tx_•¨Œ–¼.CanForwardSetFocus = True
        Me.tx_•¨Œ–¼.CanNextSetFocus = True
        Me.tx_•¨Œ–¼.CharacterSize = ExText.ExTextBox.CharSize.‚È‚µ
        Me.tx_•¨Œ–¼.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_•¨Œ–¼.EditMode = True
        Me.tx_•¨Œ–¼.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_•¨Œ–¼.IMEMode = System.Windows.Forms.ImeMode.Hiragana
        Me.tx_•¨Œ–¼.LengthType = ExText.ExTextBox.LenType.ByteType
        Me.tx_•¨Œ–¼.Location = New System.Drawing.Point(125, 122)
        Me.tx_•¨Œ–¼.MaxLength = 0
        Me.tx_•¨Œ–¼.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_•¨Œ–¼.Name = "tx_•¨Œ–¼"
        Me.tx_•¨Œ–¼.OldValue = "‚ ‚¢‚¤‚¦‚¨‚ ‚¢‚¤‚¦‚¨‚ ‚¢‚¤‚¦‚¨‚ ‚¢‚¤‚¦‚¨"
        Me.tx_•¨Œ–¼.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_•¨Œ–¼.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_•¨Œ–¼.SelectText = True
        Me.tx_•¨Œ–¼.SelLength = 0
        Me.tx_•¨Œ–¼.SelStart = 0
        Me.tx_•¨Œ–¼.SelText = ""
        Me.tx_•¨Œ–¼.Size = New System.Drawing.Size(288, 22)
        Me.tx_•¨Œ–¼.TabIndex = 5
        Me.tx_•¨Œ–¼.Text = "‚ ‚¢‚¤‚¦‚¨‚ ‚¢‚¤‚¦‚¨‚ ‚¢‚¤‚¦‚¨‚ ‚¢‚¤‚¦‚¨"
        '
        'tx_’S“–ÒCD
        '
        Me.tx_’S“–ÒCD.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_’S“–ÒCD.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.Àü
        Me.tx_’S“–ÒCD.CanForwardSetFocus = True
        Me.tx_’S“–ÒCD.CanNextSetFocus = True
        Me.tx_’S“–ÒCD.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_’S“–ÒCD.DecimalPlace = CType(0, Short)
        Me.tx_’S“–ÒCD.EditMode = False
        Me.tx_’S“–ÒCD.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_’S“–ÒCD.FormatType = ""
        Me.tx_’S“–ÒCD.FormatTypeNega = ""
        Me.tx_’S“–ÒCD.FormatTypeNull = ""
        Me.tx_’S“–ÒCD.FormatTypeZero = ""
        Me.tx_’S“–ÒCD.InputMinus = False
        Me.tx_’S“–ÒCD.InputPlus = True
        Me.tx_’S“–ÒCD.InputZero = False
        Me.tx_’S“–ÒCD.Location = New System.Drawing.Point(125, 75)
        Me.tx_’S“–ÒCD.MaxLength = 2
        Me.tx_’S“–ÒCD.Name = "tx_’S“–ÒCD"
        Me.tx_’S“–ÒCD.OldValue = ""
        Me.tx_’S“–ÒCD.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_’S“–ÒCD.SelectText = True
        Me.tx_’S“–ÒCD.SelLength = 0
        Me.tx_’S“–ÒCD.SelStart = 0
        Me.tx_’S“–ÒCD.SelText = ""
        Me.tx_’S“–ÒCD.Size = New System.Drawing.Size(43, 22)
        Me.tx_’S“–ÒCD.TabIndex = 2
        '
        'tx_s•¨Œ”Ô†
        '
        Me.tx_s•¨Œ”Ô†.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_s•¨Œ”Ô†.BorderStyle = ExText.ExTextBox.BorderStyleType.Àü
        Me.tx_s•¨Œ”Ô†.CanForwardSetFocus = True
        Me.tx_s•¨Œ”Ô†.CanNextSetFocus = True
        Me.tx_s•¨Œ”Ô†.CharacterSize = ExText.ExTextBox.CharSize.‚È‚µ
        Me.tx_s•¨Œ”Ô†.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_s•¨Œ”Ô†.EditMode = False
        Me.tx_s•¨Œ”Ô†.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_s•¨Œ”Ô†.IMEMode = System.Windows.Forms.ImeMode.Disable
        Me.tx_s•¨Œ”Ô†.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_s•¨Œ”Ô†.Location = New System.Drawing.Point(125, 99)
        Me.tx_s•¨Œ”Ô†.MaxLength = 7
        Me.tx_s•¨Œ”Ô†.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_s•¨Œ”Ô†.Name = "tx_s•¨Œ”Ô†"
        Me.tx_s•¨Œ”Ô†.OldValue = "1234567"
        Me.tx_s•¨Œ”Ô†.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_s•¨Œ”Ô†.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_s•¨Œ”Ô†.SelectText = False
        Me.tx_s•¨Œ”Ô†.SelLength = 0
        Me.tx_s•¨Œ”Ô†.SelStart = 0
        Me.tx_s•¨Œ”Ô†.SelText = ""
        Me.tx_s•¨Œ”Ô†.Size = New System.Drawing.Size(64, 22)
        Me.tx_s•¨Œ”Ô†.TabIndex = 3
        Me.tx_s•¨Œ”Ô†.Text = "1234567"
        '
        'tx_e•¨Œ”Ô†
        '
        Me.tx_e•¨Œ”Ô†.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_e•¨Œ”Ô†.BorderStyle = ExText.ExTextBox.BorderStyleType.Àü
        Me.tx_e•¨Œ”Ô†.CanForwardSetFocus = True
        Me.tx_e•¨Œ”Ô†.CanNextSetFocus = True
        Me.tx_e•¨Œ”Ô†.CharacterSize = ExText.ExTextBox.CharSize.‚È‚µ
        Me.tx_e•¨Œ”Ô†.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_e•¨Œ”Ô†.EditMode = False
        Me.tx_e•¨Œ”Ô†.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_e•¨Œ”Ô†.IMEMode = System.Windows.Forms.ImeMode.Disable
        Me.tx_e•¨Œ”Ô†.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_e•¨Œ”Ô†.Location = New System.Drawing.Point(270, 99)
        Me.tx_e•¨Œ”Ô†.MaxLength = 7
        Me.tx_e•¨Œ”Ô†.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_e•¨Œ”Ô†.Name = "tx_e•¨Œ”Ô†"
        Me.tx_e•¨Œ”Ô†.OldValue = "1234567"
        Me.tx_e•¨Œ”Ô†.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_e•¨Œ”Ô†.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_e•¨Œ”Ô†.SelectText = False
        Me.tx_e•¨Œ”Ô†.SelLength = 0
        Me.tx_e•¨Œ”Ô†.SelStart = 0
        Me.tx_e•¨Œ”Ô†.SelText = ""
        Me.tx_e•¨Œ”Ô†.Size = New System.Drawing.Size(64, 22)
        Me.tx_e•¨Œ”Ô†.TabIndex = 4
        Me.tx_e•¨Œ”Ô†.Text = "1234567"
        '
        '_lb_€–Ú_3
        '
        Me._lb_€–Ú_3.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_€–Ú_3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_€–Ú_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_€–Ú_3.Font = New System.Drawing.Font("‚l‚r ‚oƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_€–Ú_3.ForeColor = System.Drawing.Color.White
        Me._lb_€–Ú_3.Location = New System.Drawing.Point(4, 99)
        Me._lb_€–Ú_3.Name = "_lb_€–Ú_3"
        Me._lb_€–Ú_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_€–Ú_3.Size = New System.Drawing.Size(121, 22)
        Me._lb_€–Ú_3.TabIndex = 33
        Me._lb_€–Ú_3.Text = "•¨Œ‡‚"
        Me._lb_€–Ú_3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        '_lb_kara_2
        '
        Me._lb_kara_2.BackColor = System.Drawing.SystemColors.Control
        Me._lb_kara_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_kara_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_kara_2.Location = New System.Drawing.Point(252, 101)
        Me._lb_kara_2.Name = "_lb_kara_2"
        Me._lb_kara_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_kara_2.Size = New System.Drawing.Size(17, 12)
        Me._lb_kara_2.TabIndex = 32
        Me._lb_kara_2.Text = "`"
        Me._lb_kara_2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_€–Ú_2
        '
        Me._lb_€–Ú_2.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_€–Ú_2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_€–Ú_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_€–Ú_2.Font = New System.Drawing.Font("‚l‚r ‚oƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_€–Ú_2.ForeColor = System.Drawing.Color.White
        Me._lb_€–Ú_2.Location = New System.Drawing.Point(4, 75)
        Me._lb_€–Ú_2.Name = "_lb_€–Ú_2"
        Me._lb_€–Ú_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_€–Ú_2.Size = New System.Drawing.Size(121, 22)
        Me._lb_€–Ú_2.TabIndex = 31
        Me._lb_€–Ú_2.Text = "’S“–ÒCD"
        Me._lb_€–Ú_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        '_lb_€–Ú_7
        '
        Me._lb_€–Ú_7.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_€–Ú_7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_€–Ú_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_€–Ú_7.Font = New System.Drawing.Font("‚l‚r ‚oƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_€–Ú_7.ForeColor = System.Drawing.Color.White
        Me._lb_€–Ú_7.Location = New System.Drawing.Point(4, 146)
        Me._lb_€–Ú_7.Name = "_lb_€–Ú_7"
        Me._lb_€–Ú_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_€–Ú_7.Size = New System.Drawing.Size(121, 22)
        Me._lb_€–Ú_7.TabIndex = 27
        Me._lb_€–Ú_7.Text = "•¨Œ“o˜^“ú"
        Me._lb_€–Ú_7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        '_lb_€–Ú_6
        '
        Me._lb_€–Ú_6.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_€–Ú_6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_€–Ú_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_€–Ú_6.Font = New System.Drawing.Font("‚l‚r ‚oƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_€–Ú_6.ForeColor = System.Drawing.Color.White
        Me._lb_€–Ú_6.Location = New System.Drawing.Point(4, 28)
        Me._lb_€–Ú_6.Name = "_lb_€–Ú_6"
        Me._lb_€–Ú_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_€–Ú_6.Size = New System.Drawing.Size(121, 22)
        Me._lb_€–Ú_6.TabIndex = 26
        Me._lb_€–Ú_6.Text = "“¾ˆÓæCD"
        Me._lb_€–Ú_6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        '_lb_€–Ú_4
        '
        Me._lb_€–Ú_4.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_€–Ú_4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_€–Ú_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_€–Ú_4.Font = New System.Drawing.Font("‚l‚r ‚oƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_€–Ú_4.ForeColor = System.Drawing.Color.White
        Me._lb_€–Ú_4.Location = New System.Drawing.Point(4, 51)
        Me._lb_€–Ú_4.Name = "_lb_€–Ú_4"
        Me._lb_€–Ú_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_€–Ú_4.Size = New System.Drawing.Size(121, 22)
        Me._lb_€–Ú_4.TabIndex = 25
        Me._lb_€–Ú_4.Text = "”[“üæCD"
        Me._lb_€–Ú_4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        '_lb_€–Ú_5
        '
        Me._lb_€–Ú_5.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_€–Ú_5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_€–Ú_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_€–Ú_5.Font = New System.Drawing.Font("‚l‚r ‚oƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_€–Ú_5.ForeColor = System.Drawing.Color.White
        Me._lb_€–Ú_5.Location = New System.Drawing.Point(4, 122)
        Me._lb_€–Ú_5.Name = "_lb_€–Ú_5"
        Me._lb_€–Ú_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_€–Ú_5.Size = New System.Drawing.Size(121, 22)
        Me._lb_€–Ú_5.TabIndex = 24
        Me._lb_€–Ú_5.Text = "•¨Œ–¼"
        Me._lb_€–Ú_5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Window
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("‚l‚r ƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(432, 147)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(14, 14)
        Me.Label1.TabIndex = 23
        Me.Label1.Text = "“ú"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Window
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("‚l‚r ƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(387, 147)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(14, 14)
        Me.Label2.TabIndex = 22
        Me.Label2.Text = "Œ"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.SystemColors.Window
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("‚l‚r ƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(345, 147)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(14, 14)
        Me.Label3.TabIndex = 21
        Me.Label3.Text = "”N"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_“ú
        '
        Me.lb_“ú.BackColor = System.Drawing.SystemColors.Window
        Me.lb_“ú.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_“ú.Font = New System.Drawing.Font("‚l‚r ƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_“ú.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_“ú.Location = New System.Drawing.Point(257, 147)
        Me.lb_“ú.Name = "lb_“ú"
        Me.lb_“ú.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_“ú.Size = New System.Drawing.Size(14, 14)
        Me.lb_“ú.TabIndex = 20
        Me.lb_“ú.Text = "“ú"
        Me.lb_“ú.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_Œ
        '
        Me.lb_Œ.BackColor = System.Drawing.SystemColors.Window
        Me.lb_Œ.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_Œ.Font = New System.Drawing.Font("‚l‚r ƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_Œ.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_Œ.Location = New System.Drawing.Point(210, 147)
        Me.lb_Œ.Name = "lb_Œ"
        Me.lb_Œ.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_Œ.Size = New System.Drawing.Size(14, 14)
        Me.lb_Œ.TabIndex = 19
        Me.lb_Œ.Text = "Œ"
        Me.lb_Œ.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_”N
        '
        Me.lb_”N.BackColor = System.Drawing.SystemColors.Window
        Me.lb_”N.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_”N.Font = New System.Drawing.Font("‚l‚r ƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_”N.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_”N.Location = New System.Drawing.Point(171, 147)
        Me.lb_”N.Name = "lb_”N"
        Me.lb_”N.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_”N.Size = New System.Drawing.Size(14, 14)
        Me.lb_”N.TabIndex = 18
        Me.lb_”N.Text = "”N"
        Me.lb_”N.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_”„ã“ú_0
        '
        Me._lb_”„ã“ú_0.BackColor = System.Drawing.SystemColors.Window
        Me._lb_”„ã“ú_0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_”„ã“ú_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_”„ã“ú_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_”„ã“ú_0.Location = New System.Drawing.Point(125, 144)
        Me._lb_”„ã“ú_0.Name = "_lb_”„ã“ú_0"
        Me._lb_”„ã“ú_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_”„ã“ú_0.Size = New System.Drawing.Size(150, 22)
        Me._lb_”„ã“ú_0.TabIndex = 17
        '
        '_lb_kara_0
        '
        Me._lb_kara_0.BackColor = System.Drawing.SystemColors.Control
        Me._lb_kara_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_kara_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_kara_0.Location = New System.Drawing.Point(277, 149)
        Me._lb_kara_0.Name = "_lb_kara_0"
        Me._lb_kara_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_kara_0.Size = New System.Drawing.Size(20, 12)
        Me._lb_kara_0.TabIndex = 16
        Me._lb_kara_0.Text = "`"
        Me._lb_kara_0.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lbListCount
        '
        Me.lbListCount.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.lbListCount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lbListCount.Font = New System.Drawing.Font("‚l‚r ‚oƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lbListCount.ForeColor = System.Drawing.Color.Yellow
        Me.lbListCount.Location = New System.Drawing.Point(83, 5)
        Me.lbListCount.Name = "lbListCount"
        Me.lbListCount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lbListCount.Size = New System.Drawing.Size(89, 17)
        Me.lbListCount.TabIndex = 15
        Me.lbListCount.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lb_ŠY“–Œ”
        '
        Me.lb_ŠY“–Œ”.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.lb_ŠY“–Œ”.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_ŠY“–Œ”.Font = New System.Drawing.Font("‚l‚r ‚oƒSƒVƒbƒN", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_ŠY“–Œ”.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_ŠY“–Œ”.Location = New System.Drawing.Point(3, 5)
        Me.lb_ŠY“–Œ”.Name = "lb_ŠY“–Œ”"
        Me.lb_ŠY“–Œ”.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_ŠY“–Œ”.Size = New System.Drawing.Size(80, 17)
        Me.lb_ŠY“–Œ”.TabIndex = 14
        Me.lb_ŠY“–Œ”.Text = "ŠY“–Œ”"
        Me.lb_ŠY“–Œ”.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_”„ã“ú_1
        '
        Me._lb_”„ã“ú_1.BackColor = System.Drawing.SystemColors.Window
        Me._lb_”„ã“ú_1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_”„ã“ú_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_”„ã“ú_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_”„ã“ú_1.Location = New System.Drawing.Point(299, 144)
        Me._lb_”„ã“ú_1.Name = "_lb_”„ã“ú_1"
        Me._lb_”„ã“ú_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_”„ã“ú_1.Size = New System.Drawing.Size(150, 22)
        Me._lb_”„ã“ú_1.TabIndex = 28
        '
        'BukkenSen_cls
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.CmdCan
        Me.ClientSize = New System.Drawing.Size(741, 542)
        Me.Controls.Add(Me.CmdCan)
        Me.Controls.Add(Me.CmdOk)
        Me.Controls.Add(Me.SelListVw)
        Me.Controls.Add(Me.CmdFind)
        Me.Controls.Add(Me.tx_”[“üæCD)
        Me.Controls.Add(Me.tx_“¾ˆÓæCD)
        Me.Controls.Add(Me.tx_s•¨Œ“o˜^“úD)
        Me.Controls.Add(Me.tx_s•¨Œ“o˜^“úM)
        Me.Controls.Add(Me.tx_s•¨Œ“o˜^“úY)
        Me.Controls.Add(Me.tx_e•¨Œ“o˜^“úD)
        Me.Controls.Add(Me.tx_e•¨Œ“o˜^“úM)
        Me.Controls.Add(Me.tx_e•¨Œ“o˜^“úY)
        Me.Controls.Add(Me.tx_•¨Œ–¼)
        Me.Controls.Add(Me.tx_’S“–ÒCD)
        Me.Controls.Add(Me.tx_s•¨Œ”Ô†)
        Me.Controls.Add(Me.tx_e•¨Œ”Ô†)
        Me.Controls.Add(Me._lb_€–Ú_3)
        Me.Controls.Add(Me._lb_kara_2)
        Me.Controls.Add(Me._lb_€–Ú_2)
        Me.Controls.Add(Me._lb_€–Ú_7)
        Me.Controls.Add(Me._lb_€–Ú_6)
        Me.Controls.Add(Me._lb_€–Ú_4)
        Me.Controls.Add(Me._lb_€–Ú_5)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.lb_“ú)
        Me.Controls.Add(Me.lb_Œ)
        Me.Controls.Add(Me.lb_”N)
        Me.Controls.Add(Me._lb_”„ã“ú_0)
        Me.Controls.Add(Me._lb_kara_0)
        Me.Controls.Add(Me.lbListCount)
        Me.Controls.Add(Me.lb_ŠY“–Œ”)
        Me.Controls.Add(Me._lb_”„ã“ú_1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "BukkenSen_cls"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "•¨Œî•ñ‘I‘ğ"
        Me.ResumeLayout(False)

    End Sub
    'Private components As System.ComponentModel.IContainer
    Public ToolTip1 As System.Windows.Forms.ToolTip
    Public WithEvents CmdCan As System.Windows.Forms.Button
    Public WithEvents CmdOk As System.Windows.Forms.Button
    Public WithEvents SelListVw As SortableListView
    Public WithEvents CmdFind As System.Windows.Forms.Button
    Public WithEvents tx_”[“üæCD As ExText.ExTextBox
    Public WithEvents tx_“¾ˆÓæCD As ExText.ExTextBox
    Public WithEvents tx_s•¨Œ“o˜^“úD As ExDateText.ExDateTextBoxD
    Public WithEvents tx_s•¨Œ“o˜^“úM As ExDateText.ExDateTextBoxM
    Public WithEvents tx_s•¨Œ“o˜^“úY As ExDateText.ExDateTextBoxY
    Public WithEvents tx_e•¨Œ“o˜^“úD As ExDateText.ExDateTextBoxD
    Public WithEvents tx_e•¨Œ“o˜^“úM As ExDateText.ExDateTextBoxM
    Public WithEvents tx_e•¨Œ“o˜^“úY As ExDateText.ExDateTextBoxY
    Public WithEvents tx_•¨Œ–¼ As ExText.ExTextBox
    Public WithEvents tx_’S“–ÒCD As ExNmText.ExNmTextBox
    Public WithEvents tx_s•¨Œ”Ô† As ExText.ExTextBox
    Public WithEvents tx_e•¨Œ”Ô† As ExText.ExTextBox
    Public WithEvents _lb_€–Ú_3 As System.Windows.Forms.Label
    Public WithEvents _lb_kara_2 As System.Windows.Forms.Label
    Public WithEvents _lb_€–Ú_2 As System.Windows.Forms.Label
    Public WithEvents _lb_€–Ú_7 As System.Windows.Forms.Label
    Public WithEvents _lb_€–Ú_6 As System.Windows.Forms.Label
    Public WithEvents _lb_€–Ú_4 As System.Windows.Forms.Label
    Public WithEvents _lb_€–Ú_5 As System.Windows.Forms.Label
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents lb_“ú As System.Windows.Forms.Label
    Public WithEvents lb_Œ As System.Windows.Forms.Label
    Public WithEvents lb_”N As System.Windows.Forms.Label
    Public WithEvents _lb_”„ã“ú_0 As System.Windows.Forms.Label
    Public WithEvents _lb_kara_0 As System.Windows.Forms.Label
    Public WithEvents lbListCount As System.Windows.Forms.Label
    Public WithEvents lb_ŠY“–Œ” As System.Windows.Forms.Label
    Public WithEvents _lb_”„ã“ú_1 As System.Windows.Forms.Label
    Public WithEvents lb_kara As System.Windows.Forms.Label
    Public WithEvents lb_€–Ú As System.Windows.Forms.Label
    Public WithEvents lb_”„ã“ú As System.Windows.Forms.Label
End Class