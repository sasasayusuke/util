Option Strict On
Option Explicit On

Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text

''' <summary>
''' Ver.1.00           '2002.04.17
''' Ver.1.01           '2003.09.26     フォルダ作成モジュール追加[MKFolder]
''' Ver.1.02           '2013.08.08     ファイル名に使えない文字を取り除くを追加[replaceNGchar]
''' Ver.1.03           '2017.02.09     OpenFileDialogでxlsxを開くように
''' Ver.1.04           '2019.08.03     OpenFileDialogでtxtを開くように
''' Ver.1.05           '2020.01.11     OpenFileDialogでtxtとcsvを開くように
''' </summary>
Public Module Win32API_FileIO

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
    Public Structure BROWSEINFO
        Public hwndOwner As IntPtr
        Public pidlRoot As IntPtr
        <MarshalAs(UnmanagedType.LPStr)>
        Public pszDisplayName As String
        <MarshalAs(UnmanagedType.LPWStr)>
        Public lpszTitle As String
        Public ulFlags As UInteger
        Public lpfn As IntPtr
        Public lParam As IntPtr
        Public iImage As Integer
    End Structure

    'pidIrootの設定値
    Public Const CSIDL_DESKTOP = &H0           '\仮想デスクトップ
    Public Const CSIDL_PROGRAMS = &H2          '\プログラムループ
    Public Const CSIDL_CONTROLS = &H3          '\コントロールパネル
    Public Const CSIDL_PRINTER = &H4           '\プリンタ
    Public Const CSIDL_PERSONAL = &H5          '\My Documents
    Public Const CSIDL_FAVORITES = &H6         '\お気に入り
    Public Const CSIDL_STARTUP = &H7           '\スタートアップ
    Public Const CSIDL_RECENT = &H8            '\最近使ったファイル
    Public Const CSIDL_SENDTO = &H9            '\SendToフォルダ
    Public Const CSIDL_BITBUCKET = &HA         '\ごみ箱
    Public Const CSIDL_STARTMENU = &HB         '\スタートメニュー
    Public Const CSIDL_DESKTOPDIRECTORY = &H10 'デスクトップフォルダ
    Public Const CSIDL_DRIVES = &H11           'マイコンピュータ
    Public Const CSIDL_NETWORK = &H12          'ネットワークコンピュータ
    Public Const CSIDL_NETHOOD = &H13          '\Windows\NetHood
    Public Const CSIDL_FONTS = &H14            '\Windows\Fonts
    Public Const CSIDL_TEMPLATES = &H15        '\Windows\ShellNew

    'ulflagsの設定値
    Public Const BIF_RETURNONLYFSDIRS As UInteger = &H1        '実存するディレクトリのみ
    Public Const BIF_DONTGOBELOWDOMAIN As UInteger = &H2       'ネットワークコンピュータ内のリソースを表示しない
    Public Const BIF_STATUSTEXT As UInteger = &H4              'ダイアログボックスにステータス表示領域を追加
    Public Const BIF_RETURNFSANCESTERS As UInteger = &H8       'ネットワークコンピュータ内のリソース名しか選択できない
    Public Const BIF_EDITBOX As UInteger = &H10                'ダイアログボックスにファイル名入力用テキストボックスを追加
    Public Const BIF_VALIDATE As UInteger = &H20               '無効なアイテム名が入力されたときBrowsCallBackProcを呼び出す
    Public Const BIF_BROWSEFORCOMPUTER As UInteger = &H1000    'ネットワークコンピュータ内のリソース名のみ
    Public Const BIF_BROWSEFORPRINTER As UInteger = &H2000     'ネットワークプリンタ内のリソース名しか選択できない
    Public Const BIF_BROWSEONCLUDEFILES As UInteger = &H4000   '全てのリソースを選択できる

    Public Const OFN_ALLOWMULTISELECT = &H200        'ファイル名リストボックスで複数選択を可能にする
    Public Const OFN_CREATEPROMPT = &H2000           '現在存在しないファイルを作成するかを確認する
    Public Const OFN_EXTENSIONDIFFERENT = &H400      'ファイル名の拡張子とIpstrDefExtで指定された拡張子が異なる
    Public Const OFN_FILEMUSTEXIST = &H1000          '既存のファイル名だけ入力できるようにする
    Public Const OFN_HIDEREADONLY = &H4              '「読み取り専用」チェックボックスを表示しない
    Public Const OFN_NOCHANGEDIR = &H8               'ダイアログボックスを開いたときに現在のディレクトリを表示する
    Public Const OFN_NOREADONLYRETURN = &H8000       '読み取り専用属性を持たず、読み取り専用フォルダにないファイルを取得する
    Public Const OFN_NOVALIDATE = &H100              '無効な文字を含むファイル名を指定出来るようにする
    Public Const OFN_OVERWRITEPROMPT = &H2           '「ファイル名をつけて保存」ダイアログで選択したファイルが存在する場合上書き確認する
    Public Const OFN_PUTHMUSTEXIST = &H800           '無効なパスを入力したときに警告メッセージを表示する
    Public Const OFN_SHAREWARE = &H4000              '共通違反エラーを無視する
    Public Const OFN_READONLY = &H1                  '「読み取り専用」チェックボックスをオンにする
    Public Const OFN_SHOWHELP = &H10                 'ダイアログボックスに「ヘルプ」ボタンを表示する
    Public Const OFN_EXPLORER = &H80000              'エクスプローラーに似たダイアログボックスにする
    Public Const OFN_NODEREFERTRNCELINKS = &H100000  'ショートカットを実行しない
    Public Const OFN_LONGNAMES = &H200000            '長いファイル名を使用する

    Public Const MAX_PATH As Integer = 260

    <DllImport("shell32.dll", CharSet:=CharSet.Unicode)>
    Private Function SHBrowseForFolder(ByRef lpbi As BROWSEINFO) As IntPtr
    End Function

    <DllImport("shell32.dll", CharSet:=CharSet.Unicode)>
    Private Function SHGetPathFromIDList(ByVal pidl As IntPtr, ByVal pszPath As StringBuilder) As Boolean
    End Function

    <DllImport("shell32.dll", CharSet:=CharSet.Unicode)>
    Private Function SHSimpleIDListFromPath(ByVal pszPath As String) As IntPtr
    End Function

    <DllImport("ole32.dll", CharSet:=CharSet.Unicode)>
    Private Sub CoTaskMemFree(ByVal pv As IntPtr)
    End Sub

    ' PathCompactPathEx API の宣言
    <DllImport("shlwapi.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
    Private Function PathCompactPathEx(pszOut As StringBuilder, pszSrc As String, cchMax As Integer, dwFlags As Integer) As Boolean
    End Function

    ''' <summary>
    ''' 「フォルダの参照」ダイアログを呼び出す
    ''' </summary>
    ''' <param name="hwnd">呼び出し元のウィンドウハンドル</param>
    ''' <param name="title">ダイアログに表示するメッセージ</param>
    ''' <param name="rootPath">ルートパス名  ””ならデスクトップ</param>
    ''' <returns>選択したフォルダのフルパス名 キャンセル時は””</returns>
    Public Function Y_GetFolder(hwnd As IntPtr, title As String, rootPath As String) As String
        Dim browseInfo As New BROWSEINFO()
        Dim displayName As New StringBuilder(MAX_PATH)
        Dim pathBuffer As New StringBuilder(MAX_PATH)

        ' RootPathの設定
        Dim pidlRoot As IntPtr
        If String.IsNullOrEmpty(rootPath) OrElse Not IO.Directory.Exists(rootPath) Then
            pidlRoot = IntPtr.Zero  ' デスクトップをルートに設定
        Else
            pidlRoot = SHSimpleIDListFromPath(rootPath)
        End If

        ' BROWSEINFOの設定
        browseInfo.hwndOwner = hwnd
        browseInfo.pidlRoot = pidlRoot
        browseInfo.pszDisplayName = displayName.ToString
        browseInfo.lpszTitle = title
        browseInfo.ulFlags = BIF_RETURNONLYFSDIRS

        ' フォルダ選択ダイアログを表示
        Dim pidl As IntPtr = SHBrowseForFolder(browseInfo)
        If pidl <> IntPtr.Zero Then
            If SHGetPathFromIDList(pidl, pathBuffer) Then
                CoTaskMemFree(pidl)
                Return pathBuffer.ToString().TrimEnd(ControlChars.NullChar)
            Else
                CoTaskMemFree(pidl)
                Return String.Empty
            End If
        Else
            Return String.Empty
        End If
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="hwnd"></param>
    ''' <param name="title"></param>
    ''' <param name="rootPath"></param>
    ''' <returns></returns>
    Public Function GetFolderDialog(ByVal hwnd As IntPtr, ByVal title As String, ByVal rootPath As String) As String
        Dim bi As New BROWSEINFO With {
            .hwndOwner = hwnd,
            .pidlRoot = If(String.IsNullOrEmpty(rootPath), IntPtr.Zero, SHSimpleIDListFromPath(rootPath)),
            .pszDisplayName = New String(" "c, MAX_PATH),
            .lpszTitle = title,
            .ulFlags = BIF_RETURNONLYFSDIRS,
            .lpfn = IntPtr.Zero,
            .lParam = IntPtr.Zero,
            .iImage = 0
        }

        Dim pidl As IntPtr = SHBrowseForFolder(bi)
        If pidl <> IntPtr.Zero Then
            Dim path As New StringBuilder(MAX_PATH)
            If SHGetPathFromIDList(pidl, path) Then
                CoTaskMemFree(pidl)
                Return path.ToString()
            End If
        End If
        Return String.Empty
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="hwnd"></param>
    ''' <param name="filter"></param>
    ''' <param name="defaultExt"></param>
    ''' <param name="title"></param>
    ''' <param name="filePath"></param>
    ''' <returns></returns>
    Public Function GetOpenFileDialog(ByVal hwnd As IntPtr, ByVal filter As String, ByVal defaultExt As String, ByVal title As String, ByRef filePath As String) As Boolean
        Dim openFileDialog As New OpenFileDialog With {
            .Filter = filter,
            .DefaultExt = defaultExt,
            .Title = title,
            .FileName = String.Empty
        }

        If openFileDialog.ShowDialog(New WindowWrapper(hwnd)) = DialogResult.OK Then
            filePath = openFileDialog.FileName
            Return True
        End If
        Return False
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="hwnd"></param>
    ''' <param name="filter"></param>
    ''' <param name="defaultExt"></param>
    ''' <param name="title"></param>
    ''' <param name="filePath"></param>
    ''' <returns></returns>
    Public Function GetSaveFileDialog(ByVal hwnd As IntPtr, ByVal filter As String, ByVal defaultExt As String, ByVal title As String, ByRef filePath As String) As Boolean
        Dim saveFileDialog As New SaveFileDialog With {
            .Filter = filter,
            .DefaultExt = defaultExt,
            .Title = title,
            .FileName = String.Empty
        }

        If saveFileDialog.ShowDialog(New WindowWrapper(hwnd)) = DialogResult.OK Then
            filePath = saveFileDialog.FileName
            Return True
        End If
        Return False
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sFileName"></param>
    ''' <param name="DefaultFileName"></param>
    ''' <param name="DefaultExt"></param>
    ''' <returns></returns>
    Public Function OpenSaveDialog(ByRef sFileName As String, DefaultFileName As String, Optional ByVal DefaultExt As String = "csv") As Boolean
        Dim FileFilter As String

        ' Define the filter based on the default extension
        Select Case DefaultExt.ToLower()
            Case "xls", "xlsx"
                FileFilter = "Excelファイル(*.xls;*.xlsx)|*.xls;*.xlsx"
            Case "bmp"
                FileFilter = "Windows Bitmap(*.bmp)|*.bmp"
            Case "jpg"
                FileFilter = "JPEG形式(*.jpg;*.jpeg)|*.jpg;*.jpeg"
            Case "csv"
                FileFilter = "CSVファイル(*.csv)|*.csv"
            Case "pdf"
                FileFilter = "PDF(*.pdf)|*.pdf"
            Case Else
                FileFilter = "すべてのファイル(*.*)|*.*"
        End Select
        FileFilter &= "|すべてのファイル(*.*)|*.*"

        ' FileName, FilePath, and FileExtension variables
        Dim FileName As String = String.Empty
        Dim FilePath As String = String.Empty
        Dim FileExtension As String = String.Empty

        ' Separate the filename into path, name, and extension parts
        SeparateFilename(sFileName, FilePath, FileName, FileExtension)

        ' Set the default file name if not provided
        If String.IsNullOrEmpty(FileName) Then
            FileName = DefaultFileName & "." & DefaultExt
        Else
            FileName = FileName & "." & FileExtension
        End If

        ' Create and configure the SaveFileDialog
        Dim saveDialog As New SaveFileDialog()
        With saveDialog
            .DefaultExt = DefaultExt
            .Title = "出力先ファイル"
            .Filter = FileFilter
            .FilterIndex = 1
            .InitialDirectory = FilePath
            .FileName = FileName
            .AddExtension = True
        End With

        ' Show the dialog and handle the result
        If saveDialog.ShowDialog() = DialogResult.OK Then
            sFileName = saveDialog.FileName
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sFileName"></param>
    ''' <param name="DefaultFileName"></param>
    ''' <param name="DefaultExt"></param>
    ''' <returns></returns>
    Public Function ShowOpenFileDialog(ByRef sFileName As String, DefaultFileName As String, Optional ByVal DefaultExt As String = "csv") As Boolean
        Dim openFileDialog As New OpenFileDialog()
        Dim fileFilter As String = ""

        ' フィルタの設定
        Select Case DefaultExt.ToLower()
            Case "xls", "xlsx"
                fileFilter = "Excelファイル(*.xls;*.xlsx)|*.xls;*.xlsx"
            Case "bmp"
                fileFilter = "Windows Bitmap(*.bmp)|*.bmp"
            Case "jpg"
                fileFilter = "JPEG形式(*.jpg;*.jpeg)|*.jpg;*.jpeg"
            Case "csv"
                fileFilter = "CSVファイル(*.csv)|*.csv"
            Case "txt"
                fileFilter = "テキストファイル(*.txt)|*.txt"
            Case "csv;txt"
                fileFilter = "テキストファイル(*.txt;*.csv)|*.txt;*.csv"
        End Select
        fileFilter &= "|すべてのファイル(*.*)|*.*"
        openFileDialog.Filter = fileFilter

        ' デフォルトの拡張子とファイル名の設定
        openFileDialog.DefaultExt = DefaultExt
        openFileDialog.FileName = DefaultFileName & "." & DefaultExt

        ' 初期ディレクトリの設定
        If Not String.IsNullOrEmpty(sFileName) Then
            openFileDialog.InitialDirectory = System.IO.Path.GetDirectoryName(sFileName)
        Else
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        End If

        ' ダイアログの表示
        If openFileDialog.ShowDialog() = DialogResult.OK Then
            sFileName = openFileDialog.FileName
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strPath">省略したいフルパス</param>
    ''' <param name="intMax">文字数</param>
    ''' <returns>戻り値 : 省略されたパスが返る</returns>
    Public Function CompactPathEx(ByVal strPath As String, ByVal intMax As Integer) As String

        If String.IsNullOrEmpty(strPath) Then Return String.Empty

        Dim strBuffer As New StringBuilder(intMax + 1)

        If PathCompactPathEx(strBuffer, strPath, intMax, 0) Then
            Return strBuffer.ToString()
        Else
            Return strPath
        End If
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strPath"></param>
    ''' <param name="lngCheck"></param>
    ''' <returns></returns>
    Public Function MKFolder(ByVal strPath As String, Optional ByVal lngCheck As Integer = 0) As Integer
        Dim intPathLength As Integer = strPath.Length
        Dim i As Integer
        Dim intEndNumber As Integer
        Dim strCharacter As Char
        Dim strFolderPath As String = ""

        ' 値の初期化
        MKFolder = 1 ' デフォルトの戻り値をエラーに設定

        Try
            ' UNC パスの場合 3 文字目から調べて次の \ の場所から始める
            If strPath.StartsWith("\\") Then
                intEndNumber = strPath.IndexOf("\", 3) + 1
            Else
                ' 通常パスの場合 4 文字目から調べる
                intEndNumber = 4
            End If

            ' strPath が 3 文字未満の時はモジュールを抜ける
            If intPathLength < 3 Then
                MessageBox.Show("有効なパスではありません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return 1
            End If

            ' strPath の最後の文字が "\" でないなら "\" を付け足す
            If Not strPath.EndsWith("\") Then
                strPath &= "\"
                intPathLength += 1
            End If

            ' フォルダの作成
            For i = intEndNumber To intPathLength
                strCharacter = strPath(i - 1)
                If strCharacter = "\"c Then
                    strFolderPath = strPath.Substring(0, intEndNumber - 1)
                    If Not Directory.Exists(strFolderPath) Then
                        ' フォルダ作成の確認
                        If lngCheck = 1 Then
                            lngCheck = 0
                            If MessageBox.Show("以下のフォルダは存在しません。" & vbCrLf & "作成します。", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
                                Return 1
                            End If
                        End If
                        ' フォルダ作成
                        Directory.CreateDirectory(strFolderPath)
                    End If
                End If
                intEndNumber += 1
            Next

            ' フォルダが作成されているか確認
            If Directory.Exists(strPath) Then
                MKFolder = 0 ' 正常終了
            Else
                MKFolder = 1 ' エラー
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return 1
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sourceStr"></param>
    ''' <param name="replaceChar"></param>
    ''' <returns></returns>
    Public Function ReplaceNGchar(ByVal sourceStr As String, Optional ByVal replaceChar As String = "") As String
        Dim tempStr As String

        tempStr = sourceStr
        tempStr = tempStr.Replace("\", replaceChar)
        tempStr = tempStr.Replace("/", replaceChar)
        tempStr = tempStr.Replace(":", replaceChar)
        tempStr = tempStr.Replace("*", replaceChar)
        tempStr = tempStr.Replace("?", replaceChar)
        tempStr = tempStr.Replace("""", replaceChar)
        tempStr = tempStr.Replace("<", replaceChar)
        tempStr = tempStr.Replace(">", replaceChar)
        tempStr = tempStr.Replace("|", replaceChar)
        tempStr = tempStr.Replace("[", replaceChar)
        tempStr = tempStr.Replace("]", replaceChar)

        Return tempStr
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    Private Class WindowWrapper
        Implements System.Windows.Forms.IWin32Window

        Private _hwnd As IntPtr

        Public Sub New(ByVal handle As IntPtr)
            _hwnd = handle
        End Sub

        Public ReadOnly Property Handle As IntPtr Implements System.Windows.Forms.IWin32Window.Handle
            Get
                Return _hwnd
            End Get
        End Property
    End Class

End Module
