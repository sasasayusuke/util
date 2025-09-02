Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.PowerPacks
Friend Class SnwMT02F14
	Inherits System.Windows.Forms.Form
	'
	'--------------------------------------------------------------------
	'  ユーザー名           株式会社三和商研
	'  業務名　　　　　　　　積算データ管理システム
	'  部門名               見積部門
	'  プログラム名         員数シート取込画面（ＥＸＣＥＬ）
	'  作成会社             テクノウェア株式会社
	'  作成日               2015/11/09
	'  作成者               oosawa
	'--------------------------------------------------------------------
	'   UPDATE
	'       2016/08/03  oosawa      仕様NOに無条件でSを入れる
	'       2017/08/04  oosawa      しまむら新EDI用レイアウトに変更
	'                               　（進捗確認データ）
	'       2017/10/05  oosawa      取込時つぶさないように修正
	'--------------------------------------------------------------------
	Const ProfileKey As String = "しまむら消耗品取込"
	Dim GXLSPath As String '出力パス
	
	Dim gRsA As ADODB.Recordset 'データテーブル
	
	
	Dim pParentForm As System.Windows.Forms.Form
	Dim pMituNo As Integer
	
	Dim wMituNo As Integer
	Dim wSiwaNo As Integer
	Dim wSiwaGyo As Short
	Dim wTempData As Object
	
	Dim pfpSpd As AxFPSpreadADO.AxfpSpread
	Dim pActRow As Integer
	
	'選択したコードを送るコントロールをセット
	WriteOnly Property ResParentForm() As System.Windows.Forms.Form
		Set(ByVal Value As System.Windows.Forms.Form)
			pParentForm = Value
		End Set
	End Property
	
	'表示項目をセット
	WriteOnly Property fpSpd() As AxFPSpreadADO.AxfpSpread
		Set(ByVal Value As AxFPSpreadADO.AxfpSpread)
			pfpSpd = Value
		End Set
	End Property
	
	'行項目をセット
	WriteOnly Property ActRow() As Integer
		Set(ByVal Value As Integer)
			pActRow = Value
		End Set
	End Property
	
	Private Sub SnwMT02F14_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		'フォームを画面の中央に配置
		Me.Top = VB6.TwipsToPixelsY((VB6.PixelsToTwipsY(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height) - VB6.PixelsToTwipsY(Me.Height)) \ 2)
		Me.Left = VB6.TwipsToPixelsX((VB6.PixelsToTwipsX(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width) - VB6.PixelsToTwipsX(Me.Width)) \ 2)
		
		'''''    rf_見積番号 = Format$(pMituNo, "#")
		
		'入力先情報セット
		'UPGRADE_WARNING: オブジェクト GetIni() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		GXLSPath = GetIni("Path", ProfileKey, INIFile)
		'UPGRADE_WARNING: TextBox プロパティ txDir.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
		[txDir].Text = CompactPathEx(GXLSPath, [txDir].Maxlength)
	End Sub
	
	Private Sub SnwMT02F14_FormClosing(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
		Dim Cancel As Boolean = eventArgs.Cancel
		Dim UnloadMode As System.Windows.Forms.CloseReason = eventArgs.CloseReason
		''    ReleaseRs rs(0)
		''    ReleaseRs rs(1)
		pParentForm.Enabled = True
		pParentForm.Activate()
		'UPGRADE_NOTE: オブジェクト pParentForm をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		pParentForm = Nothing
		
		ReleaseRs(gRsA)
		eventArgs.Cancel = Cancel
	End Sub
	
	Private Sub cb中止_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cb中止.Click
		Me.Close()
	End Sub
	''''''''
	''''''''Private Sub cbXLS_Click()
	''''''''    Dim RecCnt As Long
	''''''''    Dim wMsg As String
	''''''''    Dim st As Long
	''''''''
	''''''''    If [txDir].Text = vbNullString Then
	''''''''        CriticalAlarm "ファイルを指定して下さい。"
	''''''''        [cb変更].SetFocus
	''''''''        Exit Sub
	''''''''    End If
	''''''''
	''''''''
	''''''''
	''''''''    If vbNo = YesNo("指定のファイルを員数シートに取込みます。") Then Exit Sub
	''''''''
	''''''''    RecCnt = CSV取込(GXLSPath)
	''''''''    Select Case RecCnt
	''''''''        Case -1
	''''''''            CriticalAlarm "該当データ無し"
	''''''''        Case Else
	''''''''            rf_取込行数.Caption = RecCnt
	''''''''            If RecCnt + pActRow > 2000 Then
	''''''''                wMsg = vbCrLf & "※全ての行を取り込めません。" & vbCrLf & vbCrLf
	''''''''            Else
	''''''''                wMsg = ""
	''''''''            End If
	''''''''            If vbYes = Question(pActRow & "行目に指定の員数シートを取込みます。" & wMsg & "適用しますか？" & vbCrLf & "", vbYes) Then
	''''''''                'csvファイル取込
	''''''''                Select Case UpdateSheet()
	''''''''                    Case 0
	''''''''                        '変換OK
	'''''''''                    Case -2
	''''''''''                        Update = -2
	'''''''''                        Exit Function
	''''''''                    Case Else
	''''''''                        '以外のエラー
	''''''''                        CriticalAlarm "取込処理が失敗しました。"
	'''''''''                        Update = -1
	''''''''                        Exit Sub
	''''''''                End Select
	''''''''
	''''''''                Set gRsA = Nothing
	''''''''                Unload Me
	''''''''                Exit Sub
	''''''''            End If
	''''''''    End Select
	''''''''End Sub
	
	'2017/08/04 ADD↓
	Private Sub cbNewImport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cbNewImport.Click
		Dim RecCnt As Integer
		Dim wMsg As String
		Dim st As Integer
		
		If [txDir].Text = vbNullString Then
			CriticalAlarm("ファイルを指定して下さい。")
			[cb変更].Focus()
			Exit Sub
		End If
		
		
		
		If MsgBoxResult.No = YesNo("指定のファイルを員数シートに取込みます。") Then Exit Sub
		
		RecCnt = CSV取込New(GXLSPath)
		Select Case RecCnt
			Case -1
				CriticalAlarm("該当データ無し")
			Case Else
				rf_取込行数.Text = CStr(RecCnt)
				If RecCnt + pActRow > 2000 Then
					wMsg = vbCrLf & "※全ての行を取り込めません。" & vbCrLf & vbCrLf
				Else
					wMsg = ""
				End If
				If MsgBoxResult.Yes = Question(pActRow & "行目に指定の員数シートを取込みます。" & wMsg & "適用しますか？" & vbCrLf & "", MsgBoxResult.Yes) Then
					'csvファイル取込
					Select Case UpdateSheet()
						Case 0
							'変換OK
							'                    Case -2
							''                        Update = -2
							'                        Exit Function
						Case Else
							'以外のエラー
							CriticalAlarm("取込処理が失敗しました。")
							'                        Update = -1
							Exit Sub
					End Select
					
					'UPGRADE_NOTE: オブジェクト gRsA をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
					gRsA = Nothing
					Me.Close()
					Exit Sub
				End If
		End Select
	End Sub
	'2017/08/04 ADD↑
	
	'2017/08/04 ADD↓
	Private Function CSV取込New(ByRef FilePath As String) As Integer
		
		'保存用テーブル作成
		gRsA = CreateTable
		
		On Error GoTo Update_Err
		
		'ＣＳＶを内部レコードセットに変換
		If ImportFixdTextToRsNew(FilePath, gRsA) = False Then
			CSV取込New = -1
			Exit Function
		End If
		
		With gRsA
			If .EOF Then
				CSV取込New = -1
			Else
				CSV取込New = gRsA.RecordCount
			End If
		End With
		
		
Update_Correct: 
		Call HourGlass(False)
		On Error GoTo 0
		Exit Function
Update_Err: '---エラー時
		CSV取込New = -1
		CheckAlarm(Err.Number & vbCrLf & Err.Description)
		Resume Update_Correct
		
	End Function
	'2017/08/04 ADD↑
	
	'201708/04 ADD↓
	Private Function ImportFixdTextToRsNew(ByRef FilePath As String, ByRef rs As ADODB.Recordset) As Boolean
		Dim cCsvReader As clsCSVReader
		cCsvReader = New clsCSVReader
		
		' 指定した CSV ファイルを開く
		If cCsvReader.OpenStream(FilePath) = False Then
			CriticalAlarm(("指定したファイルが存在しません。"))
			Exit Function
		End If
		
		' 最初の行をヘッダとして読み込む
		Call cCsvReader.ReadHeader()
		
		' CSV ファイルの中身をすべて取得する
		Dim cTable As Collection
		cTable = cCsvReader.ReadToEnd()
		
		'-------------------------
		'いらないデータの削除
		'-------------------------
		Dim i As Integer
		For i = cTable.Count() To 1 Step -1
			'UPGRADE_WARNING: オブジェクト cTable(i)(1) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If cTable.Item(i)(1) <> Chr(&H1A) And cTable.Item(i)(1) <> "" Then
			Else
				'EOFは削除
				cTable.Remove((i))
			End If
		Next 
		
		Dim NUMBR As Integer
		Dim Hdate As Date
		Dim Mcnt As Integer '明細行カウント
		Dim errKBN As Short
		
		Mcnt = 0
		
		For i = 1 To cTable.Count()
			With rs
				'UPGRADE_WARNING: オブジェクト cTable(i)(16) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				If cTable.Item(i)(16) <> "削" Then
					''            [sb_Msg].Panels(1).Text = cTable(i)(1) & cTable(i)(2) & cTable(i)(3) & "," & cTable(i)(6) & "," & cTable(i)(11) & "," & cTable(i)(5) & "," & cTable(i)(25)
					'UPGRADE_WARNING: オブジェクト cTable(i)(17) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					If cTable.Item(i)(17) = "済" Then '出荷済み
						.AddNew()
						
						'UPGRADE_WARNING: オブジェクト cTable()() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Fields("伝票番号").Value = cTable.Item(i)(3)
						'UPGRADE_WARNING: オブジェクト cTable()() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Fields("納品日").Value = OutputDateYMDFromFlatText(CStr(cTable.Item(i)(5)))
						'UPGRADE_WARNING: オブジェクト cTable()() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Fields("単品コード").Value = cTable.Item(i)(8)
						'変換しなくちゃならない製品NO
						'複数あるので変換できない！！
						'                .Fields("製品NO") = cTable(i)(7)
						'                .Fields("仕様NO") = cTable(i)(7)
						'                .Fields("品名") = cTable(i)(7)
						'                .Fields("単位名") = cTable(i)(7)
						'
						'UPGRADE_WARNING: オブジェクト cTable()() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Fields("数量").Value = cTable.Item(i)(11) '納品数
						'UPGRADE_WARNING: オブジェクト cTable()() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Fields("削除").Value = cTable.Item(i)(16)
						
						.Update()
					End If
				End If
			End With
			
		Next 
		
		ImportFixdTextToRsNew = True
	End Function
	'201708/04 ADD↑
	
	Private Sub cb変更_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cb変更.Click
		If OpenFileDialog(GXLSPath, ProfileKey, "CSV") = True Then
			WriteIni("Path", ProfileKey, GXLSPath, INIFile)
		End If
		'UPGRADE_WARNING: TextBox プロパティ txDir.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
		txDir.Text = CompactPathEx(GXLSPath, [txDir].Maxlength)
		
		rf_取込行数.Text = vbNullString
	End Sub
	''''
	''''Private Function CSV取込(filePath As String) As Long
	''''
	''''    '保存用テーブル作成
	''''    Set gRsA = CreateTable
	''''
	''''    On Error GoTo Update_Err
	''''
	''''    'ＣＳＶを内部レコードセットに変換
	''''    If ImportFixdTextToRs(filePath, gRsA) = False Then
	''''        CSV取込 = -1
	''''        Exit Function
	''''    End If
	''''
	''''    With gRsA
	''''        If .EOF Then
	''''            CSV取込 = -1
	''''        Else
	''''            CSV取込 = gRsA.RecordCount
	''''        End If
	''''    End With
	''''
	''''
	''''Update_Correct:
	''''    Call HourGlass(False)
	''''    On Error GoTo 0
	''''    Exit Function
	''''Update_Err: '---エラー時
	''''    CSV取込 = -1
	''''    CheckAlarm Err.Number & vbCrLf & Err.Description
	''''    Resume Update_Correct
	''''
	''''End Function
	
	Private Function CreateTable() As ADODB.Recordset
		Dim rs As ADODB.Recordset
		
		On Error GoTo CreateTable_Err
		
		If Not (CreateTable Is Nothing) Then
			CreateTable.Close()
			'UPGRADE_NOTE: オブジェクト CreateTable をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
			CreateTable = Nothing
		End If
		'レコードセットを作成する
		rs = New ADODB.Recordset
		rs.CursorLocation = ADODB.CursorLocationEnum.adUseClient
		With rs
			.Fields.Append("伝票番号", ADODB.DataTypeEnum.adInteger)
			.Fields.Append("納品日", ADODB.DataTypeEnum.adDate)
			
			.Fields.Append("単品コード", ADODB.DataTypeEnum.adVarChar, 4)
			.Fields.Append("製品NO", ADODB.DataTypeEnum.adVarChar, 12)
			.Fields.Append("仕様NO", ADODB.DataTypeEnum.adVarChar, 10)
			.Fields.Append("品名", ADODB.DataTypeEnum.adVarChar, 40)
			.Fields.Append("単位名", ADODB.DataTypeEnum.adVarChar, 8)
			
			.Fields.Append("数量", ADODB.DataTypeEnum.adCurrency)
			.Fields.Append("削除", ADODB.DataTypeEnum.adVarChar, 2)
			
			.CursorType = ADODB.CursorTypeEnum.adOpenStatic
			.LockType = ADODB.LockTypeEnum.adLockOptimistic
			
			.Open()
		End With
		
		If Not CreateTable Is Nothing Then
			CreateTable.Close()
			'UPGRADE_NOTE: オブジェクト CreateTable をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
			CreateTable = Nothing
		End If
		
		CreateTable = rs
		Exit Function
		
