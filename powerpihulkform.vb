Imports System.Data.SqlClient
Imports System.IO
Imports OfficeOpenXml

Public Class powerpihulkform
    Private sqlServerConnectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"

    ' Function to extract the desired substring
    Private Function ExtractSubstring(input As String) As String
        Dim parts As String() = input.Split("/"c)
        If parts.Length > 1 Then
            If parts(1).Length = 1 Then
                Return String.Join("/", parts.Take(2))
            End If
        End If
        Return parts(0)
    End Function

    ' Method to query finish_inspect and display data in a table
    Private Sub DisplayData()
        Dim data As DataTable = QueryFinishInspect()
        ' Assuming you have a DataGridView named dataGridView1
        dataGridView1.DataSource = data
    End Sub

    ' Query finish_inspect table and apply rules
    Private Function QueryFinishInspect() As DataTable
        Dim table As New DataTable()
        table.Columns.Add("OrderID")
        table.Columns.Add("Weight")
        table.Columns.Add("Height")

        Using connection As New SqlConnection(sqlServerConnectionString)
            connection.Open()
            Dim query As String = "SELECT worder_id, ISNULL(weight, 0) AS weight, height FROM finish_inspect"
            Using command As New SqlCommand(query, connection)
                Using reader As SqlDataReader = command.ExecuteReader()
                    While reader.Read()
                        Dim worder_id As String = reader("worder_id").ToString()
                        Dim weight As Double = Convert.ToDouble(reader("weight"))
                        Dim height As Double = Convert.ToDouble(reader("height"))
                        Dim extracted As String = ExtractSubstring(worder_id)
                        table.Rows.Add(extracted, weight, height)
                    End While
                End Using
            End Using
        End Using

        ' Calculate total length and weight
        Dim totalWeight As Double = table.AsEnumerable().Sum(Function(row) row.Field(Of Double)("Weight"))
        Dim totalHeight As Double = table.AsEnumerable().Sum(Function(row) row.Field(Of Double)("Height"))
        table.Rows.Add("Total", totalWeight, totalHeight)

        Return table
    End Function

    ' Event handler for search button click
    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        DisplayData()
    End Sub
End Class