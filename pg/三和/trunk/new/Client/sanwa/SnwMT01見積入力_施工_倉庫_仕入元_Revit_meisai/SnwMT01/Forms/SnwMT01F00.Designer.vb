<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SnwMT01F00

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
	Public WithEvents ck_社内伝票扱い As System.Windows.Forms.CheckBox
    Public WithEvents tx_受注日付Y As ExDateText.ExDateTextBoxY
    Public WithEvents tx_受注日付M As ExDateText.ExDateTextBoxM
    Public WithEvents tx_受注日付D As ExDateText.ExDateTextBoxD
    Public WithEvents CbTabEnd As System.Windows.Forms.Button
    Public WithEvents PicFunction As System.Windows.Forms.Panel
    Public WithEvents sb_Msg_Panel1 As System.Windows.Forms.ToolStripStatusLabel
    Public WithEvents sb_Msg_Panel2 As System.Windows.Forms.ToolStripStatusLabel
    Public WithEvents sb_Msg_Panel3 As System.Windows.Forms.ToolStripStatusLabel
    Public WithEvents sb_Msg As System.Windows.Forms.StatusStrip
    Public WithEvents tx_得意先名2 As ExText.ExTextBox
    Public WithEvents tx_備考 As ExText.ExTextBox
    Public WithEvents tx_得意先名1 As ExText.ExTextBox
    Public WithEvents tx_物件金額 As ExNmText.ExNmTextBox
    Public WithEvents tx_出精値引 As ExNmText.ExNmTextBox
    Public WithEvents tx_見積件名 As ExText.ExTextBox
    Public WithEvents tx_得TEL As ExText.ExTextBox
    Public WithEvents tx_得FAX As ExText.ExTextBox
    Public WithEvents tx_得担当者 As ExText.ExTextBox
    Public WithEvents tx_納入先CD As ExText.ExTextBox
    Public WithEvents tx_納得意先CD As ExText.ExTextBox
    Public WithEvents tx_納入先名2 As ExText.ExTextBox
    Public WithEvents tx_納入先名1 As ExText.ExTextBox
    Public WithEvents tx_郵便番号 As ExText.ExTextBox
    Public WithEvents tx_納住所1 As ExText.ExTextBox
    Public WithEvents tx_納住所2 As ExText.ExTextBox
    Public WithEvents tx_納TEL As ExText.ExTextBox
    Public WithEvents tx_納FAX As ExText.ExTextBox
    Public WithEvents tx_納担当者 As ExText.ExTextBox
    Public WithEvents tx_s納期Y As ExDateText.ExDateTextBoxY
    Public WithEvents tx_s納期M As ExDateText.ExDateTextBoxM
    Public WithEvents tx_s納期D As ExDateText.ExDateTextBoxD
    Public WithEvents tx_e納期Y As ExDateText.ExDateTextBoxY
    Public WithEvents tx_e納期M As ExDateText.ExDateTextBoxM
    Public WithEvents tx_e納期D As ExDateText.ExDateTextBoxD
    Public WithEvents tx_得意先CD As ExText.ExTextBox
    Public WithEvents tx_OPEN日Y As ExDateText.ExDateTextBoxY
    Public WithEvents tx_OPEN日M As ExDateText.ExDateTextBoxM
    Public WithEvents tx_OPEN日D As ExDateText.ExDateTextBoxD
    Public WithEvents tx_物件種別 As ExNmText.ExNmTextBox
    Public WithEvents tx_納期表示 As ExNmText.ExNmTextBox
    Public WithEvents tx_納期表示他 As ExText.ExTextBox
    Public WithEvents tx_出力日 As ExNmText.ExNmTextBox
    Public WithEvents tx_有効期限 As ExNmText.ExNmTextBox
    Public WithEvents tx_受注区分 As ExNmText.ExNmTextBox
    Public WithEvents tx_大小口区分 As ExNmText.ExNmTextBox
    Public WithEvents tx_見積日付Y As ExDateText.ExDateTextBoxY
    Public WithEvents tx_見積日付M As ExDateText.ExDateTextBoxM
    Public WithEvents tx_見積日付D As ExDateText.ExDateTextBoxD
    Public WithEvents tx_現場名 As ExText.ExTextBox
    Public WithEvents tx_支払条件 As ExNmText.ExNmTextBox
    Public WithEvents tx_支払条件他 As ExText.ExTextBox
    Public WithEvents tx_担当者CD As ExNmText.ExNmTextBox
    Public WithEvents rf_販売先得意先名2 As ExText.ExTextBox
    Public WithEvents rf_販売先得意先名1 As ExText.ExTextBox
    Public WithEvents tx_販売先得意先CD As ExText.ExTextBox
    Public WithEvents tx_販売先納入先CD As ExText.ExTextBox
    Public WithEvents tx_販売先納得意先CD As ExText.ExTextBox
    Public WithEvents rf_販売先納入先名2 As ExText.ExTextBox
    Public WithEvents rf_販売先納入先名1 As ExText.ExTextBox
    Public WithEvents tx_部署CD As ExNmText.ExNmTextBox
    Public WithEvents tx_物件番号 As ExNmText.ExNmTextBox
    Public WithEvents tx_伝票種類 As ExNmText.ExNmTextBox
    Public WithEvents tx_ウエルシアリース区分 As ExNmText.ExNmTextBox
    Public WithEvents tx_ウエルシア物件区分 As ExNmText.ExNmTextBox
    Public WithEvents tx_ウエルシア物件内容CD As ExNmText.ExNmTextBox
    Public WithEvents tx_ウエルシア物件内容名 As ExText.ExTextBox
    Public WithEvents tx_ウエルシア売場面積 As ExNmText.ExNmTextBox
    Public WithEvents tx_受付日付Y As ExDateText.ExDateTextBoxY
    Public WithEvents tx_受付日付M As ExDateText.ExDateTextBoxM
    Public WithEvents tx_受付日付D As ExDateText.ExDateTextBoxD
    Public WithEvents tx_完工日付Y As ExDateText.ExDateTextBoxY
    Public WithEvents tx_完工日付M As ExDateText.ExDateTextBoxM
    Public WithEvents tx_完工日付D As ExDateText.ExDateTextBoxD
    Public WithEvents tx_発注担当者名 As ExText.ExTextBox
    Public WithEvents tx_作業内容 As ExText.ExTextBox
    Public WithEvents tx_YKサプライ区分 As ExNmText.ExNmTextBox
    Public WithEvents tx_YK物件区分 As ExNmText.ExNmTextBox
    Public WithEvents tx_YK請求区分 As ExNmText.ExNmTextBox
    Public WithEvents tx_化粧品メーカー区分 As ExNmText.ExNmTextBox
    Public WithEvents tx_SM内容区分 As ExNmText.ExNmTextBox
    Public WithEvents tx_税集計区分 As ExNmText.ExNmTextBox
    Public WithEvents tx_クレーム区分 As ExNmText.ExNmTextBox
    Public WithEvents tx_工事担当CD As ExNmText.ExNmTextBox
    Public WithEvents tx_発注書発行日付 As ExText.ExTextBox
    Public WithEvents tx_完了者名 As ExText.ExTextBox
    Public WithEvents tx_見積確定区分 As ExNmText.ExNmTextBox
    Public WithEvents tx_経過備考1 As ExText.ExTextBox
    Public WithEvents tx_経過備考2 As ExText.ExTextBox
    Public WithEvents tx_請求予定Y As ExDateText.ExDateTextBoxY
    Public WithEvents tx_請求予定M As ExDateText.ExDateTextBoxM
    Public WithEvents tx_請求予定D As ExDateText.ExDateTextBoxD
    Public WithEvents tx_完了日Y As ExDateText.ExDateTextBoxY
    Public WithEvents tx_完了日M As ExDateText.ExDateTextBoxM
    Public WithEvents tx_完了日D As ExDateText.ExDateTextBoxD
    Public WithEvents tx_集計CD As ExText.ExTextBox
    Public WithEvents tx_B請求管轄区分 As ExNmText.ExNmTextBox
    Public WithEvents tx_BtoB番号 As ExNmText.ExNmTextBox
    Public WithEvents tx_業種区分 As ExNmText.ExNmTextBox
    Public WithEvents tx_統合見積番号 As ExNmText.ExNmTextBox
    Public WithEvents rf_統合見積件名 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_58 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_57 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_56 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_55 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_54 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_15 As System.Windows.Forms.Label
    Public WithEvents rf_集計名 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_53 As System.Windows.Forms.Label
    Public WithEvents _lb_日_8 As System.Windows.Forms.Label
    Public WithEvents Label14 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_14 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_13 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_52 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_12 As System.Windows.Forms.Label
    Public WithEvents Label13 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_51 As System.Windows.Forms.Label
    Public WithEvents Label12 As System.Windows.Forms.Label
    Public WithEvents Label9 As System.Windows.Forms.Label
    Public WithEvents _lb_日_7 As System.Windows.Forms.Label
    Public WithEvents _lb_月_7 As System.Windows.Forms.Label
    Public WithEvents _lb_年_7 As System.Windows.Forms.Label
    Public WithEvents Label10 As System.Windows.Forms.Label
    Public WithEvents rf_工事担当名 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_50 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_49 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_48 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_47 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_1 As System.Windows.Forms.Label
    Public WithEvents rf_税集計区分名 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_46 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_45 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_44 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_43 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_11 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_42 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_41 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_40 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_39 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_36 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_35 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_38 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_37 As System.Windows.Forms.Label
    Public WithEvents Label7 As System.Windows.Forms.Label
    Public WithEvents _lb_日_6 As System.Windows.Forms.Label
    Public WithEvents _lb_月_6 As System.Windows.Forms.Label
    Public WithEvents _lb_年_6 As System.Windows.Forms.Label
    Public WithEvents Label5 As System.Windows.Forms.Label
    Public WithEvents _lb_日_5 As System.Windows.Forms.Label
    Public WithEvents _lb_月_5 As System.Windows.Forms.Label
    Public WithEvents _lb_年_5 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_10 As System.Windows.Forms.Label
    Public WithEvents rf_物件略称 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_9 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_31 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_30 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_29 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_7 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_28 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_27 As System.Windows.Forms.Label
    Public WithEvents Label4 As System.Windows.Forms.Label
    Public WithEvents rf_部署名 As System.Windows.Forms.Label
    Public WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_8 As System.Windows.Forms.Label
    Public WithEvents rf_得意先別見積番号 As System.Windows.Forms.Label
    Public WithEvents rf_外税額 As System.Windows.Forms.Label
    Public WithEvents rf_原価率 As System.Windows.Forms.Label
    Public WithEvents rf_原価合計 As System.Windows.Forms.Label
    Public WithEvents rf_合計金額 As System.Windows.Forms.Label
    Public WithEvents rf_売上端数 As System.Windows.Forms.Label
    Public WithEvents rf_消費税端数 As System.Windows.Forms.Label
    Public WithEvents rf_税集計区分 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_25 As System.Windows.Forms.Label
    Public WithEvents _lb_年_4 As System.Windows.Forms.Label
    Public WithEvents _lb_月_4 As System.Windows.Forms.Label
    Public WithEvents _lb_日_4 As System.Windows.Forms.Label
    Public WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_15 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_9 As System.Windows.Forms.Label
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_5 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_26 As System.Windows.Forms.Label
    Public WithEvents _lb_日_3 As System.Windows.Forms.Label
    Public WithEvents _lb_月_3 As System.Windows.Forms.Label
    Public WithEvents _lb_年_3 As System.Windows.Forms.Label
    Public WithEvents _lb_月_1 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_24 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_23 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_22 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_21 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_20 As System.Windows.Forms.Label
    Public WithEvents rf_担当者名 As System.Windows.Forms.Label
    Public WithEvents lb_担当者CD As System.Windows.Forms.Label
    Public WithEvents _lblLabels_19 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_18 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_17 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_16 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_14 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_13 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_12 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_11 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_8 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_7 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_6 As System.Windows.Forms.Label
    Public WithEvents _lb_日_2 As System.Windows.Forms.Label
    Public WithEvents _lb_月_2 As System.Windows.Forms.Label
    Public WithEvents _lb_年_2 As System.Windows.Forms.Label
    Public WithEvents _lb_kara_0 As System.Windows.Forms.Label
    Public WithEvents _lb_年_0 As System.Windows.Forms.Label
    Public WithEvents _lb_月_0 As System.Windows.Forms.Label
    Public WithEvents _lb_日_0 As System.Windows.Forms.Label
    Public WithEvents _lb_年_1 As System.Windows.Forms.Label
    Public WithEvents _lb_日_1 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_5 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_4 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_3 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_2 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_0 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_10 As System.Windows.Forms.Label
    Public WithEvents lb_見積番号 As System.Windows.Forms.Label
    Public WithEvents rf_見積番号 As System.Windows.Forms.Label
    Public WithEvents lb_見積件名 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_0 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_6 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_2 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_3 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_4 As System.Windows.Forms.Label
    Public WithEvents _lb_項目_1 As System.Windows.Forms.Label
    Public WithEvents rf_処理区分 As System.Windows.Forms.Label
    Public WithEvents _lb_納期_0 As System.Windows.Forms.Label
    Public WithEvents _lb_納期_1 As System.Windows.Forms.Label
    Public WithEvents lb_OPEN日 As System.Windows.Forms.Label
    Public WithEvents lb_見積日付 As System.Windows.Forms.Label
    Public WithEvents lb_受注日付 As System.Windows.Forms.Label
    Public WithEvents Label6 As System.Windows.Forms.Label
    Public WithEvents Label8 As System.Windows.Forms.Label
    Public WithEvents Label11 As System.Windows.Forms.Label
    Public WithEvents rf_ウエルシア物件区分名 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_34 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_33 As System.Windows.Forms.Label
    Public WithEvents _lblLabels_32 As System.Windows.Forms.Label
    Public WithEvents _lb_コンテナ_1 As System.Windows.Forms.Label
    Public WithEvents Label16 As System.Windows.Forms.Label

    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使って変更できます。
    'コード エディタを使用して、変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SnwMT01F00))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.ck_社内伝票扱い = New System.Windows.Forms.CheckBox()
        Me.tx_受注日付Y = New ExDateText.ExDateTextBoxY()
        Me.tx_受注日付M = New ExDateText.ExDateTextBoxM()
        Me.tx_受注日付D = New ExDateText.ExDateTextBoxD()
        Me.PicFunction = New System.Windows.Forms.Panel()
        Me.CbTabEnd = New System.Windows.Forms.Button()
        Me.sb_Msg = New System.Windows.Forms.StatusStrip()
        Me.sb_Msg_Panel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.sb_Msg_Panel2 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.sb_Msg_Panel3 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tx_得意先名2 = New ExText.ExTextBox()
        Me.tx_備考 = New ExText.ExTextBox()
        Me.tx_得意先名1 = New ExText.ExTextBox()
        Me.tx_物件金額 = New ExNmText.ExNmTextBox()
        Me.tx_出精値引 = New ExNmText.ExNmTextBox()
        Me.tx_見積件名 = New ExText.ExTextBox()
        Me.tx_得TEL = New ExText.ExTextBox()
        Me.tx_得FAX = New ExText.ExTextBox()
        Me.tx_得担当者 = New ExText.ExTextBox()
        Me.tx_納入先CD = New ExText.ExTextBox()
        Me.tx_納得意先CD = New ExText.ExTextBox()
        Me.tx_納入先名2 = New ExText.ExTextBox()
        Me.tx_納入先名1 = New ExText.ExTextBox()
        Me.tx_郵便番号 = New ExText.ExTextBox()
        Me.tx_納住所1 = New ExText.ExTextBox()
        Me.tx_納住所2 = New ExText.ExTextBox()
        Me.tx_納TEL = New ExText.ExTextBox()
        Me.tx_納FAX = New ExText.ExTextBox()
        Me.tx_納担当者 = New ExText.ExTextBox()
        Me.tx_s納期Y = New ExDateText.ExDateTextBoxY()
        Me.tx_s納期M = New ExDateText.ExDateTextBoxM()
        Me.tx_s納期D = New ExDateText.ExDateTextBoxD()
        Me.tx_e納期Y = New ExDateText.ExDateTextBoxY()
        Me.tx_e納期M = New ExDateText.ExDateTextBoxM()
        Me.tx_e納期D = New ExDateText.ExDateTextBoxD()
        Me.tx_得意先CD = New ExText.ExTextBox()
        Me.tx_OPEN日Y = New ExDateText.ExDateTextBoxY()
        Me.tx_OPEN日M = New ExDateText.ExDateTextBoxM()
        Me.tx_OPEN日D = New ExDateText.ExDateTextBoxD()
        Me.tx_物件種別 = New ExNmText.ExNmTextBox()
        Me.tx_納期表示 = New ExNmText.ExNmTextBox()
        Me.tx_納期表示他 = New ExText.ExTextBox()
        Me.tx_出力日 = New ExNmText.ExNmTextBox()
        Me.tx_有効期限 = New ExNmText.ExNmTextBox()
        Me.tx_受注区分 = New ExNmText.ExNmTextBox()
        Me.tx_大小口区分 = New ExNmText.ExNmTextBox()
        Me.tx_見積日付Y = New ExDateText.ExDateTextBoxY()
        Me.tx_見積日付M = New ExDateText.ExDateTextBoxM()
        Me.tx_見積日付D = New ExDateText.ExDateTextBoxD()
        Me.tx_現場名 = New ExText.ExTextBox()
        Me.tx_支払条件 = New ExNmText.ExNmTextBox()
        Me.tx_支払条件他 = New ExText.ExTextBox()
        Me.tx_担当者CD = New ExNmText.ExNmTextBox()
        Me.rf_販売先得意先名2 = New ExText.ExTextBox()
        Me.rf_販売先得意先名1 = New ExText.ExTextBox()
        Me.tx_販売先得意先CD = New ExText.ExTextBox()
        Me.tx_販売先納入先CD = New ExText.ExTextBox()
        Me.tx_販売先納得意先CD = New ExText.ExTextBox()
        Me.rf_販売先納入先名2 = New ExText.ExTextBox()
        Me.rf_販売先納入先名1 = New ExText.ExTextBox()
        Me.tx_部署CD = New ExNmText.ExNmTextBox()
        Me.tx_物件番号 = New ExNmText.ExNmTextBox()
        Me.tx_伝票種類 = New ExNmText.ExNmTextBox()
        Me.tx_ウエルシアリース区分 = New ExNmText.ExNmTextBox()
        Me.tx_ウエルシア物件区分 = New ExNmText.ExNmTextBox()
        Me.tx_ウエルシア物件内容CD = New ExNmText.ExNmTextBox()
        Me.tx_ウエルシア物件内容名 = New ExText.ExTextBox()
        Me.tx_ウエルシア売場面積 = New ExNmText.ExNmTextBox()
        Me.tx_受付日付Y = New ExDateText.ExDateTextBoxY()
        Me.tx_受付日付M = New ExDateText.ExDateTextBoxM()
        Me.tx_受付日付D = New ExDateText.ExDateTextBoxD()
        Me.tx_完工日付Y = New ExDateText.ExDateTextBoxY()
        Me.tx_完工日付M = New ExDateText.ExDateTextBoxM()
        Me.tx_完工日付D = New ExDateText.ExDateTextBoxD()
        Me.tx_発注担当者名 = New ExText.ExTextBox()
        Me.tx_作業内容 = New ExText.ExTextBox()
        Me.tx_YKサプライ区分 = New ExNmText.ExNmTextBox()
        Me.tx_YK物件区分 = New ExNmText.ExNmTextBox()
        Me.tx_YK請求区分 = New ExNmText.ExNmTextBox()
        Me.tx_化粧品メーカー区分 = New ExNmText.ExNmTextBox()
        Me.tx_SM内容区分 = New ExNmText.ExNmTextBox()
        Me.tx_税集計区分 = New ExNmText.ExNmTextBox()
        Me.tx_クレーム区分 = New ExNmText.ExNmTextBox()
        Me.tx_工事担当CD = New ExNmText.ExNmTextBox()
        Me.tx_発注書発行日付 = New ExText.ExTextBox()
        Me.tx_完了者名 = New ExText.ExTextBox()
        Me.tx_見積確定区分 = New ExNmText.ExNmTextBox()
        Me.tx_経過備考1 = New ExText.ExTextBox()
        Me.tx_経過備考2 = New ExText.ExTextBox()
        Me.tx_請求予定Y = New ExDateText.ExDateTextBoxY()
        Me.tx_請求予定M = New ExDateText.ExDateTextBoxM()
        Me.tx_請求予定D = New ExDateText.ExDateTextBoxD()
        Me.tx_完了日Y = New ExDateText.ExDateTextBoxY()
        Me.tx_完了日M = New ExDateText.ExDateTextBoxM()
        Me.tx_完了日D = New ExDateText.ExDateTextBoxD()
        Me.tx_集計CD = New ExText.ExTextBox()
        Me.tx_B請求管轄区分 = New ExNmText.ExNmTextBox()
        Me.tx_BtoB番号 = New ExNmText.ExNmTextBox()
        Me.tx_業種区分 = New ExNmText.ExNmTextBox()
        Me.tx_統合見積番号 = New ExNmText.ExNmTextBox()
        Me.rf_統合見積件名 = New System.Windows.Forms.Label()
        Me._lblLabels_58 = New System.Windows.Forms.Label()
        Me._lblLabels_57 = New System.Windows.Forms.Label()
        Me._lblLabels_56 = New System.Windows.Forms.Label()
        Me._lblLabels_55 = New System.Windows.Forms.Label()
        Me._lblLabels_54 = New System.Windows.Forms.Label()
        Me._lb_項目_15 = New System.Windows.Forms.Label()
        Me.rf_集計名 = New System.Windows.Forms.Label()
        Me._lblLabels_53 = New System.Windows.Forms.Label()
        Me._lb_日_8 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me._lb_項目_14 = New System.Windows.Forms.Label()
        Me._lb_項目_13 = New System.Windows.Forms.Label()
        Me._lblLabels_52 = New System.Windows.Forms.Label()
        Me._lb_項目_12 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me._lblLabels_51 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me._lb_日_7 = New System.Windows.Forms.Label()
        Me._lb_月_7 = New System.Windows.Forms.Label()
        Me._lb_年_7 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.rf_工事担当名 = New System.Windows.Forms.Label()
        Me._lblLabels_50 = New System.Windows.Forms.Label()
        Me._lblLabels_49 = New System.Windows.Forms.Label()
        Me._lblLabels_48 = New System.Windows.Forms.Label()
        Me._lblLabels_47 = New System.Windows.Forms.Label()
        Me._lblLabels_1 = New System.Windows.Forms.Label()
        Me.rf_税集計区分名 = New System.Windows.Forms.Label()
        Me._lblLabels_46 = New System.Windows.Forms.Label()
        Me._lblLabels_45 = New System.Windows.Forms.Label()
        Me._lblLabels_44 = New System.Windows.Forms.Label()
        Me._lblLabels_43 = New System.Windows.Forms.Label()
        Me._lb_項目_11 = New System.Windows.Forms.Label()
        Me._lblLabels_42 = New System.Windows.Forms.Label()
        Me._lblLabels_41 = New System.Windows.Forms.Label()
        Me._lblLabels_40 = New System.Windows.Forms.Label()
        Me._lblLabels_39 = New System.Windows.Forms.Label()
        Me._lblLabels_36 = New System.Windows.Forms.Label()
        Me._lblLabels_35 = New System.Windows.Forms.Label()
        Me._lblLabels_38 = New System.Windows.Forms.Label()
        Me._lblLabels_37 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me._lb_日_6 = New System.Windows.Forms.Label()
        Me._lb_月_6 = New System.Windows.Forms.Label()
        Me._lb_年_6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me._lb_日_5 = New System.Windows.Forms.Label()
        Me._lb_月_5 = New System.Windows.Forms.Label()
        Me._lb_年_5 = New System.Windows.Forms.Label()
        Me._lb_項目_10 = New System.Windows.Forms.Label()
        Me.rf_物件略称 = New System.Windows.Forms.Label()
        Me._lb_項目_9 = New System.Windows.Forms.Label()
        Me._lblLabels_31 = New System.Windows.Forms.Label()
        Me._lblLabels_30 = New System.Windows.Forms.Label()
        Me._lblLabels_29 = New System.Windows.Forms.Label()
        Me._lb_項目_7 = New System.Windows.Forms.Label()
        Me._lblLabels_28 = New System.Windows.Forms.Label()
        Me._lblLabels_27 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.rf_部署名 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me._lb_項目_8 = New System.Windows.Forms.Label()
        Me.rf_得意先別見積番号 = New System.Windows.Forms.Label()
        Me.rf_外税額 = New System.Windows.Forms.Label()
        Me.rf_原価率 = New System.Windows.Forms.Label()
        Me.rf_原価合計 = New System.Windows.Forms.Label()
        Me.rf_合計金額 = New System.Windows.Forms.Label()
        Me.rf_売上端数 = New System.Windows.Forms.Label()
        Me.rf_消費税端数 = New System.Windows.Forms.Label()
        Me.rf_税集計区分 = New System.Windows.Forms.Label()
        Me._lblLabels_25 = New System.Windows.Forms.Label()
        Me._lb_年_4 = New System.Windows.Forms.Label()
        Me._lb_月_4 = New System.Windows.Forms.Label()
        Me._lb_日_4 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me._lblLabels_15 = New System.Windows.Forms.Label()
        Me._lblLabels_9 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me._lb_項目_5 = New System.Windows.Forms.Label()
        Me._lblLabels_26 = New System.Windows.Forms.Label()
        Me._lb_日_3 = New System.Windows.Forms.Label()
        Me._lb_月_3 = New System.Windows.Forms.Label()
        Me._lb_年_3 = New System.Windows.Forms.Label()
        Me._lb_月_1 = New System.Windows.Forms.Label()
        Me._lblLabels_24 = New System.Windows.Forms.Label()
        Me._lblLabels_23 = New System.Windows.Forms.Label()
        Me._lblLabels_22 = New System.Windows.Forms.Label()
        Me._lblLabels_21 = New System.Windows.Forms.Label()
        Me._lblLabels_20 = New System.Windows.Forms.Label()
        Me.rf_担当者名 = New System.Windows.Forms.Label()
        Me.lb_担当者CD = New System.Windows.Forms.Label()
        Me._lblLabels_19 = New System.Windows.Forms.Label()
        Me._lblLabels_18 = New System.Windows.Forms.Label()
        Me._lblLabels_17 = New System.Windows.Forms.Label()
        Me._lblLabels_16 = New System.Windows.Forms.Label()
        Me._lblLabels_14 = New System.Windows.Forms.Label()
        Me._lblLabels_13 = New System.Windows.Forms.Label()
        Me._lblLabels_12 = New System.Windows.Forms.Label()
        Me._lblLabels_11 = New System.Windows.Forms.Label()
        Me._lblLabels_8 = New System.Windows.Forms.Label()
        Me._lblLabels_7 = New System.Windows.Forms.Label()
        Me._lblLabels_6 = New System.Windows.Forms.Label()
        Me._lb_日_2 = New System.Windows.Forms.Label()
        Me._lb_月_2 = New System.Windows.Forms.Label()
        Me._lb_年_2 = New System.Windows.Forms.Label()
        Me._lb_kara_0 = New System.Windows.Forms.Label()
        Me._lb_年_0 = New System.Windows.Forms.Label()
        Me._lb_月_0 = New System.Windows.Forms.Label()
        Me._lb_日_0 = New System.Windows.Forms.Label()
        Me._lb_年_1 = New System.Windows.Forms.Label()
        Me._lb_日_1 = New System.Windows.Forms.Label()
        Me._lblLabels_5 = New System.Windows.Forms.Label()
        Me._lblLabels_4 = New System.Windows.Forms.Label()
        Me._lblLabels_3 = New System.Windows.Forms.Label()
        Me._lblLabels_2 = New System.Windows.Forms.Label()
        Me._lblLabels_0 = New System.Windows.Forms.Label()
        Me._lblLabels_10 = New System.Windows.Forms.Label()
        Me.lb_見積番号 = New System.Windows.Forms.Label()
        Me.rf_見積番号 = New System.Windows.Forms.Label()
        Me.lb_見積件名 = New System.Windows.Forms.Label()
        Me._lb_項目_0 = New System.Windows.Forms.Label()
        Me._lb_項目_6 = New System.Windows.Forms.Label()
        Me._lb_項目_2 = New System.Windows.Forms.Label()
        Me._lb_項目_3 = New System.Windows.Forms.Label()
        Me._lb_項目_4 = New System.Windows.Forms.Label()
        Me._lb_項目_1 = New System.Windows.Forms.Label()
        Me.rf_処理区分 = New System.Windows.Forms.Label()
        Me._lb_納期_0 = New System.Windows.Forms.Label()
        Me._lb_納期_1 = New System.Windows.Forms.Label()
        Me.lb_OPEN日 = New System.Windows.Forms.Label()
        Me.lb_見積日付 = New System.Windows.Forms.Label()
        Me.lb_受注日付 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.rf_ウエルシア物件区分名 = New System.Windows.Forms.Label()
        Me._lblLabels_34 = New System.Windows.Forms.Label()
        Me._lblLabels_33 = New System.Windows.Forms.Label()
        Me._lblLabels_32 = New System.Windows.Forms.Label()
        Me._lb_コンテナ_1 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.sb_Msg.SuspendLayout()
        Me.SuspendLayout()
        '
        'ck_社内伝票扱い
        '
        Me.ck_社内伝票扱い.BackColor = System.Drawing.SystemColors.Control
        Me.ck_社内伝票扱い.Cursor = System.Windows.Forms.Cursors.Default
        Me.ck_社内伝票扱い.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ck_社内伝票扱い.Location = New System.Drawing.Point(112, 354)
        Me.ck_社内伝票扱い.Name = "ck_社内伝票扱い"
        Me.ck_社内伝票扱い.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ck_社内伝票扱い.Size = New System.Drawing.Size(126, 17)
        Me.ck_社内伝票扱い.TabIndex = 28
        Me.ck_社内伝票扱い.Text = "社内伝票扱い"
        Me.ck_社内伝票扱い.UseVisualStyleBackColor = False
        '
        'tx_受注日付Y
        '
        Me.tx_受注日付Y.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_受注日付Y.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_受注日付Y.CanForwardSetFocus = True
        Me.tx_受注日付Y.CanNextSetFocus = True
        Me.tx_受注日付Y.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_受注日付Y.EditMode = True
        Me.tx_受注日付Y.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_受注日付Y.Location = New System.Drawing.Point(898, 438)
        Me.tx_受注日付Y.MaxLength = 4
        Me.tx_受注日付Y.Name = "tx_受注日付Y"
        Me.tx_受注日付Y.OldValue = "8888"
        Me.tx_受注日付Y.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_受注日付Y.SelectText = True
        Me.tx_受注日付Y.SelLength = 0
        Me.tx_受注日付Y.SelStart = 0
        Me.tx_受注日付Y.SelText = ""
        Me.tx_受注日付Y.Size = New System.Drawing.Size(32, 16)
        Me.tx_受注日付Y.TabIndex = 74
        Me.tx_受注日付Y.Text = "8888"
        '
        'tx_受注日付M
        '
        Me.tx_受注日付M.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_受注日付M.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_受注日付M.CanForwardSetFocus = True
        Me.tx_受注日付M.CanNextSetFocus = True
        Me.tx_受注日付M.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_受注日付M.EditMode = True
        Me.tx_受注日付M.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_受注日付M.Location = New System.Drawing.Point(946, 437)
        Me.tx_受注日付M.MaxLength = 2
        Me.tx_受注日付M.Name = "tx_受注日付M"
        Me.tx_受注日付M.OldValue = "88"
        Me.tx_受注日付M.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_受注日付M.SelectText = True
        Me.tx_受注日付M.SelLength = 0
        Me.tx_受注日付M.SelStart = 0
        Me.tx_受注日付M.SelText = ""
        Me.tx_受注日付M.Size = New System.Drawing.Size(20, 16)
        Me.tx_受注日付M.TabIndex = 75
        Me.tx_受注日付M.Text = "88"
        '
        'tx_受注日付D
        '
        Me.tx_受注日付D.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_受注日付D.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_受注日付D.CanForwardSetFocus = True
        Me.tx_受注日付D.CanNextSetFocus = True
        Me.tx_受注日付D.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_受注日付D.EditMode = True
        Me.tx_受注日付D.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_受注日付D.Location = New System.Drawing.Point(985, 437)
        Me.tx_受注日付D.MaxLength = 2
        Me.tx_受注日付D.Name = "tx_受注日付D"
        Me.tx_受注日付D.OldValue = "88"
        Me.tx_受注日付D.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_受注日付D.SelectText = True
        Me.tx_受注日付D.SelLength = 0
        Me.tx_受注日付D.SelStart = 0
        Me.tx_受注日付D.SelText = ""
        Me.tx_受注日付D.Size = New System.Drawing.Size(20, 16)
        Me.tx_受注日付D.TabIndex = 76
        Me.tx_受注日付D.Text = "88"
        '
        'PicFunction
        '
        Me.PicFunction.BackColor = System.Drawing.SystemColors.Control
        Me.PicFunction.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PicFunction.Cursor = System.Windows.Forms.Cursors.Default
        Me.PicFunction.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PicFunction.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.PicFunction.ForeColor = System.Drawing.SystemColors.ControlText
        Me.PicFunction.Location = New System.Drawing.Point(0, 644)
        Me.PicFunction.Name = "PicFunction"
        Me.PicFunction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.PicFunction.Size = New System.Drawing.Size(1232, 41)
        Me.PicFunction.TabIndex = 101
        '
        'CbTabEnd
        '
        Me.CbTabEnd.BackColor = System.Drawing.SystemColors.Control
        Me.CbTabEnd.Cursor = System.Windows.Forms.Cursors.Default
        Me.CbTabEnd.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CbTabEnd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CbTabEnd.Location = New System.Drawing.Point(682, 22)
        Me.CbTabEnd.Name = "CbTabEnd"
        Me.CbTabEnd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CbTabEnd.Size = New System.Drawing.Size(41, 9)
        Me.CbTabEnd.TabIndex = 90
        Me.CbTabEnd.UseVisualStyleBackColor = False
        '
        'sb_Msg
        '
        Me.sb_Msg.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.sb_Msg.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.sb_Msg.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.sb_Msg_Panel1, Me.sb_Msg_Panel2, Me.sb_Msg_Panel3})
        Me.sb_Msg.Location = New System.Drawing.Point(0, 685)
        Me.sb_Msg.Name = "sb_Msg"
        Me.sb_Msg.Size = New System.Drawing.Size(1232, 24)
        Me.sb_Msg.TabIndex = 112
        '
        'sb_Msg_Panel1
        '
        Me.sb_Msg_Panel1.AutoSize = False
        Me.sb_Msg_Panel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.sb_Msg_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me.sb_Msg_Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me.sb_Msg_Panel1.Name = "sb_Msg_Panel1"
        Me.sb_Msg_Panel1.Size = New System.Drawing.Size(1072, 24)
        Me.sb_Msg_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
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
        Me.sb_Msg_Panel2.Size = New System.Drawing.Size(96, 24)
        Me.sb_Msg_Panel2.Spring = True
        Me.sb_Msg_Panel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
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
        Me.sb_Msg_Panel3.Size = New System.Drawing.Size(49, 24)
        Me.sb_Msg_Panel3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.sb_Msg_Panel3.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        '
        'tx_得意先名2
        '
        Me.tx_得意先名2.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_得意先名2.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_得意先名2.CanForwardSetFocus = True
        Me.tx_得意先名2.CanNextSetFocus = True
        Me.tx_得意先名2.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_得意先名2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_得意先名2.EditMode = True
        Me.tx_得意先名2.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_得意先名2.IMEMode = System.Windows.Forms.ImeMode.Hiragana
        Me.tx_得意先名2.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_得意先名2.Location = New System.Drawing.Point(420, 172)
        Me.tx_得意先名2.MaxLength = 28
        Me.tx_得意先名2.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_得意先名2.Name = "tx_得意先名2"
        Me.tx_得意先名2.OldValue = "ExTextBox"
        Me.tx_得意先名2.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_得意先名2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_得意先名2.SelectText = True
        Me.tx_得意先名2.SelLength = 0
        Me.tx_得意先名2.SelStart = 0
        Me.tx_得意先名2.SelText = ""
        Me.tx_得意先名2.Size = New System.Drawing.Size(237, 22)
        Me.tx_得意先名2.TabIndex = 11
        Me.tx_得意先名2.Text = "ExTextBox"
        '
        'tx_備考
        '
        Me.tx_備考.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_備考.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_備考.CanForwardSetFocus = True
        Me.tx_備考.CanNextSetFocus = True
        Me.tx_備考.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_備考.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_備考.EditMode = True
        Me.tx_備考.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_備考.IMEMode = System.Windows.Forms.ImeMode.Hiragana
        Me.tx_備考.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_備考.Location = New System.Drawing.Point(112, 460)
        Me.tx_備考.MaxLength = 40
        Me.tx_備考.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_備考.Name = "tx_備考"
        Me.tx_備考.OldValue = "ExTextBox"
        Me.tx_備考.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_備考.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_備考.SelectText = True
        Me.tx_備考.SelLength = 0
        Me.tx_備考.SelStart = 0
        Me.tx_備考.SelText = ""
        Me.tx_備考.Size = New System.Drawing.Size(434, 22)
        Me.tx_備考.TabIndex = 38
        Me.tx_備考.Text = "ExTextBox"
        '
        'tx_得意先名1
        '
        Me.tx_得意先名1.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_得意先名1.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_得意先名1.CanForwardSetFocus = True
        Me.tx_得意先名1.CanNextSetFocus = True
        Me.tx_得意先名1.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_得意先名1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_得意先名1.EditMode = True
        Me.tx_得意先名1.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_得意先名1.IMEMode = System.Windows.Forms.ImeMode.Hiragana
        Me.tx_得意先名1.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_得意先名1.Location = New System.Drawing.Point(180, 172)
        Me.tx_得意先名1.MaxLength = 28
        Me.tx_得意先名1.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_得意先名1.Name = "tx_得意先名1"
        Me.tx_得意先名1.OldValue = "ExTextBox"
        Me.tx_得意先名1.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_得意先名1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_得意先名1.SelectText = True
        Me.tx_得意先名1.SelLength = 0
        Me.tx_得意先名1.SelStart = 0
        Me.tx_得意先名1.SelText = ""
        Me.tx_得意先名1.Size = New System.Drawing.Size(237, 22)
        Me.tx_得意先名1.TabIndex = 10
        Me.tx_得意先名1.Text = "ExTextBox"
        '
        'tx_物件金額
        '
        Me.tx_物件金額.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_物件金額.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_物件金額.CanForwardSetFocus = True
        Me.tx_物件金額.CanNextSetFocus = True
        Me.tx_物件金額.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_物件金額.DecimalPlace = CType(0, Short)
        Me.tx_物件金額.EditMode = True
        Me.tx_物件金額.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_物件金額.FormatType = "#,##0"
        Me.tx_物件金額.FormatTypeNega = ""
        Me.tx_物件金額.FormatTypeNull = ""
        Me.tx_物件金額.FormatTypeZero = ""
        Me.tx_物件金額.InputMinus = False
        Me.tx_物件金額.InputPlus = True
        Me.tx_物件金額.InputZero = False
        Me.tx_物件金額.Location = New System.Drawing.Point(200, 512)
        Me.tx_物件金額.MaxLength = 9
        Me.tx_物件金額.Name = "tx_物件金額"
        Me.tx_物件金額.OldValue = "123456789"
        Me.tx_物件金額.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_物件金額.SelectText = True
        Me.tx_物件金額.SelLength = 0
        Me.tx_物件金額.SelStart = 0
        Me.tx_物件金額.SelText = ""
        Me.tx_物件金額.Size = New System.Drawing.Size(106, 22)
        Me.tx_物件金額.TabIndex = 40
        '
        'tx_出精値引
        '
        Me.tx_出精値引.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_出精値引.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_出精値引.CanForwardSetFocus = True
        Me.tx_出精値引.CanNextSetFocus = True
        Me.tx_出精値引.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_出精値引.DecimalPlace = CType(0, Short)
        Me.tx_出精値引.EditMode = True
        Me.tx_出精値引.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_出精値引.FormatType = "#,##0"
        Me.tx_出精値引.FormatTypeNega = "-#,##0"
        Me.tx_出精値引.FormatTypeNull = ""
        Me.tx_出精値引.FormatTypeZero = ""
        Me.tx_出精値引.InputMinus = True
        Me.tx_出精値引.InputPlus = True
        Me.tx_出精値引.InputZero = False
        Me.tx_出精値引.Location = New System.Drawing.Point(896, 476)
        Me.tx_出精値引.MaxLength = 10
        Me.tx_出精値引.Name = "tx_出精値引"
        Me.tx_出精値引.OldValue = "1234567890"
        Me.tx_出精値引.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_出精値引.SelectText = True
        Me.tx_出精値引.SelLength = 0
        Me.tx_出精値引.SelStart = 0
        Me.tx_出精値引.SelText = ""
        Me.tx_出精値引.Size = New System.Drawing.Size(106, 22)
        Me.tx_出精値引.TabIndex = 78
        Me.tx_出精値引.Visible = False
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
        Me.tx_見積件名.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_見積件名.Location = New System.Drawing.Point(180, 136)
        Me.tx_見積件名.MaxLength = 60
        Me.tx_見積件名.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_見積件名.Name = "tx_見積件名"
        Me.tx_見積件名.OldValue = "ExTextBox"
        Me.tx_見積件名.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_見積件名.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_見積件名.SelectText = True
        Me.tx_見積件名.SelLength = 0
        Me.tx_見積件名.SelStart = 0
        Me.tx_見積件名.SelText = ""
        Me.tx_見積件名.Size = New System.Drawing.Size(430, 22)
        Me.tx_見積件名.TabIndex = 8
        Me.tx_見積件名.Text = "ExTextBox"
        '
        'tx_得TEL
        '
        Me.tx_得TEL.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_得TEL.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_得TEL.CanForwardSetFocus = True
        Me.tx_得TEL.CanNextSetFocus = True
        Me.tx_得TEL.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_得TEL.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_得TEL.EditMode = True
        Me.tx_得TEL.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_得TEL.IMEMode = System.Windows.Forms.ImeMode.Disable
        Me.tx_得TEL.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_得TEL.Location = New System.Drawing.Point(180, 192)
        Me.tx_得TEL.MaxLength = 15
        Me.tx_得TEL.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_得TEL.Name = "tx_得TEL"
        Me.tx_得TEL.OldValue = "ExTextBox"
        Me.tx_得TEL.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_得TEL.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_得TEL.SelectText = True
        Me.tx_得TEL.SelLength = 0
        Me.tx_得TEL.SelStart = 0
        Me.tx_得TEL.SelText = ""
        Me.tx_得TEL.Size = New System.Drawing.Size(126, 22)
        Me.tx_得TEL.TabIndex = 12
        Me.tx_得TEL.Text = "ExTextBox"
        '
        'tx_得FAX
        '
        Me.tx_得FAX.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_得FAX.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_得FAX.CanForwardSetFocus = True
        Me.tx_得FAX.CanNextSetFocus = True
        Me.tx_得FAX.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_得FAX.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_得FAX.EditMode = True
        Me.tx_得FAX.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_得FAX.IMEMode = System.Windows.Forms.ImeMode.Disable
        Me.tx_得FAX.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_得FAX.Location = New System.Drawing.Point(363, 192)
        Me.tx_得FAX.MaxLength = 15
        Me.tx_得FAX.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_得FAX.Name = "tx_得FAX"
        Me.tx_得FAX.OldValue = "ExTextBox"
        Me.tx_得FAX.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_得FAX.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_得FAX.SelectText = True
        Me.tx_得FAX.SelLength = 0
        Me.tx_得FAX.SelStart = 0
        Me.tx_得FAX.SelText = ""
        Me.tx_得FAX.Size = New System.Drawing.Size(126, 22)
        Me.tx_得FAX.TabIndex = 13
        Me.tx_得FAX.Text = "ExTextBox"
        '
        'tx_得担当者
        '
        Me.tx_得担当者.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_得担当者.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_得担当者.CanForwardSetFocus = True
        Me.tx_得担当者.CanNextSetFocus = True
        Me.tx_得担当者.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_得担当者.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_得担当者.EditMode = True
        Me.tx_得担当者.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_得担当者.IMEMode = System.Windows.Forms.ImeMode.Hiragana
        Me.tx_得担当者.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_得担当者.Location = New System.Drawing.Point(180, 212)
        Me.tx_得担当者.MaxLength = 40
        Me.tx_得担当者.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_得担当者.Name = "tx_得担当者"
        Me.tx_得担当者.OldValue = "ExTextBox"
        Me.tx_得担当者.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_得担当者.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_得担当者.SelectText = True
        Me.tx_得担当者.SelLength = 0
        Me.tx_得担当者.SelStart = 0
        Me.tx_得担当者.SelText = ""
        Me.tx_得担当者.Size = New System.Drawing.Size(285, 22)
        Me.tx_得担当者.TabIndex = 14
        Me.tx_得担当者.Text = "ExTextBox"
        '
        'tx_納入先CD
        '
        Me.tx_納入先CD.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_納入先CD.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_納入先CD.CanForwardSetFocus = True
        Me.tx_納入先CD.CanNextSetFocus = True
        Me.tx_納入先CD.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_納入先CD.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_納入先CD.EditMode = True
        Me.tx_納入先CD.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_納入先CD.IMEMode = System.Windows.Forms.ImeMode.Disable
        Me.tx_納入先CD.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_納入先CD.Location = New System.Drawing.Point(180, 264)
        Me.tx_納入先CD.MaxLength = 4
        Me.tx_納入先CD.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_納入先CD.Name = "tx_納入先CD"
        Me.tx_納入先CD.OldValue = "ExTextBox"
        Me.tx_納入先CD.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_納入先CD.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_納入先CD.SelectText = True
        Me.tx_納入先CD.SelLength = 0
        Me.tx_納入先CD.SelStart = 0
        Me.tx_納入先CD.SelText = ""
        Me.tx_納入先CD.Size = New System.Drawing.Size(39, 22)
        Me.tx_納入先CD.TabIndex = 19
        Me.tx_納入先CD.Text = "ExTextBox"
        '
        'tx_納得意先CD
        '
        Me.tx_納得意先CD.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_納得意先CD.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_納得意先CD.CanForwardSetFocus = True
        Me.tx_納得意先CD.CanNextSetFocus = True
        Me.tx_納得意先CD.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_納得意先CD.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_納得意先CD.EditMode = True
        Me.tx_納得意先CD.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_納得意先CD.IMEMode = System.Windows.Forms.ImeMode.Disable
        Me.tx_納得意先CD.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_納得意先CD.Location = New System.Drawing.Point(112, 264)
        Me.tx_納得意先CD.MaxLength = 4
        Me.tx_納得意先CD.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_納得意先CD.Name = "tx_納得意先CD"
        Me.tx_納得意先CD.OldValue = "ExTextBox"
        Me.tx_納得意先CD.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_納得意先CD.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_納得意先CD.SelectText = True
        Me.tx_納得意先CD.SelLength = 0
        Me.tx_納得意先CD.SelStart = 0
        Me.tx_納得意先CD.SelText = ""
        Me.tx_納得意先CD.Size = New System.Drawing.Size(65, 22)
        Me.tx_納得意先CD.TabIndex = 18
        Me.tx_納得意先CD.Text = "ExTextBox"
        '
        'tx_納入先名2
        '
        Me.tx_納入先名2.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_納入先名2.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_納入先名2.CanForwardSetFocus = True
        Me.tx_納入先名2.CanNextSetFocus = True
        Me.tx_納入先名2.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_納入先名2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_納入先名2.EditMode = True
        Me.tx_納入先名2.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_納入先名2.IMEMode = System.Windows.Forms.ImeMode.Hiragana
        Me.tx_納入先名2.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_納入先名2.Location = New System.Drawing.Point(460, 264)
        Me.tx_納入先名2.MaxLength = 28
        Me.tx_納入先名2.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_納入先名2.Name = "tx_納入先名2"
        Me.tx_納入先名2.OldValue = "ExTextBox"
        Me.tx_納入先名2.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_納入先名2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_納入先名2.SelectText = True
        Me.tx_納入先名2.SelLength = 0
        Me.tx_納入先名2.SelStart = 0
        Me.tx_納入先名2.SelText = ""
        Me.tx_納入先名2.Size = New System.Drawing.Size(237, 22)
        Me.tx_納入先名2.TabIndex = 21
        Me.tx_納入先名2.Text = "ExTextBox"
        '
        'tx_納入先名1
        '
        Me.tx_納入先名1.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_納入先名1.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_納入先名1.CanForwardSetFocus = True
        Me.tx_納入先名1.CanNextSetFocus = True
        Me.tx_納入先名1.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_納入先名1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_納入先名1.EditMode = True
        Me.tx_納入先名1.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_納入先名1.IMEMode = System.Windows.Forms.ImeMode.Hiragana
        Me.tx_納入先名1.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_納入先名1.Location = New System.Drawing.Point(220, 264)
        Me.tx_納入先名1.MaxLength = 28
        Me.tx_納入先名1.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_納入先名1.Name = "tx_納入先名1"
        Me.tx_納入先名1.OldValue = "ExTextBox"
        Me.tx_納入先名1.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_納入先名1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_納入先名1.SelectText = True
        Me.tx_納入先名1.SelLength = 0
        Me.tx_納入先名1.SelStart = 0
        Me.tx_納入先名1.SelText = ""
        Me.tx_納入先名1.Size = New System.Drawing.Size(237, 22)
        Me.tx_納入先名1.TabIndex = 20
        Me.tx_納入先名1.Text = "ExTextBox"
        '
        'tx_郵便番号
        '
        Me.tx_郵便番号.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_郵便番号.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_郵便番号.CanForwardSetFocus = True
        Me.tx_郵便番号.CanNextSetFocus = True
        Me.tx_郵便番号.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_郵便番号.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_郵便番号.EditMode = True
        Me.tx_郵便番号.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_郵便番号.IMEMode = System.Windows.Forms.ImeMode.Disable
        Me.tx_郵便番号.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_郵便番号.Location = New System.Drawing.Point(156, 284)
        Me.tx_郵便番号.MaxLength = 8
        Me.tx_郵便番号.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_郵便番号.Name = "tx_郵便番号"
        Me.tx_郵便番号.OldValue = "ExTextBox"
        Me.tx_郵便番号.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_郵便番号.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_郵便番号.SelectText = True
        Me.tx_郵便番号.SelLength = 0
        Me.tx_郵便番号.SelStart = 0
        Me.tx_郵便番号.SelText = ""
        Me.tx_郵便番号.Size = New System.Drawing.Size(63, 22)
        Me.tx_郵便番号.TabIndex = 22
        Me.tx_郵便番号.Text = "ExTextBox"
        '
        'tx_納住所1
        '
        Me.tx_納住所1.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_納住所1.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_納住所1.CanForwardSetFocus = True
        Me.tx_納住所1.CanNextSetFocus = True
        Me.tx_納住所1.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_納住所1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_納住所1.EditMode = True
        Me.tx_納住所1.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_納住所1.IMEMode = System.Windows.Forms.ImeMode.Hiragana
        Me.tx_納住所1.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_納住所1.Location = New System.Drawing.Point(220, 284)
        Me.tx_納住所1.MaxLength = 32
        Me.tx_納住所1.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_納住所1.Name = "tx_納住所1"
        Me.tx_納住所1.OldValue = "ExTextBox"
        Me.tx_納住所1.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_納住所1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_納住所1.SelectText = True
        Me.tx_納住所1.SelLength = 0
        Me.tx_納住所1.SelStart = 0
        Me.tx_納住所1.SelText = ""
        Me.tx_納住所1.Size = New System.Drawing.Size(237, 22)
        Me.tx_納住所1.TabIndex = 23
        Me.tx_納住所1.Text = "ExTextBox"
        '
        'tx_納住所2
        '
        Me.tx_納住所2.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_納住所2.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_納住所2.CanForwardSetFocus = True
        Me.tx_納住所2.CanNextSetFocus = True
        Me.tx_納住所2.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_納住所2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_納住所2.EditMode = True
        Me.tx_納住所2.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_納住所2.IMEMode = System.Windows.Forms.ImeMode.Hiragana
        Me.tx_納住所2.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_納住所2.Location = New System.Drawing.Point(460, 284)
        Me.tx_納住所2.MaxLength = 32
        Me.tx_納住所2.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_納住所2.Name = "tx_納住所2"
        Me.tx_納住所2.OldValue = "ExTextBox"
        Me.tx_納住所2.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_納住所2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_納住所2.SelectText = True
        Me.tx_納住所2.SelLength = 0
        Me.tx_納住所2.SelStart = 0
        Me.tx_納住所2.SelText = ""
        Me.tx_納住所2.Size = New System.Drawing.Size(237, 22)
        Me.tx_納住所2.TabIndex = 24
        Me.tx_納住所2.Text = "ExTextBox"
        '
        'tx_納TEL
        '
        Me.tx_納TEL.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_納TEL.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_納TEL.CanForwardSetFocus = True
        Me.tx_納TEL.CanNextSetFocus = True
        Me.tx_納TEL.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_納TEL.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_納TEL.EditMode = True
        Me.tx_納TEL.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_納TEL.IMEMode = System.Windows.Forms.ImeMode.Disable
        Me.tx_納TEL.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_納TEL.Location = New System.Drawing.Point(220, 304)
        Me.tx_納TEL.MaxLength = 15
        Me.tx_納TEL.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_納TEL.Name = "tx_納TEL"
        Me.tx_納TEL.OldValue = "ExTextBox"
        Me.tx_納TEL.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_納TEL.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_納TEL.SelectText = True
        Me.tx_納TEL.SelLength = 0
        Me.tx_納TEL.SelStart = 0
        Me.tx_納TEL.SelText = ""
        Me.tx_納TEL.Size = New System.Drawing.Size(126, 22)
        Me.tx_納TEL.TabIndex = 25
        Me.tx_納TEL.Text = "ExTextBox"
        '
        'tx_納FAX
        '
        Me.tx_納FAX.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_納FAX.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_納FAX.CanForwardSetFocus = True
        Me.tx_納FAX.CanNextSetFocus = True
        Me.tx_納FAX.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_納FAX.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_納FAX.EditMode = True
        Me.tx_納FAX.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_納FAX.IMEMode = System.Windows.Forms.ImeMode.Disable
        Me.tx_納FAX.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_納FAX.Location = New System.Drawing.Point(407, 304)
        Me.tx_納FAX.MaxLength = 15
        Me.tx_納FAX.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_納FAX.Name = "tx_納FAX"
        Me.tx_納FAX.OldValue = "ExTextBox"
        Me.tx_納FAX.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_納FAX.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_納FAX.SelectText = True
        Me.tx_納FAX.SelLength = 0
        Me.tx_納FAX.SelStart = 0
        Me.tx_納FAX.SelText = ""
        Me.tx_納FAX.Size = New System.Drawing.Size(126, 22)
        Me.tx_納FAX.TabIndex = 26
        Me.tx_納FAX.Text = "ExTextBox"
        '
        'tx_納担当者
        '
        Me.tx_納担当者.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_納担当者.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_納担当者.CanForwardSetFocus = True
        Me.tx_納担当者.CanNextSetFocus = True
        Me.tx_納担当者.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_納担当者.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_納担当者.EditMode = True
        Me.tx_納担当者.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_納担当者.IMEMode = System.Windows.Forms.ImeMode.Hiragana
        Me.tx_納担当者.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_納担当者.Location = New System.Drawing.Point(220, 324)
        Me.tx_納担当者.MaxLength = 40
        Me.tx_納担当者.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_納担当者.Name = "tx_納担当者"
        Me.tx_納担当者.OldValue = "ExTextBox"
        Me.tx_納担当者.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_納担当者.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_納担当者.SelectText = True
        Me.tx_納担当者.SelLength = 0
        Me.tx_納担当者.SelStart = 0
        Me.tx_納担当者.SelText = ""
        Me.tx_納担当者.Size = New System.Drawing.Size(285, 22)
        Me.tx_納担当者.TabIndex = 27
        Me.tx_納担当者.Text = "ExTextBox"
        '
        'tx_s納期Y
        '
        Me.tx_s納期Y.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_s納期Y.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_s納期Y.CanForwardSetFocus = True
        Me.tx_s納期Y.CanNextSetFocus = True
        Me.tx_s納期Y.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_s納期Y.EditMode = True
        Me.tx_s納期Y.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_s納期Y.Location = New System.Drawing.Point(113, 431)
        Me.tx_s納期Y.MaxLength = 4
        Me.tx_s納期Y.Name = "tx_s納期Y"
        Me.tx_s納期Y.OldValue = "YYYY"
        Me.tx_s納期Y.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_s納期Y.SelectText = True
        Me.tx_s納期Y.SelLength = 0
        Me.tx_s納期Y.SelStart = 0
        Me.tx_s納期Y.SelText = ""
        Me.tx_s納期Y.Size = New System.Drawing.Size(32, 16)
        Me.tx_s納期Y.TabIndex = 32
        Me.tx_s納期Y.Text = "YYYY"
        '
        'tx_s納期M
        '
        Me.tx_s納期M.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_s納期M.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_s納期M.CanForwardSetFocus = True
        Me.tx_s納期M.CanNextSetFocus = True
        Me.tx_s納期M.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_s納期M.EditMode = True
        Me.tx_s納期M.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_s納期M.Location = New System.Drawing.Point(166, 431)
        Me.tx_s納期M.MaxLength = 2
        Me.tx_s納期M.Name = "tx_s納期M"
        Me.tx_s納期M.OldValue = "MM"
        Me.tx_s納期M.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_s納期M.SelectText = True
        Me.tx_s納期M.SelLength = 0
        Me.tx_s納期M.SelStart = 0
        Me.tx_s納期M.SelText = ""
        Me.tx_s納期M.Size = New System.Drawing.Size(20, 16)
        Me.tx_s納期M.TabIndex = 33
        Me.tx_s納期M.Text = "MM"
        '
        'tx_s納期D
        '
        Me.tx_s納期D.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_s納期D.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_s納期D.CanForwardSetFocus = True
        Me.tx_s納期D.CanNextSetFocus = True
        Me.tx_s納期D.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_s納期D.EditMode = True
        Me.tx_s納期D.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_s納期D.Location = New System.Drawing.Point(203, 431)
        Me.tx_s納期D.MaxLength = 2
        Me.tx_s納期D.Name = "tx_s納期D"
        Me.tx_s納期D.OldValue = "DD"
        Me.tx_s納期D.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_s納期D.SelectText = True
        Me.tx_s納期D.SelLength = 0
        Me.tx_s納期D.SelStart = 0
        Me.tx_s納期D.SelText = ""
        Me.tx_s納期D.Size = New System.Drawing.Size(20, 16)
        Me.tx_s納期D.TabIndex = 34
        Me.tx_s納期D.Text = "DD"
        '
        'tx_e納期Y
        '
        Me.tx_e納期Y.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_e納期Y.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_e納期Y.CanForwardSetFocus = True
        Me.tx_e納期Y.CanNextSetFocus = True
        Me.tx_e納期Y.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_e納期Y.EditMode = True
        Me.tx_e納期Y.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_e納期Y.Location = New System.Drawing.Point(266, 431)
        Me.tx_e納期Y.MaxLength = 4
        Me.tx_e納期Y.Name = "tx_e納期Y"
        Me.tx_e納期Y.OldValue = "YYYY"
        Me.tx_e納期Y.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_e納期Y.SelectText = True
        Me.tx_e納期Y.SelLength = 0
        Me.tx_e納期Y.SelStart = 0
        Me.tx_e納期Y.SelText = ""
        Me.tx_e納期Y.Size = New System.Drawing.Size(32, 16)
        Me.tx_e納期Y.TabIndex = 35
        Me.tx_e納期Y.Text = "YYYY"
        '
        'tx_e納期M
        '
        Me.tx_e納期M.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_e納期M.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_e納期M.CanForwardSetFocus = True
        Me.tx_e納期M.CanNextSetFocus = True
        Me.tx_e納期M.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_e納期M.EditMode = True
        Me.tx_e納期M.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_e納期M.Location = New System.Drawing.Point(316, 431)
        Me.tx_e納期M.MaxLength = 2
        Me.tx_e納期M.Name = "tx_e納期M"
        Me.tx_e納期M.OldValue = "MM"
        Me.tx_e納期M.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_e納期M.SelectText = True
        Me.tx_e納期M.SelLength = 0
        Me.tx_e納期M.SelStart = 0
        Me.tx_e納期M.SelText = ""
        Me.tx_e納期M.Size = New System.Drawing.Size(20, 16)
        Me.tx_e納期M.TabIndex = 36
        Me.tx_e納期M.Text = "MM"
        '
        'tx_e納期D
        '
        Me.tx_e納期D.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_e納期D.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_e納期D.CanForwardSetFocus = True
        Me.tx_e納期D.CanNextSetFocus = True
        Me.tx_e納期D.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_e納期D.EditMode = True
        Me.tx_e納期D.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_e納期D.Location = New System.Drawing.Point(354, 431)
        Me.tx_e納期D.MaxLength = 2
        Me.tx_e納期D.Name = "tx_e納期D"
        Me.tx_e納期D.OldValue = "DD"
        Me.tx_e納期D.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_e納期D.SelectText = True
        Me.tx_e納期D.SelLength = 0
        Me.tx_e納期D.SelStart = 0
        Me.tx_e納期D.SelText = ""
        Me.tx_e納期D.Size = New System.Drawing.Size(20, 16)
        Me.tx_e納期D.TabIndex = 37
        Me.tx_e納期D.Text = "DD"
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
        Me.tx_得意先CD.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_得意先CD.Location = New System.Drawing.Point(112, 172)
        Me.tx_得意先CD.MaxLength = 4
        Me.tx_得意先CD.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_得意先CD.Name = "tx_得意先CD"
        Me.tx_得意先CD.OldValue = "ExTextBox"
        Me.tx_得意先CD.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_得意先CD.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_得意先CD.SelectText = True
        Me.tx_得意先CD.SelLength = 0
        Me.tx_得意先CD.SelStart = 0
        Me.tx_得意先CD.SelText = ""
        Me.tx_得意先CD.Size = New System.Drawing.Size(65, 22)
        Me.tx_得意先CD.TabIndex = 9
        Me.tx_得意先CD.Text = "ExTextBox"
        '
        'tx_OPEN日Y
        '
        Me.tx_OPEN日Y.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_OPEN日Y.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_OPEN日Y.CanForwardSetFocus = True
        Me.tx_OPEN日Y.CanNextSetFocus = True
        Me.tx_OPEN日Y.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_OPEN日Y.EditMode = True
        Me.tx_OPEN日Y.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_OPEN日Y.Location = New System.Drawing.Point(449, 513)
        Me.tx_OPEN日Y.MaxLength = 4
        Me.tx_OPEN日Y.Name = "tx_OPEN日Y"
        Me.tx_OPEN日Y.OldValue = "YYYY"
        Me.tx_OPEN日Y.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_OPEN日Y.SelectText = True
        Me.tx_OPEN日Y.SelLength = 0
        Me.tx_OPEN日Y.SelStart = 0
        Me.tx_OPEN日Y.SelText = ""
        Me.tx_OPEN日Y.Size = New System.Drawing.Size(32, 16)
        Me.tx_OPEN日Y.TabIndex = 41
        Me.tx_OPEN日Y.Text = "YYYY"
        '
        'tx_OPEN日M
        '
        Me.tx_OPEN日M.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_OPEN日M.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_OPEN日M.CanForwardSetFocus = True
        Me.tx_OPEN日M.CanNextSetFocus = True
        Me.tx_OPEN日M.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_OPEN日M.EditMode = True
        Me.tx_OPEN日M.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_OPEN日M.Location = New System.Drawing.Point(503, 513)
        Me.tx_OPEN日M.MaxLength = 2
        Me.tx_OPEN日M.Name = "tx_OPEN日M"
        Me.tx_OPEN日M.OldValue = "MM"
        Me.tx_OPEN日M.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_OPEN日M.SelectText = True
        Me.tx_OPEN日M.SelLength = 0
        Me.tx_OPEN日M.SelStart = 0
        Me.tx_OPEN日M.SelText = ""
        Me.tx_OPEN日M.Size = New System.Drawing.Size(20, 16)
        Me.tx_OPEN日M.TabIndex = 42
        Me.tx_OPEN日M.Text = "MM"
        '
        'tx_OPEN日D
        '
        Me.tx_OPEN日D.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_OPEN日D.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_OPEN日D.CanForwardSetFocus = True
        Me.tx_OPEN日D.CanNextSetFocus = True
        Me.tx_OPEN日D.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_OPEN日D.EditMode = True
        Me.tx_OPEN日D.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_OPEN日D.Location = New System.Drawing.Point(539, 513)
        Me.tx_OPEN日D.MaxLength = 2
        Me.tx_OPEN日D.Name = "tx_OPEN日D"
        Me.tx_OPEN日D.OldValue = "DD"
        Me.tx_OPEN日D.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_OPEN日D.SelectText = True
        Me.tx_OPEN日D.SelLength = 0
        Me.tx_OPEN日D.SelStart = 0
        Me.tx_OPEN日D.SelText = ""
        Me.tx_OPEN日D.Size = New System.Drawing.Size(20, 16)
        Me.tx_OPEN日D.TabIndex = 43
        Me.tx_OPEN日D.Text = "DD"
        '
        'tx_物件種別
        '
        Me.tx_物件種別.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_物件種別.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_物件種別.CanForwardSetFocus = True
        Me.tx_物件種別.CanNextSetFocus = True
        Me.tx_物件種別.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_物件種別.DecimalPlace = CType(0, Short)
        Me.tx_物件種別.EditMode = True
        Me.tx_物件種別.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_物件種別.FormatType = ""
        Me.tx_物件種別.FormatTypeNega = ""
        Me.tx_物件種別.FormatTypeNull = ""
        Me.tx_物件種別.FormatTypeZero = ""
        Me.tx_物件種別.InputMinus = False
        Me.tx_物件種別.InputPlus = True
        Me.tx_物件種別.InputZero = True
        Me.tx_物件種別.Location = New System.Drawing.Point(200, 532)
        Me.tx_物件種別.MaxLength = 1
        Me.tx_物件種別.Name = "tx_物件種別"
        Me.tx_物件種別.OldValue = "1"
        Me.tx_物件種別.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_物件種別.SelectText = True
        Me.tx_物件種別.SelLength = 0
        Me.tx_物件種別.SelStart = 0
        Me.tx_物件種別.SelText = ""
        Me.tx_物件種別.Size = New System.Drawing.Size(26, 22)
        Me.tx_物件種別.TabIndex = 44
        Me.tx_物件種別.Text = "1"
        '
        'tx_納期表示
        '
        Me.tx_納期表示.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_納期表示.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_納期表示.CanForwardSetFocus = True
        Me.tx_納期表示.CanNextSetFocus = True
        Me.tx_納期表示.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_納期表示.DecimalPlace = CType(0, Short)
        Me.tx_納期表示.EditMode = True
        Me.tx_納期表示.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_納期表示.FormatType = ""
        Me.tx_納期表示.FormatTypeNega = ""
        Me.tx_納期表示.FormatTypeNull = ""
        Me.tx_納期表示.FormatTypeZero = ""
        Me.tx_納期表示.InputMinus = False
        Me.tx_納期表示.InputPlus = True
        Me.tx_納期表示.InputZero = True
        Me.tx_納期表示.Location = New System.Drawing.Point(896, 356)
        Me.tx_納期表示.MaxLength = 1
        Me.tx_納期表示.Name = "tx_納期表示"
        Me.tx_納期表示.OldValue = "1"
        Me.tx_納期表示.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_納期表示.SelectText = True
        Me.tx_納期表示.SelLength = 0
        Me.tx_納期表示.SelStart = 0
        Me.tx_納期表示.SelText = ""
        Me.tx_納期表示.Size = New System.Drawing.Size(26, 22)
        Me.tx_納期表示.TabIndex = 69
        '
        'tx_納期表示他
        '
        Me.tx_納期表示他.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_納期表示他.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_納期表示他.CanForwardSetFocus = True
        Me.tx_納期表示他.CanNextSetFocus = True
        Me.tx_納期表示他.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_納期表示他.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_納期表示他.EditMode = True
        Me.tx_納期表示他.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_納期表示他.IMEMode = System.Windows.Forms.ImeMode.Hiragana
        Me.tx_納期表示他.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_納期表示他.Location = New System.Drawing.Point(924, 376)
        Me.tx_納期表示他.MaxLength = 30
        Me.tx_納期表示他.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_納期表示他.Name = "tx_納期表示他"
        Me.tx_納期表示他.OldValue = "ExTextBox"
        Me.tx_納期表示他.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_納期表示他.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_納期表示他.SelectText = True
        Me.tx_納期表示他.SelLength = 0
        Me.tx_納期表示他.SelStart = 0
        Me.tx_納期表示他.SelText = ""
        Me.tx_納期表示他.Size = New System.Drawing.Size(214, 22)
        Me.tx_納期表示他.TabIndex = 70
        Me.tx_納期表示他.Text = "ExTextBox"
        '
        'tx_出力日
        '
        Me.tx_出力日.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_出力日.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_出力日.CanForwardSetFocus = True
        Me.tx_出力日.CanNextSetFocus = True
        Me.tx_出力日.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_出力日.DecimalPlace = CType(0, Short)
        Me.tx_出力日.EditMode = True
        Me.tx_出力日.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_出力日.FormatType = ""
        Me.tx_出力日.FormatTypeNega = ""
        Me.tx_出力日.FormatTypeNull = ""
        Me.tx_出力日.FormatTypeZero = ""
        Me.tx_出力日.InputMinus = False
        Me.tx_出力日.InputPlus = True
        Me.tx_出力日.InputZero = True
        Me.tx_出力日.Location = New System.Drawing.Point(896, 396)
        Me.tx_出力日.MaxLength = 1
        Me.tx_出力日.Name = "tx_出力日"
        Me.tx_出力日.OldValue = "1"
        Me.tx_出力日.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_出力日.SelectText = True
        Me.tx_出力日.SelLength = 0
        Me.tx_出力日.SelStart = 0
        Me.tx_出力日.SelText = ""
        Me.tx_出力日.Size = New System.Drawing.Size(26, 22)
        Me.tx_出力日.TabIndex = 71
        '
        'tx_有効期限
        '
        Me.tx_有効期限.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_有効期限.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_有効期限.CanForwardSetFocus = True
        Me.tx_有効期限.CanNextSetFocus = True
        Me.tx_有効期限.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_有効期限.DecimalPlace = CType(0, Short)
        Me.tx_有効期限.EditMode = True
        Me.tx_有効期限.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_有効期限.FormatType = ""
        Me.tx_有効期限.FormatTypeNega = ""
        Me.tx_有効期限.FormatTypeNull = ""
        Me.tx_有効期限.FormatTypeZero = ""
        Me.tx_有効期限.InputMinus = True
        Me.tx_有効期限.InputPlus = True
        Me.tx_有効期限.InputZero = False
        Me.tx_有効期限.Location = New System.Drawing.Point(1168, 396)
        Me.tx_有効期限.MaxLength = 2
        Me.tx_有効期限.Name = "tx_有効期限"
        Me.tx_有効期限.OldValue = "1"
        Me.tx_有効期限.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_有効期限.SelectText = True
        Me.tx_有効期限.SelLength = 0
        Me.tx_有効期限.SelStart = 0
        Me.tx_有効期限.SelText = ""
        Me.tx_有効期限.Size = New System.Drawing.Size(26, 22)
        Me.tx_有効期限.TabIndex = 72
        '
        'tx_受注区分
        '
        Me.tx_受注区分.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_受注区分.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_受注区分.CanForwardSetFocus = True
        Me.tx_受注区分.CanNextSetFocus = True
        Me.tx_受注区分.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_受注区分.DecimalPlace = CType(0, Short)
        Me.tx_受注区分.EditMode = True
        Me.tx_受注区分.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_受注区分.FormatType = ""
        Me.tx_受注区分.FormatTypeNega = ""
        Me.tx_受注区分.FormatTypeNull = ""
        Me.tx_受注区分.FormatTypeZero = ""
        Me.tx_受注区分.InputMinus = False
        Me.tx_受注区分.InputPlus = True
        Me.tx_受注区分.InputZero = True
        Me.tx_受注区分.Location = New System.Drawing.Point(896, 416)
        Me.tx_受注区分.MaxLength = 1
        Me.tx_受注区分.Name = "tx_受注区分"
        Me.tx_受注区分.OldValue = "1"
        Me.tx_受注区分.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_受注区分.SelectText = True
        Me.tx_受注区分.SelLength = 0
        Me.tx_受注区分.SelStart = 0
        Me.tx_受注区分.SelText = ""
        Me.tx_受注区分.Size = New System.Drawing.Size(26, 22)
        Me.tx_受注区分.TabIndex = 73
        '
        'tx_大小口区分
        '
        Me.tx_大小口区分.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_大小口区分.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_大小口区分.CanForwardSetFocus = True
        Me.tx_大小口区分.CanNextSetFocus = True
        Me.tx_大小口区分.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_大小口区分.DecimalPlace = CType(0, Short)
        Me.tx_大小口区分.EditMode = True
        Me.tx_大小口区分.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_大小口区分.FormatType = ""
        Me.tx_大小口区分.FormatTypeNega = ""
        Me.tx_大小口区分.FormatTypeNull = ""
        Me.tx_大小口区分.FormatTypeZero = ""
        Me.tx_大小口区分.InputMinus = False
        Me.tx_大小口区分.InputPlus = True
        Me.tx_大小口区分.InputZero = True
        Me.tx_大小口区分.Location = New System.Drawing.Point(896, 456)
        Me.tx_大小口区分.MaxLength = 1
        Me.tx_大小口区分.Name = "tx_大小口区分"
        Me.tx_大小口区分.OldValue = "1"
        Me.tx_大小口区分.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_大小口区分.SelectText = True
        Me.tx_大小口区分.SelLength = 0
        Me.tx_大小口区分.SelStart = 0
        Me.tx_大小口区分.SelText = ""
        Me.tx_大小口区分.Size = New System.Drawing.Size(26, 22)
        Me.tx_大小口区分.TabIndex = 77
        '
        'tx_見積日付Y
        '
        Me.tx_見積日付Y.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_見積日付Y.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_見積日付Y.CanForwardSetFocus = True
        Me.tx_見積日付Y.CanNextSetFocus = True
        Me.tx_見積日付Y.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_見積日付Y.EditMode = True
        Me.tx_見積日付Y.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_見積日付Y.Location = New System.Drawing.Point(181, 118)
        Me.tx_見積日付Y.MaxLength = 4
        Me.tx_見積日付Y.Name = "tx_見積日付Y"
        Me.tx_見積日付Y.OldValue = "YYYY"
        Me.tx_見積日付Y.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_見積日付Y.SelectText = True
        Me.tx_見積日付Y.SelLength = 0
        Me.tx_見積日付Y.SelStart = 0
        Me.tx_見積日付Y.SelText = ""
        Me.tx_見積日付Y.Size = New System.Drawing.Size(32, 16)
        Me.tx_見積日付Y.TabIndex = 4
        Me.tx_見積日付Y.Text = "YYYY"
        '
        'tx_見積日付M
        '
        Me.tx_見積日付M.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_見積日付M.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_見積日付M.CanForwardSetFocus = True
        Me.tx_見積日付M.CanNextSetFocus = True
        Me.tx_見積日付M.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_見積日付M.EditMode = True
        Me.tx_見積日付M.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_見積日付M.Location = New System.Drawing.Point(230, 118)
        Me.tx_見積日付M.MaxLength = 2
        Me.tx_見積日付M.Name = "tx_見積日付M"
        Me.tx_見積日付M.OldValue = "MM"
        Me.tx_見積日付M.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_見積日付M.SelectText = True
        Me.tx_見積日付M.SelLength = 0
        Me.tx_見積日付M.SelStart = 0
        Me.tx_見積日付M.SelText = ""
        Me.tx_見積日付M.Size = New System.Drawing.Size(20, 16)
        Me.tx_見積日付M.TabIndex = 5
        Me.tx_見積日付M.Text = "MM"
        '
        'tx_見積日付D
        '
        Me.tx_見積日付D.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_見積日付D.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_見積日付D.CanForwardSetFocus = True
        Me.tx_見積日付D.CanNextSetFocus = True
        Me.tx_見積日付D.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_見積日付D.EditMode = True
        Me.tx_見積日付D.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_見積日付D.Location = New System.Drawing.Point(269, 118)
        Me.tx_見積日付D.MaxLength = 2
        Me.tx_見積日付D.Name = "tx_見積日付D"
        Me.tx_見積日付D.OldValue = "DD"
        Me.tx_見積日付D.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_見積日付D.SelectText = True
        Me.tx_見積日付D.SelLength = 0
        Me.tx_見積日付D.SelStart = 0
        Me.tx_見積日付D.SelText = ""
        Me.tx_見積日付D.Size = New System.Drawing.Size(20, 16)
        Me.tx_見積日付D.TabIndex = 6
        Me.tx_見積日付D.Text = "DD"
        '
        'tx_現場名
        '
        Me.tx_現場名.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_現場名.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_現場名.CanForwardSetFocus = True
        Me.tx_現場名.CanNextSetFocus = True
        Me.tx_現場名.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_現場名.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_現場名.EditMode = True
        Me.tx_現場名.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_現場名.IMEMode = System.Windows.Forms.ImeMode.Hiragana
        Me.tx_現場名.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_現場名.Location = New System.Drawing.Point(896, 296)
        Me.tx_現場名.MaxLength = 40
        Me.tx_現場名.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_現場名.Name = "tx_現場名"
        Me.tx_現場名.OldValue = "ExTextBox"
        Me.tx_現場名.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_現場名.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_現場名.SelectText = True
        Me.tx_現場名.SelLength = 0
        Me.tx_現場名.SelStart = 0
        Me.tx_現場名.SelText = ""
        Me.tx_現場名.Size = New System.Drawing.Size(290, 22)
        Me.tx_現場名.TabIndex = 66
        Me.tx_現場名.Text = "ExTextBox"
        '
        'tx_支払条件
        '
        Me.tx_支払条件.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_支払条件.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_支払条件.CanForwardSetFocus = True
        Me.tx_支払条件.CanNextSetFocus = True
        Me.tx_支払条件.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_支払条件.DecimalPlace = CType(0, Short)
        Me.tx_支払条件.EditMode = True
        Me.tx_支払条件.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_支払条件.FormatType = ""
        Me.tx_支払条件.FormatTypeNega = ""
        Me.tx_支払条件.FormatTypeNull = ""
        Me.tx_支払条件.FormatTypeZero = ""
        Me.tx_支払条件.InputMinus = False
        Me.tx_支払条件.InputPlus = True
        Me.tx_支払条件.InputZero = True
        Me.tx_支払条件.Location = New System.Drawing.Point(896, 316)
        Me.tx_支払条件.MaxLength = 1
        Me.tx_支払条件.Name = "tx_支払条件"
        Me.tx_支払条件.OldValue = "1"
        Me.tx_支払条件.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_支払条件.SelectText = True
        Me.tx_支払条件.SelLength = 0
        Me.tx_支払条件.SelStart = 0
        Me.tx_支払条件.SelText = ""
        Me.tx_支払条件.Size = New System.Drawing.Size(26, 22)
        Me.tx_支払条件.TabIndex = 67
        '
        'tx_支払条件他
        '
        Me.tx_支払条件他.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_支払条件他.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_支払条件他.CanForwardSetFocus = True
        Me.tx_支払条件他.CanNextSetFocus = True
        Me.tx_支払条件他.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_支払条件他.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_支払条件他.EditMode = True
        Me.tx_支払条件他.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_支払条件他.IMEMode = System.Windows.Forms.ImeMode.Hiragana
        Me.tx_支払条件他.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_支払条件他.Location = New System.Drawing.Point(924, 336)
        Me.tx_支払条件他.MaxLength = 30
        Me.tx_支払条件他.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_支払条件他.Name = "tx_支払条件他"
        Me.tx_支払条件他.OldValue = "ExTextBox"
        Me.tx_支払条件他.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_支払条件他.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_支払条件他.SelectText = True
        Me.tx_支払条件他.SelLength = 0
        Me.tx_支払条件他.SelStart = 0
        Me.tx_支払条件他.SelText = ""
        Me.tx_支払条件他.Size = New System.Drawing.Size(214, 22)
        Me.tx_支払条件他.TabIndex = 68
        Me.tx_支払条件他.Text = "ExTextBox"
        '
        'tx_担当者CD
        '
        Me.tx_担当者CD.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_担当者CD.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_担当者CD.CanForwardSetFocus = True
        Me.tx_担当者CD.CanNextSetFocus = True
        Me.tx_担当者CD.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_担当者CD.DecimalPlace = CType(0, Short)
        Me.tx_担当者CD.EditMode = False
        Me.tx_担当者CD.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_担当者CD.FormatType = ""
        Me.tx_担当者CD.FormatTypeNega = ""
        Me.tx_担当者CD.FormatTypeNull = ""
        Me.tx_担当者CD.FormatTypeZero = ""
        Me.tx_担当者CD.InputMinus = False
        Me.tx_担当者CD.InputPlus = True
        Me.tx_担当者CD.InputZero = False
        Me.tx_担当者CD.Location = New System.Drawing.Point(180, 96)
        Me.tx_担当者CD.MaxLength = 3
        Me.tx_担当者CD.Name = "tx_担当者CD"
        Me.tx_担当者CD.OldValue = "1"
        Me.tx_担当者CD.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_担当者CD.SelectText = True
        Me.tx_担当者CD.SelLength = 0
        Me.tx_担当者CD.SelStart = 0
        Me.tx_担当者CD.SelText = ""
        Me.tx_担当者CD.Size = New System.Drawing.Size(54, 22)
        Me.tx_担当者CD.TabIndex = 2
        '
        'rf_販売先得意先名2
        '
        Me.rf_販売先得意先名2.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.rf_販売先得意先名2.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.rf_販売先得意先名2.CanForwardSetFocus = True
        Me.rf_販売先得意先名2.CanNextSetFocus = True
        Me.rf_販売先得意先名2.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.rf_販売先得意先名2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.rf_販売先得意先名2.EditMode = True
        Me.rf_販売先得意先名2.Enabled = False
        Me.rf_販売先得意先名2.FocusBackColor = System.Drawing.SystemColors.Window
        Me.rf_販売先得意先名2.IMEMode = System.Windows.Forms.ImeMode.Hiragana
        Me.rf_販売先得意先名2.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.rf_販売先得意先名2.Location = New System.Drawing.Point(420, 372)
        Me.rf_販売先得意先名2.MaxLength = 28
        Me.rf_販売先得意先名2.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.rf_販売先得意先名2.Name = "rf_販売先得意先名2"
        Me.rf_販売先得意先名2.OldValue = "ExTextBox"
        Me.rf_販売先得意先名2.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.rf_販売先得意先名2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_販売先得意先名2.SelectText = True
        Me.rf_販売先得意先名2.SelLength = 0
        Me.rf_販売先得意先名2.SelStart = 0
        Me.rf_販売先得意先名2.SelText = ""
        Me.rf_販売先得意先名2.Size = New System.Drawing.Size(237, 22)
        Me.rf_販売先得意先名2.TabIndex = 187
        Me.rf_販売先得意先名2.Text = "ExTextBox"
        '
        'rf_販売先得意先名1
        '
        Me.rf_販売先得意先名1.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.rf_販売先得意先名1.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.rf_販売先得意先名1.CanForwardSetFocus = True
        Me.rf_販売先得意先名1.CanNextSetFocus = True
        Me.rf_販売先得意先名1.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.rf_販売先得意先名1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.rf_販売先得意先名1.EditMode = True
        Me.rf_販売先得意先名1.Enabled = False
        Me.rf_販売先得意先名1.FocusBackColor = System.Drawing.SystemColors.Window
        Me.rf_販売先得意先名1.IMEMode = System.Windows.Forms.ImeMode.Hiragana
        Me.rf_販売先得意先名1.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.rf_販売先得意先名1.Location = New System.Drawing.Point(180, 372)
        Me.rf_販売先得意先名1.MaxLength = 28
        Me.rf_販売先得意先名1.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.rf_販売先得意先名1.Name = "rf_販売先得意先名1"
        Me.rf_販売先得意先名1.OldValue = "ExTextBox"
        Me.rf_販売先得意先名1.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.rf_販売先得意先名1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_販売先得意先名1.SelectText = True
        Me.rf_販売先得意先名1.SelLength = 0
        Me.rf_販売先得意先名1.SelStart = 0
        Me.rf_販売先得意先名1.SelText = ""
        Me.rf_販売先得意先名1.Size = New System.Drawing.Size(237, 22)
        Me.rf_販売先得意先名1.TabIndex = 188
        Me.rf_販売先得意先名1.Text = "ExTextBox"
        '
        'tx_販売先得意先CD
        '
        Me.tx_販売先得意先CD.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_販売先得意先CD.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_販売先得意先CD.CanForwardSetFocus = True
        Me.tx_販売先得意先CD.CanNextSetFocus = True
        Me.tx_販売先得意先CD.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_販売先得意先CD.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_販売先得意先CD.EditMode = True
        Me.tx_販売先得意先CD.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_販売先得意先CD.IMEMode = System.Windows.Forms.ImeMode.Disable
        Me.tx_販売先得意先CD.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_販売先得意先CD.Location = New System.Drawing.Point(112, 372)
        Me.tx_販売先得意先CD.MaxLength = 4
        Me.tx_販売先得意先CD.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_販売先得意先CD.Name = "tx_販売先得意先CD"
        Me.tx_販売先得意先CD.OldValue = "ExTextBox"
        Me.tx_販売先得意先CD.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_販売先得意先CD.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_販売先得意先CD.SelectText = True
        Me.tx_販売先得意先CD.SelLength = 0
        Me.tx_販売先得意先CD.SelStart = 0
        Me.tx_販売先得意先CD.SelText = ""
        Me.tx_販売先得意先CD.Size = New System.Drawing.Size(65, 22)
        Me.tx_販売先得意先CD.TabIndex = 29
        Me.tx_販売先得意先CD.Text = "ExTextBox"
        '
        'tx_販売先納入先CD
        '
        Me.tx_販売先納入先CD.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_販売先納入先CD.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_販売先納入先CD.CanForwardSetFocus = True
        Me.tx_販売先納入先CD.CanNextSetFocus = True
        Me.tx_販売先納入先CD.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_販売先納入先CD.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_販売先納入先CD.EditMode = True
        Me.tx_販売先納入先CD.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_販売先納入先CD.IMEMode = System.Windows.Forms.ImeMode.Disable
        Me.tx_販売先納入先CD.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_販売先納入先CD.Location = New System.Drawing.Point(180, 396)
        Me.tx_販売先納入先CD.MaxLength = 4
        Me.tx_販売先納入先CD.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_販売先納入先CD.Name = "tx_販売先納入先CD"
        Me.tx_販売先納入先CD.OldValue = "ExTextBox"
        Me.tx_販売先納入先CD.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_販売先納入先CD.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_販売先納入先CD.SelectText = True
        Me.tx_販売先納入先CD.SelLength = 0
        Me.tx_販売先納入先CD.SelStart = 0
        Me.tx_販売先納入先CD.SelText = ""
        Me.tx_販売先納入先CD.Size = New System.Drawing.Size(39, 22)
        Me.tx_販売先納入先CD.TabIndex = 31
        Me.tx_販売先納入先CD.Text = "ExTextBox"
        '
        'tx_販売先納得意先CD
        '
        Me.tx_販売先納得意先CD.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_販売先納得意先CD.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_販売先納得意先CD.CanForwardSetFocus = True
        Me.tx_販売先納得意先CD.CanNextSetFocus = True
        Me.tx_販売先納得意先CD.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_販売先納得意先CD.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_販売先納得意先CD.EditMode = True
        Me.tx_販売先納得意先CD.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_販売先納得意先CD.IMEMode = System.Windows.Forms.ImeMode.Disable
        Me.tx_販売先納得意先CD.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_販売先納得意先CD.Location = New System.Drawing.Point(112, 396)
        Me.tx_販売先納得意先CD.MaxLength = 4
        Me.tx_販売先納得意先CD.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_販売先納得意先CD.Name = "tx_販売先納得意先CD"
        Me.tx_販売先納得意先CD.OldValue = "ExTextBox"
        Me.tx_販売先納得意先CD.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_販売先納得意先CD.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_販売先納得意先CD.SelectText = True
        Me.tx_販売先納得意先CD.SelLength = 0
        Me.tx_販売先納得意先CD.SelStart = 0
        Me.tx_販売先納得意先CD.SelText = ""
        Me.tx_販売先納得意先CD.Size = New System.Drawing.Size(65, 22)
        Me.tx_販売先納得意先CD.TabIndex = 30
        Me.tx_販売先納得意先CD.Text = "ExTextBox"
        '
        'rf_販売先納入先名2
        '
        Me.rf_販売先納入先名2.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.rf_販売先納入先名2.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.rf_販売先納入先名2.CanForwardSetFocus = True
        Me.rf_販売先納入先名2.CanNextSetFocus = True
        Me.rf_販売先納入先名2.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.rf_販売先納入先名2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.rf_販売先納入先名2.EditMode = True
        Me.rf_販売先納入先名2.Enabled = False
        Me.rf_販売先納入先名2.FocusBackColor = System.Drawing.SystemColors.Window
        Me.rf_販売先納入先名2.IMEMode = System.Windows.Forms.ImeMode.Hiragana
        Me.rf_販売先納入先名2.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.rf_販売先納入先名2.Location = New System.Drawing.Point(460, 396)
        Me.rf_販売先納入先名2.MaxLength = 28
        Me.rf_販売先納入先名2.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.rf_販売先納入先名2.Name = "rf_販売先納入先名2"
        Me.rf_販売先納入先名2.OldValue = "ExTextBox"
        Me.rf_販売先納入先名2.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.rf_販売先納入先名2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_販売先納入先名2.SelectText = True
        Me.rf_販売先納入先名2.SelLength = 0
        Me.rf_販売先納入先名2.SelStart = 0
        Me.rf_販売先納入先名2.SelText = ""
        Me.rf_販売先納入先名2.Size = New System.Drawing.Size(237, 22)
        Me.rf_販売先納入先名2.TabIndex = 189
        Me.rf_販売先納入先名2.Text = "ExTextBox"
        '
        'rf_販売先納入先名1
        '
        Me.rf_販売先納入先名1.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.rf_販売先納入先名1.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.rf_販売先納入先名1.CanForwardSetFocus = True
        Me.rf_販売先納入先名1.CanNextSetFocus = True
        Me.rf_販売先納入先名1.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.rf_販売先納入先名1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.rf_販売先納入先名1.EditMode = True
        Me.rf_販売先納入先名1.Enabled = False
        Me.rf_販売先納入先名1.FocusBackColor = System.Drawing.SystemColors.Window
        Me.rf_販売先納入先名1.IMEMode = System.Windows.Forms.ImeMode.Hiragana
        Me.rf_販売先納入先名1.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.rf_販売先納入先名1.Location = New System.Drawing.Point(220, 396)
        Me.rf_販売先納入先名1.MaxLength = 28
        Me.rf_販売先納入先名1.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.rf_販売先納入先名1.Name = "rf_販売先納入先名1"
        Me.rf_販売先納入先名1.OldValue = "ExTextBox"
        Me.rf_販売先納入先名1.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.rf_販売先納入先名1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_販売先納入先名1.SelectText = True
        Me.rf_販売先納入先名1.SelLength = 0
        Me.rf_販売先納入先名1.SelStart = 0
        Me.rf_販売先納入先名1.SelText = ""
        Me.rf_販売先納入先名1.Size = New System.Drawing.Size(237, 22)
        Me.rf_販売先納入先名1.TabIndex = 190
        Me.rf_販売先納入先名1.Text = "ExTextBox"
        '
        'tx_部署CD
        '
        Me.tx_部署CD.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_部署CD.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_部署CD.CanForwardSetFocus = True
        Me.tx_部署CD.CanNextSetFocus = True
        Me.tx_部署CD.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_部署CD.DecimalPlace = CType(0, Short)
        Me.tx_部署CD.EditMode = True
        Me.tx_部署CD.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_部署CD.FormatType = ""
        Me.tx_部署CD.FormatTypeNega = ""
        Me.tx_部署CD.FormatTypeNull = ""
        Me.tx_部署CD.FormatTypeZero = ""
        Me.tx_部署CD.InputMinus = False
        Me.tx_部署CD.InputPlus = True
        Me.tx_部署CD.InputZero = False
        Me.tx_部署CD.Location = New System.Drawing.Point(424, 96)
        Me.tx_部署CD.MaxLength = 4
        Me.tx_部署CD.Name = "tx_部署CD"
        Me.tx_部署CD.OldValue = "ExNmTextBox"
        Me.tx_部署CD.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_部署CD.SelectText = True
        Me.tx_部署CD.SelLength = 0
        Me.tx_部署CD.SelStart = 0
        Me.tx_部署CD.SelText = ""
        Me.tx_部署CD.Size = New System.Drawing.Size(63, 22)
        Me.tx_部署CD.TabIndex = 3
        '
        'tx_物件番号
        '
        Me.tx_物件番号.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_物件番号.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_物件番号.CanForwardSetFocus = True
        Me.tx_物件番号.CanNextSetFocus = True
        Me.tx_物件番号.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_物件番号.DecimalPlace = CType(0, Short)
        Me.tx_物件番号.EditMode = True
        Me.tx_物件番号.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_物件番号.FormatType = ""
        Me.tx_物件番号.FormatTypeNega = ""
        Me.tx_物件番号.FormatTypeNull = ""
        Me.tx_物件番号.FormatTypeZero = ""
        Me.tx_物件番号.InputMinus = False
        Me.tx_物件番号.InputPlus = True
        Me.tx_物件番号.InputZero = False
        Me.tx_物件番号.Location = New System.Drawing.Point(180, 48)
        Me.tx_物件番号.MaxLength = 7
        Me.tx_物件番号.Name = "tx_物件番号"
        Me.tx_物件番号.OldValue = "ExNmTextBox"
        Me.tx_物件番号.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_物件番号.SelectText = True
        Me.tx_物件番号.SelLength = 0
        Me.tx_物件番号.SelStart = 0
        Me.tx_物件番号.SelText = ""
        Me.tx_物件番号.Size = New System.Drawing.Size(65, 22)
        Me.tx_物件番号.TabIndex = 0
        '
        'tx_伝票種類
        '
        Me.tx_伝票種類.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_伝票種類.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_伝票種類.CanForwardSetFocus = True
        Me.tx_伝票種類.CanNextSetFocus = True
        Me.tx_伝票種類.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_伝票種類.DecimalPlace = CType(0, Short)
        Me.tx_伝票種類.EditMode = True
        Me.tx_伝票種類.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_伝票種類.FormatType = ""
        Me.tx_伝票種類.FormatTypeNega = ""
        Me.tx_伝票種類.FormatTypeNull = ""
        Me.tx_伝票種類.FormatTypeZero = ""
        Me.tx_伝票種類.InputMinus = False
        Me.tx_伝票種類.InputPlus = True
        Me.tx_伝票種類.InputZero = True
        Me.tx_伝票種類.Location = New System.Drawing.Point(900, 48)
        Me.tx_伝票種類.MaxLength = 1
        Me.tx_伝票種類.Name = "tx_伝票種類"
        Me.tx_伝票種類.OldValue = "1"
        Me.tx_伝票種類.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_伝票種類.SelectText = True
        Me.tx_伝票種類.SelLength = 0
        Me.tx_伝票種類.SelStart = 0
        Me.tx_伝票種類.SelText = ""
        Me.tx_伝票種類.Size = New System.Drawing.Size(26, 22)
        Me.tx_伝票種類.TabIndex = 54
        '
        'tx_ウエルシアリース区分
        '
        Me.tx_ウエルシアリース区分.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_ウエルシアリース区分.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_ウエルシアリース区分.CanForwardSetFocus = True
        Me.tx_ウエルシアリース区分.CanNextSetFocus = True
        Me.tx_ウエルシアリース区分.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_ウエルシアリース区分.DecimalPlace = CType(0, Short)
        Me.tx_ウエルシアリース区分.EditMode = True
        Me.tx_ウエルシアリース区分.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_ウエルシアリース区分.FormatType = ""
        Me.tx_ウエルシアリース区分.FormatTypeNega = ""
        Me.tx_ウエルシアリース区分.FormatTypeNull = ""
        Me.tx_ウエルシアリース区分.FormatTypeZero = ""
        Me.tx_ウエルシアリース区分.InputMinus = False
        Me.tx_ウエルシアリース区分.InputPlus = True
        Me.tx_ウエルシアリース区分.InputZero = False
        Me.tx_ウエルシアリース区分.Location = New System.Drawing.Point(900, 100)
        Me.tx_ウエルシアリース区分.MaxLength = 1
        Me.tx_ウエルシアリース区分.Name = "tx_ウエルシアリース区分"
        Me.tx_ウエルシアリース区分.OldValue = ""
        Me.tx_ウエルシアリース区分.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_ウエルシアリース区分.SelectText = True
        Me.tx_ウエルシアリース区分.SelLength = 0
        Me.tx_ウエルシアリース区分.SelStart = 0
        Me.tx_ウエルシアリース区分.SelText = ""
        Me.tx_ウエルシアリース区分.Size = New System.Drawing.Size(26, 22)
        Me.tx_ウエルシアリース区分.TabIndex = 56
        '
        'tx_ウエルシア物件区分
        '
        Me.tx_ウエルシア物件区分.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_ウエルシア物件区分.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_ウエルシア物件区分.CanForwardSetFocus = True
        Me.tx_ウエルシア物件区分.CanNextSetFocus = True
        Me.tx_ウエルシア物件区分.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_ウエルシア物件区分.DecimalPlace = CType(0, Short)
        Me.tx_ウエルシア物件区分.EditMode = True
        Me.tx_ウエルシア物件区分.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_ウエルシア物件区分.FormatType = ""
        Me.tx_ウエルシア物件区分.FormatTypeNega = ""
        Me.tx_ウエルシア物件区分.FormatTypeNull = ""
        Me.tx_ウエルシア物件区分.FormatTypeZero = ""
        Me.tx_ウエルシア物件区分.InputMinus = False
        Me.tx_ウエルシア物件区分.InputPlus = True
        Me.tx_ウエルシア物件区分.InputZero = False
        Me.tx_ウエルシア物件区分.Location = New System.Drawing.Point(900, 120)
        Me.tx_ウエルシア物件区分.MaxLength = 2
        Me.tx_ウエルシア物件区分.Name = "tx_ウエルシア物件区分"
        Me.tx_ウエルシア物件区分.OldValue = ""
        Me.tx_ウエルシア物件区分.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_ウエルシア物件区分.SelectText = True
        Me.tx_ウエルシア物件区分.SelLength = 0
        Me.tx_ウエルシア物件区分.SelStart = 0
        Me.tx_ウエルシア物件区分.SelText = ""
        Me.tx_ウエルシア物件区分.Size = New System.Drawing.Size(26, 22)
        Me.tx_ウエルシア物件区分.TabIndex = 57
        '
        'tx_ウエルシア物件内容CD
        '
        Me.tx_ウエルシア物件内容CD.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_ウエルシア物件内容CD.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_ウエルシア物件内容CD.CanForwardSetFocus = True
        Me.tx_ウエルシア物件内容CD.CanNextSetFocus = True
        Me.tx_ウエルシア物件内容CD.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_ウエルシア物件内容CD.DecimalPlace = CType(0, Short)
        Me.tx_ウエルシア物件内容CD.EditMode = True
        Me.tx_ウエルシア物件内容CD.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_ウエルシア物件内容CD.FormatType = ""
        Me.tx_ウエルシア物件内容CD.FormatTypeNega = ""
        Me.tx_ウエルシア物件内容CD.FormatTypeNull = ""
        Me.tx_ウエルシア物件内容CD.FormatTypeZero = ""
        Me.tx_ウエルシア物件内容CD.InputMinus = False
        Me.tx_ウエルシア物件内容CD.InputPlus = True
        Me.tx_ウエルシア物件内容CD.InputZero = False
        Me.tx_ウエルシア物件内容CD.Location = New System.Drawing.Point(900, 140)
        Me.tx_ウエルシア物件内容CD.MaxLength = 3
        Me.tx_ウエルシア物件内容CD.Name = "tx_ウエルシア物件内容CD"
        Me.tx_ウエルシア物件内容CD.OldValue = "1"
        Me.tx_ウエルシア物件内容CD.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_ウエルシア物件内容CD.SelectText = True
        Me.tx_ウエルシア物件内容CD.SelLength = 0
        Me.tx_ウエルシア物件内容CD.SelStart = 0
        Me.tx_ウエルシア物件内容CD.SelText = ""
        Me.tx_ウエルシア物件内容CD.Size = New System.Drawing.Size(34, 22)
        Me.tx_ウエルシア物件内容CD.TabIndex = 58
        '
        'tx_ウエルシア物件内容名
        '
        Me.tx_ウエルシア物件内容名.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_ウエルシア物件内容名.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_ウエルシア物件内容名.CanForwardSetFocus = True
        Me.tx_ウエルシア物件内容名.CanNextSetFocus = True
        Me.tx_ウエルシア物件内容名.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_ウエルシア物件内容名.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_ウエルシア物件内容名.EditMode = True
        Me.tx_ウエルシア物件内容名.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_ウエルシア物件内容名.IMEMode = System.Windows.Forms.ImeMode.Hiragana
        Me.tx_ウエルシア物件内容名.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_ウエルシア物件内容名.Location = New System.Drawing.Point(936, 140)
        Me.tx_ウエルシア物件内容名.MaxLength = 40
        Me.tx_ウエルシア物件内容名.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_ウエルシア物件内容名.Name = "tx_ウエルシア物件内容名"
        Me.tx_ウエルシア物件内容名.OldValue = "ExTextBox"
        Me.tx_ウエルシア物件内容名.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_ウエルシア物件内容名.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_ウエルシア物件内容名.SelectText = True
        Me.tx_ウエルシア物件内容名.SelLength = 0
        Me.tx_ウエルシア物件内容名.SelStart = 0
        Me.tx_ウエルシア物件内容名.SelText = ""
        Me.tx_ウエルシア物件内容名.Size = New System.Drawing.Size(285, 22)
        Me.tx_ウエルシア物件内容名.TabIndex = 59
        Me.tx_ウエルシア物件内容名.Text = "ExTextBox"
        '
        'tx_ウエルシア売場面積
        '
        Me.tx_ウエルシア売場面積.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_ウエルシア売場面積.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_ウエルシア売場面積.CanForwardSetFocus = True
        Me.tx_ウエルシア売場面積.CanNextSetFocus = True
        Me.tx_ウエルシア売場面積.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_ウエルシア売場面積.DecimalPlace = CType(0, Short)
        Me.tx_ウエルシア売場面積.EditMode = True
        Me.tx_ウエルシア売場面積.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_ウエルシア売場面積.FormatType = "#,###"
        Me.tx_ウエルシア売場面積.FormatTypeNega = ""
        Me.tx_ウエルシア売場面積.FormatTypeNull = ""
        Me.tx_ウエルシア売場面積.FormatTypeZero = ""
        Me.tx_ウエルシア売場面積.InputMinus = False
        Me.tx_ウエルシア売場面積.InputPlus = True
        Me.tx_ウエルシア売場面積.InputZero = False
        Me.tx_ウエルシア売場面積.Location = New System.Drawing.Point(900, 160)
        Me.tx_ウエルシア売場面積.MaxLength = 9
        Me.tx_ウエルシア売場面積.Name = "tx_ウエルシア売場面積"
        Me.tx_ウエルシア売場面積.OldValue = "123456789"
        Me.tx_ウエルシア売場面積.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_ウエルシア売場面積.SelectText = True
        Me.tx_ウエルシア売場面積.SelLength = 0
        Me.tx_ウエルシア売場面積.SelStart = 0
        Me.tx_ウエルシア売場面積.SelText = ""
        Me.tx_ウエルシア売場面積.Size = New System.Drawing.Size(106, 22)
        Me.tx_ウエルシア売場面積.TabIndex = 60
        '
        'tx_受付日付Y
        '
        Me.tx_受付日付Y.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_受付日付Y.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_受付日付Y.CanForwardSetFocus = True
        Me.tx_受付日付Y.CanNextSetFocus = True
        Me.tx_受付日付Y.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_受付日付Y.EditMode = True
        Me.tx_受付日付Y.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_受付日付Y.Location = New System.Drawing.Point(201, 553)
        Me.tx_受付日付Y.MaxLength = 4
        Me.tx_受付日付Y.Name = "tx_受付日付Y"
        Me.tx_受付日付Y.OldValue = "YYYY"
        Me.tx_受付日付Y.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_受付日付Y.SelectText = True
        Me.tx_受付日付Y.SelLength = 0
        Me.tx_受付日付Y.SelStart = 0
        Me.tx_受付日付Y.SelText = ""
        Me.tx_受付日付Y.Size = New System.Drawing.Size(32, 16)
        Me.tx_受付日付Y.TabIndex = 45
        Me.tx_受付日付Y.Text = "YYYY"
        '
        'tx_受付日付M
        '
        Me.tx_受付日付M.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_受付日付M.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_受付日付M.CanForwardSetFocus = True
        Me.tx_受付日付M.CanNextSetFocus = True
        Me.tx_受付日付M.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_受付日付M.EditMode = True
        Me.tx_受付日付M.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_受付日付M.Location = New System.Drawing.Point(250, 553)
        Me.tx_受付日付M.MaxLength = 2
        Me.tx_受付日付M.Name = "tx_受付日付M"
        Me.tx_受付日付M.OldValue = "MM"
        Me.tx_受付日付M.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_受付日付M.SelectText = True
        Me.tx_受付日付M.SelLength = 0
        Me.tx_受付日付M.SelStart = 0
        Me.tx_受付日付M.SelText = ""
        Me.tx_受付日付M.Size = New System.Drawing.Size(20, 16)
        Me.tx_受付日付M.TabIndex = 46
        Me.tx_受付日付M.Text = "MM"
        '
        'tx_受付日付D
        '
        Me.tx_受付日付D.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_受付日付D.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_受付日付D.CanForwardSetFocus = True
        Me.tx_受付日付D.CanNextSetFocus = True
        Me.tx_受付日付D.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_受付日付D.EditMode = True
        Me.tx_受付日付D.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_受付日付D.Location = New System.Drawing.Point(289, 553)
        Me.tx_受付日付D.MaxLength = 2
        Me.tx_受付日付D.Name = "tx_受付日付D"
        Me.tx_受付日付D.OldValue = "DD"
        Me.tx_受付日付D.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_受付日付D.SelectText = True
        Me.tx_受付日付D.SelLength = 0
        Me.tx_受付日付D.SelStart = 0
        Me.tx_受付日付D.SelText = ""
        Me.tx_受付日付D.Size = New System.Drawing.Size(20, 16)
        Me.tx_受付日付D.TabIndex = 47
        Me.tx_受付日付D.Text = "DD"
        '
        'tx_完工日付Y
        '
        Me.tx_完工日付Y.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_完工日付Y.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_完工日付Y.CanForwardSetFocus = True
        Me.tx_完工日付Y.CanNextSetFocus = True
        Me.tx_完工日付Y.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_完工日付Y.EditMode = True
        Me.tx_完工日付Y.Enabled = False
        Me.tx_完工日付Y.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_完工日付Y.Location = New System.Drawing.Point(201, 573)
        Me.tx_完工日付Y.MaxLength = 4
        Me.tx_完工日付Y.Name = "tx_完工日付Y"
        Me.tx_完工日付Y.OldValue = "YYYY"
        Me.tx_完工日付Y.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_完工日付Y.SelectText = True
        Me.tx_完工日付Y.SelLength = 0
        Me.tx_完工日付Y.SelStart = 0
        Me.tx_完工日付Y.SelText = ""
        Me.tx_完工日付Y.Size = New System.Drawing.Size(32, 16)
        Me.tx_完工日付Y.TabIndex = 48
        Me.tx_完工日付Y.Text = "YYYY"
        Me.tx_完工日付Y.Visible = False
        '
        'tx_完工日付M
        '
        Me.tx_完工日付M.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_完工日付M.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_完工日付M.CanForwardSetFocus = True
        Me.tx_完工日付M.CanNextSetFocus = True
        Me.tx_完工日付M.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_完工日付M.EditMode = True
        Me.tx_完工日付M.Enabled = False
        Me.tx_完工日付M.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_完工日付M.Location = New System.Drawing.Point(250, 573)
        Me.tx_完工日付M.MaxLength = 2
        Me.tx_完工日付M.Name = "tx_完工日付M"
        Me.tx_完工日付M.OldValue = "MM"
        Me.tx_完工日付M.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_完工日付M.SelectText = True
        Me.tx_完工日付M.SelLength = 0
        Me.tx_完工日付M.SelStart = 0
        Me.tx_完工日付M.SelText = ""
        Me.tx_完工日付M.Size = New System.Drawing.Size(20, 16)
        Me.tx_完工日付M.TabIndex = 49
        Me.tx_完工日付M.Text = "MM"
        Me.tx_完工日付M.Visible = False
        '
        'tx_完工日付D
        '
        Me.tx_完工日付D.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_完工日付D.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_完工日付D.CanForwardSetFocus = True
        Me.tx_完工日付D.CanNextSetFocus = True
        Me.tx_完工日付D.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_完工日付D.EditMode = True
        Me.tx_完工日付D.Enabled = False
        Me.tx_完工日付D.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_完工日付D.Location = New System.Drawing.Point(289, 573)
        Me.tx_完工日付D.MaxLength = 2
        Me.tx_完工日付D.Name = "tx_完工日付D"
        Me.tx_完工日付D.OldValue = "DD"
        Me.tx_完工日付D.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_完工日付D.SelectText = True
        Me.tx_完工日付D.SelLength = 0
        Me.tx_完工日付D.SelStart = 0
        Me.tx_完工日付D.SelText = ""
        Me.tx_完工日付D.Size = New System.Drawing.Size(20, 16)
        Me.tx_完工日付D.TabIndex = 50
        Me.tx_完工日付D.Text = "DD"
        Me.tx_完工日付D.Visible = False
        '
        'tx_発注担当者名
        '
        Me.tx_発注担当者名.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_発注担当者名.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_発注担当者名.CanForwardSetFocus = True
        Me.tx_発注担当者名.CanNextSetFocus = True
        Me.tx_発注担当者名.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_発注担当者名.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_発注担当者名.EditMode = True
        Me.tx_発注担当者名.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_発注担当者名.IMEMode = System.Windows.Forms.ImeMode.Hiragana
        Me.tx_発注担当者名.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_発注担当者名.Location = New System.Drawing.Point(200, 592)
        Me.tx_発注担当者名.MaxLength = 40
        Me.tx_発注担当者名.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_発注担当者名.Name = "tx_発注担当者名"
        Me.tx_発注担当者名.OldValue = "ExTextBox"
        Me.tx_発注担当者名.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_発注担当者名.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_発注担当者名.SelectText = True
        Me.tx_発注担当者名.SelLength = 0
        Me.tx_発注担当者名.SelStart = 0
        Me.tx_発注担当者名.SelText = ""
        Me.tx_発注担当者名.Size = New System.Drawing.Size(290, 22)
        Me.tx_発注担当者名.TabIndex = 51
        Me.tx_発注担当者名.Text = "ExTextBox"
        '
        'tx_作業内容
        '
        Me.tx_作業内容.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_作業内容.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_作業内容.CanForwardSetFocus = True
        Me.tx_作業内容.CanNextSetFocus = True
        Me.tx_作業内容.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_作業内容.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_作業内容.EditMode = True
        Me.tx_作業内容.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_作業内容.IMEMode = System.Windows.Forms.ImeMode.Hiragana
        Me.tx_作業内容.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_作業内容.Location = New System.Drawing.Point(200, 612)
        Me.tx_作業内容.MaxLength = 40
        Me.tx_作業内容.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_作業内容.Name = "tx_作業内容"
        Me.tx_作業内容.OldValue = "ExTextBox"
        Me.tx_作業内容.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_作業内容.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_作業内容.SelectText = True
        Me.tx_作業内容.SelLength = 0
        Me.tx_作業内容.SelStart = 0
        Me.tx_作業内容.SelText = ""
        Me.tx_作業内容.Size = New System.Drawing.Size(290, 22)
        Me.tx_作業内容.TabIndex = 52
        Me.tx_作業内容.Text = "ExTextBox"
        '
        'tx_YKサプライ区分
        '
        Me.tx_YKサプライ区分.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_YKサプライ区分.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_YKサプライ区分.CanForwardSetFocus = True
        Me.tx_YKサプライ区分.CanNextSetFocus = True
        Me.tx_YKサプライ区分.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_YKサプライ区分.DecimalPlace = CType(0, Short)
        Me.tx_YKサプライ区分.EditMode = True
        Me.tx_YKサプライ区分.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_YKサプライ区分.FormatType = ""
        Me.tx_YKサプライ区分.FormatTypeNega = ""
        Me.tx_YKサプライ区分.FormatTypeNull = ""
        Me.tx_YKサプライ区分.FormatTypeZero = ""
        Me.tx_YKサプライ区分.InputMinus = False
        Me.tx_YKサプライ区分.InputPlus = True
        Me.tx_YKサプライ区分.InputZero = False
        Me.tx_YKサプライ区分.Location = New System.Drawing.Point(896, 192)
        Me.tx_YKサプライ区分.MaxLength = 1
        Me.tx_YKサプライ区分.Name = "tx_YKサプライ区分"
        Me.tx_YKサプライ区分.OldValue = "1"
        Me.tx_YKサプライ区分.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_YKサプライ区分.SelectText = True
        Me.tx_YKサプライ区分.SelLength = 0
        Me.tx_YKサプライ区分.SelStart = 0
        Me.tx_YKサプライ区分.SelText = ""
        Me.tx_YKサプライ区分.Size = New System.Drawing.Size(26, 22)
        Me.tx_YKサプライ区分.TabIndex = 61
        '
        'tx_YK物件区分
        '
        Me.tx_YK物件区分.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_YK物件区分.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_YK物件区分.CanForwardSetFocus = True
        Me.tx_YK物件区分.CanNextSetFocus = True
        Me.tx_YK物件区分.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_YK物件区分.DecimalPlace = CType(0, Short)
        Me.tx_YK物件区分.EditMode = True
        Me.tx_YK物件区分.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_YK物件区分.FormatType = ""
        Me.tx_YK物件区分.FormatTypeNega = ""
        Me.tx_YK物件区分.FormatTypeNull = ""
        Me.tx_YK物件区分.FormatTypeZero = ""
        Me.tx_YK物件区分.InputMinus = False
        Me.tx_YK物件区分.InputPlus = True
        Me.tx_YK物件区分.InputZero = False
        Me.tx_YK物件区分.Location = New System.Drawing.Point(896, 212)
        Me.tx_YK物件区分.MaxLength = 1
        Me.tx_YK物件区分.Name = "tx_YK物件区分"
        Me.tx_YK物件区分.OldValue = "1"
        Me.tx_YK物件区分.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_YK物件区分.SelectText = True
        Me.tx_YK物件区分.SelLength = 0
        Me.tx_YK物件区分.SelStart = 0
        Me.tx_YK物件区分.SelText = ""
        Me.tx_YK物件区分.Size = New System.Drawing.Size(26, 22)
        Me.tx_YK物件区分.TabIndex = 62
        '
        'tx_YK請求区分
        '
        Me.tx_YK請求区分.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_YK請求区分.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_YK請求区分.CanForwardSetFocus = True
        Me.tx_YK請求区分.CanNextSetFocus = True
        Me.tx_YK請求区分.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_YK請求区分.DecimalPlace = CType(0, Short)
        Me.tx_YK請求区分.EditMode = True
        Me.tx_YK請求区分.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_YK請求区分.FormatType = ""
        Me.tx_YK請求区分.FormatTypeNega = ""
        Me.tx_YK請求区分.FormatTypeNull = ""
        Me.tx_YK請求区分.FormatTypeZero = ""
        Me.tx_YK請求区分.InputMinus = False
        Me.tx_YK請求区分.InputPlus = True
        Me.tx_YK請求区分.InputZero = False
        Me.tx_YK請求区分.Location = New System.Drawing.Point(896, 232)
        Me.tx_YK請求区分.MaxLength = 1
        Me.tx_YK請求区分.Name = "tx_YK請求区分"
        Me.tx_YK請求区分.OldValue = "1"
        Me.tx_YK請求区分.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_YK請求区分.SelectText = True
        Me.tx_YK請求区分.SelLength = 0
        Me.tx_YK請求区分.SelStart = 0
        Me.tx_YK請求区分.SelText = ""
        Me.tx_YK請求区分.Size = New System.Drawing.Size(26, 22)
        Me.tx_YK請求区分.TabIndex = 63
        '
        'tx_化粧品メーカー区分
        '
        Me.tx_化粧品メーカー区分.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_化粧品メーカー区分.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_化粧品メーカー区分.CanForwardSetFocus = True
        Me.tx_化粧品メーカー区分.CanNextSetFocus = True
        Me.tx_化粧品メーカー区分.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_化粧品メーカー区分.DecimalPlace = CType(0, Short)
        Me.tx_化粧品メーカー区分.EditMode = True
        Me.tx_化粧品メーカー区分.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_化粧品メーカー区分.FormatType = ""
        Me.tx_化粧品メーカー区分.FormatTypeNega = ""
        Me.tx_化粧品メーカー区分.FormatTypeNull = ""
        Me.tx_化粧品メーカー区分.FormatTypeZero = ""
        Me.tx_化粧品メーカー区分.InputMinus = False
        Me.tx_化粧品メーカー区分.InputPlus = True
        Me.tx_化粧品メーカー区分.InputZero = False
        Me.tx_化粧品メーカー区分.Location = New System.Drawing.Point(1024, 24)
        Me.tx_化粧品メーカー区分.MaxLength = 1
        Me.tx_化粧品メーカー区分.Name = "tx_化粧品メーカー区分"
        Me.tx_化粧品メーカー区分.OldValue = "1"
        Me.tx_化粧品メーカー区分.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_化粧品メーカー区分.SelectText = True
        Me.tx_化粧品メーカー区分.SelLength = 0
        Me.tx_化粧品メーカー区分.SelStart = 0
        Me.tx_化粧品メーカー区分.SelText = ""
        Me.tx_化粧品メーカー区分.Size = New System.Drawing.Size(26, 22)
        Me.tx_化粧品メーカー区分.TabIndex = 227
        Me.tx_化粧品メーカー区分.Visible = False
        '
        'tx_SM内容区分
        '
        Me.tx_SM内容区分.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_SM内容区分.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_SM内容区分.CanForwardSetFocus = True
        Me.tx_SM内容区分.CanNextSetFocus = True
        Me.tx_SM内容区分.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_SM内容区分.DecimalPlace = CType(0, Short)
        Me.tx_SM内容区分.EditMode = True
        Me.tx_SM内容区分.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_SM内容区分.FormatType = ""
        Me.tx_SM内容区分.FormatTypeNega = ""
        Me.tx_SM内容区分.FormatTypeNull = ""
        Me.tx_SM内容区分.FormatTypeZero = ""
        Me.tx_SM内容区分.InputMinus = False
        Me.tx_SM内容区分.InputPlus = True
        Me.tx_SM内容区分.InputZero = True
        Me.tx_SM内容区分.Location = New System.Drawing.Point(900, 68)
        Me.tx_SM内容区分.MaxLength = 1
        Me.tx_SM内容区分.Name = "tx_SM内容区分"
        Me.tx_SM内容区分.OldValue = "1"
        Me.tx_SM内容区分.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_SM内容区分.SelectText = True
        Me.tx_SM内容区分.SelLength = 0
        Me.tx_SM内容区分.SelStart = 0
        Me.tx_SM内容区分.SelText = ""
        Me.tx_SM内容区分.Size = New System.Drawing.Size(26, 22)
        Me.tx_SM内容区分.TabIndex = 55
        '
        'tx_税集計区分
        '
        Me.tx_税集計区分.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_税集計区分.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_税集計区分.CanForwardSetFocus = True
        Me.tx_税集計区分.CanNextSetFocus = True
        Me.tx_税集計区分.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_税集計区分.DecimalPlace = CType(0, Short)
        Me.tx_税集計区分.EditMode = True
        Me.tx_税集計区分.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_税集計区分.FormatType = ""
        Me.tx_税集計区分.FormatTypeNega = ""
        Me.tx_税集計区分.FormatTypeNull = ""
        Me.tx_税集計区分.FormatTypeZero = ""
        Me.tx_税集計区分.InputMinus = True
        Me.tx_税集計区分.InputPlus = True
        Me.tx_税集計区分.InputZero = True
        Me.tx_税集計区分.Location = New System.Drawing.Point(716, 3)
        Me.tx_税集計区分.MaxLength = 2
        Me.tx_税集計区分.Name = "tx_税集計区分"
        Me.tx_税集計区分.OldValue = "ExNmTextBox"
        Me.tx_税集計区分.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_税集計区分.SelectText = True
        Me.tx_税集計区分.SelLength = 0
        Me.tx_税集計区分.SelStart = 0
        Me.tx_税集計区分.SelText = ""
        Me.tx_税集計区分.Size = New System.Drawing.Size(44, 22)
        Me.tx_税集計区分.TabIndex = 15
        Me.tx_税集計区分.Visible = False
        '
        'tx_クレーム区分
        '
        Me.tx_クレーム区分.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_クレーム区分.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_クレーム区分.CanForwardSetFocus = True
        Me.tx_クレーム区分.CanNextSetFocus = True
        Me.tx_クレーム区分.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_クレーム区分.DecimalPlace = CType(0, Short)
        Me.tx_クレーム区分.EditMode = True
        Me.tx_クレーム区分.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_クレーム区分.FormatType = ""
        Me.tx_クレーム区分.FormatTypeNega = ""
        Me.tx_クレーム区分.FormatTypeNull = ""
        Me.tx_クレーム区分.FormatTypeZero = ""
        Me.tx_クレーム区分.InputMinus = False
        Me.tx_クレーム区分.InputPlus = True
        Me.tx_クレーム区分.InputZero = False
        Me.tx_クレーム区分.Location = New System.Drawing.Point(508, 563)
        Me.tx_クレーム区分.MaxLength = 1
        Me.tx_クレーム区分.Name = "tx_クレーム区分"
        Me.tx_クレーム区分.OldValue = "1"
        Me.tx_クレーム区分.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_クレーム区分.SelectText = True
        Me.tx_クレーム区分.SelLength = 0
        Me.tx_クレーム区分.SelStart = 0
        Me.tx_クレーム区分.SelText = ""
        Me.tx_クレーム区分.Size = New System.Drawing.Size(26, 22)
        Me.tx_クレーム区分.TabIndex = 53
        Me.tx_クレーム区分.Visible = False
        '
        'tx_工事担当CD
        '
        Me.tx_工事担当CD.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_工事担当CD.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_工事担当CD.CanForwardSetFocus = True
        Me.tx_工事担当CD.CanNextSetFocus = True
        Me.tx_工事担当CD.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_工事担当CD.DecimalPlace = CType(0, Short)
        Me.tx_工事担当CD.EditMode = True
        Me.tx_工事担当CD.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_工事担当CD.FormatType = ""
        Me.tx_工事担当CD.FormatTypeNega = ""
        Me.tx_工事担当CD.FormatTypeNull = ""
        Me.tx_工事担当CD.FormatTypeZero = ""
        Me.tx_工事担当CD.InputMinus = False
        Me.tx_工事担当CD.InputPlus = True
        Me.tx_工事担当CD.InputZero = False
        Me.tx_工事担当CD.Location = New System.Drawing.Point(424, 116)
        Me.tx_工事担当CD.MaxLength = 3
        Me.tx_工事担当CD.Name = "tx_工事担当CD"
        Me.tx_工事担当CD.OldValue = "8"
        Me.tx_工事担当CD.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_工事担当CD.SelectText = True
        Me.tx_工事担当CD.SelLength = 0
        Me.tx_工事担当CD.SelStart = 0
        Me.tx_工事担当CD.SelText = ""
        Me.tx_工事担当CD.Size = New System.Drawing.Size(63, 22)
        Me.tx_工事担当CD.TabIndex = 7
        '
        'tx_発注書発行日付
        '
        Me.tx_発注書発行日付.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_発注書発行日付.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_発注書発行日付.CanForwardSetFocus = True
        Me.tx_発注書発行日付.CanNextSetFocus = True
        Me.tx_発注書発行日付.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_発注書発行日付.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_発注書発行日付.EditMode = True
        Me.tx_発注書発行日付.Enabled = False
        Me.tx_発注書発行日付.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_発注書発行日付.IMEMode = System.Windows.Forms.ImeMode.Hiragana
        Me.tx_発注書発行日付.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_発注書発行日付.Location = New System.Drawing.Point(896, 508)
        Me.tx_発注書発行日付.MaxLength = 30
        Me.tx_発注書発行日付.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_発注書発行日付.Name = "tx_発注書発行日付"
        Me.tx_発注書発行日付.OldValue = "ExTextBox"
        Me.tx_発注書発行日付.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_発注書発行日付.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_発注書発行日付.SelectText = True
        Me.tx_発注書発行日付.SelLength = 0
        Me.tx_発注書発行日付.SelStart = 0
        Me.tx_発注書発行日付.SelText = ""
        Me.tx_発注書発行日付.Size = New System.Drawing.Size(110, 22)
        Me.tx_発注書発行日付.TabIndex = 79
        Me.tx_発注書発行日付.Text = "ExTextBox"
        '
        'tx_完了者名
        '
        Me.tx_完了者名.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_完了者名.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_完了者名.CanForwardSetFocus = True
        Me.tx_完了者名.CanNextSetFocus = True
        Me.tx_完了者名.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_完了者名.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_完了者名.EditMode = True
        Me.tx_完了者名.Enabled = False
        Me.tx_完了者名.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_完了者名.IMEMode = System.Windows.Forms.ImeMode.Hiragana
        Me.tx_完了者名.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_完了者名.Location = New System.Drawing.Point(1028, 548)
        Me.tx_完了者名.MaxLength = 20
        Me.tx_完了者名.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_完了者名.Name = "tx_完了者名"
        Me.tx_完了者名.OldValue = "ExTextBox"
        Me.tx_完了者名.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_完了者名.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_完了者名.SelectText = True
        Me.tx_完了者名.SelLength = 0
        Me.tx_完了者名.SelStart = 0
        Me.tx_完了者名.SelText = ""
        Me.tx_完了者名.Size = New System.Drawing.Size(146, 22)
        Me.tx_完了者名.TabIndex = 84
        Me.tx_完了者名.Text = "ExTextBox"
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
        Me.tx_見積確定区分.Enabled = False
        Me.tx_見積確定区分.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_見積確定区分.FormatType = ""
        Me.tx_見積確定区分.FormatTypeNega = ""
        Me.tx_見積確定区分.FormatTypeNull = ""
        Me.tx_見積確定区分.FormatTypeZero = ""
        Me.tx_見積確定区分.InputMinus = False
        Me.tx_見積確定区分.InputPlus = True
        Me.tx_見積確定区分.InputZero = True
        Me.tx_見積確定区分.Location = New System.Drawing.Point(896, 528)
        Me.tx_見積確定区分.MaxLength = 1
        Me.tx_見積確定区分.Name = "tx_見積確定区分"
        Me.tx_見積確定区分.OldValue = "1"
        Me.tx_見積確定区分.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_見積確定区分.SelectText = True
        Me.tx_見積確定区分.SelLength = 0
        Me.tx_見積確定区分.SelStart = 0
        Me.tx_見積確定区分.SelText = ""
        Me.tx_見積確定区分.Size = New System.Drawing.Size(26, 22)
        Me.tx_見積確定区分.TabIndex = 80
        Me.tx_見積確定区分.Visible = False
        '
        'tx_経過備考1
        '
        Me.tx_経過備考1.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_経過備考1.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_経過備考1.CanForwardSetFocus = True
        Me.tx_経過備考1.CanNextSetFocus = True
        Me.tx_経過備考1.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_経過備考1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_経過備考1.EditMode = True
        Me.tx_経過備考1.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_経過備考1.IMEMode = System.Windows.Forms.ImeMode.Hiragana
        Me.tx_経過備考1.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_経過備考1.Location = New System.Drawing.Point(896, 592)
        Me.tx_経過備考1.MaxLength = 46
        Me.tx_経過備考1.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_経過備考1.Name = "tx_経過備考1"
        Me.tx_経過備考1.OldValue = "ExTextBox"
        Me.tx_経過備考1.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_経過備考1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_経過備考1.SelectText = True
        Me.tx_経過備考1.SelLength = 0
        Me.tx_経過備考1.SelStart = 0
        Me.tx_経過備考1.SelText = ""
        Me.tx_経過備考1.Size = New System.Drawing.Size(330, 22)
        Me.tx_経過備考1.TabIndex = 88
        Me.tx_経過備考1.Text = "ExTextBox"
        '
        'tx_経過備考2
        '
        Me.tx_経過備考2.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_経過備考2.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_経過備考2.CanForwardSetFocus = True
        Me.tx_経過備考2.CanNextSetFocus = False
        Me.tx_経過備考2.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_経過備考2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_経過備考2.EditMode = True
        Me.tx_経過備考2.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_経過備考2.IMEMode = System.Windows.Forms.ImeMode.Hiragana
        Me.tx_経過備考2.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_経過備考2.Location = New System.Drawing.Point(896, 612)
        Me.tx_経過備考2.MaxLength = 46
        Me.tx_経過備考2.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_経過備考2.Name = "tx_経過備考2"
        Me.tx_経過備考2.OldValue = "ExTextBox"
        Me.tx_経過備考2.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_経過備考2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_経過備考2.SelectText = True
        Me.tx_経過備考2.SelLength = 0
        Me.tx_経過備考2.SelStart = 0
        Me.tx_経過備考2.SelText = ""
        Me.tx_経過備考2.Size = New System.Drawing.Size(330, 22)
        Me.tx_経過備考2.TabIndex = 89
        Me.tx_経過備考2.Text = "ExTextBox"
        '
        'tx_請求予定Y
        '
        Me.tx_請求予定Y.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_請求予定Y.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_請求予定Y.CanForwardSetFocus = True
        Me.tx_請求予定Y.CanNextSetFocus = True
        Me.tx_請求予定Y.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_請求予定Y.EditMode = True
        Me.tx_請求予定Y.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_請求予定Y.Location = New System.Drawing.Point(897, 573)
        Me.tx_請求予定Y.MaxLength = 4
        Me.tx_請求予定Y.Name = "tx_請求予定Y"
        Me.tx_請求予定Y.OldValue = "YYYY"
        Me.tx_請求予定Y.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_請求予定Y.SelectText = True
        Me.tx_請求予定Y.SelLength = 0
        Me.tx_請求予定Y.SelStart = 0
        Me.tx_請求予定Y.SelText = ""
        Me.tx_請求予定Y.Size = New System.Drawing.Size(32, 16)
        Me.tx_請求予定Y.TabIndex = 85
        Me.tx_請求予定Y.Text = "YYYY"
        '
        'tx_請求予定M
        '
        Me.tx_請求予定M.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_請求予定M.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_請求予定M.CanForwardSetFocus = True
        Me.tx_請求予定M.CanNextSetFocus = True
        Me.tx_請求予定M.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_請求予定M.EditMode = True
        Me.tx_請求予定M.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_請求予定M.Location = New System.Drawing.Point(946, 573)
        Me.tx_請求予定M.MaxLength = 2
        Me.tx_請求予定M.Name = "tx_請求予定M"
        Me.tx_請求予定M.OldValue = "MM"
        Me.tx_請求予定M.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_請求予定M.SelectText = True
        Me.tx_請求予定M.SelLength = 0
        Me.tx_請求予定M.SelStart = 0
        Me.tx_請求予定M.SelText = ""
        Me.tx_請求予定M.Size = New System.Drawing.Size(20, 16)
        Me.tx_請求予定M.TabIndex = 86
        Me.tx_請求予定M.Text = "MM"
        '
        'tx_請求予定D
        '
        Me.tx_請求予定D.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_請求予定D.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_請求予定D.CanForwardSetFocus = True
        Me.tx_請求予定D.CanNextSetFocus = True
        Me.tx_請求予定D.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_請求予定D.EditMode = True
        Me.tx_請求予定D.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_請求予定D.Location = New System.Drawing.Point(985, 573)
        Me.tx_請求予定D.MaxLength = 2
        Me.tx_請求予定D.Name = "tx_請求予定D"
        Me.tx_請求予定D.OldValue = "DD"
        Me.tx_請求予定D.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_請求予定D.SelectText = True
        Me.tx_請求予定D.SelLength = 0
        Me.tx_請求予定D.SelStart = 0
        Me.tx_請求予定D.SelText = ""
        Me.tx_請求予定D.Size = New System.Drawing.Size(20, 16)
        Me.tx_請求予定D.TabIndex = 87
        Me.tx_請求予定D.Text = "DD"
        '
        'tx_完了日Y
        '
        Me.tx_完了日Y.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_完了日Y.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_完了日Y.CanForwardSetFocus = True
        Me.tx_完了日Y.CanNextSetFocus = True
        Me.tx_完了日Y.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_完了日Y.EditMode = True
        Me.tx_完了日Y.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_完了日Y.Location = New System.Drawing.Point(897, 549)
        Me.tx_完了日Y.MaxLength = 4
        Me.tx_完了日Y.Name = "tx_完了日Y"
        Me.tx_完了日Y.OldValue = "YYYY"
        Me.tx_完了日Y.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_完了日Y.SelectText = True
        Me.tx_完了日Y.SelLength = 0
        Me.tx_完了日Y.SelStart = 0
        Me.tx_完了日Y.SelText = ""
        Me.tx_完了日Y.Size = New System.Drawing.Size(32, 16)
        Me.tx_完了日Y.TabIndex = 81
        Me.tx_完了日Y.Text = "YYYY"
        '
        'tx_完了日M
        '
        Me.tx_完了日M.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_完了日M.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_完了日M.CanForwardSetFocus = True
        Me.tx_完了日M.CanNextSetFocus = True
        Me.tx_完了日M.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_完了日M.EditMode = True
        Me.tx_完了日M.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_完了日M.Location = New System.Drawing.Point(946, 549)
        Me.tx_完了日M.MaxLength = 2
        Me.tx_完了日M.Name = "tx_完了日M"
        Me.tx_完了日M.OldValue = "MM"
        Me.tx_完了日M.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_完了日M.SelectText = True
        Me.tx_完了日M.SelLength = 0
        Me.tx_完了日M.SelStart = 0
        Me.tx_完了日M.SelText = ""
        Me.tx_完了日M.Size = New System.Drawing.Size(20, 16)
        Me.tx_完了日M.TabIndex = 82
        Me.tx_完了日M.Text = "MM"
        '
        'tx_完了日D
        '
        Me.tx_完了日D.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_完了日D.BorderStyle = ExDateText.ExDateTextBoxY.BorderStyleType.なし
        Me.tx_完了日D.CanForwardSetFocus = True
        Me.tx_完了日D.CanNextSetFocus = True
        Me.tx_完了日D.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_完了日D.EditMode = True
        Me.tx_完了日D.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_完了日D.Location = New System.Drawing.Point(985, 549)
        Me.tx_完了日D.MaxLength = 2
        Me.tx_完了日D.Name = "tx_完了日D"
        Me.tx_完了日D.OldValue = "DD"
        Me.tx_完了日D.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_完了日D.SelectText = True
        Me.tx_完了日D.SelLength = 0
        Me.tx_完了日D.SelStart = 0
        Me.tx_完了日D.SelText = ""
        Me.tx_完了日D.Size = New System.Drawing.Size(20, 16)
        Me.tx_完了日D.TabIndex = 83
        Me.tx_完了日D.Text = "DD"
        '
        'tx_集計CD
        '
        Me.tx_集計CD.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.tx_集計CD.BorderStyle = ExText.ExTextBox.BorderStyleType.実線
        Me.tx_集計CD.CanForwardSetFocus = True
        Me.tx_集計CD.CanNextSetFocus = True
        Me.tx_集計CD.CharacterSize = ExText.ExTextBox.CharSize.なし
        Me.tx_集計CD.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_集計CD.EditMode = True
        Me.tx_集計CD.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_集計CD.IMEMode = System.Windows.Forms.ImeMode.Disable
        Me.tx_集計CD.LengthType = ExText.ExTextBox.LenType.UnicodeType
        Me.tx_集計CD.Location = New System.Drawing.Point(180, 231)
        Me.tx_集計CD.MaxLength = 4
        Me.tx_集計CD.MousePointer = System.Windows.Forms.Cursors.IBeam
        Me.tx_集計CD.Name = "tx_集計CD"
        Me.tx_集計CD.OldValue = "ExTextBox"
        Me.tx_集計CD.PasswordChar = "" & Global.Microsoft.VisualBasic.ChrW(0)
        Me.tx_集計CD.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_集計CD.SelectText = True
        Me.tx_集計CD.SelLength = 0
        Me.tx_集計CD.SelStart = 0
        Me.tx_集計CD.SelText = ""
        Me.tx_集計CD.Size = New System.Drawing.Size(65, 22)
        Me.tx_集計CD.TabIndex = 16
        Me.tx_集計CD.Text = "ExTextBox"
        '
        'tx_B請求管轄区分
        '
        Me.tx_B請求管轄区分.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_B請求管轄区分.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_B請求管轄区分.CanForwardSetFocus = True
        Me.tx_B請求管轄区分.CanNextSetFocus = True
        Me.tx_B請求管轄区分.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_B請求管轄区分.DecimalPlace = CType(0, Short)
        Me.tx_B請求管轄区分.EditMode = True
        Me.tx_B請求管轄区分.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_B請求管轄区分.FormatType = ""
        Me.tx_B請求管轄区分.FormatTypeNega = ""
        Me.tx_B請求管轄区分.FormatTypeNull = ""
        Me.tx_B請求管轄区分.FormatTypeZero = ""
        Me.tx_B請求管轄区分.InputMinus = False
        Me.tx_B請求管轄区分.InputPlus = True
        Me.tx_B請求管轄区分.InputZero = False
        Me.tx_B請求管轄区分.Location = New System.Drawing.Point(896, 264)
        Me.tx_B請求管轄区分.MaxLength = 1
        Me.tx_B請求管轄区分.Name = "tx_B請求管轄区分"
        Me.tx_B請求管轄区分.OldValue = "1"
        Me.tx_B請求管轄区分.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_B請求管轄区分.SelectText = True
        Me.tx_B請求管轄区分.SelLength = 0
        Me.tx_B請求管轄区分.SelStart = 0
        Me.tx_B請求管轄区分.SelText = ""
        Me.tx_B請求管轄区分.Size = New System.Drawing.Size(26, 22)
        Me.tx_B請求管轄区分.TabIndex = 64
        '
        'tx_BtoB番号
        '
        Me.tx_BtoB番号.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_BtoB番号.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_BtoB番号.CanForwardSetFocus = True
        Me.tx_BtoB番号.CanNextSetFocus = True
        Me.tx_BtoB番号.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_BtoB番号.DecimalPlace = CType(0, Short)
        Me.tx_BtoB番号.EditMode = True
        Me.tx_BtoB番号.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_BtoB番号.FormatType = "###"
        Me.tx_BtoB番号.FormatTypeNega = ""
        Me.tx_BtoB番号.FormatTypeNull = ""
        Me.tx_BtoB番号.FormatTypeZero = ""
        Me.tx_BtoB番号.InputMinus = True
        Me.tx_BtoB番号.InputPlus = True
        Me.tx_BtoB番号.InputZero = False
        Me.tx_BtoB番号.Location = New System.Drawing.Point(1152, 264)
        Me.tx_BtoB番号.MaxLength = 5
        Me.tx_BtoB番号.Name = "tx_BtoB番号"
        Me.tx_BtoB番号.OldValue = "12345"
        Me.tx_BtoB番号.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_BtoB番号.SelectText = True
        Me.tx_BtoB番号.SelLength = 0
        Me.tx_BtoB番号.SelStart = 0
        Me.tx_BtoB番号.SelText = ""
        Me.tx_BtoB番号.Size = New System.Drawing.Size(54, 22)
        Me.tx_BtoB番号.TabIndex = 65
        '
        'tx_業種区分
        '
        Me.tx_業種区分.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_業種区分.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_業種区分.CanForwardSetFocus = True
        Me.tx_業種区分.CanNextSetFocus = True
        Me.tx_業種区分.Cursor = System.Windows.Forms.Cursors.Default
        Me.tx_業種区分.DecimalPlace = CType(0, Short)
        Me.tx_業種区分.EditMode = True
        Me.tx_業種区分.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_業種区分.FormatType = ""
        Me.tx_業種区分.FormatTypeNega = ""
        Me.tx_業種区分.FormatTypeNull = ""
        Me.tx_業種区分.FormatTypeZero = ""
        Me.tx_業種区分.InputMinus = False
        Me.tx_業種区分.InputPlus = True
        Me.tx_業種区分.InputZero = True
        Me.tx_業種区分.Location = New System.Drawing.Point(200, 492)
        Me.tx_業種区分.MaxLength = 1
        Me.tx_業種区分.Name = "tx_業種区分"
        Me.tx_業種区分.OldValue = "1"
        Me.tx_業種区分.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_業種区分.SelectText = True
        Me.tx_業種区分.SelLength = 0
        Me.tx_業種区分.SelStart = 0
        Me.tx_業種区分.SelText = ""
        Me.tx_業種区分.Size = New System.Drawing.Size(26, 22)
        Me.tx_業種区分.TabIndex = 39
        Me.tx_業種区分.Text = "1"
        '
        'tx_統合見積番号
        '
        Me.tx_統合見積番号.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.tx_統合見積番号.BorderStyle = ExNmText.ExNmTextBox.BorderStyleType.実線
        Me.tx_統合見積番号.CanForwardSetFocus = True
        Me.tx_統合見積番号.CanNextSetFocus = True
        Me.tx_統合見積番号.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tx_統合見積番号.DecimalPlace = CType(0, Short)
        Me.tx_統合見積番号.EditMode = True
        Me.tx_統合見積番号.FocusBackColor = System.Drawing.SystemColors.Window
        Me.tx_統合見積番号.FormatType = ""
        Me.tx_統合見積番号.FormatTypeNega = ""
        Me.tx_統合見積番号.FormatTypeNull = ""
        Me.tx_統合見積番号.FormatTypeZero = ""
        Me.tx_統合見積番号.InputMinus = False
        Me.tx_統合見積番号.InputPlus = True
        Me.tx_統合見積番号.InputZero = False
        Me.tx_統合見積番号.Location = New System.Drawing.Point(180, 68)
        Me.tx_統合見積番号.MaxLength = 7
        Me.tx_統合見積番号.Name = "tx_統合見積番号"
        Me.tx_統合見積番号.OldValue = "ExNmTextBox"
        Me.tx_統合見積番号.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tx_統合見積番号.SelectText = True
        Me.tx_統合見積番号.SelLength = 0
        Me.tx_統合見積番号.SelStart = 0
        Me.tx_統合見積番号.SelText = ""
        Me.tx_統合見積番号.Size = New System.Drawing.Size(65, 22)
        Me.tx_統合見積番号.TabIndex = 1
        '
        'rf_統合見積件名
        '
        Me.rf_統合見積件名.BackColor = System.Drawing.SystemColors.Control
        Me.rf_統合見積件名.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.rf_統合見積件名.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_統合見積件名.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_統合見積件名.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_統合見積件名.Location = New System.Drawing.Point(245, 68)
        Me.rf_統合見積件名.Name = "rf_統合見積件名"
        Me.rf_統合見積件名.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_統合見積件名.Size = New System.Drawing.Size(430, 19)
        Me.rf_統合見積件名.TabIndex = 262
        Me.rf_統合見積件名.Text = "ＷＷＷＷＷＷＷＷ"
        '
        '_lblLabels_58
        '
        Me._lblLabels_58.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_58.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_58.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_58.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_58.Location = New System.Drawing.Point(108, 496)
        Me._lblLabels_58.Name = "_lblLabels_58"
        Me._lblLabels_58.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_58.Size = New System.Drawing.Size(73, 13)
        Me._lblLabels_58.TabIndex = 261
        Me._lblLabels_58.Text = "業種区分"
        '
        '_lblLabels_57
        '
        Me._lblLabels_57.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_57.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_57.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_57.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_57.Location = New System.Drawing.Point(236, 493)
        Me._lblLabels_57.Name = "_lblLabels_57"
        Me._lblLabels_57.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_57.Size = New System.Drawing.Size(433, 19)
        Me._lblLabels_57.TabIndex = 260
        Me._lblLabels_57.Text = "0:什器 1:内装"
        '
        '_lblLabels_56
        '
        Me._lblLabels_56.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_56.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_56.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_56.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_56.Location = New System.Drawing.Point(1088, 268)
        Me._lblLabels_56.Name = "_lblLabels_56"
        Me._lblLabels_56.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_56.Size = New System.Drawing.Size(61, 13)
        Me._lblLabels_56.TabIndex = 259
        Me._lblLabels_56.Text = "BtoB番号"
        '
        '_lblLabels_55
        '
        Me._lblLabels_55.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_55.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_55.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_55.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_55.Location = New System.Drawing.Point(932, 268)
        Me._lblLabels_55.Name = "_lblLabels_55"
        Me._lblLabels_55.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_55.Size = New System.Drawing.Size(131, 18)
        Me._lblLabels_55.TabIndex = 258
        Me._lblLabels_55.Text = "1:請求書 2:BtoB"
        '
        '_lblLabels_54
        '
        Me._lblLabels_54.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_54.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_54.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_54.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_54.Location = New System.Drawing.Point(812, 268)
        Me._lblLabels_54.Name = "_lblLabels_54"
        Me._lblLabels_54.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_54.Size = New System.Drawing.Size(73, 21)
        Me._lblLabels_54.TabIndex = 257
        Me._lblLabels_54.Text = "請求管轄"
        '
        '_lb_項目_15
        '
        Me._lb_項目_15.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me._lb_項目_15.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_15.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_15.ForeColor = System.Drawing.Color.White
        Me._lb_項目_15.Location = New System.Drawing.Point(704, 260)
        Me._lb_項目_15.Name = "_lb_項目_15"
        Me._lb_項目_15.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_15.Size = New System.Drawing.Size(101, 29)
        Me._lb_項目_15.TabIndex = 256
        Me._lb_項目_15.Text = "ベルク情報"
        '
        'rf_集計名
        '
        Me.rf_集計名.BackColor = System.Drawing.SystemColors.Control
        Me.rf_集計名.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.rf_集計名.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_集計名.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_集計名.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_集計名.Location = New System.Drawing.Point(245, 231)
        Me.rf_集計名.Name = "rf_集計名"
        Me.rf_集計名.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_集計名.Size = New System.Drawing.Size(100, 19)
        Me.rf_集計名.TabIndex = 255
        Me.rf_集計名.Text = "１２３４５６７８９０１２３４５"
        '
        '_lblLabels_53
        '
        Me._lblLabels_53.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_53.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_53.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_53.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_53.Location = New System.Drawing.Point(118, 233)
        Me._lblLabels_53.Name = "_lblLabels_53"
        Me._lblLabels_53.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_53.Size = New System.Drawing.Size(55, 13)
        Me._lblLabels_53.TabIndex = 254
        Me._lblLabels_53.Text = "集計CD"
        '
        '_lb_日_8
        '
        Me._lb_日_8.BackColor = System.Drawing.SystemColors.Window
        Me._lb_日_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_日_8.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_日_8.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_日_8.Location = New System.Drawing.Point(1008, 576)
        Me._lb_日_8.Name = "_lb_日_8"
        Me._lb_日_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_日_8.Size = New System.Drawing.Size(12, 14)
        Me._lb_日_8.TabIndex = 253
        Me._lb_日_8.Text = "日"
        Me._lb_日_8.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label14
        '
        Me.Label14.BackColor = System.Drawing.SystemColors.Control
        Me.Label14.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label14.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label14.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label14.Location = New System.Drawing.Point(812, 576)
        Me.Label14.Name = "Label14"
        Me.Label14.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label14.Size = New System.Drawing.Size(77, 13)
        Me.Label14.TabIndex = 252
        Me.Label14.Text = "請求予定日"
        '
        '_lb_項目_14
        '
        Me._lb_項目_14.BackColor = System.Drawing.SystemColors.Window
        Me._lb_項目_14.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_14.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_項目_14.Location = New System.Drawing.Point(970, 576)
        Me._lb_項目_14.Name = "_lb_項目_14"
        Me._lb_項目_14.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_14.Size = New System.Drawing.Size(12, 14)
        Me._lb_項目_14.TabIndex = 250
        Me._lb_項目_14.Text = "月"
        Me._lb_項目_14.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_13
        '
        Me._lb_項目_13.BackColor = System.Drawing.SystemColors.Window
        Me._lb_項目_13.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_13.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_項目_13.Location = New System.Drawing.Point(933, 576)
        Me._lb_項目_13.Name = "_lb_項目_13"
        Me._lb_項目_13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_13.Size = New System.Drawing.Size(17, 14)
        Me._lb_項目_13.TabIndex = 249
        Me._lb_項目_13.Text = "年"
        Me._lb_項目_13.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblLabels_52
        '
        Me._lblLabels_52.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_52.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_52.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_52.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_52.Location = New System.Drawing.Point(812, 595)
        Me._lblLabels_52.Name = "_lblLabels_52"
        Me._lblLabels_52.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_52.Size = New System.Drawing.Size(81, 13)
        Me._lblLabels_52.TabIndex = 248
        Me._lblLabels_52.Text = "備　考"
        '
        '_lb_項目_12
        '
        Me._lb_項目_12.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me._lb_項目_12.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_12.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_12.ForeColor = System.Drawing.Color.White
        Me._lb_項目_12.Location = New System.Drawing.Point(704, 504)
        Me._lb_項目_12.Name = "_lb_項目_12"
        Me._lb_項目_12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_12.Size = New System.Drawing.Size(101, 133)
        Me._lb_項目_12.TabIndex = 247
        Me._lb_項目_12.Text = "経過情報"
        '
        'Label13
        '
        Me.Label13.BackColor = System.Drawing.SystemColors.Control
        Me.Label13.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label13.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label13.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label13.Location = New System.Drawing.Point(812, 512)
        Me.Label13.Name = "Label13"
        Me.Label13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label13.Size = New System.Drawing.Size(81, 13)
        Me.Label13.TabIndex = 246
        Me.Label13.Text = "発注書発行日"
        '
        '_lblLabels_51
        '
        Me._lblLabels_51.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_51.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_51.Enabled = False
        Me._lblLabels_51.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_51.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_51.Location = New System.Drawing.Point(928, 532)
        Me._lblLabels_51.Name = "_lblLabels_51"
        Me._lblLabels_51.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_51.Size = New System.Drawing.Size(169, 13)
        Me._lblLabels_51.TabIndex = 245
        Me._lblLabels_51.Text = "0:未確定 1:確定"
        Me._lblLabels_51.Visible = False
        '
        'Label12
        '
        Me.Label12.BackColor = System.Drawing.SystemColors.Control
        Me.Label12.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label12.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label12.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label12.Location = New System.Drawing.Point(812, 532)
        Me.Label12.Name = "Label12"
        Me.Label12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label12.Size = New System.Drawing.Size(77, 13)
        Me.Label12.TabIndex = 244
        Me.Label12.Text = "見積確定"
        '
        'Label9
        '
        Me.Label9.BackColor = System.Drawing.SystemColors.Control
        Me.Label9.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label9.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label9.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label9.Location = New System.Drawing.Point(812, 552)
        Me.Label9.Name = "Label9"
        Me.Label9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label9.Size = New System.Drawing.Size(77, 13)
        Me.Label9.TabIndex = 242
        Me.Label9.Text = "完了日付"
        '
        '_lb_日_7
        '
        Me._lb_日_7.BackColor = System.Drawing.SystemColors.Window
        Me._lb_日_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_日_7.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_日_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_日_7.Location = New System.Drawing.Point(1008, 551)
        Me._lb_日_7.Name = "_lb_日_7"
        Me._lb_日_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_日_7.Size = New System.Drawing.Size(12, 14)
        Me._lb_日_7.TabIndex = 241
        Me._lb_日_7.Text = "日"
        Me._lb_日_7.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_月_7
        '
        Me._lb_月_7.BackColor = System.Drawing.SystemColors.Window
        Me._lb_月_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_月_7.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_月_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_月_7.Location = New System.Drawing.Point(970, 551)
        Me._lb_月_7.Name = "_lb_月_7"
        Me._lb_月_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_月_7.Size = New System.Drawing.Size(12, 14)
        Me._lb_月_7.TabIndex = 240
        Me._lb_月_7.Text = "月"
        Me._lb_月_7.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_年_7
        '
        Me._lb_年_7.BackColor = System.Drawing.SystemColors.Window
        Me._lb_年_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_年_7.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_年_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_年_7.Location = New System.Drawing.Point(933, 551)
        Me._lb_年_7.Name = "_lb_年_7"
        Me._lb_年_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_年_7.Size = New System.Drawing.Size(17, 14)
        Me._lb_年_7.TabIndex = 239
        Me._lb_年_7.Text = "年"
        Me._lb_年_7.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label10
        '
        Me.Label10.BackColor = System.Drawing.SystemColors.Control
        Me.Label10.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label10.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label10.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label10.Location = New System.Drawing.Point(353, 120)
        Me.Label10.Name = "Label10"
        Me.Label10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label10.Size = New System.Drawing.Size(68, 13)
        Me.Label10.TabIndex = 238
        Me.Label10.Text = "工事担当"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'rf_工事担当名
        '
        Me.rf_工事担当名.BackColor = System.Drawing.SystemColors.Control
        Me.rf_工事担当名.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.rf_工事担当名.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_工事担当名.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_工事担当名.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_工事担当名.Location = New System.Drawing.Point(487, 116)
        Me.rf_工事担当名.Name = "rf_工事担当名"
        Me.rf_工事担当名.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_工事担当名.Size = New System.Drawing.Size(130, 19)
        Me.rf_工事担当名.TabIndex = 237
        Me.rf_工事担当名.Text = "ＷＷＷＷＷＷＷＷ"
        '
        '_lblLabels_50
        '
        Me._lblLabels_50.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_50.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_50.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_50.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_50.Location = New System.Drawing.Point(420, 565)
        Me._lblLabels_50.Name = "_lblLabels_50"
        Me._lblLabels_50.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_50.Size = New System.Drawing.Size(85, 13)
        Me._lblLabels_50.TabIndex = 236
        Me._lblLabels_50.Text = "クレーム区分"
        Me._lblLabels_50.Visible = False
        '
        '_lblLabels_49
        '
        Me._lblLabels_49.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_49.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_49.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_49.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_49.Location = New System.Drawing.Point(540, 565)
        Me._lblLabels_49.Name = "_lblLabels_49"
        Me._lblLabels_49.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_49.Size = New System.Drawing.Size(87, 19)
        Me._lblLabels_49.TabIndex = 235
        Me._lblLabels_49.Text = "1:クレーム"
        Me._lblLabels_49.Visible = False
        '
        '_lblLabels_48
        '
        Me._lblLabels_48.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_48.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_48.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_48.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_48.Location = New System.Drawing.Point(844, 6)
        Me._lblLabels_48.Name = "_lblLabels_48"
        Me._lblLabels_48.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_48.Size = New System.Drawing.Size(387, 13)
        Me._lblLabels_48.TabIndex = 234
        Me._lblLabels_48.Text = "外国企業で輸出入でない（国内取引）場合、課税対象に変更"
        Me._lblLabels_48.Visible = False
        '
        '_lblLabels_47
        '
        Me._lblLabels_47.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_47.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_47.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_47.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_47.Location = New System.Drawing.Point(659, 6)
        Me._lblLabels_47.Name = "_lblLabels_47"
        Me._lblLabels_47.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_47.Size = New System.Drawing.Size(52, 18)
        Me._lblLabels_47.TabIndex = 233
        Me._lblLabels_47.Text = "税集計"
        Me._lblLabels_47.Visible = False
        '
        '_lblLabels_1
        '
        Me._lblLabels_1.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_1.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_1.Location = New System.Drawing.Point(118, 216)
        Me._lblLabels_1.Name = "_lblLabels_1"
        Me._lblLabels_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_1.Size = New System.Drawing.Size(55, 13)
        Me._lblLabels_1.TabIndex = 232
        Me._lblLabels_1.Text = "担当者"
        '
        'rf_税集計区分名
        '
        Me.rf_税集計区分名.BackColor = System.Drawing.SystemColors.Control
        Me.rf_税集計区分名.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.rf_税集計区分名.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_税集計区分名.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_税集計区分名.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_税集計区分名.Location = New System.Drawing.Point(760, 3)
        Me.rf_税集計区分名.Name = "rf_税集計区分名"
        Me.rf_税集計区分名.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_税集計区分名.Size = New System.Drawing.Size(78, 19)
        Me.rf_税集計区分名.TabIndex = 17
        Me.rf_税集計区分名.Text = "伝票単位"
        Me.rf_税集計区分名.Visible = False
        '
        '_lblLabels_46
        '
        Me._lblLabels_46.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_46.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_46.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_46.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_46.Location = New System.Drawing.Point(932, 72)
        Me._lblLabels_46.Name = "_lblLabels_46"
        Me._lblLabels_46.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_46.Size = New System.Drawing.Size(287, 13)
        Me._lblLabels_46.TabIndex = 231
        Me._lblLabels_46.Text = "0:なし 1:ﾃﾞｨﾊﾞﾛ 2:台湾 3:ｼｽﾃﾑ開発"
        '
        '_lblLabels_45
        '
        Me._lblLabels_45.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_45.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_45.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_45.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_45.Location = New System.Drawing.Point(812, 72)
        Me._lblLabels_45.Name = "_lblLabels_45"
        Me._lblLabels_45.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_45.Size = New System.Drawing.Size(73, 14)
        Me._lblLabels_45.TabIndex = 230
        Me._lblLabels_45.Text = "内容区分"
        '
        '_lblLabels_44
        '
        Me._lblLabels_44.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_44.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_44.Enabled = False
        Me._lblLabels_44.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_44.Location = New System.Drawing.Point(936, 28)
        Me._lblLabels_44.Name = "_lblLabels_44"
        Me._lblLabels_44.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_44.Size = New System.Drawing.Size(85, 13)
        Me._lblLabels_44.TabIndex = 229
        Me._lblLabels_44.Text = "化粧品ﾒｰｶｰ区分"
        Me._lblLabels_44.Visible = False
        '
        '_lblLabels_43
        '
        Me._lblLabels_43.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_43.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_43.Enabled = False
        Me._lblLabels_43.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_43.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_43.Location = New System.Drawing.Point(1060, 28)
        Me._lblLabels_43.Name = "_lblLabels_43"
        Me._lblLabels_43.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_43.Size = New System.Drawing.Size(165, 18)
        Me._lblLabels_43.TabIndex = 228
        Me._lblLabels_43.Text = "1:化粧品ﾒｰｶｰ"
        Me._lblLabels_43.Visible = False
        '
        '_lb_項目_11
        '
        Me._lb_項目_11.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me._lb_項目_11.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_11.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_11.ForeColor = System.Drawing.Color.White
        Me._lb_項目_11.Location = New System.Drawing.Point(704, 44)
        Me._lb_項目_11.Name = "_lb_項目_11"
        Me._lb_項目_11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_11.Size = New System.Drawing.Size(101, 49)
        Me._lb_項目_11.TabIndex = 226
        Me._lb_項目_11.Text = "しまむら情報"
        '
        '_lblLabels_42
        '
        Me._lblLabels_42.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_42.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_42.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_42.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_42.Location = New System.Drawing.Point(812, 236)
        Me._lblLabels_42.Name = "_lblLabels_42"
        Me._lblLabels_42.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_42.Size = New System.Drawing.Size(73, 17)
        Me._lblLabels_42.TabIndex = 225
        Me._lblLabels_42.Text = "請求区分"
        '
        '_lblLabels_41
        '
        Me._lblLabels_41.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_41.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_41.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_41.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_41.Location = New System.Drawing.Point(932, 236)
        Me._lblLabels_41.Name = "_lblLabels_41"
        Me._lblLabels_41.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_41.Size = New System.Drawing.Size(237, 13)
        Me._lblLabels_41.TabIndex = 224
        Me._lblLabels_41.Text = "1:新財務 2:リース 3:ﾁｪｰﾝｽﾄｱ伝票"
        '
        '_lblLabels_40
        '
        Me._lblLabels_40.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_40.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_40.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_40.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_40.Location = New System.Drawing.Point(812, 216)
        Me._lblLabels_40.Name = "_lblLabels_40"
        Me._lblLabels_40.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_40.Size = New System.Drawing.Size(73, 18)
        Me._lblLabels_40.TabIndex = 223
        Me._lblLabels_40.Text = "物件区分"
        '
        '_lblLabels_39
        '
        Me._lblLabels_39.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_39.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_39.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_39.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_39.Location = New System.Drawing.Point(932, 216)
        Me._lblLabels_39.Name = "_lblLabels_39"
        Me._lblLabels_39.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_39.Size = New System.Drawing.Size(287, 18)
        Me._lblLabels_39.TabIndex = 222
        Me._lblLabels_39.Text = "1:物件 2:メンテ・備品 3:担当者案件"
        '
        '_lblLabels_36
        '
        Me._lblLabels_36.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_36.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_36.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_36.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_36.Location = New System.Drawing.Point(812, 196)
        Me._lblLabels_36.Name = "_lblLabels_36"
        Me._lblLabels_36.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_36.Size = New System.Drawing.Size(77, 20)
        Me._lblLabels_36.TabIndex = 221
        Me._lblLabels_36.Text = "請求管轄"
        '
        '_lblLabels_35
        '
        Me._lblLabels_35.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_35.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_35.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_35.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_35.Location = New System.Drawing.Point(932, 196)
        Me._lblLabels_35.Name = "_lblLabels_35"
        Me._lblLabels_35.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_35.Size = New System.Drawing.Size(165, 13)
        Me._lblLabels_35.TabIndex = 220
        Me._lblLabels_35.Text = "1:サプライ 2:情報ｼｽﾃﾑ"
        '
        '_lblLabels_38
        '
        Me._lblLabels_38.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_38.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_38.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_38.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_38.Location = New System.Drawing.Point(108, 616)
        Me._lblLabels_38.Name = "_lblLabels_38"
        Me._lblLabels_38.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_38.Size = New System.Drawing.Size(81, 13)
        Me._lblLabels_38.TabIndex = 219
        Me._lblLabels_38.Text = "作業内容"
        '
        '_lblLabels_37
        '
        Me._lblLabels_37.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_37.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_37.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_37.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_37.Location = New System.Drawing.Point(108, 596)
        Me._lblLabels_37.Name = "_lblLabels_37"
        Me._lblLabels_37.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_37.Size = New System.Drawing.Size(87, 13)
        Me._lblLabels_37.TabIndex = 218
        Me._lblLabels_37.Text = "発注担当者"
        '
        'Label7
        '
        Me.Label7.BackColor = System.Drawing.SystemColors.Control
        Me.Label7.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label7.Enabled = False
        Me.Label7.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label7.Location = New System.Drawing.Point(108, 576)
        Me.Label7.Name = "Label7"
        Me.Label7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label7.Size = New System.Drawing.Size(85, 21)
        Me.Label7.TabIndex = 216
        Me.Label7.Text = "完工(工)日付"
        Me.Label7.Visible = False
        '
        '_lb_日_6
        '
        Me._lb_日_6.BackColor = System.Drawing.SystemColors.Window
        Me._lb_日_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_日_6.Enabled = False
        Me._lb_日_6.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_日_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_日_6.Location = New System.Drawing.Point(312, 575)
        Me._lb_日_6.Name = "_lb_日_6"
        Me._lb_日_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_日_6.Size = New System.Drawing.Size(12, 14)
        Me._lb_日_6.TabIndex = 215
        Me._lb_日_6.Text = "日"
        Me._lb_日_6.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me._lb_日_6.Visible = False
        '
        '_lb_月_6
        '
        Me._lb_月_6.BackColor = System.Drawing.SystemColors.Window
        Me._lb_月_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_月_6.Enabled = False
        Me._lb_月_6.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_月_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_月_6.Location = New System.Drawing.Point(274, 575)
        Me._lb_月_6.Name = "_lb_月_6"
        Me._lb_月_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_月_6.Size = New System.Drawing.Size(12, 14)
        Me._lb_月_6.TabIndex = 214
        Me._lb_月_6.Text = "月"
        Me._lb_月_6.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me._lb_月_6.Visible = False
        '
        '_lb_年_6
        '
        Me._lb_年_6.BackColor = System.Drawing.SystemColors.Window
        Me._lb_年_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_年_6.Enabled = False
        Me._lb_年_6.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_年_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_年_6.Location = New System.Drawing.Point(237, 575)
        Me._lb_年_6.Name = "_lb_年_6"
        Me._lb_年_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_年_6.Size = New System.Drawing.Size(17, 14)
        Me._lb_年_6.TabIndex = 213
        Me._lb_年_6.Text = "年"
        Me._lb_年_6.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me._lb_年_6.Visible = False
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.SystemColors.Control
        Me.Label5.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label5.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label5.Location = New System.Drawing.Point(108, 556)
        Me.Label5.Name = "Label5"
        Me.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label5.Size = New System.Drawing.Size(57, 13)
        Me.Label5.TabIndex = 211
        Me.Label5.Text = "受付日付"
        '
        '_lb_日_5
        '
        Me._lb_日_5.BackColor = System.Drawing.SystemColors.Window
        Me._lb_日_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_日_5.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_日_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_日_5.Location = New System.Drawing.Point(312, 555)
        Me._lb_日_5.Name = "_lb_日_5"
        Me._lb_日_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_日_5.Size = New System.Drawing.Size(12, 14)
        Me._lb_日_5.TabIndex = 210
        Me._lb_日_5.Text = "日"
        Me._lb_日_5.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_月_5
        '
        Me._lb_月_5.BackColor = System.Drawing.SystemColors.Window
        Me._lb_月_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_月_5.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_月_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_月_5.Location = New System.Drawing.Point(274, 555)
        Me._lb_月_5.Name = "_lb_月_5"
        Me._lb_月_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_月_5.Size = New System.Drawing.Size(12, 14)
        Me._lb_月_5.TabIndex = 209
        Me._lb_月_5.Text = "月"
        Me._lb_月_5.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_年_5
        '
        Me._lb_年_5.BackColor = System.Drawing.SystemColors.Window
        Me._lb_年_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_年_5.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_年_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_年_5.Location = New System.Drawing.Point(237, 555)
        Me._lb_年_5.Name = "_lb_年_5"
        Me._lb_年_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_年_5.Size = New System.Drawing.Size(17, 14)
        Me._lb_年_5.TabIndex = 208
        Me._lb_年_5.Text = "年"
        Me._lb_年_5.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_項目_10
        '
        Me._lb_項目_10.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me._lb_項目_10.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_10.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_10.ForeColor = System.Drawing.Color.White
        Me._lb_項目_10.Location = New System.Drawing.Point(704, 188)
        Me._lb_項目_10.Name = "_lb_項目_10"
        Me._lb_項目_10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_10.Size = New System.Drawing.Size(101, 69)
        Me._lb_項目_10.TabIndex = 207
        Me._lb_項目_10.Text = "ﾔｵｺｰ情報"
        '
        'rf_物件略称
        '
        Me.rf_物件略称.BackColor = System.Drawing.SystemColors.Control
        Me.rf_物件略称.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.rf_物件略称.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_物件略称.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_物件略称.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_物件略称.Location = New System.Drawing.Point(245, 48)
        Me.rf_物件略称.Name = "rf_物件略称"
        Me.rf_物件略称.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_物件略称.Size = New System.Drawing.Size(178, 19)
        Me.rf_物件略称.TabIndex = 206
        Me.rf_物件略称.Text = "ＷＷＷＷＷＷＷＷ"
        '
        '_lb_項目_9
        '
        Me._lb_項目_9.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me._lb_項目_9.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_9.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_9.ForeColor = System.Drawing.Color.White
        Me._lb_項目_9.Location = New System.Drawing.Point(4, 44)
        Me._lb_項目_9.Name = "_lb_項目_9"
        Me._lb_項目_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_9.Size = New System.Drawing.Size(101, 45)
        Me._lb_項目_9.TabIndex = 205
        Me._lb_項目_9.Text = "物件情報"
        '
        '_lblLabels_31
        '
        Me._lblLabels_31.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_31.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_31.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_31.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_31.Location = New System.Drawing.Point(812, 164)
        Me._lblLabels_31.Name = "_lblLabels_31"
        Me._lblLabels_31.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_31.Size = New System.Drawing.Size(85, 14)
        Me._lblLabels_31.TabIndex = 200
        Me._lblLabels_31.Text = "売場面積"
        '
        '_lblLabels_30
        '
        Me._lblLabels_30.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_30.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_30.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_30.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_30.Location = New System.Drawing.Point(1008, 164)
        Me._lblLabels_30.Name = "_lblLabels_30"
        Me._lblLabels_30.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_30.Size = New System.Drawing.Size(33, 13)
        Me._lblLabels_30.TabIndex = 199
        Me._lblLabels_30.Text = "坪"
        '
        '_lblLabels_29
        '
        Me._lblLabels_29.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_29.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_29.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_29.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_29.Location = New System.Drawing.Point(812, 144)
        Me._lblLabels_29.Name = "_lblLabels_29"
        Me._lblLabels_29.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_29.Size = New System.Drawing.Size(77, 13)
        Me._lblLabels_29.TabIndex = 198
        Me._lblLabels_29.Text = "物件内容"
        '
        '_lb_項目_7
        '
        Me._lb_項目_7.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me._lb_項目_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_7.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_7.ForeColor = System.Drawing.Color.White
        Me._lb_項目_7.Location = New System.Drawing.Point(704, 96)
        Me._lb_項目_7.Name = "_lb_項目_7"
        Me._lb_項目_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_7.Size = New System.Drawing.Size(101, 89)
        Me._lb_項目_7.TabIndex = 197
        Me._lb_項目_7.Text = "ｳｴﾙｼｱ情報"
        '
        '_lblLabels_28
        '
        Me._lblLabels_28.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_28.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_28.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_28.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_28.Location = New System.Drawing.Point(812, 52)
        Me._lblLabels_28.Name = "_lblLabels_28"
        Me._lblLabels_28.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_28.Size = New System.Drawing.Size(73, 13)
        Me._lblLabels_28.TabIndex = 196
        Me._lblLabels_28.Text = "伝票種類"
        '
        '_lblLabels_27
        '
        Me._lblLabels_27.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_27.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_27.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_27.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_27.Location = New System.Drawing.Point(932, 52)
        Me._lblLabels_27.Name = "_lblLabels_27"
        Me._lblLabels_27.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_27.Size = New System.Drawing.Size(287, 15)
        Me._lblLabels_27.TabIndex = 195
        Me._lblLabels_27.Text = "0:なし 1:消耗 2:直納 3:mail消耗 4:mail直納"
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.SystemColors.Control
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(112, 52)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(61, 13)
        Me.Label4.TabIndex = 194
        Me.Label4.Text = "物件№"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'rf_部署名
        '
        Me.rf_部署名.BackColor = System.Drawing.SystemColors.Control
        Me.rf_部署名.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.rf_部署名.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_部署名.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_部署名.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_部署名.Location = New System.Drawing.Point(487, 96)
        Me.rf_部署名.Name = "rf_部署名"
        Me.rf_部署名.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_部署名.Size = New System.Drawing.Size(130, 19)
        Me.rf_部署名.TabIndex = 193
        Me.rf_部署名.Text = "ＷＷＷＷＷＷＷＷ"
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.SystemColors.Control
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(385, 100)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(37, 13)
        Me.Label3.TabIndex = 192
        Me.Label3.Text = "部署"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lb_項目_8
        '
        Me._lb_項目_8.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me._lb_項目_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_8.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_8.ForeColor = System.Drawing.Color.White
        Me._lb_項目_8.Location = New System.Drawing.Point(4, 352)
        Me._lb_項目_8.Name = "_lb_項目_8"
        Me._lb_項目_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_8.Size = New System.Drawing.Size(101, 69)
        Me._lb_項目_8.TabIndex = 191
        Me._lb_項目_8.Text = "販売先" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(社内在庫用)"
        '
        'rf_得意先別見積番号
        '
        Me.rf_得意先別見積番号.BackColor = System.Drawing.SystemColors.Control
        Me.rf_得意先別見積番号.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_得意先別見積番号.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_得意先別見積番号.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_得意先別見積番号.Location = New System.Drawing.Point(276, 24)
        Me.rf_得意先別見積番号.Name = "rf_得意先別見積番号"
        Me.rf_得意先別見積番号.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_得意先別見積番号.Size = New System.Drawing.Size(101, 13)
        Me.rf_得意先別見積番号.TabIndex = 186
        Me.rf_得意先別見積番号.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.rf_得意先別見積番号.Visible = False
        '
        'rf_外税額
        '
        Me.rf_外税額.BackColor = System.Drawing.SystemColors.Window
        Me.rf_外税額.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_外税額.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.rf_外税額.Location = New System.Drawing.Point(672, 24)
        Me.rf_外税額.Name = "rf_外税額"
        Me.rf_外税額.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_外税額.Size = New System.Drawing.Size(73, 12)
        Me.rf_外税額.TabIndex = 185
        Me.rf_外税額.Text = "100000"
        Me.rf_外税額.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.rf_外税額.Visible = False
        '
        'rf_原価率
        '
        Me.rf_原価率.BackColor = System.Drawing.SystemColors.Window
        Me.rf_原価率.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_原価率.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.rf_原価率.Location = New System.Drawing.Point(632, 24)
        Me.rf_原価率.Name = "rf_原価率"
        Me.rf_原価率.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_原価率.Size = New System.Drawing.Size(33, 12)
        Me.rf_原価率.TabIndex = 184
        Me.rf_原価率.Text = "99.99"
        Me.rf_原価率.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.rf_原価率.Visible = False
        '
        'rf_原価合計
        '
        Me.rf_原価合計.BackColor = System.Drawing.SystemColors.Window
        Me.rf_原価合計.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_原価合計.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.rf_原価合計.Location = New System.Drawing.Point(544, 24)
        Me.rf_原価合計.Name = "rf_原価合計"
        Me.rf_原価合計.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_原価合計.Size = New System.Drawing.Size(73, 12)
        Me.rf_原価合計.TabIndex = 183
        Me.rf_原価合計.Text = "100000"
        Me.rf_原価合計.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.rf_原価合計.Visible = False
        '
        'rf_合計金額
        '
        Me.rf_合計金額.BackColor = System.Drawing.SystemColors.Window
        Me.rf_合計金額.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_合計金額.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.rf_合計金額.Location = New System.Drawing.Point(460, 24)
        Me.rf_合計金額.Name = "rf_合計金額"
        Me.rf_合計金額.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_合計金額.Size = New System.Drawing.Size(73, 12)
        Me.rf_合計金額.TabIndex = 182
        Me.rf_合計金額.Text = "100000"
        Me.rf_合計金額.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.rf_合計金額.Visible = False
        '
        'rf_売上端数
        '
        Me.rf_売上端数.BackColor = System.Drawing.SystemColors.Window
        Me.rf_売上端数.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_売上端数.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.rf_売上端数.Location = New System.Drawing.Point(628, 196)
        Me.rf_売上端数.Name = "rf_売上端数"
        Me.rf_売上端数.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_売上端数.Size = New System.Drawing.Size(11, 12)
        Me.rf_売上端数.TabIndex = 181
        Me.rf_売上端数.Text = "1"
        Me.rf_売上端数.Visible = False
        '
        'rf_消費税端数
        '
        Me.rf_消費税端数.BackColor = System.Drawing.SystemColors.Window
        Me.rf_消費税端数.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_消費税端数.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.rf_消費税端数.Location = New System.Drawing.Point(656, 196)
        Me.rf_消費税端数.Name = "rf_消費税端数"
        Me.rf_消費税端数.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_消費税端数.Size = New System.Drawing.Size(9, 12)
        Me.rf_消費税端数.TabIndex = 180
        Me.rf_消費税端数.Text = "1"
        Me.rf_消費税端数.Visible = False
        '
        'rf_税集計区分
        '
        Me.rf_税集計区分.BackColor = System.Drawing.SystemColors.Window
        Me.rf_税集計区分.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_税集計区分.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.rf_税集計区分.Location = New System.Drawing.Point(560, 196)
        Me.rf_税集計区分.Name = "rf_税集計区分"
        Me.rf_税集計区分.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_税集計区分.Size = New System.Drawing.Size(5, 12)
        Me.rf_税集計区分.TabIndex = 179
        Me.rf_税集計区分.Text = "0"
        Me.rf_税集計区分.Visible = False
        '
        '_lblLabels_25
        '
        Me._lblLabels_25.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_25.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_25.Enabled = False
        Me._lblLabels_25.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_25.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_25.Location = New System.Drawing.Point(1004, 480)
        Me._lblLabels_25.Name = "_lblLabels_25"
        Me._lblLabels_25.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_25.Size = New System.Drawing.Size(21, 13)
        Me._lblLabels_25.TabIndex = 178
        Me._lblLabels_25.Text = "円"
        Me._lblLabels_25.Visible = False
        '
        '_lb_年_4
        '
        Me._lb_年_4.BackColor = System.Drawing.SystemColors.Window
        Me._lb_年_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_年_4.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_年_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_年_4.Location = New System.Drawing.Point(933, 439)
        Me._lb_年_4.Name = "_lb_年_4"
        Me._lb_年_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_年_4.Size = New System.Drawing.Size(17, 14)
        Me._lb_年_4.TabIndex = 176
        Me._lb_年_4.Text = "年"
        Me._lb_年_4.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_月_4
        '
        Me._lb_月_4.BackColor = System.Drawing.SystemColors.Window
        Me._lb_月_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_月_4.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_月_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_月_4.Location = New System.Drawing.Point(970, 439)
        Me._lb_月_4.Name = "_lb_月_4"
        Me._lb_月_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_月_4.Size = New System.Drawing.Size(12, 14)
        Me._lb_月_4.TabIndex = 175
        Me._lb_月_4.Text = "月"
        Me._lb_月_4.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_日_4
        '
        Me._lb_日_4.BackColor = System.Drawing.SystemColors.Window
        Me._lb_日_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_日_4.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_日_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_日_4.Location = New System.Drawing.Point(1008, 439)
        Me._lb_日_4.Name = "_lb_日_4"
        Me._lb_日_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_日_4.Size = New System.Drawing.Size(12, 14)
        Me._lb_日_4.TabIndex = 174
        Me._lb_日_4.Text = "日"
        Me._lb_日_4.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(812, 440)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(57, 13)
        Me.Label2.TabIndex = 173
        Me.Label2.Text = "受注日付"
        '
        '_lblLabels_15
        '
        Me._lblLabels_15.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_15.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_15.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_15.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_15.Location = New System.Drawing.Point(812, 320)
        Me._lblLabels_15.Name = "_lblLabels_15"
        Me._lblLabels_15.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_15.Size = New System.Drawing.Size(82, 13)
        Me._lblLabels_15.TabIndex = 172
        Me._lblLabels_15.Text = "御支払条件"
        '
        '_lblLabels_9
        '
        Me._lblLabels_9.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_9.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_9.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_9.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_9.Location = New System.Drawing.Point(928, 320)
        Me._lblLabels_9.Name = "_lblLabels_9"
        Me._lblLabels_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_9.Size = New System.Drawing.Size(221, 13)
        Me._lblLabels_9.TabIndex = 171
        Me._lblLabels_9.Text = "0:別途御打ち合せによる 1:その他"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(113, 120)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(61, 14)
        Me.Label1.TabIndex = 170
        Me.Label1.Text = "見積日付"
        '
        '_lb_項目_5
        '
        Me._lb_項目_5.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me._lb_項目_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_5.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_5.ForeColor = System.Drawing.Color.White
        Me._lb_項目_5.Location = New System.Drawing.Point(4, 92)
        Me._lb_項目_5.Name = "_lb_項目_5"
        Me._lb_項目_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_5.Size = New System.Drawing.Size(101, 69)
        Me._lb_項目_5.TabIndex = 169
        Me._lb_項目_5.Text = "見積情報"
        '
        '_lblLabels_26
        '
        Me._lblLabels_26.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_26.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_26.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_26.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_26.Location = New System.Drawing.Point(812, 300)
        Me._lblLabels_26.Name = "_lblLabels_26"
        Me._lblLabels_26.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_26.Size = New System.Drawing.Size(81, 13)
        Me._lblLabels_26.TabIndex = 168
        Me._lblLabels_26.Text = "現場名"
        '
        '_lb_日_3
        '
        Me._lb_日_3.BackColor = System.Drawing.SystemColors.Window
        Me._lb_日_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_日_3.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_日_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_日_3.Location = New System.Drawing.Point(292, 119)
        Me._lb_日_3.Name = "_lb_日_3"
        Me._lb_日_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_日_3.Size = New System.Drawing.Size(14, 14)
        Me._lb_日_3.TabIndex = 166
        Me._lb_日_3.Text = "日"
        Me._lb_日_3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_月_3
        '
        Me._lb_月_3.BackColor = System.Drawing.SystemColors.Window
        Me._lb_月_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_月_3.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_月_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_月_3.Location = New System.Drawing.Point(254, 119)
        Me._lb_月_3.Name = "_lb_月_3"
        Me._lb_月_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_月_3.Size = New System.Drawing.Size(14, 14)
        Me._lb_月_3.TabIndex = 165
        Me._lb_月_3.Text = "月"
        Me._lb_月_3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_年_3
        '
        Me._lb_年_3.BackColor = System.Drawing.SystemColors.Window
        Me._lb_年_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_年_3.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_年_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_年_3.Location = New System.Drawing.Point(216, 119)
        Me._lb_年_3.Name = "_lb_年_3"
        Me._lb_年_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_年_3.Size = New System.Drawing.Size(14, 14)
        Me._lb_年_3.TabIndex = 164
        Me._lb_年_3.Text = "年"
        Me._lb_年_3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_月_1
        '
        Me._lb_月_1.BackColor = System.Drawing.SystemColors.Window
        Me._lb_月_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_月_1.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_月_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_月_1.Location = New System.Drawing.Point(339, 431)
        Me._lb_月_1.Name = "_lb_月_1"
        Me._lb_月_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_月_1.Size = New System.Drawing.Size(12, 14)
        Me._lb_月_1.TabIndex = 163
        Me._lb_月_1.Text = "月"
        Me._lb_月_1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblLabels_24
        '
        Me._lblLabels_24.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_24.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_24.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_24.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_24.Location = New System.Drawing.Point(812, 460)
        Me._lblLabels_24.Name = "_lblLabels_24"
        Me._lblLabels_24.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_24.Size = New System.Drawing.Size(85, 13)
        Me._lblLabels_24.TabIndex = 162
        Me._lblLabels_24.Text = "大小口区分"
        '
        '_lblLabels_23
        '
        Me._lblLabels_23.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_23.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_23.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_23.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_23.Location = New System.Drawing.Point(928, 460)
        Me._lblLabels_23.Name = "_lblLabels_23"
        Me._lblLabels_23.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_23.Size = New System.Drawing.Size(169, 13)
        Me._lblLabels_23.TabIndex = 161
        Me._lblLabels_23.Text = "0:大口 1:小口"
        '
        '_lblLabels_22
        '
        Me._lblLabels_22.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_22.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_22.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_22.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_22.Location = New System.Drawing.Point(812, 420)
        Me._lblLabels_22.Name = "_lblLabels_22"
        Me._lblLabels_22.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_22.Size = New System.Drawing.Size(85, 13)
        Me._lblLabels_22.TabIndex = 160
        Me._lblLabels_22.Text = "受注区分"
        '
        '_lblLabels_21
        '
        Me._lblLabels_21.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_21.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_21.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_21.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_21.Location = New System.Drawing.Point(928, 420)
        Me._lblLabels_21.Name = "_lblLabels_21"
        Me._lblLabels_21.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_21.Size = New System.Drawing.Size(169, 13)
        Me._lblLabels_21.TabIndex = 159
        Me._lblLabels_21.Text = "0:仮 1:確定"
        '
        '_lblLabels_20
        '
        Me._lblLabels_20.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_20.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_20.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_20.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_20.Location = New System.Drawing.Point(308, 516)
        Me._lblLabels_20.Name = "_lblLabels_20"
        Me._lblLabels_20.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_20.Size = New System.Drawing.Size(39, 13)
        Me._lblLabels_20.TabIndex = 158
        Me._lblLabels_20.Text = "千円"
        '
        'rf_担当者名
        '
        Me.rf_担当者名.BackColor = System.Drawing.SystemColors.Control
        Me.rf_担当者名.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.rf_担当者名.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_担当者名.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_担当者名.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_担当者名.Location = New System.Drawing.Point(236, 96)
        Me.rf_担当者名.Name = "rf_担当者名"
        Me.rf_担当者名.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_担当者名.Size = New System.Drawing.Size(130, 19)
        Me.rf_担当者名.TabIndex = 157
        Me.rf_担当者名.Text = "ＷＷＷＷＷＷＷＷ"
        '
        'lb_担当者CD
        '
        Me.lb_担当者CD.BackColor = System.Drawing.SystemColors.Control
        Me.lb_担当者CD.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_担当者CD.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_担当者CD.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_担当者CD.Location = New System.Drawing.Point(113, 100)
        Me.lb_担当者CD.Name = "lb_担当者CD"
        Me.lb_担当者CD.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_担当者CD.Size = New System.Drawing.Size(52, 14)
        Me.lb_担当者CD.TabIndex = 156
        Me.lb_担当者CD.Text = "担当者"
        '
        '_lblLabels_19
        '
        Me._lblLabels_19.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_19.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_19.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_19.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_19.Location = New System.Drawing.Point(1196, 399)
        Me._lblLabels_19.Name = "_lblLabels_19"
        Me._lblLabels_19.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_19.Size = New System.Drawing.Size(27, 18)
        Me._lblLabels_19.TabIndex = 155
        Me._lblLabels_19.Text = "日"
        '
        '_lblLabels_18
        '
        Me._lblLabels_18.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_18.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_18.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_18.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_18.Location = New System.Drawing.Point(1097, 400)
        Me._lblLabels_18.Name = "_lblLabels_18"
        Me._lblLabels_18.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_18.Size = New System.Drawing.Size(67, 18)
        Me._lblLabels_18.TabIndex = 154
        Me._lblLabels_18.Text = "有効期限"
        '
        '_lblLabels_17
        '
        Me._lblLabels_17.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_17.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_17.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_17.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_17.Location = New System.Drawing.Point(928, 400)
        Me._lblLabels_17.Name = "_lblLabels_17"
        Me._lblLabels_17.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_17.Size = New System.Drawing.Size(169, 13)
        Me._lblLabels_17.TabIndex = 153
        Me._lblLabels_17.Text = "0:出力しない 1:出力する"
        '
        '_lblLabels_16
        '
        Me._lblLabels_16.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_16.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_16.Font = New System.Drawing.Font("ＭＳ ゴシック", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_16.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_16.Location = New System.Drawing.Point(928, 360)
        Me._lblLabels_16.Name = "_lblLabels_16"
        Me._lblLabels_16.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_16.Size = New System.Drawing.Size(298, 13)
        Me._lblLabels_16.TabIndex = 152
        Me._lblLabels_16.Text = "0:納期表示 1:別途御打ち合せによる 2:その他"
        '
        '_lblLabels_14
        '
        Me._lblLabels_14.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_14.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_14.Enabled = False
        Me._lblLabels_14.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_14.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_14.Location = New System.Drawing.Point(812, 480)
        Me._lblLabels_14.Name = "_lblLabels_14"
        Me._lblLabels_14.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_14.Size = New System.Drawing.Size(81, 13)
        Me._lblLabels_14.TabIndex = 151
        Me._lblLabels_14.Text = "出精値引"
        Me._lblLabels_14.Visible = False
        '
        '_lblLabels_13
        '
        Me._lblLabels_13.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_13.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_13.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_13.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_13.Location = New System.Drawing.Point(812, 400)
        Me._lblLabels_13.Name = "_lblLabels_13"
        Me._lblLabels_13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_13.Size = New System.Drawing.Size(85, 13)
        Me._lblLabels_13.TabIndex = 150
        Me._lblLabels_13.Text = "見積日出力"
        '
        '_lblLabels_12
        '
        Me._lblLabels_12.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_12.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_12.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_12.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_12.Location = New System.Drawing.Point(236, 536)
        Me._lblLabels_12.Name = "_lblLabels_12"
        Me._lblLabels_12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_12.Size = New System.Drawing.Size(433, 13)
        Me._lblLabels_12.TabIndex = 149
        Me._lblLabels_12.Text = "0:新店 1:改装 2:メンテ 6:委託"
        '
        '_lblLabels_11
        '
        Me._lblLabels_11.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_11.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_11.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_11.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_11.Location = New System.Drawing.Point(812, 360)
        Me._lblLabels_11.Name = "_lblLabels_11"
        Me._lblLabels_11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_11.Size = New System.Drawing.Size(81, 13)
        Me._lblLabels_11.TabIndex = 148
        Me._lblLabels_11.Text = "納期表示"
        '
        '_lblLabels_8
        '
        Me._lblLabels_8.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_8.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_8.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_8.Location = New System.Drawing.Point(108, 536)
        Me._lblLabels_8.Name = "_lblLabels_8"
        Me._lblLabels_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_8.Size = New System.Drawing.Size(73, 13)
        Me._lblLabels_8.TabIndex = 147
        Me._lblLabels_8.Text = "物件種別"
        '
        '_lblLabels_7
        '
        Me._lblLabels_7.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_7.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_7.Location = New System.Drawing.Point(360, 516)
        Me._lblLabels_7.Name = "_lblLabels_7"
        Me._lblLabels_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_7.Size = New System.Drawing.Size(81, 13)
        Me._lblLabels_7.TabIndex = 146
        Me._lblLabels_7.Text = "オープン日"
        '
        '_lblLabels_6
        '
        Me._lblLabels_6.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_6.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_6.Location = New System.Drawing.Point(108, 516)
        Me._lblLabels_6.Name = "_lblLabels_6"
        Me._lblLabels_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_6.Size = New System.Drawing.Size(85, 13)
        Me._lblLabels_6.TabIndex = 145
        Me._lblLabels_6.Text = "物件規模金額"
        '
        '_lb_日_2
        '
        Me._lb_日_2.BackColor = System.Drawing.SystemColors.Window
        Me._lb_日_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_日_2.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_日_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_日_2.Location = New System.Drawing.Point(560, 515)
        Me._lb_日_2.Name = "_lb_日_2"
        Me._lb_日_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_日_2.Size = New System.Drawing.Size(12, 14)
        Me._lb_日_2.TabIndex = 143
        Me._lb_日_2.Text = "日"
        Me._lb_日_2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_月_2
        '
        Me._lb_月_2.BackColor = System.Drawing.SystemColors.Window
        Me._lb_月_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_月_2.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_月_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_月_2.Location = New System.Drawing.Point(524, 515)
        Me._lb_月_2.Name = "_lb_月_2"
        Me._lb_月_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_月_2.Size = New System.Drawing.Size(12, 14)
        Me._lb_月_2.TabIndex = 142
        Me._lb_月_2.Text = "月"
        Me._lb_月_2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_年_2
        '
        Me._lb_年_2.BackColor = System.Drawing.SystemColors.Window
        Me._lb_年_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_年_2.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_年_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_年_2.Location = New System.Drawing.Point(487, 515)
        Me._lb_年_2.Name = "_lb_年_2"
        Me._lb_年_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_年_2.Size = New System.Drawing.Size(17, 14)
        Me._lb_年_2.TabIndex = 141
        Me._lb_年_2.Text = "年"
        Me._lb_年_2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_kara_0
        '
        Me._lb_kara_0.BackColor = System.Drawing.SystemColors.Control
        Me._lb_kara_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_kara_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_kara_0.Location = New System.Drawing.Point(247, 433)
        Me._lb_kara_0.Name = "_lb_kara_0"
        Me._lb_kara_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_kara_0.Size = New System.Drawing.Size(17, 12)
        Me._lb_kara_0.TabIndex = 139
        Me._lb_kara_0.Text = "～"
        Me._lb_kara_0.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_年_0
        '
        Me._lb_年_0.BackColor = System.Drawing.SystemColors.Window
        Me._lb_年_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_年_0.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_年_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_年_0.Location = New System.Drawing.Point(149, 431)
        Me._lb_年_0.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_年_0.Name = "_lb_年_0"
        Me._lb_年_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_年_0.Size = New System.Drawing.Size(14, 14)
        Me._lb_年_0.TabIndex = 136
        Me._lb_年_0.Text = "年"
        Me._lb_年_0.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_月_0
        '
        Me._lb_月_0.BackColor = System.Drawing.SystemColors.Window
        Me._lb_月_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_月_0.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_月_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_月_0.Location = New System.Drawing.Point(191, 431)
        Me._lb_月_0.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_月_0.Name = "_lb_月_0"
        Me._lb_月_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_月_0.Size = New System.Drawing.Size(14, 14)
        Me._lb_月_0.TabIndex = 137
        Me._lb_月_0.Text = "月"
        Me._lb_月_0.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_日_0
        '
        Me._lb_日_0.BackColor = System.Drawing.SystemColors.Window
        Me._lb_日_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_日_0.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_日_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_日_0.Location = New System.Drawing.Point(224, 431)
        Me._lb_日_0.Name = "_lb_日_0"
        Me._lb_日_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_日_0.Size = New System.Drawing.Size(14, 14)
        Me._lb_日_0.TabIndex = 138
        Me._lb_日_0.Text = "日"
        Me._lb_日_0.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_年_1
        '
        Me._lb_年_1.BackColor = System.Drawing.SystemColors.Window
        Me._lb_年_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_年_1.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_年_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_年_1.Location = New System.Drawing.Point(302, 431)
        Me._lb_年_1.Name = "_lb_年_1"
        Me._lb_年_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_年_1.Size = New System.Drawing.Size(14, 14)
        Me._lb_年_1.TabIndex = 134
        Me._lb_年_1.Text = "年"
        Me._lb_年_1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_日_1
        '
        Me._lb_日_1.BackColor = System.Drawing.SystemColors.Window
        Me._lb_日_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_日_1.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_日_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_日_1.Location = New System.Drawing.Point(375, 431)
        Me._lb_日_1.Name = "_lb_日_1"
        Me._lb_日_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_日_1.Size = New System.Drawing.Size(12, 14)
        Me._lb_日_1.TabIndex = 133
        Me._lb_日_1.Text = "日"
        Me._lb_日_1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblLabels_5
        '
        Me._lblLabels_5.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_5.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_5.Location = New System.Drawing.Point(159, 328)
        Me._lblLabels_5.Name = "_lblLabels_5"
        Me._lblLabels_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_5.Size = New System.Drawing.Size(55, 13)
        Me._lblLabels_5.TabIndex = 132
        Me._lblLabels_5.Text = "担当者"
        '
        '_lblLabels_4
        '
        Me._lblLabels_4.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_4.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_4.Location = New System.Drawing.Point(185, 308)
        Me._lblLabels_4.Name = "_lblLabels_4"
        Me._lblLabels_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_4.Size = New System.Drawing.Size(31, 13)
        Me._lblLabels_4.TabIndex = 131
        Me._lblLabels_4.Text = "TEL"
        '
        '_lblLabels_3
        '
        Me._lblLabels_3.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_3.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_3.Location = New System.Drawing.Point(372, 308)
        Me._lblLabels_3.Name = "_lblLabels_3"
        Me._lblLabels_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_3.Size = New System.Drawing.Size(32, 13)
        Me._lblLabels_3.TabIndex = 130
        Me._lblLabels_3.Text = "FAX"
        '
        '_lblLabels_2
        '
        Me._lblLabels_2.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_2.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_2.Location = New System.Drawing.Point(112, 288)
        Me._lblLabels_2.Name = "_lblLabels_2"
        Me._lblLabels_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_2.Size = New System.Drawing.Size(41, 18)
        Me._lblLabels_2.TabIndex = 129
        Me._lblLabels_2.Text = "住所"
        '
        '_lblLabels_0
        '
        Me._lblLabels_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_0.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_0.Location = New System.Drawing.Point(327, 196)
        Me._lblLabels_0.Name = "_lblLabels_0"
        Me._lblLabels_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_0.Size = New System.Drawing.Size(31, 13)
        Me._lblLabels_0.TabIndex = 128
        Me._lblLabels_0.Text = "FAX"
        '
        '_lblLabels_10
        '
        Me._lblLabels_10.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_10.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_10.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_10.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_10.Location = New System.Drawing.Point(145, 196)
        Me._lblLabels_10.Name = "_lblLabels_10"
        Me._lblLabels_10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_10.Size = New System.Drawing.Size(31, 13)
        Me._lblLabels_10.TabIndex = 127
        Me._lblLabels_10.Text = "TEL"
        '
        'lb_見積番号
        '
        Me.lb_見積番号.BackColor = System.Drawing.SystemColors.Control
        Me.lb_見積番号.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_見積番号.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_見積番号.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_見積番号.Location = New System.Drawing.Point(12, 24)
        Me.lb_見積番号.Name = "lb_見積番号"
        Me.lb_見積番号.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_見積番号.Size = New System.Drawing.Size(57, 13)
        Me.lb_見積番号.TabIndex = 126
        Me.lb_見積番号.Text = "見積№"
        '
        'rf_見積番号
        '
        Me.rf_見積番号.BackColor = System.Drawing.SystemColors.Control
        Me.rf_見積番号.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_見積番号.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_見積番号.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_見積番号.Location = New System.Drawing.Point(100, 24)
        Me.rf_見積番号.Name = "rf_見積番号"
        Me.rf_見積番号.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_見積番号.Size = New System.Drawing.Size(101, 13)
        Me.rf_見積番号.TabIndex = 125
        Me.rf_見積番号.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lb_見積件名
        '
        Me.lb_見積件名.BackColor = System.Drawing.SystemColors.Control
        Me.lb_見積件名.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_見積件名.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lb_見積件名.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_見積件名.Location = New System.Drawing.Point(112, 140)
        Me.lb_見積件名.Name = "lb_見積件名"
        Me.lb_見積件名.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_見積件名.Size = New System.Drawing.Size(67, 14)
        Me.lb_見積件名.TabIndex = 124
        Me.lb_見積件名.Text = "見積件名"
        '
        '_lb_項目_0
        '
        Me._lb_項目_0.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me._lb_項目_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_0.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_0.ForeColor = System.Drawing.Color.White
        Me._lb_項目_0.Location = New System.Drawing.Point(4, 164)
        Me._lb_項目_0.Name = "_lb_項目_0"
        Me._lb_項目_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_0.Size = New System.Drawing.Size(101, 93)
        Me._lb_項目_0.TabIndex = 123
        Me._lb_項目_0.Text = "得意先"
        '
        '_lb_項目_6
        '
        Me._lb_項目_6.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me._lb_項目_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_6.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_6.ForeColor = System.Drawing.Color.White
        Me._lb_項目_6.Location = New System.Drawing.Point(704, 292)
        Me._lb_項目_6.Name = "_lb_項目_6"
        Me._lb_項目_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_6.Size = New System.Drawing.Size(101, 209)
        Me._lb_項目_6.TabIndex = 118
        Me._lb_項目_6.Text = "見積" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "コントロール"
        '
        '_lb_項目_2
        '
        Me._lb_項目_2.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me._lb_項目_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_2.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_2.ForeColor = System.Drawing.Color.White
        Me._lb_項目_2.Location = New System.Drawing.Point(4, 488)
        Me._lb_項目_2.Name = "_lb_項目_2"
        Me._lb_項目_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_2.Size = New System.Drawing.Size(101, 149)
        Me._lb_項目_2.TabIndex = 117
        Me._lb_項目_2.Text = "物件情報"
        '
        '_lb_項目_3
        '
        Me._lb_項目_3.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me._lb_項目_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_3.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_3.ForeColor = System.Drawing.Color.White
        Me._lb_項目_3.Location = New System.Drawing.Point(4, 424)
        Me._lb_項目_3.Name = "_lb_項目_3"
        Me._lb_項目_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_3.Size = New System.Drawing.Size(101, 29)
        Me._lb_項目_3.TabIndex = 116
        Me._lb_項目_3.Text = "納期"
        '
        '_lb_項目_4
        '
        Me._lb_項目_4.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me._lb_項目_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_4.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_4.ForeColor = System.Drawing.Color.White
        Me._lb_項目_4.Location = New System.Drawing.Point(4, 456)
        Me._lb_項目_4.Name = "_lb_項目_4"
        Me._lb_項目_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_4.Size = New System.Drawing.Size(101, 29)
        Me._lb_項目_4.TabIndex = 115
        Me._lb_項目_4.Text = "備考"
        '
        '_lb_項目_1
        '
        Me._lb_項目_1.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._lb_項目_1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me._lb_項目_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_項目_1.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lb_項目_1.ForeColor = System.Drawing.Color.White
        Me._lb_項目_1.Location = New System.Drawing.Point(4, 260)
        Me._lb_項目_1.Name = "_lb_項目_1"
        Me._lb_項目_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_項目_1.Size = New System.Drawing.Size(101, 89)
        Me._lb_項目_1.TabIndex = 114
        Me._lb_項目_1.Text = "納入先"
        '
        'rf_処理区分
        '
        Me.rf_処理区分.BackColor = System.Drawing.SystemColors.Control
        Me.rf_処理区分.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.rf_処理区分.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_処理区分.Font = New System.Drawing.Font("ＭＳ ゴシック", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_処理区分.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_処理区分.Location = New System.Drawing.Point(4, 4)
        Me.rf_処理区分.Name = "rf_処理区分"
        Me.rf_処理区分.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_処理区分.Size = New System.Drawing.Size(81, 19)
        Me.rf_処理区分.TabIndex = 113
        Me.rf_処理区分.Text = "≪登 録≫"
        Me.rf_処理区分.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lb_納期_0
        '
        Me._lb_納期_0.BackColor = System.Drawing.SystemColors.Window
        Me._lb_納期_0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_納期_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_納期_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_納期_0.Location = New System.Drawing.Point(112, 428)
        Me._lb_納期_0.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._lb_納期_0.Name = "_lb_納期_0"
        Me._lb_納期_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_納期_0.Size = New System.Drawing.Size(127, 19)
        Me._lb_納期_0.TabIndex = 135
        '
        '_lb_納期_1
        '
        Me._lb_納期_1.BackColor = System.Drawing.SystemColors.Window
        Me._lb_納期_1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_納期_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_納期_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_納期_1.Location = New System.Drawing.Point(265, 428)
        Me._lb_納期_1.Name = "_lb_納期_1"
        Me._lb_納期_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_納期_1.Size = New System.Drawing.Size(127, 19)
        Me._lb_納期_1.TabIndex = 140
        '
        'lb_OPEN日
        '
        Me.lb_OPEN日.BackColor = System.Drawing.SystemColors.Window
        Me.lb_OPEN日.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lb_OPEN日.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_OPEN日.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_OPEN日.Location = New System.Drawing.Point(448, 512)
        Me.lb_OPEN日.Name = "lb_OPEN日"
        Me.lb_OPEN日.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_OPEN日.Size = New System.Drawing.Size(127, 19)
        Me.lb_OPEN日.TabIndex = 144
        '
        'lb_見積日付
        '
        Me.lb_見積日付.BackColor = System.Drawing.SystemColors.Window
        Me.lb_見積日付.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lb_見積日付.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_見積日付.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_見積日付.Location = New System.Drawing.Point(180, 116)
        Me.lb_見積日付.Name = "lb_見積日付"
        Me.lb_見積日付.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_見積日付.Size = New System.Drawing.Size(127, 19)
        Me.lb_見積日付.TabIndex = 167
        '
        'lb_受注日付
        '
        Me.lb_受注日付.BackColor = System.Drawing.SystemColors.Window
        Me.lb_受注日付.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lb_受注日付.Cursor = System.Windows.Forms.Cursors.Default
        Me.lb_受注日付.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lb_受注日付.Location = New System.Drawing.Point(896, 436)
        Me.lb_受注日付.Name = "lb_受注日付"
        Me.lb_受注日付.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lb_受注日付.Size = New System.Drawing.Size(127, 19)
        Me.lb_受注日付.TabIndex = 177
        '
        'Label6
        '
        Me.Label6.BackColor = System.Drawing.SystemColors.Window
        Me.Label6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label6.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label6.Location = New System.Drawing.Point(200, 552)
        Me.Label6.Name = "Label6"
        Me.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label6.Size = New System.Drawing.Size(127, 19)
        Me.Label6.TabIndex = 212
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.SystemColors.Window
        Me.Label8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label8.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label8.Enabled = False
        Me.Label8.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label8.Location = New System.Drawing.Point(200, 572)
        Me.Label8.Name = "Label8"
        Me.Label8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label8.Size = New System.Drawing.Size(127, 19)
        Me.Label8.TabIndex = 217
        Me.Label8.Visible = False
        '
        'Label11
        '
        Me.Label11.BackColor = System.Drawing.SystemColors.Window
        Me.Label11.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label11.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label11.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label11.Location = New System.Drawing.Point(896, 548)
        Me.Label11.Name = "Label11"
        Me.Label11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label11.Size = New System.Drawing.Size(127, 19)
        Me.Label11.TabIndex = 243
        '
        'rf_ウエルシア物件区分名
        '
        Me.rf_ウエルシア物件区分名.BackColor = System.Drawing.SystemColors.Control
        Me.rf_ウエルシア物件区分名.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.rf_ウエルシア物件区分名.Cursor = System.Windows.Forms.Cursors.Default
        Me.rf_ウエルシア物件区分名.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.rf_ウエルシア物件区分名.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rf_ウエルシア物件区分名.Location = New System.Drawing.Point(928, 120)
        Me.rf_ウエルシア物件区分名.Name = "rf_ウエルシア物件区分名"
        Me.rf_ウエルシア物件区分名.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.rf_ウエルシア物件区分名.Size = New System.Drawing.Size(214, 19)
        Me.rf_ウエルシア物件区分名.TabIndex = 204
        Me.rf_ウエルシア物件区分名.Text = "ＷＷＷＷＷＷＷＷ"
        '
        '_lblLabels_34
        '
        Me._lblLabels_34.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_34.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_34.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_34.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_34.Location = New System.Drawing.Point(812, 124)
        Me._lblLabels_34.Name = "_lblLabels_34"
        Me._lblLabels_34.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_34.Size = New System.Drawing.Size(77, 13)
        Me._lblLabels_34.TabIndex = 203
        Me._lblLabels_34.Text = "請求管轄"
        '
        '_lblLabels_33
        '
        Me._lblLabels_33.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_33.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_33.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_33.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_33.Location = New System.Drawing.Point(936, 104)
        Me._lblLabels_33.Name = "_lblLabels_33"
        Me._lblLabels_33.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_33.Size = New System.Drawing.Size(165, 13)
        Me._lblLabels_33.TabIndex = 202
        Me._lblLabels_33.Text = "1:通常 2:リース"
        '
        '_lblLabels_32
        '
        Me._lblLabels_32.BackColor = System.Drawing.SystemColors.Control
        Me._lblLabels_32.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabels_32.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me._lblLabels_32.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabels_32.Location = New System.Drawing.Point(812, 104)
        Me._lblLabels_32.Name = "_lblLabels_32"
        Me._lblLabels_32.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabels_32.Size = New System.Drawing.Size(87, 13)
        Me._lblLabels_32.TabIndex = 201
        Me._lblLabels_32.Text = "リース区分"
        '
        '_lb_コンテナ_1
        '
        Me._lb_コンテナ_1.BackColor = System.Drawing.Color.White
        Me._lb_コンテナ_1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lb_コンテナ_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lb_コンテナ_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lb_コンテナ_1.Location = New System.Drawing.Point(896, 571)
        Me._lb_コンテナ_1.Name = "_lb_コンテナ_1"
        Me._lb_コンテナ_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lb_コンテナ_1.Size = New System.Drawing.Size(127, 20)
        Me._lb_コンテナ_1.TabIndex = 251
        '
        'Label16
        '
        Me.Label16.BackColor = System.Drawing.SystemColors.Control
        Me.Label16.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label16.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label16.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label16.Location = New System.Drawing.Point(108, 72)
        Me.Label16.Name = "Label16"
        Me.Label16.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label16.Size = New System.Drawing.Size(69, 14)
        Me.Label16.TabIndex = 263
        Me.Label16.Text = "統合見積№"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Timer1
        '
        '
        'SnwMT01F00
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(1232, 709)
        Me.Controls.Add(Me.ck_社内伝票扱い)
        Me.Controls.Add(Me.tx_受注日付Y)
        Me.Controls.Add(Me.tx_受注日付M)
        Me.Controls.Add(Me.tx_受注日付D)
        Me.Controls.Add(Me.PicFunction)
        Me.Controls.Add(Me.sb_Msg)
        Me.Controls.Add(Me.tx_得意先名2)
        Me.Controls.Add(Me.tx_備考)
        Me.Controls.Add(Me.tx_得意先名1)
        Me.Controls.Add(Me.tx_物件金額)
        Me.Controls.Add(Me.tx_出精値引)
        Me.Controls.Add(Me.tx_見積件名)
        Me.Controls.Add(Me.tx_得TEL)
        Me.Controls.Add(Me.tx_得FAX)
        Me.Controls.Add(Me.tx_得担当者)
        Me.Controls.Add(Me.tx_納入先CD)
        Me.Controls.Add(Me.tx_納得意先CD)
        Me.Controls.Add(Me.tx_納入先名2)
        Me.Controls.Add(Me.tx_納入先名1)
        Me.Controls.Add(Me.tx_郵便番号)
        Me.Controls.Add(Me.tx_納住所1)
        Me.Controls.Add(Me.tx_納住所2)
        Me.Controls.Add(Me.tx_納TEL)
        Me.Controls.Add(Me.tx_納FAX)
        Me.Controls.Add(Me.tx_納担当者)
        Me.Controls.Add(Me.tx_s納期D)
        Me.Controls.Add(Me.tx_s納期M)
        Me.Controls.Add(Me.tx_s納期Y)
        Me.Controls.Add(Me.tx_e納期D)
        Me.Controls.Add(Me.tx_e納期M)
        Me.Controls.Add(Me.tx_e納期Y)
        Me.Controls.Add(Me.tx_得意先CD)
        Me.Controls.Add(Me.tx_OPEN日D)
        Me.Controls.Add(Me.tx_OPEN日M)
        Me.Controls.Add(Me.tx_OPEN日Y)
        Me.Controls.Add(Me.tx_物件種別)
        Me.Controls.Add(Me.tx_納期表示)
        Me.Controls.Add(Me.tx_納期表示他)
        Me.Controls.Add(Me.tx_出力日)
        Me.Controls.Add(Me.tx_有効期限)
        Me.Controls.Add(Me.tx_受注区分)
        Me.Controls.Add(Me.tx_大小口区分)
        Me.Controls.Add(Me.tx_見積日付D)
        Me.Controls.Add(Me.tx_見積日付M)
        Me.Controls.Add(Me.tx_見積日付Y)
        Me.Controls.Add(Me.tx_現場名)
        Me.Controls.Add(Me.tx_支払条件)
        Me.Controls.Add(Me.tx_支払条件他)
        Me.Controls.Add(Me.tx_担当者CD)
        Me.Controls.Add(Me.rf_販売先得意先名2)
        Me.Controls.Add(Me.rf_販売先得意先名1)
        Me.Controls.Add(Me.tx_販売先得意先CD)
        Me.Controls.Add(Me.tx_販売先納入先CD)
        Me.Controls.Add(Me.tx_販売先納得意先CD)
        Me.Controls.Add(Me.rf_販売先納入先名2)
        Me.Controls.Add(Me.rf_販売先納入先名1)
        Me.Controls.Add(Me.tx_部署CD)
        Me.Controls.Add(Me.tx_物件番号)
        Me.Controls.Add(Me.tx_伝票種類)
        Me.Controls.Add(Me.tx_ウエルシア物件内容名)
        Me.Controls.Add(Me.tx_ウエルシア物件内容CD)
        Me.Controls.Add(Me.tx_ウエルシア売場面積)
        Me.Controls.Add(Me.tx_受付日付Y)
        Me.Controls.Add(Me.tx_受付日付M)
        Me.Controls.Add(Me.tx_受付日付D)
        Me.Controls.Add(Me.tx_完工日付Y)
        Me.Controls.Add(Me.tx_完工日付M)
        Me.Controls.Add(Me.tx_完工日付D)
        Me.Controls.Add(Me.tx_発注担当者名)
        Me.Controls.Add(Me.tx_作業内容)
        Me.Controls.Add(Me.tx_YKサプライ区分)
        Me.Controls.Add(Me.tx_YK物件区分)
        Me.Controls.Add(Me.tx_YK請求区分)
        Me.Controls.Add(Me.tx_化粧品メーカー区分)
        Me.Controls.Add(Me.tx_SM内容区分)
        Me.Controls.Add(Me.tx_税集計区分)
        Me.Controls.Add(Me.tx_クレーム区分)
        Me.Controls.Add(Me.tx_工事担当CD)
        Me.Controls.Add(Me.tx_発注書発行日付)
        Me.Controls.Add(Me.tx_完了者名)
        Me.Controls.Add(Me.tx_見積確定区分)
        Me.Controls.Add(Me.tx_経過備考1)
        Me.Controls.Add(Me.tx_ウエルシア物件区分)
        Me.Controls.Add(Me.tx_ウエルシアリース区分)
        Me.Controls.Add(Me.tx_経過備考2)
        Me.Controls.Add(Me.tx_請求予定M)
        Me.Controls.Add(Me.tx_請求予定D)
        Me.Controls.Add(Me.tx_完了日Y)
        Me.Controls.Add(Me.tx_完了日M)
        Me.Controls.Add(Me.tx_完了日D)
        Me.Controls.Add(Me.tx_請求予定Y)
        Me.Controls.Add(Me.tx_集計CD)
        Me.Controls.Add(Me.tx_B請求管轄区分)
        Me.Controls.Add(Me.tx_BtoB番号)
        Me.Controls.Add(Me.tx_業種区分)
        Me.Controls.Add(Me.tx_統合見積番号)
        Me.Controls.Add(Me.rf_統合見積件名)
        Me.Controls.Add(Me._lblLabels_58)
        Me.Controls.Add(Me._lblLabels_57)
        Me.Controls.Add(Me._lblLabels_56)
        Me.Controls.Add(Me._lblLabels_55)
        Me.Controls.Add(Me._lblLabels_54)
        Me.Controls.Add(Me._lb_項目_15)
        Me.Controls.Add(Me.rf_集計名)
        Me.Controls.Add(Me._lblLabels_53)
        Me.Controls.Add(Me._lb_日_8)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me._lb_項目_14)
        Me.Controls.Add(Me._lb_項目_13)
        Me.Controls.Add(Me._lblLabels_52)
        Me.Controls.Add(Me._lb_項目_12)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me._lblLabels_51)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me._lb_日_7)
        Me.Controls.Add(Me._lb_月_7)
        Me.Controls.Add(Me._lb_年_7)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.rf_工事担当名)
        Me.Controls.Add(Me._lblLabels_50)
        Me.Controls.Add(Me._lblLabels_49)
        Me.Controls.Add(Me._lblLabels_48)
        Me.Controls.Add(Me._lblLabels_47)
        Me.Controls.Add(Me._lblLabels_1)
        Me.Controls.Add(Me.rf_税集計区分名)
        Me.Controls.Add(Me._lblLabels_46)
        Me.Controls.Add(Me._lblLabels_45)
        Me.Controls.Add(Me._lblLabels_44)
        Me.Controls.Add(Me._lblLabels_43)
        Me.Controls.Add(Me._lb_項目_11)
        Me.Controls.Add(Me._lblLabels_42)
        Me.Controls.Add(Me._lblLabels_41)
        Me.Controls.Add(Me._lblLabels_40)
        Me.Controls.Add(Me._lblLabels_39)
        Me.Controls.Add(Me._lblLabels_36)
        Me.Controls.Add(Me._lblLabels_35)
        Me.Controls.Add(Me._lblLabels_38)
        Me.Controls.Add(Me._lblLabels_37)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me._lb_日_6)
        Me.Controls.Add(Me._lb_月_6)
        Me.Controls.Add(Me._lb_年_6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me._lb_日_5)
        Me.Controls.Add(Me._lb_月_5)
        Me.Controls.Add(Me._lb_年_5)
        Me.Controls.Add(Me._lb_項目_10)
        Me.Controls.Add(Me.rf_物件略称)
        Me.Controls.Add(Me._lb_項目_9)
        Me.Controls.Add(Me._lblLabels_31)
        Me.Controls.Add(Me._lblLabels_30)
        Me.Controls.Add(Me._lblLabels_29)
        Me.Controls.Add(Me._lb_項目_7)
        Me.Controls.Add(Me._lblLabels_28)
        Me.Controls.Add(Me._lblLabels_27)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.rf_部署名)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me._lb_項目_8)
        Me.Controls.Add(Me.rf_得意先別見積番号)
        Me.Controls.Add(Me.rf_外税額)
        Me.Controls.Add(Me.rf_原価率)
        Me.Controls.Add(Me.rf_原価合計)
        Me.Controls.Add(Me.rf_合計金額)
        Me.Controls.Add(Me.rf_売上端数)
        Me.Controls.Add(Me.rf_消費税端数)
        Me.Controls.Add(Me.rf_税集計区分)
        Me.Controls.Add(Me._lblLabels_25)
        Me.Controls.Add(Me._lb_年_4)
        Me.Controls.Add(Me._lb_月_4)
        Me.Controls.Add(Me._lb_日_4)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me._lblLabels_15)
        Me.Controls.Add(Me._lblLabels_9)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me._lb_項目_5)
        Me.Controls.Add(Me._lblLabels_26)
        Me.Controls.Add(Me._lb_日_3)
        Me.Controls.Add(Me._lb_月_3)
        Me.Controls.Add(Me._lb_年_3)
        Me.Controls.Add(Me._lb_月_1)
        Me.Controls.Add(Me._lblLabels_24)
        Me.Controls.Add(Me._lblLabels_23)
        Me.Controls.Add(Me._lblLabels_22)
        Me.Controls.Add(Me._lblLabels_21)
        Me.Controls.Add(Me._lblLabels_20)
        Me.Controls.Add(Me.rf_担当者名)
        Me.Controls.Add(Me.lb_担当者CD)
        Me.Controls.Add(Me._lblLabels_19)
        Me.Controls.Add(Me._lblLabels_18)
        Me.Controls.Add(Me._lblLabels_17)
        Me.Controls.Add(Me._lblLabels_16)
        Me.Controls.Add(Me._lblLabels_14)
        Me.Controls.Add(Me._lblLabels_13)
        Me.Controls.Add(Me._lblLabels_12)
        Me.Controls.Add(Me._lblLabels_11)
        Me.Controls.Add(Me._lblLabels_8)
        Me.Controls.Add(Me._lblLabels_7)
        Me.Controls.Add(Me._lblLabels_6)
        Me.Controls.Add(Me._lb_日_2)
        Me.Controls.Add(Me._lb_月_2)
        Me.Controls.Add(Me._lb_年_2)
        Me.Controls.Add(Me._lb_kara_0)
        Me.Controls.Add(Me._lb_年_0)
        Me.Controls.Add(Me._lb_月_0)
        Me.Controls.Add(Me._lb_日_0)
        Me.Controls.Add(Me._lb_年_1)
        Me.Controls.Add(Me._lb_日_1)
        Me.Controls.Add(Me._lblLabels_5)
        Me.Controls.Add(Me._lblLabels_4)
        Me.Controls.Add(Me._lblLabels_3)
        Me.Controls.Add(Me._lblLabels_2)
        Me.Controls.Add(Me._lblLabels_0)
        Me.Controls.Add(Me._lblLabels_10)
        Me.Controls.Add(Me.lb_見積番号)
        Me.Controls.Add(Me.rf_見積番号)
        Me.Controls.Add(Me.lb_見積件名)
        Me.Controls.Add(Me._lb_項目_0)
        Me.Controls.Add(Me._lb_項目_6)
        Me.Controls.Add(Me._lb_項目_2)
        Me.Controls.Add(Me._lb_項目_3)
        Me.Controls.Add(Me._lb_項目_4)
        Me.Controls.Add(Me._lb_項目_1)
        Me.Controls.Add(Me.rf_処理区分)
        Me.Controls.Add(Me._lb_納期_0)
        Me.Controls.Add(Me._lb_納期_1)
        Me.Controls.Add(Me.lb_OPEN日)
        Me.Controls.Add(Me.lb_見積日付)
        Me.Controls.Add(Me.lb_受注日付)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.rf_ウエルシア物件区分名)
        Me.Controls.Add(Me._lblLabels_34)
        Me.Controls.Add(Me._lblLabels_33)
        Me.Controls.Add(Me._lblLabels_32)
        Me.Controls.Add(Me._lb_コンテナ_1)
        Me.Controls.Add(Me.Label16)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(11, 30)
        Me.Name = "SnwMT01F00"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "見積入力"
        Me.sb_Msg.ResumeLayout(False)
        Me.sb_Msg.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Timer1 As Timer
End Class