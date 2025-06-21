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
Public Class mainrowinspectform
    Private sqlServerConnectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"
    Private stopwatch As Stopwatch
    Private btnMoveSelected As Button ' Add button for moving selected items

    Private Sub mainrowinspectform_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
    End Sub

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
                    speed, links, techid, GETDATE(), id, N'فحص خام'
                FROM row_inspect 
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
                    N'فحص خام' as department,
                    0 as finspect_def_id
                FROM row_inspect_defects 
                WHERE worder_id = @worder_id AND roll = @roll"

            Using cmdDefects As New SqlCommand(moveDefectsQuery, conn, transaction)
                cmdDefects.Parameters.AddWithValue("@worder_id", workOrder)
                cmdDefects.Parameters.AddWithValue("@roll", roll)
                cmdDefects.ExecuteNonQuery()
            End Using

            ' 3. Delete original data
            Dim deleteDefectsQuery As String = "DELETE FROM row_inspect_defects WHERE worder_id = @worder_id AND roll = @roll"
            Using cmd As New SqlCommand(deleteDefectsQuery, conn, transaction)
                cmd.Parameters.AddWithValue("@worder_id", workOrder)
                cmd.Parameters.AddWithValue("@roll", roll)
                cmd.ExecuteNonQuery()
            End Using

            Dim deleteRollQuery As String = "DELETE FROM row_inspect WHERE worder_id = @worder_id AND roll = @roll"
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
            LoadDetailsIntoDataGridView(cmbworder.Text)
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


    Private Sub btnaddroll_Click(sender As Object, e As EventArgs) Handles btnaddroll.Click
        ' Get the selected worderId
        Dim selectedWorderId As String = cmbworder.Text

        ' Check if the selected worderId is in the list
        If cmbworder.Items.Contains(selectedWorderId) Then
            ' Hide the mainfinishinspectform
            Me.Hide()

            ' Create an instance of fetchrowform with the selected worderId and show it
            Dim fetchForm As New fetchrowform(selectedWorderId)
            fetchForm.Show()
        Else
            MessageBox.Show("Please select a valid Work Order ID from the list.")
        End If
    End Sub

    Private Sub btnrollnd_Click(sender As Object, e As EventArgs)
        ' Get the selected worderId
        Dim selectedWorderId As String = cmbworder.Text

        ' Check if the selected worderId is in the list
        If cmbworder.Items.Contains(selectedWorderId) Then
            ' Hide the mainfinishinspectform
            Me.Hide()

            ' Create an instance of fetchrowform with the selected worderId and show it
            Dim fetchForm As New fetchfinishlinksform(selectedWorderId)
            fetchForm.Show()
        Else
            MessageBox.Show("Please select a valid Work Order ID from the list.")
        End If
    End Sub




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
        Dim mainDataQuery As String = "SELECT fi.worder_id, c.color, MAX(fi.date) AS last_date, c.material " &
                                  "FROM row_inspect fi " &
                                  "LEFT JOIN techdata td ON fi.worder_id = td.worderid " &
                                  "LEFT JOIN Contracts c ON td.contract_id = c.ContractID " &
                                  "WHERE fi.worder_id = @worderid " &
                                  "GROUP BY fi.worder_id, c.color, c.material"

        ' Query to get the roll details
        Dim rollDetailsQuery As String = "SELECT fi.roll, fi.fabric_grade, fi.width, fi.height, " &
                                     "(SELECT (SUM(point) * 10000) / (fi.width * fi.height) FROM row_inspect_defects WHERE worder_id = fi.worder_id AND roll = fi.roll) AS defectRate, " &
                                     "fi.notes, us.public_name " &
                                     "FROM row_inspect fi " &
                                     "LEFT JOIN dep_users us ON fi.username = us.username " &
                                     "WHERE fi.worder_id = @worderid"

        ' Query to get the distinct defect names
        Dim defectNamesQuery As String = "SELECT DISTINCT gd.name_ar " &
                                     "FROM row_inspect_defects fid " &
                                     "JOIN gray_defects gd ON fid.defect_id = gd.id " &
                                     "WHERE fid.worder_id = @worderid"

        ' Query to get the defect places for each roll
        Dim defectPlacesQuery As String = "SELECT fi.roll, gd.name_ar, fid.def_place " &
                                      "FROM row_inspect_defects fid " &
                                      "JOIN gray_defects gd ON fid.defect_id = gd.id " &
                                      "JOIN row_inspect fi ON fid.worder_id = fi.worder_id AND fid.roll = fi.roll " &
                                      "WHERE fid.worder_id = @worderid"

        Dim mainData As String = ""
        Dim rolls As New List(Of String)()
        Dim defectNames As New List(Of String)()
        Dim defectPlaces As New Dictionary(Of String, Dictionary(Of String, String))()
        Dim totalHeight As Decimal = 0
        Dim grade2Height As Decimal = 0
        Dim totalPoints As Decimal = 0
        Dim rollCount As Integer = 0
        Dim totalDefectPoints As Decimal = 0

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

                        Dim defectRate As String = If(reader("defectRate") IsNot DBNull.Value, Convert.ToDecimal(reader("defectRate")).ToString("F2"), "N/A")
                        Dim notes As String = If(reader("notes") IsNot DBNull.Value, reader("notes").ToString(), "N/A")
                        Dim workerName As String = If(reader("public_name") IsNot DBNull.Value, reader("public_name").ToString(), "N/A")
                        Dim fabricGrade As String = If(reader("fabric_grade") IsNot DBNull.Value, reader("fabric_grade").ToString(), "N/A")

                        rolls.Add($"<tr>
                    <td style='border: 1px solid black;'>{roll}</td>
                    <td style='border: 1px solid black;'>{width}</td>
                    <td style='border: 1px solid black;'>{height}</td>
                    
                    <td style='border: 1px solid black;'>{defectRate}</td>
                    <td style='border: 1px solid black;'>{notes}</td>
                    <td style='border: 1px solid black; white-space: nowrap;'>{workerName}</td>
                    <td style='border: 1px solid black;'>{fabricGrade}</td>")

                        totalHeight += height

                        rollCount += 1
                        If fabricGrade = "2" Then
                            grade2Height += height
                        End If
                        ' Add defect points to total
                        If defectRate <> "N/A" Then
                            totalDefectPoints += Convert.ToDecimal(defectRate)
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
                            defectPlaces(roll) = New Dictionary(Of String, String)()
                        End If
                        defectPlaces(roll)(defectName) = defPlace
                    End While
                    reader.Close()
                Catch ex As Exception
                    MessageBox.Show("Error loading defect places: " & ex.Message)
                    Return ""
                End Try
            End Using

            ' Calculate total points
            Dim totalPointsQuery As String = "SELECT SUM(fid.point) * 10000 / (SUM(fi.height) * MAX(fi.width)) AS TotalPoints " &
                                         "FROM row_inspect_defects fid " &
                                         "JOIN row_inspect fi ON fid.worder_id = fi.worder_id AND fid.roll = fi.roll " &
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
    <center><h2>Raw INSPECTION REPORT تقرير فحص الخام</h2></center>
    <center><h3>Quality Control Department</h3></center>
<center><h3>Total Defect Points: {totalDefectPoints:F2}</h3></center>
    <br>
    <table>
        {mainData}
        
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
        
    </table>
    <br>
    <center><h3>Roll Details</h3></center>
    <table>
        <tr>
            <th>Roll Number</th>
            <th>Width</th>
            <th>Height</th>
            
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
                Dim defPlace As String = If(defectPlaces.ContainsKey(rollNumber) AndAlso defectPlaces(rollNumber).ContainsKey(defectName), defectPlaces(rollNumber)(defectName), "")
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
        Dim query As String = "SELECT ri.date,ri.worder_id as 'رقم الأمر', c.material as 'الخامة', " &
                          "ri.roll as 'رقم التوب', ri.width as 'العرض', ri.height AS 'الطول', ri.weight as 'الوزن', ri.fabric_grade as 'الدرجة', us.user_ar as 'العامل',ri.elapsed_time,ri.notes," &
                          "STUFF((SELECT ' - ' + CAST(rid.point AS VARCHAR) " &
                          "FROM row_inspect_defects rid " &
                          "WHERE rid.worder_id = ri.worder_id AND rid.roll = ri.roll " &
                          "FOR XML PATH('')), 1, 3, '') AS 'النقاط', " &
                          "STUFF((SELECT ' - ' + gd.name_ar " &
                          "FROM row_inspect_defects rid " &
                          "LEFT JOIN gray_defects gd ON rid.defect_id = gd.id " &
                          "WHERE rid.worder_id = ri.worder_id AND rid.roll = ri.roll " &
                          "FOR XML PATH('')), 1, 3, '') AS 'العيوب' " &
                          "FROM row_inspect ri " &
                          "LEFT JOIN techdata td ON ri.worder_id = td.worderid " &
                          "LEFT JOIN Contracts c ON td.contract_id = c.ContractID " &
                          "LEFT JOIN clients cs ON c.ClientCode = cs.id " &
                          "LEFT JOIN dep_users us ON ri.username = us.username " &
                          "WHERE ri.worder_id = @worderid"

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
                    Dim totalPointsQuery As String = "SELECT SUM(rid.point) * 10000 / (SUM(ri.height) * MAX(ri.width)) AS TotalPoints " &
                                                 "FROM row_inspect_defects rid " &
                                                 "JOIN row_inspect ri ON rid.worder_id = ri.worder_id AND rid.roll = ri.roll " &
                                                 "WHERE rid.worder_id = @worderid"

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
        Dim query As String = "SELECT worder_id, roll, date, width, height, fabric_grade FROM row_inspect WHERE worder_id = @worderid"

        Dim rows As New List(Of String)()
        Dim totalHeight As Decimal = 0

        Dim totalRolls As Integer = 0
        Dim totalHeightGrade1 As Decimal = 0

        Dim totalRollsGrade1 As Integer = 0
        Dim totalHeightGrade2 As Decimal = 0

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

                        Dim fabricGrade As String = reader("fabric_grade").ToString()

                        rows.Add($"<tr>
                    <td style='border: 2px solid black;'>{worderIdValue}</td>
                    <td style='border: 2px solid black;'>{roll}</td>
                    <td style='border: 2px solid black;'>{dateValue}</td>
                    <td style='border: 2px solid black;'>{width}</td>
                    <td style='border: 2px solid black;'>{height}</td>
                   
                    <td style='border: 2px solid black;'>{fabricGrade}</td>
                </tr>")

                        totalHeight += height

                        totalRolls += 1

                        If fabricGrade = "1" Then
                            totalHeightGrade1 += height

                            totalRollsGrade1 += 1
                        ElseIf fabricGrade = "2" Then
                            totalHeightGrade2 += height

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
            <td><strong>عدد الأتواب</strong></td>
            <td>{totalRolls}</td>
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

End Class