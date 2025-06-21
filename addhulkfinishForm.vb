Imports System.Data.SqlClient

Public Class addhulkfinishForm
    Private connectionsqlString As String = "Server=localhost;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"

    Private Sub addhulkfinishForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' Access the logged-in username from the global variable
        lblUsername.Text = "Logged in as: " & LoggedInUsername
        ' Retrieve the public_name for the logged-in user
        Dim publicName As String = GetPublicName(LoggedInUsername)
    End Sub

  
    Private Sub btnInsert_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnInsert.Click
        ' Validate inputs
        If String.IsNullOrWhiteSpace(txtkg.Text) OrElse Not IsNumeric(txtkg.Text) Then
            MessageBox.Show("Weight (kg) must be a valid number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Retrieve the values from the labels
        Dim contractNo As String = lblContractNo.Text
        Dim refNo As String = lblRefNo.Text
        Dim batchNo As String = lblBatchNo.Text
        Dim clientCode As String = lblClientCode.Text
        Dim color As String = lblColor.Text
        Dim productName As String = lblProductName.Text

        ' Retrieve other values from the form
        Dim worderId As String = txtworderid.Text
        Dim weight As Decimal = Convert.ToDecimal(txtkg.Text)
        Dim height As Decimal = If(String.IsNullOrWhiteSpace(txtm.Text), 0, Convert.ToDecimal(txtm.Text))
        Dim username As String = LoggedInUsername
        Dim weightpk As Decimal = weight
        Dim heightpk As Decimal = height

        Using connection As New SqlConnection(connectionsqlString)
            Try
                connection.Open()

                ' Step 1: Insert into store_finish table with the values from labels and textboxes
                Dim insertQuery As String = "INSERT INTO store_finish (worder_id, height, weight, roll, gid, fabric_grade,transaction_date, username, heightpk, weightpk, contract_no, ref_no, batch_no, client_code, color, product_name) " &
                                             "VALUES (@worder_id, @height, @weight, @roll, @gid, @fabric_grade,@transaction_date, @username, @heightpk, @weightpk, @contract_no, @ref_no, @batch_no, @client_code, @color, @product_name)"
                Using cmd As New SqlCommand(insertQuery, connection)
                    cmd.Parameters.Add("@worder_id", SqlDbType.NVarChar).Value = worderId
                    cmd.Parameters.Add("@height", SqlDbType.Decimal).Value = height
                    cmd.Parameters.Add("@weight", SqlDbType.Decimal).Value = weight
                    cmd.Parameters.Add("@roll", SqlDbType.Int).Value = 0 ' Set roll to 0
                    cmd.Parameters.Add("@gid", SqlDbType.Int).Value = 0 ' Set gid to 0
                    cmd.Parameters.Add("@fabric_grade", SqlDbType.Int).Value = 5
                    cmd.Parameters.Add("@transaction_date", SqlDbType.DateTime).Value = DateTime.Now
                    cmd.Parameters.Add("@username", SqlDbType.NVarChar).Value = username
                    cmd.Parameters.Add("@heightpk", SqlDbType.Decimal).Value = height
                    cmd.Parameters.Add("@weightpk", SqlDbType.Decimal).Value = weight
                    cmd.Parameters.Add("@contract_no", SqlDbType.NVarChar).Value = contractNo
                    cmd.Parameters.Add("@ref_no", SqlDbType.NVarChar).Value = refNo
                    cmd.Parameters.Add("@batch_no", SqlDbType.NVarChar).Value = batchNo
                    cmd.Parameters.Add("@client_code", SqlDbType.NVarChar).Value = clientCode
                    cmd.Parameters.Add("@color", SqlDbType.NVarChar).Value = color
                    cmd.Parameters.Add("@product_name", SqlDbType.NVarChar).Value = productName

                    Dim rowsInserted As Integer = cmd.ExecuteNonQuery()
                    If rowsInserted > 0 Then
                        MessageBox.Show("Record inserted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

                        ' Clear textboxes and labels
                        ClearInputs()
                    Else
                        MessageBox.Show("Failed to insert record.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                End Using
            Catch ex As SqlException
                MessageBox.Show("A database error occurred: " & ex.Message, "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Catch ex As Exception
                MessageBox.Show("An unexpected error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                connection.Close()
            End Try
        End Using
    End Sub

    ' Method to clear textboxes and labels
    Private Sub ClearInputs()
        txtworderid.Text = ""
        txtkg.Text = ""
        txtm.Text = ""
        lblContractNo.Text = ""
        lblRefNo.Text = ""
        lblBatchNo.Text = ""
        lblClientCode.Text = ""
        lblColor.Text = ""
        lblProductName.Text = ""
    End Sub

    Private Function GetPublicName(ByVal username As String) As String
        Dim publicName As String = String.Empty
        Try
            Using conn As New SqlConnection(connectionsqlString)
                ' SQL query to get the public_name from dep_users where username matches
                Dim query As String = "SELECT public_name FROM dep_users WHERE username = @username"
                Dim cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@username", username)

                conn.Open()
                ' Execute the query and retrieve the public_name
                Dim result = cmd.ExecuteScalar()
                If result IsNot Nothing Then
                    publicName = result.ToString()
                End If
                conn.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error retrieving public name: " & ex.Message)
        End Try
        Return publicName
    End Function
End Class
