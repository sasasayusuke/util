Option Strict Off
Option Explicit On
Friend Class SnwMT02F13
	Inherits System.Windows.Forms.Form
	
	
	'''Dim ResultCodeGenRituMax As Currency         '原価率上限
	'''Dim ResultCodeGenRituMin As Currency         '原価率下限
	
	''''原価率上限をセット
	'''Property Let ResCodeGenRituMax(ByRef New_GenRituMax As Currency)
	'''    ResultCodeGenRituMax = New_GenRituMax
	'''End Property
	''''原価率下限をセット
	'''Property Let ResCodeGenRituMin(ByRef New_GenRituMin As Currency)
	'''    ResultCodeGenRituMin = New_GenRituMin
	'''End Property
	
	Private Sub SnwMT02F13_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		tx_原価率上限.Text = VB6.Format(SpcToNull(gGenRituMax, 0), "#,##0.00")
		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		tx_原価率下限.Text = VB6.Format(SpcToNull(gGenRituMin, 0), "#,##0.00")
	End Sub
	
	Private Sub cb中止_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cb中止.Click
		Me.Close()
	End Sub
	
	Private Sub cdOK_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cdOK.Click
		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If CDec(SpcToNull(tx_原価率上限, 0)) <= CDec(SpcToNull(tx_原価率下限, 0)) Then
			Inform("下限値が上限値をオーバーしています。")
			Exit Sub
		End If
		
		WriteIni("Limit", "GenRituMax", tx_原価率上限.Text, INIFile)
		gGenRituMax = CDec(tx_原価率上限.Text)
		
		WriteIni("Limit", "GenRituMin", tx_原価率下限.Text, INIFile)
		gGenRituMin = CDec(tx_原価率下限.Text)
		
		Me.Close()
	End Sub
End Class