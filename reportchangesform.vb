Imports System.Data.SqlClient

Public Class reportchangesform
    ' Connection string for your database
    Private connectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"

    ' Event handler for form load
    Private Sub reportchangesform_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        LoadKindTrans()
        Loaddepartment()
        CustomizeDataGridView()
    End Sub

    ' Method to load distinct kindtrans into the combo box
    Private Sub LoadKindTrans()
        Dim query As String = "SELECT DISTINCT kindtrans FROM activity_logs"
        Using connection As New SqlConnection(connectionString)
            Try
                connection.Open()
                Using command As New SqlCommand(query, connection)
                    Using reader As SqlDataReader = command.ExecuteReader()
                        cmbkindtrans.Items.Clear()
                        While reader.Read()
                            cmbkindtrans.Items.Add(reader("kindtrans").ToString())
                        End While
                    End Using
                End Using
            Catch ex As Exception
                MessageBox.Show("Error loading kindtrans: " & ex.Message)
            End Try
        End Using
    End Sub
    ' Method to load distinct kindtrans into the combo box
    Private Sub Loaddepartment()
        Dim query As String = "SELECT DISTINCT department FROM activity_logs"
        Using connection As New SqlConnection(connectionString)
            Try
                connection.Open()
                Using command As New SqlCommand(query, connection)
                    Using reader As SqlDataReader = command.ExecuteReader()
                        cmbdepartment.Items.Clear()
                        While reader.Read()
                            cmbdepartment.Items.Add(reader("department").ToString())
                        End While
                    End Using
                End Using
            Catch ex As Exception
                MessageBox.Show("Error loading Department: " & ex.Message)
            End Try
        End Using
    End Sub
    Private Sub CustomizeDataGridView()
        ' توسيط النصوص داخل الأعمدة
        For Each column As DataGridViewColumn In dgvResults.Columns
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            column.DefaultCellStyle.WrapMode = DataGridViewTriState.True
            column.DefaultCellStyle.Font = New Font("Arial", 12, FontStyle.Bold)
        Next

        ' ضبط عرض الأعمدة تلقائيًا حسب المحتوى
        dgvResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells

        ' تغيير ألوان العناوين
        dgvResults.EnableHeadersVisualStyles = False
        dgvResults.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue ' اختر اللون المناسب
        dgvResults.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black
        dgvResults.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 12, FontStyle.Bold)
        dgvResults.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
    End Sub
    ' Event handler for the search button click
    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsearch.Click
        ' Ensure that cmbkindtrans has a selected value
        Dim kindtrans As String = If(cmbkindtrans.SelectedItem.ToString(), String.Empty)

        If String.IsNullOrEmpty(kindtrans) Then
            MessageBox.Show("Please select a KindTrans.")
            Return
        End If

        ' Validate if either txtworderid or cmbdepartment is provided
        Dim hasWorderId As Boolean = Not String.IsNullOrWhiteSpace(txtworderid.Text)
        Dim hasDepartment As Boolean = cmbdepartment.SelectedItem IsNot Nothing

        

        ' Initialize the base query
        Dim query As String = String.Empty
        Dim conditions As New List(Of String)

        ' Add KindTrans condition (mandatory)
        conditions.Add("kindtrans = @kindtrans")

        ' Add condition for Worder ID if provided
        If hasWorderId Then
            conditions.Add("worderid = @worderid")
        End If

        ' Add condition for Department if provided
        If hasDepartment Then
            conditions.Add("department = @department")
        End If

        ' Add condition for Date Range if provided
        If dtpfrom.Value.Date <= dtpto.Value.Date Then
            conditions.Add("timestamp BETWEEN @fromdate AND @todate")
        End If

        ' Build query based on selected KindTrans
        If kindtrans = "تعديل كود المكتبه" Then
            query = "SELECT timestamp AS 'التاريخ', department AS 'الإدارة', username AS 'يوزر', kindtrans AS 'نوع الحركه ', worderid AS 'أمر شغل', oldcodelib AS 'من كود مكتبه', newcodelib AS 'الى كود مكتبه ', reason AS 'سبب التغيير' " &
                    "FROM activity_logs WHERE "
        ElseIf kindtrans = "تغيير كميات أمر شغل" Then
            query = "SELECT timestamp AS 'التاريخ', department AS 'الإدارة', username AS 'يوزر', kindtrans AS 'نوع الحركه ', worderid AS 'أمر شغل', oldqtykg AS 'من كمية KG', newqtykg AS 'الى كمية KG', oldqtym AS 'من كمية M', newqtym AS 'الى كمية متر', reason AS 'سبب التغيير' " &
                    "FROM activity_logs WHERE "
        Else
            MessageBox.Show("Invalid KindTrans selected.")
            Return
        End If

        ' Append conditions to query
        query &= String.Join(" AND ", conditions)

        ' Execute the query and display results
        DisplaySearchResults(query)
    End Sub

    ' Method to execute the search query and display results in DataGridView
    Private Sub DisplaySearchResults(ByVal query As String)
        Using connection As New SqlConnection(connectionString)
            Try
                connection.Open()
                Using command As New SqlCommand(query, connection)
                    ' Add parameters for query
                    command.Parameters.AddWithValue("@kindtrans", cmbkindtrans.SelectedItem.ToString())

                    If cmbdepartment.SelectedItem IsNot Nothing Then
                        command.Parameters.AddWithValue("@department", cmbdepartment.SelectedItem.ToString())
                    End If

                    If Not String.IsNullOrWhiteSpace(txtworderid.Text) Then
                        command.Parameters.AddWithValue("@worderid", txtworderid.Text)
                    End If

                    If dtpfrom.Value.Date <= dtpto.Value.Date Then
                        command.Parameters.AddWithValue("@fromdate", dtpfrom.Value.Date)
                        command.Parameters.AddWithValue("@todate", dtpto.Value.Date)
                    End If

                    ' Execute and fill DataGridView
                    Using reader As SqlDataReader = command.ExecuteReader()
                        If reader.HasRows Then
                            Dim table As New DataTable()
                            table.Load(reader)
                            dgvResults.DataSource = table
                        Else
                            MessageBox.Show("No data found for the selected search criteria.")
                        End If
                    End Using
                End Using
            Catch ex As Exception
                MessageBox.Show("Error executing search: " & ex.Message)
            End Try
        End Using
    End Sub

End Class
