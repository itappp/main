Imports System.Data.SqlClient

Public Class UpdateDeliveryDateForm
    ' SQL Server connection string
    Dim sqlServerConnectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"

    ' List to store all work orders
    Private allWorkOrders As New List(Of String)()

    ' Initialize the form
    Public Sub New()
        InitializeComponent()
        PopulateWorkOrders()
    End Sub

    ' Populate cmbworder with work orders from the database
    Private Sub PopulateWorkOrders()
        Dim query As String = "SELECT DISTINCT worderid FROM techdata"

        Using conn As New SqlConnection(sqlServerConnectionString)
            Dim cmd As New SqlCommand(query, conn)
            Try
                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()

                cmbworder.Items.Clear()
                While reader.Read()
                    cmbworder.Items.Add(reader("worderid").ToString())
                End While
            Catch ex As Exception
                MessageBox.Show("Error loading Work Order IDs: " & ex.Message)
            End Try
        End Using
    End Sub

    ' Event handler for cmbworder SelectedIndexChanged
    Private Sub cmbworder_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbworder.SelectedIndexChanged
        If cmbworder.SelectedItem IsNot Nothing Then
            Dim selectedWorder As String = cmbworder.SelectedItem.ToString()
            FetchDeliveryDate(selectedWorder)
        End If
    End Sub

    ' Fetch Delivery_Dat for the selected work order
    Private Sub FetchDeliveryDate(ByVal worderid As String)
        Dim query As String = "SELECT Delivery_Dat FROM techdata WHERE worderid = @worderid"

        Using connection As New SqlConnection(sqlServerConnectionString)
            Try
                connection.Open()
                Using cmd As New SqlCommand(query, connection)
                    cmd.Parameters.AddWithValue("@worderid", worderid)
                    Dim result As Object = cmd.ExecuteScalar()

                    If result IsNot Nothing Then
                        Dim deliveryDate As DateTime = Convert.ToDateTime(result)
                        lblDeliveryDate.Text = "Delivery Date: " & deliveryDate.ToShortDateString()
                    Else
                        lblDeliveryDate.Text = "Delivery Date: Not found"
                    End If
                End Using
            Catch ex As SqlException
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
    End Sub

    ' Event handler for btnupdate click
    Private Sub btnupdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnupdate.Click
        If cmbworder.SelectedItem IsNot Nothing Then
            Dim selectedWorder As String = cmbworder.SelectedItem.ToString()
            Dim newDeliveryDate As DateTime = dtpDeliveryDate.Value

            ' SQL query to update Delivery_Dat
            Dim query As String = "UPDATE techdata SET Delivery_Dat = @Delivery_Dat WHERE worderid = @worderid"

            Using connection As New SqlConnection(sqlServerConnectionString)
                Try
                    connection.Open()
                    Using cmd As New SqlCommand(query, connection)
                        cmd.Parameters.AddWithValue("@Delivery_Dat", newDeliveryDate)
                        cmd.Parameters.AddWithValue("@worderid", selectedWorder)

                        Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
                        If rowsAffected > 0 Then
                            MessageBox.Show("Delivery date updated successfully.")
                            ' Clear ComboBox and Label after successful update
                            cmbworder.SelectedIndex = -1
                            lblDeliveryDate.Text = ""
                        Else
                            MessageBox.Show("Failed to update delivery date.")
                        End If
                    End Using
                Catch ex As SqlException
                    MessageBox.Show("Error: " & ex.Message)
                End Try
            End Using
        End If
    End Sub

    ' Event handler for cmbworder TextChanged

End Class
