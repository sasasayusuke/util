Option Strict Off
Option Explicit On
Friend Class Fw_Msg
	Inherits System.Windows.Forms.Form
	'Ver.1.00           不明
	'Ver.1.01           '2003.01.15     ProgMax:ProgValueの型をStringからLongに
	'                                   ProgMaxのGetを設定
	'                                   ｸﾞﾛｰﾊﾞﾙ変数の削除
	'Ver.1.02           2015/10/23  ooswa       ProgValueのGetを設定
	
	
	'システムメニューのウインドウハンドルを取得する
	Private Declare Function GetSystemMenu Lib "user32" (ByVal Hwnd As Integer, ByVal bRevert As Integer) As Integer
	
	'メニュー項目を削除する
	Private Declare Function RemoveMenu Lib "user32" (ByVal hMenu As Integer, ByVal nPosition As Integer, ByVal wFlags As Integer) As Integer
	
	Private Const MF_BYCOMMAND As Integer = &H0 'nPositionはメニュー項目のID値
	Private Const MF_BYPOSITION As Integer = &H400 'nPositionはメニュー項目の一番上を０とした上からの位置
	
	Private Const SC_CLOSE As Integer = &HF060 '閉じる(C)
	
	Dim p_AbortDoing As Boolean
	''Dim p_MeCaption As String                 '2003/01/15 DEL
	''Dim p_StsCaption As String                '2003/01/15 DEL
	
	Private Sub Fw_Msg_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		Dim Hwnd As Integer
		
		p_AbortDoing = False
		lbl_Status.Text = ""
		Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
		
		'システムメニューのウインドウハンドルを取得する
		Hwnd = GetSystemMenu(Me.Handle.ToInt32, 0)
		'システムメニューの項目を削除する
		RemoveMenu(Hwnd, SC_CLOSE, MF_BYCOMMAND)
		
		'--フォームを画面の中央に配置
		Me.Top = VB6.TwipsToPixelsY((VB6.PixelsToTwipsY(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height) - VB6.PixelsToTwipsY(Me.Height)) \ 2)
		Me.Left = VB6.TwipsToPixelsX((VB6.PixelsToTwipsX(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width) - VB6.PixelsToTwipsX(Me.Width)) \ 2)
	End Sub
	
	Private Sub Fw_Msg_MouseMove(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseMove
		Dim Button As Short = eventArgs.Button \ &H100000
		Dim Shift As Short = System.Windows.Forms.Control.ModifierKeys \ &H10000
		Dim X As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim Y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
	End Sub
	
	Private Sub cbAbort_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cbAbort.Click
		If p_AbortDoing = False Then
			[cbAbort].Enabled = False
			p_AbortDoing = True
		End If
	End Sub
	
	Private Sub cbAbort_MouseMove(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles cbAbort.MouseMove
		Dim Button As Short = eventArgs.Button \ &H100000
		Dim Shift As Short = System.Windows.Forms.Control.ModifierKeys \ &H10000
		Dim X As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim Y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		Me.Cursor = System.Windows.Forms.Cursors.Default
	End Sub
	
	ReadOnly Property AbortDoing() As Object
		Get
			'UPGRADE_WARNING: オブジェクト AbortDoing の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			AbortDoing = p_AbortDoing
		End Get
	End Property
	
	WriteOnly Property StsCaption() As String
		Set(ByVal Value As String)
			lbl_Status.Text = Value
		End Set
	End Property
	
	
	Property ProgMax() As Integer
		Get
			ProgMax = ProgBar.Maximum
		End Get
		Set(ByVal Value As Integer)
			ProgBar.Maximum = Value
		End Set
	End Property
	
	'2015/10/23 ADD↓
	'2015/10/23 ADD↑
	
	Property ProgValue() As Integer
		Get
			ProgValue = ProgBar.Value
		End Get
		Set(ByVal Value As Integer)
			If ProgBar.Maximum >= Value Then
				ProgBar.Value = Value
			Else
				ProgBar.Value = ProgBar.Maximum
			End If
		End Set
	End Property
	
	WriteOnly Property ProgVisible() As Boolean
		Set(ByVal Value As Boolean)
			ProgBar.Visible = Value
		End Set
	End Property
	
	WriteOnly Property CancelEnable() As Boolean
		Set(ByVal Value As Boolean)
			[cbAbort].Enabled = Value
		End Set
	End Property
End Class