Imports System.Data.SqlClient
Imports System.Windows.Forms

Public Class frmAddWorkOrder
    Inherits Form

    Private lblMachine As New Label()
    Private cmbMachine As New ComboBox()
    Private lblProcess As New Label()
    Private cmbProcess As New ComboBox()
    Private lblStart As New Label()
    Private dtpStart As New DateTimePicker()
    Private lblEnd As New Label()
    Private dtpEnd As New DateTimePicker()
    Private lblDuration As New Label()
    Private txtDuration As New TextBox()
    Private lblWorderID As New Label()
    Private txtWorderID As New TextBox()
    Private lblQuantityKg As New Label()
    Private txtQuantityKg As New TextBox()
    Private lblQuantityM As New Label()
    Private txtQuantityM As New TextBox()
    Private lblSpeed As New Label()
    Private txtSpeed As New TextBox()
    Private lblNotes As New Label()
    Private txtNotes As New TextBox()
    Private btnSave As New Button()

    Private sqlServerConnectionString As String = "Server=localhost;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"

    Public Property SelectedMachineID As Integer
    Public Property SelectedStart As DateTime
    Public Property SelectedEnd As DateTime

    Public Event WorkOrderSaved As EventHandler

    Public Sub New(Optional machineId As Integer = 0, Optional startTime As DateTime = Nothing, Optional endTime As DateTime = Nothing)
        InitializeComponent()
        Me.Text = "إضافة أمر شغل جديد"
        Me.Size = New Size(500, 500)
        Me.StartPosition = FormStartPosition.CenterParent
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False

        ' ترتيب العناصر
        lblMachine.Text = "الماكينة:"
        lblMachine.Location = New Point(30, 30)
        lblMachine.Size = New Size(80, 25)
        cmbMachine.Location = New Point(120, 30)
        cmbMachine.Size = New Size(200, 25)
        AddHandler cmbMachine.SelectedIndexChanged, AddressOf cmbMachine_SelectedIndexChanged

        lblProcess.Text = "المرحلة:"
        lblProcess.Location = New Point(30, 70)
        lblProcess.Size = New Size(80, 25)
        cmbProcess.Location = New Point(120, 70)
        cmbProcess.Size = New Size(200, 25)

        lblStart.Text = "من:"
        lblStart.Location = New Point(30, 110)
        lblStart.Size = New Size(80, 25)
        dtpStart.Location = New Point(120, 110)
        dtpStart.Size = New Size(200, 25)
        dtpStart.Format = DateTimePickerFormat.Custom
        dtpStart.CustomFormat = "yyyy-MM-dd hh:mm:ss tt"
        AddHandler dtpStart.ValueChanged, AddressOf UpdateDuration

        lblEnd.Text = "إلى:"
        lblEnd.Location = New Point(30, 150)
        lblEnd.Size = New Size(80, 25)
        dtpEnd.Location = New Point(120, 150)
        dtpEnd.Size = New Size(200, 25)
        dtpEnd.Format = DateTimePickerFormat.Custom
        dtpEnd.CustomFormat = "yyyy-MM-dd hh:mm:ss tt"
        AddHandler dtpEnd.ValueChanged, AddressOf UpdateDuration

        lblDuration.Text = "المدة:"
        lblDuration.Location = New Point(30, 190)
        lblDuration.Size = New Size(80, 25)
        txtDuration.Location = New Point(120, 190)
        txtDuration.Size = New Size(200, 25)
        txtDuration.ReadOnly = True

        lblWorderID.Text = "رقم أمر الشغل:"
        lblWorderID.Location = New Point(30, 230)
        lblWorderID.Size = New Size(80, 25)
        txtWorderID.Location = New Point(120, 230)
        txtWorderID.Size = New Size(200, 25)

        lblQuantityKg.Text = "الكمية (وزن):"
        lblQuantityKg.Location = New Point(30, 270)
        lblQuantityKg.Size = New Size(80, 25)
        txtQuantityKg.Location = New Point(120, 270)
        txtQuantityKg.Size = New Size(200, 25)

        lblQuantityM.Text = "الكمية (متر):"
        lblQuantityM.Location = New Point(30, 310)
        lblQuantityM.Size = New Size(80, 25)
        txtQuantityM.Location = New Point(120, 310)
        txtQuantityM.Size = New Size(200, 25)

        lblSpeed.Text = "السرعة:"
        lblSpeed.Location = New Point(30, 350)
        lblSpeed.Size = New Size(80, 25)
        txtSpeed.Location = New Point(120, 350)
        txtSpeed.Size = New Size(200, 25)

        lblNotes.Text = "ملاحظات:"
        lblNotes.Location = New Point(30, 390)
        lblNotes.Size = New Size(80, 25)
        txtNotes.Location = New Point(120, 390)
        txtNotes.Size = New Size(200, 25)

        btnSave.Text = "تسجيل"
        btnSave.Location = New Point(200, 430)
        btnSave.Size = New Size(100, 30)
        AddHandler btnSave.Click, AddressOf btnSave_Click

        Me.Controls.AddRange(New Control() {lblMachine, cmbMachine, lblProcess, cmbProcess, lblStart, dtpStart, lblEnd, dtpEnd, lblDuration, txtDuration, lblWorderID, txtWorderID, lblQuantityKg, txtQuantityKg, lblQuantityM, txtQuantityM, lblSpeed, txtSpeed, lblNotes, txtNotes, btnSave})

        ' تعبئة ComboBox الماكينات
        LoadMachines()
        If machineId <> 0 Then
            cmbMachine.SelectedValue = machineId
        End If
        If startTime <> Nothing Then dtpStart.Value = startTime
        If endTime <> Nothing Then dtpEnd.Value = endTime
        UpdateDuration(Nothing, Nothing)
    End Sub

    Private Sub LoadMachines()
        cmbMachine.DataSource = Nothing
        Dim dt As New DataTable()
        Using con As New SqlConnection(sqlServerConnectionString)
            con.Open()
            Dim cmd As New SqlCommand("SELECT id, name_arab FROM Machine ORDER BY name_arab", con)
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(dt)
        End Using
        If dt.Rows.Count > 0 Then
            cmbMachine.DataSource = dt
            cmbMachine.DisplayMember = "name_arab"
            cmbMachine.ValueMember = "id"
        End If
    End Sub

    Private Sub cmbMachine_SelectedIndexChanged(sender As Object, e As EventArgs)
        LoadProcesses()
    End Sub

    Private Sub LoadProcesses()
        cmbProcess.DataSource = Nothing
        If cmbMachine.SelectedValue Is Nothing Then Return
        Dim dt As New DataTable()
        Using con As New SqlConnection(sqlServerConnectionString)
            con.Open()
            Dim cmd As New SqlCommand("SELECT id, name_ar FROM machine_steps WHERE machine_id = @MachineID", con)
            Dim machineId As Object = cmbMachine.SelectedValue
            If TypeOf machineId Is DataRowView Then
                machineId = CType(machineId, DataRowView)("id")
            End If
            cmd.Parameters.AddWithValue("@MachineID", machineId)
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(dt)
        End Using
        If dt.Rows.Count > 0 Then
            cmbProcess.DataSource = dt
            cmbProcess.DisplayMember = "name_ar"
            cmbProcess.ValueMember = "id"
        End If
    End Sub

    Private Sub UpdateDuration(sender As Object, e As EventArgs)
        Dim diff As TimeSpan = dtpEnd.Value - dtpStart.Value
        If diff.TotalSeconds < 0 Then
            txtDuration.Text = "00:00:00"
        Else
            txtDuration.Text = diff.ToString("hh\:mm\:ss")
        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs)
        ' تجهيز نص رسالة التأكيد
        Dim machineName As String = cmbMachine.Text
        Dim worderId As String = txtWorderID.Text
        Dim startTime As String = dtpStart.Value.ToString("yyyy-MM-dd HH:mm")
        Dim endTime As String = dtpEnd.Value.ToString("yyyy-MM-dd HH:mm")
        Dim duration As String = txtDuration.Text

        Dim confirmMsg As String = $"هل تريد تسجيل أمر شغل رقم {worderId} ووقت تشغيل {duration} من يوم {startTime} إلى يوم {endTime} على ماكينة {machineName}؟"
        Dim result As DialogResult = MessageBox.Show(confirmMsg, "تأكيد التسجيل", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result <> DialogResult.Yes Then
            Return
        End If

        Using con As New SqlConnection(sqlServerConnectionString)
            con.Open()
            Dim cmd As New SqlCommand("INSERT INTO WorkOrder (WorderID, Speed, StartTime, EndTime, Duration, MachineID, processid, notes, total_time, quantitym, quantitykg,worder_merged) VALUES (@WorderID, @Speed, @StartTime, @EndTime, @Duration, @MachineID, @processid, @notes, @total_time, @quantitym, @quantitykg,@worder_merged)", con)
            cmd.Parameters.AddWithValue("@WorderID", txtWorderID.Text)
            cmd.Parameters.AddWithValue("@worder_merged", txtWorderID.Text)
            cmd.Parameters.AddWithValue("@Speed", txtSpeed.Text)
            cmd.Parameters.AddWithValue("@StartTime", dtpStart.Value)
            cmd.Parameters.AddWithValue("@EndTime", dtpEnd.Value)
            cmd.Parameters.AddWithValue("@Duration", txtDuration.Text)
            cmd.Parameters.AddWithValue("@MachineID", cmbMachine.SelectedValue)
            cmd.Parameters.AddWithValue("@processid", cmbProcess.SelectedValue)
            cmd.Parameters.AddWithValue("@notes", txtNotes.Text)
            cmd.Parameters.AddWithValue("@total_time", txtDuration.Text)
            cmd.Parameters.AddWithValue("@quantitym", txtQuantityM.Text)
            cmd.Parameters.AddWithValue("@quantitykg", txtQuantityKg.Text)
            cmd.ExecuteNonQuery()
        End Using
        MessageBox.Show("تم تسجيل أمر الشغل بنجاح.")
        RaiseEvent WorkOrderSaved(Me, EventArgs.Empty)
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub
End Class
