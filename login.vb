Imports System.Data.SqlClient

Public Class LoginForm
    Private Sub btnLogin_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLogin.Click
        Dim username As String = txtUsername.Text
        Dim password As String = txtPassword.Text

        ' Connection string to your SQL Server database
        Dim connectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"

        ' SQL query to check for the user and retrieve acc_level and department
        Dim query As String = "SELECT acc_level, department FROM dep_users WHERE username=@username AND password=@password"

        ' Open a connection to the database
        Using conn As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(query, conn)
                ' Add parameters to avoid SQL injection
                cmd.Parameters.AddWithValue("@username", username)
                cmd.Parameters.AddWithValue("@password", password) ' Ensure password is hashed in production

                ' Open connection and execute query
                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()

                If reader.HasRows Then
                    reader.Read()
                    Dim accLevel As Integer = reader("acc_level")
                    Dim department As String = reader("department").ToString().Trim().ToLower()

                    ' Set global variables
                    LoggedInUsername = username
                    UserAccessLevel = accLevel
                    UserDepartment = department

                    ' Redirect to MainITForm for all departments
                    Me.Hide()
                    Dim mainitform As New MainITForm()
                    mainitform.Show()
                Else
                    MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End Using
        End Using
    End Sub
End Class
