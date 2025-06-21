Imports System.Data.SqlClient
Imports System.Net
Imports System.Drawing
Imports System.Net.Mail

Public Class recievedrawform

    Private connectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"

    Private Sub recievedrawform_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadComboBoxes()
        lblUsername.Text = "Logged in as: " & LoggedInUsername
        ' Retrieve the public_name for the logged-in user
        Dim publicName As String = GetPublicName(LoggedInUsername)
    End Sub

    Private Sub LoadComboBoxes()
        Using conn As New SqlConnection(connectionString)
            conn.Open()

            ' Load cmbpo
            Using cmd As New SqlCommand("SELECT DISTINCT po_number FROM batch_raw", conn)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        cmbpo.Items.Add(reader("po_number").ToString())
                    End While
                End Using
            End Using
        End Using
    End Sub

    Private Sub cmbpo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbpo.SelectedIndexChanged
        cmbbatch.Items.Clear()
        LoadBatchComboBox(cmbpo.Text)
    End Sub

    Private Sub LoadBatchComboBox(poNumber As String)
        Using conn As New SqlConnection(connectionString)
            conn.Open()

            ' Load cmbbatch based on selected po_number
            Using cmd As New SqlCommand("SELECT DISTINCT batch FROM batch_raw WHERE po_number = @poNumber", conn)
                cmd.Parameters.AddWithValue("@poNumber", poNumber)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        cmbbatch.Items.Add(reader("batch").ToString())
                    End While
                End Using
            End Using
        End Using
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim poNumber As String = cmbpo.Text
        Dim batchId As String = cmbbatch.Text

        If String.IsNullOrEmpty(poNumber) AndAlso String.IsNullOrEmpty(batchId) Then
            MessageBox.Show("Please select a PO Number or Batch ID.")
            Return
        End If

        Dim query As String = "SELECT CONVERT(DATE, bd.datetrans) AS 'تاريخ الاستلام', bd.batch_id AS 'رقم الرسالة', bd.lot, bd.client_permission AS 'اذن العميل', bd.client_item_code AS 'كود صنف العميل', bd.weightpk AS 'كميه وزن', bd.meter_quantity AS 'كميه متر', bd.rollpk AS 'عدد اتواب',bd.store_permission  as 'اذن اضافه المخزن' FROM batch_details bd LEFT JOIN batch_raw br ON bd.batch_id = br.batch WHERE 1=1"
        If Not String.IsNullOrEmpty(poNumber) Then
            query &= " AND po_number = @poNumber"
        End If
        If Not String.IsNullOrEmpty(batchId) Then
            query &= " AND batch_id = @batchId"
        End If

        Using conn As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(query, conn)
                If Not String.IsNullOrEmpty(poNumber) Then
                    cmd.Parameters.AddWithValue("@poNumber", poNumber)
                End If
                If Not String.IsNullOrEmpty(batchId) Then
                    cmd.Parameters.AddWithValue("@batchId", batchId)
                End If

                Dim adapter As New SqlDataAdapter(cmd)
                Dim table As New DataTable()
                adapter.Fill(table)
                dgvdetailsbatch.DataSource = table

                ' Hide the "رقم الرسالة" column
                If dgvdetailsbatch.Columns.Contains("رقم الرسالة") Then
                    dgvdetailsbatch.Columns("رقم الرسالة").Visible = False
                End If

                ' Format DataGridView
                FormatDataGridView()

                ' Set rows to read-only if they contain data
                For Each row As DataGridViewRow In dgvdetailsbatch.Rows
                    If Not row.IsNewRow AndAlso row.Cells.Cast(Of DataGridViewCell).Any(Function(cell) Not String.IsNullOrEmpty(cell.Value?.ToString())) Then
                        row.ReadOnly = True
                    End If
                Next

                ' Get the batchId from the result if not provided
                If String.IsNullOrEmpty(batchId) AndAlso table.Rows.Count > 0 Then
                    batchId = table.Rows(0)("رقم الرسالة").ToString()
                End If
            End Using
        End Using

        ' Load additional details
        LoadAdditionalDetails(batchId)
    End Sub

    Private Sub dgvdetailsbatch_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles dgvdetailsbatch.CellBeginEdit
        Dim currentRow As DataGridViewRow = dgvdetailsbatch.Rows(e.RowIndex)
        If String.IsNullOrEmpty(currentRow.Cells("Lot").Value?.ToString()) Then
            Dim selectedClientCode As String = lblclient.Text.Split(":")(1).Trim()
            Dim batchValue As String = lblbatch.Text.Split(":")(1).Trim()
            If selectedClientCode = "P10000" Then
                Dim suffix As Integer = e.RowIndex + 1 ' Ensure suffix starts from 1
                currentRow.Cells("Lot").Value = batchValue & "\" & suffix.ToString()
            Else
                currentRow.Cells("Lot").Value = batchValue
            End If
        End If
    End Sub

    Private Sub dgvdetailsbatch_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgvdetailsbatch.CellValueChanged
        If e.RowIndex >= 0 AndAlso e.ColumnIndex <> dgvdetailsbatch.Columns("Lot").Index Then
            Dim currentRow As DataGridViewRow = dgvdetailsbatch.Rows(e.RowIndex)
            If String.IsNullOrEmpty(currentRow.Cells("Lot").Value?.ToString()) Then
                Dim selectedClientCode As String = lblclient.Text.Split(":")(1).Trim()
                Dim batchValue As String = lblbatch.Text.Split(":")(1).Trim()
                If selectedClientCode = "P10000" Then
                    Dim suffix As Integer = e.RowIndex + 1 ' Ensure suffix starts from 1
                    currentRow.Cells("Lot").Value = batchValue & "\" & suffix.ToString()
                Else
                    currentRow.Cells("Lot").Value = batchValue
                End If
            End If
        End If
    End Sub



    Private Sub FormatDataGridView()
        ' Center align text in DataGridView
        For Each column As DataGridViewColumn In dgvdetailsbatch.Columns
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            column.DefaultCellStyle.Font = New Font(dgvdetailsbatch.Font, FontStyle.Bold)
        Next

        ' Set header style
        dgvdetailsbatch.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvdetailsbatch.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue
        dgvdetailsbatch.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black
        dgvdetailsbatch.ColumnHeadersDefaultCellStyle.Font = New Font(dgvdetailsbatch.Font, FontStyle.Bold)
        dgvdetailsbatch.EnableHeadersVisualStyles = False

        ' Adjust column widths to fill the DataGridView
        dgvdetailsbatch.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        ' Set "تاريخ الاستلام" column to read-only
        If dgvdetailsbatch.Columns.Contains("تاريخ الاستلام") Then
            dgvdetailsbatch.Columns("تاريخ الاستلام").ReadOnly = True
        End If
    End Sub

    Private Sub LoadAdditionalDetails(batchId As String)
        If String.IsNullOrEmpty(batchId) Then
            Return
        End If

        Dim query As String = "SELECT ISNULL(c1.code, '') AS 'كود العميل', ISNULL(sup.code, '') AS 'كود المورد', ISNULL(sup.name, '') AS 'المورد', ISNULL(s.code, '') AS 'كود الخامة', ISNULL(br.batch, '') AS 'رقم الرسالة', ISNULL(s.name, '') AS 'الخامة2', ISNULL(ks.service_ar, '') AS 'نوع الخدمه', ISNULL(br.material, '') AS 'الخامة' " &
                          "FROM batch_raw br " &
                          "JOIN clients c1 ON br.client_code = c1.id " &
                          "LEFT JOIN suppliers sup ON br.sup_code = sup.id " &
                          "LEFT JOIN fabric f ON br.fabric_type = f.id " &
                          "LEFT JOIN style s ON br.style_id = s.id " &
                          "LEFT JOIN kind_service ks ON br.service_id = ks.id " &
                          "WHERE br.batch = @batchId"

        Using conn As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@batchId", batchId)

                Dim adapter As New SqlDataAdapter(cmd)
                Dim table As New DataTable()
                adapter.Fill(table)
                dgvdetailsbatch1.DataSource = table

                ' Update lblbatch and lblclient with the values from the respective columns
                If table.Rows.Count > 0 Then
                    lblbatch.Text = "رقم الرسالة: " & table.Rows(0)("رقم الرسالة").ToString()
                    lblclient.Text = "كود العميل: " & table.Rows(0)("كود العميل").ToString()
                Else
                    lblbatch.Text = "رقم الرسالة: "
                    lblclient.Text = "كود العميل: "
                End If

                ' Format DataGridView
                FormatDataGridView(dgvdetailsbatch1)
            End Using
        End Using
    End Sub

    Private Sub FormatDataGridView(dgv As DataGridView)
        ' Center align text in DataGridView
        For Each column As DataGridViewColumn In dgv.Columns
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            column.DefaultCellStyle.Font = New Font(dgv.Font, FontStyle.Bold)
        Next

        ' Set header style
        dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black
        dgv.ColumnHeadersDefaultCellStyle.Font = New Font(dgv.Font, FontStyle.Bold)
        dgv.EnableHeadersVisualStyles = False

        ' Adjust column widths to fill the DataGridView
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    End Sub



    Private Sub btnInsert_Click(sender As Object, e As EventArgs) Handles btninsert.Click
        Using conn As New SqlConnection(connectionString)
            conn.Open()

            ' Extract the numeric part from lblbatch.Text
            Dim batchId As String = lblbatch.Text.Split(":")(1).Trim()

            For Each row As DataGridViewRow In dgvdetailsbatch.Rows
                If row.IsNewRow OrElse row.ReadOnly Then
                    Continue For
                End If

                Dim query As String = "INSERT INTO batch_details (batch_id, lot, client_permission, client_item_code, weight_quantity, meter_quantity, rolls_count, store_permission, datetrans, weightpk, rollpk, username) " &
                                  "VALUES (@batch_id, @lot, @client_permission, @client_item_code, @weight_quantity, @meter_quantity, @rolls_count, @store_permission, @datetrans, @weightpk, @rollpk, @username)"

                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@batch_id", batchId)
                    cmd.Parameters.AddWithValue("@lot", row.Cells("lot").Value)
                    cmd.Parameters.AddWithValue("@client_permission", row.Cells("اذن العميل").Value)
                    cmd.Parameters.AddWithValue("@client_item_code", row.Cells("كود صنف العميل").Value)
                    cmd.Parameters.AddWithValue("@weight_quantity", row.Cells("كميه وزن").Value)
                    cmd.Parameters.AddWithValue("@weightpk", row.Cells("كميه وزن").Value)
                    cmd.Parameters.AddWithValue("@meter_quantity", row.Cells("كميه متر").Value)
                    cmd.Parameters.AddWithValue("@rolls_count", row.Cells("عدد اتواب").Value)
                    cmd.Parameters.AddWithValue("@rollpk", row.Cells("عدد اتواب").Value)
                    cmd.Parameters.AddWithValue("@store_permission", row.Cells("اذن اضافه المخزن").Value)
                    cmd.Parameters.AddWithValue("@datetrans", DateTime.Now)
                    cmd.Parameters.AddWithValue("@username", LoggedInUsername)

                    cmd.ExecuteNonQuery()

                    ' Send email notification for each row
                    Dim subject As String = "Batch Details Inserted Successfully"
                    Dim body As String = $"تم استلام وزن {row.Cells("كميه وزن").Value} ومتر {row.Cells("كميه متر").Value} على رقم رساله {batchId} واللوت {row.Cells("lot").Value} باذن عميل رقم {row.Cells("اذن العميل").Value} وعدد رول {row.Cells("عدد اتواب").Value} على اذن مخزن {row.Cells("اذن اضافه المخزن").Value}."
                    SendEmailNotification(subject, body)
                End Using
            Next
        End Using

        MessageBox.Show("Data inserted successfully.")
    End Sub


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
.Subject = subject,
                .Body = body
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
