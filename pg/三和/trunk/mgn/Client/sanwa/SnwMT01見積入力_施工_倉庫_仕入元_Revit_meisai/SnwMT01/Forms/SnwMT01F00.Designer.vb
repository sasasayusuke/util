<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class SnwMT01F00
#Region "Windows フォーム デザイナによって生成されたコード "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
		'この呼び出しは、Windows フォーム デザイナで必要です。
		InitializeComponent()
	End Sub
	'Form は、コンポーネント一覧に後処理を実行するために dispose をオーバーライドします。
	<System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			If Not components Is Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Windows フォーム デザイナで必要です。
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents ck_社内伝票扱い As System.Windows.Forms.CheckBox
	Public WithEvents tx_受注日付Y As System.Windows.Forms.TextBox
	Public WithEvents tx_受注日付M As System.Windows.Forms.TextBox
	Public WithEvents tx_受注日付D As System.Windows.Forms.TextBox
	Public WithEvents _cbFunc_6 As System.Windows.Forms.Button
	Public WithEvents _cbFunc_7 As System.Windows.Forms.Button
	Public WithEvents _cbFunc_5 As System.Windows.Forms.Button
	Public WithEvents _cbFunc_4 As System.Windows.Forms.Button
	Public WithEvents _cbFunc_3 As System.Windows.Forms.Button
	Public WithEvents _cbFunc_2 As System.Windows.Forms.Button
	Public WithEvents _cbFunc_1 As System.Windows.Forms.Button
	Public WithEvents _cbFunc_8 As System.Windows.Forms.Button
	Public WithEvents _cbFunc_9 As System.Windows.Forms.Button
	Public WithEvents _cbFunc_12 As System.Windows.Forms.Button
	Public WithEvents _cbFunc_11 As System.Windows.Forms.Button
	Public WithEvents _cbFunc_10 As System.Windows.Forms.Button
	Public WithEvents cbTabEnd As System.Windows.Forms.Button
	Public WithEvents _lb_Func_12 As System.Windows.Forms.Label
	Public WithEvents _lb_Func_11 As System.Windows.Forms.Label
	Public WithEvents _lb_Func_5 As System.Windows.Forms.Label
	Public WithEvents _lb_Func_3 As System.Windows.Forms.Label
	Public WithEvents _lb_Func_2 As System.Windows.Forms.Label
	Public WithEvents _lb_Func_4 As System.Windows.Forms.Label
	Public WithEvents _lb_Func_1 As System.Windows.Forms.Label
	Public WithEvents _lb_Func_6 As System.Windows.Forms.Label
	Public WithEvents _lb_Func_7 As System.Windows.Forms.Label
	Public WithEvents _lb_Func_8 As System.Windows.Forms.Label
	Public WithEvents _lb_Func_9 As System.Windows.Forms.Label
	Public WithEvents _lb_Func_10 As System.Windows.Forms.Label
	Public WithEvents picFunction As System.Windows.Forms.Panel
	Public WithEvents _sb_Msg_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents _sb_Msg_Panel2 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents _sb_Msg_Panel3 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents sb_Msg As System.Windows.Forms.StatusStrip
	Public WithEvents tx_得意先名2 As System.Windows.Forms.TextBox
	Public WithEvents tx_備考 As System.Windows.Forms.TextBox
	Public WithEvents tx_得意先名1 As System.Windows.Forms.TextBox
	Public WithEvents tx_物件金額 As System.Windows.Forms.TextBox
	Public WithEvents tx_出精値引 As System.Windows.Forms.TextBox
	Public WithEvents tx_見積件名 As System.Windows.Forms.TextBox
	Public WithEvents tx_得TEL As System.Windows.Forms.TextBox
	Public WithEvents tx_得FAX As System.Windows.Forms.TextBox
	Public WithEvents tx_得担当者 As System.Windows.Forms.TextBox
	Public WithEvents tx_納入先CD As System.Windows.Forms.TextBox
	Public WithEvents tx_納得意先CD As System.Windows.Forms.TextBox
	Public WithEvents tx_納入先名2 As System.Windows.Forms.TextBox
	Public WithEvents tx_納入先名1 As System.Windows.Forms.TextBox
	Public WithEvents tx_郵便番号 As System.Windows.Forms.TextBox
	Public WithEvents tx_納住所1 As System.Windows.Forms.TextBox
	Public WithEvents tx_納住所2 As System.Windows.Forms.TextBox
	Public WithEvents tx_納TEL As System.Windows.Forms.TextBox
	Public WithEvents tx_納FAX As System.Windows.Forms.TextBox
	Public WithEvents tx_納担当者 As System.Windows.Forms.TextBox
	Public WithEvents tx_s納期D As System.Windows.Forms.TextBox
	Public WithEvents tx_s納期M As System.Windows.Forms.TextBox
	Public WithEvents tx_s納期Y As System.Windows.Forms.TextBox
	Public WithEvents tx_e納期D As System.Windows.Forms.TextBox
	Public WithEvents tx_e納期M As System.Windows.Forms.TextBox
	Public WithEvents tx_e納期Y As System.Windows.Forms.TextBox
	Public WithEvents tx_得意先CD As System.Windows.Forms.TextBox
	Public WithEvents tx_OPEN日D As System.Windows.Forms.TextBox
	Public WithEvents tx_OPEN日M As System.Windows.Forms.TextBox
	Public WithEvents tx_OPEN日Y As System.Windows.Forms.TextBox
	Public WithEvents tx_物件種別 As System.Windows.Forms.TextBox
	Public WithEvents tx_納期表示 As System.Windows.Forms.TextBox
	Public WithEvents tx_納期表示他 As System.Windows.Forms.TextBox
	Public WithEvents tx_出力日 As System.Windows.Forms.TextBox
	Public WithEvents tx_有効期限 As System.Windows.Forms.TextBox
	Public WithEvents tx_受注区分 As System.Windows.Forms.TextBox
	Public WithEvents tx_大小口区分 As System.Windows.Forms.TextBox
	Public WithEvents tx_見積日付D As System.Windows.Forms.TextBox
	Public WithEvents tx_見積日付M As System.Windows.Forms.TextBox
	Public WithEvents tx_見積日付Y As System.Windows.Forms.TextBox
	Public WithEvents tx_現場名 As System.Windows.Forms.TextBox
	Public WithEvents tx_支払条件 As System.Windows.Forms.TextBox
	Public WithEvents tx_支払条件他 As System.Windows.Forms.TextBox
	Public WithEvents tx_担当者CD As System.Windows.Forms.TextBox
	Public WithEvents rf_販売先得意先名2 As System.Windows.Forms.TextBox
	Public WithEvents rf_販売先得意先名1 As System.Windows.Forms.TextBox
	Public WithEvents tx_販売先得意先CD As System.Windows.Forms.TextBox
	Public WithEvents tx_販売先納入先CD As System.Windows.Forms.TextBox
	Public WithEvents tx_販売先納得意先CD As System.Windows.Forms.TextBox
	Public WithEvents rf_販売先納入先名2 As System.Windows.Forms.TextBox
	Public WithEvents rf_販売先納入先名1 As System.Windows.Forms.TextBox
	Public WithEvents tx_部署CD As System.Windows.Forms.TextBox
	Public WithEvents tx_物件番号 As System.Windows.Forms.TextBox
	Public WithEvents tx_伝票種類 As System.Windows.Forms.TextBox
	Public WithEvents tx_ウエルシア物件内容名 As System.Windows.Forms.TextBox
	Public WithEvents tx_ウエルシア物件内容CD As System.Windows.Forms.TextBox
	Public WithEvents tx_ウエルシア売場面積 As System.Windows.Forms.TextBox
	Public WithEvents tx_受付日付Y As System.Windows.Forms.TextBox
	Public WithEvents tx_受付日付M As System.Windows.Forms.TextBox
	Public WithEvents tx_受付日付D As System.Windows.Forms.TextBox
	Public WithEvents tx_完工日付Y As System.Windows.Forms.TextBox
	Public WithEvents tx_完工日付M As System.Windows.Forms.TextBox
	Public WithEvents tx_完工日付D As System.Windows.Forms.TextBox
	Public WithEvents tx_発注担当者名 As System.Windows.Forms.TextBox
	Public WithEvents tx_作業内容 As System.Windows.Forms.TextBox
	Public WithEvents tx_YKサプライ区分 As System.Windows.Forms.TextBox
	Public WithEvents tx_YK物件区分 As System.Windows.Forms.TextBox
	Public WithEvents tx_YK請求区分 As System.Windows.Forms.TextBox
	Public WithEvents tx_化粧品メーカー区分 As System.Windows.Forms.TextBox
	Public WithEvents tx_SM内容区分 As System.Windows.Forms.TextBox
	Public WithEvents tx_税集計区分 As System.Windows.Forms.TextBox
	Public WithEvents tx_クレーム区分 As System.Windows.Forms.TextBox
	Public WithEvents tx_工事担当CD As System.Windows.Forms.TextBox
	Public WithEvents tx_発注書発行日付 As System.Windows.Forms.TextBox
	Public WithEvents tx_完了者名 As System.Windows.Forms.TextBox
	Public WithEvents tx_見積確定区分 As System.Windows.Forms.TextBox
	Public WithEvents tx_経過備考1 As System.Windows.Forms.TextBox
	Public WithEvents tx_ウエルシア物件区分 As System.Windows.Forms.TextBox
	Public WithEvents tx_ウエルシアリース区分 As System.Windows.Forms.TextBox
	Public WithEvents tx_経過備考2 As System.Windows.Forms.TextBox
	Public WithEvents tx_請求予定M As System.Windows.Forms.TextBox
	Public WithEvents tx_請求予定D As System.Windows.Forms.TextBox
	Public WithEvents tx_完了日Y As System.Windows.Forms.TextBox
	Public WithEvents tx_完了日M As System.Windows.Forms.TextBox
	Public WithEvents tx_完了日D As System.Windows.Forms.TextBox
	Public WithEvents tx_請求予定Y As System.Windows.Forms.TextBox
	Public WithEvents tx_集計CD As System.Windows.Forms.TextBox
	Public WithEvents tx_B請求管轄区分 As System.Windows.Forms.TextBox
	Public WithEvents tx_BtoB番号 As System.Windows.Forms.TextBox
	Public WithEvents tx_業種区分 As System.Windows.Forms.TextBox
	Public WithEvents tx_統合見積番号 As System.Windows.Forms.TextBox
	Public WithEvents rf_統合見積件名 As System.Windows.Forms.Label
	Public WithEvents _lblLabels_58 As System.Windows.Forms.Label
	Public WithEvents _lblLabels_57 As System.Windows.Forms.Label
	Public WithEvents _lblLabels_56 As System.Windows.Forms.Label
	Public WithEvents _lblLabels_55 As System.Windows.Forms.Label
	Public WithEvents _lblLabels_54 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_15 As System.Windows.Forms.Label
	Public WithEvents _Shape1_13 As Microsoft.VisualBasic.PowerPacks.RectangleShape
	Public WithEvents rf_集計名 As System.Windows.Forms.Label
	Public WithEvents _lblLabels_53 As System.Windows.Forms.Label
	Public WithEvents _lb_日_8 As System.Windows.Forms.Label
	Public WithEvents Label14 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_14 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_13 As System.Windows.Forms.Label
	Public WithEvents _lblLabels_52 As System.Windows.Forms.Label
	Public WithEvents _Shape1_12 As Microsoft.VisualBasic.PowerPacks.RectangleShape
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
	Public WithEvents _Shape1_11 As Microsoft.VisualBasic.PowerPacks.RectangleShape
	Public WithEvents _lb_項目_11 As System.Windows.Forms.Label
	Public WithEvents _Shape1_10 As Microsoft.VisualBasic.PowerPacks.RectangleShape
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
	Public WithEvents _Shape1_9 As Microsoft.VisualBasic.PowerPacks.RectangleShape
	Public WithEvents _lblLabels_31 As System.Windows.Forms.Label
	Public WithEvents _lblLabels_30 As System.Windows.Forms.Label
	Public WithEvents _lblLabels_29 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_7 As System.Windows.Forms.Label
	Public WithEvents _Shape1_7 As Microsoft.VisualBasic.PowerPacks.RectangleShape
	Public WithEvents _lblLabels_28 As System.Windows.Forms.Label
	Public WithEvents _lblLabels_27 As System.Windows.Forms.Label
	Public WithEvents Label4 As System.Windows.Forms.Label
	Public WithEvents rf_部署名 As System.Windows.Forms.Label
	Public WithEvents Label3 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_8 As System.Windows.Forms.Label
	Public WithEvents _Shape1_8 As Microsoft.VisualBasic.PowerPacks.RectangleShape
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
	Public WithEvents _Shape1_6 As Microsoft.VisualBasic.PowerPacks.RectangleShape
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
	Public WithEvents _Shape1_5 As Microsoft.VisualBasic.PowerPacks.RectangleShape
	Public WithEvents _lblLabels_14 As System.Windows.Forms.Label
	Public WithEvents _lblLabels_13 As System.Windows.Forms.Label
	Public WithEvents _lblLabels_12 As System.Windows.Forms.Label
	Public WithEvents _lblLabels_11 As System.Windows.Forms.Label
	Public WithEvents _Shape1_4 As Microsoft.VisualBasic.PowerPacks.RectangleShape
	Public WithEvents _lblLabels_8 As System.Windows.Forms.Label
	Public WithEvents _lblLabels_7 As System.Windows.Forms.Label
	Public WithEvents _lblLabels_6 As System.Windows.Forms.Label
	Public WithEvents _lb_日_2 As System.Windows.Forms.Label
	Public WithEvents _lb_月_2 As System.Windows.Forms.Label
	Public WithEvents _lb_年_2 As System.Windows.Forms.Label
	Public WithEvents _Shape1_3 As Microsoft.VisualBasic.PowerPacks.RectangleShape
	Public WithEvents _Shape1_0 As Microsoft.VisualBasic.PowerPacks.RectangleShape
	Public WithEvents _Shape1_1 As Microsoft.VisualBasic.PowerPacks.RectangleShape
	Public WithEvents _Shape1_2 As Microsoft.VisualBasic.PowerPacks.RectangleShape
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
	Public WithEvents Line1 As Microsoft.VisualBasic.PowerPacks.LineShape
	Public WithEvents _lb_項目_0 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_6 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_2 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_3 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_4 As System.Windows.Forms.Label
	Public WithEvents _lb_項目_1 As System.Windows.Forms.Label
	Public WithEvents rf_処理区分 As System.Windows.Forms.Label
	Public WithEvents _ln_over_1 As Microsoft.VisualBasic.PowerPacks.LineShape
	Public WithEvents _ln_over_0 As Microsoft.VisualBasic.PowerPacks.LineShape
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
	Public WithEvents cbFunc As Microsoft.VisualBasic.Compatibility.VB6.ButtonArray
	Public WithEvents lb_Func As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents lb_kara As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents lb_コンテナ As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents lb_月 As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents lb_項目 As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents lb_日 As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents lb_年 As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents lb_納期 As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents lblLabels As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents ln_over As LineShapeArray
	Public WithEvents Shape1 As RectangleShapeArray
	Public WithEvents ShapeContainer1 As Microsoft.VisualBasic.PowerPacks.ShapeContainer
	'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
	'Windows フォーム デザイナを使って変更できます。
	'コード エディタを使用して、変更しないでください。
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(SnwMT01F00))
		Me.components = New System.ComponentModel.Container()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
		Me.ShapeContainer1 = New Microsoft.VisualBasic.PowerPacks.ShapeContainer
		Me.ck_社内伝票扱い = New System.Windows.Forms.CheckBox
		Me.tx_受注日付Y = New System.Windows.Forms.TextBox
		Me.tx_受注日付M = New System.Windows.Forms.TextBox
		Me.tx_受注日付D = New System.Windows.Forms.TextBox
		Me.picFunction = New System.Windows.Forms.Panel
		Me._cbFunc_6 = New System.Windows.Forms.Button
		Me._cbFunc_7 = New System.Windows.Forms.Button
		Me._cbFunc_5 = New System.Windows.Forms.Button
		Me._cbFunc_4 = New System.Windows.Forms.Button
		Me._cbFunc_3 = New System.Windows.Forms.Button
		Me._cbFunc_2 = New System.Windows.Forms.Button
		Me._cbFunc_1 = New System.Windows.Forms.Button
		Me._cbFunc_8 = New System.Windows.Forms.Button
		Me._cbFunc_9 = New System.Windows.Forms.Button
		Me._cbFunc_12 = New System.Windows.Forms.Button
		Me._cbFunc_11 = New System.Windows.Forms.Button
		Me._cbFunc_10 = New System.Windows.Forms.Button
		Me.cbTabEnd = New System.Windows.Forms.Button
		Me._lb_Func_12 = New System.Windows.Forms.Label
		Me._lb_Func_11 = New System.Windows.Forms.Label
		Me._lb_Func_5 = New System.Windows.Forms.Label
		Me._lb_Func_3 = New System.Windows.Forms.Label
		Me._lb_Func_2 = New System.Windows.Forms.Label
		Me._lb_Func_4 = New System.Windows.Forms.Label
		Me._lb_Func_1 = New System.Windows.Forms.Label
		Me._lb_Func_6 = New System.Windows.Forms.Label
		Me._lb_Func_7 = New System.Windows.Forms.Label
		Me._lb_Func_8 = New System.Windows.Forms.Label
		Me._lb_Func_9 = New System.Windows.Forms.Label
		Me._lb_Func_10 = New System.Windows.Forms.Label
		Me.sb_Msg = New System.Windows.Forms.StatusStrip
		Me._sb_Msg_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
		Me._sb_Msg_Panel2 = New System.Windows.Forms.ToolStripStatusLabel
		Me._sb_Msg_Panel3 = New System.Windows.Forms.ToolStripStatusLabel
		Me.tx_得意先名2 = New System.Windows.Forms.TextBox
		Me.tx_備考 = New System.Windows.Forms.TextBox
		Me.tx_得意先名1 = New System.Windows.Forms.TextBox
		Me.tx_物件金額 = New System.Windows.Forms.TextBox
		Me.tx_出精値引 = New System.Windows.Forms.TextBox
		Me.tx_見積件名 = New System.Windows.Forms.TextBox
		Me.tx_得TEL = New System.Windows.Forms.TextBox
		Me.tx_得FAX = New System.Windows.Forms.TextBox
		Me.tx_得担当者 = New System.Windows.Forms.TextBox
		Me.tx_納入先CD = New System.Windows.Forms.TextBox
		Me.tx_納得意先CD = New System.Windows.Forms.TextBox
		Me.tx_納入先名2 = New System.Windows.Forms.TextBox
		Me.tx_納入先名1 = New System.Windows.Forms.TextBox
		Me.tx_郵便番号 = New System.Windows.Forms.TextBox
		Me.tx_納住所1 = New System.Windows.Forms.TextBox
		Me.tx_納住所2 = New System.Windows.Forms.TextBox
		Me.tx_納TEL = New System.Windows.Forms.TextBox
		Me.tx_納FAX = New System.Windows.Forms.TextBox
		Me.tx_納担当者 = New System.Windows.Forms.TextBox
		Me.tx_s納期D = New System.Windows.Forms.TextBox
		Me.tx_s納期M = New System.Windows.Forms.TextBox
		Me.tx_s納期Y = New System.Windows.Forms.TextBox
		Me.tx_e納期D = New System.Windows.Forms.TextBox
		Me.tx_e納期M = New System.Windows.Forms.TextBox
		Me.tx_e納期Y = New System.Windows.Forms.TextBox
		Me.tx_得意先CD = New System.Windows.Forms.TextBox
		Me.tx_OPEN日D = New System.Windows.Forms.TextBox
		Me.tx_OPEN日M = New System.Windows.Forms.TextBox
		Me.tx_OPEN日Y = New System.Windows.Forms.TextBox
		Me.tx_物件種別 = New System.Windows.Forms.TextBox
		Me.tx_納期表示 = New System.Windows.Forms.TextBox
		Me.tx_納期表示他 = New System.Windows.Forms.TextBox
		Me.tx_出力日 = New System.Windows.Forms.TextBox
		Me.tx_有効期限 = New System.Windows.Forms.TextBox
		Me.tx_受注区分 = New System.Windows.Forms.TextBox
		Me.tx_大小口区分 = New System.Windows.Forms.TextBox
		Me.tx_見積日付D = New System.Windows.Forms.TextBox
		Me.tx_見積日付M = New System.Windows.Forms.TextBox
		Me.tx_見積日付Y = New System.Windows.Forms.TextBox
		Me.tx_現場名 = New System.Windows.Forms.TextBox
		Me.tx_支払条件 = New System.Windows.Forms.TextBox
		Me.tx_支払条件他 = New System.Windows.Forms.TextBox
		Me.tx_担当者CD = New System.Windows.Forms.TextBox
		Me.rf_販売先得意先名2 = New System.Windows.Forms.TextBox
		Me.rf_販売先得意先名1 = New System.Windows.Forms.TextBox
		Me.tx_販売先得意先CD = New System.Windows.Forms.TextBox
		Me.tx_販売先納入先CD = New System.Windows.Forms.TextBox
		Me.tx_販売先納得意先CD = New System.Windows.Forms.TextBox
		Me.rf_販売先納入先名2 = New System.Windows.Forms.TextBox
		Me.rf_販売先納入先名1 = New System.Windows.Forms.TextBox
		Me.tx_部署CD = New System.Windows.Forms.TextBox
		Me.tx_物件番号 = New System.Windows.Forms.TextBox
		Me.tx_伝票種類 = New System.Windows.Forms.TextBox
		Me.tx_ウエルシア物件内容名 = New System.Windows.Forms.TextBox
		Me.tx_ウエルシア物件内容CD = New System.Windows.Forms.TextBox
		Me.tx_ウエルシア売場面積 = New System.Windows.Forms.TextBox
		Me.tx_受付日付Y = New System.Windows.Forms.TextBox
		Me.tx_受付日付M = New System.Windows.Forms.TextBox
		Me.tx_受付日付D = New System.Windows.Forms.TextBox
		Me.tx_完工日付Y = New System.Windows.Forms.TextBox
		Me.tx_完工日付M = New System.Windows.Forms.TextBox
		Me.tx_完工日付D = New System.Windows.Forms.TextBox
		Me.tx_発注担当者名 = New System.Windows.Forms.TextBox
		Me.tx_作業内容 = New System.Windows.Forms.TextBox
		Me.tx_YKサプライ区分 = New System.Windows.Forms.TextBox
		Me.tx_YK物件区分 = New System.Windows.Forms.TextBox
		Me.tx_YK請求区分 = New System.Windows.Forms.TextBox
		Me.tx_化粧品メーカー区分 = New System.Windows.Forms.TextBox
		Me.tx_SM内容区分 = New System.Windows.Forms.TextBox
		Me.tx_税集計区分 = New System.Windows.Forms.TextBox
		Me.tx_クレーム区分 = New System.Windows.Forms.TextBox
		Me.tx_工事担当CD = New System.Windows.Forms.TextBox
		Me.tx_発注書発行日付 = New System.Windows.Forms.TextBox
		Me.tx_完了者名 = New System.Windows.Forms.TextBox
		Me.tx_見積確定区分 = New System.Windows.Forms.TextBox
		Me.tx_経過備考1 = New System.Windows.Forms.TextBox
		Me.tx_ウエルシア物件区分 = New System.Windows.Forms.TextBox
		Me.tx_ウエルシアリース区分 = New System.Windows.Forms.TextBox
		Me.tx_経過備考2 = New System.Windows.Forms.TextBox
		Me.tx_請求予定M = New System.Windows.Forms.TextBox
		Me.tx_請求予定D = New System.Windows.Forms.TextBox
		Me.tx_完了日Y = New System.Windows.Forms.TextBox
		Me.tx_完了日M = New System.Windows.Forms.TextBox
		Me.tx_完了日D = New System.Windows.Forms.TextBox
		Me.tx_請求予定Y = New System.Windows.Forms.TextBox
		Me.tx_集計CD = New System.Windows.Forms.TextBox
		Me.tx_B請求管轄区分 = New System.Windows.Forms.TextBox
		Me.tx_BtoB番号 = New System.Windows.Forms.TextBox
		Me.tx_業種区分 = New System.Windows.Forms.TextBox
		Me.tx_統合見積番号 = New System.Windows.Forms.TextBox
		Me.rf_統合見積件名 = New System.Windows.Forms.Label
		Me._lblLabels_58 = New System.Windows.Forms.Label
		Me._lblLabels_57 = New System.Windows.Forms.Label
		Me._lblLabels_56 = New System.Windows.Forms.Label
		Me._lblLabels_55 = New System.Windows.Forms.Label
		Me._lblLabels_54 = New System.Windows.Forms.Label
		Me._lb_項目_15 = New System.Windows.Forms.Label
		Me._Shape1_13 = New Microsoft.VisualBasic.PowerPacks.RectangleShape
		Me.rf_集計名 = New System.Windows.Forms.Label
		Me._lblLabels_53 = New System.Windows.Forms.Label
		Me._lb_日_8 = New System.Windows.Forms.Label
		Me.Label14 = New System.Windows.Forms.Label
		Me._lb_項目_14 = New System.Windows.Forms.Label
		Me._lb_項目_13 = New System.Windows.Forms.Label
		Me._lblLabels_52 = New System.Windows.Forms.Label
		Me._Shape1_12 = New Microsoft.VisualBasic.PowerPacks.RectangleShape
		Me._lb_項目_12 = New System.Windows.Forms.Label
		Me.Label13 = New System.Windows.Forms.Label
		Me._lblLabels_51 = New System.Windows.Forms.Label
		Me.Label12 = New System.Windows.Forms.Label
		Me.Label9 = New System.Windows.Forms.Label
		Me._lb_日_7 = New System.Windows.Forms.Label
		Me._lb_月_7 = New System.Windows.Forms.Label
		Me._lb_年_7 = New System.Windows.Forms.Label
		Me.Label10 = New System.Windows.Forms.Label
		Me.rf_工事担当名 = New System.Windows.Forms.Label
		Me._lblLabels_50 = New System.Windows.Forms.Label
		Me._lblLabels_49 = New System.Windows.Forms.Label
		Me._lblLabels_48 = New System.Windows.Forms.Label
		Me._lblLabels_47 = New System.Windows.Forms.Label
		Me._lblLabels_1 = New System.Windows.Forms.Label
		Me.rf_税集計区分名 = New System.Windows.Forms.Label
		Me._lblLabels_46 = New System.Windows.Forms.Label
		Me._lblLabels_45 = New System.Windows.Forms.Label
		Me._lblLabels_44 = New System.Windows.Forms.Label
		Me._lblLabels_43 = New System.Windows.Forms.Label
		Me._Shape1_11 = New Microsoft.VisualBasic.PowerPacks.RectangleShape
		Me._lb_項目_11 = New System.Windows.Forms.Label
		Me._Shape1_10 = New Microsoft.VisualBasic.PowerPacks.RectangleShape
		Me._lblLabels_42 = New System.Windows.Forms.Label
		Me._lblLabels_41 = New System.Windows.Forms.Label
		Me._lblLabels_40 = New System.Windows.Forms.Label
		Me._lblLabels_39 = New System.Windows.Forms.Label
		Me._lblLabels_36 = New System.Windows.Forms.Label
		Me._lblLabels_35 = New System.Windows.Forms.Label
		Me._lblLabels_38 = New System.Windows.Forms.Label
		Me._lblLabels_37 = New System.Windows.Forms.Label
		Me.Label7 = New System.Windows.Forms.Label
		Me._lb_日_6 = New System.Windows.Forms.Label
		Me._lb_月_6 = New System.Windows.Forms.Label
		Me._lb_年_6 = New System.Windows.Forms.Label
		Me.Label5 = New System.Windows.Forms.Label
		Me._lb_日_5 = New System.Windows.Forms.Label
		Me._lb_月_5 = New System.Windows.Forms.Label
		Me._lb_年_5 = New System.Windows.Forms.Label
		Me._lb_項目_10 = New System.Windows.Forms.Label
		Me.rf_物件略称 = New System.Windows.Forms.Label
		Me._lb_項目_9 = New System.Windows.Forms.Label
		Me._Shape1_9 = New Microsoft.VisualBasic.PowerPacks.RectangleShape
		Me._lblLabels_31 = New System.Windows.Forms.Label
		Me._lblLabels_30 = New System.Windows.Forms.Label
		Me._lblLabels_29 = New System.Windows.Forms.Label
		Me._lb_項目_7 = New System.Windows.Forms.Label
		Me._Shape1_7 = New Microsoft.VisualBasic.PowerPacks.RectangleShape
		Me._lblLabels_28 = New System.Windows.Forms.Label
		Me._lblLabels_27 = New System.Windows.Forms.Label
		Me.Label4 = New System.Windows.Forms.Label
		Me.rf_部署名 = New System.Windows.Forms.Label
		Me.Label3 = New System.Windows.Forms.Label
		Me._lb_項目_8 = New System.Windows.Forms.Label
		Me._Shape1_8 = New Microsoft.VisualBasic.PowerPacks.RectangleShape
		Me.rf_得意先別見積番号 = New System.Windows.Forms.Label
		Me.rf_外税額 = New System.Windows.Forms.Label
		Me.rf_原価率 = New System.Windows.Forms.Label
		Me.rf_原価合計 = New System.Windows.Forms.Label
		Me.rf_合計金額 = New System.Windows.Forms.Label
		Me.rf_売上端数 = New System.Windows.Forms.Label
		Me.rf_消費税端数 = New System.Windows.Forms.Label
		Me.rf_税集計区分 = New System.Windows.Forms.Label
		Me._lblLabels_25 = New System.Windows.Forms.Label
		Me._lb_年_4 = New System.Windows.Forms.Label
		Me._lb_月_4 = New System.Windows.Forms.Label
		Me._lb_日_4 = New System.Windows.Forms.Label
		Me.Label2 = New System.Windows.Forms.Label
		Me._lblLabels_15 = New System.Windows.Forms.Label
		Me._lblLabels_9 = New System.Windows.Forms.Label
		Me._Shape1_6 = New Microsoft.VisualBasic.PowerPacks.RectangleShape
		Me.Label1 = New System.Windows.Forms.Label
		Me._lb_項目_5 = New System.Windows.Forms.Label
		Me._lblLabels_26 = New System.Windows.Forms.Label
		Me._lb_日_3 = New System.Windows.Forms.Label
		Me._lb_月_3 = New System.Windows.Forms.Label
		Me._lb_年_3 = New System.Windows.Forms.Label
		Me._lb_月_1 = New System.Windows.Forms.Label
		Me._lblLabels_24 = New System.Windows.Forms.Label
		Me._lblLabels_23 = New System.Windows.Forms.Label
		Me._lblLabels_22 = New System.Windows.Forms.Label
		Me._lblLabels_21 = New System.Windows.Forms.Label
		Me._lblLabels_20 = New System.Windows.Forms.Label
		Me.rf_担当者名 = New System.Windows.Forms.Label
		Me.lb_担当者CD = New System.Windows.Forms.Label
		Me._lblLabels_19 = New System.Windows.Forms.Label
		Me._lblLabels_18 = New System.Windows.Forms.Label
		Me._lblLabels_17 = New System.Windows.Forms.Label
		Me._lblLabels_16 = New System.Windows.Forms.Label
		Me._Shape1_5 = New Microsoft.VisualBasic.PowerPacks.RectangleShape
		Me._lblLabels_14 = New System.Windows.Forms.Label
		Me._lblLabels_13 = New System.Windows.Forms.Label
		Me._lblLabels_12 = New System.Windows.Forms.Label
		Me._lblLabels_11 = New System.Windows.Forms.Label
		Me._Shape1_4 = New Microsoft.VisualBasic.PowerPacks.RectangleShape
		Me._lblLabels_8 = New System.Windows.Forms.Label
		Me._lblLabels_7 = New System.Windows.Forms.Label
		Me._lblLabels_6 = New System.Windows.Forms.Label
		Me._lb_日_2 = New System.Windows.Forms.Label
		Me._lb_月_2 = New System.Windows.Forms.Label
		Me._lb_年_2 = New System.Windows.Forms.Label
		Me._Shape1_3 = New Microsoft.VisualBasic.PowerPacks.RectangleShape
		Me._Shape1_0 = New Microsoft.VisualBasic.PowerPacks.RectangleShape
		Me._Shape1_1 = New Microsoft.VisualBasic.PowerPacks.RectangleShape
		Me._Shape1_2 = New Microsoft.VisualBasic.PowerPacks.RectangleShape
		Me._lb_kara_0 = New System.Windows.Forms.Label
		Me._lb_年_0 = New System.Windows.Forms.Label
		Me._lb_月_0 = New System.Windows.Forms.Label
		Me._lb_日_0 = New System.Windows.Forms.Label
		Me._lb_年_1 = New System.Windows.Forms.Label
		Me._lb_日_1 = New System.Windows.Forms.Label
		Me._lblLabels_5 = New System.Windows.Forms.Label
		Me._lblLabels_4 = New System.Windows.Forms.Label
		Me._lblLabels_3 = New System.Windows.Forms.Label
		Me._lblLabels_2 = New System.Windows.Forms.Label
		Me._lblLabels_0 = New System.Windows.Forms.Label
		Me._lblLabels_10 = New System.Windows.Forms.Label
		Me.lb_見積番号 = New System.Windows.Forms.Label
		Me.rf_見積番号 = New System.Windows.Forms.Label
		Me.lb_見積件名 = New System.Windows.Forms.Label
		Me.Line1 = New Microsoft.VisualBasic.PowerPacks.LineShape
		Me._lb_項目_0 = New System.Windows.Forms.Label
		Me._lb_項目_6 = New System.Windows.Forms.Label
		Me._lb_項目_2 = New System.Windows.Forms.Label
		Me._lb_項目_3 = New System.Windows.Forms.Label
		Me._lb_項目_4 = New System.Windows.Forms.Label
		Me._lb_項目_1 = New System.Windows.Forms.Label
		Me.rf_処理区分 = New System.Windows.Forms.Label
		Me._ln_over_1 = New Microsoft.VisualBasic.PowerPacks.LineShape
		Me._ln_over_0 = New Microsoft.VisualBasic.PowerPacks.LineShape
		Me._lb_納期_0 = New System.Windows.Forms.Label
		Me._lb_納期_1 = New System.Windows.Forms.Label
		Me.lb_OPEN日 = New System.Windows.Forms.Label
		Me.lb_見積日付 = New System.Windows.Forms.Label
		Me.lb_受注日付 = New System.Windows.Forms.Label
		Me.Label6 = New System.Windows.Forms.Label
		Me.Label8 = New System.Windows.Forms.Label
		Me.Label11 = New System.Windows.Forms.Label
		Me.rf_ウエルシア物件区分名 = New System.Windows.Forms.Label
		Me._lblLabels_34 = New System.Windows.Forms.Label
		Me._lblLabels_33 = New System.Windows.Forms.Label
		Me._lblLabels_32 = New System.Windows.Forms.Label
		Me._lb_コンテナ_1 = New System.Windows.Forms.Label
		Me.Label16 = New System.Windows.Forms.Label
		Me.cbFunc = New Microsoft.VisualBasic.Compatibility.VB6.ButtonArray(components)
		Me.lb_Func = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(components)
		Me.lb_kara = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(components)
		Me.lb_コンテナ = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(components)
		Me.lb_月 = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(components)
		Me.lb_項目 = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(components)
		Me.lb_日 = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(components)
		Me.lb_年 = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(components)
		Me.lb_納期 = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(components)
		Me.lblLabels = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(components)
		Me.ln_over = New LineShapeArray(components)
		Me.Shape1 = New RectangleShapeArray(components)
		Me.picFunction.SuspendLayout()
		Me.sb_Msg.SuspendLayout()
		Me.SuspendLayout()
		Me.ToolTip1.Active = True
		CType(Me.cbFunc, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lb_Func, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lb_kara, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lb_コンテナ, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lb_月, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lb_項目, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lb_日, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lb_年, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lb_納期, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lblLabels, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.ln_over, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Shape1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.Text = "見積入力"
		Me.ClientSize = New System.Drawing.Size(1232, 709)
		Me.Location = New System.Drawing.Point(11, 30)
		Me.Font = New System.Drawing.Font("ＭＳ ゴシック", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.Icon = CType(resources.GetObject("SnwMT01F00.Icon"), System.Drawing.Icon)
		Me.KeyPreview = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.ControlBox = True
		Me.Enabled = True
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.HelpButton = False
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.Name = "SnwMT01F00"
		Me.ck_社内伝票扱い.Text = "社内伝票扱い"
		Me.ck_社内伝票扱い.Size = New System.Drawing.Size(113, 13)
		Me.ck_社内伝票扱い.Location = New System.Drawing.Point(112, 356)
		Me.ck_社内伝票扱い.TabIndex = 28
		Me.ck_社内伝票扱い.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.ck_社内伝票扱い.FlatStyle = System.Windows.Forms.FlatStyle.Standard
		Me.ck_社内伝票扱い.BackColor = System.Drawing.SystemColors.Control
		Me.ck_社内伝票扱い.CausesValidation = True
		Me.ck_社内伝票扱い.Enabled = True
		Me.ck_社内伝票扱い.ForeColor = System.Drawing.SystemColors.ControlText
		Me.ck_社内伝票扱い.Cursor = System.Windows.Forms.Cursors.Default
		Me.ck_社内伝票扱い.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ck_社内伝票扱い.Appearance = System.Windows.Forms.Appearance.Normal
		Me.ck_社内伝票扱い.TabStop = True
		Me.ck_社内伝票扱い.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.ck_社内伝票扱い.Visible = True
		Me.ck_社内伝票扱い.Name = "ck_社内伝票扱い"
		Me.tx_受注日付Y.AutoSize = False
		Me.tx_受注日付Y.Size = New System.Drawing.Size(33, 13)
		Me.tx_受注日付Y.Location = New System.Drawing.Point(902, 439)
		Me.tx_受注日付Y.TabIndex = 74
		Me.tx_受注日付Y.Text = "8888"
		Me.tx_受注日付Y.Maxlength = 4
		Me.tx_受注日付Y.AcceptsReturn = True
		Me.tx_受注日付Y.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_受注日付Y.BackColor = System.Drawing.SystemColors.Window
		Me.tx_受注日付Y.CausesValidation = True
		Me.tx_受注日付Y.Enabled = True
		Me.tx_受注日付Y.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_受注日付Y.HideSelection = True
		Me.tx_受注日付Y.ReadOnly = False
		Me.tx_受注日付Y.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_受注日付Y.MultiLine = False
		Me.tx_受注日付Y.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_受注日付Y.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_受注日付Y.TabStop = True
		Me.tx_受注日付Y.Visible = True
		Me.tx_受注日付Y.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_受注日付Y.Name = "tx_受注日付Y"
		Me.tx_受注日付M.AutoSize = False
		Me.tx_受注日付M.Size = New System.Drawing.Size(16, 13)
		Me.tx_受注日付M.Location = New System.Drawing.Point(953, 439)
		Me.tx_受注日付M.TabIndex = 75
		Me.tx_受注日付M.Text = "88"
		Me.tx_受注日付M.Maxlength = 2
		Me.tx_受注日付M.AcceptsReturn = True
		Me.tx_受注日付M.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_受注日付M.BackColor = System.Drawing.SystemColors.Window
		Me.tx_受注日付M.CausesValidation = True
		Me.tx_受注日付M.Enabled = True
		Me.tx_受注日付M.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_受注日付M.HideSelection = True
		Me.tx_受注日付M.ReadOnly = False
		Me.tx_受注日付M.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_受注日付M.MultiLine = False
		Me.tx_受注日付M.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_受注日付M.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_受注日付M.TabStop = True
		Me.tx_受注日付M.Visible = True
		Me.tx_受注日付M.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_受注日付M.Name = "tx_受注日付M"
		Me.tx_受注日付D.AutoSize = False
		Me.tx_受注日付D.Size = New System.Drawing.Size(16, 13)
		Me.tx_受注日付D.Location = New System.Drawing.Point(988, 439)
		Me.tx_受注日付D.TabIndex = 76
		Me.tx_受注日付D.Text = "88"
		Me.tx_受注日付D.Maxlength = 2
		Me.tx_受注日付D.AcceptsReturn = True
		Me.tx_受注日付D.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_受注日付D.BackColor = System.Drawing.SystemColors.Window
		Me.tx_受注日付D.CausesValidation = True
		Me.tx_受注日付D.Enabled = True
		Me.tx_受注日付D.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_受注日付D.HideSelection = True
		Me.tx_受注日付D.ReadOnly = False
		Me.tx_受注日付D.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_受注日付D.MultiLine = False
		Me.tx_受注日付D.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_受注日付D.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_受注日付D.TabStop = True
		Me.tx_受注日付D.Visible = True
		Me.tx_受注日付D.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_受注日付D.Name = "tx_受注日付D"
		Me.picFunction.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.picFunction.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.picFunction.Size = New System.Drawing.Size(1232, 41)
		Me.picFunction.Location = New System.Drawing.Point(0, 648)
		Me.picFunction.TabIndex = 101
		Me.picFunction.TabStop = False
		Me.picFunction.BackColor = System.Drawing.SystemColors.Control
		Me.picFunction.CausesValidation = True
		Me.picFunction.Enabled = True
		Me.picFunction.ForeColor = System.Drawing.SystemColors.ControlText
		Me.picFunction.Cursor = System.Windows.Forms.Cursors.Default
		Me.picFunction.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.picFunction.Visible = True
		Me.picFunction.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.picFunction.Name = "picFunction"
		Me._cbFunc_6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me._cbFunc_6.BackColor = System.Drawing.SystemColors.Control
		Me._cbFunc_6.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._cbFunc_6.Size = New System.Drawing.Size(60, 22)
		Me._cbFunc_6.Location = New System.Drawing.Point(308, 14)
		Me._cbFunc_6.TabIndex = 120
		Me._cbFunc_6.TabStop = False
		Me._cbFunc_6.CausesValidation = True
		Me._cbFunc_6.Enabled = True
		Me._cbFunc_6.ForeColor = System.Drawing.SystemColors.ControlText
		Me._cbFunc_6.Cursor = System.Windows.Forms.Cursors.Default
		Me._cbFunc_6.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._cbFunc_6.Name = "_cbFunc_6"
		Me._cbFunc_7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me._cbFunc_7.BackColor = System.Drawing.SystemColors.Control
		Me._cbFunc_7.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._cbFunc_7.Size = New System.Drawing.Size(60, 22)
		Me._cbFunc_7.Location = New System.Drawing.Point(368, 14)
		Me._cbFunc_7.TabIndex = 119
		Me._cbFunc_7.TabStop = False
		Me._cbFunc_7.CausesValidation = True
		Me._cbFunc_7.Enabled = True
		Me._cbFunc_7.ForeColor = System.Drawing.SystemColors.ControlText
		Me._cbFunc_7.Cursor = System.Windows.Forms.Cursors.Default
		Me._cbFunc_7.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._cbFunc_7.Name = "_cbFunc_7"
		Me._cbFunc_5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me._cbFunc_5.BackColor = System.Drawing.SystemColors.Control
		Me._cbFunc_5.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._cbFunc_5.Size = New System.Drawing.Size(60, 22)
		Me._cbFunc_5.Location = New System.Drawing.Point(248, 14)
		Me._cbFunc_5.TabIndex = 95
		Me._cbFunc_5.TabStop = False
		Me._cbFunc_5.CausesValidation = True
		Me._cbFunc_5.Enabled = True
		Me._cbFunc_5.ForeColor = System.Drawing.SystemColors.ControlText
		Me._cbFunc_5.Cursor = System.Windows.Forms.Cursors.Default
		Me._cbFunc_5.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._cbFunc_5.Name = "_cbFunc_5"
		Me._cbFunc_4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me._cbFunc_4.BackColor = System.Drawing.SystemColors.Control
		Me._cbFunc_4.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._cbFunc_4.Size = New System.Drawing.Size(60, 22)
		Me._cbFunc_4.Location = New System.Drawing.Point(181, 14)
		Me._cbFunc_4.TabIndex = 94
		Me._cbFunc_4.TabStop = False
		Me._cbFunc_4.CausesValidation = True
		Me._cbFunc_4.Enabled = True
		Me._cbFunc_4.ForeColor = System.Drawing.SystemColors.ControlText
		Me._cbFunc_4.Cursor = System.Windows.Forms.Cursors.Default
		Me._cbFunc_4.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._cbFunc_4.Name = "_cbFunc_4"
		Me._cbFunc_3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me._cbFunc_3.BackColor = System.Drawing.SystemColors.Control
		Me._cbFunc_3.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._cbFunc_3.Size = New System.Drawing.Size(60, 22)
		Me._cbFunc_3.Location = New System.Drawing.Point(121, 14)
		Me._cbFunc_3.TabIndex = 93
		Me._cbFunc_3.TabStop = False
		Me._cbFunc_3.CausesValidation = True
		Me._cbFunc_3.Enabled = True
		Me._cbFunc_3.ForeColor = System.Drawing.SystemColors.ControlText
		Me._cbFunc_3.Cursor = System.Windows.Forms.Cursors.Default
		Me._cbFunc_3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._cbFunc_3.Name = "_cbFunc_3"
		Me._cbFunc_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me._cbFunc_2.BackColor = System.Drawing.SystemColors.Control
		Me._cbFunc_2.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._cbFunc_2.Size = New System.Drawing.Size(60, 22)
		Me._cbFunc_2.Location = New System.Drawing.Point(61, 14)
		Me._cbFunc_2.TabIndex = 92
		Me._cbFunc_2.TabStop = False
		Me._cbFunc_2.CausesValidation = True
		Me._cbFunc_2.Enabled = True
		Me._cbFunc_2.ForeColor = System.Drawing.SystemColors.ControlText
		Me._cbFunc_2.Cursor = System.Windows.Forms.Cursors.Default
		Me._cbFunc_2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._cbFunc_2.Name = "_cbFunc_2"
		Me._cbFunc_1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me._cbFunc_1.BackColor = System.Drawing.SystemColors.Control
		Me._cbFunc_1.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._cbFunc_1.Size = New System.Drawing.Size(60, 22)
		Me._cbFunc_1.Location = New System.Drawing.Point(1, 14)
		Me._cbFunc_1.TabIndex = 91
		Me._cbFunc_1.TabStop = False
		Me._cbFunc_1.CausesValidation = True
		Me._cbFunc_1.Enabled = True
		Me._cbFunc_1.ForeColor = System.Drawing.SystemColors.ControlText
		Me._cbFunc_1.Cursor = System.Windows.Forms.Cursors.Default
		Me._cbFunc_1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._cbFunc_1.Name = "_cbFunc_1"
		Me._cbFunc_8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me._cbFunc_8.BackColor = System.Drawing.SystemColors.Control
		Me._cbFunc_8.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._cbFunc_8.Size = New System.Drawing.Size(60, 22)
		Me._cbFunc_8.Location = New System.Drawing.Point(428, 14)
		Me._cbFunc_8.TabIndex = 96
		Me._cbFunc_8.TabStop = False
		Me._cbFunc_8.CausesValidation = True
		Me._cbFunc_8.Enabled = True
		Me._cbFunc_8.ForeColor = System.Drawing.SystemColors.ControlText
		Me._cbFunc_8.Cursor = System.Windows.Forms.Cursors.Default
		Me._cbFunc_8.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._cbFunc_8.Name = "_cbFunc_8"
		Me._cbFunc_9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me._cbFunc_9.BackColor = System.Drawing.SystemColors.Control
		Me._cbFunc_9.Text = "削除"
		Me._cbFunc_9.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._cbFunc_9.Size = New System.Drawing.Size(60, 22)
		Me._cbFunc_9.Location = New System.Drawing.Point(494, 14)
		Me._cbFunc_9.TabIndex = 97
		Me._cbFunc_9.TabStop = False
		Me._cbFunc_9.CausesValidation = True
		Me._cbFunc_9.Enabled = True
		Me._cbFunc_9.ForeColor = System.Drawing.SystemColors.ControlText
		Me._cbFunc_9.Cursor = System.Windows.Forms.Cursors.Default
		Me._cbFunc_9.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._cbFunc_9.Name = "_cbFunc_9"
		Me._cbFunc_12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me._cbFunc_12.BackColor = System.Drawing.SystemColors.Control
		Me._cbFunc_12.Text = "戻る"
		Me._cbFunc_12.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._cbFunc_12.Size = New System.Drawing.Size(60, 22)
		Me._cbFunc_12.Location = New System.Drawing.Point(674, 14)
		Me._cbFunc_12.TabIndex = 100
		Me._cbFunc_12.TabStop = False
		Me._cbFunc_12.CausesValidation = True
		Me._cbFunc_12.Enabled = True
		Me._cbFunc_12.ForeColor = System.Drawing.SystemColors.ControlText
		Me._cbFunc_12.Cursor = System.Windows.Forms.Cursors.Default
		Me._cbFunc_12.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._cbFunc_12.Name = "_cbFunc_12"
		Me._cbFunc_11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me._cbFunc_11.BackColor = System.Drawing.SystemColors.Control
		Me._cbFunc_11.Text = "次へ"
		Me._cbFunc_11.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._cbFunc_11.Size = New System.Drawing.Size(60, 22)
		Me._cbFunc_11.Location = New System.Drawing.Point(614, 14)
		Me._cbFunc_11.TabIndex = 99
		Me._cbFunc_11.TabStop = False
		Me._cbFunc_11.CausesValidation = True
		Me._cbFunc_11.Enabled = True
		Me._cbFunc_11.ForeColor = System.Drawing.SystemColors.ControlText
		Me._cbFunc_11.Cursor = System.Windows.Forms.Cursors.Default
		Me._cbFunc_11.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._cbFunc_11.Name = "_cbFunc_11"
		Me._cbFunc_10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me._cbFunc_10.BackColor = System.Drawing.SystemColors.Control
		Me._cbFunc_10.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._cbFunc_10.Size = New System.Drawing.Size(60, 22)
		Me._cbFunc_10.Location = New System.Drawing.Point(554, 14)
		Me._cbFunc_10.TabIndex = 98
		Me._cbFunc_10.TabStop = False
		Me._cbFunc_10.CausesValidation = True
		Me._cbFunc_10.Enabled = True
		Me._cbFunc_10.ForeColor = System.Drawing.SystemColors.ControlText
		Me._cbFunc_10.Cursor = System.Windows.Forms.Cursors.Default
		Me._cbFunc_10.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._cbFunc_10.Name = "_cbFunc_10"
		Me.cbTabEnd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cbTabEnd.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.cbTabEnd.Size = New System.Drawing.Size(41, 9)
		Me.cbTabEnd.Location = New System.Drawing.Point(682, 22)
		Me.cbTabEnd.TabIndex = 90
		Me.cbTabEnd.BackColor = System.Drawing.SystemColors.Control
		Me.cbTabEnd.CausesValidation = True
		Me.cbTabEnd.Enabled = True
		Me.cbTabEnd.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cbTabEnd.Cursor = System.Windows.Forms.Cursors.Default
		Me.cbTabEnd.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cbTabEnd.TabStop = True
		Me.cbTabEnd.Name = "cbTabEnd"
		Me._lb_Func_12.Text = " F12"
		Me._lb_Func_12.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_Func_12.Size = New System.Drawing.Size(60, 10)
		Me._lb_Func_12.Location = New System.Drawing.Point(674, 1)
		Me._lb_Func_12.TabIndex = 122
		Me._lb_Func_12.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lb_Func_12.BackColor = System.Drawing.Color.Transparent
		Me._lb_Func_12.Enabled = True
		Me._lb_Func_12.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_Func_12.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_Func_12.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_Func_12.UseMnemonic = True
		Me._lb_Func_12.Visible = True
		Me._lb_Func_12.AutoSize = False
		Me._lb_Func_12.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_Func_12.Name = "_lb_Func_12"
		Me._lb_Func_11.Text = " F11"
		Me._lb_Func_11.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_Func_11.Size = New System.Drawing.Size(60, 10)
		Me._lb_Func_11.Location = New System.Drawing.Point(614, 1)
		Me._lb_Func_11.TabIndex = 121
		Me._lb_Func_11.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lb_Func_11.BackColor = System.Drawing.Color.Transparent
		Me._lb_Func_11.Enabled = True
		Me._lb_Func_11.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_Func_11.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_Func_11.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_Func_11.UseMnemonic = True
		Me._lb_Func_11.Visible = True
		Me._lb_Func_11.AutoSize = False
		Me._lb_Func_11.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_Func_11.Name = "_lb_Func_11"
		Me._lb_Func_5.Text = " F5"
		Me._lb_Func_5.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_Func_5.Size = New System.Drawing.Size(60, 10)
		Me._lb_Func_5.Location = New System.Drawing.Point(248, 1)
		Me._lb_Func_5.TabIndex = 111
		Me._lb_Func_5.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lb_Func_5.BackColor = System.Drawing.Color.Transparent
		Me._lb_Func_5.Enabled = True
		Me._lb_Func_5.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_Func_5.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_Func_5.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_Func_5.UseMnemonic = True
		Me._lb_Func_5.Visible = True
		Me._lb_Func_5.AutoSize = False
		Me._lb_Func_5.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_Func_5.Name = "_lb_Func_5"
		Me._lb_Func_3.Text = " F3"
		Me._lb_Func_3.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_Func_3.Size = New System.Drawing.Size(60, 10)
		Me._lb_Func_3.Location = New System.Drawing.Point(121, 1)
		Me._lb_Func_3.TabIndex = 110
		Me._lb_Func_3.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lb_Func_3.BackColor = System.Drawing.Color.Transparent
		Me._lb_Func_3.Enabled = True
		Me._lb_Func_3.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_Func_3.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_Func_3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_Func_3.UseMnemonic = True
		Me._lb_Func_3.Visible = True
		Me._lb_Func_3.AutoSize = False
		Me._lb_Func_3.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_Func_3.Name = "_lb_Func_3"
		Me._lb_Func_2.Text = " F2"
		Me._lb_Func_2.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_Func_2.Size = New System.Drawing.Size(60, 10)
		Me._lb_Func_2.Location = New System.Drawing.Point(61, 1)
		Me._lb_Func_2.TabIndex = 109
		Me._lb_Func_2.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lb_Func_2.BackColor = System.Drawing.Color.Transparent
		Me._lb_Func_2.Enabled = True
		Me._lb_Func_2.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_Func_2.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_Func_2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_Func_2.UseMnemonic = True
		Me._lb_Func_2.Visible = True
		Me._lb_Func_2.AutoSize = False
		Me._lb_Func_2.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_Func_2.Name = "_lb_Func_2"
		Me._lb_Func_4.Text = " F4"
		Me._lb_Func_4.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_Func_4.Size = New System.Drawing.Size(60, 10)
		Me._lb_Func_4.Location = New System.Drawing.Point(181, 1)
		Me._lb_Func_4.TabIndex = 108
		Me._lb_Func_4.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lb_Func_4.BackColor = System.Drawing.Color.Transparent
		Me._lb_Func_4.Enabled = True
		Me._lb_Func_4.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_Func_4.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_Func_4.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_Func_4.UseMnemonic = True
		Me._lb_Func_4.Visible = True
		Me._lb_Func_4.AutoSize = False
		Me._lb_Func_4.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_Func_4.Name = "_lb_Func_4"
		Me._lb_Func_1.Text = " F1"
		Me._lb_Func_1.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_Func_1.Size = New System.Drawing.Size(60, 10)
		Me._lb_Func_1.Location = New System.Drawing.Point(1, 1)
		Me._lb_Func_1.TabIndex = 107
		Me._lb_Func_1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lb_Func_1.BackColor = System.Drawing.Color.Transparent
		Me._lb_Func_1.Enabled = True
		Me._lb_Func_1.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_Func_1.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_Func_1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_Func_1.UseMnemonic = True
		Me._lb_Func_1.Visible = True
		Me._lb_Func_1.AutoSize = False
		Me._lb_Func_1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_Func_1.Name = "_lb_Func_1"
		Me._lb_Func_6.Text = " F6"
		Me._lb_Func_6.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_Func_6.Size = New System.Drawing.Size(60, 10)
		Me._lb_Func_6.Location = New System.Drawing.Point(308, 1)
		Me._lb_Func_6.TabIndex = 106
		Me._lb_Func_6.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lb_Func_6.BackColor = System.Drawing.Color.Transparent
		Me._lb_Func_6.Enabled = True
		Me._lb_Func_6.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_Func_6.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_Func_6.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_Func_6.UseMnemonic = True
		Me._lb_Func_6.Visible = True
		Me._lb_Func_6.AutoSize = False
		Me._lb_Func_6.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_Func_6.Name = "_lb_Func_6"
		Me._lb_Func_7.Text = " F7"
		Me._lb_Func_7.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_Func_7.Size = New System.Drawing.Size(60, 10)
		Me._lb_Func_7.Location = New System.Drawing.Point(368, 1)
		Me._lb_Func_7.TabIndex = 105
		Me._lb_Func_7.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lb_Func_7.BackColor = System.Drawing.Color.Transparent
		Me._lb_Func_7.Enabled = True
		Me._lb_Func_7.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_Func_7.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_Func_7.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_Func_7.UseMnemonic = True
		Me._lb_Func_7.Visible = True
		Me._lb_Func_7.AutoSize = False
		Me._lb_Func_7.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_Func_7.Name = "_lb_Func_7"
		Me._lb_Func_8.Text = " F8"
		Me._lb_Func_8.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_Func_8.Size = New System.Drawing.Size(60, 10)
		Me._lb_Func_8.Location = New System.Drawing.Point(428, 1)
		Me._lb_Func_8.TabIndex = 104
		Me._lb_Func_8.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lb_Func_8.BackColor = System.Drawing.Color.Transparent
		Me._lb_Func_8.Enabled = True
		Me._lb_Func_8.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_Func_8.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_Func_8.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_Func_8.UseMnemonic = True
		Me._lb_Func_8.Visible = True
		Me._lb_Func_8.AutoSize = False
		Me._lb_Func_8.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_Func_8.Name = "_lb_Func_8"
		Me._lb_Func_9.Text = " F9"
		Me._lb_Func_9.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_Func_9.Size = New System.Drawing.Size(60, 10)
		Me._lb_Func_9.Location = New System.Drawing.Point(494, 1)
		Me._lb_Func_9.TabIndex = 103
		Me._lb_Func_9.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lb_Func_9.BackColor = System.Drawing.Color.Transparent
		Me._lb_Func_9.Enabled = True
		Me._lb_Func_9.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_Func_9.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_Func_9.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_Func_9.UseMnemonic = True
		Me._lb_Func_9.Visible = True
		Me._lb_Func_9.AutoSize = False
		Me._lb_Func_9.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_Func_9.Name = "_lb_Func_9"
		Me._lb_Func_10.Text = " F10"
		Me._lb_Func_10.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_Func_10.Size = New System.Drawing.Size(60, 10)
		Me._lb_Func_10.Location = New System.Drawing.Point(554, 1)
		Me._lb_Func_10.TabIndex = 102
		Me._lb_Func_10.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lb_Func_10.BackColor = System.Drawing.Color.Transparent
		Me._lb_Func_10.Enabled = True
		Me._lb_Func_10.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_Func_10.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_Func_10.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_Func_10.UseMnemonic = True
		Me._lb_Func_10.Visible = True
		Me._lb_Func_10.AutoSize = False
		Me._lb_Func_10.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_Func_10.Name = "_lb_Func_10"
		Me.sb_Msg.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.sb_Msg.Size = New System.Drawing.Size(1232, 20)
		Me.sb_Msg.Location = New System.Drawing.Point(0, 689)
		Me.sb_Msg.TabIndex = 112
		Me.sb_Msg.Font = New System.Drawing.Font("ＭＳ ゴシック", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.sb_Msg.Name = "sb_Msg"
		Me._sb_Msg_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
		Me._sb_Msg_Panel1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
		Me._sb_Msg_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._sb_Msg_Panel1.Size = New System.Drawing.Size(1069, 20)
		Me._sb_Msg_Panel1.Spring = True
		Me._sb_Msg_Panel1.AutoSize = True
		Me._sb_Msg_Panel1.BorderSides = CType(System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom, System.Windows.Forms.ToolStripStatusLabelBorderSides)
		Me._sb_Msg_Panel1.Margin = New System.Windows.Forms.Padding(0)
		Me._sb_Msg_Panel1.AutoSize = False
		Me._sb_Msg_Panel2.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
		Me._sb_Msg_Panel2.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
		Me._sb_Msg_Panel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		Me._sb_Msg_Panel2.BorderSides = CType(System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom, System.Windows.Forms.ToolStripStatusLabelBorderSides)
		Me._sb_Msg_Panel2.Margin = New System.Windows.Forms.Padding(0)
		Me._sb_Msg_Panel2.Size = New System.Drawing.Size(96, 20)
		Me._sb_Msg_Panel2.AutoSize = False
		Me._sb_Msg_Panel3.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
		Me._sb_Msg_Panel3.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
		Me._sb_Msg_Panel3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		Me._sb_Msg_Panel3.Size = New System.Drawing.Size(49, 20)
		Me._sb_Msg_Panel3.BorderSides = CType(System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom, System.Windows.Forms.ToolStripStatusLabelBorderSides)
		Me._sb_Msg_Panel3.Margin = New System.Windows.Forms.Padding(0)
		Me._sb_Msg_Panel3.AutoSize = False
		Me.tx_得意先名2.AutoSize = False
		Me.tx_得意先名2.Size = New System.Drawing.Size(237, 19)
		Me.tx_得意先名2.Location = New System.Drawing.Point(420, 172)
		Me.tx_得意先名2.TabIndex = 11
		Me.tx_得意先名2.Text = "１２３４５６７８９０１２３４"
		Me.tx_得意先名2.Maxlength = 28
		Me.tx_得意先名2.AcceptsReturn = True
		Me.tx_得意先名2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_得意先名2.BackColor = System.Drawing.SystemColors.Window
		Me.tx_得意先名2.CausesValidation = True
		Me.tx_得意先名2.Enabled = True
		Me.tx_得意先名2.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_得意先名2.HideSelection = True
		Me.tx_得意先名2.ReadOnly = False
		Me.tx_得意先名2.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_得意先名2.MultiLine = False
		Me.tx_得意先名2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_得意先名2.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_得意先名2.TabStop = True
		Me.tx_得意先名2.Visible = True
		Me.tx_得意先名2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_得意先名2.Name = "tx_得意先名2"
		Me.tx_備考.AutoSize = False
		Me.tx_備考.Size = New System.Drawing.Size(434, 19)
		Me.tx_備考.Location = New System.Drawing.Point(112, 460)
		Me.tx_備考.TabIndex = 38
		Me.tx_備考.Text = "123456789012345678901234567890123456789012345678901234567890"
		Me.tx_備考.Maxlength = 40
		Me.tx_備考.AcceptsReturn = True
		Me.tx_備考.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_備考.BackColor = System.Drawing.SystemColors.Window
		Me.tx_備考.CausesValidation = True
		Me.tx_備考.Enabled = True
		Me.tx_備考.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_備考.HideSelection = True
		Me.tx_備考.ReadOnly = False
		Me.tx_備考.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_備考.MultiLine = False
		Me.tx_備考.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_備考.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_備考.TabStop = True
		Me.tx_備考.Visible = True
		Me.tx_備考.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_備考.Name = "tx_備考"
		Me.tx_得意先名1.AutoSize = False
		Me.tx_得意先名1.Size = New System.Drawing.Size(237, 19)
		Me.tx_得意先名1.Location = New System.Drawing.Point(180, 172)
		Me.tx_得意先名1.TabIndex = 10
		Me.tx_得意先名1.Text = "１２３４５６７８９０１２３４"
		Me.tx_得意先名1.Maxlength = 28
		Me.tx_得意先名1.AcceptsReturn = True
		Me.tx_得意先名1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_得意先名1.BackColor = System.Drawing.SystemColors.Window
		Me.tx_得意先名1.CausesValidation = True
		Me.tx_得意先名1.Enabled = True
		Me.tx_得意先名1.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_得意先名1.HideSelection = True
		Me.tx_得意先名1.ReadOnly = False
		Me.tx_得意先名1.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_得意先名1.MultiLine = False
		Me.tx_得意先名1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_得意先名1.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_得意先名1.TabStop = True
		Me.tx_得意先名1.Visible = True
		Me.tx_得意先名1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_得意先名1.Name = "tx_得意先名1"
		Me.tx_物件金額.AutoSize = False
		Me.tx_物件金額.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_物件金額.Size = New System.Drawing.Size(106, 19)
		Me.tx_物件金額.Location = New System.Drawing.Point(200, 512)
		Me.tx_物件金額.TabIndex = 40
		Me.tx_物件金額.Text = "123456789"
		Me.tx_物件金額.Maxlength = 9
		Me.tx_物件金額.AcceptsReturn = True
		Me.tx_物件金額.BackColor = System.Drawing.SystemColors.Window
		Me.tx_物件金額.CausesValidation = True
		Me.tx_物件金額.Enabled = True
		Me.tx_物件金額.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_物件金額.HideSelection = True
		Me.tx_物件金額.ReadOnly = False
		Me.tx_物件金額.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_物件金額.MultiLine = False
		Me.tx_物件金額.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_物件金額.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_物件金額.TabStop = True
		Me.tx_物件金額.Visible = True
		Me.tx_物件金額.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_物件金額.Name = "tx_物件金額"
		Me.tx_出精値引.AutoSize = False
		Me.tx_出精値引.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_出精値引.Size = New System.Drawing.Size(106, 19)
		Me.tx_出精値引.Location = New System.Drawing.Point(896, 476)
		Me.tx_出精値引.TabIndex = 78
		Me.tx_出精値引.Text = "1234567890"
		Me.tx_出精値引.Maxlength = 10
		Me.tx_出精値引.AcceptsReturn = True
		Me.tx_出精値引.BackColor = System.Drawing.SystemColors.Window
		Me.tx_出精値引.CausesValidation = True
		Me.tx_出精値引.Enabled = True
		Me.tx_出精値引.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_出精値引.HideSelection = True
		Me.tx_出精値引.ReadOnly = False
		Me.tx_出精値引.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_出精値引.MultiLine = False
		Me.tx_出精値引.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_出精値引.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_出精値引.TabStop = True
		Me.tx_出精値引.Visible = True
		Me.tx_出精値引.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_出精値引.Name = "tx_出精値引"
		Me.tx_見積件名.AutoSize = False
		Me.tx_見積件名.Size = New System.Drawing.Size(430, 19)
		Me.tx_見積件名.Location = New System.Drawing.Point(180, 136)
		Me.tx_見積件名.TabIndex = 8
		Me.tx_見積件名.Text = "１２３４５６７８９０１２３４５６７８９０１２３４５６７８９０"
		Me.tx_見積件名.Maxlength = 60
		Me.tx_見積件名.AcceptsReturn = True
		Me.tx_見積件名.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_見積件名.BackColor = System.Drawing.SystemColors.Window
		Me.tx_見積件名.CausesValidation = True
		Me.tx_見積件名.Enabled = True
		Me.tx_見積件名.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_見積件名.HideSelection = True
		Me.tx_見積件名.ReadOnly = False
		Me.tx_見積件名.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_見積件名.MultiLine = False
		Me.tx_見積件名.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_見積件名.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_見積件名.TabStop = True
		Me.tx_見積件名.Visible = True
		Me.tx_見積件名.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_見積件名.Name = "tx_見積件名"
		Me.tx_得TEL.AutoSize = False
		Me.tx_得TEL.Size = New System.Drawing.Size(126, 19)
		Me.tx_得TEL.Location = New System.Drawing.Point(180, 192)
		Me.tx_得TEL.TabIndex = 12
		Me.tx_得TEL.Text = "123456789012345"
		Me.tx_得TEL.Maxlength = 15
		Me.tx_得TEL.AcceptsReturn = True
		Me.tx_得TEL.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_得TEL.BackColor = System.Drawing.SystemColors.Window
		Me.tx_得TEL.CausesValidation = True
		Me.tx_得TEL.Enabled = True
		Me.tx_得TEL.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_得TEL.HideSelection = True
		Me.tx_得TEL.ReadOnly = False
		Me.tx_得TEL.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_得TEL.MultiLine = False
		Me.tx_得TEL.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_得TEL.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_得TEL.TabStop = True
		Me.tx_得TEL.Visible = True
		Me.tx_得TEL.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_得TEL.Name = "tx_得TEL"
		Me.tx_得FAX.AutoSize = False
		Me.tx_得FAX.Size = New System.Drawing.Size(126, 19)
		Me.tx_得FAX.Location = New System.Drawing.Point(363, 192)
		Me.tx_得FAX.TabIndex = 13
		Me.tx_得FAX.Text = "123456789012345"
		Me.tx_得FAX.Maxlength = 15
		Me.tx_得FAX.AcceptsReturn = True
		Me.tx_得FAX.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_得FAX.BackColor = System.Drawing.SystemColors.Window
		Me.tx_得FAX.CausesValidation = True
		Me.tx_得FAX.Enabled = True
		Me.tx_得FAX.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_得FAX.HideSelection = True
		Me.tx_得FAX.ReadOnly = False
		Me.tx_得FAX.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_得FAX.MultiLine = False
		Me.tx_得FAX.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_得FAX.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_得FAX.TabStop = True
		Me.tx_得FAX.Visible = True
		Me.tx_得FAX.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_得FAX.Name = "tx_得FAX"
		Me.tx_得担当者.AutoSize = False
		Me.tx_得担当者.Size = New System.Drawing.Size(285, 19)
		Me.tx_得担当者.Location = New System.Drawing.Point(180, 212)
		Me.tx_得担当者.TabIndex = 14
		Me.tx_得担当者.Text = "１２３４５６７８９０１２３４５６７８９０"
		Me.tx_得担当者.Maxlength = 40
		Me.tx_得担当者.AcceptsReturn = True
		Me.tx_得担当者.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_得担当者.BackColor = System.Drawing.SystemColors.Window
		Me.tx_得担当者.CausesValidation = True
		Me.tx_得担当者.Enabled = True
		Me.tx_得担当者.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_得担当者.HideSelection = True
		Me.tx_得担当者.ReadOnly = False
		Me.tx_得担当者.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_得担当者.MultiLine = False
		Me.tx_得担当者.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_得担当者.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_得担当者.TabStop = True
		Me.tx_得担当者.Visible = True
		Me.tx_得担当者.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_得担当者.Name = "tx_得担当者"
		Me.tx_納入先CD.AutoSize = False
		Me.tx_納入先CD.Size = New System.Drawing.Size(39, 19)
		Me.tx_納入先CD.Location = New System.Drawing.Point(180, 264)
		Me.tx_納入先CD.TabIndex = 19
		Me.tx_納入先CD.Text = "WWWW"
		Me.tx_納入先CD.Maxlength = 4
		Me.tx_納入先CD.AcceptsReturn = True
		Me.tx_納入先CD.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_納入先CD.BackColor = System.Drawing.SystemColors.Window
		Me.tx_納入先CD.CausesValidation = True
		Me.tx_納入先CD.Enabled = True
		Me.tx_納入先CD.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_納入先CD.HideSelection = True
		Me.tx_納入先CD.ReadOnly = False
		Me.tx_納入先CD.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_納入先CD.MultiLine = False
		Me.tx_納入先CD.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_納入先CD.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_納入先CD.TabStop = True
		Me.tx_納入先CD.Visible = True
		Me.tx_納入先CD.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_納入先CD.Name = "tx_納入先CD"
		Me.tx_納得意先CD.AutoSize = False
		Me.tx_納得意先CD.Size = New System.Drawing.Size(65, 19)
		Me.tx_納得意先CD.Location = New System.Drawing.Point(112, 264)
		Me.tx_納得意先CD.TabIndex = 18
		Me.tx_納得意先CD.Text = "WWWW"
		Me.tx_納得意先CD.Maxlength = 4
		Me.tx_納得意先CD.AcceptsReturn = True
		Me.tx_納得意先CD.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_納得意先CD.BackColor = System.Drawing.SystemColors.Window
		Me.tx_納得意先CD.CausesValidation = True
		Me.tx_納得意先CD.Enabled = True
		Me.tx_納得意先CD.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_納得意先CD.HideSelection = True
		Me.tx_納得意先CD.ReadOnly = False
		Me.tx_納得意先CD.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_納得意先CD.MultiLine = False
		Me.tx_納得意先CD.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_納得意先CD.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_納得意先CD.TabStop = True
		Me.tx_納得意先CD.Visible = True
		Me.tx_納得意先CD.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_納得意先CD.Name = "tx_納得意先CD"
		Me.tx_納入先名2.AutoSize = False
		Me.tx_納入先名2.Size = New System.Drawing.Size(237, 19)
		Me.tx_納入先名2.Location = New System.Drawing.Point(460, 264)
		Me.tx_納入先名2.TabIndex = 21
		Me.tx_納入先名2.Text = "1234567890123456789012345678"
		Me.tx_納入先名2.Maxlength = 28
		Me.tx_納入先名2.AcceptsReturn = True
		Me.tx_納入先名2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_納入先名2.BackColor = System.Drawing.SystemColors.Window
		Me.tx_納入先名2.CausesValidation = True
		Me.tx_納入先名2.Enabled = True
		Me.tx_納入先名2.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_納入先名2.HideSelection = True
		Me.tx_納入先名2.ReadOnly = False
		Me.tx_納入先名2.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_納入先名2.MultiLine = False
		Me.tx_納入先名2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_納入先名2.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_納入先名2.TabStop = True
		Me.tx_納入先名2.Visible = True
		Me.tx_納入先名2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_納入先名2.Name = "tx_納入先名2"
		Me.tx_納入先名1.AutoSize = False
		Me.tx_納入先名1.Size = New System.Drawing.Size(237, 19)
		Me.tx_納入先名1.Location = New System.Drawing.Point(220, 264)
		Me.tx_納入先名1.TabIndex = 20
		Me.tx_納入先名1.Text = "１２３４５６７８９０１２３４"
		Me.tx_納入先名1.Maxlength = 28
		Me.tx_納入先名1.AcceptsReturn = True
		Me.tx_納入先名1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_納入先名1.BackColor = System.Drawing.SystemColors.Window
		Me.tx_納入先名1.CausesValidation = True
		Me.tx_納入先名1.Enabled = True
		Me.tx_納入先名1.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_納入先名1.HideSelection = True
		Me.tx_納入先名1.ReadOnly = False
		Me.tx_納入先名1.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_納入先名1.MultiLine = False
		Me.tx_納入先名1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_納入先名1.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_納入先名1.TabStop = True
		Me.tx_納入先名1.Visible = True
		Me.tx_納入先名1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_納入先名1.Name = "tx_納入先名1"
		Me.tx_郵便番号.AutoSize = False
		Me.tx_郵便番号.Size = New System.Drawing.Size(63, 19)
		Me.tx_郵便番号.Location = New System.Drawing.Point(156, 284)
		Me.tx_郵便番号.TabIndex = 22
		Me.tx_郵便番号.Text = "123-4567"
		Me.tx_郵便番号.Maxlength = 8
		Me.tx_郵便番号.AcceptsReturn = True
		Me.tx_郵便番号.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_郵便番号.BackColor = System.Drawing.SystemColors.Window
		Me.tx_郵便番号.CausesValidation = True
		Me.tx_郵便番号.Enabled = True
		Me.tx_郵便番号.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_郵便番号.HideSelection = True
		Me.tx_郵便番号.ReadOnly = False
		Me.tx_郵便番号.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_郵便番号.MultiLine = False
		Me.tx_郵便番号.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_郵便番号.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_郵便番号.TabStop = True
		Me.tx_郵便番号.Visible = True
		Me.tx_郵便番号.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_郵便番号.Name = "tx_郵便番号"
		Me.tx_納住所1.AutoSize = False
		Me.tx_納住所1.Size = New System.Drawing.Size(237, 19)
		Me.tx_納住所1.Location = New System.Drawing.Point(220, 284)
		Me.tx_納住所1.TabIndex = 23
		Me.tx_納住所1.Text = "１２３４５６７８９０１２３４５６"
		Me.tx_納住所1.Maxlength = 32
		Me.tx_納住所1.AcceptsReturn = True
		Me.tx_納住所1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_納住所1.BackColor = System.Drawing.SystemColors.Window
		Me.tx_納住所1.CausesValidation = True
		Me.tx_納住所1.Enabled = True
		Me.tx_納住所1.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_納住所1.HideSelection = True
		Me.tx_納住所1.ReadOnly = False
		Me.tx_納住所1.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_納住所1.MultiLine = False
		Me.tx_納住所1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_納住所1.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_納住所1.TabStop = True
		Me.tx_納住所1.Visible = True
		Me.tx_納住所1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_納住所1.Name = "tx_納住所1"
		Me.tx_納住所2.AutoSize = False
		Me.tx_納住所2.Size = New System.Drawing.Size(237, 19)
		Me.tx_納住所2.Location = New System.Drawing.Point(460, 284)
		Me.tx_納住所2.TabIndex = 24
		Me.tx_納住所2.Text = "12345678901234567890123456789012"
		Me.tx_納住所2.Maxlength = 32
		Me.tx_納住所2.AcceptsReturn = True
		Me.tx_納住所2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_納住所2.BackColor = System.Drawing.SystemColors.Window
		Me.tx_納住所2.CausesValidation = True
		Me.tx_納住所2.Enabled = True
		Me.tx_納住所2.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_納住所2.HideSelection = True
		Me.tx_納住所2.ReadOnly = False
		Me.tx_納住所2.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_納住所2.MultiLine = False
		Me.tx_納住所2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_納住所2.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_納住所2.TabStop = True
		Me.tx_納住所2.Visible = True
		Me.tx_納住所2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_納住所2.Name = "tx_納住所2"
		Me.tx_納TEL.AutoSize = False
		Me.tx_納TEL.Size = New System.Drawing.Size(126, 19)
		Me.tx_納TEL.Location = New System.Drawing.Point(220, 304)
		Me.tx_納TEL.TabIndex = 25
		Me.tx_納TEL.Text = "123456789012345"
		Me.tx_納TEL.Maxlength = 15
		Me.tx_納TEL.AcceptsReturn = True
		Me.tx_納TEL.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_納TEL.BackColor = System.Drawing.SystemColors.Window
		Me.tx_納TEL.CausesValidation = True
		Me.tx_納TEL.Enabled = True
		Me.tx_納TEL.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_納TEL.HideSelection = True
		Me.tx_納TEL.ReadOnly = False
		Me.tx_納TEL.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_納TEL.MultiLine = False
		Me.tx_納TEL.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_納TEL.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_納TEL.TabStop = True
		Me.tx_納TEL.Visible = True
		Me.tx_納TEL.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_納TEL.Name = "tx_納TEL"
		Me.tx_納FAX.AutoSize = False
		Me.tx_納FAX.Size = New System.Drawing.Size(126, 19)
		Me.tx_納FAX.Location = New System.Drawing.Point(407, 304)
		Me.tx_納FAX.TabIndex = 26
		Me.tx_納FAX.Text = "123456789012345"
		Me.tx_納FAX.Maxlength = 15
		Me.tx_納FAX.AcceptsReturn = True
		Me.tx_納FAX.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_納FAX.BackColor = System.Drawing.SystemColors.Window
		Me.tx_納FAX.CausesValidation = True
		Me.tx_納FAX.Enabled = True
		Me.tx_納FAX.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_納FAX.HideSelection = True
		Me.tx_納FAX.ReadOnly = False
		Me.tx_納FAX.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_納FAX.MultiLine = False
		Me.tx_納FAX.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_納FAX.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_納FAX.TabStop = True
		Me.tx_納FAX.Visible = True
		Me.tx_納FAX.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_納FAX.Name = "tx_納FAX"
		Me.tx_納担当者.AutoSize = False
		Me.tx_納担当者.Size = New System.Drawing.Size(285, 19)
		Me.tx_納担当者.Location = New System.Drawing.Point(220, 324)
		Me.tx_納担当者.TabIndex = 27
		Me.tx_納担当者.Text = "１２３４５６７８９０"
		Me.tx_納担当者.Maxlength = 40
		Me.tx_納担当者.AcceptsReturn = True
		Me.tx_納担当者.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_納担当者.BackColor = System.Drawing.SystemColors.Window
		Me.tx_納担当者.CausesValidation = True
		Me.tx_納担当者.Enabled = True
		Me.tx_納担当者.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_納担当者.HideSelection = True
		Me.tx_納担当者.ReadOnly = False
		Me.tx_納担当者.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_納担当者.MultiLine = False
		Me.tx_納担当者.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_納担当者.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_納担当者.TabStop = True
		Me.tx_納担当者.Visible = True
		Me.tx_納担当者.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_納担当者.Name = "tx_納担当者"
		Me.tx_s納期D.AutoSize = False
		Me.tx_s納期D.Size = New System.Drawing.Size(16, 13)
		Me.tx_s納期D.Location = New System.Drawing.Point(204, 431)
		Me.tx_s納期D.TabIndex = 34
		Me.tx_s納期D.Text = "88"
		Me.tx_s納期D.Maxlength = 2
		Me.tx_s納期D.AcceptsReturn = True
		Me.tx_s納期D.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_s納期D.BackColor = System.Drawing.SystemColors.Window
		Me.tx_s納期D.CausesValidation = True
		Me.tx_s納期D.Enabled = True
		Me.tx_s納期D.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_s納期D.HideSelection = True
		Me.tx_s納期D.ReadOnly = False
		Me.tx_s納期D.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_s納期D.MultiLine = False
		Me.tx_s納期D.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_s納期D.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_s納期D.TabStop = True
		Me.tx_s納期D.Visible = True
		Me.tx_s納期D.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_s納期D.Name = "tx_s納期D"
		Me.tx_s納期M.AutoSize = False
		Me.tx_s納期M.Size = New System.Drawing.Size(16, 13)
		Me.tx_s納期M.Location = New System.Drawing.Point(169, 431)
		Me.tx_s納期M.TabIndex = 33
		Me.tx_s納期M.Text = "88"
		Me.tx_s納期M.Maxlength = 2
		Me.tx_s納期M.AcceptsReturn = True
		Me.tx_s納期M.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_s納期M.BackColor = System.Drawing.SystemColors.Window
		Me.tx_s納期M.CausesValidation = True
		Me.tx_s納期M.Enabled = True
		Me.tx_s納期M.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_s納期M.HideSelection = True
		Me.tx_s納期M.ReadOnly = False
		Me.tx_s納期M.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_s納期M.MultiLine = False
		Me.tx_s納期M.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_s納期M.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_s納期M.TabStop = True
		Me.tx_s納期M.Visible = True
		Me.tx_s納期M.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_s納期M.Name = "tx_s納期M"
		Me.tx_s納期Y.AutoSize = False
		Me.tx_s納期Y.Size = New System.Drawing.Size(33, 13)
		Me.tx_s納期Y.Location = New System.Drawing.Point(118, 431)
		Me.tx_s納期Y.TabIndex = 32
		Me.tx_s納期Y.Text = "8888"
		Me.tx_s納期Y.Maxlength = 4
		Me.tx_s納期Y.AcceptsReturn = True
		Me.tx_s納期Y.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_s納期Y.BackColor = System.Drawing.SystemColors.Window
		Me.tx_s納期Y.CausesValidation = True
		Me.tx_s納期Y.Enabled = True
		Me.tx_s納期Y.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_s納期Y.HideSelection = True
		Me.tx_s納期Y.ReadOnly = False
		Me.tx_s納期Y.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_s納期Y.MultiLine = False
		Me.tx_s納期Y.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_s納期Y.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_s納期Y.TabStop = True
		Me.tx_s納期Y.Visible = True
		Me.tx_s納期Y.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_s納期Y.Name = "tx_s納期Y"
		Me.tx_e納期D.AutoSize = False
		Me.tx_e納期D.Size = New System.Drawing.Size(16, 13)
		Me.tx_e納期D.Location = New System.Drawing.Point(349, 431)
		Me.tx_e納期D.TabIndex = 37
		Me.tx_e納期D.Text = "88"
		Me.tx_e納期D.Maxlength = 2
		Me.tx_e納期D.AcceptsReturn = True
		Me.tx_e納期D.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_e納期D.BackColor = System.Drawing.SystemColors.Window
		Me.tx_e納期D.CausesValidation = True
		Me.tx_e納期D.Enabled = True
		Me.tx_e納期D.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_e納期D.HideSelection = True
		Me.tx_e納期D.ReadOnly = False
		Me.tx_e納期D.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_e納期D.MultiLine = False
		Me.tx_e納期D.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_e納期D.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_e納期D.TabStop = True
		Me.tx_e納期D.Visible = True
		Me.tx_e納期D.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_e納期D.Name = "tx_e納期D"
		Me.tx_e納期M.AutoSize = False
		Me.tx_e納期M.Size = New System.Drawing.Size(16, 13)
		Me.tx_e納期M.Location = New System.Drawing.Point(315, 431)
		Me.tx_e納期M.TabIndex = 36
		Me.tx_e納期M.Text = "88"
		Me.tx_e納期M.Maxlength = 2
		Me.tx_e納期M.AcceptsReturn = True
		Me.tx_e納期M.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_e納期M.BackColor = System.Drawing.SystemColors.Window
		Me.tx_e納期M.CausesValidation = True
		Me.tx_e納期M.Enabled = True
		Me.tx_e納期M.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_e納期M.HideSelection = True
		Me.tx_e納期M.ReadOnly = False
		Me.tx_e納期M.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_e納期M.MultiLine = False
		Me.tx_e納期M.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_e納期M.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_e納期M.TabStop = True
		Me.tx_e納期M.Visible = True
		Me.tx_e納期M.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_e納期M.Name = "tx_e納期M"
		Me.tx_e納期Y.AutoSize = False
		Me.tx_e納期Y.Size = New System.Drawing.Size(33, 13)
		Me.tx_e納期Y.Location = New System.Drawing.Point(263, 431)
		Me.tx_e納期Y.TabIndex = 35
		Me.tx_e納期Y.Text = "8888"
		Me.tx_e納期Y.Maxlength = 4
		Me.tx_e納期Y.AcceptsReturn = True
		Me.tx_e納期Y.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_e納期Y.BackColor = System.Drawing.SystemColors.Window
		Me.tx_e納期Y.CausesValidation = True
		Me.tx_e納期Y.Enabled = True
		Me.tx_e納期Y.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_e納期Y.HideSelection = True
		Me.tx_e納期Y.ReadOnly = False
		Me.tx_e納期Y.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_e納期Y.MultiLine = False
		Me.tx_e納期Y.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_e納期Y.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_e納期Y.TabStop = True
		Me.tx_e納期Y.Visible = True
		Me.tx_e納期Y.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_e納期Y.Name = "tx_e納期Y"
		Me.tx_得意先CD.AutoSize = False
		Me.tx_得意先CD.Size = New System.Drawing.Size(65, 19)
		Me.tx_得意先CD.Location = New System.Drawing.Point(112, 172)
		Me.tx_得意先CD.TabIndex = 9
		Me.tx_得意先CD.Text = "WWWW"
		Me.tx_得意先CD.Maxlength = 4
		Me.tx_得意先CD.AcceptsReturn = True
		Me.tx_得意先CD.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_得意先CD.BackColor = System.Drawing.SystemColors.Window
		Me.tx_得意先CD.CausesValidation = True
		Me.tx_得意先CD.Enabled = True
		Me.tx_得意先CD.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_得意先CD.HideSelection = True
		Me.tx_得意先CD.ReadOnly = False
		Me.tx_得意先CD.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_得意先CD.MultiLine = False
		Me.tx_得意先CD.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_得意先CD.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_得意先CD.TabStop = True
		Me.tx_得意先CD.Visible = True
		Me.tx_得意先CD.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_得意先CD.Name = "tx_得意先CD"
		Me.tx_OPEN日D.AutoSize = False
		Me.tx_OPEN日D.Size = New System.Drawing.Size(16, 13)
		Me.tx_OPEN日D.Location = New System.Drawing.Point(542, 515)
		Me.tx_OPEN日D.TabIndex = 43
		Me.tx_OPEN日D.Text = "88"
		Me.tx_OPEN日D.Maxlength = 2
		Me.tx_OPEN日D.AcceptsReturn = True
		Me.tx_OPEN日D.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_OPEN日D.BackColor = System.Drawing.SystemColors.Window
		Me.tx_OPEN日D.CausesValidation = True
		Me.tx_OPEN日D.Enabled = True
		Me.tx_OPEN日D.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_OPEN日D.HideSelection = True
		Me.tx_OPEN日D.ReadOnly = False
		Me.tx_OPEN日D.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_OPEN日D.MultiLine = False
		Me.tx_OPEN日D.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_OPEN日D.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_OPEN日D.TabStop = True
		Me.tx_OPEN日D.Visible = True
		Me.tx_OPEN日D.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_OPEN日D.Name = "tx_OPEN日D"
		Me.tx_OPEN日M.AutoSize = False
		Me.tx_OPEN日M.Size = New System.Drawing.Size(16, 13)
		Me.tx_OPEN日M.Location = New System.Drawing.Point(507, 515)
		Me.tx_OPEN日M.TabIndex = 42
		Me.tx_OPEN日M.Text = "88"
		Me.tx_OPEN日M.Maxlength = 2
		Me.tx_OPEN日M.AcceptsReturn = True
		Me.tx_OPEN日M.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_OPEN日M.BackColor = System.Drawing.SystemColors.Window
		Me.tx_OPEN日M.CausesValidation = True
		Me.tx_OPEN日M.Enabled = True
		Me.tx_OPEN日M.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_OPEN日M.HideSelection = True
		Me.tx_OPEN日M.ReadOnly = False
		Me.tx_OPEN日M.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_OPEN日M.MultiLine = False
		Me.tx_OPEN日M.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_OPEN日M.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_OPEN日M.TabStop = True
		Me.tx_OPEN日M.Visible = True
		Me.tx_OPEN日M.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_OPEN日M.Name = "tx_OPEN日M"
		Me.tx_OPEN日Y.AutoSize = False
		Me.tx_OPEN日Y.Size = New System.Drawing.Size(33, 13)
		Me.tx_OPEN日Y.Location = New System.Drawing.Point(456, 515)
		Me.tx_OPEN日Y.TabIndex = 41
		Me.tx_OPEN日Y.Text = "8888"
		Me.tx_OPEN日Y.Maxlength = 4
		Me.tx_OPEN日Y.AcceptsReturn = True
		Me.tx_OPEN日Y.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_OPEN日Y.BackColor = System.Drawing.SystemColors.Window
		Me.tx_OPEN日Y.CausesValidation = True
		Me.tx_OPEN日Y.Enabled = True
		Me.tx_OPEN日Y.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_OPEN日Y.HideSelection = True
		Me.tx_OPEN日Y.ReadOnly = False
		Me.tx_OPEN日Y.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_OPEN日Y.MultiLine = False
		Me.tx_OPEN日Y.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_OPEN日Y.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_OPEN日Y.TabStop = True
		Me.tx_OPEN日Y.Visible = True
		Me.tx_OPEN日Y.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_OPEN日Y.Name = "tx_OPEN日Y"
		Me.tx_物件種別.AutoSize = False
		Me.tx_物件種別.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_物件種別.Size = New System.Drawing.Size(26, 19)
		Me.tx_物件種別.Location = New System.Drawing.Point(200, 532)
		Me.tx_物件種別.TabIndex = 44
		Me.tx_物件種別.Text = "1"
		Me.tx_物件種別.Maxlength = 1
		Me.tx_物件種別.AcceptsReturn = True
		Me.tx_物件種別.BackColor = System.Drawing.SystemColors.Window
		Me.tx_物件種別.CausesValidation = True
		Me.tx_物件種別.Enabled = True
		Me.tx_物件種別.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_物件種別.HideSelection = True
		Me.tx_物件種別.ReadOnly = False
		Me.tx_物件種別.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_物件種別.MultiLine = False
		Me.tx_物件種別.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_物件種別.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_物件種別.TabStop = True
		Me.tx_物件種別.Visible = True
		Me.tx_物件種別.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_物件種別.Name = "tx_物件種別"
		Me.tx_納期表示.AutoSize = False
		Me.tx_納期表示.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_納期表示.Size = New System.Drawing.Size(26, 19)
		Me.tx_納期表示.Location = New System.Drawing.Point(896, 356)
		Me.tx_納期表示.TabIndex = 69
		Me.tx_納期表示.Text = "1"
		Me.tx_納期表示.Maxlength = 1
		Me.tx_納期表示.AcceptsReturn = True
		Me.tx_納期表示.BackColor = System.Drawing.SystemColors.Window
		Me.tx_納期表示.CausesValidation = True
		Me.tx_納期表示.Enabled = True
		Me.tx_納期表示.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_納期表示.HideSelection = True
		Me.tx_納期表示.ReadOnly = False
		Me.tx_納期表示.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_納期表示.MultiLine = False
		Me.tx_納期表示.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_納期表示.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_納期表示.TabStop = True
		Me.tx_納期表示.Visible = True
		Me.tx_納期表示.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_納期表示.Name = "tx_納期表示"
		Me.tx_納期表示他.AutoSize = False
		Me.tx_納期表示他.Size = New System.Drawing.Size(214, 19)
		Me.tx_納期表示他.Location = New System.Drawing.Point(924, 376)
		Me.tx_納期表示他.TabIndex = 70
		Me.tx_納期表示他.Text = "１２３４５６７８９０１２３４５"
		Me.tx_納期表示他.Maxlength = 30
		Me.tx_納期表示他.AcceptsReturn = True
		Me.tx_納期表示他.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_納期表示他.BackColor = System.Drawing.SystemColors.Window
		Me.tx_納期表示他.CausesValidation = True
		Me.tx_納期表示他.Enabled = True
		Me.tx_納期表示他.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_納期表示他.HideSelection = True
		Me.tx_納期表示他.ReadOnly = False
		Me.tx_納期表示他.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_納期表示他.MultiLine = False
		Me.tx_納期表示他.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_納期表示他.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_納期表示他.TabStop = True
		Me.tx_納期表示他.Visible = True
		Me.tx_納期表示他.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_納期表示他.Name = "tx_納期表示他"
		Me.tx_出力日.AutoSize = False
		Me.tx_出力日.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_出力日.Size = New System.Drawing.Size(26, 19)
		Me.tx_出力日.Location = New System.Drawing.Point(896, 396)
		Me.tx_出力日.TabIndex = 71
		Me.tx_出力日.Text = "1"
		Me.tx_出力日.Maxlength = 1
		Me.tx_出力日.AcceptsReturn = True
		Me.tx_出力日.BackColor = System.Drawing.SystemColors.Window
		Me.tx_出力日.CausesValidation = True
		Me.tx_出力日.Enabled = True
		Me.tx_出力日.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_出力日.HideSelection = True
		Me.tx_出力日.ReadOnly = False
		Me.tx_出力日.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_出力日.MultiLine = False
		Me.tx_出力日.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_出力日.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_出力日.TabStop = True
		Me.tx_出力日.Visible = True
		Me.tx_出力日.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_出力日.Name = "tx_出力日"
		Me.tx_有効期限.AutoSize = False
		Me.tx_有効期限.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_有効期限.Size = New System.Drawing.Size(26, 19)
		Me.tx_有効期限.Location = New System.Drawing.Point(1164, 396)
		Me.tx_有効期限.TabIndex = 72
		Me.tx_有効期限.Text = "1"
		Me.tx_有効期限.Maxlength = 2
		Me.tx_有効期限.AcceptsReturn = True
		Me.tx_有効期限.BackColor = System.Drawing.SystemColors.Window
		Me.tx_有効期限.CausesValidation = True
		Me.tx_有効期限.Enabled = True
		Me.tx_有効期限.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_有効期限.HideSelection = True
		Me.tx_有効期限.ReadOnly = False
		Me.tx_有効期限.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_有効期限.MultiLine = False
		Me.tx_有効期限.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_有効期限.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_有効期限.TabStop = True
		Me.tx_有効期限.Visible = True
		Me.tx_有効期限.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_有効期限.Name = "tx_有効期限"
		Me.tx_受注区分.AutoSize = False
		Me.tx_受注区分.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_受注区分.Size = New System.Drawing.Size(26, 19)
		Me.tx_受注区分.Location = New System.Drawing.Point(896, 416)
		Me.tx_受注区分.TabIndex = 73
		Me.tx_受注区分.Text = "1"
		Me.tx_受注区分.Maxlength = 1
		Me.tx_受注区分.AcceptsReturn = True
		Me.tx_受注区分.BackColor = System.Drawing.SystemColors.Window
		Me.tx_受注区分.CausesValidation = True
		Me.tx_受注区分.Enabled = True
		Me.tx_受注区分.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_受注区分.HideSelection = True
		Me.tx_受注区分.ReadOnly = False
		Me.tx_受注区分.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_受注区分.MultiLine = False
		Me.tx_受注区分.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_受注区分.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_受注区分.TabStop = True
		Me.tx_受注区分.Visible = True
		Me.tx_受注区分.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_受注区分.Name = "tx_受注区分"
		Me.tx_大小口区分.AutoSize = False
		Me.tx_大小口区分.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_大小口区分.Size = New System.Drawing.Size(26, 19)
		Me.tx_大小口区分.Location = New System.Drawing.Point(896, 456)
		Me.tx_大小口区分.TabIndex = 77
		Me.tx_大小口区分.Text = "1"
		Me.tx_大小口区分.Maxlength = 1
		Me.tx_大小口区分.AcceptsReturn = True
		Me.tx_大小口区分.BackColor = System.Drawing.SystemColors.Window
		Me.tx_大小口区分.CausesValidation = True
		Me.tx_大小口区分.Enabled = True
		Me.tx_大小口区分.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_大小口区分.HideSelection = True
		Me.tx_大小口区分.ReadOnly = False
		Me.tx_大小口区分.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_大小口区分.MultiLine = False
		Me.tx_大小口区分.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_大小口区分.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_大小口区分.TabStop = True
		Me.tx_大小口区分.Visible = True
		Me.tx_大小口区分.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_大小口区分.Name = "tx_大小口区分"
		Me.tx_見積日付D.AutoSize = False
		Me.tx_見積日付D.Size = New System.Drawing.Size(16, 13)
		Me.tx_見積日付D.Location = New System.Drawing.Point(272, 119)
		Me.tx_見積日付D.TabIndex = 6
		Me.tx_見積日付D.Text = "88"
		Me.tx_見積日付D.Maxlength = 2
		Me.tx_見積日付D.AcceptsReturn = True
		Me.tx_見積日付D.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_見積日付D.BackColor = System.Drawing.SystemColors.Window
		Me.tx_見積日付D.CausesValidation = True
		Me.tx_見積日付D.Enabled = True
		Me.tx_見積日付D.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_見積日付D.HideSelection = True
		Me.tx_見積日付D.ReadOnly = False
		Me.tx_見積日付D.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_見積日付D.MultiLine = False
		Me.tx_見積日付D.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_見積日付D.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_見積日付D.TabStop = True
		Me.tx_見積日付D.Visible = True
		Me.tx_見積日付D.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_見積日付D.Name = "tx_見積日付D"
		Me.tx_見積日付M.AutoSize = False
		Me.tx_見積日付M.Size = New System.Drawing.Size(16, 13)
		Me.tx_見積日付M.Location = New System.Drawing.Point(237, 119)
		Me.tx_見積日付M.TabIndex = 5
		Me.tx_見積日付M.Text = "88"
		Me.tx_見積日付M.Maxlength = 2
		Me.tx_見積日付M.AcceptsReturn = True
		Me.tx_見積日付M.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_見積日付M.BackColor = System.Drawing.SystemColors.Window
		Me.tx_見積日付M.CausesValidation = True
		Me.tx_見積日付M.Enabled = True
		Me.tx_見積日付M.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_見積日付M.HideSelection = True
		Me.tx_見積日付M.ReadOnly = False
		Me.tx_見積日付M.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_見積日付M.MultiLine = False
		Me.tx_見積日付M.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_見積日付M.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_見積日付M.TabStop = True
		Me.tx_見積日付M.Visible = True
		Me.tx_見積日付M.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_見積日付M.Name = "tx_見積日付M"
		Me.tx_見積日付Y.AutoSize = False
		Me.tx_見積日付Y.Size = New System.Drawing.Size(33, 13)
		Me.tx_見積日付Y.Location = New System.Drawing.Point(186, 119)
		Me.tx_見積日付Y.TabIndex = 4
		Me.tx_見積日付Y.Text = "8888"
		Me.tx_見積日付Y.Maxlength = 4
		Me.tx_見積日付Y.AcceptsReturn = True
		Me.tx_見積日付Y.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_見積日付Y.BackColor = System.Drawing.SystemColors.Window
		Me.tx_見積日付Y.CausesValidation = True
		Me.tx_見積日付Y.Enabled = True
		Me.tx_見積日付Y.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_見積日付Y.HideSelection = True
		Me.tx_見積日付Y.ReadOnly = False
		Me.tx_見積日付Y.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_見積日付Y.MultiLine = False
		Me.tx_見積日付Y.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_見積日付Y.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_見積日付Y.TabStop = True
		Me.tx_見積日付Y.Visible = True
		Me.tx_見積日付Y.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_見積日付Y.Name = "tx_見積日付Y"
		Me.tx_現場名.AutoSize = False
		Me.tx_現場名.Size = New System.Drawing.Size(290, 19)
		Me.tx_現場名.Location = New System.Drawing.Point(896, 296)
		Me.tx_現場名.TabIndex = 66
		Me.tx_現場名.Text = "１２３４５６７８９０１２３４５６７８９０"
		Me.tx_現場名.Maxlength = 40
		Me.tx_現場名.AcceptsReturn = True
		Me.tx_現場名.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_現場名.BackColor = System.Drawing.SystemColors.Window
		Me.tx_現場名.CausesValidation = True
		Me.tx_現場名.Enabled = True
		Me.tx_現場名.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_現場名.HideSelection = True
		Me.tx_現場名.ReadOnly = False
		Me.tx_現場名.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_現場名.MultiLine = False
		Me.tx_現場名.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_現場名.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_現場名.TabStop = True
		Me.tx_現場名.Visible = True
		Me.tx_現場名.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_現場名.Name = "tx_現場名"
		Me.tx_支払条件.AutoSize = False
		Me.tx_支払条件.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_支払条件.Size = New System.Drawing.Size(26, 19)
		Me.tx_支払条件.Location = New System.Drawing.Point(896, 316)
		Me.tx_支払条件.TabIndex = 67
		Me.tx_支払条件.Text = "1"
		Me.tx_支払条件.Maxlength = 1
		Me.tx_支払条件.AcceptsReturn = True
		Me.tx_支払条件.BackColor = System.Drawing.SystemColors.Window
		Me.tx_支払条件.CausesValidation = True
		Me.tx_支払条件.Enabled = True
		Me.tx_支払条件.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_支払条件.HideSelection = True
		Me.tx_支払条件.ReadOnly = False
		Me.tx_支払条件.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_支払条件.MultiLine = False
		Me.tx_支払条件.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_支払条件.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_支払条件.TabStop = True
		Me.tx_支払条件.Visible = True
		Me.tx_支払条件.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_支払条件.Name = "tx_支払条件"
		Me.tx_支払条件他.AutoSize = False
		Me.tx_支払条件他.Size = New System.Drawing.Size(214, 19)
		Me.tx_支払条件他.Location = New System.Drawing.Point(924, 336)
		Me.tx_支払条件他.TabIndex = 68
		Me.tx_支払条件他.Text = "１２３４５６７８９０１２３４５"
		Me.tx_支払条件他.Maxlength = 30
		Me.tx_支払条件他.AcceptsReturn = True
		Me.tx_支払条件他.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_支払条件他.BackColor = System.Drawing.SystemColors.Window
		Me.tx_支払条件他.CausesValidation = True
		Me.tx_支払条件他.Enabled = True
		Me.tx_支払条件他.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_支払条件他.HideSelection = True
		Me.tx_支払条件他.ReadOnly = False
		Me.tx_支払条件他.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_支払条件他.MultiLine = False
		Me.tx_支払条件他.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_支払条件他.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_支払条件他.TabStop = True
		Me.tx_支払条件他.Visible = True
		Me.tx_支払条件他.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_支払条件他.Name = "tx_支払条件他"
		Me.tx_担当者CD.AutoSize = False
		Me.tx_担当者CD.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_担当者CD.Size = New System.Drawing.Size(54, 19)
		Me.tx_担当者CD.Location = New System.Drawing.Point(180, 96)
		Me.tx_担当者CD.TabIndex = 2
		Me.tx_担当者CD.Text = "1"
		Me.tx_担当者CD.Maxlength = 3
		Me.tx_担当者CD.AcceptsReturn = True
		Me.tx_担当者CD.BackColor = System.Drawing.SystemColors.Window
		Me.tx_担当者CD.CausesValidation = True
		Me.tx_担当者CD.Enabled = True
		Me.tx_担当者CD.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_担当者CD.HideSelection = True
		Me.tx_担当者CD.ReadOnly = False
		Me.tx_担当者CD.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_担当者CD.MultiLine = False
		Me.tx_担当者CD.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_担当者CD.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_担当者CD.TabStop = True
		Me.tx_担当者CD.Visible = True
		Me.tx_担当者CD.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_担当者CD.Name = "tx_担当者CD"
		Me.rf_販売先得意先名2.AutoSize = False
		Me.rf_販売先得意先名2.Size = New System.Drawing.Size(237, 19)
		Me.rf_販売先得意先名2.Location = New System.Drawing.Point(420, 372)
		Me.rf_販売先得意先名2.TabIndex = 187
		Me.rf_販売先得意先名2.Text = "１２３４５６７８９０１２３４"
		Me.rf_販売先得意先名2.Maxlength = 28
		Me.rf_販売先得意先名2.AcceptsReturn = True
		Me.rf_販売先得意先名2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.rf_販売先得意先名2.BackColor = System.Drawing.SystemColors.Window
		Me.rf_販売先得意先名2.CausesValidation = True
		Me.rf_販売先得意先名2.Enabled = True
		Me.rf_販売先得意先名2.ForeColor = System.Drawing.SystemColors.WindowText
		Me.rf_販売先得意先名2.HideSelection = True
		Me.rf_販売先得意先名2.ReadOnly = False
		Me.rf_販売先得意先名2.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.rf_販売先得意先名2.MultiLine = False
		Me.rf_販売先得意先名2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.rf_販売先得意先名2.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.rf_販売先得意先名2.TabStop = True
		Me.rf_販売先得意先名2.Visible = True
		Me.rf_販売先得意先名2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.rf_販売先得意先名2.Name = "rf_販売先得意先名2"
		Me.rf_販売先得意先名1.AutoSize = False
		Me.rf_販売先得意先名1.Size = New System.Drawing.Size(237, 19)
		Me.rf_販売先得意先名1.Location = New System.Drawing.Point(180, 372)
		Me.rf_販売先得意先名1.TabIndex = 188
		Me.rf_販売先得意先名1.Text = "１２３４５６７８９０１２３４"
		Me.rf_販売先得意先名1.Maxlength = 28
		Me.rf_販売先得意先名1.AcceptsReturn = True
		Me.rf_販売先得意先名1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.rf_販売先得意先名1.BackColor = System.Drawing.SystemColors.Window
		Me.rf_販売先得意先名1.CausesValidation = True
		Me.rf_販売先得意先名1.Enabled = True
		Me.rf_販売先得意先名1.ForeColor = System.Drawing.SystemColors.WindowText
		Me.rf_販売先得意先名1.HideSelection = True
		Me.rf_販売先得意先名1.ReadOnly = False
		Me.rf_販売先得意先名1.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.rf_販売先得意先名1.MultiLine = False
		Me.rf_販売先得意先名1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.rf_販売先得意先名1.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.rf_販売先得意先名1.TabStop = True
		Me.rf_販売先得意先名1.Visible = True
		Me.rf_販売先得意先名1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.rf_販売先得意先名1.Name = "rf_販売先得意先名1"
		Me.tx_販売先得意先CD.AutoSize = False
		Me.tx_販売先得意先CD.Size = New System.Drawing.Size(65, 19)
		Me.tx_販売先得意先CD.Location = New System.Drawing.Point(112, 372)
		Me.tx_販売先得意先CD.TabIndex = 29
		Me.tx_販売先得意先CD.Text = "WWWW"
		Me.tx_販売先得意先CD.Maxlength = 4
		Me.tx_販売先得意先CD.AcceptsReturn = True
		Me.tx_販売先得意先CD.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_販売先得意先CD.BackColor = System.Drawing.SystemColors.Window
		Me.tx_販売先得意先CD.CausesValidation = True
		Me.tx_販売先得意先CD.Enabled = True
		Me.tx_販売先得意先CD.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_販売先得意先CD.HideSelection = True
		Me.tx_販売先得意先CD.ReadOnly = False
		Me.tx_販売先得意先CD.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_販売先得意先CD.MultiLine = False
		Me.tx_販売先得意先CD.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_販売先得意先CD.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_販売先得意先CD.TabStop = True
		Me.tx_販売先得意先CD.Visible = True
		Me.tx_販売先得意先CD.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_販売先得意先CD.Name = "tx_販売先得意先CD"
		Me.tx_販売先納入先CD.AutoSize = False
		Me.tx_販売先納入先CD.Size = New System.Drawing.Size(39, 19)
		Me.tx_販売先納入先CD.Location = New System.Drawing.Point(180, 396)
		Me.tx_販売先納入先CD.TabIndex = 31
		Me.tx_販売先納入先CD.Text = "WWWW"
		Me.tx_販売先納入先CD.Maxlength = 4
		Me.tx_販売先納入先CD.AcceptsReturn = True
		Me.tx_販売先納入先CD.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_販売先納入先CD.BackColor = System.Drawing.SystemColors.Window
		Me.tx_販売先納入先CD.CausesValidation = True
		Me.tx_販売先納入先CD.Enabled = True
		Me.tx_販売先納入先CD.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_販売先納入先CD.HideSelection = True
		Me.tx_販売先納入先CD.ReadOnly = False
		Me.tx_販売先納入先CD.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_販売先納入先CD.MultiLine = False
		Me.tx_販売先納入先CD.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_販売先納入先CD.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_販売先納入先CD.TabStop = True
		Me.tx_販売先納入先CD.Visible = True
		Me.tx_販売先納入先CD.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_販売先納入先CD.Name = "tx_販売先納入先CD"
		Me.tx_販売先納得意先CD.AutoSize = False
		Me.tx_販売先納得意先CD.Size = New System.Drawing.Size(65, 19)
		Me.tx_販売先納得意先CD.Location = New System.Drawing.Point(112, 396)
		Me.tx_販売先納得意先CD.TabIndex = 30
		Me.tx_販売先納得意先CD.Text = "WWWW"
		Me.tx_販売先納得意先CD.Maxlength = 4
		Me.tx_販売先納得意先CD.AcceptsReturn = True
		Me.tx_販売先納得意先CD.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_販売先納得意先CD.BackColor = System.Drawing.SystemColors.Window
		Me.tx_販売先納得意先CD.CausesValidation = True
		Me.tx_販売先納得意先CD.Enabled = True
		Me.tx_販売先納得意先CD.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_販売先納得意先CD.HideSelection = True
		Me.tx_販売先納得意先CD.ReadOnly = False
		Me.tx_販売先納得意先CD.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_販売先納得意先CD.MultiLine = False
		Me.tx_販売先納得意先CD.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_販売先納得意先CD.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_販売先納得意先CD.TabStop = True
		Me.tx_販売先納得意先CD.Visible = True
		Me.tx_販売先納得意先CD.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_販売先納得意先CD.Name = "tx_販売先納得意先CD"
		Me.rf_販売先納入先名2.AutoSize = False
		Me.rf_販売先納入先名2.Size = New System.Drawing.Size(237, 19)
		Me.rf_販売先納入先名2.Location = New System.Drawing.Point(460, 396)
		Me.rf_販売先納入先名2.TabIndex = 189
		Me.rf_販売先納入先名2.Text = "1234567890123456789012345678"
		Me.rf_販売先納入先名2.Maxlength = 28
		Me.rf_販売先納入先名2.AcceptsReturn = True
		Me.rf_販売先納入先名2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.rf_販売先納入先名2.BackColor = System.Drawing.SystemColors.Window
		Me.rf_販売先納入先名2.CausesValidation = True
		Me.rf_販売先納入先名2.Enabled = True
		Me.rf_販売先納入先名2.ForeColor = System.Drawing.SystemColors.WindowText
		Me.rf_販売先納入先名2.HideSelection = True
		Me.rf_販売先納入先名2.ReadOnly = False
		Me.rf_販売先納入先名2.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.rf_販売先納入先名2.MultiLine = False
		Me.rf_販売先納入先名2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.rf_販売先納入先名2.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.rf_販売先納入先名2.TabStop = True
		Me.rf_販売先納入先名2.Visible = True
		Me.rf_販売先納入先名2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.rf_販売先納入先名2.Name = "rf_販売先納入先名2"
		Me.rf_販売先納入先名1.AutoSize = False
		Me.rf_販売先納入先名1.Size = New System.Drawing.Size(237, 19)
		Me.rf_販売先納入先名1.Location = New System.Drawing.Point(220, 396)
		Me.rf_販売先納入先名1.TabIndex = 190
		Me.rf_販売先納入先名1.Text = "１２３４５６７８９０１２３４"
		Me.rf_販売先納入先名1.Maxlength = 28
		Me.rf_販売先納入先名1.AcceptsReturn = True
		Me.rf_販売先納入先名1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.rf_販売先納入先名1.BackColor = System.Drawing.SystemColors.Window
		Me.rf_販売先納入先名1.CausesValidation = True
		Me.rf_販売先納入先名1.Enabled = True
		Me.rf_販売先納入先名1.ForeColor = System.Drawing.SystemColors.WindowText
		Me.rf_販売先納入先名1.HideSelection = True
		Me.rf_販売先納入先名1.ReadOnly = False
		Me.rf_販売先納入先名1.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.rf_販売先納入先名1.MultiLine = False
		Me.rf_販売先納入先名1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.rf_販売先納入先名1.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.rf_販売先納入先名1.TabStop = True
		Me.rf_販売先納入先名1.Visible = True
		Me.rf_販売先納入先名1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.rf_販売先納入先名1.Name = "rf_販売先納入先名1"
		Me.tx_部署CD.AutoSize = False
		Me.tx_部署CD.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_部署CD.Size = New System.Drawing.Size(63, 19)
		Me.tx_部署CD.Location = New System.Drawing.Point(424, 96)
		Me.tx_部署CD.TabIndex = 3
		Me.tx_部署CD.Text = ""
		Me.tx_部署CD.Maxlength = 4
		Me.tx_部署CD.AcceptsReturn = True
		Me.tx_部署CD.BackColor = System.Drawing.SystemColors.Window
		Me.tx_部署CD.CausesValidation = True
		Me.tx_部署CD.Enabled = True
		Me.tx_部署CD.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_部署CD.HideSelection = True
		Me.tx_部署CD.ReadOnly = False
		Me.tx_部署CD.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_部署CD.MultiLine = False
		Me.tx_部署CD.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_部署CD.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_部署CD.TabStop = True
		Me.tx_部署CD.Visible = True
		Me.tx_部署CD.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_部署CD.Name = "tx_部署CD"
		Me.tx_物件番号.AutoSize = False
		Me.tx_物件番号.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_物件番号.Size = New System.Drawing.Size(65, 19)
		Me.tx_物件番号.Location = New System.Drawing.Point(180, 48)
		Me.tx_物件番号.TabIndex = 0
		Me.tx_物件番号.Text = ""
		Me.tx_物件番号.Maxlength = 7
		Me.tx_物件番号.AcceptsReturn = True
		Me.tx_物件番号.BackColor = System.Drawing.SystemColors.Window
		Me.tx_物件番号.CausesValidation = True
		Me.tx_物件番号.Enabled = True
		Me.tx_物件番号.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_物件番号.HideSelection = True
		Me.tx_物件番号.ReadOnly = False
		Me.tx_物件番号.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_物件番号.MultiLine = False
		Me.tx_物件番号.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_物件番号.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_物件番号.TabStop = True
		Me.tx_物件番号.Visible = True
		Me.tx_物件番号.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_物件番号.Name = "tx_物件番号"
		Me.tx_伝票種類.AutoSize = False
		Me.tx_伝票種類.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_伝票種類.Size = New System.Drawing.Size(26, 19)
		Me.tx_伝票種類.Location = New System.Drawing.Point(900, 48)
		Me.tx_伝票種類.TabIndex = 54
		Me.tx_伝票種類.Text = "1"
		Me.tx_伝票種類.Maxlength = 1
		Me.tx_伝票種類.AcceptsReturn = True
		Me.tx_伝票種類.BackColor = System.Drawing.SystemColors.Window
		Me.tx_伝票種類.CausesValidation = True
		Me.tx_伝票種類.Enabled = True
		Me.tx_伝票種類.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_伝票種類.HideSelection = True
		Me.tx_伝票種類.ReadOnly = False
		Me.tx_伝票種類.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_伝票種類.MultiLine = False
		Me.tx_伝票種類.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_伝票種類.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_伝票種類.TabStop = True
		Me.tx_伝票種類.Visible = True
		Me.tx_伝票種類.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_伝票種類.Name = "tx_伝票種類"
		Me.tx_ウエルシア物件内容名.AutoSize = False
		Me.tx_ウエルシア物件内容名.Size = New System.Drawing.Size(285, 19)
		Me.tx_ウエルシア物件内容名.Location = New System.Drawing.Point(936, 140)
		Me.tx_ウエルシア物件内容名.TabIndex = 59
		Me.tx_ウエルシア物件内容名.Text = "１２３４５６７８９０"
		Me.tx_ウエルシア物件内容名.Maxlength = 40
		Me.tx_ウエルシア物件内容名.AcceptsReturn = True
		Me.tx_ウエルシア物件内容名.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_ウエルシア物件内容名.BackColor = System.Drawing.SystemColors.Window
		Me.tx_ウエルシア物件内容名.CausesValidation = True
		Me.tx_ウエルシア物件内容名.Enabled = True
		Me.tx_ウエルシア物件内容名.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_ウエルシア物件内容名.HideSelection = True
		Me.tx_ウエルシア物件内容名.ReadOnly = False
		Me.tx_ウエルシア物件内容名.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_ウエルシア物件内容名.MultiLine = False
		Me.tx_ウエルシア物件内容名.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_ウエルシア物件内容名.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_ウエルシア物件内容名.TabStop = True
		Me.tx_ウエルシア物件内容名.Visible = True
		Me.tx_ウエルシア物件内容名.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_ウエルシア物件内容名.Name = "tx_ウエルシア物件内容名"
		Me.tx_ウエルシア物件内容CD.AutoSize = False
		Me.tx_ウエルシア物件内容CD.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_ウエルシア物件内容CD.Size = New System.Drawing.Size(34, 19)
		Me.tx_ウエルシア物件内容CD.Location = New System.Drawing.Point(900, 140)
		Me.tx_ウエルシア物件内容CD.TabIndex = 58
		Me.tx_ウエルシア物件内容CD.Text = "1"
		Me.tx_ウエルシア物件内容CD.Maxlength = 3
		Me.tx_ウエルシア物件内容CD.AcceptsReturn = True
		Me.tx_ウエルシア物件内容CD.BackColor = System.Drawing.SystemColors.Window
		Me.tx_ウエルシア物件内容CD.CausesValidation = True
		Me.tx_ウエルシア物件内容CD.Enabled = True
		Me.tx_ウエルシア物件内容CD.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_ウエルシア物件内容CD.HideSelection = True
		Me.tx_ウエルシア物件内容CD.ReadOnly = False
		Me.tx_ウエルシア物件内容CD.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_ウエルシア物件内容CD.MultiLine = False
		Me.tx_ウエルシア物件内容CD.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_ウエルシア物件内容CD.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_ウエルシア物件内容CD.TabStop = True
		Me.tx_ウエルシア物件内容CD.Visible = True
		Me.tx_ウエルシア物件内容CD.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_ウエルシア物件内容CD.Name = "tx_ウエルシア物件内容CD"
		Me.tx_ウエルシア売場面積.AutoSize = False
		Me.tx_ウエルシア売場面積.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_ウエルシア売場面積.Size = New System.Drawing.Size(106, 19)
		Me.tx_ウエルシア売場面積.Location = New System.Drawing.Point(900, 160)
		Me.tx_ウエルシア売場面積.TabIndex = 60
		Me.tx_ウエルシア売場面積.Text = "123456789"
		Me.tx_ウエルシア売場面積.Maxlength = 9
		Me.tx_ウエルシア売場面積.AcceptsReturn = True
		Me.tx_ウエルシア売場面積.BackColor = System.Drawing.SystemColors.Window
		Me.tx_ウエルシア売場面積.CausesValidation = True
		Me.tx_ウエルシア売場面積.Enabled = True
		Me.tx_ウエルシア売場面積.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_ウエルシア売場面積.HideSelection = True
		Me.tx_ウエルシア売場面積.ReadOnly = False
		Me.tx_ウエルシア売場面積.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_ウエルシア売場面積.MultiLine = False
		Me.tx_ウエルシア売場面積.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_ウエルシア売場面積.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_ウエルシア売場面積.TabStop = True
		Me.tx_ウエルシア売場面積.Visible = True
		Me.tx_ウエルシア売場面積.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_ウエルシア売場面積.Name = "tx_ウエルシア売場面積"
		Me.tx_受付日付Y.AutoSize = False
		Me.tx_受付日付Y.Size = New System.Drawing.Size(33, 13)
		Me.tx_受付日付Y.Location = New System.Drawing.Point(206, 555)
		Me.tx_受付日付Y.TabIndex = 45
		Me.tx_受付日付Y.Text = "8888"
		Me.tx_受付日付Y.Maxlength = 4
		Me.tx_受付日付Y.AcceptsReturn = True
		Me.tx_受付日付Y.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_受付日付Y.BackColor = System.Drawing.SystemColors.Window
		Me.tx_受付日付Y.CausesValidation = True
		Me.tx_受付日付Y.Enabled = True
		Me.tx_受付日付Y.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_受付日付Y.HideSelection = True
		Me.tx_受付日付Y.ReadOnly = False
		Me.tx_受付日付Y.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_受付日付Y.MultiLine = False
		Me.tx_受付日付Y.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_受付日付Y.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_受付日付Y.TabStop = True
		Me.tx_受付日付Y.Visible = True
		Me.tx_受付日付Y.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_受付日付Y.Name = "tx_受付日付Y"
		Me.tx_受付日付M.AutoSize = False
		Me.tx_受付日付M.Size = New System.Drawing.Size(16, 13)
		Me.tx_受付日付M.Location = New System.Drawing.Point(257, 555)
		Me.tx_受付日付M.TabIndex = 46
		Me.tx_受付日付M.Text = "88"
		Me.tx_受付日付M.Maxlength = 2
		Me.tx_受付日付M.AcceptsReturn = True
		Me.tx_受付日付M.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_受付日付M.BackColor = System.Drawing.SystemColors.Window
		Me.tx_受付日付M.CausesValidation = True
		Me.tx_受付日付M.Enabled = True
		Me.tx_受付日付M.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_受付日付M.HideSelection = True
		Me.tx_受付日付M.ReadOnly = False
		Me.tx_受付日付M.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_受付日付M.MultiLine = False
		Me.tx_受付日付M.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_受付日付M.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_受付日付M.TabStop = True
		Me.tx_受付日付M.Visible = True
		Me.tx_受付日付M.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_受付日付M.Name = "tx_受付日付M"
		Me.tx_受付日付D.AutoSize = False
		Me.tx_受付日付D.Size = New System.Drawing.Size(16, 13)
		Me.tx_受付日付D.Location = New System.Drawing.Point(292, 555)
		Me.tx_受付日付D.TabIndex = 47
		Me.tx_受付日付D.Text = "88"
		Me.tx_受付日付D.Maxlength = 2
		Me.tx_受付日付D.AcceptsReturn = True
		Me.tx_受付日付D.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_受付日付D.BackColor = System.Drawing.SystemColors.Window
		Me.tx_受付日付D.CausesValidation = True
		Me.tx_受付日付D.Enabled = True
		Me.tx_受付日付D.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_受付日付D.HideSelection = True
		Me.tx_受付日付D.ReadOnly = False
		Me.tx_受付日付D.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_受付日付D.MultiLine = False
		Me.tx_受付日付D.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_受付日付D.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_受付日付D.TabStop = True
		Me.tx_受付日付D.Visible = True
		Me.tx_受付日付D.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_受付日付D.Name = "tx_受付日付D"
		Me.tx_完工日付Y.AutoSize = False
		Me.tx_完工日付Y.Size = New System.Drawing.Size(33, 13)
		Me.tx_完工日付Y.Location = New System.Drawing.Point(206, 575)
		Me.tx_完工日付Y.TabIndex = 48
		Me.tx_完工日付Y.Text = "8888"
		Me.tx_完工日付Y.Maxlength = 4
		Me.tx_完工日付Y.AcceptsReturn = True
		Me.tx_完工日付Y.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_完工日付Y.BackColor = System.Drawing.SystemColors.Window
		Me.tx_完工日付Y.CausesValidation = True
		Me.tx_完工日付Y.Enabled = True
		Me.tx_完工日付Y.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_完工日付Y.HideSelection = True
		Me.tx_完工日付Y.ReadOnly = False
		Me.tx_完工日付Y.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_完工日付Y.MultiLine = False
		Me.tx_完工日付Y.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_完工日付Y.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_完工日付Y.TabStop = True
		Me.tx_完工日付Y.Visible = True
		Me.tx_完工日付Y.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_完工日付Y.Name = "tx_完工日付Y"
		Me.tx_完工日付M.AutoSize = False
		Me.tx_完工日付M.Size = New System.Drawing.Size(16, 13)
		Me.tx_完工日付M.Location = New System.Drawing.Point(257, 575)
		Me.tx_完工日付M.TabIndex = 49
		Me.tx_完工日付M.Text = "88"
		Me.tx_完工日付M.Maxlength = 2
		Me.tx_完工日付M.AcceptsReturn = True
		Me.tx_完工日付M.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_完工日付M.BackColor = System.Drawing.SystemColors.Window
		Me.tx_完工日付M.CausesValidation = True
		Me.tx_完工日付M.Enabled = True
		Me.tx_完工日付M.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_完工日付M.HideSelection = True
		Me.tx_完工日付M.ReadOnly = False
		Me.tx_完工日付M.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_完工日付M.MultiLine = False
		Me.tx_完工日付M.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_完工日付M.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_完工日付M.TabStop = True
		Me.tx_完工日付M.Visible = True
		Me.tx_完工日付M.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_完工日付M.Name = "tx_完工日付M"
		Me.tx_完工日付D.AutoSize = False
		Me.tx_完工日付D.Size = New System.Drawing.Size(16, 13)
		Me.tx_完工日付D.Location = New System.Drawing.Point(292, 575)
		Me.tx_完工日付D.TabIndex = 50
		Me.tx_完工日付D.Text = "88"
		Me.tx_完工日付D.Maxlength = 2
		Me.tx_完工日付D.AcceptsReturn = True
		Me.tx_完工日付D.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_完工日付D.BackColor = System.Drawing.SystemColors.Window
		Me.tx_完工日付D.CausesValidation = True
		Me.tx_完工日付D.Enabled = True
		Me.tx_完工日付D.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_完工日付D.HideSelection = True
		Me.tx_完工日付D.ReadOnly = False
		Me.tx_完工日付D.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_完工日付D.MultiLine = False
		Me.tx_完工日付D.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_完工日付D.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_完工日付D.TabStop = True
		Me.tx_完工日付D.Visible = True
		Me.tx_完工日付D.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_完工日付D.Name = "tx_完工日付D"
		Me.tx_発注担当者名.AutoSize = False
		Me.tx_発注担当者名.Size = New System.Drawing.Size(290, 19)
		Me.tx_発注担当者名.Location = New System.Drawing.Point(200, 592)
		Me.tx_発注担当者名.TabIndex = 51
		Me.tx_発注担当者名.Text = "１２３４５６７８９０１２３４５６７８９０"
		Me.tx_発注担当者名.Maxlength = 40
		Me.tx_発注担当者名.AcceptsReturn = True
		Me.tx_発注担当者名.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_発注担当者名.BackColor = System.Drawing.SystemColors.Window
		Me.tx_発注担当者名.CausesValidation = True
		Me.tx_発注担当者名.Enabled = True
		Me.tx_発注担当者名.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_発注担当者名.HideSelection = True
		Me.tx_発注担当者名.ReadOnly = False
		Me.tx_発注担当者名.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_発注担当者名.MultiLine = False
		Me.tx_発注担当者名.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_発注担当者名.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_発注担当者名.TabStop = True
		Me.tx_発注担当者名.Visible = True
		Me.tx_発注担当者名.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_発注担当者名.Name = "tx_発注担当者名"
		Me.tx_作業内容.AutoSize = False
		Me.tx_作業内容.Size = New System.Drawing.Size(290, 19)
		Me.tx_作業内容.Location = New System.Drawing.Point(200, 612)
		Me.tx_作業内容.TabIndex = 52
		Me.tx_作業内容.Text = "１２３４５６７８９０１２３４５６７８９０"
		Me.tx_作業内容.Maxlength = 40
		Me.tx_作業内容.AcceptsReturn = True
		Me.tx_作業内容.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_作業内容.BackColor = System.Drawing.SystemColors.Window
		Me.tx_作業内容.CausesValidation = True
		Me.tx_作業内容.Enabled = True
		Me.tx_作業内容.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_作業内容.HideSelection = True
		Me.tx_作業内容.ReadOnly = False
		Me.tx_作業内容.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_作業内容.MultiLine = False
		Me.tx_作業内容.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_作業内容.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_作業内容.TabStop = True
		Me.tx_作業内容.Visible = True
		Me.tx_作業内容.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_作業内容.Name = "tx_作業内容"
		Me.tx_YKサプライ区分.AutoSize = False
		Me.tx_YKサプライ区分.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_YKサプライ区分.Size = New System.Drawing.Size(26, 19)
		Me.tx_YKサプライ区分.Location = New System.Drawing.Point(896, 192)
		Me.tx_YKサプライ区分.TabIndex = 61
		Me.tx_YKサプライ区分.Text = "1"
		Me.tx_YKサプライ区分.Maxlength = 1
		Me.tx_YKサプライ区分.AcceptsReturn = True
		Me.tx_YKサプライ区分.BackColor = System.Drawing.SystemColors.Window
		Me.tx_YKサプライ区分.CausesValidation = True
		Me.tx_YKサプライ区分.Enabled = True
		Me.tx_YKサプライ区分.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_YKサプライ区分.HideSelection = True
		Me.tx_YKサプライ区分.ReadOnly = False
		Me.tx_YKサプライ区分.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_YKサプライ区分.MultiLine = False
		Me.tx_YKサプライ区分.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_YKサプライ区分.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_YKサプライ区分.TabStop = True
		Me.tx_YKサプライ区分.Visible = True
		Me.tx_YKサプライ区分.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_YKサプライ区分.Name = "tx_YKサプライ区分"
		Me.tx_YK物件区分.AutoSize = False
		Me.tx_YK物件区分.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_YK物件区分.Size = New System.Drawing.Size(26, 19)
		Me.tx_YK物件区分.Location = New System.Drawing.Point(896, 212)
		Me.tx_YK物件区分.TabIndex = 62
		Me.tx_YK物件区分.Text = "1"
		Me.tx_YK物件区分.Maxlength = 1
		Me.tx_YK物件区分.AcceptsReturn = True
		Me.tx_YK物件区分.BackColor = System.Drawing.SystemColors.Window
		Me.tx_YK物件区分.CausesValidation = True
		Me.tx_YK物件区分.Enabled = True
		Me.tx_YK物件区分.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_YK物件区分.HideSelection = True
		Me.tx_YK物件区分.ReadOnly = False
		Me.tx_YK物件区分.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_YK物件区分.MultiLine = False
		Me.tx_YK物件区分.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_YK物件区分.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_YK物件区分.TabStop = True
		Me.tx_YK物件区分.Visible = True
		Me.tx_YK物件区分.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_YK物件区分.Name = "tx_YK物件区分"
		Me.tx_YK請求区分.AutoSize = False
		Me.tx_YK請求区分.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_YK請求区分.Size = New System.Drawing.Size(26, 19)
		Me.tx_YK請求区分.Location = New System.Drawing.Point(896, 232)
		Me.tx_YK請求区分.TabIndex = 63
		Me.tx_YK請求区分.Text = "1"
		Me.tx_YK請求区分.Maxlength = 1
		Me.tx_YK請求区分.AcceptsReturn = True
		Me.tx_YK請求区分.BackColor = System.Drawing.SystemColors.Window
		Me.tx_YK請求区分.CausesValidation = True
		Me.tx_YK請求区分.Enabled = True
		Me.tx_YK請求区分.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_YK請求区分.HideSelection = True
		Me.tx_YK請求区分.ReadOnly = False
		Me.tx_YK請求区分.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_YK請求区分.MultiLine = False
		Me.tx_YK請求区分.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_YK請求区分.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_YK請求区分.TabStop = True
		Me.tx_YK請求区分.Visible = True
		Me.tx_YK請求区分.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_YK請求区分.Name = "tx_YK請求区分"
		Me.tx_化粧品メーカー区分.AutoSize = False
		Me.tx_化粧品メーカー区分.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_化粧品メーカー区分.Size = New System.Drawing.Size(26, 19)
		Me.tx_化粧品メーカー区分.Location = New System.Drawing.Point(1024, 24)
		Me.tx_化粧品メーカー区分.TabIndex = 227
		Me.tx_化粧品メーカー区分.Text = "1"
		Me.tx_化粧品メーカー区分.Maxlength = 1
		Me.tx_化粧品メーカー区分.AcceptsReturn = True
		Me.tx_化粧品メーカー区分.BackColor = System.Drawing.SystemColors.Window
		Me.tx_化粧品メーカー区分.CausesValidation = True
		Me.tx_化粧品メーカー区分.Enabled = True
		Me.tx_化粧品メーカー区分.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_化粧品メーカー区分.HideSelection = True
		Me.tx_化粧品メーカー区分.ReadOnly = False
		Me.tx_化粧品メーカー区分.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_化粧品メーカー区分.MultiLine = False
		Me.tx_化粧品メーカー区分.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_化粧品メーカー区分.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_化粧品メーカー区分.TabStop = True
		Me.tx_化粧品メーカー区分.Visible = True
		Me.tx_化粧品メーカー区分.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_化粧品メーカー区分.Name = "tx_化粧品メーカー区分"
		Me.tx_SM内容区分.AutoSize = False
		Me.tx_SM内容区分.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_SM内容区分.Size = New System.Drawing.Size(26, 19)
		Me.tx_SM内容区分.Location = New System.Drawing.Point(900, 68)
		Me.tx_SM内容区分.TabIndex = 55
		Me.tx_SM内容区分.Text = "1"
		Me.tx_SM内容区分.Maxlength = 1
		Me.tx_SM内容区分.AcceptsReturn = True
		Me.tx_SM内容区分.BackColor = System.Drawing.SystemColors.Window
		Me.tx_SM内容区分.CausesValidation = True
		Me.tx_SM内容区分.Enabled = True
		Me.tx_SM内容区分.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_SM内容区分.HideSelection = True
		Me.tx_SM内容区分.ReadOnly = False
		Me.tx_SM内容区分.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_SM内容区分.MultiLine = False
		Me.tx_SM内容区分.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_SM内容区分.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_SM内容区分.TabStop = True
		Me.tx_SM内容区分.Visible = True
		Me.tx_SM内容区分.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_SM内容区分.Name = "tx_SM内容区分"
		Me.tx_税集計区分.AutoSize = False
		Me.tx_税集計区分.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_税集計区分.Size = New System.Drawing.Size(44, 19)
		Me.tx_税集計区分.Location = New System.Drawing.Point(716, 3)
		Me.tx_税集計区分.TabIndex = 15
		Me.tx_税集計区分.Text = ""
		Me.tx_税集計区分.Maxlength = 2
		Me.tx_税集計区分.AcceptsReturn = True
		Me.tx_税集計区分.BackColor = System.Drawing.SystemColors.Window
		Me.tx_税集計区分.CausesValidation = True
		Me.tx_税集計区分.Enabled = True
		Me.tx_税集計区分.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_税集計区分.HideSelection = True
		Me.tx_税集計区分.ReadOnly = False
		Me.tx_税集計区分.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_税集計区分.MultiLine = False
		Me.tx_税集計区分.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_税集計区分.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_税集計区分.TabStop = True
		Me.tx_税集計区分.Visible = True
		Me.tx_税集計区分.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_税集計区分.Name = "tx_税集計区分"
		Me.tx_クレーム区分.AutoSize = False
		Me.tx_クレーム区分.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_クレーム区分.Size = New System.Drawing.Size(26, 19)
		Me.tx_クレーム区分.Location = New System.Drawing.Point(508, 563)
		Me.tx_クレーム区分.TabIndex = 53
		Me.tx_クレーム区分.Text = "1"
		Me.tx_クレーム区分.Maxlength = 1
		Me.tx_クレーム区分.AcceptsReturn = True
		Me.tx_クレーム区分.BackColor = System.Drawing.SystemColors.Window
		Me.tx_クレーム区分.CausesValidation = True
		Me.tx_クレーム区分.Enabled = True
		Me.tx_クレーム区分.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_クレーム区分.HideSelection = True
		Me.tx_クレーム区分.ReadOnly = False
		Me.tx_クレーム区分.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_クレーム区分.MultiLine = False
		Me.tx_クレーム区分.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_クレーム区分.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_クレーム区分.TabStop = True
		Me.tx_クレーム区分.Visible = True
		Me.tx_クレーム区分.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_クレーム区分.Name = "tx_クレーム区分"
		Me.tx_工事担当CD.AutoSize = False
		Me.tx_工事担当CD.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_工事担当CD.Size = New System.Drawing.Size(63, 19)
		Me.tx_工事担当CD.Location = New System.Drawing.Point(424, 116)
		Me.tx_工事担当CD.TabIndex = 7
		Me.tx_工事担当CD.Text = "8"
		Me.tx_工事担当CD.Maxlength = 3
		Me.tx_工事担当CD.AcceptsReturn = True
		Me.tx_工事担当CD.BackColor = System.Drawing.SystemColors.Window
		Me.tx_工事担当CD.CausesValidation = True
		Me.tx_工事担当CD.Enabled = True
		Me.tx_工事担当CD.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_工事担当CD.HideSelection = True
		Me.tx_工事担当CD.ReadOnly = False
		Me.tx_工事担当CD.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_工事担当CD.MultiLine = False
		Me.tx_工事担当CD.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_工事担当CD.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_工事担当CD.TabStop = True
		Me.tx_工事担当CD.Visible = True
		Me.tx_工事担当CD.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_工事担当CD.Name = "tx_工事担当CD"
		Me.tx_発注書発行日付.AutoSize = False
		Me.tx_発注書発行日付.Size = New System.Drawing.Size(110, 19)
		Me.tx_発注書発行日付.Location = New System.Drawing.Point(896, 508)
		Me.tx_発注書発行日付.TabIndex = 79
		Me.tx_発注書発行日付.Text = "2020年04月11日"
		Me.tx_発注書発行日付.Maxlength = 30
		Me.tx_発注書発行日付.AcceptsReturn = True
		Me.tx_発注書発行日付.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_発注書発行日付.BackColor = System.Drawing.SystemColors.Window
		Me.tx_発注書発行日付.CausesValidation = True
		Me.tx_発注書発行日付.Enabled = True
		Me.tx_発注書発行日付.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_発注書発行日付.HideSelection = True
		Me.tx_発注書発行日付.ReadOnly = False
		Me.tx_発注書発行日付.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_発注書発行日付.MultiLine = False
		Me.tx_発注書発行日付.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_発注書発行日付.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_発注書発行日付.TabStop = True
		Me.tx_発注書発行日付.Visible = True
		Me.tx_発注書発行日付.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_発注書発行日付.Name = "tx_発注書発行日付"
		Me.tx_完了者名.AutoSize = False
		Me.tx_完了者名.Size = New System.Drawing.Size(146, 19)
		Me.tx_完了者名.Location = New System.Drawing.Point(1028, 548)
		Me.tx_完了者名.TabIndex = 84
		Me.tx_完了者名.Text = "１２３４５６７８９０"
		Me.tx_完了者名.Maxlength = 20
		Me.tx_完了者名.AcceptsReturn = True
		Me.tx_完了者名.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_完了者名.BackColor = System.Drawing.SystemColors.Window
		Me.tx_完了者名.CausesValidation = True
		Me.tx_完了者名.Enabled = True
		Me.tx_完了者名.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_完了者名.HideSelection = True
		Me.tx_完了者名.ReadOnly = False
		Me.tx_完了者名.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_完了者名.MultiLine = False
		Me.tx_完了者名.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_完了者名.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_完了者名.TabStop = True
		Me.tx_完了者名.Visible = True
		Me.tx_完了者名.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_完了者名.Name = "tx_完了者名"
		Me.tx_見積確定区分.AutoSize = False
		Me.tx_見積確定区分.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_見積確定区分.Size = New System.Drawing.Size(26, 19)
		Me.tx_見積確定区分.Location = New System.Drawing.Point(896, 528)
		Me.tx_見積確定区分.TabIndex = 80
		Me.tx_見積確定区分.Text = "1"
		Me.tx_見積確定区分.Maxlength = 1
		Me.tx_見積確定区分.AcceptsReturn = True
		Me.tx_見積確定区分.BackColor = System.Drawing.SystemColors.Window
		Me.tx_見積確定区分.CausesValidation = True
		Me.tx_見積確定区分.Enabled = True
		Me.tx_見積確定区分.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_見積確定区分.HideSelection = True
		Me.tx_見積確定区分.ReadOnly = False
		Me.tx_見積確定区分.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_見積確定区分.MultiLine = False
		Me.tx_見積確定区分.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_見積確定区分.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_見積確定区分.TabStop = True
		Me.tx_見積確定区分.Visible = True
		Me.tx_見積確定区分.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_見積確定区分.Name = "tx_見積確定区分"
		Me.tx_経過備考1.AutoSize = False
		Me.tx_経過備考1.Size = New System.Drawing.Size(330, 19)
		Me.tx_経過備考1.Location = New System.Drawing.Point(896, 587)
		Me.tx_経過備考1.TabIndex = 88
		Me.tx_経過備考1.Text = "１２３４５６７８９０１２３４５６７８９０１２３"
		Me.tx_経過備考1.Maxlength = 46
		Me.tx_経過備考1.AcceptsReturn = True
		Me.tx_経過備考1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_経過備考1.BackColor = System.Drawing.SystemColors.Window
		Me.tx_経過備考1.CausesValidation = True
		Me.tx_経過備考1.Enabled = True
		Me.tx_経過備考1.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_経過備考1.HideSelection = True
		Me.tx_経過備考1.ReadOnly = False
		Me.tx_経過備考1.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_経過備考1.MultiLine = False
		Me.tx_経過備考1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_経過備考1.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_経過備考1.TabStop = True
		Me.tx_経過備考1.Visible = True
		Me.tx_経過備考1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_経過備考1.Name = "tx_経過備考1"
		Me.tx_ウエルシア物件区分.AutoSize = False
		Me.tx_ウエルシア物件区分.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_ウエルシア物件区分.Size = New System.Drawing.Size(26, 19)
		Me.tx_ウエルシア物件区分.Location = New System.Drawing.Point(900, 120)
		Me.tx_ウエルシア物件区分.TabIndex = 57
		Me.tx_ウエルシア物件区分.Text = "1"
		Me.tx_ウエルシア物件区分.Maxlength = 2
		Me.tx_ウエルシア物件区分.AcceptsReturn = True
		Me.tx_ウエルシア物件区分.BackColor = System.Drawing.SystemColors.Window
		Me.tx_ウエルシア物件区分.CausesValidation = True
		Me.tx_ウエルシア物件区分.Enabled = True
		Me.tx_ウエルシア物件区分.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_ウエルシア物件区分.HideSelection = True
		Me.tx_ウエルシア物件区分.ReadOnly = False
		Me.tx_ウエルシア物件区分.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_ウエルシア物件区分.MultiLine = False
		Me.tx_ウエルシア物件区分.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_ウエルシア物件区分.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_ウエルシア物件区分.TabStop = True
		Me.tx_ウエルシア物件区分.Visible = True
		Me.tx_ウエルシア物件区分.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_ウエルシア物件区分.Name = "tx_ウエルシア物件区分"
		Me.tx_ウエルシアリース区分.AutoSize = False
		Me.tx_ウエルシアリース区分.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_ウエルシアリース区分.Size = New System.Drawing.Size(26, 19)
		Me.tx_ウエルシアリース区分.Location = New System.Drawing.Point(900, 100)
		Me.tx_ウエルシアリース区分.TabIndex = 56
		Me.tx_ウエルシアリース区分.Text = "1"
		Me.tx_ウエルシアリース区分.Maxlength = 1
		Me.tx_ウエルシアリース区分.AcceptsReturn = True
		Me.tx_ウエルシアリース区分.BackColor = System.Drawing.SystemColors.Window
		Me.tx_ウエルシアリース区分.CausesValidation = True
		Me.tx_ウエルシアリース区分.Enabled = True
		Me.tx_ウエルシアリース区分.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_ウエルシアリース区分.HideSelection = True
		Me.tx_ウエルシアリース区分.ReadOnly = False
		Me.tx_ウエルシアリース区分.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_ウエルシアリース区分.MultiLine = False
		Me.tx_ウエルシアリース区分.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_ウエルシアリース区分.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_ウエルシアリース区分.TabStop = True
		Me.tx_ウエルシアリース区分.Visible = True
		Me.tx_ウエルシアリース区分.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_ウエルシアリース区分.Name = "tx_ウエルシアリース区分"
		Me.tx_経過備考2.AutoSize = False
		Me.tx_経過備考2.Size = New System.Drawing.Size(330, 19)
		Me.tx_経過備考2.Location = New System.Drawing.Point(896, 606)
		Me.tx_経過備考2.TabIndex = 89
		Me.tx_経過備考2.Text = "１２３４５６７８９０１２３４５６７８９０１２３"
		Me.tx_経過備考2.Maxlength = 46
		Me.tx_経過備考2.AcceptsReturn = True
		Me.tx_経過備考2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_経過備考2.BackColor = System.Drawing.SystemColors.Window
		Me.tx_経過備考2.CausesValidation = True
		Me.tx_経過備考2.Enabled = True
		Me.tx_経過備考2.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_経過備考2.HideSelection = True
		Me.tx_経過備考2.ReadOnly = False
		Me.tx_経過備考2.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_経過備考2.MultiLine = False
		Me.tx_経過備考2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_経過備考2.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_経過備考2.TabStop = True
		Me.tx_経過備考2.Visible = True
		Me.tx_経過備考2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_経過備考2.Name = "tx_経過備考2"
		Me.tx_請求予定M.AutoSize = False
		Me.tx_請求予定M.Size = New System.Drawing.Size(16, 13)
		Me.tx_請求予定M.Location = New System.Drawing.Point(953, 571)
		Me.tx_請求予定M.TabIndex = 86
		Me.tx_請求予定M.Text = "88"
		Me.tx_請求予定M.Maxlength = 2
		Me.tx_請求予定M.AcceptsReturn = True
		Me.tx_請求予定M.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_請求予定M.BackColor = System.Drawing.SystemColors.Window
		Me.tx_請求予定M.CausesValidation = True
		Me.tx_請求予定M.Enabled = True
		Me.tx_請求予定M.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_請求予定M.HideSelection = True
		Me.tx_請求予定M.ReadOnly = False
		Me.tx_請求予定M.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_請求予定M.MultiLine = False
		Me.tx_請求予定M.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_請求予定M.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_請求予定M.TabStop = True
		Me.tx_請求予定M.Visible = True
		Me.tx_請求予定M.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_請求予定M.Name = "tx_請求予定M"
		Me.tx_請求予定D.AutoSize = False
		Me.tx_請求予定D.Size = New System.Drawing.Size(16, 13)
		Me.tx_請求予定D.Location = New System.Drawing.Point(988, 571)
		Me.tx_請求予定D.TabIndex = 87
		Me.tx_請求予定D.Text = "88"
		Me.tx_請求予定D.Maxlength = 2
		Me.tx_請求予定D.AcceptsReturn = True
		Me.tx_請求予定D.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_請求予定D.BackColor = System.Drawing.SystemColors.Window
		Me.tx_請求予定D.CausesValidation = True
		Me.tx_請求予定D.Enabled = True
		Me.tx_請求予定D.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_請求予定D.HideSelection = True
		Me.tx_請求予定D.ReadOnly = False
		Me.tx_請求予定D.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_請求予定D.MultiLine = False
		Me.tx_請求予定D.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_請求予定D.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_請求予定D.TabStop = True
		Me.tx_請求予定D.Visible = True
		Me.tx_請求予定D.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_請求予定D.Name = "tx_請求予定D"
		Me.tx_完了日Y.AutoSize = False
		Me.tx_完了日Y.Size = New System.Drawing.Size(33, 13)
		Me.tx_完了日Y.Location = New System.Drawing.Point(902, 551)
		Me.tx_完了日Y.TabIndex = 81
		Me.tx_完了日Y.Text = "8888"
		Me.tx_完了日Y.Maxlength = 4
		Me.tx_完了日Y.AcceptsReturn = True
		Me.tx_完了日Y.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_完了日Y.BackColor = System.Drawing.SystemColors.Window
		Me.tx_完了日Y.CausesValidation = True
		Me.tx_完了日Y.Enabled = True
		Me.tx_完了日Y.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_完了日Y.HideSelection = True
		Me.tx_完了日Y.ReadOnly = False
		Me.tx_完了日Y.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_完了日Y.MultiLine = False
		Me.tx_完了日Y.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_完了日Y.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_完了日Y.TabStop = True
		Me.tx_完了日Y.Visible = True
		Me.tx_完了日Y.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_完了日Y.Name = "tx_完了日Y"
		Me.tx_完了日M.AutoSize = False
		Me.tx_完了日M.Size = New System.Drawing.Size(16, 13)
		Me.tx_完了日M.Location = New System.Drawing.Point(953, 551)
		Me.tx_完了日M.TabIndex = 82
		Me.tx_完了日M.Text = "88"
		Me.tx_完了日M.Maxlength = 2
		Me.tx_完了日M.AcceptsReturn = True
		Me.tx_完了日M.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_完了日M.BackColor = System.Drawing.SystemColors.Window
		Me.tx_完了日M.CausesValidation = True
		Me.tx_完了日M.Enabled = True
		Me.tx_完了日M.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_完了日M.HideSelection = True
		Me.tx_完了日M.ReadOnly = False
		Me.tx_完了日M.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_完了日M.MultiLine = False
		Me.tx_完了日M.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_完了日M.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_完了日M.TabStop = True
		Me.tx_完了日M.Visible = True
		Me.tx_完了日M.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_完了日M.Name = "tx_完了日M"
		Me.tx_完了日D.AutoSize = False
		Me.tx_完了日D.Size = New System.Drawing.Size(16, 13)
		Me.tx_完了日D.Location = New System.Drawing.Point(988, 551)
		Me.tx_完了日D.TabIndex = 83
		Me.tx_完了日D.Text = "88"
		Me.tx_完了日D.Maxlength = 2
		Me.tx_完了日D.AcceptsReturn = True
		Me.tx_完了日D.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_完了日D.BackColor = System.Drawing.SystemColors.Window
		Me.tx_完了日D.CausesValidation = True
		Me.tx_完了日D.Enabled = True
		Me.tx_完了日D.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_完了日D.HideSelection = True
		Me.tx_完了日D.ReadOnly = False
		Me.tx_完了日D.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_完了日D.MultiLine = False
		Me.tx_完了日D.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_完了日D.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_完了日D.TabStop = True
		Me.tx_完了日D.Visible = True
		Me.tx_完了日D.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_完了日D.Name = "tx_完了日D"
		Me.tx_請求予定Y.AutoSize = False
		Me.tx_請求予定Y.Size = New System.Drawing.Size(33, 13)
		Me.tx_請求予定Y.Location = New System.Drawing.Point(902, 571)
		Me.tx_請求予定Y.TabIndex = 85
		Me.tx_請求予定Y.Text = "8888"
		Me.tx_請求予定Y.Maxlength = 4
		Me.tx_請求予定Y.AcceptsReturn = True
		Me.tx_請求予定Y.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_請求予定Y.BackColor = System.Drawing.SystemColors.Window
		Me.tx_請求予定Y.CausesValidation = True
		Me.tx_請求予定Y.Enabled = True
		Me.tx_請求予定Y.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_請求予定Y.HideSelection = True
		Me.tx_請求予定Y.ReadOnly = False
		Me.tx_請求予定Y.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_請求予定Y.MultiLine = False
		Me.tx_請求予定Y.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_請求予定Y.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_請求予定Y.TabStop = True
		Me.tx_請求予定Y.Visible = True
		Me.tx_請求予定Y.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_請求予定Y.Name = "tx_請求予定Y"
		Me.tx_集計CD.AutoSize = False
		Me.tx_集計CD.Size = New System.Drawing.Size(65, 19)
		Me.tx_集計CD.Location = New System.Drawing.Point(180, 231)
		Me.tx_集計CD.TabIndex = 16
		Me.tx_集計CD.Text = "WWWW"
		Me.tx_集計CD.Maxlength = 4
		Me.tx_集計CD.AcceptsReturn = True
		Me.tx_集計CD.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.tx_集計CD.BackColor = System.Drawing.SystemColors.Window
		Me.tx_集計CD.CausesValidation = True
		Me.tx_集計CD.Enabled = True
		Me.tx_集計CD.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_集計CD.HideSelection = True
		Me.tx_集計CD.ReadOnly = False
		Me.tx_集計CD.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_集計CD.MultiLine = False
		Me.tx_集計CD.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_集計CD.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_集計CD.TabStop = True
		Me.tx_集計CD.Visible = True
		Me.tx_集計CD.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_集計CD.Name = "tx_集計CD"
		Me.tx_B請求管轄区分.AutoSize = False
		Me.tx_B請求管轄区分.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_B請求管轄区分.Size = New System.Drawing.Size(26, 19)
		Me.tx_B請求管轄区分.Location = New System.Drawing.Point(896, 264)
		Me.tx_B請求管轄区分.TabIndex = 64
		Me.tx_B請求管轄区分.Text = "1"
		Me.tx_B請求管轄区分.Maxlength = 1
		Me.tx_B請求管轄区分.AcceptsReturn = True
		Me.tx_B請求管轄区分.BackColor = System.Drawing.SystemColors.Window
		Me.tx_B請求管轄区分.CausesValidation = True
		Me.tx_B請求管轄区分.Enabled = True
		Me.tx_B請求管轄区分.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_B請求管轄区分.HideSelection = True
		Me.tx_B請求管轄区分.ReadOnly = False
		Me.tx_B請求管轄区分.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_B請求管轄区分.MultiLine = False
		Me.tx_B請求管轄区分.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_B請求管轄区分.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_B請求管轄区分.TabStop = True
		Me.tx_B請求管轄区分.Visible = True
		Me.tx_B請求管轄区分.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_B請求管轄区分.Name = "tx_B請求管轄区分"
		Me.tx_BtoB番号.AutoSize = False
		Me.tx_BtoB番号.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_BtoB番号.Size = New System.Drawing.Size(54, 19)
		Me.tx_BtoB番号.Location = New System.Drawing.Point(1152, 264)
		Me.tx_BtoB番号.TabIndex = 65
		Me.tx_BtoB番号.Text = "12345"
		Me.tx_BtoB番号.Maxlength = 5
		Me.tx_BtoB番号.AcceptsReturn = True
		Me.tx_BtoB番号.BackColor = System.Drawing.SystemColors.Window
		Me.tx_BtoB番号.CausesValidation = True
		Me.tx_BtoB番号.Enabled = True
		Me.tx_BtoB番号.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_BtoB番号.HideSelection = True
		Me.tx_BtoB番号.ReadOnly = False
		Me.tx_BtoB番号.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_BtoB番号.MultiLine = False
		Me.tx_BtoB番号.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_BtoB番号.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_BtoB番号.TabStop = True
		Me.tx_BtoB番号.Visible = True
		Me.tx_BtoB番号.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_BtoB番号.Name = "tx_BtoB番号"
		Me.tx_業種区分.AutoSize = False
		Me.tx_業種区分.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_業種区分.Size = New System.Drawing.Size(26, 19)
		Me.tx_業種区分.Location = New System.Drawing.Point(200, 492)
		Me.tx_業種区分.TabIndex = 39
		Me.tx_業種区分.Text = "1"
		Me.tx_業種区分.Maxlength = 1
		Me.tx_業種区分.AcceptsReturn = True
		Me.tx_業種区分.BackColor = System.Drawing.SystemColors.Window
		Me.tx_業種区分.CausesValidation = True
		Me.tx_業種区分.Enabled = True
		Me.tx_業種区分.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_業種区分.HideSelection = True
		Me.tx_業種区分.ReadOnly = False
		Me.tx_業種区分.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_業種区分.MultiLine = False
		Me.tx_業種区分.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_業種区分.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_業種区分.TabStop = True
		Me.tx_業種区分.Visible = True
		Me.tx_業種区分.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_業種区分.Name = "tx_業種区分"
		Me.tx_統合見積番号.AutoSize = False
		Me.tx_統合見積番号.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.tx_統合見積番号.Size = New System.Drawing.Size(65, 19)
		Me.tx_統合見積番号.Location = New System.Drawing.Point(180, 68)
		Me.tx_統合見積番号.TabIndex = 1
		Me.tx_統合見積番号.Text = ""
		Me.tx_統合見積番号.Maxlength = 7
		Me.tx_統合見積番号.AcceptsReturn = True
		Me.tx_統合見積番号.BackColor = System.Drawing.SystemColors.Window
		Me.tx_統合見積番号.CausesValidation = True
		Me.tx_統合見積番号.Enabled = True
		Me.tx_統合見積番号.ForeColor = System.Drawing.SystemColors.WindowText
		Me.tx_統合見積番号.HideSelection = True
		Me.tx_統合見積番号.ReadOnly = False
		Me.tx_統合見積番号.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.tx_統合見積番号.MultiLine = False
		Me.tx_統合見積番号.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.tx_統合見積番号.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.tx_統合見積番号.TabStop = True
		Me.tx_統合見積番号.Visible = True
		Me.tx_統合見積番号.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tx_統合見積番号.Name = "tx_統合見積番号"
		Me.rf_統合見積件名.Text = "ＷＷＷＷＷＷＷＷ"
		Me.rf_統合見積件名.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.rf_統合見積件名.Size = New System.Drawing.Size(430, 19)
		Me.rf_統合見積件名.Location = New System.Drawing.Point(245, 68)
		Me.rf_統合見積件名.TabIndex = 262
		Me.rf_統合見積件名.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.rf_統合見積件名.BackColor = System.Drawing.SystemColors.Control
		Me.rf_統合見積件名.Enabled = True
		Me.rf_統合見積件名.ForeColor = System.Drawing.SystemColors.ControlText
		Me.rf_統合見積件名.Cursor = System.Windows.Forms.Cursors.Default
		Me.rf_統合見積件名.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.rf_統合見積件名.UseMnemonic = True
		Me.rf_統合見積件名.Visible = True
		Me.rf_統合見積件名.AutoSize = False
		Me.rf_統合見積件名.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.rf_統合見積件名.Name = "rf_統合見積件名"
		Me._lblLabels_58.Text = "業種区分"
		Me._lblLabels_58.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_58.Size = New System.Drawing.Size(73, 13)
		Me._lblLabels_58.Location = New System.Drawing.Point(112, 496)
		Me._lblLabels_58.TabIndex = 261
		Me._lblLabels_58.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_58.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_58.Enabled = True
		Me._lblLabels_58.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_58.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_58.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_58.UseMnemonic = True
		Me._lblLabels_58.Visible = True
		Me._lblLabels_58.AutoSize = False
		Me._lblLabels_58.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_58.Name = "_lblLabels_58"
		Me._lblLabels_57.Text = "0:什器 1:内装"
		Me._lblLabels_57.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_57.Size = New System.Drawing.Size(433, 13)
		Me._lblLabels_57.Location = New System.Drawing.Point(236, 496)
		Me._lblLabels_57.TabIndex = 260
		Me._lblLabels_57.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_57.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_57.Enabled = True
		Me._lblLabels_57.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_57.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_57.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_57.UseMnemonic = True
		Me._lblLabels_57.Visible = True
		Me._lblLabels_57.AutoSize = False
		Me._lblLabels_57.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_57.Name = "_lblLabels_57"
		Me._lblLabels_56.Text = "BtoB番号"
		Me._lblLabels_56.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_56.Size = New System.Drawing.Size(61, 13)
		Me._lblLabels_56.Location = New System.Drawing.Point(1088, 268)
		Me._lblLabels_56.TabIndex = 259
		Me._lblLabels_56.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_56.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_56.Enabled = True
		Me._lblLabels_56.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_56.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_56.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_56.UseMnemonic = True
		Me._lblLabels_56.Visible = True
		Me._lblLabels_56.AutoSize = False
		Me._lblLabels_56.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_56.Name = "_lblLabels_56"
		Me._lblLabels_55.Text = "1:請求書 2:BtoB"
		Me._lblLabels_55.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_55.Size = New System.Drawing.Size(133, 13)
		Me._lblLabels_55.Location = New System.Drawing.Point(932, 268)
		Me._lblLabels_55.TabIndex = 258
		Me._lblLabels_55.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_55.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_55.Enabled = True
		Me._lblLabels_55.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_55.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_55.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_55.UseMnemonic = True
		Me._lblLabels_55.Visible = True
		Me._lblLabels_55.AutoSize = False
		Me._lblLabels_55.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_55.Name = "_lblLabels_55"
		Me._lblLabels_54.Text = "請求管轄"
		Me._lblLabels_54.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_54.Size = New System.Drawing.Size(73, 13)
		Me._lblLabels_54.Location = New System.Drawing.Point(812, 268)
		Me._lblLabels_54.TabIndex = 257
		Me._lblLabels_54.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_54.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_54.Enabled = True
		Me._lblLabels_54.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_54.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_54.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_54.UseMnemonic = True
		Me._lblLabels_54.Visible = True
		Me._lblLabels_54.AutoSize = False
		Me._lblLabels_54.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_54.Name = "_lblLabels_54"
		Me._lb_項目_15.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me._lb_項目_15.Text = "ベルク情報"
		Me._lb_項目_15.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_15.ForeColor = System.Drawing.Color.White
		Me._lb_項目_15.Size = New System.Drawing.Size(101, 29)
		Me._lb_項目_15.Location = New System.Drawing.Point(704, 260)
		Me._lb_項目_15.TabIndex = 256
		Me._lb_項目_15.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lb_項目_15.Enabled = True
		Me._lb_項目_15.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_15.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_15.UseMnemonic = True
		Me._lb_項目_15.Visible = True
		Me._lb_項目_15.AutoSize = False
		Me._lb_項目_15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me._lb_項目_15.Name = "_lb_項目_15"
		Me._Shape1_13.BorderColor = System.Drawing.SystemColors.ControlDark
		Me._Shape1_13.Size = New System.Drawing.Size(421, 29)
		Me._Shape1_13.Location = New System.Drawing.Point(808, 260)
		Me._Shape1_13.BackColor = System.Drawing.SystemColors.Window
		Me._Shape1_13.BackStyle = Microsoft.VisualBasic.PowerPacks.BackStyle.Transparent
		Me._Shape1_13.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid
		Me._Shape1_13.BorderWidth = 1
		Me._Shape1_13.FillColor = System.Drawing.Color.Black
		Me._Shape1_13.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Transparent
		Me._Shape1_13.Visible = True
		Me._Shape1_13.Name = "_Shape1_13"
		Me.rf_集計名.Text = "１２３４５６７８９０１２３４５"
		Me.rf_集計名.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.rf_集計名.Size = New System.Drawing.Size(100, 19)
		Me.rf_集計名.Location = New System.Drawing.Point(245, 231)
		Me.rf_集計名.TabIndex = 255
		Me.rf_集計名.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.rf_集計名.BackColor = System.Drawing.SystemColors.Control
		Me.rf_集計名.Enabled = True
		Me.rf_集計名.ForeColor = System.Drawing.SystemColors.ControlText
		Me.rf_集計名.Cursor = System.Windows.Forms.Cursors.Default
		Me.rf_集計名.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.rf_集計名.UseMnemonic = True
		Me.rf_集計名.Visible = True
		Me.rf_集計名.AutoSize = False
		Me.rf_集計名.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.rf_集計名.Name = "rf_集計名"
		Me._lblLabels_53.Text = "集計CD"
		Me._lblLabels_53.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_53.Size = New System.Drawing.Size(43, 13)
		Me._lblLabels_53.Location = New System.Drawing.Point(128, 233)
		Me._lblLabels_53.TabIndex = 254
		Me._lblLabels_53.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_53.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_53.Enabled = True
		Me._lblLabels_53.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_53.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_53.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_53.UseMnemonic = True
		Me._lblLabels_53.Visible = True
		Me._lblLabels_53.AutoSize = False
		Me._lblLabels_53.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_53.Name = "_lblLabels_53"
		Me._lb_日_8.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_日_8.BackColor = System.Drawing.Color.Transparent
		Me._lb_日_8.Text = "日"
		Me._lb_日_8.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_日_8.Size = New System.Drawing.Size(12, 15)
		Me._lb_日_8.Location = New System.Drawing.Point(1008, 572)
		Me._lb_日_8.TabIndex = 253
		Me._lb_日_8.Enabled = True
		Me._lb_日_8.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_日_8.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_日_8.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_日_8.UseMnemonic = True
		Me._lb_日_8.Visible = True
		Me._lb_日_8.AutoSize = False
		Me._lb_日_8.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_日_8.Name = "_lb_日_8"
		Me.Label14.Text = "請求予定日"
		Me.Label14.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.Label14.Size = New System.Drawing.Size(77, 13)
		Me.Label14.Location = New System.Drawing.Point(812, 572)
		Me.Label14.TabIndex = 252
		Me.Label14.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label14.BackColor = System.Drawing.SystemColors.Control
		Me.Label14.Enabled = True
		Me.Label14.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label14.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label14.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label14.UseMnemonic = True
		Me.Label14.Visible = True
		Me.Label14.AutoSize = False
		Me.Label14.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label14.Name = "Label14"
		Me._lb_項目_14.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_項目_14.BackColor = System.Drawing.Color.Transparent
		Me._lb_項目_14.Text = "月"
		Me._lb_項目_14.Size = New System.Drawing.Size(12, 12)
		Me._lb_項目_14.Location = New System.Drawing.Point(970, 572)
		Me._lb_項目_14.TabIndex = 250
		Me._lb_項目_14.Enabled = True
		Me._lb_項目_14.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_項目_14.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_14.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_14.UseMnemonic = True
		Me._lb_項目_14.Visible = True
		Me._lb_項目_14.AutoSize = False
		Me._lb_項目_14.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_項目_14.Name = "_lb_項目_14"
		Me._lb_項目_13.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_項目_13.BackColor = System.Drawing.Color.Transparent
		Me._lb_項目_13.Text = "年"
		Me._lb_項目_13.Size = New System.Drawing.Size(17, 12)
		Me._lb_項目_13.Location = New System.Drawing.Point(933, 572)
		Me._lb_項目_13.TabIndex = 249
		Me._lb_項目_13.Enabled = True
		Me._lb_項目_13.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_項目_13.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_13.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_13.UseMnemonic = True
		Me._lb_項目_13.Visible = True
		Me._lb_項目_13.AutoSize = False
		Me._lb_項目_13.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_項目_13.Name = "_lb_項目_13"
		Me._lblLabels_52.Text = "備　考"
		Me._lblLabels_52.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_52.Size = New System.Drawing.Size(81, 13)
		Me._lblLabels_52.Location = New System.Drawing.Point(812, 590)
		Me._lblLabels_52.TabIndex = 248
		Me._lblLabels_52.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_52.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_52.Enabled = True
		Me._lblLabels_52.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_52.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_52.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_52.UseMnemonic = True
		Me._lblLabels_52.Visible = True
		Me._lblLabels_52.AutoSize = False
		Me._lblLabels_52.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_52.Name = "_lblLabels_52"
		Me._Shape1_12.BorderColor = System.Drawing.SystemColors.ControlDark
		Me._Shape1_12.Size = New System.Drawing.Size(421, 125)
		Me._Shape1_12.Location = New System.Drawing.Point(808, 504)
		Me._Shape1_12.BackColor = System.Drawing.SystemColors.Window
		Me._Shape1_12.BackStyle = Microsoft.VisualBasic.PowerPacks.BackStyle.Transparent
		Me._Shape1_12.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid
		Me._Shape1_12.BorderWidth = 1
		Me._Shape1_12.FillColor = System.Drawing.Color.Black
		Me._Shape1_12.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Transparent
		Me._Shape1_12.Visible = True
		Me._Shape1_12.Name = "_Shape1_12"
		Me._lb_項目_12.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me._lb_項目_12.Text = "経過情報"
		Me._lb_項目_12.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_12.ForeColor = System.Drawing.Color.White
		Me._lb_項目_12.Size = New System.Drawing.Size(101, 125)
		Me._lb_項目_12.Location = New System.Drawing.Point(704, 504)
		Me._lb_項目_12.TabIndex = 247
		Me._lb_項目_12.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lb_項目_12.Enabled = True
		Me._lb_項目_12.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_12.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_12.UseMnemonic = True
		Me._lb_項目_12.Visible = True
		Me._lb_項目_12.AutoSize = False
		Me._lb_項目_12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me._lb_項目_12.Name = "_lb_項目_12"
		Me.Label13.Text = "発注書発行日"
		Me.Label13.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.Label13.Size = New System.Drawing.Size(81, 13)
		Me.Label13.Location = New System.Drawing.Point(812, 512)
		Me.Label13.TabIndex = 246
		Me.Label13.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label13.BackColor = System.Drawing.SystemColors.Control
		Me.Label13.Enabled = True
		Me.Label13.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label13.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label13.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label13.UseMnemonic = True
		Me.Label13.Visible = True
		Me.Label13.AutoSize = False
		Me.Label13.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label13.Name = "Label13"
		Me._lblLabels_51.Text = "0:未確定 1:確定"
		Me._lblLabels_51.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_51.Size = New System.Drawing.Size(169, 13)
		Me._lblLabels_51.Location = New System.Drawing.Point(928, 532)
		Me._lblLabels_51.TabIndex = 245
		Me._lblLabels_51.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_51.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_51.Enabled = True
		Me._lblLabels_51.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_51.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_51.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_51.UseMnemonic = True
		Me._lblLabels_51.Visible = True
		Me._lblLabels_51.AutoSize = False
		Me._lblLabels_51.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_51.Name = "_lblLabels_51"
		Me.Label12.Text = "見積確定"
		Me.Label12.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.Label12.Size = New System.Drawing.Size(77, 13)
		Me.Label12.Location = New System.Drawing.Point(812, 532)
		Me.Label12.TabIndex = 244
		Me.Label12.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label12.BackColor = System.Drawing.SystemColors.Control
		Me.Label12.Enabled = True
		Me.Label12.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label12.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label12.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label12.UseMnemonic = True
		Me.Label12.Visible = True
		Me.Label12.AutoSize = False
		Me.Label12.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label12.Name = "Label12"
		Me.Label9.Text = "完了日付"
		Me.Label9.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.Label9.Size = New System.Drawing.Size(77, 13)
		Me.Label9.Location = New System.Drawing.Point(812, 552)
		Me.Label9.TabIndex = 242
		Me.Label9.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label9.BackColor = System.Drawing.SystemColors.Control
		Me.Label9.Enabled = True
		Me.Label9.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label9.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label9.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label9.UseMnemonic = True
		Me.Label9.Visible = True
		Me.Label9.AutoSize = False
		Me.Label9.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label9.Name = "Label9"
		Me._lb_日_7.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_日_7.BackColor = System.Drawing.Color.Transparent
		Me._lb_日_7.Text = "日"
		Me._lb_日_7.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_日_7.Size = New System.Drawing.Size(12, 15)
		Me._lb_日_7.Location = New System.Drawing.Point(1008, 551)
		Me._lb_日_7.TabIndex = 241
		Me._lb_日_7.Enabled = True
		Me._lb_日_7.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_日_7.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_日_7.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_日_7.UseMnemonic = True
		Me._lb_日_7.Visible = True
		Me._lb_日_7.AutoSize = False
		Me._lb_日_7.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_日_7.Name = "_lb_日_7"
		Me._lb_月_7.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_月_7.BackColor = System.Drawing.Color.Transparent
		Me._lb_月_7.Text = "月"
		Me._lb_月_7.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_月_7.Size = New System.Drawing.Size(12, 15)
		Me._lb_月_7.Location = New System.Drawing.Point(970, 551)
		Me._lb_月_7.TabIndex = 240
		Me._lb_月_7.Enabled = True
		Me._lb_月_7.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_月_7.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_月_7.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_月_7.UseMnemonic = True
		Me._lb_月_7.Visible = True
		Me._lb_月_7.AutoSize = False
		Me._lb_月_7.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_月_7.Name = "_lb_月_7"
		Me._lb_年_7.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_年_7.BackColor = System.Drawing.Color.Transparent
		Me._lb_年_7.Text = "年"
		Me._lb_年_7.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_年_7.Size = New System.Drawing.Size(17, 15)
		Me._lb_年_7.Location = New System.Drawing.Point(933, 551)
		Me._lb_年_7.TabIndex = 239
		Me._lb_年_7.Enabled = True
		Me._lb_年_7.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_年_7.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_年_7.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_年_7.UseMnemonic = True
		Me._lb_年_7.Visible = True
		Me._lb_年_7.AutoSize = False
		Me._lb_年_7.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_年_7.Name = "_lb_年_7"
		Me.Label10.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.Label10.Text = "工事担当"
		Me.Label10.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.Label10.Size = New System.Drawing.Size(53, 13)
		Me.Label10.Location = New System.Drawing.Point(360, 120)
		Me.Label10.TabIndex = 238
		Me.Label10.BackColor = System.Drawing.SystemColors.Control
		Me.Label10.Enabled = True
		Me.Label10.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label10.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label10.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label10.UseMnemonic = True
		Me.Label10.Visible = True
		Me.Label10.AutoSize = False
		Me.Label10.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label10.Name = "Label10"
		Me.rf_工事担当名.Text = "ＷＷＷＷＷＷＷＷ"
		Me.rf_工事担当名.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.rf_工事担当名.Size = New System.Drawing.Size(130, 19)
		Me.rf_工事担当名.Location = New System.Drawing.Point(487, 116)
		Me.rf_工事担当名.TabIndex = 237
		Me.rf_工事担当名.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.rf_工事担当名.BackColor = System.Drawing.SystemColors.Control
		Me.rf_工事担当名.Enabled = True
		Me.rf_工事担当名.ForeColor = System.Drawing.SystemColors.ControlText
		Me.rf_工事担当名.Cursor = System.Windows.Forms.Cursors.Default
		Me.rf_工事担当名.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.rf_工事担当名.UseMnemonic = True
		Me.rf_工事担当名.Visible = True
		Me.rf_工事担当名.AutoSize = False
		Me.rf_工事担当名.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.rf_工事担当名.Name = "rf_工事担当名"
		Me._lblLabels_50.Text = "クレーム区分"
		Me._lblLabels_50.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_50.Size = New System.Drawing.Size(85, 13)
		Me._lblLabels_50.Location = New System.Drawing.Point(420, 565)
		Me._lblLabels_50.TabIndex = 236
		Me._lblLabels_50.Visible = False
		Me._lblLabels_50.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_50.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_50.Enabled = True
		Me._lblLabels_50.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_50.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_50.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_50.UseMnemonic = True
		Me._lblLabels_50.AutoSize = False
		Me._lblLabels_50.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_50.Name = "_lblLabels_50"
		Me._lblLabels_49.Text = "1:クレーム"
		Me._lblLabels_49.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_49.Size = New System.Drawing.Size(77, 13)
		Me._lblLabels_49.Location = New System.Drawing.Point(540, 565)
		Me._lblLabels_49.TabIndex = 235
		Me._lblLabels_49.Visible = False
		Me._lblLabels_49.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_49.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_49.Enabled = True
		Me._lblLabels_49.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_49.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_49.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_49.UseMnemonic = True
		Me._lblLabels_49.AutoSize = False
		Me._lblLabels_49.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_49.Name = "_lblLabels_49"
		Me._lblLabels_48.Text = "外国企業で輸出入でない（国内取引）場合、課税対象に変更"
		Me._lblLabels_48.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_48.Size = New System.Drawing.Size(387, 13)
		Me._lblLabels_48.Location = New System.Drawing.Point(844, 6)
		Me._lblLabels_48.TabIndex = 234
		Me._lblLabels_48.Visible = False
		Me._lblLabels_48.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_48.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_48.Enabled = True
		Me._lblLabels_48.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_48.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_48.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_48.UseMnemonic = True
		Me._lblLabels_48.AutoSize = False
		Me._lblLabels_48.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_48.Name = "_lblLabels_48"
		Me._lblLabels_47.Text = "税集計"
		Me._lblLabels_47.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_47.Size = New System.Drawing.Size(47, 13)
		Me._lblLabels_47.Location = New System.Drawing.Point(664, 6)
		Me._lblLabels_47.TabIndex = 233
		Me._lblLabels_47.Visible = False
		Me._lblLabels_47.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_47.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_47.Enabled = True
		Me._lblLabels_47.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_47.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_47.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_47.UseMnemonic = True
		Me._lblLabels_47.AutoSize = False
		Me._lblLabels_47.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_47.Name = "_lblLabels_47"
		Me._lblLabels_1.Text = "担当者"
		Me._lblLabels_1.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_1.Size = New System.Drawing.Size(47, 13)
		Me._lblLabels_1.Location = New System.Drawing.Point(128, 216)
		Me._lblLabels_1.TabIndex = 232
		Me._lblLabels_1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_1.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_1.Enabled = True
		Me._lblLabels_1.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_1.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_1.UseMnemonic = True
		Me._lblLabels_1.Visible = True
		Me._lblLabels_1.AutoSize = False
		Me._lblLabels_1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_1.Name = "_lblLabels_1"
		Me.rf_税集計区分名.Text = "伝票単位"
		Me.rf_税集計区分名.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.rf_税集計区分名.Size = New System.Drawing.Size(78, 19)
		Me.rf_税集計区分名.Location = New System.Drawing.Point(760, 3)
		Me.rf_税集計区分名.TabIndex = 17
		Me.rf_税集計区分名.Visible = False
		Me.rf_税集計区分名.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.rf_税集計区分名.BackColor = System.Drawing.SystemColors.Control
		Me.rf_税集計区分名.Enabled = True
		Me.rf_税集計区分名.ForeColor = System.Drawing.SystemColors.ControlText
		Me.rf_税集計区分名.Cursor = System.Windows.Forms.Cursors.Default
		Me.rf_税集計区分名.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.rf_税集計区分名.UseMnemonic = True
		Me.rf_税集計区分名.AutoSize = False
		Me.rf_税集計区分名.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.rf_税集計区分名.Name = "rf_税集計区分名"
		Me._lblLabels_46.Text = "0:なし 1:ﾃﾞｨﾊﾞﾛ 2:台湾 3:ｼｽﾃﾑ開発"
		Me._lblLabels_46.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_46.Size = New System.Drawing.Size(287, 13)
		Me._lblLabels_46.Location = New System.Drawing.Point(932, 72)
		Me._lblLabels_46.TabIndex = 231
		Me._lblLabels_46.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_46.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_46.Enabled = True
		Me._lblLabels_46.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_46.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_46.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_46.UseMnemonic = True
		Me._lblLabels_46.Visible = True
		Me._lblLabels_46.AutoSize = False
		Me._lblLabels_46.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_46.Name = "_lblLabels_46"
		Me._lblLabels_45.Text = "内容区分"
		Me._lblLabels_45.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_45.Size = New System.Drawing.Size(73, 13)
		Me._lblLabels_45.Location = New System.Drawing.Point(812, 72)
		Me._lblLabels_45.TabIndex = 230
		Me._lblLabels_45.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_45.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_45.Enabled = True
		Me._lblLabels_45.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_45.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_45.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_45.UseMnemonic = True
		Me._lblLabels_45.Visible = True
		Me._lblLabels_45.AutoSize = False
		Me._lblLabels_45.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_45.Name = "_lblLabels_45"
		Me._lblLabels_44.Text = "化粧品ﾒｰｶｰ区分"
		Me._lblLabels_44.Enabled = False
		Me._lblLabels_44.Size = New System.Drawing.Size(85, 13)
		Me._lblLabels_44.Location = New System.Drawing.Point(936, 28)
		Me._lblLabels_44.TabIndex = 229
		Me._lblLabels_44.Visible = False
		Me._lblLabels_44.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_44.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_44.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_44.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_44.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_44.UseMnemonic = True
		Me._lblLabels_44.AutoSize = False
		Me._lblLabels_44.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_44.Name = "_lblLabels_44"
		Me._lblLabels_43.Text = "1:化粧品ﾒｰｶｰ"
		Me._lblLabels_43.Enabled = False
		Me._lblLabels_43.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_43.Size = New System.Drawing.Size(165, 13)
		Me._lblLabels_43.Location = New System.Drawing.Point(1060, 28)
		Me._lblLabels_43.TabIndex = 228
		Me._lblLabels_43.Visible = False
		Me._lblLabels_43.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_43.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_43.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_43.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_43.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_43.UseMnemonic = True
		Me._lblLabels_43.AutoSize = False
		Me._lblLabels_43.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_43.Name = "_lblLabels_43"
		Me._Shape1_11.BorderColor = System.Drawing.SystemColors.ControlDark
		Me._Shape1_11.Size = New System.Drawing.Size(421, 49)
		Me._Shape1_11.Location = New System.Drawing.Point(808, 44)
		Me._Shape1_11.BackColor = System.Drawing.SystemColors.Window
		Me._Shape1_11.BackStyle = Microsoft.VisualBasic.PowerPacks.BackStyle.Transparent
		Me._Shape1_11.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid
		Me._Shape1_11.BorderWidth = 1
		Me._Shape1_11.FillColor = System.Drawing.Color.Black
		Me._Shape1_11.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Transparent
		Me._Shape1_11.Visible = True
		Me._Shape1_11.Name = "_Shape1_11"
		Me._lb_項目_11.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me._lb_項目_11.Text = "しまむら情報"
		Me._lb_項目_11.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_11.ForeColor = System.Drawing.Color.White
		Me._lb_項目_11.Size = New System.Drawing.Size(101, 49)
		Me._lb_項目_11.Location = New System.Drawing.Point(704, 44)
		Me._lb_項目_11.TabIndex = 226
		Me._lb_項目_11.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lb_項目_11.Enabled = True
		Me._lb_項目_11.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_11.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_11.UseMnemonic = True
		Me._lb_項目_11.Visible = True
		Me._lb_項目_11.AutoSize = False
		Me._lb_項目_11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me._lb_項目_11.Name = "_lb_項目_11"
		Me._Shape1_10.BorderColor = System.Drawing.SystemColors.ControlDark
		Me._Shape1_10.Size = New System.Drawing.Size(421, 69)
		Me._Shape1_10.Location = New System.Drawing.Point(808, 188)
		Me._Shape1_10.BackColor = System.Drawing.SystemColors.Window
		Me._Shape1_10.BackStyle = Microsoft.VisualBasic.PowerPacks.BackStyle.Transparent
		Me._Shape1_10.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid
		Me._Shape1_10.BorderWidth = 1
		Me._Shape1_10.FillColor = System.Drawing.Color.Black
		Me._Shape1_10.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Transparent
		Me._Shape1_10.Visible = True
		Me._Shape1_10.Name = "_Shape1_10"
		Me._lblLabels_42.Text = "請求区分"
		Me._lblLabels_42.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_42.Size = New System.Drawing.Size(73, 13)
		Me._lblLabels_42.Location = New System.Drawing.Point(812, 236)
		Me._lblLabels_42.TabIndex = 225
		Me._lblLabels_42.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_42.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_42.Enabled = True
		Me._lblLabels_42.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_42.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_42.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_42.UseMnemonic = True
		Me._lblLabels_42.Visible = True
		Me._lblLabels_42.AutoSize = False
		Me._lblLabels_42.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_42.Name = "_lblLabels_42"
		Me._lblLabels_41.Text = "1:新財務 2:リース 3:ﾁｪｰﾝｽﾄｱ伝票"
		Me._lblLabels_41.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_41.Size = New System.Drawing.Size(237, 13)
		Me._lblLabels_41.Location = New System.Drawing.Point(932, 236)
		Me._lblLabels_41.TabIndex = 224
		Me._lblLabels_41.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_41.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_41.Enabled = True
		Me._lblLabels_41.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_41.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_41.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_41.UseMnemonic = True
		Me._lblLabels_41.Visible = True
		Me._lblLabels_41.AutoSize = False
		Me._lblLabels_41.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_41.Name = "_lblLabels_41"
		Me._lblLabels_40.Text = "物件区分"
		Me._lblLabels_40.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_40.Size = New System.Drawing.Size(73, 13)
		Me._lblLabels_40.Location = New System.Drawing.Point(812, 216)
		Me._lblLabels_40.TabIndex = 223
		Me._lblLabels_40.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_40.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_40.Enabled = True
		Me._lblLabels_40.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_40.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_40.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_40.UseMnemonic = True
		Me._lblLabels_40.Visible = True
		Me._lblLabels_40.AutoSize = False
		Me._lblLabels_40.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_40.Name = "_lblLabels_40"
		Me._lblLabels_39.Text = "1:物件 2:メンテ・備品 3:担当者案件"
		Me._lblLabels_39.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_39.Size = New System.Drawing.Size(267, 13)
		Me._lblLabels_39.Location = New System.Drawing.Point(932, 216)
		Me._lblLabels_39.TabIndex = 222
		Me._lblLabels_39.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_39.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_39.Enabled = True
		Me._lblLabels_39.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_39.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_39.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_39.UseMnemonic = True
		Me._lblLabels_39.Visible = True
		Me._lblLabels_39.AutoSize = False
		Me._lblLabels_39.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_39.Name = "_lblLabels_39"
		Me._lblLabels_36.Text = "請求管轄"
		Me._lblLabels_36.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_36.Size = New System.Drawing.Size(77, 13)
		Me._lblLabels_36.Location = New System.Drawing.Point(812, 196)
		Me._lblLabels_36.TabIndex = 221
		Me._lblLabels_36.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_36.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_36.Enabled = True
		Me._lblLabels_36.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_36.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_36.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_36.UseMnemonic = True
		Me._lblLabels_36.Visible = True
		Me._lblLabels_36.AutoSize = False
		Me._lblLabels_36.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_36.Name = "_lblLabels_36"
		Me._lblLabels_35.Text = "1:サプライ 2:情報ｼｽﾃﾑ"
		Me._lblLabels_35.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_35.Size = New System.Drawing.Size(165, 13)
		Me._lblLabels_35.Location = New System.Drawing.Point(932, 196)
		Me._lblLabels_35.TabIndex = 220
		Me._lblLabels_35.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_35.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_35.Enabled = True
		Me._lblLabels_35.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_35.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_35.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_35.UseMnemonic = True
		Me._lblLabels_35.Visible = True
		Me._lblLabels_35.AutoSize = False
		Me._lblLabels_35.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_35.Name = "_lblLabels_35"
		Me._lblLabels_38.Text = "作業内容"
		Me._lblLabels_38.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_38.Size = New System.Drawing.Size(81, 13)
		Me._lblLabels_38.Location = New System.Drawing.Point(112, 616)
		Me._lblLabels_38.TabIndex = 219
		Me._lblLabels_38.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_38.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_38.Enabled = True
		Me._lblLabels_38.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_38.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_38.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_38.UseMnemonic = True
		Me._lblLabels_38.Visible = True
		Me._lblLabels_38.AutoSize = False
		Me._lblLabels_38.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_38.Name = "_lblLabels_38"
		Me._lblLabels_37.Text = "発注担当者"
		Me._lblLabels_37.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_37.Size = New System.Drawing.Size(81, 13)
		Me._lblLabels_37.Location = New System.Drawing.Point(112, 596)
		Me._lblLabels_37.TabIndex = 218
		Me._lblLabels_37.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_37.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_37.Enabled = True
		Me._lblLabels_37.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_37.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_37.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_37.UseMnemonic = True
		Me._lblLabels_37.Visible = True
		Me._lblLabels_37.AutoSize = False
		Me._lblLabels_37.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_37.Name = "_lblLabels_37"
		Me.Label7.Text = "完工日付"
		Me.Label7.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.Label7.Size = New System.Drawing.Size(57, 13)
		Me.Label7.Location = New System.Drawing.Point(112, 576)
		Me.Label7.TabIndex = 216
		Me.Label7.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label7.BackColor = System.Drawing.SystemColors.Control
		Me.Label7.Enabled = True
		Me.Label7.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label7.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label7.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label7.UseMnemonic = True
		Me.Label7.Visible = True
		Me.Label7.AutoSize = False
		Me.Label7.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label7.Name = "Label7"
		Me._lb_日_6.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_日_6.BackColor = System.Drawing.Color.Transparent
		Me._lb_日_6.Text = "日"
		Me._lb_日_6.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_日_6.Size = New System.Drawing.Size(12, 15)
		Me._lb_日_6.Location = New System.Drawing.Point(312, 575)
		Me._lb_日_6.TabIndex = 215
		Me._lb_日_6.Enabled = True
		Me._lb_日_6.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_日_6.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_日_6.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_日_6.UseMnemonic = True
		Me._lb_日_6.Visible = True
		Me._lb_日_6.AutoSize = False
		Me._lb_日_6.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_日_6.Name = "_lb_日_6"
		Me._lb_月_6.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_月_6.BackColor = System.Drawing.Color.Transparent
		Me._lb_月_6.Text = "月"
		Me._lb_月_6.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_月_6.Size = New System.Drawing.Size(12, 15)
		Me._lb_月_6.Location = New System.Drawing.Point(274, 575)
		Me._lb_月_6.TabIndex = 214
		Me._lb_月_6.Enabled = True
		Me._lb_月_6.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_月_6.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_月_6.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_月_6.UseMnemonic = True
		Me._lb_月_6.Visible = True
		Me._lb_月_6.AutoSize = False
		Me._lb_月_6.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_月_6.Name = "_lb_月_6"
		Me._lb_年_6.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_年_6.BackColor = System.Drawing.Color.Transparent
		Me._lb_年_6.Text = "年"
		Me._lb_年_6.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_年_6.Size = New System.Drawing.Size(17, 15)
		Me._lb_年_6.Location = New System.Drawing.Point(237, 575)
		Me._lb_年_6.TabIndex = 213
		Me._lb_年_6.Enabled = True
		Me._lb_年_6.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_年_6.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_年_6.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_年_6.UseMnemonic = True
		Me._lb_年_6.Visible = True
		Me._lb_年_6.AutoSize = False
		Me._lb_年_6.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_年_6.Name = "_lb_年_6"
		Me.Label5.Text = "受付日付"
		Me.Label5.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.Label5.Size = New System.Drawing.Size(57, 13)
		Me.Label5.Location = New System.Drawing.Point(112, 556)
		Me.Label5.TabIndex = 211
		Me.Label5.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label5.BackColor = System.Drawing.SystemColors.Control
		Me.Label5.Enabled = True
		Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label5.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label5.UseMnemonic = True
		Me.Label5.Visible = True
		Me.Label5.AutoSize = False
		Me.Label5.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label5.Name = "Label5"
		Me._lb_日_5.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_日_5.BackColor = System.Drawing.Color.Transparent
		Me._lb_日_5.Text = "日"
		Me._lb_日_5.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_日_5.Size = New System.Drawing.Size(12, 15)
		Me._lb_日_5.Location = New System.Drawing.Point(312, 555)
		Me._lb_日_5.TabIndex = 210
		Me._lb_日_5.Enabled = True
		Me._lb_日_5.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_日_5.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_日_5.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_日_5.UseMnemonic = True
		Me._lb_日_5.Visible = True
		Me._lb_日_5.AutoSize = False
		Me._lb_日_5.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_日_5.Name = "_lb_日_5"
		Me._lb_月_5.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_月_5.BackColor = System.Drawing.Color.Transparent
		Me._lb_月_5.Text = "月"
		Me._lb_月_5.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_月_5.Size = New System.Drawing.Size(12, 15)
		Me._lb_月_5.Location = New System.Drawing.Point(274, 555)
		Me._lb_月_5.TabIndex = 209
		Me._lb_月_5.Enabled = True
		Me._lb_月_5.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_月_5.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_月_5.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_月_5.UseMnemonic = True
		Me._lb_月_5.Visible = True
		Me._lb_月_5.AutoSize = False
		Me._lb_月_5.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_月_5.Name = "_lb_月_5"
		Me._lb_年_5.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_年_5.BackColor = System.Drawing.Color.Transparent
		Me._lb_年_5.Text = "年"
		Me._lb_年_5.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_年_5.Size = New System.Drawing.Size(17, 15)
		Me._lb_年_5.Location = New System.Drawing.Point(237, 555)
		Me._lb_年_5.TabIndex = 208
		Me._lb_年_5.Enabled = True
		Me._lb_年_5.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_年_5.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_年_5.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_年_5.UseMnemonic = True
		Me._lb_年_5.Visible = True
		Me._lb_年_5.AutoSize = False
		Me._lb_年_5.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_年_5.Name = "_lb_年_5"
		Me._lb_項目_10.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me._lb_項目_10.Text = "ﾔｵｺｰ情報"
		Me._lb_項目_10.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_10.ForeColor = System.Drawing.Color.White
		Me._lb_項目_10.Size = New System.Drawing.Size(101, 69)
		Me._lb_項目_10.Location = New System.Drawing.Point(704, 188)
		Me._lb_項目_10.TabIndex = 207
		Me._lb_項目_10.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lb_項目_10.Enabled = True
		Me._lb_項目_10.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_10.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_10.UseMnemonic = True
		Me._lb_項目_10.Visible = True
		Me._lb_項目_10.AutoSize = False
		Me._lb_項目_10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me._lb_項目_10.Name = "_lb_項目_10"
		Me.rf_物件略称.Text = "ＷＷＷＷＷＷＷＷ"
		Me.rf_物件略称.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.rf_物件略称.Size = New System.Drawing.Size(178, 19)
		Me.rf_物件略称.Location = New System.Drawing.Point(245, 48)
		Me.rf_物件略称.TabIndex = 206
		Me.rf_物件略称.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.rf_物件略称.BackColor = System.Drawing.SystemColors.Control
		Me.rf_物件略称.Enabled = True
		Me.rf_物件略称.ForeColor = System.Drawing.SystemColors.ControlText
		Me.rf_物件略称.Cursor = System.Windows.Forms.Cursors.Default
		Me.rf_物件略称.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.rf_物件略称.UseMnemonic = True
		Me.rf_物件略称.Visible = True
		Me.rf_物件略称.AutoSize = False
		Me.rf_物件略称.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.rf_物件略称.Name = "rf_物件略称"
		Me._lb_項目_9.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me._lb_項目_9.Text = "物件情報"
		Me._lb_項目_9.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_9.ForeColor = System.Drawing.Color.White
		Me._lb_項目_9.Size = New System.Drawing.Size(101, 45)
		Me._lb_項目_9.Location = New System.Drawing.Point(4, 44)
		Me._lb_項目_9.TabIndex = 205
		Me._lb_項目_9.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lb_項目_9.Enabled = True
		Me._lb_項目_9.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_9.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_9.UseMnemonic = True
		Me._lb_項目_9.Visible = True
		Me._lb_項目_9.AutoSize = False
		Me._lb_項目_9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me._lb_項目_9.Name = "_lb_項目_9"
		Me._Shape1_9.BorderColor = System.Drawing.SystemColors.ControlDark
		Me._Shape1_9.Size = New System.Drawing.Size(593, 45)
		Me._Shape1_9.Location = New System.Drawing.Point(108, 44)
		Me._Shape1_9.BackColor = System.Drawing.SystemColors.Window
		Me._Shape1_9.BackStyle = Microsoft.VisualBasic.PowerPacks.BackStyle.Transparent
		Me._Shape1_9.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid
		Me._Shape1_9.BorderWidth = 1
		Me._Shape1_9.FillColor = System.Drawing.Color.Black
		Me._Shape1_9.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Transparent
		Me._Shape1_9.Visible = True
		Me._Shape1_9.Name = "_Shape1_9"
		Me._lblLabels_31.Text = "売場面積"
		Me._lblLabels_31.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_31.Size = New System.Drawing.Size(85, 13)
		Me._lblLabels_31.Location = New System.Drawing.Point(812, 164)
		Me._lblLabels_31.TabIndex = 200
		Me._lblLabels_31.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_31.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_31.Enabled = True
		Me._lblLabels_31.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_31.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_31.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_31.UseMnemonic = True
		Me._lblLabels_31.Visible = True
		Me._lblLabels_31.AutoSize = False
		Me._lblLabels_31.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_31.Name = "_lblLabels_31"
		Me._lblLabels_30.Text = "坪"
		Me._lblLabels_30.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_30.Size = New System.Drawing.Size(33, 13)
		Me._lblLabels_30.Location = New System.Drawing.Point(1008, 164)
		Me._lblLabels_30.TabIndex = 199
		Me._lblLabels_30.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_30.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_30.Enabled = True
		Me._lblLabels_30.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_30.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_30.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_30.UseMnemonic = True
		Me._lblLabels_30.Visible = True
		Me._lblLabels_30.AutoSize = False
		Me._lblLabels_30.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_30.Name = "_lblLabels_30"
		Me._lblLabels_29.Text = "物件内容"
		Me._lblLabels_29.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_29.Size = New System.Drawing.Size(77, 13)
		Me._lblLabels_29.Location = New System.Drawing.Point(812, 144)
		Me._lblLabels_29.TabIndex = 198
		Me._lblLabels_29.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_29.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_29.Enabled = True
		Me._lblLabels_29.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_29.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_29.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_29.UseMnemonic = True
		Me._lblLabels_29.Visible = True
		Me._lblLabels_29.AutoSize = False
		Me._lblLabels_29.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_29.Name = "_lblLabels_29"
		Me._lb_項目_7.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me._lb_項目_7.Text = "ｳｴﾙｼｱ情報"
		Me._lb_項目_7.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_7.ForeColor = System.Drawing.Color.White
		Me._lb_項目_7.Size = New System.Drawing.Size(101, 89)
		Me._lb_項目_7.Location = New System.Drawing.Point(704, 96)
		Me._lb_項目_7.TabIndex = 197
		Me._lb_項目_7.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lb_項目_7.Enabled = True
		Me._lb_項目_7.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_7.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_7.UseMnemonic = True
		Me._lb_項目_7.Visible = True
		Me._lb_項目_7.AutoSize = False
		Me._lb_項目_7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me._lb_項目_7.Name = "_lb_項目_7"
		Me._Shape1_7.BorderColor = System.Drawing.SystemColors.ControlDark
		Me._Shape1_7.Size = New System.Drawing.Size(421, 89)
		Me._Shape1_7.Location = New System.Drawing.Point(808, 96)
		Me._Shape1_7.BackColor = System.Drawing.SystemColors.Window
		Me._Shape1_7.BackStyle = Microsoft.VisualBasic.PowerPacks.BackStyle.Transparent
		Me._Shape1_7.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid
		Me._Shape1_7.BorderWidth = 1
		Me._Shape1_7.FillColor = System.Drawing.Color.Black
		Me._Shape1_7.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Transparent
		Me._Shape1_7.Visible = True
		Me._Shape1_7.Name = "_Shape1_7"
		Me._lblLabels_28.Text = "伝票種類"
		Me._lblLabels_28.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_28.Size = New System.Drawing.Size(73, 13)
		Me._lblLabels_28.Location = New System.Drawing.Point(812, 52)
		Me._lblLabels_28.TabIndex = 196
		Me._lblLabels_28.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_28.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_28.Enabled = True
		Me._lblLabels_28.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_28.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_28.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_28.UseMnemonic = True
		Me._lblLabels_28.Visible = True
		Me._lblLabels_28.AutoSize = False
		Me._lblLabels_28.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_28.Name = "_lblLabels_28"
		Me._lblLabels_27.Text = "0:なし 1:消耗 2:直納 3:mail消耗 4:mail直納"
		Me._lblLabels_27.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_27.Size = New System.Drawing.Size(296, 13)
		Me._lblLabels_27.Location = New System.Drawing.Point(932, 52)
		Me._lblLabels_27.TabIndex = 195
		Me._lblLabels_27.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_27.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_27.Enabled = True
		Me._lblLabels_27.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_27.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_27.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_27.UseMnemonic = True
		Me._lblLabels_27.Visible = True
		Me._lblLabels_27.AutoSize = False
		Me._lblLabels_27.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_27.Name = "_lblLabels_27"
		Me.Label4.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.Label4.Text = "物件№"
		Me.Label4.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.Label4.Size = New System.Drawing.Size(61, 13)
		Me.Label4.Location = New System.Drawing.Point(112, 52)
		Me.Label4.TabIndex = 194
		Me.Label4.BackColor = System.Drawing.SystemColors.Control
		Me.Label4.Enabled = True
		Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label4.UseMnemonic = True
		Me.Label4.Visible = True
		Me.Label4.AutoSize = False
		Me.Label4.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label4.Name = "Label4"
		Me.rf_部署名.Text = "ＷＷＷＷＷＷＷＷ"
		Me.rf_部署名.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.rf_部署名.Size = New System.Drawing.Size(130, 19)
		Me.rf_部署名.Location = New System.Drawing.Point(487, 96)
		Me.rf_部署名.TabIndex = 193
		Me.rf_部署名.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.rf_部署名.BackColor = System.Drawing.SystemColors.Control
		Me.rf_部署名.Enabled = True
		Me.rf_部署名.ForeColor = System.Drawing.SystemColors.ControlText
		Me.rf_部署名.Cursor = System.Windows.Forms.Cursors.Default
		Me.rf_部署名.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.rf_部署名.UseMnemonic = True
		Me.rf_部署名.Visible = True
		Me.rf_部署名.AutoSize = False
		Me.rf_部署名.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.rf_部署名.Name = "rf_部署名"
		Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.Label3.Text = "部署"
		Me.Label3.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.Label3.Size = New System.Drawing.Size(37, 13)
		Me.Label3.Location = New System.Drawing.Point(376, 100)
		Me.Label3.TabIndex = 192
		Me.Label3.BackColor = System.Drawing.SystemColors.Control
		Me.Label3.Enabled = True
		Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label3.UseMnemonic = True
		Me.Label3.Visible = True
		Me.Label3.AutoSize = False
		Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label3.Name = "Label3"
		Me._lb_項目_8.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me._lb_項目_8.Text = "販売先" & Chr(13) & Chr(10) & "(社内在庫用)"
		Me._lb_項目_8.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_8.ForeColor = System.Drawing.Color.White
		Me._lb_項目_8.Size = New System.Drawing.Size(101, 69)
		Me._lb_項目_8.Location = New System.Drawing.Point(4, 352)
		Me._lb_項目_8.TabIndex = 191
		Me._lb_項目_8.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lb_項目_8.Enabled = True
		Me._lb_項目_8.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_8.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_8.UseMnemonic = True
		Me._lb_項目_8.Visible = True
		Me._lb_項目_8.AutoSize = False
		Me._lb_項目_8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me._lb_項目_8.Name = "_lb_項目_8"
		Me._Shape1_8.BorderColor = System.Drawing.SystemColors.ControlDark
		Me._Shape1_8.Size = New System.Drawing.Size(593, 69)
		Me._Shape1_8.Location = New System.Drawing.Point(108, 352)
		Me._Shape1_8.BackColor = System.Drawing.SystemColors.Window
		Me._Shape1_8.BackStyle = Microsoft.VisualBasic.PowerPacks.BackStyle.Transparent
		Me._Shape1_8.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid
		Me._Shape1_8.BorderWidth = 1
		Me._Shape1_8.FillColor = System.Drawing.Color.Black
		Me._Shape1_8.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Transparent
		Me._Shape1_8.Visible = True
		Me._Shape1_8.Name = "_Shape1_8"
		Me.rf_得意先別見積番号.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.rf_得意先別見積番号.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.rf_得意先別見積番号.Size = New System.Drawing.Size(101, 13)
		Me.rf_得意先別見積番号.Location = New System.Drawing.Point(276, 24)
		Me.rf_得意先別見積番号.TabIndex = 186
		Me.rf_得意先別見積番号.Visible = False
		Me.rf_得意先別見積番号.BackColor = System.Drawing.SystemColors.Control
		Me.rf_得意先別見積番号.Enabled = True
		Me.rf_得意先別見積番号.ForeColor = System.Drawing.SystemColors.ControlText
		Me.rf_得意先別見積番号.Cursor = System.Windows.Forms.Cursors.Default
		Me.rf_得意先別見積番号.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.rf_得意先別見積番号.UseMnemonic = True
		Me.rf_得意先別見積番号.AutoSize = False
		Me.rf_得意先別見積番号.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.rf_得意先別見積番号.Name = "rf_得意先別見積番号"
		Me.rf_外税額.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.rf_外税額.BackColor = System.Drawing.Color.Transparent
		Me.rf_外税額.Text = "100000"
		Me.rf_外税額.ForeColor = System.Drawing.SystemColors.ActiveCaption
		Me.rf_外税額.Size = New System.Drawing.Size(73, 12)
		Me.rf_外税額.Location = New System.Drawing.Point(672, 24)
		Me.rf_外税額.TabIndex = 185
		Me.rf_外税額.Visible = False
		Me.rf_外税額.Enabled = True
		Me.rf_外税額.Cursor = System.Windows.Forms.Cursors.Default
		Me.rf_外税額.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.rf_外税額.UseMnemonic = True
		Me.rf_外税額.AutoSize = False
		Me.rf_外税額.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.rf_外税額.Name = "rf_外税額"
		Me.rf_原価率.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.rf_原価率.BackColor = System.Drawing.Color.Transparent
		Me.rf_原価率.Text = "99.99"
		Me.rf_原価率.ForeColor = System.Drawing.SystemColors.ActiveCaption
		Me.rf_原価率.Size = New System.Drawing.Size(33, 12)
		Me.rf_原価率.Location = New System.Drawing.Point(632, 24)
		Me.rf_原価率.TabIndex = 184
		Me.rf_原価率.Visible = False
		Me.rf_原価率.Enabled = True
		Me.rf_原価率.Cursor = System.Windows.Forms.Cursors.Default
		Me.rf_原価率.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.rf_原価率.UseMnemonic = True
		Me.rf_原価率.AutoSize = False
		Me.rf_原価率.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.rf_原価率.Name = "rf_原価率"
		Me.rf_原価合計.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.rf_原価合計.BackColor = System.Drawing.Color.Transparent
		Me.rf_原価合計.Text = "100000"
		Me.rf_原価合計.ForeColor = System.Drawing.SystemColors.ActiveCaption
		Me.rf_原価合計.Size = New System.Drawing.Size(73, 12)
		Me.rf_原価合計.Location = New System.Drawing.Point(544, 24)
		Me.rf_原価合計.TabIndex = 183
		Me.rf_原価合計.Visible = False
		Me.rf_原価合計.Enabled = True
		Me.rf_原価合計.Cursor = System.Windows.Forms.Cursors.Default
		Me.rf_原価合計.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.rf_原価合計.UseMnemonic = True
		Me.rf_原価合計.AutoSize = False
		Me.rf_原価合計.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.rf_原価合計.Name = "rf_原価合計"
		Me.rf_合計金額.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.rf_合計金額.BackColor = System.Drawing.Color.Transparent
		Me.rf_合計金額.Text = "100000"
		Me.rf_合計金額.ForeColor = System.Drawing.SystemColors.ActiveCaption
		Me.rf_合計金額.Size = New System.Drawing.Size(73, 12)
		Me.rf_合計金額.Location = New System.Drawing.Point(460, 24)
		Me.rf_合計金額.TabIndex = 182
		Me.rf_合計金額.Visible = False
		Me.rf_合計金額.Enabled = True
		Me.rf_合計金額.Cursor = System.Windows.Forms.Cursors.Default
		Me.rf_合計金額.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.rf_合計金額.UseMnemonic = True
		Me.rf_合計金額.AutoSize = False
		Me.rf_合計金額.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.rf_合計金額.Name = "rf_合計金額"
		Me.rf_売上端数.BackColor = System.Drawing.Color.Transparent
		Me.rf_売上端数.Text = "1"
		Me.rf_売上端数.ForeColor = System.Drawing.SystemColors.ActiveCaption
		Me.rf_売上端数.Size = New System.Drawing.Size(11, 12)
		Me.rf_売上端数.Location = New System.Drawing.Point(628, 196)
		Me.rf_売上端数.TabIndex = 181
		Me.rf_売上端数.Visible = False
		Me.rf_売上端数.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.rf_売上端数.Enabled = True
		Me.rf_売上端数.Cursor = System.Windows.Forms.Cursors.Default
		Me.rf_売上端数.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.rf_売上端数.UseMnemonic = True
		Me.rf_売上端数.AutoSize = False
		Me.rf_売上端数.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.rf_売上端数.Name = "rf_売上端数"
		Me.rf_消費税端数.BackColor = System.Drawing.Color.Transparent
		Me.rf_消費税端数.Text = "1"
		Me.rf_消費税端数.ForeColor = System.Drawing.SystemColors.ActiveCaption
		Me.rf_消費税端数.Size = New System.Drawing.Size(9, 12)
		Me.rf_消費税端数.Location = New System.Drawing.Point(656, 196)
		Me.rf_消費税端数.TabIndex = 180
		Me.rf_消費税端数.Visible = False
		Me.rf_消費税端数.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.rf_消費税端数.Enabled = True
		Me.rf_消費税端数.Cursor = System.Windows.Forms.Cursors.Default
		Me.rf_消費税端数.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.rf_消費税端数.UseMnemonic = True
		Me.rf_消費税端数.AutoSize = False
		Me.rf_消費税端数.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.rf_消費税端数.Name = "rf_消費税端数"
		Me.rf_税集計区分.BackColor = System.Drawing.Color.Transparent
		Me.rf_税集計区分.Text = "0"
		Me.rf_税集計区分.ForeColor = System.Drawing.SystemColors.ActiveCaption
		Me.rf_税集計区分.Size = New System.Drawing.Size(5, 12)
		Me.rf_税集計区分.Location = New System.Drawing.Point(560, 196)
		Me.rf_税集計区分.TabIndex = 179
		Me.rf_税集計区分.Visible = False
		Me.rf_税集計区分.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.rf_税集計区分.Enabled = True
		Me.rf_税集計区分.Cursor = System.Windows.Forms.Cursors.Default
		Me.rf_税集計区分.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.rf_税集計区分.UseMnemonic = True
		Me.rf_税集計区分.AutoSize = False
		Me.rf_税集計区分.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.rf_税集計区分.Name = "rf_税集計区分"
		Me._lblLabels_25.Text = "円"
		Me._lblLabels_25.Enabled = False
		Me._lblLabels_25.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_25.Size = New System.Drawing.Size(21, 13)
		Me._lblLabels_25.Location = New System.Drawing.Point(1004, 480)
		Me._lblLabels_25.TabIndex = 178
		Me._lblLabels_25.Visible = False
		Me._lblLabels_25.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_25.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_25.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_25.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_25.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_25.UseMnemonic = True
		Me._lblLabels_25.AutoSize = False
		Me._lblLabels_25.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_25.Name = "_lblLabels_25"
		Me._lb_年_4.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_年_4.BackColor = System.Drawing.Color.Transparent
		Me._lb_年_4.Text = "年"
		Me._lb_年_4.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_年_4.Size = New System.Drawing.Size(17, 15)
		Me._lb_年_4.Location = New System.Drawing.Point(933, 439)
		Me._lb_年_4.TabIndex = 176
		Me._lb_年_4.Enabled = True
		Me._lb_年_4.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_年_4.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_年_4.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_年_4.UseMnemonic = True
		Me._lb_年_4.Visible = True
		Me._lb_年_4.AutoSize = False
		Me._lb_年_4.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_年_4.Name = "_lb_年_4"
		Me._lb_月_4.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_月_4.BackColor = System.Drawing.Color.Transparent
		Me._lb_月_4.Text = "月"
		Me._lb_月_4.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_月_4.Size = New System.Drawing.Size(12, 15)
		Me._lb_月_4.Location = New System.Drawing.Point(970, 439)
		Me._lb_月_4.TabIndex = 175
		Me._lb_月_4.Enabled = True
		Me._lb_月_4.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_月_4.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_月_4.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_月_4.UseMnemonic = True
		Me._lb_月_4.Visible = True
		Me._lb_月_4.AutoSize = False
		Me._lb_月_4.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_月_4.Name = "_lb_月_4"
		Me._lb_日_4.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_日_4.BackColor = System.Drawing.Color.Transparent
		Me._lb_日_4.Text = "日"
		Me._lb_日_4.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_日_4.Size = New System.Drawing.Size(12, 15)
		Me._lb_日_4.Location = New System.Drawing.Point(1008, 439)
		Me._lb_日_4.TabIndex = 174
		Me._lb_日_4.Enabled = True
		Me._lb_日_4.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_日_4.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_日_4.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_日_4.UseMnemonic = True
		Me._lb_日_4.Visible = True
		Me._lb_日_4.AutoSize = False
		Me._lb_日_4.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_日_4.Name = "_lb_日_4"
		Me.Label2.Text = "受注日付"
		Me.Label2.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.Label2.Size = New System.Drawing.Size(57, 13)
		Me.Label2.Location = New System.Drawing.Point(812, 440)
		Me.Label2.TabIndex = 173
		Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label2.BackColor = System.Drawing.SystemColors.Control
		Me.Label2.Enabled = True
		Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label2.UseMnemonic = True
		Me.Label2.Visible = True
		Me.Label2.AutoSize = False
		Me.Label2.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label2.Name = "Label2"
		Me._lblLabels_15.Text = "御支払条件"
		Me._lblLabels_15.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_15.Size = New System.Drawing.Size(81, 13)
		Me._lblLabels_15.Location = New System.Drawing.Point(812, 320)
		Me._lblLabels_15.TabIndex = 172
		Me._lblLabels_15.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_15.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_15.Enabled = True
		Me._lblLabels_15.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_15.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_15.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_15.UseMnemonic = True
		Me._lblLabels_15.Visible = True
		Me._lblLabels_15.AutoSize = False
		Me._lblLabels_15.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_15.Name = "_lblLabels_15"
		Me._lblLabels_9.Text = "0:別途御打ち合せによる 1:その他"
		Me._lblLabels_9.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_9.Size = New System.Drawing.Size(221, 13)
		Me._lblLabels_9.Location = New System.Drawing.Point(928, 320)
		Me._lblLabels_9.TabIndex = 171
		Me._lblLabels_9.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_9.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_9.Enabled = True
		Me._lblLabels_9.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_9.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_9.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_9.UseMnemonic = True
		Me._lblLabels_9.Visible = True
		Me._lblLabels_9.AutoSize = False
		Me._lblLabels_9.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_9.Name = "_lblLabels_9"
		Me._Shape1_6.BorderColor = System.Drawing.SystemColors.ControlDark
		Me._Shape1_6.Size = New System.Drawing.Size(593, 69)
		Me._Shape1_6.Location = New System.Drawing.Point(108, 92)
		Me._Shape1_6.BackColor = System.Drawing.SystemColors.Window
		Me._Shape1_6.BackStyle = Microsoft.VisualBasic.PowerPacks.BackStyle.Transparent
		Me._Shape1_6.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid
		Me._Shape1_6.BorderWidth = 1
		Me._Shape1_6.FillColor = System.Drawing.Color.Black
		Me._Shape1_6.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Transparent
		Me._Shape1_6.Visible = True
		Me._Shape1_6.Name = "_Shape1_6"
		Me.Label1.Text = "見積日付"
		Me.Label1.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.Label1.Size = New System.Drawing.Size(61, 13)
		Me.Label1.Location = New System.Drawing.Point(116, 120)
		Me.Label1.TabIndex = 170
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label1.BackColor = System.Drawing.SystemColors.Control
		Me.Label1.Enabled = True
		Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label1.UseMnemonic = True
		Me.Label1.Visible = True
		Me.Label1.AutoSize = False
		Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label1.Name = "Label1"
		Me._lb_項目_5.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me._lb_項目_5.Text = "見積情報"
		Me._lb_項目_5.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_5.ForeColor = System.Drawing.Color.White
		Me._lb_項目_5.Size = New System.Drawing.Size(101, 69)
		Me._lb_項目_5.Location = New System.Drawing.Point(4, 92)
		Me._lb_項目_5.TabIndex = 169
		Me._lb_項目_5.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lb_項目_5.Enabled = True
		Me._lb_項目_5.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_5.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_5.UseMnemonic = True
		Me._lb_項目_5.Visible = True
		Me._lb_項目_5.AutoSize = False
		Me._lb_項目_5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me._lb_項目_5.Name = "_lb_項目_5"
		Me._lblLabels_26.Text = "現場名"
		Me._lblLabels_26.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_26.Size = New System.Drawing.Size(81, 13)
		Me._lblLabels_26.Location = New System.Drawing.Point(812, 300)
		Me._lblLabels_26.TabIndex = 168
		Me._lblLabels_26.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_26.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_26.Enabled = True
		Me._lblLabels_26.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_26.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_26.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_26.UseMnemonic = True
		Me._lblLabels_26.Visible = True
		Me._lblLabels_26.AutoSize = False
		Me._lblLabels_26.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_26.Name = "_lblLabels_26"
		Me._lb_日_3.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_日_3.BackColor = System.Drawing.Color.Transparent
		Me._lb_日_3.Text = "日"
		Me._lb_日_3.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_日_3.Size = New System.Drawing.Size(12, 15)
		Me._lb_日_3.Location = New System.Drawing.Point(290, 119)
		Me._lb_日_3.TabIndex = 166
		Me._lb_日_3.Enabled = True
		Me._lb_日_3.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_日_3.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_日_3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_日_3.UseMnemonic = True
		Me._lb_日_3.Visible = True
		Me._lb_日_3.AutoSize = False
		Me._lb_日_3.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_日_3.Name = "_lb_日_3"
		Me._lb_月_3.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_月_3.BackColor = System.Drawing.Color.Transparent
		Me._lb_月_3.Text = "月"
		Me._lb_月_3.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_月_3.Size = New System.Drawing.Size(12, 15)
		Me._lb_月_3.Location = New System.Drawing.Point(254, 119)
		Me._lb_月_3.TabIndex = 165
		Me._lb_月_3.Enabled = True
		Me._lb_月_3.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_月_3.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_月_3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_月_3.UseMnemonic = True
		Me._lb_月_3.Visible = True
		Me._lb_月_3.AutoSize = False
		Me._lb_月_3.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_月_3.Name = "_lb_月_3"
		Me._lb_年_3.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_年_3.BackColor = System.Drawing.Color.Transparent
		Me._lb_年_3.Text = "年"
		Me._lb_年_3.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_年_3.Size = New System.Drawing.Size(17, 15)
		Me._lb_年_3.Location = New System.Drawing.Point(217, 119)
		Me._lb_年_3.TabIndex = 164
		Me._lb_年_3.Enabled = True
		Me._lb_年_3.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_年_3.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_年_3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_年_3.UseMnemonic = True
		Me._lb_年_3.Visible = True
		Me._lb_年_3.AutoSize = False
		Me._lb_年_3.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_年_3.Name = "_lb_年_3"
		Me._lb_月_1.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_月_1.BackColor = System.Drawing.Color.Transparent
		Me._lb_月_1.Text = "月"
		Me._lb_月_1.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_月_1.Size = New System.Drawing.Size(12, 15)
		Me._lb_月_1.Location = New System.Drawing.Point(331, 431)
		Me._lb_月_1.TabIndex = 163
		Me._lb_月_1.Enabled = True
		Me._lb_月_1.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_月_1.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_月_1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_月_1.UseMnemonic = True
		Me._lb_月_1.Visible = True
		Me._lb_月_1.AutoSize = False
		Me._lb_月_1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_月_1.Name = "_lb_月_1"
		Me._lblLabels_24.Text = "大小口区分"
		Me._lblLabels_24.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_24.Size = New System.Drawing.Size(85, 13)
		Me._lblLabels_24.Location = New System.Drawing.Point(812, 460)
		Me._lblLabels_24.TabIndex = 162
		Me._lblLabels_24.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_24.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_24.Enabled = True
		Me._lblLabels_24.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_24.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_24.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_24.UseMnemonic = True
		Me._lblLabels_24.Visible = True
		Me._lblLabels_24.AutoSize = False
		Me._lblLabels_24.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_24.Name = "_lblLabels_24"
		Me._lblLabels_23.Text = "0:大口 1:小口"
		Me._lblLabels_23.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_23.Size = New System.Drawing.Size(169, 13)
		Me._lblLabels_23.Location = New System.Drawing.Point(928, 460)
		Me._lblLabels_23.TabIndex = 161
		Me._lblLabels_23.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_23.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_23.Enabled = True
		Me._lblLabels_23.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_23.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_23.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_23.UseMnemonic = True
		Me._lblLabels_23.Visible = True
		Me._lblLabels_23.AutoSize = False
		Me._lblLabels_23.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_23.Name = "_lblLabels_23"
		Me._lblLabels_22.Text = "受注区分"
		Me._lblLabels_22.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_22.Size = New System.Drawing.Size(85, 13)
		Me._lblLabels_22.Location = New System.Drawing.Point(812, 420)
		Me._lblLabels_22.TabIndex = 160
		Me._lblLabels_22.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_22.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_22.Enabled = True
		Me._lblLabels_22.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_22.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_22.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_22.UseMnemonic = True
		Me._lblLabels_22.Visible = True
		Me._lblLabels_22.AutoSize = False
		Me._lblLabels_22.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_22.Name = "_lblLabels_22"
		Me._lblLabels_21.Text = "0:仮 1:確定"
		Me._lblLabels_21.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_21.Size = New System.Drawing.Size(169, 13)
		Me._lblLabels_21.Location = New System.Drawing.Point(928, 420)
		Me._lblLabels_21.TabIndex = 159
		Me._lblLabels_21.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_21.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_21.Enabled = True
		Me._lblLabels_21.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_21.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_21.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_21.UseMnemonic = True
		Me._lblLabels_21.Visible = True
		Me._lblLabels_21.AutoSize = False
		Me._lblLabels_21.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_21.Name = "_lblLabels_21"
		Me._lblLabels_20.Text = "千円"
		Me._lblLabels_20.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_20.Size = New System.Drawing.Size(33, 13)
		Me._lblLabels_20.Location = New System.Drawing.Point(308, 516)
		Me._lblLabels_20.TabIndex = 158
		Me._lblLabels_20.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_20.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_20.Enabled = True
		Me._lblLabels_20.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_20.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_20.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_20.UseMnemonic = True
		Me._lblLabels_20.Visible = True
		Me._lblLabels_20.AutoSize = False
		Me._lblLabels_20.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_20.Name = "_lblLabels_20"
		Me.rf_担当者名.Text = "ＷＷＷＷＷＷＷＷ"
		Me.rf_担当者名.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.rf_担当者名.Size = New System.Drawing.Size(130, 19)
		Me.rf_担当者名.Location = New System.Drawing.Point(236, 96)
		Me.rf_担当者名.TabIndex = 157
		Me.rf_担当者名.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.rf_担当者名.BackColor = System.Drawing.SystemColors.Control
		Me.rf_担当者名.Enabled = True
		Me.rf_担当者名.ForeColor = System.Drawing.SystemColors.ControlText
		Me.rf_担当者名.Cursor = System.Windows.Forms.Cursors.Default
		Me.rf_担当者名.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.rf_担当者名.UseMnemonic = True
		Me.rf_担当者名.Visible = True
		Me.rf_担当者名.AutoSize = False
		Me.rf_担当者名.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.rf_担当者名.Name = "rf_担当者名"
		Me.lb_担当者CD.Text = "担当者"
		Me.lb_担当者CD.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.lb_担当者CD.Size = New System.Drawing.Size(45, 13)
		Me.lb_担当者CD.Location = New System.Drawing.Point(128, 100)
		Me.lb_担当者CD.TabIndex = 156
		Me.lb_担当者CD.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lb_担当者CD.BackColor = System.Drawing.SystemColors.Control
		Me.lb_担当者CD.Enabled = True
		Me.lb_担当者CD.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lb_担当者CD.Cursor = System.Windows.Forms.Cursors.Default
		Me.lb_担当者CD.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lb_担当者CD.UseMnemonic = True
		Me.lb_担当者CD.Visible = True
		Me.lb_担当者CD.AutoSize = False
		Me.lb_担当者CD.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lb_担当者CD.Name = "lb_担当者CD"
		Me._lblLabels_19.Text = "日"
		Me._lblLabels_19.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_19.Size = New System.Drawing.Size(17, 13)
		Me._lblLabels_19.Location = New System.Drawing.Point(1192, 400)
		Me._lblLabels_19.TabIndex = 155
		Me._lblLabels_19.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_19.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_19.Enabled = True
		Me._lblLabels_19.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_19.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_19.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_19.UseMnemonic = True
		Me._lblLabels_19.Visible = True
		Me._lblLabels_19.AutoSize = False
		Me._lblLabels_19.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_19.Name = "_lblLabels_19"
		Me._lblLabels_18.Text = "有効期限"
		Me._lblLabels_18.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_18.Size = New System.Drawing.Size(57, 13)
		Me._lblLabels_18.Location = New System.Drawing.Point(1104, 400)
		Me._lblLabels_18.TabIndex = 154
		Me._lblLabels_18.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_18.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_18.Enabled = True
		Me._lblLabels_18.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_18.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_18.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_18.UseMnemonic = True
		Me._lblLabels_18.Visible = True
		Me._lblLabels_18.AutoSize = False
		Me._lblLabels_18.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_18.Name = "_lblLabels_18"
		Me._lblLabels_17.Text = "0:出力しない 1:出力する"
		Me._lblLabels_17.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_17.Size = New System.Drawing.Size(169, 13)
		Me._lblLabels_17.Location = New System.Drawing.Point(928, 400)
		Me._lblLabels_17.TabIndex = 153
		Me._lblLabels_17.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_17.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_17.Enabled = True
		Me._lblLabels_17.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_17.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_17.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_17.UseMnemonic = True
		Me._lblLabels_17.Visible = True
		Me._lblLabels_17.AutoSize = False
		Me._lblLabels_17.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_17.Name = "_lblLabels_17"
		Me._lblLabels_16.Text = "0:納期表示 1:別途御打ち合せによる 2:その他"
		Me._lblLabels_16.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_16.Size = New System.Drawing.Size(297, 13)
		Me._lblLabels_16.Location = New System.Drawing.Point(928, 360)
		Me._lblLabels_16.TabIndex = 152
		Me._lblLabels_16.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_16.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_16.Enabled = True
		Me._lblLabels_16.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_16.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_16.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_16.UseMnemonic = True
		Me._lblLabels_16.Visible = True
		Me._lblLabels_16.AutoSize = False
		Me._lblLabels_16.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_16.Name = "_lblLabels_16"
		Me._Shape1_5.BorderColor = System.Drawing.SystemColors.ControlDark
		Me._Shape1_5.Size = New System.Drawing.Size(421, 209)
		Me._Shape1_5.Location = New System.Drawing.Point(808, 292)
		Me._Shape1_5.BackColor = System.Drawing.SystemColors.Window
		Me._Shape1_5.BackStyle = Microsoft.VisualBasic.PowerPacks.BackStyle.Transparent
		Me._Shape1_5.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid
		Me._Shape1_5.BorderWidth = 1
		Me._Shape1_5.FillColor = System.Drawing.Color.Black
		Me._Shape1_5.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Transparent
		Me._Shape1_5.Visible = True
		Me._Shape1_5.Name = "_Shape1_5"
		Me._lblLabels_14.Text = "出精値引"
		Me._lblLabels_14.Enabled = False
		Me._lblLabels_14.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_14.Size = New System.Drawing.Size(81, 13)
		Me._lblLabels_14.Location = New System.Drawing.Point(812, 480)
		Me._lblLabels_14.TabIndex = 151
		Me._lblLabels_14.Visible = False
		Me._lblLabels_14.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_14.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_14.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_14.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_14.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_14.UseMnemonic = True
		Me._lblLabels_14.AutoSize = False
		Me._lblLabels_14.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_14.Name = "_lblLabels_14"
		Me._lblLabels_13.Text = "見積日出力"
		Me._lblLabels_13.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_13.Size = New System.Drawing.Size(85, 13)
		Me._lblLabels_13.Location = New System.Drawing.Point(812, 400)
		Me._lblLabels_13.TabIndex = 150
		Me._lblLabels_13.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_13.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_13.Enabled = True
		Me._lblLabels_13.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_13.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_13.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_13.UseMnemonic = True
		Me._lblLabels_13.Visible = True
		Me._lblLabels_13.AutoSize = False
		Me._lblLabels_13.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_13.Name = "_lblLabels_13"
		Me._lblLabels_12.Text = "0:新店 1:改装 2:メンテ 6:委託"
		Me._lblLabels_12.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_12.Size = New System.Drawing.Size(433, 13)
		Me._lblLabels_12.Location = New System.Drawing.Point(236, 536)
		Me._lblLabels_12.TabIndex = 149
		Me._lblLabels_12.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_12.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_12.Enabled = True
		Me._lblLabels_12.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_12.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_12.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_12.UseMnemonic = True
		Me._lblLabels_12.Visible = True
		Me._lblLabels_12.AutoSize = False
		Me._lblLabels_12.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_12.Name = "_lblLabels_12"
		Me._lblLabels_11.Text = "納期表示"
		Me._lblLabels_11.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_11.Size = New System.Drawing.Size(81, 13)
		Me._lblLabels_11.Location = New System.Drawing.Point(812, 360)
		Me._lblLabels_11.TabIndex = 148
		Me._lblLabels_11.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_11.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_11.Enabled = True
		Me._lblLabels_11.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_11.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_11.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_11.UseMnemonic = True
		Me._lblLabels_11.Visible = True
		Me._lblLabels_11.AutoSize = False
		Me._lblLabels_11.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_11.Name = "_lblLabels_11"
		Me._Shape1_4.BorderColor = System.Drawing.SystemColors.ControlDark
		Me._Shape1_4.Size = New System.Drawing.Size(593, 149)
		Me._Shape1_4.Location = New System.Drawing.Point(108, 488)
		Me._Shape1_4.BackColor = System.Drawing.SystemColors.Window
		Me._Shape1_4.BackStyle = Microsoft.VisualBasic.PowerPacks.BackStyle.Transparent
		Me._Shape1_4.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid
		Me._Shape1_4.BorderWidth = 1
		Me._Shape1_4.FillColor = System.Drawing.Color.Black
		Me._Shape1_4.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Transparent
		Me._Shape1_4.Visible = True
		Me._Shape1_4.Name = "_Shape1_4"
		Me._lblLabels_8.Text = "物件種別"
		Me._lblLabels_8.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_8.Size = New System.Drawing.Size(73, 13)
		Me._lblLabels_8.Location = New System.Drawing.Point(112, 536)
		Me._lblLabels_8.TabIndex = 147
		Me._lblLabels_8.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_8.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_8.Enabled = True
		Me._lblLabels_8.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_8.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_8.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_8.UseMnemonic = True
		Me._lblLabels_8.Visible = True
		Me._lblLabels_8.AutoSize = False
		Me._lblLabels_8.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_8.Name = "_lblLabels_8"
		Me._lblLabels_7.Text = "オープン日"
		Me._lblLabels_7.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_7.Size = New System.Drawing.Size(81, 13)
		Me._lblLabels_7.Location = New System.Drawing.Point(360, 516)
		Me._lblLabels_7.TabIndex = 146
		Me._lblLabels_7.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_7.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_7.Enabled = True
		Me._lblLabels_7.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_7.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_7.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_7.UseMnemonic = True
		Me._lblLabels_7.Visible = True
		Me._lblLabels_7.AutoSize = False
		Me._lblLabels_7.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_7.Name = "_lblLabels_7"
		Me._lblLabels_6.Text = "物件規模金額"
		Me._lblLabels_6.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_6.Size = New System.Drawing.Size(85, 13)
		Me._lblLabels_6.Location = New System.Drawing.Point(112, 516)
		Me._lblLabels_6.TabIndex = 145
		Me._lblLabels_6.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_6.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_6.Enabled = True
		Me._lblLabels_6.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_6.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_6.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_6.UseMnemonic = True
		Me._lblLabels_6.Visible = True
		Me._lblLabels_6.AutoSize = False
		Me._lblLabels_6.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_6.Name = "_lblLabels_6"
		Me._lb_日_2.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_日_2.BackColor = System.Drawing.Color.Transparent
		Me._lb_日_2.Text = "日"
		Me._lb_日_2.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_日_2.Size = New System.Drawing.Size(12, 15)
		Me._lb_日_2.Location = New System.Drawing.Point(560, 515)
		Me._lb_日_2.TabIndex = 143
		Me._lb_日_2.Enabled = True
		Me._lb_日_2.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_日_2.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_日_2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_日_2.UseMnemonic = True
		Me._lb_日_2.Visible = True
		Me._lb_日_2.AutoSize = False
		Me._lb_日_2.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_日_2.Name = "_lb_日_2"
		Me._lb_月_2.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_月_2.BackColor = System.Drawing.Color.Transparent
		Me._lb_月_2.Text = "月"
		Me._lb_月_2.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_月_2.Size = New System.Drawing.Size(12, 15)
		Me._lb_月_2.Location = New System.Drawing.Point(524, 515)
		Me._lb_月_2.TabIndex = 142
		Me._lb_月_2.Enabled = True
		Me._lb_月_2.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_月_2.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_月_2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_月_2.UseMnemonic = True
		Me._lb_月_2.Visible = True
		Me._lb_月_2.AutoSize = False
		Me._lb_月_2.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_月_2.Name = "_lb_月_2"
		Me._lb_年_2.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_年_2.BackColor = System.Drawing.Color.Transparent
		Me._lb_年_2.Text = "年"
		Me._lb_年_2.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_年_2.Size = New System.Drawing.Size(17, 15)
		Me._lb_年_2.Location = New System.Drawing.Point(487, 515)
		Me._lb_年_2.TabIndex = 141
		Me._lb_年_2.Enabled = True
		Me._lb_年_2.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_年_2.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_年_2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_年_2.UseMnemonic = True
		Me._lb_年_2.Visible = True
		Me._lb_年_2.AutoSize = False
		Me._lb_年_2.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_年_2.Name = "_lb_年_2"
		Me._Shape1_3.BorderColor = System.Drawing.SystemColors.ControlDark
		Me._Shape1_3.Size = New System.Drawing.Size(593, 29)
		Me._Shape1_3.Location = New System.Drawing.Point(108, 456)
		Me._Shape1_3.BackColor = System.Drawing.SystemColors.Window
		Me._Shape1_3.BackStyle = Microsoft.VisualBasic.PowerPacks.BackStyle.Transparent
		Me._Shape1_3.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid
		Me._Shape1_3.BorderWidth = 1
		Me._Shape1_3.FillColor = System.Drawing.Color.Black
		Me._Shape1_3.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Transparent
		Me._Shape1_3.Visible = True
		Me._Shape1_3.Name = "_Shape1_3"
		Me._Shape1_0.BorderColor = System.Drawing.SystemColors.ControlDark
		Me._Shape1_0.Size = New System.Drawing.Size(593, 93)
		Me._Shape1_0.Location = New System.Drawing.Point(108, 164)
		Me._Shape1_0.BackColor = System.Drawing.SystemColors.Window
		Me._Shape1_0.BackStyle = Microsoft.VisualBasic.PowerPacks.BackStyle.Transparent
		Me._Shape1_0.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid
		Me._Shape1_0.BorderWidth = 1
		Me._Shape1_0.FillColor = System.Drawing.Color.Black
		Me._Shape1_0.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Transparent
		Me._Shape1_0.Visible = True
		Me._Shape1_0.Name = "_Shape1_0"
		Me._Shape1_1.BorderColor = System.Drawing.SystemColors.ControlDark
		Me._Shape1_1.Size = New System.Drawing.Size(593, 89)
		Me._Shape1_1.Location = New System.Drawing.Point(108, 260)
		Me._Shape1_1.BackColor = System.Drawing.SystemColors.Window
		Me._Shape1_1.BackStyle = Microsoft.VisualBasic.PowerPacks.BackStyle.Transparent
		Me._Shape1_1.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid
		Me._Shape1_1.BorderWidth = 1
		Me._Shape1_1.FillColor = System.Drawing.Color.Black
		Me._Shape1_1.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Transparent
		Me._Shape1_1.Visible = True
		Me._Shape1_1.Name = "_Shape1_1"
		Me._Shape1_2.BorderColor = System.Drawing.SystemColors.ControlDark
		Me._Shape1_2.Size = New System.Drawing.Size(593, 29)
		Me._Shape1_2.Location = New System.Drawing.Point(108, 424)
		Me._Shape1_2.BackColor = System.Drawing.SystemColors.Window
		Me._Shape1_2.BackStyle = Microsoft.VisualBasic.PowerPacks.BackStyle.Transparent
		Me._Shape1_2.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid
		Me._Shape1_2.BorderWidth = 1
		Me._Shape1_2.FillColor = System.Drawing.Color.Black
		Me._Shape1_2.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Transparent
		Me._Shape1_2.Visible = True
		Me._Shape1_2.Name = "_Shape1_2"
		Me._lb_kara_0.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_kara_0.Text = "～"
		Me._lb_kara_0.Size = New System.Drawing.Size(17, 12)
		Me._lb_kara_0.Location = New System.Drawing.Point(239, 433)
		Me._lb_kara_0.TabIndex = 139
		Me._lb_kara_0.BackColor = System.Drawing.SystemColors.Control
		Me._lb_kara_0.Enabled = True
		Me._lb_kara_0.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_kara_0.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_kara_0.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_kara_0.UseMnemonic = True
		Me._lb_kara_0.Visible = True
		Me._lb_kara_0.AutoSize = False
		Me._lb_kara_0.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_kara_0.Name = "_lb_kara_0"
		Me._lb_年_0.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_年_0.BackColor = System.Drawing.Color.Transparent
		Me._lb_年_0.Text = "年"
		Me._lb_年_0.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_年_0.Size = New System.Drawing.Size(17, 15)
		Me._lb_年_0.Location = New System.Drawing.Point(149, 431)
		Me._lb_年_0.TabIndex = 137
		Me._lb_年_0.Enabled = True
		Me._lb_年_0.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_年_0.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_年_0.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_年_0.UseMnemonic = True
		Me._lb_年_0.Visible = True
		Me._lb_年_0.AutoSize = False
		Me._lb_年_0.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_年_0.Name = "_lb_年_0"
		Me._lb_月_0.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_月_0.BackColor = System.Drawing.Color.Transparent
		Me._lb_月_0.Text = "月"
		Me._lb_月_0.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_月_0.Size = New System.Drawing.Size(12, 15)
		Me._lb_月_0.Location = New System.Drawing.Point(186, 431)
		Me._lb_月_0.TabIndex = 136
		Me._lb_月_0.Enabled = True
		Me._lb_月_0.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_月_0.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_月_0.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_月_0.UseMnemonic = True
		Me._lb_月_0.Visible = True
		Me._lb_月_0.AutoSize = False
		Me._lb_月_0.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_月_0.Name = "_lb_月_0"
		Me._lb_日_0.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_日_0.BackColor = System.Drawing.Color.Transparent
		Me._lb_日_0.Text = "日"
		Me._lb_日_0.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_日_0.Size = New System.Drawing.Size(12, 15)
		Me._lb_日_0.Location = New System.Drawing.Point(222, 431)
		Me._lb_日_0.TabIndex = 135
		Me._lb_日_0.Enabled = True
		Me._lb_日_0.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_日_0.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_日_0.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_日_0.UseMnemonic = True
		Me._lb_日_0.Visible = True
		Me._lb_日_0.AutoSize = False
		Me._lb_日_0.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_日_0.Name = "_lb_日_0"
		Me._lb_年_1.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_年_1.BackColor = System.Drawing.Color.Transparent
		Me._lb_年_1.Text = "年"
		Me._lb_年_1.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_年_1.Size = New System.Drawing.Size(17, 15)
		Me._lb_年_1.Location = New System.Drawing.Point(294, 431)
		Me._lb_年_1.TabIndex = 134
		Me._lb_年_1.Enabled = True
		Me._lb_年_1.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_年_1.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_年_1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_年_1.UseMnemonic = True
		Me._lb_年_1.Visible = True
		Me._lb_年_1.AutoSize = False
		Me._lb_年_1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_年_1.Name = "_lb_年_1"
		Me._lb_日_1.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me._lb_日_1.BackColor = System.Drawing.Color.Transparent
		Me._lb_日_1.Text = "日"
		Me._lb_日_1.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_日_1.Size = New System.Drawing.Size(12, 15)
		Me._lb_日_1.Location = New System.Drawing.Point(367, 431)
		Me._lb_日_1.TabIndex = 133
		Me._lb_日_1.Enabled = True
		Me._lb_日_1.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_日_1.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_日_1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_日_1.UseMnemonic = True
		Me._lb_日_1.Visible = True
		Me._lb_日_1.AutoSize = False
		Me._lb_日_1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lb_日_1.Name = "_lb_日_1"
		Me._lblLabels_5.Text = "担当者"
		Me._lblLabels_5.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_5.Size = New System.Drawing.Size(41, 13)
		Me._lblLabels_5.Location = New System.Drawing.Point(172, 328)
		Me._lblLabels_5.TabIndex = 132
		Me._lblLabels_5.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_5.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_5.Enabled = True
		Me._lblLabels_5.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_5.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_5.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_5.UseMnemonic = True
		Me._lblLabels_5.Visible = True
		Me._lblLabels_5.AutoSize = False
		Me._lblLabels_5.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_5.Name = "_lblLabels_5"
		Me._lblLabels_4.Text = "TEL"
		Me._lblLabels_4.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_4.Size = New System.Drawing.Size(25, 13)
		Me._lblLabels_4.Location = New System.Drawing.Point(188, 308)
		Me._lblLabels_4.TabIndex = 131
		Me._lblLabels_4.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_4.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_4.Enabled = True
		Me._lblLabels_4.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_4.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_4.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_4.UseMnemonic = True
		Me._lblLabels_4.Visible = True
		Me._lblLabels_4.AutoSize = False
		Me._lblLabels_4.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_4.Name = "_lblLabels_4"
		Me._lblLabels_3.Text = "FAX"
		Me._lblLabels_3.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_3.Size = New System.Drawing.Size(25, 13)
		Me._lblLabels_3.Location = New System.Drawing.Point(376, 308)
		Me._lblLabels_3.TabIndex = 130
		Me._lblLabels_3.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_3.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_3.Enabled = True
		Me._lblLabels_3.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_3.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_3.UseMnemonic = True
		Me._lblLabels_3.Visible = True
		Me._lblLabels_3.AutoSize = False
		Me._lblLabels_3.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_3.Name = "_lblLabels_3"
		Me._lblLabels_2.Text = "住所"
		Me._lblLabels_2.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_2.Size = New System.Drawing.Size(29, 13)
		Me._lblLabels_2.Location = New System.Drawing.Point(124, 288)
		Me._lblLabels_2.TabIndex = 129
		Me._lblLabels_2.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_2.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_2.Enabled = True
		Me._lblLabels_2.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_2.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_2.UseMnemonic = True
		Me._lblLabels_2.Visible = True
		Me._lblLabels_2.AutoSize = False
		Me._lblLabels_2.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_2.Name = "_lblLabels_2"
		Me._lblLabels_0.Text = "FAX"
		Me._lblLabels_0.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_0.Size = New System.Drawing.Size(25, 13)
		Me._lblLabels_0.Location = New System.Drawing.Point(332, 196)
		Me._lblLabels_0.TabIndex = 128
		Me._lblLabels_0.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_0.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_0.Enabled = True
		Me._lblLabels_0.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_0.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_0.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_0.UseMnemonic = True
		Me._lblLabels_0.Visible = True
		Me._lblLabels_0.AutoSize = False
		Me._lblLabels_0.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_0.Name = "_lblLabels_0"
		Me._lblLabels_10.Text = "TEL"
		Me._lblLabels_10.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_10.Size = New System.Drawing.Size(25, 13)
		Me._lblLabels_10.Location = New System.Drawing.Point(148, 196)
		Me._lblLabels_10.TabIndex = 127
		Me._lblLabels_10.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_10.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_10.Enabled = True
		Me._lblLabels_10.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_10.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_10.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_10.UseMnemonic = True
		Me._lblLabels_10.Visible = True
		Me._lblLabels_10.AutoSize = False
		Me._lblLabels_10.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_10.Name = "_lblLabels_10"
		Me.lb_見積番号.Text = "見積№"
		Me.lb_見積番号.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.lb_見積番号.Size = New System.Drawing.Size(57, 13)
		Me.lb_見積番号.Location = New System.Drawing.Point(12, 24)
		Me.lb_見積番号.TabIndex = 126
		Me.lb_見積番号.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lb_見積番号.BackColor = System.Drawing.SystemColors.Control
		Me.lb_見積番号.Enabled = True
		Me.lb_見積番号.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lb_見積番号.Cursor = System.Windows.Forms.Cursors.Default
		Me.lb_見積番号.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lb_見積番号.UseMnemonic = True
		Me.lb_見積番号.Visible = True
		Me.lb_見積番号.AutoSize = False
		Me.lb_見積番号.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lb_見積番号.Name = "lb_見積番号"
		Me.rf_見積番号.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.rf_見積番号.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.rf_見積番号.Size = New System.Drawing.Size(101, 13)
		Me.rf_見積番号.Location = New System.Drawing.Point(100, 24)
		Me.rf_見積番号.TabIndex = 125
		Me.rf_見積番号.BackColor = System.Drawing.SystemColors.Control
		Me.rf_見積番号.Enabled = True
		Me.rf_見積番号.ForeColor = System.Drawing.SystemColors.ControlText
		Me.rf_見積番号.Cursor = System.Windows.Forms.Cursors.Default
		Me.rf_見積番号.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.rf_見積番号.UseMnemonic = True
		Me.rf_見積番号.Visible = True
		Me.rf_見積番号.AutoSize = False
		Me.rf_見積番号.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.rf_見積番号.Name = "rf_見積番号"
		Me.lb_見積件名.Text = "見積件名"
		Me.lb_見積件名.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.lb_見積件名.Size = New System.Drawing.Size(61, 13)
		Me.lb_見積件名.Location = New System.Drawing.Point(116, 140)
		Me.lb_見積件名.TabIndex = 124
		Me.lb_見積件名.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lb_見積件名.BackColor = System.Drawing.SystemColors.Control
		Me.lb_見積件名.Enabled = True
		Me.lb_見積件名.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lb_見積件名.Cursor = System.Windows.Forms.Cursors.Default
		Me.lb_見積件名.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lb_見積件名.UseMnemonic = True
		Me.lb_見積件名.Visible = True
		Me.lb_見積件名.AutoSize = False
		Me.lb_見積件名.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lb_見積件名.Name = "lb_見積件名"
		Me.Line1.X1 = 8
		Me.Line1.X2 = 212
		Me.Line1.Y1 = 40
		Me.Line1.Y2 = 40
		Me.Line1.BorderColor = System.Drawing.SystemColors.WindowText
		Me.Line1.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid
		Me.Line1.BorderWidth = 1
		Me.Line1.Visible = True
		Me.Line1.Name = "Line1"
		Me._lb_項目_0.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me._lb_項目_0.Text = "得意先"
		Me._lb_項目_0.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_0.ForeColor = System.Drawing.Color.White
		Me._lb_項目_0.Size = New System.Drawing.Size(101, 93)
		Me._lb_項目_0.Location = New System.Drawing.Point(4, 164)
		Me._lb_項目_0.TabIndex = 123
		Me._lb_項目_0.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lb_項目_0.Enabled = True
		Me._lb_項目_0.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_0.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_0.UseMnemonic = True
		Me._lb_項目_0.Visible = True
		Me._lb_項目_0.AutoSize = False
		Me._lb_項目_0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me._lb_項目_0.Name = "_lb_項目_0"
		Me._lb_項目_6.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me._lb_項目_6.Text = "見積" & Chr(13) & Chr(10) & "コントロール"
		Me._lb_項目_6.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_6.ForeColor = System.Drawing.Color.White
		Me._lb_項目_6.Size = New System.Drawing.Size(101, 209)
		Me._lb_項目_6.Location = New System.Drawing.Point(704, 292)
		Me._lb_項目_6.TabIndex = 118
		Me._lb_項目_6.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lb_項目_6.Enabled = True
		Me._lb_項目_6.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_6.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_6.UseMnemonic = True
		Me._lb_項目_6.Visible = True
		Me._lb_項目_6.AutoSize = False
		Me._lb_項目_6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me._lb_項目_6.Name = "_lb_項目_6"
		Me._lb_項目_2.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me._lb_項目_2.Text = "物件情報"
		Me._lb_項目_2.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_2.ForeColor = System.Drawing.Color.White
		Me._lb_項目_2.Size = New System.Drawing.Size(101, 149)
		Me._lb_項目_2.Location = New System.Drawing.Point(4, 488)
		Me._lb_項目_2.TabIndex = 117
		Me._lb_項目_2.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lb_項目_2.Enabled = True
		Me._lb_項目_2.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_2.UseMnemonic = True
		Me._lb_項目_2.Visible = True
		Me._lb_項目_2.AutoSize = False
		Me._lb_項目_2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me._lb_項目_2.Name = "_lb_項目_2"
		Me._lb_項目_3.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me._lb_項目_3.Text = "納期"
		Me._lb_項目_3.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_3.ForeColor = System.Drawing.Color.White
		Me._lb_項目_3.Size = New System.Drawing.Size(101, 29)
		Me._lb_項目_3.Location = New System.Drawing.Point(4, 424)
		Me._lb_項目_3.TabIndex = 116
		Me._lb_項目_3.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lb_項目_3.Enabled = True
		Me._lb_項目_3.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_3.UseMnemonic = True
		Me._lb_項目_3.Visible = True
		Me._lb_項目_3.AutoSize = False
		Me._lb_項目_3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me._lb_項目_3.Name = "_lb_項目_3"
		Me._lb_項目_4.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me._lb_項目_4.Text = "備考"
		Me._lb_項目_4.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_4.ForeColor = System.Drawing.Color.White
		Me._lb_項目_4.Size = New System.Drawing.Size(101, 29)
		Me._lb_項目_4.Location = New System.Drawing.Point(4, 456)
		Me._lb_項目_4.TabIndex = 115
		Me._lb_項目_4.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lb_項目_4.Enabled = True
		Me._lb_項目_4.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_4.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_4.UseMnemonic = True
		Me._lb_項目_4.Visible = True
		Me._lb_項目_4.AutoSize = False
		Me._lb_項目_4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me._lb_項目_4.Name = "_lb_項目_4"
		Me._lb_項目_1.BackColor = System.Drawing.Color.FromARGB(128, 128, 128)
		Me._lb_項目_1.Text = "納入先"
		Me._lb_項目_1.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lb_項目_1.ForeColor = System.Drawing.Color.White
		Me._lb_項目_1.Size = New System.Drawing.Size(101, 89)
		Me._lb_項目_1.Location = New System.Drawing.Point(4, 260)
		Me._lb_項目_1.TabIndex = 114
		Me._lb_項目_1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lb_項目_1.Enabled = True
		Me._lb_項目_1.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_項目_1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_項目_1.UseMnemonic = True
		Me._lb_項目_1.Visible = True
		Me._lb_項目_1.AutoSize = False
		Me._lb_項目_1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me._lb_項目_1.Name = "_lb_項目_1"
		Me.rf_処理区分.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me.rf_処理区分.Text = "≪登 録≫"
		Me.rf_処理区分.Font = New System.Drawing.Font("ＭＳ ゴシック", 11.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.rf_処理区分.Size = New System.Drawing.Size(81, 19)
		Me.rf_処理区分.Location = New System.Drawing.Point(4, 4)
		Me.rf_処理区分.TabIndex = 113
		Me.rf_処理区分.BackColor = System.Drawing.SystemColors.Control
		Me.rf_処理区分.Enabled = True
		Me.rf_処理区分.ForeColor = System.Drawing.SystemColors.ControlText
		Me.rf_処理区分.Cursor = System.Windows.Forms.Cursors.Default
		Me.rf_処理区分.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.rf_処理区分.UseMnemonic = True
		Me.rf_処理区分.Visible = True
		Me.rf_処理区分.AutoSize = False
		Me.rf_処理区分.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.rf_処理区分.Name = "rf_処理区分"
		Me._ln_over_1.BorderColor = System.Drawing.Color.White
		Me._ln_over_1.X1 = 0
		Me._ln_over_1.X2 = 632
		Me._ln_over_1.Y1 = 1
		Me._ln_over_1.Y2 = 1
		Me._ln_over_1.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid
		Me._ln_over_1.BorderWidth = 1
		Me._ln_over_1.Visible = True
		Me._ln_over_1.Name = "_ln_over_1"
		Me._ln_over_0.BorderColor = System.Drawing.Color.FromARGB(64, 64, 0)
		Me._ln_over_0.X1 = 1
		Me._ln_over_0.X2 = 632
		Me._ln_over_0.Y1 = 0
		Me._ln_over_0.Y2 = 0
		Me._ln_over_0.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid
		Me._ln_over_0.BorderWidth = 1
		Me._ln_over_0.Visible = True
		Me._ln_over_0.Name = "_ln_over_0"
		Me._lb_納期_0.BackColor = System.Drawing.SystemColors.Window
		Me._lb_納期_0.Size = New System.Drawing.Size(127, 19)
		Me._lb_納期_0.Location = New System.Drawing.Point(112, 428)
		Me._lb_納期_0.TabIndex = 138
		Me._lb_納期_0.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lb_納期_0.Enabled = True
		Me._lb_納期_0.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_納期_0.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_納期_0.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_納期_0.UseMnemonic = True
		Me._lb_納期_0.Visible = True
		Me._lb_納期_0.AutoSize = False
		Me._lb_納期_0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._lb_納期_0.Name = "_lb_納期_0"
		Me._lb_納期_1.BackColor = System.Drawing.SystemColors.Window
		Me._lb_納期_1.Size = New System.Drawing.Size(127, 19)
		Me._lb_納期_1.Location = New System.Drawing.Point(257, 428)
		Me._lb_納期_1.TabIndex = 140
		Me._lb_納期_1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lb_納期_1.Enabled = True
		Me._lb_納期_1.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_納期_1.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_納期_1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_納期_1.UseMnemonic = True
		Me._lb_納期_1.Visible = True
		Me._lb_納期_1.AutoSize = False
		Me._lb_納期_1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._lb_納期_1.Name = "_lb_納期_1"
		Me.lb_OPEN日.BackColor = System.Drawing.SystemColors.Window
		Me.lb_OPEN日.Size = New System.Drawing.Size(127, 19)
		Me.lb_OPEN日.Location = New System.Drawing.Point(448, 512)
		Me.lb_OPEN日.TabIndex = 144
		Me.lb_OPEN日.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lb_OPEN日.Enabled = True
		Me.lb_OPEN日.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lb_OPEN日.Cursor = System.Windows.Forms.Cursors.Default
		Me.lb_OPEN日.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lb_OPEN日.UseMnemonic = True
		Me.lb_OPEN日.Visible = True
		Me.lb_OPEN日.AutoSize = False
		Me.lb_OPEN日.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lb_OPEN日.Name = "lb_OPEN日"
		Me.lb_見積日付.BackColor = System.Drawing.SystemColors.Window
		Me.lb_見積日付.Size = New System.Drawing.Size(127, 19)
		Me.lb_見積日付.Location = New System.Drawing.Point(180, 116)
		Me.lb_見積日付.TabIndex = 167
		Me.lb_見積日付.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lb_見積日付.Enabled = True
		Me.lb_見積日付.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lb_見積日付.Cursor = System.Windows.Forms.Cursors.Default
		Me.lb_見積日付.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lb_見積日付.UseMnemonic = True
		Me.lb_見積日付.Visible = True
		Me.lb_見積日付.AutoSize = False
		Me.lb_見積日付.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lb_見積日付.Name = "lb_見積日付"
		Me.lb_受注日付.BackColor = System.Drawing.SystemColors.Window
		Me.lb_受注日付.Size = New System.Drawing.Size(127, 19)
		Me.lb_受注日付.Location = New System.Drawing.Point(896, 436)
		Me.lb_受注日付.TabIndex = 177
		Me.lb_受注日付.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lb_受注日付.Enabled = True
		Me.lb_受注日付.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lb_受注日付.Cursor = System.Windows.Forms.Cursors.Default
		Me.lb_受注日付.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lb_受注日付.UseMnemonic = True
		Me.lb_受注日付.Visible = True
		Me.lb_受注日付.AutoSize = False
		Me.lb_受注日付.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lb_受注日付.Name = "lb_受注日付"
		Me.Label6.BackColor = System.Drawing.SystemColors.Window
		Me.Label6.Size = New System.Drawing.Size(127, 19)
		Me.Label6.Location = New System.Drawing.Point(200, 552)
		Me.Label6.TabIndex = 212
		Me.Label6.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label6.Enabled = True
		Me.Label6.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label6.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label6.UseMnemonic = True
		Me.Label6.Visible = True
		Me.Label6.AutoSize = False
		Me.Label6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.Label6.Name = "Label6"
		Me.Label8.BackColor = System.Drawing.SystemColors.Window
		Me.Label8.Size = New System.Drawing.Size(127, 19)
		Me.Label8.Location = New System.Drawing.Point(200, 572)
		Me.Label8.TabIndex = 217
		Me.Label8.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label8.Enabled = True
		Me.Label8.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label8.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label8.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label8.UseMnemonic = True
		Me.Label8.Visible = True
		Me.Label8.AutoSize = False
		Me.Label8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.Label8.Name = "Label8"
		Me.Label11.BackColor = System.Drawing.SystemColors.Window
		Me.Label11.Size = New System.Drawing.Size(127, 19)
		Me.Label11.Location = New System.Drawing.Point(896, 548)
		Me.Label11.TabIndex = 243
		Me.Label11.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label11.Enabled = True
		Me.Label11.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label11.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label11.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label11.UseMnemonic = True
		Me.Label11.Visible = True
		Me.Label11.AutoSize = False
		Me.Label11.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.Label11.Name = "Label11"
		Me.rf_ウエルシア物件区分名.Text = "ＷＷＷＷＷＷＷＷ"
		Me.rf_ウエルシア物件区分名.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.rf_ウエルシア物件区分名.Size = New System.Drawing.Size(214, 19)
		Me.rf_ウエルシア物件区分名.Location = New System.Drawing.Point(928, 120)
		Me.rf_ウエルシア物件区分名.TabIndex = 204
		Me.rf_ウエルシア物件区分名.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.rf_ウエルシア物件区分名.BackColor = System.Drawing.SystemColors.Control
		Me.rf_ウエルシア物件区分名.Enabled = True
		Me.rf_ウエルシア物件区分名.ForeColor = System.Drawing.SystemColors.ControlText
		Me.rf_ウエルシア物件区分名.Cursor = System.Windows.Forms.Cursors.Default
		Me.rf_ウエルシア物件区分名.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.rf_ウエルシア物件区分名.UseMnemonic = True
		Me.rf_ウエルシア物件区分名.Visible = True
		Me.rf_ウエルシア物件区分名.AutoSize = False
		Me.rf_ウエルシア物件区分名.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.rf_ウエルシア物件区分名.Name = "rf_ウエルシア物件区分名"
		Me._lblLabels_34.Text = "請求管轄"
		Me._lblLabels_34.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_34.Size = New System.Drawing.Size(77, 13)
		Me._lblLabels_34.Location = New System.Drawing.Point(812, 124)
		Me._lblLabels_34.TabIndex = 203
		Me._lblLabels_34.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_34.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_34.Enabled = True
		Me._lblLabels_34.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_34.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_34.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_34.UseMnemonic = True
		Me._lblLabels_34.Visible = True
		Me._lblLabels_34.AutoSize = False
		Me._lblLabels_34.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_34.Name = "_lblLabels_34"
		Me._lblLabels_33.Text = "1:通常 2:リース"
		Me._lblLabels_33.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_33.Size = New System.Drawing.Size(165, 13)
		Me._lblLabels_33.Location = New System.Drawing.Point(936, 104)
		Me._lblLabels_33.TabIndex = 202
		Me._lblLabels_33.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_33.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_33.Enabled = True
		Me._lblLabels_33.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_33.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_33.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_33.UseMnemonic = True
		Me._lblLabels_33.Visible = True
		Me._lblLabels_33.AutoSize = False
		Me._lblLabels_33.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_33.Name = "_lblLabels_33"
		Me._lblLabels_32.Text = "リース区分"
		Me._lblLabels_32.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me._lblLabels_32.Size = New System.Drawing.Size(73, 13)
		Me._lblLabels_32.Location = New System.Drawing.Point(812, 104)
		Me._lblLabels_32.TabIndex = 201
		Me._lblLabels_32.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblLabels_32.BackColor = System.Drawing.SystemColors.Control
		Me._lblLabels_32.Enabled = True
		Me._lblLabels_32.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabels_32.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabels_32.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabels_32.UseMnemonic = True
		Me._lblLabels_32.Visible = True
		Me._lblLabels_32.AutoSize = False
		Me._lblLabels_32.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblLabels_32.Name = "_lblLabels_32"
		Me._lb_コンテナ_1.BackColor = System.Drawing.Color.White
		Me._lb_コンテナ_1.Size = New System.Drawing.Size(127, 20)
		Me._lb_コンテナ_1.Location = New System.Drawing.Point(896, 567)
		Me._lb_コンテナ_1.TabIndex = 251
		Me._lb_コンテナ_1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lb_コンテナ_1.Enabled = True
		Me._lb_コンテナ_1.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lb_コンテナ_1.Cursor = System.Windows.Forms.Cursors.Default
		Me._lb_コンテナ_1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lb_コンテナ_1.UseMnemonic = True
		Me._lb_コンテナ_1.Visible = True
		Me._lb_コンテナ_1.AutoSize = False
		Me._lb_コンテナ_1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._lb_コンテナ_1.Name = "_lb_コンテナ_1"
		Me.Label16.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.Label16.Text = "統合見積№"
		Me.Label16.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
		Me.Label16.Size = New System.Drawing.Size(69, 13)
		Me.Label16.Location = New System.Drawing.Point(108, 72)
		Me.Label16.TabIndex = 263
		Me.Label16.BackColor = System.Drawing.SystemColors.Control
		Me.Label16.Enabled = True
		Me.Label16.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label16.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label16.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label16.UseMnemonic = True
		Me.Label16.Visible = True
		Me.Label16.AutoSize = False
		Me.Label16.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label16.Name = "Label16"
		Me.Controls.Add(ck_社内伝票扱い)
		Me.Controls.Add(tx_受注日付Y)
		Me.Controls.Add(tx_受注日付M)
		Me.Controls.Add(tx_受注日付D)
		Me.Controls.Add(picFunction)
		Me.Controls.Add(sb_Msg)
		Me.Controls.Add(tx_得意先名2)
		Me.Controls.Add(tx_備考)
		Me.Controls.Add(tx_得意先名1)
		Me.Controls.Add(tx_物件金額)
		Me.Controls.Add(tx_出精値引)
		Me.Controls.Add(tx_見積件名)
		Me.Controls.Add(tx_得TEL)
		Me.Controls.Add(tx_得FAX)
		Me.Controls.Add(tx_得担当者)
		Me.Controls.Add(tx_納入先CD)
		Me.Controls.Add(tx_納得意先CD)
		Me.Controls.Add(tx_納入先名2)
		Me.Controls.Add(tx_納入先名1)
		Me.Controls.Add(tx_郵便番号)
		Me.Controls.Add(tx_納住所1)
		Me.Controls.Add(tx_納住所2)
		Me.Controls.Add(tx_納TEL)
		Me.Controls.Add(tx_納FAX)
		Me.Controls.Add(tx_納担当者)
		Me.Controls.Add(tx_s納期D)
		Me.Controls.Add(tx_s納期M)
		Me.Controls.Add(tx_s納期Y)
		Me.Controls.Add(tx_e納期D)
		Me.Controls.Add(tx_e納期M)
		Me.Controls.Add(tx_e納期Y)
		Me.Controls.Add(tx_得意先CD)
		Me.Controls.Add(tx_OPEN日D)
		Me.Controls.Add(tx_OPEN日M)
		Me.Controls.Add(tx_OPEN日Y)
		Me.Controls.Add(tx_物件種別)
		Me.Controls.Add(tx_納期表示)
		Me.Controls.Add(tx_納期表示他)
		Me.Controls.Add(tx_出力日)
		Me.Controls.Add(tx_有効期限)
		Me.Controls.Add(tx_受注区分)
		Me.Controls.Add(tx_大小口区分)
		Me.Controls.Add(tx_見積日付D)
		Me.Controls.Add(tx_見積日付M)
		Me.Controls.Add(tx_見積日付Y)
		Me.Controls.Add(tx_現場名)
		Me.Controls.Add(tx_支払条件)
		Me.Controls.Add(tx_支払条件他)
		Me.Controls.Add(tx_担当者CD)
		Me.Controls.Add(rf_販売先得意先名2)
		Me.Controls.Add(rf_販売先得意先名1)
		Me.Controls.Add(tx_販売先得意先CD)
		Me.Controls.Add(tx_販売先納入先CD)
		Me.Controls.Add(tx_販売先納得意先CD)
		Me.Controls.Add(rf_販売先納入先名2)
		Me.Controls.Add(rf_販売先納入先名1)
		Me.Controls.Add(tx_部署CD)
		Me.Controls.Add(tx_物件番号)
		Me.Controls.Add(tx_伝票種類)
		Me.Controls.Add(tx_ウエルシア物件内容名)
		Me.Controls.Add(tx_ウエルシア物件内容CD)
		Me.Controls.Add(tx_ウエルシア売場面積)
		Me.Controls.Add(tx_受付日付Y)
		Me.Controls.Add(tx_受付日付M)
		Me.Controls.Add(tx_受付日付D)
		Me.Controls.Add(tx_完工日付Y)
		Me.Controls.Add(tx_完工日付M)
		Me.Controls.Add(tx_完工日付D)
		Me.Controls.Add(tx_発注担当者名)
		Me.Controls.Add(tx_作業内容)
		Me.Controls.Add(tx_YKサプライ区分)
		Me.Controls.Add(tx_YK物件区分)
		Me.Controls.Add(tx_YK請求区分)
		Me.Controls.Add(tx_化粧品メーカー区分)
		Me.Controls.Add(tx_SM内容区分)
		Me.Controls.Add(tx_税集計区分)
		Me.Controls.Add(tx_クレーム区分)
		Me.Controls.Add(tx_工事担当CD)
		Me.Controls.Add(tx_発注書発行日付)
		Me.Controls.Add(tx_完了者名)
		Me.Controls.Add(tx_見積確定区分)
		Me.Controls.Add(tx_経過備考1)
		Me.Controls.Add(tx_ウエルシア物件区分)
		Me.Controls.Add(tx_ウエルシアリース区分)
		Me.Controls.Add(tx_経過備考2)
		Me.Controls.Add(tx_請求予定M)
		Me.Controls.Add(tx_請求予定D)
		Me.Controls.Add(tx_完了日Y)
		Me.Controls.Add(tx_完了日M)
		Me.Controls.Add(tx_完了日D)
		Me.Controls.Add(tx_請求予定Y)
		Me.Controls.Add(tx_集計CD)
		Me.Controls.Add(tx_B請求管轄区分)
		Me.Controls.Add(tx_BtoB番号)
		Me.Controls.Add(tx_業種区分)
		Me.Controls.Add(tx_統合見積番号)
		Me.Controls.Add(rf_統合見積件名)
		Me.Controls.Add(_lblLabels_58)
		Me.Controls.Add(_lblLabels_57)
		Me.Controls.Add(_lblLabels_56)
		Me.Controls.Add(_lblLabels_55)
		Me.Controls.Add(_lblLabels_54)
		Me.Controls.Add(_lb_項目_15)
		Me.ShapeContainer1.Shapes.Add(_Shape1_13)
		Me.Controls.Add(rf_集計名)
		Me.Controls.Add(_lblLabels_53)
		Me.Controls.Add(_lb_日_8)
		Me.Controls.Add(Label14)
		Me.Controls.Add(_lb_項目_14)
		Me.Controls.Add(_lb_項目_13)
		Me.Controls.Add(_lblLabels_52)
		Me.ShapeContainer1.Shapes.Add(_Shape1_12)
		Me.Controls.Add(_lb_項目_12)
		Me.Controls.Add(Label13)
		Me.Controls.Add(_lblLabels_51)
		Me.Controls.Add(Label12)
		Me.Controls.Add(Label9)
		Me.Controls.Add(_lb_日_7)
		Me.Controls.Add(_lb_月_7)
		Me.Controls.Add(_lb_年_7)
		Me.Controls.Add(Label10)
		Me.Controls.Add(rf_工事担当名)
		Me.Controls.Add(_lblLabels_50)
		Me.Controls.Add(_lblLabels_49)
		Me.Controls.Add(_lblLabels_48)
		Me.Controls.Add(_lblLabels_47)
		Me.Controls.Add(_lblLabels_1)
		Me.Controls.Add(rf_税集計区分名)
		Me.Controls.Add(_lblLabels_46)
		Me.Controls.Add(_lblLabels_45)
		Me.Controls.Add(_lblLabels_44)
		Me.Controls.Add(_lblLabels_43)
		Me.ShapeContainer1.Shapes.Add(_Shape1_11)
		Me.Controls.Add(_lb_項目_11)
		Me.ShapeContainer1.Shapes.Add(_Shape1_10)
		Me.Controls.Add(_lblLabels_42)
		Me.Controls.Add(_lblLabels_41)
		Me.Controls.Add(_lblLabels_40)
		Me.Controls.Add(_lblLabels_39)
		Me.Controls.Add(_lblLabels_36)
		Me.Controls.Add(_lblLabels_35)
		Me.Controls.Add(_lblLabels_38)
		Me.Controls.Add(_lblLabels_37)
		Me.Controls.Add(Label7)
		Me.Controls.Add(_lb_日_6)
		Me.Controls.Add(_lb_月_6)
		Me.Controls.Add(_lb_年_6)
		Me.Controls.Add(Label5)
		Me.Controls.Add(_lb_日_5)
		Me.Controls.Add(_lb_月_5)
		Me.Controls.Add(_lb_年_5)
		Me.Controls.Add(_lb_項目_10)
		Me.Controls.Add(rf_物件略称)
		Me.Controls.Add(_lb_項目_9)
		Me.ShapeContainer1.Shapes.Add(_Shape1_9)
		Me.Controls.Add(_lblLabels_31)
		Me.Controls.Add(_lblLabels_30)
		Me.Controls.Add(_lblLabels_29)
		Me.Controls.Add(_lb_項目_7)
		Me.ShapeContainer1.Shapes.Add(_Shape1_7)
		Me.Controls.Add(_lblLabels_28)
		Me.Controls.Add(_lblLabels_27)
		Me.Controls.Add(Label4)
		Me.Controls.Add(rf_部署名)
		Me.Controls.Add(Label3)
		Me.Controls.Add(_lb_項目_8)
		Me.ShapeContainer1.Shapes.Add(_Shape1_8)
		Me.Controls.Add(rf_得意先別見積番号)
		Me.Controls.Add(rf_外税額)
		Me.Controls.Add(rf_原価率)
		Me.Controls.Add(rf_原価合計)
		Me.Controls.Add(rf_合計金額)
		Me.Controls.Add(rf_売上端数)
		Me.Controls.Add(rf_消費税端数)
		Me.Controls.Add(rf_税集計区分)
		Me.Controls.Add(_lblLabels_25)
		Me.Controls.Add(_lb_年_4)
		Me.Controls.Add(_lb_月_4)
		Me.Controls.Add(_lb_日_4)
		Me.Controls.Add(Label2)
		Me.Controls.Add(_lblLabels_15)
		Me.Controls.Add(_lblLabels_9)
		Me.ShapeContainer1.Shapes.Add(_Shape1_6)
		Me.Controls.Add(Label1)
		Me.Controls.Add(_lb_項目_5)
		Me.Controls.Add(_lblLabels_26)
		Me.Controls.Add(_lb_日_3)
		Me.Controls.Add(_lb_月_3)
		Me.Controls.Add(_lb_年_3)
		Me.Controls.Add(_lb_月_1)
		Me.Controls.Add(_lblLabels_24)
		Me.Controls.Add(_lblLabels_23)
		Me.Controls.Add(_lblLabels_22)
		Me.Controls.Add(_lblLabels_21)
		Me.Controls.Add(_lblLabels_20)
		Me.Controls.Add(rf_担当者名)
		Me.Controls.Add(lb_担当者CD)
		Me.Controls.Add(_lblLabels_19)
		Me.Controls.Add(_lblLabels_18)
		Me.Controls.Add(_lblLabels_17)
		Me.Controls.Add(_lblLabels_16)
		Me.ShapeContainer1.Shapes.Add(_Shape1_5)
		Me.Controls.Add(_lblLabels_14)
		Me.Controls.Add(_lblLabels_13)
		Me.Controls.Add(_lblLabels_12)
		Me.Controls.Add(_lblLabels_11)
		Me.ShapeContainer1.Shapes.Add(_Shape1_4)
		Me.Controls.Add(_lblLabels_8)
		Me.Controls.Add(_lblLabels_7)
		Me.Controls.Add(_lblLabels_6)
		Me.Controls.Add(_lb_日_2)
		Me.Controls.Add(_lb_月_2)
		Me.Controls.Add(_lb_年_2)
		Me.ShapeContainer1.Shapes.Add(_Shape1_3)
		Me.ShapeContainer1.Shapes.Add(_Shape1_0)
		Me.ShapeContainer1.Shapes.Add(_Shape1_1)
		Me.ShapeContainer1.Shapes.Add(_Shape1_2)
		Me.Controls.Add(_lb_kara_0)
		Me.Controls.Add(_lb_年_0)
		Me.Controls.Add(_lb_月_0)
		Me.Controls.Add(_lb_日_0)
		Me.Controls.Add(_lb_年_1)
		Me.Controls.Add(_lb_日_1)
		Me.Controls.Add(_lblLabels_5)
		Me.Controls.Add(_lblLabels_4)
		Me.Controls.Add(_lblLabels_3)
		Me.Controls.Add(_lblLabels_2)
		Me.Controls.Add(_lblLabels_0)
		Me.Controls.Add(_lblLabels_10)
		Me.Controls.Add(lb_見積番号)
		Me.Controls.Add(rf_見積番号)
		Me.Controls.Add(lb_見積件名)
		Me.ShapeContainer1.Shapes.Add(Line1)
		Me.Controls.Add(_lb_項目_0)
		Me.Controls.Add(_lb_項目_6)
		Me.Controls.Add(_lb_項目_2)
		Me.Controls.Add(_lb_項目_3)
		Me.Controls.Add(_lb_項目_4)
		Me.Controls.Add(_lb_項目_1)
		Me.Controls.Add(rf_処理区分)
		Me.ShapeContainer1.Shapes.Add(_ln_over_1)
		Me.ShapeContainer1.Shapes.Add(_ln_over_0)
		Me.Controls.Add(_lb_納期_0)
		Me.Controls.Add(_lb_納期_1)
		Me.Controls.Add(lb_OPEN日)
		Me.Controls.Add(lb_見積日付)
		Me.Controls.Add(lb_受注日付)
		Me.Controls.Add(Label6)
		Me.Controls.Add(Label8)
		Me.Controls.Add(Label11)
		Me.Controls.Add(rf_ウエルシア物件区分名)
		Me.Controls.Add(_lblLabels_34)
		Me.Controls.Add(_lblLabels_33)
		Me.Controls.Add(_lblLabels_32)
		Me.Controls.Add(_lb_コンテナ_1)
		Me.Controls.Add(Label16)
		Me.Controls.Add(ShapeContainer1)
		Me.picFunction.Controls.Add(_cbFunc_6)
		Me.picFunction.Controls.Add(_cbFunc_7)
		Me.picFunction.Controls.Add(_cbFunc_5)
		Me.picFunction.Controls.Add(_cbFunc_4)
		Me.picFunction.Controls.Add(_cbFunc_3)
		Me.picFunction.Controls.Add(_cbFunc_2)
		Me.picFunction.Controls.Add(_cbFunc_1)
		Me.picFunction.Controls.Add(_cbFunc_8)
		Me.picFunction.Controls.Add(_cbFunc_9)
		Me.picFunction.Controls.Add(_cbFunc_12)
		Me.picFunction.Controls.Add(_cbFunc_11)
		Me.picFunction.Controls.Add(_cbFunc_10)
		Me.picFunction.Controls.Add(cbTabEnd)
		Me.picFunction.Controls.Add(_lb_Func_12)
		Me.picFunction.Controls.Add(_lb_Func_11)
		Me.picFunction.Controls.Add(_lb_Func_5)
		Me.picFunction.Controls.Add(_lb_Func_3)
		Me.picFunction.Controls.Add(_lb_Func_2)
		Me.picFunction.Controls.Add(_lb_Func_4)
		Me.picFunction.Controls.Add(_lb_Func_1)
		Me.picFunction.Controls.Add(_lb_Func_6)
		Me.picFunction.Controls.Add(_lb_Func_7)
		Me.picFunction.Controls.Add(_lb_Func_8)
		Me.picFunction.Controls.Add(_lb_Func_9)
		Me.picFunction.Controls.Add(_lb_Func_10)
		Me.sb_Msg.Items.AddRange(New System.Windows.Forms.ToolStripItem(){Me._sb_Msg_Panel1})
		Me.sb_Msg.Items.AddRange(New System.Windows.Forms.ToolStripItem(){Me._sb_Msg_Panel2})
		Me.sb_Msg.Items.AddRange(New System.Windows.Forms.ToolStripItem(){Me._sb_Msg_Panel3})
		Me.cbFunc.SetIndex(_cbFunc_6, CType(6, Short))
		Me.cbFunc.SetIndex(_cbFunc_7, CType(7, Short))
		Me.cbFunc.SetIndex(_cbFunc_5, CType(5, Short))
		Me.cbFunc.SetIndex(_cbFunc_4, CType(4, Short))
		Me.cbFunc.SetIndex(_cbFunc_3, CType(3, Short))
		Me.cbFunc.SetIndex(_cbFunc_2, CType(2, Short))
		Me.cbFunc.SetIndex(_cbFunc_1, CType(1, Short))
		Me.cbFunc.SetIndex(_cbFunc_8, CType(8, Short))
		Me.cbFunc.SetIndex(_cbFunc_9, CType(9, Short))
		Me.cbFunc.SetIndex(_cbFunc_12, CType(12, Short))
		Me.cbFunc.SetIndex(_cbFunc_11, CType(11, Short))
		Me.cbFunc.SetIndex(_cbFunc_10, CType(10, Short))
		Me.lb_Func.SetIndex(_lb_Func_12, CType(12, Short))
		Me.lb_Func.SetIndex(_lb_Func_11, CType(11, Short))
		Me.lb_Func.SetIndex(_lb_Func_5, CType(5, Short))
		Me.lb_Func.SetIndex(_lb_Func_3, CType(3, Short))
		Me.lb_Func.SetIndex(_lb_Func_2, CType(2, Short))
		Me.lb_Func.SetIndex(_lb_Func_4, CType(4, Short))
		Me.lb_Func.SetIndex(_lb_Func_1, CType(1, Short))
		Me.lb_Func.SetIndex(_lb_Func_6, CType(6, Short))
		Me.lb_Func.SetIndex(_lb_Func_7, CType(7, Short))
		Me.lb_Func.SetIndex(_lb_Func_8, CType(8, Short))
		Me.lb_Func.SetIndex(_lb_Func_9, CType(9, Short))
		Me.lb_Func.SetIndex(_lb_Func_10, CType(10, Short))
		Me.lb_kara.SetIndex(_lb_kara_0, CType(0, Short))
		Me.lb_コンテナ.SetIndex(_lb_コンテナ_1, CType(1, Short))
		Me.lb_月.SetIndex(_lb_月_7, CType(7, Short))
		Me.lb_月.SetIndex(_lb_月_6, CType(6, Short))
		Me.lb_月.SetIndex(_lb_月_5, CType(5, Short))
		Me.lb_月.SetIndex(_lb_月_4, CType(4, Short))
		Me.lb_月.SetIndex(_lb_月_3, CType(3, Short))
		Me.lb_月.SetIndex(_lb_月_1, CType(1, Short))
		Me.lb_月.SetIndex(_lb_月_2, CType(2, Short))
		Me.lb_月.SetIndex(_lb_月_0, CType(0, Short))
		Me.lb_項目.SetIndex(_lb_項目_15, CType(15, Short))
		Me.lb_項目.SetIndex(_lb_項目_14, CType(14, Short))
		Me.lb_項目.SetIndex(_lb_項目_13, CType(13, Short))
		Me.lb_項目.SetIndex(_lb_項目_12, CType(12, Short))
		Me.lb_項目.SetIndex(_lb_項目_11, CType(11, Short))
		Me.lb_項目.SetIndex(_lb_項目_10, CType(10, Short))
		Me.lb_項目.SetIndex(_lb_項目_9, CType(9, Short))
		Me.lb_項目.SetIndex(_lb_項目_7, CType(7, Short))
		Me.lb_項目.SetIndex(_lb_項目_8, CType(8, Short))
		Me.lb_項目.SetIndex(_lb_項目_5, CType(5, Short))
		Me.lb_項目.SetIndex(_lb_項目_0, CType(0, Short))
		Me.lb_項目.SetIndex(_lb_項目_6, CType(6, Short))
		Me.lb_項目.SetIndex(_lb_項目_2, CType(2, Short))
		Me.lb_項目.SetIndex(_lb_項目_3, CType(3, Short))
		Me.lb_項目.SetIndex(_lb_項目_4, CType(4, Short))
		Me.lb_項目.SetIndex(_lb_項目_1, CType(1, Short))
		Me.lb_日.SetIndex(_lb_日_8, CType(8, Short))
		Me.lb_日.SetIndex(_lb_日_7, CType(7, Short))
		Me.lb_日.SetIndex(_lb_日_6, CType(6, Short))
		Me.lb_日.SetIndex(_lb_日_5, CType(5, Short))
		Me.lb_日.SetIndex(_lb_日_4, CType(4, Short))
		Me.lb_日.SetIndex(_lb_日_3, CType(3, Short))
		Me.lb_日.SetIndex(_lb_日_2, CType(2, Short))
		Me.lb_日.SetIndex(_lb_日_0, CType(0, Short))
		Me.lb_日.SetIndex(_lb_日_1, CType(1, Short))
		Me.lb_年.SetIndex(_lb_年_7, CType(7, Short))
		Me.lb_年.SetIndex(_lb_年_6, CType(6, Short))
		Me.lb_年.SetIndex(_lb_年_5, CType(5, Short))
		Me.lb_年.SetIndex(_lb_年_4, CType(4, Short))
		Me.lb_年.SetIndex(_lb_年_3, CType(3, Short))
		Me.lb_年.SetIndex(_lb_年_2, CType(2, Short))
		Me.lb_年.SetIndex(_lb_年_0, CType(0, Short))
		Me.lb_年.SetIndex(_lb_年_1, CType(1, Short))
		Me.lb_納期.SetIndex(_lb_納期_0, CType(0, Short))
		Me.lb_納期.SetIndex(_lb_納期_1, CType(1, Short))
		Me.lblLabels.SetIndex(_lblLabels_58, CType(58, Short))
		Me.lblLabels.SetIndex(_lblLabels_57, CType(57, Short))
		Me.lblLabels.SetIndex(_lblLabels_56, CType(56, Short))
		Me.lblLabels.SetIndex(_lblLabels_55, CType(55, Short))
		Me.lblLabels.SetIndex(_lblLabels_54, CType(54, Short))
		Me.lblLabels.SetIndex(_lblLabels_53, CType(53, Short))
		Me.lblLabels.SetIndex(_lblLabels_52, CType(52, Short))
		Me.lblLabels.SetIndex(_lblLabels_51, CType(51, Short))
		Me.lblLabels.SetIndex(_lblLabels_50, CType(50, Short))
		Me.lblLabels.SetIndex(_lblLabels_49, CType(49, Short))
		Me.lblLabels.SetIndex(_lblLabels_48, CType(48, Short))
		Me.lblLabels.SetIndex(_lblLabels_47, CType(47, Short))
		Me.lblLabels.SetIndex(_lblLabels_1, CType(1, Short))
		Me.lblLabels.SetIndex(_lblLabels_46, CType(46, Short))
		Me.lblLabels.SetIndex(_lblLabels_45, CType(45, Short))
		Me.lblLabels.SetIndex(_lblLabels_44, CType(44, Short))
		Me.lblLabels.SetIndex(_lblLabels_43, CType(43, Short))
		Me.lblLabels.SetIndex(_lblLabels_42, CType(42, Short))
		Me.lblLabels.SetIndex(_lblLabels_41, CType(41, Short))
		Me.lblLabels.SetIndex(_lblLabels_40, CType(40, Short))
		Me.lblLabels.SetIndex(_lblLabels_39, CType(39, Short))
		Me.lblLabels.SetIndex(_lblLabels_36, CType(36, Short))
		Me.lblLabels.SetIndex(_lblLabels_35, CType(35, Short))
		Me.lblLabels.SetIndex(_lblLabels_38, CType(38, Short))
		Me.lblLabels.SetIndex(_lblLabels_37, CType(37, Short))
		Me.lblLabels.SetIndex(_lblLabels_31, CType(31, Short))
		Me.lblLabels.SetIndex(_lblLabels_30, CType(30, Short))
		Me.lblLabels.SetIndex(_lblLabels_29, CType(29, Short))
		Me.lblLabels.SetIndex(_lblLabels_28, CType(28, Short))
		Me.lblLabels.SetIndex(_lblLabels_27, CType(27, Short))
		Me.lblLabels.SetIndex(_lblLabels_25, CType(25, Short))
		Me.lblLabels.SetIndex(_lblLabels_15, CType(15, Short))
		Me.lblLabels.SetIndex(_lblLabels_9, CType(9, Short))
		Me.lblLabels.SetIndex(_lblLabels_26, CType(26, Short))
		Me.lblLabels.SetIndex(_lblLabels_24, CType(24, Short))
		Me.lblLabels.SetIndex(_lblLabels_23, CType(23, Short))
		Me.lblLabels.SetIndex(_lblLabels_22, CType(22, Short))
		Me.lblLabels.SetIndex(_lblLabels_21, CType(21, Short))
		Me.lblLabels.SetIndex(_lblLabels_20, CType(20, Short))
		Me.lblLabels.SetIndex(_lblLabels_19, CType(19, Short))
		Me.lblLabels.SetIndex(_lblLabels_18, CType(18, Short))
		Me.lblLabels.SetIndex(_lblLabels_17, CType(17, Short))
		Me.lblLabels.SetIndex(_lblLabels_16, CType(16, Short))
		Me.lblLabels.SetIndex(_lblLabels_14, CType(14, Short))
		Me.lblLabels.SetIndex(_lblLabels_13, CType(13, Short))
		Me.lblLabels.SetIndex(_lblLabels_12, CType(12, Short))
		Me.lblLabels.SetIndex(_lblLabels_11, CType(11, Short))
		Me.lblLabels.SetIndex(_lblLabels_8, CType(8, Short))
		Me.lblLabels.SetIndex(_lblLabels_7, CType(7, Short))
		Me.lblLabels.SetIndex(_lblLabels_6, CType(6, Short))
		Me.lblLabels.SetIndex(_lblLabels_5, CType(5, Short))
		Me.lblLabels.SetIndex(_lblLabels_4, CType(4, Short))
		Me.lblLabels.SetIndex(_lblLabels_3, CType(3, Short))
		Me.lblLabels.SetIndex(_lblLabels_2, CType(2, Short))
		Me.lblLabels.SetIndex(_lblLabels_0, CType(0, Short))
		Me.lblLabels.SetIndex(_lblLabels_10, CType(10, Short))
		Me.lblLabels.SetIndex(_lblLabels_34, CType(34, Short))
		Me.lblLabels.SetIndex(_lblLabels_33, CType(33, Short))
		Me.lblLabels.SetIndex(_lblLabels_32, CType(32, Short))
		Me.ln_over.SetIndex(_ln_over_1, CType(1, Short))
		Me.ln_over.SetIndex(_ln_over_0, CType(0, Short))
		Me.Shape1.SetIndex(_Shape1_13, CType(13, Short))
		Me.Shape1.SetIndex(_Shape1_12, CType(12, Short))
		Me.Shape1.SetIndex(_Shape1_11, CType(11, Short))
		Me.Shape1.SetIndex(_Shape1_10, CType(10, Short))
		Me.Shape1.SetIndex(_Shape1_9, CType(9, Short))
		Me.Shape1.SetIndex(_Shape1_7, CType(7, Short))
		Me.Shape1.SetIndex(_Shape1_8, CType(8, Short))
		Me.Shape1.SetIndex(_Shape1_6, CType(6, Short))
		Me.Shape1.SetIndex(_Shape1_5, CType(5, Short))
		Me.Shape1.SetIndex(_Shape1_4, CType(4, Short))
		Me.Shape1.SetIndex(_Shape1_3, CType(3, Short))
		Me.Shape1.SetIndex(_Shape1_0, CType(0, Short))
		Me.Shape1.SetIndex(_Shape1_1, CType(1, Short))
		Me.Shape1.SetIndex(_Shape1_2, CType(2, Short))
		CType(Me.Shape1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.ln_over, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lblLabels, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lb_納期, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lb_年, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lb_日, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lb_項目, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lb_月, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lb_コンテナ, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lb_kara, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lb_Func, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.cbFunc, System.ComponentModel.ISupportInitialize).EndInit()
		Me.picFunction.ResumeLayout(False)
		Me.sb_Msg.ResumeLayout(False)
		Me.ResumeLayout(False)
		Me.PerformLayout()
	End Sub
#End Region 
End Class