Imports System.Data.SqlClient

Public Class accaddclientform
    Private connectionString As String = "Server=localhost;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"

    Private Sub accaddclientform_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' إعدادات الفورم عند التحميل
        ComboBox1.Items.Add("Client")
        ComboBox1.Items.Add("Supplier")
        UpdateLabels()
    End Sub

    Private Sub ButtonAdd_Click(sender As Object, e As EventArgs) Handles ButtonAdd.Click
        Dim code As String = TextBoxCode.Text
        Dim name As String = TextBoxName.Text
        Dim group As String = TextBoxGroup.Text
        Dim kind As String = ComboBox1.SelectedItem.ToString()

        If kind = "Supplier" Then
            AddSupplier(code, name, group)
        ElseIf kind = "Client" Then
            AddClient(code, name, group)
        End If

        UpdateLabels()
    End Sub

    Private Sub AddSupplier(code As String, name As String, group As String)
        ' تحقق من عدم تكرار الكود
        If Not IsCodeExists("suppliers", code) Then
            ' إضافة المورد إلى الجدول
            Dim query As String = "INSERT INTO suppliers (code, name, kind_sup) VALUES (@code, @name, @group)"
            Using conn As New SqlConnection(connectionString)
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@code", code)
                    cmd.Parameters.AddWithValue("@name", name)
                    cmd.Parameters.AddWithValue("@group", group)
                    conn.Open()
                    cmd.ExecuteNonQuery()
                    MessageBox.Show("تم تسجيل المورد بنجاح")
                End Using
            End Using
        Else
            MessageBox.Show("Code already exists!")
        End If
    End Sub

    Private Sub AddClient(code As String, name As String, group As String)
        ' تحقق من عدم تكرار الكود
        If Not IsCodeExists("clients", code) Then
            ' إضافة العميل إلى الجدول
            Dim query As String = "INSERT INTO clients (code, name, custgroup) VALUES (@code, @name, @group)"
            Using conn As New SqlConnection(connectionString)
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@code", code)
                    cmd.Parameters.AddWithValue("@name", name)
                    cmd.Parameters.AddWithValue("@group", group)
                    conn.Open()
                    cmd.ExecuteNonQuery()
                    MessageBox.Show("تم تسجيل العميل بنجاح")
                End Using
            End Using
        Else
            MessageBox.Show("الكود مسجل بالفعل")
        End If
    End Sub

    Private Function IsCodeExists(tableName As String, code As String) As Boolean
        Dim query As String = $"SELECT COUNT(*) FROM {tableName} WHERE code = @code"
        Using conn As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@code", code)
                conn.Open()
                Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                Return count > 0
            End Using
        End Using
    End Function

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim kind As String = ComboBox1.SelectedItem.ToString()
        If kind = "Supplier" Then
            TextBoxCode.Text = GetNextCode("suppliers", "S")
        ElseIf kind = "Client" Then
            TextBoxCode.Text = GetNextCode("clients", "F")
        End If
    End Sub

    Private Function GetNextCode(tableName As String, prefix As String) As String
        Dim query As String = $"SELECT MAX(code) FROM {tableName} WHERE code LIKE '{prefix}%'"
        Using conn As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(query, conn)
                conn.Open()
                Dim maxCode As String = Convert.ToString(cmd.ExecuteScalar())
                If String.IsNullOrEmpty(maxCode) Then
                    Return $"{prefix}00001"
                Else
                    Dim number As Integer = Convert.ToInt32(maxCode.Substring(1)) + 1
                    Return $"{prefix}{number:D5}"
                End If
            End Using
        End Using
    End Function

    Private Sub UpdateLabels()
        LabelClientFCode.Text = GetNextCode("clients", "F")
        LabelClientPCode.Text = GetNextCode("clients", "P")
        LabelSupplierCode.Text = GetNextCode("suppliers", "S")
    End Sub
End Class
