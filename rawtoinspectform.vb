Imports System.Data.SqlClient

Public Class rawtoinspectform
    Private connectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"
    Private WithEvents cmbBatch As New ComboBox()
    Private WithEvents cmbLot As New ComboBox()
    Private WithEvents cmbWorkOrder As New ComboBox()
    Private WithEvents dgvRolls As New DataGridView()
    Private WithEvents dgvSummary As New DataGridView()
    Private WithEvents chkSelectAll As New CheckBox()
    Private WithEvents txtBarcode As New TextBox()
    Private WithEvents btnIssue As New Button()
    Private WithEvents lblBatch As New Label()
    Private WithEvents lblLot As New Label()
    Private WithEvents lblWorkOrder As New Label()
    Private WithEvents lblBarcode As New Label()
    Private WithEvents lblTotalWeight As New Label()
    Private WithEvents lblusername As New Label()
    Private WithEvents txtrefstore As New TextBox()
    Private WithEvents lblrefstore As New Label()

    Private Sub RawToInspectForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set form properties
        Me.WindowState = FormWindowState.Maximized
        Me.FormBorderStyle = FormBorderStyle.Sizable
        Me.RightToLeft = RightToLeft.Yes

        lblusername.Text = "" & LoggedInUsername
        ' Retrieve the public_name for the logged-in user
        Dim publicName As String = GetPublicName(LoggedInUsername)

        SetupControls()
        LoadBatches()
        LoadWorkOrders()
    End Sub
    Private Function GetPublicName(ByVal username As String) As String
        Dim publicName As String = String.Empty
        Try
            Using connection As New SqlConnection(connectionString)
                ' SQL query to get the public_name from dep_users where username matches
                Dim query As String = "SELECT public_name FROM dep_users WHERE username = @username"
                Dim cmd As New SqlCommand(query, connection)
                cmd.Parameters.AddWithValue("@username", username)

                connection.Open()
                ' Execute the query and retrieve the public_name
                Dim result = cmd.ExecuteScalar()
                If result IsNot Nothing Then
                    publicName = result.ToString()
                End If
                connection.Close()
            End Using
        Catch ex As SqlException
            MessageBox.Show("Error retrieving public name: " & ex.Message)
        End Try
        Return publicName
    End Function
    Private Sub SetupControls()
        ' Setup Batch Label and ComboBox
        lblBatch.Text = "رقم الرسالة:"
        lblBatch.Location = New Point(20, 20)
        lblBatch.Size = New Size(80, 25)
        lblBatch.Font = New Font("Arial", 12, FontStyle.Bold)
        Me.Controls.Add(lblBatch)

        cmbBatch.Location = New Point(110, 20)
        cmbBatch.Size = New Size(150, 25)
        cmbBatch.Font = New Font("Arial", 12)
        cmbBatch.DropDownStyle = ComboBoxStyle.DropDown
        cmbBatch.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cmbBatch.AutoCompleteSource = AutoCompleteSource.ListItems
        Me.Controls.Add(cmbBatch)

        ' Setup Username Label
        lblusername.Location = New Point(10, 90) ' Top-left corner
        lblusername.Size = New Size(50, 25)
        lblusername.TextAlign = ContentAlignment.MiddleLeft
        lblusername.Font = New Font("Arial", 8)
        Me.Controls.Add(lblusername)

        ' Setup Lot Label and ComboBox
        lblLot.Text = "رقم اللوت:"
        lblLot.Location = New Point(280, 20)
        lblLot.Size = New Size(80, 25)
        lblLot.Font = New Font("Arial", 12, FontStyle.Bold)
        Me.Controls.Add(lblLot)

        cmbLot.Location = New Point(370, 20)
        cmbLot.Size = New Size(150, 25)
        cmbLot.Font = New Font("Arial", 12)
        cmbLot.DropDownStyle = ComboBoxStyle.DropDown
        cmbLot.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cmbLot.AutoCompleteSource = AutoCompleteSource.ListItems
        Me.Controls.Add(cmbLot)

        ' Setup Work Order Label and ComboBox
        lblWorkOrder.Text = "أمر الشغل:"
        lblWorkOrder.Location = New Point(540, 20)
        lblWorkOrder.Size = New Size(80, 25)
        lblWorkOrder.Font = New Font("Arial", 12, FontStyle.Bold)
        Me.Controls.Add(lblWorkOrder)

        cmbWorkOrder.Location = New Point(630, 20)
        cmbWorkOrder.Size = New Size(150, 25)
        cmbWorkOrder.Font = New Font("Arial", 12)
        cmbWorkOrder.DropDownStyle = ComboBoxStyle.DropDown
        cmbWorkOrder.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cmbWorkOrder.AutoCompleteSource = AutoCompleteSource.ListItems
        Me.Controls.Add(cmbWorkOrder)

        ' Move Ref Store Label and TextBox next to Work Order
        lblrefstore.Text = "رقم إذن المخزن:"
        lblrefstore.Location = New Point(800, 20)
        lblrefstore.Size = New Size(120, 25)
        lblrefstore.Font = New Font("Arial", 12, FontStyle.Bold)
        Me.Controls.Add(lblrefstore)

        txtrefstore.Location = New Point(925, 20)
        txtrefstore.Size = New Size(160, 25)
        txtrefstore.Font = New Font("Arial", 12)
        Me.Controls.Add(txtrefstore)

        ' Setup Barcode Label and TextBox
        lblBarcode.Text = "الباركود:"
        lblBarcode.Location = New Point(20, 90)
        lblBarcode.Size = New Size(80, 25)
        lblBarcode.Font = New Font("Arial", 12, FontStyle.Bold)
        Me.Controls.Add(lblBarcode)

        txtBarcode.Location = New Point(110, 90)
        txtBarcode.Size = New Size(200, 25)
        txtBarcode.Font = New Font("Arial", 12)
        Me.Controls.Add(txtBarcode)

        ' Setup Select All Checkbox
        chkSelectAll.Text = "تحديد الكل"
        chkSelectAll.Location = New Point(20, 100)
        chkSelectAll.Size = New Size(150, 25)
        chkSelectAll.Font = New Font("Arial", 12, FontStyle.Bold)
        Me.Controls.Add(chkSelectAll)

        ' Setup Total Weight Label
        lblTotalWeight.Text = "إجمالي الوزن: 0.000"
        lblTotalWeight.Location = New Point(200, 100)
        lblTotalWeight.Size = New Size(200, 25)
        lblTotalWeight.Font = New Font("Arial", 12, FontStyle.Bold)
        Me.Controls.Add(lblTotalWeight)

        ' Setup Issue Button
        btnIssue.Text = "صرف"
        btnIssue.Location = New Point(Me.ClientSize.Width - 170, 60)
        btnIssue.Size = New Size(150, 35)
        btnIssue.Font = New Font("Arial", 12, FontStyle.Bold)
        btnIssue.BackColor = Color.FromArgb(0, 120, 215)
        btnIssue.ForeColor = Color.White
        btnIssue.FlatStyle = FlatStyle.Flat
        Me.Controls.Add(btnIssue)

        ' Setup Rolls DataGridView
        dgvRolls.Location = New Point(20, 140)
        dgvRolls.Size = New Size(Me.ClientSize.Width - 40, Me.ClientSize.Height - 400)
        dgvRolls.Font = New Font("Arial", 12)
        dgvRolls.RightToLeft = RightToLeft.Yes
        dgvRolls.AllowUserToAddRows = False
        dgvRolls.AllowUserToDeleteRows = False
        dgvRolls.MultiSelect = False
        dgvRolls.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvRolls.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        ' إعادة ترتيب الأعمدة بالشكل المطلوب
        dgvRolls.Columns.Add(New DataGridViewCheckBoxColumn() With {
            .Name = "Select",
            .HeaderText = "تحديد",
            .Width = 70,
            .ReadOnly = False ' السماح بالتحديد
        })

        ' إضافة الأعمدة الأخرى مع جعلها readonly
        Dim batchColumn As New DataGridViewTextBoxColumn()
        With batchColumn
            .Name = "Batch"
            .HeaderText = "رقم الرسالة"
            .ReadOnly = True
        End With
        dgvRolls.Columns.Add(batchColumn)

        Dim lotColumn As New DataGridViewTextBoxColumn()
        With lotColumn
            .Name = "Lot"
            .HeaderText = "رقم اللوت"
            .ReadOnly = True
        End With
        dgvRolls.Columns.Add(lotColumn)

        Dim rollColumn As New DataGridViewTextBoxColumn()
        With rollColumn
            .Name = "Roll"
            .HeaderText = "رقم التوب"
            .ReadOnly = True
        End With
        dgvRolls.Columns.Add(rollColumn)

        Dim weightColumn As New DataGridViewTextBoxColumn()
        With weightColumn
            .Name = "Weight"
            .HeaderText = "الوزن الكلي"
            .ReadOnly = True
        End With
        dgvRolls.Columns.Add(weightColumn)

        Dim issueWeightColumn As New DataGridViewTextBoxColumn()
        With issueWeightColumn
            .Name = "IssueWeight"
            .HeaderText = "الوزن المطلوب صرفه"
            .ReadOnly = False ' السماح بالتعديل
        End With
        dgvRolls.Columns.Add(issueWeightColumn)

        Dim locationColumn As New DataGridViewTextBoxColumn()
        With locationColumn
            .Name = "Location"
            .HeaderText = "الموقع"
            .ReadOnly = True
        End With
        dgvRolls.Columns.Add(locationColumn)

        Me.Controls.Add(dgvRolls)

        ' Setup Summary DataGridView
        dgvSummary.Location = New Point(20, dgvRolls.Bottom + 10)
        dgvSummary.Size = New Size(Me.ClientSize.Width - 40, 100)
        dgvSummary.Font = New Font("Arial", 12)
        dgvSummary.RightToLeft = RightToLeft.Yes
        dgvSummary.AllowUserToAddRows = False
        dgvSummary.AllowUserToDeleteRows = False
        dgvSummary.ReadOnly = True
        dgvSummary.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        ' Add columns to Summary DataGridView
        dgvSummary.Columns.Add("TotalRolls", "عدد الأتواب")
        dgvSummary.Columns.Add("TotalWeight", "إجمالي الوزن")
        dgvSummary.Rows.Add(0, "0.000")
        Me.Controls.Add(dgvSummary)

        ' Add resize handler
        AddHandler Me.Resize, AddressOf Form_Resize
    End Sub

    Private Sub Form_Resize(sender As Object, e As EventArgs)
        ' Update controls sizes and positions
        dgvRolls.Width = Me.ClientSize.Width - 40
        dgvRolls.Height = Me.ClientSize.Height - 400

        dgvSummary.Location = New Point(20, dgvRolls.Bottom + 10)
        dgvSummary.Width = Me.ClientSize.Width - 40

        ' تحديث موضع زر الصرف
        btnIssue.Location = New Point(Me.ClientSize.Width - 170, 60)
    End Sub

    Private Sub LoadBatches()
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim query As String = "SELECT DISTINCT batch FROM batch_details_rolls ORDER BY batch DESC"
                Using cmd As New SqlCommand(query, conn)
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        cmbBatch.Items.Clear()
                        While reader.Read()
                            cmbBatch.Items.Add(reader("batch").ToString())
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل أرقام الرسائل: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LoadLots()
        If cmbBatch.SelectedItem Is Nothing Then Return

        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim query As String = "SELECT DISTINCT lot FROM batch_details_rolls WHERE batch = @batch ORDER BY lot"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@batch", cmbBatch.SelectedItem.ToString())
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        cmbLot.Items.Clear()
                        While reader.Read()
                            cmbLot.Items.Add(reader("lot").ToString())
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل أرقام اللوت: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LoadWorkOrders()
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim query As String = "SELECT DISTINCT worderid FROM techdata"
                Using cmd As New SqlCommand(query, conn)
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        cmbWorkOrder.Items.Clear()
                        While reader.Read()
                            cmbWorkOrder.Items.Add(reader("worderid").ToString())
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل أوامر الشغل: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LoadRolls()
        If String.IsNullOrEmpty(cmbBatch.Text) OrElse String.IsNullOrEmpty(cmbLot.Text) Then Return

        ' عند تحميل الأتواب، نمسح الجدول فقط إذا لم يكن هناك أتواب مختارة
        If dgvRolls.Rows.Count = 0 OrElse Not dgvRolls.Rows.Cast(Of DataGridViewRow).Any(Function(r) CBool(r.Cells("Select").Value)) Then
            dgvRolls.Rows.Clear()
        End If

        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim query As String = "SELECT id, batch, lot, roll, weight, location FROM batch_details_rolls " &
                                    "WHERE batch = @batch AND lot = @lot"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@batch", cmbBatch.Text)
                    cmd.Parameters.AddWithValue("@lot", cmbLot.Text)

                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim id As Integer = Convert.ToInt32(reader("id"))
                            Dim batch As String = reader("batch").ToString()
                            Dim lot As String = reader("lot").ToString()
                            Dim roll As String = reader("roll").ToString()
                            Dim weight As Decimal = Convert.ToDecimal(reader("weight"))
                            Dim location As String = If(reader("location") IsNot DBNull.Value, reader("location").ToString(), "")

                            ' التحقق من عدم وجود التوب مسبقاً في الجدول
                            Dim rollExists As Boolean = False
                            For Each row As DataGridViewRow In dgvRolls.Rows
                                If row.Tag?.ToString() = id.ToString() Then
                                    rollExists = True
                                    Exit For
                                End If
                            Next

                            If Not rollExists Then
                                Dim rowIndex As Integer = dgvRolls.Rows.Add(False, batch, lot, roll, weight.ToString("F3"), "", location)
                                dgvRolls.Rows(rowIndex).Tag = id
                            End If
                        End While
                    End Using
                End Using
            End Using

            UpdateSummary()
        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل بيانات الأتواب: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub UpdateSummary()
        Dim totalRolls As Integer = 0
        Dim totalWeight As Decimal = 0

        For Each row As DataGridViewRow In dgvRolls.Rows
            If CBool(row.Cells("Select").Value) Then
                totalRolls += 1
                ' استخدام الوزن المطلوب صرفه في حساب الإجمالي
                If Not String.IsNullOrEmpty(row.Cells("IssueWeight").Value?.ToString()) Then
                    totalWeight += Convert.ToDecimal(row.Cells("IssueWeight").Value)
                End If
            End If
        Next

        dgvSummary.Rows(0).Cells("TotalRolls").Value = totalRolls
        dgvSummary.Rows(0).Cells("TotalWeight").Value = totalWeight.ToString("F3")
        lblTotalWeight.Text = $"إجمالي الوزن: {totalWeight.ToString("F3")}"
    End Sub

    Private Sub cmbBatch_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbBatch.SelectedIndexChanged
        LoadLots()
        dgvRolls.Rows.Clear()
        UpdateSummary()
    End Sub

    Private Sub cmbLot_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbLot.SelectedIndexChanged
        LoadRolls()
        UpdateSummary()
    End Sub

    Private Sub chkSelectAll_CheckedChanged(sender As Object, e As EventArgs) Handles chkSelectAll.CheckedChanged
        For Each row As DataGridViewRow In dgvRolls.Rows
            row.Cells("Select").Value = chkSelectAll.Checked
            If chkSelectAll.Checked Then
                ' إذا تم تحديد الكل، قم بتعيين الوزن المطلوب صرفه بنفس الوزن الكلي
                row.Cells("IssueWeight").Value = row.Cells("Weight").Value
            Else
                ' إذا تم إلغاء التحديد، قم بتفريغ الوزن المطلوب صرفه
                row.Cells("IssueWeight").Value = ""
            End If
        Next
        UpdateSummary()
    End Sub


    Private Sub dgvRolls_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgvRolls.CellValueChanged
        If e.ColumnIndex = dgvRolls.Columns("IssueWeight").Index AndAlso e.RowIndex >= 0 Then
            ' التحقق من صحة القيمة المدخلة
            Dim row As DataGridViewRow = dgvRolls.Rows(e.RowIndex)
            Dim issueWeightCell As DataGridViewCell = row.Cells("IssueWeight")
            Dim totalWeight As Decimal = Convert.ToDecimal(row.Cells("Weight").Value)

            If Not String.IsNullOrEmpty(issueWeightCell.Value?.ToString()) Then
                Dim issueWeight As Decimal
                If Decimal.TryParse(issueWeightCell.Value.ToString(), issueWeight) Then
                    If issueWeight > totalWeight Then
                        MessageBox.Show("الوزن المطلوب صرفه يتجاوز الوزن الكلي للتوب", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        issueWeightCell.Value = totalWeight
                    End If
                Else
                    MessageBox.Show("الرجاء إدخال قيمة رقمية صحيحة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    issueWeightCell.Value = totalWeight
                End If
            End If

            UpdateSummary()
        ElseIf e.ColumnIndex = dgvRolls.Columns("Select").Index AndAlso e.RowIndex >= 0 Then
            UpdateSummary()
        End If
    End Sub

    Private Sub txtBarcode_KeyDown(sender As Object, e As KeyEventArgs) Handles txtBarcode.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True ' منع صوت البيب
            SearchByBarcode()
        End If
    End Sub

    Private Sub SearchByBarcode()
        Try
            Dim barcodeText As String = txtBarcode.Text.Trim()
            If String.IsNullOrEmpty(barcodeText) Then Return

            ' تقسيم الباركود إلى لوت ورقم توب
            Dim parts = barcodeText.Split("*"c)
            If parts.Length <> 2 Then
                MessageBox.Show("صيغة الباركود غير صحيحة. يجب أن تكون: رقم_اللوت*رقم_التوب", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            Dim lotNumber As String = parts(0)
            Dim rollNumber As String = parts(1)

            ' البحث عن اللوت والتوب مباشرة في قاعدة البيانات
            Using conn As New SqlConnection(connectionString)
                conn.Open()

                ' البحث عن اللوت والتوب
                Dim rollQuery As String = "SELECT * FROM batch_details_rolls WHERE lot = @lot AND roll = @roll"
                Using cmdRoll As New SqlCommand(rollQuery, conn)
                    cmdRoll.Parameters.AddWithValue("@lot", lotNumber)
                    cmdRoll.Parameters.AddWithValue("@roll", rollNumber)

                    Using reader As SqlDataReader = cmdRoll.ExecuteReader()
                        If reader.Read() Then
                            Dim batchId As String = reader("batch").ToString()
                            Dim weight As Decimal = Convert.ToDecimal(reader("weight"))
                            Dim location As String = If(reader("location") IsNot DBNull.Value, reader("location").ToString(), "")

                            ' البحث عن التوب في الجدول
                            Dim rollExists As Boolean = False
                            For Each row As DataGridViewRow In dgvRolls.Rows
                                If row.Cells("Roll").Value.ToString() = rollNumber AndAlso
                                   row.Cells("Lot").Value.ToString() = lotNumber Then
                                    rollExists = True
                                    row.Cells("Select").Value = True
                                    row.Selected = True
                                    dgvRolls.FirstDisplayedScrollingRowIndex = row.Index
                                    Exit For
                                End If
                            Next

                            If Not rollExists Then
                                ' إضافة التوب إلى الجدول
                                dgvRolls.Rows.Add(True, batchId, lotNumber, rollNumber, weight.ToString("F3"), weight.ToString("F3"), location)
                                dgvRolls.ClearSelection()
                                dgvRolls.Rows(dgvRolls.Rows.Count - 1).Selected = True
                                dgvRolls.FirstDisplayedScrollingRowIndex = dgvRolls.Rows.Count - 1
                                dgvRolls.Rows(dgvRolls.Rows.Count - 1).Tag = reader("id")
                            End If

                            ' تحديث إجمالي الوزن
                            UpdateSummary()
                        Else
                            MessageBox.Show("لم يتم العثور على التوب المطلوب", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        End If
                    End Using
                End Using
            End Using

            ' مسح محتوى حقل الباركود
            txtBarcode.Clear()

        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء البحث: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub UpdateTotalWeight()
        Dim totalWeight As Decimal = 0
        Dim totalRolls As Integer = 0

        For Each row As DataGridViewRow In dgvRolls.Rows
            If CBool(row.Cells("Select").Value) Then
                totalRolls += 1
                If Decimal.TryParse(row.Cells("Weight").Value.ToString(), totalWeight) Then
                    totalWeight += Decimal.Parse(row.Cells("Weight").Value.ToString())
                End If
            End If
        Next

        ' تحديث ملصق إجمالي الوزن
        lblTotalWeight.Text = $"إجمالي الوزن: {totalWeight.ToString("F3")}"

        ' تحديث جدول الملخص
        If dgvSummary.Rows.Count > 0 Then
            dgvSummary.Rows(0).Cells("TotalRolls").Value = totalRolls
            dgvSummary.Rows(0).Cells("TotalWeight").Value = totalWeight.ToString("F3")
        End If
    End Sub

    Private Function GetNextRefNo() As String
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim query As String = "SELECT TOP 1 refno FROM raw_to_inspect WHERE refno LIKE 'wmraw_inspect%' ORDER BY refno DESC"
                Using cmd As New SqlCommand(query, conn)
                    Dim lastRefNo As String = cmd.ExecuteScalar()?.ToString()
                    If Not String.IsNullOrEmpty(lastRefNo) Then
                        ' استخراج الرقم من آخر إذن
                        Dim numberPart As String = lastRefNo.Replace("wmraw_inspect", "")
                        Dim currentNumber As Integer
                        If Integer.TryParse(numberPart, currentNumber) Then
                            ' زيادة الرقم بواحد وتنسيقه بستة أرقام
                            Return $"wmraw_inspect{(currentNumber + 1).ToString("000000")}"
                        End If
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("خطأ في إنشاء رقم الإذن: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        ' إذا لم يتم العثور على أي إذن سابق أو حدث خطأ، نبدأ من الرقم 1
        Return "wmraw_inspect000001"
    End Function

    Private Sub ClearForm()
        ' مسح الكومبوبوكس
        cmbBatch.SelectedIndex = -1
        cmbLot.SelectedIndex = -1
        cmbWorkOrder.SelectedIndex = -1

        ' مسح الباركود
        txtBarcode.Clear()

        ' مسح الجداول
        dgvRolls.Rows.Clear()
        dgvSummary.Rows.Clear()
        dgvSummary.Rows.Add(0, "0.000")

        ' إعادة تعيين التشيك بوكس
        chkSelectAll.Checked = False

        ' إعادة تعيين إجمالي الوزن
        lblTotalWeight.Text = "إجمالي الوزن: 0.000"

        ' مسح محتوى حقل رقم إذن المخزن
        txtrefstore.Clear()
    End Sub

    Private Sub btnIssue_Click(sender As Object, e As EventArgs) Handles btnIssue.Click
        If cmbWorkOrder.SelectedItem Is Nothing Then
            MessageBox.Show("الرجاء اختيار أمر الشغل", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' شرط إدخال رقم إذن المخزن
        If String.IsNullOrWhiteSpace(txtrefstore.Text) Then
            MessageBox.Show("الرجاء إدخال رقم إذن المخزن", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim selectedRolls As Integer = 0
        For Each row As DataGridViewRow In dgvRolls.Rows
            If CBool(row.Cells("Select").Value) Then
                selectedRolls += 1
                ' التحقق من أن الوزن المتبقي للتوب أكبر من صفر
                Dim remainingWeight As Decimal = Convert.ToDecimal(row.Cells("Weight").Value)
                If remainingWeight <= 0 Then
                    MessageBox.Show($"لا يمكن صرف التوب رقم {row.Cells("Roll").Value} لأن رصيده صفر", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If
            End If
        Next

        If selectedRolls = 0 Then
            MessageBox.Show("الرجاء تحديد الأتواب المراد صرفها", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Using transaction As SqlTransaction = conn.BeginTransaction()
                    Try
                        Dim refNo As String = GetNextRefNo()

                        ' 1. إدراج في raw_to_inspect
                        Dim insertQuery As String = "INSERT INTO raw_to_inspect (date, worderid, batch_id_roll, batch, lot, weight, refno, username, ref_store) " &
                                                  "VALUES (@date, @worderid, @batch_id_roll, @batch, @lot, @weight, @refno, @username, @ref_store)"

                        ' 2. تحديث الوزن في batch_details_rolls - تم تعديل الاستعلام ليخصم الكمية المحددة فقط
                        Dim updateRollsQuery As String = "UPDATE batch_details_rolls SET weight = weight - @issue_weight WHERE id = @id"

                        ' 3. تحديث الوزن في batch_details
                        Dim updateBatchDetailsQuery As String = "UPDATE batch_details " &
                                                              "SET weight_quantity = weight_quantity - @weight " &
                                                              "WHERE batch_id = @batch_id AND lot = @lot"

                        Using cmdInsert As New SqlCommand(insertQuery, conn, transaction),
                              cmdUpdateRolls As New SqlCommand(updateRollsQuery, conn, transaction),
                              cmdUpdateBatchDetails As New SqlCommand(updateBatchDetailsQuery, conn, transaction)

                            For Each row As DataGridViewRow In dgvRolls.Rows
                                If CBool(row.Cells("Select").Value) Then
                                    ' التحقق من إدخال وزن صحيح للصرف
                                    Dim issueWeightCell As DataGridViewCell = row.Cells("IssueWeight")
                                    If String.IsNullOrEmpty(issueWeightCell.Value?.ToString()) Then
                                        MessageBox.Show($"الرجاء إدخال الوزن المطلوب صرفه للتوب رقم {row.Cells("Roll").Value}", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                        Return
                                    End If

                                    Dim totalWeight As Decimal = Convert.ToDecimal(row.Cells("Weight").Value)
                                    Dim issueWeight As Decimal = Convert.ToDecimal(issueWeightCell.Value)

                                    ' التحقق من أن الوزن المطلوب صرفه لا يتجاوز الوزن الكلي
                                    If issueWeight > totalWeight Then
                                        MessageBox.Show($"الوزن المطلوب صرفه للتوب رقم {row.Cells("Roll").Value} يتجاوز الوزن الكلي", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                        Return
                                    End If

                                    ' التحقق من أن الوزن المطلوب صرفه أكبر من صفر
                                    If issueWeight <= 0 Then
                                        MessageBox.Show($"لا يمكن صرف وزن صفر أو سالب للتوب رقم {row.Cells("Roll").Value}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                        Return
                                    End If

                                    ' إدراج في raw_to_inspect
                                    cmdInsert.Parameters.Clear()
                                    cmdInsert.Parameters.AddWithValue("@date", DateTime.Now)
                                    cmdInsert.Parameters.AddWithValue("@worderid", cmbWorkOrder.SelectedItem.ToString())
                                    cmdInsert.Parameters.AddWithValue("@batch_id_roll", row.Tag)
                                    cmdInsert.Parameters.AddWithValue("@batch", row.Cells("Batch").Value)
                                    cmdInsert.Parameters.AddWithValue("@lot", row.Cells("Lot").Value)
                                    cmdInsert.Parameters.AddWithValue("@weight", issueWeight)
                                    cmdInsert.Parameters.AddWithValue("@refno", refNo)
                                    cmdInsert.Parameters.AddWithValue("@username", LoggedInUsername)
                                    cmdInsert.Parameters.AddWithValue("@ref_store", txtrefstore.Text)
                                    cmdInsert.ExecuteNonQuery()

                                    ' تحديث batch_details_rolls
                                    cmdUpdateRolls.Parameters.Clear()
                                    cmdUpdateRolls.Parameters.AddWithValue("@id", row.Tag)
                                    cmdUpdateRolls.Parameters.AddWithValue("@issue_weight", issueWeight)
                                    cmdUpdateRolls.ExecuteNonQuery()

                                    ' تحديث batch_details
                                    cmdUpdateBatchDetails.Parameters.Clear()
                                    cmdUpdateBatchDetails.Parameters.AddWithValue("@weight", issueWeight)
                                    cmdUpdateBatchDetails.Parameters.AddWithValue("@batch_id", row.Cells("Batch").Value)
                                    cmdUpdateBatchDetails.Parameters.AddWithValue("@lot", row.Cells("Lot").Value)
                                    cmdUpdateBatchDetails.ExecuteNonQuery()
                                End If
                            Next
                        End Using

                        transaction.Commit()
                        MessageBox.Show($"تم الصرف بنجاح{vbCrLf}رقم الإذن: {refNo}", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)

                        ' مسح محتويات الفورم بعد الصرف الناجح
                        ClearForm()

                    Catch ex As Exception
                        transaction.Rollback()
                        Throw
                    End Try
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("خطأ في عملية الصرف: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub dgvRolls_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvRolls.CellContentClick
        If e.ColumnIndex = dgvRolls.Columns("Select").Index AndAlso e.RowIndex >= 0 Then
            Dim isChecked As Boolean = Not CBool(dgvRolls.Rows(e.RowIndex).Cells("Select").Value)
            dgvRolls.Rows(e.RowIndex).Cells("Select").Value = isChecked

            ' إذا تم التحديد، نضع الوزن الكلي في عمود الوزن المطلوب صرفه
            If isChecked Then
                dgvRolls.Rows(e.RowIndex).Cells("IssueWeight").Value = dgvRolls.Rows(e.RowIndex).Cells("Weight").Value
            Else
                dgvRolls.Rows(e.RowIndex).Cells("IssueWeight").Value = ""
            End If

            UpdateSummary()
        End If
    End Sub
End Class