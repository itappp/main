Imports MySql.Data.MySqlClient
Imports System.Data.SqlClient
Imports Excel = Microsoft.Office.Interop.Excel ' For Excel interop
Imports OfficeOpenXml ' Add this line for EPPlus
Imports System.IO
Imports System.Net
Imports System.Net.Mail
Imports System.Text
Public Class techdataform
    ' SQL Server connection string
    Dim sqlServerConnectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"

    ' Create a Timer
    Private WithEvents searchTimer As New Timer()

    ' Initialize the form and the timer
    Public Sub New()
        InitializeComponent()

        ' Set the timer interval to 1500 milliseconds (1.5 seconds)
        searchTimer.Interval = 1500
        PopulateContractNo()
        ' Access the logged-in username from the global variable
        lblusername.Text = "Logged in as: " & LoggedInUsername
        ' Retrieve the public_name for the logged-in user
        Dim publicName As String = GetPublicName(LoggedInUsername)

    End Sub

    ' Event handler for when text is changed in the txtBatch textbox
    Private Sub txtBatch_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtbatch.TextChanged
        ' Restart the timer whenever text changes
        searchTimer.Stop()
        searchTimer.Start()
    End Sub

    ' Event handler for the Timer's Tick event
    Private Sub searchTimer_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles searchTimer.Tick
        ' Stop the timer
        searchTimer.Stop()

        ' Call the method to fetch data

        Fetchbatch1()




    End Sub


    Private Sub Fetchbatch1()
        Dim ContractId As String = lblcontractid.Text ' Get contract number from the combobox
        If String.IsNullOrEmpty(ContractId) Then
            dataGridbatch1.DataSource = Nothing ' Clear the DataGridView if no contract number is entered

            cmbbatch.SelectedIndex = -1 ' Clear cmbcodelib if no contract number
            lblgetlastworder.Text = "" ' Clear the last work order label
            clbWorderIds.Items.Clear()

            Return
        End If

        ' SQL query to fetch data based on the contract number
        Dim query As String = "SELECT c.ContractID as 'ID', c.ContractNo as 'رقم التعاقد', f.fabricType_ar as 'نوع التعاقد', c.ContractDate as 'تاريخ التعاقد', " &
                              "cl.code as 'كود العميل', c.Material as 'الخامة', c.refno as 'رقم الاذن', c.Batch as 'رقم الرساله',c.lot, c.QuantityM as 'الكمية متر', c.QuantityK as 'الكمية كيلو', " &
                              "c.color, c.FabricCode as 'كود الخامه', c.WidthReq as 'العرض المطلوب', c.WeightM as 'الوزن المطلوب', c.RollM as 'أمتار التوب المطلوبة', c.Notes as 'ملاحظات' " &
                              "FROM Contracts c LEFT JOIN clients cl ON c.clientcode = cl.id LEFT JOIN fabric f ON c.contracttype = f.id WHERE contractid = @contractid"

        Using connection As New SqlConnection(sqlServerConnectionString)
            Try
                connection.Open()
                Using cmd As New SqlCommand(query, connection)
                    ' Parameterize the query to prevent SQL injection
                    cmd.Parameters.AddWithValue("@ContractId", ContractId)

                    ' Create SqlDataAdapter to fill the DataTable
                    Dim adapter As New SqlDataAdapter(cmd)
                    Dim table As New DataTable()

                    ' Fill the DataTable with data
                    adapter.Fill(table)

                    ' Set the DataSource of the DataGridView to display the data
                    dataGridbatch1.DataSource = table

                    ' Center the content and headers of each column and set the width
                    For Each column As DataGridViewColumn In dataGridbatch1.Columns
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                        column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter ' Center the header
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells ' Set auto size based on displayed cells
                    Next

                    ' Optionally, auto resize the columns to fit the content
                    dataGridbatch1.AutoResizeColumns()

                    ' Check if there are any rows in the DataGridView
                    If dataGridbatch1.Rows.Count > 0 Then
                        ' Get the fabric type from the first row
                        Dim fabricType As String = dataGridbatch1.Rows(0).Cells("نوع التعاقد").Value.ToString()
                        PopulateCode(fabricType) ' Call the method to populate codes based on fabric type

                        ' Set the prefix based on fabric type
                        Dim prefix As String = If(fabricType = "نسيج", "W", If(fabricType = "تريكو", "K", String.Empty))

                        If Not String.IsNullOrEmpty(prefix) Then
                            ' Call method to retrieve last work order based on fabric type
                            GetLastWorkOrder(prefix)
                        Else
                            lblgetlastworder.Text = "Invalid fabric type."
                        End If

                        ' Fetch all worderid based on contract ID
                        FetchAllWorderIds() ' Call the method to fetch all work order IDs
                    Else

                        lblgetlastworder.Text = "" ' Clear the last work order label

                    End If
                End Using
            Catch ex As SqlException
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
    End Sub
    Private Sub FetchAllWorderIds()
        Dim ContractId As String = lblcontractid.Text ' Get contract number from the label
        If String.IsNullOrEmpty(ContractId) Then
            clbWorderIds.Items.Clear() ' Clear the CheckedListBox if no contract number is entered
            Return
        End If

        ' SQL query to fetch all worderid based on the contract number
        Dim query As String = "SELECT worderid FROM techdata WHERE contract_id = @contractid"

        Using connection As New SqlConnection(sqlServerConnectionString)
            Try
                connection.Open()
                Using cmd As New SqlCommand(query, connection)
                    ' Parameterize the query to prevent SQL injection
                    cmd.Parameters.AddWithValue("@contractid", ContractId)

                    ' Execute the command and read the results
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        clbWorderIds.Items.Clear() ' Clear the CheckedListBox before adding new items
                        While reader.Read()
                            If Not reader.IsDBNull(0) Then
                                clbWorderIds.Items.Add(reader.GetString(0)) ' Add worderid to the CheckedListBox
                            End If
                        End While

                        ' Display a message if no work orders are found
                        If clbWorderIds.Items.Count = 0 Then
                            MessageBox.Show("No work orders found.")
                        End If
                    End Using
                End Using
            Catch ex As SqlException
                MessageBox.Show("Error fetching work orders: " & ex.Message)
            End Try
        End Using
    End Sub
    Private Sub GetLastWorkOrder(ByVal prefix As String)
        ' Query to get the last work order ID for a given prefix
        Dim query As String = "SELECT TOP 1 LEFT(worderid, LEN(@prefix) + 5) AS PrefixAndDigits FROM techdata WHERE worderid LIKE @prefix + '%' AND ISNUMERIC(SUBSTRING(worderid, LEN(@prefix) + 1, 5)) = 1 ORDER BY CAST(SUBSTRING(worderid, LEN(@prefix) + 1, 5) AS INT) DESC;"

        Dim lastOrder As String = "No work orders found starting with " & prefix

        Using connection As New SqlConnection(sqlServerConnectionString)
            Try
                connection.Open()
                Using cmd As New SqlCommand(query, connection)
                    ' Set prefix as parameter
                    cmd.Parameters.AddWithValue("@prefix", prefix)

                    ' ExecuteScalar to fetch the result
                    Dim lastWorkOrder As Object = cmd.ExecuteScalar()
                    If lastWorkOrder IsNot Nothing Then
                        lastOrder = "Last Work Order for " & prefix & ": " & lastWorkOrder.ToString()
                    End If

                    ' Update the label to display the result
                    lblgetlastworder.Text = lastOrder
                End Using
            Catch ex As SqlException
                MessageBox.Show("Error fetching last work order: " & ex.Message)
            End Try
        End Using
    End Sub


    ' Declare a dictionary to store contractId against contractNo
    Private contractDictionary As New Dictionary(Of String, Integer)

    ' Populate cmbContractNo with unique contract numbers from SQL Server
    Private Sub PopulateContractNo()
        Dim query As String = "SELECT DISTINCT contractid, contractno FROM contracts"
        Dim uniqueContractNos As New HashSet(Of String)

        Using connection As New SqlConnection(sqlServerConnectionString)
            Try
                connection.Open()
                Using cmd As New SqlCommand(query, connection)
                    Dim reader As SqlDataReader = cmd.ExecuteReader()
                    cmbcontractno.Items.Clear()
                    contractDictionary.Clear()

                    While reader.Read()
                        Dim contractId As Integer = reader("contractid")
                        Dim contractNo As String = reader("contractno").ToString()

                        ' Check if the contract number is already in the HashSet (unique check)
                        If Not uniqueContractNos.Contains(contractNo) Then
                            ' Add contractNo to ComboBox and HashSet
                            cmbcontractno.Items.Add(contractNo)
                            uniqueContractNos.Add(contractNo)

                            ' Store contractId associated with the contractNo in the dictionary
                            contractDictionary(contractNo) = contractId
                        End If
                    End While
                End Using
            Catch ex As SqlException
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
    End Sub
    ' Handle selected item in cmbContractNo
    Private Sub cmbContractNo_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbcontractno.SelectedIndexChanged
        ' Clear cmbBatch whenever contractNo is changed
        cmbbatch.Items.Clear() ' Ensure the items are cleared
        txtbatch.Text = ""
        lblcontractid.Text = "" ' Optionally clear the contractId label

        ' Check if something is selected
        If cmbcontractno.SelectedItem IsNot Nothing Then
            ' Get the selected contract number
            Dim selectedContractNo As String = cmbcontractno.SelectedItem.ToString()

            ' Retrieve the contractId from the dictionary based on the selected contractNo
            If contractDictionary.ContainsKey(selectedContractNo) Then
                ' Get the contractId associated with the selected contractNo
                Dim contractId As Integer = contractDictionary(selectedContractNo)

                ' Call method to fetch batches for the selected contractNo
                FetchBatchesByContractId(selectedContractNo)
            End If
        End If
    End Sub

    ' Fetch batches for the selected contract number and refresh cmbBatch
    Private Sub FetchBatchesByContractId(ByVal contractNo As String)
        Dim query As String = "SELECT lot, contractid FROM contracts WHERE contractno = @contractNo"

        Using connection As New SqlConnection(sqlServerConnectionString)
            Try
                connection.Open()

                Using cmd As New SqlCommand(query, connection)
                    cmd.Parameters.AddWithValue("@contractNo", contractNo) ' Use contractNo to fetch batches

                    Dim reader As SqlDataReader = cmd.ExecuteReader()
                    cmbbatch.Items.Clear() ' Ensure previous items are cleared

                    ' Read all batches and their corresponding contractIds for the selected contractNo
                    While reader.Read()
                        Dim batch As String = reader("lot").ToString()
                        Dim contractId As Integer = reader("contractid") ' Get the contractId

                        ' Add a new item to the ComboBox using batch as display and store contractId as Tag
                        Dim item As New ComboBoxItem() With {
                            .Batch = batch,
                            .ContractId = contractId
                        }
                        cmbbatch.Items.Add(item) ' Add each batch to the ComboBox
                    End While

                    If cmbbatch.Items.Count = 0 Then
                        MessageBox.Show("No batches found for the selected contract.")
                    End If
                End Using
            Catch ex As SqlException
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
    End Sub


    ' Handle selected item in cmbBatch
    Private Sub cmbBatch_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbbatch.SelectedIndexChanged
        ' Get the selected item (which is of type ComboBoxItem)
        Dim selectedItem As ComboBoxItem = CType(cmbbatch.SelectedItem, ComboBoxItem)

        ' Display the contractId of the selected batch
        If selectedItem IsNot Nothing Then
            lblcontractid.Text = "" & selectedItem.ContractId.ToString()

            ' Write the selected batch to txtBatch
            txtbatch.Text = selectedItem.Batch
        End If
    End Sub

    ' Class to hold batch and contractId together
    Public Class ComboBoxItem
        Public Property Batch As String
        Public Property ContractId As Integer


        ' Override ToString method to display batch in ComboBox
        Public Overrides Function ToString() As String
            Return Batch ' Display only the batch in the ComboBox

        End Function

    End Class
    Private Sub PopulateCode(ByVal fabricType As String)
        ' Define the SQL query to fetch ids and codes from the library
        Dim query As String

        ' Determine the query based on fabric type
        If fabricType = "تريكو" Then
            query = "SELECT id, code FROM library WHERE code LIKE 'k%'"
        ElseIf fabricType = "نسيج" Then
            query = "SELECT id, code FROM library WHERE code LIKE 'w%'"
        Else
            ' If no valid fabric type is provided, clear the combobox and exit

            Return
        End If

        ' Use a SqlConnection to connect to the SQL Server database
        Using connection As New SqlConnection(sqlServerConnectionString)
            Try
                connection.Open()
                Using cmd As New SqlCommand(query, connection)
                    ' Create a SqlDataReader to read the data
                    Dim reader As SqlDataReader = cmd.ExecuteReader()



                    ' Populate the cmbcodelib with KeyValuePairs of id and code
                    While reader.Read()
                        Dim code As String = reader("code").ToString()
                        Dim id As Integer = Convert.ToInt32(reader("id"))

                    End While


                End Using
            Catch ex As SqlException
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
    End Sub

    ' Method to fetch lib_code from the library table based on the selected id
    Private Sub FetchLibCodeData(ByVal libraryId As Integer)
        ' SQL query to fetch lib_code based on the selected library id
        Dim query As String = "SELECT lib_code FROM library WHERE id = @id"

        ' Use a SqlConnection to connect to the SQL Server database
        Using connection As New SqlConnection(sqlServerConnectionString)
            Try
                ' Open the connection
                connection.Open()

                ' Create a SqlCommand to execute the query
                Using cmd As New SqlCommand(query, connection)
                    ' Parameterize the query to prevent SQL injection
                    cmd.Parameters.AddWithValue("@id", libraryId)

                    ' Execute the command and retrieve the lib_code
                    Dim result As Object = cmd.ExecuteScalar()



                    ' Add each part of the lib_code string to a new row in the DataGridView

                End Using

            Catch ex As SqlException
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
    End Sub
    ' Event handler for the btnInsert button click
    Private Sub btnInsert_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnInsert.Click
        Try
            ' Validation: Check if txtworder contains 'r' or 's'
            Dim worderId As String = txtworder.Text

            ' Proceed only if worderId ends with "r"
            If worderId.EndsWith("r") Then
                ' Check if clbWorderIds has items
                If clbWorderIds.Items.Count > 0 Then
                    ' If there are items, ensure at least one is checked
                    If clbWorderIds.CheckedItems.Count = 0 Then
                        MessageBox.Show("Please select at least one Worder ID before proceeding.")
                        Return
                    End If
                End If
            End If
            If worderId.Length < 6 OrElse Not (worderId.All(Function(c) Char.IsLetterOrDigit(c) OrElse c = "/"c)) Then
                MessageBox.Show("Please enter a Worder ID with at least 6 alphanumeric characters (letters, digits, or slashes).")
                Return
            End If
            Dim containsRorS As Boolean = worderId.Contains("r") OrElse worderId.Contains("s")

            ' If worder contains 'r' or 's', validate other fields
            If containsRorS Then
                If String.IsNullOrWhiteSpace(txtstagefix.Text) OrElse
           String.IsNullOrWhiteSpace(txtstagereason.Text) OrElse
           String.IsNullOrWhiteSpace(txtfrom.Text) OrElse
           String.IsNullOrWhiteSpace(txtrefno.Text) OrElse
           String.IsNullOrWhiteSpace(txtdefect.Text) Then
                    MessageBox.Show("Please fill in Stage Fix, Stage Reason, From, and Defect fields before proceeding.")
                    Return
                End If
            End If

            ' Get the selected QC ID

            Dim contractId As Integer = Integer.Parse(lblcontractid.Text)
            Dim qtyM As Decimal = Decimal.Parse(txtqtym.Text)
            Dim qtyKg As Decimal = Decimal.Parse(txtqtykg.Text)

            Dim deliveryDate As DateTime = dtpDeliveryDate.Value

            ' SQL query to check if worderid already exists
            Dim checkWorderIdQuery As String = "SELECT COUNT(*) FROM techdata WHERE worderid = @worderid"

            ' SQL insert query for techdata table
            Dim insertTechDataQuery As String = "INSERT INTO techdata (contract_id, worderid, qty_m, qty_kg, Delivery_Dat, created_date, InsertedBy, date, stagefix, stagereason, fromdep, defect,ref_no) " &
                                        "VALUES (@contract_id, @worderid, @qty_m, @qty_kg, @Delivery_Dat, @created_date, @InsertedBy, @date, @stagefix, @stagereason, @fromdep, @defect,@refno)"

            ' SQL insert query for worder_status table
            Dim insertWorderStatusQuery As String = "INSERT INTO worder_status (worderid, status) VALUES (@worderid, @status)"
            Dim updateWorderStatusQuery As String = "UPDATE worder_status SET status = 'Inactive' WHERE worderid = @worderid"

            ' Open SQL Server connection and execute the queries
            Using connection As New SqlConnection(sqlServerConnectionString)
                Try
                    connection.Open()
                    ' Check if worderid already exists
                    Using checkCmd As New SqlCommand(checkWorderIdQuery, connection)
                        checkCmd.Parameters.AddWithValue("@worderid", worderId)
                        Dim recordCount As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())

                        If recordCount > 0 Then
                            ' worderid already exists, show a message and exit
                            MessageBox.Show("Worder ID already exists. Please enter a unique Worder ID.")
                            Return
                        End If
                    End Using
                    ' Begin a transaction for both insertions
                    Using transaction As SqlTransaction = connection.BeginTransaction()

                        ' Insert into techdata table
                        Using cmdTechData As New SqlCommand(insertTechDataQuery, connection, transaction)
                            ' Add parameters to prevent SQL injection

                            cmdTechData.Parameters.AddWithValue("@contract_id", contractId)
                            cmdTechData.Parameters.AddWithValue("@worderid", worderId)
                            cmdTechData.Parameters.AddWithValue("@qty_m", qtyM)
                            cmdTechData.Parameters.AddWithValue("@qty_kg", qtyKg)

                            cmdTechData.Parameters.AddWithValue("@Delivery_Dat", deliveryDate)
                            cmdTechData.Parameters.AddWithValue("@created_date", DateTime.Now.Date)
                            cmdTechData.Parameters.AddWithValue("@date", DateTime.Now)
                            cmdTechData.Parameters.AddWithValue("@InsertedBy", LoggedInUsername)
                            cmdTechData.Parameters.AddWithValue("@stagefix", txtstagefix.Text)
                            cmdTechData.Parameters.AddWithValue("@stagereason", txtstagereason.Text)
                            cmdTechData.Parameters.AddWithValue("@fromdep", txtfrom.Text)
                            cmdTechData.Parameters.AddWithValue("@defect", txtdefect.Text)
                            cmdTechData.Parameters.AddWithValue("@refno", txtrefno.Text)

                            ' Execute the insert command for techdata
                            Dim rowsAffectedTechData As Integer = cmdTechData.ExecuteNonQuery()

                            ' Check if the techdata insertion succeeded
                            If rowsAffectedTechData > 0 Then
                                ' Initialize concatenatedSpeeds to an empty string

                                ' Insert "Active" status for txtworder value
                                Using cmdWorderStatus As New SqlCommand(insertWorderStatusQuery, connection, transaction)
                                    cmdWorderStatus.Parameters.AddWithValue("@worderid", worderId)
                                    cmdWorderStatus.Parameters.AddWithValue("@status", "Active")
                                    cmdWorderStatus.ExecuteNonQuery()
                                End Using

                                ' Loop through checked WorderIds and update their status to "Inactive"
                                For Each checkedItem As String In clbWorderIds.CheckedItems
                                    ' Check if the worderid exists
                                    Using checkCmd As New SqlCommand("SELECT COUNT(*) FROM worder_status WHERE worderid = @worderid", connection, transaction)
                                        checkCmd.Parameters.AddWithValue("@worderid", checkedItem)
                                        Dim recordCount As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())

                                        If recordCount > 0 Then
                                            ' Update to "Inactive"
                                            Using updateCmd As New SqlCommand(updateWorderStatusQuery, connection, transaction)
                                                updateCmd.Parameters.AddWithValue("@worderid", checkedItem)
                                                updateCmd.ExecuteNonQuery()
                                            End Using
                                        Else
                                            ' Insert with "Inactive" status
                                            Using insertCmd As New SqlCommand(insertWorderStatusQuery, connection, transaction)
                                                insertCmd.Parameters.AddWithValue("@worderid", checkedItem)
                                                insertCmd.Parameters.AddWithValue("@status", "Inactive")
                                                insertCmd.ExecuteNonQuery()
                                            End Using
                                        End If
                                    End Using
                                Next

                                ' Commit the transaction after both insertions
                                transaction.Commit()

                                ' Retrieve the contract number from the contracts table
                                Dim contractNo As String = GetContractNoById(contractId)

                                ' Inform the user
                                MessageBox.Show("Data inserted successfully into techdata and speedproccess tables!")

                                ' Initialize the email body with HTML table structure
                                Dim emailBody As New StringBuilder()
                                emailBody.AppendLine($"<h2>تم إنشاء أمر شغل .</h2>")
                                emailBody.AppendLine("<h3>تفاصيل البيانات:</h3>")
                                emailBody.AppendLine("<table border='1' style='border-collapse: collapse; width: 100%;'>")
                                emailBody.AppendLine("<tr>")
                                emailBody.AppendLine("<th>رقم التعاقد</th>")
                                emailBody.AppendLine("<th>رقم أمر الشغل</th>")
                                emailBody.AppendLine("<th>الكمية بالمتر</th>")
                                emailBody.AppendLine("<th>الكمية بالكيلو</th>")
                                emailBody.AppendLine("<th>تاريخ التسليم</th>")
                                emailBody.AppendLine("<th>إصلاح المرحلة</th>")
                                emailBody.AppendLine("<th>سبب المرحلة</th>")
                                emailBody.AppendLine("<th>من القسم</th>")
                                emailBody.AppendLine("<th>العيب</th>")
                                emailBody.AppendLine("</tr>")
                                emailBody.AppendLine("<tr>")
                                emailBody.AppendLine($"<td>{contractNo}</td>") ' Use contractNo instead of contractId
                                emailBody.AppendLine($"<td>{worderId}</td>")
                                emailBody.AppendLine($"<td>{qtyM}</td>")
                                emailBody.AppendLine($"<td>{qtyKg}</td>")
                                emailBody.AppendLine($"<td>{deliveryDate.ToShortDateString()}</td>")
                                emailBody.AppendLine($"<td>{txtstagefix.Text}</td>")
                                emailBody.AppendLine($"<td>{txtstagereason.Text}</td>")
                                emailBody.AppendLine($"<td>{txtfrom.Text}</td>")
                                emailBody.AppendLine($"<td>{txtdefect.Text}</td>")
                                emailBody.AppendLine("</tr>")
                                emailBody.AppendLine("</table>")

                                ' Send email notification
                                Dim subject As String = "Tech Data Inserted Successfully"
                                Dim body As String = emailBody.ToString()
                                SendEmailNotification(subject, body)

                                ' Clear all form controls after successful insertion
                                txtworder.Text = ""
                                txtqtym.Text = ""
                                txtqtykg.Text = ""
                                txtstagefix.Text = ""
                                txtstagereason.Text = ""
                                txtfrom.Text = ""
                                txtdefect.Text = ""

                                lblcontractid.Text = ""
                                dtpDeliveryDate.Value = DateTime.Now ' Reset to the current date

                                ' Clear combo boxes
                                cmbcontractno.SelectedIndex = -1 ' Clear cmbcontractno selection
                                cmbbatch.SelectedIndex = -1 ' Clear cmbbatch selection
                            Else
                                ' Rollback if techdata insertion failed
                                transaction.Rollback()
                                MessageBox.Show("Insertion into techdata failed.")
                            End If
                        End Using
                    End Using
                Catch ex As SqlException
                    MessageBox.Show("Error: " & ex.Message)
                End Try
            End Using
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub

    Private Function GetContractNoById(ByVal contractId As Integer) As String
        Dim contractNo As String = String.Empty
        Try
            Using connection As New SqlConnection(sqlServerConnectionString)
                ' SQL query to get the contract number from contracts table
                Dim query As String = "SELECT contractno FROM contracts WHERE contractid = @contractid"
                Dim cmd As New SqlCommand(query, connection)
                cmd.Parameters.AddWithValue("@contractid", contractId)

                connection.Open()
                ' Execute the query and retrieve the contract number
                Dim result = cmd.ExecuteScalar()
                If result IsNot Nothing Then
                    contractNo = result.ToString()
                End If
                connection.Close()
            End Using
        Catch ex As SqlException
            MessageBox.Show("Error retrieving contract number: " & ex.Message)
        End Try
        Return contractNo
    End Function



    Private Function GetPublicName(ByVal username As String) As String
        Dim publicName As String = String.Empty
        Try
            Using connection As New SqlConnection(sqlServerConnectionString)
                ' SQL query to get the public_name from dep_users where username matches
                Dim query As String = "SELECT public_name FROM dep_users WHERE username = @username"
                Dim cmd As New SqlCommand(query, connection)
                cmd.Parameters.AddWithValue("@username", username)

                connection.Open()
                ' Execute the query and retrieve the public_name
                Dim result = cmd.ExecuteScalar()
                If result IsNot Nothing Then
                    publicName = result.ToString()
                End If
                connection.Close()
            End Using
        Catch ex As SqlException
            MessageBox.Show("Error retrieving public name: " & ex.Message)
        End Try
        Return publicName
    End Function
    Private Sub SendEmailNotification(subject As String, body As String)
        Try
            Using conn As New SqlConnection(sqlServerConnectionString)
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
                .Body = body,
                .IsBodyHtml = True ' Enable HTML formatting
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




