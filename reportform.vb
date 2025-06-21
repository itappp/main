Imports System.Data.SqlClient
Imports System.Windows.Forms
Imports System.Data
Imports Excel = Microsoft.Office.Interop.Excel ' Alias for Excel Interop
Imports System.IO

Public Class reportform
    ' Define the connection string
    Private connectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"

    ' زر جديد لتصدير أعمدة محددة بتنسيق خاص
    Private WithEvents btnExportSelectedExcel As New Button With {.Text = "Export Selected Excel"}

    ' Constructor for the form
    Public Sub New()
        InitializeComponent()

        ' Add event handlers for Load Report and export buttons
        AddHandler btnLoadReport.Click, AddressOf btnLoadReport_Click
        AddHandler btnExportExcel.Click, AddressOf btnExportExcel_Click
        AddHandler btnExportSelectedExcel.Click, AddressOf btnExportSelectedExcel_Click
        ' أضف الزر للفورم (يمكنك وضعه في المكان المناسب)
        Me.Controls.Add(btnExportSelectedExcel)
        btnExportSelectedExcel.Top = btnExportExcel.Top
        btnExportSelectedExcel.Left = btnExportExcel.Left + btnExportExcel.Width + 10
    End Sub

    ' Load report data when the button is clicked
    Private Sub btnLoadReport_Click(ByVal sender As Object, ByVal e As EventArgs)
        LoadReportData()
    End Sub

    ' Method to load report data from the database
    Private Sub LoadReportData()
        Using conn As New SqlConnection(connectionString)
            Try
                conn.Open()

                ' Base query for retrieving data with custom column names
                Dim query As String = "SELECT WorderID AS 'Worder ID', " &
                                      "name_arab AS 'Machine', " &
                                      "name_ar AS 'Process', " &
                                      "speed AS 'Speed', " &
                                      "FORMAT(StartTime, 'yyyy-MM-dd hh:mm:ss tt') AS 'Start Time', " &
                                      "FORMAT(EndTime, 'yyyy-MM-dd hh:mm:ss tt') AS 'End Time', " &
                                      "CASE  " &
                                      "    WHEN (" &
                                      "        CASE  " &
                                      "            WHEN DATEPART(HOUR, StartTime) >= 8 AND DATEPART(HOUR, StartTime) < 16 THEN N'الورديه الأولى' " &
                                      "            WHEN DATEPART(HOUR, StartTime) >= 16 AND DATEPART(HOUR, StartTime) < 23 THEN N'الورديه الثانية' " &
                                      "            ELSE N'الورديه الثالثة' " &
                                      "        END " &
                                      "    ) = (" &
                                      "        CASE  " &
                                      "            WHEN DATEPART(HOUR, EndTime) >= 8 AND DATEPART(HOUR, EndTime) < 16 THEN N'الورديه الأولى' " &
                                      "            WHEN DATEPART(HOUR, EndTime) >= 16 AND DATEPART(HOUR, EndTime) < 23 THEN N'الورديه الثانية' " &
                                      "            ELSE N'الورديه الثالثة' " &
                                      "        END " &
                                      "    ) THEN (" &
                                      "        CASE  " &
                                      "            WHEN DATEPART(HOUR, StartTime) >= 8 AND DATEPART(HOUR, StartTime) < 16 THEN N'الورديه الأولى' " &
                                      "            WHEN DATEPART(HOUR, StartTime) >= 16 AND DATEPART(HOUR, StartTime) < 23 THEN N'الورديه الثانية' " &
                                      "            ELSE N'الورديه الثالثة' " &
                                      "        END " &
                                      "    ) ELSE (" &
                                      "        CASE  " &
                                      "            WHEN DATEPART(HOUR, StartTime) >= 8 AND DATEPART(HOUR, StartTime) < 16 THEN N'الورديه الأولى' " &
                                      "            WHEN DATEPART(HOUR, StartTime) >= 16 AND DATEPART(HOUR, StartTime) < 23 THEN N'الورديه الثانية' " &
                                      "            ELSE N'الورديه الثالثة' " &
                                      "        END + N' و' + " &
                                      "        CASE  " &
                                      "            WHEN DATEPART(HOUR, EndTime) >= 8 AND DATEPART(HOUR, EndTime) < 16 THEN N'الورديه الأولى' " &
                                      "            WHEN DATEPART(HOUR, EndTime) >= 16 AND DATEPART(HOUR, EndTime) < 23 THEN N'الورديه الثانية' " &
                                      "            ELSE N'الورديه الثالثة' " &
                                      "        END " &
                                      "    ) " &
                                      "END AS 'الورديه', " &
                                      "CASE " &
                                      "    WHEN (DATEPART(HOUR, StartTime) >= 23) THEN FORMAT(StartTime, 'yyyy-MM-dd') " &
                                      "    WHEN (DATEPART(HOUR, StartTime) < 8) THEN FORMAT(DATEADD(day, -1, StartTime), 'yyyy-MM-dd') " &
                                      "    ELSE FORMAT(StartTime, 'yyyy-MM-dd') " &
                                      "END AS 'تاريخ الورديه', " &
                                      "quantity AS 'Quantity', " &
                                      "quantitym AS 'QuantityM', " &
                                      "quantitykg AS 'QuantityKG', " &
                                      "total_time AS 'Total Time', " &
                                      "duration AS 'Net Time', " &
                                      "(DATEPART(HOUR, duration) * 3600 + DATEPART(MINUTE, duration) * 60 + DATEPART(SECOND, duration)) AS 'Net Time Seconds', " &
                                      "(DATEPART(HOUR, duration) * 60 + DATEPART(MINUTE, duration) + DATEPART(SECOND, duration) / 60.0) AS 'minutes', " &
                                      "(DATEPART(HOUR, duration) + DATEPART(MINUTE, duration) / 60.0 + DATEPART(SECOND, duration) / 3600.0) AS 'hours', " &
                                      "totalstoptime AS 'Total Stop Time', " &
                                      "username AS 'Worker', " &
                                      "Notes, " &
                                      "LAG(name_ar, 1) OVER (PARTITION BY WorderID ORDER BY StartTime) AS 'Previous Process', " &
                                      "LAG(EndTime, 1) OVER (PARTITION BY WorderID ORDER BY StartTime) AS 'Previous End Time', " &
                                      "DATEDIFF(day, LAG(EndTime, 1) OVER (PARTITION BY WorderID ORDER BY StartTime), StartTime) AS 'Days Difference', " &
                                      "DATEDIFF(second, LAG(EndTime, 1) OVER (PARTITION BY WorderID ORDER BY StartTime), StartTime) / 3600 AS 'Hours Difference' " &
                                      "FROM WorkOrder " &
                                      "LEFT JOIN machine ON workorder.machineid = machine.id " &
                                      "LEFT JOIN machine_steps ON workorder.processid = machine_steps.id " &
                                      "WHERE 1=1 " ' Base query to simplify appending filters

                ' Prepare filters
                Dim filters As New List(Of String)
                Dim parameters As New List(Of SqlParameter)

                ' Check if Work Order ID is provided and add the filter for Work Order ID
                If Not String.IsNullOrEmpty(txtWorderID.Text) Then
                    filters.Add("WorderID = @WorderID")
                    parameters.Add(New SqlParameter("@WorderID", SqlDbType.VarChar) With {.Value = txtWorderID.Text})
                Else
                    ' If no Work Order ID is provided, apply the shift-based date filter
                    If dtpFrom.Value <= dtpTo.Value Then
                        If dtpFrom.Value.Date = dtpTo.Value.Date Then
                            ' Single day search: from 8 AM of the day to 8 AM of the next day
                            Dim fromDateTime As DateTime = dtpFrom.Value.Date.AddHours(8)
                            Dim toDateTime As DateTime = dtpFrom.Value.Date.AddDays(1).AddHours(8)
                            filters.Add("StartTime >= @FromDateTime AND StartTime < @ToDateTime")
                            parameters.Add(New SqlParameter("@FromDateTime", SqlDbType.DateTime) With {.Value = fromDateTime})
                            parameters.Add(New SqlParameter("@ToDateTime", SqlDbType.DateTime) With {.Value = toDateTime})
                        Else
                            ' Range search: from 8 AM of the start day to 8 AM of the day after the end day
                            Dim fromDateTime As DateTime = dtpFrom.Value.Date.AddHours(8)
                            Dim toDateTime As DateTime = dtpTo.Value.Date.AddDays(1).AddHours(8)
                            filters.Add("StartTime >= @FromDateTime AND StartTime < @ToDateTime")
                            parameters.Add(New SqlParameter("@FromDateTime", SqlDbType.DateTime) With {.Value = fromDateTime})
                            parameters.Add(New SqlParameter("@ToDateTime", SqlDbType.DateTime) With {.Value = toDateTime})
                        End If
                    Else
                        MessageBox.Show("Please provide a valid date range.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Exit Sub
                    End If
                End If

                ' Append filters to query
                If filters.Count > 0 Then
                    query &= " AND " & String.Join(" AND ", filters)
                End If

                ' Create SQL command
                Using cmd As New SqlCommand(query, conn)
                    ' Add the parameters to the command
                    cmd.Parameters.AddRange(parameters.ToArray())

                    ' Execute query and load the data
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        Dim dataTable As New System.Data.DataTable() ' Fully qualified DataTable
                        dataTable.Load(reader)
                        FormatReport(dataTable) ' Call method to format and display data
                    End Using
                End Using

            Catch ex As SqlException
                MessageBox.Show("Error loading report: " & ex.Message)
            Finally
                conn.Close()
            End Try
        End Using
    End Sub

    ' Method to format and display the report
    Private Sub FormatReport(ByVal dataTable As System.Data.DataTable) ' Fully qualified DataTable
        ' Clear existing data in the DataGridView
        dgvReport.DataSource = Nothing

        ' Set the DataSource of DataGridView to the DataTable
        dgvReport.DataSource = dataTable

        ' Format the DataGridView
        For Each column As DataGridViewColumn In dgvReport.Columns
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
        Next

        ' Set specific column widths after the column names are updated
        dgvReport.Columns("Worder ID").Width = 100
        dgvReport.Columns("Machine").Width = 100
        dgvReport.Columns("Process").Width = 100
        dgvReport.Columns("Speed").Width = 100
        dgvReport.Columns("Start Time").Width = 150
        dgvReport.Columns("End Time").Width = 150
        dgvReport.Columns("الورديه").Width = 120
        dgvReport.Columns("تاريخ الورديه").Width = 120
        dgvReport.Columns("Net Time").Width = 100
        dgvReport.Columns("Net Time Seconds").Width = 100
        dgvReport.Columns("minutes").Width = 80
        dgvReport.Columns("hours").Width = 80
        dgvReport.Columns("Worker").Width = 100
        dgvReport.Columns("Quantity").Width = 100
        dgvReport.Columns("QuantityM").Width = 100
        dgvReport.Columns("QuantityKG").Width = 100
        dgvReport.Columns("Total Time").Width = 100
        dgvReport.Columns("Total Stop Time").Width = 100
        dgvReport.Columns("Notes").Width = 200
        dgvReport.Columns("Previous Process").Width = 150
        dgvReport.Columns("Previous End Time").Width = 150
        dgvReport.Columns("Days Difference").Width = 100
        dgvReport.Columns("Hours Difference").Width = 100

        ' Optionally, format the DataGridView appearance
        dgvReport.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.LightGray
        dgvReport.RowsDefaultCellStyle.BackColor = System.Drawing.Color.White
        dgvReport.RowsDefaultCellStyle.ForeColor = System.Drawing.Color.Black
        dgvReport.RowHeadersVisible = False
    End Sub

    ' Export to Excel
    Private Sub ExportToExcel(ByVal dgv As DataGridView)
        Dim excelApp As New Excel.Application() ' Use the alias for Excel Application
        Dim workbook As Excel.Workbook = excelApp.Workbooks.Add(Type.Missing)
        Dim worksheet As Excel.Worksheet = workbook.Sheets("Sheet1")
        worksheet = workbook.ActiveSheet
        worksheet.Name = "Work Order Report"

        ' Export Column Headers
        For i As Integer = 1 To dgv.Columns.Count
            worksheet.Cells(1, i) = dgv.Columns(i - 1).HeaderText
        Next

        ' Export Data
        For i As Integer = 0 To dgv.Rows.Count - 1
            For j As Integer = 0 To dgv.Columns.Count - 1
                ' Check if the cell value is not Nothing before calling .ToString()
                If dgv.Rows(i).Cells(j).Value IsNot Nothing Then
                    worksheet.Cells(i + 2, j + 1) = dgv.Rows(i).Cells(j).Value.ToString()
                Else
                    worksheet.Cells(i + 2, j + 1) = "" ' Set to an empty string if the value is Nothing
                End If
            Next
        Next

        ' Adjust Column Widths
        worksheet.Columns.AutoFit()

        ' Show the Excel application
        excelApp.Visible = True
    End Sub

    ' Event handler for Export to Excel button
    Private Sub btnExportExcel_Click(ByVal sender As Object, ByVal e As EventArgs)
        ExportToExcel(dgvReport)
    End Sub

    ' دالة التصدير المخصصة
    Private Sub ExportSelectedToExcel(ByVal dgv As DataGridView)
        Dim selectedColumns As String() = {"Worder ID", "Machine", "Process", "Speed", "Start Time", "End Time", "الورديه", "تاريخ الورديه", "QuantityKG", "QuantityM", "Total Time", "Net Time", "minutes", "hours"}
        Dim excelApp As New Excel.Application()
        Dim workbook As Excel.Workbook = excelApp.Workbooks.Add(Type.Missing)
        Dim worksheet As Excel.Worksheet = workbook.Sheets("Sheet1")
        worksheet = workbook.ActiveSheet
        worksheet.Name = "Work Order Selected"

        ' تنسيق العناوين
        Dim headerFontSize As Integer = 12
        Dim headerColor As Integer = RGB(0, 112, 192) ' أزرق غامق
        Dim headerRow As Integer = 1
        Dim colIndex As Integer = 1
        For Each colName In selectedColumns
            worksheet.Cells(headerRow, colIndex) = colName
            worksheet.Cells(headerRow, colIndex).Font.Bold = True
            worksheet.Cells(headerRow, colIndex).Font.Size = headerFontSize
            worksheet.Cells(headerRow, colIndex).Font.Name = "Arial"
            worksheet.Cells(headerRow, colIndex).Interior.Color = headerColor
            worksheet.Cells(headerRow, colIndex).Font.Color = RGB(255, 255, 255)
            colIndex += 1
        Next

        ' تصدير البيانات
        Dim rowIndex As Integer = 2
        For i As Integer = 0 To dgv.Rows.Count - 1
            colIndex = 1
            For Each colName In selectedColumns
                If dgv.Columns.Contains(colName) Then
                    Dim value = dgv.Rows(i).Cells(colName).Value
                    worksheet.Cells(rowIndex, colIndex) = If(value IsNot Nothing, value.ToString(), "")
                End If
                colIndex += 1
            Next
            rowIndex += 1
        Next

        ' تنسيق الأعمدة
        worksheet.Rows(1).Font.Bold = True
        worksheet.Rows(1).Font.Size = headerFontSize
        worksheet.Rows(1).Font.Name = "Arial"
        worksheet.Rows(1).Interior.Color = headerColor
        worksheet.Rows(1).Font.Color = RGB(255, 255, 255)
        worksheet.Columns.AutoFit()
        worksheet.Rows.Font.Name = "Arial"
        worksheet.Rows.Font.Size = 11

        ' إظهار ملف الإكسل
        excelApp.Visible = True
    End Sub

    ' حدث الزر الجديد
    Private Sub btnExportSelectedExcel_Click(ByVal sender As Object, ByVal e As EventArgs)
        ExportSelectedToExcel(dgvReport)
    End Sub

    ' Form Load event (if needed)
    Private Sub ReportForm_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        ' Optionally, load data when form loads
        ' LoadReportData()
    End Sub
End Class
