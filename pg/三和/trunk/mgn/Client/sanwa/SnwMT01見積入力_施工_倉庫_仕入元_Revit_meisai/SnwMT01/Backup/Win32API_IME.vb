Option Strict Off
Option Explicit On
Module Win32API_IME
	'Ver.1.00           '2003.08.01
	
	' ウィンドウに関連付けされた入力コンテキストを
	' 取得する関数の宣言
	Declare Function ImmGetContext Lib "Imm32.dll" (ByVal hwnd As Integer) As Integer
	' ウィンドウに関連付けされた入力コンテキストを
	' 開放する関数の宣言
	Declare Function ImmReleaseContext Lib "Imm32.dll" (ByVal hwnd As Integer, ByVal himc As Integer) As Integer
	' IMEのオープン状態を取得する関数の宣言
	Declare Function ImmGetOpenStatus Lib "Imm32.dll" (ByVal himc As Integer) As Integer
	' IMEのオープン状態を設定する関数の宣言
	Declare Function ImmSetOpenStatus Lib "Imm32.dll" (ByVal himc As Integer, ByVal fOpen As Integer) As Integer
	' 現在の変換状態を設定する関数の宣言
	Declare Function ImmSetConversionStatus Lib "Imm32.dll" (ByVal himc As Integer, ByVal fdwConversion As Integer, ByVal fdwSentence As Integer) As Integer
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
	' 変換モードを示す定数の宣言
	Public Const IME_SMODE_PHRASEPREDICT As Integer = &H8
	
	Public Enum ConversionMODE
		MODEOFF = 0 'OFF
		MODEON = 1 'ON
		ZENHIRA = 2 'ひらがな
		ZENKANA = 3 '全角カタカナ
		HANKANA = 4 '半角カタカナ
		ZENEISU = 5 '全角英数字
		HANEISU = 6 '半角英数字
	End Enum
	
	Dim lngInputContextHandle As Integer
	Dim lngWin32apiResultCode As Integer
	Dim lngIMEConversionStatus As Integer
	Dim lngIMESentenceStatus As Integer
	
	Public Function ImmOpenModeSet(ByRef hwnd As Integer, Optional ByRef ConversionStatus As ConversionMODE = ConversionMODE.MODEOFF) As Object
		' ウィンドウに関連付けされた入力コンテキストを取得
		lngInputContextHandle = ImmGetContext(hwnd)
		' 入力コンテキストを取得できたときは
		If lngInputContextHandle <> 0 Then
			
			' IMEのオープン状態でないときは
			If ImmGetOpenStatus(lngInputContextHandle) = False Then
				
				Select Case ConversionStatus
					Case ConversionMODE.MODEOFF, ConversionMODE.MODEON
					Case Else
						' IMEをオープン
						lngWin32apiResultCode = ImmSetOpenStatus(lngInputContextHandle, True)
				End Select
			End If
			
			' 初期入力モードを指定
			Select Case ConversionStatus
				Case ConversionMODE.MODEOFF
					' IMEをクローズ
					lngWin32apiResultCode = ImmSetOpenStatus(lngInputContextHandle, False)
				Case ConversionMODE.MODEON
					' IMEをオープン
					lngWin32apiResultCode = ImmSetOpenStatus(lngInputContextHandle, True)
				Case ConversionMODE.ZENHIRA
					'1:ひらがな
					lngIMEConversionStatus = IME_CMODE_FULLSHAPE Or IME_CMODE_JAPANESE
				Case ConversionMODE.ZENKANA
					'2:全角カタカナ
					lngIMEConversionStatus = IME_CMODE_FULLSHAPE Or IME_CMODE_LANGUAGE
				Case ConversionMODE.HANKANA
					'3:半角カタカナ
					lngIMEConversionStatus = IME_CMODE_LANGUAGE
				Case ConversionMODE.ZENEISU
					'4:全角英数字
					lngIMEConversionStatus = IME_CMODE_FULLSHAPE
				Case ConversionMODE.HANEISU
					'5:半角英数字
					lngIMEConversionStatus = IME_CMODE_ALPHANUMERIC
			End Select
			
			' 初期変換モードを指定
			lngIMESentenceStatus = IME_SMODE_PHRASEPREDICT
			' IMEの初期方式を設定
			lngWin32apiResultCode = ImmSetConversionStatus(lngInputContextHandle, lngIMEConversionStatus, lngIMESentenceStatus)
		End If
	End Function
End Module