Imports System.Data.SqlClient
Imports MySql.Data.MySqlClient
Imports Microsoft.Office.Interop
Imports Excel = Microsoft.Office.Interop.Excel ' For Excel interop
Public Class reporttechnicalform

    ' SQL Server connection string
    Private sqlServerConnectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"

    ' MySQL connection string
    Private mySqlConnectionString As String = "Server=150.1.1.7;Database=wm;Uid=root1;Pwd=WMg2024$;"
    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsearch.Click
        If Not String.IsNullOrEmpty(txtWorderID.Text) Then
            ' Search by WorderID
            SearchByWorderID(txtWorderID.Text)
        ElseIf Not String.IsNullOrEmpty(txtcode.Text) Then
            ' Search by Code
            SearchByCode(txtcode.Text)
        ElseIf Not String.IsNullOrEmpty(txtcontractno.Text) Then
            ' Search by ContractNo
            SearchByContractNo(txtcontractno.Text)
        Else
            ' If both fields are empty, check ComboBox selection
            If comboBoxSearchOption.SelectedItem IsNot Nothing Then
                Dim selectedOption As String = comboBoxSearchOption.SelectedItem.ToString()
                If selectedOption = "WorderID" Then
                    ' Retrieve all records by WorderID
                    SearchAllRecords()
                ElseIf selectedOption = "Code" Then
                    ' Retrieve all records by Code
                    SearchAllRecordsByCode()
                Else
                    MessageBox.Show("Please select a valid search option.")
                End If
            Else
                MessageBox.Show("Please select an option from the ComboBox.")
            End If
        End If
    End Sub
    ' Search all records when Code is empty (by Code)
    Private Sub SearchAllRecordsByCode()
        Using conn As New SqlConnection(sqlServerConnectionString)
            Dim query As String = "SELECT l.id, l.name_code as 'Name Code', l.code as 'Code', l.lib_code as 'Library Code' FROM library l "

            Dim cmd As New SqlCommand(query, conn)

            Try
                conn.Open()
                ' Fill DataGridView with the results
                Dim adapter As New SqlDataAdapter(cmd)
                Dim table As New DataTable()
                adapter.Fill(table)

                dataGridView1.DataSource = table

                ' Set specific width for each column if needed
                dataGridView1.Columns("id").Width = 30
                dataGridView1.Columns("Name Code").Width = 210  ' Set width for "Name Code" column
                dataGridView1.Columns("Code").Width = 140      ' Set width for "Code" column

                ' Set a fixed width for the "Library Code" column and enable wrapping
                dataGridView1.Columns("Library Code").Width = 1000
                dataGridView1.Columns("Library Code").DefaultCellStyle.WrapMode = DataGridViewTriState.True
            Catch ex As Exception
                MessageBox.Show("An error occurred: " & ex.Message)
            Finally
                conn.Close()
            End Try
        End Using
    End Sub
    ' Search by Worder ID to retrieve data from the "techdata" table
    Private Sub SearchByWorderID(ByVal worderID As String)
        Using conn As New SqlConnection(sqlServerConnectionString)
            ' Query to retrieve data for the specified Worder ID
            Dim query As String = "SELECT td.worderid as 'امر شغل', c.ContractDate as 'تاريخ التعاقد', td.created_date as 'تاريخ الانشاء', td.Delivery_Dat as 'تاريخ التسليم', c.ContractNo as 'رقم التعاقد', fb.fabrictype_ar as 'نوع العاقد', cli.Code as 'كود العميل', c.Batch as 'الرسالة', c.color as 'اللون', c.Material as 'الخامه', " & _
                       "COUNT(CASE WHEN al.kindtrans = 'change code' THEN al.worderid END) AS 'عدد مرات تغيير المكتبه', c.Notes as 'ملاحظات البيع', td.qty_m as 'كمية متر (tech) ', td.qty_kg as 'كمية كيلو (tech) ', l.code as 'كود المكتبة', c.QuantityM as 'اجمالى كمية الرسالةمتر', c.QuantityK as 'اجمالى كمية الرسالة كيلو', c.WeightM as 'وزن المتر المربع المطلوب', c.RollM as 'طول التوب المطلوب', c.WidthReq as 'العرض المطلوب', c.fabriccode as 'كود الخامة', c.refno as 'رقم الاذن', " & _
                       "td.qc_id, td.InsertedBy, qc.batch_no AS 'Batch No', qc.d1 AS 'Raw Before Width', qc.d2 AS 'Raw After Width', qc.d3 AS 'Weight of M2 Before', qc.d4 AS 'Weight of M2 After', qc.d5 AS 'PVA / Starch', qc.d6 AS 'Mixing Percentage', qc.d7 AS 'Rupture Warp', qc.d8 AS 'Rupture Weft', qc.d9 AS 'Rupture Result', qc.d10 AS 'Color Fastness to Water', qc.d11 AS 'Tear Warp', qc.d12 AS 'Tear Weft', qc.d13 AS 'Tear Result', qc.d14 AS 'Color Fastness for Washing', qc.d15 AS 'Color Fastness for Mercerization', qc.d16 AS 'Notes', qc.mix_rate AS 'Mix Rate' " & _
                       "FROM techdata td " & _
                       "LEFT JOIN contracts c ON td.contract_id = c.contractid " & _
                       "LEFT JOIN library l ON td.code_lib = l.id " & _
                       "LEFT JOIN clients cli ON c.ClientCode = cli.id " & _
                       "LEFT JOIN qc_lab qc ON td.qc_id = qc.qc_id " & _
                       "LEFT JOIN fabric fb ON c.ContractType = fb.id " & _
                       "LEFT JOIN activity_logs al ON td.worderid = al.worderid " & _
                       "WHERE td.worderid = @worderID " & _
                       "GROUP BY td.worderid, c.ContractDate, td.created_date, td.Delivery_Dat, c.ContractNo, fb.fabrictype_ar, cli.Code, c.Batch, c.color, c.Material, c.Notes, td.qty_m, td.qty_kg, l.code, c.QuantityM, c.QuantityK, c.WeightM, c.RollM, c.WidthReq, c.fabriccode, c.refno, td.qc_id, td.InsertedBy, qc.batch_no, qc.d1, qc.d2, qc.d3, qc.d4, qc.d5, qc.d6, qc.d7, qc.d8, qc.d9, qc.d10, qc.d11, qc.d12, qc.d13, qc.d14, qc.d15, qc.d16, qc.mix_rate"


            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@worderID", worderID)

            Try
                conn.Open()

                ' Fill DataGridView with the results
                Dim adapter As New SqlDataAdapter(cmd)
                Dim table As New DataTable()
                adapter.Fill(table)

                dataGridView1.DataSource = table

                ' Set column width based on data
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells

            Catch ex As Exception
                MessageBox.Show("An error occurred: " & ex.Message)
            Finally
                conn.Close()
            End Try
        End Using
    End Sub
    ' Search all records when Worder ID is empty
    Private Sub SearchAllRecords()
        Using conn As New SqlConnection(sqlServerConnectionString)
            ' Query to retrieve all data from the "techdata" table
            Dim query As String = _
     "SELECT " & _
     "td.worderid AS 'امر شغل', " & _
     "c.ContractDate AS 'تاريخ التعاقد', " & _
     "td.created_date AS 'تاريخ الانشاء', " & _
     "td.Delivery_Dat AS 'تاريخ التسليم', " & _
     "c.ContractNo AS 'رقم التعاقد', " & _
     "fb.fabrictype_ar AS 'نوع العاقد', " & _
     "cli.Code AS 'كود العميل', " & _
     "c.Batch AS 'الرسالة', " & _
     "c.color AS 'اللون', " & _
     "c.Material AS 'الخامه', " & _
     "COUNT(CASE WHEN al.kindtrans = 'change code' THEN al.worderid END) AS 'عدد مرات تغيير المكتبه', " & _
     "c.Notes AS 'ملاحظات البيع', " & _
     "td.qty_m AS 'كمية متر (tech)', " & _
     "td.qty_kg AS 'كمية كيلو (tech)', " & _
     "l.code AS 'كود المكتبة', " & _
     "c.QuantityM AS 'اجمالى كمية الرسالة متر', " & _
     "c.QuantityK AS 'اجمالى كمية الرسالة كيلو', " & _
     "c.WeightM AS 'وزن المتر المربع المطلوب', " & _
     "c.RollM AS 'طول التوب المطلوب', " & _
     "c.WidthReq AS 'العرض المطلوب', " & _
     "c.fabriccode AS 'كود الخامة', " & _
     "c.refno AS 'رقم الاذن', " & _
     "td.qc_id, " & _
     "td.InsertedBy, " & _
     "qc.batch_no AS 'Batch No', " & _
     "qc.d1 AS 'Raw Before Width', " & _
     "qc.d2 AS 'Raw After Width', " & _
     "qc.d3 AS 'Weight of M2 Before', " & _
     "qc.d4 AS 'Weight of M2 After', " & _
     "qc.d5 AS 'PVA / Starch', " & _
     "qc.d6 AS 'Mixing Percentage', " & _
     "qc.d7 AS 'Rupture Warp', " & _
     "qc.d8 AS 'Rupture Weft', " & _
     "qc.d9 AS 'Rupture Result', " & _
     "qc.d10 AS 'Color Fastness to Water', " & _
     "qc.d11 AS 'Tear Warp', " & _
     "qc.d12 AS 'Tear Weft', " & _
     "qc.d13 AS 'Tear Result', " & _
     "qc.d14 AS 'Color Fastness for Washing', " & _
     "qc.d15 AS 'Color Fastness for Mercerization', " & _
     "qc.d16 AS 'Notes', " & _
     "qc.mix_rate AS 'Mix Rate' " & _
     "FROM techdata td " & _
     "LEFT JOIN contracts c ON td.contract_id = c.contractid " & _
     "LEFT JOIN library l ON td.code_lib = l.id " & _
     "LEFT JOIN clients cli ON c.ClientCode = cli.id " & _
     "LEFT JOIN activity_logs al ON td.worderid = al.worderid " & _
     "LEFT JOIN qc_lab qc ON td.qc_id = qc.qc_id " & _
     "LEFT JOIN fabric fb ON c.ContractType = fb.id " & _
     "GROUP BY " & _
     "td.worderid, " & _
     "c.ContractDate, " & _
     "td.created_date, " & _
     "td.Delivery_Dat, " & _
     "c.ContractNo, " & _
     "fb.fabrictype_ar, " & _
     "cli.Code, " & _
     "c.Batch, " & _
     "c.color, " & _
     "c.Material, " & _
     "c.Notes, " & _
     "td.qty_m, " & _
     "td.qty_kg, " & _
     "l.code, " & _
     "c.QuantityM, " & _
     "c.QuantityK, " & _
     "c.WeightM, " & _
     "c.RollM, " & _
     "c.WidthReq, " & _
     "c.fabriccode, " & _
     "c.refno, " & _
     "td.qc_id, " & _
     "td.InsertedBy, " & _
     "qc.batch_no, " & _
     "qc.d1, " & _
     "qc.d2, " & _
     "qc.d3, " & _
     "qc.d4, " & _
     "qc.d5, " & _
     "qc.d6, " & _
     "qc.d7, " & _
     "qc.d8, " & _
     "qc.d9, " & _
     "qc.d10, " & _
     "qc.d11, " & _
     "qc.d12, " & _
     "qc.d13, " & _
     "qc.d14, " & _
     "qc.d15, " & _
     "qc.d16, " & _
     "qc.mix_rate;"




            Dim cmd As New SqlCommand(query, conn)

            Try
                conn.Open()

                ' Fill DataGridView with the results
                Dim adapter As New SqlDataAdapter(cmd)
                Dim table As New DataTable()
                adapter.Fill(table)

                dataGridView1.DataSource = table
                ' Set column width based on data
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            Catch ex As Exception
                MessageBox.Show("An error occurred: " & ex.Message)
            Finally
                conn.Close()
            End Try
        End Using
    End Sub
    Private Sub SearchByCode(ByVal code As String)
        Using conn As New SqlConnection(sqlServerConnectionString)
            ' Query to retrieve data for the specified Code from the library table
            Dim query As String = "SELECT l.name_code as 'Name Code', l.code as 'Code', l.lib_code as 'Library Code' " & _
                                  "FROM library l " & _
                                  "WHERE l.code = @code"

            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@code", code)

            Try
                conn.Open()

                ' Fill DataGridView with the results
                Dim adapter As New SqlDataAdapter(cmd)
                Dim table As New DataTable()
                adapter.Fill(table)

                dataGridView1.DataSource = table

                ' Set specific width for each column if needed
                dataGridView1.Columns("Name Code").Width = 210  ' Set width for "Name Code" column
                dataGridView1.Columns("Code").Width = 150      ' Set width for "Code" column

                ' Set a fixed width for the "Library Code" column and enable wrapping
                dataGridView1.Columns("Library Code").Width = 1020
                dataGridView1.Columns("Library Code").DefaultCellStyle.WrapMode = DataGridViewTriState.True

            Catch ex As Exception
                MessageBox.Show("An error occurred: " & ex.Message)
            Finally
                conn.Close()
            End Try
        End Using
    End Sub
    Private Sub SearchByContractNo(ByVal contractNo As String)
        Using conn As New SqlConnection(sqlServerConnectionString)
            Dim query As String = _
     "SELECT " & _
     "td.worderid AS 'امر شغل', " & _
     "c.ContractDate AS 'تاريخ التعاقد', " & _
     "td.created_date AS 'تاريخ الانشاء', " & _
     "td.Delivery_Dat AS 'تاريخ التسليم', " & _
     "c.ContractNo AS 'رقم التعاقد', " & _
     "fb.fabrictype_ar AS 'نوع العاقد', " & _
     "cli.Code AS 'كود العميل', " & _
     "c.Batch AS 'الرسالة', " & _
     "c.color AS 'اللون', " & _
     "c.Material AS 'الخامه', " & _
     "COUNT(CASE WHEN al.kindtrans = 'change code' THEN al.worderid END) AS 'Activity Count', " & _
     "c.Notes AS 'ملاحظات البيع', " & _
     "td.qty_m AS 'كمية متر (tech)', " & _
     "td.qty_kg AS 'كمية كيلو (tech)', " & _
     "l.code AS 'كود المكتبة', " & _
     "c.QuantityM AS 'اجمالى كمية الرسالة متر', " & _
     "c.QuantityK AS 'اجمالى كمية الرسالة كيلو', " & _
     "c.WeightM AS 'وزن المتر المربع المطلوب', " & _
     "c.RollM AS 'طول التوب المطلوب', " & _
     "c.WidthReq AS 'العرض المطلوب', " & _
     "c.fabriccode AS 'كود الخامة', " & _
     "c.refno AS 'رقم الاذن', " & _
     "td.qc_id, " & _
     "td.InsertedBy, " & _
     "qc.batch_no AS 'Batch No', " & _
     "qc.d1 AS 'Raw Before Width', " & _
     "qc.d2 AS 'Raw After Width', " & _
     "qc.d3 AS 'Weight of M2 Before', " & _
     "qc.d4 AS 'Weight of M2 After', " & _
     "qc.d5 AS 'PVA / Starch', " & _
     "qc.d6 AS 'Mixing Percentage', " & _
     "qc.d7 AS 'Rupture Warp', " & _
     "qc.d8 AS 'Rupture Weft', " & _
     "qc.d9 AS 'Rupture Result', " & _
     "qc.d10 AS 'Color Fastness to Water', " & _
     "qc.d11 AS 'Tear Warp', " & _
     "qc.d12 AS 'Tear Weft', " & _
     "qc.d13 AS 'Tear Result', " & _
     "qc.d14 AS 'Color Fastness for Washing', " & _
     "qc.d15 AS 'Color Fastness for Mercerization', " & _
     "qc.d16 AS 'Notes', " & _
     "qc.mix_rate AS 'Mix Rate' " & _
     "FROM contracts c " & _
     "LEFT JOIN techdata td ON c.contractid = td.contract_id " & _
     "LEFT JOIN library l ON td.code_lib = l.id " & _
     "LEFT JOIN clients cli ON c.ClientCode = cli.id " & _
     "LEFT JOIN qc_lab qc ON td.qc_id = qc.qc_id " & _
     "LEFT JOIN fabric fb ON c.ContractType = fb.id " & _
     "LEFT JOIN activity_logs al ON td.worderid = al.worderid " & _
     "WHERE c.ContractNo = @contractNo"

            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@contractNo", contractNo)

            Try

                conn.Open()

                Dim adapter As New SqlDataAdapter(cmd)
                Dim table As New DataTable()
                adapter.Fill(table)

                If table.Rows.Count = 0 Then
                    MessageBox.Show("No records found for ContractNo: " & contractNo)
                Else
                    dataGridView1.DataSource = table
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
                End If

            Catch ex As Exception
                MessageBox.Show("An error occurred: " & ex.Message)
            Finally
                conn.Close()
            End Try
        End Using
    End Sub


    Private Sub btnExportExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportExcel.Click
        If dataGridView1.Rows.Count > 0 Then
            ' Initialize Excel application
            Dim xlApp As New Excel.Application
            Dim xlWorkbook As Excel.Workbook = xlApp.Workbooks.Add
            Dim xlWorksheet As Excel.Worksheet = CType(xlWorkbook.Sheets(1), Excel.Worksheet)

            ' Transfer DataGridView column headers to Excel
            For i As Integer = 1 To dataGridView1.Columns.Count
                xlWorksheet.Cells(1, i) = dataGridView1.Columns(i - 1).HeaderText
            Next

            ' Transfer DataGridView rows to Excel
            For rowIndex As Integer = 0 To dataGridView1.Rows.Count - 1
                For colIndex As Integer = 0 To dataGridView1.Columns.Count - 1
                    ' Check if the cell value is null or DBNull and handle accordingly
                    Dim cellValue As Object = dataGridView1.Rows(rowIndex).Cells(colIndex).Value
                    If IsDBNull(cellValue) OrElse cellValue Is Nothing Then
                        xlWorksheet.Cells(rowIndex + 2, colIndex + 1) = "" ' Assign an empty string if null
                    Else
                        xlWorksheet.Cells(rowIndex + 2, colIndex + 1) = cellValue.ToString()
                    End If
                Next
            Next

            ' Save the Excel file
            Dim saveFileDialog As New SaveFileDialog
            saveFileDialog.Filter = "Excel Files|*.xlsx"
            saveFileDialog.Title = "Save Excel File"
            saveFileDialog.ShowDialog()

            If saveFileDialog.FileName <> "" Then
                xlWorkbook.SaveAs(saveFileDialog.FileName)
                xlWorkbook.Close()
                xlApp.Quit()
                MessageBox.Show("Data exported successfully.", "Export to Excel")
            End If
        Else
            MessageBox.Show("No data to export.", "Export to Excel")
        End If
    End Sub

End Class