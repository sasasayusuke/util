Option Explicit On

Imports System.IO
Imports SnwA01.Win32API

''' <summary>
''' 
''' </summary>
Module SnwMain

    Sub Main()
        Dim SvEXEPath As String
        'Dim PGS As String
        Dim UPDATES As String

        ' "AppPath" 関数がないため、現在のアプリケーションのパスを取得するために "Application.StartupPath" を使用
        If Not File.Exists(Path.Combine(Application.StartupPath, "UPDATE.ini")) Then
            Throw New ApplicationException("ＩＮＩファイルがありません。")
            Exit Sub
        End If

        'サーバーの最新EXEが入っているフォルダを指定する。
        SvEXEPath = GetIni("Common", "SvEXEPath", "UPDATE.ini")
        If String.IsNullOrEmpty(SvEXEPath) Then
            WriteIni("Common", "SvEXEPath", DefSvEXEPath, "UPDATE.ini")
            SvEXEPath = DefSvEXEPath
        End If

        'UPDATE.iniの最新チェック
        'バージョンアップチェック
        AutoVersionUpIni(SvEXEPath, "UPDATE.ini")

        'UPDATEするファイル名を取得する。
        UPDATES = GetIni("UPDATES", "UPDATES", "UPDATE.ini")
        If String.IsNullOrEmpty(UPDATES) Then
            Throw New ApplicationException("ファイル名がありません。")
            Exit Sub
        End If

        'バージョンアップチェック
        If Not AutoVersionUp(SvEXEPath, UPDATES) Then
            '--------------------------------------------------
            'アプリケーションの初期化を行う
            '--------------------------------------------------
            If Not ApplicationInit() Then
                Exit Sub
            End If

            ' フォームの起動
            Dim F00 As New SnwA01F00()
            Application.Run(F00)
        End If

    End Sub

End Module
