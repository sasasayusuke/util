Option Strict Off
Option Explicit On
Module SnwMT01B01
	
	'ヘッダー部保存用変数
	Public HD_見積番号 As Object
	Public HD_担当者CD As Short
	Public HD_見積日付 As Object
	Public HD_見積件名 As String
	Public HD_得意先CD As String
	Public HD_得意先名1 As String
	Public HD_得意先名2 As String
	Public HD_得TEL As String
	Public HD_得FAX As String
	Public HD_得担当者 As String
	Public HD_納得意先CD As String
	Public HD_納入先CD As String
	Public HD_納入先名1 As String
	Public HD_納入先名2 As String
	Public HD_納郵便番号 As String
	Public HD_納住所1 As String
	Public HD_納住所2 As String
	Public HD_納TEL As String
	Public HD_納FAX As String
	Public HD_納担当者 As String
	''Public HD_納期S            As Date
	''Public HD_納期E            As Date
	Public HD_納期S As Object
	Public HD_納期E As Object
	Public HD_備考 As String
	Public HD_規模金額 As Decimal
	''Public HD_OPEN日           As Date
	Public HD_OPEN日 As Object
	Public HD_物件種別 As Short
	Public HD_現場名 As String
	Public HD_支払条件 As Short
	Public HD_支払条件他 As String
	Public HD_納期表示 As Short
	Public HD_納期表示他 As String
	Public HD_見積日出力 As Short
	Public HD_有効期限 As Short
	Public HD_受注区分 As Short
	Public HD_受注日付 As Object
	Public HD_大小口区分 As Short
	Public HD_出精値引 As Decimal
	'修正チェック用
	Public HD_売上日付 As Object
	Public HD_仕入日付 As Object
	
	Public HD_合計金額 As Decimal
	Public HD_原価合計 As Decimal
	Public HD_原価率 As Decimal
	Public HD_外税額 As Decimal
	
	Public HD_税集計区分 As Short
	Public HD_売上端数 As Short
	Public HD_消費税端数 As Short
	
	Public HD_得意先別見積番号 As Object '2005/07/04 ADD
	
	'''Public HD_見積区分          As Integer                  '2009/09/24 ADD
	'''Public HH_見積区分          As Integer                  '2013/04/09 ADD
	Public HH_受注区分 As Short '2013/08/08 ADD
	'仕分レベル保存用変数
	'(ZEROオリジン)
	''''''Public gSiwakeTBL() As Variant
	Public gSiwakeTBL As Object
	
	Public Const SPMaxRow As Short = 2000
	
	'2008/01/23 ADD↓
	Public gGenRituMax As Decimal '原価率上限値
	Public gGenRituMin As Decimal '原価率下限値
	'2008/01/23 ADD↑
	
	
	'2013/03/08 ADD↓
	Public HD_部署CD As Short
	
	'''Public HD_営業推進部CD      As Integer '2019/12/12 ADD
	Public HD_工事担当CD As Short '2022/08/08 ADD
	
	Public HD_販売先得意先CD As String
	Public HD_販売先得意先名1 As String
	Public HD_販売先得意先名2 As String
	Public HD_販売先納得意先CD As String
	Public HD_販売先納入先CD As String
	Public HD_販売先納入先名1 As String
	Public HD_販売先納入先名2 As String
	'2013/03/08 ADD↑
	
	'2013/07/19 ADD↓
	Public HD_社内伝票扱い As Short
	'2013/07/19 ADD↑
	
	Public HD_伝票種類 As Short '2015/02/04 ADD
	Public HD_SM内容区分 As Short '2015/11/19 ADD
	
	'2015/06/12 ADD↓
	Public HD_ウエルシア物件内容CD As Integer
	Public HD_ウエルシア物件内容名 As String
	Public HD_ウエルシアリース区分 As Integer
	Public HD_ウエルシア物件区分CD As Integer
	Public HD_ウエルシア売場面積 As Decimal
	'2015/06/12 ADD↑
	
	Public HD_物件番号 As Object '2015/07/10 ADD
	Public HD_統合見積番号 As Object '2024/01/16 ADD
	
	'2015/10/16 ADD↓
	Public HD_受付日付 As Object
	Public HD_完工日付 As Object
	Public HD_発注担当者名 As String
	Public HD_作業内容 As String
	Public HD_YKサプライ区分 As Integer
	Public HD_YK物件区分 As Integer
	Public HD_YK請求区分 As Integer
	'2015/10/16 ADD↑
	'2015/11/03 ADD↓
	Public HD_化粧品メーカー区分 As Integer
	'2015/11/03 ADD↑
	
	Public HD_クレーム区分 As Integer '2016/04/09 ADD
	
	'2020/04/11 ADD↓
	Public HD_見積確定区分 As Integer
	
	Public HD_完了日付 As Object
	Public HD_完了者名 As String
	Public HD_入力USERID As String
	Public HD_請求予定日 As Object
	Public HD_経過備考1 As String
	Public HD_経過備考2 As String
	'2020/04/11 ADD↑
	Public HD_集計CD As String '2021/02/25 ADD
	
	'2022/10/10 ADD↓
	Public HD_B請求管轄区分 As Integer
	Public HD_BtoB番号 As Integer
	'2022/10/10 ADD↑
	Public HD_業種区分 As Integer '2023/04/18 ADD
	
	'2015/09/29 ADD↓privateから移動
	'ｽﾌﾟﾚｯﾄﾞｼｰﾄの定数
	Public Const Col展開 As Short = 1
	Public Const Col見積区分 As Short = 2
	
	Public Const Colクレーム区分 As Short = 3
	
	Public Const Col他社納品日付 As Short = 4
	Public Const Col他社伝票番号 As Short = 5
	Public Const ColSP区分 As Short = 6
	Public Const ColPC区分 As Short = 7
	
	Public Const Col追番R As Short = 8 '2019/12/12 ADD
	Public Const Col製品NO As Short = 9
	Public Const Col仕様NO As Short = 10
	Public Const Colベース色 As Short = 11
	Public Const Col名称 As Short = 12
	Public Const ColW As Short = 13
	Public Const ColD As Short = 14
	Public Const ColH As Short = 15
	Public Const ColD1 As Short = 16
	Public Const ColD2 As Short = 17
	Public Const ColH1 As Short = 18
	Public Const ColH2 As Short = 19
	Public Const Col明細備考 As Short = 20 '2018/05/03 ADD
	Public Const Colエラー内容 As Short = 21
	Public Const Col定価 As Short = 22
	Public Const ColU区分 As Short = 23 '2005/10/14.ADD  以降の項目を１プラス
	Public Const Col原価 As Short = 24
	Public Const Col仕入率 As Short = 25
	Public Const Col売価 As Short = 26
	Public Const Col売上率 As Short = 27
	Public Const ColM As Short = 28
	Public Const Col見積数量 As Short = 29
	Public Const Col単位 As Short = 30
	Public Const Col金額 As Short = 31
	Public Const Col仕入金額 As Short = 32
	Public Const Col売上税区分 As Short = 33
	Public Const Col消費税額 As Short = 34
	'2017/03/10 ADD↓
	Public Const Col仕入業者CD As Short = 35
	Public Const Col仕入業者名 As Short = 36
	'2017/03/10 ADD↑
	Public Const Col仕入先CD As Short = 37
	Public Const Col仕入先名 As Short = 38
	
	Public Const Col送り先CD As Short = 39
	Public Const Col送り先名 As Short = 40
	
	Public Const Col作業区分CD As Short = 41
	Public Const Col作業区分名 As Short = 42
	
	Public Const Col入庫日 As Short = 43 '2018/05/03 ADD
	Public Const Col出庫日 As Short = 44 '2020/09/16 ADD
	Public Const Col社内在庫 As Short = 45
	Public Const Col社内在庫参照 As Short = 46
	Public Const Col客先在庫 As Short = 47
	Public Const Col客先在庫参照 As Short = 48
	Public Const Col転用 As Short = 49
	Public Const Col発注調整数 As Short = 50 '2014/09/09 ADD
	Public Const Col発注数 As Short = 51
	Public Const Col数量合計 As Short = 52
	'F03にも持つので修正時はそちらも修正！
	Public Const Col仕分数1 As Short = 53
	Public Const Col製品区分 As Short = 83
	Public Const Col見積明細連番 As Short = 84
	Public Const Col仕入済数 As Short = 85 '2014/09/09 ADD
	Public Const Col仕入済CNT As Short = 86 '2018/06/19 ADD
	Public Const Col売上済CNT As Short = 87 '2014/09/09 ADD
	
	'''public Const SiwakeCol As Integer = 33      '仕分数１のCol
	'''public Const TeikaCol As Integer = 14       '定価のCol
	'''public Const SiireCol As Integer = 26       '仕入先のCol
	
	
	
	
	'2004/11/29 ADD
	Public Function Chk締日To見積(ByRef No As Integer) As Boolean
		Dim rs As ADODB.Recordset
		Dim sql As String
		Dim UGetuDate As Date
		Dim KGetuDate As Date
		
		On Error GoTo Chk締日To見積_Err
		
		'2011/12/26 ADD↓
		If Cn Is Nothing Then
			If ApplicationInit = False Then
				'UPGRADE_NOTE: オブジェクト Cn をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
				Cn = Nothing
				Exit Function
			End If
		End If
		'2011/12/26 ADD↑
		
		'マウスポインターを砂時計にする
		HourGlass(True)
		
		Dim ShimeDate As Date
		
		'    ShimeDate = GetDates("月次更新日")
		'2013/09/30 ADD↓
		Dim cDates As clsDates
		cDates = New clsDates
		With cDates
			.DateID = "売掛月次更新日"
			.GetbyID()
			UGetuDate = .更新日付
		End With
		With cDates
			.DateID = "買掛月次更新日"
			.GetbyID()
			KGetuDate = .更新日付
		End With
		'UPGRADE_NOTE: オブジェクト cDates をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		cDates = Nothing
		'2013/09/30 ADD↑
		
		
		sql = "SELECT * FROM TD見積" & " WHERE 見積番号 = " & No
		
		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockReadOnly)
		With rs
			If .EOF Then
				ReleaseRs(rs)
				GoTo Chk締日To見積_Correct
			Else
				'''            If ![仕入日付] < ShimeDate Then
				'''                GoTo Chk締日To見積_Correct
				'''            ElseIf ![売上日付] < ShimeDate Then
				'''                GoTo Chk締日To見積_Correct
				'''            End If
				'2013/09/30 ADD↓
				''''            If ![仕入日付] < KGetuDate Then
				''''                GoTo Chk締日To見積_Correct
				''''            ElseIf ![売上日付] < UGetuDate Then
				''''                GoTo Chk締日To見積_Correct
				''''            End If
				'2013/09/30 ADD↑
				'2013/12/26 ADD↓
				If (.Fields("仕入日付").Value < KGetuDate) And (.Fields("売上日付").Value < UGetuDate) Then
					GoTo Chk締日To見積_Correct
				End If
				'2013/12/26 ADD↑
				
			End If
		End With
		
		Chk締日To見積 = True
Chk締日To見積_Correct: 
		ReleaseRs(rs)
		HourGlass(False)
		Exit Function
Chk締日To見積_Err: 
		MsgBox(Err.Number & " " & Err.Description)
		'UPGRADE_NOTE: オブジェクト Cn をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		Cn = Nothing '2011/12/26 ADD
		HourGlass(False)
	End Function
End Module