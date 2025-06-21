Imports System.Data.SqlClient

Public Class rawinspectadddefectsform

    Private sqlServerConnectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"

    Private Sub btninsert_Click(sender As Object, e As EventArgs) Handles btninsert.Click
        Dim queryCheck As String = "SELECT COUNT(*) FROM gray_defects WHERE name_ar = @name_ar AND raw = 1"
        Dim queryInsert As String = "INSERT INTO gray_defects (raw, name_ar) VALUES (@raw, @name_ar)"

        Using connection As New SqlConnection(sqlServerConnectionString)
            Using commandCheck As New SqlCommand(queryCheck, connection)
                commandCheck.Parameters.AddWithValue("@name_ar", txtdefect.Text)

                connection.Open()
                Dim count As Integer = Convert.ToInt32(commandCheck.ExecuteScalar())

                If count > 0 Then
                    MessageBox.Show("The name already exists in the database.")
                Else
                    Using commandInsert As New SqlCommand(queryInsert, connection)
                        commandInsert.Parameters.AddWithValue("@raw", 1)
                        commandInsert.Parameters.AddWithValue("@name_ar", txtdefect.Text)

                        commandInsert.ExecuteNonQuery()
                        MessageBox.Show("Record inserted successfully")
                    End Using
                End If
            End Using
        End Using
    End Sub


End Class
