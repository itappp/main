Imports System.Data.SqlClient
Imports System.Windows.Forms
Imports System.Data
Imports Excel = Microsoft.Office.Interop.Excel ' Alias for Excel Interop

Imports System.IO

Public Class reportstop
    ' Define the connection string
    Private connectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"

    ' Constructor for the form
    Public Sub New()
        InitializeComponent()

        ' Add event handlers for Load Report and export buttons
        AddHandler btnLoadReport.Click, AddressOf btnLoadReport_Click
        AddHandler btnExportExcel.Click, AddressOf btnExportExcel_Click
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

                ' Base query for retrieving data
                ' Base query for retrieving data with custom column names
                Dim query As String = "SELECT WorderID AS 'Worder ID', " & _
                    "name_arab AS 'Machine', " & _
                                      "StopStartTime AS 'Stop From', " & _
                                      "StopEndTime AS 'Stop To', " & _
                                      "StopDuration AS 'Net Time Stop', " & _
                                      "StopReason AS 'Stop Reason' " & _
                                      "FROM StopSchedule " & _
                                      "LEFT JOIN machine ON StopSchedule.machineid = machine.id " & _
                                      "WHERE 1=1 " ' Base query to simplify appending filters
                ' Base query to simplify appending filters

                ' Prepare filters
                Dim filters As New List(Of String)
                Dim parameters As New List(Of SqlParameter)

                ' Check if Work Order ID is provided and add the filter for Work Order ID
                If Not String.IsNullOrEmpty(txtWorderID.Text) Then
                    filters.Add("WorderID = @WorderID")
                    parameters.Add(New SqlParameter("@WorderID", SqlDbType.VarChar) With {.Value = txtWorderID.Text})
                Else
                    ' If no Work Order ID is provided, apply the date filter
                    If dtpFrom.Value <= dtpTo.Value Then
                        filters.Add("CAST(StopStartTime AS DATE) BETWEEN @FromDate AND @ToDate")
                        parameters.Add(New SqlParameter("@FromDate", SqlDbType.Date) With {.Value = dtpFrom.Value.Date})
                        parameters.Add(New SqlParameter("@ToDate", SqlDbType.Date) With {.Value = dtpTo.Value.Date})
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
        dgvReport.Columns("Worder ID").Width = 150
        dgvReport.Columns("Machine").Width = 150
        dgvReport.Columns("Stop From").Width = 150
        dgvReport.Columns("Stop To").Width = 150
        dgvReport.Columns("Net Time Stop").Width = 150
        dgvReport.Columns("Stop Reason").Width = 350
        


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

    ' Form Load event (if needed)
    Private Sub ReportForm_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        ' Optionally, load data when form loads
        ' LoadReportData()
    End Sub


End Class
