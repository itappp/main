Imports System.Data.SqlClient
Imports System.Drawing.Printing
Imports System.Net
Imports System.Net.Mail
Imports System.Text
Imports MySql.Data.MySqlClient
Imports System.Drawing
Imports System.Text.RegularExpressions

Public Class ContractForm

    ' Connection string to SQL Server database
    Private connectionString As String = "Server=localhost;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"
    Private mysqlServerConnectionString As String = "Server=150.1.1.7;Database=wm;Uid=root1;Pwd=WMg2024$;"

    Private Sub SetFormControlsEnabled(ByVal enabled As Boolean)
        ' Enable/disable controls based on contract type selection.
        ' cmbcontracttype is always enabled.
        txtcontractno.ReadOnly = Not enabled
        dtpContract.Enabled = enabled
        cmbclientcode.Enabled = enabled
        dgvContractDetails.Enabled = enabled
        btninsert.Enabled = enabled
    End Sub

    ' Form Load Event to populate the contract type combo box and format DataGridView
    Private Sub ContractForm_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        SetFormControlsEnabled(False) ' Lock controls on load

        ' Load the last contract number
        LoadLastContractNumber()
        LoadFabricTypes()
        ' Access the logged-in username from the global variable
        lblUsername.Text = "Logged in as: " & LoggedInUsername
        ' Retrieve the public_name for the logged-in user
        Dim publicName As String = GetPublicName(LoggedInUsername)
        ' Attach the event handler manually using AddHandler
        AddHandler btnView.Click, AddressOf btnView_Click
        ' Optionally load the combo box with client codes from the database
        LoadClientCodes()
        ' Ensure ComboBoxes don't have any selected item by default
        cmbclientcode.SelectedIndex = -1
        cmbcontracttype.SelectedIndex = -1

        ' إعادة ترتيب الأعمدة: نوع التشكيل أول عمود، عدد ريب بعد ملاحظات
        dgvContractDetails.Columns.Clear()
        ' نوع التشكيل
        Dim formationTypeColumn As New DataGridViewComboBoxColumn()
        formationTypeColumn.Name = "FormationType"
        formationTypeColumn.HeaderText = "نوع التشكيل"
        formationTypeColumn.Items.AddRange("جسم", "اكسسوار")
        formationTypeColumn.Width = 120
        dgvContractDetails.Columns.Add(formationTypeColumn)
        ' الخامة
        dgvContractDetails.Columns.Add("Material", "الخامة")
        AddBatchColumn()
        AddLotColumn()
        dgvContractDetails.Columns.Add("QuantityM", "M الكمية ")
        dgvContractDetails.Columns.Add("QuantityK", "KG الكمية ")
        ' نسبه الهالك قبل رقم اذن العميل
        dgvContractDetails.Columns.Add("WastePercentage", "نسبه الهالك")
        dgvContractDetails.Columns.Add("RefNo", "رقم اذن العميل")
        dgvContractDetails.Columns.Add("Notes", "ملاحظات")
        ' عدد ريب
        Dim ribCountColumn As New DataGridViewTextBoxColumn()
        ribCountColumn.Name = "RibCount"
        ribCountColumn.HeaderText = "عدد ريب"
        ribCountColumn.Width = 80
        dgvContractDetails.Columns.Add(ribCountColumn)
        dgvContractDetails.Columns.Add("rollm", "أمتار التوب المطلوبة")
        dgvContractDetails.Columns.Add("weightm", "وزن المتر المربع المطلوب")
        dgvContractDetails.Columns.Add("widthreq", "العرض المطلوب")
        ' اللون
        Dim colorColumn As New DataGridViewComboBoxColumn()
        colorColumn.Name = "color"
        colorColumn.HeaderText = "اللون"
        colorColumn.DataSource = GetColorCodes()
        colorColumn.DisplayMember = "Display"
        colorColumn.ValueMember = "code"
        colorColumn.Width = 204
        Dim colorTable As DataTable = GetColorCodes()
        colorTable.Columns.Add("Display", GetType(String), "code + ' ' + name")
        colorColumn.DataSource = colorTable
        dgvContractDetails.Columns.Add(colorColumn)
        ' كود الخامه
        Dim fabricCodeColumn As New DataGridViewComboBoxColumn()
        fabricCodeColumn.Name = "fabriccode"
        fabricCodeColumn.HeaderText = "كود الخامه"
        fabricCodeColumn.DataSource = GetFabricCodes()
        fabricCodeColumn.DisplayMember = "code"
        fabricCodeColumn.ValueMember = "code"
        fabricCodeColumn.Width = 250
        dgvContractDetails.Columns.Add(fabricCodeColumn)

        ' Customize the DataGridView appearance
        CustomizeDataGridView()

        ' Make quantity columns read-only initially
        dgvContractDetails.Columns("QuantityM").ReadOnly = True
        dgvContractDetails.Columns("QuantityK").ReadOnly = True

        dtpContract.ShowCheckBox = True
        dtpContract.Checked = False
    End Sub

    Private WithEvents cmbColorEditing As ComboBox

    Private Sub LoadQC1DataFromSQL(ByVal batchNumbers As List(Of String))
        ' SQL Query to fetch data from the second table based on batch numbers
        Dim query As String = "SELECT id, batch_id as 'batch_no', lot, raw_befor_width as 'Raw before width', raw_after_width as 'Raw after width', " &
                          "raw_befor_weight as 'Weight of m2 before', raw_after_weight as 'Weight of m2 after', pva_Starch as 'pva / Starch', " &
                          "mix_rate as 'Mixing Percentage', tensile_weft as 'Rupture Weft', tensile_warp as 'Rupture Warp', " &
                          "tensile_result as 'Rupture Result', color_water as 'Color Fastness to water', tear_weft as 'Tear Weft', " &
                          "tear_warp as 'Tear Warp', tear_result as 'Tear Result', washing as 'Color Fastness for Washing', " &
                          "color_mercerize as 'Color Fastness for Mercerization', notes, username as 'user' " &
                          "FROM qc_raw_test WHERE batch_id IN (" & String.Join(",", batchNumbers.Select(Function(b) "'" & b & "'")) & ")"

        ' Create a DataTable to hold the results for the DataGridView
        Dim dt As New DataTable()

        ' Connection and command
        Using conn As New SqlConnection(connectionString)
            Dim cmd As New SqlCommand(query, conn)

            Try
                conn.Open()
                ' Fill the DataTable with data from the query
                Dim adapter As New SqlDataAdapter(cmd)
                adapter.Fill(dt)

                ' Check if DataTable has rows and then populate the DataGridView
                If dt.Rows.Count > 0 Then
                    ' Bind the DataTable to the DataGridView (dgvqc)
                    dgvqc.DataSource = dt
                    dgvqc.Refresh() ' Ensure the DataGridView is redrawn after binding

                    ' Customize the DataGridView appearance
                    dgvqc.AutoResizeColumnHeadersHeight() ' Resize the headers
                    dgvqc.DefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Bold) ' Set font to Bold and size 12
                    dgvqc.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter ' Center align content

                    ' Loop through each column and set header styles
                    For Each column As DataGridViewColumn In dgvqc.Columns
                        column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter ' Center header text
                        column.HeaderCell.Style.Font = New Font("Arial", 8, FontStyle.Bold) ' Set header font to bold
                        column.HeaderCell.Style.ForeColor = Color.White ' Set header font color to white
                        column.HeaderCell.Style.BackColor = Color.Blue ' Set header background color to dark blue
                    Next

                    ' Adjust column widths based on content
                    For Each column As DataGridViewColumn In dgvqc.Columns
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                    Next
                Else
                    dgvqc.DataSource = Nothing
                    MessageBox.Show("لم يتم عمل اختبارات من قبل معمل الجوده على هذه الرسالة")
                End If
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
    End Sub


    ' Method to add Batch ComboBox column
    Private Sub AddBatchColumn()
        Dim batchColumn As New DataGridViewComboBoxColumn()
        batchColumn.Name = "Batch"
        batchColumn.HeaderText = "رقم الرسالة"
        batchColumn.DataSource = GetBatchDetails()
        batchColumn.DisplayMember = "batch_id"
        batchColumn.ValueMember = "batch_id"
        dgvContractDetails.Columns.Add(batchColumn)
    End Sub

    ' Method to add Lot ComboBox column
    Private Sub AddLotColumn()
        Dim lotColumn As New DataGridViewComboBoxColumn()
        lotColumn.Name = "Lot"
        lotColumn.HeaderText = "اللوت"
        dgvContractDetails.Columns.Add(lotColumn)
    End Sub

    ' Method to get batch details from the database
    Private Function GetBatchDetails() As DataTable
        Dim dt As New DataTable()
        Try
            Using conn As New SqlConnection(connectionString)
                Dim query As String = "SELECT batch_id, lot FROM batch_details"
                Dim cmd As New SqlCommand(query, conn)
                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                dt.Load(reader)
                conn.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading batch details: " & ex.Message)
        End Try
        Return dt
    End Function

    Private Sub dgvContractDetails_CellValueChanged(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles dgvContractDetails.CellValueChanged
        If e.ColumnIndex = dgvContractDetails.Columns("Batch").Index Then
            Dim batchValue As String = dgvContractDetails.Rows(e.RowIndex).Cells("Batch").Value.ToString()
            Dim lotColumn As DataGridViewComboBoxCell = CType(dgvContractDetails.Rows(e.RowIndex).Cells("Lot"), DataGridViewComboBoxCell)

            ' Clear any existing selection in the Lot column
            lotColumn.Value = Nothing

            ' Refresh the Lot column with new data
            lotColumn.DataSource = GetLotsForBatch(batchValue)
            lotColumn.DisplayMember = "lot"
            lotColumn.ValueMember = "lot"

            ' Clear quantity values when batch changes
            dgvContractDetails.Rows(e.RowIndex).Cells("QuantityM").Value = Nothing
            dgvContractDetails.Rows(e.RowIndex).Cells("QuantityK").Value = Nothing

            ' Make quantity columns read-only when batch changes
            dgvContractDetails.Rows(e.RowIndex).Cells("QuantityM").ReadOnly = True
            dgvContractDetails.Rows(e.RowIndex).Cells("QuantityK").ReadOnly = True

            ' Collect all selected batch numbers
            Dim selectedBatchNumbers As New List(Of String)
            For Each row As DataGridViewRow In dgvContractDetails.Rows
                If Not row.IsNewRow AndAlso row.Cells("Batch").Value IsNot Nothing Then
                    selectedBatchNumbers.Add(row.Cells("Batch").Value.ToString())
                End If
            Next

            ' Load QC data for the selected batch numbers
            LoadQC1DataFromSQL(selectedBatchNumbers)
        ElseIf e.ColumnIndex = dgvContractDetails.Columns("Lot").Index Then
            Dim lotValue As Object = dgvContractDetails.Rows(e.RowIndex).Cells("Lot").Value
            If lotValue IsNot Nothing Then
                Dim status As Integer = GetLotStatus(lotValue.ToString())

                If status = 0 Then
                    dgvContractDetails.Rows(e.RowIndex).Cells("Lot").Style.BackColor = Color.Green
                    ' Enable quantity columns when lot is selected
                    dgvContractDetails.Rows(e.RowIndex).Cells("QuantityM").ReadOnly = False
                    dgvContractDetails.Rows(e.RowIndex).Cells("QuantityK").ReadOnly = False
                ElseIf status = 1 Then
                    dgvContractDetails.Rows(e.RowIndex).Cells("Lot").Style.BackColor = Color.Red
                    ' Keep quantity columns read-only for invalid lot
                    dgvContractDetails.Rows(e.RowIndex).Cells("QuantityM").ReadOnly = True
                    dgvContractDetails.Rows(e.RowIndex).Cells("QuantityK").ReadOnly = True
                ElseIf status = -1 Then
                    dgvContractDetails.Rows(e.RowIndex).Cells("Lot").Style.BackColor = Color.Yellow
                    ' Keep quantity columns read-only for invalid lot
                    dgvContractDetails.Rows(e.RowIndex).Cells("QuantityM").ReadOnly = True
                    dgvContractDetails.Rows(e.RowIndex).Cells("QuantityK").ReadOnly = True
                End If

                ' Clear quantity values when lot changes
                dgvContractDetails.Rows(e.RowIndex).Cells("QuantityM").Value = Nothing
                dgvContractDetails.Rows(e.RowIndex).Cells("QuantityK").Value = Nothing
            End If
        ElseIf e.ColumnIndex = dgvContractDetails.Columns("QuantityK").Index Then
            Dim lotValue As Object = dgvContractDetails.Rows(e.RowIndex).Cells("Lot").Value
            Dim quantityKValue As Object = dgvContractDetails.Rows(e.RowIndex).Cells("QuantityK").Value
            If lotValue IsNot Nothing AndAlso quantityKValue IsNot Nothing AndAlso IsNumeric(quantityKValue) Then
                Dim requestedQuantity As Decimal = Convert.ToDecimal(quantityKValue)
                Dim availableQuantity As Decimal = GetAvailableQuantity(lotValue.ToString(), "weightpk")

                ' Subtract quantities from contracts
                Dim usedQuantity As Decimal = GetUsedQuantityFromContracts(lotValue.ToString(), "QuantityK")
                availableQuantity -= usedQuantity

                If requestedQuantity > availableQuantity Then
                    MessageBox.Show("الكمية المطلوبة غير متاحة. الكمية المتاحة: " & availableQuantity & " كيلو.")
                    dgvContractDetails.Rows(e.RowIndex).Cells("QuantityK").Value = availableQuantity
                End If
            End If
        ElseIf e.ColumnIndex = dgvContractDetails.Columns("QuantityM").Index Then
            Dim lotValue As Object = dgvContractDetails.Rows(e.RowIndex).Cells("Lot").Value
            Dim quantityMValue As Object = dgvContractDetails.Rows(e.RowIndex).Cells("QuantityM").Value
            If lotValue IsNot Nothing AndAlso quantityMValue IsNot Nothing AndAlso IsNumeric(quantityMValue) Then
                Dim requestedQuantity As Decimal = Convert.ToDecimal(quantityMValue)
                Dim availableQuantity As Decimal = GetAvailableQuantity(lotValue.ToString(), "meter_quantity")

                ' Subtract quantities from contracts
                Dim usedQuantity As Decimal = GetUsedQuantityFromContracts(lotValue.ToString(), "QuantityM")
                availableQuantity -= usedQuantity

                If requestedQuantity > availableQuantity Then
                    MessageBox.Show("الكمية المطلوبة غير متاحة. الكمية المتاحة: " & availableQuantity & " متر.")
                    dgvContractDetails.Rows(e.RowIndex).Cells("QuantityM").Value = availableQuantity
                End If
            End If
        End If

        ' Check if any Lot cell is red or yellow
        Dim showInsertButton As Boolean = True
        For Each row As DataGridViewRow In dgvContractDetails.Rows
            If Not row.IsNewRow Then
                Dim lotCell As DataGridViewCell = row.Cells("Lot")
                If lotCell.Style.BackColor = Color.Red OrElse lotCell.Style.BackColor = Color.Yellow Then
                    showInsertButton = False
                    Exit For
                End If
            End If
        Next

        btninsert.Visible = showInsertButton
    End Sub

    Private Function GetAvailableQuantity(lot As String, columnName As String) As Decimal
        Dim availableQuantity As Decimal = 0
        Try
            Using conn As New SqlConnection(connectionString)
                Dim query As String = "SELECT " & columnName & " FROM batch_details WHERE lot = @lot"
                Dim cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@lot", lot)
                conn.Open()
                Dim result As Object = cmd.ExecuteScalar()
                If result IsNot Nothing Then
                    availableQuantity = Convert.ToDecimal(result)
                End If
                conn.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading available quantity: " & ex.Message)
        End Try
        Return availableQuantity
    End Function

    Private Function GetUsedQuantityFromContracts(lot As String, columnName As String) As Decimal
        Dim usedQuantity As Decimal = 0
        Try
            Using conn As New SqlConnection(connectionString)
                Dim query As String = "SELECT SUM(" & columnName & ") FROM contracts WHERE lot = @lot"
                Dim cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@lot", lot)
                conn.Open()
                Dim result As Object = cmd.ExecuteScalar()
                If result IsNot Nothing AndAlso result IsNot DBNull.Value Then
                    usedQuantity = Convert.ToDecimal(result)
                End If
                conn.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading used quantity: " & ex.Message)
        End Try
        Return usedQuantity
    End Function


    Private Function GetLotStatus(lot As String) As Integer
        Dim status As Integer = -1
        Try
            Using conn As New SqlConnection(connectionString)
                Dim query As String = "SELECT status FROM batch_lot_status WHERE lot = @lot"
                Dim cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@lot", lot)
                conn.Open()
                Dim result As Object = cmd.ExecuteScalar()
                If result IsNot Nothing Then
                    status = Convert.ToInt32(result)
                End If
                conn.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading lot status: " & ex.Message)
        End Try
        Return status
    End Function



    ' Method to get lots for a specific batch
    Private Function GetLotsForBatch(batchId As String) As DataTable
        Dim dt As New DataTable()
        Try
            Using conn As New SqlConnection(connectionString)
                Dim query As String = "SELECT lot FROM batch_details WHERE batch_id = @batch_id"
                Dim cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@batch_id", batchId)
                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                dt.Load(reader)
                conn.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading lots: " & ex.Message)
        End Try
        Return dt
    End Function

    ' Method to customize the appearance of DataGridView
    Private Sub CustomizeDataGridView()
        ' Set the font and row height
        dgvContractDetails.Font = New Font("Arial", 12) ' Set font size
        dgvContractDetails.RowTemplate.Height = 30 ' Set row height

        ' Customize column width
        If dgvContractDetails.Columns.Contains("Material") Then dgvContractDetails.Columns("Material").Width = 237
        If dgvContractDetails.Columns.Contains("Batch") Then dgvContractDetails.Columns("Batch").Width = 120
        If dgvContractDetails.Columns.Contains("Lot") Then dgvContractDetails.Columns("Lot").Width = 120
        If dgvContractDetails.Columns.Contains("QuantityM") Then dgvContractDetails.Columns("QuantityM").Width = 130
        If dgvContractDetails.Columns.Contains("QuantityK") Then dgvContractDetails.Columns("QuantityK").Width = 120
        If dgvContractDetails.Columns.Contains("RefNo") Then dgvContractDetails.Columns("RefNo").Width = 150
        If dgvContractDetails.Columns.Contains("Notes") Then dgvContractDetails.Columns("Notes").Width = 300  ' Adjust width for Notes
        If dgvContractDetails.Columns.Contains("rollm") Then dgvContractDetails.Columns("rollm").Width = 100
        If dgvContractDetails.Columns.Contains("weightm") Then dgvContractDetails.Columns("weightm").Width = 130
        If dgvContractDetails.Columns.Contains("widthreq") Then dgvContractDetails.Columns("widthreq").Width = 110
        If dgvContractDetails.Columns.Contains("fabriccode") Then dgvContractDetails.Columns("fabriccode").Width = 250
        If dgvContractDetails.Columns.Contains("WastePercentage") Then dgvContractDetails.Columns("WastePercentage").Width = 120

        ' Set alternating row colors
        dgvContractDetails.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray
        ' Set header style
        dgvContractDetails.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 12, FontStyle.Bold)
        dgvContractDetails.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvContractDetails.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue
        dgvContractDetails.EnableHeadersVisualStyles = False

        ' Center-align column headers and cell contents
        For Each col As DataGridViewColumn In dgvContractDetails.Columns
            col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Next

        ' Optional: Set grid line style
        dgvContractDetails.GridColor = Color.Black
        dgvContractDetails.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
    End Sub

    ' Method to load client codes into the ComboBox (cmbclientcode)
    Private Sub LoadClientCodes()
        Try
            Using conn As New SqlConnection(connectionString)
                Dim query As String = "SELECT id, code FROM Clients" ' Adjust the query to get both id and code
                Dim cmd As New SqlCommand(query, conn)
                Dim dt As New DataTable()

                ' Open the connection
                conn.Open()

                ' Fill the DataTable with the result of the query
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                dt.Load(reader)

                ' Set the ComboBox properties
                cmbclientcode.DataSource = dt
                cmbclientcode.DisplayMember = "code"  ' Column to display
                cmbclientcode.ValueMember = "id"      ' Value to store

                conn.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading client codes: " & ex.Message)
        End Try
    End Sub

    Private Sub btnInsert_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btninsert.Click
        ' --- VALIDATION BLOCK ---
        If cmbcontracttype.SelectedIndex = -1 Then
            MessageBox.Show("أختر نوع التعاقد")
            Exit Sub
        End If
        If cmbclientcode.SelectedIndex = -1 Then
            MessageBox.Show("أختر العميل")
            Exit Sub
        End If
        If Not dtpContract.Checked Then
            MessageBox.Show("يرجى اختيار تاريخ التعاقد")
            Exit Sub
        End If
        If String.IsNullOrWhiteSpace(txtcontractno.Text) Then
            MessageBox.Show("يرجى إدخال رقم التعاقد")
            Exit Sub
        End If

        Dim hasData As Boolean = False
        For Each row As DataGridViewRow In dgvContractDetails.Rows
            If Not row.IsNewRow Then
                hasData = True
                Exit For
            End If
        Next
        If Not hasData Then
            MessageBox.Show("يرجى إضافة بيانات على الأقل في الجدول قبل التسجيل.")
            Exit Sub
        End If

        For Each row As DataGridViewRow In dgvContractDetails.Rows
            If row.IsNewRow Then Continue For

            Dim rowIndexStr As String = (row.Index + 1).ToString()

            ' Mandatory fields validation
            If row.Cells("FormationType").Value Is Nothing OrElse String.IsNullOrWhiteSpace(row.Cells("FormationType").Value.ToString()) Then
                MessageBox.Show("يرجى اختيار نوع التشكيل في الصف رقم " & rowIndexStr)
                Exit Sub
            End If
            If row.Cells("Material").Value Is Nothing OrElse String.IsNullOrWhiteSpace(row.Cells("Material").Value.ToString()) Then
                MessageBox.Show("يرجى إدخال الخامة في الصف رقم " & rowIndexStr)
                Exit Sub
            End If
            If row.Cells("Batch").Value Is Nothing OrElse String.IsNullOrWhiteSpace(row.Cells("Batch").Value.ToString()) Then
                MessageBox.Show("يرجى اختيار الرسالة في الصف رقم " & rowIndexStr)
                Exit Sub
            End If
            If row.Cells("Lot").Value Is Nothing OrElse String.IsNullOrWhiteSpace(row.Cells("Lot").Value.ToString()) Then
                MessageBox.Show("يرجى اختيار اللوت في الصف رقم " & rowIndexStr)
                Exit Sub
            End If
            If (row.Cells("QuantityM").Value Is Nothing OrElse String.IsNullOrWhiteSpace(row.Cells("QuantityM").Value.ToString())) AndAlso _
               (row.Cells("QuantityK").Value Is Nothing OrElse String.IsNullOrWhiteSpace(row.Cells("QuantityK").Value.ToString())) Then
                MessageBox.Show("يرجى إدخال الكمية بالمتر أو بالكيلو في الصف رقم " & rowIndexStr)
                Exit Sub
            End If
            Dim wasteCell = row.Cells("WastePercentage").Value
            If wasteCell Is Nothing OrElse String.IsNullOrWhiteSpace(wasteCell.ToString()) OrElse Convert.ToDecimal(wasteCell) = 0 Then
                MessageBox.Show("نسبه الهالك اجباريه ويجب أن تكون أكبر من صفر في الصف رقم " & rowIndexStr)
                Exit Sub
            End If

            ' Existing Color Rule
            Dim notesCell = row.Cells("Notes").Value
            Dim colorCell = row.Cells("color").Value
            Dim notesText As String = If(notesCell Is Nothing, "", notesCell.ToString().ToLower())
            Dim colorMentionedInNotes As Boolean = False
            Dim englishColorNames As New List(Of String) From {"red", "blue", "green", "yellow", "black", "white", "orange", "purple", "pink", "brown", "gray", "grey"}
            Dim arabicColorNames As New List(Of String) From {"احمر", "ازرق", "اخضر", "اصفر", "اسود", "ابيض", "برتقالي", "بنفسجي", "وردي", "بني", "رمادي", "لون"}
            For Each colorName In arabicColorNames
                If notesText.Contains(colorName) Then colorMentionedInNotes = True
            Next
            If Not colorMentionedInNotes Then
                For Each colorRow As DataRow In GetColorCodes().Rows
                    If notesText.Contains(colorRow("name").ToString().ToLower()) Then colorMentionedInNotes = True
                Next
            End If
            If Not colorMentionedInNotes Then
                For Each colorName In englishColorNames
                    If notesText.Contains(colorName.ToLower()) Then colorMentionedInNotes = True
                Next
            End If
            If colorMentionedInNotes AndAlso (colorCell Is Nothing OrElse String.IsNullOrWhiteSpace(colorCell.ToString())) Then
                MessageBox.Show("يجب اختيار اللون من عمود اللون إذا تم ذكر لون في الملاحظات في السطر رقم " & rowIndexStr)
                Exit Sub
            End If

            ' Existing Fabric Code Rule
            Dim clientCodeText As String = cmbclientcode.Text.Trim()
            If clientCodeText.StartsWith("P", StringComparison.OrdinalIgnoreCase) Then
                If row.Cells("fabriccode").Value Is Nothing OrElse String.IsNullOrWhiteSpace(row.Cells("fabriccode").Value.ToString()) Then
                    MessageBox.Show("لابد من اختيار كود الخامه الخاصه بالخام للعميل P في الصف رقم " & rowIndexStr)
                    Exit Sub
                End If
            End If
        Next
        ' --- END VALIDATION BLOCK ---

        Try
            ' Create a new connection object using the connection string
            Using conn As New SqlConnection(connectionString)
                conn.Open()

                ' Determine the prefix based on contract type
                Dim contractTypeId As Integer = Convert.ToInt32(cmbcontracttype.SelectedValue)
                Dim prefix As String = ""
                If contractTypeId = 1 Then
                    prefix = "FW-"
                ElseIf contractTypeId = 2 Then
                    prefix = "FK-"
                Else
                    MessageBox.Show("نوع التعاقد غير صالح لتوليد كود الصنف.")
                    Exit Sub
                End If

                ' Get the last sequential number for the prefix
                Dim lastSequentialNumber As Integer = 0
                Dim getLastItemQuery As String = "SELECT TOP 1 item FROM Contracts WHERE item LIKE @Prefix + '%' ORDER BY item DESC"
                Using getLastItemCmd As New SqlCommand(getLastItemQuery, conn)
                    getLastItemCmd.Parameters.AddWithValue("@Prefix", prefix)
                    Dim lastItem As Object = getLastItemCmd.ExecuteScalar()
                    If lastItem IsNot Nothing Then
                        Dim lastItemString As String = lastItem.ToString()
                        ' Extract the numerical part after the prefix
                        If lastItemString.StartsWith(prefix) Then
                            Dim numPart As String = lastItemString.Substring(prefix.Length)
                            If Integer.TryParse(numPart, lastSequentialNumber) Then
                                ' Successfully parsed, lastSequentialNumber is set
                            Else
                                ' Handle cases where the number part is not a valid integer
                                lastSequentialNumber = 0 ' Or handle error appropriately
                            End If
                        End If
                    End If
                End Using

                ' SQL query to insert data into Contracts table
                Dim query As String = "INSERT INTO Contracts (ContractNo, ContractType, delivery_date, ClientCode, Material, Batch, Lot, QuantityM, QuantityK, WidthReq, WeightM, RollM, fabriccode, refno, color, Notes, kind_fab, count_rep, DateInserted, InsertedBy, waste, item) " &
                                  "VALUES (@ContractNo, @ContractType, @delivery_date, @ClientCode, @Material, @Batch, @Lot, @QuantityM, @QuantityK, @WidthReq, @WeightM, @RollM, @fabriccode, @refno, @color, @Notes, @kind_fab, @count_rep, GETDATE(), @InsertedBy, @waste, @item);"

                ' Initialize the email body with HTML table structure
                Dim emailBody As New StringBuilder()
                emailBody.AppendLine($"<h2>تم تسجيل تعاقد رقم {txtcontractno.Text} بنجاح.</h2>")
                emailBody.AppendLine("<h3>تفاصيل تعاقد:</h3>")
                emailBody.AppendLine("<table border='1' style='border-collapse: collapse; width: 100%;'>")
                emailBody.AppendLine("<tr>")
                emailBody.AppendLine("<th>كود الصنف</th>") ' Add item code header
                emailBody.AppendLine("<th>الخامة</th>")
                emailBody.AppendLine("<th>رقم الرسالة</th>")
                emailBody.AppendLine("<th>اللوت</th>")
                emailBody.AppendLine("<th>M الكمية</th>")
                emailBody.AppendLine("<th>KG الكمية</th>")
                emailBody.AppendLine("<th>رقم اذن العميل</th>")
                emailBody.AppendLine("<th>اللون</th>")
                emailBody.AppendLine("<th>ملاحظات</th>")
                emailBody.AppendLine("<th>أمتار التوب المطلوبة</th>")
                emailBody.AppendLine("<th>وزن المتر المربع المطلوب</th>")
                emailBody.AppendLine("<th>العرض المطلوب</th>")
                emailBody.AppendLine("<th>كود الخامه</th>")
                emailBody.AppendLine("<th>نسبه الهالك</th>")
                emailBody.AppendLine("</tr>")

                ' Iterate through each row in the DataGridView and insert into the Contracts table
                For Each row As DataGridViewRow In dgvContractDetails.Rows
                    If Not row.IsNewRow Then
                        ' Create a new SQL command for each row
                        Using cmd As New SqlCommand(query, conn)
                            ' Add parameters that will be reused for each row
                            cmd.Parameters.AddWithValue("@ContractNo", txtcontractno.Text)
                            cmd.Parameters.AddWithValue("@ContractType", cmbcontracttype.SelectedValue)
                            cmd.Parameters.AddWithValue("@delivery_date", dtpContract.Value)
                            cmd.Parameters.AddWithValue("@ClientCode", cmbclientcode.SelectedValue) ' Use SelectedValue here
                            cmd.Parameters.AddWithValue("@InsertedBy", LoggedInUsername)

                            ' Check for null or empty values in the DataGridView cells and handle them
                            cmd.Parameters.AddWithValue("@Material", If(row.Cells("Material").Value Is Nothing OrElse String.IsNullOrWhiteSpace(row.Cells("Material").Value.ToString()), DBNull.Value, row.Cells("Material").Value))
                            cmd.Parameters.AddWithValue("@Batch", If(row.Cells("Batch").Value Is Nothing OrElse String.IsNullOrWhiteSpace(row.Cells("Batch").Value.ToString()), DBNull.Value, row.Cells("Batch").Value))
                            cmd.Parameters.AddWithValue("@Lot", If(row.Cells("Lot").Value Is Nothing OrElse String.IsNullOrWhiteSpace(row.Cells("Lot").Value.ToString()), DBNull.Value, row.Cells("Lot").Value))
                            cmd.Parameters.AddWithValue("@QuantityM", If(row.Cells("QuantityM").Value Is Nothing OrElse String.IsNullOrWhiteSpace(row.Cells("QuantityM").Value.ToString()), DBNull.Value, Convert.ToDecimal(row.Cells("QuantityM").Value)))
                            cmd.Parameters.AddWithValue("@QuantityK", If(row.Cells("QuantityK").Value Is Nothing OrElse String.IsNullOrWhiteSpace(row.Cells("QuantityK").Value.ToString()), DBNull.Value, Convert.ToDecimal(row.Cells("QuantityK").Value)))
                            cmd.Parameters.AddWithValue("@refno", If(row.Cells("RefNo").Value Is Nothing OrElse String.IsNullOrWhiteSpace(row.Cells("RefNo").Value.ToString()), DBNull.Value, row.Cells("RefNo").Value))
                            Dim colorCode As String = ""
                            Dim colorName As String = ""
                            If row.Cells("color").Value IsNot Nothing Then
                                colorCode = row.Cells("color").Value.ToString()
                                ' ابحث عن الاسم بناءً على الكود
                                Dim colorTable As DataTable = GetColorCodes()
                                Dim foundRows = colorTable.Select("code = '" & colorCode & "'")
                                If foundRows.Length > 0 Then
                                    colorName = foundRows(0)("name").ToString()
                                End If
                            End If
                            Dim colorValueToSave = colorCode
                            If colorName <> "" Then
                                colorValueToSave &= " " & colorName
                            End If
                            cmd.Parameters.AddWithValue("@color", If(colorValueToSave = "", DBNull.Value, colorValueToSave))
                            ' نوع التشكيل
                            cmd.Parameters.AddWithValue("@kind_fab", If(row.Cells("FormationType").Value Is Nothing OrElse String.IsNullOrWhiteSpace(row.Cells("FormationType").Value.ToString()), DBNull.Value, row.Cells("FormationType").Value))
                            ' عدد ريب
                            If row.Cells("FormationType").Value IsNot Nothing AndAlso row.Cells("FormationType").Value.ToString() = "جسم" Then
                                cmd.Parameters.AddWithValue("@count_rep", If(row.Cells("RibCount").Value Is Nothing OrElse String.IsNullOrWhiteSpace(row.Cells("RibCount").Value.ToString()), DBNull.Value, Convert.ToInt32(row.Cells("RibCount").Value)))
                            Else
                                cmd.Parameters.AddWithValue("@count_rep", DBNull.Value)
                            End If
                            ' Add Notes from DataGridView instead of txtnotes
                            cmd.Parameters.AddWithValue("@Notes", If(row.Cells("Notes").Value Is Nothing OrElse String.IsNullOrWhiteSpace(row.Cells("Notes").Value.ToString()), DBNull.Value, row.Cells("Notes").Value))
                            cmd.Parameters.AddWithValue("@WidthReq", If(row.Cells("WidthReq").Value Is Nothing OrElse String.IsNullOrWhiteSpace(row.Cells("WidthReq").Value.ToString()), DBNull.Value, row.Cells("WidthReq").Value))
                            cmd.Parameters.AddWithValue("@WeightM", If(row.Cells("WeightM").Value Is Nothing OrElse String.IsNullOrWhiteSpace(row.Cells("WeightM").Value.ToString()), DBNull.Value, row.Cells("WeightM").Value))
                            cmd.Parameters.AddWithValue("@RollM", If(row.Cells("RollM").Value Is Nothing OrElse String.IsNullOrWhiteSpace(row.Cells("RollM").Value.ToString()), DBNull.Value, row.Cells("RollM").Value))
                            cmd.Parameters.AddWithValue("@fabriccode", If(row.Cells("fabriccode").Value Is Nothing OrElse String.IsNullOrWhiteSpace(row.Cells("fabriccode").Value.ToString()), DBNull.Value, row.Cells("fabriccode").Value))
                            ' Add the waste percentage parameter
                            cmd.Parameters.AddWithValue("@waste", Convert.ToDecimal(row.Cells("WastePercentage").Value))
                            ' Generate and add the item code parameter
                            lastSequentialNumber += 1
                            Dim newItemCode As String = prefix & lastSequentialNumber.ToString("D6")
                            cmd.Parameters.AddWithValue("@item", newItemCode)
                            ' Execute the insert query for each row
                            cmd.ExecuteNonQuery()

                            ' Append row details to the email body
                            emailBody.AppendLine("<tr>")
                            emailBody.AppendLine($"<td>{newItemCode}</td>") ' Add item code to email body
                            emailBody.AppendLine($"<td>{row.Cells("Material").Value}</td>")
                            emailBody.AppendLine($"<td>{row.Cells("Batch").Value}</td>")
                            emailBody.AppendLine($"<td>{row.Cells("Lot").Value}</td>")
                            emailBody.AppendLine($"<td>{row.Cells("QuantityM").Value}</td>")
                            emailBody.AppendLine($"<td>{row.Cells("QuantityK").Value}</td>")
                            emailBody.AppendLine($"<td>{row.Cells("RefNo").Value}</td>")
                            emailBody.AppendLine($"<td>{row.Cells("color").Value}</td>")
                            emailBody.AppendLine($"<td>{row.Cells("Notes").Value}</td>")
                            emailBody.AppendLine($"<td>{row.Cells("rollm").Value}</td>")
                            emailBody.AppendLine($"<td>{row.Cells("weightm").Value}</td>")
                            emailBody.AppendLine($"<td>{row.Cells("widthreq").Value}</td>")
                            emailBody.AppendLine($"<td>{row.Cells("fabriccode").Value}</td>")
                            emailBody.AppendLine($"<td>{row.Cells("WastePercentage").Value}</td>")
                            emailBody.AppendLine("</tr>")
                        End Using
                    End If
                Next

                ' Close the HTML table
                emailBody.AppendLine("</table>")

                ' Inform the user
                MessageBox.Show("تم تسجيل البيانات بنجاح")

                ' Send email notification
                Dim subject As String = "Contract Inserted Successfully"
                Dim body As String = emailBody.ToString()
                SendEmailNotification(subject, body)

                ' Clear the form after successful insertion
                ClearForm()
                ' Load the next contract number
                LoadLastContractNumber()
            End Using
        Catch ex As Exception
            ' Handle exceptions (e.g., database connection issues)
            MessageBox.Show("An error occurred: " & ex.Message)
        End Try
    End Sub

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






    Private Sub ClearForm()
        txtcontractno.Clear()
        cmbclientcode.SelectedIndex = -1
        cmbcontracttype.SelectedIndex = -1
        dtpContract.Checked = False ' Reset the DatePicker to unchecked (no date)
        dgvContractDetails.Rows.Clear() ' Clear the DataGridView
        dgvqc.DataSource = Nothing ' Clear the dgvqc DataGridView
    End Sub


    ' Declare PrintPreviewDialog object
    Private WithEvents printPreview As New PrintPreviewDialog()
    Private WithEvents printDoc As New PrintDocument()
    Private Sub btnPrint_Click(ByVal sender As Object, ByVal e As EventArgs)
        ' Show print preview dialog to the user
        printPreview.Document = printDoc
        printPreview.ShowDialog()
    End Sub
    Private Sub LoadLastContractNumber()
        Try
            ' Check if a contract type is selected
            If cmbcontracttype.SelectedIndex = -1 Then
                lbllastcontractno.Text = "Select a valid contract type."
                Return
            End If

            ' Get the selected ID
            Dim contractTypeId As Object = cmbcontracttype.SelectedValue
            If contractTypeId Is Nothing OrElse Not IsNumeric(contractTypeId) Then
                lbllastcontractno.Text = "Invalid contract type selected."
                Return
            End If

            Dim startingLetter As String = ""

            ' Determine the starting letter based on the ID
            If contractTypeId = 1 Then
                startingLetter = "W" ' For نسيج
            ElseIf contractTypeId = 2 Then
                startingLetter = "K" ' For تريكو
            Else
                lbllastcontractno.Text = "Invalid contract type selected."
                Return
            End If

            ' SQL Query
            Dim query As String = "SELECT TOP 1 ContractNo FROM Contracts WHERE ContractType = @ContractType AND ContractNo LIKE @StartingLetter + '%' ORDER BY TRY_CAST(SUBSTRING(ContractNo, LEN(@StartingLetter) + 1, LEN(ContractNo)) AS INT) DESC"

            ' Execute the query
            Using conn As New SqlConnection(connectionString)
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@ContractType", contractTypeId) ' Use ID for the query
                    cmd.Parameters.AddWithValue("@StartingLetter", startingLetter)

                    conn.Open()
                    Dim lastContractNo As Object = cmd.ExecuteScalar()

                    If lastContractNo IsNot DBNull.Value Then
                        lbllastcontractno.Text = lastContractNo.ToString()
                    Else
                        lbllastcontractno.Text = "No contracts found."
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message)
        End Try
    End Sub


    ' Event handler for View button click
    Private Sub btnView_Click(ByVal sender As Object, ByVal e As EventArgs)
        ' Navigate to the ContractViewForm
        Dim viewForm As New ContractViewForm()
        viewForm.Show()
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

    ' Event handler for when the contract type is changed
    Private Sub cmbcontracttype_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbcontracttype.SelectedIndexChanged
        If cmbcontracttype.SelectedIndex <> -1 Then
            SetFormControlsEnabled(True)
        Else
            SetFormControlsEnabled(False)
        End If

        LoadLastContractNumber()
        SetNextContractNumber()
    End Sub

    Private Sub SetNextContractNumber()
        Try
            ' If no contract type is selected, clear the textbox and exit.
            If cmbcontracttype.SelectedIndex = -1 Then
                txtcontractno.Clear()
                Return
            End If
            ' Get the value from lbllastcontractno
            Dim lastContractNoText As String = lbllastcontractno.Text
            Dim lastNumber As Integer = 0
            ' Use regex to find all digits in the string
            Dim match As Match = Regex.Match(lastContractNoText, "\d+")
            If match.Success Then
                lastNumber = Integer.Parse(match.Value)
            End If
            ' Calculate the next contract number
            Dim nextContractNo As Integer = lastNumber + 1
            ' Determine the prefix based on the selected contract type
            Dim prefix As String = ""
            Dim contractTypeId As Object = cmbcontracttype.SelectedValue
            If contractTypeId IsNot Nothing AndAlso IsNumeric(contractTypeId) Then
                If CInt(contractTypeId) = 1 Then
                    prefix = "W"
                ElseIf CInt(contractTypeId) = 2 Then
                    prefix = "K"
                End If
            End If
            ' Set the next contract number in txtcontractno, only if we have a prefix
            If Not String.IsNullOrEmpty(prefix) Then
                txtcontractno.Text = prefix & nextContractNo.ToString()
            Else
                ' This handles cases where contract type is invalid, or something else went wrong.
                txtcontractno.Clear()
            End If
        Catch ex As Exception
            ' Clear the textbox on any error
            txtcontractno.Clear()
        End Try
    End Sub
    ' Method to load fabric types into the ComboBox (cmbcontracttype)
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
                cmbcontracttype.DataSource = dt
                cmbcontracttype.DisplayMember = "fabrictype_ar"  ' Column to display
                cmbcontracttype.ValueMember = "id"               ' Value to store

                conn.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading fabric types: " & ex.Message)
        End Try
    End Sub

    Private Function GetColorCodes() As DataTable
        Dim dt As New DataTable()
        Try
            Using conn As New SqlConnection(connectionString)
                Dim query As String = "SELECT code, name FROM color_code"
                Dim cmd As New SqlCommand(query, conn)
                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                dt.Load(reader)
                conn.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading color codes: " & ex.Message)
        End Try
        Return dt
    End Function

    Private Function GetFabricCodes() As DataTable
        Dim dt As New DataTable()
        Try
            Using conn As New SqlConnection(connectionString)
                Dim query As String = "SELECT code FROM style"
                Dim cmd As New SqlCommand(query, conn)
                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                dt.Load(reader)
                conn.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading fabric codes: " & ex.Message)
        End Try
        Return dt
    End Function

    Private Sub dgvContractDetails_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles dgvContractDetails.EditingControlShowing
        If dgvContractDetails.CurrentCell.ColumnIndex = dgvContractDetails.Columns("color").Index OrElse
           dgvContractDetails.CurrentCell.ColumnIndex = dgvContractDetails.Columns("fabriccode").Index Then

            Dim cmb As ComboBox = TryCast(e.Control, ComboBox)
            If cmb IsNot Nothing Then
                cmb.DropDownStyle = ComboBoxStyle.DropDown
                cmb.AutoCompleteMode = AutoCompleteMode.SuggestAppend
                cmb.AutoCompleteSource = AutoCompleteSource.ListItems
            End If
        End If
    End Sub

    ' Event handler for when a cell finishes being edited
    Private Sub dgvContractDetails_CellEndEdit(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles dgvContractDetails.CellEndEdit
        ' This handler is kept for the interactive logic of color selection
        ' Check if the edited column is the Notes column
        If dgvContractDetails.Columns(e.ColumnIndex).Name = "Notes" Then
            Dim notesCell As DataGridViewCell = dgvContractDetails.Rows(e.RowIndex).Cells("Notes")
            Dim colorCell As DataGridViewCell = dgvContractDetails.Rows(e.RowIndex).Cells("color")
            Dim notesText As String = If(notesCell.Value Is Nothing, "", notesCell.Value.ToString().ToLower())

            Dim colorMentionedInNotes As Boolean = False

            ' Add common English color names
            Dim englishColorNames As New List(Of String) From {"red", "blue", "green", "yellow", "black", "white", "orange", "purple", "pink", "brown", "gray", "grey"}

            ' Add common Arabic color names
            Dim arabicColorNames As New List(Of String) From {"احمر", "ازرق", "اخضر", "اصفر", "اسود", "ابيض", "برتقالي", "بنفسجي", "وردي", "بني", "رمادي", "لون"}

            ' Check for the Arabic word "لون" or any Arabic color name
            For Each arabicColor As String In arabicColorNames
                If notesText.Contains(arabicColor) Then colorMentionedInNotes = True
            Next

            ' Check for Arabic color names from the database
            If Not colorMentionedInNotes Then
                For Each colorRow As DataRow In GetColorCodes().Rows
                    If notesText.Contains(colorRow("name").ToString().ToLower()) Then colorMentionedInNotes = True
                Next
            End If

            ' Check for common English color names
            If Not colorMentionedInNotes Then
                For Each englishColorName As String In englishColorNames
                    If notesText.Contains(englishColorName.ToLower()) Then colorMentionedInNotes = True
                Next
            End If

            ' If a color is mentioned in notes
            If colorMentionedInNotes Then
                ' Make all cells in the row read-only except the color column
                For Each cell As DataGridViewCell In dgvContractDetails.Rows(e.RowIndex).Cells
                    If cell.OwningColumn.Name <> "color" Then
                        cell.ReadOnly = True
                    End If
                Next

                ' If no color is selected, show message and focus on color column
                If colorCell.Value Is Nothing OrElse String.IsNullOrWhiteSpace(colorCell.Value.ToString()) Then
                    MessageBox.Show("يجب اختيار اللون من عمود اللون أولاً قبل إكمال باقي البيانات.")
                    ' Use BeginInvoke to change the current cell after the current event is processed
                    Me.BeginInvoke(Sub()
                                       dgvContractDetails.CurrentCell = colorCell
                                       dgvContractDetails.BeginEdit(True)
                                   End Sub)
                End If
            End If
        End If

        ' If the edited column is the color column
        If dgvContractDetails.Columns(e.ColumnIndex).Name = "color" Then
            Dim colorCell As DataGridViewCell = dgvContractDetails.Rows(e.RowIndex).Cells("color")
            ' If a color is selected, make all cells in the row editable again
            If colorCell.Value IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(colorCell.Value.ToString()) Then
                For Each cell As DataGridViewCell In dgvContractDetails.Rows(e.RowIndex).Cells
                    cell.ReadOnly = False
                Next
            End If
        End If
    End Sub

End Class

