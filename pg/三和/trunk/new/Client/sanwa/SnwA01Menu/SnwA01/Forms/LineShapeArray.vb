Option Strict Off
Option Explicit On

Imports System.Windows.Forms
Imports System.ComponentModel
Imports Microsoft.VisualBasic.PowerPacks

<ProvideProperty("Index", GetType(LineShape))>
Friend Class LineShapeArray
    Implements IExtenderProvider

    Private lineShapes As New List(Of LineShape)()

    Public Event [Click] As System.EventHandler

    Public Sub New()
    End Sub

    Public Sub New(ByVal Container As IContainer)
        Container.Add(Me)
    End Sub

    ' Determines whether the provider can extend the specified object
    Public Function CanExtend(ByVal Target As Object) As Boolean Implements IExtenderProvider.CanExtend
        Return TypeOf Target Is LineShape
    End Function

    ' Adds a LineShape to the list at a specified index
    Public Function GetIndex(ByVal o As LineShape) As Short
        Return lineShapes.IndexOf(o)
    End Function

    ' Sets a LineShape's index in the collection
    Public Sub SetIndex(ByVal o As LineShape, ByVal Index As Short)
        If lineShapes.Contains(o) Then
            lineShapes.Remove(o)
        End If
        lineShapes.Insert(Index, o)
    End Sub

    ' Loads a LineShape control at a specified index
    Public Sub Load(ByVal Index As Short)
        If Index >= 0 AndAlso Index < lineShapes.Count Then
            Dim shape = lineShapes(Index)
            CType(shape.Parent, ShapeContainer).Shapes.Add(shape)
        End If
    End Sub

    ' Unloads a LineShape control from a specified index
    Public Sub Unload(ByVal Index As Short)
        If Index >= 0 AndAlso Index < lineShapes.Count Then
            Dim shape = lineShapes(Index)
            CType(shape.Parent, ShapeContainer).Shapes.Remove(shape)
            lineShapes.RemoveAt(Index)
        End If
    End Sub

    ' Accesses a LineShape by index
    Default Public ReadOnly Property Item(ByVal Index As Short) As LineShape
        Get
            Return lineShapes(Index)
        End Get
    End Property

    ' Event hook-up
    Protected Sub HookUpControlEvents(ByVal o As Object)
        Dim ctl As LineShape = CType(o, LineShape)
        AddHandler ctl.Click, AddressOf OnClick
    End Sub

    ' Event handler for Click events
    Private Sub OnClick(sender As Object, e As EventArgs)
        RaiseEvent [Click](sender, e)
    End Sub

End Class