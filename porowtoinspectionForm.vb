Imports MySql.Data.MySqlClient
Imports System.Data.SqlClient
Imports Excel = Microsoft.Office.Interop.Excel ' For Excel interop
Imports OfficeOpenXml ' Add this line for EPPlus
Imports System.IO
Public Class porowtoinspectionForm
    Private sqlServerConnectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"
    Private Sub porowtoinspectionForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadWorderIDs()
        lblusername.Text = "Logged in as: " & LoggedInUsername
        ' Retrieve the public_name for the logged-in user
        Dim publicName As String = GetPublicName(LoggedInUsername)
    End Sub

    Private Sub LoadWorderIDs()
        Dim query As String = "SELECT DISTINCT worderid FROM techdata"

        Using conn As New SqlConnection(sqlServerConnectionString)
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

    Private Sub cmbworder_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbworder.SelectedIndexChanged
        ' Clear previous data before loading new data
        lblbatchno.Text = "الرسالة: N/A"
        lblcontractno.Text = "رقم التعاقد: N/A"
        lblqtym.Text = "الكمية متر: N/A"
        lblqtykg.Text = "الكمية وزن: N/A"
        lblstatus.Text = "" ' Clear status

        ' SQL Query to retrieve details for the selected Work Order
        Dim query As String = "SELECT td.worderid, c.contractno, c.batch, td.qty_m, td.qty_kg " & _
                              "FROM techdata td " & _
                              "LEFT JOIN Contracts c ON td.contract_id = c.ContractID " & _
                              "WHERE td.worderid = @worderid"

        ' SQL Query to check if the Work Order exists in porow_toinspect
        Dim checkQuery As String = "SELECT qtykgstore FROM porow_toinspect WHERE worderid = @worderid" ' Query to check qty_m for the selected Work Order

        Using conn As New SqlConnection(sqlServerConnectionString)
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

            Using checkCmd As New SqlCommand(checkQuery, conn)
                checkCmd.Parameters.AddWithValue("@worderid", cmbworder.Text)
                Try
                    Dim qtykgstore As Object = checkCmd.ExecuteScalar()
                    If qtykgstore IsNot Nothing AndAlso qtykgstore IsNot DBNull.Value Then
                        Dim qtykgstoreValue As Double = Convert.ToDouble(qtykgstore) ' Convert to appropriate type
                        lblstatus.Text = "الكمية وزن: " & qtykgstoreValue.ToString()
                        lblstatus.ForeColor = Color.Green
                    Else
                        lblstatus.Text = "الكمية وزن: لم يتم العثور على البيانات"
                        lblstatus.ForeColor = Color.Red
                    End If
                Catch ex As Exception
                    MessageBox.Show("Error checking qty_m for the selected Work Order: " & ex.Message)
                End Try
            End Using
        End Using

    End Sub


    Private Function GetPublicName(ByVal username As String) As String
        Dim publicName As String = String.Empty
        Try
            Using connection As New SqlConnection(sqlServerConnectionString)
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
   Private Sub btnInsert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btninsert.Click
        ' Validate that a Work Order is selected and txtkg is not empty
        If String.IsNullOrEmpty(cmbworder.Text) Then
            MessageBox.Show("Please select a Work Order ID.")
            Return
        End If

        If String.IsNullOrEmpty(txtkg.Text) OrElse Not IsNumeric(txtkg.Text) Then
            MessageBox.Show("Please enter a valid quantity in KG.")
            Return
        End If
        If String.IsNullOrEmpty(txtrolls.Text) OrElse Not IsNumeric(txtkg.Text) Then
            MessageBox.Show("Please enter a valid quantity in KG.")
            Return
        End If

        Dim isAlreadyInserted As Boolean = False
        Dim checkQuery As String = "SELECT COUNT(*) FROM porow_toinspect WHERE worderid = @worderid"

        Using conn As New SqlConnection(sqlServerConnectionString)
            Using cmd As New SqlCommand(checkQuery, conn)
                cmd.Parameters.AddWithValue("@worderid", cmbworder.Text)

                Try
                    conn.Open()
                    Dim result As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                    isAlreadyInserted = (result > 0)
                Catch ex As Exception
                    MessageBox.Show("Error checking data existence: " & ex.Message)
                    Return
                End Try
            End Using
        End Using

        Dim query As String

        If isAlreadyInserted Then
            ' Update query if the record exists
            query = "UPDATE porow_toinspect SET qtykgstore = @qtykgstore, rolls = @rolls, date_trans = @date_trans, usernamestore = @usernamestore WHERE worderid = @worderid"
        Else
            ' Insert query if the record does not exist
            query = "INSERT INTO porow_toinspect (worderid, qtykgstore, rolls,date_trans, usernamestore) VALUES (@worderid, @qtykgstore, @rolls,@date_trans, @usernamestore)"
        End If

        Using conn As New SqlConnection(sqlServerConnectionString)
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@worderid", cmbworder.Text)
                cmd.Parameters.AddWithValue("@qtykgstore", Convert.ToDecimal(txtkg.Text))
                cmd.Parameters.AddWithValue("@rolls", (txtrolls.Text))
                cmd.Parameters.AddWithValue("@date_trans", DateTime.Now) ' Use the current date and time
                cmd.Parameters.AddWithValue("@usernamestore", LoggedInUsername)

                Try
                    conn.Open()
                    Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

                    If rowsAffected > 0 Then
                        lblstatus.Text = If(isAlreadyInserted, "تم تحديث البيانات بنجاح", "تم التسجيل بنجاح")
                        lblstatus.ForeColor = If(isAlreadyInserted, Color.Blue, Color.Red)
                        ClearFieldsAndLabels()
                    Else
                        lblstatus.Text = "Failed to update or insert data"
                        lblstatus.ForeColor = Color.Red
                    End If
                Catch ex As Exception
                    MessageBox.Show("Error executing query: " & ex.Message)
                End Try
            End Using
        End Using
    End Sub



    ' Method to clear fields and labels
    Private Sub ClearFieldsAndLabels()
        cmbworder.SelectedIndex = -1 ' Clear ComboBox selection
        txtkg.Clear() ' Clear TextBox
        txtrolls.Clear()
        lblbatchno.Text = "الرسالة: N/A"
        lblcontractno.Text = "رقم التعاقد: N/A"
        lblqtym.Text = "الكمية متر: N/A"
        lblqtykg.Text = "الكمية وزن: N/A"
    End Sub


End Class