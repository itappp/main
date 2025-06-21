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
Public Class fetchrowinspectsampleform
    Private sqlServerConnectionString As String = "Server=localhost;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"
    Private stopwatch As Stopwatch
    Private mysqlServerConnectionString As String = "Server=180.1.1.3;Database=wm;Uid=root1;Pwd=WMg2024$;"

    Private Sub fetchrowinspectsampleform_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
            Case "192.168.191.154"
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

        Catch ex As Exception
            MessageBox.Show($"Error fetching data: {ex.Message}")
        Finally
            If driver IsNot Nothing Then
                driver.Quit()
            End If
        End Try
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

    Private Sub dataGridViewDefects_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs)
        If dataGridViewDefects.CurrentCell.ColumnIndex = dataGridViewDefects.Columns("Defect").Index Then
            Dim autoCombo As ComboBox = TryCast(e.Control, ComboBox)
            If autoCombo IsNot Nothing Then
                autoCombo.DropDownStyle = ComboBoxStyle.DropDown
                autoCombo.AutoCompleteMode = AutoCompleteMode.SuggestAppend
                autoCombo.AutoCompleteSource = AutoCompleteSource.ListItems
            End If
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
    Public Sub SetData(data As DataTable, lotData As String, batchData As String)
        ' تأكد من أن الأعمدة 'lot' و 'batch' موجودة في DataTable
        If Not data.Columns.Contains("lot") Then
            data.Columns.Add("lot", GetType(String))
        End If
        If Not data.Columns.Contains("رقم الرسالة") Then
            data.Columns.Add("رقم الرسالة", GetType(String))
        End If

        ' تعيين مصدر البيانات لـ DataGridView
        dataGridViewDetails.DataSource = data

        ' Center the content of the DataGridView
        For Each column As DataGridViewColumn In dataGridViewDetails.Columns
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            column.Width = 150 ' Set a larger width for each column
        Next

        ' Set the font size to 12 and make it bold for content
        dataGridViewDetails.DefaultCellStyle.Font = New Font("Arial", 12, FontStyle.Bold)

        ' Set the header font size to 12, make it bold, and center the content
        dataGridViewDetails.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 14, FontStyle.Bold)
        dataGridViewDetails.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        ' Fill the color of the headers
        dataGridViewDetails.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue
        dataGridViewDetails.EnableHeadersVisualStyles = False

        ' Adjust the width of each column to fit the data
        dataGridViewDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells

        ' Add the lot data and batch data to the DataGridView
        If data.Rows.Count > 0 Then
            ' تحديث الصف الأول ببيانات lot و batch
            data.Rows(0)("lot") = lotData
            data.Rows(0)("رقم الرسالة") = batchData
        Else
            ' إضافة صف جديد ببيانات lot و batch
            Dim newRow As DataRow = data.NewRow()
            newRow("lot") = lotData
            newRow("رقم الرسالة") = batchData
            data.Rows.Add(newRow)
        End If
    End Sub

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
        ' Convert negative height to positive
        height = Math.Abs(height)

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

        ' Proceed with the existing logic if all validations pass
        stopwatch.Stop()
        Dim totalElapsedTime As TimeSpan = stopwatch.Elapsed

        Dim notes As String = txtnotes.Text
        Dim dateValue As DateTime = DateTime.Now
        Dim degree As Decimal

        Dim ipAddress As String = GetLocalIPAddress()
        Dim pcName As String = Environment.MachineName

        ' Get the selected user id from cmbuser2
        Dim selectedUserId As Integer = Convert.ToInt32(cmbuser2.SelectedValue)

        ' Get the batch and lot data from DataGridView
        Dim batchData As String = dataGridViewDetails.Rows(0).Cells("رقم الرسالة").Value.ToString()
        Dim lotData As String = dataGridViewDetails.Rows(0).Cells("lot").Value.ToString()

        ' Get the next roll number
        Dim roll As Integer = GetNextRollNumber(batchData, lotData)

        ' Insert data into SQL Server
        If InsertDataIntoSqlServer(batchData, lotData, roll, notes, dateValue, height, width, speed.ToString(), degree, totalElapsedTime, ipAddress, pcName, selectedUserId) Then
            MessageBox.Show("تم إدخال البيانات بنجاح.")

            ' إخفاء النموذج الحالي
            Me.Hide()

            ' عرض النموذج الرئيسي
            Dim mainForm As New mainrowsampleform()
            mainForm.Show()

            ' تحديد نفس الرسالة واللوت في النموذج الرئيسي
            mainForm.SelectBatchAndLot(batchData, lotData)
        End If
    End Sub
    Private Function InsertDataIntoSqlServer(batchData As String, lotData As String, roll As Integer, notes As String, dateValue As DateTime, height As Decimal, width As Decimal, speed As String, degree As Decimal, totalElapsedTime As TimeSpan, ipAddress As String, pcName As String, userId As Integer) As Boolean
        Using conn As New SqlConnection(sqlServerConnectionString)
            conn.Open()

            Try
                ' Insert fabric_grade 1
                Dim heightToInsert1 As Decimal = height
                Dim elapsedTime1 As TimeSpan = totalElapsedTime
                Dim fabricGrade As Integer = 1

                Dim query As String = "INSERT INTO row_inspect_sample (batch_id, lot, roll, notes, date, height, width, ip_address, pc_name, elapsed_time, speed, fabric_grade, username, username2) " &
                                  "VALUES (@batch_id, @lot, @roll, @notes, @date, @height, @width, @ip_address, @pc_name, @elapsed_time, @speed, @fabric_grade, @username, @username2)"

                Dim cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@batch_id", batchData)
                cmd.Parameters.AddWithValue("@lot", lotData)
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
                cmd.Parameters.AddWithValue("@username2", userId)

                cmd.ExecuteNonQuery()

                ' Insert defects into row_inspect_sample_defects table for fabric grade 1 only
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
                        Dim defectQuery As String = "INSERT INTO row_inspect_sample_defects (batch_id, lot, roll, notes, date, defect_id, def_place, point) " &
                                                "VALUES (@batch_id, @lot, @roll, @notes, @date, @defect_id, @def_place, @point)"

                        Dim defectCmd As New SqlCommand(defectQuery, conn)
                        defectCmd.Parameters.AddWithValue("@batch_id", batchData)
                        defectCmd.Parameters.AddWithValue("@lot", lotData)
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

    Private Function GetNextRollNumber(batchData As String, lotData As String) As Integer
        Dim query As String = "SELECT ISNULL(MAX(roll), 0) + 1 FROM row_inspect_sample WHERE batch_id = @batch_id AND lot = @lot"
        Using conn As New SqlConnection(sqlServerConnectionString)
            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@batch_id", batchData)
            cmd.Parameters.AddWithValue("@lot", lotData)
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