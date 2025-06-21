Imports MySql.Data.MySqlClient
Imports System.Data.SqlClient
Imports Excel = Microsoft.Office.Interop.Excel ' For Excel interop
Imports OfficeOpenXml ' Add this line for EPPlus
Imports System.IO
Imports System.Net
Imports System.Net.Mail
Imports System.Text

Public Class technicallibqconpoForm
    Private sqlServerConnectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"
    Private mysqlServerConnectionString As String = "Server=150.1.1.7;Database=wm;Uid=root1;Pwd=WMg2024$;"

    Private Sub technicallibqconpoForm_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        LoadWorderIDs()
        lblusername.Text = "Logged in as: " & LoggedInUsername
        ' Retrieve the public_name for the logged-in user
        Dim publicName As String = GetPublicName(LoggedInUsername)

    End Sub
    Private Sub LoadLibraryCodes()
        Dim query As String = "SELECT DISTINCT code FROM lib"

        Using conn As New SqlConnection(sqlServerConnectionString)
            Dim cmd As New SqlCommand(query, conn)
            Try
                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()

                Dim autoCompleteCollection As New AutoCompleteStringCollection()
                cmbcodelib.Items.Clear()
                While reader.Read()
                    Dim code As String = reader("code").ToString()
                    cmbcodelib.Items.Add(code)
                    autoCompleteCollection.Add(code)
                End While

                ' Set up the ComboBox for auto-complete
                cmbcodelib.AutoCompleteMode = AutoCompleteMode.SuggestAppend
                cmbcodelib.AutoCompleteSource = AutoCompleteSource.CustomSource
                cmbcodelib.AutoCompleteCustomSource = autoCompleteCollection

            Catch ex As Exception
                MessageBox.Show("Error loading library codes: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Sub cmbcodelib_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbcodelib.SelectedIndexChanged
        LoadProcessStages(cmbcodelib.Text)
        DisplayLibraryCodeId(cmbcodelib.Text)
    End Sub
    Private Sub DisplayLibraryCodeId(ByVal code As String)

        Dim query As String = "SELECT distinct code_id FROM lib WHERE code = @code"

        Using conn As New SqlConnection(sqlServerConnectionString)
            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@code", code)
            Try
                conn.Open()
                Dim result = cmd.ExecuteScalar()
                If result IsNot Nothing Then
                    lbllibcode.Text = result.ToString()

                Else
                    lbllibcode.Text = "N/A"

                End If
            Catch ex As Exception
                MessageBox.Show("Error loading library code ID: " & ex.Message)
            End Try
        End Using
    End Sub
    Private Sub LoadProcessStages(ByVal code As String)
        Dim query As String = "SELECT np.proccess_ar, nm.name_ar as machine_name FROM new_proccess np " &
                             "LEFT JOIN lib l ON np.id = l.proccess_id " &
                             "LEFT JOIN new_machines nm ON np.machine_id = nm.id " &
                             "WHERE l.code = @code"

        Using conn As New SqlConnection(sqlServerConnectionString)
            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@code", code)
            Try
                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()

                dgvLibCode.Rows.Clear()
                dgvLibCode.Columns.Clear()
                dgvLibCode.Columns.Add("ProcessStage", "مرحلة العملية")
                dgvLibCode.Columns.Add("Machine", "الماكينة")
                dgvLibCode.Columns.Add("Proccess Code", "كود العمليه")
                dgvLibCode.Columns.Add("l\kg", "l\kg")

                While reader.Read()
                    Dim processStage As String = reader("proccess_ar").ToString()
                    Dim machineName As String = If(reader("machine_name") IsNot DBNull.Value, reader("machine_name").ToString(), "")
                    dgvLibCode.Rows.Add(processStage, machineName, String.Empty, String.Empty)
                End While

                ' Set columns to autosize based on content
                dgvLibCode.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
                ' Set the Proccess Code column to fill the remaining space
                dgvLibCode.Columns("Proccess Code").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill

                ' Add ComboBox to rows where machine name contains 'jet'
                For Each row As DataGridViewRow In dgvLibCode.Rows
                    Dim machineName As String = row.Cells("Machine").Value.ToString()
                    If machineName.ToLower().Contains("jet") Then
                        Dim cmb As New DataGridViewComboBoxCell()
                        cmb.DataSource = GetCodesFromDatabase()
                        row.Cells("Proccess Code") = cmb

                        ' Make l/kg cell editable for jet machines
                        row.Cells("l\kg").ReadOnly = False
                    Else
                        ' Make l/kg cell read-only for non-jet machines
                        row.Cells("l\kg").ReadOnly = True
                    End If
                Next

            Catch ex As Exception
                MessageBox.Show("Error loading process stages: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Function GetCodesFromDatabase() As List(Of String)
        Dim codes As New List(Of String)
        Dim query As String = "SELECT code FROM codes where kind='d'"

        Using conn As New SqlConnection(sqlServerConnectionString)
            Dim cmd As New SqlCommand(query, conn)
            Try
                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                While reader.Read()
                    codes.Add(reader("code").ToString())
                End While
            Catch ex As Exception
                MessageBox.Show("Error loading codes: " & ex.Message)
            End Try
        End Using

        Return codes
    End Function

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
    Private Sub cmbworder_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbworder.SelectedIndexChanged
        ' Clear previous data before loading new data
        lblbatchno.Text = "الرسالة: N/A"
        lblcontractno.Text = "رقم التعاقد: N/A"
        lblcontractid.Text = "ID: N/A"
        lblkindcontract.Text = "نوع التعاقد: N/A"
        lblstatus.Text = String.Empty ' Clear status label
        lbloldcode.Text = String.Empty ' Clear old code label
        lblqty.Text = "الكمية: N/A" ' Clear quantity label
        lblweightcalc.Text = "حساب الوزن: N/A" ' Clear weight calculation label
        dgvoldcode.Rows.Clear()
        dgvoldcode.Columns.Clear()

        ' Add columns for lib_code and speed
        dgvoldcode.Columns.Add("LibCode", "المكتبة")
        dgvoldcode.Columns.Add("Speed", "السرعة")

        ' Set columns to autosize based on content
        dgvoldcode.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells

        ' Set font to bold, size 12 and align content to center
        Dim boldFont As New Font("Microsoft Sans Serif", 8, FontStyle.Bold)
        dgvoldcode.Columns("LibCode").DefaultCellStyle.Font = boldFont
        dgvoldcode.Columns("Speed").DefaultCellStyle.Font = boldFont
        dgvoldcode.Columns("LibCode").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvoldcode.Columns("Speed").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        ' Set header style
        Dim headerFont As New Font("Microsoft Sans Serif", 8, FontStyle.Bold)
        dgvoldcode.ColumnHeadersDefaultCellStyle.Font = headerFont
        dgvoldcode.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        Dim query As String = "SELECT f.fabrictype_ar, td.worderid, c.contractno, c.batch, c.ContractID, td.code_lib,td.new_code_lib, library.code, library.lib_code, speedproccess.speed, td.qty_m, c.lot, qrt.raw_befor_width, qrt.raw_befor_weight " &
                         "FROM techdata td " &
                         "LEFT JOIN Contracts c ON td.contract_id = c.ContractID " &
                         "LEFT JOIN fabric f ON c.ContractType = f.id " &
                         "LEFT JOIN library ON td.code_lib = library.id " &
                         "LEFT JOIN speedproccess ON td.worderid = speedproccess.worderid " &
                         "LEFT JOIN qc_raw_test qrt ON c.lot = qrt.lot " &
                         "WHERE td.worderid = @worderid"

        ' Connection and command
        Using conn As New SqlConnection(sqlServerConnectionString)
            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@worderid", cmbworder.Text) ' Use Text instead of SelectedItem
            Try
                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()

                If reader.Read() Then
                    ' Update label values
                    lblbatchno.Text = "Batch: " & If(reader("batch") IsNot DBNull.Value, reader("batch").ToString(), "N/A")
                    lblcontractno.Text = "Contract: " & If(reader("contractno") IsNot DBNull.Value, reader("contractno").ToString(), "N/A")
                    lblcontractid.Text = If(reader("ContractID") IsNot DBNull.Value, reader("ContractID").ToString(), "N/A")
                    lblkindcontract.Text = "Type: " & If(reader("fabrictype_ar") IsNot DBNull.Value, reader("fabrictype_ar").ToString(), "N/A")

                    ' Display quantity if work order starts with 'w'
                    If cmbworder.Text.StartsWith("w", StringComparison.OrdinalIgnoreCase) Then
                        Dim qty As Double
                        If Double.TryParse(If(reader("qty_m") IsNot DBNull.Value, reader("qty_m").ToString(), "0"), qty) Then
                            lblqty.Text = "الكمية: " & qty.ToString()

                            ' Get raw_befor_width and raw_befor_weight
                            Dim rawBeforeWidth As Double
                            Dim rawBeforeWeight As Double
                            If Double.TryParse(If(reader("raw_befor_width") IsNot DBNull.Value, reader("raw_befor_width").ToString(), "0"), rawBeforeWidth) AndAlso
                               Double.TryParse(If(reader("raw_befor_weight") IsNot DBNull.Value, reader("raw_befor_weight").ToString(), "0"), rawBeforeWeight) Then
                                ' Calculate weight using the formula
                                Dim weight As Double = qty * rawBeforeWidth * rawBeforeWeight / 100000
                                lblweightcalc.Text = $"حساب الوزن: {weight:F2}"
                            End If
                        End If
                    End If

                    ' Check for lib_code or new_code_lib and update status
                    If reader("new_code_lib") IsNot DBNull.Value Then
                        lblstatus.Text = "تم تسجيل المكتبة"
                        lblstatus.ForeColor = Color.Green
                    Else
                        lblstatus.Text = "لم يتم تسجيل المكتبة"
                        lblstatus.ForeColor = Color.Red
                    End If

                    ' Display code in lbloldcode
                    lbloldcode.Text = If(reader("code") IsNot DBNull.Value, reader("code").ToString(), String.Empty)
                    ' Process lib_code and speed data
                    Dim libCode As String = If(reader("lib_code") IsNot DBNull.Value, reader("lib_code").ToString(), "")
                    Dim speed As String = If(reader("speed") IsNot DBNull.Value, reader("speed").ToString(), "")

                    ' Split libCode and speed strings by "-"
                    Dim libCodeItems As String() = libCode.Split("-"c)
                    Dim speedItems As String() = speed.Split("-"c)

                    ' Ensure both arrays have the same length before displaying
                    Dim maxLength As Integer = Math.Max(libCodeItems.Length, speedItems.Length)

                    ' Loop through each item and add as rows
                    For i As Integer = 0 To maxLength - 1
                        Dim libCodeValue As String = If(i < libCodeItems.Length, libCodeItems(i), "")
                        Dim speedValue As String = If(i < speedItems.Length, speedItems(i), "")
                        dgvoldcode.Rows.Add(libCodeValue, speedValue)
                    Next
                End If

                LoadQC2Data(cmbworder.Text)
                FilterLibraryCodes(cmbworder.Text)

                ' Get the lot value from dgvsales
                Dim lot As String = String.Empty
                If dgvsales.Rows.Count > 0 AndAlso dgvsales.Columns.Contains("lot") Then
                    lot = dgvsales.Rows(0).Cells("lot").Value.ToString()
                End If

                ' Load QC1 data using the lot value
                LoadQC1DataFromSQL(lot)

            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Sub FilterLibraryCodes(ByVal worderid As String)
        If String.IsNullOrEmpty(worderid) Then
            Return
        End If

        Dim prefix As String = worderid.Substring(0, 1) ' Get the first letter of the work order ID

        Dim query As String = "SELECT DISTINCT code FROM lib WHERE code LIKE @prefix + '%' ORDER BY code"
        Using conn As New SqlConnection(sqlServerConnectionString)
            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@prefix", prefix)
            Try
                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()

                cmbcodelib.Items.Clear()
                Dim autoCompleteCollection As New AutoCompleteStringCollection()
                While reader.Read()
                    Dim code As String = reader("code").ToString()
                    cmbcodelib.Items.Add(code)
                    autoCompleteCollection.Add(code)
                End While

                ' Set up the ComboBox for auto-complete
                cmbcodelib.AutoCompleteMode = AutoCompleteMode.SuggestAppend
                cmbcodelib.AutoCompleteSource = AutoCompleteSource.CustomSource
                cmbcodelib.AutoCompleteCustomSource = autoCompleteCollection


            Catch ex As Exception
                MessageBox.Show("Error filtering library codes: " & ex.Message)
            End Try
        End Using
    End Sub
    Private Sub lblcontractid_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles lblcontractid.TextChanged
        ' Check if lblcontractid has a valid value
        If lblcontractid.Text <> "ID: N/A" Then
            ' Get the contract ID from lblcontractid
            Dim contractid As String = lblcontractid.Text.Replace("ID: ", "")

            ' SQL Query to fetch data based on the contract ID
            Dim query As String = "SELECT c.ContractID as 'ID', c.ContractNo as 'رقم التعاقد', f.fabricType_ar as 'نوع التعاقد', " &
                                  "c.ContractDate as 'تاريخ التعاقد', cl.code as 'كود العميل', c.color,c.Material as 'الخامة', " &
                                  "c.refno as 'رقم الاذن', c.Batch as 'رقم الرساله', c.lot,c.QuantityM as 'الكمية متر', " &
                                  "c.QuantityK as 'الكمية كيلو', c.FabricCode as 'كود الخامه', c.WidthReq as 'العرض المطلوب', " &
                                  "c.WeightM as 'الوزن المطلوب', c.RollM as 'أمتار التوب المطلوبة', c.Notes as 'ملاحظات' " &
                                  "FROM Contracts c LEFT JOIN clients cl ON c.clientcode = cl.id " &
                                  "LEFT JOIN fabric f ON c.contracttype = f.id WHERE contractid = @contractid"

            ' Create a DataTable to hold the results for the DataGridView
            Dim dt As New DataTable()

            ' Connection and command
            Using conn As New SqlConnection(sqlServerConnectionString)
                Dim cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@contractid", contractid) ' Use the value from lblcontractid

                Try
                    conn.Open()
                    ' Fill the DataTable with data from the query
                    Dim adapter As New SqlDataAdapter(cmd)
                    adapter.Fill(dt)

                    ' Bind the DataTable to the DataGridView
                    dgvsales.DataSource = dt
                    dgvsales.Refresh() ' Ensure the DataGridView is redrawn after binding

                    ' Customize the DataGridView appearance

                    dgvsales.AutoResizeColumnHeadersHeight() ' Resize the headers

                    dgvsales.DefaultCellStyle.Font = New Font("Arial", 12, FontStyle.Bold) ' Set font to Bold and size 12
                    dgvsales.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter ' Center align content

                    ' Loop through each column and set header styles
                    For Each column As DataGridViewColumn In dgvsales.Columns
                        column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter ' Center header text
                        column.HeaderCell.Style.Font = New Font("Arial", 10, FontStyle.Bold) ' Set header font to bold
                        column.HeaderCell.Style.ForeColor = Color.White ' Set header font color to white
                        column.HeaderCell.Style.BackColor = Color.MidnightBlue ' Set header background color to midnight blue
                    Next

                    ' Adjust column widths based on content
                    For Each column As DataGridViewColumn In dgvsales.Columns
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                    Next

                Catch ex As Exception
                    MessageBox.Show("Error: " & ex.Message)
                End Try
            End Using
        End If
    End Sub
    Private Function GetSelectedQcId() As Integer
        For Each row As DataGridViewRow In dgvqc.Rows
            Dim isChecked As Boolean = Convert.ToBoolean(row.Cells("Select").Value)
            If isChecked Then
                Return Convert.ToInt32(row.Cells("id").Value) ' Return the qc2.id of the selected row
            End If
        Next
        ' If no record is selected, throw an exception
        Throw New Exception("No QC record selected.")
    End Function


    Private Sub LoadQC1DataFromSQL(ByVal lot As String)
        ' SQL Query to fetch data from the second table based on lot number
        Dim query As String = "SELECT id, batch_id as 'batch_no', lot, raw_befor_width as 'Raw before width', raw_after_width as 'Raw after width', " &
                          "raw_befor_weight as 'Weight of m2 before', raw_after_weight as 'Weight of m2 after', pva_Starch as 'pva / Starch', " &
                          "mix_rate as 'Mixing Percentage', tensile_weft as 'Rupture Weft', tensile_warp as 'Rupture Warp', " &
                          "tensile_result as 'Rupture Result', color_water as 'Color Fastness to water', tear_weft as 'Tear Weft', " &
                          "tear_warp as 'Tear Warp', tear_result as 'Tear Result', washing as 'Color Fastness for Washing', " &
                          "color_mercerize as 'Color Fastness for Mercerization', notes, username as 'user' " &
                          "FROM qc_raw_test WHERE lot = @lot"

        ' Create a DataTable to hold the results for the DataGridView
        Dim dt As New DataTable()

        ' Connection and command
        Using conn As New SqlConnection(sqlServerConnectionString)
            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@lot", lot) ' Use the lot number passed into the function

            Try
                conn.Open()
                ' Fill the DataTable with data from the query
                Dim adapter As New SqlDataAdapter(cmd)
                adapter.Fill(dt)

                ' Check if DataTable has rows and then populate the DataGridView
                If dt.Rows.Count > 0 Then
                    ' Clear existing columns
                    dgvqc.Columns.Clear()

                    ' Add checkbox column first
                    Dim chkColumn As New DataGridViewCheckBoxColumn()
                    chkColumn.Name = "Select"
                    chkColumn.HeaderText = "Select"
                    chkColumn.Width = 50
                    chkColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    dgvqc.Columns.Add(chkColumn)

                    ' Bind the DataTable to the DataGridView (dgvqc)
                    dgvqc.DataSource = dt
                    dgvqc.Refresh() ' Ensure the DataGridView is redrawn after binding

                    ' Customize the DataGridView appearance
                    dgvqc.AutoResizeColumnHeadersHeight() ' Resize the headers
                    dgvqc.DefaultCellStyle.Font = New Font("Arial", 12, FontStyle.Bold) ' Set font to Bold and size 12
                    dgvqc.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter ' Center align content

                    ' Loop through each column and set header styles
                    For Each column As DataGridViewColumn In dgvqc.Columns
                        column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter ' Center header text
                        column.HeaderCell.Style.Font = New Font("Arial", 10, FontStyle.Bold) ' Set header font to bold
                        column.HeaderCell.Style.ForeColor = Color.White ' Set header font color to white
                        column.HeaderCell.Style.BackColor = Color.MidnightBlue ' Set header background color to midnight blue
                    Next

                    ' Adjust column widths based on content
                    For Each column As DataGridViewColumn In dgvqc.Columns
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                    Next


                Else
                    dgvqc.DataSource = Nothing
                    MessageBox.Show("لم يتم عمل اختبارات من قبل معمل الجوده على هذه الرسالة")
                End If
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
    End Sub



    Private Sub LoadQC2Data(ByVal worderid As String)
        ' SQL Query to fetch data based on work order ID
        Dim query As String = "SELECT " &
                  "bd.pva_Starch AS 'PVA Starch', bd.needle AS 'Needle', bd.Separation AS 'Separation', " &
                  "bd.Durability AS 'Durability', bd.notes AS 'ملاحظات العيوب',bd.Raw_Moisture as 'الرطوبة' " &
                  "FROM techdata td " &
                  "LEFT JOIN contracts c ON td.contract_id = c.contractid " &
                  "LEFT JOIN batch_lot_defect bd ON c.lot = bd.lot " &
                  "WHERE td.worderid = @worderid"

        ' Create a DataTable to hold the results for the DataGridView
        Dim dt As New DataTable()

        ' Connection and command
        Using conn As New SqlConnection(sqlServerConnectionString)
            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@worderid", worderid) ' Use the work order ID passed into the function

            Try
                conn.Open()
                ' Fill the DataTable with data from the query
                Dim adapter As New SqlDataAdapter(cmd)
                adapter.Fill(dt)

                ' Check if DataTable has rows and then populate the DataGridView
                If dt.Rows.Count > 0 Then
                    ' Bind the DataTable to the DataGridView (dgvqc)
                    dgvdefects.DataSource = dt
                    dgvdefects.Refresh() ' Ensure the DataGridView is redrawn after binding

                    ' Customize the DataGridView appearance
                    dgvdefects.AutoResizeColumnHeadersHeight() ' Resize the headers
                    dgvdefects.DefaultCellStyle.Font = New Font("Arial", 12, FontStyle.Bold) ' Set font to Bold and size 12
                    dgvdefects.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter ' Center align content

                    ' Loop through each column and set header styles
                    For Each column As DataGridViewColumn In dgvdefects.Columns
                        column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter ' Center header text
                        column.HeaderCell.Style.Font = New Font("Arial", 10, FontStyle.Bold) ' Set header font to bold
                        column.HeaderCell.Style.ForeColor = Color.White ' Set header font color to white
                        column.HeaderCell.Style.BackColor = Color.MidnightBlue ' Set header background color to midnight blue
                    Next

                    ' Adjust column widths based on content
                    For Each column As DataGridViewColumn In dgvdefects.Columns
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                    Next
                Else
                    dgvdefects.DataSource = Nothing
                    MessageBox.Show("لم يتم تسجيل العيوب الخاصه بالرساله")
                End If
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
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
    Private Sub btnupdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnupdate.Click
        ' Ensure all required fields are filled
        If String.IsNullOrEmpty(cmbworder.Text) Then
            MessageBox.Show("Please select a Work Order ID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If String.IsNullOrEmpty(lbllibcode.Text) Then
            MessageBox.Show("Please select a Library Code.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Check if all jet machines have codes selected and l/kg values
        For Each row As DataGridViewRow In dgvLibCode.Rows
            Dim machineName As String = row.Cells("Machine").Value.ToString()
            If machineName.ToLower().Contains("jet") Then
                Dim cell As DataGridViewCell = row.Cells("Proccess Code")
                If TypeOf cell Is DataGridViewComboBoxCell Then
                    Dim comboCell As DataGridViewComboBoxCell = DirectCast(cell, DataGridViewComboBoxCell)
                    If comboCell.Value Is Nothing OrElse String.IsNullOrEmpty(comboCell.Value.ToString()) Then
                        MessageBox.Show("من فضلك اختر كود العمليه للماكينة " & machineName, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If
                End If

                ' Check if l/kg value is provided
                Dim lkgValue As String = row.Cells("l\kg").Value?.ToString()
                If String.IsNullOrEmpty(lkgValue) Then
                    MessageBox.Show("من فضلك ادخل قيمة l/kg للماكينة " & machineName, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                ' Validate l/kg is a number
                Dim lkgNumber As Double
                If Not Double.TryParse(lkgValue, lkgNumber) Then
                    MessageBox.Show("قيمة l/kg يجب ان تكون رقما للماكينة " & machineName, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If
            End If
        Next

        ' Convert lbllibcode to integer
        Dim codeLib As Integer
        If Not Integer.TryParse(lbllibcode.Text, codeLib) Then
            MessageBox.Show("Invalid Library Code format.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' Get the selected Work Order ID
        Dim worderid As String = cmbworder.Text

        ' Get the selected QC ID
        Dim selectedQcId As Integer = GetSelectedQcId()
        If selectedQcId <= 0 Then
            MessageBox.Show("No QC ID is selected. Please select a valid QC ID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Retrieve the id of the selected worder from techdata
        Dim techdataId As Integer
        Dim techdataIdQuery As String = "SELECT id FROM techdata WHERE worderid = @worderid"
        Using conn As New SqlConnection(sqlServerConnectionString)
            Using cmd As New SqlCommand(techdataIdQuery, conn)
                cmd.Parameters.AddWithValue("@worderid", worderid)
                Try
                    conn.Open()
                    techdataId = Convert.ToInt32(cmd.ExecuteScalar())
                Catch ex As Exception
                    MessageBox.Show("Error retrieving techdata ID: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End Try
            End Using
        End Using

        ' Check if qc_id or code_lib already have values
        Dim existsQuery As String = "SELECT new_qc_id, new_code_lib FROM techdata WHERE worderid = @worderid"
        Using conn As New SqlConnection(sqlServerConnectionString)
            Using cmd As New SqlCommand(existsQuery, conn)
                cmd.Parameters.AddWithValue("@worderid", worderid)
                Try
                    conn.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.HasRows Then
                            reader.Read()
                            Dim existingQcId As Object = reader("new_qc_id")
                            Dim existingCodeLib As Object = reader("new_code_lib")

                            ' Check if qc_id or code_lib already has values
                            If (existingQcId IsNot Nothing AndAlso Not DBNull.Value.Equals(existingQcId)) OrElse
                       (existingCodeLib IsNot Nothing AndAlso Not DBNull.Value.Equals(existingCodeLib)) Then
                                MessageBox.Show("The record already has values for QC ID or Code Library. Update is not allowed.", "Update Prevented", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                Return
                            End If
                        End If
                    End Using
                Catch ex As Exception
                    MessageBox.Show("Error checking for existing values: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End Try
            End Using
        End Using

        ' Proceed with the update if no existing values
        Dim updateQuery As String = "UPDATE techdata SET new_code_lib = @codeLib, new_qc_id = @qcId, user_tech = @userTech, date_lib = @datelib, notes = @notes" &
                                  If(worderid.StartsWith("w", StringComparison.OrdinalIgnoreCase), ", qty_kg = @qtyKg", "") &
                                  " WHERE worderid = @worderid"
        Using conn As New SqlConnection(sqlServerConnectionString)
            conn.Open()
            Dim transaction As SqlTransaction = conn.BeginTransaction()
            Try
                Using cmd As New SqlCommand(updateQuery, conn, transaction)
                    ' Add parameters
                    cmd.Parameters.AddWithValue("@codeLib", codeLib)
                    cmd.Parameters.AddWithValue("@qcId", selectedQcId)
                    cmd.Parameters.AddWithValue("@worderid", worderid)
                    cmd.Parameters.AddWithValue("@userTech", LoggedInUsername)
                    cmd.Parameters.AddWithValue("@datelib", DateTime.Now)
                    cmd.Parameters.AddWithValue("@notes", txtnotes.Text)

                    ' Get the weight calculation value from lblweightcalc only if work order starts with 'w'
                    If worderid.StartsWith("w", StringComparison.OrdinalIgnoreCase) Then
                        Dim weightCalcText As String = lblweightcalc.Text.Replace("حساب الوزن: ", "")
                        Dim qtyKg As Double = 0
                        If Double.TryParse(weightCalcText, qtyKg) Then
                            cmd.Parameters.AddWithValue("@qtyKg", qtyKg)
                        Else
                            cmd.Parameters.AddWithValue("@qtyKg", DBNull.Value)
                        End If
                    End If

                    Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
                    If rowsAffected > 0 Then
                        MessageBox.Show("Record updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

                        ' Insert data into qc_lab
                        Dim insertQuery As String = "INSERT INTO qc_lab (qc_id, batch_no, d1, d2, d3, d4, d5, d6, d7, d8, d9, d10, d11, d12, d13, d14, d15, d16, mix_rate, worder_id, idworder) " &
                                            "VALUES (@qc_id, @batch_no, @d1, @d2, @d3, @d4, @d5, @d6, @d7, @d8, @d9, @d10, @d11, @d12, @d13, @d14, @d15, @d16, @mix_rate, @worder_id, @idworder)"
                        For Each row As DataGridViewRow In dgvqc.Rows
                            If Not row.IsNewRow AndAlso Convert.ToBoolean(row.Cells("Select").Value) = True Then
                                Using cmdInsert As New SqlCommand(insertQuery, conn, transaction)
                                    cmdInsert.Parameters.AddWithValue("@qc_id", row.Cells("id").Value)
                                    cmdInsert.Parameters.AddWithValue("@batch_no", row.Cells("batch_no").Value)
                                    cmdInsert.Parameters.AddWithValue("@d1", row.Cells("Raw before width").Value)
                                    cmdInsert.Parameters.AddWithValue("@d2", row.Cells("Raw after width").Value)
                                    cmdInsert.Parameters.AddWithValue("@d3", row.Cells("Weight of m2 before").Value)
                                    cmdInsert.Parameters.AddWithValue("@d4", row.Cells("Weight of m2 after").Value)
                                    cmdInsert.Parameters.AddWithValue("@d5", row.Cells("pva / Starch").Value)
                                    cmdInsert.Parameters.AddWithValue("@d6", row.Cells("Mixing Percentage").Value)
                                    cmdInsert.Parameters.AddWithValue("@d7", row.Cells("Rupture Warp").Value)
                                    cmdInsert.Parameters.AddWithValue("@d8", row.Cells("Rupture Weft").Value)
                                    cmdInsert.Parameters.AddWithValue("@d9", row.Cells("Rupture Result").Value)
                                    cmdInsert.Parameters.AddWithValue("@d10", row.Cells("Color Fastness to water").Value)
                                    cmdInsert.Parameters.AddWithValue("@d11", row.Cells("Tear Warp").Value)
                                    cmdInsert.Parameters.AddWithValue("@d12", row.Cells("Tear Weft").Value)
                                    cmdInsert.Parameters.AddWithValue("@d13", row.Cells("Tear Result").Value)
                                    cmdInsert.Parameters.AddWithValue("@d14", row.Cells("Color Fastness for Washing").Value)
                                    cmdInsert.Parameters.AddWithValue("@d15", row.Cells("Color Fastness for Mercerization").Value)
                                    cmdInsert.Parameters.AddWithValue("@d16", row.Cells("Notes").Value)
                                    cmdInsert.Parameters.AddWithValue("@mix_rate", row.Cells("Mixing Percentage").Value)
                                    cmdInsert.Parameters.AddWithValue("@worder_id", cmbworder.Text)
                                    cmdInsert.Parameters.AddWithValue("@idworder", techdataId)
                                    cmdInsert.ExecuteNonQuery()
                                End Using
                            End If
                        Next

                        ' Insert layout codes data
                        Dim layoutCodeQuery As String = "INSERT INTO layout_codes (worderid, machineid, proccess_id, step_num, layout_code_id, lib_code_id, date, username, l_kg) " &
                                                      "VALUES (@worderid, @machineid, @proccess_id, @step_num, @layout_code_id, @lib_code_id, @date, @username, @l_kg)"

                        ' Get process stages and their IDs
                        Dim processStagesQuery As String = "SELECT np.id as proccess_id, np.proccess_ar, nm.id as machine_id, nm.name_ar " &
                                                         "FROM new_proccess np " &
                                                         "LEFT JOIN lib l ON np.id = l.proccess_id " &
                                                         "LEFT JOIN new_machines nm ON np.machine_id = nm.id " &
                                                         "WHERE l.code = @code"

                        Using cmdProcess As New SqlCommand(processStagesQuery, conn, transaction)
                            cmdProcess.Parameters.AddWithValue("@code", cmbcodelib.Text)
                            Using reader As SqlDataReader = cmdProcess.ExecuteReader()
                                Dim processStages As New List(Of (ProcessId As Integer, MachineId As Integer, StepNum As Integer))
                                Dim stepNum As Integer = 1

                                While reader.Read()
                                    processStages.Add((
                                        ProcessId:=Convert.ToInt32(reader("proccess_id")),
                                        MachineId:=If(reader("machine_id") IsNot DBNull.Value, Convert.ToInt32(reader("machine_id")), 0),
                                        StepNum:=stepNum))
                                    stepNum += 1
                                End While

                                reader.Close()

                                ' Insert layout codes for each process stage
                                For Each row As DataGridViewRow In dgvLibCode.Rows
                                    Dim stepIndex As Integer = row.Index
                                    If stepIndex < processStages.Count Then
                                        Dim processStage = processStages(stepIndex)
                                        Dim cell As DataGridViewCell = row.Cells("Proccess Code")

                                        If TypeOf cell Is DataGridViewComboBoxCell Then
                                            Dim comboCell As DataGridViewComboBoxCell = DirectCast(cell, DataGridViewComboBoxCell)
                                            If comboCell.Value IsNot Nothing Then
                                                ' Get layout_code_id from codes table
                                                Dim layoutCodeIdQuery As String = "SELECT id FROM codes WHERE code = @code"
                                                Using cmdLayoutCode As New SqlCommand(layoutCodeIdQuery, conn, transaction)
                                                    cmdLayoutCode.Parameters.AddWithValue("@code", comboCell.Value.ToString())
                                                    Dim layoutCodeId As Integer = Convert.ToInt32(cmdLayoutCode.ExecuteScalar())

                                                    ' Get l/kg value
                                                    Dim lkgValue As String = row.Cells("l\kg").Value?.ToString()
                                                    If String.IsNullOrEmpty(lkgValue) Then
                                                        lkgValue = "0"  ' Default to 0 if no value provided
                                                    End If

                                                    ' Check if layout code already exists for this work order, machine and process
                                                    Dim checkExistingQuery As String = "SELECT COUNT(*) FROM layout_codes " &
                                                                                      "WHERE worderid = @worderid AND machineid = @machineid AND proccess_id = @proccess_id"
                                                    Using cmdCheck As New SqlCommand(checkExistingQuery, conn, transaction)
                                                        cmdCheck.Parameters.AddWithValue("@worderid", worderid)
                                                        cmdCheck.Parameters.AddWithValue("@machineid", processStage.MachineId)
                                                        cmdCheck.Parameters.AddWithValue("@proccess_id", processStage.ProcessId)
                                                        Dim existingCount As Integer = Convert.ToInt32(cmdCheck.ExecuteScalar())

                                                        If existingCount > 0 Then
                                                            Throw New Exception($"تم تسجيل كود التخطيط من قبل لأمر الشغل {worderid} على الماكينة رقم {processStage.MachineId} في المرحلة {processStage.ProcessId}")
                                                        End If
                                                    End Using

                                                    ' Insert into layout_codes
                                                    Using cmdInsert As New SqlCommand(layoutCodeQuery, conn, transaction)
                                                        cmdInsert.Parameters.AddWithValue("@worderid", worderid)
                                                        cmdInsert.Parameters.AddWithValue("@machineid", processStage.MachineId)
                                                        cmdInsert.Parameters.AddWithValue("@proccess_id", processStage.ProcessId)
                                                        cmdInsert.Parameters.AddWithValue("@step_num", processStage.StepNum)
                                                        cmdInsert.Parameters.AddWithValue("@layout_code_id", layoutCodeId)
                                                        cmdInsert.Parameters.AddWithValue("@lib_code_id", codeLib)  ' Add lib_code_id
                                                        cmdInsert.Parameters.AddWithValue("@date", DateTime.Now)    ' Add current date and time
                                                        cmdInsert.Parameters.AddWithValue("@username", LoggedInUsername)  ' Add username
                                                        cmdInsert.Parameters.AddWithValue("@l_kg", lkgValue)  ' Add l_kg
                                                        cmdInsert.ExecuteNonQuery()
                                                    End Using
                                                End Using
                                            End If
                                        End If
                                    End If
                                Next
                            End Using
                        End Using

                        ' Commit transaction
                        transaction.Commit()

                        ' Fetch code and lib_code from the library table
                        Dim libraryCode As String = String.Empty
                        Dim libraryLibCode As String = String.Empty
                        Dim libraryQuery As String = "SELECT code, lib_code FROM library WHERE id = @id"
                        Using libCmd As New SqlCommand(libraryQuery, conn)
                            libCmd.Parameters.AddWithValue("@id", codeLib)
                            Using reader As SqlDataReader = libCmd.ExecuteReader()
                                If reader.Read() Then
                                    libraryCode = reader("code").ToString()
                                    libraryLibCode = reader("lib_code").ToString()
                                End If
                            End Using
                        End Using

                        ' Initialize the email body with HTML table structure
                        Dim emailBody As New StringBuilder()
                        emailBody.AppendLine($"<h2>تم تسجيل المكتبة على أمر شغل.</h2>")
                        emailBody.AppendLine("<h3>تفاصيل البيانات:</h3>")
                        emailBody.AppendLine("<table border='1' style='border-collapse: collapse; width: 100%;'>")
                        emailBody.AppendLine("<tr>")
                        emailBody.AppendLine("<th>رقم أمر الشغل</th>")
                        emailBody.AppendLine("<th>كود المكتبة</th>")
                        emailBody.AppendLine("<th>محتوى المكتبة</th>")
                        emailBody.AppendLine("<th>رقم QC</th>")
                        emailBody.AppendLine("<th>المستخدم</th>")
                        emailBody.AppendLine("<th>تاريخ التحديث</th>")
                        emailBody.AppendLine("</tr>")
                        emailBody.AppendLine("<tr>")
                        emailBody.AppendLine($"<td>{worderid}</td>")
                        emailBody.AppendLine($"<td>{libraryCode}</td>")
                        emailBody.AppendLine($"<td>{libraryLibCode}</td>")
                        emailBody.AppendLine($"<td>{selectedQcId}</td>")
                        emailBody.AppendLine($"<td>{LoggedInUsername}</td>")
                        emailBody.AppendLine($"<td>{DateTime.Now}</td>")
                        emailBody.AppendLine("</tr>")
                        emailBody.AppendLine("</table>")

                        ' Send email notification
                        Dim subject As String = "Data Updated Successfully"
                        Dim body As String = emailBody.ToString()
                        SendEmailNotification(subject, body)

                        ' Clear controls as required
                        cmbworder.Text = String.Empty
                        cmbworder.SelectedIndex = -1
                        dgvsales.DataSource = Nothing
                        dgvsales.Rows.Clear()
                        dgvqc.DataSource = Nothing
                        dgvqc.Rows.Clear()
                        dgvdefects.DataSource = Nothing
                        dgvdefects.Rows.Clear()
                        dgvLibCode.DataSource = Nothing
                        dgvLibCode.Rows.Clear()
                        cmbcodelib.Text = String.Empty
                        cmbcodelib.SelectedIndex = -1
                        lbllibcode.Text = String.Empty
                    End If
                End Using
            Catch ex As Exception
                transaction.Rollback()
                MessageBox.Show("Error updating the record: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub


    Private Sub SendEmailNotification(subject As String, body As String)
        Try
            Using conn As New SqlConnection(sqlServerConnectionString)
                conn.Open()

                ' Get email addresses from the mails table
                Dim query As String = "SELECT mail FROM mails"
                Dim cmd As New SqlCommand(query, conn)
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                Dim emailAddresses As New List(Of String)

                While reader.Read()
                    emailAddresses.Add(reader("mail").ToString())
                End While

                conn.Close()

                ' Send email
                Dim smtpClient As New SmtpClient("smtppro.zoho.com", 587) With {
                .Credentials = New NetworkCredential("it.app@moamen.com", "WMG@#$it$#@2024"),
                .EnableSsl = True
            }

                Dim mailMessage As New MailMessage() With {
                .From = New MailAddress("it.app@moamen.com"),
                .Subject = subject,
                .Body = body,
                .IsBodyHtml = True ' Enable HTML formatting
            }

                For Each mail As String In emailAddresses
                    mailMessage.To.Add(mail)
                Next

                smtpClient.Send(mailMessage)
            End Using
        Catch ex As SmtpException
            MessageBox.Show("SMTP error sending email notification: " & ex.Message & vbCrLf & "Status Code: " & ex.StatusCode)
        Catch ex As Exception
            MessageBox.Show("Error sending email notification: " & ex.Message)
        End Try
    End Sub

End Class
