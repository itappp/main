Imports System.Data.SqlClient
Imports Excel = Microsoft.Office.Interop.Excel ' For Excel interop

Public Class addnewcodeform
    ' SQL Server connection string
    Dim sqlServerConnectionString As String = "Server=localhost;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"

    ' Variables to store previous state
    Private previousTxtCodeAll As String
    Private previousTxtProcessIds As String

    ' Form Load Event
    Private Sub AddNewCodeForm_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        ' Initially disable the insert button
        btninsert.Enabled = False
        ' Load machines into ComboBox
        LoadMachines()
        lblUsername.Text = "Logged in as: " & LoggedInUsername
        ' Retrieve the public_name for the logged-in user
        Dim publicName As String = GetPublicName(LoggedInUsername)
    End Sub

    ' Load machines into ComboBox
    Private Sub LoadMachines()
        Dim query As String = "SELECT id, name_ar FROM new_machines"
        Using connection As New SqlConnection(sqlServerConnectionString)
            Using cmd As New SqlCommand(query, connection)
                connection.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim item As New ComboBoxItem(reader("name_ar").ToString(), reader("id").ToString())
                        cmbMachines.Items.Add(item)
                    End While
                End Using
            End Using
        End Using
    End Sub

    Private Sub cmbMachines_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbMachines.SelectedIndexChanged
        ' Clear the processes ComboBox
        cmbProcesses.Items.Clear()

        ' Load processes for the selected machine
        Dim selectedItem As ComboBoxItem = CType(cmbMachines.SelectedItem, ComboBoxItem)
        LoadProcesses(selectedItem.Value)
    End Sub

    ' Load processes into ComboBox
    Private Sub LoadProcesses(machineId As String)
        cmbProcesses.Items.Clear()
        Dim query As String = "SELECT id, proccess_ar FROM new_proccess WHERE machine_id = @machine_id"
        Using connection As New SqlConnection(sqlServerConnectionString)
            Using cmd As New SqlCommand(query, connection)
                cmd.Parameters.AddWithValue("@machine_id", machineId)
                connection.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim item As New ComboBoxItem(reader("proccess_ar").ToString(), reader("id").ToString())
                        cmbProcesses.Items.Add(item)
                    End While
                End Using
            End Using
        End Using
    End Sub

    ' Add Process Button Click Event
    Private Sub btnAddProcess_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddProcess.Click
        If cmbProcesses.SelectedItem IsNot Nothing Then
            ' Store previous state
            previousTxtCodeAll = txtcodeall.Text
            previousTxtProcessIds = txtProcessIds.Text

            Dim selectedProcess As ComboBoxItem = CType(cmbProcesses.SelectedItem, ComboBoxItem)

            ' Add hyphen if txtcodeall is not empty
            If Not String.IsNullOrEmpty(txtcodeall.Text) Then
                txtcodeall.Text &= " - "
            End If

            txtcodeall.Text &= selectedProcess.Text
            txtProcessIds.Text &= selectedProcess.Value & " "
        End If
    End Sub


    ' Undo Button Click Event
    Private Sub btnUndo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUndo.Click
        ' Restore previous state
        txtcodeall.Text = previousTxtCodeAll
        txtProcessIds.Text = previousTxtProcessIds
    End Sub

    ' TextChanged Event for TextBoxes
    Private Sub txtPublicName_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtpublicname.TextChanged, txtcode.TextChanged, txtcodeall.TextChanged
        ' Enable btnInsert only if all text fields are filled
        btninsert.Enabled = Not String.IsNullOrEmpty(txtpublicname.Text) AndAlso
                           Not String.IsNullOrEmpty(txtcode.Text) AndAlso
                           Not String.IsNullOrEmpty(txtcodeall.Text)
    End Sub

    ' Insert Button Click Event
    ' Insert Button Click Event
    ' Insert Button Click Event
    Private Sub btnInsert_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btninsert.Click
        ' Validate that fields are not empty
        If String.IsNullOrEmpty(txtpublicname.Text) OrElse String.IsNullOrEmpty(txtcode.Text) OrElse String.IsNullOrEmpty(txtcodeall.Text) Then
            MessageBox.Show("Please write all fields.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Check if the code already exists in the database
        Using connection As New SqlConnection(sqlServerConnectionString)
            connection.Open()
            Dim checkQuery As String = "SELECT COUNT(*) FROM lib WHERE code = @code"
            Using cmd As New SqlCommand(checkQuery, connection)
                cmd.Parameters.AddWithValue("@code", txtcode.Text.Trim())
                Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                If count > 0 Then
                    MessageBox.Show("The code already exists.", "Duplicate Code", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If
            End Using
        End Using

        ' Split the process IDs by spaces
        Dim processIds As String() = txtProcessIds.Text.Trim().Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)

        ' Get the current date and time
        Dim currentDateTime As DateTime = DateTime.Now

        ' Open SQL Server connection
        Using connection As New SqlConnection(sqlServerConnectionString)
            Try
                connection.Open()

                ' Get the next available code_id
                Dim nextCodeId As Integer
                Dim getMaxCodeIdQuery As String = "SELECT ISNULL(MAX(code_id), 0) + 1 FROM lib"
                Using cmd As New SqlCommand(getMaxCodeIdQuery, connection)
                    nextCodeId = Convert.ToInt32(cmd.ExecuteScalar())
                End Using

                ' Loop through each process ID and insert it into the database
                For i As Integer = 0 To processIds.Length - 1
                    ' SQL insert query for library table
                    Dim insertQuery As String = "INSERT INTO lib (code, proccess_id, notes, datetrans, steps_num, code_id) VALUES (@code, @proccessid, @notes, @datetrans, @stepsnum, @code_id)"
                    Using cmd As New SqlCommand(insertQuery, connection)
                        cmd.Parameters.AddWithValue("@code", txtcode.Text.Trim())
                        cmd.Parameters.AddWithValue("@proccessid", processIds(i).Trim())
                        cmd.Parameters.AddWithValue("@notes", txtpublicname.Text.Trim())
                        cmd.Parameters.AddWithValue("@datetrans", currentDateTime)
                        cmd.Parameters.AddWithValue("@stepsnum", i + 1)
                        cmd.Parameters.AddWithValue("@code_id", nextCodeId) ' Assign the next available code_id

                        ' Execute the insert command
                        cmd.ExecuteNonQuery()
                    End Using
                Next

                ' Insert into library table
                Dim insertLibraryQuery As String = "INSERT INTO library (code, lib_code) VALUES (@code, @lib_code)"
                Using cmd As New SqlCommand(insertLibraryQuery, connection)
                    cmd.Parameters.AddWithValue("@code", txtcode.Text.Trim())
                    cmd.Parameters.AddWithValue("@lib_code", txtcodeall.Text.Trim())
                    cmd.ExecuteNonQuery()
                End Using

                ' Optional: Clear fields after successful insertion
                txtpublicname.Clear()
                txtcode.Clear()
                txtcodeall.Clear()
                txtProcessIds.Text = ""
                cmbProcesses.Items.Clear()

                MessageBox.Show("Record inserted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    ' Helper method to release COM objects
    Private Sub ReleaseObject(ByVal obj As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            obj = Nothing
        Catch ex As Exception
            obj = Nothing
        Finally
            GC.Collect()
        End Try
    End Sub

    ' ComboBoxItem class to hold text and value
    Private Class ComboBoxItem
        Public Property Text As String
        Public Property Value As String

        Public Sub New(text As String, value As String)
            Me.Text = text
            Me.Value = value
        End Sub

        Public Overrides Function ToString() As String
            Return Text
        End Function
    End Class
    Private Function GetPublicName(ByVal username As String) As String
        Dim publicName As String = String.Empty
        Try
            Using conn As New SqlConnection(sqlServerConnectionString)
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

