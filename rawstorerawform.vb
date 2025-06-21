Imports System.Data.SqlClient
Imports System.Drawing
Imports Excel = Microsoft.Office.Interop.Excel
Public Class rawstorerawform
    Private connectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"

    Private Sub rawstorerawform_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadComboBoxes()
    End Sub
    Private Sub LoadComboBoxes()
        Using conn As New SqlConnection(connectionString)
            conn.Open()

            ' Load cmbpo
            Using cmd As New SqlCommand("SELECT DISTINCT po_number FROM batch_raw", conn)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        cmbpo.Items.Add(reader("po_number").ToString())
                    End While
                End Using
            End Using

            ' Load cmbServiceType
            Using cmd As New SqlCommand("SELECT DISTINCT service_ar FROM kind_service", conn)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        cmbServiceType.Items.Add(reader("service_ar").ToString())
                    End While
                End Using
            End Using

            ' Load cmbBatchId
            Using cmd As New SqlCommand("SELECT DISTINCT batch FROM batch_raw", conn)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        cmbBatchId.Items.Add(reader("batch").ToString())
                    End While
                End Using
            End Using

            ' Load cmbStorePermission
            Using cmd As New SqlCommand("SELECT DISTINCT store_permission FROM batch_details", conn)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        cmbStorePermission.Items.Add(reader("store_permission").ToString())
                    End While
                End Using
            End Using
        End Using
    End Sub



    Private Sub FormatDataGridView()
        ' Center align text in DataGridView
        For Each column As DataGridViewColumn In dgvReport.Columns
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            column.DefaultCellStyle.Font = New Font(dgvReport.Font, FontStyle.Bold)
        Next

        ' Set header style
        dgvReport.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvReport.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue
        dgvReport.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black
        dgvReport.ColumnHeadersDefaultCellStyle.Font = New Font(dgvReport.Font.FontFamily, 8, FontStyle.Bold) ' Change the font size to 8
        dgvReport.EnableHeadersVisualStyles = False
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim fromDate As DateTime? = If(dtpFromDate.Checked, dtpFromDate.Value, CType(Nothing, DateTime?))
        Dim toDate As DateTime? = If(dtpToDate.Checked, dtpToDate.Value, CType(Nothing, DateTime?))
        Dim batchId As String = If(cmbBatchId.SelectedIndex = -1, Nothing, cmbBatchId.SelectedItem.ToString())
        Dim storePermission As String = If(cmbStorePermission.SelectedIndex = -1, Nothing, cmbStorePermission.SelectedItem.ToString())
        Dim serviceType As String = If(cmbServiceType.SelectedIndex = -1, Nothing, cmbServiceType.SelectedItem.ToString())
        Dim poNumber As String = If(cmbpo.SelectedIndex = -1, Nothing, cmbpo.SelectedItem.ToString())

        Dim query As String = "SELECT bd.datetrans as 'تاريخ الحركه', br.batch as 'رقم الرسالة', bd.lot, bd.weight_quantity as 'فعلى وزن', bd.rolls_count as 'فعلى اتواب', bd.meter_quantity as 'متر', bd.weightpk as 'استلامه وزن', bd.rollpk as 'استلامه اتواب', c1.code AS 'كود العميل', c1.name AS 'العميل', sup.code AS 'كود المورد', sup.name AS 'المورد', br.po_number as 'رقم الpo', s.code AS 'كود الخامة', s.name AS 'الخامة1', ks.service_ar as 'نوع الخدمه', br.material as 'الخامة2', bd.client_permission as 'اذن العميل', bd.client_item_code as 'كود الصنف', bd.store_permission as 'اذن الاضافه', bd.username as 'يوزر' " &
                          "FROM batch_raw br " &
                          "JOIN clients c1 ON br.client_code = c1.id " &
                          "LEFT JOIN suppliers sup ON br.sup_code = sup.id " &
                          "LEFT JOIN fabric f ON br.fabric_type = f.id " &
                          "LEFT JOIN style s ON br.style_id = s.id " &
                          "LEFT JOIN kind_service ks ON br.service_id = ks.id " &
                          "LEFT JOIN batch_details bd ON br.batch = bd.batch_id " &
                          "WHERE (1=1) " &
                          "AND (@fromDate IS NULL OR bd.datetrans >= @fromDate) " &
                          "AND (@toDate IS NULL OR bd.datetrans <= @toDate) " &
                          "AND (@batchId IS NULL OR br.batch = @batchId) " &
                          "AND (@storePermission IS NULL OR bd.store_permission = @storePermission) " &
                          "AND (@serviceType IS NULL OR ks.service_ar = @serviceType) " &
                          "AND (@poNumber IS NULL OR br.po_number = @poNumber)"

        Using conn As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@fromDate", If(fromDate.HasValue, fromDate, DBNull.Value))
                cmd.Parameters.AddWithValue("@toDate", If(toDate.HasValue, toDate, DBNull.Value))
                cmd.Parameters.AddWithValue("@batchId", If(batchId IsNot Nothing, batchId, DBNull.Value))
                cmd.Parameters.AddWithValue("@storePermission", If(storePermission IsNot Nothing, storePermission, DBNull.Value))
                cmd.Parameters.AddWithValue("@serviceType", If(serviceType IsNot Nothing, serviceType, DBNull.Value))
                cmd.Parameters.AddWithValue("@poNumber", If(poNumber IsNot Nothing, poNumber, DBNull.Value))

                Dim adapter As New SqlDataAdapter(cmd)
                Dim table As New DataTable()
                adapter.Fill(table)
                dgvReport.DataSource = table

                ' Format DataGridView
                FormatDataGridView()
            End Using
        End Using
    End Sub
    Private Sub btnSearchOther_Click(sender As Object, e As EventArgs) Handles btnSearchOther.Click
        Dim fromDate As DateTime? = If(dtpFromDate.Checked, dtpFromDate.Value, CType(Nothing, DateTime?))
        Dim toDate As DateTime? = If(dtpToDate.Checked, dtpToDate.Value, CType(Nothing, DateTime?))
        Dim batchId As String = If(cmbBatchId.SelectedIndex = -1, Nothing, cmbBatchId.SelectedItem.ToString())

        Dim query As String = "SELECT batch_id as 'الرسالة', lot, issued_weight as 'وزن صرف او مرتجع', issued_rolls as 'اتواب صرف او مرتجع', issue_date as 'تاريخ الحركة', worderid as 'امر شغل', username as 'يوزر', store_ref as 'الاذن'" &
                          "FROM raw_distribute " &
                          "WHERE (1=1) " &
                          "AND (@fromDate IS NULL OR issue_date >= @fromDate) " &
                          "AND (@toDate IS NULL OR issue_date <= @toDate) " &
                          "AND (@batchId IS NULL OR batch_id = @batchId)"

        Using conn As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@fromDate", If(fromDate.HasValue, fromDate, DBNull.Value))
                cmd.Parameters.AddWithValue("@toDate", If(toDate.HasValue, toDate, DBNull.Value))
                cmd.Parameters.AddWithValue("@batchId", If(batchId IsNot Nothing, batchId, DBNull.Value))

                Dim adapter As New SqlDataAdapter(cmd)
                Dim table As New DataTable()
                adapter.Fill(table)
                dgvReport.DataSource = table

                ' Format DataGridView
                FormatDataGridView()
            End Using
        End Using
    End Sub

    Private Sub btnExportToExcel_Click(sender As Object, e As EventArgs) Handles btnExportToExcel.Click
        Dim excelApp As New Excel.Application()
        Dim workbook As Excel.Workbook = excelApp.Workbooks.Add()
        Dim worksheet As Excel.Worksheet = CType(workbook.Sheets(1), Excel.Worksheet)

        ' Add column headers
        For i As Integer = 1 To dgvReport.Columns.Count
            worksheet.Cells(1, i) = dgvReport.Columns(i - 1).HeaderText
        Next

        ' Add rows
        For i As Integer = 0 To dgvReport.Rows.Count - 1
            For j As Integer = 0 To dgvReport.Columns.Count - 1
                worksheet.Cells(i + 2, j + 1) = dgvReport.Rows(i).Cells(j).Value
            Next
        Next

        ' Format the header row
        Dim headerRange As Excel.Range = worksheet.Range(worksheet.Cells(1, 1), worksheet.Cells(1, dgvReport.Columns.Count))
        headerRange.Font.Bold = True
        headerRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightBlue)
        headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter

        ' Auto-fit columns
        worksheet.Columns.AutoFit()

        ' Show the Excel application
        excelApp.Visible = True
    End Sub

End Class