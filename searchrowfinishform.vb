Imports MySql.Data.MySqlClient
Imports System.Threading.Tasks

Public Class SearchRowFinishForm
    ' Define the connection string to connect to the MySQL database
    Dim connectionString As String = "Server=150.1.1.7;Database=wm;Uid=root1;Pwd=WMg2024$;"

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        Dim worderId As String = txtWorderId.Text
        Dim contractNo As String = txtContractNo.Text
        Dim fromDate As String = dtpFromDate.Value.ToString("yyyy-MM-dd")
        Dim toDate As String = dtpToDate.Value.ToString("yyyy-MM-dd")

        Dim conditions As New List(Of String)

        If Not String.IsNullOrEmpty(worderId) Then
            conditions.Add("g2.worder_id LIKE '%" & MySqlHelper.EscapeString(worderId) & "%'")
        End If

        If Not String.IsNullOrEmpty(contractNo) Then
            conditions.Add("g2d.contract_no = '" & MySqlHelper.EscapeString(contractNo) & "'")
        End If

        If Not String.IsNullOrEmpty(fromDate) AndAlso Not String.IsNullOrEmpty(toDate) Then
            conditions.Add("DATE(g2.date) BETWEEN '" & fromDate & "' AND '" & toDate & "'")
        ElseIf Not String.IsNullOrEmpty(fromDate) Then
            conditions.Add("DATE(g2.date) >= '" & fromDate & "'")
        ElseIf Not String.IsNullOrEmpty(toDate) Then
            conditions.Add("DATE(g2.date) <= '" & toDate & "'")
        End If

        Dim whereClause As String = If(conditions.Count > 0, "WHERE " & String.Join(" AND ", conditions), "")

        ' Query to calculate distinct sum of height, count of rolls without duplicates, and last date
        Dim query As String = "SELECT subquery.worder_id AS 'رقم أمر الشغل', subquery.contract_no AS 'رقم التعاقد',COUNT(DISTINCT subquery.roll) AS 'عدد أتواب', SUM(subquery.h) AS 'إجمالى متر', SUM(subquery.ww) AS 'إجمالى وزن' FROM (SELECT DISTINCT g2.roll, g2.h, g2.ww, g2.worder_id, g2d.contract_no FROM gray_2 g2 LEFT JOIN gray_2_data g2d ON g2.worder_id = g2d.worder_id " & whereClause & ") AS subquery GROUP BY subquery.worder_id, subquery.contract_no;"


        ' Execute the query
        LoadData(query)
    End Sub
    Private Sub LoadData(ByVal query As String)
        Using connection As New MySqlConnection(connectionString)
            Dim adapter As New MySqlDataAdapter(query, connection)
            Dim dt As New DataTable()

            Try
                connection.Open()
                adapter.Fill(dt)
                dgvResults.DataSource = dt
                ' Center-align content and headers for each column
                dgvResults.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                For Each column As DataGridViewColumn In dgvResults.Columns
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                Next

                ' Set custom widths for each column after binding
                dgvResults.Columns("أمر شغل").Width = 180 ' Set width in pixels
                dgvResults.Columns("رقم التعاقد").Width = 120
                dgvResults.Columns("رقم الرسالة").Width = 100
                dgvResults.Columns("الكمية متر").Width = 80
                dgvResults.Columns("إجمالى الأتواب").Width = 80
                dgvResults.Columns("آخر تاريخ").Width = 180
            Catch ex As Exception
                MessageBox.Show("An error occurred: " & ex.Message)
            End Try
        End Using
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