CreateTable_Err: 
		MsgBox(Err.Number & " " & Err.Description)
		Call HourGlass(False)
	End Function
	''''''
	''''''Private Function ImportFixdTextToRs(ByRef filePath As String, ByRef rs As Recordset) As Boolean
	''''''    Dim cCsvReader As clsCSVReader
	''''''    Set cCsvReader = New clsCSVReader
	''''''
	''''''    ' 指定した CSV ファイルを開く
	''''''    If cCsvReader.OpenStream(filePath) = False Then
	''''''        CriticalAlarm ("指定したファイルが存在しません。")
	''''''        Exit Function
	''''''    End If
	''''''
	''''''    ' 最初の行をヘッダとして読み込む
	''''''    Call cCsvReader.ReadHeader
	''''''
	''''''    ' CSV ファイルの中身をすべて取得する
	''''''    Dim cTable As Collection
	''''''    Set cTable = cCsvReader.ReadToEnd()
	''''''
	''''''    '-------------------------
	''''''    'いらないデータの削除
	''''''    '-------------------------
	''''''    Dim i As Long
	''''''    For i = cTable.Count To 1 Step -1
	''''''        If cTable(i)(1) <> Chr(&H1A) And cTable(i)(1) <> "" Then
	''''''        Else
	''''''            'EOFは削除
	''''''            cTable.Remove (i)
	''''''        End If
	''''''    Next
	''''''
	''''''    Dim NUMBR As Long
	''''''    Dim Hdate As Date
	''''''    Dim Mcnt As Long    '明細行カウント
	''''''    Dim errKBN As Integer
	''''''
	''''''    Mcnt = 0
	''''''
	''''''    For i = 1 To cTable.Count
	''''''        With rs
	''''''            If cTable(i)(14) <> "削" Then
	''''''''            [sb_Msg].Panels(1).Text = cTable(i)(1) & cTable(i)(2) & cTable(i)(3) & "," & cTable(i)(6) & "," & cTable(i)(11) & "," & cTable(i)(5) & "," & cTable(i)(25)
	''''''
	''''''                .AddNew
	''''''
	''''''                .Fields("伝票番号") = cTable(i)(3)
	''''''                .Fields("納品日") = OutputDateYMDFromFlatText(CStr(cTable(i)(5)))
	''''''                .Fields("単品コード") = cTable(i)(7)
	''''''                '変換しなくちゃならない製品NO
	''''''                '複数あるので変換できない！！
	'''''''                .Fields("製品NO") = cTable(i)(7)
	'''''''                .Fields("仕様NO") = cTable(i)(7)
	'''''''                .Fields("品名") = cTable(i)(7)
	'''''''                .Fields("単位名") = cTable(i)(7)
	'''''''
	''''''                .Fields("数量") = cTable(i)(10)
	''''''                .Fields("削除") = cTable(i)(14)
	''''''
	''''''                .Update
	''''''            End If
	''''''        End With
	''''''
	''''''    Next
	''''''
	''''''    ImportFixdTextToRs = True
	''''''End Function
	
	Private Function UpdateSheet() As Short
		Dim sql As String
		Dim rs As ADODB.Recordset
		
		'2017/10/05 ADD↓
		'ｽﾌﾟﾚｯﾄﾞのクラス
		'UPGRADE_NOTE: clsSPD は clsSPD_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
		Dim clsSPD_Renamed As clsSPD
		
		'SPREAD設定
		clsSPD_Renamed = New clsSPD
		clsSPD_Renamed.CtlSpd = pfpSpd
		'2017/10/05 ADD↑
		
		Dim NUMBR As Integer
		Dim Hdate As Date
		Dim Mcnt As Integer '明細行カウント
		
		'2016/08/03 ADD↓
		Dim HaisoName As String
		Dim cHaisosaki As clsHaisosaki
		cHaisosaki = New clsHaisosaki
		
		HaisoName = ""
		'配送先名取得("01")
		''    cHaisosaki.配送先CD = "01"
		cHaisosaki.配送先CD = "03" '2017/01/06 ADD
		If cHaisosaki.GetbyID = True Then
			HaisoName = cHaisosaki.略称
		End If
		'UPGRADE_NOTE: オブジェクト cHaisosaki をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		cHaisosaki = Nothing
		'2016/08/03 ADD↑
		
		'2017/01/06 ADD↓
		Dim SirName As String
		Dim cSiiresaki As clsSiiresaki
		cSiiresaki = New clsSiiresaki
		
		SirName = ""
		'仕入先名取得("3150")
		cSiiresaki.仕入先CD = "3150"
		If cSiiresaki.GetbyID = True Then
			SirName = cSiiresaki.略称
		End If
		'UPGRADE_NOTE: オブジェクト cSiiresaki をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		cSiiresaki = Nothing
		
		Mcnt = 0
		
		'正常分
		'    gRsA.Filter = "削除 <> '削'"
		gRsA.Sort = "納品日,伝票番号"
		
		Do Until gRsA.EOF
			'2017/10/05 ADD↓
			If clsSPD_Renamed.GetTextEX(Col見積区分, pActRow + Mcnt) = "" And clsSPD_Renamed.GetTextEX(Col製品NO, pActRow + Mcnt) = "" And clsSPD_Renamed.GetTextEX(Col仕様NO, pActRow + Mcnt) = "" And clsSPD_Renamed.GetTextEX(Col名称, pActRow + Mcnt) = "" Then
				'見積区分・製品NO・仕様NO・名称が空白の場合、行に書き込む
				'空白以外は1行下げて確認する。
				'2017/10/05 ADD↑
				
				
				'明細部作成
				With pfpSpd
					.SetText(Col他社納品日付, pActRow + Mcnt, gRsA.Fields("納品日"))
					.SetText(Col他社伝票番号, pActRow + Mcnt, gRsA.Fields("伝票番号"))
					.SetText(Col製品NO, pActRow + Mcnt, gRsA.Fields("単品コード"))
					
					.SetText(Col仕様NO, pActRow + Mcnt, "S") '2016/08/03 ADD
					
					'クリアする 2016/08/03 ADD
					.SetText(Col名称, pActRow + Mcnt, "")
					.SetText(ColW, pActRow + Mcnt, "")
					.SetText(ColD, pActRow + Mcnt, "")
					.SetText(ColH, pActRow + Mcnt, "")
					.SetText(ColD1, pActRow + Mcnt, "")
					.SetText(ColD2, pActRow + Mcnt, "")
					.SetText(ColH1, pActRow + Mcnt, "")
					.SetText(ColH2, pActRow + Mcnt, "")
					.SetText(Col原価, pActRow + Mcnt, "")
					.SetText(Col売価, pActRow + Mcnt, "")
					.SetText(Col単位, pActRow + Mcnt, "")
					'            .SetText Col仕入先CD, pActRow + Mcnt, ""
					'            .SetText Col仕入先名, pActRow + Mcnt, ""
					.SetText(Col仕入先CD, pActRow + Mcnt, "3150") '2017/01/06 ADD
					.SetText(Col仕入先名, pActRow + Mcnt, SirName) '2017/01/06 ADD
					''            .SetText Col送り先CD, pActRow + Mcnt, "01" '直送 '2016/08/03 ADD
					''            .SetText Col送り先名, pActRow + Mcnt, HaisoName '直送 '2016/08/03 ADD
					.SetText(Col送り先CD, pActRow + Mcnt, "03") '三和納品 '2017/01/06 ADD
					.SetText(Col送り先名, pActRow + Mcnt, HaisoName) '2017/01/06 ADD
					
					''            .SetText Col社内在庫, pActRow + Mcnt, gRsA![数量]
					.SetText(Col仕分数1, pActRow + Mcnt, gRsA.Fields("数量"))
					
				End With
				
				gRsA.MoveNext()
			End If
			'明細行カウントアップ
			Mcnt = Mcnt + 1
		Loop 
		
		'UPGRADE_NOTE: オブジェクト clsSPD_Renamed をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		clsSPD_Renamed = Nothing
	End Function
End Class