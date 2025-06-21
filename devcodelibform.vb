Imports System.Data.SqlClient
Imports Excel = Microsoft.Office.Interop.Excel ' For Excel interop
Public Class devcodelibform
    ' SQL Server connection string
    Dim sqlServerConnectionString As String = "Server=localhost;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"
    Private Sub devcodelibform_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        btninsert.Enabled = False
    End Sub
    ' TextChanged Event for TextBoxes
    Private Sub txtPublicName_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtpublicname.TextChanged, txtcode.TextChanged, txtcodeall.TextChanged
        ' Enable btnInsert only if all text fields are filled
        btnInsert.Enabled = Not String.IsNullOrEmpty(txtPublicName.Text) AndAlso _
                           Not String.IsNullOrEmpty(txtCode.Text) AndAlso _
                           Not String.IsNullOrEmpty(txtCodeAll.Text)
    End Sub

    ' Insert Button Click Event
    Private Sub btnInsert_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btninsert.Click
        ' Validate that fields are not empty
        If String.IsNullOrEmpty(txtPublicName.Text) OrElse String.IsNullOrEmpty(txtCode.Text) OrElse String.IsNullOrEmpty(txtCodeAll.Text) Then
            MessageBox.Show("Please write all fields.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' SQL insert query for library table
        Dim insertQuery As String = "INSERT INTO library (name_code, code, lib_code) VALUES (@name_code, @code, @lib_code)"

        ' Open SQL Server connection and execute the insert query
        Using connection As New SqlConnection(sqlServerConnectionString)
            Try
                connection.Open()
                Using cmd As New SqlCommand(insertQuery, connection)
                    cmd.Parameters.AddWithValue("@name_code", txtPublicName.Text.Trim())
                    cmd.Parameters.AddWithValue("@code", txtCode.Text.Trim())
                    cmd.Parameters.AddWithValue("@lib_code", txtCodeAll.Text.Trim())

                    ' Execute the insert command
                    cmd.ExecuteNonQuery()
                End Using



                ' Optional: Clear fields after successful insertion
                txtPublicName.Clear()
                txtCode.Clear()
                txtCodeAll.Clear()

                MessageBox.Show("Record inserted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    ' Helper method to release COM objects
    Private Sub ReleaseObject(ByVal obj As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            obj = Nothing
        Catch ex As Exception
            obj = Nothing
        Finally
            GC.Collect()
        End Try
    End Sub
End Class