Option Explicit On

Imports FarPoint.Win.Spread

''' <summary>
''' 売価変更
''' </summary>
Friend Class SnwMT02F11
    Inherits System.Windows.Forms.Form

    Dim ResultCodeSetControl As FpSpread '選択したコードの送り先をセットする。

    Dim ResultCodeGenka As Decimal '原価
    Dim ResultCodeBaika As Decimal '売価

    Dim pActRow As Integer
    Dim pActColumn As Integer

    ' VB.NETのコレクションによるコントロール配列の置き換え
    Private lOb_Conds As New List(Of RadioButton)

    Private isInitializing As Boolean = True

    '選択したコードを送るコントロールをセット
    WriteOnly Property ResCodeCTL() As FpSpread
        Set(ByVal Value As FpSpread)
            ResultCodeSetControl = Value
        End Set
    End Property

    '原価をセット
    WriteOnly Property ResCodeGenka() As Decimal
        Set(ByVal Value As Decimal)
            ResultCodeGenka = Value
        End Set
    End Property

    '売価をセット
    WriteOnly Property ResCodeBaika() As Decimal
        Set(ByVal Value As Decimal)
            ResultCodeBaika = Value
        End Set
    End Property

    'Rowをセット
    WriteOnly Property ActRow() As Integer
        Set(ByVal Value As Integer)
            pActRow = Value
        End Set
    End Property

    'Columnをセット
    WriteOnly Property ActColumn() As Integer
        Set(ByVal Value As Integer)
            pActColumn = Value
        End Set
    End Property

    Private Sub Cb中止_Click(sender As Object, e As EventArgs) Handles Cb中止.Click
        Me.Close()
    End Sub

    Private Sub CdOK_Click(sender As Object, e As EventArgs) Handles CdOK.Click
        If Len("" & rf_売価.Text) > 13 Then
            Inform("桁がオーバーフローしました。")
            '        PreviousControl.Undo
            '        PreviousControl.SetFocus
            Exit Sub
        End If

        'ResultCodeSetControl.Text = rf_売価.Text
        'ResultCodeSetControl.ActiveSheet.Cells(pActRow, pActColumn).Value = rf_売価.Text
        ResultCodeSetControl.ActiveSheet.SetText(pActRow, pActColumn, rf_売価.Text)

        Me.Close()
    End Sub

    Private Sub SnwMT02F11_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.Dispose()
    End Sub

    Private Sub SnwMT02F11_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Me.Load

        lOb_Conds.Clear()
        'ロード時にボタンを動的に配置する
        AddRadioFunction()

        'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
        rf_原価.Text = Convert.ToDecimal(SpcToNull(ResultCodeGenka, 0)).ToString("#,##0.00")
        'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
        rf_売価.Text = Convert.ToDecimal(SpcToNull(ResultCodeBaika, 0)).ToString("#,##0.00")
        If rf_原価.Text = "0.00" Then
            tx_掛率.Text = CStr(0)
        Else
            tx_掛率.Text = ISRound((CDbl(rf_売価.Text) / CDbl(rf_原価.Text)) * 100, 2).ToString("#,##0.00")
        End If
        '    rf_売価 = Format$((rf_原価 * tx_掛率) / 100, "#,##0.00")

        isInitializing = False ' 初期化完了後にフラグをリセット

    End Sub

    Private Sub AddRadioFunction()

        ' ラベル・ボタンを配置するPanelを指定
        Dim targetPanel As Panel = Pic_印刷条件

        ' 生成数を指定
        Dim genButtonWidth As Integer = 65
        Dim genButtonHeight As Integer = 30

        Dim marginX As Integer = 1
        Dim marginY As Integer = 2             ' 横余白

        ' ボタンを配置
        Dim buttonCounter As Integer = 0

        For col As Integer = 0 To 1            ' 2列
            For row As Integer = 0 To 0        ' 1行

                ' 新しいボタンを作成
                Dim btn As New RadioButton()

                ' ボタンのプロパティを設定
                btn.Tag = buttonCounter + 1                  ' IndexをTagプロパティに格納
                btn.Name = "Ob_Cond" & buttonCounter + 1
                btn.Font = New Font("ＭＳ Ｐゴシック", 9.0, FontStyle.Regular)
                btn.FlatStyle = FlatStyle.Standard
                If buttonCounter = 0 Then
                    btn.Checked = True
                End If

                btn.Size = New Size(genButtonWidth, genButtonHeight)
                btn.Location = New Point(col * (genButtonWidth + marginX) + 10, row * (genButtonHeight + marginY))
                'Debug.WriteLine($"X:{col * (genButtonWidth + marginX) + 10}, Y:{row * (genButtonHeight + marginY)}")

                Select Case buttonCounter
                    Case 0
                        btn.Text = "掛率"
                        btn.TabIndex = 1
                    Case 1
                        btn.Text = "原価率"
                        btn.TabIndex = 2
                    Case Else
                        btn.Text = ""
                End Select

                ' ボタンにクリックイベントを追加
                AddHandler btn.Enter, AddressOf Ob_Cond_Enter

                ' ボタンにクリックイベントを追加
                AddHandler btn.CheckedChanged, AddressOf Ob_Cond_CheckedChanged

                ' ボタンにクリックイベントを追加
                AddHandler btn.Leave, AddressOf Ob_Cond_Leave

                ' ボタンをPanelに追加
                targetPanel.Controls.Add(btn)

                ' リストにボタンを追加
                lOb_Conds.Add(btn)

                buttonCounter += 1

            Next
        Next

    End Sub

    Private Sub Ob_Cond_Enter(sender As Object, e As EventArgs)
        Dim rb As RadioButton = CType(sender, RadioButton)
        Dim Index As Short = CType(rb.Tag, Integer) - 1

        'NOTE SS Class定義しているのでClassのインスタンス化(module定義)
        Dim OpSubCls As OpSubCls = New OpSubCls
        Call OpSubCls.StartOption(lOb_Conds(Index))
    End Sub

    Private Sub Ob_Cond_CheckedChanged(sender As Object, e As EventArgs)
        Dim rb As RadioButton = CType(sender, RadioButton)
        Dim index As Integer = CType(rb.Tag, Integer) - 1

        If isInitializing Then
            ' 初期化中のイベントを無視
            Return
        End If

        If rb.Checked Then
            'Dim Index As Integer = Ob_条件.GetIndex(eventSender)

            If (lOb_Conds(0).Checked = True) Then
                '[lb_項目](5).Text = "掛  率"
                _lb_項目_5.Text = "掛  率"
                If CDec(rf_原価.Text) = 0 Then '2004/03/05
                Else
                    tx_掛率.Text = ISRound((CDbl(rf_売価.Text) / CDbl(rf_原価.Text)) * 100, 2).ToString("#,##0.00")
                End If
            Else
                '[lb_項目](5).Text = "原価率"
                _lb_項目_5.Text = "原価率"
                If CDec(rf_売価.Text) = 0 Then '2004/03/05
                Else
                    tx_掛率.Text = ISRound((CDbl(rf_原価.Text) / CDbl(rf_売価.Text)) * 100, 2).ToString("#,##0.00")
                End If
            End If

        End If
    End Sub

    Private Sub Ob_Cond_Leave(sender As Object, e As EventArgs)
        Dim rb As RadioButton = CType(sender, RadioButton)
        Dim index As Integer = CType(rb.Tag, Integer) - 1
        'MessageBox.Show($"RadioButton {index},{rb.Name} clicked!")
    End Sub

    'UPGRADE_ISSUE: PictureBox イベント pic_印刷条件.GotFocus はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="ABD9AF39-7E24-4AFF-AD8D-3675C1AA3054"' をクリックしてください。
    Private Sub Pic_印刷条件_GotFocus()
        If (lOb_Conds(0).Checked = True) Then
            lOb_Conds(0).Focus()
        ElseIf (lOb_Conds(1).Checked = True) Then
            lOb_Conds(1).Focus()
        End If
    End Sub

    Private Sub tx_掛率_Leave(sender As Object, e As EventArgs) Handles tx_掛率.Leave
        Select Case True
            Case lOb_Conds(0).Checked
                rf_売価.Text = ((CDbl(rf_原価.Text) * CDbl(tx_掛率.Text)) / 100).ToString("#,##0.00")
            Case lOb_Conds(1).Checked
                If CDec(tx_掛率.Text) = 0 Then '2004/04/01
                Else
                    rf_売価.Text = ((CDbl(rf_原価.Text) / CDbl(tx_掛率.Text)) * 100).ToString("#,##0.00")
                End If
        End Select
    End Sub

End Class