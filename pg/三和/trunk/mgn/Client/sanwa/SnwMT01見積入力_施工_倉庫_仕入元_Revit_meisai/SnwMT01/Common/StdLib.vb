Option Strict Off
Option Explicit On
Module StdLib
	'Ver.1.00           '2002.04.17
	'Ver.1.01           '2002.08.20     CurrentObjCaption 修正
	'Ver.1.02           '2002.09.19     SpcToNull NULLのチェックを追加
	'Ver.1.03           '2002.10.16     IsCheckNullでNULLの場合、Trim$がｴﾗｰになるのでTrimにする
	'Ver.1.04           '2002.10.16     CurrentObjCaption レポートでも使用するのでObject型に変更
	'Ver.1.05           '2003.05.16     OpenChildForm CloseMeを追加 ×使えない
	'Ver.1.06           '2003.09.30     GetDecimalFormat$を追加
	'Ver.1.07           '2003.10.03     CutTextを追加
	
	
	
	'Ver.1.11           '2012.07.25     AnsiLeftBがWin7だと泣き分かれを起こすので修正
	'Ver.1.12           '2012/10/09     AnsiLeftBの修正がおかしいので修正
	
	
	Public HourGlassSwNest As Short 'マウスカーソルの変更回数カウント
	
	Public Const MB_ICONMASK As Integer = &HF0
	Public Const MB_OK As Integer = &H0
	Public Const MB_ICONHAND As Integer = &H10
	Public Const MB_ICONQUESTION As Integer = &H20
	Public Const MB_ICONEXCLAMATION As Integer = &H30
	Public Const MB_ICONASTERISK As Integer = &H40
	Public Const MB_ICONINFORMATION As Integer = MB_ICONASTERISK
	
	Declare Function MessageBEEP Lib "user32"  Alias "MessageBeep"(ByVal wType As Integer) As Integer
	
	'Const FRM_DOING = "~~Doing"
	
	Enum eFormMode
		FormClose = -1
		FormHide = 0
		FormShow = 1
	End Enum
	'Public NestDoing As Integer
	'Public AbortDoing As Boolean
	'Public FormOpenCancel As String
	'Public ReportNoDataStatus As String
	
	'Public ErrorDetails As New Collection
	
	Public Sub BeepA()
		MessageBEEP(MB_ICONQUESTION)
	End Sub
	
	Public Sub MSGbeep(Optional ByVal MBpara As Integer = MB_OK)
		MessageBEEP(MBpara And MB_ICONMASK)
	End Sub
	
	Public Function BeepMsgBox(ByRef Prompt As String, ByRef Buttons As Integer, ByRef Title As String) As Integer
		MSGbeep(Buttons)
		BeepMsgBox = MsgBox(Prompt, Buttons, Title)
	End Function
	
	'///// 現在コードを実行中のオブジェクトの標題を取得します
	Public Function CurrentObjCaption() As Object
		Dim obj As Object 'レポートでも使用するのでObject型にする
		''    Dim obj As Form
		''    On Error Resume Next
		obj = System.Windows.Forms.Form.ActiveForm
		If Not (obj Is Nothing) Then
			''    If Err = 0 Then
			''        Do
			''            Set obj = obj.Parent
			''            If Not (obj Is Nothing) Then
			''            If Err Then
			''                Err.Clear
			''                Exit Do
			''            End If
			''        Loop
			Do 
				'UPGRADE_WARNING: オブジェクト obj.MDIChild の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				If obj.MDIChild = True Then
					'UPGRADE_WARNING: オブジェクト obj.Parent の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					obj = obj.Parent
				Else
					Exit Do
				End If
			Loop 
			'UPGRADE_WARNING: オブジェクト obj.Caption の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト CurrentObjCaption の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			CurrentObjCaption = obj.Caption
		Else
			''        Err.Clear
			'UPGRADE_WARNING: オブジェクト CurrentObjCaption の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			CurrentObjCaption = My.Application.Info.Title
			'UPGRADE_WARNING: オブジェクト CurrentObjCaption の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If CurrentObjCaption = vbNullString Then
				''        If Err Then
				''            Err.Clear
				'UPGRADE_WARNING: App プロパティ App.EXEName には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト CurrentObjCaption の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				CurrentObjCaption = My.Application.Info.AssemblyName & " System Error."
			End If
		End If
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		If IsDbNull(CurrentObjCaption) Then
			'UPGRADE_WARNING: オブジェクト CurrentObjCaption の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			CurrentObjCaption = ""
		End If
		'UPGRADE_NOTE: オブジェクト obj をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		obj = Nothing
	End Function
	
	'///// MsgBox:Title取得
	Private Function MsgTitle(ByRef Title As Object) As String
		'UPGRADE_WARNING: オブジェクト Title の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If Title = "" Then
			'UPGRADE_WARNING: オブジェクト CurrentObjCaption() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			MsgTitle = CurrentObjCaption()
		Else
			'UPGRADE_WARNING: オブジェクト Title の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			MsgTitle = Title
		End If
	End Function
	
	'///// 警告ﾒｯｾｰｼﾞ（赤丸×）
	Sub CriticalAlarm(ByRef msg As String, Optional ByRef Title As String = vbNullString)
		BeepMsgBox(msg, MsgBoxStyle.Critical + MsgBoxStyle.OKOnly, MsgTitle(Title))
	End Sub
	
	'///// 項目ﾁｪｯｸｴﾗｰﾒｯｾｰｼﾞ（三角！)
	Sub CheckAlarm(ByRef msg As String, Optional ByRef Title As String = vbNullString)
		BeepMsgBox(msg, MsgBoxStyle.Exclamation + MsgBoxStyle.OKOnly, MsgTitle(Title))
	End Sub
	
	'///// 確認ﾒｯｾｰｼﾞ
	Function VerifyYesNo(ByRef msg As String, ByVal DefaultBT As MsgBoxStyle, Optional ByRef Title As String = vbNullString) As Object
		'UPGRADE_WARNING: オブジェクト Question() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		VerifyYesNo = Question(msg & vbCrLf & vbCrLf & "よろしいですか？", DefaultBT, Title)
	End Function
	
	'///// 確認ﾒｯｾｰｼﾞ（吹き出し？）
	Function YesNo(ByRef msg As String, Optional ByRef Title As String = vbNullString) As Object
		'UPGRADE_WARNING: オブジェクト VerifyYesNo() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		YesNo = VerifyYesNo(msg, MsgBoxResult.Yes, Title)
	End Function
	
	'///// 確認ﾒｯｾｰｼﾞ（吹き出し？）
	Function NoYes(ByRef msg As String, Optional ByRef Title As String = vbNullString) As Object
		'UPGRADE_WARNING: オブジェクト VerifyYesNo() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		NoYes = VerifyYesNo(msg, MsgBoxResult.No, Title)
	End Function
	
	'///// 確認ﾒｯｾｰｼﾞ（吹き出し？）
	Function Question(ByRef msg As String, ByVal DefaultBT As MsgBoxStyle, Optional ByRef Title As String = vbNullString) As Object
		Dim BT As Short
		Select Case DefaultBT
			Case MsgBoxResult.Yes : BT = MsgBoxStyle.DefaultButton1
			Case Else : BT = MsgBoxStyle.DefaultButton2
		End Select
		MSGbeep(MB_OK)
		Question = MsgBox(msg, MsgBoxStyle.Question + MsgBoxStyle.YesNo + BT, MsgTitle(Title))
	End Function
	'（吹き出しｉ）
	Function Inform(ByRef msg As String, Optional ByRef Title As String = "") As Object
		MSGbeep(MB_OK)
		MsgBox(msg, MsgBoxStyle.Information + MsgBoxStyle.OKOnly, MsgTitle(Title))
	End Function
	
	Public Sub HourGlass(ByRef sw As Boolean)
		If sw Then
			'UPGRADE_WARNING: Screen プロパティ Screen.MousePointer には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
			System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
			HourGlassSwNest = HourGlassSwNest + 1
		Else
			If HourGlassSwNest > 0 Then
				HourGlassSwNest = HourGlassSwNest - 1
			End If
			If HourGlassSwNest = 0 Then
				'UPGRADE_WARNING: Screen プロパティ Screen.MousePointer には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
			End If
		End If
	End Sub
	''
	''
	'''///// 指定のフォーム名のフォームをオープンする（指定のフォーム＝呼び先フォーム）
	'''///// オープンする際、呼び元のフォームは指定された状態にする
	'''///// 引数 ------------
	'''///// ParentForm     : 呼び元のフォームオブジェクトを指定する。
	'''///// ChildFormName  : 呼び先のフォーム名を指定する。
	'''///// ParentFormMode : 呼び元のフォームの状態を指定する。(Close or Hide or Show)
	'''///// ChildFormModal : 呼び先のフォームの状態を指定する。(Modeless or Modal)
	''Public Sub OpenChildForm(ByVal ParentForm As Form, ByVal ChildFormName As String, Optional ParentFormMode As eFormMode = FormHide, Optional ChildFormModal As Boolean = False)
	''    Dim ChildForm As Form
	''    Dim MyStack As Variant
	''    Dim FormStack As Variant
	''    Dim i As Integer
	''
	''    On Error GoTo Err_OpenChildForm
	''
	''    '呼び先のフォームをセットする
	''''''    Set ChildForm = Forms.Add(ChildFormName)
	''    For i = 0 To Forms.Count - 1
	''        If Forms(i).Name = ChildFormName Then
	''            Set ChildForm = Forms(i)
	''            Exit For
	''        End If
	''    Next
	''    If ChildForm Is Nothing Then
	''        Set ChildForm = Forms.Add(ChildFormName)
	''    End If
	''    '呼び元のフォーム状態を呼び先のフォームにセットする
	''    Select Case ParentFormMode
	''        Case FormClose
	''            MyStack = 1
	''        Case FormHide
	''            MyStack = 0
	''        Case FormShow
	''            MyStack = vbNullString
	''    End Select
	''    FormStack = ParentForm.Name & vbCr & MyStack
	''    ChildForm.Tag = FormStack
	''
	''    '呼び先のフォームを開く
	''    Select Case ChildFormModal
	''        Case True
	''            ChildForm.Show vbModal
	''        Case False
	''            ChildForm.Show
	''    End Select
	''
	''    '呼び元のフォームを指定された状態にする。
	''    Select Case ParentFormMode
	''        Case FormHide
	''            ParentForm.Hide
	''        Case FormClose
	''            Unload ParentForm
	''    End Select
	''
	''Exit_OpenChildForm:
	''    Set ChildForm = Nothing
	''
	''    Exit Sub
	''Err_OpenChildForm:
	''    Set ChildForm = Nothing
	''
	''    MsgBox Err.Description & " : " & _
	'''    Err.Number & " : " & _
	'''    Err.Source & " on OpenChildForm"
	''    Exit Sub
	''End Sub
	''
	'''///// 呼出し元のﾌｫｰﾑを閉じ、親ﾌｫｰﾑを開きます
	''Public Sub CloseMe(CloseForm As Form)
	''    Dim RetFormName As String
	''    Dim RetForm As Form
	''    Dim FormStack As Variant
	''    Dim Sep As Integer
	''    Dim i As Integer
	''
	''    On Error GoTo Err_CloseMe
	''
	''    If CloseForm Is Nothing Then
	''        Debug.Print "ACT:" & Screen.ActiveForm.Name
	''        Set CloseForm = Screen.ActiveForm
	''    End If
	''    '親フォームの状態を取得する
	''    FormStack = CloseForm.Tag
	''    If FormStack = vbNullString Then
	''        RetFormName = vbNullString
	''    Else
	''        Sep = InStr(FormStack, vbCr)
	''        If Sep Then
	''            RetFormName = Left$(FormStack, Sep - 1)
	''            FormStack = Mid$(FormStack, Sep + 1)
	''        Else
	''            RetFormName = FormStack
	''            FormStack = vbNullString
	''        End If
	''    End If
	''
	''    If FormStack <> vbNullString Then
	''        '親フォームの存在チェック
	''        For i = 0 To Forms.Count - 1
	''            If Forms(i).Name = RetFormName Then
	''                Set RetForm = Forms(i)
	''                Exit For
	''            End If
	''        Next
	''        If i = Forms.Count Then
	''            Set RetForm = Forms.Add(RetFormName)
	''        End If
	''
	''        '親フォームの状態によって処理する
	''        If FormStack = 1 Then
	''            RetForm.Show
	''        Else
	''            RetForm.Visible = True
	''        End If
	''    End If
	''
	''    '呼び元のフォームを閉じる
	''    Unload CloseForm
	''
	''Exit_CloseMe:
	''    Set RetForm = Nothing
	''
	''    Exit Sub
	''Err_CloseMe:
	''    Set RetForm = Nothing
	''
	''    MsgBox Err.Description & " : " & _
	'''    Err.Number & " : " & _
	'''    Err.Source & " on CloseMe"
	''    Exit Sub
	''End Sub
	
	'*******************************************************************************************
	'関数名     CutOutPath
	'機能       フルパスからファイル名だけを除いたパス取得
	'引数       tmp:ファイル名フルパス
	'*******************************************************************************************
	Public Function CutOutPath(ByRef tmp As String) As String
		Dim p1, p2 As Short
		p1 = 0
		Do 
			p2 = InStr(p1 + 1, tmp, "\")
			If p2 = 0 Then Exit Do
			p1 = p2
		Loop 
		If p1 = 0 Then
			CutOutPath = vbNullString
		Else
			CutOutPath = Left(tmp, p1 - 1)
		End If
	End Function
	
	'*******************************************************************************************
	'関数名     AppPath
	'機能       App.Path使用時に￥マークを取得する方法
	'引数       この関数により、EXEファイルがどのディレクトリに存在しようと、
	'           AppPath("fname")は
	'*******************************************************************************************
	Public Function AppPath(Optional ByRef fileName As String = "", Optional ByRef AttachedFilename As Boolean = True) As String
		
		If Right(My.Application.Info.DirectoryPath, 1) = "\" Then
			If AttachedFilename Then
				AppPath = My.Application.Info.DirectoryPath & fileName
			Else
				AppPath = My.Application.Info.DirectoryPath
			End If
		Else
			If AttachedFilename Then
				AppPath = My.Application.Info.DirectoryPath & "\" & fileName
			Else
				AppPath = My.Application.Info.DirectoryPath & "\"
			End If
		End If
		
	End Function
	
	
	Public Sub SeparateFilename(ByRef fileName As String, ByRef filePath As String, ByRef BaseName As String, ByRef Extension As String)
		Dim strTmp As String
		Dim p1, p2 As Short
		strTmp = fileName
		p1 = 0
		Do 
			p2 = InStr(p1 + 1, strTmp, "\")
			If p2 = 0 Then Exit Do
			p1 = p2
		Loop 
		If p1 = 0 Then
			filePath = "."
		Else
			filePath = Left(strTmp, p1 - 1)
		End If
		strTmp = Mid(strTmp, p1 + 1)
		p1 = InStr(strTmp, ".")
		If p1 = 0 Then
			BaseName = strTmp
			Extension = ""
		Else
			Do 
				p2 = InStr(p1 + 1, strTmp, ".")
				If p2 = 0 Then Exit Do
				p1 = p2
			Loop 
			BaseName = Left(strTmp, p1 - 1)
			Extension = Mid(strTmp, p1 + 1)
		End If
	End Sub
	
	'任意数のオブジェクトを配列格納タイプのバリアント変数に格納します
	'UPGRADE_WARNING: ParamArray Objs が ByRef から ByVal に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="93C6A0DC-8C99-429A-8696-35FC4DCEFCCC"' をクリックしてください。
	Public Function SetObjectArray(ParamArray ByVal Objs() As Object) As Object
		Dim ret As Object
		Dim i As Short
		'UPGRADE_WARNING: Array に新しい動作が指定されています。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト ret の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		ret = New Object(){0}
		ReDim ret(UBound(Objs))
		For i = 0 To UBound(Objs)
			ret(i) = Objs(i)
		Next 
		'UPGRADE_WARNING: オブジェクト ret の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト SetObjectArray の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		SetObjectArray = ret
	End Function
	
	'********************************************************************
	'*                                                                  *
	'*  Ｎｚ関数もどき                                                   *
	'*                                                                  *
	'*  パラメータ１：変換前文字列                                        *
	'*  パラメータ２：変換したい値（0　or　""）                            *
	'*  戻り値　　　：変換後文字列                                        *
	'*                                                                  *
	'*  修正履歴    2003/10/16  kawamura    引数が文字型以外はTrim$をしないに変更
	'**************************************************************************
	Public Function NullToZero(ByRef Value As Object, Optional ByRef ChangeValue As Object = 0) As Object
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		If IsDbNull(Value) Then
			'UPGRADE_WARNING: オブジェクト ChangeValue の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト NullToZero の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			NullToZero = ChangeValue
			'UPGRADE_WARNING: オブジェクト Value の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		ElseIf Value = vbNullString Then 
			'UPGRADE_WARNING: オブジェクト ChangeValue の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト NullToZero の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			NullToZero = ChangeValue
		Else
			''---2003.10.16.DEL
			''        NullToZero = Trim$(Value)
			''---2003.10.16.ADD
			'UPGRADE_WARNING: VarType に新しい動作が指定されています。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
			Select Case VarType(Value)
				Case VariantType.String
					'UPGRADE_WARNING: オブジェクト Value の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					NullToZero = Trim(Value)
				Case Else
					'UPGRADE_WARNING: オブジェクト Value の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト NullToZero の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					NullToZero = Value
			End Select
			''-----------------
		End If
	End Function
	
	'*******************************************************************************************
	'関数名     SpcToNull
	'機能       変数がスペースの場合、引数２の値を返す
	'           変数がスペース以外､前後スペースをはずした変数を返す
	'引数       Value:値
	'           ChangeValue:スペースから変換したい値
	'*******************************************************************************************
	'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
	Public Function SpcToNull(ByRef Value As Object, Optional ByRef ChangeValue As Object = System.DBNull.Value) As Object
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		If IsDbNull(Value) Then '2002/09/19 ADD
			'UPGRADE_WARNING: オブジェクト ChangeValue の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト SpcToNull の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			SpcToNull = ChangeValue
			'UPGRADE_WARNING: オブジェクト Value の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		ElseIf Trim(Value) = vbNullString Then 
			'UPGRADE_WARNING: オブジェクト ChangeValue の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト SpcToNull の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			SpcToNull = ChangeValue
		Else
			'UPGRADE_WARNING: オブジェクト Value の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			SpcToNull = Trim(Value)
		End If
	End Function
	
	'*******************************************************************************************
	'関数名     SelText
	'機能       入力文字反転
	'引数       SelectControl：反転するテキスト内文字列
	'戻り値
	'*******************************************************************************************
	Public Sub SelText(ByRef SelectControl As System.Windows.Forms.Control)
		On Error Resume Next
		'UPGRADE_WARNING: オブジェクト SelectControl.SelStart の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		SelectControl.SelStart = 0
		'UPGRADE_WARNING: オブジェクト SelectControl.SelLength の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		SelectControl.SelLength = Len(SelectControl.Text)
		On Error GoTo 0
	End Sub
	
	'*******************************************************************************************
	'関数名     AnsiRightB
	'機能       RightB関数のANSI版
	'引数       Str：文字列
	'           Length：取り出す文字列の文字数
	'戻り値
	'*******************************************************************************************
	'UPGRADE_NOTE: Str は Str_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
	Public Function AnsiRightB(ByVal Str_Renamed As String, ByVal Length As Integer) As Object
		'UPGRADE_ISSUE: 定数 vbUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
		'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
		'UPGRADE_ISSUE: RightB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト AnsiRightB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		AnsiRightB = CObj(StrConv(RightB(StrConv(Str_Renamed, vbFromUnicode), Length), vbUnicode))
	End Function
	
	'*******************************************************************************************
	'関数名     AnsiLeftB
	'機能       LeftB関数のANSI版
	'引数       Str：文字列
	'           Length：取り出す文字列の文字数
	'戻り値
	'*******************************************************************************************
	'UPGRADE_NOTE: Str は Str_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
	Public Function AnsiLeftB(ByVal Str_Renamed As String, ByVal Length As Integer) As Object
		'''2012/07/25 ADD↓
		'''Win7だとうまくいかない！！ので修正
		Dim leng As Integer
		
		'''''''''    If Len(StrConv(LeftB(StrConv(Str, vbFromUnicode), Length), vbUnicode)) = Len(StrConv(LeftB(StrConv(Str, vbFromUnicode), Length - 1), vbUnicode)) Then
		'''''''''    '1バイト前で切ったときに文字数が変わらないのは切った位置が全角文字の後半であるので
		'''''''''    '切り位置が正しいと判断する
		'''''''''        leng = Length
		'''''''''    Else  '泣き別れのパターン
		'''''''''        leng = Length - 1
		'''''''''    End If
		'2012/10/9 ADD↓    '上記がうまくいかなかったので修正
		'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
		'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
		If LenB(StrConv(Str_Renamed, vbFromUnicode)) <= Length Then
			'バイト数が切り出し数より小さい場合、何もしない
			leng = Length
		Else
			'UPGRADE_ISSUE: 定数 vbUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			'UPGRADE_ISSUE: LeftB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
			If Len(StrConv(LeftB(StrConv(Str_Renamed, vbFromUnicode), Length), vbUnicode)) = Len(StrConv(LeftB(StrConv(Str_Renamed, vbFromUnicode), Length + 1), vbUnicode)) Then
				'[(N+0)バイト = (N+1)バイト]なら、文字最中でぶった切ったことになる
				leng = Length - 1
			Else
				leng = Length
			End If
		End If
		
		'UPGRADE_ISSUE: 定数 vbUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
		'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
		'UPGRADE_ISSUE: LeftB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト AnsiLeftB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		AnsiLeftB = CObj(StrConv(LeftB(StrConv(Str_Renamed, vbFromUnicode), leng), vbUnicode))
		'''2012/07/25 ADD↑
		'''    AnsiLeftB = CVar(StrConv(LeftB(StrConv(Str, vbFromUnicode), Length), vbUnicode))
		
	End Function
	
	'*******************************************************************************************
	'関数名     AnsiLenB
	'機能       LenB関数のANSI版
	'引数       Str：文字列
	'戻り値
	'*******************************************************************************************
	'UPGRADE_NOTE: Str は Str_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
	Public Function AnsiLenB(ByVal Str_Renamed As String) As Integer
		'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
		'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
		AnsiLenB = LenB(StrConv(Str_Renamed, vbFromUnicode))
	End Function
	
	'*******************************************************************************************
	'関数名     AnsiMidB
	'機能       MidB関数のANSI版
	'引数       Str：文字列
	'           Start：取り出す文字列の開始位置
	'           Length：取り出す文字列の文字数
	'戻り値
	'*******************************************************************************************
	'UPGRADE_NOTE: Str は Str_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
	Public Function AnsiMidB(ByVal Str_Renamed As String, ByVal Start As Integer, Optional ByRef Length As Object = Nothing) As Object
		'UPGRADE_NOTE: IsMissing() は IsNothing() に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="8AE1CB93-37AB-439A-A4FF-BE3B6760BB23"' をクリックしてください。
		If IsNothing(Length) Then
			'UPGRADE_ISSUE: 定数 vbUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			'UPGRADE_ISSUE: MidB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト AnsiMidB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			AnsiMidB = CObj(StrConv(MidB(StrConv(Str_Renamed, vbFromUnicode), Start), vbUnicode))
		Else
			'UPGRADE_ISSUE: 定数 vbUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			'UPGRADE_ISSUE: MidB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト AnsiMidB の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			AnsiMidB = CObj(StrConv(MidB(StrConv(Str_Renamed, vbFromUnicode), Start, Length), vbUnicode))
		End If
	End Function
	
	'********************************************
	'
	'             数値変換を行う
	'
	'********************************************
	Function Numeric_Cnv(ByRef InData As Object) As Double
		On Error Resume Next
		Dim OUTDATA As String
		Dim LenMax As Short
		Dim Idx As Short
		Dim ChkChr As String
		OUTDATA = ""
		'UPGRADE_WARNING: オブジェクト InData の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		LenMax = AnsiLenB(InData)
		For Idx = 1 To LenMax
			'UPGRADE_WARNING: オブジェクト InData の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト AnsiMidB() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			ChkChr = AnsiMidB(InData, Idx, 1)
			If (Val(ChkChr) >= 0 And Val(ChkChr) <= 9) Or ChkChr = "." Or ChkChr = "-" Then
				OUTDATA = OUTDATA & ChkChr
			End If
		Next Idx
		Numeric_Cnv = CDbl(OUTDATA)
	End Function
	
	'*******************************************************************************************
	'関数名     IsCheckNull
	'機能       指定文字列が ＮＵＬＬ／空白 かチェックする
	'引数       Chk_Str : チェックする文字列
	'戻り値     True  : ＮＵＬＬ／空白
	'           False : ＮＵＬＬ／空白でない
	'*******************************************************************************************
	Public Function IsCheckNull(ByRef Chk_Str As Object) As Short
		'''''    If (IsNull(Chk_Str) = True) Or (Trim$(Chk_Str) = vbNullString) Then    '2002/10/16
		'UPGRADE_WARNING: オブジェクト Chk_Str の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		If (IsDbNull(Chk_Str) = True) Or (Trim(Chk_Str) = vbNullString) Then
			IsCheckNull = True
		Else
			IsCheckNull = False
		End If
	End Function
	
	'*******************************************************************************************
	'関数名     IsCheckText
	'機能       指定テキストが ＮＵＬＬ／空白 かチェックする
	'引数       Chk_Str : チェックするテキスト
	'戻り値     True  : ＮＵＬＬ／空白
	'           False : ＮＵＬＬ／空白でない
	'*******************************************************************************************
	Public Function IsCheckText(ByRef ctl As System.Windows.Forms.Control) As Short
		On Error GoTo Check_Text_Err
		IsCheckText = False
		'--- 項目チェック
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		If IsDbNull(ctl.Text) = True Then
			Exit Function
		End If
		If Trim(ctl.Text) = vbNullString Then
			Exit Function
		End If
		
		IsCheckText = True
		Exit Function
Check_Text_Err: 
		MsgBox(Err.Number & " " & Err.Description)
	End Function
	
	'*******************************************************************************************
	'         四捨五入ルーチン
	'*******************************************************************************************
	' 書式        Round(Num,Keta)
	'  値         Num … 四捨五入する値    Keta … 四捨五入した結果の桁数
	'
	' 使用例  少数点以下第２位で四捨五入して、少数点位下１桁の値にする場合
	'             Round(四捨五入する値 , 1)
	'*******************************************************************************************
	Public Function ISRound(ByRef Num As Decimal, ByRef Keta As Short) As Decimal
		ISRound = Int(System.Math.Abs(Num) * CDec(10@ ^ Keta) + 0.5@) / CDec(10@ ^ Keta) * System.Math.Sign(Num)
	End Function
	
	'*******************************************************************************************
	'         切り上げルーチン
	'*******************************************************************************************
	' 書式        RoundUp(Num,Keta)
	'  値         Num … 切り捨てする値    Keta … 切り捨てした結果の桁数
	'
	' 使用例  少数点以下第２位で切り上げして、少数点位下１桁の値にする場合
	'             RoundDown(切り上げする値 , 1)
	'*******************************************************************************************
	Public Function ISRoundUp(ByRef Num As Decimal, ByRef Keta As Short) As Decimal
		ISRoundUp = Int(System.Math.Abs(Num) * CDec(10@ ^ Keta) + 0.9@) / CDec(10@ ^ Keta) * System.Math.Sign(Num)
	End Function
	
	'*******************************************************************************************
	'         切り捨てルーチン
	'*******************************************************************************************
	' 書式        RoundDown(Num,Keta)
	'  値         Num … 切り捨てする値    Keta … 切り捨てした結果の桁数
	'
	' 使用例  少数点以下第２位で切り捨てして、少数点位下１桁の値にする場合
	'             RoundDown(切り捨てする値 , 1)
	'*******************************************************************************************
	Public Function ISRoundDown(ByRef Num As Decimal, ByRef Keta As Short) As Decimal
		ISRoundDown = Int(System.Math.Abs(Num) * CDec(10@ ^ Keta)) / CDec(10@ ^ Keta) * System.Math.Sign(Num)
	End Function
	
	'*******************************************************************************************
	'         端数処理ルーチン
	'*******************************************************************************************
	' 書式        ISHasuu_rtn(kubun,Num,Keta)
	'  値         kubun … 端数区分    Num … 値    Keta … 結果の桁数
	'
	'*******************************************************************************************
	Public Function ISHasuu_rtn(ByRef kubun As Short, ByRef Num As Decimal, ByRef Keta As Short) As Decimal
		Select Case kubun
			Case 0
				ISHasuu_rtn = ISRound(Num, Keta)
			Case 1
				ISHasuu_rtn = ISRoundUp(Num, Keta)
			Case 2
				ISHasuu_rtn = ISRoundDown(Num, Keta)
		End Select
	End Function
	
	'*******************************************************************************************
	'         整数チェックルーチン
	'*******************************************************************************************
	' 書式        X = ISInt(CODE)
	'  値         X    … True (．－を含まない整数の場合)  ,  False（それ以外の場合）
	'             CODE … 調べたい文字列
	'
	'*******************************************************************************************
	Public Function ISInt(ByRef code As Object) As Boolean
		ISInt = False
		
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		If IsDbNull(code) Then
			Exit Function
		End If
		
		If Not IsNumeric(code) Then
			Exit Function
		End If
		
		'UPGRADE_WARNING: オブジェクト code の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If InStr(code, ".") <> 0 Then
			Exit Function
		End If
		
		'UPGRADE_WARNING: オブジェクト code の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If InStr(code, "-") <> 0 Then
			Exit Function
		End If
		
		'UPGRADE_WARNING: オブジェクト code の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If InStr(code, "+") <> 0 Then
			Exit Function
		End If
		
		'UPGRADE_WARNING: オブジェクト code の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If InStr(code, "\") <> 0 Then
			Exit Function
		End If
		
		'UPGRADE_WARNING: オブジェクト code の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If InStr(code, "&") <> 0 Then
			Exit Function
		End If
		
		ISInt = True
	End Function
	
	Public Function GetDecimalFormat(ByRef Number As Decimal, Optional ByRef Place As Integer = 2, Optional ByRef DispZero As Boolean = False, Optional ByRef DispComma As Boolean = True) As String
		'***********************************************************
		'機能       小数部の編集を行なう。
		'           小数部がゼロの場合は小数部なしに編集する。
		'
		'引数       Number  : 小数部を編集する値
		'           Place   : 小数部の表示桁数
		'           DispZero: ゼロ表示するか
		'           DispComma: カンマ区切り表示をするか
		'
		'戻り値     編集後の値を文字で返す。
		'***********************************************************
		
		If (Number = ISRoundDown(Number, 0)) Then
			If DispZero Then
				If DispComma Then
					GetDecimalFormat = VB6.Format(Number, "#,##0")
				Else
					GetDecimalFormat = VB6.Format(Number, "0")
				End If
			Else
				If DispComma Then
					GetDecimalFormat = VB6.Format(Number, "#,###")
				Else
					GetDecimalFormat = VB6.Format(Number, "#")
				End If
			End If
		Else
			If DispComma Then
				GetDecimalFormat = VB6.Format(Number, "#,##0." & New String("0", Place))
			Else
				GetDecimalFormat = VB6.Format(Number, "0." & New String("0", Place))
			End If
		End If
	End Function
	
	Public Function CutText(ByRef Text As String, ByRef ByteLength As Integer) As Object
		'***********************************************************
		'機能       文字列をバイト単位で左から切り出す
		'           全角の間で切れてもゴミが残らない
		'           泣き別れ
		'
		'引数       Text        : 切り出す対象の文字列を指定する
		'           ByteLength  : 切り出すバイト数
		'
		'戻り値     文字で返す。
		'***********************************************************
		Dim i As Integer
		Dim TextLength As Integer
		Dim RetText As String
		Dim strChar As String '１文字分の文字
		Dim BLengthCnt As Integer '文字バイト数のカウント
		
		'文字数の取得
		TextLength = Len(Text)
		RetText = vbNullString
		BLengthCnt = 0
		
		For i = 1 To TextLength
			strChar = Mid(Text, i, 1)
			'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
			Select Case LenB(StrConv(strChar, vbFromUnicode))
				Case 1 '半角
					BLengthCnt = BLengthCnt + 1
				Case 2 '全角
					BLengthCnt = BLengthCnt + 2
			End Select
			If BLengthCnt > ByteLength Then Exit For
			RetText = RetText & strChar
		Next 
		
		'UPGRADE_WARNING: オブジェクト CutText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		CutText = RetText
		
	End Function
End Module