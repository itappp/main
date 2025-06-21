Imports MySql.Data.MySqlClient
Imports System.Data.SqlClient

Public Class UpdateLibraryCodeForm
    Private sqlServerConnectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"
    Private mysqlServerConnectionString As String = "Server=150.1.1.7;Database=wm;Uid=root1;Pwd=WMg2024$;"

    Private Sub UpdateLibraryCodeForm_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        LoadWorkOrderIDs()
        LoadLibraryCodes()
        ' Access the logged-in username from the global variable
        lblusername.Text = "Logged in as: " & LoggedInUsername
        ' Set the user access level based on logged-in username
        SetUserAccessLevel(LoggedInUsername)

    End Sub
    Private Sub SetUserAccessLevel(ByVal username As String)
        Try
            Using conn As New SqlConnection(sqlServerConnectionString)
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
    Private Sub LoadWorkOrderIDs()
        Dim query As String = "SELECT DISTINCT worderid FROM techdata"

        Using conn As New SqlConnection(sqlServerConnectionString)
            Dim cmd As New SqlCommand(query, conn)
            Try
                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()

                cmbWorkOrder.Items.Clear()
                While reader.Read()
                    cmbWorkOrder.Items.Add(reader("worderid").ToString())
                End While
            Catch ex As Exception
                MessageBox.Show("Error loading Work Order IDs: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Sub LoadLibraryCodes()
        Dim query As String = "SELECT DISTINCT code FROM lib"

        Using conn As New SqlConnection(sqlServerConnectionString)
            Dim cmd As New SqlCommand(query, conn)
            Try
                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()

                cmbNewLibraryCode.Items.Clear()
                While reader.Read()
                    cmbNewLibraryCode.Items.Add(reader("code").ToString())
                End While
            Catch ex As Exception
                MessageBox.Show("Error loading library codes: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Sub cmbWorkOrder_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbWorkOrder.SelectedIndexChanged
        LoadOldLibraryCode(cmbWorkOrder.Text)
    End Sub

    Private Sub LoadOldLibraryCode(ByVal workOrderID As String)
        Dim query As String = "SELECT l.code FROM techdata t JOIN lib l ON t.new_code_lib = l.code_id WHERE t.worderid = @worderid"

        Using conn As New SqlConnection(sqlServerConnectionString)
            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@worderid", workOrderID)
            Try
                conn.Open()
                Dim result = cmd.ExecuteScalar()
                If result IsNot Nothing Then
                    lblOldLibraryCode.Text = result.ToString()
                    LoadLibraryStages(result.ToString(), dgvOldLibraryStages)
                Else
                    lblOldLibraryCode.Text = "N/A"
                    dgvOldLibraryStages.Rows.Clear()
                End If
            Catch ex As Exception
                MessageBox.Show("Error loading old library code: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Sub LoadLibraryStages(ByVal code As String, ByVal dgv As DataGridView)
        Dim query As String = "SELECT np.proccess_ar FROM new_proccess np LEFT JOIN lib l ON np.id = l.proccess_id WHERE l.code = @code"

        Using conn As New SqlConnection(sqlServerConnectionString)
            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@code", code)
            Try
                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()

                dgv.Rows.Clear()
                dgv.Columns.Clear()
                dgv.Columns.Add("ProcessStage", "مرحلة العملية")

                While reader.Read()
                    dgv.Rows.Add(reader("proccess_ar").ToString())
                End While

                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells

                ' Debugging message to check if stages are loaded
                If dgv.Rows.Count = 0 Then
                    MessageBox.Show("No stages found for the library code.", "Debug Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                End If
            Catch ex As Exception
                MessageBox.Show("Error loading library stages: " & ex.Message)
            End Try
        End Using
    End Sub


    Private Sub cmbNewLibraryCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbNewLibraryCode.SelectedIndexChanged
        LoadLibraryStages(cmbNewLibraryCode.Text, dgvNewLibraryStages)
    End Sub

    Private Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdate.Click
        If String.IsNullOrEmpty(cmbWorkOrder.Text) OrElse String.IsNullOrEmpty(cmbNewLibraryCode.Text) Then
            MessageBox.Show("Please select a Work Order ID and a new Library Code.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' جلب code_id من جدول lib بناءً على الكود الجديد المحدد
        Dim newCodeLibId As Integer
        Dim getCodeIdQuery As String = "SELECT code_id FROM lib WHERE code = @code"
        Using conn As New SqlConnection(sqlServerConnectionString)
            Dim cmd As New SqlCommand(getCodeIdQuery, conn)
            cmd.Parameters.AddWithValue("@code", cmbNewLibraryCode.Text)
            Try
                conn.Open()
                Dim result = cmd.ExecuteScalar()
                If result IsNot Nothing Then
                    newCodeLibId = Convert.ToInt32(result)
                Else
                    MessageBox.Show("Error: New Library Code ID not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If
            Catch ex As Exception
                MessageBox.Show("Error retrieving new library code ID: " & ex.Message)
                Return
            End Try
        End Using

        ' Retrieve the username from lblUsername
        Dim userName As String = lblusername.Text.Replace("Logged in as: ", "").Trim()
        If String.IsNullOrWhiteSpace(userName) Then
            MessageBox.Show("Username is not available.")
            Return
        End If

        ' Query to retrieve public_name and department from dep_users
        Dim publicName As String = ""
        Dim department As String = ""
        Dim selectUserQuery As String = "SELECT public_name, department FROM dep_users WHERE username = @username"

        Using conn As New SqlConnection(sqlServerConnectionString)
            conn.Open()
            Using selectCmd As New SqlCommand(selectUserQuery, conn)
                selectCmd.Parameters.AddWithValue("@username", userName)
                Using reader As SqlDataReader = selectCmd.ExecuteReader()
                    If reader.Read() Then
                        publicName = reader("public_name").ToString()
                        department = reader("department").ToString()
                    Else
                        MessageBox.Show("User information not found in dep_users.")
                        Return
                    End If
                End Using
            End Using

            ' تحديث new_code_lib في جدول techdata باستخدام code_id
            Dim updateQuery As String = "UPDATE techdata SET new_code_lib = @newCodeLibId WHERE worderid = @worderid"
            Using cmd As New SqlCommand(updateQuery, conn)
                cmd.Parameters.AddWithValue("@newCodeLibId", newCodeLibId)
                cmd.Parameters.AddWithValue("@worderid", cmbWorkOrder.Text)
                Try
                    Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
                    If rowsAffected > 0 Then
                        ' تسجيل البيانات في جدول activity_logs
                        Dim insertLogQuery As String = "INSERT INTO activity_logs (oldcodelib, newcodelib, worderid, username, department, reason, timestamp, kindtrans) " &
                                                       "VALUES (@oldCodeLib, @newCodeLib, @worderid, @username, @department, @reason, @timestamp, @kindtrans)"
                        Using logCmd As New SqlCommand(insertLogQuery, conn)
                            logCmd.Parameters.AddWithValue("@oldCodeLib", lblOldLibraryCode.Text)
                            logCmd.Parameters.AddWithValue("@newCodeLib", cmbNewLibraryCode.Text)
                            logCmd.Parameters.AddWithValue("@worderid", cmbWorkOrder.Text)
                            logCmd.Parameters.AddWithValue("@username", LoggedInUsername)
                            logCmd.Parameters.AddWithValue("@department", department)
                            logCmd.Parameters.AddWithValue("@reason", txtnotes.Text) ' استخدام القيمة من txtNotes كسبب للتحديث
                            logCmd.Parameters.AddWithValue("@timestamp", DateTime.Now)
                            logCmd.Parameters.AddWithValue("@kindtrans", "تعديل كود المكتبه")
                            logCmd.ExecuteNonQuery()
                        End Using

                        ' مسح البيانات من جميع العناصر
                        ClearFormData()

                        MessageBox.Show("Library code updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        MessageBox.Show("No record found for the selected Work Order ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                Catch ex As Exception
                    MessageBox.Show("Error updating library code: " & ex.Message)
                End Try
            End Using
        End Using
    End Sub

    Private Sub ClearFormData()
        ' مسح البيانات من جميع العناصر
        lblOldLibraryCode.Text = String.Empty
        cmbWorkOrder.SelectedIndex = -1
        cmbNewLibraryCode.SelectedIndex = -1
        dgvOldLibraryStages.Rows.Clear()
        dgvNewLibraryStages.Rows.Clear()
        txtnotes.Text = String.Empty
    End Sub




End Class
