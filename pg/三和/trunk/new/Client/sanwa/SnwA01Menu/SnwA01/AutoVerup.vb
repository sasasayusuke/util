Option Strict Off
Option Explicit On

Imports System.IO
Imports Microsoft.VisualBasic.FileIO

''' <summary>
''' 
''' </summary>
Public Module AutoVerup

    ' 自動バージョンアップINIファイル用
    Public Function AutoVersionUpIni(sSetUpFolderUNC As String, Optional sFileName As String = "") As Boolean
        Dim sSource As String        '新バージョンのファイル
        Dim sExeName As String       '実行ファイル名
        Dim sExeFullPath As String
        Dim isModified As Boolean
        Dim ExeIsModified As Boolean

        ' 指定されたフォルダが存在するか確認
        If Not Directory.Exists(sSetUpFolderUNC) Then
            Return False
        End If

        ' Iniファイルのコピー判定
        sExeName = sFileName
        sSource = Path.Combine(sSetUpFolderUNC, sExeName)
        sExeFullPath = Path.Combine(AppPath(), sExeName)
        ExeIsModified = NewCopyFile(sSource, sExeFullPath)
        isModified = isModified Or ExeIsModified

        ' ファイルコピーされていたら
        If isModified Then
            Return True
        End If

        Return False
    End Function

    ' 自動バージョンアップ
    Public Function AutoVersionUp(sSetUpFolderUNC As String, Optional sFileName As String = "") As Boolean
        Dim sSource As String        '新バージョンのファイル
        Dim sDest As String          'ファイルのコピー先
        Dim sScript As String        '再起動のスクリプトファイル名
        Dim sExeName As String       '実行ファイル名
        Dim sExeFullPath As String
        Dim sFileArray() As String
        Dim isModified As Boolean = False
        Dim ExeIsModified As Boolean = False

        ' 指定されたフォルダが存在するか確認
        If Not Directory.Exists(sSetUpFolderUNC) Then
            Return False
        End If

        ' ファイルリストにあるファイルをコピー
        sFileArray = sFileName.Split(","c)
        For Each file In sFileArray
            sSource = Path.Combine(sSetUpFolderUNC, file.Trim())
            sDest = Path.Combine(AppPath(), file.Trim())
            isModified = isModified Or NewCopyFile(sSource, sDest)
        Next

        ' Exeファイルのコピー判定
        sExeName = My.Application.Info.AssemblyName & ".exe"
        sSource = Path.Combine(sSetUpFolderUNC, sExeName)
        sExeFullPath = Path.Combine(AppPath(), sExeName)
        ExeIsModified = NewCopyFile(sSource, sExeFullPath, False)  '実際にはコピーしない
        isModified = isModified Or ExeIsModified

        ' ファイルコピーされていたらプログラムを再起動
        If isModified Then
            sScript = Path.Combine(AppPath(), "update.vbs")
            ' 再起動するスクリプトファイルを作成する
            Using ts As New StreamWriter(sScript, False)
                ts.WriteLine("Dim fso, wsh")
                ts.WriteLine("Set fso = CreateObject(""Scripting.FileSystemObject"")")
                If ExeIsModified Then
                    ts.WriteLine($"fso.CopyFile ""{sSource}"", ""{sExeFullPath}""")
                End If
                ts.WriteLine("Set wsh = CreateObject(""WScript.Shell"")")
                ts.WriteLine($"wsh.Run ""{sExeFullPath}""")
                'ts.WriteLine($"fso.DeleteFile ""{sScript}""")
            End Using

            ' スクリプトファイルを実行する
            If File.Exists(sScript) Then
                Dim wsh = CreateObject("WScript.Shell")
                wsh.Run(sScript, 0, True)
                wsh = Nothing
            End If

            ' スクリプトの削除
            If File.Exists(sScript) Then
                File.Delete(sScript)
            End If
            Return True
        End If

        Return False
    End Function

    Public Function GetFileNode(sFullPath As String) As String
        Return Path.GetFileName(sFullPath)
    End Function

    ' ファイルが新しければコピー
    Private Function NewCopyFile(sSource As String, sDest As String, Optional bExecute As Boolean = True) As Boolean
        If Not File.Exists(sSource) Then
            ' 元のファイルがないのでコピーできない
            Return False
        End If

        Dim fileSource = New FileInfo(sSource)
        Dim fileDest = New FileInfo(sDest)

        ' ファイルが新しければコピー
        If Not File.Exists(sDest) OrElse fileSource.LastWriteTime > fileDest.LastWriteTime Then
            If bExecute Then
                fileSource.CopyTo(sDest, True)
            End If
            Return True
        End If

        Return False
    End Function

    ' アプリケーションのパスを取得する関数
    Private Function AppPath() As String
        Return AppDomain.CurrentDomain.BaseDirectory
    End Function

End Module
