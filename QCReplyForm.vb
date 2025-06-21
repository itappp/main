Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System.Net
Public Class QCReplyForm
    Dim con As New SqlConnection("Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;")
    Private connectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"
    Private selectedRef As String = ""

    ' الكنترولز
    Private WithEvents dgvProblems As New DataGridView()
    Private tblDetails As New TableLayoutPanel()
    Private lblReply As New Label()
    Private txtqcreply As New TextBox()
    Private lblStatus As New Label()
    Private cmbStatus As New ComboBox()
    Private WithEvents btnSave As New Button()

    Private Sub QCReplyForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.WindowState = FormWindowState.Maximized
        Me.Text = "الرد على مشاكل العملاء"

        ' إعداد DataGridView
        dgvProblems.Location = New Point(10, 10)
        dgvProblems.AutoSize = False
        dgvProblems.Height = 250 ' ارتفاع ثابت مناسب
        dgvProblems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        dgvProblems.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvProblems.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvProblems.DefaultCellStyle.Font = New Font("Segoe UI", 12, FontStyle.Regular)
        dgvProblems.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        dgvProblems.RowTemplate.Height = 35
        dgvProblems.AllowUserToResizeRows = False
        dgvProblems.AllowUserToResizeColumns = False
        dgvProblems.Anchor = AnchorStyles.Top Or AnchorStyles.Left
        Me.Controls.Add(dgvProblems)

        ' تحميل المشاكل غير المردود عليها
        LoadUnrepliedProblems()
        dgvProblems.AutoResizeColumns()
        dgvProblems.Width = dgvProblems.Columns.GetColumnsWidth(DataGridViewElementStates.Visible) + dgvProblems.RowHeadersWidth + 2

        ' إعداد TableLayoutPanel لعرض تفاصيل المشكلة
        tblDetails.Location = New Point(10, dgvProblems.Bottom + 10)
        tblDetails.Size = New Size(Me.ClientSize.Width - 40, 80)
        tblDetails.ColumnCount = 6 ' زيادة عدد الأعمدة لاستيعاب الكميات
        tblDetails.RowCount = 2
        tblDetails.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
        tblDetails.Font = New Font("Segoe UI", 12, FontStyle.Regular)
        tblDetails.AutoSize = True
        tblDetails.AutoSizeMode = AutoSizeMode.GrowAndShrink

        ' إضافة العناوين
        tblDetails.Controls.Add(New Label() With {.Text = "رقم التعاقد", .TextAlign = ContentAlignment.MiddleCenter, .Dock = DockStyle.Fill, .Font = New Font("Segoe UI", 12, FontStyle.Bold)}, 0, 0)
        tblDetails.Controls.Add(New Label() With {.Text = "الخامة", .TextAlign = ContentAlignment.MiddleCenter, .Dock = DockStyle.Fill, .Font = New Font("Segoe UI", 12, FontStyle.Bold)}, 1, 0)
        tblDetails.Controls.Add(New Label() With {.Text = "باتش", .TextAlign = ContentAlignment.MiddleCenter, .Dock = DockStyle.Fill, .Font = New Font("Segoe UI", 12, FontStyle.Bold)}, 2, 0)
        tblDetails.Controls.Add(New Label() With {.Text = "لوت", .TextAlign = ContentAlignment.MiddleCenter, .Dock = DockStyle.Fill, .Font = New Font("Segoe UI", 12, FontStyle.Bold)}, 3, 0)
        tblDetails.Controls.Add(New Label() With {.Text = "كمية بالمتر", .TextAlign = ContentAlignment.MiddleCenter, .Dock = DockStyle.Fill, .Font = New Font("Segoe UI", 12, FontStyle.Bold)}, 4, 0) ' عنوان جديد لكمية المتر
        tblDetails.Controls.Add(New Label() With {.Text = "كمية بالكيلو", .TextAlign = ContentAlignment.MiddleCenter, .Dock = DockStyle.Fill, .Font = New Font("Segoe UI", 12, FontStyle.Bold)}, 5, 0) ' عنوان جديد لكمية الكيلو
        ' صف البيانات
        For i As Integer = 0 To 5 ' تعديل الحلقة لإضافة خلايا البيانات الجديدة
            tblDetails.Controls.Add(New Label() With {.Text = "-", .TextAlign = ContentAlignment.MiddleCenter, .Dock = DockStyle.Fill}, i, 1)
        Next
        Me.Controls.Add(tblDetails)

        ' إعداد الكنترولز الأخرى (الرد، الحالة، الحفظ)
        lblReply.Text = "الرد على المشكلة:"
        lblReply.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        lblReply.Location = New Point(10, tblDetails.Bottom + 15)
        lblReply.AutoSize = True
        Me.Controls.Add(lblReply)

        txtqcreply.Multiline = True
        txtqcreply.Font = New Font("Segoe UI", 12, FontStyle.Regular)
        txtqcreply.Width = Me.ClientSize.Width - 40
        txtqcreply.Height = 80
        txtqcreply.Location = New Point(10, lblReply.Bottom + 5)
        Me.Controls.Add(txtqcreply)

        lblStatus.Text = "حالة المشكلة:"
        lblStatus.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        lblStatus.Location = New Point(10, txtqcreply.Bottom + 10)
        lblStatus.AutoSize = True
        Me.Controls.Add(lblStatus)

        cmbStatus.Items.AddRange(New String() {"حقيقي", "غير حقيقي"})
        cmbStatus.Font = New Font("Segoe UI", 12, FontStyle.Regular)
        cmbStatus.Width = 200
        cmbStatus.Location = New Point(10, lblStatus.Bottom + 5)
        Me.Controls.Add(cmbStatus)

        btnSave.Text = "حفظ الرد"
        btnSave.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        btnSave.Width = 200
        btnSave.Height = 40
        btnSave.Location = New Point(10, cmbStatus.Bottom + 15)
        Me.Controls.Add(btnSave)
    End Sub

    Private Sub LoadUnrepliedProblems()
        Try
            Using conn As New SqlConnection(connectionString)
                Dim query As String = "SELECT ref, (SELECT code FROM Clients WHERE id=clientid) AS ClientCode, Issue, DateInserted FROM CustomerProblems WHERE qcreply IS NULL ORDER BY DateInserted DESC"
                Dim da As New SqlDataAdapter(query, conn)
                Dim dt As New DataTable()
                da.Fill(dt)
                dgvProblems.DataSource = dt

                ' تفعيل التفاف النص في عمود المشكلة
                If dgvProblems.Columns.Contains("Issue") Then
                    dgvProblems.Columns("Issue").DefaultCellStyle.WrapMode = DataGridViewTriState.True
                End If
                ' جعل الصفوف تأخذ ارتفاع تلقائي حسب المحتوى
                dgvProblems.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells

                ' توسيط كل الأعمدة
                For Each col As DataGridViewColumn In dgvProblems.Columns
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                Next

                ' تلوين رؤوس الأعمدة
                dgvProblems.EnableHeadersVisualStyles = False
                dgvProblems.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue
                dgvProblems.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
                dgvProblems.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 12, FontStyle.Bold)

                ' ضبط عرض الداتا جريد ليملأ الفورم
                dgvProblems.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
                dgvProblems.Width = Me.ClientSize.Width - 40
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading problems: " & ex.Message)
        End Try
    End Sub

    Private Sub dgvProblems_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvProblems.CellClick
        If e.RowIndex >= 0 Then
            selectedRef = dgvProblems.Rows(e.RowIndex).Cells("ref").Value.ToString()
            LoadProblemDetails(selectedRef)
        End If
    End Sub

    Private Sub LoadProblemDetails(ref As String)
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim query As String = "SELECT cp.*, c.ContractNo, cl.code AS ClientCode, c.Material, c.Batch, c.lot FROM CustomerProblems cp LEFT JOIN Contracts c ON cp.ContractID = c.ContractID LEFT JOIN Clients cl ON cp.clientid = cl.id WHERE cp.ref = @ref" ' استعلام محدث لجلب تفاصيل المشكلة
                Dim cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@ref", ref)
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                If reader.Read() Then
                    tblDetails.GetControlFromPosition(0, 1).Text = reader("ContractNo").ToString()
                    tblDetails.GetControlFromPosition(1, 1).Text = reader("Material").ToString()
                    tblDetails.GetControlFromPosition(2, 1).Text = reader("Batch").ToString()
                    tblDetails.GetControlFromPosition(3, 1).Text = reader("lot").ToString()
                    ' تحديث الخلايا الجديدة بالكميات
                    tblDetails.GetControlFromPosition(4, 1).Text = If(IsDBNull(reader("QtyM")), "-", reader("QtyM").ToString())
                    tblDetails.GetControlFromPosition(5, 1).Text = If(IsDBNull(reader("QtyKg")), "-", reader("QtyKg").ToString())
                End If
                reader.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading problem details: " & ex.Message)
        End Try
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If selectedRef = "" Then
            MessageBox.Show("يرجى اختيار مشكلة أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        If txtqcreply.Text.Trim = "" Then
            MessageBox.Show("يرجى كتابة الرد.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        If cmbStatus.SelectedIndex = -1 Then
            MessageBox.Show("يرجى تحديد حالة المشكلة.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim cmd As New SqlCommand("UPDATE CustomerProblems SET qcreply=@reply, status=@status, dateqc=@dateqc, qcuser=@qcuser WHERE ref=@ref", conn)
                cmd.Parameters.AddWithValue("@reply", txtqcreply.Text.Trim)
                cmd.Parameters.AddWithValue("@status", cmbStatus.SelectedItem.ToString())
                cmd.Parameters.AddWithValue("@dateqc", DateTime.Now)
                cmd.Parameters.AddWithValue("@qcuser", LoggedInUsername)
                cmd.Parameters.AddWithValue("@ref", selectedRef)
                cmd.ExecuteNonQuery()

                ' جلب تفاصيل المشكلة لإرسالها في الإيميل
                Dim detailsCmd As New SqlCommand("SELECT cp.ref, cl.code AS ClientCode, cp.Issue, cp.QtyM, cp.QtyKg, cp.qcreply, cp.status, cp.qcuser, cp.dateqc, c.ContractNo FROM CustomerProblems cp LEFT JOIN Clients cl ON cp.clientid = cl.id LEFT JOIN Contracts c ON cp.ContractID = c.ContractID WHERE cp.ref = @ref", conn)
                detailsCmd.Parameters.AddWithValue("@ref", selectedRef)
                Dim reader = detailsCmd.ExecuteReader()
                Dim ref As String = "", clientCode As String = "", issue As String = "", qtyM As String = "", qtyKg As String = "", qcreply As String = "", status As String = "", qcuser As String = "", dateqc As String = "", contractNo As String = ""
                If reader.Read() Then
                    ref = reader("ref").ToString()
                    clientCode = reader("ClientCode").ToString()
                    issue = reader("Issue").ToString()
                    qtyM = If(IsDBNull(reader("QtyM")), "-", reader("QtyM").ToString())
                    qtyKg = If(IsDBNull(reader("QtyKg")), "-", reader("QtyKg").ToString())
                    qcreply = reader("qcreply").ToString()
                    status = reader("status").ToString()
                    qcuser = reader("qcuser").ToString()
                    dateqc = If(IsDBNull(reader("dateqc")), "-", Convert.ToDateTime(reader("dateqc")).ToString("yyyy-MM-dd HH:mm:ss"))
                    contractNo = If(IsDBNull(reader("ContractNo")), "-", reader("ContractNo").ToString())
                End If
                reader.Close()

                ' تجهيز نص الإيميل
                Dim subject As String = "رد على مشكلة عميل (" & ref & ")"
                Dim body As String = "<html><body>" &
                    "<h2 style='color:#2d3748;'>تم الرد على مشكلة عميل</h2>" &
                    "<table border='1' cellpadding='8' cellspacing='0' style='border-collapse:collapse;font-family:Segoe UI,Arial,sans-serif;font-size:15px;'>" &
                    "<tr style='background-color:#f2f2f2;'><th>رقم الإذن (ref)</th><td>" & ref & "</td></tr>" &
                    "<tr><th>رقم التعاقد</th><td>" & contractNo & "</td></tr>" &
                    "<tr style='background-color:#f2f2f2;'><th>كود العميل</th><td>" & clientCode & "</td></tr>" &
                    "<tr><th>المشكلة</th><td>" & issue & "</td></tr>" &
                    "<tr style='background-color:#f2f2f2;'><th>كمية بالمتر</th><td>" & qtyM & "</td></tr>" &
                    "<tr><th>كمية بالكيلو</th><td>" & qtyKg & "</td></tr>" &
                    "<tr style='background-color:#f2f2f2;'><th>الرد</th><td>" & qcreply & "</td></tr>" &
                    "<tr><th>الحالة</th><td>" & status & "</td></tr>" &
                    "<tr style='background-color:#f2f2f2;'><th>اسم المستخدم</th><td>" & qcuser & "</td></tr>" &
                    "<tr><th>تاريخ الرد</th><td>" & dateqc & "</td></tr>" &
                    "</table>" &
                    "</body></html>"
                SendEmailNotification(subject, body, cmbStatus.SelectedItem.ToString())

                MessageBox.Show("تم حفظ الرد بنجاح.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ' إعادة تحميل الداتا جريد
                LoadUnrepliedProblems()
                ' مسح التفاصيل
                tblDetails.GetControlFromPosition(0, 1).Text = "-"
                tblDetails.GetControlFromPosition(1, 1).Text = "-"
                tblDetails.GetControlFromPosition(2, 1).Text = "-"
                tblDetails.GetControlFromPosition(3, 1).Text = "-"
                tblDetails.GetControlFromPosition(4, 1).Text = "-" ' مسح خلية كمية المتر
                tblDetails.GetControlFromPosition(5, 1).Text = "-" ' مسح خلية كمية الكيلو
                txtqcreply.Text = ""
                cmbStatus.SelectedIndex = -1
                selectedRef = ""
            End Using
        Catch ex As Exception
            MessageBox.Show("Error saving reply: " & ex.Message)
        End Try
    End Sub

    ' إرسال إيميل إشعار
    Private Sub SendEmailNotification(subject As String, body As String, status As String)
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                ' جلب الإيميلات من sales_qc دائماً
                Dim emailAddresses As New List(Of String)
                Dim query1 As String = "SELECT sales_qc FROM mails"
                Dim cmd1 As New SqlCommand(query1, conn)
                Dim reader1 As SqlDataReader = cmd1.ExecuteReader()
                While reader1.Read()
                    If Not IsDBNull(reader1("sales_qc")) AndAlso reader1("sales_qc").ToString().Trim() <> "" Then
                        emailAddresses.Add(reader1("sales_qc").ToString().Trim())
                    End If
                End While
                reader1.Close()

                ' إذا كانت الحالة حقيقي، أضف إيميلات التخطيط
                If status = "حقيقي" Then
                    Dim query2 As String = "SELECT mail FROM mails WHERE department='planning'"
                    Dim cmd2 As New SqlCommand(query2, conn)
                    Dim reader2 As SqlDataReader = cmd2.ExecuteReader()
                    While reader2.Read()
                        If Not IsDBNull(reader2("mail")) AndAlso reader2("mail").ToString().Trim() <> "" Then
                            emailAddresses.Add(reader2("mail").ToString().Trim())
                        End If
                    End While
                    reader2.Close()
                End If
                conn.Close()

                ' إزالة التكرار
                Dim uniqueEmails = emailAddresses.Distinct().ToList()

                ' إرسال الإيميل
                Dim smtpClient As New SmtpClient("smtppro.zoho.com", 587) With {
                    .Credentials = New NetworkCredential("mina.mouress@moamen.com", "Min@$MO#@wmg2024"),
                    .EnableSsl = True
                }
                Dim mailMessage As New MailMessage() With {
                    .From = New MailAddress("mina.mouress@moamen.com"),
                    .Subject = subject,
                    .Body = body,
                    .IsBodyHtml = True
                }
                For Each mail As String In uniqueEmails
                    mailMessage.To.Add(mail)
                Next
                smtpClient.Send(mailMessage)
            End Using
        Catch ex As SmtpException
            MessageBox.Show("SMTP error sending email notification: " & ex.Message & vbCrLf & "Status Code: " & ex.StatusCode)
        Catch ex As Exception
            MessageBox.Show("Error sending email notification: " & ex.Message)
        End Try
    End Sub

    ' ربط الأحداث
    Private Sub QCReplyForm_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        AddHandler dgvProblems.CellClick, AddressOf dgvProblems_CellClick
        AddHandler btnSave.Click, AddressOf btnSave_Click
    End Sub
End Class
