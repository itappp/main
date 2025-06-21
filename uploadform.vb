Imports OfficeOpenXml ' For reading Excel
Imports System.Data.SqlClient ' For SQL Server connection
Imports System.Windows.Forms ' For OpenFileDialog
Imports System.IO ' For file handling
Imports System.Linq


Public Class UploadForm
    ' SQL Server connection string
    Dim connectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"

    Private Sub UploadForm_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        ' Add event handler for the "Upload" button
        AddHandler btnUpload.Click, AddressOf btnUpload_Click
        AddHandler cmbTables.SelectedIndexChanged, AddressOf cmbTables_SelectedIndexChanged

        ' Populate the ComboBox with table names
        PopulateTableNames()
    End Sub

    ' Method to populate ComboBox with table names from the database
    Private Sub PopulateTableNames()
        Using connection As New SqlConnection(connectionString)
            Try
                connection.Open()
                Dim query As String = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'"
                Using command As New SqlCommand(query, connection)
                    Using reader As SqlDataReader = command.ExecuteReader()
                        While reader.Read()
                            cmbTables.Items.Add(reader("TABLE_NAME").ToString())
                        End While
                    End Using
                End Using
            Catch ex As Exception
                MessageBox.Show("Error retrieving table names: " & ex.Message)
            End Try
        End Using
    End Sub

    ' Event handler for when a table is selected
    Private Sub cmbTables_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If cmbTables.SelectedIndex <> -1 Then
            DisplayTableColumns(cmbTables.SelectedItem.ToString())
        End If
    End Sub

    ' Method to display columns of the selected table with checkboxes
    Private Sub DisplayTableColumns(ByVal tableName As String)
        ' Clear previous controls
        flpColumns.Controls.Clear()

        Using connection As New SqlConnection(connectionString)
            Try
                connection.Open()
                Dim query As String = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @TableName"
                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@TableName", tableName)
                    Using reader As SqlDataReader = command.ExecuteReader()
                        While reader.Read()
                            ' Create a CheckBox for each column
                            Dim columnCheckBox As New CheckBox()
                            columnCheckBox.Text = reader("COLUMN_NAME").ToString()
                            columnCheckBox.AutoSize = True
                            flpColumns.Controls.Add(columnCheckBox)
                        End While
                    End Using
                End Using
            Catch ex As Exception
                MessageBox.Show("Error retrieving columns: " & ex.Message)
            End Try
        End Using
    End Sub

    ' Event handler for the "Upload" button
    Private Sub btnUpload_Click(ByVal sender As Object, ByVal e As EventArgs)
        If cmbTables.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a table.")
            Return
        End If

        ' Collect selected columns from checkboxes
        Dim selectedColumns As New List(Of String)()
        For Each control As Control In flpColumns.Controls
            If TypeOf control Is CheckBox Then
                Dim checkBox As CheckBox = CType(control, CheckBox)
                If checkBox.Checked Then
                    selectedColumns.Add(checkBox.Text)
                End If
            End If
        Next

        If selectedColumns.Count = 0 Then
            MessageBox.Show("Please select at least one column.")
            Return
        End If

        ' Open file dialog for selecting the Excel file
        Dim openFileDialog As New OpenFileDialog() With {
            .Filter = "Excel Files|*.xlsx;*.xls",
            .Title = "Select an Excel File"
        }

        If openFileDialog.ShowDialog() = DialogResult.OK Then
            Dim selectedFilePath As String = openFileDialog.FileName
            lblFilePath.Text = selectedFilePath
            UploadExcelToDatabase(selectedFilePath, cmbTables.SelectedItem.ToString(), selectedColumns)
        End If
    End Sub
    ' Method to upload Excel data to the database for selected columns
    Private Sub UploadExcelToDatabase(ByVal filePath As String, ByVal tableName As String, ByVal columns As List(Of String))
        ' Set the license context for EPPlus
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial

        Using package As New ExcelPackage(New FileInfo(filePath))
            ' Check if the workbook contains at least one worksheet
            If package.Workbook.Worksheets.Count < 1 Then
                MessageBox.Show("The selected Excel file does not contain any worksheets.")
                Return
            End If

            Dim worksheet As ExcelWorksheet = package.Workbook.Worksheets.First()
            If worksheet Is Nothing Then
                MessageBox.Show("The selected Excel file does not contain a valid worksheet.")
                Return
            End If

            Using connection As New SqlConnection(connectionString)
                connection.Open()

                ' Iterate over the rows in the worksheet, starting from row 2 to skip headers
                For row As Integer = 2 To worksheet.Dimension.End.Row
                    Dim values As New List(Of Object)()

                    ' Collect cell values for each selected column
                    For i As Integer = 0 To columns.Count - 1
                        Dim cellValue As String = worksheet.Cells(row, i + 1).Text
                        values.Add(cellValue)
                    Next

                    ' Construct the parameterized query using String.Format
                    Dim columnList As String = String.Join(", ", columns)
                    Dim parameterList As String = String.Join(", ", columns.Select(Function(c, index) "@param" & index))
                    Dim query As String = String.Format("INSERT INTO [{0}] ({1}) VALUES ({2})", tableName, columnList, parameterList)

                    Using command As New SqlCommand(query, connection)
                        ' Add parameters to the command
                        For i As Integer = 0 To values.Count - 1
                            command.Parameters.AddWithValue("@param" & i, values(i))
                        Next

                        Try
                            command.ExecuteNonQuery()
                        Catch ex As SqlException
                            MessageBox.Show("Error inserting data: " & ex.Message)
                        End Try
                    End Using
                Next
            End Using
        End Using

        MessageBox.Show("Data uploaded successfully!")
    End Sub

    ' Event handler for the "Generate Sample Excel" button
    Private Sub btnSample_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsample.Click
        ' Collect selected columns from the CheckBox controls
        Dim selectedColumns As New List(Of String)()
        For Each control As Control In flpColumns.Controls
            If TypeOf control Is CheckBox Then
                Dim checkBox As CheckBox = CType(control, CheckBox)
                If checkBox.Checked Then
                    selectedColumns.Add(checkBox.Text)
                End If
            End If
        Next

        ' Check if any columns were selected
        If selectedColumns.Count = 0 Then
            MessageBox.Show("Please select at least one column to generate the sample Excel sheet.")
            Return
        End If

        ' Generate and save the sample Excel sheet
        GenerateSampleExcel(selectedColumns)
    End Sub

    ' Method to generate a sample Excel sheet
    Private Sub GenerateSampleExcel(ByVal columns As List(Of String))
        Dim filePath As String = Path.Combine("D:\SYSTEMS\Upload", "SampleExcelSheet.xlsx")

        ' Set the license context for EPPlus
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial

        ' Create a new Excel package
        Using package As New ExcelPackage()
            Dim worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add("SampleSheet")

            ' Add column headers to the first row
            For i As Integer = 0 To columns.Count - 1
                worksheet.Cells(1, i + 1).Value = columns(i)
            Next

            ' Auto-fit columns for better readability
            worksheet.Cells.AutoFitColumns()

            ' Save the Excel file to the specified path
            Dim fileBytes As Byte() = package.GetAsByteArray()
            File.WriteAllBytes(filePath, fileBytes)

            ' Notify the user and provide the file location
            MessageBox.Show("Sample Excel sheet created successfully at: " & filePath)
        End Using
    End Sub

End Class

