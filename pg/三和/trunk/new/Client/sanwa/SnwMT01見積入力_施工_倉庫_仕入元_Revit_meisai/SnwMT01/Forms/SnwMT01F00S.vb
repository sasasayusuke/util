Option Strict Off
Option Explicit On

Imports System.Runtime.InteropServices

''' <summary>
''' --------------------------------------------------------------------
'''   ユーザー名           株式会社三和商研
'''   業務名               販売管理システム
'''   部門名               見積部門
'''   プログラム名         員数シート選択処理
'''   作成会社             テクノウェア株式会社
'''   作成日               2003/04/22
'''   作成者               oosawa
''' --------------------------------------------------------------------
'''    UPDATE
'''        2003/12/04  kawamura  合計金額に出精値引を含める
'''        2004/01/17  oosawa    見積日付・見積番号の降順に並べる
'''                              idcの開放
'''        2004/11/29  oosawa    更新日での制御を追加
'''        2005/07/05  kawamura  選択項目のリスト幅をiniファイルに保持
'''        2009/12/02  oosawa    担当者での抽出を追加
'''        2010/05/06  oosawa    処理種別で0～2以外が入力できてしまう
'''        2011/11/29  oosawa    見積区分での抽出追加
'''                              売上単価ﾏｽﾀとの単価違いをエラーとして表示する
'''                              仕分レベルの時間を未入力時00:00をセットし、入力しなくても良いようにする
'''        2012/09/13  oosawa    SelListVw.Widthの幅を追従するように変更
'''        2012/10/26  oosawa    最大化で表示するように変更
'''                              登録変更日を表示するように変更
'''        2013/01/15  oosawa    原価率を表示
'''        2013/09/18  oosawa    売上・仕入済みでも変更可能に
'''                              売上日・仕入日を表示
'''        2015/01/09  oosawa    仕入日を未仕入行数に変更
'''        2015/01/19  oosawa    仕入日を復活
'''        2016/10/26  oosawa    開始納期の範囲指定を追加
'''        2017/02/08  oosawa    見積ｺﾋﾟｰ機能停止（社長命令）
'''        2020/06/18  oosawa    経過表と統合
'''        2020/10/30  oosawa    ロックできる（開ける）データを３までに制御する
''' 
''' --------------------------------------------------------------------
''' </summary>
Friend Class SnwMT01F00S
    Inherits System.Windows.Forms.Form

    Const ProfileKey As String = "経過確認表"
    Const DefaultTemplateFile As String = "Temp経過確認表.xlsx"
    Dim GXLSPath As String '出力パス

    Private Const password_Exl As String = "sanwa55" 'Excelパスワード

    'フォームの再描画用のAPI定義
    'Private Declare Function LockWindowUpdate Lib "user32" (ByVal Hwnd As Integer) As Integer
    <DllImport("user32.dll", SetLastError:=True)>
    Private Shared Function LockWindowUpdate(ByVal hwndLock As IntPtr) As Boolean
    End Function

    '--Formで使用する変数--
    'FormLoadできたかのﾁｪｯｸﾌﾗｸﾞ
    Dim Loaded As Boolean
    'コントロールの戻りを制御
    Dim PreviousControl As System.Windows.Forms.Control
    '各項目でEnterKeyが押されたかのﾁｪｯｸﾌﾗｸﾞ
    Dim ReturnF As Boolean
    Dim KeyUpF As Boolean
    '起動時のフォームサイズHold用
    Dim MeWidth As Integer
    Dim MeHeight As Integer
    Dim wkWidth As Integer
    Dim WidthSa As Integer
    Dim WkHeight As Integer
    Dim LvHeightLimit, MeHeightLimit As Integer

    'ボタン２重起動防止フラグ(CbFunc_Clickで使用)
    Dim CLK2F As Boolean

    'リストビュー制御用(ListViewで使用)
    Dim CLKFLG As Boolean
    Dim psel_SQL As String
    Dim pSel_PKey As String
    Dim pSel_Caption As String
    Dim Find_ID As String
    Dim Find_Name As String
    Dim sql As String
    Public grs As ADODB.Recordset
    'Dim ResultCodeSetControl As Control     '選択したコードの送り先をセットする。

    '日付用ワーク
    'UPGRADE_WARNING: 配列を New で宣言することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC9D3AE5-6B95-4B43-91C7-28276302A5E8"' をクリックしてください。
    Dim idc(9) As iDate

    'クラス
    Private cTanto As clsTanto
    Private cTokuisaki As clsTokuisaki
    Private cNonyusaki As clsNonyusaki
    Private cWelBukkenKubun As clsWelBukkenKubun
    Private cBukken As clsBukken

	'List配列CbFuncのButton
	Private LcbFunc As New List(Of Button)

    Private isInitialLoad As Boolean = True

    Public Sub New()
        InitializeComponent()

        ' 最初から非表示にする（フォームが表示される前）
        Me.Opacity = 0 ' または Me.Visible = False や Me.Hide()
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)

        ' 初回ロード時だけ表示
        If isInitialLoad Then
            isInitialLoad = False
            ' ここで準備が整った後に表示
            Me.Opacity = 1 ' または Me.Show()
        End If
    End Sub

    Private Sub SnwMT01F00S_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Me.Load
        Dim SelListWidth As String
        Dim vSelListWidth As Object

        '    iconﾌﾟﾛﾊﾟﾃｨで指定に変更しないとEXEのｱｲｺﾝは変わらない
        '    Set Me.Icon = LoadPicture("snwIcon.ico", , 2) '2021/01/17 ADD

        On Error GoTo Form_Load_Err

        Loaded = False

        '実行ファイルのパスを除いて引数のみを取得
        Dim args As String() = Environment.GetCommandLineArgs()
        If Not args.Any() Then
            Call CriticalAlarm("このプログラムは正しく起動していません。", Me.Text)
            Call Me.Close()
            Exit Sub
        ElseIf Environment.GetCommandLineArgs(0).ToUpper().Contains("DEBUG") Then
            INI.CONNECT = args(1)
        Else
            INI.CONNECT = args(1)
        End If

        'Me.Hide() ' コンストラクタで非表示に変更
        If ApplicationInit(True) = False Then
            Me.Close()
            Exit Sub
        End If
        Me.Show()

        '表示項目設定取得
        'UPGRADE_WARNING: オブジェクト GetIni() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
        SelListWidth = GetIni("SelListWidth", "SE23F00_SPD", INIFile)
        If SelListWidth = "" Then
            SelListWidth = "1050,1050,1050,800,900,3500,1500,900,900,1100"
            WriteIni("SelListWidth", "SE23F00_SPD", SelListWidth, INIFile)
        End If

        'UPGRADE_WARNING: オブジェクト vSelListWidth の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
        vSelListWidth = Split(SelListWidth, ",")

        '2013/09/18 ADD↓
        Dim i As Integer
        Dim cnt As Integer
        'cnt = 15
        'cnt = 23 '2015/11/26 ADD 2016/10/26 ADD
        cnt = 28 '2022/10/10 ADD

        If UBound(vSelListWidth) < cnt Then
            ReDim Preserve vSelListWidth(cnt)
            For i = 0 To cnt
                'UPGRADE_WARNING: オブジェクト vSelListWidth(i) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                If vSelListWidth(i) = "" Then
                    'UPGRADE_WARNING: オブジェクト vSelListWidth() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                    vSelListWidth(i) = 900
                End If
            Next
        End If
        '2013/09/18 ADD↑

        'Debug.WriteLine($"vSelListWidth(0):{CInt(VB6Conv.TwipsToPixelsX(vSelListWidth(0)))}")

        'リストヘッダー項目セット
        With SelListVw
            .Columns.Clear()
            .Columns.Add("", "見積番号", CInt(VB6Conv.TwipsToPixelsX(vSelListWidth(0))))
            .Columns.Add("見積日付", CInt(VB6Conv.TwipsToPixelsX(vSelListWidth(1))), System.Windows.Forms.HorizontalAlignment.Center)
            .Columns.Add("種別", CInt(VB6Conv.TwipsToPixelsX(vSelListWidth(2))), System.Windows.Forms.HorizontalAlignment.Center)
            .Columns.Add("", "得意先", CInt(VB6Conv.TwipsToPixelsX(vSelListWidth(3))))
            .Columns.Add("", "納入先", CInt(VB6Conv.TwipsToPixelsX(vSelListWidth(4)))) '2015/06/12 ADD
            .Columns.Add("", "見積件名", CInt(VB6Conv.TwipsToPixelsX(vSelListWidth(5))))
            .Columns.Add("見積金額", CInt(VB6Conv.TwipsToPixelsX(vSelListWidth(6))), System.Windows.Forms.HorizontalAlignment.Right)

            .Columns.Add("Wﾘｰｽ区分", CInt(VB6Conv.TwipsToPixelsX(vSelListWidth(7))), System.Windows.Forms.HorizontalAlignment.Center)
            .Columns.Add("W物件区分", CInt(VB6Conv.TwipsToPixelsX(vSelListWidth(8))), System.Windows.Forms.HorizontalAlignment.Center)

            '2022/10/10 ADD↓
            .Columns.Add("YK管轄", CInt(VB6Conv.TwipsToPixelsX(vSelListWidth(9))), System.Windows.Forms.HorizontalAlignment.Center)
            .Columns.Add("YK物件", CInt(VB6Conv.TwipsToPixelsX(vSelListWidth(10))), System.Windows.Forms.HorizontalAlignment.Center)
            .Columns.Add("YK請求", CInt(VB6Conv.TwipsToPixelsX(vSelListWidth(11))), System.Windows.Forms.HorizontalAlignment.Center)

            .Columns.Add("B管轄", CInt(VB6Conv.TwipsToPixelsX(vSelListWidth(12))), System.Windows.Forms.HorizontalAlignment.Center)
            .Columns.Add("BtoB番号", CInt(VB6Conv.TwipsToPixelsX(vSelListWidth(13))), System.Windows.Forms.HorizontalAlignment.Center)
            '2022/10/10 ADD↑

            .Columns.Add("出力日", CInt(VB6Conv.TwipsToPixelsX(vSelListWidth(14))), System.Windows.Forms.HorizontalAlignment.Center)
            .Columns.Add("発注日", CInt(VB6Conv.TwipsToPixelsX(vSelListWidth(15))), System.Windows.Forms.HorizontalAlignment.Center)
            .Columns.Add("開始納期", CInt(VB6Conv.TwipsToPixelsX(vSelListWidth(16))), System.Windows.Forms.HorizontalAlignment.Center) '2016/10/26 ADD
            .Columns.Add("仕入日", CInt(VB6Conv.TwipsToPixelsX(vSelListWidth(17))), System.Windows.Forms.HorizontalAlignment.Center)
            .Columns.Add("未仕入行数", CInt(VB6Conv.TwipsToPixelsX(vSelListWidth(18))), System.Windows.Forms.HorizontalAlignment.Right) '2015/01/09 ADD
            .Columns.Add("売上日", CInt(VB6Conv.TwipsToPixelsX(vSelListWidth(19))), System.Windows.Forms.HorizontalAlignment.Center)
            .Columns.Add("完了日", CInt(VB6Conv.TwipsToPixelsX(vSelListWidth(20))), System.Windows.Forms.HorizontalAlignment.Center)

            .Columns.Add("請求予定", CInt(VB6Conv.TwipsToPixelsX(vSelListWidth(21))), System.Windows.Forms.HorizontalAlignment.Center)

            .Columns.Add("", "経過備考1", CInt(VB6Conv.TwipsToPixelsX(vSelListWidth(22))))
            .Columns.Add("", "経過備考2", CInt(VB6Conv.TwipsToPixelsX(vSelListWidth(23))))

            .Columns.Add("請求書", CInt(VB6Conv.TwipsToPixelsX(vSelListWidth(24))), System.Windows.Forms.HorizontalAlignment.Center)

            .Columns.Add("原価率", CInt(VB6Conv.TwipsToPixelsX(vSelListWidth(25))), System.Windows.Forms.HorizontalAlignment.Right) '2013/01/18 ADD

            .Columns.Add("更新日付", CInt(VB6Conv.TwipsToPixelsX(vSelListWidth(26))), System.Windows.Forms.HorizontalAlignment.Center) '2012/10/26 ADD
            .Columns.Add("物件番号", CInt(VB6Conv.TwipsToPixelsX(vSelListWidth(27))), System.Windows.Forms.HorizontalAlignment.Right) '2015/11/26 ADD

            .Columns.Add("", "見積確定", CInt(VB6Conv.TwipsToPixelsX(vSelListWidth(28)))) '2021/04/11 ADD
        End With

        SelListVw.View = View.Details
        SelListVw.Scrollable = True
        'SelListVw.Width = 11650 + 330     'スクロールバー分プラス
        'SelListVw.Width = 20385 + 330     'スクロールバー分プラス   2011/11/29 ADD
        'SelListVw.Width = VB6Conv.TwipsToPixelsX(12750 + 330) 'スクロールバー分プラス   2020/08/20 ADD
        'SelListVw.Width = 12750 + 330 'スクロールバー分プラス   2020/08/20 ADD

        wkWidth = (VB6Conv.PixelsToTwipsX(Me.SelListVw.Left) * 2) + VB6Conv.PixelsToTwipsX(SelListVw.Width) 'wkフォーム幅セット
        WidthSa = VB6Conv.PixelsToTwipsX(Me.Width) - VB6Conv.PixelsToTwipsX(Me.ClientRectangle.Width) 'フォームWidthの内外値差
        'wkWidth = (Me.SelListVw.Left * 2) + SelListVw.Width 'wkフォーム幅セット
        'WidthSa = Me.Width - Me.ClientRectangle.Width 'フォームWidthの内外値差

        If wkWidth < 11070 Then
            wkWidth = 11070
        End If
        Me.Width = VB6Conv.TwipsToPixelsX(wkWidth + WidthSa) 'フォーム幅決定(Width)
        'Me.Width = (wkWidth + WidthSa) 'フォーム幅決定(Width)

        'クラス生成
        cTanto = New clsTanto
        cTokuisaki = New clsTokuisaki
        cNonyusaki = New clsNonyusaki
        cWelBukkenKubun = New clsWelBukkenKubun
        cBukken = New clsBukken

        'NOTE SS IDC() NEWの代替え
        For i = 0 To idc.Length - 1
            idc(i) = New iDate()  ' 配列の各要素にインスタンスを作成して代入
        Next

        '日付セット
        idc(0).SetupA(Me, "s見積日", 0)
        idc(1).SetupA(Me, "e見積日", 0)

        idc(2).SetupA(Me, "s開始納期", 0) '2016/10/26 ADD
        idc(3).SetupA(Me, "e開始納期", 0) '2016/10/26 ADD

        idc(4).SetupA(Me, "s完了日", 0) '2020/04/16 ADD
        idc(5).SetupA(Me, "e完了日", 0) '2020/04/16 ADD

        idc(6).SetupA(Me, "s請求予定", 0) '2020/04/16 ADD
        idc(7).SetupA(Me, "e請求予定", 0) '2020/04/16 ADD

        idc(8).SetupA(Me, "s仕入日", 0) '2020/04/16 ADD
        idc(9).SetupA(Me, "e仕入日", 0) '2020/04/16 ADD

        'フォームを画面の中央に配置
        'Me.Top = VB6Conv.TwipsToPixelsY((VB6Conv.PixelsToTwipsY(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height) - VB6Conv.PixelsToTwipsY(Me.Height)) \ 2)
        'Me.Left = VB6Conv.TwipsToPixelsX((VB6Conv.PixelsToTwipsX(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width) - VB6Conv.PixelsToTwipsX(Me.Width)) \ 2)
        Me.Top = (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - Me.Height) \ 2
        Me.Left = (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - Me.Width) \ 2
        '項目クリア
        Call FormInitialize()
        [tx_売上種別].Text = CStr(0)

        MeWidth = VB6Conv.PixelsToTwipsX(Me.Width)
        MeHeight = VB6Conv.PixelsToTwipsY(Me.Height) - VB6Conv.PixelsToTwipsY(SelListVw.Height)
        'LvHeightLimit = SelListVw.Font.SizeInPoints * (34 + 24) '(列見出し + 明細一行分）
        LvHeightLimit = SelListVw.Font.Height * 2  ' 1行分（明細）+ タイトル行 のつもり
        MeHeightLimit = MeHeight + LvHeightLimit

        SelListVw.Width = Me.ClientSize.Width - 20
        SelListVw.Height = Me.ClientSize.Height - 272

        '2014/07/10 ADD
        If COUNTRY_CODE = "CN" Then
            'NOTE SS ExNmText(FormatType/DecimalPlace)→MultiSelection分離
            'UPGRADE_WARNING: オブジェクト tx_s見積金額.FormatType の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            '[tx_s見積金額].FormatType = "#,##0" & KIN_FMT & ";-#,##0" & KIN_FMT & ";#;#;"
            [tx_s見積金額].FormatType = "#,##0" & KIN_FMT
            [tx_s見積金額].FormatTypeNega = "-#,##0" & KIN_FMT
            [tx_s見積金額].FormatTypeNull = "#"
            [tx_s見積金額].FormatTypeZero = "#"
            'UPGRADE_WARNING: オブジェクト tx_s見積金額.DecimalPlace の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_s見積金額].DecimalPlace = KIN_HASU
            'UPGRADE_WARNING: オブジェクト tx_e見積金額.FormatType の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            '[tx_e見積金額].FormatType = "#,##0" & KIN_FMT & ";-#,##0" & KIN_FMT & ";#;#;"
            [tx_e見積金額].FormatType = "#,##0" & KIN_FMT
            [tx_e見積金額].FormatTypeNega = "-#,##0" & KIN_FMT
            [tx_e見積金額].FormatTypeNull = "#"
            [tx_e見積金額].FormatTypeZero = "#"
            'UPGRADE_WARNING: オブジェクト tx_e見積金額.DecimalPlace の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_e見積金額].DecimalPlace = KIN_HASU
        End If

        '出力先情報セット
        'UPGRADE_WARNING: オブジェクト GetIni() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
        GXLSPath = GetIni("Path", ProfileKey, INIFile)
        'UPGRADE_WARNING: TextBox プロパティ txDir.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
        [txDir].Text = CompactPathEx(GXLSPath, [txDir].MaxLength)

        ' フォームでキー入力を受け取れるようにする
        Me.KeyPreview = True

        'ロード時にラベル・ボタンを動的に配置する
        AddPicFunction()

        Loaded = True
        Exit Sub
Form_Load_Err:
        MsgBox(Err.Number & " " & Err.Description)
        Exit Sub
    End Sub

    Private Sub AddPicFunction()

        ' ラベル・ボタンを配置するPanelを指定
        Dim targetPanel As Panel = PicFunction

        ' 生成数を指定
        Dim genLabelCount As Integer = 12
        Dim genLabelWidth As Integer = 65
        Dim genLabelHeight As Integer = 12
        Dim genLabelY As Integer = 1
        Dim genButtonCount As Integer = 12
        Dim genButtonWidth As Integer = 65
        Dim genButtonHeight As Integer = 18
        Dim genButtonY As Integer = 12

        Dim margin As Integer = 7               ' PFキー4つ置きのボタン間の余白

        Dim i As Integer

        For i = 0 To genLabelCount - 1

            ' 新しいラベルを作成
            Dim lbl As New Label()

            ' ラベルのプロパティを設定
            lbl.Tag = i + 1                     ' IndexをTagプロパティに格納
            lbl.Name = "LbFunc" & i + 1

            ' ラベルをPanelに追加
            targetPanel.Controls.Add(lbl)

            lbl.Size = New Size(genLabelWidth, genLabelHeight)
            lbl.Location = New Point((i * genLabelWidth) + 1 + ((i \ 4) * margin), genLabelY) ' ラベルの位置を計算

            lbl.Text = "F" & i + 1
        Next

        For i = 0 To genButtonCount - 1

            ' 新しいボタンを作成
            Dim btn As New Button()

            ' ボタンのプロパティを設定
            btn.Tag = i + 1                     ' IndexをTagプロパティに格納
			btn.TabIndex = i + 1                ' TabIndexをプロパティに格納
            btn.Name = "CbFunc" & i + 1
            btn.Font = New Font("MS Gothic", 7.8, FontStyle.Regular)
            btn.FlatStyle = FlatStyle.Standard

            btn.Size = New Size(genButtonWidth, genButtonHeight)
            btn.Location = New Point((i * genButtonWidth) + 1 + ((i \ 4) * margin), genButtonY) ' ボタンの位置を計算

            'Debug.WriteLine($"Location X:{(i * genButtonWidth) + 1 + ((i \ 4) * margin)}")

            Select Case i
                Case 3
                    btn.Text = "新規見積"
                Case 6
                    btn.Text = "出力"
                Case 9
                    btn.Text = "中止"
                Case 10
                    btn.Text = "選択"
                Case 11
                    btn.Text = "閉じる"
                Case Else
                    btn.Text = ""
            End Select

            ' ボタンにクリックイベントを追加
            AddHandler btn.Click, AddressOf CbFunc_Click

            ' ボタンをPanelに追加
            targetPanel.Controls.Add(btn)

            ' リストにボタンを追加
            LcbFunc.Add(btn)

        Next

    End Sub

    ' 動的に追加したボタンのクリックイベント
    Private Sub CbFunc_Click(sender As Object, e As EventArgs)
        Dim btn As Button = CType(sender, Button)
        Dim Index As Integer = CType(btn.Tag, Integer) - 1
        'Dim Index As Integer = cbFunc.GetIndex(eventSender)
        '    Dim check As Boolean
        If LcbFunc(Index).Text = "" Then
            PreviousControl.Focus()
            Exit Sub
        End If
        'ボタンを押されたのが２回目ならば抜ける
        If CLK2F = True Then
            Exit Sub
        End If
        'ボタン２重起動防止フラグのセット
        CLK2F = True
        LcbFunc(Index).TabStop = True
        Select Case Index
            Case 0 'Caption:見積ｺﾋﾟｰ
                If SelListVw.Items.Count <> 0 Then
                    If SelListVw.FocusedItem IsNot Nothing Then
                        If SelListVw.FocusedItem.Selected = True Then
                            '見積コピー画面呼び出し
                            SnwMT02F07.ResParentForm = Me
                            'UPGRADE_WARNING: コレクション SelListVw.ListItems の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
                            'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                            SnwMT02F07.MituNo = SpcToNull(Trim(SelListVw.Items.Item(SelListVw.FocusedItem.Index).Text), 0)
                            '                    SnwMT02F07.MituNM = SpcToNull(Trim$(SelListVw.ListItems.Item(SelListVw.SelectedItem.Index).ListSubItems(4)), "")
                            'UPGRADE_WARNING: コレクション SelListVw.ListItems の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
                            'UPGRADE_WARNING: コレクション SelListVw.ListItems.Item().ListSubItems の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
                            'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                            SnwMT02F07.MituNM = SpcToNull(Trim(SelListVw.Items.Item(SelListVw.FocusedItem.Index).SubItems.Item(5).Text), "")
                            'UPGRADE_WARNING: コレクション SelListVw.ListItems の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
                            'UPGRADE_WARNING: コレクション SelListVw.ListItems.Item().ListSubItems の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
                            'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                            SnwMT02F07.TOKUCD = SpcToNull(Trim(SelListVw.Items.Item(SelListVw.FocusedItem.Index).SubItems.Item(3).Text), "")
                            'VB6.ShowForm(SnwMT02F07, VB6.FormShowConstants.Modal, Me)
                            SnwMT02F07.ShowDialog(Me)
                            PreviousControl.Focus()
                            '                    SelListVw.SelectedItem.Selected = False
                        Else
                            PreviousControl.Focus()
                        End If
                    Else
                        PreviousControl.Focus()
                    End If
                Else
                    PreviousControl.Focus()
                End If
            Case 1
            Case 2
            Case 3
                '見積入力画面呼び出し
                With SnwMT01F00
                    .ParentForm_Renamed = Me
                    .MituNo = 0
                    On Error Resume Next
                    .Show()
                    'If Err.Number = 0 Then
                    'Else
                    Me.Hide()
                    'End If
                    On Error GoTo 0
                End With
            Case 4
            Case 5
            Case 6
                '経過表出力
                CLKFLG = True
                '            SetResultValues
                Cb出力_Click()
            Case 7
            Case 8
            Case 9
                If MsgBoxResult.Yes = NoYes("現在の編集内容を破棄します。", Me.Text) Then
                    System.Windows.Forms.Application.DoEvents()
                    Call FormInitialize()
                Else
                    PreviousControl.Focus()
                End If
            Case 10
                CLKFLG = True
                SetResultValues()
            Case 11
                Me.Close()
                CLK2F = False
                Exit Sub
        End Select
        LcbFunc(Index).TabStop = False
        'ボタン２重起動防止フラグの初期化
        CLK2F = False
    End Sub

    ' 指定したインデックスのボタン動作を実行
    Private Sub LcbFunc_Action(index As Integer)
        If index >= 0 AndAlso index < LcbFunc.Count Then
            ' ボタンのクリックイベントを呼び出す
            LcbFunc(index).PerformClick()
        End If
    End Sub

    'UPGRADE_WARNING: イベント SnwMT01F00S.Resize は、フォームが初期化されたときに発生します。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"' をクリックしてください。
    Private Sub SnwMT01F00S_Resize(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Me.Resize
        On Error Resume Next
        If Me.WindowState <> System.Windows.Forms.FormWindowState.Minimized Then
            'フォーム最小（幅）制御
            'If VB6Conv.PixelsToTwipsX(Me.Width) < MeWidth Then
            '    Me.Width = VB6Conv.TwipsToPixelsX(MeWidth)
            'End If
            'フォーム最小（高さ）制御・リストビューの高さをフォームの高さに比例
            'If VB6Conv.PixelsToTwipsY(Me.Height) < MeHeightLimit Then
            '    Me.Height = VB6Conv.TwipsToPixelsY(MeHeightLimit)
            'End If
            'リストビューの高さ・ボタン位置
            'SelListVw.Width = VB6Conv.TwipsToPixelsX(VB6Conv.PixelsToTwipsX(Me.ClientRectangle.Width) - (VB6Conv.PixelsToTwipsX(SelListVw.Left) * 2)) '2012/09/13 ADD
            'WkHeight = VB6Conv.PixelsToTwipsY(PicFunction.Top) - (VB6Conv.PixelsToTwipsY(SelListVw.Top) + VB6Conv.PixelsToTwipsY(SelListVw.Height))
            'SelListVw.Height = VB6Conv.TwipsToPixelsY(VB6Conv.PixelsToTwipsY(Me.ClientRectangle.Height) - VB6Conv.PixelsToTwipsY(SelListVw.Top) - (WkHeight) - VB6Conv.PixelsToTwipsY(PicFunction.Height))
            SelListVw.Width = Me.ClientSize.Width - 20
            SelListVw.Height = Me.ClientSize.Height - 272

            CmdFind.Left = VB6Conv.TwipsToPixelsX(VB6Conv.PixelsToTwipsX(Me.SelListVw.Left) + VB6Conv.PixelsToTwipsX(SelListVw.Width) - VB6Conv.PixelsToTwipsX(CmdFind.Width))
        End If
        On Error GoTo 0
    End Sub

	Private Sub SnwMT01F00S_KeyDown(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        Dim ctl As System.Windows.Forms.Control
        On Error GoTo Form_KeyDown_Err
        Select Case KeyCode
            Case System.Windows.Forms.Keys.Escape
                KeyCode = 0
                Exit Sub
            Case System.Windows.Forms.Keys.F1 To System.Windows.Forms.Keys.F12
                On Error Resume Next
                'ctl = Me.Controls("CbFunc" & (KeyCode - Keys.F1 + 1).ToString())
                'If Err.Number = 0 Then
                '    If ctl IsNot Nothing Then
                '        If ctl.Text <> String.Empty Then
                '            If ctl.Enabled = True Then
                '                ctl.Focus()
                '                If Err.Number = 0 Then
                '                    SendReturnKey()
                '                End If
                '            End If
                '        End If
                '    End If
                'End If
                'UPGRADE_NOTE: オブジェクト ctl をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
                'ctl = Nothing
                Select Case KeyCode
                    Case Keys.F1
                        LcbFunc_Action(0)
                    Case Keys.F2
                        LcbFunc_Action(1)
                    Case Keys.F3
                        LcbFunc_Action(2)
                    Case Keys.F4
                        LcbFunc_Action(3)
                    Case Keys.F5
                        LcbFunc_Action(4)
                    Case Keys.F6
                        LcbFunc_Action(5)
                    Case Keys.F7
                        LcbFunc_Action(6)
                    Case Keys.F8
                        LcbFunc_Action(7)
                    Case Keys.F9
                        LcbFunc_Action(8)
                    Case Keys.F10
                        LcbFunc_Action(9)
                    Case Keys.F11
                        LcbFunc_Action(10)
                    Case Keys.F12
                        LcbFunc_Action(11)
                End Select
                KeyCode = 0
                On Error GoTo 0
                Exit Sub
        End Select

        '2017/11/30 ADD↓
        'かくしきのう
        Select Case KeyCode
            Case System.Windows.Forms.Keys.V
                'If Shift = (VB6.ShiftConstants.AltMask Or VB6.ShiftConstants.CtrlMask Or VB6.ShiftConstants.ShiftMask) Then
                If (Control.ModifierKeys And Keys.Alt) = Keys.Alt AndAlso
                   (Control.ModifierKeys And Keys.Control) = Keys.Control AndAlso
                   (Control.ModifierKeys And Keys.Shift) = Keys.Shift Then
                    KeyCode = 0
                    If NoYes("表示します。") = MsgBoxResult.Yes Then
                        LcbFunc(0).Text = "見積ｺﾋﾟｰ"
                        Inform("表示しました。")
                    End If
                End If
        End Select
        '2017/11/30 ADD↑

        Exit Sub
Form_KeyDown_Err:
        MsgBox(Err.Number & " " & Err.Description)
    End Sub

    Private Sub SnwMT01F00S_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        On Error GoTo Form_KeyPress_Err
        Select Case KeyAscii
            Case System.Windows.Forms.Keys.Escape
                Me.Close()
            Case System.Windows.Forms.Keys.Return
                'ビープ音消去用
                'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
                If Me.ActiveControl.Name <> "SelListVw" Then
                    KeyAscii = 0
                End If
        End Select
        GoTo EventExitSub
Form_KeyPress_Err:
        MsgBox(Err.Number & " " & Err.Description)
        GoTo EventExitSub
EventExitSub:
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub SnwMT01F00S_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Dim Cancel As Boolean = e.Cancel
        Dim UnloadMode As System.Windows.Forms.CloseReason = e.CloseReason
        If Loaded Then
            If MsgBoxResult.Yes = NoYes("現在の処理を終了します。") Then
                'UPGRADE_NOTE: オブジェクト idc() をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
                idc(0) = Nothing
                'UPGRADE_NOTE: オブジェクト idc() をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
                idc(1) = Nothing
                'UPGRADE_NOTE: オブジェクト PreviousControl をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
                PreviousControl = Nothing
            Else
                If PreviousControl IsNot Nothing Then
                    PreviousControl.Focus()
                End If
                Cancel = True
                e.Cancel = Cancel
                Exit Sub
            End If
        End If
        e.Cancel = Cancel
        Me.Dispose()
    End Sub

    Private Sub FormInitialize()
        Call SetupBlank()
    End Sub

	Private Sub SetupBlank()
		Try
			ClearControls(Me)
			SelListVw.Items.Clear()
		Catch ex As Exception
		End Try
	End Sub

	Private Sub ClearControls(ByVal parent As Control)
		For Each ctl As Control In parent.Controls
            'UPGRADE_WARNING: TypeOf に新しい動作が指定されています。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
            If TypeOf ctl Is System.Windows.Forms.TextBox Then
                ctl.Text = vbNullString
            End If
            'UPGRADE_WARNING: TypeOf に新しい動作が指定されています。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
            If TypeOf ctl Is ExText.ExTextBox Then
                ctl.Text = vbNullString
            End If
            'UPGRADE_WARNING: TypeOf に新しい動作が指定されています。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
            If TypeOf ctl Is ExNmText.ExNmTextBox Then
                ctl.Text = vbNullString
            End If
            'UPGRADE_WARNING: TypeOf に新しい動作が指定されています。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
            If TypeOf ctl Is ExDateText.ExDateTextBoxY Then
                ctl.Text = vbNullString
            End If
            'UPGRADE_WARNING: TypeOf に新しい動作が指定されています。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
            If TypeOf ctl Is ExDateText.ExDateTextBoxM Then
                ctl.Text = vbNullString
            End If
            'UPGRADE_WARNING: TypeOf に新しい動作が指定されています。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
            If TypeOf ctl Is ExDateText.ExDateTextBoxD Then
                ctl.Text = vbNullString
            End If
            'UPGRADE_WARNING: TypeOf に新しい動作が指定されています。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
            If TypeOf ctl Is System.Windows.Forms.Label Then
                If ctl.Name Like "rf_*" Then
                    ctl.Text = vbNullString
                End If
            End If

			' 子コントロールがある場合、再帰的に処理
			If ctl.HasChildren Then
			    ClearControls(ctl)
			End If
		Next
	End Sub

    Private Sub Cb変更_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Cb変更.Click
        '    If Item_Check(cb変更.TabIndex) = False Then Exit Sub
        If OpenSaveDialog(GXLSPath, ProfileKey, "xlsx") = True Then
            WriteIni("Path", ProfileKey, GXLSPath, INIFile)
        End If
        'UPGRADE_WARNING: TextBox プロパティ txDir.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
        [txDir].Text = CompactPathEx(GXLSPath, [txDir].MaxLength)
    End Sub

    Private Sub Cb出力_Click()
        Dim st As Integer

        '    If Item_Check([cb出力].TabIndex) = False Then
        '        Exit Sub
        '    End If

        If [txDir].Text = vbNullString Then
            CriticalAlarm("ファイルを指定して下さい。")
            Cb変更.Focus()
            Exit Sub
        End If

        'ボタンを押されたのが２回目ならば抜ける
        '    If CLK2F = True Then
        '        Exit Sub
        '    End If
        'ボタン２重起動防止フラグのセット
        '    CLK2F = True

        If MsgBoxResult.No = YesNo("経過確認表をExcelBookファイルに出力します。") Then GoTo cb出力_Exit
        If FileOverWriteCheck(GXLSPath) <> 0 Then GoTo cb出力_Exit 'オーバーライトをチェックします

        'st = 0
        If Download() = False Then
            st = -1
            Inform("該当データ無し")
            PreviousControl.Focus()
        Else
            st = 経過確認表出力(GXLSPath)
        End If
        If st = 0 Then
            If MsgBoxResult.Yes = Question("出力を完了しました。" & vbCrLf & "今すぐ開きますか？", MsgBoxResult.Yes) Then
                ShellExecute(Me.Handle, vbNullString, "Excel.exe", """" & GXLSPath & """", vbNullString, AppWinStyle.MaximizedFocus)
            End If
            '        Me.Unload
        End If
        ReleaseRs(grs)
cb出力_Exit:
        'ボタン２重起動防止フラグの初期化
        CLK2F = False
    End Sub

    Private Sub CmdFind_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdFind.Enter
        Item_Check((CmdFind.TabIndex))
    End Sub

    Private Sub CmdFind_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdFind.Click
        On Error GoTo CmdFind_Click_Err
        If Not Download() Then
            CheckAlarm("該当データがありません。")
            rf_ListCount.Text = ""
            [tx_得意先CD].Focus()
            Exit Sub
        Else
            If SetupItems() Then
                ' スクロールバーを確実に表示するためのカラム幅調整
                Dim totalColumnWidth As Integer = 0
                For Each col As ColumnHeader In SelListVw.Columns
                    totalColumnWidth += col.Width
                Next
                'SelListVw.Width = totalColumnWidth - 100 ' コントロールの幅をカラム幅より小さくする
                SelListVw.Width = Me.ClientSize.Width - 20
                CmdFind.Focus()
                Exit Sub
            End If
        End If

        '検索ＯＫ処理
        '    CLKFLG = True
        SelListVw.Focus()
        Exit Sub
CmdFind_Click_Err:
        HourGlass(False)
        MsgBox(Err.Number & " " & Err.Description)
        Exit Sub
    End Sub

    Private Sub SelListVw_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles SelListVw.Enter
        If SelListVw.Items.Count = 0 Then
            CmdFind.Focus()
        Else
            If SelListVw.FocusedItem IsNot Nothing Then
                SelListVw.FocusedItem.Selected = True
            Else
                SelListVw.Items(0).Selected = True
            End If
        End If
    End Sub

    Private Sub SelListVw_DoubleClick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles SelListVw.DoubleClick
        CLKFLG = True
        SetResultValues()
    End Sub

    'UPGRADE_ISSUE: MSComctlLib.ListView イベント SelListVw.ItemClick はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="ABD9AF39-7E24-4AFF-AD8D-3675C1AA3054"' をクリックしてください。
    'Private Sub SelListVw_ItemClick(ByVal Item As System.Windows.Forms.ListViewItem)
    '    CLKFLG = True
    'End Sub

	Private Sub SelListVw_MouseClick(sender As Object, e As MouseEventArgs) Handles SelListVw.MouseClick
		CLKFLG = True
	End Sub

    Private Sub SelListVw_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles SelListVw.Leave
        '--2005/07/05.ADD
        Dim SelListWidth As String
        Dim i As Integer
        'Debug.WriteLine($"Item.Count: {SelListVw.Items.Count}")
        'Debug.WriteLine($"Columns.Count: {SelListVw.Columns.Count}")
        With SelListVw
            'SelListWidth = vbNullString
            SelListWidth = ""
            For i = 0 To .Columns.Count - 1
                If SelListWidth <> "" Then SelListWidth = SelListWidth & ","
                'UPGRADE_WARNING: コレクション SelListVw.ColumnHeaders の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
                SelListWidth = SelListWidth & VB6Conv.PixelsToTwipsX(.Columns.Item(i).Width)
            Next
        End With
        '設定情報の保持
        WriteIni("SelListWidth", "SE23F00_SPD", SelListWidth, INIFile)
    End Sub

    Private Sub SelListVw_MouseDown(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles SelListVw.MouseDown
        Dim Button As Integer = eventArgs.Button \ &H100000
        Dim Shift As Integer = System.Windows.Forms.Control.ModifierKeys \ &H10000
        Dim X As Single = VB6Conv.PixelsToTwipsX(eventArgs.X)
        Dim Y As Single = VB6Conv.PixelsToTwipsY(eventArgs.Y)
        CLKFLG = False
    End Sub

    Private Sub SelListVw_KeyDown(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyEventArgs) Handles SelListVw.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        Select Case KeyCode
            Case System.Windows.Forms.Keys.Return
                'KeyCode = 0
                CLKFLG = True
                SetResultValues()
            Case System.Windows.Forms.Keys.Up
                If SelListVw.FocusedItem.Index = 0 Then
                    'KeyCode = 0
                    '                tx_e見積日D.SetFocus
                    [tx_e請求予定D].Focus() '2016/10/26 ADD
                End If
        End Select
    End Sub

    Private Sub SelListVw_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles SelListVw.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        Dim itmFound As System.Windows.Forms.ListViewItem ' FoundItem 変数。

        itmFound = SelListVw.FindItemWithText(Space(1) & Chr(KeyAscii), True, 0, True)
        KeyAscii = 0
        ' EnsureVisible メソッドを使用して
        ' コントロールをスクロールし、ListItem を選択します。
        If Not (itmFound Is Nothing) Then
            'UPGRADE_WARNING: MSComctlLib.ListItem メソッド itmFound.EnsureVisible には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
            itmFound.EnsureVisible() ' リスト ビュー コントロールをスクロールして、検出された ListItem を表示します。
            itmFound.Selected = True ' ListItem を選択します。
            ' コントロールにフォーカスを返し、選択した内容を表示します。
            SelListVw.Focus()
        End If
        'UPGRADE_NOTE: オブジェクト itmFound をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
        'itmFound = Nothing
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub SelListVw_ColumnClick(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.ColumnClickEventArgs) Handles SelListVw.ColumnClick
        Dim ColumnHeader As System.Windows.Forms.ColumnHeader = SelListVw.Columns(eventArgs.Column) '2005/07/04 ADD
        ' ColumnHeader オブジェクトがクリックされると、その列のサブアイテムで、
        ' リスト ビュー コントロールが並べ替えられます。

        'UPGRADE_ISSUE: MSComctlLib.ListView プロパティ SelListVw.SortKey はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
        'NOTE SS If SelListVw.SortKey = ColumnHeader.Index - 1 Then
        If SelListVw.SortStyle = SortableListView.SortStyles.SortSelectedColumn Then
            If SelListVw.Sorting = System.Windows.Forms.SortOrder.Ascending Then
                SelListVw.Sorting = System.Windows.Forms.SortOrder.Descending
            Else
                SelListVw.Sorting = System.Windows.Forms.SortOrder.Ascending
            End If
        Else
            '前回並び替えされていない場合は昇順で。
            SelListVw.Sorting = System.Windows.Forms.SortOrder.Ascending
        End If
        ' SortKey を、ColumnHeader の Index プロパティから 1 を引いた値に設定します。
        'UPGRADE_ISSUE: MSComctlLib.ListView プロパティ SelListVw.SortKey はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
        'NOTE SS SelListVw.SortKey = ColumnHeader.Index - 1
        SelListVw.SortStyle = SortableListView.SortStyles.SortSelectedColumn
        ' Sorted プロパティを真 (True) に設定してリストを並べ替えます。
        SelListVw.Sort()
    End Sub

    Private Sub SetResultValues()
        If Not CLKFLG Then Exit Sub

        ''    HourGlass True
        '2020/10/30 ADD↓
        If GetAppLockCount("見積番号") >= 3 Then
            MsgBox("複数起動制限！")
            '        Call Unload(Me)
            Exit Sub
        End If
        '2020/10/30 ADD↑

        If SelListVw.SelectedItems.Count <> 0 Then
            If SelListVw.FocusedItem IsNot Nothing Then
                If SelListVw.FocusedItem.Selected = False Then GoTo SetResultValues_Correct
            End If

            '2004/11/29 ADD
            ''        If Chk締日To見積(Trim$(SelListVw.ListItems(SelListVw.SelectedItem.Index))) = False Then
            ''            CriticalAlarm "更新済みの為、修正できません。"
            ''            Exit Sub
            ''        End If

            With SnwMT01F00
                .ParentForm_Renamed = Me
                'UPGRADE_WARNING: コレクション SelListVw.ListItems の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
                .MituNo = CInt(Trim(SelListVw.Items.Item(SelListVw.FocusedItem.Index).Text))
                On Error Resume Next
                .Show()
                If Err.Number = 0 Then
                Else
                    Me.Hide()
                End If
                On Error GoTo 0
            End With
        End If
SetResultValues_Correct:
        '    HourGlass False
    End Sub

    '2004/11/29 ADD
    'Public Function Chk締日To見積(No As Long) As Boolean
    '    Dim rs As ADODB.Recordset
    '    Dim SQL As String
    '
    '    On Error GoTo Chk締日To見積_Err
    '
    '    'マウスポインターを砂時計にする
    '    HourGlass True
    '
    '    Dim ShimeDate As Date
    '
    '    ShimeDate = GetDates("月次更新日")
    '
    '
    '    SQL = "SELECT * FROM TD見積" _
    '        & " WHERE 見積番号 = " & No
    '
    '    Set rs = OpenRs(SQL, Cn, adOpenKeyset, adLockReadOnly)
    '    With rs
    '        If .EOF Then
    '            ReleaseRs rs
    '            GoTo Chk締日To見積_Correct
    '        Else
    '            If ![仕入日付] < ShimeDate Then
    '                GoTo Chk締日To見積_Correct
    '            ElseIf ![売上日付] < ShimeDate Then
    '                GoTo Chk締日To見積_Correct
    '            End If
    '        End If
    '    End With
    '
    '    Chk締日To見積 = True
    'Chk締日To見積_Correct:
    '    ReleaseRs rs
    '    HourGlass False
    '    Exit Function
    'Chk締日To見積_Err:
    '    MsgBox Err.Number & " " & Err.Description
    '    HourGlass False
    'End Function

    Private Function Item_Check(ByRef ItemNo As Integer) As Boolean
        On Error GoTo Item_Check_Err
        Item_Check = False

        If Loaded = False Then
            Exit Function
        End If

        'キー項目「得意先CD」のチェック
        If ItemNo > [tx_得意先CD].TabIndex Then
            If ISInt(([tx_得意先CD].Text)) Then
                'UPGRADE_WARNING: TextBox プロパティ tx_得意先CD.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
                [tx_得意先CD].Text = CType([tx_得意先CD].Text, Integer).ToString(New String("0"c, [tx_得意先CD].MaxLength))
            End If
        End If

        'キー項目「納入先CD」のチェック
        If ItemNo > [tx_納入先CD].TabIndex Then
            If ISInt(([tx_納入先CD].Text)) Then
                'UPGRADE_WARNING: TextBox プロパティ tx_納入先CD.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
                [tx_納入先CD].Text = CType([tx_納入先CD].Text, Integer).ToString(New String("0"c, [tx_納入先CD].MaxLength))
            End If
        End If

        'ウエルシア物件区分のチェック
        If ItemNo > [tx_ウエルシア物件区分].TabIndex Then
            '        If IsCheckText(tx_ウエルシア物件区分) = False Then
            '            CriticalAlarm "ウエルシア物件区分が未入力です。"
            '            [tx_ウエルシア物件区分].Undo
            '            [tx_ウエルシア物件区分].SetFocus
            '            Exit Function
            '        End If

            '--- 入力値をチェック用クラスへ格納
            If IsCheckText([tx_ウエルシア物件区分]) = True Then
                With cWelBukkenKubun
                    .Initialize()
                    .ウエルシア物件区分CD = tx_ウエルシア物件区分.Text
                    If .GetbyID = False Then
                        CriticalAlarm("指定の物件区分は存在しません。")
                        'UPGRADE_WARNING: オブジェクト tx_ウエルシア物件区分.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                        tx_ウエルシア物件区分.Undo()
                        [tx_ウエルシア物件区分].Focus()
                        '                    HWELBKNCD = Null
                        Exit Function
                    Else
                        [tx_ウエルシア物件区分].Text = .ウエルシア物件区分CD
                        rf_ウエルシア物件区分名.Text = .ウエルシア物件区分名
                    End If
                End With
            Else
                rf_ウエルシア物件区分名.Text = ""
            End If
        End If
        '2015/06/12 ADD↑

        '開始見積日付のチェック
        If ItemNo > idc(0).TabIndex Then
            If Not idc(0).IsAllNull Then
                If IsDate(idc(0).Text) = False Then
                    'Debug.WriteLine($"idc(0).Text: {idc(0).Text}")
                    CriticalAlarm("開始見積日が不正です。")
                    'UPGRADE_WARNING: オブジェクト idc().ErrorPart.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                    'NOTE SS idc(0).ErrorPart.Undo()
                    'NOTE SS idc(0).ErrorPart.Focus()
                    tx_s見積日Y.Focus()
                    Exit Function
                End If
            End If
        End If

        '終了見積日付のチェック
        If ItemNo > idc(1).TabIndex Then
            If Not idc(1).IsAllNull Then
                If IsDate(idc(1).Text) = False Then
                    CriticalAlarm("終了見積日が不正です。")
                    'UPGRADE_WARNING: オブジェクト idc().ErrorPart.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                    'NOTE SS idc(1).ErrorPart.Undo()
                    'NOTE SS idc(1).ErrorPart.Focus()
                    tx_e見積日Y.Focus()
                    Exit Function
                End If
            End If
        End If

        '開始S納期のチェック
        If ItemNo > idc(2).TabIndex Then
            If Not idc(2).IsAllNull Then
                If IsDate(idc(2).Text) = False Then
                    CriticalAlarm("開始納期が不正です。")
                    'UPGRADE_WARNING: オブジェクト idc().ErrorPart.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                    'NOTE SS idc(2).ErrorPart.Undo()
                    'NOTE SS idc(2).ErrorPart.Focus()
                    tx_s開始納期Y.Focus()
                    Exit Function
                End If
            End If
        End If

        '終了E納期のチェック
        If ItemNo > idc(3).TabIndex Then
            If Not idc(3).IsAllNull Then
                If IsDate(idc(3).Text) = False Then
                    CriticalAlarm("終了納期が不正です。")
                    'UPGRADE_WARNING: オブジェクト idc().ErrorPart.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                    'NOTE SS idc(3).ErrorPart.Undo()
                    'NOTE SS idc(3).ErrorPart.Focus()
                    tx_e開始納期Y.Focus()
                    Exit Function
                End If
            End If
        End If
        '開始仕入日のチェック
        If ItemNo > idc(8).TabIndex Then
            If Not idc(8).IsAllNull Then
                If IsDate(idc(8).Text) = False Then
                    CriticalAlarm("開始仕入日が不正です。")
                    'UPGRADE_WARNING: オブジェクト idc().ErrorPart.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                    'NOTE SS idc(8).ErrorPart.Undo()
                    'NOTE SS idc(8).ErrorPart.Focus()
                    tx_s仕入日Y.Focus()
                    Exit Function
                End If
            End If
        End If
        '終了仕入日のチェック
        If ItemNo > idc(9).TabIndex Then
            If Not idc(9).IsAllNull Then
                If IsDate(idc(9).Text) = False Then
                    CriticalAlarm("終了仕入日が不正です。")
                    'UPGRADE_WARNING: オブジェクト idc().ErrorPart.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                    'NOTE SS idc(9).ErrorPart.Undo()
                    'NOTE SS idc(9).ErrorPart.Focus()
                    tx_e仕入日Y.Focus()
                    Exit Function
                End If
            End If
        End If

        '開始完了日のチェック
        If ItemNo > idc(4).TabIndex Then
            If Not idc(4).IsAllNull Then
                If IsDate(idc(4).Text) = False Then
                    CriticalAlarm("開始完了日が不正です。")
                    'UPGRADE_WARNING: オブジェクト idc().ErrorPart.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                    'NOTE SS idc(4).ErrorPart.Undo()
                    'NOTE SS idc(4).ErrorPart.Focus()
                    tx_s完了日Y.Focus()
                    Exit Function
                End If
            End If
        End If
        '終了完了日のチェック
        If ItemNo > idc(5).TabIndex Then
            If Not idc(5).IsAllNull Then
                If IsDate(idc(5).Text) = False Then
                    CriticalAlarm("終了完了日が不正です。")
                    'UPGRADE_WARNING: オブジェクト idc().ErrorPart.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                    'NOTE SS idc(5).ErrorPart.Undo()
                    'NOTE SS idc(5).ErrorPart.Focus()
                    tx_e完了日Y.Focus()
                    Exit Function
                End If
            End If
        End If

        '開始請求予定のチェック
        If ItemNo > idc(6).TabIndex Then
            If Not idc(6).IsAllNull Then
                If IsDate(idc(6).Text) = False Then
                    CriticalAlarm("開始請求予定が不正です。")
                    'UPGRADE_WARNING: オブジェクト idc().ErrorPart.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                    'NOTE SS idc(6).ErrorPart.Undo()
                    'NOTE SS idc(6).ErrorPart.Focus()
                    tx_s請求予定Y.Focus()
                    Exit Function
                End If
            End If
        End If
        '終了請求予定のチェック
        If ItemNo > idc(7).TabIndex Then
            If Not idc(7).IsAllNull Then
                If IsDate(idc(7).Text) = False Then
                    CriticalAlarm("終了請求予定が不正です。")
                    'UPGRADE_WARNING: オブジェクト idc().ErrorPart.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                    'NOTE SS idc(7).ErrorPart.Undo()
                    'NOTE SS idc(7).ErrorPart.Focus()
                    tx_e請求予定Y.Focus()
                    Exit Function
                End If
            End If
        End If
        Item_Check = True
        Exit Function
Item_Check_Err:
        MsgBox(Err.Number & " " & Err.Description)
    End Function

    Public Function Download() As Boolean
        'SnwMT01F00からも使用する
        Dim whr As String
        Dim ret As String '種別変換用

        Download = False
        HourGlass(True)

        '---2003/12/04.DEL

        whr = ""

        '2015/01/09 ADD
        sql = "SELECT  MT.見積番号, MT.見積日付, MT.物件種別, MT.得意先CD, MT.納入先CD, MT.見積件名,"
        sql += "        MT.合計金額 + MT.出精値引 AS 合計金額"
        sql += "        ,MT.ウエルシアリース区分"
        sql += "        ,MT.ウエルシア物件区分CD,WB.ウエルシア物件区分名"
        sql += "        ,MT.YKサプライ区分,MT.YK物件区分,MT.YK請求区分" '2022/10/10 ADD
        sql += "        ,MT.B請求管轄区分, MT.BtoB番号" '2022/10/10 ADD
        sql += "        ,MT.見積書出力日,MHK.発注書発行日付"
        sql += "        ,MT.納期S" '2016/10/26 ADD
        sql += "        ,MT.仕入日付"
        sql += "        ,未仕入件数 = ISNULL(CNT.件数,0)"
        sql += "        ,MT.売上日付"
        sql += "        ,MHK.完了日付,MHK.請求予定日,MHK.経過備考1,MHK.経過備考2"
        sql += "        ,USH.請求書発行日付"
        sql += "        ,MT.原価率"
        sql += "        ,MT.登録変更日"
        sql += "        ,物件番号" '2015/11/26 ADD
        sql += "        ,見積確定区分 = ISNULL(MHK.見積確定区分,0)" '2021/04/11 ADD
        sql += "     FROM TD見積 AS MT"
        sql += "            LEFT JOIN TD売上請求H AS USH"
        sql += "                ON MT.見積番号 = USH.見積番号"
        sql += "            LEFT JOIN TD見積_経過 AS MHK"
        sql += "                ON MT.見積番号 = MHK.見積番号"
        sql += "            LEFT JOIN TMウエルシア物件区分 AS WB"
        sql += "                ON MT.ウエルシア物件区分CD = WB.ウエルシア物件区分CD"
        sql += "            LEFT JOIN ("
        sql += "                    SELECT 見積番号,件数 = COUNT(見積明細連番)"
        sql += "                        FROM ("
        sql += "                                SELECT  MT.見積番号, MT.見積明細連番"
        sql += "                                    FROM    TD見積シートM AS MT"
        sql += "                                        LEFT JOIN"
        sql += "                                              (SELECT   見積明細連番, SUM(仕入数量) AS 仕入数量"
        sql += "                                                    FROM TD仕入明細内訳"
        sql += "                                                    GROUP BY    見積明細連番"
        sql += "                                                ) AS SRU"
        sql += "                                                ON MT.見積明細連番 = SRU.見積明細連番"
        sql += "                                    WHERE (MT.発注数 - IsNull(SRU.仕入数量, 0) <> 0)"
        sql += "                            ) AS DATA_A"
        sql += "                        GROUP BY 見積番号"
        sql += "                    ) AS CNT"
        sql += "                    ON MT.見積番号 = CNT.見積番号"


        '売上種別
        Select Case [tx_売上種別].Text
            Case "0"
                whr += "(MHK.発注書発行日付 IS NULL AND MT.仕入日付 IS NULL)"
            Case "1" '発注済
                whr += "MHK.発注書発行日付 IS NOT NULL"
            Case "2" '売上済
                whr += "MT.売上日付 IS NOT NULL"
        End Select

        '得意先
        If IsCheckText([tx_得意先CD]) = True Then
            If whr <> "" Then
                whr += " and "
            End If
            whr += "得意先CD LIKE '" & SQLString([tx_得意先CD].Text) & "%'"
        End If
        '納入先
        If IsCheckText([tx_納入先CD]) = True Then
            If whr <> "" Then
                whr += " and "
            End If
            whr += "納入先CD Like '" & SQLString([tx_納入先CD].Text) & "%'"
        End If
        '2009/12/02 ADD↓
        '担当者
        If IsCheckText([tx_担当者CD]) = True Then
            If whr <> "" Then
                whr += " and "
            End If
            whr += "担当者CD Like '%" & SQLString([tx_担当者CD].Text) & "%'"
        End If
        '2009/12/02 ADD↑

        '2023/04/18 ADD↓
        '業種区分
        Select Case [tx_業種区分].Text
            Case ""
            Case Else
                If whr <> "" Then
                    whr += " AND "
                End If
                whr += "業種区分 =" & [tx_業種区分].Text
        End Select
        '2023/04/18 ADD↑

        '2015/05/28 ADD↓
        '物件種別
        Select Case [tx_物件種別].Text
            Case ""
            Case Else
                If whr <> "" Then
                    whr += " AND "
                End If
                whr += "物件種別 =" & [tx_物件種別].Text
        End Select
        '2015/05/28 ADD↑

        '見積番号
        If IsCheckText([tx_s見積番号]) = True Or IsCheckText([tx_e見積番号]) = True Then
            If whr <> "" Then
                whr += " and "
            End If
            whr += SQLIntRange("MT.見積番号", Trim([tx_s見積番号].Text), Trim([tx_e見積番号].Text),  , False)
        End If
        '見積件名
        If IsCheckText([tx_見積件名]) = True Then
            If whr <> "" Then
                whr += " and "
            End If
            whr += "見積件名 Like '%" & SQLString([tx_見積件名].Text) & "%'"
        End If
        '見積金額
        If IsCheckText([tx_s見積金額]) = True Or IsCheckText([tx_e見積金額]) = True Then
            If whr <> "" Then
                whr += " and "
            End If
            whr += SQLCurRange("MT.合計金額 + MT.出精値引", Trim([tx_s見積金額].Text), Trim([tx_e見積金額].Text),  , False)
        End If

        '物件番号
        If IsCheckText([tx_s物件番号]) = True Or IsCheckText([tx_e物件番号]) = True Then
            If whr <> "" Then
                whr += " and "
            End If
            whr += SQLIntRange("MT.物件番号", Trim([tx_s物件番号].Text), Trim([tx_e物件番号].Text),  , False)
        End If

        'ウエルシアリース区分
        Select Case [tx_ウエルシアリース区分].Text
            Case ""
            Case Else
                If whr <> "" Then
                    whr += " AND "
                End If
                whr += "MT.ウエルシアリース区分 =" & [tx_ウエルシアリース区分].Text
        End Select

        'ウエルシア物件区分
        Select Case [tx_ウエルシア物件区分].Text
            Case ""
            Case Else
                If whr <> "" Then
                    whr += " AND "
                End If
                whr += "MT.ウエルシア物件区分CD =" & [tx_ウエルシア物件区分].Text
        End Select

        'Debug.WriteLine(idc(0).GetType().ToString())

        '見積日
        'If idc(0).Text <> vbNullString Or idc(1).Text <> vbNullString Then
        If idc(0).Text <> "" Or idc(1).Text <> "" Then
            If whr <> "" Then
                whr += " and "
            End If
            whr += SQLDateRange("MT.見積日付", idc(0).Text, idc(1).Text, DBType, False)
        End If

        '2016/10/26 ADD↓
        '納期S
        'If idc(2).Text <> vbNullString Or idc(3).Text <> vbNullString Then
        If idc(2).Text <> "" Or idc(3).Text <> "" Then
            If whr <> "" Then
                whr += " and "
            End If
            whr += SQLDateRange("MT.納期S", idc(2).Text, idc(3).Text, DBType, False)
        End If
        '2016/10/26 ADD↑

        '2020/04/16 ADD↓
        '完了日
        'If idc(4).Text <> vbNullString Or idc(5).Text <> vbNullString Then
        If idc(4).Text <> "" Or idc(5).Text <> "" Then
            If whr <> "" Then
                whr += " and "
            End If
            whr += SQLDateRange("MHK.完了日付", idc(4).Text, idc(5).Text, DBType, False)
        End If
        '2020/04/16 ADD↑

        '2020/04/16 ADD↓
        '請求予定日
        'If idc(6).Text <> vbNullString Or idc(7).Text <> vbNullString Then
        If idc(6).Text <> "" Or idc(7).Text <> "" Then
            If whr <> "" Then
                whr += " and "
            End If
            whr += SQLDateRange("MHK.請求予定日", idc(6).Text, idc(7).Text, DBType, False)
        End If

        '仕入日
        'If idc(8).Text <> vbNullString Or idc(9).Text <> vbNullString Then
        If idc(8).Text <> "" Or idc(9).Text <> "" Then
            If whr <> "" Then
                whr += " and "
            End If
            whr += SQLDateRange("MT.仕入日付", idc(8).Text, idc(9).Text, DBType, False)
        End If
        '2020/04/16 ADD↑

        '2021/04/11 ADD↓
        '見積確定区分
        Select Case [tx_見積確定区分].Text
            Case ""
            Case Else
                If whr <> "" Then
                    whr += " AND "
                End If
                whr += "ISNULL(MHK.見積確定区分,0) =" & [tx_見積確定区分].Text
        End Select
        '2021/04/11 ADD↑

        If whr <> "" Then
            whr = " WHERE " & whr
        End If
        sql = sql & whr & " ORDER BY MT.得意先CD,MT.見積日付 DESC, MT.見積番号 DESC"

        '    grs.CursorLocation = adUseClient

        grs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)
        If grs.EOF Then
            Download = False
        Else
            Download = True
        End If
Download_correct:
        '    Set cmd = Nothing
        On Error GoTo 0
        HourGlass(False)
        Exit Function
Download_Err:
        CriticalAlarm(Err.Number & " " & Err.Description)
        '    Set cmd = Nothing
        HourGlass(False)
    End Function

    Public Function SetupItems() As Boolean
        'Private Function SetupItems() As Boolean
        'SnwMT01F00からも使用する
        Dim itmX As System.Windows.Forms.ListViewItem
        'Dim itmFound As System.Windows.Forms.ListViewItem ' FoundItem 変数。
        'Dim FindListText As String
        Dim buf As String '文字成型用
        'Dim i As Integer
        SetupItems = False
        HourGlass(True)

        '    LockWindowUpdate Me.Hwnd

        SelListVw.Items.Clear()
        Do Until grs.EOF
            buf = New String("0", 7) & "0" 'ひとつ余計にする
            If ISInt((grs.Fields(0).Value)) Then
                buf = RSet(CStr(grs.Fields(0).Value), Len(buf))
            Else
                buf = Space(1) & CStr(grs.Fields("見積番号").Value)
            End If
            itmX = SelListVw.Items.Add(CStr(grs.Fields("見積番号").Value) & " ID", buf, "")

            'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            itmX.SubItems.Add(CType(NullToZero(grs.Fields("見積日付").Value, ""), DateTime).ToString("yy/MM/dd")) '2007/02/16 ADD

            '        '2005/07/04 ADD
            '        If ISInt(rs("得意先別見積番号").Value) Then
            '            RSet buf = CStr(rs("得意先別見積番号").Value)
            '        Else
            '            buf = Space$(1) & CStr(rs(8).Value)
            '        End If
            '        itmX.ListSubItems.Add , , buf
            '        '---------------------------------

            itmX.SubItems.Add(ModKubuns.Get物件種別名(NullToZero((grs.Fields("物件種別").Value), 0))) '2012/09/06 ADD
            'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            itmX.SubItems.Add(CStr(NullToZero((grs.Fields("得意先CD").Value), "")))
            'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            itmX.SubItems.Add(CStr(NullToZero((grs.Fields("納入先CD").Value), ""))) '2015/06/12 ADD
            'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            itmX.SubItems.Add(CStr(NullToZero((grs.Fields("見積件名").Value), "")))
            '        itmX.ListSubItems.Add , , CStr(Format$(NullToZero(grs("合計金額").Value, ""), "#,##0" & KIN_FMT)) '2014/07/10 ADD
            'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            itmX.SubItems.Add(Strings.Right(Space(12) & CType(NullToZero((grs.Fields("合計金額").Value), ""), Integer).ToString("#,##0" & KIN_FMT), 12)) '金額並び替え用にSPCをつける

            'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            itmX.SubItems.Add(ModKubuns.Getウエルシアリース区分名(CType(NullToZero((grs.Fields("ウエルシアリース区分").Value), 0), Integer)))

            '        With cWelBukkenKubun
            '            .Initialize
            '            .ウエルシア物件区分CD = NullToZero(grs("ウエルシア物件区分CD").Value, 0)
            '
            '            If .GetbyID Then
            '                itmX.ListSubItems.Add , , .ウエルシア物件区分名
            '            Else
            '                itmX.ListSubItems.Add , , "　"
            '            End If
            '        End With
            'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            itmX.SubItems.Add(CStr(NullToZero((grs.Fields("ウエルシア物件区分名").Value), "")))

            '2022/10/10 ADD↓
            itmX.SubItems.Add(CType((grs.Fields("YKサプライ区分").Value & ""), Integer).ToString("#"))
            itmX.SubItems.Add(CType((grs.Fields("YK物件区分").Value & ""), Integer).ToString("#"))
            itmX.SubItems.Add(CType((grs.Fields("YK請求区分").Value & ""), Integer).ToString("#"))
            itmX.SubItems.Add(CType((grs.Fields("B請求管轄区分").Value & ""), Integer).ToString("#"))
            itmX.SubItems.Add(CType((grs.Fields("BtoB番号").Value & ""), Integer).ToString("#"))
            '2022/10/10 ADD↑

            'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            If NullToZero((grs.Fields("見積書出力日").Value), "").ToString = "" Then
                itmX.SubItems.Add("")
            Else
                itmX.SubItems.Add(CType((grs.Fields("見積書出力日").Value), DateTime).ToString("MM/dd"))
            End If
            'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            If NullToZero((grs.Fields("発注書発行日付").Value), "").ToString = "" Then
                itmX.SubItems.Add("")
            Else
                itmX.SubItems.Add(CType(NullToZero((grs.Fields("発注書発行日付").Value), ""), DateTime).ToString("MM/dd"))
            End If

            'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            If NullToZero((grs.Fields("納期S").Value), "").ToString = "" Then
                itmX.SubItems.Add("")
            Else
                itmX.SubItems.Add(CType(NullToZero((grs.Fields("納期S").Value), ""), DateTime).ToString("MM/dd")) '2016/10/26 ADD
            End If
            'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            If NullToZero((grs.Fields("仕入日付").Value), "").ToString = "" Then
                itmX.SubItems.Add("")
            Else
                itmX.SubItems.Add(CType(NullToZero((grs.Fields("仕入日付").Value), ""), DateTime).ToString("yy/MM/dd")) '2013/09/18 ADD
            End If
            'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            If NullToZero((grs.Fields("未仕入件数").Value), "").ToString = "" Then
                itmX.SubItems.Add("")
            Else
                itmX.SubItems.Add(CType(NullToZero((grs.Fields("未仕入件数").Value), ""), Integer).ToString("#,##0" & KIN_FMT)) '2015/01/09 ADD
            End If
            'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            If NullToZero((grs.Fields("売上日付").Value), "").ToString = "" Then
                itmX.SubItems.Add("")
            Else
                itmX.SubItems.Add(CType(NullToZero((grs.Fields("売上日付").Value), ""), DateTime).ToString("yy/MM/dd")) '2013/09/18 ADD
            End If
            'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            If NullToZero((grs.Fields("完了日付").Value), "").ToString = "" Then
                itmX.SubItems.Add("")
            Else
                itmX.SubItems.Add(CType(NullToZero((grs.Fields("完了日付").Value), ""), DateTime).ToString("yy/MM/dd")) '2020/04/16 ADD
            End If
            'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            If NullToZero((grs.Fields("請求予定日").Value), "").ToString = "" Then
                itmX.SubItems.Add("")
            Else
                itmX.SubItems.Add(CType(NullToZero((grs.Fields("請求予定日").Value), ""), DateTime).ToString("yy/MM/dd")) '2020/04/16 ADD
            End If
            'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            itmX.SubItems.Add(CStr(NullToZero((grs.Fields("経過備考1").Value), "")))
            'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            itmX.SubItems.Add(CStr(NullToZero((grs.Fields("経過備考2").Value), "")))
            'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            If NullToZero((grs.Fields("請求書発行日付").Value), "").ToString = "" Then
                itmX.SubItems.Add("")
            Else
                itmX.SubItems.Add(CType(NullToZero((grs.Fields("請求書発行日付").Value), ""), DateTime).ToString("yy/MM/dd"))
            End If
            'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            itmX.SubItems.Add(CType(NullToZero((grs.Fields("原価率").Value), ""), Double).ToString("##0.00\%")) '2013/01/18 ADD
            'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            If NullToZero((grs.Fields("登録変更日").Value), "").ToString = "" Then
                itmX.SubItems.Add("")
            Else
                itmX.SubItems.Add(CType(NullToZero((grs.Fields("登録変更日").Value), ""), DateTime).ToString("yy/MM/dd hh:mm")) '2012/10/26 ADD
            End If
            If NullToZero((grs.Fields("物件番号").Value), "").ToString = "" Then
                itmX.SubItems.Add("")
            Else
                itmX.SubItems.Add(CType(grs.Fields("物件番号").Value & "", Integer).ToString("#")) '2015/11/26 ADD
            End If
            'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            itmX.SubItems.Add(CStr(NullToZero((grs.Fields("見積確定区分").Value), ""))) '2021/04/11 ADD

            grs.MoveNext()
        Loop
        ReleaseRs(grs)
        'UPGRADE_NOTE: オブジェクト itmX をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
        'itmX = Nothing
        LockWindowUpdate(0)
        HourGlass(False)

        'データ件数セット
        If SelListVw.Items.Count <> 0 Then
            rf_ListCount.Text = SelListVw.Items.Count.ToString("#,##0")
        Else
            Exit Function
        End If
        SetupItems = True
    End Function

    Private Function 経過確認表出力(ByRef OutFile As String) As Integer
        Dim st As Integer
        'UPGRADE_NOTE: xl は xl_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
        Dim xl_Renamed As Object 'Excel.Application
        Dim TemplateFile As String
        Dim OriginalCalcMode As Integer 'XlCalculation
        Dim wkb As Object = Nothing 'Excel.Workbook
        Dim twks As Object 'Excel.Worksheet
        Dim wks As Object = Nothing 'Excel.Worksheet

        Dim PrintTitleRows, PrintTitleColumns As String
        Dim Core As Object 'Excel.Range
        Dim CoreRows, CoreColumns As Integer
        Dim CoreHeight As Integer
        '    Dim SumCore         As Object                               'Excel.Range
        '    Dim SumCoreRows     As Integer, SumCoreColumns As Integer
        '    Dim SumCoreHeight   As Integer
        '    Dim TukiRetuCore     As Object                              'Excel.Range
        '    Dim TukiRetuCoreRows As Long, TukiRetuCoreColumns As Long
        '    Dim TukiNameCore    As Object                               'Excel.Range
        '    Dim TukiNameCoreRows As Long, TukiNameCoreColumns As Long
        '
        '    Dim CoreRow As Object 'Excel.Range
        '    Dim CoreRowRows As Long, CoreRowColumns As Long
        '
        '    Dim CoreTokuRow As Object 'Excel.Range
        '
        Dim LCNT As Integer '現在行の保持
        Dim sLcnt As Integer
        '    Dim eLcnt As Long
        Dim i, r As Integer
        '    Dim j As Integer

        Dim HinCnt As Integer
        Dim RecCnt As Integer
        Dim H区分け As String 'ホールド
        H区分け = ""

        Dim RecCnt_kh As Integer
        Dim Lcnt_kh As Integer '経費のポジション
        Dim HinCnt_kh As Integer

        '処理メッセージ準備
        System.Windows.Forms.Application.DoEvents()
        'マウスポインターを砂時計にする
        Me.Enabled = False
        HourGlass(True)
        'メッセージを表示
        Fw_Msg.Text = Me.Text
        'VB6.ShowForm(Fw_Msg, VB6.FormShowConstants.Modeless, Me)
        Fw_Msg.Show(Me)
        Fw_Msg.Refresh()

        '戻り値をリセットします
        st = 0
        '*---処理メッセージ表示
        Fw_Msg.StsCaption = "作成中..."

        'テンプレートのファイル名を準備します
        'UPGRADE_WARNING: オブジェクト GetIni() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
        TemplateFile = GetIni("Template", ProfileKey, INIFile)
        If TemplateFile = "" Then
            TemplateFile = AppPath(DefaultTemplateFile)
        End If

        'Excel.Application クラスオブジェクトを開きます
        xl_Renamed = xlOpen()
        If xl_Renamed Is Nothing Then
            st = -1
            GoTo exit_proc
        End If
        'Templateのブックを開きます
        wkb = xlOpenBook(xl_Renamed, TemplateFile)
        If wkb Is Nothing Then
            st = -2
            GoTo exit_proc
        End If
        '自動再計算を禁止します
        'UPGRADE_WARNING: オブジェクト xl_Renamed.Calculation の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
        OriginalCalcMode = xl_Renamed.Calculation
        'UPGRADE_WARNING: オブジェクト xl_Renamed.Calculation の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
        xl_Renamed.Calculation = -4135 'xlCalculationManual

        'テンプレートのシートを変数にセットします
        'UPGRADE_WARNING: オブジェクト wkb.Worksheets の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
        twks = wkb.Worksheets(1)

        '中止ボタンの確認
        System.Windows.Forms.Application.DoEvents()
        If Fw_Msg.AbortDoing = True Then GoTo Abort

        '2014/07/10 ADD
        '中国用に金額を小数点2桁にする。
        If COUNTRY_CODE = "CN" Then
            With twks
                'UPGRADE_WARNING: オブジェクト twks.Range の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                .Range("F3:H7").NumberFormatLocal = "_ * #,##0" & KIN_FMT & "_ ;_ * -#,##0" & KIN_FMT & "_ ;_ * "" - ""_ ;_ @_ "
            End With
        End If

        'xl.Visible = True
        'xl.ScreenUpdating = True

        On Error GoTo on_err 'エラートラップを開始します

        '作成日をセットします
        'UPGRADE_WARNING: オブジェクト xl_Renamed.Range の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
        xl_Renamed.Range("作成日") = "作成日： " & CType(Today, DateTime).ToString("yyyy/MM/dd")


        '表の雛形セル範囲を変数にセットします
        'UPGRADE_WARNING: オブジェクト xl_Renamed.Range の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
        Core = xl_Renamed.Range("CoreCells")
        'UPGRADE_WARNING: オブジェクト Core.ROWS の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
        CoreRows = Core.ROWS.Count
        'UPGRADE_WARNING: オブジェクト Core.Columns の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
        CoreColumns = Core.Columns.Count

        '印刷タイトル範囲を設定します
        With twks
            '        PrintTitleRows = .Range(.Cells(1, 1), .Cells(2, 1)).EntireRow.Address
            '        PrintTitleColumns = .Range("$A$1:" & Core.Cells(1, 4).Address).EntireColumn.Address
            '        PrintTitleRows = .Range(.Range("Print_Titles").ADDRESS).EntireRow.ADDRESS
        End With

        '中止ボタンの確認
        System.Windows.Forms.Application.DoEvents()
        If Fw_Msg.AbortDoing = True Then GoTo Abort

        'プログレスバーの値セット
        Fw_Msg.ProgValue = 0
        '    Fw_Msg.ProgMax = gRecCnt

        r = 0

        '*---処理メッセージ表示
        Fw_Msg.StsCaption = "集計情報を抽出しています。"


        '集計値をセルにセットします
        Do Until grs.EOF
            If "" = H区分け Then '1回のみ行なう
                'シートを追加します
                'UPGRADE_WARNING: オブジェクト wkb.Worksheets の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                'UPGRADE_WARNING: オブジェクト twks.Copy の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                twks.Copy(after:=wkb.Worksheets(wkb.Worksheets.Count))
                'UPGRADE_WARNING: オブジェクト wkb.Worksheets の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                wks = wkb.Worksheets(wkb.Worksheets.Count)
                With wks
                    '追加したシートの設定
                    '                 cTeam.Initialize
                    '                 cTeam.統計集計先CD = NullToZero(grs![統計集計先CD], 0)
                    '                 cTeam.GetbyID
                    '                .Name = NullToZero(cTeam.統計集計先名, " その他")
                    'UPGRADE_WARNING: オブジェクト wks.Name の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                    .Name = "経過確認表"
                End With

                '担当者ごとの行数
                '            grs.MaxRecords
                '            grs.MoveFirst
                RecCnt = grs.RecordCount
                '            RecCnt = gRecCnt

                LCNT = 3
                With wks
                    If RecCnt > 2 Then
                        '不足している行を挿入します
                        'UPGRADE_WARNING: オブジェクト wks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                        'UPGRADE_WARNING: オブジェクト wks.Range の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                        .Range(.Cells(LCNT + 1, 1), .Cells(LCNT + RecCnt - 2, CoreColumns)).EntireRow.Insert()
                        'セル内容を行コピーします
                        'UPGRADE_WARNING: オブジェクト wks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                        'UPGRADE_WARNING: オブジェクト wks.Range の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                        .Range(.Cells(LCNT, 1), .Cells(LCNT + RecCnt - 1, CoreColumns)).filldown()
                    ElseIf RecCnt = 2 Then
                        'セル内容を行コピーします
                        'UPGRADE_WARNING: オブジェクト wks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                        'UPGRADE_WARNING: オブジェクト wks.Range の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                        .Range(.Cells(LCNT, 1), .Cells(LCNT + RecCnt - 1, CoreColumns - 2 + RecCnt)).filldown()
                    ElseIf RecCnt < 2 Then
                        'ダミー行が残ってしまうので削除しておきます
                        'UPGRADE_WARNING: オブジェクト wks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                        'UPGRADE_WARNING: オブジェクト wks.Range の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                        .Range(.Cells(LCNT + 1, 1), .Cells(LCNT + 1, CoreColumns - 2 + RecCnt)).EntireRow.Delete()
                    End If
                End With
                ''
                sLcnt = LCNT
                ' ''            r = 0
                HinCnt = 0
                ''
                ''
                '抽出条件をコメントで
                'UPGRADE_WARNING: オブジェクト wks.Range の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                With wks.Range("A1")
                    'UPGRADE_WARNING: オブジェクト wks.Range の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                    .AddComment()
                    'UPGRADE_WARNING: オブジェクト wks.Range の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                    .Comment.Visible = False
                    'UPGRADE_WARNING: オブジェクト wks.Range の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                    .Comment.Text(Text:="処理種別:" & [tx_売上種別].Text)
                    'UPGRADE_WARNING: オブジェクト wks.Range の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                    .Comment.Text(Text:= .Comment.Text & vbCrLf & "得意先:" & [tx_得意先CD].Text)
                    'UPGRADE_WARNING: オブジェクト wks.Range の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                    .Comment.Text(Text:= .Comment.Text & vbCrLf & "納入先:" & [tx_納入先CD].Text)

                    'UPGRADE_WARNING: オブジェクト wks.Range の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                    .Comment.Text(Text:= .Comment.Text & vbCrLf & "担当者:" & [tx_担当者CD].Text)
                    'UPGRADE_WARNING: オブジェクト wks.Range の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                    .Comment.Text(Text:= .Comment.Text & vbCrLf & "物件種別:" & [tx_物件種別].Text)
                    'UPGRADE_WARNING: オブジェクト wks.Range の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                    .Comment.Text(Text:= .Comment.Text & vbCrLf & "見積番号:" & [tx_s見積番号].Text & "～" & [tx_e見積番号].Text)
                    'UPGRADE_WARNING: オブジェクト wks.Range の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                    .Comment.Text(Text:= .Comment.Text & vbCrLf & "見積件名:" & [tx_見積件名].Text)
                    'UPGRADE_WARNING: オブジェクト wks.Range の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                    .Comment.Text(Text:= .Comment.Text & vbCrLf & "見積金額:" & [tx_s見積金額].Text & "～" & [tx_e見積金額].Text)

                    'UPGRADE_WARNING: オブジェクト wks.Range の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                    .Comment.Text(Text:= .Comment.Text & vbCrLf & "Wリース区分:" & [tx_ウエルシアリース区分].Text)
                    'UPGRADE_WARNING: オブジェクト wks.Range の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                    .Comment.Text(Text:= .Comment.Text & vbCrLf & "W物件区分:" & [tx_ウエルシア物件区分].Text)

                    'UPGRADE_WARNING: オブジェクト wks.Range の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                    .Comment.Text(Text:= .Comment.Text & vbCrLf & "見積日付:" & idc(0).Text & "～" & idc(1).Text)
                    'UPGRADE_WARNING: オブジェクト wks.Range の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                    .Comment.Text(Text:= .Comment.Text & vbCrLf & "開始納期:" & idc(2).Text & "～" & idc(3).Text)
                    'UPGRADE_WARNING: オブジェクト wks.Range の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                    .Comment.Text(Text:= .Comment.Text & vbCrLf & "完了日付:" & idc(4).Text & "～" & idc(5).Text)
                    'UPGRADE_WARNING: オブジェクト wks.Range の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                    .Comment.Text(Text:= .Comment.Text & vbCrLf & "請求予定:" & idc(6).Text & "～" & idc(7).Text)
                End With
                H区分け = CStr(1)
            End If

            '中止ボタンの確認
            System.Windows.Forms.Application.DoEvents()
            If Fw_Msg.AbortDoing = True Then GoTo Abort
            'プログレスバーの値セット
            Fw_Msg.ProgValue = r

            With wks
                '----ブレイクキーHOLD
                '            HチームCD = grs![統計集計先CD] & ""
                '
                'UPGRADE_WARNING: オブジェクト wks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                .Cells(HinCnt + sLcnt, 1) = grs.Fields("見積番号").Value
                'UPGRADE_WARNING: オブジェクト wks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                .Cells(HinCnt + sLcnt, 2) = ModKubuns.Get物件種別名((grs.Fields("物件種別").Value).ToString)
                'UPGRADE_WARNING: オブジェクト wks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                .Cells(HinCnt + sLcnt, 3) = grs.Fields("得意先CD").Value
                'UPGRADE_WARNING: オブジェクト wks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                .Cells(HinCnt + sLcnt, 4) = grs.Fields("納入先CD").Value
                'UPGRADE_WARNING: オブジェクト wks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                .Cells(HinCnt + sLcnt, 5) = grs.Fields("見積件名").Value

                'UPGRADE_WARNING: オブジェクト wks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                .Cells(HinCnt + sLcnt, 6) = CType(NullToZero((grs.Fields("合計金額").Value), ""), Integer).ToString("#,##0") & KIN_FMT

                'UPGRADE_WARNING: オブジェクト wks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                .Cells(HinCnt + sLcnt, 7) = ModKubuns.Getウエルシアリース区分名(NullToZero((grs.Fields("ウエルシアリース区分").Value), 0))
                'UPGRADE_WARNING: オブジェクト wks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                .Cells(HinCnt + sLcnt, 8) = SpcToNull((grs.Fields("ウエルシア物件区分名").Value), "")
                '            .Cells(HinCnt + sLcnt, 9).Formula = Format$(NullToZero(grs("完了日付").Value, ""), "yy/m/d")
                'UPGRADE_WARNING: オブジェクト wks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                .Cells(HinCnt + sLcnt, 9).Formula = NullToZero((grs.Fields("完了日付").Value), "")
                '            .Cells(HinCnt + sLcnt, 10).Formula = Format$(NullToZero(grs("請求書発行日付").Value, ""), "yy/m/d")
                'UPGRADE_WARNING: オブジェクト wks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                .Cells(HinCnt + sLcnt, 10).Formula = NullToZero((grs.Fields("請求書発行日付").Value), "")

                '            .Cells(HinCnt + sLcnt, 11).Formula = Format$(NullToZero(grs("請求予定月").Value, ""), "yy/m")
                'UPGRADE_WARNING: オブジェクト wks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                .Cells(HinCnt + sLcnt, 11).Formula = NullToZero((grs.Fields("請求予定日").Value), "")
                'UPGRADE_WARNING: オブジェクト wks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                .Cells(HinCnt + sLcnt, 12) = NullToZero((grs.Fields("経過備考1").Value), "")
                'UPGRADE_WARNING: オブジェクト wks.Cells の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                .Cells(HinCnt + sLcnt, 13) = NullToZero((grs.Fields("経過備考2").Value), "")

                '------
                ''            .Cells(HinCnt + sLcnt, 14).Formula = NullToZero(grs("見積日付").Value, "")
                ''            .Cells(HinCnt + sLcnt, 15).Formula = NullToZero(grs("売上日付").Value, "")

                '------
                HinCnt += 1
                r += 1
                '            H納入得意先CD = grs![納入得意先CD] & ""
            End With

            grs.MoveNext()
        Loop

        'テンプレートシートへの名前参照のごみが残らないように予め定義名を削除しておきます
        'UPGRADE_WARNING: オブジェクト wkb.names の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
        For i = wkb.names.Count To 1 Step -1
            '        wkb.Names(i).Delete
            'UPGRADE_WARNING: オブジェクト wkb.names の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            If wkb.names(i).Name Like "*Print_Titles*" = False Then
                'UPGRADE_WARNING: オブジェクト wkb.names の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                wkb.names(i).Delete()
            End If
        Next

        '*---処理メッセージ表示
        Fw_Msg.StsCaption = "保存しています。"
        Fw_Msg.ProgValue = RecCnt
        '中止ボタンの確認
        System.Windows.Forms.Application.DoEvents()
        If Fw_Msg.AbortDoing = True Then GoTo Abort

        'テンプレートのシートを削除します
        'UPGRADE_NOTE: オブジェクト Core をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
        Core = Nothing
        '    Set SumCore = Nothing

        'UPGRADE_WARNING: オブジェクト twks.Delete の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
        twks.Delete()
        'UPGRADE_NOTE: オブジェクト twks をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
        twks = Nothing
        '再計算
        'UPGRADE_WARNING: オブジェクト xl_Renamed.Calculate の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
        xl_Renamed.Calculate()
        'UPGRADE_WARNING: オブジェクト xl_Renamed.Calculation の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
        xl_Renamed.Calculation = OriginalCalcMode '再計算モードを復旧します

        'UPGRADE_WARNING: オブジェクト wkb.Worksheets の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
        For i = 1 To wkb.Worksheets.Count
            '[改ページプレビュー][100%]
            'UPGRADE_WARNING: オブジェクト wkb.Worksheets の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            wkb.Worksheets(i).Activate()
            'UPGRADE_WARNING: オブジェクト xl_Renamed.ActiveWindow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            xl_Renamed.ActiveWindow.View = 2 'xlPageBreakPreview
            'UPGRADE_WARNING: オブジェクト xl_Renamed.ActiveWindow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            xl_Renamed.ActiveWindow.Zoom = 100
        Next

        '最初のシートを選択しておきます
        'UPGRADE_WARNING: オブジェクト wkb.Worksheets の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
        wkb.Worksheets(1).Activate()

        'ブックにパスワードをセットします。
        'UPGRADE_WARNING: オブジェクト wkb.password の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
        wkb.password = password_Exl '2012/06/20 ADD

        '名前を付けて保存します
        'UPGRADE_WARNING: オブジェクト wkb.SaveAs の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
        'wkb.SaveAs(OutFile, -4143) 'xlWorkbookNormal
        wkb.SaveAs(OutFile, 51) 'xlWorkbookDefault

        GoTo exit_proc
Abort:
        '*---処理メッセージ表示
        Fw_Msg.StsCaption = "中止しています．．．"
        st = -1
exit_proc:
        On Error GoTo 0 'エラートラップを解除します
        '    Cn.CursorLocation = adUseServer
        ReleaseRs(grs)

        'UPGRADE_NOTE: オブジェクト Core をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
        Core = Nothing
        '    Set SumCore = Nothing

        'UPGRADE_NOTE: オブジェクト twks をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
        twks = Nothing
        'UPGRADE_NOTE: オブジェクト wks をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
        wks = Nothing
        If xl_Renamed IsNot Nothing Then
            If OriginalCalcMode <> 0 Then
                'UPGRADE_WARNING: オブジェクト xl_Renamed.Calculation の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                xl_Renamed.Calculation = OriginalCalcMode '再計算モードを復旧します
            End If
            If wkb IsNot Nothing Then
                xlCloseBook(wkb) 'ワークブックを閉じます
                xlClose(xl_Renamed) 'Excel.Applicationオブジェクトを開放します
            End If
        End If
        経過確認表出力 = st

        '処理メッセージの終了
        Me.Enabled = True
        Me.Activate()
        'メッセージを閉じる
        Fw_Msg.Hide()
        Fw_Msg.Refresh()
        'UPGRADE_NOTE: オブジェクト Fw_Msg をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
        Fw_Msg = Nothing
        HourGlass(False)
        Exit Function
on_err:
        st = Err.Number
        CriticalAlarm(GetErrorDetails(Cn))
        Resume exit_proc
    End Function

    Private Sub tx_売上種別_Enter(sender As Object, e As EventArgs) Handles tx_売上種別.Enter
        If Item_Check(([tx_売上種別].TabIndex)) = False Then
            Exit Sub
        End If
        PreviousControl = Me.ActiveControl
    End Sub

    '2010/05/06 ADD↓
    Private Sub tx_売上種別_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_売上種別.KeyPress
        Dim KeyAscii As Integer = Asc(e.KeyChar)
        'MaxLengthは１多く設定する。

        Const Numbers As String = "0123" ' 入力許可文字
        'Dim strText As String
        If KeyAscii = System.Windows.Forms.Keys.Return Then KeyAscii = 0

        '数字検査をする
        If KeyAscii <> 8 Then ' バックスペースは例外
            If InStr(Numbers, Chr(KeyAscii)) = 0 Then
                KeyAscii = 0 ' 入力を無効にする
                Exit Sub
            End If
        End If
        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If
    End Sub
    '2010/05/06 ADD↑

    Private Sub tx_売上種別_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_売上種別.KeyDown
        ' ↑キーを検出
        If e.KeyCode = Keys.Up Then
            KeyUpF = True
            e.Handled = True
        Else
            ReturnF = True
        End If
    End Sub

    Private Sub tx_売上種別_Leave(sender As Object, e As EventArgs) Handles tx_売上種別.Leave
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_売上種別.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_売上種別].Undo()
        End If
        ReturnF = False
    End Sub

    Private Sub Cb変更_Enter(sender As Object, e As EventArgs) Handles Cb変更.Enter
        If KeyUpF Then
            tx_売上種別.Focus()
            tx_売上種別.Select()
        End If
    End Sub

    '2011/11/29 ADD↓
    'Private Sub tx_見積区分_GotFocus()
    '    If Item_Check([tx_見積区分].TabIndex) = False Then
    '        Exit Sub
    '    End If
    '
    '    Set PreviousControl = Me.ActiveControl
    'End Sub
    '
    'Private Sub tx_見積区分_KeyPress(KeyAscii As Integer)
    'MaxLengthは１多く設定する。
    '
    '    Const Numbers As String = "012" ' 入力許可文字
    '    Dim strText As String
    '
    '    If KeyAscii = vbKeyReturn Then KeyAscii = 0
    '
    '    '数字検査をする
    '    If KeyAscii <> 8 Then ' バックスペースは例外
    '        If InStr(Numbers, Chr(KeyAscii)) = 0 Then
    '            KeyAscii = 0 ' 入力を無効にする
    '            Exit Sub
    '        End If
    '    End If
    'End Sub
    '
    'Private Sub tx_見積区分_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_見積区分.KeyDown
    '    ReturnF = True
    'End Sub
    '
    'Private Sub tx_見積区分_LostFocus()
    '    If ReturnF = False Then
    '        [tx_見積区分].Undo
    '    End If
    '    ReturnF = False
    'End Sub
    '2011/11/29 ADD↑

    Private Sub tx_得意先CD_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_得意先CD.Enter
        If Item_Check(([tx_得意先CD].TabIndex)) = False Then
            Exit Sub
        End If
        PreviousControl = Me.ActiveControl
    End Sub

    Private Sub tx_得意先CD_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_得意先CD.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii = Asc(" ") And ([tx_得意先CD].SelStart = 0 And [tx_得意先CD].SelLength = Len([tx_得意先CD].Text)) Then
            KeyAscii = 0
            '---参照画面表示
            '        SelectF = True
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
        ReturnF = True
    End Sub

    Private Sub tx_得意先CD_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_得意先CD.Leave
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_得意先CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_得意先CD].Undo()
        End If
        ReturnF = False
    End Sub

    Private Sub tx_納入先CD_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_納入先CD.Enter
        If Item_Check(([tx_納入先CD].TabIndex)) = False Then
            Exit Sub
        End If
        PreviousControl = Me.ActiveControl
    End Sub

    Private Sub tx_納入先CD_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_納入先CD.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii = Asc(" ") And ([tx_納入先CD].SelStart = 0 And [tx_納入先CD].SelLength = Len([tx_納入先CD].Text)) Then
            KeyAscii = 0
            '---参照画面表示
            cNonyusaki.得意先CD = [tx_得意先CD].Text
            If cNonyusaki.ShowDialog = True Then
                [tx_納入先CD].Text = cNonyusaki.納入先CD
                ReturnF = True
                [tx_納入先CD].NextSetFocus()
            Else
                [tx_納入先CD].Focus()
            End If
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub tx_納入先CD_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_納入先CD.KeyDown
        ReturnF = True
    End Sub

    Private Sub tx_納入先CD_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_納入先CD.Leave
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_納入先CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_納入先CD].Undo()
        End If
        ReturnF = False
    End Sub

    '2009/12/02 ADD↓
    Private Sub tx_担当者CD_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_担当者CD.Enter
        If Item_Check(([tx_担当者CD].TabIndex)) = False Then
            Exit Sub
        End If
        PreviousControl = Me.ActiveControl
    End Sub

    Private Sub tx_担当者CD_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_担当者CD.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii = Asc(" ") And ([tx_担当者CD].SelStart = 0 And [tx_担当者CD].SelLength = Len([tx_担当者CD].Text)) Then
            'KeyAscii = 0
            '---参照画面表示
            If cTanto.ShowDialog = True Then
                [tx_担当者CD].Text = cTanto.担当者CD
                ReturnF = True
                [tx_担当者CD].NextSetFocus()
            Else
                [tx_担当者CD].Focus()
            End If
        End If
    End Sub

    Private Sub tx_担当者CD_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_担当者CD.KeyDown
        ReturnF = True
    End Sub

    Private Sub tx_担当者CD_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_担当者CD.Leave
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_担当者CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_担当者CD].Undo()
        End If
        ReturnF = False
    End Sub
    '2009/12/02 ADD↑

    '2023/04/18 ADD↓
    Private Sub tx_業種区分_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_業種区分.Enter
        If Item_Check(([tx_業種区分].TabIndex)) = False Then
            Exit Sub
        End If
        PreviousControl = Me.ActiveControl
    End Sub

    Private Sub tx_業種区分_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_業種区分.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        'MaxLengthは１多く設定する。

        Const Numbers As String = "01" ' 入力許可文字
        'Dim strText As String
        If KeyAscii = System.Windows.Forms.Keys.Return Then KeyAscii = 0

        '数字検査をする
        If KeyAscii <> 8 Then ' バックスペースは例外
            If InStr(Numbers, Chr(KeyAscii)) = 0 Then
                KeyAscii = 0 ' 入力を無効にする
                GoTo EventExitSub
            Else
                'UPGRADE_WARNING: TextBox プロパティ tx_業種区分.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
                'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
                'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
                If LenB([tx_業種区分].Text) = [tx_業種区分].MaxLength - 1 Then
                    Call SelText([tx_業種区分])
                End If
                'UPGRADE_WARNING: TextBox プロパティ tx_業種区分.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
                'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
                'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
                If LenB([tx_業種区分].Text & Chr(KeyAscii)) - LenB([tx_業種区分].SelText) = [tx_業種区分].MaxLength - 1 Then
                    ReturnF = True
                    [tx_業種区分].NextSetFocus()
                End If
            End If
        End If
