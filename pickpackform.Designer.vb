<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class pickpackform
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.dgvresults = New System.Windows.Forms.DataGridView()
        Me.lblUsername = New System.Windows.Forms.Label()
        Me.btnview = New System.Windows.Forms.Button()
        Me.lbltotalstock = New System.Windows.Forms.Label()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.dtptodate = New System.Windows.Forms.DateTimePicker()
        Me.dtpfromdate = New System.Windows.Forms.DateTimePicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtworderid = New System.Windows.Forms.TextBox()
        Me.btnInsert = New System.Windows.Forms.Button()
        Me.lbltotalstockw = New System.Windows.Forms.Label()
        Me.cmpclientto = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtid = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.dgvSummary = New System.Windows.Forms.DataGridView()
        Me.dgvall = New System.Windows.Forms.DataGridView()
        Me.txtdegree = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btnUploadExcel = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtbarcode = New System.Windows.Forms.TextBox()
        CType(Me.dgvresults, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvSummary, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvall, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvresults
        '
        Me.dgvresults.AllowUserToAddRows = False
        Me.dgvresults.AllowUserToDeleteRows = False
        Me.dgvresults.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dgvresults.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvresults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvresults.Location = New System.Drawing.Point(13, 284)
        Me.dgvresults.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvresults.Name = "dgvresults"
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ActiveCaption
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Tahoma", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvresults.RowHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvresults.RowHeadersWidth = 51
        Me.dgvresults.RowTemplate.Height = 26
        Me.dgvresults.Size = New System.Drawing.Size(808, 615)
        Me.dgvresults.TabIndex = 7
        '
        'lblUsername
        '
        Me.lblUsername.AutoSize = True
        Me.lblUsername.Location = New System.Drawing.Point(131, 218)
        Me.lblUsername.Name = "lblUsername"
        Me.lblUsername.Size = New System.Drawing.Size(45, 22)
        Me.lblUsername.TabIndex = 16
        Me.lblUsername.Text = "User"
        Me.lblUsername.Visible = False
        '
        'btnview
        '
        Me.btnview.Location = New System.Drawing.Point(13, 210)
        Me.btnview.Name = "btnview"
        Me.btnview.Size = New System.Drawing.Size(79, 38)
        Me.btnview.TabIndex = 17
        Me.btnview.Text = "view"
        Me.btnview.UseVisualStyleBackColor = True
        '
        'lbltotalstock
        '
        Me.lbltotalstock.AutoSize = True
        Me.lbltotalstock.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbltotalstock.Location = New System.Drawing.Point(220, 252)
        Me.lbltotalstock.Name = "lbltotalstock"
        Me.lbltotalstock.Size = New System.Drawing.Size(125, 21)
        Me.lbltotalstock.TabIndex = 39
        Me.lbltotalstock.Text = "رصيد المخزن متر"
        '
        'btnSearch
        '
        Me.btnSearch.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSearch.Location = New System.Drawing.Point(210, 210)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(154, 39)
        Me.btnSearch.TabIndex = 36
        Me.btnSearch.Text = "بحث"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(29, 163)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(31, 21)
        Me.Label3.TabIndex = 33
        Me.Label3.Text = "To"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(0, 106)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(54, 21)
        Me.Label2.TabIndex = 32
        Me.Label2.Text = "From"
        '
        'dtptodate
        '
        Me.dtptodate.Location = New System.Drawing.Point(77, 163)
        Me.dtptodate.Name = "dtptodate"
        Me.dtptodate.Size = New System.Drawing.Size(253, 29)
        Me.dtptodate.TabIndex = 31
        '
        'dtpfromdate
        '
        Me.dtpfromdate.Location = New System.Drawing.Point(77, 110)
        Me.dtpfromdate.Name = "dtpfromdate"
        Me.dtpfromdate.Size = New System.Drawing.Size(253, 29)
        Me.dtpfromdate.TabIndex = 30
        Me.dtpfromdate.Value = New Date(2024, 1, 1, 0, 0, 0, 0)
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(82, 21)
        Me.Label1.TabIndex = 29
        Me.Label1.Text = "أمر شغل"
        '
        'txtworderid
        '
        Me.txtworderid.Location = New System.Drawing.Point(100, 12)
        Me.txtworderid.Name = "txtworderid"
        Me.txtworderid.Size = New System.Drawing.Size(245, 29)
        Me.txtworderid.TabIndex = 28
        '
        'btnInsert
        '
        Me.btnInsert.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnInsert.Location = New System.Drawing.Point(598, 11)
        Me.btnInsert.Name = "btnInsert"
        Me.btnInsert.Size = New System.Drawing.Size(122, 38)
        Me.btnInsert.TabIndex = 44
        Me.btnInsert.Text = "شحن"
        Me.btnInsert.UseVisualStyleBackColor = True
        '
        'lbltotalstockw
        '
        Me.lbltotalstockw.AutoSize = True
        Me.lbltotalstockw.Location = New System.Drawing.Point(503, 251)
        Me.lbltotalstockw.Name = "lbltotalstockw"
        Me.lbltotalstockw.Size = New System.Drawing.Size(138, 22)
        Me.lbltotalstockw.TabIndex = 45
        Me.lbltotalstockw.Text = "رصيد المخزن وزن"
        '
        'cmpclientto
        '
        Me.cmpclientto.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmpclientto.FormattingEnabled = True
        Me.cmpclientto.Location = New System.Drawing.Point(556, 100)
        Me.cmpclientto.Name = "cmpclientto"
        Me.cmpclientto.Size = New System.Drawing.Size(254, 32)
        Me.cmpclientto.TabIndex = 51
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(639, 75)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(87, 22)
        Me.Label7.TabIndex = 52
        Me.Label7.Text = "شحن الى"
        '
        'txtid
        '
        Me.txtid.Location = New System.Drawing.Point(100, 59)
        Me.txtid.Name = "txtid"
        Me.txtid.Size = New System.Drawing.Size(245, 29)
        Me.txtid.TabIndex = 53
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(23, 63)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(31, 21)
        Me.Label8.TabIndex = 54
        Me.Label8.Text = "ID"
        '
        'dgvSummary
        '
        Me.dgvSummary.AllowUserToAddRows = False
        Me.dgvSummary.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dgvSummary.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvSummary.Location = New System.Drawing.Point(846, 6)
        Me.dgvSummary.Name = "dgvSummary"
        Me.dgvSummary.RowHeadersWidth = 51
        Me.dgvSummary.RowTemplate.Height = 24
        Me.dgvSummary.Size = New System.Drawing.Size(656, 461)
        Me.dgvSummary.TabIndex = 56
        '
        'dgvall
        '
        Me.dgvall.AllowUserToAddRows = False
        Me.dgvall.AllowUserToDeleteRows = False
        Me.dgvall.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dgvall.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvall.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvall.Location = New System.Drawing.Point(846, 495)
        Me.dgvall.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvall.Name = "dgvall"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ActiveCaption
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Tahoma", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvall.RowHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.dgvall.RowHeadersWidth = 51
        Me.dgvall.RowTemplate.Height = 26
        Me.dgvall.Size = New System.Drawing.Size(640, 427)
        Me.dgvall.TabIndex = 57
        '
        'txtdegree
        '
        Me.txtdegree.Location = New System.Drawing.Point(409, 35)
        Me.txtdegree.Name = "txtdegree"
        Me.txtdegree.Size = New System.Drawing.Size(44, 29)
        Me.txtdegree.TabIndex = 58
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(405, 6)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(63, 21)
        Me.Label4.TabIndex = 59
        Me.Label4.Text = "الدرجه"
        '
        'btnUploadExcel
        '
        Me.btnUploadExcel.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnUploadExcel.Location = New System.Drawing.Point(368, 89)
        Me.btnUploadExcel.Name = "btnUploadExcel"
        Me.btnUploadExcel.Size = New System.Drawing.Size(64, 38)
        Me.btnUploadExcel.TabIndex = 60
        Me.btnUploadExcel.Text = "رفع"
        Me.btnUploadExcel.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(468, 167)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(58, 21)
        Me.Label5.TabIndex = 62
        Me.Label5.Text = "باركود"
        '
        'txtbarcode
        '
        Me.txtbarcode.Location = New System.Drawing.Point(556, 163)
        Me.txtbarcode.Name = "txtbarcode"
        Me.txtbarcode.Size = New System.Drawing.Size(265, 29)
        Me.txtbarcode.TabIndex = 61
        '
        'pickpackform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(10.0!, 22.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1555, 1030)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtbarcode)
        Me.Controls.Add(Me.btnUploadExcel)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtdegree)
        Me.Controls.Add(Me.dgvall)
        Me.Controls.Add(Me.dgvSummary)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.txtid)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.cmpclientto)
        Me.Controls.Add(Me.lbltotalstockw)
        Me.Controls.Add(Me.btnInsert)
        Me.Controls.Add(Me.lbltotalstock)
        Me.Controls.Add(Me.btnSearch)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.dtptodate)
        Me.Controls.Add(Me.dtpfromdate)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtworderid)
        Me.Controls.Add(Me.btnview)
        Me.Controls.Add(Me.lblUsername)
        Me.Controls.Add(Me.dgvresults)
        Me.Font = New System.Drawing.Font("Tahoma", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "pickpackform"
        Me.Text = "شاشة الشحن"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.dgvresults, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvSummary, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvall, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents dgvresults As System.Windows.Forms.DataGridView
    Private WithEvents lblUsername As System.Windows.Forms.Label
    Private WithEvents btnview As System.Windows.Forms.Button
    Private WithEvents lbltotalstock As System.Windows.Forms.Label
    Private WithEvents btnSearch As System.Windows.Forms.Button
    Private WithEvents Label3 As System.Windows.Forms.Label
    Private WithEvents Label2 As System.Windows.Forms.Label
    Private WithEvents dtptodate As System.Windows.Forms.DateTimePicker
    Private WithEvents dtpfromdate As System.Windows.Forms.DateTimePicker
    Private WithEvents Label1 As System.Windows.Forms.Label
    Private WithEvents txtworderid As System.Windows.Forms.TextBox
    Private WithEvents btnInsert As System.Windows.Forms.Button
    Private WithEvents lbltotalstockw As System.Windows.Forms.Label
    Private WithEvents cmpclientto As System.Windows.Forms.ComboBox
    Private WithEvents Label7 As System.Windows.Forms.Label
    Private WithEvents txtid As System.Windows.Forms.TextBox
    Private WithEvents Label8 As System.Windows.Forms.Label
    Private WithEvents dgvSummary As System.Windows.Forms.DataGridView
    Private WithEvents dgvall As System.Windows.Forms.DataGridView
    Private WithEvents txtdegree As TextBox
    Private WithEvents Label4 As Label
    Private WithEvents btnUploadExcel As Button
    Private WithEvents Label5 As Label
    Private WithEvents txtbarcode As TextBox
End Class
