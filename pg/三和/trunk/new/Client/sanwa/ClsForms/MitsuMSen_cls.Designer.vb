<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MitsuMSen_cls
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
	<System.Diagnostics.DebuggerStepThrough()> _
	Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MitsuMSen_cls))
        Me.ck_NotZero = New System.Windows.Forms.CheckBox()
        Me.SelListVw = New SortableListView()
        Me.CmdOk = New System.Windows.Forms.Button()
        Me.CmdCan = New System.Windows.Forms.Button()
        Me.CmdFind = New System.Windows.Forms.Button()
        Me.tx_検索製品 = New ExText.ExTextBox()
        Me.tx_検索名称 = New ExText.ExTextBox()
        Me.tx_検索仕様 = New ExText.ExTextBox()
        Me.tx_W = New ExNmText.ExNmTextBox()
        Me.tx_D = New ExNmText.ExNmTextBox()
        Me.tx_H = New ExNmText.ExNmTextBox()
        Me.tx_得意先CD = New ExText.ExTextBox()
        Me.tx_仕入先CD = New ExText.ExTextBox()
        Me.tx_納入先CD = New ExText.ExTextBox()
        Me.tx_見積件名 = New ExText.ExTextBox()
        Me.tx_仕入業者CD = New ExText.ExTextBox()
        Me._lb_項目_9 = New System.Windows.Forms.Label()
        Me._lb_項目_7 = New System.Windows.Forms.Label()
        Me._lb_項目_2 = New System.Windows.Forms.Label()
        Me._lb_項目_8 = New System.Windows.Forms.Label()
        Me._lb_項目_3 = New System.Windows.Forms.Label()
        Me._lb_項目_12 = New System.Windows.Forms.Label()
        Me._lb_項目_11 = New System.Windows.Forms.Label()
        Me._lb_項目_6 = New System.Windows.Forms.Label()
        Me._lb_項目_5 = New System.Windows.Forms.Label()
        Me._lb_項目_4 = New System.Windows.Forms.Label()
        Me._lb_項目_1 = New System.Windows.Forms.Label()
        Me.lbGuide = New System.Windows.Forms.Label()
        Me.lbListCount = New System.Windows.Forms.Label()
        Me.lb_該当件数 = New System.Windows.Forms.Label()
        Me._lb_項目_0 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'ck_NotZero
        '
        Me.ck_NotZero.BackColor = System.Drawing.SystemColors.Control
        Me.ck_NotZero.Checked = True
        Me.ck_NotZero.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ck_NotZero.Cursor = System.Windows.Forms.Cursors.Default
        Me.ck_NotZero.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.ck_NotZero.ForeColor = System.Drawing.Color.Black
        Me.ck_NotZero.Location = New System.Drawing.Point(664, 115)
        Me.ck_NotZero.Name = "ck_NotZero"
        Me.ck_NotZero.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ck_NotZero.Size = New System.Drawing.Size(17, 19)
        Me.ck_NotZero.TabIndex = 11
        Me.ck_NotZero.UseVisualStyleBackColor = False
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
        Me.SelListVw.Size = New System.Drawing.Size(782, 316)
        Me.SelListVw.SortStyle = SortableListView.SortStyles.SortDefault
        Me.SelListVw.TabIndex = 13
        Me.SelListVw.UseCompatibleStateImageBehavior = False
        Me.SelListVw.View = System.Windows.Forms.View.Details
        '
        'CmdOk
        '
        Me.CmdOk.BackColor = System.Drawing.SystemColors.Control
        Me.CmdOk.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmdOk.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CmdOk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdOk.Location = New System.Drawing.Point(497, 471)
        Me.CmdOk.Name = "CmdOk"
        Me.CmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdOk.Size = New System.Drawing.Size(92, 25)
        Me.CmdOk.TabIndex = 14
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
        Me.CmdCan.Location = New System.Drawing.Point(605, 471)
        Me.CmdCan.Name = "CmdCan"
        Me.CmdCan.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdCan.Size = New System.Drawing.Size(92, 25)
        Me.CmdCan.TabIndex = 15
        Me.CmdCan.Text = "ｷｬﾝｾﾙ(&C)"
        Me.CmdCan.UseVisualStyleBackColor = False
        '
        'CmdFind
        '
        Me.CmdFind.BackColor = System.Drawing.SystemColors.Control
        Me.CmdFind.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmdFind.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CmdFind.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdFind.Location = New System.Drawing.Point(711, 111)
        Me.CmdFind.Name = "CmdFind"
        Me.CmdFind.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdFind.Size = New System.Drawing.Size(83, 25)
        Me.CmdFind.TabIndex = 12
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
        Me.tx_検索製品.Location = New System.Drawing.Point(263, 67)
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
        Me.tx_検索製品.TabIndex = 5
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
        Me.tx_検索名称.Location = New System.Drawing.Point(263, 112)
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
        Me.tx_検索名称.TabIndex = 7
        Me.tx_検索名称.Text = "ExTextBox"
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
        Me.tx_検索仕様.Location = New System.Drawing.Point(263, 90)
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
        Me.tx_検索仕様.TabIndex = 6
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
        Me.tx_W.Location = New System.Drawing.Point(645, 45)
        Me.tx_W.MaxLength = 4
        Me.tx_W.Name = "tx_W"
        Me.tx_W.OldValue = "9999"
        Me.tx_W.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_W.SelectText = True
        Me.tx_W.SelLength = 0
        Me.tx_W.SelStart = 0
        Me.tx_W.SelText = ""
        Me.tx_W.Size = New System.Drawing.Size(52, 22)
        Me.tx_W.TabIndex = 8
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
        Me.tx_D.Location = New System.Drawing.Point(645, 67)
        Me.tx_D.MaxLength = 4
        Me.tx_D.Name = "tx_D"
        Me.tx_D.OldValue = "9999"
        Me.tx_D.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_D.SelectText = True
        Me.tx_D.SelLength = 0
        Me.tx_D.SelStart = 0
        Me.tx_D.SelText = ""
        Me.tx_D.Size = New System.Drawing.Size(52, 22)
        Me.tx_D.TabIndex = 9
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
        Me.tx_H.Location = New System.Drawing.Point(645, 90)
        Me.tx_H.MaxLength = 4
        Me.tx_H.Name = "tx_H"
        Me.tx_H.OldValue = "9999"
        Me.tx_H.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_H.SelectText = True
        Me.tx_H.SelLength = 0
        Me.tx_H.SelStart = 0
        Me.tx_H.SelText = ""
        Me.tx_H.Size = New System.Drawing.Size(52, 22)
        Me.tx_H.TabIndex = 10
        Me.tx_H.Text = "9999"
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
        Me.tx_得意先CD.IMEMode = System.Windows.Forms.ImeMode.Disable
        Me.tx_得意先CD.LengthType = ExText.ExTextBox.LenType.ByteType
        Me.tx_得意先CD.Location = New System.Drawing.Point(92, 45)
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
        Me.tx_仕入先CD.IMEMode = System.Windows.Forms.ImeMode.Disable
        Me.tx_仕入先CD.LengthType = ExText.ExTextBox.LenType.ByteType
        Me.tx_仕入先CD.Location = New System.Drawing.Point(92, 112)
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
        Me.tx_仕入先CD.TabIndex = 3
        Me.tx_仕入先CD.Text = "8888"
        '
        'tx_納入先CD
        '
        Me.tx_納入先CD.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_納入先CD.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_納入先CD.CanForwardSetFocus = True
        Me.tx_納入先CD.CanNextSetFocus = True
        Me.tx_納入先CD.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_納入先CD.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_納入先CD.EditMode = False
        Me.tx_納入先CD.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_納入先CD.IMEMode = System.Windows.Forms.ImeMode.Disable
        Me.tx_納入先CD.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_納入先CD.Location = New System.Drawing.Point(92, 67)
        Me.tx_納入先CD.MaxLength = 4
        Me.tx_納入先CD.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_納入先CD.Name = "tx_納入先CD"
        Me.tx_納入先CD.OldValue = "1234"
        Me.tx_納入先CD.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_納入先CD.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_納入先CD.SelectText = False
        Me.tx_納入先CD.SelLength = 0
        Me.tx_納入先CD.SelStart = 0
        Me.tx_納入先CD.SelText = ""
        Me.tx_納入先CD.Size = New System.Drawing.Size(50, 22)
        Me.tx_納入先CD.TabIndex = 1
        Me.tx_納入先CD.Text = "1234"
        '
        'tx_見積件名
        '
        Me.tx_見積件名.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_見積件名.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_見積件名.CanForwardSetFocus = True
        Me.tx_見積件名.CanNextSetFocus = True
        Me.tx_見積件名.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_見積件名.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_見積件名.EditMode = True
        Me.tx_見積件名.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_見積件名.IMEMode = System.Windows.Forms.ImeMode.Hiragana
        Me.tx_見積件名.LengthType = ExText.ExTextBox.LenType.ByteType
        Me.tx_見積件名.Location = New System.Drawing.Point(263, 45)
        Me.tx_見積件名.MaxLength = 0
        Me.tx_見積件名.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_見積件名.Name = "tx_見積件名"
        Me.tx_見積件名.OldValue = "あいうえおあいうえおあいうえおあいうえお"
        Me.tx_見積件名.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_見積件名.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_見積件名.SelectText = True
        Me.tx_見積件名.SelLength = 0
        Me.tx_見積件名.SelStart = 0
        Me.tx_見積件名.SelText = ""
        Me.tx_見積件名.Size = New System.Drawing.Size(288, 22)
        Me.tx_見積件名.TabIndex = 4
        Me.tx_見積件名.Text = "あいうえおあいうえおあいうえおあいうえお"
        '
        'tx_仕入業者CD
        '
        Me.tx_仕入業者CD.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_仕入業者CD.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_仕入業者CD.CanForwardSetFocus = True
        Me.tx_仕入業者CD.CanNextSetFocus = True
        Me.tx_仕入業者CD.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_仕入業者CD.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_仕入業者CD.EditMode = True
        Me.tx_仕入業者CD.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_仕入業者CD.IMEMode = System.Windows.Forms.ImeMode.Disable
        Me.tx_仕入業者CD.LengthType = ExText.ExTextBox.LenType.ByteType
        Me.tx_仕入業者CD.Location = New System.Drawing.Point(92, 90)
        Me.tx_仕入業者CD.MaxLength = 4
        Me.tx_仕入業者CD.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_仕入業者CD.Name = "tx_仕入業者CD"
        Me.tx_仕入業者CD.OldValue = "8888"
        Me.tx_仕入業者CD.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_仕入業者CD.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_仕入業者CD.SelectText = True
        Me.tx_仕入業者CD.SelLength = 0
        Me.tx_仕入業者CD.SelStart = 0
        Me.tx_仕入業者CD.SelText = ""
        Me.tx_仕入業者CD.Size = New System.Drawing.Size(50, 22)
        Me.tx_仕入業者CD.TabIndex = 2
        Me.tx_仕入業者CD.Text = "8888"
        '
        '_lb_項目_9
        '
        Me._lb_項目_9.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_9.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_9.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_9.ForeColor = System.Drawing.Color.White
        Me._lb_項目_9.Location = New System.Drawing.Point(556, 112)
        Me._lb_項目_9.Name = "_lb_項目_9"
        Me._lb_項目_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_9.Size = New System.Drawing.Size(89, 22)
        Me._lb_項目_9.TabIndex = 30
        Me._lb_項目_9.Text = "数量0以外"
        Me._lb_項目_9.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_7
        '
        Me._lb_項目_7.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_7.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_7.ForeColor = System.Drawing.Color.White
        Me._lb_項目_7.Location = New System.Drawing.Point(174, 45)
        Me._lb_項目_7.Name = "_lb_項目_7"
        Me._lb_項目_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_7.Size = New System.Drawing.Size(89, 22)
        Me._lb_項目_7.TabIndex = 29
        Me._lb_項目_7.Text = "見積件名"
        Me._lb_項目_7.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_2
        '
        Me._lb_項目_2.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_2.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_2.ForeColor = System.Drawing.Color.White
        Me._lb_項目_2.Location = New System.Drawing.Point(174, 112)
        Me._lb_項目_2.Name = "_lb_項目_2"
        Me._lb_項目_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_2.Size = New System.Drawing.Size(89, 22)
        Me._lb_項目_2.TabIndex = 28
        Me._lb_項目_2.Text = "漢字名称"
        Me._lb_項目_2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_8
        '
        Me._lb_項目_8.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_8.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_8.ForeColor = System.Drawing.Color.White
        Me._lb_項目_8.Location = New System.Drawing.Point(12, 90)
        Me._lb_項目_8.Name = "_lb_項目_8"
        Me._lb_項目_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_8.Size = New System.Drawing.Size(80, 22)
        Me._lb_項目_8.TabIndex = 27
        Me._lb_項目_8.Text = "仕入業者"
        Me._lb_項目_8.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_3
        '
        Me._lb_項目_3.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_3.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_3.ForeColor = System.Drawing.Color.White
        Me._lb_項目_3.Location = New System.Drawing.Point(12, 67)
        Me._lb_項目_3.Name = "_lb_項目_3"
        Me._lb_項目_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_3.Size = New System.Drawing.Size(80, 22)
        Me._lb_項目_3.TabIndex = 26
        Me._lb_項目_3.Text = "納入先"
        Me._lb_項目_3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_12
        '
        Me._lb_項目_12.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_12.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_12.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_12.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_12.ForeColor = System.Drawing.Color.White
        Me._lb_項目_12.Location = New System.Drawing.Point(12, 112)
        Me._lb_項目_12.Name = "_lb_項目_12"
        Me._lb_項目_12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_12.Size = New System.Drawing.Size(80, 22)
        Me._lb_項目_12.TabIndex = 25
        Me._lb_項目_12.Text = "出荷元"
        Me._lb_項目_12.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_11
        '
        Me._lb_項目_11.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_11.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_11.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_11.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_11.ForeColor = System.Drawing.Color.White
        Me._lb_項目_11.Location = New System.Drawing.Point(12, 45)
        Me._lb_項目_11.Name = "_lb_項目_11"
        Me._lb_項目_11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_11.Size = New System.Drawing.Size(80, 22)
        Me._lb_項目_11.TabIndex = 24
        Me._lb_項目_11.Text = "得意先"
        Me._lb_項目_11.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_6
        '
        Me._lb_項目_6.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_6.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_6.ForeColor = System.Drawing.Color.White
        Me._lb_項目_6.Location = New System.Drawing.Point(556, 90)
        Me._lb_項目_6.Name = "_lb_項目_6"
        Me._lb_項目_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_6.Size = New System.Drawing.Size(89, 22)
        Me._lb_項目_6.TabIndex = 23
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
        Me._lb_項目_5.Location = New System.Drawing.Point(556, 67)
        Me._lb_項目_5.Name = "_lb_項目_5"
        Me._lb_項目_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_5.Size = New System.Drawing.Size(89, 22)
        Me._lb_項目_5.TabIndex = 22
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
        Me._lb_項目_4.Location = New System.Drawing.Point(556, 45)
        Me._lb_項目_4.Name = "_lb_項目_4"
        Me._lb_項目_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_4.Size = New System.Drawing.Size(89, 22)
        Me._lb_項目_4.TabIndex = 21
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
        Me._lb_項目_1.Location = New System.Drawing.Point(174, 90)
        Me._lb_項目_1.Name = "_lb_項目_1"
        Me._lb_項目_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_1.Size = New System.Drawing.Size(89, 22)
        Me._lb_項目_1.TabIndex = 20
        Me._lb_項目_1.Text = "仕様NO"
        Me._lb_項目_1.TextAlign = System.Drawing.ContentAlignment.TopCenter
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
        Me.lbGuide.Size = New System.Drawing.Size(318, 17)
        Me.lbGuide.TabIndex = 19
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
        Me.lb_該当件数.Size = New System.Drawing.Size(69, 17)
        Me.lb_該当件数.TabIndex = 17
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
        Me._lb_項目_0.Location = New System.Drawing.Point(174, 67)
        Me._lb_項目_0.Name = "_lb_項目_0"
        Me._lb_項目_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_0.Size = New System.Drawing.Size(89, 22)
        Me._lb_項目_0.TabIndex = 16
        Me._lb_項目_0.Text = "製品NO"
        Me._lb_項目_0.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'MitsuMSen_cls
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.CmdCan
        Me.ClientSize = New System.Drawing.Size(804, 502)
        Me.Controls.Add(Me.ck_NotZero)
        Me.Controls.Add(Me.SelListVw)
        Me.Controls.Add(Me.CmdOk)
        Me.Controls.Add(Me.CmdCan)
        Me.Controls.Add(Me.CmdFind)
        Me.Controls.Add(Me.tx_検索製品)
        Me.Controls.Add(Me.tx_検索名称)
        Me.Controls.Add(Me.tx_検索仕様)
        Me.Controls.Add(Me.tx_W)
        Me.Controls.Add(Me.tx_D)
        Me.Controls.Add(Me.tx_H)
        Me.Controls.Add(Me.tx_得意先CD)
        Me.Controls.Add(Me.tx_仕入先CD)
        Me.Controls.Add(Me.tx_納入先CD)
        Me.Controls.Add(Me.tx_見積件名)
        Me.Controls.Add(Me.tx_仕入業者CD)
        Me.Controls.Add(Me._lb_項目_9)
        Me.Controls.Add(Me._lb_項目_7)
        Me.Controls.Add(Me._lb_項目_2)
        Me.Controls.Add(Me._lb_項目_8)
        Me.Controls.Add(Me._lb_項目_3)
        Me.Controls.Add(Me._lb_項目_12)
        Me.Controls.Add(Me._lb_項目_11)
        Me.Controls.Add(Me._lb_項目_6)
        Me.Controls.Add(Me._lb_項目_5)
        Me.Controls.Add(Me._lb_項目_4)
        Me.Controls.Add(Me._lb_項目_1)
        Me.Controls.Add(Me.lbGuide)
        Me.Controls.Add(Me.lbListCount)
        Me.Controls.Add(Me.lb_該当件数)
        Me.Controls.Add(Me._lb_項目_0)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "MitsuMSen_cls"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "見積明細検索"
        Me.ResumeLayout(False)

    End Sub
    Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents ck_NotZero As System.Windows.Forms.CheckBox
    Public WithEvents SelListVw As SortableListView
    Public WithEvents CmdOk As System.Windows.Forms.Button
	Public WithEvents CmdCan As System.Windows.Forms.Button
	Public WithEvents CmdFind As System.Windows.Forms.Button
    Public WithEvents tx_検索製品 As ExText.ExTextBox
    Public WithEvents tx_検索名称 As ExText.ExTextBox
    Public WithEvents tx_検索仕様 As ExText.ExTextBox
    Public WithEvents tx_W As ExNmText.ExNmTextBox
    Public WithEvents tx_D As ExNmText.ExNmTextBox
    Public WithEvents tx_H As ExNmText.ExNmTextBox
    Public WithEvents tx_得意先CD As ExText.ExTextBox
    Public WithEvents tx_仕入先CD As ExText.ExTextBox
    Public WithEvents tx_納入先CD As ExText.ExTextBox
    Public WithEvents tx_見積件名 As ExText.ExTextBox
    Public WithEvents tx_仕入業者CD As ExText.ExTextBox
    Public WithEvents _lb_項目_9 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_7 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_2 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_8 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_3 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_12 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_11 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_6 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_5 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_4 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_1 As System.Windows.Forms.Label
	Public WithEvents lbGuide As System.Windows.Forms.Label
	Public WithEvents lbListCount As System.Windows.Forms.Label
	Public WithEvents lb_該当件数 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_0 As System.Windows.Forms.Label
	Public WithEvents lb_項目 As System.Windows.Forms.Label

End Class