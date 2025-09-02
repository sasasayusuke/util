Option Strict Off
Option Explicit On
Module DBStd
	'ADODB VERSION
	'Ver.1.00           '2002.04.17
	'Ver.1.01           '2002.05.03     SQLDateRange '9999/12/31'対応
	'Ver.1.02           '2002.09.19     SQLIntRange 大小比較をINTに変換後比較
	'Ver.1.03           '2003.04.28     SQLCurRange 作成
	'Ver.1.04           '2003.06.18     SQLCurRange 不具合修正
	'Ver.1.05           '2005.03.03     SQLIntRange 大小比較をINTに変換だとｵｰﾊﾞｰﾌﾛｰになるのでclngにする
	
	Public Enum wsTypes
		wsTypeACC = 0
		wsTypeSQLSV = 1
	End Enum
	
	Public Function DBOpen(ByRef Provider As String, ByRef Cn As ADODB.Connection, Optional ByRef ReturnErr As Boolean = False) As Boolean
		Dim ErMsg As String
		
		DBOpen = False
		On Error GoTo DBOpen_Err
DBOpen_Connect: 
		'UPGRADE_WARNING: Screen プロパティ Screen.MousePointer には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
		System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
		If Cn Is Nothing Then
			Cn = New ADODB.Connection
			''        Cn.ConnectionTimeout = 0 '2004/09/29
			''        Cn.CommandTimeout = 0   '2004/08/17
			Cn.Open(Provider)
		ElseIf Cn.ConnectionString = vbNullString Then 
			Cn = New ADODB.Connection
			''        Cn.ConnectionTimeout = 0 '2004/09/29
			''        Cn.CommandTimeout = 0   '2004/08/17
			Cn.Open(Provider)
		End If
		DBOpen = True
		
DBOpen_Exit: 
		'UPGRADE_WARNING: Screen プロパティ Screen.MousePointer には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
		System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
		Exit Function
DBOpen_Err: 
		If ReturnErr = False Then Resume DBOpen_Exit
		
		ErMsg = GetErrorDetails(Cn)
		'UPGRADE_WARNING: Screen プロパティ Screen.MousePointer には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
		System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
		If MsgBox("データベースに接続できません。" & vbCrLf & "(" & INI.CONNECT & ")" & vbCrLf & ErMsg, MsgBoxStyle.Exclamation + MsgBoxStyle.RetryCancel, "サーバーへの接続") = MsgBoxResult.Retry Then
			'UPGRADE_NOTE: オブジェクト Cn をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
			Cn = Nothing
			Resume DBOpen_Connect
		End If
		Resume DBOpen_Exit
	End Function
	
	Public Function DBClose(ByRef Cn As ADODB.Connection) As Boolean
		On Error GoTo DBClose_Err
		DBClose = False '戻り値初期設定
		If Not Cn Is Nothing Then
			Cn.Close()
			'UPGRADE_NOTE: オブジェクト Cn をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
			Cn = Nothing
		End If
		DBClose = True '戻り値設定
		Exit Function
		
