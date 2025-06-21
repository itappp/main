Imports System.Data.SqlClient
Imports System.Drawing ' Required for Font

Public Class UpdateCodeOnPOForm
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
        DisplayCodeAndLibCode(txtworderid.Text)

    End Sub

    Private Sub DisplayCodeAndLibCode(ByVal worderid As String)
        ' SQL query to retrieve code, lib_code, and speed based on the entered Worder ID
        Dim query As String = "SELECT techdata.worderid, library.code, library.lib_code, speedproccess.speed " &
                              "FROM techdata " &
                              "LEFT JOIN library ON techdata.code_lib = library.id " &
                              "LEFT JOIN speedproccess ON techdata.worderid = speedproccess.worderid " &
                              "WHERE techdata.worderid = @worderid"

        ' Clear previous data
        lbloldcode.Text = String.Empty
        dgvoldcode.Rows.Clear()
        dgvoldcode.Columns.Clear()

        ' Add columns for lib_code and speed
        dgvoldcode.Columns.Add("LibCode", "المكتبة")


        ' Set columns to autosize based on content
        dgvoldcode.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells

        ' Set font to bold, size 12 and align content to center
        Dim boldFont As New Font("Microsoft Sans Serif", 8, FontStyle.Bold)
        dgvoldcode.Columns("LibCode").DefaultCellStyle.Font = boldFont
        dgvoldcode.Columns("Speed").DefaultCellStyle.Font = boldFont
        dgvoldcode.Columns("LibCode").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvoldcode.Columns("Speed").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        ' Set header style
        Dim headerFont As New Font("Microsoft Sans Serif", 8, FontStyle.Bold)
        dgvoldcode.ColumnHeadersDefaultCellStyle.Font = headerFont
        dgvoldcode.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        Using conn As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(query, conn)
                ' Add parameter to prevent SQL injection
                cmd.Parameters.AddWithValue("@worderid", worderid)

                Try
                    conn.Open()
                    Dim reader As SqlDataReader = cmd.ExecuteReader()

                    ' Check if records are returned
                    If reader.HasRows Then
                        While reader.Read()
                            ' Display code in lbloldcode
                            lbloldcode.Text = reader("code").ToString()

                            ' Get lib_code and speed
                            Dim libCode As String = reader("lib_code").ToString()
                            Dim speed As String = reader("speed").ToString()

                            ' Split libCode and speed strings by "-"
                            Dim libCodeItems As String() = libCode.Split("-"c)
                            Dim speedItems As String() = speed.Split("-"c)

                            ' Ensure both arrays have the same length before displaying
                            Dim maxLength As Integer = Math.Max(libCodeItems.Length, speedItems.Length)

                            ' Loop through each item and add as rows
                            For i As Integer = 0 To maxLength - 1
                                Dim libCodeValue As String = If(i < libCodeItems.Length, libCodeItems(i), "")
                                Dim speedValue As String = If(i < speedItems.Length, speedItems(i), "")
                                dgvoldcode.Rows.Add(libCodeValue, speedValue)
                            Next
                        End While
                    Else
                        MessageBox.Show("No records found for the entered Worder ID.")
                    End If

                    reader.Close()
                Catch ex As Exception
                    MessageBox.Show("Error retrieving data: " & ex.Message)
                End Try
            End Using
        End Using
    End Sub
    ' Event handler for form load to populate cmbcodelib
    Private Sub UpdateCodeOnPOForm_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        PopulateCode()
        StyleDataGridView() ' Apply the styling when the form loads

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

    ' Method to populate cmbcodelib with library codes and their corresponding ids
    Private Sub PopulateCode(Optional ByVal filter As String = "")
        Dim query As String = "SELECT id, code FROM lib"
        If Not String.IsNullOrEmpty(filter) Then
            query &= " WHERE code LIKE @filter" ' Add filter to query if specified
        End If

        Using conn As New SqlConnection(connectionString)
            Try
                conn.Open()

                Using cmd As New SqlCommand(query, conn)
                    If Not String.IsNullOrEmpty(filter) Then
                        cmd.Parameters.AddWithValue("@filter", filter & "%") ' Filter by starting letter
                    End If

                    Dim reader As SqlDataReader = cmd.ExecuteReader()

                    cmbcodelib.Items.Clear()

                    ' Populate cmbcodelib with KeyValuePairs of id and code
                    While reader.Read()
                        Dim code As String = reader("code").ToString()
                        Dim id As Integer = Convert.ToInt32(reader("id"))
                        cmbcodelib.Items.Add(New KeyValuePair(Of Integer, String)(id, code))
                    End While

                    ' Set display and value members for cmbcodelib
                    cmbcodelib.DisplayMember = "Value"  ' Display the 'code' in the combo box
                    cmbcodelib.ValueMember = "Key"      ' Access the 'id' value
                End Using

            Catch ex As SqlException
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
    End Sub

    ' Event handler for txtworderid TextChanged to filter cmbcodelib
    Private Sub txtworderid_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtworderid.TextChanged
        Dim input As String = txtworderid.Text.Trim()

        If input.StartsWith("k", StringComparison.OrdinalIgnoreCase) Then
            PopulateCode("k") ' Filter cmbcodelib for codes starting with "k"
        ElseIf input.StartsWith("w", StringComparison.OrdinalIgnoreCase) Then
            PopulateCode("w") ' Filter cmbcodelib for codes starting with "w"
        Else
            PopulateCode() ' Load all codes if no specific filter
        End If
    End Sub

    ' Event handler for cmbcodelib selection change
    Private Sub cmbcodelib_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbcodelib.SelectedIndexChanged
        If cmbcodelib.SelectedItem IsNot Nothing Then
            Dim selectedItem As KeyValuePair(Of Integer, String) = CType(cmbcodelib.SelectedItem, KeyValuePair(Of Integer, String))
            lblidlibcode.Text = selectedItem.Key.ToString() ' Display the selected id
            FetchLibCodeData(selectedItem.Key) ' Fetch and display lib_code data in dgvLibCode
        End If
    End Sub

    ' Method to fetch and display lib_code data based on selected id
    Private Sub FetchLibCodeData(ByVal libraryId As Integer)
        Dim query As String = "SELECT lib_code FROM library WHERE id = @id"

        Using conn As New SqlConnection(connectionString)
            Try
                conn.Open()

                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@id", libraryId)

                    Dim result As Object = cmd.ExecuteScalar()

                    If result IsNot Nothing Then
                        Dim libCodeItems As String() = result.ToString().Split("-"c)

                        dgvLibCode.Rows.Clear()
                        dgvLibCode.Columns.Clear()

                        ' Add columns for lib_code and speed
                        dgvLibCode.Columns.Add("LibCodeColumn", "Library Code")
                        dgvLibCode.Columns.Add("SpeedColumn", "Speed")

                        ' Populate dgvLibCode with lib_code items
                        For Each item As String In libCodeItems
                            dgvLibCode.Rows.Add(item.Trim(), "")
                        Next

                        dgvLibCode.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
                    Else
                        dgvLibCode.Rows.Clear()
                    End If
                End Using

            Catch ex As SqlException
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
    End Sub

    ' Method to apply styles to dgvLibCode
    Private Sub StyleDataGridView()
        ' Set header style
        dgvLibCode.ColumnHeadersDefaultCellStyle.Font = New Font("Microsoft Sans Serif", 10, FontStyle.Bold)
        dgvLibCode.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        ' Set cell style
        dgvLibCode.DefaultCellStyle.Font = New Font("Microsoft Sans Serif", 8, FontStyle.Bold)
        dgvLibCode.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
    End Sub
  ' Event handler for btnupdate click
    Private Sub btnupdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnupdate.Click

        If String.IsNullOrWhiteSpace(lbloldcode.Text) Then
            MessageBox.Show("The old code field cannot be empty. Please select a valid old code.")
            Return
        End If

        ' Validate input
        If cmbcodelib.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a library code.")
            Return
        End If

        If String.IsNullOrWhiteSpace(txtworderid.Text) Then
            MessageBox.Show("Please enter a work order ID.")
            Return
        End If

        If String.IsNullOrWhiteSpace(txtreason.Text) Then
            MessageBox.Show("Please enter a reason for the update.")
            Return
        End If



        ' Get selected library ID and work order ID
        Dim selectedLibraryId As Integer = CType(cmbcodelib.SelectedItem, KeyValuePair(Of Integer, String)).Key
        Dim newCode As String = CType(cmbcodelib.SelectedItem, KeyValuePair(Of Integer, String)).Value
        Dim workOrderId As String = txtworderid.Text.Trim()
        Dim reason As String = txtreason.Text.Trim()

        ' Variables to store old values
        Dim oldCode As String = lbloldcode.Text
        Dim timestamp As String = DateTime.Now.ToString("M/d/yyyy h:mm:ss tt") ' Format timestamp

        Using conn As New SqlConnection(connectionString)
            Try
                conn.Open()

                ' Step 1: Update the techdata table
                Dim updateQuery As String = "UPDATE techdata SET code_lib = @newCodeLib WHERE worderid = @worderid"
                Using cmd As New SqlCommand(updateQuery, conn)
                    cmd.Parameters.AddWithValue("@newCodeLib", selectedLibraryId)
                    cmd.Parameters.AddWithValue("@worderid", workOrderId)
                    cmd.ExecuteNonQuery()
                End Using
                Dim updateSpeedProcessQuery As String = "UPDATE speedproccess SET code_lib = @code_lib, speed = @speed WHERE worderid = @worderid"
                ' Update speedproccess table
                Using cmd As New SqlCommand(updateSpeedProcessQuery, conn)
                    Dim speedValues As New List(Of String)()
                    For Each row As DataGridViewRow In dgvLibCode.Rows
                        If row.Cells("LibCodeColumn").Value Is Nothing OrElse String.IsNullOrWhiteSpace(row.Cells("LibCodeColumn").Value.ToString()) Then Exit For
                        Dim speed As String = If(row.Cells("SpeedColumn").Value IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(row.Cells("SpeedColumn").Value.ToString()),
                                                 row.Cells("SpeedColumn").Value.ToString(), "0").Trim()
                        speedValues.Add(speed)
                    Next
                    Dim speedString As String = String.Join("-", speedValues)

                    cmd.Parameters.AddWithValue("@code_lib", selectedLibraryId)
                    cmd.Parameters.AddWithValue("@speed", speedString)
                    cmd.Parameters.AddWithValue("@worderid", workOrderId)
                    cmd.ExecuteNonQuery()
                End Using

                ' Retrieve the username from lblUsername
                Dim userName As String = lblUsername.Text.Replace("Logged in as: ", "").Trim()
                If String.IsNullOrWhiteSpace(userName) Then
                    MessageBox.Show("Username is not available.")
                    Return
                End If

                ' Query to retrieve public_name and department from dep_users
                Dim publicName As String = ""
                Dim department As String = ""
                Dim selectUserQuery As String = "SELECT public_name, department FROM dep_users WHERE username = @username"

                Using selectCmd As New SqlCommand(selectUserQuery, conn)
                    selectCmd.Parameters.AddWithValue("@username", userName)
                    Using reader As SqlDataReader = selectCmd.ExecuteReader()
                        If reader.Read() Then
                            publicName = reader("public_name").ToString()
                            department = reader("department").ToString()
                        Else
                            MessageBox.Show("User information not found in dep_users.")
                            Return
                        End If
                    End Using
                End Using

                ' Insert log entry into activity_logs
                Dim insertLogQuery As String = "INSERT INTO activity_logs (oldcodelib, newcodelib, worderid, username, department, reason, timestamp, kindtrans) " &
                                               "VALUES (@oldCodeLib, @newCodeLib, @worderid, @username, @department, @reason, @timestamp, @kindtrans)"
                Using cmd As New SqlCommand(insertLogQuery, conn)
                    cmd.Parameters.AddWithValue("@oldCodeLib", oldCode)
                    cmd.Parameters.AddWithValue("@newCodeLib", newCode)
                    cmd.Parameters.AddWithValue("@worderid", workOrderId)
                    cmd.Parameters.AddWithValue("@username", publicName) ' Insert public_name as username
                    cmd.Parameters.AddWithValue("@department", department) ' Insert department
                    cmd.Parameters.AddWithValue("@reason", reason)
                    cmd.Parameters.AddWithValue("@timestamp", timestamp)
                    cmd.Parameters.AddWithValue("@kindtrans", "تعديل كود المكتبه") ' Add the specific text
                    cmd.ExecuteNonQuery()
                End Using

                cmbcodelib.SelectedIndex = -1 ' Clear selection in the ComboBox
                txtreason.Clear() ' Clear the TextBox
                dgvLibCode.Rows.Clear() ' Clear all rows in the DataGridView
                dgvoldcode.Rows.Clear()
                lblidlibcode.Text = String.Empty ' Clear lblIdLibCode
                lbloldcode.Text = String.Empty ' Clear lblOldCode
                ' Confirmation message
                MessageBox.Show("Update and log entry successful!")
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
    End Sub



End Class

