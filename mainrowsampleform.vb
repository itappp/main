Imports System.Data.SqlClient

Public Class mainrowsampleform
    Private sqlServerConnectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"

    Private Sub mainrowsampleform_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadBatchIDs()
        lblUsername.Text = "Logged in as: " & LoggedInUsername
        ' Retrieve the public_name for the logged-in user
        Dim publicName As String = GetPublicName(LoggedInUsername)

        cmbStatus.Items.Add("مقبولة")
        cmbStatus.Items.Add("مرفوضة")

        Dim allowedUsers As String() = {"mirna", "mostafa"}
        If Not allowedUsers.Contains(LoggedInUsername.ToLower()) Then
            btnadd.Enabled = False
        End If

        ' Load data into dataGridViewDetails2
        LoadInitialData()
        Loadstatus()
    End Sub
    Private Sub LoadInitialData()

        Dim query As String = "SELECT bd.batch_id as 'الرساله', bd.lot, sp.name as 'المورد', cs.name as 'كود العميل', bd.datetrans as 'تاريخ استلام المخزن' " &
                              "FROM batch_details bd " &
                              "LEFT JOIN row_inspect_sample ris ON bd.lot = ris.lot " &
                              "LEFT JOIN batch_raw bt ON bd.batch_id = bt.batch " &
                              "LEFT JOIN suppliers sp ON bt.sup_code = sp.id " &
                              "LEFT JOIN clients cs ON bt.client_code = cs.id " &
                              "WHERE ris.lot IS NULL AND bd.datetrans > '2025-02-02' AND cs.id <> 1 " &
                              "ORDER BY bd.datetrans DESC"

            Using conn As New SqlConnection(sqlServerConnectionString)
                Dim cmd As New SqlCommand(query, conn)
                Try
                    conn.Open()
                    Dim reader As SqlDataReader = cmd.ExecuteReader()
                    Dim dt As New DataTable()
                    dt.Load(reader)
                    dataGridViewDetails2.DataSource = dt

                    ' Center the content of the DataGridView
                    For Each column As DataGridViewColumn In dataGridViewDetails2.Columns
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                        column.Width = 150 ' Set a larger width for each column
                    Next

                    ' Set the font size to 12 and make it bold for content
                    dataGridViewDetails2.DefaultCellStyle.Font = New Font("Arial", 12, FontStyle.Bold)

                    ' Set the header font size to 12, make it bold, and center the content
                    dataGridViewDetails2.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 14, FontStyle.Bold)
                    dataGridViewDetails2.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

                    ' Fill the color of the headers
                    dataGridViewDetails2.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue
                    dataGridViewDetails2.EnableHeadersVisualStyles = False

                    ' Adjust the width of each column to fit the data
                    dataGridViewDetails2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
                Catch ex As Exception
                    MessageBox.Show("Error loading initial data: " & ex.Message)
                End Try
            End Using


    End Sub


    Private Sub Loadstatus()
        Dim query As String = "SELECT bd.batch_id as 'الرساله', bd.lot, " &
                              "sp.name as 'المورد', cs.name as 'كود العميل', " &
                              "bd.datetrans as 'تاريخ استلام المخزن' " &
                              "FROM batch_details bd " &
                              "LEFT JOIN batch_lot_status bls ON bd.lot = bls.lot " &
                              "LEFT JOIN batch_raw bt ON bd.batch_id = bt.batch " &
                              "LEFT JOIN suppliers sp ON bt.sup_code = sp.id " &
                              "LEFT JOIN clients cs ON bt.client_code = cs.id " &
                              "WHERE bls.lot IS NULL " &
                              "AND bd.datetrans > '2025-02-02' " &
                              "ORDER BY bd.datetrans DESC;"

        Using conn As New SqlConnection(sqlServerConnectionString)
            Dim cmd As New SqlCommand(query, conn)
            Try
                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                Dim dt As New DataTable()
                dt.Load(reader)
                dgvstatus.DataSource = dt

                ' Center the content of the DataGridView
                For Each column As DataGridViewColumn In dgvstatus.Columns
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    column.Width = 150 ' Set a larger width for each column
                Next

                ' Set the font size to 12 and make it bold for content
                dgvstatus.DefaultCellStyle.Font = New Font("Arial", 12, FontStyle.Bold)

                ' Set the header font size to 12, make it bold, and center the content
                dgvstatus.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 14, FontStyle.Bold)
                dgvstatus.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

                ' Fill the color of the headers
                dgvstatus.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue
                dgvstatus.EnableHeadersVisualStyles = False

                ' Adjust the width of each column to fit the data
                dgvstatus.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            Catch ex As Exception
                MessageBox.Show("Error loading initial data: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Sub chkShowInitialData_CheckedChanged(sender As Object, e As EventArgs) Handles chkShowInitialData.CheckedChanged
        LoadInitialData()
    End Sub
    Private Sub dataGridViewDetails2_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dataGridViewDetails2.CellClick
        If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
            Dim selectedBatchId As String = dataGridViewDetails2.Rows(e.RowIndex).Cells("الرساله").Value.ToString()
            Dim selectedLot As String = dataGridViewDetails2.Rows(e.RowIndex).Cells("lot").Value.ToString()


            ' Select the batch and lot in ComboBox
            SelectBatchAndLot(selectedBatchId, selectedLot)
        End If
    End Sub
    Private Sub dgvstatus_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvstatus.CellClick
        If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
            Dim selectedBatchId As String = dgvstatus.Rows(e.RowIndex).Cells("الرساله").Value.ToString()
            Dim selectedLot As String = dgvstatus.Rows(e.RowIndex).Cells("lot").Value.ToString()


            ' Select the batch and lot in ComboBox
            SelectBatchAndLot(selectedBatchId, selectedLot)
        End If
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
            Dim brefForm As New brefsampleinfoform()

            ' تعيين القيم في النموذج الجديد
            brefForm.SetBatchAndLot(batchData, lotData)

            ' عرض النموذج الجديد
            brefForm.Show()
        Else
            MessageBox.Show("No lot or batch selected in ComboBox.")
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
        Dim query As String = "SELECT COUNT(*) FROM batch_lot_defect WHERE batch_id = @batch_id AND lot = @lot"
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
        Dim query As String = "SELECT Raw_Moisture as 'رطوبه الخام', mix_rate,raw_width as 'العرض', pva_Starch as 'نشا', needle as 'ابره شق', Separation as 'التفصيد', Durability as 'المتانة', notes " &
                          "FROM batch_lot_defect WHERE batch_id = @batch_id AND lot = @lot"
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
                        column.DefaultCellStyle.Font = New Font("Arial", 12, FontStyle.Bold) ' Set font size to 12 and make it bold
                        column.Width = 150 ' Set a larger width for each column
                    Next

                    ' Set the header font size to 12, make it bold, and center the content
                    dgvresult.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 14, FontStyle.Bold)
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
        LoadDetailsIntoDataGridView(cmbbatch.SelectedItem.ToString(), cmblot.SelectedItem.ToString())
        CheckAndSetStatus(cmbbatch.SelectedItem.ToString(), cmblot.SelectedItem.ToString())
        LoadDataIntoDgvResult(cmbbatch.SelectedItem.ToString(), cmblot.SelectedItem.ToString())
        UpdateTotalHeight(cmbbatch.SelectedItem.ToString(), cmblot.SelectedItem.ToString())
    End Sub
    Private Sub UpdateTotalHeight(batchId As String, lot As String)
        Dim query As String = "SELECT SUM(height) AS TotalHeight FROM row_inspect_sample WHERE batch_id = @batch_id AND lot = @lot"
        Using conn As New SqlConnection(sqlServerConnectionString)
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@batch_id", batchId)
                cmd.Parameters.AddWithValue("@lot", lot)
                Try
                    conn.Open()
                    Dim totalHeight As Object = cmd.ExecuteScalar()
                    If totalHeight IsNot DBNull.Value Then
                        lblTotalHeight.Text = "اجمالى الطول: " & Convert.ToDecimal(totalHeight).ToString("F2")
                    Else
                        lblTotalHeight.Text = "Total Height: N/A"
                    End If
                Catch ex As Exception
                    MessageBox.Show("Error calculating total height: " & ex.Message)
                End Try
            End Using
        End Using
    End Sub
    Private Sub LoadLotDetails(lot As String)
        Dim query As String = "SELECT cs.code AS 'كود العميل',cs.name AS 'العميل', sp.name as 'المورد',bd.weightpk AS 'كميه وزن', bd.meter_quantity AS 'كميه متر', bd.rollpk AS 'عدد اتواب',br.material " &
                              "FROM batch_details bd " &
                              "LEFT JOIN batch_raw br ON bd.batch_id = br.batch " &
                              "LEFT JOIN clients cs ON br.client_code = cs.id " &
                              "LEFT JOIN suppliers sp ON br.sup_code = sp.id " &
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
    Private Sub btnaddroll_Click(sender As Object, e As EventArgs) Handles btnaddroll.Click
        ' تحقق من تحديد عنصر في ComboBox
        If cmblot.SelectedIndex <> -1 AndAlso cmbbatch.SelectedIndex <> -1 Then
            ' احصل على بيانات lot و batch من ComboBox
            Dim lotData As String = cmblot.SelectedItem.ToString()
            Dim batchData As String = cmbbatch.SelectedItem.ToString()

            ' عرض رسالة تحتوي على بيانات lot و batch
            MessageBox.Show("Batch Data: " & batchData & vbCrLf & "Lot Data: " & lotData)

            ' إخفاء النموذج الحالي
            Me.Hide()

            ' إنشاء نموذج جديد
            Dim fetchForm As New fetchrowinspectsampleform()

            ' تمرير البيانات إلى النموذج الجديد
            fetchForm.SetData(dataGridViewDetails.DataSource, lotData, batchData)

            ' عرض النموذج الجديد
            fetchForm.Show()
        Else
            MessageBox.Show("No lot or batch selected in ComboBox.")
        End If
    End Sub

    Public Sub SelectBatchAndLot(batchData As String, lotData As String)
        ' تحديد الرسالة في ComboBox
        cmbbatch.SelectedItem = batchData

        ' تحميل اللوتات بناءً على الرسالة المحددة
        LoadLotIDs(batchData)

        ' تحديد اللوت في ComboBox
        cmblot.SelectedItem = lotData
    End Sub


    Private Sub LoadDetailsIntoDataGridView(batchId As String, lot As String)
        Dim query As String = "SELECT fi.date as 'التاريخ', fi.batch_id as 'رقم الرسالة', fi.lot as 'اللوت', " &
                          "fi.roll as 'رقم التوب', fi.width as 'العرض', fi.height AS 'الطول', us.user_ar as 'عامل1', us2.user_ar as 'عامل2'," &
                          "(SELECT CAST((SUM(fid.point) * 10000) / (fi.width * fi.height) AS DECIMAL(10, 2)) " &
                          "FROM row_inspect_sample_defects fid " &
                          "WHERE fid.batch_id = fi.batch_id AND fid.lot = fi.lot AND fid.roll = fi.roll) AS 'points', " &
                          "fi.notes, " &
                          "STUFF((SELECT ' - ' + CAST(fid.point AS VARCHAR) " &
                          "FROM row_inspect_sample_defects fid " &
                          "WHERE fid.batch_id = fi.batch_id AND fid.lot = fi.lot AND fid.roll = fi.roll " &
                          "FOR XML PATH('')), 1, 3, '') AS 'النقاط', " &
                          "STUFF((SELECT ' - ' + gd.name_ar " &
                          "FROM row_inspect_sample_defects fid " &
                          "LEFT JOIN gray_defects gd ON fid.defect_id = gd.id " &
                          "WHERE fid.batch_id = fi.batch_id AND fid.lot = fi.lot AND fid.roll = fi.roll " &
                          "FOR XML PATH('')), 1, 3, '') AS 'العيوب' " &
                          "FROM row_inspect_sample fi " &
                          "LEFT JOIN dep_users us ON fi.username = us.username " &
                          "LEFT JOIN dep_users us2 ON fi.username2 = us2.id " &
                          "WHERE fi.batch_id = @batch_id AND fi.lot = @lot"

        Using conn As New SqlConnection(sqlServerConnectionString)
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@batch_id", batchId)
                cmd.Parameters.AddWithValue("@lot", lot)
                Try
                    conn.Open()
                    Dim reader As SqlDataReader = cmd.ExecuteReader()
                    Dim dt As New DataTable()
                    dt.Load(reader)
                    dataGridViewDetails2.DataSource = dt

                    ' Center the content of the DataGridView
                    For Each column As DataGridViewColumn In dataGridViewDetails2.Columns
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                        column.Width = 150 ' Set a larger width for each column
                    Next

                    ' Set the font size to 12 and make it bold for content
                    dataGridViewDetails2.DefaultCellStyle.Font = New Font("Arial", 12, FontStyle.Bold)

                    ' Set the header font size to 12, make it bold, and center the content
                    dataGridViewDetails2.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 14, FontStyle.Bold)
                    dataGridViewDetails2.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

                    ' Fill the color of the headers
                    dataGridViewDetails2.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue
                    dataGridViewDetails2.EnableHeadersVisualStyles = False

                    ' Adjust the width of each column to fit the data
                    dataGridViewDetails2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells

                    ' Calculate the total points
                    Dim totalPointsQuery As String = "SELECT CAST(SUM(fid.point) * 10000 / (SUM(fi.height) * MAX(fi.width)) AS DECIMAL(10, 2)) AS TotalPoints " &
                                                 "FROM row_inspect_sample_defects fid " &
                                                 "JOIN row_inspect_sample fi ON fid.batch_id = fi.batch_id AND fid.lot = fi.lot AND fid.roll = fi.roll " &
                                                 "WHERE fid.batch_id = @batch_id AND fid.lot = @lot"

                    Using totalPointsCmd As New SqlCommand(totalPointsQuery, conn)
                        totalPointsCmd.Parameters.AddWithValue("@batch_id", batchId)
                        totalPointsCmd.Parameters.AddWithValue("@lot", lot)
                        Dim totalPoints As Object = totalPointsCmd.ExecuteScalar()
                        If totalPoints IsNot DBNull.Value Then
                            lbltotalpoints.Text = "Total Points: " & Convert.ToDecimal(totalPoints).ToString("F2")
                        Else
                            lbltotalpoints.Text = "Total Points: N/A"
                        End If
                    End Using

                Catch ex As Exception
                    MessageBox.Show("Error loading details: " & ex.Message)
                End Try
            End Using
        End Using
    End Sub

    Private Sub btnreport_Click(sender As Object, e As EventArgs) Handles btnreport.Click
        Dim htmlContent As String = GenerateReportContent()
        If String.IsNullOrEmpty(htmlContent) Then
            MessageBox.Show("Error generating report content.")
            Return
        End If

        ' Create a temporary HTML file
        Dim tempFilePath As String = IO.Path.Combine(IO.Path.GetTempPath(), "report.html")
        IO.File.WriteAllText(tempFilePath, htmlContent)

        ' Open the HTML file in the default browser
        Process.Start(New ProcessStartInfo(tempFilePath) With {.UseShellExecute = True})
    End Sub

    Private Function GenerateReportContent() As String
        Dim batchId As String = cmbbatch.SelectedItem.ToString()
        Dim lot As String = cmblot.SelectedItem.ToString()
        Dim query As String = "SELECT batch_id, lot, roll, date, width, height, fabric_grade FROM row_inspect_sample WHERE batch_id = @batch_id AND lot = @lot"

        Dim rows As New List(Of String)()
        Dim totalHeight As Decimal = 0
        Dim totalRolls As Integer = 0
        Dim totalHeightGrade1 As Decimal = 0
        Dim totalRollsGrade1 As Integer = 0
        Dim totalHeightGrade2 As Decimal = 0
        Dim totalRollsGrade2 As Integer = 0

        Using conn As New SqlConnection(sqlServerConnectionString)
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@batch_id", batchId)
                cmd.Parameters.AddWithValue("@lot", lot)
                Try
                    conn.Open()
                    Dim reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim batchIdValue As String = reader("batch_id").ToString()
                        Dim lotValue As String = reader("lot").ToString()
                        Dim roll As String = reader("roll").ToString()
                        Dim dateValue As String = Convert.ToDateTime(reader("date")).ToString("yyyy-MM-dd")
                        Dim width As String = reader("width").ToString()
                        Dim height As Decimal = Convert.ToDecimal(reader("height"))
                        Dim fabricGrade As String = reader("fabric_grade").ToString()

                        rows.Add($"<tr>
                        <td style='border: 2px solid black;'>{batchIdValue}</td>
                        <td style='border: 2px solid black;'>{lotValue}</td>
                        <td style='border: 2px solid black;'>{roll}</td>
                        <td style='border: 2px solid black;'>{dateValue}</td>
                        <td style='border: 2px solid black;'>{width}</td>
                        <td style='border: 2px solid black;'>{height}</td>
                        <td style='border: 2px solid black;'>{fabricGrade}</td>
                    </tr>")

                        totalHeight += height
                        totalRolls += 1

                        If fabricGrade = "1" Then
                            totalHeightGrade1 += height
                            totalRollsGrade1 += 1
                        ElseIf fabricGrade = "2" Then
                            totalHeightGrade2 += height
                            totalRollsGrade2 += 1
                        End If
                    End While
                Catch ex As Exception
                    MessageBox.Show("Error loading report data: " & ex.Message)
                    Return ""
                End Try
            End Using
        End Using

        ' Calculate the percentage of grade 2 height
        Dim grade2Percentage As Decimal = If(totalHeight > 0, (totalHeightGrade2 / totalHeight) * 100, 0)

        Dim htmlContent As String = $"
<html>
<head>
    <style>
        table {{ width: 100%; border-collapse: collapse; text-align: center; font-size: 16px; font-weight: bold; }}
        table, th, td {{ border: 2px solid black; }}
        th, td {{ padding: 10px; }}
    </style>
</head>
<body>
    <table id='demo2_table' cellpadding='1' cellspacing='1' border='0' width='100%' align='center' dir='rtl'>
        <tr>
            <td><strong>Batch ID رقم الرسالة</strong></td>
            <td><strong>Lot No. رقم اللوت</strong></td>
            <td><strong>Roll No. رقم التوب</strong></td>
            <td><strong>Date التاريخ</strong></td>
            <td><strong>Width عرض التوب</strong></td>
            <td><strong>Height ارتفاع التوب</strong></td>
            <td><strong>Fabric Grade درجة القماش</strong></td>
        </tr>
        {String.Join(Environment.NewLine, rows)}
    </table>
    <br>
    <table id='summary_table' cellpadding='1' cellspacing='1' border='0' width='100%' align='center' dir='rtl'>
        <tr>
            <td><strong>اجمالى الطول</strong></td>
            <td>{totalHeight}</td>
        </tr>
        <tr>
            <td><strong>عدد الأتواب</strong></td>
            <td>{totalRolls}</td>
        </tr>
    </table>
    <script type='text/javascript'>
        window.print();
    </script>
</body>
</html>"

        Return htmlContent
    End Function
    Private Sub btnprint2_Click(sender As Object, e As EventArgs) Handles btnprint2.Click
        Dim batchId As String = cmbbatch.SelectedItem.ToString()
        Dim lot As String = cmblot.SelectedItem.ToString()
        Dim htmlContent As String = GeneratePrintContentForAllRolls(batchId, lot)
        If String.IsNullOrEmpty(htmlContent) Then
            MessageBox.Show("Error generating print content.")
            Return
        End If

        ' Create a temporary HTML file
        Dim tempFilePath As String = IO.Path.Combine(IO.Path.GetTempPath(), "print_all_rolls.html")
        IO.File.WriteAllText(tempFilePath, htmlContent)

        ' Check if the file was created successfully
        If Not IO.File.Exists(tempFilePath) Then
            MessageBox.Show("Error creating HTML file.")
            Return
        End If

        ' Open the HTML file in the default browser
        Try
            Process.Start(New ProcessStartInfo(tempFilePath) With {.UseShellExecute = True})
        Catch ex As Exception
            MessageBox.Show("Error opening HTML file: " & ex.Message)
        End Try
    End Sub

    Private Function GeneratePrintContentForAllRolls(batchId As String, lot As String) As String
        ' Query to get the main data
        Dim mainDataQuery As String = "SELECT fi.batch_id, fi.lot, MAX(fi.date) AS last_date, br.material " &
                                  "FROM row_inspect_sample fi " &
                                  "LEFT JOIN batch_raw br ON fi.batch_id = br.batch " &
                                  "WHERE fi.batch_id = @batch_id AND fi.lot = @lot " &
                                  "GROUP BY fi.batch_id, fi.lot, br.material"

        ' Query to get the roll details
        Dim rollDetailsQuery As String = "SELECT fi.roll, fi.fabric_grade, fi.width, fi.height, " &
                                     "(SELECT (SUM(point) * 10000) / (fi.width * fi.height) FROM row_inspect_sample_defects WHERE batch_id = fi.batch_id AND lot = fi.lot AND roll = fi.roll) AS defectRate, " &
                                     "fi.notes, us.public_name " &
                                     "FROM row_inspect_sample fi " &
                                     "LEFT JOIN dep_users us ON fi.username = us.username " &
                                     "WHERE fi.batch_id = @batch_id AND fi.lot = @lot"

        ' Query to get the distinct defect names
        Dim defectNamesQuery As String = "SELECT DISTINCT gd.name_ar " &
                                     "FROM row_inspect_sample_defects fid " &
                                     "JOIN gray_defects gd ON fid.defect_id = gd.id " &
                                     "WHERE fid.batch_id = @batch_id AND fid.lot = @lot"

        ' Query to get the defect places for each roll
        Dim defectPlacesQuery As String = "SELECT fi.roll, gd.name_ar, fid.def_place " &
                                      "FROM row_inspect_sample_defects fid " &
                                      "JOIN gray_defects gd ON fid.defect_id = gd.id " &
                                      "JOIN row_inspect_sample fi ON fid.batch_id = fi.batch_id AND fid.lot = fi.lot AND fid.roll = fi.roll " &
                                      "WHERE fid.batch_id = @batch_id AND fid.lot = @lot"

        Dim mainData As String = ""
        Dim rolls As New List(Of String)()
        Dim defectNames As New List(Of String)()
        Dim defectPlaces As New Dictionary(Of String, Dictionary(Of String, String))()
        Dim totalHeight As Decimal = 0
        Dim grade2Height As Decimal = 0
        Dim totalPoints As Decimal = 0
        Dim rollCount As Integer = 0
        Dim totalDefectPoints As Decimal = 0

        Using conn As New SqlConnection(sqlServerConnectionString)
            ' Get the main data
            Using cmd As New SqlCommand(mainDataQuery, conn)
                cmd.Parameters.AddWithValue("@batch_id", batchId)
                cmd.Parameters.AddWithValue("@lot", lot)
                Try
                    conn.Open()
                    Dim reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        Dim lastDate As String = Convert.ToDateTime(reader("last_date")).ToString("yyyy-MM-dd")
                        Dim material As String = reader("material").ToString()

                        mainData = $"<tr>
                        <td style='border: 1px solid black;'>Batch ID</td>
                        <td style='border: 1px solid black;'>{batchId}</td>
                    </tr>
                    <tr>
                        <td style='border: 1px solid black;'>Lot</td>
                        <td style='border: 1px solid black;'>{lot}</td>
                    </tr>
                    <tr>
                        <td style='border: 1px solid black;'>Last Inspection Date</td>
                        <td style='border: 1px solid black;'>{lastDate}</td>
                    </tr>
                    <tr>
                        <td style='border: 1px solid black;'>Material</td>
                        <td style='border: 1px solid black;'>{material}</td>
                    </tr>"
                    End If
                    reader.Close()
                Catch ex As Exception
                    MessageBox.Show("Error loading main data: " & ex.Message)
                    Return ""
                End Try
            End Using

            ' Get the roll details
            Using cmd As New SqlCommand(rollDetailsQuery, conn)
                cmd.Parameters.AddWithValue("@batch_id", batchId)
                cmd.Parameters.AddWithValue("@lot", lot)
                Try
                    Dim reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim roll As String = reader("roll").ToString()
                        Dim width As String = reader("width").ToString()
                        Dim height As Decimal = Convert.ToDecimal(reader("height"))
                        Dim defectRate As String = If(reader("defectRate") IsNot DBNull.Value, Convert.ToDecimal(reader("defectRate")).ToString("F2"), "N/A")
                        Dim notes As String = If(reader("notes") IsNot DBNull.Value, reader("notes").ToString(), "N/A")
                        Dim workerName As String = If(reader("public_name") IsNot DBNull.Value, reader("public_name").ToString(), "N/A")
                        Dim fabricGrade As String = If(reader("fabric_grade") IsNot DBNull.Value, reader("fabric_grade").ToString(), "N/A")

                        rolls.Add($"<tr>
                        <td style='border: 1px solid black;'>{roll}</td>
                        <td style='border: 1px solid black;'>{width}</td>
                        <td style='border: 1px solid black;'>{height}</td>
                        <td style='border: 1px solid black;'>{defectRate}</td>
                        <td style='border: 1px solid black;'>{notes}</td>
                        <td style='border: 1px solid black; white-space: nowrap;'>{workerName}</td>
                        <td style='border: 1px solid black;'>{fabricGrade}</td>")

                        totalHeight += height
                        rollCount += 1
                        If fabricGrade = "2" Then
                            grade2Height += height
                        End If
                        ' Add defect points to total
                        If defectRate <> "N/A" Then
                            totalDefectPoints += Convert.ToDecimal(defectRate)
                        End If
                    End While
                    reader.Close()
                Catch ex As Exception
                    MessageBox.Show("Error loading roll details: " & ex.Message)
                    Return ""
                End Try
            End Using

            ' Get the distinct defect names
            Using cmd As New SqlCommand(defectNamesQuery, conn)
                cmd.Parameters.AddWithValue("@batch_id", batchId)
                cmd.Parameters.AddWithValue("@lot", lot)
                Try
                    Dim reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        defectNames.Add(reader("name_ar").ToString())
                    End While
                    reader.Close()
                Catch ex As Exception
                    MessageBox.Show("Error loading defect names: " & ex.Message)
                    Return ""
                End Try
            End Using

            ' Get the defect places for each roll
            Using cmd As New SqlCommand(defectPlacesQuery, conn)
                cmd.Parameters.AddWithValue("@batch_id", batchId)
                cmd.Parameters.AddWithValue("@lot", lot)
                Try
                    Dim reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim roll As String = reader("roll").ToString()
                        Dim defectName As String = reader("name_ar").ToString()
                        Dim defPlace As String = reader("def_place").ToString()

                        If Not defectPlaces.ContainsKey(roll) Then
                            defectPlaces(roll) = New Dictionary(Of String, String)()
                        End If
                        defectPlaces(roll)(defectName) = defPlace
                    End While
                    reader.Close()
                Catch ex As Exception
                    MessageBox.Show("Error loading defect places: " & ex.Message)
                    Return ""
                End Try
            End Using

            ' Calculate total points
            Dim totalPointsQuery As String = "SELECT SUM(fid.point) * 10000 / (SUM(fi.height) * MAX(fi.width)) AS TotalPoints " &
                                         "FROM row_inspect_sample_defects fid " &
                                         "JOIN row_inspect_sample fi ON fid.batch_id = fi.batch_id AND fid.lot = fi.lot AND fid.roll = fi.roll " &
                                         "WHERE fid.batch_id = @batch_id AND fid.lot = @lot"

            Using cmd As New SqlCommand(totalPointsQuery, conn)
                cmd.Parameters.AddWithValue("@batch_id", batchId)
                cmd.Parameters.AddWithValue("@lot", lot)
                Try
                    totalPoints = Convert.ToDecimal(cmd.ExecuteScalar())
                Catch ex As Exception
                    MessageBox.Show("Error calculating total points: " & ex.Message)
                    Return ""
                End Try
            End Using
        End Using

        ' Calculate the percentage of fabric_grade 2 height
        Dim grade2Percentage As Decimal = If(totalHeight > 0, (grade2Height / totalHeight) * 100, 0)

        ' Construct the HTML content
        Dim htmlContent As String = $"
<html>
<head>
    <style>
        table {{ width: 100%; border-collapse: collapse; font-size: 16px; font-weight: bold; }}
        table, th, td {{ border: 1px solid black; }}
        th, td {{ padding: 10px; text-align: left; }}
    </style>
</head>
<body>
    <center><h2>Raw INSPECTION REPORT تقرير فحص نسبه ال %10</h2></center>
    <center><h3>Quality Control Department</h3></center>
    <center><h3>Total Defect Points: {totalDefectPoints:F2}</h3></center>
    <br>
    <table>
        {mainData}
        <tr>
            <td style='border: 1px solid black;'>Total Points</td>
            <td style='border: 1px solid black;'>{totalPoints:F2}</td>
        </tr>
        <tr>
            <td style='border: 1px solid black;'>اجمالى عدد الأتواب</td>
            <td style='border: 1px solid black;'>{rollCount}</td>
        </tr>
        <tr>
            <td style='border: 1px solid black;'>اجمالى الطول</td>
            <td style='border: 1px solid black;'>{totalHeight}</td>
        </tr>
    </table>
    <br>
    <center><h3>Roll Details</h3></center>
    <table>
        <tr>
            <th>Roll Number</th>
            <th>Width</th>
            <th>Height</th>
            <th>Defect Rate</th>
            <th>Notes</th>
            <th>اسم العامل</th>
            <th>درجة التوب</th>"

        ' Add defect names as column headers
        For Each defectName In defectNames
            htmlContent &= $"<th>{defectName}</th>"
        Next

        htmlContent &= "</tr>"

        ' Add roll details and defect places
        For Each roll In rolls
            htmlContent &= roll

            ' Extract the roll number from the HTML row
            Dim rollNumber As String = roll.Split(">"c)(2).Split("<"c)(0)

            For Each defectName In defectNames
                Dim defPlace As String = If(defectPlaces.ContainsKey(rollNumber) AndAlso defectPlaces(rollNumber).ContainsKey(defectName), defectPlaces(rollNumber)(defectName), "")
                htmlContent &= $"<td style='border: 1px solid black;'>{defPlace}</td>"
            Next

            htmlContent &= "</tr>"
        Next

        htmlContent &= "
    </table>
    <script type='text/javascript'>
        window.onload = function() {{
            window.print();
        }};
    </script>
</body>
</html>"

        Return htmlContent
    End Function

    Private Sub CheckAndSetStatus(batchId As String, lot As String)
        If IsStatusAlreadySaved(batchId, lot) Then
            Dim existingStatus As String = GetExistingStatus(batchId, lot)
            cmbStatus.SelectedItem = existingStatus
            cmbStatus.Enabled = False
        Else
            cmbStatus.SelectedItem = Nothing
            cmbStatus.Enabled = True
        End If
    End Sub
    Private Sub btnSaveStatus_Click(sender As Object, e As EventArgs) Handles btnSaveStatus.Click
        Dim batchId As String = cmbbatch.SelectedItem.ToString()
        Dim lot As String = cmblot.SelectedItem.ToString()
        Dim status As String = cmbStatus.SelectedItem.ToString()
        Dim allowedUsers As String() = {"mostafa", "mirna"}

        If String.IsNullOrEmpty(batchId) Or String.IsNullOrEmpty(lot) Or String.IsNullOrEmpty(status) Then
            MessageBox.Show("برجاء اختيار الرسالة واللوت والحالة.")
            Return
        End If

        If Not allowedUsers.Contains(LoggedInUsername.ToLower()) Then
            MessageBox.Show("ليس لديك الصلاحية لتسجيل الحالة.")
            Return
        End If

        If IsStatusAlreadySaved(batchId, lot) Then
            Dim existingStatus As String = GetExistingStatus(batchId, lot)
            MessageBox.Show("تم تسجيل الحالة مسبقاً كـ " & existingStatus)
            Return
        End If

        SaveStatus(batchId, lot, status)
        Loadstatus() ' Refresh dgvstatus after saving the status
    End Sub

    Private Sub SaveStatus(batchId As String, lot As String, status As String)
        Dim statusValue As Integer = If(status = "مقبولة", 0, 1)
        Dim query As String = "INSERT INTO batch_lot_status (batch_id, lot, status) VALUES (@batch_id, @lot, @status)"

        Using conn As New SqlConnection(sqlServerConnectionString)
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@batch_id", batchId)
                cmd.Parameters.AddWithValue("@lot", lot)
                cmd.Parameters.AddWithValue("@status", statusValue)
                Try
                    conn.Open()
                    cmd.ExecuteNonQuery()
                    MessageBox.Show("تم حفظ الحالة بنجاح.")
                Catch ex As Exception
                    MessageBox.Show("Error saving status: " & ex.Message)
                End Try
            End Using
        End Using
    End Sub


    Private Function IsStatusAlreadySaved(batchId As String, lot As String) As Boolean
        Dim query As String = "SELECT COUNT(*) FROM batch_lot_status WHERE batch_id = @batch_id AND lot = @lot"
        Using conn As New SqlConnection(sqlServerConnectionString)
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@batch_id", batchId)
                cmd.Parameters.AddWithValue("@lot", lot)
                Try
                    conn.Open()
                    Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                    Return count > 0
                Catch ex As Exception
                    MessageBox.Show("Error checking status: " & ex.Message)
                    Return False
                End Try
            End Using
        End Using
    End Function

    Private Function GetExistingStatus(batchId As String, lot As String) As String
        Dim query As String = "SELECT status FROM batch_lot_status WHERE batch_id = @batch_id AND lot = @lot"
        Using conn As New SqlConnection(sqlServerConnectionString)
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@batch_id", batchId)
                cmd.Parameters.AddWithValue("@lot", lot)
                Try
                    conn.Open()
                    Dim statusValue As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                    Return If(statusValue = 0, "مقبولة", "مرفوضة")
                Catch ex As Exception
                    MessageBox.Show("Error retrieving existing status: " & ex.Message)
                    Return String.Empty
                End Try
            End Using
        End Using
    End Function


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


End Class
