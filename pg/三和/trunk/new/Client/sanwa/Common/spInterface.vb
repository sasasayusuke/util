Option Explicit On

Imports ADODB

''' <summary>
''' 
''' </summary>
Public Module spInterface

    Function GetServerCurrentMonth(Optional Mode As Integer = 0) As Object
        'システム情報の日付を取得します
        Dim cmd As ADODB.Command
        cmd = New ADODB.Command

        GetServerCurrentMonth = ""

        cmd.ActiveConnection = Cn
        cmd.CommandText = "sp_GetPrmDate"
        cmd.CommandType = ADODB.CommandTypeEnum.adCmdStoredProc

        With cmd.Parameters
            .Item(1).Value = Mode
        End With

        cmd.Execute()

        If cmd.State <> 0 Then
            If cmd.Parameters(0).Value <> 0 Then
                CriticalAlarm(cmd.Parameters("@RetST").Value & vbCrLf & cmd.Parameters("@RetMSG").Value)
                GoTo GetServerCurrentMonth_err
            End If
        Else
            If cmd.Parameters(0).Value <> 0 Then
                CriticalAlarm(cmd.Parameters("@RetST").Value & vbCrLf & cmd.Parameters("@RetMSG").Value)
                GoTo GetServerCurrentMonth_err
            End If
        End If

        GetServerCurrentMonth = cmd.Parameters("@PARAM").Value

GetServerCurrentMonth_err:
        cmd = Nothing
    End Function

    Public Function GetServerPassWord() As Object
        Dim rs As ADODB.Recordset
        Dim sql As String

        'サーバーのパスワードを取得します
        sql = "EXEC sp_MMPWD "
        Try
            Dim cmd As New ADODB.Command
            rs = DBStd.OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly)
            If rs.Read() Then
                Return rs.GetValue(0)
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function LockData(DataName As String, No As Long,
                         Optional CD As Object = Nothing, Optional check As Integer = 0) As Boolean
        'データをロックを行なう
        Dim cmd As ADODB.Command
        cmd = New ADODB.Command

        LockData = False

        cmd.ActiveConnection = Cn
        cmd.CommandText = "usp_LockData"
        cmd.CommandType = ADODB.CommandTypeEnum.adCmdStoredProc

        With cmd.Parameters
            .Item(1).Value = DataName
            .Item(2).Value = No
            .Item(3).Value = CD
            .Item(4).Value = check
        End With

        cmd.Execute()

        If cmd.State <> 0 Then
            If cmd.Parameters(0).Value <> 0 Then
                CriticalAlarm(cmd.Parameters("@RetST").Value & vbCrLf & cmd.Parameters("@RetMSG").Value)
                GoTo LockData_err
            End If
        Else
            If cmd.Parameters(0).Value <> 0 Then
                CriticalAlarm(cmd.Parameters("@RetST").Value & vbCrLf & cmd.Parameters("@RetMSG").Value)
                GoTo LockData_err
            End If
        End If

        LockData = True

LockData_err:
        cmd = Nothing
    End Function


    Public Function UnLockData(DataName As String, No As Long, Optional CD As Object = Nothing) As Boolean
        'データをロックを解除する
        Dim cmd As ADODB.Command
        cmd = New ADODB.Command

        UnLockData = False

        On Error GoTo UnLockData_err

        cmd.ActiveConnection = Cn
        cmd.CommandText = "usp_UnLockData"
        cmd.CommandType = ADODB.CommandTypeEnum.adCmdStoredProc

        With cmd.Parameters
            .Item(1).Value = DataName
            .Item(2).Value = No
            .Item(3).Value = CD
        End With

        cmd.Execute()

        If cmd.State <> 0 Then
            If cmd.Parameters(0).Value <> 0 Then
                CriticalAlarm(cmd.Parameters("@RetST").Value & vbCrLf & cmd.Parameters("@RetMSG").Value)
                GoTo UnLockData_err
            End If
        Else
            If cmd.Parameters(0).Value <> 0 Then
                CriticalAlarm(cmd.Parameters("@RetST").Value & vbCrLf & cmd.Parameters("@RetMSG").Value)
                GoTo UnLockData_err
            End If
        End If

        UnLockData = True

UnLockData_err:
        cmd = Nothing
    End Function

    Public Function UnLockAllData() As Boolean
        '全てのデータのロックを解除する
        Dim cmd As ADODB.Command
        cmd = New ADODB.Command

        UnLockAllData = False

        cmd.ActiveConnection = Cn
        cmd.CommandText = "usp_UnLockAllData"
        cmd.CommandType = ADODB.CommandTypeEnum.adCmdStoredProc

        cmd.Parameters.Refresh()
        cmd.Execute()

        If cmd.State <> 0 Then
            If cmd.Parameters(0).Value <> 0 Then
                CriticalAlarm(cmd.Parameters("@RetST").Value & vbCrLf & cmd.Parameters("@RetMSG").Value)
                GoTo UnLockAllData_err
            End If
        Else
            If cmd.Parameters(0).Value <> 0 Then
                CriticalAlarm(cmd.Parameters("@RetST").Value & vbCrLf & cmd.Parameters("@RetMSG").Value)
                GoTo UnLockAllData_err
            End If
        End If

        UnLockAllData = True

UnLockAllData_err:
        cmd = Nothing
    End Function

    Public Function GetAppLockCount(DataName As String) As Long
        'データロック中のアプリケーションのカウントを返す
        Dim rs As ADODB.Recordset
        Dim sql As String

        On Error GoTo GetAppLockCount_Err

        'SQL生成
        sql = "SELECT COUNT(DataName) AS CNT FROM AppLockData WHERE DataName = '" & DataName & "' AND PCName = '" & GetPCName() & "'"

        'SQL実行
        rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)

        With rs
            If .EOF Then
                GetAppLockCount = 0
            Else
                GetAppLockCount = .Fields("CNT").Value
            End If
        End With

        ReleaseRs(rs)
        Exit Function

GetAppLockCount_Err:
        Err.Raise(Err.Number, , Err.Description)
    End Function

End Module
