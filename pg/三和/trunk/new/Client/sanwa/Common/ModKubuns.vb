Option Strict On
Option Explicit On

''' <summary>
''' 2023/04/18 業種区分追加
''' </summary>
Public Module ModKubuns

	'物件種別名
	Public Function Get物件種別名(ByRef No As Object) As String

		Select Case NullToZero(No).ToString
			Case "0"
				Get物件種別名 = "新店"
			Case "1"
				Get物件種別名 = "改装"
			Case "2"
				Get物件種別名 = "ﾒﾝﾃ"
			Case "3"
				Get物件種別名 = "補充"
			Case "4"
				Get物件種別名 = "内装"
			Case "5"
				Get物件種別名 = "その他"
			Case "6"
				Get物件種別名 = "委託" '2022/12/08 ADD
			Case Else
				Get物件種別名 = ""
		End Select

	End Function

	'物件種別名(正式名称)
	Public Function Get物件種別名L(ByRef No As Object) As String

		Select Case NullToZero(No).ToString
			Case "0"
				Get物件種別名L = "新店"
			Case "1"
				Get物件種別名L = "改装"
			Case "2"
				Get物件種別名L = "メンテナンス"
			Case "3"
				Get物件種別名L = "補充（しまむら用）"
			Case "4"
				Get物件種別名L = "内装"
			Case "5"
				Get物件種別名L = "その他"
			Case "6"
				Get物件種別名L = "委託" '2022/12/08 ADD
			Case Else
				Get物件種別名L = ""
		End Select

	End Function

	'作業区分
	Public Function Get作業区分名(ByRef No As String) As String

		Select Case No
			Case ""
				Get作業区分名 = vbNullString
			Case "1"
				''            Get作業区分名 = "施工"
				Get作業区分名 = "工事" '2022/09/02 ADD
			Case "2"
				Get作業区分名 = "コール"
			Case "3"
				Get作業区分名 = "内装"
			Case "4"
				Get作業区分名 = "ｸﾚｰﾑ"
			Case Else
				Get作業区分名 = vbNullString
		End Select
	End Function

	'ウエルシアリース区分
	Public Function Getウエルシアリース区分名(ByRef No As String) As String

		Select Case No
			Case ""
				Getウエルシアリース区分名 = vbNullString
			Case "1"
				Getウエルシアリース区分名 = "通常"
			Case "2"
				Getウエルシアリース区分名 = "リース"
			Case Else
				Getウエルシアリース区分名 = vbNullString
		End Select
	End Function

	'2023/04/18 ADD
	'業種区分
	Public Function Get業種区分名(ByRef No As String) As String

		Select Case No
			Case ""
				Get業種区分名 = vbNullString
			Case "0"
				Get業種区分名 = "什器"
			Case "1"
				Get業種区分名 = "内装"
			Case Else
				Get業種区分名 = vbNullString
		End Select
	End Function

End Module