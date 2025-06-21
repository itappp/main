Imports System.Data.SqlClient

Public Class insertcolorsform
    ' تعريف عناصر الفورم
    Private txtcode As New TextBox()
    Private cmbname As New ComboBox()
    Private txtnotes As New TextBox()
    Private btninsert As New Button()
    Private lblcode As New Label()
    Private lblname As New Label()
    Private lblnotes As New Label()

    Private connectionString As String = "Server=localhost;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"
    Private colorNames As New List(Of String)()

    Private Sub insertcolorsform_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' تكبير الفورم
        Me.WindowState = FormWindowState.Maximized
        Me.Text = "إضافة لون جديد"
        Me.BackColor = Color.White

        Dim bigFont As New Font("Tahoma", 18, FontStyle.Bold)
        Dim labelFont As New Font("Tahoma", 16, FontStyle.Regular)

        ' إعداد خصائص العناصر
        lblcode.Text = "كود اللون:"
        lblcode.Font = labelFont
        lblcode.Location = New Point(100, 100)
        lblcode.AutoSize = True

        txtcode.Location = New Point(300, 95)
        txtcode.Width = 400
        txtcode.Font = bigFont
        txtcode.Text = "كود اللون"
        txtcode.ForeColor = Color.Gray
        AddHandler txtcode.GotFocus, AddressOf RemovePlaceholderCode
        AddHandler txtcode.LostFocus, AddressOf SetPlaceholderCode

        lblname.Text = "اسم اللون:"
        lblname.Font = labelFont
        lblname.Location = New Point(100, 180)
        lblname.AutoSize = True

        cmbname.Location = New Point(300, 175)
        cmbname.Width = 400
        cmbname.Font = bigFont
        cmbname.DropDownStyle = ComboBoxStyle.DropDown
        AddHandler cmbname.TextChanged, AddressOf cmbname_TextChanged

        lblnotes.Text = "ملاحظات:"
        lblnotes.Font = labelFont
        lblnotes.Location = New Point(100, 260)
        lblnotes.AutoSize = True

        txtnotes.Location = New Point(300, 255)
        txtnotes.Width = 400
        txtnotes.Font = bigFont
        txtnotes.Text = "ملاحظات"
        txtnotes.ForeColor = Color.Gray
        AddHandler txtnotes.GotFocus, AddressOf RemovePlaceholderNotes
        AddHandler txtnotes.LostFocus, AddressOf SetPlaceholderNotes

        btninsert.Width = 400
        btninsert.Height = 50
        btninsert.Font = bigFont
        btninsert.Text = "إضافة"
        btninsert.BackColor = Color.LightGreen
        btninsert.ForeColor = Color.Black
        ' مكان الزر على اليمين
        btninsert.Location = New Point(Me.ClientSize.Width - btninsert.Width - 100, 340)
        btninsert.Anchor = AnchorStyles.Top Or AnchorStyles.Right

        ' إضافة العناصر للفورم
        Me.Controls.Add(lblcode)
        Me.Controls.Add(txtcode)
        Me.Controls.Add(lblname)
        Me.Controls.Add(cmbname)
        Me.Controls.Add(lblnotes)
        Me.Controls.Add(txtnotes)
        Me.Controls.Add(btninsert)

        ' ربط حدث زر الإضافة
        AddHandler btninsert.Click, AddressOf btninsert_Click

        ' تحميل أسماء الألوان المميزة
        LoadColorNames()
        UpdateComboBoxAutoComplete("")

        ' ربط حدث تغيير حجم الفورم
        AddHandler Me.Resize, AddressOf insertcolorsform_Resize
        ' ضبط مكان الزر أول مرة
        AdjustInsertButtonLocation()
    End Sub

    Private Sub insertcolorsform_Resize(sender As Object, e As EventArgs)
        AdjustInsertButtonLocation()
    End Sub

    Private Sub AdjustInsertButtonLocation()
        btninsert.Location = New Point(Me.ClientSize.Width - btninsert.Width - 100, 340)
    End Sub

    Private Sub LoadColorNames()
        colorNames.Clear()
        Dim con As New SqlConnection(connectionString)
        Dim cmd As New SqlCommand("SELECT DISTINCT name FROM color_code", con)
        con.Open()
        Dim reader = cmd.ExecuteReader()
        While reader.Read()
            colorNames.Add(reader("name").ToString())
        End While
        con.Close()
        cmbname.Items.Clear()
        cmbname.Items.AddRange(colorNames.ToArray())
    End Sub

    Private Sub UpdateComboBoxAutoComplete(filter As String)
        Dim autoSource As New AutoCompleteStringCollection()
        If filter = "" Then
            autoSource.AddRange(colorNames.ToArray())
        Else
            autoSource.AddRange(colorNames.Where(Function(n) n.ToLower().Contains(filter.ToLower())).ToArray())
        End If
        cmbname.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cmbname.AutoCompleteSource = AutoCompleteSource.CustomSource
        cmbname.AutoCompleteCustomSource = autoSource
    End Sub

    Private Sub cmbname_TextChanged(sender As Object, e As EventArgs)
        UpdateComboBoxAutoComplete(cmbname.Text)
    End Sub

    Private Sub btninsert_Click(sender As Object, e As EventArgs)
        Dim code As String = txtcode.Text.Trim()
        Dim name As String = cmbname.Text.Trim()
        Dim notes As String = txtnotes.Text.Trim()

        If code = "" Or code = "كود اللون" Or name = "" Then
            MessageBox.Show("يرجى إدخال الكود والاسم.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Using con As New SqlConnection(connectionString)
            Dim checkCmd As New SqlCommand("SELECT COUNT(*) FROM color_code WHERE code = @code", con)
            checkCmd.Parameters.AddWithValue("@code", code)
            con.Open()
            Dim exists As Integer = CInt(checkCmd.ExecuteScalar())
            If exists > 0 Then
                MessageBox.Show("هذا الكود موجود بالفعل.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            Dim insertCmd As New SqlCommand("INSERT INTO color_code (code, name, notes) VALUES (@code, @name, @notes)", con)
            insertCmd.Parameters.AddWithValue("@code", code)
            insertCmd.Parameters.AddWithValue("@name", name)
            insertCmd.Parameters.AddWithValue("@notes", notes)
            insertCmd.ExecuteNonQuery()
            MessageBox.Show("تمت الإضافة بنجاح.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Using

        ' إذا الاسم غير موجود في القائمة أضفه
        If Not colorNames.Contains(name) Then
            colorNames.Add(name)
            cmbname.Items.Add(name)
        End If
        txtcode.Text = "كود اللون"
        txtcode.ForeColor = Color.Gray
        cmbname.Text = ""
        txtnotes.Text = "ملاحظات"
        txtnotes.ForeColor = Color.Gray
    End Sub

    Private Sub RemovePlaceholderCode(sender As Object, e As EventArgs)
        If txtcode.Text = "كود اللون" Then
            txtcode.Text = ""
            txtcode.ForeColor = Color.Black
        End If
    End Sub

    Private Sub SetPlaceholderCode(sender As Object, e As EventArgs)
        If txtcode.Text = "" Then
            txtcode.Text = "كود اللون"
            txtcode.ForeColor = Color.Gray
        End If
    End Sub

    Private Sub RemovePlaceholderNotes(sender As Object, e As EventArgs)
        If txtnotes.Text = "ملاحظات" Then
            txtnotes.Text = ""
            txtnotes.ForeColor = Color.Black
        End If
    End Sub

    Private Sub SetPlaceholderNotes(sender As Object, e As EventArgs)
        If txtnotes.Text = "" Then
            txtnotes.Text = "ملاحظات"
            txtnotes.ForeColor = Color.Gray
        End If
    End Sub
End Class