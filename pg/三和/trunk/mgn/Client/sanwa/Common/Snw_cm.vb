Option Strict Off
Option Explicit On
Module Snw_CM
	
	'INIﾌｧｲﾙ
	Public Structure INI_REC
		Dim CONNECT As String
	End Structure
	
	Public LoginSucceeded As Boolean 'Login
	
	
	'Public Const DBType As Integer = 0     'ACCESS
	Public Const DBType As Short = 1 'SQLSERVER
	
	''Const DBProvider As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source="
	''Const DBPath As String = "..\DataBase\Mercury_tbl.mdb"
	
	Const DBProvider As String = "Provider=SQLOLEDB.1;User ID=sa;Initial Catalog=SanwaSDB;Data Source=XC16_nt"
	Const DBPath As String = ""
	
	
	'--項目の桁数設定----------
	Public Const TokuIDLength As Short = 4 '仕入先の桁数
	'''Public Const SeiIDLength As Integer = 7         '製品の桁数
	Public Const SeiIDLength As Short = 7 '製品の桁数
	Public Const ShiyoIDLength As Short = 7 '仕様の桁数
	'Public Const SeiIDLength As Integer = 12         '製品の桁数
	'Public Const ShiyoIDLength As Integer = 10         '仕様の桁数
	
	'--レジストリの項目名の指定----------
	Public Const ProductName As String = "SanwaS"
	
	Public INI As INI_REC
	Public Cn As ADODB.Connection
	
	'国情報
	Public COUNTRY_CODE As String '2014/07/10 ADD
	Public KIN_HASU As Short '2014/07/10 ADD
	Public KIN_FMT As String '2014/07/10 ADD
	
	'''Public Function ApplicationInit() As Boolean
	Public Function ApplicationInit(Optional ByRef MultiMode As Boolean = False) As Boolean
		Dim strAppCaption As String
		Dim lngFirstTophWnd As Integer
		Dim lngFirstPophWnd As Integer
		Dim lngResult As Integer
		
		ApplicationInit = False
		
		''    If App.StartMode = vbSModeStandalone Then
		''        If App.PrevInstance = True Then
		'''            MsgBox "このアプリケーションは既に起動されています。"
		'''            Exit Function
		''            '多重起動を許さない。
		''            'アプリケーションのキャプションを得る。
		''            strAppCaption = App.Title
		''            '自アプリのキャプションを変更しておく。
		''            App.Title = strAppCaption & " "
		''            'トップレベルのウィンドウハンドルを得る。
		''            lngFirstTophWnd = FindWindow("ThunderRT6Main", strAppCaption)
		''            '直前にアクティブなウィンドウハンドルを得る。
		''            lngFirstPophWnd = GetLastActivePopup(lngFirstTophWnd)
		''            'ウィンドウをアクティブにする。
		''            lngResult = SetForegroundWindow(lngFirstPophWnd)
		''            '最小化されタスクバーに格納されてある場合があるので、ウィンドウを通常表示にさせる。
		''            lngResult = ShowWindow(lngFirstPophWnd, SW_RESTORE)
		''            Exit Function
		''        End If
		''    End If
		'2013/03/13 ADD↓
		'UPGRADE_ISSUE: 定数 vbSModeStandalone はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
		'UPGRADE_ISSUE: App プロパティ App.StartMode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"' をクリックしてください。
		If App.StartMode = vbSModeStandalone Then
			'UPGRADE_ISSUE: App プロパティ App.PrevInstance はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"' をクリックしてください。
			If App.PrevInstance = True Then
				If MultiMode = False Then
					'            MsgBox "このアプリケーションは既に起動されています。"
					'            Exit Function
					'多重起動を許さない。
					'アプリケーションのキャプションを得る。
					strAppCaption = My.Application.Info.Title
					'自アプリのキャプションを変更しておく。
					'UPGRADE_ISSUE: App プロパティ App.Title はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"' をクリックしてください。
					App.Title = strAppCaption & " "
					'トップレベルのウィンドウハンドルを得る。
					lngFirstTophWnd = FindWindow("ThunderRT6Main", strAppCaption)
					'直前にアクティブなウィンドウハンドルを得る。
					lngFirstPophWnd = GetLastActivePopup(lngFirstTophWnd)
					'ウィンドウをアクティブにする。
					lngResult = SetForegroundWindow(lngFirstPophWnd)
					'最小化されタスクバーに格納されてある場合があるので、ウィンドウを通常表示にさせる。
					lngResult = ShowWindow(lngFirstPophWnd, SW_RESTORE)
					Exit Function
				Else
					MsgBox("複数起動中です。")
				End If
			End If
		End If
		'2013/03/13 ADD↑
		
		If INIFile = vbNullString Then
			INIFile = "SanwaS.ini"
			'UPGRADE_WARNING: Dir に新しい動作が指定されています。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
			If Len(Dir(AppPath(INIFile))) = 0 Then
				Err.Raise(vbObjectError + 1,  , "ＩＮＩファイルがありません。")
				Exit Function
			End If
		End If
		
		'----------------------------------------------------
		'INIファイルより情報を取得
		'----------------------------------------------------
		If INI.CONNECT = vbNullString Then
			'UPGRADE_WARNING: オブジェクト GetIni() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			INI.CONNECT = GetIni("Common", "Connect", INIFile)
			'iniFileのデータベース接続文字列設定
			If INI.CONNECT = vbNullString Then
				If DBType = 0 Then
					Call WriteIni("Common", "Connect", DBProvider & AppPath & DBPath, INIFile)
					INI.CONNECT = DBProvider & AppPath & DBPath
				Else
					Call WriteIni("Common", "Connect", DBProvider, INIFile)
					INI.CONNECT = DBProvider
				End If
			End If
		End If
		
		'データベースオープン
		If DBOpen(INI.CONNECT, Cn, True) = False Then
			Exit Function
		End If
		
		'国情報
		''    COUNTRY_CODE = GetIni("COUNTRY", "COUNTRY", INIFile) '2014/07/10 ADD
		[COUNTRY_CODE] = GetCountry
		If [COUNTRY_CODE] = "CN" Then
			KIN_HASU = 2
			KIN_FMT = ".00"
		Else
			KIN_HASU = 0
			KIN_FMT = ""
		End If
		
		ApplicationInit = True
	End Function
	
	Public Function ApplicationUnload() As Boolean
		Cn.Close()
		'UPGRADE_NOTE: オブジェクト Cn をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		Cn = Nothing
	End Function
	
	'----------------------------------------------------------
	'管理テーブルより番号を取得する
	'----------------------------------------------------------
	Public Function GetCounter(ByRef sItemName As String, Optional ByRef sTokuID As String = "") As Integer
		Dim cmd As ADODB.Command
		
		cmd = New ADODB.Command
		
		cmd.let_ActiveConnection(Cn)
		cmd.CommandText = "usp_GetCounter"
		cmd.CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
		
		With cmd.Parameters
			''        .Refresh
			''        .Item(0).Direction = adParamReturnValue
			.Item(1).Value = sItemName
			.Item(2).Value = sTokuID
			''        .Item(2).Value = sTokuID
			''        .Item(3).Direction = adParamOutput
			''        .Item(4).Direction = adParamOutput
			''        .Item(5).Direction = adParamOutput
		End With
		
		cmd.Execute()
		If cmd.State <> 0 Then
			If cmd.Parameters(0).Value <> 0 Then
				CriticalAlarm((cmd.Parameters("@RetST").Value & vbCrLf & cmd.Parameters("@RetMSG").Value))
				GoTo GetCount_err
			End If
		Else
			If cmd.Parameters(0).Value <> 0 Then
				CriticalAlarm((cmd.Parameters("@RetST").Value & vbCrLf & cmd.Parameters("@RetMSG").Value))
				GoTo GetCount_err
			End If
		End If
		
		GetCounter = cmd.Parameters("@GetNO").Value
		
