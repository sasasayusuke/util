Option Strict Off
Option Explicit On

Imports System.Runtime.InteropServices
Imports System.Text

Friend Class FrmAbout
    Inherits System.Windows.Forms.Form

    ' ﾚｼﾞｽﾄﾘ ｷｰ ｾｷｭﾘﾃｨ ｵﾌﾟｼｮﾝ...
    Const READ_CONTROL As Integer = &H20000
    Const KEY_QUERY_VALUE As Integer = &H1
    Const KEY_SET_VALUE As Integer = &H2
    Const KEY_CREATE_SUB_KEY As Integer = &H4
    Const KEY_ENUMERATE_SUB_KEYS As Integer = &H8
    Const KEY_NOTIFY As Integer = &H10
    Const KEY_CREATE_LINK As Integer = &H20
    Const KEY_READ As Integer = &H20019
    Const KEY_ALL_ACCESS As Double = KEY_QUERY_VALUE + KEY_SET_VALUE + KEY_CREATE_SUB_KEY + KEY_ENUMERATE_SUB_KEYS + KEY_NOTIFY + KEY_CREATE_LINK + READ_CONTROL

    ' ﾚｼﾞｽﾄﾘ ｷｰ ROOT 型...
    Const HKEY_LOCAL_MACHINE As Integer = &H80000002
    Const ERROR_SUCCESS As Short = 0
    Const REG_SZ As Integer = 1 ' Null 文字で終わる Unicode 文字列
    Const REG_EXPAND_SZ As Integer = 2 ' Null 文字で終わる Unicode 文字列
    Const REG_DWORD As Integer = 4 ' 32 ﾋﾞｯﾄ数値

    Const gREGKEYSYSINFOLOC As String = "SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths"
    Const gREGVALSYSINFOLOC As String = "MSINFO"
    Const gREGKEYSYSINFO As String = "SOFTWARE\Microsoft\Shared Tools\MSINFO"
    Const gREGVALSYSINFO As String = "PATH"

    'Private Declare Function RegOpenKeyEx Lib "advapi32"  Alias "RegOpenKeyExA"(ByVal hKey As Integer, ByVal lpSubKey As String, ByVal ulOptions As Integer, ByVal samDesired As Integer, ByRef phkResult As Integer) As Integer
    <DllImport("advapi32.dll", SetLastError:=True, CharSet:=CharSet.Ansi)>
    Private Shared Function RegOpenKeyEx(hKey As Integer, lpSubKey As String, ulOptions As Integer, samDesired As Integer, ByRef phkResult As Integer) As Integer
    End Function

    'Private Declare Function RegQueryValueEx Lib "advapi32"  Alias "RegQueryValueExA"(ByVal hKey As Integer, ByVal lpValueName As String, ByVal lpReserved As Integer, ByRef lpType As Integer, ByVal lpData As String, ByRef lpcbData As Integer) As Integer
    <DllImport("advapi32.dll", SetLastError:=True, CharSet:=CharSet.Ansi)>
    Private Shared Function RegQueryValueEx(hKey As Integer, lpValueName As String, lpReserved As IntPtr, ByRef lpType As Integer, lpData As StringBuilder, ByRef lpcbData As Integer) As Integer
    End Function

    'Private Declare Function RegCloseKey Lib "advapi32" (ByVal hKey As Integer) As Integer
    <DllImport("advapi32.dll", SetLastError:=True)>
    Private Shared Function RegCloseKey(hKey As Integer) As Integer
    End Function

    Private Sub CmdSysInfo_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSysInfo.Click
        Call StartSysInfo()
    End Sub

    Private Sub CmdOK_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOK.Click
        Me.Close()
    End Sub

    Private Sub FrmAbout_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        Me.Text = My.Application.Info.Title & " ﾊﾞｰｼﾞｮﾝ情報"
        lblTitle.Text = My.Application.Info.Title & " Ver." & My.Application.Info.Version.Major & "." & My.Application.Info.Version.Minor
        lblVersion.Text = My.Application.Info.Title & "  Build:" & My.Application.Info.Version.Major & "." & My.Application.Info.Version.Minor & "." & My.Application.Info.Version.Revision
        lblCopyLight.Text = My.Application.Info.Copyright
        lblDisclaimer.Text = lblDisclaimer.Text & " " & My.Application.Info.Description
        lblDescription.Text = ""
    End Sub

    Public Sub StartSysInfo()
        Try
        	'Dim rc As Integer
            Dim SysInfoPath As String = String.Empty

        	' ﾚｼﾞｽﾄﾘからｼｽﾃﾑ情報ﾌﾟﾛｸﾞﾗﾑのﾊﾟｽ\名前を取得します...
        	If GetKeyValue(HKEY_LOCAL_MACHINE, gREGKEYSYSINFO, gREGVALSYSINFO, SysInfoPath) Then
            	' ﾚｼﾞｽﾄﾘからｼｽﾃﾑ情報ﾌﾟﾛｸﾞﾗﾑのﾊﾟｽ名のみを取得します...
        	ElseIf GetKeyValue(HKEY_LOCAL_MACHINE, gREGKEYSYSINFOLOC, gREGVALSYSINFOLOC, SysInfoPath) Then
            	' 既に存在するはずの 32 ﾋﾞｯﾄ ﾊﾞｰｼﾞｮﾝのﾌｧｲﾙを確認します。
            	'UPGRADE_WARNING: Dir に新しい動作が指定されています。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
            	If (Dir(SysInfoPath & "\MSINFO32.EXE") <> "") Then
                	SysInfoPath = SysInfoPath & "\MSINFO32.EXE"
            	Else
                	' ｴﾗｰ - ﾌｧｲﾙが見つかりません...
                    Throw New Exception("ファイルが見つかりません。")
            	End If
        	Else
            	' ｴﾗｰ - ﾚｼﾞｽﾄﾘ ｴﾝﾄﾘが見つかりません...
                Throw New Exception("レジストリエントリが見つかりません。")
        	End If

        	Call Shell(SysInfoPath, AppWinStyle.NormalFocus)

        Catch ex As Exception
            ' エラーメッセージを表示
        	MsgBox("現時点ではシステム情報を使用できません", MsgBoxStyle.OkOnly)
        End Try
    End Sub

    Public Function GetKeyValue(ByRef KeyRoot As Integer, ByRef KeyName As String, ByRef SubKeyRef As String, ByRef KeyVal As String) As Boolean
        'Dim i As Integer ' ﾙｰﾌﾟ ｶｳﾝﾀ
        Dim rc As Integer ' 戻り値
        Dim hKey As Integer ' ｵｰﾌﾟﾝしたﾚｼﾞｽﾄﾘ ｷｰのﾊﾝﾄﾞﾙ
        'Dim hDepth As Integer '
        Dim KeyValType As Integer ' ﾚｼﾞｽﾄﾘ ｷｰのﾃﾞｰﾀ型
        Dim tmpVal As StringBuilder ' ﾚｼﾞｽﾄﾘ ｷｰ値の一時保存用変数
        Dim KeyValSize As Integer ' ﾚｼﾞｽﾄﾘ ｷｰ変数のｻｲｽﾞ
        '------------------------------------------------------------
        ' ﾙｰﾄ ｷｰ {HKEY_LOCAL_MACHINE...} にあるﾚｼﾞｽﾄﾘ ｷｰを開きます。
        '------------------------------------------------------------
        rc = RegOpenKeyEx(KeyRoot, KeyName, 0, KEY_READ, hKey) ' ﾚｼﾞｽﾄﾘ ｷｰを開く

        If (rc <> ERROR_SUCCESS) Then GoTo GetKeyError ' ﾊﾝﾄﾞﾙ ｴﾗｰ...

        tmpVal = New StringBuilder(Chr(0), 1024) ' 変数領域の割り当て
        KeyValSize = tmpVal.Capacity ' 変数のｻｲｽﾞを記憶
        KeyValType = 0

        '------------------------------------------------------------
        ' ﾚｼﾞｽﾄﾘ ｷｰ値を取得します...
        '------------------------------------------------------------
        rc = RegQueryValueEx(hKey, SubKeyRef, 0, KeyValType, tmpVal, KeyValSize) ' ｷｰ値の取得/作成

        If (rc <> ERROR_SUCCESS) Then GoTo GetKeyError ' ﾊﾝﾄﾞﾙ ｴﾗｰ

        'If (Asc(Mid(tmpVal, KeyValSize, 1)) = 0) Then ' Win95 は Null で終わる文字列を追加します...
        '    tmpVal = VB6Conv.Left(tmpVal, KeyValSize - 1) ' Null が見つかったら、文字列から抽出します。
        'Else ' WinNT は Null で終わる文字列を使用しません...
        '    tmpVal = VB6Conv.Left(tmpVal, KeyValSize) ' Null が見つからなかったら、文字列のみを抽出します。
        'End If
        '------------------------------------------------------------
        ' 変換のために、ｷｰ値の型を調べます...
        '------------------------------------------------------------
        Select Case KeyValType ' ﾃﾞｰﾀ型検索...
            Case REG_SZ ' String ﾚｼﾞｽﾄﾘ ｷｰ ﾃﾞｰﾀ型
            Case REG_EXPAND_SZ
                'KeyVal = tmpVal ' String 値をｺﾋﾟｰ
                KeyVal = tmpVal.ToString().TrimEnd(ChrW(0)) ' Null 終端を削除
            Case REG_DWORD ' Double Word ﾚｼﾞｽﾄﾘ ｷｰ ﾃﾞｰﾀ型
                'For i = Len(tmpVal) To 1 Step -1 ' 各ﾋﾞｯﾄの変換
                '    KeyVal = KeyVal & Hex(Asc(Mid(tmpVal, i, 1))) ' Char ごとに値を作成
                'Next
                'KeyVal = VB6.Format("&h" & KeyVal) ' Double Word を String に変換
                KeyVal = BitConverter.ToInt32(Encoding.Default.GetBytes(tmpVal.ToString().Substring(0, 4)), 0).ToString()
            Case Else
                KeyVal = ""
        End Select

        GetKeyValue = True ' 正常終了
        RegCloseKey(hKey) ' ﾚｼﾞｽﾄﾘ ｷｰをｸﾛｰｽﾞ
        Exit Function ' 終了

GetKeyError: ' ｴﾗｰ発生後の後始末...
        KeyVal = "" ' 戻り値の値を空文字列に設定
        GetKeyValue = False ' 異常終了
        RegCloseKey(hKey) ' ﾚｼﾞｽﾄﾘ ｷｰをｸﾛｰｽﾞ
    End Function

End Class