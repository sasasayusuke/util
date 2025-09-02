Imports System.ComponentModel

<ToolboxBitmap(GetType(SortableListView), "tbxSortableListView")> _
Public Class SortableListView
    Inherits ListView

    Public Enum SortStyles
        SortDefault
        SortAllColumns
        SortSelectedColumn
    End Enum

    ' The current sort column for selected column sorting.
    Private m_SelectedColumn As Integer = -1

    ' m_SortSubitems(i) is the i-th sub-item
    ' in the sort order for all column sorting.
    Private m_SortSubitems() As Integer = Nothing

    ' Initialize the sort item order to the order given by the column headers.
    Private Sub SetSortSubitems()
        ReDim m_SortSubitems(Me.Columns.Count - 1)
        For i As Integer = 0 To Me.Columns.Count - 1
            m_SortSubitems(Me.Columns(i).DisplayIndex) = i
        Next i
    End Sub

    ' Whether we sort by all columns, one column, or not at all.
    Private m_SortStyle As SortStyles = SortStyles.SortDefault

    Public Property SortStyle() As SortStyles
        Get
            Return m_SortStyle
        End Get
        Set(ByVal value As SortStyles)
            ' If the current style is SortSelectedColumn,
            ' remove the column sort indicator.
            If m_SortStyle = SortStyles.SortSelectedColumn Then
                If m_SelectedColumn >= 0 Then
                    Me.Columns(m_SelectedColumn).ImageKey = Nothing
                    m_SelectedColumn = -1
                End If
            End If

            ' Save the new value.
            m_SortStyle = value

            Select Case m_SortStyle
                Case SortStyles.SortDefault
                    Me.ListViewItemSorter = Nothing
                Case SortStyles.SortAllColumns
                    Me.ListViewItemSorter = New AllColumnSorter()
                Case SortStyles.SortSelectedColumn
                    Me.ListViewItemSorter = New SelectedColumnSorter()
            End Select

            ' Resort.
            ' Me.Sort()
        End Set
    End Property

    ' The user reordered the columns. Resort.
    Protected Overrides Sub OnColumnReordered(ByVal e As System.Windows.Forms.ColumnReorderedEventArgs)
        ' This raises the ColumnReordered event.
        MyBase.OnColumnReordered(e)

        ' If the main program canceled, do nothing.
        If e.Cancel Then Exit Sub

        ' Rebuild the list of sort sub-items.
        SetSortSubitems()

        ' Fix the list up to account for the moved column.
        MoveArrayItem(m_SortSubitems, e.OldDisplayIndex, e.NewDisplayIndex)

        ' Resort.
        Me.Sort()
    End Sub

    ' Move an item from position idx_fr to idx_to.
    Private Sub MoveArrayItem(ByVal values() As Integer, ByVal idx_fr As Integer, ByVal idx_to As Integer)
        Dim moved_value As Integer = values(idx_fr)
        Dim num_moved As Integer = Math.Abs(idx_fr - idx_to)

        If idx_to < idx_fr Then
            Array.Copy(values, idx_to, values, idx_to + 1, num_moved)
        Else
            Array.Copy(values, idx_fr + 1, values, idx_fr, num_moved)
        End If

        values(idx_to) = moved_value
    End Sub

    ' Change the selected sort column.
    Protected Overrides Sub OnColumnClick(ByVal e As System.Windows.Forms.ColumnClickEventArgs)
        MyBase.OnColumnClick(e)

        If Me.SortStyle = SortStyles.SortSelectedColumn Then
            ' If this is the same sort column, switch the sort order.
            If e.Column = m_SelectedColumn Then
                If Me.Sorting = SortOrder.Ascending Then
                    Me.Sorting = SortOrder.Descending
                Else
                    Me.Sorting = SortOrder.Ascending
                End If
            End If

            ' Remove the image from the previous sort column.
            If m_SelectedColumn >= 0 Then
                Me.Columns(m_SelectedColumn).ImageKey = Nothing
            End If

            ' If we're not currently sorting, sort ascending.
            If Me.Sorting = SortOrder.None Then
                Me.Sorting = SortOrder.Ascending
            End If

            ' Save the new sort column and give it an image.
            m_SelectedColumn = e.Column
            If Me.Sorting = SortOrder.Descending Then
                Me.Columns(m_SelectedColumn).ImageKey = "sortDescending.bmp"
            Else
                Me.Columns(m_SelectedColumn).ImageKey = "sortAscending.bmp"
            End If

            ' Resort.
            Me.Sort()
        End If
    End Sub

    ' ---------------------------------------
    ' Sort the ListView items by all columns.
    Private Class AllColumnSorter
        Implements IComparer

        ' Compare two ListViewItems.
        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
            Dim itemx As ListViewItem = DirectCast(x, ListViewItem)
            Dim itemy As ListViewItem = DirectCast(y, ListViewItem)

            ' Compare the items' strings.
            If itemx.ListView.Sorting = SortOrder.Ascending Then
                Return String.Compare(ItemString(itemx), ItemString(itemy))
            Else
                Return -String.Compare(ItemString(itemx), ItemString(itemy))
            End If
        End Function

        ' Return a string representing this item as a
        ' null-separated list of the item sub-item values.
        Private Function ItemString(ByVal listview_item As ListViewItem) As String
            Dim slvw As SortableListView = listview_item.ListView

            ' Make sure we have the sort sub-items' order.
            If slvw.m_SortSubitems Is Nothing Then slvw.SetSortSubitems()

            ' Make an array to hold the sort sub-items' values.
            Dim num_cols As Integer = slvw.Columns.Count
            Dim values(num_cols - 1) As String

            ' Build the list of fields in display order.
            For i As Integer = 0 To slvw.m_SortSubitems.Length - 1
                Dim idx As Integer = slvw.m_SortSubitems(i)

                ' Get this sub-item's value.
                Dim item_value As String = ""
                If idx < listview_item.SubItems.Count Then
                    item_value = listview_item.SubItems(idx).Text
                End If

                ' Align appropriately.
                If slvw.Columns(idx).TextAlign = HorizontalAlignment.Right Then
                    ' Pad so numeric values sort properly.
                    values(i) = item_value.PadLeft(20)
                Else
                    values(i) = item_value
                End If
            Next i

            ' Debug.WriteLine($"|" {values}")

            ' Concatenate the values to build the result.
            Return String.Join(vbNullChar, values)
        End Function
    End Class

    ' ---------------------------------------
    ' Sort the ListView items by the selected column.
    Private Class SelectedColumnSorter
        Implements IComparer

        ' Compare two ListViewItems.
        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
            ' Get the items.
            Dim itemx As ListViewItem = DirectCast(x, ListViewItem)
            Dim itemy As ListViewItem = DirectCast(y, ListViewItem)

            ' Get the selected column index.
            Dim slvw As SortableListView = itemx.ListView
            Dim idx As Integer = slvw.m_SelectedColumn
            If idx < 0 Then Return 0

            ' Compare the sub-items.
            If itemx.ListView.Sorting = SortOrder.Ascending Then
                Return String.Compare(ItemString(itemx, idx), ItemString(itemy, idx))
            Else
                Return -String.Compare(ItemString(itemx, idx), ItemString(itemy, idx))
            End If
        End Function

        ' Return a string representing this item's sub-item.
        Private Function ItemString(ByVal listview_item As ListViewItem, ByVal idx As Integer) As String
            Dim slvw As SortableListView = listview_item.ListView

            ' Make sure the item has the needed sub-item.
            Dim value As String = ""
            If idx <= listview_item.SubItems.Count - 1 Then
                value = listview_item.SubItems(idx).Text
            End If

            ' Return the sub-item's value.
            If slvw.Columns(idx).TextAlign = HorizontalAlignment.Right Then
                ' Pad so numeric values sort properly.
                Return value.PadLeft(20)
            Else
                Return value
            End If
        End Function
    End Class

    Private Sub InitializeComponent()

        Me.SuspendLayout()
        '
        'SortableListView
        '
        Me.ResumeLayout(False)

    End Sub

End Class
