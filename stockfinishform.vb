Imports System.Data.SqlClient
Imports System.Management
Imports System.Threading
Imports System.IO.Ports
Imports System.Net
Imports System.Net.Mail
Imports ZXing
Imports ZXing.Common

Public Class stockfinishform
    ' Declare the ComboBox control
    Private WithEvents cmbworderid As ComboBox
    Private WithEvents cmbLocation As ComboBox
    Private WithEvents lblLocation As Label
    Private WithEvents lblWorkOrder As Label
    Private WithEvents dgvStoreFinish As DataGridView
    Private WithEvents btnPullAll As Button
    Private connectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"
    Private hasUnsavedChanges As Boolean = False
    Private WithEvents serialPort As New SerialPort()
    Private isScaleConnected As Boolean = False
    Private saveIcon As System.Drawing.Image
    Private WithEvents lblusername As New Label()

    Private Sub SetupControls()
        ' Set form properties
        Me.WindowState = FormWindowState.Maximized
        Me.FormBorderStyle = FormBorderStyle.Sizable
        Me.RightToLeft = RightToLeft.Yes

        ' Setup username label
        lblusername.Text = "" & LoggedInUsername
        lblusername.Location = New Point(10, 10)
        lblusername.Size = New Size(200, 20)
        lblusername.Font = New Font("Arial", 12, FontStyle.Bold)
        Me.Controls.Add(lblusername)

        ' Load save icon
        Try
            saveIcon = System.Drawing.Image.FromFile("save_icon.png")
        Catch ex As Exception
            ' If icon file not found, create a default icon
            saveIcon = CreateDefaultSaveIcon()
        End Try

        ' Initialize Location Label
        lblLocation = New Label()
        lblLocation.Location = New Point(20, 23)
        lblLocation.Size = New Size(80, 25)
        lblLocation.Text = "الموقع:"
        lblLocation.Font = New Font("Arial", 14, FontStyle.Bold)
        lblLocation.TextAlign = ContentAlignment.MiddleLeft
        Me.Controls.Add(lblLocation)

        ' Initialize the Location ComboBox
        cmbLocation = New ComboBox()
        cmbLocation.Location = New Point(100, 20)
        cmbLocation.Size = New Size(200, 30)
        cmbLocation.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cmbLocation.AutoCompleteSource = AutoCompleteSource.ListItems
        cmbLocation.Font = New Font("Arial", 14, FontStyle.Bold)
        cmbLocation.DropDownHeight = 200
        Me.Controls.Add(cmbLocation)

        ' Initialize Work Order Label
        lblWorkOrder = New Label()
        lblWorkOrder.Location = New Point(20, 63)
        lblWorkOrder.Size = New Size(80, 25)
        lblWorkOrder.Text = "أمر الشغل:"
        lblWorkOrder.Font = New Font("Arial", 14, FontStyle.Bold)
        lblWorkOrder.TextAlign = ContentAlignment.MiddleLeft
        Me.Controls.Add(lblWorkOrder)

        ' Initialize the ComboBox
        cmbworderid = New ComboBox()
        cmbworderid.Location = New Point(100, 60)
        cmbworderid.Size = New Size(200, 30)
        cmbworderid.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cmbworderid.AutoCompleteSource = AutoCompleteSource.ListItems
        cmbworderid.Font = New Font("Arial", 14, FontStyle.Bold)
        cmbworderid.DropDownHeight = 200
        Me.Controls.Add(cmbworderid)

        ' Initialize Pull All Button
        btnPullAll = New Button()
        btnPullAll.Location = New Point(500, 60)
        btnPullAll.Size = New Size(150, 30)
        btnPullAll.Text = "سحب الجميع"
        btnPullAll.Font = New Font("Arial", 12, FontStyle.Bold)
        btnPullAll.BackColor = Color.LightGreen
        AddHandler btnPullAll.Click, AddressOf btnPullAll_Click
        Me.Controls.Add(btnPullAll)

        ' Initialize DataGridView
        dgvStoreFinish = New DataGridView()
        dgvStoreFinish.Location = New Point(100, 110)
        dgvStoreFinish.Size = New Size(Me.ClientSize.Width - 200, Me.ClientSize.Height - 150)
        dgvStoreFinish.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvStoreFinish.AllowUserToAddRows = False
        dgvStoreFinish.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvStoreFinish.DefaultCellStyle.Font = New Font("Arial", 12, FontStyle.Bold)
        dgvStoreFinish.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvStoreFinish.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 12, FontStyle.Bold)
        dgvStoreFinish.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue
        dgvStoreFinish.EnableHeadersVisualStyles = False
        AddHandler dgvStoreFinish.CellValueChanged, AddressOf dgvStoreFinish_CellValueChanged
        AddHandler dgvStoreFinish.CellClick, AddressOf dgvStoreFinish_CellClick
        AddHandler dgvStoreFinish.CellEnter, AddressOf dgvStoreFinish_CellEnter
        AddHandler dgvStoreFinish.KeyDown, AddressOf dgvStoreFinish_KeyDown
        Me.Controls.Add(dgvStoreFinish)

        LoadLocations()
        LoadWorderIDs()
    End Sub

    Private Sub LoadLocations()
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim query As String = "SELECT location FROM store_location where kind_store='2'"
                Using cmd As New SqlCommand(query, conn)
                    Dim dt As New DataTable()
                    dt.Load(cmd.ExecuteReader())

                    ' إعداد مصدر البيانات للقائمة المنسدلة
                    cmbLocation.DataSource = dt
                    cmbLocation.DisplayMember = "location"
                    cmbLocation.ValueMember = "location"
                    cmbLocation.SelectedIndex = -1

                    ' إضافة قائمة الاقتراحات التلقائية
                    Dim autoCompleteCollection As New AutoCompleteStringCollection()
                    For Each row As DataRow In dt.Rows
                        autoCompleteCollection.Add(row("location").ToString())
                    Next
                    cmbLocation.AutoCompleteCustomSource = autoCompleteCollection
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل المواقع: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub dgvStoreFinish_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvStoreFinish.CellClick
        If e.RowIndex >= 0 Then
            Dim columnName As String = dgvStoreFinish.Columns(e.ColumnIndex).Name

            If columnName = "سحب الاصل" Then
                ' Handle Pull Original button click
                Dim row As DataGridViewRow = dgvStoreFinish.Rows(e.RowIndex)
                Dim id As Integer = Convert.ToInt32(row.Cells("id").Value)
                Dim roll As Integer = Convert.ToInt32(row.Cells("رقم التوب").Value)
                Dim height As Double = Convert.ToDouble(row.Cells("اصل الطول").Value)
                Dim weight As Double = Convert.ToDouble(row.Cells("اصل الوزن").Value)

                ' Update the row with original values
                row.Cells("طول فعلى").Value = height
                row.Cells("وزن فعلى").Value = weight

                ' Update the database
                UpdateStoreFinish(id, height, weight)

            ElseIf columnName = "قراءة من الميزان" Then
                ' Check if location is selected
                If cmbLocation.SelectedIndex = -1 Then
                    MessageBox.Show("الرجاء اختيار الموقع أولاً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                ' Get the current row
                Dim currentRow As DataGridViewRow = dgvStoreFinish.Rows(e.RowIndex)

                ' Try to read weight from scale
                Dim weight As String = ReadWeightFromScale()
                If String.IsNullOrEmpty(weight) Then
                    ' If scale reading fails, ask user if they want to enter weight manually
                    If MessageBox.Show("فشلت قراءة الوزن من الميزان. هل تريد إدخال الوزن يدوياً؟", "تنبيه", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                        ' Allow manual entry for this cell
                        dgvStoreFinish.Columns("وزن فعلى").ReadOnly = False
                        dgvStoreFinish.CurrentCell = currentRow.Cells("وزن فعلى")
                        dgvStoreFinish.BeginEdit(True)
                    End If
                Else
                    ' Set the weight
                    currentRow.Cells("وزن فعلى").Value = weight

                    ' Update the database
                    Dim id As Integer = Convert.ToInt32(currentRow.Cells("id").Value)
                    Dim currentHeight As Double = Convert.ToDouble(currentRow.Cells("طول فعلى").Value)
                    UpdateStoreFinish(id, currentHeight, Convert.ToDouble(weight))

                    ' Trigger the CellValueChanged event to update calculations
                    Dim eventArgs As New DataGridViewCellEventArgs(dgvStoreFinish.Columns("وزن فعلى").Index, currentRow.Index)
                    dgvStoreFinish_CellValueChanged(dgvStoreFinish, eventArgs)
                End If
            End If
        End If
    End Sub

    Private Sub dgvStoreFinish_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs)
        If e.ColumnIndex >= 0 AndAlso dgvStoreFinish.Columns(e.ColumnIndex).Name = "وزن فعلى" Then
            Try
                ' Check if location is selected
                If cmbLocation.SelectedIndex = -1 Then
                    MessageBox.Show("الرجاء اختيار الموقع أولاً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    ' Revert the cell value
                    dgvStoreFinish.Rows(e.RowIndex).Cells("وزن فعلى").Value = "0"
                    Return
                End If

                Dim weight As Double
                Dim originalWeight As Double
                Dim calculatedLength As Double = 0
                Dim id As String = dgvStoreFinish.Rows(e.RowIndex).Cells("id").Value.ToString()

                ' Get the new weight value
                If Not Double.TryParse(dgvStoreFinish.Rows(e.RowIndex).Cells("وزن فعلى").Value?.ToString(), weight) Then
                    MessageBox.Show("الرجاء إدخال قيمة وزن صحيحة", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    dgvStoreFinish.Rows(e.RowIndex).Cells("وزن فعلى").Value = "0"
                    Return
                End If

                ' Get the original weight value
                If Not Double.TryParse(dgvStoreFinish.Rows(e.RowIndex).Cells("اصل الوزن").Value?.ToString(), originalWeight) Then
                    MessageBox.Show("خطأ في قراءة الوزن الأصلي", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If

                ' Check if the new weight exceeds the original weight
                If weight > originalWeight Then
                    MessageBox.Show("لا يمكن أن يكون الوزن الفعلي أكبر من الوزن الأصلي", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)

                    ' استرجاع القيم القديمة من قاعدة البيانات
                    Using conn As New SqlConnection(connectionString)
                        conn.Open()
                        Dim query As String = "SELECT weight, height FROM store_finish WHERE id = @id"
                        Using cmd As New SqlCommand(query, conn)
                            cmd.Parameters.AddWithValue("@id", id)
                            Using reader As SqlDataReader = cmd.ExecuteReader()
                                If reader.Read() Then
                                    ' استرجاع القيم القديمة
                                    dgvStoreFinish.Rows(e.RowIndex).Cells("وزن فعلى").Value = reader("weight").ToString()
                                    dgvStoreFinish.Rows(e.RowIndex).Cells("طول فعلى").Value = reader("height").ToString()
                                End If
                            End Using
                        End Using
                    End Using

                    ' تحديث ألوان الصفوف
                    ColorRows()
                    Return
                End If

                ' إذا كان الوزن الفعلي يساوي الوزن الأصلي، استخدم الطول الأصلي مباشرة
                If Math.Abs(weight - originalWeight) < 0.001 Then
                    calculatedLength = Double.Parse(dgvStoreFinish.Rows(e.RowIndex).Cells("اصل الطول").Value.ToString())
                Else
                    ' الحصول على قيمة متر لكل كيلو من العمود
                    Dim metersPerKg As Double
                    If Double.TryParse(dgvStoreFinish.Rows(e.RowIndex).Cells("متر لكل كيلو").Value.ToString(), metersPerKg) Then
                        ' حساب الطول الفعلي
                        calculatedLength = weight * metersPerKg
                    End If
                End If

                ' تحديث قيمة الطول
                dgvStoreFinish.Rows(e.RowIndex).Cells("طول فعلى").Value = calculatedLength.ToString("F2")

                ' Update row color after value change
                ColorRows()
                hasUnsavedChanges = True

                ' Save the changes immediately
                Using conn As New SqlConnection(connectionString)
                    conn.Open()
                    Dim query As String = "UPDATE store_finish SET weight = @weight, height = @height, status = 1, location = @location WHERE id = @id"
                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@weight", weight)
                        cmd.Parameters.AddWithValue("@height", calculatedLength)
                        cmd.Parameters.AddWithValue("@location", cmbLocation.SelectedValue.ToString())
                        cmd.Parameters.AddWithValue("@id", id)
                        cmd.ExecuteNonQuery()
                    End Using
                End Using

                hasUnsavedChanges = False
                MessageBox.Show("تم حفظ البيانات بنجاح", "تم", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Catch ex As Exception
                MessageBox.Show("Error calculating length: " & ex.Message)
            End Try
        End If
    End Sub

    Private Sub dgvStoreFinish_CellEnter(sender As Object, e As DataGridViewCellEventArgs)
        If hasUnsavedChanges Then
            ' Prevent moving to the next cell if there are unsaved changes
            dgvStoreFinish.CurrentCell = dgvStoreFinish.Rows(e.RowIndex).Cells("وزن فعلى")
            MessageBox.Show("الرجاء حفظ التغييرات قبل الانتقال للصف التالي", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub dgvStoreFinish_KeyDown(sender As Object, e As KeyEventArgs)
        If hasUnsavedChanges AndAlso (e.KeyCode = Keys.Enter OrElse e.KeyCode = Keys.Tab OrElse e.KeyCode = Keys.Down) Then
            ' Prevent moving to the next row if there are unsaved changes
            e.Handled = True
            MessageBox.Show("الرجاء حفظ التغييرات قبل الانتقال للصف التالي", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub LoadWorderIDs()
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()

                ' Query to get distinct worder_ids from store_finish table
                Dim query As String = "SELECT DISTINCT worder_id FROM store_finish"

                Using cmd As New SqlCommand(query, conn)
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        ' Clear existing items
                        cmbworderid.Items.Clear()

                        ' Add items to ComboBox
                        While reader.Read()
                            cmbworderid.Items.Add(reader("worder_id").ToString())
                        End While
                    End Using
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Error loading worder IDs: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cmbworderid_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbworderid.SelectedIndexChanged
        If hasUnsavedChanges Then
            Dim result As DialogResult = MessageBox.Show("هناك تغييرات غير محفوظة. هل تريد حفظ التغييرات قبل تغيير أمر الشغل؟", "تنبيه", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning)

            If result = DialogResult.Yes Then
                ' Update all changed rows
                For Each row As DataGridViewRow In dgvStoreFinish.Rows
                    Dim id As String = row.Cells("id").Value.ToString()
                    Dim weight As String = row.Cells("وزن فعلى").Value.ToString()
                    Dim length As String = row.Cells("طول فعلى").Value.ToString()

                    Using conn As New SqlConnection(connectionString)
                        conn.Open()
                        Dim query As String = "UPDATE store_finish SET weight = @weight, height = @height, status = 1 WHERE id = @id"
                        Using cmd As New SqlCommand(query, conn)
                            cmd.Parameters.AddWithValue("@weight", weight)
                            cmd.Parameters.AddWithValue("@height", length)
                            cmd.Parameters.AddWithValue("@id", id)
                            cmd.ExecuteNonQuery()
                        End Using
                    End Using
                Next
                hasUnsavedChanges = False
            ElseIf result = DialogResult.Cancel Then
                ' Cancel the selection change
                cmbworderid.SelectedIndex = cmbworderid.SelectedIndex
                Return
            End If
        End If

        LoadStoreFinishData()
    End Sub

    Private Sub LoadStoreFinishData()
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()

                ' Query to get store_finish data for selected worder_id
                Dim query As String = "SELECT id, worder_id, " &
                                    "CAST(roll as int) as 'رقم التوب', " &
                                    "fabric_grade as 'الدرجه', " &
                                    "heightpk as 'اصل الطول', " &
                                    "weightpk as 'اصل الوزن', " &
                                    "CAST(CASE WHEN weightpk = 0 THEN 0 ELSE heightpk / weightpk END as decimal(10,2)) as 'متر لكل كيلو', " &
                                    "height as 'طول فعلى', " &
                                    "weight as 'وزن فعلى' " &
                                    "FROM store_finish WHERE worder_id = @worder_id"

                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@worder_id", cmbworderid.SelectedItem.ToString())

                    Using adapter As New SqlDataAdapter(cmd)
                        Dim dt As New DataTable()
                        adapter.Fill(dt)

                        ' Clear existing columns and data
                        dgvStoreFinish.Columns.Clear()
                        dgvStoreFinish.DataSource = dt

                        ' Set default font for all cells
                        dgvStoreFinish.DefaultCellStyle.Font = New Font("Arial", 12, FontStyle.Regular)
                        dgvStoreFinish.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 12, FontStyle.Bold)
                        dgvStoreFinish.RowHeadersDefaultCellStyle.Font = New Font("Arial", 12, FontStyle.Regular)

                        ' Hide id and worder_id columns
                        If dgvStoreFinish.Columns.Contains("id") Then
                            dgvStoreFinish.Columns("id").Visible = False
                        End If
                        If dgvStoreFinish.Columns.Contains("worder_id") Then
                            dgvStoreFinish.Columns("worder_id").Visible = False
                        End If

                        ' Add Button Column for Pull Original at the start
                        Dim pullOriginalButtonColumn As New DataGridViewButtonColumn()
                        pullOriginalButtonColumn.Name = "سحب الاصل"
                        pullOriginalButtonColumn.HeaderText = "سحب الاصل"
                        pullOriginalButtonColumn.UseColumnTextForButtonValue = True
                        pullOriginalButtonColumn.Text = "سحب الاصل"
                        pullOriginalButtonColumn.DefaultCellStyle.BackColor = Color.FromArgb(255, 182, 193) ' Light Pink
                        pullOriginalButtonColumn.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 105, 180) ' Hot Pink
                        pullOriginalButtonColumn.DefaultCellStyle.ForeColor = Color.FromArgb(139, 0, 0) ' Dark Red
                        pullOriginalButtonColumn.DefaultCellStyle.SelectionForeColor = Color.White
                        pullOriginalButtonColumn.DefaultCellStyle.Font = New Font("Arial", 12, FontStyle.Bold)
                        pullOriginalButtonColumn.HeaderCell.Style.BackColor = Color.FromArgb(255, 182, 193) ' Light Pink
                        dgvStoreFinish.Columns.Add(pullOriginalButtonColumn)

                        ' Add Button Column for Read Weight at the end
                        Dim readWeightButtonColumn As New DataGridViewButtonColumn()
                        readWeightButtonColumn.Name = "قراءة من الميزان"
                        readWeightButtonColumn.HeaderText = "قراءة من الميزان"
                        readWeightButtonColumn.UseColumnTextForButtonValue = True
                        readWeightButtonColumn.Text = "قراءة من الميزان"
                        readWeightButtonColumn.DefaultCellStyle.BackColor = Color.FromArgb(144, 238, 144) ' Light Green
                        readWeightButtonColumn.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 128, 0) ' Green
                        readWeightButtonColumn.DefaultCellStyle.ForeColor = Color.FromArgb(0, 100, 0) ' Dark Green
                        readWeightButtonColumn.DefaultCellStyle.SelectionForeColor = Color.White
                        readWeightButtonColumn.DefaultCellStyle.Font = New Font("Arial", 12, FontStyle.Bold)
                        readWeightButtonColumn.HeaderCell.Style.BackColor = Color.FromArgb(144, 238, 144) ' Light Green
                        dgvStoreFinish.Columns.Add(readWeightButtonColumn)

                        ' Set weight column header color and make it editable
                        If dgvStoreFinish.Columns.Contains("وزن فعلى") Then
                            dgvStoreFinish.Columns("وزن فعلى").HeaderCell.Style.BackColor = Color.LightGreen
                            dgvStoreFinish.Columns("وزن فعلى").ReadOnly = False
                        End If

                        ' Set meters per kg column color
                        If dgvStoreFinish.Columns.Contains("متر لكل كيلو") Then
                            dgvStoreFinish.Columns("متر لكل كيلو").DefaultCellStyle.BackColor = Color.LightYellow
                        End If

                        ' Make other columns read-only, except for the weight and button columns
                        For Each col As DataGridViewColumn In dgvStoreFinish.Columns
                            If col.Name <> "وزن فعلى" AndAlso col.Name <> "سحب الاصل" AndAlso col.Name <> "قراءة من الميزان" Then
                                col.ReadOnly = True
                            End If
                        Next

                        ' Reorder columns
                        dgvStoreFinish.Columns("سحب الاصل").DisplayIndex = 0
                        dgvStoreFinish.Columns("رقم التوب").DisplayIndex = 1
                        dgvStoreFinish.Columns("الدرجه").DisplayIndex = 2
                        dgvStoreFinish.Columns("اصل الطول").DisplayIndex = 3
                        dgvStoreFinish.Columns("اصل الوزن").DisplayIndex = 4
                        dgvStoreFinish.Columns("متر لكل كيلو").DisplayIndex = 5
                        dgvStoreFinish.Columns("طول فعلى").DisplayIndex = 6
                        dgvStoreFinish.Columns("وزن فعلى").DisplayIndex = 7
                        dgvStoreFinish.Columns("قراءة من الميزان").DisplayIndex = 8

                        ' Color the rows based on actual values
                        ColorRows()

                        ' Set the entire column background color for both button columns
                        For Each row As DataGridViewRow In dgvStoreFinish.Rows
                            row.Cells("سحب الاصل").Style.BackColor = Color.FromArgb(255, 182, 193) ' Light Pink
                            row.Cells("قراءة من الميزان").Style.BackColor = Color.FromArgb(144, 238, 144) ' Light Green
                        Next

                        ' Adjust row height for better visibility
                        dgvStoreFinish.RowTemplate.Height = 35

                    End Using
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Error loading store finish data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ColorRows()
        For Each row As DataGridViewRow In dgvStoreFinish.Rows
            Dim actualLength As Double
            Dim actualWeight As Double
            Dim originalLength As Double
            Dim originalWeight As Double

            ' Try to parse the values
            Double.TryParse(row.Cells("طول فعلى").Value?.ToString(), actualLength)
            Double.TryParse(row.Cells("وزن فعلى").Value?.ToString(), actualWeight)
            Double.TryParse(row.Cells("اصل الطول").Value?.ToString(), originalLength)
            Double.TryParse(row.Cells("اصل الوزن").Value?.ToString(), originalWeight)

            ' Set row color based on conditions
            If actualLength = 0 AndAlso actualWeight = 0 Then
                ' Both actual values are 0 - Red
                row.DefaultCellStyle.BackColor = Color.LightPink
            ElseIf Math.Abs(actualLength - originalLength) < 0.01 AndAlso Math.Abs(actualWeight - originalWeight) < 0.01 Then
                ' Actual values match original values - Green
                row.DefaultCellStyle.BackColor = Color.LightGreen
            Else
                ' Actual values are different from original and not zero - Yellow
                row.DefaultCellStyle.BackColor = Color.LightYellow
            End If
        Next
    End Sub

    Private Sub PullOriginalData(rowIndex As Integer)
        Try
            ' Check if location is selected
            If cmbLocation.SelectedIndex = -1 Then
                MessageBox.Show("الرجاء اختيار الموقع أولاً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim row As DataGridViewRow = dgvStoreFinish.Rows(rowIndex)
            Dim id As String = row.Cells("id").Value.ToString()

            ' Get original values
            Dim originalLength As String = row.Cells("اصل الطول").Value.ToString()
            Dim originalWeight As String = row.Cells("اصل الوزن").Value.ToString()

            ' Set actual values to original values
            row.Cells("طول فعلى").Value = originalLength
            row.Cells("وزن فعلى").Value = originalWeight

            ' Update the database
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim query As String = "UPDATE store_finish SET weight = @weight, height = @height, status = 1, location = @location WHERE id = @id"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@weight", originalWeight)
                    cmd.Parameters.AddWithValue("@height", originalLength)
                    cmd.Parameters.AddWithValue("@location", cmbLocation.SelectedValue.ToString())
                    cmd.Parameters.AddWithValue("@id", id)
                    cmd.ExecuteNonQuery()
                End Using
            End Using

            ' Update row color
            ColorRows()
            hasUnsavedChanges = False

            MessageBox.Show("تم سحب وتحديث البيانات الاصلية بنجاح", "تم", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء سحب وتحديث البيانات الاصلية: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnPullAll_Click(sender As Object, e As EventArgs)
        Try
            ' Check if location is selected
            If cmbLocation.SelectedIndex = -1 Then
                MessageBox.Show("الرجاء اختيار الموقع أولاً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            If dgvStoreFinish.Rows.Count = 0 Then
                MessageBox.Show("لا توجد بيانات للتعامل معها", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Confirm with user
            Dim result As DialogResult = MessageBox.Show("هل أنت متأكد من سحب البيانات الاصلية لجميع الاتواب؟", "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = DialogResult.No Then
                Return
            End If

            ' Get all IDs and their original values
            Dim updates As New List(Of Tuple(Of String, String, String))()
            For Each row As DataGridViewRow In dgvStoreFinish.Rows
                Dim id As String = row.Cells("id").Value.ToString()
                Dim originalLength As String = row.Cells("اصل الطول").Value.ToString()
                Dim originalWeight As String = row.Cells("اصل الوزن").Value.ToString()
                updates.Add(New Tuple(Of String, String, String)(id, originalLength, originalWeight))
            Next

            ' Update database
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Using transaction As SqlTransaction = conn.BeginTransaction()
                    Try
                        Dim query As String = "UPDATE store_finish SET weight = @weight, height = @height, status = 1, location = @location WHERE id = @id"
                        Using cmd As New SqlCommand(query, conn, transaction)
                            For Each updateItem In updates
                                cmd.Parameters.Clear()
                                cmd.Parameters.AddWithValue("@weight", updateItem.Item3)
                                cmd.Parameters.AddWithValue("@height", updateItem.Item2)
                                cmd.Parameters.AddWithValue("@location", cmbLocation.SelectedValue.ToString())
                                cmd.Parameters.AddWithValue("@id", updateItem.Item1)
                                cmd.ExecuteNonQuery()
                            Next
                        End Using

                        transaction.Commit()

                        ' Reload the data to ensure accurate display
                        LoadStoreFinishData()

                        MessageBox.Show("تم سحب وتحديث البيانات الاصلية لجميع الاتواب بنجاح", "تم", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Catch ex As Exception
                        transaction.Rollback()
                        Throw
                    End Try
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء سحب وتحديث البيانات: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Function ReadWeightFromScale() As String
        Try
            If Not serialPort.IsOpen Then
                serialPort.ReadTimeout = 3000
                serialPort.WriteTimeout = 3000
                serialPort.Open()
            End If

            ' Clear buffers
            serialPort.DiscardInBuffer()
            serialPort.DiscardOutBuffer()

            ' Send command to request weight
            serialPort.WriteLine("W")
            System.Threading.Thread.Sleep(500)

            ' Read response
            Dim response As String = ""
            Dim buffer(256) As Byte
            Dim bytesRead As Integer = 0

            Try
                While serialPort.BytesToRead > 0 AndAlso bytesRead < buffer.Length
                    buffer(bytesRead) = CByte(serialPort.ReadByte())
                    bytesRead += 1
                End While

                If bytesRead > 0 Then
                    ' Convert bytes to string
                    response = System.Text.Encoding.ASCII.GetString(buffer, 0, bytesRead)

                    ' Clean and process the response
                    ' Remove any non-numeric characters except decimal point
                    response = New String(response.Where(Function(c) Char.IsDigit(c) OrElse c = "."c).ToArray())

                    ' Try to find a valid decimal number in the response
                    Dim match = System.Text.RegularExpressions.Regex.Match(response, "\d+\.?\d*")
                    If match.Success Then
                        Dim weightStr As String = match.Value
                        Dim weight As Decimal

                        If Decimal.TryParse(weightStr, weight) Then
                            ' Format with exactly 3 decimal places
                            weight = Decimal.Round(weight, 3)

                            ' Validate weight range (0.1 kg to 500 kg)
                            If weight >= 0.1D AndAlso weight <= 500D Then
                                Return weight.ToString("F3")
                            Else
                                MessageBox.Show($"الوزن {weight.ToString("F3")} كجم خارج النطاق المسموح به (0.1 كجم إلى 500 كجم)", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                Return String.Empty
                            End If
                        End If
                    End If

                    MessageBox.Show("تم استلام قيمة غير صالحة من الميزان", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return String.Empty
                End If

            Catch ex As TimeoutException
                MessageBox.Show("لم يتم استلام استجابة من الميزان في الوقت المحدد. يرجى المحاولة مرة أخرى.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return String.Empty
            End Try

            Return String.Empty

        Catch ex As Exception
            MessageBox.Show("خطأ في قراءة الوزن: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return String.Empty
        Finally
            If serialPort.IsOpen Then
                serialPort.Close()
            End If
        End Try
    End Function

    Private Sub UpdateStoreFinish(id As Integer, height As Double, weight As Double)
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim query As String = "UPDATE store_finish SET height = @height, weight = @weight WHERE id = @id"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@height", height)
                    cmd.Parameters.AddWithValue("@weight", weight)
                    cmd.Parameters.AddWithValue("@id", id)
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error updating store finish: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Protected Overrides Sub OnFormClosing(e As FormClosingEventArgs)
        If hasUnsavedChanges Then
            Dim result As DialogResult = MessageBox.Show("هناك تغييرات غير محفوظة. هل تريد حفظ التغييرات قبل الخروج؟", "تنبيه", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning)

            If result = DialogResult.Yes Then
                ' Update all changed rows
                For Each row As DataGridViewRow In dgvStoreFinish.Rows
                    Dim id As String = row.Cells("id").Value.ToString()
                    Dim weight As String = row.Cells("وزن فعلى").Value.ToString()
                    Dim length As String = row.Cells("طول فعلى").Value.ToString()

                    Using conn As New SqlConnection(connectionString)
                        conn.Open()
                        Dim query As String = "UPDATE store_finish SET weight = @weight, height = @height, status = 1 WHERE id = @id"
                        Using cmd As New SqlCommand(query, conn)
                            cmd.Parameters.AddWithValue("@weight", weight)
                            cmd.Parameters.AddWithValue("@height", length)
                            cmd.Parameters.AddWithValue("@id", id)
                            cmd.ExecuteNonQuery()
                        End Using
                    End Using
                Next
            ElseIf result = DialogResult.Cancel Then
                e.Cancel = True
                Return
            End If
        End If

        MyBase.OnFormClosing(e)
    End Sub

    Private Sub InitializeScale()
        Try
            ' Get all available COM ports
            Dim ports() As String = SerialPort.GetPortNames()
            If ports.Length = 0 Then
                MessageBox.Show("لم يتم العثور على أي منافذ COM متاحة. الرجاء التأكد من توصيل الجهاز.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                isScaleConnected = False
                Return
            End If

            ' Look for the Prolific port
            Dim prolificPort As String = Nothing
            For Each port In ports
                ' Use Windows Management Instrumentation (WMI) to get port details
                Using searcher As New Management.ManagementObjectSearcher(
                    "SELECT * FROM Win32_PnPEntity WHERE Caption LIKE '%Prolific PL2303GT%'")
                    For Each queryObj As Management.ManagementObject In searcher.Get()
                        If queryObj("Caption").ToString().Contains("Prolific PL2303GT") Then
                            ' Extract COM port number from the caption
                            Dim caption As String = queryObj("Caption").ToString()
                            Dim match As System.Text.RegularExpressions.Match =
                                System.Text.RegularExpressions.Regex.Match(caption, "COM[0-9]+")
                            If match.Success Then
                                prolificPort = match.Value
                                Exit For
                            End If
                        End If
                    Next
                End Using
                If prolificPort IsNot Nothing Then Exit For
            Next

            If prolificPort Is Nothing Then
                MessageBox.Show("لم يتم العثور على منفذ Prolific PL2303GT. الرجاء التأكد من توصيل الجهاز.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                isScaleConnected = False
                Return
            End If

            ' Configure serial port with longer timeouts
            serialPort.PortName = prolificPort
            serialPort.BaudRate = 9600
            serialPort.DataBits = 8
            serialPort.StopBits = StopBits.One
            serialPort.Parity = Parity.None
            serialPort.ReadTimeout = 3000  ' 3 seconds
            serialPort.WriteTimeout = 3000 ' 3 seconds
            serialPort.Handshake = Handshake.None
            serialPort.DtrEnable = True
            serialPort.RtsEnable = True

            ' Test the connection
            Try
                serialPort.Open()
                serialPort.Close()
                isScaleConnected = True
                MessageBox.Show("تم الاتصال بالميزان بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                isScaleConnected = False
                MessageBox.Show("لا يمكن الاتصال بالميزان. الرجاء التأكد من توصيل الجهاز وتثبيت برنامج التشغيل.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

        Catch ex As Exception
            isScaleConnected = False
            MessageBox.Show("خطأ في تهيئة المنفذ التسلسلي: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub



    Private Sub Form_Resize(sender As Object, e As EventArgs)
        ' Add form resize logic here
    End Sub

    Private Function CreateDefaultSaveIcon() As System.Drawing.Image
        ' Create a simple save icon
        Dim bmp As New Bitmap(32, 32)
        Using g As Graphics = Graphics.FromImage(bmp)
            g.Clear(Color.White)
            g.DrawRectangle(Pens.Black, 4, 4, 24, 24)
            g.DrawLine(Pens.Black, 8, 8, 20, 8)
            g.DrawLine(Pens.Black, 8, 12, 20, 12)
            g.DrawLine(Pens.Black, 8, 16, 20, 16)
            g.DrawLine(Pens.Black, 8, 20, 20, 20)
        End Using
        Return bmp
    End Function

    Private Sub stockfinishform_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Initialize scale connection
        InitializeScale()

        ' Set form properties
        Me.WindowState = FormWindowState.Maximized
        Me.FormBorderStyle = FormBorderStyle.Sizable
        Me.RightToLeft = RightToLeft.Yes

        ' Load save icon
        Try
            saveIcon = System.Drawing.Image.FromFile("save_icon.png")
        Catch ex As Exception
            ' If icon file not found, create a default icon
            saveIcon = CreateDefaultSaveIcon()
        End Try

        SetupControls()

        ' Force resize event to adjust controls
        Form_Resize(Nothing, Nothing)
        lblusername.Text = "" & LoggedInUsername
        ' Retrieve the public_name for the logged-in user

    End Sub
End Class
