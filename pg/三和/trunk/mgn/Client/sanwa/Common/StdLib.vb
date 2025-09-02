Option Strict Off
Option Explicit On

Imports System.Runtime.InteropServices

''' <summary>
''' Ver.1.00           '2002.04.17
''' Ver.1.01           '2002.08.20     CurrentObjCaption 修正
''' Ver.1.02           '2002.09.19     SpcToNull NULLのチェックを追加
''' Ver.1.03           '2002.10.16     IsCheckNullでNULLの場合、Trim$がｴﾗｰになるのでTrimにする
''' Ver.1.04           '2002.10.16     CurrentObjCaption レポートでも使用するのでObject型に変更
''' Ver.1.05           '2003.05.16     OpenChildForm CloseMeを追加 ×使えない
''' Ver.1.06           '2003.09.30     GetDecimalFormat$を追加
''' Ver.1.07           '2003.10.03     CutTextを追加
'''
'''
'''
''' Ver.1.11           '2012.07.25     AnsiLeftBがWin7だと泣き分かれを起こすので修正
''' Ver.1.12           '2012/10/09     AnsiLeftBの修正がおかしいので修正
''' </summary>

Public Module StdLib

    ' マウスカーソルの変更回数カウント
    Public HourGlassSwNest As Integer

    ' メッセージボックス定数
    Public Const MB_ICONMASK As Integer = &HF0
    Public Const MB_OK As Integer = &H0
    Public Const MB_ICONHAND As Integer = &H10
    Public Const MB_ICONQUESTION As Integer = &H20
    Public Const MB_ICONEXCLAMATION As Integer = &H30
    Public Const MB_ICONASTERISK As Integer = &H40
    Public Const MB_ICONINFORMATION As Integer = MB_ICONASTERISK

    ' メッセージビープ関数の宣言
    <DllImport("user32.dll", CharSet:=CharSet.Auto)>
    Public Function MessageBeep(ByVal uType As UInteger) As Boolean
    End Function

    ' フォームモードの列挙型
    Public Enum eFormMode
        FormClose = -1
        FormHide = 0
        FormShow = 1
    End Enum

    ' ビープ音を鳴らすサブルーチン
    Public Sub BeepA()
        MessageBeep(MB_ICONQUESTION)
    End Sub

    ' メッセージビープ音を鳴らすサブルーチン
    Public Sub MSGbeep(Optional ByVal MBpara As Integer = MB_OK)
        MessageBeep(MBpara And MB_ICONMASK)
    End Sub

    ' ビープ音を鳴らし、メッセージボックスを表示する関数
    Public Function BeepMsgBox(ByVal Prompt As String, ByVal Buttons As Integer, ByVal Title As String) As MsgBoxResult
        MSGbeep(Buttons)
        Return MsgBox(Prompt, Buttons, Title)
    End Function

    ' 現在のオブジェクトの標題を取得する関数
    Public Function CurrentObjCaption() As String
        Dim obj As Object = Nothing

        Try
            obj = Form.ActiveForm
            If obj IsNot Nothing Then
                Do
                    If obj.MDIChild Then
                        obj = obj.Parent
                    Else
                        Exit Do
                    End If
                Loop
                Return obj.Text
            Else
                Return My.Application.Info.Title
            End If
        Catch ex As Exception
            Return My.Application.Info.Title
        Finally
            If obj IsNot Nothing Then
                Marshal.ReleaseComObject(obj)
            End If
        End Try
    End Function

    ' メッセージボックスのタイトルを取得する関数
    Private Function MsgTitle(ByVal Title As String) As String
        If String.IsNullOrEmpty(Title) Then
            Return CurrentObjCaption()
        Else
            Return Title
        End If
    End Function

    ' 警告メッセージを表示するサブルーチン
    Public Sub CriticalAlarm(ByVal msg As String, Optional ByVal Title As String = "")
        BeepMsgBox(msg, vbCritical + vbOKOnly, MsgTitle(Title))
    End Sub

    ' 確認メッセージを表示する関数
    Public Function VerifyYesNo(ByVal msg As String, ByVal DefaultBT As MsgBoxStyle, Optional ByVal Title As String = "") As MsgBoxResult
        Return Question(msg & vbCrLf & vbCrLf & "よろしいですか？", DefaultBT, Title)
    End Function

    ' 質問メッセージを表示する関数
    Public Function Question(ByVal msg As String, ByVal DefaultBT As MsgBoxStyle, Optional ByVal Title As String = "") As MsgBoxResult
        Dim BT As Integer
        Select Case DefaultBT
            Case vbYes
                BT = vbDefaultButton1
            Case Else
                BT = vbDefaultButton2
        End Select
        MSGbeep(MB_OK)
        Return MsgBox(msg, vbQuestion + vbYesNo + BT, MsgTitle(Title))
    End Function

    ' 情報メッセージを表示する関数
    Public Function Inform(ByVal msg As String, Optional ByVal Title As String = "") As MsgBoxResult
        MSGbeep(MB_OK)
        Return MsgBox(msg, vbInformation + vbOKOnly, MsgTitle(Title))
    End Function

    ' マウスカーソルを砂時計に変更するサブルーチン
    Public Sub HourGlass(ByVal sw As Boolean)
        If sw Then
            Cursor.Current = Cursors.WaitCursor
            HourGlassSwNest += 1
        Else
            If HourGlassSwNest > 0 Then
                HourGlassSwNest -= 1
            End If
            If HourGlassSwNest = 0 Then
                Cursor.Current = Cursors.Default
            End If
        End If
    End Sub

    ' フルパスからファイル名を除いたパスを取得する関数
    Public Function CutOutPath(ByVal tmp As String) As String
        Dim p1 As Integer = 0
        Dim p2 As Integer = 0
        Do
            p2 = tmp.IndexOf("\"c, p1 + 1)
            If p2 = -1 Then Exit Do
            p1 = p2
        Loop
        If p1 = 0 Then
            Return String.Empty
        Else
            Return tmp.Substring(0, p1)
        End If
    End Function

    ' アプリケーションのパスを取得する関数
    Public Function AppPath(Optional ByVal fileName As String = "", Optional ByVal AttachedFilename As Boolean = True) As String
        If Application.ExecutablePath.EndsWith("\") Then
            If AttachedFilename Then
                Return Application.ExecutablePath & fileName
            Else
                Return Application.ExecutablePath
            End If
        Else
            If AttachedFilename Then
                Return Application.ExecutablePath & "\" & fileName
            Else
                Return Application.ExecutablePath & "\"
            End If
        End If
    End Function

    ' ファイル名を分離するサブルーチン
    Public Sub SeparateFilename(ByVal fileName As String, ByRef filePath As String, ByRef BaseName As String, ByRef Extension As String)
        Dim p1 As Integer = fileName.LastIndexOf("\"c)
        If p1 = -1 Then
            filePath = "."
        Else
            filePath = fileName.Substring(0, p1)
        End If
        Dim strTmp As String = fileName.Substring(p1 + 1)
        Dim p2 As Integer = strTmp.LastIndexOf("."c)
        If p2 = -1 Then
            BaseName = strTmp
            Extension = String.Empty
        Else
            BaseName = strTmp.Substring(0, p2)
            Extension = strTmp.Substring(p2 + 1)
        End If
    End Sub

    ' 任意の数のオブジェクトを配列に格納する関数
    Public Function SetObjectArray(ParamArray Objs() As Object) As Object()
        Return Objs
    End Function

    ' Null 値をゼロに変換する関数
    Public Function NullToZero(ByVal Value As Object, Optional ByVal ChangeValue As Object = 0) As Object
        If Value Is Nothing OrElse Value.ToString() = String.Empty Then
            Return ChangeValue
        Else
            Return Value
        End If
    End Function

    ' スペースを Null に変換する関数
    Public Function SpcToNull(ByVal Value As Object, Optional ByVal ChangeValue As Object = Nothing) As Object
        If Value Is Nothing OrElse Value.ToString().Trim() = String.Empty Then
            Return ChangeValue
        Else
            Return Value.ToString().Trim()
        End If
    End Function

    ' テキストを選択するサブルーチン
    Public Sub SelText(ByVal SelectControl As Control)
        On Error Resume Next
        Dim txtBox As TextBox = TryCast(SelectControl, TextBox)
        If txtBox IsNot Nothing Then
            txtBox.SelectAll()
        End If
        On Error GoTo 0
    End Sub

    ' ANSI 版 RightB 関数
    Public Function AnsiRightB(ByVal Str As String, ByVal Length As Integer) As String
        Return System.Text.Encoding.Default.GetString(System.Text.Encoding.Default.GetBytes(Str).Skip(System.Text.Encoding.Default.GetBytes(Str).Length - Length).ToArray())
    End Function

    ' ANSI 版 LeftB 関数
    Public Function AnsiLeftB(ByVal Str As String, ByVal Length As Integer) As String
        Dim bytes As Byte() = System.Text.Encoding.Default.GetBytes(Str)
        If bytes.Length <= Length Then
            Return Str
        Else
            Return System.Text.Encoding.Default.GetString(bytes.Take(Length).ToArray())
        End If
    End Function

    ' ANSI 版 LenB 関数
    Public Function AnsiLenB(ByVal Str As String) As Integer
        Return System.Text.Encoding.Default.GetBytes(Str).Length
    End Function

    ' ANSI 版 MidB 関数
    Public Function AnsiMidB(ByVal Str As String, ByVal Start As Integer, ByVal Length As Integer) As String
        Dim bytes As Byte() = System.Text.Encoding.Default.GetBytes(Str)
        If Start > bytes.Length Then
            Return String.Empty
        Else
            Return System.Text.Encoding.Default.GetString(bytes.Skip(Start - 1).Take(Length).ToArray())
        End If
    End Function

End Module
