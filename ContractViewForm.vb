Imports System.Data.SqlClient
Imports System.Net
Imports Microsoft.Office.Interop

Public Class ContractViewForm

    ' Connection string to the SQL Server database
    Private connectionString As String = "Server=localhost;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"
    Private UserAccessLevel As Integer = 0 ' Default to 0 (normal user)
    ' Form Load Event to initialize the form

    Private reasonTextBox As TextBox
    Private submitReasonButton As Button
    Private dtpSearchDate As DateTimePicker
    Private cmbContractNo As ComboBox
    Private lblDate As Label
    Private lblContractNo As Label

    Private Sub ContractViewForm_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        ' Set form to full screen
        Me.WindowState = FormWindowState.Maximized
        Me.FormBorderStyle = FormBorderStyle.Sizable

        ' Access the logged-in username from the global variable
        lblUsername.Text = "Logged in as: " & LoggedInUsername
        ' Set the user access level based on logged-in username
        SetUserAccessLevel(LoggedInUsername)

        ' Initialize Labels
        lblDate = New Label()
        lblDate.Text = "تاريخ البحث:"
        lblDate.Location = New Point(10, 20)
        lblDate.Size = New Size(100, 20)
        lblDate.Font = New Font("Arial", 10)
        Me.Controls.Add(lblDate)

        lblContractNo = New Label()
        lblContractNo.Text = "رقم التعاقد:"
        lblContractNo.Location = New Point(220, 20)
        lblContractNo.Size = New Size(100, 20)
        lblContractNo.Font = New Font("Arial", 10)
        Me.Controls.Add(lblContractNo)

        ' Initialize DateTimePicker
        dtpSearchDate = New DateTimePicker()
        dtpSearchDate.Location = New Point(10, 50)
        dtpSearchDate.Size = New Size(200, 30)
        dtpSearchDate.Format = DateTimePickerFormat.Short
        dtpSearchDate.ShowCheckBox = True
        dtpSearchDate.Checked = False
        dtpSearchDate.Font = New Font("Arial", 10)
        Me.Controls.Add(dtpSearchDate)

        ' Initialize ComboBox with auto-complete
        cmbContractNo = New ComboBox()
        cmbContractNo.Location = New Point(220, 50)
        cmbContractNo.Size = New Size(200, 30)
        cmbContractNo.DropDownStyle = ComboBoxStyle.DropDown
        cmbContractNo.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cmbContractNo.AutoCompleteSource = AutoCompleteSource.ListItems
        cmbContractNo.Font = New Font("Arial", 10)
        Me.Controls.Add(cmbContractNo)



        ' Disable editing in DataGridView for normal users
        If UserAccessLevel = 0 Then
            dgvContractDetails.ReadOnly = True
        Else
            dgvContractDetails.ReadOnly = False
        End If

        ' Load contract numbers into ComboBox
        LoadContractNumbers()

        ' Initialize the DataGridView columns
        dgvContractDetails.Columns.Add("ContractID", "ID") ' Added ContractID column for updates
        dgvContractDetails.Columns.Add("ContractNo", "رقم التعاقد")
        dgvContractDetails.Columns.Add("ContractType", "نوع التعاقد")
        dgvContractDetails.Columns.Add("ContractDate", "تاريخ التعاقد")
        dgvContractDetails.Columns.Add("ClientCode", "كود العميل")
        dgvContractDetails.Columns.Add("Material", "الخامة")
        dgvContractDetails.Columns.Add("refno", "رقم الإذن")
        dgvContractDetails.Columns.Add("Batch", "رقم الرسالة")
        dgvContractDetails.Columns.Add("lot", "lot")
        dgvContractDetails.Columns.Add("QuantityM", "الكمية متر")
        dgvContractDetails.Columns.Add("QuantityK", "الكمية كيلو")
        dgvContractDetails.Columns.Add("FabricCode", "كود الخامة")
        dgvContractDetails.Columns.Add("color", "اللون")
        dgvContractDetails.Columns.Add("WidthReq", "العرض المطلوب")
        dgvContractDetails.Columns.Add("WeightM", "الوزن المطلوب")
        dgvContractDetails.Columns.Add("RollM", "أمتار التوب المطلوبة")
        dgvContractDetails.Columns.Add("Notes", "ملاحظات")
        dgvContractDetails.Columns.Add("DateInserted", "تاريخ الحركة")

        ' Customize DataGridView appearance
        CustomizeDataGridView()
        ' Set column widths
        SetColumnWidths()

        ' Add TextBox for reason
        reasonTextBox = New TextBox()
        reasonTextBox.Location = New Point(10, dgvContractDetails.Top - 35)
        reasonTextBox.Size = New Size(300, 30)
        reasonTextBox.Visible = False
        Me.Controls.Add(reasonTextBox)

        ' Add Button to submit reason
        submitReasonButton = New Button()
        submitReasonButton.Text = "Submit Reason"
        submitReasonButton.Location = New Point(reasonTextBox.Right - 1, reasonTextBox.Top)
        submitReasonButton.Size = New Size(100, 30)
        submitReasonButton.Visible = False
        AddHandler submitReasonButton.Click, AddressOf SubmitReasonButton_Click
        Me.Controls.Add(submitReasonButton)
    End Sub

    ' Customize the DataGridView appearance
    Private Sub CustomizeDataGridView()
        dgvContractDetails.Font = New Font("Arial", 10)
        dgvContractDetails.RowTemplate.Height = 30

        ' Set alternating row colors
        dgvContractDetails.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray
        dgvContractDetails.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 10, FontStyle.Bold)
        dgvContractDetails.EnableHeadersVisualStyles = False

        ' Center align the content in all columns
        For Each column As DataGridViewColumn In dgvContractDetails.Columns
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter ' Center align the header too
        Next
    End Sub

    ' Set the widths of DataGridView columns
    Private Sub SetColumnWidths()
        dgvContractDetails.Columns("ContractID").Width = 15 ' Make sure to set width for ContractID
        dgvContractDetails.Columns("ContractNo").Width = 100
        dgvContractDetails.Columns("ContractType").Width = 120
        dgvContractDetails.Columns("ContractDate").Width = 120
        dgvContractDetails.Columns("ClientCode").Width = 80
        dgvContractDetails.Columns("Material").Width = 200
        dgvContractDetails.Columns("refno").Width = 100
        dgvContractDetails.Columns("Batch").Width = 100
        dgvContractDetails.Columns("lot").Width = 100
        dgvContractDetails.Columns("QuantityM").Width = 100
        dgvContractDetails.Columns("QuantityK").Width = 100
        dgvContractDetails.Columns("FabricCode").Width = 100
        dgvContractDetails.Columns("color").Width = 100
        dgvContractDetails.Columns("WidthReq").Width = 70
        dgvContractDetails.Columns("WeightM").Width = 100
        dgvContractDetails.Columns("RollM").Width = 90
        dgvContractDetails.Columns("Notes").Width = 250
        dgvContractDetails.Columns("DateInserted").Width = 120
    End Sub

    ' Load contract numbers into ComboBox
    Private Sub LoadContractNumbers()
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim query As String = "SELECT DISTINCT ContractNo FROM Contracts ORDER BY ContractNo"
                Using cmd As New SqlCommand(query, conn)
                    Dim reader As SqlDataReader = cmd.ExecuteReader()
                    cmbContractNo.Items.Clear()
                    cmbContractNo.Items.Add("") ' Add empty option
                    While reader.Read()
                        cmbContractNo.Items.Add(reader("ContractNo").ToString())
                    End While
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading contract numbers: " & ex.Message)
        End Try
    End Sub

    ' Search button click event
    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        ' Clear previous data from DataGridView
        dgvContractDetails.Rows.Clear()

        ' Fetch and display contract details
        LoadContractDetails()
    End Sub

    ' Load contract details from the database based on the ContractNo and Date
    Private Sub LoadContractDetails()
        Try
            ' Establish SQL connection
            Using conn As New SqlConnection(connectionString)
                ' SQL query to retrieve contract and client details
                Dim query As String
                Dim parameters As New List(Of SqlParameter)

                query = "SELECT c.ContractID, c.ContractNo, f.fabricType_ar as contracttype, c.ContractDate, " &
                        "cl.code AS ClientCode, c.Material, c.refno, c.Batch, c.lot, c.QuantityM, c.QuantityK, " &
                        "c.FabricCode, c.color as'color', c.WidthReq, c.WeightM, c.RollM, c.Notes, c.DateInserted " &
                        "FROM Contracts c LEFT JOIN clients cl ON c.clientcode = cl.id LEFT JOIN fabric f ON c.contracttype = f.id " &
                        "WHERE 1=1"

                ' Add ContractNo filter if selected
                If Not String.IsNullOrEmpty(cmbContractNo.Text.Trim()) Then
                    query &= " AND c.ContractNo = @ContractNo"
                    parameters.Add(New SqlParameter("@ContractNo", cmbContractNo.Text.Trim()))
                End If

                ' Add Date filter if selected
                If dtpSearchDate.Checked Then
                    query &= " AND CONVERT(date, c.ContractDate) = @SearchDate"
                    parameters.Add(New SqlParameter("@SearchDate", dtpSearchDate.Value.Date))
                End If

                ' Create a SQL command object
                Using cmd As New SqlCommand(query, conn)
                    ' Add parameters
                    For Each param As SqlParameter In parameters
                        cmd.Parameters.Add(param)
                    Next

                    ' Open the connection
                    conn.Open()

                    ' Execute the query and fetch data
                    Dim reader As SqlDataReader = cmd.ExecuteReader()

                    If reader.HasRows Then
                        ' Populate the DataGridView with the fetched data
                        While reader.Read()
                            dgvContractDetails.Rows.Add(reader("ContractID"),
                                                        reader("ContractNo").ToString(),
                                                        reader("ContractType").ToString(),
                                                        Convert.ToDateTime(reader("ContractDate")).ToString("yyyy-MM-dd"),
                                                        reader("ClientCode").ToString(),
                                                        reader("Material").ToString(),
                                                        reader("refno").ToString(),
                                                        reader("Batch").ToString(),
                                                        reader("lot").ToString(),
                                                        reader("QuantityM").ToString(),
                                                        reader("QuantityK").ToString(),
                                                        reader("FabricCode").ToString(),
                                                        reader("color").ToString(),
                                                        reader("WidthReq").ToString(),
                                                        reader("WeightM").ToString(),
                                                        reader("RollM").ToString(),
                                                        reader("Notes").ToString(),
                                                        Convert.ToDateTime(reader("DateInserted")).ToString("yyyy-MM-dd"))
                        End While
                    Else
                        MessageBox.Show("No contracts found matching the search criteria!")
                    End If

                    ' Close the reader and connection
                    reader.Close()
                    conn.Close()
                End Using
            End Using
        Catch ex As Exception
            ' Handle any potential errors
            MessageBox.Show("An error occurred: " & ex.Message)
        End Try
    End Sub

    ' Event handler for when editing a cell ends
    Private Sub dgvContractDetails_CellEndEdit(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles dgvContractDetails.CellEndEdit
        ' Check if the user has the required access level to edit
        If UserAccessLevel = 0 Then
            MessageBox.Show("You do not have permission to edit this data.")
            ' Revert the cell value to its original value
            dgvContractDetails.CancelEdit()
            Return
        End If

        ' Check if reason TextBox is already visible (indicating a pending change)
        If reasonTextBox.Visible Then
            MessageBox.Show("You must provide a reason for the previous change before making another change.")
            ' Revert the cell value to its original value
            dgvContractDetails.CancelEdit()
            Return
        End If

        ' Show reason TextBox and Button
        reasonTextBox.Visible = True
        submitReasonButton.Visible = True

        ' Store the necessary information for the update
        reasonTextBox.Tag = New With {
            .ContractID = CInt(dgvContractDetails.Rows(e.RowIndex).Cells("ContractID").Value),
            .ColumnName = dgvContractDetails.Columns(e.ColumnIndex).Name,
            .NewValue = dgvContractDetails.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString(),
            .OldValue = dgvContractDetails.Rows(e.RowIndex).Cells(e.ColumnIndex).Tag.ToString()
        }
    End Sub
    Private Function GetLocalIPAddress() As String
        Dim host As String = Dns.GetHostName()
        Dim ip As String = Dns.GetHostEntry(host).AddressList _
        .FirstOrDefault(Function(a) a.AddressFamily = Sockets.AddressFamily.InterNetwork).ToString()
        Return ip
    End Function


    ' Method to update contract detail in the database
    Private Sub UpdateContractDetail(ByVal contractID As Integer, ByVal columnName As String, ByVal newValue As String, ByVal oldValue As String, ByVal reason As String)
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                ' Update the contract detail
                Dim updateQuery As String = "UPDATE Contracts SET " & columnName & " = @newValue WHERE ContractID = @ContractID"
                Using updateCmd As New SqlCommand(updateQuery, conn)
                    updateCmd.Parameters.AddWithValue("@newValue", newValue)
                    updateCmd.Parameters.AddWithValue("@ContractID", contractID)
                    updateCmd.ExecuteNonQuery()
                End Using
                ' Insert into activity_logs
                Dim insertLogQuery As String = "INSERT INTO activity_logs (contract, oldtrans, newtrans, timestamp, username, kindtrans, contract_reason, ip_address, coloumn_name) " &
                                           "VALUES (@contract, @oldtrans, @newtrans, @timestamp, @username, @kindtrans, @contract_reason, @ip_address, @coloumn_name)"
                Using insertLogCmd As New SqlCommand(insertLogQuery, conn)
                    insertLogCmd.Parameters.AddWithValue("@contract", contractID)
                    insertLogCmd.Parameters.AddWithValue("@oldtrans", oldValue)
                    insertLogCmd.Parameters.AddWithValue("@newtrans", newValue)
                    insertLogCmd.Parameters.AddWithValue("@timestamp", DateTime.Now)
                    insertLogCmd.Parameters.AddWithValue("@username", LoggedInUsername)
                    insertLogCmd.Parameters.AddWithValue("@kindtrans", "تغيير فى بيانات التعاقد")
                    insertLogCmd.Parameters.AddWithValue("@contract_reason", reason)
                    insertLogCmd.Parameters.AddWithValue("@ip_address", GetLocalIPAddress())
                    insertLogCmd.Parameters.AddWithValue("@coloumn_name", columnName)
                    insertLogCmd.ExecuteNonQuery()
                End Using
            End Using
            MessageBox.Show("تم تسجيل البيانات بنجاح")
            ' --- إرسال إيميل بعد التحديث ---
            ' جلب رقم التعاقد واللوت من قاعدة البيانات
            Dim contractNo As String = ""
            Dim lot As String = ""
            Try
                Using conn As New SqlConnection(connectionString)
                    conn.Open()
                    Dim query As String = "SELECT ContractNo, lot FROM Contracts WHERE ContractID = @ContractID"
                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@ContractID", contractID)
                        Using reader As SqlDataReader = cmd.ExecuteReader()
                            If reader.Read() Then
                                contractNo = reader("ContractNo").ToString()
                                lot = reader("lot").ToString()
                            End If
                        End Using
                    End Using
                End Using
            Catch ex As Exception
                ' لو فشل الجلب، نتركها فارغة
            End Try
            Dim subject As String = $"تم تعديل بيانات تعاقد رقم {contractNo} (Lot: {lot})"
            Dim body As String = $"<h2>تم تعديل بيانات تعاقد</h2>" & _
                $"<b>رقم التعاقد:</b> {contractNo}<br>" & _
                $"<b>اللوت:</b> {lot}<br>" & _
                $"<b>العمود المعدل:</b> {columnName}<br>" & _
                $"<b>القيمة القديمة:</b> {oldValue}<br>" & _
                $"<b>القيمة الجديدة:</b> {newValue}<br>" & _
                $"<b>سبب التغيير:</b> {reason}<br>" & _
                $"<b>المستخدم:</b> {LoggedInUsername}<br>" & _
                $"<b>تاريخ التغيير:</b> {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}<br>"
            SendEmailNotification(subject, body)
            ' --- نهاية إرسال الإيميل ---
        Catch ex As Exception
            MessageBox.Show("An error occurred while updating the contract detail: " & ex.Message)
        End Try
    End Sub

    Private Sub SubmitReasonButton_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim info = DirectCast(reasonTextBox.Tag, Object)
        Dim contractID As Integer = info.ContractID
        Dim columnName As String = info.ColumnName
        Dim newValue As String = info.NewValue
        Dim oldValue As String = info.OldValue
        Dim reason As String = reasonTextBox.Text

        ' Check if reason is provided
        If String.IsNullOrWhiteSpace(reason) Then
            MessageBox.Show("ادخل سبب التغيير")
            Return
        End If

        ' Check if reason is clear and meaningful
        If Not IsReasonValid(reason) Then
            MessageBox.Show("هذا السبب غير واضح كفاية لتحديث البيانات ")
            Return
        End If

        ' Hide reason TextBox and Button
        reasonTextBox.Visible = False
        submitReasonButton.Visible = False

        ' Clear reason TextBox
        reasonTextBox.Text = String.Empty

        ' Update contract detail and log the change
        UpdateContractDetail(contractID, columnName, newValue, oldValue, reason)
    End Sub


    ' Function to validate the reason
    Private Function IsReasonValid(ByVal reason As String) As Boolean
        ' Split the reason into words and check if there are more than two words
        Dim words As String() = reason.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
        Return words.Length > 2
    End Function



    Private Sub dgvContractDetails_CellBeginEdit(ByVal sender As Object, ByVal e As DataGridViewCellCancelEventArgs) Handles dgvContractDetails.CellBeginEdit
        ' Prevent editing of the "ClientCode" column
        If dgvContractDetails.Columns(e.ColumnIndex).Name = "ClientCode" Then
            MessageBox.Show("Editing the Client Code is not allowed.")
            e.Cancel = True
            Return
        End If
        If dgvContractDetails.Columns(e.ColumnIndex).Name = "Batch" Then
            MessageBox.Show("Editing the batch is not allowed.")
            e.Cancel = True
            Return
        End If
        If dgvContractDetails.Columns(e.ColumnIndex).Name = "lot" Then
            MessageBox.Show("Editing the lot is not allowed.")
            e.Cancel = True
            Return
        End If
        If dgvContractDetails.Columns(e.ColumnIndex).Name = "ContractType" Then
            MessageBox.Show("Editing the contract kind is not allowed.")
            e.Cancel = True
            Return
        End If

        If dgvContractDetails.Columns(e.ColumnIndex).Name = "ContractDate" Then
            MessageBox.Show("Editing the Contract Date is not allowed.")
            e.Cancel = True
            Return
        End If

        If dgvContractDetails.Columns(e.ColumnIndex).Name = "ContractNo" Then
            MessageBox.Show("Editing the Contract is not allowed.")
            e.Cancel = True
            Return
        End If
        ' Store the original value of the cell before editing
        dgvContractDetails.Rows(e.RowIndex).Cells(e.ColumnIndex).Tag = dgvContractDetails.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString()
    End Sub

    ' Event handler for Export to Excel button
    Private Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportToExcel.Click
        ExportToExcel()
    End Sub

    ' Method to export DataGridView data to Excel
    Private Sub ExportToExcel()
        Try
            ' Check if DataGridView has rows
            If dgvContractDetails.Rows.Count = 0 Then
                MessageBox.Show("No data available to export.")
                Return
            End If

            ' Create a new Excel application instance
            Dim excelApp As New Excel.Application()
            excelApp.Workbooks.Add()
            Dim worksheet As Excel.Worksheet = excelApp.ActiveSheet

            ' Add column headers
            For i As Integer = 0 To dgvContractDetails.Columns.Count - 1
                worksheet.Cells(1, i + 1) = dgvContractDetails.Columns(i).HeaderText
            Next

            ' Add data from DataGridView to Excel
            For i As Integer = 0 To dgvContractDetails.Rows.Count - 1
                For j As Integer = 0 To dgvContractDetails.Columns.Count - 1
                    If dgvContractDetails.Rows(i).Cells(j).Value IsNot Nothing Then
                        worksheet.Cells(i + 2, j + 1) = dgvContractDetails.Rows(i).Cells(j).Value.ToString()
                    End If
                Next
            Next

            ' Save the Excel file
            Dim saveFileDialog As New SaveFileDialog()
            saveFileDialog.Filter = "Excel Files|*.xlsx"
            saveFileDialog.Title = "Save an Excel File"
            If saveFileDialog.ShowDialog() = DialogResult.OK Then
                worksheet.SaveAs(saveFileDialog.FileName)
                MessageBox.Show("Exported Successfully!")
            End If

            ' Cleanup
            excelApp.Visible = True
            System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp)

        Catch ex As Exception
            MessageBox.Show("An error occurred while exporting to Excel: " & ex.Message)
        End Try
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

    ' Handle form resize
    Private Sub ContractViewForm_Resize(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Resize
        If dgvContractDetails IsNot Nothing Then
            dgvContractDetails.Size = New Size(Me.ClientSize.Width - 20, Me.ClientSize.Height - 150)
        End If
    End Sub

    ' دالة إرسال الإيميل (منقولة من ContractForm)
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
                Dim smtpClient As New Net.Mail.SmtpClient("smtppro.zoho.com", 587) With {
                    .Credentials = New Net.NetworkCredential("it.app@moamen.com", "WMG@#$it$#@2024"),
                    .EnableSsl = True
                }
                Dim mailMessage As New Net.Mail.MailMessage() With {
                    .From = New Net.Mail.MailAddress("it.app@moamen.com"),
                    .Subject = subject,
                    .Body = body,
                    .IsBodyHtml = True
                }
                For Each mail As String In emailAddresses
                    mailMessage.To.Add(mail)
                Next
                smtpClient.Send(mailMessage)
            End Using
        Catch ex As Net.Mail.SmtpException
            MessageBox.Show("SMTP error sending email notification: " & ex.Message & vbCrLf & "Status Code: " & ex.StatusCode)
        Catch ex As Exception
            MessageBox.Show("Error sending email notification: " & ex.Message)
        End Try
    End Sub

End Class
