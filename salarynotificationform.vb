Imports System.Data
Imports System.Data.OleDb
Imports System.IO
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.Threading
Imports System.Windows.Forms.SendKeys
Imports OfficeOpenXml
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Linq

Public Class salarynotificationform
    Private employeesData As DataTable
    Private currentMessageIndex As Integer = 0
    Private WithEvents messageTimer As New System.Windows.Forms.Timer()
    Private WithEvents txtMessageTemplate As New TextBox()
    Private WithEvents lblMessageTemplate As New Label()
    Private WithEvents panelTop As New Panel()
    Private WithEvents panelBottom As New Panel()
    Private WithEvents lblPreview As New Label()

    ' Fixed parts of the message
    Private Const MESSAGE_PREFIX As String = "عزيزى الموظف {name}، "
    Private Const MESSAGE_SUFFIX As String = ". شكرا لك على المجهود المبذول ونتمنى الأفضل"

    Public Sub New()
        ' Set EPPlus license context
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial
        InitializeComponent()
        InitializeDataTable()
        SetupFormLayout()
        SetupTimer()
    End Sub

    Private Sub SetupTimer()
        messageTimer.Interval = 5000 ' 5 seconds between messages
        messageTimer.Enabled = False
    End Sub

    Private Sub SetupFormLayout()
        ' Set form properties
        Me.WindowState = FormWindowState.Maximized
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.Text = "Salary Notification System"
        Me.MinimumSize = New Size(800, 600)

        ' Configure top panel
        panelTop.Dock = DockStyle.Top
        panelTop.Height = 200
        panelTop.Padding = New Padding(20)
        Me.Controls.Add(panelTop)

        ' Configure bottom panel
        panelBottom.Dock = DockStyle.Bottom
        panelBottom.Height = 60
        panelBottom.Padding = New Padding(10)
        Me.Controls.Add(panelBottom)

        ' Configure message template label
        lblMessageTemplate.Text = "الجزء القابل للتعديل من الرسالة:"
        lblMessageTemplate.AutoSize = True
        lblMessageTemplate.Font = New Font(lblMessageTemplate.Font.FontFamily, 12, FontStyle.Bold)
        lblMessageTemplate.Location = New Point(20, 20)
        lblMessageTemplate.RightToLeft = RightToLeft.Yes
        panelTop.Controls.Add(lblMessageTemplate)

        ' Configure message template text box
        txtMessageTemplate.Multiline = True
        txtMessageTemplate.Text = "راتبك لهذا الشهر هو"
        txtMessageTemplate.Font = New Font(txtMessageTemplate.Font.FontFamily, 12)
        txtMessageTemplate.ScrollBars = ScrollBars.Vertical
        txtMessageTemplate.Location = New Point(20, 50)
        txtMessageTemplate.Width = panelTop.Width - 40
        txtMessageTemplate.Height = 40
        txtMessageTemplate.RightToLeft = RightToLeft.Yes
        AddHandler txtMessageTemplate.TextChanged, AddressOf UpdateMessagePreview
        panelTop.Controls.Add(txtMessageTemplate)

        ' Configure preview label
        lblPreview.Text = "معاينة الرسالة:"
        lblPreview.AutoSize = True
        lblPreview.Font = New Font(lblPreview.Font.FontFamily, 12, FontStyle.Bold)
        lblPreview.Location = New Point(20, 100)
        lblPreview.RightToLeft = RightToLeft.Yes
        panelTop.Controls.Add(lblPreview)

        ' Configure preview text box
        Dim txtPreview As New TextBox()
        txtPreview.Multiline = True
        txtPreview.Text = GetFullMessage("اسم الموظف", "الراتب")
        txtPreview.Font = New Font(txtPreview.Font.FontFamily, 12)
        txtPreview.ScrollBars = ScrollBars.Vertical
        txtPreview.Location = New Point(20, 130)
        txtPreview.Width = panelTop.Width - 40
        txtPreview.Height = 50
        txtPreview.RightToLeft = RightToLeft.Yes
        txtPreview.ReadOnly = True
        txtPreview.Name = "txtPreview"
        panelTop.Controls.Add(txtPreview)

        ' Configure buttons in bottom panel
        btnUpload.Text = "تحميل الملف"
        btnUpload.Width = 120
        btnUpload.Height = 40
        btnUpload.Font = New Font(btnUpload.Font.FontFamily, 12)
        btnUpload.Location = New Point(20, 10)
        panelBottom.Controls.Add(btnUpload)

        btnSend.Text = "إرسال الرسائل"
        btnSend.Width = 120
        btnSend.Height = 40
        btnSend.Font = New Font(btnSend.Font.FontFamily, 12)
        btnSend.Location = New Point(160, 10)
        panelBottom.Controls.Add(btnSend)

        btnDownloadSample.Text = "تحميل النموذج"
        btnDownloadSample.Width = 120
        btnDownloadSample.Height = 40
        btnDownloadSample.Font = New Font(btnDownloadSample.Font.FontFamily, 12)
        btnDownloadSample.Location = New Point(300, 10)
        panelBottom.Controls.Add(btnDownloadSample)

        ' زر إنشاء الصور
        Dim btnGenerateImages As New Button()
        btnGenerateImages.Text = "إنشاء صور الموظفين"
        btnGenerateImages.Width = 160
        btnGenerateImages.Height = 40
        btnGenerateImages.Font = New Font(btnDownloadSample.Font.FontFamily, 12)
        btnGenerateImages.Location = New Point(440, 10)
        AddHandler btnGenerateImages.Click, AddressOf btnGenerateImages_Click
        panelBottom.Controls.Add(btnGenerateImages)

        ' زر إرسال الصور عبر واتساب
        Dim btnSendImages As New Button()
        btnSendImages.Text = "إرسال صور الموظفين"
        btnSendImages.Width = 160
        btnSendImages.Height = 40
        btnSendImages.Font = New Font(btnDownloadSample.Font.FontFamily, 12)
        btnSendImages.Location = New Point(610, 10)
        AddHandler btnSendImages.Click, AddressOf btnSendImages_Click
        panelBottom.Controls.Add(btnSendImages)

        ' زر تصدير أرقام الفشل
        Dim btnExportFailed As New Button()
        btnExportFailed.Text = "تصدير أرقام الفشل"
        btnExportFailed.Width = 160
        btnExportFailed.Height = 40
        btnExportFailed.Font = New Font(btnDownloadSample.Font.FontFamily, 12)
        btnExportFailed.Location = New Point(780, 10)
        AddHandler btnExportFailed.Click, AddressOf btnExportFailed_Click
        panelBottom.Controls.Add(btnExportFailed)

        ' Configure DataGridView
        dgvEmployees.Dock = DockStyle.Fill
        dgvEmployees.Font = New Font(dgvEmployees.Font.FontFamily, 12)
        dgvEmployees.RowTemplate.Height = 35
        dgvEmployees.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvEmployees.ColumnHeadersHeight = 40
        dgvEmployees.EnableHeadersVisualStyles = False
        dgvEmployees.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray
        dgvEmployees.ColumnHeadersDefaultCellStyle.Font = New Font(dgvEmployees.Font.FontFamily, 12, FontStyle.Bold)
        dgvEmployees.RowHeadersVisible = False
        dgvEmployees.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvEmployees.MultiSelect = False
        dgvEmployees.AllowUserToAddRows = False
        dgvEmployees.AllowUserToDeleteRows = False
        dgvEmployees.ReadOnly = True
        dgvEmployees.Margin = New Padding(20)
        Me.Controls.Add(dgvEmployees)

        ' Set control order
        Me.Controls.SetChildIndex(panelTop, 0)
        Me.Controls.SetChildIndex(panelBottom, 1)
        Me.Controls.SetChildIndex(dgvEmployees, 2)
    End Sub

    Private Sub InitializeDataTable()
        employeesData = New DataTable()
        employeesData.Columns.Add("ID", GetType(Integer))
        employeesData.Columns.Add("Name", GetType(String))
        employeesData.Columns.Add("Phone", GetType(String))
        employeesData.Columns.Add("Salary", GetType(String))
        employeesData.Columns.Add("Status", GetType(String))

        ' Set up DataGridView columns
        dgvEmployees.AutoGenerateColumns = False
        dgvEmployees.Columns.Clear()

        ' Add ID column
        Dim idColumn As New DataGridViewTextBoxColumn()
        idColumn.Name = "ID"
        idColumn.HeaderText = "رقم"
        idColumn.DataPropertyName = "ID"
        idColumn.Width = 50
        idColumn.Visible = True
        dgvEmployees.Columns.Add(idColumn)

        ' Add Name column
        Dim nameColumn As New DataGridViewTextBoxColumn()
        nameColumn.Name = "Name"
        nameColumn.HeaderText = "الاسم"
        nameColumn.DataPropertyName = "Name"
        nameColumn.Width = 200
        nameColumn.Visible = True
        dgvEmployees.Columns.Add(nameColumn)

        ' Add Phone column
        Dim phoneColumn As New DataGridViewTextBoxColumn()
        phoneColumn.Name = "Phone"
        phoneColumn.HeaderText = "رقم الهاتف"
        phoneColumn.DataPropertyName = "Phone"
        phoneColumn.Width = 150
        phoneColumn.Visible = True
        dgvEmployees.Columns.Add(phoneColumn)

        ' Add Salary column
        Dim salaryColumn As New DataGridViewTextBoxColumn()
        salaryColumn.Name = "Salary"
        salaryColumn.HeaderText = "الراتب"
        salaryColumn.DataPropertyName = "Salary"
        salaryColumn.Width = 150
        salaryColumn.Visible = True
        dgvEmployees.Columns.Add(salaryColumn)

        ' Add Status column
        Dim statusColumn As New DataGridViewTextBoxColumn()
        statusColumn.Name = "Status"
        statusColumn.HeaderText = "الحالة"
        statusColumn.DataPropertyName = "Status"
        statusColumn.Width = 200
        statusColumn.Visible = True
        dgvEmployees.Columns.Add(statusColumn)

        ' Additional DataGridView settings
        dgvEmployees.Visible = True
        dgvEmployees.BringToFront()
        dgvEmployees.Enabled = True
        dgvEmployees.AllowUserToResizeRows = True
        dgvEmployees.AllowUserToResizeColumns = True
        dgvEmployees.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        dgvEmployees.DefaultCellStyle.WrapMode = DataGridViewTriState.True

        ' Bind the DataTable to DataGridView
        dgvEmployees.DataSource = employeesData
    End Sub

    Private Sub btnSend_Click(sender As Object, e As EventArgs) Handles btnSend.Click
        If employeesData.Rows.Count = 0 Then
            MessageBox.Show("Please upload employee data first!", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        currentMessageIndex = 0
        SendNextMessage()
        messageTimer.Enabled = True
    End Sub

    Private Sub messageTimer_Tick(sender As Object, e As EventArgs) Handles messageTimer.Tick
        SendNextMessage()
    End Sub

    Private Sub UpdateMessagePreview(sender As Object, e As EventArgs)
        Dim previewTextBox As TextBox = DirectCast(panelTop.Controls.Find("txtPreview", True)(0), TextBox)
        previewTextBox.Text = GetFullMessage("اسم الموظف", "الراتب")
    End Sub

    Private Function GetFullMessage(name As String, salary As String) As String
        Return MESSAGE_PREFIX.Replace("{name}", name) & txtMessageTemplate.Text & " {salary}" & MESSAGE_SUFFIX
    End Function

    Private Sub SendNextMessage()
        If currentMessageIndex >= employeesData.Rows.Count Then
            messageTimer.Enabled = False
            MessageBox.Show("تم إرسال جميع الرسائل!", "اكتمل", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Try
            Dim row As DataRow = employeesData.Rows(currentMessageIndex)
            ' استخراج رقم الهاتف
            Dim phone As String = ""
            If employeesData.Columns.Contains("رقم التليفون") Then
                phone = row("رقم التليفون").ToString().Trim()
            ElseIf employeesData.Columns.Contains("Phone") Then
                phone = row("Phone").ToString().Trim()
            End If

            ' استخراج الاسم
            Dim name As String = ""
            If employeesData.Columns.Contains("الاسم") Then
                name = row("الاسم").ToString().Trim()
            ElseIf employeesData.Columns.Contains("Name") Then
                name = row("Name").ToString().Trim()
            End If

            ' استخراج الراتب
            Dim salary As String = ""
            If employeesData.Columns.Contains("الراتب المستحق") Then
                salary = row("الراتب المستحق").ToString().Trim()
            ElseIf employeesData.Columns.Contains("Salary") Then
                salary = row("Salary").ToString().Trim()
            End If

            ' Format Egyptian phone number
            If phone.StartsWith("01") AndAlso phone.Length = 11 Then
                phone = "+20" & phone.Substring(1)
            ElseIf phone.StartsWith("+20") AndAlso phone.Length = 13 Then
                ' Already in international format
            Else
                row("Status") = "فشل - رقم هاتف غير صالح"
                currentMessageIndex += 1
                Return
            End If

            ' Format message using the template
            Dim message As String = GetFullMessage(name, salary)
            message = message.Replace("{salary}", salary)

            Try
                ' Open WhatsApp Desktop with the message
                Process.Start($"whatsapp://send?phone={phone}&text={Uri.EscapeDataString(message)}")

                ' Wait for WhatsApp to open
                Thread.Sleep(2000)

                ' Simulate pressing Enter key to send the message
                SendKeys.SendWait("{ENTER}")
                row("Status") = "تم الإرسال"
            Catch ex As Exception
                row("Status") = "فشل - خطأ في واتساب"
            End Try

            dgvEmployees.Refresh()
            currentMessageIndex += 1

        Catch ex As Exception
            employeesData.Rows(currentMessageIndex)("Status") = "فشل - خطأ"
            currentMessageIndex += 1
        End Try
    End Sub

    Private Sub dgvEmployees_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvEmployees.CellFormatting
        If e.ColumnIndex = dgvEmployees.Columns("Status").Index AndAlso e.Value IsNot Nothing Then
            Dim status As String = e.Value.ToString()
            Select Case status
                Case "تم الإرسال"
                    e.CellStyle.BackColor = Color.LightGreen
                    e.CellStyle.ForeColor = Color.Black
                Case "فشل - رقم هاتف غير صالح", "فشل - خطأ في واتساب", "فشل - خطأ"
                    e.CellStyle.BackColor = Color.LightPink
                    e.CellStyle.ForeColor = Color.Black
                Case Else
                    e.CellStyle.BackColor = Color.White
                    e.CellStyle.ForeColor = Color.Black
            End Select
        End If
    End Sub

    Private Sub btnDownloadSample_Click(sender As Object, e As EventArgs) Handles btnDownloadSample.Click
        Try
            Using saveFileDialog As New SaveFileDialog()
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv"
                saveFileDialog.FileName = "SalaryTemplate.csv"

                If saveFileDialog.ShowDialog() = DialogResult.OK Then
                    ' Create CSV file with headers and sample data
                    Using writer As New StreamWriter(saveFileDialog.FileName)
                        ' Write headers
                        writer.WriteLine("ID,Name,Phone,Salary")
                        ' Write sample data
                        writer.WriteLine("1,John Doe,1234567890,5000.00")
                        writer.WriteLine("2,Jane Smith,1234567891,6000.00")
                        writer.WriteLine("3,Ahmed Ali,1234567892,7000.00")
                    End Using

                    MessageBox.Show("تم تنزيل نموذج الملف بنجاح! يمكنك فتح هذا الملف في Excel.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("خطأ في إنشاء الملف النموذجي: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnUpload_Click(sender As Object, e As EventArgs) Handles btnUpload.Click
        openFileDialog.Filter = "Excel/CSV Files (*.xlsx;*.xls;*.csv)|*.xlsx;*.xls;*.csv|Excel Files (*.xlsx;*.xls)|*.xlsx;*.xls|CSV Files (*.csv)|*.csv"
        If openFileDialog.ShowDialog() = DialogResult.OK Then
            Try
                Dim fileExtension As String = Path.GetExtension(openFileDialog.FileName).ToLower()

                ' Clear existing data
                employeesData.Clear()
                employeesData.Columns.Clear()
                dgvEmployees.Columns.Clear()

                Dim headers As New List(Of String)()

                If fileExtension = ".csv" Then
                    ' Handle CSV file
                    Using reader As New StreamReader(openFileDialog.FileName)
                        ' Read header row
                        Dim headerLine As String = reader.ReadLine()
                        headers.AddRange(headerLine.Split(","c))
                        ' Add columns to DataTable and DataGridView
                        For Each h In headers
                            employeesData.Columns.Add(h, GetType(String))
                            Dim col As New DataGridViewTextBoxColumn()
                            col.Name = h
                            col.HeaderText = h
                            col.DataPropertyName = h
                            dgvEmployees.Columns.Add(col)
                        Next
                        ' Add Status column
                        employeesData.Columns.Add("Status", GetType(String))
                        Dim statusCol As New DataGridViewTextBoxColumn()
                        statusCol.Name = "Status"
                        statusCol.HeaderText = "الحالة"
                        statusCol.DataPropertyName = "Status"
                        dgvEmployees.Columns.Add(statusCol)

                        While Not reader.EndOfStream
                            Dim line As String = reader.ReadLine()
                            Dim values As String() = line.Split(","c)
                            If values.Length >= 1 Then
                                Dim newRow As DataRow = employeesData.NewRow()
                                For i As Integer = 0 To Math.Min(headers.Count, values.Length) - 1
                                    newRow(headers(i)) = values(i)
                                Next
                                newRow("Status") = "Pending"
                                employeesData.Rows.Add(newRow)
                                ' توليد صورة بيانات الموظف
                                Dim employeeDict As New Dictionary(Of String, String)()
                                For i As Integer = 0 To Math.Min(headers.Count, values.Length) - 1
                                    employeeDict(headers(i)) = values(i)
                                Next
                                Dim phoneValue As String = If(newRow.Table.Columns.Contains("رقم التليفون"), newRow("رقم التليفون").ToString(), If(newRow.Table.Columns.Contains("Phone"), newRow("Phone").ToString(), ""))
                                Dim imgPath As String = Path.Combine(Application.StartupPath, $"employee_{phoneValue}.png")
                                CreateEmployeeImage(employeeDict, imgPath)
                            End If
                        End While
                    End Using
                Else
                    ' Handle Excel file using EPPlus
                    Using package As New ExcelPackage(New FileInfo(openFileDialog.FileName))
                        If package.Workbook.Worksheets.Count = 0 Then
                            Throw New Exception("الملف لا يحتوي على أي أوراق عمل.")
                        End If
                        Dim worksheet As ExcelWorksheet = package.Workbook.Worksheets.First()
                        ' قراءة رؤوس الأعمدة
                        Dim col As Integer = 1
                        While worksheet.Cells(1, col).Value IsNot Nothing
                            headers.Add(worksheet.Cells(1, col).Value.ToString())
                            col += 1
                        End While
                        ' Add columns to DataTable and DataGridView
                        For Each h In headers
                            employeesData.Columns.Add(h, GetType(String))
                            Dim colObj As New DataGridViewTextBoxColumn()
                            colObj.Name = h
                            colObj.HeaderText = h
                            colObj.DataPropertyName = h
                            dgvEmployees.Columns.Add(colObj)
                        Next
                        ' Add Status column
                        employeesData.Columns.Add("Status", GetType(String))
                        Dim statusCol As New DataGridViewTextBoxColumn()
                        statusCol.Name = "Status"
                        statusCol.HeaderText = "الحالة"
                        statusCol.DataPropertyName = "Status"
                        dgvEmployees.Columns.Add(statusCol)

                        Dim startRow As Integer = 2 ' Skip header row
                        While worksheet.Cells(startRow, 1).Value IsNot Nothing
                            Dim newRow As DataRow = employeesData.NewRow()
                            For i As Integer = 0 To headers.Count - 1
                                newRow(headers(i)) = If(worksheet.Cells(startRow, i + 1).Value IsNot Nothing, worksheet.Cells(startRow, i + 1).Value.ToString(), "")
                            Next
                            newRow("Status") = "Pending"
                            employeesData.Rows.Add(newRow)
                            ' توليد صورة بيانات الموظف
                            Dim employeeDict As New Dictionary(Of String, String)()
                            For i As Integer = 0 To headers.Count - 1
                                employeeDict(headers(i)) = If(worksheet.Cells(startRow, i + 1).Value IsNot Nothing, worksheet.Cells(startRow, i + 1).Value.ToString(), "")
                            Next
                            Dim phoneValue As String = If(newRow.Table.Columns.Contains("رقم التليفون"), newRow("رقم التليفون").ToString(), If(newRow.Table.Columns.Contains("Phone"), newRow("Phone").ToString(), ""))
                            Dim imgPath As String = Path.Combine(Application.StartupPath, $"employee_{phoneValue}.png")
                            CreateEmployeeImage(employeeDict, imgPath)
                            startRow += 1
                        End While
                    End Using
                End If

                ' Force DataGridView to update
                dgvEmployees.DataSource = Nothing
                dgvEmployees.DataSource = employeesData
                dgvEmployees.Refresh()
                dgvEmployees.Update()
                dgvEmployees.BringToFront()

                ' Enable the send button
                btnSend.Enabled = True

                MessageBox.Show($"تم تحميل {employeesData.Rows.Count} سجل بنجاح!", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Catch ex As Exception
                MessageBox.Show("خطأ في تحميل الملف: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    ' دالة إنشاء صورة بيانات موظف (الاسم ورقم الهاتف والكود والقسم في عنوان، طريقة القبض في جزء منفصل)
    Private Sub CreateEmployeeImage(employeeData As Dictionary(Of String, String), outputPath As String)
        Dim maxCols As Integer = 7
        Dim maxDataColumns As Integer = 32 ' الحد الأقصى لعدد الأعمدة في الصورة

        ' قوائم الأعمدة للاستحقاقات والاستقطاعات
        Dim earningsKeys As New List(Of String) From {
            "اضافى نهارى", "اضافى ليلى", "اضافى عطلات", "اجمالى الحافز", " اخرى بالزيادة", "ملاحظات", "اجمالى الاستحقاقات"
        }
        Dim deductionsKeys As New List(Of String) From {
            "التأمينات", "الضريبة", "سلفة15الشهر", "سلفة مقسطة", "الكشف الطبى", "ايصال خزينة", "عدد أيام الغياب بدون اذن", "خصم قيمة اليوم", "خصم حافز الانتظام", "أخرى بالناقص", "الملاحظات", "اجمالى الاستقطاعات"
        }

        ' تجهيز نسخة آمنة من البيانات
        Dim safeEmployeeData As New Dictionary(Of String, String)
        For Each kvp In employeeData
            Dim safeValue As String
            If String.IsNullOrWhiteSpace(kvp.Value) Then
                safeValue = ""
            ElseIf kvp.Value.Length > 100 Then
                safeValue = kvp.Value.Substring(0, 100) & "..."
            Else
                safeValue = kvp.Value
            End If
            safeEmployeeData(kvp.Key) = safeValue
        Next

        ' استخراج البيانات للعنوان والجزء الأوسط
        Dim employeeName As String = ""
        Dim employeePhone As String = ""
        Dim employeeCode As String = ""
        Dim employeeDepartment As String = ""
        Dim employeePaymentMethod As String = ""
        Dim employeeTotalWage As String = ""
        Dim dataKeys As New List(Of String)

        For Each kvp In safeEmployeeData
            Select Case kvp.Key
                Case "الاسم", "Name"
                    employeeName = kvp.Value
                Case "رقم التليفون", "Phone"
                    employeePhone = kvp.Value
                Case "الكود", "Code"
                    employeeCode = kvp.Value
                Case "القسم", "Department"
                    employeeDepartment = kvp.Value
                Case "طريقة القبض", "Payment Method"
                    employeePaymentMethod = kvp.Value
                Case "الاجر الشامل", "Total Wage"
                    employeeTotalWage = kvp.Value
                Case Else
                    If Not (kvp.Key = "الاسم" OrElse kvp.Key = "Name" OrElse
                            kvp.Key = "رقم التليفون" OrElse kvp.Key = "Phone" OrElse
                            kvp.Key = "الكود" OrElse kvp.Key = "Code" OrElse
                            kvp.Key = "القسم" OrElse kvp.Key = "Department" OrElse
                            kvp.Key = "طريقة القبض" OrElse kvp.Key = "Payment Method" OrElse
                            kvp.Key = "الاجر الشامل" OrElse kvp.Key = "Total Wage") Then
                        If dataKeys.Count < maxDataColumns Then
                            dataKeys.Add(kvp.Key)
                        End If
                    End If
            End Select
        Next

        ' تقسيم الأعمدة
        Dim earningsList As New List(Of String)
        Dim deductionsList As New List(Of String)
        Dim otherList As New List(Of String)
        For Each key In dataKeys
            If earningsKeys.Contains(key) Then
                earningsList.Add(key)
            ElseIf deductionsKeys.Contains(key) Then
                deductionsList.Add(key)
            Else
                otherList.Add(key)
            End If
        Next

        ' حساب أبعاد الصورة
        Dim cellWidth As Integer = 250
        Dim cellHeight As Integer = 70
        Dim headerHeight As Integer = 130
        Dim paymentMethodHeight As Integer = 80
        Dim sectionSpacing As Integer = 10
        Dim tableSpacing As Integer = 10
        Dim y As Integer = headerHeight + paymentMethodHeight + sectionSpacing

        ' حساب عرض الجداول
        Dim maxColsPerGroup As Integer = maxCols
        Dim deductionsWidth As Integer = Math.Min(deductionsList.Count + 1, maxColsPerGroup) * cellWidth
        Dim earningsWidth As Integer = Math.Min(earningsList.Count + 1, maxColsPerGroup) * cellWidth
        Dim otherWidth As Integer = Math.Min(otherList.Count, maxColsPerGroup) * cellWidth
        Dim maxTableWidth As Integer = Math.Max(Math.Max(deductionsWidth, earningsWidth), otherWidth)
        If maxTableWidth < 600 Then maxTableWidth = 600

        ' حساب ارتفاع الصورة
        Dim totalHeight As Integer = headerHeight + paymentMethodHeight + sectionSpacing * 2
        If deductionsList.Count > 0 Then
            Dim numGroups As Integer = Math.Ceiling((deductionsList.Count + 1) / maxColsPerGroup)
            totalHeight += (numGroups * cellHeight * 2) + (numGroups - 1) * tableSpacing + sectionSpacing * 2
        End If
        If earningsList.Count > 0 Then
            Dim numGroups As Integer = Math.Ceiling((earningsList.Count + 1) / maxColsPerGroup)
            totalHeight += (numGroups * cellHeight * 2) + (numGroups - 1) * tableSpacing + sectionSpacing * 2
        End If
        If otherList.Count > 0 Then
            Dim numGroups As Integer = Math.Ceiling(otherList.Count / maxColsPerGroup)
            totalHeight += (numGroups * cellHeight * 2) + (numGroups - 1) * tableSpacing + sectionSpacing * 2
        End If

        ' إضافة هامش إضافي في نهاية الصورة

        ' تنظيف اسم الملف والتأكد من وجود المجلد
        Dim dir = System.IO.Path.GetDirectoryName(outputPath)
        If Not System.IO.Directory.Exists(dir) Then
            System.IO.Directory.CreateDirectory(dir)
        End If
        Dim fileNameOnly = System.IO.Path.GetFileName(outputPath)
        Dim safeFileName = CleanFileName(fileNameOnly)
        Dim safeOutputPath = System.IO.Path.Combine(dir, safeFileName)

        Using bmp As New Bitmap(maxTableWidth + 40, totalHeight)
            ' تعيين دقة الصورة
            bmp.SetResolution(150, 150) ' زيادة الدقة لتحسين الجودة

            Using g As Graphics = Graphics.FromImage(bmp)
                ' إعدادات الرسم عالية الجودة
                g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
                g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
                g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit

                g.Clear(Color.White)
                Dim fontTitle As New Font("Arial", 18, FontStyle.Bold)
                Dim fontValue As New Font("Arial", 16, FontStyle.Regular)
                Dim brushTitle As New SolidBrush(Color.Black)
                Dim brushValue As New SolidBrush(Color.Black)
                Dim fillTitle As New SolidBrush(Color.FromArgb(230, 230, 230))
                Dim fillSalary As New SolidBrush(Color.FromArgb(210, 240, 210))
                Dim fillEarnings As New SolidBrush(Color.FromArgb(200, 230, 255)) ' لون أزرق فاتح للاستحقاقات
                Dim fillDeductions As New SolidBrush(Color.FromArgb(255, 230, 230)) ' لون وردي فاتح للاستقطاعات
                Dim pen As New Pen(Color.Black, 1)
                Dim penEarnings As New Pen(Color.FromArgb(100, 150, 200), 2) ' إطار أزرق للاستحقاقات
                Dim penDeductions As New Pen(Color.FromArgb(200, 100, 100), 2) ' إطار وردي للاستقطاعات
                Dim sfCenter As New StringFormat()
                sfCenter.Alignment = StringAlignment.Center
                sfCenter.LineAlignment = StringAlignment.Center
                sfCenter.FormatFlags = StringFormatFlags.DirectionRightToLeft

                Dim sfRight As New StringFormat()
                sfRight.Alignment = StringAlignment.Far
                sfRight.LineAlignment = StringAlignment.Center
                sfRight.FormatFlags = StringFormatFlags.DirectionRightToLeft

                ' String format for vertical text
                Dim sfVertical As New StringFormat()
                sfVertical.Alignment = StringAlignment.Center
                sfVertical.LineAlignment = StringAlignment.Center
                sfVertical.FormatFlags = StringFormatFlags.DirectionVertical

                ' رسم الجزء العلوي (Header)
                g.DrawString(":الاسم ", fontTitle, brushTitle, bmp.Width - 120, 20, sfRight)
                g.DrawString(employeeName, fontValue, brushValue, bmp.Width - 440, 20, sfRight)
                g.DrawString(":رقم الهاتف ", fontTitle, brushTitle, bmp.Width - 120, 60, sfRight)
                g.DrawString(employeePhone, fontValue, brushValue, bmp.Width - 440, 60, sfRight)
                g.DrawString("الكود: ", fontTitle, brushTitle, 20, 20, sfRight)
                g.DrawString(employeeCode, fontValue, brushValue, 200, 20, sfRight)
                g.DrawString("القسم: ", fontTitle, brushTitle, 20, 60, sfRight)
                g.DrawString(employeeDepartment, fontValue, brushValue, 200, 60, sfRight)
                g.DrawString(":الاجر الشامل ", fontTitle, brushTitle, bmp.Width - 120, 100, sfRight)
                ' رسم خلفية خضراء للقيمة بناءً على حجم النص ومحاذاة اليمين
                Dim totalWageTextSize As SizeF = g.MeasureString(employeeTotalWage, fontValue)
                ' زيادة عرض المستطيل قليلاً لضمان تغطية كاملة
                Dim wageRectWidth As Single = totalWageTextSize.Width + 15 ' تقليل الهامش الجانبي
                Dim wageRectHeight As Single = fontValue.Height + 5 ' استخدام ارتفاع الخط مع هامش للتوسيط الرأسي

                ' تحديد الحافة اليمنى للمستطيل لتكون عند نفس موضع الحافة اليمنى للنص
                Dim rectRightEdgeX As Single = bmp.Width - 440 ' موضع الحافة اليمنى للنص

                ' حساب موضع الجهة اليسرى للمستطيل بناءً على الحافة اليمنى والعرض الجديد
                Dim wageRectX As Single = rectRightEdgeX - wageRectWidth + 80 ' تحريك المستطيل لليمين أكثر

                ' حساب موضع Y للمستطيل لتوسيطه رأسياً حول النقطة Y=100
                Dim wageRectY As Single = 100 - wageRectHeight / 2

                Dim wageRect As New RectangleF(wageRectX, wageRectY, wageRectWidth, wageRectHeight)
                g.FillRectangle(fillSalary, wageRect)
                ' رسم القيمة - موضع النص لم يتغير ليظل في مكانه الأصلي
                g.DrawString(employeeTotalWage, fontValue, brushValue, bmp.Width - 440, 100, sfRight)

                g.DrawLine(pen, 20, headerHeight - 10, maxTableWidth + 20, headerHeight - 10)

                ' رسم الجزء الأوسط (طريقة القبض)
                Dim paymentMethodY As Integer = headerHeight + 10
                g.DrawString("طريقة القبض: ", fontTitle, brushTitle, New RectangleF(20, paymentMethodY, maxTableWidth, paymentMethodHeight - 20), sfCenter)
                g.DrawString(employeePaymentMethod, fontValue, brushValue, New RectangleF(20, paymentMethodY + (paymentMethodHeight - 20) / 2, maxTableWidth, (paymentMethodHeight - 20) / 2), sfCenter)
                g.DrawLine(pen, 20, headerHeight + paymentMethodHeight - 10, maxTableWidth + 20, headerHeight + paymentMethodHeight - 10)

                y = headerHeight + paymentMethodHeight + sectionSpacing

                ' Initialize table dimensions
                Dim tableHeightDed1 As Integer = 0
                Dim tableWidthDed1 As Integer = 0
                Dim tableHeightDed2 As Integer = 0
                Dim tableWidthDed2 As Integer = 0
                Dim tableHeightEarn As Integer = 0
                Dim tableWidthEarn As Integer = 0
                Dim numColumnsEarn As Integer = 0

                Dim maxRowsPerColumn As Integer = maxCols ' Maximum items per vertical column

                ' تقسيم قائمة الاستقطاعات إلى قسمين لإنشاء جدولين
                Dim deductionsList1 As New List(Of String)
                Dim deductionsList2 As New List(Of String)

                ' تحديد كيفية تقسيم قائمة الاستقطاعات - مثال: أول maxRowsPerColumn عناصر في القائمة الأولى والباقي في الثانية
                Dim splitIndex As Integer = Math.Min(maxRowsPerColumn, deductionsList.Count)
                For i As Integer = 0 To deductionsList.Count - 1
                    If i < splitIndex Then
                        deductionsList1.Add(deductionsList(i))
                    Else
                        deductionsList2.Add(deductionsList(i))
                    End If
                Next

                ' Calculate dimensions for the deduction tables and the earnings table
                Dim numColumnsDed1 As Integer = Math.Ceiling(CSng(deductionsList1.Count) / maxRowsPerColumn)
                tableHeightDed1 = Math.Min(deductionsList1.Count, maxRowsPerColumn) * cellHeight
                tableWidthDed1 = numColumnsDed1 * cellWidth * 2

                Dim numColumnsDed2 As Integer = Math.Ceiling(CSng(deductionsList2.Count) / maxRowsPerColumn)
                tableHeightDed2 = Math.Min(deductionsList2.Count, maxRowsPerColumn) * cellHeight
                tableWidthDed2 = numColumnsDed2 * cellWidth * 2

                ' Calculate table dimensions for vertical list layout (Earnings)
                numColumnsEarn = Math.Ceiling(CSng(earningsList.Count) / maxRowsPerColumn)
                tableHeightEarn = Math.Min(earningsList.Count, maxRowsPerColumn) * cellHeight
                tableWidthEarn = numColumnsEarn * cellWidth * 2

                ' Define the width for vertical titles
                Dim verticalTitleWidth As Integer = 40
                Dim verticalTitlePadding As Integer = 20 ' Padding on each side of vertical title
                Dim verticalTitleSpace As Integer = verticalTitleWidth + verticalTitlePadding * 2 ' Total space for vertical title

                ' Calculate X positions for the three main blocks (tables) based on the NEW order: Ded2 | Ded1 | Vert Ded Title | Earn | Vert Earn Title
                Dim startXDed2 As Integer = 20 ' Second deductions table starts from the left
                Dim startXDed1 As Integer = startXDed2 + tableWidthDed2 + verticalTitlePadding ' First deductions table position
                Dim startXVerticalTitleDed As Integer = startXDed1 + tableWidthDed1 ' Vertical الاستقطاعات title position

                ' Add extra space between sections
                Dim extraSpace As Integer = 100 ' Space between deductions and earnings sections

                ' Position earnings table and its title
                Dim startXEarn As Integer = startXVerticalTitleDed + verticalTitleWidth + extraSpace ' Earnings table position
                Dim startXVerticalTitleEarn As Integer = startXEarn + tableWidthEarn ' Vertical الاستحقاقات title position (now on the right)

                ' Calculate the total width needed for the image
                Dim totalWidth As Integer = startXVerticalTitleEarn + verticalTitleWidth + 20 ' Add 20 for right margin

                Dim currentYTableTop As Integer = headerHeight + paymentMethodHeight + sectionSpacing ' Consistent Y for top of all main tables

                ' Track the bottom Y position after drawing tables
                Dim bottomYAfterTables As Integer = currentYTableTop

                ' جدول الاستحقاقات (على اليسار)
                If earningsList.Count > 0 Then
                    ' عرض العناوين والقيم في أعمدة رأسية
                    Dim columnStartX As Integer = startXEarn ' X position for the current column
                    Dim columnCurrentY As Integer = currentYTableTop ' Y position starts at the top of the tables area

                    For Each earningKey In earningsList
                        Dim xTitle As Integer = columnStartX + cellWidth ' Right column for title
                        Dim xValue As Integer = columnStartX ' Left column for value

                        ' رسم خلفية العنوان
                        g.FillRectangle(fillEarnings, CSng(xTitle), CSng(columnCurrentY), CSng(cellWidth), CSng(cellHeight))
                        g.DrawRectangle(penEarnings, CSng(xTitle), CSng(columnCurrentY), CSng(cellWidth), CSng(cellHeight))
                        ' رسم العنوان
                        g.DrawString(earningKey, fontTitle, brushTitle, New RectangleF(CSng(xTitle), CSng(columnCurrentY), CSng(cellWidth), CSng(cellHeight)), sfCenter)

                        ' رسم خلفية القيمة
                        ' تحديد لون خلفية القيمة
                        Dim earningValueBrush As Brush
                        If earningKey.Contains("الاجر الشامل") Then
                            earningValueBrush = fillSalary ' لون أخضر للأجر الشامل
                        ElseIf earningKey = "اجمالى الاستحقاقات" Then
                            earningValueBrush = fillEarnings ' لون الاستحقاقات لإجمالي الاستحقاقات
                        Else
                            earningValueBrush = Brushes.White ' أبيض للقِيم الأخرى
                        End If
                        g.FillRectangle(earningValueBrush, CSng(xValue), CSng(columnCurrentY), CSng(cellWidth), CSng(cellHeight))
                        g.DrawRectangle(penEarnings, CSng(xValue), CSng(columnCurrentY), CSng(cellWidth), CSng(cellHeight))
                        ' رسم القيمة
                        g.DrawString(FormatNumber(safeEmployeeData(earningKey), earningKey), fontValue, brushValue, New RectangleF(CSng(xValue), CSng(columnCurrentY), CSng(cellWidth), CSng(cellHeight)), sfCenter)

                        columnCurrentY += cellHeight ' Move to the next row within this column
                    Next
                    bottomYAfterTables = Math.Max(bottomYAfterTables, currentYTableTop + tableHeightEarn)
                End If

                ' جدول الاستقطاعات الأول (على اليمين)
                If deductionsList1.Count > 0 Then
                    ' عرض العناوين والقيم في أعمدة رأسية للجدول الأول
                    Dim columnStartX As Integer = startXDed1 ' X position for the current column
                    Dim columnCurrentY As Integer = currentYTableTop ' Y position starts at the top of the tables area

                    For Each deductionKey In deductionsList1
                        Dim xTitle As Integer = columnStartX + cellWidth ' Right column for title
                        Dim xValue As Integer = columnStartX ' Left column for value

                        ' رسم خلفية العنوان
                        g.FillRectangle(fillDeductions, CSng(xTitle), CSng(columnCurrentY), CSng(cellWidth), CSng(cellHeight))
                        g.DrawRectangle(penDeductions, CSng(xTitle), CSng(columnCurrentY), CSng(cellWidth), CSng(cellHeight))
                        ' رسم العنوان
                        g.DrawString(deductionKey, fontTitle, brushTitle, New RectangleF(CSng(xTitle), CSng(columnCurrentY), CSng(cellWidth), CSng(cellHeight)), sfCenter)

                        ' رسم خلفية القيمة
                        ' تحديد لون خلفية القيمة
                        Dim deductionValueBrush As Brush = If(deductionKey = "اجمالى الاستقطاعات", fillDeductions, Brushes.White)
                        g.FillRectangle(deductionValueBrush, CSng(xValue), CSng(columnCurrentY), CSng(cellWidth), CSng(cellHeight))
                        g.DrawRectangle(penDeductions, CSng(xValue), CSng(columnCurrentY), CSng(cellWidth), CSng(cellHeight))
                        ' رسم القيمة
                        g.DrawString(FormatNumber(safeEmployeeData(deductionKey), deductionKey), fontValue, brushValue, New RectangleF(CSng(xValue), CSng(columnCurrentY), CSng(cellWidth), CSng(cellHeight)), sfCenter)

                        columnCurrentY += cellHeight ' Move to the next row within this column
                    Next
                    bottomYAfterTables = Math.Max(bottomYAfterTables, currentYTableTop + tableHeightDed1)
                End If

                ' جدول الاستقطاعات الثاني (على اليمين، بعد الأول)
                If deductionsList2.Count > 0 Then
                    ' عرض العناوين والقيم في أعمدة رأسية للجدول الثاني
                    Dim columnStartX As Integer = startXDed2 ' X position for the current column
                    Dim columnCurrentY As Integer = currentYTableTop ' Y position starts at the top of the tables area

                    For Each deductionKey In deductionsList2
                        Dim xTitle As Integer = columnStartX + cellWidth ' Right column for title
                        Dim xValue As Integer = columnStartX ' Left column for value

                        ' رسم خلفية العنوان
                        g.FillRectangle(fillDeductions, CSng(xTitle), CSng(columnCurrentY), CSng(cellWidth), CSng(cellHeight))
                        g.DrawRectangle(penDeductions, CSng(xTitle), CSng(columnCurrentY), CSng(cellWidth), CSng(cellHeight))
                        ' رسم العنوان
                        g.DrawString(deductionKey, fontTitle, brushTitle, New RectangleF(CSng(xTitle), CSng(columnCurrentY), CSng(cellWidth), CSng(cellHeight)), sfCenter)

                        ' رسم خلفية القيمة
                        ' تحديد لون خلفية القيمة
                        Dim deductionValueBrush As Brush = If(deductionKey = "اجمالى الاستقطاعات", fillDeductions, Brushes.White)
                        g.FillRectangle(deductionValueBrush, CSng(xValue), CSng(columnCurrentY), CSng(cellWidth), CSng(cellHeight))
                        g.DrawRectangle(penDeductions, CSng(xValue), CSng(columnCurrentY), CSng(cellWidth), CSng(cellHeight))
                        ' رسم القيمة
                        g.DrawString(FormatNumber(safeEmployeeData(deductionKey), deductionKey), fontValue, brushValue, New RectangleF(CSng(xValue), CSng(columnCurrentY), CSng(cellWidth), CSng(cellHeight)), sfCenter)

                        columnCurrentY += cellHeight ' Move to the next row within this column
                    Next
                    bottomYAfterTables = Math.Max(bottomYAfterTables, currentYTableTop + tableHeightDed2)
                End If

                ' رسم كلمة الاستحقاقات بشكل رأسي على يمين جدول الاستحقاقات
                If earningsList.Count > 0 Then
                    Dim verticalTitleFont As New Font("Arial", 18, FontStyle.Bold)
                    ' Calculate vertical position based on the earnings table height
                    Dim earningsTitleY As Single = currentYTableTop + tableHeightEarn / 2 ' Vertical center of the earnings table

                    ' Define the rectangle for the الاستحقاقات title
                    Dim earningsTitleRectHeight As Single = tableHeightEarn ' Height should match the earnings table
                    Dim earningsTitleRectX As Single = startXVerticalTitleEarn ' Position to the right of earnings table
                    Dim earningsTitleRectY As Single = currentYTableTop ' Align with the top of the earnings table

                    ' Draw background and border for الاستحقاقات title
                    g.FillRectangle(fillEarnings, earningsTitleRectX, earningsTitleRectY, verticalTitleWidth, earningsTitleRectHeight)
                    g.DrawRectangle(penEarnings, earningsTitleRectX, earningsTitleRectY, verticalTitleWidth, earningsTitleRectHeight)

                    ' Draw الاستحقاقات title (centered within the rectangle)
                    g.DrawString("الاستحقاقات", verticalTitleFont, brushTitle, New RectangleF(earningsTitleRectX, earningsTitleRectY, verticalTitleWidth, earningsTitleRectHeight), sfVertical)
                End If

                ' رسم كلمة الاستقطاعات بشكل رأسي على يمين جدول الاستقطاعات الثاني
                If deductionsList1.Count > 0 OrElse deductionsList2.Count > 0 Then
                    Dim verticalTitleFont As New Font("Arial", 18, FontStyle.Bold)
                    ' Calculate vertical position based on the taller of the two deduction tables
                    Dim combinedDeductionsHeight As Single = Math.Max(tableHeightDed1, tableHeightDed2)
                    Dim deductionsTitleY As Single = currentYTableTop + combinedDeductionsHeight / 2 ' Vertical center of the combined deductions tables height

                    ' Draw background and border for الاستقطاعات title
                    Dim deductionsTitleRectHeight As Single = combinedDeductionsHeight ' Height should match the taller of the two deduction tables
                    Dim deductionsTitleRectX As Single = startXVerticalTitleDed ' Position to the right of second deductions table
                    Dim deductionsTitleRectY As Single = currentYTableTop ' Align with the top of the combined deductions tables area

                    g.FillRectangle(fillDeductions, deductionsTitleRectX, deductionsTitleRectY, verticalTitleWidth, deductionsTitleRectHeight)
                    g.DrawRectangle(penDeductions, deductionsTitleRectX, deductionsTitleRectY, verticalTitleWidth, deductionsTitleRectHeight)

                    ' Draw الاستقطاعات title (centered within the rectangle)
                    g.DrawString("الاستقطاعات", verticalTitleFont, brushTitle, New RectangleF(deductionsTitleRectX, deductionsTitleRectY, verticalTitleWidth, deductionsTitleRectHeight), sfVertical)
                End If

                ' Update the y position to be below the lowest of the three tables
                y = bottomYAfterTables + sectionSpacing

                ' جدول بيانات أخرى (إن وجدت)
                If otherList.Count > 0 Then
                    ' رسم عنوان الجدول مع إطار
                    Dim titleRect As New RectangleF(20, y, maxTableWidth, cellHeight)
                    g.FillRectangle(New SolidBrush(Color.FromArgb(200, 200, 255)), titleRect) ' لون أزرق فاتح
                    g.DrawRectangle(New Pen(Color.FromArgb(100, 100, 200), 2), titleRect.X, titleRect.Y, titleRect.Width, titleRect.Height) ' إطار أزرق داكن
                    g.DrawString("صافى الراتب ورصيد الأجازات", fontTitle, brushTitle, titleRect, sfCenter)
                    y += cellHeight
                    ' صف العناوين
                    Dim tableWidth As Integer = otherList.Count * cellWidth
                    Dim startX As Integer = (maxTableWidth - tableWidth) / 2
                    For c As Integer = otherList.Count - 1 To 0 Step -1 ' عكس ترتيب الأعمدة
                        Dim x As Integer = startX + c * cellWidth
                        g.FillRectangle(fillTitle, x, y, cellWidth, cellHeight)
                        g.DrawRectangle(pen, x, y, cellWidth, cellHeight)
                        g.DrawString(otherList(c), fontTitle, brushTitle, New RectangleF(x, y, cellWidth, cellHeight), sfCenter)
                    Next
                    y += cellHeight
                    ' صف القيم
                    For c As Integer = otherList.Count - 1 To 0 Step -1 ' عكس ترتيب القيم
                        Dim x As Integer = startX + c * cellWidth
                        ' تحديد لون خلفية القيمة
                        Dim otherValueBrush As Brush
                        If otherList(c).Contains("الاجر الشامل") Then
                            otherValueBrush = fillSalary ' لون أخضر للأجر الشامل
                        ElseIf otherList(c) = "الراتب المستحق" Then
                            otherValueBrush = fillEarnings ' لون الاستحقاقات للراتب المستحق في هذا الجدول
                        Else
                            otherValueBrush = Brushes.White ' أبيض للقِيم الأخرى
                        End If
                        g.FillRectangle(otherValueBrush, x, y, cellWidth, cellHeight)
                        g.DrawRectangle(pen, x, y, cellWidth, cellHeight)
                        g.DrawString(FormatNumber(safeEmployeeData(otherList(c)), otherList(c)), fontValue, brushValue, New RectangleF(x, y, cellWidth, cellHeight), sfCenter)
                    Next
                    y += cellHeight + tableSpacing
                End If

                ' حفظ الصورة بتنسيق JPEG مع جودة عالية جداً
                Dim encoderParameters As New Imaging.EncoderParameters(1)
                encoderParameters.Param(0) = New Imaging.EncoderParameter(Imaging.Encoder.Quality, 100L)
                Dim jpegCodec = Imaging.ImageCodecInfo.GetImageEncoders().FirstOrDefault(Function(codec) codec.FormatID = Imaging.ImageFormat.Jpeg.Guid)
                If jpegCodec IsNot Nothing Then
                    bmp.Save(safeOutputPath, jpegCodec, encoderParameters)
                Else
                    bmp.Save(safeOutputPath)
                End If
            End Using
        End Using
    End Sub

    ' دالة تنظيف اسم الملف من الرموز غير الصالحة
    Private Function CleanFileName(fileName As String) As String
        Dim invalidChars = System.IO.Path.GetInvalidFileNameChars()
        For Each c In invalidChars
            fileName = fileName.Replace(c, "_"c)
        Next
        Return fileName
    End Function

    ' دالة تنسيق الأرقام بحيث تظهر بكسور عشرية واحدة فقط
    Private Function FormatNumber(value As String, columnName As String) As String
        ' إذا كان العمود هو "عدد ساعات الاذن"، نعيد القيمة كما هي
        If columnName = "عدد ساعات الاذن" Then
            Return value
        End If

        Dim result As Double
        If Double.TryParse(value, result) Then
            Return result.ToString("F1")
        End If
        Return value
    End Function

    ' زر إنشاء صور الموظفين
    Private Sub btnGenerateImages_Click(sender As Object, e As EventArgs)
        If employeesData Is Nothing OrElse employeesData.Rows.Count = 0 Then
            MessageBox.Show("يرجى تحميل بيانات الموظفين أولاً!", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        Dim desktopPath As String = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        Dim imagesDir As String = Path.Combine(desktopPath, "EmployeeImages")
        If Not Directory.Exists(imagesDir) Then
            Directory.CreateDirectory(imagesDir)
        End If
        For Each row As DataRow In employeesData.Rows
            ' استخراج رقم الهاتف
            Dim phone As String = ""
            If employeesData.Columns.Contains("رقم التليفون") Then
                phone = row("رقم التليفون").ToString().Trim()
            ElseIf employeesData.Columns.Contains("Phone") Then
                phone = row("Phone").ToString().Trim()
            End If
            If String.IsNullOrWhiteSpace(phone) Then
                Continue For
            End If
            ' تجهيز رقم الهاتف الدولي
            Dim phoneInternational As String = phone
            If phoneInternational.StartsWith("01") AndAlso phoneInternational.Length = 11 Then
                phoneInternational = "+20" & phoneInternational.Substring(1)
            End If
            ' تجهيز بيانات الموظف
            Dim employeeDict As New Dictionary(Of String, String)()
            For Each col As DataColumn In employeesData.Columns
                If col.ColumnName <> "Status" Then
                    employeeDict(col.ColumnName) = row(col.ColumnName).ToString()
                End If
            Next
            Dim imgPath As String = Path.Combine(imagesDir, $"{phoneInternational}.png")
            CreateEmployeeImage(employeeDict, imgPath)
        Next
        MessageBox.Show("تم إنشاء جميع الصور بنجاح! سيتم فتح المجلد الآن.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Process.Start("explorer.exe", imagesDir)
    End Sub

    ' زر إرسال صور الموظفين عبر واتساب
    Private Sub btnSendImages_Click(sender As Object, e As EventArgs)
        Dim desktopPath As String = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        Dim imagesDir As String = Path.Combine(desktopPath, "EmployeeImages")
        If employeesData Is Nothing OrElse employeesData.Rows.Count = 0 OrElse Not Directory.Exists(imagesDir) Then
            MessageBox.Show("يرجى تحميل بيانات الموظفين وإنشاء الصور أولاً!", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        For Each row As DataRow In employeesData.Rows
            ' استخراج رقم الهاتف
            Dim phone As String = ""
            If employeesData.Columns.Contains("رقم التليفون") Then
                phone = row("رقم التليفون").ToString().Trim()
            ElseIf employeesData.Columns.Contains("Phone") Then
                phone = row("Phone").ToString().Trim()
            End If

            ' تحقق من صحة الرقم
            Dim isValid As Boolean = False
            If phone.StartsWith("01") AndAlso phone.Length = 11 Then
                phone = "+20" & phone.Substring(1)
                isValid = True
            ElseIf phone.StartsWith("+20") AndAlso phone.Length = 13 Then
                isValid = True
            End If

            If Not isValid Then
                row("Status") = "فشل - رقم هاتف غير صالح"
                dgvEmployees.Refresh()
                Continue For
            End If

            Dim imgPath As String = Path.Combine(imagesDir, $"{phone}.png")
            If Not File.Exists(imgPath) Then
                row("Status") = "فشل - لم يتم العثور على صورة"
                dgvEmployees.Refresh()
                Continue For
            End If

            Try
                ' فتح شات واتساب
                Process.Start($"whatsapp://send?phone={phone}")
                Thread.Sleep(3000)

                ' نسخ الصورة إلى الحافظة مع الحفاظ على الجودة
                Using img As Image = Image.FromFile(imgPath)
                    ' تحسين جودة الصورة قبل نسخها
                    Using highQualityImg As New Bitmap(img.Width, img.Height)
                        highQualityImg.SetResolution(150, 150)
                        Using g As Graphics = Graphics.FromImage(highQualityImg)
                            g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
                            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
                            g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
                            g.DrawImage(img, 0, 0, img.Width, img.Height)
                        End Using
                        Clipboard.SetImage(highQualityImg)
                    End Using
                End Using

                Thread.Sleep(1000)
                ' لصق الصورة في الشات
                SendKeys.SendWait("^v")
                Thread.Sleep(500)
                ' إرسال الصورة
                SendKeys.SendWait("{ENTER}")
                Thread.Sleep(1500)
                row("Status") = "تم الإرسال"
            Catch ex As Exception
                row("Status") = "فشل - خطأ في واتساب"
            End Try
            dgvEmployees.Refresh()
        Next
        MessageBox.Show("تم إرسال جميع الصور عبر واتساب!", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ' زر تصدير أرقام الفشل
    Private Sub btnExportFailed_Click(sender As Object, e As EventArgs)
        If employeesData Is Nothing OrElse employeesData.Rows.Count = 0 Then
            MessageBox.Show("لا توجد بيانات!", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        Dim failedRows = employeesData.Select("Status <> 'تم الإرسال'")
        If failedRows.Length = 0 Then
            MessageBox.Show("لا يوجد أرقام فشل الإرسال لها!", "معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        Using sfd As New SaveFileDialog()
            sfd.Filter = "CSV File (*.csv)|*.csv|Text File (*.txt)|*.txt"
            sfd.FileName = "FailedNumbers.csv"
            If sfd.ShowDialog() = DialogResult.OK Then
                Using writer As New StreamWriter(sfd.FileName, False, System.Text.Encoding.UTF8)
                    writer.WriteLine("رقم الهاتف,الحالة")
                    For Each row As DataRow In failedRows
                        Dim phone As String = ""
                        If employeesData.Columns.Contains("رقم التليفون") Then
                            phone = row("رقم التليفون").ToString().Trim()
                        ElseIf employeesData.Columns.Contains("Phone") Then
                            phone = row("Phone").ToString().Trim()
                        End If
                        writer.WriteLine($"{phone},{row("Status")}")
                    Next
                End Using
                MessageBox.Show("تم تصدير الأرقام بنجاح!", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End Using
    End Sub
End Class