GetCount_err: 
		'UPGRADE_NOTE: オブジェクト cmd をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		cmd = Nothing
		
	End Function
	
	'----------------------------------------------------------
	'製品情報Ｍより製品区分を取得する
	'----------------------------------------------------------
	Public Function Get製品区分(ByRef PCKBN As String, ByRef SEIHNO As String, ByRef SIYONO As String) As Short
		Dim cmd As ADODB.Command
		
		''    Debug.Print "Get製品区分"
		
		HourGlass(True)
		Get製品区分 = -1
		On Error GoTo Get製品区分_Err
		
		cmd = New ADODB.Command
		
		cmd.let_ActiveConnection(Cn)
		cmd.CommandText = "usp_Get製品区分"
		cmd.CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
		
		With cmd.Parameters
			.Item(0).Direction = ADODB.ParameterDirectionEnum.adParamReturnValue
			.Item("@iPC区分").Value = PCKBN
			.Item("@i製品NO").Value = SEIHNO
			.Item("@i仕様NO").Value = SIYONO
		End With
		
		cmd.Execute()
		If cmd.State <> 0 Then
			If cmd.Parameters(0).Value <> 0 Then
				CriticalAlarm((cmd.Parameters("@RetST").Value & vbCrLf & cmd.Parameters("@RetMSG").Value))
				GoTo Get製品区分_Err
			End If
		Else
			If cmd.Parameters(0).Value <> 0 Then
				CriticalAlarm((cmd.Parameters("@RetST").Value & vbCrLf & cmd.Parameters("@RetMSG").Value))
				GoTo Get製品区分_Err
			End If
		End If
		
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		If IsDbNull(cmd.Parameters("@o製品区分").Value) Then
			Get製品区分 = -1
		Else
			Get製品区分 = cmd.Parameters("@o製品区分").Value
		End If
		
