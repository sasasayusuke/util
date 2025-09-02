<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SeiSen_cls
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SeiSen_cls))
        Me.CmdOk = New System.Windows.Forms.Button()
        Me.SelListVw = New SortableListView()
        Me.CmdCan = New System.Windows.Forms.Button()
        Me.CmdFind = New System.Windows.Forms.Button()
        Me.tx_検索製品 = New ExText.ExTextBox()
        Me.tx_検索名称 = New ExText.ExTextBox()
        Me.tx_検索仕入先 = New ExText.ExTextBox()
        Me.tx_検索仕様 = New ExText.ExTextBox()
        Me.tx_W = New ExNmText.ExNmTextBox()
        Me.tx_D = New ExNmText.ExNmTextBox()
        Me.tx_H = New ExNmText.ExNmTextBox()
        Me.tx_D2 = New ExNmText.ExNmTextBox()
        Me.tx_D1 = New ExNmText.ExNmTextBox()
        Me.tx_H2 = New ExNmText.ExNmTextBox()
        Me.tx_H1 = New ExNmText.ExNmTextBox()
        Me._lb_項目_10 = New System.Windows.Forms.Label()
        Me._lb_項目_9 = New System.Windows.Forms.Label()
        Me._lb_項目_7 = New System.Windows.Forms.Label()
        Me._lb_項目_8 = New System.Windows.Forms.Label()
        Me._lb_項目_6 = New System.Windows.Forms.Label()
        Me._lb_項目_5 = New System.Windows.Forms.Label()
        Me._lb_項目_4 = New System.Windows.Forms.Label()
        Me._lb_項目_1 = New System.Windows.Forms.Label()
        Me._lb_項目_2 = New System.Windows.Forms.Label()
        Me.lbGuide = New System.Windows.Forms.Label()
        Me.lbListCount = New System.Windows.Forms.Label()
        Me.lb_該当件数 = New System.Windows.Forms.Label()
        Me._lb_項目_3 = New System.Windows.Forms.Label()
        Me._lb_項目_0 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'CmdOk
        '
        Me.CmdOk.BackColor = System.Drawing.SystemColors.Control
        Me.CmdOk.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmdOk.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CmdOk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdOk.Location = New System.Drawing.Point(506, 471)
        Me.CmdOk.Name = "CmdOk"
        Me.CmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdOk.Size = New System.Drawing.Size(92, 25)
        Me.CmdOk.TabIndex = 13
        Me.CmdOk.Text = "ＯＫ(&O)"
        Me.CmdOk.UseVisualStyleBackColor = False
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
        Me.SelListVw.Location = New System.Drawing.Point(12, 148)
        Me.SelListVw.Name = "SelListVw"
        Me.SelListVw.Size = New System.Drawing.Size(687, 316)
        Me.SelListVw.SortStyle = SortableListView.SortStyles.SortDefault
        Me.SelListVw.TabIndex = 12
        Me.SelListVw.UseCompatibleStateImageBehavior = False
        Me.SelListVw.View = System.Windows.Forms.View.Details
        '
        'CmdCan
        '
        Me.CmdCan.BackColor = System.Drawing.SystemColors.Control
        Me.CmdCan.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmdCan.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CmdCan.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CmdCan.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdCan.Location = New System.Drawing.Point(607, 471)
        Me.CmdCan.Name = "CmdCan"
        Me.CmdCan.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdCan.Size = New System.Drawing.Size(92, 25)
        Me.CmdCan.TabIndex = 14
        Me.CmdCan.Text = "ｷｬﾝｾﾙ(&C)"
        Me.CmdCan.UseVisualStyleBackColor = False
        '
        'CmdFind
        '
        Me.CmdFind.BackColor = System.Drawing.SystemColors.Control
        Me.CmdFind.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmdFind.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CmdFind.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdFind.Location = New System.Drawing.Point(711, 113)
        Me.CmdFind.Name = "CmdFind"
        Me.CmdFind.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdFind.Size = New System.Drawing.Size(83, 25)
        Me.CmdFind.TabIndex = 11
        Me.CmdFind.Text = "検索(&F)"
        Me.CmdFind.UseVisualStyleBackColor = False
        '
        'tx_検索製品
        '
        Me.tx_検索製品.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_検索製品.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_検索製品.CanForwardSetFocus = True
        Me.tx_検索製品.CanNextSetFocus = True
        Me.tx_検索製品.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_検索製品.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_検索製品.EditMode = True
        Me.tx_検索製品.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_検索製品.IMEMode = System.Windows.Forms.ImeMode.Disable
        Me.tx_検索製品.LengthType = ExText.ExTextBox.LenType.ByteType
        Me.tx_検索製品.Location = New System.Drawing.Point(99, 45)
        Me.tx_検索製品.MaxLength = 7
        Me.tx_検索製品.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_検索製品.Name = "tx_検索製品"
        Me.tx_検索製品.OldValue = "ExTextBox"
        Me.tx_検索製品.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_検索製品.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_検索製品.SelectText = True
        Me.tx_検索製品.SelLength = 0
        Me.tx_検索製品.SelStart = 0
        Me.tx_検索製品.SelText = ""
        Me.tx_検索製品.Size = New System.Drawing.Size(93, 22)
        Me.tx_検索製品.TabIndex = 0
        Me.tx_検索製品.Text = "ExTextBox"
        '
        'tx_検索名称
        '
        Me.tx_検索名称.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_検索名称.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_検索名称.CanForwardSetFocus = True
        Me.tx_検索名称.CanNextSetFocus = True
        Me.tx_検索名称.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_検索名称.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_検索名称.EditMode = True
        Me.tx_検索名称.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_検索名称.IMEMode = System.Windows.Forms.ImeMode.Hiragana
        Me.tx_検索名称.LengthType = ExText.ExTextBox.LenType.ByteType
        Me.tx_検索名称.Location = New System.Drawing.Point(98, 91)
        Me.tx_検索名称.MaxLength = 40
        Me.tx_検索名称.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_検索名称.Name = "tx_検索名称"
        Me.tx_検索名称.OldValue = "ExTextBox"
        Me.tx_検索名称.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_検索名称.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_検索名称.SelectText = True
        Me.tx_検索名称.SelLength = 0
        Me.tx_検索名称.SelStart = 0
        Me.tx_検索名称.SelText = ""
        Me.tx_検索名称.Size = New System.Drawing.Size(267, 22)
        Me.tx_検索名称.TabIndex = 2
        Me.tx_検索名称.Text = "ExTextBox"
        '
        'tx_検索仕入先
        '
        Me.tx_検索仕入先.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_検索仕入先.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_検索仕入先.CanForwardSetFocus = True
        Me.tx_検索仕入先.CanNextSetFocus = True
        Me.tx_検索仕入先.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_検索仕入先.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_検索仕入先.EditMode = True
        Me.tx_検索仕入先.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_検索仕入先.IMEMode = System.Windows.Forms.ImeMode.Disable
        Me.tx_検索仕入先.LengthType = ExText.ExTextBox.LenType.ByteType
        Me.tx_検索仕入先.Location = New System.Drawing.Point(99, 114)
        Me.tx_検索仕入先.MaxLength = 4
        Me.tx_検索仕入先.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_検索仕入先.Name = "tx_検索仕入先"
        Me.tx_検索仕入先.OldValue = "ExTextBox"
        Me.tx_検索仕入先.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_検索仕入先.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_検索仕入先.SelectText = True
        Me.tx_検索仕入先.SelLength = 0
        Me.tx_検索仕入先.SelStart = 0
        Me.tx_検索仕入先.SelText = ""
        Me.tx_検索仕入先.Size = New System.Drawing.Size(67, 22)
        Me.tx_検索仕入先.TabIndex = 3
        Me.tx_検索仕入先.Text = "ExTextBox"
        '
        'tx_検索仕様
        '
        Me.tx_検索仕様.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_検索仕様.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_検索仕様.CanForwardSetFocus = True
        Me.tx_検索仕様.CanNextSetFocus = True
        Me.tx_検索仕様.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_検索仕様.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_検索仕様.EditMode = True
        Me.tx_検索仕様.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_検索仕様.IMEMode = System.Windows.Forms.ImeMode.KatakanaHalf
        Me.tx_検索仕様.LengthType = ExText.ExTextBox.LenType.ByteType
        Me.tx_検索仕様.Location = New System.Drawing.Point(99, 68)
        Me.tx_検索仕様.MaxLength = 7
        Me.tx_検索仕様.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_検索仕様.Name = "tx_検索仕様"
        Me.tx_検索仕様.OldValue = "ExTextBox"
        Me.tx_検索仕様.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_検索仕様.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_検索仕様.SelectText = True
        Me.tx_検索仕様.SelLength = 0
        Me.tx_検索仕様.SelStart = 0
        Me.tx_検索仕様.SelText = ""
        Me.tx_検索仕様.Size = New System.Drawing.Size(79, 22)
        Me.tx_検索仕様.TabIndex = 1
        Me.tx_検索仕様.Text = "ExTextBox"
        '
        'tx_W
        '
        Me.tx_W.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_W.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_W.CanForwardSetFocus = True
        Me.tx_W.CanNextSetFocus = True
        Me.tx_W.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_W.DecimalPlace = CType(0, Short)
        Me.tx_W.EditMode = True
        Me.tx_W.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_W.FormatType = ""
        Me.tx_W.FormatTypeNega = ""
        Me.tx_W.FormatTypeNull = ""
        Me.tx_W.FormatTypeZero = ""
        Me.tx_W.InputMinus = False
        Me.tx_W.InputPlus = True
        Me.tx_W.InputZero = False
        Me.tx_W.Location = New System.Drawing.Point(481, 45)
        Me.tx_W.MaxLength = 4
        Me.tx_W.Name = "tx_W"
        Me.tx_W.OldValue = "9999"
        Me.tx_W.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_W.SelectText = True
        Me.tx_W.SelLength = 0
        Me.tx_W.SelStart = 0
        Me.tx_W.SelText = ""
        Me.tx_W.Size = New System.Drawing.Size(52, 22)
        Me.tx_W.TabIndex = 4
        Me.tx_W.Text = "9999"
        '
        'tx_D
        '
        Me.tx_D.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_D.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_D.CanForwardSetFocus = True
        Me.tx_D.CanNextSetFocus = True
        Me.tx_D.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_D.DecimalPlace = CType(0, Short)
        Me.tx_D.EditMode = True
        Me.tx_D.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_D.FormatType = ""
        Me.tx_D.FormatTypeNega = ""
        Me.tx_D.FormatTypeNull = ""
        Me.tx_D.FormatTypeZero = ""
        Me.tx_D.InputMinus = False
        Me.tx_D.InputPlus = True
        Me.tx_D.InputZero = False
        Me.tx_D.Location = New System.Drawing.Point(481, 68)
        Me.tx_D.MaxLength = 4
        Me.tx_D.Name = "tx_D"
        Me.tx_D.OldValue = "9999"
        Me.tx_D.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_D.SelectText = True
        Me.tx_D.SelLength = 0
        Me.tx_D.SelStart = 0
        Me.tx_D.SelText = ""
        Me.tx_D.Size = New System.Drawing.Size(52, 22)
        Me.tx_D.TabIndex = 5
        Me.tx_D.Text = "9999"
        '
        'tx_H
        '
        Me.tx_H.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_H.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_H.CanForwardSetFocus = True
        Me.tx_H.CanNextSetFocus = True
        Me.tx_H.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_H.DecimalPlace = CType(0, Short)
        Me.tx_H.EditMode = True
        Me.tx_H.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_H.FormatType = ""
        Me.tx_H.FormatTypeNega = ""
        Me.tx_H.FormatTypeNull = ""
        Me.tx_H.FormatTypeZero = ""
        Me.tx_H.InputMinus = False
        Me.tx_H.InputPlus = True
        Me.tx_H.InputZero = False
        Me.tx_H.Location = New System.Drawing.Point(481, 91)
        Me.tx_H.MaxLength = 4
        Me.tx_H.Name = "tx_H"
        Me.tx_H.OldValue = "9999"
        Me.tx_H.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_H.SelectText = True
        Me.tx_H.SelLength = 0
        Me.tx_H.SelStart = 0
        Me.tx_H.SelText = ""
        Me.tx_H.Size = New System.Drawing.Size(52, 22)
        Me.tx_H.TabIndex = 6
        Me.tx_H.Text = "9999"
        '
        'tx_D2
        '
        Me.tx_D2.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_D2.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_D2.CanForwardSetFocus = True
        Me.tx_D2.CanNextSetFocus = True
        Me.tx_D2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_D2.DecimalPlace = CType(0, Short)
        Me.tx_D2.EditMode = True
        Me.tx_D2.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_D2.FormatType = ""
        Me.tx_D2.FormatTypeNega = ""
        Me.tx_D2.FormatTypeNull = ""
        Me.tx_D2.FormatTypeZero = ""
        Me.tx_D2.InputMinus = False
        Me.tx_D2.InputPlus = True
        Me.tx_D2.InputZero = False
        Me.tx_D2.Location = New System.Drawing.Point(641, 68)
        Me.tx_D2.MaxLength = 4
        Me.tx_D2.Name = "tx_D2"
        Me.tx_D2.OldValue = "9999"
        Me.tx_D2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_D2.SelectText = True
        Me.tx_D2.SelLength = 0
        Me.tx_D2.SelStart = 0
        Me.tx_D2.SelText = ""
        Me.tx_D2.Size = New System.Drawing.Size(52, 22)
        Me.tx_D2.TabIndex = 8
        Me.tx_D2.Text = "9999"
        '
        'tx_D1
        '
        Me.tx_D1.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_D1.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_D1.CanForwardSetFocus = True
        Me.tx_D1.CanNextSetFocus = True
        Me.tx_D1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_D1.DecimalPlace = CType(0, Short)
        Me.tx_D1.EditMode = True
        Me.tx_D1.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_D1.FormatType = ""
        Me.tx_D1.FormatTypeNega = ""
        Me.tx_D1.FormatTypeNull = ""
        Me.tx_D1.FormatTypeZero = ""
        Me.tx_D1.InputMinus = False
        Me.tx_D1.InputPlus = True
        Me.tx_D1.InputZero = False
        Me.tx_D1.Location = New System.Drawing.Point(641, 45)
        Me.tx_D1.MaxLength = 4
        Me.tx_D1.Name = "tx_D1"
        Me.tx_D1.OldValue = "9999"
        Me.tx_D1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_D1.SelectText = True
        Me.tx_D1.SelLength = 0
        Me.tx_D1.SelStart = 0
        Me.tx_D1.SelText = ""
        Me.tx_D1.Size = New System.Drawing.Size(52, 22)
        Me.tx_D1.TabIndex = 7
        Me.tx_D1.Text = "9999"
        '
        'tx_H2
        '
        Me.tx_H2.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_H2.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_H2.CanForwardSetFocus = True
        Me.tx_H2.CanNextSetFocus = True
        Me.tx_H2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_H2.DecimalPlace = CType(0, Short)
        Me.tx_H2.EditMode = True
        Me.tx_H2.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_H2.FormatType = ""
        Me.tx_H2.FormatTypeNega = ""
        Me.tx_H2.FormatTypeNull = ""
        Me.tx_H2.FormatTypeZero = ""
        Me.tx_H2.InputMinus = False
        Me.tx_H2.InputPlus = True
        Me.tx_H2.InputZero = False
        Me.tx_H2.Location = New System.Drawing.Point(641, 114)
        Me.tx_H2.MaxLength = 4
        Me.tx_H2.Name = "tx_H2"
        Me.tx_H2.OldValue = "9999"
        Me.tx_H2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_H2.SelectText = True
        Me.tx_H2.SelLength = 0
        Me.tx_H2.SelStart = 0
        Me.tx_H2.SelText = ""
        Me.tx_H2.Size = New System.Drawing.Size(52, 22)
        Me.tx_H2.TabIndex = 10
        Me.tx_H2.Text = "9999"
        '
        'tx_H1
        '
        Me.tx_H1.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_H1.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_H1.CanForwardSetFocus = True
        Me.tx_H1.CanNextSetFocus = True
        Me.tx_H1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_H1.DecimalPlace = CType(0, Short)
        Me.tx_H1.EditMode = True
        Me.tx_H1.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_H1.FormatType = ""
        Me.tx_H1.FormatTypeNega = ""
        Me.tx_H1.FormatTypeNull = ""
        Me.tx_H1.FormatTypeZero = ""
        Me.tx_H1.InputMinus = False
        Me.tx_H1.InputPlus = True
        Me.tx_H1.InputZero = False
        Me.tx_H1.Location = New System.Drawing.Point(641, 91)
        Me.tx_H1.MaxLength = 4
        Me.tx_H1.Name = "tx_H1"
        Me.tx_H1.OldValue = "9999"
        Me.tx_H1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_H1.SelectText = True
        Me.tx_H1.SelLength = 0
        Me.tx_H1.SelStart = 0
        Me.tx_H1.SelText = ""
        Me.tx_H1.Size = New System.Drawing.Size(52, 22)
        Me.tx_H1.TabIndex = 9
        Me.tx_H1.Text = "9999"
        '
        '_lb_項目_10
        '
        Me._lb_項目_10.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_10.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_10.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_10.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_10.ForeColor = System.Drawing.Color.White
        Me._lb_項目_10.Location = New System.Drawing.Point(552, 91)
        Me._lb_項目_10.Name = "_lb_項目_10"
        Me._lb_項目_10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_10.Size = New System.Drawing.Size(89, 20)
        Me._lb_項目_10.TabIndex = 28
        Me._lb_項目_10.Text = "Ｈ１"
        Me._lb_項目_10.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_9
        '
        Me._lb_項目_9.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_9.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_9.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_9.ForeColor = System.Drawing.Color.White
        Me._lb_項目_9.Location = New System.Drawing.Point(552, 114)
        Me._lb_項目_9.Name = "_lb_項目_9"
        Me._lb_項目_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_9.Size = New System.Drawing.Size(89, 20)
        Me._lb_項目_9.TabIndex = 27
        Me._lb_項目_9.Text = "Ｈ２"
        Me._lb_項目_9.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_7
        '
        Me._lb_項目_7.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_7.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_7.ForeColor = System.Drawing.Color.White
        Me._lb_項目_7.Location = New System.Drawing.Point(552, 45)
        Me._lb_項目_7.Name = "_lb_項目_7"
        Me._lb_項目_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_7.Size = New System.Drawing.Size(89, 20)
        Me._lb_項目_7.TabIndex = 26
        Me._lb_項目_7.Text = "Ｄ１"
        Me._lb_項目_7.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_8
        '
        Me._lb_項目_8.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_8.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_8.ForeColor = System.Drawing.Color.White
        Me._lb_項目_8.Location = New System.Drawing.Point(552, 68)
        Me._lb_項目_8.Name = "_lb_項目_8"
        Me._lb_項目_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_8.Size = New System.Drawing.Size(89, 20)
        Me._lb_項目_8.TabIndex = 25
        Me._lb_項目_8.Text = "Ｄ２"
        Me._lb_項目_8.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_6
        '
        Me._lb_項目_6.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_6.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_6.ForeColor = System.Drawing.Color.White
        Me._lb_項目_6.Location = New System.Drawing.Point(392, 91)
        Me._lb_項目_6.Name = "_lb_項目_6"
        Me._lb_項目_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_6.Size = New System.Drawing.Size(89, 20)
        Me._lb_項目_6.TabIndex = 24
        Me._lb_項目_6.Text = "Ｈ"
        Me._lb_項目_6.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_5
        '
        Me._lb_項目_5.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_5.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_5.ForeColor = System.Drawing.Color.White
        Me._lb_項目_5.Location = New System.Drawing.Point(392, 68)
        Me._lb_項目_5.Name = "_lb_項目_5"
        Me._lb_項目_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_5.Size = New System.Drawing.Size(89, 20)
        Me._lb_項目_5.TabIndex = 23
        Me._lb_項目_5.Text = "Ｄ"
        Me._lb_項目_5.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_4
        '
        Me._lb_項目_4.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_4.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_4.ForeColor = System.Drawing.Color.White
        Me._lb_項目_4.Location = New System.Drawing.Point(392, 45)
        Me._lb_項目_4.Name = "_lb_項目_4"
        Me._lb_項目_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_4.Size = New System.Drawing.Size(89, 20)
        Me._lb_項目_4.TabIndex = 22
        Me._lb_項目_4.Text = "Ｗ"
        Me._lb_項目_4.TextAlign = System.Drawing.ContentAlignment.TopCenter
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
        Me._lb_項目_1.Size = New System.Drawing.Size(89, 20)
        Me._lb_項目_1.TabIndex = 21
        Me._lb_項目_1.Text = "仕様NO"
        Me._lb_項目_1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_2
        '
        Me._lb_項目_2.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_2.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_2.ForeColor = System.Drawing.Color.White
        Me._lb_項目_2.Location = New System.Drawing.Point(9, 91)
        Me._lb_項目_2.Name = "_lb_項目_2"
        Me._lb_項目_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_2.Size = New System.Drawing.Size(89, 20)
        Me._lb_項目_2.TabIndex = 20
        Me._lb_項目_2.Text = "漢字名称"
        Me._lb_項目_2.TextAlign = System.Drawing.ContentAlignment.TopCenter
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
        Me.lbGuide.Size = New System.Drawing.Size(317, 17)
        Me.lbGuide.TabIndex = 19
        Me.lbGuide.Text = "[↑][↓]で選択、[Enter]で決定 ／[Esc]で中止"
        '
        'lbListCount
        '
        Me.lbListCount.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.lbListCount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lbListCount.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lbListCount.ForeColor = System.Drawing.Color.Yellow
        Me.lbListCount.Location = New System.Drawing.Point(86, 5)
        Me.lbListCount.Name = "lbListCount"
        Me.lbListCount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lbListCount.Size = New System.Drawing.Size(89, 17)
        Me.lbListCount.TabIndex = 18
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
        Me.lb_該当件数.Size = New System.Drawing.Size(75, 17)
        Me.lb_該当件数.TabIndex = 17
        Me.lb_該当件数.Text = "該当件数"
        Me.lb_該当件数.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_3
        '
        Me._lb_項目_3.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_3.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_3.ForeColor = System.Drawing.Color.White
        Me._lb_項目_3.Location = New System.Drawing.Point(10, 114)
        Me._lb_項目_3.Name = "_lb_項目_3"
        Me._lb_項目_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_3.Size = New System.Drawing.Size(89, 20)
        Me._lb_項目_3.TabIndex = 16
        Me._lb_項目_3.Text = "主仕入先"
        Me._lb_項目_3.TextAlign = System.Drawing.ContentAlignment.TopCenter
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
        Me._lb_項目_0.Size = New System.Drawing.Size(89, 20)
        Me._lb_項目_0.TabIndex = 15
        Me._lb_項目_0.Text = "製品NO"
        Me._lb_項目_0.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'SeiSen_cls
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.CmdCan
        Me.ClientSize = New System.Drawing.Size(804, 499)
        Me.Controls.Add(Me.SelListVw)
        Me.Controls.Add(Me.CmdOk)
        Me.Controls.Add(Me.CmdCan)
        Me.Controls.Add(Me.CmdFind)
        Me.Controls.Add(Me.tx_検索製品)
        Me.Controls.Add(Me.tx_検索名称)
        Me.Controls.Add(Me.tx_検索仕入先)
        Me.Controls.Add(Me.tx_検索仕様)
        Me.Controls.Add(Me.tx_W)
        Me.Controls.Add(Me.tx_D)
        Me.Controls.Add(Me.tx_H)
        Me.Controls.Add(Me.tx_D2)
        Me.Controls.Add(Me.tx_D1)
        Me.Controls.Add(Me.tx_H2)
        Me.Controls.Add(Me.tx_H1)
        Me.Controls.Add(Me._lb_項目_10)
        Me.Controls.Add(Me._lb_項目_9)
        Me.Controls.Add(Me._lb_項目_7)
        Me.Controls.Add(Me._lb_項目_8)
        Me.Controls.Add(Me._lb_項目_6)
        Me.Controls.Add(Me._lb_項目_5)
        Me.Controls.Add(Me._lb_項目_4)
        Me.Controls.Add(Me._lb_項目_1)
        Me.Controls.Add(Me._lb_項目_2)
        Me.Controls.Add(Me.lbGuide)
        Me.Controls.Add(Me.lbListCount)
        Me.Controls.Add(Me.lb_該当件数)
        Me.Controls.Add(Me._lb_項目_3)
        Me.Controls.Add(Me._lb_項目_0)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "SeiSen_cls"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "製品選択"
        Me.ResumeLayout(False)

    End Sub
    Public ToolTip1 As System.Windows.Forms.ToolTip
    Public WithEvents SelListVw As SortableListView
    Public WithEvents CmdOk As System.Windows.Forms.Button
    Public WithEvents CmdCan As System.Windows.Forms.Button
    Public WithEvents CmdFind As System.Windows.Forms.Button
    Public WithEvents tx_検索製品 As ExText.ExTextBox
    Public WithEvents tx_検索名称 As ExText.ExTextBox
    Public WithEvents tx_検索仕入先 As ExText.ExTextBox
    Public WithEvents tx_検索仕様 As ExText.ExTextBox
    Public WithEvents tx_W As ExNmText.ExNmTextBox
    Public WithEvents tx_D As ExNmText.ExNmTextBox
    Public WithEvents tx_H As ExNmText.ExNmTextBox
    Public WithEvents tx_D2 As ExNmText.ExNmTextBox
    Public WithEvents tx_D1 As ExNmText.ExNmTextBox
    Public WithEvents tx_H2 As ExNmText.ExNmTextBox
    Public WithEvents tx_H1 As ExNmText.ExNmTextBox
    Public WithEvents _lb_項目_10 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_9 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_7 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_8 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_6 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_5 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_4 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_1 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_2 As System.Windows.Forms.Label
    Public WithEvents lbGuide As System.Windows.Forms.Label
    Public WithEvents lbListCount As System.Windows.Forms.Label
    Public WithEvents lb_該当件数 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_3 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_0 As System.Windows.Forms.Label
End Class