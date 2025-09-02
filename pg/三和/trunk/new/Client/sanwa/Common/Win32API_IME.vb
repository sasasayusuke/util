Option Strict On
Option Explicit On

Imports System.Runtime.InteropServices

''' <summary>
''' Ver.1.00           '2003.08.01
''' </summary>
Public Module Win32API_IME

    ' ウィンドウに関連付けされた入力コンテキストを取得する関数の宣言
    <DllImport("Imm32.dll")>
    Public Function ImmGetContext(ByVal hwnd As IntPtr) As IntPtr
    End Function

    ' ウィンドウに関連付けされた入力コンテキストを開放する関数の宣言
    <DllImport("Imm32.dll")>
    Public Function ImmReleaseContext(ByVal hwnd As IntPtr, ByVal himc As IntPtr) As Boolean
    End Function

    ' IMEのオープン状態を取得する関数の宣言
    <DllImport("Imm32.dll")>
    Public Function ImmGetOpenStatus(ByVal himc As IntPtr) As Boolean
    End Function

    ' IMEのオープン状態を設定する関数の宣言
    <DllImport("Imm32.dll")>
    Public Function ImmSetOpenStatus(ByVal himc As IntPtr, ByVal fOpen As Boolean) As Boolean
    End Function

    ' 現在の変換状態を設定する関数の宣言
    <DllImport("Imm32.dll")>
    Public Function ImmSetConversionStatus(ByVal himc As IntPtr, ByVal fdwConversion As Integer, ByVal fdwSentence As Integer) As Boolean
    End Function

    ' 入力モードを示す定数の宣言
    Public Const IME_CMODE_ALPHANUMERIC As Integer = &H0
    Public Const IME_CMODE_NATIVE As Integer = &H1
    Public Const IME_CMODE_CHINESE As Integer = IME_CMODE_NATIVE
    Public Const IME_CMODE_HANGEUL As Integer = IME_CMODE_NATIVE
    Public Const IME_CMODE_JAPANESE As Integer = IME_CMODE_NATIVE
    Public Const IME_CMODE_KATAKANA As Integer = &H2
    Public Const IME_CMODE_LANGUAGE As Integer = &H3
    Public Const IME_CMODE_FULLSHAPE As Integer = &H8
    Public Const IME_CMODE_NOCONVERSION As Integer = &H100
    Public Const IME_SMODE_PHRASEPREDICT As Integer = &H8

    Public Enum ConversionMODE
        MODEOFF = 0 ' OFF
        MODEON = 1 ' ON
        ZENHIRA = 2 ' ひらがな
        ZENKANA = 3 ' 全角カタカナ
        HANKANA = 4 ' 半角カタカナ
        ZENEISU = 5 ' 全角英数字
        HANEISU = 6 ' 半角英数字
    End Enum

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="hwnd"></param>
    ''' <param name="ConversionStatus"></param>
    Public Sub ImmOpenModeSet(hwnd As IntPtr, Optional ConversionStatus As ConversionMODE = ConversionMODE.MODEOFF)
        ' ウィンドウに関連付けされた入力コンテキストを取得
        Dim lngInputContextHandle As IntPtr = ImmGetContext(hwnd)

        ' 入力コンテキストを取得できたときは
        If lngInputContextHandle <> IntPtr.Zero Then
            ' IMEのオープン状態でないときは
            If Not ImmGetOpenStatus(lngInputContextHandle) Then
                Select Case ConversionStatus
                    Case ConversionMODE.MODEOFF, ConversionMODE.MODEON
                    Case Else
                        ' IMEをオープン
                        ImmSetOpenStatus(lngInputContextHandle, True)
                End Select
            End If

            ' 初期入力モードを指定
            Dim lngIMEConversionStatus As Integer = 0
            Select Case ConversionStatus
                Case ConversionMODE.MODEOFF
                    ' IMEをクローズ
                    ImmSetOpenStatus(lngInputContextHandle, False)
                Case ConversionMODE.MODEON
                    ' IMEをオープン
                    ImmSetOpenStatus(lngInputContextHandle, True)
                Case ConversionMODE.ZENHIRA
                    ' ひらがな
                    lngIMEConversionStatus = IME_CMODE_FULLSHAPE Or IME_CMODE_JAPANESE
                Case ConversionMODE.ZENKANA
                    ' 全角カタカナ
                    lngIMEConversionStatus = IME_CMODE_FULLSHAPE Or IME_CMODE_LANGUAGE
                Case ConversionMODE.HANKANA
                    ' 半角カタカナ
                    lngIMEConversionStatus = IME_CMODE_LANGUAGE
                Case ConversionMODE.ZENEISU
                    ' 全角英数字
                    lngIMEConversionStatus = IME_CMODE_FULLSHAPE
                Case ConversionMODE.HANEISU
                    ' 半角英数字
                    lngIMEConversionStatus = IME_CMODE_ALPHANUMERIC
            End Select

            ' 初期変換モードを指定
            Dim lngIMESentenceStatus As Integer = IME_SMODE_PHRASEPREDICT

            ' IMEの初期方式を設定
            ImmSetConversionStatus(lngInputContextHandle, lngIMEConversionStatus, lngIMESentenceStatus)

            ' 入力コンテキストを開放
            ImmReleaseContext(hwnd, lngInputContextHandle)
        End If
    End Sub

End Module
