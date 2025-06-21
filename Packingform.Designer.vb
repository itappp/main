<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class packingform
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
        Me.Label7 = New System.Windows.Forms.Label()
        Me.cmpclientto = New System.Windows.Forms.ComboBox()
        Me.lblrol = New System.Windows.Forms.Label()
        Me.lblweight = New System.Windows.Forms.Label()
        Me.lblmeter = New System.Windows.Forms.Label()
        Me.lblroll = New System.Windows.Forms.Label()
        Me.lbltotalw = New System.Windows.Forms.Label()
        Me.lbltotalstockw = New System.Windows.Forms.Label()
        Me.btnInsert = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtbatch = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.lbltotal = New System.Windows.Forms.Label()
        Me.lbltotalstock = New System.Windows.Forms.Label()
        Me.cmbClient = New System.Windows.Forms.ComboBox()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtContractNo = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.dtptodate = New System.Windows.Forms.DateTimePicker()
        Me.dtpfromdate = New System.Windows.Forms.DateTimePicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtworderid = New System.Windows.Forms.TextBox()
        Me.btnview = New System.Windows.Forms.Button()
        Me.lblUsername = New System.Windows.Forms.Label()
        Me.dgvresults = New System.Windows.Forms.DataGridView()
        CType(Me.dgvresults, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(1096, 17)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(83, 21)
        Me.Label7.TabIndex = 79
        Me.Label7.Text = "شحن الى"
        '
        'cmpclientto
        '
        Me.cmpclientto.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmpclientto.FormattingEnabled = True
        Me.cmpclientto.Location = New System.Drawing.Point(822, 17)
        Me.cmpclientto.Name = "cmpclientto"
        Me.cmpclientto.Size = New System.Drawing.Size(254, 32)
        Me.cmpclientto.TabIndex = 78
        '
        'lblrol
        '
        Me.lblrol.AutoSize = True
        Me.lblrol.Location = New System.Drawing.Point(998, 209)
        Me.lblrol.Name = "lblrol"
        Me.lblrol.Size = New System.Drawing.Size(37, 21)
        Me.lblrol.TabIndex = 77
        Me.lblrol.Text = "توب"
        '
        'lblweight
        '
        Me.lblweight.AutoSize = True
        Me.lblweight.Location = New System.Drawing.Point(998, 175)
        Me.lblweight.Name = "lblweight"
        Me.lblweight.Size = New System.Drawing.Size(34, 21)
        Me.lblweight.TabIndex = 76
        Me.lblweight.Text = "وزن"
        '
        'lblmeter
        '
        Me.lblmeter.AutoSize = True
        Me.lblmeter.Location = New System.Drawing.Point(998, 147)
        Me.lblmeter.Name = "lblmeter"
        Me.lblmeter.Size = New System.Drawing.Size(32, 21)
        Me.lblmeter.TabIndex = 75
        Me.lblmeter.Text = "متر"
        '
        'lblroll
        '
        Me.lblroll.AutoSize = True
        Me.lblroll.Location = New System.Drawing.Point(696, 200)
        Me.lblroll.Name = "lblroll"
        Me.lblroll.Size = New System.Drawing.Size(33, 21)
        Me.lblroll.TabIndex = 74
        Me.lblroll.Text = "roll"
        '
        'lbltotalw
        '
        Me.lbltotalw.AutoSize = True
        Me.lbltotalw.Location = New System.Drawing.Point(696, 173)
        Me.lbltotalw.Name = "lbltotalw"
        Me.lbltotalw.Size = New System.Drawing.Size(123, 21)
        Me.lbltotalw.TabIndex = 73
        Me.lbltotalw.Text = "رصيد العميل وزن"
        '
        'lbltotalstockw
        '
        Me.lbltotalstockw.AutoSize = True
        Me.lbltotalstockw.Location = New System.Drawing.Point(220, 219)
        Me.lbltotalstockw.Name = "lbltotalstockw"
        Me.lbltotalstockw.Size = New System.Drawing.Size(127, 21)
        Me.lbltotalstockw.TabIndex = 72
        Me.lbltotalstockw.Text = "رصيد المخزن وزن"
        '
        'btnInsert
        '
        Me.btnInsert.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnInsert.Location = New System.Drawing.Point(881, 64)
        Me.btnInsert.Name = "btnInsert"
        Me.btnInsert.Size = New System.Drawing.Size(122, 38)
        Me.btnInsert.TabIndex = 71
        Me.btnInsert.Text = "شحن"
        Me.btnInsert.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(65, 64)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(60, 21)
        Me.Label6.TabIndex = 70
        Me.Label6.Text = "رسالة"
        '
        'txtbatch
        '
        Me.txtbatch.Location = New System.Drawing.Point(150, 62)
        Me.txtbatch.Name = "txtbatch"
        Me.txtbatch.Size = New System.Drawing.Size(212, 28)
        Me.txtbatch.TabIndex = 69
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(20, 99)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(96, 21)
        Me.Label5.TabIndex = 68
        Me.Label5.Text = "كود العميل"
        '
        'lbltotal
        '
        Me.lbltotal.AutoSize = True
        Me.lbltotal.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbltotal.Location = New System.Drawing.Point(715, 144)
        Me.lbltotal.Name = "lbltotal"
        Me.lbltotal.Size = New System.Drawing.Size(121, 21)
        Me.lbltotal.TabIndex = 67
        Me.lbltotal.Text = "رصيد العميل متر"
        '
        'lbltotalstock
        '
        Me.lbltotalstock.AutoSize = True
        Me.lbltotalstock.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbltotalstock.Location = New System.Drawing.Point(239, 185)
        Me.lbltotalstock.Name = "lbltotalstock"
        Me.lbltotalstock.Size = New System.Drawing.Size(125, 21)
        Me.lbltotalstock.TabIndex = 66
        Me.lbltotalstock.Text = "رصيد المخزن متر"
        '
        'cmbClient
        '
        Me.cmbClient.FormattingEnabled = True
        Me.cmbClient.Location = New System.Drawing.Point(150, 100)
        Me.cmbClient.Name = "cmbClient"
        Me.cmbClient.Size = New System.Drawing.Size(212, 29)
        Me.cmbClient.TabIndex = 65
        '
        'btnSearch
        '
        Me.btnSearch.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSearch.Location = New System.Drawing.Point(401, 160)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(154, 39)
        Me.btnSearch.TabIndex = 64
        Me.btnSearch.Text = "بحث"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(25, 141)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(98, 21)
        Me.Label4.TabIndex = 63
        Me.Label4.Text = "رقم التعاقد"
        '
        'txtContractNo
        '
        Me.txtContractNo.Location = New System.Drawing.Point(150, 141)
        Me.txtContractNo.Name = "txtContractNo"
        Me.txtContractNo.Size = New System.Drawing.Size(214, 28)
        Me.txtContractNo.TabIndex = 62
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(456, 67)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(31, 21)
        Me.Label3.TabIndex = 61
        Me.Label3.Text = "To"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(427, 10)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(54, 21)
        Me.Label2.TabIndex = 60
        Me.Label2.Text = "From"
        '
        'dtptodate
        '
        Me.dtptodate.Location = New System.Drawing.Point(504, 67)
        Me.dtptodate.Name = "dtptodate"
        Me.dtptodate.Size = New System.Drawing.Size(253, 28)
        Me.dtptodate.TabIndex = 59
        '
        'dtpfromdate
        '
        Me.dtpfromdate.Location = New System.Drawing.Point(504, 14)
        Me.dtpfromdate.Name = "dtpfromdate"
        Me.dtpfromdate.Size = New System.Drawing.Size(253, 28)
        Me.dtpfromdate.TabIndex = 58
        Me.dtpfromdate.Value = New Date(2024, 1, 1, 0, 0, 0, 0)
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(37, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(82, 21)
        Me.Label1.TabIndex = 57
        Me.Label1.Text = "أمر شغل"
        '
        'txtworderid
        '
        Me.txtworderid.Location = New System.Drawing.Point(150, 17)
        Me.txtworderid.Name = "txtworderid"
        Me.txtworderid.Size = New System.Drawing.Size(214, 28)
        Me.txtworderid.TabIndex = 56
        '
        'btnview
        '
        Me.btnview.Location = New System.Drawing.Point(17, 176)
        Me.btnview.Name = "btnview"
        Me.btnview.Size = New System.Drawing.Size(79, 38)
        Me.btnview.TabIndex = 55
        Me.btnview.Text = "view"
        Me.btnview.UseVisualStyleBackColor = True
        '
        'lblUsername
        '
        Me.lblUsername.AutoSize = True
        Me.lblUsername.Location = New System.Drawing.Point(25, 217)
        Me.lblUsername.Name = "lblUsername"
        Me.lblUsername.Size = New System.Drawing.Size(44, 21)
        Me.lblUsername.TabIndex = 54
        Me.lblUsername.Text = "User"
        '
        'dgvresults
        '
        Me.dgvresults.AllowUserToAddRows = False
        Me.dgvresults.AllowUserToDeleteRows = False
        Me.dgvresults.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dgvresults.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvresults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvresults.Location = New System.Drawing.Point(13, 249)
        Me.dgvresults.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvresults.Name = "dgvresults"
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ActiveCaption
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvresults.RowHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvresults.RowTemplate.Height = 26
        Me.dgvresults.Size = New System.Drawing.Size(1506, 497)
        Me.dgvresults.TabIndex = 53
        '
        'packingform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1548, 819)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.cmpclientto)
        Me.Controls.Add(Me.lblrol)
        Me.Controls.Add(Me.lblweight)
        Me.Controls.Add(Me.lblmeter)
        Me.Controls.Add(Me.lblroll)
        Me.Controls.Add(Me.lbltotalw)
        Me.Controls.Add(Me.lbltotalstockw)
        Me.Controls.Add(Me.btnInsert)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtbatch)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.lbltotal)
        Me.Controls.Add(Me.lbltotalstock)
        Me.Controls.Add(Me.cmbClient)
        Me.Controls.Add(Me.btnSearch)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtContractNo)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.dtptodate)
        Me.Controls.Add(Me.dtpfromdate)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtworderid)
        Me.Controls.Add(Me.btnview)
        Me.Controls.Add(Me.lblUsername)
        Me.Controls.Add(Me.dgvresults)
        Me.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "packingform"
        Me.Text = "Packingform"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.dgvresults, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents cmpclientto As System.Windows.Forms.ComboBox
    Friend WithEvents lblrol As System.Windows.Forms.Label
    Friend WithEvents lblweight As System.Windows.Forms.Label
    Friend WithEvents lblmeter As System.Windows.Forms.Label
    Friend WithEvents lblroll As System.Windows.Forms.Label
    Friend WithEvents lbltotalw As System.Windows.Forms.Label
    Friend WithEvents lbltotalstockw As System.Windows.Forms.Label
    Friend WithEvents btnInsert As System.Windows.Forms.Button
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtbatch As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents lbltotal As System.Windows.Forms.Label
    Friend WithEvents lbltotalstock As System.Windows.Forms.Label
    Friend WithEvents cmbClient As System.Windows.Forms.ComboBox
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtContractNo As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents dtptodate As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpfromdate As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtworderid As System.Windows.Forms.TextBox
    Friend WithEvents btnview As System.Windows.Forms.Button
    Friend WithEvents lblUsername As System.Windows.Forms.Label
    Friend WithEvents dgvresults As System.Windows.Forms.DataGridView
End Class
