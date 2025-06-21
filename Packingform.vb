Imports System.Data.SqlClient
Imports Excel = Microsoft.Office.Interop.Excel
Imports OfficeOpenXml
Imports System.IO


Public Class packingform

    ' Corrected SQL Server connection string
    Private sqlServerConnectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"
    Private headerCheckbox As CheckBox = New CheckBox()
    Private WithEvents Panel1 As Panel
    Private WithEvents Panel2 As Panel

    Private Sub packingform_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Make form responsive
        Me.WindowState = FormWindowState.Maximized
        Me.FormBorderStyle = FormBorderStyle.Sizable
        Me.MinimumSize = New Size(1024, 768)

        ' Make all controls responsive
        For Each control As Control In Me.Controls
            MakeControlResponsive(control)
        Next

        LoadClientCodes()
        CalculateTotalHeight()
        CalculateTotalweight()
        LoadClientCodesto()
        cmpclientto.SelectedIndex = -1
    End Sub

    Private Sub MakeControlResponsive(control As Control)
        ' Handle different types of controls
        Select Case True
            Case TypeOf control Is DataGridView
                Dim dgv As DataGridView = DirectCast(control, DataGridView)
                dgv.Dock = DockStyle.Fill
                dgv.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right

            Case TypeOf control Is Panel
                Dim panel As Panel = DirectCast(control, Panel)
                panel.Dock = DockStyle.Fill
                panel.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right

                ' Make panel's controls responsive
                For Each subControl As Control In panel.Controls
                    MakeControlResponsive(subControl)
                Next

            Case TypeOf control Is TableLayoutPanel
                Dim tableLayout As TableLayoutPanel = DirectCast(control, TableLayoutPanel)
                tableLayout.Dock = DockStyle.Fill
                tableLayout.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right

            Case TypeOf control Is FlowLayoutPanel
                Dim flowLayout As FlowLayoutPanel = DirectCast(control, FlowLayoutPanel)
                flowLayout.Dock = DockStyle.Fill
                flowLayout.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right

            Case TypeOf control Is GroupBox
                Dim groupBox As GroupBox = DirectCast(control, GroupBox)
                groupBox.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right

                ' Make groupbox's controls responsive
                For Each subControl As Control In groupBox.Controls
                    MakeControlResponsive(subControl)
                Next

            Case TypeOf control Is TextBox, TypeOf control Is ComboBox, TypeOf control Is DateTimePicker
                control.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right

            Case TypeOf control Is Button
                control.Anchor = AnchorStyles.Top Or AnchorStyles.Left

            Case TypeOf control Is Label
                control.Anchor = AnchorStyles.Top Or AnchorStyles.Left

            Case TypeOf control Is CheckBox
                control.Anchor = AnchorStyles.Top Or AnchorStyles.Left

            Case TypeOf control Is RadioButton
                control.Anchor = AnchorStyles.Top Or AnchorStyles.Left
        End Select
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
        conditions.Add("NOT (pf.height = 0 AND pf.weight = 0)")

        ' Combine all conditions into the WHERE clause
        Dim whereClause As String = If(conditions.Count > 0, "WHERE " & String.Join(" AND ", conditions), "")

        ' SQL query with the dynamic WHERE clause
        Dim query As String = "SELECT id, worder_id AS 'أمر شغل',client_code AS 'كود العميل',height AS 'الطول', weight AS 'الوزن', roll AS 'رقم التوب', fabric_grade AS 'درجه القماش', " &
                       "contract_no AS 'رقم التعاقد', batch_no AS 'رقم الرسالة', ref_no AS 'رقم الإذن', " &
                       "transaction_date AS 'تاريخ المخزن', width AS 'العرض', color AS 'اللون', product_name AS 'الخامة' " &
                       "FROM store_finish pf " &
                       whereClause & " " &
                       "GROUP BY id, worder_id, contract_no, batch_no, ref_no, roll, client_code, inspection_date, transaction_date, " &
                       "width, height, weight, fabric_grade, color, product_name, username"

        ' Pass parameters to LoadData
        LoadData(query, worderIds, contractNos, batchNos, clientCode, fromDate, toDate)
        CalculateFilteredTotalHeight()
        ApplyDataGridViewStyles()
        CalculateTotalweight()
        CalculateTotalHeight()
        CalculateFilteredTotalweight()
        CalculateFilteredRollCount()
    End Sub
    Private Sub dgvresults_CellFormatting(ByVal sender As Object, ByVal e As DataGridViewCellFormattingEventArgs) Handles dgvresults.CellFormatting
        If e.ColumnIndex = dgvresults.Columns("أمر شغل").Index Then
            ' Check if the current and previous cells have the same value
            If e.RowIndex > 0 AndAlso dgvresults.Rows(e.RowIndex - 1).Cells(e.ColumnIndex).Value IsNot Nothing AndAlso
               dgvresults.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = dgvresults.Rows(e.RowIndex - 1).Cells(e.ColumnIndex).Value Then
                e.Value = String.Empty ' Hide duplicate value
                e.FormattingApplied = True
            End If
        End If
        If e.ColumnIndex = dgvresults.Columns("رقم الرسالة").Index Then
            ' Check if the current and previous cells have the same value
            If e.RowIndex > 0 AndAlso dgvresults.Rows(e.RowIndex - 1).Cells(e.ColumnIndex).Value IsNot Nothing AndAlso
               dgvresults.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = dgvresults.Rows(e.RowIndex - 1).Cells(e.ColumnIndex).Value Then
                e.Value = String.Empty ' Hide duplicate value
                e.FormattingApplied = True
            End If
        End If
        If e.ColumnIndex = dgvresults.Columns("رقم الإذن").Index Then
            ' Check if the current and previous cells have the same value
            If e.RowIndex > 0 AndAlso dgvresults.Rows(e.RowIndex - 1).Cells(e.ColumnIndex).Value IsNot Nothing AndAlso
               dgvresults.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = dgvresults.Rows(e.RowIndex - 1).Cells(e.ColumnIndex).Value Then
                e.Value = String.Empty ' Hide duplicate value
                e.FormattingApplied = True
            End If
        End If
        If e.ColumnIndex = dgvresults.Columns("كود العميل").Index Then
            ' Check if the current and previous cells have the same value
            If e.RowIndex > 0 AndAlso dgvresults.Rows(e.RowIndex - 1).Cells(e.ColumnIndex).Value IsNot Nothing AndAlso
               dgvresults.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = dgvresults.Rows(e.RowIndex - 1).Cells(e.ColumnIndex).Value Then
                e.Value = String.Empty ' Hide duplicate value
                e.FormattingApplied = True
            End If
        End If
        If e.ColumnIndex = dgvresults.Columns("رقم التعاقد").Index Then
            ' Check if the current and previous cells have the same value
            If e.RowIndex > 0 AndAlso dgvresults.Rows(e.RowIndex - 1).Cells(e.ColumnIndex).Value IsNot Nothing AndAlso
               dgvresults.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = dgvresults.Rows(e.RowIndex - 1).Cells(e.ColumnIndex).Value Then
                e.Value = String.Empty ' Hide duplicate value
                e.FormattingApplied = True
            End If
        End If
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


    Private Sub dgvresults_CurrentCellDirtyStateChanged(ByVal sender As Object, ByVal e As EventArgs) Handles dgvresults.CurrentCellDirtyStateChanged
        ' Commit the checkbox value change immediately
        If dgvresults.IsCurrentCellDirty AndAlso dgvresults.CurrentCell.OwningColumn.Name = "select_checkbox" Then
            dgvresults.CommitEdit(DataGridViewDataErrorContexts.Commit)
        End If
    End Sub

    Private Sub CalculateSelectedRowTotals()
        ' Initialize variables to store the totals
        Dim totalHeight As Double = 0
        Dim totalWeight As Double = 0
        Dim rollCount As Integer = 0

        ' Loop through all rows in the DataGridView
        For Each row As DataGridViewRow In dgvresults.Rows
            Try
                ' Check if the checkbox is checked
                Dim isSelected As Boolean = False
                If Not IsDBNull(row.Cells("select_checkbox").Value) Then
                    isSelected = Convert.ToBoolean(row.Cells("select_checkbox").Value)
                End If

                If isSelected Then
                    ' Increment roll count
                    rollCount += 1

                    ' Safely parse height and weight values
                    Dim height As Double = 0
                    Dim weight As Double = 0

                    If Not IsDBNull(row.Cells("الطول").Value) AndAlso IsNumeric(row.Cells("الطول").Value) Then
                        height = Convert.ToDouble(row.Cells("الطول").Value)
                    End If

                    If Not IsDBNull(row.Cells("الوزن").Value) AndAlso IsNumeric(row.Cells("الوزن").Value) Then
                        weight = Convert.ToDouble(row.Cells("الوزن").Value)
                    End If

                    ' Add to totals
                    totalHeight += height
                    totalWeight += weight
                End If
            Catch ex As Exception
                ' Log or display an error message for debugging purposes
                MessageBox.Show("Error processing row: " & ex.Message)
            End Try
        Next

        ' Update the labels with the calculated totals
        lblmeter.Text = "متر " & totalHeight.ToString("F2") ' Format to 2 decimal places
        lblweight.Text = "وزن " & totalWeight.ToString("F2")
        lblrol.Text = "توب " & rollCount.ToString()
    End Sub
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
    Private Sub CalculateTotalHeight()
        Using connection As New SqlConnection(sqlServerConnectionString)
            Dim query As String = "SELECT SUM(CAST(height AS DECIMAL(18,2))) FROM store_finish"
            Dim cmd As New SqlCommand(query, connection)

            Try
                connection.Open()
                Dim heightSum As Object = cmd.ExecuteScalar()
                ' Check if the result is DBNull or Nothing and display the result accordingly
                If IsDBNull(heightSum) OrElse heightSum Is Nothing Then
                    lbltotalstock.Text = "إجمالى رصيد المخزن: 0"
                Else
                    lbltotalstock.Text = "  رصيد المخزن متر : " & Convert.ToDecimal(heightSum).ToString("N2") ' Format to two decimal places
                End If
            Catch ex As Exception
                MessageBox.Show("An error occurred while calculating total height: " & ex.Message)
            End Try
        End Using
    End Sub
    Private Sub CalculateTotalweight()
        Using connection As New SqlConnection(sqlServerConnectionString)
            Dim query As String = "SELECT SUM(CAST(weight AS DECIMAL(18,2))) FROM store_finish"
            Dim cmd As New SqlCommand(query, connection)

            Try
                connection.Open()
                Dim weightSum As Object = cmd.ExecuteScalar()
                ' Check if the result is DBNull or Nothing and display the result accordingly
                If IsDBNull(weightSum) OrElse weightSum Is Nothing Then
                    lbltotalstockw.Text = "إجمالى رصيد المخزن: 0"
                Else
                    lbltotalstockw.Text = "  رصيد المخزن وزن : " & Convert.ToDecimal(weightSum).ToString("N2") ' Format to two decimal places
                End If
            Catch ex As Exception
                MessageBox.Show("An error occurred while calculating total height: " & ex.Message)
            End Try
        End Using
    End Sub
    Private Sub CalculateFilteredTotalHeight()
        Using connection As New SqlConnection(sqlServerConnectionString)
            ' Base query to sum the height based on filters
            Dim query As String = "SELECT SUM(CAST(height AS DECIMAL(18,2))) FROM store_finish WHERE 1=1"
            Dim cmd As New SqlCommand(query, connection)

            ' Apply filters based on input fields
            If Not String.IsNullOrEmpty(txtworderid.Text) Then
                ' Split the comma-separated values
                Dim worderIds = txtworderid.Text.Split(","c).Select(Function(id) "%" & id.Trim() & "%").ToArray()
                query &= " AND ("
                ' Add each worderId filter condition using OR
                query &= String.Join(" OR ", worderIds.Select(Function(id, index) "worder_id LIKE @worderId" & index))
                query &= ")"
                ' Add parameters for each worderId
                For i As Integer = 0 To worderIds.Length - 1
                    cmd.Parameters.AddWithValue("@worderId" & i, worderIds(i))
                Next
            End If

            If Not String.IsNullOrEmpty(txtContractNo.Text) Then
                ' Split the comma-separated values
                Dim contractNos = txtContractNo.Text.Split(","c).Select(Function(c) c.Trim()).ToArray()
                query &= " AND contract_no IN (" & String.Join(",", contractNos.Select(Function(c, index) "@contractNo" & index)) & ")"
                ' Add parameters for each contractNo
                For i As Integer = 0 To contractNos.Length - 1
                    cmd.Parameters.AddWithValue("@contractNo" & i, contractNos(i))
                Next
            End If

            If cmbClient.SelectedIndex <> -1 Then
                query &= " AND client_code = @clientCode"
                cmd.Parameters.AddWithValue("@clientCode", cmbClient.SelectedItem.ToString())
            End If

            If Not String.IsNullOrEmpty(txtbatch.Text) Then
                ' Split the comma-separated values
                Dim batchNos = txtbatch.Text.Split(","c).Select(Function(b) b.Trim()).ToArray()
                query &= " AND batch_no IN (" & String.Join(",", batchNos.Select(Function(b, index) "@batchNo" & index)) & ")"
                ' Add parameters for each batchNo
                For i As Integer = 0 To batchNos.Length - 1
                    cmd.Parameters.AddWithValue("@batchNo" & i, batchNos(i))
                Next
            End If

            ' Set the command text after all filters are applied
            cmd.CommandText = query

            Try
                connection.Open()
                Dim heightSum As Object = cmd.ExecuteScalar()
                ' Display the calculated height sum in lbltotal
                lbltotal.Text = "رصيد العميل متر: " & If(IsDBNull(heightSum) OrElse heightSum Is Nothing, 0, Convert.ToDecimal(heightSum)).ToString("N2") & " "
            Catch ex As Exception
                MessageBox.Show("An error occurred while calculating filtered total height: " & ex.Message)
            End Try
        End Using
    End Sub
    Private Sub CalculateFilteredTotalweight()
        Using connection As New SqlConnection(sqlServerConnectionString)
            ' Base query to sum the weight based on filters
            Dim query As String = "SELECT SUM(CAST(weight AS DECIMAL(18,2))) FROM store_finish WHERE 1=1"
            Dim cmd As New SqlCommand(query, connection)

            ' Apply filters based on input fields
            If Not String.IsNullOrEmpty(txtworderid.Text) Then
                ' Split the comma-separated values
                Dim worderIds = txtworderid.Text.Split(","c).Select(Function(id) "%" & id.Trim() & "%").ToArray()
                query &= " AND ("
                ' Add each worderId filter condition using OR
                query &= String.Join(" OR ", worderIds.Select(Function(id, index) "worder_id LIKE @worderId" & index))
                query &= ")"
                ' Add parameters for each worderId
                For i As Integer = 0 To worderIds.Length - 1
                    cmd.Parameters.AddWithValue("@worderId" & i, worderIds(i))
                Next
            End If

            If Not String.IsNullOrEmpty(txtContractNo.Text) Then
                ' Split the comma-separated values
                Dim contractNos = txtContractNo.Text.Split(","c).Select(Function(c) c.Trim()).ToArray()
                query &= " AND contract_no IN (" & String.Join(",", contractNos.Select(Function(c, index) "@contractNo" & index)) & ")"
                ' Add parameters for each contractNo
                For i As Integer = 0 To contractNos.Length - 1
                    cmd.Parameters.AddWithValue("@contractNo" & i, contractNos(i))
                Next
            End If

            If cmbClient.SelectedIndex <> -1 Then
                query &= " AND client_code = @clientCode"
                cmd.Parameters.AddWithValue("@clientCode", cmbClient.SelectedItem.ToString())
            End If

            If Not String.IsNullOrEmpty(txtbatch.Text) Then
                ' Split the comma-separated values
                Dim batchNos = txtbatch.Text.Split(","c).Select(Function(b) b.Trim()).ToArray()
                query &= " AND batch_no IN (" & String.Join(",", batchNos.Select(Function(b, index) "@batchNo" & index)) & ")"
                ' Add parameters for each batchNo
                For i As Integer = 0 To batchNos.Length - 1
                    cmd.Parameters.AddWithValue("@batchNo" & i, batchNos(i))
                Next
            End If

            ' Set the command text after all filters are applied
            cmd.CommandText = query

            Try
                connection.Open()
                Dim weightSum As Object = cmd.ExecuteScalar()
                ' Display the calculated weight sum in lbltotalw
                lbltotalw.Text = "رصيد العميل وزن: " & If(IsDBNull(weightSum) OrElse weightSum Is Nothing, 0, Convert.ToDecimal(weightSum)).ToString("N2") & ""
            Catch ex As Exception
                MessageBox.Show("An error occurred while calculating filtered total Weight: " & ex.Message)
            End Try
        End Using
    End Sub
    Private Sub CalculateFilteredRollCount()
        Using connection As New SqlConnection(sqlServerConnectionString)
            ' Base query to count the rolls based on filters
            Dim query As String = "SELECT COUNT(*) FROM store_finish WHERE 1=1"
            Dim cmd As New SqlCommand(query, connection)

            ' Apply filters based on input fields
            If Not String.IsNullOrEmpty(txtworderid.Text) Then
                ' Split the comma-separated values
                Dim worderIds = txtworderid.Text.Split(","c).Select(Function(id) "%" & id.Trim() & "%").ToArray()
                query &= " AND ("
                ' Add each worderId filter condition using OR
                query &= String.Join(" OR ", worderIds.Select(Function(id, index) "worder_id LIKE @worderId" & index))
                query &= ")"
                ' Add parameters for each worderId
                For i As Integer = 0 To worderIds.Length - 1
                    cmd.Parameters.AddWithValue("@worderId" & i, worderIds(i))
                Next
            End If

            If Not String.IsNullOrEmpty(txtContractNo.Text) Then
                ' Split the comma-separated values
                Dim contractNos = txtContractNo.Text.Split(","c).Select(Function(c) c.Trim()).ToArray()
                query &= " AND contract_no IN (" & String.Join(",", contractNos.Select(Function(c, index) "@contractNo" & index)) & ")"
                ' Add parameters for each contractNo
                For i As Integer = 0 To contractNos.Length - 1
                    cmd.Parameters.AddWithValue("@contractNo" & i, contractNos(i))
                Next
            End If

            If cmbClient.SelectedIndex <> -1 Then
                query &= " AND client_code = @clientCode"
                cmd.Parameters.AddWithValue("@clientCode", cmbClient.SelectedItem.ToString())
            End If

            If Not String.IsNullOrEmpty(txtbatch.Text) Then
                ' Split the comma-separated values
                Dim batchNos = txtbatch.Text.Split(","c).Select(Function(b) b.Trim()).ToArray()
                query &= " AND batch_no IN (" & String.Join(",", batchNos.Select(Function(b, index) "@batchNo" & index)) & ")"
                ' Add parameters for each batchNo
                For i As Integer = 0 To batchNos.Length - 1
                    cmd.Parameters.AddWithValue("@batchNo" & i, batchNos(i))
                Next
            End If

            ' Set the command text after all filters are applied
            cmd.CommandText = query

            Try
                connection.Open()
                Dim rollCount As Object = cmd.ExecuteScalar()

                ' Display the calculated roll count in lblroll
                lblroll.Text = "عدد الأتواب: " & If(IsDBNull(rollCount) OrElse rollCount Is Nothing, 0, Convert.ToInt32(rollCount)).ToString()
            Catch ex As Exception
                MessageBox.Show("An error occurred while calculating filtered roll count: " & ex.Message)
            End Try
        End Using
    End Sub
    ' Event handler for View button click
    Private Sub btnView_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnview.Click
        ' Navigate to the ContractViewForm
        Dim viewForm As New packingviewform()
        viewForm.Show()
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

            ' Set font and color for the DataGridView header
            dgvresults.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 12, FontStyle.Bold)
            dgvresults.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue
            dgvresults.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black
            dgvresults.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            ' Apply header and content styles
            dgvresults.EnableHeadersVisualStyles = False ' Allow custom header styles to take effect

            ' Add the "Select All" checkbox functionality for the header click
            AddHandler selectAllCheckBox.CheckedChanged, AddressOf SelectAllCheckBox_CheckedChanged
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
        If dgvresults.Columns.Contains("width") Then
            dgvresults.Columns("width").Width = 20
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
    Private Sub SelectAllCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim isChecked As Boolean = selectAllCheckBox.Checked

        ' Loop through each row and set the checkbox value based on "Select All" checkbox state
        For Each row As DataGridViewRow In dgvresults.Rows
            row.Cells("select_checkbox").Value = isChecked
        Next
        dgvresults.RefreshEdit()
    End Sub
    Private Sub dgvresults_CellValueChanged(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles dgvresults.CellValueChanged
        ' Check if the changed cell is in the checkbox column
        If e.ColumnIndex = dgvresults.Columns("select_checkbox").Index Then
            ' Recalculate totals when a checkbox value changes
            CalculateSelectedRowTotals()
        End If
    End Sub
    Private Sub btnInsert_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnInsert.Click
        ' Validate that a client is selected in cmpclientto
        If cmpclientto.SelectedValue Is Nothing Then
            MessageBox.Show("Please select a client before proceeding.")
            Return
        End If

        Dim selectedClientId As Integer = Convert.ToInt32(cmpclientto.SelectedValue)
        Dim checkedRowsCount As Integer = 0 ' To keep track of how many rows were processed
        Dim isMessageDisplayed As Boolean = False ' Flag to track if the message has been shown
        Dim newRefPacking As Integer = 0 ' Variable to store the incremented ref_packing

        ' Retrieve the next ref_packing value only once before processing any rows
        Using connection As New SqlConnection(sqlServerConnectionString)
            Try
                connection.Open()
                Dim getMaxQuery As String = "SELECT ISNULL(MAX(ref_packing), 0) FROM packing"
                Using getMaxCmd As New SqlCommand(getMaxQuery, connection)
                    Dim result = getMaxCmd.ExecuteScalar()
                    If result IsNot Nothing AndAlso IsNumeric(result) Then
                        newRefPacking = Convert.ToInt32(result) + 1
                    End If
                End Using
            Catch ex As Exception
                MessageBox.Show("Error retrieving ref_packing: " & ex.Message)
                Return
            Finally
                connection.Close()
            End Try
        End Using

        ' Initial validation to check all selected rows
        For Each row As DataGridViewRow In dgvresults.Rows
            Dim checkBoxCell As DataGridViewCheckBoxCell = TryCast(row.Cells("select_checkbox"), DataGridViewCheckBoxCell)
            If checkBoxCell IsNot Nothing AndAlso CBool(checkBoxCell.Value) Then
                checkedRowsCount += 1

                Dim selectedId As Integer
                If Not Integer.TryParse(row.Cells("id").Value.ToString(), selectedId) Then
                    MessageBox.Show("Invalid ID in one of the selected rows.")
                    Return
                End If

                Dim insertedHeight As Decimal
                Dim insertedWeight As Decimal
                Try
                    insertedHeight = Convert.ToDecimal(row.Cells("الطول").Value)
                    insertedWeight = Convert.ToDecimal(row.Cells("الوزن").Value)
                Catch ex As Exception
                    MessageBox.Show("Invalid height or weight format: " & ex.Message)
                    Return
                End Try

                ' Validate against store_finish
                If Not ValidateAgainstStoreFinish(selectedId, insertedHeight, insertedWeight) Then
                    MessageBox.Show("كمية الصرف أكبر من كميه المخزون")
                    Return ' Stop further processing if any row exceeds available quantity
                End If
            End If
        Next

        If checkedRowsCount = 0 Then
            MessageBox.Show("No rows selected for insertion.")
            Return
        End If

        ' Proceed with insertion if all validations pass
        For Each row As DataGridViewRow In dgvresults.Rows
            Dim checkBoxCell As DataGridViewCheckBoxCell = TryCast(row.Cells("select_checkbox"), DataGridViewCheckBoxCell)
            If checkBoxCell IsNot Nothing AndAlso CBool(checkBoxCell.Value) Then
                Dim selectedId As Integer = Convert.ToInt32(row.Cells("id").Value)
                Dim insertedHeight As Decimal = Convert.ToDecimal(row.Cells("الطول").Value)
                Dim insertedWeight As Decimal = Convert.ToDecimal(row.Cells("الوزن").Value)

                ' Insert into packing
                Dim username As String = LoggedInUsername
                InsertIntoPacking(selectedId, insertedHeight, insertedWeight, username, newRefPacking, selectedClientId, isMessageDisplayed)

                ' Increment ref_packing
                newRefPacking += 1

                ' Update store_finish
                Using connection As New SqlConnection(sqlServerConnectionString)
                    connection.Open()
                    UpdateStoreFinished(connection, selectedId, insertedHeight, insertedWeight)
                End Using
            End If
        Next
    End Sub

    Private Function ValidateAgainstStoreFinish(ByVal id As Integer, ByVal insertedHeight As Decimal, ByVal insertedWeight As Decimal) As Boolean
        Dim isValid As Boolean = True
        Using connection As New SqlConnection(sqlServerConnectionString)
            Try
                connection.Open()
                Dim query As String = "SELECT height, weight FROM store_finish WHERE id = @id"
                Using cmd As New SqlCommand(query, connection)
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            Dim availableHeight As Decimal = reader.GetDecimal(0)
                            Dim availableWeight As Decimal = reader.GetDecimal(1)

                            If insertedHeight > availableHeight OrElse insertedWeight > availableWeight Then
                                isValid = False
                            End If
                        End If
                    End Using
                End Using
            Catch ex As Exception
                MessageBox.Show("Error validating store_finish: " & ex.Message)
                isValid = False
            Finally
                connection.Close()
            End Try
        End Using
        Return isValid
    End Function

    Private Sub InsertIntoPacking(ByVal id As Integer, ByVal height As Decimal, ByVal weight As Decimal, ByVal username As String, ByVal refPacking As Integer, ByVal clientId As Integer, ByRef isMessageDisplayed As Boolean)
        Using connection As New SqlConnection(sqlServerConnectionString)
            Try
                connection.Open()

                ' Insert into packing
                Dim query As String = "INSERT INTO packing (storefinishid, weight, height, date, ref_packing, toclient) VALUES (@id, @weight, @height, @date, @ref_packing, @toclient)"
                Using cmd As New SqlCommand(query, connection)
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id
                    cmd.Parameters.Add("@weight", SqlDbType.Decimal).Value = weight
                    cmd.Parameters.Add("@height", SqlDbType.Decimal).Value = height

                    cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = DateTime.Now
                    cmd.Parameters.Add("@ref_packing", SqlDbType.Int).Value = refPacking
                    cmd.Parameters.Add("@toclient", SqlDbType.Int).Value = clientId

                    cmd.ExecuteNonQuery()

                    If Not isMessageDisplayed Then
                        MessageBox.Show("Record inserted into packing successfully with ref_packing: " & refPacking)
                        dgvresults.DataSource = Nothing
                        CalculateFilteredTotalHeight()
                        CalculateTotalHeight()
                        CalculateTotalweight()
                        cmpclientto.SelectedIndex = -1
                        cmbClient.SelectedIndex = -1
                        CalculateSelectedRowTotals()
                        isMessageDisplayed = True
                    End If
                End Using

            Catch ex As SqlException
                MessageBox.Show("An error occurred while inserting into packing (SQL error): " & ex.Message)
            Catch ex As Exception
                MessageBox.Show("An unexpected error occurred while inserting into packing: " & ex.Message)
            Finally
                connection.Close()
            End Try
        End Using
    End Sub
    Private Sub UpdateStoreFinished(ByVal connection As SqlConnection, ByVal id As Integer, ByVal insertedHeight As Decimal, ByVal insertedWeight As Decimal)
        ' Determine if we need to add or subtract from store_finish
        Dim query As String = "UPDATE store_finish SET height = height - @insertedHeight, weight = weight - @insertedWeight WHERE id = @id"

       
        Using cmd As New SqlCommand(query, connection)
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id
            cmd.Parameters.Add("@insertedHeight", SqlDbType.Decimal).Value = insertedHeight
            cmd.Parameters.Add("@insertedWeight", SqlDbType.Decimal).Value = insertedWeight

            Try
                Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

                If rowsAffected > 0 AndAlso Not isStoreFinishedMessageDisplayed Then
                    MessageBox.Show("Store finished table updated successfully!")
                    isStoreFinishedMessageDisplayed = True ' Set the flag to prevent further messages
                ElseIf rowsAffected = 0 Then
                    MessageBox.Show("No matching records found to update.")
                End If
            Catch ex As SqlException
                MessageBox.Show("An error occurred while updating store_finish (SQL error): " & ex.Message)
            Catch ex As Exception
                MessageBox.Show("An unexpected error occurred while updating store_finish: " & ex.Message)
            End Try
        End Using
    End Sub
    Private Function GetSelectedId() As Integer
        ' Add logic to retrieve the selected ID from the DataGridView
        ' This function assumes that the first column (id) is the one selected in the DataGridView
        If dgvresults.SelectedRows.Count > 0 Then
            Return Convert.ToInt32(dgvresults.SelectedRows(0).Cells("id").Value)
        Else
            MessageBox.Show("Please select a row to insert.")
            Return -1
        End If
    End Function

    Private Sub LoadClientCodesto()
        Using connection As New SqlConnection(sqlServerConnectionString)
            Dim query As String = "SELECT id, code FROM Clients"
            Dim cmd As New SqlCommand(query, connection)

            Try
                connection.Open()
                Dim dt As New DataTable()
                Dim adapter As New SqlDataAdapter(cmd)
                adapter.Fill(dt)

                ' Bind the ComboBox
                cmpclientto.DataSource = dt
                cmpclientto.DisplayMember = "code" ' Display the code in the dropdown
                cmpclientto.ValueMember = "id"    ' Store the id as the value
            Catch ex As Exception
                MessageBox.Show("An error occurred while loading client codes: " & ex.Message)
            End Try
        End Using
    End Sub
    Private isStoreFinishedMessageDisplayed As Boolean = False
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



End Class

