Imports System.Data.SqlClient
Imports System.Net
Imports Org.BouncyCastle.Asn1.Cmp

Public Class rawdisbursementform
    Private connectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"

    Private Sub rawdisbursementform_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadBatchComboBox()
        LoadWorderIDs()
        lblUsername.Text = "Logged in as: " & LoggedInUsername
        ' Retrieve the public_name for the logged-in user
        Dim publicName As String = GetPublicName(LoggedInUsername)
    End Sub

    Private Sub LoadBatchComboBox()
        Using conn As New SqlConnection(connectionString)
            conn.Open()

            ' Load cmbBatch
            Using cmd As New SqlCommand("SELECT DISTINCT batch FROM batch_raw", conn)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        cmbbatch.Items.Add(reader("batch").ToString())
                    End While
                End Using
            End Using
        End Using
    End Sub

    Private Sub LoadWorderIDs()
        Dim query As String = "SELECT DISTINCT worderid FROM techdata"

        Using conn As New SqlConnection(connectionString)
            Dim cmd As New SqlCommand(query, conn)
            Try
                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()

                cmbworder.Items.Clear()
                While reader.Read()
                    cmbworder.Items.Add(reader("worderid").ToString())
                End While
            Catch ex As Exception
                MessageBox.Show("Error loading Work Order IDs: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Sub cmbworder_TextChanged(sender As Object, e As EventArgs) Handles cmbworder.TextChanged
        Dim text As String = cmbworder.Text
        For Each item As String In cmbworder.Items
            If item.StartsWith(text, StringComparison.OrdinalIgnoreCase) Then
                cmbworder.SelectedItem = item
                cmbworder.SelectionStart = text.Length
                cmbworder.SelectionLength = item.Length - text.Length
                Exit For
            End If
        Next
    End Sub
    Private Sub cmbworder_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbworder.SelectedIndexChanged
        ' Clear previous data before loading new data
        lblbatchno.Text = "الرسالة: N/A"
        lblcontractno.Text = "رقم التعاقد: N/A"
        lblqtym.Text = "الكمية متر: N/A"
        lblqtykg.Text = "الكمية وزن: N/A"


        ' SQL Query to retrieve details for the selected Work Order
        Dim query As String = "SELECT td.worderid, c.contractno, c.batch, td.qty_m, td.qty_kg " &
                              "FROM techdata td " &
                              "LEFT JOIN Contracts c ON td.contract_id = c.ContractID " &
                              "WHERE td.worderid = @worderid"

        ' SQL Query to check if the Work Order exists in porow_toinspect
        Dim checkQuery As String = "SELECT qtykgstore FROM porow_toinspect WHERE worderid = @worderid" ' Query to check qty_m for the selected Work Order

        Using conn As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@worderid", cmbworder.Text)
                Try
                    conn.Open()
                    Dim reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        lblbatchno.Text = "الرسالة: " & If(reader("batch") IsNot DBNull.Value, reader("batch").ToString(), "N/A")
                        lblcontractno.Text = "رقم التعاقد: " & If(reader("contractno") IsNot DBNull.Value, reader("contractno").ToString(), "N/A")
                        lblqtym.Text = "الكمية متر: " & If(reader("qty_m") IsNot DBNull.Value, reader("qty_m").ToString(), "N/A")
                        lblqtykg.Text = "الكمية وزن: " & If(reader("qty_kg") IsNot DBNull.Value, reader("qty_kg").ToString(), "N/A")
                    End If
                    reader.Close() ' Close reader to execute next query
                Catch ex As Exception
                    MessageBox.Show("Error loading Work Order details: " & ex.Message)
                    Return
                End Try
            End Using


        End Using

    End Sub
    Private Sub cmbBatch_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbbatch.SelectedIndexChanged
        cmbLot.Items.Clear()
        LoadLotComboBox(cmbbatch.Text)
    End Sub

    Private Sub LoadLotComboBox(batch As String)
        Using conn As New SqlConnection(connectionString)
            conn.Open()

            ' Load cmbLot based on selected batch
            Using cmd As New SqlCommand("SELECT DISTINCT lot FROM batch_details WHERE batch_id = @batch", conn)
                cmd.Parameters.AddWithValue("@batch", batch)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        cmbLot.Items.Add(reader("lot").ToString())
                    End While
                End Using
            End Using
        End Using
    End Sub

    Private Sub cmbLot_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbLot.SelectedIndexChanged
        LoadStockData(cmbbatch.Text, cmbLot.Text)
    End Sub

    Private Sub btninsert_Click(sender As Object, e As EventArgs) Handles btninsert.Click
        Dim batch As String = cmbbatch.Text
        Dim lot As String = cmbLot.Text
        Dim issuedWeight As Decimal = Convert.ToDecimal(txtIssuedWeight.Text)
        Dim issuedRolls As Decimal = Convert.ToDecimal(txtIssuedRolls.Text)
        Dim issuedref As Integer = txtref.Text
        Dim worderid As String = cmbworder.Text

        If String.IsNullOrEmpty(batch) OrElse String.IsNullOrEmpty(lot) OrElse String.IsNullOrEmpty(issuedref) OrElse issuedWeight <= 0 OrElse issuedRolls <= 0 OrElse String.IsNullOrEmpty(worderid) Then
            MessageBox.Show("لا يسمح ب خانات فارغه مثل الرساله اللوت,الوزن المصروف,الاتواب المصروفه,اذن الصرف ")
            Return
        End If

        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()

                ' Check available stock
                Dim query As String = "SELECT weight_quantity, rolls_count FROM batch_details WHERE batch_id = @batch AND lot = @lot"
                Dim cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@batch", batch)
                cmd.Parameters.AddWithValue("@lot", lot)

                Dim reader As SqlDataReader = cmd.ExecuteReader()
                If reader.Read() Then
                    Dim availableWeight As Decimal = Convert.ToDecimal(reader("weight_quantity"))
                    Dim availableRolls As Decimal = Convert.ToDecimal(reader("rolls_count"))

                    If issuedWeight > availableWeight OrElse issuedRolls > availableRolls Then
                        MessageBox.Show("لايمكن صرف كمية اكبر من  المتاح فى المخزن")
                        Return
                    End If

                    reader.Close()

                    ' Insert issue record
                    query = "INSERT INTO raw_distribute (batch_id, lot, issued_weight, issued_rolls, issue_date, worderid,username,store_ref) VALUES (@batch, @lot, @issued_weight, @issued_rolls, @issue_date, @worderid,@username,@store_ref)"
                    cmd = New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@batch", batch)
                    cmd.Parameters.AddWithValue("@lot", lot)
                    cmd.Parameters.AddWithValue("@issued_weight", issuedWeight)
                    cmd.Parameters.AddWithValue("@issued_rolls", issuedRolls)
                    cmd.Parameters.AddWithValue("@issue_date", DateTime.Now)
                    cmd.Parameters.AddWithValue("@worderid", worderid)
                    cmd.Parameters.AddWithValue("@username", LoggedInUsername)
                    cmd.Parameters.AddWithValue("@store_ref", issuedref)
                    cmd.ExecuteNonQuery()

                    ' Update stock
                    query = "UPDATE batch_details SET weight_quantity = weight_quantity - @issued_weight, rolls_count = rolls_count - @issued_rolls WHERE batch_id = @batch AND lot = @lot"
                    cmd = New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@issued_weight", issuedWeight)
                    cmd.Parameters.AddWithValue("@issued_rolls", issuedRolls)
                    cmd.Parameters.AddWithValue("@batch", batch)
                    cmd.Parameters.AddWithValue("@lot", lot)
                    cmd.ExecuteNonQuery()

                    MessageBox.Show("Issue recorded successfully.")
                    LoadStockData(batch, lot)

                    ' Clear all labels, combo boxes, and text boxes
                    lblbatchno.Text = "الرسالة: N/A"
                    lblcontractno.Text = "رقم التعاقد: N/A"
                    lblqtym.Text = "الكمية متر: N/A"
                    lblqtykg.Text = "الكمية وزن: N/A"
                    cmbbatch.SelectedIndex = -1
                    cmbLot.SelectedIndex = -1
                    cmbworder.SelectedIndex = -1
                    txtIssuedWeight.Clear()
                    txtIssuedRolls.Clear()
                    txtref.Clear()
                    dgvStock.DataSource = Nothing
                Else
                    MessageBox.Show("Batch and lot not found.")
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Error issuing stock: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadStockData(batch As String, lot As String)
        Using conn As New SqlConnection(connectionString)
            conn.Open()

            Dim query As String = "SELECT batch_id AS 'Batch', lot AS 'Lot', weight_quantity AS 'الوزن المتاح', rolls_count AS 'الأتواب المتاحه' FROM batch_details WHERE batch_id = @batch AND lot = @lot"
            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@batch", batch)
            cmd.Parameters.AddWithValue("@lot", lot)
            Dim adapter As New SqlDataAdapter(cmd)
            Dim table As New DataTable()
            adapter.Fill(table)
            dgvStock.DataSource = table

            ' Call FormatDataGridView after loading data
            FormatDataGridView()
        End Using
    End Sub

    Private Sub FormatDataGridView()
        ' Center align text in DataGridView
        For Each column As DataGridViewColumn In dgvStock.Columns
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            column.DefaultCellStyle.Font = New Font(dgvStock.Font, FontStyle.Bold)
        Next

        ' Set header style
        dgvStock.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvStock.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue
        dgvStock.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black
        dgvStock.ColumnHeadersDefaultCellStyle.Font = New Font(dgvStock.Font.FontFamily, 12, FontStyle.Bold) ' Change the font size to 12
        dgvStock.EnableHeadersVisualStyles = False

        ' Adjust column widths to fill the DataGridView
        dgvStock.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    End Sub
    Private Function GetPublicName(ByVal username As String) As String
        Dim publicName As String = String.Empty
        Try
            Using conn As New SqlConnection(connectionString)
                ' SQL query to get the public_name from dep_users where username matches
                Dim query As String = "SELECT public_name FROM dep_users WHERE username = @username"
                Dim cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@username", username)

                conn.Open()
                ' Execute the query and retrieve the public_name
                Dim result = cmd.ExecuteScalar()
                If result IsNot Nothing Then
                    publicName = result.ToString()
                End If
                conn.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error retrieving public name: " & ex.Message)
        End Try
        Return publicName
    End Function
End Class

