Imports MySql.Data.MySqlClient
Imports System.Data.SqlClient
Imports Excel = Microsoft.Office.Interop.Excel
Imports OfficeOpenXml
Imports System.IO
Imports System.Drawing.Printing
Imports ZXing
Imports System.Drawing

Public Class storefinishform
    ' Define the connection string to connect to the MySQL database
    Private connectionsqlString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"
    Private printDocument As New PrintDocument()
    Private printData As New List(Of Tuple(Of String, String))()
    Private currentPrintIndex As Integer = 0

    ' Checkbox in the header
    Private headerCheckbox As CheckBox = New CheckBox()
    Private pinkRowsCheckbox As CheckBox = New CheckBox() ' New checkbox for pink rows

    ' Add these variables at the class level
    Private accumulatedResults As New DataTable()
    Private isFirstSearch As Boolean = True



    Private Sub btnPrintAllRolls_Click(sender As Object, e As EventArgs) Handles btnPrintAllRolls.Click
        Dim worderId As String = txtworderid.Text.Trim()
        If String.IsNullOrEmpty(worderId) Then
            MessageBox.Show("Please enter a work order ID first.")
            Return
        End If

        ' Ask user if they want to print all rolls or a specific roll
        Dim result As DialogResult = MessageBox.Show("هل تريد طباعة كل الأتواب؟", "اختيار نوع الطباعة",
                                                   MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        Dim rollNumber As String = ""
        If result = DialogResult.No Then
            ' Print specific roll
            rollNumber = InputBox("أدخل رقم التوب الذي تريد طباعته:", "رقم التوب")
            If String.IsNullOrEmpty(rollNumber) Then
                MessageBox.Show("يجب إدخال رقم التوب")
                Return
            End If
        End If

        Using connection As New SqlConnection(connectionsqlString)
            Try
                connection.Open()
                Dim query As String
                If result = DialogResult.Yes Then
                    ' Print all rolls
                    query = "SELECT worder_id, roll FROM store_finish WHERE worder_id = @worder_id ORDER BY CAST(roll AS INT)"
                Else
                    query = "SELECT worder_id, roll FROM store_finish WHERE worder_id = @worder_id AND roll = @roll"
                End If

                Using cmd As New SqlCommand(query, connection)
                    cmd.Parameters.AddWithValue("@worder_id", worderId)
                    If result = DialogResult.No Then
                        cmd.Parameters.AddWithValue("@roll", rollNumber)
                    End If

                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        printData.Clear()
                        While reader.Read()
                            printData.Add(New Tuple(Of String, String)(reader("worder_id").ToString(), reader("roll").ToString()))
                        End While
                    End Using
                End Using

                If printData.Count = 0 Then
                    MessageBox.Show("لم يتم العثور على أتواب للطباعة")
                    Return
                End If

                ' Reset the print index
                currentPrintIndex = 0

                ' Configure print document
                printDocument.DefaultPageSettings.Margins = New Margins(0, 0, 0, 0)
                printDocument.DefaultPageSettings.Landscape = False
                printDocument.DefaultPageSettings.PaperSize = New PaperSize("Custom", 400, 600) ' Adjust size as needed

                ' Remove any existing handlers to prevent duplicates
                RemoveHandler printDocument.PrintPage, AddressOf PrintDocument_PrintPage
                AddHandler printDocument.PrintPage, AddressOf PrintDocument_PrintPage

                ' Show printer selection dialog directly
                Dim printDialog As New PrintDialog()
                printDialog.Document = printDocument
                printDialog.AllowSomePages = True
                printDialog.AllowSelection = False
                printDialog.AllowPrintToFile = False
                printDialog.ShowHelp = True
                printDialog.UseEXDialog = True
                If printDialog.ShowDialog() = DialogResult.OK Then
                    ' Configure printer settings
                    printDocument.PrinterSettings = printDialog.PrinterSettings
                    printDocument.DefaultPageSettings.Margins = New Margins(0, 0, 0, 0)

                    ' Print the document
                    printDocument.Print()
                End If

            Catch ex As Exception
                MessageBox.Show("Error printing: " & ex.Message)
                currentPrintIndex = 0
            End Try
        End Using
    End Sub

    Private Sub PrintDocument_PrintPage(sender As Object, e As PrintPageEventArgs)
        Try
            ' Convert mm to pixels (assuming 96 DPI)
            Dim labelWidth As Integer = CInt(104 * 3.779527559) ' 104mm to pixels
            Dim labelHeight As Integer = CInt(130 * 3.779527559) ' 130mm to pixels

            ' Set up fonts
            Dim labelFont As New Font("Arial", 20, FontStyle.Bold) ' Font for labels
            Dim valueFont As New Font("Arial", 20, FontStyle.Bold) ' Font for values
            Dim brush As New SolidBrush(Color.Black)

            ' Print one label per page
            If currentPrintIndex < printData.Count Then
                Dim worderId As String = printData(currentPrintIndex).Item1
                Dim roll As String = printData(currentPrintIndex).Item2

                ' Calculate center position for the label
                Dim centerX As Integer = e.PageBounds.Width \ 2
                Dim centerY As Integer = e.PageBounds.Height \ 2

                ' Define table layout
                Dim tableStartY As Integer = centerY - 60 ' Start table 60 pixels above center
                Dim rowHeight As Integer = 40 ' Height of each row
                Dim columnLabelWidth As Integer = 100 ' Width for labels
                Dim columnValueWidth As Integer = 250 ' Width for values
                Dim leftMargin As Integer = centerX - (columnLabelWidth + columnValueWidth) \ 2 ' Left margin for table

                ' Draw table border
                Dim tableWidth As Integer = columnLabelWidth + columnValueWidth
                Dim tableHeight As Integer = rowHeight * 2
                e.Graphics.DrawRectangle(Pens.Black, leftMargin, tableStartY, tableWidth, tableHeight)

                ' Draw horizontal line between rows
                e.Graphics.DrawLine(Pens.Black, leftMargin, tableStartY + rowHeight, leftMargin + tableWidth, tableStartY + rowHeight)

                ' Draw vertical line between label and value
                e.Graphics.DrawLine(Pens.Black, leftMargin + columnLabelWidth, tableStartY, leftMargin + columnLabelWidth, tableStartY + tableHeight)

                ' Center-align labels and values
                ' First row: Work Order ID
                Dim label1Text As String = "أمر الشغل:"
                Dim value1Text As String = worderId
                Dim label1Size As SizeF = e.Graphics.MeasureString(label1Text, labelFont)
                Dim value1Size As SizeF = e.Graphics.MeasureString(value1Text, valueFont)

                Dim label1X As Single = leftMargin + (columnLabelWidth - label1Size.Width) / 2
                Dim value1X As Single = leftMargin + columnLabelWidth + (columnValueWidth - value1Size.Width) / 2
                Dim row1Y As Single = tableStartY + (rowHeight - label1Size.Height) / 2

                e.Graphics.DrawString(label1Text, labelFont, brush, label1X, row1Y)
                e.Graphics.DrawString(value1Text, valueFont, brush, value1X, row1Y)

                ' Second row: Roll Number
                Dim label2Text As String = "رقم التوب:"
                Dim value2Text As String = roll
                Dim label2Size As SizeF = e.Graphics.MeasureString(label2Text, labelFont)
                Dim value2Size As SizeF = e.Graphics.MeasureString(value2Text, valueFont)

                Dim label2X As Single = leftMargin + (columnLabelWidth - label2Size.Width) / 2
                Dim value2X As Single = leftMargin + columnLabelWidth + (columnValueWidth - value2Size.Width) / 2
                Dim row2Y As Single = tableStartY + rowHeight + (rowHeight - label2Size.Height) / 2

                e.Graphics.DrawString(label2Text, labelFont, brush, label2X, row2Y)
                e.Graphics.DrawString(value2Text, valueFont, brush, value2X, row2Y)

                ' Create barcode text
                Dim barcodeText As String = worderId & "*" & roll

                ' Generate barcode using ZXing
                Dim writer As New ZXing.BarcodeWriter()
                writer.Format = ZXing.BarcodeFormat.CODE_128
                writer.Options = New ZXing.Common.EncodingOptions With {
                    .Width = CInt(e.PageBounds.Width * 0.6), ' 60% of page width
                    .Height = 60,
                    .Margin = 0
                }

                ' Generate the barcode bitmap
                Dim barcodeBitmap As Bitmap = writer.Write(barcodeText)

                ' Calculate position for the barcode (centered at the bottom)
                Dim barcodeX As Integer = centerX - (barcodeBitmap.Width \ 2)
                Dim barcodeY As Integer = centerY + 50

                ' Draw the barcode
                e.Graphics.DrawImage(barcodeBitmap, barcodeX, barcodeY)

                ' Draw the barcode text below the barcode
                Dim barcodeFont As New Font("Arial", 10, FontStyle.Bold)
                Dim barcodeTextSize As SizeF = e.Graphics.MeasureString(barcodeText, barcodeFont)
                Dim barcodeTextX As Single = centerX - (barcodeTextSize.Width / 2)
                e.Graphics.DrawString(barcodeText, barcodeFont, brush, barcodeTextX, barcodeY + barcodeBitmap.Height + 5)

                currentPrintIndex += 1
            End If

            ' Set HasMorePages to true if there are more labels to print
            e.HasMorePages = (currentPrintIndex < printData.Count)

            ' If this is the last page, reset the currentPrintIndex for next print job
            If Not e.HasMorePages Then
                currentPrintIndex = 0
            End If

        Catch ex As Exception
            MessageBox.Show("Error during printing: " & ex.Message)
            e.HasMorePages = False
            currentPrintIndex = 0
        End Try
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        Dim worderId As String = txtworderid.Text.Trim()
        Dim contractNo As String = txtcontractno.Text.Trim()
        Dim fromDate As String = dtpfromDate.Value.ToString("yyyy-MM-dd")
        Dim toDate As String = dtpToDate.Value.ToString("yyyy-MM-dd")

        Dim conditions As New List(Of String)

        ' Search for exact match for worder_id instead of using LIKE with '%'
        If Not String.IsNullOrEmpty(worderId) Then
            conditions.Add("fi.worder_id = '" & worderId & "'")
        End If

        If Not String.IsNullOrEmpty(contractNo) Then
            conditions.Add("cs.contractno = '" & contractNo & "'")
        End If

        ' Handle date range search
        If dtpfromDate.Checked AndAlso dtpToDate.Checked Then
            conditions.Add("CAST(fi.date AS DATE) BETWEEN '" & fromDate & "' AND '" & toDate & "'")
        ElseIf dtpfromDate.Checked Then
            conditions.Add("CAST(fi.date AS DATE) >= '" & fromDate & "'")
        ElseIf dtpToDate.Checked Then
            conditions.Add("CAST(fi.date AS DATE) <= '" & toDate & "'")
        End If

        ' Build the WHERE clause based on conditions
        Dim whereClause As String = If(conditions.Count > 0, "WHERE " & String.Join(" AND ", conditions), "")

        ' Final query to be executed
        Dim query As String = "SELECT fi.id, " &
       "fi.worder_id AS 'أمر شغل', " &
       "cs.contractno AS 'رقم التعاقد', " &
       "cs.batch AS 'الرسالة', " &
       "cs.refno AS 'رقم الاذن', " &
       "fi.roll AS 'توب رقم', " &
       "clients.code AS 'كود العميل', " &
       "MAX(fi.date) AS 'تاريخ الفحص', " &
       "fi.width AS 'العرض', " &
       "fi.height AS 'طول التوب', " &
       "fi.weight AS 'الوزن', " &
       "fi.fabric_grade AS 'درجة القماش', " &
       "cs.color AS 'اللون', " &
       "cs.Material AS 'الخامة' " &
       "FROM finish_inspect fi " &
       "LEFT JOIN techdata td ON fi.worder_id = td.worderid " &
       "LEFT JOIN Contracts cs ON td.contract_id = cs.ContractID " &
       "LEFT JOIN clients ON cs.ClientCode = clients.id " &
       whereClause & " " &
       "GROUP BY fi.id, fi.worder_id, cs.contractno, cs.batch, cs.refno, fi.roll, clients.code, fi.width, fi.height, fi.weight, fi.fabric_grade, cs.color, cs.Material"

        ' Load the data with the constructed query
        LoadData(query)
    End Sub

    Private Sub LoadData(ByVal query As String)
        Using connection As New SqlConnection(connectionsqlString)
            Dim adapter As New SqlDataAdapter(query, connection)
            Dim dt As New DataTable()

            Try
                connection.Open()
                adapter.Fill(dt)

                ' Create a new DataTable to hold the sorted data
                Dim sortedDt As New DataTable()
                sortedDt = dt.Clone()

                ' Add a column to track if the row exists in store_finish
                sortedDt.Columns.Add("ExistsInStore", GetType(Boolean))

                ' Process each row and check against store_finish
                For Each row As DataRow In dt.Rows
                    Dim newRow As DataRow = sortedDt.NewRow()
                    newRow.ItemArray = row.ItemArray

                    ' Check if the row exists in store_finish
                    Dim worderId As String = row("أمر شغل").ToString()
                    Dim roll As String = row("توب رقم").ToString()

                    Dim checkQuery As String = "SELECT COUNT(*) FROM store_finish WHERE worder_id = @worder_id AND roll = @roll"
                    Using checkCmd As New SqlCommand(checkQuery, connection)
                        checkCmd.Parameters.AddWithValue("@worder_id", worderId)
                        checkCmd.Parameters.AddWithValue("@roll", roll)
                        Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())
                        newRow("ExistsInStore") = (count > 0)
                    End Using

                    sortedDt.Rows.Add(newRow)
                Next

                ' Sort the DataTable - non-existing records first, then existing ones
                Dim sortedView As New DataView(sortedDt)
                sortedView.Sort = "ExistsInStore ASC"
                sortedDt = sortedView.ToTable()

                ' Remove the temporary column
                sortedDt.Columns.Remove("ExistsInStore")

                ' Bind data to DataGridView
                dgvresults.DataSource = sortedDt

                ' Set font size and style
                dgvresults.DefaultCellStyle.Font = New Font("Arial", 14, FontStyle.Bold)
                dgvresults.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 14, FontStyle.Bold)

                ' Set header colors
                dgvresults.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue
                dgvresults.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black
                dgvresults.EnableHeadersVisualStyles = False

                ' Add checkbox column if it doesn't already exist
                If Not dgvresults.Columns.Contains("Select") Then
                    Dim chkColumn As New DataGridViewCheckBoxColumn()
                    chkColumn.Name = "Select"
                    chkColumn.HeaderText = "Select"
                    chkColumn.Width = 30
                    chkColumn.ReadOnly = False
                    dgvresults.Columns.Insert(0, chkColumn)
                End If

                ' Center-align content and headers
                dgvresults.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                For Each column As DataGridViewColumn In dgvresults.Columns
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                Next

                ' Highlight existing records in green and non-existing in light pink
                For Each row As DataGridViewRow In dgvresults.Rows
                    Dim worderId As String = row.Cells("أمر شغل").Value.ToString()
                    Dim roll As String = row.Cells("توب رقم").Value.ToString()

                    Dim checkQuery As String = "SELECT COUNT(*) FROM store_finish WHERE worder_id = @worder_id AND roll = @roll"
                    Using checkCmd As New SqlCommand(checkQuery, connection)
                        checkCmd.Parameters.AddWithValue("@worder_id", worderId)
                        checkCmd.Parameters.AddWithValue("@roll", roll)
                        Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())

                        If count > 0 Then
                            ' Existing records in green
                            row.DefaultCellStyle.BackColor = Color.LightGreen
                            row.DefaultCellStyle.ForeColor = Color.Black
                        Else
                            ' Non-existing records in light pink
                            row.DefaultCellStyle.BackColor = Color.LightPink
                            row.DefaultCellStyle.ForeColor = Color.Black
                        End If
                    End Using
                Next

                ' Initialize totals
                CalculateTotals()
                SetupHeaderCheckbox()

            Catch ex As Exception
                MessageBox.Show("An error occurred: " & ex.Message)
            End Try
        End Using
    End Sub


    Private Sub CalculateTotals()
        Dim totalH As Decimal = 0
        Dim totalWW As Decimal = 0
        Dim totalRollCount As Integer = 0
        Dim countFabricGrade1 As Integer = 0
        Dim countFabricGrade2 As Integer = 0

        For Each row As DataGridViewRow In dgvresults.Rows
            If Convert.ToBoolean(row.Cells("Select").Value) Then ' Check if the row is selected
                If Not IsDBNull(row.Cells("طول التوب").Value) Then
                    totalH += Convert.ToDecimal(row.Cells("طول التوب").Value)
                End If
                If Not IsDBNull(row.Cells("الوزن").Value) Then
                    totalWW += Convert.ToDecimal(row.Cells("الوزن").Value)
                End If
                totalRollCount += 1

                ' Count occurrences of fabric_grade
                If Not IsDBNull(row.Cells("درجة القماش").Value) Then
                    Dim fabricGrade As Integer = Convert.ToInt32(row.Cells("درجة القماش").Value)
                    If fabricGrade = 1 Then
                        countFabricGrade1 += 1
                    ElseIf fabricGrade = 2 Then
                        countFabricGrade2 += 1
                    End If
                End If
            End If
        Next

        ' Display the results in the labels
        lbltotalh.Text = "الإجمالى بالمتر " & totalH.ToString()
        lbltotalroll.Text = "إجمالى عدد أتواب " & totalRollCount.ToString()
        lbltotalww.Text = "الإجمالى بالوزن " & totalWW.ToString() ' Add label for total WW
        lblfirst.Text = "توب درجة أولى" & countFabricGrade1.ToString() ' Count for fabric grade 1
        lblcnd.Text = "توب درجة تانية " & countFabricGrade2.ToString() ' Count for fabric grade 2
    End Sub
    Private Sub SetupHeaderCheckbox()
        If dgvresults.Controls.Contains(headerCheckbox) Then
            dgvresults.Controls.Remove(headerCheckbox)
        End If
        If dgvresults.Controls.Contains(pinkRowsCheckbox) Then
            dgvresults.Controls.Remove(pinkRowsCheckbox)
        End If

        If dgvresults.Columns.Count > 0 Then
            ' Setup main header checkbox
            headerCheckbox.Size = New Size(15, 15)
            headerCheckbox.BackColor = Color.Transparent
            Dim rect As Rectangle = dgvresults.GetCellDisplayRectangle(0, -1, True)
            headerCheckbox.Location = rect.Location
            AddHandler headerCheckbox.CheckedChanged, AddressOf HeaderCheckboxClick
            dgvresults.Controls.Add(headerCheckbox)

            ' Setup pink rows checkbox
            pinkRowsCheckbox.Size = New Size(15, 15)
            pinkRowsCheckbox.BackColor = Color.Transparent
            pinkRowsCheckbox.Location = New Point(rect.Location.X + 20, rect.Location.Y) ' Position it next to the main checkbox
            pinkRowsCheckbox.Text = "تحديد الصفوف الوردية"
            pinkRowsCheckbox.Font = New Font("Arial", 8, FontStyle.Bold)
            AddHandler pinkRowsCheckbox.CheckedChanged, AddressOf PinkRowsCheckboxClick
            dgvresults.Controls.Add(pinkRowsCheckbox)
        End If
    End Sub

    Private Sub PinkRowsCheckboxClick(ByVal sender As Object, ByVal e As EventArgs)
        Dim isChecked As Boolean = pinkRowsCheckbox.Checked
        For Each row As DataGridViewRow In dgvresults.Rows
            ' Only check/uncheck rows that are pink (not in store_finish)
            If row.DefaultCellStyle.BackColor = Color.LightPink Then
                row.Cells("Select").Value = isChecked
            End If
        Next
        CalculateTotals() ' Recalculate totals after selection
    End Sub

    Private Sub HeaderCheckboxClick(ByVal sender As Object, ByVal e As EventArgs)
        Dim isChecked As Boolean = headerCheckbox.Checked
        For Each row As DataGridViewRow In dgvresults.Rows
            row.Cells("Select").Value = isChecked ' Update each checkbox's value
        Next
        CalculateTotals() ' Recalculate totals after selection
    End Sub

    Private Sub dgvResults_CellContentClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs)
        If e.ColumnIndex = dgvresults.Columns("Select").Index AndAlso e.RowIndex >= 0 Then
            ' Toggle the checkbox
            Dim cell As DataGridViewCheckBoxCell = CType(dgvresults.Rows(e.RowIndex).Cells("Select"), DataGridViewCheckBoxCell)
            cell.Value = Not Convert.ToBoolean(cell.Value)

            ' Update the header checkbox based on the current row selections
            Dim allChecked As Boolean = dgvresults.Rows.Cast(Of DataGridViewRow)().All(Function(r) Convert.ToBoolean(r.Cells("Select").Value))
            headerCheckbox.Checked = allChecked

            dgvresults.EndEdit() ' End the edit to trigger any data bindings
            CalculateTotals() ' Recalculate totals whenever a checkbox is clicked
        End If
    End Sub

    Private Sub dgvResults_Resize(ByVal sender As Object, ByVal e As EventArgs) Handles dgvresults.Resize
        SetupHeaderCheckbox()
    End Sub
    Private Sub btnInsert_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnInsert.Click
        ' Check if at least one checkbox is selected
        Dim isChecked As Boolean = False
        For Each row As DataGridViewRow In dgvresults.Rows
            If Convert.ToBoolean(row.Cells("Select").Value) Then ' Assuming "Select" is the checkbox column name
                isChecked = True
                Exit For ' Exit the loop if at least one checkbox is checked
            End If
        Next

        ' If no checkboxes are selected, show a message and exit the method
        If Not isChecked Then
            MessageBox.Show("اختر الأتواب ")
            Return ' Exit the subroutine
        End If

        ' Open a connection to the SQL Server database
        Using connection As New SqlConnection(connectionsqlString)
            connection.Open()

            ' Loop through each row in the DataGridView
            For Each row As DataGridViewRow In dgvresults.Rows
                If Convert.ToBoolean(row.Cells("Select").Value) Then ' Check if the row is selected
                    Dim gid As String = row.Cells("id").Value.ToString()
                    Dim worderId As String = row.Cells("أمر شغل").Value.ToString()
                    Dim roll As String = row.Cells("توب رقم").Value.ToString()

                    ' Check if both worder_id and roll already exist
                    Dim checkQuery As String = "SELECT COUNT(*) FROM store_finish WHERE worder_id = @worder_id AND roll = @roll"
                    Using checkCmd As New SqlCommand(checkQuery, connection)
                        checkCmd.Parameters.AddWithValue("@worder_id", worderId)
                        checkCmd.Parameters.AddWithValue("@roll", roll)
                        Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())

                        If count > 0 Then
                            ' If both worder_id and roll exist, show a message and skip this row
                            MessageBox.Show("أمر الشغل رقم " & worderId & " والتوب رقم " & roll & " موجودان بالفعل")
                            Continue For ' Skip to the next row
                        End If
                    End Using

                    ' Define the SQL insert query
                    Dim query As String = "INSERT INTO store_finish (gid, worder_id, contract_no, batch_no, ref_no, roll, client_code, inspection_date, transaction_date, width, height, weight, fabric_grade, color, product_name, username, heightPK, weightPK) " &
                                          "VALUES (@gid, @worder_id, @contract_no, @batch_no, @ref_no, @roll, @client_code, @inspection_date, @transaction_date, @width, @height, @weight, @fabric_grade, @color, @product_name, @username, @heightPK, @weightPK)"

                    Using cmd As New SqlCommand(query, connection)
                        ' Add parameters to the command
                        cmd.Parameters.AddWithValue("@gid", gid)
                        cmd.Parameters.AddWithValue("@worder_id", worderId)
                        cmd.Parameters.AddWithValue("@contract_no", row.Cells("رقم التعاقد").Value)
                        cmd.Parameters.AddWithValue("@batch_no", row.Cells("الرسالة").Value)
                        cmd.Parameters.AddWithValue("@ref_no", row.Cells("رقم الاذن").Value)
                        cmd.Parameters.AddWithValue("@roll", roll)
                        cmd.Parameters.AddWithValue("@client_code", row.Cells("كود العميل").Value)
                        cmd.Parameters.AddWithValue("@transaction_date", DateTime.Now) ' Current date and time
                        cmd.Parameters.AddWithValue("@inspection_date", row.Cells("تاريخ الفحص").Value)
                        cmd.Parameters.AddWithValue("@width", row.Cells("العرض").Value)
                        cmd.Parameters.AddWithValue("@height", row.Cells("طول التوب").Value)
                        cmd.Parameters.AddWithValue("@weight", row.Cells("الوزن").Value)
                        cmd.Parameters.AddWithValue("@heightPK", row.Cells("طول التوب").Value)
                        cmd.Parameters.AddWithValue("@weightPK", row.Cells("الوزن").Value)
                        cmd.Parameters.AddWithValue("@fabric_grade", row.Cells("درجة القماش").Value)
                        cmd.Parameters.AddWithValue("@color", row.Cells("اللون").Value)
                        cmd.Parameters.AddWithValue("@product_name", row.Cells("الخامة").Value)
                        cmd.Parameters.AddWithValue("@username", lblUsername.Text.Replace("Logged in as: ", "")) ' Extract username from the label

                        ' Execute the insert command
                        cmd.ExecuteNonQuery()
                    End Using
                    ' After successfully inserting into store_finish

                    Dim worderStatusQuery As String = "IF EXISTS (SELECT 1 FROM worder_status WHERE worderid = @worderid) " &
                                                      "BEGIN " &
                                                      "    UPDATE worder_status " &
                                                      "    SET status = 'DONE' " &
                                                      "    WHERE worderid = @worderid " &
                                                      "END " &
                                                      "ELSE " &
                                                      "BEGIN " &
                                                      "    INSERT INTO worder_status (worderid, status) " &
                                                      "    VALUES (@worderid, 'DONE') " &
                                                      "END"

                    Using statusCmd As New SqlCommand(worderStatusQuery, connection)
                        statusCmd.Parameters.AddWithValue("@worderid", worderId)
                        statusCmd.ExecuteNonQuery()
                    End Using
                End If

            Next
        End Using
        ' Notify the user that data has been inserted successfully
        MessageBox.Show("تم تسجيل البيانات بنجاح")
        dgvresults.DataSource = Nothing
        CalculateTotals()
    End Sub

    Private Sub btnClear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnclear.Click
        ' Clear the DataGridView
        dgvresults.DataSource = Nothing

        ' Reset accumulated results
        accumulatedResults = New DataTable()
        isFirstSearch = True

        ' Clear the barcode textbox
        txtbarcode.Clear()

        ' Reset totals
        lbltotalh.Text = "الإجمالى بالمتر 0"
        lbltotalroll.Text = "إجمالى عدد أتواب 0"
        lbltotalww.Text = "الإجمالى بالوزن 0"
        lblfirst.Text = "توب درجة أولى 0"
        lblcnd.Text = "توب درجة تانية 0"
    End Sub

    Private Sub storefinishform_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Access the logged-in username from the global variable
        lblUsername.Text = "Logged in as: " & LoggedInUsername
        ' Set the user access level based on logged-in username
        SetUserAccessLevel(LoggedInUsername)
        ' Attach the event handler manually using AddHandler
        AddHandler btnview.Click, AddressOf btnView_Click
        AddHandler dgvresults.CellContentClick, AddressOf dgvResults_CellContentClick
        AddHandler btnclear.Click, AddressOf btnClear_Click
        SetupHeaderCheckbox()

    End Sub
    ' Event handler for View button click
    Private Sub btnView_Click(ByVal sender As Object, ByVal e As EventArgs)
        ' Navigate to the ContractViewForm
        Dim viewForm As New storefinishviewform()
        viewForm.Show()
    End Sub
    ' Assume this function is called during login to set the UserAccessLevel
    Private Sub SetUserAccessLevel(ByVal username As String)
        Try
            Using connection As New SqlConnection(connectionsqlString)
                connection.Open()
                Dim query As String = "SELECT acc_level FROM dep_users WHERE username = @username"
                Using cmd As New SqlCommand(query, connection)
                    cmd.Parameters.AddWithValue("@username", username)
                    Dim accLevel As Object = cmd.ExecuteScalar()
                    If accLevel IsNot Nothing Then
                        UserAccessLevel = CInt(accLevel)
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error retrieving user access level: " & ex.Message)
        End Try
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
        Dim query As String = "SELECT fi.id, " &
           "fi.worder_id AS 'أمر شغل', " &
           "cs.contractno AS 'رقم التعاقد', " &
           "cs.batch AS 'الرسالة', " &
           "cs.refno AS 'رقم الاذن', " &
           "fi.roll AS 'توب رقم', " &
           "clients.code AS 'كود العميل', " &
           "fi.date AS 'تاريخ الفحص', " &
           "fi.width AS 'العرض', " &
           "fi.height AS 'طول التوب', " &
           "fi.weight AS 'الوزن', " &
           "fi.fabric_grade AS 'درجة القماش', " &
           "cs.color AS 'اللون', " &
           "cs.Material AS 'الخامة' " &
           "FROM finish_inspect fi " &
           "LEFT JOIN techdata td ON fi.worder_id = td.worderid " &
           "LEFT JOIN Contracts cs ON td.contract_id = cs.ContractID " &
           "LEFT JOIN clients ON cs.ClientCode = clients.id " &
           "WHERE fi.worder_id = @worder_id"

        ' Load the data with parameters
        LoadBarcodeDataWithParams(query, worderId, roll)

        ' Clear the barcode textbox for next scan
        txtbarcode.Clear()
    End Sub

    Private Sub LoadBarcodeDataWithParams(ByVal query As String, ByVal worderId As String, ByVal roll As String)
        Using connection As New SqlConnection(connectionsqlString)
            Try
                connection.Open()
                Using cmd As New SqlCommand(query, connection)
                    cmd.Parameters.AddWithValue("@worder_id", worderId)

                    Dim adapter As New SqlDataAdapter(cmd)
                    Dim dt As New DataTable()
                    adapter.Fill(dt)

                    If dt.Rows.Count = 0 Then
                        MessageBox.Show("لم يتم العثور على بيانات لأمر الشغل: " & worderId)
                        Return
                    End If

                    ' Filter the results to show only the specific tube number
                    Dim filteredRows = dt.AsEnumerable().Where(Function(r) r.Field(Of Object)("توب رقم").ToString() = roll).ToList()
                    If filteredRows.Count = 0 Then
                        MessageBox.Show("لم يتم العثور على التوب رقم " & roll & " لأمر الشغل: " & worderId)
                        Return
                    End If

                    ' Create a new DataTable with only the filtered rows
                    Dim filteredDt As DataTable = dt.Clone()
                    For Each row In filteredRows
                        filteredDt.ImportRow(row)
                    Next

                    ' Initialize accumulatedResults if it's the first search
                    If isFirstSearch Then
                        accumulatedResults = filteredDt.Clone()
                        isFirstSearch = False
                    End If

                    ' Add new rows to accumulated results
                    For Each row As DataRow In filteredDt.Rows
                        ' Check if this combination already exists in accumulated results
                        Dim exists As Boolean = False
                        For Each accRow As DataRow In accumulatedResults.Rows
                            If accRow("أمر شغل").ToString() = row("أمر شغل").ToString() AndAlso
                               accRow("توب رقم").ToString() = row("توب رقم").ToString() Then
                                exists = True
                                Exit For
                            End If
                        Next

                        If Not exists Then
                            accumulatedResults.ImportRow(row)
                        End If
                    Next

                    ' Bind accumulated results to DataGridView
                    dgvresults.DataSource = accumulatedResults

                    ' Apply formatting
                    FormatDataGridView()

                    ' Initialize totals
                    CalculateTotals()
                    SetupHeaderCheckbox()
                End Using
            Catch ex As Exception
                MessageBox.Show("حدث خطأ: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Sub FormatDataGridView()
        ' Set font size and style
        dgvresults.DefaultCellStyle.Font = New Font("Arial", 14, FontStyle.Bold)
        dgvresults.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 14, FontStyle.Bold)

        ' Set header colors
        dgvresults.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue
        dgvresults.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black
        dgvresults.EnableHeadersVisualStyles = False

        ' Add checkbox column if it doesn't already exist
        If Not dgvresults.Columns.Contains("Select") Then
            Dim chkColumn As New DataGridViewCheckBoxColumn()
            chkColumn.Name = "Select"
            chkColumn.HeaderText = "Select"
            chkColumn.Width = 30
            chkColumn.ReadOnly = False
            dgvresults.Columns.Insert(0, chkColumn)
        End If

        ' Center-align content and headers
        dgvresults.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        For Each column As DataGridViewColumn In dgvresults.Columns
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Next

        ' Highlight existing records in green and non-existing in light pink
        Using connection As New SqlConnection(connectionsqlString)
            Try
                connection.Open()
                For Each row As DataGridViewRow In dgvresults.Rows
                    Dim worderId As String = row.Cells("أمر شغل").Value.ToString()
                    Dim roll As String = row.Cells("توب رقم").Value.ToString()

                    Dim checkQuery As String = "SELECT COUNT(*) FROM store_finish WHERE worder_id = @worder_id AND roll = @roll"
                    Using checkCmd As New SqlCommand(checkQuery, connection)
                        checkCmd.Parameters.AddWithValue("@worder_id", worderId)
                        checkCmd.Parameters.AddWithValue("@roll", roll)
                        Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())

                        If count > 0 Then
                            row.DefaultCellStyle.BackColor = Color.LightGreen
                            row.DefaultCellStyle.ForeColor = Color.Black
                            row.Cells("Select").Value = False ' Uncheck green rows
                        Else
                            row.DefaultCellStyle.BackColor = Color.LightPink
                            row.DefaultCellStyle.ForeColor = Color.Black
                            row.Cells("Select").Value = True ' Automatically check pink rows
                        End If
                    End Using
                Next

                ' Update totals after automatic selection
                CalculateTotals()
            Catch ex As Exception
                MessageBox.Show("حدث خطأ أثناء التحقق من السجلات: " & ex.Message)
            End Try
        End Using
    End Sub
End Class


