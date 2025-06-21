Imports System.Data.SqlClient
Imports Excel = Microsoft.Office.Interop.Excel
Imports OfficeOpenXml
Imports System.IO
Imports System.Runtime.InteropServices
Public Class salesprintpackingform

    ' Corrected SQL Server connection string
    Private sqlServerConnectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"
    Private headerCheckbox As CheckBox = New CheckBox()

    Private Sub pickpackform_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Access the logged-in username from the global variable

        lblUsername.Text = "Logged in as: " & LoggedInUsername
        ' Retrieve the public_name for the logged-in user
        Dim publicName As String = GetPublicName(LoggedInUsername)
        ' Initialize ComboBoxes and Button


        ' Populate ComboBoxes after loading data into dgvResults
        PopulateComboBoxes()
        LoadClientCodes()
        CalculateTotalHeight()
        CalculateTotalweight()
    End Sub
    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        ' Validate controls initialization
        If dgvResults Is Nothing Then
            MessageBox.Show("DataGridView not initialized.")
            Return
        End If

        Dim worderId As String = txtworderid.Text
        Dim contractNo As String = txtContractNo.Text
        Dim batchNo As String = txtBatch.Text ' Get batch number from txtBatch
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
        Dim query As String = "SELECT id, worder_id as 'أمر شغل', contract_no as 'رقم التعاقد', batch_no as 'رقم الرسالة', ref_no as 'رقم الإذن', roll as 'رقم التوب', client_code as 'كود العميل ', transaction_date as 'تاريخ المخزن', height as 'الطول', weight as 'الوزن', fabric_grade as 'درجه القماش', color as 'اللون', product_name as 'الخامة' FROM store_finish pf " & whereClause & " GROUP BY id, worder_id, contract_no, batch_no, ref_no, roll, client_code, inspection_date, transaction_date, height, weight, fabric_grade, color, product_name"

        ' Pass parameters to LoadData
        LoadData(query, worderIds, contractNos, batchNos, clientCode, fromDate, toDate)
        PopulateComboBoxes()
        CalculateFilteredTotalHeight()
        ApplyDataGridViewStyles()
        CalculateTotalweight()
        CalculateTotalHeight()
        CalculateFilteredTotalweight()
        CalculateFilteredRollCount()
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
                dgvResults.DataSource = table

            Catch ex As Exception
                MessageBox.Show("An error occurred: " & ex.Message)
            End Try
        End Using
    End Sub
    Private Sub BtnSelectAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnselectall.Click
        ' Get selected values
        Dim selectedProductName As String = cmbProductName.SelectedItem.ToString()
        Dim selectedColor As String = cmbColor.SelectedItem.ToString()

        If String.IsNullOrEmpty(selectedProductName) OrElse String.IsNullOrEmpty(selectedColor) Then
            MessageBox.Show("Please select both Product Name and Color.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Loop through rows to find matching ones
        For Each row As DataGridViewRow In dgvResults.Rows
            If Not row.IsNewRow AndAlso
               row.Cells("الخامة").Value.ToString() = selectedProductName AndAlso
               row.Cells("اللون").Value.ToString() = selectedColor Then
                row.Cells("select_checkbox").Value = True ' Select matching rows
            End If
        Next
        UpdateSum()
    End Sub
Private Sub PopulateComboBoxes()
        If dgvResults.Rows.Count > 0 Then
            ' Fetch distinct Product Names for cmbProductName
            Dim productNames = dgvResults.Rows.Cast(Of DataGridViewRow)().
                               Where(Function(row) Not row.IsNewRow AndAlso row.Cells("الخامة").Value IsNot Nothing).
                               Select(Function(row) row.Cells("الخامة").Value.ToString()).
                               Distinct().ToList()

            cmbProductName.Items.Clear()
            cmbProductName.Items.AddRange(productNames.ToArray())

            ' Optionally clear cmbColor as it will depend on Product Name
            cmbColor.Items.Clear()
        Else
            Debug.WriteLine("DataGridView is empty.")
        End If
    End Sub
    Private Sub cmbProductName_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbProductName.SelectedIndexChanged
        ' Get selected Product Name
        Dim selectedProductName As String = If(cmbProductName.SelectedItem IsNot Nothing, cmbProductName.SelectedItem.ToString(), String.Empty)

        ' Filter Colors based on the selected Product Name
        Dim colors = dgvResults.Rows.Cast(Of DataGridViewRow)().
                     Where(Function(row) Not row.IsNewRow AndAlso
                                          row.Cells("الخامة").Value IsNot Nothing AndAlso
                                          row.Cells("الخامة").Value.ToString() = selectedProductName AndAlso
                                          row.Cells("اللون").Value IsNot Nothing).
                     Select(Function(row) row.Cells("اللون").Value.ToString()).
                     Distinct().
                     ToList()

        ' Populate cmbColor with filtered colors
        cmbColor.Items.Clear()
        cmbColor.Items.AddRange(colors.ToArray())

        ' Automatically select the first item if available
        If cmbColor.Items.Count > 0 Then
            cmbColor.SelectedIndex = 0
        End If
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
            UpdateSum() ' Update sum whenever the checkbox is clicked
        End If
    End Sub
    Private Sub SelectAllCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim isChecked As Boolean = selectAllCheckBox.Checked
        For Each row As DataGridViewRow In dgvresults.Rows
            row.Cells("select_checkbox").Value = isChecked
        Next
        dgvresults.RefreshEdit()
        UpdateSum()
    End Sub
    Private Sub UpdateSum()
        ' Declare the dictionary to hold the sum of height and weight for each "أمر شغل"
        Dim dictSum As New Dictionary(Of String, Tuple(Of Double, Double)) ' key: أمر شغل, value: (totalHeight, totalWeight)

        ' Loop through the selected rows and calculate the sum of height and weight for each أمر شغل
        For Each row As DataGridViewRow In dgvResults.Rows
            If Convert.ToBoolean(row.Cells("select_checkbox").Value) Then ' Check if the row is selected
                ' Check for valid data
                If Not IsDBNull(row.Cells("الطول").Value) AndAlso Not IsDBNull(row.Cells("الوزن").Value) Then
                    Dim worder As String = row.Cells("أمر شغل").Value.ToString()
                    Dim height As Double = Convert.ToDouble(row.Cells("الطول").Value)
                    Dim weight As Double = Convert.ToDouble(row.Cells("الوزن").Value)

                    ' If أمر شغل already exists in the dictionary, update the sum
                    If dictSum.ContainsKey(worder) Then
                        dictSum(worder) = New Tuple(Of Double, Double)(dictSum(worder).Item1 + height, dictSum(worder).Item2 + weight)
                    Else
                        ' Otherwise, add it to the dictionary
                        dictSum.Add(worder, New Tuple(Of Double, Double)(height, weight))
                    End If
                End If
            End If
        Next

        ' Display the sum for each أمر شغل in the label
        Dim sumText As String = ""
        For Each worder In dictSum.Keys
            Dim totalHeight As Double = dictSum(worder).Item1
            Dim totalWeight As Double = dictSum(worder).Item2
            sumText &= String.Format("أمر شغل: {0} -       M: {1}        KG: {2}" & vbCrLf, worder, totalHeight, totalWeight)
        Next

        ' Display the results in a label
        lblSumHeightWeight.Text = sumText
    End Sub


    Private Sub btnPrint_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnprint.Click
        ' Check if there are selected rows
        Dim selectedRows As New List(Of DataGridViewRow)
        For Each row As DataGridViewRow In dgvresults.Rows
            If row.Cells("select_checkbox").Value IsNot Nothing AndAlso Convert.ToBoolean(row.Cells("select_checkbox").Value) Then
                selectedRows.Add(row)
            End If
        Next

        ' Export only if there are selected rows
        If selectedRows.Count > 0 Then
            ExportSelectedRowsToExcel(selectedRows)
        Else
            MessageBox.Show("No rows selected for export.")
        End If

    End Sub
    Private Sub ExportSelectedRowsToExcel(ByVal selectedRows As List(Of DataGridViewRow))
        ' Create an Excel application object
        Dim excelApp As New Excel.Application()
        Dim workbook As Excel.Workbook = excelApp.Workbooks.Add()
        Dim worksheet As Excel.Worksheet = workbook.Sheets(1)

        ' Set up headers in Excel
        For colIndex As Integer = 1 To dgvresults.Columns.Count - 1
            worksheet.Cells(1, colIndex).Value = dgvresults.Columns(colIndex).HeaderText
        Next

        ' Copy data from selected rows to the Excel worksheet
        Dim rowIndex As Integer = 2 ' Start from the second row for data
        For Each row As DataGridViewRow In selectedRows
            For colIndex As Integer = 1 To dgvresults.Columns.Count - 1
                worksheet.Cells(rowIndex, colIndex).Value = row.Cells(colIndex).Value
            Next
            rowIndex += 1
        Next

        ' Determine the range to format
        Dim lastColumn As Integer = dgvresults.Columns.Count - 1
        Dim lastRow As Integer = rowIndex - 1
        Dim dataRange As Excel.Range = worksheet.Range(worksheet.Cells(1, 1), worksheet.Cells(lastRow, lastColumn))

        ' Apply formatting to the range
        With dataRange
            ' Bold and font size
            .Font.Bold = True
            .Font.Size = 12 ' Change font size to 12 or your preference

            ' Center align text horizontally and vertically
            .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
            .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter

            ' Apply borders to all cells
            .Borders(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous
            .Borders(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous
            .Borders(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous
            .Borders(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous
            .Borders(Excel.XlBordersIndex.xlInsideHorizontal).LineStyle = Excel.XlLineStyle.xlContinuous
            .Borders(Excel.XlBordersIndex.xlInsideVertical).LineStyle = Excel.XlLineStyle.xlContinuous
        End With

        ' Assuming you have the worksheet and lastRow defined
        Dim concatFormula As String = "=TEXTJOIN("","", TRUE, A2:A" & lastRow & ")"

        ' Insert the formula in the next available row (lastRow + 1)
        worksheet.Cells(lastRow + 1, 1).Formula = concatFormula

        ' Set text wrapping for the cell
        worksheet.Cells(lastRow + 1, 1).Style.WrapText = True



        ' AutoFit columns
        worksheet.Columns.AutoFit()

        ' Make Excel visible
        excelApp.Visible = True

        ' Release COM objects to free resources
        Marshal.ReleaseComObject(worksheet)
        Marshal.ReleaseComObject(workbook)
        Marshal.ReleaseComObject(excelApp)

        worksheet = Nothing
        workbook = Nothing
        excelApp = Nothing

        ' Inform the user
        MessageBox.Show("Export completed successfully.")
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

