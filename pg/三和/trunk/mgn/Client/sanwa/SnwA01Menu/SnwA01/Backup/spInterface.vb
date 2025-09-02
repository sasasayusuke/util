Option Strict Off
Option Explicit On
Module spInterface
	'Ver.1.00           '2002.04.17
	'Ver.2.00           '2003.09.04     Lock系にｺｰﾄﾞを追加
	'Ver.2.10           '2020.10.30     ロックしているデータのカウントを取るメソッド追加
	
	Function GetServerCurrentMonth(Optional ByRef Mode As Short = 0) As Object
		'***********************************************************
		'機能       システム情報の日付を取得します
		'引数       Mode 0 : サーバーのGETDATE()による月度
		'                1 : システムの販売月度
		'                2 : システムの棚卸日付
		'戻り値     0:OK 以外NG
		'***********************************************************
		Dim cmd As ADODB.Command
		
		cmd = New ADODB.Command
		
		cmd.let_ActiveConnection(Cn)
		cmd.CommandText = "sp_GetPrmDate"
		cmd.CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
		
		With cmd.Parameters
			.Item(1).Value = Mode
		End With
		
		cmd.Execute()
		If cmd.State <> 0 Then
			If cmd.Parameters(0).Value <> 0 Then
				CriticalAlarm((cmd.Parameters("@RetST").Value & vbCrLf & cmd.Parameters("@RetMSG").Value))
				GoTo GetServerCurrentMonth_err
			End If
		Else
			If cmd.Parameters(0).Value <> 0 Then
				CriticalAlarm((cmd.Parameters("@RetST").Value & vbCrLf & cmd.Parameters("@RetMSG").Value))
				GoTo GetServerCurrentMonth_err
			End If
		End If
		
		GetServerCurrentMonth = cmd.Parameters("@PARAM").Value
		
GetServerCurrentMonth_err: 
		'UPGRADE_NOTE: オブジェクト cmd をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		cmd = Nothing
		
	End Function
	
	'''''Function GetServerPassWord() As Variant
	'''''    Dim rs As ADODB.Recordset, sql As String
	'''''
	'''''    'サーバーのパスワードを取得します
	'''''    sql = "EXEC sp_MMPWD "
	'''''    On Error Resume Next
	'''''    Set rs = OpenRs(sql, Cn, adOpenForwardOnly)
	'''''    If Err Then
	'''''        On Error GoTo 0
	'''''        GetServerPassWord = Null
	'''''    Else
	'''''        On Error GoTo 0
	'''''        GetServerPassWord = rs(0)
	'''''        rs.Close
	'''''    End If
	'''''End Function
	
	
	Public Function LockData(ByRef DataName As String, ByRef No As Integer, Optional ByRef CD As Object = "", Optional ByRef check As Short = 0) As Boolean
		'***********************************************************
		'機能       データをロックを行なう。
		'引数       DataName  読み出すデータ名をセット
		'           No        ユニークな伝票番号をセット
		'戻り値     成功したらTrue  失敗したら(使用中)False
		'***********************************************************
		Dim cmd As ADODB.Command
		
		cmd = New ADODB.Command
		
		cmd.let_ActiveConnection(Cn)
		cmd.CommandText = "usp_LockData"
		cmd.CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
		
		With cmd.Parameters
			.Item(1).Value = DataName
			.Item(2).Value = No
			'UPGRADE_WARNING: オブジェクト CD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item(3).Value = CD '2003/09/04 ADD
			.Item(4).Value = check '2003/09/04 ADD
		End With
		
		cmd.Execute()
		If cmd.State <> 0 Then
			If cmd.Parameters(0).Value <> 0 Then
				CriticalAlarm((cmd.Parameters("@RetST").Value & vbCrLf & cmd.Parameters("@RetMSG").Value))
				GoTo LockData_err
			End If
		Else
			If cmd.Parameters(0).Value <> 0 Then
				CriticalAlarm((cmd.Parameters("@RetST").Value & vbCrLf & cmd.Parameters("@RetMSG").Value))
				GoTo LockData_err
			End If
		End If
		
		LockData = True
		
