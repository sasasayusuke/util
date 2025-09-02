Option Strict Off
Option Explicit On

Imports System.Windows.Forms
Imports System.ComponentModel
Imports Microsoft.VisualBasic.PowerPacks

<ProvideProperty("Index", GetType(RectangleShape))>
Friend Class RectangleShapeArray
    Implements IExtenderProvider

    Private rectangleShapes As New List(Of RectangleShape)()

    Public Event [Click] As EventHandler

    Public Sub New()
    End Sub

    Public Sub New(ByVal Container As IContainer)
        Container.Add(Me)
    End Sub

    ' Determines whether the provider can extend the specified object
    Public Function CanExtend(ByVal Target As Object) As Boolean Implements IExtenderProvider.CanExtend
        Return TypeOf Target Is RectangleShape
    End Function

    ' Retrieves the index of a RectangleShape in the collection
    Public Function GetIndex(ByVal o As RectangleShape) As Short
        Return CType(rectangleShapes.IndexOf(o), Short)
    End Function

    ' Sets the index of a RectangleShape in the collection
    Public Sub SetIndex(ByVal o As RectangleShape, ByVal Index As Short)
        If rectangleShapes.Contains(o) Then
            rectangleShapes.Remove(o)
        End If
        rectangleShapes.Insert(Index, o)
    End Sub

    ' Loads a RectangleShape into the ShapeContainer
    Public Sub Load(ByVal Index As Short)
        If Index >= 0 AndAlso Index < rectangleShapes.Count Then
            Dim shape = rectangleShapes(Index)
            CType(shape.Parent, ShapeContainer).Shapes.Add(shape)
        End If
    End Sub

    ' Unloads a RectangleShape from the ShapeContainer
    Public Sub Unload(ByVal Index As Short)
        If Index >= 0 AndAlso Index < rectangleShapes.Count Then
            Dim shape = rectangleShapes(Index)
            CType(shape.Parent, ShapeContainer).Shapes.Remove(shape)
            rectangleShapes.RemoveAt(Index)
        End If
    End Sub

    ' Provides access to RectangleShape instances by index
    Default Public ReadOnly Property Item(ByVal Index As Short) As RectangleShape
        Get
            Return rectangleShapes(Index)
        End Get
    End Property

    ' Sets up the Click event for RectangleShape instances
    Protected Sub HookUpControlEvents(ByVal o As Object)
        Dim ctl As RectangleShape = CType(o, RectangleShape)
        AddHandler ctl.Click, AddressOf OnClick
    End Sub

    ' Event handler for Click events
    Private Sub OnClick(sender As Object, e As EventArgs)
        RaiseEvent [Click](sender, e)
    End Sub

End Class