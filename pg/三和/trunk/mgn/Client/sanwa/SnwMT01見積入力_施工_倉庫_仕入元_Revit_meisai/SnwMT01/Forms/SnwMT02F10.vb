Option Strict Off
Option Explicit On
Friend Class SnwMT02F10
	Inherits System.Windows.Forms.Form
	
	Private Sub cbCANCEL_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cbCANCEL.Click
		Me.Close()
	End Sub
	
	Private Sub cbOK_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cbOK.Click
		Dim Disp_Spd As String
		Dim i As Short
		
		Disp_Spd = vbNullString
		
		For i = 0 To ck_Disp.UBound
			If Disp_Spd <> vbNullString Then Disp_Spd = Disp_Spd & ","
			Disp_Spd = Disp_Spd & ck_Disp(i).CheckState
		Next 
		
		WriteIni("Disp", "MT02F00_SPD", Disp_Spd, INIFile)
		
		Me.Close()
	End Sub
	
	Private Sub ck_Disp_KeyDown(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyEventArgs) Handles ck_Disp.KeyDown
		Dim KeyCode As Short = eventArgs.KeyCode
		Dim Shift As Short = eventArgs.KeyData \ &H10000
		Dim Index As Short = ck_Disp.GetIndex(eventSender)
		If KeyCode = System.Windows.Forms.Keys.Return Then
			SendTabKey()
		End If
	End Sub
	
	Private Sub SnwMT02F10_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		Dim i As Short
		Dim Disp_Spd As String
		Dim vDisp_Spd As Object
		
		'表示項目設定取得
		'UPGRADE_WARNING: オブジェクト GetIni() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Disp_Spd = GetIni("Disp", "MT02F00_SPD", INIFile)
		
		'UPGRADE_WARNING: オブジェクト vDisp_Spd の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		vDisp_Spd = Split(Disp_Spd, ",")
		
		For i = 0 To UBound(vDisp_Spd)
			'UPGRADE_WARNING: オブジェクト vDisp_Spd(i) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト vDisp_Spd() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			ck_Disp(i).CheckState = vDisp_Spd(i)
		Next 
		
	End Sub
End Class