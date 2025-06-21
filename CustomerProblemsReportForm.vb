Imports System.Data.SqlClient
Imports ClosedXML.Excel

Public Class CustomerProblemsReportForm
    Private connectionString As String = "Server=localhost;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"
    Private dgvReport As New DataGridView()
    Private cmbRef As New ComboBox()
    Private cmbContractNo As New ComboBox()
    Private cmbClientCode As New ComboBox()
    Private dtpFrom As New DateTimePicker()
    Private dtpTo As New DateTimePicker()
    Private btnSearch As New Button()
    Private btnExportExcel As New Button()
    Private lblRef As New Label()
    Private lblContractNo As New Label()
    Private lblClientCode As New Label()
    Private lblFrom As New Label()
    Private lblTo As New Label()

    Private Sub CustomerProblemsReportForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.WindowState = FormWindowState.Maximized
        Me.Text = "تقرير مشاكل العملاء"

        ' إعداد الكنترولز
        lblRef.Text = "رقم الإذن:"
        lblRef.Location = New Point(10, 10)
        lblRef.AutoSize = True
        Me.Controls.Add(lblRef)
        cmbRef.Location = New Point(lblRef.Right + 5, 10)
        cmbRef.Width = 120
        cmbRef.DropDownStyle = ComboBoxStyle.DropDown
        cmbRef.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cmbRef.AutoCompleteSource = AutoCompleteSource.ListItems
        Me.Controls.Add(cmbRef)

        lblContractNo.Text = "رقم التعاقد:"
        lblContractNo.Location = New Point(cmbRef.Right + 20, 10)
        lblContractNo.AutoSize = True
        Me.Controls.Add(lblContractNo)
        cmbContractNo.Location = New Point(lblContractNo.Right + 5, 10)
        cmbContractNo.Width = 120
        cmbContractNo.DropDownStyle = ComboBoxStyle.DropDown
        cmbContractNo.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cmbContractNo.AutoCompleteSource = AutoCompleteSource.ListItems
        Me.Controls.Add(cmbContractNo)

        lblClientCode.Text = "كود العميل:"
        lblClientCode.Location = New Point(cmbContractNo.Right + 20, 10)
        lblClientCode.AutoSize = True
        Me.Controls.Add(lblClientCode)
        cmbClientCode.Location = New Point(lblClientCode.Right + 5, 10)
        cmbClientCode.Width = 120
        cmbClientCode.DropDownStyle = ComboBoxStyle.DropDown
        cmbClientCode.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cmbClientCode.AutoCompleteSource = AutoCompleteSource.ListItems
        Me.Controls.Add(cmbClientCode)

        lblFrom.Text = "من تاريخ:"
        lblFrom.Location = New Point(cmbClientCode.Right + 20, 10)
        lblFrom.AutoSize = True
        Me.Controls.Add(lblFrom)
        dtpFrom.Location = New Point(lblFrom.Right + 5, 10)
        dtpFrom.Format = DateTimePickerFormat.Short
        Me.Controls.Add(dtpFrom)

        lblTo.Text = "إلى تاريخ:"
        lblTo.Location = New Point(dtpFrom.Right + 20, 10)
        lblTo.AutoSize = True
        Me.Controls.Add(lblTo)
        dtpTo.Location = New Point(lblTo.Right + 5, 10)
        dtpTo.Format = DateTimePickerFormat.Short
        Me.Controls.Add(dtpTo)

        btnSearch.Text = "بحث"
        btnSearch.Location = New Point(dtpTo.Right + 20, 10)
        btnSearch.Width = 80
        AddHandler btnSearch.Click, AddressOf btnSearch_Click
        Me.Controls.Add(btnSearch)

        btnExportExcel.Text = "تصدير إلى إكسل"
        btnExportExcel.Location = New Point(btnSearch.Right + 20, 10)
        btnExportExcel.Width = 120
        AddHandler btnExportExcel.Click, AddressOf btnExportExcel_Click
        Me.Controls.Add(btnExportExcel)

        dgvReport.Location = New Point(10, 50)
        dgvReport.Width = Me.ClientSize.Width - 40
        dgvReport.Height = Me.ClientSize.Height - 70
        dgvReport.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Bottom
        dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        dgvReport.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvReport.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvReport.DefaultCellStyle.Font = New Font("Segoe UI", 12, FontStyle.Regular)
        dgvReport.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        dgvReport.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        Me.Controls.Add(dgvReport)

        LoadComboBoxes()
        LoadReport()
    End Sub

    Private Sub LoadComboBoxes()
        ' تعبئة cmbRef بالقيم الفريدة
        cmbRef.Items.Clear()
        cmbRef.Items.Add("")
        Using con As New SqlConnection(connectionString)
            con.Open()
            Using cmd As New SqlCommand("SELECT DISTINCT ref FROM CustomerProblems WHERE ref IS NOT NULL AND ref <> '' ORDER BY ref", con)
                Using reader = cmd.ExecuteReader()
                    While reader.Read()
                        cmbRef.Items.Add(reader("ref").ToString())
                    End While
                End Using
            End Using
            ' تعبئة cmbContractNo فقط بأرقام التعاقدات المرتبطة بمشاكل
            cmbContractNo.Items.Clear()
            cmbContractNo.Items.Add("")
            Using cmd2 As New SqlCommand("SELECT DISTINCT c.ContractNo FROM CustomerProblems cp INNER JOIN Contracts c ON cp.ContractID = c.ContractID WHERE c.ContractNo IS NOT NULL AND c.ContractNo <> '' ORDER BY c.ContractNo", con)
                Using reader2 = cmd2.ExecuteReader()
                    While reader2.Read()
                        cmbContractNo.Items.Add(reader2("ContractNo").ToString())
                    End While
                End Using
            End Using
            ' تعبئة cmbClientCode فقط بأكواد العملاء الذين لديهم مشاكل
            cmbClientCode.Items.Clear()
            cmbClientCode.Items.Add("")
            Using cmd3 As New SqlCommand("SELECT DISTINCT cl.code FROM CustomerProblems cp INNER JOIN Clients cl ON cp.clientid = cl.id WHERE cl.code IS NOT NULL AND cl.code <> '' ORDER BY cl.code", con)
                Using reader3 = cmd3.ExecuteReader()
                    While reader3.Read()
                        cmbClientCode.Items.Add(reader3("code").ToString())
                    End While
                End Using
            End Using
        End Using
        cmbRef.SelectedIndex = 0
        cmbContractNo.SelectedIndex = 0
        cmbClientCode.SelectedIndex = 0
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs)
        LoadReport()
    End Sub

    Private Sub btnExportExcel_Click(sender As Object, e As EventArgs)
        If dgvReport.Rows.Count = 0 Then
            MessageBox.Show("لا توجد بيانات للتصدير.")
            Return
        End If
        Using sfd As New SaveFileDialog()
            sfd.Filter = "Excel Files|*.xlsx"
            sfd.Title = "اختر مكان حفظ ملف الإكسل"
            sfd.FileName = "تقرير مشاكل العملاء_" & DateTime.Now.ToString("yyyyMMdd_HHmmss") & ".xlsx"
            If sfd.ShowDialog() = DialogResult.OK Then
                Using wb As New XLWorkbook()
                    Dim ws = wb.Worksheets.Add("تقرير مشاكل العملاء")
                    ' إضافة رؤوس الأعمدة
                    For col = 0 To dgvReport.Columns.Count - 1
                        ws.Cell(1, col + 1).Value = dgvReport.Columns(col).HeaderText
                        ws.Cell(1, col + 1).Style.Font.Bold = True
                        ws.Cell(1, col + 1).Style.Fill.BackgroundColor = XLColor.LightGray
                        ws.Cell(1, col + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                        ws.Cell(1, col + 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin
                    Next
                    ' إضافة البيانات
                    For row = 0 To dgvReport.Rows.Count - 1
                        For col = 0 To dgvReport.Columns.Count - 1
                            ws.Cell(row + 2, col + 1).Value = dgvReport.Rows(row).Cells(col).Value
                            ws.Cell(row + 2, col + 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin
                            ws.Cell(row + 2, col + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                        Next
                    Next
                    ' تنسيق الأعمدة
                    ws.Columns().AdjustToContents()
                    ' تفعيل Wrap Text لعمود Issue
                    Dim issueColIdx As Integer = -1
                    For i = 0 To dgvReport.Columns.Count - 1
                        If dgvReport.Columns(i).Name = "Issue" Then
                            issueColIdx = i + 1
                            Exit For
                        End If
                    Next
                    If issueColIdx > 0 Then
                        ws.Column(issueColIdx).Style.Alignment.WrapText = True
                    End If
                    wb.SaveAs(sfd.FileName)
                End Using
                MessageBox.Show("تم تصدير البيانات إلى إكسل بنجاح!", "تصدير", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End Using
    End Sub

    Private Sub LoadReport()
        Dim query As String = "SELECT cp.ref, c.ContractNo, cp.DateInserted, cp.Issue, u.public_name, cl.code AS ClientCode, " &
            "c.Material, f.fabricType_ar as contracttype, c.color, c.QuantityM, c.QuantityK, " &
            "cp.qcreply, cp.dateqc, cp.qcuser, cp.status " &
            "FROM CustomerProblems cp " &
            "LEFT JOIN Contracts c ON cp.ContractID = c.ContractID " &
            "LEFT JOIN dep_users u ON cp.qcuser = u.username " &
            "LEFT JOIN clients cl ON cp.clientid = cl.id " &
            "LEFT JOIN fabric f ON c.contracttype = f.id WHERE 1=1 "

        Dim params As New List(Of SqlParameter)()
        If cmbRef.SelectedIndex > 0 AndAlso cmbRef.Text.Trim <> "" Then
            query &= " AND cp.ref = @ref"
            params.Add(New SqlParameter("@ref", cmbRef.Text.Trim))
        End If
        If cmbContractNo.SelectedIndex > 0 AndAlso cmbContractNo.Text.Trim <> "" Then
            query &= " AND c.ContractNo = @contractno"
            params.Add(New SqlParameter("@contractno", cmbContractNo.Text.Trim))
        End If
        If cmbClientCode.SelectedIndex > 0 AndAlso cmbClientCode.Text.Trim <> "" Then
            query &= " AND cl.code = @clientcode"
            params.Add(New SqlParameter("@clientcode", cmbClientCode.Text.Trim))
        End If
        If dtpFrom.Value.Date <= dtpTo.Value.Date Then
            query &= " AND cp.DateInserted >= @fromdate AND cp.DateInserted <= @todate"
            params.Add(New SqlParameter("@fromdate", dtpFrom.Value.Date))
            params.Add(New SqlParameter("@todate", dtpTo.Value.Date.AddDays(1).AddSeconds(-1)))
        End If

        Dim dt As New DataTable()
        Using con As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(query, con)
                cmd.Parameters.AddRange(params.ToArray())
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
            End Using
        End Using
        dgvReport.DataSource = dt

        ' تفعيل Wrap Text لعمود Issue
        If dgvReport.Columns.Contains("Issue") Then
            dgvReport.Columns("Issue").DefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvReport.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        End If
    End Sub
End Class
