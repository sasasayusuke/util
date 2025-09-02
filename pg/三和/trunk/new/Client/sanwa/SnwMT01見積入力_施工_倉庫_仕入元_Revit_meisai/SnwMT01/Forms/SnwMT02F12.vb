Option Strict Off
Option Explicit On

''' <summary>
''' --------------------------------------------------------------------
'''   ユーザー名           株式会社三和商研
'''   業務名               積算データ管理システム
'''   部門名               見積部門
'''   プログラム名         員数取込画面（レコードセット）
'''   作成会社             テクノウェア株式会社
'''   作成日               2004/02/16
'''   作成者               oosawa
''' --------------------------------------------------------------------
''' 
'''        2018/11/03  oosawa      F12員数取込で行指定を追加
''' --------------------------------------------------------------------
''' </summary>
Friend Class SnwMT02F12
	Inherits System.Windows.Forms.Form

	Dim pParentForm As SnwMT02F00
	Dim pActRow As Integer

	Dim grs As ADODB.Recordset
	Dim PreviousControl As System.Windows.Forms.Control 'コントロールの戻りを制御
	Dim ReturnF As Boolean 'リターンキー時（確定時）True
	Dim SelectF As Boolean

	'選択したコードを送るコントロールをセット
	WriteOnly Property ResParentForm() As System.Windows.Forms.Form
		Set(ByVal Value As System.Windows.Forms.Form)
			pParentForm = Value
		End Set
	End Property

	'行項目をセット
	WriteOnly Property ActRow() As Integer
		Set(ByVal Value As Integer)
			pActRow = Value
		End Set
	End Property

	Private Sub SnwMT02F12_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Me.Load
		'フォームを画面の中央に配置
		Me.Top = VB6Conv.TwipsToPixelsY((VB6Conv.PixelsToTwipsY(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height) - VB6Conv.PixelsToTwipsY(Me.Height)) \ 2)
		Me.Left = VB6Conv.TwipsToPixelsX((VB6Conv.PixelsToTwipsX(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width) - VB6Conv.PixelsToTwipsX(Me.Width)) \ 2)

		' フォームでキー入力を受け取れるようにする
		Me.KeyPreview = True
	End Sub

	Private Sub SnwMT02F12_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
		' Graphics オブジェクトを取得
		Dim g As Graphics = e.Graphics

		' DPIを取得
		Dim dpiX As Single = g.DpiX ' 水平方向のDPI
		Dim dpiY As Single = g.DpiY ' 垂直方向のDPI

		' 1ポイントをピクセルに変換
		Dim lineWidth As Single = 1 * (dpiX / 72)

		Dim pen As New Pen(Color.Black, lineWidth)

		' 線を描く、取込行数
		g.DrawLine(pen, 32, 82, 150, 82)

		' リソースを解放
		pen.Dispose()
	End Sub

	Private Sub SnwMT02F12_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
		Dim Cancel As Boolean = e.Cancel
		Dim UnloadMode As System.Windows.Forms.CloseReason = e.CloseReason
		pParentForm.Enabled = True
		pParentForm.Activate()
		'UPGRADE_NOTE: オブジェクト pParentForm をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		pParentForm = Nothing
		ReleaseRs(grs)
		'UPGRADE_NOTE: オブジェクト PreviousControl をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		PreviousControl = Nothing
		e.Cancel = Cancel
		Me.Dispose()
	End Sub

	Private Sub Cb中止_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Cb中止.Click
		Me.Close()
	End Sub

	Private Sub CbGET_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CbGET.Click
		Dim RecCnt As Integer
		Dim wMsg As String

		If Item_Check((CbGET.TabIndex)) = False Then
			Exit Sub
		End If

		If MsgBoxResult.No = YesNo("指定の員数シートを取込みます。") Then Exit Sub

		'    RecCnt = 員数取込(tx_見積番号.Text)
		RecCnt = 員数取込(CInt([tx_見積番号].Text), ([tx_s行].Text), ([tx_e行].Text)) '2018/11/03 ADD
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
				If MsgBoxResult.Yes = Question(pActRow + 1 & "行目に指定の員数シートを取込みます。" & wMsg & "適用しますか？" & vbCrLf & "", MsgBoxResult.Yes) Then
					'親のメソッドで処理をする。
					'UPGRADE_ISSUE: Control DspGetData は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
					pParentForm.DspGetData(grs, pActRow)
					'UPGRADE_NOTE: オブジェクト grs をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
					grs = Nothing
					Me.Close()
					Exit Sub
				End If
		End Select

	End Sub

	Private Sub tx_見積番号_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_見積番号.Enter
		If Item_Check(([tx_見積番号].TabIndex)) = False Then
			Exit Sub
		End If

		'    sb_Msg.Panels(1).Text = "見積番号を入力して下さい。　選択画面：Space"
		PreviousControl = Me.ActiveControl
	End Sub

	Private Sub tx_見積番号_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_見積番号.KeyPress
		Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
		'2008/01/23 ADD↓
		If KeyAscii = Asc(" ") And ([tx_見積番号].SelStart = 0 And [tx_見積番号].SelLength = Len([tx_見積番号].Text)) Then
			KeyAscii = 0
			'---参照画面表示
			SelectF = True
			SnwMT03F00S.ResCodeCTL = [tx_見積番号]
			'VB6.ShowFxorm(SnwMT03F00S, VB6.FormShowConstants.Modal, Me)
			SnwMT03F00S.ShowDialog(Me)
			If [tx_見積番号].Tag <> "" Then
				ReturnF = True
				[tx_見積番号].NextSetFocus()
			Else
				[tx_見積番号].Focus()
			End If
		End If
		'2008/01/23 ADD↑
	End Sub

	Private Sub tx_見積番号_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_見積番号.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_見積番号_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_見積番号.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		sb_Msg.Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_見積番号.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_見積番号].Undo()
		End If
		ReturnF = False
	End Sub

	'Private Function 員数取込(MituNo As Long) As Long
	Private Function 員数取込(ByRef MituNo As Integer, ByRef sGyo As Object, ByRef eGyo As Object) As Integer
		Dim rs As ADODB.Recordset
		Dim sql As String
		Dim wMeisai As Object
		Dim wLine As Integer
		Dim i As Integer

		On Error GoTo Download_Err
		'マウスポインターを砂時計にする
		HourGlass(True)

		員数取込 = 0

		'---見積明細セット
		'    sql = "SELECT * " _
		'        & "FROM VD見積シートM AS MM " _
		'        & "WHERE MM.見積番号 = " & MituNo _
		'        & " ORDER BY 行番号"
		'2018/11/03 ADD
		sql = "SELECT * "
		sql = sql & " FROM VD見積シートM AS MM"
		sql = sql & " WHERE MM.見積番号 = " & MituNo
		'UPGRADE_WARNING: オブジェクト sGyo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'sql = sql & " AND MM.行番号 >= COALESCE(" & SpcToNull(CType(sGyo, Integer).ToString("#"), "Null") & ",MM.行番号)"
		If sGyo = "" Then
			sql = sql & " AND MM.行番号 >= COALESCE(" & "Null" & ",MM.行番号)"
		Else
			sql = sql & " AND MM.行番号 >= COALESCE(" & CType(sGyo, Integer).ToString("#") & ",MM.行番号)"
		End If
		'UPGRADE_WARNING: オブジェクト eGyo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'sql = sql & " AND MM.行番号 <= COALESCE(" & SpcToNull(CType(eGyo, Integer).ToString("#"), "Null") & ",MM.行番号)"
		If sGyo = "" Then
			sql = sql & " AND MM.行番号 <= COALESCE(" & "Null" & ",MM.行番号)"
		Else
			sql = sql & " AND MM.行番号 <= COALESCE(" & CType(eGyo, Integer).ToString("#") & ",MM.行番号)"
		End If
		sql = sql & " ORDER BY 行番号"


		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly) 'RecordCountを取る為

		With rs
			If .EOF Then
				員数取込 = -1
			Else
				員数取込 = rs.RecordCount
			End If
		End With

		If 員数取込 > 0 Then

			grs = rs

		End If

		HourGlass(False)
		Exit Function
Download_Err:
		Call ReleaseRs(rs)
		MsgBox(Err.Number & " " & Err.Description)
		HourGlass(False)
	End Function

	Private Function Item_Check(ByRef ItemNo As Integer) As Boolean

		On Error GoTo Item_Check_Err
		Item_Check = False

		'見積番号のチェック
		If ItemNo > [tx_見積番号].TabIndex Then
			'UPGRADE_WARNING: オブジェクト NullToZero(tx_見積番号, 0) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If NullToZero([tx_見積番号].Text, 0) = 0 Then
				CriticalAlarm("見積番号を入力して下さい。")
				[tx_見積番号].Focus()
				Exit Function
			End If
			'        '見積データ使用チェック
			'        If LockData("見積番号", tx_見積番号, , 1) = False Then
			'            HourGlass False
			'            Exit Function
			'        End If
		End If

		Item_Check = True

		Exit Function
Item_Check_Err:
		MsgBox(Err.Number & " " & Err.Description)
	End Function

End Class