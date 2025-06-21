Imports System.Data.SqlClient
Imports System.Drawing
Imports Microsoft.Office.Interop
Imports Excel = Microsoft.Office.Interop.Excel

Public Class finishdisbursmentviewform
    Private sqlServerConnectionString As String = "Server=localhost;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        If dgvResults Is Nothing Then
            MessageBox.Show("DataGridView not initialized.")
            Return
        End If

        Dim worderId As String = txtworderid.Text.Trim()
        Dim fromDate As DateTime = dtpfromdate.Value.Date
        Dim toDate As DateTime = dtptodate.Value.Date
        Dim refsampleno As String = If(cmbref.SelectedItem IsNot Nothing, cmbref.SelectedItem.ToString(), String.Empty)

        Dim conditions As New List(Of String)

        If Not String.IsNullOrEmpty(worderId) Then
            conditions.Add("sf.worder_id LIKE @worderId")
        End If

        If Not String.IsNullOrEmpty(refsampleno) Then
            conditions.Add("ss.refsample_no = @refsampleno")
        End If

        If fromDate <> DateTime.MinValue AndAlso toDate <> DateTime.MinValue Then
            conditions.Add("CAST(ss.date AS DATE) BETWEEN @fromDate AND @toDate")
        ElseIf fromDate <> DateTime.MinValue Then
            conditions.Add("CAST(ss.date AS DATE) >= @fromDate")
        ElseIf toDate <> DateTime.MinValue Then
            conditions.Add("CAST(ss.date AS DATE) <= @toDate")
        End If

        Dim whereClause As String = If(conditions.Count > 0, "WHERE " & String.Join(" AND ", conditions), "")

        Dim query As String = "SELECT  ss.date, ss.kind_trans as 'نوع الحركه', ss.refsample_no AS 'رقم الاذن', ss.to_or_from AS 'قسم', ss.notes AS 'ملاحظات', " &
                              "sf.worder_id AS 'أمر شغل', sf.batch_no AS 'رقم الرسالة', sf.client_code AS 'عميل', " &
                              "sf.roll AS 'رقم التوب', SUM(ss.height) AS 'الطول', SUM(ss.weight) AS 'الوزن', " &
                              "sf.fabric_grade AS 'درجه القماش', sf.color AS 'اللون', sf.product_name AS 'الخامه' " &
                              "FROM sample_finish ss LEFT JOIN store_finish sf ON ss.storefinishid = sf.id " &
                              whereClause &
                              " GROUP BY sf.id, ss.date, ss.kind_trans,ss.refsample_no, ss.to_or_from, ss.notes, sf.worder_id, sf.batch_no, sf.client_code, sf.roll, sf.fabric_grade, sf.color, sf.product_name"

        Console.WriteLine("Executing Query: " & query)
        LoadData(query, worderId, fromDate, toDate, refsampleno)
    End Sub

    Private Sub LoadData(ByVal query As String, ByVal worderId As String, ByVal fromDate As DateTime, ByVal toDate As DateTime, ByVal refsampleno As String)
        Using connection As New SqlConnection(sqlServerConnectionString)
            Dim adapter As New SqlDataAdapter(query, connection)
            Dim dt As New DataTable()

            ' Add parameters if needed
            If Not String.IsNullOrEmpty(worderId) Then
                adapter.SelectCommand.Parameters.AddWithValue("@worderId", "%" & worderId & "%")
            End If

            If fromDate <> DateTime.MinValue Then
                adapter.SelectCommand.Parameters.AddWithValue("@fromDate", fromDate)
            End If

            If toDate <> DateTime.MinValue Then
                adapter.SelectCommand.Parameters.AddWithValue("@toDate", toDate)
            End If

            If Not String.IsNullOrEmpty(refsampleno) Then
                adapter.SelectCommand.Parameters.AddWithValue("@refsampleno", refsampleno)
            End If

            Try
                connection.Open()
                adapter.Fill(dt)

                If dt.Rows.Count = 0 Then
                    MessageBox.Show("No data found for the given criteria.")
                    Return
                End If

                dgvResults.DataSource = dt
            Catch ex As Exception
                MessageBox.Show("An error occurred while loading data: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Sub finishdisbursmentviewform_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Loadrefsampleno()
        CustomizeDataGridView()
    End Sub
    Private Sub CustomizeDataGridView()
        ' توسيط النصوص داخل الأعمدة
        For Each column As DataGridViewColumn In dgvResults.Columns
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            column.DefaultCellStyle.WrapMode = DataGridViewTriState.True ' لضبط التفاف النصوص
        Next

        ' ضبط عرض الأعمدة تلقائيًا حسب المحتوى
        dgvResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells

        ' تغيير ألوان العناوين
        dgvResults.EnableHeadersVisualStyles = False
        dgvResults.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue ' اختر اللون المناسب
        dgvResults.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black
        dgvResults.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 12, FontStyle.Bold)
        dgvResults.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
    End Sub
    Private Sub Loadrefsampleno()
        cmbref.Items.Clear()

        Using connection As New SqlConnection(sqlServerConnectionString)
            Dim query As String = "SELECT refsample_no FROM sample_finish WHERE refsample_no IS NOT NULL"

            Try
                connection.Open()
                Using command As New SqlCommand(query, connection)
                    Using reader As SqlDataReader = command.ExecuteReader()
                        While reader.Read()
                            cmbref.Items.Add(reader("refsample_no").ToString())
                        End While
                    End Using
                End Using
            Catch ex As Exception
                MessageBox.Show("Error loading refsample_no values: " & ex.Message)
            End Try
        End Using
    End Sub
    Private Sub btnPrint_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnprint.Click
        Dim refsampleno As String = If(Not String.IsNullOrEmpty(txtrefprint.Text), txtrefprint.Text, String.Empty)

        ' If refsampleno is empty, show a message
        If String.IsNullOrEmpty(refsampleno) Then
            MessageBox.Show("Please enter reference ref sample values.")
            Return
        End If

        ' Split the refsampleno string into individual items (comma-separated)
        Dim refsamplenoList As String() = refsampleno.Split(","c)
        ' Optionally, trim spaces
        refsamplenoList = refsamplenoList.Select(Function(r) r.Trim()).ToArray()

        Dim query As String = "SELECT ss.date as 'تاريخ الحركه', ss.kind_trans as 'نوع الحركه', ss.refsample_no AS 'رقم الاذن', ss.to_or_from AS 'قسم', ss.notes AS 'ملاحظات', " & _
                      "sf.worder_id AS 'أمر شغل', sf.batch_no AS 'رقم الرسالة', sf.client_code AS 'عميل', " & _
                      "sf.roll AS 'رقم التوب', SUM(ss.height) AS 'الطول', SUM(ss.weight) AS 'الوزن', " & _
                      "sf.fabric_grade AS 'درجه القماش', sf.color AS 'اللون', sf.product_name AS 'الخامه' " & _
                      "FROM sample_finish ss LEFT JOIN store_finish sf ON ss.storefinishid = sf.id " & _
                      "WHERE ss.refsample_no IN (" & String.Join(",", refsamplenoList.Select(Function(r) "'" & r & "'")) & ") " & _
                      "GROUP BY ss.date, ss.kind_trans, ss.refsample_no, ss.to_or_from, ss.notes, " & _
                      "sf.worder_id, sf.batch_no, sf.client_code, sf.roll, sf.fabric_grade, sf.color, sf.product_name " & _
                      "ORDER BY ss.refsample_no ASC"






        ' Execute the query and export to Excel
        ExportDataToExcel(query)
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
            worksheet.Range(worksheet.Cells(lastRow, 1), worksheet.Cells(lastRow, 8)).Merge()

            ' Calculate and write totals in specified columns
            worksheet.Cells(lastRow, 9).Formula = "=count(i3:i" & (lastRow - 1) & ")"
            worksheet.Cells(lastRow, 10).Formula = "=SUM(j3:j" & (lastRow - 1) & ")"
            worksheet.Cells(lastRow, 11).Formula = "=SUM(k3:k" & (lastRow - 1) & ")"

            ' Set formatting for total row columns
            For col As Integer = 9 To 11
                worksheet.Cells(lastRow, col).Font.Size = 18
                worksheet.Cells(lastRow, col).Font.Bold = True
                worksheet.Cells(lastRow, col).Interior.Color = RGB(184, 204, 228) ' Blue accent, lighter 60%
                worksheet.Cells(lastRow, col).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
            Next

            ' Set borders for the entire data range
            Dim dataRange As Microsoft.Office.Interop.Excel.Range = worksheet.Range("A2", "k" & lastRow)
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
        Dim dt As DataTable = DataGridViewToDataTable(dgvResults)
        ExportToExcel(dt)
    End Sub
    Private Function DataGridViewToDataTable(ByVal dgv As DataGridView) As DataTable
        Dim dt As New DataTable()

        ' Add columns to DataTable
        For Each column As DataGridViewColumn In dgv.Columns
            dt.Columns.Add(column.HeaderText, column.ValueType)
        Next

        ' Add rows to DataTable
        For Each row As DataGridViewRow In dgv.Rows
            If Not row.IsNewRow Then
                Dim newRow As DataRow = dt.NewRow()
                For Each cell As DataGridViewCell In row.Cells
                    newRow(cell.ColumnIndex) = If(cell.Value IsNot Nothing, cell.Value, DBNull.Value)
                Next
                dt.Rows.Add(newRow)
            End If
        Next

        Return dt
    End Function
    Private Sub ExportToExcel(ByVal dt As DataTable)
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
            worksheet.Range(worksheet.Cells(lastRow, 1), worksheet.Cells(lastRow, 8)).Merge()

            ' Calculate and write totals in specified columns
            worksheet.Cells(lastRow, 9).Formula = "=count(i3:i" & (lastRow - 1) & ")"
            worksheet.Cells(lastRow, 10).Formula = "=SUM(j3:j" & (lastRow - 1) & ")"
            worksheet.Cells(lastRow, 11).Formula = "=SUM(k3:k" & (lastRow - 1) & ")"

            ' Set formatting for total row columns
            For col As Integer = 9 To 11
                worksheet.Cells(lastRow, col).Font.Size = 18
                worksheet.Cells(lastRow, col).Font.Bold = True
                worksheet.Cells(lastRow, col).Interior.Color = RGB(184, 204, 228) ' Blue accent, lighter 60%
                worksheet.Cells(lastRow, col).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
            Next

            ' Set borders for the entire data range
            Dim dataRange As Microsoft.Office.Interop.Excel.Range = worksheet.Range("A2", "k" & lastRow)
            dataRange.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin



            ' Adjust column widths automatically
            worksheet.Columns.AutoFit()

            ' Make Excel visible and save changes
            excelApp.Visible = True
        Catch ex As System.Runtime.InteropServices.COMException
            MessageBox.Show("COM Exception: " & ex.Message)
        Catch ex As Exception
            MessageBox.Show("An error occurred while exporting to Excel: " & ex.Message)
        End Try
    End Sub

End Class
