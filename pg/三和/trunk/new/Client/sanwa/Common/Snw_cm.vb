Option Strict Off
Option Explicit On

Imports System.Runtime.InteropServices

''' <summary>
''' 
''' </summary>
Public Module Snw_CM

	<DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
	Private Function FindWindow(lpClassName As String, lpWindowName As String) As IntPtr
	End Function

	<DllImport("user32.dll")>
	Private Function GetLastActivePopup(hWnd As IntPtr) As IntPtr
	End Function

	<DllImport("user32.dll")>
	Private Function SetForegroundWindow(hWnd As IntPtr) As Boolean
	End Function

	<DllImport("user32.dll")>
	Private Function ShowWindow(hWnd As IntPtr, nCmdShow As Integer) As Boolean
	End Function

	Private Const SW_RESTORE As Integer = 9

	'INIファイル
	Public Structure INI_REC
		Public CONNECT As String
	End Structure

	Public LoginSucceeded As Boolean 'Login

	Public Const DBType As Integer = 1 'SQLSERVER

	'Const DBProvider As String = "Provider=SQLOLEDB.1;User ID=sa;Initial Catalog=SanwaSDB;Data Source=XC16_nt"
	Private ReadOnly DBProvider As String = My.Settings.AdodbConnectionString
	Const DBPath As String = ""

	'--項目の桁数設定----------
	Public Const TokuIDLength As Integer = 4 '仕入先の桁数
	Public Const SeiIDLength As Integer = 7 '製品の桁数
	Public Const ShiyoIDLength As Integer = 7 '仕様の桁数

	'--レジストリの項目名の指定----------
	Public Const ProductName As String = "SanwaS"

	Public INI As INI_REC
	Public Cn As ADODB.Connection

	'国情報
	Public COUNTRY_CODE As String '2014/07/10 ADD
	Public KIN_HASU As Integer '2014/07/10 ADD
	Public KIN_FMT As String '2014/07/10 ADD

	Public Function ApplicationInit(Optional MultiMode As Boolean = False) As Boolean
		'Dim lngFirstTophWnd As Long
		'Dim lngFirstPophWnd As Long
		'Dim lngResult As Long

		ApplicationInit = False

		'2013/03/13 ADD↓
		If PrevInstance() Then
			If MultiMode = False Then
				Dim strAppName As String = System.Diagnostics.Process.GetCurrentProcess().ProcessName
				Dim hWndMain As IntPtr = FindWindow(Nothing, strAppName)
				If hWndMain <> IntPtr.Zero Then
					Dim hWndPopup As IntPtr = GetLastActivePopup(hWndMain)
					SetForegroundWindow(hWndPopup)
					ShowWindow(hWndPopup, SW_RESTORE)
				End If
				Exit Function
			Else
				MessageBox.Show("複数起動中です。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning)
			End If
		End If
		'2013/03/13 ADD↑

		If INIFile = "" Then
			INIFile = "SanwaS.ini"
			Dim filePath As String = AppPath(INIFile)

			' ファイルが存在しない場合は例外をスロー
			If Not System.IO.File.Exists(filePath) Then
				Throw New ApplicationException("ＩＮＩファイルがありません。")
			End If
		End If

		'----------------------------------------------------
		'INIファイルより情報を取得
		'----------------------------------------------------
		If String.IsNullOrEmpty(INI.CONNECT) Then
			INI.CONNECT = GetIni("Common", "Connect", INIFile)
			If String.IsNullOrEmpty(INI.CONNECT) Then
				If DBType = 0 Then
					'WriteIni("Common", "Connect", DBProvider & AppPath() & DBPath, INIFile)
					INI.CONNECT = DBProvider & AppPath() & DBPath
				Else
					'WriteIni("Common", "Connect", DBProvider, INIFile)
					INI.CONNECT = DBProvider
				End If
			End If
		End If

		'データベースオープン
		If Not DBOpen(INI.CONNECT, Cn, True) Then
			Return False
		End If

		'国情報
		COUNTRY_CODE = GetCountry()
		If COUNTRY_CODE = "CN" Then
			KIN_HASU = 2
			KIN_FMT = ".00"
		Else
			KIN_HASU = 0
			KIN_FMT = ""
		End If

		ApplicationInit = True
	End Function

	Public Function ApplicationUnload() As Boolean
		If Cn IsNot Nothing AndAlso Cn.State = ConnectionState.Open Then
			Cn.Close()
			Cn = Nothing
		End If
		Return True
	End Function

	'管理テーブルより番号を取得する
	Public Function GetCounter(sItemName As String, Optional sTokuID As String = "") As Integer
		Dim cmd As New ADODB.Command

		GetCounter = 0

		cmd.ActiveConnection = Cn
		cmd.CommandText = "usp_GetCounter"
		cmd.CommandType = ADODB.CommandTypeEnum.adCmdStoredProc

		With cmd.Parameters
			' .Refresh
			' .Item(0).Direction = ADODB.ParameterDirectionEnum.adParamReturnValue
			.Item(1).Value = sItemName
			.Item(2).Value = sTokuID
			' .Item(3).Direction = ADODB.ParameterDirectionEnum.adParamOutput
			' .Item(4).Direction = ADODB.ParameterDirectionEnum.adParamOutput
			' .Item(5).Direction = ADODB.ParameterDirectionEnum.adParamOutput
		End With

		cmd.Execute()

		If cmd.State <> 0 Then
			If cmd.Parameters(0).Value <> 0 Then
				CriticalAlarm(cmd.Parameters("@RetST").Value & vbCrLf & cmd.Parameters("@RetMSG").Value)
				GoTo GetCount_err
			End If
		Else
			If cmd.Parameters(0).Value <> 0 Then
				CriticalAlarm(cmd.Parameters("@RetST").Value & vbCrLf & cmd.Parameters("@RetMSG").Value)
				GoTo GetCount_err
			End If
		End If

		GetCounter = CInt(cmd.Parameters("@GetNO").Value)

GetCount_err:
		cmd = Nothing
	End Function

	'製品情報Ｍより製品区分を取得する
	Public Function Get製品区分(PCKBN As String, SEIHNO As String, SIYONO As String) As Integer
		Dim cmd As New ADODB.Command

		' Debug.Print "Get製品区分"

		HourGlass(True)
		Get製品区分 = -1
		On Error GoTo Get製品区分_Err

		cmd.ActiveConnection = Cn
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
				CriticalAlarm(cmd.Parameters("@RetST").Value & vbCrLf & cmd.Parameters("@RetMSG").Value)
				GoTo Get製品区分_Err
			End If
		Else
			If cmd.Parameters(0).Value <> 0 Then
				CriticalAlarm(cmd.Parameters("@RetST").Value & vbCrLf & cmd.Parameters("@RetMSG").Value)
				GoTo Get製品区分_Err
			End If
		End If

		If IsDBNull(cmd.Parameters("@o製品区分").Value) Then
			Get製品区分 = -1
		Else
			Get製品区分 = CInt(cmd.Parameters("@o製品区分").Value)
		End If

Get製品区分_Correct:
		On Error GoTo 0

		HourGlass(False)
		cmd = Nothing

		Exit Function

Get製品区分_Err:
		With Err()
			Beep()
			MsgBox("trans Error code = " & .Number & vbCrLf & .Description, vbCritical)
			Resume Get製品区分_Correct
		End With
	End Function

	''' <summary>
	''' 更新日付を取得します。
	''' </summary>
	''' <param name="ID">更新日付名</param>
	''' <returns>日付</returns>
	Public Function GetDates(ID As String) As String

		Dim rs As ADODB.Recordset
		Dim sql As String

		On Error GoTo GetDates_Err

		' マウスポインターを砂時計にする
		HourGlass(True)

		sql = "SELECT 更新日付 FROM TMDates WHERE DateID = '" & SQLString(ID) & "'"

		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockReadOnly)
		With rs
			If .EOF Then
				GetDates = "1990/01/01"
				ReleaseRs(rs)
				GoTo GetDates_Correct
			Else
				GetDates = .Fields("更新日付").Value.ToString()
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

	''' <summary>
	''' 国情報取得
	''' </summary>
	''' <returns></returns>
	Private Function GetCountry() As String
		'国情報取得
		'COUNTRY_CODE = GetIni("COUNTRY", "COUNTRY", INIFile) '2014/07/10 ADD

		Dim rs As ADODB.Recordset
		Dim sql As String

		On Error GoTo GetCountry_Err

		' マウスポインターを砂時計にする
		HourGlass(True)

		sql = "SELECT COUNTRY_CODE FROM AppCountry"

		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockReadOnly)
		With rs
			If .EOF Then
				GetCountry = ""
				GoTo GetCountry_Correct
			Else
				GetCountry = .Fields("COUNTRY_CODE").Value.ToString()
			End If
		End With

GetCountry_Correct:
		ReleaseRs(rs)
		HourGlass(False)
		Exit Function

GetCountry_Err:
		MsgBox(Err.Number & " " & Err.Description)
		HourGlass(False)
	End Function

	''' <summary>
	''' 同名のプロセスが起動しているかどうかを示す値を返します。
	''' </summary>
	''' <returns>
	''' プロセス起動判定結果(<c>Boolean</c>)
	''' <c>True</c>:同名のプロセスが起動中
	''' <c>False</c>:同名のプロセスは未起動
	''' </returns>
	''' <remarks></remarks>
	Private Function PrevInstance() As Boolean

		' このアプリケーションのプロセス名を取得
		Dim strThisProcess As String = System.Diagnostics.Process.GetCurrentProcess().ProcessName

		' 同名のプロセスが他に存在する場合は、既に起動していると判断する
		If System.Diagnostics.Process.GetProcessesByName(strThisProcess).Length > 1 Then
			Return True
		End If

		' 存在しない場合は False を返す
		Return False
	End Function 'PrevInstance

End Module
