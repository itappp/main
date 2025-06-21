Imports System.Data.SqlClient
Imports System.Text

Public Class requestbatchrawfromstoreform
    Dim sqlServerConnectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"
    Dim selectedWorkOrders As New List(Of String)()

    Private Sub requestbatchrawfromstoreform_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadWorkOrders()
    End Sub

    Private Sub LoadWorkOrders()
        Dim query As String = "SELECT worderid FROM techdata"
        Using connection As New SqlConnection(sqlServerConnectionString)
            Try
                connection.Open()
                Using cmd As New SqlCommand(query, connection)
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            cmbWorder.Items.Add(reader("worderid").ToString())
                        End While
                    End Using
                End Using
            Catch ex As SqlException
                MessageBox.Show("Error loading work orders: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Sub cmbWorder_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbWorder.SelectedIndexChanged
        If cmbWorder.SelectedItem IsNot Nothing Then
            Dim selectedOrder As String = cmbWorder.SelectedItem.ToString()
            If Not selectedWorkOrders.Contains(selectedOrder) Then
                selectedWorkOrders.Add(selectedOrder)
                UpdateSelectedWorkOrdersLabel()
                MessageBox.Show($"Added work order: {selectedOrder}")
            End If
        End If
    End Sub

    Private Sub UpdateSelectedWorkOrdersLabel()
        lblSelectedWorkOrders.Text = "Selected Work Orders: " & String.Join(", ", selectedWorkOrders)
    End Sub

    Private Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click
        If selectedWorkOrders.Count > 0 Then
            Dim htmlContent As New StringBuilder()
            htmlContent.AppendLine("<html>")
            htmlContent.AppendLine("<head>")
            htmlContent.AppendLine("<style>")
            htmlContent.AppendLine("table { width: 100%; border-collapse: collapse; font-size: 12px; }")
            htmlContent.AppendLine("th, td { border: 1px solid black; padding: 4px; text-align: left; }")
            htmlContent.AppendLine("th { background-color: #f2f2f2; }") ' Color for headers
            htmlContent.AppendLine("h2 { font-size: 14px; }")
            htmlContent.AppendLine("</style>")
            htmlContent.AppendLine("</head>")
            htmlContent.AppendLine("<body>")

            For Each worderId In selectedWorkOrders
                htmlContent.AppendLine(GenerateHTMLContent(worderId))
            Next

            htmlContent.AppendLine("</body>")
            htmlContent.AppendLine("</html>")

            ' Display HTML content in the WebBrowser control
            WebBrowser.DocumentText = htmlContent.ToString()
        Else
            MessageBox.Show("Please select at least one work order.")
        End If
    End Sub

    Private Function GenerateHTMLContent(worderId As String) As String
        Dim query As String = "SELECT t.qty_kg, c.contractno, t.qty_m, c.Batch, c.lot, c.QuantityK, c.QuantityM, cs.code, c.Material " &
                          "FROM techdata t " &
                          "JOIN contracts c ON t.contract_id = c.contractid " &
                          "JOIN clients cs ON c.clientCode = cs.id " &
                          "WHERE t.worderid = @worderid"
        Using connection As New SqlConnection(sqlServerConnectionString)
            Try
                connection.Open()
                Using cmd As New SqlCommand(query, connection)
                    cmd.Parameters.AddWithValue("@worderid", worderId)
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            Dim contractno As String = reader("contractno").ToString()
                            Dim qtyKg As String = reader("qty_kg").ToString()
                            Dim qtyM As String = reader("qty_m").ToString()
                            Dim batch As String = reader("Batch").ToString()
                            Dim lot As String = reader("lot").ToString()
                            Dim salesQtyKg As String = reader("QuantityK").ToString()
                            Dim salesQtyM As String = reader("QuantityM").ToString()
                            Dim clientCode As String = reader("code").ToString()
                            Dim materialType As String = reader("Material").ToString()

                            ' Create HTML content for a single work order
                            Dim htmlContent As New StringBuilder()
                            htmlContent.AppendLine("<h2>إذن صرف قماش خام للتشغيل</h2>")
                            ' Add small table for headers
                            htmlContent.AppendLine("<table>")
                            htmlContent.AppendLine("<tr><th>أمر الشغل</th><th>رقم التعاقد</th><th>رقم الرسالة</th><th>Lot</th><th>كود العميل</th><th>نوع الخامه</th><th>اللون</th></tr>")
                            htmlContent.AppendLine($"<tr><td>{worderId}</td><td>{contractno}</td><td>{batch}</td><td>{lot}</td><td>{clientCode}</td><td>{materialType}</td><td>{materialType}</td></tr>")
                            htmlContent.AppendLine("</table>")
                            htmlContent.AppendLine("<br/>")
                            ' Add main table with side columns
                            htmlContent.AppendLine("<table>")
                            htmlContent.AppendLine("<tr><th colspan='2'>التخطيط</th><th colspan='2'>التعاقد</th></tr>")
                            htmlContent.AppendLine($"<tr><td>تخطيط(kg)</td><td>{qtyKg}</td><td>تعاقد(Kg)</td><td>{salesQtyKg}</td></tr>")
                            htmlContent.AppendLine($"<tr><td>تخطيط(M)</td><td>{qtyM}</td><td>تعاقد(M)</td><td>{salesQtyM}</td></tr>")
                            htmlContent.AppendLine("</table>")
                            htmlContent.AppendLine("<br/>")

                            Return htmlContent.ToString()
                        Else
                            Return "<p>No data found for the selected work order.</p>"
                        End If
                    End Using
                End Using
            Catch ex As SqlException
                Return $"<p>Error generating HTML: {ex.Message}</p>"
            End Try
        End Using
    End Function
End Class

