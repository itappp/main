Imports System.Data.SqlClient
Imports System.Net
Imports System.Net.Mail
Imports ZXing
Imports ZXing.Common
Imports System.IO.Ports
Imports System.Management
Public Class mainrawstoreform
    Private connectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"
    Private centerX As Integer
    Private saveIcon As Image
    Private hasNewRow As Boolean = False
    Private WithEvents btnSaveRolls As New Button()
    Private WithEvents btnReadWeight As New Button()

    ' Controls declaration
    Private WithEvents cmbMessageType As New ComboBox()
    Private WithEvents lblBatchNumber As New Label()
    Private WithEvents cmbPO As New ComboBox()
    Private WithEvents cmbBatch As New ComboBox()
    Private WithEvents lblPO As New Label()
    Private WithEvents lblBatch As New Label()
    Private WithEvents dgvBatchDetails As New DataGridView()
    Private WithEvents dgvRolls As New DataGridView()
    Private WithEvents btnAddRolls As New Button()
    Private WithEvents cmbLotAction As New ComboBox()
    Private WithEvents lblLotAction As New Label()
    Private WithEvents dgvBatchInfo As New DataGridView()
    Private WithEvents cmbLocation As New ComboBox() ' Add location ComboBox
    Private WithEvents lblLocation As New Label() ' Add location label
    Private WithEvents lblusername As New Label() ' Add username label

    ' New controls for weight distribution
    Private WithEvents txtTotalWeight As New TextBox()
    Private WithEvents txtNumberOfRolls As New TextBox()
    Private WithEvents btnDivideWeight As New Button()
    Private WithEvents lblTotalWeight As New Label()
    Private WithEvents lblNumberOfRolls As New Label()

    ' New controls for batch creation
    Private WithEvents cmbservice As New ComboBox()
    Private WithEvents cmbstyle As New ComboBox()
    Private WithEvents cmbclient As New ComboBox()
    Private WithEvents cmbsupplier As New ComboBox()
    Private WithEvents cmbkindfabric As New ComboBox()
    Private WithEvents txtpo As New TextBox()
    Private WithEvents txtmaterial As New TextBox()
    Private WithEvents btninsert As New Button()
    Private WithEvents lblclient As New Label()
    Private WithEvents lblsup As New Label()
    Private WithEvents lblstyle As New Label()
    Private WithEvents lblNewBatchNumber As New Label()

    ' Labels for controls
    Private WithEvents lblService As New Label()
    Private WithEvents lblClientCode As New Label()
    Private WithEvents lblSupplier As New Label()
    Private WithEvents lblStyleCode As New Label()
    Private WithEvents lblFabric As New Label()
    Private WithEvents lblPONumber As New Label()
    Private WithEvents lblMaterial As New Label()
    Private WithEvents lblMessageType As New Label()

    ' State tracking variables
    Private isServiceLoaded As Boolean = False
    Private isStyleLoaded As Boolean = False
    Private isClientLoaded As Boolean = False
    Private isSupplierLoaded As Boolean = False

    ' Add at the top of the class with other declarations
    Private serialPort As New SerialPort
    Private selectedRow As Integer = -1

    Private isScaleConnected As Boolean = False

    ' Add this function after the class declarations
    Private Sub InitializeSerialPort()
        Try
            ' Get available ports
            Dim ports As String() = SerialPort.GetPortNames()

            ' Look for the Prolific port
            Dim prolificPort As String = Nothing
            For Each port In ports
                ' Use Windows Management Instrumentation (WMI) to get port details
                Using searcher As New Management.ManagementObjectSearcher(
                    "SELECT * FROM Win32_PnPEntity WHERE Caption LIKE '%Prolific PL2303GT%'")
                    For Each queryObj As Management.ManagementObject In searcher.Get()
                        If queryObj("Caption").ToString().Contains("Prolific PL2303GT") Then
                            ' Extract COM port number from the caption
                            Dim caption As String = queryObj("Caption").ToString()
                            Dim match As System.Text.RegularExpressions.Match =
                                System.Text.RegularExpressions.Regex.Match(caption, "COM[0-9]+")
                            If match.Success Then
                                prolificPort = match.Value
                                Exit For
                            End If
                        End If
                    Next
                End Using
                If prolificPort IsNot Nothing Then Exit For
            Next

            If prolificPort Is Nothing Then
                MessageBox.Show("لم يتم العثور على منفذ Prolific PL2303GT. الرجاء التأكد من توصيل الجهاز.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                isScaleConnected = False
                Return
            End If

            ' Configure serial port with longer timeouts
            serialPort.PortName = prolificPort
            serialPort.BaudRate = 9600
            serialPort.DataBits = 8
            serialPort.StopBits = StopBits.One
            serialPort.Parity = Parity.None
            serialPort.ReadTimeout = 3000  ' 3 seconds
            serialPort.WriteTimeout = 3000 ' 3 seconds
            serialPort.Handshake = Handshake.None
            serialPort.DtrEnable = True
            serialPort.RtsEnable = True

            ' Test the connection
            Try
                serialPort.Open()
                serialPort.Close()
                isScaleConnected = True
                MessageBox.Show("تم الاتصال بالميزان بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                isScaleConnected = False
                MessageBox.Show("لا يمكن الاتصال بالميزان. الرجاء التأكد من توصيل الجهاز وتثبيت برنامج التشغيل.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

        Catch ex As Exception
            isScaleConnected = False
            MessageBox.Show("خطأ في تهيئة المنفذ التسلسلي: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Add this function to read weight from scale
    Private Function ReadWeightFromScale() As String
        Try
            If Not serialPort.IsOpen Then
                serialPort.ReadTimeout = 3000
                serialPort.WriteTimeout = 3000
                serialPort.Open()
            End If

            ' Clear buffers
            serialPort.DiscardInBuffer()
            serialPort.DiscardOutBuffer()

            ' Send command to request weight
            serialPort.WriteLine("W")
            System.Threading.Thread.Sleep(500)

            ' Read response
            Dim response As String = ""
            Dim buffer(256) As Byte
            Dim bytesRead As Integer = 0

            Try
                While serialPort.BytesToRead > 0 AndAlso bytesRead < buffer.Length
                    buffer(bytesRead) = CByte(serialPort.ReadByte())
                    bytesRead += 1
                End While

                If bytesRead > 0 Then
                    ' Convert bytes to string
                    response = System.Text.Encoding.ASCII.GetString(buffer, 0, bytesRead)

                    ' Clean and process the response
                    ' Remove any non-numeric characters except decimal point
                    response = New String(response.Where(Function(c) Char.IsDigit(c) OrElse c = "."c).ToArray())

                    ' Try to find a valid decimal number in the response
                    Dim match = System.Text.RegularExpressions.Regex.Match(response, "\d+\.?\d*")
                    If match.Success Then
                        Dim weightStr As String = match.Value
                        Dim weight As Decimal

                        If Decimal.TryParse(weightStr, weight) Then
                            ' Format with exactly 3 decimal places
                            weight = Decimal.Round(weight, 3)

                            ' Validate weight range (0.1 kg to 500 kg)
                            If weight >= 0.1D AndAlso weight <= 500D Then
                                Return weight.ToString("F3")
                            Else
                                MessageBox.Show($"الوزن {weight.ToString("F3")} كجم خارج النطاق المسموح به (0.1 كجم إلى 500 كجم)", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                Return String.Empty
                            End If
                        End If
                    End If

                    MessageBox.Show("تم استلام قيمة غير صالحة من الميزان", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return String.Empty
                End If

            Catch ex As TimeoutException
                MessageBox.Show("لم يتم استلام استجابة من الميزان في الوقت المحدد. يرجى المحاولة مرة أخرى.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return String.Empty
            End Try

            Return String.Empty

        Catch ex As Exception
            MessageBox.Show("خطأ في قراءة الوزن: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return String.Empty
        Finally
            If serialPort.IsOpen Then
                serialPort.Close()
            End If
        End Try
    End Function

    Private Function FindRepeatingPattern(input As String) As String
        If String.IsNullOrEmpty(input) OrElse input.Length < 2 Then
            Return input
        End If

        ' First, try to find a decimal number at the start of the string
        Dim match As System.Text.RegularExpressions.Match = System.Text.RegularExpressions.Regex.Match(input, "^\d*\.?\d+")
        If match.Success Then
            Return match.Value
        End If

        ' If no decimal number found, look for repeating patterns
        For length As Integer = 1 To input.Length \ 2
            Dim pattern As String = input.Substring(0, length)
            Dim isRepeating As Boolean = True

            For i As Integer = length To input.Length - 1 Step length
                Dim remainingLength As Integer = Math.Min(length, input.Length - i)
                If input.Substring(i, remainingLength) <> pattern.Substring(0, remainingLength) Then
                    isRepeating = False
                    Exit For
                End If
            Next

            If isRepeating Then
                Return pattern
            End If
        Next

        ' If no pattern found, return first number found
        match = System.Text.RegularExpressions.Regex.Match(input, "\d*\.?\d+")
        If match.Success Then
            Return match.Value
        End If

        Return input
    End Function

    Private Sub mainrawstoreform_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set form to full screen
        Me.WindowState = FormWindowState.Maximized
        Me.FormBorderStyle = FormBorderStyle.Sizable

        ' Load save icon
        Try
            saveIcon = System.Drawing.Image.FromFile("save_icon.png")
        Catch ex As Exception
            ' If icon file not found, create a default icon
            saveIcon = CreateDefaultSaveIcon()
        End Try
        SetupControls()

        ' Force resize event to adjust controls
        Form_Resize(Nothing, Nothing)
        lblusername.Text = "" & LoggedInUsername
        ' Retrieve the public_name for the logged-in user
        Dim publicName As String = GetPublicName(LoggedInUsername)
    End Sub
    Private Function GetPublicName(ByVal username As String) As String
        Dim publicName As String = String.Empty
        Try
            Using connection As New SqlConnection(connectionString)
                ' SQL query to get the public_name from dep_users where username matches
                Dim query As String = "SELECT public_name FROM dep_users WHERE username = @username"
                Dim cmd As New SqlCommand(query, connection)
                cmd.Parameters.AddWithValue("@username", username)

                connection.Open()
                ' Execute the query and retrieve the public_name
                Dim result = cmd.ExecuteScalar()
                If result IsNot Nothing Then
                    publicName = result.ToString()
                End If
                connection.Close()
            End Using
        Catch ex As SqlException
            MessageBox.Show("Error retrieving public name: " & ex.Message)
        End Try
        Return publicName
    End Function
    Private Function CreateDefaultSaveIcon() As Image
        ' Create a simple save icon if the image file is not found
        Dim bmp As New Bitmap(16, 16)
        Using g As Graphics = Graphics.FromImage(bmp)
            g.Clear(Color.Transparent)
            g.DrawRectangle(Pens.Green, 2, 2, 11, 11)
            g.FillRectangle(Brushes.Green, 3, 3, 10, 10)
        End Using
        Return bmp
    End Function

    Private Sub SetupControls()
        ' Setup Username Label
        lblusername.Location = New Point(10, 10) ' Top-left corner
        lblusername.Size = New Size(50, 25)
        lblusername.TextAlign = ContentAlignment.MiddleLeft
        lblusername.Font = New Font("Arial", 8)
        Me.Controls.Add(lblusername)

        ' Calculate center positions
        centerX = Me.ClientSize.Width \ 2

        ' Setup Message Type Label and ComboBox
        lblMessageType.Text = "نوع العملية:"
        lblMessageType.Location = New Point(centerX + 510, 20)
        lblMessageType.Size = New Size(100, 25)
        lblMessageType.TextAlign = ContentAlignment.MiddleRight
        lblMessageType.Font = New Font("Arial", 12, FontStyle.Bold)
        Me.Controls.Add(lblMessageType)

        cmbMessageType.Location = New Point(centerX + 300, 20)
        cmbMessageType.Size = New Size(200, 25)
        cmbMessageType.DropDownStyle = ComboBoxStyle.DropDownList
        cmbMessageType.Items.AddRange({"رسالة جديدة", "استلام على رسالة"})
        cmbMessageType.Font = New Font("Arial", 12)
        Me.Controls.Add(cmbMessageType)

        ' Setup Service Label and ComboBox
        lblService.Text = "نوع الخدمة:"
        lblService.Location = New Point(centerX + 510, 80)
        lblService.Size = New Size(100, 25)
        lblService.TextAlign = ContentAlignment.MiddleRight
        lblService.Font = New Font("Arial", 12, FontStyle.Bold)
        lblService.Visible = False
        Me.Controls.Add(lblService)

        cmbservice.Location = New Point(centerX + 300, 80)
        cmbservice.Size = New Size(200, 25)
        cmbservice.Font = New Font("Arial", 12)
        cmbservice.Visible = False
        Me.Controls.Add(cmbservice)

        ' Setup Client Label and ComboBox
        lblClientCode.Text = "كود العميل:"
        lblClientCode.Location = New Point(centerX + 190, 80)
        lblClientCode.Size = New Size(100, 25)
        lblClientCode.TextAlign = ContentAlignment.MiddleRight
        lblClientCode.Font = New Font("Arial", 12, FontStyle.Bold)
        lblClientCode.Visible = False
        Me.Controls.Add(lblClientCode)

        cmbclient.Location = New Point(centerX - 20, 80)
        cmbclient.Size = New Size(200, 25)
        cmbclient.Font = New Font("Arial", 12)
        cmbclient.Visible = False
        Me.Controls.Add(cmbclient)

        ' Setup Supplier Label and ComboBox
        lblSupplier.Text = "المورد:"
        lblSupplier.Location = New Point(centerX - 130, 80)
        lblSupplier.Size = New Size(100, 25)
        lblSupplier.TextAlign = ContentAlignment.MiddleRight
        lblSupplier.Font = New Font("Arial", 12, FontStyle.Bold)
        lblSupplier.Visible = False
        Me.Controls.Add(lblSupplier)

        cmbsupplier.Location = New Point(centerX - 340, 80)
        cmbsupplier.Size = New Size(200, 25)
        cmbsupplier.Font = New Font("Arial", 12)
        cmbsupplier.Visible = False
        Me.Controls.Add(cmbsupplier)

        ' Setup Style Label and ComboBox
        lblStyleCode.Text = "كود الخامة:"
        lblStyleCode.Location = New Point(centerX + 510, 130)
        lblStyleCode.Size = New Size(100, 25)
        lblStyleCode.TextAlign = ContentAlignment.MiddleRight
        lblStyleCode.Font = New Font("Arial", 12, FontStyle.Bold)
        lblStyleCode.Visible = False
        Me.Controls.Add(lblStyleCode)

        cmbstyle.Location = New Point(centerX + 300, 130)
        cmbstyle.Size = New Size(200, 25)
        cmbstyle.Font = New Font("Arial", 12)
        cmbstyle.Visible = False
        Me.Controls.Add(cmbstyle)

        ' Setup Fabric Label and ComboBox
        lblFabric.Text = "نوع القماش:"
        lblFabric.Location = New Point(centerX + 190, 130)
        lblFabric.Size = New Size(100, 25)
        lblFabric.TextAlign = ContentAlignment.MiddleRight
        lblFabric.Font = New Font("Arial", 12, FontStyle.Bold)
        lblFabric.Visible = False
        Me.Controls.Add(lblFabric)

        cmbkindfabric.Location = New Point(centerX - 20, 130)
        cmbkindfabric.Size = New Size(200, 25)
        cmbkindfabric.Font = New Font("Arial", 12)
        cmbkindfabric.Visible = False
        Me.Controls.Add(cmbkindfabric)

        ' Setup PO Label and TextBox
        lblPONumber.Text = "رقم PO:"
        lblPONumber.Location = New Point(centerX + 510, 180)
        lblPONumber.Size = New Size(100, 25)
        lblPONumber.TextAlign = ContentAlignment.MiddleRight
        lblPONumber.Font = New Font("Arial", 12, FontStyle.Bold)
        lblPONumber.Visible = False
        Me.Controls.Add(lblPONumber)

        txtpo.Location = New Point(centerX + 300, 180)
        txtpo.Size = New Size(200, 25)
        txtpo.Font = New Font("Arial", 12)
        txtpo.Visible = False
        Me.Controls.Add(txtpo)

        ' Setup Material Label and TextBox
        lblMaterial.Text = "الخامة:"
        lblMaterial.Location = New Point(centerX + 190, 180)
        lblMaterial.Size = New Size(100, 25)
        lblMaterial.TextAlign = ContentAlignment.MiddleRight
        lblMaterial.Font = New Font("Arial", 12, FontStyle.Bold)
        lblMaterial.Visible = False
        Me.Controls.Add(lblMaterial)

        txtmaterial.Location = New Point(centerX - 20, 180)
        txtmaterial.Size = New Size(200, 25)
        txtmaterial.Font = New Font("Arial", 12)
        txtmaterial.Visible = False
        Me.Controls.Add(txtmaterial)

        ' Setup PO Label
        lblPO.Text = "رقم أمر الشراء:"
        lblPO.Location = New Point(centerX + 190, 20)
        lblPO.Size = New Size(100, 25)
        lblPO.TextAlign = ContentAlignment.MiddleRight
        lblPO.Visible = False
        Me.Controls.Add(lblPO)

        ' Setup PO ComboBox
        cmbPO.Location = New Point(centerX + 10, 20)
        cmbPO.Size = New Size(180, 25)
        cmbPO.DropDownStyle = ComboBoxStyle.DropDown
        cmbPO.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cmbPO.AutoCompleteSource = AutoCompleteSource.CustomSource
        cmbPO.Visible = False
        Me.Controls.Add(cmbPO)

        ' Setup Batch Label
        lblBatch.Text = "رقم الرسالة:"
        lblBatch.Location = New Point(centerX - 90, 20)
        lblBatch.Size = New Size(100, 25)
        lblBatch.TextAlign = ContentAlignment.MiddleRight
        lblBatch.Visible = False
        Me.Controls.Add(lblBatch)

        ' Setup Batch ComboBox
        cmbBatch.Location = New Point(centerX - 270, 20)
        cmbBatch.Size = New Size(180, 25)
        cmbBatch.DropDownStyle = ComboBoxStyle.DropDown
        cmbBatch.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cmbBatch.AutoCompleteSource = AutoCompleteSource.CustomSource
        cmbBatch.Visible = False
        Me.Controls.Add(cmbBatch)

        ' Setup Lot Action Label
        lblLotAction.Text = "اختيار اللوت:"
        lblLotAction.Location = New Point(centerX - 390, 20)
        lblLotAction.Size = New Size(100, 25)
        lblLotAction.TextAlign = ContentAlignment.MiddleRight
        lblLotAction.Visible = False
        Me.Controls.Add(lblLotAction)

        ' Setup Lot Action ComboBox
        cmbLotAction.Location = New Point(centerX - 670, 20)
        cmbLotAction.Size = New Size(280, 25)
        cmbLotAction.DropDownStyle = ComboBoxStyle.DropDownList
        cmbLotAction.Items.AddRange({"اختيار من لوت موجود", "اضافة لوت جديد"})
        cmbLotAction.Visible = False
        Me.Controls.Add(cmbLotAction)

        ' Setup Batch Info DataGridView
        dgvBatchInfo.Location = New Point(20, 60)
        dgvBatchInfo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvBatchInfo.RightToLeft = RightToLeft.Yes
        dgvBatchInfo.Visible = False
        dgvBatchInfo.AllowUserToAddRows = False
        dgvBatchInfo.ReadOnly = True
        dgvBatchInfo.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvBatchInfo.MultiSelect = False
        dgvBatchInfo.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        dgvBatchInfo.DefaultCellStyle.Font = New Font("Arial", 12, FontStyle.Regular)
        dgvBatchInfo.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 12, FontStyle.Bold)
        Me.Controls.Add(dgvBatchInfo)

        ' Setup DataGridView
        dgvBatchDetails.Location = New Point(20, 280)
        dgvBatchDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvBatchDetails.RightToLeft = RightToLeft.Yes
        dgvBatchDetails.Visible = False
        dgvBatchDetails.AllowUserToAddRows = False
        dgvBatchDetails.ReadOnly = False
        dgvBatchDetails.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvBatchDetails.MultiSelect = False
        dgvBatchDetails.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        dgvBatchDetails.DefaultCellStyle.Font = New Font("Arial", 14, FontStyle.Regular)
        dgvBatchDetails.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 14, FontStyle.Bold)
        Me.Controls.Add(dgvBatchDetails)

        ' Setup Rolls DataGridView
        dgvRolls.Location = New Point(20, 450)
        dgvRolls.Size = New Size(Me.ClientSize.Width / 2 - 30, Me.ClientSize.Height - 500)
        dgvRolls.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left
        dgvRolls.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvRolls.RightToLeft = RightToLeft.Yes
        dgvRolls.Visible = False
        dgvRolls.AllowUserToAddRows = False
        dgvRolls.ReadOnly = False ' Change to False to allow manual entry
        dgvRolls.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvRolls.MultiSelect = False
        dgvRolls.DefaultCellStyle.Font = New Font("Arial", 14, FontStyle.Regular)
        dgvRolls.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 14, FontStyle.Bold)

        ' Add columns to Rolls DataGridView
        dgvRolls.Columns.Add("RollNumber", "رقم التوب")
        dgvRolls.Columns.Add("Weight", "الوزن")

        ' Add Print button column
        Dim printButtonColumn As New DataGridViewButtonColumn()
        printButtonColumn.Name = "PrintButton"
        printButtonColumn.HeaderText = "طباعة"
        printButtonColumn.Text = "طباعة"
        printButtonColumn.UseColumnTextForButtonValue = True
        dgvRolls.Columns.Add(printButtonColumn)

        ' Make RollNumber column readonly but Weight column editable
        dgvRolls.Columns("RollNumber").ReadOnly = True
        dgvRolls.Columns("Weight").ReadOnly = False

        Me.Controls.Add(dgvRolls)

        ' Initialize serial port
        InitializeSerialPort()

        ' Setup Save Rolls button with updated properties
        btnSaveRolls.Text = "حفظ الأتواب"
        btnSaveRolls.Size = New Size(200, 35)
        btnSaveRolls.Font = New Font("Arial", 12, FontStyle.Bold)
        btnSaveRolls.BackColor = Color.FromArgb(0, 120, 215)
        btnSaveRolls.ForeColor = Color.White
        btnSaveRolls.FlatStyle = FlatStyle.Flat
        btnSaveRolls.RightToLeft = RightToLeft.Yes
        btnSaveRolls.Visible = False ' Initially hidden
        Me.Controls.Add(btnSaveRolls)

        ' Setup Read Weight button with updated properties
        btnReadWeight.Text = "قراءة الوزن"
        btnReadWeight.Size = New Size(120, 35)
        btnReadWeight.Font = New Font("Arial", 12, FontStyle.Bold)
        btnReadWeight.BackColor = Color.FromArgb(0, 120, 215)
        btnReadWeight.ForeColor = Color.White
        btnReadWeight.FlatStyle = FlatStyle.Flat
        btnReadWeight.RightToLeft = RightToLeft.Yes
        btnReadWeight.Visible = False ' Initially hidden
        Me.Controls.Add(btnReadWeight)

        ' Add resize event handler
        AddHandler Me.Resize, AddressOf Form_Resize

        ' Setup Batch Number Label with prominent styling and center position
        lblNewBatchNumber.Location = New Point(centerX - 150, 20)
        lblNewBatchNumber.Size = New Size(300, 40)
        lblNewBatchNumber.Font = New Font("Arial", 20, FontStyle.Bold)
        lblNewBatchNumber.TextAlign = ContentAlignment.MiddleCenter
        lblNewBatchNumber.BackColor = Color.FromArgb(240, 240, 240)
        lblNewBatchNumber.BorderStyle = BorderStyle.FixedSingle
        lblNewBatchNumber.Visible = False
        Me.Controls.Add(lblNewBatchNumber)

        ' Setup Insert Button
        btninsert.Text = "تسجيل البيانات"
        btninsert.Location = New Point(centerX - 100, 230)
        btninsert.Size = New Size(200, 35)
        btninsert.Visible = False
        btninsert.BackColor = Color.FromArgb(0, 120, 215)
        btninsert.ForeColor = Color.White
        btninsert.FlatStyle = FlatStyle.Flat
        btninsert.Font = New Font("Arial", 12, FontStyle.Bold)
        Me.Controls.Add(btninsert)

        ' Setup weight distribution controls
        lblTotalWeight.Text = "الوزن الكلي:"
        lblTotalWeight.Location = New Point(dgvRolls.Right + 100, dgvRolls.Top + 150)
        lblTotalWeight.Size = New Size(100, 30)
        lblTotalWeight.Font = New Font("Arial", 12, FontStyle.Bold)
        lblTotalWeight.RightToLeft = RightToLeft.Yes
        lblTotalWeight.Visible = False ' Set initial visibility to False
        Me.Controls.Add(lblTotalWeight)

        txtTotalWeight.Location = New Point(dgvRolls.Right + 100, lblTotalWeight.Bottom + 5)
        txtTotalWeight.Size = New Size(150, 30)
        txtTotalWeight.Font = New Font("Arial", 12)
        txtTotalWeight.RightToLeft = RightToLeft.Yes
        txtTotalWeight.Visible = False ' Set initial visibility to False
        Me.Controls.Add(txtTotalWeight)

        lblNumberOfRolls.Text = "عدد الأتواب:"
        lblNumberOfRolls.Location = New Point(dgvRolls.Right + 100, txtTotalWeight.Bottom + 10)
        lblNumberOfRolls.Size = New Size(100, 30)
        lblNumberOfRolls.Font = New Font("Arial", 12, FontStyle.Bold)
        lblNumberOfRolls.RightToLeft = RightToLeft.Yes
        lblNumberOfRolls.Visible = False ' Set initial visibility to False
        Me.Controls.Add(lblNumberOfRolls)

        txtNumberOfRolls.Location = New Point(dgvRolls.Right + 100, lblNumberOfRolls.Bottom + 5)
        txtNumberOfRolls.Size = New Size(150, 30)
        txtNumberOfRolls.Font = New Font("Arial", 12)
        txtNumberOfRolls.RightToLeft = RightToLeft.Yes
        txtNumberOfRolls.Visible = False ' Set initial visibility to False
        Me.Controls.Add(txtNumberOfRolls)

        btnDivideWeight.Text = "تقسيم الوزن"
        btnDivideWeight.Location = New Point(dgvRolls.Right + 100, txtNumberOfRolls.Bottom + 10)
        btnDivideWeight.Size = New Size(150, 35)
        btnDivideWeight.Font = New Font("Arial", 12, FontStyle.Bold)
        btnDivideWeight.BackColor = Color.FromArgb(0, 120, 215)
        btnDivideWeight.ForeColor = Color.White
        btnDivideWeight.FlatStyle = FlatStyle.Flat
        btnDivideWeight.RightToLeft = RightToLeft.Yes
        btnDivideWeight.Visible = False ' Set initial visibility to False
        AddHandler btnDivideWeight.Click, AddressOf btnDivideWeight_Click
        Me.Controls.Add(btnDivideWeight)
    End Sub

    Private Sub UpdateBatchDetailsHeight()
        ' Calculate total height needed for rows plus header
        Dim totalHeight As Integer = dgvBatchDetails.ColumnHeadersHeight
        For Each row As DataGridViewRow In dgvBatchDetails.Rows
            totalHeight += row.Height
        Next

        ' Add a small padding
        totalHeight += 5

        ' Set a maximum height (مثلاً نصف ارتفاع الفورم)
        Dim maxHeight As Integer = Me.ClientSize.Height \ 4
        If totalHeight > maxHeight Then
            dgvBatchDetails.Height = maxHeight
            dgvBatchDetails.ScrollBars = ScrollBars.Vertical
        Else
            dgvBatchDetails.Height = totalHeight
            dgvBatchDetails.ScrollBars = ScrollBars.None
        End If

        ' Update rolls grid position if visible
        If dgvRolls.Visible Then
            dgvRolls.Location = New Point(20, dgvBatchDetails.Bottom + 10)
            dgvRolls.Height = Me.ClientSize.Height - dgvRolls.Top - 20
        End If
    End Sub

    Private Function HasUnsavedWeights() As Boolean
        If dgvRolls.Visible Then
            For Each row As DataGridViewRow In dgvRolls.Rows
                If row.Cells("Weight").Value IsNot Nothing AndAlso Not String.IsNullOrEmpty(row.Cells("Weight").Value.ToString()) Then
                    Return True
                End If
            Next
        End If
        Return False
    End Function

    Private Sub HideRollsGrid()
        dgvRolls.Rows.Clear()
        dgvRolls.Visible = False
        btnSaveRolls.Visible = False
        btnReadWeight.Visible = False
    End Sub

    Private Sub cmbMessageType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbMessageType.SelectedIndexChanged
        If HasUnsavedWeights() Then
            MessageBox.Show("الرجاء حفظ الأوزان المدخلة أولاً قبل تغيير الاختيار.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            ' Restore previous selection
            RemoveHandler cmbMessageType.SelectedIndexChanged, AddressOf cmbMessageType_SelectedIndexChanged
            cmbMessageType.SelectedIndex = cmbMessageType.Tag
            AddHandler cmbMessageType.SelectedIndexChanged, AddressOf cmbMessageType_SelectedIndexChanged
            Return
        End If

        ' Store current selection for potential rollback
        cmbMessageType.Tag = cmbMessageType.SelectedIndex

        HideRollsGrid()
        If cmbMessageType.SelectedItem.ToString() = "رسالة جديدة" Then
            ' Hide existing message controls
            lblPO.Visible = False
            cmbPO.Visible = False
            lblBatch.Visible = False
            cmbBatch.Visible = False
            dgvBatchDetails.Visible = False
            lblBatchNumber.Visible = False
            dgvBatchInfo.Visible = False
            cmbLotAction.Visible = False
            lblLotAction.Visible = False

            ' Show all labels and controls for new batch
            lblService.Visible = True
            lblClientCode.Visible = True
            lblSupplier.Visible = True
            lblStyleCode.Visible = True
            lblFabric.Visible = True
            lblPONumber.Visible = True
            lblMaterial.Visible = True
            lblNewBatchNumber.Visible = True

            ' Show and enable all new batch controls
            cmbservice.Visible = True
            cmbservice.Enabled = True

            cmbclient.Visible = True
            cmbclient.Enabled = True
            lblclient.Visible = True

            cmbsupplier.Visible = True
            cmbsupplier.Enabled = True
            lblsup.Visible = True

            cmbstyle.Visible = True
            cmbstyle.Enabled = True
            lblstyle.Visible = True

            cmbkindfabric.Visible = True
            cmbkindfabric.Enabled = True

            txtpo.Visible = True
            txtpo.Enabled = True
            txtmaterial.Visible = True
            txtmaterial.Enabled = True

            btninsert.Visible = True
            btninsert.Enabled = True

            ' Load data for ComboBoxes
            LoadClientCodes()
            LoadSuppliersCodes()
            Loadservice()
            Loadstyle()
            LoadFabricTypes()

            ' Set batch number
            lblNewBatchNumber.Text = GetNextBatchNumber()

        Else
            ' Hide new batch controls and labels
            lblService.Visible = False
            lblClientCode.Visible = False
            lblSupplier.Visible = False
            lblStyleCode.Visible = False
            lblFabric.Visible = False
            lblPONumber.Visible = False
            lblMaterial.Visible = False
            lblNewBatchNumber.Visible = False

            cmbservice.Visible = False
            cmbclient.Visible = False
            cmbsupplier.Visible = False
            cmbstyle.Visible = False
            cmbkindfabric.Visible = False
            txtpo.Visible = False
            txtmaterial.Visible = False
            btninsert.Visible = False
            lblclient.Visible = False
            lblsup.Visible = False
            lblstyle.Visible = False

            ' Show existing message controls
            lblBatchNumber.Visible = False
            lblPO.Visible = True
            cmbPO.Visible = True
            lblBatch.Visible = True
            cmbBatch.Visible = True
            LoadComboBoxes()
        End If
    End Sub

    Private Sub ShowNewBatchControls(show As Boolean)
        ' Show/hide all new batch controls and their labels
        For Each ctrl As Control In Me.Controls
            Select Case ctrl.Name
                ' ComboBoxes
                Case "cmbservice", "cmbstyle", "cmbclient", "cmbsupplier", "cmbkindfabric"
                    ctrl.Visible = show
                    ctrl.Enabled = show
                ' TextBoxes
                Case "txtpo", "txtmaterial"
                    ctrl.Visible = show
                    ctrl.Enabled = show
                ' Labels
                Case "lblclient", "lblsup", "lblstyle", "lblNewBatchNumber"
                    ctrl.Visible = show
                ' Button
                Case "btninsert"
                    ctrl.Visible = show
                    ctrl.Enabled = show
                ' Labels for ComboBoxes
                Case "lblService", "lblClientCode", "lblSupplier", "lblStyleCode", "lblFabric", "lblPONumber", "lblMaterial"
                    ctrl.Visible = show
            End Select
        Next

        ' Reset controls if hiding
        If Not show Then
            cmbservice.SelectedIndex = -1
            cmbstyle.SelectedIndex = -1
            cmbclient.SelectedIndex = -1
            cmbsupplier.SelectedIndex = -1
            cmbkindfabric.SelectedIndex = -1
            txtpo.Text = ""
            txtmaterial.Text = ""
            lblclient.Text = ""
            lblsup.Text = ""
            lblstyle.Text = ""
            lblNewBatchNumber.Text = ""
        End If
    End Sub

    Private Function GetNextBatchNumber() As String
        Dim nextBatchNumber As String = String.Empty
        Try
            Using conn As New SqlConnection(connectionString)
                Dim query As String = "SELECT TOP 1 batch FROM batch_raw ORDER BY batch DESC"
                Dim cmd As New SqlCommand(query, conn)

                ' Open the connection
                conn.Open()

                ' Execute the query and get the result
                Dim result = cmd.ExecuteScalar()
                If result IsNot Nothing Then
                    nextBatchNumber = (Convert.ToInt32(result) + 1).ToString()
                Else
                    nextBatchNumber = "1" ' If no batches exist, start with 1
                End If

                conn.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error fetching next batch number: " & ex.Message)
        End Try

        Return nextBatchNumber
    End Function

    Private Sub LoadComboBoxes()
        cmbPO.Items.Clear()
        cmbBatch.Items.Clear()
        cmbPO.AutoCompleteCustomSource.Clear()

        Using conn As New SqlConnection(connectionString)
            conn.Open()

            ' Load cmbPO
            Using cmd As New SqlCommand("SELECT DISTINCT po_number FROM batch_raw", conn)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim poNumber As String = reader("po_number").ToString()
                        cmbPO.Items.Add(poNumber)
                        cmbPO.AutoCompleteCustomSource.Add(poNumber)
                    End While
                End Using
            End Using
        End Using
    End Sub

    Private Sub cmbPO_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPO.SelectedIndexChanged
        If HasUnsavedWeights() Then
            MessageBox.Show("الرجاء حفظ الأوزان المدخلة أولاً قبل تغيير الاختيار.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            ' Restore previous selection
            RemoveHandler cmbPO.SelectedIndexChanged, AddressOf cmbPO_SelectedIndexChanged
            cmbPO.SelectedIndex = cmbPO.Tag
            AddHandler cmbPO.SelectedIndexChanged, AddressOf cmbPO_SelectedIndexChanged
            Return
        End If

        ' Store current selection for potential rollback
        cmbPO.Tag = cmbPO.SelectedIndex

        HideRollsGrid()
        ' Clear batch ComboBox and hide DataGridView when PO changes
        cmbBatch.Items.Clear()
        cmbBatch.Text = "" ' إضافة هذا السطر لمسح النص المعروض
        cmbBatch.SelectedIndex = -1
        dgvBatchDetails.Visible = False
        dgvBatchInfo.Visible = False ' إخفاء جدول معلومات الرسالة أيضاً

        ' Reset lot action selection
        cmbLotAction.SelectedIndex = -1
        cmbLotAction.Visible = False
        lblLotAction.Visible = False

        If cmbPO.SelectedItem IsNot Nothing Then
            LoadBatchComboBox(cmbPO.SelectedItem.ToString())
        End If
    End Sub

    Private Sub cmbBatch_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbBatch.SelectedIndexChanged
        If HasUnsavedWeights() Then
            MessageBox.Show("الرجاء حفظ الأوزان المدخلة أولاً قبل تغيير الاختيار.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            ' Restore previous selection
            RemoveHandler cmbBatch.SelectedIndexChanged, AddressOf cmbBatch_SelectedIndexChanged
            cmbBatch.SelectedIndex = cmbBatch.Tag
            AddHandler cmbBatch.SelectedIndexChanged, AddressOf cmbBatch_SelectedIndexChanged
            Return
        End If

        ' Store current selection for potential rollback
        cmbBatch.Tag = cmbBatch.SelectedIndex

        HideRollsGrid()
        If cmbBatch.SelectedItem IsNot Nothing AndAlso cmbPO.SelectedItem IsNot Nothing Then
            ' Reset lot action selection
            cmbLotAction.SelectedIndex = -1
            hasNewRow = False

            ' Load batch info first
            LoadBatchInfo(cmbBatch.SelectedItem.ToString())

            ' Then load batch details
            LoadBatchDetails(cmbPO.SelectedItem.ToString(), cmbBatch.SelectedItem.ToString())
        End If
    End Sub

    Private Sub LoadBatchDetails(poNumber As String, batchNumber As String)
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()

                Dim query As String = "SELECT CONVERT(DATE, bd.datetrans) AS 'تاريخ الاستلام', " &
                                    "bd.batch_id AS 'رقم الرسالة', " &
                                    "bd.lot AS 'lot', " &
                                    "bd.client_permission AS 'اذن العميل', " &
                                    "bd.client_item_code AS 'كود صنف العميل', " &
                                    "bd.weightpk AS 'كميه وزن', " &
                                    "bd.meter_quantity AS 'كميه متر', " &
                                    "bd.rollpk AS 'عدد اتواب', " &
                                    "bd.store_permission as 'اذن اضافه المخزن' " &
                                    "FROM batch_details bd " &
                                    "LEFT JOIN batch_raw br ON bd.batch_id = br.batch " &
                                    "WHERE br.po_number = @poNumber AND br.batch = @batchNumber"

                Using adapter As New SqlDataAdapter(query, conn)
                    adapter.SelectCommand.Parameters.AddWithValue("@poNumber", poNumber)
                    adapter.SelectCommand.Parameters.AddWithValue("@batchNumber", batchNumber)

                    Dim dt As New DataTable()
                    adapter.Fill(dt)

                    dgvBatchDetails.DataSource = dt
                    dgvBatchDetails.Visible = True
                    dgvBatchDetails.ReadOnly = False ' Make the grid editable by default

                    ' Add checkbox column if it doesn't exist
                    If Not dgvBatchDetails.Columns.Contains("SelectRow") Then
                        Dim checkBoxColumn As New DataGridViewCheckBoxColumn()
                        checkBoxColumn.Name = "SelectRow"
                        checkBoxColumn.HeaderText = "اختيار"
                        checkBoxColumn.Width = 50
                        checkBoxColumn.ReadOnly = False
                        dgvBatchDetails.Columns.Insert(0, checkBoxColumn)
                    End If

                    ' Add save button and add rolls button columns if they don't exist
                    If Not dgvBatchDetails.Columns.Contains("SaveButton") Then
                        Dim saveButtonColumn As New DataGridViewImageColumn()
                        saveButtonColumn.Name = "SaveButton"
                        saveButtonColumn.HeaderText = ""
                        saveButtonColumn.Image = saveIcon
                        saveButtonColumn.Width = 30
                        dgvBatchDetails.Columns.Add(saveButtonColumn)
                    End If

                    If Not dgvBatchDetails.Columns.Contains("AddRollsButton") Then
                        Dim addRollsButtonColumn As New DataGridViewImageColumn()
                        addRollsButtonColumn.Name = "AddRollsButton"
                        addRollsButtonColumn.HeaderText = ""
                        addRollsButtonColumn.Image = CreateAddRollsIcon()
                        addRollsButtonColumn.Width = 30
                        dgvBatchDetails.Columns.Add(addRollsButtonColumn)
                    End If

                    ' Set column read-only properties
                    For Each column As DataGridViewColumn In dgvBatchDetails.Columns
                        Select Case column.Name
                            Case "SelectRow"
                                column.ReadOnly = False
                            Case "SaveButton", "AddRollsButton"
                                column.ReadOnly = True
                            Case "تاريخ الاستلام", "رقم الرسالة", "lot", "كميه وزن", "عدد اتواب"
                                column.ReadOnly = True
                            Case "اذن العميل", "كود صنف العميل", "كميه متر", "اذن اضافه المخزن"
                                column.ReadOnly = False
                        End Select
                    Next

                    ' Show lot action controls
                    lblLotAction.Visible = True
                    cmbLotAction.Visible = True

                    ' Set initial button visibility
                    UpdateButtonsVisibility()

                    ' Update grid height
                    UpdateBatchDetailsHeight()
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading batch details: " & ex.Message)
        End Try
    End Sub

    Private Function CreateAddRollsIcon() As Image
        ' Create a simple add rolls icon
        Dim bmp As New Bitmap(16, 16)
        Using g As Graphics = Graphics.FromImage(bmp)
            g.Clear(Color.Transparent)
            g.DrawRectangle(Pens.Blue, 2, 2, 11, 11)
            g.DrawLine(Pens.Blue, 7, 4, 7, 11)  ' Vertical line
            g.DrawLine(Pens.Blue, 4, 7, 11, 7)  ' Horizontal line
        End Using
        Return bmp
    End Function

    Private Sub UpdateButtonsVisibility()
        For Each row As DataGridViewRow In dgvBatchDetails.Rows
            ' Show buttons only for selected row in existing records mode
            ' or for the new row in add mode
            Dim showButtons As Boolean = False

            If cmbLotAction.SelectedItem IsNot Nothing Then
                If cmbLotAction.SelectedItem.ToString() = "اختيار من لوت موجود" Then
                    showButtons = CBool(row.Cells("SelectRow").Value)
                Else
                    showButtons = hasNewRow AndAlso row.Index = dgvBatchDetails.Rows.Count - 1
                End If
            End If

            row.Cells("SaveButton").Value = If(showButtons, saveIcon, Nothing)
            row.Cells("AddRollsButton").Value = If(showButtons, CreateAddRollsIcon(), Nothing)
        Next

        ' Ensure Add Rolls button is disabled for new rows until saved
        If hasNewRow Then
            btnAddRolls.Enabled = False
        End If
    End Sub

    Private Sub LoadBatchComboBox(poNumber As String)
        cmbBatch.Items.Clear()
        cmbBatch.AutoCompleteCustomSource.Clear()

        Using conn As New SqlConnection(connectionString)
            conn.Open()

            ' Load cmbBatch based on selected po_number
            Using cmd As New SqlCommand("SELECT DISTINCT batch FROM batch_raw WHERE po_number = @poNumber", conn)
                cmd.Parameters.AddWithValue("@poNumber", poNumber)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim batchNumber As String = reader("batch").ToString()
                        cmbBatch.Items.Add(batchNumber)
                        cmbBatch.AutoCompleteCustomSource.Add(batchNumber)
                    End While
                End Using
            End Using
        End Using
    End Sub

    Private Sub cmbLotAction_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbLotAction.SelectedIndexChanged
        If HasUnsavedWeights() Then
            MessageBox.Show("الرجاء حفظ الأوزان المدخلة أولاً قبل تغيير الاختيار.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            ' Restore previous selection
            RemoveHandler cmbLotAction.SelectedIndexChanged, AddressOf cmbLotAction_SelectedIndexChanged
            cmbLotAction.SelectedIndex = cmbLotAction.Tag
            AddHandler cmbLotAction.SelectedIndexChanged, AddressOf cmbLotAction_SelectedIndexChanged
            Return
        End If

        ' Store current selection for potential rollback
        cmbLotAction.Tag = cmbLotAction.SelectedIndex

        HideRollsGrid()
        If cmbLotAction.SelectedItem IsNot Nothing Then
            If cmbLotAction.SelectedItem.ToString() = "اختيار من لوت موجود" Then
                ' Remove the new row if it exists
                If hasNewRow Then
                    Dim dt As DataTable = DirectCast(dgvBatchDetails.DataSource, DataTable)
                    dt.Rows.RemoveAt(dt.Rows.Count - 1)
                    dgvBatchDetails.DataSource = dt
                    hasNewRow = False
                End If

                ' Enable checkbox column for selection
                dgvBatchDetails.ReadOnly = False
                For Each col As DataGridViewColumn In dgvBatchDetails.Columns
                    If col.Name = "SelectRow" Then
                        col.ReadOnly = False
                    End If
                Next

                ' Enable Add Rolls button
                btnAddRolls.Enabled = True

                ' Clear all selections
                For Each row As DataGridViewRow In dgvBatchDetails.Rows
                    row.Cells("SelectRow").Value = False
                Next

            Else ' اضافة لوت جديد
                ' Clear any existing selections first
                For Each row As DataGridViewRow In dgvBatchDetails.Rows
                    row.Cells("SelectRow").Value = False
                Next

                ' Only add new row if we don't already have one
                If Not hasNewRow Then
                    AddNewLotRow()
                End If

                ' Disable Add Rolls button until the new lot is saved
                btnAddRolls.Enabled = False
            End If

            ' Update buttons visibility after changes
            UpdateButtonsVisibility()
        End If
    End Sub

    Private Sub AddNewLotRow()
        Try
            ' Get all lot numbers for this batch
            Dim lastLot As String = ""
            Dim newLot As String = ""
            Dim maxNumber As Integer = 0

            Using conn As New SqlConnection(connectionString)
                conn.Open()
                ' Get all lots for this batch to analyze the pattern
                Dim query As String = "SELECT lot FROM batch_details WHERE batch_id = @batchId"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@batchId", cmbBatch.SelectedItem.ToString())
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim currentLot As String = reader("lot").ToString()
                            ' Split by either \ or /
                            Dim parts = currentLot.Split({"\"c, "/"c}, StringSplitOptions.RemoveEmptyEntries)

                            If parts.Length > 1 Then
                                ' If we have a base number with suffix
                                Dim baseNumber As String = parts(0)
                                Dim suffixNumber As Integer
                                If Integer.TryParse(parts(1), suffixNumber) Then
                                    If suffixNumber > maxNumber Then
                                        maxNumber = suffixNumber
                                        lastLot = currentLot
                                    End If
                                End If
                            ElseIf parts.Length = 1 Then
                                ' If we just have a base number without suffix
                                lastLot = parts(0)
                                maxNumber = 0
                            End If
                        End While
                    End Using
                End Using
            End Using

            ' Determine the new lot number
            If String.IsNullOrEmpty(lastLot) Then
                ' If no lots exist, start with \1
                newLot = "1/1"
            Else
                ' Split the last lot by either \ or /
                Dim parts = lastLot.Split({"\"c, "/"c}, StringSplitOptions.RemoveEmptyEntries)
                If parts.Length > 1 Then
                    ' If we have a base number with suffix
                    newLot = parts(0) & "/" & (maxNumber + 1).ToString()
                Else
                    ' If we just have a base number without suffix
                    newLot = lastLot & "/1"
                End If
            End If

            ' Create new DataTable with same schema
            Dim dt As DataTable = DirectCast(dgvBatchDetails.DataSource, DataTable)
            Dim newRow As DataRow = dt.NewRow()

            ' Set default values
            newRow("تاريخ الاستلام") = DateTime.Today
            newRow("رقم الرسالة") = cmbBatch.SelectedItem.ToString()
            newRow("lot") = newLot
            newRow("كميه وزن") = 0
            newRow("عدد اتواب") = 0

            dt.Rows.Add(newRow)
            dgvBatchDetails.DataSource = dt

            ' First, make all cells readonly
            dgvBatchDetails.ReadOnly = False
            For Each row As DataGridViewRow In dgvBatchDetails.Rows
                For Each cell As DataGridViewCell In row.Cells
                    cell.ReadOnly = False
                Next
            Next

            ' Then, make specific cells in the new row editable
            Dim newRowIndex As Integer = dgvBatchDetails.Rows.Count - 1
            For Each column As DataGridViewColumn In dgvBatchDetails.Columns
                Select Case column.Name
                    Case "SelectRow"
                        dgvBatchDetails.Rows(newRowIndex).Cells(column.Name).Value = True
                        dgvBatchDetails.Rows(newRowIndex).Cells(column.Name).ReadOnly = True
                    Case "تاريخ الاستلام", "رقم الرسالة", "lot", "كميه وزن", "عدد اتواب"
                        dgvBatchDetails.Rows(newRowIndex).Cells(column.Name).ReadOnly = True
                    Case Else
                        dgvBatchDetails.Rows(newRowIndex).Cells(column.Name).ReadOnly = False
                End Select
            Next

            ' Scroll to the new row
            dgvBatchDetails.FirstDisplayedScrollingRowIndex = newRowIndex
            dgvBatchDetails.CurrentCell = dgvBatchDetails.Rows(newRowIndex).Cells(0)

            hasNewRow = True
        Catch ex As Exception
            MessageBox.Show("Error adding new lot: " & ex.Message)
        End Try
    End Sub

    Private Sub dgvBatchDetails_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvBatchDetails.CellContentClick
        ' Handle checkbox column clicks
        If e.ColumnIndex = dgvBatchDetails.Columns("SelectRow").Index AndAlso e.RowIndex >= 0 AndAlso cmbLotAction.SelectedItem?.ToString() = "اختيار من لوت موجود" Then
            ' Hide rolls grid and save button first
            dgvRolls.Visible = False
            btnSaveRolls.Visible = False
            btnReadWeight.Visible = False

            ' Toggle the clicked checkbox
            Dim isChecked = Not CBool(dgvBatchDetails.Rows(e.RowIndex).Cells("SelectRow").Value)
            dgvBatchDetails.Rows(e.RowIndex).Cells("SelectRow").Value = isChecked

            ' If this row is being checked, uncheck all others
            If isChecked Then
                For Each row As DataGridViewRow In dgvBatchDetails.Rows
                    If row.Index <> e.RowIndex Then
                        row.Cells("SelectRow").Value = False
                    End If
                Next
            End If

            ' Update buttons visibility
            UpdateButtonsVisibility()

        ElseIf e.ColumnIndex = dgvBatchDetails.Columns("SaveButton").Index AndAlso e.RowIndex >= 0 Then
            ' Handle save button click
            Try
                Using conn As New SqlConnection(connectionString)
                    conn.Open()
                    Dim row As DataGridViewRow = dgvBatchDetails.Rows(e.RowIndex)
                    Dim lotValue As String = row.Cells("lot").Value.ToString()

                    ' Check if required fields are filled
                    If String.IsNullOrEmpty(row.Cells("اذن العميل").Value?.ToString()) OrElse
                       String.IsNullOrEmpty(row.Cells("كود صنف العميل").Value?.ToString()) Then
                        MessageBox.Show("الرجاء إدخال اذن العميل وكود صنف العميل", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If

                    ' Check for existing record
                    Dim checkQuery As String = "SELECT COUNT(*) FROM batch_details WHERE batch_id = @batchId AND lot = @lot"
                    Using checkCmd As New SqlCommand(checkQuery, conn)
                        checkCmd.Parameters.AddWithValue("@batchId", row.Cells("رقم الرسالة").Value)
                        checkCmd.Parameters.AddWithValue("@lot", lotValue)

                        Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())
                        If count > 0 Then
                            ' Update existing record
                            Dim updateQuery As String = "UPDATE batch_details SET " &
                                                      "client_permission = @clientPermission, " &
                                                      "client_item_code = @clientItemCode, " &
                                                      "meter_quantity = @meterQuantity, " &
                                                      "store_permission = @storePermission " &
                                                      "WHERE batch_id = @batchId AND lot = @lot"

                            Using cmd As New SqlCommand(updateQuery, conn)
                                cmd.Parameters.AddWithValue("@batchId", row.Cells("رقم الرسالة").Value)
                                cmd.Parameters.AddWithValue("@lot", lotValue)
                                cmd.Parameters.AddWithValue("@clientPermission", If(row.Cells("اذن العميل").Value Is Nothing, DBNull.Value, row.Cells("اذن العميل").Value))
                                cmd.Parameters.AddWithValue("@clientItemCode", If(row.Cells("كود صنف العميل").Value Is Nothing, DBNull.Value, row.Cells("كود صنف العميل").Value))
                                cmd.Parameters.AddWithValue("@meterQuantity", If(row.Cells("كميه متر").Value Is Nothing OrElse String.IsNullOrEmpty(row.Cells("كميه متر").Value.ToString()), DBNull.Value, row.Cells("كميه متر").Value))
                                cmd.Parameters.AddWithValue("@storePermission", If(row.Cells("اذن اضافه المخزن").Value Is Nothing, DBNull.Value, row.Cells("اذن اضافه المخزن").Value))

                                cmd.ExecuteNonQuery()
                            End Using
                        Else
                            ' Insert new record
                            Dim insertQuery As String = "INSERT INTO batch_details (datetrans, batch_id, lot, client_permission, client_item_code, weightpk, meter_quantity, rollpk, store_permission, rolls_count, weight_quantity, username) " &
                                                      "VALUES (@datetrans, @batchId, @lot, @clientPermission, @clientItemCode, @weightpk, @meterQuantity, @rollpk, @storePermission, @rolls_count, @weight_quantity, @username)"

                            Using cmd As New SqlCommand(insertQuery, conn)
                                cmd.Parameters.AddWithValue("@datetrans", Convert.ToDateTime(row.Cells("تاريخ الاستلام").Value))
                                cmd.Parameters.AddWithValue("@batchId", row.Cells("رقم الرسالة").Value)
                                cmd.Parameters.AddWithValue("@lot", lotValue)
                                cmd.Parameters.AddWithValue("@clientPermission", If(row.Cells("اذن العميل").Value Is Nothing, DBNull.Value, row.Cells("اذن العميل").Value))
                                cmd.Parameters.AddWithValue("@clientItemCode", If(row.Cells("كود صنف العميل").Value Is Nothing, DBNull.Value, row.Cells("كود صنف العميل").Value))
                                cmd.Parameters.AddWithValue("@weightpk", 0)
                                cmd.Parameters.AddWithValue("@meterQuantity", If(row.Cells("كميه متر").Value Is Nothing OrElse String.IsNullOrEmpty(row.Cells("كميه متر").Value.ToString()), DBNull.Value, row.Cells("كميه متر").Value))
                                cmd.Parameters.AddWithValue("@rollpk", 0)
                                cmd.Parameters.AddWithValue("@storePermission", If(row.Cells("اذن اضافه المخزن").Value Is Nothing, DBNull.Value, row.Cells("اذن اضافه المخزن").Value))
                                cmd.Parameters.AddWithValue("@rolls_count", 0)
                                cmd.Parameters.AddWithValue("@weight_quantity", 0)
                                cmd.Parameters.AddWithValue("@username", LoggedInUsername)

                                cmd.ExecuteNonQuery()
                            End Using
                        End If
                    End Using

                    MessageBox.Show("تم حفظ البيانات بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    ' Enable the AddRolls button after successful save
                    row.Cells("AddRollsButton").Value = CreateAddRollsIcon()

                    hasNewRow = False
                    UpdateButtonsVisibility()

                End Using
            Catch ex As Exception
                MessageBox.Show("خطأ في حفظ البيانات: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

        ElseIf e.ColumnIndex = dgvBatchDetails.Columns("AddRollsButton").Index AndAlso e.RowIndex >= 0 Then
            ' Handle add rolls button click
            Dim isSelected As Boolean = CBool(dgvBatchDetails.Rows(e.RowIndex).Cells("SelectRow").Value)

            If Not isSelected Then
                MessageBox.Show("الرجاء تحديد الصف أولاً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Check if the selected lot is saved in the database
            Dim lotValue As String = dgvBatchDetails.Rows(e.RowIndex).Cells("lot").Value.ToString()
            Dim batchId As String = dgvBatchDetails.Rows(e.RowIndex).Cells("رقم الرسالة").Value.ToString()
            Dim isLotSaved As Boolean = False

            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim query As String = "SELECT COUNT(*) FROM batch_details WHERE batch_id = @batchId AND lot = @lot"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@batchId", batchId)
                    cmd.Parameters.AddWithValue("@lot", lotValue)
                    Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                    isLotSaved = (count > 0)
                End Using
            End Using

            If Not isLotSaved Then
                MessageBox.Show("الرجاء حفظ بيانات اللوت أولاً قبل إضافة الأتواب.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Additional check to ensure required fields are filled
            Dim row As DataGridViewRow = dgvBatchDetails.Rows(e.RowIndex)
            If String.IsNullOrEmpty(row.Cells("اذن العميل").Value?.ToString()) OrElse
               String.IsNullOrEmpty(row.Cells("كود صنف العميل").Value?.ToString()) Then
                MessageBox.Show("الرجاء إدخال اذن العميل وكود صنف العميل وحفظ البيانات أولاً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ShowRollsGrid()
        End If
    End Sub

    Private Sub btnAddRolls_Click(sender As Object, e As EventArgs) Handles btnAddRolls.Click
        ' Check if a row is selected
        Dim selectedRow As DataGridViewRow = Nothing
        For Each row As DataGridViewRow In dgvBatchDetails.Rows
            If CBool(row.Cells("SelectRow").Value) Then
                selectedRow = row
                Exit For
            End If
        Next

        If selectedRow Is Nothing Then
            MessageBox.Show("الرجاء تحديد صف أولاً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Check if we are in "add new lot" mode and the lot hasn't been saved yet
        If cmbLotAction.SelectedItem?.ToString() = "اضافة لوت جديد" AndAlso hasNewRow Then
            MessageBox.Show("الرجاء حفظ بيانات اللوت الجديد أولاً قبل إضافة الأتواب.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Check if the selected lot is saved in the database
        Dim lotValue As String = selectedRow.Cells("lot").Value.ToString()
        Dim batchId As String = selectedRow.Cells("رقم الرسالة").Value.ToString()
        Dim isLotSaved As Boolean = False

        Using conn As New SqlConnection(connectionString)
            conn.Open()
            Dim query As String = "SELECT COUNT(*) FROM batch_details WHERE batch_id = @batchId AND lot = @lot"
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@batchId", batchId)
                cmd.Parameters.AddWithValue("@lot", lotValue)
                Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                isLotSaved = (count > 0)
            End Using
        End Using

        If Not isLotSaved Then
            MessageBox.Show("الرجاء حفظ بيانات اللوت أولاً قبل إضافة الأتواب.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Show rolls grid and setup initial row
        dgvRolls.Rows.Clear()
        dgvRolls.Visible = True

        ' Position rolls grid and save button
        dgvRolls.Location = New Point(20, dgvBatchDetails.Bottom + 10)
        dgvRolls.Width = Me.ClientSize.Width / 2 - 30
        dgvRolls.Height = Me.ClientSize.Height - dgvRolls.Top - 20

        ' Position and show save button
        btnSaveRolls.Location = New Point(dgvRolls.Right + 10, dgvRolls.Top)
        btnSaveRolls.Visible = True

        AddNewRollRow()
    End Sub

    Private Sub AddNewRollRow()
        ' Get the selected batch row
        Dim selectedBatchRow As DataGridViewRow = Nothing
        For Each row As DataGridViewRow In dgvBatchDetails.Rows
            If CBool(row.Cells("SelectRow").Value) Then
                selectedBatchRow = row
                Exit For
            End If
        Next

        If selectedBatchRow IsNot Nothing Then
            Dim batchId As String = selectedBatchRow.Cells("رقم الرسالة").Value.ToString()
            Dim lotNumber As String = selectedBatchRow.Cells(GetLotColumnIndex()).Value.ToString()
            Dim nextRollNumber As Integer = GetNextRollNumber(batchId, lotNumber)
            dgvRolls.Rows.Add(nextRollNumber, Nothing, "طباعة")
            ' Show save button when adding rows
            btnSaveRolls.Visible = True
        End If
    End Sub

    Private Function GetNextRollNumber(batchId As String, lotNumber As String) As Integer
        Dim maxNumber As Integer = 0
        Try
            ' Get the maximum roll number from the database
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim query As String = "SELECT MAX(CAST(roll AS INT)) FROM batch_details_rolls WHERE batch = @batchId AND lot = @lot"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@batchId", batchId)
                    cmd.Parameters.AddWithValue("@lot", lotNumber)
                    Dim result = cmd.ExecuteScalar()
                    If result IsNot DBNull.Value AndAlso result IsNot Nothing Then
                        maxNumber = Convert.ToInt32(result)
                    End If
                End Using
            End Using

            ' Get the maximum roll number from the current dgvRolls
            For Each row As DataGridViewRow In dgvRolls.Rows
                If row.Cells("RollNumber").Value IsNot Nothing Then
                    Dim currentRollNumber As Integer
                    If Integer.TryParse(row.Cells("RollNumber").Value.ToString(), currentRollNumber) Then
                        If currentRollNumber > maxNumber Then
                            maxNumber = currentRollNumber
                        End If
                    End If
                End If
            Next

            ' If we found any existing numbers, increment the maximum
            ' Otherwise, start from 1
            If maxNumber > 0 Then
                Return maxNumber + 1
            Else
                Return 1
            End If

        Catch ex As Exception
            MessageBox.Show("خطأ في الحصول على رقم التوب التالي: " & ex.Message)
            Return 1 ' Return 1 in case of error
        End Try
    End Function

    Private Sub dgvRolls_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvRolls.CellContentClick
        If e.RowIndex >= 0 Then
            If e.ColumnIndex = dgvRolls.Columns("PrintButton").Index Then
                ' Handle print button click (existing code)
                Dim weightCell = dgvRolls.Rows(e.RowIndex).Cells("Weight").Value
                If weightCell Is Nothing OrElse String.IsNullOrEmpty(weightCell.ToString()) Then
                    MessageBox.Show("الرجاء إدخال الوزن قبل الطباعة.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                ' Proceed with printing
                PrintLabel(dgvRolls.Rows(e.RowIndex))
            End If
        End If
    End Sub

    Private Sub dgvRolls_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgvRolls.CellValueChanged
        ' When weight is entered
        If e.ColumnIndex = dgvRolls.Columns("Weight").Index AndAlso e.RowIndex >= 0 Then
            ' Get the selected batch row
            Dim selectedBatchRow As DataGridViewRow = Nothing
            For Each row As DataGridViewRow In dgvBatchDetails.Rows
                If CBool(row.Cells("SelectRow").Value) Then
                    selectedBatchRow = row
                    Exit For
                End If
            Next

            If selectedBatchRow IsNot Nothing Then
                ' Get existing values from batch_details
                Dim existingWeight As Decimal = 0
                Dim existingRolls As Integer = 0

                Using conn As New SqlConnection(connectionString)
                    conn.Open()
                    Dim query As String = "SELECT ISNULL(weightpk, 0) as weightpk, ISNULL(rollpk, 0) as rollpk FROM batch_details " &
                                        "WHERE batch_id = @batchId AND lot = @lot"
                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@batchId", selectedBatchRow.Cells("رقم الرسالة").Value)
                        cmd.Parameters.AddWithValue("@lot", selectedBatchRow.Cells(GetLotColumnIndex()).Value)
                        Using reader As SqlDataReader = cmd.ExecuteReader()
                            If reader.Read() Then
                                existingWeight = Convert.ToDecimal(reader("weightpk"))
                                existingRolls = Convert.ToInt32(reader("rollpk"))
                            End If
                        End Using
                    End Using
                End Using

                ' Calculate total weight from current rolls
                Dim currentWeight As Decimal = 0
                For Each row As DataGridViewRow In dgvRolls.Rows
                    If row.Cells("Weight").Value IsNot Nothing AndAlso Not String.IsNullOrEmpty(row.Cells("Weight").Value.ToString()) Then
                        currentWeight += Convert.ToDecimal(row.Cells("Weight").Value)
                    End If
                Next

                ' Update weight and roll count in batch details
                selectedBatchRow.Cells("كميه وزن").Value = existingWeight + currentWeight
                selectedBatchRow.Cells("عدد اتواب").Value = existingRolls + dgvRolls.Rows.Count

                ' Add new row if this is the last row and weight is entered
                If e.RowIndex = dgvRolls.Rows.Count - 1 AndAlso
                   dgvRolls.Rows(e.RowIndex).Cells("Weight").Value IsNot Nothing AndAlso
                   Not String.IsNullOrEmpty(dgvRolls.Rows(e.RowIndex).Cells("Weight").Value.ToString()) Then
                    Me.BeginInvoke(New Action(AddressOf DelayedAddNewRow))
                End If
            End If
        End If
    End Sub

    Private Sub DelayedAddNewRow()
        Try
            ' Get the selected batch row
            Dim selectedBatchRow As DataGridViewRow = Nothing
            For Each row As DataGridViewRow In dgvBatchDetails.Rows
                If CBool(row.Cells("SelectRow").Value) Then
                    selectedBatchRow = row
                    Exit For
                End If
            Next

            If selectedBatchRow IsNot Nothing Then
                Dim batchId As String = selectedBatchRow.Cells("رقم الرسالة").Value.ToString()
                Dim lotNumber As String = selectedBatchRow.Cells(GetLotColumnIndex()).Value.ToString()
                Dim nextRollNumber As Integer = GetNextRollNumber(batchId, lotNumber)
                dgvRolls.Rows.Add(nextRollNumber, Nothing, "طباعة")
            End If
        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء إضافة صف جديد: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Function GetLotColumnIndex() As Integer
        For Each col As DataGridViewColumn In dgvBatchDetails.Columns
            If col.DataPropertyName = "lot" Then
                Return col.Index
            End If
        Next
        Return -1
    End Function

    Private Sub btnSaveRolls_Click(sender As Object, e As EventArgs) Handles btnSaveRolls.Click
        Try
            ' Get the selected batch row
            Dim selectedBatchRow As DataGridViewRow = Nothing
            For Each row As DataGridViewRow In dgvBatchDetails.Rows
                If CBool(row.Cells("SelectRow").Value) Then
                    selectedBatchRow = row
                    Exit For
                End If
            Next

            If selectedBatchRow Is Nothing Then
                MessageBox.Show("الرجاء تحديد صف من جدول التفاصيل أولاً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Using transaction As SqlTransaction = conn.BeginTransaction()
                    Try
                        ' Calculate total weight and count of new rolls being saved
                        Dim newTotalWeight As Decimal = 0
                        Dim newRollsCount As Integer = 0

                        ' Insert new rolls and calculate totals
                        For Each row As DataGridViewRow In dgvRolls.Rows
                            If row.Cells("Weight").Value IsNot Nothing AndAlso
                               Not String.IsNullOrEmpty(row.Cells("Weight").Value.ToString()) Then

                                Dim weight As Decimal = Convert.ToDecimal(row.Cells("Weight").Value)
                                newTotalWeight += weight
                                newRollsCount += 1

                                Dim insertRollQuery As String = "INSERT INTO batch_details_rolls (date, batch, lot, roll, weight, weightpk, location,username) " &
                                                               "VALUES (@date, @batch, @lot, @roll, @weight, @weightpk, @location, @username)"

                                Using cmdInsert As New SqlCommand(insertRollQuery, conn, transaction)
                                    cmdInsert.Parameters.AddWithValue("@date", DateTime.Now)
                                    cmdInsert.Parameters.AddWithValue("@batch", selectedBatchRow.Cells("رقم الرسالة").Value)
                                    cmdInsert.Parameters.AddWithValue("@lot", selectedBatchRow.Cells(GetLotColumnIndex()).Value)
                                    cmdInsert.Parameters.AddWithValue("@roll", row.Cells("RollNumber").Value)
                                    cmdInsert.Parameters.AddWithValue("@weight", weight)
                                    cmdInsert.Parameters.AddWithValue("@weightpk", weight)
                                    cmdInsert.Parameters.AddWithValue("@username", LoggedInUsername)

                                    ' Handle empty location value
                                    If row.Cells("Location").Value IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(row.Cells("Location").Value.ToString()) Then
                                        cmdInsert.Parameters.AddWithValue("@location", row.Cells("Location").Value)
                                    Else
                                        cmdInsert.Parameters.AddWithValue("@location", DBNull.Value)
                                    End If

                                    cmdInsert.ExecuteNonQuery()
                                End Using
                            End If
                        Next

                        ' Get current values from batch_details
                        Dim currentWeightPK As Decimal = 0
                        Dim currentRollsPK As Integer = 0
                        Dim currentWeightQuantity As Decimal = 0
                        Dim currentRollsCount As Integer = 0

                        Dim getCurrentValuesQuery As String = "SELECT ISNULL(weightpk, 0) as weightpk, ISNULL(rollpk, 0) as rollpk, " &
                                                           "ISNULL(weight_quantity, 0) as weight_quantity, ISNULL(rolls_count, 0) as rolls_count " &
                                                           "FROM batch_details " &
                                                           "WHERE batch_id = @batchId AND lot = @lot"

                        Using cmdGet As New SqlCommand(getCurrentValuesQuery, conn, transaction)
                            cmdGet.Parameters.AddWithValue("@batchId", selectedBatchRow.Cells("رقم الرسالة").Value)
                            cmdGet.Parameters.AddWithValue("@lot", selectedBatchRow.Cells(GetLotColumnIndex()).Value)
                            Using reader As SqlDataReader = cmdGet.ExecuteReader()
                                If reader.Read() Then
                                    currentWeightPK = Convert.ToDecimal(reader("weightpk"))
                                    currentRollsPK = Convert.ToInt32(reader("rollpk"))
                                    currentWeightQuantity = Convert.ToDecimal(reader("weight_quantity"))
                                    currentRollsCount = Convert.ToInt32(reader("rolls_count"))
                                End If
                            End Using
                        End Using

                        ' Update batch_details with new totals
                        ' weightpk and rollpk represent the total (current + new)
                        ' weight_quantity and rolls_count represent the sum of old and new values
                        Dim updateQuery As String = "UPDATE batch_details SET " &
                                                  "weightpk = @totalWeightPK, " &
                                                  "rollpk = @totalRollsPK, " &
                                                  "weight_quantity = @totalWeightQuantity, " &
                                                  "rolls_count = @totalRollsCount, " &
                                                  "datetrans = @datetrans " &
                                                  "WHERE batch_id = @batch AND lot = @lot"

                        Using cmdUpdate As New SqlCommand(updateQuery, conn, transaction)
                            cmdUpdate.Parameters.AddWithValue("@batch", selectedBatchRow.Cells("رقم الرسالة").Value)
                            cmdUpdate.Parameters.AddWithValue("@lot", selectedBatchRow.Cells(GetLotColumnIndex()).Value)
                            cmdUpdate.Parameters.AddWithValue("@totalWeightPK", currentWeightPK + newTotalWeight)
                            cmdUpdate.Parameters.AddWithValue("@totalRollsPK", currentRollsPK + newRollsCount)
                            cmdUpdate.Parameters.AddWithValue("@totalWeightQuantity", currentWeightQuantity + newTotalWeight)
                            cmdUpdate.Parameters.AddWithValue("@totalRollsCount", currentRollsCount + newRollsCount)
                            cmdUpdate.Parameters.AddWithValue("@datetrans", DateTime.Now)
                            cmdUpdate.ExecuteNonQuery()
                        End Using

                        transaction.Commit()
                        MessageBox.Show("تم حفظ الأتواب بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)

                        ' Hide location controls after successful save
                        cmbLocation.Visible = False
                        lblLocation.Visible = False
                        lblTotalWeight.Visible = False
                        txtTotalWeight.Visible = False
                        lblNumberOfRolls.Visible = False
                        txtNumberOfRolls.Visible = False
                        btnDivideWeight.Visible = False



                        ' Clear and hide rolls grid
                        HideRollsGrid()

                    Catch ex As Exception
                        transaction.Rollback()
                        Throw
                    End Try
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء حفظ الأتواب: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub PrintLabel(row As DataGridViewRow)
        Try
            ' Get the selected batch details
            Dim selectedBatchRow As DataGridViewRow = Nothing
            For Each batchRow As DataGridViewRow In dgvBatchDetails.Rows
                If CBool(batchRow.Cells("SelectRow").Value) Then
                    selectedBatchRow = batchRow
                    Exit For
                End If
            Next

            If selectedBatchRow Is Nothing Then
                MessageBox.Show("لم يتم العثور على الصف المحدد", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            ' Get the values for printing
            Dim batchNumber As String = selectedBatchRow.Cells("رقم الرسالة").Value.ToString()
            Dim lotValue As String = selectedBatchRow.Cells(GetLotColumnIndex()).Value.ToString()
            Dim rollNumber As String = row.Cells("RollNumber").Value.ToString()
            Dim weight As String = If(row.Cells("Weight").Value IsNot Nothing, row.Cells("Weight").Value.ToString(), "0")

            ' First check if the printer exists
            Dim printerName As String = "ZD220-203dpi ZPL"
            Dim printerFound As Boolean = False
            For Each printer As String In System.Drawing.Printing.PrinterSettings.InstalledPrinters
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

            ' Convert backslash to hex for ZPL
            Dim printableLotValue As String = ""
            For Each c As Char In lotValue
                If c = "\"c Then
                    printableLotValue &= "\\"
                Else
                    printableLotValue &= c
                End If
            Next

            ' Create barcode data
            Dim barcodeData As String = printableLotValue & "*" & rollNumber

            ' ZPL code for Zebra printer with table layout and barcode
            Dim zplCode As String = "^XA" & vbCrLf &
                                  "^CI28" & vbCrLf &  ' Set encoding to Unicode UTF-8
                                  "^PON" & vbCrLf &
                                  "^LH0,0" & vbCrLf &
                                  "^FO20,20^A0N,100,100^FB792,1,0,C^FDROLL DATA^FS" & vbCrLf &  ' Increased font size
                                  "^FO20,120^GB792,400,2^FS" & vbCrLf &  ' Table border
                                  "^FO20,220^GB792,0,2^FS" & vbCrLf &  ' Horizontal lines
                                  "^FO20,320^GB792,0,2^FS" & vbCrLf &
                                  "^FO406,120^GB0,400,2^FS" & vbCrLf &  ' Vertical line
                                  "^FO40,170^A0N,60,60^FB366,1,0,C^FDBatch No:^FS" & vbCrLf &  ' Increased font size
                                  "^FO40,270^A0N,60,60^FB366,1,0,C^FDLot No:^FS" & vbCrLf &
                                  "^FO40,370^A0N,60,60^FB366,1,0,C^FDRoll No:^FS" & vbCrLf &
                                  "^FO40,470^A0N,60,60^FB366,1,0,C^FDWeight:^FS" & vbCrLf &
                                  "^FO426,170^A0N,60,60^FB366,1,0,C^FD" & batchNumber & "^FS" & vbCrLf &  ' Increased font size
                                  "^FO426,270^A0N,60,60^FB366,1,0,C^FD" & printableLotValue & "^FS" & vbCrLf &
                                  "^FO426,370^A0N,60,60^FB366,1,0,C^FD" & rollNumber & "^FS" & vbCrLf &
                                  "^FO426,470^A0N,60,60^FB366,1,0,C^FD" & weight & " KG^FS" & vbCrLf &
                                  "^FO20,600^GB792,0,2^FS" & vbCrLf &  ' Line above barcode
                                  "^FO20,620^A0N,50,50^FB792,1,0,C^FDBarcode^FS" & vbCrLf &  ' Increased font size
                                  "^FO196,680^BY4" & vbCrLf &  ' Centered barcode
                                  "^BCN,180,Y,N,N" & vbCrLf &
                                  "^FD" & barcodeData & "^FS" & vbCrLf &
                                  "^XZ"

            ' Send directly to printer
            If RawPrinterHelper.SendStringToPrinter(printerName, zplCode) Then
                MessageBox.Show("تمت الطباعة بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("حدث خطأ أثناء الطباعة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        Catch ex As Exception
            MessageBox.Show("خطأ عام: " & ex.Message & vbCrLf &
                          "نوع الخطأ: " & ex.GetType().Name & vbCrLf &
                          "تفاصيل إضافية: " & ex.StackTrace,
                          "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Add RawPrinterHelper class for printing
    Public Class RawPrinterHelper
        ' Structure and API declarations
        <System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet:=System.Runtime.InteropServices.CharSet.Ansi)>
        Private Class DOCINFOA
            <System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPStr)>
            Public pDocName As String
            <System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPStr)>
            Public pOutputFile As String
            <System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPStr)>
            Public pDataType As String
        End Class

        <System.Runtime.InteropServices.DllImport("winspool.Drv", EntryPoint:="OpenPrinterA", SetLastError:=True, CharSet:=System.Runtime.InteropServices.CharSet.Ansi, ExactSpelling:=True, CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)>
        Private Shared Function OpenPrinter(<System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPStr)> ByVal szPrinter As String, ByRef hPrinter As IntPtr, ByVal pd As IntPtr) As Boolean
        End Function

        <System.Runtime.InteropServices.DllImport("winspool.Drv", EntryPoint:="ClosePrinter", SetLastError:=True, ExactSpelling:=True, CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)>
        Private Shared Function ClosePrinter(ByVal hPrinter As IntPtr) As Boolean
        End Function

        <System.Runtime.InteropServices.DllImport("winspool.Drv", EntryPoint:="StartDocPrinterA", SetLastError:=True, CharSet:=System.Runtime.InteropServices.CharSet.Ansi, ExactSpelling:=True, CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)>
        Private Shared Function StartDocPrinter(ByVal hPrinter As IntPtr, ByVal level As Int32, <System.Runtime.InteropServices.In, System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPStruct)> ByVal di As DOCINFOA) As Boolean
        End Function

        <System.Runtime.InteropServices.DllImport("winspool.Drv", EntryPoint:="EndDocPrinter", SetLastError:=True, ExactSpelling:=True, CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)>
        Private Shared Function EndDocPrinter(ByVal hPrinter As IntPtr) As Boolean
        End Function

        <System.Runtime.InteropServices.DllImport("winspool.Drv", EntryPoint:="StartPagePrinter", SetLastError:=True, ExactSpelling:=True, CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)>
        Private Shared Function StartPagePrinter(ByVal hPrinter As IntPtr) As Boolean
        End Function

        <System.Runtime.InteropServices.DllImport("winspool.Drv", EntryPoint:="EndPagePrinter", SetLastError:=True, ExactSpelling:=True, CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)>
        Private Shared Function EndPagePrinter(ByVal hPrinter As IntPtr) As Boolean
        End Function

        <System.Runtime.InteropServices.DllImport("winspool.Drv", EntryPoint:="WritePrinter", SetLastError:=True, ExactSpelling:=True, CallingConvention:=System.Runtime.InteropServices.CallingConvention.StdCall)>
        Private Shared Function WritePrinter(ByVal hPrinter As IntPtr, ByVal pBytes As IntPtr, ByVal dwCount As Int32, ByRef dwWritten As Int32) As Boolean
        End Function

        Public Shared Function SendStringToPrinter(ByVal szPrinterName As String, ByVal szString As String) As Boolean
            Dim di As New DOCINFOA()
            di.pDocName = "ZebraLabel"
            di.pDataType = "RAW"
            di.pOutputFile = Nothing

            Dim hPrinter As IntPtr = IntPtr.Zero

            Try
                If Not OpenPrinter(szPrinterName.Normalize(), hPrinter, IntPtr.Zero) Then
                    Throw New Exception("Could not open printer. Error: " & System.Runtime.InteropServices.Marshal.GetLastWin32Error().ToString())
                End If

                If Not StartDocPrinter(hPrinter, 1, di) Then
                    Throw New Exception("StartDocPrinter failed. Error: " & System.Runtime.InteropServices.Marshal.GetLastWin32Error().ToString())
                End If

                Dim btArray() As Byte = System.Text.Encoding.GetEncoding("IBM437").GetBytes(szString)
                Dim dwCount As Int32 = btArray.Length
                Dim dwWritten As Int32 = 0
                Dim pBytes As IntPtr = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(dwCount)

                Try
                    System.Runtime.InteropServices.Marshal.Copy(btArray, 0, pBytes, dwCount)

                    If Not StartPagePrinter(hPrinter) Then
                        Throw New Exception("StartPagePrinter failed. Error: " & System.Runtime.InteropServices.Marshal.GetLastWin32Error().ToString())
                    End If

                    If Not WritePrinter(hPrinter, pBytes, dwCount, dwWritten) Then
                        Throw New Exception("WritePrinter failed. Error: " & System.Runtime.InteropServices.Marshal.GetLastWin32Error().ToString())
                    End If

                    If Not EndPagePrinter(hPrinter) Then
                        Throw New Exception("EndPagePrinter failed. Error: " & System.Runtime.InteropServices.Marshal.GetLastWin32Error().ToString())
                    End If

                    If Not EndDocPrinter(hPrinter) Then
                        Throw New Exception("EndDocPrinter failed. Error: " & System.Runtime.InteropServices.Marshal.GetLastWin32Error().ToString())
                    End If

                    Return True

                Finally
                    If pBytes <> IntPtr.Zero Then
                        System.Runtime.InteropServices.Marshal.FreeCoTaskMem(pBytes)
                    End If
                End Try

            Catch ex As Exception
                MessageBox.Show("خطأ في إرسال البيانات إلى الطابعة: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False

            Finally
                If hPrinter <> IntPtr.Zero Then
                    ClosePrinter(hPrinter)
                End If
            End Try
        End Function
    End Class

    Private Sub Form_Resize(sender As Object, e As EventArgs)
        centerX = Me.ClientSize.Width \ 2

        ' Update control positions
        cmbMessageType.Left = centerX + 300
        lblPO.Left = centerX + 190
        cmbPO.Left = centerX + 10
        lblBatch.Left = centerX - 90
        cmbBatch.Left = centerX - 270
        lblLotAction.Left = centerX - 390
        cmbLotAction.Left = centerX - 670
        lblBatchNumber.Left = centerX - 150

        ' Update grid widths
        dgvBatchInfo.Width = Me.ClientSize.Width - 40
        dgvBatchDetails.Width = Me.ClientSize.Width - 40

        ' Update rolls grid if visible
        If dgvRolls.Visible Then
            dgvRolls.Width = Me.ClientSize.Width / 2 - 30
            dgvRolls.Height = Me.ClientSize.Height - dgvRolls.Top - 60 ' Reduce height to make room for save button

            ' Update read weight button position (next to grid)
            btnReadWeight.Location = New Point(dgvRolls.Right + 10, dgvRolls.Top)
            btnReadWeight.BringToFront()

            ' Update save button position (below grid)
            btnSaveRolls.Location = New Point(dgvRolls.Left, dgvRolls.Bottom + 10)
            btnSaveRolls.Width = dgvRolls.Width
            btnSaveRolls.BringToFront()
        End If

        ' Update weight distribution controls position
        lblTotalWeight.Location = New Point(dgvRolls.Right + 100, dgvRolls.Top + 150)
        txtTotalWeight.Location = New Point(dgvRolls.Right + 100, lblTotalWeight.Bottom + 5)
        lblNumberOfRolls.Location = New Point(dgvRolls.Right + 100, txtTotalWeight.Bottom + 10)
        txtNumberOfRolls.Location = New Point(dgvRolls.Right + 100, lblNumberOfRolls.Bottom + 5)
        btnDivideWeight.Location = New Point(dgvRolls.Right + 100, txtNumberOfRolls.Bottom + 10)
    End Sub

    ' Add ConvertToHex function to handle Arabic text
    Private Function ConvertToHex(text As String) As String
        Dim encoding As New System.Text.UTF8Encoding()
        Dim bytes As Byte() = encoding.GetBytes(text)
        Return "_" & BitConverter.ToString(bytes).Replace("-", "_")
    End Function

    Private Sub LoadBatchInfo(batchId As String)
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()

                Dim query As String = "SELECT ISNULL(c1.code, '') AS 'كود العميل', " &
                                    "ISNULL(sup.code, '') AS 'كود المورد', " &
                                    "ISNULL(sup.name, '') AS 'المورد', " &
                                    "ISNULL(s.code, '') AS 'كود الخامة', " &
                                    "ISNULL(br.batch, '') AS 'رقم الرسالة', " &
                                    "ISNULL(s.name, '') AS 'الخامة2', " &
                                    "ISNULL(ks.service_ar, '') AS 'نوع الخدمه', " &
                                    "ISNULL(br.material, '') AS 'الخامة' " &
                                    "FROM batch_raw br " &
                                    "JOIN clients c1 ON br.client_code = c1.id " &
                                    "LEFT JOIN suppliers sup ON br.sup_code = sup.id " &
                                    "LEFT JOIN fabric f ON br.fabric_type = f.id " &
                                    "LEFT JOIN style s ON br.style_id = s.id " &
                                    "LEFT JOIN kind_service ks ON br.service_id = ks.id " &
                                    "WHERE br.batch = @batchId"

                Using adapter As New SqlDataAdapter(query, conn)
                    adapter.SelectCommand.Parameters.AddWithValue("@batchId", batchId)

                    Dim dt As New DataTable()
                    adapter.Fill(dt)

                    dgvBatchInfo.DataSource = dt
                    dgvBatchInfo.Visible = True

                    ' Update height after loading data
                    UpdateBatchInfoHeight()
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل بيانات الرسالة: " & ex.Message)
        End Try
    End Sub

    ' Add function to update dgvBatchInfo height
    Private Sub UpdateBatchInfoHeight()
        If dgvBatchInfo.Rows.Count > 0 Then
            ' Calculate total height needed for rows plus header
            Dim totalHeight As Integer = dgvBatchInfo.ColumnHeadersHeight
            For Each row As DataGridViewRow In dgvBatchInfo.Rows
                totalHeight += row.Height
            Next

            ' Add a small padding
            totalHeight += 5

            ' Update grid height
            dgvBatchInfo.Height = totalHeight

            ' Update position of dgvBatchDetails
            dgvBatchDetails.Location = New Point(20, dgvBatchInfo.Bottom + 10)
        End If
    End Sub

    Private Sub Loadservice()
        Try
            Using conn As New SqlConnection(connectionString)
                Dim query As String = "SELECT id,service_ar FROM kind_service"
                Dim cmd As New SqlCommand(query, conn)
                Dim dt As New DataTable()

                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                dt.Load(reader)

                cmbservice.DataSource = dt
                cmbservice.DisplayMember = "service_ar"
                cmbservice.ValueMember = "id"
                cmbservice.SelectedIndex = -1
                conn.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading service: " & ex.Message)
        End Try
        isServiceLoaded = True
    End Sub

    Private Sub Loadstyle()
        Try
            Using conn As New SqlConnection(connectionString)
                Dim query As String = "SELECT id, code, name FROM style"
                Dim cmd As New SqlCommand(query, conn)
                Dim dt As New DataTable()

                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                dt.Load(reader)

                cmbstyle.DataSource = dt
                cmbstyle.DisplayMember = "code"
                cmbstyle.ValueMember = "id"
                cmbstyle.SelectedIndex = -1
                conn.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading style: " & ex.Message)
        End Try
        isStyleLoaded = True
    End Sub

    Private Sub LoadClientCodes()
        Try
            Using conn As New SqlConnection(connectionString)
                Dim query As String = "SELECT id, code, name FROM Clients"
                Dim cmd As New SqlCommand(query, conn)
                Dim dt As New DataTable()

                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                dt.Load(reader)

                cmbclient.DataSource = dt
                cmbclient.DisplayMember = "code"
                cmbclient.ValueMember = "id"
                cmbclient.SelectedIndex = -1
                conn.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading client codes: " & ex.Message)
        End Try
        isClientLoaded = True
    End Sub

    Private Sub LoadSuppliersCodes()
        Try
            Using conn As New SqlConnection(connectionString)
                Dim query As String = "SELECT id, code, name FROM suppliers"
                Dim cmd As New SqlCommand(query, conn)
                Dim dt As New DataTable()

                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                dt.Load(reader)

                cmbsupplier.DataSource = dt
                cmbsupplier.DisplayMember = "name"
                cmbsupplier.ValueMember = "id"
                cmbsupplier.SelectedIndex = -1
                conn.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading supplier codes: " & ex.Message)
        End Try
        isSupplierLoaded = True
    End Sub

    Private Sub LoadFabricTypes()
        Try
            Using conn As New SqlConnection(connectionString)
                Dim query As String = "SELECT id, fabrictype_ar FROM fabric"
                Dim cmd As New SqlCommand(query, conn)
                Dim dt As New DataTable()

                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                dt.Load(reader)

                cmbkindfabric.DataSource = dt
                cmbkindfabric.DisplayMember = "fabrictype_ar"
                cmbkindfabric.ValueMember = "id"
                cmbkindfabric.SelectedIndex = -1
                conn.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading fabric types: " & ex.Message)
        End Try
    End Sub

    Private Sub cmbservice_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbservice.SelectedIndexChanged
        If isServiceLoaded AndAlso cmbservice.SelectedIndex <> -1 Then
            Dim selectedServiceId As Integer = Convert.ToInt32(cmbservice.SelectedValue)
            If selectedServiceId = 2 OrElse selectedServiceId = 4 Then
                cmbsupplier.Enabled = False
                cmbsupplier.SelectedIndex = -1
            Else
                cmbsupplier.Enabled = True
            End If
        End If
    End Sub

    Private Sub cmbclient_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbclient.SelectedIndexChanged
        If isClientLoaded AndAlso cmbclient.SelectedIndex <> -1 Then
            Dim selectedRow As DataRowView = DirectCast(cmbclient.SelectedItem, DataRowView)
            lblclient.Text = "عميل: " & selectedRow("name").ToString()
        End If
    End Sub

    Private Sub cmbsupplier_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbsupplier.SelectedIndexChanged
        If isSupplierLoaded AndAlso cmbsupplier.SelectedIndex <> -1 Then
            Dim selectedRow As DataRowView = DirectCast(cmbsupplier.SelectedItem, DataRowView)
            lblsup.Text = "مورد: " & selectedRow("code").ToString()
        End If
    End Sub

    Private Sub cmbstyle_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbstyle.SelectedIndexChanged
        If isStyleLoaded AndAlso cmbstyle.SelectedIndex <> -1 Then
            Dim selectedRow As DataRowView = DirectCast(cmbstyle.SelectedItem, DataRowView)
            lblstyle.Text = "الخامة: " & selectedRow("name").ToString()
        End If
    End Sub

    Private Sub btninsert_Click(sender As Object, e As EventArgs) Handles btninsert.Click
        If cmbclient.SelectedIndex = -1 OrElse cmbservice.SelectedIndex = -1 OrElse
           cmbkindfabric.SelectedIndex = -1 OrElse String.IsNullOrEmpty(txtmaterial.Text) Then
            MessageBox.Show("الرجاء إدخال جميع البيانات المطلوبة قبل التسجيل.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If BatchNumberExists(Convert.ToInt32(lblNewBatchNumber.Text)) Then
            MessageBox.Show("رقم الرسالة موجود بالفعل. الرجاء إنشاء رقم رسالة جديد.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Using transaction As SqlTransaction = conn.BeginTransaction()
                    Try
                        ' Insert into batch_raw
                        Dim batchQuery As String = "INSERT INTO batch_raw (datetrans, batch, sup_code, client_code, fabric_type, " &
                                                 "style_id, service_id, po_number, material, username) " &
                                                 "VALUES (@datetrans, @batch, @sup_code, @client_code, @fabric_type, " &
                                                 "@style_id, @service_id, @ponumber, @material, @username)"

                        Using cmdBatch As New SqlCommand(batchQuery, conn, transaction)
                            cmdBatch.Parameters.AddWithValue("@datetrans", DateTime.Now)
                            cmdBatch.Parameters.AddWithValue("@batch", Convert.ToInt32(lblNewBatchNumber.Text))
                            cmdBatch.Parameters.AddWithValue("@sup_code", If(cmbsupplier.SelectedValue Is Nothing, DBNull.Value, Convert.ToInt32(cmbsupplier.SelectedValue)))
                            cmdBatch.Parameters.AddWithValue("@client_code", Convert.ToInt32(cmbclient.SelectedValue))
                            cmdBatch.Parameters.AddWithValue("@fabric_type", Convert.ToInt32(cmbkindfabric.SelectedValue))
                            cmdBatch.Parameters.AddWithValue("@style_id", Convert.ToInt32(cmbstyle.SelectedValue))
                            cmdBatch.Parameters.AddWithValue("@service_id", Convert.ToInt32(cmbservice.SelectedValue))
                            cmdBatch.Parameters.AddWithValue("@ponumber", If(String.IsNullOrEmpty(txtpo.Text), String.Empty, txtpo.Text))
                            cmdBatch.Parameters.AddWithValue("@material", txtmaterial.Text)
                            cmdBatch.Parameters.AddWithValue("@username", LoggedInUsername)

                            cmdBatch.ExecuteNonQuery()
                        End Using

                        ' Insert into batch_details using the same batch number for lot
                        Dim detailsQuery As String = "INSERT INTO batch_details (datetrans, batch_id, lot,username) VALUES (@datetrans, @batch_id, @lot,@username)"
                        Using cmdDetails As New SqlCommand(detailsQuery, conn, transaction)
                            cmdDetails.Parameters.AddWithValue("@datetrans", DateTime.Now)
                            cmdDetails.Parameters.AddWithValue("@batch_id", lblNewBatchNumber.Text)
                            cmdDetails.Parameters.AddWithValue("@lot", lblNewBatchNumber.Text)
                            cmdDetails.Parameters.AddWithValue("@username", LoggedInUsername)
                            cmdDetails.ExecuteNonQuery()
                        End Using

                        transaction.Commit()
                        MessageBox.Show("تم تسجيل البيانات بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)

                        ' Disable controls after successful insertion
                        DisableNewBatchControls()

                        ' Create DataTable for batch details
                        Dim dt As New DataTable()
                        dt.Columns.Add("تاريخ الاستلام", GetType(Date))
                        dt.Columns.Add("رقم الرسالة", GetType(String))
                        dt.Columns.Add("lot", GetType(String))
                        dt.Columns.Add("اذن العميل", GetType(String))
                        dt.Columns.Add("كود صنف العميل", GetType(String))
                        dt.Columns.Add("كميه وزن", GetType(Decimal))
                        dt.Columns.Add("كميه متر", GetType(Decimal))
                        dt.Columns.Add("عدد اتواب", GetType(Integer))
                        dt.Columns.Add("اذن اضافه المخزن", GetType(String))

                        ' Add initial row
                        Dim row As DataRow = dt.NewRow()
                        row("تاريخ الاستلام") = DateTime.Now
                        row("رقم الرسالة") = lblNewBatchNumber.Text
                        row("lot") = lblNewBatchNumber.Text
                        row("كميه وزن") = 0
                        row("عدد اتواب") = 0
                        dt.Rows.Add(row)

                        ' Set up dgvBatchDetails
                        dgvBatchDetails.DataSource = dt
                        dgvBatchDetails.Visible = True
                        dgvBatchDetails.ReadOnly = False ' Make the grid editable

                        ' Add checkbox column if it doesn't exist
                        If Not dgvBatchDetails.Columns.Contains("SelectRow") Then
                            Dim checkBoxColumn As New DataGridViewCheckBoxColumn()
                            checkBoxColumn.Name = "SelectRow"
                            checkBoxColumn.HeaderText = "اختيار"
                            checkBoxColumn.Width = 50
                            checkBoxColumn.ReadOnly = False
                            dgvBatchDetails.Columns.Insert(0, checkBoxColumn)
                        End If

                        ' Add save button column
                        If Not dgvBatchDetails.Columns.Contains("SaveButton") Then
                            Dim saveButtonColumn As New DataGridViewImageColumn()
                            saveButtonColumn.Name = "SaveButton"
                            saveButtonColumn.HeaderText = ""
                            saveButtonColumn.Image = saveIcon
                            saveButtonColumn.Width = 30
                            dgvBatchDetails.Columns.Add(saveButtonColumn)
                        End If

                        ' Add add rolls button column
                        If Not dgvBatchDetails.Columns.Contains("AddRollsButton") Then
                            Dim addRollsButtonColumn As New DataGridViewImageColumn()
                            addRollsButtonColumn.Name = "AddRollsButton"
                            addRollsButtonColumn.HeaderText = ""
                            addRollsButtonColumn.Image = CreateAddRollsIcon()
                            addRollsButtonColumn.Width = 30
                            dgvBatchDetails.Columns.Add(addRollsButtonColumn)
                        End If

                        ' Set column read-only properties
                        For Each column As DataGridViewColumn In dgvBatchDetails.Columns
                            Select Case column.Name
                                Case "SelectRow", "SaveButton", "AddRollsButton"
                                    column.ReadOnly = True
                                Case "تاريخ الاستلام", "رقم الرسالة", "lot", "كميه وزن", "عدد اتواب"
                                    column.ReadOnly = True
                                Case "اذن العميل", "كود صنف العميل", "كميه متر", "اذن اضافه المخزن"
                                    column.ReadOnly = False
                            End Select
                        Next

                        ' Set the first row as selected and mark as new row
                        dgvBatchDetails.Rows(0).Cells("SelectRow").Value = True
                        hasNewRow = True ' Set hasNewRow to True for the new row

                        ' Show save button for the new row
                        dgvBatchDetails.Rows(0).Cells("SaveButton").Value = saveIcon

                        ' Hide add rolls button for the new row
                        dgvBatchDetails.Rows(0).Cells("AddRollsButton").Value = Nothing

                        ' Position the grids
                        dgvBatchDetails.Location = New Point(20, btninsert.Bottom + 20)
                        dgvBatchDetails.Height = 150
                        dgvBatchDetails.Width = Me.ClientSize.Width - 40

                        ' Update grid height
                        UpdateBatchDetailsHeight()

                        ' Show message to user
                        MessageBox.Show("يمكنك الآن إدخال البيانات في الأعمدة المتاحة ثم الضغط على زر الحفظ", "معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    Catch ex As Exception
                        transaction.Rollback()
                        Throw
                    End Try
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("خطأ في تسجيل البيانات: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub DisableNewBatchControls()
        cmbclient.Enabled = False
        cmbsupplier.Enabled = False
        cmbkindfabric.Enabled = False
        cmbstyle.Enabled = False
        btninsert.Enabled = False
        cmbservice.Enabled = False
        txtpo.Enabled = False
        txtmaterial.Enabled = False
    End Sub

    Private Function BatchNumberExists(batchNumber As Integer) As Boolean
        Dim exists As Boolean = False
        Try
            Using conn As New SqlConnection(connectionString)
                Dim query As String = "SELECT COUNT(*) FROM batch_raw WHERE batch = @batch"
                Dim cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@batch", batchNumber)

                conn.Open()
                Dim result = Convert.ToInt32(cmd.ExecuteScalar())
                exists = (result > 0)
                conn.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("خطأ في التحقق من رقم الرسالة: " & ex.Message)
        End Try
        Return exists
    End Function

    Private Sub btnReadWeight_Click(sender As Object, e As EventArgs) Handles btnReadWeight.Click
        ' Check if location is selected
        If cmbLocation.SelectedIndex = -1 Then
            MessageBox.Show("الرجاء اختيار الموقع أولاً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Get the current row that needs weight
        Dim currentRow As DataGridViewRow = Nothing
        For Each row As DataGridViewRow In dgvRolls.Rows
            If String.IsNullOrEmpty(row.Cells("Weight").Value?.ToString()) Then
                currentRow = row
                Exit For
            End If
        Next

        If currentRow Is Nothing Then
            ' If no empty weight row found, add a new row
            AddNewRollRow()
            currentRow = dgvRolls.Rows(dgvRolls.Rows.Count - 1)
        End If

        ' Set the location for the current row
        currentRow.Cells("Location").Value = cmbLocation.SelectedValue.ToString()

        ' Try to read weight from scale
        Dim weight As String = ReadWeightFromScale()
        If String.IsNullOrEmpty(weight) Then
            ' If scale reading fails, ask user if they want to enter weight manually
            If MessageBox.Show("فشلت قراءة الوزن من الميزان. هل تريد إدخال الوزن يدوياً؟", "تنبيه", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                ' Allow manual entry for this cell
                dgvRolls.Columns("Weight").ReadOnly = False
                dgvRolls.CurrentCell = currentRow.Cells("Weight")
                dgvRolls.BeginEdit(True)
            End If
        Else
            ' Set the weight
            currentRow.Cells("Weight").Value = weight

            ' Print label automatically
            PrintLabel(currentRow)

            ' Trigger the CellValueChanged event to update totals
            Dim eventArgs As New DataGridViewCellEventArgs(dgvRolls.Columns("Weight").Index, currentRow.Index)
            dgvRolls_CellValueChanged(dgvRolls, eventArgs)

            ' Move to next row automatically
            If currentRow.Index = dgvRolls.Rows.Count - 1 Then
                AddNewRollRow()
            End If
            dgvRolls.CurrentCell = dgvRolls.Rows(currentRow.Index + 1).Cells("Weight")
        End If
    End Sub

    Private Sub ShowRollsGrid()
        ' Get the selected batch row
        Dim selectedBatchRow As DataGridViewRow = Nothing
        For Each row As DataGridViewRow In dgvBatchDetails.Rows
            If CBool(row.Cells("SelectRow").Value) Then
                selectedBatchRow = row
                Exit For
            End If
        Next

        If selectedBatchRow Is Nothing Then
            MessageBox.Show("الرجاء تحديد صف أولاً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Check if required fields are filled
        If String.IsNullOrEmpty(selectedBatchRow.Cells("اذن العميل").Value?.ToString()) OrElse
           String.IsNullOrEmpty(selectedBatchRow.Cells("كود صنف العميل").Value?.ToString()) Then
            MessageBox.Show("الرجاء إدخال اذن العميل وكود صنف العميل وحفظ البيانات أولاً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Check if the data is saved in the database
        Dim lotValue As String = selectedBatchRow.Cells("lot").Value.ToString()
        Dim batchId As String = selectedBatchRow.Cells("رقم الرسالة").Value.ToString()
        Dim isLotSaved As Boolean = False
        Dim hasRequiredData As Boolean = False

        Using conn As New SqlConnection(connectionString)
            conn.Open()
            Dim query As String = "SELECT COUNT(*) FROM batch_details WHERE batch_id = @batchId AND lot = @lot " &
                                "AND client_permission IS NOT NULL AND client_item_code IS NOT NULL"
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@batchId", batchId)
                cmd.Parameters.AddWithValue("@lot", lotValue)
                Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                isLotSaved = (count > 0)
            End Using
        End Using

        If Not isLotSaved Then
            MessageBox.Show("الرجاء حفظ البيانات أولاً قبل إضافة الأتواب", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        dgvRolls.Rows.Clear()

        ' Position rolls grid
        dgvRolls.Location = New Point(20, dgvBatchDetails.Bottom + 10)
        dgvRolls.Width = Me.ClientSize.Width / 2 - 30
        dgvRolls.Height = Me.ClientSize.Height - dgvRolls.Top - 60
        dgvRolls.Visible = True

        ' Add Location column if it doesn't exist
        If Not dgvRolls.Columns.Contains("Location") Then
            dgvRolls.Columns.Add("Location", "الموقع")
            dgvRolls.Columns("Location").ReadOnly = False
        End If

        ' Show weight distribution controls
        lblTotalWeight.Visible = True
        txtTotalWeight.Visible = True
        lblNumberOfRolls.Visible = True
        txtNumberOfRolls.Visible = True
        btnDivideWeight.Visible = True

        ' Position weight distribution controls - Moved more to the right
        lblTotalWeight.Location = New Point(dgvRolls.Right + 100, dgvRolls.Top + 150)
        txtTotalWeight.Location = New Point(dgvRolls.Right + 100, lblTotalWeight.Bottom + 5)
        lblNumberOfRolls.Location = New Point(dgvRolls.Right + 100, txtTotalWeight.Bottom + 10)
        txtNumberOfRolls.Location = New Point(dgvRolls.Right + 100, lblNumberOfRolls.Bottom + 5)
        btnDivideWeight.Location = New Point(dgvRolls.Right + 100, txtNumberOfRolls.Bottom + 10)

        ' Position and show read weight button
        btnReadWeight.Size = New Size(120, 35)
        btnReadWeight.Location = New Point(dgvRolls.Right + 10, dgvRolls.Top + 40)
        btnReadWeight.Text = "قراءة الوزن"
        btnReadWeight.Font = New Font("Arial", 12, FontStyle.Bold)
        btnReadWeight.BackColor = Color.FromArgb(0, 120, 215)
        btnReadWeight.ForeColor = Color.White
        btnReadWeight.FlatStyle = FlatStyle.Flat
        btnReadWeight.RightToLeft = RightToLeft.Yes
        btnReadWeight.BringToFront()
        btnReadWeight.Visible = True
        btnReadWeight.Enabled = True ' Always enable the Read Weight button

        ' Position and show location label and ComboBox
        lblLocation.Text = "الموقع:"
        lblLocation.Size = New Size(60, 25)
        lblLocation.Location = New Point(dgvRolls.Right + 10, btnDivideWeight.Bottom + 20) ' Moved below weight distribution controls
        lblLocation.Font = New Font("Arial", 12, FontStyle.Bold)
        lblLocation.TextAlign = ContentAlignment.MiddleRight
        lblLocation.Visible = True
        Me.Controls.Add(lblLocation)

        cmbLocation.Size = New Size(200, 25)
        cmbLocation.Location = New Point(dgvRolls.Right + 80, btnDivideWeight.Bottom + 20) ' Moved below weight distribution controls
        cmbLocation.Font = New Font("Arial", 12)
        cmbLocation.DropDownStyle = ComboBoxStyle.DropDown
        cmbLocation.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cmbLocation.AutoCompleteSource = AutoCompleteSource.ListItems
        cmbLocation.Visible = True
        Me.Controls.Add(cmbLocation)

        ' Load locations
        LoadLocations()

        AddNewRollRow()

        ' Force a form resize to ensure everything is positioned correctly
        Form_Resize(Nothing, Nothing)
    End Sub

    Private Sub LoadLocations()
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim query As String = "SELECT location FROM store_location ORDER BY location"
                Using cmd As New SqlCommand(query, conn)
                    Dim dt As New DataTable()
                    dt.Load(cmd.ExecuteReader())

                    ' إعداد مصدر البيانات للقائمة المنسدلة
                    cmbLocation.DataSource = dt
                    cmbLocation.DisplayMember = "location"
                    cmbLocation.ValueMember = "location"
                    cmbLocation.SelectedIndex = -1

                    ' إضافة قائمة الاقتراحات التلقائية
                    Dim autoCompleteCollection As New AutoCompleteStringCollection()
                    For Each row As DataRow In dt.Rows
                        autoCompleteCollection.Add(row("location").ToString())
                    Next
                    cmbLocation.AutoCompleteCustomSource = autoCompleteCollection
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل المواقع: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub dgvRolls_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) Handles dgvRolls.CellValidating
        ' Only validate the Weight column
        If e.ColumnIndex = dgvRolls.Columns("Weight").Index Then
            Dim newValue As String = e.FormattedValue.ToString()

            ' Skip validation if cell is empty
            If String.IsNullOrEmpty(newValue) Then
                Return
            End If

            Dim weight As Decimal
            If Not Decimal.TryParse(newValue, weight) Then
                e.Cancel = True
                MessageBox.Show("الرجاء إدخال قيمة رقمية صحيحة للوزن", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            ' Validate weight range (0.1 kg to 500 kg)
            If weight < 0.1D OrElse weight > 500D Then
                e.Cancel = True
                MessageBox.Show($"الوزن {weight.ToString("F3")} كجم خارج النطاق المسموح به (0.1 كجم إلى 500 كجم)", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Format the weight to 3 decimal places
            dgvRolls.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = weight.ToString("F3")
        End If
    End Sub

    Private Sub dgvRolls_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvRolls.CellEndEdit
        If e.ColumnIndex = dgvRolls.Columns("Weight").Index AndAlso e.RowIndex >= 0 Then
            ' Reset ReadOnly state after manual entry
            If isScaleConnected Then
                dgvRolls.Columns("Weight").ReadOnly = False
            End If

            Dim row As DataGridViewRow = dgvRolls.Rows(e.RowIndex)
            Dim weightValue As String = If(row.Cells("Weight").Value?.ToString(), "")

            If Not String.IsNullOrEmpty(weightValue) Then
                ' Print label automatically after manual entry
                PrintLabel(row)

                ' Move to the Weight cell of the next row
                If e.RowIndex < dgvRolls.Rows.Count - 1 Then
                    dgvRolls.CurrentCell = dgvRolls.Rows(e.RowIndex + 1).Cells("Weight")
                End If
            End If
        End If
    End Sub

    Private Sub dgvRolls_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles dgvRolls.CellBeginEdit
        ' Allow manual entry if the user has explicitly chosen to enter weight manually
        If e.ColumnIndex = dgvRolls.Columns("Weight").Index AndAlso dgvRolls.Columns("Weight").ReadOnly Then
            e.Cancel = True
            MessageBox.Show("الرجاء استخدام زر قراءة الوزن لإدخال الوزن.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub btnDivideWeight_Click(sender As Object, e As EventArgs)
        Try
            ' Check user access level
            If UserAccessLevel <> 1 Then
                MessageBox.Show("ليس لديك صلاحية لتقسيم الأتواب. هذه الصلاحية متاحة فقط للمستخدمين ذوي المستوى 1", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Check if location is selected
            If String.IsNullOrEmpty(cmbLocation.Text) Then
                MessageBox.Show("الرجاء تحديد الموقع أولاً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Validate inputs
            If String.IsNullOrWhiteSpace(txtTotalWeight.Text) OrElse String.IsNullOrWhiteSpace(txtNumberOfRolls.Text) Then
                MessageBox.Show("الرجاء إدخال الوزن الكلي وعدد الأتواب", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim totalWeight As Decimal
            Dim numberOfRolls As Integer

            If Not Decimal.TryParse(txtTotalWeight.Text, totalWeight) Then
                MessageBox.Show("الرجاء إدخال قيمة صحيحة للوزن الكلي", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            If Not Integer.TryParse(txtNumberOfRolls.Text, numberOfRolls) OrElse numberOfRolls <= 0 Then
                MessageBox.Show("الرجاء إدخال عدد صحيح موجب للأتواب", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Get the current roll number from the selected row
            Dim startRollNumber As Integer = 1
            If dgvRolls.CurrentRow IsNot Nothing AndAlso Not dgvRolls.CurrentRow.IsNewRow Then
                If Integer.TryParse(dgvRolls.CurrentRow.Cells("RollNumber").Value.ToString(), startRollNumber) Then
                    ' Keep the current roll number as starting point
                End If
            End If

            ' Calculate weight per roll
            Dim weightPerRoll As Decimal = totalWeight / numberOfRolls

            ' Clear existing rows
            dgvRolls.Rows.Clear()

            ' Add rows with calculated weights starting from the current roll number
            For i As Integer = 0 To numberOfRolls - 1
                ' Add new row
                dgvRolls.Rows.Add()
                ' Get the newly added row
                Dim newRow As DataGridViewRow = dgvRolls.Rows(dgvRolls.Rows.Count - 1)
                ' Set values for the new row
                newRow.Cells("RollNumber").Value = startRollNumber + i
                newRow.Cells("Weight").Value = weightPerRoll.ToString("F3")
                newRow.Cells("Location").Value = cmbLocation.Text
            Next

            MessageBox.Show("تم تقسيم الوزن بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء تقسيم الوزن: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SaveRollsToDatabase()
        Try
            Dim success As Boolean = False

            ' Get the selected batch row
            Dim selectedBatchRow As DataGridViewRow = Nothing
            For Each row As DataGridViewRow In dgvBatchDetails.Rows
                If CBool(row.Cells("SelectRow").Value) Then
                    selectedBatchRow = row
                    Exit For
                End If
            Next

            If selectedBatchRow Is Nothing Then
                MessageBox.Show("الرجاء تحديد صف أولاً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Get batch information
            Dim batchId As String = selectedBatchRow.Cells("رقم الرسالة").Value.ToString()
            Dim lotValue As String = selectedBatchRow.Cells("lot").Value.ToString()

            ' Save each roll to database
            Using connection As New SqlConnection(connectionString)
                connection.Open()

                For Each row As DataGridViewRow In dgvRolls.Rows
                    If Not row.IsNewRow Then
                        Dim rollNumber As String = row.Cells("RollNumber").Value.ToString()
                        Dim weight As Decimal = Decimal.Parse(row.Cells("Weight").Value.ToString())

                        ' Insert roll data
                        Using command As New SqlCommand("INSERT INTO Rolls (BatchId, Lot, RollNumber, Weight) VALUES (@BatchId, @Lot, @RollNumber, @Weight)", connection)
                            command.Parameters.AddWithValue("@BatchId", batchId)
                            command.Parameters.AddWithValue("@Lot", lotValue)
                            command.Parameters.AddWithValue("@RollNumber", rollNumber)
                            command.Parameters.AddWithValue("@Weight", weight)
                            command.ExecuteNonQuery()
                        End Using
                    End If
                Next

                success = True
            End Using

            If success Then
                MessageBox.Show("تم حفظ الأتواب بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)
                dgvRolls.Rows.Clear()
                dgvRolls.Visible = False
                btnSaveRolls.Visible = False

                ' Hide weight distribution controls
                lblTotalWeight.Visible = False
                txtTotalWeight.Visible = False
                lblNumberOfRolls.Visible = False
                txtNumberOfRolls.Visible = False
                btnDivideWeight.Visible = False



                ' Clear the input fields
                txtTotalWeight.Text = ""
                txtNumberOfRolls.Text = ""
            End If

        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء حفظ الأتواب: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

End Class