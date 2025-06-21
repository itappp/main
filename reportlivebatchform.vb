Imports System.Data.SqlClient
Imports Excel = Microsoft.Office.Interop.Excel
Public Class reportlivebatchform
    Private sqlServerConnectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"

    Private Sub reportlivebatchform_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Add items to cmbKind
        cmbKind.Items.Add("Need Approve")
        cmbKind.Items.Add("Need Inspect")
        cmbKind.Items.Add("QC LAB")

        ' Set default selected item
        cmbKind.SelectedIndex = 0
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnsearch.Click
        Dim kind As String = cmbKind.SelectedItem.ToString()
        Dim query As String = ""

        Select Case kind
            Case "Need Approve"
                query = "SELECT bd.datetrans as 'تاريخ استلام المخزن', sp.name as 'المورد', cs.name as 'كود العميل', bd.batch_id, bd.lot " &
                        "FROM batch_details bd " &
                        "LEFT JOIN batch_lot_status bls ON bd.lot = bls.lot " &
                        "LEFT JOIN batch_raw bt ON bd.batch_id = bt.batch " &
                        "LEFT JOIN suppliers sp ON bt.sup_code = sp.id " &
                        "LEFT JOIN clients cs ON bt.client_code = cs.id " &
                        "WHERE bls.lot IS NULL " &
                        "AND bd.datetrans > '2025-02-02' " &
                        "AND cs.id <> 1"
            Case "Need Inspect"
                query = "SELECT bd.datetrans as 'تاريخ استلام المخزن', sp.name as 'المورد', cs.name as 'كود العميل', bd.batch_id, bd.lot " &
                        "FROM batch_details bd " &
                        "LEFT JOIN row_inspect_sample ris ON bd.lot = ris.lot " &
                        "LEFT JOIN batch_raw bt ON bd.batch_id = bt.batch " &
                        "LEFT JOIN suppliers sp ON bt.sup_code = sp.id " &
                        "LEFT JOIN clients cs ON bt.client_code = cs.id " &
                        "WHERE ris.lot IS NULL " &
                        "AND bd.datetrans > '2025-02-02' " &
                        "AND cs.id <> 1"
            Case "QC LAB"
                query = "SELECT bd.datetrans, ks.service_ar, bd.batch_id, bd.lot " &
                        "FROM batch_details bd " &
                        "LEFT JOIN qc_raw_test qt ON bd.lot = qt.lot " &
                        "LEFT JOIN batch_raw bt ON bd.batch_id = bt.batch " &
                        "LEFT JOIN kind_service ks ON bt.service_id = ks.id " &
                        "WHERE qt.lot IS NULL " &
                        "AND bd.datetrans > '2025-02-02'"
        End Select

        Using conn As New SqlConnection(sqlServerConnectionString)
            Using cmd As New SqlCommand(query, conn)
                Try
                    conn.Open()
                    Dim reader As SqlDataReader = cmd.ExecuteReader()
                    Dim dt As New DataTable()
                    dt.Load(reader)
                    dgvDetails.DataSource = dt

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
        For Each column As DataGridViewColumn In dgvDetails.Columns
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            column.DefaultCellStyle.Font = New Font("Arial", 16, FontStyle.Bold)
        Next

        ' Set the header font size to 16, make it bold, and center the content
        dgvDetails.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 16, FontStyle.Bold)
        dgvDetails.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        ' Fill the color of the headers
        dgvDetails.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue
        dgvDetails.EnableHeadersVisualStyles = False
        dgvDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells

    End Sub
    Private Sub btnExportToExcel_Click(sender As Object, e As EventArgs) Handles btnExportToExcel.Click
        Dim excelApp As New Excel.Application()
        Dim workbook As Excel.Workbook = excelApp.Workbooks.Add()
        Dim worksheet As Excel.Worksheet = CType(workbook.Sheets(1), Excel.Worksheet)

        ' Add column headers
        For i As Integer = 1 To dgvDetails.Columns.Count
            worksheet.Cells(1, i) = dgvDetails.Columns(i - 1).HeaderText
        Next

        ' Add rows
        For i As Integer = 0 To dgvDetails.Rows.Count - 1
            For j As Integer = 0 To dgvDetails.Columns.Count - 1
                worksheet.Cells(i + 2, j + 1) = dgvDetails.Rows(i).Cells(j).Value
            Next
        Next

        ' Format the header row
        Dim headerRange As Excel.Range = worksheet.Range(worksheet.Cells(1, 1), worksheet.Cells(1, dgvDetails.Columns.Count))
        headerRange.Font.Bold = True
        headerRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightBlue)
        headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter

        ' Auto-fit columns
        worksheet.Columns.AutoFit()

        ' Show the Excel application
        excelApp.Visible = True
    End Sub
End Class