LockData_err: 
		'UPGRADE_NOTE: オブジェクト cmd をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		cmd = Nothing
		
	End Function
	
	Public Function UnLockData(ByRef DataName As String, ByRef No As Integer, Optional ByRef CD As Object = "") As Boolean
		'***********************************************************
		'機能       データをロックを解除する。
		'引数       DataName  読み出すデータ名をセット
		'           No        ユニークな伝票番号をセット
		'戻り値     成功したらTrue  失敗したら(使用中)False
		'***********************************************************
		Dim cmd As ADODB.Command
		
		cmd = New ADODB.Command
		
		cmd.let_ActiveConnection(Cn)
		cmd.CommandText = "usp_UnLockData"
		cmd.CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
		
		With cmd.Parameters
			.Item(1).Value = DataName
			.Item(2).Value = No
			'UPGRADE_WARNING: オブジェクト CD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item(3).Value = CD '2003/09/04 ADD
		End With
		
		cmd.Execute()
		If cmd.State <> 0 Then
			If cmd.Parameters(0).Value <> 0 Then
				CriticalAlarm((cmd.Parameters("@RetST").Value & vbCrLf & cmd.Parameters("@RetMSG").Value))
				GoTo UnLockData_err
			End If
		Else
			If cmd.Parameters(0).Value <> 0 Then
				CriticalAlarm((cmd.Parameters("@RetST").Value & vbCrLf & cmd.Parameters("@RetMSG").Value))
				GoTo UnLockData_err
			End If
		End If
		
		UnLockData = True
		
UnLockData_err: 
		'UPGRADE_NOTE: オブジェクト cmd をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		cmd = Nothing
		
	End Function
	
	Public Function UnLockAllData() As Boolean
		'***********************************************************
		'機能       端末ごとの全てのデータのロックを解除する。
		'引数
		'戻り値     成功したらTrue  失敗したら(使用中)False
		'***********************************************************
		Dim cmd As ADODB.Command
		
		cmd = New ADODB.Command
		
		cmd.let_ActiveConnection(Cn)
		cmd.CommandText = "usp_UnLockAllData"
		cmd.CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
		
		cmd.Parameters.Refresh()
		cmd.Execute()
		If cmd.State <> 0 Then
			If cmd.Parameters(0).Value <> 0 Then
				CriticalAlarm((cmd.Parameters("@RetST").Value & vbCrLf & cmd.Parameters("@RetMSG").Value))
				GoTo UnLockAllData_err
			End If
		Else
			If cmd.Parameters(0).Value <> 0 Then
				CriticalAlarm((cmd.Parameters("@RetST").Value & vbCrLf & cmd.Parameters("@RetMSG").Value))
				GoTo UnLockAllData_err
			End If
		End If
		
		UnLockAllData = True
		
UnLockAllData_err: 
		'UPGRADE_NOTE: オブジェクト cmd をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		cmd = Nothing
		
	End Function
	
	'2020/10/30 ADD↓
	Public Function GetAppLockCount(ByRef DataName As String) As Integer
		'***********************************************************
		'機能       データロック中のアプリケーションのカウントを返す
		'引数       DataName    読み出すデータ名をセット
		'           PCName      起動したプログラム名をセット
		'戻り値     上記のデータロックしている起動数を返す
		'***********************************************************
		Dim rs As ADODB.Recordset
		Dim sql As String
		
		On Error GoTo GetAppLockCount_Err
		
		'マウスポインターを砂時計にする
		'Call HourGlass(True)
		
		'SQL生成
		sql = "SELECT  CNT = COUNT(DataName)"
		sql = sql & " FROM AppLockData"
		sql = sql & " WHERE   DataName = '" & DataName & "'"
		sql = sql & " AND     PCName = '" & GetPCName() & "'"
		
		
		
		'SQL実行
		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
		
		With rs
			If .EOF Then
				GetAppLockCount = 0
			Else
				GetAppLockCount = .Fields("cnt").Value
			End If
		End With
		
		Call ReleaseRs(rs)
		
		'Call HourGlass(False)
		Exit Function
GetAppLockCount_Err: 
		'Call HourGlass(False)
		'エラーの生成
		Err.Raise(Err.Number,  , Err.Description)
	End Function
	'2020/10/30 ADD↑
End Module