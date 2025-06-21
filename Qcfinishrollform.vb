Imports System.Data.SqlClient
Imports Excel = Microsoft.Office.Interop.Excel
Imports OfficeOpenXml
Imports System.IO
Imports System.Runtime.InteropServices
Public Class Qcfinishrollform

    ' Corrected SQL Server connection string
    Private sqlServerConnectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"
    Private headerCheckbox As CheckBox = New CheckBox()

    Private Sub Qcfinishrollform_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Access the logged-in username from the global variable

        lblUsername.Text = "Logged in as: " & LoggedInUsername
        ' Retrieve the public_name for the logged-in user
        Dim publicName As String = GetPublicName(LoggedInUsername)
        ' Initialize ComboBoxes and Button


        ' Populate ComboBoxes after loading data into dgvResults

        LoadClientCodes()
    
    End Sub
    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        ' Validate controls initialization
        If dgvresults Is Nothing Then
            MessageBox.Show("DataGridView not initialized.")
            Return
        End If

        Dim worderId As String = txtworderid.Text
        Dim contractNo As String = txtContractNo.Text
        Dim batchNo As String = txtbatch.Text ' Get batch number from txtBatch
        Dim fromDate As DateTime = dtpfromdate.Value.Date
        Dim toDate As DateTime = dtptodate.Value.Date
        Dim clientCode As String = String.Empty

        If cmbClient.SelectedItem IsNot Nothing Then
            clientCode = cmbClient.SelectedItem.ToString()
        End If

        Dim conditions As New List(Of String)

        ' Handle worderId input
        Dim worderIds As String() = If(Not String.IsNullOrEmpty(worderId), worderId.Split(","c), New String() {})
        If worderIds.Length > 0 Then
            ' Add condition for each worderId (each part of the array)
            Dim worderConditions As New List(Of String)
            For i As Integer = 0 To worderIds.Length - 1
                worderConditions.Add("pf.worder_id = @worderId_" & i)
            Next
            conditions.Add("(" & String.Join(" OR ", worderConditions) & ")")
        End If

        ' Handle contractNo input
        Dim contractNos As String() = If(Not String.IsNullOrEmpty(contractNo), contractNo.Split(","c), New String() {})
        If contractNos.Length > 0 Then
            ' Add condition for each contractNo (each part of the array)
            Dim contractConditions As New List(Of String)
            For i As Integer = 0 To contractNos.Length - 1
                contractConditions.Add("pf.contract_no = @contractNo_" & i)
            Next
            conditions.Add("(" & String.Join(" OR ", contractConditions) & ")")
        End If

        ' Handle batchNo input
        Dim batchNos As String() = If(Not String.IsNullOrEmpty(batchNo), batchNo.Split(","c), New String() {})
        If batchNos.Length > 0 Then
            ' Add condition for each batchNo (each part of the array)
            Dim batchConditions As New List(Of String)
            For i As Integer = 0 To batchNos.Length - 1
                batchConditions.Add("pf.batch_no = @batchNo_" & i)
            Next
            conditions.Add("(" & String.Join(" OR ", batchConditions) & ")")
        End If

        ' Add condition for clientCode if not empty
        If Not String.IsNullOrEmpty(clientCode) Then
            conditions.Add("pf.client_code = @clientCode")
        End If

        ' Handle date range conditions
        If fromDate <> DateTime.MinValue AndAlso toDate <> DateTime.MinValue Then
            conditions.Add("CAST(pf.transaction_date AS DATE) BETWEEN @fromDate AND @toDate")
        ElseIf fromDate <> DateTime.MinValue Then
            conditions.Add("CAST(pf.transaction_date AS DATE) >= @fromDate")
        ElseIf toDate <> DateTime.MinValue Then
            conditions.Add("CAST(pf.transaction_date AS DATE) <= @toDate")
        End If

        ' Add condition to exclude rows where height is 0
        conditions.Add("pf.height > 0")

        ' Combine all conditions into the WHERE clause
        Dim whereClause As String = If(conditions.Count > 0, "WHERE " & String.Join(" AND ", conditions), "")

        ' SQL query with the dynamic WHERE clause
        Dim query As String = "SELECT id, worder_id as 'أمر شغل', roll as 'رقم التوب', qc_roll as 'توافق اللون',contract_no as 'رقم التعاقد', batch_no as 'رقم الرسالة', ref_no as 'رقم الإذن', client_code as 'كود العميل ', transaction_date as 'تاريخ المخزن', height as 'الطول', weight as 'الوزن', fabric_grade as 'درجه القماش', color as 'اللون', product_name as 'الخامة' FROM store_finish pf " & whereClause & " GROUP BY id, worder_id, contract_no, roll, qc_roll,batch_no, ref_no, client_code, inspection_date, transaction_date, height, weight, fabric_grade, color, product_name"

        ' Pass parameters to LoadData
        LoadData(query, worderIds, contractNos, batchNos, clientCode, fromDate, toDate)


        ApplyDataGridViewStyles()
       
    End Sub

    Private Sub LoadData(ByVal query As String, ByVal worderIds As String(), ByVal contractNos As String(), ByVal batchNos As String(), ByVal clientCode As String, ByVal fromDate As Date, ByVal toDate As Date)
        Using connection As New SqlConnection(sqlServerConnectionString)
            Dim cmd As New SqlCommand(query, connection)

            ' Bind parameters for worderIds dynamically
            For i As Integer = 0 To worderIds.Length - 1
                cmd.Parameters.AddWithValue("@worderId_" & i, worderIds(i))
            Next

            ' Bind parameters for contractNos dynamically
            For i As Integer = 0 To contractNos.Length - 1
                cmd.Parameters.AddWithValue("@contractNo_" & i, contractNos(i))
            Next

            ' Bind parameters for batchNos dynamically
            For i As Integer = 0 To batchNos.Length - 1
                cmd.Parameters.AddWithValue("@batchNo_" & i, batchNos(i))
            Next

            ' Bind other parameters
            If Not String.IsNullOrEmpty(clientCode) Then
                cmd.Parameters.AddWithValue("@clientCode", clientCode)
            End If
            If fromDate <> DateTime.MinValue Then
                cmd.Parameters.AddWithValue("@fromDate", fromDate)
            End If
            If toDate <> DateTime.MinValue Then
                cmd.Parameters.AddWithValue("@toDate", toDate)
            End If

            ' Execute command and populate dgvResults
            Try
                connection.Open()
                Dim adapter As New SqlDataAdapter(cmd)
                Dim table As New DataTable()
                adapter.Fill(table)
                dgvresults.DataSource = table

            Catch ex As Exception
                MessageBox.Show("An error occurred: " & ex.Message)
            End Try
        End Using
    End Sub
    
  
    Private selectAllCheckBox As CheckBox

    Private Sub ApplyDataGridViewStyles()
        ' Add checkbox column for selecting individual rows if it doesn't exist
        If Not dgvresults.Columns.Contains("select_checkbox") Then
            Dim checkboxColumn As New DataGridViewCheckBoxColumn()
            checkboxColumn.Name = "select_checkbox"
            checkboxColumn.HeaderText = "Select"
            checkboxColumn.Width = 50 ' Adjust width as needed
            dgvresults.Columns.Insert(0, checkboxColumn) ' Insert it at the first column position
        End If

        ' Add "Select All" checkbox to the header only if it doesn't already exist
        If selectAllCheckBox Is Nothing Then
            selectAllCheckBox = New CheckBox()
            selectAllCheckBox.Size = New Size(15, 15)
            Dim headerCellRect As Rectangle = dgvresults.GetCellDisplayRectangle(0, -1, True)
            selectAllCheckBox.Location = New Point(headerCellRect.Location.X + headerCellRect.Width / 2 - selectAllCheckBox.Width / 2, headerCellRect.Location.Y + 4)
            selectAllCheckBox.BackColor = Color.Transparent
            dgvresults.Controls.Add(selectAllCheckBox)

            ' Set font and alignment for the DataGridView content
            dgvresults.DefaultCellStyle.Font = New Font("Arial", 12, FontStyle.Bold)
            dgvresults.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter


        End If
        ' Set the width for other columns (as per your previous code)
        If dgvresults.Columns.Contains("id") Then
            dgvresults.Columns("id").Width = 20
        End If
        If dgvresults.Columns.Contains("worder_id") Then
            dgvresults.Columns("worder_id").Width = 170
        End If
        If dgvresults.Columns.Contains("contract_no") Then
            dgvresults.Columns("contract_no").Width = 150
        End If
        If dgvresults.Columns.Contains("batch_no") Then
            dgvresults.Columns("batch_no").Width = 100
        End If
        If dgvresults.Columns.Contains("ref_no") Then
            dgvresults.Columns("ref_no").Width = 130
        End If
        If dgvresults.Columns.Contains("roll") Then
            dgvresults.Columns("roll").Width = 20
        End If
        If dgvresults.Columns.Contains("client_code") Then
            dgvresults.Columns("client_code").Width = 100
        End If
        If dgvresults.Columns.Contains("transaction_date") Then
            dgvresults.Columns("transaction_date").Width = 140
        End If

        If dgvresults.Columns.Contains("height") Then
            dgvresults.Columns("height").Width = 20
        End If
        If dgvresults.Columns.Contains("weight") Then
            dgvresults.Columns("weight").Width = 20
        End If
        If dgvresults.Columns.Contains("fabric_grade") Then
            dgvresults.Columns("fabric_grade").Width = 20
        End If
        If dgvresults.Columns.Contains("color") Then
            dgvresults.Columns("color").Width = 150
        End If
        If dgvresults.Columns.Contains("product_name") Then
            dgvresults.Columns("product_name").Width = 150
        End If
        If dgvresults.Columns.Contains("username") Then
            dgvresults.Columns("username").Width = 80
        End If

    End Sub
    Private Sub dgvresults_CellValueChanged(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs)
        ' Check if the changed cell is the checkbox column
        If e.ColumnIndex = dgvresults.Columns("select_checkbox").Index Then

        End If
    End Sub
    Private Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdate.Click
        ' Check DataGridView
        If dgvresults Is Nothing OrElse dgvresults.Rows.Count = 0 Then
            MessageBox.Show("No data loaded or DataGridView is empty.")
            Return
        End If

        Dim isAnyRowUpdated As Boolean = False ' Track if any row is updated
        Dim currentDateTime As DateTime = DateTime.Now ' Get current DateTime

        ' Process each selected row
        For Each row As DataGridViewRow In dgvresults.Rows
            Try
                ' Check if the row is selected
                Dim isSelected As Boolean = Convert.ToBoolean(row.Cells("select_checkbox").Value)
                If isSelected Then
                    ' Retrieve the ID and qc_roll value
                    Dim id As Integer = Convert.ToInt32(row.Cells("id").Value)
                    Dim qcRoll As String = row.Cells("توافق اللون").Value.ToString().Trim()

                    ' Update both qc_roll and qc_date columns for the specific ID
                    Dim query As String = "UPDATE store_finish " & _
                                          "SET qc_roll = @qcRoll, qc_date = @qcDate, qc_user = @qcUser " & _
                                          "WHERE id = @id"

                    Using connection As New SqlConnection(sqlServerConnectionString)
                        Dim cmd As New SqlCommand(query, connection)
                        cmd.Parameters.AddWithValue("@qcRoll", qcRoll)
                        cmd.Parameters.AddWithValue("@qcDate", currentDateTime)
                        cmd.Parameters.AddWithValue("@qcUser", LoggedInUsername)
                        cmd.Parameters.AddWithValue("@id", id)

                        connection.Open()
                        cmd.ExecuteNonQuery()
                        isAnyRowUpdated = True ' Mark as updated
                    End Using
                End If
            Catch ex As Exception
                MessageBox.Show("Error processing row: " & ex.Message)
            End Try
        Next

        ' Check if any row was updated
        If isAnyRowUpdated Then
            MessageBox.Show("Selected rows updated successfully.")

            ' Clear DataGridView data source and reset ComboBox selection
            dgvresults.DataSource = Nothing
            cmbClient.SelectedIndex = -1
        Else
            MessageBox.Show("No rows were selected for update.")
        End If
    End Sub





    Private Sub SelectAllCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim isChecked As Boolean = selectAllCheckBox.Checked
        For Each row As DataGridViewRow In dgvresults.Rows
            row.Cells("select_checkbox").Value = isChecked
        Next
        dgvresults.RefreshEdit()

    End Sub
    Private Function AreHeightAndWeightValid(ByVal selectedId As Integer) As Boolean
        ' Check if the height and weight are zero in the store_finish table
        Dim height As Decimal
        Dim weight As Decimal

        ' Query to get height and weight from store_finish based on the selected ID
        Using connection As New SqlConnection(sqlServerConnectionString)
            Dim query As String = "SELECT height, weight FROM store_finish WHERE id = @selectedId"
            Dim cmd As New SqlCommand(query, connection)
            cmd.Parameters.AddWithValue("@selectedId", selectedId)

            Try
                connection.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                If reader.HasRows Then
                    reader.Read()
                    height = Convert.ToDecimal(reader("height"))
                    weight = Convert.ToDecimal(reader("weight"))

                    ' Check if either height or weight is zero
                    If height = 0 Or weight = 0 Then
                        Return False ' Invalid, don't proceed with the insert/update
                    End If
                Else
                    Return False ' No data found for the selected ID
                End If
            Catch ex As Exception
                MessageBox.Show("An error occurred while validating height and weight: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End Try
        End Using

        Return True ' Height and weight are valid (not zero)
    End Function
    Private Sub LoadClientCodes()
        Using connection As New SqlConnection(sqlServerConnectionString)
            Dim query As String = "SELECT DISTINCT client_code FROM store_finish"
            Dim cmd As New SqlCommand(query, connection)

            Try
                connection.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                While reader.Read()
                    cmbClient.Items.Add(reader("client_code").ToString())
                End While
                reader.Close()
            Catch ex As Exception
                MessageBox.Show("An error occurred while loading client codes: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Function GetPublicName(ByVal username As String) As String
        Dim publicName As String = String.Empty
        Try
            Using conn As New SqlConnection(sqlServerConnectionString)
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
    Private Sub LogActivity(ByVal action As String, ByVal username As String, ByVal selectedId As Integer)
        ' Create a log message that combines the action and other relevant details
        Dim logMessage As String = String.Format("User '{0}'  action: {1} on record with ID {2} at {3}",
                                                  username, action, selectedId, DateTime.Now)

        ' Define the SQL query to insert the activity log
        Dim query As String = "INSERT INTO activity_logs (log_message) VALUES (@logMessage)"

        Using connection As New SqlConnection(sqlServerConnectionString)
            Using command As New SqlCommand(query, connection)
                ' Add parameter to prevent SQL injection
                command.Parameters.AddWithValue("@logMessage", logMessage)

                Try
                    connection.Open()
                    command.ExecuteNonQuery() ' Execute the insert command
                Catch ex As Exception
                    MessageBox.Show("Error logging activity: " & ex.Message)
                End Try
            End Using
        End Using
    End Sub

   
End Class

