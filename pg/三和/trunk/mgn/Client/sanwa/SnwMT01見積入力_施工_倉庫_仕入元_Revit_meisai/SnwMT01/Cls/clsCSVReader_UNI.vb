Option Strict Off
Option Explicit On
Friend Class clsCSVReader_UNI
	'/** CsvReader_UNI クラス */
	'Unicode用
	
	'2018/04/11 oosawa      新設
	'2018/04/11 oosawa      カンマの処理を修正
	
	
	' EnumState2 列挙体
	Private Enum EnumState2
		None = 0 '読み込み開始前
		Beginning = 1 '初期状態の入力待ち
		WaitInput = 2 '入力待ち
		FindQuote = 3 '引用符を発見
		FindQuoteDouble = 4 '引用符の連続を発見
		InQuote = 5 '引用符の中で入力待ち
		FindQuoteInQuote = 6 '引用符の中で引用符を発見
		FindComma = 7 'カンマを発見
		FindCr = 8 'Cr を発見
		FindCrLf = 9 'CrLf を発見
		Error_Renamed = 255 'エラー発生
	End Enum
	
	' プロパティ変数
	Private m_FilePath As String
	Private m_HasHeader As Boolean
	Private m_IgnoreError As Boolean
	
	' Private フィールド
	'Private mTextStream As TextStream
	Private mTextStream As ADODB.Stream
	Private mState As EnumState2
	Private mHeaders As Collection
	
	' コンストラクタ
	'UPGRADE_NOTE: Class_Initialize は Class_Initialize_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
	Private Sub Class_Initialize_Renamed()
		mState = EnumState2.Beginning
	End Sub
	Public Sub New()
		MyBase.New()
		Class_Initialize_Renamed()
	End Sub
	
	' デストラクタ
	'UPGRADE_NOTE: Class_Terminate は Class_Terminate_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
	Private Sub Class_Terminate_Renamed()
		Call Me.CloseStream()
	End Sub
	Protected Overrides Sub Finalize()
		Class_Terminate_Renamed()
		MyBase.Finalize()
	End Sub
	
	' FilePath プロパティ - Getter
	Public ReadOnly Property FilePath() As String
		Get
			FilePath = m_FilePath
		End Get
	End Property
	
	' HasHeader プロパティ - Getter
	Public ReadOnly Property HasHeader() As Boolean
		Get
			HasHeader = m_HasHeader
		End Get
	End Property
	
	' IgnoreError プロパティ - Getter
	
	' IgnoreError プロパティ - Setter
	Public Property IgnoreError() As Boolean
		Get
			IgnoreError = m_IgnoreError
		End Get
		Set(ByVal Value As Boolean)
			m_IgnoreError = Value
		End Set
	End Property
	
	' EndOfStream プロパティ
	Public ReadOnly Property EndOfStream() As Boolean
		Get
			'    Let EndOfStream = mTextStream.AtEndOfStream
			EndOfStream = mTextStream.EOS
		End Get
	End Property
	
	' OpenStream メソッド
	'Public Function OpenStream(ByVal stFilePath As String) As Boolean
	Public Function OpenStream(ByVal stFilePath As String, Optional ByVal charset As String = "Unicode") As Boolean
		'"Shift-JIS"
		'"EUC-JP"
		'BOM付きUTF-8 = "UTF-8"
		
		
		On Error GoTo Exception
		
		m_FilePath = stFilePath
		
		'    Dim cFso As FileSystemObject
		'    Set cFso = New FileSystemObject
		'
		''    If cFso.FileExists(Me.filePath) = True Then
		'        'ファイルが存在した場合
		''        Set mTextStream = cFso.OpenTextFile(Me.filePath, , , TristateFalse)
		'        Set mTextStream = cFso.OpenTextFile(Me.filePath)
		'
		''    End If
		
		mTextStream = New ADODB.Stream
		'    Dim strData As String
		
		With mTextStream
			'キャラクタコードを設定
			'
			.charset = charset
			'Streamオブジェクトを開く
			.Open()
			'テキストファイルをStreamオブジェクトに読み込み
			.LoadFromFile(Me.FilePath)
			'        'Streamオブジェクトの内容を変数strDataに読み込み
			'        strData = .ReadText
		End With
		
		OpenStream = True
		Exit Function
		
