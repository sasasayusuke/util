Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms

<ToolboxItem(False)>
Public Class Twc3DLabel
    Inherits UserControl

    Private LabelA As New Label()
    Private LineTop As New Label()
    Private LineLeft As New Label()
    Private ShapeA As New Panel()

    Private pTopMargin As Integer

    Public Sub New()
        Me.InitializeComponent()
    End Sub


    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        MyBase.OnResize(e)
        Me.ResizeControl()
    End Sub

    Private Sub ResizeControl()
        Me.ShapeA.Location = New Point(0, 0)
        Me.ShapeA.Size = Me.ClientSize

        Me.LineTop.Location = New Point(15, 15)
        Me.LineTop.Size = New Size(Me.ShapeA.Width - 30, 1)

        Me.LineLeft.Location = New Point(15, 15)
        Me.LineLeft.Size = New Size(1, Me.ShapeA.Height - 30)

        ' TODO SS Me.LabelA.Height = Me.TextRenderer.MeasureText(Me.LabelA.Text, Me.LabelA.Font).Height
        Me.LabelA.Width = Math.Max(Me.ShapeA.Width - 140, 0)
        Me.LabelA.Left = 80
        Me.LabelA.Top = (Me.ShapeA.Height / 2 - Me.LabelA.Height / 2) + Me.pTopMargin
    End Sub

    <Browsable(True), Description("Control text alignment.")>
    Public Property Alignment() As ContentAlignment
        Get
            Return Me.LabelA.TextAlign
        End Get
        Set(ByVal value As ContentAlignment)
            Me.LabelA.TextAlign = value
        End Set
    End Property

    <Browsable(True), Description("The text associated with the control.")>
    Public Property Caption() As String
        Get
            Return Me.LabelA.Text
        End Get
        Set(ByVal value As String)
            Me.LabelA.Text = value
        End Set
    End Property

    <Browsable(True), Description("The foreground color of the control.")>
    Public Overrides Property ForeColor() As Color
        Get
            Return Me.LabelA.ForeColor
        End Get
        Set(ByVal value As Color)
            Me.LabelA.ForeColor = value
        End Set
    End Property

    <Browsable(True), Description("The background color of the control.")>
    Public Overrides Property BackColor() As Color
        Get
            Return Me.ShapeA.BackColor
        End Get
        Set(ByVal value As Color)
            Me.ShapeA.BackColor = value
        End Set
    End Property

    <Browsable(True), Description("The font of the text displayed by the control.")>
    Public Overrides Property Font() As Font
        Get
            Return Me.LabelA.Font
        End Get
        Set(ByVal value As Font)
            Me.LabelA.Font = value
            Me.ResizeControl()
        End Set
    End Property

    <Browsable(True), Description("The top margin of the text within the control.")>
    Public Property TopMargin() As Integer
        Get
            Return Me.pTopMargin
        End Get
        Set(ByVal value As Integer)
            Me.pTopMargin = value
            Me.ResizeControl()
        End Set
    End Property

End Class
