Imports System.Data.SqlClient
Imports Mysqlx.XDevAPI.CRUD
Imports Org.BouncyCastle.Asn1.Cmp
Imports Excel = Microsoft.Office.Interop.Excel
Public Class mainqcrawtestform
    Private sqlServerConnectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"

    Private Sub mainqcrawtestform_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadBatchIDs()
        lblUsername.Text = "Logged in as: " & LoggedInUsername
        ' Retrieve the public_name for the logged-in user
        Dim publicName As String = GetPublicName(LoggedInUsername)
        Loadstatus()
    End Sub

    Private Sub btnadd_Click(sender As Object, e As EventArgs) Handles btnadd.Click
        ' تحقق من تحديد عنصر في ComboBox
        If cmblot.SelectedIndex <> -1 AndAlso cmbbatch.SelectedIndex <> -1 Then
            ' احصل على بيانات lot و batch من ComboBox
            Dim lotData As String = cmblot.SelectedItem.ToString()
            Dim batchData As String = cmbbatch.SelectedItem.ToString()

            ' إخفاء النموذج الحالي
            Me.Hide()

            ' إنشاء نموذج جديد
            Dim brefForm As New qcrawtestform()

            ' تعيين القيم في النموذج الجديد
            brefForm.SetBatchAndLot(batchData, lotData)

            ' عرض النموذج الجديد
            brefForm.Show()
        Else
            MessageBox.Show("No lot or batch selected in ComboBox.")
        End If
    End Sub
    Private Sub Loadstatus()
        Dim query As String = "SELECT bd.batch_id as 'الرساله', bd.lot, ks.service_ar as 'نوع الخدمه', CLIENT_ITEM_CODE as 'كود صنف العميل',bd.datetrans " &
          "FROM batch_details bd " &
          "LEFT JOIN qc_raw_test qt ON bd.lot = qt.lot " &
          "LEFT JOIN batch_raw bt ON bd.batch_id = bt.batch " &
          "LEFT JOIN kind_service ks ON bt.service_id = ks.id " &
          "WHERE qt.lot IS NULL " &
          "AND bd.datetrans > '2025-02-02'" &
          "ORDER BY bd.datetrans DESC;"



        Using conn As New SqlConnection(sqlServerConnectionString)
            Dim cmd As New SqlCommand(query, conn)
            Try
                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                Dim dt As New DataTable()
                dt.Load(reader)
                dgvresult.DataSource = dt

                ' Center the content of the DataGridView
                For Each column As DataGridViewColumn In dgvresult.Columns
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    column.Width = 150 ' Set a larger width for each column
                Next

                ' Set the font size to 12 and make it bold for content
                dgvresult.DefaultCellStyle.Font = New Font("Arial", 12, FontStyle.Bold)

                ' Set the header font size to 12, make it bold, and center the content
                dgvresult.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 14, FontStyle.Bold)
                dgvresult.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

                ' Fill the color of the headers
                dgvresult.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue
                dgvresult.EnableHeadersVisualStyles = False

                ' Adjust the width of each column to fit the data
                dgvresult.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            Catch ex As Exception
                MessageBox.Show("Error loading initial data: " & ex.Message)
            End Try
        End Using
    End Sub
    Private Sub chkShowInitialData_CheckedChanged(sender As Object, e As EventArgs) Handles chkShowInitialData.CheckedChanged
        Loadstatus()
    End Sub
    Private Sub dgvresult_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvresult.CellClick
        If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
            Dim selectedBatchId As String = dgvresult.Rows(e.RowIndex).Cells("الرساله").Value.ToString()
            Dim selectedLot As String = dgvresult.Rows(e.RowIndex).Cells("lot").Value.ToString()


            ' Select the batch and lot in ComboBox
            SelectBatchAndLot(selectedBatchId, selectedLot)
        End If
    End Sub
    Private Sub LoadBatchIDs()
        Dim query As String = "SELECT DISTINCT batch_id FROM batch_details"

        Using conn As New SqlConnection(sqlServerConnectionString)
            Dim cmd As New SqlCommand(query, conn)
            Try
                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()

                cmbbatch.Items.Clear()
                While reader.Read()
                    cmbbatch.Items.Add(reader("batch_id").ToString())
                End While
            Catch ex As Exception
                MessageBox.Show("Error loading Batch IDs: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Sub cmbbatch_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbbatch.SelectedIndexChanged
        LoadLotIDs(cmbbatch.SelectedItem.ToString())
    End Sub

    Private Sub LoadLotIDs(batchId As String)
        Dim query As String = "SELECT lot FROM batch_details WHERE batch_id = @batch_id"

        Using conn As New SqlConnection(sqlServerConnectionString)
            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@batch_id", batchId)
            Try
                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()

                cmblot.Items.Clear()
                While reader.Read()
                    cmblot.Items.Add(reader("lot").ToString())
                End While
            Catch ex As Exception
                MessageBox.Show("Error loading Lot IDs: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Function IsDataAlreadySaved(batchId As String, lot As String) As Boolean
        Dim query As String = "SELECT COUNT(*) FROM qc_raw_test WHERE batch_id = @batch_id AND lot = @lot"
        Using conn As New SqlConnection(sqlServerConnectionString)
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@batch_id", batchId)
                cmd.Parameters.AddWithValue("@lot", lot)
                Try
                    conn.Open()
                    Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                    Return count > 0
                Catch ex As Exception
                    MessageBox.Show("Error checking data: " & ex.Message)
                    Return False
                End Try
            End Using
        End Using
    End Function

    Private Sub LoadDataIntoDgvResult(batchId As String, lot As String)
        Dim query As String = "SELECT date,batch_id as 'batch',lot,raw_befor_width as 'عرض قبل',raw_after_width as 'عرض بعد',raw_befor_weight as 'وزن متر مربع قبل',raw_after_weight as 'وزن متر مربع بعد',pva_Starch as 'Pva / Starch',mix_rate as 'نسبه الخلط',tensile_weft as 'Tensile Weft',tensile_warp as 'Tensile Warp',tensile_result as 'Tensile Result',color_water as 'Color Fitness Water',tear_weft as 'Tear Weft',tear_warp as 'Tear Warp',tear_result as 'Tear Result',washing as 'Color Fastness for Washing',color_mercerize as 'Color Fastness for Mercerization',notes,username as 'user' FROM qc_raw_test WHERE batch_id = @batch_id AND lot = @lot"
        Using conn As New SqlConnection(sqlServerConnectionString)
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@batch_id", batchId)
                cmd.Parameters.AddWithValue("@lot", lot)
                Try
                    conn.Open()
                    Dim reader As SqlDataReader = cmd.ExecuteReader()
                    Dim dt As New DataTable()
                    dt.Load(reader)
                    dgvresult.DataSource = dt

                    ' Center the content of the DataGridView
                    For Each column As DataGridViewColumn In dgvresult.Columns
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                        column.DefaultCellStyle.Font = New Font("Arial", 10, FontStyle.Bold) ' Set font size to 12 and make it bold
                        column.Width = 150 ' Set a larger width for each column
                    Next

                    ' Set the header font size to 12, make it bold, and center the content
                    dgvresult.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 10, FontStyle.Bold)
                    dgvresult.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

                    ' Fill the color of the headers
                    dgvresult.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue
                    dgvresult.EnableHeadersVisualStyles = False

                    ' Adjust the width of each column to fit the data
                    dgvresult.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
                Catch ex As Exception
                    MessageBox.Show("Error loading data: " & ex.Message)
                End Try
            End Using
        End Using
    End Sub

    Private Sub SetDgvResultReadOnly()
        For Each column As DataGridViewColumn In dgvresult.Columns
            column.ReadOnly = True
        Next
    End Sub

    Private Sub cmblot_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmblot.SelectedIndexChanged
        LoadLotDetails(cmblot.SelectedItem.ToString())
        LoadDataIntoDgvResult(cmbbatch.SelectedItem.ToString(), cmblot.SelectedItem.ToString())
    End Sub

    Private Sub LoadLotDetails(lot As String)
        Dim query As String = "SELECT cs.code AS 'كود العميل',cs.name AS 'العميل', bd.weightpk AS 'كميه وزن', bd.meter_quantity AS 'كميه متر', bd.rollpk AS 'عدد اتواب',br.material " &
                              "FROM batch_details bd " &
                              "LEFT JOIN batch_raw br ON bd.batch_id = br.batch " &
                              "LEFT JOIN clients cs ON br.client_code = cs.id " &
                              "WHERE bd.lot = @lot"

        Using conn As New SqlConnection(sqlServerConnectionString)
            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@lot", lot)
            Try
                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                Dim dt As New DataTable()
                dt.Load(reader)
                dataGridViewDetails.DataSource = dt

                ' Center the content of the DataGridView
                For Each column As DataGridViewColumn In dataGridViewDetails.Columns
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    column.Width = 150 ' Set a larger width for each column
                Next

                ' Set the font size to 12 and make it bold for content
                dataGridViewDetails.DefaultCellStyle.Font = New Font("Arial", 12, FontStyle.Bold)

                ' Set the header font size to 12, make it bold, and center the content
                dataGridViewDetails.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 14, FontStyle.Bold)
                dataGridViewDetails.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

                ' Fill the color of the headers
                dataGridViewDetails.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue
                dataGridViewDetails.EnableHeadersVisualStyles = False

                ' Adjust the width of each column to fit the data
                dataGridViewDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            Catch ex As Exception
                MessageBox.Show("Error loading Lot details: " & ex.Message)
            End Try
        End Using
    End Sub

    Public Sub SelectBatchAndLot(batchData As String, lotData As String)
        ' تحديد الرسالة في ComboBox
        cmbbatch.SelectedItem = batchData

        ' تحميل اللوتات بناءً على الرسالة المحددة
        LoadLotIDs(batchData)

        ' تحديد اللوت في ComboBox
        cmblot.SelectedItem = lotData
    End Sub

    Private Function GetPublicName(ByVal username As String) As String
        Dim publicName As String = String.Empty
        Try
            Using conn As New SqlConnection(sqlServerConnectionString)
                ' SQL query to get the public_name from dep_users where username matches
                Dim query As String = "SELECT public_name FROM dep_users WHERE username = @username"
                Dim cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@username", username)

                conn.Open()
                ' Execute the query and retrieve the public_name
                Dim result = cmd.ExecuteScalar()
                If result IsNot Nothing Then
                    publicName = result.ToString()
                End If
                conn.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error retrieving public name: " & ex.Message)
        End Try
        Return publicName
    End Function

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim fromDate As DateTime = dtpFromDate.Value
        Dim toDate As DateTime = dtpToDate.Value

        LoadDataByDateRange(fromDate, toDate)
    End Sub

    Private Sub LoadDataByDateRange(fromDate As DateTime, toDate As DateTime)
        ' Adjust the toDate to include the entire day
        toDate = toDate.AddDays(1).AddTicks(-1)

        Dim query As String = "SELECT date,batch_id as 'batch',lot,raw_befor_width as 'عرض قبل',raw_after_width as 'عرض بعد',raw_befor_weight as 'وزن متر مربع قبل',raw_after_weight as 'وزن متر مربع بعد',pva_Starch as 'Pva / Starch',mix_rate as 'نسبه الخلط',tensile_weft as 'Tensile Weft',tensile_warp as 'Tensile Warp',tensile_result as 'Tensile Result',color_water as 'Color Fitness Water',tear_weft as 'Tear Weft',tear_warp as 'Tear Warp',tear_result as 'Tear Result',washing as 'Color Fastness for Washing',color_mercerize as 'Color Fastness for Mercerization',notes,username as 'user' FROM qc_raw_test WHERE date BETWEEN @fromDate AND @toDate"
        Using conn As New SqlConnection(sqlServerConnectionString)
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@fromDate", fromDate)
                cmd.Parameters.AddWithValue("@toDate", toDate)
                Try
                    conn.Open()
                    Dim reader As SqlDataReader = cmd.ExecuteReader()
                    Dim dt As New DataTable()
                    dt.Load(reader)
                    dgvresult.DataSource = dt

                    ' Center the content of the DataGridView
                    For Each column As DataGridViewColumn In dgvresult.Columns
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                        column.DefaultCellStyle.Font = New Font("Arial", 10, FontStyle.Bold) ' Set font size to 12 and make it bold
                        column.Width = 150 ' Set a larger width for each column
                    Next

                    ' Set the header font size to 12, make it bold, and center the content
                    dgvresult.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 10, FontStyle.Bold)
                    dgvresult.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

                    ' Fill the color of the headers
                    dgvresult.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue
                    dgvresult.EnableHeadersVisualStyles = False

                    ' Adjust the width of each column to fit the data
                    dgvresult.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
                Catch ex As Exception
                    MessageBox.Show("Error loading data: " & ex.Message)
                End Try
            End Using
        End Using
    End Sub
    Private Sub btnExportToExcel_Click(sender As Object, e As EventArgs) Handles btnExportToExcel.Click
        Dim excelApp As New Excel.Application()
        Dim workbook As Excel.Workbook = excelApp.Workbooks.Add()
        Dim worksheet As Excel.Worksheet = CType(workbook.Sheets(1), Excel.Worksheet)

        ' Add column headers
        For i As Integer = 1 To dgvresult.Columns.Count
            worksheet.Cells(1, i) = dgvresult.Columns(i - 1).HeaderText
        Next

        ' Add rows
        For i As Integer = 0 To dgvresult.Rows.Count - 1
            For j As Integer = 0 To dgvresult.Columns.Count - 1
                worksheet.Cells(i + 2, j + 1) = dgvresult.Rows(i).Cells(j).Value
            Next
        Next

        ' Format the header row
        Dim headerRange As Excel.Range = worksheet.Range(worksheet.Cells(1, 1), worksheet.Cells(1, dgvresult.Columns.Count))
        headerRange.Font.Bold = True
        headerRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightBlue)
        headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter

        ' Auto-fit columns
        worksheet.Columns.AutoFit()

        ' Show the Excel application
        excelApp.Visible = True
    End Sub

End Class
