Option Strict Off
Option Explicit On

''' <summary>
''' 客先在庫履歴表示
''' </summary>
Friend Class SnwMT02F09
	Inherits System.Windows.Forms.Form

	'Dim ResultCodeSetControl As fpSpread     '選択したコードの送り先をセットする。
	'ｽﾌﾟﾚｯﾄﾞのクラス
	'UPGRADE_NOTE: clsSPD は clsSPD_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
	Dim clsSPD As clsSPD

	'Dim m_指定日付 As Date          '指定日付
	Dim m_指定日付 As Object '指定日付
	Dim m_担当者CD As Short '担当者CD
	Dim m_得意先CD As String '得意先CD
	Dim m_製品NO As String '製品NO
	Dim m_仕様NO As String '仕様NO
	Dim m_製品名 As String '製品名
	'Dim ResultZaikosu As Currency           '社内在庫数
	'
	'選択したコードを送るコントロールをセット
	'Property Set ResCodeCTL(ByRef ctl As Control)
	'    Set ResultCodeSetControl = ctl
	'End Property

	'指定日付をセット
	WriteOnly Property 指定日付() As Object
		Set(ByVal Value As Object)
			'Property Let 指定日付(ByRef New_指定日付 As Date)
			'UPGRADE_WARNING: オブジェクト New_指定日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト m_指定日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			m_指定日付 = Value
		End Set
	End Property

	'担当者CDをセット
	WriteOnly Property 担当者CD() As Short
		Set(ByVal Value As Short)
			m_担当者CD = Value
		End Set
	End Property

	'得意先CDをセット
	WriteOnly Property 得意先CD() As String
		Set(ByVal Value As String)
			m_得意先CD = Value
		End Set
	End Property

	'製品NOをセット
	WriteOnly Property 製品NO() As String
		Set(ByVal Value As String)
			m_製品NO = Value
		End Set
	End Property

	'仕様NOをセット
	WriteOnly Property 仕様NO() As String
		Set(ByVal Value As String)
			m_仕様NO = Value
		End Set
	End Property

	'製品名をセット
	WriteOnly Property 製品名() As String
		Set(ByVal Value As String)
			m_製品名 = Value
		End Set
	End Property

	Private Sub SnwMT02F09_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Me.Load
		'Dim fpSpd As Object
		Dim buf As String
		buf = ""

		'SPREAD設定
		clsSPD = New clsSPD
		clsSPD.CtlSpd = fpSpd

		If Get担当者DB(CStr(m_担当者CD), buf) = True Then
			rf_担当者名.Text = buf
		Else
			rf_担当者名.Text = ""
		End If

		rf_得意先名1.Text = HD_得意先名1
		rf_得意先名2.Text = HD_得意先名2
		rf_製品NO.Text = m_製品NO
		rf_仕様NO.Text = m_仕様NO
		rf_製品名.Text = m_製品名
		'    tx_社内在庫数.Text = ResultZaikosu

		'シートのクリア
		Call clsSPD.SprClearText()

		Call Download()
	End Sub

	Private Sub SnwMT02F09_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
		Dim Cancel As Boolean = e.Cancel
		Dim UnloadMode As System.Windows.Forms.CloseReason = e.CloseReason
		'UPGRADE_NOTE: オブジェクト clsSPD をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		clsSPD = Nothing

		e.Cancel = Cancel
		Me.Dispose()
	End Sub

	Private Sub CdOK_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CdOK.Click
		'    ResultCodeSetControl.Text = tx_社内在庫数.Text
		'UPGRADE_NOTE: オブジェクト clsSPD をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		clsSPD = Nothing
		Me.Close()
	End Sub

	Private Function Get担当者DB(ByRef ID As String, ByRef IDName As String) As Boolean
		Dim rs As ADODB.Recordset
		Dim sql As String

		On Error GoTo Get担当者DB_Err

		'マウスポインターを砂時計にする
		HourGlass(True)

		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		If IsDBNull(ID) Or Trim(ID) = vbNullString Then
			'        IDName1 = vbNullString
			IDName = "未設定"
			HourGlass(False)
			Exit Function
		End If

		sql = "SELECT 担当者名 FROM TM担当者 " & "WHERE 担当者CD = '" & SQLString(Trim(ID)) & "'"
		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)

		With rs
			If .EOF Then
				'            IDName = "未登録"
				Get担当者DB = False
			Else
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				IDName = NullToZero(.Fields("担当者名").Value, vbNullString)
				Get担当者DB = True
			End If
		End With
		Call ReleaseRs(rs)

		HourGlass(False)
		Exit Function
Get担当者DB_Err:
		MsgBox(Err.Number & " " & Err.Description)
		HourGlass(False)
	End Function

	Public Function Download() As Boolean
		Dim cmd As New ADODB.Command
		Dim rs As ADODB.Recordset

		'マウスポインターを砂時計にする
		HourGlass(True)

		'---コマンドパラメータ設定
		' コマンドを実行する接続先を指定する
		cmd.let_ActiveConnection(Cn)
		cmd.CommandTimeout = 0
		cmd.CommandText = "usp_MT0111客先在庫履歴表示"
		cmd.CommandType = ADODB.CommandTypeEnum.adCmdStoredProc

		' それぞれのパラメータの値を指定する
		With cmd.Parameters

			'UPGRADE_WARNING: オブジェクト m_指定日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@i指定日付").Value = m_指定日付

			.Item("@i担当者CD").Value = m_担当者CD
			.Item("@i得意先CD").Value = m_得意先CD

			'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
			.Item("@i製品NO").Value = If(m_製品NO = "", System.DBNull.Value, m_製品NO)
			'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
			.Item("@i仕様NO").Value = If(m_仕様NO = "", System.DBNull.Value, m_仕様NO)

		End With

		Cn.CursorLocation = ADODB.CursorLocationEnum.adUseClient 'MoveLastを使用する場合

		rs = New ADODB.Recordset
		'---コマンド実行
		rs = cmd.Execute

		Cn.CursorLocation = ADODB.CursorLocationEnum.adUseServer

		If rs.State <> 0 Then
			If rs.EOF Then
				Download = False
			Else
				Download = True
				Call SetupItems(rs)
			End If
		Else
			Download = False
			CriticalAlarm((cmd.Parameters("@RetST").Value & " : " & cmd.Parameters("@RetMsg").Value))
		End If

		'UPGRADE_NOTE: オブジェクト cmd をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		cmd = Nothing

		On Error GoTo 0
		ReleaseRs(rs)
		Call HourGlass(False)
		Exit Function

Download_Err:
		CriticalAlarm(Err.Number & " " & Err.Description)
		'UPGRADE_NOTE: オブジェクト cmd をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		cmd = Nothing
		ReleaseRs(rs)
		Call HourGlass(False)
	End Function

	Private Sub SetupItems(ByRef rs As ADODB.Recordset)
		'Dim fpSpd As Object
		Dim RecArry(,) As Object
		Dim i, j As Integer

		Dim GOUKEI As Decimal
		GOUKEI = 0

		'UPGRADE_WARNING: Array に新しい動作が指定されています。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト fpSpd.MaxRows の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト rs.GetRows() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		RecArry = rs.GetRows(FpSpd.ActiveSheet.RowCount,  , New Object() {"日付", "見積番号", "行番号", "見積件名", "残数", "受注数", "売上数", "入庫数"})

		For i = 0 To UBound(RecArry, 2)
			For j = 0 To UBound(RecArry)
				Select Case j + 1
					Case 2 To 3
						'UPGRADE_WARNING: オブジェクト RecArry() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						FpSpd.ActiveSheet.SetText(j + 1, i + 1, If(Trim("" & RecArry(j, i)) = "0", "", Trim("" & RecArry(j, i))))
					Case 5 To 10
						'UPGRADE_WARNING: オブジェクト RecArry() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						FpSpd.ActiveSheet.SetText(j + 1, i + 1, If(Trim("" & RecArry(j, i)) = "0", "", Trim("" & RecArry(j, i))))
					Case Else
						'UPGRADE_WARNING: オブジェクト RecArry() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						FpSpd.ActiveSheet.SetText(j + 1, i + 1, Trim("" & RecArry(j, i)))
				End Select
			Next
			'合計数
			'UPGRADE_WARNING: オブジェクト RecArry(7, i) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト RecArry(6, i) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト RecArry(5, i) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト RecArry() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			GOUKEI = GOUKEI + RecArry(4, i) - RecArry(5, i) - RecArry(6, i) + RecArry(7, i)
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			FpSpd.ActiveSheet.SetText(12, i + 1, GOUKEI)
		Next

		'    fpSpd.Col = 1
		'    fpSpd.Row = 1
		'    fpSpd.Action = ActionActiveCell
		'
	End Sub
End Class