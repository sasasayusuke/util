''' <summary>
''' 表示項目設定
''' </summary>
Friend Class SnwMT02F10
    Inherits System.Windows.Forms.Form

    'List配列CkDispのCheckBox
    Private lCkDisp As New List(Of CheckBox)

    Private isInitializing As Boolean = True

    Private Sub CbCANCEL_Click(sender As Object, e As EventArgs) Handles CbCANCEL.Click
        Me.Close()
    End Sub

    Private Sub CbOK_Click(sender As Object, e As EventArgs) Handles CbOK.Click
        Dim Disp_Spd As String

        Disp_Spd = ""

        ' チェックボックスを配列に取り込む
        Dim CheckBoxes = Me.ChkFunction.Controls.OfType(Of CheckBox)().ToArray()

        ' 配列内のチェックボックスをループで処理
        For Each chk As CheckBox In CheckBoxes
            If Disp_Spd <> "" Then Disp_Spd += ","
            If chk.CheckState = CheckState.Unchecked Then
                Disp_Spd += "0"
            Else
                Disp_Spd += "1"
            End If
        Next

        WriteIni("Disp", "MT02F00_SPD", Disp_Spd, INIFile)

        Me.Close()
    End Sub

    Private Sub SnwMT02F10_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.Dispose()
    End Sub

    Private Sub SnwMT02F10_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Me.Load
        Dim i As Integer
        Dim Disp_Spd As String
        Dim vDisp_Spd As Object

        lCkDisp.Clear()
        'ロード時にボタンを動的に配置する
        AddChkFunction()

        INIFile = "SanwaS.ini"

        '表示項目設定取得
        'UPGRADE_WARNING: オブジェクト GetIni() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
        Disp_Spd = GetIni("Disp", "MT02F00_SPD", INIFile)

        'UPGRADE_WARNING: オブジェクト vDisp_Spd の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
        vDisp_Spd = Split(Disp_Spd, ",")

        For i = 0 To 26 -1
            'UPGRADE_WARNING: オブジェクト vDisp_Spd(i) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            'UPGRADE_WARNING: オブジェクト vDisp_Spd() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
            If vDisp_Spd(i).ToString = "0" Then
                lCkDisp(i).CheckState = CheckState.Unchecked
            Else
                lCkDisp(i).CheckState = CheckState.Checked
            End If
        Next

        isInitializing = False ' 初期化完了後にフラグをリセット

	End Sub

	Private Sub AddChkFunction()

        ' ラベル・ボタンを配置するPanelを指定
        Dim targetPanel As Panel = ChkFunction

        ' 生成数を指定
        Dim genButtonWidth As Integer = 100
        Dim genButtonHeight As Integer = 30

        Dim marginX As Integer = 10            ' 縦余白
        Dim marginY As Integer = 2             ' 横余白

        ' ボタンを配置
        Dim buttonCounter As Integer = 0

        For col As Integer = 0 To 2            ' 3列
            For row As Integer = 0 To 8        ' 9行
                If col = 1 And row = 8 Then
                Else
                    ' 新しいボタンを作成
                    Dim btn As New CheckBox()

                    ' ボタンのプロパティを設定
                    btn.Tag = buttonCounter + 1                  ' IndexをTagプロパティに格納
                    btn.Name = "CkDisp" & buttonCounter + 1
                    btn.Font = New Font("ＭＳ Ｐゴシック", 9.0, FontStyle.Regular)
                    btn.FlatStyle = FlatStyle.Standard

                    btn.Size = New Size(genButtonWidth, genButtonHeight)
                    btn.Location = New Point(col * (genButtonWidth + marginX) + 10, row * (genButtonHeight + marginY))
                    'Debug.WriteLine($"X:{col * (genButtonWidth + marginX) + 10}, Y:{row * (genButtonHeight + marginY)}")

                    Select Case buttonCounter
                        Case 0
                            btn.Text = "SP区分"
                        Case 1
                            btn.Text = "PC区分"
                        Case 2
                            btn.Text = "ベース色"
                        Case 3
                            btn.Text = "名称"
                        Case 4
                            btn.Text = "D1～H2"
                        Case 5
                            btn.Text = "定価"
                        Case 6
                            btn.Text = "Ｕ"
                        Case 7
                            btn.Text = "原価"
                        Case 8
                            btn.Text = "仕入％"
                        Case 9
                            btn.Text = "売価"
                        Case 10
                            btn.Text = "売上％"
                        Case 11
                            btn.Text = "Ｍ"
                        Case 12
                            btn.Text = "数量"
                        Case 13
                            btn.Text = "単位"
                        Case 14
                            btn.Text = "金額"
                        Case 15
                            btn.Text = "仕入先CD"
                        Case 16
                            btn.Text = "仕入先名"
                        Case 17
                            btn.Text = "送り先CD"
                        Case 18
                            btn.Text = "送り先名"
                        Case 19
                            btn.Text = "社内在庫"
                        Case 20
                            btn.Text = "客先在庫"
                        Case 21
                            btn.Text = "転用"
                        Case 22
                            btn.Text = "発注数"
                        Case 23
                            btn.Text = "伝票番号(S)"
                        Case 24
                            btn.Text = "納品日付(S)"
                        Case 25
                            btn.Text = "明細備考"
                        Case Else
                            btn.Text = ""
                    End Select

                    ' ボタンにクリックイベントを追加
                    AddHandler btn.CheckedChanged, AddressOf Ck_Disp_CheckedChanged

                    ' ボタンをPanelに追加
                    targetPanel.Controls.Add(btn)

                    ' リストにボタンを追加
                    lCkDisp.Add(btn)

                    buttonCounter += 1

                End If

            Next
        Next

    End Sub

    Private Sub Ck_Disp_CheckedChanged(sender As Object, e As EventArgs)
        Dim cb As CheckBox = CType(sender, CheckBox)
        Dim index As Integer = CType(cb.Tag, Integer) - 1
        'MessageBox.Show($"CheckBox {index},{cb.Name} clicked!")

        If isInitializing Then
            ' 初期化中のイベントを無視
            Return
        End If

    End Sub

End Class