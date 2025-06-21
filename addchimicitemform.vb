Imports System.Data.SqlClient
Imports System.Runtime.CompilerServices.RuntimeHelpers

Public Class AddChimicItemForm
    Private connectionString As String = "Server=localhost;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"

    Private Sub AddChimicItemForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Populate ComboBox with code types
        cmbCodeType.Items.AddRange(New String() {"DY", "LQ", "PW"})
    End Sub

    Private Sub cmbCodeType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCodeType.SelectedIndexChanged
        Dim selectedCodeType As String = cmbCodeType.SelectedItem.ToString()
        Dim nextCode As String = GetNextCode(selectedCodeType)
        lblNextCode.Text = nextCode
        txtCode.Text = nextCode
    End Sub

    Private Function GetNextCode(codeType As String) As String
        Using conn As New SqlConnection(connectionString)
            conn.Open()
            Dim query As String = "SELECT MAX(CAST(SUBSTRING(code, 4, LEN(code) - 3) AS INT)) FROM chimic_items WHERE code LIKE @codeType + '%'"
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@codeType", codeType)
                Dim maxNumber As Object = cmd.ExecuteScalar()
                Dim nextNumber As Integer = If(IsDBNull(maxNumber), 1, Convert.ToInt32(maxNumber) + 1)
                Return $"{codeType}-{nextNumber:D4}"
            End Using
        End Using
    End Function

    Private Sub btnInsert_Click(sender As Object, e As EventArgs) Handles btnInsert.Click
        Dim code As String = txtCode.Text
        Dim productName As String = txtProductName.Text

        If String.IsNullOrEmpty(code) OrElse String.IsNullOrEmpty(productName) Then
            MessageBox.Show("Please enter both code and product name.")
            Return
        End If

        Using conn As New SqlConnection(connectionString)
            conn.Open()

            ' Check if the code or product_name already exists
            Dim checkQuery As String = "SELECT COUNT(*) FROM chimic_items WHERE code = @code OR product_name = @product_name"
            Using checkCmd As New SqlCommand(checkQuery, conn)
                checkCmd.Parameters.AddWithValue("@code", code)
                checkCmd.Parameters.AddWithValue("@product_name", productName)

                Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())
                If count > 0 Then
                    MessageBox.Show("The code or product name already exists.")
                    Return
                End If
            End Using

            ' Insert the new item
            Dim insertQuery As String = "INSERT INTO chimic_items (code, product_name) VALUES (@code, @product_name)"
            Using insertCmd As New SqlCommand(insertQuery, conn)
                insertCmd.Parameters.AddWithValue("@code", code)
                insertCmd.Parameters.AddWithValue("@product_name", productName)

                Try
                    insertCmd.ExecuteNonQuery()
                    MessageBox.Show("Item inserted successfully.")
                    ' Clear the text boxes after successful insertion
                    txtCode.Clear()
                    txtProductName.Clear()
                Catch ex As Exception
                    MessageBox.Show("Error inserting item: " & ex.Message)
                End Try
            End Using
        End Using
    End Sub
End Class
