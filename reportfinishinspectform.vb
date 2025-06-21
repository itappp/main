Imports System.Data.SqlClient
Imports ClosedXML.Excel

Public Class reportfinishinspectform
    Private sqlServerConnectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"

    Private Sub reportfinishinspectform_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Add items to cmbShift
        cmbShift.Items.Add("All Shifts")
        cmbShift.Items.Add("Shift 1")
        cmbShift.Items.Add("Shift 2")
        cmbShift.Items.Add("Shift 3")
        cmbShift.Items.Add("Shift 1&2")
        cmbShift.Items.Add("Shift 2&3")
        cmbShift.Items.Add("Shift 1&3")

        ' Set default selected item
        cmbShift.SelectedIndex = 0
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim worderId As String = txtWorderId.Text.Trim()
        Dim contract As String = txtContract.Text.Trim()
        Dim batch As String = txtBatch.Text.Trim()
        Dim dateFrom As DateTime = dtpDateFrom.Value
        Dim dateTo As DateTime = dtpDateto.Value
        Dim shift As String = cmbShift.SelectedItem.ToString()

        Dim query As String = "SELECT fi.worder_id as 'أمر شغل', count(fi.roll) AS 'عدد الأتواب', SUM(fi.height) AS 'الطول', SUM(fi.weight) AS 'الوزن', c.contractno as 'رقم التعاقد', cs.code as 'كود العميل', c.batch as 'رقم الرسالة', " &
                              "CASE " &
                              "WHEN DATEPART(HOUR, fi.date) >= 8 AND DATEPART(HOUR, fi.date) < 16 THEN 'Shift 1' " &
                              "WHEN DATEPART(HOUR, fi.date) >= 16 AND DATEPART(HOUR, fi.date) < 23 THEN 'Shift 2' " &
                              "WHEN DATEPART(HOUR, fi.date) >= 23 OR (DATEPART(HOUR, fi.date) < 8 AND DATEPART(HOUR, fi.date) >= 0) THEN 'Shift 3' " &
                              "END AS 'الوردية', " &
                              "CASE " &
                              "WHEN DATEPART(HOUR, fi.date) >= 23 THEN CONVERT(date, fi.date) " &
                              "WHEN DATEPART(HOUR, fi.date) < 8 THEN CONVERT(date, DATEADD(day, -1, fi.date)) " &
                              "ELSE CONVERT(date, fi.date) " &
                              "END AS 'تاريخ الوردية', " &
                              "us.user_ar AS 'العامل' " &
                              "FROM finish_inspect fi " &
                              "LEFT JOIN techdata td ON fi.worder_id = td.worderid " &
                              "LEFT JOIN Contracts c ON td.contract_id = c.ContractID " &
                              "LEFT JOIN Clients cs ON c.ClientCode = cs.ID " &
                              "LEFT JOIN dep_users us ON fi.username = us.username " &
                              "WHERE 1=1"

        If Not String.IsNullOrEmpty(worderId) Then
            query &= " AND fi.worder_id = @worderid"
        End If

        If Not String.IsNullOrEmpty(contract) Then
            query &= " AND c.contractno LIKE @contract"
        End If

        If Not String.IsNullOrEmpty(batch) Then
            query &= " AND c.batch LIKE @batch"
        End If

        If dateFrom <= dateTo Then
            query &= " AND CASE WHEN DATEPART(HOUR, fi.date) >= 23 THEN CONVERT(date, fi.date) " &
                     "WHEN DATEPART(HOUR, fi.date) < 8 THEN CONVERT(date, DATEADD(day, -1, fi.date)) " &
                     "ELSE CONVERT(date, fi.date) END BETWEEN @dateFrom AND @dateTo"
        End If

        ' Add shift condition
        Select Case shift
            Case "Shift 1"
                query &= " AND DATEPART(HOUR, fi.date) >= 8 AND DATEPART(HOUR, fi.date) < 16"
            Case "Shift 2"
                query &= " AND DATEPART(HOUR, fi.date) >= 16 AND DATEPART(HOUR, fi.date) < 23"
            Case "Shift 3"
                query &= " AND (DATEPART(HOUR, fi.date) >= 23 OR (DATEPART(HOUR, fi.date) < 8 AND DATEPART(HOUR, fi.date) >= 0))"
            Case "Shift 1&2"
                query &= " AND DATEPART(HOUR, fi.date) >= 8 AND DATEPART(HOUR, fi.date) < 23"
            Case "Shift 2&3"
                query &= " AND (DATEPART(HOUR, fi.date) >= 16 OR (DATEPART(HOUR, fi.date) < 8 AND DATEPART(HOUR, fi.date) >= 0))"
            Case "Shift 1&3"
                query &= " AND (DATEPART(HOUR, fi.date) >= 8 AND DATEPART(HOUR, fi.date) < 16 OR DATEPART(HOUR, fi.date) >= 23 OR (DATEPART(HOUR, fi.date) < 8 AND DATEPART(HOUR, fi.date) >= 0))"
            Case "All Shifts"
                ' No additional condition needed for All Shifts
        End Select

        query &= " GROUP BY fi.worder_id, c.contractno, cs.code,c.batch, " &
                 "CASE WHEN DATEPART(HOUR, fi.date) >= 23 THEN CONVERT(date, fi.date) " &
                 "WHEN DATEPART(HOUR, fi.date) < 8 THEN CONVERT(date, DATEADD(day, -1, fi.date)) " &
                 "ELSE CONVERT(date, fi.date) END, " &
                 "CASE WHEN DATEPART(HOUR, fi.date) >= 8 AND DATEPART(HOUR, fi.date) < 16 THEN 'Shift 1' " &
                 "WHEN DATEPART(HOUR, fi.date) >= 16 AND DATEPART(HOUR, fi.date) < 23 THEN 'Shift 2' " &
                 "WHEN DATEPART(HOUR, fi.date) >= 23 OR (DATEPART(HOUR, fi.date) < 8 AND DATEPART(HOUR, fi.date) >= 0) THEN 'Shift 3' END, us.user_ar"

        Using conn As New SqlConnection(sqlServerConnectionString)
            Using cmd As New SqlCommand(query, conn)
                If Not String.IsNullOrEmpty(worderId) Then
                    cmd.Parameters.AddWithValue("@worderid", worderId)
                End If

                If Not String.IsNullOrEmpty(contract) Then
                    cmd.Parameters.AddWithValue("@contract", "%" & contract & "%")
                End If

                If Not String.IsNullOrEmpty(batch) Then
                    cmd.Parameters.AddWithValue("@batch", "%" & batch & "%")
                End If

                If dateFrom <= dateTo Then
                    cmd.Parameters.AddWithValue("@dateFrom", dateFrom.Date)
                    cmd.Parameters.AddWithValue("@dateTo", dateTo.Date)
                End If

                Try
                    conn.Open()
                    Dim reader As SqlDataReader = cmd.ExecuteReader()
                    Dim dt As New DataTable()
                    dt.Load(reader)
                    dgvResults.DataSource = dt

                    ' Calculate totals
                    Dim totalHeight As Decimal = dt.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("الطول"))
                    Dim totalWeight As Decimal = dt.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("الوزن"))
                    Dim countRolls As Integer = dt.AsEnumerable().Sum(Function(row) row.Field(Of Integer)("عدد الأتواب"))

                    ' Update labels
                    lblTotalHeight.Text = "اجمالى متر: " & totalHeight.ToString()
                    lblTotalWeight.Text = "اجمالى وزن: " & totalWeight.ToString()
                    lblCountRolls.Text = "اجمالى اتواب: " & countRolls.ToString()

                    ' Format DataGridView
                    FormatDataGridView()
                Catch ex As Exception
                    MessageBox.Show("Error loading search results: " & ex.Message)
                End Try
            End Using
        End Using
    End Sub

    Private Sub btnSearch2_Click(sender As Object, e As EventArgs) Handles btnsearch2.Click
        Dim worderId As String = txtWorderId.Text.Trim()
        Dim contract As String = txtContract.Text.Trim()
        Dim batch As String = txtBatch.Text.Trim()
        Dim dateFrom As DateTime = dtpDateFrom.Value
        Dim dateTo As DateTime = dtpDateto.Value
        Dim shift As String = cmbShift.SelectedItem.ToString()

        Dim query As String = "SELECT fi.worder_id as 'أمر شغل', COUNT(fi.roll) AS 'عدد الأتواب',fi.fabric_grade as 'الدرجه ', SUM(fi.height) AS 'الطول', SUM(fi.weight) AS 'الوزن', c.contractno as 'رقم التعاقد', cs.code as 'كود العميل', c.batch as 'رقم الرسالة', " &
                              "CASE " &
                              "WHEN DATEPART(HOUR, fi.date) >= 23 THEN CONVERT(date, fi.date) " &
                              "WHEN DATEPART(HOUR, fi.date) < 8 THEN CONVERT(date, DATEADD(day, -1, fi.date)) " &
                              "ELSE CONVERT(date, fi.date) " &
                              "END AS 'تاريخ الوردية' " &
                              "FROM finish_inspect fi " &
                              "LEFT JOIN techdata td ON fi.worder_id = td.worderid " &
                              "LEFT JOIN Contracts c ON td.contract_id = c.ContractID " &
                              "LEFT JOIN Clients cs ON c.ClientCode = cs.ID " &
                              "LEFT JOIN dep_users us ON fi.username = us.username " &
                              "WHERE 1=1"

        If Not String.IsNullOrEmpty(worderId) Then
            query &= " AND fi.worder_id = @worderid"
        End If

        If Not String.IsNullOrEmpty(contract) Then
            query &= " AND c.contractno LIKE @contract"
        End If

        If Not String.IsNullOrEmpty(batch) Then
            query &= " AND c.batch LIKE @batch"
        End If

        If dateFrom <= dateTo Then
            query &= " AND CASE WHEN DATEPART(HOUR, fi.date) >= 23 THEN CONVERT(date, fi.date) " &
                     "WHEN DATEPART(HOUR, fi.date) < 8 THEN CONVERT(date, DATEADD(day, -1, fi.date)) " &
                     "ELSE CONVERT(date, fi.date) END BETWEEN @dateFrom AND @dateTo"
        End If

        ' Add shift condition
        Select Case shift
            Case "Shift 1"
                query &= " AND DATEPART(HOUR, fi.date) >= 8 AND DATEPART(HOUR, fi.date) < 16"
            Case "Shift 2"
                query &= " AND DATEPART(HOUR, fi.date) >= 16 AND DATEPART(HOUR, fi.date) < 23"
            Case "Shift 3"
                query &= " AND (DATEPART(HOUR, fi.date) >= 23 OR (DATEPART(HOUR, fi.date) < 8 AND DATEPART(HOUR, fi.date) >= 0))"
            Case "Shift 1&2"
                query &= " AND DATEPART(HOUR, fi.date) >= 8 AND DATEPART(HOUR, fi.date) < 23"
            Case "Shift 2&3"
                query &= " AND (DATEPART(HOUR, fi.date) >= 16 OR (DATEPART(HOUR, fi.date) < 8 AND DATEPART(HOUR, fi.date) >= 0))"
            Case "Shift 1&3"
                query &= " AND (DATEPART(HOUR, fi.date) >= 8 AND DATEPART(HOUR, fi.date) < 16 OR DATEPART(HOUR, fi.date) >= 23 OR (DATEPART(HOUR, fi.date) < 8 AND DATEPART(HOUR, fi.date) >= 0))"
            Case "All Shifts"
                ' No additional condition needed for All Shifts
        End Select

        query &= " GROUP BY fi.worder_id, c.contractno, cs.code,c.batch,fi.fabric_grade, " &
                 "CASE WHEN DATEPART(HOUR, fi.date) >= 23 THEN CONVERT(date, fi.date) " &
                 "WHEN DATEPART(HOUR, fi.date) < 8 THEN CONVERT(date, DATEADD(day, -1, fi.date)) " &
                 "ELSE CONVERT(date, fi.date) END"


        Using conn As New SqlConnection(sqlServerConnectionString)
            Using cmd As New SqlCommand(query, conn)
                If Not String.IsNullOrEmpty(worderId) Then
                    cmd.Parameters.AddWithValue("@worderid", worderId)
                End If

                If Not String.IsNullOrEmpty(contract) Then
                    cmd.Parameters.AddWithValue("@contract", "%" & contract & "%")
                End If

                If Not String.IsNullOrEmpty(batch) Then
                    cmd.Parameters.AddWithValue("@batch", "%" & batch & "%")
                End If

                If dateFrom <= dateTo Then
                    cmd.Parameters.AddWithValue("@dateFrom", dateFrom.Date)
                    cmd.Parameters.AddWithValue("@dateTo", dateTo.Date)
                End If

                Try
                    conn.Open()
                    Dim reader As SqlDataReader = cmd.ExecuteReader()
                    Dim dt As New DataTable()
                    dt.Load(reader)
                    dgvResults.DataSource = dt

                    ' Calculate totals
                    Dim totalHeight As Decimal = dt.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("الطول"))
                    Dim totalWeight As Decimal = dt.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("الوزن"))
                    Dim countRolls As Integer = dt.AsEnumerable().Sum(Function(row) row.Field(Of Integer)("عدد الأتواب"))

                    ' Update labels
                    lblTotalHeight.Text = "اجمالى متر: " & totalHeight.ToString()
                    lblTotalWeight.Text = "اجمالى وزن: " & totalWeight.ToString()
                    lblCountRolls.Text = "اجمالى اتواب: " & countRolls.ToString()

                    ' Format DataGridView
                    FormatDataGridView()
                Catch ex As Exception
                    MessageBox.Show("Error loading search results: " & ex.Message)
                End Try
            End Using
        End Using
    End Sub



    Private Sub FormatDataGridView()
        ' Center the content of the DataGridView
        For Each column As DataGridViewColumn In dgvResults.Columns
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            column.DefaultCellStyle.Font = New Font("Arial", 11)
        Next

        ' Set the header font size to 12, make it bold, and center the content
        dgvResults.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 12, FontStyle.Bold)
        dgvResults.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        ' Fill the color of the headers
        dgvResults.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue
        dgvResults.EnableHeadersVisualStyles = False
    End Sub

    Private Sub btnExportToExcel_Click(sender As Object, e As EventArgs) Handles btnExportToExcel.Click
        If dgvResults.Rows.Count > 0 Then
            Try
                Using workbook As New XLWorkbook()
                    ' Add the main worksheet
                    Dim worksheet = workbook.Worksheets.Add("Report")
                    For i As Integer = 0 To dgvResults.Columns.Count - 1
                        worksheet.Cell(1, i + 1).Value = dgvResults.Columns(i).HeaderText
                        worksheet.Cell(1, i + 1).Style.Font.Bold = True
                        worksheet.Cell(1, i + 1).Style.Font.FontSize = 12
                        worksheet.Cell(1, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                        worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.LightBlue
                        worksheet.Column(i + 1).AdjustToContents() ' Adjust column width
                        worksheet.Column(i + 1).Style.Alignment.WrapText = False ' Disable wrap text
                    Next

                    For i As Integer = 0 To dgvResults.Rows.Count - 1
                        For j As Integer = 0 To dgvResults.Columns.Count - 1
                            worksheet.Cell(i + 2, j + 1).Value = dgvResults.Rows(i).Cells(j).Value
                            worksheet.Cell(i + 2, j + 1).Style.Font.FontSize = 11
                            worksheet.Cell(i + 2, j + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                            worksheet.Cell(i + 2, j + 1).Style.Alignment.WrapText = False ' Disable wrap text
                        Next
                    Next

                    ' Calculate totals for the main worksheet
                    Dim totalRollsReport As Integer = dgvResults.Rows.Cast(Of DataGridViewRow)().Sum(Function(row) Convert.ToInt32(row.Cells("عدد الأتواب").Value))
                    Dim totalHeightReport As Decimal = dgvResults.Rows.Cast(Of DataGridViewRow)().Sum(Function(row) Convert.ToDecimal(row.Cells("الطول").Value))
                    Dim totalWeightReport As Decimal = dgvResults.Rows.Cast(Of DataGridViewRow)().Sum(Function(row) Convert.ToDecimal(row.Cells("الوزن").Value))

                    ' Add totals row to the main worksheet
                    Dim totalRowIndexReport As Integer = dgvResults.Rows.Count + 2
                    worksheet.Cell(totalRowIndexReport, 1).Value = "الإجمالي"
                    worksheet.Cell(totalRowIndexReport, 2).Value = totalRollsReport
                    worksheet.Cell(totalRowIndexReport, 3).Value = totalHeightReport
                    worksheet.Cell(totalRowIndexReport, 4).Value = totalWeightReport

                    ' Set totals row styles for the main worksheet
                    For i As Integer = 1 To 4
                        worksheet.Cell(totalRowIndexReport, i).Style.Font.Bold = True
                        worksheet.Cell(totalRowIndexReport, i).Style.Font.FontSize = 12
                        worksheet.Cell(totalRowIndexReport, i).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                        worksheet.Cell(totalRowIndexReport, i).Style.Fill.BackgroundColor = XLColor.LightGray
                    Next

                    ' Add the summary worksheet
                    Dim summaryWorksheet = workbook.Worksheets.Add("Summary")
                    summaryWorksheet.Cell(1, 1).Value = "أمر شغل"
                    summaryWorksheet.Cell(1, 2).Value = "اجمالى اتواب"
                    summaryWorksheet.Cell(1, 3).Value = "اجمالى الطول"
                    summaryWorksheet.Cell(1, 4).Value = "اجمالى الوزن"
                    summaryWorksheet.Cell(1, 5).Value = "رقم التعاقد"
                    summaryWorksheet.Cell(1, 6).Value = "كود العميل"
                    summaryWorksheet.Cell(1, 7).Value = "رقم الرسالة"
                    summaryWorksheet.Cell(1, 8).Value = "تاريخ الوردية"

                    ' Set header styles
                    For i As Integer = 1 To 8
                        summaryWorksheet.Cell(1, i).Style.Font.Bold = True
                        summaryWorksheet.Cell(1, i).Style.Font.FontSize = 12
                        summaryWorksheet.Cell(1, i).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                        summaryWorksheet.Cell(1, i).Style.Fill.BackgroundColor = XLColor.LightBlue
                        summaryWorksheet.Column(i).AdjustToContents() ' Adjust column width
                        summaryWorksheet.Column(i).Style.Alignment.WrapText = False ' Disable wrap text
                    Next

                    ' Group and summarize data
                    Dim summaryData = dgvResults.Rows.Cast(Of DataGridViewRow)().
                    GroupBy(Function(row) New With {
                        Key .worder_id = row.Cells("أمر شغل").Value,
                        Key .contractno = row.Cells("رقم التعاقد").Value,
                        Key .clientCode = row.Cells("كود العميل").Value,
                        Key .batch = row.Cells("رقم الرسالة").Value,
                        Key .shiftDate = row.Cells("تاريخ الوردية").Value
                    }).
                    Select(Function(g) New With {
                        .worder_id = g.Key.worder_id,
                        .totalRolls = g.Sum(Function(row) Convert.ToInt32(row.Cells("عدد الأتواب").Value)),
                        .totalHeight = g.Sum(Function(row) Convert.ToDecimal(row.Cells("الطول").Value)),
                        .totalWeight = g.Sum(Function(row) Convert.ToDecimal(row.Cells("الوزن").Value)),
                        .contractno = g.Key.contractno,
                        .clientCode = g.Key.clientCode,
                        .batch = g.Key.batch,
                        .shiftDate = g.Key.shiftDate
                    }).ToList()

                    ' Fill summary worksheet
                    For i As Integer = 0 To summaryData.Count - 1
                        summaryWorksheet.Cell(i + 2, 1).Value = summaryData(i).worder_id
                        summaryWorksheet.Cell(i + 2, 2).Value = summaryData(i).totalRolls
                        summaryWorksheet.Cell(i + 2, 3).Value = summaryData(i).totalHeight
                        summaryWorksheet.Cell(i + 2, 4).Value = summaryData(i).totalWeight
                        summaryWorksheet.Cell(i + 2, 5).Value = summaryData(i).contractno
                        summaryWorksheet.Cell(i + 2, 6).Value = summaryData(i).clientCode
                        summaryWorksheet.Cell(i + 2, 7).Value = summaryData(i).batch
                        summaryWorksheet.Cell(i + 2, 8).Value = summaryData(i).shiftDate
                    Next

                    ' Calculate totals for the summary worksheet
                    Dim totalRollsSummary As Integer = summaryData.Sum(Function(item) item.totalRolls)
                    Dim totalHeightSummary As Decimal = summaryData.Sum(Function(item) item.totalHeight)
                    Dim totalWeightSummary As Decimal = summaryData.Sum(Function(item) item.totalWeight)

                    ' Add totals row to the summary worksheet
                    Dim totalRowIndexSummary As Integer = summaryData.Count + 2
                    summaryWorksheet.Cell(totalRowIndexSummary, 1).Value = "الإجمالي"
                    summaryWorksheet.Cell(totalRowIndexSummary, 2).Value = totalRollsSummary
                    summaryWorksheet.Cell(totalRowIndexSummary, 3).Value = totalHeightSummary
                    summaryWorksheet.Cell(totalRowIndexSummary, 4).Value = totalWeightSummary

                    ' Set totals row styles for the summary worksheet
                    For i As Integer = 1 To 4
                        summaryWorksheet.Cell(totalRowIndexSummary, i).Style.Font.Bold = True
                        summaryWorksheet.Cell(totalRowIndexSummary, i).Style.Font.FontSize = 12
                        summaryWorksheet.Cell(totalRowIndexSummary, i).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                        summaryWorksheet.Cell(totalRowIndexSummary, i).Style.Fill.BackgroundColor = XLColor.LightGray
                    Next

                    ' Adjust column widths and disable wrap text for summary worksheet
                    For i As Integer = 1 To 8
                        summaryWorksheet.Column(i).AdjustToContents()
                        summaryWorksheet.Column(i).Style.Alignment.WrapText = False
                    Next

                    ' Save the workbook to a temporary file
                    Dim tempFilePath As String = IO.Path.GetTempFileName() & ".xlsx"
                    workbook.SaveAs(tempFilePath)

                    ' Open the temporary file
                    Process.Start(tempFilePath)
                End Using
            Catch ex As Exception
                MessageBox.Show("Error exporting data: " & ex.Message, "Export to Excel", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Else
            MessageBox.Show("No data to export.", "Export to Excel", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub




End Class