Exception: 
		Call Me.CloseStream()
		OpenStream = False
	End Function
	
	' CloseStream メソッド
	Public Sub CloseStream()
		If Not mTextStream Is Nothing Then
			On Error Resume Next
			''        Call mTextStream.Close
			Call mTextStream.Close()
			On Error GoTo 0
		End If
	End Sub
	'''
	'''Public Sub WriteReadedFlg()
	'''    '書き込み
	'''    mTextStream.Position = 0
	'''    mTextStream.WriteText "xxx", adWriteLine
	'''    mTextStream.SaveToFile Me.FilePath, adSaveCreateOverWrite
	'''End Sub
	
	
	' ReadHeader メソッド
	Public Function ReadHeader() As Collection
		mHeaders = Me.ReadLine()
		m_HasHeader = True
		ReadHeader = mHeaders
	End Function
	
	' ReadLine メソッド
	Public Function ReadLine() As Collection
		Dim stReadLine As String
		Dim cRow As Collection
		Do While (True)
			''        stReadLine = stReadLine & mTextStream.ReadLine()
			stReadLine = stReadLine & mTextStream.ReadText(ADODB.StreamReadEnum.adReadLine)
			
			cRow = ReadLineInternal(stReadLine)
			
			Select Case mState
				Case EnumState2.FindQuote, EnumState2.InQuote
					stReadLine = stReadLine & vbNewLine
				Case Else
					Exit Do
			End Select
		Loop 
		
		ReadLine = cRow
	End Function
	
	' ReadToEnd メソッド
	Public Function ReadToEnd() As Collection
		Dim cTable As Collection
		cTable = New Collection
		
		Dim stReadAll As String
		''    stReadAll = mTextStream.ReadAll()
		stReadAll = mTextStream.ReadText(ADODB.StreamReadEnum.adReadAll)
		
		Dim stBuffers() As String
		stBuffers = Split(stReadAll, vbNewLine)
		
		Dim stReadLine As String
		Dim i As Integer
		Dim lIndex As Integer
		
		Dim cRow As Collection
		For i = LBound(stBuffers) To UBound(stBuffers)
			stReadLine = stReadLine & stBuffers(i)
			
			cRow = ReadLineInternal(stReadLine)
			
			Select Case mState
				Case EnumState2.FindQuote, EnumState2.InQuote
					stReadLine = stReadLine & vbNewLine
				Case Else
					stReadLine = ""
					lIndex = lIndex + 1
					Call cTable.Add(cRow)
			End Select
		Next 
		
		ReadToEnd = cTable
	End Function
	
	' 1 行読み込み
	Private Function ReadLineInternal(ByVal stBuffer As String) As Collection
		Dim cRow As Collection
		cRow = New Collection
		
		mState = EnumState2.Beginning
		
		Dim stItem As String
		Dim iIndex As Short
		Dim iSeek As Short
		
		Dim chNext As String
		For iSeek = 1 To Len(stBuffer)
			chNext = Mid(stBuffer, iSeek, 1)
			
			Select Case mState
				Case EnumState2.Beginning
					stItem = ReadForStateBeginning(stItem, chNext)
				Case EnumState2.WaitInput
					stItem = ReadForStateWaitInput(stItem, chNext)
				Case EnumState2.FindQuote
					stItem = ReadForStateFindQuote(stItem, chNext)
				Case EnumState2.FindQuoteDouble
					stItem = ReadForStateFindQuoteDouble(stItem, chNext)
				Case EnumState2.InQuote
					stItem = ReadForStateInQuote(stItem, chNext)
				Case EnumState2.FindQuoteInQuote
					stItem = ReadForStateFindQuoteInQuote(stItem, chNext)
			End Select
			
			Select Case mState
				Case EnumState2.FindCrLf
					mState = EnumState2.Beginning
					Exit For
				Case EnumState2.FindComma
					iIndex = iIndex + 1
					Call AddRowItem(stItem, cRow, iIndex)
					mState = EnumState2.Beginning
					stItem = ""
				Case EnumState2.Error_Renamed
					If Not Me.IgnoreError Then
						Call Err.Raise(5, "ReadLineInternal", "書式が不正です。")
					End If
					
					mState = EnumState2.WaitInput
			End Select
		Next 
		
		If mState = EnumState2.FindQuoteDouble Then
			'        stItem = stItem & """"
		End If
		
		iIndex = iIndex + 1
		Call AddRowItem(stItem, cRow, iIndex)
		ReadLineInternal = cRow
	End Function
	
	' 初回入力待ち状態での Read
	Private Function ReadForStateBeginning(ByVal stItem As String, ByVal chNext As String) As String
		Select Case chNext
			Case vbCr
				mState = EnumState2.FindCr
			Case ","
				mState = EnumState2.FindComma
			Case """"
				mState = EnumState2.FindQuote
			Case Else
				mState = EnumState2.WaitInput
				stItem = stItem & chNext
		End Select
		
		ReadForStateBeginning = stItem
	End Function
	
	' 入力待ち状態での Read
	Private Function ReadForStateWaitInput(ByVal stItem As String, ByVal chNext As String) As String
		Select Case chNext
			Case vbCr
				mState = EnumState2.FindCr
			Case ","
				mState = EnumState2.FindComma
			Case """"
				mState = EnumState2.FindQuote
			Case Else
				stItem = stItem & chNext
		End Select
		
		ReadForStateWaitInput = stItem
	End Function
	
	' 引用符を発見した状態での Read
	Private Function ReadForStateFindQuote(ByVal stItem As String, ByVal chNext As String) As String
		Select Case chNext
			Case """"
				mState = EnumState2.FindQuoteDouble
			Case Else
				mState = EnumState2.InQuote
				stItem = stItem & chNext
		End Select
		
		ReadForStateFindQuote = stItem
	End Function
	
	' 引用符の連続を発見した状態での Read
	Private Function ReadForStateFindQuoteDouble(ByVal stItem As String, ByVal chNext As String) As String
		Select Case chNext
			Case vbCr
				mState = EnumState2.FindCr
				stItem = stItem & """"
			Case ","
				mState = EnumState2.FindComma
				'連続「"」の後の「,」の場合は「"」をセットする必要なくない？'2018/04/11 DEL
				'            stItem = stItem & """"
				'            stItem = stItem & """"""
			Case """"
				mState = EnumState2.FindQuote
				stItem = stItem & """"
			Case Else
				mState = EnumState2.WaitInput
				stItem = stItem & """" & chNext
		End Select
		
		ReadForStateFindQuoteDouble = stItem
	End Function
	
	' 引用符の中で入力待ち状態での Read
	Private Function ReadForStateInQuote(ByVal stItem As String, ByVal chNext As String) As String
		Select Case chNext
			Case """"
				mState = EnumState2.FindQuoteInQuote
			Case Else
				stItem = stItem & chNext
		End Select
		
		ReadForStateInQuote = stItem
	End Function
	
	' 引用符の中で引用符を発見した状態での Read
	Private Function ReadForStateFindQuoteInQuote(ByVal stItem As String, ByVal chNext As String) As String
		Select Case chNext
			Case vbCr
				mState = EnumState2.FindCr
			Case ","
				mState = EnumState2.FindComma
			Case """"
				mState = EnumState2.InQuote
				stItem = stItem & """"
			Case Else
				mState = EnumState2.Error_Renamed
		End Select
		
		ReadForStateFindQuoteInQuote = stItem
	End Function
	
	' Row にアイテムを入れる
	Private Sub AddRowItem(ByVal stItem As String, ByVal cRow As Collection, ByVal lIndex As Integer)
		If Me.HasHeader Then
			Call cRow.Add(stItem, mHeaders.Item(lIndex))
		Else
			Call cRow.Add(stItem)
		End If
	End Sub
	
	'''' 使用例となるサンプルコードを以下に示します｡
	''''
	''''VB (VB6) - CSV 読み込みクラス 使用例 1
	''' Private Sub MosaMosaAA()
	'''    Dim cCsvReader As CsvReader
	'''    Set cCsvReader = New CsvReader
	'''
	'''    ' 指定した CSV ファイルを開く
	'''    Call cCsvReader.OpenStream("C:\MakiMaki.csv")
	'''
	'''    ' CSV ファイルの中身をすべて取得する
	'''    Dim cTable As Collection
	'''    Set cTable = cCsvReader.ReadToEnd()
	'''
	'''    ' すべての中身 (Table) から 行 (Row) を列挙して取り出す
	'''    Dim cRow As Collection
	'''    For Each cRow In cTable
	'''        Dim i As Integer
	'''
	'''        ' 行から添え字を使って各 Item を出力する
	'''        For i = 1 To cRow.Count()
	'''            Debug.Print cRow(i)
	'''        Next
	'''
	'''        Debug.Print vbNewLine
	'''    Next
	'''End Sub
	
	'''' ReadHeader メソッドを使用すると、各 Item にカラム名からアクセスすることができます。
	''''
	''''VB (VB6) - CSV 読み込みクラス 使用例 2
	''' Private Sub MosaMosaAA()
	'''    Dim cCsvReader As CsvReader
	'''    Set cCsvReader = New CsvReader
	'''
	'''    ' 指定した CSV ファイルを開く
	'''    Call cCsvReader.OpenStream("C:\MakiMaki.csv")
	'''
	'''    ' 最初の行をヘッダとして読み込む
	'''    Call cCsvReader.ReadHeader
	'''
	'''    ' CSV ファイルの中身をすべて取得する
	'''    Dim cTable As Collection
	'''    Set cTable = cCsvReader.ReadToEnd()
	'''
	'''    ' すべての中身 (Table) から 行 (Row) を列挙して取り出す
	'''    Dim cRow As Collection
	'''    For Each cRow In cTable
	'''        ' 行からカラム名を使って各 Item を出力する
	'''        Debug.Print cRow("社員番号")
	'''        Debug.Print cRow("社員名")
	'''        Debug.Print cRow("住所")
	'''        Debug.Print cRow("電話番号")
	'''        Debug.Print
	'''    Next
	'''End Sub
End Class