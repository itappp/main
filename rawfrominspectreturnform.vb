Imports System.Data.SqlClient

Public Class rawfrominspectreturnform
    Private connectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"
    Private WithEvents cmbBatch As New ComboBox()
    Private WithEvents cmbLot As New ComboBox()
    Private WithEvents cmbWorkOrder As New ComboBox()
    Private WithEvents dgvRolls As New DataGridView()
    Private WithEvents dgvSummary As New DataGridView()
    Private WithEvents chkSelectAll As New CheckBox()
    Private WithEvents txtBarcode As New TextBox()
    Private WithEvents btnReturn As New Button()
    Private WithEvents lblBatch As New Label()
    Private WithEvents lblLot As New Label()
    Private WithEvents lblWorkOrder As New Label()
    Private WithEvents lblBarcode As New Label()
    Private WithEvents lblTotalWeight As New Label()
    Private WithEvents lblusername As New Label()

    Private Sub RawToInspectReturnForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set form properties
        Me.WindowState = FormWindowState.Maximized
        Me.FormBorderStyle = FormBorderStyle.Sizable
        Me.RightToLeft = RightToLeft.Yes
        Me.Text = "مرتجع خام للتفتيش"

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
                Dim query As String = "SELECT public_name FROM dep_users WHERE username = @username"
                Dim cmd As New SqlCommand(query, connection)
                cmd.Parameters.AddWithValue("@username", username)

                connection.Open()
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
        lblusername.Location = New Point(10, 90)
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

        ' Setup Barcode Label and TextBox
        lblBarcode.Text = "الباركود:"
        lblBarcode.Location = New Point(20, 60)
        lblBarcode.Size = New Size(80, 25)
        lblBarcode.Font = New Font("Arial", 12, FontStyle.Bold)
        Me.Controls.Add(lblBarcode)

        txtBarcode.Location = New Point(110, 60)
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

        ' Setup Return Button
        btnReturn.Text = "مرتجع"
        btnReturn.Location = New Point(Me.ClientSize.Width - 170, 60)
        btnReturn.Size = New Size(150, 35)
        btnReturn.Font = New Font("Arial", 12, FontStyle.Bold)
        btnReturn.BackColor = Color.FromArgb(0, 120, 215)
        btnReturn.ForeColor = Color.White
        btnReturn.FlatStyle = FlatStyle.Flat
        Me.Controls.Add(btnReturn)

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

        ' Add columns to DataGridView
        dgvRolls.Columns.Add(New DataGridViewCheckBoxColumn() With {
            .Name = "Select",
            .HeaderText = "تحديد",
            .Width = 70,
            .ReadOnly = False
        })

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

        Dim returnWeightColumn As New DataGridViewTextBoxColumn()
        With returnWeightColumn
            .Name = "ReturnWeight"
            .HeaderText = "الوزن المطلوب إرجاعه"
            .ReadOnly = False
        End With
        dgvRolls.Columns.Add(returnWeightColumn)

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

        dgvSummary.Columns.Add("TotalRolls", "عدد الأتواب")
        dgvSummary.Columns.Add("TotalWeight", "إجمالي الوزن")
        dgvSummary.Rows.Add(0, "0.000")
        Me.Controls.Add(dgvSummary)

        AddHandler Me.Resize, AddressOf Form_Resize
    End Sub

    Private Sub Form_Resize(sender As Object, e As EventArgs)
        dgvRolls.Width = Me.ClientSize.Width - 40
        dgvRolls.Height = Me.ClientSize.Height - 400

        dgvSummary.Location = New Point(20, dgvRolls.Bottom + 10)
        dgvSummary.Width = Me.ClientSize.Width - 40

        btnReturn.Location = New Point(Me.ClientSize.Width - 170, 60)
    End Sub

    Private Sub LoadBatches()
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim query As String = "SELECT DISTINCT batch FROM raw_to_inspect ORDER BY batch DESC"
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
                Dim query As String = "SELECT DISTINCT lot FROM raw_to_inspect WHERE batch = @batch ORDER BY lot"
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
                Dim query As String = "SELECT DISTINCT worderid FROM raw_to_inspect WHERE worderid IS NOT NULL"
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

        If dgvRolls.Rows.Count = 0 OrElse Not dgvRolls.Rows.Cast(Of DataGridViewRow).Any(Function(r) CBool(r.Cells("Select").Value)) Then
            dgvRolls.Rows.Clear()
        End If

        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()

                ' جلب بيانات الأتواب أولاً في قائمة مؤقتة
                Dim rollsList As New List(Of Tuple(Of Integer, String, String, String, Decimal))
                Dim query As String = "SELECT raw_to_inspect.id, raw_to_inspect.batch, raw_to_inspect.lot, " &
                                  "batch_details_rolls.roll, raw_to_inspect.weight " &
                                  "FROM raw_to_inspect " &
                                  "LEFT JOIN batch_details_rolls ON raw_to_inspect.batch_id_roll = batch_details_rolls.id " &
                                  "WHERE raw_to_inspect.batch = @batch AND raw_to_inspect.lot = @lot"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@batch", cmbBatch.Text)
                    cmd.Parameters.AddWithValue("@lot", cmbLot.Text)

                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim id As Integer = Convert.ToInt32(reader("id"))
                            Dim batch As String = reader("batch").ToString()
                            Dim lot As String = reader("lot").ToString()
                            Dim roll As String = If(reader("roll") IsNot DBNull.Value, reader("roll").ToString(), "")
                            Dim weight As Decimal = Convert.ToDecimal(reader("weight"))
                            rollsList.Add(Tuple.Create(id, batch, lot, roll, weight))
                        End While
                    End Using ' هنا يتم إغلاق الـ reader
                End Using

                ' الآن أضف الصفوف للـ DataGridView بعد إغلاق الـ reader
                For Each rollData In rollsList
                    Dim id = rollData.Item1
                    Dim batch = rollData.Item2
                    Dim lot = rollData.Item3
                    Dim roll = rollData.Item4
                    Dim weight = rollData.Item5

                    ' التحقق من عدم وجود التوب مسبقاً في الجدول
                    Dim rollExists As Boolean = False
                    For Each row As DataGridViewRow In dgvRolls.Rows
                        If row.Tag?.ToString() = id.ToString() Then
                            rollExists = True
                            Exit For
                        End If
                    Next

                    If Not rollExists Then
                        ' حساب الوزن المرتجع من raw_return_inspect
                        Dim returnedWeight As Decimal = 0
                        Using returnCmd As New SqlCommand("SELECT ISNULL(SUM(weight), 0) FROM raw_return_inspect WHERE batch_id_roll = @id", conn)
                            returnCmd.Parameters.AddWithValue("@id", id)
                            returnedWeight = Convert.ToDecimal(returnCmd.ExecuteScalar())
                        End Using

                        ' حساب الرصيد المتاح
                        Dim availableWeight As Decimal = weight - returnedWeight

                        ' إضافة الصف إلى DataGridView مع الرصيد المتاح في عمود الوزن الكلي
                        Dim rowIndex As Integer = dgvRolls.Rows.Add(False, batch, lot, roll, availableWeight.ToString("F3"), "")
                        dgvRolls.Rows(rowIndex).Tag = id
                        ' إذا الوزن الكلي = 0 اجعل خانة التحديد غير مفعلة
                        If availableWeight = 0 Then
                            dgvRolls.Rows(rowIndex).Cells("Select").ReadOnly = True
                            dgvRolls.Rows(rowIndex).DefaultCellStyle.BackColor = Color.Red ' تمييز الصف باللون الأحمر
                        End If
                    End If
                Next
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
                If Not String.IsNullOrEmpty(row.Cells("ReturnWeight").Value?.ToString()) Then
                    totalWeight += Convert.ToDecimal(row.Cells("ReturnWeight").Value)
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
            ' لا تحدد الصف إذا الوزن الكلي = 0
            If Not row.Cells("Select").ReadOnly Then
                row.Cells("Select").Value = chkSelectAll.Checked
                If chkSelectAll.Checked Then
                    row.Cells("returnWeight").Value = row.Cells("Weight").Value
                Else
                    row.Cells("returnWeight").Value = ""
                End If
            End If
        Next
        UpdateSummary()
    End Sub

    Private Sub dgvRolls_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgvRolls.CellValueChanged
        If e.ColumnIndex = dgvRolls.Columns("ReturnWeight").Index AndAlso e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = dgvRolls.Rows(e.RowIndex)
            Dim returnWeightCell As DataGridViewCell = row.Cells("ReturnWeight")
            Dim totalWeight As Decimal = Convert.ToDecimal(row.Cells("Weight").Value)

            If Not String.IsNullOrEmpty(returnWeightCell.Value?.ToString()) Then
                Dim returnWeight As Decimal
                If Decimal.TryParse(returnWeightCell.Value.ToString(), returnWeight) Then
                    If returnWeight > totalWeight Then
                        MessageBox.Show("الوزن المطلوب إرجاعه يتجاوز الوزن الكلي للتوب", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        returnWeightCell.Value = totalWeight
                    End If
                Else
                    MessageBox.Show("الرجاء إدخال قيمة رقمية صحيحة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    returnWeightCell.Value = totalWeight
                End If
            End If

            UpdateSummary()
        ElseIf e.ColumnIndex = dgvRolls.Columns("Select").Index AndAlso e.RowIndex >= 0 Then
            UpdateSummary()
        End If
    End Sub

    Private Sub txtBarcode_KeyDown(sender As Object, e As KeyEventArgs) Handles txtBarcode.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            SearchByBarcode()
        End If
    End Sub

    Private Sub SearchByBarcode()
        Try
            Dim barcodeText As String = txtBarcode.Text.Trim()
            If String.IsNullOrEmpty(barcodeText) Then Return

            Dim parts = barcodeText.Split("*"c)
            If parts.Length <> 2 Then
                MessageBox.Show("صيغة الباركود غير صحيحة. يجب أن تكون: رقم_اللوت*رقم_التوب", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            Dim lotNumber As String = parts(0)
            Dim rollNumber As String = parts(1)

            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim rollQuery As String = "SELECT * FROM raw_to_inspect WHERE lot = @lot AND roll = @roll"
                Using cmdRoll As New SqlCommand(rollQuery, conn)
                    cmdRoll.Parameters.AddWithValue("@lot", lotNumber)
                    cmdRoll.Parameters.AddWithValue("@roll", rollNumber)

                    Using reader As SqlDataReader = cmdRoll.ExecuteReader()
                        If reader.Read() Then
                            Dim batchId As String = reader("batch").ToString()
                            Dim weight As Decimal = Convert.ToDecimal(reader("weight"))
                            Dim location As String = If(reader("location") IsNot DBNull.Value, reader("location").ToString(), "")

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
                                dgvRolls.Rows.Add(True, batchId, lotNumber, rollNumber, weight.ToString("F3"), weight.ToString("F3"), location)
                                dgvRolls.ClearSelection()
                                dgvRolls.Rows(dgvRolls.Rows.Count - 1).Selected = True
                                dgvRolls.FirstDisplayedScrollingRowIndex = dgvRolls.Rows.Count - 1
                                dgvRolls.Rows(dgvRolls.Rows.Count - 1).Tag = reader("id")
                            End If

                            UpdateSummary()
                        Else
                            MessageBox.Show("لم يتم العثور على التوب المطلوب", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        End If
                    End Using
                End Using
            End Using

            txtBarcode.Clear()

        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء البحث: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Function GetNextRefNo() As String
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim query As String = "SELECT TOP 1 refno FROM raw_to_inspect WHERE refno LIKE 'wmraw_inspect_return%' ORDER BY refno DESC"
                Using cmd As New SqlCommand(query, conn)
                    Dim lastRefNo As String = cmd.ExecuteScalar()?.ToString()
                    If Not String.IsNullOrEmpty(lastRefNo) Then
                        Dim numberPart As String = lastRefNo.Replace("wmraw_inspect_return", "")
                        Dim currentNumber As Integer
                        If Integer.TryParse(numberPart, currentNumber) Then
                            Return $"wmraw_inspect_return{(currentNumber + 1).ToString("000000")}"
                        End If
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("خطأ في إنشاء رقم الإذن: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Return "wmraw_inspect_return000001"
    End Function

    Private Sub ClearForm()
        cmbBatch.SelectedIndex = -1
        cmbLot.SelectedIndex = -1
        cmbWorkOrder.SelectedIndex = -1
        txtBarcode.Clear()
        dgvRolls.Rows.Clear()
        dgvSummary.Rows.Clear()
        dgvSummary.Rows.Add(0, "0.000")
        chkSelectAll.Checked = False
        lblTotalWeight.Text = "إجمالي الوزن: 0.000"
    End Sub

    Private Sub btnReturn_Click(sender As Object, e As EventArgs) Handles btnReturn.Click
        Dim selectedRolls As Integer = 0
        For Each row As DataGridViewRow In dgvRolls.Rows
            If CBool(row.Cells("Select").Value) Then
                selectedRolls += 1
            End If
        Next

        If selectedRolls = 0 Then
            MessageBox.Show("الرجاء تحديد الأتواب المراد إرجاعها", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Using transaction As SqlTransaction = conn.BeginTransaction()
                    Try
                        Dim refNo As String = GetNextRefNo()

                        ' 1. إدراج في raw_to_inspect_return
                        Dim insertQuery As String = "INSERT INTO raw_return_inspect (date, batch_id_roll, batch, lot, weight, refno, username) " &
                                              "VALUES (@date, @batch_id_roll, @batch, @lot, @weight, @refno, @username)"

                        ' 2. تحديث الوزن في batch_details_rolls
                        Dim updateRollsQuery As String = "UPDATE batch_details_rolls SET weight = weight + @return_weight WHERE batch = @batch AND lot = @lot AND roll = @roll"

                        ' 3. تحديث الوزن في batch_details
                        Dim updateBatchDetailsQuery As String = "UPDATE batch_details " &
                                                          "SET weight_quantity = weight_quantity + @weight " &
                                                          "WHERE batch_id = @batch_id AND lot = @lot"

                        Using cmdInsert As New SqlCommand(insertQuery, conn, transaction),
                          cmdUpdateRolls As New SqlCommand(updateRollsQuery, conn, transaction),
                          cmdUpdateBatchDetails As New SqlCommand(updateBatchDetailsQuery, conn, transaction)

                            For Each row As DataGridViewRow In dgvRolls.Rows
                                If CBool(row.Cells("Select").Value) Then
                                    Dim returnWeightCell As DataGridViewCell = row.Cells("ReturnWeight")
                                    If String.IsNullOrEmpty(returnWeightCell.Value?.ToString()) Then
                                        MessageBox.Show($"الرجاء إدخال الوزن المطلوب إرجاعه للتوب رقم {row.Cells("Roll").Value}", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                        Return
                                    End If

                                    Dim returnWeight As Decimal = Convert.ToDecimal(returnWeightCell.Value)

                                    ' إدراج في raw_to_inspect_return
                                    cmdInsert.Parameters.Clear()
                                    cmdInsert.Parameters.AddWithValue("@date", DateTime.Now)
                                    cmdInsert.Parameters.AddWithValue("@batch_id_roll", row.Tag)
                                    cmdInsert.Parameters.AddWithValue("@batch", row.Cells("Batch").Value)
                                    cmdInsert.Parameters.AddWithValue("@lot", row.Cells("Lot").Value)
                                    cmdInsert.Parameters.AddWithValue("@weight", returnWeight)
                                    cmdInsert.Parameters.AddWithValue("@refno", refNo)
                                    cmdInsert.Parameters.AddWithValue("@username", LoggedInUsername)
                                    cmdInsert.ExecuteNonQuery()

                                    ' تحديث batch_details_rolls
                                    cmdUpdateRolls.Parameters.Clear()
                                    cmdUpdateRolls.Parameters.AddWithValue("@batch", row.Cells("Batch").Value)
                                    cmdUpdateRolls.Parameters.AddWithValue("@lot", row.Cells("Lot").Value)
                                    cmdUpdateRolls.Parameters.AddWithValue("@roll", row.Cells("Roll").Value)
                                    cmdUpdateRolls.Parameters.AddWithValue("@return_weight", returnWeight)
                                    cmdUpdateRolls.ExecuteNonQuery()

                                    ' تحديث batch_details
                                    cmdUpdateBatchDetails.Parameters.Clear()
                                    cmdUpdateBatchDetails.Parameters.AddWithValue("@weight", returnWeight)
                                    cmdUpdateBatchDetails.Parameters.AddWithValue("@batch_id", row.Cells("Batch").Value)
                                    cmdUpdateBatchDetails.Parameters.AddWithValue("@lot", row.Cells("Lot").Value)
                                    cmdUpdateBatchDetails.ExecuteNonQuery()
                                End If
                            Next
                        End Using

                        transaction.Commit()
                        MessageBox.Show($"تم الإرجاع بنجاح{vbCrLf}رقم الإذن: {refNo}", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)

                        ClearForm()

                    Catch ex As Exception
                        transaction.Rollback()
                        Throw
                    End Try
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("خطأ في عملية الإرجاع: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub dgvRolls_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvRolls.CellContentClick
        If e.ColumnIndex = dgvRolls.Columns("Select").Index AndAlso e.RowIndex >= 0 Then
            ' لا تسمح بتحديد الصف إذا الوزن الكلي = 0
            If dgvRolls.Rows(e.RowIndex).Cells("Select").ReadOnly Then Return
            Dim isChecked As Boolean = Not CBool(dgvRolls.Rows(e.RowIndex).Cells("Select").Value)
            dgvRolls.Rows(e.RowIndex).Cells("Select").Value = isChecked

            If isChecked Then
                dgvRolls.Rows(e.RowIndex).Cells("ReturnWeight").Value = dgvRolls.Rows(e.RowIndex).Cells("Weight").Value
            Else
                dgvRolls.Rows(e.RowIndex).Cells("ReturnWeight").Value = ""
            End If

            UpdateSummary()
        End If
    End Sub
End Class
