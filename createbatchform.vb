Imports System.Data.SqlClient
Imports System.Net
Imports System.Net.Mail

Public Class createbatchform
    Private connectionString As String = "Server=localhost;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"
    Private isClientLoaded As Boolean = False
    Private isSupplierLoaded As Boolean = False
    Private isStyleLoaded As Boolean = False
    Private isServiceLoaded As Boolean = False

    Private Sub createbatchform_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Fetch and set the next batch number
        lblbatch.Text = GetNextBatchNumber()

        ' Populate combo boxes
        LoadClientCodes()
        LoadSuppliersCodes()
        LoadFabricTypes()
        Loadstyle()
        Loadservice()

        ' Enable AutoComplete for combo boxes
        EnableAutoComplete(cmbclient)
        EnableAutoComplete(cmbsupplier)
        EnableAutoComplete(cmbkindfabric)
        EnableAutoComplete(cmbstyle)
        EnableAutoComplete(cmbservice)

        lblUsername.Text = "Logged in as: " & LoggedInUsername
        ' Retrieve the public_name for the logged-in user
        Dim publicName As String = GetPublicName(LoggedInUsername)
    End Sub

    Private Function GetNextBatchNumber() As String
        Dim nextBatchNumber As String = String.Empty
        Try
            Using conn As New SqlConnection(connectionString)
                Dim query As String = "SELECT TOP 1 batch FROM batch_raw ORDER BY batch DESC"
                Dim cmd As New SqlCommand(query, conn)

                ' Open the connection
                conn.Open()

                ' Execute the query and get the result
                Dim result = cmd.ExecuteScalar()
                If result IsNot Nothing Then
                    nextBatchNumber = (Convert.ToInt32(result) + 1).ToString()
                Else
                    nextBatchNumber = "1" ' If no batches exist, start with 1
                End If

                conn.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error fetching next batch number: " & ex.Message)
        End Try

        Return nextBatchNumber
    End Function

    Private Sub Loadservice()
        Try
            Using conn As New SqlConnection(connectionString)
                Dim query As String = "SELECT id,service_ar FROM kind_service" ' Adjust the query to get id, code, and name
                Dim cmd As New SqlCommand(query, conn)
                Dim dt As New DataTable()

                ' Open the connection
                conn.Open()

                ' Fill the DataTable with the result of the query
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                dt.Load(reader)

                ' Set the ComboBox properties
                cmbservice.DataSource = dt
                cmbservice.DisplayMember = "service_ar"  ' Column to display
                cmbservice.ValueMember = "id"      ' Value to store
                cmbservice.SelectedIndex = -1
                conn.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading style: " & ex.Message)
        End Try

        ' Mark client ComboBox as loaded
        isServiceLoaded = True
    End Sub
    Private Sub Loadstyle()
        Try
            Using conn As New SqlConnection(connectionString)
                Dim query As String = "SELECT id, code, name FROM style" ' Adjust the query to get id, code, and name
                Dim cmd As New SqlCommand(query, conn)
                Dim dt As New DataTable()

                ' Open the connection
                conn.Open()

                ' Fill the DataTable with the result of the query
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                dt.Load(reader)

                ' Set the ComboBox properties
                cmbstyle.DataSource = dt
                cmbstyle.DisplayMember = "code"  ' Column to display
                cmbstyle.ValueMember = "id"      ' Value to store
                cmbstyle.SelectedIndex = -1
                conn.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading style: " & ex.Message)
        End Try

        ' Mark client ComboBox as loaded
        isStyleLoaded = True
    End Sub

    Private Sub LoadClientCodes()
        Try
            Using conn As New SqlConnection(connectionString)
                Dim query As String = "SELECT id, code, name FROM Clients" ' Adjust the query to get id, code, and name
                Dim cmd As New SqlCommand(query, conn)
                Dim dt As New DataTable()

                ' Open the connection
                conn.Open()

                ' Fill the DataTable with the result of the query
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                dt.Load(reader)

                ' Set the ComboBox properties
                cmbclient.DataSource = dt
                cmbclient.DisplayMember = "code"  ' Column to display
                cmbclient.ValueMember = "id"      ' Value to store
                cmbclient.SelectedIndex = -1
                conn.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading client codes: " & ex.Message)
        End Try

        ' Mark client ComboBox as loaded
        isClientLoaded = True
    End Sub

    Private Sub LoadSuppliersCodes()
        Try
            Using conn As New SqlConnection(connectionString)
                Dim query As String = "SELECT id, code, name FROM suppliers" ' Adjust the query to get id, code, and name
                Dim cmd As New SqlCommand(query, conn)
                Dim dt As New DataTable()

                ' Open the connection
                conn.Open()

                ' Fill the DataTable with the result of the query
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                dt.Load(reader)

                ' Set the ComboBox properties
                cmbsupplier.DataSource = dt
                cmbsupplier.DisplayMember = "name"  ' Column to display
                cmbsupplier.ValueMember = "id"      ' Value to store
                cmbsupplier.SelectedIndex = -1
                conn.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading supplier codes: " & ex.Message)
        End Try

        ' Mark supplier ComboBox as loaded
        isSupplierLoaded = True
    End Sub

    Private Sub cmbservice_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbservice.SelectedIndexChanged
        If isServiceLoaded AndAlso cmbservice.SelectedIndex <> -1 Then
            Dim selectedServiceId As Integer = Convert.ToInt32(cmbservice.SelectedValue)
            If selectedServiceId = 2 OrElse selectedServiceId = 4 Then
                cmbsupplier.Enabled = False
                cmbsupplier.SelectedIndex = -1 ' Clear the selection
            Else
                cmbsupplier.Enabled = True
            End If
        End If
    End Sub

    Private Sub cmbclient_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbclient.SelectedIndexChanged
        If isClientLoaded AndAlso cmbclient.SelectedIndex <> -1 Then
            Dim selectedRow As DataRowView = DirectCast(cmbclient.SelectedItem, DataRowView)
            lblclient.Text = "عميل: " & selectedRow("name").ToString()
        End If
    End Sub

    Private Sub cmbsupplier_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbsupplier.SelectedIndexChanged
        If isSupplierLoaded AndAlso cmbsupplier.SelectedIndex <> -1 Then
            Dim selectedRow As DataRowView = DirectCast(cmbsupplier.SelectedItem, DataRowView)
            lblsup.Text = "مورد: " & selectedRow("code").ToString()
        End If
    End Sub
    Private Sub cmbstyle_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbstyle.SelectedIndexChanged
        If isStyleLoaded AndAlso cmbstyle.SelectedIndex <> -1 Then
            Dim selectedRow As DataRowView = DirectCast(cmbstyle.SelectedItem, DataRowView)
            lblstyle.Text = "الخامة: " & selectedRow("name").ToString()
        End If
    End Sub

    Private Sub LoadFabricTypes()
        Try
            Using conn As New SqlConnection(connectionString)
                Dim query As String = "SELECT id, fabrictype_ar FROM fabric" ' Query to fetch fabric id and fabrictype_ar
                Dim cmd As New SqlCommand(query, conn)
                Dim dt As New DataTable()

                ' Open the connection
                conn.Open()

                ' Fill the DataTable with the result of the query
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                dt.Load(reader)

                ' Set the ComboBox properties
                cmbkindfabric.DataSource = dt
                cmbkindfabric.DisplayMember = "fabrictype_ar"  ' Column to display
                cmbkindfabric.ValueMember = "id"               ' Value to store
                cmbkindfabric.SelectedIndex = -1
                conn.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading fabric types: " & ex.Message)
        End Try
    End Sub

    Private Sub EnableAutoComplete(cmb As ComboBox)
        cmb.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cmb.AutoCompleteSource = AutoCompleteSource.ListItems
    End Sub

    Private Sub btninsert_Click(sender As Object, e As EventArgs) Handles btninsert.Click
        ' Check if all ComboBoxes have a selected value
        If cmbclient.SelectedIndex = -1 OrElse cmbservice.SelectedIndex = -1 OrElse cmbkindfabric.SelectedIndex = -1 OrElse String.IsNullOrEmpty(txtmaterial.Text) Then
            MessageBox.Show("Please select a value for all fields and enter material before inserting.")
            Return
        End If

        ' Check if the batch number already exists
        If BatchNumberExists(Convert.ToInt32(lblbatch.Text)) Then
            MessageBox.Show("Batch number already exists. Please generate a new batch number.")
            Return
        End If
        Dim poNumber As String = If(String.IsNullOrEmpty(txtpo.Text), String.Empty, txtpo.Text)
        Dim material As String = If(String.IsNullOrEmpty(txtmaterial.Text), String.Empty, txtmaterial.Text)

        Try
            Using conn As New SqlConnection(connectionString)
                Dim query As String = "INSERT INTO batch_raw (datetrans, batch, sup_code, client_code, fabric_type, style_id,service_id,po_number,material,username) VALUES (@datetrans, @batch, @sup_code, @client_code, @fabric_type, @style_id,@service_id,@ponumber,@material,@username)"
                Dim cmd As New SqlCommand(query, conn)

                ' Set the parameters
                cmd.Parameters.AddWithValue("@datetrans", DateTime.Now)
                cmd.Parameters.AddWithValue("@batch", Convert.ToInt32(lblbatch.Text))
                cmd.Parameters.AddWithValue("@sup_code", If(cmbsupplier.SelectedValue Is Nothing, DBNull.Value, Convert.ToInt32(cmbsupplier.SelectedValue)))
                cmd.Parameters.AddWithValue("@client_code", Convert.ToInt32(cmbclient.SelectedValue))
                cmd.Parameters.AddWithValue("@fabric_type", Convert.ToInt32(cmbkindfabric.SelectedValue))
                cmd.Parameters.AddWithValue("@style_id", Convert.ToInt32(cmbstyle.SelectedValue))
                cmd.Parameters.AddWithValue("@service_id", Convert.ToInt32(cmbservice.SelectedValue))
                cmd.Parameters.AddWithValue("@ponumber", poNumber)
                cmd.Parameters.AddWithValue("@material", material)
                cmd.Parameters.AddWithValue("@username", LoggedInUsername)

                ' Open the connection
                conn.Open()

                ' Execute the query
                cmd.ExecuteNonQuery()

                ' Close the connection
                conn.Close()

                MessageBox.Show("Batch inserted successfully.")

                ' Send email notification
                Dim subject As String = "Batch Inserted Successfully"
                Dim body As String = $"تم انشاء رساله رقم {lblbatch.Text} لكود عميل {cmbclient.Text} ونوع الخدمة {cmbservice.Text} والخامة {cmbkindfabric.Text} و PO Number {poNumber}."
                SendEmailNotification(subject, body)

                ' Disable ComboBoxes after successful insertion
                cmbclient.Enabled = False
                cmbsupplier.Enabled = False
                cmbkindfabric.Enabled = False
                cmbstyle.Enabled = False
                btninsert.Enabled = False
                cmbservice.Enabled = False
                txtpo.Enabled = False
                txtmaterial.Enabled = False

                ' Show and populate DataGridView
                ShowAndPopulateDataGridView()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error inserting batch: " & ex.Message)
        End Try
    End Sub


    Private Function BatchNumberExists(batchNumber As Integer) As Boolean
        Dim exists As Boolean = False
        Try
            Using conn As New SqlConnection(connectionString)
                Dim query As String = "SELECT COUNT(*) FROM batch_raw WHERE batch = @batch"
                Dim cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@batch", batchNumber)

                ' Open the connection
                conn.Open()

                ' Execute the query and get the result
                Dim result = Convert.ToInt32(cmd.ExecuteScalar())
                If result > 0 Then
                    exists = True
                End If

                conn.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error checking batch number: " & ex.Message)
        End Try

        Return exists
    End Function

    Private Sub ShowAndPopulateDataGridView()
        ' Show the DataGridView
        dgvdetailsbatch.Visible = True
        btninsert2.Visible = True

        ' Create columns
        dgvdetailsbatch.Columns.Clear()
        dgvdetailsbatch.Columns.Add("Lot", "lot")
        dgvdetailsbatch.Columns.Add("ClientPermission", "اذن العميل")
        dgvdetailsbatch.Columns.Add("ClientItemCode", "كود صنف العميل")
        dgvdetailsbatch.Columns.Add("WeightQuantity", "كميه وزن")
        dgvdetailsbatch.Columns.Add("MeterQuantity", "كميه متر")
        dgvdetailsbatch.Columns.Add("RollsCount", "عدد اتواب")
        dgvdetailsbatch.Columns.Add("storepermission", "اذن اضافة المخزن")


        ' Set header style
        For Each column As DataGridViewColumn In dgvdetailsbatch.Columns
            column.HeaderCell.Style.Font = New Font("Arial", 10, FontStyle.Bold)
            column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            column.HeaderCell.Style.BackColor = Color.LightGray
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Next

        ' Set column width to fill the DataGridView
        dgvdetailsbatch.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        ' Apply header style
        dgvdetailsbatch.EnableHeadersVisualStyles = False

        ' Add the first row with the batch number
        Dim batchNumber As String = lblbatch.Text
        dgvdetailsbatch.Rows.Add(batchNumber, "", "", "", "", "", "", "", "", "")

        ' Handle the CellValueChanged event to add batch number with incremented suffix
        AddHandler dgvdetailsbatch.CellValueChanged, AddressOf dgvdetailsbatch_CellValueChanged
    End Sub


    Private Sub dgvdetailsbatch_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex >= 0 AndAlso e.ColumnIndex <> dgvdetailsbatch.Columns("Lot").Index Then
            Dim currentRow As DataGridViewRow = dgvdetailsbatch.Rows(e.RowIndex)
            If String.IsNullOrEmpty(currentRow.Cells("Lot").Value?.ToString()) Then
                Dim selectedClientCode As String = DirectCast(cmbclient.SelectedItem, DataRowView)("code").ToString()
                If selectedClientCode = "P10000" Then
                    Dim suffix As Integer = e.RowIndex + 1 ' Ensure suffix starts from 1
                    currentRow.Cells("Lot").Value = lblbatch.Text & "\" & suffix.ToString()
                Else
                    currentRow.Cells("Lot").Value = lblbatch.Text
                End If
            End If
        End If
    End Sub






    Private Sub btninsert2_Click(sender As Object, e As EventArgs) Handles btninsert2.Click
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()

                ' Generate the add permission once
                Dim addPermissionCounter As Integer = GetNextAddPermissionCounter(conn)
                Dim addPermission As String = GenerateAddPermission(addPermissionCounter)

                For Each row As DataGridViewRow In dgvdetailsbatch.Rows
                    If Not row.IsNewRow Then
                        ' Check if required fields are filled
                        If String.IsNullOrEmpty(row.Cells("Lot").Value?.ToString()) OrElse
                       String.IsNullOrEmpty(row.Cells("WeightQuantity").Value?.ToString()) OrElse
                       String.IsNullOrEmpty(row.Cells("storepermission").Value?.ToString()) OrElse
                       String.IsNullOrEmpty(row.Cells("RollsCount").Value?.ToString()) Then
                            MessageBox.Show("Please fill in the required fields: Lot, WeightQuantity, and RollsCount.")
                            Return ' Exit the method if any required field is empty
                        End If
                    End If
                Next

                For Each row As DataGridViewRow In dgvdetailsbatch.Rows
                    If Not row.IsNewRow Then
                        Dim query As String = "INSERT INTO batch_details (batch_id, datetrans, lot, client_permission, client_item_code, weight_quantity, meter_quantity, rolls_count, add_permission,store_permission,rollpk,weightpk,username) " &
                                          "VALUES (@batch_id, @datetrans, @lot, @client_permission, @client_item_code, @weight_quantity, @meter_quantity, @rolls_count, @add_permission,@store_permission,@rollpk,@weightpk,@username)"
                        Dim cmd As New SqlCommand(query, conn)

                        ' Set the parameters
                        cmd.Parameters.AddWithValue("@batch_id", Convert.ToInt32(lblbatch.Text))
                        cmd.Parameters.AddWithValue("@datetrans", DateTime.Now)
                        cmd.Parameters.AddWithValue("@lot", row.Cells("Lot").Value)
                        cmd.Parameters.AddWithValue("@client_permission", If(String.IsNullOrEmpty(row.Cells("ClientPermission").Value?.ToString()), DBNull.Value, row.Cells("ClientPermission").Value))
                        cmd.Parameters.AddWithValue("@client_item_code", If(String.IsNullOrEmpty(row.Cells("ClientItemCode").Value?.ToString()), DBNull.Value, row.Cells("ClientItemCode").Value))
                        cmd.Parameters.AddWithValue("@weight_quantity", If(IsDBNull(row.Cells("WeightQuantity").Value), 0, row.Cells("WeightQuantity").Value))
                        cmd.Parameters.AddWithValue("@weightpk", If(IsDBNull(row.Cells("WeightQuantity").Value), 0, row.Cells("WeightQuantity").Value))
                        cmd.Parameters.AddWithValue("@meter_quantity", If(String.IsNullOrEmpty(row.Cells("MeterQuantity").Value?.ToString()), DBNull.Value, row.Cells("MeterQuantity").Value))
                        cmd.Parameters.AddWithValue("@rolls_count", row.Cells("RollsCount").Value)
                        cmd.Parameters.AddWithValue("@rollpk", row.Cells("RollsCount").Value)
                        cmd.Parameters.AddWithValue("@add_permission", addPermission)
                        cmd.Parameters.AddWithValue("@store_permission", If(IsDBNull(row.Cells("storepermission").Value), 0, row.Cells("storepermission").Value))
                        cmd.Parameters.AddWithValue("@username", LoggedInUsername)

                        ' Execute the query
                        cmd.ExecuteNonQuery()

                        ' Send email notification for each row
                        Dim subject As String = "Batch Details Inserted Successfully"
                        Dim body As String = $"تم استلام وزن {row.Cells("WeightQuantity").Value} ومتر {row.Cells("MeterQuantity").Value} على رقم رساله {lblbatch.Text} واللوت {row.Cells("Lot").Value} باذن عميل رقم {row.Cells("ClientPermission").Value} وعدد رول {row.Cells("RollsCount").Value} على اذن مخزن {row.Cells("storepermission").Value}."
                        SendEmailNotification(subject, body)
                    End If
                Next

                conn.Close()

                MessageBox.Show("Batch details inserted successfully.")
                ' Clear all fields, hide insert button and DataGridView, and reset batch number
                ClearAllFields()
                btninsert2.Visible = False
                dgvdetailsbatch.Visible = False
                lblbatch.Text = GetNextBatchNumber()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error inserting batch details: " & ex.Message)
        End Try
    End Sub
    Private Sub ClearAllFields()
        ' Clear DataGridView
        dgvdetailsbatch.Rows.Clear()

        ' Clear labels
        lblbatch.Text = String.Empty
        lblclient.Text = String.Empty
        lblsup.Text = String.Empty
        lblstyle.Text = String.Empty
        lblUsername.Text = String.Empty

        ' Clear ComboBoxes
        cmbclient.SelectedIndex = -1
        cmbsupplier.SelectedIndex = -1
        cmbkindfabric.SelectedIndex = -1
        cmbstyle.SelectedIndex = -1
        cmbservice.SelectedIndex = -1

        ' Clear TextBoxes
        txtpo.Text = String.Empty
        txtmaterial.Text = String.Empty

        ' Enable ComboBoxes and TextBoxes
        cmbclient.Enabled = True
        cmbsupplier.Enabled = True
        cmbkindfabric.Enabled = True
        cmbstyle.Enabled = True
        cmbservice.Enabled = True
        txtpo.Enabled = True
        txtmaterial.Enabled = True

        ' Enable insert button
        btninsert.Enabled = True
    End Sub

    Private Function GetNextAddPermissionCounter(conn As SqlConnection) As Integer
        Dim counter As Integer = 1
        Try
            Dim query As String = "SELECT MAX(CAST(SUBSTRING(add_permission, 6, LEN(add_permission)) AS INT)) FROM batch_details WHERE add_permission LIKE 'RRWM-%'"
            Dim cmd As New SqlCommand(query, conn)

            ' Execute the query and get the result
            Dim result = cmd.ExecuteScalar()
            If result IsNot DBNull.Value AndAlso result IsNot Nothing Then
                counter = Convert.ToInt32(result) + 1
            End If
        Catch ex As Exception
            MessageBox.Show("Error fetching next add permission counter: " & ex.Message)
        End Try

        Return counter
    End Function

    Private Function GenerateAddPermission(counter As Integer) As String
        Return "RRWM-" & counter.ToString("D7")
    End Function

    Private Function GetPublicName(ByVal username As String) As String
        Dim publicName As String = String.Empty
        Try
            Using conn As New SqlConnection(connectionString)
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
    Private Sub SendEmailNotification(subject As String, body As String)
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()

                ' Get email addresses from the mails table
                Dim query As String = "SELECT mail FROM mails"
                Dim cmd As New SqlCommand(query, conn)
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                Dim emailAddresses As New List(Of String)

                While reader.Read()
                    emailAddresses.Add(reader("mail").ToString())
                End While

                conn.Close()

                ' Send email
                Dim smtpClient As New SmtpClient("smtppro.zoho.com", 587) With {
                .Credentials = New NetworkCredential("it.app@moamen.com", "WMG@#$it$#@2024"),
                .EnableSsl = True
            }

                Dim mailMessage As New MailMessage() With {
                .From = New MailAddress("it.app@moamen.com"),
                .subject = subject,
                .body = body
            }

                For Each mail As String In emailAddresses
                    mailMessage.To.Add(mail)
                Next

                smtpClient.Send(mailMessage)
            End Using
        Catch ex As SmtpException
            MessageBox.Show("SMTP error sending email notification: " & ex.Message & vbCrLf & "Status Code: " & ex.StatusCode)
        Catch ex As Exception
            MessageBox.Show("Error sending email notification: " & ex.Message)
        End Try
    End Sub

End Class
