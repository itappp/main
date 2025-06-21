Imports System.Windows.Forms
Imports System.Drawing
Imports System.Data
Imports System.Data.SqlClient
Imports System.Threading.Tasks
Imports Excel = Microsoft.Office.Interop.Excel

Public Class AIAnalysisForm
    Inherits Form

    Private connectionString As String = "Server=localhost;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"
    Private WithEvents dtpFromDate As DateTimePicker
    Private WithEvents dtpToDate As DateTimePicker
    Private WithEvents txtWorderID As TextBox
    Private WithEvents cmbReportType As ComboBox
    Private WithEvents btnGenerateReport As Button
    Private WithEvents btnExportExcel As Button
    Private WithEvents lblStatus As Label
    Private WithEvents dgvReport As DataGridView

    Public Sub New()
        ' Initialize all controls first
        dtpFromDate = New DateTimePicker()
        dtpToDate = New DateTimePicker()
        txtWorderID = New TextBox()
        cmbReportType = New ComboBox()
        btnGenerateReport = New Button()
        btnExportExcel = New Button()
        lblStatus = New Label()
        dgvReport = New DataGridView()

        ' Then call InitializeComponent
        InitializeForm()
    End Sub

    Private Sub InitializeForm()
        ' Form Settings
        Me.Text = "تتبع أوامر الشغل"
        Me.WindowState = FormWindowState.Maximized
        Me.MinimumSize = New Size(800, 600)
        Me.RightToLeft = RightToLeft.Yes
        Me.RightToLeftLayout = True

        ' Main container
        Dim mainContainer As New TableLayoutPanel()
        mainContainer.Dock = DockStyle.Fill
        mainContainer.RowCount = 2
        mainContainer.ColumnCount = 1
        mainContainer.RowStyles.Add(New RowStyle(SizeType.Absolute, 100)) ' Top panel height
        mainContainer.RowStyles.Add(New RowStyle(SizeType.Percent, 100)) ' Grid takes remaining space

        ' Controls
        Dim pnlTop As New TableLayoutPanel()
        pnlTop.Dock = DockStyle.Fill
        pnlTop.Height = 100
        pnlTop.Padding = New Padding(10)
        pnlTop.ColumnCount = 10
        pnlTop.RowCount = 2

        ' Set column percentages
        pnlTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 10)) ' Label
        pnlTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 15)) ' TextBox/ComboBox
        pnlTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 8)) ' Label
        pnlTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 12)) ' DatePicker
        pnlTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 8)) ' Label
        pnlTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 12)) ' DatePicker
        pnlTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 10)) ' Label
        pnlTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 15)) ' ComboBox
        pnlTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 5)) ' Button
        pnlTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 5)) ' Button

        ' Set row heights
        pnlTop.RowStyles.Add(New RowStyle(SizeType.Percent, 50))
        pnlTop.RowStyles.Add(New RowStyle(SizeType.Percent, 50))

        ' Work Order ID
        Dim lblWorderID As New Label()
        lblWorderID.Text = "رقم أمر الشغل:"
        lblWorderID.AutoSize = True
        lblWorderID.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        lblWorderID.TextAlign = ContentAlignment.MiddleCenter

        txtWorderID.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        txtWorderID.Margin = New Padding(3, 3, 10, 3)

        ' Date Range
        Dim lblFromDate As New Label()
        lblFromDate.Text = "من تاريخ:"
        lblFromDate.AutoSize = True
        lblFromDate.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        lblFromDate.TextAlign = ContentAlignment.MiddleCenter

        dtpFromDate.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        dtpFromDate.Format = DateTimePickerFormat.Short
        dtpFromDate.Value = DateTime.Now.AddDays(-7)
        dtpFromDate.Margin = New Padding(3, 3, 10, 3)

        Dim lblToDate As New Label()
        lblToDate.Text = "إلى تاريخ:"
        lblToDate.AutoSize = True
        lblToDate.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        lblToDate.TextAlign = ContentAlignment.MiddleCenter

        dtpToDate.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        dtpToDate.Format = DateTimePickerFormat.Short
        dtpToDate.Value = DateTime.Now
        dtpToDate.Margin = New Padding(3, 3, 10, 3)

        ' Report Type ComboBox
        Dim lblReportType As New Label()
        lblReportType.Text = "نوع التقرير:"
        lblReportType.AutoSize = True
        lblReportType.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        lblReportType.TextAlign = ContentAlignment.MiddleCenter

        cmbReportType.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        cmbReportType.DropDownStyle = ComboBoxStyle.DropDownList
        cmbReportType.Items.AddRange(New String() {
            "تتبع أوامر الشغل",
            "إجماليات الماكينات",
            "إنتاجية المجهز",
            "استلام مخزن المجهز"
        })
        RemoveHandler cmbReportType.SelectedIndexChanged, AddressOf cmbReportType_SelectedIndexChanged
        cmbReportType.SelectedIndex = 0
        AddHandler cmbReportType.SelectedIndexChanged, AddressOf cmbReportType_SelectedIndexChanged
        cmbReportType.Margin = New Padding(3, 3, 10, 3)

        ' Generate Button
        btnGenerateReport.Text = "عرض التقرير"
        btnGenerateReport.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        btnGenerateReport.Height = 30
        btnGenerateReport.Margin = New Padding(3, 3, 10, 3)

        ' Export Button
        btnExportExcel.Text = "تصدير إلى Excel"
        btnExportExcel.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        btnExportExcel.Height = 30
        btnExportExcel.Margin = New Padding(3, 3, 10, 3)

        ' Status Label
        lblStatus.AutoSize = True
        lblStatus.Dock = DockStyle.Fill
        lblStatus.TextAlign = ContentAlignment.MiddleRight

        ' Add controls to TableLayoutPanel
        pnlTop.Controls.Add(lblWorderID, 0, 0)
        pnlTop.Controls.Add(txtWorderID, 1, 0)
        pnlTop.Controls.Add(lblFromDate, 2, 0)
        pnlTop.Controls.Add(dtpFromDate, 3, 0)
        pnlTop.Controls.Add(lblToDate, 4, 0)
        pnlTop.Controls.Add(dtpToDate, 5, 0)
        pnlTop.Controls.Add(lblReportType, 6, 0)
        pnlTop.Controls.Add(cmbReportType, 7, 0)
        pnlTop.Controls.Add(btnGenerateReport, 8, 0)
        pnlTop.Controls.Add(btnExportExcel, 9, 0)
        pnlTop.Controls.Add(lblStatus, 0, 1)
        pnlTop.SetColumnSpan(lblStatus, 10)

        ' DataGridView
        dgvReport.Dock = DockStyle.Fill
        ConfigureDataGridView(dgvReport)

        ' Add panels to main container
        mainContainer.Controls.Add(pnlTop, 0, 0)
        mainContainer.Controls.Add(dgvReport, 0, 1)

        ' Add main container to form
        Me.Controls.Add(mainContainer)

        ' Add event handlers
        AddHandler btnGenerateReport.Click, AddressOf btnGenerateReport_Click
        AddHandler btnExportExcel.Click, AddressOf btnExportExcel_Click
        AddHandler dgvReport.CellPainting, AddressOf dgvReport_CellPainting
    End Sub

    Private Sub ConfigureDataGridView(dgv As DataGridView)
        With dgv
            .RightToLeft = RightToLeft.Yes
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .ReadOnly = True
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
            .RowHeadersVisible = False
            .DefaultCellStyle.Font = New Font("Arial", 10)
            .ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 10, FontStyle.Bold)
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersVisible = True
            .ColumnHeadersHeight = 40
            .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            .EnableHeadersVisualStyles = False
            .ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.Black
            .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single
            .BorderStyle = BorderStyle.Fixed3D
            .BackgroundColor = Color.White
        End With
    End Sub

    Private Sub btnGenerateReport_Click(sender As Object, e As EventArgs)
        Try
            lblStatus.Text = "جاري تحميل البيانات..."
            lblStatus.ForeColor = Color.Blue
            Application.DoEvents()

            Select Case cmbReportType.SelectedItem.ToString()
                Case "تتبع أوامر الشغل"
                    LoadWorkOrderDataAsync()
                Case "إجماليات الماكينات"
                    LoadMachineTotalsAsync()
                Case "إنتاجية المجهز"
                    LoadFinishingInspectorReport()
                Case "استلام مخزن المجهز"
                    LoadStoreFinishReport()
            End Select

            lblStatus.Text = "تم تحميل البيانات بنجاح"
            lblStatus.ForeColor = Color.Green
        Catch ex As Exception
            lblStatus.Text = "حدث خطأ أثناء تحميل البيانات: " & ex.Message
            lblStatus.ForeColor = Color.Red
            MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LoadWorkOrderDataAsync()
        Dim query As String = "
            WITH AllProcesses AS (
                SELECT 
                    WorderID,
                    name_arab,
                    name_ar,
                    speed,
                    StartTime,
                    EndTime,
                    quantity,
                    quantitym,
                    quantitykg,
                    total_time,
                    duration,
                    totalstoptime,
                    username,
                    Notes,
                    LAG(name_ar, 1) OVER (PARTITION BY WorderID ORDER BY StartTime) AS PrevProcess,
                    LAG(EndTime, 1) OVER (PARTITION BY WorderID ORDER BY StartTime) AS PrevEndTime
                FROM WorkOrder WITH (NOLOCK)
                LEFT JOIN machine WITH (NOLOCK) ON workorder.machineid = machine.id
                LEFT JOIN machine_steps WITH (NOLOCK) ON workorder.processid = machine_steps.id
                WHERE WorkOrder.WorderID NOT IN ('0')
            )
            SELECT 
                WorderID + ' - ' + name_arab + ' - ' + name_ar AS 'Merge',
                WorderID AS 'رقم أمر الشغل',
                name_arab AS 'الماكينة',
                name_ar AS 'العملية',
                speed AS 'السرعة',
                StartTime AS 'وقت البداية',
                EndTime AS 'وقت النهاية',
                quantity AS 'الكمية',
                quantitym AS 'الطول',
                quantitykg AS 'الوزن',
                total_time AS 'الوقت الكلي',
                duration AS 'الوقت الصافي',
                totalstoptime AS 'وقت التوقف',
                username AS 'العامل',
                Notes AS 'ملاحظات',
                PrevProcess AS 'العملية السابقة',
                PrevEndTime AS 'وقت نهاية العملية السابقة',
                DATEDIFF(day, PrevEndTime, StartTime) AS 'الفرق بالأيام',
                CAST(DATEDIFF(second, PrevEndTime, StartTime) / 3600.0 AS DECIMAL(10,2)) AS 'الفرق بالساعات'
            FROM AllProcesses
            WHERE StartTime >= @FromDate 
            AND StartTime < DATEADD(day, 1, @ToDate)
            " & If(String.IsNullOrEmpty(txtWorderID.Text.Trim()), "", "AND WorderID = @WorderID") & "
            ORDER BY WorderID, StartTime"

        Using conn As New SqlConnection(connectionString)
            conn.Open()

            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@FromDate", dtpFromDate.Value.Date)
                cmd.Parameters.AddWithValue("@ToDate", dtpToDate.Value.Date)
                If Not String.IsNullOrEmpty(txtWorderID.Text.Trim()) Then
                    cmd.Parameters.AddWithValue("@WorderID", txtWorderID.Text.Trim())
                End If
                cmd.CommandTimeout = 180

                Using adapter As New SqlDataAdapter(cmd)
                    Dim dt As New DataTable()
                    adapter.Fill(dt)
                    dgvReport.DataSource = dt


                    ' Format datetime columns
                    For Each col As DataGridViewColumn In dgvReport.Columns
                        If col.ValueType IsNot Nothing AndAlso col.ValueType.Equals(GetType(DateTime)) Then
                            col.DefaultCellStyle.Format = "yyyy/MM/dd HH:mm:ss"
                        End If
                    Next
                End Using
            End Using
        End Using
    End Sub

    Private Sub LoadMachineTotalsAsync()
        Dim query As String = "
            WITH MachineStats AS (
                SELECT 
                    CONVERT(date, w.StartTime) AS work_date,
                    m.name_arab,
                    COUNT(DISTINCT w.WorderID) AS work_orders_count,
                    CAST(SUM(CAST(ISNULL(w.quantitym, 0) AS decimal(18,2))) AS decimal(18,2)) AS total_length,
                    CAST(SUM(CAST(ISNULL(w.quantitykg, 0) AS decimal(18,2))) AS decimal(18,2)) AS total_weight,
                    CAST(SUM(
                        DATEDIFF(SECOND, '00:00:00',
                            CASE 
                                WHEN ISDATE(w.duration) = 1 THEN w.duration 
                                ELSE '00:00:00' 
                            END
                        )
                    ) AS decimal(18,2)) AS total_duration_seconds,
                    CAST(SUM(
                        DATEDIFF(SECOND, '00:00:00',
                            CASE 
                                WHEN ISDATE(w.totalstoptime) = 1 THEN w.totalstoptime 
                                ELSE '00:00:00' 
                            END
                        )
                    ) AS decimal(18,2)) AS total_stoptime_seconds,
                    24 * 3600 AS total_seconds_per_day
                FROM WorkOrder w WITH (NOLOCK)
                LEFT JOIN machine m WITH (NOLOCK) ON w.machineid = m.id
                WHERE w.WorderID NOT IN ('0')
                GROUP BY CONVERT(date, w.StartTime), m.name_arab
            )
            SELECT 
                FORMAT(work_date, 'yyyy/MM/dd') AS 'التاريخ',
                name_arab AS 'الماكينة',
                work_orders_count AS 'عدد أوامر الشغل',
                total_length AS 'إجمالي الطول',
                total_weight AS 'إجمالي الوزن',
                CAST(FLOOR(total_duration_seconds / 3600) AS VARCHAR) + ':' +
                RIGHT('0' + CAST(FLOOR((total_duration_seconds % 3600) / 60) AS VARCHAR), 2) + ':' +
                RIGHT('0' + CAST(total_duration_seconds % 60 AS VARCHAR), 2) AS 'إجمالي الوقت الصافي',
                CAST(FLOOR(total_stoptime_seconds / 3600) AS VARCHAR) + ':' +
                RIGHT('0' + CAST(FLOOR((total_stoptime_seconds % 3600) / 60) AS VARCHAR), 2) + ':' +
                RIGHT('0' + CAST(total_stoptime_seconds % 60 AS VARCHAR), 2) AS 'إجمالي وقت التوقف',
                CAST(
                    CASE 
                        WHEN (total_duration_seconds + total_stoptime_seconds) = 0 THEN 0
                        ELSE (total_stoptime_seconds * 100.0) / (total_duration_seconds + total_stoptime_seconds)
                    END 
                AS decimal(18,2)) AS 'نسبة التوقف %',
                CAST(FLOOR((total_seconds_per_day - (total_duration_seconds + total_stoptime_seconds)) / 3600) AS VARCHAR) + ':' +
                RIGHT('0' + CAST(FLOOR(((total_seconds_per_day - (total_duration_seconds + total_stoptime_seconds)) % 3600) / 60) AS VARCHAR), 2) + ':' +
                RIGHT('0' + CAST((total_seconds_per_day - (total_duration_seconds + total_stoptime_seconds)) % 60 AS VARCHAR), 2) AS 'إجمالي التوقف بدون تشغيل',
                CAST(
                    CASE 
                        WHEN total_seconds_per_day = 0 THEN 0
                        ELSE ((total_seconds_per_day - (total_duration_seconds + total_stoptime_seconds)) * 100.0) / total_seconds_per_day
                    END 
                AS decimal(18,2)) AS 'نسبة التوقف بدون تشغيل %',
                CAST(
                    CASE 
                        WHEN total_seconds_per_day = 0 THEN 0
                        ELSE (total_duration_seconds * 100.0) / total_seconds_per_day
                    END 
                AS decimal(18,2)) AS 'نسبة استغلال الماكينة %',
                CASE 
                    WHEN (total_duration_seconds * 100.0) / total_seconds_per_day >= 75 THEN N'ممتاز'
                    WHEN (total_duration_seconds * 100.0) / total_seconds_per_day >= 50 THEN N'جيد'
                    WHEN (total_duration_seconds * 100.0) / total_seconds_per_day >= 25 THEN N'متوسط'
                    ELSE N'ضعيف'
                END AS 'تقييم الأداء'
            FROM MachineStats
            WHERE work_date >= @FromDate 
            AND work_date < DATEADD(day, 1, @ToDate)
            ORDER BY work_date DESC, total_length DESC;"

        Using conn As New SqlConnection(connectionString)
            conn.Open()

            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@FromDate", dtpFromDate.Value.Date)
                cmd.Parameters.AddWithValue("@ToDate", dtpToDate.Value.Date)
                cmd.CommandTimeout = 180

                Using adapter As New SqlDataAdapter(cmd)
                    Dim dt As New DataTable()
                    adapter.Fill(dt)
                    dgvReport.DataSource = dt
                End Using
            End Using
        End Using
    End Sub

    Private Sub LoadFinishingInspectorReport()
        Dim query As String = "
            WITH InspectorStats AS (
                SELECT 
                    CASE
                        WHEN DATEPART(HOUR, fi.date) >= 23 THEN CONVERT(date, fi.date)  
                        WHEN DATEPART(HOUR, fi.date) < 8 THEN CONVERT(date, DATEADD(day, -1, fi.date))  
                        ELSE CONVERT(date, fi.date)  
                    END AS shift_date,
                    fi.fabric_grade,
                    COUNT(fi.roll) AS rolls_count,
                    CAST(SUM(ISNULL(fi.height, 0)) AS decimal(18,2)) AS total_length,
                    CAST(SUM(ISNULL(fi.weight, 0)) AS decimal(18,2)) AS total_weight
                FROM finish_inspect fi WITH (NOLOCK)
                WHERE CASE 
                        WHEN DATEPART(HOUR, fi.date) >= 23 THEN CONVERT(date, fi.date)  
                        WHEN DATEPART(HOUR, fi.date) < 8 THEN CONVERT(date, DATEADD(day, -1, fi.date))  
                        ELSE CONVERT(date, fi.date)  
                    END BETWEEN @FromDate AND @ToDate
                GROUP BY 
                    CASE
                        WHEN DATEPART(HOUR, fi.date) >= 23 THEN CONVERT(date, fi.date)  
                        WHEN DATEPART(HOUR, fi.date) < 8 THEN CONVERT(date, DATEADD(day, -1, fi.date))  
                        ELSE CONVERT(date, fi.date)  
                    END,
                    fi.fabric_grade
            )
            SELECT 
                FORMAT(@FromDate, 'yyyy/MM/dd') + ' - ' + FORMAT(@ToDate, 'yyyy/MM/dd') AS 'تاريخ الوردية',
                SUM(rolls_count) AS 'إجمالي عدد الأتواب',
                CAST(SUM(total_length) AS decimal(18,2)) AS 'إجمالي الطول',
                CAST(SUM(total_weight) AS decimal(18,2)) AS 'إجمالي الوزن',
                CAST(SUM(CASE WHEN fabric_grade = 1 THEN total_length ELSE 0 END) AS decimal(18,2)) AS 'إجمالي طول الدرجة الأولى',
                CAST(SUM(CASE WHEN fabric_grade = 1 THEN total_weight ELSE 0 END) AS decimal(18,2)) AS 'إجمالي وزن الدرجة الأولى',
                CAST(SUM(CASE WHEN fabric_grade = 2 OR fabric_grade IS NULL OR fabric_grade = 0 THEN total_length ELSE 0 END) AS decimal(18,2)) AS 'إجمالي طول الدرجة الثانية',
                CAST(SUM(CASE WHEN fabric_grade = 2 OR fabric_grade IS NULL OR fabric_grade = 0 THEN total_weight ELSE 0 END) AS decimal(18,2)) AS 'إجمالي وزن الدرجة الثانية',
                CAST(
                    CASE 
                        WHEN SUM(total_length) = 0 THEN 0 
                        ELSE (SUM(CASE WHEN fabric_grade = 2 OR fabric_grade IS NULL OR fabric_grade = 0 THEN total_length ELSE 0 END) * 100.0) / SUM(total_length)
                    END 
                AS decimal(18,2)) AS 'نسبة الدرجة الثانية متر %',
                CAST(
                    CASE 
                        WHEN SUM(total_weight) = 0 THEN 0 
                        ELSE (SUM(CASE WHEN fabric_grade = 2 OR fabric_grade IS NULL OR fabric_grade = 0 THEN total_weight ELSE 0 END) * 100.0) / SUM(total_weight)
                    END 
                AS decimal(18,2)) AS 'نسبة الدرجة الثانية وزن %'
            FROM InspectorStats;"

        Using conn As New SqlConnection(connectionString)
            conn.Open()

            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@FromDate", dtpFromDate.Value.Date)
                cmd.Parameters.AddWithValue("@ToDate", dtpToDate.Value.Date)
                cmd.CommandTimeout = 180

                Using adapter As New SqlDataAdapter(cmd)
                    Dim dt As New DataTable()
                    adapter.Fill(dt)
                    dgvReport.DataSource = dt
                End Using
            End Using
        End Using
    End Sub

    Private Sub LoadStoreFinishReport()
        Dim query As String = "
            WITH StoreStats AS (
                SELECT 
                    sf.fabric_grade,
                    COUNT(sf.roll) AS rolls_count,
                    CAST(SUM(ISNULL(sf.heightPK, 0)) AS decimal(18,2)) AS total_length,
                    CAST(SUM(ISNULL(sf.weightPK, 0)) AS decimal(18,2)) AS total_weight
                FROM store_finish sf WITH (NOLOCK)
                WHERE sf.transaction_date >= @FromDate 
                AND sf.transaction_date < DATEADD(day, 1, @ToDate)
                GROUP BY sf.fabric_grade
            )
            SELECT 
                FORMAT(@FromDate, 'yyyy/MM/dd') + ' - ' + FORMAT(@ToDate, 'yyyy/MM/dd') AS 'الفترة',
                SUM(rolls_count) AS 'إجمالي عدد الأتواب',
                CAST(SUM(total_length) AS decimal(18,2)) AS 'إجمالي الطول',
                CAST(SUM(total_weight) AS decimal(18,2)) AS 'إجمالي الوزن',
                CAST(SUM(CASE WHEN fabric_grade = 1 THEN total_length ELSE 0 END) AS decimal(18,2)) AS 'إجمالي طول الدرجة الأولى',
                CAST(SUM(CASE WHEN fabric_grade = 1 THEN total_weight ELSE 0 END) AS decimal(18,2)) AS 'إجمالي وزن الدرجة الأولى',
                CAST(SUM(CASE WHEN fabric_grade = 2 OR fabric_grade IS NULL OR fabric_grade = 0 THEN total_length ELSE 0 END) AS decimal(18,2)) AS 'إجمالي طول الدرجة الثانية',
                CAST(SUM(CASE WHEN fabric_grade = 2 OR fabric_grade IS NULL OR fabric_grade = 0 THEN total_weight ELSE 0 END) AS decimal(18,2)) AS 'إجمالي وزن الدرجة الثانية',
                CAST(SUM(CASE WHEN fabric_grade = 5 THEN total_weight ELSE 0 END) AS decimal(18,2)) AS 'إجمالي وزن الهالك',
                CAST(
                    CASE 
                        WHEN SUM(total_length) = 0 THEN 0 
                        ELSE (SUM(CASE WHEN fabric_grade = 2 OR fabric_grade IS NULL OR fabric_grade = 0 THEN total_length ELSE 0 END) * 100.0) / SUM(total_length)
                    END 
                AS decimal(18,2)) AS 'نسبة الدرجة الثانية متر %',
                CAST(
                    CASE 
                        WHEN SUM(total_weight) = 0 THEN 0 
                        ELSE (SUM(CASE WHEN fabric_grade = 2 OR fabric_grade IS NULL OR fabric_grade = 0 THEN total_weight ELSE 0 END) * 100.0) / SUM(total_weight)
                    END 
                AS decimal(18,2)) AS 'نسبة الدرجة الثانية وزن %'
            FROM StoreStats;"

        Using conn As New SqlConnection(connectionString)
            conn.Open()

            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@FromDate", dtpFromDate.Value.Date)
                cmd.Parameters.AddWithValue("@ToDate", dtpToDate.Value.Date)
                cmd.CommandTimeout = 180

                Using adapter As New SqlDataAdapter(cmd)
                    Dim dt As New DataTable()
                    adapter.Fill(dt)
                    dgvReport.DataSource = dt
                End Using
            End Using
        End Using
    End Sub

    Private Sub MergeDateCells(startRow As Integer, endRow As Integer)
        If startRow < endRow Then
            ' Hide the date value in all rows except the first one
            For i As Integer = startRow + 1 To endRow
                dgvReport.Rows(i).Cells("التاريخ").Value = ""
            Next

            ' Calculate the merged cell height
            Dim totalHeight As Integer = 0
            For i As Integer = startRow To endRow
                totalHeight += dgvReport.Rows(i).Height
            Next

            ' Create merged cell style
            Dim mergedStyle As New DataGridViewCellStyle()
            mergedStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            mergedStyle.Font = New Font("Arial", 10)
            mergedStyle.BackColor = Color.White
            mergedStyle.ForeColor = Color.Black

            ' Apply style to the first cell
            With dgvReport.Rows(startRow).Cells("التاريخ")
                .Style = mergedStyle
                .Tag = New Rectangle(
                    dgvReport.GetCellDisplayRectangle(0, startRow, True).X,
                    dgvReport.GetCellDisplayRectangle(0, startRow, True).Y,
                    dgvReport.GetCellDisplayRectangle(0, startRow, True).Width,
                    totalHeight
                )
            End With
        End If
    End Sub

    Private Sub dgvReport_CellPainting(sender As Object, e As DataGridViewCellPaintingEventArgs) Handles dgvReport.CellPainting
        If e.ColumnIndex = 0 AndAlso e.RowIndex >= 0 Then ' Date column
            Dim cell As DataGridViewCell = dgvReport.Rows(e.RowIndex).Cells(e.ColumnIndex)
            If cell.Tag IsNot Nothing Then
                e.Handled = True
            ElseIf String.IsNullOrEmpty(cell.Value?.ToString()) Then
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub btnExportExcel_Click(sender As Object, e As EventArgs)
        Dim excelApp As Excel.Application = Nothing
        Dim workbook As Excel.Workbook = Nothing
        Dim worksheet As Excel.Worksheet = Nothing

        Try
            If dgvReport.Rows.Count = 0 Then
                MessageBox.Show("لا توجد بيانات للتصدير", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            lblStatus.Text = "جاري التصدير إلى Excel..."
            lblStatus.ForeColor = Color.Blue
            Application.DoEvents()

            ' Create Excel application
            excelApp = New Excel.Application With {
                .Visible = False,
                .DisplayAlerts = False
            }
            workbook = excelApp.Workbooks.Add()
            worksheet = DirectCast(workbook.ActiveSheet, Excel.Worksheet)

            ' Add report title and date range
            Dim reportTitle As String = cmbReportType.SelectedItem.ToString()
            Dim dateRange As String = $"الفترة من {dtpFromDate.Value:yyyy/MM/dd} إلى {dtpToDate.Value:yyyy/MM/dd}"

            worksheet.Cells(1, 1) = reportTitle
            worksheet.Cells(2, 1) = dateRange

            ' Format title
            Dim titleRange As Excel.Range = worksheet.Range(worksheet.Cells(1, 1), worksheet.Cells(1, dgvReport.Columns.Count))
            titleRange.Merge()
            titleRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
            titleRange.Font.Name = "Arial"
            titleRange.Font.Size = 14
            titleRange.Font.Bold = True

            ' Format date range
            Dim dateRangeRow As Excel.Range = worksheet.Range(worksheet.Cells(2, 1), worksheet.Cells(2, dgvReport.Columns.Count))
            dateRangeRow.Merge()
            dateRangeRow.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
            dateRangeRow.Font.Name = "Arial"
            dateRangeRow.Font.Size = 12
            dateRangeRow.Font.Bold = True

            ' Prepare data array
            Dim rowCount As Integer = dgvReport.Rows.Count
            Dim colCount As Integer = dgvReport.Columns.Count
            Dim dataArray(rowCount, colCount - 1) As Object

            ' Fill headers
            For j As Integer = 0 To colCount - 1
                dataArray(0, j) = dgvReport.Columns(j).HeaderText
            Next

            ' Fill data
            For i As Integer = 0 To rowCount - 1
                For j As Integer = 0 To colCount - 1
                    dataArray(i + 1, j) = If(dgvReport.Rows(i).Cells(j).Value IsNot Nothing,
                                           dgvReport.Rows(i).Cells(j).Value.ToString(),
                                           "")
                Next
            Next

            ' Write all data at once (starting from row 4 to account for title and date range)
            Dim dataRange As Excel.Range = worksheet.Range(
                worksheet.Cells(4, 1),
                worksheet.Cells(rowCount + 4, colCount)
            )
            dataRange.Value2 = dataArray

            ' Format as table
            Dim tableRange As Excel.Range = worksheet.Range(
                worksheet.Cells(4, 1),
                worksheet.Cells(rowCount + 4, colCount)
            )
            Dim tableObj As Excel.ListObject = worksheet.ListObjects.Add(
                Excel.XlListObjectSourceType.xlSrcRange,
                tableRange,
                ,
                Excel.XlYesNoGuess.xlYes
            )
            tableObj.Name = "DataTable"

            ' Apply formatting
            dataRange.Font.Name = "Arial"
            dataRange.Font.Size = 10
            dataRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
            dataRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
            dataRange.WrapText = False
            dataRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous
            dataRange.Borders.Weight = Excel.XlBorderWeight.xlThin

            ' Format headers
            Dim headerRange As Excel.Range = worksheet.Range(worksheet.Cells(4, 1), worksheet.Cells(4, colCount))
            headerRange.Interior.Color = RGB(173, 216, 230)
            headerRange.Font.Bold = True

            ' Optimize column width
            dataRange.Columns.AutoFit()

            ' Set print settings
            worksheet.PageSetup.PrintArea = worksheet.Range(worksheet.Cells(1, 1), worksheet.Cells(rowCount + 4, colCount)).Address
            worksheet.PageSetup.Orientation = Excel.XlPageOrientation.xlLandscape
            worksheet.PageSetup.FitToPagesWide = 1
            worksheet.PageSetup.FitToPagesTall = False
            worksheet.PageSetup.RightHeader = "تاريخ الطباعة: &D"
            worksheet.PageSetup.RightFooter = "صفحة &P من &N"

            ' Save file
            Dim saveDialog As New SaveFileDialog()
            saveDialog.Filter = "Excel Files (*.xlsx)|*.xlsx"
            saveDialog.FileName = $"{reportTitle}_{dtpFromDate.Value:yyyy-MM-dd}_الى_{dtpToDate.Value:yyyy-MM-dd}"

            If saveDialog.ShowDialog() = DialogResult.OK Then
                workbook.SaveAs(saveDialog.FileName)
                MessageBox.Show("تم تصدير البيانات بنجاح", "تم", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            lblStatus.Text = "تم تصدير البيانات بنجاح"
            lblStatus.ForeColor = Color.Green

        Catch ex As Exception
            lblStatus.Text = "حدث خطأ أثناء التصدير: " & ex.Message
            lblStatus.ForeColor = Color.Red
            MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            ' Cleanup in reverse order
            If worksheet IsNot Nothing Then
                ReleaseObject(worksheet)
            End If

            If workbook IsNot Nothing Then
                workbook.Close(False)
                ReleaseObject(workbook)
            End If

            If excelApp IsNot Nothing Then
                excelApp.Quit()
                ReleaseObject(excelApp)
            End If

            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try
    End Sub

    Private Sub ReleaseObject(ByVal obj As Object)
        Try
            If obj IsNot Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            End If
        Catch
        Finally
            obj = Nothing
        End Try
    End Sub

    Private Sub cmbReportType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbReportType.SelectedIndexChanged
        Select Case cmbReportType.SelectedItem.ToString()
            Case "تتبع أوامر الشغل"
                LoadWorkOrderDataAsync()
            Case "إجماليات الماكينات"
                LoadMachineTotalsAsync()
            Case "إنتاجية المجهز"
                LoadFinishingInspectorReport()
            Case "استلام مخزن المجهز"
                LoadStoreFinishReport()
        End Select
    End Sub
End Class
