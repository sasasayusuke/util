Option Strict Off
Option Explicit On

''' <summary>
''' 'Ver.1.00           '2002.04.17
''' 'Ver.1.01           '2003.04.16     Class_Tarminateを付ける
''' </summary>
Friend Class iDate

	'TODO SS Form/Control の継承でも undo は利用できていない

	Dim iDateID As Short
	Dim OwnerLocalID As Short
	Dim ctlY As System.Windows.Forms.Control
	Dim ctlM As System.Windows.Forms.Control
	Dim ctlD As System.Windows.Forms.Control
	Dim Parent As System.Windows.Forms.Form
	Dim SrcFld As String
	Dim Txt As Object
	'UPGRADE_NOTE: Val は Val_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
	Dim Val_Renamed As Object

	'UPGRADE_NOTE: Class_Terminate は Class_Terminate_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
	Private Sub Class_Terminate_Renamed()
		'UPGRADE_NOTE: オブジェクト ctlY をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		ctlY = Nothing
		'UPGRADE_NOTE: オブジェクト ctlM をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		ctlM = Nothing
		'UPGRADE_NOTE: オブジェクト ctlD をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		ctlD = Nothing
		'UPGRADE_NOTE: オブジェクト Parent をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		Parent = Nothing
	End Sub

	Protected Overrides Sub Finalize()
		Class_Terminate_Renamed()
		MyBase.Finalize()
	End Sub

	Sub ISDateDownload(ByRef flds As ADODB.Fields, ByRef ctlNM As String)
		Dim fld As ADODB.Field
		'Dim i As Integer

		For Each fld In flds
			'Download
			If fld.Name = ctlNM Then
				'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
				If Not IsDBNull(fld.Value) Then
					Select Case fld.Type
						Case ADODB.DataTypeEnum.adDate, ADODB.DataTypeEnum.adDBTimeStamp
							'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							ctlY.Text = NullToZero(DatePart(Microsoft.VisualBasic.DateInterval.Year, fld.Value), vbNullString)
							'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							ctlM.Text = NullToZero(DatePart(Microsoft.VisualBasic.DateInterval.Month, fld.Value), vbNullString)
							'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							ctlD.Text = NullToZero(DatePart(Microsoft.VisualBasic.DateInterval.Day, fld.Value), vbNullString)
					End Select
				End If
				Exit For
			End If
		Next fld
	End Sub

	Sub SetupA(ByRef Owner As System.Windows.Forms.Form, ByRef SourceFieldName As String, Optional ByRef LocalID As Short = -1)
		Parent = Owner
		SrcFld = SourceFieldName
		On Error Resume Next
		ctlY = CType(Owner.Controls("tx_" & SourceFieldName & "Y"), Object)
		ctlM = CType(Owner.Controls("tx_" & SourceFieldName & "M"), Object)
		ctlD = CType(Owner.Controls("tx_" & SourceFieldName & "D"), Object)
		On Error GoTo 0
		''    Default = Date
		If LocalID <> -1 Then
			OwnerLocalID = LocalID
		End If
		''    RegistIDC Me
	End Sub

	Sub SetupB(ByRef Owner As System.Windows.Forms.Form, ByRef SourceFieldName As String, ByRef i As Short, Optional ByRef LocalID As Short = -1)
		Parent = Owner
		SrcFld = SourceFieldName
		On Error Resume Next
		ctlY = CType(Owner.Controls("tx_" & SourceFieldName & "Y"), Object)(i)
		ctlM = CType(Owner.Controls("tx_" & SourceFieldName & "M"), Object)(i)
		ctlD = CType(Owner.Controls("tx_" & SourceFieldName & "D"), Object)(i)
		On Error GoTo 0
		''    Default = Date
		If LocalID <> -1 Then
			OwnerLocalID = LocalID
		End If
		''    RegistIDC Me
	End Sub

	Sub Refresh()
		Dim M, Y, d As String
		Dim tmp As Object

		If (Parent Is Nothing) Or SrcFld = "" Then
			Exit Sub
		End If
		If (ctlY Is "") Or (ctlM Is "") Then
			'UPGRADE_WARNING: オブジェクト tmp の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			tmp = ""
		Else
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			Y = NullToZero(ctlY, "").Text
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			M = NullToZero(ctlM, "").Text
			If ctlD Is Nothing Then
				d = CStr(1)
			Else
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				d = NullToZero(ctlD, "").Text
			End If
			If IsDate(CStr(Y) & "/" & CStr(M) & "/" & CStr(d)) = False Then
				'UPGRADE_WARNING: オブジェクト tmp の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				tmp = ""
			Else
				'UPGRADE_WARNING: オブジェクト tmp の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				tmp = DateSerial(CInt(Y), CInt(M), CInt(d)).ToString("yyyy/MM/dd")
			End If
		End If
		'UPGRADE_WARNING: オブジェクト tmp の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If tmp = "" Then
			''        If Not IsNull(Parent(SrcFld)) Then
			''            Parent(SrcFld) = tmp
			''        End If
			''    ElseIf IsNull(Parent(SrcFld)) Then
			''        Parent(SrcFld) = tmp
			''    ElseIf tmp <> Parent(SrcFld) Then
			''        Parent(SrcFld) = tmp
		End If
		'UPGRADE_WARNING: オブジェクト tmp の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト Txt の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Txt = tmp
	End Sub

	ReadOnly Property IsAllNull() As Boolean
		Get
			IsAllNull = False
			Call Refresh()
			'UPGRADE_WARNING: オブジェクト Txt の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If Txt <> "" Then
				Exit Property
			End If
			If ctlY IsNot Nothing Then
				If ctlY.Text <> "" Then
					Exit Property
				End If
			End If
			If ctlM IsNot Nothing Then
				If ctlM.Text <> "" Then
					Exit Property
				End If
			End If
			If ctlD IsNot Nothing Then
				If ctlD.Text <> "" Then
					Exit Property
				End If
			End If
			IsAllNull = True
		End Get
	End Property

	ReadOnly Property ErrorPart() As System.Windows.Forms.Control
		Get
			'-----ErrorPartを使用する際は以下の順序で使用してください。
			''''''            idc(0).ErrorPart.SetFocus
			''''''            idc(0).ErrorPart.Undo
			'-----------------------------------------------------
			''    Call Refresh
			''    If Txt = vbNullString Then
			''        Set ErrorPart = Nothing
			''        Exit Property
			''    End If
			If ctlY IsNot Nothing Then
				If ctlY.Text = "" Then
					ErrorPart = ctlY
					Exit Property
				End If
			End If
			If ctlM IsNot Nothing Then
				Select Case CType(NullToZero(ctlM.Text, 0), Integer)
					Case 1 To 12
					Case Else
						ErrorPart = ctlM
						Exit Property
				End Select
			End If
			ErrorPart = ctlD
		End Get
	End Property

	Property ID() As Short
		Get
			ID = iDateID
		End Get
		Set(ByVal Value As Short)
			iDateID = Value
		End Set
	End Property

	Property LocalID() As Short
		Get
			LocalID = OwnerLocalID
		End Get
		Set(ByVal Value As Short)
			OwnerLocalID = Value
		End Set
	End Property

	Property ControlYear() As System.Windows.Forms.Control
		Get
			ControlYear = ctlY
		End Get
		Set(ByVal Value As System.Windows.Forms.Control)
			ctlY = Value
		End Set
	End Property

	Property ControlMonth() As System.Windows.Forms.Control
		Get
			ControlMonth = ctlM
		End Get
		Set(ByVal Value As System.Windows.Forms.Control)
			ctlM = Value
		End Set
	End Property

	Property ControlDay() As System.Windows.Forms.Control
		Get
			ControlDay = ctlD
		End Get
		Set(ByVal Value As System.Windows.Forms.Control)
			ctlD = Value
		End Set
	End Property

	Property SourceFiled() As String
		Get
			SourceFiled = SrcFld
		End Get
		Set(ByVal Value As String)
			SrcFld = Value
		End Set
	End Property

	Property OwnerForm() As System.Windows.Forms.Form
		Get
			OwnerForm = Parent
		End Get
		Set(ByVal Value As System.Windows.Forms.Form)
			Parent = Value
		End Set
	End Property

	Property TabIndex() As Short
		Get
			If ctlD IsNot Nothing Then
				TabIndex = ctlD.TabIndex
			ElseIf ctlM IsNot Nothing Then
				TabIndex = ctlM.TabIndex
			Else
				TabIndex = ctlY.TabIndex
			End If
		End Get
		Set(ByVal Value As Short)
			If ctlD IsNot Nothing Then
				ctlD.TabIndex = Value
			End If
		End Set
	End Property

	Property Text() As Object
		Get
			Call Refresh()
			'UPGRADE_WARNING: オブジェクト Txt の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト Text の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			Text = Txt
		End Get
		Set(ByVal Value As Object)
			If (Parent IsNot Nothing) And (SrcFld <> "") Then
				''        Parent(SrcFld) = DateText
				If IsDate(Value) Then
					If ctlY IsNot Nothing Then
						'UPGRADE_WARNING: オブジェクト DateText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト ctlY の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						ctlY.Text = Year(Value)
					End If
					If ctlM IsNot Nothing Then
						'UPGRADE_WARNING: オブジェクト DateText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト ctlM の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						ctlM.Text = Month(Value)
					End If
					If ctlD IsNot Nothing Then
						'UPGRADE_WARNING: オブジェクト DateText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト ctlD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						ctlD.Text = DateAndTime.Day(Value)
					End If
				Else
					If ctlY IsNot Nothing Then
						'UPGRADE_WARNING: オブジェクト ctlY の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						ctlY.Text = vbNullString
					End If
					If ctlM IsNot Nothing Then
						'UPGRADE_WARNING: オブジェクト ctlM の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						ctlM.Text = vbNullString
					End If
					If ctlD IsNot Nothing Then
						'UPGRADE_WARNING: オブジェクト ctlD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						ctlD.Text = vbNullString
					End If
				End If
			End If
		End Set
	End Property

	Property Value() As Object
		Get
			Call Refresh()
			'UPGRADE_WARNING: オブジェクト Val_Renamed の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト Value の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			Value = Val_Renamed
		End Get
		Set(ByVal Value As Object)
			If (Parent IsNot Nothing) And (SrcFld <> "") Then
				''        Parent(SrcFld) = DateValue
				If IsDate(Value) Then
					If ctlY IsNot Nothing Then
						'UPGRADE_WARNING: オブジェクト DateValue_Renamed の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト ctlY の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						ctlY.Text = Year(Value)
					End If
					If ctlM IsNot Nothing Then
						'UPGRADE_WARNING: オブジェクト DateValue_Renamed の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト ctlM の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						ctlM.Text = Month(Value)
					End If
					If ctlD IsNot Nothing Then
						'UPGRADE_WARNING: オブジェクト DateValue_Renamed の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト ctlD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						ctlD.Text = DateAndTime.Day(Value)
					End If
				Else
					If ctlY IsNot Nothing Then
						'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト ctlY の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						ctlY.Text = ""
					End If
					If ctlM IsNot Nothing Then
						'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト ctlM の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						ctlM.Text = ""
					End If
					If ctlD IsNot Nothing Then
						'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト ctlD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						ctlD.Text = ""
					End If
				End If
			End If
		End Set
	End Property
End Class