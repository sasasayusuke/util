Option Strict Off
Option Explicit On

Imports System.Data.OleDb

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

	Public Enum wsTypes
		wsTypeACC = 0
		wsTypeSQLSV = 1
	End Enum

	Public Function DBOpen(ByVal connectionString As String, ByRef Cn As OleDbConnection, Optional ByVal ReturnErr As Boolean = False) As Boolean
		Dim ErMsg As String = String.Empty
		DBOpen = False

		Try
			Cursor.Current = Cursors.WaitCursor

			If Cn Is Nothing Then
				Cn = New OleDbConnection(connectionString)
				Cn.Open()
			ElseIf Cn.ConnectionString = String.Empty Then
				Cn = New OleDbConnection(connectionString)
				Cn.Open()
			End If

			DBOpen = True

		Catch ex As Exception
			If Not ReturnErr Then
				Return DBOpen
			End If

			ErMsg = ex.Message

			Cursor.Current = Cursors.Default

			If MessageBox.Show("データベースに接続できません。" & vbCrLf & "(" & connectionString & ")" & vbCrLf & ErMsg, "サーバーへの接続", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation) = DialogResult.Retry Then
				Cn = Nothing
				Return DBOpen(connectionString, Cn, ReturnErr)
			End If
		Finally
			Cursor.Current = Cursors.Default
		End Try

		Return DBOpen
	End Function

	Public Function DBClose(ByRef Cn As OleDbConnection) As Boolean
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

	Public Function GetErrorDetails(ByVal Cn As OleDbConnection) As String
		Dim ErrMsg As String = String.Empty

		Try
			''SS For Each er As OleDbError In Cn.Errors
			''SS     ErrMsg &= "(" & er.Number & ") " & er.Message & vbCrLf
			''SS Next

		Catch ex As Exception
			ErrMsg = ex.Message
		End Try

		Return ErrMsg
	End Function

	''SS Public Function OpenRs(Source As String, Cn As OleDbConnection, Optional ByVal CursorType As CursorTypeEnum = CursorTypeEnum.ForwardOnly, Optional ByVal LockType As LockTypeEnum = LockTypeEnum.ReadOnly, Optional ByVal Options As Long = -1) As OleDbDataReader
	''SS     Dim cmd As New OleDbCommand(Source, Cn)
	''SS     Dim reader As OleDbDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
	''SS     Return reader
	''SS End Function

	Public Sub ReleaseRs(ByRef rs As OleDbDataReader)
		If rs IsNot Nothing Then
			rs.Close()
			rs = Nothing
		End If
	End Sub

	Public Function SQLString(ByVal code As Object) As String
		If IsDBNull(code) OrElse String.IsNullOrWhiteSpace(code.ToString()) Then
			Return String.Empty
		End If

		Dim strCode As String = code.ToString().Trim()

		If Not strCode.Contains("'") Then
			Return strCode
		End If

		Return strCode.Replace("'", "''")
	End Function

End Module