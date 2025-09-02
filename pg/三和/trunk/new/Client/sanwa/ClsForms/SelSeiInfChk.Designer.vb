<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SelSeiInfChk
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

    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使って変更できます。
    'コード エディタを使用して、変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SelSeiInfChk))
        Me.PicFind_0 = New System.Windows.Forms.Panel()
        Me.tx_F1検索製品 = New ExText.ExTextBox()
        Me.tx_F1検索名称 = New ExText.ExTextBox()
        Me.tx_F1検索仕入先 = New ExText.ExTextBox()
        Me.tx_F1検索仕様 = New ExText.ExTextBox()
        Me.tx_F1検索W = New ExNmText.ExNmTextBox()
        Me.tx_F1検索D = New ExNmText.ExNmTextBox()
        Me.tx_F1検索H = New ExNmText.ExNmTextBox()
        Me.tx_F1検索D1 = New ExNmText.ExNmTextBox()
        Me.tx_F1検索D2 = New ExNmText.ExNmTextBox()
        Me.tx_F1検索H1 = New ExNmText.ExNmTextBox()
        Me.tx_F1検索H2 = New ExNmText.ExNmTextBox()
        Me.tx_F1検索仕様e = New ExText.ExTextBox()
        Me._lb_kara_2 = New System.Windows.Forms.Label()
        Me._lb_項目_23 = New System.Windows.Forms.Label()
        Me._lb_項目_22 = New System.Windows.Forms.Label()
        Me._lb_項目_16 = New System.Windows.Forms.Label()
        Me._lb_項目_15 = New System.Windows.Forms.Label()
        Me._lb_項目_14 = New System.Windows.Forms.Label()
        Me._lb_項目_13 = New System.Windows.Forms.Label()
        Me._lb_項目_12 = New System.Windows.Forms.Label()
        Me._lb_項目_0 = New System.Windows.Forms.Label()
        Me._lb_項目_3 = New System.Windows.Forms.Label()
        Me._lb_項目_2 = New System.Windows.Forms.Label()
        Me._lb_項目_1 = New System.Windows.Forms.Label()
        Me.PicFind_1 = New System.Windows.Forms.Panel()
        Me.tx_F2検索品群 = New ExText.ExTextBox()
        Me.tx_F2検索名称 = New ExText.ExTextBox()
        Me._lb_項目_4 = New System.Windows.Forms.Label()
        Me._lb_項目_5 = New System.Windows.Forms.Label()
        Me.PicFind_2 = New System.Windows.Forms.Panel()
        Me.tx_F3検索ユニット = New ExText.ExTextBox()
        Me.tx_F3検索名称 = New ExText.ExTextBox()
        Me._lb_項目_8 = New System.Windows.Forms.Label()
        Me._lb_項目_9 = New System.Windows.Forms.Label()
        Me.PicFind_3 = New System.Windows.Forms.Panel()
        Me.tx_F4検索製品 = New ExText.ExTextBox()
        Me.tx_F4検索名称 = New ExText.ExTextBox()
        Me.tx_F4検索PC区分 = New ExText.ExTextBox()
        Me.tx_F4検索仕入先 = New ExText.ExTextBox()
        Me.tx_F4検索W = New ExNmText.ExNmTextBox()
        Me.tx_F4検索D = New ExNmText.ExNmTextBox()
        Me.tx_F4検索H = New ExNmText.ExNmTextBox()
        Me.tx_F4検索径 = New ExNmText.ExNmTextBox()
        Me.tx_F4検索T = New ExNmText.ExNmTextBox()
        Me._lb_項目_21 = New System.Windows.Forms.Label()
        Me._lb_項目_20 = New System.Windows.Forms.Label()
        Me._lb_項目_19 = New System.Windows.Forms.Label()
        Me._lb_項目_18 = New System.Windows.Forms.Label()
        Me._lb_項目_17 = New System.Windows.Forms.Label()
        Me._lb_項目_6 = New System.Windows.Forms.Label()
        Me._lb_項目_7 = New System.Windows.Forms.Label()
        Me._lb_項目_10 = New System.Windows.Forms.Label()
        Me._lb_項目_11 = New System.Windows.Forms.Label()
        Me.cmdChkOn = New System.Windows.Forms.Button()
        Me.cmdChkOff = New System.Windows.Forms.Button()
        Me.ck_Find_3 = New System.Windows.Forms.CheckBox()
        Me.ck_Find_2 = New System.Windows.Forms.CheckBox()
        Me.ck_Find_1 = New System.Windows.Forms.CheckBox()
        Me.ck_Find_0 = New System.Windows.Forms.CheckBox()
        Me.CmdCan = New System.Windows.Forms.Button()
        Me.CmdOk = New System.Windows.Forms.Button()
        Me.CmdFind = New System.Windows.Forms.Button()
        Me.SelListVw = New SortableListView()
        Me.lb_該当件数 = New System.Windows.Forms.Label()
        Me.lbListCount = New System.Windows.Forms.Label()
        Me.lbGuide = New System.Windows.Forms.Label()
        Me.lb_Find_3 = New System.Windows.Forms.Label()
        Me.lb_Find_2 = New System.Windows.Forms.Label()
        Me.lb_Find_1 = New System.Windows.Forms.Label()
        Me.lb_Find_0 = New System.Windows.Forms.Label()
        Me.PicFind_0.SuspendLayout()
        Me.PicFind_1.SuspendLayout()
        Me.PicFind_2.SuspendLayout()
        Me.PicFind_3.SuspendLayout()
        Me.SuspendLayout()
        '
        'PicFind_0
        '
        Me.PicFind_0.BackColor = System.Drawing.SystemColors.Control
        Me.PicFind_0.Controls.Add(Me.tx_F1検索製品)
        Me.PicFind_0.Controls.Add(Me.tx_F1検索名称)
        Me.PicFind_0.Controls.Add(Me.tx_F1検索仕入先)
        Me.PicFind_0.Controls.Add(Me.tx_F1検索仕様)
        Me.PicFind_0.Controls.Add(Me.tx_F1検索W)
        Me.PicFind_0.Controls.Add(Me.tx_F1検索D)
        Me.PicFind_0.Controls.Add(Me.tx_F1検索H)
        Me.PicFind_0.Controls.Add(Me.tx_F1検索D1)
        Me.PicFind_0.Controls.Add(Me.tx_F1検索D2)
        Me.PicFind_0.Controls.Add(Me.tx_F1検索H1)
        Me.PicFind_0.Controls.Add(Me.tx_F1検索H2)
        Me.PicFind_0.Controls.Add(Me.tx_F1検索仕様e)
        Me.PicFind_0.Controls.Add(Me._lb_kara_2)
        Me.PicFind_0.Controls.Add(Me._lb_項目_23)
        Me.PicFind_0.Controls.Add(Me._lb_項目_22)
        Me.PicFind_0.Controls.Add(Me._lb_項目_16)
        Me.PicFind_0.Controls.Add(Me._lb_項目_15)
        Me.PicFind_0.Controls.Add(Me._lb_項目_14)
        Me.PicFind_0.Controls.Add(Me._lb_項目_13)
        Me.PicFind_0.Controls.Add(Me._lb_項目_12)
        Me.PicFind_0.Controls.Add(Me._lb_項目_0)
        Me.PicFind_0.Controls.Add(Me._lb_項目_3)
        Me.PicFind_0.Controls.Add(Me._lb_項目_2)
        Me.PicFind_0.Controls.Add(Me._lb_項目_1)
        Me.PicFind_0.Cursor = System.Windows.Forms.Cursors.Default
        Me.PicFind_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.PicFind_0.Location = New System.Drawing.Point(12, 68)
        Me.PicFind_0.Name = "PicFind_0"
        Me.PicFind_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.PicFind_0.Size = New System.Drawing.Size(691, 90)
        Me.PicFind_0.TabIndex = 4
        '
        'tx_F1検索製品
        '
        Me.tx_F1検索製品.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_F1検索製品.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_F1検索製品.CanForwardSetFocus = True
        Me.tx_F1検索製品.CanNextSetFocus = True
        Me.tx_F1検索製品.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_F1検索製品.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_F1検索製品.EditMode = True
        Me.tx_F1検索製品.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_F1検索製品.IMEMode = System.Windows.Forms.ImeMode.Disable
        Me.tx_F1検索製品.LengthType = ExText.ExTextBox.LenType.ByteType
        Me.tx_F1検索製品.Location = New System.Drawing.Point(89, 0)
        Me.tx_F1検索製品.MaxLength = 7
        Me.tx_F1検索製品.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_F1検索製品.Name = "tx_F1検索製品"
        Me.tx_F1検索製品.OldValue = "ExTextBox"
        Me.tx_F1検索製品.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_F1検索製品.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_F1検索製品.SelectText = True
        Me.tx_F1検索製品.SelLength = 0
        Me.tx_F1検索製品.SelStart = 0
        Me.tx_F1検索製品.SelText = ""
        Me.tx_F1検索製品.Size = New System.Drawing.Size(93, 22)
        Me.tx_F1検索製品.TabIndex = 5
        Me.tx_F1検索製品.Text = "ExTextBox"
        '
        'tx_F1検索名称
        '
        Me.tx_F1検索名称.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_F1検索名称.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_F1検索名称.CanForwardSetFocus = True
        Me.tx_F1検索名称.CanNextSetFocus = True
        Me.tx_F1検索名称.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_F1検索名称.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_F1検索名称.EditMode = True
        Me.tx_F1検索名称.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_F1検索名称.IMEMode = System.Windows.Forms.ImeMode.Hiragana
        Me.tx_F1検索名称.LengthType = ExText.ExTextBox.LenType.ByteType
        Me.tx_F1検索名称.Location = New System.Drawing.Point(89, 43)
        Me.tx_F1検索名称.MaxLength = 40
        Me.tx_F1検索名称.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_F1検索名称.Name = "tx_F1検索名称"
        Me.tx_F1検索名称.OldValue = "ExTextBox"
        Me.tx_F1検索名称.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_F1検索名称.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_F1検索名称.SelectText = True
        Me.tx_F1検索名称.SelLength = 0
        Me.tx_F1検索名称.SelStart = 0
        Me.tx_F1検索名称.SelText = ""
        Me.tx_F1検索名称.Size = New System.Drawing.Size(267, 22)
        Me.tx_F1検索名称.TabIndex = 8
        Me.tx_F1検索名称.Text = "ExTextBox"
        '
        'tx_F1検索仕入先
        '
        Me.tx_F1検索仕入先.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_F1検索仕入先.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_F1検索仕入先.CanForwardSetFocus = True
        Me.tx_F1検索仕入先.CanNextSetFocus = True
        Me.tx_F1検索仕入先.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_F1検索仕入先.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_F1検索仕入先.EditMode = True
        Me.tx_F1検索仕入先.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_F1検索仕入先.IMEMode = System.Windows.Forms.ImeMode.Disable
        Me.tx_F1検索仕入先.LengthType = ExText.ExTextBox.LenType.ByteType
        Me.tx_F1検索仕入先.Location = New System.Drawing.Point(89, 64)
        Me.tx_F1検索仕入先.MaxLength = 4
        Me.tx_F1検索仕入先.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_F1検索仕入先.Name = "tx_F1検索仕入先"
        Me.tx_F1検索仕入先.OldValue = "ExTextBox"
        Me.tx_F1検索仕入先.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_F1検索仕入先.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_F1検索仕入先.SelectText = True
        Me.tx_F1検索仕入先.SelLength = 0
        Me.tx_F1検索仕入先.SelStart = 0
        Me.tx_F1検索仕入先.SelText = ""
        Me.tx_F1検索仕入先.Size = New System.Drawing.Size(67, 22)
        Me.tx_F1検索仕入先.TabIndex = 9
        Me.tx_F1検索仕入先.Text = "ExTextBox"
        '
        'tx_F1検索仕様
        '
        Me.tx_F1検索仕様.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_F1検索仕様.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_F1検索仕様.CanForwardSetFocus = True
        Me.tx_F1検索仕様.CanNextSetFocus = True
        Me.tx_F1検索仕様.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_F1検索仕様.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_F1検索仕様.EditMode = True
        Me.tx_F1検索仕様.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_F1検索仕様.IMEMode = System.Windows.Forms.ImeMode.KatakanaHalf
        Me.tx_F1検索仕様.LengthType = ExText.ExTextBox.LenType.ByteType
        Me.tx_F1検索仕様.Location = New System.Drawing.Point(89, 21)
        Me.tx_F1検索仕様.MaxLength = 7
        Me.tx_F1検索仕様.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_F1検索仕様.Name = "tx_F1検索仕様"
        Me.tx_F1検索仕様.OldValue = "ExTextBox"
        Me.tx_F1検索仕様.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_F1検索仕様.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_F1検索仕様.SelectText = True
        Me.tx_F1検索仕様.SelLength = 0
        Me.tx_F1検索仕様.SelStart = 0
        Me.tx_F1検索仕様.SelText = ""
        Me.tx_F1検索仕様.Size = New System.Drawing.Size(79, 22)
        Me.tx_F1検索仕様.TabIndex = 6
        Me.tx_F1検索仕様.Text = "ExTextBox"
        '
        'tx_F1検索W
        '
        Me.tx_F1検索W.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_F1検索W.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_F1検索W.CanForwardSetFocus = True
        Me.tx_F1検索W.CanNextSetFocus = True
        Me.tx_F1検索W.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_F1検索W.DecimalPlace = CType(0, Short)
        Me.tx_F1検索W.EditMode = True
        Me.tx_F1検索W.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_F1検索W.FormatType = ""
        Me.tx_F1検索W.FormatTypeNega = ""
        Me.tx_F1検索W.FormatTypeNull = ""
        Me.tx_F1検索W.FormatTypeZero = ""
        Me.tx_F1検索W.InputMinus = False
        Me.tx_F1検索W.InputPlus = True
        Me.tx_F1検索W.InputZero = False
        Me.tx_F1検索W.Location = New System.Drawing.Point(469, 0)
        Me.tx_F1検索W.MaxLength = 4
        Me.tx_F1検索W.Name = "tx_F1検索W"
        Me.tx_F1検索W.OldValue = "9999"
        Me.tx_F1検索W.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_F1検索W.SelectText = True
        Me.tx_F1検索W.SelLength = 0
        Me.tx_F1検索W.SelStart = 0
        Me.tx_F1検索W.SelText = ""
        Me.tx_F1検索W.Size = New System.Drawing.Size(52, 22)
        Me.tx_F1検索W.TabIndex = 10
        Me.tx_F1検索W.Text = "9999"
        '
        'tx_F1検索D
        '
        Me.tx_F1検索D.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_F1検索D.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_F1検索D.CanForwardSetFocus = True
        Me.tx_F1検索D.CanNextSetFocus = True
        Me.tx_F1検索D.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_F1検索D.DecimalPlace = CType(0, Short)
        Me.tx_F1検索D.EditMode = True
        Me.tx_F1検索D.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_F1検索D.FormatType = ""
        Me.tx_F1検索D.FormatTypeNega = ""
        Me.tx_F1検索D.FormatTypeNull = ""
        Me.tx_F1検索D.FormatTypeZero = ""
        Me.tx_F1検索D.InputMinus = False
        Me.tx_F1検索D.InputPlus = True
        Me.tx_F1検索D.InputZero = False
        Me.tx_F1検索D.Location = New System.Drawing.Point(469, 21)
        Me.tx_F1検索D.MaxLength = 4
        Me.tx_F1検索D.Name = "tx_F1検索D"
        Me.tx_F1検索D.OldValue = "9999"
        Me.tx_F1検索D.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_F1検索D.SelectText = True
        Me.tx_F1検索D.SelLength = 0
        Me.tx_F1検索D.SelStart = 0
        Me.tx_F1検索D.SelText = ""
        Me.tx_F1検索D.Size = New System.Drawing.Size(52, 22)
        Me.tx_F1検索D.TabIndex = 11
        Me.tx_F1検索D.Text = "9999"
        '
        'tx_F1検索H
        '
        Me.tx_F1検索H.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_F1検索H.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_F1検索H.CanForwardSetFocus = True
        Me.tx_F1検索H.CanNextSetFocus = True
        Me.tx_F1検索H.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_F1検索H.DecimalPlace = CType(0, Short)
        Me.tx_F1検索H.EditMode = True
        Me.tx_F1検索H.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_F1検索H.FormatType = ""
        Me.tx_F1検索H.FormatTypeNega = ""
        Me.tx_F1検索H.FormatTypeNull = ""
        Me.tx_F1検索H.FormatTypeZero = ""
        Me.tx_F1検索H.InputMinus = False
        Me.tx_F1検索H.InputPlus = True
        Me.tx_F1検索H.InputZero = False
        Me.tx_F1検索H.Location = New System.Drawing.Point(469, 42)
        Me.tx_F1検索H.MaxLength = 4
        Me.tx_F1検索H.Name = "tx_F1検索H"
        Me.tx_F1検索H.OldValue = "9999"
        Me.tx_F1検索H.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_F1検索H.SelectText = True
        Me.tx_F1検索H.SelLength = 0
        Me.tx_F1検索H.SelStart = 0
        Me.tx_F1検索H.SelText = ""
        Me.tx_F1検索H.Size = New System.Drawing.Size(52, 22)
        Me.tx_F1検索H.TabIndex = 12
        Me.tx_F1検索H.Text = "9999"
        '
        'tx_F1検索D1
        '
        Me.tx_F1検索D1.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_F1検索D1.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_F1検索D1.CanForwardSetFocus = True
        Me.tx_F1検索D1.CanNextSetFocus = True
        Me.tx_F1検索D1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_F1検索D1.DecimalPlace = CType(0, Short)
        Me.tx_F1検索D1.EditMode = True
        Me.tx_F1検索D1.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_F1検索D1.FormatType = ""
        Me.tx_F1検索D1.FormatTypeNega = ""
        Me.tx_F1検索D1.FormatTypeNull = ""
        Me.tx_F1検索D1.FormatTypeZero = ""
        Me.tx_F1検索D1.InputMinus = False
        Me.tx_F1検索D1.InputPlus = True
        Me.tx_F1検索D1.InputZero = False
        Me.tx_F1検索D1.Location = New System.Drawing.Point(629, 0)
        Me.tx_F1検索D1.MaxLength = 4
        Me.tx_F1検索D1.Name = "tx_F1検索D1"
        Me.tx_F1検索D1.OldValue = "9999"
        Me.tx_F1検索D1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_F1検索D1.SelectText = True
        Me.tx_F1検索D1.SelLength = 0
        Me.tx_F1検索D1.SelStart = 0
        Me.tx_F1検索D1.SelText = ""
        Me.tx_F1検索D1.Size = New System.Drawing.Size(52, 22)
        Me.tx_F1検索D1.TabIndex = 13
        Me.tx_F1検索D1.Text = "9999"
        '
        'tx_F1検索D2
        '
        Me.tx_F1検索D2.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_F1検索D2.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_F1検索D2.CanForwardSetFocus = True
        Me.tx_F1検索D2.CanNextSetFocus = True
        Me.tx_F1検索D2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_F1検索D2.DecimalPlace = CType(0, Short)
        Me.tx_F1検索D2.EditMode = True
        Me.tx_F1検索D2.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_F1検索D2.FormatType = ""
        Me.tx_F1検索D2.FormatTypeNega = ""
        Me.tx_F1検索D2.FormatTypeNull = ""
        Me.tx_F1検索D2.FormatTypeZero = ""
        Me.tx_F1検索D2.InputMinus = False
        Me.tx_F1検索D2.InputPlus = True
        Me.tx_F1検索D2.InputZero = False
        Me.tx_F1検索D2.Location = New System.Drawing.Point(629, 21)
        Me.tx_F1検索D2.MaxLength = 4
        Me.tx_F1検索D2.Name = "tx_F1検索D2"
        Me.tx_F1検索D2.OldValue = "9999"
        Me.tx_F1検索D2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_F1検索D2.SelectText = True
        Me.tx_F1検索D2.SelLength = 0
        Me.tx_F1検索D2.SelStart = 0
        Me.tx_F1検索D2.SelText = ""
        Me.tx_F1検索D2.Size = New System.Drawing.Size(52, 22)
        Me.tx_F1検索D2.TabIndex = 14
        Me.tx_F1検索D2.Text = "9999"
        '
        'tx_F1検索H1
        '
        Me.tx_F1検索H1.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_F1検索H1.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_F1検索H1.CanForwardSetFocus = True
        Me.tx_F1検索H1.CanNextSetFocus = True
        Me.tx_F1検索H1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_F1検索H1.DecimalPlace = CType(0, Short)
        Me.tx_F1検索H1.EditMode = True
        Me.tx_F1検索H1.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_F1検索H1.FormatType = ""
        Me.tx_F1検索H1.FormatTypeNega = ""
        Me.tx_F1検索H1.FormatTypeNull = ""
        Me.tx_F1検索H1.FormatTypeZero = ""
        Me.tx_F1検索H1.InputMinus = False
        Me.tx_F1検索H1.InputPlus = True
        Me.tx_F1検索H1.InputZero = False
        Me.tx_F1検索H1.Location = New System.Drawing.Point(629, 42)
        Me.tx_F1検索H1.MaxLength = 4
        Me.tx_F1検索H1.Name = "tx_F1検索H1"
        Me.tx_F1検索H1.OldValue = "9999"
        Me.tx_F1検索H1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_F1検索H1.SelectText = True
        Me.tx_F1検索H1.SelLength = 0
        Me.tx_F1検索H1.SelStart = 0
        Me.tx_F1検索H1.SelText = ""
        Me.tx_F1検索H1.Size = New System.Drawing.Size(52, 22)
        Me.tx_F1検索H1.TabIndex = 15
        Me.tx_F1検索H1.Text = "9999"
        '
        'tx_F1検索H2
        '
        Me.tx_F1検索H2.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_F1検索H2.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_F1検索H2.CanForwardSetFocus = True
        Me.tx_F1検索H2.CanNextSetFocus = True
        Me.tx_F1検索H2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_F1検索H2.DecimalPlace = CType(0, Short)
        Me.tx_F1検索H2.EditMode = True
        Me.tx_F1検索H2.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_F1検索H2.FormatType = ""
        Me.tx_F1検索H2.FormatTypeNega = ""
        Me.tx_F1検索H2.FormatTypeNull = ""
        Me.tx_F1検索H2.FormatTypeZero = ""
        Me.tx_F1検索H2.InputMinus = False
        Me.tx_F1検索H2.InputPlus = True
        Me.tx_F1検索H2.InputZero = False
        Me.tx_F1検索H2.Location = New System.Drawing.Point(629, 63)
        Me.tx_F1検索H2.MaxLength = 4
        Me.tx_F1検索H2.Name = "tx_F1検索H2"
        Me.tx_F1検索H2.OldValue = "9999"
        Me.tx_F1検索H2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_F1検索H2.SelectText = True
        Me.tx_F1検索H2.SelLength = 0
        Me.tx_F1検索H2.SelStart = 0
        Me.tx_F1検索H2.SelText = ""
        Me.tx_F1検索H2.Size = New System.Drawing.Size(52, 22)
        Me.tx_F1検索H2.TabIndex = 16
        Me.tx_F1検索H2.Text = "9999"
        '
        'tx_F1検索仕様e
        '
        Me.tx_F1検索仕様e.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_F1検索仕様e.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_F1検索仕様e.CanForwardSetFocus = True
        Me.tx_F1検索仕様e.CanNextSetFocus = True
        Me.tx_F1検索仕様e.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_F1検索仕様e.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_F1検索仕様e.EditMode = True
        Me.tx_F1検索仕様e.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_F1検索仕様e.IMEMode = System.Windows.Forms.ImeMode.KatakanaHalf
        Me.tx_F1検索仕様e.LengthType = ExText.ExTextBox.LenType.ByteType
        Me.tx_F1検索仕様e.Location = New System.Drawing.Point(200, 21)
        Me.tx_F1検索仕様e.MaxLength = 7
        Me.tx_F1検索仕様e.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_F1検索仕様e.Name = "tx_F1検索仕様e"
        Me.tx_F1検索仕様e.OldValue = "ExTextBox"
        Me.tx_F1検索仕様e.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_F1検索仕様e.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_F1検索仕様e.SelectText = True
        Me.tx_F1検索仕様e.SelLength = 0
        Me.tx_F1検索仕様e.SelStart = 0
        Me.tx_F1検索仕様e.SelText = ""
        Me.tx_F1検索仕様e.Size = New System.Drawing.Size(79, 22)
        Me.tx_F1検索仕様e.TabIndex = 7
        Me.tx_F1検索仕様e.Text = "ExTextBox"
        '
        '_lb_kara_2
        '
        Me._lb_kara_2.BackColor = System.Drawing.SystemColors.Control
        Me._lb_kara_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_kara_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_kara_2.Location = New System.Drawing.Point(180, 26)
        Me._lb_kara_2.Name = "_lb_kara_2"
        Me._lb_kara_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_kara_2.Size = New System.Drawing.Size(17, 12)
        Me._lb_kara_2.TabIndex = 70
        Me._lb_kara_2.Text = "～"
        Me._lb_kara_2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_23
        '
        Me._lb_項目_23.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_23.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_23.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_23.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_23.ForeColor = System.Drawing.Color.White
        Me._lb_項目_23.Location = New System.Drawing.Point(540, 65)
        Me._lb_項目_23.Name = "_lb_項目_23"
        Me._lb_項目_23.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_23.Size = New System.Drawing.Size(89, 20)
        Me._lb_項目_23.TabIndex = 69
        Me._lb_項目_23.Text = "Ｈ２"
        Me._lb_項目_23.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_22
        '
        Me._lb_項目_22.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_22.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_22.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_22.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_22.ForeColor = System.Drawing.Color.White
        Me._lb_項目_22.Location = New System.Drawing.Point(540, 44)
        Me._lb_項目_22.Name = "_lb_項目_22"
        Me._lb_項目_22.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_22.Size = New System.Drawing.Size(89, 20)
        Me._lb_項目_22.TabIndex = 68
        Me._lb_項目_22.Text = "Ｈ１"
        Me._lb_項目_22.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_16
        '
        Me._lb_項目_16.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_16.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_16.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_16.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_16.ForeColor = System.Drawing.Color.White
        Me._lb_項目_16.Location = New System.Drawing.Point(380, 1)
        Me._lb_項目_16.Name = "_lb_項目_16"
        Me._lb_項目_16.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_16.Size = New System.Drawing.Size(89, 20)
        Me._lb_項目_16.TabIndex = 62
        Me._lb_項目_16.Text = "Ｗ"
        Me._lb_項目_16.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_15
        '
        Me._lb_項目_15.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_15.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_15.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_15.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_15.ForeColor = System.Drawing.Color.White
        Me._lb_項目_15.Location = New System.Drawing.Point(380, 23)
        Me._lb_項目_15.Name = "_lb_項目_15"
        Me._lb_項目_15.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_15.Size = New System.Drawing.Size(89, 20)
        Me._lb_項目_15.TabIndex = 61
        Me._lb_項目_15.Text = "Ｄ"
        Me._lb_項目_15.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_14
        '
        Me._lb_項目_14.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_14.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_14.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_14.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_14.ForeColor = System.Drawing.Color.White
        Me._lb_項目_14.Location = New System.Drawing.Point(380, 44)
        Me._lb_項目_14.Name = "_lb_項目_14"
        Me._lb_項目_14.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_14.Size = New System.Drawing.Size(89, 21)
        Me._lb_項目_14.TabIndex = 60
        Me._lb_項目_14.Text = "Ｈ"
        Me._lb_項目_14.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_13
        '
        Me._lb_項目_13.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_13.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_13.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_13.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_13.ForeColor = System.Drawing.Color.White
        Me._lb_項目_13.Location = New System.Drawing.Point(540, 1)
        Me._lb_項目_13.Name = "_lb_項目_13"
        Me._lb_項目_13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_13.Size = New System.Drawing.Size(89, 20)
        Me._lb_項目_13.TabIndex = 59
        Me._lb_項目_13.Text = "Ｄ１"
        Me._lb_項目_13.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_12
        '
        Me._lb_項目_12.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_12.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_12.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_12.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_12.ForeColor = System.Drawing.Color.White
        Me._lb_項目_12.Location = New System.Drawing.Point(540, 23)
        Me._lb_項目_12.Name = "_lb_項目_12"
        Me._lb_項目_12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_12.Size = New System.Drawing.Size(89, 20)
        Me._lb_項目_12.TabIndex = 58
        Me._lb_項目_12.Text = "Ｄ２"
        Me._lb_項目_12.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_0
        '
        Me._lb_項目_0.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_0.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_0.ForeColor = System.Drawing.Color.White
        Me._lb_項目_0.Location = New System.Drawing.Point(0, 1)
        Me._lb_項目_0.Name = "_lb_項目_0"
        Me._lb_項目_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_0.Size = New System.Drawing.Size(89, 20)
        Me._lb_項目_0.TabIndex = 41
        Me._lb_項目_0.Text = "製品NO"
        Me._lb_項目_0.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_3
        '
        Me._lb_項目_3.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_3.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_3.ForeColor = System.Drawing.Color.White
        Me._lb_項目_3.Location = New System.Drawing.Point(0, 65)
        Me._lb_項目_3.Name = "_lb_項目_3"
        Me._lb_項目_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_3.Size = New System.Drawing.Size(89, 20)
        Me._lb_項目_3.TabIndex = 40
        Me._lb_項目_3.Text = "主仕入先"
        Me._lb_項目_3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_2
        '
        Me._lb_項目_2.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_2.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_2.ForeColor = System.Drawing.Color.White
        Me._lb_項目_2.Location = New System.Drawing.Point(0, 44)
        Me._lb_項目_2.Name = "_lb_項目_2"
        Me._lb_項目_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_2.Size = New System.Drawing.Size(89, 20)
        Me._lb_項目_2.TabIndex = 38
        Me._lb_項目_2.Text = "漢字名称"
        Me._lb_項目_2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_1
        '
        Me._lb_項目_1.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_1.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_1.ForeColor = System.Drawing.Color.White
        Me._lb_項目_1.Location = New System.Drawing.Point(0, 22)
        Me._lb_項目_1.Name = "_lb_項目_1"
        Me._lb_項目_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_1.Size = New System.Drawing.Size(89, 20)
        Me._lb_項目_1.TabIndex = 37
        Me._lb_項目_1.Text = "仕様NO"
        Me._lb_項目_1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'PicFind_1
        '
        Me.PicFind_1.BackColor = System.Drawing.SystemColors.Control
        Me.PicFind_1.Controls.Add(Me.tx_F2検索品群)
        Me.PicFind_1.Controls.Add(Me.tx_F2検索名称)
        Me.PicFind_1.Controls.Add(Me._lb_項目_4)
        Me.PicFind_1.Controls.Add(Me._lb_項目_5)
        Me.PicFind_1.Cursor = System.Windows.Forms.Cursors.Default
        Me.PicFind_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.PicFind_1.Location = New System.Drawing.Point(64, 187)
        Me.PicFind_1.Name = "PicFind_1"
        Me.PicFind_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.PicFind_1.Size = New System.Drawing.Size(691, 96)
        Me.PicFind_1.TabIndex = 17
        '
        'tx_F2検索品群
        '
        Me.tx_F2検索品群.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_F2検索品群.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_F2検索品群.CanForwardSetFocus = True
        Me.tx_F2検索品群.CanNextSetFocus = True
        Me.tx_F2検索品群.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_F2検索品群.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_F2検索品群.EditMode = True
        Me.tx_F2検索品群.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_F2検索品群.IMEMode = System.Windows.Forms.ImeMode.Disable
        Me.tx_F2検索品群.LengthType = ExText.ExTextBox.LenType.ByteType
        Me.tx_F2検索品群.Location = New System.Drawing.Point(89, 0)
        Me.tx_F2検索品群.MaxLength = 7
        Me.tx_F2検索品群.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_F2検索品群.Name = "tx_F2検索品群"
        Me.tx_F2検索品群.OldValue = "ExTextBox"
        Me.tx_F2検索品群.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_F2検索品群.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_F2検索品群.SelectText = True
        Me.tx_F2検索品群.SelLength = 0
        Me.tx_F2検索品群.SelStart = 0
        Me.tx_F2検索品群.SelText = ""
        Me.tx_F2検索品群.Size = New System.Drawing.Size(67, 22)
        Me.tx_F2検索品群.TabIndex = 18
        Me.tx_F2検索品群.Text = "ExTextBox"
        '
        'tx_F2検索名称
        '
        Me.tx_F2検索名称.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_F2検索名称.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_F2検索名称.CanForwardSetFocus = True
        Me.tx_F2検索名称.CanNextSetFocus = True
        Me.tx_F2検索名称.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_F2検索名称.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_F2検索名称.EditMode = True
        Me.tx_F2検索名称.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_F2検索名称.IMEMode = System.Windows.Forms.ImeMode.Hiragana
        Me.tx_F2検索名称.LengthType = ExText.ExTextBox.LenType.ByteType
        Me.tx_F2検索名称.Location = New System.Drawing.Point(89, 21)
        Me.tx_F2検索名称.MaxLength = 40
        Me.tx_F2検索名称.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_F2検索名称.Name = "tx_F2検索名称"
        Me.tx_F2検索名称.OldValue = "ExTextBox"
        Me.tx_F2検索名称.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_F2検索名称.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_F2検索名称.SelectText = True
        Me.tx_F2検索名称.SelLength = 0
        Me.tx_F2検索名称.SelStart = 0
        Me.tx_F2検索名称.SelText = ""
        Me.tx_F2検索名称.Size = New System.Drawing.Size(267, 22)
        Me.tx_F2検索名称.TabIndex = 19
        Me.tx_F2検索名称.Text = "ExTextBox"
        '
        '_lb_項目_4
        '
        Me._lb_項目_4.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_4.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_4.ForeColor = System.Drawing.Color.White
        Me._lb_項目_4.Location = New System.Drawing.Point(0, 21)
        Me._lb_項目_4.Name = "_lb_項目_4"
        Me._lb_項目_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_4.Size = New System.Drawing.Size(89, 19)
        Me._lb_項目_4.TabIndex = 49
        Me._lb_項目_4.Text = "漢字名称"
        Me._lb_項目_4.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_5
        '
        Me._lb_項目_5.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_5.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_5.ForeColor = System.Drawing.Color.White
        Me._lb_項目_5.Location = New System.Drawing.Point(0, 0)
        Me._lb_項目_5.Name = "_lb_項目_5"
        Me._lb_項目_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_5.Size = New System.Drawing.Size(89, 19)
        Me._lb_項目_5.TabIndex = 47
        Me._lb_項目_5.Text = "品群NO"
        Me._lb_項目_5.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'PicFind_2
        '
        Me.PicFind_2.BackColor = System.Drawing.SystemColors.Control
        Me.PicFind_2.Controls.Add(Me.tx_F3検索ユニット)
        Me.PicFind_2.Controls.Add(Me.tx_F3検索名称)
        Me.PicFind_2.Controls.Add(Me._lb_項目_8)
        Me.PicFind_2.Controls.Add(Me._lb_項目_9)
        Me.PicFind_2.Cursor = System.Windows.Forms.Cursors.Default
        Me.PicFind_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.PicFind_2.Location = New System.Drawing.Point(64, 287)
        Me.PicFind_2.Name = "PicFind_2"
        Me.PicFind_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.PicFind_2.Size = New System.Drawing.Size(691, 96)
        Me.PicFind_2.TabIndex = 20
        '
        'tx_F3検索ユニット
        '
        Me.tx_F3検索ユニット.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_F3検索ユニット.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_F3検索ユニット.CanForwardSetFocus = True
        Me.tx_F3検索ユニット.CanNextSetFocus = True
        Me.tx_F3検索ユニット.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_F3検索ユニット.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_F3検索ユニット.EditMode = True
        Me.tx_F3検索ユニット.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_F3検索ユニット.IMEMode = System.Windows.Forms.ImeMode.Disable
        Me.tx_F3検索ユニット.LengthType = ExText.ExTextBox.LenType.ByteType
        Me.tx_F3検索ユニット.Location = New System.Drawing.Point(89, 0)
        Me.tx_F3検索ユニット.MaxLength = 7
        Me.tx_F3検索ユニット.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_F3検索ユニット.Name = "tx_F3検索ユニット"
        Me.tx_F3検索ユニット.OldValue = "ExTextBox"
        Me.tx_F3検索ユニット.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_F3検索ユニット.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_F3検索ユニット.SelectText = True
        Me.tx_F3検索ユニット.SelLength = 0
        Me.tx_F3検索ユニット.SelStart = 0
        Me.tx_F3検索ユニット.SelText = ""
        Me.tx_F3検索ユニット.Size = New System.Drawing.Size(67, 22)
        Me.tx_F3検索ユニット.TabIndex = 21
        Me.tx_F3検索ユニット.Text = "ExTextBox"
        '
        'tx_F3検索名称
        '
        Me.tx_F3検索名称.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_F3検索名称.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_F3検索名称.CanForwardSetFocus = True
        Me.tx_F3検索名称.CanNextSetFocus = True
        Me.tx_F3検索名称.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_F3検索名称.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_F3検索名称.EditMode = True
        Me.tx_F3検索名称.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_F3検索名称.IMEMode = System.Windows.Forms.ImeMode.Hiragana
        Me.tx_F3検索名称.LengthType = ExText.ExTextBox.LenType.ByteType
        Me.tx_F3検索名称.Location = New System.Drawing.Point(89, 21)
        Me.tx_F3検索名称.MaxLength = 40
        Me.tx_F3検索名称.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_F3検索名称.Name = "tx_F3検索名称"
        Me.tx_F3検索名称.OldValue = "ExTextBox"
        Me.tx_F3検索名称.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_F3検索名称.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_F3検索名称.SelectText = True
        Me.tx_F3検索名称.SelLength = 0
        Me.tx_F3検索名称.SelStart = 0
        Me.tx_F3検索名称.SelText = ""
        Me.tx_F3検索名称.Size = New System.Drawing.Size(267, 22)
        Me.tx_F3検索名称.TabIndex = 22
        Me.tx_F3検索名称.Text = "ExTextBox"
        '
        '_lb_項目_8
        '
        Me._lb_項目_8.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_8.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_8.ForeColor = System.Drawing.Color.White
        Me._lb_項目_8.Location = New System.Drawing.Point(0, 21)
        Me._lb_項目_8.Name = "_lb_項目_8"
        Me._lb_項目_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_8.Size = New System.Drawing.Size(89, 19)
        Me._lb_項目_8.TabIndex = 51
        Me._lb_項目_8.Text = "漢字名称"
        Me._lb_項目_8.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_9
        '
        Me._lb_項目_9.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_9.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_9.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_9.ForeColor = System.Drawing.Color.White
        Me._lb_項目_9.Location = New System.Drawing.Point(0, 0)
        Me._lb_項目_9.Name = "_lb_項目_9"
        Me._lb_項目_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_9.Size = New System.Drawing.Size(89, 19)
        Me._lb_項目_9.TabIndex = 50
        Me._lb_項目_9.Text = "ユニットNO"
        Me._lb_項目_9.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'PicFind_3
        '
        Me.PicFind_3.BackColor = System.Drawing.SystemColors.Control
        Me.PicFind_3.Controls.Add(Me.tx_F4検索製品)
        Me.PicFind_3.Controls.Add(Me.tx_F4検索名称)
        Me.PicFind_3.Controls.Add(Me.tx_F4検索PC区分)
        Me.PicFind_3.Controls.Add(Me.tx_F4検索仕入先)
        Me.PicFind_3.Controls.Add(Me.tx_F4検索W)
        Me.PicFind_3.Controls.Add(Me.tx_F4検索D)
        Me.PicFind_3.Controls.Add(Me.tx_F4検索H)
        Me.PicFind_3.Controls.Add(Me.tx_F4検索径)
        Me.PicFind_3.Controls.Add(Me.tx_F4検索T)
        Me.PicFind_3.Controls.Add(Me._lb_項目_21)
        Me.PicFind_3.Controls.Add(Me._lb_項目_20)
        Me.PicFind_3.Controls.Add(Me._lb_項目_19)
        Me.PicFind_3.Controls.Add(Me._lb_項目_18)
        Me.PicFind_3.Controls.Add(Me._lb_項目_17)
        Me.PicFind_3.Controls.Add(Me._lb_項目_6)
        Me.PicFind_3.Controls.Add(Me._lb_項目_7)
        Me.PicFind_3.Controls.Add(Me._lb_項目_10)
        Me.PicFind_3.Controls.Add(Me._lb_項目_11)
        Me.PicFind_3.Cursor = System.Windows.Forms.Cursors.Default
        Me.PicFind_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.PicFind_3.Location = New System.Drawing.Point(64, 387)
        Me.PicFind_3.Name = "PicFind_3"
        Me.PicFind_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.PicFind_3.Size = New System.Drawing.Size(691, 96)
        Me.PicFind_3.TabIndex = 23
        '
        'tx_F4検索製品
        '
        Me.tx_F4検索製品.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_F4検索製品.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_F4検索製品.CanForwardSetFocus = True
        Me.tx_F4検索製品.CanNextSetFocus = True
        Me.tx_F4検索製品.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_F4検索製品.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_F4検索製品.EditMode = True
        Me.tx_F4検索製品.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_F4検索製品.IMEMode = System.Windows.Forms.ImeMode.Disable
        Me.tx_F4検索製品.LengthType = ExText.ExTextBox.LenType.ByteType
        Me.tx_F4検索製品.Location = New System.Drawing.Point(89, 19)
        Me.tx_F4検索製品.MaxLength = 7
        Me.tx_F4検索製品.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_F4検索製品.Name = "tx_F4検索製品"
        Me.tx_F4検索製品.OldValue = "ExTextBox"
        Me.tx_F4検索製品.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_F4検索製品.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_F4検索製品.SelectText = True
        Me.tx_F4検索製品.SelLength = 0
        Me.tx_F4検索製品.SelStart = 0
        Me.tx_F4検索製品.SelText = ""
        Me.tx_F4検索製品.Size = New System.Drawing.Size(67, 22)
        Me.tx_F4検索製品.TabIndex = 25
        Me.tx_F4検索製品.Text = "ExTextBox"
        '
        'tx_F4検索名称
        '
        Me.tx_F4検索名称.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_F4検索名称.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_F4検索名称.CanForwardSetFocus = True
        Me.tx_F4検索名称.CanNextSetFocus = True
        Me.tx_F4検索名称.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_F4検索名称.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_F4検索名称.EditMode = True
        Me.tx_F4検索名称.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_F4検索名称.IMEMode = System.Windows.Forms.ImeMode.Hiragana
        Me.tx_F4検索名称.LengthType = ExText.ExTextBox.LenType.ByteType
        Me.tx_F4検索名称.Location = New System.Drawing.Point(89, 40)
        Me.tx_F4検索名称.MaxLength = 40
        Me.tx_F4検索名称.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_F4検索名称.Name = "tx_F4検索名称"
        Me.tx_F4検索名称.OldValue = "ExTextBox"
        Me.tx_F4検索名称.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_F4検索名称.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_F4検索名称.SelectText = True
        Me.tx_F4検索名称.SelLength = 0
        Me.tx_F4検索名称.SelStart = 0
        Me.tx_F4検索名称.SelText = ""
        Me.tx_F4検索名称.Size = New System.Drawing.Size(267, 22)
        Me.tx_F4検索名称.TabIndex = 26
        Me.tx_F4検索名称.Text = "ExTextBox"
        '
        'tx_F4検索PC区分
        '
        Me.tx_F4検索PC区分.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_F4検索PC区分.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_F4検索PC区分.CanForwardSetFocus = True
        Me.tx_F4検索PC区分.CanNextSetFocus = True
        Me.tx_F4検索PC区分.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_F4検索PC区分.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_F4検索PC区分.EditMode = True
        Me.tx_F4検索PC区分.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_F4検索PC区分.IMEMode = System.Windows.Forms.ImeMode.Disable
        Me.tx_F4検索PC区分.LengthType = ExText.ExTextBox.LenType.ByteType
        Me.tx_F4検索PC区分.Location = New System.Drawing.Point(89, 0)
        Me.tx_F4検索PC区分.MaxLength = 7
        Me.tx_F4検索PC区分.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_F4検索PC区分.Name = "tx_F4検索PC区分"
        Me.tx_F4検索PC区分.OldValue = "ExTextBox"
        Me.tx_F4検索PC区分.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_F4検索PC区分.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_F4検索PC区分.SelectText = True
        Me.tx_F4検索PC区分.SelLength = 0
        Me.tx_F4検索PC区分.SelStart = 0
        Me.tx_F4検索PC区分.SelText = ""
        Me.tx_F4検索PC区分.Size = New System.Drawing.Size(67, 22)
        Me.tx_F4検索PC区分.TabIndex = 24
        Me.tx_F4検索PC区分.Text = "ExTextBox"
        '
        'tx_F4検索仕入先
        '
        Me.tx_F4検索仕入先.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_F4検索仕入先.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_F4検索仕入先.CanForwardSetFocus = True
        Me.tx_F4検索仕入先.CanNextSetFocus = True
        Me.tx_F4検索仕入先.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_F4検索仕入先.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_F4検索仕入先.EditMode = True
        Me.tx_F4検索仕入先.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_F4検索仕入先.IMEMode = System.Windows.Forms.ImeMode.Disable
        Me.tx_F4検索仕入先.LengthType = ExText.ExTextBox.LenType.ByteType
        Me.tx_F4検索仕入先.Location = New System.Drawing.Point(89, 60)
        Me.tx_F4検索仕入先.MaxLength = 4
        Me.tx_F4検索仕入先.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_F4検索仕入先.Name = "tx_F4検索仕入先"
        Me.tx_F4検索仕入先.OldValue = "ExTextBox"
        Me.tx_F4検索仕入先.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_F4検索仕入先.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_F4検索仕入先.SelectText = True
        Me.tx_F4検索仕入先.SelLength = 0
        Me.tx_F4検索仕入先.SelStart = 0
        Me.tx_F4検索仕入先.SelText = ""
        Me.tx_F4検索仕入先.Size = New System.Drawing.Size(67, 22)
        Me.tx_F4検索仕入先.TabIndex = 27
        Me.tx_F4検索仕入先.Text = "ExTextBox"
        '
        'tx_F4検索W
        '
        Me.tx_F4検索W.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_F4検索W.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_F4検索W.CanForwardSetFocus = True
        Me.tx_F4検索W.CanNextSetFocus = True
        Me.tx_F4検索W.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_F4検索W.DecimalPlace = CType(0, Short)
        Me.tx_F4検索W.EditMode = True
        Me.tx_F4検索W.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_F4検索W.FormatType = ""
        Me.tx_F4検索W.FormatTypeNega = ""
        Me.tx_F4検索W.FormatTypeNull = ""
        Me.tx_F4検索W.FormatTypeZero = ""
        Me.tx_F4検索W.InputMinus = False
        Me.tx_F4検索W.InputPlus = True
        Me.tx_F4検索W.InputZero = False
        Me.tx_F4検索W.Location = New System.Drawing.Point(469, 0)
        Me.tx_F4検索W.MaxLength = 4
        Me.tx_F4検索W.Name = "tx_F4検索W"
        Me.tx_F4検索W.OldValue = "9999"
        Me.tx_F4検索W.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_F4検索W.SelectText = True
        Me.tx_F4検索W.SelLength = 0
        Me.tx_F4検索W.SelStart = 0
        Me.tx_F4検索W.SelText = ""
        Me.tx_F4検索W.Size = New System.Drawing.Size(52, 22)
        Me.tx_F4検索W.TabIndex = 28
        Me.tx_F4検索W.Text = "9999"
        '
        'tx_F4検索D
        '
        Me.tx_F4検索D.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_F4検索D.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_F4検索D.CanForwardSetFocus = True
        Me.tx_F4検索D.CanNextSetFocus = True
        Me.tx_F4検索D.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_F4検索D.DecimalPlace = CType(0, Short)
        Me.tx_F4検索D.EditMode = True
        Me.tx_F4検索D.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_F4検索D.FormatType = ""
        Me.tx_F4検索D.FormatTypeNega = ""
        Me.tx_F4検索D.FormatTypeNull = ""
        Me.tx_F4検索D.FormatTypeZero = ""
        Me.tx_F4検索D.InputMinus = False
        Me.tx_F4検索D.InputPlus = True
        Me.tx_F4検索D.InputZero = False
        Me.tx_F4検索D.Location = New System.Drawing.Point(469, 19)
        Me.tx_F4検索D.MaxLength = 4
        Me.tx_F4検索D.Name = "tx_F4検索D"
        Me.tx_F4検索D.OldValue = "9999"
        Me.tx_F4検索D.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_F4検索D.SelectText = True
        Me.tx_F4検索D.SelLength = 0
        Me.tx_F4検索D.SelStart = 0
        Me.tx_F4検索D.SelText = ""
        Me.tx_F4検索D.Size = New System.Drawing.Size(52, 22)
        Me.tx_F4検索D.TabIndex = 29
        Me.tx_F4検索D.Text = "9999"
        '
        'tx_F4検索H
        '
        Me.tx_F4検索H.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_F4検索H.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_F4検索H.CanForwardSetFocus = True
        Me.tx_F4検索H.CanNextSetFocus = True
        Me.tx_F4検索H.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_F4検索H.DecimalPlace = CType(0, Short)
        Me.tx_F4検索H.EditMode = True
        Me.tx_F4検索H.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_F4検索H.FormatType = ""
        Me.tx_F4検索H.FormatTypeNega = ""
        Me.tx_F4検索H.FormatTypeNull = ""
        Me.tx_F4検索H.FormatTypeZero = ""
        Me.tx_F4検索H.InputMinus = False
        Me.tx_F4検索H.InputPlus = True
        Me.tx_F4検索H.InputZero = False
        Me.tx_F4検索H.Location = New System.Drawing.Point(469, 38)
        Me.tx_F4検索H.MaxLength = 4
        Me.tx_F4検索H.Name = "tx_F4検索H"
        Me.tx_F4検索H.OldValue = "9999"
        Me.tx_F4検索H.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_F4検索H.SelectText = True
        Me.tx_F4検索H.SelLength = 0
        Me.tx_F4検索H.SelStart = 0
        Me.tx_F4検索H.SelText = ""
        Me.tx_F4検索H.Size = New System.Drawing.Size(52, 22)
        Me.tx_F4検索H.TabIndex = 30
        Me.tx_F4検索H.Text = "9999"
        '
        'tx_F4検索径
        '
        Me.tx_F4検索径.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_F4検索径.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_F4検索径.CanForwardSetFocus = True
        Me.tx_F4検索径.CanNextSetFocus = True
        Me.tx_F4検索径.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_F4検索径.DecimalPlace = CType(0, Short)
        Me.tx_F4検索径.EditMode = True
        Me.tx_F4検索径.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_F4検索径.FormatType = ""
        Me.tx_F4検索径.FormatTypeNega = ""
        Me.tx_F4検索径.FormatTypeNull = ""
        Me.tx_F4検索径.FormatTypeZero = ""
        Me.tx_F4検索径.InputMinus = False
        Me.tx_F4検索径.InputPlus = True
        Me.tx_F4検索径.InputZero = False
        Me.tx_F4検索径.Location = New System.Drawing.Point(469, 57)
        Me.tx_F4検索径.MaxLength = 4
        Me.tx_F4検索径.Name = "tx_F4検索径"
        Me.tx_F4検索径.OldValue = "9999"
        Me.tx_F4検索径.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_F4検索径.SelectText = True
        Me.tx_F4検索径.SelLength = 0
        Me.tx_F4検索径.SelStart = 0
        Me.tx_F4検索径.SelText = ""
        Me.tx_F4検索径.Size = New System.Drawing.Size(52, 22)
        Me.tx_F4検索径.TabIndex = 31
        Me.tx_F4検索径.Text = "9999"
        '
        'tx_F4検索T
        '
        Me.tx_F4検索T.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_F4検索T.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_F4検索T.CanForwardSetFocus = True
        Me.tx_F4検索T.CanNextSetFocus = True
        Me.tx_F4検索T.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_F4検索T.DecimalPlace = CType(0, Short)
        Me.tx_F4検索T.EditMode = True
        Me.tx_F4検索T.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_F4検索T.FormatType = ""
        Me.tx_F4検索T.FormatTypeNega = ""
        Me.tx_F4検索T.FormatTypeNull = ""
        Me.tx_F4検索T.FormatTypeZero = ""
        Me.tx_F4検索T.InputMinus = False
        Me.tx_F4検索T.InputPlus = True
        Me.tx_F4検索T.InputZero = False
        Me.tx_F4検索T.Location = New System.Drawing.Point(469, 76)
        Me.tx_F4検索T.MaxLength = 4
        Me.tx_F4検索T.Name = "tx_F4検索T"
        Me.tx_F4検索T.OldValue = "9999"
        Me.tx_F4検索T.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_F4検索T.SelectText = True
        Me.tx_F4検索T.SelLength = 0
        Me.tx_F4検索T.SelStart = 0
        Me.tx_F4検索T.SelText = ""
        Me.tx_F4検索T.Size = New System.Drawing.Size(52, 22)
        Me.tx_F4検索T.TabIndex = 32
        Me.tx_F4検索T.Text = "9999"
        '
        '_lb_項目_21
        '
        Me._lb_項目_21.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_21.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_21.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_21.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_21.ForeColor = System.Drawing.Color.White
        Me._lb_項目_21.Location = New System.Drawing.Point(380, 0)
        Me._lb_項目_21.Name = "_lb_項目_21"
        Me._lb_項目_21.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_21.Size = New System.Drawing.Size(89, 19)
        Me._lb_項目_21.TabIndex = 67
        Me._lb_項目_21.Text = "Ｗ"
        Me._lb_項目_21.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_20
        '
        Me._lb_項目_20.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_20.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_20.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_20.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_20.ForeColor = System.Drawing.Color.White
        Me._lb_項目_20.Location = New System.Drawing.Point(380, 19)
        Me._lb_項目_20.Name = "_lb_項目_20"
        Me._lb_項目_20.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_20.Size = New System.Drawing.Size(89, 19)
        Me._lb_項目_20.TabIndex = 66
        Me._lb_項目_20.Text = "Ｄ"
        Me._lb_項目_20.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_19
        '
        Me._lb_項目_19.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_19.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_19.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_19.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_19.ForeColor = System.Drawing.Color.White
        Me._lb_項目_19.Location = New System.Drawing.Point(380, 38)
        Me._lb_項目_19.Name = "_lb_項目_19"
        Me._lb_項目_19.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_19.Size = New System.Drawing.Size(89, 19)
        Me._lb_項目_19.TabIndex = 65
        Me._lb_項目_19.Text = "Ｈ"
        Me._lb_項目_19.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_18
        '
        Me._lb_項目_18.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_18.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_18.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_18.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_18.ForeColor = System.Drawing.Color.White
        Me._lb_項目_18.Location = New System.Drawing.Point(380, 57)
        Me._lb_項目_18.Name = "_lb_項目_18"
        Me._lb_項目_18.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_18.Size = New System.Drawing.Size(89, 19)
        Me._lb_項目_18.TabIndex = 64
        Me._lb_項目_18.Text = "径"
        Me._lb_項目_18.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_17
        '
        Me._lb_項目_17.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_17.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_17.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_17.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_17.ForeColor = System.Drawing.Color.White
        Me._lb_項目_17.Location = New System.Drawing.Point(380, 76)
        Me._lb_項目_17.Name = "_lb_項目_17"
        Me._lb_項目_17.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_17.Size = New System.Drawing.Size(89, 19)
        Me._lb_項目_17.TabIndex = 63
        Me._lb_項目_17.Text = "Ｔ"
        Me._lb_項目_17.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_6
        '
        Me._lb_項目_6.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_6.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_6.ForeColor = System.Drawing.Color.White
        Me._lb_項目_6.Location = New System.Drawing.Point(0, 60)
        Me._lb_項目_6.Name = "_lb_項目_6"
        Me._lb_項目_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_6.Size = New System.Drawing.Size(89, 19)
        Me._lb_項目_6.TabIndex = 57
        Me._lb_項目_6.Text = "主仕入先"
        Me._lb_項目_6.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_7
        '
        Me._lb_項目_7.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_7.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_7.ForeColor = System.Drawing.Color.White
        Me._lb_項目_7.Location = New System.Drawing.Point(0, 40)
        Me._lb_項目_7.Name = "_lb_項目_7"
        Me._lb_項目_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_7.Size = New System.Drawing.Size(89, 19)
        Me._lb_項目_7.TabIndex = 54
        Me._lb_項目_7.Text = "漢字名称"
        Me._lb_項目_7.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_10
        '
        Me._lb_項目_10.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_10.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_10.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_10.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_10.ForeColor = System.Drawing.Color.White
        Me._lb_項目_10.Location = New System.Drawing.Point(0, 19)
        Me._lb_項目_10.Name = "_lb_項目_10"
        Me._lb_項目_10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_10.Size = New System.Drawing.Size(89, 19)
        Me._lb_項目_10.TabIndex = 53
        Me._lb_項目_10.Text = "製品NO"
        Me._lb_項目_10.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_11
        '
        Me._lb_項目_11.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_11.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_11.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_11.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_11.ForeColor = System.Drawing.Color.White
        Me._lb_項目_11.Location = New System.Drawing.Point(0, 0)
        Me._lb_項目_11.Name = "_lb_項目_11"
        Me._lb_項目_11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_11.Size = New System.Drawing.Size(89, 19)
        Me._lb_項目_11.TabIndex = 52
        Me._lb_項目_11.Text = "ＰＣ区分"
        Me._lb_項目_11.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'cmdChkOn
        '
        Me.cmdChkOn.BackColor = System.Drawing.SystemColors.Control
        Me.cmdChkOn.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdChkOn.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Bold)
        Me.cmdChkOn.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdChkOn.Location = New System.Drawing.Point(10, 527)
        Me.cmdChkOn.Name = "cmdChkOn"
        Me.cmdChkOn.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdChkOn.Size = New System.Drawing.Size(83, 25)
        Me.cmdChkOn.TabIndex = 56
        Me.cmdChkOn.Text = "ALL選択"
        Me.cmdChkOn.UseVisualStyleBackColor = False
        '
        'cmdChkOff
        '
        Me.cmdChkOff.BackColor = System.Drawing.SystemColors.Control
        Me.cmdChkOff.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdChkOff.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Bold)
        Me.cmdChkOff.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdChkOff.Location = New System.Drawing.Point(93, 527)
        Me.cmdChkOff.Name = "cmdChkOff"
        Me.cmdChkOff.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdChkOff.Size = New System.Drawing.Size(83, 25)
        Me.cmdChkOff.TabIndex = 55
        Me.cmdChkOff.Text = "ALL解除"
        Me.cmdChkOff.UseVisualStyleBackColor = False
        '
        'ck_Find_3
        '
        Me.ck_Find_3.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ck_Find_3.Cursor = System.Windows.Forms.Cursors.Default
        Me.ck_Find_3.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.ck_Find_3.ForeColor = System.Drawing.Color.Black
        Me.ck_Find_3.Location = New System.Drawing.Point(287, 48)
        Me.ck_Find_3.Name = "ck_Find_3"
        Me.ck_Find_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ck_Find_3.Size = New System.Drawing.Size(18, 18)
        Me.ck_Find_3.TabIndex = 3
        Me.ck_Find_3.TabStop = False
        Me.ck_Find_3.UseVisualStyleBackColor = False
        '
        'ck_Find_2
        '
        Me.ck_Find_2.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ck_Find_2.Cursor = System.Windows.Forms.Cursors.Default
        Me.ck_Find_2.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.ck_Find_2.ForeColor = System.Drawing.Color.Black
        Me.ck_Find_2.Location = New System.Drawing.Point(194, 48)
        Me.ck_Find_2.Name = "ck_Find_2"
        Me.ck_Find_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ck_Find_2.Size = New System.Drawing.Size(18, 18)
        Me.ck_Find_2.TabIndex = 2
        Me.ck_Find_2.TabStop = False
        Me.ck_Find_2.UseVisualStyleBackColor = False
        '
        'ck_Find_1
        '
        Me.ck_Find_1.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ck_Find_1.Cursor = System.Windows.Forms.Cursors.Default
        Me.ck_Find_1.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.ck_Find_1.ForeColor = System.Drawing.Color.Black
        Me.ck_Find_1.Location = New System.Drawing.Point(103, 48)
        Me.ck_Find_1.Name = "ck_Find_1"
        Me.ck_Find_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ck_Find_1.Size = New System.Drawing.Size(18, 18)
        Me.ck_Find_1.TabIndex = 1
        Me.ck_Find_1.TabStop = False
        Me.ck_Find_1.UseVisualStyleBackColor = False
        '
        'ck_Find_0
        '
        Me.ck_Find_0.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ck_Find_0.Cursor = System.Windows.Forms.Cursors.Default
        Me.ck_Find_0.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.ck_Find_0.ForeColor = System.Drawing.Color.Black
        Me.ck_Find_0.Location = New System.Drawing.Point(14, 48)
        Me.ck_Find_0.Name = "ck_Find_0"
        Me.ck_Find_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ck_Find_0.Size = New System.Drawing.Size(18, 18)
        Me.ck_Find_0.TabIndex = 0
        Me.ck_Find_0.TabStop = False
        Me.ck_Find_0.UseVisualStyleBackColor = False
        '
        'CmdCan
        '
        Me.CmdCan.BackColor = System.Drawing.SystemColors.Control
        Me.CmdCan.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmdCan.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CmdCan.Font = New System.Drawing.Font("MS UI Gothic", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CmdCan.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdCan.Location = New System.Drawing.Point(437, 527)
        Me.CmdCan.Name = "CmdCan"
        Me.CmdCan.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdCan.Size = New System.Drawing.Size(109, 25)
        Me.CmdCan.TabIndex = 36
        Me.CmdCan.Text = "ｷｬﾝｾﾙ(&C)"
        Me.CmdCan.UseVisualStyleBackColor = False
        '
        'CmdOk
        '
        Me.CmdOk.BackColor = System.Drawing.SystemColors.Control
        Me.CmdOk.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmdOk.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Bold)
        Me.CmdOk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdOk.Location = New System.Drawing.Point(329, 527)
        Me.CmdOk.Name = "CmdOk"
        Me.CmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdOk.Size = New System.Drawing.Size(109, 25)
        Me.CmdOk.TabIndex = 35
        Me.CmdOk.Text = "ＯＫ(&O)"
        Me.CmdOk.UseVisualStyleBackColor = False
        '
        'CmdFind
        '
        Me.CmdFind.BackColor = System.Drawing.SystemColors.Control
        Me.CmdFind.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmdFind.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CmdFind.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdFind.Location = New System.Drawing.Point(710, 141)
        Me.CmdFind.Name = "CmdFind"
        Me.CmdFind.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdFind.Size = New System.Drawing.Size(83, 25)
        Me.CmdFind.TabIndex = 33
        Me.CmdFind.Text = "検索(&F)"
        Me.CmdFind.UseVisualStyleBackColor = False
        '
        'SelListVw
        '
        Me.SelListVw.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SelListVw.BackColor = System.Drawing.Color.FromArgb(CType(CType(198, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.SelListVw.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.SelListVw.CheckBoxes = True
        Me.SelListVw.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.SelListVw.ForeColor = System.Drawing.SystemColors.WindowText
        Me.SelListVw.FullRowSelect = True
        Me.SelListVw.HideSelection = False
        Me.SelListVw.Location = New System.Drawing.Point(11, 185)
        Me.SelListVw.Name = "SelListVw"
        Me.SelListVw.Size = New System.Drawing.Size(365, 329)
        Me.SelListVw.SortStyle = SortableListView.SortStyles.SortDefault
        Me.SelListVw.TabIndex = 34
        Me.SelListVw.UseCompatibleStateImageBehavior = False
        Me.SelListVw.View = System.Windows.Forms.View.Details
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
        Me.lb_該当件数.Size = New System.Drawing.Size(74, 17)
        Me.lb_該当件数.TabIndex = 46
        Me.lb_該当件数.Text = "該当件数"
        Me.lb_該当件数.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lbListCount
        '
        Me.lbListCount.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.lbListCount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lbListCount.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lbListCount.ForeColor = System.Drawing.Color.Yellow
        Me.lbListCount.Location = New System.Drawing.Point(85, 5)
        Me.lbListCount.Name = "lbListCount"
        Me.lbListCount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lbListCount.Size = New System.Drawing.Size(89, 17)
        Me.lbListCount.TabIndex = 44
        Me.lbListCount.TextAlign = System.Drawing.ContentAlignment.TopRight
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
        Me.lbGuide.Size = New System.Drawing.Size(322, 17)
        Me.lbGuide.TabIndex = 43
        Me.lbGuide.Text = "[↑][↓]で選択、[Enter]で決定 ／[Esc]で中止"
        '
        'lb_Find_3
        '
        Me.lb_Find_3.BackColor = System.Drawing.SystemColors.Control
        Me.lb_Find_3.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_Find_3.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_Find_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_Find_3.Location = New System.Drawing.Point(311, 48)
        Me.lb_Find_3.Name = "lb_Find_3"
        Me.lb_Find_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_Find_3.Size = New System.Drawing.Size(58, 19)
        Me.lb_Find_3.TabIndex = 48
        Me.lb_Find_3.Text = "ＰＣ"
        '
        'lb_Find_2
        '
        Me.lb_Find_2.BackColor = System.Drawing.SystemColors.Control
        Me.lb_Find_2.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_Find_2.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_Find_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_Find_2.Location = New System.Drawing.Point(218, 48)
        Me.lb_Find_2.Name = "lb_Find_2"
        Me.lb_Find_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_Find_2.Size = New System.Drawing.Size(61, 19)
        Me.lb_Find_2.TabIndex = 45
        Me.lb_Find_2.Text = "ユニット"
        '
        'lb_Find_1
        '
        Me.lb_Find_1.BackColor = System.Drawing.SystemColors.Control
        Me.lb_Find_1.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_Find_1.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_Find_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_Find_1.Location = New System.Drawing.Point(126, 48)
        Me.lb_Find_1.Name = "lb_Find_1"
        Me.lb_Find_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_Find_1.Size = New System.Drawing.Size(49, 19)
        Me.lb_Find_1.TabIndex = 42
        Me.lb_Find_1.Text = "品群"
        '
        'lb_Find_0
        '
        Me.lb_Find_0.BackColor = System.Drawing.SystemColors.Control
        Me.lb_Find_0.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_Find_0.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_Find_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_Find_0.Location = New System.Drawing.Point(37, 48)
        Me.lb_Find_0.Name = "lb_Find_0"
        Me.lb_Find_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_Find_0.Size = New System.Drawing.Size(53, 19)
        Me.lb_Find_0.TabIndex = 39
        Me.lb_Find_0.Text = "製品"
        '
        'SelSeiInfChk
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.CmdCan
        Me.ClientSize = New System.Drawing.Size(804, 563)
        Me.Controls.Add(Me.PicFind_0)
        Me.Controls.Add(Me.PicFind_1)
        Me.Controls.Add(Me.PicFind_2)
        Me.Controls.Add(Me.PicFind_3)
        Me.Controls.Add(Me.cmdChkOn)
        Me.Controls.Add(Me.cmdChkOff)
        Me.Controls.Add(Me.ck_Find_3)
        Me.Controls.Add(Me.ck_Find_2)
        Me.Controls.Add(Me.ck_Find_1)
        Me.Controls.Add(Me.ck_Find_0)
        Me.Controls.Add(Me.CmdCan)
        Me.Controls.Add(Me.CmdOk)
        Me.Controls.Add(Me.CmdFind)
        Me.Controls.Add(Me.SelListVw)
        Me.Controls.Add(Me.lb_該当件数)
        Me.Controls.Add(Me.lbListCount)
        Me.Controls.Add(Me.lbGuide)
        Me.Controls.Add(Me.lb_Find_3)
        Me.Controls.Add(Me.lb_Find_2)
        Me.Controls.Add(Me.lb_Find_1)
        Me.Controls.Add(Me.lb_Find_0)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "SelSeiInfChk"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "製品情報検索"
        Me.PicFind_0.ResumeLayout(False)
        Me.PicFind_1.ResumeLayout(False)
        Me.PicFind_2.ResumeLayout(False)
        Me.PicFind_3.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Public ToolTip1 As System.Windows.Forms.ToolTip
    Public WithEvents tx_F1検索製品 As ExText.ExTextBox
    Public WithEvents tx_F1検索名称 As ExText.ExTextBox
    Public WithEvents tx_F1検索仕入先 As ExText.ExTextBox
    Public WithEvents tx_F1検索仕様 As ExText.ExTextBox
    Public WithEvents tx_F1検索W As ExNmText.ExNmTextBox
    Public WithEvents tx_F1検索D As ExNmText.ExNmTextBox
    Public WithEvents tx_F1検索H As ExNmText.ExNmTextBox
    Public WithEvents tx_F1検索D1 As ExNmText.ExNmTextBox
    Public WithEvents tx_F1検索D2 As ExNmText.ExNmTextBox
    Public WithEvents tx_F1検索H1 As ExNmText.ExNmTextBox
    Public WithEvents tx_F1検索H2 As ExNmText.ExNmTextBox
    Public WithEvents tx_F1検索仕様e As ExText.ExTextBox
    Public WithEvents _lb_kara_2 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_23 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_22 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_16 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_15 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_14 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_13 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_12 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_0 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_3 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_2 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_1 As System.Windows.Forms.Label
	Public WithEvents PicFind_0 As System.Windows.Forms.Panel
    Public WithEvents tx_F2検索品群 As ExText.ExTextBox
    Public WithEvents tx_F2検索名称 As ExText.ExTextBox
    Public WithEvents _lb_項目_4 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_5 As System.Windows.Forms.Label
	Public WithEvents PicFind_1 As System.Windows.Forms.Panel
    Public WithEvents tx_F3検索ユニット As ExText.ExTextBox
    Public WithEvents tx_F3検索名称 As ExText.ExTextBox
    Public WithEvents _lb_項目_8 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_9 As System.Windows.Forms.Label
	Public WithEvents PicFind_2 As System.Windows.Forms.Panel
    Public WithEvents tx_F4検索製品 As ExText.ExTextBox
    Public WithEvents tx_F4検索名称 As ExText.ExTextBox
    Public WithEvents tx_F4検索PC区分 As ExText.ExTextBox
    Public WithEvents tx_F4検索仕入先 As ExText.ExTextBox
    Public WithEvents tx_F4検索W As ExNmText.ExNmTextBox
    Public WithEvents tx_F4検索D As ExNmText.ExNmTextBox
    Public WithEvents tx_F4検索H As ExNmText.ExNmTextBox
    Public WithEvents tx_F4検索径 As ExNmText.ExNmTextBox
    Public WithEvents tx_F4検索T As ExNmText.ExNmTextBox
    Public WithEvents _lb_項目_21 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_20 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_19 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_18 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_17 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_6 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_7 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_10 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_11 As System.Windows.Forms.Label
	Public WithEvents PicFind_3 As System.Windows.Forms.Panel
	Public WithEvents cmdChkOn As System.Windows.Forms.Button
	Public WithEvents cmdChkOff As System.Windows.Forms.Button
	Public WithEvents ck_Find_3 As System.Windows.Forms.CheckBox
	Public WithEvents ck_Find_2 As System.Windows.Forms.CheckBox
	Public WithEvents ck_Find_1 As System.Windows.Forms.CheckBox
	Public WithEvents ck_Find_0 As System.Windows.Forms.CheckBox
	Public WithEvents CmdCan As System.Windows.Forms.Button
	Public WithEvents CmdOk As System.Windows.Forms.Button
	Public WithEvents CmdFind As System.Windows.Forms.Button
	Public WithEvents lb_該当件数 As System.Windows.Forms.Label
	Public WithEvents lbListCount As System.Windows.Forms.Label
	Public WithEvents lbGuide As System.Windows.Forms.Label
	Public WithEvents lb_Find_3 As System.Windows.Forms.Label
	Public WithEvents lb_Find_2 As System.Windows.Forms.Label
	Public WithEvents lb_Find_1 As System.Windows.Forms.Label
	Public WithEvents lb_Find_0 As System.Windows.Forms.Label
    Public WithEvents PicFind As System.Windows.Forms.Panel
    Public WithEvents ck_Find As System.Windows.Forms.CheckBox
    Public WithEvents lb_Find As System.Windows.Forms.Label
    Public WithEvents lb_kara As System.Windows.Forms.Label
    Public WithEvents lb_項目 As System.Windows.Forms.Label
    Public WithEvents SelListVw As SortableListView
End Class