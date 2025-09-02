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
''' Ver.1.11           '2012.07.25     AnsiLeftBがWin7だと泣き分かれを起こすので修正
''' Ver.1.12           '2012/10/09     AnsiLeftBの修正がおかしいので修正
''' </summary>

Public Module StdLib

    ''' <summary>
    ''' マウスカーソルの変更回数カウント
    ''' </summary>
    Public HourGlassSwNest As Integer

    ''' <summary>
    ''' メッセージボックス定数
    ''' </summary>
    Public Const MB_ICONMASK As Integer = &HF0
    Public Const MB_OK As Integer = &H0
    Public Const MB_ICONHAND As Integer = &H10
    Public Const MB_ICONQUESTION As Integer = &H20
    Public Const MB_ICONEXCLAMATION As Integer = &H30
    Public Const MB_ICONASTERISK As Integer = &H40
    Public Const MB_ICONINFORMATION As Integer = MB_ICONASTERISK

    ''' <summary>
    ''' メッセージビープ関数の宣言
    ''' </summary>
    ''' <param name="uType"></param>
    ''' <returns></returns>
    <DllImport("user32.dll", CharSet:=CharSet.Auto)>
    Public Function MessageBeep(ByVal uType As Integer) As Boolean
    End Function

    ''' <summary>
    ''' フォームモードの列挙型
    ''' </summary>
    Public Enum eFormMode
        FormClose = -1
        FormHide = 0
        FormShow = 1
    End Enum

    ''' <summary>
    ''' ビープ音を鳴らすサブルーチン
    ''' </summary>
    Public Sub BeepA()
        MessageBeep(MB_ICONQUESTION)
    End Sub

    ''' <summary>
    ''' メッセージビープ音を鳴らすサブルーチン
    ''' </summary>
    Public Sub MSGbeep(Optional ByVal MBpara As Integer = MB_OK)
        MessageBeep(MBpara And MB_ICONMASK)
    End Sub

    ''' <summary>
    ''' ビープ音を鳴らし、メッセージボックスを表示する関数
    ''' </summary>
    Public Function BeepMsgBox(ByVal Prompt As String, ByVal Buttons As Integer, ByVal Title As String) As MsgBoxResult
        MSGbeep(Buttons)
        Return MsgBox(Prompt, Buttons, Title)
    End Function

    ''' <summary>
    ''' 現在のオブジェクトの標題を取得する関数
    ''' </summary>
    Public Function CurrentObjCaption() As String
        Dim obj As Object = Nothing

        Try
            obj = Form.ActiveForm
            If obj IsNot Nothing Then
                Do
                    If obj.IsMdiChild Then
                        obj = obj.MdiParent
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
                obj = Nothing
            End If
        End Try
    End Function

    ''' <summary>
    ''' メッセージボックスのタイトルを取得する関数
    ''' </summary>
    Private Function MsgTitle(ByVal Title As String) As String
        If String.IsNullOrEmpty(Title) Then
            Return CurrentObjCaption()
        Else
            Return Title
        End If
    End Function

    ''' <summary>
    ''' 項目ﾁｪｯｸｴﾗｰﾒｯｾｰｼﾞ（三角！)
    ''' </summary>
    Public Sub CriticalAlarm(ByVal msg As String, Optional ByVal Title As String = "")
        BeepMsgBox(msg, vbCritical + vbOKOnly, MsgTitle(Title))
    End Sub

    ''' <summary>
    ''' 項目チェックエラーメッセージ（三角！)
    ''' </summary>
    ''' <param name="msg"></param>
    ''' <param name="Title"></param>
    Sub CheckAlarm(msg As String, Optional Title As String = "")
        Beep()
        MessageBox.Show(msg, MsgTitle(Title), MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    End Sub

    ''' <summary>
    ''' 確認ﾒｯｾｰｼﾞ
    ''' </summary>
    Public Function VerifyYesNo(ByVal msg As String, ByVal DefaultBT As MsgBoxStyle, Optional ByVal Title As String = "") As MsgBoxResult
        Return Question(msg & vbCrLf & vbCrLf & "よろしいですか？", DefaultBT, Title)
    End Function

    ''' <summary>
    ''' 確認メッセージ（吹き出し？）
    ''' </summary>
    ''' <param name="msg"></param>
    ''' <param name="Title"></param>
    ''' <returns></returns>
    Function YesNo(msg As String, Optional Title As String = "") As DialogResult
        Return VerifyYesNo(msg, vbYes, Title)
    End Function

    ''' <summary>
    ''' 確認メッセージ（吹き出し？）
    ''' </summary>
    ''' <param name="msg"></param>
    ''' <param name="Title"></param>
    ''' <returns></returns>
    Function NoYes(msg As String, Optional Title As String = "") As DialogResult
        Return VerifyYesNo(msg, vbNo, Title)
    End Function

    ''' <summary>
    ''' 確認ﾒｯｾｰｼﾞ（吹き出し？）
    ''' </summary>
	Public Function Question(ByVal msg As String, ByVal DefaultBT As MsgBoxStyle, Optional ByVal Title As String = "") As MsgBoxResult
	    Dim defaultButton As MessageBoxDefaultButton = MessageBoxDefaultButton.Button2
	    If DefaultBT = vbYes Then
	        defaultButton = MessageBoxDefaultButton.Button1
	    End If

	    MSGbeep(MB_OK)
	    Dim result As DialogResult = MessageBox.Show(msg, MsgTitle(Title), MessageBoxButtons.YesNo, MessageBoxIcon.Question, defaultButton)

	    Return ConvertDialogResultToMsgBoxResult(result)
	End Function

    ''' <summary>
    ''' 情報メッセージを表示する関数
    ''' </summary>
	Public Function Inform(ByVal msg As String, Optional ByVal Title As String = "") As MsgBoxResult
	    MSGbeep(MB_OK)
	    MessageBox.Show(msg, MsgTitle(Title), MessageBoxButtons.OK, MessageBoxIcon.Information)
	    Return MsgBoxResult.Ok ' OKボタンしかないので固定
	End Function

	''' <summary>
	''' DialogResult を MsgBoxResult に変換
	''' </summary>
	Private Function ConvertDialogResultToMsgBoxResult(result As DialogResult) As MsgBoxResult
	    Select Case result
	        Case DialogResult.Yes
	            Return MsgBoxResult.Yes
	        Case DialogResult.No
	            Return MsgBoxResult.No
	        Case DialogResult.Ok
	            Return MsgBoxResult.Ok
	        Case DialogResult.Cancel
	            Return MsgBoxResult.Cancel
	        Case DialogResult.Abort
	            Return MsgBoxResult.Abort
	        Case DialogResult.Retry
	            Return MsgBoxResult.Retry
	        Case DialogResult.Ignore
	            Return MsgBoxResult.Ignore
            Case Else
                Return MsgBoxResult.No ' デフォルトは No
	    End Select
	End Function

    ''' <summary>
    ''' マウスカーソルを砂時計に変更するサブルーチン
    ''' </summary>
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

    ''' <summary>
    ''' フルパスからファイル名を除いたパスを取得する関数
    ''' </summary>
    Public Function CutOutPath(ByVal tmp As String) As String
        Dim p1 As Integer = 0
        Dim p2 As Integer
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

    ''' <summary>
    ''' アプリケーションのパスを取得する関数
    ''' </summary>
    Public Function AppPath(Optional ByVal fileName As String = "", Optional ByVal AttachedFilename As Boolean = True) As String
        If Application.StartupPath.EndsWith("\") Then
            If AttachedFilename Then
                Return Application.StartupPath & fileName
            Else
                Return Application.StartupPath
            End If
        Else
            If AttachedFilename Then
                Return Application.StartupPath & "\" & fileName
            Else
                Return Application.StartupPath & "\"
            End If
        End If
    End Function

    ''' <summary>
    ''' ファイル名を分離するサブルーチン
    ''' </summary>
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

    ''' <summary>
    ''' 任意の数のオブジェクトを配列に格納する関数
    ''' </summary>
    Public Function SetObjectArray(ParamArray Objs() As Object) As Object()
        Return Objs
    End Function

    ''' <summary>
    ''' Null 値をゼロに変換する関数
    ''' </summary>
    Public Function NullToZero(ByVal Value As Object, Optional ByVal ChangeValue As Object = 0) As Object

        ' DBNullチェック
        If IsDBNull(Value) Then
            Return ChangeValue
        End If
        ' 空文字列チェック
        If Value Is Nothing OrElse TypeOf Value Is String AndAlso DirectCast(Value, String) = String.Empty Then
            Return ChangeValue
        End If
        ' 型ごとの処理
        Select Case Type.GetTypeCode(Value.GetType())
            Case TypeCode.String
                Return Value.ToString().Trim()
            Case Else
                Return Value
        End Select

    End Function

    ''' <summary>
    ''' スペースを Null に変換する関数
    ''' </summary>
    Public Function SpcToNull(ByVal Value As Object, Optional ByVal ChangeValue As Object = Nothing) As Object
        If Value Is Nothing OrElse Value.ToString().Trim() = String.Empty Then
            Return ChangeValue
        Else
            Return Value.ToString().Trim()
        End If
    End Function

    ''' <summary>
    ''' テキストを選択するサブルーチン
    ''' </summary>
    Public Sub SelText(ByVal SelectControl As Control)
        On Error Resume Next
        Dim txtBox As TextBox = TryCast(SelectControl, TextBox)
        If txtBox IsNot Nothing Then
            txtBox.SelectAll()
        End If
        On Error GoTo 0
    End Sub

    ''' <summary>
    ''' ANSI 版 RightB 関数
    ''' </summary>
    Public Function AnsiRightB(ByVal str As String, ByVal length As Integer) As String
        Dim byteArray As Byte() = System.Text.Encoding.Default.GetBytes(str)
        If length >= byteArray.Length Then
            Return str
        End If
        Dim resultBytes As Byte() = New Byte(length - 1) {}
        Array.Copy(byteArray, byteArray.Length - length, resultBytes, 0, length)
        Return System.Text.Encoding.Default.GetString(resultBytes)
    End Function

    ''' <summary>
    ''' ANSI 版 LeftB 関数
    ''' </summary>
    Public Function AnsiLeftB(ByVal str As String, ByVal length As Integer) As String
        Dim byteArray As Byte() = System.Text.Encoding.Default.GetBytes(str)
        If length >= byteArray.Length Then
            Return str
        End If
        Dim resultBytes As Byte() = New Byte(length - 1) {}
        Array.Copy(byteArray, resultBytes, length)
        Return System.Text.Encoding.Default.GetString(resultBytes)
    End Function

    ''' <summary>
    ''' ANSI 版 LenB 関数
    ''' </summary>
    Public Function AnsiLenB(ByVal Str As String) As Integer
        Return System.Text.Encoding.Default.GetBytes(Str).Length
    End Function

    ''' <summary>
    ''' ANSI 版 MidB 関数
    ''' </summary>
    Public Function AnsiMidB(ByVal str As String, ByVal start As Integer, Optional ByVal length As Integer = -1) As String
        Dim byteArray As Byte() = System.Text.Encoding.Default.GetBytes(str)
        If start > byteArray.Length Then
            Return ""
        End If
        If length = -1 OrElse start + length > byteArray.Length Then
            length = byteArray.Length - start + 1
        End If
        Dim resultBytes As Byte() = New Byte(length - 1) {}
        Array.Copy(byteArray, start - 1, resultBytes, 0, length)
        Return System.Text.Encoding.Default.GetString(resultBytes)
    End Function

    ''' <summary>
    ''' IsCheckNull
    ''' 指定文字列が ＮＵＬＬ／空白 かチェックする
    ''' </summary>
    ''' <param name="Chk_Str">チェックする文字列</param>
    ''' <returns>True  : ＮＵＬＬ／空白、False : ＮＵＬＬ／空白でない</returns>
    Public Function IsCheckNull(ByVal Chk_Str As Object) As Boolean
        If Chk_Str Is Nothing OrElse Trim(Chk_Str.ToString()) = String.Empty Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' IsCheckText
    ''' 指定テキストが ＮＵＬＬ／空白 かチェックする
    ''' </summary>
    ''' <param name="ctl">チェックするテキスト</param>
    ''' <returns>True  : ＮＵＬＬ／空白、False : ＮＵＬＬ／空白でない</returns>
    Public Function IsCheckText(ByVal ctl As Control) As Boolean
        On Error GoTo Check_Text_Err
        IsCheckText = False
        '--- 項目チェック
        If ctl.Text Is Nothing OrElse Trim(ctl.Text) = String.Empty Then
            Return False
        End If
        IsCheckText = True
        Exit Function
Check_Text_Err:
        MsgBox(Err.Number & " " & Err.Description)
    End Function

    ''' <summary>
    ''' 四捨五入ルーチン
    ''' Round(Num,Keta)
    ''' 少数点以下第２位で四捨五入して、少数点位下１桁の値にする場合
    ''' Round(四捨五入する値 , 1)
    ''' </summary>
    ''' <param name="Num">四捨五入する値</param>
    ''' <param name="Keta">四捨五入した結果の桁数</param>
    ''' <returns></returns>
    Public Function ISRound(ByVal Num As Decimal, ByVal Keta As Integer) As Decimal
        Return Math.Round(Num, Keta, MidpointRounding.AwayFromZero)
    End Function

    ''' <summary>
    ''' 切り上げルーチン
    ''' </summary>
    ''' <param name="Num">切り捨てする値</param>
    ''' <param name="Keta">切り捨てした結果の桁数</param>
    ''' <returns></returns>
    Public Function ISRoundUp(ByVal Num As Decimal, ByVal Keta As Integer) As Decimal
        Return Math.Ceiling(Num * 10 ^ Keta) / 10 ^ Keta
    End Function

    ''' <summary>
    ''' 切り捨てルーチン
    ''' </summary>
    ''' <param name="Num">切り捨てする値</param>
    ''' <param name="Keta">切り捨てした結果の桁数</param>
    ''' <returns></returns>
    Public Function ISRoundDown(ByVal Num As Decimal, ByVal Keta As Integer) As Decimal
        Return Math.Floor(Num * 10 ^ Keta) / 10 ^ Keta
    End Function

    ''' <summary>
    ''' 端数処理ルーチン
    ''' </summary>
    ''' <param name="kubun">端数区分</param>
    ''' <param name="Num">値</param>
    ''' <param name="Keta">結果の桁数</param>
    ''' <returns></returns>
    Public Function ISHasuu_rtn(ByVal kubun As Integer, ByVal Num As Decimal, ByVal Keta As Integer) As Decimal
        Select Case kubun
            Case 0
                Return ISRound(Num, Keta)
            Case 1
                Return ISRoundUp(Num, Keta)
            Case 2
                Return ISRoundDown(Num, Keta)
        End Select
        Return 0
    End Function

    ''' <summary>
    ''' 整数チェックルーチン
    ''' </summary>
    ''' <param name="code">調べたい文字列</param>
    ''' <returns>True (．－を含まない整数の場合)  ,  False（それ以外の場合）</returns>
    Public Function ISInt(ByVal code As Object) As Boolean
        If code Is Nothing Then Return False
        If Not IsNumeric(code) Then Return False

        Dim strCode As String = code.ToString()
        If strCode.Contains(".") OrElse strCode.Contains("-") OrElse strCode.Contains("+") OrElse strCode.Contains("\") OrElse strCode.Contains("&") Then
            Return False
        End If

        Return True
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Number"></param>
    ''' <param name="Place"></param>
    ''' <param name="DispZero"></param>
    ''' <param name="DispComma"></param>
    ''' <returns></returns>
    Public Function GetDecimalFormat(ByVal Number As Decimal, Optional ByVal Place As Integer = 2,
                                 Optional ByVal DispZero As Boolean = False, Optional ByVal DispComma As Boolean = True) As String
        If Number = ISRoundDown(Number, 0) Then
            If DispZero Then
                If DispComma Then
                    Return Format(Number, "#,##0")
                Else
                    Return Format(Number, "0")
                End If
            Else
                If DispComma Then
                    Return Format(Number, "#,###")
                Else
                    Return Format(Number, "#")
                End If
            End If
        Else
            If DispComma Then
                Return Format(Number, "#,##0." & New String("0"c, Place))
            Else
                Return Format(Number, "0." & New String("0"c, Place))
            End If
        End If
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Text"></param>
    ''' <param name="ByteLength"></param>
    ''' <returns></returns>
    Public Function CutText(ByVal Text As String, ByVal ByteLength As Integer) As String
        Dim i As Integer
        Dim BLengthCnt As Integer = 0
        Dim RetText As String = String.Empty

        For i = 0 To Text.Length - 1
            Dim strChar As String = Text.Substring(i, 1)
            If System.Text.Encoding.GetEncoding("Shift_JIS").GetByteCount(strChar) = 1 Then
                BLengthCnt += 1
            Else
                BLengthCnt += 2
            End If
            If BLengthCnt > ByteLength Then Exit For
            RetText &= strChar
        Next

        Return RetText
    End Function

End Module
