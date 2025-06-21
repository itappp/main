Imports OpenQA.Selenium
Imports OpenQA.Selenium.Chrome
Imports OpenQA.Selenium.Support.UI
Imports WebDriverManager
Imports WebDriverManager.DriverConfigs.Impl
Imports System.Net
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports MySql.Data.MySqlClient
Imports Org.BouncyCastle.Asn1.Cmp
Imports System.IO

Public Class fetchrowform
    Private sqlServerConnectionString As String = "Server=localhost;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"
    Private stopwatch As Stopwatch
    Private mysqlServerConnectionString As String = "Server=180.1.1.3;Database=wm;Uid=root1;Pwd=WMg2024$;"

    ' When the form loads, fetch the Work Order IDs and populate the defects table
    Private Sub fetchrowform_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.WindowState = FormWindowState.Maximized
        Me.FormBorderStyle = FormBorderStyle.Sizable
        Me.MaximumSize = Screen.PrimaryScreen.WorkingArea.Size

        lblUsername.Text = "Logged in as: " & LoggedInUsername
        ' Retrieve the public_name for the logged-in user
        Dim publicName As String = GetPublicName(LoggedInUsername)
        LoadUsers()
        PopulateDefectsTable()
        StartTimer()
    End Sub

    Private Sub btnFetchData_Click(sender As Object, e As EventArgs) Handles btnFetchData.Click
        ' Get the local machine's IP address
        Dim localIp As String = GetLocalIPAddress()

        ' Map the IP address to the corresponding URL
        Dim url As String = GetUrlByIp(localIp)

        ' XPaths to the desired elements for height, width, and speed
        Dim xpaths As New Dictionary(Of String, String) From {
        {"height", "/html/body/div[2]/div[8]/span"},
        {"speed", "/html/body/div[2]/div[5]/span"}
    }

        ' Fetch and display the values
        If Not String.IsNullOrEmpty(url) Then
            FetchHmiValue(url, xpaths, New List(Of String) From {"height", "width", "speed"})
        Else
            MessageBox.Show("No URL mapped for this IP address.")
        End If
    End Sub



    ' Method to get the local IP address
    Private Function GetLocalIPAddress() As String
        Dim hostName As String = Dns.GetHostName()
        Dim ipAddresses As IPAddress() = Dns.GetHostAddresses(hostName)

        ' Return the first non-local IP address
        For Each ip In ipAddresses
            If Not ip.ToString().StartsWith("127.") AndAlso ip.AddressFamily = Sockets.AddressFamily.InterNetwork Then
                Return ip.ToString()
            End If
        Next

        Return String.Empty
    End Function

    ' Method to map IP address to the corresponding URL
    Private Function GetUrlByIp(ip As String) As String
        Select Case ip
            Case "150.1.1.118"
                Return "http://192.168.0.121/"
            Case "192.168.0.118"
                Return "http://192.168.0.117/"
            Case "192.168.0.122"
                Return "http://192.168.0.121/"
            Case "192.168.0.151"
                Return "http://192.168.0.128/"

            Case Else
                ' Handle cases where the IP address is not mapped
                Return "http://default-url/" ' Replace with a default URL or handle accordingly
        End Select
    End Function

    ' Private Sub to fetch the values
    Private Sub FetchHmiValue(url As String, xpaths As Dictionary(Of String, String), fieldsToFetch As List(Of String))
        Dim driver As ChromeDriver = Nothing

        Try
            ' Check network connectivity first
            If Not IsNetworkAvailable() Then
                MessageBox.Show("لا يوجد اتصال بالإنترنت. يرجى التحقق من اتصالك بالشبكة والمحاولة مرة أخرى.", "خطأ في الاتصال", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            ' Set up Chrome options without requiring internet
            Dim options As New ChromeOptions()
            options.AddArgument("--headless")
            options.AddArgument("--disable-gpu")
            options.AddArgument("--no-sandbox")
            options.AddArgument("--disable-dev-shm-usage")
            options.AddArgument("--window-size=1920,1080")
            options.AddArgument("--log-level=3")
            options.AddArgument("--disable-extensions")
            options.AddArgument("--disable-software-rasterizer")
            options.AddArgument("--ignore-certificate-errors")
            options.AddArgument("--disable-web-security")
            options.AddArgument("--allow-running-insecure-content")

            ' Use network ChromeDriver path
            Dim chromeDriverPath As String = "\\localhost\new app\chromedriver\135.0.7049.84\X64\chromedriver.exe"
            Dim chromeDriverService As ChromeDriverService = ChromeDriverService.CreateDefaultService(Path.GetDirectoryName(chromeDriverPath))
            chromeDriverService.HideCommandPromptWindow = True

            driver = New ChromeDriver(chromeDriverService, options)
            driver.Navigate().GoToUrl(url)

            ' Wait for the page to fully load
            Dim wait As New WebDriverWait(driver, TimeSpan.FromSeconds(30))
            wait.Until(Function(d) CType(d, IJavaScriptExecutor).ExecuteScript("return document.readyState").Equals("complete"))

            Dim values As New Dictionary(Of String, String)()

            ' Loop through each XPath and fetch its value
            For Each item In xpaths
                Dim label As String = item.Key
                Dim xpath As String = item.Value

                Try
                    Dim valueElement As IWebElement = wait.Until(Function(d)
                                                                     Dim elem = d.FindElement(By.XPath(xpath))
                                                                     If elem.Displayed AndAlso elem.Enabled Then
                                                                         Return elem
                                                                     Else
                                                                         Return Nothing
                                                                     End If
                                                                 End Function)

                    values(label) = valueElement.Text.Trim()
                Catch ex As WebDriverTimeoutException
                    values(label) = "Element not found"
                Catch ex As NoSuchElementException
                    values(label) = "Element not found"
                Catch ex As Exception
                    values(label) = $"Error: {ex.Message}"
                End Try
            Next

            ' Assign the fetched values to the corresponding textboxes
            If fieldsToFetch.Contains("height") AndAlso values.ContainsKey("height") Then txtHeight.Text = values("height")
            If fieldsToFetch.Contains("width") AndAlso values.ContainsKey("width") Then txtWidth.Text = values("width")
            If fieldsToFetch.Contains("speed") AndAlso values.ContainsKey("speed") Then txtSpeed.Text = values("speed")

        Catch ex As WebDriverException
            MessageBox.Show("حدث خطأ في الاتصال بالجهاز. يرجى التحقق من:" & vbCrLf & 
                          "1. اتصال الشبكة" & vbCrLf & 
                          "2. تشغيل الجهاز" & vbCrLf & 
                          "3. إمكانية الوصول إلى عنوان IP الجهاز", 
                          "خطأ في الاتصال", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MessageBox.Show($"حدث خطأ أثناء جلب البيانات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If driver IsNot Nothing Then
                Try
                    driver.Quit()
                Catch ex As Exception
                    ' Ignore errors during driver cleanup
                End Try
            End If
        End Try
    End Sub

    ' Helper function to check network connectivity
    Private Function IsNetworkAvailable() As Boolean
        Try
            Using ping As New System.Net.NetworkInformation.Ping()
                Dim reply = ping.Send("8.8.8.8", 2000)
                Return reply.Status = System.Net.NetworkInformation.IPStatus.Success
            End Using
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Sub New(selectedWorderId As String)
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        LoadWorderIDs()
        cmbworder.SelectedItem = selectedWorderId
        cmbworder.Enabled = False
    End Sub

    Private Sub LoadUsers()
        Dim query As String = "SELECT id, user_ar FROM dep_users WHERE department = 'row_inspection'"

        Using conn As New SqlConnection(sqlServerConnectionString)
            Dim cmd As New SqlCommand(query, conn)
            Try
                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()

                Dim dt As New DataTable()
                dt.Load(reader)

                cmbuser2.DataSource = dt
                cmbuser2.DisplayMember = "user_ar"
                cmbuser2.ValueMember = "id"
                cmbuser2.SelectedIndex = -1 ' عدم تحديد أي عنصر بشكل افتراضي
            Catch ex As Exception
                MessageBox.Show("Error loading users: " & ex.Message)
            End Try
        End Using
    End Sub



    Private Sub LoadWorderIDs()
        Dim query As String = "SELECT DISTINCT worderid FROM techdata"

        Using conn As New SqlConnection("Server=localhost;Database=wmg-po;User Id=sa;Password=admin@WMG2024;")
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
        lbltotalm.Text = "إجمالي الطول: N/A"
        lbltotalw.Text = "إجمالي الوزن: N/A"
        lblclient.Text = "عميل: N/A"
        lblcolor.Text = "اللون: N/A"
        lblmaterial.Text = "الخامة: N/A"
        lbltotalm.BackColor = SystemColors.Control
        lbltotalw.BackColor = SystemColors.Control

        ' SQL Query to retrieve details for the selected Work Order
        Dim query As String = "SELECT td.worderid, c.contractno, c.batch, td.qty_m, td.qty_kg,cl.code,c.color,c.material " &
                          "FROM techdata td " &
                          "LEFT JOIN Contracts c ON td.contract_id = c.ContractID " &
                          "LEFT JOIN Clients cl ON c.clientcode = cl.id " &
                          "WHERE td.worderid = @worderid"

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
                        lblclient.Text = "عميل: " & If(reader("code") IsNot DBNull.Value, reader("code").ToString(), "N/A")
                        lblcolor.Text = "اللون: " & If(reader("color") IsNot DBNull.Value, reader("color").ToString(), "N/A")
                        lblmaterial.Text = "الخامة: " & If(reader("material") IsNot DBNull.Value, reader("material").ToString(), "N/A")
                    End If
                    reader.Close() ' Close reader to execute next query
                Catch ex As Exception
                    MessageBox.Show("Error loading Work Order details: " & ex.Message)
                    Return
                End Try
            End Using
        End Using

        ' SQL Query to sum height and weight for the selected Work Order
        Dim sumQuery As String = "SELECT SUM(height) AS totalHeight, SUM(weight) AS totalWeight " &
                             "FROM row_inspect " &
                             "WHERE worder_id = @worderid"

        Using conn As New SqlConnection(sqlServerConnectionString)
            Using cmd As New SqlCommand(sumQuery, conn)
                cmd.Parameters.AddWithValue("@worderid", cmbworder.Text)
                Try
                    conn.Open()
                    Dim reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        Dim totalHeight As Decimal = If(reader("totalHeight") IsNot DBNull.Value, Convert.ToDecimal(reader("totalHeight")), 0)

                        lbltotalm.Text = " متر فحص: " & totalHeight.ToString()


                        ' Compare values and set colors
                        Dim qtym As Decimal = If(lblqtym.Text <> "الكمية متر: N/A", Convert.ToDecimal(lblqtym.Text.Replace("الكمية متر: ", "")), 0)
                        Dim qtykg As Decimal = If(lblqtykg.Text <> "الكمية وزن: N/A", Convert.ToDecimal(lblqtykg.Text.Replace("الكمية وزن: ", "")), 0)

                        Dim lowerBoundM As Decimal = qtym * 0.92D
                        Dim upperBoundM As Decimal = qtym * 1.08D
                        Dim lowerBoundW As Decimal = qtykg * 0.92D
                        Dim upperBoundW As Decimal = qtykg * 1.08D

                        If totalHeight = 0 Then
                            lbltotalm.BackColor = Color.Red
                        ElseIf totalHeight < lowerBoundM OrElse totalHeight > upperBoundM Then
                            lbltotalm.BackColor = Color.Red
                        Else
                            lbltotalm.BackColor = Color.Green
                        End If


                    End If
                    reader.Close()
                Catch ex As Exception
                    MessageBox.Show("Error loading total height: " & ex.Message)
                End Try
            End Using
        End Using
    End Sub

    Private Sub PopulateDefectsTable()
        ' Define the columns for the DataGridView
        dataGridViewDefects.Columns.Clear()

        ' Add a combo box column for defects with auto-complete
        Dim defectColumn As New DataGridViewComboBoxColumn()
        defectColumn.Name = "Defect"
        defectColumn.HeaderText = "Defect العيب"
        defectColumn.DataSource = GetDefectOptions()
        defectColumn.DisplayMember = "name_ar"
        defectColumn.ValueMember = "id"
        dataGridViewDefects.Columns.Add(defectColumn)

        dataGridViewDefects.Columns.Add("DefectPlace", "Defect place مكان العيب")

        ' Add a combo box column for points
        Dim pointColumn As New DataGridViewComboBoxColumn()
        pointColumn.Name = "Point"
        pointColumn.HeaderText = "Point النقاط"
        pointColumn.Items.AddRange("Point 1", "Point 2", "Point 3", "Point 4")
        dataGridViewDefects.Columns.Add(pointColumn)

        ' Add rows to the DataGridView
        For i As Integer = 1 To 50
            dataGridViewDefects.Rows.Add(i)
        Next

        ' Set the header style
        Dim headerStyle As New DataGridViewCellStyle()
        headerStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        headerStyle.BackColor = Color.LightBlue
        headerStyle.Font = New Font(dataGridViewDefects.Font, FontStyle.Bold)
        dataGridViewDefects.ColumnHeadersDefaultCellStyle = headerStyle

        ' Set the header row height
        dataGridViewDefects.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing
        dataGridViewDefects.ColumnHeadersHeight = 40

        ' Add auto-complete functionality to the defect column
        AddHandler dataGridViewDefects.EditingControlShowing, AddressOf dataGridViewDefects_EditingControlShowing
    End Sub

    Private Sub dataGridViewDefects_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles dataGridViewDefects.EditingControlShowing
        If dataGridViewDefects.CurrentCell.ColumnIndex = dataGridViewDefects.Columns("Defect").Index Then
            Dim autoCombo As ComboBox = TryCast(e.Control, ComboBox)
            If autoCombo IsNot Nothing Then
                autoCombo.DropDownStyle = ComboBoxStyle.DropDown
                autoCombo.AutoCompleteMode = AutoCompleteMode.SuggestAppend
                autoCombo.AutoCompleteSource = AutoCompleteSource.ListItems

                ' Add event handler for SelectedIndexChanged
                AddHandler autoCombo.SelectedIndexChanged, AddressOf AutoCombo_SelectedIndexChanged
            End If
        End If
    End Sub

    Private Sub AutoCombo_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim comboBox As ComboBox = CType(sender, ComboBox)
        Dim selectedDefectId As Integer

        ' Check if the selected value is a DataRowView
        If TypeOf comboBox.SelectedValue Is DataRowView Then
            Dim rowView As DataRowView = CType(comboBox.SelectedValue, DataRowView)
            selectedDefectId = Convert.ToInt32(rowView("id"))
        Else
            selectedDefectId = Convert.ToInt32(comboBox.SelectedValue)
        End If

        ' Get the current row
        Dim currentRow As DataGridViewRow = dataGridViewDefects.CurrentRow

        ' Check if the selected defect ID matches the specified IDs
        If selectedDefectId = 126 OrElse selectedDefectId = 127 OrElse selectedDefectId = 129 OrElse selectedDefectId = 136 OrElse selectedDefectId = 137 Then
            currentRow.Cells("DefectPlace").Value = "1"
            currentRow.Cells("Point").Value = "Point 4"
        ElseIf selectedDefectId = 134 Then
            currentRow.Cells("Point").Value = "Point 4"
            currentRow.Cells("DefectPlace").Value = String.Empty
        Else
            ' Clear the values for manual input
            currentRow.Cells("DefectPlace").Value = String.Empty
            currentRow.Cells("Point").Value = String.Empty
        End If
    End Sub


    Private Function GetDefectOptions() As DataTable
        Dim dt As New DataTable()
        dt.Columns.Add("id", GetType(Integer))
        dt.Columns.Add("name_ar", GetType(String))

        Dim query As String = "SELECT id, name_ar FROM gray_defects WHERE raw=1 ORDER BY name ASC"
        Using conn As New SqlConnection(sqlServerConnectionString)
            Dim cmd As New SqlCommand(query, conn)
            Try
                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                While reader.Read()
                    dt.Rows.Add(reader("id"), reader("name_ar"))
                End While
            Catch ex As Exception
                MessageBox.Show("Error loading defects: " & ex.Message)
            End Try
        End Using

        Return dt
    End Function


    ' Method to start the timer
    Private Sub StartTimer()
        stopwatch = New Stopwatch()
        stopwatch.Start()
    End Sub


    Private Sub btnInsert_Click(sender As Object, e As EventArgs) Handles btninsert.Click
        ' Validate input values
        If String.IsNullOrEmpty(txtHeight.Text) OrElse String.IsNullOrEmpty(txtWidth.Text) OrElse String.IsNullOrEmpty(txtSpeed.Text) Then
            MessageBox.Show("برجاء كتابه البيانات المطلوبه فى الحقل الفارغ")
            Return
        End If

        Dim height As Decimal
        Dim width As Decimal
        Dim speed As Decimal

        If Not Decimal.TryParse(txtHeight.Text, height) OrElse height = 0 Then
            MessageBox.Show("غير مقبول قيمه صفر فى الطول")
            Return
        End If
        If height < 0 Then
            MessageBox.Show("غير مقبول القيمه بالسالب فى الطول")
            Return
        End If

        If Not Decimal.TryParse(txtWidth.Text, width) OrElse width = 0 Then
            MessageBox.Show("غير مقبول قيمه صفر فى العرض")
            Return
        End If
        If width < 0 Then
            MessageBox.Show("غير مقبول القيمه بالسالب فى العرض")
            Return
        End If

        If Not Decimal.TryParse(txtSpeed.Text, speed) OrElse speed = 0 Then
            MessageBox.Show("غير مقبول قيمه صفر فى السرعه")
            Return
        End If
        If speed < 0 Then
            MessageBox.Show("غير مقبول القيمه بالسالب فى السرعه")
            Return
        End If

        ' Show confirmation message with height and width values
        Dim confirmationMessage As String = $"هل تريد بالفعل تسجيل الطول: {height} والعرض: {width}؟"
        Dim result As DialogResult = MessageBox.Show(confirmationMessage, "تأكيد", MessageBoxButtons.YesNo)
        If result = DialogResult.No Then
            Return
        End If

        ' Check if chkadd is checked and add 4 meters to height
        If chkadd.Checked Then
            height += 4
        End If

        ' Proceed with the existing logic if all validations pass
        stopwatch.Stop()
        Dim totalElapsedTime As TimeSpan = stopwatch.Elapsed

        Dim worderId As String = cmbworder.SelectedItem.ToString()
        Dim roll As Integer = GetNextRollNumber(worderId)
        Dim notes As String = txtnotes.Text
        Dim dateValue As DateTime = DateTime.Now
        Dim degree As Decimal

        Dim ipAddress As String = GetLocalIPAddress()
        Dim pcName As String = Environment.MachineName

        ' Get the selected user id from cmbuser2
        Dim selectedUserId As Integer = Convert.ToInt32(cmbuser2.SelectedValue)

        ' Insert data into SQL Server
        If InsertDataIntoSqlServer(worderId, roll, notes, dateValue, height, width, speed.ToString(), degree, totalElapsedTime, ipAddress, pcName, selectedUserId) Then
            MessageBox.Show("Data inserted successfully.")
            Dim selectedWorderId As String = cmbworder.SelectedItem.ToString()

            ' Hide the fetchfinishform
            Me.Hide()

            ' Show the mainfinishinspectform and set the selected worderId
            Dim mainForm As New mainrowinspectform()
            mainForm.SetSelectedWorderId(selectedWorderId)
            mainForm.Show()
        End If
    End Sub
    Private Function InsertDataIntoSqlServer(worderId As String, roll As Integer, notes As String, dateValue As DateTime, height As Decimal, width As Decimal, speed As String, degree As Decimal, totalElapsedTime As TimeSpan, ipAddress As String, pcName As String, userId As Integer) As Boolean
        Using conn As New SqlConnection(sqlServerConnectionString)
            conn.Open()

            Try
                ' Get techid
                Dim techid As Integer? = GetTechId(worderId)

                ' Insert fabric_grade 1
                Dim heightToInsert1 As Decimal = height

                Dim elapsedTime1 As TimeSpan = totalElapsedTime
                Dim fabricGrade As Integer = 1

                Dim query As String = "INSERT INTO row_inspect (worder_id, roll, notes, date, height, width, ip_address, pc_name, elapsed_time, speed, fabric_grade, username, techid, username2) " &
                                  "VALUES (@worder_id, @roll, @notes, @date, @height, @width, @ip_address, @pc_name, @elapsed_time, @speed, @fabric_grade, @username, @techid, @username2)"

                Dim cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@worder_id", worderId)
                cmd.Parameters.AddWithValue("@roll", roll)
                cmd.Parameters.AddWithValue("@notes", notes)
                cmd.Parameters.AddWithValue("@date", dateValue)
                cmd.Parameters.AddWithValue("@height", heightToInsert1)
                cmd.Parameters.AddWithValue("@width", width)
                cmd.Parameters.AddWithValue("@ip_address", ipAddress)
                cmd.Parameters.AddWithValue("@pc_name", pcName)
                cmd.Parameters.AddWithValue("@elapsed_time", elapsedTime1.ToString("hh\:mm\:ss"))
                cmd.Parameters.AddWithValue("@speed", speed)
                cmd.Parameters.AddWithValue("@fabric_grade", fabricGrade)
                cmd.Parameters.AddWithValue("@username", LoggedInUsername)
                cmd.Parameters.AddWithValue("@techid", techid)
                cmd.Parameters.AddWithValue("@username2", userId)

                cmd.ExecuteNonQuery()

                ' Insert defects into row_inspect_defects table for fabric grade 1 only
                For Each row As DataGridViewRow In dataGridViewDefects.Rows
                    If Not row.IsNewRow AndAlso row.Cells("Defect").Value IsNot Nothing Then
                        Dim defectId As Integer = Convert.ToInt32(row.Cells("Defect").Value)
                        Dim defPlace As String = If(row.Cells("DefectPlace").Value IsNot Nothing, row.Cells("DefectPlace").Value.ToString(), String.Empty)
                        Dim point As Integer = 0
                        If row.Cells("Point").Value IsNot Nothing Then
                            Select Case row.Cells("Point").Value.ToString()
                                Case "Point 1"
                                    point = 1
                                Case "Point 2"
                                    point = 2
                                Case "Point 3"
                                    point = 3
                                Case "Point 4"
                                    point = 4
                            End Select
                        End If

                        ' Insert defects for fabric grade 1 only
                        Dim defectQuery As String = "INSERT INTO row_inspect_defects (worder_id, roll, notes, date, defect_id, def_place, point) " &
                                                "VALUES (@worder_id, @roll, @notes, @date, @defect_id, @def_place, @point)"

                        Dim defectCmd As New SqlCommand(defectQuery, conn)
                        defectCmd.Parameters.AddWithValue("@worder_id", worderId)
                        defectCmd.Parameters.AddWithValue("@roll", roll) ' Use roll for fabric grade 1
                        defectCmd.Parameters.AddWithValue("@notes", notes)
                        defectCmd.Parameters.AddWithValue("@date", dateValue)
                        defectCmd.Parameters.AddWithValue("@defect_id", defectId)
                        defectCmd.Parameters.AddWithValue("@def_place", defPlace)
                        defectCmd.Parameters.AddWithValue("@point", point)

                        defectCmd.ExecuteNonQuery()
                    End If
                Next

                Return True
            Catch ex As Exception
                MessageBox.Show("Error inserting/updating data in SQL Server: " & ex.Message)
                Return False
            End Try
        End Using
    End Function

    Private Function GetTechId(worderId As String) As Integer?
        Dim techid As Integer? = Nothing
        Dim query As String = "SELECT id FROM techdata WHERE worderid = @worderid"

        Using conn As New SqlConnection(sqlServerConnectionString)
            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@worderid", worderId)
            Try
                conn.Open()
                Dim result = cmd.ExecuteScalar()
                If result IsNot Nothing Then
                    techid = Convert.ToInt32(result)
                End If
            Catch ex As Exception
                MessageBox.Show("Error selecting techid: " & ex.Message)
            End Try
        End Using

        Return techid
    End Function


    Private Function GetNextRollNumber(worderId As String) As Integer
        Dim query As String = "SELECT ISNULL(MAX(roll), 0) + 1 FROM row_inspect WHERE worder_id = @worder_id"
        Using conn As New SqlConnection(sqlServerConnectionString)
            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@worder_id", worderId)
            Try
                conn.Open()
                Return Convert.ToInt32(cmd.ExecuteScalar())
            Catch ex As Exception
                MessageBox.Show("Error getting next roll number: " & ex.Message)
                Return 1
            End Try
        End Using
    End Function

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


