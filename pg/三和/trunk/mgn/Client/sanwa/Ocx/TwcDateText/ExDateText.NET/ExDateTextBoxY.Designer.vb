<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated(), ToolboxBitmap(GetType(ExDateTextBoxY), "ExDateTextBoxY.ToolboxBitmap")> Partial Class ExDateTextBoxY
#Region "Windows フォーム デザイナによって生成されたコード "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
		'この呼び出しは、Windows フォーム デザイナで必要です。
		InitializeComponent()
	End Sub
	'Form は、コンポーネント一覧に後処理を実行するために dispose をオーバーライドします。
	<System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			UserControl_Terminate()
			If Not components Is Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Windows フォーム デザイナで必要です。
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Friend WithEvents ExDateTextY As System.Windows.Forms.TextBox
	'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
	'Windows フォーム デザイナを使って変更できます。
	'コード エディタを使用して、変更しないでください。
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(ExDateTextBoxY))
		Me.components = New System.ComponentModel.Container()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
		Me.ExDateTextY = New System.Windows.Forms.TextBox
		Me.SuspendLayout()
		Me.ToolTip1.Active = True
		Me.ClientSize = New System.Drawing.Size(45, 34)
		MyBase.Location = New System.Drawing.Point(0, 0)
		MyBase.Name = "ExDateTextBoxY"
		Me.ExDateTextY.AutoSize = False
		Me.ExDateTextY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.ExDateTextY.Size = New System.Drawing.Size(37, 18)
		Me.ExDateTextY.IMEMode = System.Windows.Forms.ImeMode.Disable
		Me.ExDateTextY.Location = New System.Drawing.Point(4, 8)
		Me.ExDateTextY.Maxlength = 4
		Me.ExDateTextY.MultiLine = True
		Me.ExDateTextY.TabIndex = 0
		Me.ExDateTextY.Text = "YYYY" & Chr(13) & Chr(10)
		Me.ExDateTextY.AcceptsReturn = True
		Me.ExDateTextY.BackColor = System.Drawing.SystemColors.Window
		Me.ExDateTextY.CausesValidation = True
		Me.ExDateTextY.Enabled = True
		Me.ExDateTextY.ForeColor = System.Drawing.SystemColors.WindowText
		Me.ExDateTextY.HideSelection = True
		Me.ExDateTextY.ReadOnly = False
		Me.ExDateTextY.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.ExDateTextY.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ExDateTextY.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.ExDateTextY.TabStop = True
		Me.ExDateTextY.Visible = True
		Me.ExDateTextY.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.ExDateTextY.Name = "ExDateTextY"
		Me.Controls.Add(ExDateTextY)
		Me.ResumeLayout(False)
		Me.PerformLayout()
	End Sub