DBClose_Err: 
		CriticalAlarm(GetErrorDetails(Cn))
		
	End Function
	'発生しているエラーを Public ErrorDetails にセットし、その内容を文字列で返します
	Public Function GetErrorDetails(ByRef Cn As ADODB.Connection) As String
		
		'エラーの情報がうまく取れない(ado errors)
		Dim ErrMsg As String
		Dim Er As ADODB.Error
		Dim i As Short
		Dim ErrorDetails As New Collection
		ErrMsg = ""
		'UPGRADE_NOTE: オブジェクト ErrorDetails をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		ErrorDetails = Nothing
		Cn.Errors.Refresh()
		i = Cn.Errors.Count - 1
		If i = -1 Then
			ErrorDetails.Add(Err)
			ErrMsg = ErrMsg & "(" & CStr(Err.Number) & ")" & Err.Description
		Else
			If Cn.Errors(i).Number <> Err.Number Then
				ErrorDetails.Add(Err)
				ErrMsg = ErrMsg & "(" & CStr(Err.Number) & ")" & Err.Description
			End If
			For i = i To 0 Step -1
				Er = Cn.Errors(i)
				ErrorDetails.Add(Er)
				ErrMsg = ErrMsg & vbCrLf & "(" & CStr(Er.Number) & ") " & Er.Description
			Next 
		End If
		GetErrorDetails = ErrMsg
	End Function
	
	'_________________________________________________________
	'
	'関数名     OpenRs
	'機能       レコードを開く
	'引数       なし
	'戻り値     true    正常終了
	'           false   異常終了
	'_________________________________________________________
	Public Function OpenRs(ByRef Source As String, ByRef Cn As ADODB.Connection, Optional ByRef CursorType As ADODB.CursorTypeEnum = ADODB.CursorTypeEnum.adOpenForwardOnly, Optional ByRef LockType As ADODB.LockTypeEnum = ADODB.LockTypeEnum.adLockReadOnly, Optional ByRef Options As Integer = -1) As ADODB.Recordset
		
		Dim RST As ADODB.Recordset
		
		RST = New ADODB.Recordset
		RST.Open(Source, Cn, CursorType, LockType, Options)
		OpenRs = RST
		'    RST.Close
		'    Set RST = Nothing
	End Function
	
	Public Sub ReleaseRs(ByRef rs As ADODB.Recordset)
		On Error Resume Next
		rs.Close()
		'UPGRADE_NOTE: オブジェクト rs をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		rs = Nothing
		On Error GoTo 0
	End Sub
	
	Public Function SQLIntRange(ByRef FieldName As String, Optional ByVal TextFrom As Object = Nothing, Optional ByVal TextTo As Object = Nothing, Optional ByVal wsType As wsTypes = wsTypes.wsTypeACC, Optional ByRef CharJoin As Boolean = False) As String
		
		Dim Criteria As String
		
		'UPGRADE_NOTE: IsMissing() は IsNothing() に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="8AE1CB93-37AB-439A-A4FF-BE3B6760BB23"' をクリックしてください。
		If IsNothing(TextFrom) Then
			'UPGRADE_NOTE: IsMissing() は IsNothing() に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="8AE1CB93-37AB-439A-A4FF-BE3B6760BB23"' をクリックしてください。
			If IsNothing(TextTo) Then
				SQLIntRange = ""
				Exit Function
			ElseIf IsCheckNull(TextTo) Then 
				SQLIntRange = ""
				Exit Function
			Else
				'UPGRADE_WARNING: オブジェクト TextTo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト TextFrom の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				TextFrom = TextTo
			End If
		ElseIf IsCheckNull(TextFrom) Then 
			'UPGRADE_NOTE: IsMissing() は IsNothing() に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="8AE1CB93-37AB-439A-A4FF-BE3B6760BB23"' をクリックしてください。
			If IsNothing(TextTo) Then
				SQLIntRange = ""
				Exit Function
			ElseIf IsCheckNull(TextTo) Then 
				SQLIntRange = vbNullString
				Exit Function
			Else
				If CharJoin Then
					Criteria = " and"
				End If
				'UPGRADE_WARNING: オブジェクト TextTo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				SQLIntRange = Criteria & " " & FieldName & "<=" & Trim(TextTo)
				Exit Function
			End If
		Else
			'UPGRADE_NOTE: IsMissing() は IsNothing() に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="8AE1CB93-37AB-439A-A4FF-BE3B6760BB23"' をクリックしてください。
			If IsNothing(TextTo) Then
				'UPGRADE_WARNING: オブジェクト TextFrom の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト TextTo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				TextTo = TextFrom
			ElseIf IsCheckNull(TextTo) Then 
				If CharJoin Then
					Criteria = " and"
				End If
				'UPGRADE_WARNING: オブジェクト TextFrom の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				SQLIntRange = Criteria & " " & FieldName & ">=" & Trim(TextFrom)
				Exit Function
			End If
		End If
		
		If CharJoin Then
			Criteria = " and"
		End If
		'大小が逆の場合
		'    If CInt(TextFrom) > CInt(TextTo) Then       '2002/09/19 UPD
		'UPGRADE_WARNING: オブジェクト TextTo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト TextFrom の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If CInt(TextFrom) > CInt(TextTo) Then '2005/03/03 UPD
			'UPGRADE_WARNING: オブジェクト TextFrom の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト TextTo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			SQLIntRange = Criteria & " " & FieldName & ">=" & Trim(TextTo) & " And " & FieldName & "<=" & Trim(TextFrom)
		Else
			'UPGRADE_WARNING: オブジェクト TextTo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト TextFrom の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			SQLIntRange = Criteria & " " & FieldName & ">=" & Trim(TextFrom) & " And " & FieldName & "<=" & Trim(TextTo)
		End If
	End Function
	'2003/04/28 ADD
	Public Function SQLCurRange(ByRef FieldName As String, Optional ByVal TextFrom As Object = Nothing, Optional ByVal TextTo As Object = Nothing, Optional ByVal wsType As wsTypes = wsTypes.wsTypeACC, Optional ByRef CharJoin As Boolean = False) As String
		
		Dim Criteria As String
		
		'UPGRADE_NOTE: IsMissing() は IsNothing() に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="8AE1CB93-37AB-439A-A4FF-BE3B6760BB23"' をクリックしてください。
		If IsNothing(TextFrom) Then
			'UPGRADE_NOTE: IsMissing() は IsNothing() に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="8AE1CB93-37AB-439A-A4FF-BE3B6760BB23"' をクリックしてください。
			If IsNothing(TextTo) Then
				SQLCurRange = ""
				Exit Function
			ElseIf IsCheckNull(TextTo) Then 
				SQLCurRange = ""
				Exit Function
			Else
				'UPGRADE_WARNING: オブジェクト TextTo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト TextFrom の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				TextFrom = TextTo
			End If
		ElseIf IsCheckNull(TextFrom) Then 
			'UPGRADE_NOTE: IsMissing() は IsNothing() に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="8AE1CB93-37AB-439A-A4FF-BE3B6760BB23"' をクリックしてください。
			If IsNothing(TextTo) Then
				SQLCurRange = ""
				Exit Function
			ElseIf IsCheckNull(TextTo) Then 
				SQLCurRange = vbNullString
				Exit Function
			Else
				If CharJoin Then
					Criteria = " and"
				End If
				'UPGRADE_WARNING: オブジェクト TextTo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				SQLCurRange = Criteria & " " & FieldName & "<=" & Trim(CStr(CDec(TextTo)))
				Exit Function
			End If
		Else
			'UPGRADE_NOTE: IsMissing() は IsNothing() に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="8AE1CB93-37AB-439A-A4FF-BE3B6760BB23"' をクリックしてください。
			If IsNothing(TextTo) Then
				'UPGRADE_WARNING: オブジェクト TextFrom の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト TextTo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				TextTo = TextFrom
			ElseIf IsCheckNull(TextTo) Then 
				If CharJoin Then
					Criteria = " and"
				End If
				'UPGRADE_WARNING: オブジェクト TextFrom の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				SQLCurRange = Criteria & " " & FieldName & ">=" & Trim(CStr(CDec(TextFrom)))
				Exit Function
			End If
		End If
		
		If CharJoin Then
			Criteria = " and"
		End If
		'大小が逆の場合
		'UPGRADE_WARNING: オブジェクト TextTo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト TextFrom の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If CDec(TextFrom) > CDec(TextTo) Then
			'UPGRADE_WARNING: オブジェクト TextFrom の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト TextTo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			SQLCurRange = Criteria & " " & FieldName & ">=" & Trim(CStr(CDec(TextTo))) & " And " & FieldName & "<=" & Trim(CStr(CDec(TextFrom)))
		Else
			'UPGRADE_WARNING: オブジェクト TextTo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト TextFrom の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			SQLCurRange = Criteria & " " & FieldName & ">=" & Trim(CStr(CDec(TextFrom))) & " And " & FieldName & "<=" & Trim(CStr(CDec(TextTo)))
		End If
	End Function
	
	Public Function SQLQuoteString(ByRef code As Object) As String
		'UPGRADE_WARNING: オブジェクト code の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		If IsDbNull(code) Or Trim(code) = vbNullString Then
			SQLQuoteString = "Null"
			Exit Function
		End If
		SQLQuoteString = "'" & SQLString(code) & "'"
	End Function
	
	Public Function SQLString(ByRef code As Object) As String
		'*******************************************************************************************
		'         文字列内のダブルクォーテーションチェックルーチン
		'*******************************************************************************************
		' 書式        st = SQLString(CODE)
		'  値
		'             CODE … 調べたい文字列
		' 戻り値      変換されたコード
		'*******************************************************************************************
		Dim cnt As Integer 'ダブルクォーテーションの存在する桁位置
		Dim st As String '変換後の文字列
		Dim i As Short
		Dim X As Object
		Dim Y As Object
		
		If IsCheckNull(code) Then
			Exit Function
		End If
		
		'UPGRADE_WARNING: オブジェクト code の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		code = Trim(code)
		
		'UPGRADE_WARNING: オブジェクト code の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If InStr(code, "'") = 0 Then
			'UPGRADE_WARNING: オブジェクト code の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			SQLString = code
			Exit Function
		End If
		
		st = ""
		'UPGRADE_WARNING: オブジェクト X の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		X = "'"
		
		For i = 1 To Len(code)
			'UPGRADE_WARNING: オブジェクト code の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト Y の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			Y = Mid(code, i, 1)
			'UPGRADE_WARNING: オブジェクト Y の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト X の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If StrComp(X, Y, 0) = 0 Then
				'UPGRADE_WARNING: オブジェクト Y の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				st = st & Y & "'"
			Else
				'UPGRADE_WARNING: オブジェクト Y の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				st = st & Y
			End If
		Next i
		
		SQLString = st
	End Function
	
	Public Function SQLStringRange(ByRef FieldName As String, Optional ByVal TextFrom As Object = Nothing, Optional ByVal TextTo As Object = Nothing, Optional ByVal wsType As wsTypes = wsTypes.wsTypeACC, Optional ByRef CharJoin As Boolean = False) As String
		
		Dim Criteria As String
		
		'UPGRADE_NOTE: IsMissing() は IsNothing() に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="8AE1CB93-37AB-439A-A4FF-BE3B6760BB23"' をクリックしてください。
		If IsNothing(TextFrom) Then
			'UPGRADE_NOTE: IsMissing() は IsNothing() に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="8AE1CB93-37AB-439A-A4FF-BE3B6760BB23"' をクリックしてください。
			If IsNothing(TextTo) Then
				SQLStringRange = vbNullString
				Exit Function
				'UPGRADE_WARNING: オブジェクト TextTo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			ElseIf IsCheckNull(TextTo) Or TextTo = "最後" Then 
				SQLStringRange = vbNullString
				Exit Function
			Else
				'UPGRADE_WARNING: オブジェクト TextTo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト TextFrom の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				TextFrom = TextTo
			End If
			'UPGRADE_WARNING: オブジェクト TextFrom の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		ElseIf IsCheckNull(TextFrom) Or TextFrom = "最初" Then 
			'UPGRADE_NOTE: IsMissing() は IsNothing() に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="8AE1CB93-37AB-439A-A4FF-BE3B6760BB23"' をクリックしてください。
			If IsNothing(TextTo) Then
				SQLStringRange = ""
				Exit Function
				'UPGRADE_WARNING: オブジェクト TextTo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			ElseIf IsCheckNull(TextTo) Or TextTo = "最後" Then 
				SQLStringRange = ""
				Exit Function
			Else
				If CharJoin Then
					Criteria = " and"
				End If
				SQLStringRange = Criteria & " " & FieldName & "<='" & SQLString(TextTo) & "'"
				Exit Function
			End If
		Else
			'UPGRADE_NOTE: IsMissing() は IsNothing() に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="8AE1CB93-37AB-439A-A4FF-BE3B6760BB23"' をクリックしてください。
			If IsNothing(TextTo) Then
				'UPGRADE_WARNING: オブジェクト TextFrom の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト TextTo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				TextTo = TextFrom
				'UPGRADE_WARNING: オブジェクト TextTo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			ElseIf IsCheckNull(TextTo) Or TextTo = "最後" Then 
				If CharJoin Then
					Criteria = " and"
				End If
				SQLStringRange = Criteria & " " & FieldName & ">='" & SQLString(TextFrom) & "'"
				Exit Function
			End If
		End If
		
		If CharJoin Then
			Criteria = " and"
		End If
		'大小が逆の場合
		'UPGRADE_WARNING: オブジェクト TextTo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト TextFrom の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If TextFrom > TextTo Then
			SQLStringRange = Criteria & " " & FieldName & ">='" & SQLString(TextTo) & "' And " & FieldName & "<='" & SQLString(TextFrom) & "'"
		Else
			SQLStringRange = Criteria & " " & FieldName & ">='" & SQLString(TextFrom) & "' And " & FieldName & "<='" & SQLString(TextTo) & "'"
		End If
	End Function
	
	Public Function SQLDate(ByRef d As Object, Optional ByVal wsType As wsTypes = wsTypes.wsTypeACC) As String
		If Not IsDate(d) Then
			SQLDate = "Null"
			Exit Function
		End If
		If wsType = wsTypes.wsTypeACC Then
			'UPGRADE_WARNING: オブジェクト d の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			SQLDate = VB6.Format(d, "\#yyyy/mm/dd\#")
		Else
			'UPGRADE_WARNING: オブジェクト d の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			SQLDate = VB6.Format(d, "\'yyyy/mm/dd\'")
		End If
	End Function
	
	Public Function SQLDateRange(ByRef FieldName As String, Optional ByVal DateFrom As Object = Nothing, Optional ByVal DateTo As Object = Nothing, Optional ByVal wsType As wsTypes = wsTypes.wsTypeACC, Optional ByRef CharJoin As Boolean = False) As String
		'時刻まで含む項目に対する条件文字列を返します
		Dim Criteria As String
		Dim TmpDate As Object 'Day+1の日付
		
		'UPGRADE_NOTE: IsMissing() は IsNothing() に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="8AE1CB93-37AB-439A-A4FF-BE3B6760BB23"' をクリックしてください。
		If IsNothing(DateFrom) Then
			'UPGRADE_NOTE: IsMissing() は IsNothing() に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="8AE1CB93-37AB-439A-A4FF-BE3B6760BB23"' をクリックしてください。
			If IsNothing(DateTo) Then
				SQLDateRange = vbNullString
				Exit Function
			ElseIf IsCheckNull(DateTo) Then 
				SQLDateRange = vbNullString
				Exit Function
			Else
				'UPGRADE_WARNING: オブジェクト DateTo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト DateFrom の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				DateFrom = DateTo
			End If
		ElseIf IsCheckNull(DateFrom) Then 
			'UPGRADE_NOTE: IsMissing() は IsNothing() に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="8AE1CB93-37AB-439A-A4FF-BE3B6760BB23"' をクリックしてください。
			If IsNothing(DateTo) Then
				SQLDateRange = vbNullString
				Exit Function
			ElseIf IsCheckNull(DateTo) Then 
				SQLDateRange = vbNullString
				Exit Function
			Else
				If CharJoin Then
					Criteria = " and"
				End If
				On Error Resume Next '2002/05/03 ADD
				'UPGRADE_WARNING: オブジェクト DateTo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト TmpDate の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				TmpDate = DateAdd(Microsoft.VisualBasic.DateInterval.Day, 1, DateTo)
				If Err.Number Then
					Err.Clear()
					On Error GoTo 0
					'UPGRADE_WARNING: オブジェクト DateTo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト TmpDate の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					TmpDate = DateTo
					SQLDateRange = Criteria & " " & FieldName & "<=" & SQLDate(TmpDate, wsType)
				Else
					Err.Clear()
					On Error GoTo 0
					SQLDateRange = Criteria & " " & FieldName & "<" & SQLDate(TmpDate, wsType)
					'''                SQLDateRange = Criteria & " " & FieldName & "<" & SQLDate(DateAdd("d", 1, DateTo), wsType)
				End If '2002/05/03 ADD
				Exit Function
			End If
		Else
			'UPGRADE_NOTE: IsMissing() は IsNothing() に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="8AE1CB93-37AB-439A-A4FF-BE3B6760BB23"' をクリックしてください。
			If IsNothing(DateTo) Then
				'UPGRADE_WARNING: オブジェクト DateFrom の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト DateTo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				DateTo = DateFrom
			ElseIf IsCheckNull(DateTo) Then 
				If CharJoin Then
					Criteria = " and"
				End If
				SQLDateRange = Criteria & " " & FieldName & ">=" & SQLDate(DateFrom, wsType)
				Exit Function
			End If
		End If
		
		If CharJoin Then
			Criteria = " and"
		End If
		'日付の大小が逆の場合
		'UPGRADE_WARNING: オブジェクト DateTo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト DateFrom の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If DateFrom > DateTo Then
			On Error Resume Next '2002/05/03 ADD
			'UPGRADE_WARNING: オブジェクト DateFrom の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト TmpDate の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			TmpDate = DateAdd(Microsoft.VisualBasic.DateInterval.Day, 1, DateFrom)
			If Err.Number Then
				Err.Clear()
				On Error GoTo 0
				'UPGRADE_WARNING: オブジェクト DateFrom の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト TmpDate の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				TmpDate = DateFrom
				SQLDateRange = Criteria & " " & FieldName & ">=" & SQLDate(DateTo, wsType) & " And " & FieldName & "<=" & SQLDate(TmpDate, wsType)
			Else
				Err.Clear()
				On Error GoTo 0
				SQLDateRange = Criteria & " " & FieldName & ">=" & SQLDate(DateTo, wsType) & " And " & FieldName & "<" & SQLDate(TmpDate, wsType)
				'''        SQLDateRange = Criteria & " " & FieldName & ">=" & SQLDate(DateTo, wsType) & " And " & FieldName & "<" & SQLDate(DateAdd("d", 1, DateFrom), wsType)
			End If
		Else
			On Error Resume Next '2002/05/03 ADD
			'UPGRADE_WARNING: オブジェクト DateTo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト TmpDate の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			TmpDate = DateAdd(Microsoft.VisualBasic.DateInterval.Day, 1, DateTo)
			If Err.Number Then
				Err.Clear()
				On Error GoTo 0
				'UPGRADE_WARNING: オブジェクト DateTo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト TmpDate の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				TmpDate = DateTo
				SQLDateRange = Criteria & " " & FieldName & ">=" & SQLDate(DateFrom, wsType) & " And " & FieldName & "<=" & SQLDate(TmpDate, wsType)
			Else
				Err.Clear()
				On Error GoTo 0
				SQLDateRange = Criteria & " " & FieldName & ">=" & SQLDate(DateFrom, wsType) & " And " & FieldName & "<" & SQLDate(TmpDate, wsType)
				'''        SQLDateRange = Criteria & " " & FieldName & ">=" & SQLDate(DateFrom, wsType) & " And " & FieldName & "<" & SQLDate(DateAdd("d", 1, DateTo), wsType)
			End If
		End If
	End Function
	
	Public Function SQLBetween(ByRef FieldName As String, ByRef DateFrom As Object, ByRef DateTo As Object, Optional ByRef Prefix As String = "", Optional ByVal wsType As wsTypes = wsTypes.wsTypeACC) As String
		Dim strTmp As String
		strTmp = ""
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		If IsDbNull(DateFrom) Then
			'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
			If Not IsDbNull(DateTo) Then
				If IsDate(DateTo) Then
					strTmp = "<= " & SQLDate(DateTo, wsType)
				End If
			End If
		ElseIf IsDate(DateFrom) Then 
			'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
			If IsDbNull(DateTo) Then
				strTmp = ">= " & SQLDate(DateFrom, wsType)
			ElseIf IsDate(DateTo) Then 
				'日付の大小が逆の場合
				'UPGRADE_WARNING: オブジェクト DateTo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト DateFrom の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				If DateFrom > DateTo Then
					strTmp = "Between " & SQLDate(DateTo, wsType) & " And " & SQLDate(DateFrom, wsType) & " "
				Else
					strTmp = "Between " & SQLDate(DateFrom, wsType) & " And " & SQLDate(DateTo, wsType) & " "
				End If
			End If
		End If
		If strTmp <> "" Then
			strTmp = Prefix & " " & FieldName & " " & strTmp
		End If
		SQLBetween = strTmp
	End Function
	
	Public Function DownloadRecords(ByRef SourceRs As ADODB.Recordset, ByRef ReceiveRs As ADODB.Recordset) As Integer
		'ダウンロードレコードセットと受信用レコードセットの双方に存在する同一名のフィールドについて
		'ダウンロードレコードセットからレコードを取り出し受信用レコードセットへ格納します
		'戻り値     >0: 格納レコード数
		'           -1: 一致するフィールド名がない
		
		Dim fld() As ADODB.Field
		Dim CopyFieldsCount, i As Short
		Dim Records As Integer
		
		DownloadRecords = 0
		If SourceRs.EOF Then
			Exit Function
		End If
		With ReceiveRs
			CopyFieldsCount = 0
			ReDim fld(1, .Fields.Count - 1)
			On Error Resume Next
			For i = 0 To .Fields.Count - 1
				Err.Clear()
				fld(0, CopyFieldsCount) = SourceRs.Fields(.Fields(i).Name)
				If Err.Number = 0 Then
					fld(1, CopyFieldsCount) = .Fields(i)
					CopyFieldsCount = CopyFieldsCount + 1
				End If
			Next 
			On Error GoTo 0
		End With
		If CopyFieldsCount = 0 Then
			DownloadRecords = -1
		Else
			Do Until SourceRs.EOF
				ReceiveRs.AddNew()
				For i = 0 To CopyFieldsCount - 1
					fld(1, i).Value = fld(0, i).Value
				Next 
				ReceiveRs.Update()
				DownloadRecords = DownloadRecords + 1
				SourceRs.MoveNext()
			Loop 
		End If
	End Function
End Module