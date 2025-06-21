Imports System.Data.SqlClient
Imports MySql.Data.MySqlClient
Imports Microsoft.Office.Interop
Imports Excel = Microsoft.Office.Interop.Excel ' For Excel interop
Imports System.Text
Public Class techviewform
    ' SQL Server connection string
    Private sqlServerConnectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"

    ' MySQL connection string
    Private mySqlConnectionString As String = "Server=150.1.1.7;Database=wm;Uid=root1;Pwd=WMg2024$;"

    Private Sub btnPrintWorder_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnprintworder.Click
        If String.IsNullOrEmpty(txtWorderID.Text) Then
            MessageBox.Show("Please enter a Worder ID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Using conn As New SqlConnection(sqlServerConnectionString)
            Dim query As String = "SELECT DISTINCT " &
                          "td.worderid AS 'امر شغل', CONVERT(VARCHAR(10), c.ContractDate, 111) AS 'تاريخ التعاقد', CONVERT(VARCHAR(10), td.created_date, 111) AS 'تاريخ الانشاء', " &
                          "CONVERT(VARCHAR(10), td.Delivery_Dat, 111) AS 'تاريخ التسليم', c.ContractNo AS 'رقم التعاقد', fb.fabrictype_ar AS 'نوع العاقد', " &
                          "cli.Code AS 'كود العميل', c.Batch AS 'الرسالة', c.color AS 'اللون', c.Material AS 'الخامه', " &
                          "l.code AS 'كود المكتبة', l.lib_code AS 'محتوى المكتبة', c.Notes AS 'ملاحظات البيع', td.qty_m AS 'كمية متر (tech)', " &
                          "td.qty_kg AS 'كمية كيلو (tech)', c.QuantityM AS 'اجمالى كمية الرسالة متر', c.QuantityK AS 'اجمالى كمية الرسالة كيلو', " &
                          "c.WeightM AS 'وزن المتر المربع المطلوب', c.RollM AS 'طول التوب المطلوب', c.WidthReq AS 'العرض المطلوب', " &
                          "c.fabriccode AS 'كود الخامة', c.refno AS 'رقم الاذن', td.qc_id, td.InsertedBy, qc.batch_no AS 'Batch No', " &
                          "qc.d1 AS 'Raw Before Width', qc.d2 AS 'Raw After Width', qc.d3 AS 'Weight of M2 Before', " &
                          "qc.d4 AS 'Weight of M2 After', qc.d5 AS 'PVA / Starch', qc.d6 AS 'Mixing Percentage', " &
                          "qc.d7 AS 'Rupture Warp', qc.d8 AS 'Rupture Weft', qc.d9 AS 'Rupture Result', " &
                          "qc.d10 AS 'Color Fastness to Water', qc.d11 AS 'Tear Warp', qc.d12 AS 'Tear Weft', " &
                          "qc.d13 AS 'Tear Result', qc.d14 AS 'Color Fastness for Washing', qc.d15 AS 'Color Fastness for Mercerization', " &
                          "qc.d16 AS 'Notes', qc.mix_rate AS 'Mix Rate' " &
                          "FROM techdata td " &
                          "LEFT JOIN contracts c ON td.contract_id = c.contractid " &
                          "LEFT JOIN library l ON td.code_lib = l.id " &
                          "LEFT JOIN clients cli ON c.ClientCode = cli.id " &
                          "LEFT JOIN qc_lab qc ON td.qc_id = qc.qc_id " &
                          "LEFT JOIN fabric fb ON c.ContractType = fb.id " &
                          "WHERE td.worderid = @WorderID"

            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@WorderID", txtWorderID.Text)

            Try
                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()

                If reader.HasRows Then
                    Dim html As New StringBuilder()
                    html.Append("<html><head><title>Worder Details</title>")
                    html.Append("<style>")
                    html.Append("table { width: auto; border-collapse: collapse; margin-left: 0; }")
                    html.Append("th, td { border: 1px solid black; text-align: center; font-size: 14pt; font-weight: bold; }")
                    html.Append("th { background-color: #f2f2f2; }") ' Light color for headers
                    html.Append("h1, h2 { text-align: center; }")
                    html.Append(".header { display: flex; justify-content: space-between; align-items: center; }")
                    html.Append(".barcode { text-align: right; }")
                    html.Append(".small-table { width: 30%; margin-left: 0; font-size: 14pt; font-weight: bold; }")
                    html.Append(".data-container { display: flex; justify-content: space-between; }")
                    html.Append(".data-table { }")
                    html.Append(".table-header { text-align: left; margin-bottom: 10px; }")
                    html.Append(".other-data { margin-left: auto; }")
                    html.Append(".other-data th, .other-data td { font-size: 11pt; font-weight: bold; }")
                    html.Append("</style>")
                    html.Append("</head><body>")
                    html.Append("<div class='header'><table class='small-table'><tr><th>كمية متر (tech)</th><th>كمية كيلو (tech)</th></tr>")
                    While reader.Read()
                        html.Append("<tr><td>" & reader("كمية متر (tech)").ToString() & "</td><td>" & reader("كمية كيلو (tech)").ToString() & "</td></tr>")
                    End While
                    html.Append("</table><h1 style='flex-grow: 1; text-align: center;'>Worder Details</h1>")

                    html.Append("<div class='barcode'><img src='https://barcode.tec-it.com/barcode.ashx?data=" & txtWorderID.Text & "&code=Code128&dpi=96' alt='Barcode'/></div></div>")

                    html.Append("<div class='data-container'>")

                    ' Main Data Table
                    html.Append("<div class='data-table main-data'><h2 class='table-header'>Main Data</h2>")
                    html.Append("<table>")

                    ' Define headers
                    Dim headers As String() = {"نوع العاقد", "امر شغل", "رقم التعاقد", "الرسالة", "اللون", "الخامه", "كود العميل", "كود المكتبة", "اجمالى كمية الرسالة متر", "اجمالى كمية الرسالة كيلو", "وزن المتر المربع المطلوب", "طول التوب المطلوب", "العرض المطلوب", "كود الخامة", "رقم الاذن"}

                    reader.Close()
                    reader = cmd.ExecuteReader()

                    While reader.Read()
                        For i As Integer = 0 To headers.Length - 1 Step 6
                            ' Add headers row
                            html.Append("<tr>")
                            For j As Integer = i To Math.Min(i + 5, headers.Length - 1)
                                html.Append("<th>" & headers(j) & "</th>")
                            Next
                            html.Append("</tr>")

                            ' Add data row
                            html.Append("<tr>")
                            For j As Integer = i To Math.Min(i + 5, headers.Length - 1)
                                html.Append("<td>" & reader(headers(j)).ToString() & "</td>")
                            Next
                            html.Append("</tr>")
                        Next
                    End While
                    html.Append("</table></div>")

                    ' Other Data Table
                    html.Append("<div class='data-table other-data'><h2 class='table-header'>QC Data</h2>")
                    html.Append("<table>")
                    ' Add first row of headers
                    html.Append("<tr><th>Raw Before Width</th><th>Raw After Width</th><th>Weight of M2 Before</th><th>Weight of M2 After</th></tr>")
                    ' Add first row of data
                    reader.Close()
                    reader = cmd.ExecuteReader()
                    While reader.Read()
                        html.Append("<tr>")
                        html.Append("<td>" & reader("Raw Before Width").ToString() & "</td>")
                        html.Append("<td>" & reader("Raw After Width").ToString() & "</td>")
                        html.Append("<td>" & reader("Weight of M2 Before").ToString() & "</td>")
                        html.Append("<td>" & reader("Weight of M2 After").ToString() & "</td>")
                        html.Append("</tr>")
                    End While
                    ' Add second row of headers
                    html.Append("<tr><th>PVA / Starch</th><th>Mixing Percentage</th><th>Rupture Warp</th><th>Rupture Weft</th></tr>")
                    ' Add second row of data
                    reader.Close()
                    reader = cmd.ExecuteReader()
                    While reader.Read()
                        html.Append("<tr>")
                        html.Append("<td>" & reader("PVA / Starch").ToString() & "</td>")
                        html.Append("<td>" & reader("Mixing Percentage").ToString() & "</td>")
                        html.Append("<td>" & reader("Rupture Warp").ToString() & "</td>")
                        html.Append("<td>" & reader("Rupture Weft").ToString() & "</td>")
                        html.Append("</tr>")
                    End While
                    ' Add third row of headers
                    html.Append("<tr><th>Rupture Result</th><th>Color Fastness to Water</th><th>Tear Warp</th><th>Tear Weft</th></tr>")
                    ' Add third row of data
                    reader.Close()
                    reader = cmd.ExecuteReader()
                    While reader.Read()
                        html.Append("<tr>")
                        html.Append("<td>" & reader("Rupture Result").ToString() & "</td>")
                        html.Append("<td>" & reader("Color Fastness to Water").ToString() & "</td>")
                        html.Append("<td>" & reader("Tear Warp").ToString() & "</td>")
                        html.Append("<td>" & reader("Tear Weft").ToString() & "</td>")
                        html.Append("</tr>")
                    End While
                    ' Add fourth row of headers
                    html.Append("<tr><th>Tear Result</th><th>Color Fastness for Washing</th><th>Color Fastness for Mercerization</th><th>Notes</th></tr>")
                    ' Add fourth row of data
                    reader.Close()
                    reader = cmd.ExecuteReader()
                    While reader.Read()
                        html.Append("<tr>")
                        html.Append("<td>" & reader("Tear Result").ToString() & "</td>")
                        html.Append("<td>" & reader("Color Fastness for Washing").ToString() & "</td>")
                        html.Append("<td>" & reader("Color Fastness for Mercerization").ToString() & "</td>")
                        html.Append("<td>" & reader("Notes").ToString() & "</td>")
                        html.Append("</tr>")
                    End While
                    html.Append("</table></div>")

                    html.Append("</div>") ' Close data-container div

                    ' Notes Table
                    html.Append("<h2 style='text-align: center;'>ملاحظات البيع</h2>")
                    html.Append("<table style='margin: 0 auto;'><tr>")
                    ' Add table headers for notes

                    html.Append("</tr>")

                    ' Add table rows for notes
                    reader.Close()
                    reader = cmd.ExecuteReader()
                    While reader.Read()
                        html.Append("<tr>")
                        html.Append("<td>" & reader("ملاحظات البيع").ToString() & "</td>")
                        html.Append("</tr>")
                    End While
                    html.Append("</table>")

                    ' Library Content Table
                    html.Append("<h2>محتوى المكتبة</h2>")
                    html.Append("<table><tr>")
                    ' Add table headers for library content
                    html.Append("<th style='width: 250px;'>محتوى المكتبة</th>")
                    html.Append("<th style='width: 200px;'>التاريخ</th>")
                    html.Append("<th style='width: 200px;'>دخول وقت</th>")
                    html.Append("<th style='width: 200px;'>خروج وقت</th>")
                    html.Append("<th style='width: 120px;'>متر</th>")
                    html.Append("<th style='width: 120px;'>كيلو</th>")
                    html.Append("<th style='width: 200px;'>رقم الإفريم</th>")
                    html.Append("<th style='width: 200px;'>ملاحظات</th>")
                    html.Append("</tr>")

                    ' Add table rows for library content
                    reader.Close()
                    reader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim libContent As String = reader("محتوى المكتبة").ToString()
                        Dim libContentItems As String() = libContent.Split("-"c)
                        For Each item As String In libContentItems
                            html.Append("<tr><td>" & item.Trim() & "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>")
                        Next

                    End While
                    html.Append("</table>")

                    html.Append("</body></html>")

                    ' Create a temporary file and open it in the default web browser
                    Dim tempFilePath As String = System.IO.Path.GetTempFileName() & ".html"
                    System.IO.File.WriteAllText(tempFilePath, html.ToString())
                    Process.Start(tempFilePath)
                Else
                    MessageBox.Show("No records found for WorderID: " & txtWorderID.Text)
                End If
            Catch ex As Exception
                MessageBox.Show("An error occurred: " & ex.Message)
            Finally
                conn.Close()
            End Try
        End Using
    End Sub


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
            Dim query As String = "SELECT td.worderid as 'امر شغل', c.ContractDate as 'تاريخ التعاقد', td.created_date as 'تاريخ الانشاء', td.Delivery_Dat as 'تاريخ التسليم', c.ContractNo as 'رقم التعاقد', fb.fabrictype_ar as 'نوع العاقد', cli.Code as 'كود العميل', c.Batch as 'الرسالة', c.color as 'اللون', c.Material as 'الخامه', c.Notes as 'ملاحظات البيع', td.qty_m as 'كمية متر (tech) ', td.qty_kg as 'كمية كيلو (tech) ', l.code as 'كود المكتبة', c.QuantityM as 'اجمالى كمية الرسالةمتر', c.QuantityK as 'اجمالى كمية الرسالة كيلو', c.WeightM as 'وزن المتر المربع المطلوب', c.RollM as 'طول التوب المطلوب', c.WidthReq as 'العرض المطلوب', c.fabriccode as 'كود الخامة', c.refno as 'رقم الاذن', td.qc_id, td.InsertedBy, qc.batch_no AS 'Batch No', qc.d1 AS 'Raw Before Width', qc.d2 AS 'Raw After Width', qc.d3 AS 'Weight of M2 Before', qc.d4 AS 'Weight of M2 After', qc.d5 AS 'PVA / Starch', qc.d6 AS 'Mixing Percentage', qc.d7 AS 'Rupture Warp', qc.d8 AS 'Rupture Weft', qc.d9 AS 'Rupture Result', qc.d10 AS 'Color Fastness to Water', qc.d11 AS 'Tear Warp', qc.d12 AS 'Tear Weft', qc.d13 AS 'Tear Result', qc.d14 AS 'Color Fastness for Washing', qc.d15 AS 'Color Fastness for Mercerization', qc.d16 AS 'Notes', qc.mix_rate AS 'Mix Rate' " &
                                 "FROM techdata td " &
                                 "LEFT JOIN contracts c ON td.contract_id = c.contractid " &
                                 "LEFT JOIN library l ON td.code_lib = l.id " &
                                 "LEFT JOIN clients cli ON c.ClientCode = cli.id " &
                                 "LEFT JOIN qc_lab qc ON td.qc_id = qc.qc_id " &
                                 "LEFT JOIN fabric fb ON c.ContractType = fb.id " &
                                 "WHERE td.worderid = @worderID"

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
            Dim query As String = "SELECT td.worderid AS 'امر شغل', c.ContractDate AS 'تاريخ التعاقد', td.created_date AS 'تاريخ الانشاء', td.Delivery_Dat AS 'تاريخ التسليم', c.ContractNo AS 'رقم التعاقد', fb.fabrictype_ar AS 'نوع العاقد', cli.Code AS 'كود العميل', c.Batch AS 'الرسالة', c.color AS 'اللون', c.Material AS 'الخامه', c.Notes AS 'ملاحظات البيع', td.qty_m AS 'كمية متر (tech)', td.qty_kg AS 'كمية كيلو (tech)', l.code AS 'كود المكتبة', c.QuantityM AS 'اجمالى كمية الرسالة متر', c.QuantityK AS 'اجمالى كمية الرسالة كيلو', c.WeightM AS 'وزن المتر المربع المطلوب', c.RollM AS 'طول التوب المطلوب', c.WidthReq AS 'العرض المطلوب', c.fabriccode AS 'كود الخامة', c.refno AS 'رقم الاذن', td.qc_id, td.InsertedBy, qc.batch_no AS 'Batch No', qc.d1 AS 'Raw Before Width', qc.d2 AS 'Raw After Width', qc.d3 AS 'Weight of M2 Before', qc.d4 AS 'Weight of M2 After', qc.d5 AS 'PVA / Starch', qc.d6 AS 'Mixing Percentage', qc.d7 AS 'Rupture Warp', qc.d8 AS 'Rupture Weft', qc.d9 AS 'Rupture Result', qc.d10 AS 'Color Fastness to Water', qc.d11 AS 'Tear Warp', qc.d12 AS 'Tear Weft', qc.d13 AS 'Tear Result', qc.d14 AS 'Color Fastness for Washing', qc.d15 AS 'Color Fastness for Mercerization', qc.d16 AS 'Notes', qc.mix_rate AS 'Mix Rate' FROM techdata td LEFT JOIN contracts c ON td.contract_id = c.contractid LEFT JOIN library l ON td.code_lib = l.id LEFT JOIN clients cli ON c.ClientCode = cli.id LEFT JOIN qc_lab qc ON td.qc_id = qc.qc_id LEFT JOIN fabric fb ON c.ContractType = fb.id GROUP BY td.worderid, c.ContractDate, td.created_date, td.Delivery_Dat, c.ContractNo, fb.fabrictype_ar, cli.Code, c.Batch, c.color, c.Material, c.Notes, td.qty_m, td.qty_kg, l.code, c.QuantityM, c.QuantityK, c.WeightM, c.RollM, c.WidthReq, c.fabriccode, c.refno, td.qc_id, td.InsertedBy, qc.batch_no, qc.d1, qc.d2, qc.d3, qc.d4, qc.d5, qc.d6, qc.d7, qc.d8, qc.d9, qc.d10, qc.d11, qc.d12, qc.d13, qc.d14, qc.d15, qc.d16, qc.mix_rate;"




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
            Dim query As String = "SELECT l.name_code as 'Name Code', l.code as 'Code', l.lib_code as 'Library Code' " &
                                  "FROM library l " &
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
            Dim query As String = "SELECT td.worderid AS 'امر شغل', c.ContractDate AS 'تاريخ التعاقد', td.created_date AS 'تاريخ الانشاء', " &
                                  "td.Delivery_Dat AS 'تاريخ التسليم', c.ContractNo AS 'رقم التعاقد', fb.fabrictype_ar AS 'نوع العاقد', " &
                                  "cli.Code AS 'كود العميل', c.Batch AS 'الرسالة', c.color AS 'اللون', c.Material AS 'الخامه', " &
                                  "c.Notes AS 'ملاحظات البيع', td.qty_m AS 'كمية متر (tech)', td.qty_kg AS 'كمية كيلو (tech)', " &
                                  "l.code AS 'كود المكتبة', c.QuantityM AS 'اجمالى كمية الرسالة متر', c.QuantityK AS 'اجمالى كمية الرسالة كيلو', " &
                                  "c.WeightM AS 'وزن المتر المربع المطلوب', c.RollM AS 'طول التوب المطلوب', c.WidthReq AS 'العرض المطلوب', " &
                                  "c.fabriccode AS 'كود الخامة', c.refno AS 'رقم الاذن', td.qc_id, td.InsertedBy, qc.batch_no AS 'Batch No', " &
                                  "qc.d1 AS 'Raw Before Width', qc.d2 AS 'Raw After Width', qc.d3 AS 'Weight of M2 Before', " &
                                  "qc.d4 AS 'Weight of M2 After', qc.d5 AS 'PVA / Starch', qc.d6 AS 'Mixing Percentage', " &
                                  "qc.d7 AS 'Rupture Warp', qc.d8 AS 'Rupture Weft', qc.d9 AS 'Rupture Result', " &
                                  "qc.d10 AS 'Color Fastness to Water', qc.d11 AS 'Tear Warp', qc.d12 AS 'Tear Weft', " &
                                  "qc.d13 AS 'Tear Result', qc.d14 AS 'Color Fastness for Washing', qc.d15 AS 'Color Fastness for Mercerization', " &
                                  "qc.d16 AS 'Notes', qc.mix_rate AS 'Mix Rate' " &
                                  "FROM contracts c " &
                                  "LEFT JOIN techdata td ON c.contractid = td.contract_id " &
                                  "LEFT JOIN library l ON td.code_lib = l.id " &
                                  "LEFT JOIN clients cli ON c.ClientCode = cli.id " &
                                  "LEFT JOIN qc_lab qc ON  td.qc_id = qc.qc_id " &
                                  "LEFT JOIN fabric fb ON c.ContractType = fb.id " &
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
    Private Sub btnPrint_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnprint.Click
        ' Check for required data in the database before proceeding
        Try
            ' Use your connection string
            Dim connString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"
            Using conn As New SqlConnection(connString)
                conn.Open()

                ' Query to check if qc_id or code_lib are NULL or empty for the given WorderID
                Dim query As String = "SELECT qc_id, code_lib FROM techdata WHERE WorderID = @WorderID"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@WorderID", txtWorderID.Text)

                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            Dim qcId As Object = reader("qc_id")
                            Dim codeLib As Object = reader("code_lib")

                            ' Check if either field is NULL or empty
                            If IsDBNull(qcId) OrElse String.IsNullOrEmpty(qcId.ToString()) OrElse
                               IsDBNull(codeLib) OrElse String.IsNullOrEmpty(codeLib.ToString()) Then
                                MessageBox.Show("Check QC and Code Library before printing.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                Exit Sub
                            End If
                        Else
                            MessageBox.Show("WorderID not found in the database.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            Exit Sub
                        End If
                    End Using
                End Using
            End Using

            ' Proceed to open the Excel file if validation passes
            Dim excelApp As New Excel.Application

            ' Specify the path to the Excel file in the shared folder
            Dim excelFilePath As String = "\\150.1.1.4\wagdy moamen system\New folder\database\data.xlsx"

            ' Open the workbook from the shared folder
            Dim workbook As Excel.Workbook = excelApp.Workbooks.Open(excelFilePath)

            ' Get the worksheet named "Production Woven Order"
            Dim worksheet As Excel.Worksheet = workbook.Sheets("Production Woven Order")

            ' Activate the sheet
            worksheet.Activate()

            ' Write the data from txtWorderID to cell T3
            worksheet.Range("T3").Value = txtWorderID.Text

            ' Select cell T3
            worksheet.Range("T3").Select()

            ' Make Excel visible to the user
            excelApp.Visible = True
        Catch ex As Exception
            MessageBox.Show("Failed to open Excel file: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    Private Sub techviewform_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class
