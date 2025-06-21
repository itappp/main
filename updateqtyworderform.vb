Imports System.Data.SqlClient
Imports System.Drawing ' Required for Font

Public Class updateqtyworderform

    ' Connection string for your database
    Private connectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"

    ' Event handler for Search button click
    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        ' Check if worderid is entered
        If String.IsNullOrWhiteSpace(txtworderid.Text) Then
            MessageBox.Show("Please enter a Worder ID.")
            Return
        End If

        ' Call the function to search and display results
        Displayoldqty(txtworderid.Text)

    End Sub
    Private Sub Displayoldqty(ByVal worderid As String)
        ' SQL query to retrieve data for the given Worder ID
        Dim query As String = "SELECT worderid AS 'Worderid', qty_m AS 'الكمية متر', qty_kg AS 'الكمية كيلو' " & _
                              "FROM techdata " & _
                              "WHERE techdata.worderid = @worderid"

        ' Clear existing rows and columns in the DataGridView
        dgvoldqty.Rows.Clear()
        dgvoldqty.Columns.Clear()

        ' Establish connection and retrieve data
        Using connection As New SqlConnection(connectionString)
            Try
                ' Open the connection
                connection.Open()

                ' Create a SQL command
                Using command As New SqlCommand(query, connection)
                    ' Add the Worder ID as a parameter to prevent SQL injection
                    command.Parameters.AddWithValue("@worderid", worderid)

                    ' Execute the command and read the data
                    Using reader As SqlDataReader = command.ExecuteReader()
                        ' Check if the reader has any columns
                        If reader.HasRows Then
                            ' Add columns to DataGridView based on the data
                            For i As Integer = 0 To reader.FieldCount - 1
                                Dim column As New DataGridViewTextBoxColumn()
                                column.HeaderText = reader.GetName(i)
                                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                                column.DefaultCellStyle.Font = New Font("Arial", 10, FontStyle.Bold)
                                dgvoldqty.Columns.Add(column)
                            Next

                            ' Style the header
                            dgvoldqty.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                            dgvoldqty.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 10, FontStyle.Bold)

                            ' Populate rows with data from the reader
                            While reader.Read()
                                Dim row As Object() = New Object(reader.FieldCount - 1) {}
                                reader.GetValues(row)
                                dgvoldqty.Rows.Add(row)
                            End While

                            ' Apply the same style to all cells in the DataGridView
                            dgvoldqty.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                            dgvoldqty.DefaultCellStyle.Font = New Font("Arial", 10, FontStyle.Bold)
                        Else
                            MessageBox.Show("No data found for the specified Worder ID.")
                        End If
                    End Using
                End Using
            Catch ex As Exception
                MessageBox.Show("Error retrieving data: " & ex.Message)
            End Try
        End Using
    End Sub
    Private Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnupdate.Click
        ' Validate input
        If String.IsNullOrWhiteSpace(txtworderid.Text) OrElse String.IsNullOrWhiteSpace(txtReason.Text) Then
            MessageBox.Show("Please provide a Worder ID and reason for the update.")
            Return
        End If

        Dim worderId As String = txtworderid.Text.Trim()
        Dim reason As String = txtReason.Text.Trim()
        Dim timestamp As String = DateTime.Now.ToString("M/d/yyyy h:mm:ss tt")
        Dim userName As String = lblUsername.Text.Replace("Logged in as: ", "").Trim()

        ' Ensure username is available
        If String.IsNullOrWhiteSpace(userName) Then
            MessageBox.Show("Username is not available.")
            Return
        End If

        ' Variables to hold old and new quantities
        Dim oldqtym As Decimal = 0, oldqtykg As Decimal = 0
        Dim newqtym As Decimal, newqtykg As Decimal

        ' Establish connection to database
        Using connection As New SqlConnection(connectionString)
            Try
                connection.Open()

                ' Retrieve old values
                Dim selectQuery As String = "SELECT qty_m, qty_kg FROM techdata WHERE worderid = @worderid"
                Using selectCmd As New SqlCommand(selectQuery, connection)
                    selectCmd.Parameters.AddWithValue("@worderid", worderId)
                    Using reader As SqlDataReader = selectCmd.ExecuteReader()
                        If reader.Read() Then
                            oldqtym = Convert.ToDecimal(reader("qty_m"))
                            oldqtykg = Convert.ToDecimal(reader("qty_kg"))
                        Else
                            MessageBox.Show("No record found for the specified Worder ID.")
                            Return
                        End If
                    End Using
                End Using

                ' Perform update
                If Decimal.TryParse(txtNewQtyM.Text, newqtym) AndAlso Decimal.TryParse(txtNewQtyKg.Text, newqtykg) Then
                    Dim updateQuery As String = "UPDATE techdata SET qty_m = @qty_m, qty_kg = @qty_kg WHERE worderid = @worderid"
                    Using updateCmd As New SqlCommand(updateQuery, connection)
                        updateCmd.Parameters.AddWithValue("@qty_m", newqtym)
                        updateCmd.Parameters.AddWithValue("@qty_kg", newqtykg)
                        updateCmd.Parameters.AddWithValue("@worderid", worderId)
                        updateCmd.ExecuteNonQuery()
                    End Using
                Else
                    MessageBox.Show("Invalid quantity values provided.")
                    Return
                End If

                ' Retrieve user details
                Dim publicName As String = ""
                Dim department As String = ""
                Dim selectUserQuery As String = "SELECT public_name, department FROM dep_users WHERE username = @username"
                Using userCmd As New SqlCommand(selectUserQuery, connection)
                    userCmd.Parameters.AddWithValue("@username", userName)
                    Using reader As SqlDataReader = userCmd.ExecuteReader()
                        If reader.Read() Then
                            publicName = reader("public_name").ToString()
                            department = reader("department").ToString()
                        Else
                            MessageBox.Show("User information not found.")
                            Return
                        End If
                    End Using
                End Using

                ' Log the update
                Dim logQuery As String = "INSERT INTO activity_logs (oldqtykg, newqtykg, oldqtym, newqtym, worderid, username, department, reason, timestamp, kindtrans) " &
                                         "VALUES (@oldqtykg, @newqtykg, @oldqtym, @newqtym, @worderid, @username, @department, @reason, @timestamp, @kindtrans)"
                Using logCmd As New SqlCommand(logQuery, connection)
                    logCmd.Parameters.AddWithValue("@oldqtykg", oldqtykg)
                    logCmd.Parameters.AddWithValue("@newqtykg", newqtykg)
                    logCmd.Parameters.AddWithValue("@oldqtym", oldqtym)
                    logCmd.Parameters.AddWithValue("@newqtym", newqtym)
                    logCmd.Parameters.AddWithValue("@worderid", worderId)
                    logCmd.Parameters.AddWithValue("@username", publicName)
                    logCmd.Parameters.AddWithValue("@department", department)
                    logCmd.Parameters.AddWithValue("@reason", reason)
                    logCmd.Parameters.AddWithValue("@timestamp", timestamp)
                    logCmd.Parameters.AddWithValue("@kindtrans", "تغيير كميات أمر شغل")
                    logCmd.ExecuteNonQuery()
                End Using
                txtNewQtyM.Clear()
                txtNewQtyKg.Clear()
                txtworderid.Clear()
                txtReason.Clear()
                dgvoldqty.Rows.Clear()
                MessageBox.Show("Data updated and logged successfully.")
            Catch ex As SqlException
                MessageBox.Show("SQL Error: " & ex.Message)
            Catch ex As Exception
                MessageBox.Show("Error updating data: " & ex.Message)
            End Try
        End Using
    End Sub




    Private Sub updateqtyworderform_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Access the logged-in username from the global variable
        lblUsername.Text = "Logged in as: " & LoggedInUsername
        ' Set the user access level based on logged-in username
        SetUserAccessLevel(LoggedInUsername)
    End Sub
    ' Assume this function is called during login to set the UserAccessLevel
    Private Sub SetUserAccessLevel(ByVal username As String)
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim query As String = "SELECT acc_level FROM dep_users WHERE username = @username"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@username", username)
                    Dim accLevel As Object = cmd.ExecuteScalar()
                    If accLevel IsNot Nothing Then
                        UserAccessLevel = CInt(accLevel)
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error retrieving user access level: " & ex.Message)
        End Try
    End Sub
End Class