Get製品区分_Correct: 
		On Error GoTo 0
		
		HourGlass(False)
		'UPGRADE_NOTE: オブジェクト cmd をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		cmd = Nothing
		
		Exit Function
		
Get製品区分_Err: '---エラー時
		With Err
			Beep()
			MsgBox("trans Error code = " & Err.Number & vbCrLf & Err.Description, MsgBoxStyle.Critical)
			'---トランザクションをロールバックする
			Resume Get製品区分_Correct
		End With
	End Function
	
	Public Function GetDates(ByRef ID As String) As String
		'***********************************************************
		'機能       更新日付を取得します。
		'引数       ID：更新日付名
		'                「月次更新日」等
		'戻り値     日付
		'***********************************************************
		Dim rs As ADODB.Recordset
		Dim sql As String
		
		On Error GoTo GetDates_Err
		
		'マウスポインターを砂時計にする
		HourGlass(True)
		
		sql = "SELECT 更新日付 FROM TMDates" & " WHERE DateID = '" & SQLString(ID) & "'"
		
		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockReadOnly)
		With rs
			If .EOF Then
				GetDates = "1990/01/01"
				ReleaseRs(rs)
				GoTo GetDates_Correct
			Else
				GetDates = .Fields("更新日付").Value
			End If
		End With
		
GetDates_Correct: 
		ReleaseRs(rs)
		HourGlass(False)
		Exit Function
GetDates_Err: 
		MsgBox(Err.Number & " " & Err.Description)
		HourGlass(False)
		
	End Function
	
	Private Function GetCountry() As String
		'国情報取得
		'    COUNTRY_CODE = GetIni("COUNTRY", "COUNTRY", INIFile) '2014/07/10 ADD
		
		Dim rs As ADODB.Recordset
		Dim sql As String
		
		On Error GoTo GetDates_Err
		
		'マウスポインターを砂時計にする
		HourGlass(True)
		
		sql = "SELECT COUNTRY_CODE"
		sql = sql & " FROM AppCountry"
		
		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockReadOnly)
		With rs
			If .EOF Then
				GetCountry = ""
				'            ReleaseRs rs
				GoTo GetDates_Correct
			Else
				GetCountry = .Fields("COUNTRY_CODE").Value
			End If
		End With
		
GetDates_Correct: 
		ReleaseRs(rs)
		HourGlass(False)
		Exit Function
GetDates_Err: 
		MsgBox(Err.Number & " " & Err.Description)
		HourGlass(False)
		
	End Function
End Module