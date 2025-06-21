Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Text
Imports Microsoft.Office.Interop
Imports Excel = Microsoft.Office.Interop.Excel
Imports System.IO

Public Class packingviewform
    ' SQL Server connection string
    Private sqlServerConnectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"

    ' Load event for the form
    Private Sub packingviewform_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        LoadRefPacking()
    End Sub

    ' Load ref_packing dropdown values
    Private Sub LoadRefPacking()
        cmbref.Items.Clear()

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

    ' Search button click event
    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        If dgvResults Is Nothing Then
            MessageBox.Show("DataGridView not initialized.")
            Return
        End If

        Dim worderId As String = txtworderid.Text
        Dim contractNo As String = txtContractNo.Text
        Dim fromDate As DateTime = dtpfromdate.Value.Date
        Dim toDate As DateTime = dtpToDate.Value.Date
        Dim refPacking As String = If(cmbref.SelectedItem IsNot Nothing, cmbref.SelectedItem.ToString(), String.Empty)

        Dim conditions As New List(Of String)

        If Not String.IsNullOrEmpty(worderId) Then
            conditions.Add("pf.worder_id LIKE @worderId")
        End If

        If Not String.IsNullOrEmpty(contractNo) Then
            conditions.Add("pf.contract_no = @contractNo")
        End If

        If Not String.IsNullOrEmpty(refPacking) Then
            conditions.Add("pk.ref_packing = @refPacking")
        End If

        If fromDate <> DateTime.MinValue AndAlso toDate <> DateTime.MinValue Then
            conditions.Add("CAST(pk.date AS DATE) BETWEEN @fromDate AND @toDate")
        ElseIf fromDate <> DateTime.MinValue Then
            conditions.Add("CAST(pk.date AS DATE) >= @fromDate")
        ElseIf toDate <> DateTime.MinValue Then
            conditions.Add("CAST(pk.date AS DATE) <= @toDate")
        End If

        Dim whereClause As String = If(conditions.Count > 0, "WHERE " & String.Join(" AND ", conditions), "")

        Dim query As String = "SELECT pf.worder_id AS 'أمر شغل', clients.name AS 'العميل', clients.code AS 'كود العميل شحن', " &
                              "pf.contract_no AS 'رقم التعاقد', pf.batch_no AS 'رقم الرسالة', pf.ref_no AS 'رقم اذن العميل', " &
                              "pf.qc_roll as'توافق أتواب',pf.roll AS 'رقم التوب', pk.date AS 'تاريخ الشحن', pf.width AS 'العرض', pk.height AS 'الطول بيع', " &
                              "pk.weight AS 'الوزن بيع', pf.fabric_grade AS 'درجه القماش', pf.color AS 'اللون', pf.product_name AS 'الخامة', " &
                              "pk.ref_packing AS 'اذن البيع', pf.client_code AS 'كود العميل', pk.username AS 'اسم المستخدم' " &
                              "FROM packing pk " &
                              "LEFT JOIN store_finish pf ON pk.storefinishid = pf.id " &
                              "LEFT JOIN clients ON pk.toclient = clients.id " &
                              whereClause &
                              " GROUP BY pf.worder_id, clients.name, clients.code, pf.contract_no, pf.batch_no, pf.ref_no, pf.qc_roll,pf.roll, " &
                              "pk.date, pf.width, pk.height, pk.weight, pf.fabric_grade, pf.color, pf.product_name, pk.ref_packing, pf.client_code, pk.username"

        LoadData(query, worderId, contractNo, fromDate, toDate, refPacking)
    End Sub

    ' Load data into DataGridView
    Private Sub LoadData(ByVal query As String, ByVal worderId As String, ByVal contractNo As String, ByVal fromDate As DateTime, ByVal toDate As DateTime, ByVal refPacking As String)
        Using connection As New SqlConnection(sqlServerConnectionString)
            Dim adapter As New SqlDataAdapter(query, connection)
            Dim dt As New DataTable()

            If Not String.IsNullOrEmpty(worderId) Then
                adapter.SelectCommand.Parameters.AddWithValue("@worderId", "%" & worderId & "%")
            End If

            If Not String.IsNullOrEmpty(contractNo) Then
                adapter.SelectCommand.Parameters.AddWithValue("@contractNo", contractNo)
            End If

            If fromDate <> DateTime.MinValue Then
                adapter.SelectCommand.Parameters.AddWithValue("@fromDate", fromDate)
            End If

            If toDate <> DateTime.MinValue Then
                adapter.SelectCommand.Parameters.AddWithValue("@toDate", toDate)
            End If

            If Not String.IsNullOrEmpty(refPacking) Then
                adapter.SelectCommand.Parameters.AddWithValue("@refPacking", refPacking)
            End If

            Try
                connection.Open()
                adapter.Fill(dt)

                If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                    MessageBox.Show("No data found for the given criteria.")
                    Return
                End If

                dgvResults.DataSource = dt
            Catch ex As Exception
                MessageBox.Show("An error occurred while loading data: " & ex.Message)
            End Try
        End Using
    End Sub


    Private Sub btnPrint_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnprint.Click
        Dim refPacking As String = If(Not String.IsNullOrEmpty(txtrefprint.Text), txtrefprint.Text, String.Empty)

        ' If refPacking is empty, show a message
        If String.IsNullOrEmpty(refPacking) Then
            MessageBox.Show("Please enter reference packing values.")
            Return
        End If

        ' Split the refPacking string into individual items (comma-separated)
        Dim refPackingList As String() = refPacking.Split(","c)
        ' Optionally, trim spaces
        refPackingList = refPackingList.Select(Function(r) r.Trim()).ToArray()

        Dim query As String = "SELECT pk.ref_packing AS 'اذن بيع', " &
                               "cs.name AS 'العميل', cs.code AS 'كود العميل', " &
                               "pf.worder_id AS 'أمر شغل', pf.batch_no AS 'رقم الرسالة', pf.ref_no AS 'اذن العميل', " &
                               "COUNT(pf.roll) AS 'الأتواب', " &
                               "SUM(CAST(pk.height AS FLOAT)) AS 'إجمالى أمتار', " &
                               "SUM(CAST(pk.weight AS FLOAT)) AS 'الوزن', " &
                               "pf.fabric_grade AS 'درجه القماش', pf.color AS 'اللون', pf.product_name AS 'الخامة' " &
                               "FROM packing pk " &
                               "LEFT JOIN store_finish pf ON pk.storefinishid = pf.id " &
                               "LEFT JOIN clients cs ON pk.toclient = cs.id " &
                               "WHERE pk.ref_packing IN (" & String.Join(",", refPackingList.Select(Function(r) "'" & r & "'")) & ") " &
                               "GROUP BY pk.ref_packing, cs.name, cs.code, pf.worder_id, pf.batch_no, pf.ref_no, pf.fabric_grade, pf.color, pf.product_name " &
                               "ORDER BY pk.ref_packing ASC"

        ' Execute the query and export to Excel
        ExportDataToExcel(query)
    End Sub
    Private Sub btnprint2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnprint2.Click
        Dim refPacking As String = If(Not String.IsNullOrEmpty(txtrefprint.Text), txtrefprint.Text, String.Empty)

        ' If refPacking is empty, show a message
        If String.IsNullOrEmpty(refPacking) Then
            MessageBox.Show("Please enter reference packing values.")
            Return
        End If

        ' Split the refPacking string into individual items (comma-separated)
        Dim refPackingList As String() = refPacking.Split(","c)
        refPackingList = refPackingList.Select(Function(r) r.Trim()).ToArray()


        ' Fetch unique worder_ids
        Dim uniqueWorderQuery As String = "SELECT DISTINCT pf.worder_id " & _
                                          "FROM packing pk " & _
                                          "LEFT JOIN store_finish pf ON pk.storefinishid = pf.id " & _
                                          "WHERE pk.ref_packing IN (" & String.Join(",", refPackingList.Select(Function(r) "'" & r & "'")) & ")"

        Dim worderIds As New List(Of String)()
        Using connection As New SqlConnection(sqlServerConnectionString)
            Dim command As New SqlCommand(uniqueWorderQuery, connection)
            connection.Open()
            Dim reader As SqlDataReader = command.ExecuteReader()
            While reader.Read()
                worderIds.Add(reader("worder_id").ToString())
            End While
            connection.Close()
        End Using

        ' Split worderIds into two groups (half for each table)
        Dim halfIndex As Integer = Math.Ceiling(worderIds.Count / 2.0)
        Dim table1WorderIds As List(Of String) = worderIds.Take(halfIndex).ToList()
        Dim table2WorderIds As List(Of String) = worderIds.Skip(halfIndex).ToList()

        ' Prepare the detailed data query for each worder_id
        Dim queryTemplate As String = "SELECT pf.worder_id, pk.ref_packing AS 'اذن بيع', cs.name AS 'العميل', cs.code AS 'كود العميل', " &
                                       "pf.worder_id AS 'أمر شغل', pf.batch_no AS 'رقم الرسالة', pf.ref_no AS 'اذن العميل', " &
                                       "pf.qc_roll AS 'توافق أتواب', pf.roll AS 'رقم التوب', pk.height AS 'طول التوب', pk.weight AS 'الوزن', " &
                                       "pf.fabric_grade AS 'درجه القماش', pf.color AS 'اللون', pf.product_name AS 'الخامة' " &
                                       "FROM packing pk " &
                                       "LEFT JOIN store_finish pf ON pk.storefinishid = pf.id " &
                                       "LEFT JOIN clients cs ON pk.toclient = cs.id " &
                                       "WHERE pk.ref_packing IN (" & String.Join(",", refPackingList.Select(Function(r) "'" & r & "'")) & ") " &
                                       "AND pf.worder_id = '{0}' " &
                                       "ORDER BY pk.ref_packing ASC"

        ' Export data to Excel for each worder_id
        Dim excelApp As New Microsoft.Office.Interop.Excel.Application()
        Dim workbook As Microsoft.Office.Interop.Excel.Workbook = excelApp.Workbooks.Add()
        Dim worksheet As Microsoft.Office.Interop.Excel.Worksheet = workbook.Sheets(1)
        worksheet.Name = "Packing Data"

        Dim currentRow As Integer = 1
        Dim firstTableStartColumn As Integer = 1
        Dim secondTableStartColumn As Integer = 8 ' Second table starts from column 8 (top-right of the first table)

        Using connection As New SqlConnection(sqlServerConnectionString)
            connection.Open()

            ' Write Table 1
            For Each worderId In table1WorderIds
                Dim detailedQuery As String = String.Format(queryTemplate, worderId)

                Dim dt As New DataTable()
                Dim adapter As New SqlDataAdapter(detailedQuery, connection)
                adapter.Fill(dt)

                ' Ensure that data for the current worder_id is available
                If dt.Rows.Count > 0 Then
                    ' Write worder_id as a section header for the first table
                    worksheet.Cells(currentRow, firstTableStartColumn).Value = "worder_id: " & worderId
                    worksheet.Cells(currentRow, firstTableStartColumn).Font.Bold = True
                    worksheet.Cells(currentRow, firstTableStartColumn).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                    currentRow += 1

                    ' Write the unique basic data (الخامة, رقم الرسالة, اللون) only once for each worder_id
                    Dim uniqueRow = dt.Rows(0)
                    worksheet.Cells(currentRow, firstTableStartColumn).Value = uniqueRow("الخامة")
                    worksheet.Cells(currentRow, firstTableStartColumn + 1).Value = uniqueRow("رقم الرسالة")
                    worksheet.Cells(currentRow, firstTableStartColumn + 2).Value = uniqueRow("اللون")
                    currentRow += 1

                    ' Write the column headers for the second table
                    worksheet.Cells(currentRow, firstTableStartColumn).Value = "توافق أتواب"
                    worksheet.Cells(currentRow, firstTableStartColumn + 1).Value = "رقم التوب"
                    worksheet.Cells(currentRow, firstTableStartColumn + 2).Value = "طول التوب"
                    worksheet.Cells(currentRow, firstTableStartColumn + 3).Value = "الوزن"
                    worksheet.Cells(currentRow, firstTableStartColumn + 4).Value = "درجه القماش"

                    ' Fill color for header row
                    worksheet.Range(worksheet.Cells(currentRow, firstTableStartColumn), worksheet.Cells(currentRow, firstTableStartColumn + 4)).Interior.Color = System.Drawing.Color.LightGray
                    currentRow += 1

                    Dim totalCount As Integer = 0
                    Dim totalLength As Double = 0
                    Dim totalWeight As Double = 0

                    ' Write data for the second table (detailed info for each row)
                    For Each row As DataRow In dt.Rows
                        worksheet.Cells(currentRow, firstTableStartColumn).Value = row("توافق أتواب")
                        worksheet.Cells(currentRow, firstTableStartColumn + 1).Value = row("رقم التوب")
                        worksheet.Cells(currentRow, firstTableStartColumn + 2).Value = row("طول التوب")
                        worksheet.Cells(currentRow, firstTableStartColumn + 3).Value = row("الوزن")
                        worksheet.Cells(currentRow, firstTableStartColumn + 4).Value = row("درجه القماش")

                        ' Summing total values
                        totalCount += 1
                        totalLength += Convert.ToDouble(row("طول التوب"))
                        totalWeight += Convert.ToDouble(row("الوزن"))

                        currentRow += 1
                    Next

                    ' Add Total Row at the end of the table
                    worksheet.Cells(currentRow, firstTableStartColumn).Value = "Total"
                    worksheet.Cells(currentRow, firstTableStartColumn + 1).Value = totalCount
                    worksheet.Cells(currentRow, firstTableStartColumn + 2).Value = "Sum"
                    worksheet.Cells(currentRow, firstTableStartColumn + 3).Value = totalLength
                    worksheet.Cells(currentRow, firstTableStartColumn + 4).Value = totalWeight

                    ' Fill color for total row
                    worksheet.Range(worksheet.Cells(currentRow, firstTableStartColumn), worksheet.Cells(currentRow, firstTableStartColumn + 4)).Interior.Color = System.Drawing.Color.LightYellow
                    currentRow += 1

                    ' Apply Borders to the current table (All cells)
                    Dim tableRange As Excel.Range = worksheet.Range(worksheet.Cells(currentRow - totalCount - 1, firstTableStartColumn), worksheet.Cells(currentRow - 1, firstTableStartColumn + 4))
                    tableRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous
                    tableRange.Borders.Color = System.Drawing.Color.Black
                    tableRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                    tableRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter

                    ' Adjust column widths based on the data
                    For col = firstTableStartColumn To firstTableStartColumn + 4
                        worksheet.Columns(col).AutoFit()
                    Next

                    ' Add a space between worder_id sections for the first table
                    currentRow += 1
                End If
            Next

            ' Reset the currentRow to the first row after the first table
            currentRow = 1

            ' Write Table 2, ensuring it starts at the same row, but at the right side
            For Each worderId In table2WorderIds
                Dim detailedQuery As String = String.Format(queryTemplate, worderId)

                Dim dt As New DataTable()
                Dim adapter As New SqlDataAdapter(detailedQuery, connection)
                adapter.Fill(dt)

                ' Ensure that data for the current worder_id is available
                If dt.Rows.Count > 0 Then
                    ' Write worder_id as a section header for the second table
                    worksheet.Cells(currentRow, secondTableStartColumn).Value = "worder_id: " & worderId
                    worksheet.Cells(currentRow, secondTableStartColumn).Font.Bold = True
                    worksheet.Cells(currentRow, secondTableStartColumn).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                    currentRow += 1

                    ' Write the unique basic data (الخامة, رقم الرسالة, اللون) only once for each worder_id
                    Dim uniqueRow = dt.Rows(0)
                    worksheet.Cells(currentRow, secondTableStartColumn).Value = uniqueRow("الخامة")
                    worksheet.Cells(currentRow, secondTableStartColumn + 1).Value = uniqueRow("رقم الرسالة")
                    worksheet.Cells(currentRow, secondTableStartColumn + 2).Value = uniqueRow("اللون")
                    currentRow += 1

                    ' Write the column headers for the second table
                    worksheet.Cells(currentRow, secondTableStartColumn).Value = "توافق أتواب"
                    worksheet.Cells(currentRow, secondTableStartColumn + 1).Value = "رقم التوب"
                    worksheet.Cells(currentRow, secondTableStartColumn + 2).Value = "طول التوب"
                    worksheet.Cells(currentRow, secondTableStartColumn + 3).Value = "الوزن"
                    worksheet.Cells(currentRow, secondTableStartColumn + 4).Value = "درجه القماش"

                    ' Fill color for header row
                    worksheet.Range(worksheet.Cells(currentRow, secondTableStartColumn), worksheet.Cells(currentRow, secondTableStartColumn + 4)).Interior.Color = System.Drawing.Color.LightGray
                    currentRow += 1

                    Dim totalCount As Integer = 0
                    Dim totalLength As Double = 0
                    Dim totalWeight As Double = 0

                    ' Write data for the second table (detailed info for each row)
                    For Each row As DataRow In dt.Rows
                        worksheet.Cells(currentRow, secondTableStartColumn).Value = row("توافق أتواب")
                        worksheet.Cells(currentRow, secondTableStartColumn + 1).Value = row("رقم التوب")
                        worksheet.Cells(currentRow, secondTableStartColumn + 2).Value = row("طول التوب")
                        worksheet.Cells(currentRow, secondTableStartColumn + 3).Value = row("الوزن")
                        worksheet.Cells(currentRow, secondTableStartColumn + 4).Value = row("درجه القماش")

                        ' Summing total values
                        totalCount += 1
                        totalLength += Convert.ToDouble(row("طول التوب"))
                        totalWeight += Convert.ToDouble(row("الوزن"))

                        currentRow += 1
                    Next

                    ' Add Total Row at the end of the table
                    worksheet.Cells(currentRow, secondTableStartColumn).Value = "Total"
                    worksheet.Cells(currentRow, secondTableStartColumn + 1).Value = totalCount
                    worksheet.Cells(currentRow, secondTableStartColumn + 2).Value = "Sum"
                    worksheet.Cells(currentRow, secondTableStartColumn + 3).Value = totalLength
                    worksheet.Cells(currentRow, secondTableStartColumn + 4).Value = totalWeight

                    ' Fill color for total row
                    worksheet.Range(worksheet.Cells(currentRow, secondTableStartColumn), worksheet.Cells(currentRow, secondTableStartColumn + 4)).Interior.Color = System.Drawing.Color.LightYellow
                    currentRow += 1

                    ' Apply Borders to the current table (All cells)
                    Dim tableRange As Excel.Range = worksheet.Range(worksheet.Cells(currentRow - totalCount - 1, secondTableStartColumn), worksheet.Cells(currentRow - 1, secondTableStartColumn + 4))
                    tableRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous
                    tableRange.Borders.Color = System.Drawing.Color.Black
                    tableRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                    tableRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter

                    ' Adjust column widths based on the data
                    For col = secondTableStartColumn To secondTableStartColumn + 4
                        worksheet.Columns(col).AutoFit()
                    Next

                End If
            Next
            ' Start adding the summary table after completing the data for both tables
            Dim summaryStartRow As Integer = currentRow + 2 ' Leave some space below the data
            worksheet.Cells(summaryStartRow, 1).Value = "Summary Table"
            worksheet.Cells(summaryStartRow, 2).Value = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt")
            worksheet.Cells(summaryStartRow, 1).Font.Bold = True
            worksheet.Cells(summaryStartRow, 1).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter

            ' Write the headers for the summary table
            Dim summaryHeaders As String() = {"اذن بيع", "العميل", "كود العميل", "أمر شغل", "رقم الرسالة", "اذن العميل", "الأتواب", "إجمالى أمتار", "الوزن", "درجه القماش", "اللون", "الخامة"}
            For colIndex As Integer = 1 To summaryHeaders.Length
                worksheet.Cells(summaryStartRow + 1, colIndex).Value = summaryHeaders(colIndex - 1)
                worksheet.Cells(summaryStartRow + 1, colIndex).Interior.Color = System.Drawing.Color.LightGray
                worksheet.Cells(summaryStartRow + 1, colIndex).Font.Bold = True
            Next

            ' Fetch summary data and populate the summary table
            Dim summaryQuery As String = "SELECT pk.ref_packing AS 'اذن بيع', " &
                               "cs.name AS 'العميل', cs.code AS 'كود العميل', " &
                               "pf.worder_id AS 'أمر شغل', pf.batch_no AS 'رقم الرسالة', pf.ref_no AS 'اذن العميل', " &
                               "COUNT(pf.roll) AS 'الأتواب', " &
                               "SUM(CAST(pk.height AS FLOAT)) AS 'إجمالى أمتار', " &
                               "SUM(CAST(pk.weight AS FLOAT)) AS 'الوزن', " &
                               "pf.fabric_grade AS 'درجه القماش', pf.color AS 'اللون', pf.product_name AS 'الخامة' " &
                               "FROM packing pk " &
                               "LEFT JOIN store_finish pf ON pk.storefinishid = pf.id " &
                               "LEFT JOIN clients cs ON pk.toclient = cs.id " &
                               "WHERE pk.ref_packing IN (" & String.Join(",", refPackingList.Select(Function(r) "'" & r & "'")) & ") " &
                               "GROUP BY pk.ref_packing, cs.name, cs.code, pf.worder_id, pf.batch_no, pf.ref_no, pf.fabric_grade, pf.color, pf.product_name " &
                               "ORDER BY pk.ref_packing ASC"

            Dim summaryTable As New DataTable()
            Using localConnection As New SqlConnection(sqlServerConnectionString)

                Dim adapter As New SqlDataAdapter(summaryQuery, connection)
                adapter.Fill(summaryTable)
            End Using

            ' Write the summary data to Excel
            Dim summaryRow As Integer = summaryStartRow + 2
            Dim totalsRolls As Integer = 0
            Dim totalsHeight As Double = 0.0
            Dim totalsWeight As Double = 0.0

            For Each row As DataRow In summaryTable.Rows
                worksheet.Cells(summaryRow, 1).Value = row("اذن بيع")
                worksheet.Cells(summaryRow, 2).Value = row("العميل")
                worksheet.Cells(summaryRow, 3).Value = row("كود العميل")
                worksheet.Cells(summaryRow, 4).Value = row("أمر شغل")
                worksheet.Cells(summaryRow, 5).Value = row("رقم الرسالة")
                worksheet.Cells(summaryRow, 6).Value = row("اذن العميل")
                worksheet.Cells(summaryRow, 7).Value = row("الأتواب")
                worksheet.Cells(summaryRow, 8).Value = row("إجمالى أمتار")
                worksheet.Cells(summaryRow, 9).Value = row("الوزن")
                worksheet.Cells(summaryRow, 10).Value = row("درجه القماش")
                worksheet.Cells(summaryRow, 11).Value = row("اللون")
                worksheet.Cells(summaryRow, 12).Value = row("الخامة")

                ' Accumulate totals
                totalsRolls += Convert.ToInt32(row("الأتواب"))
                totalsHeight += Convert.ToDouble(row("إجمالى أمتار"))
                totalsWeight += Convert.ToDouble(row("الوزن"))

                summaryRow += 1
            Next

            ' Add totals row
            worksheet.Cells(summaryRow, 1).Value = "الإجمالى"
            worksheet.Cells(summaryRow, 1).Font.Bold = True
            worksheet.Cells(summaryRow, 7).Value = totalsRolls
            worksheet.Cells(summaryRow, 8).Value = totalsHeight
            worksheet.Cells(summaryRow, 9).Value = totalsWeight

            ' Style the totals row
            Dim totalRange As Excel.Range = worksheet.Range(worksheet.Cells(summaryRow, 1), worksheet.Cells(summaryRow, 12))
            totalRange.Interior.Color = System.Drawing.Color.LightYellow
            totalRange.Font.Bold = True
            totalRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter

            ' Apply borders to the entire table including totals
            Dim summaryRange As Excel.Range = worksheet.Range(worksheet.Cells(summaryStartRow + 1, 1), worksheet.Cells(summaryRow, 12))
            summaryRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous
            summaryRange.Borders.Color = System.Drawing.Color.Black
            summaryRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
            summaryRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter

            ' Adjust column widths
            For colIndex As Integer = 1 To 12
                worksheet.Columns(colIndex).AutoFit()
            Next
            ' Define the row where the signatures will be added
            Dim signaturesRow As Integer = summaryRow + 2 ' Leave some space below the table

            ' Add "مدير المخازن" to the right
            worksheet.Cells(signaturesRow, 1).Value = "مدير المخازن"
            worksheet.Cells(signaturesRow, 1).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft
            worksheet.Cells(signaturesRow, 1).Font.Bold = True

            ' Add "المستلم" in the center
            Dim centerColumn As Integer = Math.Ceiling(12 / 2.0) ' Assuming 12 columns
            worksheet.Cells(signaturesRow, centerColumn).Value = "المستلم"
            worksheet.Cells(signaturesRow, centerColumn).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
            worksheet.Cells(signaturesRow, centerColumn).Font.Bold = True

            ' Add "أمين المخزن" to the left
            worksheet.Cells(signaturesRow, 12).Value = "أمين المخزن"
            worksheet.Cells(signaturesRow, 12).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight
            worksheet.Cells(signaturesRow, 12).Font.Bold = True

            ' Merge cells for better alignment if required
            worksheet.Range(worksheet.Cells(signaturesRow, 1), worksheet.Cells(signaturesRow, centerColumn - 1)).Merge()
            worksheet.Range(worksheet.Cells(signaturesRow, centerColumn + 1), worksheet.Cells(signaturesRow, 11)).Merge()

            ' Optional: Adjust the row height for better visibility
            worksheet.Rows(signaturesRow).RowHeight = 25


            connection.Close()
        End Using

        ' Show Excel
        excelApp.Visible = True
    End Sub

    Private Sub PopulateSheetWithData(ByVal worksheet As Microsoft.Office.Interop.Excel.Worksheet, ByVal dt As DataTable, ByVal startRow As Integer)
        Dim startColumn As Integer = 1

        ' Insert "تاريخ الشحن" and current date and time with AM/PM
        worksheet.Cells(startRow, startColumn).Value = "تاريخ الشحن"
        worksheet.Cells(startRow, startColumn + 1).Value = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt")
        With worksheet.Cells(startRow, startColumn)
            .Font.Bold = True
            .Font.Size = 14
            .HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
            .VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter
            .Interior.Color = RGB(191, 191, 191) ' Light gray background
        End With

        startRow += 1 ' Move to the next row for headers

        ' Set column headers
        For i As Integer = 0 To dt.Columns.Count - 1
            Dim headerCell = worksheet.Cells(startRow, startColumn + i)
            headerCell.Value = dt.Columns(i).ColumnName
            With headerCell
                .Font.Bold = True
                .Font.Size = 14
                .HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
                .VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter
                .Interior.Color = RGB(191, 191, 191) ' Light gray background
            End With
        Next
        startRow += 1 ' Move to the next row for data

        ' Populate data rows
        For i As Integer = 0 To dt.Rows.Count - 1
            For j As Integer = 0 To dt.Columns.Count - 1
                Dim dataCell = worksheet.Cells(startRow + i, startColumn + j)
                dataCell.Value = If(dt.Rows(i)(j) IsNot DBNull.Value, dt.Rows(i)(j).ToString(), String.Empty)
                With dataCell
                    .HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
                    .VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter
                    .Font.Size = 14
                End With
            Next
        Next

        ' Add "TOTAL" row for specific columns (G, H, I)
        Dim totalRow As Integer = startRow + dt.Rows.Count
        worksheet.Cells(totalRow, startColumn + 5).Value = "TOTAL" ' Column F for label

        ' Calculate totals for columns G, H, and I
        worksheet.Cells(totalRow, startColumn + 6).Formula = "=SUM(G" & startRow & ":G" & totalRow - 1 & ")"
        worksheet.Cells(totalRow, startColumn + 7).Formula = "=SUM(H" & startRow & ":H" & totalRow - 1 & ")"
        worksheet.Cells(totalRow, startColumn + 8).Formula = "=SUM(I" & startRow & ":I" & totalRow - 1 & ")"

        ' Format the "TOTAL" row
        Dim totalRange As Microsoft.Office.Interop.Excel.Range = worksheet.Range(worksheet.Cells(totalRow, startColumn + 5), worksheet.Cells(totalRow, startColumn + 8))
        With totalRange
            .Font.Bold = True
            .Font.Size = 14
            .HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
            .Interior.Color = RGB(191, 191, 191) ' Light gray background
        End With

        ' Highlight the "TOTAL" label
        With worksheet.Cells(totalRow, startColumn + 5)
            .Font.Bold = True
            .Font.Size = 16
            .Font.Color = RGB(255, 0, 0) ' Red color
        End With

        ' Add borders to all populated cells
        Dim lastRow As Integer = totalRow
        Dim lastColumn As Integer = startColumn + dt.Columns.Count - 1
        Dim dataRange As Microsoft.Office.Interop.Excel.Range = worksheet.Range(worksheet.Cells(startRow - 1, startColumn), worksheet.Cells(lastRow, lastColumn))
        dataRange.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin

        ' Auto-fit columns for better readability
        worksheet.Columns.AutoFit()
    End Sub
    Private Sub ProcessAndPopulateDetailedData(ByVal worksheet As Microsoft.Office.Interop.Excel.Worksheet, ByVal dt As DataTable, ByVal startRow As Integer)
        Dim groupedData = dt.AsEnumerable().GroupBy(Function(row) row("worder_id").ToString()).ToList()
        Dim currentRow As Integer = startRow
        Dim startColumn As Integer = 1

        ' Set headers
        For i As Integer = 0 To dt.Columns.Count - 1
            Dim headerCell = worksheet.Cells(currentRow, startColumn + i)
            headerCell.Value = dt.Columns(i).ColumnName
            headerCell.Font.Bold = True
            headerCell.Font.Size = 14
            headerCell.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
            headerCell.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter
            headerCell.Interior.Color = RGB(191, 191, 191) ' Light gray background
        Next
        currentRow += 1

        ' Populate grouped data
        For Each group In groupedData
            Dim subtotalHeight As Double = group.Sum(Function(row) CDbl(row("طول التوب")))
            Dim subtotalWeight As Double = group.Sum(Function(row) CDbl(row("الوزن")))
            Dim rollCount As Integer = group.Count()
            Dim isFirstRow As Boolean = True

            ' Process each row in the group
            For Each row As DataRow In group
                For j As Integer = 0 To dt.Columns.Count - 1
                    Dim dataCell = worksheet.Cells(currentRow, startColumn + j)
                    dataCell.Value = If(row(j) IsNot DBNull.Value, row(j).ToString(), String.Empty)
                    dataCell.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
                    dataCell.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter
                    dataCell.Font.Size = 14
                Next
                currentRow += 1
            Next

            ' Insert subtotal row
            worksheet.Cells(currentRow, startColumn + 8).Value = "Subtotal"
            worksheet.Cells(currentRow, startColumn + 9).Value = subtotalHeight
            worksheet.Cells(currentRow, startColumn + 10).Value = subtotalWeight
            worksheet.Cells(currentRow, startColumn + 11).Value = rollCount

            ' Format subtotal row
            With worksheet.Range(worksheet.Cells(currentRow, startColumn + 8), worksheet.Cells(currentRow, startColumn + 11))
                .Font.Bold = True
                .Font.Size = 14
                .HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
                .Interior.Color = RGB(217, 234, 211) ' Light green background
            End With
            currentRow += 2 ' Leave space before the next group
        Next

        ' Auto-fit columns for better readability
        worksheet.Columns.AutoFit()
    End Sub




    Private Sub ExportDataToExcel(ByVal query As String)
        Using connection As New SqlConnection(sqlServerConnectionString)
            Dim adapter As New SqlDataAdapter(query, connection)
            Dim dt As New DataTable()

            Try
                connection.Open()
                adapter.Fill(dt)

                If dt.Rows.Count > 0 Then
                    ' Export to Excel
                    ExportTotoExcel(dt) ' Pass the DataTable directly
                Else
                    MessageBox.Show("No data found for the selected reference packing.")
                End If
            Catch ex As Exception
                MessageBox.Show("An error occurred while fetching data: " & ex.Message)
            End Try
        End Using
    End Sub
    Private Sub ExportTotoExcel(ByVal dt As DataTable)
        Try
            If dt.Rows.Count = 0 Then
                MessageBox.Show("No data to export.")
                Return
            End If

            ' Create a new Excel application instance
            Dim excelApp As New Microsoft.Office.Interop.Excel.Application()
            Dim workbook As Microsoft.Office.Interop.Excel.Workbook = excelApp.Workbooks.Add()
            Dim worksheet As Microsoft.Office.Interop.Excel.Worksheet = CType(workbook.Sheets(1), Microsoft.Office.Interop.Excel.Worksheet)

            ' Set the current date and time in the header
            worksheet.Cells(1, 1) = " " & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            worksheet.Cells(1, 1).Font.Bold = True
            worksheet.Cells(1, 1).Font.Size = 16
            worksheet.Cells(1, 1).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
            worksheet.Range("A1:F1").Merge() ' Merge cells for the header

            ' Set column headers
            For i As Integer = 0 To dt.Columns.Count - 1
                worksheet.Cells(2, i + 1) = dt.Columns(i).ColumnName
                worksheet.Cells(2, i + 1).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
                worksheet.Cells(2, i + 1).Font.Bold = True
                worksheet.Cells(2, i + 1).Font.Size = 16
                worksheet.Cells(2, i + 1).Interior.Color = RGB(191, 191, 191) ' Header color: White Background 1, Darker 25%
            Next

            ' Populate data and format cells with borders
            For i As Integer = 0 To dt.Rows.Count - 1
                For j As Integer = 0 To dt.Columns.Count - 1
                    worksheet.Cells(i + 3, j + 1) = If(dt.Rows(i)(j) IsNot DBNull.Value, dt.Rows(i)(j).ToString(), String.Empty)
                    worksheet.Cells(i + 3, j + 1).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
                    worksheet.Cells(i + 3, j + 1).Font.Bold = True
                    worksheet.Cells(i + 3, j + 1).Font.Size = 16
                    worksheet.Cells(i + 3, j + 1).Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin
                Next
            Next

            ' Calculate the total row
            Dim lastRow As Integer = dt.Rows.Count + 3
            worksheet.Cells(lastRow, 1).Value = "Total"
            worksheet.Cells(lastRow, 1).Font.Size = 18
            worksheet.Cells(lastRow, 1).Font.Bold = True
            worksheet.Cells(lastRow, 1).Interior.Color = RGB(184, 204, 228) ' Blue accent, lighter 60%
            worksheet.Range(worksheet.Cells(lastRow, 1), worksheet.Cells(lastRow, 6)).Merge()

            ' Calculate and write totals in specified columns
            worksheet.Cells(lastRow, 7).Formula = "=SUM(G3:G" & (lastRow - 1) & ")"
            worksheet.Cells(lastRow, 8).Formula = "=SUM(H3:H" & (lastRow - 1) & ")"
            worksheet.Cells(lastRow, 9).Formula = "=SUM(I3:I" & (lastRow - 1) & ")"

            ' Set formatting for total row columns
            For col As Integer = 7 To 9
                worksheet.Cells(lastRow, col).Font.Size = 18
                worksheet.Cells(lastRow, col).Font.Bold = True
                worksheet.Cells(lastRow, col).Interior.Color = RGB(184, 204, 228) ' Blue accent, lighter 60%
                worksheet.Cells(lastRow, col).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
            Next

            ' Set borders for the entire data range
            Dim dataRange As Microsoft.Office.Interop.Excel.Range = worksheet.Range("A2", "I" & lastRow)
            dataRange.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin

            ' Write footer information
            worksheet.Cells(lastRow + 2, 1).Value = "مدير المخازن"
            worksheet.Cells(lastRow + 2, 1).Font.Size = 16
            worksheet.Cells(lastRow + 2, 1).Font.Bold = True

            worksheet.Cells(lastRow + 2, dt.Columns.Count \ 2).Value = "المستلم"
            worksheet.Cells(lastRow + 2, dt.Columns.Count \ 2).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
            worksheet.Cells(lastRow + 2, dt.Columns.Count \ 2).Font.Size = 16
            worksheet.Cells(lastRow + 2, dt.Columns.Count \ 2).Font.Bold = True

            worksheet.Cells(lastRow + 2, dt.Columns.Count).Value = "أمين المخزن"
            worksheet.Cells(lastRow + 2, dt.Columns.Count).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
            worksheet.Cells(lastRow + 2, dt.Columns.Count).Font.Size = 16
            worksheet.Cells(lastRow + 2, dt.Columns.Count).Font.Bold = True

            ' Adjust column widths automatically
            worksheet.Columns.AutoFit()

            ' Make Excel visible and save changes
            excelApp.Visible = True
        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message)
        End Try
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
    ' Print Packing as HTML
    ' Print Packing as HTML
    Private Sub btnPrintPacking_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnprintpacking.Click
        Dim refPacking As String = If(Not String.IsNullOrEmpty(txtrefprint.Text), txtrefprint.Text, String.Empty)

        ' If refPacking is empty, show a message
        If String.IsNullOrEmpty(refPacking) Then
            MessageBox.Show("Please enter reference packing values.")
            Return
        End If

        ' Split the refPacking string into individual items (comma-separated)
        Dim refPackingList As String() = refPacking.Split(","c)
        refPackingList = refPackingList.Select(Function(r) r.Trim()).ToArray()

        ' Prepare the detailed data query for each worder_id
        Dim queryTemplate As String = "SELECT pf.worder_id, pk.ref_packing AS 'اذن بيع', cs.name AS 'العميل', cs.code AS 'كود العميل', " &
                       "pf.worder_id AS 'أمر شغل', pf.batch_no AS 'رقم الرسالة', pf.ref_no AS 'اذن العميل', " &
                       "pf.qc_roll AS 'توافق أتواب', pf.roll AS 'رقم التوب', pk.height AS 'طول التوب', pk.weight AS 'الوزن', " &
                       "pf.fabric_grade AS 'درجه القماش', pf.color AS 'اللون', pf.product_name AS 'الخامة' " &
                       "FROM packing pk " &
                       "LEFT JOIN store_finish pf ON pk.storefinishid = pf.id " &
                       "LEFT JOIN clients cs ON pk.toclient = cs.id " &
                       "WHERE pk.ref_packing IN (" & String.Join(",", refPackingList.Select(Function(r) "'" & r & "'")) & ") " &
                       "AND pf.worder_id = '{0}' " &
                       "ORDER BY pk.ref_packing ASC"

        ' Fetch summary data and populate the summary table
        Dim summaryQuery1 As String = "SELECT pk.ref_packing AS 'اذن بيع', " &
               "cs.name AS 'العميل', cs.code AS 'كود العميل', " &
               "CONVERT(VARCHAR, MAX(pk.date), 111) AS 'تاريخ الشحن' " &
               "FROM packing pk " &
               "LEFT JOIN store_finish pf ON pk.storefinishid = pf.id " &
               "LEFT JOIN clients cs ON pk.toclient = cs.id " &
               "WHERE pk.ref_packing IN (" & String.Join(",", refPackingList.Select(Function(r) "'" & r & "'")) & ") " &
               "GROUP BY pk.ref_packing, cs.name, cs.code " &
               "ORDER BY pk.ref_packing ASC"

        Dim summaryQuery2 As String = "SELECT pf.worder_id AS 'أمر شغل', pf.batch_no AS 'رقم الرسالة', pf.ref_no AS 'اذن العميل', " &
               "COUNT(CASE WHEN pf.fabric_grade <> 5 THEN pf.roll ELSE NULL END) AS 'الأتواب', " &
               "SUM(CAST(pk.height AS FLOAT)) AS 'إجمالى أمتار', " &
               "SUM(CAST(pk.weight AS FLOAT)) AS 'الوزن', " &
               "pf.fabric_grade AS 'درجه القماش', pf.color AS 'اللون', pf.product_name AS 'الخامة' " &
               "FROM packing pk " &
               "LEFT JOIN store_finish pf ON pk.storefinishid = pf.id " &
               "LEFT JOIN clients cs ON pk.toclient = cs.id " &
               "WHERE pk.ref_packing IN (" & String.Join(",", refPackingList.Select(Function(r) "'" & r & "'")) & ") " &
               "GROUP BY pf.worder_id, pf.batch_no, pf.ref_no, pf.fabric_grade, pf.color, pf.product_name " &
               "ORDER BY pf.worder_id ASC"

        ' Generate HTML content
        Dim htmlContent As New StringBuilder()
        htmlContent.Append("<html><head><style>")
        htmlContent.Append("table { width: 100%; border-collapse: collapse; font-family: Arial, sans-serif; page-break-inside: avoid; }")
        htmlContent.Append("th, td { border: 1px solid black; padding: 8px; text-align: center; font-size: 16px; }")
        htmlContent.Append("th { background-color: #d3d3d3; font-weight: bold; color: black; }") ' Light gray background for all table headers
        htmlContent.Append(".table-container { display: flex; justify-content: space-between; }")
        htmlContent.Append(".total-row { background-color: #FFFF99; font-weight: bold; }") ' Light yellow background for total row
        htmlContent.Append(".thick-border { border: 2px solid black; }") ' Thick border for table
        htmlContent.Append("@media print { body { zoom: 75%; } }") ' Set print scale to 75%
        ' Add CSS for summary table font size and bold
        htmlContent.Append(".summary-table th, .summary-table td { font-size: 20px; font-weight: bold; }") ' Increased font size and bold
        htmlContent.Append("</style></head><body>")

        ' Add detailed data for each worder_id
        Using connection As New SqlConnection(sqlServerConnectionString)
            connection.Open()

            ' Fetch summary data
            Dim summaryDt As New DataTable()
            Using adapter As New SqlDataAdapter(summaryQuery1, connection)
                adapter.Fill(summaryDt)
            End Using

            ' Assume the first row of the summary data contains the common information
            If summaryDt.Rows.Count > 0 Then
                Dim firstRow = summaryDt.Rows(0)

                ' Add the small table with shipping date, sale auth, client code, client name
                htmlContent.Append("<table>")
                htmlContent.Append("<tr><th>تاريخ الشحن</th><th>اذن البيع</th><th>كود العميل</th><th>اسم العميل</th></tr>")
                htmlContent.Append("<tr><td>" & firstRow("تاريخ الشحن") & "</td><td>" & firstRow("اذن بيع") & "</td><td>" & firstRow("كود العميل") & "</td><td>" & firstRow("العميل") & "</td></tr>")
                htmlContent.Append("</table><br/>")
            End If

            ' Fetch unique worder_ids
            Dim uniqueWorderQuery As String = "SELECT DISTINCT pf.worder_id " &
                              "FROM packing pk " &
                              "LEFT JOIN store_finish pf ON pk.storefinishid = pf.id " &
                              "WHERE pk.ref_packing IN (" & String.Join(",", refPackingList.Select(Function(r) "'" & r & "'")) & ")"

            Dim worderIds As New List(Of String)()
            Using command As New SqlCommand(uniqueWorderQuery, connection)
                Using reader As SqlDataReader = command.ExecuteReader()
                    While reader.Read()
                        worderIds.Add(reader("worder_id").ToString())
                    End While
                End Using
            End Using

            Dim currentTable As Integer = 1

            ' Add detailed data for each worder_id
            For Each worderId In worderIds
                Dim detailedQuery As String = String.Format(queryTemplate, worderId)
                Dim dt As New DataTable()
                Using adapter As New SqlDataAdapter(detailedQuery, connection)
                    adapter.Fill(dt)
                End Using

                Dim rowCount As Integer = 0

                While rowCount < dt.Rows.Count
                    If currentTable > 3 Then
                        htmlContent.Append("</div><br/>")
                        currentTable = 1
                    End If

                    If currentTable = 1 Then
                        htmlContent.Append("<div class='table-container'>")
                    End If

                    htmlContent.Append("<table class='thick-border'>")

                    ' Add worder_id, batch_no, product_name, color
                    htmlContent.Append("<tr><th colspan='4'>أمر شغل: " & worderId & "</th></tr>")
                    htmlContent.Append("<tr>")
                    htmlContent.Append("<td style='border: 1px solid black;'>رقم الرسالة: " & If(rowCount < dt.Rows.Count, dt.Rows(rowCount)("رقم الرسالة").ToString(), "") & "</td>")
                    htmlContent.Append("<td style='border: 1px solid black;' colspan='2'>الخامة: " & If(rowCount < dt.Rows.Count, dt.Rows(rowCount)("الخامة").ToString(), "") & "</td>")
                    htmlContent.Append("<td style='border: 1px solid black;'>اللون: " & If(rowCount < dt.Rows.Count, dt.Rows(rowCount)("اللون").ToString(), "") & "</td>")
                    htmlContent.Append("</tr>")

                    htmlContent.Append("<tr>")
                    htmlContent.Append("<th>رقم التوب</th><th>طول التوب</th><th>الوزن</th><th>درجه القماش</th>")
                    htmlContent.Append("</tr>")

                    Dim totalWeight As Double = 0
                    Dim totalHeight As Double = 0
                    Dim totalRolls As Integer = 0

                    For j As Integer = rowCount To Math.Min(rowCount + 9, dt.Rows.Count - 1)
                        htmlContent.Append("<tr>")
                        htmlContent.Append("<td>" & If(j < dt.Rows.Count, dt.Rows(j)("رقم التوب").ToString(), "") & "</td>")
                        htmlContent.Append("<td>" & If(j < dt.Rows.Count, dt.Rows(j)("طول التوب").ToString(), "") & "</td>")
                        htmlContent.Append("<td>" & If(j < dt.Rows.Count, dt.Rows(j)("الوزن").ToString(), "") & "</td>")
                        htmlContent.Append("<td>" & If(j < dt.Rows.Count, dt.Rows(j)("درجه القماش").ToString(), "") & "</td>")
                        htmlContent.Append("</tr>")

                        If j < dt.Rows.Count Then
                            totalHeight += Convert.ToDouble(dt.Rows(j)("طول التوب"))
                            totalWeight += Convert.ToDouble(dt.Rows(j)("الوزن"))
                            totalRolls += 1
                        End If
                    Next

                    ' Fill remaining rows if less than 10
                    For k As Integer = totalRolls To 9
                        htmlContent.Append("<tr>")
                        htmlContent.Append("<td></td><td></td><td></td><td></td>")
                        htmlContent.Append("</tr>")
                    Next

                    htmlContent.Append("<tr class='total-row'>")
                    htmlContent.Append("<td>" & totalRolls & "</td><td>" & totalHeight & "</td><td>" & totalWeight & "</td><td>الإجمالى</td>")
                    htmlContent.Append("</tr>")
                    htmlContent.Append("</table>")

                    rowCount += 10
                    currentTable += 1
                End While
            Next

            ' Close the last table container if not closed
            If currentTable > 1 Then
                htmlContent.Append("</div><br/>")
            End If

            ' Add a page break before the summary tables
            htmlContent.Append("<div style='page-break-before: always;'></div>")

            ' Fetch summary data for the first table
            Dim summaryDt1 As New DataTable()
            Using adapter As New SqlDataAdapter(summaryQuery1, connection)
                adapter.Fill(summaryDt1)
            End Using

            If summaryDt1.Rows.Count > 0 Then
                htmlContent.Append("<h2>Summary</h2>")
                htmlContent.Append("<table class='summary-table'><tr>")
                For Each column As DataColumn In summaryDt1.Columns
                    htmlContent.Append("<th>" & column.ColumnName & "</th>")
                Next
                htmlContent.Append("</tr>")

                For Each row As DataRow In summaryDt1.Rows
                    htmlContent.Append("<tr>")
                    For Each column As DataColumn In summaryDt1.Columns
                        htmlContent.Append("<td>" & row(column).ToString() & "</td>")
                    Next
                    htmlContent.Append("</tr>")
                Next

                htmlContent.Append("</table><br/>")
            End If

            ' Fetch summary data for the second table
            Dim summaryDt2 As New DataTable()
            Using adapter As New SqlDataAdapter(summaryQuery2, connection)
                adapter.Fill(summaryDt2)
            End Using

            If summaryDt2.Rows.Count > 0 Then
                htmlContent.Append("<h2></h2>")
                htmlContent.Append("<table class='summary-table'><tr>")
                For Each column As DataColumn In summaryDt2.Columns
                    htmlContent.Append("<th>" & column.ColumnName & "</th>")
                Next
                htmlContent.Append("</tr>")

                Dim totalRolls As Integer = 0
                Dim totalHeight As Double = 0
                Dim totalWeight As Double = 0

                For Each row As DataRow In summaryDt2.Rows
                    htmlContent.Append("<tr>")
                    For Each column As DataColumn In summaryDt2.Columns
                        htmlContent.Append("<td>" & row(column).ToString() & "</td>")
                    Next
                    htmlContent.Append("</tr>")

                    totalRolls += Convert.ToInt32(row("الأتواب"))
                    totalHeight += Convert.ToDouble(row("إجمالى أمتار"))
                    totalWeight += Convert.ToDouble(row("الوزن"))
                Next

                ' Add total row
                htmlContent.Append("<tr class='total-row'>")
                htmlContent.Append("<td colspan='6'>Total</td>")
                htmlContent.Append("<td>" & totalRolls & "</td>")
                htmlContent.Append("<td>" & totalHeight & "</td>")
                htmlContent.Append("<td>" & totalWeight & "</td>")
                htmlContent.Append("<td colspan='3'></td>")
                htmlContent.Append("</tr>")

                htmlContent.Append("</table>")
            End If

            connection.Close()
        End Using

        htmlContent.Append("</body></html>")

        ' Save HTML content to a temporary file
        Dim tempFilePath As String = Path.GetTempFileName() & ".html"
        File.WriteAllText(tempFilePath, htmlContent.ToString())

        ' Open the temporary file in the default web browser
        Process.Start(tempFilePath)
    End Sub


    Private Sub btnprinttotal_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnprinttotal.Click
        Dim refPacking As String = If(Not String.IsNullOrEmpty(txtrefprint.Text), txtrefprint.Text, String.Empty)

        ' If refPacking is empty, show a message
        If String.IsNullOrEmpty(refPacking) Then
            MessageBox.Show("Please enter reference packing values.")
            Return
        End If

        ' Split the refPacking string into individual items (comma-separated)
        Dim refPackingList As String() = refPacking.Split(","c)
        refPackingList = refPackingList.Select(Function(r) r.Trim()).ToArray()

        ' Fetch summary data and populate the summary table
        Dim summaryQuery As String = "SELECT pk.ref_packing AS 'اذن بيع', " &
                   "cs.name AS 'العميل', cs.code AS 'كود العميل', " &
                   "CONVERT(VARCHAR, MAX(pk.date), 111) AS 'تاريخ الشحن' " & ' Fetch only the date part
                   "FROM packing pk " &
                   "LEFT JOIN store_finish pf ON pk.storefinishid = pf.id " &
                   "LEFT JOIN clients cs ON pk.toclient = cs.id " &
                   "WHERE pk.ref_packing IN (" & String.Join(",", refPackingList.Select(Function(r) "'" & r & "'")) & ") " &
                   "GROUP BY pk.ref_packing, cs.name, cs.code " &
                   "ORDER BY pk.ref_packing ASC"

        ' Fetch detailed data
        Dim detailedQuery As String = "SELECT pf.worder_id AS 'أمر شغل', pf.batch_no AS 'رقم الرسالة', pf.ref_no AS 'اذن العميل', " &
                   "COUNT(CASE WHEN pf.fabric_grade <> 5 THEN pf.roll ELSE NULL END) AS 'الأتواب', " &
                   "SUM(CAST(pk.height AS FLOAT)) AS 'إجمالى أمتار', " &
                   "SUM(CAST(pk.weight AS FLOAT)) AS 'الوزن', " &
                   "pf.fabric_grade AS 'درجه القماش', pf.color AS 'اللون', pf.product_name AS 'الخامة' " &
                   "FROM packing pk " &
                   "LEFT JOIN store_finish pf ON pk.storefinishid = pf.id " &
                   "LEFT JOIN clients cs ON pk.toclient = cs.id " &
                   "WHERE pk.ref_packing IN (" & String.Join(",", refPackingList.Select(Function(r) "'" & r & "'")) & ") " &
                   "GROUP BY pf.worder_id, pf.batch_no, pf.ref_no, pf.fabric_grade, pf.color, pf.product_name " &
                   "ORDER BY pf.worder_id ASC"

        ' Generate HTML content
        Dim htmlContent As New StringBuilder()
        htmlContent.Append("<html><head><style>")
        htmlContent.Append("table { width: 100%; border-collapse: collapse; font-family: Arial, sans-serif; page-break-inside: avoid; }")
        htmlContent.Append("th, td { border: 1px solid black; padding: 8px; text-align: center; font-size: 18px; font-weight: bold; }") ' Increased font size and bold
        htmlContent.Append("th { background-color: #d3d3d3; color: black; }") ' Light gray background for all table headers
        htmlContent.Append(".total-row { background-color: #FFFF99; font-weight: bold; }") ' Light yellow background for total row
        htmlContent.Append("@media print { body { zoom: 75%; } }") ' Set print scale to 75%
        htmlContent.Append("</style></head><body>")

        ' Add summary data to the document
        Using connection As New SqlConnection(sqlServerConnectionString)
            connection.Open()

            ' Fetch summary data
            Dim summaryDt As New DataTable()
            Using adapter As New SqlDataAdapter(summaryQuery, connection)
                adapter.Fill(summaryDt)
            End Using

            ' Add summary data to the document
            If summaryDt.Rows.Count > 0 Then
                htmlContent.Append("<h2>Summary</h2>")
                htmlContent.Append("<table class='summary-table'><tr>")
                htmlContent.Append("<th>اذن بيع</th>")
                htmlContent.Append("<th>العميل</th>")
                htmlContent.Append("<th>كود العميل</th>")
                htmlContent.Append("<th>تاريخ الشحن</th>")
                htmlContent.Append("</tr>")

                For Each row As DataRow In summaryDt.Rows
                    htmlContent.Append("<tr>")
                    htmlContent.Append("<td>" & row("اذن بيع").ToString() & "</td>")
                    htmlContent.Append("<td>" & row("العميل").ToString() & "</td>")
                    htmlContent.Append("<td>" & row("كود العميل").ToString() & "</td>")
                    htmlContent.Append("<td>" & row("تاريخ الشحن").ToString() & "</td>")
                    htmlContent.Append("</tr>")
                Next

                htmlContent.Append("</table>")
            End If

            ' Fetch detailed data
            Dim detailedDt As New DataTable()
            Using adapter As New SqlDataAdapter(detailedQuery, connection)
                adapter.Fill(detailedDt)
            End Using

            ' Add detailed data to the document
            If detailedDt.Rows.Count > 0 Then
                htmlContent.Append("<h2>Detailed Data</h2>")
                htmlContent.Append("<table class='detailed-table'><tr>")
                For Each column As DataColumn In detailedDt.Columns
                    htmlContent.Append("<th>" & column.ColumnName & "</th>")
                Next
                htmlContent.Append("</tr>")

                Dim totalRolls As Integer = 0
                Dim totalHeight As Double = 0
                Dim totalWeight As Double = 0

                For Each row As DataRow In detailedDt.Rows
                    htmlContent.Append("<tr>")
                    For Each column As DataColumn In detailedDt.Columns
                        htmlContent.Append("<td>" & row(column).ToString() & "</td>")
                    Next
                    htmlContent.Append("</tr>")

                    totalRolls += Convert.ToInt32(row("الأتواب"))
                    totalHeight += Convert.ToDouble(row("إجمالى أمتار"))
                    totalWeight += Convert.ToDouble(row("الوزن"))
                Next

                ' Add total row
                htmlContent.Append("<tr class='total-row'>")
                htmlContent.Append("<td colspan='3'>Total</td>")
                htmlContent.Append("<td>" & totalRolls & "</td>")
                htmlContent.Append("<td>" & totalHeight & "</td>")
                htmlContent.Append("<td>" & totalWeight & "</td>")
                htmlContent.Append("<td colspan='3'></td>")
                htmlContent.Append("</tr>")

                htmlContent.Append("</table>")
            End If

            connection.Close()
        End Using

        htmlContent.Append("</body></html>")

        ' Save HTML content to a temporary file
        Dim tempFilePath As String = Path.GetTempFileName() & ".html"
        File.WriteAllText(tempFilePath, htmlContent.ToString())

        ' Open the temporary file in the default web browser
        Process.Start(tempFilePath)
    End Sub









End Class
