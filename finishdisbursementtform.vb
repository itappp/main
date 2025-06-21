Imports System.Data.SqlClient

Public Class finishDisbursementtform
    Private sqlServerConnectionString As String = "Server=localhost;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"
     
    ' Form Load event to populate the DataGridView and ComboBox
    Private Sub FinishDisbursementForm_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        cmbKindTrans.Items.Clear()
        cmbKindTrans.Items.Add("صرف مجهز")
        cmbKindTrans.Items.Add("مرتجع مجهز")
        cmbtoorfrom.Items.Clear()
        cmbtoorfrom.Items.Add("الإدارة التجارية")
        cmbtoorfrom.Items.Add("الادارة الفنيه")
        cmbtoorfrom.Items.Add("الإنتاج")
        cmbtoorfrom.Items.Add("عميل")
        cmbtoorfrom.Items.Add("فحص المجهز")
        cmbtoorfrom.Items.Add("مخزن المجهز")
        cmbtoorfrom.Items.Add("فحص الخام")
        cmbtoorfrom.Items.Add("معمل الجوده")
        cmbtoorfrom.Items.Add("معمل الألوان")
        cmbKindTrans.SelectedIndex = 0 ' Set default selection

        CustomizeDataGridView()
        PopulateProductNames()
        Populateclientcode()
        Populatebatchno()
        Populatecolor()

        lblusername.Text = "Logged in as: " & LoggedInUsername
        ' Retrieve the public_name for the logged-in user
        Dim publicName As String = GetPublicName(LoggedInUsername)
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsearch.Click
        ' Check if cmbKindTrans has a selected item
        If cmbKindTrans.SelectedItem Is Nothing Then
            MessageBox.Show("أختر نوع الحركه صرف مجهز ام مرتجع مجهز")
            Return
        End If

        ' Retrieve filters
        Dim worderIds As String = txtworderid.Text.Trim()
        Dim productName As String = If(CmbProductName.SelectedItem IsNot Nothing, CmbProductName.SelectedItem.ToString(), "")
        Dim clientCode As String = If(cmbclient.SelectedItem IsNot Nothing, cmbclient.SelectedItem.ToString(), "")
        Dim batchNo As String = If(cmbbatch.SelectedItem IsNot Nothing, cmbbatch.SelectedItem.ToString(), "")
        Dim color As String = If(cmbcolor.SelectedItem IsNot Nothing, cmbcolor.SelectedItem.ToString(), "")

        ' Construct the base query
        Dim baseQuery As String = ""
        Dim filters As New List(Of String)

        If cmbKindTrans.SelectedItem.ToString() = "صرف مجهز" Then
            baseQuery = "SELECT id, worder_id AS 'أمر شغل ', batch_no AS 'رقم الرسالة', client_code AS 'عميل', roll AS 'رقم التوب', " & _
                        "height AS 'الطول رصيد', weight AS 'الوزن رصيد', fabric_grade AS 'درجه القماش', color AS 'اللون', " & _
                        "product_name AS 'الخامة', CAST(0 AS Decimal(10,2)) AS 'حركه متر',CAST(0 AS Decimal(10,2)) AS 'حركه وزن' FROM store_finish"
        ElseIf cmbKindTrans.SelectedItem.ToString() = "مرتجع مجهز" Then
            baseQuery = "SELECT sf.id as 'id', sf.worder_id AS 'أمر شغل ', sf.batch_no AS 'رقم الرسالة', sf.client_code AS 'عميل', " & _
                        "sf.roll AS 'رقم التوب', SUM(ss.height) AS 'الطول رصيد', SUM(ss.weight) AS 'الوزن رصيد', " & _
                        "sf.fabric_grade AS 'درجه القماش', sf.color AS 'اللون', sf.product_name AS 'الخامة', CAST(0 AS Decimal(10,2)) AS 'حركه متر',CAST(0 AS Decimal(10,2)) AS 'حركه وزن' " & _
                        "FROM sample_finish ss LEFT JOIN store_finish sf ON ss.storefinishid = sf.id"
        End If

        ' Add filters based on user input
        If Not String.IsNullOrEmpty(worderIds) Then
            ' Handle multiple worder IDs
            Dim worderIdList As String() = worderIds.Split(","c).Select(Function(id) id.Trim()).ToArray()
            Dim worderIdParameters As String = String.Join(",", worderIdList.Select(Function(id, index) "@worder_id" & index))
            filters.Add("worder_id IN (" & worderIdParameters & ")")
        End If

        If Not String.IsNullOrEmpty(productName) Then
            filters.Add("product_name = @product_name")
        End If
        If Not String.IsNullOrEmpty(clientCode) Then
            filters.Add("client_code = @client_code")
        End If
        If Not String.IsNullOrEmpty(batchNo) Then
            filters.Add("batch_no = @batch_no")
        End If
        If Not String.IsNullOrEmpty(color) Then
            filters.Add("color = @color")
        End If

        ' Combine query with filters
        If filters.Count > 0 Then
            baseQuery &= " WHERE " & String.Join(" AND ", filters)
        End If

        ' Additional grouping for "مرتجع مجهز"
        If cmbKindTrans.SelectedItem.ToString() = "مرتجع مجهز" Then
            baseQuery &= " GROUP BY sf.id, sf.worder_id, sf.batch_no, sf.client_code, sf.roll, sf.fabric_grade, sf.color, sf.product_name"
        End If

        ' Execute the query and load data
        Try
            Using connection As New SqlConnection(sqlServerConnectionString)
                connection.Open()
                Dim command As New SqlCommand(baseQuery, connection)

                ' Add parameters for multiple worder IDs
                If Not String.IsNullOrEmpty(worderIds) Then
                    Dim worderIdList As String() = worderIds.Split(","c).Select(Function(id) id.Trim()).ToArray()
                    For index As Integer = 0 To worderIdList.Length - 1
                        command.Parameters.AddWithValue("@worder_id" & index, worderIdList(index))
                    Next
                End If

                ' Add other parameters if needed
                If Not String.IsNullOrEmpty(productName) Then
                    command.Parameters.AddWithValue("@product_name", productName)
                End If
                If Not String.IsNullOrEmpty(clientCode) Then
                    command.Parameters.AddWithValue("@client_code", clientCode)
                End If
                If Not String.IsNullOrEmpty(batchNo) Then
                    command.Parameters.AddWithValue("@batch_no", batchNo)
                End If
                If Not String.IsNullOrEmpty(color) Then
                    command.Parameters.AddWithValue("@color", color)
                End If

                Dim adapter As New SqlDataAdapter(command)
                Dim dataTable As New DataTable()
                adapter.Fill(dataTable)
                DataGridView1.DataSource = dataTable
                AddCheckBoxColumn()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading data: " & ex.Message)
        End Try
    End Sub
    Private Sub cmbKindTrans_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbKindTrans.SelectedIndexChanged
        PopulateProductNames()
        Populateclientcode()
        Populatebatchno()
        Populatecolor()
    End Sub
    Private Sub PopulateProductNames()
        Try
            Dim query As String = ""
            If cmbKindTrans.SelectedItem.ToString() = "صرف مجهز" Then
                query = "SELECT DISTINCT product_name FROM store_finish"
            ElseIf cmbKindTrans.SelectedItem.ToString() = "مرتجع مجهز" Then
                query = "SELECT DISTINCT sf.product_name FROM sample_finish ss " &
                        "LEFT JOIN store_finish sf ON ss.storefinishid = sf.id"
            End If

            Using connection As New SqlConnection(sqlServerConnectionString)
                connection.Open()
                Dim cmd As New SqlCommand(query, connection)
                Dim reader As SqlDataReader = cmd.ExecuteReader()

                CmbProductName.Items.Clear()
                While reader.Read()
                    CmbProductName.Items.Add(reader("product_name").ToString())
                End While
                reader.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading product names: " & ex.Message)
        End Try
    End Sub
    Private Sub Populateclientcode()
        Try
            Dim query As String = ""

            ' Use cmbKindTrans.SelectedItem instead of cmbclient.SelectedItem
            If cmbKindTrans.SelectedItem IsNot Nothing AndAlso cmbKindTrans.SelectedItem.ToString() = "صرف مجهز" Then
                query = "SELECT DISTINCT client_code FROM store_finish"
            ElseIf cmbKindTrans.SelectedItem IsNot Nothing AndAlso cmbKindTrans.SelectedItem.ToString() = "مرتجع مجهز" Then
                query = "SELECT DISTINCT sf.client_code FROM sample_finish ss " &
                        "LEFT JOIN store_finish sf ON ss.storefinishid = sf.id"
            Else
                MessageBox.Show("Please select a valid transaction type.")
                Return
            End If

            Using connection As New SqlConnection(sqlServerConnectionString)
                connection.Open()
                Dim cmd As New SqlCommand(query, connection)
                Dim reader As SqlDataReader = cmd.ExecuteReader()

                cmbclient.Items.Clear() ' Clear previous items
                While reader.Read()
                    cmbclient.Items.Add(reader("client_code").ToString())
                End While
                reader.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading client codes: " & ex.Message)
        End Try
    End Sub
    Private Sub Populatebatchno()
        Try
            Dim query As String = ""

            ' Correctly check cmbKindTrans for the transaction type
            If cmbKindTrans.SelectedItem IsNot Nothing AndAlso cmbKindTrans.SelectedItem.ToString() = "صرف مجهز" Then
                query = "SELECT DISTINCT batch_no FROM store_finish"
            ElseIf cmbKindTrans.SelectedItem IsNot Nothing AndAlso cmbKindTrans.SelectedItem.ToString() = "مرتجع مجهز" Then
                query = "SELECT DISTINCT sf.batch_no FROM sample_finish ss " &
                        "LEFT JOIN store_finish sf ON ss.storefinishid = sf.id"
            Else
                MessageBox.Show("Please select a valid transaction type.")
                Return
            End If

            Using connection As New SqlConnection(sqlServerConnectionString)
                connection.Open()
                Dim cmd As New SqlCommand(query, connection)
                Dim reader As SqlDataReader = cmd.ExecuteReader()

                cmbbatch.Items.Clear() ' Clear previous items
                While reader.Read()
                    cmbbatch.Items.Add(reader("batch_no").ToString()) ' Ensure column name matches database
                End While
                reader.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading batch numbers: " & ex.Message)
        End Try
    End Sub
    Private Sub Populatecolor()
        Try
            Dim query As String = ""

            ' Correctly check cmbKindTrans for the transaction type
            If cmbKindTrans.SelectedItem IsNot Nothing AndAlso cmbKindTrans.SelectedItem.ToString() = "صرف مجهز" Then
                query = "SELECT DISTINCT color FROM store_finish"
            ElseIf cmbKindTrans.SelectedItem IsNot Nothing AndAlso cmbKindTrans.SelectedItem.ToString() = "مرتجع مجهز" Then
                query = "SELECT DISTINCT sf.color FROM sample_finish ss " &
                        "LEFT JOIN store_finish sf ON ss.storefinishid = sf.id"
            Else
                MessageBox.Show("Please select a valid transaction type.")
                Return
            End If

            Using connection As New SqlConnection(sqlServerConnectionString)
                connection.Open()
                Dim cmd As New SqlCommand(query, connection)
                Dim reader As SqlDataReader = cmd.ExecuteReader()

                cmbcolor.Items.Clear() ' Clear previous items
                While reader.Read()
                    cmbcolor.Items.Add(reader("color").ToString()) ' Ensure column name matches database
                End While
                reader.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading color: " & ex.Message)
        End Try
    End Sub
   ' Method to add a checkbox column to the DataGridView
    Private Sub AddCheckBoxColumn()
        If DataGridView1.Columns.Contains("SelectColumn") Then Return

        Dim checkBoxColumn As New DataGridViewCheckBoxColumn() With {
            .Name = "SelectColumn",
            .HeaderText = "اختر",
            .Width = 50
        }
        DataGridView1.Columns.Insert(0, checkBoxColumn)
    End Sub
    Private Sub CustomizeDataGridView()
        ' توسيط النصوص داخل الأعمدة
        For Each column As DataGridViewColumn In DataGridView1.Columns
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            column.DefaultCellStyle.WrapMode = DataGridViewTriState.True
            column.DefaultCellStyle.Font = New Font("Arial", 12, FontStyle.Bold)
        Next

        ' ضبط عرض الأعمدة تلقائيًا حسب المحتوى
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells

        ' تغيير ألوان العناوين
        DataGridView1.EnableHeadersVisualStyles = False
        DataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue ' اختر اللون المناسب
        DataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black
        DataGridView1.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 12, FontStyle.Bold)
        DataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
    End Sub
    Private Sub btnSubmit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSubmit.Click
        Try
            ' Check if cmbtoorfrom has a selected item
            If cmbtoorfrom.SelectedIndex = -1 Then
                MessageBox.Show("من فضلك اختر الادارة او القسم ")
                Return
            End If

            ' Check if txtnotes is empty
            If String.IsNullOrEmpty(txtnotes.Text) Then
                MessageBox.Show("Please enter notes.")
                Return
            End If

            Dim selectedRows As New List(Of Tuple(Of Integer, Double, Double))()
            Dim kindTrans As String = cmbKindTrans.SelectedItem.ToString()
            Dim prefix As String = If(kindTrans = "مرتجع مجهز", "WMR-", "WMS-")

            ' جمع الصفوف المحددة من DataGridView
            For Each row As DataGridViewRow In DataGridView1.Rows
                If Convert.ToBoolean(row.Cells("SelectColumn").Value) Then
                    Dim id As Integer = Convert.ToInt32(row.Cells("id").Value)
                    Dim motionMeter As Double = Convert.ToDouble(row.Cells("حركه متر").Value)
                    Dim motionWeight As Double = Convert.ToDouble(row.Cells("حركه وزن").Value)

                    ' التحقق من وجود قيم صالحة
                    If motionMeter <= 0 Or motionWeight <= 0 Then
                        MessageBox.Show("Please enter valid values for motion meter and weight.")
                        Return
                    End If

                    selectedRows.Add(Tuple.Create(id, motionMeter, motionWeight))
                End If
            Next

            ' التحقق من اختيار صف واحد على الأقل
            If selectedRows.Count = 0 Then
                MessageBox.Show("من فضلك حدد التوب")
                Return
            End If

            ' تحقق من الكميات المتوفرة قبل الموافقة على العملية
            Using connection As New SqlConnection(sqlServerConnectionString)
                connection.Open()
                For Each row In selectedRows
                    Dim id As Integer = row.Item1
                    Dim motionMeter As Double = row.Item2
                    Dim motionWeight As Double = row.Item3
                    Dim availableHeight As Double = 0
                    Dim availableWeight As Double = 0

                    If kindTrans = "صرف مجهز" Then
                        ' تحقق من الكمية المتوفرة في store_finish (صرف مجهز)
                        Dim checkQuery As String = "SELECT height, weight FROM store_finish WHERE store_finish.id = @id"
                        Using checkCommand As New SqlCommand(checkQuery, connection)
                            checkCommand.Parameters.AddWithValue("@id", id)
                            Using reader As SqlDataReader = checkCommand.ExecuteReader()
                                If reader.Read() Then
                                    availableHeight = Convert.ToDouble(reader("height"))
                                    availableWeight = Convert.ToDouble(reader("weight"))
                                End If
                            End Using
                        End Using

                        ' تحقق من أن الكمية المطلوبة أقل من أو تساوي الكمية المتوفرة
                        If motionMeter > availableHeight Or motionWeight > availableWeight Then
                            MessageBox.Show("كميه الصرف أكبر من الكميه الفعليه")
                            Return
                        End If
                    ElseIf kindTrans = "مرتجع مجهز" Then
                        ' Check available stock in sample_finish for Return
                        Dim checkQuery As String = "SELECT SUM(height) AS totalHeight, SUM(weight) AS totalWeight FROM sample_finish WHERE storefinishid = @id"
                        Using checkCommand As New SqlCommand(checkQuery, connection)
                            checkCommand.Parameters.AddWithValue("@id", id)
                            Using reader As SqlDataReader = checkCommand.ExecuteReader()
                                If reader.Read() Then
                                    availableHeight = Convert.ToDouble(reader("totalHeight"))
                                    availableWeight = Convert.ToDouble(reader("totalWeight"))
                                End If
                            End Using
                        End Using

                        ' Ensure requested quantity does not exceed available stock
                        If motionMeter > availableHeight Or motionWeight > availableWeight Then
                            MessageBox.Show("كميه المرتجع أكبر من الكميه الفعليه")
                            Return
                        End If
                    End If
                Next

                ' إذا كانت جميع الفحوصات صحيحة، استكمل العملية
                Dim transaction As SqlTransaction = connection.BeginTransaction()

                ' الحصول على الرقم التسلسلي التالي
                Dim nextSequenceNumber As String = GetNextSequenceNumber(connection, transaction, prefix)

                ' إجراء التحديث والإدخال في الجداول
                For Each row In selectedRows
                    Dim id As Integer = row.Item1
                    Dim motionMeter As Double = row.Item2
                    Dim motionWeight As Double = row.Item3

                    ' إذا كانت العملية "صرف"، يتم خصم القيم بالسالب
                    If kindTrans = "صرف مجهز" Then
                        motionMeter = -motionMeter
                        motionWeight = -motionWeight
                    End If

                    ' تحديث جدول store_finish
                    Dim updateQuery As String = "UPDATE store_finish SET height = height + @motionMeter, weight = weight + @motionWeight WHERE id = @id"
                    Using updateCommand As New SqlCommand(updateQuery, connection, transaction)
                        updateCommand.Parameters.AddWithValue("@motionMeter", motionMeter)
                        updateCommand.Parameters.AddWithValue("@motionWeight", motionWeight)
                        updateCommand.Parameters.AddWithValue("@id", id)
                        updateCommand.ExecuteNonQuery()
                    End Using

                    ' إدخال البيانات في sample_finish
                    Console.WriteLine("Inserting Date: " & DateTime.Now.ToString()) ' Log Date

                    Dim insertQuery As String = "INSERT INTO sample_finish (storefinishid, height, weight, kind_trans, refsample_no, to_or_from, notes, date, Username) " & _
                                                "VALUES (@id, @motionMeter, @motionWeight, @kindTrans, @refsample_no, @toOrFrom, @notes, @date, @Username)"

                    Using insertCommand As New SqlCommand(insertQuery, connection, transaction)
                        insertCommand.Parameters.AddWithValue("@id", id)

                        ' تسجيل الصرف بالموجب والمرتجع بالسالب
                        If kindTrans = "صرف مجهز" Then
                            insertCommand.Parameters.AddWithValue("@motionMeter", Math.Abs(motionMeter)) ' القيم موجبة
                            insertCommand.Parameters.AddWithValue("@motionWeight", Math.Abs(motionWeight)) ' القيم موجبة
                        ElseIf kindTrans = "مرتجع مجهز" Then
                            insertCommand.Parameters.AddWithValue("@motionMeter", -Math.Abs(motionMeter)) ' القيم سالبة
                            insertCommand.Parameters.AddWithValue("@motionWeight", -Math.Abs(motionWeight)) ' القيم سالبة
                        End If

                        insertCommand.Parameters.AddWithValue("@kindTrans", kindTrans)
                        insertCommand.Parameters.AddWithValue("@refsample_no", nextSequenceNumber)
                        insertCommand.Parameters.AddWithValue("@toOrFrom", cmbtoorfrom.SelectedItem.ToString()) ' Insert selected item from cmbtoorfrom
                        insertCommand.Parameters.AddWithValue("@notes", txtnotes.Text) ' Insert text from txtnotes
                        insertCommand.Parameters.AddWithValue("@date", DateTime.Now) ' This is the date parameter
                        insertCommand.Parameters.AddWithValue("@username", LoggedInUsername)

                        ' Log the inserted date value for debugging
                        Console.WriteLine("Inserting Date Parameter: " & DateTime.Now.ToString())

                        insertCommand.ExecuteNonQuery()
                    End Using

                Next

                ' إنهاء المعاملة
                transaction.Commit()

                ' Display the success message including the reference number
                MessageBox.Show("تم تسجيل الإذن برقم " & nextSequenceNumber)
                Cmbproductname.SelectedIndex = -1
                cmbclient.SelectedIndex = -1
                cmbbatch.SelectedIndex = -1
                cmbtoorfrom.SelectedIndex = -1
                txtnotes.Clear()
                txtworderid.Clear() ' Clears the TextBox
                DataGridView1.DataSource = Nothing  ' This clears the grid's data

            End Using
        Catch ex As Exception
            MessageBox.Show("Error during submission: " & ex.Message)
        End Try
    End Sub


    ' Method to get the next sequence number
    Private Function GetNextSequenceNumber(ByVal connection As SqlConnection, ByVal transaction As SqlTransaction, ByVal prefix As String) As String
        Dim query As String = "SELECT MAX(refsample_no) FROM sample_finish WHERE refsample_no LIKE @prefix"
        Using command As New SqlCommand(query, connection, transaction)
            command.Parameters.AddWithValue("@prefix", prefix & "%")
            Dim result As Object = command.ExecuteScalar()
            Dim nextNumber As Integer = 1

            If result IsNot DBNull.Value AndAlso result IsNot Nothing Then
                Dim lastSequence As String = result.ToString().Substring(prefix.Length)
                nextNumber = Convert.ToInt32(lastSequence) + 1
            End If

            Return prefix & nextNumber.ToString("D6")
        End Using
    End Function
    ' Event handler for View button click
    Private Sub btnView_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnView.Click

        Dim viewForm As New finishdisbursmentviewform()
        viewForm.Show()
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
