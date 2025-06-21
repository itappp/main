Imports System.Data.SqlClient
Imports System.Drawing.Printing
Imports System.Net
Imports System.Text

Public Class ContractLotForm
    ' Connection string to SQL Server database
    Private connectionString As String = "Server=localhost;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"

    ' Add this line to declare the event handler
    Private WithEvents cmbBatch As ComboBox
    Private WithEvents cmbLot As ComboBox
    Private WithEvents cmbClient As ComboBox  ' إضافة ComboBox لكود العميل

    Private Sub ContractLotForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set form properties
        Me.Text = "تقرير حالة اللوت"
        Me.RightToLeft = RightToLeft.Yes
        Me.Size = New Size(1200, 700)
        Me.StartPosition = FormStartPosition.CenterScreen

        ' Create search panel
        Dim searchPanel As New Panel()
        searchPanel.Dock = DockStyle.Top
        searchPanel.Height = 130
        searchPanel.BackColor = Color.LightGray

        ' Create Batch search controls
        Dim lblBatch As New Label()
        lblBatch.Text = "رقم الرسالة:"
        lblBatch.Location = New Point(1100, 15)
        lblBatch.AutoSize = True
        lblBatch.Font = New Font("Arial", 12)

        ' Initialize the ComboBox that was declared at class level
        cmbBatch = New ComboBox()
        cmbBatch.Name = "cmbBatch"
        cmbBatch.Location = New Point(900, 15)
        cmbBatch.Width = 180
        cmbBatch.Font = New Font("Arial", 12)
        cmbBatch.DropDownStyle = ComboBoxStyle.DropDown
        cmbBatch.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cmbBatch.AutoCompleteSource = AutoCompleteSource.ListItems

        ' Create Lot search controls
        Dim lblLot As New Label()
        lblLot.Text = "رقم اللوت:"
        lblLot.Location = New Point(1100, 55)
        lblLot.AutoSize = True
        lblLot.Font = New Font("Arial", 12)

        ' Initialize the ComboBox that was declared at class level
        cmbLot = New ComboBox()
        cmbLot.Name = "cmbLot"
        cmbLot.Location = New Point(900, 55)
        cmbLot.Width = 180
        cmbLot.Font = New Font("Arial", 12)
        cmbLot.DropDownStyle = ComboBoxStyle.DropDown
        cmbLot.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cmbLot.AutoCompleteSource = AutoCompleteSource.ListItems

        ' Create Client search controls
        Dim lblClient As New Label()
        lblClient.Text = "كود العميل:"
        lblClient.Location = New Point(1100, 95)  ' تعديل الموقع ليكون تحت اللوت
        lblClient.AutoSize = True
        lblClient.Font = New Font("Arial", 12)

        ' Initialize the ComboBox for client
        cmbClient = New ComboBox()
        cmbClient.Name = "cmbClient"
        cmbClient.Location = New Point(900, 95)  ' تعديل الموقع ليكون تحت اللوت
        cmbClient.Width = 180
        cmbClient.Font = New Font("Arial", 12)
        cmbClient.DropDownStyle = ComboBoxStyle.DropDown
        cmbClient.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cmbClient.AutoCompleteSource = AutoCompleteSource.ListItems

        ' Create Search button
        Dim btnSearch As New Button()
        btnSearch.Text = "بحث"
        btnSearch.Width = 120
        btnSearch.Height = 35
        btnSearch.Location = New Point(760, 50)
        btnSearch.Font = New Font("Arial", 12)
        AddHandler btnSearch.Click, AddressOf SearchData

        ' Create Clear button
        Dim btnClear As New Button()
        btnClear.Text = "مسح"
        btnClear.Width = 120
        btnClear.Height = 35
        btnClear.Location = New Point(620, 50)
        btnClear.Font = New Font("Arial", 12)
        AddHandler btnClear.Click, AddressOf ClearSearch

        ' Create Export button
        Dim btnExport As New Button()
        btnExport.Text = "تصدير إلى Excel"
        btnExport.Width = 120
        btnExport.Height = 35
        btnExport.Location = New Point(480, 50)
        btnExport.Font = New Font("Arial", 12)
        AddHandler btnExport.Click, AddressOf ExportToExcel

        ' Add controls to search panel
        searchPanel.Controls.AddRange(New Control() {lblBatch, cmbBatch, lblLot, cmbLot, lblClient, cmbClient, btnSearch, btnClear, btnExport})

        ' Create and setup the DataGridView
        Dim dgvReport As New DataGridView()
        dgvReport.Name = "dgvReport"
        dgvReport.Dock = DockStyle.Fill
        dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvReport.ReadOnly = True
        dgvReport.RightToLeft = RightToLeft.Yes
        dgvReport.AllowUserToAddRows = False
        dgvReport.AllowUserToDeleteRows = False
        dgvReport.Visible = False ' Hide initially until search is performed

        ' Add controls to form
        Me.Controls.Add(dgvReport)
        Me.Controls.Add(searchPanel)

        ' Load batch numbers into combo box
        LoadBatchNumbers()
        LoadClientCodes()
    End Sub

    ' Add the event handler with the correct declaration
    Private Sub cmbBatch_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbBatch.SelectedIndexChanged
        Try
            ' مسح بيانات اللوت السابقة
            cmbLot.DataSource = Nothing
            cmbLot.Items.Clear()

            ' تحميل بيانات اللوت فقط إذا تم اختيار رسالة
            If cmbBatch.SelectedIndex <> -1 AndAlso cmbBatch.SelectedValue IsNot Nothing Then
                Using conn As New SqlConnection(connectionString)
                    Dim query As String = "SELECT lot FROM batch_details WHERE batch_id = @batch_id ORDER BY lot"
                    Dim cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@batch_id", cmbBatch.SelectedValue)
                    Dim dt As New DataTable()

                    conn.Open()
                    dt.Load(cmd.ExecuteReader())

                    cmbLot.DataSource = dt
                    cmbLot.DisplayMember = "lot"
                    cmbLot.ValueMember = "lot"
                    cmbLot.SelectedIndex = -1
                End Using
            End If
        Catch ex As Exception
            ' لا نعرض رسالة خطأ هنا لأنها قد تظهر عند فتح الفورم
        End Try
    End Sub

    Private Sub SearchData()
        Try
            ' Build the WHERE clause based on search criteria
            Dim whereClause As New List(Of String)
            If cmbLot.SelectedIndex <> -1 Then
                whereClause.Add("bd.lot = @lot")
            End If
            If cmbBatch.SelectedIndex <> -1 Then
                whereClause.Add("bd.batch_id = @batch")
            End If
            If cmbClient.SelectedIndex <> -1 Then
                whereClause.Add("br.client_code = @client")
            End If

            ' Base query
            Dim query As String = "
                SELECT 
                    bd.lot as 'رقم اللوت',
                    bd.batch_id as 'رقم الرسالة',
                    c.code as 'كود العميل',
                    bd.meter_quantity as 'الكمية المتاحة (متر)',
                    bd.weightpk as 'الكمية المتاحة (كجم)',
                    COALESCE(
                        (SELECT SUM(QuantityM) FROM Contracts WHERE lot = bd.lot), 
                        0
                    ) as 'الكمية المستخدمة (متر)',
                    COALESCE(
                        (SELECT SUM(QuantityK) FROM Contracts WHERE lot = bd.lot), 
                        0
                    ) as 'الكمية المستخدمة (كجم)',
                    CASE bls.status 
                        WHEN 0 THEN N'مقبول'
                        WHEN 1 THEN N'مرفوض'
                        WHEN -1 THEN N'غير معروف'
                        ELSE N'غير محدد'
                    END as 'الحالة'
                FROM batch_details bd
                LEFT JOIN batch_raw br ON bd.batch_id = br.batch
                LEFT JOIN batch_lot_status bls ON bd.lot = bls.lot
                LEFT JOIN clients c ON br.client_code = c.id"

            ' Add WHERE clause if search criteria exist
            If whereClause.Count > 0 Then
                query &= " WHERE " & String.Join(" AND ", whereClause)
            End If

            query &= " ORDER BY bd.batch_id, bd.lot"

            Using conn As New SqlConnection(connectionString)
                Dim dt As New DataTable()
                Using adapter As New SqlDataAdapter(query, conn)
                    ' Add parameters if search criteria exist
                    If cmbLot.SelectedIndex <> -1 Then
                        adapter.SelectCommand.Parameters.AddWithValue("@lot", cmbLot.SelectedValue)
                    End If
                    If cmbBatch.SelectedIndex <> -1 Then
                        adapter.SelectCommand.Parameters.AddWithValue("@batch", cmbBatch.SelectedValue)
                    End If
                    If cmbClient.SelectedIndex <> -1 Then
                        adapter.SelectCommand.Parameters.AddWithValue("@client", cmbClient.SelectedValue)
                    End If

                    adapter.Fill(dt)
                End Using

                ' Add calculated columns
                dt.Columns.Add("الكمية المتبقية (متر)", GetType(Decimal), "[الكمية المتاحة (متر)] - [الكمية المستخدمة (متر)]")
                dt.Columns.Add("الكمية المتبقية (كجم)", GetType(Decimal), "[الكمية المتاحة (كجم)] - [الكمية المستخدمة (كجم)]")

                ' Get the DataGridView and update it
                Dim dgvReport As DataGridView = DirectCast(Me.Controls.Find("dgvReport", True)(0), DataGridView)
                dgvReport.DataSource = dt
                dgvReport.Visible = True

                ' Format the DataGridView
                FormatDataGridView(dgvReport)

                If dt.Rows.Count = 0 Then
                    MessageBox.Show("لا توجد نتائج للبحث")
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء البحث: " & ex.Message)
        End Try
    End Sub

    Private Sub ClearSearch()
        cmbBatch.SelectedIndex = -1
        cmbBatch.Text = ""
        cmbLot.DataSource = Nothing
        cmbLot.Items.Clear()
        cmbLot.Text = ""
        cmbClient.SelectedIndex = -1
        cmbClient.Text = ""
        Dim dgvReport As DataGridView = DirectCast(Me.Controls.Find("dgvReport", True)(0), DataGridView)
        dgvReport.DataSource = Nothing
        dgvReport.Visible = False
    End Sub

    Private Sub FormatDataGridView(dgv As DataGridView)
        ' Set column styles
        dgv.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 12, FontStyle.Bold)
        dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue
        dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgv.DefaultCellStyle.Font = New Font("Arial", 10)
        dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        ' Set alternating row colors
        dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray

        ' Add color coding for status
        AddHandler dgv.CellFormatting, AddressOf DataGridView_CellFormatting

        ' Set column widths
        For Each col As DataGridViewColumn In dgv.Columns
            col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        Next
    End Sub

    Private Sub DataGridView_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        Dim dgv As DataGridView = DirectCast(sender, DataGridView)
        If e.ColumnIndex = dgv.Columns("الحالة").Index AndAlso e.Value IsNot Nothing Then
            Select Case e.Value.ToString()
                Case "مقبول"
                    e.CellStyle.BackColor = Color.LightGreen
                Case "مرفوض"
                    e.CellStyle.BackColor = Color.LightPink
                Case "غير معروف"
                    e.CellStyle.BackColor = Color.LightYellow
            End Select
        End If
    End Sub

    Private Sub ExportToExcel()
        Try
            Dim dgvReport As DataGridView = DirectCast(Me.Controls.Find("dgvReport", True)(0), DataGridView)
            If dgvReport.Rows.Count = 0 Then
                MessageBox.Show("لا توجد بيانات للتصدير")
                Return
            End If

            ' Create save file dialog
            Dim saveFileDialog As New SaveFileDialog()
            saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx"
            saveFileDialog.FileName = "تقرير_حالة_اللوت_" & DateTime.Now.ToString("yyyy-MM-dd")

            If saveFileDialog.ShowDialog() = DialogResult.OK Then
                ' Create Excel application
                Dim excel As Object = CreateObject("Excel.Application")
                Dim workbook As Object = excel.Workbooks.Add
                Dim worksheet As Object = workbook.Worksheets(1)

                ' Export headers
                For i As Integer = 0 To dgvReport.Columns.Count - 1
                    worksheet.Cells(1, i + 1) = dgvReport.Columns(i).HeaderText
                Next

                ' Export data
                For i As Integer = 0 To dgvReport.Rows.Count - 1
                    For j As Integer = 0 To dgvReport.Columns.Count - 1
                        worksheet.Cells(i + 2, j + 1) = dgvReport.Rows(i).Cells(j).Value
                    Next
                Next

                ' Auto-fit columns
                worksheet.Columns.AutoFit()

                ' Save and close
                workbook.SaveAs(saveFileDialog.FileName)
                workbook.Close()
                excel.Quit()

                MessageBox.Show("تم تصدير التقرير بنجاح")
            End If
        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء التصدير: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadBatchNumbers()
        Try
            Using conn As New SqlConnection(connectionString)
                Dim query As String = "SELECT DISTINCT batch_id FROM batch_details ORDER BY batch_id"
                Dim cmd As New SqlCommand(query, conn)
                Dim dt As New DataTable()

                conn.Open()
                dt.Load(cmd.ExecuteReader())

                cmbBatch.DataSource = dt
                cmbBatch.DisplayMember = "batch_id"
                cmbBatch.ValueMember = "batch_id"
                cmbBatch.SelectedIndex = -1
            End Using
        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء تحميل أرقام الرسائل: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadClientCodes()
        Try
            Using conn As New SqlConnection(connectionString)
                Dim query As String = "SELECT id, code FROM clients ORDER BY code"
                Dim cmd As New SqlCommand(query, conn)
                Dim dt As New DataTable()

                conn.Open()
                dt.Load(cmd.ExecuteReader())

                cmbClient.DataSource = dt
                cmbClient.DisplayMember = "code"
                cmbClient.ValueMember = "id"
                cmbClient.SelectedIndex = -1
            End Using
        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء تحميل أكواد العملاء: " & ex.Message)
        End Try
    End Sub
End Class