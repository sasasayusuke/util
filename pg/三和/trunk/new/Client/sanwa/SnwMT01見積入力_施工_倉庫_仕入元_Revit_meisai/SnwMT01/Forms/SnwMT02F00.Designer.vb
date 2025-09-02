<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SnwMT02F00

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
    Public WithEvents Cbレビット取込 As System.Windows.Forms.Button
    Public WithEvents Cb員数取込 As System.Windows.Forms.Button
    Public WithEvents Ck_MoveMode As System.Windows.Forms.CheckBox
    Public WithEvents Cbしまむら消耗品取込 As System.Windows.Forms.Button
    Public WithEvents CbWel1パー算出 As System.Windows.Forms.Button
    Public WithEvents Cb端数値引計算 As System.Windows.Forms.Button
    Public WithEvents Ck_DragMode As System.Windows.Forms.CheckBox
    Public WithEvents Command1 As System.Windows.Forms.Button
    Public WithEvents Cb固定列 As System.Windows.Forms.Button
    Public WithEvents tx_大小口区分 As System.Windows.Forms.TextBox
    Public WithEvents CmdChkOn As System.Windows.Forms.Button
    Public WithEvents CmdChkOff As System.Windows.Forms.Button
    Public WithEvents tx_受注区分 As System.Windows.Forms.TextBox
    Public WithEvents Cb展開 As System.Windows.Forms.Button
    Public WithEvents CbTabEnd As System.Windows.Forms.Button
    Public WithEvents PicFunction As System.Windows.Forms.Panel
    Public WithEvents sb_Msg_Panel1 As System.Windows.Forms.ToolStripStatusLabel
    Public WithEvents sb_Msg_Panel2 As System.Windows.Forms.ToolStripStatusLabel
    Public WithEvents sb_Msg_Panel3 As System.Windows.Forms.ToolStripStatusLabel
    Public WithEvents sb_Msg As System.Windows.Forms.StatusStrip
    Public WithEvents tx_Dummy1 As System.Windows.Forms.TextBox
    Public WithEvents tx_外税額 As System.Windows.Forms.TextBox
    Public WithEvents tx_原価合計 As System.Windows.Forms.TextBox
    Public WithEvents tx_合計金額 As System.Windows.Forms.TextBox
    Public WithEvents tx_売上端数 As System.Windows.Forms.TextBox
    Public WithEvents tx_消費税端数 As System.Windows.Forms.TextBox
    Public WithEvents tx_見積日付 As System.Windows.Forms.TextBox
    Public WithEvents tx_端数値引桁数 As ExNmText.ExNmTextBox
    Public WithEvents tx_Welパー算出 As ExNmText.ExNmTextBox
    Public WithEvents rf_売価確定 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_10 As System.Windows.Forms.Label
    Public WithEvents rf_原価合計 As System.Windows.Forms.Label
    Public WithEvents rf_1パー金額 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_9 As System.Windows.Forms.Label
    Public WithEvents rf_税込金額 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_5 As System.Windows.Forms.Label
    Public WithEvents rf_端数値引金額 As System.Windows.Forms.Label
    Public WithEvents rf_得意先別見積番号 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_8 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_7 As System.Windows.Forms.Label
    Public WithEvents rf_原価率 As System.Windows.Forms.Label
    Public WithEvents rf_合計金額 As System.Windows.Forms.Label
    Public WithEvents rf_得意先CD As System.Windows.Forms.Label
    Public WithEvents _lb_項目_2 As System.Windows.Forms.Label
    Public WithEvents rf_得意先名 As System.Windows.Forms.Label
    Public WithEvents rf_処理区分 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_6 As System.Windows.Forms.Label
    Public WithEvents rf_見積名称 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_3 As System.Windows.Forms.Label
    Public WithEvents rf_見積番号 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_1 As System.Windows.Forms.Label

    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使って変更できます。
    'コード エディタを使用して、変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SnwMT02F00))
        Me.SubFunction = New System.Windows.Forms.Panel()
        Me.tx_見積確定区分 = New ExNmText.ExNmTextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me._lblLabels_43 = New System.Windows.Forms.Label()
        Me.Cbレビット取込 = New System.Windows.Forms.Button()
        Me.Cb員数取込 = New System.Windows.Forms.Button()
        Me.Ck_MoveMode = New System.Windows.Forms.CheckBox()
        Me.Cbしまむら消耗品取込 = New System.Windows.Forms.Button()
        Me.CbWel1パー算出 = New System.Windows.Forms.Button()
        Me.Cb端数値引計算 = New System.Windows.Forms.Button()
        Me.Ck_DragMode = New System.Windows.Forms.CheckBox()
        Me.Command1 = New System.Windows.Forms.Button()
        Me.Cb固定列 = New System.Windows.Forms.Button()
        Me.tx_大小口区分 = New System.Windows.Forms.TextBox()
        Me.CmdChkOn = New System.Windows.Forms.Button()
        Me.CmdChkOff = New System.Windows.Forms.Button()
        Me.tx_受注区分 = New System.Windows.Forms.TextBox()
        Me.Cb展開 = New System.Windows.Forms.Button()
        Me.CbTabEnd = New System.Windows.Forms.Button()
        Me.tx_Dummy1 = New System.Windows.Forms.TextBox()
        Me.tx_外税額 = New System.Windows.Forms.TextBox()
        Me.tx_原価合計 = New System.Windows.Forms.TextBox()
        Me.tx_合計金額 = New System.Windows.Forms.TextBox()
        Me.tx_売上端数 = New System.Windows.Forms.TextBox()
        Me.tx_消費税端数 = New System.Windows.Forms.TextBox()
        Me.tx_見積日付 = New System.Windows.Forms.TextBox()
        Me.tx_端数値引桁数 = New ExNmText.ExNmTextBox()
        Me.tx_Welパー算出 = New ExNmText.ExNmTextBox()
        Me.rf_売価確定 = New System.Windows.Forms.Label()
        Me._lb_項目_10 = New System.Windows.Forms.Label()
        Me.rf_原価合計 = New System.Windows.Forms.Label()
        Me.rf_1パー金額 = New System.Windows.Forms.Label()
        Me._lb_項目_9 = New System.Windows.Forms.Label()
        Me.rf_税込金額 = New System.Windows.Forms.Label()
        Me._lb_項目_5 = New System.Windows.Forms.Label()
        Me.rf_端数値引金額 = New System.Windows.Forms.Label()
        Me.rf_得意先別見積番号 = New System.Windows.Forms.Label()
        Me._lb_項目_8 = New System.Windows.Forms.Label()
        Me._lb_項目_7 = New System.Windows.Forms.Label()
        Me.rf_原価率 = New System.Windows.Forms.Label()
        Me.rf_合計金額 = New System.Windows.Forms.Label()
        Me.rf_得意先CD = New System.Windows.Forms.Label()
        Me._lb_項目_2 = New System.Windows.Forms.Label()
        Me.rf_得意先名 = New System.Windows.Forms.Label()
        Me.rf_処理区分 = New System.Windows.Forms.Label()
        Me._lb_項目_6 = New System.Windows.Forms.Label()
        Me.rf_見積名称 = New System.Windows.Forms.Label()
        Me._lb_項目_3 = New System.Windows.Forms.Label()
        Me.rf_見積番号 = New System.Windows.Forms.Label()
        Me._lb_項目_1 = New System.Windows.Forms.Label()
        Me.sb_Msg = New System.Windows.Forms.StatusStrip()
        Me.sb_Msg_Panel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.sb_Msg_Panel2 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.sb_Msg_Panel3 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.PicFunction = New System.Windows.Forms.Panel()
        Me.fr_Disp = New System.Windows.Forms.Panel()
        Me._lb_項目_4 = New System.Windows.Forms.Label()
        Me.tx_製品NO = New ExText.ExTextBox()
        Me.Cb検索 = New System.Windows.Forms.Button()
        Me._lb_項目_0 = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.FpSpd = New FarPoint.Win.Spread.FpSpread(FarPoint.Win.Spread.LegacyBehaviors.Protect, resources.GetObject("resource1"))
        Me.FpSpd_Sheet1 = Me.FpSpd.GetSheet(0)
        Me.sb_Msg.SuspendLayout()
        Me.fr_Disp.SuspendLayout()
        CType(Me.FpSpd, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SubFunction
        '
        Me.SubFunction.Location = New System.Drawing.Point(94, 2)
        Me.SubFunction.Name = "SubFunction"
        Me.SubFunction.Size = New System.Drawing.Size(572, 27)
        Me.SubFunction.TabIndex = 40
        '
        'tx_見積確定区分
        '
        Me.tx_見積確定区分.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_見積確定区分.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_見積確定区分.CanForwardSetFocus = True
        Me.tx_見積確定区分.CanNextSetFocus = True
        Me.tx_見積確定区分.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_見積確定区分.DecimalPlace = CType(0, Short)
        Me.tx_見積確定区分.EditMode = True
        Me.tx_見積確定区分.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_見積確定区分.FormatType = ""
        Me.tx_見積確定区分.FormatTypeNega = ""
        Me.tx_見積確定区分.FormatTypeNull = ""
        Me.tx_見積確定区分.FormatTypeZero = ""
        Me.tx_見積確定区分.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.tx_見積確定区分.InputMinus = True
        Me.tx_見積確定区分.InputPlus = True
        Me.tx_見積確定区分.InputZero = True
        Me.tx_見積確定区分.Location = New System.Drawing.Point(1326, 2)
        Me.tx_見積確定区分.MaxLength = 1
        Me.tx_見積確定区分.Name = "tx_見積確定区分"
        Me.tx_見積確定区分.OldValue = "1"
        Me.tx_見積確定区分.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_見積確定区分.SelectText = True
        Me.tx_見積確定区分.SelLength = 0
        Me.tx_見積確定区分.SelStart = 0
        Me.tx_見積確定区分.SelText = ""
        Me.tx_見積確定区分.Size = New System.Drawing.Size(26, 22)
        Me.tx_見積確定区分.TabIndex = 89
        Me.tx_見積確定区分.Text = "1"
        '
        'Label12
        '
        Me.Label12.BackColor = System.Drawing.SystemColors.Control
        Me.Label12.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label12.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label12.Location = New System.Drawing.Point(1252, 7)
        Me.Label12.Name = "Label12"
        Me.Label12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label12.Size = New System.Drawing.Size(85, 19)
        Me.Label12.TabIndex = 232
        Me.Label12.Text = "見積確定"
        '
        '_lblLabels_43
        '
        Me._lblLabels_43.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_43.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_43.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_43.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_43.Location = New System.Drawing.Point(1356, 7)
        Me._lblLabels_43.Name = "_lblLabels_43"
        Me._lblLabels_43.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_43.Size = New System.Drawing.Size(165, 19)
        Me._lblLabels_43.TabIndex = 231
        Me._lblLabels_43.Text = "0:未確定 1:確定"
        '
        'Cbレビット取込
        '
        Me.Cbレビット取込.BackColor = System.Drawing.SystemColors.Control
        Me.Cbレビット取込.Cursor = System.Windows.Forms.Cursors.Default
        Me.Cbレビット取込.Enabled = False
        Me.Cbレビット取込.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Cbレビット取込.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Cbレビット取込.Location = New System.Drawing.Point(938, 5)
        Me.Cbレビット取込.Name = "Cbレビット取込"
        Me.Cbレビット取込.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Cbレビット取込.Size = New System.Drawing.Size(76, 22)
        Me.Cbレビット取込.TabIndex = 85
        Me.Cbレビット取込.Text = "ﾚﾋﾞｯﾄ取"
        Me.Cbレビット取込.UseVisualStyleBackColor = False
        Me.Cbレビット取込.Visible = False
        '
        'Cb員数取込
        '
        Me.Cb員数取込.BackColor = System.Drawing.SystemColors.Control
        Me.Cb員数取込.Cursor = System.Windows.Forms.Cursors.Default
        Me.Cb員数取込.Font = New System.Drawing.Font("ＭＳ ゴシック", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Cb員数取込.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Cb員数取込.Location = New System.Drawing.Point(808, 4)
        Me.Cb員数取込.Name = "Cb員数取込"
        Me.Cb員数取込.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Cb員数取込.Size = New System.Drawing.Size(83, 24)
        Me.Cb員数取込.TabIndex = 84
        Me.Cb員数取込.Text = "員数取込"
        Me.Cb員数取込.UseVisualStyleBackColor = False
        '
        'Ck_MoveMode
        '
        Me.Ck_MoveMode.Appearance = System.Windows.Forms.Appearance.Button
        Me.Ck_MoveMode.BackColor = System.Drawing.SystemColors.Control
        Me.Ck_MoveMode.Cursor = System.Windows.Forms.Cursors.Default
        Me.Ck_MoveMode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Ck_MoveMode.Location = New System.Drawing.Point(132, 55)
        Me.Ck_MoveMode.Name = "Ck_MoveMode"
        Me.Ck_MoveMode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Ck_MoveMode.Size = New System.Drawing.Size(32, 26)
        Me.Ck_MoveMode.TabIndex = 83
        Me.Ck_MoveMode.TabStop = False
        Me.Ck_MoveMode.Text = "→"
        Me.Ck_MoveMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.Ck_MoveMode.UseVisualStyleBackColor = False
        '
        'Cbしまむら消耗品取込
        '
        Me.Cbしまむら消耗品取込.BackColor = System.Drawing.SystemColors.Control
        Me.Cbしまむら消耗品取込.Cursor = System.Windows.Forms.Cursors.Default
        Me.Cbしまむら消耗品取込.Enabled = False
        Me.Cbしまむら消耗品取込.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Cbしまむら消耗品取込.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Cbしまむら消耗品取込.Location = New System.Drawing.Point(930, 4)
        Me.Cbしまむら消耗品取込.Name = "Cbしまむら消耗品取込"
        Me.Cbしまむら消耗品取込.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Cbしまむら消耗品取込.Size = New System.Drawing.Size(76, 22)
        Me.Cbしまむら消耗品取込.TabIndex = 82
        Me.Cbしまむら消耗品取込.Text = "SM取込"
        Me.Cbしまむら消耗品取込.UseVisualStyleBackColor = False
        Me.Cbしまむら消耗品取込.Visible = False
        '
        'CbWel1パー算出
        '
        Me.CbWel1パー算出.BackColor = System.Drawing.SystemColors.Control
        Me.CbWel1パー算出.Cursor = System.Windows.Forms.Cursors.Default
        Me.CbWel1パー算出.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CbWel1パー算出.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CbWel1パー算出.Location = New System.Drawing.Point(1048, 2)
        Me.CbWel1パー算出.Name = "CbWel1パー算出"
        Me.CbWel1パー算出.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CbWel1パー算出.Size = New System.Drawing.Size(39, 24)
        Me.CbWel1パー算出.TabIndex = 80
        Me.CbWel1パー算出.Text = "%"
        Me.CbWel1パー算出.UseVisualStyleBackColor = False
        '
        'Cb端数値引計算
        '
        Me.Cb端数値引計算.BackColor = System.Drawing.SystemColors.Control
        Me.Cb端数値引計算.Cursor = System.Windows.Forms.Cursors.Default
        Me.Cb端数値引計算.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Cb端数値引計算.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Cb端数値引計算.Location = New System.Drawing.Point(1098, 54)
        Me.Cb端数値引計算.Name = "Cb端数値引計算"
        Me.Cb端数値引計算.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Cb端数値引計算.Size = New System.Drawing.Size(55, 24)
        Me.Cb端数値引計算.TabIndex = 76
        Me.Cb端数値引計算.Text = "計算"
        Me.Cb端数値引計算.UseVisualStyleBackColor = False
        '
        'Ck_DragMode
        '
        Me.Ck_DragMode.Appearance = System.Windows.Forms.Appearance.Button
        Me.Ck_DragMode.BackColor = System.Drawing.SystemColors.Control
        Me.Ck_DragMode.Cursor = System.Windows.Forms.Cursors.Default
        Me.Ck_DragMode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Ck_DragMode.Location = New System.Drawing.Point(68, 55)
        Me.Ck_DragMode.Name = "Ck_DragMode"
        Me.Ck_DragMode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Ck_DragMode.Size = New System.Drawing.Size(60, 26)
        Me.Ck_DragMode.TabIndex = 72
        Me.Ck_DragMode.TabStop = False
        Me.Ck_DragMode.Text = "ﾄﾞﾗｯｸﾞ"
        Me.Ck_DragMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.Ck_DragMode.UseVisualStyleBackColor = False
        '
        'Command1
        '
        Me.Command1.BackColor = System.Drawing.SystemColors.Control
        Me.Command1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Command1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Command1.Location = New System.Drawing.Point(232, 56)
        Me.Command1.Name = "Command1"
        Me.Command1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Command1.Size = New System.Drawing.Size(29, 21)
        Me.Command1.TabIndex = 68
        Me.Command1.Text = "Command1"
        Me.Command1.UseVisualStyleBackColor = False
        Me.Command1.Visible = False
        '
        'Cb固定列
        '
        Me.Cb固定列.BackColor = System.Drawing.SystemColors.Control
        Me.Cb固定列.Cursor = System.Windows.Forms.Cursors.Default
        Me.Cb固定列.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Cb固定列.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Cb固定列.Location = New System.Drawing.Point(4, 55)
        Me.Cb固定列.Name = "Cb固定列"
        Me.Cb固定列.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Cb固定列.Size = New System.Drawing.Size(60, 26)
        Me.Cb固定列.TabIndex = 67
        Me.Cb固定列.TabStop = False
        Me.Cb固定列.Text = "固定列"
        Me.Cb固定列.UseVisualStyleBackColor = False
        '
        'tx_大小口区分
        '
        Me.tx_大小口区分.AcceptsReturn = True
        Me.tx_大小口区分.BackColor = System.Drawing.SystemColors.Window
        Me.tx_大小口区分.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_大小口区分.Enabled = False
        Me.tx_大小口区分.ForeColor = System.Drawing.SystemColors.WindowText
        Me.tx_大小口区分.Location = New System.Drawing.Point(221, 56)
        Me.tx_大小口区分.MaxLength = 0
        Me.tx_大小口区分.Name = "tx_大小口区分"
        Me.tx_大小口区分.ReadOnly = True
        Me.tx_大小口区分.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_大小口区分.Size = New System.Drawing.Size(67, 22)
        Me.tx_大小口区分.TabIndex = 63
        Me.tx_大小口区分.Text = "大小口区分"
        Me.tx_大小口区分.Visible = False
        '
        'CmdChkOn
        '
        Me.CmdChkOn.BackColor = System.Drawing.SystemColors.Control
        Me.CmdChkOn.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmdChkOn.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CmdChkOn.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdChkOn.Location = New System.Drawing.Point(1052, 204)
        Me.CmdChkOn.Name = "CmdChkOn"
        Me.CmdChkOn.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdChkOn.Size = New System.Drawing.Size(91, 22)
        Me.CmdChkOn.TabIndex = 62
        Me.CmdChkOn.Text = "展開ALL選択"
        Me.CmdChkOn.UseVisualStyleBackColor = False
        Me.CmdChkOn.Visible = False
        '
        'CmdChkOff
        '
        Me.CmdChkOff.BackColor = System.Drawing.SystemColors.Control
        Me.CmdChkOff.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmdChkOff.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CmdChkOff.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdChkOff.Location = New System.Drawing.Point(1032, 204)
        Me.CmdChkOff.Name = "CmdChkOff"
        Me.CmdChkOff.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdChkOff.Size = New System.Drawing.Size(91, 22)
        Me.CmdChkOff.TabIndex = 61
        Me.CmdChkOff.Text = "展開ALL解除"
        Me.CmdChkOff.UseVisualStyleBackColor = False
        Me.CmdChkOff.Visible = False
        '
        'tx_受注区分
        '
        Me.tx_受注区分.AcceptsReturn = True
        Me.tx_受注区分.BackColor = System.Drawing.SystemColors.Window
        Me.tx_受注区分.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_受注区分.Enabled = False
        Me.tx_受注区分.ForeColor = System.Drawing.SystemColors.WindowText
        Me.tx_受注区分.Location = New System.Drawing.Point(86, 56)
        Me.tx_受注区分.MaxLength = 0
        Me.tx_受注区分.Name = "tx_受注区分"
        Me.tx_受注区分.ReadOnly = True
        Me.tx_受注区分.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_受注区分.Size = New System.Drawing.Size(58, 22)
        Me.tx_受注区分.TabIndex = 60
        Me.tx_受注区分.Text = "受注区分"
        Me.tx_受注区分.Visible = False
        '
        'Cb展開
        '
        Me.Cb展開.BackColor = System.Drawing.SystemColors.Control
        Me.Cb展開.Cursor = System.Windows.Forms.Cursors.Default
        Me.Cb展開.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Cb展開.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Cb展開.Location = New System.Drawing.Point(1012, 204)
        Me.Cb展開.Name = "Cb展開"
        Me.Cb展開.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Cb展開.Size = New System.Drawing.Size(60, 22)
        Me.Cb展開.TabIndex = 40
        Me.Cb展開.TabStop = False
        Me.Cb展開.Text = "展開"
        Me.Cb展開.UseVisualStyleBackColor = False
        Me.Cb展開.Visible = False
        '
        'CbTabEnd
        '
        Me.CbTabEnd.BackColor = System.Drawing.SystemColors.Control
        Me.CbTabEnd.Cursor = System.Windows.Forms.Cursors.Default
        Me.CbTabEnd.Enabled = False
        Me.CbTabEnd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CbTabEnd.Location = New System.Drawing.Point(1000, 19)
        Me.CbTabEnd.Name = "CbTabEnd"
        Me.CbTabEnd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CbTabEnd.Size = New System.Drawing.Size(41, 12)
        Me.CbTabEnd.TabIndex = 1
        Me.CbTabEnd.UseVisualStyleBackColor = False
        Me.CbTabEnd.Visible = False
        '
        'tx_Dummy1
        '
        Me.tx_Dummy1.AcceptsReturn = True
        Me.tx_Dummy1.BackColor = System.Drawing.SystemColors.Window
        Me.tx_Dummy1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_Dummy1.Enabled = False
        Me.tx_Dummy1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.tx_Dummy1.Location = New System.Drawing.Point(1004, 164)
        Me.tx_Dummy1.MaxLength = 0
        Me.tx_Dummy1.Name = "tx_Dummy1"
        Me.tx_Dummy1.ReadOnly = True
        Me.tx_Dummy1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_Dummy1.Size = New System.Drawing.Size(42, 22)
        Me.tx_Dummy1.TabIndex = 53
        Me.tx_Dummy1.Text = "Dummy1"
        Me.tx_Dummy1.Visible = False
        '
        'tx_外税額
        '
        Me.tx_外税額.AcceptsReturn = True
        Me.tx_外税額.BackColor = System.Drawing.SystemColors.Window
        Me.tx_外税額.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_外税額.Enabled = False
        Me.tx_外税額.ForeColor = System.Drawing.SystemColors.WindowText
        Me.tx_外税額.Location = New System.Drawing.Point(1004, 144)
        Me.tx_外税額.MaxLength = 0
        Me.tx_外税額.Name = "tx_外税額"
        Me.tx_外税額.ReadOnly = True
        Me.tx_外税額.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_外税額.Size = New System.Drawing.Size(67, 22)
        Me.tx_外税額.TabIndex = 54
        Me.tx_外税額.Text = "外税額"
        '
        'tx_原価合計
        '
        Me.tx_原価合計.AcceptsReturn = True
        Me.tx_原価合計.BackColor = System.Drawing.SystemColors.Window
        Me.tx_原価合計.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_原価合計.Enabled = False
        Me.tx_原価合計.ForeColor = System.Drawing.SystemColors.WindowText
        Me.tx_原価合計.Location = New System.Drawing.Point(1004, 120)
        Me.tx_原価合計.MaxLength = 0
        Me.tx_原価合計.Name = "tx_原価合計"
        Me.tx_原価合計.ReadOnly = True
        Me.tx_原価合計.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_原価合計.Size = New System.Drawing.Size(67, 22)
        Me.tx_原価合計.TabIndex = 55
        Me.tx_原価合計.Text = "原価合計"
        Me.tx_原価合計.Visible = False
        '
        'tx_合計金額
        '
        Me.tx_合計金額.AcceptsReturn = True
        Me.tx_合計金額.BackColor = System.Drawing.SystemColors.Window
        Me.tx_合計金額.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_合計金額.Enabled = False
        Me.tx_合計金額.ForeColor = System.Drawing.SystemColors.WindowText
        Me.tx_合計金額.Location = New System.Drawing.Point(1004, 96)
        Me.tx_合計金額.MaxLength = 0
        Me.tx_合計金額.Name = "tx_合計金額"
        Me.tx_合計金額.ReadOnly = True
        Me.tx_合計金額.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_合計金額.Size = New System.Drawing.Size(67, 22)
        Me.tx_合計金額.TabIndex = 56
        Me.tx_合計金額.Text = "合計金額"
        '
        'tx_売上端数
        '
        Me.tx_売上端数.AcceptsReturn = True
        Me.tx_売上端数.BackColor = System.Drawing.SystemColors.Window
        Me.tx_売上端数.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_売上端数.Enabled = False
        Me.tx_売上端数.ForeColor = System.Drawing.SystemColors.WindowText
        Me.tx_売上端数.Location = New System.Drawing.Point(194, 56)
        Me.tx_売上端数.MaxLength = 0
        Me.tx_売上端数.Name = "tx_売上端数"
        Me.tx_売上端数.ReadOnly = True
        Me.tx_売上端数.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_売上端数.Size = New System.Drawing.Size(67, 22)
        Me.tx_売上端数.TabIndex = 57
        Me.tx_売上端数.Text = "売上端数"
        Me.tx_売上端数.Visible = False
        '
        'tx_消費税端数
        '
        Me.tx_消費税端数.AcceptsReturn = True
        Me.tx_消費税端数.BackColor = System.Drawing.SystemColors.Window
        Me.tx_消費税端数.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_消費税端数.Enabled = False
        Me.tx_消費税端数.ForeColor = System.Drawing.SystemColors.WindowText
        Me.tx_消費税端数.Location = New System.Drawing.Point(171, 56)
        Me.tx_消費税端数.MaxLength = 0
        Me.tx_消費税端数.Name = "tx_消費税端数"
        Me.tx_消費税端数.ReadOnly = True
        Me.tx_消費税端数.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_消費税端数.Size = New System.Drawing.Size(67, 22)
        Me.tx_消費税端数.TabIndex = 58
        Me.tx_消費税端数.Text = "消費税端数"
        Me.tx_消費税端数.Visible = False
        '
        'tx_見積日付
        '
        Me.tx_見積日付.AcceptsReturn = True
        Me.tx_見積日付.BackColor = System.Drawing.SystemColors.Window
        Me.tx_見積日付.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_見積日付.Enabled = False
        Me.tx_見積日付.ForeColor = System.Drawing.SystemColors.WindowText
        Me.tx_見積日付.Location = New System.Drawing.Point(144, 56)
        Me.tx_見積日付.MaxLength = 0
        Me.tx_見積日付.Name = "tx_見積日付"
        Me.tx_見積日付.ReadOnly = True
        Me.tx_見積日付.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_見積日付.Size = New System.Drawing.Size(67, 22)
        Me.tx_見積日付.TabIndex = 59
        Me.tx_見積日付.Text = "見積日付"
        Me.tx_見積日付.Visible = False
        '
        'tx_端数値引桁数
        '
        Me.tx_端数値引桁数.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_端数値引桁数.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_端数値引桁数.CanForwardSetFocus = True
        Me.tx_端数値引桁数.CanNextSetFocus = True
        Me.tx_端数値引桁数.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_端数値引桁数.DecimalPlace = CType(0, Short)
        Me.tx_端数値引桁数.EditMode = True
        Me.tx_端数値引桁数.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_端数値引桁数.FormatType = ""
        Me.tx_端数値引桁数.FormatTypeNega = ""
        Me.tx_端数値引桁数.FormatTypeNull = ""
        Me.tx_端数値引桁数.FormatTypeZero = ""
        Me.tx_端数値引桁数.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.tx_端数値引桁数.InputMinus = True
        Me.tx_端数値引桁数.InputPlus = True
        Me.tx_端数値引桁数.InputZero = True
        Me.tx_端数値引桁数.Location = New System.Drawing.Point(1066, 53)
        Me.tx_端数値引桁数.MaxLength = 1
        Me.tx_端数値引桁数.Name = "tx_端数値引桁数"
        Me.tx_端数値引桁数.OldValue = "ExNmTextBox"
        Me.tx_端数値引桁数.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_端数値引桁数.SelectText = True
        Me.tx_端数値引桁数.SelLength = 0
        Me.tx_端数値引桁数.SelStart = 0
        Me.tx_端数値引桁数.SelText = ""
        Me.tx_端数値引桁数.Size = New System.Drawing.Size(25, 22)
        Me.tx_端数値引桁数.TabIndex = 75
        '
        'tx_Welパー算出
        '
        Me.tx_Welパー算出.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_Welパー算出.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_Welパー算出.CanForwardSetFocus = True
        Me.tx_Welパー算出.CanNextSetFocus = True
        Me.tx_Welパー算出.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_Welパー算出.DecimalPlace = CType(0, Short)
        Me.tx_Welパー算出.EditMode = True
        Me.tx_Welパー算出.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_Welパー算出.FormatType = ""
        Me.tx_Welパー算出.FormatTypeNega = ""
        Me.tx_Welパー算出.FormatTypeNull = ""
        Me.tx_Welパー算出.FormatTypeZero = ""
        Me.tx_Welパー算出.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.tx_Welパー算出.InputMinus = True
        Me.tx_Welパー算出.InputPlus = True
        Me.tx_Welパー算出.InputZero = True
        Me.tx_Welパー算出.Location = New System.Drawing.Point(1020, 2)
        Me.tx_Welパー算出.MaxLength = 1
        Me.tx_Welパー算出.Name = "tx_Welパー算出"
        Me.tx_Welパー算出.OldValue = "ExNmTextBox"
        Me.tx_Welパー算出.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_Welパー算出.SelectText = True
        Me.tx_Welパー算出.SelLength = 0
        Me.tx_Welパー算出.SelStart = 0
        Me.tx_Welパー算出.SelText = ""
        Me.tx_Welパー算出.Size = New System.Drawing.Size(25, 22)
        Me.tx_Welパー算出.TabIndex = 79
        '
        'rf_売価確定
        '
        Me.rf_売価確定.BackColor = System.Drawing.SystemColors.Control
        Me.rf_売価確定.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_売価確定.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_売価確定.ForeColor = System.Drawing.Color.Red
        Me.rf_売価確定.Location = New System.Drawing.Point(666, 8)
        Me.rf_売価確定.Name = "rf_売価確定"
        Me.rf_売価確定.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_売価確定.Size = New System.Drawing.Size(105, 17)
        Me.rf_売価確定.TabIndex = 88
        Me.rf_売価確定.Text = "売価確定済み"
        Me.rf_売価確定.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        '_lb_項目_10
        '
        Me._lb_項目_10.BackColor = System.Drawing.SystemColors.Control
        Me._lb_項目_10.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_10.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_10.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_項目_10.Location = New System.Drawing.Point(339, 60)
        Me._lb_項目_10.Name = "_lb_項目_10"
        Me._lb_項目_10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_10.Size = New System.Drawing.Size(72, 15)
        Me._lb_項目_10.TabIndex = 87
        Me._lb_項目_10.Text = "原価合計"
        '
        'rf_原価合計
        '
        Me.rf_原価合計.BackColor = System.Drawing.SystemColors.Control
        Me.rf_原価合計.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_原価合計.Font = New System.Drawing.Font("ＭＳ ゴシック", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_原価合計.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_原価合計.Location = New System.Drawing.Point(413, 59)
        Me.rf_原価合計.Name = "rf_原価合計"
        Me.rf_原価合計.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_原価合計.Size = New System.Drawing.Size(134, 16)
        Me.rf_原価合計.TabIndex = 86
        Me.rf_原価合計.Text = "\1,234,567,890"
        Me.rf_原価合計.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'rf_1パー金額
        '
        Me.rf_1パー金額.BackColor = System.Drawing.SystemColors.Control
        Me.rf_1パー金額.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_1パー金額.Font = New System.Drawing.Font("ＭＳ ゴシック", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_1パー金額.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_1パー金額.Location = New System.Drawing.Point(1170, 6)
        Me.rf_1パー金額.Name = "rf_1パー金額"
        Me.rf_1パー金額.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_1パー金額.Size = New System.Drawing.Size(74, 15)
        Me.rf_1パー金額.TabIndex = 81
        Me.rf_1パー金額.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lb_項目_9
        '
        Me._lb_項目_9.BackColor = System.Drawing.SystemColors.Control
        Me._lb_項目_9.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_9.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_9.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_項目_9.Location = New System.Drawing.Point(1022, 32)
        Me._lb_項目_9.Name = "_lb_項目_9"
        Me._lb_項目_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_9.Size = New System.Drawing.Size(85, 15)
        Me._lb_項目_9.TabIndex = 78
        Me._lb_項目_9.Text = "税込金額"
        '
        'rf_税込金額
        '
        Me.rf_税込金額.BackColor = System.Drawing.SystemColors.Control
        Me.rf_税込金額.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_税込金額.Font = New System.Drawing.Font("ＭＳ ゴシック", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_税込金額.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_税込金額.Location = New System.Drawing.Point(1106, 30)
        Me.rf_税込金額.Name = "rf_税込金額"
        Me.rf_税込金額.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_税込金額.Size = New System.Drawing.Size(139, 15)
        Me.rf_税込金額.TabIndex = 77
        Me.rf_税込金額.Text = "\1,234,567,890"
        Me.rf_税込金額.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lb_項目_5
        '
        Me._lb_項目_5.BackColor = System.Drawing.SystemColors.Control
        Me._lb_項目_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_5.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_項目_5.Location = New System.Drawing.Point(1023, 60)
        Me._lb_項目_5.Name = "_lb_項目_5"
        Me._lb_項目_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_5.Size = New System.Drawing.Size(42, 17)
        Me._lb_項目_5.TabIndex = 74
        Me._lb_項目_5.Text = "端数"
        '
        'rf_端数値引金額
        '
        Me.rf_端数値引金額.BackColor = System.Drawing.SystemColors.Control
        Me.rf_端数値引金額.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_端数値引金額.Font = New System.Drawing.Font("ＭＳ ゴシック", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_端数値引金額.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_端数値引金額.Location = New System.Drawing.Point(1180, 59)
        Me.rf_端数値引金額.Name = "rf_端数値引金額"
        Me.rf_端数値引金額.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_端数値引金額.Size = New System.Drawing.Size(65, 15)
        Me.rf_端数値引金額.TabIndex = 73
        Me.rf_端数値引金額.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'rf_得意先別見積番号
        '
        Me.rf_得意先別見積番号.BackColor = System.Drawing.SystemColors.Control
        Me.rf_得意先別見積番号.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_得意先別見積番号.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_得意先別見積番号.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_得意先別見積番号.Location = New System.Drawing.Point(258, 60)
        Me.rf_得意先別見積番号.Name = "rf_得意先別見積番号"
        Me.rf_得意先別見積番号.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_得意先別見積番号.Size = New System.Drawing.Size(67, 13)
        Me.rf_得意先別見積番号.TabIndex = 66
        Me.rf_得意先別見積番号.Text = "21755009"
        Me.rf_得意先別見積番号.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.rf_得意先別見積番号.Visible = False
        '
        '_lb_項目_8
        '
        Me._lb_項目_8.BackColor = System.Drawing.SystemColors.Control
        Me._lb_項目_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_8.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_8.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_項目_8.Location = New System.Drawing.Point(962, 60)
        Me._lb_項目_8.Name = "_lb_項目_8"
        Me._lb_項目_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_8.Size = New System.Drawing.Size(20, 13)
        Me._lb_項目_8.TabIndex = 52
        Me._lb_項目_8.Text = "％"
        '
        '_lb_項目_7
        '
        Me._lb_項目_7.BackColor = System.Drawing.SystemColors.Control
        Me._lb_項目_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_7.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_項目_7.Location = New System.Drawing.Point(835, 60)
        Me._lb_項目_7.Name = "_lb_項目_7"
        Me._lb_項目_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_7.Size = New System.Drawing.Size(59, 14)
        Me._lb_項目_7.TabIndex = 51
        Me._lb_項目_7.Text = "原価率"
        '
        'rf_原価率
        '
        Me.rf_原価率.BackColor = System.Drawing.SystemColors.Control
        Me.rf_原価率.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_原価率.Font = New System.Drawing.Font("ＭＳ ゴシック", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_原価率.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_原価率.Location = New System.Drawing.Point(899, 59)
        Me.rf_原価率.Name = "rf_原価率"
        Me.rf_原価率.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_原価率.Size = New System.Drawing.Size(60, 15)
        Me.rf_原価率.TabIndex = 50
        Me.rf_原価率.Text = "100.99"
        Me.rf_原価率.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'rf_合計金額
        '
        Me.rf_合計金額.BackColor = System.Drawing.SystemColors.Control
        Me.rf_合計金額.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_合計金額.Font = New System.Drawing.Font("ＭＳ ゴシック", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_合計金額.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_合計金額.Location = New System.Drawing.Point(644, 59)
        Me.rf_合計金額.Name = "rf_合計金額"
        Me.rf_合計金額.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_合計金額.Size = New System.Drawing.Size(140, 16)
        Me.rf_合計金額.TabIndex = 49
        Me.rf_合計金額.Text = "\1,234,567,890"
        Me.rf_合計金額.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'rf_得意先CD
        '
        Me.rf_得意先CD.BackColor = System.Drawing.SystemColors.Control
        Me.rf_得意先CD.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_得意先CD.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_得意先CD.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_得意先CD.Location = New System.Drawing.Point(633, 32)
        Me.rf_得意先CD.Name = "rf_得意先CD"
        Me.rf_得意先CD.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_得意先CD.Size = New System.Drawing.Size(39, 16)
        Me.rf_得意先CD.TabIndex = 48
        Me.rf_得意先CD.Text = "1234"
        '
        '_lb_項目_2
        '
        Me._lb_項目_2.BackColor = System.Drawing.SystemColors.Control
        Me._lb_項目_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_2.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_項目_2.Location = New System.Drawing.Point(560, 32)
        Me._lb_項目_2.Name = "_lb_項目_2"
        Me._lb_項目_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_2.Size = New System.Drawing.Size(63, 14)
        Me._lb_項目_2.TabIndex = 47
        Me._lb_項目_2.Text = "得意先"
        '
        'rf_得意先名
        '
        Me.rf_得意先名.BackColor = System.Drawing.SystemColors.Control
        Me.rf_得意先名.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_得意先名.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_得意先名.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_得意先名.Location = New System.Drawing.Point(672, 32)
        Me.rf_得意先名.Name = "rf_得意先名"
        Me.rf_得意先名.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_得意先名.Size = New System.Drawing.Size(327, 16)
        Me.rf_得意先名.TabIndex = 46
        Me.rf_得意先名.Text = "１２３４５６７８９０１２３４"
        '
        'rf_処理区分
        '
        Me.rf_処理区分.BackColor = System.Drawing.SystemColors.Control
        Me.rf_処理区分.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.rf_処理区分.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_処理区分.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_処理区分.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_処理区分.Location = New System.Drawing.Point(4, 4)
        Me.rf_処理区分.Name = "rf_処理区分"
        Me.rf_処理区分.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_処理区分.Size = New System.Drawing.Size(84, 20)
        Me.rf_処理区分.TabIndex = 39
        Me.rf_処理区分.Text = "≪登 録≫"
        Me.rf_処理区分.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        '_lb_項目_6
        '
        Me._lb_項目_6.BackColor = System.Drawing.SystemColors.Control
        Me._lb_項目_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_6.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_項目_6.Location = New System.Drawing.Point(559, 60)
        Me._lb_項目_6.Name = "_lb_項目_6"
        Me._lb_項目_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_6.Size = New System.Drawing.Size(73, 15)
        Me._lb_項目_6.TabIndex = 38
        Me._lb_項目_6.Text = "合計金額"
        '
        'rf_見積名称
        '
        Me.rf_見積名称.BackColor = System.Drawing.SystemColors.Control
        Me.rf_見積名称.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_見積名称.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_見積名称.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_見積名称.Location = New System.Drawing.Point(248, 32)
        Me.rf_見積名称.Name = "rf_見積名称"
        Me.rf_見積名称.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_見積名称.Size = New System.Drawing.Size(310, 16)
        Me.rf_見積名称.TabIndex = 35
        Me.rf_見積名称.Text = "スーパーOKAMURA"
        '
        '_lb_項目_3
        '
        Me._lb_項目_3.BackColor = System.Drawing.SystemColors.Control
        Me._lb_項目_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_3.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_項目_3.Location = New System.Drawing.Point(202, 32)
        Me._lb_項目_3.Name = "_lb_項目_3"
        Me._lb_項目_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_3.Size = New System.Drawing.Size(41, 15)
        Me._lb_項目_3.TabIndex = 34
        Me._lb_項目_3.Text = "名称"
        '
        'rf_見積番号
        '
        Me.rf_見積番号.BackColor = System.Drawing.SystemColors.Control
        Me.rf_見積番号.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_見積番号.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_見積番号.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_見積番号.Location = New System.Drawing.Point(92, 32)
        Me.rf_見積番号.Name = "rf_見積番号"
        Me.rf_見積番号.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_見積番号.Size = New System.Drawing.Size(67, 13)
        Me.rf_見積番号.TabIndex = 33
        Me.rf_見積番号.Text = "21755009"
        Me.rf_見積番号.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lb_項目_1
        '
        Me._lb_項目_1.BackColor = System.Drawing.SystemColors.Control
        Me._lb_項目_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_1.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_項目_1.Location = New System.Drawing.Point(16, 32)
        Me._lb_項目_1.Name = "_lb_項目_1"
        Me._lb_項目_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_1.Size = New System.Drawing.Size(62, 16)
        Me._lb_項目_1.TabIndex = 32
        Me._lb_項目_1.Text = "見積№"
        '
        'sb_Msg
        '
        Me.sb_Msg.Font = New System.Drawing.Font("MS UI Gothic", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.sb_Msg.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.sb_Msg.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.sb_Msg_Panel1, Me.sb_Msg_Panel2, Me.sb_Msg_Panel3})
        Me.sb_Msg.Location = New System.Drawing.Point(0, 469)
        Me.sb_Msg.Name = "sb_Msg"
        Me.sb_Msg.Size = New System.Drawing.Size(1492, 22)
        Me.sb_Msg.TabIndex = 25
        '
        'sb_Msg_Panel1
        '
        Me.sb_Msg_Panel1.AutoSize = False
        Me.sb_Msg_Panel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.sb_Msg_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me.sb_Msg_Panel1.Font = New System.Drawing.Font("MS UI Gothic", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.sb_Msg_Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me.sb_Msg_Panel1.Name = "sb_Msg_Panel1"
        Me.sb_Msg_Panel1.Size = New System.Drawing.Size(1670, 22)
        Me.sb_Msg_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.sb_Msg_Panel1.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        '
        'sb_Msg_Panel2
        '
        Me.sb_Msg_Panel2.AutoSize = False
        Me.sb_Msg_Panel2.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.sb_Msg_Panel2.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me.sb_Msg_Panel2.Margin = New System.Windows.Forms.Padding(0)
        Me.sb_Msg_Panel2.Name = "sb_Msg_Panel2"
        Me.sb_Msg_Panel2.Overflow = System.Windows.Forms.ToolStripItemOverflow.Always
        Me.sb_Msg_Panel2.Size = New System.Drawing.Size(96, 22)
        Me.sb_Msg_Panel2.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        '
        'sb_Msg_Panel3
        '
        Me.sb_Msg_Panel3.AutoSize = False
        Me.sb_Msg_Panel3.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.sb_Msg_Panel3.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me.sb_Msg_Panel3.Margin = New System.Windows.Forms.Padding(0)
        Me.sb_Msg_Panel3.Name = "sb_Msg_Panel3"
        Me.sb_Msg_Panel3.Overflow = System.Windows.Forms.ToolStripItemOverflow.Always
        Me.sb_Msg_Panel3.Size = New System.Drawing.Size(55, 22)
        Me.sb_Msg_Panel3.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        '
        'PicFunction
        '
        Me.PicFunction.BackColor = System.Drawing.SystemColors.Control
        Me.PicFunction.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PicFunction.Cursor = System.Windows.Forms.Cursors.Default
        Me.PicFunction.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PicFunction.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.PicFunction.ForeColor = System.Drawing.SystemColors.ControlText
        Me.PicFunction.Location = New System.Drawing.Point(0, 426)
        Me.PicFunction.Name = "PicFunction"
        Me.PicFunction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.PicFunction.Size = New System.Drawing.Size(1492, 43)
        Me.PicFunction.TabIndex = 2
        '
        'fr_Disp
        '
        Me.fr_Disp.BackColor = System.Drawing.SystemColors.Control
        Me.fr_Disp.Controls.Add(Me._lb_項目_4)
        Me.fr_Disp.Controls.Add(Me.tx_製品NO)
        Me.fr_Disp.Controls.Add(Me.Cb検索)
        Me.fr_Disp.Controls.Add(Me._lb_項目_0)
        Me.fr_Disp.Cursor = System.Windows.Forms.Cursors.Default
        Me.fr_Disp.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.fr_Disp.ForeColor = System.Drawing.SystemColors.WindowText
        Me.fr_Disp.Location = New System.Drawing.Point(0, 395)
        Me.fr_Disp.Name = "fr_Disp"
        Me.fr_Disp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fr_Disp.Size = New System.Drawing.Size(1492, 31)
        Me.fr_Disp.TabIndex = 41
        '
        '_lb_項目_4
        '
        Me._lb_項目_4.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_項目_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_4.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_4.ForeColor = System.Drawing.Color.White
        Me._lb_項目_4.Location = New System.Drawing.Point(4, 7)
        Me._lb_項目_4.Name = "_lb_項目_4"
        Me._lb_項目_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_4.Size = New System.Drawing.Size(61, 19)
        Me._lb_項目_4.TabIndex = 70
        Me._lb_項目_4.Text = "検索"
        Me._lb_項目_4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'tx_製品NO
        '
        Me.tx_製品NO.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_製品NO.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_製品NO.CanForwardSetFocus = False
        Me.tx_製品NO.CanNextSetFocus = False
        Me.tx_製品NO.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_製品NO.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_製品NO.EditMode = True
        Me.tx_製品NO.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_製品NO.IMEMode = System.Windows.Forms.ImeMode.Off
        Me.tx_製品NO.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_製品NO.Location = New System.Drawing.Point(70, 5)
        Me.tx_製品NO.MaxLength = 0
        Me.tx_製品NO.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_製品NO.Name = "tx_製品NO"
        Me.tx_製品NO.OldValue = ""
        Me.tx_製品NO.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_製品NO.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_製品NO.SelectText = False
        Me.tx_製品NO.SelLength = 0
        Me.tx_製品NO.SelStart = 0
        Me.tx_製品NO.SelText = ""
        Me.tx_製品NO.Size = New System.Drawing.Size(93, 22)
        Me.tx_製品NO.TabIndex = 69
        Me.tx_製品NO.Text = ""
        '
        'Cb検索
        '
        Me.Cb検索.BackColor = System.Drawing.SystemColors.Control
        Me.Cb検索.Cursor = System.Windows.Forms.Cursors.Default
        Me.Cb検索.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Cb検索.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Cb検索.Location = New System.Drawing.Point(168, 5)
        Me.Cb検索.Name = "Cb検索"
        Me.Cb検索.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Cb検索.Size = New System.Drawing.Size(74, 22)
        Me.Cb検索.TabIndex = 71
        Me.Cb検索.Text = "検索(&K)"
        Me.Cb検索.UseVisualStyleBackColor = False
        '
        '_lb_項目_0
        '
        Me._lb_項目_0.BackColor = System.Drawing.SystemColors.Control
        Me._lb_項目_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_0.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_項目_0.Location = New System.Drawing.Point(248, 9)
        Me._lb_項目_0.Name = "_lb_項目_0"
        Me._lb_項目_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_0.Size = New System.Drawing.Size(77, 17)
        Me._lb_項目_0.TabIndex = 31
        Me._lb_項目_0.Text = "表示切替"
        Me._lb_項目_0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Timer1
        '
        '
        'FpSpd
        '
        Me.FpSpd.AccessibleDescription = "FpSpd, Sheet1, Row 0, Column 0"
        Me.FpSpd.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 11.0!)
        Me.FpSpd.Location = New System.Drawing.Point(4, 84)
        Me.FpSpd.Name = "FpSpd"
        Me.FpSpd.Size = New System.Drawing.Size(986, 305)
        Me.FpSpd.TabIndex = 233
        '
        'SnwMT02F00
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(1492, 491)
        Me.Controls.Add(Me.FpSpd)
        Me.Controls.Add(Me.fr_Disp)
        Me.Controls.Add(Me.SubFunction)
        Me.Controls.Add(Me.PicFunction)
        Me.Controls.Add(Me.sb_Msg)
        Me.Controls.Add(Me.tx_見積確定区分)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me._lblLabels_43)
        Me.Controls.Add(Me.Cbレビット取込)
        Me.Controls.Add(Me.Cb員数取込)
        Me.Controls.Add(Me.Ck_MoveMode)
        Me.Controls.Add(Me.Cbしまむら消耗品取込)
        Me.Controls.Add(Me.CbWel1パー算出)
        Me.Controls.Add(Me.Cb端数値引計算)
        Me.Controls.Add(Me.Ck_DragMode)
        Me.Controls.Add(Me.Command1)
        Me.Controls.Add(Me.Cb固定列)
        Me.Controls.Add(Me.tx_大小口区分)
        Me.Controls.Add(Me.CmdChkOn)
        Me.Controls.Add(Me.CmdChkOff)
        Me.Controls.Add(Me.tx_受注区分)
        Me.Controls.Add(Me.Cb展開)
        Me.Controls.Add(Me.tx_Dummy1)
        Me.Controls.Add(Me.tx_外税額)
        Me.Controls.Add(Me.tx_原価合計)
        Me.Controls.Add(Me.tx_合計金額)
        Me.Controls.Add(Me.tx_売上端数)
        Me.Controls.Add(Me.tx_消費税端数)
        Me.Controls.Add(Me.tx_見積日付)
        Me.Controls.Add(Me.tx_端数値引桁数)
        Me.Controls.Add(Me.tx_Welパー算出)
        Me.Controls.Add(Me.rf_売価確定)
        Me.Controls.Add(Me._lb_項目_10)
        Me.Controls.Add(Me.rf_原価合計)
        Me.Controls.Add(Me.rf_1パー金額)
        Me.Controls.Add(Me._lb_項目_9)
        Me.Controls.Add(Me.rf_税込金額)
        Me.Controls.Add(Me._lb_項目_5)
        Me.Controls.Add(Me.rf_端数値引金額)
        Me.Controls.Add(Me.rf_得意先別見積番号)
        Me.Controls.Add(Me._lb_項目_8)
        Me.Controls.Add(Me._lb_項目_7)
        Me.Controls.Add(Me.rf_原価率)
        Me.Controls.Add(Me.rf_合計金額)
        Me.Controls.Add(Me.rf_得意先CD)
        Me.Controls.Add(Me._lb_項目_2)
        Me.Controls.Add(Me.rf_得意先名)
        Me.Controls.Add(Me.rf_処理区分)
        Me.Controls.Add(Me._lb_項目_6)
        Me.Controls.Add(Me.rf_見積名称)
        Me.Controls.Add(Me._lb_項目_3)
        Me.Controls.Add(Me.rf_見積番号)
        Me.Controls.Add(Me._lb_項目_1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(4, 30)
        Me.Name = "SnwMT02F00"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "員数入力"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.sb_Msg.ResumeLayout(False)
        Me.sb_Msg.PerformLayout()
        Me.fr_Disp.ResumeLayout(False)
        CType(Me.FpSpd, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Public WithEvents tx_見積確定区分 As ExNmText.ExNmTextBox
    Public WithEvents Label12 As Label
    Public WithEvents _lblLabels_43 As Label
    Public WithEvents fr_Disp As Panel
    Public WithEvents _lb_項目_4 As Label
    Public WithEvents tx_製品NO As ExText.ExTextBox
    Public WithEvents Cb検索 As Button
    Public WithEvents _lb_項目_0 As Label
    Friend WithEvents SubFunction As Panel
    Friend WithEvents Timer1 As Timer
    Friend WithEvents FpSpd As FarPoint.Win.Spread.FpSpread
    Friend WithEvents FpSpd_Sheet1 As FarPoint.Win.Spread.SheetView
End Class