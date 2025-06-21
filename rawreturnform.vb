Imports System.Data.SqlClient

Public Class rawreturnform
    Private connectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"

    Private Sub rawreturnform_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadBatchComboBox()
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

    Private Sub cmbBatch_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbbatch.SelectedIndexChanged
        cmbLot.Items.Clear()
        LoadLotComboBox(cmbbatch.Text)
    End Sub

    Private Sub LoadLotComboBox(batch As String)
        Using conn As New SqlConnection(connectionString)
            conn.Open()

            ' Load cmbLot based on selected batch
            Using cmd As New SqlCommand("SELECT DISTINCT lot FROM raw_distribute WHERE batch_id = @batch", conn)
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
        LoadNetDistributedData(cmbbatch.Text, cmbLot.Text)
    End Sub

    Private Sub btnReturn_Click(sender As Object, e As EventArgs) Handles btnReturn.Click
        Dim batch As String = cmbbatch.Text
        Dim lot As String = cmbLot.Text
        Dim returnedWeight As Decimal = Convert.ToDecimal(txtReturnedWeight.Text)
        Dim returnedRolls As Integer = Convert.ToInt32(txtReturnedRolls.Text)
        Dim ref As String = txtref.Text

        If String.IsNullOrEmpty(batch) OrElse String.IsNullOrEmpty(lot) OrElse String.IsNullOrEmpty(ref) OrElse returnedWeight <= 0 OrElse returnedRolls <= 0 Then
            MessageBox.Show("ادخل قيم متاحه فى الرصيد")
            Return
        End If

        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()

                ' Check net distributed stock
                Dim query As String = "SELECT SUM(issued_weight) AS net_weight, SUM(issued_rolls) AS net_rolls FROM raw_distribute WHERE batch_id = @batch AND lot = @lot"
                Dim cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@batch", batch)
                cmd.Parameters.AddWithValue("@lot", lot)

                Dim reader As SqlDataReader = cmd.ExecuteReader()
                If reader.Read() Then
                    Dim netWeight As Decimal = If(reader("net_weight") IsNot DBNull.Value, Convert.ToDecimal(reader("net_weight")), 0)
                    Dim netRolls As Integer = If(reader("net_rolls") IsNot DBNull.Value, Convert.ToInt32(reader("net_rolls")), 0)

                    If returnedWeight > netWeight OrElse returnedRolls > netRolls Then
                        MessageBox.Show("الكمية اكبر من الرصيد")
                        Return
                    End If

                    reader.Close()

                    ' Insert return record in rawreturn_details
                    query = "INSERT INTO rawreturn_details (batch_id, lot, returned_weight, returned_rolls, return_date,username,store_ref) VALUES (@batch, @lot, @returned_weight, @returned_rolls, @return_date,@username,@store_ref)"
                    cmd = New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@batch", batch)
                    cmd.Parameters.AddWithValue("@lot", lot)
                    cmd.Parameters.AddWithValue("@returned_weight", returnedWeight)
                    cmd.Parameters.AddWithValue("@returned_rolls", returnedRolls)
                    cmd.Parameters.AddWithValue("@return_date", DateTime.Now)
                    cmd.Parameters.AddWithValue("@username", LoggedInUsername)
                    cmd.Parameters.AddWithValue("@store_ref", ref)
                    cmd.ExecuteNonQuery()

                    ' Insert negative record in raw_distribute
                    query = "INSERT INTO raw_distribute (batch_id, lot, issued_weight, issued_rolls, issue_date,username,store_ref) VALUES (@batch, @lot, @issued_weight, @issued_rolls, @issue_date,@username,@store_ref)"
                    cmd = New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@batch", batch)
                    cmd.Parameters.AddWithValue("@lot", lot)
                    cmd.Parameters.AddWithValue("@issued_weight", -returnedWeight)
                    cmd.Parameters.AddWithValue("@issued_rolls", -returnedRolls)
                    cmd.Parameters.AddWithValue("@issue_date", DateTime.Now)
                    cmd.Parameters.AddWithValue("@username", LoggedInUsername)
                    cmd.Parameters.AddWithValue("@store_ref", ref)
                    cmd.ExecuteNonQuery()

                    ' Update stock
                    query = "UPDATE batch_details SET weight_quantity = weight_quantity + @returned_weight, rolls_count = rolls_count + @returned_rolls WHERE batch_id = @batch AND lot = @lot"
                    cmd = New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@returned_weight", returnedWeight)
                    cmd.Parameters.AddWithValue("@returned_rolls", returnedRolls)
                    cmd.Parameters.AddWithValue("@batch", batch)
                    cmd.Parameters.AddWithValue("@lot", lot)
                    cmd.ExecuteNonQuery()

                    MessageBox.Show("Return recorded successfully.")
                    LoadNetDistributedData(batch, lot)
                    dgvStock.DataSource = Nothing
                    cmbbatch.Items.Clear()
                    cmbLot.Items.Clear()
                    txtReturnedWeight.Clear()
                    txtReturnedRolls.Clear()
                    txtref.Clear()
                Else
                    MessageBox.Show("Batch and lot not found.")
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Error recording return: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadNetDistributedData(batch As String, lot As String)
        Using conn As New SqlConnection(connectionString)
            conn.Open()

            Dim query As String = "SELECT batch_id AS 'Batch', lot AS 'Lot', SUM(issued_weight) AS 'الوزن المتاح', SUM(issued_rolls) AS 'الأتواب المتاحة' FROM raw_distribute WHERE batch_id = @batch AND lot = @lot GROUP BY batch_id, lot"
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




