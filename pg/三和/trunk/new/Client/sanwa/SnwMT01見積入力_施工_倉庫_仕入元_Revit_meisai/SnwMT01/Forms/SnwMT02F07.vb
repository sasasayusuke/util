Option Strict Off
Option Explicit On

''' <summary>
''' --------------------------------------------------------------------
'''   ユーザー名           株式会社三和商研
'''   業務名               積算データ管理システム
'''   部門名               見積部門
'''   プログラム名         見積入力処理（見積コピー処理）
'''   作成会社             テクノウェア株式会社
'''   作成日               2003/07/25
'''   作成者               kawamura
''' --------------------------------------------------------------------
''' </summary>
Friend Class SnwMT02F07
	Inherits System.Windows.Forms.Form

	Dim SelectF As Boolean '選択画面制御用
	Dim ReturnF As Boolean 'リターンキー時（確定時）True

	Dim pParentForm As SnwMT01F00S
	Dim pMituNo As Integer
	Dim pMituNM As String
	Dim pTOKUCD As String '2005/07/04 ADD

	'新見積番号
	Dim wNUMBER As Integer

	Dim PreviousControl As System.Windows.Forms.Control 'コントロールの戻りを制御
	'データHold用ワーク
	Dim HTOKUCD As Object
	'クラス
	Private cTokuisaki As clsTokuisaki

	Private Sub SnwMT02F07_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Me.Load
		Dim buf1 As String '名前取得用バッファ
		Dim buf2 As String
		buf1 = ""
		buf2 = ""

		On Error GoTo SysErr_Form_Load

		'フォームを画面の中央に配置
		Me.Top = VB6Conv.TwipsToPixelsY((VB6Conv.PixelsToTwipsY(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height) - VB6Conv.PixelsToTwipsY(Me.Height)) \ 2)
		Me.Left = VB6Conv.TwipsToPixelsX((VB6Conv.PixelsToTwipsX(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width) - VB6Conv.PixelsToTwipsX(Me.Width)) \ 2)

		'クラス生成
		cTokuisaki = New clsTokuisaki

		rf_見積番号.Text = pMituNo.ToString("#")
		rf_見積件名.Text = pMituNM

		[tx_得意先CD].Text = pTOKUCD '2005/07/04 ADD
		'UPGRADE_WARNING: オブジェクト HTOKUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HTOKUCD = [tx_得意先CD].Text

		If Get得意先DB([tx_得意先CD].Text, buf1, buf2) <> 0 Then
			[rf_得意先名1].Text = vbNullString
			[rf_得意先名2].Text = vbNullString
		Else
			[rf_得意先名1].Text = buf1
			[rf_得意先名2].Text = buf2
		End If

		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HTOKUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HTOKUCD = System.DBNull.Value '2005/07/04 ADD

		' フォームでキー入力を受け取れるようにする
		Me.KeyPreview = True

		Exit Sub
SysErr_Form_Load:
		MsgBox(Err.Number & " " & Err.Description)
	End Sub

	Private Sub SnwMT02F07_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
		' Graphics オブジェクトを取得
		Dim g As Graphics = e.Graphics

		' DPIを取得
		Dim dpiX As Single = g.DpiX ' 水平方向のDPI
		Dim dpiY As Single = g.DpiY ' 垂直方向のDPI

		' 1ポイントをピクセルに変換
		Dim lineWidth As Single = 1 * (dpiX / 72)

		Dim pen As New Pen(Color.Black, lineWidth)

		' 線を描く、見積番号
		g.DrawLine(pen, 10, 55, 180, 55)

		' 線を描く、見積件名
		g.DrawLine(pen, 10, 84, 340, 84)

		' 線を描く、得意先指定
		g.DrawLine(pen, 10, 130, 340, 130)

		' リソースを解放
		pen.Dispose()
	End Sub

	Private Sub SnwMT02F07_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
		Dim Cancel As Boolean = e.Cancel
		Dim UnloadMode As System.Windows.Forms.CloseReason = e.CloseReason
		'UPGRADE_NOTE: オブジェクト pParentForm をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		pParentForm = Nothing
		e.Cancel = Cancel
		Me.Dispose()
	End Sub

	Private Sub tx_得意先CD_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_得意先CD.Enter
		If SelectF = False Then
			PreviousControl = Me.ActiveControl
		End If
		SelectF = False
		'    lb_Footer.Caption = "得意先CDを入力して下さい。　選択画面：Space"
	End Sub

	Private Sub tx_得意先CD_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_得意先CD.KeyPress
		Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
		If KeyAscii = Asc(" ") And ([tx_得意先CD].SelStart = 0 And [tx_得意先CD].SelLength = Len([tx_得意先CD].Text)) Then
			KeyAscii = 0
			'---参照画面表示
			SelectF = True
			If cTokuisaki.ShowDialog = True Then
				[tx_得意先CD].Text = cTokuisaki.得意先CD
				ReturnF = True
				[tx_得意先CD].NextSetFocus()
			Else
				[tx_得意先CD].Focus()
			End If
		End If
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub

	Private Sub tx_得意先CD_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_得意先CD.KeyDown
		If [tx_得意先CD].Text = "" Then
			'KeyCode = 0
			'---参照画面表示
			SelectF = True
			If cTokuisaki.ShowDialog = True Then
				[tx_得意先CD].Text = cTokuisaki.得意先CD
				[tx_得意先CD].NextSetFocus()
			Else
				[tx_得意先CD].Focus()
			End If
		End If

		ReturnF = True
	End Sub

	Private Sub tx_得意先CD_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_得意先CD.Leave
		'    lb_Footer.Caption = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_得意先CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_得意先CD].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub CbClose_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CbClose.Click
		Me.Close()
	End Sub

	Private Sub CbCopy_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CbCopy.Enter
		If Item_Check((CbCopy.TabIndex)) = False Then
			Exit Sub
		End If
	End Sub

	Private Sub CbCopy_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CbCopy.Click
		If Item_Check((CbCopy.TabIndex)) = False Then
			Exit Sub
		End If

		If MsgBox("指定された見積№をコピーします。", MsgBoxStyle.Question + MsgBoxStyle.OkCancel, Me.Text) = MsgBoxResult.Ok Then
			'コピー処理
			If CopySyori() = True Then
				System.Windows.Forms.Application.DoEvents()
				Inform("終了しました。" & vbCrLf & vbCrLf & "見積番号[" & wNUMBER & "]にコピーされました。")
				'UPGRADE_ISSUE: Control Download は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
				pParentForm.Download()
				'UPGRADE_ISSUE: Control SetupItems は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
				pParentForm.SetupItems()
				Me.Close()
			End If
		End If
	End Sub

	Private Function CopySyori() As Boolean
		Dim cmd As New ADODB.Command

		CopySyori = False

		HourGlass(True)

		' コマンドを実行する接続先を指定する
		cmd.let_ActiveConnection(Cn)
		cmd.CommandTimeout = 0 '2007/09/14 ADD
		cmd.CommandText = "usp_MT0100CPY見積"
		cmd.CommandType = ADODB.CommandTypeEnum.adCmdStoredProc 'これを書くとtextが編集される

		' それぞれのパラメータの値を指定する
		With cmd.Parameters
			.Item(0).Direction = ADODB.ParameterDirectionEnum.adParamReturnValue
			.Item("@i見積番号").Value = rf_見積番号.Text
			.Item("@i得意先CD").Value = [tx_得意先CD].Text '2005/07/04 ADD
			.Item("@RetNUMBER").Direction = ADODB.ParameterDirectionEnum.adParamOutput
		End With

		cmd.Execute()

		If cmd.Parameters(0).Value <> 0 Then
			CriticalAlarm((cmd.Parameters("@RetST").Value & " : " & cmd.Parameters("@RetMsg").Value))
		End If

		wNUMBER = cmd.Parameters("@RetNUMBER").Value

		'UPGRADE_NOTE: オブジェクト cmd をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		cmd = Nothing
		CopySyori = True

		HourGlass(False)

	End Function

	Private Function Item_Check(ByRef ItemNo As Integer) As Boolean
		Dim buf1 As String '名前取得用バッファ
		Dim buf2 As String
		buf1 = ""
		buf2 = ""

		Dim Chk_ID As String 'ﾁｪｯｸ用ワーク

		On Error GoTo Item_Check_Err
		Item_Check = False

		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HTOKUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'If ItemNo > [tx_得意先CD].TabIndex And ([tx_得意先CD].Text <> HTOKUCD Or IsDBNull(HTOKUCD)) Then
		Dim StrHTOKUCD As String = If(IsDBNull(HTOKUCD), String.Empty, HTOKUCD.ToString())
		If ItemNo > [tx_得意先CD].TabIndex And ([tx_得意先CD].Text <> StrHTOKUCD) Then
			If IsCheckText([tx_得意先CD]) = False Then
				CriticalAlarm("得意先CDが未入力です。")
				'UPGRADE_WARNING: オブジェクト tx_得意先CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				[tx_得意先CD].Undo()
				[tx_得意先CD].Focus()
				Exit Function
			End If

			'--- 入力値をチェック用ワークへ格納
			If ISInt(([tx_得意先CD].Text)) Then
				'UPGRADE_WARNING: TextBox プロパティ tx_得意先CD.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				Chk_ID = CType([tx_得意先CD].Text, Integer).ToString(New String("0"c, [tx_得意先CD].MaxLength))
			Else
				Chk_ID = [tx_得意先CD].Text
			End If

			'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト HTOKUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'If (Chk_ID <> HTOKUCD) Or IsDBNull(HTOKUCD) Then
			If (Chk_ID <> StrHTOKUCD) Then
				'--得意先Ｍ情報セット
				If Get得意先DB(Chk_ID, buf1, buf2) <> 0 Then
					CriticalAlarm("指定の得意先CDは未登録です。")
					'UPGRADE_WARNING: オブジェクト tx_得意先CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					[tx_得意先CD].Undo()
					[tx_得意先CD].Focus()
					Exit Function
				Else
					[rf_得意先名1].Text = buf1
					[rf_得意先名2].Text = buf2
				End If
			End If

			'--- 入力値をワークへ格納
			[tx_得意先CD].Text = Chk_ID
			'UPGRADE_WARNING: オブジェクト HTOKUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			HTOKUCD = [tx_得意先CD].Text
		End If
		Item_Check = True

		Exit Function
Item_Check_Err:
		MsgBox(Err.Number & " " & Err.Description)
	End Function

	Private Function Get得意先DB(ByVal ID As String, ByRef IDName1 As String, ByRef IDName2 As String) As Short
		Dim rs As ADODB.Recordset
		Dim sql As String

		On Error GoTo Get得意先DB_Err
		'マウスポインターを砂時計にする
		HourGlass(True)

		sql = "SELECT 得意先名1, 得意先名2 " & "FROM TM得意先 " & "WHERE 得意先CD = '" & SQLString(Trim(ID)) & "'"
		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)

		With rs
			If .EOF Then
				'            IDName = "未登録"
				Get得意先DB = -1
			Else
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				IDName1 = NullToZero(.Fields("得意先名1").Value, vbNullString)
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				IDName2 = NullToZero(.Fields("得意先名2").Value, vbNullString)
				Get得意先DB = 0
			End If
		End With
		ReleaseRs(rs)

		HourGlass(False)
		Exit Function
Get得意先DB_Err:
		MsgBox(Err.Number & " " & Err.Description)
		HourGlass(False)
	End Function

	'選択したコードを送るコントロールをセット
	WriteOnly Property ResParentForm() As System.Windows.Forms.Form
		Set(ByVal Value As System.Windows.Forms.Form)
			pParentForm = Value
		End Set
	End Property

	'表示項目をセット
	WriteOnly Property MituNo() As Integer
		Set(ByVal Value As Integer)
			pMituNo = Value
		End Set
	End Property

	'表示項目をセット
	WriteOnly Property MituNM() As String
		Set(ByVal Value As String)
			pMituNM = Value
		End Set
	End Property

	'表示項目をセット
	WriteOnly Property TOKUCD() As String
		Set(ByVal Value As String) '2005/07/04 ADD
			pTOKUCD = Value
		End Set
	End Property
End Class