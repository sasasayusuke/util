Option Strict Off
Option Explicit On

Imports ADODB
Imports System.Runtime.InteropServices
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Chrome
Imports OpenQA.Selenium.Edge
Imports OpenQA.Selenium.Interactions
Imports OpenQA.Selenium.Support.UI
Imports Microsoft.Win32

''' <summary>
''' --------------------------------------------------------------------
'''   ユーザー名           株式会社三和商研
'''   業務名               積算・販売管理システム
'''   部門名
'''   プログラム名
'''   作成会社             テクノウェア株式会社
'''   作成日               2003/07/16
'''   作成者               kawamura
''' --------------------------------------------------------------------
'''   ﾒﾝﾃ版　日付        担当者       内容
'''      01  2003/09/30  kawamura    プログラムの起動をShellExecuteExに変更
'''                                  終了するまで監視を行い、全ＰＧが終了していないとメッセージを表示
'''      02  2004/12/01  kawamura    「月次締め処理を追加」
'''      02  2004/12/16  oosawa      「発注一覧表出力処理を追加」
'''      03  2006/07/05  kawamura    「月別締別売上集計表を追加」
'''      04  2009/03/17  oosawa      「納品書用CSV出力処理」を追加
'''      05  2009/09/18  kawamura    追加
''' 
''' 
'''        2012/05/07  oosawa          統計メニュー作成
'''                                    得意先別年間売上順位推移表・仕入別年間仕入順位推移表　追加
'''        2012/05/18  oosawa          担当者別仕入先原価推移表作成 追加
'''        2012/07/31  oosawa          経費データ取込処理追加
'''        2012/08/07  oosawa          支払予定表処理追加
'''        2012/08/10  oosawa          入金消込入力処理追加
'''        2012/08/20  oosawa          未回収一覧表追加
'''        2012/08/29  oosawa          消込済一覧表追加
'''        2012/08/31  oosawa          担当者別得意先別物件別売上推移表追加
'''        2012/09/14  oosawa          物件種別得意先別売上推移表追加
'''        2012/11/06  oosawa          担当者別売上仕入推移表追加
'''        2013/04/03  oosawa          担当者別店舗別原価売価積上表追加
'''        2013/05/01  oosawa          請求明細一覧表追加
'''        2013/05/17  oosawa          担当者別在庫金額増減推移表追加
'''        2013/08/28  oosawa          得意先別売上仕入推移表
'''        2014/02/14  oosawa          担当者別売上仕入推移表TK06からTK10に変更（名称も担当者別推移表に変更）
'''        2014/08/25  oosawa          担当者別売上仕入推移表TK10からTK11に変更（旧担当者別推移表も表示）
'''        2015/02/26  oosawa          チーム別推移表TK18からTK19へ変更
'''        2015/03/04  oosawa          TK14からTK20へ変更
'''        2015/05/20  oosawa          入金予定表追加
'''        2015/06/11  oosawa          ウエルシア物件内容マスタ追加
'''        2015/09/02  oosawa          消費税調整入力を追加
'''        2015/10/21  oosawa          YK請求明細表追加
'''        2015/10/23  oosawa          MM請求明細表追加
'''        2015/11/26  oosawa          見積明細一覧追加
'''        2016/03/10  oosawa          拓用に制御
'''        2016/07/12  oosawa          発注一覧表を復活
'''        2016/07/16  oosawa          TK22・TK23
'''        2016/12/25  oosawa          パラメータを設定できるように
'''        2017/05/31  oosawa          月ズレチェック表追加
'''        2017/12/05  oosawa          「受注明細票」の名称を「発注書」へ変更
'''        2018/02/19  oosawa          統計表、新組織に変更TK29
'''        2018/03/29  oosawa          メニューのロックを追加
'''        2021/02/13  oosawa          統計変更・按分変更
'''        2022/09/07  oosawa          統計変更2022・按分変更2022 Part2
'''        2023/03/08  oosawa          表題にDB名を表示
'''        2023/04/21  oosawa          統計変更2023・按分変更2023
'''        2023/07/25  oosawa          見積出力・発注書出力　できないように Enable=false
'''        2023/09/27  oosawa          統計集計2023年10月以降追加
''' -------------------------------------------------------------------- 
''' </summary>
Friend Class SnwA01F00
	Inherits System.Windows.Forms.Form

	''''Private Declare Function GetWindowLong Lib "user32" Alias "GetWindowLongA" (ByVal Hwnd As Long, ByVal nIndex As Long) As Long
	''''Private Declare Function SetWindowLong Lib "user32" Alias "SetWindowLongA" (ByVal Hwnd As Long, ByVal nIndex As Long, ByVal dwNewLong As Long) As Long

	<DllImport("user32.dll", SetLastError:=True)>
	Private Shared Function GetWindowLong(ByVal hWnd As IntPtr, ByVal nIndex As Integer) As Integer
	End Function

	<DllImport("user32.dll", SetLastError:=True)>
	Private Shared Function SetWindowLong(ByVal hWnd As IntPtr, ByVal nIndex As Integer, ByVal dwNewLong As Integer) As Integer
	End Function

	''''Const GWL_STYLE = (-16)
	''''Const WS_MAXIMIZEBOX = &H10000
	''''Const WS_MINIMIZEBOX = &H20000
	''''Const WS_THICKFRAME = &H40000
	Private Const GWL_STYLE As Integer = -16
	Private Const WS_MINIMIZEBOX As Integer = &H20000
	Private Const WS_MAXIMIZEBOX As Integer = &H10000
	Private Const WS_THICKFRAME As Integer = &H40000

	'ﾀｽｸﾊﾞｰを除いた画面領域を取得する。
	'Private Declare Function SystemParametersInfo Lib "user32" Alias "SystemParametersInfoA" (ByVal uAction As Long, ByVal uParam As Long, ByRef lpvParam As Any, ByVal fuWinIni As Long) As Long
	<DllImport("user32.dll")>
	Private Shared Function SystemParametersInfo(ByVal uiAction As Integer, ByVal uiParam As Integer, ByRef pvParam As RECT, ByVal fWinIni As Integer) As Boolean
	End Function

	'Private Const SPI_GETWORKAREA = 48
	Private Const SPI_GETWORKAREA As Integer = 48

	'Private Type RECT
	'    Left As Long
	'    Top As Long
	'    Right As Long
	'    Bottom As Long
	'End Type
	<StructLayout(LayoutKind.Sequential)>
	Private Structure RECT
		Public Left As Integer
		Public Top As Integer
		Public Right As Integer
		Public Bottom As Integer
	End Structure

	Private Loaded As Boolean
	Private CONNECT As String

	'起動されたEXEのカウント
	Dim gPGCnt As Long

	'起動時のフォームサイズHold用
	'Dim MeWidth As Long, MeHeight As Long
	Private MeWidth As Integer
	Private MeHeight As Integer

	'Private Type PRGINF
	'    PRGTAB  As String               'Tab番号セット
	'    PRGEXE  As String               'EXE名セット
	'    PRGLAST As String               'Tab最終ボタンか？
	'    PRGPARAM As String              'パラメータ2016/12/25 ADD
	'End Type

	Public Structure PRGINF
		Public PRGTAB As String          ' Tab番号セット
		Public PRGEXE As String          ' EXE名セット
		Public PRGLAST As String         ' Tab最終ボタンか？
		Public PRGPARAM As String        ' パラメータ2016/12/25 ADD
	End Structure

	Dim Prg_Name(0 To 9999) As PRGINF   'プログラム用配列(0ｵﾘｼﾞﾝ：３桁目はタブ数を採用(1タブ100ボタン))

	Private Sub SnwA01F00_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

		Loaded = False

		Dim rcWorkArea As RECT
		SystemParametersInfo(SPI_GETWORKAREA, 0, rcWorkArea, 0)

		Me.Top = rcWorkArea.Top
		Me.Left = rcWorkArea.Left
		'Me.Width = rcWorkArea.Right * Screen.PrimaryScreen.Bounds.Width / SystemInformation.WorkingArea.Width
		'Me.Height = rcWorkArea.Bottom * Screen.PrimaryScreen.Bounds.Height / SystemInformation.WorkingArea.Height

		' 最小化ボタンの消去
		' Dim lngStyle As Integer = GetWindowLong(Me.Handle, GWL_STYLE)
		' SetWindowLong(Me.Handle, GWL_STYLE, lngStyle And Not WS_MINIMIZEBOX)

		' 最大化ボタンの消去
		' lngStyle = GetWindowLong(Me.Handle, GWL_STYLE)
		' SetWindowLong(Me.Handle, GWL_STYLE, lngStyle And Not WS_MAXIMIZEBOX)

		' 最大化ボタンの消去
		' lngStyle = GetWindowLong(Me.Handle, GWL_STYLE)
		' SetWindowLong(Me.Handle, GWL_STYLE, lngStyle And Not WS_THICKFRAME)

		' フォームを画面の中央に配置
		Me.Top = (Screen.PrimaryScreen.Bounds.Height - Me.Height) \ 2
		Me.Left = (Screen.PrimaryScreen.Bounds.Width - Me.Width) \ 2

		' コネクションストリングの保持
		CONNECT = My.Settings.AdodbConnectionString
		Dim blRet As Boolean
		blRet = DBStd.DBOpen(My.Settings.AdodbConnectionString, Cn, True)

		' 2018/03/29 ADD
		LockMenu()
		' リサイズ用初期値設定（幅・高さ）
		MeWidth = Me.Width
		MeHeight = Me.Height

		' DB名表示
		Me.Text = Me.Text & "-" & Cn.DefaultDatabase ' 2023/03/08 ADD

		' 端末のロック情報を削除する
		UnLockAllData()

		' DBへの接続解除
		ApplicationUnload()

		' プログラムのEXE名を配列に格納
		Prg_Set()

		' Tb_Menu.TabVisible(6) = False

		' タブMenuの未使用分の削除
		With Tb_Menu
			' タブコントロール内のすべてのタブページを配列に格納
			Dim tabPagesArray As TabPage() = .TabPages.Cast(Of TabPage)().ToArray()
			For Each tabPage As TabPage In tabPagesArray
				Select Case tabPage.Text
					Case "仕入・支払"
						.TabPages.Remove(tabPage)
					Case "売上・入金"
						.TabPages.Remove(tabPage)
					Case "請求・売掛"
						.TabPages.Remove(tabPage)
					Case "支払・買掛"
						.TabPages.Remove(tabPage)
					Case "在庫"
						.TabPages.Remove(tabPage)
					Case "統計"
						.TabPages.Remove(tabPage)
					Case "経費"
						.TabPages.Remove(tabPage)
					Case "マスタ１"
						.TabPages.Remove(tabPage)
					Case "マスタ２"
						.TabPages.Remove(tabPage)
					Case "保守"
						.TabPages.Remove(tabPage)
				End Select
			Next
		End With

		' 2016/03/10 ADD
		' 得意先：三和商研3055の制御
		If GetIni("Disp", "VisibleTAKU", INIFile) = "FALSE" Then
			With Tb_Menu
				' UPGRADE_WARNING .TabVisible(1) = False
				' UPGRADE_WARNING .TabVisible(2) = False
				' UPGRADE_WARNING .TabVisible(3) = False
				' UPGRADE_WARNING .TabVisible(4) = False
				' UPGRADE_WARNING .TabVisible(5) = False
				' UPGRADE_WARNING .TabVisible(6) = False
				' UPGRADE_WARNING .TabVisible(7) = False
				' .TabVisible(8) = False
				' UPGRADE_WARNING .TabVisible(9) = False
				' UPGRADE_WARNING .TabVisible(10) = False

				' 配列を使って各タブページの Enabled を設定
				Dim tabPagesArray As TabPage() = .TabPages.Cast(Of TabPage)().ToArray()
				For Each tabPage As TabPage In tabPagesArray
					If tabPage.Text = "見積・発注" Then '(0)
						tabPage.Enabled = True  ' 有効化
					ElseIf tabPage.Text = "経費" Then   '(8)
						tabPage.Enabled = True  ' 有効化
					ElseIf tabPage.Text = "保守" Then   '(11)
						tabPage.Enabled = True  ' 有効化
					Else
						tabPage.Enabled = False ' 無効化
					End If
				Next
			End With
			'NOTE SS .Itemは廃止の為、コメント
			'With cbStart
			'.Item(1).Visible = False
			' .Item(2).Visible = False
			' .Item(3).Visible = False
			' NOTE SS .Item(4).Visible = False
			' .Item(5).Visible = False '2016/07/12 DEL
			' NOTE SS .Item(7).Visible = False
			'End With
		End If
		' 2016/03/10 ADD

		' ToolStripの編集
		'sb_Msg_Panel1.Text = "てすと"
		sb_Msg_Panel2.Text = DateTime.Now.ToString("yyyy/MM/dd")
		sb_Msg_Panel3.Text = DateTime.Now.ToString("HH:mm")

		' タイマーの間隔を設定 (10秒ごとに更新)
		Timer1.Interval = 10000
		Timer1.Start() ' タイマーを開始

		Loaded = True

	End Sub

	Private Sub SnwA01F00_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
		Dim KeyCode As Short = e.KeyCode
		Dim Shift As Short = e.KeyData \ &H10000
		Dim ctl As System.Windows.Forms.Control

		On Error GoTo Form_KeyDown_Err
		Select Case KeyCode
			Case System.Windows.Forms.Keys.Escape
				KeyCode = 0
				Exit Sub
			Case System.Windows.Forms.Keys.F12
				On Error Resume Next
				If KeyCode = System.Windows.Forms.Keys.F12 Then
					' cbClose のクリックイベントを呼び出す
					CbClose.PerformClick()
					'NOTE SS 未通過 Else
					'NOTE SS 未通過     ctl = Me.Controls("cbFunc" & (KeyCode - Keys.F1 + 1).ToString())
				End If
				'NOTE SS 未通過 If Err.Number = 0 Then
				'NOTE SS 未通過     If ctl IsNot Nothing Then
				'NOTE SS 未通過         If ctl.Text <> String.Empty Then
				'NOTE SS 未通過             If ctl.Enabled = True Then
				'NOTE SS 未通過                 ctl.Focus()
				'NOTE SS 未通過                 If Err.Number = 0 Then
				'NOTE SS 未通過                     SendReturnKey()
				'NOTE SS 未通過                 End If
				'NOTE SS 未通過             End If
				'NOTE SS 未通過         End If
				'NOTE SS 未通過     End If
				'NOTE SS 未通過 End If
				'UPGRADE_NOTE: オブジェクト ctl をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
				ctl = Nothing
				KeyCode = 0
				On Error GoTo 0
				Exit Sub
		End Select

		'統計メニュー表示
		'    Select Case KeyCode
		'        Case vbKeyV
		'            If Shift = (vbShiftMask Or vbCtrlMask) Then
		'                KeyCode = 0
		'                If NoYes("統計メニューを表示します。") = vbYes Then
		'                    Tb_Menu.TabVisible(6) = True
		'                End If
		'            End If
		'    End Select

		'    'かくしきのう
		'    Select Case KeyCode
		'        Case vbKeyV
		'            If Shift = (vbAltMask Or vbCtrlMask) Then
		'                KeyCode = 0
		'                If NoYes("表示します。") = vbYes Then
		'                        cbStart(800).Visible = True
		''                        cbStart(810).Visible = True
		'                        Inform "表示しました。"
		'                End If
		'            End If
		'    End Select

		Exit Sub
Form_KeyDown_Err:
		MsgBox(Err.Number & " " & Err.Description)
	End Sub

	Private Sub SnwA01F00_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		'ビープ音消去用
		Select Case KeyAscii
			Case System.Windows.Forms.Keys.Return
				KeyAscii = 0
		End Select
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub

	'UPGRADE_WARNING: イベント SnwA01F00.Resize は、フォームが初期化されたときに発生します。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"' をクリックしてください。
	Private Sub SnwA01F00_Resize(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Resize
	End Sub

	Private Sub SnwA01F00_FormClosing(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
		Dim Cancel As Boolean = eventArgs.Cancel
		Dim UnloadMode As System.Windows.Forms.CloseReason = eventArgs.CloseReason
		'Dim gPGCnt As Object
		If Loaded Then
			'UPGRADE_WARNING: オブジェクト gPGCnt の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If gPGCnt <> 0 Then
				Inform("実行中のプログラムがある為、終了することが出来ません。" & vbCrLf & "実行中のプログラムを閉じてから終了して下さい。")
				Cancel = True
			Else
				If MsgBoxResult.Yes = NoYes("処理を終了します。") Then
				Else
					Cancel = True
				End If
			End If
		End If
		eventArgs.Cancel = Cancel
	End Sub

	Private Sub CbClose_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CbClose.Click
		Me.Close()
	End Sub

	Private Sub Prg_Set()
		'Dim Prg_Name As Object
		'On Error Resume Next

		'---見積------------------
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(6).PRGTAB = 0
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(6).PRGEXE = "SnwMT00.exe" '物件入力
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(6).PRGLAST = 1

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(1).PRGTAB = 0
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(1).PRGEXE = "SnwMT03.exe" '見積書出力
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(1).PRGLAST = 0

		'2016/12/25 ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(7).PRGTAB = 0
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(7).PRGEXE = "SnwMT04.exe" '部材リスト
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(7).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(2).PRGTAB = 0
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(2).PRGEXE = "SnwMT04v2.exe" '部材リスト新型
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(2).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(3).PRGTAB = 0
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(3).PRGEXE = "SnwHC02.exe" '受注明細票出力
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(3).PRGLAST = 0

		''    Prg_Name(4).PRGTAB = 0
		''    Prg_Name(4).PRGEXE = "SnwHC02B.exe"     'オカム用発注書出力
		''    Prg_Name(4).PRGLAST = 2

		'2004/12/16 ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(5).PRGTAB = 0
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(5).PRGEXE = "SnwHC03.exe" '発注一覧表出力
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(5).PRGLAST = 2

		'2015/11/26 ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(0).PRGTAB = 0
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(0).PRGEXE = "SnwMT05.exe" '見積明細一覧
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(0).PRGLAST = 0

		'2016/12/25 ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(8).PRGTAB = 0
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(8).PRGEXE = "SnwMT01.exe" '見積入力
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(8).PRGLAST = 0

		'2018/07/31 ADD↓
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(9).PRGTAB = 0
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(9).PRGEXE = "SnwMT06.exe" '社内伝入荷リスト
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(9).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(10).PRGTAB = 0
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(10).PRGEXE = "SnwMT03_01.exe" '見積書出力（屋外広告物専用）
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(10).PRGLAST = 0
		'2018/07/31 ADD↑

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(11).PRGTAB = 0
		''    Prg_Name(11).PRGEXE = "SnwTN27.exe"     'しまむら消耗品受注取込
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(11).PRGEXE = "SnwMT03_02.exe" '見積書出力（コーセー専用）
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(11).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(12).PRGTAB = 0
		'    Prg_Name(12).PRGEXE = "SnwTN28.exe"     'しまむら消耗品受注リスト
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(12).PRGEXE = "SnwMT11.exe" '注文書出力'2023/10/16 ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(12).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(13).PRGTAB = 0
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(13).PRGEXE = "SnwMT09.exe" '見積依頼書出力
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(13).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(14).PRGTAB = 0
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(14).PRGEXE = "SnwMT12.exe" '出荷リスト出力
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(14).PRGLAST = 0



		'---仕入・支払------------
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(100).PRGTAB = 1
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(100).PRGEXE = "SnwHS01.exe" '仕入入力
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(100).PRGLAST = 1

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(101).PRGTAB = 1
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(101).PRGEXE = "SnwHS02.exe" '仕入日計表
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(101).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(102).PRGTAB = 1
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(102).PRGEXE = "SnwSD01.exe" '支払入力
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(102).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(103).PRGTAB = 1
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(103).PRGEXE = "SnwSD02.exe" '支払日計表
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(103).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(104).PRGTAB = 1
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(104).PRGEXE = "SnwSD03.exe" '支払予定表
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(104).PRGLAST = 2

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(105).PRGTAB = 1
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(105).PRGEXE = "SnwHS01-1.exe" '仕入入力
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(105).PRGLAST = 2
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGPARAM の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(105).PRGPARAM = "通常伝"

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(106).PRGTAB = 1
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(106).PRGEXE = "SnwHS01-1.exe" '仕入入力
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(106).PRGLAST = 2
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGPARAM の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(106).PRGPARAM = "社内伝"

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(107).PRGTAB = 1
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(107).PRGEXE = "SnwSD04.exe" '支払データ取込処理
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(107).PRGLAST = 2

		'---売上・入金-------------
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(200).PRGTAB = 2
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(200).PRGEXE = "SnwHD07.exe" '売上入力
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(200).PRGLAST = 1

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(201).PRGTAB = 2
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(201).PRGEXE = "SnwHD08.exe" '納品書発行
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(201).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(202).PRGTAB = 2
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(202).PRGEXE = "SnwHD09.exe" '売上日計表
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(202).PRGLAST = 0

		'    Prg_Name(203).PRGTAB = 2
		'    Prg_Name(203).PRGEXE = "SnwHD04.exe"     '売上仕入日計累計表
		'    Prg_Name(203).PRGLAST = 0
		'
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(204).PRGTAB = 2
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(204).PRGEXE = "SnwND01.exe" '入金入力
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(204).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(205).PRGTAB = 2
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(205).PRGEXE = "SnwND02.exe" '入金チェックリスト
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(205).PRGLAST = 2
		'2009/04/17 ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(206).PRGTAB = 2
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(206).PRGEXE = "SnwHD11.exe" '納品書用CSV出力処理
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(206).PRGLAST = 2
		'2009/09/18 ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(207).PRGTAB = 2
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(207).PRGEXE = "SnwHD06.exe" '仮納品書発行
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(207).PRGLAST = 2

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(208).PRGTAB = 2
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(208).PRGEXE = "SnwND06.exe" '入金消込入力
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(208).PRGLAST = 2

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(209).PRGTAB = 2
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(209).PRGEXE = "SnwND07.exe" '未回収一覧表
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(209).PRGLAST = 2

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(210).PRGTAB = 2
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(210).PRGEXE = "SnwND08.exe" '消込済一覧表
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(210).PRGLAST = 2

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(211).PRGTAB = 2
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(211).PRGEXE = "SnwND09.exe" '入金予定表    2015/05/20ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(211).PRGLAST = 2

		'---請求・売掛---------------
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(310).PRGTAB = 3
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(310).PRGEXE = "SnwSE09.exe" '請求確定
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(310).PRGLAST = 1

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(300).PRGTAB = 3
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(300).PRGEXE = "SnwSE08.exe" '請求書発行
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(300).PRGLAST = 1

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(301).PRGTAB = 3
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(301).PRGEXE = "SnwSE10.exe" '請求鏡発行
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(301).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(302).PRGTAB = 3
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(302).PRGEXE = "SnwSE03.exe" '手入力鏡発行
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(302).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(303).PRGTAB = 3
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(303).PRGEXE = "SnwSE11.exe" '売掛金元帳
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(303).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(304).PRGTAB = 3
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(304).PRGEXE = "SnwSE05.exe" '請求一覧表
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(304).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(309).PRGTAB = 3
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(309).PRGEXE = "SnwSE13.exe" '請求明細一覧表     '2013/05/01 ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(309).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(305).PRGTAB = 3
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(305).PRGEXE = "SnwHK03.exe" '売掛金集計表
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(305).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(306).PRGTAB = 3
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(306).PRGEXE = "SnwHK04.exe" '得意先別売上集計表
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(306).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(308).PRGTAB = 3
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(308).PRGEXE = "SnwSE12.exe" '月別締別売上集計表     '2006/07/05.ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(308).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(310).PRGTAB = 3
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(310).PRGEXE = "SnwHK05.exe" '売掛明細一覧表 '2015/01/14 ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(310).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(307).PRGTAB = 3
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(307).PRGEXE = "SnwGE01.exe" '月次締め処理   '2004/12/01.ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(307).PRGLAST = 2


		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(311).PRGTAB = 3
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(311).PRGEXE = "SnwSE14.exe" 'ウエルシア請求明細 '2015/06/15 ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(311).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(312).PRGTAB = 3
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(312).PRGEXE = "SnwHK06.exe" '売掛消費税調整入力 '2015/09/02 ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(312).PRGLAST = 0

		'    Prg_Name(313).PRGTAB = 3
		'    Prg_Name(313).PRGEXE = "SnwSE15.exe"    'しまむら消耗品請求書発行 '2015/10/04 ADD
		'    Prg_Name(313).PRGLAST = 0
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(313).PRGTAB = 3
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(313).PRGEXE = "SnwSE24.exe" 'ジョイテック請求書発行 '2022/10/20 ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(313).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(314).PRGTAB = 3
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(314).PRGEXE = "SnwSE16.exe" 'しまむら消耗品請求書発行 '2015/10/04 ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(314).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(315).PRGTAB = 3
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(315).PRGEXE = "SnwSE17.exe" 'ウエルシア中日施工売価原価集計表 '2015/10/13 ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(315).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(316).PRGTAB = 3
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(316).PRGEXE = "SnwSE19.exe" 'YK請求明細表'2015/10/21 ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(316).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(317).PRGTAB = 3
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(317).PRGEXE = "SnwSE20.exe" 'MM請求明細表'2015/10/23 ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(317).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(318).PRGTAB = 3
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(318).PRGEXE = "SnwSE22.exe" 'ウエルシア調剤請求一覧'2016/01/05 ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(318).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(319).PRGTAB = 3
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(319).PRGEXE = "SnwSE21.exe" 'ベルク合計請求書'2016/01/05 ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(319).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(320).PRGTAB = 3
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(320).PRGEXE = "SnwHD12.exe" 'YKﾒﾝﾃ用請求CSV出力'2016/03/09 ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(320).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(321).PRGTAB = 3
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(321).PRGEXE = "SnwSE25.exe" '請求原価率表 '2023/03/23 ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(321).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(322).PRGTAB = 3
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(322).PRGEXE = "SnwSE08_01.exe" '請求書発行(屋外広告物専用) '2023/08/29 ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(322).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(323).PRGTAB = 3
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(323).PRGEXE = "SnwSE08_02.exe" '請求書発行(コーセー専用) '2023/08/29 ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(323).PRGLAST = 0


		'---支払・買掛---------------
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(400).PRGTAB = 4
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(400).PRGEXE = "SnwSH01.exe" '買掛金元帳
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(400).PRGLAST = 1

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(401).PRGTAB = 4
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(401).PRGEXE = "SnwSH02.exe" '検収一覧表
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(401).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(402).PRGTAB = 4
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(402).PRGEXE = "SnwSK01.exe" '買掛金集計表
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(402).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(403).PRGTAB = 4
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(403).PRGEXE = "SnwSK02.exe" '取引先別仕入集計表
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(403).PRGLAST = 2

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(404).PRGTAB = 4
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(404).PRGEXE = "SnwSH03.exe" 'チーム別仕入先検収明細表
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(404).PRGLAST = 2

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(405).PRGTAB = 4
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(405).PRGEXE = "SnwSK03.exe" 'チーム別月ズレチェック表'2017/05/31 ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(405).PRGLAST = 2

		'---在庫--------------------
		''---2009/09/18 DEL
		''    Prg_Name(500).PRGTAB = 5
		''    Prg_Name(500).PRGEXE = "SnwTN01.exe"    '社内在庫調整入力
		''    Prg_Name(500).PRGLAST = 1
		''
		''    Prg_Name(501).PRGTAB = 5
		''    Prg_Name(501).PRGEXE = "SnwTN02.exe"    '社内在庫表
		''    Prg_Name(501).PRGLAST = 0
		''
		''    Prg_Name(502).PRGTAB = 5
		''    Prg_Name(502).PRGEXE = "SnwTN03.exe"    '客先在庫調整入力
		''    Prg_Name(502).PRGLAST = 0
		''
		''    Prg_Name(503).PRGTAB = 5
		''    Prg_Name(503).PRGEXE = "SnwTN04.exe"    '客先在庫表
		''    Prg_Name(503).PRGLAST = 0
		''
		''    Prg_Name(504).PRGTAB = 5
		''    Prg_Name(504).PRGEXE = "SnwZK01.exe"    '在庫予定表
		''    Prg_Name(504).PRGLAST = 2
		''
		'2015/09/22 DEL↓
		'''''---2009/09/18 ADD
		''''    Prg_Name(500).PRGTAB = 5
		''''    Prg_Name(500).PRGEXE = "SnwTN05.exe"    '担当者別社内在庫棚卸入力
		''''    Prg_Name(500).PRGLAST = 1
		''''
		''''    Prg_Name(501).PRGTAB = 5
		''''    Prg_Name(501).PRGEXE = "SnwTN06.exe"    '担当者別社内在庫一覧表
		''''    Prg_Name(501).PRGLAST = 0
		''''
		''''    Prg_Name(502).PRGTAB = 5
		''''    Prg_Name(502).PRGEXE = "SnwTN07.exe"    '担当者別社内在庫移動入力
		''''    Prg_Name(502).PRGLAST = 0
		''''
		''''    Prg_Name(503).PRGTAB = 5
		''''    Prg_Name(503).PRGEXE = "SnwTN08.exe"    '担当者別客先在庫棚卸入力
		''''    Prg_Name(503).PRGLAST = 0
		''''
		''''    Prg_Name(504).PRGTAB = 5
		''''    Prg_Name(504).PRGEXE = "SnwTN09.exe"    '担当者別客先在庫一覧表
		''''    Prg_Name(504).PRGLAST = 2
		''''
		''''    Prg_Name(505).PRGTAB = 5
		''''    Prg_Name(505).PRGEXE = "SnwTN10.exe"    '担当者別客先在庫入庫入力
		''''    Prg_Name(505).PRGLAST = 2
		''''
		''''    Prg_Name(506).PRGTAB = 5
		''''    Prg_Name(506).PRGEXE = "SnwTN11.exe"    '担当者別客先在庫参照   '2013/02/21 ADD
		''''    Prg_Name(506).PRGLAST = 2
		''''
		''''    Prg_Name(507).PRGTAB = 5
		''''    Prg_Name(507).PRGEXE = "SnwTN12.exe"    '担当者別客先在庫出庫入力   '2013/02/21 ADD
		''''    Prg_Name(507).PRGLAST = 2
		''''
		''''    Prg_Name(508).PRGTAB = 5
		''''    Prg_Name(508).PRGEXE = "SnwTN13.exe"    '担当者別客先在庫入庫入力   '2013/02/21 ADD
		''''    Prg_Name(508).PRGLAST = 2
		'''''''---2009/09/18 ADD-END
		'2015/09/22 DEL↑

		'2015/09/22 ADD↓
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(500).PRGTAB = 5
		''    Prg_Name(500).PRGEXE = "SnwTN05.exe"    'チーム別社内在庫棚卸入力
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(500).PRGEXE = "SnwTN20.exe" 'チーム別庫棚卸入力
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(500).PRGLAST = 1

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(501).PRGTAB = 5
		''    Prg_Name(501).PRGEXE = "SnwTN13.exe"    'チーム別社内在庫入庫入力
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(501).PRGEXE = "SnwTN21.exe" 'チーム別入出庫一覧
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(501).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(502).PRGTAB = 5
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(502).PRGEXE = "SnwTN19.exe" 'チーム別在庫調整入力
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(502).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(503).PRGTAB = 5
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(503).PRGEXE = "SnwTN22.exe" '棚卸表出力
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(503).PRGLAST = 0

		'2018/06/20 ADD↓
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(504).PRGTAB = 5
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(504).PRGEXE = "SnwTN25.exe" 'チーム別個別棚卸入力
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(504).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(505).PRGTAB = 5
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(505).PRGEXE = "SnwTN26.exe" '在庫状況一覧
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(505).PRGLAST = 0
		'2018/06/20 ADD↑

		''    Prg_Name(503).PRGTAB = 5
		''    Prg_Name(503).PRGEXE = "SnwTN07.exe"    'チーム別社内在庫移動入力
		''    Prg_Name(503).PRGLAST = 0
		''
		''    Prg_Name(504).PRGTAB = 5
		''    Prg_Name(504).PRGEXE = "SnwTN17.exe"    'チーム別社内在庫棚卸明細表
		''    Prg_Name(504).PRGLAST = 0
		''
		''    Prg_Name(505).PRGTAB = 5
		''    Prg_Name(505).PRGEXE = "SnwTN14.exe"    'チーム別社内在庫入庫明細表
		''    Prg_Name(505).PRGLAST = 2
		''
		''    Prg_Name(506).PRGTAB = 5
		''    Prg_Name(506).PRGEXE = "SnwTN15.exe"    'チーム別社内在庫出庫明細表
		''    Prg_Name(506).PRGLAST = 2
		''
		''    Prg_Name(507).PRGTAB = 5
		''    Prg_Name(507).PRGEXE = "SnwTN16.exe"    'チーム別社内在庫移動明細表
		''    Prg_Name(507).PRGLAST = 2
		'2015/09/22 ADD↑

		''''''---経費--------------------
		'''''    Prg_Name(600).PRGTAB = 6
		'''''    Prg_Name(600).PRGEXE = "SnwKD01.exe"    '経費入力
		'''''    Prg_Name(600).PRGLAST = 1
		'''''
		'''''    Prg_Name(601).PRGTAB = 6
		'''''    Prg_Name(601).PRGEXE = "SnwKD02.exe"    '損益計算書
		'''''    Prg_Name(601).PRGLAST = 0
		'''''
		'''''    Prg_Name(602).PRGTAB = 6
		'''''    Prg_Name(602).PRGEXE = "SnwKD03.exe"    '担当者別経費一覧表
		'''''    Prg_Name(602).PRGLAST = 2
		'''''
		''''''---マスタ１-------------------
		'''''    Prg_Name(700).PRGTAB = 7
		'''''    Prg_Name(700).PRGEXE = "SnwM06.exe"     '製品マスタ
		'''''    Prg_Name(700).PRGLAST = 1
		'''''
		'''''    Prg_Name(701).PRGTAB = 7
		'''''    Prg_Name(701).PRGEXE = "SnwM08.exe"     '品群マスタ
		'''''    Prg_Name(701).PRGLAST = 0
		'''''
		'''''    Prg_Name(702).PRGTAB = 7
		'''''    Prg_Name(702).PRGEXE = "SnwM12.exe"     'ユニットマスタ
		'''''    Prg_Name(702).PRGLAST = 0
		'''''
		'''''    Prg_Name(703).PRGTAB = 7
		'''''    Prg_Name(703).PRGEXE = "SnwM13.exe"     'ＰＣマスタ
		'''''    Prg_Name(703).PRGLAST = 0
		'''''
		'''''    Prg_Name(704).PRGTAB = 7
		'''''    Prg_Name(704).PRGEXE = "SnwM07.exe"     '製品構成マスタ
		'''''    Prg_Name(704).PRGLAST = 0
		'''''
		'''''    Prg_Name(705).PRGTAB = 7
		'''''    Prg_Name(705).PRGEXE = "SnwM09.exe"     '品群構成マスタ
		'''''    Prg_Name(705).PRGLAST = 0
		'''''
		'''''    Prg_Name(706).PRGTAB = 7
		'''''    Prg_Name(706).PRGEXE = ""
		'''''    Prg_Name(706).PRGLAST = 0
		'''''
		'''''    Prg_Name(707).PRGTAB = 7
		'''''    Prg_Name(707).PRGEXE = "SnwM14.exe"     '顧客テンプレート
		'''''    Prg_Name(707).PRGLAST = 0
		'''''
		'''''    Prg_Name(708).PRGTAB = 7
		'''''    Prg_Name(708).PRGEXE = "SnwM02.exe"     '得意先マスタ
		'''''    Prg_Name(708).PRGLAST = 0
		'''''
		'''''    Prg_Name(709).PRGTAB = 7
		'''''    Prg_Name(709).PRGEXE = "SnwM03.exe"     '納入先マスタ
		'''''    Prg_Name(709).PRGLAST = 0
		'''''
		'''''    Prg_Name(710).PRGTAB = 7
		'''''    Prg_Name(710).PRGEXE = "SnwM05.exe"     '仕入先マスタ
		'''''    Prg_Name(710).PRGLAST = 0
		'''''
		'''''    Prg_Name(711).PRGTAB = 7
		'''''    Prg_Name(711).PRGEXE = "SnwM15.exe"     '配送先マスタ
		'''''    Prg_Name(711).PRGLAST = 0
		'''''
		'''''    Prg_Name(712).PRGTAB = 7
		'''''    Prg_Name(712).PRGEXE = "SnwM16.exe"     '単価
		'''''    Prg_Name(712).PRGLAST = 2
		'''''
		'''''    Prg_Name(713).PRGTAB = 7
		'''''    Prg_Name(713).PRGEXE = ""
		'''''    Prg_Name(713).PRGLAST = 2
		'''''
		''''''---マスタ２-------------------
		'''''    Prg_Name(800).PRGTAB = 8
		'''''    Prg_Name(800).PRGEXE = "SnwM01.exe"     '会社マスタ
		'''''    Prg_Name(800).PRGLAST = 1
		'''''
		'''''    Prg_Name(801).PRGTAB = 8
		'''''    Prg_Name(801).PRGEXE = "SnwM30.exe"     '税率マスタ
		'''''    Prg_Name(801).PRGLAST = 0
		'''''
		'''''    Prg_Name(802).PRGTAB = 8
		'''''    Prg_Name(802).PRGEXE = "SnwM04.exe"     '担当者マスタ
		'''''    Prg_Name(802).PRGLAST = 0
		'''''
		'''''    Prg_Name(803).PRGTAB = 8
		'''''    Prg_Name(803).PRGEXE = "SnwM10.exe"     '科目マスタ
		'''''    Prg_Name(803).PRGLAST = 0
		'''''
		'''''    Prg_Name(804).PRGTAB = 8
		'''''    Prg_Name(804).PRGEXE = "SnwM11.exe"     '科目摘要マスタ
		'''''    Prg_Name(804).PRGLAST = 0
		'''''
		'''''    Prg_Name(805).PRGTAB = 8
		'''''    Prg_Name(805).PRGEXE = "SnwM21.exe"     '担当者目標マスタ
		'''''    Prg_Name(805).PRGLAST = 0
		'''''
		'''''    Prg_Name(806).PRGTAB = 8
		'''''    Prg_Name(806).PRGEXE = "SnwM22.exe"     '科目目標マスタ
		'''''    Prg_Name(806).PRGLAST = 0
		'''''
		'''''    Prg_Name(807).PRGTAB = 8
		'''''    Prg_Name(807).PRGEXE = "SnwM17.exe"     '請求金額マスタ
		'''''    Prg_Name(807).PRGLAST = 0
		'''''
		'''''    Prg_Name(808).PRGTAB = 8
		'''''    Prg_Name(808).PRGEXE = "SnwM18.exe"     '売掛金額マスタ
		'''''    Prg_Name(808).PRGLAST = 0
		'''''
		'''''    Prg_Name(809).PRGTAB = 8
		'''''    Prg_Name(809).PRGEXE = "SnwM19.exe"     '支払金額マスタ
		'''''    Prg_Name(809).PRGLAST = 0
		'''''
		'''''    Prg_Name(810).PRGTAB = 8
		'''''    Prg_Name(810).PRGEXE = "SnwM20.exe"     '買掛金額マスタ
		'''''    Prg_Name(810).PRGLAST = 0
		'''''    '2009/09/18 ADD
		'''''    Prg_Name(811).PRGTAB = 8
		'''''    Prg_Name(811).PRGEXE = "SnwM23.exe"     '担当者別社内在庫マスタ
		'''''    Prg_Name(811).PRGLAST = 0
		'''''    '2009/09/18 ADD
		'''''    Prg_Name(812).PRGTAB = 8
		'''''    Prg_Name(812).PRGEXE = "SnwM24.exe"     '担当者別客先在庫マスタ
		'''''    Prg_Name(812).PRGLAST = 0
		'''''
		''''''---保守---------------------
		'''''    Prg_Name(900).PRGTAB = 9
		'''''    Prg_Name(900).PRGEXE = "SnwHJ01.exe"    'データ削除
		'''''    Prg_Name(900).PRGLAST = 1
		'''''
		'''''    Prg_Name(901).PRGTAB = 9
		'''''    Prg_Name(901).PRGEXE = "SnwHJ02.exe"    'データバックアップ
		'''''    Prg_Name(901).PRGLAST = 2
		'2012/05/09 ADD↓
		'---統計--------------------
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(600).PRGTAB = 6
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(600).PRGEXE = "SnwTK13.exe" '得意先別年間売上順位推移表
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(600).PRGLAST = 1

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(601).PRGTAB = 6
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(601).PRGEXE = "SnwTK02.exe" '仕入別年間仕入順位推移表
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(601).PRGLAST = 0

		'2014/02/14 ADD↓
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(602).PRGTAB = 6
		''Prg_Name(602).PRGEXE = "SnwTK10.exe"    '部署別売上仕入推移表作成
		'    Prg_Name(602).PRGEXE = "SnwTK18.exe"    '部署別売上仕入推移表作成       '2014/08/25 ADD
		'    Prg_Name(602).PRGEXE = "SnwTK19.exe"    'チーム別売上仕入推移表作成       '2015/02/27 ADD
		'    Prg_Name(602).PRGEXE = "SnwTK12.exe"    '部署別売上仕入推移表作成       '2014/08/25 ADD
		'    Prg_Name(602).PRGEXE = "SnwTK22.exe"    'チーム別売上仕入推移表作成       '2016/07/16 ADD
		'    Prg_Name(602).PRGEXE = "SnwTK24.exe"    'チーム別売上仕入推移表作成       '2017/02/25 ADD
		'    Prg_Name(602).PRGEXE = "SnwTK28.exe"    'チーム別売上仕入推移表作成       '2017/07/31 ADD
		'    Prg_Name(602).PRGEXE = "SnwTK29.exe"    'チーム別売上仕入推移表作成       '2018/02/19 ADD
		'    Prg_Name(602).PRGEXE = "SnwTK31.exe"    'チーム別売上仕入推移表作成       '2019/02/28 ADD
		'    Prg_Name(602).PRGEXE = "SnwTK32.exe"    '別売上仕入推移表作成       '2020/02/26 ADD
		'    Prg_Name(602).PRGEXE = "SnwTK32v2.exe"    '統計推移表作成2021       '2021/02/13 ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(602).PRGEXE = "SnwTK32v3.exe" '統計推移表作成2022       '2023/04/21 ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(602).PRGLAST = 2

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(603).PRGTAB = 6
		'    Prg_Name(603).PRGEXE = "SnwTK14.exe"    '担当者別店舗別原価売価積上表作成
		'    Prg_Name(603).PRGEXE = "SnwTK20.exe"    '担当者別店舗別原価売価積上表作成'2015/03/02 ADD
		'    Prg_Name(603).PRGEXE = "SnwTK23.exe"    '担当者別店舗別原価売価積上表作成'2016/07/16 ADD
		'    Prg_Name(603).PRGEXE = "SnwTK25.exe"    '担当者別店舗別原価売価積上表作成'2017/02/25 ADD
		'    Prg_Name(603).PRGEXE = "SnwTK33.exe"    'チーム別店舗別原価売価積上表'2020/02/29 ADD

		'    Prg_Name(603).PRGEXE = "SnwTK32v3.exe"    '統計推移表作成2022       '2022/09/07 ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(603).PRGEXE = "SnwTK32v4.exe" '統計推移表作成2023       '2023/04/21 ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(603).PRGLAST = 2

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(604).PRGTAB = 6
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(604).PRGEXE = "SnwTK15.exe" '担当者別在庫金額増減推移表作成
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(604).PRGLAST = 2

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(605).PRGTAB = 6
		'    Prg_Name(605).PRGEXE = "SnwTK16.exe"    '得意先別売上仕入推移表作成
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(605).PRGEXE = "SnwTK32v5.exe" '統計推移表作成2023       '2023/09/27 ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(605).PRGLAST = 2
		'2014/02/14 ADD↑

		''    Prg_Name(606).PRGTAB = 6
		''    Prg_Name(606).PRGEXE = "SnwTK17.exe"    '部署別売上仕入推移表作成   '2014/08/25 ADD
		''    Prg_Name(606).PRGLAST = 2

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(606).PRGTAB = 6
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(606).PRGEXE = "SnwTK26.exe" 'チーム別積上チェック表作成   '2017/05/09 ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(606).PRGLAST = 2

		''    '2017/07/11 ADD↓
		''    Prg_Name(607).PRGTAB = 6
		''    Prg_Name(607).PRGEXE = "SnwTK27.exe"    'チーム別予実グラフ作成
		''    Prg_Name(607).PRGLAST = 2
		''    '2017/07/11 ADD↑

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(607).PRGTAB = 6
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(607).PRGEXE = "SnwTK30.exe" '顧客別施工金額実績集計表'2018/07/30 ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(607).PRGLAST = 2

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(608).PRGTAB = 6
		''    Prg_Name(608).PRGEXE = "SnwTK34.exe"    '確定済見積番号一覧表作成 '2021/03/04 ADD　2022/10/05 DEL
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(608).PRGEXE = "SnwTK35.exe" 'クレーム集計表作成 '2022/10/05 ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(608).PRGLAST = 2


		'2014/02/14 DEL↓
		''    Prg_Name(602).PRGTAB = 6
		''    Prg_Name(602).PRGEXE = "SnwTK03.exe"    '担当者別仕入先原価推移表作成
		''    Prg_Name(602).PRGLAST = 2
		''
		''    Prg_Name(603).PRGTAB = 6
		''    Prg_Name(603).PRGEXE = "SnwTK04.exe"    '担当者別得意先別物件別売上推移表作成
		''    Prg_Name(603).PRGLAST = 2
		''
		''    Prg_Name(604).PRGTAB = 6
		''    Prg_Name(604).PRGEXE = "SnwTK05.exe"    '物件種別得意先別売上推移表作成
		''    Prg_Name(604).PRGLAST = 2
		''
		''2014/02/14 DEL↓
		''    Prg_Name(605).PRGTAB = 6
		''    Prg_Name(605).PRGEXE = "SnwTK06.exe"    '担当者別売上仕入推移表作成
		''    Prg_Name(605).PRGLAST = 2
		''2014/02/14 DEL↑
		''2014/02/14 ADD↓
		''    Prg_Name(605).PRGTAB = 6
		''    Prg_Name(605).PRGEXE = "SnwTK10.exe"    '部署別売上仕入推移表作成
		''    Prg_Name(605).PRGLAST = 2
		''2014/02/14 ADD↑
		''
		''    Prg_Name(606).PRGTAB = 6
		''    Prg_Name(606).PRGEXE = "SnwTK07.exe"    '担当者別店舗別原価売価積上表作成
		''    Prg_Name(606).PRGLAST = 2
		''
		''    Prg_Name(607).PRGTAB = 6
		''    Prg_Name(607).PRGEXE = "SnwTK08.exe"    '担当者別在庫金額増減推移表作成
		''    Prg_Name(607).PRGLAST = 2
		''
		''    Prg_Name(608).PRGTAB = 6
		''    Prg_Name(608).PRGEXE = "SnwTK09.exe"    '得意先別売上仕入推移表作成
		''    Prg_Name(608).PRGLAST = 2
		'2014/02/14 DEL↑

		'---経費--------------------
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(700).PRGTAB = 7
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(700).PRGEXE = "SnwKD01.exe" '経費入力
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(700).PRGLAST = 1

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(701).PRGTAB = 7
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(701).PRGEXE = "SnwKD06.exe" '損益計算書
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(701).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(702).PRGTAB = 7
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(702).PRGEXE = "SnwKD03.exe" '担当者別経費一覧表
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(702).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(703).PRGTAB = 7
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(703).PRGEXE = "SnwHK03.exe" '売掛金集計表
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(703).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(704).PRGTAB = 7
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(704).PRGEXE = "SnwSK01.exe" '買掛金集計表
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(704).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(705).PRGTAB = 7
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(705).PRGEXE = "SnwND02.exe" '入金チェックリスト
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(705).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(706).PRGTAB = 7
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(706).PRGEXE = "SnwSD02.exe" '支払日計表
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(706).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(707).PRGTAB = 7
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(707).PRGEXE = "SnwKD04.exe" '経費取込処理
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(707).PRGLAST = 2

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(708).PRGTAB = 7
		'    Prg_Name(708).PRGEXE = "SnwKD05.exe"     '部署別経費集計表
		'    Prg_Name(708).PRGEXE = "SnwKD08.exe"     'チーム別按分経費集計表作成'2017/07/11 ADD
		'    Prg_Name(708).PRGEXE = "SnwKD08v2.exe"     'チーム別按分経費集計表作成'2017/07/11 ADD
		'    Prg_Name(708).PRGEXE = "SnwKD08v3.exe"     '按分経費集計表作成'2021/02/26 ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(708).PRGEXE = "SnwKD08v4.exe" '按分経費集計表作成'2023/04/21 ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(708).PRGLAST = 2

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(709).PRGTAB = 7
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(709).PRGEXE = "SnwKD07.exe" 'EXCEL仕訳CSV変換
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(709).PRGLAST = 2

		'2017/07/11 DEL
		''    Prg_Name(710).PRGTAB = 7
		''    Prg_Name(710).PRGEXE = "SnwKD08.exe"     'チーム別按分経費集計表作成
		''    Prg_Name(710).PRGLAST = 2
		''
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(710).PRGTAB = 7
		'    Prg_Name(710).PRGEXE = "SnwKD08v4.exe"     '按分経費集計表作成'2022/09/07 ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(710).PRGEXE = "SnwKD08v5.exe" '按分経費集計表作成'2023/04/21 ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(710).PRGLAST = 2


		'---マスタ１-------------------
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(800).PRGTAB = 8
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(800).PRGEXE = "SnwM06.exe" '製品マスタ
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(800).PRGLAST = 1

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(801).PRGTAB = 8
		'''''    Prg_Name(801).PRGEXE = "SnwM08.exe"     '品群マスタ
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(801).PRGEXE = "SnwM43.exe" '棚番マスタ
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(801).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(802).PRGTAB = 8
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(802).PRGEXE = "SnwM12.exe" 'ユニットマスタ
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(802).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(803).PRGTAB = 8
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(803).PRGEXE = "SnwM13.exe" 'ＰＣマスタ
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(803).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(804).PRGTAB = 8
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(804).PRGEXE = "SnwM07.exe" '製品構成マスタ
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(804).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(805).PRGTAB = 8
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(805).PRGEXE = "SnwM09.exe" '品群構成マスタ
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(805).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(806).PRGTAB = 8
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(806).PRGEXE = "SnwM33.exe" 'チームマスタ
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(806).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(807).PRGTAB = 8
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(807).PRGEXE = "SnwM14.exe" '顧客テンプレート
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(807).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(808).PRGTAB = 8
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(808).PRGEXE = "SnwM02.exe" '得意先マスタ
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(808).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(809).PRGTAB = 8
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(809).PRGEXE = "SnwM03.exe" '納入先マスタ
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(809).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(810).PRGTAB = 8
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(810).PRGEXE = "SnwM05.exe" '仕入先マスタ
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(810).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(811).PRGTAB = 8
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(811).PRGEXE = "SnwM15.exe" '配送先マスタ
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(811).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(812).PRGTAB = 8
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(812).PRGEXE = "SnwM16.exe" '単価
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(812).PRGLAST = 2

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(813).PRGTAB = 8
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(813).PRGEXE = "SnwM32.exe" '部署 2012/12/15 ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(813).PRGLAST = 2

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(814).PRGTAB = 8
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(814).PRGEXE = "SnwM36.exe" '仕入単価M　2015/07/20 ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(814).PRGLAST = 2

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(815).PRGTAB = 8
		'    Prg_Name(815).PRGEXE = "SnwM37.exe"      '従業員按分M　2016/01/26ADD
		'    Prg_Name(815).PRGEXE = "SnwM37v2.exe"      '従業員按分M　2016/01/26ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(815).PRGEXE = "SnwM49.exe" '業種按分M　2023/04/21 ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(815).PRGLAST = 2

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(816).PRGTAB = 8
		'    Prg_Name(816).PRGEXE = "SnwM39.exe"      '科目按分M　2016/10/21ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(816).PRGEXE = "SnwM39v2.exe" '科目按分M　2016/10/21ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(816).PRGLAST = 2

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(817).PRGTAB = 8
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(817).PRGEXE = "SnwM38.exe" '製品変換M　2016/02/07ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(817).PRGLAST = 2

		'    Prg_Name(818).PRGTAB = 8
		'    Prg_Name(818).PRGEXE = "SnwM40.exe"      'チーム予測M　2017/03/04ADD
		'    Prg_Name(818).PRGLAST = 2

		'    Prg_Name(819).PRGTAB = 8
		'    Prg_Name(819).PRGEXE = "SnwM42.exe"      '三研予測入力　2017/03/30ADD
		'    Prg_Name(819).PRGLAST = 2
		'
		'    Prg_Name(820).PRGTAB = 8
		'    Prg_Name(820).PRGEXE = "SnwM41.exe"      '三研実績入力　2017/03/04ADD
		'    Prg_Name(820).PRGLAST = 2

		'---マスタ２-------------------
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(900).PRGTAB = 9
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(900).PRGEXE = "SnwM01.exe" '会社マスタ
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(900).PRGLAST = 1

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(901).PRGTAB = 9
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(901).PRGEXE = "SnwM30.exe" '税率マスタ
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(901).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(902).PRGTAB = 9
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(902).PRGEXE = "SnwM04.exe" '担当者マスタ
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(902).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(903).PRGTAB = 9
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(903).PRGEXE = "SnwM10.exe" '科目マスタ
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(903).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(904).PRGTAB = 9
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(904).PRGEXE = "SnwM11.exe" '科目摘要マスタ
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(904).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(905).PRGTAB = 9
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(905).PRGEXE = "SnwM21.exe" '担当者目標マスタ
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(905).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(906).PRGTAB = 9
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(906).PRGEXE = "SnwM22.exe" '科目目標マスタ
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(906).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(907).PRGTAB = 9
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(907).PRGEXE = "SnwM17.exe" '請求金額マスタ
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(907).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(908).PRGTAB = 9
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(908).PRGEXE = "SnwM18.exe" '売掛金額マスタ
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(908).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(909).PRGTAB = 9
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(909).PRGEXE = "SnwM19.exe" '支払金額マスタ
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(909).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(910).PRGTAB = 9
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(910).PRGEXE = "SnwM20.exe" '買掛金額マスタ
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(910).PRGLAST = 0
		''    '2009/09/18 ADD
		''    Prg_Name(911).PRGTAB = 9
		''    Prg_Name(911).PRGEXE = "SnwM23.exe"     '担当者別社内在庫マスタ
		''    Prg_Name(911).PRGLAST = 0
		''    '2009/09/18 ADD
		'2015/06/11 ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(911).PRGTAB = 9
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(911).PRGEXE = "SnwM35.exe" 'ウエルシア物件内容マスタ
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(911).PRGLAST = 0
		'2015/06/11 ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(912).PRGTAB = 9
		''    Prg_Name(912).PRGEXE = "SnwM24.exe"     '担当者別客先在庫マスタ
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(912).PRGEXE = "SnwM50.exe" '工事担当マスタ '2024/01/31 ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(912).PRGLAST = 0

		'2012/05/31 ADD
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(913).PRGTAB = 9
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(913).PRGEXE = "SnwM31.exe" '補助科目マスタ
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(913).PRGLAST = 0

		'2018/07/07 ADD↓
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(914).PRGTAB = 9
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(914).PRGEXE = "SnwM40.exe" '統計集計先予測 --チーム予測M
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(914).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(915).PRGTAB = 9
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(915).PRGEXE = "SnwM42.exe" '三研予測入力
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(915).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(916).PRGTAB = 9
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(916).PRGEXE = "SnwM46.exe" 'フロント予測入力
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(916).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(917).PRGTAB = 9
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(917).PRGEXE = "SnwM48.exe" '三和ｻｰﾋﾞｽ予測入力
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(917).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(918).PRGTAB = 9
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(918).PRGEXE = "SnwM41.exe" '三研実績入力
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(918).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(919).PRGTAB = 9
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(919).PRGEXE = "SnwM45.exe" 'フロント実績入力
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(919).PRGLAST = 0

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(920).PRGTAB = 9
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(920).PRGEXE = "SnwM47.exe" '三和ｻｰﾋﾞｽ実績入力
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(920).PRGLAST = 0



		'---保守---------------------
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(1000).PRGTAB = 10
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(1000).PRGEXE = "SnwHJ01.exe" 'データ削除
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(1000).PRGLAST = 1

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(1001).PRGTAB = 10
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(1001).PRGEXE = "SnwHJ02.exe" 'データバックアップ
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(1001).PRGLAST = 2
		'2012/05/09 ADD↑

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(1002).PRGTAB = 10
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(1002).PRGEXE = "SnwHJ03.exe" '製品No変更
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(1002).PRGLAST = 2

		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGTAB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(1003).PRGTAB = 10
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGEXE の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(1003).PRGEXE = "SnwM44.exe" 'ログイン制御
		'UPGRADE_WARNING: オブジェクト Prg_Name().PRGLAST の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Prg_Name(1003).PRGLAST = 2

		Call LockMenuTab()

		'    On Error GoTo 0
	End Sub

	Private Sub Tb_Menu_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Tb_Menu.SelectedIndexChanged
		Static PreviousTab As Short = Tb_Menu.SelectedIndex()


		PreviousTab = Tb_Menu.SelectedIndex()
	End Sub

	Private Sub LockMenuTab()
		'ボタンロック

	End Sub

	' ''Private Sub cbStart_Click(ZIX As Integer)
	' ''    Dim Ret As Long
	' ''
	' ''    On Error Resume Next
	' ''
	' ''    'プログラムの起動
	' ''    HourGlass True
	' ''    Ret = ShellExecute(Me.hwnd, vbNullString, AppPath(Prg_Name(ZIX).PRGEXE), CONNECT, vbNullString, 1)
	' ''    HourGlass False
	' ''
	' ''    On Error GoTo 0
	' ''End Sub

	'Private WithEvents cbStart As Button ' Assuming cbStart is a Button control

	' Constants for the nShowCmd parameter
	Private Const SW_SHOWNORMAL As Integer = 1

	' Declare the ShellExecute function from the shell32.dll
	<DllImport("shell32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
	Private Shared Function ShellExecute(
		ByVal hwnd As IntPtr,
		ByVal lpOperation As String,
		ByVal lpFile As String,
		ByVal lpParameters As String,
		ByVal lpDirectory As String,
		ByVal nShowCmd As Integer) As IntPtr
	End Function

	Private Sub CbExe(ByVal ZIX As Integer)
		'Dim ZIX As Integer ' Assuming ZIX is an integer, adjust the type if necessary
		'Dim Ret As Long
		Dim result As IntPtr

		Try
			' Program launch
			HourGlass(True)
			'Ret = ShellExecute(Me.Handle, Nothing, AppPath(Prg_Name(ZIX).PRGEXE), CONNECT, Nothing, 1)
			result = ShellExecute(Me.Handle, "open", System.IO.Path.Combine(AppPath, Prg_Name(ZIX).PRGEXE), $"""{CONNECT}""", Nothing, SW_SHOWNORMAL)

			'Debug.WriteLine(String.Join("File:", System.IO.Path.Combine(AppPath, Prg_Name(ZIX).PRGEXE)))
			'Debug.WriteLine(String.Join("Parameters:", CONNECT))
			'Debug.WriteLine(String.Join("ShowCmd:", SW_SHOWNORMAL))

			' Check the result for errors
			If result.ToInt64 <= 32 Then
				MessageBox.Show("Failed to launch application. Error code: " & result.ToInt64.ToString())
			End If

		Catch ex As Exception
			' Handle exceptions here
		Finally
			HourGlass(False)
		End Try
	End Sub

	Private Sub HourGlass(ByVal state As Boolean)
		If state Then
			Cursor.Current = Cursors.WaitCursor
		Else
			Cursor.Current = Cursors.Default
		End If
	End Sub

	'//////////////////////////////////////
	'   メニューボタンを使用不可
	'//////////////////////////////////////
	Public Function LockMenu() As Boolean
		Dim rs As ADODB.Recordset = Nothing
		Dim sql As String
		Dim LoginName As String

		LockMenu = False

		Try
			LoginName = GetUName()

			' SQL生成
			sql = "SELECT AT.AppTitle, AT.AppNo, AT.MenuNo1, AT.MenuNo2, LA.AppEnabled, LA.LoginName " &
				  "FROM AppTitle AS AT " &
				  "LEFT OUTER JOIN TMログイン制御 AS LA " &
				  "ON AT.AppTitle = LA.AppTitle " &
				  "WHERE LoginName = '" & SQLString(LoginName) & "'"

			Dim blRet As Boolean
			blRet = DBStd.DBOpen(My.Settings.AdodbConnectionString, Cn, True)

			' SQL実行
			rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)

			Dim ctrl As Control
			If rs IsNot Nothing Then
				Do Until rs.EOF
					If rs.Fields("MenuNo1").Value <> 0 Then
						ctrl = Me.Controls.Find("CbStart_" & rs.Fields("MenuNo1").Value.ToString, True).FirstOrDefault()
						' NOTE SS clsLoginControl.Property AppEnabled:
						' NOTE SS ctrl.Enabled = ![AppEnabled]:
						ctrl.Enabled = rs.Fields("AppEnabled").Value
						If rs.Fields("AppEnabled").Value = False Then
							ctrl.Visible = False
						End If
					End If
					If rs.Fields("MenuNo2").Value <> 0 Then
						ctrl = Me.Controls.Find("CbStart_" & rs.Fields("MenuNo2").Value.ToString, True).FirstOrDefault()
						' NOTE SS clsLoginControl.Property AppEnabled:
						' NOTE SS ctrl.Enabled = ![AppEnabled]:
						ctrl.Enabled = rs.Fields("AppEnabled").Value
						If rs.Fields("AppEnabled").Value = False Then
							ctrl.Visible = False
						End If
					End If
					rs.MoveNext()
				Loop
			End If

		Catch ex As Exception
			' エラー時の処理
			Throw New ApplicationException(ex.Message)
		Finally
			' リソースの解放
			If rs IsNot Nothing Then ReleaseRs(rs)
		End Try
	End Function

	''' <summary>
	''' 見積入力起動
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	Private Sub CbStart_8_Click(sender As Object, e As EventArgs) Handles CbStart_8.Click
		Debug.WriteLine(String.Join("Prg_Name.PRGEXE:", Prg_Name(8).PRGEXE))
		Call CbExe(8)
	End Sub

	''' <summary>
	''' 標準ブラウザ起動
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	Private Sub CbStart_16_Click(sender As Object, e As EventArgs) Handles CbStart_16.Click

		' 標準ブラウザのProgIdを取得
		Dim progId As String = GetDefaultBrowserProgId()

		' WebDriver定義
		Dim driver As IWebDriver = Nothing

		Select Case progId.ToLower()
			Case "chromehtml"
				Dim chromeOptions As New ChromeOptions()

				chromeOptions.AddArgument("--start-maximized") ' ブラウザを最大化して起動
				chromeOptions.LeaveBrowserRunning = True ' WebDriver 終了後もブラウザを保持
				'chromeOptions.AddAdditionalOption("detach", True) ' detach オプションを有効化
				'chromeOptions.AddArgument("--remote-debugging-port=9222") ' リモートデバッグ用ポートを設定

				' 自動テストメッセージを非表示にする設定
				chromeOptions.AddExcludedArgument("enable-automation")
				chromeOptions.AddAdditionalOption("useAutomationExtension", False)

				' ChromeDriverサービスをカスタマイズしてプロンプトを非表示
				Dim chromeService As ChromeDriverService = ChromeDriverService.CreateDefaultService()
				chromeService.HideCommandPromptWindow = True ' コンソールウィンドウを非表示

				' ChromeDriverの起動
				driver = New ChromeDriver(chromeService, chromeOptions)

			Case "msedgehtm"
				Dim edgeOptions As New EdgeOptions()

				edgeOptions.AddArgument("--start-maximized") ' ブラウザを最大化して起動
				edgeOptions.LeaveBrowserRunning = True ' WebDriver 終了後もブラウザを保持
				'edgeOptions.AddArgument("--remote-debugging-port=9222") ' リモートデバッグ用ポートを設定

				' 自動テストメッセージを非表示にする設定
				edgeOptions.AddExcludedArgument("enable-automation")
				edgeOptions.AddAdditionalOption("useAutomationExtension", False)

				' EdgeDriverサービスをカスタマイズしてプロンプトを非表示
				Dim edgeService As EdgeDriverService = EdgeDriverService.CreateDefaultService()
				edgeService.HideCommandPromptWindow = True ' コンソールウィンドウを非表示

				' EdgeDriverの起動
				driver = New EdgeDriver(edgeService, edgeOptions)

			Case Else
				MessageBox.Show("対応していないブラウザが指定されました。")
				Return
		End Select

		Try

			' Windowsログインユーザー名を取得
			Dim windowsUser As String = Environment.UserName

			' ログインURLを開く
			driver.Navigate().GoToUrl(My.Settings.PleasanterURL)

			' WebDriverWait のインスタンスを作成 (最大 5 秒待機)
			Dim wait As New WebDriverWait(driver, TimeSpan.FromSeconds(5))

			' ユーザー名を送信
			Dim usernameField As IWebElement = wait.Until(Function(d)
															  Dim el = d.FindElement(By.Name("Users_LoginId")) ' Pleasanterのユーザー名
															  If el.Displayed Then
																  Return el
															  Else
																  Return Nothing
															  End If
														  End Function)
			'Dim usernameField As IWebElement = driver.FindElement(By.Name("Users_LoginId"))  ' Pleasanterのユーザー名
			usernameField.SendKeys(windowsUser)

			' パスワードを送信
			Dim passwordField As IWebElement = wait.Until(Function(d)
															  Dim el = d.FindElement(By.Name("Users_Password")) ' Pleasanterのパスワード
															  If el.Displayed Then
																  Return el
															  Else
																  Return Nothing
															  End If
														  End Function)
			'Dim passwordField As IWebElement = driver.FindElement(By.Name("Users_Password")) ' Pleasanterのパスワード
			passwordField.SendKeys("")

			' 検索結果を少し待つ (例: 3秒)
			'Threading.Thread.Sleep(3000)

		Catch ex As Exception
			MessageBox.Show($"エラーが発生しました: {ex.Message}")

		Finally
			' WebDriver を終了
			If driver IsNot Nothing Then
				'driver.Close()
				'driver.Dispose()
			End If
		End Try
	End Sub

	''' <summary>
	''' 標準ブラウザのProgIdを取得
	''' </summary>
	''' <returns></returns>
	Private Function GetDefaultBrowserProgId() As String
		Try
			' レジストリキーを開く
			Dim regKey As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice", False)

			' 標準ブラウザのProgId値を取得
			If regKey IsNot Nothing Then
				Dim ProgId As String = regKey.GetValue("ProgId").ToString()

				Return ProgId
			End If
		Catch ex As Exception
			MessageBox.Show($"エラーが発生しました: {ex.Message}")
		End Try

		Return String.Empty
	End Function

	''' <summary>
	''' Timer Tick
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
		sb_Msg_Panel2.Text = DateTime.Now.ToString("yyyy/MM/dd")
		sb_Msg_Panel3.Text = DateTime.Now.ToString("HH:mm")
	End Sub

End Class