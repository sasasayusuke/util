<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SnwMT01F00S

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

    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使って変更できます。
    'コード エディタを使用して、変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SnwMT01F00S))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.txDir = New System.Windows.Forms.TextBox()
        Me.Cb変更 = New System.Windows.Forms.Button()
        Me.PicFunction = New System.Windows.Forms.Panel()
        Me.CmdFind = New System.Windows.Forms.Button()
        Me.tx_納入先CD = New ExText.ExTextBox()
        Me.tx_得意先CD = New ExText.ExTextBox()
        Me.tx_s見積日D = New ExDateText.ExDateTextBoxD()
        Me.tx_s見積日M = New ExDateText.ExDateTextBoxM()
        Me.tx_s見積日Y = New ExDateText.ExDateTextBoxY()
        Me.tx_e見積日D = New ExDateText.ExDateTextBoxD()
        Me.tx_e見積日M = New ExDateText.ExDateTextBoxM()
        Me.tx_e見積日Y = New ExDateText.ExDateTextBoxY()
        Me.tx_見積件名 = New ExText.ExTextBox()
        Me.tx_s見積番号 = New ExText.ExTextBox()
        Me.tx_e見積番号 = New ExText.ExTextBox()
        Me.tx_s見積金額 = New ExNmText.ExNmTextBox()
        Me.tx_e見積金額 = New ExNmText.ExNmTextBox()
        Me.tx_売上種別 = New ExNmText.ExNmTextBox()
        Me.tx_担当者CD = New ExNmText.ExNmTextBox()
        Me.tx_物件種別 = New ExText.ExTextBox()
        Me.tx_s開始納期D = New ExDateText.ExDateTextBoxD()
        Me.tx_s開始納期M = New ExDateText.ExDateTextBoxM()
        Me.tx_s開始納期Y = New ExDateText.ExDateTextBoxY()
        Me.tx_e開始納期D = New ExDateText.ExDateTextBoxD()
        Me.tx_e開始納期M = New ExDateText.ExDateTextBoxM()
        Me.tx_e開始納期Y = New ExDateText.ExDateTextBoxY()
        Me.tx_s完了日D = New ExDateText.ExDateTextBoxD()
        Me.tx_s完了日M = New ExDateText.ExDateTextBoxM()
        Me.tx_s完了日Y = New ExDateText.ExDateTextBoxY()
        Me.tx_e完了日D = New ExDateText.ExDateTextBoxD()
        Me.tx_e完了日M = New ExDateText.ExDateTextBoxM()
        Me.tx_e完了日Y = New ExDateText.ExDateTextBoxY()
        Me.tx_ウエルシア物件区分 = New ExNmText.ExNmTextBox()
        Me.tx_s請求予定D = New ExDateText.ExDateTextBoxD()
        Me.tx_s請求予定M = New ExDateText.ExDateTextBoxM()
        Me.tx_s請求予定Y = New ExDateText.ExDateTextBoxY()
        Me.tx_e請求予定D = New ExDateText.ExDateTextBoxD()
        Me.tx_e請求予定M = New ExDateText.ExDateTextBoxM()
        Me.tx_e請求予定Y = New ExDateText.ExDateTextBoxY()
        Me.tx_ウエルシアリース区分 = New ExNmText.ExNmTextBox()
        Me.tx_s仕入日D = New ExDateText.ExDateTextBoxD()
        Me.tx_s仕入日M = New ExDateText.ExDateTextBoxM()
        Me.tx_s仕入日Y = New ExDateText.ExDateTextBoxY()
        Me.tx_e仕入日D = New ExDateText.ExDateTextBoxD()
        Me.tx_e仕入日M = New ExDateText.ExDateTextBoxM()
        Me.tx_e仕入日Y = New ExDateText.ExDateTextBoxY()
        Me.tx_s物件番号 = New ExText.ExTextBox()
        Me.tx_e物件番号 = New ExText.ExTextBox()
        Me.tx_見積確定区分 = New ExText.ExTextBox()
        Me.tx_業種区分 = New ExText.ExTextBox()
        Me._lb_項目_18 = New System.Windows.Forms.Label()
        Me.Label30 = New System.Windows.Forms.Label()
        Me._lblLabels_0 = New System.Windows.Forms.Label()
        Me._lb_項目_17 = New System.Windows.Forms.Label()
        Me._lb_項目_16 = New System.Windows.Forms.Label()
        Me._lb_物件番号_kara = New System.Windows.Forms.Label()
        Me._lb_仕入日_kara = New System.Windows.Forms.Label()
        Me.lb_s仕入日年 = New System.Windows.Forms.Label()
        Me.lb_s仕入日月 = New System.Windows.Forms.Label()
        Me.lb_s仕入日日 = New System.Windows.Forms.Label()
        Me.lb_e仕入日年 = New System.Windows.Forms.Label()
        Me.lb_e仕入日月 = New System.Windows.Forms.Label()
        Me.lb_e仕入日日 = New System.Windows.Forms.Label()
        Me._lb_項目_15 = New System.Windows.Forms.Label()
        Me.lb_s請求予定日 = New System.Windows.Forms.Label()
        Me.lb_e請求予定日 = New System.Windows.Forms.Label()
        Me._lb_項目_14 = New System.Windows.Forms.Label()
        Me._lb_項目_13 = New System.Windows.Forms.Label()
        Me._lblLabels_33 = New System.Windows.Forms.Label()
        Me._lb_項目_12 = New System.Windows.Forms.Label()
        Me._lb請求予定_kara = New System.Windows.Forms.Label()
        Me.lb_s請求予定年 = New System.Windows.Forms.Label()
        Me.lb_s請求予定月 = New System.Windows.Forms.Label()
        Me.lb_e請求予定年 = New System.Windows.Forms.Label()
        Me.lb_e請求予定月 = New System.Windows.Forms.Label()
        Me._lb_項目_11 = New System.Windows.Forms.Label()
        Me.rf_ウエルシア物件区分名 = New System.Windows.Forms.Label()
        Me._lb_項目_10 = New System.Windows.Forms.Label()
        Me.lb_e完了日日 = New System.Windows.Forms.Label()
        Me.lb_e完了日月 = New System.Windows.Forms.Label()
        Me.lb_e完了日年 = New System.Windows.Forms.Label()
        Me.lb_s完了日日 = New System.Windows.Forms.Label()
        Me.lb_s完了日月 = New System.Windows.Forms.Label()
        Me.lb_s完了日年 = New System.Windows.Forms.Label()
        Me._lb_完了日_kara = New System.Windows.Forms.Label()
        Me._lb_開始納期_kara = New System.Windows.Forms.Label()
        Me.lb_s開始納期年 = New System.Windows.Forms.Label()
        Me.lb_s開始納期月 = New System.Windows.Forms.Label()
        Me.lb_s開始納期日 = New System.Windows.Forms.Label()
        Me.lb_e開始納期年 = New System.Windows.Forms.Label()
        Me.lb_e開始納期月 = New System.Windows.Forms.Label()
        Me.lb_e開始納期日 = New System.Windows.Forms.Label()
        Me._lb_項目_9 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me._lb_項目_8 = New System.Windows.Forms.Label()
        Me._lb_見積番号_kara = New System.Windows.Forms.Label()
        Me._lb_項目_2 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me._lb_項目_1 = New System.Windows.Forms.Label()
        Me._lb_見積金額_kara = New System.Windows.Forms.Label()
        Me._lb_項目_0 = New System.Windows.Forms.Label()
        Me._lb_項目_7 = New System.Windows.Forms.Label()
        Me._lb_項目_6 = New System.Windows.Forms.Label()
        Me._lb_項目_3 = New System.Windows.Forms.Label()
        Me._lb_項目_4 = New System.Windows.Forms.Label()
        Me._lb_項目_5 = New System.Windows.Forms.Label()
        Me.lb_e見積日日 = New System.Windows.Forms.Label()
        Me.lb_e見積日月 = New System.Windows.Forms.Label()
        Me.lb_e見積日年 = New System.Windows.Forms.Label()
        Me.lb_s見積日日 = New System.Windows.Forms.Label()
        Me.lb_s見積日月 = New System.Windows.Forms.Label()
        Me.lb_s見積日年 = New System.Windows.Forms.Label()
        Me._lb_s見積日 = New System.Windows.Forms.Label()
        Me._lb_見積日_kara = New System.Windows.Forms.Label()
        Me.rf_ListCount = New System.Windows.Forms.Label()
        Me.lb_該当件数 = New System.Windows.Forms.Label()
        Me._lb_e見積日 = New System.Windows.Forms.Label()
        Me._lb_e開始納期 = New System.Windows.Forms.Label()
        Me._lb_s開始納期 = New System.Windows.Forms.Label()
        Me._lb_e完了日 = New System.Windows.Forms.Label()
        Me._lb_s完了日 = New System.Windows.Forms.Label()
        Me._lb_s請求予定 = New System.Windows.Forms.Label()
        Me._lb_e請求予定 = New System.Windows.Forms.Label()
        Me._lb_s仕入日 = New System.Windows.Forms.Label()
        Me._lb_e仕入日 = New System.Windows.Forms.Label()
        Me.SelListVw = New SnwMT01.SortableListView()
        Me.SuspendLayout()
        '
        'txDir
        '
        Me.txDir.AcceptsReturn = True
        Me.txDir.BackColor = System.Drawing.SystemColors.Window
        Me.txDir.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txDir.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txDir.Location = New System.Drawing.Point(799, 28)
        Me.txDir.Margin = New System.Windows.Forms.Padding(2)
        Me.txDir.MaxLength = 60
        Me.txDir.Name = "txDir"
        Me.txDir.ReadOnly = True
        Me.txDir.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txDir.Size = New System.Drawing.Size(159, 22)
        Me.txDir.TabIndex = 132
        Me.txDir.TabStop = False
        '
        'Cb変更
        '
        Me.Cb変更.BackColor = System.Drawing.SystemColors.Control
        Me.Cb変更.Cursor = System.Windows.Forms.Cursors.Default
        Me.Cb変更.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Cb変更.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Cb変更.Location = New System.Drawing.Point(966, 28)
        Me.Cb変更.Margin = New System.Windows.Forms.Padding(2)
        Me.Cb変更.Name = "Cb変更"
        Me.Cb変更.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Cb変更.Size = New System.Drawing.Size(65, 22)
        Me.Cb変更.TabIndex = 131
        Me.Cb変更.Text = "変更(&D)"
        Me.Cb変更.UseVisualStyleBackColor = False
        '
        'PicFunction
        '
        Me.PicFunction.BackColor = System.Drawing.SystemColors.Control
        Me.PicFunction.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PicFunction.Cursor = System.Windows.Forms.Cursors.Default
        Me.PicFunction.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PicFunction.ForeColor = System.Drawing.SystemColors.ControlText
        Me.PicFunction.Location = New System.Drawing.Point(0, 563)
        Me.PicFunction.Margin = New System.Windows.Forms.Padding(2)
        Me.PicFunction.Name = "PicFunction"
        Me.PicFunction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.PicFunction.Size = New System.Drawing.Size(1055, 37)
        Me.PicFunction.TabIndex = 58
        '
        'CmdFind
        '
        Me.CmdFind.BackColor = System.Drawing.SystemColors.Control
        Me.CmdFind.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmdFind.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CmdFind.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdFind.Location = New System.Drawing.Point(844, 163)
        Me.CmdFind.Margin = New System.Windows.Forms.Padding(2)
        Me.CmdFind.Name = "CmdFind"
        Me.CmdFind.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdFind.Size = New System.Drawing.Size(81, 25)
        Me.CmdFind.TabIndex = 46
        Me.CmdFind.Text = "検索(&F)"
        Me.CmdFind.UseVisualStyleBackColor = False
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
        Me.tx_納入先CD.Location = New System.Drawing.Point(101, 67)
        Me.tx_納入先CD.Margin = New System.Windows.Forms.Padding(2)
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
        Me.tx_納入先CD.Size = New System.Drawing.Size(35, 22)
        Me.tx_納入先CD.TabIndex = 2
        Me.tx_納入先CD.Text = "1234"
        '
        'tx_得意先CD
        '
        Me.tx_得意先CD.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_得意先CD.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_得意先CD.CanForwardSetFocus = True
        Me.tx_得意先CD.CanNextSetFocus = True
        Me.tx_得意先CD.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_得意先CD.Cursor = System.Windows.Forms.Cursors.Default
        Me.tx_得意先CD.EditMode = False
        Me.tx_得意先CD.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_得意先CD.IMEMode = System.Windows.Forms.ImeMode.Disable
        Me.tx_得意先CD.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_得意先CD.Location = New System.Drawing.Point(101, 46)
        Me.tx_得意先CD.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_得意先CD.MaxLength = 4
        Me.tx_得意先CD.MousePointer = System.Windows.Forms.Cursors.Default
        Me.tx_得意先CD.Name = "tx_得意先CD"
        Me.tx_得意先CD.OldValue = "1234567"
        Me.tx_得意先CD.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_得意先CD.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_得意先CD.SelectText = False
        Me.tx_得意先CD.SelLength = 0
        Me.tx_得意先CD.SelStart = 0
        Me.tx_得意先CD.SelText = ""
        Me.tx_得意先CD.Size = New System.Drawing.Size(59, 22)
        Me.tx_得意先CD.TabIndex = 1
        Me.tx_得意先CD.Text = "1234567"
        '
        'tx_s見積日D
        '
        Me.tx_s見積日D.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_s見積日D.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_s見積日D.CanForwardSetFocus = True
        Me.tx_s見積日D.CanNextSetFocus = True
        Me.tx_s見積日D.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_s見積日D.EditMode = False
        Me.tx_s見積日D.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_s見積日D.Location = New System.Drawing.Point(624, 88)
        Me.tx_s見積日D.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_s見積日D.MaxLength = 2
        Me.tx_s見積日D.Name = "tx_s見積日D"
        Me.tx_s見積日D.OldValue = "DD"
        Me.tx_s見積日D.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_s見積日D.SelectText = True
        Me.tx_s見積日D.SelLength = 0
        Me.tx_s見積日D.SelStart = 0
        Me.tx_s見積日D.SelText = ""
        Me.tx_s見積日D.Size = New System.Drawing.Size(20, 16)
        Me.tx_s見積日D.TabIndex = 17
        Me.tx_s見積日D.Text = "DD"
        '
        'tx_s見積日M
        '
        Me.tx_s見積日M.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_s見積日M.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_s見積日M.CanForwardSetFocus = True
        Me.tx_s見積日M.CanNextSetFocus = True
        Me.tx_s見積日M.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_s見積日M.EditMode = False
        Me.tx_s見積日M.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_s見積日M.Location = New System.Drawing.Point(586, 88)
        Me.tx_s見積日M.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_s見積日M.MaxLength = 2
        Me.tx_s見積日M.Name = "tx_s見積日M"
        Me.tx_s見積日M.OldValue = "MM"
        Me.tx_s見積日M.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_s見積日M.SelectText = True
        Me.tx_s見積日M.SelLength = 0
        Me.tx_s見積日M.SelStart = 0
        Me.tx_s見積日M.SelText = ""
        Me.tx_s見積日M.Size = New System.Drawing.Size(20, 16)
        Me.tx_s見積日M.TabIndex = 16
        Me.tx_s見積日M.Text = "MM"
        '
        'tx_s見積日Y
        '
        Me.tx_s見積日Y.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_s見積日Y.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_s見積日Y.CanForwardSetFocus = True
        Me.tx_s見積日Y.CanNextSetFocus = True
        Me.tx_s見積日Y.Cursor = System.Windows.Forms.Cursors.Default
        Me.tx_s見積日Y.EditMode = True
        Me.tx_s見積日Y.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_s見積日Y.Location = New System.Drawing.Point(535, 88)
        Me.tx_s見積日Y.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_s見積日Y.MaxLength = 4
        Me.tx_s見積日Y.Name = "tx_s見積日Y"
        Me.tx_s見積日Y.OldValue = "YYYY"
        Me.tx_s見積日Y.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_s見積日Y.SelectText = True
        Me.tx_s見積日Y.SelLength = 0
        Me.tx_s見積日Y.SelStart = 0
        Me.tx_s見積日Y.SelText = ""
        Me.tx_s見積日Y.Size = New System.Drawing.Size(32, 16)
        Me.tx_s見積日Y.TabIndex = 15
        Me.tx_s見積日Y.Text = "YYYY"
        '
        'tx_e見積日D
        '
        Me.tx_e見積日D.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_e見積日D.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_e見積日D.CanForwardSetFocus = True
        Me.tx_e見積日D.CanNextSetFocus = True
        Me.tx_e見積日D.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_e見積日D.EditMode = False
        Me.tx_e見積日D.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_e見積日D.Location = New System.Drawing.Point(787, 88)
        Me.tx_e見積日D.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_e見積日D.MaxLength = 2
        Me.tx_e見積日D.Name = "tx_e見積日D"
        Me.tx_e見積日D.OldValue = "DD"
        Me.tx_e見積日D.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_e見積日D.SelectText = True
        Me.tx_e見積日D.SelLength = 0
        Me.tx_e見積日D.SelStart = 0
        Me.tx_e見積日D.SelText = ""
        Me.tx_e見積日D.Size = New System.Drawing.Size(20, 16)
        Me.tx_e見積日D.TabIndex = 20
        Me.tx_e見積日D.Text = "DD"
        '
        'tx_e見積日M
        '
        Me.tx_e見積日M.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_e見積日M.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_e見積日M.CanForwardSetFocus = True
        Me.tx_e見積日M.CanNextSetFocus = True
        Me.tx_e見積日M.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_e見積日M.EditMode = False
        Me.tx_e見積日M.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_e見積日M.Location = New System.Drawing.Point(749, 88)
        Me.tx_e見積日M.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_e見積日M.MaxLength = 2
        Me.tx_e見積日M.Name = "tx_e見積日M"
        Me.tx_e見積日M.OldValue = "MM"
        Me.tx_e見積日M.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_e見積日M.SelectText = True
        Me.tx_e見積日M.SelLength = 0
        Me.tx_e見積日M.SelStart = 0
        Me.tx_e見積日M.SelText = ""
        Me.tx_e見積日M.Size = New System.Drawing.Size(20, 16)
        Me.tx_e見積日M.TabIndex = 19
        Me.tx_e見積日M.Text = "MM"
        '
        'tx_e見積日Y
        '
        Me.tx_e見積日Y.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_e見積日Y.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_e見積日Y.CanForwardSetFocus = True
        Me.tx_e見積日Y.CanNextSetFocus = True
        Me.tx_e見積日Y.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_e見積日Y.EditMode = False
        Me.tx_e見積日Y.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_e見積日Y.Location = New System.Drawing.Point(699, 88)
        Me.tx_e見積日Y.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_e見積日Y.MaxLength = 4
        Me.tx_e見積日Y.Name = "tx_e見積日Y"
        Me.tx_e見積日Y.OldValue = "YYYY"
        Me.tx_e見積日Y.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_e見積日Y.SelectText = True
        Me.tx_e見積日Y.SelLength = 0
        Me.tx_e見積日Y.SelStart = 0
        Me.tx_e見積日Y.SelText = ""
        Me.tx_e見積日Y.Size = New System.Drawing.Size(32, 16)
        Me.tx_e見積日Y.TabIndex = 18
        Me.tx_e見積日Y.Text = "YYYY"
        '
        'tx_見積件名
        '
        Me.tx_見積件名.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_見積件名.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_見積件名.CanForwardSetFocus = True
        Me.tx_見積件名.CanNextSetFocus = True
        Me.tx_見積件名.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_見積件名.EditMode = True
        Me.tx_見積件名.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_見積件名.IMEMode = System.Windows.Forms.ImeMode.Hiragana
        Me.tx_見積件名.LengthType = ExText.ExTextBox.LenType.ByteType
        Me.tx_見積件名.Location = New System.Drawing.Point(101, 169)
        Me.tx_見積件名.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_見積件名.MaxLength = 0
        Me.tx_見積件名.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_見積件名.Name = "tx_見積件名"
        Me.tx_見積件名.OldValue = "ExTextBox"
        Me.tx_見積件名.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_見積件名.SelectText = True
        Me.tx_見積件名.SelLength = 0
        Me.tx_見積件名.SelStart = 0
        Me.tx_見積件名.SelText = ""
        Me.tx_見積件名.Size = New System.Drawing.Size(217, 22)
        Me.tx_見積件名.TabIndex = 8
        Me.tx_見積件名.Text = "ExTextBox"
        '
        'tx_s見積番号
        '
        Me.tx_s見積番号.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_s見積番号.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_s見積番号.CanForwardSetFocus = True
        Me.tx_s見積番号.CanNextSetFocus = True
        Me.tx_s見積番号.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_s見積番号.Cursor = System.Windows.Forms.Cursors.Default
        Me.tx_s見積番号.EditMode = False
        Me.tx_s見積番号.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_s見積番号.IMEMode = System.Windows.Forms.ImeMode.Disable
        Me.tx_s見積番号.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_s見積番号.Location = New System.Drawing.Point(101, 149)
        Me.tx_s見積番号.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_s見積番号.MaxLength = 7
        Me.tx_s見積番号.MousePointer = System.Windows.Forms.Cursors.Default
        Me.tx_s見積番号.Name = "tx_s見積番号"
        Me.tx_s見積番号.OldValue = "1234567"
        Me.tx_s見積番号.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_s見積番号.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_s見積番号.SelectText = False
        Me.tx_s見積番号.SelLength = 0
        Me.tx_s見積番号.SelStart = 0
        Me.tx_s見積番号.SelText = ""
        Me.tx_s見積番号.Size = New System.Drawing.Size(60, 22)
        Me.tx_s見積番号.TabIndex = 6
        Me.tx_s見積番号.Text = "1234567"
        '
        'tx_e見積番号
        '
        Me.tx_e見積番号.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_e見積番号.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_e見積番号.CanForwardSetFocus = True
        Me.tx_e見積番号.CanNextSetFocus = True
        Me.tx_e見積番号.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_e見積番号.Cursor = System.Windows.Forms.Cursors.Default
        Me.tx_e見積番号.EditMode = False
        Me.tx_e見積番号.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_e見積番号.IMEMode = System.Windows.Forms.ImeMode.Disable
        Me.tx_e見積番号.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_e見積番号.Location = New System.Drawing.Point(210, 149)
        Me.tx_e見積番号.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_e見積番号.MaxLength = 7
        Me.tx_e見積番号.MousePointer = System.Windows.Forms.Cursors.Default
        Me.tx_e見積番号.Name = "tx_e見積番号"
        Me.tx_e見積番号.OldValue = "ExTextBox"
        Me.tx_e見積番号.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_e見積番号.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_e見積番号.SelectText = False
        Me.tx_e見積番号.SelLength = 0
        Me.tx_e見積番号.SelStart = 0
        Me.tx_e見積番号.SelText = ""
        Me.tx_e見積番号.Size = New System.Drawing.Size(60, 22)
        Me.tx_e見積番号.TabIndex = 7
        Me.tx_e見積番号.Text = "ExTextBox"
        '
        'tx_s見積金額
        '
        Me.tx_s見積金額.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_s見積金額.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_s見積金額.CanForwardSetFocus = True
        Me.tx_s見積金額.CanNextSetFocus = True
        Me.tx_s見積金額.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_s見積金額.DecimalPlace = CType(0, Short)
        Me.tx_s見積金額.EditMode = False
        Me.tx_s見積金額.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_s見積金額.FormatType = "#,###"
        Me.tx_s見積金額.FormatTypeNega = ""
        Me.tx_s見積金額.FormatTypeNull = "#"
        Me.tx_s見積金額.FormatTypeZero = "#"
        Me.tx_s見積金額.InputMinus = True
        Me.tx_s見積金額.InputPlus = True
        Me.tx_s見積金額.InputZero = False
        Me.tx_s見積金額.Location = New System.Drawing.Point(101, 189)
        Me.tx_s見積金額.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_s見積金額.MaxLength = 9
        Me.tx_s見積金額.Name = "tx_s見積金額"
        Me.tx_s見積金額.OldValue = ""
        Me.tx_s見積金額.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_s見積金額.SelectText = True
        Me.tx_s見積金額.SelLength = 0
        Me.tx_s見積金額.SelStart = 0
        Me.tx_s見積金額.SelText = ""
        Me.tx_s見積金額.Size = New System.Drawing.Size(61, 22)
        Me.tx_s見積金額.TabIndex = 9
        '
        'tx_e見積金額
        '
        Me.tx_e見積金額.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_e見積金額.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_e見積金額.CanForwardSetFocus = True
        Me.tx_e見積金額.CanNextSetFocus = True
        Me.tx_e見積金額.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_e見積金額.DecimalPlace = CType(0, Short)
        Me.tx_e見積金額.EditMode = False
        Me.tx_e見積金額.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_e見積金額.FormatType = "#,###"
        Me.tx_e見積金額.FormatTypeNega = ""
        Me.tx_e見積金額.FormatTypeNull = "#"
        Me.tx_e見積金額.FormatTypeZero = "#"
        Me.tx_e見積金額.InputMinus = True
        Me.tx_e見積金額.InputPlus = True
        Me.tx_e見積金額.InputZero = False
        Me.tx_e見積金額.Location = New System.Drawing.Point(210, 189)
        Me.tx_e見積金額.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_e見積金額.MaxLength = 9
        Me.tx_e見積金額.Name = "tx_e見積金額"
        Me.tx_e見積金額.OldValue = ""
        Me.tx_e見積金額.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_e見積金額.SelectText = True
        Me.tx_e見積金額.SelLength = 0
        Me.tx_e見積金額.SelStart = 0
        Me.tx_e見積金額.SelText = ""
        Me.tx_e見積金額.Size = New System.Drawing.Size(61, 22)
        Me.tx_e見積金額.TabIndex = 10
        '
        'tx_売上種別
        '
        Me.tx_売上種別.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_売上種別.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_売上種別.CanForwardSetFocus = True
        Me.tx_売上種別.CanNextSetFocus = True
        Me.tx_売上種別.Cursor = System.Windows.Forms.Cursors.Default
        Me.tx_売上種別.DecimalPlace = CType(0, Short)
        Me.tx_売上種別.EditMode = True
        Me.tx_売上種別.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_売上種別.FormatType = "0"
        Me.tx_売上種別.FormatTypeNega = "0"
        Me.tx_売上種別.FormatTypeNull = "0"
        Me.tx_売上種別.FormatTypeZero = "0"
        Me.tx_売上種別.InputMinus = False
        Me.tx_売上種別.InputPlus = True
        Me.tx_売上種別.InputZero = True
        Me.tx_売上種別.Location = New System.Drawing.Point(101, 26)
        Me.tx_売上種別.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_売上種別.MaxLength = 1
        Me.tx_売上種別.Name = "tx_売上種別"
        Me.tx_売上種別.OldValue = "0"
        Me.tx_売上種別.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_売上種別.SelectText = True
        Me.tx_売上種別.SelLength = 0
        Me.tx_売上種別.SelStart = 0
        Me.tx_売上種別.SelText = ""
        Me.tx_売上種別.Size = New System.Drawing.Size(27, 22)
        Me.tx_売上種別.TabIndex = 0
        '
        'tx_担当者CD
        '
        Me.tx_担当者CD.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_担当者CD.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_担当者CD.CanForwardSetFocus = True
        Me.tx_担当者CD.CanNextSetFocus = True
        Me.tx_担当者CD.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_担当者CD.DecimalPlace = CType(0, Short)
        Me.tx_担当者CD.EditMode = True
        Me.tx_担当者CD.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_担当者CD.FormatType = "#"
        Me.tx_担当者CD.FormatTypeNega = ""
        Me.tx_担当者CD.FormatTypeNull = ""
        Me.tx_担当者CD.FormatTypeZero = ""
        Me.tx_担当者CD.InputMinus = False
        Me.tx_担当者CD.InputPlus = True
        Me.tx_担当者CD.InputZero = False
        Me.tx_担当者CD.Location = New System.Drawing.Point(101, 87)
        Me.tx_担当者CD.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_担当者CD.MaxLength = 3
        Me.tx_担当者CD.Name = "tx_担当者CD"
        Me.tx_担当者CD.OldValue = ""
        Me.tx_担当者CD.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_担当者CD.SelectText = True
        Me.tx_担当者CD.SelLength = 0
        Me.tx_担当者CD.SelStart = 0
        Me.tx_担当者CD.SelText = ""
        Me.tx_担当者CD.Size = New System.Drawing.Size(51, 22)
        Me.tx_担当者CD.TabIndex = 3
        '
        'tx_物件種別
        '
        Me.tx_物件種別.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_物件種別.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_物件種別.CanForwardSetFocus = True
        Me.tx_物件種別.CanNextSetFocus = True
        Me.tx_物件種別.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_物件種別.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_物件種別.EditMode = False
        Me.tx_物件種別.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_物件種別.IMEMode = System.Windows.Forms.ImeMode.Disable
        Me.tx_物件種別.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_物件種別.Location = New System.Drawing.Point(101, 128)
        Me.tx_物件種別.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_物件種別.MaxLength = 2
        Me.tx_物件種別.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_物件種別.Name = "tx_物件種別"
        Me.tx_物件種別.OldValue = "ExTextBox"
        Me.tx_物件種別.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_物件種別.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_物件種別.SelectText = True
        Me.tx_物件種別.SelLength = 0
        Me.tx_物件種別.SelStart = 0
        Me.tx_物件種別.SelText = ""
        Me.tx_物件種別.Size = New System.Drawing.Size(20, 22)
        Me.tx_物件種別.TabIndex = 5
        Me.tx_物件種別.Text = "ExTextBox"
        '
        'tx_s開始納期D
        '
        Me.tx_s開始納期D.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_s開始納期D.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_s開始納期D.CanForwardSetFocus = True
        Me.tx_s開始納期D.CanNextSetFocus = True
        Me.tx_s開始納期D.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_s開始納期D.EditMode = False
        Me.tx_s開始納期D.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_s開始納期D.Location = New System.Drawing.Point(624, 108)
        Me.tx_s開始納期D.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_s開始納期D.MaxLength = 2
        Me.tx_s開始納期D.Name = "tx_s開始納期D"
        Me.tx_s開始納期D.OldValue = "DD"
        Me.tx_s開始納期D.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_s開始納期D.SelectText = True
        Me.tx_s開始納期D.SelLength = 0
        Me.tx_s開始納期D.SelStart = 0
        Me.tx_s開始納期D.SelText = ""
        Me.tx_s開始納期D.Size = New System.Drawing.Size(20, 16)
        Me.tx_s開始納期D.TabIndex = 23
        Me.tx_s開始納期D.Text = "DD"
        '
        'tx_s開始納期M
        '
        Me.tx_s開始納期M.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_s開始納期M.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_s開始納期M.CanForwardSetFocus = True
        Me.tx_s開始納期M.CanNextSetFocus = True
        Me.tx_s開始納期M.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_s開始納期M.EditMode = False
        Me.tx_s開始納期M.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_s開始納期M.Location = New System.Drawing.Point(586, 108)
        Me.tx_s開始納期M.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_s開始納期M.MaxLength = 2
        Me.tx_s開始納期M.Name = "tx_s開始納期M"
        Me.tx_s開始納期M.OldValue = "MM"
        Me.tx_s開始納期M.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_s開始納期M.SelectText = True
        Me.tx_s開始納期M.SelLength = 0
        Me.tx_s開始納期M.SelStart = 0
        Me.tx_s開始納期M.SelText = ""
        Me.tx_s開始納期M.Size = New System.Drawing.Size(20, 16)
        Me.tx_s開始納期M.TabIndex = 22
        Me.tx_s開始納期M.Text = "MM"
        '
        'tx_s開始納期Y
        '
        Me.tx_s開始納期Y.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_s開始納期Y.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_s開始納期Y.CanForwardSetFocus = True
        Me.tx_s開始納期Y.CanNextSetFocus = True
        Me.tx_s開始納期Y.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_s開始納期Y.EditMode = False
        Me.tx_s開始納期Y.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_s開始納期Y.Location = New System.Drawing.Point(535, 108)
        Me.tx_s開始納期Y.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_s開始納期Y.MaxLength = 4
        Me.tx_s開始納期Y.Name = "tx_s開始納期Y"
        Me.tx_s開始納期Y.OldValue = "YYYY"
        Me.tx_s開始納期Y.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_s開始納期Y.SelectText = True
        Me.tx_s開始納期Y.SelLength = 0
        Me.tx_s開始納期Y.SelStart = 0
        Me.tx_s開始納期Y.SelText = ""
        Me.tx_s開始納期Y.Size = New System.Drawing.Size(32, 16)
        Me.tx_s開始納期Y.TabIndex = 21
        Me.tx_s開始納期Y.Text = "YYYY"
        '
        'tx_e開始納期D
        '
        Me.tx_e開始納期D.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_e開始納期D.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_e開始納期D.CanForwardSetFocus = True
        Me.tx_e開始納期D.CanNextSetFocus = True
        Me.tx_e開始納期D.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_e開始納期D.EditMode = False
        Me.tx_e開始納期D.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_e開始納期D.Location = New System.Drawing.Point(787, 108)
        Me.tx_e開始納期D.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_e開始納期D.MaxLength = 2
        Me.tx_e開始納期D.Name = "tx_e開始納期D"
        Me.tx_e開始納期D.OldValue = "DD"
        Me.tx_e開始納期D.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_e開始納期D.SelectText = True
        Me.tx_e開始納期D.SelLength = 0
        Me.tx_e開始納期D.SelStart = 0
        Me.tx_e開始納期D.SelText = ""
        Me.tx_e開始納期D.Size = New System.Drawing.Size(20, 16)
        Me.tx_e開始納期D.TabIndex = 26
        Me.tx_e開始納期D.Text = "DD"
        '
        'tx_e開始納期M
        '
        Me.tx_e開始納期M.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_e開始納期M.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_e開始納期M.CanForwardSetFocus = True
        Me.tx_e開始納期M.CanNextSetFocus = True
        Me.tx_e開始納期M.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_e開始納期M.EditMode = False
        Me.tx_e開始納期M.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_e開始納期M.Location = New System.Drawing.Point(749, 108)
        Me.tx_e開始納期M.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_e開始納期M.MaxLength = 2
        Me.tx_e開始納期M.Name = "tx_e開始納期M"
        Me.tx_e開始納期M.OldValue = "MM"
        Me.tx_e開始納期M.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_e開始納期M.SelectText = True
        Me.tx_e開始納期M.SelLength = 0
        Me.tx_e開始納期M.SelStart = 0
        Me.tx_e開始納期M.SelText = ""
        Me.tx_e開始納期M.Size = New System.Drawing.Size(20, 16)
        Me.tx_e開始納期M.TabIndex = 25
        Me.tx_e開始納期M.Text = "MM"
        '
        'tx_e開始納期Y
        '
        Me.tx_e開始納期Y.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_e開始納期Y.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_e開始納期Y.CanForwardSetFocus = True
        Me.tx_e開始納期Y.CanNextSetFocus = True
        Me.tx_e開始納期Y.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_e開始納期Y.EditMode = False
        Me.tx_e開始納期Y.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_e開始納期Y.Location = New System.Drawing.Point(699, 108)
        Me.tx_e開始納期Y.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_e開始納期Y.MaxLength = 4
        Me.tx_e開始納期Y.Name = "tx_e開始納期Y"
        Me.tx_e開始納期Y.OldValue = "YYYY"
        Me.tx_e開始納期Y.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_e開始納期Y.SelectText = True
        Me.tx_e開始納期Y.SelLength = 0
        Me.tx_e開始納期Y.SelStart = 0
        Me.tx_e開始納期Y.SelText = ""
        Me.tx_e開始納期Y.Size = New System.Drawing.Size(32, 16)
        Me.tx_e開始納期Y.TabIndex = 24
        Me.tx_e開始納期Y.Text = "YYYY"
        '
        'tx_s完了日D
        '
        Me.tx_s完了日D.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_s完了日D.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_s完了日D.CanForwardSetFocus = True
        Me.tx_s完了日D.CanNextSetFocus = True
        Me.tx_s完了日D.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_s完了日D.EditMode = False
        Me.tx_s完了日D.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_s完了日D.Location = New System.Drawing.Point(624, 148)
        Me.tx_s完了日D.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_s完了日D.MaxLength = 2
        Me.tx_s完了日D.Name = "tx_s完了日D"
        Me.tx_s完了日D.OldValue = "DD"
        Me.tx_s完了日D.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_s完了日D.SelectText = True
        Me.tx_s完了日D.SelLength = 0
        Me.tx_s完了日D.SelStart = 0
        Me.tx_s完了日D.SelText = ""
        Me.tx_s完了日D.Size = New System.Drawing.Size(20, 16)
        Me.tx_s完了日D.TabIndex = 35
        Me.tx_s完了日D.Text = "DD"
        '
        'tx_s完了日M
        '
        Me.tx_s完了日M.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_s完了日M.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_s完了日M.CanForwardSetFocus = True
        Me.tx_s完了日M.CanNextSetFocus = True
        Me.tx_s完了日M.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_s完了日M.EditMode = False
        Me.tx_s完了日M.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_s完了日M.Location = New System.Drawing.Point(586, 148)
        Me.tx_s完了日M.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_s完了日M.MaxLength = 2
        Me.tx_s完了日M.Name = "tx_s完了日M"
        Me.tx_s完了日M.OldValue = "MM"
        Me.tx_s完了日M.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_s完了日M.SelectText = True
        Me.tx_s完了日M.SelLength = 0
        Me.tx_s完了日M.SelStart = 0
        Me.tx_s完了日M.SelText = ""
        Me.tx_s完了日M.Size = New System.Drawing.Size(20, 16)
        Me.tx_s完了日M.TabIndex = 34
        Me.tx_s完了日M.Text = "MM"
        '
        'tx_s完了日Y
        '
        Me.tx_s完了日Y.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_s完了日Y.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_s完了日Y.CanForwardSetFocus = True
        Me.tx_s完了日Y.CanNextSetFocus = True
        Me.tx_s完了日Y.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_s完了日Y.EditMode = False
        Me.tx_s完了日Y.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_s完了日Y.Location = New System.Drawing.Point(535, 148)
        Me.tx_s完了日Y.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_s完了日Y.MaxLength = 4
        Me.tx_s完了日Y.Name = "tx_s完了日Y"
        Me.tx_s完了日Y.OldValue = "YYYY"
        Me.tx_s完了日Y.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_s完了日Y.SelectText = True
        Me.tx_s完了日Y.SelLength = 0
        Me.tx_s完了日Y.SelStart = 0
        Me.tx_s完了日Y.SelText = ""
        Me.tx_s完了日Y.Size = New System.Drawing.Size(32, 16)
        Me.tx_s完了日Y.TabIndex = 33
        Me.tx_s完了日Y.Text = "YYYY"
        '
        'tx_e完了日D
        '
        Me.tx_e完了日D.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_e完了日D.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_e完了日D.CanForwardSetFocus = True
        Me.tx_e完了日D.CanNextSetFocus = True
        Me.tx_e完了日D.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_e完了日D.EditMode = False
        Me.tx_e完了日D.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_e完了日D.Location = New System.Drawing.Point(787, 148)
        Me.tx_e完了日D.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_e完了日D.MaxLength = 2
        Me.tx_e完了日D.Name = "tx_e完了日D"
        Me.tx_e完了日D.OldValue = "DD"
        Me.tx_e完了日D.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_e完了日D.SelectText = True
        Me.tx_e完了日D.SelLength = 0
        Me.tx_e完了日D.SelStart = 0
        Me.tx_e完了日D.SelText = ""
        Me.tx_e完了日D.Size = New System.Drawing.Size(20, 16)
        Me.tx_e完了日D.TabIndex = 38
        Me.tx_e完了日D.Text = "DD"
        '
        'tx_e完了日M
        '
        Me.tx_e完了日M.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_e完了日M.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_e完了日M.CanForwardSetFocus = True
        Me.tx_e完了日M.CanNextSetFocus = True
        Me.tx_e完了日M.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_e完了日M.EditMode = False
        Me.tx_e完了日M.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_e完了日M.Location = New System.Drawing.Point(749, 148)
        Me.tx_e完了日M.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_e完了日M.MaxLength = 2
        Me.tx_e完了日M.Name = "tx_e完了日M"
        Me.tx_e完了日M.OldValue = "MM"
        Me.tx_e完了日M.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_e完了日M.SelectText = True
        Me.tx_e完了日M.SelLength = 0
        Me.tx_e完了日M.SelStart = 0
        Me.tx_e完了日M.SelText = ""
        Me.tx_e完了日M.Size = New System.Drawing.Size(20, 16)
        Me.tx_e完了日M.TabIndex = 37
        Me.tx_e完了日M.Text = "MM"
        '
        'tx_e完了日Y
        '
        Me.tx_e完了日Y.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_e完了日Y.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_e完了日Y.CanForwardSetFocus = True
        Me.tx_e完了日Y.CanNextSetFocus = True
        Me.tx_e完了日Y.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_e完了日Y.EditMode = False
        Me.tx_e完了日Y.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_e完了日Y.Location = New System.Drawing.Point(699, 148)
        Me.tx_e完了日Y.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_e完了日Y.MaxLength = 4
        Me.tx_e完了日Y.Name = "tx_e完了日Y"
        Me.tx_e完了日Y.OldValue = "YYYY"
        Me.tx_e完了日Y.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_e完了日Y.SelectText = True
        Me.tx_e完了日Y.SelLength = 0
        Me.tx_e完了日Y.SelStart = 0
        Me.tx_e完了日Y.SelText = ""
        Me.tx_e完了日Y.Size = New System.Drawing.Size(32, 16)
        Me.tx_e完了日Y.TabIndex = 36
        Me.tx_e完了日Y.Text = "YYYY"
        '
        'tx_ウエルシア物件区分
        '
        Me.tx_ウエルシア物件区分.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_ウエルシア物件区分.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_ウエルシア物件区分.CanForwardSetFocus = True
        Me.tx_ウエルシア物件区分.CanNextSetFocus = True
        Me.tx_ウエルシア物件区分.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_ウエルシア物件区分.DecimalPlace = CType(0, Short)
        Me.tx_ウエルシア物件区分.EditMode = False
        Me.tx_ウエルシア物件区分.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_ウエルシア物件区分.FormatType = ""
        Me.tx_ウエルシア物件区分.FormatTypeNega = ""
        Me.tx_ウエルシア物件区分.FormatTypeNull = ""
        Me.tx_ウエルシア物件区分.FormatTypeZero = ""
        Me.tx_ウエルシア物件区分.InputMinus = True
        Me.tx_ウエルシア物件区分.InputPlus = True
        Me.tx_ウエルシア物件区分.InputZero = False
        Me.tx_ウエルシア物件区分.Location = New System.Drawing.Point(533, 66)
        Me.tx_ウエルシア物件区分.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_ウエルシア物件区分.MaxLength = 9
        Me.tx_ウエルシア物件区分.Name = "tx_ウエルシア物件区分"
        Me.tx_ウエルシア物件区分.OldValue = ""
        Me.tx_ウエルシア物件区分.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_ウエルシア物件区分.SelectText = True
        Me.tx_ウエルシア物件区分.SelLength = 0
        Me.tx_ウエルシア物件区分.SelStart = 0
        Me.tx_ウエルシア物件区分.SelText = ""
        Me.tx_ウエルシア物件区分.Size = New System.Drawing.Size(20, 22)
        Me.tx_ウエルシア物件区分.TabIndex = 14
        '
        'tx_s請求予定D
        '
        Me.tx_s請求予定D.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_s請求予定D.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_s請求予定D.CanForwardSetFocus = True
        Me.tx_s請求予定D.CanNextSetFocus = True
        Me.tx_s請求予定D.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_s請求予定D.EditMode = False
        Me.tx_s請求予定D.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_s請求予定D.Location = New System.Drawing.Point(624, 167)
        Me.tx_s請求予定D.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_s請求予定D.MaxLength = 2
        Me.tx_s請求予定D.Name = "tx_s請求予定D"
        Me.tx_s請求予定D.OldValue = "DD"
        Me.tx_s請求予定D.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_s請求予定D.SelectText = True
        Me.tx_s請求予定D.SelLength = 0
        Me.tx_s請求予定D.SelStart = 0
        Me.tx_s請求予定D.SelText = ""
        Me.tx_s請求予定D.Size = New System.Drawing.Size(20, 16)
        Me.tx_s請求予定D.TabIndex = 41
        Me.tx_s請求予定D.Text = "DD"
        '
        'tx_s請求予定M
        '
        Me.tx_s請求予定M.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_s請求予定M.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_s請求予定M.CanForwardSetFocus = True
        Me.tx_s請求予定M.CanNextSetFocus = True
        Me.tx_s請求予定M.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_s請求予定M.EditMode = False
        Me.tx_s請求予定M.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_s請求予定M.Location = New System.Drawing.Point(586, 167)
        Me.tx_s請求予定M.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_s請求予定M.MaxLength = 2
        Me.tx_s請求予定M.Name = "tx_s請求予定M"
        Me.tx_s請求予定M.OldValue = "MM"
        Me.tx_s請求予定M.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_s請求予定M.SelectText = True
        Me.tx_s請求予定M.SelLength = 0
        Me.tx_s請求予定M.SelStart = 0
        Me.tx_s請求予定M.SelText = ""
        Me.tx_s請求予定M.Size = New System.Drawing.Size(20, 16)
        Me.tx_s請求予定M.TabIndex = 40
        Me.tx_s請求予定M.Text = "MM"
        '
        'tx_s請求予定Y
        '
        Me.tx_s請求予定Y.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_s請求予定Y.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_s請求予定Y.CanForwardSetFocus = True
        Me.tx_s請求予定Y.CanNextSetFocus = True
        Me.tx_s請求予定Y.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_s請求予定Y.EditMode = False
        Me.tx_s請求予定Y.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_s請求予定Y.Location = New System.Drawing.Point(535, 167)
        Me.tx_s請求予定Y.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_s請求予定Y.MaxLength = 4
        Me.tx_s請求予定Y.Name = "tx_s請求予定Y"
        Me.tx_s請求予定Y.OldValue = "YYYY"
        Me.tx_s請求予定Y.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_s請求予定Y.SelectText = True
        Me.tx_s請求予定Y.SelLength = 0
        Me.tx_s請求予定Y.SelStart = 0
        Me.tx_s請求予定Y.SelText = ""
        Me.tx_s請求予定Y.Size = New System.Drawing.Size(32, 16)
        Me.tx_s請求予定Y.TabIndex = 39
        Me.tx_s請求予定Y.Text = "YYYY"
        '
        'tx_e請求予定D
        '
        Me.tx_e請求予定D.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_e請求予定D.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_e請求予定D.CanForwardSetFocus = True
        Me.tx_e請求予定D.CanNextSetFocus = True
        Me.tx_e請求予定D.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_e請求予定D.EditMode = False
        Me.tx_e請求予定D.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_e請求予定D.Location = New System.Drawing.Point(787, 167)
        Me.tx_e請求予定D.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_e請求予定D.MaxLength = 2
        Me.tx_e請求予定D.Name = "tx_e請求予定D"
        Me.tx_e請求予定D.OldValue = "DD"
        Me.tx_e請求予定D.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_e請求予定D.SelectText = True
        Me.tx_e請求予定D.SelLength = 0
        Me.tx_e請求予定D.SelStart = 0
        Me.tx_e請求予定D.SelText = ""
        Me.tx_e請求予定D.Size = New System.Drawing.Size(20, 16)
        Me.tx_e請求予定D.TabIndex = 44
        Me.tx_e請求予定D.Text = "DD"
        '
        'tx_e請求予定M
        '
        Me.tx_e請求予定M.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_e請求予定M.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_e請求予定M.CanForwardSetFocus = True
        Me.tx_e請求予定M.CanNextSetFocus = True
        Me.tx_e請求予定M.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_e請求予定M.EditMode = False
        Me.tx_e請求予定M.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_e請求予定M.Location = New System.Drawing.Point(749, 167)
        Me.tx_e請求予定M.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_e請求予定M.MaxLength = 2
        Me.tx_e請求予定M.Name = "tx_e請求予定M"
        Me.tx_e請求予定M.OldValue = "MM"
        Me.tx_e請求予定M.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_e請求予定M.SelectText = True
        Me.tx_e請求予定M.SelLength = 0
        Me.tx_e請求予定M.SelStart = 0
        Me.tx_e請求予定M.SelText = ""
        Me.tx_e請求予定M.Size = New System.Drawing.Size(20, 16)
        Me.tx_e請求予定M.TabIndex = 43
        Me.tx_e請求予定M.Text = "MM"
        '
        'tx_e請求予定Y
        '
        Me.tx_e請求予定Y.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_e請求予定Y.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_e請求予定Y.CanForwardSetFocus = True
        Me.tx_e請求予定Y.CanNextSetFocus = True
        Me.tx_e請求予定Y.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_e請求予定Y.EditMode = False
        Me.tx_e請求予定Y.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_e請求予定Y.Location = New System.Drawing.Point(699, 167)
        Me.tx_e請求予定Y.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_e請求予定Y.MaxLength = 4
        Me.tx_e請求予定Y.Name = "tx_e請求予定Y"
        Me.tx_e請求予定Y.OldValue = "YYYY"
        Me.tx_e請求予定Y.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_e請求予定Y.SelectText = True
        Me.tx_e請求予定Y.SelLength = 0
        Me.tx_e請求予定Y.SelStart = 0
        Me.tx_e請求予定Y.SelText = ""
        Me.tx_e請求予定Y.Size = New System.Drawing.Size(32, 16)
        Me.tx_e請求予定Y.TabIndex = 42
        Me.tx_e請求予定Y.Text = "YYYY"
        '
        'tx_ウエルシアリース区分
        '
        Me.tx_ウエルシアリース区分.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_ウエルシアリース区分.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_ウエルシアリース区分.CanForwardSetFocus = True
        Me.tx_ウエルシアリース区分.CanNextSetFocus = True
        Me.tx_ウエルシアリース区分.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_ウエルシアリース区分.DecimalPlace = CType(0, Short)
        Me.tx_ウエルシアリース区分.EditMode = False
        Me.tx_ウエルシアリース区分.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_ウエルシアリース区分.FormatType = ""
        Me.tx_ウエルシアリース区分.FormatTypeNega = ""
        Me.tx_ウエルシアリース区分.FormatTypeNull = ""
        Me.tx_ウエルシアリース区分.FormatTypeZero = ""
        Me.tx_ウエルシアリース区分.InputMinus = True
        Me.tx_ウエルシアリース区分.InputPlus = True
        Me.tx_ウエルシアリース区分.InputZero = False
        Me.tx_ウエルシアリース区分.Location = New System.Drawing.Point(533, 46)
        Me.tx_ウエルシアリース区分.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_ウエルシアリース区分.MaxLength = 9
        Me.tx_ウエルシアリース区分.Name = "tx_ウエルシアリース区分"
        Me.tx_ウエルシアリース区分.OldValue = ""
        Me.tx_ウエルシアリース区分.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_ウエルシアリース区分.SelectText = True
        Me.tx_ウエルシアリース区分.SelLength = 0
        Me.tx_ウエルシアリース区分.SelStart = 0
        Me.tx_ウエルシアリース区分.SelText = ""
        Me.tx_ウエルシアリース区分.Size = New System.Drawing.Size(20, 22)
        Me.tx_ウエルシアリース区分.TabIndex = 13
        '
        'tx_s仕入日D
        '
        Me.tx_s仕入日D.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_s仕入日D.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_s仕入日D.CanForwardSetFocus = True
        Me.tx_s仕入日D.CanNextSetFocus = True
        Me.tx_s仕入日D.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_s仕入日D.EditMode = False
        Me.tx_s仕入日D.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_s仕入日D.Location = New System.Drawing.Point(624, 127)
        Me.tx_s仕入日D.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_s仕入日D.MaxLength = 2
        Me.tx_s仕入日D.Name = "tx_s仕入日D"
        Me.tx_s仕入日D.OldValue = "DD"
        Me.tx_s仕入日D.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_s仕入日D.SelectText = True
        Me.tx_s仕入日D.SelLength = 0
        Me.tx_s仕入日D.SelStart = 0
        Me.tx_s仕入日D.SelText = ""
        Me.tx_s仕入日D.Size = New System.Drawing.Size(20, 16)
        Me.tx_s仕入日D.TabIndex = 29
        Me.tx_s仕入日D.Text = "DD"
        '
        'tx_s仕入日M
        '
        Me.tx_s仕入日M.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_s仕入日M.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_s仕入日M.CanForwardSetFocus = True
        Me.tx_s仕入日M.CanNextSetFocus = True
        Me.tx_s仕入日M.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_s仕入日M.EditMode = False
        Me.tx_s仕入日M.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_s仕入日M.Location = New System.Drawing.Point(586, 127)
        Me.tx_s仕入日M.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_s仕入日M.MaxLength = 2
        Me.tx_s仕入日M.Name = "tx_s仕入日M"
        Me.tx_s仕入日M.OldValue = "MM"
        Me.tx_s仕入日M.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_s仕入日M.SelectText = True
        Me.tx_s仕入日M.SelLength = 0
        Me.tx_s仕入日M.SelStart = 0
        Me.tx_s仕入日M.SelText = ""
        Me.tx_s仕入日M.Size = New System.Drawing.Size(20, 16)
        Me.tx_s仕入日M.TabIndex = 28
        Me.tx_s仕入日M.Text = "MM"
        '
        'tx_s仕入日Y
        '
        Me.tx_s仕入日Y.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_s仕入日Y.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_s仕入日Y.CanForwardSetFocus = True
        Me.tx_s仕入日Y.CanNextSetFocus = True
        Me.tx_s仕入日Y.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_s仕入日Y.EditMode = False
        Me.tx_s仕入日Y.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_s仕入日Y.Location = New System.Drawing.Point(535, 127)
        Me.tx_s仕入日Y.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_s仕入日Y.MaxLength = 4
        Me.tx_s仕入日Y.Name = "tx_s仕入日Y"
        Me.tx_s仕入日Y.OldValue = "YYYY"
        Me.tx_s仕入日Y.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_s仕入日Y.SelectText = True
        Me.tx_s仕入日Y.SelLength = 0
        Me.tx_s仕入日Y.SelStart = 0
        Me.tx_s仕入日Y.SelText = ""
        Me.tx_s仕入日Y.Size = New System.Drawing.Size(32, 16)
        Me.tx_s仕入日Y.TabIndex = 27
        Me.tx_s仕入日Y.Text = "YYYY"
        '
        'tx_e仕入日D
        '
        Me.tx_e仕入日D.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_e仕入日D.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_e仕入日D.CanForwardSetFocus = True
        Me.tx_e仕入日D.CanNextSetFocus = True
        Me.tx_e仕入日D.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_e仕入日D.EditMode = False
        Me.tx_e仕入日D.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_e仕入日D.Location = New System.Drawing.Point(787, 127)
        Me.tx_e仕入日D.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_e仕入日D.MaxLength = 2
        Me.tx_e仕入日D.Name = "tx_e仕入日D"
        Me.tx_e仕入日D.OldValue = "DD"
        Me.tx_e仕入日D.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_e仕入日D.SelectText = True
        Me.tx_e仕入日D.SelLength = 0
        Me.tx_e仕入日D.SelStart = 0
        Me.tx_e仕入日D.SelText = ""
        Me.tx_e仕入日D.Size = New System.Drawing.Size(20, 16)
        Me.tx_e仕入日D.TabIndex = 32
        Me.tx_e仕入日D.Text = "DD"
        '
        'tx_e仕入日M
        '
        Me.tx_e仕入日M.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_e仕入日M.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_e仕入日M.CanForwardSetFocus = True
        Me.tx_e仕入日M.CanNextSetFocus = True
        Me.tx_e仕入日M.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_e仕入日M.EditMode = False
        Me.tx_e仕入日M.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_e仕入日M.Location = New System.Drawing.Point(749, 127)
        Me.tx_e仕入日M.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_e仕入日M.MaxLength = 2
        Me.tx_e仕入日M.Name = "tx_e仕入日M"
        Me.tx_e仕入日M.OldValue = "MM"
        Me.tx_e仕入日M.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_e仕入日M.SelectText = True
        Me.tx_e仕入日M.SelLength = 0
        Me.tx_e仕入日M.SelStart = 0
        Me.tx_e仕入日M.SelText = ""
        Me.tx_e仕入日M.Size = New System.Drawing.Size(20, 16)
        Me.tx_e仕入日M.TabIndex = 31
        Me.tx_e仕入日M.Text = "MM"
        '
        'tx_e仕入日Y
        '
        Me.tx_e仕入日Y.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_e仕入日Y.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_e仕入日Y.CanForwardSetFocus = True
        Me.tx_e仕入日Y.CanNextSetFocus = True
        Me.tx_e仕入日Y.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_e仕入日Y.EditMode = False
        Me.tx_e仕入日Y.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_e仕入日Y.Location = New System.Drawing.Point(699, 127)
        Me.tx_e仕入日Y.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_e仕入日Y.MaxLength = 4
        Me.tx_e仕入日Y.Name = "tx_e仕入日Y"
        Me.tx_e仕入日Y.OldValue = "YYYY"
        Me.tx_e仕入日Y.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_e仕入日Y.SelectText = True
        Me.tx_e仕入日Y.SelLength = 0
        Me.tx_e仕入日Y.SelStart = 0
        Me.tx_e仕入日Y.SelText = ""
        Me.tx_e仕入日Y.Size = New System.Drawing.Size(32, 16)
        Me.tx_e仕入日Y.TabIndex = 30
        Me.tx_e仕入日Y.Text = "YYYY"
        '
        'tx_s物件番号
        '
        Me.tx_s物件番号.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_s物件番号.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_s物件番号.CanForwardSetFocus = True
        Me.tx_s物件番号.CanNextSetFocus = True
        Me.tx_s物件番号.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_s物件番号.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_s物件番号.EditMode = False
        Me.tx_s物件番号.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_s物件番号.IMEMode = System.Windows.Forms.ImeMode.Disable
        Me.tx_s物件番号.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_s物件番号.Location = New System.Drawing.Point(533, 26)
        Me.tx_s物件番号.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_s物件番号.MaxLength = 7
        Me.tx_s物件番号.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_s物件番号.Name = "tx_s物件番号"
        Me.tx_s物件番号.OldValue = "1234567"
        Me.tx_s物件番号.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_s物件番号.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_s物件番号.SelectText = False
        Me.tx_s物件番号.SelLength = 0
        Me.tx_s物件番号.SelStart = 0
        Me.tx_s物件番号.SelText = ""
        Me.tx_s物件番号.Size = New System.Drawing.Size(49, 22)
        Me.tx_s物件番号.TabIndex = 11
        Me.tx_s物件番号.Text = "1234567"
        '
        'tx_e物件番号
        '
        Me.tx_e物件番号.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_e物件番号.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_e物件番号.CanForwardSetFocus = True
        Me.tx_e物件番号.CanNextSetFocus = True
        Me.tx_e物件番号.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_e物件番号.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_e物件番号.EditMode = False
        Me.tx_e物件番号.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_e物件番号.IMEMode = System.Windows.Forms.ImeMode.Disable
        Me.tx_e物件番号.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_e物件番号.Location = New System.Drawing.Point(642, 26)
        Me.tx_e物件番号.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_e物件番号.MaxLength = 7
        Me.tx_e物件番号.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_e物件番号.Name = "tx_e物件番号"
        Me.tx_e物件番号.OldValue = "1234567"
        Me.tx_e物件番号.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_e物件番号.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_e物件番号.SelectText = False
        Me.tx_e物件番号.SelLength = 0
        Me.tx_e物件番号.SelStart = 0
        Me.tx_e物件番号.SelText = ""
        Me.tx_e物件番号.Size = New System.Drawing.Size(49, 22)
        Me.tx_e物件番号.TabIndex = 12
        Me.tx_e物件番号.Text = "1234567"
        '
        'tx_見積確定区分
        '
        Me.tx_見積確定区分.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_見積確定区分.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_見積確定区分.CanForwardSetFocus = True
        Me.tx_見積確定区分.CanNextSetFocus = True
        Me.tx_見積確定区分.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_見積確定区分.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_見積確定区分.EditMode = False
        Me.tx_見積確定区分.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_見積確定区分.IMEMode = System.Windows.Forms.ImeMode.Disable
        Me.tx_見積確定区分.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_見積確定区分.Location = New System.Drawing.Point(533, 186)
        Me.tx_見積確定区分.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_見積確定区分.MaxLength = 2
        Me.tx_見積確定区分.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_見積確定区分.Name = "tx_見積確定区分"
        Me.tx_見積確定区分.OldValue = "ExTextBox"
        Me.tx_見積確定区分.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_見積確定区分.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_見積確定区分.SelectText = True
        Me.tx_見積確定区分.SelLength = 0
        Me.tx_見積確定区分.SelStart = 0
        Me.tx_見積確定区分.SelText = ""
        Me.tx_見積確定区分.Size = New System.Drawing.Size(20, 22)
        Me.tx_見積確定区分.TabIndex = 45
        Me.tx_見積確定区分.Text = "ExTextBox"
        '
        'tx_業種区分
        '
        Me.tx_業種区分.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_業種区分.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_業種区分.CanForwardSetFocus = True
        Me.tx_業種区分.CanNextSetFocus = True
        Me.tx_業種区分.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_業種区分.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_業種区分.EditMode = False
        Me.tx_業種区分.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_業種区分.IMEMode = System.Windows.Forms.ImeMode.Disable
        Me.tx_業種区分.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_業種区分.Location = New System.Drawing.Point(101, 108)
        Me.tx_業種区分.Margin = New System.Windows.Forms.Padding(2)
        Me.tx_業種区分.MaxLength = 2
        Me.tx_業種区分.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_業種区分.Name = "tx_業種区分"
        Me.tx_業種区分.OldValue = "ExTextBox"
        Me.tx_業種区分.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_業種区分.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_業種区分.SelectText = False
        Me.tx_業種区分.SelLength = 0
        Me.tx_業種区分.SelStart = 0
        Me.tx_業種区分.SelText = ""
        Me.tx_業種区分.Size = New System.Drawing.Size(20, 22)
        Me.tx_業種区分.TabIndex = 4
        Me.tx_業種区分.Text = "ExTextBox"
        '
        '_lb_項目_18
        '
        Me._lb_項目_18.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_18.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_18.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_18.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_18.ForeColor = System.Drawing.Color.White
        Me._lb_項目_18.Location = New System.Drawing.Point(9, 108)
        Me._lb_項目_18.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_項目_18.Name = "_lb_項目_18"
        Me._lb_項目_18.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_18.Size = New System.Drawing.Size(91, 19)
        Me._lb_項目_18.TabIndex = 150
        Me._lb_項目_18.Text = "業種区分"
        Me._lb_項目_18.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label30
        '
        Me.Label30.BackColor = System.Drawing.SystemColors.Control
        Me.Label30.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label30.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label30.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label30.Location = New System.Drawing.Point(128, 111)
        Me.Label30.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label30.Name = "Label30"
        Me.Label30.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label30.Size = New System.Drawing.Size(220, 14)
        Me.Label30.TabIndex = 149
        Me.Label30.Text = "0:什器 1:内装"
        '
        '_lblLabels_0
        '
        Me._lblLabels_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_0.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_0.Location = New System.Drawing.Point(551, 189)
        Me._lblLabels_0.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lblLabels_0.Name = "_lblLabels_0"
        Me._lblLabels_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_0.Size = New System.Drawing.Size(131, 15)
        Me._lblLabels_0.TabIndex = 148
        Me._lblLabels_0.Text = "（0:未確定 1:確定）"
        '
        '_lb_項目_17
        '
        Me._lb_項目_17.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_17.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_17.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_17.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_17.ForeColor = System.Drawing.Color.White
        Me._lb_項目_17.Location = New System.Drawing.Point(441, 187)
        Me._lb_項目_17.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_項目_17.Name = "_lb_項目_17"
        Me._lb_項目_17.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_17.Size = New System.Drawing.Size(91, 19)
        Me._lb_項目_17.TabIndex = 147
        Me._lb_項目_17.Text = "見積確定"
        Me._lb_項目_17.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_16
        '
        Me._lb_項目_16.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_16.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_16.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_16.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_16.ForeColor = System.Drawing.Color.White
        Me._lb_項目_16.Location = New System.Drawing.Point(441, 26)
        Me._lb_項目_16.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_項目_16.Name = "_lb_項目_16"
        Me._lb_項目_16.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_16.Size = New System.Drawing.Size(91, 19)
        Me._lb_項目_16.TabIndex = 146
        Me._lb_項目_16.Text = "物件№"
        Me._lb_項目_16.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_物件番号_kara
        '
        Me._lb_物件番号_kara.BackColor = System.Drawing.SystemColors.Control
        Me._lb_物件番号_kara.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_物件番号_kara.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_物件番号_kara.Location = New System.Drawing.Point(607, 28)
        Me._lb_物件番号_kara.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_物件番号_kara.Name = "_lb_物件番号_kara"
        Me._lb_物件番号_kara.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_物件番号_kara.Size = New System.Drawing.Size(13, 10)
        Me._lb_物件番号_kara.TabIndex = 145
        Me._lb_物件番号_kara.Text = "～"
        Me._lb_物件番号_kara.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_仕入日_kara
        '
        Me._lb_仕入日_kara.BackColor = System.Drawing.SystemColors.Control
        Me._lb_仕入日_kara.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_仕入日_kara.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_仕入日_kara.Location = New System.Drawing.Point(670, 131)
        Me._lb_仕入日_kara.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_仕入日_kara.Name = "_lb_仕入日_kara"
        Me._lb_仕入日_kara.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_仕入日_kara.Size = New System.Drawing.Size(13, 10)
        Me._lb_仕入日_kara.TabIndex = 142
        Me._lb_仕入日_kara.Text = "～"
        Me._lb_仕入日_kara.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_s仕入日年
        '
        Me.lb_s仕入日年.BackColor = System.Drawing.SystemColors.Window
        Me.lb_s仕入日年.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_s仕入日年.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_s仕入日年.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_s仕入日年.Location = New System.Drawing.Point(569, 129)
        Me.lb_s仕入日年.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lb_s仕入日年.Name = "lb_s仕入日年"
        Me.lb_s仕入日年.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_s仕入日年.Size = New System.Drawing.Size(14, 14)
        Me.lb_s仕入日年.TabIndex = 141
        Me.lb_s仕入日年.Text = "年"
        Me.lb_s仕入日年.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_s仕入日月
        '
        Me.lb_s仕入日月.BackColor = System.Drawing.SystemColors.Window
        Me.lb_s仕入日月.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_s仕入日月.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_s仕入日月.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_s仕入日月.Location = New System.Drawing.Point(610, 129)
        Me.lb_s仕入日月.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lb_s仕入日月.Name = "lb_s仕入日月"
        Me.lb_s仕入日月.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_s仕入日月.Size = New System.Drawing.Size(14, 14)
        Me.lb_s仕入日月.TabIndex = 140
        Me.lb_s仕入日月.Text = "月"
        Me.lb_s仕入日月.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_s仕入日日
        '
        Me.lb_s仕入日日.BackColor = System.Drawing.SystemColors.Window
        Me.lb_s仕入日日.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_s仕入日日.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_s仕入日日.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_s仕入日日.Location = New System.Drawing.Point(646, 129)
        Me.lb_s仕入日日.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lb_s仕入日日.Name = "lb_s仕入日日"
        Me.lb_s仕入日日.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_s仕入日日.Size = New System.Drawing.Size(14, 14)
        Me.lb_s仕入日日.TabIndex = 139
        Me.lb_s仕入日日.Text = "日"
        Me.lb_s仕入日日.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_e仕入日年
        '
        Me.lb_e仕入日年.BackColor = System.Drawing.SystemColors.Window
        Me.lb_e仕入日年.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_e仕入日年.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_e仕入日年.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_e仕入日年.Location = New System.Drawing.Point(732, 129)
        Me.lb_e仕入日年.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lb_e仕入日年.Name = "lb_e仕入日年"
        Me.lb_e仕入日年.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_e仕入日年.Size = New System.Drawing.Size(14, 14)
        Me.lb_e仕入日年.TabIndex = 138
        Me.lb_e仕入日年.Text = "年"
        Me.lb_e仕入日年.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_e仕入日月
        '
        Me.lb_e仕入日月.BackColor = System.Drawing.SystemColors.Window
        Me.lb_e仕入日月.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_e仕入日月.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_e仕入日月.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_e仕入日月.Location = New System.Drawing.Point(772, 129)
        Me.lb_e仕入日月.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lb_e仕入日月.Name = "lb_e仕入日月"
        Me.lb_e仕入日月.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_e仕入日月.Size = New System.Drawing.Size(14, 14)
        Me.lb_e仕入日月.TabIndex = 137
        Me.lb_e仕入日月.Text = "月"
        Me.lb_e仕入日月.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_e仕入日日
        '
        Me.lb_e仕入日日.BackColor = System.Drawing.SystemColors.Window
        Me.lb_e仕入日日.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_e仕入日日.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_e仕入日日.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_e仕入日日.Location = New System.Drawing.Point(809, 129)
        Me.lb_e仕入日日.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lb_e仕入日日.Name = "lb_e仕入日日"
        Me.lb_e仕入日日.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_e仕入日日.Size = New System.Drawing.Size(14, 14)
        Me.lb_e仕入日日.TabIndex = 136
        Me.lb_e仕入日日.Text = "日"
        Me.lb_e仕入日日.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_15
        '
        Me._lb_項目_15.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_15.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_15.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_15.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_15.ForeColor = System.Drawing.Color.White
        Me._lb_項目_15.Location = New System.Drawing.Point(441, 126)
        Me._lb_項目_15.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_項目_15.Name = "_lb_項目_15"
        Me._lb_項目_15.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_15.Size = New System.Drawing.Size(91, 19)
        Me._lb_項目_15.TabIndex = 135
        Me._lb_項目_15.Text = "仕入日"
        Me._lb_項目_15.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_s請求予定日
        '
        Me.lb_s請求予定日.BackColor = System.Drawing.SystemColors.Window
        Me.lb_s請求予定日.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_s請求予定日.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_s請求予定日.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_s請求予定日.Location = New System.Drawing.Point(646, 169)
        Me.lb_s請求予定日.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lb_s請求予定日.Name = "lb_s請求予定日"
        Me.lb_s請求予定日.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_s請求予定日.Size = New System.Drawing.Size(14, 14)
        Me.lb_s請求予定日.TabIndex = 134
        Me.lb_s請求予定日.Text = "日"
        Me.lb_s請求予定日.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_e請求予定日
        '
        Me.lb_e請求予定日.BackColor = System.Drawing.SystemColors.Window
        Me.lb_e請求予定日.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_e請求予定日.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_e請求予定日.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_e請求予定日.Location = New System.Drawing.Point(809, 169)
        Me.lb_e請求予定日.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lb_e請求予定日.Name = "lb_e請求予定日"
        Me.lb_e請求予定日.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_e請求予定日.Size = New System.Drawing.Size(14, 14)
        Me.lb_e請求予定日.TabIndex = 133
        Me.lb_e請求予定日.Text = "日"
        Me.lb_e請求予定日.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_14
        '
        Me._lb_項目_14.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_14.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_14.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_14.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_14.ForeColor = System.Drawing.Color.White
        Me._lb_項目_14.Location = New System.Drawing.Point(708, 28)
        Me._lb_項目_14.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_項目_14.Name = "_lb_項目_14"
        Me._lb_項目_14.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_14.Size = New System.Drawing.Size(91, 21)
        Me._lb_項目_14.TabIndex = 130
        Me._lb_項目_14.Text = "出力先"
        Me._lb_項目_14.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_13
        '
        Me._lb_項目_13.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_13.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_13.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_13.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_13.ForeColor = System.Drawing.Color.White
        Me._lb_項目_13.Location = New System.Drawing.Point(441, 46)
        Me._lb_項目_13.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_項目_13.Name = "_lb_項目_13"
        Me._lb_項目_13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_13.Size = New System.Drawing.Size(91, 19)
        Me._lb_項目_13.TabIndex = 129
        Me._lb_項目_13.Text = "W リース区分"
        Me._lb_項目_13.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblLabels_33
        '
        Me._lblLabels_33.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_33.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_33.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_33.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_33.Location = New System.Drawing.Point(554, 49)
        Me._lblLabels_33.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lblLabels_33.Name = "_lblLabels_33"
        Me._lblLabels_33.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_33.Size = New System.Drawing.Size(130, 12)
        Me._lblLabels_33.TabIndex = 128
        Me._lblLabels_33.Text = "（1:通常 2:リース）"
        '
        '_lb_項目_12
        '
        Me._lb_項目_12.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_12.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_12.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_12.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_12.ForeColor = System.Drawing.Color.White
        Me._lb_項目_12.Location = New System.Drawing.Point(441, 66)
        Me._lb_項目_12.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_項目_12.Name = "_lb_項目_12"
        Me._lb_項目_12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_12.Size = New System.Drawing.Size(91, 19)
        Me._lb_項目_12.TabIndex = 127
        Me._lb_項目_12.Text = "W 請求管轄"
        Me._lb_項目_12.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb請求予定_kara
        '
        Me._lb請求予定_kara.BackColor = System.Drawing.SystemColors.Control
        Me._lb請求予定_kara.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb請求予定_kara.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb請求予定_kara.Location = New System.Drawing.Point(670, 171)
        Me._lb請求予定_kara.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb請求予定_kara.Name = "_lb請求予定_kara"
        Me._lb請求予定_kara.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb請求予定_kara.Size = New System.Drawing.Size(13, 10)
        Me._lb請求予定_kara.TabIndex = 124
        Me._lb請求予定_kara.Text = "～"
        Me._lb請求予定_kara.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_s請求予定年
        '
        Me.lb_s請求予定年.BackColor = System.Drawing.SystemColors.Window
        Me.lb_s請求予定年.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_s請求予定年.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_s請求予定年.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_s請求予定年.Location = New System.Drawing.Point(569, 169)
        Me.lb_s請求予定年.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lb_s請求予定年.Name = "lb_s請求予定年"
        Me.lb_s請求予定年.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_s請求予定年.Size = New System.Drawing.Size(14, 14)
        Me.lb_s請求予定年.TabIndex = 123
        Me.lb_s請求予定年.Text = "年"
        Me.lb_s請求予定年.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_s請求予定月
        '
        Me.lb_s請求予定月.BackColor = System.Drawing.SystemColors.Window
        Me.lb_s請求予定月.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_s請求予定月.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_s請求予定月.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_s請求予定月.Location = New System.Drawing.Point(610, 169)
        Me.lb_s請求予定月.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lb_s請求予定月.Name = "lb_s請求予定月"
        Me.lb_s請求予定月.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_s請求予定月.Size = New System.Drawing.Size(14, 14)
        Me.lb_s請求予定月.TabIndex = 122
        Me.lb_s請求予定月.Text = "月"
        Me.lb_s請求予定月.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_e請求予定年
        '
        Me.lb_e請求予定年.BackColor = System.Drawing.SystemColors.Window
        Me.lb_e請求予定年.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_e請求予定年.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_e請求予定年.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_e請求予定年.Location = New System.Drawing.Point(732, 169)
        Me.lb_e請求予定年.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lb_e請求予定年.Name = "lb_e請求予定年"
        Me.lb_e請求予定年.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_e請求予定年.Size = New System.Drawing.Size(14, 14)
        Me.lb_e請求予定年.TabIndex = 121
        Me.lb_e請求予定年.Text = "年"
        Me.lb_e請求予定年.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_e請求予定月
        '
        Me.lb_e請求予定月.BackColor = System.Drawing.SystemColors.Window
        Me.lb_e請求予定月.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_e請求予定月.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_e請求予定月.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_e請求予定月.Location = New System.Drawing.Point(772, 169)
        Me.lb_e請求予定月.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lb_e請求予定月.Name = "lb_e請求予定月"
        Me.lb_e請求予定月.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_e請求予定月.Size = New System.Drawing.Size(14, 14)
        Me.lb_e請求予定月.TabIndex = 120
        Me.lb_e請求予定月.Text = "月"
        Me.lb_e請求予定月.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_11
        '
        Me._lb_項目_11.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_11.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_11.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_11.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_11.ForeColor = System.Drawing.Color.White
        Me._lb_項目_11.Location = New System.Drawing.Point(441, 167)
        Me._lb_項目_11.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_項目_11.Name = "_lb_項目_11"
        Me._lb_項目_11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_11.Size = New System.Drawing.Size(91, 19)
        Me._lb_項目_11.TabIndex = 119
        Me._lb_項目_11.Text = "請求予定日"
        Me._lb_項目_11.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'rf_ウエルシア物件区分名
        '
        Me.rf_ウエルシア物件区分名.BackColor = System.Drawing.SystemColors.Control
        Me.rf_ウエルシア物件区分名.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.rf_ウエルシア物件区分名.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_ウエルシア物件区分名.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_ウエルシア物件区分名.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_ウエルシア物件区分名.Location = New System.Drawing.Point(554, 67)
        Me.rf_ウエルシア物件区分名.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.rf_ウエルシア物件区分名.Name = "rf_ウエルシア物件区分名"
        Me.rf_ウエルシア物件区分名.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_ウエルシア物件区分名.Size = New System.Drawing.Size(160, 15)
        Me.rf_ウエルシア物件区分名.TabIndex = 118
        Me.rf_ウエルシア物件区分名.Text = "ＷＷＷＷＷＷＷＷ"
        '
        '_lb_項目_10
        '
        Me._lb_項目_10.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_10.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_10.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_10.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_10.ForeColor = System.Drawing.Color.White
        Me._lb_項目_10.Location = New System.Drawing.Point(441, 146)
        Me._lb_項目_10.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_項目_10.Name = "_lb_項目_10"
        Me._lb_項目_10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_10.Size = New System.Drawing.Size(91, 19)
        Me._lb_項目_10.TabIndex = 115
        Me._lb_項目_10.Text = "完了日"
        Me._lb_項目_10.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_e完了日日
        '
        Me.lb_e完了日日.BackColor = System.Drawing.SystemColors.Window
        Me.lb_e完了日日.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_e完了日日.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_e完了日日.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_e完了日日.Location = New System.Drawing.Point(809, 149)
        Me.lb_e完了日日.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lb_e完了日日.Name = "lb_e完了日日"
        Me.lb_e完了日日.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_e完了日日.Size = New System.Drawing.Size(14, 14)
        Me.lb_e完了日日.TabIndex = 114
        Me.lb_e完了日日.Text = "日"
        Me.lb_e完了日日.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_e完了日月
        '
        Me.lb_e完了日月.BackColor = System.Drawing.SystemColors.Window
        Me.lb_e完了日月.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_e完了日月.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_e完了日月.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_e完了日月.Location = New System.Drawing.Point(772, 149)
        Me.lb_e完了日月.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lb_e完了日月.Name = "lb_e完了日月"
        Me.lb_e完了日月.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_e完了日月.Size = New System.Drawing.Size(14, 14)
        Me.lb_e完了日月.TabIndex = 113
        Me.lb_e完了日月.Text = "月"
        Me.lb_e完了日月.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_e完了日年
        '
        Me.lb_e完了日年.BackColor = System.Drawing.SystemColors.Window
        Me.lb_e完了日年.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_e完了日年.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_e完了日年.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_e完了日年.Location = New System.Drawing.Point(732, 149)
        Me.lb_e完了日年.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lb_e完了日年.Name = "lb_e完了日年"
        Me.lb_e完了日年.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_e完了日年.Size = New System.Drawing.Size(14, 14)
        Me.lb_e完了日年.TabIndex = 112
        Me.lb_e完了日年.Text = "年"
        Me.lb_e完了日年.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_s完了日日
        '
        Me.lb_s完了日日.BackColor = System.Drawing.SystemColors.Window
        Me.lb_s完了日日.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_s完了日日.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_s完了日日.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_s完了日日.Location = New System.Drawing.Point(646, 149)
        Me.lb_s完了日日.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lb_s完了日日.Name = "lb_s完了日日"
        Me.lb_s完了日日.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_s完了日日.Size = New System.Drawing.Size(14, 14)
        Me.lb_s完了日日.TabIndex = 111
        Me.lb_s完了日日.Text = "日"
        Me.lb_s完了日日.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_s完了日月
        '
        Me.lb_s完了日月.BackColor = System.Drawing.SystemColors.Window
        Me.lb_s完了日月.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_s完了日月.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_s完了日月.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_s完了日月.Location = New System.Drawing.Point(610, 149)
        Me.lb_s完了日月.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lb_s完了日月.Name = "lb_s完了日月"
        Me.lb_s完了日月.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_s完了日月.Size = New System.Drawing.Size(14, 14)
        Me.lb_s完了日月.TabIndex = 110
        Me.lb_s完了日月.Text = "月"
        Me.lb_s完了日月.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_s完了日年
        '
        Me.lb_s完了日年.BackColor = System.Drawing.SystemColors.Window
        Me.lb_s完了日年.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_s完了日年.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_s完了日年.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_s完了日年.Location = New System.Drawing.Point(569, 149)
        Me.lb_s完了日年.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lb_s完了日年.Name = "lb_s完了日年"
        Me.lb_s完了日年.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_s完了日年.Size = New System.Drawing.Size(14, 14)
        Me.lb_s完了日年.TabIndex = 109
        Me.lb_s完了日年.Text = "年"
        Me.lb_s完了日年.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_完了日_kara
        '
        Me._lb_完了日_kara.BackColor = System.Drawing.SystemColors.Control
        Me._lb_完了日_kara.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_完了日_kara.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_完了日_kara.Location = New System.Drawing.Point(670, 151)
        Me._lb_完了日_kara.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_完了日_kara.Name = "_lb_完了日_kara"
        Me._lb_完了日_kara.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_完了日_kara.Size = New System.Drawing.Size(13, 10)
        Me._lb_完了日_kara.TabIndex = 108
        Me._lb_完了日_kara.Text = "～"
        Me._lb_完了日_kara.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_開始納期_kara
        '
        Me._lb_開始納期_kara.BackColor = System.Drawing.SystemColors.Control
        Me._lb_開始納期_kara.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_開始納期_kara.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_開始納期_kara.Location = New System.Drawing.Point(670, 110)
        Me._lb_開始納期_kara.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_開始納期_kara.Name = "_lb_開始納期_kara"
        Me._lb_開始納期_kara.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_開始納期_kara.Size = New System.Drawing.Size(13, 10)
        Me._lb_開始納期_kara.TabIndex = 106
        Me._lb_開始納期_kara.Text = "～"
        Me._lb_開始納期_kara.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_s開始納期年
        '
        Me.lb_s開始納期年.BackColor = System.Drawing.SystemColors.Window
        Me.lb_s開始納期年.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_s開始納期年.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_s開始納期年.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_s開始納期年.Location = New System.Drawing.Point(569, 109)
        Me.lb_s開始納期年.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lb_s開始納期年.Name = "lb_s開始納期年"
        Me.lb_s開始納期年.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_s開始納期年.Size = New System.Drawing.Size(14, 14)
        Me.lb_s開始納期年.TabIndex = 104
        Me.lb_s開始納期年.Text = "年"
        Me.lb_s開始納期年.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_s開始納期月
        '
        Me.lb_s開始納期月.BackColor = System.Drawing.SystemColors.Window
        Me.lb_s開始納期月.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_s開始納期月.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_s開始納期月.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_s開始納期月.Location = New System.Drawing.Point(610, 109)
        Me.lb_s開始納期月.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lb_s開始納期月.Name = "lb_s開始納期月"
        Me.lb_s開始納期月.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_s開始納期月.Size = New System.Drawing.Size(14, 14)
        Me.lb_s開始納期月.TabIndex = 103
        Me.lb_s開始納期月.Text = "月"
        Me.lb_s開始納期月.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_s開始納期日
        '
        Me.lb_s開始納期日.BackColor = System.Drawing.SystemColors.Window
        Me.lb_s開始納期日.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_s開始納期日.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_s開始納期日.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_s開始納期日.Location = New System.Drawing.Point(646, 109)
        Me.lb_s開始納期日.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lb_s開始納期日.Name = "lb_s開始納期日"
        Me.lb_s開始納期日.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_s開始納期日.Size = New System.Drawing.Size(14, 14)
        Me.lb_s開始納期日.TabIndex = 102
        Me.lb_s開始納期日.Text = "日"
        Me.lb_s開始納期日.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_e開始納期年
        '
        Me.lb_e開始納期年.BackColor = System.Drawing.SystemColors.Window
        Me.lb_e開始納期年.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_e開始納期年.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_e開始納期年.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_e開始納期年.Location = New System.Drawing.Point(732, 109)
        Me.lb_e開始納期年.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lb_e開始納期年.Name = "lb_e開始納期年"
        Me.lb_e開始納期年.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_e開始納期年.Size = New System.Drawing.Size(14, 14)
        Me.lb_e開始納期年.TabIndex = 101
        Me.lb_e開始納期年.Text = "年"
        Me.lb_e開始納期年.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_e開始納期月
        '
        Me.lb_e開始納期月.BackColor = System.Drawing.SystemColors.Window
        Me.lb_e開始納期月.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_e開始納期月.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_e開始納期月.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_e開始納期月.Location = New System.Drawing.Point(772, 109)
        Me.lb_e開始納期月.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lb_e開始納期月.Name = "lb_e開始納期月"
        Me.lb_e開始納期月.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_e開始納期月.Size = New System.Drawing.Size(14, 14)
        Me.lb_e開始納期月.TabIndex = 100
        Me.lb_e開始納期月.Text = "月"
        Me.lb_e開始納期月.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_e開始納期日
        '
        Me.lb_e開始納期日.BackColor = System.Drawing.SystemColors.Window
        Me.lb_e開始納期日.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_e開始納期日.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_e開始納期日.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_e開始納期日.Location = New System.Drawing.Point(809, 109)
        Me.lb_e開始納期日.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lb_e開始納期日.Name = "lb_e開始納期日"
        Me.lb_e開始納期日.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_e開始納期日.Size = New System.Drawing.Size(14, 14)
        Me.lb_e開始納期日.TabIndex = 99
        Me.lb_e開始納期日.Text = "日"
        Me.lb_e開始納期日.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_9
        '
        Me._lb_項目_9.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_9.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_9.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_9.ForeColor = System.Drawing.Color.White
        Me._lb_項目_9.Location = New System.Drawing.Point(441, 106)
        Me._lb_項目_9.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_項目_9.Name = "_lb_項目_9"
        Me._lb_項目_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_9.Size = New System.Drawing.Size(91, 19)
        Me._lb_項目_9.TabIndex = 98
        Me._lb_項目_9.Text = "開始納期"
        Me._lb_項目_9.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.SystemColors.Control
        Me.Label5.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label5.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label5.Location = New System.Drawing.Point(128, 130)
        Me.Label5.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label5.Size = New System.Drawing.Size(220, 14)
        Me.Label5.TabIndex = 97
        Me.Label5.Text = "0:新店 1:改装 2:メンテ 6:委託"
        '
        '_lb_項目_8
        '
        Me._lb_項目_8.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_8.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_8.ForeColor = System.Drawing.Color.White
        Me._lb_項目_8.Location = New System.Drawing.Point(9, 129)
        Me._lb_項目_8.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_項目_8.Name = "_lb_項目_8"
        Me._lb_項目_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_8.Size = New System.Drawing.Size(91, 19)
        Me._lb_項目_8.TabIndex = 96
        Me._lb_項目_8.Text = "物件種別"
        Me._lb_項目_8.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_見積番号_kara
        '
        Me._lb_見積番号_kara.BackColor = System.Drawing.SystemColors.Control
        Me._lb_見積番号_kara.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_見積番号_kara.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_見積番号_kara.Location = New System.Drawing.Point(196, 151)
        Me._lb_見積番号_kara.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_見積番号_kara.Name = "_lb_見積番号_kara"
        Me._lb_見積番号_kara.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_見積番号_kara.Size = New System.Drawing.Size(13, 10)
        Me._lb_見積番号_kara.TabIndex = 95
        Me._lb_見積番号_kara.Text = "～"
        Me._lb_見積番号_kara.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_2
        '
        Me._lb_項目_2.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_2.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_2.ForeColor = System.Drawing.Color.White
        Me._lb_項目_2.Location = New System.Drawing.Point(9, 88)
        Me._lb_項目_2.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_項目_2.Name = "_lb_項目_2"
        Me._lb_項目_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_2.Size = New System.Drawing.Size(91, 19)
        Me._lb_項目_2.TabIndex = 94
        Me._lb_項目_2.Text = "担当者CD"
        Me._lb_項目_2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.SystemColors.Control
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(132, 31)
        Me.Label4.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(252, 14)
        Me.Label4.TabIndex = 93
        Me.Label4.Text = "（0:未処理  1:発注済  2:売上済　3:全て）"
        '
        '_lb_項目_1
        '
        Me._lb_項目_1.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_1.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_1.ForeColor = System.Drawing.Color.White
        Me._lb_項目_1.Location = New System.Drawing.Point(9, 26)
        Me._lb_項目_1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_項目_1.Name = "_lb_項目_1"
        Me._lb_項目_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_1.Size = New System.Drawing.Size(91, 19)
        Me._lb_項目_1.TabIndex = 92
        Me._lb_項目_1.Text = "処理種別"
        Me._lb_項目_1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_見積金額_kara
        '
        Me._lb_見積金額_kara.BackColor = System.Drawing.SystemColors.Control
        Me._lb_見積金額_kara.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_見積金額_kara.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_見積金額_kara.Location = New System.Drawing.Point(196, 192)
        Me._lb_見積金額_kara.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_見積金額_kara.Name = "_lb_見積金額_kara"
        Me._lb_見積金額_kara.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_見積金額_kara.Size = New System.Drawing.Size(13, 10)
        Me._lb_見積金額_kara.TabIndex = 91
        Me._lb_見積金額_kara.Text = "～"
        Me._lb_見積金額_kara.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_0
        '
        Me._lb_項目_0.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_0.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_0.ForeColor = System.Drawing.Color.White
        Me._lb_項目_0.Location = New System.Drawing.Point(9, 190)
        Me._lb_項目_0.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_項目_0.Name = "_lb_項目_0"
        Me._lb_項目_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_0.Size = New System.Drawing.Size(91, 19)
        Me._lb_項目_0.TabIndex = 90
        Me._lb_項目_0.Text = "見積金額"
        Me._lb_項目_0.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_7
        '
        Me._lb_項目_7.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_7.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_7.ForeColor = System.Drawing.Color.White
        Me._lb_項目_7.Location = New System.Drawing.Point(441, 86)
        Me._lb_項目_7.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_項目_7.Name = "_lb_項目_7"
        Me._lb_項目_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_7.Size = New System.Drawing.Size(91, 19)
        Me._lb_項目_7.TabIndex = 88
        Me._lb_項目_7.Text = "見積日"
        Me._lb_項目_7.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_6
        '
        Me._lb_項目_6.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_6.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_6.ForeColor = System.Drawing.Color.White
        Me._lb_項目_6.Location = New System.Drawing.Point(9, 46)
        Me._lb_項目_6.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_項目_6.Name = "_lb_項目_6"
        Me._lb_項目_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_6.Size = New System.Drawing.Size(91, 19)
        Me._lb_項目_6.TabIndex = 87
        Me._lb_項目_6.Text = "得意先CD"
        Me._lb_項目_6.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_3
        '
        Me._lb_項目_3.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_3.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_3.ForeColor = System.Drawing.Color.White
        Me._lb_項目_3.Location = New System.Drawing.Point(9, 149)
        Me._lb_項目_3.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_項目_3.Name = "_lb_項目_3"
        Me._lb_項目_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_3.Size = New System.Drawing.Size(91, 19)
        Me._lb_項目_3.TabIndex = 86
        Me._lb_項目_3.Text = "見積№"
        Me._lb_項目_3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_4
        '
        Me._lb_項目_4.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_4.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_4.ForeColor = System.Drawing.Color.White
        Me._lb_項目_4.Location = New System.Drawing.Point(9, 67)
        Me._lb_項目_4.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_項目_4.Name = "_lb_項目_4"
        Me._lb_項目_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_4.Size = New System.Drawing.Size(91, 19)
        Me._lb_項目_4.TabIndex = 85
        Me._lb_項目_4.Text = "納入先CD"
        Me._lb_項目_4.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_5
        '
        Me._lb_項目_5.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_5.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_5.ForeColor = System.Drawing.Color.White
        Me._lb_項目_5.Location = New System.Drawing.Point(9, 170)
        Me._lb_項目_5.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_項目_5.Name = "_lb_項目_5"
        Me._lb_項目_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_5.Size = New System.Drawing.Size(91, 19)
        Me._lb_項目_5.TabIndex = 84
        Me._lb_項目_5.Text = "見積件名"
        Me._lb_項目_5.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_e見積日日
        '
        Me.lb_e見積日日.BackColor = System.Drawing.SystemColors.Window
        Me.lb_e見積日日.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_e見積日日.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_e見積日日.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_e見積日日.Location = New System.Drawing.Point(809, 88)
        Me.lb_e見積日日.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lb_e見積日日.Name = "lb_e見積日日"
        Me.lb_e見積日日.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_e見積日日.Size = New System.Drawing.Size(14, 14)
        Me.lb_e見積日日.TabIndex = 57
        Me.lb_e見積日日.Text = "日"
        Me.lb_e見積日日.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_e見積日月
        '
        Me.lb_e見積日月.BackColor = System.Drawing.SystemColors.Window
        Me.lb_e見積日月.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_e見積日月.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_e見積日月.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_e見積日月.Location = New System.Drawing.Point(772, 88)
        Me.lb_e見積日月.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lb_e見積日月.Name = "lb_e見積日月"
        Me.lb_e見積日月.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_e見積日月.Size = New System.Drawing.Size(14, 14)
        Me.lb_e見積日月.TabIndex = 56
        Me.lb_e見積日月.Text = "月"
        Me.lb_e見積日月.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_e見積日年
        '
        Me.lb_e見積日年.BackColor = System.Drawing.SystemColors.Window
        Me.lb_e見積日年.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_e見積日年.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_e見積日年.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_e見積日年.Location = New System.Drawing.Point(732, 88)
        Me.lb_e見積日年.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lb_e見積日年.Name = "lb_e見積日年"
        Me.lb_e見積日年.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_e見積日年.Size = New System.Drawing.Size(14, 14)
        Me.lb_e見積日年.TabIndex = 55
        Me.lb_e見積日年.Text = "年"
        Me.lb_e見積日年.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_s見積日日
        '
        Me.lb_s見積日日.BackColor = System.Drawing.SystemColors.Window
        Me.lb_s見積日日.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_s見積日日.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_s見積日日.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_s見積日日.Location = New System.Drawing.Point(646, 88)
        Me.lb_s見積日日.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lb_s見積日日.Name = "lb_s見積日日"
        Me.lb_s見積日日.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_s見積日日.Size = New System.Drawing.Size(14, 14)
        Me.lb_s見積日日.TabIndex = 54
        Me.lb_s見積日日.Text = "日"
        Me.lb_s見積日日.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_s見積日月
        '
        Me.lb_s見積日月.BackColor = System.Drawing.SystemColors.Window
        Me.lb_s見積日月.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_s見積日月.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_s見積日月.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_s見積日月.Location = New System.Drawing.Point(610, 88)
        Me.lb_s見積日月.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lb_s見積日月.Name = "lb_s見積日月"
        Me.lb_s見積日月.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_s見積日月.Size = New System.Drawing.Size(14, 14)
        Me.lb_s見積日月.TabIndex = 53
        Me.lb_s見積日月.Text = "月"
        Me.lb_s見積日月.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lb_s見積日年
        '
        Me.lb_s見積日年.BackColor = System.Drawing.SystemColors.Window
        Me.lb_s見積日年.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_s見積日年.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_s見積日年.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_s見積日年.Location = New System.Drawing.Point(569, 88)
        Me.lb_s見積日年.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lb_s見積日年.Name = "lb_s見積日年"
        Me.lb_s見積日年.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_s見積日年.Size = New System.Drawing.Size(14, 14)
        Me.lb_s見積日年.TabIndex = 52
        Me.lb_s見積日年.Text = "年"
        Me.lb_s見積日年.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_s見積日
        '
        Me._lb_s見積日.BackColor = System.Drawing.SystemColors.Window
        Me._lb_s見積日.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_s見積日.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_s見積日.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_s見積日.Location = New System.Drawing.Point(533, 85)
        Me._lb_s見積日.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_s見積日.Name = "_lb_s見積日"
        Me._lb_s見積日.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_s見積日.Size = New System.Drawing.Size(127, 18)
        Me._lb_s見積日.TabIndex = 51
        '
        '_lb_見積日_kara
        '
        Me._lb_見積日_kara.BackColor = System.Drawing.SystemColors.Control
        Me._lb_見積日_kara.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_見積日_kara.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_見積日_kara.Location = New System.Drawing.Point(670, 89)
        Me._lb_見積日_kara.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_見積日_kara.Name = "_lb_見積日_kara"
        Me._lb_見積日_kara.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_見積日_kara.Size = New System.Drawing.Size(13, 10)
        Me._lb_見積日_kara.TabIndex = 50
        Me._lb_見積日_kara.Text = "～"
        Me._lb_見積日_kara.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'rf_ListCount
        '
        Me.rf_ListCount.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.rf_ListCount.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_ListCount.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_ListCount.ForeColor = System.Drawing.Color.Yellow
        Me.rf_ListCount.Location = New System.Drawing.Point(77, 4)
        Me.rf_ListCount.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.rf_ListCount.Name = "rf_ListCount"
        Me.rf_ListCount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_ListCount.Size = New System.Drawing.Size(67, 15)
        Me.rf_ListCount.TabIndex = 49
        Me.rf_ListCount.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lb_該当件数
        '
        Me.lb_該当件数.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.lb_該当件数.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_該当件数.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_該当件数.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_該当件数.Location = New System.Drawing.Point(8, 4)
        Me.lb_該当件数.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lb_該当件数.Name = "lb_該当件数"
        Me.lb_該当件数.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_該当件数.Size = New System.Drawing.Size(69, 15)
        Me.lb_該当件数.TabIndex = 48
        Me.lb_該当件数.Text = "該当件数"
        Me.lb_該当件数.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_e見積日
        '
        Me._lb_e見積日.BackColor = System.Drawing.SystemColors.Window
        Me._lb_e見積日.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_e見積日.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_e見積日.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_e見積日.Location = New System.Drawing.Point(698, 85)
        Me._lb_e見積日.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_e見積日.Name = "_lb_e見積日"
        Me._lb_e見積日.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_e見積日.Size = New System.Drawing.Size(127, 18)
        Me._lb_e見積日.TabIndex = 89
        '
        '_lb_e開始納期
        '
        Me._lb_e開始納期.BackColor = System.Drawing.SystemColors.Window
        Me._lb_e開始納期.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_e開始納期.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_e開始納期.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_e開始納期.Location = New System.Drawing.Point(698, 106)
        Me._lb_e開始納期.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_e開始納期.Name = "_lb_e開始納期"
        Me._lb_e開始納期.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_e開始納期.Size = New System.Drawing.Size(127, 18)
        Me._lb_e開始納期.TabIndex = 107
        '
        '_lb_s開始納期
        '
        Me._lb_s開始納期.BackColor = System.Drawing.SystemColors.Window
        Me._lb_s開始納期.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_s開始納期.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_s開始納期.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_s開始納期.Location = New System.Drawing.Point(533, 106)
        Me._lb_s開始納期.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_s開始納期.Name = "_lb_s開始納期"
        Me._lb_s開始納期.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_s開始納期.Size = New System.Drawing.Size(127, 18)
        Me._lb_s開始納期.TabIndex = 105
        '
        '_lb_e完了日
        '
        Me._lb_e完了日.BackColor = System.Drawing.SystemColors.Window
        Me._lb_e完了日.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_e完了日.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_e完了日.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_e完了日.Location = New System.Drawing.Point(698, 146)
        Me._lb_e完了日.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_e完了日.Name = "_lb_e完了日"
        Me._lb_e完了日.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_e完了日.Size = New System.Drawing.Size(127, 18)
        Me._lb_e完了日.TabIndex = 116
        '
        '_lb_s完了日
        '
        Me._lb_s完了日.BackColor = System.Drawing.SystemColors.Window
        Me._lb_s完了日.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_s完了日.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_s完了日.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_s完了日.Location = New System.Drawing.Point(533, 146)
        Me._lb_s完了日.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_s完了日.Name = "_lb_s完了日"
        Me._lb_s完了日.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_s完了日.Size = New System.Drawing.Size(127, 18)
        Me._lb_s完了日.TabIndex = 117
        '
        '_lb_s請求予定
        '
        Me._lb_s請求予定.BackColor = System.Drawing.SystemColors.Window
        Me._lb_s請求予定.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_s請求予定.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_s請求予定.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_s請求予定.Location = New System.Drawing.Point(533, 166)
        Me._lb_s請求予定.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_s請求予定.Name = "_lb_s請求予定"
        Me._lb_s請求予定.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_s請求予定.Size = New System.Drawing.Size(127, 18)
        Me._lb_s請求予定.TabIndex = 126
        '
        '_lb_e請求予定
        '
        Me._lb_e請求予定.BackColor = System.Drawing.SystemColors.Window
        Me._lb_e請求予定.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_e請求予定.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_e請求予定.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_e請求予定.Location = New System.Drawing.Point(698, 166)
        Me._lb_e請求予定.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_e請求予定.Name = "_lb_e請求予定"
        Me._lb_e請求予定.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_e請求予定.Size = New System.Drawing.Size(127, 18)
        Me._lb_e請求予定.TabIndex = 125
        '
        '_lb_s仕入日
        '
        Me._lb_s仕入日.BackColor = System.Drawing.SystemColors.Window
        Me._lb_s仕入日.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_s仕入日.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_s仕入日.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_s仕入日.Location = New System.Drawing.Point(533, 126)
        Me._lb_s仕入日.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_s仕入日.Name = "_lb_s仕入日"
        Me._lb_s仕入日.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_s仕入日.Size = New System.Drawing.Size(127, 18)
        Me._lb_s仕入日.TabIndex = 144
        '
        '_lb_e仕入日
        '
        Me._lb_e仕入日.BackColor = System.Drawing.SystemColors.Window
        Me._lb_e仕入日.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_e仕入日.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_e仕入日.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_e仕入日.Location = New System.Drawing.Point(698, 126)
        Me._lb_e仕入日.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_e仕入日.Name = "_lb_e仕入日"
        Me._lb_e仕入日.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_e仕入日.Size = New System.Drawing.Size(127, 18)
        Me._lb_e仕入日.TabIndex = 143
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
        Me.SelListVw.Location = New System.Drawing.Point(9, 225)
        Me.SelListVw.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.SelListVw.MultiSelect = False
        Me.SelListVw.Name = "SelListVw"
        Me.SelListVw.Size = New System.Drawing.Size(1029, 418)
        Me.SelListVw.SortStyle = SnwMT01.SortableListView.SortStyles.SortDefault
        Me.SelListVw.TabIndex = 47
        Me.SelListVw.UseCompatibleStateImageBehavior = False
        Me.SelListVw.View = System.Windows.Forms.View.Details
        '
        'SnwMT01F00S
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(1055, 600)
        Me.Controls.Add(Me.tx_s見積番号)
        Me.Controls.Add(Me.txDir)
        Me.Controls.Add(Me.Cb変更)
        Me.Controls.Add(Me.PicFunction)
        Me.Controls.Add(Me.SelListVw)
        Me.Controls.Add(Me.CmdFind)
        Me.Controls.Add(Me.tx_得意先CD)
        Me.Controls.Add(Me.tx_s見積日D)
        Me.Controls.Add(Me.tx_s見積日M)
        Me.Controls.Add(Me.tx_s見積日Y)
        Me.Controls.Add(Me.tx_e見積日D)
        Me.Controls.Add(Me.tx_e見積日M)
        Me.Controls.Add(Me.tx_e見積日Y)
        Me.Controls.Add(Me.tx_見積件名)
        Me.Controls.Add(Me.tx_e見積金額)
        Me.Controls.Add(Me.tx_売上種別)
        Me.Controls.Add(Me.tx_担当者CD)
        Me.Controls.Add(Me.tx_e見積番号)
        Me.Controls.Add(Me.tx_s開始納期D)
        Me.Controls.Add(Me.tx_s開始納期M)
        Me.Controls.Add(Me.tx_s開始納期Y)
        Me.Controls.Add(Me.tx_e開始納期D)
        Me.Controls.Add(Me.tx_e開始納期M)
        Me.Controls.Add(Me.tx_e開始納期Y)
        Me.Controls.Add(Me.tx_s完了日D)
        Me.Controls.Add(Me.tx_s完了日M)
        Me.Controls.Add(Me.tx_s完了日Y)
        Me.Controls.Add(Me.tx_e完了日D)
        Me.Controls.Add(Me.tx_e完了日M)
        Me.Controls.Add(Me.tx_e完了日Y)
        Me.Controls.Add(Me.tx_s請求予定M)
        Me.Controls.Add(Me.tx_s請求予定Y)
        Me.Controls.Add(Me.tx_e請求予定M)
        Me.Controls.Add(Me.tx_e請求予定Y)
        Me.Controls.Add(Me.tx_s請求予定D)
        Me.Controls.Add(Me.tx_e請求予定D)
        Me.Controls.Add(Me.tx_s仕入日D)
        Me.Controls.Add(Me.tx_s仕入日M)
        Me.Controls.Add(Me.tx_s仕入日Y)
        Me.Controls.Add(Me.tx_e仕入日D)
        Me.Controls.Add(Me.tx_e仕入日M)
        Me.Controls.Add(Me.tx_e仕入日Y)
        Me.Controls.Add(Me.tx_s物件番号)
        Me.Controls.Add(Me.tx_e物件番号)
        Me.Controls.Add(Me.tx_見積確定区分)
        Me.Controls.Add(Me.tx_業種区分)
        Me.Controls.Add(Me._lb_項目_18)
        Me.Controls.Add(Me.Label30)
        Me.Controls.Add(Me._lblLabels_0)
        Me.Controls.Add(Me._lb_項目_17)
        Me.Controls.Add(Me._lb_項目_16)
        Me.Controls.Add(Me._lb_物件番号_kara)
        Me.Controls.Add(Me._lb_仕入日_kara)
        Me.Controls.Add(Me.lb_s仕入日年)
        Me.Controls.Add(Me.lb_s仕入日月)
        Me.Controls.Add(Me.lb_s仕入日日)
        Me.Controls.Add(Me.lb_e仕入日年)
        Me.Controls.Add(Me.lb_e仕入日月)
        Me.Controls.Add(Me.lb_e仕入日日)
        Me.Controls.Add(Me._lb_項目_15)
        Me.Controls.Add(Me.lb_s請求予定日)
        Me.Controls.Add(Me.lb_e請求予定日)
        Me.Controls.Add(Me._lb_項目_14)
        Me.Controls.Add(Me._lb_項目_13)
        Me.Controls.Add(Me._lblLabels_33)
        Me.Controls.Add(Me._lb_項目_12)
        Me.Controls.Add(Me._lb請求予定_kara)
        Me.Controls.Add(Me.lb_s請求予定年)
        Me.Controls.Add(Me.lb_s請求予定月)
        Me.Controls.Add(Me.lb_e請求予定年)
        Me.Controls.Add(Me.lb_e請求予定月)
        Me.Controls.Add(Me._lb_項目_11)
        Me.Controls.Add(Me.rf_ウエルシア物件区分名)
        Me.Controls.Add(Me._lb_項目_10)
        Me.Controls.Add(Me.lb_e完了日日)
        Me.Controls.Add(Me.lb_e完了日月)
        Me.Controls.Add(Me.lb_e完了日年)
        Me.Controls.Add(Me.lb_s完了日日)
        Me.Controls.Add(Me.lb_s完了日月)
        Me.Controls.Add(Me.lb_s完了日年)
        Me.Controls.Add(Me._lb_完了日_kara)
        Me.Controls.Add(Me._lb_開始納期_kara)
        Me.Controls.Add(Me.lb_s開始納期年)
        Me.Controls.Add(Me.lb_s開始納期月)
        Me.Controls.Add(Me.lb_s開始納期日)
        Me.Controls.Add(Me.lb_e開始納期年)
        Me.Controls.Add(Me.lb_e開始納期月)
        Me.Controls.Add(Me.lb_e開始納期日)
        Me.Controls.Add(Me._lb_項目_9)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me._lb_項目_8)
        Me.Controls.Add(Me._lb_見積番号_kara)
        Me.Controls.Add(Me._lb_項目_2)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me._lb_項目_1)
        Me.Controls.Add(Me._lb_見積金額_kara)
        Me.Controls.Add(Me._lb_項目_0)
        Me.Controls.Add(Me._lb_項目_7)
        Me.Controls.Add(Me._lb_項目_6)
        Me.Controls.Add(Me._lb_項目_3)
        Me.Controls.Add(Me._lb_項目_4)
        Me.Controls.Add(Me._lb_項目_5)
        Me.Controls.Add(Me.lb_e見積日日)
        Me.Controls.Add(Me.lb_e見積日月)
        Me.Controls.Add(Me.lb_e見積日年)
        Me.Controls.Add(Me.lb_s見積日日)
        Me.Controls.Add(Me.lb_s見積日月)
        Me.Controls.Add(Me.lb_s見積日年)
        Me.Controls.Add(Me._lb_s見積日)
        Me.Controls.Add(Me._lb_見積日_kara)
        Me.Controls.Add(Me.rf_ListCount)
        Me.Controls.Add(Me.lb_該当件数)
        Me.Controls.Add(Me._lb_e見積日)
        Me.Controls.Add(Me._lb_e開始納期)
        Me.Controls.Add(Me._lb_s開始納期)
        Me.Controls.Add(Me._lb_e完了日)
        Me.Controls.Add(Me._lb_s完了日)
        Me.Controls.Add(Me._lb_s請求予定)
        Me.Controls.Add(Me._lb_e請求予定)
        Me.Controls.Add(Me._lb_s仕入日)
        Me.Controls.Add(Me._lb_e仕入日)
        Me.Controls.Add(Me.tx_物件種別)
        Me.Controls.Add(Me.tx_s見積金額)
        Me.Controls.Add(Me.tx_納入先CD)
        Me.Controls.Add(Me.tx_ウエルシア物件区分)
        Me.Controls.Add(Me.tx_ウエルシアリース区分)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(79, 67)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "SnwMT01F00S"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "見積選択"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents SelListVw As SortableListView
    Public ToolTip1 As System.Windows.Forms.ToolTip
    Public WithEvents txDir As System.Windows.Forms.TextBox
    Public WithEvents Cb変更 As System.Windows.Forms.Button
    Public WithEvents PicFunction As System.Windows.Forms.Panel
    Public WithEvents CmdFind As System.Windows.Forms.Button
    Public WithEvents tx_納入先CD As ExText.ExTextBox
    Public WithEvents tx_得意先CD As ExText.ExTextBox
    Public WithEvents tx_s見積日D As ExDateText.ExDateTextBoxD
    Public WithEvents tx_s見積日M As ExDateText.ExDateTextBoxM
    Public WithEvents tx_s見積日Y As ExDateText.ExDateTextBoxY
    Public WithEvents tx_e見積日D As ExDateText.ExDateTextBoxD
    Public WithEvents tx_e見積日M As ExDateText.ExDateTextBoxM
    Public WithEvents tx_e見積日Y As ExDateText.ExDateTextBoxY
    Public WithEvents tx_見積件名 As ExText.ExTextBox
    Public WithEvents tx_s見積番号 As ExText.ExTextBox
    Public WithEvents tx_e見積番号 As ExText.ExTextBox
    Public WithEvents tx_s見積金額 As ExNmText.ExNmTextBox
    Public WithEvents tx_e見積金額 As ExNmText.ExNmTextBox
    Public WithEvents tx_売上種別 As ExNmText.ExNmTextBox
    Public WithEvents tx_担当者CD As ExNmText.ExNmTextBox
    Public WithEvents tx_物件種別 As ExText.ExTextBox
    Public WithEvents tx_s開始納期D As ExDateText.ExDateTextBoxD
    Public WithEvents tx_s開始納期M As ExDateText.ExDateTextBoxM
    Public WithEvents tx_s開始納期Y As ExDateText.ExDateTextBoxY
    Public WithEvents tx_e開始納期D As ExDateText.ExDateTextBoxD
    Public WithEvents tx_e開始納期M As ExDateText.ExDateTextBoxM
    Public WithEvents tx_e開始納期Y As ExDateText.ExDateTextBoxY
    Public WithEvents tx_s完了日D As ExDateText.ExDateTextBoxD
    Public WithEvents tx_s完了日M As ExDateText.ExDateTextBoxM
    Public WithEvents tx_s完了日Y As ExDateText.ExDateTextBoxY
    Public WithEvents tx_e完了日D As ExDateText.ExDateTextBoxD
    Public WithEvents tx_e完了日M As ExDateText.ExDateTextBoxM
    Public WithEvents tx_e完了日Y As ExDateText.ExDateTextBoxY
    Public WithEvents tx_ウエルシア物件区分 As ExNmText.ExNmTextBox
    Public WithEvents tx_s請求予定D As ExDateText.ExDateTextBoxD
    Public WithEvents tx_s請求予定M As ExDateText.ExDateTextBoxM
    Public WithEvents tx_s請求予定Y As ExDateText.ExDateTextBoxY
    Public WithEvents tx_e請求予定D As ExDateText.ExDateTextBoxD
    Public WithEvents tx_e請求予定M As ExDateText.ExDateTextBoxM
    Public WithEvents tx_e請求予定Y As ExDateText.ExDateTextBoxY
    Public WithEvents tx_ウエルシアリース区分 As ExNmText.ExNmTextBox
    Public WithEvents tx_s仕入日D As ExDateText.ExDateTextBoxD
    Public WithEvents tx_s仕入日M As ExDateText.ExDateTextBoxM
    Public WithEvents tx_s仕入日Y As ExDateText.ExDateTextBoxY
    Public WithEvents tx_e仕入日D As ExDateText.ExDateTextBoxD
    Public WithEvents tx_e仕入日M As ExDateText.ExDateTextBoxM
    Public WithEvents tx_e仕入日Y As ExDateText.ExDateTextBoxY
    Public WithEvents tx_s物件番号 As ExText.ExTextBox
    Public WithEvents tx_e物件番号 As ExText.ExTextBox
    Public WithEvents tx_見積確定区分 As ExText.ExTextBox
    Public WithEvents tx_業種区分 As ExText.ExTextBox
    Public WithEvents _lb_項目_18 As System.Windows.Forms.Label
    Public WithEvents Label30 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_0 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_17 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_16 As System.Windows.Forms.Label
    Public WithEvents _lb_物件番号_kara As System.Windows.Forms.Label
    Public WithEvents _lb_仕入日_kara As System.Windows.Forms.Label
    Public WithEvents lb_s仕入日年 As System.Windows.Forms.Label
    Public WithEvents lb_s仕入日月 As System.Windows.Forms.Label
    Public WithEvents lb_s仕入日日 As System.Windows.Forms.Label
    Public WithEvents lb_e仕入日年 As System.Windows.Forms.Label
    Public WithEvents lb_e仕入日月 As System.Windows.Forms.Label
    Public WithEvents lb_e仕入日日 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_15 As System.Windows.Forms.Label
    Public WithEvents lb_s請求予定日 As System.Windows.Forms.Label
    Public WithEvents lb_e請求予定日 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_14 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_13 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_33 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_12 As System.Windows.Forms.Label
    Public WithEvents _lb請求予定_kara As System.Windows.Forms.Label
    Public WithEvents lb_s請求予定年 As System.Windows.Forms.Label
    Public WithEvents lb_s請求予定月 As System.Windows.Forms.Label
    Public WithEvents lb_e請求予定年 As System.Windows.Forms.Label
    Public WithEvents lb_e請求予定月 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_11 As System.Windows.Forms.Label
    Public WithEvents rf_ウエルシア物件区分名 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_10 As System.Windows.Forms.Label
    Public WithEvents lb_e完了日日 As System.Windows.Forms.Label
    Public WithEvents lb_e完了日月 As System.Windows.Forms.Label
    Public WithEvents lb_e完了日年 As System.Windows.Forms.Label
    Public WithEvents lb_s完了日日 As System.Windows.Forms.Label
    Public WithEvents lb_s完了日月 As System.Windows.Forms.Label
    Public WithEvents lb_s完了日年 As System.Windows.Forms.Label
    Public WithEvents _lb_完了日_kara As System.Windows.Forms.Label
    Public WithEvents _lb_開始納期_kara As System.Windows.Forms.Label
    Public WithEvents lb_s開始納期年 As System.Windows.Forms.Label
    Public WithEvents lb_s開始納期月 As System.Windows.Forms.Label
    Public WithEvents lb_s開始納期日 As System.Windows.Forms.Label
    Public WithEvents lb_e開始納期年 As System.Windows.Forms.Label
    Public WithEvents lb_e開始納期月 As System.Windows.Forms.Label
    Public WithEvents lb_e開始納期日 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_9 As System.Windows.Forms.Label
    Public WithEvents Label5 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_8 As System.Windows.Forms.Label
    Public WithEvents _lb_見積番号_kara As System.Windows.Forms.Label
    Public WithEvents _lb_項目_2 As System.Windows.Forms.Label
    Public WithEvents Label4 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_1 As System.Windows.Forms.Label
    Public WithEvents _lb_見積金額_kara As System.Windows.Forms.Label
    Public WithEvents _lb_項目_0 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_7 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_6 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_3 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_4 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_5 As System.Windows.Forms.Label
    Public WithEvents lb_e見積日日 As System.Windows.Forms.Label
    Public WithEvents lb_e見積日月 As System.Windows.Forms.Label
    Public WithEvents lb_e見積日年 As System.Windows.Forms.Label
    Public WithEvents lb_s見積日日 As System.Windows.Forms.Label
    Public WithEvents lb_s見積日月 As System.Windows.Forms.Label
    Public WithEvents lb_s見積日年 As System.Windows.Forms.Label
    Public WithEvents _lb_s見積日 As System.Windows.Forms.Label
    Public WithEvents _lb_見積日_kara As System.Windows.Forms.Label
    Public WithEvents rf_ListCount As System.Windows.Forms.Label
    Public WithEvents lb_該当件数 As System.Windows.Forms.Label
    Public WithEvents _lb_e見積日 As System.Windows.Forms.Label
    Public WithEvents _lb_e開始納期 As System.Windows.Forms.Label
    Public WithEvents _lb_s開始納期 As System.Windows.Forms.Label
    Public WithEvents _lb_e完了日 As System.Windows.Forms.Label
    Public WithEvents _lb_s完了日 As System.Windows.Forms.Label
    Public WithEvents _lb_s請求予定 As System.Windows.Forms.Label
    Public WithEvents _lb_e請求予定 As System.Windows.Forms.Label
    Public WithEvents _lb_s仕入日 As System.Windows.Forms.Label
    Public WithEvents _lb_e仕入日 As System.Windows.Forms.Label
End Class