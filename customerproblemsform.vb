Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System.Net

Public Class CustomerProblemsForm
    Dim con As New SqlConnection("Server=localhost;Database=wmg-po;User Id=sa;Password=admin@WMG2024;")
    Private connectionString As String = "Server=localhost;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"
    Private lblUsername As New Label()
    ' تعريف الكنترولز
    Private WithEvents cmbContract As New ComboBox()
    Private WithEvents dgvContracts As New DataGridView()
    Private WithEvents dgvWorkOrders As New DataGridView()
    ' كنترولز جديدة
    Private lblIssue As New Label()
    Private txtIssue As New TextBox()
    Private lblQtyM As New Label()
    Private txtQtyM As New TextBox()
    Private lblQtyKg As New Label()
    Private txtQtyKg As New TextBox()
    Private WithEvents btnSave As New Button()
    ' كنترول كود العميل
    Private lblClientCode As New Label()
    Private WithEvents cmbclientcode As New ComboBox()

    Private Sub CustomerProblemsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' تهيئة الفورم ليفتح بكامل الشاشة
        Me.WindowState = FormWindowState.Maximized

        ' إعداد lblUsername
        lblUsername.Font = New Font("Segoe UI", 10, FontStyle.Regular)
        lblUsername.AutoSize = True
        lblUsername.Location = New Point(Me.ClientSize.Width - 250, 10)
        lblUsername.Text = "اسم المستخدم: "
        Me.Controls.Add(lblUsername)

        ' تهيئة الكنترولز وإضافتها للفورم
        cmbContract.Location = New Point(10, 10)
        cmbContract.Width = 200
        Me.Controls.Add(cmbContract)

        dgvContracts.Location = New Point(10, 50)
        dgvContracts.Width = Me.ClientSize.Width - 40
        dgvContracts.Height = 250
        dgvContracts.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        ' إعدادات الداتا جريد
        dgvContracts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        dgvContracts.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvContracts.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvContracts.DefaultCellStyle.Font = New Font("Segoe UI", 12, FontStyle.Regular)
        dgvContracts.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        Me.Controls.Add(dgvContracts)

        ' سيتم ضبط مكان dgvWorkOrders لاحقاً بعد تحميل بيانات العقود
        dgvWorkOrders.Width = Me.ClientSize.Width - 40
        dgvWorkOrders.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        dgvWorkOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        dgvWorkOrders.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvWorkOrders.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvWorkOrders.DefaultCellStyle.Font = New Font("Segoe UI", 12, FontStyle.Regular)
        dgvWorkOrders.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        Me.Controls.Add(dgvWorkOrders)

        ' إعداد الكنترولز الخاصة بالمشكلة
        lblIssue.Text = "المشكلة:"
        lblIssue.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        Me.Controls.Add(lblIssue)

        txtIssue.Multiline = True
        txtIssue.Font = New Font("Segoe UI", 12, FontStyle.Regular)
        txtIssue.Height = 80
        txtIssue.Width = Me.ClientSize.Width - 40
        Me.Controls.Add(txtIssue)

        lblQtyM.Text = "الكمية (متر):"
        lblQtyM.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        Me.Controls.Add(lblQtyM)

        txtQtyM.Font = New Font("Segoe UI", 12, FontStyle.Regular)
        txtQtyM.Width = 200
        Me.Controls.Add(txtQtyM)

        lblQtyKg.Text = "الكمية (كيلو):"
        lblQtyKg.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        Me.Controls.Add(lblQtyKg)

        txtQtyKg.Font = New Font("Segoe UI", 12, FontStyle.Regular)
        txtQtyKg.Width = 200
        Me.Controls.Add(txtQtyKg)

        ' إعداد كنترول كود العميل
        lblClientCode.Text = "كود العميل:"
        lblClientCode.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        Me.Controls.Add(lblClientCode)
        cmbclientcode.Font = New Font("Segoe UI", 12, FontStyle.Regular)
        cmbclientcode.Width = 200
        Me.Controls.Add(cmbclientcode)

        btnSave.Text = "تسجيل"
        btnSave.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        btnSave.Width = 200
        btnSave.Height = 40
        Me.Controls.Add(btnSave)

        ' إخفاء كل الكنترولز الخاصة بأوامر الشغل والمشكلة عند بدء التشغيل
        dgvWorkOrders.Visible = False
        lblIssue.Visible = False
        txtIssue.Visible = False
        lblQtyM.Visible = False
        txtQtyM.Visible = False
        lblQtyKg.Visible = False
        txtQtyKg.Visible = False
        lblClientCode.Visible = False
        cmbclientcode.Visible = False
        btnSave.Visible = False

        ' Temporarily remove event handler
        RemoveHandler cmbContract.SelectedIndexChanged, AddressOf cmbContract_SelectedIndexChanged
        LoadContracts()
        ' Add event handler back
        AddHandler cmbContract.SelectedIndexChanged, AddressOf cmbContract_SelectedIndexChanged

        LoadClientCodes()

        ' Access the logged-in username from the global variable
        lblUsername.Text = "Logged in as: " & LoggedInUsername
        ' Set the user access level based on logged-in username
        SetUserAccessLevel(LoggedInUsername)
        ' Clear dgvContracts initially
        dgvContracts.DataSource = Nothing
    End Sub
    Private Sub SetUserAccessLevel(ByVal username As String)
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim query As String = "SELECT acc_level FROM dep_users WHERE username = @username"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@username", username)
                    Dim accLevel As Object = cmd.ExecuteScalar()
                    If accLevel IsNot Nothing Then
                        UserAccessLevel = CInt(accLevel)
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error retrieving user access level: " & ex.Message)
        End Try
    End Sub
    Private Sub PositionWorkOrdersAndInputs()
        ' ضبط مكان dgvWorkOrders أسفل dgvContracts
        dgvWorkOrders.Location = New Point(10, dgvContracts.Bottom + 10)
        ' ضبط مكان الكنترولز أسفل dgvWorkOrders
        lblIssue.Location = New Point(10, dgvWorkOrders.Bottom + 10)
        txtIssue.Location = New Point(10, lblIssue.Bottom + 5)
        lblQtyM.Location = New Point(10, txtIssue.Bottom + 10)
        txtQtyM.Location = New Point(10, lblQtyM.Bottom + 5)
        lblQtyKg.Location = New Point(txtQtyM.Right + 20, txtIssue.Bottom + 10)
        txtQtyKg.Location = New Point(txtQtyM.Right + 20, lblQtyKg.Bottom + 5)
        ' مكان كود العميل بجانب txtQtyKg
        lblClientCode.Location = New Point(txtQtyKg.Right + 20, txtIssue.Bottom + 10)
        cmbclientcode.Location = New Point(txtQtyKg.Right + 20, lblClientCode.Bottom + 5)
        btnSave.Location = New Point(10, Math.Max(Math.Max(txtQtyM.Bottom, txtQtyKg.Bottom), cmbclientcode.Bottom) + 15)
        ' ضبط عرض الكنترولز مع تغيير حجم الفورم
        txtIssue.Width = Me.ClientSize.Width - 40
    End Sub

    Private Sub LoadContracts()

        Dim dt As New DataTable()
        Dim da As New SqlDataAdapter("SELECT ContractID, ContractNo FROM Contracts", con)
        da.Fill(dt)
        cmbContract.DataSource = dt
        cmbContract.DisplayMember = "ContractNo"
        cmbContract.ValueMember = "ContractID"
        cmbContract.SelectedIndex = -1
    End Sub

    Private Sub cmbContract_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbContract.SelectedIndexChanged
        ' عند تغيير التعاقد، أخفِ كل الكنترولز الخاصة بأوامر الشغل والمشكلة وامسح أي بيانات سابقة
        dgvWorkOrders.Visible = False
        lblIssue.Visible = False
        txtIssue.Visible = False
        lblQtyM.Visible = False
        txtQtyM.Visible = False
        lblQtyKg.Visible = False
        txtQtyKg.Visible = False
        lblClientCode.Visible = False
        cmbclientcode.Visible = False
        btnSave.Visible = False
        ' امسح أي بيانات سابقة
        txtIssue.Text = ""
        txtQtyM.Text = ""
        txtQtyKg.Text = ""
        ' امسح أي اختيارات سابقة في dgvWorkOrders
        For Each row As DataGridViewRow In dgvWorkOrders.Rows
            If Not row.IsNewRow AndAlso row.Cells("SelectWO").Value IsNot Nothing Then
                row.Cells("SelectWO").Value = False
            End If
        Next

        If cmbContract.SelectedIndex = -1 Then Exit Sub

        ' Ensure SelectedValue is an Integer before passing to SQL parameter
        ' Dim contractId As Integer
        ' If cmbContract.SelectedValue IsNot Nothing AndAlso IsNumeric(cmbContract.SelectedValue) Then
        '     contractId = Convert.ToInt32(cmbContract.SelectedValue)
        ' Else
        '     Exit Sub
        ' End If

        Dim selectedContractNo As String = cmbContract.Text

        Dim dt As New DataTable()
        Dim cmd As New SqlCommand("SELECT c.ContractID, c.ContractNo, f.fabricType_ar as contracttype, c.ContractDate, " &
                                  "cl.code AS ClientCode, c.Material, c.refno, c.Batch, c.lot, c.QuantityM, c.QuantityK, " &
                                  "c.FabricCode, c.color as color, c.WidthReq, c.WeightM, c.RollM, c.Notes, c.DateInserted " &
                                  "FROM Contracts c LEFT JOIN clients cl ON c.clientcode = cl.id LEFT JOIN fabric f ON c.contracttype = f.id " &
                                  "WHERE c.ContractNo = @ContractNo", con)
        ' cmd.Parameters.AddWithValue("@ContractID", contractId)
        cmd.Parameters.AddWithValue("@ContractNo", selectedContractNo)
        Dim da As New SqlDataAdapter(cmd)
        da.Fill(dt)
        dgvContracts.DataSource = dt

        ' إعادة ضبط خصائص الأعمدة بعد تحميل البيانات
        dgvContracts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        dgvContracts.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvContracts.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvContracts.DefaultCellStyle.Font = New Font("Segoe UI", 12, FontStyle.Regular)
        dgvContracts.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        dgvContracts.RowTemplate.Height = 36
        dgvContracts.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        Dim contractsRowCount As Integer = dgvContracts.Rows.Count
        Dim contractsHeaderHeight As Integer = dgvContracts.ColumnHeadersHeight
        dgvContracts.Height = contractsHeaderHeight + (contractsRowCount * dgvContracts.RowTemplate.Height) + 10

        ' إضافة عمود Checkbox إذا لم يكن موجود
        If dgvContracts.Columns("Select") Is Nothing Then
            Dim chk As New DataGridViewCheckBoxColumn()
            chk.Name = "Select"
            chk.HeaderText = "اختر"
            dgvContracts.Columns.Insert(0, chk)
        End If

        ' إعادة ضبط مكان dgvWorkOrders والكنترولز
        PositionWorkOrdersAndInputs()

        ' بعد تحميل التعاقد، أظهر فقط dgvWorkOrders
        dgvWorkOrders.Visible = True
    End Sub

    Private Sub dgvContracts_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvContracts.CellContentClick
        If e.ColumnIndex = dgvContracts.Columns("Select").Index AndAlso e.RowIndex >= 0 Then
            ' Uncheck all other checkboxes in the 'Select' column
            For Each row As DataGridViewRow In dgvContracts.Rows
                If Not row.IsNewRow AndAlso row.Index <> e.RowIndex Then
                    row.Cells("Select").Value = False
                End If
            Next

            Dim contractId As Integer = CInt(dgvContracts.Rows(e.RowIndex).Cells("ContractID").Value)
            LoadWorkOrders(contractId)
        End If
    End Sub

    Private Sub LoadWorkOrders(contractId As Integer)

        Dim dt As New DataTable()
        Dim cmd As New SqlCommand("SELECT worderid FROM techdata WHERE contract_id = @ContractID", con)
        cmd.Parameters.AddWithValue("@ContractID", contractId)
        Dim da As New SqlDataAdapter(cmd)
        da.Fill(dt)
        dgvWorkOrders.DataSource = dt

        ' إعادة ضبط خصائص الأعمدة بعد تحميل البيانات
        dgvWorkOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        dgvWorkOrders.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvWorkOrders.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvWorkOrders.DefaultCellStyle.Font = New Font("Segoe UI", 12, FontStyle.Regular)
        dgvWorkOrders.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        ' ضبط ارتفاع الصفوف
        dgvWorkOrders.RowTemplate.Height = 36
        dgvWorkOrders.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        ' ضبط ارتفاع الداتا جريد حسب عدد الصفوف
        Dim workOrdersRowCount As Integer = dgvWorkOrders.Rows.Count
        Dim workOrdersHeaderHeight As Integer = dgvWorkOrders.ColumnHeadersHeight
        dgvWorkOrders.Height = workOrdersHeaderHeight + (workOrdersRowCount * dgvWorkOrders.RowTemplate.Height) + 10

        ' إضافة عمود Checkbox إذا لم يكن موجود
        If dgvWorkOrders.Columns("SelectWO") Is Nothing Then
            Dim chk As New DataGridViewCheckBoxColumn()
            chk.Name = "SelectWO"
            chk.HeaderText = "اختر أمر الشغل"
            dgvWorkOrders.Columns.Insert(0, chk)
        End If

        ' إعادة ضبط مكان الكنترولز
        PositionWorkOrdersAndInputs()
    End Sub

    Private Sub dgvWorkOrders_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvWorkOrders.CellContentClick
        If e.ColumnIndex = dgvWorkOrders.Columns("SelectWO").Index AndAlso e.RowIndex >= 0 Then
            ' إظهار الكنترولز عند اختيار أمر شغل
            lblIssue.Visible = True
            txtIssue.Visible = True
            lblQtyM.Visible = True
            txtQtyM.Visible = True
            lblQtyKg.Visible = True
            txtQtyKg.Visible = True
            lblClientCode.Visible = True
            cmbclientcode.Visible = True
            btnSave.Visible = True
            ' إعادة ضبط أماكن الكنترولز
            PositionWorkOrdersAndInputs()
        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        ' اجمع أرقام أوامر الشغل المختارة في نص واحد مفصول بفاصلة
        Dim selectedWorkOrders As New List(Of String)()
        For Each row As DataGridViewRow In dgvWorkOrders.Rows
            If Not row.IsNewRow AndAlso row.Cells("SelectWO").Value IsNot Nothing AndAlso Convert.ToBoolean(row.Cells("SelectWO").Value) = True Then
                selectedWorkOrders.Add(row.Cells("worderid").Value.ToString())
            End If
        Next
        Dim workOrderNumbers As String = String.Join(",", selectedWorkOrders)

        ' تحقق من أن هناك أوامر شغل مختارة
        If workOrderNumbers = "" Then
            MsgBox("يرجى اختيار أمر شغل واحد على الأقل.", MsgBoxStyle.Exclamation)
            Return
        End If

        ' تحقق من أن هناك مشكلة مكتوبة
        If txtIssue.Text.Trim = "" Then
            MsgBox("يرجى كتابة المشكلة.", MsgBoxStyle.Exclamation)
            Return
        End If

        ' تحقق من إدخال كمية في أحد الحقلين على الأقل
        If txtQtyM.Text.Trim = "" AndAlso txtQtyKg.Text.Trim = "" Then
            MsgBox("يرجى إدخال الكمية بالمتر أو الكيلو.", MsgBoxStyle.Exclamation)
            Return
        End If

        ' تحقق من اختيار كود عميل
        If cmbclientcode.SelectedIndex = -1 OrElse cmbclientcode.SelectedValue Is Nothing Then
            MsgBox("يرجى اختيار كود العميل.", MsgBoxStyle.Exclamation)
            Return
        End If

        ' جلب رقم التعاقد الحالي
        Dim contractId As Integer = 0
        If cmbContract.SelectedValue IsNot Nothing AndAlso IsNumeric(cmbContract.SelectedValue) Then
            contractId = Convert.ToInt32(cmbContract.SelectedValue)
        End If

        ' جلب القيم الأخرى
        Dim clientId As Integer = Convert.ToInt32(cmbclientcode.SelectedValue)
        Dim clientCode As String = cmbclientcode.Text
        Dim issue As String = txtIssue.Text.Trim
        Dim qtyM As Decimal = 0
        Dim qtyKg As Decimal = 0
        Decimal.TryParse(txtQtyM.Text, qtyM)
        Decimal.TryParse(txtQtyKg.Text, qtyKg)

        ' تأكد أن الاتصال مفتوح قبل أي استخدام
        If con.State <> ConnectionState.Open Then con.Open()

        ' توليد رقم ref تلقائي
        Dim newRef As String = ""
        Using refCmd As New SqlCommand("SELECT TOP 1 ref FROM CustomerProblems WHERE ref IS NOT NULL AND ref <> '' ORDER BY ID DESC", con)
            Dim lastRefObj = refCmd.ExecuteScalar()
            If lastRefObj IsNot Nothing AndAlso lastRefObj.ToString().StartsWith("CPRP-") Then
                Dim lastNum As Integer = 0
                Integer.TryParse(lastRefObj.ToString().Substring(5), lastNum)
                newRef = "CPRP-" & (lastNum + 1).ToString("D7")
            Else
                newRef = "CPRP-0000001"
            End If
        End Using

        ' كود الإدخال في قاعدة البيانات
        Try
            Dim cmd As New SqlCommand("INSERT INTO CustomerProblems (ContractID, WorkOrderNumbers, Issue, QtyM, QtyKg, ref, clientid, kind, DateInserted) VALUES (@ContractID, @WorkOrderNumbers, @Issue, @QtyM, @QtyKg, @ref, @clientid, @kind, @DateInserted)", con)
            cmd.Parameters.AddWithValue("@ContractID", contractId)
            cmd.Parameters.AddWithValue("@WorkOrderNumbers", workOrderNumbers)
            cmd.Parameters.AddWithValue("@Issue", issue)
            cmd.Parameters.AddWithValue("@QtyM", If(qtyM = 0, DBNull.Value, qtyM))
            cmd.Parameters.AddWithValue("@QtyKg", If(qtyKg = 0, DBNull.Value, qtyKg))
            cmd.Parameters.AddWithValue("@ref", newRef)
            cmd.Parameters.AddWithValue("@clientid", clientId)
            cmd.Parameters.AddWithValue("@kind", "البيع")
            cmd.Parameters.AddWithValue("@DateInserted", DateTime.Now)
            cmd.ExecuteNonQuery()
            MsgBox("تم حفظ المشكلة بنجاح.", MsgBoxStyle.Information)

            ' جلب رقم التعاقد (ContractNo) من قاعدة البيانات
            Dim contractNo As String = ""
            Using contractCmd As New SqlCommand("SELECT ContractNo FROM Contracts WHERE ContractID = @ContractID", con)
                contractCmd.Parameters.AddWithValue("@ContractID", contractId)
                Dim result = contractCmd.ExecuteScalar()
                If result IsNot Nothing Then
                    contractNo = result.ToString()
                End If
            End Using

            ' جلب اسم المستخدم (public_name) من dep_users
            Dim publicName As String = ""
            Using userCmd As New SqlCommand("SELECT public_name FROM dep_users WHERE username = @username", con)
                userCmd.Parameters.AddWithValue("@username", LoggedInUsername)
                Dim result = userCmd.ExecuteScalar()
                If result IsNot Nothing Then
                    publicName = result.ToString()
                End If
            End Using

            ' إعداد body الإيميل كجدول HTML منسق
            Dim subject As String = "تسجيل مشكلة جديدة"
            Dim body As String = "<html><body>" &
                "<h2 style='color:#2d3748;'>تم تسجيل مشكلة جديدة</h2>" &
                "<table border='1' cellpadding='8' cellspacing='0' style='border-collapse:collapse;font-family:Segoe UI,Arial,sans-serif;font-size:15px;'>" &
                "<tr style='background-color:#f2f2f2;'><th>رقم الإذن (ref)</th><td>" & newRef & "</td></tr>" &
                "<tr><th>اسم المستخدم</th><td>" & publicName & "</td></tr>" &
                "<tr style='background-color:#f2f2f2;'><th>رقم التعاقد</th><td>" & contractNo & "</td></tr>" &
                "<tr><th>كود العميل</th><td>" & clientCode & "</td></tr>" &
                "<tr style='background-color:#f2f2f2;'><th>أوامر الشغل</th><td>" & workOrderNumbers & "</td></tr>" &
                "<tr><th>المشكلة</th><td>" & issue & "</td></tr>" &
                "<tr style='background-color:#f2f2f2;'><th>كمية بالمتر</th><td>" & txtQtyM.Text & "</td></tr>" &
                "<tr><th>كمية بالكيلو</th><td>" & txtQtyKg.Text & "</td></tr>" &
                "<tr style='background-color:#f2f2f2;'><th>تاريخ الإدخال</th><td>" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "</td></tr>" &
                "</table>" &
                "</body></html>"
            SendEmailNotification(subject, body)

            ' إعادة تهيئة الفورم بعد الحفظ والإيميل
            ResetFormState()

            ' امسح الحقول بعد الحفظ
            ' (تم نقل هذه الأوامر إلى ResetFormState)
        Catch ex As Exception
            MsgBox("حدث خطأ أثناء الحفظ: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            If con.State = ConnectionState.Open Then con.Close()
        End Try
    End Sub

    Private Sub SendEmailNotification(subject As String, body As String)
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()

                ' Get email addresses from the mails table
                Dim query As String = "SELECT sales_qc FROM mails"
                Dim cmd As New SqlCommand(query, conn)
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                Dim emailAddresses As New List(Of String)

                While reader.Read()
                    Dim email = reader("sales_qc").ToString().Trim()
                    If Not String.IsNullOrEmpty(email) Then
                        emailAddresses.Add(email)
                    End If
                End While
                reader.Close() ' مهم جداً إغلاق الـ reader قبل استخدام الاتصال مرة أخرى

                If emailAddresses.Count = 0 Then
                    MessageBox.Show("لا يوجد بريد إلكتروني لإرسال الإشعار إليه.")
                    Return
                End If

                ' Send email
                Dim smtpClient As New SmtpClient("smtppro.zoho.com", 587) With {
                    .Credentials = New NetworkCredential("customer.service@moamen.com", "cust@#$2024wmG"),
                    .EnableSsl = True
                }

                Dim mailMessage As New MailMessage() With {
                    .From = New MailAddress("customer.service@moamen.com"),
                    .Subject = subject,
                    .Body = body,
                    .IsBodyHtml = True ' Enable HTML formatting
                }

                For Each mail As String In emailAddresses
                    mailMessage.To.Add(mail)
                Next

                smtpClient.Send(mailMessage)
            End Using
        Catch ex As SmtpException
            MessageBox.Show("SMTP error sending email notification: " & ex.Message)
        Catch ex As Exception
            MessageBox.Show("Error sending email notification: " & ex.Message)
        End Try
    End Sub

    ' تحميل أكواد العملاء
    Private Sub LoadClientCodes()
        Try
            Using conn As New SqlConnection(connectionString)
                Dim query As String = "SELECT id, code FROM Clients"
                Dim cmd As New SqlCommand(query, conn)
                Dim dt As New DataTable()
                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                dt.Load(reader)
                cmbclientcode.DataSource = dt
                cmbclientcode.DisplayMember = "code"
                cmbclientcode.ValueMember = "id"
                cmbclientcode.SelectedIndex = -1
                conn.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading client codes: " & ex.Message)
        End Try
    End Sub

    ' دالة لإعادة تهيئة الفورم
    Private Sub ResetFormState()
        ' إخفاء الكنترولز الخاصة بالمشكلة
        dgvWorkOrders.Visible = False
        lblIssue.Visible = False
        txtIssue.Visible = False
        lblQtyM.Visible = False
        txtQtyM.Visible = False
        lblQtyKg.Visible = False
        txtQtyKg.Visible = False
        lblClientCode.Visible = False
        cmbclientcode.Visible = False
        btnSave.Visible = False

        ' إفراغ الداتا جريد
        dgvContracts.DataSource = Nothing
        dgvWorkOrders.DataSource = Nothing

        ' إفراغ الحقول
        txtIssue.Text = ""
        txtQtyM.Text = ""
        txtQtyKg.Text = ""
        cmbclientcode.SelectedIndex = -1

        ' إعادة تحميل العقود وأكواد العملاء
        LoadContracts()
        LoadClientCodes()
    End Sub

End Class