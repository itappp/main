Imports System.Data.SqlClient
Imports System.Drawing ' Required for Font
Imports Microsoft.Office.Interop
Imports Excel = Microsoft.Office.Interop.Excel ' For Excel interop

Public Class storefinishviewform
    ' SQL Server connection string
    Private sqlServerConnectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"

    Private Sub storefinishviewform_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        LoadClientCodes()
        CalculateTotalHeight()
        LoadRefPacking()

    End Sub
    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        ' Validate controls initialization
        If dgvResults Is Nothing Then
            MessageBox.Show("DataGridView not initialized.")
            Return
        End If

        Dim worderId As String = txtworderid.Text
        Dim contractNo As String = txtContractNo.Text
        Dim batchNo As String = txtbatch.Text ' Get batch number from txtBatch
        Dim fromDate As DateTime = dtpfromdate.Value.Date
        Dim toDate As DateTime = dtptodate.Value.Date
        Dim clientCode As String = String.Empty
        Dim refPacking As String = If(cmbref.SelectedItem IsNot Nothing, cmbref.SelectedItem.ToString(), String.Empty)
        If cmbClient.SelectedItem IsNot Nothing Then
            clientCode = cmbClient.SelectedItem.ToString()
        End If

        Dim conditions As New List(Of String)

        If Not String.IsNullOrEmpty(worderId) Then
            conditions.Add("pf.worder_id LIKE @worderId")
        End If

        If Not String.IsNullOrEmpty(contractNo) Then
            conditions.Add("pf.contract_no = @contractNo")
        End If

        If Not String.IsNullOrEmpty(batchNo) Then
            conditions.Add("pf.batch_no = @batchNo") ' Add condition for batch_no
        End If

        If Not String.IsNullOrEmpty(clientCode) Then
            conditions.Add("pf.client_code = @clientCode")
        End If
        If Not String.IsNullOrEmpty(refPacking) Then
            conditions.Add("pk.ref_packing = @refPacking")
        End If

        If fromDate <> DateTime.MinValue AndAlso toDate <> DateTime.MinValue Then
            conditions.Add("CAST(pf.transaction_date AS DATE) BETWEEN @fromDate AND @toDate")
        ElseIf fromDate <> DateTime.MinValue Then
            conditions.Add("CAST(pf.transaction_date AS DATE) >= @fromDate")
        ElseIf toDate <> DateTime.MinValue Then
            conditions.Add("CAST(pf.transaction_date AS DATE) <= @toDate")
        End If

        Dim whereClause As String = If(conditions.Count > 0, "WHERE " & String.Join(" AND ", conditions), "")

        Dim query As String = "SELECT pf.worder_id AS 'أمر شغل', pf.contract_no AS 'رقم التعاقد', pf.batch_no AS 'رقم الرسالة', pf.ref_no AS 'رقم اذن العميل', pf.roll AS 'رقم التوب', pf.client_code AS 'كود العميل', pf.inspection_date AS 'تاريخ الفحص', pf.transaction_date AS 'تاريخ المخزن', pf.width AS 'العرض', pf.height AS 'الطول مخزن', pk.height AS 'الطول بيع', pf.weight AS 'الوزن مخزن', pk.weight AS 'الوزن بيع', pf.fabric_grade AS 'درجه القماش', pf.color AS 'اللون', pf.product_name AS 'الخامة', pk.ref_packing AS 'اذن البيع', pf.username AS 'user', " &
    "DATEDIFF(day, pf.inspection_date, pf.transaction_date) AS 'فرق الأيام', DATEDIFF(hour, pf.inspection_date, pf.transaction_date) AS 'فرق الساعات' " &
    "FROM store_finish pf LEFT JOIN packing pk ON pf.id = pk.storefinishid " &
    whereClause &
    " GROUP BY pf.worder_id, pf.contract_no, pf.batch_no, pf.ref_no, pf.roll, pf.client_code, pf.inspection_date, pf.transaction_date, pf.width, pf.height, pk.height, pf.weight, pk.weight, pf.fabric_grade, pf.color, pf.product_name, pk.ref_packing, pf.username"

        ' Pass parameters to LoadData or directly bind them to the SqlCommand
        LoadData(query, worderId, contractNo, fromDate, toDate, clientCode, batchNo, refPacking)
        CalculateFilteredTotalHeight()
        ApplyDataGridViewStyles()
    End Sub
    Private Sub LoadData(ByVal query As String, ByVal worderId As String, ByVal contractNo As String, ByVal fromDate As Date, ByVal toDate As Date, ByVal clientCode As String, ByVal batchNo As String, ByVal refpacking As String)
        Using connection As New SqlConnection(sqlServerConnectionString)
            Dim cmd As New SqlCommand(query, connection)

            ' Bind parameters if not empty
            If Not String.IsNullOrEmpty(worderId) Then
                cmd.Parameters.AddWithValue("@worderId", "%" & worderId & "%")
            End If
            If Not String.IsNullOrEmpty(contractNo) Then
                cmd.Parameters.AddWithValue("@contractNo", contractNo)
            End If
            If Not String.IsNullOrEmpty(batchNo) Then
                cmd.Parameters.AddWithValue("@batchNo", batchNo)
            End If
            If Not String.IsNullOrEmpty(clientCode) Then
                cmd.Parameters.AddWithValue("@clientCode", clientCode)
            End If
            If fromDate <> DateTime.MinValue Then
                cmd.Parameters.AddWithValue("@fromDate", fromDate)
            End If
            If toDate <> DateTime.MinValue Then
                cmd.Parameters.AddWithValue("@toDate", toDate)
            End If
            If Not String.IsNullOrEmpty(refpacking) Then
                cmd.Parameters.AddWithValue("@refPacking", refpacking)
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
    Private Sub LoadRefPacking()
        ' Clear existing items
        cmbref.Items.Clear()

        ' Create SQL connection and command
        Using connection As New SqlConnection(sqlServerConnectionString)
            Dim query As String = "SELECT DISTINCT ref_packing FROM packing WHERE ref_packing IS NOT NULL"

            Try
                connection.Open()
                Using command As New SqlCommand(query, connection)
                    Using reader As SqlDataReader = command.ExecuteReader()
                        While reader.Read()
                            cmbref.Items.Add(reader("ref_packing").ToString())
                        End While
                    End Using
                End Using
            Catch ex As Exception
                MessageBox.Show("Error loading ref_packing values: " & ex.Message)
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
                    lbltotalstock.Text = "  إجمالى رصيد المخزن متر : " & Convert.ToDecimal(heightSum).ToString("N2") ' Format to two decimal places
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
                query &= " AND worder_id LIKE @worderId"
                cmd.Parameters.AddWithValue("@worderId", "%" & txtworderid.Text & "%")
            End If

            If Not String.IsNullOrEmpty(txtContractNo.Text) Then
                query &= " AND contract_no = @contractNo"
                cmd.Parameters.AddWithValue("@contractNo", txtContractNo.Text)
            End If

            If cmbClient.SelectedIndex <> -1 Then
                query &= " AND client_code = @clientCode"
                cmd.Parameters.AddWithValue("@clientCode", cmbClient.SelectedItem.ToString())
            End If
            If Not String.IsNullOrEmpty(txtbatch.Text) Then
                query &= " AND batch_no = @batchNo"
                cmd.Parameters.AddWithValue("@batchNo", txtbatch.Text)
            End If

            cmd.CommandText = query

            Try
                connection.Open()
                Dim heightSum As Object = cmd.ExecuteScalar()
                ' Display the calculated height sum in lbltotal
                lbltotal.Text = "إجمالى الكمية للعميل: " & If(IsDBNull(heightSum) OrElse heightSum Is Nothing, 0, Convert.ToDecimal(heightSum)).ToString("N2") & " meters"
            Catch ex As Exception
                MessageBox.Show("An error occurred while calculating filtered total height: " & ex.Message)
            End Try
        End Using
    End Sub
    Private Sub ApplyDataGridViewStyles()
        ' Set font and alignment for the DataGridView content
        dgvResults.DefaultCellStyle.Font = New Font("Arial", 9, FontStyle.Bold)
        dgvResults.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        ' Set font and color for the DataGridView header
        dgvResults.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 10, FontStyle.Bold)
        dgvResults.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue
        dgvResults.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black
        dgvResults.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        ' Apply header and content styles
        dgvResults.EnableHeadersVisualStyles = True ' Allow custom header styles to take effect
    End Sub

    Private Sub btnExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click
        ExportToExcel(dgvResults)
    End Sub

    Private Sub ExportToExcel(ByVal dataGridView As DataGridView)
        Try
            If dataGridView.Rows.Count = 0 Then
                MessageBox.Show("No data to export.")
                Return
            End If

            ' Create a new Excel application instance
            Dim excelApp As New Microsoft.Office.Interop.Excel.Application()
            Dim workbook As Microsoft.Office.Interop.Excel.Workbook = excelApp.Workbooks.Add()
            Dim worksheet As Microsoft.Office.Interop.Excel.Worksheet = CType(workbook.Sheets(1), Microsoft.Office.Interop.Excel.Worksheet)

            ' Set the headers
            For i As Integer = 0 To dataGridView.Columns.Count - 1
                worksheet.Cells(1, i + 1) = dataGridView.Columns(i).HeaderText
                worksheet.Cells(1, i + 1).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
            Next

            ' Set the data and center-align each cell
            For i As Integer = 0 To dataGridView.Rows.Count - 1
                For j As Integer = 0 To dataGridView.Columns.Count - 1
                    worksheet.Cells(i + 2, j + 1) = If(dataGridView.Rows(i).Cells(j).Value IsNot Nothing, dataGridView.Rows(i).Cells(j).Value.ToString(), String.Empty)
                    worksheet.Cells(i + 2, j + 1).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
                Next
            Next

            ' Set column widths as needed
            worksheet.Columns("A").ColumnWidth = 15
            worksheet.Columns("B").ColumnWidth = 18
            worksheet.Columns("C").ColumnWidth = 15
            worksheet.Columns("D").ColumnWidth = 12
            worksheet.Columns("E").ColumnWidth = 12
            worksheet.Columns("F").ColumnWidth = 20

            ' Show Excel
            excelApp.Visible = True
        Catch ex As System.Runtime.InteropServices.COMException
            MessageBox.Show("COM Exception: " & ex.Message)
        Catch ex As Exception
            MessageBox.Show("An error occurred while exporting to Excel: " & ex.Message)
        End Try
    End Sub
End Class
