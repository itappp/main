Imports MySql.Data.MySqlClient
Imports System.Data.SqlClient
Imports Excel = Microsoft.Office.Interop.Excel ' For Excel interop
Imports OfficeOpenXml ' Add this line for EPPlus
Imports System.IO
Public Class planningForm
    Private sqlServerConnectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"
    Private machineList As New Dictionary(Of String, String)()
    Private Sub planningForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadWorderIDs()
        ' Access the logged-in username from the global variable
        lblusername.Text = "Logged in as: " & LoggedInUsername
        ' Retrieve the public_name for the logged-in user
        Dim publicName As String = GetPublicName(LoggedInUsername)
        Dim cmbMachine As ComboBox = Panel1.Controls.OfType(Of ComboBox)().FirstOrDefault()
        If cmbMachine IsNot Nothing Then
            LoadMachines(cmbMachine)
        Else

        End If
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
        lblcontractid.Text = "ID: N/A"
        lblkindcontract.Text = "نوع التعاقد: N/A"
        lblcodelib.Text = "library: N/A"

        ' SQL Query
        Dim query As String = "SELECT f.fabrictype_ar, td.worderid, c.contractno, c.batch, c.ContractID, lib.code " &
                      "FROM techdata td " &
                      "LEFT JOIN Contracts c ON td.contract_id = c.ContractID " &
                      "LEFT JOIN fabric f ON c.ContractType = f.id " &
                      "LEFT JOIN lib lib ON td.new_Code_lib = lib.code_id " &
                      "WHERE td.worderid = @worderid"
        ' Connection and command
        Using conn As New SqlConnection(sqlServerConnectionString)
            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@worderid", cmbworder.Text) ' Use Text instead of SelectedItem
            Try
                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()

                If reader.Read() Then
                    lblbatchno.Text = "Batch: " & If(reader("batch") IsNot DBNull.Value, reader("batch").ToString(), "N/A")
                    lblcontractno.Text = "Contract: " & If(reader("contractno") IsNot DBNull.Value, reader("contractno").ToString(), "N/A")
                    lblcontractid.Text = If(reader("ContractID") IsNot DBNull.Value, reader("ContractID").ToString(), "N/A")
                    lblkindcontract.Text = "Type: " & If(reader("fabrictype_ar") IsNot DBNull.Value, reader("fabrictype_ar").ToString(), "N/A")
                    lblcodelib.Text = "library: " & If(reader("code") IsNot DBNull.Value, reader("code").ToString(), "N/A")
                Else

                End If
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using


    End Sub
    Private Sub cmbworder_SelectediIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbworder.SelectedIndexChanged
        ' Clear previous controls in the dynamic Panel
        Panel1.Controls.Clear()

        ' Query to fetch data from database
        Dim query As String = "SELECT np.id, np.proccess_ar, lib.steps_num " &
                          "FROM new_proccess np " &
                          "LEFT JOIN lib lib ON np.id = lib.proccess_id " &
                          "LEFT JOIN techdata td ON lib.code_id = td.new_code_lib " &
                          "WHERE td.worderid = @worderid " &
                          "ORDER BY lib.steps_num"

        Using conn As New SqlConnection(sqlServerConnectionString)
            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@worderid", cmbworder.Text)

            Try
                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()

                Dim yPos As Integer = 20
                While reader.Read()
                    Dim proccessAr As String = If(reader("proccess_ar") IsNot DBNull.Value, reader("proccess_ar").ToString(), String.Empty)
                    Dim npId As Integer = Convert.ToInt32(reader("id"))
                    Dim stepsNum As Integer = If(reader("steps_num") IsNot DBNull.Value, Convert.ToInt32(reader("steps_num")), 0)

                    If Not String.IsNullOrEmpty(proccessAr) Then
                        AddDynamicControls(proccessAr, yPos, npId, stepsNum)
                        yPos += 40 ' Move to the next row
                    End If
                End While

                ' Adjust the height of the panel
                Panel1.Height = yPos + 10 ' Add some padding for aesthetics
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            Finally
                conn.Close()
            End Try
        End Using
    End Sub

    Private Function CheckIfLibCodeExistsInPlanning(ByVal npId As Integer) As Boolean
        Dim query As String = "SELECT COUNT(*) FROM planning WHERE worderid = @worderid AND proccessid = @proccessid"

        Using conn As New SqlConnection(sqlServerConnectionString)
            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@worderid", cmbworder.Text)
            cmd.Parameters.AddWithValue("@proccessid", npId)

            Try
                conn.Open()
                Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                Return count > 0 ' True if proccess exists, otherwise False
            Catch ex As Exception
                MessageBox.Show("Error checking process in planning: " & ex.Message)
                Return False
            Finally
                conn.Close()
            End Try
        End Using
    End Function

    Private Function GetPlanningDetails(ByVal npId As Integer) As DataTable
        Dim query As String = "SELECT m.name_arab as 'الماكينه', p.qtym as 'متر', p.qtykg as 'كيلو', " &
                          "p.datein as 'دخول بتاريخ', p.timein as 'دخول وقت', " &
                          "p.dateout as 'خروج بتاريخ', p.timeout as 'خروج وقت' " &
                          "FROM Planning p " &
                          "LEFT JOIN machine m ON p.machine_id = m.id " &
                          "WHERE p.worderid = @worderid AND p.proccessid = @proccessid"

        Using conn As New SqlConnection(sqlServerConnectionString)
            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@worderid", cmbworder.Text)
            cmd.Parameters.AddWithValue("@proccessid", npId)

            Dim adapter As New SqlDataAdapter(cmd)
            Dim dataTable As New DataTable()

            Try
                conn.Open()
                adapter.Fill(dataTable)
                Return dataTable
            Catch ex As Exception
                MessageBox.Show("Error retrieving planning details: " & ex.Message)
                Return Nothing
            Finally
                conn.Close()
            End Try
        End Using
    End Function

    Private Sub AddDynamicControls(ByVal proccessAr As String, ByRef yPos As Integer, ByVal npId As Integer, ByVal stepsNum As Integer)
        Dim xPos As Integer = 15 ' بداية الإحداثيات الأفقية
        Dim existsInPlanning As Boolean = CheckIfLibCodeExistsInPlanning(npId)
        Dim planningDetails As DataTable = GetPlanningDetails(npId)

        ' Create Label with step number
        Dim lblLib As New Label With {
        .Text = stepsNum.ToString() & ". " & proccessAr,
        .Location = New Point(xPos, yPos),
        .Size = New Size(150, 25),
        .Font = New Font("Arial", 10, FontStyle.Bold),
        .ForeColor = If(planningDetails IsNot Nothing AndAlso planningDetails.Rows.Count > 0, Color.Green, Color.Red),
        .Tag = npId ' Store np.id in the Tag property
    }

        Panel1.Controls.Add(lblLib)
        xPos += 160 ' تحريك الإحداثيات الأفقية

        ' إنشاء CheckBox
        Dim chkToggle As New CheckBox With {
        .Text = "",
        .Location = New Point(xPos, yPos),
        .Size = New Size(20, 25)
    }
        Panel1.Controls.Add(chkToggle)
        xPos += 25 ' تحريك الإحداثيات الأفقية

        ' Add dynamic details if available
        If planningDetails IsNot Nothing AndAlso planningDetails.Rows.Count > 0 Then
            Dim details As DataRow = planningDetails.Rows(0)

            ' Create lblDetails
            Dim lblDetails As New Label With {
            .Text = "الماكينه: " & details("الماكينه").ToString() & ",    متر: " & details("متر").ToString() & ",    كيلو: " & details("كيلو").ToString() & ", " &
                    "دخول بتاريخ: " & details("دخول بتاريخ").ToString() & ",    دخول وقت: " & details("دخول وقت").ToString() & ", " &
                    "خروج بتاريخ: " & details("خروج بتاريخ").ToString() & ",    خروج وقت: " & details("خروج وقت").ToString(),
            .Location = New Point(xPos, yPos),
            .Size = New Size(1000, 25),
            .Font = New Font("Arial", 9)
        }
            Panel1.Controls.Add(lblDetails)

            ' Handle CheckBox CheckedChanged event
            AddHandler chkToggle.CheckedChanged, Sub(sender As Object, e As EventArgs)
                                                     lblDetails.Visible = Not chkToggle.Checked
                                                 End Sub
        End If

        ' إنشاء ComboBox للمكائن
        Dim cmbMachine As New ComboBox With {
        .Location = New Point(xPos, yPos),
        .Size = New Size(150, 25),
        .DropDownStyle = ComboBoxStyle.DropDownList,
        .Visible = False
    }
        LoadMachines(cmbMachine) ' تحميل بيانات المكائن
        Panel1.Controls.Add(cmbMachine)
        xPos += 160 ' تحريك الإحداثيات الأفقية

        ' إنشاء TextBox لـ "M"
        Dim txtM As New TextBox With {
        .Location = New Point(xPos, yPos),
        .Size = New Size(70, 25),
        .Text = "M",
        .ForeColor = Color.Gray,
        .Visible = False
    }
        AddTextBoxHandlers(txtM, "M")
        Panel1.Controls.Add(txtM)
        xPos += 110 ' تحريك الإحداثيات الأفقية

        ' إنشاء TextBox لـ "KG"
        Dim txtKg As New TextBox With {
        .Location = New Point(xPos, yPos),
        .Size = New Size(70, 25),
        .Text = "KG",
        .ForeColor = Color.Gray,
        .Visible = False
    }
        AddTextBoxHandlers(txtKg, "KG")
        Panel1.Controls.Add(txtKg)
        xPos += 110 ' تحريك الإحداثيات الأفقية

        ' إضافة العناصر المتعلقة بالتاريخ والوقت
        AddDateTimeControls(xPos, yPos, chkToggle, cmbMachine, txtM, txtKg)

        ' تحديث اللوحة
        Panel1.Invalidate()
    End Sub



    Private Sub AddTextBoxHandlers(ByVal txtBox As TextBox, ByVal defaultText As String)
        AddHandler txtBox.Enter, Sub(sender, e)
                                     If txtBox.Text = defaultText Then
                                         txtBox.Text = ""
                                         txtBox.ForeColor = Color.Black
                                     End If
                                 End Sub
        AddHandler txtBox.Leave, Sub(sender, e)
                                     If String.IsNullOrWhiteSpace(txtBox.Text) Then
                                         txtBox.Text = defaultText
                                         txtBox.ForeColor = Color.Gray
                                     End If
                                 End Sub
    End Sub

    Private Function CreateLabel(ByVal text As String, ByVal xPos As Integer, ByVal yPos As Integer) As Label
        Return New Label With {
            .Text = text,
            .Location = New Point(xPos, yPos),
            .Size = New Size(100, 20),
            .Visible = False
        }
    End Function

    Private Sub LoadMachines(ByVal cmbMachine As ComboBox)
        Dim query As String = "SELECT id, name_arab FROM machine"

        Using conn As New SqlConnection(sqlServerConnectionString)
            Dim cmd As New SqlCommand(query, conn)

            Try
                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()

                Dim machines As New Dictionary(Of Integer, String)
                machines.Add(-1, "Select a machine") ' Default value prompt

                While reader.Read()
                    Dim id As Integer = Convert.ToInt32(reader("id"))
                    Dim nameArab As String = reader("name_arab").ToString()
                    machines.Add(id, nameArab)
                End While

                ' Bind dictionary to the ComboBox
                cmbMachine.DataSource = New BindingSource(machines, Nothing)
                cmbMachine.DisplayMember = "Value"
                cmbMachine.ValueMember = "Key"
                cmbMachine.SelectedValue = -1 ' Default to prompt value
            Catch ex As Exception
                MessageBox.Show("Error loading machines: " & ex.Message)
            Finally
                conn.Close()
            End Try
        End Using
    End Sub

    Private Sub AddDateTimeControls(ByRef xPos As Integer, ByVal yPos As Integer, ByVal chkToggle As CheckBox, ByVal cmbMachine As ComboBox, ByVal txtM As TextBox, ByVal txtKg As TextBox)


        ' ارتفاع العناوين عن الحقول
        Dim headerOffset As Integer = -20 ' رفع العنوان 20 بكسل أعلى الحقل

        ' إضافة عنوان وتاريخ الدخول
        Dim lblDateIn As Label = CreateLabel("دخول بتاريخ", xPos, yPos + headerOffset)
        lblDateIn.Visible = False ' Set visible to True during testing
        Panel1.Controls.Add(lblDateIn)

        Dim dtpDateIn As DateTimePicker = CreateDatePicker(xPos, yPos)
        dtpDateIn.Name = "dtpDateIn" ' Set the Name property for easier access
        dtpDateIn.Visible = False ' Set visible to True during testing
        Panel1.Controls.Add(dtpDateIn)

        xPos += 170 ' تحريك الإحداثيات الأفقية

        ' إضافة عنوان ووقت الدخول
        Dim lblTimeIn As Label = CreateLabel("دخول وقت", xPos, yPos + headerOffset)
        lblTimeIn.Visible = False ' Set visible to True during testing
        Panel1.Controls.Add(lblTimeIn)

        Dim dtpTimeIn As DateTimePicker = CreateTimePicker(xPos, yPos)
        dtpTimeIn.Name = "dtpTimeIn" ' Set the Name property for easier access
        dtpTimeIn.Visible = False ' Set visible to True during testing
        Panel1.Controls.Add(dtpTimeIn)

        xPos += 170 ' تحريك الإحداثيات الأفقية

        ' إضافة عنوان وتاريخ الخروج
        Dim lblDateOut As Label = CreateLabel("خروج بتاريخ", xPos, yPos + headerOffset)
        lblDateOut.Visible = False ' Set visible to True during testing
        Panel1.Controls.Add(lblDateOut)

        Dim dtpDateOut As DateTimePicker = CreateDatePicker(xPos, yPos)
        dtpDateOut.Name = "dtpDateOut" ' Set the Name property for easier access
        dtpDateOut.Visible = False ' Set visible to True during testing
        Panel1.Controls.Add(dtpDateOut)

        xPos += 170 ' تحريك الإحداثيات الأفقية

        ' إضافة عنوان ووقت الخروج
        Dim lblTimeOut As Label = CreateLabel("خروج وقت", xPos, yPos + headerOffset)
        lblTimeOut.Visible = False ' Set visible to True during testing
        Panel1.Controls.Add(lblTimeOut)

        Dim dtpTimeOut As DateTimePicker = CreateTimePicker(xPos, yPos)
        dtpTimeOut.Name = "dtpTimeOut" ' Set the Name property for easier access
        dtpTimeOut.Visible = False ' Set visible to True during testing
        Panel1.Controls.Add(dtpTimeOut)

        ' إضافة الإزاحة العمودية
        yPos += 40 ' تحريك الصف التالي للحقول

        ' ربط حدث CheckBox لتغيير رؤية العناصر
        AddHandler chkToggle.CheckedChanged, Sub(senderChk As Object, eChk As EventArgs)
                                                 Dim isChecked As Boolean = DirectCast(senderChk, CheckBox).Checked
                                                 lblDateIn.Visible = isChecked
                                                 dtpDateIn.Visible = isChecked
                                                 lblTimeIn.Visible = isChecked
                                                 dtpTimeIn.Visible = isChecked
                                                 lblDateOut.Visible = isChecked
                                                 dtpDateOut.Visible = isChecked
                                                 lblTimeOut.Visible = isChecked
                                                 dtpTimeOut.Visible = isChecked

                                                 ' تغيير رؤية المكائن وصناديق النصوص
                                                 cmbMachine.Visible = isChecked
                                                 txtM.Visible = isChecked
                                                 txtKg.Visible = isChecked
                                             End Sub
    End Sub
    Private Function CreateDatePicker(ByVal xPos As Integer, ByVal yPos As Integer) As DateTimePicker
        Return New DateTimePicker With {
            .Format = DateTimePickerFormat.Short,
            .Location = New Point(xPos, yPos),
            .Size = New Size(130, 25),
            .Visible = False
        }
    End Function

    Private Function CreateTimePicker(ByVal xPos As Integer, ByVal yPos As Integer) As DateTimePicker
        Dim timePicker As New DateTimePicker With {
            .Format = DateTimePickerFormat.Time,
            .ShowUpDown = True,
            .Location = New Point(xPos, yPos),
            .Size = New Size(130, 25),
            .Visible = False
        }
        Return timePicker
    End Function
    Private Sub btnInsert_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnInsert.Click
        ' Loop through each checkbox checked in Panel1
        For Each chkBox As CheckBox In Panel1.Controls.OfType(Of CheckBox)()
            If chkBox.Checked Then
                Dim currentChkBox As CheckBox = chkBox ' Create a local variable to hold the current checkbox

                ' Find the ComboBox, TextBox (M), and TextBox (Kg) related to this CheckBox
                Dim cmbMachine As ComboBox = Panel1.Controls.OfType(Of ComboBox)().
            FirstOrDefault(Function(cb) Math.Abs(cb.Location.Y - currentChkBox.Location.Y) <= 10)
                Dim txtM As TextBox = Panel1.Controls.OfType(Of TextBox)().
            FirstOrDefault(Function(tb) Math.Abs(tb.Location.Y - currentChkBox.Location.Y) <= 10 AndAlso tb.Location.X = currentChkBox.Location.X + 25 + 160)
                Dim txtKg As TextBox = Panel1.Controls.OfType(Of TextBox)().
            FirstOrDefault(Function(tb) Math.Abs(tb.Location.Y - currentChkBox.Location.Y) <= 10 AndAlso tb.Location.X = currentChkBox.Location.X + 25 + 160 + 110)
                Dim lblLib As Label = Panel1.Controls.OfType(Of Label)().
            FirstOrDefault(Function(lbl) Math.Abs(lbl.Location.Y - currentChkBox.Location.Y) <= 10)
                Dim worderId As String = cmbworder.Text ' Get the Work Order ID from combo box

                ' Check if all required controls are found
                If cmbMachine Is Nothing OrElse txtM Is Nothing OrElse txtKg Is Nothing OrElse lblLib Is Nothing Then
                    MessageBox.Show("One or more required controls are missing for the checkbox at Y=" & currentChkBox.Location.Y.ToString())
                    Continue For
                End If

                ' Find the DateTimePickers specific to this CheckBox (using the Y position)
                Dim dtpDateIn As DateTimePicker = Panel1.Controls.OfType(Of DateTimePicker)().
            FirstOrDefault(Function(dtp) Math.Abs(dtp.Location.Y - currentChkBox.Location.Y) <= 10 AndAlso dtp.Name = "dtpDateIn")
                Dim dtpDateOut As DateTimePicker = Panel1.Controls.OfType(Of DateTimePicker)().
            FirstOrDefault(Function(dtp) Math.Abs(dtp.Location.Y - currentChkBox.Location.Y) <= 10 AndAlso dtp.Name = "dtpDateOut")
                Dim dtpTimeIn As DateTimePicker = Panel1.Controls.OfType(Of DateTimePicker)().
            FirstOrDefault(Function(dtp) Math.Abs(dtp.Location.Y - currentChkBox.Location.Y) <= 10 AndAlso dtp.Name = "dtpTimeIn")
                Dim dtpTimeOut As DateTimePicker = Panel1.Controls.OfType(Of DateTimePicker)().
            FirstOrDefault(Function(dtp) Math.Abs(dtp.Location.Y - currentChkBox.Location.Y) <= 10 AndAlso dtp.Name = "dtpTimeOut")

                ' Check if DateTimePickers are missing
                If dtpDateIn Is Nothing OrElse dtpDateOut Is Nothing OrElse dtpTimeIn Is Nothing OrElse dtpTimeOut Is Nothing Then
                    MessageBox.Show("DateTimePickers are missing for the checkbox at Y=" & currentChkBox.Location.Y.ToString())
                    Continue For
                End If

                ' Validate controls first
                If cmbMachine.SelectedValue Is Nothing OrElse Convert.ToInt32(cmbMachine.SelectedValue) = -1 Then
                    MessageBox.Show("Please select a valid machine for the line with CheckBox at Y=" & currentChkBox.Location.Y.ToString())
                    Continue For
                End If

                If String.IsNullOrWhiteSpace(txtM.Text) OrElse String.IsNullOrWhiteSpace(txtKg.Text) Then
                    MessageBox.Show("Please fill out both KG and M fields for the line with CheckBox at Y=" & currentChkBox.Location.Y.ToString())
                    Continue For
                End If

                ' Calculate the time difference between DateIn and DateOut, and TimeIn and TimeOut
                Dim dateIn As DateTime = dtpDateIn.Value
                Dim dateOut As DateTime = dtpDateOut.Value
                Dim timeIn As DateTime = dtpTimeIn.Value
                Dim timeOut As DateTime = dtpTimeOut.Value

                ' Calculate total time in minutes
                Dim totalTime As Double = (dateOut - dateIn).TotalMinutes + (timeOut - timeIn).TotalMinutes

                ' Insert data into the `planning` table
                Dim machineId As Integer = Convert.ToInt32(cmbMachine.SelectedValue)
                Dim npId As Integer = Convert.ToInt32(lblLib.Tag) ' Use the stored np.id from the Tag property

                ' Get the step number from the label text (format is "1. Process Name")
                Dim stepOrder As Integer = 0
                If lblLib.Text.Contains(".") Then
                    Dim stepText As String = lblLib.Text.Split(".")(0)
                    Integer.TryParse(stepText, stepOrder)
                End If

                Dim queryMachinePlanning As String = "INSERT INTO planning (qtykg, qtym, machineid, proccessid, worderId, datein, timein, dateout, timeout, total_time, datetrans, username, step) " &
                                             "VALUES (@qtykg, @qtym, @machineid, @proccessid, @worderid, @datein, @timein, @dateout, @timeout, @total_time, @datetrans, @username, @step)"

                Using conn As New SqlConnection(sqlServerConnectionString)
                    Using cmd As New SqlCommand(queryMachinePlanning, conn)
                        cmd.Parameters.AddWithValue("@qtykg", txtKg.Text)
                        cmd.Parameters.AddWithValue("@qtym", txtM.Text)
                        cmd.Parameters.AddWithValue("@machineid", machineId)
                        cmd.Parameters.AddWithValue("@proccessid", npId) ' Use np.id for the proccess
                        cmd.Parameters.AddWithValue("@worderid", worderId)
                        cmd.Parameters.AddWithValue("@datein", dtpDateIn.Value)
                        cmd.Parameters.AddWithValue("@timein", dtpTimeIn.Value)
                        cmd.Parameters.AddWithValue("@timeout", dtpTimeOut.Value)
                        cmd.Parameters.AddWithValue("@dateout", dtpDateOut.Value)
                        cmd.Parameters.AddWithValue("@total_time", totalTime) ' Insert the calculated total time
                        cmd.Parameters.AddWithValue("@datetrans", DateTime.Now)
                        cmd.Parameters.AddWithValue("@username", LoggedInUsername)
                        cmd.Parameters.AddWithValue("@step", stepOrder)
                        Try
                            conn.Open()
                            cmd.ExecuteNonQuery()
                            MessageBox.Show("Data for machine line at Y=" & currentChkBox.Location.Y.ToString() & " inserted successfully!")
                        Catch ex As Exception
                            MessageBox.Show("Error inserting data into planning: " & ex.Message)
                        End Try
                    End Using
                End Using
            End If
        Next

        ' Clear inputs in Panel1 after all insertions are done
        ClearPanel1Inputs()
    End Sub

    Private Sub ClearPanel1Inputs()
        For Each ctrl As Control In Panel1.Controls
            If TypeOf ctrl Is CheckBox Then
                DirectCast(ctrl, CheckBox).Checked = False
            ElseIf TypeOf ctrl Is TextBox Then
                DirectCast(ctrl, TextBox).Text = String.Empty
            ElseIf TypeOf ctrl Is ComboBox Then
                DirectCast(ctrl, ComboBox).SelectedValue = -1 ' Reset combo box to default
            ElseIf TypeOf ctrl Is Label Then
                DirectCast(ctrl, Label).Text = String.Empty
            End If
        Next
        Panel1.Controls.Clear()
        cmbworder.Text = String.Empty
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

End Class
