Imports System.Data.SqlClient
Imports Mysqlx.XDevAPI.CRUD
Imports Excel = Microsoft.Office.Interop.Excel

Public Class trackingform
    Private sqlServerConnectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"

    Private Sub TrackingForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadTrackingData()
    End Sub

    Private Sub LoadTrackingData(Optional ByVal worderid As String = "")
        Dim query As String = "WITH LastStage AS (" &
  "SELECT pl.worderid, lib.code, lib.steps_num, np.proccess_ar, nm.name_ar, " &
  "clients.code AS client_code, td.Delivery_dat, pl.datein, " &
  "DATEDIFF(DAY, GETDATE(), td.Delivery_dat) AS days_to_delivery, " &
  "DATEDIFF(DAY, pl.datein, GETDATE()) AS stage_duration, " &
  "ROW_NUMBER() OVER (PARTITION BY pl.worderid ORDER BY lib.steps_num DESC) AS rn " &
  "FROM Planning pl " &
  "INNER JOIN techdata td ON pl.worderid = td.worderid " &
  "INNER JOIN lib lib ON td.new_code_lib = lib.code_id " &
  "INNER JOIN new_proccess np ON lib.proccess_id = np.id " &
  "INNER JOIN new_machines nm ON np.machine_id = nm.id " &
  "LEFT JOIN contracts ON td.contract_id = contracts.ContractID " &
  "LEFT JOIN clients ON contracts.ClientCode = clients.id " &
  "WHERE pl.proccessid = np.id AND np.id = lib.proccess_id AND td.new_code_lib IS NOT NULL) " &
  "SELECT ls.worderid, ls.client_code, ls.Delivery_dat, ls.days_to_delivery, ls.datein, ls.stage_duration, ls.code, ls.steps_num, ls.proccess_ar, ls.name_ar, " &
  "lib_next.steps_num AS next_steps_num, np_next.proccess_ar AS next_proccess_ar, nm_next.name_ar AS next_name_ar " &
  "FROM LastStage ls " &
  "LEFT JOIN lib lib_next ON ls.code = lib_next.code AND ls.steps_num + 1 = lib_next.steps_num " &
  "LEFT JOIN new_proccess np_next ON lib_next.proccess_id = np_next.id " &
  "LEFT JOIN new_machines nm_next ON np_next.machine_id = nm_next.id " &
  "WHERE ls.rn = 1 "

        ' Add condition based on CheckBox state
        If Not chkShowAll.Checked Then
            query &= "AND np_next.proccess_ar IS NOT NULL "
        End If

        query &= "UNION " &
  "SELECT td.worderid, clients.code AS client_code, td.Delivery_dat, " &
  "DATEDIFF(DAY, GETDATE(), td.Delivery_dat) AS days_to_delivery, NULL AS datein, NULL AS stage_duration, " &
  "lib.code, 1 AS steps_num, NULL AS proccess_ar, NULL AS name_ar, " &
  "lib.steps_num AS next_steps_num, np.proccess_ar AS next_proccess_ar, nm.name_ar AS next_name_ar " &
  "FROM techdata td " &
  "INNER JOIN lib ON td.new_code_lib = lib.code_id " &
  "INNER JOIN new_proccess np ON lib.proccess_id = np.id " &
  "INNER JOIN new_machines nm ON np.machine_id = nm.id " &
  "LEFT JOIN contracts ON td.contract_id = contracts.ContractID " &
  "LEFT JOIN clients ON contracts.ClientCode = clients.id " &
  "WHERE NOT EXISTS (SELECT 1 FROM Planning pl WHERE pl.worderid = td.worderid) " &
  "AND lib.steps_num = 1 AND td.new_code_lib IS NOT NULL"

        If Not String.IsNullOrEmpty(worderid) AndAlso worderid <> "All" Then
            query &= " AND td.worderid = @worderid"
        End If

        Using conn As New SqlConnection(sqlServerConnectionString)
            Dim cmd As New SqlCommand(query, conn)
            If Not String.IsNullOrEmpty(worderid) AndAlso worderid <> "All" Then
                cmd.Parameters.AddWithValue("@worderid", worderid)
            End If
            Dim adapter As New SqlDataAdapter(cmd)
            Dim dataTable As New DataTable()

            Try
                conn.Open()
                adapter.Fill(dataTable)

                If dataTable.Rows.Count = 0 Then
                    MessageBox.Show("No data found for the specified work order ID.")
                End If

                DataGridView1.DataSource = dataTable

                ' Populate ComboBoxes with distinct values
                PopulateCurrentMachineComboBox(dataTable)
                PopulateNextMachineComboBoxFromDataGridView() ' Update this call
                PopulateClientsComboBox(dataTable)
                PopulateWorderComboBox(dataTable)

                ' Reorder columns
                DataGridView1.Columns("worderid").DisplayIndex = 0
                DataGridView1.Columns("code").DisplayIndex = 1
                DataGridView1.Columns("client_code").DisplayIndex = 2
                DataGridView1.Columns("Delivery_dat").DisplayIndex = 3
                DataGridView1.Columns("days_to_delivery").DisplayIndex = 4
                DataGridView1.Columns("datein").DisplayIndex = 5
                DataGridView1.Columns("proccess_ar").DisplayIndex = 6
                DataGridView1.Columns("name_ar").DisplayIndex = 7
                DataGridView1.Columns("steps_num").DisplayIndex = 8
                DataGridView1.Columns("next_steps_num").DisplayIndex = 9
                DataGridView1.Columns("next_proccess_ar").DisplayIndex = 10
                DataGridView1.Columns("next_name_ar").DisplayIndex = 11
                DataGridView1.Columns("stage_duration").DisplayIndex = 12
            Catch ex As Exception
                MessageBox.Show("Error loading tracking data: " & ex.Message)
            End Try
        End Using
        FormatDataGridView()
    End Sub


    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        ' Call LoadTrackingData to refresh the data
        LoadTrackingData()

        ' Reset the selected index of all ComboBoxes to -1
        cmbCurrentMachine.SelectedIndex = -1
        cmbNextMachine.SelectedIndex = -1
        cmbclients.SelectedIndex = -1
        cmbWorder.SelectedIndex = -1
    End Sub
    Private Sub FormatDataGridView()
        ' Format the header row
        For Each column As DataGridViewColumn In DataGridView1.Columns
            column.HeaderCell.Style.Font = New Font("Arial", 10, FontStyle.Bold)
            column.HeaderCell.Style.BackColor = Color.LightBlue
            column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

            ' Change header text based on column name
            Select Case column.Name
                Case "steps_num"
                    column.HeaderText = "ترتيب المرحله السابقة"
                Case "proccess_ar"
                    column.HeaderText = "المرحله السابقة"
                Case "name_ar"
                    column.HeaderText = "الماكينه السابقة"
                Case "next_steps_num"
                    column.HeaderText = "ترتيب المرحله الحالية"
                Case "next_proccess_ar"
                    column.HeaderText = "المرحله الحالية"
                Case "next_name_ar"
                    column.HeaderText = "الماكينه الحالية"
                Case "client_code"
                    column.HeaderText = "كود العميل"
                Case "Delivery_dat"
                    column.HeaderText = "تاريخ التسليم"
                Case "days_to_delivery"
                    column.HeaderText = "الايام المتبقيه للتسليم"
                Case "code"
                    column.HeaderText = "كود المكتبه"
                Case "datein"
                    column.HeaderText = "تاريخ المرحله السابقة"
                Case "stage_duration"
                    column.HeaderText = "فتره التوقف علي المرحله"
            End Select
        Next

        ' Center the content of all cells
        For Each row As DataGridViewRow In DataGridView1.Rows
            For Each cell As DataGridViewCell In row.Cells
                cell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            Next
        Next
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        ' Load the data
        LoadTrackingData()

        ' Get the DataTable from the DataGridView
        Dim dataTable As DataTable = CType(DataGridView1.DataSource, DataTable)
        If dataTable Is Nothing Then
            MessageBox.Show("No data available to filter.")
            Return
        End If

        ' Build the filter expression
        Dim filterExpression As String = String.Empty

        ' Add filters based on ComboBox selections
        If cmbCurrentMachine.SelectedIndex <> -1 Then
            filterExpression &= $"name_ar = '{cmbCurrentMachine.SelectedItem.ToString()}'"
        End If
        If cmbNextMachine.SelectedIndex <> -1 Then
            If Not String.IsNullOrEmpty(filterExpression) Then filterExpression &= " AND "
            filterExpression &= $"next_name_ar = '{cmbNextMachine.SelectedItem.ToString()}'"
        End If
        If cmbclients.SelectedIndex <> -1 Then
            If Not String.IsNullOrEmpty(filterExpression) Then filterExpression &= " AND "
            filterExpression &= $"client_code = '{cmbclients.SelectedItem.ToString()}'"
        End If
        If cmbWorder.SelectedIndex <> -1 Then
            If Not String.IsNullOrEmpty(filterExpression) Then filterExpression &= " AND "
            filterExpression &= $"worderid = '{cmbWorder.SelectedItem.ToString()}'"
        End If

        ' Apply the filter to the DataView
        Dim dataView As New DataView(dataTable)
        dataView.RowFilter = filterExpression

        ' Update the DataGridView with the filtered data
        DataGridView1.DataSource = dataView

        ' Populate ComboBoxes with distinct values
        PopulateCurrentMachineComboBox(dataTable)
        PopulateNextMachineComboBoxFromDataGridView()
        PopulateClientsComboBox(dataTable)
        PopulateWorderComboBox(dataTable)

        ' Format the DataGridView
        FormatDataGridView()
    End Sub

    Private Sub PopulateNextMachineComboBoxFromDataGridView()
        Dim distinctValues = DataGridView1.Rows.
                         Cast(Of DataGridViewRow)().
                         Where(Function(row) Not row.IsNewRow AndAlso row.Cells("next_name_ar").Value IsNot Nothing).
                         Select(Function(row) row.Cells("next_name_ar").Value.ToString()).
                         Distinct().
                         ToList()

        cmbNextMachine.DataSource = distinctValues
        cmbNextMachine.SelectedIndex = -1 ' Ensure no item is selected by default
    End Sub





    Private Sub PopulateCurrentMachineComboBox(dataTable As DataTable)
        Dim distinctValues = dataTable.AsEnumerable().
                     Select(Function(row) row.Field(Of String)("name_ar")).
                     Distinct().
                     ToList()

        cmbCurrentMachine.DataSource = distinctValues
        cmbCurrentMachine.SelectedIndex = -1 ' Ensure no item is selected by default
    End Sub





    Private Sub PopulateClientsComboBox(dataTable As DataTable)
        Dim distinctValues = dataTable.AsEnumerable().
                     Select(Function(row) row.Field(Of String)("client_code")).
                     Distinct().
                     ToList()

        cmbclients.DataSource = distinctValues
        cmbclients.SelectedIndex = -1 ' Ensure no item is selected by default
    End Sub

    Private Sub PopulateWorderComboBox(dataTable As DataTable)
        Dim distinctValues = dataTable.AsEnumerable().
                     Select(Function(row) row.Field(Of String)("worderid")).
                     Distinct().
                     ToList()

        cmbWorder.DataSource = distinctValues
        cmbWorder.SelectedIndex = -1 ' Ensure no item is selected by default
    End Sub



    Private Sub btnExportToExcel_Click(sender As Object, e As EventArgs) Handles btnExportToExcel.Click
        Dim excelApp As New Excel.Application()
        Dim workbook As Excel.Workbook = excelApp.Workbooks.Add()
        Dim worksheet As Excel.Worksheet = CType(workbook.Sheets(1), Excel.Worksheet)

        ' Add column headers
        For i As Integer = 1 To DataGridView1.Columns.Count
            worksheet.Cells(1, i) = DataGridView1.Columns(i - 1).HeaderText
        Next

        ' Add rows
        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            For j As Integer = 0 To DataGridView1.Columns.Count - 1
                worksheet.Cells(i + 2, j + 1) = DataGridView1.Rows(i).Cells(j).Value
            Next
        Next

        ' Format the header row
        Dim headerRange As Excel.Range = worksheet.Range(worksheet.Cells(1, 1), worksheet.Cells(1, DataGridView1.Columns.Count))
        headerRange.Font.Bold = True
        headerRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightBlue)
        headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter

        ' Auto-fit columns
        worksheet.Columns.AutoFit()

        ' Show the Excel application
        excelApp.Visible = True
    End Sub
End Class