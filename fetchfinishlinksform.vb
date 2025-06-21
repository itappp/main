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
Imports ZXing
Imports ZXing.Common
Imports System.Drawing.Imaging
Public Class fetchfinishlinksform
    Private sqlServerConnectionString As String = "Server=localhost;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"
    Private stopwatch As Stopwatch
    Private mysqlServerConnectionString As String = "Server=180.1.1.3;Database=wm;Uid=root1;Pwd=WMg2024$;"

    Private Sub fetchfinishlinksform_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.WindowState = FormWindowState.Maximized
        Me.FormBorderStyle = FormBorderStyle.Sizable
        Me.MaximumSize = Screen.PrimaryScreen.WorkingArea.Size

        lblUsername.Text = "Logged in as: " & LoggedInUsername
        ' Retrieve the public_name for the logged-in user
        Dim publicName As String = GetPublicName(LoggedInUsername)

        PopulateDefectsTable()
        StartTimer()
    End Sub


    Public Sub New(selectedWorderId As String)
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        LoadWorderIDs()
        cmbworder.SelectedItem = selectedWorderId
        cmbworder.Enabled = False
    End Sub

    Private Sub btnmenu_Click(sender As Object, e As EventArgs) Handles btnmenu.Click

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

        Dim query As String = "SELECT id, name_ar FROM gray_defects WHERE raw=0 ORDER BY name ASC"
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


    Private Sub btnFetchData_Click(sender As Object, e As EventArgs)
        ' Get the local machine's IP address
        Dim localIp As String = GetLocalIPAddress()

        ' Map the IP address to the corresponding URL
        Dim url As String = GetUrlByIp(localIp)

        ' XPaths to the desired elements for height, width, and speed
        Dim xpaths As New Dictionary(Of String, String) From {
        {"height", "/html/body/div[2]/div[8]/span"},
        {"width", "/html/body/div[2]/div[26]/span"},
        {"speed", "/html/body/div[2]/div[6]/span"}
    }

        ' Fetch and display the values
        If Not String.IsNullOrEmpty(url) Then
            FetchHmiValue(url, xpaths, New List(Of String) From {"height", "width", "speed"})
        Else
            MessageBox.Show("No URL mapped for this IP address.")
        End If
    End Sub

    Private Sub btnFetchWeight_Click(sender As Object, e As EventArgs) Handles btnFetchWeight.Click
        ' Get the local machine's IP address
        Dim localIp As String = GetLocalIPAddress()

        ' Map the IP address to the corresponding URL
        Dim url As String = GetUrlByIp(localIp)

        ' XPaths to the desired element for weight
        Dim xpaths As New Dictionary(Of String, String) From {
        {"weight", "/html/body/div[2]/div[3]/span"}
    }

        ' Fetch and display the value
        If Not String.IsNullOrEmpty(url) Then
            FetchHmiValue(url, xpaths, New List(Of String) From {"weight"})
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
                Return "http://192.168.0.103/"
            Case "192.168.0.114"
                Return "http://192.168.0.113/"
            Case "192.168.0.109"
                Return "http://192.168.0.108/"
            Case "192.168.0.104"
                Return "http://192.168.0.103/"
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
            If fieldsToFetch.Contains("weight") AndAlso values.ContainsKey("weight") Then txtWeight.Text = values("weight")

        Catch ex As Exception
            MessageBox.Show($"Error fetching data: {ex.Message}")
        Finally
            If driver IsNot Nothing Then
                driver.Quit()
            End If
        End Try
    End Sub



    ' Method to start the timer
    Private Sub StartTimer()
        stopwatch = New Stopwatch()
        stopwatch.Start()
    End Sub

    Private Sub btnInsert_Click(sender As Object, e As EventArgs) Handles btninsert.Click
        ' Validate input values
        If String.IsNullOrEmpty(txtWeight.Text) Then
            MessageBox.Show("برجاء كتابه البيانات المطلوبه فى الحقل الفارغ")
            Return
        End If

        Dim weight As Decimal

        If Not Decimal.TryParse(txtWeight.Text, weight) OrElse weight = 0 Then
            MessageBox.Show("غير مقبول قيمه صفر فى الوزن")
            Return
        End If

        If weight < 0 Then
            MessageBox.Show("غير مقبول القيمه بالسالب فى الوزن")
            Return
        End If

        ' Show confirmation message with weight value
        Dim confirmationMessage As String = $"هل تريد بالفعل تسجيل الوزن: {weight}؟"
        Dim result As DialogResult = MessageBox.Show(confirmationMessage, "تأكيد", MessageBoxButtons.YesNo)
        If result = DialogResult.No Then
            Return
        End If

        ' Proceed with the existing logic if all validations pass
        stopwatch.Stop()
        Dim totalElapsedTime As TimeSpan = stopwatch.Elapsed

        Dim worderId As String = cmbworder.SelectedItem.ToString()
        Dim notes As String = txtnotes.Text
        Dim dateValue As DateTime = DateTime.Now

        ' Select links from finish_inspect
        Dim links As Integer = SelectLinks(worderId)
        If links <> 3 Then
            MessageBox.Show("Links value is not 3.")
            Return
        End If

        ' Get the techid from techdata table
        Dim techid As Integer? = GetTechId(worderId)
        If Not techid.HasValue Then
            MessageBox.Show("Failed to get techid.")
            Return
        End If

        ' Insert weight into finish_inspect table and get the roll number
        Dim roll As Integer? = InsertWeight(worderId, weight, techid.Value)
        If roll.HasValue Then
            ' Insert defects into finish_inspect_defects table
            If InsertDefects(worderId, roll.Value, notes, dateValue) Then
                MessageBox.Show("Data inserted successfully.")
                Dim selectedWorderId As String = cmbworder.SelectedItem.ToString()

                ' Generate the print content
                Dim printContent As String = GeneratePrintContent(worderId, roll.Value.ToString())

                ' Create a temporary HTML file
                Dim tempFilePath As String = System.IO.Path.GetTempFileName() & ".html"
                System.IO.File.WriteAllText(tempFilePath, printContent)

                ' Open the HTML file in the default browser
                Process.Start(New ProcessStartInfo(tempFilePath) With {.UseShellExecute = True})

                ' Hide the fetchfinishform
                Me.Hide()

                ' Show the mainfinishinspectform and set the selected worderId
                Dim mainForm As New mainfinishinspectform()
                mainForm.SetSelectedWorderId(selectedWorderId)
                mainForm.Show()
            Else
                MessageBox.Show("Failed to insert defects.")
            End If
        Else
            MessageBox.Show("Failed to insert weight.")
        End If
    End Sub
    Private Function GeneratePrintContent(worderId As String, roll As String) As String
        ' Generate barcode
        Dim barcodeWriter As New BarcodeWriter()
        barcodeWriter.Format = BarcodeFormat.CODE_128
        barcodeWriter.Options = New EncodingOptions With {
            .Width = 100,
            .Height = 60,
            .Margin = 10
        }

        ' Create barcode text (worderId + roll)
        Dim barcodeText As String = worderId & "*" & roll
        Dim barcodeBitmap As Bitmap = barcodeWriter.Write(barcodeText)

        ' Save barcode to temporary file
        Dim tempBarcodePath As String = Path.Combine(Path.GetTempPath(), "barcode.png")
        barcodeBitmap.Save(tempBarcodePath, ImageFormat.Png)

        ' Fetch the details for the selected worder and roll
        Dim query As String = "SELECT fi.worder_id as 'رقم الأمر', c.contractno as 'رقم التعاقد', cs.code as 'كود العميل', c.color as 'اللون', c.material as 'الخامة', " &
                          "fi.roll as 'رقم التوب', fi.width as 'العرض', fi.height AS 'الطول', fi.weight as 'الوزن', fi.fabric_grade as 'الدرجة', " &
                          "(SELECT (SUM(point) * 10000) / (fi.width * fi.height) FROM finish_inspect_defects WHERE worder_id = @worderid AND roll = @roll) AS 'نسبة العيوب' " &
                          "FROM finish_inspect fi " &
                          "LEFT JOIN techdata td on fi.worder_id = td.worderid " &
                          "LEFT JOIN Contracts c on td.contract_id = c.ContractID " &
                          "LEFT JOIN clients cs on c.ClientCode = cs.id " &
                          "WHERE fi.worder_id = @worderid AND fi.roll = @roll"

        Dim contractNo As String = ""
        Dim clientCode As String = ""
        Dim color As String = ""
        Dim material As String = ""
        Dim width As Decimal = 0
        Dim length As Decimal = 0
        Dim weight As Decimal = 0
        Dim defectRate As Decimal = 0
        Dim fabricGrade As String = ""

        Using conn As New SqlConnection(sqlServerConnectionString)
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@worderid", worderId)
                cmd.Parameters.AddWithValue("@roll", roll)
                Try
                    conn.Open()
                    Dim reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        contractNo = If(reader("رقم التعاقد") IsNot DBNull.Value, reader("رقم التعاقد").ToString(), "")
                        clientCode = If(reader("كود العميل") IsNot DBNull.Value, reader("كود العميل").ToString(), "")
                        color = If(reader("اللون") IsNot DBNull.Value, reader("اللون").ToString(), "")
                        material = If(reader("الخامة") IsNot DBNull.Value, reader("الخامة").ToString(), "")
                        width = If(reader("العرض") IsNot DBNull.Value, Convert.ToDecimal(reader("العرض")), 0)
                        length = If(reader("الطول") IsNot DBNull.Value, Convert.ToDecimal(reader("الطول")), 0)
                        weight = If(reader("الوزن") IsNot DBNull.Value, Convert.ToDecimal(reader("الوزن")), 0)
                        defectRate = If(reader("نسبة العيوب") IsNot DBNull.Value, Convert.ToDecimal(reader("نسبة العيوب")), 0)
                        fabricGrade = If(reader("الدرجة") IsNot DBNull.Value, reader("الدرجة").ToString(), "")
                    End If
                    reader.Close()
                Catch ex As Exception
                    MessageBox.Show("Error loading print details: " & ex.Message)
                    Return ""
                End Try
            End Using
        End Using

        ' Construct the full path to the image
        Dim imagePath As String = "\\localhost\prod\alllllll\Departmentsa-40-NOinternaL -\Departments\images\logo.jpg"
        If clientCode = "F00009" Then
            imagePath = "\\localhost\prod\alllllll\Departmentsa-40-NOinternaL -\Departments\images\f00009.jpg"
        End If

        ' Determine if the logo and defect rate should be included
        Dim includeLogo As Boolean = Not (clientCode = "F00010" Or clientCode = "F00046" Or clientCode = "F00054" Or clientCode = "F00054" Or clientCode = "F00059")
        Dim includeDefectRate As Boolean = Not (clientCode = "F00009" Or clientCode = "F00010" Or clientCode = "F00046" Or clientCode = "F00054" Or clientCode = "F00059")

        ' Generate the HTML content
        Dim htmlContent As String = "
    <html>
    <head>
        <style>
            table { width: 100%; border-collapse: collapse; font-size: 16px; font-weight: bold; }
            table, th, td { border: 1px solid black; }
            th, td { padding: 10px; text-align: left; }
            img { width: 50%; }
            .barcode { text-align: center; margin-top: 10px; }
            .barcode-text { font-size: 14px; margin-top: 5px; font-weight: bold; }
            .barcode img { width: 70%; max-width: 300px; }
        </style>
    </head>
    <body>"

        If includeLogo Then
            htmlContent &= "
        <center>
            <img src='" & imagePath & "'>
        </center>"
        End If

        htmlContent &= "
        <br><br>
        <table>
            <tr>
                <td><strong>رقم الأمر</strong></td>
                <td>" & worderId & "</td>
            </tr>
            <tr>
                <td><strong>رقم التعاقد</strong></td>
                <td>" & contractNo & "</td>
            </tr>
            <tr>
                <td><strong>كود العميل</strong></td>
                <td>" & clientCode & "</td>
            </tr>
            <tr>
                <td><strong>اللون</strong></td>
                <td>" & color & "</td>
            </tr>
            <tr>
                <td><strong>الخامة</strong></td>
                <td>" & material & "</td>
            </tr>
            <tr>
                <td><strong>رقم التوب</strong></td>
                <td>" & roll & "</td>
            </tr>
            <tr>
                <td><strong>العرض</strong></td>
                <td>" & width & "</td>
            </tr>
            <tr>
                <td><strong>الطول</strong></td>
                <td>" & length & "</td>
            </tr>
            <tr>
                <td><strong>الوزن</strong></td>
                <td>" & weight & "</td>
            </tr>"

        If includeDefectRate Then
            htmlContent &= "
            <tr>
                <td><strong>نسبة العيوب</strong></td>
                <td>" & defectRate.ToString("F2") & "</td>
            </tr>"
        End If

        htmlContent &= "
            <tr>
                <td><strong>الدرجة</strong></td>
                <td>" & fabricGrade & "</td>
            </tr>
        </table>
        <div class='barcode'>
            <img src='" & tempBarcodePath & "' alt='Barcode'>
            <div class='barcode-text'>" & barcodeText & "</div>
        </div>
        <script type='text/javascript'>
            window.print();
        </script>
    </body>
    </html>"

        Return htmlContent
    End Function



    Private Function SelectLinks(worderId As String) As Integer
        Dim links As Integer = -1
        Dim query As String = "SELECT links FROM finish_inspect WHERE worder_id = @worder_id AND links = 3 and weight is null"

        Using conn As New SqlConnection(sqlServerConnectionString)
            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@worder_id", worderId)
            Try
                conn.Open()
                Dim result = cmd.ExecuteScalar()
                If result IsNot Nothing Then
                    links = Convert.ToInt32(result)
                End If
            Catch ex As Exception
                MessageBox.Show("Error selecting links: " & ex.Message)
            End Try
        End Using

        Return links
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

    Private Function InsertWeight(worderId As String, weight As Decimal, techid As Integer) As Integer?
        Dim query As String = "UPDATE finish_inspect SET weight = @weight, fabric_grade = 2, techid = @techid OUTPUT inserted.roll WHERE worder_id = @worder_id AND links = 3 and weight is null"
        Dim roll As Integer? = Nothing

        Using conn As New SqlConnection(sqlServerConnectionString)
            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@worder_id", worderId)
            cmd.Parameters.AddWithValue("@weight", weight)
            cmd.Parameters.AddWithValue("@techid", techid)
            Try
                conn.Open()
                roll = Convert.ToInt32(cmd.ExecuteScalar())
            Catch ex As Exception
                MessageBox.Show("Error updating weight, fabric_grade, and techid: " & ex.Message)
            End Try
        End Using

        Return roll
    End Function


    Private Function InsertDefects(worderId As String, roll As Integer, notes As String, dateValue As DateTime) As Boolean
        Dim query As String = "INSERT INTO finish_inspect_defects (worder_id, roll, notes, date, defect_id, def_place, point) " &
                          "VALUES (@worder_id, @roll, @notes, @date, @defect_id, @def_place, @point)"

        Using conn As New SqlConnection(sqlServerConnectionString)
            Try
                conn.Open()
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

                        ' Insert defects for the specified roll
                        Dim defectCmd As New SqlCommand(query, conn)
                        defectCmd.Parameters.AddWithValue("@worder_id", worderId)
                        defectCmd.Parameters.AddWithValue("@roll", roll)
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
                MessageBox.Show("Error inserting defects: " & ex.Message)
                Return False
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