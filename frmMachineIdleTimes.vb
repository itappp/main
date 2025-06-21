Imports System.Data.SqlClient
Imports System.Windows.Forms

Public Class frmMachineIdleTimes
    Inherits Form

    Private sqlServerConnectionString As String = "Server=localhost;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"

    Private dtpDate As New DateTimePicker()
    Private dgvIdleTimes As New DataGridView()
    Private cmbMachine As New ComboBox()
    Private btnSearch As New Button()
    Private flowTimelines As New FlowLayoutPanel()

    ' تعريف هيكل للفترات
    Private Structure WorkPeriod
        Public StartTime As DateTime
        Public EndTime As DateTime
        Public WOrderID As String
        Public IsIdle As Boolean ' True = توقف، False = شغال
        Public IsLive As Boolean ' جديد
        Public IsScheduledStop As Boolean ' توقف مجدول
        Public StopReason As String ' سبب التوقف المجدول
    End Structure
    Private timelinePeriods As New List(Of WorkPeriod)()
    Private timelineTooltip As New ToolTip()
    ' Timer لتحديث الوميض
    Private WithEvents blinkTimer As New Timer() With {.Interval = 1000}

    ' تعريف المتغيرات المطلوبة لرسم AM و PM
    Private amStartX As Integer = 0
    Private amEndX As Integer = 0
    Private pmStartX As Integer = 0
    Private pmEndX As Integer = 0
    Private amCenterX As Integer = 0
    Private pmCenterX As Integer = 0
    Private ampmFont As New Font("Tahoma", 12, FontStyle.Bold)
    Private ampmBrush As New SolidBrush(Color.DarkBlue)

    Private livePictureBoxes As New List(Of PictureBox)()
    Private lastTooltipPeriod As WorkPeriod? = Nothing

    Public Sub New()
        ' إعداد خصائص الفورم الأساسية
        Me.Text = "فترات توقف الماكينات"
        Me.WindowState = FormWindowState.Maximized
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.FormBorderStyle = FormBorderStyle.Sizable
        Me.MaximizeBox = True
        Me.MinimizeBox = True

        ' إعداد العناصر
        cmbMachine.Location = New Point(30, 30)
        cmbMachine.Size = New Size(120, 30)
        cmbMachine.DropDownStyle = ComboBoxStyle.DropDownList
        cmbMachine.Anchor = AnchorStyles.Top Or AnchorStyles.Left
        Me.Controls.Add(cmbMachine)

        dtpDate.Location = New Point(170, 30)
        dtpDate.Size = New Size(150, 30)
        dtpDate.Format = DateTimePickerFormat.Short
        dtpDate.Anchor = AnchorStyles.Top Or AnchorStyles.Left
        Me.Controls.Add(dtpDate)
        AddHandler dtpDate.ValueChanged, AddressOf dtpDate_ValueChanged

        btnSearch.Location = New Point(340, 30)
        btnSearch.Size = New Size(100, 30)
        btnSearch.Text = "بحث"
        btnSearch.Anchor = AnchorStyles.Top Or AnchorStyles.Left
        Me.Controls.Add(btnSearch)
        AddHandler btnSearch.Click, AddressOf btnSearch_Click

        flowTimelines.Location = New Point(30, 70)
        flowTimelines.Size = New Size(Me.ClientSize.Width - 60, Me.ClientSize.Height - 90)
        flowTimelines.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        flowTimelines.AutoScroll = True
        flowTimelines.FlowDirection = FlowDirection.TopDown
        flowTimelines.WrapContents = False
        Me.Controls.Add(flowTimelines)

        dgvIdleTimes.Location = New Point((Me.ClientSize.Width - dgvIdleTimes.Width) \ 2, Me.ClientSize.Height - dgvIdleTimes.Height - 10)
        dgvIdleTimes.Size = New Size(400, 100)
        dgvIdleTimes.Anchor = AnchorStyles.Top Or AnchorStyles.Left
        dgvIdleTimes.Visible = False
        Me.Controls.Add(dgvIdleTimes)

        ' تشغيل التايمر للوميض
        blinkTimer.Start()
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        LoadMachines()
        dtpDate.Value = DateTime.Now.Date
        AdjustLayout()
        ShowAllMachinesTimelines()
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        AdjustLayout()
    End Sub

    Private Sub AdjustLayout()
        flowTimelines.Location = New Point(30, 70)
        flowTimelines.Size = New Size(Me.ClientSize.Width - 60, Me.ClientSize.Height - 90)
        dgvIdleTimes.Size = New Size(400, 100)
        dgvIdleTimes.Location = New Point((Me.ClientSize.Width - dgvIdleTimes.Width) \ 2, Me.ClientSize.Height - dgvIdleTimes.Height - 10)
        flowTimelines.Refresh()
    End Sub

    Private Sub LoadMachines()
        cmbMachine.DataSource = Nothing
        cmbMachine.Items.Clear()
        Dim dt As New DataTable()
        Using con As New SqlConnection(sqlServerConnectionString)
            con.Open()
            Dim cmd As New SqlCommand("SELECT DISTINCT WorkOrder.MachineID, Machine.name_arab FROM WorkOrder LEFT JOIN Machine ON WorkOrder.MachineID = Machine.id ORDER BY Machine.name_arab", con)
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(dt)
        End Using
        If dt.Rows.Count > 0 Then
            cmbMachine.DataSource = dt
            cmbMachine.DisplayMember = "name_arab"
            cmbMachine.ValueMember = "MachineID"
            cmbMachine.SelectedIndex = 0
        End If
    End Sub

    Private Sub dtpDate_ValueChanged(sender As Object, e As EventArgs)
        ShowAllMachinesTimelines()
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs)
        ShowAllMachinesTimelines()
    End Sub

    Private Sub ShowAllMachinesTimelines()
        livePictureBoxes.Clear()
        flowTimelines.Controls.Clear()
        dgvIdleTimes.Visible = False
        Dim selectedDate As Date = dtpDate.Value.Date
        Dim machines As New List(Of Tuple(Of Integer, String))
        ' جلب كل الماكينات
        Using con As New SqlConnection(sqlServerConnectionString)
            con.Open()
            Dim cmd As New SqlCommand("SELECT DISTINCT WorkOrder.MachineID, Machine.name_arab FROM WorkOrder LEFT JOIN Machine ON WorkOrder.MachineID = Machine.id ORDER BY Machine.name_arab", con)
            Dim dr As SqlDataReader = cmd.ExecuteReader()
            While dr.Read()
                machines.Add(New Tuple(Of Integer, String)(CInt(dr("MachineID")), dr("name_arab").ToString()))
            End While
        End Using
        For Each m In machines
            Dim machineId = m.Item1
            Dim machineName = m.Item2
            ' جلب أمر الشغل الحالى (LIVE) لكل ماكينة
            Dim liveWorderID As String = ""
            Dim liveStartTime As DateTime = DateTime.MinValue
            Using con As New SqlConnection(sqlServerConnectionString)
                con.Open()
                Dim cmdLive As New SqlCommand("SELECT TOP 1 WorderID, StartTime FROM LiveProduction WHERE MachineID=@MachineID", con)
                cmdLive.Parameters.AddWithValue("@MachineID", machineId)
                Dim drLive As SqlDataReader = cmdLive.ExecuteReader()
                If drLive.Read() Then
                    liveWorderID = drLive("WorderID").ToString()
                    liveStartTime = drLive.GetDateTime(1)
                End If
            End Using
            ' جلب فترات التشغيل لهذه الماكينة
            Dim dayStart1 As DateTime = selectedDate.AddHours(8)
            Dim dayEnd1 As DateTime = selectedDate.AddDays(1)
            Dim dayStart2 As DateTime = selectedDate.AddDays(1)
            Dim dayEnd2 As DateTime = selectedDate.AddDays(1).AddHours(8)
            Dim allPeriods As New List(Of Tuple(Of DateTime, DateTime, String))
            Using con As New SqlConnection(sqlServerConnectionString)
                con.Open()
                Dim cmd1 As New SqlCommand("SELECT StartTime, EndTime, worder_merged FROM WorkOrder WHERE MachineID=@MachineID AND ((StartTime >= @DayStart1 AND StartTime < @DayEnd1) OR (EndTime > @DayStart1 AND EndTime <= @DayEnd1) OR (StartTime <= @DayStart1 AND EndTime >= @DayEnd1)) ORDER BY StartTime", con)
                cmd1.Parameters.AddWithValue("@MachineID", machineId)
                cmd1.Parameters.AddWithValue("@DayStart1", dayStart1)
                cmd1.Parameters.AddWithValue("@DayEnd1", dayEnd1)
                Dim dr1 As SqlDataReader = cmd1.ExecuteReader()
                While dr1.Read()
                    Dim s As DateTime = dr1.GetDateTime(0)
                    Dim en As DateTime = dr1.GetDateTime(1)
                    Dim wid As String = dr1("worder_merged").ToString()
                    If en > dayEnd1 Then en = dayEnd1
                    allPeriods.Add(New Tuple(Of DateTime, DateTime, String)(s, en, wid))
                End While
                dr1.Close()
                Dim cmd2 As New SqlCommand("SELECT StartTime, EndTime, worder_merged FROM WorkOrder WHERE MachineID=@MachineID AND ((StartTime >= @DayStart2 AND StartTime < @DayEnd2) OR (EndTime > @DayStart2 AND EndTime <= @DayEnd2) OR (StartTime <= @DayStart2 AND EndTime >= @DayEnd2)) ORDER BY StartTime", con)
                cmd2.Parameters.AddWithValue("@MachineID", machineId)
                cmd2.Parameters.AddWithValue("@DayStart2", dayStart2)
                cmd2.Parameters.AddWithValue("@DayEnd2", dayEnd2)
                Dim dr2 As SqlDataReader = cmd2.ExecuteReader()
                While dr2.Read()
                    Dim s As DateTime = dr2.GetDateTime(0)
                    Dim en As DateTime = dr2.GetDateTime(1)
                    Dim wid As String = dr2("worder_merged").ToString()
                    If en > dayEnd2 Then en = dayEnd2
                    allPeriods.Add(New Tuple(Of DateTime, DateTime, String)(s, en, wid))
                End While
                dr2.Close()
            End Using
            ' بناء فترات التشغيل والتوقف
            Dim timelinePeriods As New List(Of WorkPeriod)()
            Dim timelineStart As DateTime = dayStart1
            Dim timelineEnd As DateTime = dayEnd2
            Dim sortedPeriods = allPeriods.OrderBy(Function(p) p.Item1).ToList()
            Dim lastEnd As DateTime = timelineStart
            For Each p In sortedPeriods
                ' إذا كان هناك فجوة بين نهاية آخر فترة وبداية الفترة الحالية
                If p.Item1 > lastEnd Then
                    ' تحقق من وجود توقف مجدول
                    Dim isScheduled As Boolean = False
                    Dim stopReason As String = ""
                    Using con2 As New SqlConnection(sqlServerConnectionString)
                        con2.Open()
                        Dim cmdStop As New SqlCommand("SELECT TOP 1 StopReason FROM StopSchedule WHERE machineid=@mid AND ((@from < StopEndTime AND @to > StopStartTime))", con2)
                        cmdStop.Parameters.AddWithValue("@mid", machineId)
                        cmdStop.Parameters.AddWithValue("@from", lastEnd)
                        cmdStop.Parameters.AddWithValue("@to", p.Item1)
                        Dim drStop = cmdStop.ExecuteReader()
                        If drStop.Read() Then
                            isScheduled = True
                            stopReason = drStop("StopReason").ToString()
                        End If
                        drStop.Close()
                    End Using
                    ' إذا لم يكن توقف مجدول، اعتبرها توقف عادي (أحمر)
                    timelinePeriods.Add(New WorkPeriod With {.StartTime = lastEnd, .EndTime = p.Item1, .WOrderID = "", .IsIdle = True, .IsLive = False, .IsScheduledStop = isScheduled, .StopReason = stopReason})
                End If
                ' أضف فترة التشغيل
                timelinePeriods.Add(New WorkPeriod With {.StartTime = p.Item1, .EndTime = p.Item2, .WOrderID = p.Item3, .IsIdle = False, .IsLive = False, .IsScheduledStop = False, .StopReason = ""})
                If p.Item2 > lastEnd Then lastEnd = p.Item2
            Next
            ' إذا كان هناك فجوة بعد آخر فترة حتى نهاية اليوم
            If lastEnd < timelineEnd Then
                ' تحقق من وجود توقف مجدول
                Dim isScheduled As Boolean = False
                Dim stopReason As String = ""
                Using con2 As New SqlConnection(sqlServerConnectionString)
                    con2.Open()
                    Dim cmdStop As New SqlCommand("SELECT TOP 1 StopReason FROM StopSchedule WHERE machineid=@mid AND ((@from < StopEndTime AND @to > StopStartTime))", con2)
                    cmdStop.Parameters.AddWithValue("@mid", machineId)
                    cmdStop.Parameters.AddWithValue("@from", lastEnd)
                    cmdStop.Parameters.AddWithValue("@to", timelineEnd)
                    Dim drStop = cmdStop.ExecuteReader()
                    If drStop.Read() Then
                        isScheduled = True
                        stopReason = drStop("StopReason").ToString()
                    End If
                    drStop.Close()
                End Using
                ' إذا لم يكن توقف مجدول، اعتبرها توقف عادي (أحمر)
                timelinePeriods.Add(New WorkPeriod With {.StartTime = lastEnd, .EndTime = timelineEnd, .WOrderID = "", .IsIdle = True, .IsLive = False, .IsScheduledStop = isScheduled, .StopReason = stopReason})
            End If
            ' تعديل فترة التوقف الحالية إذا كان هناك أمر LIVE
            Dim currentTime As DateTime = DateTime.Now
            If liveWorderID <> "" Then
                For i = 0 To timelinePeriods.Count - 1
                    Dim period = timelinePeriods(i)
                    If period.IsIdle AndAlso currentTime >= period.StartTime AndAlso currentTime < period.EndTime Then
                        ' عدل الفترة لتكون LIVE
                        timelinePeriods(i) = New WorkPeriod With {
                            .StartTime = liveStartTime,
                            .EndTime = period.EndTime,
                            .WOrderID = liveWorderID,
                            .IsIdle = False,
                            .IsLive = True,
                            .IsScheduledStop = False,
                            .StopReason = ""
                        }
                        Exit For
                    End If
                Next
            End If
            ' دمج الفترات الخضراء المتداخلة
            timelinePeriods = MergeOverlappingWorkPeriods(timelinePeriods)
            ' Panel أفقي لكل ماكينة
            Dim pnl As New Panel()
            pnl.Height = 80
            pnl.Width = flowTimelines.ClientSize.Width - 20
            pnl.Margin = New Padding(5, 5, 5, 0)
            pnl.Padding = New Padding(0)
            pnl.BackColor = Color.Transparent
            ' اسم الماكينة
            Dim lbl As New Label()
            lbl.Text = machineName
            lbl.Font = New Font("Tahoma", 10, FontStyle.Bold)
            lbl.TextAlign = ContentAlignment.MiddleLeft
            lbl.Dock = DockStyle.Left
            lbl.Width = 120
            lbl.AutoSize = False
            lbl.Margin = New Padding(0)
            ' الشريط الزمني
            Dim pb As New PictureBox()
            pb.Dock = DockStyle.Fill
            pb.Height = 140
            pb.BackColor = Color.White
            pb.Margin = New Padding(0)
            AddHandler pb.Paint, Sub(sender As Object, e As PaintEventArgs)
                                     Dim g = e.Graphics
                                     g.Clear(Color.White)
                                     Dim totalMinutes As Double = (timelineEnd - timelineStart).TotalMinutes
                                     Dim width As Integer = pb.ClientSize.Width
                                     Dim barHeight As Integer = 32
                                     Dim y As Integer = 10
                                     Dim now As DateTime = DateTime.Now
                                     For Each period In timelinePeriods
                                         Dim startOffset As Double = (period.StartTime - timelineStart).TotalMinutes / totalMinutes * width
                                         Dim endOffset As Double = (period.EndTime - timelineStart).TotalMinutes / totalMinutes * width
                                         Dim rect As New Rectangle(CInt(startOffset), y, Math.Max(1, CInt(endOffset - startOffset)), barHeight)
                                         If period.IsLive Then
                                             ' إذا كان أمر الشغل live هو "No Work" أو "0" أو "1"، استخدم لون أحمر ووميض
                                             Dim isRedLive As Boolean = (period.WOrderID IsNot Nothing AndAlso (period.WOrderID.Trim() = "No Work" Or period.WOrderID.Trim() = "0" Or period.WOrderID.Trim() = "1"))
                                             Dim blinkColor As Brush
                                             If isRedLive Then
                                                 blinkColor = If(DateTime.Now.Second Mod 2 = 0, Brushes.Red, Brushes.DarkRed)
                                             Else
                                                 blinkColor = If(DateTime.Now.Second Mod 2 = 0, Brushes.Yellow, Brushes.Gold)
                                             End If
                                             g.FillRectangle(blinkColor, rect)
                                             ' رسم رقم أمر الشغل ووقت البداية باللون الأسود
                                             Dim sf As New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
                                             g.DrawString($"{period.WOrderID} - {period.StartTime:yyyy-MM-dd hh:mm tt}", New Font("Tahoma", 8, FontStyle.Bold), Brushes.Black, rect, sf)
                                         ElseIf period.IsIdle Then
                                             If period.IsScheduledStop Then
                                                 g.FillRectangle(New SolidBrush(Color.FromArgb(180, 120, 180, 255)), rect) ' أزرق فاتح
                                                 Dim sf As New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
                                                 g.DrawString(period.StopReason, New Font("Tahoma", 8, FontStyle.Bold), Brushes.Navy, rect, sf)
                                             Else
                                                 g.FillRectangle(Brushes.Red, rect)
                                             End If
                                         Else
                                             g.FillRectangle(Brushes.Green, rect)
                                             Dim sf As New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
                                             g.DrawString(period.WOrderID, New Font("Tahoma", 8, FontStyle.Bold), Brushes.White, rect, sf)
                                         End If
                                         g.DrawRectangle(Pens.Black, rect)
                                     Next
                                     ' رسم خط زمني للساعات بشكل أوضح
                                     Dim fontHour As New Font("Tahoma", 9, FontStyle.Bold)
                                     Dim brushHour As New SolidBrush(Color.Navy)
                                     Dim bgBrush As New SolidBrush(Color.White)
                                     Dim minHourStep As Integer = If(width < 800, 2, 1)
                                     For h = 0 To 24 Step minHourStep
                                         Dim hourTime = timelineStart.AddHours(h)
                                         If hourTime > timelineEnd Then Exit For
                                         Dim x = CInt((hourTime - timelineStart).TotalMinutes / totalMinutes * width)
                                         Dim hourText As String = hourTime.ToString("hh:mm")
                                         Dim ampmText As String = hourTime.ToString("tt")
                                         Dim szHour = g.MeasureString(hourText, fontHour)
                                         Dim szAMPM = g.MeasureString(ampmText, fontHour)
                                         ' رسم مستطيل أبيض يغطي السطرين
                                         Dim totalHeight As Single = szHour.Height + szAMPM.Height
                                         Dim maxWidth As Single = Math.Max(szHour.Width, szAMPM.Width)
                                         g.FillRectangle(bgBrush, x - maxWidth / 2, y + barHeight + 20, maxWidth, totalHeight)
                                         ' رسم الساعة في الأعلى
                                         g.DrawString(hourText, fontHour, Brushes.Black, x, y + barHeight + 20, New StringFormat With {.Alignment = StringAlignment.Center})
                                         ' رسم AM/PM تحت الساعة باللون الأحمر
                                         g.DrawString(ampmText, fontHour, Brushes.Red, x, y + barHeight + 20 + szHour.Height, New StringFormat With {.Alignment = StringAlignment.Center})
                                         g.DrawLine(Pens.Gray, x, y + barHeight, x, y + barHeight + 22)
                                     Next
                                     ' تعريف المتغيرات المطلوبة لرسم AM و PM
                                     amStartX = 0
                                     amEndX = CInt(((timelineStart.AddHours(4) - timelineStart).TotalMinutes / totalMinutes) * width) ' 8:00 + 4 ساعات = 12:00
                                     pmStartX = amEndX
                                     pmEndX = width
                                     amCenterX = (amStartX + amEndX) \ 2
                                     pmCenterX = (pmStartX + pmEndX) \ 2
                                     ' رسم كلمة AM و PM أسفل الساعات مباشرة
                                     Dim ampmY As Integer = y + barHeight + 20 + fontHour.Height + 5
                                     g.DrawString("AM", ampmFont, ampmBrush, amCenterX, ampmY, New StringFormat With {.Alignment = StringAlignment.Center})
                                     g.DrawString("PM", ampmFont, ampmBrush, pmCenterX, ampmY, New StringFormat With {.Alignment = StringAlignment.Center})
                                 End Sub
            ' حدث الضغط على الشريط
            AddHandler pb.MouseClick, Sub(sender As Object, e As MouseEventArgs)
                                          Dim totalMinutes As Double = (timelineEnd - timelineStart).TotalMinutes
                                          Dim width As Integer = pb.ClientSize.Width
                                          Dim xClick As Integer = e.X
                                          Dim clickedTime As DateTime = timelineStart.AddMinutes(xClick / width * totalMinutes)
                                          For Each period In timelinePeriods
                                              If clickedTime >= period.StartTime AndAlso clickedTime < period.EndTime Then
                                                  ' فتح فورم إضافة أمر شغل مع ربط الحدث
                                                  Dim frm As New frmAddWorkOrder(machineId, period.StartTime, period.EndTime)
                                                  AddHandler frm.WorkOrderSaved, Sub(sender2, e2)
                                                                                     ShowAllMachinesTimelines()
                                                                                 End Sub
                                                  frm.ShowDialog()
                                                  Exit For
                                              End If
                                          Next
                                      End Sub
            ' Tooltip عند الوقوف بالماوس
            AddHandler pb.MouseMove, Sub(sender As Object, e As MouseEventArgs)
                                         Dim totalMinutes As Double = (timelineEnd - timelineStart).TotalMinutes
                                         Dim width As Integer = pb.ClientSize.Width
                                         Dim xMove As Integer = e.X
                                         Dim hoveredTime As DateTime = timelineStart.AddMinutes(xMove / width * totalMinutes)
                                         Dim found As Boolean = False
                                         For Each period In timelinePeriods
                                             If hoveredTime >= period.StartTime AndAlso hoveredTime < period.EndTime Then
                                                 If lastTooltipPeriod.HasValue = False OrElse Not period.Equals(lastTooltipPeriod.Value) Then
                                                     Dim msg As String
                                                     If period.IsIdle Then
                                                         msg = $"توقف\nمن: {period.StartTime:yyyy-MM-dd HH:mm}\nإلى: {period.EndTime:yyyy-MM-dd HH:mm}"
                                                     Else
                                                         msg = $"أمر الشغل: {period.WOrderID}\nمن: {period.StartTime:yyyy-MM-dd HH:mm}\nإلى: {period.EndTime:yyyy-MM-dd HH:mm}"
                                                     End If
                                                     timelineTooltip.SetToolTip(pb, msg)
                                                     lastTooltipPeriod = period
                                                 End If
                                                 found = True
                                                 Exit For
                                             End If
                                         Next
                                         If Not found Then
                                             timelineTooltip.SetToolTip(pb, "")
                                             lastTooltipPeriod = Nothing
                                         End If
                                     End Sub
            ' أوقف التايمر عند دخول الماوس وأعد تشغيله عند الخروج
            AddHandler pb.MouseEnter, Sub(sender As Object, e As EventArgs)
                                          blinkTimer.Stop()
                                      End Sub
            AddHandler pb.MouseLeave, Sub(sender As Object, e As EventArgs)
                                          blinkTimer.Start()
                                          timelineTooltip.SetToolTip(pb, "")
                                          lastTooltipPeriod = Nothing
                                      End Sub
            If timelinePeriods.Any(Function(p) p.IsLive) Then
                livePictureBoxes.Add(pb)
            End If
            pnl.Controls.Add(pb)
            pnl.Controls.Add(lbl)
            flowTimelines.Controls.Add(pnl)
        Next
    End Sub

    ' إعادة رسم الشاشات عند كل وميض
    Private Sub blinkTimer_Tick(sender As Object, e As EventArgs) Handles blinkTimer.Tick
        For Each pb In livePictureBoxes
            pb.Invalidate()
        Next
    End Sub

    ' دالة لدمج الفترات الخضراء المتداخلة
    Private Function MergeOverlappingWorkPeriods(periods As List(Of WorkPeriod)) As List(Of WorkPeriod)
        Dim result As New List(Of WorkPeriod)()
        ' فقط الفترات الخضراء
        Dim greenPeriods = periods.Where(Function(p) Not p.IsIdle AndAlso Not p.IsLive).OrderBy(Function(p) p.StartTime).ToList()
        Dim idlePeriods = periods.Where(Function(p) p.IsIdle Or p.IsLive).ToList()
        Dim i As Integer = 0
        While i < greenPeriods.Count
            Dim current = greenPeriods(i)
            Dim mergedStart = current.StartTime
            Dim mergedEnd = current.EndTime
            Dim worderIDs As New List(Of String)()
            worderIDs.Add(current.WOrderID)
            Dim j = i + 1
            While j < greenPeriods.Count AndAlso greenPeriods(j).StartTime < mergedEnd
                mergedEnd = If(greenPeriods(j).EndTime > mergedEnd, greenPeriods(j).EndTime, mergedEnd)
                worderIDs.Add(greenPeriods(j).WOrderID)
                j += 1
            End While
            result.Add(New WorkPeriod With {
                .StartTime = mergedStart,
                .EndTime = mergedEnd,
                .WOrderID = String.Join("-", worderIDs.Distinct()),
                .IsIdle = False,
                .IsLive = False,
                .IsScheduledStop = False,
                .StopReason = ""
            })
            i = j
        End While
        ' أضف الفترات الحمراء (توقف) وLive كما هي
        result.AddRange(idlePeriods)
        ' أعد ترتيب الكل حسب البداية
        Return result.OrderBy(Function(p) p.StartTime).ToList()
    End Function
End Class
