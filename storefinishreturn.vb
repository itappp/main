Imports System.Data.SqlClient

Public Class storefinishreturn
    Private sqlServerConnectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"
    Private selectAllCheckBox As CheckBox
    Private summaryDataGridView As DataGridView


    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        Dim workOrders As String = txtWorkOrder.Text
        If String.IsNullOrWhiteSpace(workOrders) Then
            MessageBox.Show("Please enter one or more work orders.")
            Return
        End If

        Dim workOrderList As String() = workOrders.Split(","c).Select(Function(wo) wo.Trim()).ToArray()

        Using connection As New SqlConnection(sqlServerConnectionString)
            Dim query As String = "SELECT p.storefinishid,p.ref_packing, sf.worder_id as 'امر الشغل', sf.roll as 'توب رقم', p.height as 'طول بيع', p.weight as 'وزن بيع' " &
                                  "FROM store_finish sf LEFT JOIN packing p ON sf.id = p.storefinishid " &
                                  "WHERE sf.worder_id IN (" & String.Join(",", workOrderList.Select(Function(w) "'" & w & "'")) & ")"
            Dim cmd As New SqlCommand(query, connection)

            Try
                connection.Open()
                Dim adapter As New SqlDataAdapter(cmd)
                Dim table As New DataTable()
                adapter.Fill(table)
                dgvResults.DataSource = table

                ' Add columns for user input if they don't already exist
                If Not dgvResults.Columns.Contains("select_checkbox") Then
                    Dim checkboxColumn As New DataGridViewCheckBoxColumn()
                    checkboxColumn.Name = "select_checkbox"
                    checkboxColumn.HeaderText = "Select"
                    dgvResults.Columns.Insert(0, checkboxColumn)
                End If

                If Not dgvResults.Columns.Contains("return_height") Then
                    Dim heightColumn As New DataGridViewTextBoxColumn()
                    heightColumn.Name = "return_height"
                    heightColumn.HeaderText = "طول المرتجع"
                    dgvResults.Columns.Add(heightColumn)
                End If

                If Not dgvResults.Columns.Contains("return_weight") Then
                    Dim weightColumn As New DataGridViewTextBoxColumn()
                    weightColumn.Name = "return_weight"
                    weightColumn.HeaderText = "وزن المرتجع"
                    dgvResults.Columns.Add(weightColumn)
                End If

                ' Add "Select All" checkbox to the header only if it doesn't already exist
                If selectAllCheckBox Is Nothing Then
                    selectAllCheckBox = New CheckBox()
                    selectAllCheckBox.Size = New Size(15, 15)
                    Dim headerCellRect As Rectangle = dgvResults.GetCellDisplayRectangle(0, -1, True)
                    selectAllCheckBox.Location = New Point(headerCellRect.Location.X + headerCellRect.Width / 2 - selectAllCheckBox.Width / 2, headerCellRect.Location.Y + 4)
                    selectAllCheckBox.BackColor = Color.Transparent
                    dgvResults.Controls.Add(selectAllCheckBox)

                    ' Add the "Select All" checkbox functionality for the header click
                    AddHandler selectAllCheckBox.CheckedChanged, AddressOf SelectAllCheckBox_CheckedChanged
                End If

                ' Apply DataGridView styles
                ApplyDataGridViewStyles()

            Catch ex As Exception
                MessageBox.Show("An error occurred: " & ex.Message)
            End Try
        End Using
        AddHandler dgvResults.CellValueChanged, AddressOf dgvResults_CellValueChanged
        AddHandler dgvResults.CurrentCellDirtyStateChanged, AddressOf dgvResults_CurrentCellDirtyStateChanged
    End Sub
    Private Sub dgvResults_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex >= 0 AndAlso (e.ColumnIndex = dgvResults.Columns("select_checkbox").Index OrElse e.ColumnIndex = dgvResults.Columns("return_height").Index OrElse e.ColumnIndex = dgvResults.Columns("return_weight").Index) Then
            UpdateSummary()
        End If
    End Sub

    Private Sub dgvResults_CurrentCellDirtyStateChanged(sender As Object, e As EventArgs)
        If dgvResults.IsCurrentCellDirty Then
            dgvResults.CommitEdit(DataGridViewDataErrorContexts.Commit)
        End If
    End Sub

    Private Sub UpdateSummary()
        Dim summaryTable As New DataTable()
        summaryTable.Columns.Add("worder_id", GetType(String))
        summaryTable.Columns.Add("total_return_height", GetType(Decimal))
        summaryTable.Columns.Add("total_return_weight", GetType(Decimal))
        summaryTable.Columns.Add("roll_count", GetType(Integer))

        Dim summaryDict As New Dictionary(Of String, (Decimal, Decimal, Integer))()

        For Each row As DataGridViewRow In dgvResults.Rows
            If row.Cells("select_checkbox").Value IsNot Nothing AndAlso Convert.ToBoolean(row.Cells("select_checkbox").Value) Then
                Dim worderId As String = row.Cells("امر الشغل").Value.ToString()
                Dim returnHeight As Decimal = If(row.Cells("return_height").Value IsNot Nothing, Convert.ToDecimal(row.Cells("return_height").Value), 0)
                Dim returnWeight As Decimal = If(row.Cells("return_weight").Value IsNot Nothing, Convert.ToDecimal(row.Cells("return_weight").Value), 0)

                If summaryDict.ContainsKey(worderId) Then
                    summaryDict(worderId) = (summaryDict(worderId).Item1 + returnHeight, summaryDict(worderId).Item2 + returnWeight, summaryDict(worderId).Item3 + 1)
                Else
                    summaryDict(worderId) = (returnHeight, returnWeight, 1)
                End If
            End If
        Next

        Dim totalReturnHeight As Decimal = 0
        Dim totalReturnWeight As Decimal = 0
        Dim totalRollCount As Integer = 0

        For Each kvp In summaryDict
            summaryTable.Rows.Add(kvp.Key, kvp.Value.Item1, kvp.Value.Item2, kvp.Value.Item3)
            totalReturnHeight += kvp.Value.Item1
            totalReturnWeight += kvp.Value.Item2
            totalRollCount += kvp.Value.Item3
        Next

        ' Add the total row
        summaryTable.Rows.Add("Total", totalReturnHeight, totalReturnWeight, totalRollCount)

        summaryDataGridView.DataSource = summaryTable
    End Sub


    Private Sub SelectAllCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim isChecked As Boolean = selectAllCheckBox.Checked

        ' Loop through each row and set the checkbox value based on "Select All" checkbox state
        For Each row As DataGridViewRow In dgvResults.Rows
            row.Cells("select_checkbox").Value = isChecked
        Next
        dgvResults.RefreshEdit()
        UpdateSummary()
    End Sub

    Private Sub btnReturn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnReturn.Click
        Dim workOrders As String = txtWorkOrder.Text
        If String.IsNullOrWhiteSpace(workOrders) Then
            MessageBox.Show("Please enter one or more work orders.")
            Return
        End If

        Using connection As New SqlConnection(sqlServerConnectionString)
            connection.Open()
            Dim transaction As SqlTransaction = connection.BeginTransaction()

            Try
                ' Generate SalesReturnCode once
                Dim salesReturnCode As String = GenerateSalesReturnCode(connection, transaction)

                For Each row As DataGridViewRow In dgvResults.Rows
                    Dim isSelected As Boolean = Convert.ToBoolean(row.Cells("select_checkbox").Value)
                    If isSelected Then
                        Dim storefinishid As Integer = Convert.ToInt32(row.Cells("storefinishid").Value)
                        Dim refPacking As String = row.Cells("ref_packing").Value.ToString()
                        Dim returnHeight As Decimal = 0
                        Dim returnWeight As Decimal = 0
                        Dim availableHeight As Decimal = Convert.ToDecimal(row.Cells("طول بيع").Value)
                        Dim availableWeight As Decimal = Convert.ToDecimal(row.Cells("وزن بيع").Value)

                        ' Validate and get the return height and weight
                        If Not Decimal.TryParse(row.Cells("return_height").Value?.ToString(), returnHeight) OrElse returnHeight <= 0 Then
                            MessageBox.Show("Please enter a valid return height for ref_packing: " & refPacking)
                            Continue For
                        End If

                        If Not Decimal.TryParse(row.Cells("return_weight").Value?.ToString(), returnWeight) OrElse returnWeight <= 0 Then
                            MessageBox.Show("Please enter a valid return weight for ref_packing: " & refPacking)
                            Continue For
                        End If

                        ' Check if return height or weight is zero
                        If returnHeight = 0 OrElse returnWeight = 0 Then
                            MessageBox.Show("Return height and weight cannot be zero for ref_packing: " & refPacking)
                            Continue For
                        End If

                        ' Check if return height or weight is greater than available
                        If returnHeight > availableHeight OrElse returnWeight > availableWeight Then
                            MessageBox.Show("لا يمكن عمل مرتجع بكميه اكبر من الكميه المتاحه: " & refPacking)
                            Continue For
                        End If

                        ' Update store_finish
                        Dim updateQuery As String = "UPDATE store_finish SET height = height + @height, weight = weight + @weight WHERE id = @storefinishid"
                        Using updateCmd As New SqlCommand(updateQuery, connection, transaction)
                            updateCmd.Parameters.AddWithValue("@storefinishid", storefinishid)
                            updateCmd.Parameters.AddWithValue("@height", returnHeight)
                            updateCmd.Parameters.AddWithValue("@weight", returnWeight)
                            updateCmd.ExecuteNonQuery()
                        End Using

                        ' Update packing
                        Dim updatePackingQuery As String = "UPDATE packing SET height = height - @height, weight = weight - @weight WHERE storefinishid = @storefinishid"
                        Using updatePackingCmd As New SqlCommand(updatePackingQuery, connection, transaction)
                            updatePackingCmd.Parameters.AddWithValue("@storefinishid", storefinishid)
                            updatePackingCmd.Parameters.AddWithValue("@height", returnHeight)
                            updatePackingCmd.Parameters.AddWithValue("@weight", returnWeight)
                            updatePackingCmd.ExecuteNonQuery()
                        End Using

                        ' Insert into SalesReturn
                        Dim insertSalesReturnQuery As String = "INSERT INTO SalesReturn (SalesReturnCode, StoreFinishId, ReturnHeight, ReturnWeight, ReturnDate, WorkOrder, username) VALUES (@SalesReturnCode, @StoreFinishId, @ReturnHeight, @ReturnWeight, @ReturnDate, @WorkOrder, @username)"
                        Using insertSalesReturnCmd As New SqlCommand(insertSalesReturnQuery, connection, transaction)
                            insertSalesReturnCmd.Parameters.AddWithValue("@SalesReturnCode", salesReturnCode)
                            insertSalesReturnCmd.Parameters.AddWithValue("@StoreFinishId", storefinishid)
                            insertSalesReturnCmd.Parameters.AddWithValue("@ReturnHeight", returnHeight)
                            insertSalesReturnCmd.Parameters.AddWithValue("@ReturnWeight", returnWeight)
                            insertSalesReturnCmd.Parameters.AddWithValue("@ReturnDate", DateTime.Now)
                            insertSalesReturnCmd.Parameters.AddWithValue("@WorkOrder", row.Cells("امر الشغل").Value.ToString())
                            insertSalesReturnCmd.Parameters.AddWithValue("@username", LoggedInUsername)
                            insertSalesReturnCmd.ExecuteNonQuery()
                        End Using
                    End If
                Next

                transaction.Commit()
                MessageBox.Show("Return processed successfully.")

                ' Clear txtWorkOrder and dgvResults
                txtWorkOrder.Clear()
                dgvResults.DataSource = Nothing
                summaryDataGridView.DataSource = Nothing
            Catch ex As Exception
                transaction.Rollback()
                MessageBox.Show("An error occurred: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Function GenerateSalesReturnCode(connection As SqlConnection, transaction As SqlTransaction) As String
        Dim query As String = "SELECT MAX(SalesReturnCode) FROM SalesReturn"
        Using cmd As New SqlCommand(query, connection, transaction)
            Dim result As Object = cmd.ExecuteScalar()
            If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                Dim lastCode As String = result.ToString()
                Dim lastNumber As Integer = Integer.Parse(lastCode.Split("-"c)(1))
                Return "salesreturn-" & (lastNumber + 1).ToString("D7")
            Else
                Return "salesreturn-0000001"
            End If
        End Using
    End Function


    Private Sub ApplyDataGridViewStyles()
        ' Set font and alignment for the DataGridView content
        dgvResults.DefaultCellStyle.Font = New Font("Arial", 12, FontStyle.Bold)
        dgvResults.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        ' Set font and color for the DataGridView header
        dgvResults.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 12, FontStyle.Bold)
        dgvResults.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue
        dgvResults.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black
        dgvResults.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        ' Apply header and content styles
        dgvResults.EnableHeadersVisualStyles = False ' Allow custom header styles to take effect

        ' Set columns to fill the DataGridView width
        dgvResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    End Sub

    Private Sub storefinishreturn_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblUsername.Text = "Logged in as: " & LoggedInUsername
        ' Retrieve the public_name for the logged-in user
        Dim publicName As String = GetPublicName(LoggedInUsername)
        summaryDataGridView = New DataGridView()
        summaryDataGridView.Location = New Point(10, dgvResults.Bottom + 10)
        summaryDataGridView.Size = New Size(dgvResults.Width, 150)
        summaryDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        summaryDataGridView.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 12, FontStyle.Bold)
        summaryDataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue
        summaryDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black
        summaryDataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        summaryDataGridView.EnableHeadersVisualStyles = False

        Me.Controls.Add(summaryDataGridView)
    End Sub

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