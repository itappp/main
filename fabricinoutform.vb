Imports MySql.Data.MySqlClient

Public Class FabricInOutForm
    Private mysqlServerConnectionString As String = "Server=150.1.1.7;Database=wm;Uid=root1;Pwd=WMg2024$;"

    ' Method to fetch and display summary
    Private Sub SearchWorderByPrefix(ByVal worderPrefix As String)
        ' Query for the summary
        ' Query for the summary
        Dim summaryQuery As String = "SELECT g3.worder_prefix AS 'أمر شغل', " & _
                                     "DATE(MAX(g3.max_date)) AS 'أخر تاريخ خام', " & _
                                     "COALESCE(SUM(g3.sum_h), 0) AS 'فحص الخام', " & _
                                     "DATE(MAX(g2.max_date)) AS 'أخر تاريخ مجهز', " & _
                                     "COALESCE(SUM(g2.sum_h), 0) AS 'فحص المجهز', " & _
                                     "COALESCE(SUM(g3.sum_h), 0) - COALESCE(SUM(g2.sum_h), 0) AS 'هالك', " & _
                                     "CASE WHEN COALESCE(SUM(g3.sum_h), 0) > 0 THEN " & _
                                     "ROUND((COALESCE(SUM(g3.sum_h), 0) - COALESCE(SUM(g2.sum_h), 0)) * 100 / COALESCE(SUM(g3.sum_h), 0), 2) " & _
                                     "ELSE 0 END AS 'هالك نسبه' " & _
                                     "FROM ( " & _
                                     "  SELECT roll, LEFT(worder_id, 6) AS worder_prefix, SUM(DISTINCT h) AS sum_h, MAX(date) AS max_date " & _
                                     "  FROM gray_2 " & _
                                     "  GROUP BY roll, LEFT(worder_id, 6) " & _
                                     ") g2 " & _
                                     "RIGHT JOIN ( " & _
                                     "  SELECT roll, LEFT(worder_id, 6) AS worder_prefix, SUM(DISTINCT h) AS sum_h, MAX(date) AS max_date " & _
                                     "  FROM gray_3 " & _
                                     "  GROUP BY roll, LEFT(worder_id, 6) " & _
                                     ") g3 " & _
                                     "ON g2.worder_prefix = g3.worder_prefix AND g2.roll = g3.roll " & _
                                     "WHERE g3.worder_prefix LIKE @worder_prefix AND LENGTH(g3.worder_prefix) = 6 " & _
                                     "GROUP BY g3.worder_prefix;"
        Dim detailsQuery As String = "SELECT g3.worder_prefix AS 'أمر شغل', g3.roll AS 'رقم التوب تجهيز', g3.sum_h AS 'اجمالى متر خام', " &
                              "g2.worder_id AS 'أمر شغل خام', g2.roll AS 'رقم التوب خام', g2.sum_h AS 'اجمالى متر مجهز' " &
                              "FROM ( " &
                              "  SELECT roll, worder_id, SUM(DISTINCT h) AS sum_h " &
                              "  FROM gray_2 " &
                              "  GROUP BY roll, worder_id " &
                              ") g2 " &
                              "RIGHT JOIN ( " &
                              "  SELECT roll, worder_id, SUM(DISTINCT h) AS sum_h " &
                              "  FROM gray_3 " &
                              "  GROUP BY roll, worder_id " &
                              ") g3 " &
                              "ON g2.worder_id = g3.worder_id AND g2.roll = g3.roll " &
                              "WHERE LEFT(g3.worder_id, 6) = @worder_prefix;"


        Using connection As New MySqlConnection(mysqlServerConnectionString)
            Try
                connection.Open()

                ' Fetch summary data
                Using cmdSummary As New MySqlCommand(summaryQuery, connection)
                    cmdSummary.Parameters.AddWithValue("@worder_prefix", worderPrefix)
                    Dim adapterSummary As New MySqlDataAdapter(cmdSummary)
                    Dim tableSummary As New DataTable()
                    adapterSummary.Fill(tableSummary)
                    dgvResults.DataSource = tableSummary
                End Using

                ' Fetch detailed data
                Using cmdDetails As New MySqlCommand(detailsQuery, connection)
                    cmdDetails.Parameters.AddWithValue("@worder_prefix", worderPrefix)
                    Dim adapterDetails As New MySqlDataAdapter(cmdDetails)
                    Dim tableDetails As New DataTable()
                    adapterDetails.Fill(tableDetails)

                End Using

            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
    End Sub
    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        Dim worderPrefix As String = txtWorderPrefix.Text.Trim() ' Get the value from the text box

        If String.IsNullOrEmpty(worderPrefix) Then
            ' If the text box is empty, fetch all data
            SearchWorderByPrefix("%")
        ElseIf worderPrefix.Length >= 6 Then
            ' If there is a prefix entered, fetch data based on that prefix (first 6 characters)
            SearchWorderByPrefix(worderPrefix.Substring(0, 6))
        Else
            MessageBox.Show("Please enter at least 6 characters for the worder prefix.")
        End If
    End Sub

End Class
