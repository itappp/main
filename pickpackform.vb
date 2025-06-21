Imports System.Data.SqlClient
Imports Excel = Microsoft.Office.Interop.Excel
Imports OfficeOpenXml
Imports System.IO
Imports ClosedXML.Excel


Public Class pickpackform
    Shared Sub New()
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial
    End Sub

    ' Corrected SQL Server connection string
    Private sqlServerConnectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"
    Private headerCheckbox As CheckBox = New CheckBox()

    Private Sub pickpackform_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Access the logged-in username from the global variable
        lblUsername.Text = "Logged in as: " & LoggedInUsername
        ' Retrieve the public_name for the logged-in user
        Dim publicName As String = GetPublicName(LoggedInUsername)

        LoadClientCodes()
        CalculateTotalHeight()
        CalculateTotalweight()
        LoadClientCodesto()
        cmpclientto.SelectedIndex = -1

        ' Add handlers for RowPostPaint events
        AddHandler dgvSummary.RowPostPaint, AddressOf dgvSummary_RowPostPaint
        AddHandler dgvall.RowPostPaint, AddressOf dgvall_RowPostPaint

        ApplyDataGridViewStyles()
    End Sub

    Private Sub pickpackform_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        ' No resize handling needed
    End Sub

    Private Sub CalculateSummaryForSelectedWorderIds()
        ' Create a DataTable to hold the summary data
        Dim summaryTable As New DataTable()
        summaryTable.Columns.Add("worder_id", GetType(String))
        summaryTable.Columns.Add("اجمالى طول", GetType(Double))
        summaryTable.Columns.Add("اجمالى وزن", GetType(Double))
        summaryTable.Columns.Add("الدرجه", GetType(String))
        summaryTable.Columns.Add("RollCount", GetType(Integer))

        ' Add checkbox column to dgvall if it doesn't exist
        If Not dgvall.Columns.Contains("select_checkbox") Then
            Dim checkboxColumn As New DataGridViewCheckBoxColumn()
            checkboxColumn.Name = "select_checkbox"
            checkboxColumn.HeaderText = "Select"
            checkboxColumn.Width = 50
            dgvall.Columns.Insert(0, checkboxColumn)
        End If
        ' Get the selected worder_ids from dgvresults
        Dim selectedWorderIds As New List(Of String)()
        For Each row As DataGridViewRow In dgvresults.Rows
            Dim worderId As String = row.Cells("worder_id").Value.ToString()
            If Not selectedWorderIds.Contains(worderId) Then
                selectedWorderIds.Add(worderId)
            End If
        Next

        ' Calculate the summary for each selected worder_id and fabric grade
        For Each worderId As String In selectedWorderIds
            Dim grades As New Dictionary(Of String, (TotalHeight As Double, TotalWeight As Double, RollCount As Integer))()

            For Each row As DataGridViewRow In dgvresults.Rows
                If row.Cells("worder_id").Value.ToString() = worderId Then

                    Dim height As Double = Convert.ToDouble(row.Cells("height").Value)
                    Dim weight As Double = Convert.ToDouble(row.Cells("weight").Value)
                    Dim fabricGrade As String = row.Cells("الدرجة").Value.ToString()

                    If Not grades.ContainsKey(fabricGrade) Then
                        grades(fabricGrade) = (0, 0, 0)
                    End If

                    grades(fabricGrade) = (grades(fabricGrade).TotalHeight + height, grades(fabricGrade).TotalWeight + weight, grades(fabricGrade).RollCount + 1)
                End If
            Next

            ' Add the summary data to the DataTable
            For Each grade In grades
                summaryTable.Rows.Add(worderId, grade.Value.TotalHeight, grade.Value.TotalWeight, grade.Key, grade.Value.RollCount)
            Next
        Next

        ' Bind the summary DataTable to the new DataGridView
        dgvall.DataSource = summaryTable

        ' Calculate and add the total summary row
        AddTotalSummaryRow(summaryTable)
    End Sub

    Private Sub AddTotalSummaryRow(ByVal summaryTable As DataTable)
        Dim totalHeight As Double = 0
        Dim totalWeight As Double = 0
        Dim totalRollCount As Integer = 0

        For Each row As DataRow In summaryTable.Rows
            totalHeight += Convert.ToDouble(row("اجمالى طول"))
            totalWeight += Convert.ToDouble(row("اجمالى وزن"))
            totalRollCount += Convert.ToInt32(row("RollCount"))
        Next

        ' Add the total summary row to the DataTable
        Dim totalRow As DataRow = summaryTable.NewRow()
        totalRow("worder_id") = "Total"

        totalRow("اجمالى طول") = totalHeight
        totalRow("اجمالى وزن") = totalWeight
        totalRow("الدرجه") = ""
        totalRow("RollCount") = totalRollCount
        summaryTable.Rows.Add(totalRow)
    End Sub

    Private Sub txtbarcode_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles txtbarcode.KeyPress
        ' Check if Enter key is pressed
        If e.KeyChar = ChrW(Keys.Enter) Then
            e.Handled = True ' Prevent the beep sound
            ProcessBarcode()
        End If
    End Sub
    Private Sub ProcessBarcode()
        Dim barcode As String = txtbarcode.Text.Trim()

        ' Play sound when barcode is scanned
        System.Media.SystemSounds.Beep.Play()

        ' Parse the barcode (format: workorder&tubenumber)
        Dim parts() As String = barcode.Split("*"c)
        If parts.Length <> 2 Then
            MessageBox.Show("صيغة الباركود غير صحيحة. يجب أن تكون بالشكل: أمر_الشغل*رقم_التوب")
            txtbarcode.Clear()
            Return
        End If

        Dim worderId As String = parts(0)
        Dim roll As String = parts(1)

        ' Build the search query to get all tubes for the work order
        Dim query As String = "SELECT id, worder_id, height as 'height', weight as 'weight', fabric_grade as 'الدرجة', roll, " &
                       "batch_no AS 'رقم الرسالة', " &
                       "client_code AS 'كود العميل',color AS 'اللون', product_name AS 'الخامة' " &
                       "FROM store_finish pf " &
                       "WHERE worder_id = @worderId AND roll = @roll"

        ' Execute the query and update dgvresults
        Using connection As New SqlConnection(sqlServerConnectionString)
            Try
                connection.Open()
                Using cmd As New SqlCommand(query, connection)
                    cmd.Parameters.AddWithValue("@worderId", worderId)
                    cmd.Parameters.AddWithValue("@roll", roll)

                    Dim adapter As New SqlDataAdapter(cmd)
                    Dim table As New DataTable()
                    adapter.Fill(table)

                    ' Bind the results to dgvresults
                    dgvresults.DataSource = table

                    ' Apply styles and update calculations
                    ApplyDataGridViewStyles()
                    CalculateSummaryForSelectedWorderIds()
                    CalculateTotalHeight()
                    CalculateTotalweight()
                End Using
            Catch ex As Exception
                MessageBox.Show("حدث خطأ أثناء البحث عن الباركود: " & ex.Message)
            End Try
        End Using

        ' Clear the barcode textbox for next scan
        txtbarcode.Clear()
    End Sub
    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        ' Validate controls initialization
        If dgvresults Is Nothing Then
            MessageBox.Show("DataGridView not initialized.")
            Return
        End If

        ' Get values from the textboxes
        Dim worderIdInput As String = txtworderid.Text
        Dim degree As String = txtdegree.Text.Trim() ' Get the degree from the new TextBox

        ' Check for missing work orders first
        If Not String.IsNullOrEmpty(worderIdInput) Then
            Dim inputWorderIds As String() = worderIdInput.Split(","c)
            Dim missingOrders As New List(Of String)

            Using connection As New SqlConnection(sqlServerConnectionString)
                Try
                    connection.Open()
                    For Each order As String In inputWorderIds
                        order = order.Trim()
                        Dim checkQuery As String = "SELECT COUNT(*) FROM store_finish WHERE worder_id = @worderId"
                        Using cmd As New SqlCommand(checkQuery, connection)
                            cmd.Parameters.AddWithValue("@worderId", order)
                            Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                            If count = 0 Then
                                missingOrders.Add(order)
                            End If
                        End Using
                    Next

                    If missingOrders.Count > 0 Then
                        Dim message As String = "أوامر الشغل التالية غير موجودة:" & vbCrLf & String.Join(vbCrLf, missingOrders)
                        MessageBox.Show(message, "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    End If
                Catch ex As Exception
                    MessageBox.Show("حدث خطأ أثناء التحقق من أوامر الشغل: " & ex.Message)
                End Try
            End Using
        End If

        Dim fromDate As DateTime = dtpfromdate.Value.Date
        Dim toDate As DateTime = dtptodate.Value.Date
        Dim clientCode As String = String.Empty
        Dim id As String = txtid.Text ' Get ID from txtid

        ' Initialize conditions list for dynamic WHERE clause
        Dim conditions As New List(Of String)

        ' Handle worderId input
        Dim searchWorderIds As String() = If(Not String.IsNullOrEmpty(worderIdInput), worderIdInput.Split(","c), New String() {})
        If searchWorderIds.Length > 0 Then
            Dim worderConditions As New List(Of String)
            For i As Integer = 0 To searchWorderIds.Length - 1
                worderConditions.Add("pf.worder_id = @worderId_" & i)
            Next
            conditions.Add("(" & String.Join(" OR ", worderConditions) & ")")
        End If

        ' Add condition for fabric grade/degree
        If Not String.IsNullOrEmpty(degree) Then
            conditions.Add("pf.fabric_grade = @degree")
        End If

        ' Handle clientCode input
        If Not String.IsNullOrEmpty(clientCode) Then
            conditions.Add("pf.client_code = @clientCode")
        End If

        ' Handle ID input (txtid) as comma-separated values
        Dim ids As String() = If(Not String.IsNullOrEmpty(id), id.Split(","c), New String() {})
        If ids.Length > 0 Then
            Dim idConditions As New List(Of String)
            For i As Integer = 0 To ids.Length - 1
                idConditions.Add("pf.id = @id_" & i)
            Next
            conditions.Add("(" & String.Join(" OR ", idConditions) & ")")
        End If

        ' Handle date range conditions
        If fromDate <> DateTime.MinValue AndAlso toDate <> DateTime.MinValue Then
            conditions.Add("CAST(pf.transaction_date AS DATE) BETWEEN @fromDate AND @toDate")
        ElseIf fromDate <> DateTime.MinValue Then
            conditions.Add("CAST(pf.transaction_date AS DATE) >= @fromDate")
        ElseIf toDate <> DateTime.MinValue Then
            conditions.Add("CAST(pf.transaction_date AS DATE) <= @toDate")
        End If

        ' Add condition to exclude rows where height is 0
        conditions.Add("NOT (pf.height = 0 AND pf.weight = 0)")

        ' Combine all conditions into the WHERE clause
        Dim whereClause As String = If(conditions.Count > 0, "WHERE " & String.Join(" AND ", conditions), "")

        ' SQL query with the dynamic WHERE clause
        Dim mainQuery As String = "SELECT id, worder_id,height as 'height', weight as 'weight', fabric_grade as 'الدرجة', roll, " &
                       "batch_no AS 'رقم الرسالة', " &
                       "client_code AS 'كود العميل',color AS 'اللون', product_name AS 'الخامة' " &
                       "FROM store_finish pf " & whereClause & " " &
                       "GROUP BY id, worder_id,height,weight, fabric_grade,roll, batch_no,client_code,color, product_name  "


        ' Pass parameters to LoadData
        LoadData(mainQuery, searchWorderIds, fromDate, toDate, ids, degree)

        ApplyDataGridViewStyles()
        CalculateTotalweight()
        CalculateTotalHeight()

        CalculateSummaryForSelectedWorderIds()
    End Sub
    Private Sub LoadData(ByVal query As String, ByVal worderIds As String(), ByVal fromDate As Date, ByVal toDate As Date, ByVal ids As String(), ByVal degree As String)
        Using connection As New SqlConnection(sqlServerConnectionString)
            Dim cmd As New SqlCommand(query, connection)

            ' Bind parameters for worderIds dynamically
            For i As Integer = 0 To worderIds.Length - 1
                cmd.Parameters.AddWithValue("@worderId_" & i, worderIds(i))
            Next

            ' Add parameter for degree
            If Not String.IsNullOrEmpty(degree) Then
                cmd.Parameters.AddWithValue("@degree", degree)
            End If

            ' Bind parameters for ids dynamically
            For i As Integer = 0 To ids.Length - 1
                cmd.Parameters.AddWithValue("@id_" & i, ids(i))
            Next

            If fromDate <> DateTime.MinValue Then
                cmd.Parameters.AddWithValue("@fromDate", fromDate)
            End If
            If toDate <> DateTime.MinValue Then
                cmd.Parameters.AddWithValue("@toDate", toDate)
            End If

            ' Execute command and populate dgvResults
            Try
                connection.Open()
                Dim adapter As New SqlDataAdapter(cmd)
                Dim table As New DataTable()
                adapter.Fill(table)
                dgvresults.DataSource = table

            Catch ex As Exception
                MessageBox.Show("An error occurred: " & ex.Message)
            End Try
        End Using
    End Sub

    Private selectAllCheckBox As CheckBox

    Private Sub ApplyDataGridViewStyles()
        ' Add checkbox column for selecting individual rows if it doesn't exist
        If Not dgvresults.Columns.Contains("select_checkbox") Then
            Dim checkboxColumn As New DataGridViewCheckBoxColumn()
            checkboxColumn.Name = "select_checkbox"
            checkboxColumn.HeaderText = "Select"
            checkboxColumn.Width = 50 ' Adjust width as needed
            dgvresults.Columns.Insert(0, checkboxColumn) ' Insert it at the first column position
        End If

        ' Add "Select All" checkbox to the header only if it doesn't already exist
        If selectAllCheckBox Is Nothing Then
            selectAllCheckBox = New CheckBox()
            selectAllCheckBox.Size = New Size(15, 15)
            Dim headerCellRect As Rectangle = dgvresults.GetCellDisplayRectangle(0, -1, True)
            selectAllCheckBox.Location = New Point(headerCellRect.Location.X + headerCellRect.Width / 2 - selectAllCheckBox.Width / 2, headerCellRect.Location.Y + 4)
            selectAllCheckBox.BackColor = Color.Transparent
            dgvresults.Controls.Add(selectAllCheckBox)

            ' Set font and alignment for the DataGridView content
            dgvresults.DefaultCellStyle.Font = New Font("Arial", 12, FontStyle.Bold)
            dgvresults.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            ' Set font and color for the DataGridView header
            dgvresults.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 12, FontStyle.Bold)
            dgvresults.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue
            dgvresults.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black
            dgvresults.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            ' Apply header and content styles
            dgvresults.EnableHeadersVisualStyles = False ' Allow custom header styles to take effect

            ' Add the "Select All" checkbox functionality for the header click
            AddHandler selectAllCheckBox.CheckedChanged, AddressOf SelectAllCheckBox_CheckedChanged
        End If
        ' Set the width for other columns (as per your previous code)
        If dgvresults.Columns.Contains("id") Then
            dgvresults.Columns("id").Width = 20
        End If
    End Sub
    Private Sub SelectAllCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim isChecked As Boolean = selectAllCheckBox.Checked

        ' Loop through each row and set the checkbox value based on "Select All" checkbox state
        For Each row As DataGridViewRow In dgvresults.Rows
            row.Cells("select_checkbox").Value = isChecked
        Next
        dgvresults.RefreshEdit()
    End Sub

    Private Sub UpdateSummaryDataGridView()
        ' Create a DataTable to hold the summary data
        Dim summaryTable As New DataTable()
        summaryTable.Columns.Add("worder_id", GetType(String))
        summaryTable.Columns.Add("اجمالى طول", GetType(Double))
        summaryTable.Columns.Add("اجمالى وزن", GetType(Double))
        summaryTable.Columns.Add("الدرجة", GetType(String))
        summaryTable.Columns.Add("RollCount", GetType(Integer))

        ' Get the selected worder_ids from dgvresults
        Dim selectedWorderIds As New List(Of String)()
        For Each row As DataGridViewRow In dgvresults.Rows
            Dim checkBoxCell As DataGridViewCheckBoxCell = TryCast(row.Cells("select_checkbox"), DataGridViewCheckBoxCell)
            If checkBoxCell IsNot Nothing AndAlso CBool(checkBoxCell.Value) Then
                Dim worderId As String = row.Cells("worder_id").Value.ToString()
                If Not selectedWorderIds.Contains(worderId) Then
                    selectedWorderIds.Add(worderId)
                End If
            End If
        Next

        ' Calculate the summary for each selected worder_id and fabric grade
        For Each worderId As String In selectedWorderIds
            Dim grades As New Dictionary(Of String, (TotalHeight As Double, TotalWeight As Double, RollCount As Integer))()

            For Each row As DataGridViewRow In dgvresults.Rows
                Dim checkBoxCell As DataGridViewCheckBoxCell = TryCast(row.Cells("select_checkbox"), DataGridViewCheckBoxCell)
                If checkBoxCell IsNot Nothing AndAlso CBool(checkBoxCell.Value) AndAlso row.Cells("worder_id").Value.ToString() = worderId Then
                    Dim height As Double = Convert.ToDouble(row.Cells("height").Value)
                    Dim weight As Double = Convert.ToDouble(row.Cells("weight").Value)
                    Dim fabricGrade As String = row.Cells("الدرجة").Value.ToString()

                    If Not grades.ContainsKey(fabricGrade) Then
                        grades(fabricGrade) = (0, 0, 0)
                    End If

                    grades(fabricGrade) = (grades(fabricGrade).TotalHeight + height, grades(fabricGrade).TotalWeight + weight, grades(fabricGrade).RollCount + 1)
                End If
            Next

            ' Add the summary data to the DataTable
            For Each grade In grades
                summaryTable.Rows.Add(worderId, grade.Value.TotalHeight, grade.Value.TotalWeight, grade.Key, grade.Value.RollCount)
            Next
        Next

        ' Calculate the total summary
        Dim totalHeight As Double = 0
        Dim totalWeight As Double = 0
        Dim totalRollCount As Integer = 0

        For Each row As DataRow In summaryTable.Rows
            totalHeight += Convert.ToDouble(row("اجمالى طول"))
            totalWeight += Convert.ToDouble(row("اجمالى وزن"))
            totalRollCount += Convert.ToInt32(row("RollCount"))
        Next

        ' Add the total summary row to the DataTable
        Dim totalRow As DataRow = summaryTable.NewRow()
        totalRow("worder_id") = "Total"
        totalRow("اجمالى طول") = totalHeight
        totalRow("اجمالى وزن") = totalWeight
        totalRow("الدرجة") = ""
        totalRow("RollCount") = totalRollCount
        summaryTable.Rows.Add(totalRow)

        ' Bind the summary DataTable to the new DataGridView
        dgvSummary.DataSource = summaryTable
    End Sub

    Private Sub dgvresults_CellValueChanged(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles dgvresults.CellValueChanged
        ' Check if the changed cell is in the checkbox column
        If e.ColumnIndex = dgvresults.Columns("select_checkbox").Index Then
            UpdateSummaryDataGridView()
        End If
    End Sub

    Private Sub dgvresults_CurrentCellDirtyStateChanged(ByVal sender As Object, ByVal e As EventArgs) Handles dgvresults.CurrentCellDirtyStateChanged
        ' Commit the checkbox value change immediately
        If dgvresults.IsCurrentCellDirty AndAlso dgvresults.CurrentCell.OwningColumn.Name = "select_checkbox" Then
            dgvresults.CommitEdit(DataGridViewDataErrorContexts.Commit)
        End If
    End Sub

    Private Sub dgvSummary_RowPostPaint(ByVal sender As Object, ByVal e As DataGridViewRowPostPaintEventArgs) Handles dgvSummary.RowPostPaint
        ' Check if the current row is the last row
        If e.RowIndex = dgvSummary.Rows.Count - 1 Then
            dgvSummary.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.LightYellow
            dgvSummary.Rows(e.RowIndex).DefaultCellStyle.Font = New Font(dgvSummary.DefaultCellStyle.Font, FontStyle.Bold)
        End If
    End Sub
    Private Sub dgvall_RowPostPaint(ByVal sender As Object, ByVal e As DataGridViewRowPostPaintEventArgs) Handles dgvall.RowPostPaint
        ' Check if the current row is the last row
        If e.RowIndex = dgvall.Rows.Count - 1 Then
            dgvall.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.LightYellow
            dgvall.Rows(e.RowIndex).DefaultCellStyle.Font = New Font(dgvall.DefaultCellStyle.Font, FontStyle.Bold)
        End If
    End Sub
    Private Sub LoadClientCodes()
        Using connection As New SqlConnection(sqlServerConnectionString)
            Dim query As String = "SELECT DISTINCT client_code FROM store_finish"
            Dim cmd As New SqlCommand(query, connection)

            Try
                connection.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                While reader.Read()

                End While
                reader.Close()
            Catch ex As Exception
                MessageBox.Show("An error occurred while loading client codes: " & ex.Message)
            End Try
        End Using
    End Sub
    Private Sub CalculateTotalHeight()
        Using connection As New SqlConnection(sqlServerConnectionString)
            Dim query As String = "SELECT SUM(CAST(height AS DECIMAL(18,2))) FROM store_finish"
            Dim cmd As New SqlCommand(query, connection)

            Try
                connection.Open()
                Dim heightSum As Object = cmd.ExecuteScalar()
                ' Check if the result is DBNull or Nothing and display the result accordingly
                If IsDBNull(heightSum) OrElse heightSum Is Nothing Then
                    lbltotalstock.Text = "إجمالى رصيد المخزن: 0"
                Else
                    lbltotalstock.Text = "  رصيد المخزن متر : " & Convert.ToDecimal(heightSum).ToString("N2") ' Format to two decimal places
                End If
            Catch ex As Exception
                MessageBox.Show("An error occurred while calculating total height: " & ex.Message)
            End Try
        End Using
    End Sub
    Private Sub CalculateTotalweight()
        Using connection As New SqlConnection(sqlServerConnectionString)
            Dim query As String = "SELECT SUM(CAST(weight AS DECIMAL(18,2))) FROM store_finish"
            Dim cmd As New SqlCommand(query, connection)

            Try
                connection.Open()
                Dim weightSum As Object = cmd.ExecuteScalar()
                ' Check if the result is DBNull or Nothing and display the result accordingly
                If IsDBNull(weightSum) OrElse weightSum Is Nothing Then
                    lbltotalstockw.Text = "إجمالى رصيد المخزن: 0"
                Else
                    lbltotalstockw.Text = "  رصيد المخزن وزن : " & Convert.ToDecimal(weightSum).ToString("N2") ' Format to two decimal places
                End If
            Catch ex As Exception
                MessageBox.Show("An error occurred while calculating total height: " & ex.Message)
            End Try
        End Using
    End Sub

    ' Event handler for View button click
    Private Sub btnView_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnview.Click
        ' Navigate to the ContractViewForm
        Dim viewForm As New packingviewform()
        viewForm.Show()
    End Sub
    Private Sub btnInsert_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnInsert.Click
        ' Validate that a client is selected in cmpclientto
        If cmpclientto.SelectedValue Is Nothing Then
            MessageBox.Show("Please select a client before proceeding.")
            Return
        End If

        Dim selectedClientId As Integer = Convert.ToInt32(cmpclientto.SelectedValue)
        Dim checkedRowsCount As Integer = 0 ' To keep track of how many rows were processed
        Dim isMessageDisplayed As Boolean = False ' Flag to track if the message "كمية الصرف أكبر من كميه المخزون" has been shown
        Dim isStoreFinishedMessageDisplayed As Boolean = False ' Flag for store_finish message
        Dim newRefPacking As Integer = 0 ' Variable to store the incremented ref_packing
        Dim canClearDataSource As Boolean = True ' Flag to control clearing of DataGridView

        ' Retrieve the next ref_packing value only once before processing any rows
        Using connection As New SqlConnection(sqlServerConnectionString)
            Try
                connection.Open()

                Dim getMaxQuery As String = "SELECT ISNULL(MAX(ref_packing), 0) FROM packing"
                Using getMaxCmd As New SqlCommand(getMaxQuery, connection)
                    Dim result = getMaxCmd.ExecuteScalar()
                    If result IsNot Nothing AndAlso IsNumeric(result) Then
                        newRefPacking = Convert.ToInt32(result) + 1
                    End If
                End Using
            Catch ex As Exception
                MessageBox.Show("Error retrieving ref_packing: " & ex.Message)
                Return
            End Try
        End Using

        ' Iterate through all rows in the DataGridView
        For Each row As DataGridViewRow In dgvresults.Rows
            Dim checkBoxCell As DataGridViewCheckBoxCell = TryCast(row.Cells("select_checkbox"), DataGridViewCheckBoxCell)
            If checkBoxCell IsNot Nothing AndAlso CBool(checkBoxCell.Value) Then
                checkedRowsCount += 1
                Dim selectedId As Integer
                If Not Integer.TryParse(row.Cells("id").Value.ToString(), selectedId) Then
                    MessageBox.Show("Invalid ID in one of the selected rows.")
                    Continue For
                End If

                Dim insertedHeight As Decimal
                Dim insertedWeight As Decimal
                Try
                    insertedHeight = Convert.ToDecimal(row.Cells("height").Value)
                    insertedWeight = Convert.ToDecimal(row.Cells("weight").Value)
                Catch ex As Exception
                    MessageBox.Show("Invalid height or weight format: " & ex.Message)
                    Continue For
                End Try

                ' Validate against the store_finish values
                Dim isValid As Boolean = ValidateAgainstStoreFinish(selectedId, insertedHeight, insertedWeight)
                If Not isValid Then
                    MessageBox.Show("كمية الصرف أكبر من كميه المخزون")
                    canClearDataSource = False ' Do not clear DataGridView if this condition occurs
                    Continue For
                End If

                ' Insert into packing
                Dim username As String = LoggedInUsername
                InsertIntoPacking(selectedId, insertedHeight, insertedWeight, username, newRefPacking, selectedClientId, isMessageDisplayed)

                ' Update store_finish
                Using connection As New SqlConnection(sqlServerConnectionString)
                    Try
                        connection.Open()
                        UpdateStoreFinished(connection, selectedId, insertedHeight, insertedWeight, isStoreFinishedMessageDisplayed)
                    Catch ex As Exception
                        MessageBox.Show("Error updating store_finish: " & ex.Message)
                    End Try
                End Using
            End If
        Next

        ' Post-processing
        If checkedRowsCount = 0 Then
            MessageBox.Show("No rows selected for insertion.")
        Else
            ' Only clear DataGridView if the flag allows
            If canClearDataSource Then
                dgvresults.DataSource = Nothing
            End If


            CalculateTotalHeight()
            CalculateTotalweight()
            cmpclientto.SelectedIndex = -1


        End If
    End Sub

    Private Sub InsertIntoPacking(ByVal id As Integer, ByVal height As Decimal, ByVal weight As Decimal, ByVal username As String, ByVal refPacking As Integer, ByVal clientId As Integer, ByRef isMessageDisplayed As Boolean)
        Using connection As New SqlConnection(sqlServerConnectionString)
            Try
                connection.Open()

                Dim query As String = "INSERT INTO packing (storefinishid, weight, height, username, date, ref_packing, toclient) VALUES (@id, @weight, @height, @username, @date, @ref_packing, @toclient)"
                Using cmd As New SqlCommand(query, connection)
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id
                    cmd.Parameters.Add("@weight", SqlDbType.Decimal).Value = weight
                    cmd.Parameters.Add("@height", SqlDbType.Decimal).Value = height
                    cmd.Parameters.Add("@username", SqlDbType.NVarChar).Value = username
                    cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = DateTime.Now
                    cmd.Parameters.Add("@ref_packing", SqlDbType.Int).Value = refPacking
                    cmd.Parameters.Add("@toclient", SqlDbType.Int).Value = clientId

                    cmd.ExecuteNonQuery()

                    If Not isMessageDisplayed Then
                        MessageBox.Show("Record inserted into packing successfully with ref_packing: " & refPacking)
                        isMessageDisplayed = True
                    End If
                End Using
            Catch ex As SqlException
                MessageBox.Show("An error occurred while inserting into packing (SQL error): " & ex.Message)
            Catch ex As Exception
                MessageBox.Show("An unexpected error occurred while inserting into packing: " & ex.Message)
            Finally
                connection.Close()
            End Try
        End Using
    End Sub
    Private Sub UpdateStoreFinished(ByVal connection As SqlConnection, ByVal id As Integer, ByVal insertedHeight As Decimal, ByVal insertedWeight As Decimal, ByRef isStoreFinishedMessageDisplayed As Boolean)
        Dim query As String = "UPDATE store_finish SET height = height - @insertedHeight, weight = weight - @insertedWeight WHERE id = @id"

        Using cmd As New SqlCommand(query, connection)
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id
            cmd.Parameters.Add("@insertedHeight", SqlDbType.Decimal).Value = insertedHeight
            cmd.Parameters.Add("@insertedWeight", SqlDbType.Decimal).Value = insertedWeight

            Try
                Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
                If rowsAffected > 0 AndAlso Not isStoreFinishedMessageDisplayed Then
                    MessageBox.Show("Store finished table updated successfully!")
                    isStoreFinishedMessageDisplayed = True
                ElseIf rowsAffected = 0 Then
                    MessageBox.Show("No matching records found to update.")
                End If
            Catch ex As Exception
                MessageBox.Show("Error updating store_finish: " & ex.Message)
            End Try
        End Using
    End Sub
    Private Function ValidateAgainstStoreFinish(ByVal id As Integer, ByVal insertedHeight As Decimal, ByVal insertedWeight As Decimal) As Boolean
        Dim isValid As Boolean = True
        Using connection As New SqlConnection(sqlServerConnectionString)
            Try
                connection.Open()
                Dim query As String = "SELECT height, weight FROM store_finish WHERE id = @id"
                Using cmd As New SqlCommand(query, connection)
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            Dim availableHeight As Decimal = reader.GetDecimal(0)
                            Dim availableWeight As Decimal = reader.GetDecimal(1)

                            If insertedHeight > availableHeight OrElse insertedWeight > availableWeight Then
                                isValid = False
                            End If
                        End If
                    End Using
                End Using
            Catch ex As Exception
                MessageBox.Show("Error validating store_finish: " & ex.Message)
                isValid = False
            Finally
                connection.Close()
            End Try
        End Using
        Return isValid
    End Function
    Private Function GetSelectedId() As Integer
        ' Add logic to retrieve the selected ID from the DataGridView
        ' This function assumes that the first column (id) is the one selected in the DataGridView
        If dgvresults.SelectedRows.Count > 0 Then
            Return Convert.ToInt32(dgvresults.SelectedRows(0).Cells("id").Value)
        Else
            MessageBox.Show("Please select a row to insert.")
            Return -1
        End If
    End Function

    Private Sub LoadClientCodesto()
        Using connection As New SqlConnection(sqlServerConnectionString)
            Dim query As String = "SELECT id, code FROM Clients"
            Dim cmd As New SqlCommand(query, connection)

            Try
                connection.Open()
                Dim dt As New DataTable()
                Dim adapter As New SqlDataAdapter(cmd)
                adapter.Fill(dt)

                ' Bind the ComboBox
                cmpclientto.DataSource = dt
                cmpclientto.DisplayMember = "code" ' Display the code in the dropdown
                cmpclientto.ValueMember = "id"    ' Store the id as the value
            Catch ex As Exception
                MessageBox.Show("An error occurred while loading client codes: " & ex.Message)
            End Try
        End Using
    End Sub
    Private isStoreFinishedMessageDisplayed As Boolean = False
    Private Function AreHeightAndWeightValid(ByVal selectedId As Integer) As Boolean
        ' Check if the height and weight are zero in the store_finish table
        Dim height As Decimal
        Dim weight As Decimal

        ' Query to get height and weight from store_finish based on the selected ID
        Using connection As New SqlConnection(sqlServerConnectionString)
            Dim query As String = "SELECT height, weight FROM store_finish WHERE id = @selectedId"
            Dim cmd As New SqlCommand(query, connection)
            cmd.Parameters.AddWithValue("@selectedId", selectedId)

            Try
                connection.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                If reader.HasRows Then
                    reader.Read()
                    height = Convert.ToDecimal(reader("height"))
                    weight = Convert.ToDecimal(reader("weight"))

                    ' Check if either height or weight is zero
                    If height = 0 Or weight = 0 Then
                        Return False ' Invalid, don't proceed with the insert/update
                    End If
                Else
                    Return False ' No data found for the selected ID
                End If
            Catch ex As Exception
                MessageBox.Show("An error occurred while validating height and weight: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End Try
        End Using

        Return True ' Height and weight are valid (not zero)
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
    Private Sub LogActivity(ByVal action As String, ByVal username As String, ByVal selectedId As Integer)
        ' Create a log message that combines the action and other relevant details
        Dim logMessage As String = String.Format("User '{0}'  action: {1} on record with ID {2} at {3}",
                                                  username, action, selectedId, DateTime.Now)

        ' Define the SQL query to insert the activity log
        Dim query As String = "INSERT INTO activity_logs (log_message) VALUES (@logMessage)"

        Using connection As New SqlConnection(sqlServerConnectionString)
            Using command As New SqlCommand(query, connection)
                ' Add parameter to prevent SQL injection
                command.Parameters.AddWithValue("@logMessage", logMessage)

                Try
                    connection.Open()
                    command.ExecuteNonQuery() ' Execute the insert command
                Catch ex As Exception
                    MessageBox.Show("Error logging activity: " & ex.Message)
                End Try
            End Using
        End Using
    End Sub

    Private Sub dgvSummary_CellClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles dgvSummary.CellClick
        ' Skip if clicking on header or empty row
        If e.RowIndex < 0 OrElse e.RowIndex >= dgvSummary.Rows.Count Then Return

        ' Get the worder_id from the clicked row
        Dim selectedWorderId As String = dgvSummary.Rows(e.RowIndex).Cells("worder_id").Value.ToString()

        ' Skip if it's the total row
        If selectedWorderId = "Total" Then Return

        ' Uncheck all rows first
        For Each row As DataGridViewRow In dgvresults.Rows
            row.Cells("select_checkbox").Value = False
        Next

        ' Check all rows that match the selected worder_id
        For Each row As DataGridViewRow In dgvresults.Rows
            If row.Cells("worder_id").Value.ToString() = selectedWorderId Then
                row.Cells("select_checkbox").Value = True
            End If
        Next

        ' Update the summary display
        UpdateSummaryDataGridView()
    End Sub

    Private Sub dgvall_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgvall.CellValueChanged
        If e.ColumnIndex = dgvall.Columns("select_checkbox").Index AndAlso e.RowIndex >= 0 Then
            ' Skip if it's the total row
            If dgvall.Rows(e.RowIndex).Cells("worder_id").Value.ToString() = "Total" Then
                Return
            End If

            ' Get the checkbox state and row information
            Dim isChecked As Boolean = If(dgvall.Rows(e.RowIndex).Cells("select_checkbox").Value Is Nothing,
                                        False,
                                        Convert.ToBoolean(dgvall.Rows(e.RowIndex).Cells("select_checkbox").Value))
            Dim selectedWorderId As String = dgvall.Rows(e.RowIndex).Cells("worder_id").Value.ToString()
            Dim selectedGrade As String = dgvall.Rows(e.RowIndex).Cells("الدرجه").Value.ToString()

            ' Update checkboxes in dgvresults
            For Each row As DataGridViewRow In dgvresults.Rows
                If row.Cells("worder_id").Value.ToString() = selectedWorderId AndAlso
                   row.Cells("الدرجة").Value.ToString() = selectedGrade Then
                    row.Cells("select_checkbox").Value = isChecked
                End If
            Next

            ' Update the summary display
            UpdateSummaryDataGridView()
        End If
    End Sub

    Private Sub dgvall_CurrentCellDirtyStateChanged(sender As Object, e As EventArgs) Handles dgvall.CurrentCellDirtyStateChanged
        If dgvall.IsCurrentCellDirty AndAlso
           dgvall.CurrentCell.OwningColumn.Name = "select_checkbox" Then
            dgvall.CommitEdit(DataGridViewDataErrorContexts.Commit)
        End If
    End Sub

    ' Add new button for Excel upload
    Private Sub btnUploadExcel_Click(sender As Object, e As EventArgs) Handles btnUploadExcel.Click
        Using ofd As New OpenFileDialog()
            ofd.Filter = "Excel Files|*.xlsx;*.xls"
            ofd.Title = "Select Excel File"

            If ofd.ShowDialog() = DialogResult.OK Then
                Try
                    SearchByExcelFile(ofd.FileName)
                Catch ex As Exception
                    MessageBox.Show("Error processing Excel file: " & ex.Message)
                End Try
            End If
        End Using
    End Sub

    Private Sub SearchByExcelFile(filePath As String)
        Try
            Using package As New ExcelPackage(New FileInfo(filePath))
                Dim worksheet = package.Workbook.Worksheets(0) ' First worksheet
                Dim rowCount = worksheet.Dimension.Rows

                ' Create lists to store search criteria with order
                Dim searchCriteria As New List(Of (Order As Integer, WOrderId As String, RollNo As String))

                ' Read Excel data (assuming first column is worder_id and second column is roll)
                For row As Integer = 2 To rowCount ' Start from row 2 assuming row 1 is header
                    Dim worderId As String = worksheet.Cells(row, 1).Value?.ToString()
                    Dim rollNo As String = worksheet.Cells(row, 2).Value?.ToString()

                    If Not String.IsNullOrEmpty(worderId) Then
                        ' Store the original order with the data
                        searchCriteria.Add((row - 2, worderId, rollNo))
                    End If
                Next

                ' Perform search based on Excel data
                SearchByExcelCriteria(searchCriteria)
            End Using
        Catch ex As Exception
            MessageBox.Show("Error reading Excel file: " & ex.Message)
        End Try
    End Sub

    Private Sub SearchByExcelCriteria(searchCriteria As List(Of (Order As Integer, WOrderId As String, RollNo As String)))
        If searchCriteria.Count = 0 Then
            MessageBox.Show("No search criteria found in Excel file.")
            Return
        End If

        Using connection As New SqlConnection(sqlServerConnectionString)
            Try
                connection.Open()

                ' Create a temporary table to store results with order
                Dim resultTable As New DataTable()
                resultTable.Columns.Add("OrderIndex", GetType(Integer))
                resultTable.Columns.Add("id", GetType(Integer))
                resultTable.Columns.Add("worder_id", GetType(String))
                resultTable.Columns.Add("height", GetType(Decimal))
                resultTable.Columns.Add("weight", GetType(Decimal))
                resultTable.Columns.Add("الدرجة", GetType(String))
                resultTable.Columns.Add("roll", GetType(String))
                resultTable.Columns.Add("رقم الرسالة", GetType(String))
                resultTable.Columns.Add("كود العميل", GetType(String))
                resultTable.Columns.Add("اللون", GetType(String))
                resultTable.Columns.Add("الخامة", GetType(String))

                ' Process each search criteria and maintain order
                For Each criteria In searchCriteria
                    Dim query As String = "SELECT id, worder_id, height, weight, fabric_grade as 'الدرجة', " &
                                        "roll, batch_no AS 'رقم الرسالة', client_code AS 'كود العميل', " &
                                        "color AS 'اللون', product_name AS 'الخامة' " &
                                        "FROM store_finish pf " &
                                        "WHERE worder_id = @worder_id"

                    If Not String.IsNullOrEmpty(criteria.RollNo) Then
                        query &= " AND roll = @roll"
                    End If

                    Using cmd As New SqlCommand(query, connection)
                        cmd.Parameters.AddWithValue("@worder_id", criteria.WOrderId)
                        If Not String.IsNullOrEmpty(criteria.RollNo) Then
                            cmd.Parameters.AddWithValue("@roll", criteria.RollNo)
                        End If

                        Using reader As SqlDataReader = cmd.ExecuteReader()
                            While reader.Read()
                                Dim row As DataRow = resultTable.NewRow()
                                row("OrderIndex") = criteria.Order
                                row("id") = reader("id")
                                row("worder_id") = reader("worder_id")
                                row("height") = reader("height")
                                row("weight") = reader("weight")
                                row("الدرجة") = reader("الدرجة")
                                row("roll") = reader("roll")
                                row("رقم الرسالة") = reader("رقم الرسالة")
                                row("كود العميل") = reader("كود العميل")
                                row("اللون") = reader("اللون")
                                row("الخامة") = reader("الخامة")
                                resultTable.Rows.Add(row)
                            End While
                        End Using
                    End Using
                Next

                ' Sort the results by the original order
                Dim sortedView As New DataView(resultTable)
                sortedView.Sort = "OrderIndex ASC"

                ' Create final table without the OrderIndex column
                Dim finalTable As DataTable = sortedView.ToTable(False, "id", "worder_id", "height", "weight",
                                                               "الدرجة", "roll", "رقم الرسالة", "كود العميل",
                                                               "اللون", "الخامة")

                ' Bind the sorted results to the DataGridView
                dgvresults.DataSource = finalTable

                ' Apply styles and update calculations
                ApplyDataGridViewStyles()
                CalculateSummaryForSelectedWorderIds()
                CalculateTotalHeight()
                CalculateTotalweight()

            Catch ex As Exception
                MessageBox.Show("Error executing search: " & ex.Message)
            End Try
        End Using
    End Sub

End Class

