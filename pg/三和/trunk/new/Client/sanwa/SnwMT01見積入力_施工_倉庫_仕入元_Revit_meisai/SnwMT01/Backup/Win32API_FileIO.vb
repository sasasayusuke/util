Option Strict Off
Option Explicit On
Module Win32API_FileIO
	'Ver.1.00           '2002.04.17
	'Ver.1.01           '2003.09.26     フォルダ作成モジュール追加[MKFolder]
	'Ver.1.02           '2013.08.08     ファイル名に使えない文字を取り除くを追加[replaceNGchar]
	'Ver.1.03           '2017.02.09     OpenFileDialogでxlsxを開くように
	'Ver.1.04           '2019.08.03     OpenFileDialogでtxtを開くように
	'Ver.1.05           '2020.01.11     OpenFileDialogでtxtとcsvを開くように
	
	
	'フォルダ選択
	'UPGRADE_WARNING: 構造体 BROWSEINFO に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
	Declare Function SHBrowseForFolder Lib "shell32" (ByRef lpbi As BROWSEINFO) As Integer
	
	'ItemIDLListのポインタからパス名を取得する
	Declare Function SHGetPathFromIDList Lib "shell32" (ByVal plDL As Integer, ByVal pszPath As String) As Integer
	
	'パス名からItemDListのポインタを獲得する
	Declare Function SHSimpleIDListfromPath Lib "shell32"  Alias "#162"(ByVal pszPath As String) As Integer
	
	'メモリ開放
	'UPGRADE_NOTE: pv は pv_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
	Declare Sub CoTaskMemFree Lib "ole32" (ByVal pv_Renamed As Integer)
	
	Structure BROWSEINFO
		Dim hwndOwner As Integer '呼び出し元ウィンドウハンドル
		Dim pidIRoot As Integer 'ルートフォルダのＩＤ
		Dim pszDisplayName As String '選択したフォルダの表示名(c:\msdev\bin なら bin)
		Dim IpszTitle As String '表示するメッセージ
		Dim ulFlags As Short 'フラグ
		Dim Ipfn As Integer 'Callbackprocを指定する。0でデフォルトを使う
		Dim IParam As Integer 'Callback関数に引数を渡すときに使用する
		Dim iImage As Short 'おそらく Shell32.DLLのアイコン番号が返ってくる
	End Structure
	
	'pidIrootの設定値
	Public Const CSIDL_DESKTOP As Integer = &H0 '\仮想デスクトップ
	Public Const CSIDL_PROGRAMS As Integer = &H2 '\プログラムループ
	Public Const CSIDL_CONTROLS As Integer = &H3 '\コントロールパネル
	Public Const CSIDL_PRINTER As Integer = &H4 '\プリンタ
	Public Const CSIDL_PERSONAL As Integer = &H5 '\My Documents
	Public Const CSIDL_FAVORITES As Integer = &H6 '\お気に入り
	Public Const CSIDL_STARTUP As Integer = &H7 '\スタートアップ
	Public Const CSIDL_RECENT As Integer = &H8 '\最近使ったファイル
	Public Const CSIDL_SENDTO As Integer = &H9 '\SendToフォルダ
	Public Const CSIDL_BITBUCKET As Integer = &HA '\ごみ箱
	Public Const CSIDL_STARTMENU As Integer = &HB '\スタートメニュー
	Public Const CSIDL_DESKTOPDIRECTORY As Integer = &H10 'デスクトップフォルダ
	Public Const CSIDL_DRIVES As Integer = &H11 'マイコンピュータ
	Public Const CSIDL_NETWORK As Integer = &H12 'ネットワークコンピュータ
	Public Const CSIDL_NETHOOD As Integer = &H13 '\Windows\NetHood
	Public Const CSIDL_FONTS As Integer = &H14 '\Windows\Fonts
	Public Const CSIDL_TEMPLATES As Integer = &H15 '\Windows\ShellNew
	
	'ulflagsの設定値
	Public Const BIF_RETURNONLYFSDIRS As Integer = &H1 '実存するディレクトリのみ
	Public Const BIF_DONTGOBELOWDOMAIN As Integer = &H2 'ネットワークコンピュータ内のリソースを表示しない
	Public Const BIF_STATUSTEXT As Integer = &H4 'ダイアログボックスにステータス表示領域を追加
	Public Const BIF_RETURNFSANCESTERS As Integer = &H8 'ネットワークコンピュータ内のリソース名しか選択できない
	Public Const BIF_EDITBOX As Integer = &H10 'ダイアログボックスにファイル名入力用テキストボックスを追加
	Public Const BIF_VALIDATE As Integer = &H20 '無効なアイテム名が入力されたときBrowsCallBackProcを呼び出す
	Public Const BIF_BROWSEFORCOMPUTER As Integer = &H1000 'ネットワークコンピュータ内のリソース名のみ
	Public Const BIF_BROWSEFORPRINTER As Integer = &H2000 'ネットワークプリンタ内のリソース名しか選択できない
	Public Const BIF_BROWSEONCLUDEFILES As Integer = &H4000 '全てのリソースを選択できる
	
	Public Structure OPENFILENAME
		Dim IStructSize As Integer 'この構造体の長さ
		Dim hwndOwner As Integer '呼び出し元ウィンドウハンドル
		Dim hInstance As Integer 'モジュールのインスタンスハンドル
		Dim Ipstrfilter As String 'フィルタ文字列
		Dim IpstrCustomFilter As String 'ユーザー定義のフィルタ文字列のペア
		Dim nMaxCustrFilter As Integer 'IpstrCustomFilterのバッファサイズ
		Dim nFilterIndex As Integer 'フィルタコンボボックスの初期インデックス値
		Dim Ipstrfile As String '選択されたファイル名のフルパス
		Dim nMaxFile As Integer 'IpstrFileのバッファサイズ
		Dim IpstrFileTitle As String '選択されたファイル名のタイトル
		Dim nMaxFileTitle As Integer 'IpstrFileTitleのバッファサイズ
		Dim IpstrInitialDir As String '初期フォルダ名
		Dim IpstrTitle As String 'ダイアログボックスのタイトル名
		Dim flags As Integer '以下のFlagsの値の組み合わせ
		Dim nFileOffset As Short 'IpstrFileの最後の￥までのオフセット値
		Dim nFileExtension As Short '拡張子までのオフセット値
		Dim IpstrDefExt As String 'ファイル名の入力時、拡張子が省略された時の拡張子
		Dim ICustrData As Integer 'ＯＳがIpfnHookで指定されたフック関数に渡すアプリ定義のデータ
		Dim IpfnHook As Integer 'ダイアログに送られるメッセージを処理するフック関数のポインタ
		Dim IpTemplateName As String
	End Structure
	
	'Flagsに設定する値
	Public Const OFN_ALLOWMULTISELECT As Integer = &H200 'ファイル名リストボックスで複数選択を可能にする
	Public Const OFN_CREATEPROMPT As Integer = &H2000 '現在存在しないファイルを作成するかを確認する
	Public Const OFN_EXTENSIONDIFFERENT As Integer = &H400 'ファイル名の拡張子とIpstrDefExtで指定された拡張子が異なる
	Public Const OFN_FILEMUSTEXIST As Integer = &H1000 '既存のファイル名だけ入力できるようにする
	Public Const OFN_HIDEREADONLY As Integer = &H4 '「読み取り専用」チェックボックスを表示しない
	Public Const OFN_NOCHANGEDIR As Integer = &H8 'ダイアログボックスを開いたときに現在のディレクトリを表示する
	Public Const OFN_NOREADONLYRETURN As Integer = &H8000 '読み取り専用属性を持たず、読み取り専用フォルダにないファイルを取得する
	Public Const OFN_NOVALIDATE As Integer = &H100 '無効な文字を含むファイル名を指定出来るようにする
	Public Const OFN_OVERWRITEPROMPT As Integer = &H2 '「ファイル名をつけて保存」ダイアログで選択したファイルが存在する場合上書き確認する
	Public Const OFN_PUTHMUSTEXIST As Integer = &H800 '無効なパスを入力したときに警告メッセージを表示する
	Public Const OFN_SHAREWARE As Integer = &H4000 '共通違反エラーを無視する
	Public Const OFN_READONLY As Integer = &H1 '「読み取り専用」チェックボックスをオンにする
	Public Const OFN_SHOWHELP As Integer = &H10 'ダイアログボックスに「ヘルプ」ボタンを表示する
	Public Const OFN_EXPLORER As Integer = &H80000 'エクスプローラーに似たダイアログボックスにする
	Public Const OFN_NODEREFERTRNCELINKS As Integer = &H100000 'ショートカットを実行しない
	Public Const OFN_LONGNAMES As Integer = &H200000 '長いファイル名を使用する
	
	
	'「名前をつけてファイルに保存」コモンダイアログを呼び出す。
	'UPGRADE_WARNING: 構造体 OPENFILENAME に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
	Declare Function GetSaveFileName Lib "comdlg32.dll"  Alias "GetSaveFileNameA"(ByRef pOpenfilename As OPENFILENAME) As Integer
	
	'UPGRADE_WARNING: 構造体 OPENFILENAME に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
	Declare Function GetOpenFileName Lib "comdlg32.dll"  Alias "GetOpenFileNameA"(ByRef pLoadFile As OPENFILENAME) As Integer
	
	Public Structure OpenFileName2
		Dim DefaultExt As String '拡張子をつけなかったときのデフォルト拡張子
		Dim DialogTitle As String 'タイトルバーに表示するタイトル名
		Dim FileName As String 'ダイアログを閉じた後、選択したファイルのフルパスが入る
		Dim FilePath As String '選択したファイルが含まれるパスの名前
		Dim FileTitle As String '選択したファイルのパスを含まない名前
		'UPGRADE_NOTE: Filter は Filter_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
		Dim Filter_Renamed As String 'フィルター
		Dim FilterIndex As Integer '複数フィルターを設定しているときの表示するフィルターのインデックス番号
		Dim flags As Integer 'ダイアログボックスの作成フラグ
		Dim InitDir As String '初期フォルダ名
		Dim MaxFileSize As Integer 'ファイル名の最大サイズを設定（1～32768 規定値256）
		Dim OKflg As Short '1:ファイルを選択した 0：選択をキャンセルした
		Dim InitFileName As String '初期ファイル名
	End Structure
	
	Public Const MAX_PATH As Short = 260
	
	'パスを文字数指定で省略する。元に戻す方法？が分からない。
	Public Declare Function PathCompactPathEx Lib "SHLWAPI.DLL"  Alias "PathCompactPathExA"(ByVal pszOut As String, ByVal pszSrc As String, ByVal cchMax As Integer, ByVal dwFlags As Integer) As Integer
	
	Function Y_GetFolder(ByRef Hwnd As Integer, ByRef Title As String, ByRef Rootpath As String) As String
		'***********************************************************
		'機能  ：「フォルダの参照」ダイアログを呼び出す
		'引数  ： hWnd = 呼び出し元のウィンドウハンドル
		'        Title = ダイアログに表示するメッセージ
		'        RootPath = ルートパス名  ””ならデスクトップ
		'戻り値：選択したフォルダのフルパス名 キャンセル時は””
		'***********************************************************
		
		Dim taginfo As BROWSEINFO
		Dim StrBuf As String
		Dim strbuf2 As String
		Dim pIDL As Integer
		Dim rIDL As Integer
		
		strbuf2 = New String(Chr(0), MAX_PATH)
		
		'UPGRADE_WARNING: Dir に新しい動作が指定されています。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
		If (Rootpath = vbNullString) Or (Dir(Rootpath, FileAttribute.Directory) = vbNullString) Then
			rIDL = CSIDL_DESKTOP
		Else
			rIDL = SHSimpleIDListfromPath(Rootpath)
		End If
		
		With taginfo
			.hwndOwner = Hwnd
			.ulFlags = BIF_RETURNONLYFSDIRS
			.IpszTitle = strbuf2
			.pidIRoot = rIDL
			.Ipfn = 0
			.IParam = 0
		End With
		
		'-----ダイアログ表示
		pIDL = SHBrowseForFolder(taginfo)
		If (pIDL > 0) Then
			StrBuf = New String(Chr(0), MAX_PATH) 'フォルダ名の獲得
			SHGetPathFromIDList(pIDL, StrBuf)
			
			CoTaskMemFree(pIDL) 'メモリの開放
			
			Y_GetFolder = UniAsczToStr(StrBuf) '結果表示
		Else
			Y_GetFolder = vbNullString
		End If
		
	End Function
	
	Public Function Y_GetOpenFileDialog(ByRef Hwnd As Integer, ByRef OpenInfo As OpenFileName2) As Boolean
		'***************************************************************
		'機能  ：「ファイルを開く」ダイアログを呼び出す
		'引数  ：Hwnd 呼び出し元のフォームオブジェクトのハンドル
		'       Openinfo = 「ファイルを開く」ダイアログの初期設定値
		'戻り値：True 「開く」ボタンが押された
		'        False 「キャンセル」ボタンが押された
		'***************************************************************
		
		Dim getfile As OPENFILENAME
		Dim FilterBuf As String
		Dim StrBuf As String
		Dim i As Integer
		Dim j As Integer
		Dim cnt As Short
		Dim filindex As Short
		Dim longret As Integer
		
		'-----初期値設定
		If (Left(OpenInfo.DefaultExt, 1) = ".") Then
			OpenInfo.DefaultExt = Mid(OpenInfo.DefaultExt, 2)
		End If
		
		If (OpenInfo.DialogTitle = vbNullString) Then
			OpenInfo.DialogTitle = "ファイルを開く"
		End If
		
		If (OpenInfo.MaxFileSize < 1) Or (OpenInfo.MaxFileSize > 32768) Then
			OpenInfo.MaxFileSize = MAX_PATH
		End If
		
		If (OpenInfo.FileTitle = vbNullString) Then
			OpenInfo.FileTitle = New String(Chr(0), OpenInfo.MaxFileSize)
		End If
		
		FilterBuf = OpenInfo.Filter_Renamed
		j = 1
		cnt = 1
		
		Do 
			i = InStr(j, FilterBuf, "|")
			If (i = 0) Then
				Exit Do
			End If
			
			Mid(FilterBuf, i, 1) = vbNullChar
			j = i + 1
			cnt = cnt + 1
		Loop 
		
		If (OpenInfo.FilterIndex < 1) Or (OpenInfo.FilterIndex > cnt) Then
			filindex = 0
		Else
			filindex = OpenInfo.FilterIndex
		End If
		
		''    StrBuf = String(OpenInfo.MaxFileSize, 0)
		'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
		'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
		StrBuf = OpenInfo.InitFileName & New String(Chr(0), OpenInfo.MaxFileSize - LenB(StrConv(OpenInfo.InitFileName, vbFromUnicode)))
		
		'-----コモンダイアログを呼び出す
		With getfile
			.IStructSize = Len(getfile)
			.hwndOwner = Hwnd
			.hInstance = 0 'App.hinstance
			.Ipstrfilter = FilterBuf
			.nMaxCustrFilter = 0
			.nFilterIndex = filindex
			.Ipstrfile = StrBuf
			.nMaxFile = OpenInfo.MaxFileSize
			.IpstrFileTitle = OpenInfo.FileTitle
			.nMaxFileTitle = Len(OpenInfo.FileTitle) + 1
			.IpstrInitialDir = OpenInfo.InitDir
			.IpstrTitle = OpenInfo.DialogTitle
			.flags = OpenInfo.flags
			.IpstrDefExt = OpenInfo.DefaultExt
		End With
		
		Y_GetOpenFileDialog = (GetOpenFileName(getfile) <> 0)
		
		'-----戻り値セット
		With OpenInfo
			.FileName = UniAsczToStr(getfile.Ipstrfile)
			'UPGRADE_ISSUE: 定数 vbUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			'UPGRADE_ISSUE: LeftB$ 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
			.FilePath = StrConv(LeftB$(StrConv(getfile.Ipstrfile, vbFromUnicode), getfile.nFileOffset), vbUnicode)
			.FileTitle = UniAsczToStr(getfile.IpstrFileTitle)
		End With
		
	End Function
	
	Public Function Y_GetSaveFileDialog(ByRef Hwnd As Integer, ByRef OpenInfo As OpenFileName2) As Boolean
		'***************************************************************
		'機能  ：「ファイル名を付けて保存」ダイアログを呼び出す
		'引数  ：Fm = 呼び出し元のフォームオブジェクト
		'       Openinfo = 「ファイル名を付けて保存」ダイアログの初期設定値
		'戻り値：ダイアログを閉じた後の設定値
		'***************************************************************
		
		Dim getfile As OPENFILENAME
		Dim FilterBuf As String
		Dim StrBuf As String
		Dim i As Integer
		Dim j As Integer
		Dim cnt As Short
		Dim filindex As Short
		Dim longret As Integer
		
		'-----初期値設定
		If (Left(OpenInfo.DefaultExt, 1) = ".") Then
			OpenInfo.DefaultExt = Mid(OpenInfo.DefaultExt, 2)
		End If
		
		If (OpenInfo.DialogTitle = vbNullString) Then
			OpenInfo.DialogTitle = "保存先ファイル"
		End If
		
		If (OpenInfo.MaxFileSize < 1) Or (OpenInfo.MaxFileSize > 32768) Then
			OpenInfo.MaxFileSize = MAX_PATH
		End If
		
		If (OpenInfo.FileTitle = vbNullString) Then
			OpenInfo.FileTitle = New String(Chr(0), OpenInfo.MaxFileSize)
		End If
		
		FilterBuf = OpenInfo.Filter_Renamed
		j = 1
		cnt = 1
		
		Do 
			i = InStr(j, FilterBuf, "|")
			If (i = 0) Then
				Exit Do
			End If
			
			Mid(FilterBuf, i, 1) = vbNullChar
			j = i + 1
			cnt = cnt + 1
		Loop 
		
		If (OpenInfo.FilterIndex < 1) Or (OpenInfo.FilterIndex > cnt) Then
			filindex = 0
		Else
			filindex = OpenInfo.FilterIndex
		End If
		
		''    StrBuf = String(OpenInfo.MaxFileSize, 0)
		'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
		'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
		StrBuf = OpenInfo.InitFileName & New String(Chr(0), OpenInfo.MaxFileSize - LenB(StrConv(OpenInfo.InitFileName, vbFromUnicode)))
		
		'-----コモンダイアログを呼び出す
		With getfile
			.IStructSize = Len(getfile)
			.hwndOwner = Hwnd
			.hInstance = 0 'App.hinstance
			.Ipstrfilter = FilterBuf
			.nMaxCustrFilter = 0
			.nFilterIndex = filindex
			.Ipstrfile = StrBuf
			.nMaxFile = OpenInfo.MaxFileSize
			.IpstrFileTitle = OpenInfo.FileTitle
			.nMaxFileTitle = Len(OpenInfo.FileTitle) + 1
			.IpstrInitialDir = OpenInfo.InitDir
			.IpstrTitle = OpenInfo.DialogTitle
			.flags = OpenInfo.flags
			.IpstrDefExt = OpenInfo.DefaultExt
		End With
		
		Y_GetSaveFileDialog = (GetSaveFileName(getfile) <> 0)
		
		'-----戻り値セット
		With OpenInfo
			.FileName = UniAsczToStr(getfile.Ipstrfile)
			'UPGRADE_ISSUE: 定数 vbUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			'UPGRADE_ISSUE: LeftB$ 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
			.FilePath = StrConv(LeftB$(StrConv(getfile.Ipstrfile, vbFromUnicode), getfile.nFileOffset), vbUnicode)
			.FileTitle = UniAsczToStr(getfile.IpstrFileTitle)
		End With
		
	End Function
	
	Public Function OpenSaveDialog(ByRef sFileName As String, ByRef DefaultFileName As String, Optional ByVal DefaultExt As String = "csv") As Boolean
		Dim INFO As OpenFileName2
		Dim FilePath, FileName, FileExtension As String
		
		Dim FileFilter As String
		Select Case DefaultExt
			Case "xls"
				'            FileFilter = "Excelファイル(*.xls)|*.xls"
				FileFilter = "Excelファイル(*.xls;*.xlsx)|*.xls;*.xlsx" '2017/02/09 ADD
			Case "bmp"
				FileFilter = "Windows Bitmap(*.bmp)|*.bmp"
			Case "jpg"
				FileFilter = "JPEG形式(*.jpg;*.jpeg)|*.jpg;*jpeg"
			Case "csv"
				FileFilter = "CSVファイル(*.csv)|*.csv"
			Case "pdf"
				FileFilter = "PDF(*.pdf)|*.pdf"
		End Select
		FileFilter = FileFilter & "|すべてのファイル(*.*)|*.*"
		'    FileName = NullToZero(FileNameTextBox, "")
		''''    Call SeparateFilename(FileNameTextBox.Text, FilePath, FileName, FileExtension)
		Call SeparateFilename(sFileName, FilePath, FileName, FileExtension)
		''    FilePath = NullToZero(INI.GAZOU, "")
		If FileName = vbNullString Then
			FileName = DefaultFileName & "." & DefaultExt
		Else
			FileName = FileName & "." & FileExtension
		End If
		
		'-----ダイアログのパラメータ設定
		With INFO
			.DefaultExt = DefaultExt
			.DialogTitle = "出力先ファイル"
			.FileName = vbNullString
			.FilePath = vbNullString
			.FileTitle = vbNullString
			.Filter_Renamed = FileFilter
			.FilterIndex = 1
			.flags = OFN_HIDEREADONLY
			.InitDir = FilePath
			.MaxFileSize = MAX_PATH
			.InitFileName = FileName
		End With
		
		'-----ダイアログ呼び出し
		'''    If Y_GetOpenFileDialog(FileNameTextBox.Hwnd, INFO) Then
		If Y_GetSaveFileDialog(System.Windows.Forms.Form.ActiveForm.Handle.ToInt32, INFO) Then
			sFileName = INFO.FileName
			'''        FileNameTextBox = FileName
			OpenSaveDialog = True
		Else
			OpenSaveDialog = False
		End If
	End Function
	
	Public Function OpenFileDialog(ByRef sFileName As String, ByRef DefaultFileName As String, Optional ByVal DefaultExt As String = "csv") As Boolean
		Dim INFO As OpenFileName2
		Dim FilePath, FileName, FileExtension As String
		
		Dim FileFilter As String
		Select Case DefaultExt
			Case "xls"
				'            FileFilter = "Excelファイル(*.xls)|*.xls"
				FileFilter = "Excelファイル(*.xls;*.xlsx)|*.xls;*.xlsx" '2017/02/09 ADD
			Case "bmp"
				FileFilter = "Windows Bitmap(*.bmp)|*.bmp"
			Case "jpg"
				FileFilter = "JPEG形式(*.jpg;*.jpeg)|*.jpg;*jpeg"
			Case "csv"
				FileFilter = "CSVファイル(*.csv)|*.csv"
			Case "txt"
				FileFilter = "テキストファイル(*.txt)|*.txt" '2019/08/03 ADD
			Case "csv;txt"
				FileFilter = "テキストファイル(*.txt;*.csv)|*.txt;*.csv" '2020/01/11 ADD
		End Select
		FileFilter = FileFilter & "|すべてのファイル(*.*)|*.*"
		'    FileName = NullToZero(FileNameTextBox, "")
		''''    Call SeparateFilename(FileNameTextBox.Text, FilePath, FileName, FileExtension)
		Call SeparateFilename(sFileName, FilePath, FileName, FileExtension)
		''    FilePath = NullToZero(INI.GAZOU, "")
		If FileName = vbNullString Then
			FileName = DefaultFileName & "." & DefaultExt
		Else
			FileName = FileName & "." & FileExtension
		End If
		
		'-----ダイアログのパラメータ設定
		With INFO
			.DefaultExt = DefaultExt
			.DialogTitle = "入力先ファイル"
			.FileName = vbNullString
			.FilePath = vbNullString
			.FileTitle = vbNullString
			.Filter_Renamed = FileFilter
			.FilterIndex = 1
			.flags = OFN_HIDEREADONLY
			.InitDir = FilePath
			.MaxFileSize = MAX_PATH
			.InitFileName = FileName
		End With
		
		'-----ダイアログ呼び出し
		'''    If Y_GetOpenFileDialog(FileNameTextBox.Hwnd, INFO) Then
		If Y_GetOpenFileDialog(System.Windows.Forms.Form.ActiveForm.Handle.ToInt32, INFO) Then
			sFileName = INFO.FileName
			'''        FileNameTextBox = FileName
			OpenFileDialog = True
		Else
			OpenFileDialog = False
		End If
	End Function
	
	Public Function CompactPathEx(ByVal strPath As String, ByVal intMax As Short) As String
		' strPath : 省略したいフルパス
		' intMax  : 文字数
		' 戻り値  : 省略されたパスが返る
		''''    Dim Buf As String
		''''
		''''    If LenB(StrConv(strPath, vbFromUnicode)) > intMax Then
		''''        Buf = StrConv(LeftB$(StrConv(strPath, vbFromUnicode), (intMax / 2 - 2)), vbUnicode)
		''''        Buf = Buf & " .. " & StrConv(RightB$(StrConv(strPath, vbFromUnicode), intMax / 2 - 2), vbUnicode)
		''''    Else
		''''        Buf = strPath
		''''    End If
		''''
		''''    CompactPathEx = Buf
		
		
		
		Dim lngResult As Integer
		Dim strBuffer As String
		''    intMax = 20
		
		If strPath = vbNullString Then Exit Function
		
		'''    strBuffer = String(LenB(StrConv(strPath, vbFromUnicode)), vbNullChar)
		strBuffer = New String(vbNullChar, intMax + 1)
		lngResult = PathCompactPathEx(strBuffer, strPath, intMax, 0)
		If lngResult Then
			If InStr(strBuffer, vbNullChar) > 0 Then
				CompactPathEx = Left(strBuffer, InStr(strBuffer, vbNullChar) - 1) 'ok
			Else
				CompactPathEx = strPath
				''            CompactPathEx = strBuffer
			End If
		Else
			CompactPathEx = strPath
		End If
		'''''''    CompactPathEx = strPath
		
	End Function
	
	'指定したフォルダが存在しないときに作成するモジュール
	'====================================================================
	'関数   MKFolder(Byval strPath as string,[Byval lngCheck as long]) as long
	'引数   strPath：フォルダのパス
	'       lngCheck：フォルダの作成前に確認するかどうか。
	'                 （省略可、省略したときは確認しない）
	'                  確認するときは １ を代入
	'戻り値 正常終了:0
	'       エラー ：1
	'特徴：Windowsが許す限り何階層でも一気に作れます。
	'注意：入力するパスはドライブ名を含むフルパスでないとエラーを返す
	'      ドライブ名のみの時もエラーを返す
	'====================================================================
	Public Function MKFolder(ByVal strPath As String, Optional ByVal lngCheck As Integer = 0) As Integer
		Dim lngHantei As Integer 'strPathが有効なパスかどうかを格納
		Dim intPathLength As Short 'パスの長さを格納
		Dim i As Short 'ループ変数
		Dim intEndNumber As Short 'フォルダ名の読み終わりの位置
		Dim strCharacter As String '一文字格納
		Dim strFolderPath As String '作成するフォルダのパスを格納
		
		Dim FSO As Object 'FileSystemObject
		
		On Error GoTo MKFolder_Err
		
		'値の初期化
		lngHantei = 0
		intPathLength = Len(strPath)
		'''    intEndNumber = 4    '４文字目から調べ始める
		
		If Left(strPath, 2) = "\\" Then
			'UNCパスの場合３文字目から調べて次の\の場所から始める
			intEndNumber = InStr(3, strPath, "\") + 1
		Else
			'通常パスの場合４文字目から調べる
			intEndNumber = 4
		End If
		
		'strPathが２文字以下の時はモジュールをぬける。
		If intPathLength < 3 Then
			CriticalAlarm("有効なパスではありません。")
			MKFolder = 1
			Exit Function
		End If
		
		'strPathが有効なパスでないときはモジュールをぬける。
		For i = 1 To intPathLength
			strCharacter = Mid(strPath, i, 1)
			'もし、"\"が見つかればループをぬける
			If strCharacter = "\" Then
				lngHantei = 1
				Exit For
			End If
		Next i
		
		'''    '２，３文字目が":\"でないとき(ドライブが指定されてないとき）
		'''    If Mid$(strPath, 2, 2) = ":\" Then lngHantei = 1
		'''
		'''    If lngHantei = 0 Then
		'''        CriticalAlarm "有効なパスではありません。"
		'''        MKFolder = 1
		'''        Exit Function
		'''    End If
		
		''''    'パスがドライブのみのときは、モジュールをぬける。
		''''    If Len(strPath) = 3 Then
		''''        CriticalAlarm "ドライブは作成できません。"
		''''        MKFolder = 1
		''''        Exit Function
		''''    End If
		
		'''''    'もし、フォルダが存在するときは、モジュールをぬける。
		'''''    If Len(Dir$(strPath, vbDirectory)) Then
		'''''        CriticalAlarm "フォルダがすでに存在しています"
		'''''        MKFolder = 1
		'''''        Exit Function
		'''''    End If
		
		
		'もし、最後の文字が"\"でないなら"\"を付け足す。
		If Right(strPath, 1) <> "\" Then
			strPath = strPath & "\"
			intPathLength = intPathLength + 1
		End If
		
		'FileSystemObjectの生成
		FSO = CreateObject("scripting.FileSystemObject")
		
		'フォルダの作成
		''    For i = 4 To intPathLength
		For i = intEndNumber To intPathLength
			strCharacter = Mid(strPath, i, 1)
			If strCharacter = "\" Then
				strFolderPath = Left(strPath, intEndNumber - 1)
				'''            If Len(Dir$(strFolderPath, vbDirectory)) = 0 Then
				'UPGRADE_WARNING: オブジェクト FSO.FolderExists の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				If FSO.FolderExists(strFolderPath) = False Then
					'フォルダ作成の確認
					If lngCheck = 1 Then
						lngCheck = 0
						If MsgBoxResult.No = YesNo("以下のフォルダは存在しません。" & vbCrLf & "作成します。") Then
							MKFolder = 1
							'UPGRADE_NOTE: オブジェクト FSO をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
							FSO = Nothing
							Exit Function
						End If
					End If
					'フォルダ作成
					'UPGRADE_WARNING: オブジェクト FSO.CreateFolder の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					FSO.CreateFolder(strFolderPath)
				End If
			End If
			intEndNumber = intEndNumber + 1
		Next i
		
		'戻り値を格納
		'UPGRADE_WARNING: Dir に新しい動作が指定されています。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
		If Len(Dir(strPath, 31)) Then
			MKFolder = 0
		Else
			MKFolder = 1
		End If
		
		'UPGRADE_NOTE: オブジェクト FSO をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		FSO = Nothing
		Exit Function
MKFolder_Err: 
		MsgBox(Err.Number & " " & Err.Description)
		MKFolder = 1
		'UPGRADE_NOTE: オブジェクト FSO をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		FSO = Nothing
	End Function
	
	'ファイル名に使えない文字を取り除く
	Public Function replaceNGchar(ByVal sourceStr As String, Optional ByVal replaceChar As String = "") As String
		
		Dim tempStr As String
		
		tempStr = sourceStr
		tempStr = Replace(tempStr, "\", replaceChar)
		tempStr = Replace(tempStr, "/", replaceChar)
		tempStr = Replace(tempStr, ":", replaceChar)
		tempStr = Replace(tempStr, "*", replaceChar)
		tempStr = Replace(tempStr, "?", replaceChar)
		tempStr = Replace(tempStr, """", replaceChar)
		tempStr = Replace(tempStr, "<", replaceChar)
		tempStr = Replace(tempStr, ">", replaceChar)
		tempStr = Replace(tempStr, "|", replaceChar)
		tempStr = Replace(tempStr, "[", replaceChar)
		tempStr = Replace(tempStr, "]", replaceChar)
		
		replaceNGchar = tempStr
	End Function
End Module