#End Region 
#Region "Upgrade Support"
	<System.Runtime.InteropServices.ProgId("RtnKeyDownEventArgs_NET.RtnKeyDownEventArgs")> Public NotInheritable Class RtnKeyDownEventArgs
		Inherits System.EventArgs
		Public KeyCode As Short
		Public Shift As Short
		Public Cancel As Boolean
		Public Sub New(ByRef KeyCode As Short, ByRef Shift As Short, ByRef Cancel As Boolean)
			MyBase.New()
			Me.KeyCode = KeyCode
			Me.Shift = Shift
			Me.Cancel = Cancel
		End Sub
	End Class
	'UPGRADE_ISSUE: DataObject オブジェクト はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6B85A2A7-FE9F-4FBE-AA0C-CF11AC86A305"' をクリックしてください。
	<System.Runtime.InteropServices.ProgId("OLEStartDragEventArgs_NET.OLEStartDragEventArgs")> Public NotInheritable Class OLEStartDragEventArgs
		Inherits System.EventArgs
		Public Data As DataObject
		Public AllowedEffects As Integer
		Public Sub New(ByRef Data As DataObject, ByRef AllowedEffects As Integer)
			MyBase.New()
			Me.Data = Data
			Me.AllowedEffects = AllowedEffects
		End Sub
	End Class
	'UPGRADE_ISSUE: DataObject オブジェクト はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6B85A2A7-FE9F-4FBE-AA0C-CF11AC86A305"' をクリックしてください。
	<System.Runtime.InteropServices.ProgId("OLESetDataEventArgs_NET.OLESetDataEventArgs")> Public NotInheritable Class OLESetDataEventArgs
		Inherits System.EventArgs
		Public Data As DataObject
		Public DataFormat As Short
		Public Sub New(ByRef Data As DataObject, ByRef DataFormat As Short)
			MyBase.New()
			Me.Data = Data
			Me.DataFormat = DataFormat
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("OLEGiveFeedbackEventArgs_NET.OLEGiveFeedbackEventArgs")> Public NotInheritable Class OLEGiveFeedbackEventArgs
		Inherits System.EventArgs
		Public Effect As Integer
		Public DefaultCursors As Boolean
		Public Sub New(ByRef Effect As Integer, ByRef DefaultCursors As Boolean)
			MyBase.New()
			Me.Effect = Effect
			Me.DefaultCursors = DefaultCursors
		End Sub
	End Class
	'UPGRADE_ISSUE: DataObject オブジェクト はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6B85A2A7-FE9F-4FBE-AA0C-CF11AC86A305"' をクリックしてください。
	<System.Runtime.InteropServices.ProgId("OLEDragOverEventArgs_NET.OLEDragOverEventArgs")> Public NotInheritable Class OLEDragOverEventArgs
		Inherits System.EventArgs
		Public Data As DataObject
		Public Effect As Integer
		Public Button As Short
		Public Shift As Short
		Public X As Single
		Public Y As Single
		Public State As Short
		Public Sub New(ByRef Data As DataObject, ByRef Effect As Integer, ByRef Button As Short, ByRef Shift As Short, ByRef X As Single, ByRef Y As Single, ByRef State As Short)
			MyBase.New()
			Me.Data = Data
			Me.Effect = Effect
			Me.Button = Button
			Me.Shift = Shift
			Me.X = X
			Me.Y = Y
			Me.State = State
		End Sub
	End Class
	'UPGRADE_ISSUE: DataObject オブジェクト はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6B85A2A7-FE9F-4FBE-AA0C-CF11AC86A305"' をクリックしてください。
	<System.Runtime.InteropServices.ProgId("OLEDragDropEventArgs_NET.OLEDragDropEventArgs")> Public NotInheritable Class OLEDragDropEventArgs
		Inherits System.EventArgs
		Public Data As DataObject
		Public Effect As Integer
		Public Button As Short
		Public Shift As Short
		Public X As Single
		Public Y As Single
		Public Sub New(ByRef Data As DataObject, ByRef Effect As Integer, ByRef Button As Short, ByRef Shift As Short, ByRef X As Single, ByRef Y As Single)
			MyBase.New()
			Me.Data = Data
			Me.Effect = Effect
			Me.Button = Button
			Me.Shift = Shift
			Me.X = X
			Me.Y = Y
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("OLECompleteDragEventArgs_NET.OLECompleteDragEventArgs")> Public NotInheritable Class OLECompleteDragEventArgs
		Inherits System.EventArgs
		Public Effect As Integer
		Public Sub New(ByRef Effect As Integer)
			MyBase.New()
			Me.Effect = Effect
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("MouseUpEventArgs_NET.MouseUpEventArgs")> Public NotInheritable Class MouseUpEventArgs
		Inherits System.EventArgs
		Public Button As Short
		Public Shift As Short
		Public X As Single
		Public Y As Single
		Public Sub New(ByRef Button As Short, ByRef Shift As Short, ByRef X As Single, ByRef Y As Single)
			MyBase.New()
			Me.Button = Button
			Me.Shift = Shift
			Me.X = X
			Me.Y = Y
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("MouseMoveEventArgs_NET.MouseMoveEventArgs")> Public NotInheritable Class MouseMoveEventArgs
		Inherits System.EventArgs
		Public Button As Short
		Public Shift As Short
		Public X As Single
		Public Y As Single
		Public Sub New(ByRef Button As Short, ByRef Shift As Short, ByRef X As Single, ByRef Y As Single)
			MyBase.New()
			Me.Button = Button
			Me.Shift = Shift
			Me.X = X
			Me.Y = Y
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("MouseDownEventArgs_NET.MouseDownEventArgs")> Public NotInheritable Class MouseDownEventArgs
		Inherits System.EventArgs
		Public Button As Short
		Public Shift As Short
		Public X As Single
		Public Y As Single
		Public Sub New(ByRef Button As Short, ByRef Shift As Short, ByRef X As Single, ByRef Y As Single)
			MyBase.New()
			Me.Button = Button
			Me.Shift = Shift
			Me.X = X
			Me.Y = Y
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("KeyUpEventArgs_NET.KeyUpEventArgs")> Public NotInheritable Class KeyUpEventArgs
		Inherits System.EventArgs
		Public KeyCode As Short
		Public Shift As Short
		Public Sub New(ByRef KeyCode As Short, ByRef Shift As Short)
			MyBase.New()
			Me.KeyCode = KeyCode
			Me.Shift = Shift
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("KeyPressEventArgs_NET.KeyPressEventArgs")> Public NotInheritable Class KeyPressEventArgs
		Inherits System.EventArgs
		Public KeyAscii As Short
		Public Sub New(ByRef KeyAscii As Short)
			MyBase.New()
			Me.KeyAscii = KeyAscii
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("KeyDownEventArgs_NET.KeyDownEventArgs")> Public NotInheritable Class KeyDownEventArgs
		Inherits System.EventArgs
		Public KeyCode As Short
		Public Shift As Short
		Public Sub New(ByRef KeyCode As Short, ByRef Shift As Short)
			MyBase.New()
			Me.KeyCode = KeyCode
			Me.Shift = Shift
		End Sub
	End Class
#End Region 
End Class