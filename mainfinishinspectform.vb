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
Imports ZXing
Imports ZXing.Common
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Text
Imports System.Threading.Tasks
Public Class mainfinishinspectform
    Private sqlServerConnectionString As String = "Server=localhost;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"
    Private stopwatch As Stopwatch
    Private btnMoveSelected As Button ' Add button for moving selected items

    Private Sub mainfinishinspectform_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.WindowState = FormWindowState.Maximized
        Me.FormBorderStyle = FormBorderStyle.Sizable
        Me.MaximumSize = Screen.PrimaryScreen.WorkingArea.Size

        lblUsername.Text = "Logged in as: " & LoggedInUsername
        ' Retrieve the public_name for the logged-in user
        Dim publicName As String = GetPublicName(LoggedInUsername)
        LoadWorderIDs()

        ' Add checkbox column to DataGridView
        Dim checkBoxColumn As New DataGridViewCheckBoxColumn()
        checkBoxColumn.HeaderText = "اختيار"
        checkBoxColumn.Name = "SelectRow"
        dataGridViewDetails.Columns.Insert(0, checkBoxColumn)

        ' Add Move Selected button
        btnMoveSelected = New Button()
        btnMoveSelected.Text = "نقل المحدد"
        btnMoveSelected.Size = New Size(100, 30)
        btnMoveSelected.Location = New Point(dataGridViewDetails.Right - 120, dataGridViewDetails.Top - 40)
        Me.Controls.Add(btnMoveSelected)
        AddHandler btnMoveSelected.Click, AddressOf BtnMoveSelected_Click

        AddHandler dataGridViewDetails.CellEndEdit, AddressOf dataGridViewDetails_CellEndEdit
    End Sub

    Private Function GetUserAccessLevel(ByVal username As String) As Integer
        Dim accessLevel As Integer = 0
        Try
            Using conn As New SqlConnection(sqlServerConnectionString)
                Dim query As String = "SELECT acc_level FROM dep_users WHERE username = @username"
                Dim cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@username", username)

                conn.Open()
                Dim result = cmd.ExecuteScalar()
                If result IsNot Nothing Then
                    accessLevel = Convert.ToInt32(result)
                End If
                conn.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error retrieving access level: " & ex.Message)
        End Try
        Return accessLevel
    End Function

    Private Sub dataGridViewDetails_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs)
        Dim accessLevel As Integer = GetUserAccessLevel(LoggedInUsername)
        If accessLevel <> 1 Then
            MessageBox.Show("You do not have permission to update this data.")
            Return
        End If

        Dim columnName As String = dataGridViewDetails.Columns(e.ColumnIndex).Name
        If columnName = "الدرجة" Or columnName = "العرض" Or columnName = "notes" Then
            Dim worderId As String = dataGridViewDetails.Rows(e.RowIndex).Cells("رقم الأمر").Value.ToString()
            Dim roll As String = dataGridViewDetails.Rows(e.RowIndex).Cells("رقم التوب").Value.ToString()
            Dim newValue As String = dataGridViewDetails.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString()

            Dim query As String = ""
            If columnName = "الدرجة" Then
                query = "UPDATE finish_inspect SET fabric_grade = @newValue WHERE worder_id = @worderId AND roll = @roll"
            ElseIf columnName = "العرض" Then
                query = "UPDATE finish_inspect SET width = @newValue WHERE worder_id = @worderId AND roll = @roll"
            ElseIf columnName = "notes" Then
                query = "UPDATE finish_inspect SET notes = @newValue WHERE worder_id = @worderId AND roll = @roll"
            End If

            Using conn As New SqlConnection(sqlServerConnectionString)
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@newValue", newValue)
                    cmd.Parameters.AddWithValue("@worderId", worderId)
                    cmd.Parameters.AddWithValue("@roll", roll)
                    Try
                        conn.Open()
                        cmd.ExecuteNonQuery()
                        MessageBox.Show("Data updated successfully.")
                    Catch ex As Exception
                        MessageBox.Show("Error updating data: " & ex.Message)
                    End Try
                End Using
            End Using
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

    Private Sub cmbworder_TextChanged(sender As Object, e As EventArgs) Handles cmbworder.TextChanged
        Dim text As String = cmbworder.Text
        For Each item As String In cmbworder.Items
            If item.StartsWith(text, StringComparison.OrdinalIgnoreCase) Then
                cmbworder.SelectedItem = item
                cmbworder.SelectionStart = text.Length
                cmbworder.SelectionLength = item.Length - text.Length
                Exit For
            End If
        Next
    End Sub
    Public Sub SetSelectedWorderId(selectedWorderId As String)
        LoadWorderIDs()
        cmbworder.SelectedItem = selectedWorderId

    End Sub

    Private Sub LoadRolls(worderId As String)
        Dim query As String = "SELECT roll FROM finish_inspect WHERE worder_id = @worderid"

        Using conn As New SqlConnection(sqlServerConnectionString)
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@worderid", worderId)
                Try
                    conn.Open()
                    Dim reader As SqlDataReader = cmd.ExecuteReader()

                    cmbroll.Items.Clear()
                    While reader.Read()
                        cmbroll.Items.Add(reader("roll").ToString())
                    End While
                Catch ex As Exception
                    MessageBox.Show("Error loading rolls: " & ex.Message)
                End Try
            End Using
        End Using
    End Sub
    Private Sub cmbroll_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbroll.SelectedIndexChanged
        If cmbworder.SelectedItem IsNot Nothing AndAlso cmbroll.SelectedItem IsNot Nothing Then
            Dim worderId As String = cmbworder.SelectedItem.ToString()
            Dim roll As String = cmbroll.SelectedItem.ToString()

            Dim query As String = "SELECT height, weight FROM finish_inspect WHERE worder_id = @worderid AND roll = @roll"

            Using conn As New SqlConnection(sqlServerConnectionString)
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@worderid", worderId)
                    cmd.Parameters.AddWithValue("@roll", roll)
                    Try
                        conn.Open()
                        Dim reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            lblm.Text = "متر: " & If(reader("height") IsNot DBNull.Value, reader("height").ToString(), "N/A")
                            lblkg.Text = "وزن: " & If(reader("weight") IsNot DBNull.Value, reader("weight").ToString(), "N/A")
                        End If
                        reader.Close()
                    Catch ex As Exception
                        MessageBox.Show("Error loading roll details: " & ex.Message)
                    End Try
                End Using
            End Using
        Else
            MessageBox.Show("Please select a valid Work Order ID and Roll.")
        End If
    End Sub
    Private Sub btnaddroll_Click(sender As Object, e As EventArgs) Handles btnaddroll.Click
        ' Get the selected worderId
        Dim selectedWorderId As String = cmbworder.Text

        ' Check if the selected worderId is in the list
        If cmbworder.Items.Contains(selectedWorderId) Then
            ' Hide the mainfinishinspectform
            Me.Hide()

            ' Create an instance of fetchfinishform with the selected worderId and show it
            Dim fetchForm As New fetchfinishform(selectedWorderId)
            fetchForm.Show()
        Else
            MessageBox.Show("Please select a valid Work Order ID from the list.")
        End If
    End Sub

    Private Sub btnrollnd_Click(sender As Object, e As EventArgs) Handles btnrollnd.Click
        ' Get the selected worderId
        Dim selectedWorderId As String = cmbworder.Text

        ' Check if the selected worderId is in the list
        If cmbworder.Items.Contains(selectedWorderId) Then
            ' Hide the mainfinishinspectform
            Me.Hide()

            ' Create an instance of fetchfinishform with the selected worderId and show it
            Dim fetchForm As New fetchfinishlinksform(selectedWorderId)
            fetchForm.Show()
        Else
            MessageBox.Show("Please select a valid Work Order ID from the list.")
        End If
    End Sub


    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnprint.Click
        Dim worderId As String = cmbworder.SelectedItem.ToString()
        Dim roll As String = cmbroll.SelectedItem.ToString()

        Dim htmlContent As String = GeneratePrintContent(worderId, roll)
        If String.IsNullOrEmpty(htmlContent) Then
            MessageBox.Show("Error generating print content.")
            Return
        End If

        ' Create a temporary HTML file
        Dim tempFilePath As String = IO.Path.Combine(IO.Path.GetTempPath(), "print.html")
        IO.File.WriteAllText(tempFilePath, htmlContent)

        ' Open the HTML file in the default browser
        Process.Start(New ProcessStartInfo(tempFilePath) With {.UseShellExecute = True})
    End Sub

    Private Function GeneratePrintContent(worderId As String, roll As String) As String
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

        ' Generate barcode
        Dim barcodeWriter As New BarcodeWriter()
        barcodeWriter.Format = BarcodeFormat.CODE_128
        barcodeWriter.Options = New EncodingOptions With {
            .Width = 100,
            .Height = 60,
            .Margin = 10
        }

        ' Create barcode text with the exact work order ID and roll number
        Dim barcodeText As String = worderId & "*" & roll

        ' Save barcode to temporary file
        Dim barcodeBitmap As Bitmap = barcodeWriter.Write(barcodeText)
        Dim tempBarcodePath As String = Path.Combine(Path.GetTempPath(), $"barcode_{worderId}_{roll}.png")
        barcodeBitmap.Save(tempBarcodePath, ImageFormat.Png)

        ' Construct the full path to the image
        Dim imagePath As String = "\\180.1.1.6\new app\images\logo.jpg"
        If clientCode = "F00009" Then
            imagePath = "\\180.1.1.6\new app\images\f00009.jpg"
        End If

        ' Determine if the logo and defect rate should be included
        Dim includeLogo As Boolean = Not (clientCode = "F00010" Or clientCode = "F00046" Or clientCode = "F00054" Or clientCode = "F00059")
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

    Private Sub btnprint2_Click(sender As Object, e As EventArgs) Handles btnprint2.Click
        Dim worderId As String = cmbworder.SelectedItem.ToString()
        Dim htmlContent As String = GeneratePrintContentForAllRolls(worderId)
        If String.IsNullOrEmpty(htmlContent) Then
            MessageBox.Show("Error generating print content.")
            Return
        End If

        ' Create a temporary HTML file
        Dim tempFilePath As String = IO.Path.Combine(IO.Path.GetTempPath(), "print_all_rolls.html")
        IO.File.WriteAllText(tempFilePath, htmlContent)

        ' Check if the file was created successfully
        If Not IO.File.Exists(tempFilePath) Then
            MessageBox.Show("Error creating HTML file.")
            Return
        End If

        ' Open the HTML file in the default browser
        Try
            Process.Start(New ProcessStartInfo(tempFilePath) With {.UseShellExecute = True})
        Catch ex As Exception
            MessageBox.Show("Error opening HTML file: " & ex.Message)
        End Try
    End Sub

    Private Function GeneratePrintContentForAllRolls(worderId As String) As String
        ' Query to get the main data
        Dim mainDataQuery As String = "SELECT fi.worder_id, c.color, MAX(fi.date) AS last_date, c.material, cs.code AS client_code " &
                                  "FROM finish_inspect fi " &
                                  "LEFT JOIN techdata td ON fi.worder_id = td.worderid " &
                                  "LEFT JOIN Contracts c ON td.contract_id = c.ContractID " &
                                  "LEFT JOIN clients cs ON c.ClientCode = cs.id " &
                                  "WHERE fi.worder_id = @worderid " &
                                  "GROUP BY fi.worder_id, c.color, c.material, cs.code"

        ' Query to get the roll details
        Dim rollDetailsQuery As String = "SELECT fi.roll, fi.fabric_grade, fi.width, fi.height, fi.weight, " &
                                     "(SELECT (SUM(point) * 10000) / (fi.width * fi.height) FROM finish_inspect_defects WHERE worder_id = fi.worder_id AND roll = fi.roll) AS defectRate, " &
                                     "fi.notes, us.public_name " &
                                     "FROM finish_inspect fi " &
                                     "LEFT JOIN dep_users us ON fi.username = us.username " &
                                     "WHERE fi.worder_id = @worderid"

        ' Query to get the distinct defect names
        Dim defectNamesQuery As String = "SELECT DISTINCT gd.name_ar " &
                                     "FROM finish_inspect_defects fid " &
                                     "JOIN gray_defects gd ON fid.defect_id = gd.id " &
                                     "WHERE fid.worder_id = @worderid"

        ' Query to get the defect places for each roll
        Dim defectPlacesQuery As String = "SELECT fi.roll, gd.name_ar, fid.def_place " &
                                      "FROM finish_inspect_defects fid " &
                                      "JOIN gray_defects gd ON fid.defect_id = gd.id " &
                                      "JOIN finish_inspect fi ON fid.worder_id = fi.worder_id AND fid.roll = fi.roll " &
                                      "WHERE fid.worder_id = @worderid"

        Dim mainData As String = ""
        Dim rolls As New List(Of String)()
        Dim defectNames As New List(Of String)()
        Dim defectPlaces As New Dictionary(Of String, Dictionary(Of String, List(Of String)))()
        Dim totalHeight As Decimal = 0
        Dim grade2Height As Decimal = 0
        Dim totalPoints As Decimal = 0
        Dim rollCount As Integer = 0
        Dim totalWeight As Decimal = 0

        Using conn As New SqlConnection(sqlServerConnectionString)
            ' Get the main data
            Using cmd As New SqlCommand(mainDataQuery, conn)
                cmd.Parameters.AddWithValue("@worderid", worderId)
                Try
                    conn.Open()
                    Dim reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        Dim color As String = reader("color").ToString()
                        Dim lastDate As String = Convert.ToDateTime(reader("last_date")).ToString("yyyy-MM-dd")
                        Dim material As String = reader("material").ToString()
                        Dim clientCode As String = reader("client_code").ToString()

                        mainData = $"<tr>
                    <td style='border: 1px solid black;'>أمر شغل</td>
                    <td style='border: 1px solid black;'>{worderId}</td>
                </tr>
                <tr>
                    <td style='border: 1px solid black;'>اللون</td>
                    <td style='border: 1px solid black;'>{color}</td>
                </tr>
                <tr>
                    <td style='border: 1px solid black;'>أخر تاريخ فحص</td>
                    <td style='border: 1px solid black;'>{lastDate}</td>
                </tr>
                <tr>
                    <td style='border: 1px solid black;'>الخامة</td>
                    <td style='border: 1px solid black;'>{material}</td>
                </tr>
                <tr>
                    <td style='border: 1px solid black;'>كود العميل</td>
                    <td style='border: 1px solid black;'>{clientCode}</td>
                </tr>"
                    End If
                    reader.Close()
                Catch ex As Exception
                    MessageBox.Show("Error loading main data: " & ex.Message)
                    Return ""
                End Try
            End Using

            ' Get the roll details
            Using cmd As New SqlCommand(rollDetailsQuery, conn)
                cmd.Parameters.AddWithValue("@worderid", worderId)
                Try
                    Dim reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim roll As String = reader("roll").ToString()
                        Dim width As String = reader("width").ToString()
                        Dim height As Decimal = Convert.ToDecimal(reader("height"))
                        Dim weight As Decimal = Convert.ToDecimal(reader("weight"))
                        Dim defectRate As String = If(reader("defectRate") IsNot DBNull.Value, Convert.ToDecimal(reader("defectRate")).ToString("F2"), "N/A")
                        Dim notes As String = If(reader("notes") IsNot DBNull.Value, reader("notes").ToString(), "N/A")
                        Dim workerName As String = If(reader("public_name") IsNot DBNull.Value, reader("public_name").ToString(), "N/A")
                        Dim fabricGrade As String = If(reader("fabric_grade") IsNot DBNull.Value, reader("fabric_grade").ToString(), "N/A")

                        rolls.Add($"<tr>
                    <td style='border: 1px solid black;'>{roll}</td>
                    <td style='border: 1px solid black;'>{width}</td>
                    <td style='border: 1px solid black;'>{height}</td>
                    <td style='border: 1px solid black;'>{weight}</td>
                    <td style='border: 1px solid black;'>{defectRate}</td>
                    <td style='border: 1px solid black;'>{notes}</td>
                    <td style='border: 1px solid black; word-wrap: break-word; white-space: normal;'>{workerName}</td>
                    <td style='border: 1px solid black;'>{fabricGrade}</td>")

                        totalHeight += height
                        totalWeight += weight
                        rollCount += 1
                        If fabricGrade = "2" Then
                            grade2Height += height
                        End If
                    End While
                    reader.Close()
                Catch ex As Exception
                    MessageBox.Show("Error loading roll details: " & ex.Message)
                    Return ""
                End Try
            End Using

            ' Get the distinct defect names
            Using cmd As New SqlCommand(defectNamesQuery, conn)
                cmd.Parameters.AddWithValue("@worderid", worderId)
                Try
                    Dim reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        defectNames.Add(reader("name_ar").ToString())
                    End While
                    reader.Close()
                Catch ex As Exception
                    MessageBox.Show("Error loading defect names: " & ex.Message)
                    Return ""
                End Try
            End Using

            ' Get the defect places for each roll
            Using cmd As New SqlCommand(defectPlacesQuery, conn)
                cmd.Parameters.AddWithValue("@worderid", worderId)
                Try
                    Dim reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim roll As String = reader("roll").ToString()
                        Dim defectName As String = reader("name_ar").ToString()
                        Dim defPlace As String = reader("def_place").ToString()

                        If Not defectPlaces.ContainsKey(roll) Then
                            defectPlaces(roll) = New Dictionary(Of String, List(Of String))()
                        End If
                        If Not defectPlaces(roll).ContainsKey(defectName) Then
                            defectPlaces(roll)(defectName) = New List(Of String)()
                        End If
                        defectPlaces(roll)(defectName).Add(defPlace)
                    End While
                    reader.Close()
                Catch ex As Exception
                    MessageBox.Show("Error loading defect places: " & ex.Message)
                    Return ""
                End Try
            End Using

            ' Calculate total points
            Dim totalPointsQuery As String = "SELECT SUM(fid.point) * 10000 / (SUM(fi.height) * MAX(fi.width)) AS TotalPoints " &
                                         "FROM finish_inspect_defects fid " &
                                         "JOIN finish_inspect fi ON fid.worder_id = fi.worder_id AND fid.roll = fi.roll " &
                                         "WHERE fid.worder_id = @worderid"

            Using cmd As New SqlCommand(totalPointsQuery, conn)
                cmd.Parameters.AddWithValue("@worderid", worderId)
                Try
                    totalPoints = Convert.ToDecimal(cmd.ExecuteScalar())
                Catch ex As Exception
                    MessageBox.Show("Error calculating total points: " & ex.Message)
                    Return ""
                End Try
            End Using
        End Using

        ' Calculate the percentage of fabric_grade 2 height
        Dim grade2Percentage As Decimal = If(totalHeight > 0, (grade2Height / totalHeight) * 100, 0)

        ' Construct the HTML content
        Dim htmlContent As String = $"
<html>
<head>
    <style>
        table {{ width: 100%; border-collapse: collapse; font-size: 16px; font-weight: bold; }}
        table, th, td {{ border: 1px solid black; }}
        th, td {{ padding: 10px; text-align: left; }}
    </style>
</head>
<body>
    <center><h2>FINAL INSPECTION REPORT تقرير الفحص النهائى</h2></center>
    <center><h3>Quality Control Department</h3></center>
    <br>
    <table>
        {mainData}
        <tr>
            <td style='border: 1px solid black;'>نسبه الدرجه التانيه</td>
            <td style='border: 1px solid black;'>{grade2Percentage:F2}%</td>
        </tr>
        <tr>
            <td style='border: 1px solid black;'>Total Points</td>
            <td style='border: 1px solid black;'>{totalPoints:F2}</td>
        </tr>
        <tr>
            <td style='border: 1px solid black;'>اجمالى عدد الأتواب</td>
            <td style='border: 1px solid black;'>{rollCount}</td>
        </tr>
        <tr>
            <td style='border: 1px solid black;'>اجمالى الطول</td>
            <td style='border: 1px solid black;'>{totalHeight}</td>
        </tr>
        <tr>
            <td style='border: 1px solid black;'>اجمالى الوزن</td>
            <td style='border: 1px solid black;'>{totalWeight}</td>
        </tr>
    </table>
    <br>
    <center><h3>Roll Details</h3></center>
    <table>
        <tr>
            <th>Roll</th>
            <th>Width</th>
            <th>Height</th>
            <th>Weight</th>
            <th>Defect Rate</th>
            <th>Notes</th>
            <th>اسم العامل</th>
            <th>درجة التوب</th>"

        ' Add defect names as column headers
        For Each defectName In defectNames
            htmlContent &= $"<th>{defectName}</th>"
        Next

        htmlContent &= "</tr>"

        ' Add roll details and defect places
        For Each roll In rolls
            htmlContent &= roll

            ' Extract the roll number from the HTML row
            Dim rollNumber As String = roll.Split(">"c)(2).Split("<"c)(0)

            For Each defectName In defectNames
                Dim defPlace As String = If(defectPlaces.ContainsKey(rollNumber) AndAlso defectPlaces(rollNumber).ContainsKey(defectName), String.Join(" - ", defectPlaces(rollNumber)(defectName)), "")
                htmlContent &= $"<td style='border: 1px solid black;'>{defPlace}</td>"
            Next

            htmlContent &= "</tr>"
        Next

        htmlContent &= "
    </table>
    <script type='text/javascript'>
        window.onload = function() {{
            window.print();
        }};
    </script>
</body>
</html>"

        Return htmlContent
    End Function

    Private Sub btnprintall_Click(sender As Object, e As EventArgs) Handles btnprintall.Click
        Dim worderId As String = cmbworder.SelectedItem.ToString()

        ' Get all rolls for the selected work order and sort them
        Dim rolls As List(Of String) = GetRollsForWorkOrder(worderId)

        ' Generate HTML content for each roll and combine them
        Dim combinedHtmlContent As New StringBuilder()
        combinedHtmlContent.AppendLine("<!DOCTYPE html><html><head><style>")
        combinedHtmlContent.AppendLine("table { width: 100%; border-collapse: collapse; font-size: 16px; font-weight: bold; }")
        combinedHtmlContent.AppendLine("table, th, td { border: 1px solid black; }")
        combinedHtmlContent.AppendLine("th, td { padding: 10px; text-align: left; }")
        combinedHtmlContent.AppendLine("img { width: 50%; }")
        combinedHtmlContent.AppendLine(".barcode { text-align: center; margin-top: 10px; }")
        combinedHtmlContent.AppendLine(".barcode-text { font-size: 14px; margin-top: 5px; font-weight: bold; }")
        combinedHtmlContent.AppendLine(".barcode img { width: 70%; max-width: 300px; }")
        combinedHtmlContent.AppendLine("</style></head><body>")

        ' Create a temporary directory for barcodes with a unique name
        Dim tempDir As String = Path.Combine(Path.GetTempPath(), "barcodes_" & Guid.NewGuid().ToString())
        Try
            If Directory.Exists(tempDir) Then
                Directory.Delete(tempDir, True)
            End If
            Directory.CreateDirectory(tempDir)

            For Each roll As String In rolls
                ' Generate barcode for this specific roll
                Dim barcodeWriter As New BarcodeWriter()
                barcodeWriter.Format = BarcodeFormat.CODE_128
                barcodeWriter.Options = New EncodingOptions With {
                    .Width = 300,
                    .Height = 150,
                    .Margin = 10
                }

                ' Create unique barcode text for this roll
                Dim barcodeText As String = worderId & "*" & roll

                ' Generate and save the barcode
                Try
                    Using barcodeBitmap As Bitmap = barcodeWriter.Write(barcodeText)
                        ' Create a unique filename for this barcode
                        Dim tempBarcodePath As String = Path.Combine(tempDir, $"barcode_{worderId}_{roll}_{Guid.NewGuid()}.png")

                        ' Save the barcode with error handling
                        Try
                            barcodeBitmap.Save(tempBarcodePath, ImageFormat.Png)
                        Catch ex As Exception
                            MessageBox.Show($"Error saving barcode for roll {roll}: {ex.Message}")
                            Continue For
                        End Try

                        ' Generate HTML content for this roll
                        Dim htmlContent As String = GeneratePrintContent(worderId, roll)
                        If Not String.IsNullOrEmpty(htmlContent) Then
                            ' Extract only the body content of the HTML
                            Dim bodyStartIndex As Integer = htmlContent.IndexOf("<body>") + 6
                            Dim bodyEndIndex As Integer = htmlContent.IndexOf("</body>")
                            If bodyStartIndex > 0 AndAlso bodyEndIndex > bodyStartIndex Then
                                Dim bodyContent As String = htmlContent.Substring(bodyStartIndex, bodyEndIndex - bodyStartIndex)
                                ' Replace the barcode image path
                                bodyContent = bodyContent.Replace("barcode.png", Path.GetFileName(tempBarcodePath))
                                combinedHtmlContent.AppendLine(bodyContent)
                            End If
                        End If
                    End Using
                Catch ex As Exception
                    MessageBox.Show($"Error generating barcode for roll {roll}: {ex.Message}")
                    Continue For
                End Try
            Next

            combinedHtmlContent.AppendLine("</body></html>")

            If combinedHtmlContent.Length = 0 Then
                MessageBox.Show("Error generating print content.")
                Return
            End If

            ' Create a temporary HTML file
            Dim tempFilePath As String = Path.Combine(tempDir, "print_all_rolls.html")
            File.WriteAllText(tempFilePath, combinedHtmlContent.ToString())

            ' Open the HTML file in the default browser
            Try
                Process.Start(New ProcessStartInfo(tempFilePath) With {.UseShellExecute = True})
            Catch ex As Exception
                MessageBox.Show("Error opening HTML file: " & ex.Message)
            End Try

            ' Clean up temporary files after a delay
            Task.Delay(10000).ContinueWith(Sub()
                                               Try
                                                   If Directory.Exists(tempDir) Then
                                                       Directory.Delete(tempDir, True)
                                                   End If
                                               Catch ex As Exception
                                                   ' Ignore cleanup errors
                                               End Try
                                           End Sub)

        Catch ex As Exception
            MessageBox.Show("Error during print process: " & ex.Message)
            ' Try to clean up if there was an error
            Try
                If Directory.Exists(tempDir) Then
                    Directory.Delete(tempDir, True)
                End If
            Catch
                ' Ignore cleanup errors
            End Try
        End Try
    End Sub

    Private Function GetRollsForWorkOrder(worderId As String) As List(Of String)
        Dim rolls As New List(Of String)()
        Dim query As String = "SELECT roll FROM finish_inspect WHERE worder_id = @worderid"

        Using conn As New SqlConnection(sqlServerConnectionString)
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@worderid", worderId)
                Try
                    conn.Open()
                    Dim reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        rolls.Add(reader("roll").ToString())
                    End While
                    reader.Close()
                Catch ex As Exception
                    MessageBox.Show("Error loading rolls: " & ex.Message)
                End Try
            End Using
        End Using

        ' Sort rolls numerically
        rolls.Sort(Function(a, b)
                       Dim aNum As Integer
                       Dim bNum As Integer
                       ' Try to parse the roll numbers, if parsing fails, use 0
                       If Not Integer.TryParse(a, aNum) Then aNum = 0
                       If Not Integer.TryParse(b, bNum) Then bNum = 0
                       Return aNum.CompareTo(bNum)
                   End Function)

        Return rolls
    End Function

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
        LoadRolls(cmbworder.Text)
        ' SQL Query to sum height and weight for the selected Work Order
        Dim sumQuery As String = "SELECT SUM(height) AS totalHeight, SUM(weight) AS totalWeight " &
                             "FROM finish_inspect " &
                             "WHERE worder_id = @worderid"

        Using conn As New SqlConnection(sqlServerConnectionString)
            Using cmd As New SqlCommand(sumQuery, conn)
                cmd.Parameters.AddWithValue("@worderid", cmbworder.Text)
                Try
                    conn.Open()
                    Dim reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        Dim totalHeight As Decimal = If(reader("totalHeight") IsNot DBNull.Value, Convert.ToDecimal(reader("totalHeight")), 0)
                        Dim totalWeight As Decimal = If(reader("totalWeight") IsNot DBNull.Value, Convert.ToDecimal(reader("totalWeight")), 0)
                        lbltotalm.Text = " متر فحص: " & totalHeight.ToString()
                        lbltotalw.Text = " وزن فحص: " & totalWeight.ToString()

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

                        If totalWeight = 0 Then
                            lbltotalw.BackColor = Color.Red
                        ElseIf totalWeight < lowerBoundW OrElse totalWeight > upperBoundW Then
                            lbltotalw.BackColor = Color.Red
                        Else
                            lbltotalw.BackColor = Color.Green
                        End If
                    End If
                    reader.Close()
                Catch ex As Exception
                    MessageBox.Show("Error loading total height and weight: " & ex.Message)
                End Try
            End Using
        End Using
        ' Load the details into the DataGridView
        LoadDetailsIntoDataGridView(cmbworder.Text)
    End Sub
    Private Sub LoadDetailsIntoDataGridView(worderId As String)
        Dim query As String = "SELECT fi.date,fi.worder_id as 'رقم الأمر', c.material as 'الخامة', " &
                          "fi.roll as 'رقم التوب', fi.width as 'العرض', fi.height AS 'الطول', fi.weight as 'الوزن', fi.fabric_grade as 'الدرجة', us.user_ar as 'العامل',fi.elapsed_time,fi.notes," &
                          "STUFF((SELECT ' - ' + CAST(fid.point AS VARCHAR) " &
                          "FROM finish_inspect_defects fid " &
                          "WHERE fid.worder_id = fi.worder_id AND fid.roll = fi.roll " &
                          "FOR XML PATH('')), 1, 3, '') AS 'النقاط', " &
                          "STUFF((SELECT ' - ' + gd.name_ar " &
                          "FROM finish_inspect_defects fid " &
                          "LEFT JOIN gray_defects gd ON fid.defect_id = gd.id " &
                          "WHERE fid.worder_id = fi.worder_id AND fid.roll = fi.roll " &
                          "FOR XML PATH('')), 1, 3, '') AS 'العيوب' " &
                          "FROM finish_inspect fi " &
                          "LEFT JOIN techdata td ON fi.worder_id = td.worderid " &
                          "LEFT JOIN Contracts c ON td.contract_id = c.ContractID " &
                          "LEFT JOIN clients cs ON c.ClientCode = cs.id " &
                          "LEFT JOIN dep_users us ON fi.username = us.username " &
                          "WHERE fi.worder_id = @worderid"

        Using conn As New SqlConnection(sqlServerConnectionString)
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@worderid", worderId)
                Try
                    conn.Open()
                    Dim reader As SqlDataReader = cmd.ExecuteReader()
                    Dim dt As New DataTable()
                    dt.Load(reader)
                    dataGridViewDetails.DataSource = dt

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

                    ' Calculate the total points
                    Dim totalPointsQuery As String = "SELECT SUM(fid.point) * 10000 / (SUM(fi.height) * MAX(fi.width)) AS TotalPoints " &
                                                 "FROM finish_inspect_defects fid " &
                                                 "JOIN finish_inspect fi ON fid.worder_id = fi.worder_id AND fid.roll = fi.roll " &
                                                 "WHERE fid.worder_id = @worderid"

                    Using totalPointsCmd As New SqlCommand(totalPointsQuery, conn)
                        totalPointsCmd.Parameters.AddWithValue("@worderid", worderId)
                        Dim totalPoints As Object = totalPointsCmd.ExecuteScalar()
                        If totalPoints IsNot DBNull.Value Then
                            lbltotalpoints.Text = "Total Points: " & Convert.ToDecimal(totalPoints).ToString("F2")
                        Else
                            lbltotalpoints.Text = "Total Points: N/A"
                        End If
                    End Using

                Catch ex As Exception
                    MessageBox.Show("Error loading details: " & ex.Message)
                End Try
            End Using
        End Using
    End Sub




    Private Sub btnreport_Click(sender As Object, e As EventArgs) Handles btnreport.Click
        Dim htmlContent As String = GenerateReportContent()
        If String.IsNullOrEmpty(htmlContent) Then
            MessageBox.Show("Error generating report content.")
            Return
        End If

        ' Create a temporary HTML file
        Dim tempFilePath As String = IO.Path.Combine(IO.Path.GetTempPath(), "report.html")
        IO.File.WriteAllText(tempFilePath, htmlContent)

        ' Open the HTML file in the default browser
        Process.Start(New ProcessStartInfo(tempFilePath) With {.UseShellExecute = True})
    End Sub

    Private Function GenerateReportContent() As String
        Dim worderId As String = cmbworder.SelectedItem.ToString()
        Dim query As String = "SELECT worder_id, roll, date, width, height, weight, fabric_grade FROM finish_inspect WHERE worder_id = @worderid"

        Dim rows As New List(Of String)()
        Dim totalHeight As Decimal = 0
        Dim totalWeight As Decimal = 0
        Dim totalRolls As Integer = 0
        Dim totalHeightGrade1 As Decimal = 0
        Dim totalWeightGrade1 As Decimal = 0
        Dim totalRollsGrade1 As Integer = 0
        Dim totalHeightGrade2 As Decimal = 0
        Dim totalWeightGrade2 As Decimal = 0
        Dim totalRollsGrade2 As Integer = 0

        Using conn As New SqlConnection(sqlServerConnectionString)
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@worderid", worderId)
                Try
                    conn.Open()
                    Dim reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim worderIdValue As String = reader("worder_id").ToString()
                        Dim roll As String = reader("roll").ToString()
                        Dim dateValue As String = Convert.ToDateTime(reader("date")).ToString("yyyy-MM-dd")
                        Dim width As String = reader("width").ToString()
                        Dim height As Decimal = Convert.ToDecimal(reader("height"))
                        Dim weight As Decimal = Convert.ToDecimal(reader("weight"))
                        Dim fabricGrade As String = reader("fabric_grade").ToString()

                        rows.Add($"<tr>
                    <td style='border: 2px solid black;'>{worderIdValue}</td>
                    <td style='border: 2px solid black;'>{roll}</td>
                    <td style='border: 2px solid black;'>{dateValue}</td>
                    <td style='border: 2px solid black;'>{width}</td>
                    <td style='border: 2px solid black;'>{height}</td>
                    <td style='border: 2px solid black;'>{weight}</td>
                    <td style='border: 2px solid black;'>{fabricGrade}</td>
                </tr>")

                        totalHeight += height
                        totalWeight += weight
                        totalRolls += 1

                        If fabricGrade = "1" Then
                            totalHeightGrade1 += height
                            totalWeightGrade1 += weight
                            totalRollsGrade1 += 1
                        ElseIf fabricGrade = "2" Then
                            totalHeightGrade2 += height
                            totalWeightGrade2 += weight
                            totalRollsGrade2 += 1
                        End If
                    End While
                Catch ex As Exception
                    MessageBox.Show("Error loading report data: " & ex.Message)
                    Return ""
                End Try
            End Using
        End Using

        ' Calculate the percentage of grade 2 height
        Dim grade2Percentage As Decimal = If(totalHeight > 0, (totalHeightGrade2 / totalHeight) * 100, 0)

        Dim htmlContent As String = $"
<html>
<head>
    <style>
        table {{ width: 100%; border-collapse: collapse; text-align: center; font-size: 16px; font-weight: bold; }}
        table, th, td {{ border: 2px solid black; }}
        th, td {{ padding: 10px; }}
    </style>
</head>
<body>
    <table id='demo2_table' cellpadding='1' cellspacing='1' border='0' width='100%' align='center' dir='rtl'>
        <tr>
            <td><strong>Work Order ID رقم الأمر</strong></td>
            <td><strong>Roll No. رقم التوب</strong></td>
            <td><strong>Date التاريخ</strong></td>
            <td><strong>Width عرض التوب</strong></td>
            <td><strong>Height ارتفاع التوب</strong></td>
            <td><strong>Weight الوزن</strong></td>
            <td><strong>درجة القماش</strong></td>
        </tr>
        {String.Join(Environment.NewLine, rows)}
    </table>
    <br>
    <table id='summary_table' cellpadding='1' cellspacing='1' border='0' width='100%' align='center' dir='rtl'>
        <tr>
            <td><strong>اجمالى الطول</strong></td>
            <td>{totalHeight}</td>
        </tr>
        <tr>
            <td><strong>اجمالى الوزن</strong></td>
            <td>{totalWeight}</td>
        </tr>
        <tr>
            <td><strong>عدد الأتواب</strong></td>
            <td>{totalRolls}</td>
        </tr>
        <tr>
            <td><strong>اجمالى طول الدرجه الاولى</strong></td>
            <td>{totalHeightGrade1}</td>
        </tr>
        <tr>
            <td><strong>وزن الدرجه الاولى</strong></td>
            <td>{totalWeightGrade1}</td>
        </tr>
        <tr>
            <td><strong>عدد اتواب الدرجه الاولى</strong></td>
            <td>{totalRollsGrade1}</td>
        </tr>
        <tr>
            <td><strong>طول الدرجه التانيه</strong></td>
            <td>{totalHeightGrade2}</td>
        </tr>
        <tr>
            <td><strong>وزن الدرجه التانيه</strong></td>
            <td>{totalWeightGrade2}</td>
        </tr>
        <tr>
            <td><strong>عدد اتواب الدرجه التانيه</strong></td>
            <td>{totalRollsGrade2}</td>
        </tr>
        <tr>
            <td><strong>نسبة الدرجه التانيه</strong></td>
            <td>{grade2Percentage:F2}%</td>
        </tr>
    </table>
    <script type='text/javascript'>
        window.print();
    </script>
</body>
</html>"

        Return htmlContent
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

    Private Function HasMovePermission() As Boolean
        Try
            Using conn As New SqlConnection(sqlServerConnectionString)
                conn.Open()
                Dim query As String = "SELECT full_perm FROM dep_users WHERE username = @username"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@username", LoggedInUsername)
                    Dim result = cmd.ExecuteScalar()
                    Return result IsNot Nothing AndAlso Convert.ToInt32(result) = 1
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error checking permissions: " & ex.Message)
            Return False
        End Try
    End Function

    Private Sub BtnMoveSelected_Click(sender As Object, e As EventArgs)
        If Not HasMovePermission() Then
            MessageBox.Show("ليس لديك صلاحية لنقل البيانات")
            Return
        End If

        Dim selectedRolls As New List(Of (String, Integer))() ' (WorkOrder, Roll)

        For Each row As DataGridViewRow In dataGridViewDetails.Rows
            If row.Cells("SelectRow").Value = True Then
                Dim workOrder As String = row.Cells("رقم الأمر").Value.ToString()
                Dim roll As Integer = Convert.ToInt32(row.Cells("رقم التوب").Value)
                selectedRolls.Add((workOrder, roll))
            End If
        Next

        If selectedRolls.Count = 0 Then
            MessageBox.Show("الرجاء تحديد توب واحد على الأقل")
            Return
        End If

        Dim successCount As Integer = 0
        For Each item In selectedRolls
            If MoveInspectionData(item.Item1, item.Item2) Then
                successCount += 1
            End If
        Next

        MessageBox.Show($"تم نقل {successCount} من {selectedRolls.Count} توب بنجاح")
        RefreshData() ' Refresh the grid after moving
    End Sub

    Private Function MoveInspectionData(workOrder As String, roll As Integer) As Boolean
        Dim transaction As SqlTransaction = Nothing
        Dim conn As SqlConnection = Nothing
        Dim moveDefectsQuery As String = ""  ' Declare at function level

        Try
            conn = New SqlConnection(sqlServerConnectionString)
            conn.Open()
            transaction = conn.BeginTransaction()

            ' 1. Move roll data to del_finspect_rolls
            Dim moveRollQuery As String = "
                INSERT INTO del_finspect_rolls (
                    worder_id, roll, notes, date, weight, height, width, 
                    username, fabric_grade, ip_address, pc_name, elapsed_time, 
                    speed, links, techid, deleted_at, finspect_id, department
                )
                SELECT 
                    worder_id, roll, ISNULL(notes, ''), date, weight, height, width,
                    username, fabric_grade, ip_address, pc_name, elapsed_time,
                    speed, links, techid, GETDATE(), id, N'فحص نهائي'
                FROM finish_inspect 
                WHERE worder_id = @worder_id AND roll = @roll"

            Using cmd As New SqlCommand(moveRollQuery, conn, transaction)
                cmd.Parameters.AddWithValue("@worder_id", workOrder)
                cmd.Parameters.AddWithValue("@roll", roll)
                cmd.ExecuteNonQuery()
            End Using

            ' 2. Move defects data to del_finspect_defects
            moveDefectsQuery = "
                INSERT INTO del_finspect_defects (
                    worder_id, 
                    roll, 
                    notes, 
                    date, 
                    defect_id, 
                    def_place, 
                    point, 
                    department,
                    finspect_def_id
                )
                SELECT 
                    worder_id, 
                    roll, 
                    ISNULL(notes, '') as notes, 
                    date, 
                    defect_id, 
                    def_place,
                    point,
                    N'فحص نهائي' as department,
                    0 as finspect_def_id
                FROM finish_inspect_defects 
                WHERE worder_id = @worder_id AND roll = @roll"

            Using cmdDefects As New SqlCommand(moveDefectsQuery, conn, transaction)
                cmdDefects.Parameters.AddWithValue("@worder_id", workOrder)
                cmdDefects.Parameters.AddWithValue("@roll", roll)
                cmdDefects.ExecuteNonQuery()
            End Using

            ' 3. Delete original data
            Dim deleteDefectsQuery As String = "DELETE FROM finish_inspect_defects WHERE worder_id = @worder_id AND roll = @roll"
            Using cmd As New SqlCommand(deleteDefectsQuery, conn, transaction)
                cmd.Parameters.AddWithValue("@worder_id", workOrder)
                cmd.Parameters.AddWithValue("@roll", roll)
                cmd.ExecuteNonQuery()
            End Using

            Dim deleteRollQuery As String = "DELETE FROM finish_inspect WHERE worder_id = @worder_id AND roll = @roll"
            Using cmd As New SqlCommand(deleteRollQuery, conn, transaction)
                cmd.Parameters.AddWithValue("@worder_id", workOrder)
                cmd.Parameters.AddWithValue("@roll", roll)
                cmd.ExecuteNonQuery()
            End Using

            transaction.Commit()
            Return True

        Catch ex As Exception
            MessageBox.Show("تفاصيل الخطأ في نقل العيوب: " & ex.Message & vbCrLf & "SQL Query: " & moveDefectsQuery)
            If transaction IsNot Nothing Then
                Try
                    transaction.Rollback()
                Catch rollbackEx As Exception
                    MessageBox.Show("Error rolling back transaction: " & rollbackEx.Message)
                End Try
            End If
            Return False

        Finally
            If conn IsNot Nothing Then
                conn.Close()
            End If
        End Try
    End Function

    Private Sub RefreshData()
        ' Refresh the DataGridView with current data
        If cmbworder.SelectedItem IsNot Nothing Then
            ' Re-fetch and display data for the current work order
            ' (You'll need to implement this based on your existing data loading logic)
        End If
    End Sub

End Class