Imports System.Data.SqlClient
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Drawing.Printing
Imports ZXing
Imports ZXing.Common

Public Class printbarcoderawform
    Private connectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"
    Private WithEvents cmbBatch As New ComboBox()
    Private WithEvents cmbLot As New ComboBox()
    Private WithEvents dgvRolls As New DataGridView()
    Private WithEvents lblBatch As New Label()
    Private WithEvents lblLot As New Label()
    Private loadingData As Boolean = False
    Private previewImage As Image = Nothing
    Private currentZplCode As String = ""

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        SetupForm()
    End Sub

    Private Sub SetupForm()
        ' تعيين خصائص النموذج
        Me.Text = "طباعة الباركود"
        Me.Size = New Size(1024, 768)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.RightToLeft = RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.Font = New Font("Arial", 12)

        ' إنشاء TableLayoutPanel للتحكم في تخطيط النموذج
        Dim mainLayout As New TableLayoutPanel()
        mainLayout.Dock = DockStyle.Fill
        mainLayout.ColumnCount = 1
        mainLayout.RowCount = 2
        mainLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100))
        mainLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 100)) ' للتحكم العلوي
        mainLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 100)) ' للجدول
        Me.Controls.Add(mainLayout)

        ' Panel for controls at top
        Dim controlPanel As New Panel()
        controlPanel.Dock = DockStyle.Fill
        controlPanel.Padding = New Padding(10)
        mainLayout.Controls.Add(controlPanel, 0, 0)

        ' Labels setup
        lblBatch.Text = "رقم الرسالة:"
        lblBatch.AutoSize = True
        lblBatch.Font = New Font("Arial", 12, FontStyle.Bold)
        lblBatch.Location = New Point(controlPanel.Width - lblBatch.Width - 20, 20)
        controlPanel.Controls.Add(lblBatch)

        lblLot.Text = "رقم اللوت:"
        lblLot.AutoSize = True
        lblLot.Font = New Font("Arial", 12, FontStyle.Bold)
        lblLot.Location = New Point(controlPanel.Width - lblLot.Width - 20, 60)
        controlPanel.Controls.Add(lblLot)

        ' ComboBoxes setup with AutoComplete
        cmbBatch.Size = New Size(250, 30)
        cmbBatch.Location = New Point(lblBatch.Left - cmbBatch.Width - 10, 15)
        cmbBatch.DropDownStyle = ComboBoxStyle.DropDown
        cmbBatch.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cmbBatch.AutoCompleteSource = AutoCompleteSource.ListItems
        controlPanel.Controls.Add(cmbBatch)

        cmbLot.Size = New Size(250, 30)
        cmbLot.Location = New Point(lblLot.Left - cmbLot.Width - 10, 55)
        cmbLot.DropDownStyle = ComboBoxStyle.DropDown
        cmbLot.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cmbLot.AutoCompleteSource = AutoCompleteSource.ListItems
        controlPanel.Controls.Add(cmbLot)

        ' Panel for DataGridView
        Dim dgvPanel As New Panel()
        dgvPanel.Dock = DockStyle.Fill
        dgvPanel.Padding = New Padding(10)
        mainLayout.Controls.Add(dgvPanel, 0, 1)

        ' إعداد DataGridView
        dgvRolls.Dock = DockStyle.Fill
        dgvRolls.AllowUserToAddRows = False
        dgvRolls.AllowUserToDeleteRows = False
        dgvRolls.ReadOnly = True
        dgvRolls.MultiSelect = False
        dgvRolls.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvRolls.RowHeadersVisible = False
        dgvRolls.Font = New Font("Arial", 12)
        dgvRolls.RightToLeft = RightToLeft.Yes
        dgvRolls.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvRolls.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvRolls.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        dgvRolls.ColumnHeadersHeight = 40
        dgvRolls.RowTemplate.Height = 35
        dgvRolls.EnableHeadersVisualStyles = False
        dgvRolls.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray
        dgvRolls.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 12, FontStyle.Bold)
        dgvRolls.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
        dgvRolls.ScrollBars = ScrollBars.Both
        dgvRolls.BorderStyle = BorderStyle.Fixed3D
        dgvRolls.BackgroundColor = Color.White
        dgvPanel.Controls.Add(dgvRolls)

        ' Load initial data
        LoadBatches()
    End Sub

    Private Sub LoadBatches()
        Try
            loadingData = True
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim query As String = "SELECT DISTINCT batch FROM batch_details_rolls ORDER BY batch DESC"
                Using cmd As New SqlCommand(query, conn)
                    Dim dt As New DataTable()
                    dt.Load(cmd.ExecuteReader())
                    cmbBatch.DataSource = Nothing
                    cmbBatch.Items.Clear()
                    cmbBatch.DataSource = dt
                    cmbBatch.DisplayMember = "batch"
                    cmbBatch.ValueMember = "batch"
                    cmbBatch.SelectedIndex = -1
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل الرسائل: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            loadingData = False
        End Try
    End Sub

    Private Sub cmbBatch_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbBatch.SelectedIndexChanged
        If Not loadingData AndAlso cmbBatch.SelectedValue IsNot Nothing Then
            LoadLots(cmbBatch.SelectedValue.ToString())
        End If
    End Sub

    Private Sub cmbBatch_TextChanged(sender As Object, e As EventArgs) Handles cmbBatch.TextChanged
        If Not loadingData AndAlso Not String.IsNullOrEmpty(cmbBatch.Text) Then
            Dim matchingItem = cmbBatch.Items.Cast(Of DataRowView)().
                FirstOrDefault(Function(x) x("batch").ToString().Contains(cmbBatch.Text))
            If matchingItem IsNot Nothing Then
                cmbBatch.SelectedItem = matchingItem
            End If
        End If
    End Sub

    Private Sub LoadLots(batchNumber As String)
        Try
            loadingData = True
            cmbLot.DataSource = Nothing
            cmbLot.Items.Clear()

            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim query As String = "SELECT DISTINCT lot FROM batch_details_rolls WHERE batch = @batch ORDER BY lot"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@batch", batchNumber)
                    Dim dt As New DataTable()
                    dt.Load(cmd.ExecuteReader())
                    cmbLot.DataSource = dt
                    cmbLot.DisplayMember = "lot"
                    cmbLot.ValueMember = "lot"
                    cmbLot.SelectedIndex = -1
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل اللوت: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            loadingData = False
        End Try
    End Sub

    Private Sub cmbLot_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbLot.SelectedIndexChanged
        If Not loadingData AndAlso cmbLot.SelectedValue IsNot Nothing AndAlso cmbBatch.SelectedValue IsNot Nothing Then
            LoadRolls(cmbBatch.SelectedValue.ToString(), cmbLot.SelectedValue.ToString())
        End If
    End Sub

    Private Sub cmbLot_TextChanged(sender As Object, e As EventArgs) Handles cmbLot.TextChanged
        If Not loadingData AndAlso Not String.IsNullOrEmpty(cmbLot.Text) Then
            Dim matchingItem = cmbLot.Items.Cast(Of DataRowView)().
                FirstOrDefault(Function(x) x("lot").ToString().Contains(cmbLot.Text))
            If matchingItem IsNot Nothing Then
                cmbLot.SelectedItem = matchingItem
            End If
        End If
    End Sub

    Private Sub LoadRolls(batchNumber As String, lotNumber As String)
        Try
            dgvRolls.SuspendLayout()
            dgvRolls.Columns.Clear()
            dgvRolls.DataSource = Nothing
            dgvRolls.TopLeftHeaderCell.Value = Nothing

            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim query As String = "SELECT roll AS 'رقم التوب', " &
                                    "weight AS 'الوزن', " &
                                    "location AS 'الموقع', " &
                                    "date AS 'التاريخ' " &
                                    "FROM batch_details_rolls " &
                                    "WHERE batch = @batch AND lot = @lot " &
                                    "ORDER BY roll"

                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@batch", batchNumber)
                    cmd.Parameters.AddWithValue("@lot", lotNumber)
                    Dim dt As New DataTable()
                    dt.Load(cmd.ExecuteReader())

                    dgvRolls.DataSource = dt

                    ' إضافة عمود زر الطباعة
                    Dim printButtonColumn As New DataGridViewButtonColumn()
                    printButtonColumn.Name = "PrintButton"
                    printButtonColumn.HeaderText = "طباعة"
                    printButtonColumn.Text = "طباعة"
                    printButtonColumn.UseColumnTextForButtonValue = True
                    dgvRolls.Columns.Add(printButtonColumn)

                    ' تنسيق الأعمدة
                    For Each col As DataGridViewColumn In dgvRolls.Columns
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                        If col.Name = "PrintButton" Then
                            col.Width = 80
                        End If
                    Next

                    ' تطبيق التنسيق على الخلايا
                    dgvRolls.DefaultCellStyle.Padding = New Padding(5)
                    For Each row As DataGridViewRow In dgvRolls.Rows
                        row.Height = 35
                    Next

                    ' التأكد من عرض البيانات من البداية
                    If dgvRolls.Rows.Count > 0 Then
                        dgvRolls.FirstDisplayedScrollingRowIndex = 0
                        dgvRolls.CurrentCell = dgvRolls.Rows(0).Cells(0)
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل بيانات الأتواب: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            dgvRolls.ResumeLayout()
            Application.DoEvents()
        End Try
    End Sub

    Private Sub dgvRolls_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvRolls.CellContentClick
        If e.ColumnIndex = dgvRolls.Columns("PrintButton").Index AndAlso e.RowIndex >= 0 Then
            PrintLabel(dgvRolls.Rows(e.RowIndex))
        End If
    End Sub

    Private Sub ShowPrintPreview(zplCode As String)
        Try
            currentZplCode = zplCode
            Dim previewForm As New Form()
            previewForm.Text = "معاينة الطباعة"
            previewForm.Size = New Size(850, 600)
            previewForm.StartPosition = FormStartPosition.CenterScreen
            previewForm.RightToLeft = RightToLeft.Yes
            previewForm.RightToLeftLayout = True

            ' إضافة Panel للمحتوى
            Dim contentPanel As New Panel()
            contentPanel.Dock = DockStyle.Fill
            contentPanel.BackColor = Color.White
            contentPanel.Padding = New Padding(20)
            previewForm.Controls.Add(contentPanel)

            ' إنشاء Panel للمعاينة
            Dim previewPanel As New Panel()
            previewPanel.Size = New Size(792, 400)  ' نفس حجم الملصق
            previewPanel.BackColor = Color.White
            previewPanel.BorderStyle = BorderStyle.FixedSingle
            previewPanel.Location = New Point((contentPanel.Width - previewPanel.Width) \ 2, 20)
            AddHandler previewPanel.Paint, AddressOf PreviewPanel_Paint
            contentPanel.Controls.Add(previewPanel)

            ' إضافة لوحة الأزرار
            Dim buttonPanel As New Panel()
            buttonPanel.Dock = DockStyle.Bottom
            buttonPanel.Height = 60
            buttonPanel.Padding = New Padding(10)
            previewForm.Controls.Add(buttonPanel)

            ' زر الطباعة
            Dim printButton As New Button()
            printButton.Text = "طباعة"
            printButton.Width = 120
            printButton.Height = 35
            printButton.Font = New Font("Arial", 12, FontStyle.Bold)
            printButton.Location = New Point(buttonPanel.Width - 140, 12)
            AddHandler printButton.Click, Sub()
                                              PrintZpl(zplCode)
                                              previewForm.Close()
                                          End Sub
            buttonPanel.Controls.Add(printButton)

            ' زر الإلغاء
            Dim cancelButton As New Button()
            cancelButton.Text = "إلغاء"
            cancelButton.Width = 120
            cancelButton.Height = 35
            cancelButton.Font = New Font("Arial", 12, FontStyle.Bold)
            cancelButton.Location = New Point(printButton.Left - 140, 12)
            AddHandler cancelButton.Click, Sub() previewForm.Close()
            buttonPanel.Controls.Add(cancelButton)

            previewForm.ShowDialog()
        Catch ex As Exception
            MessageBox.Show("خطأ في عرض المعاينة: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub PreviewPanel_Paint(sender As Object, e As PaintEventArgs)
        Try
            Dim panel As Panel = DirectCast(sender, Panel)
            Using g As Graphics = e.Graphics
                ' تعيين جودة الرسم
                g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
                g.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit

                ' رسم العنوان
                Using titleFont As New Font("Arial", 20, FontStyle.Bold)
                    Dim titleSize As SizeF = g.MeasureString("ROLL DATA", titleFont)
                    g.DrawString("ROLL DATA", titleFont, Brushes.Black, (panel.Width - titleSize.Width) \ 2, 20)
                End Using

                ' تحديد المناطق
                Dim startX As Integer = 50
                Dim startY As Integer = 80
                Dim cellWidth As Integer = (panel.Width - 100) \ 2
                Dim cellHeight As Integer = 40
                Dim labelX As Integer = startX
                Dim valueX As Integer = startX + cellWidth

                ' رسم الإطار الخارجي للجدول
                g.DrawRectangle(Pens.Black, startX, startY, cellWidth * 2, cellHeight * 4)

                ' رسم الخط العمودي الفاصل
                g.DrawLine(Pens.Black, startX + cellWidth, startY, startX + cellWidth, startY + (cellHeight * 4))

                ' رسم الخطوط الأفقية
                For i As Integer = 1 To 3
                    g.DrawLine(Pens.Black, startX, startY + (cellHeight * i), startX + (cellWidth * 2), startY + (cellHeight * i))
                Next

                ' إعداد الخطوط
                Using labelFont As New Font("Arial", 12, FontStyle.Bold)
                    Using valueFont As New Font("Arial", 12)
                        ' رسم العناوين والقيم
                        Dim currentY As Integer = startY

                        ' Batch No
                        g.DrawString("Batch No:", labelFont, Brushes.Black, labelX + 5, currentY + 10)
                        g.DrawString(cmbBatch.SelectedValue.ToString(), valueFont, Brushes.Black, valueX + 5, currentY + 10)
                        currentY += cellHeight

                        ' Lot No
                        g.DrawString("Lot No:", labelFont, Brushes.Black, labelX + 5, currentY + 10)
                        g.DrawString(cmbLot.SelectedValue.ToString(), valueFont, Brushes.Black, valueX + 5, currentY + 10)
                        currentY += cellHeight

                        ' Roll No
                        g.DrawString("Roll No:", labelFont, Brushes.Black, labelX + 5, currentY + 10)
                        g.DrawString(dgvRolls.CurrentRow.Cells("رقم التوب").Value.ToString(), valueFont, Brushes.Black, valueX + 5, currentY + 10)
                        currentY += cellHeight

                        ' Weight
                        g.DrawString("Weight:", labelFont, Brushes.Black, labelX + 5, currentY + 10)
                        g.DrawString(dgvRolls.CurrentRow.Cells("الوزن").Value.ToString() & " KG", valueFont, Brushes.Black, valueX + 5, currentY + 10)
                    End Using
                End Using

                ' رسم عنوان الباركود
                Using barcodeFont As New Font("Arial", 14, FontStyle.Bold)
                    Dim barcodeTitle As String = "Barcode"
                    Dim barcodeTitleSize As SizeF = g.MeasureString(barcodeTitle, barcodeFont)
                    g.DrawString(barcodeTitle, barcodeFont, Brushes.Black, (panel.Width - barcodeTitleSize.Width) \ 2, startY + (cellHeight * 4) + 20)
                End Using

                ' إنشاء الباركود باستخدام ZXing
                Dim barcodeData As String = cmbLot.SelectedValue.ToString() & "*" & dgvRolls.CurrentRow.Cells("رقم التوب").Value.ToString()
                Dim writer As New BarcodeWriter With {
                    .Format = BarcodeFormat.CODE_128,
                    .Options = New EncodingOptions With {
                        .Width = 300,
                        .Height = 80,
                        .Margin = 0,
                        .PureBarcode = True
                    }
                }

                ' إنشاء صورة الباركود
                Using barcodeBitmap As Bitmap = writer.Write(barcodeData)
                    ' رسم الباركود
                    Dim barcodeY As Integer = startY + (cellHeight * 4) + 60
                    g.DrawImage(barcodeBitmap, (panel.Width - 300) \ 2, barcodeY, 300, 80)

                    ' رسم نص الباركود تحت الصورة
                    Using textFont As New Font("Arial", 12)
                        Dim textSize As SizeF = g.MeasureString(barcodeData, textFont)
                        g.DrawString(barcodeData, textFont, Brushes.Black, (panel.Width - textSize.Width) \ 2, barcodeY + 90)
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("خطأ في رسم المعاينة: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub PrintZpl(zplCode As String)
        Try
            ' التحقق من وجود الطابعة
            Dim printerName As String = "ZD220-203dpi ZPL"
            Dim printerFound As Boolean = False
            For Each printer As String In PrinterSettings.InstalledPrinters
                If printer.Contains("ZD220") Then
                    printerName = printer
                    printerFound = True
                    Exit For
                End If
            Next

            If Not printerFound Then
                MessageBox.Show("لم يتم العثور على طابعة Zebra ZD220. الرجاء التأكد من تثبيت الطابعة وتشغيلها.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            ' إرسال مباشرة للطابعة
            If mainrawstoreform.RawPrinterHelper.SendStringToPrinter(printerName, zplCode) Then
                MessageBox.Show("تمت الطباعة بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("حدث خطأ أثناء الطباعة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As Exception
            MessageBox.Show("خطأ في الطباعة: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub PrintLabel(row As DataGridViewRow)
        Try
            ' تحضير البيانات للطباعة
            Dim lotValue As String = cmbLot.SelectedValue.ToString()
            Dim rollNumber As String = row.Cells("رقم التوب").Value.ToString()
            Dim weight As String = row.Cells("الوزن").Value.ToString()

            ' تحويل الباك سلاش إلى هيكس للـ ZPL
            Dim printableLotValue As String = ""
            For Each c As Char In lotValue
                If c = "\"c Then
                    printableLotValue &= "\\"
                Else
                    printableLotValue &= c
                End If
            Next

            ' إنشاء بيانات الباركود
            Dim barcodeData As String = printableLotValue & "*" & rollNumber

            ' كود ZPL للطباعة
            Dim zplCode As String = "^XA" & vbCrLf &
                                  "^CI28" & vbCrLf &
                                  "^PON" & vbCrLf &
                                  "^LH0,0" & vbCrLf &
                                  "^FO20,20^A0N,100,100^FB792,1,0,C^FDROLL DATA^FS" & vbCrLf &
                                  "^FO20,120^GB792,400,2^FS" & vbCrLf &
                                  "^FO20,220^GB792,0,2^FS" & vbCrLf &
                                  "^FO20,320^GB792,0,2^FS" & vbCrLf &
                                  "^FO406,120^GB0,400,2^FS" & vbCrLf &
                                  "^FO40,170^A0N,60,60^FB366,1,0,C^FDBatch No:^FS" & vbCrLf &
                                  "^FO40,270^A0N,60,60^FB366,1,0,C^FDLot No:^FS" & vbCrLf &
                                  "^FO40,370^A0N,60,60^FB366,1,0,C^FDRoll No:^FS" & vbCrLf &
                                  "^FO40,470^A0N,60,60^FB366,1,0,C^FDWeight:^FS" & vbCrLf &
                                  "^FO426,170^A0N,60,60^FB366,1,0,C^FD" & cmbBatch.SelectedValue.ToString() & "^FS" & vbCrLf &
                                  "^FO426,270^A0N,60,60^FB366,1,0,C^FD" & printableLotValue & "^FS" & vbCrLf &
                                  "^FO426,370^A0N,60,60^FB366,1,0,C^FD" & rollNumber & "^FS" & vbCrLf &
                                  "^FO426,470^A0N,60,60^FB366,1,0,C^FD" & weight & " KG^FS" & vbCrLf &
                                  "^FO20,600^GB792,0,2^FS" & vbCrLf &
                                  "^FO20,620^A0N,50,50^FB792,1,0,C^FDBarcode^FS" & vbCrLf &
                                  "^FO196,680^BY4" & vbCrLf &
                                  "^BCN,180,Y,N,N" & vbCrLf &
                                  "^FD" & barcodeData & "^FS" & vbCrLf &
                                  "^XZ"

            ' عرض المعاينة قبل الطباعة
            ShowPrintPreview(zplCode)

        Catch ex As Exception
            MessageBox.Show("خطأ في الطباعة: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class