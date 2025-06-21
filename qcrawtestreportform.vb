Imports MySql.Data.MySqlClient
Imports System.Data.SqlClient
Imports Excel = Microsoft.Office.Interop.Excel ' For Excel interop
Imports OfficeOpenXml ' Add this line for EPPlus
Imports System.IO
Imports System.Net
Imports Mysqlx.XDevAPI.CRUD
Public Class qcrawtestreportform
    Private sqlServerConnectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"
    Private mysqlServerConnectionString As String = "Server=150.1.1.7;Database=wm;Uid=root1;Pwd=WMg2024$;"

    Private Sub qcrawtestreportform_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Load batch numbers into cmbbatch
        LoadBatchNumbers()
    End Sub

    Private Sub LoadBatchNumbers()
        Dim query1 As String = "SELECT DISTINCT batch_id FROM qc_raw_test"

        Dim batchNumbers As New HashSet(Of String)()

        ' Load batch numbers from the first data source
        Using conn As New SqlConnection(sqlServerConnectionString)
            Dim cmd As New SqlCommand(query1, conn)
            Try
                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                While reader.Read()
                    batchNumbers.Add(reader("batch_id").ToString())
                End While
            Catch ex As Exception
                MessageBox.Show("Error loading batch numbers from SQL Server: " & ex.Message)
            End Try
        End Using



        ' Add batch numbers to the combo box
        cmbbatch.Items.Clear()
        For Each batchNumber As String In batchNumbers
            cmbbatch.Items.Add(batchNumber)
        Next
    End Sub


    Private Sub btnsearch_Click(sender As Object, e As EventArgs) Handles btnsearch.Click
        If cmbbatch.SelectedIndex <> -1 Then
            Dim selectedBatch As String = cmbbatch.SelectedItem.ToString()
            LoadQC1DataFromSQL(selectedBatch)
        Else
            MessageBox.Show("Please select a batch number.")
        End If
    End Sub



    Private Sub LoadQC1DataFromSQL(ByVal batchNo As String)
        ' SQL Query to fetch data from the second table based on batch number
        Dim query As String = "SELECT batch_id as 'batch_no', lot, raw_befor_width as 'Raw before width', raw_after_width as 'Raw after width', " &
                          "raw_befor_weight as 'Weight of m2 before', raw_after_weight as 'Weight of m2 after', pva_Starch as 'pva / Starch', " &
                          "mix_rate as 'Mixing Percentage', tensile_weft as 'Rupture Weft', tensile_warp as 'Rupture Warp', " &
                          "tensile_result as 'Rupture Result', color_water as 'Color Fastness to water', tear_weft as 'Tear Weft', " &
                          "tear_warp as 'Tear Warp', tear_result as 'Tear Result', washing as 'Color Fastness for Washing', " &
                          "color_mercerize as 'Color Fastness for Mercerization', notes, username as 'user' " &
                          "FROM qc_raw_test WHERE batch_id = @batch_id"

        ' Create a DataTable to hold the results for the DataGridView
        Dim dt As New DataTable()

        ' Connection and command
        Using conn As New SqlConnection(sqlServerConnectionString)
            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@batch_id", batchNo) ' Use the batch number passed into the function

            Try
                conn.Open()
                ' Fill the DataTable with data from the query
                Dim adapter As New SqlDataAdapter(cmd)
                adapter.Fill(dt)

                ' Check if DataTable has rows and then populate the DataGridView
                If dt.Rows.Count > 0 Then
                    ' Add checkbox column if it doesn't exist

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
                        column.HeaderCell.Style.BackColor = Color.Blue ' Set header background color to dark blue
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
    Private Sub btnExportToExcel_Click(sender As Object, e As EventArgs) Handles btnExportToExcel.Click
        Dim excelApp As New Excel.Application()
        Dim workbook As Excel.Workbook = excelApp.Workbooks.Add()
        Dim worksheet As Excel.Worksheet = CType(workbook.Sheets(1), Excel.Worksheet)

        ' Add column headers
        For i As Integer = 1 To dgvqc.Columns.Count
            worksheet.Cells(1, i) = dgvqc.Columns(i - 1).HeaderText
        Next

        ' Add rows
        For i As Integer = 0 To dgvqc.Rows.Count - 1
            For j As Integer = 0 To dgvqc.Columns.Count - 1
                worksheet.Cells(i + 2, j + 1) = dgvqc.Rows(i).Cells(j).Value
            Next
        Next

        ' Format the header row
        Dim headerRange As Excel.Range = worksheet.Range(worksheet.Cells(1, 1), worksheet.Cells(1, dgvqc.Columns.Count))
        headerRange.Font.Bold = True
        headerRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightBlue)
        headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter

        ' Auto-fit columns
        worksheet.Columns.AutoFit()

        ' Show the Excel application
        excelApp.Visible = True
    End Sub
End Class
