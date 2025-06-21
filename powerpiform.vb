Imports System.Data.SqlClient
Imports System.IO
Imports OfficeOpenXml
Imports OfficeOpenXml.Drawing.Chart

Public Class powerpiform

    Private sqlServerConnectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click

        Dim dateFrom As DateTime = dtpDateFrom.Value
        Dim dateTo As DateTime = dtpDateto.Value

        Dim query As String = "SELECT " &
                              "CASE " &
                              "WHEN DATEPART(HOUR, fi.date) >= 8 AND DATEPART(HOUR, fi.date) < 16 THEN 'Shift 1' " &
                              "WHEN DATEPART(HOUR, fi.date) >= 16 AND DATEPART(HOUR, fi.date) < 23 THEN 'Shift 2' " &
                              "WHEN DATEPART(HOUR, fi.date) >= 23 OR (DATEPART(HOUR, fi.date) < 8 AND DATEPART(HOUR, fi.date) >= 0) THEN 'Shift 3' " &
                              "END AS الوردية, " &
                              "CASE " &
                              "WHEN DATEPART(HOUR, fi.date) >= 23 THEN CONVERT(date, fi.date) " &
                              "WHEN DATEPART(HOUR, fi.date) < 8 THEN CONVERT(date, DATEADD(day, -1, fi.date)) " &
                              "ELSE CONVERT(date, fi.date) " &
                              "END AS تاريخ_الوردية, " &
                              "SUM(COALESCE(fi.height, 0)) AS الطول, " &
                              "SUM(COALESCE(fi.weight, 0)) AS الوزن, " &
                              "SUM(CASE WHEN COALESCE(fi.fabric_grade, 2) = 1 THEN COALESCE(fi.height, 0) ELSE 0 END) AS الطول_الدرجة_الأولى, " &
                              "SUM(CASE WHEN COALESCE(fi.fabric_grade, 2) = 1 THEN COALESCE(fi.weight, 0) ELSE 0 END) AS الوزن_الدرجة_الأولى, " &
                              "SUM(CASE WHEN COALESCE(fi.fabric_grade, 2) = 2 THEN COALESCE(fi.height, 0) ELSE 0 END) AS الطول_الدرجة_الثانية, " &
                              "SUM(CASE WHEN COALESCE(fi.fabric_grade, 2) = 2 THEN COALESCE(fi.weight, 0) ELSE 0 END) AS الوزن_الدرجة_الثانية " &
                              "FROM finish_inspect fi " &
                              "WHERE CASE " &
                              "WHEN DATEPART(HOUR, fi.date) >= 23 THEN CONVERT(date, fi.date) " &
                              "WHEN DATEPART(HOUR, fi.date) < 8 THEN CONVERT(date, DATEADD(day, -1, fi.date)) " &
                              "ELSE CONVERT(date, fi.date) " &
                              "END BETWEEN @dateFrom AND @dateTo " &
                              "GROUP BY CASE " &
                              "WHEN DATEPART(HOUR, fi.date) >= 23 THEN CONVERT(date, fi.date) " &
                              "WHEN DATEPART(HOUR, fi.date) < 8 THEN CONVERT(date, DATEADD(day, -1, fi.date)) " &
                              "ELSE CONVERT(date, fi.date) " &
                              "END, " &
                              "CASE " &
                              "WHEN DATEPART(HOUR, fi.date) >= 8 AND DATEPART(HOUR, fi.date) < 16 THEN 'Shift 1' " &
                              "WHEN DATEPART(HOUR, fi.date) >= 16 AND DATEPART(HOUR, fi.date) < 23 THEN 'Shift 2' " &
                              "WHEN DATEPART(HOUR, fi.date) >= 23 OR (DATEPART(HOUR, fi.date) < 8 AND DATEPART(HOUR, fi.date) >= 0) THEN 'Shift 3' " &
                              "END " &
                              "ORDER BY تاريخ_الوردية, الوردية;"

        Using conn As New SqlConnection(sqlServerConnectionString)
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@dateFrom", dateFrom.Date)
                cmd.Parameters.AddWithValue("@dateTo", dateTo.Date)

                Try
                    conn.Open()
                    Dim reader As SqlDataReader = cmd.ExecuteReader()
                    Dim dt As New DataTable()
                    dt.Load(reader)
                    dgvResults.DataSource = dt

                    ' Format DataGridView
                    FormatDataGridView(dgvResults)
                Catch ex As Exception
                    MessageBox.Show("Error loading search results: " & ex.Message)
                End Try
            End Using
        End Using

        ' Load additional data from store_finish
        LoadStoreFinishData(dateFrom, dateTo)
    End Sub

    Private Sub LoadStoreFinishData(dateFrom As DateTime, dateTo As DateTime)
        Dim query As String = "SELECT " &
            "SUM(COALESCE(sf.heightPK, 0)) AS الطول, " &
                              "SUM(COALESCE(sf.weightPK, 0)) AS الوزن, " &
                              "SUM(CASE WHEN COALESCE(sf.fabric_grade, 2) = 1 THEN COALESCE(sf.heightPK, 0) ELSE 0 END) AS الطول_الدرجة_الأولى, " &
                              "SUM(CASE WHEN COALESCE(sf.fabric_grade, 2) = 1 THEN COALESCE(sf.weightPK, 0) ELSE 0 END) AS الوزن_الدرجة_الأولى, " &
                              "SUM(CASE WHEN COALESCE(sf.fabric_grade, 2) = 2 THEN COALESCE(sf.heightPK, 0) ELSE 0 END) AS الطول_الدرجة_الثانية, " &
                              "SUM(CASE WHEN COALESCE(sf.fabric_grade, 2) = 2 THEN COALESCE(sf.weightPK, 0) ELSE 0 END) AS الوزن_الدرجة_الثانية " &
                              "FROM store_finish sf " &
                              "WHERE CONVERT(DATE, sf.transaction_date) BETWEEN @dateFrom AND @dateTo;"

        Using conn As New SqlConnection(sqlServerConnectionString)
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@dateFrom", dateFrom.Date)
                cmd.Parameters.AddWithValue("@dateTo", dateTo.Date)

                Try
                    conn.Open()
                    Dim reader As SqlDataReader = cmd.ExecuteReader()
                    Dim dt As New DataTable()
                    dt.Load(reader)
                    dgvStoreFinishResults.DataSource = dt

                    ' Format DataGridView
                    FormatDataGridView(dgvStoreFinishResults)
                Catch ex As Exception
                    MessageBox.Show("Error loading store finish data: " & ex.Message)
                End Try
            End Using
        End Using
    End Sub

    Private Sub FormatDataGridView(dgv As DataGridView)
        ' Center the content of the DataGridView
        For Each column As DataGridViewColumn In dgv.Columns
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            column.DefaultCellStyle.Font = New Font("Arial", 11)
        Next

        ' Set the header font size to 12, make it bold, and center the content
        dgv.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 12, FontStyle.Bold)
        dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        ' Fill the color of the headers
        dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue
        dgv.EnableHeadersVisualStyles = False
    End Sub

    Private Sub btnGenerateReport_Click(sender As Object, e As EventArgs) Handles btnGenerateReport.Click
        GeneratePowerBIReport()
    End Sub

    Private Sub GeneratePowerBIReport()
        ' Connect to the database and retrieve the data
        Dim dateFrom As DateTime = dtpDateFrom.Value
        Dim dateTo As DateTime = dtpDateto.Value

        Dim query As String = "SELECT " &
                              "CASE " &
                              "WHEN DATEPART(HOUR, fi.date) >= 8 AND DATEPART(HOUR, fi.date) < 16 THEN 'Shift 1' " &
                              "WHEN DATEPART(HOUR, fi.date) >= 16 AND DATEPART(HOUR, fi.date) < 23 THEN 'Shift 2' " &
                              "WHEN DATEPART(HOUR, fi.date) >= 23 OR (DATEPART(HOUR, fi.date) < 8 AND DATEPART(HOUR, fi.date) >= 0) THEN 'Shift 3' " &
                              "END AS الوردية, " &
                              "CASE " &
                              "WHEN DATEPART(HOUR, fi.date) >= 23 THEN CONVERT(DATE, fi.date) " &
                              "WHEN DATEPART(HOUR, fi.date) < 8 THEN CONVERT(DATE, DATEADD(DAY, -1, fi.date)) " &
                              "ELSE CONVERT(DATE, fi.date) " &
                              "END AS تاريخ_الوردية, " &
                              "SUM(COALESCE(fi.height, 0)) AS الطول, " &
                              "SUM(COALESCE(fi.weight, 0)) AS الوزن, " &
                              "SUM(CASE WHEN COALESCE(fi.fabric_grade, 2) = 1 THEN COALESCE(fi.height, 0) ELSE 0 END) AS الطول_الدرجة_الأولى, " &
                              "SUM(CASE WHEN COALESCE(fi.fabric_grade, 2) = 1 THEN COALESCE(fi.weight, 0) ELSE 0 END) AS الوزن_الدرجة_الأولى, " &
                              "SUM(CASE WHEN COALESCE(fi.fabric_grade, 2) = 2 THEN COALESCE(fi.height, 0) ELSE 0 END) AS الطول_الدرجة_الثانية, " &
                              "SUM(CASE WHEN COALESCE(fi.fabric_grade, 2) = 2 THEN COALESCE(fi.weight, 0) ELSE 0 END) AS الوزن_الدرجة_الثانية " &
                              "FROM finish_inspect fi " &
                              "WHERE CASE " &
                              "WHEN DATEPART(HOUR, fi.date) >= 23 THEN CONVERT(DATE, fi.date) " &
                              "WHEN DATEPART(HOUR, fi.date) < 8 THEN CONVERT(DATE, DATEADD(DAY, -1, fi.date)) " &
                              "ELSE CONVERT(DATE, fi.date) " &
                              "END BETWEEN @dateFrom AND @dateTo " &
                              "GROUP BY CASE " &
                              "WHEN DATEPART(HOUR, fi.date) >= 23 THEN CONVERT(DATE, fi.date) " &
                              "WHEN DATEPART(HOUR, fi.date) < 8 THEN CONVERT(DATE, DATEADD(DAY, -1, fi.date)) " &
                              "ELSE CONVERT(DATE, fi.date) " &
                              "END, " &
                              "CASE " &
                              "WHEN DATEPART(HOUR, fi.date) >= 8 AND DATEPART(HOUR, fi.date) < 16 THEN 'Shift 1' " &
                              "WHEN DATEPART(HOUR, fi.date) >= 16 AND DATEPART(HOUR, fi.date) < 23 THEN 'Shift 2' " &
                              "WHEN DATEPART(HOUR, fi.date) >= 23 OR (DATEPART(HOUR, fi.date) < 8 AND DATEPART(HOUR, fi.date) >= 0) THEN 'Shift 3' " &
                              "END " &
                              "ORDER BY تاريخ_الوردية, الوردية;"

        Using conn As New SqlConnection(sqlServerConnectionString)
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@dateFrom", dateFrom.Date)
                cmd.Parameters.AddWithValue("@dateTo", dateTo.Date)

                Try
                    conn.Open()
                    Dim reader As SqlDataReader = cmd.ExecuteReader()
                    Dim dt As New DataTable()
                    dt.Load(reader)

                    ' Generate the Power BI report
                    GenerateExcelReport(dt, dateFrom, dateTo)
                Catch ex As Exception
                    MessageBox.Show("Error generating report: " & ex.Message)
                End Try
            End Using
        End Using
    End Sub


    Private Sub GenerateExcelReport(dt As DataTable, dateFrom As DateTime, dateTo As DateTime)
        ' Set the license context for EPPlus
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial

        Using package As New ExcelPackage()
            Dim worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add("Report")

            ' Add title for the first section
            worksheet.Cells("A1").Value = "فحص المجهز"
            worksheet.Cells("A1").Style.Font.Bold = True
            worksheet.Cells("A1").Style.Font.Size = 18
            worksheet.Cells("A1").Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center
            worksheet.Cells("A1").Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center
            worksheet.Cells("A1:H1").Merge = True

            ' Add headers for the first table
            For i As Integer = 0 To dt.Columns.Count - 1
                worksheet.Cells(2, i + 1).Value = dt.Columns(i).ColumnName
            Next

            ' Add data for the first table
            For i As Integer = 0 To dt.Rows.Count - 1
                For j As Integer = 0 To dt.Columns.Count - 1
                    worksheet.Cells(i + 3, j + 1).Value = dt.Rows(i)(j)
                Next
            Next

            ' Format the first table
            Dim mainTableRange As ExcelRange = worksheet.Cells(2, 1, dt.Rows.Count + 2, dt.Columns.Count)
            mainTableRange.Style.Font.Size = 14
            mainTableRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center
            mainTableRange.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin
            mainTableRange.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin
            mainTableRange.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin
            mainTableRange.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin

            ' Format the first table headers
            Dim mainTableHeaderRange As ExcelRange = worksheet.Cells(2, 1, 2, dt.Columns.Count)
            mainTableHeaderRange.Style.Font.Bold = True
            mainTableHeaderRange.Style.Font.Size = 16
            mainTableHeaderRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid
            mainTableHeaderRange.Style.Fill.BackgroundColor.SetColor(Color.LightBlue)
            mainTableHeaderRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center

            ' Format the تاريخ_الوردية column as short date
            Dim dateColumnIndex As Integer = dt.Columns.IndexOf("تاريخ_الوردية") + 1
            If dateColumnIndex > 0 Then
                Dim dateColumnRange As ExcelRange = worksheet.Cells(3, dateColumnIndex, dt.Rows.Count + 2, dateColumnIndex)
                dateColumnRange.Style.Numberformat.Format = "dd-mm-yyyy"
            End If

            ' Color the الطول and الوزن columns
            Dim heightColumnIndex As Integer = dt.Columns.IndexOf("الطول") + 1
            Dim weightColumnIndex As Integer = dt.Columns.IndexOf("الوزن") + 1
            If heightColumnIndex > 0 Then
                Dim heightColumnRange As ExcelRange = worksheet.Cells(3, heightColumnIndex, dt.Rows.Count + 2, heightColumnIndex)
                heightColumnRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid
                heightColumnRange.Style.Fill.BackgroundColor.SetColor(Color.LightGreen)
            End If
            If weightColumnIndex > 0 Then
                Dim weightColumnRange As ExcelRange = worksheet.Cells(3, weightColumnIndex, dt.Rows.Count + 2, weightColumnIndex)
                weightColumnRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid
                weightColumnRange.Style.Fill.BackgroundColor.SetColor(Color.LightYellow)
            End If

            ' Add totals for the first table
            Dim totalHeight As Decimal = dt.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("الطول"))
            Dim totalWeight As Decimal = dt.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("الوزن"))
            Dim totalFirstGradeHeight As Decimal = dt.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("الطول_الدرجة_الأولى"))
            Dim totalFirstGradeWeight As Decimal = dt.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("الوزن_الدرجة_الأولى"))
            Dim totalSecondGradeHeight As Decimal = dt.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("الطول_الدرجة_الثانية"))
            Dim totalSecondGradeWeight As Decimal = dt.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("الوزن_الدرجة_الثانية"))
            Dim totalQuantity As Decimal = totalFirstGradeWeight + totalSecondGradeWeight
            Dim secondGradePercentage As Decimal = If(totalQuantity > 0, (totalSecondGradeWeight / totalQuantity) * 100, 0)

            Dim totalRow As Integer = dt.Rows.Count + 4
            worksheet.Cells(totalRow, 1).Value = "الإجماليات"
            worksheet.Cells(totalRow, 2).Value = "الطول"
            worksheet.Cells(totalRow, 3).Value = "الوزن"
            worksheet.Cells(totalRow, 4).Value = "الطول الدرجة الأولى"
            worksheet.Cells(totalRow, 5).Value = "الوزن الدرجة الأولى"
            worksheet.Cells(totalRow, 6).Value = "الطول الدرجة الثانية"
            worksheet.Cells(totalRow, 7).Value = "الوزن الدرجة الثانية"
            worksheet.Cells(totalRow, 8).Value = "نسبة الدرجة الثانية"

            worksheet.Cells(totalRow + 1, 2).Value = totalHeight
            worksheet.Cells(totalRow + 1, 3).Value = totalWeight
            worksheet.Cells(totalRow + 1, 4).Value = totalFirstGradeHeight
            worksheet.Cells(totalRow + 1, 5).Value = totalFirstGradeWeight
            worksheet.Cells(totalRow + 1, 6).Value = totalSecondGradeHeight
            worksheet.Cells(totalRow + 1, 7).Value = totalSecondGradeWeight
            worksheet.Cells(totalRow + 1, 8).Value = Math.Round(secondGradePercentage, 2) & "%"

            ' Format the totals table
            Dim totalsTableRange As ExcelRange = worksheet.Cells(totalRow, 1, totalRow + 1, 8)
            totalsTableRange.Style.Font.Size = 14
            totalsTableRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center
            totalsTableRange.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin
            totalsTableRange.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin
            totalsTableRange.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin
            totalsTableRange.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin

            ' Format the totals table headers
            Dim totalsTableHeaderRange As ExcelRange = worksheet.Cells(totalRow, 1, totalRow, 8)
            totalsTableHeaderRange.Style.Font.Bold = True
            totalsTableHeaderRange.Style.Font.Size = 16
            totalsTableHeaderRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid
            totalsTableHeaderRange.Style.Fill.BackgroundColor.SetColor(Color.LightBlue)
            totalsTableHeaderRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center

            ' Color the الطول and الوزن columns in the totals table
            If heightColumnIndex > 0 Then
                Dim totalsHeightColumnRange As ExcelRange = worksheet.Cells(totalRow + 1, heightColumnIndex, totalRow + 1, heightColumnIndex)
                totalsHeightColumnRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid
                totalsHeightColumnRange.Style.Fill.BackgroundColor.SetColor(Color.LightGreen)
            End If
            If weightColumnIndex > 0 Then
                Dim totalsWeightColumnRange As ExcelRange = worksheet.Cells(totalRow + 1, weightColumnIndex, totalRow + 1, weightColumnIndex)
                totalsWeightColumnRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid
                totalsWeightColumnRange.Style.Fill.BackgroundColor.SetColor(Color.LightYellow)
            End If

            ' Load additional data from store_finish
            Dim storeFinishData As DataTable = LoadStoreFinishDataForReport(dateFrom, dateTo)

            ' Add title for the second section
            Dim storeFinishStartRow As Integer = totalRow + 5
            worksheet.Cells(storeFinishStartRow, 1).Value = "استلام مخزن المجهز"
            worksheet.Cells(storeFinishStartRow, 1).Style.Font.Bold = True
            worksheet.Cells(storeFinishStartRow, 1).Style.Font.Size = 18
            worksheet.Cells(storeFinishStartRow, 1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center
            worksheet.Cells(storeFinishStartRow, 1).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center
            worksheet.Cells(storeFinishStartRow, 1, storeFinishStartRow, storeFinishData.Columns.Count + 1).Merge = True

            ' Add headers for the second table
            For i As Integer = 0 To storeFinishData.Columns.Count - 1
                worksheet.Cells(storeFinishStartRow + 1, i + 1).Value = storeFinishData.Columns(i).ColumnName
            Next
            worksheet.Cells(storeFinishStartRow + 1, storeFinishData.Columns.Count + 1).Value = "نسبة الدرجة الثانية"

            ' Add data for the second table
            For i As Integer = 0 To storeFinishData.Rows.Count - 1
                For j As Integer = 0 To storeFinishData.Columns.Count - 1
                    worksheet.Cells(storeFinishStartRow + 2 + i, j + 1).Value = storeFinishData.Rows(i)(j)
                Next
                ' Calculate and add the second grade percentage
                Dim rowTotalWeight As Decimal = storeFinishData.Rows(i).Field(Of Decimal)("الوزن")
                Dim rowSecondGradeWeight As Decimal = storeFinishData.Rows(i).Field(Of Decimal)("الوزن_الدرجة_الثانية")
                Dim rowSecondGradePercentage As Decimal = If(rowTotalWeight > 0, (rowSecondGradeWeight / rowTotalWeight) * 100, 0)
                worksheet.Cells(storeFinishStartRow + 2 + i, storeFinishData.Columns.Count + 1).Value = Math.Round(rowSecondGradePercentage, 2) & "%"
            Next

            ' Format the second table
            Dim storeFinishTableRange As ExcelRange = worksheet.Cells(storeFinishStartRow + 1, 1, storeFinishStartRow + 1 + storeFinishData.Rows.Count, storeFinishData.Columns.Count + 1)
            storeFinishTableRange.Style.Font.Size = 14
            storeFinishTableRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center
            storeFinishTableRange.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin
            storeFinishTableRange.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin
            storeFinishTableRange.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin
            storeFinishTableRange.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin

            ' Format the second table headers
            Dim storeFinishTableHeaderRange As ExcelRange = worksheet.Cells(storeFinishStartRow + 1, 1, storeFinishStartRow + 1, storeFinishData.Columns.Count + 1)
            storeFinishTableHeaderRange.Style.Font.Bold = True
            storeFinishTableHeaderRange.Style.Font.Size = 16
            storeFinishTableHeaderRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid
            storeFinishTableHeaderRange.Style.Fill.BackgroundColor.SetColor(Color.LightBlue)
            storeFinishTableHeaderRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center

            ' Format the التاريخ column as short date
            Dim storeFinishDateColumnIndex As Integer = storeFinishData.Columns.IndexOf("التاريخ") + 1
            If storeFinishDateColumnIndex > 0 Then
                Dim storeFinishDateColumnRange As ExcelRange = worksheet.Cells(storeFinishStartRow + 2, storeFinishDateColumnIndex, storeFinishStartRow + 1 + storeFinishData.Rows.Count, storeFinishDateColumnIndex)
                storeFinishDateColumnRange.Style.Numberformat.Format = "dd-mm-yyyy"
            End If

            ' Color the الطول and الوزن columns in the second table
            Dim storeFinishHeightColumnIndex As Integer = storeFinishData.Columns.IndexOf("الطول") + 1
            Dim storeFinishWeightColumnIndex As Integer = storeFinishData.Columns.IndexOf("الوزن") + 1
            If storeFinishHeightColumnIndex > 0 Then
                Dim storeFinishHeightColumnRange As ExcelRange = worksheet.Cells(storeFinishStartRow + 2, storeFinishHeightColumnIndex, storeFinishStartRow + 1 + storeFinishData.Rows.Count, storeFinishHeightColumnIndex)
                storeFinishHeightColumnRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid
                storeFinishHeightColumnRange.Style.Fill.BackgroundColor.SetColor(Color.LightGreen)
            End If
            If storeFinishWeightColumnIndex > 0 Then
                Dim storeFinishWeightColumnRange As ExcelRange = worksheet.Cells(storeFinishStartRow + 2, storeFinishWeightColumnIndex, storeFinishStartRow + 1 + storeFinishData.Rows.Count, storeFinishWeightColumnIndex)
                storeFinishWeightColumnRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid
                storeFinishWeightColumnRange.Style.Fill.BackgroundColor.SetColor(Color.LightYellow)
            End If

            ' Auto fit columns
            worksheet.Cells(worksheet.Dimension.Address).AutoFitColumns()

            ' Create a pie chart
            Dim chart As ExcelPieChart = worksheet.Drawings.AddChart("Summary Chart", eChartType.Pie)
            chart.SetPosition(storeFinishStartRow + storeFinishData.Rows.Count + 3, 0, 0, 0)
            chart.SetSize(800, 400)

            Dim series As ExcelPieChartSerie = chart.Series.Add(worksheet.Cells("d2:d" & (totalRow - 1)), worksheet.Cells("a2:a" & (totalRow - 1)))
            series.Header = "Summary"

            ' Set data labels
            series.DataLabel.ShowCategory = True
            series.DataLabel.ShowValue = True
            series.DataLabel.ShowPercent = True

            ' Save the report
            Dim saveFileDialog As New SaveFileDialog()
            saveFileDialog.Filter = "Excel Workbook|*.xlsx"
            saveFileDialog.Title = "Save Report"
            saveFileDialog.FileName = "PowerBI_finish-inspect.xlsx"

            If saveFileDialog.ShowDialog() = DialogResult.OK Then
                Dim fileInfo As New FileInfo(saveFileDialog.FileName)
                package.SaveAs(fileInfo)
                MessageBox.Show("Report generated successfully!")
            End If
        End Using
    End Sub

    Private Function LoadStoreFinishDataForReport(dateFrom As DateTime, dateTo As DateTime) As DataTable
        Dim query As String = "SELECT " &
                              "CONVERT(DATE, sf.transaction_date) AS التاريخ, " &
                              "SUM(COALESCE(sf.heightPK, 0)) AS الطول, " &
                              "SUM(COALESCE(sf.weightPK, 0)) AS الوزن, " &
                              "SUM(CASE WHEN COALESCE(sf.fabric_grade, 2) = 1 THEN COALESCE(sf.heightPK, 0) ELSE 0 END) AS الطول_الدرجة_الأولى, " &
                              "SUM(CASE WHEN COALESCE(sf.fabric_grade, 2) = 1 THEN COALESCE(sf.weightPK, 0) ELSE 0 END) AS الوزن_الدرجة_الأولى, " &
                              "SUM(CASE WHEN COALESCE(sf.fabric_grade, 2) = 2 THEN COALESCE(sf.heightPK, 0) ELSE 0 END) AS الطول_الدرجة_الثانية, " &
                              "SUM(CASE WHEN COALESCE(sf.fabric_grade, 2) = 2 THEN COALESCE(sf.weightPK, 0) ELSE 0 END) AS الوزن_الدرجة_الثانية " &
                              "FROM store_finish sf " &
                              "WHERE CONVERT(DATE, sf.transaction_date) BETWEEN @dateFrom AND @dateTo " &
                              "GROUP BY CONVERT(DATE, sf.transaction_date);"

        Using conn As New SqlConnection(sqlServerConnectionString)
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@dateFrom", dateFrom.Date)
                cmd.Parameters.AddWithValue("@dateTo", dateTo.Date)

                Dim dt As New DataTable()
                Try
                    conn.Open()
                    Dim reader As SqlDataReader = cmd.ExecuteReader()
                    dt.Load(reader)
                Catch ex As Exception
                    MessageBox.Show("Error loading store finish data: " & ex.Message)
                End Try
                Return dt
            End Using
        End Using
    End Function

End Class

