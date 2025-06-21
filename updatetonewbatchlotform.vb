
Imports System.Data.SqlClient
Imports System.Net
Imports System.Net.Mail
Imports System.Drawing

Public Class updatetonewbatchlotform
    Private connectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"

    Private Sub updatetonewbatchlotform_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Check user access level
        If Not CheckUserAccessLevel() Then
            MessageBox.Show("You do not have the required access level to use this form.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
            Return
        End If

        ' Populate ComboBoxes
        LoadBatchIds()
        LoadChangeBatchTo()
    End Sub

    Private Function CheckUserAccessLevel() As Boolean
        Try
            Using conn As New SqlConnection(connectionString)
                Dim query As String = "SELECT acc_level FROM dep_users WHERE username = @username"
                Dim cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@username", LoggedInUsername)

                conn.Open()
                Dim result = cmd.ExecuteScalar()
                conn.Close()

                If result IsNot Nothing AndAlso Convert.ToInt32(result) = 1 Then
                    Return True
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Error checking user access level: " & ex.Message)
        End Try

        Return False
    End Function

    Private Sub LoadBatchIds()
        Try
            Using conn As New SqlConnection(connectionString)
                Dim query As String = "SELECT DISTINCT batch FROM batch_raw"
                Dim cmd As New SqlCommand(query, conn)
                Dim dt As New DataTable()

                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                dt.Load(reader)

                cmbBatchId.DataSource = dt
                cmbBatchId.DisplayMember = "batch"
                cmbBatchId.ValueMember = "batch"
                cmbBatchId.SelectedIndex = -1

                conn.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading batch IDs: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadChangeBatchTo()
        Try
            Using conn As New SqlConnection(connectionString)
                Dim query As String = "SELECT DISTINCT batch FROM batch_raw"
                Dim cmd As New SqlCommand(query, conn)
                Dim dt As New DataTable()

                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                dt.Load(reader)

                cmbChangeBatchTo.DataSource = dt
                cmbChangeBatchTo.DisplayMember = "batch"
                cmbChangeBatchTo.ValueMember = "batch"
                cmbChangeBatchTo.SelectedIndex = -1

                conn.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading batch IDs: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadLots(batchId As Integer)
        Try
            Using conn As New SqlConnection(connectionString)
                Dim query As String = "SELECT DISTINCT lot FROM batch_details WHERE batch_id = @batchId"
                Dim cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@batchId", batchId)
                Dim dt As New DataTable()

                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                dt.Load(reader)

                cmbLot.DataSource = dt
                cmbLot.DisplayMember = "lot"
                cmbLot.ValueMember = "lot"
                cmbLot.SelectedIndex = -1

                conn.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading lots: " & ex.Message)
        End Try
    End Sub

    Private Sub cmbBatchId_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbBatchId.SelectedIndexChanged
        If cmbBatchId.SelectedIndex <> -1 Then
            Dim selectedBatchId As Integer = Convert.ToInt32(CType(cmbBatchId.SelectedItem, DataRowView)("batch"))
            LoadLots(selectedBatchId)
        End If
    End Sub

    Private Sub btnUpdateBatch_Click(sender As Object, e As EventArgs) Handles btnUpdateBatch.Click
        If cmbBatchId.SelectedIndex = -1 OrElse cmbLot.SelectedIndex = -1 OrElse (chkUpdateOnly.Checked AndAlso (cmbChangeBatchTo.SelectedIndex = -1 OrElse String.IsNullOrWhiteSpace(txtNewLot.Text))) Then
            MessageBox.Show("Please select a batch ID, lot, and if updating only, select the new batch ID and enter the new lot.")
            Return
        End If

        Dim oldBatchId As Integer = Convert.ToInt32(cmbBatchId.SelectedValue)
        Dim oldLot As String = cmbLot.SelectedValue.ToString()
        Dim newBatchId As Integer = If(chkUpdateOnly.Checked, Convert.ToInt32(cmbChangeBatchTo.SelectedValue), oldBatchId)
        Dim newLot As String = If(chkUpdateOnly.Checked, txtNewLot.Text, oldLot)

        If chkUpdateOnly.Checked Then
            UpdateBatchAndLot(oldBatchId, oldLot, newBatchId, newLot)
        Else
            InsertAndUpdateBatchAndLot(oldBatchId, oldLot, newBatchId, newLot)
        End If
    End Sub

    Private Sub UpdateBatchAndLot(oldBatchId As Integer, oldLot As String, newBatchId As Integer, newLot As String)
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()

                ' Update batch details
                Dim tables As String() = {"batch_details", "batch_lot_status", "batch_lot_defect", "row_inspect_sample", "row_inspect_sample_defects", "qc_raw_test"}
                For Each table As String In tables
                    Dim updateQuery As String = $"UPDATE {table} SET batch_id = @newBatchId, lot = @newLot WHERE batch_id = @oldBatchId AND lot = @oldLot"
                    Dim updateCmd As New SqlCommand(updateQuery, conn)
                    updateCmd.Parameters.AddWithValue("@newBatchId", newBatchId)
                    updateCmd.Parameters.AddWithValue("@newLot", newLot)
                    updateCmd.Parameters.AddWithValue("@oldBatchId", oldBatchId)
                    updateCmd.Parameters.AddWithValue("@oldLot", oldLot)
                    updateCmd.ExecuteNonQuery()
                Next

                conn.Close()
                MessageBox.Show("Batch and lot updated successfully.")

                ' Clear ComboBoxes
                ClearComboBoxes()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error updating batch and lot: " & ex.Message)
        End Try
    End Sub

    Private Sub InsertAndUpdateBatchAndLot(oldBatchId As Integer, oldLot As String, newBatchId As Integer, newLot As String)
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()

                ' Insert new batch details
                Dim insertQuery As String = "INSERT INTO batch_details (batch_id, lot, client_permission, client_item_code, weight_quantity, meter_quantity, rolls_count, add_permission, store_permission, rollpk, weightpk, username) " &
                                            "SELECT @newBatchId, @newLot, client_permission, client_item_code, weight_quantity, meter_quantity, rolls_count, add_permission, store_permission, rollpk, weightpk, username " &
                                            "FROM batch_details WHERE batch_id = @oldBatchId AND lot = @oldLot"
                Dim insertCmd As New SqlCommand(insertQuery, conn)
                insertCmd.Parameters.AddWithValue("@newBatchId", newBatchId)
                insertCmd.Parameters.AddWithValue("@newLot", newLot)
                insertCmd.Parameters.AddWithValue("@oldBatchId", oldBatchId)
                insertCmd.Parameters.AddWithValue("@oldLot", oldLot)
                insertCmd.ExecuteNonQuery()

                ' Update old batch details
                UpdateBatchAndLot(oldBatchId, oldLot, newBatchId, newLot)

                conn.Close()
                MessageBox.Show("Batch and lot inserted and updated successfully.")

                ' Clear ComboBoxes
                ClearComboBoxes()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error inserting and updating batch and lot: " & ex.Message)
        End Try
    End Sub

    Private Sub ClearComboBoxes()
        cmbBatchId.SelectedIndex = -1
        cmbLot.SelectedIndex = -1
        cmbChangeBatchTo.SelectedIndex = -1
        txtNewLot.Text = String.Empty
    End Sub

    Private Sub btnDuplicateBatch_Click(sender As Object, e As EventArgs) Handles btnDuplicateBatch.Click
        If cmbBatchId.SelectedIndex = -1 OrElse cmbLot.SelectedIndex = -1 Then
            MessageBox.Show("Please select a batch ID and a lot.")
            Return
        End If

        Dim oldBatchId As Integer = Convert.ToInt32(cmbBatchId.SelectedValue)
        Dim oldLot As String = cmbLot.SelectedValue.ToString()

        ' Check if the lot contains the character "/"
        If Not oldLot.Contains("/") AndAlso Not oldLot.Contains("\") Then
            MessageBox.Show("The selected lot does not contain the character '/' or '\'.")
            Return
        End If


        DuplicateBatchAndUpdateRelatedTables(oldBatchId, oldLot)
    End Sub

    Private Sub DuplicateBatchAndUpdateRelatedTables(oldBatchId As Integer, oldLot As String)
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()

                ' Step 1: Get the next batch number
                Dim newBatchNumber As Integer = GetNextBatchNumber(conn)

                ' Step 2: Insert new batch in batch_raw
                Dim insertBatchQuery As String = "INSERT INTO batch_raw (datetrans, batch, sup_code, client_code, fabric_type, style_id, service_id, po_number, material, username) " &
                                             "SELECT datetrans, @newBatch, sup_code, client_code, fabric_type, style_id, service_id, po_number, material, username " &
                                             "FROM batch_raw WHERE batch = @oldBatch"
                Dim insertBatchCmd As New SqlCommand(insertBatchQuery, conn)
                insertBatchCmd.Parameters.AddWithValue("@newBatch", newBatchNumber)
                insertBatchCmd.Parameters.AddWithValue("@oldBatch", oldBatchId)
                insertBatchCmd.ExecuteNonQuery()

                ' Step 3: Update related tables
                UpdateRelatedTables(conn, oldBatchId, oldLot, newBatchNumber)

                conn.Close()
                MessageBox.Show("Batch duplicated and related tables updated successfully. New batch number: " & newBatchNumber)

                ' Send email notification
                SendEmailNotification(oldBatchId, oldLot, newBatchNumber)

                ' Clear ComboBoxes
                ClearComboBoxes()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error duplicating batch and updating related tables: " & ex.Message)
        End Try
    End Sub

    Private Function GetNextBatchNumber(conn As SqlConnection) As Integer
        Dim nextBatchNumber As Integer = 1
        Try
            Dim query As String = "SELECT TOP 1 batch FROM batch_raw ORDER BY batch DESC"
            Dim cmd As New SqlCommand(query, conn)
            Dim result = cmd.ExecuteScalar()
            If result IsNot Nothing Then
                nextBatchNumber = Convert.ToInt32(result) + 1
            End If
        Catch ex As Exception
            MessageBox.Show("Error fetching next batch number: " & ex.Message)
        End Try
        Return nextBatchNumber
    End Function

    Private Sub UpdateRelatedTables(conn As SqlConnection, oldBatchId As Integer, oldLot As String, newBatchId As Integer)
        Dim tables As String() = {"batch_details", "batch_lot_status", "batch_lot_defect", "row_inspect_sample", "row_inspect_sample_defects", "qc_raw_test"}
        For Each table As String In tables
            Dim updateQuery As String = $"UPDATE {table} SET batch_id = @newBatchId, lot = @newBatchId WHERE batch_id = @oldBatchId AND lot = @oldLot"
            Dim updateCmd As New SqlCommand(updateQuery, conn)
            updateCmd.Parameters.AddWithValue("@newBatchId", newBatchId)
            updateCmd.Parameters.AddWithValue("@oldBatchId", oldBatchId)
            updateCmd.Parameters.AddWithValue("@oldLot", oldLot)
            updateCmd.ExecuteNonQuery()
        Next
    End Sub

    Private Sub SendEmailNotification(oldBatchId As Integer, oldLot As String, newBatchNumber As Integer)
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()

                ' Get email addresses from the mails table
                Dim query As String = "SELECT mail FROM mails"
                Dim cmd As New SqlCommand(query, conn)
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                Dim emailAddresses As New List(Of String)

                While reader.Read()
                    emailAddresses.Add(reader("mail").ToString())
                End While

                conn.Close()

                ' Send email
                Dim smtpClient As New SmtpClient("smtppro.zoho.com", 587) With {
                .Credentials = New NetworkCredential("it.app@moamen.com", "WMG@#$it$#@2024"),
                .EnableSsl = True
            }

                Dim mailMessage As New MailMessage() With {
                .From = New MailAddress("it.app@moamen.com"),
                .Subject = "Batch Update Notification",
                .Body = $"تم تحويل رقم الرساله {oldBatchId} باللوت {oldLot} الى رقم رساله {newBatchNumber} ورقم لوت {newBatchNumber}"
            }

                For Each mail As String In emailAddresses
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



End Class