EventExitSub:
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub tx_業種区分_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_業種区分.KeyDown
        ReturnF = True
    End Sub

    Private Sub tx_業種区分_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_業種区分.Leave
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_業種区分.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_業種区分].Undo()
        End If
        ReturnF = False
    End Sub
    '2023/04/18 ADD↑

    '2015/05/28 ADD↓
    Private Sub tx_物件種別_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_物件種別.Enter
        If Item_Check(([tx_物件種別].TabIndex)) = False Then
            Exit Sub
        End If
        PreviousControl = Me.ActiveControl
    End Sub

    Private Sub tx_物件種別_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_物件種別.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        'MaxLengthは１多く設定する。

        '    Const Numbers As String = "01234" ' 入力許可文字
        '    Const Numbers As String = "012345" ' 入力許可文字       '2012/08/21 ADD
        '    Const Numbers As String = "01245" ' 入力許可文字       '2012/08/21 ADD
        '    Const Numbers As String = "012456" ' 入力許可文字       '2022/08/31 ADD
        '    Const Numbers As String = "01256" ' 入力許可文字       '2023/04/18 ADD
        Const Numbers As String = "0126" ' 入力許可文字       '2024/01/31 ADD
        'Dim strText As String
        If KeyAscii = System.Windows.Forms.Keys.Return Then KeyAscii = 0

        '数字検査をする
        If KeyAscii <> 8 Then ' バックスペースは例外
            If InStr(Numbers, Chr(KeyAscii)) = 0 Then
                KeyAscii = 0 ' 入力を無効にする
                Exit Sub
            Else
                'UPGRADE_WARNING: TextBox プロパティ tx_物件種別.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
                'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
                'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
                If LenB([tx_物件種別].Text) = [tx_物件種別].MaxLength - 1 Then
                    Call SelText([tx_物件種別])
                End If
                'UPGRADE_WARNING: TextBox プロパティ tx_物件種別.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
                'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
                'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
                If LenB([tx_物件種別].Text & Chr(KeyAscii)) - LenB([tx_物件種別].SelText) = [tx_物件種別].MaxLength - 1 Then
                    ReturnF = True
                    [tx_物件種別].NextSetFocus()
                End If
            End If
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub tx_物件種別_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_物件種別.KeyDown
        ReturnF = True
    End Sub

    Private Sub tx_物件種別_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_物件種別.Leave
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_物件種別.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_物件種別].Undo()
        End If
        ReturnF = False
    End Sub
    '2015/05/28 ADD↑

    Private Sub tx_s見積番号_Enter(sender As Object, e As EventArgs) Handles tx_s見積番号.Enter
        If Item_Check(([tx_s見積番号].TabIndex)) = False Then
            Exit Sub
        End If
        [tx_s見積番号].Text = Trim([tx_s見積番号].Text)
        PreviousControl = Me.ActiveControl
    End Sub

    Private Sub tx_s見積番号_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_s見積番号.KeyPress
        Dim KeyAscii As Integer = Asc(e.KeyChar)
        Const Numbers As String = "0123456789" ' 入力許可文字

        If KeyAscii = System.Windows.Forms.Keys.Return Then KeyAscii = 0

        '数字検査をする
        If KeyAscii <> 8 Then ' バックスペースは例外
            If InStr(Numbers, Chr(KeyAscii)) = 0 Then
                KeyAscii = 0 ' 入力を無効にする
                Exit Sub
            End If
        End If
        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub tx_s見積番号_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_s見積番号.KeyDown
        ReturnF = True
    End Sub

    Private Sub tx_s見積番号_Leave(sender As Object, e As EventArgs) Handles tx_s見積番号.Leave
        Dim buf As String
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_s見積番号.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_s見積番号].Undo()
        End If
        ReturnF = False

        If [tx_s見積番号].MaxLength = 0 Then [tx_s見積番号].MaxLength = 7

        'UPGRADE_WARNING: TextBox プロパティ tx_s見積番号.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
        buf = New String(" "c, [tx_s見積番号].MaxLength)
        buf = RSet(Trim([tx_s見積番号].Text), Len(buf))
        [tx_s見積番号].Text = buf
    End Sub

    Private Sub tx_e見積番号_Enter(sender As Object, e As EventArgs) Handles tx_e見積番号.Enter
        If Item_Check(([tx_e見積番号].TabIndex)) = False Then
            Exit Sub
        End If
        [tx_e見積番号].Text = Trim([tx_e見積番号].Text)
        PreviousControl = Me.ActiveControl
    End Sub

    Private Sub tx_e見積番号_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_e見積番号.KeyPress
        Dim KeyAscii As Integer = Asc(e.KeyChar)
        Const Numbers As String = "0123456789" ' 入力許可文字

        If KeyAscii = System.Windows.Forms.Keys.Return Then KeyAscii = 0

        '数字検査をする
        If KeyAscii <> 8 Then ' バックスペースは例外
            If InStr(Numbers, Chr(KeyAscii)) = 0 Then
                KeyAscii = 0 ' 入力を無効にする
                Exit Sub
            End If
        End If
        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub tx_e見積番号_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_e見積番号.KeyDown
        ReturnF = True
    End Sub

    Private Sub tx_e見積番号_Leave(sender As Object, e As EventArgs) Handles tx_e見積番号.Leave
        Dim buf As String
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_e見積番号.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_e見積番号].Undo()
        End If
        ReturnF = False

        If [tx_e見積番号].MaxLength = 0 Then [tx_e見積番号].MaxLength = 7

        'UPGRADE_WARNING: TextBox プロパティ tx_e見積番号.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
        buf = New String(" "c, [tx_e見積番号].MaxLength)
        buf = RSet(Trim([tx_e見積番号].Text), Len(buf))
        [tx_e見積番号].Text = buf
    End Sub

    Private Sub tx_見積件名_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_見積件名.Enter
        If Item_Check(([tx_見積件名].TabIndex)) = False Then
            Exit Sub
        End If
        PreviousControl = Me.ActiveControl
    End Sub

    Private Sub tx_見積件名_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_見積件名.KeyDown
        ReturnF = True
    End Sub

    Private Sub tx_見積件名_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_見積件名.Leave
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_見積件名.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_見積件名].Undo()
        End If
        ReturnF = False
    End Sub

    Private Sub tx_s見積金額_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_s見積金額.Enter
        If Item_Check(([tx_s見積金額].TabIndex)) = False Then
            Exit Sub
        End If
        PreviousControl = Me.ActiveControl
    End Sub

    Private Sub tx_s見積金額_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_s見積金額.KeyDown
        ReturnF = True
    End Sub

    Private Sub tx_s見積金額_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_s見積金額.Leave
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_s見積金額.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_s見積金額].Undo()
        End If
        ReturnF = False
    End Sub

    Private Sub tx_s物件番号_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_s物件番号.Enter
        If Item_Check(([tx_s物件番号].TabIndex)) = False Then
            Exit Sub
        End If

        If [tx_s物件番号].MaxLength = 0 Then [tx_s物件番号].MaxLength = 7

        [tx_s物件番号].Text = Trim([tx_s物件番号].Text)
        PreviousControl = Me.ActiveControl
    End Sub

    Private Sub tx_s物件番号_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_s物件番号.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        Const Numbers As String = "0123456789" ' 入力許可文字

        If KeyAscii = System.Windows.Forms.Keys.Return Then KeyAscii = 0

        If KeyAscii = Asc(" ") And ([tx_s物件番号].SelStart = 0 And [tx_s物件番号].SelLength = Len([tx_s物件番号].Text)) Then
            KeyAscii = 0
            '---参照画面表示
            '        SelectF = True
            If cBukken.ShowDialog = True Then
                [tx_s物件番号].Text = CStr(cBukken.物件番号)
                ReturnF = True
                [tx_s物件番号].NextSetFocus()
            Else
                [tx_s物件番号].Focus()
            End If
        End If

        '数字検査をする
        If KeyAscii <> 8 Then ' バックスペースは例外
            If InStr(Numbers, Chr(KeyAscii)) = 0 Then
                KeyAscii = 0 ' 入力を無効にする
                Exit Sub
            End If
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub tx_s物件番号_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_s物件番号.KeyDown
        ReturnF = True
    End Sub

    Private Sub tx_s物件番号_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_s物件番号.Leave
        Dim buf As String
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_s物件番号.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_s物件番号].Undo()
        End If
        ReturnF = False

        'UPGRADE_WARNING: TextBox プロパティ tx_s物件番号.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
        buf = New String(" "c, [tx_s物件番号].MaxLength)
        buf = RSet(Trim([tx_s物件番号].Text), Len(buf))
        [tx_s物件番号].Text = buf
    End Sub

    Private Sub tx_e物件番号_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_e物件番号.Enter
        If Item_Check(([tx_e物件番号].TabIndex)) = False Then
            Exit Sub
        End If

        If [tx_e物件番号].MaxLength = 0 Then [tx_e物件番号].MaxLength = 7

        [tx_e物件番号].Text = Trim([tx_e物件番号].Text)
        PreviousControl = Me.ActiveControl
    End Sub

    Private Sub tx_e物件番号_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_e物件番号.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        Const Numbers As String = "0123456789" ' 入力許可文字

        If KeyAscii = System.Windows.Forms.Keys.Return Then KeyAscii = 0

        If KeyAscii = Asc(" ") And ([tx_e物件番号].SelStart = 0 And [tx_e物件番号].SelLength = Len([tx_e物件番号].Text)) Then
            KeyAscii = 0
            '---参照画面表示
            '        SelectF = True
            If cBukken.ShowDialog = True Then
                [tx_e物件番号].Text = CStr(cBukken.物件番号)
                ReturnF = True
                [tx_e物件番号].NextSetFocus()
            Else
                [tx_e物件番号].Focus()
            End If
        End If

        '数字検査をする
        If KeyAscii <> 8 Then ' バックスペースは例外
            If InStr(Numbers, Chr(KeyAscii)) = 0 Then
                KeyAscii = 0 ' 入力を無効にする
                Exit Sub
            End If
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub tx_e物件番号_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_e物件番号.KeyDown
        ReturnF = True
    End Sub

    Private Sub tx_e物件番号_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_e物件番号.Leave
        Dim buf As String
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_e物件番号.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_e物件番号].Undo()
        End If
        ReturnF = False

        'UPGRADE_WARNING: TextBox プロパティ tx_e物件番号.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
        buf = New String(" "c, [tx_e物件番号].MaxLength)
        buf = RSet(Trim([tx_e物件番号].Text), Len(buf))
        [tx_e物件番号].Text = buf
    End Sub

    Private Sub tx_ウエルシアリース区分_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_ウエルシアリース区分.Enter
        If Item_Check(([tx_ウエルシアリース区分].TabIndex)) = False Then
            Exit Sub
        End If
        PreviousControl = Me.ActiveControl

        '    'ボタン名設定
        '    Call SetUpFuncs(Me.ActiveControl.Name)
        '    sb_Msg.Panels(0).Text = "ウエルシアリース区分を入力して下さい。"
    End Sub

    Private Sub tx_ウエルシアリース区分_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_ウエルシアリース区分.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        'MaxLengthは１多く設定する。

        Const Numbers As String = "012" ' 入力許可文字
        'Dim strText As String
        If KeyAscii = System.Windows.Forms.Keys.Return Then KeyAscii = 0

        '数字検査をする
        If KeyAscii <> 8 Then ' バックスペースは例外
            If InStr(Numbers, Chr(KeyAscii)) = 0 Then
                KeyAscii = 0 ' 入力を無効にする
                Exit Sub
            Else
                'UPGRADE_WARNING: TextBox プロパティ tx_ウエルシアリース区分.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
                'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
                'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
                If LenB([tx_ウエルシアリース区分].Text) = [tx_ウエルシアリース区分].MaxLength - 1 Then
                    Call SelText([tx_ウエルシアリース区分])
                End If
                'UPGRADE_WARNING: TextBox プロパティ tx_ウエルシアリース区分.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
                'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
                'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
                If LenB([tx_ウエルシアリース区分].Text & Chr(KeyAscii)) - LenB([tx_ウエルシアリース区分].SelText) = [tx_ウエルシアリース区分].MaxLength - 1 Then
                    ReturnF = True
                    [tx_ウエルシアリース区分].NextSetFocus()
                End If
            End If
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub tx_ウエルシアリース区分_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_ウエルシアリース区分.KeyDown
        ReturnF = True
    End Sub

    Private Sub tx_ウエルシアリース区分_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_ウエルシアリース区分.Leave
        '    sb_Msg.Panels(0).Text = ""
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_ウエルシアリース区分.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_ウエルシアリース区分].Undo()
        End If
        ReturnF = False
    End Sub

    Private Sub tx_ウエルシア物件区分_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_ウエルシア物件区分.Enter
        '    If SelectF = False Then
        If Item_Check(([tx_ウエルシア物件区分].TabIndex)) = False Then
            Exit Sub
        End If
        '    End If
        '    SelectF = False
        PreviousControl = Me.ActiveControl
        'ボタン名設定
        '    Call SetUpFuncs(Me.ActiveControl.Name)
        '    [sb_Msg].Items.Item(0).Text = "ウエルシア物件区分を入力して下さい。　選択画面：Space"
    End Sub

    Private Sub tx_ウエルシア物件区分_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_ウエルシア物件区分.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii = Asc(" ") And ([tx_ウエルシア物件区分].SelStart = 0 And [tx_ウエルシア物件区分].SelLength = Len([tx_ウエルシア物件区分].Text)) Then
            'KeyAscii = 0
            '---参照画面表示
            '        SelectF = True

            If cWelBukkenKubun.ShowDialog = True Then
                [tx_ウエルシア物件区分].Text = cWelBukkenKubun.ウエルシア物件区分CD
                ReturnF = True
                [tx_ウエルシア物件区分].NextSetFocus()
            Else
                [tx_ウエルシア物件区分].Focus()
            End If
        End If
    End Sub

    Private Sub tx_ウエルシア物件区分_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_ウエルシア物件区分.KeyDown
        ReturnF = True
    End Sub

    Private Sub tx_ウエルシア物件区分_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_ウエルシア物件区分.Leave
        '    [sb_Msg].Items.Item(0).Text = ""
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_ウエルシア物件区分.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_ウエルシア物件区分].Undo()
        End If
        ReturnF = False
    End Sub

    Private Sub tx_s見積日Y_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_s見積日Y.Enter
        '入力チェック
        If Item_Check(([tx_s見積日Y].TabIndex)) = False Then
            Exit Sub
        End If
        PreviousControl = Me.ActiveControl
    End Sub

    Private Sub tx_s見積日Y_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_s見積日Y.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii <> 8 Then ' バックスペースは例外
            'UPGRADE_WARNING: TextBox プロパティ tx_s見積日Y.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
            'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
            'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
            If LenB([tx_s見積日Y].Text & Chr(KeyAscii)) - LenB([tx_s見積日Y].SelText) = [tx_s見積日Y].MaxLength Then
                ReturnF = True
                [tx_s見積日Y].Focus()
            End If
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub tx_s見積日Y_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_s見積日Y.KeyDown
        ReturnF = True
    End Sub

    Private Sub tx_s見積日Y_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_s見積日Y.Leave
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_s見積日Y.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_s見積日Y].Undo()
        End If
        ReturnF = False
    End Sub

    Private Sub tx_s見積日M_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_s見積日M.Enter
        '入力チェック
        If Item_Check(([tx_s見積日M].TabIndex)) = False Then
            Exit Sub
        End If
        PreviousControl = Me.ActiveControl
    End Sub

    Private Sub tx_s見積日M_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_s見積日M.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii <> 8 Then ' バックスペースは例外
            'UPGRADE_WARNING: TextBox プロパティ tx_s見積日M.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
            'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
            'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
            If LenB([tx_s見積日M].Text & Chr(KeyAscii)) - LenB([tx_s見積日M].SelText) = [tx_s見積日M].MaxLength Then
                If KeyAscii <> 0 Then
                    ReturnF = True
                    [tx_s見積日M].Focus()
                End If
            Else
                Select Case Chr(KeyAscii)
                    Case CStr(2) To CStr(9)
                        ReturnF = True
                        [tx_s見積日D].Focus()
                End Select
            End If
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub tx_s見積日M_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_s見積日M.KeyDown
        ReturnF = True
    End Sub

    Private Sub tx_s見積日M_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_s見積日M.Leave
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_s見積日M.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_s見積日M].Undo()
        End If
        ReturnF = False
    End Sub

    Private Sub tx_s見積日D_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_s見積日D.Enter
        '入力チェック
        If Item_Check(([tx_s見積日D].TabIndex)) = False Then
            Exit Sub
        End If
        PreviousControl = Me.ActiveControl
    End Sub

    Private Sub tx_s見積日D_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_s見積日D.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii <> 8 Then ' バックスペースは例外
            'UPGRADE_WARNING: TextBox プロパティ tx_s見積日D.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
            'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
            'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
            If LenB([tx_s見積日D].Text & Chr(KeyAscii)) - LenB([tx_s見積日D].SelText) = [tx_s見積日D].MaxLength Then
                If KeyAscii <> 0 Then '入力キーがエラーでないならば
                    ReturnF = True
                    [tx_s見積日D].Focus()
                End If
            Else
                Select Case Chr(KeyAscii)
                    Case CStr(4) To CStr(9)
                        ReturnF = True
                        [tx_s見積日D].Focus()
                End Select
            End If
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub tx_s見積日D_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_s見積日D.KeyDown
        ReturnF = True
    End Sub

    Private Sub tx_s見積日D_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_s見積日D.Leave
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_s見積日D.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_s見積日D].Undo()
        End If
        ReturnF = False
    End Sub

    Private Sub tx_e見積日Y_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_e見積日Y.Enter
        '入力チェック
        If Item_Check(([tx_e見積日Y].TabIndex)) = False Then
            Exit Sub
        End If
        PreviousControl = Me.ActiveControl
    End Sub

    Private Sub tx_e見積日Y_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_e見積日Y.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii <> 8 Then ' バックスペースは例外
            'UPGRADE_WARNING: TextBox プロパティ tx_e見積日Y.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
            'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
            'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
            If LenB([tx_e見積日Y].Text & Chr(KeyAscii)) - LenB([tx_e見積日Y].SelText) = [tx_e見積日Y].MaxLength Then
                ReturnF = True
                [tx_e見積日Y].Focus()
            End If
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub tx_e見積日Y_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_e見積日Y.KeyDown
        ReturnF = True
    End Sub

    Private Sub tx_e見積日Y_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_e見積日Y.Leave
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_e見積日Y.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_e見積日Y].Undo()
        End If
        ReturnF = False
    End Sub

    Private Sub tx_e見積日M_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_e見積日M.Enter
        '入力チェック
        If Item_Check(([tx_e見積日M].TabIndex)) = False Then
            Exit Sub
        End If
        PreviousControl = Me.ActiveControl
    End Sub

    Private Sub tx_e見積日M_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_e見積日M.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii <> 8 Then ' バックスペースは例外
            'UPGRADE_WARNING: TextBox プロパティ tx_e見積日M.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
            'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
            'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
            If LenB([tx_e見積日M].Text & Chr(KeyAscii)) - LenB([tx_e見積日M].SelText) = [tx_e見積日M].MaxLength Then
                If KeyAscii <> 0 Then
                    ReturnF = True
                    [tx_e見積日M].Focus()
                End If
            Else
                Select Case Chr(KeyAscii)
                    Case CStr(2) To CStr(9)
                        ReturnF = True
                        [tx_e見積日D].Focus()
                End Select
            End If
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub tx_e見積日M_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_e見積日M.KeyDown
        ReturnF = True
    End Sub

    Private Sub tx_e見積日M_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_e見積日M.Leave
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_e見積日M.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_e見積日M].Undo()
        End If
        ReturnF = False
    End Sub

    Private Sub tx_e見積日D_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_e見積日D.Enter
        '入力チェック
        If Item_Check(([tx_e見積日D].TabIndex)) = False Then
            Exit Sub
        End If
        PreviousControl = Me.ActiveControl
    End Sub

    Private Sub tx_e見積日D_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_e見積日D.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii <> 8 Then ' バックスペースは例外
            'UPGRADE_WARNING: TextBox プロパティ tx_e見積日D.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
            'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
            'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
            If LenB([tx_e見積日D].Text & Chr(KeyAscii)) - LenB([tx_e見積日D].SelText) = [tx_e見積日D].MaxLength Then
                If KeyAscii <> 0 Then '入力キーがエラーでないならば
                    ReturnF = True
                    [tx_e見積日D].Focus()
                End If
            Else
                Select Case Chr(KeyAscii)
                    Case CStr(4) To CStr(9)
                        ReturnF = True
                        [tx_e見積日D].Focus()
                End Select
            End If
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub tx_e見積日D_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_e見積日D.KeyDown
        ReturnF = True
    End Sub

    Private Sub tx_e見積日D_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_e見積日D.Leave
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_e見積日D.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_e見積日D].Undo()
        End If
        ReturnF = False
    End Sub

    '2016/10/26 ADD↓
    Private Sub tx_s開始納期Y_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_s開始納期Y.Enter
        '入力チェック
        If Item_Check(([tx_s開始納期Y].TabIndex)) = False Then
            Exit Sub
        End If
        PreviousControl = Me.ActiveControl
    End Sub

    Private Sub tx_s開始納期Y_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_s開始納期Y.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii <> 8 Then ' バックスペースは例外
            'UPGRADE_WARNING: TextBox プロパティ tx_s開始納期Y.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
            'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
            'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
            If LenB([tx_s開始納期Y].Text & Chr(KeyAscii)) - LenB([tx_s開始納期Y].SelText) = [tx_s開始納期Y].MaxLength Then
                ReturnF = True
                [tx_s開始納期Y].Focus()
            End If
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub tx_s開始納期Y_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_s開始納期Y.KeyDown
        ReturnF = True
    End Sub

    Private Sub tx_s開始納期Y_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_s開始納期Y.Leave
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_s開始納期Y.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_s開始納期Y].Undo()
        End If
        ReturnF = False
    End Sub

    Private Sub tx_s開始納期M_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_s開始納期M.Enter
        '入力チェック
        If Item_Check(([tx_s開始納期M].TabIndex)) = False Then
            Exit Sub
        End If
        PreviousControl = Me.ActiveControl
    End Sub

    Private Sub tx_s開始納期M_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_s開始納期M.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii <> 8 Then ' バックスペースは例外
            'UPGRADE_WARNING: TextBox プロパティ tx_s開始納期M.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
            'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
            'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
            If LenB([tx_s開始納期M].Text & Chr(KeyAscii)) - LenB([tx_s開始納期M].SelText) = [tx_s開始納期M].MaxLength Then
                If KeyAscii <> 0 Then
                    ReturnF = True
                    [tx_s開始納期M].Focus()
                End If
            Else
                Select Case Chr(KeyAscii)
                    Case CStr(2) To CStr(9)
                        ReturnF = True
                        [tx_s開始納期D].Focus()
                End Select
            End If
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub tx_s開始納期M_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_s開始納期M.KeyDown
        ReturnF = True
    End Sub

    Private Sub tx_s開始納期M_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_s開始納期M.Leave
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_s開始納期M.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_s開始納期M].Undo()
        End If
        ReturnF = False
    End Sub

    Private Sub tx_s開始納期D_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_s開始納期D.Enter
        '入力チェック
        If Item_Check(([tx_s開始納期D].TabIndex)) = False Then
            Exit Sub
        End If
        PreviousControl = Me.ActiveControl
    End Sub

    Private Sub tx_s開始納期D_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_s開始納期D.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii <> 8 Then ' バックスペースは例外
            'UPGRADE_WARNING: TextBox プロパティ tx_s開始納期D.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
            'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
            'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
            If LenB([tx_s開始納期D].Text & Chr(KeyAscii)) - LenB([tx_s開始納期D].SelText) = [tx_s開始納期D].MaxLength Then
                If KeyAscii <> 0 Then '入力キーがエラーでないならば
                    ReturnF = True
                    [tx_s開始納期D].Focus()
                End If
            Else
                Select Case Chr(KeyAscii)
                    Case CStr(4) To CStr(9)
                        ReturnF = True
                        [tx_s開始納期D].Focus()
                End Select
            End If
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub tx_s開始納期D_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_s開始納期D.KeyDown
        ReturnF = True
    End Sub

    Private Sub tx_s開始納期D_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_s開始納期D.Leave
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_s開始納期D.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_s開始納期D].Undo()
        End If
        ReturnF = False
    End Sub

    Private Sub tx_e開始納期Y_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_e開始納期Y.Enter
        '入力チェック
        If Item_Check(([tx_e開始納期Y].TabIndex)) = False Then
            Exit Sub
        End If
        PreviousControl = Me.ActiveControl
    End Sub

    Private Sub tx_e開始納期Y_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_e開始納期Y.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii <> 8 Then ' バックスペースは例外
            'UPGRADE_WARNING: TextBox プロパティ tx_e開始納期Y.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
            'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
            'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
            If LenB([tx_e開始納期Y].Text & Chr(KeyAscii)) - LenB([tx_e開始納期Y].SelText) = [tx_e開始納期Y].MaxLength Then
                ReturnF = True
                [tx_e開始納期Y].Focus()
            End If
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub tx_e開始納期Y_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_e開始納期Y.KeyDown
        ReturnF = True
    End Sub

    Private Sub tx_e開始納期Y_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_e開始納期Y.Leave
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_e開始納期Y.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_e開始納期Y].Undo()
        End If
        ReturnF = False
    End Sub

    Private Sub tx_e開始納期M_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_e開始納期M.Enter
        '入力チェック
        If Item_Check(([tx_e開始納期M].TabIndex)) = False Then
            Exit Sub
        End If
        PreviousControl = Me.ActiveControl
    End Sub

    Private Sub tx_e開始納期M_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_e開始納期M.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii <> 8 Then ' バックスペースは例外
            'UPGRADE_WARNING: TextBox プロパティ tx_e開始納期M.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
            'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
            'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
            If LenB([tx_e開始納期M].Text & Chr(KeyAscii)) - LenB([tx_e開始納期M].SelText) = [tx_e開始納期M].MaxLength Then
                If KeyAscii <> 0 Then
                    ReturnF = True
                    [tx_e開始納期M].Focus()
                End If
            Else
                Select Case Chr(KeyAscii)
                    Case CStr(2) To CStr(9)
                        ReturnF = True
                        [tx_e開始納期D].Focus()
                End Select
            End If
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub tx_e開始納期M_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_e開始納期M.KeyDown
        ReturnF = True
    End Sub

    Private Sub tx_e開始納期M_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_e開始納期M.Leave
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_e開始納期M.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_e開始納期M].Undo()
        End If
        ReturnF = False
    End Sub

    Private Sub tx_e開始納期D_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_e開始納期D.Enter
        '入力チェック
        If Item_Check(([tx_e開始納期D].TabIndex)) = False Then
            Exit Sub
        End If
        PreviousControl = Me.ActiveControl
    End Sub

    Private Sub tx_e開始納期D_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_e開始納期D.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii <> 8 Then ' バックスペースは例外
            'UPGRADE_WARNING: TextBox プロパティ tx_e開始納期D.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
            'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
            'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
            If LenB([tx_e開始納期D].Text & Chr(KeyAscii)) - LenB([tx_e開始納期D].SelText) = [tx_e開始納期D].MaxLength Then
                If KeyAscii <> 0 Then '入力キーがエラーでないならば
                    ReturnF = True
                    [tx_e開始納期D].Focus()
                End If
            Else
                Select Case Chr(KeyAscii)
                    Case CStr(4) To CStr(9)
                        ReturnF = True
                        [tx_e開始納期D].Focus()
                End Select
            End If
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub tx_e開始納期D_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_e開始納期D.KeyDown
        ReturnF = True
    End Sub

    Private Sub tx_e開始納期D_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_e開始納期D.Leave
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_e開始納期D.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_e開始納期D].Undo()
        End If
        ReturnF = False
    End Sub
    '2016/10/26 ADD↑

    '2020/04/16 ADD↓
    Private Sub tx_s仕入日Y_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_s仕入日Y.Enter
        '入力チェック
        If Item_Check(([tx_s仕入日Y].TabIndex)) = False Then
            Exit Sub
        End If
        PreviousControl = Me.ActiveControl
    End Sub

    Private Sub tx_s仕入日Y_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_s仕入日Y.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii <> 8 Then ' バックスペースは例外
            'UPGRADE_WARNING: TextBox プロパティ tx_s仕入日Y.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
            'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
            'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
            If LenB([tx_s仕入日Y].Text & Chr(KeyAscii)) - LenB([tx_s仕入日Y].SelText) = [tx_s仕入日Y].MaxLength Then
                ReturnF = True
                [tx_s仕入日Y].Focus()
            End If
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub tx_s仕入日Y_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_s仕入日Y.KeyDown
        ReturnF = True
    End Sub

    Private Sub tx_s仕入日Y_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_s仕入日Y.Leave
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_s仕入日Y.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_s仕入日Y].Undo()
        End If
        ReturnF = False
    End Sub

    Private Sub tx_s仕入日M_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_s仕入日M.Enter
        '入力チェック
        If Item_Check(([tx_s仕入日M].TabIndex)) = False Then
            Exit Sub
        End If
        PreviousControl = Me.ActiveControl
    End Sub

    Private Sub tx_s仕入日M_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_s仕入日M.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii <> 8 Then ' バックスペースは例外
            'UPGRADE_WARNING: TextBox プロパティ tx_s仕入日M.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
            'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
            'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
            If LenB([tx_s仕入日M].Text & Chr(KeyAscii)) - LenB([tx_s仕入日M].SelText) = [tx_s仕入日M].MaxLength Then
                If KeyAscii <> 0 Then
                    ReturnF = True
                    [tx_s仕入日M].Focus()
                End If
            Else
                Select Case Chr(KeyAscii)
                    Case CStr(2) To CStr(9)
                        ReturnF = True
                        [tx_s仕入日D].Focus()
                End Select
            End If
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub tx_s仕入日M_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_s仕入日M.KeyDown
        ReturnF = True
    End Sub

    Private Sub tx_s仕入日M_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_s仕入日M.Leave
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_s仕入日M.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_s仕入日M].Undo()
        End If
        ReturnF = False
    End Sub

    Private Sub tx_s仕入日D_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_s仕入日D.Enter
        '入力チェック
        If Item_Check(([tx_s仕入日D].TabIndex)) = False Then
            Exit Sub
        End If
        PreviousControl = Me.ActiveControl
    End Sub

    Private Sub tx_s仕入日D_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_s仕入日D.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii <> 8 Then ' バックスペースは例外
            'UPGRADE_WARNING: TextBox プロパティ tx_s仕入日D.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
            'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
            'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
            If LenB([tx_s仕入日D].Text & Chr(KeyAscii)) - LenB([tx_s仕入日D].SelText) = [tx_s仕入日D].MaxLength Then
                If KeyAscii <> 0 Then '入力キーがエラーでないならば
                    ReturnF = True
                    [tx_s仕入日D].Focus()
                End If
            Else
                Select Case Chr(KeyAscii)
                    Case CStr(4) To CStr(9)
                        ReturnF = True
                        [tx_s仕入日D].Focus()
                End Select
            End If
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub tx_s仕入日D_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_s仕入日D.KeyDown
        ReturnF = True
    End Sub

    Private Sub tx_s仕入日D_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_s仕入日D.Leave
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_s仕入日D.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_s仕入日D].Undo()
        End If
        ReturnF = False
    End Sub

    Private Sub tx_e仕入日Y_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_e仕入日Y.Enter
        '入力チェック
        If Item_Check(([tx_e仕入日Y].TabIndex)) = False Then
            Exit Sub
        End If
        PreviousControl = Me.ActiveControl
    End Sub

    Private Sub tx_e仕入日Y_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_e仕入日Y.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii <> 8 Then ' バックスペースは例外
            'UPGRADE_WARNING: TextBox プロパティ tx_e仕入日Y.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
            'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
            'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
            If LenB([tx_e仕入日Y].Text & Chr(KeyAscii)) - LenB([tx_e仕入日Y].SelText) = [tx_e仕入日Y].MaxLength Then
                ReturnF = True
                [tx_e仕入日Y].Focus()
            End If
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub tx_e仕入日Y_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_e仕入日Y.KeyDown
        ReturnF = True
    End Sub

    Private Sub tx_e仕入日Y_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_e仕入日Y.Leave
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_e仕入日Y.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_e仕入日Y].Undo()
        End If
        ReturnF = False
    End Sub

    Private Sub tx_e仕入日M_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_e仕入日M.Enter
        '入力チェック
        If Item_Check(([tx_e仕入日M].TabIndex)) = False Then
            Exit Sub
        End If
        PreviousControl = Me.ActiveControl
    End Sub

    Private Sub tx_e仕入日M_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_e仕入日M.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii <> 8 Then ' バックスペースは例外
            'UPGRADE_WARNING: TextBox プロパティ tx_e仕入日M.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
            'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
            'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
            If LenB([tx_e仕入日M].Text & Chr(KeyAscii)) - LenB([tx_e仕入日M].SelText) = [tx_e仕入日M].MaxLength Then
                If KeyAscii <> 0 Then
                    ReturnF = True
                    [tx_e仕入日M].Focus()
                End If
            Else
                Select Case Chr(KeyAscii)
                    Case CStr(2) To CStr(9)
                        ReturnF = True
                        [tx_e仕入日D].Focus()
                End Select
            End If
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub tx_e仕入日M_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_e仕入日M.KeyDown
        ReturnF = True
    End Sub

    Private Sub tx_e仕入日M_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_e仕入日M.Leave
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_e仕入日M.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_e仕入日M].Undo()
        End If
        ReturnF = False
    End Sub

    Private Sub tx_e仕入日D_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_e仕入日D.Enter
        '入力チェック
        If Item_Check(([tx_e仕入日D].TabIndex)) = False Then
            Exit Sub
        End If
        PreviousControl = Me.ActiveControl
    End Sub

    Private Sub tx_e仕入日D_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_e仕入日D.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii <> 8 Then ' バックスペースは例外
            'UPGRADE_WARNING: TextBox プロパティ tx_e仕入日D.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
            'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
            'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
            If LenB([tx_e仕入日D].Text & Chr(KeyAscii)) - LenB([tx_e仕入日D].SelText) = [tx_e仕入日D].MaxLength Then
                If KeyAscii <> 0 Then '入力キーがエラーでないならば
                    ReturnF = True
                    [tx_e仕入日D].Focus()
                End If
            Else
                Select Case Chr(KeyAscii)
                    Case CStr(4) To CStr(9)
                        ReturnF = True
                        [tx_e仕入日D].Focus()
                End Select
            End If
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub tx_e仕入日D_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_e仕入日D.KeyDown
        ReturnF = True
    End Sub

    Private Sub tx_e仕入日D_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_e仕入日D.Leave
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_e仕入日D.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_e仕入日D].Undo()
        End If
        ReturnF = False
    End Sub

    Private Sub tx_s完了日Y_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_s完了日Y.Enter
        '入力チェック
        If Item_Check(([tx_s完了日Y].TabIndex)) = False Then
            Exit Sub
        End If
        PreviousControl = Me.ActiveControl
    End Sub

    Private Sub tx_s完了日Y_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_s完了日Y.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii <> 8 Then ' バックスペースは例外
            'UPGRADE_WARNING: TextBox プロパティ tx_s完了日Y.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
            'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
            'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
            If LenB([tx_s完了日Y].Text & Chr(KeyAscii)) - LenB([tx_s完了日Y].SelText) = [tx_s完了日Y].MaxLength Then
                ReturnF = True
                [tx_s完了日Y].Focus()
            End If
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub tx_s完了日Y_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_s完了日Y.KeyDown
        ReturnF = True
    End Sub

    Private Sub tx_s完了日Y_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_s完了日Y.Leave
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_s完了日Y.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_s完了日Y].Undo()
        End If
        ReturnF = False
    End Sub

    Private Sub tx_s完了日M_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_s完了日M.Enter
        '入力チェック
        If Item_Check(([tx_s完了日M].TabIndex)) = False Then
            Exit Sub
        End If
        PreviousControl = Me.ActiveControl
    End Sub

    Private Sub tx_s完了日M_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_s完了日M.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii <> 8 Then ' バックスペースは例外
            'UPGRADE_WARNING: TextBox プロパティ tx_s完了日M.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
            'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
            'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
            If LenB([tx_s完了日M].Text & Chr(KeyAscii)) - LenB([tx_s完了日M].SelText) = [tx_s完了日M].MaxLength Then
                If KeyAscii <> 0 Then
                    ReturnF = True
                    [tx_s完了日M].Focus()
                End If
            Else
                Select Case Chr(KeyAscii)
                    Case CStr(2) To CStr(9)
                        ReturnF = True
                        [tx_s完了日D].Focus()
                End Select
            End If
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub tx_s完了日M_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_s完了日M.KeyDown
        ReturnF = True
    End Sub

    Private Sub tx_s完了日M_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_s完了日M.Leave
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_s完了日M.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_s完了日M].Undo()
        End If
        ReturnF = False
    End Sub

    Private Sub tx_s完了日D_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_s完了日D.Enter
        '入力チェック
        If Item_Check(([tx_s完了日D].TabIndex)) = False Then
            Exit Sub
        End If
        PreviousControl = Me.ActiveControl
    End Sub

    Private Sub tx_s完了日D_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_s完了日D.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii <> 8 Then ' バックスペースは例外
            'UPGRADE_WARNING: TextBox プロパティ tx_s完了日D.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
            'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
            'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
            If LenB([tx_s完了日D].Text & Chr(KeyAscii)) - LenB([tx_s完了日D].SelText) = [tx_s完了日D].MaxLength Then
                If KeyAscii <> 0 Then '入力キーがエラーでないならば
                    ReturnF = True
                    [tx_s完了日D].Focus()
                End If
            Else
                Select Case Chr(KeyAscii)
                    Case CStr(4) To CStr(9)
                        ReturnF = True
                        [tx_s完了日D].Focus()
                End Select
            End If
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub tx_s完了日D_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_s完了日D.KeyDown
        ReturnF = True
    End Sub

    Private Sub tx_s完了日D_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_s完了日D.Leave
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_s完了日D.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_s完了日D].Undo()
        End If
        ReturnF = False
    End Sub

    Private Sub tx_e完了日Y_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_e完了日Y.Enter
        '入力チェック
        If Item_Check(([tx_e完了日Y].TabIndex)) = False Then
            Exit Sub
        End If
        PreviousControl = Me.ActiveControl
    End Sub

    Private Sub tx_e完了日Y_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_e完了日Y.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii <> 8 Then ' バックスペースは例外
            'UPGRADE_WARNING: TextBox プロパティ tx_e完了日Y.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
            'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
            'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
            If LenB([tx_e完了日Y].Text & Chr(KeyAscii)) - LenB([tx_e完了日Y].SelText) = [tx_e完了日Y].MaxLength Then
                ReturnF = True
                [tx_e完了日Y].Focus()
            End If
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub tx_e完了日Y_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_e完了日Y.KeyDown
        ReturnF = True
    End Sub

    Private Sub tx_e完了日Y_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_e完了日Y.Leave
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_e完了日Y.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_e完了日Y].Undo()
        End If
        ReturnF = False
    End Sub

    Private Sub tx_e完了日M_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_e完了日M.Enter
        '入力チェック
        If Item_Check(([tx_e完了日M].TabIndex)) = False Then
            Exit Sub
        End If
        PreviousControl = Me.ActiveControl
    End Sub

    Private Sub tx_e完了日M_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_e完了日M.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii <> 8 Then ' バックスペースは例外
            'UPGRADE_WARNING: TextBox プロパティ tx_e完了日M.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
            'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
            'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
            If LenB([tx_e完了日M].Text & Chr(KeyAscii)) - LenB([tx_e完了日M].SelText) = [tx_e完了日M].MaxLength Then
                If KeyAscii <> 0 Then
                    ReturnF = True
                    [tx_e完了日M].Focus()
                End If
            Else
                Select Case Chr(KeyAscii)
                    Case CStr(2) To CStr(9)
                        ReturnF = True
                        [tx_e完了日D].Focus()
                End Select
            End If
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub tx_e完了日M_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_e完了日M.KeyDown
        ReturnF = True
    End Sub

    Private Sub tx_e完了日M_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_e完了日M.Leave
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_e完了日M.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_e完了日M].Undo()
        End If
        ReturnF = False
    End Sub

    Private Sub tx_e完了日D_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_e完了日D.Enter
        '入力チェック
        If Item_Check(([tx_e完了日D].TabIndex)) = False Then
            Exit Sub
        End If
        PreviousControl = Me.ActiveControl
    End Sub

    Private Sub tx_e完了日D_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_e完了日D.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii <> 8 Then ' バックスペースは例外
            'UPGRADE_WARNING: TextBox プロパティ tx_e完了日D.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
            'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
            'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
            If LenB([tx_e完了日D].Text & Chr(KeyAscii)) - LenB([tx_e完了日D].SelText) = [tx_e完了日D].MaxLength Then
                If KeyAscii <> 0 Then '入力キーがエラーでないならば
                    ReturnF = True
                    [tx_e完了日D].Focus()
                End If
            Else
                Select Case Chr(KeyAscii)
                    Case CStr(4) To CStr(9)
                        ReturnF = True
                        [tx_e完了日D].Focus()
                End Select
            End If
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub tx_e完了日D_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_e完了日D.KeyDown
        ReturnF = True
    End Sub

    Private Sub tx_e完了日D_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_e完了日D.Leave
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_e完了日D.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_e完了日D].Undo()
        End If
        ReturnF = False
    End Sub

    Private Sub tx_s請求予定Y_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_s請求予定Y.Enter
        '入力チェック
        If Item_Check(([tx_s請求予定Y].TabIndex)) = False Then
            Exit Sub
        End If
        PreviousControl = Me.ActiveControl
        '    'ボタン名設定
        '    Call SetUpFuncs(Me.ActiveControl.Name)
        '    sb_Msg.Panels(0).Text = "請求予定を入力して下さい。"
    End Sub

    Private Sub tx_s請求予定Y_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_s請求予定Y.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii <> 8 Then ' バックスペースは例外
            'UPGRADE_WARNING: TextBox プロパティ tx_s請求予定Y.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
            'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
            'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
            If LenB([tx_s請求予定Y].Text & Chr(KeyAscii)) - LenB([tx_s請求予定Y].SelText) = [tx_s請求予定Y].MaxLength Then
                ReturnF = True
                [tx_s請求予定Y].Focus()
            End If
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub tx_s請求予定Y_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_s請求予定Y.KeyDown
        ReturnF = True
    End Sub

    Private Sub tx_s請求予定Y_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_s請求予定Y.Leave
        '    sb_Msg.Panels(0).Text = ""
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_s請求予定Y.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_s請求予定Y].Undo()
        End If
        ReturnF = False
    End Sub

    Private Sub tx_s請求予定M_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_s請求予定M.Enter
        '入力チェック
        If Item_Check(([tx_s請求予定M].TabIndex)) = False Then
            Exit Sub
        End If
        PreviousControl = Me.ActiveControl
        'ボタン名設定
        '    Call SetUpFuncs(Me.ActiveControl.Name)
        '    sb_Msg.Panels(0).Text = "請求予定を入力して下さい。"
    End Sub

    Private Sub tx_s請求予定M_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_s請求予定M.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii <> 8 Then ' バックスペースは例外
            'UPGRADE_WARNING: TextBox プロパティ tx_s請求予定M.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
            'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
            'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
            If LenB([tx_s請求予定M].Text & Chr(KeyAscii)) - LenB([tx_s請求予定M].SelText) = [tx_s請求予定M].MaxLength Then
                If KeyAscii <> 0 Then '入力キーがエラーでないならば
                    ReturnF = True
                    [tx_s請求予定M].Focus()
                End If
            Else
                Select Case Chr(KeyAscii)
                    Case CStr(2) To CStr(9)
                        ReturnF = True
                        [tx_s請求予定D].Focus()
                End Select
            End If
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub tx_s請求予定M_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_s請求予定M.KeyDown
        ReturnF = True
    End Sub

    Private Sub tx_s請求予定M_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_s請求予定M.Leave
        '    sb_Msg.Panels(0).Text = ""
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_s請求予定M.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_s請求予定M].Undo()
        End If
        ReturnF = False
    End Sub

    Private Sub tx_s請求予定D_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_s請求予定D.Enter
        '入力チェック
        If Item_Check(([tx_s請求予定D].TabIndex)) = False Then
            Exit Sub
        End If
        PreviousControl = Me.ActiveControl
    End Sub

    Private Sub tx_s請求予定D_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_s請求予定D.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii <> 8 Then ' バックスペースは例外
            'UPGRADE_WARNING: TextBox プロパティ tx_s請求予定D.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
            'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
            'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
            If LenB([tx_s請求予定D].Text & Chr(KeyAscii)) - LenB([tx_s請求予定D].SelText) = [tx_s請求予定D].MaxLength Then
                If KeyAscii <> 0 Then '入力キーがエラーでないならば
                    ReturnF = True
                    [tx_s請求予定D].Focus()
                End If
            Else
                Select Case Chr(KeyAscii)
                    Case CStr(4) To CStr(9)
                        ReturnF = True
                        [tx_s請求予定D].Focus()
                End Select
            End If
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub tx_s請求予定D_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_s請求予定D.KeyDown
        ReturnF = True
    End Sub

    Private Sub tx_s請求予定D_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_s請求予定D.Leave
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_s請求予定D.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_s請求予定D].Undo()
        End If
        ReturnF = False
    End Sub

    Private Sub tx_e請求予定Y_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_e請求予定Y.Enter
        '入力チェック
        If Item_Check(([tx_e請求予定Y].TabIndex)) = False Then
            Exit Sub
        End If
        PreviousControl = Me.ActiveControl
        '    'ボタン名設定
        '    Call SetUpFuncs(Me.ActiveControl.Name)
        '    sb_Msg.Panels(0).Text = "請求予定を入力して下さい。"
    End Sub

    Private Sub tx_e請求予定Y_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_e請求予定Y.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii <> 8 Then ' バックスペースは例外
            'UPGRADE_WARNING: TextBox プロパティ tx_e請求予定Y.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
            'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
            'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
            If LenB([tx_e請求予定Y].Text & Chr(KeyAscii)) - LenB([tx_e請求予定Y].SelText) = [tx_e請求予定Y].MaxLength Then
                ReturnF = True
                [tx_e請求予定Y].Focus()
            End If
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub tx_e請求予定Y_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_e請求予定Y.KeyDown
        ReturnF = True
    End Sub

    Private Sub tx_e請求予定Y_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_e請求予定Y.Leave
        '    sb_Msg.Panels(0).Text = ""
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_e請求予定Y.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_e請求予定Y].Undo()
        End If
        ReturnF = False
    End Sub

    Private Sub tx_e請求予定M_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_e請求予定M.Enter
        '入力チェック
        If Item_Check(([tx_e請求予定M].TabIndex)) = False Then
            Exit Sub
        End If
        PreviousControl = Me.ActiveControl
        'ボタン名設定
        '    Call SetUpFuncs(Me.ActiveControl.Name)
        '    sb_Msg.Panels(0).Text = "請求予定を入力して下さい。"
    End Sub

    Private Sub tx_e請求予定M_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_e請求予定M.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii <> 8 Then ' バックスペースは例外
            'UPGRADE_WARNING: TextBox プロパティ tx_e請求予定M.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
            'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
            'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
            If LenB([tx_e請求予定M].Text & Chr(KeyAscii)) - LenB([tx_e請求予定M].SelText) = [tx_e請求予定M].MaxLength Then
                If KeyAscii <> 0 Then '入力キーがエラーでないならば
                    ReturnF = True
                    [tx_e請求予定M].Focus()
                End If
            Else
                Select Case Chr(KeyAscii)
                    Case CStr(2) To CStr(9)
                        ReturnF = True
                        [tx_e請求予定D].Focus()
                End Select
            End If
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub tx_e請求予定M_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_e請求予定M.KeyDown
        ReturnF = True
    End Sub

    Private Sub tx_e請求予定M_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_e請求予定M.Leave
        '    sb_Msg.Panels(0).Text = ""
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_e請求予定M.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_e請求予定M].Undo()
        End If
        ReturnF = False
    End Sub

    Private Sub tx_e請求予定D_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_e請求予定D.Enter
        '入力チェック
        If Item_Check(([tx_e請求予定D].TabIndex)) = False Then
            Exit Sub
        End If
        PreviousControl = Me.ActiveControl
    End Sub

    Private Sub tx_e請求予定D_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_e請求予定D.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii <> 8 Then ' バックスペースは例外
            'UPGRADE_WARNING: TextBox プロパティ tx_e請求予定D.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
            'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
            'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
            If LenB([tx_e請求予定D].Text & Chr(KeyAscii)) - LenB([tx_e請求予定D].SelText) = [tx_e請求予定D].MaxLength Then
                If KeyAscii <> 0 Then '入力キーがエラーでないならば
                    ReturnF = True
                    [tx_e請求予定D].Focus()
                End If
            Else
                Select Case Chr(KeyAscii)
                    Case CStr(4) To CStr(9)
                        ReturnF = True
                        [tx_e請求予定D].Focus()
                End Select
            End If
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub tx_e請求予定D_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_e請求予定D.KeyDown
        ReturnF = True
    End Sub

    Private Sub tx_e請求予定D_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_e請求予定D.Leave
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_e請求予定D.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_e請求予定D].Undo()
        End If
        ReturnF = False
    End Sub

    '2021/04/11 ADD↓
    Private Sub tx_見積確定区分_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_見積確定区分.Enter
        If Item_Check(([tx_見積確定区分].TabIndex)) = False Then
            Exit Sub
        End If
        PreviousControl = Me.ActiveControl

        '    'ボタン名設定
        '    Call SetUpFuncs(Me.ActiveControl.Name)
        '    sb_Msg.Panels(0).Text = "見積確定区分を入力して下さい。"
    End Sub

    Private Sub tx_見積確定区分_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles tx_見積確定区分.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        'MaxLengthは１多く設定する。

        Const Numbers As String = "01" ' 入力許可文字
        'Dim strText As String
        If KeyAscii = System.Windows.Forms.Keys.Return Then KeyAscii = 0

        '数字検査をする
        If KeyAscii <> 8 Then ' バックスペースは例外
            If InStr(Numbers, Chr(KeyAscii)) = 0 Then
                KeyAscii = 0 ' 入力を無効にする
                Exit Sub
            Else
                'UPGRADE_WARNING: TextBox プロパティ tx_見積確定区分.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
                'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
                'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
                If LenB([tx_見積確定区分].Text) = [tx_見積確定区分].MaxLength - 1 Then
                    Call SelText([tx_見積確定区分])
                End If
                'UPGRADE_WARNING: TextBox プロパティ tx_見積確定区分.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
                'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
                'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
                If LenB([tx_見積確定区分].Text & Chr(KeyAscii)) - LenB([tx_見積確定区分].SelText) = [tx_見積確定区分].MaxLength - 1 Then
                    ReturnF = True
                    [tx_見積確定区分].NextSetFocus()
                End If
            End If
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub tx_見積確定区分_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_見積確定区分.KeyDown
        ReturnF = True
    End Sub

    Private Sub tx_見積確定区分_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tx_見積確定区分.Leave
        '    sb_Msg.Panels(0).Text = ""
        If ReturnF = False Then
            'UPGRADE_WARNING: オブジェクト tx_見積確定区分.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            [tx_見積確定区分].Undo()
        End If
        ReturnF = False
    End Sub
    '2021/04/11 ADD↑

    '2020/04/16 ADD↑
    '
    'Private Sub Purge()
    '    Dim sql As String
    '
    '    'マウスポインターを砂時計にする
    '    HourGlass True
    '
    '    Cn.BeginTrans   '---トランザクションの開始
    '    On Error GoTo Trans_err
    '
    ''    SQL = "delete FROM 製造日報MD where 製造日報NO =" & tx_NO
    ''    Cn.Execute SQL
    ''    SQL = "DELETE FROM 製造日報HD where 製造日報NO =" & tx_NO
    ''    Cn.Execute SQL
    '
    '    Cn.CommitTrans  '---トランザクションをコミットする
    '
    'Trans_Correct:
    '    On Error GoTo 0
    '
    '    HourGlass False
    '    Exit Sub
    '
    'Trans_err:  '---エラー時
    '    MsgBox Err.Number & " " & Err.Description
    '    Cn.RollbackTrans 'トランザクションを破棄する
    '    Resume Trans_Correct
    'End Sub
End Class