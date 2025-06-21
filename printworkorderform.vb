Imports MySql.Data.MySqlClient
Imports System.Data.SqlClient
Imports System.Text
Imports ZXing
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Printing
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.tool.xml
Imports ClosedXML.Excel
Imports System.Diagnostics

Public Class PrintWorkOrderForm
    Private sqlServerConnectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"
    Private Shadows WithEvents contextMenu As New ContextMenuStrip()
    Private WithEvents printPreviewMenuItem As New ToolStripMenuItem("Print Preview")
    Private WithEvents btnPrintPreview As New Button()

    Private printDocument As New PrintDocument()
    Private Sub PrintWorkOrderForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadWorkOrderIDs()
        ' إعداد زر btnPrint2
        btnPrint2.Text = "Print Preview"
        AddHandler btnPrint2.Click, AddressOf btnPrint2_Click
        Me.Controls.Add(btnPrint2)
        webBrowserPreview.ScriptErrorsSuppressed = True


    End Sub


    Private Sub btnPrint2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' إعداد إعدادات الطباعة
        AddHandler webBrowserPreview.DocumentCompleted, AddressOf WebBrowserPreview_DocumentCompleted
        webBrowserPreview.ShowPrintPreviewDialog()

    End Sub


    Private Sub printDocument_PrintPage(ByVal sender As Object, ByVal e As PrintPageEventArgs)
        ' إعداد الهوامش وحجم الورق
        e.PageSettings.Margins = New Margins(0, 0, 0, 0)
        e.PageSettings.PaperSize = New PaperSize("A4", 827, 1169)

        ' تنسيق البيانات للطباعة
        Dim font As New System.Drawing.Font("Arial", 12)
        Dim brush As New SolidBrush(System.Drawing.Color.Black)
        Dim x As Integer = e.MarginBounds.Left
        Dim y As Integer = e.MarginBounds.Top

        ' طباعة محتوى WebBrowser
        Dim content As String = webBrowserPreview.DocumentText
        e.Graphics.DrawString(content, font, brush, x, y)
    End Sub
    Private Sub WebBrowserPreview_DocumentCompleted(ByVal sender As Object, ByVal e As WebBrowserDocumentCompletedEventArgs)
        webBrowserPreview.Print()
        RemoveHandler webBrowserPreview.DocumentCompleted, AddressOf WebBrowserPreview_DocumentCompleted
    End Sub

    Private Sub LoadWorkOrderIDs()
        Dim query As String = "SELECT DISTINCT worderid FROM techdata"

        Using conn As New SqlConnection(sqlServerConnectionString)
            Dim cmd As New SqlCommand(query, conn)
            Try
                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()

                cmbWorkOrder.Items.Clear()
                While reader.Read()
                    cmbWorkOrder.Items.Add(reader("worderid").ToString())
                End While
            Catch ex As Exception
                MessageBox.Show("Error loading Work Order IDs: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim workOrderID As String = cmbWorkOrder.Text
        If String.IsNullOrEmpty(workOrderID) Then
            MessageBox.Show("Please select a Work Order ID.")
            Return
        End If

        ' Generate barcode for the work order ID
        Dim barcodeWriter As New BarcodeWriter()
        barcodeWriter.Format = BarcodeFormat.CODE_128

        ' Set the dimensions for the barcode
        Dim options As New ZXing.Common.EncodingOptions()
        options.Width = 300 ' Set the desired width
        options.Height = 100 ' Set the desired height
        barcodeWriter.Options = options

        Dim barcodeBitmap As Bitmap = barcodeWriter.Write(workOrderID)
        Dim barcodeBase64 As String
        Using ms As New MemoryStream()
            barcodeBitmap.Save(ms, Imaging.ImageFormat.Png)
            barcodeBase64 = Convert.ToBase64String(ms.ToArray())
        End Using

        ' Query to fetch all work order details
        Dim query As String = "SELECT DISTINCT td.worderid AS 'امر شغل', " &
              "c.ContractNo AS 'رقم التعاقد', cli.Code AS 'كود العميل',c.WidthReq AS 'العرض المطلوب', " &
              "c.lot AS 'الرساله', c.color AS 'اللون', c.Material AS 'الخامه', l.code AS 'كود المكتبة', " &
              "td.qty_m AS 'كمية متر (tech)', td.qty_kg AS 'كمية كيلو (tech)', c.QuantityM AS 'تعاقد متر', " &
              "c.QuantityK AS 'تعاقد وزن', c.WeightM AS 'وزن المتر المربع المطلوب', c.RollM AS 'طول التوب المطلوب', " &
              " c.fabriccode AS 'كود الخامة', c.refno AS 'رقم الاذن',  CONVERT(VARCHAR(10), c.ContractDate, 111) AS 'Contract',CONVERT(VARCHAR(10), td.created_date, 111) AS 'Worder', CONVERT(VARCHAR(10), td.Delivery_Dat, 111) AS 'Delievry', td.notes AS 'ملاحظات المكتب الفنى', c.notes AS 'ملاحظات البيع'" &
              "FROM techdata td " &
              "LEFT JOIN contracts c ON td.contract_id = c.contractid " &
              "LEFT JOIN lib l ON td.new_code_lib = l.code_id " &
              "LEFT JOIN clients cli ON c.ClientCode = cli.id " &
              "LEFT JOIN fabric fb ON c.ContractType = fb.id " &
              "WHERE td.worderid = @worderid"

        Using conn As New SqlConnection(sqlServerConnectionString)
            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@worderid", workOrderID)
            Try
                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                If reader.HasRows Then
                    ' Read all data into a list of dictionaries
                    Dim data As New List(Of Dictionary(Of String, String))()
                    While reader.Read()
                        Dim row As New Dictionary(Of String, String)()
                        For i As Integer = 0 To reader.FieldCount - 1
                            row.Add(reader.GetName(i), reader(i).ToString())
                        Next
                        data.Add(row)
                    End While

                    Dim htmlContent As New StringBuilder()
                    ' Inside the btnPrint_Click method, update the CSS for the HTML content
                    htmlContent.Append("<html><head><style>")
                    htmlContent.Append("@page { size: A4 landscape; margin: 0; }") ' ضبط الهوامش لتكون 0
                    htmlContent.Append("body { direction: rtl; text-align: right; margin: 0; font-family: Arial, sans-serif; transform: scale(0.7); transform-origin: 0 0; }") ' ضبط حجم الطباعة إلى 70%
                    htmlContent.Append("table { width: 100%; border-collapse: collapse; margin-bottom: 10px; }") ' تقليل المسافة بين الجداول
                    htmlContent.Append("th, td { border: 1px solid black; padding: 5px; text-align: center; }") ' تقليل الحشو إلى 5px
                    htmlContent.Append("th { font-size: 16px; font-weight: bold; background-color: #f2f2f2; }") ' إضافة لون خلفية للرؤوس
                    htmlContent.Append("td { font-size: 15px; font-weight: bold;}")
                    htmlContent.Append("tr:nth-child(even) { background-color: #f9f9f9; }") ' إضافة لون خلفية للصفوف الزوجية
                    htmlContent.Append("tr:hover { background-color: #f1f1f1; }") ' إضافة تأثير التمرير
                    htmlContent.Append("th.stage-order, td.stage-order { width: 50px; }")
                    htmlContent.Append("th.stage-name, td.stage-name { width: auto; }")
                    htmlContent.Append(".header { display: flex; justify-content: space-between; align-items: flex-start; margin-bottom: 0; }") ' استخدام flexbox للرأس
                    htmlContent.Append(".header img { margin-left: auto; }") ' إضافة هامش للباركود
                    htmlContent.Append(".header h1 { margin: 0; text-align: center; flex-grow: 1; }") ' توسيط العنوان
                    htmlContent.Append(".info-table { margin-top: 0; margin-bottom: 10px; width: auto; border-collapse: collapse; }") ' تقليل المسافة بين الجداول
                    htmlContent.Append(".info-table th, .info-table td { border: 1px solid black; padding: 5px; text-align: left; }") ' تقليل الحشو إلى 5px
                    htmlContent.Append(".small-table { position: absolute; top: 70px; left: 0; width: 140px; font-size: 7px; }") ' تنسيق الجدول الصغير
                    htmlContent.Append("</style></head><body>")


                    htmlContent.Append("<div class='header'>")
                    htmlContent.Append("<img src='\\180.1.1.6\new app\images\wagdymoamen.jpg' style='position: absolute; top: 0; left: 0; width: 140px; height: 70px;' />")
                    htmlContent.AppendFormat("<div style='position: absolute; top: 0; right: 0; text-align: right;'><img src='data:image/png;base64,{0}' class='barcode' style='width: 220px; height: 70px;' /><br /><table style='width: 200px; font-size: 14px; font-weight: bold;'><tr><td>{1}</td></tr></table></div>", barcodeBase64, data(0)("كود العميل"))
                    htmlContent.Append("</div>")
                    htmlContent.Append("<div style='text-align: center;'>")
                    htmlContent.Append("<table style='width: 30%; border: 1px solid black; margin-left: auto; margin-right: auto; display: block;'><tr><td style='height: 50px;'></td></tr></table>")

                    htmlContent.Append("<h2>" & data(0)("امر شغل") & "</h2>")
                    htmlContent.Append("<h2>" & data(0)("الخامه") & "</h2>")
                    htmlContent.Append("</div>")

                    ' Split columns into two groups: first 10 columns and remaining columns
                    Dim firstGroupColumns As New List(Of String)()
                    Dim secondGroupColumns As New List(Of String)()
                    For i As Integer = 0 To data(0).Count - 1
                        If i < 10 Then
                            firstGroupColumns.Add(data(0).Keys(i))
                        Else
                            secondGroupColumns.Add(data(0).Keys(i))
                        End If
                    Next

                    ' Remove "امر شغل" from the first group columns
                    firstGroupColumns.Remove("امر شغل")
                    firstGroupColumns.Remove("كمية متر (tech)")
                    firstGroupColumns.Remove("كمية كيلو (tech)")
                    firstGroupColumns.Remove("كود العميل")
                    firstGroupColumns.Remove("الخامه")
                    secondGroupColumns.Remove("ملاحظات المكتب الفنى")
                    secondGroupColumns.Remove("ملاحظات البيع")

                    ' Add first group table
                    htmlContent.Append("<table>")
                    ' Add table headers
                    htmlContent.Append("<tr>")
                    For Each columnName As String In firstGroupColumns
                        htmlContent.AppendFormat("<th>{0}</th>", columnName)
                    Next
                    htmlContent.Append("</tr>")
                    ' Add table rows
                    For Each row As Dictionary(Of String, String) In data
                        htmlContent.Append("<tr>")
                        For Each columnName As String In firstGroupColumns
                            htmlContent.AppendFormat("<td>{0}</td>", row(columnName))
                        Next
                        htmlContent.Append("</tr>")
                    Next
                    htmlContent.Append("</table>")

                    ' Add second group table
                    htmlContent.Append("<table>")
                    ' Add table headers
                    htmlContent.Append("<tr>")
                    For Each columnName As String In secondGroupColumns
                        htmlContent.AppendFormat("<th>{0}</th>", columnName)
                    Next
                    htmlContent.Append("</tr>")
                    ' Add table rows
                    For Each row As Dictionary(Of String, String) In data
                        htmlContent.Append("<tr>")
                        For Each columnName As String In secondGroupColumns
                            htmlContent.AppendFormat("<td>{0}</td>", row(columnName))
                        Next
                        htmlContent.Append("</tr>")
                    Next
                    htmlContent.Append("</table>")

                    ' Add the small table for كمية متر (tech) and كمية كيلو (tech) under the image
                    htmlContent.Append("<table class='small-table' style='width: 140px; font-size: 16px; margin-bottom: 24px;'>")
                    htmlContent.Append("<tr>")
                    htmlContent.Append("<th style='font-size: 14px;'>M</th>")
                    htmlContent.Append("<th style='font-size: 14px;'>KG</th>")
                    htmlContent.Append("</tr>")
                    htmlContent.Append("<tr>")
                    htmlContent.Append("<td style='font-size: 14px;'>" & data(0)("كمية متر (tech)") & "</td>")
                    htmlContent.Append("<td style='font-size: 14px;'>" & data(0)("كمية كيلو (tech)") & "</td>")
                    htmlContent.Append("</tr>")
                    htmlContent.Append("</table>")

                    ' Close the first reader before executing the second command
                    reader.Close()

                    ' Add quality data table
                    cmd.CommandText = "SELECT [raw_befor_weight], [raw_after_weight], [tensile_weft], [tensile_warp], [tensile_result], " &
                              "[tear_weft], [tear_warp], [tear_result], [pva_Starch], [color_water], [washing], [color_mercerize], " &
                              "[mix_rate], qc.[notes], [raw_after_width], [raw_befor_width] " &
                              "FROM qc_raw_test qc " &
                              "WHERE id = (SELECT new_qc_id FROM techdata WHERE worderid = @worderid)"
                    reader = cmd.ExecuteReader()

                    If reader.HasRows Then
                        reader.Read() ' Read the first row of the result set
                        htmlContent.Append("<table class='quality-table'>")

                        ' Add table headers for the first 8 columns
                        htmlContent.Append("<tr>")
                        htmlContent.Append("<th>وزن قبل</th>")
                        htmlContent.Append("<th>وزن بعد</th>")
                        htmlContent.Append("<th>عرض بعد</th>")
                        htmlContent.Append("<th>عرض قبل</th>")
                        htmlContent.Append("<th>Sizing Type</th>")
                        htmlContent.Append("<th>washing</th>")
                        htmlContent.Append("<th>color water</th>")
                        htmlContent.Append("<th>color mercerize</th>")

                        htmlContent.Append("</tr>")

                        ' Add table rows for the first 8 columns
                        htmlContent.Append("<tr>")
                        htmlContent.AppendFormat("<td>{0}</td>", reader("raw_befor_weight").ToString())
                        htmlContent.AppendFormat("<td>{0}</td>", reader("raw_after_weight").ToString())
                        htmlContent.AppendFormat("<td>{0}</td>", reader("raw_after_width").ToString())
                        htmlContent.AppendFormat("<td>{0}</td>", reader("raw_befor_width").ToString())
                        htmlContent.AppendFormat("<td>{0}</td>", reader("pva_Starch").ToString())
                        htmlContent.AppendFormat("<td>{0}</td>", reader("washing").ToString())
                        htmlContent.AppendFormat("<td>{0}</td>", reader("color_water").ToString())
                        htmlContent.AppendFormat("<td>{0}</td>", reader("color_mercerize").ToString())

                        htmlContent.Append("</tr>")

                        ' Add table headers for the remaining columns
                        htmlContent.Append("<tr>")
                        htmlContent.Append("<th>tensile_weft</th>")
                        htmlContent.Append("<th>tensile_warp</th>")
                        htmlContent.Append("<th>tensile_result</th>")
                        htmlContent.Append("<th>tear_weft</th>")
                        htmlContent.Append("<th>tear_warp</th>")
                        htmlContent.Append("<th>tear_result</th>")
                        htmlContent.Append("<th>mix rate</th>")
                        htmlContent.Append("<th>notes</th>")
                        htmlContent.Append("</tr>")

                        ' Add table rows for the remaining columns
                        htmlContent.Append("<tr>")
                        htmlContent.AppendFormat("<td>{0}</td>", reader("tensile_weft").ToString())
                        htmlContent.AppendFormat("<td>{0}</td>", reader("tensile_warp").ToString())
                        htmlContent.AppendFormat("<td>{0}</td>", reader("tensile_result").ToString())
                        htmlContent.AppendFormat("<td>{0}</td>", reader("tear_weft").ToString())
                        htmlContent.AppendFormat("<td>{0}</td>", reader("tear_warp").ToString())
                        htmlContent.AppendFormat("<td>{0}</td>", reader("tear_result").ToString())
                        htmlContent.AppendFormat("<td>{0}</td>", reader("mix_rate").ToString())
                        htmlContent.AppendFormat("<td>{0}</td>", reader("notes").ToString())
                        htmlContent.Append("</tr>")

                        htmlContent.Append("</table>")
                    End If

                    ' Close the second reader before executing the third command
                    reader.Close()

                    ' Add sales notes table
                    cmd.CommandText = "SELECT td.notes AS 'ملاحظات المكتب الفنى', c.notes AS 'ملاحظات البيع', td.defect AS 'العيب', td.fromdep AS 'من القسم', td.stagereason AS 'سبب المرحلة', td.stagefix AS 'إصلاح المرحلة' FROM techdata td LEFT JOIN contracts c ON td.contract_id = c.contractid WHERE td.worderid = @worderid"
                    reader = cmd.ExecuteReader()

                    If reader.HasRows Then
                        reader.Read() ' Read the first row of the result set
                        htmlContent.Append("<table class='sales-notes-table'>")

                        ' Add table headers
                        htmlContent.Append("<tr>")
                        htmlContent.Append("<th>ملاحظات المكتب الفنى</th>")
                        htmlContent.Append("<th>ملاحظات البيع</th>")
                        htmlContent.Append("<th>العيب</th>")
                        htmlContent.Append("<th>من القسم</th>")
                        htmlContent.Append("<th>سبب المرحلة</th>")
                        htmlContent.Append("<th>إصلاح المرحلة</th>")
                        htmlContent.Append("</tr>")

                        ' Add table rows
                        htmlContent.Append("<tr>")
                        htmlContent.AppendFormat("<td>{0}</td>", reader("ملاحظات المكتب الفنى").ToString())
                        htmlContent.AppendFormat("<td>{0}</td>", reader("ملاحظات البيع").ToString())
                        htmlContent.AppendFormat("<td>{0}</td>", reader("العيب").ToString())
                        htmlContent.AppendFormat("<td>{0}</td>", reader("من القسم").ToString())
                        htmlContent.AppendFormat("<td>{0}</td>", reader("سبب المرحلة").ToString())
                        htmlContent.AppendFormat("<td>{0}</td>", reader("إصلاح المرحلة").ToString())
                        htmlContent.Append("</tr>")

                        htmlContent.Append("</table>")
                    End If
                    reader.Close()
                    ' تعديل الاستعلام لإضافة الأعمدة المطلوبة من جدول batch_lot_defect
                    cmd.CommandText = "SELECT " &
                  "bd.pva_Starch AS 'PVA Starch', bd.needle AS 'Needle', bd.Separation AS 'Separation', " &
                  "bd.Durability AS 'Durability', bd.notes AS 'ملاحظات العيوب',bd.Raw_Moisture as 'الرطوبة' " &
                  "FROM techdata td " &
                  "LEFT JOIN contracts c ON td.contract_id = c.contractid " &
                  "LEFT JOIN batch_lot_defect bd ON c.lot = bd.lot " &
                  "WHERE td.worderid = @worderid"

                    reader = cmd.ExecuteReader()

                    If reader.HasRows Then
                        reader.Read() ' Read the first row of the result set
                        htmlContent.Append("<table class='sales-notes-table'>")

                        ' Add table headers
                        htmlContent.Append("<tr>")

                        htmlContent.Append("<th>نشا</th>")
                        htmlContent.Append("<th>ابره شق</th>")
                        htmlContent.Append("<th>التفصيد</th>")
                        htmlContent.Append("<th>المتانه</th>")
                        htmlContent.Append("<th>الرطوبة</th>")
                        htmlContent.Append("<th>ملاحظات العيوب</th>")
                        htmlContent.Append("</tr>")

                        ' Add table rows
                        htmlContent.Append("<tr>")

                        htmlContent.AppendFormat("<td>{0}</td>", reader("PVA Starch").ToString())
                        htmlContent.AppendFormat("<td>{0}</td>", reader("Needle").ToString())
                        htmlContent.AppendFormat("<td>{0}</td>", reader("Separation").ToString())
                        htmlContent.AppendFormat("<td>{0}</td>", reader("Durability").ToString())
                        htmlContent.AppendFormat("<td>{0}</td>", reader("الرطوبة").ToString())
                        htmlContent.AppendFormat("<td>{0}</td>", reader("ملاحظات العيوب").ToString())
                        htmlContent.Append("</tr>")

                        htmlContent.Append("</table>")
                    End If

                    htmlContent.Append("<table>")



                    ' Add table headers for stages
                    htmlContent.Append("<table>")
                    htmlContent.Append("<tr>")
                    htmlContent.Append("<th class='stage-order'>ترتيب المرحله</th>")
                    htmlContent.Append("<th class='stage-name'>المراحل</th>")
                    htmlContent.Append("<th>التاريخ</th>")
                    htmlContent.Append("<th>الورديه</th>")
                    htmlContent.Append("<th>عرض in</th>")
                    htmlContent.Append("<th>عرض out</th>")
                    htmlContent.Append("<th>دخول وقت</th>")
                    htmlContent.Append("<th>خروج وقت</th>")
                    htmlContent.Append("<th>متر</th>")
                    htmlContent.Append("<th>كيلو</th>")
                    htmlContent.Append("<th>رقم الإفريم</th>")
                    htmlContent.Append("<th>العامل</th>")
                    htmlContent.Append("<th class='notes'>ملاحظات</th>")
                    htmlContent.Append("</tr>")

                    ' Reset the reader to fetch stages data
                    reader.Close()
                    cmd.CommandText = "SELECT l.steps_num as 'ترتيب المرحله', CONCAT(np.proccess_ar, ' (', nm.name_ar, ')') as 'المراحل' " &
    "FROM techdata td " &
    "LEFT JOIN lib l ON td.new_code_lib = l.code_id " &
    "LEFT JOIN new_proccess np ON l.proccess_id = np.id " &
    "LEFT JOIN new_machines nm ON np.machine_id = nm.id " &
    "WHERE td.worderid = @worderid"

                    reader = cmd.ExecuteReader()

                    ' Add table rows for stages
                    While reader.Read()
                        htmlContent.Append("<tr>")
                        htmlContent.AppendFormat("<td class='stage-order'>{0}</td>", reader("ترتيب المرحله").ToString())
                        htmlContent.AppendFormat("<td class='stage-name'>{0}</td>", reader("المراحل").ToString())
                        htmlContent.Append("<td></td>") ' Empty cell for التاريخ
                        htmlContent.Append("<td></td>") ' Empty cell for الورديه
                        htmlContent.Append("<td></td>") ' Empty cell for عرض in
                        htmlContent.Append("<td></td>") ' Empty cell for عرض out
                        htmlContent.Append("<td></td>") ' Empty cell for دخول وقت
                        htmlContent.Append("<td></td>") ' Empty cell for خروج وقت
                        htmlContent.Append("<td></td>") ' Empty cell for متر
                        htmlContent.Append("<td></td>") ' Empty cell for كيلو
                        htmlContent.Append("<td></td>") ' Empty cell for رقم الإفريم
                        htmlContent.Append("<td></td>") ' Empty cell for العامل
                        htmlContent.Append("<td class='notes'></td>") ' Empty cell for ملاحظات
                        htmlContent.Append("</tr>")
                    End While

                    htmlContent.Append("</table>")


                    ' Close the HTML tags
                    htmlContent.Append("</body></html>")

                    ' Display the HTML content in the WebBrowser control
                    webBrowserPreview.DocumentText = htmlContent.ToString()

                Else
                    MessageBox.Show("No data found for the selected Work Order ID.")
                End If
            Catch ex As Exception
                MessageBox.Show("Error retrieving Work Order details: " & ex.Message)
            End Try
        End Using
    End Sub
End Class







