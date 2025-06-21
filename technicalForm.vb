Imports MySql.Data.MySqlClient
Imports System.Data.SqlClient
Imports Excel = Microsoft.Office.Interop.Excel ' For Excel interop
Imports OfficeOpenXml ' Add this line for EPPlus
Imports System.IO
Public Class technicalform
    ' SQL Server connection string
    Dim sqlServerConnectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"
    ' Define the connection string to connect to the MySQL database
    Dim connectionString As String = "Server=150.1.1.7;Database=wm;Uid=root1;Pwd=WMg2024$;"


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
        FetchBatchData()
        Fetchbatch1()
        FetchBatchDataqc()



    End Sub
    Private Sub FetchBatchData()
        Dim batchNo As String = txtbatch.Text ' Get batch number from the textbox
        If String.IsNullOrEmpty(batchNo) Then
            dataGridbatch.DataSource = Nothing ' Clear the DataGridView if no batch number is entered
            Return
        End If

        ' mySQL query to fetch data based on the batch number
        Dim query As String = "SELECT qc2.id,qc2.batch_no, qc2.d1 AS 'Raw before width', qc2.d2 AS 'Raw after width',qc2.d3 as 'Weight of m2 before',qc2.d4 as 'Weight of m2 after', case WHEN qc2.d5 = 1 THEN 'Starch' WHEN qc2.d5 = 2 THEN 'PVA' WHEN qc2.d5 = 3 THEN 'PVA / Starch' WHEN qc2.d5 = 4 THEN 'No' ELSE 'Unknown' END AS 'pva / Starch',mix_rate.name as 'Mixing Percentage',d7 as'Rupture Warp',d8 as 'Rupture Weft',case WHEN qc2.d9 = 1 THEN 'Accept' WHEN qc2.d9 = 2 THEN 'Reject' ELSE 'Unknown' END AS 'Ruture Result',case WHEN qc2.d10 = 1 THEN 'Accept' WHEN qc2.d10 = 2 THEN 'Reject' ELSE 'Unknown' END AS 'Color Fastness to water',d11 as 'Tear Warp',d12 as 'Tear Weft',case WHEN qc2.d13 = 1 THEN 'Accept' WHEN qc2.d13 = 2 THEN 'Reject' ELSE 'Unknown' END AS 'Tear Result',case WHEN qc2.d14 = 1 THEN 'Accept' WHEN qc2.d14 = 2 THEN 'Reject' ELSE 'Unknown' END AS 'Color Fastness for Washing',case WHEN qc2.d15 = 1 THEN 'Accept' WHEN qc2.d15 = 2 THEN 'Reject' ELSE 'Unknown' END AS 'Color Fastness for Mercerization',d16 as' Notes' FROM qc2 left join mix_rate on qc2.d6 = mix_rate.id WHERE batch_no = @batchNo"

        ' Use a MySqlConnection to connect to the MySQL database
        Using connection As New MySqlConnection(connectionString)
            Try
                ' Open the connection
                connection.Open()

                ' Create a MySqlCommand to execute the query
                Using cmd As New MySqlCommand(query, connection)
                    ' Parameterize the query to prevent SQL injection
                    cmd.Parameters.AddWithValue("@batchNo", batchNo)

                    ' Create a MySqlDataAdapter to fill the DataGridView with data
                    Dim adapter As New MySqlDataAdapter(cmd)
                    Dim table As New DataTable()

                    ' Fill the DataTable with data
                    adapter.Fill(table)

                    ' Add a checkbox column if it does not already exist
                    If dataGridbatch.Columns("Select") Is Nothing Then
                        Dim chkColumn As New DataGridViewCheckBoxColumn()
                        chkColumn.Name = "Select"
                        chkColumn.HeaderText = "Select"
                        chkColumn.Width = 50 ' Adjust the width of the checkbox column
                        chkColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                        dataGridbatch.Columns.Insert(0, chkColumn) ' Add the checkbox column at the start
                    End If
                    ' Set the DataSource of the DataGridView to display the data
                    dataGridbatch.DataSource = table

                    ' Center the content and headers of each column and set the width
                    For Each column As DataGridViewColumn In dataGridbatch.Columns
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                        column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter ' Center the header
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells ' Set auto size based on displayed cells
                    Next

                    ' Optionally, auto resize the columns to fit the content
                    dataGridbatch.AutoResizeColumns()
                End Using

            Catch ex As MySqlException
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
    End Sub
    Private Sub FetchBatchDataqc()
        Dim batchNo As String = txtbatch.Text ' Get batch number from the textbox
        If String.IsNullOrEmpty(batchNo) Then
            dataGridbatchqc.DataSource = Nothing ' Clear the DataGridView if no batch number is entered
            Return
        End If

        ' mySQL query to fetch data based on the batch number
        Dim query As String = "SELECT qc2.batch_no,qc1.d1 as 'Raw Moisture ',qc1.d2 as 'raw width ', case WHEN qc1.d3 = 1 THEN 'نشا' WHEN qc1.d3 = 2 THEN 'pva' WHEN qc1.d3 = 3 THEN 'pva / نشا' WHEN qc1.d3 = 4 THEN 'لايوجد' ELSE 'Unknown' END AS 'Pva', mix_rate.name as 'Mix Rate ', qc1.d4 as 'إبرة شق ', case WHEN qc1.d5 = 1 THEN 'عالى - High' WHEN qc1.d5 = 2 THEN 'متوسط - Medium' WHEN qc1.d5 = 3 THEN 'لايوجد -  None' ELSE 'Unknown' END AS 'التفصيد', case WHEN qc1.d6 = 1 THEN 'جيده  - Good' WHEN qc1.d6 = 2 THEN 'متوسطه  - Medium' WHEN qc1.d6 = 3 THEN 'لايوجدضعيفه -  Weak' ELSE 'Unknown' END AS 'المتانه', qc1.d7 as 'ملاحظات الفحص' FROM qc1 left join mix_rate on qc1.mix_rate = mix_rate.id left join qc2 on qc1.inv_id = qc2.inv_id WHERE batch_no = @batchNo"

        ' Use a MySqlConnection to connect to the MySQL database
        Using connection As New MySqlConnection(connectionString)
            Try
                ' Open the connection
                connection.Open()

                ' Create a MySqlCommand to execute the query
                Using cmd As New MySqlCommand(query, connection)
                    ' Parameterize the query to prevent SQL injection
                    cmd.Parameters.AddWithValue("@batchNo", batchNo)

                    ' Create a MySqlDataAdapter to fill the DataGridView with data
                    Dim adapter As New MySqlDataAdapter(cmd)
                    Dim table As New DataTable()

                    ' Fill the DataTable with data
                    adapter.Fill(table)


                    ' Set the DataSource of the DataGridView to display the data
                    dataGridbatchqc.DataSource = table

                    ' Center the content and headers of each column and set the width
                    For Each column As DataGridViewColumn In dataGridbatch.Columns
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                        column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter ' Center the header
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells ' Set auto size based on displayed cells
                    Next

                    ' Optionally, auto resize the columns to fit the content
                    dataGridbatchqc.AutoResizeColumns()
                End Using

            Catch ex As MySqlException
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
    End Sub
    Private Sub Fetchbatch1()
        Dim ContractId As String = lblcontractid.Text ' Get contract number from the combobox
        If String.IsNullOrEmpty(ContractId) Then
            dataGridbatch1.DataSource = Nothing ' Clear the DataGridView if no contract number is entered
            cmbcodelib.Items.Clear()
            cmbbatch.SelectedIndex = -1 ' Clear cmbcodelib if no contract number
           

            Return
        End If

        ' SQL query to fetch data based on the contract number
        Dim query As String = "SELECT c.ContractID as 'ID', c.ContractNo as 'رقم التعاقد', f.fabricType_ar as 'نوع التعاقد', c.ContractDate as 'تاريخ التعاقد', " &
                              "cl.code as 'كود العميل', c.Material as 'الخامة', c.refno as 'رقم الاذن', c.Batch as 'رقم الرساله', c.QuantityM as 'الكمية متر', c.QuantityK as 'الكمية كيلو', " &
                              "c.FabricCode as 'كود الخامه', c.WidthReq as 'العرض المطلوب', c.WeightM as 'الوزن المطلوب', c.RollM as 'أمتار التوب المطلوبة', c.Notes as 'ملاحظات' " &
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
                           
                        
                        End If

                        
                    Else
                        cmbcodelib.Items.Clear() ' Clear cmbcodelib if no rows available


                    End If
                End Using
            Catch ex As SqlException
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
    End Sub
   


    ' Declare a dictionary to store contractId against contractNo
    Private contractDictionary As New Dictionary(Of String, Integer)

    ' Populate cmbContractNo with unique contract numbers from SQL Server
    Private Sub PopulateContractNo()
        Dim query As String = "SELECT worderid FROM techdata"
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
        Dim query As String = "SELECT batch, contractid FROM contracts WHERE contractno = @contractNo"

        Using connection As New SqlConnection(sqlServerConnectionString)
            Try
                connection.Open()

                Using cmd As New SqlCommand(query, connection)
                    cmd.Parameters.AddWithValue("@contractNo", contractNo) ' Use contractNo to fetch batches

                    Dim reader As SqlDataReader = cmd.ExecuteReader()
                    cmbbatch.Items.Clear() ' Ensure previous items are cleared

                    ' Read all batches and their corresponding contractIds for the selected contractNo
                    While reader.Read()
                        Dim batch As String = reader("batch").ToString()
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
            cmbcodelib.Items.Clear()
            Return
        End If

        ' Use a SqlConnection to connect to the SQL Server database
        Using connection As New SqlConnection(sqlServerConnectionString)
            Try
                connection.Open()
                Using cmd As New SqlCommand(query, connection)
                    ' Create a SqlDataReader to read the data
                    Dim reader As SqlDataReader = cmd.ExecuteReader()

                    ' Clear the cmbcodelib before populating it
                    cmbcodelib.Items.Clear()

                    ' Populate the cmbcodelib with KeyValuePairs of id and code
                    While reader.Read()
                        Dim code As String = reader("code").ToString()
                        Dim id As Integer = Convert.ToInt32(reader("id"))
                        cmbcodelib.Items.Add(New KeyValuePair(Of Integer, String)(id, code))
                    End While

                    ' Set the display member to show only the code
                    cmbcodelib.DisplayMember = "Value" ' This will display the 'code' in the combo box
                    cmbcodelib.ValueMember = "Key" ' This will allow us to access the 'id' value
                End Using
            Catch ex As SqlException
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
    End Sub
    Private Sub cmbcodelib_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbcodelib.SelectedIndexChanged
        ' Check if an item is selected
        If cmbcodelib.SelectedItem IsNot Nothing Then
            ' Retrieve the selected KeyValuePair
            Dim selectedItem As KeyValuePair(Of Integer, String) = CType(cmbcodelib.SelectedItem, KeyValuePair(Of Integer, String))

            ' Display the 'id' in lbllibcode
            lbllibcode.Text = selectedItem.Key.ToString() ' Set the label text to the selected id

            ' Fetch data based on the selected id
            FetchLibCodeData(selectedItem.Key) ' Pass the id (Key) to FetchLibCodeData
        End If
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

                    If result IsNot Nothing Then
                        ' Split the lib_code string using '-' as a delimiter
                        Dim libCodeItems As String() = result.ToString().Split("-"c)

                        ' Clear the DataGridView first
                        dgvLibCode.Rows.Clear()

                        ' Add columns if they haven't been added already
                        If dgvLibCode.Columns.Count = 0 Then
                            ' Add a column to display lib_code
                            dgvLibCode.Columns.Add("libCodeColumn", "Library Code")

                            ' Add another column for speed input
                            Dim speedColumn As New DataGridViewTextBoxColumn()
                            speedColumn.HeaderText = "Speed"
                            speedColumn.Name = "speedColumn"
                            dgvLibCode.Columns.Add(speedColumn)
                        End If

                        ' Add each part of the lib_code string to a new row in the DataGridView
                        For Each item As String In libCodeItems
                            dgvLibCode.Rows.Add(item.Trim(), "") ' Add empty cell for Speed
                        Next

                        ' Automatically resize the columns based on content
                        dgvLibCode.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
                    Else
                        ' If no result is found, clear the DataGridView
                        dgvLibCode.Rows.Clear()
                    End If
                End Using

            Catch ex As SqlException
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
    End Sub


    ' Helper method to get the selected QC ID from DataGridView
    Private Function GetSelectedQcId() As Integer
        For Each row As DataGridViewRow In dataGridbatch.Rows
            Dim isChecked As Boolean = Convert.ToBoolean(row.Cells("Select").Value)
            If isChecked Then
                Return Convert.ToInt32(row.Cells("id").Value) ' Return the qc2.id of the selected row
            End If
        Next
        ' If no record is selected, throw an exception
        Throw New Exception("No QC record selected.")
    End Function

    ' Event handler for the btnInsert button click
    Private Sub btnInsert_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnInsert.Click
        Try

            ' Get the selected QC ID
            Dim selectedQcId As Integer = GetSelectedQcId() ' Method to get selected QC ID from DataGridView
            Dim contractId As Integer = Integer.Parse(lblcontractid.Text)

            Dim codelib As Integer = Integer.Parse(lbllibcode.Text)



            ' SQL insert query for technicaldata table
            Dim inserttechnicaldataQuery As String = "INSERT INTO technical_data (qc_id, contract_id, code_lib, created_date, InsertedBy, date) " &
                                                "VALUES (@qc_id, @contract_id, @code_lib, @created_date, @InsertedBy, @date)"


            ' SQL insert query for speedproccess table
            Dim insertSpeedProccessQuery As String = "INSERT INTO speedproccess (code_lib, speed) VALUES (@code_lib, @speed)"

            ' Define the SQL insert query for the qc_lab table
            Dim insertQuery As String = "INSERT INTO qc_lab (qc_id, batch_no, d1, d2, d3, d4, d5, d6, d7, d8, d9, d10, d11, d12, d13, d14, d15, d16, mix_rate) " &
                                        "VALUES (@qc_id, @batch_no, @d1, @d2, @d3, @d4, @d5, @d6, @d7, @d8, @d9, @d10, @d11, @d12, @d13, @d14, @d15, @d16, @mix_rate)"

            ' Open SQL Server connection and execute the queries
            Using connection As New SqlConnection(sqlServerConnectionString)
                Try
                    connection.Open()
                    ' Check if worderid already exists





                    ' Begin a transaction for both insertions
                    Using transaction As SqlTransaction = connection.BeginTransaction()

                        ' Insert into technicaldata table
                        Using cmdtechnicaldata As New SqlCommand(inserttechnicaldataQuery, connection, transaction)
                            ' Add parameters to prevent SQL injection
                            cmdtechnicaldata.Parameters.AddWithValue("@qc_id", selectedQcId)
                            cmdtechnicaldata.Parameters.AddWithValue("@contract_id", contractId)
                            cmdtechnicaldata.Parameters.AddWithValue("@code_lib", codelib)
                            cmdtechnicaldata.Parameters.AddWithValue("@created_date", DateTime.Now.Date)
                            cmdtechnicaldata.Parameters.AddWithValue("@date", DateTime.Now)
                            cmdtechnicaldata.Parameters.AddWithValue("@InsertedBy", LoggedInUsername)
                            ' Execute the insert command for technical_data
                            Dim rowsAffectedtechnicaldata As Integer = cmdtechnicaldata.ExecuteNonQuery()

                            ' Check if the technicaldata insertion succeeded
                            If rowsAffectedtechnicaldata > 0 Then
                                ' Initialize concatenatedSpeeds to an empty string
                                Dim concatenatedSpeeds As String = String.Empty

                                ' Loop through each row in the DataGridView (dgvLibCode)
                                For Each row As DataGridViewRow In dgvLibCode.Rows
                                    If Not row.IsNewRow Then
                                        Dim speedValue As String = row.Cells("speedColumn").Value.ToString().Trim()

                                        ' Check if speedValue is empty and replace it with "0"
                                        If String.IsNullOrEmpty(speedValue) Then
                                            speedValue = "0"
                                        End If

                                        ' Concatenate speed values, separated by "-"
                                        If String.IsNullOrEmpty(concatenatedSpeeds) Then
                                            concatenatedSpeeds = speedValue
                                        Else
                                            concatenatedSpeeds &= "-" & speedValue
                                        End If
                                    End If
                                Next


                                ' Prepare to insert into speedproccess table
                                Using cmdSpeedProccess As New SqlCommand(insertSpeedProccessQuery, connection, transaction)

                                    cmdSpeedProccess.Parameters.AddWithValue("@code_lib", codelib)
                                    cmdSpeedProccess.Parameters.AddWithValue("@speed", concatenatedSpeeds) ' Concatenated speed values

                                    ' Execute the insert command for speedproccess
                                    cmdSpeedProccess.ExecuteNonQuery()
                                End Using




                                ' Loop through the rows in the DataGridView (dataGridbatch)
                                For Each row As DataGridViewRow In dataGridbatch.Rows
                                    ' Ensure the row is not new and is selected
                                    If Not row.IsNewRow AndAlso Convert.ToBoolean(row.Cells("Select").Value) = True Then
                                        ' Prepare to insert the data
                                        Using cmdInsert As New SqlCommand(insertQuery, connection, transaction)
                                            ' Add parameters to the SQL query to avoid SQL injection
                                            cmdInsert.Parameters.AddWithValue("@qc_id", row.Cells("id").Value)
                                            cmdInsert.Parameters.AddWithValue("@batch_no", row.Cells("batch_no").Value)
                                            cmdInsert.Parameters.AddWithValue("@d1", row.Cells("Raw before width").Value)
                                            cmdInsert.Parameters.AddWithValue("@d2", row.Cells("Raw after width").Value)
                                            cmdInsert.Parameters.AddWithValue("@d3", row.Cells("Weight of m2 before").Value)
                                            cmdInsert.Parameters.AddWithValue("@d4", row.Cells("Weight of m2 after").Value)
                                            cmdInsert.Parameters.AddWithValue("@d5", row.Cells("pva / Starch").Value)
                                            cmdInsert.Parameters.AddWithValue("@d6", row.Cells("Mixing Percentage").Value)
                                            cmdInsert.Parameters.AddWithValue("@d7", row.Cells("Rupture Warp").Value)
                                            cmdInsert.Parameters.AddWithValue("@d8", row.Cells("Rupture Weft").Value)
                                            cmdInsert.Parameters.AddWithValue("@d9", row.Cells("Ruture Result").Value)
                                            cmdInsert.Parameters.AddWithValue("@d10", row.Cells("Color Fastness to water").Value)
                                            cmdInsert.Parameters.AddWithValue("@d11", row.Cells("Tear Warp").Value)
                                            cmdInsert.Parameters.AddWithValue("@d12", row.Cells("Tear Weft").Value)
                                            cmdInsert.Parameters.AddWithValue("@d13", row.Cells("Tear Result").Value)
                                            cmdInsert.Parameters.AddWithValue("@d14", row.Cells("Color Fastness for Washing").Value)
                                            cmdInsert.Parameters.AddWithValue("@d15", row.Cells("Color Fastness for Mercerization").Value)
                                            cmdInsert.Parameters.AddWithValue("@d16", row.Cells("Notes").Value)
                                            cmdInsert.Parameters.AddWithValue("@mix_rate", row.Cells("Mixing Percentage").Value)

                                            ' Execute the insert command
                                            cmdInsert.ExecuteNonQuery()
                                        End Using
                                    End If
                                Next

                                ' Commit the transaction after both insertions
                                transaction.Commit()


                                MessageBox.Show("Data inserted successfully into technicaldata and speedproccess tables!")
                                ' Clear all form controls after successful insertion

                                lbllibcode.Text = ""
                                lblcontractid.Text = ""

                                dgvLibCode.Rows.Clear() ' Clear DataGridView
                                ' Clear combo boxes
                                cmbcontractno.SelectedIndex = -1 ' Clear cmbcontractno selection
                                cmbbatch.SelectedIndex = -1 ' Clear cmbbatch selection
                                cmbcodelib.SelectedIndex = -1

                            Else
                                ' Rollback if technicaldata insertion failed
                                transaction.Rollback()
                                MessageBox.Show("Insertion into technicaldata failed.")
                            End If
                        End Using
                    End Using
                Catch ex As SqlException
                    MessageBox.Show("Error: " & ex.Message)
                End Try
            End Using

        Catch ex As Exception
            ' If no QC record is selected, show a message
            MessageBox.Show("No QC record selected.")
        End Try
    End Sub


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


End Class




