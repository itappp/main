Imports System.Data.SqlClient
Imports Microsoft.Office.Interop

Public Class colorsreportform
    Private txtCodeSearch As New TextBox()
    Private txtNameSearch As New TextBox()
    Private btnSearch As New Button()
    Private dgvResults As New DataGridView()
    Private connectionString As String = "Server=localhost;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"
    Private pnlTop As New Panel()
    Private btnExport As New Button()

    Private Sub colorsreportform_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "تقرير الألوان"
        Me.WindowState = FormWindowState.Maximized

        ' إعداد Panel لأدوات البحث
        pnlTop.Dock = DockStyle.Top
        pnlTop.Height = 70
        pnlTop.BackColor = Color.WhiteSmoke

        txtCodeSearch.Location = New Point(50, 20)
        txtCodeSearch.Width = 200
        txtNameSearch.Location = New Point(270, 20)
        txtNameSearch.Width = 200
        btnSearch.Text = "بحث"
        btnSearch.Location = New Point(490, 20)
        btnSearch.Width = 100
        AddHandler btnSearch.Click, AddressOf btnSearch_Click

        btnExport.Text = "تصدير إلى Excel"
        btnExport.Location = New Point(610, 20)
        btnExport.Width = 150
        AddHandler btnExport.Click, AddressOf btnExport_Click

        pnlTop.Controls.Add(txtCodeSearch)
        pnlTop.Controls.Add(txtNameSearch)
        pnlTop.Controls.Add(btnSearch)
        pnlTop.Controls.Add(btnExport)
        Me.Controls.Add(pnlTop)

        dgvResults.Dock = DockStyle.Fill
        dgvResults.ReadOnly = True
        dgvResults.AllowUserToAddRows = False
        dgvResults.AllowUserToDeleteRows = False
        dgvResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvResults.RowTemplate.Height = 35
        dgvResults.GridColor = Color.Gray
        dgvResults.BorderStyle = BorderStyle.Fixed3D
        dgvResults.BackgroundColor = Color.White
        dgvResults.DefaultCellStyle.SelectionBackColor = Color.LightBlue
        dgvResults.DefaultCellStyle.SelectionForeColor = Color.Black

        Dim headerStyle As New DataGridViewCellStyle()
        headerStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        headerStyle.Font = New Font("Arial", 16, FontStyle.Bold)
        headerStyle.BackColor = Color.LightSteelBlue
        headerStyle.ForeColor = Color.Black
        dgvResults.ColumnHeadersDefaultCellStyle = headerStyle
        dgvResults.EnableHeadersVisualStyles = False

        Dim cellStyle As New DataGridViewCellStyle()
        cellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        cellStyle.Font = New Font("Arial", 14, FontStyle.Bold)
        cellStyle.ForeColor = Color.Black
        cellStyle.BackColor = Color.White
        dgvResults.DefaultCellStyle = cellStyle

        Me.Controls.Add(dgvResults)

        LoadData()
        SetPlaceholders()
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs)
        LoadData()
    End Sub

    Private Sub LoadData()
        Dim codeFilter As String = txtCodeSearch.Text.Trim()
        Dim nameFilter As String = txtNameSearch.Text.Trim()

        Dim query As String = "SELECT code AS [كود اللون], name AS [اسم اللون], notes AS [ملاحظات] FROM color_code WHERE 1=1"
        If codeFilter <> "" Then
            query &= " AND code LIKE @code"
        End If
        If nameFilter <> "" Then
            query &= " AND name LIKE @name"
        End If

        Using con As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(query, con)
                If codeFilter <> "" Then
                    cmd.Parameters.AddWithValue("@code", "%" & codeFilter & "%")
                End If
                If nameFilter <> "" Then
                    cmd.Parameters.AddWithValue("@name", "%" & nameFilter & "%")
                End If
                Dim dt As New DataTable()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                dgvResults.DataSource = dt
            End Using
        End Using
    End Sub

    Private Sub SetPlaceholders()
        txtCodeSearch.Text = "بحث بالكود"
        txtCodeSearch.ForeColor = Color.Gray
        AddHandler txtCodeSearch.Enter, AddressOf RemovePlaceholderCode
        AddHandler txtCodeSearch.Leave, AddressOf SetPlaceholderCode

        txtNameSearch.Text = "بحث بالاسم"
        txtNameSearch.ForeColor = Color.Gray
        AddHandler txtNameSearch.Enter, AddressOf RemovePlaceholderName
        AddHandler txtNameSearch.Leave, AddressOf SetPlaceholderName
    End Sub

    Private Sub RemovePlaceholderCode(sender As Object, e As EventArgs)
        If txtCodeSearch.Text = "بحث بالكود" Then
            txtCodeSearch.Text = ""
            txtCodeSearch.ForeColor = Color.Black
        End If
    End Sub

    Private Sub SetPlaceholderCode(sender As Object, e As EventArgs)
        If txtCodeSearch.Text = "" Then
            txtCodeSearch.Text = "بحث بالكود"
            txtCodeSearch.ForeColor = Color.Gray
        End If
    End Sub

    Private Sub RemovePlaceholderName(sender As Object, e As EventArgs)
        If txtNameSearch.Text = "بحث بالاسم" Then
            txtNameSearch.Text = ""
            txtNameSearch.ForeColor = Color.Black
        End If
    End Sub

    Private Sub SetPlaceholderName(sender As Object, e As EventArgs)
        If txtNameSearch.Text = "" Then
            txtNameSearch.Text = "بحث بالاسم"
            txtNameSearch.ForeColor = Color.Gray
        End If
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs)
        If dgvResults.Rows.Count = 0 Then
            MessageBox.Show("لا توجد بيانات للتصدير.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        Try
            Dim xlApp As New Excel.Application
            Dim xlWorkBook As Excel.Workbook = xlApp.Workbooks.Add()
            Dim xlWorkSheet As Excel.Worksheet = xlWorkBook.Sheets(1)

            ' تصدير رؤوس الأعمدة
            For col As Integer = 1 To dgvResults.Columns.Count
                xlWorkSheet.Cells(1, col) = dgvResults.Columns(col - 1).HeaderText
            Next

            ' تصدير البيانات
            For row As Integer = 0 To dgvResults.Rows.Count - 1
                For col As Integer = 0 To dgvResults.Columns.Count - 1
                    xlWorkSheet.Cells(row + 2, col + 1) = dgvResults.Rows(row).Cells(col).Value?.ToString()
                Next
            Next

            ' تنسيق الخط والمحاذاة
            Dim usedRange = xlWorkSheet.UsedRange
            usedRange.Font.Name = "Arial"
            usedRange.Font.Size = 14
            usedRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
            usedRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
            usedRange.Columns.AutoFit()
            usedRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous

            ' حفظ الملف
            Dim sfd As New SaveFileDialog()
            sfd.Filter = "Excel Files|*.xlsx"
            sfd.Title = "حفظ ملف Excel"
            sfd.FileName = "تقرير الألوان.xlsx"
            If sfd.ShowDialog() = DialogResult.OK Then
                xlWorkBook.SaveAs(sfd.FileName)
                MessageBox.Show("تم تصدير البيانات بنجاح!", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
            xlWorkBook.Close(False)
            xlApp.Quit()
        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء التصدير: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class