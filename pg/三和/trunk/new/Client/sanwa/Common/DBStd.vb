Option Strict On
Option Explicit On

Imports ADODB

''' <summary>
''' ADODB VERSION
''' Ver.1.00           '2002.04.17
''' Ver.1.01           '2002.05.03     SQLDateRange '9999/12/31'対応
''' Ver.1.02           '2002.09.19     SQLIntRange 大小比較をINTに変換後比較
''' Ver.1.03           '2003.04.28     SQLCurRange 作成
''' Ver.1.04           '2003.06.18     SQLCurRange 不具合修正
''' Ver.1.05           '2005.03.03     SQLIntRange 大小比較をINTに変換だとｵｰﾊﾞｰﾌﾛｰになるのでclngにする
''' </summary>

Public Module DBStd

    ''' <summary>
    ''' 
    ''' </summary>
    Public Enum wsTypes
        wsTypeACC = 0
        wsTypeSQLSV = 1
    End Enum

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Provider"></param>
    ''' <param name="Cn"></param>
    ''' <param name="ReturnErr"></param>
    ''' <returns>true: 正常終了、false:異常終了</returns>
    Public Function DBOpen(ByVal Provider As String, ByRef Cn As adodb.Connection, Optional ReturnErr As Boolean = False) As Boolean
        Dim ErMsg As String
        DBOpen = False

        Try
            Cursor.Current = Cursors.WaitCursor

            If Cn Is Nothing Then
                Cn = New adodb.Connection
                Cn.Open(Provider)
            ElseIf Cn.State = 0 Then
                Cn.Open(Provider)
            End If

            DBOpen = True

        Catch ex As Exception
            If Not ReturnErr Then
                Return DBOpen
            End If

            ErMsg = GetErrorDetails(Cn)

            Cursor.Current = Cursors.Default

            If MessageBox.Show("データベースに接続できません。" & vbCrLf & "(" & Provider & ")" & vbCrLf & ErMsg, "サーバーへの接続", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation) = DialogResult.Retry Then
                Cn = Nothing
                Return DBOpen(Provider, Cn, ReturnErr)
            End If
        Finally
            Cursor.Current = Cursors.Default
        End Try

        Return DBOpen
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Cn"></param>
    ''' <returns>true: 正常終了、false:異常終了</returns>
    Public Function DBClose(ByRef Cn As adodb.Connection) As Boolean
        'DBClose = False    '戻り値初期設定
        Try
            If Cn IsNot Nothing Then
                Cn.Close()
                Cn = Nothing
            End If

            Return True

        Catch ex As Exception
            CriticalAlarm(ex.Message)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' 発生しているエラーを Public ErrorDetails にセットし、その内容を文字列で返します
    ''' </summary>
    ''' <param name="Cn"></param>
    ''' <returns>String</returns>
    Public Function GetErrorDetails(ByVal Cn As adodb.Connection) As String
        Dim ErrMsg As String = String.Empty
        Dim Er As adodb.Error

        Cn.Errors.Refresh()
        If Cn.Errors.Count > 0 Then
            For Each Er In Cn.Errors
                ErrMsg &= vbCrLf & "(" & Er.Number & ") " & Er.Description
            Next
        End If
        Return ErrMsg
    End Function

    ''' <summary>
    ''' レコードを開く
    ''' </summary>
    ''' <param name="Source"></param>
    ''' <param name="Cn"></param>
    ''' <param name="CursorType"></param>
    ''' <param name="LockType"></param>
    ''' <param name="Options"></param>
    ''' <returns></returns>
    Public Function OpenRs(Source As String, Cn As adodb.Connection,
        Optional CursorType As CursorTypeEnum = CursorTypeEnum.adOpenForwardOnly,
        Optional LockType As LockTypeEnum = LockTypeEnum.adLockReadOnly,
        Optional Options As Integer = -1) As adodb.Recordset

        Dim RST As New adodb.Recordset
        RST.Open(Source, Cn, CursorType, LockType, Options)
        Return RST
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="rs"></param>
    Public Sub ReleaseRs(ByRef rs As adodb.Recordset)
        If rs IsNot Nothing Then
            rs.Close()
            rs = Nothing
        End If
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="FieldName"></param>
    ''' <param name="TextFrom"></param>
    ''' <param name="TextTo"></param>
    ''' <param name="wsType"></param>
    ''' <param name="CharJoin"></param>
    ''' <returns></returns>
    Public Function SQLIntRange(ByVal FieldName As String, Optional ByVal TextFrom As Object = Nothing,
        Optional ByVal TextTo As Object = Nothing, Optional ByVal wsType As wsTypes = wsTypes.wsTypeACC,
        Optional CharJoin As Boolean = False) As String

        Dim Criteria As String = String.Empty

        If TextFrom Is Nothing Then
            If TextTo Is Nothing Then
                Return String.Empty
            ElseIf IsCheckNull(TextTo) Then
                Return String.Empty
            Else
                TextFrom = TextTo
            End If
        ElseIf IsCheckNull(TextFrom) Then
            If TextTo Is Nothing Then
                Return String.Empty
            ElseIf IsCheckNull(TextTo) Then
                Return String.Empty
            Else
                If CharJoin Then
                    Criteria = " and"
                End If
                Return Criteria & " " & FieldName & "<=" & Trim(TextTo.ToString())
            End If
        Else
            If TextTo Is Nothing Then
                TextTo = TextFrom
            ElseIf IsCheckNull(TextTo) Then
                If CharJoin Then
                    Criteria = " and"
                End If
                Return Criteria & " " & FieldName & ">=" & Trim(TextFrom.ToString())
            End If
        End If

        If CharJoin Then
            Criteria = " and"
        End If

        ' 大小が逆の場合
        If CLng(TextFrom) > CLng(TextTo) Then
            Return Criteria & " " & FieldName & ">=" & Trim(TextTo.ToString()) & " And " & FieldName & "<=" & Trim(TextFrom.ToString())
        Else
            Return Criteria & " " & FieldName & ">=" & Trim(TextFrom.ToString()) & " And " & FieldName & "<=" & Trim(TextTo.ToString())
        End If
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="FieldName"></param>
    ''' <param name="TextFrom"></param>
    ''' <param name="TextTo"></param>
    ''' <param name="wsType"></param>
    ''' <param name="CharJoin"></param>
    ''' <returns></returns>
    Public Function SQLCurRange(ByVal FieldName As String, Optional ByVal TextFrom As Object = Nothing,
        Optional ByVal TextTo As Object = Nothing, Optional ByVal wsType As wsTypes = wsTypes.wsTypeACC,
        Optional ByVal CharJoin As Boolean = False) As String

        Dim Criteria As String = ""

        If TextFrom Is Nothing Then
            If TextTo Is Nothing OrElse IsCheckNull(TextTo) Then
                Return ""
            Else
                TextFrom = TextTo
            End If
        ElseIf IsCheckNull(TextFrom) Then
            If TextTo Is Nothing OrElse IsCheckNull(TextTo) Then
                Return ""
            Else
                If CharJoin Then Criteria = " and"
                Return Criteria & " " & FieldName & " <= " & Trim(CDec(TextTo).ToString)
            End If
        End If

        If TextTo Is Nothing Then
            TextTo = TextFrom
        ElseIf IsCheckNull(TextTo) Then
            If CharJoin Then Criteria = " and"
            Return Criteria & " " & FieldName & ">=" & Trim(CDec(TextFrom).ToString)
        End If

        If CharJoin Then Criteria = " and"

        ' 大小が逆の場合
        If CDec(TextFrom) > CDec(TextTo) Then
            Return Criteria & " " & FieldName & " >= " & Trim(CDec(TextTo).ToString) & " And " & FieldName & "<=" & Trim(CDec(TextFrom).ToString)
        Else
            Return Criteria & " " & FieldName & " >= " & Trim(CDec(TextFrom).ToString) & " And " & FieldName & "<=" & Trim(CDec(TextTo).ToString)
        End If
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="code"></param>
    ''' <returns></returns>
    Public Function SQLQuoteString(ByVal code As Object) As String
        If IsDBNull(code) OrElse Trim(code.ToString()) = String.Empty Then
            Return "Null"
        End If
        Return "'" & SQLString(code) & "'"
    End Function

    ''' <summary>
    ''' 文字列内のダブルクォーテーションチェックルーチン
    ''' 書式        st = SQLString(CODE)
    ''' </summary>
    ''' <param name="code"></param>
    ''' <returns>変換されたコード</returns>
    Public Function SQLString(ByVal code As Object) As String
        If IsCheckNull(code) Then
            Return String.Empty
        End If

        Dim result As String = Trim(code.ToString())

        If Not result.Contains("'") Then
            Return result
        End If

        Return result.Replace("'", "''")
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="FieldName"></param>
    ''' <param name="TextFrom"></param>
    ''' <param name="TextTo"></param>
    ''' <param name="wsType"></param>
    ''' <param name="CharJoin"></param>
    ''' <returns></returns>
    Public Function SQLStringRange(ByVal FieldName As String, Optional ByVal TextFrom As Object = Nothing,
        Optional ByVal TextTo As Object = Nothing, Optional ByVal wsType As wsTypes = wsTypes.wsTypeACC,
        Optional CharJoin As Boolean = False) As String

        Dim Criteria As String = ""

        If TextFrom Is Nothing Then
            If TextTo Is Nothing OrElse IsCheckNull(TextTo) OrElse TextTo.ToString() = "最後" Then
                Return String.Empty
            Else
                TextFrom = TextTo
            End If
        ElseIf IsCheckNull(TextFrom) OrElse TextFrom.ToString() = "最初" Then
            If TextTo Is Nothing OrElse IsCheckNull(TextTo) OrElse TextTo.ToString() = "最後" Then
                Return String.Empty
            Else
                If CharJoin Then Criteria = " and"
                Return Criteria & " " & FieldName & "<='" & SQLString(TextTo) & "'"
            End If
        End If

        If TextTo Is Nothing Then
            TextTo = TextFrom
        ElseIf IsCheckNull(TextTo) OrElse TextTo.ToString() = "最後" Then
            If CharJoin Then Criteria = " and"
            Return Criteria & " " & FieldName & ">='" & SQLString(TextFrom) & "'"
        End If

        If CharJoin Then Criteria = " and"

        ' 大小が逆の場合
        If String.Compare(TextFrom.ToString(), TextTo.ToString()) > 0 Then
            Return Criteria & " " & FieldName & ">='" & SQLString(TextTo) & "' And " & FieldName & "<='" & SQLString(TextFrom) & "'"
        Else
            Return Criteria & " " & FieldName & ">='" & SQLString(TextFrom) & "' And " & FieldName & "<='" & SQLString(TextTo) & "'"
        End If
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="d"></param>
    ''' <param name="wsType"></param>
    ''' <returns></returns>
    Public Function SQLDate(ByVal d As Object, Optional ByVal wsType As wsTypes = wsTypes.wsTypeACC) As String
        If Not IsDate(d) Then
            Return "Null"
        End If
        If wsType = wsTypes.wsTypeACC Then
            Return "#" & Format(CDate(d), "yyyy/MM/dd") & "#"
        Else
            Return "'" & Format(CDate(d), "yyyy/MM/dd") & "'"
        End If
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="FieldName"></param>
    ''' <param name="DateFrom"></param>
    ''' <param name="DateTo"></param>
    ''' <param name="wsType"></param>
    ''' <param name="CharJoin"></param>
    ''' <returns></returns>
    Public Function SQLDateRange(FieldName As String, Optional ByVal DateFrom As Object = Nothing,
        Optional ByVal DateTo As Object = Nothing, Optional ByVal wsType As wsTypes = wsTypes.wsTypeACC,
        Optional CharJoin As Boolean = False) As String

        ' 時刻まで含む項目に対する条件文字列を返します
        Dim Criteria As String = String.Empty
        Dim TmpDate As Object

        ' 日付のNullチェックとキャスト
        If DateFrom Is Nothing OrElse IsCheckNull(DateFrom) Then
            If DateTo Is Nothing OrElse IsCheckNull(DateTo) Then
                Return String.Empty
            Else
                DateFrom = DateTo
            End If
        End If

        If DateTo Is Nothing OrElse IsCheckNull(DateTo) Then
            If CharJoin Then
                Criteria = " and"
            End If
            Return Criteria & " " & FieldName & " >= " & SQLDate(CDate(DateFrom), wsType)
        End If

        If CharJoin Then
            Criteria = " and"
        End If

        ' 日付の大小が逆の場合
        If CDate(DateFrom) > CDate(DateTo) Then
            Try
                TmpDate = CDate(DateFrom).AddDays(1)
                Return Criteria & " " & FieldName & " >= " & SQLDate(CDate(DateTo), wsType) & " And " & FieldName & " < " & SQLDate(CDate(TmpDate), wsType)
            Catch ex As Exception
                TmpDate = DateFrom
                Return Criteria & " " & FieldName & " >= " & SQLDate(CDate(DateTo), wsType) & " And " & FieldName & " <= " & SQLDate(CDate(TmpDate), wsType)
            End Try
        Else
            Try
                TmpDate = CDate(DateTo).AddDays(1)
                Return Criteria & " " & FieldName & " >= " & SQLDate(CDate(DateFrom), wsType) & " And " & FieldName & " < " & SQLDate(CDate(TmpDate), wsType)
            Catch ex As Exception
                TmpDate = DateTo
                Return Criteria & " " & FieldName & " >= " & SQLDate(CDate(DateFrom), wsType) & " And " & FieldName & " <= " & SQLDate(CDate(TmpDate), wsType)
            End Try
        End If
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="FieldName"></param>
    ''' <param name="DateFrom"></param>
    ''' <param name="DateTo"></param>
    ''' <param name="Prefix"></param>
    ''' <param name="wsType"></param>
    ''' <returns></returns>
    Public Function SQLBetween(ByVal FieldName As String, ByVal DateFrom As Object, ByVal DateTo As Object,
            Optional ByVal Prefix As String = "",
            Optional ByVal wsType As wsTypes = wsTypes.wsTypeACC) As String
        Dim strTmp As String = String.Empty

        If IsDBNull(DateFrom) Then
            If Not IsDBNull(DateTo) AndAlso IsDate(DateTo) Then
                strTmp = $"<= {SQLDate(DateTo, wsType)}"
            End If
        ElseIf IsDate(DateFrom) Then
            If IsDBNull(DateTo) Then
                strTmp = $">= {SQLDate(DateFrom, wsType)}"
            ElseIf IsDate(DateTo) Then
                If CDate(DateFrom) > CDate(DateTo) Then
                    strTmp = $"Between {SQLDate(DateTo, wsType)} And {SQLDate(DateFrom, wsType)}"
                Else
                    strTmp = $"Between {SQLDate(DateFrom, wsType)} And {SQLDate(DateTo, wsType)}"
                End If
            End If
        End If

        If strTmp <> String.Empty Then
            strTmp = $"{Prefix} {FieldName} {strTmp}"
        End If

        Return strTmp
    End Function

    ''' <summary>
    ''' ダウンロードレコードセットと受信用レコードセットの双方に存在する同一名のフィールドについて
    ''' ダウンロードレコードセットからレコードを取り出し受信用レコードセットへ格納します
    ''' </summary>
    ''' <param name="SourceRs"></param>
    ''' <param name="ReceiveRs"></param>
    ''' <returns>0: 格納レコード数,-1: 一致するフィールド名がない</returns>
    Public Function DownloadRecords(ByVal SourceRs As adodb.Recordset, ByVal ReceiveRs As adodb.Recordset) As Long

        Dim fld(,) As adodb.Field
        Dim CopyFieldsCount As Integer = 0
        Dim Records As Long = 0

        If SourceRs.EOF Then
            Return 0
        End If

        ' フィールドマッピングを作成
        With ReceiveRs
            ReDim fld(1, .Fields.Count - 1)
            For i As Integer = 0 To .Fields.Count - 1
                Dim sourceField As adodb.Field = Nothing
                Try
                    sourceField = SourceRs.Fields(.Fields(i).Name)
                Catch ex As Exception
                    ' フィールドが存在しない場合
                End Try

                If sourceField IsNot Nothing Then
                    fld(0, CopyFieldsCount) = sourceField
                    fld(1, CopyFieldsCount) = .Fields(i)
                    CopyFieldsCount += 1
                End If
            Next
        End With

        If CopyFieldsCount = 0 Then
            Return -1
        Else
            Do Until SourceRs.EOF
                ReceiveRs.AddNew()
                For i As Integer = 0 To CopyFieldsCount - 1
                    fld(1, i).Value = fld(0, i).Value
                Next
                ReceiveRs.Update()
                Records += 1
                SourceRs.MoveNext()
            Loop
        End If

        Return Records
    End Function

End Module