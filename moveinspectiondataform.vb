Imports System.Data.SqlClient
Imports System.Windows.Forms

Partial Public Class moveinspectiondataform
    Private connectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"
    Private selectedWorkOrder As String
    Private selectedRolls As New List(Of Integer)
    Private LoggedInUsername As String

    ' UI Controls
    Private cmbWorkOrder As ComboBox
    Private lstRolls As ListBox
    Private btnMove As Button
    Private lblStatus As Label

    Public Sub New()
        InitializeComponent()
        SetupUI()

        ' Get username from database
        If GetUserFromDatabase() Then
            SetupForm()
        Else
            Me.Close()
        End If
    End Sub

    Private Function GetUserFromDatabase() As Boolean
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                ' Get the first active user with full permissions
                Dim query As String = "SELECT TOP 1 username FROM dep_users WHERE full_perm = 1"
                Using cmd As New SqlCommand(query, conn)
                    Dim result = cmd.ExecuteScalar()
                    If result Is Nothing Then
                        MessageBox.Show("لم يتم العثور على مستخدم لديه صلاحيات كاملة")
                        Return False
                    End If
                    LoggedInUsername = result.ToString()
                    Return True
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show($"خطأ في الحصول على بيانات المستخدم: {ex.Message}")
            Return False
        End Try
    End Function

    Private Function HasPermission() As Boolean
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim query As String = "SELECT full_perm FROM dep_users WHERE username = @username"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@username", LoggedInUsername)
                    Dim result = cmd.ExecuteScalar()

                    If result Is Nothing Then
                        MessageBox.Show($"لم يتم العثور على المستخدم في قاعدة البيانات. اسم المستخدم: {LoggedInUsername}")
                        Return False
                    End If

                    Dim permValue = Convert.ToInt32(result)
                    If permValue <> 1 Then
                        MessageBox.Show($"ليس لديك صلاحية كافية. قيمة الصلاحية: {permValue}")
                        Return False
                    End If

                    Return True
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show($"خطأ في التحقق من الصلاحيات: {ex.Message}" & vbCrLf &
                          $"اسم المستخدم: {LoggedInUsername}")
            Return False
        End Try
    End Function

    Private Sub SetupUI()
        ' Work Order ComboBox
        Me.cmbWorkOrder = New ComboBox()
        Me.cmbWorkOrder.Location = New Point(20, 20)
        Me.cmbWorkOrder.Size = New Size(200, 25)
        Me.cmbWorkOrder.DropDownStyle = ComboBoxStyle.DropDown ' Changed to allow typing
        Me.Controls.Add(cmbWorkOrder)

        ' Rolls ListBox (Multiple Selection)
        Me.lstRolls = New ListBox()
        Me.lstRolls.Location = New Point(20, 60)
        Me.lstRolls.Size = New Size(200, 100)
        Me.lstRolls.SelectionMode = SelectionMode.MultiSimple
        Me.Controls.Add(lstRolls)

        ' Move Button
        Me.btnMove = New Button()
        Me.btnMove.Location = New Point(20, 170)
        Me.btnMove.Size = New Size(200, 30)
        Me.btnMove.Text = "نقل البيانات"
        Me.Controls.Add(btnMove)

        ' Status Label
        Me.lblStatus = New Label()
        Me.lblStatus.Location = New Point(20, 210)
        Me.lblStatus.Size = New Size(350, 25)
        Me.Controls.Add(lblStatus)

        ' Form settings
        Me.Text = "نقل بيانات الفحص"
        Me.Size = New Size(400, 280)
        Me.StartPosition = FormStartPosition.CenterScreen

        ' Add event handlers
        AddHandler cmbWorkOrder.TextChanged, AddressOf CmbWorkOrder_TextChanged
        AddHandler btnMove.Click, AddressOf BtnMove_Click
    End Sub

    Private Sub SetupForm()
        ' Check if user has permission
        If Not HasPermission() Then
            MessageBox.Show("ليس لديك صلاحية لاستخدام هذه الشاشة")
            Me.Close()
            Return
        End If

        LoadWorkOrders()
    End Sub

    Private Sub LoadWorkOrders()
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim query As String = "SELECT DISTINCT worder_id FROM finish_inspect ORDER BY worder_id"
                Using cmd As New SqlCommand(query, conn)
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        cmbWorkOrder.Items.Clear()
                        While reader.Read()
                            cmbWorkOrder.Items.Add(reader("worder_id").ToString())
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading work orders: " & ex.Message)
        End Try
    End Sub

    Private Sub CmbWorkOrder_TextChanged(sender As Object, e As EventArgs)
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim query As String = "SELECT DISTINCT worder_id FROM finish_inspect WHERE worder_id LIKE @search + '%' ORDER BY worder_id"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@search", cmbWorkOrder.Text)
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        cmbWorkOrder.Items.Clear()
                        While reader.Read()
                            cmbWorkOrder.Items.Add(reader("worder_id").ToString())
                        End While
                    End Using
                End Using
            End Using

            If cmbWorkOrder.Items.Count > 0 AndAlso Not String.IsNullOrEmpty(cmbWorkOrder.Text) Then
                cmbWorkOrder.DroppedDown = True
                Dim currentText As String = cmbWorkOrder.Text
                cmbWorkOrder.SelectionStart = currentText.Length
            End If

            ' Load rolls if exact match is found
            If cmbWorkOrder.Items.Contains(cmbWorkOrder.Text) Then
                selectedWorkOrder = cmbWorkOrder.Text
                LoadRolls(selectedWorkOrder)
            End If
        Catch ex As Exception
            MessageBox.Show("Error searching work orders: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadRolls(workOrder As String)
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim query As String = "SELECT roll FROM finish_inspect WHERE worder_id = @worder_id ORDER BY roll"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@worder_id", workOrder)
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        lstRolls.Items.Clear()
                        While reader.Read()
                            lstRolls.Items.Add(reader("roll"))
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading rolls: " & ex.Message)
        End Try
    End Sub

    Private Sub BtnMove_Click(sender As Object, e As EventArgs)
        If cmbWorkOrder.SelectedItem Is Nothing OrElse lstRolls.SelectedItems.Count = 0 Then
            MessageBox.Show("الرجاء اختيار أمر الشغل وتوب واحد على الأقل")
            Return
        End If

        selectedWorkOrder = cmbWorkOrder.SelectedItem.ToString()
        selectedRolls.Clear()
        For Each item In lstRolls.SelectedItems
            selectedRolls.Add(Convert.ToInt32(item))
        Next

        Dim successCount As Integer = 0
        For Each roll In selectedRolls
            If MoveInspectionData(roll) Then
                successCount += 1
            End If
        Next

        MessageBox.Show($"تم نقل {successCount} من {selectedRolls.Count} توب بنجاح")
        LoadWorkOrders()
        lstRolls.Items.Clear()
    End Sub

    Private Function MoveInspectionData(roll As Integer) As Boolean
        Dim transaction As SqlTransaction = Nothing
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(connectionString)
            conn.Open()
            transaction = conn.BeginTransaction()

            ' 1. Move roll data to del_finspect_rolls
            Dim moveRollQuery As String = "
                INSERT INTO del_finspect_rolls (
                    worder_id, roll, notes, date, weight, height, width, 
                    username, fabric_grade, ip_address, pc_name, elapsed_time, 
                    speed, links, techid, deleted_at, finspect_id, department, id
                )
                SELECT 
                    worder_id, roll, ISNULL(notes, ''), date, weight, height, width,
                    username, fabric_grade, ip_address, pc_name, elapsed_time,
                    speed, links, techid, GETDATE(), id, N'فحص مجهز',
                    ISNULL(id, 0) -- Use 0 if id is NULL
                FROM finish_inspect 
                WHERE worder_id = @worder_id AND roll = @roll"

            Using cmd As New SqlCommand(moveRollQuery, conn, transaction)
                cmd.Parameters.AddWithValue("@worder_id", selectedWorkOrder)
                cmd.Parameters.AddWithValue("@roll", roll)
                cmd.ExecuteNonQuery()
            End Using

            ' 2. Move defects data to del_finspect_defects
            Dim moveDefectsQuery As String = "
                INSERT INTO del_finspect_defects (
                    worder_id, roll, notes, date, defect_id, def_place, 
                    point, finspect_def_id, department, id
                )
                SELECT 
                    worder_id, roll, ISNULL(notes, ''), date, defect_id, def_place,
                    point, id, N'فحص مجهز',
                    ISNULL(id, (SELECT ISNULL(MAX(id), 0) + 1 FROM del_finspect_defects))
                FROM finish_inspect_defects 
                WHERE worder_id = @worder_id AND roll = @roll"

            Using cmd As New SqlCommand(moveDefectsQuery, conn, transaction)
                cmd.Parameters.AddWithValue("@worder_id", selectedWorkOrder)
                cmd.Parameters.AddWithValue("@roll", roll)
                cmd.ExecuteNonQuery()
            End Using

            ' 3. Delete original data
            Dim deleteDefectsQuery As String = "DELETE FROM finish_inspect_defects WHERE worder_id = @worder_id AND roll = @roll"
            Using cmd As New SqlCommand(deleteDefectsQuery, conn, transaction)
                cmd.Parameters.AddWithValue("@worder_id", selectedWorkOrder)
                cmd.Parameters.AddWithValue("@roll", roll)
                cmd.ExecuteNonQuery()
            End Using

            Dim deleteRollQuery As String = "DELETE FROM finish_inspect WHERE worder_id = @worder_id AND roll = @roll"
            Using cmd As New SqlCommand(deleteRollQuery, conn, transaction)
                cmd.Parameters.AddWithValue("@worder_id", selectedWorkOrder)
                cmd.Parameters.AddWithValue("@roll", roll)
                cmd.ExecuteNonQuery()
            End Using

            transaction.Commit()
            Return True

        Catch ex As Exception
            If transaction IsNot Nothing Then
                Try
                    transaction.Rollback()
                Catch rollbackEx As Exception
                    MessageBox.Show("Error rolling back transaction: " & rollbackEx.Message)
                End Try
            End If
            MessageBox.Show("Error moving inspection data for roll " & roll.ToString() & ": " & ex.Message)
            Return False

        Finally
            If conn IsNot Nothing Then
                conn.Close()
            End If
        End Try
    End Function
End Class