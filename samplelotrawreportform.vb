Imports System.Data.SqlClient
Imports System.Drawing
Imports Mysqlx.XDevAPI.CRUD
Imports Excel = Microsoft.Office.Interop.Excel

Public Class samplelotrawreportform

    Private sqlServerConnectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"

    Private Sub ReportForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Populate batch and lot ComboBoxes
        PopulateComboBox(cmbBatch, "SELECT DISTINCT batch_id FROM batch_lot_defect")
        PopulateComboBox(cmblot, "SELECT DISTINCT lot FROM batch_lot_defect")
    End Sub

    Private Sub PopulateComboBox(comboBox As ComboBox, query As String)
        Using connection As New SqlConnection(sqlServerConnectionString)
            Dim command As New SqlCommand(query, connection)
            connection.Open()
            Dim reader As SqlDataReader = command.ExecuteReader()
            Dim items As New List(Of String)
            While reader.Read()
                items.Add(reader(0).ToString())
            End While
            comboBox.DataSource = items
        End Using
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim query As String = "SELECT date,batch_id as 'رقم الرسالة',lot, Raw_Moisture As 'رطوبه الخام', mix_rate,raw_width as 'العرض', pva_Starch as 'نشا', needle as 'ابره شق', Separation as 'التفصيد', Durability as 'المتانة', notes " &
                          "FROM batch_lot_defect WHERE 1=1"
        Dim parameters As New List(Of SqlParameter)

        If cmbBatch.SelectedItem IsNot Nothing Then
            query &= " AND batch_id = @batch_id"
            parameters.Add(New SqlParameter("@batch_id", cmbBatch.SelectedItem.ToString()))
        End If

        If cmblot.SelectedItem IsNot Nothing Then
            query &= " AND lot = @lot"
            parameters.Add(New SqlParameter("@lot", cmblot.SelectedItem.ToString()))
        End If

        If dtpfrom.Checked Then
            query &= " AND [date] >= @dateFrom"
            parameters.Add(New SqlParameter("@dateFrom", dtpfrom.Value.Date))
        End If

        If dtpto.Checked Then
            query &= " AND [date] <= @dateTo"
            parameters.Add(New SqlParameter("@dateTo", dtpto.Value.Date))
        End If

        Using connection As New SqlConnection(sqlServerConnectionString)
            Dim command As New SqlCommand(query, connection)
            command.Parameters.AddRange(parameters.ToArray())
            Dim adapter As New SqlDataAdapter(command)
            Dim table As New DataTable()
            adapter.Fill(table)
            dgvResults.DataSource = table
            For Each column As DataGridViewColumn In dgvResults.Columns
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                column.DefaultCellStyle.Font = New Font("Arial", 12, FontStyle.Bold) ' Set font size to 12 and make it bold
                column.Width = 150 ' Set a larger width for each column
            Next

            ' Set the header font size to 12, make it bold, and center the content
            dgvResults.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 14, FontStyle.Bold)
            dgvResults.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            ' Fill the color of the headers
            dgvResults.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue
            dgvResults.EnableHeadersVisualStyles = False

            ' Adjust the width of each column to fit the data
            dgvResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells

        End Using
    End Sub
    Private Sub btnExportToExcel_Click(sender As Object, e As EventArgs) Handles btnExportToExcel.Click
        Dim excelApp As New Excel.Application()
        Dim workbook As Excel.Workbook = excelApp.Workbooks.Add()
        Dim worksheet As Excel.Worksheet = CType(workbook.Sheets(1), Excel.Worksheet)

        ' Add column headers
        For i As Integer = 1 To dgvResults.Columns.Count
            worksheet.Cells(1, i) = dgvResults.Columns(i - 1).HeaderText
        Next

        ' Add rows
        For i As Integer = 0 To dgvResults.Rows.Count - 1
            For j As Integer = 0 To dgvResults.Columns.Count - 1
                worksheet.Cells(i + 2, j + 1) = dgvResults.Rows(i).Cells(j).Value
            Next
        Next

        ' Format the header row
        Dim headerRange As Excel.Range = worksheet.Range(worksheet.Cells(1, 1), worksheet.Cells(1, dgvResults.Columns.Count))
        headerRange.Font.Bold = True
        headerRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightBlue)
        headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter

        ' Auto-fit columns
        worksheet.Columns.AutoFit()

        ' Show the Excel application
        excelApp.Visible = True
    End Sub
End Class
