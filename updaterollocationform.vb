Imports System.Data.SqlClient

Public Class updaterollocationform
    Private connectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"

    ' Controls declaration
    Private WithEvents cmbBatch As New ComboBox()
    Private WithEvents cmbLot As New ComboBox()
    Private WithEvents dgvRolls As New DataGridView()
    Private WithEvents cmbLocation As New ComboBox()
    Private WithEvents btnUpdate As New Button()
    Private WithEvents lblBatch As New Label()
    Private WithEvents lblLot As New Label()
    Private WithEvents lblLocation As New Label()
    Private WithEvents btnSelectAll As New Button()
    Private isAllSelected As Boolean = False

    Public Sub New()
        InitializeComponent()
        SetupControls()
    End Sub

    Private Sub SetupControls()
        ' Calculate center position
        Dim centerX As Integer = Me.ClientSize.Width \ 2

        ' Setup Batch Label and ComboBox
        lblBatch.Text = "رقم الرسالة:"
        lblBatch.Location = New Point(centerX + 190, 20)
        lblBatch.Size = New Size(100, 25)
        lblBatch.TextAlign = ContentAlignment.MiddleRight
        lblBatch.Font = New Font("Arial", 12, FontStyle.Bold)
        Me.Controls.Add(lblBatch)

        cmbBatch.Location = New Point(centerX + 10, 20)
        cmbBatch.Size = New Size(180, 25)
        cmbBatch.DropDownStyle = ComboBoxStyle.DropDown
        cmbBatch.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cmbBatch.AutoCompleteSource = AutoCompleteSource.CustomSource
        cmbBatch.Font = New Font("Arial", 12)
        Me.Controls.Add(cmbBatch)

        ' Setup Lot Label and ComboBox
        lblLot.Text = "رقم اللوت:"
        lblLot.Location = New Point(centerX - 90, 20)
        lblLot.Size = New Size(100, 25)
        lblLot.TextAlign = ContentAlignment.MiddleRight
        lblLot.Font = New Font("Arial", 12, FontStyle.Bold)
        Me.Controls.Add(lblLot)

        cmbLot.Location = New Point(centerX - 270, 20)
        cmbLot.Size = New Size(180, 25)
        cmbLot.DropDownStyle = ComboBoxStyle.DropDown
        cmbLot.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cmbLot.AutoCompleteSource = AutoCompleteSource.CustomSource
        cmbLot.Font = New Font("Arial", 12)
        Me.Controls.Add(cmbLot)

        ' Setup Location Label and ComboBox
        lblLocation.Text = "الموقع:"
        lblLocation.Location = New Point(centerX - 390, 20)
        lblLocation.Size = New Size(100, 25)
        lblLocation.TextAlign = ContentAlignment.MiddleRight
        lblLocation.Font = New Font("Arial", 12, FontStyle.Bold)
        Me.Controls.Add(lblLocation)

        cmbLocation.Location = New Point(centerX - 670, 20)
        cmbLocation.Size = New Size(280, 25)
        cmbLocation.DropDownStyle = ComboBoxStyle.DropDown
        cmbLocation.Font = New Font("Arial", 12)
        Me.Controls.Add(cmbLocation)

        ' Setup Select All Button
        btnSelectAll.Text = "تحديد الكل"
        btnSelectAll.Size = New Size(120, 30)
        btnSelectAll.Location = New Point(20, 60)
        btnSelectAll.Font = New Font("Arial", 12, FontStyle.Bold)
        btnSelectAll.BackColor = Color.FromArgb(0, 120, 215)
        btnSelectAll.ForeColor = Color.White
        btnSelectAll.FlatStyle = FlatStyle.Flat
        Me.Controls.Add(btnSelectAll)

        ' Setup DataGridView
        dgvRolls.Location = New Point(20, 100)
        dgvRolls.Size = New Size(Me.ClientSize.Width - 40, Me.ClientSize.Height - 160)
        dgvRolls.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvRolls.RightToLeft = RightToLeft.Yes
        dgvRolls.AllowUserToAddRows = False
        dgvRolls.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvRolls.MultiSelect = True
        dgvRolls.DefaultCellStyle.Font = New Font("Arial", 12)
        dgvRolls.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 12, FontStyle.Bold)

        ' Add columns to DataGridView
        Dim selectColumn As New DataGridViewCheckBoxColumn()
        selectColumn.Name = "Select"
        selectColumn.HeaderText = "اختيار"
        selectColumn.Width = 60
        dgvRolls.Columns.Add(selectColumn)

        dgvRolls.Columns.Add("RollNumber", "رقم التوب")
        dgvRolls.Columns.Add("Weight", "الوزن")
        dgvRolls.Columns.Add("CurrentLocation", "الموقع الحالي")

        Me.Controls.Add(dgvRolls)

        ' Setup Update Button
        btnUpdate.Text = "تحديث الموقع"
        btnUpdate.Size = New Size(200, 35)
        btnUpdate.Location = New Point(Me.ClientSize.Width - 220, Me.ClientSize.Height - 50)
        btnUpdate.Font = New Font("Arial", 12, FontStyle.Bold)
        btnUpdate.BackColor = Color.FromArgb(0, 120, 215)
        btnUpdate.ForeColor = Color.White
        btnUpdate.FlatStyle = FlatStyle.Flat
        btnUpdate.Enabled = False
        Me.Controls.Add(btnUpdate)

        ' Load initial data
        LoadBatches()
        LoadLocations()

        ' Add event handlers
        AddHandler cmbBatch.SelectedIndexChanged, AddressOf cmbBatch_SelectedIndexChanged
        AddHandler cmbLot.SelectedIndexChanged, AddressOf cmbLot_SelectedIndexChanged
        AddHandler btnUpdate.Click, AddressOf btnUpdate_Click
        AddHandler Me.Resize, AddressOf Form_Resize
        AddHandler cmbLocation.SelectedIndexChanged, AddressOf cmbLocation_SelectedIndexChanged
        AddHandler dgvRolls.CellValueChanged, AddressOf dgvRolls_CellValueChanged
        AddHandler btnSelectAll.Click, AddressOf btnSelectAll_Click
    End Sub

    Private Sub LoadBatches()
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim query As String = "SELECT DISTINCT batch FROM batch_details_rolls ORDER BY batch"
                Using cmd As New SqlCommand(query, conn)
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        cmbBatch.Items.Clear()
                        cmbBatch.AutoCompleteCustomSource.Clear()
                        While reader.Read()
                            Dim batchNumber As String = reader("batch").ToString()
                            cmbBatch.Items.Add(batchNumber)
                            cmbBatch.AutoCompleteCustomSource.Add(batchNumber)
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل أرقام الرسائل: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LoadLots(batchNumber As String)
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim query As String = "SELECT DISTINCT lot FROM batch_details_rolls WHERE batch = @batch ORDER BY lot"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@batch", batchNumber)
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        cmbLot.Items.Clear()
                        cmbLot.AutoCompleteCustomSource.Clear()
                        While reader.Read()
                            Dim lotNumber As String = reader("lot").ToString()
                            cmbLot.Items.Add(lotNumber)
                            cmbLot.AutoCompleteCustomSource.Add(lotNumber)
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل أرقام اللوت: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LoadLocations()
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim query As String = "SELECT location FROM store_location ORDER BY location"
                Using cmd As New SqlCommand(query, conn)
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        cmbLocation.Items.Clear()
                        While reader.Read()
                            cmbLocation.Items.Add(reader("location").ToString())
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل المواقع: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LoadRolls(batchNumber As String, lotNumber As String)
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim query As String = "SELECT roll AS RollNumber, weight AS Weight, location AS CurrentLocation " &
                                    "FROM batch_details_rolls " &
                                    "WHERE batch = @batch AND lot = @lot " &
                                    "ORDER BY roll"

                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@batch", batchNumber)
                    cmd.Parameters.AddWithValue("@lot", lotNumber)
                    Dim dt As New DataTable()
                    dt.Load(cmd.ExecuteReader())

                    dgvRolls.Rows.Clear()
                    For Each row As DataRow In dt.Rows
                        dgvRolls.Rows.Add(False, row("RollNumber"), row("Weight"),
                                        If(row("CurrentLocation") Is DBNull.Value, "", row("CurrentLocation")))
                    Next
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل الأتواب: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cmbBatch_SelectedIndexChanged(sender As Object, e As EventArgs)
        If cmbBatch.SelectedItem IsNot Nothing Then
            LoadLots(cmbBatch.SelectedItem.ToString())
            cmbLot.SelectedIndex = -1
            dgvRolls.Rows.Clear()
            btnUpdate.Enabled = False
        End If
    End Sub

    Private Sub cmbLot_SelectedIndexChanged(sender As Object, e As EventArgs)
        If cmbBatch.SelectedItem IsNot Nothing AndAlso cmbLot.SelectedItem IsNot Nothing Then
            LoadRolls(cmbBatch.SelectedItem.ToString(), cmbLot.SelectedItem.ToString())
            btnUpdate.Enabled = True
        End If
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs)
        If cmbBatch.SelectedItem Is Nothing OrElse cmbLot.SelectedItem Is Nothing Then
            MessageBox.Show("الرجاء اختيار الرسالة واللوت", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Check if any rows are selected
        Dim selectedRows = dgvRolls.Rows.Cast(Of DataGridViewRow).Where(Function(r) CBool(r.Cells("Select").Value)).ToList()
        If selectedRows.Count = 0 Then
            MessageBox.Show("الرجاء اختيار الأتواب المراد تحديث موقعها", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Check if location is selected
        If cmbLocation.SelectedItem Is Nothing Then
            MessageBox.Show("الرجاء اختيار الموقع الجديد", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Using transaction As SqlTransaction = conn.BeginTransaction()
                    Try
                        For Each row In selectedRows
                            Dim updateQuery As String = "UPDATE batch_details_rolls SET location = @location " &
                                                      "WHERE batch = @batch AND lot = @lot AND roll = @roll"

                            Using cmd As New SqlCommand(updateQuery, conn, transaction)
                                cmd.Parameters.AddWithValue("@location", cmbLocation.SelectedItem.ToString())
                                cmd.Parameters.AddWithValue("@batch", cmbBatch.SelectedItem.ToString())
                                cmd.Parameters.AddWithValue("@lot", cmbLot.SelectedItem.ToString())
                                cmd.Parameters.AddWithValue("@roll", row.Cells("RollNumber").Value)
                                cmd.ExecuteNonQuery()
                            End Using
                        Next

                        transaction.Commit()
                        MessageBox.Show("تم تحديث مواقع الأتواب بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)

                        ' Refresh the rolls grid
                        LoadRolls(cmbBatch.SelectedItem.ToString(), cmbLot.SelectedItem.ToString())

                    Catch ex As Exception
                        transaction.Rollback()
                        Throw
                    End Try
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("خطأ في تحديث مواقع الأتواب: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Form_Resize(sender As Object, e As EventArgs)
        ' Calculate center position
        Dim centerX As Integer = Me.ClientSize.Width \ 2

        ' Update control positions
        lblBatch.Left = centerX + 190
        cmbBatch.Left = centerX + 10
        lblLot.Left = centerX - 90
        cmbLot.Left = centerX - 270
        lblLocation.Left = centerX - 390
        cmbLocation.Left = centerX - 670

        ' Update grid size
        dgvRolls.Width = Me.ClientSize.Width - 40
        dgvRolls.Height = Me.ClientSize.Height - 160

        ' Update button positions
        btnUpdate.Left = Me.ClientSize.Width - 220
        btnUpdate.Top = Me.ClientSize.Height - 50
    End Sub

    Private Sub cmbLocation_SelectedIndexChanged(sender As Object, e As EventArgs)
        If cmbLocation.SelectedItem IsNot Nothing Then
            ' Update location for all selected rows
            For Each row As DataGridViewRow In dgvRolls.Rows
                If CBool(row.Cells("Select").Value) Then
                    row.Cells("CurrentLocation").Value = cmbLocation.SelectedItem.ToString()
                End If
            Next
        End If
    End Sub

    Private Sub dgvRolls_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgvRolls.CellValueChanged
        If e.ColumnIndex = dgvRolls.Columns("Select").Index AndAlso e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = dgvRolls.Rows(e.RowIndex)
            If CBool(row.Cells("Select").Value) AndAlso cmbLocation.SelectedItem IsNot Nothing Then
                row.Cells("CurrentLocation").Value = cmbLocation.SelectedItem.ToString()
            End If
        End If
    End Sub

    Private Sub btnSelectAll_Click(sender As Object, e As EventArgs)
        isAllSelected = Not isAllSelected
        btnSelectAll.Text = If(isAllSelected, "إلغاء تحديد الكل", "تحديد الكل")

        For Each row As DataGridViewRow In dgvRolls.Rows
            row.Cells("Select").Value = isAllSelected
        Next

        ' Update locations for all rows if a location is selected
        If cmbLocation.SelectedItem IsNot Nothing Then
            For Each row As DataGridViewRow In dgvRolls.Rows
                row.Cells("CurrentLocation").Value = cmbLocation.SelectedItem.ToString()
            Next
        End If
    End Sub
End Class