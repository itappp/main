<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class salesprintpackingform
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
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.lbltotalw = New System.Windows.Forms.Label()
        Me.lbltotalstockw = New System.Windows.Forms.Label()
        Me.btnprint = New System.Windows.Forms.Button()
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
        Me.lblroll = New System.Windows.Forms.Label()
        Me.cmbProductName = New System.Windows.Forms.ComboBox()
        Me.cmbColor = New System.Windows.Forms.ComboBox()
        Me.btnselectall = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.lblSumHeightWeight = New System.Windows.Forms.Label()
        CType(Me.dgvresults, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lbltotalw
        '
        Me.lbltotalw.AutoSize = True
        Me.lbltotalw.Location = New System.Drawing.Point(536, 188)
        Me.lbltotalw.Name = "lbltotalw"
        Me.lbltotalw.Size = New System.Drawing.Size(103, 17)
        Me.lbltotalw.TabIndex = 67
        Me.lbltotalw.Text = "رصيد العميل وزن"
        '
        'lbltotalstockw
        '
        Me.lbltotalstockw.AutoSize = True
        Me.lbltotalstockw.Location = New System.Drawing.Point(265, 217)
        Me.lbltotalstockw.Name = "lbltotalstockw"
        Me.lbltotalstockw.Size = New System.Drawing.Size(106, 17)
        Me.lbltotalstockw.TabIndex = 66
        Me.lbltotalstockw.Text = "رصيد المخزن وزن"
        '
        'btnprint
        '
        Me.btnprint.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnprint.Location = New System.Drawing.Point(1147, 156)
        Me.btnprint.Name = "btnprint"
        Me.btnprint.Size = New System.Drawing.Size(122, 38)
        Me.btnprint.TabIndex = 65
        Me.btnprint.Text = "Print"
        Me.btnprint.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(92, 143)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(60, 21)
        Me.Label6.TabIndex = 64
        Me.Label6.Text = "رسالة"
        '
        'txtbatch
        '
        Me.txtbatch.Location = New System.Drawing.Point(177, 141)
        Me.txtbatch.Name = "txtbatch"
        Me.txtbatch.Size = New System.Drawing.Size(212, 24)
        Me.txtbatch.TabIndex = 63
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(47, 97)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(96, 21)
        Me.Label5.TabIndex = 62
        Me.Label5.Text = "كود العميل"
        '
        'lbltotal
        '
        Me.lbltotal.AutoSize = True
        Me.lbltotal.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbltotal.Location = New System.Drawing.Point(550, 156)
        Me.lbltotal.Name = "lbltotal"
        Me.lbltotal.Size = New System.Drawing.Size(121, 21)
        Me.lbltotal.TabIndex = 61
        Me.lbltotal.Text = "رصيد العميل متر"
        '
        'lbltotalstock
        '
        Me.lbltotalstock.AutoSize = True
        Me.lbltotalstock.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbltotalstock.Location = New System.Drawing.Point(264, 180)
        Me.lbltotalstock.Name = "lbltotalstock"
        Me.lbltotalstock.Size = New System.Drawing.Size(125, 21)
        Me.lbltotalstock.TabIndex = 60
        Me.lbltotalstock.Text = "رصيد المخزن متر"
        '
        'cmbClient
        '
        Me.cmbClient.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbClient.FormattingEnabled = True
        Me.cmbClient.Location = New System.Drawing.Point(177, 98)
        Me.cmbClient.Name = "cmbClient"
        Me.cmbClient.Size = New System.Drawing.Size(212, 29)
        Me.cmbClient.TabIndex = 59
        '
        'btnSearch
        '
        Me.btnSearch.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSearch.Location = New System.Drawing.Point(539, 95)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(154, 39)
        Me.btnSearch.TabIndex = 58
        Me.btnSearch.Text = "بحث"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(50, 53)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(98, 21)
        Me.Label4.TabIndex = 57
        Me.Label4.Text = "رقم التعاقد"
        '
        'txtContractNo
        '
        Me.txtContractNo.Location = New System.Drawing.Point(175, 53)
        Me.txtContractNo.Name = "txtContractNo"
        Me.txtContractNo.Size = New System.Drawing.Size(214, 24)
        Me.txtContractNo.TabIndex = 56
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(456, 65)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(31, 21)
        Me.Label3.TabIndex = 55
        Me.Label3.Text = "To"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(427, 8)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(54, 21)
        Me.Label2.TabIndex = 54
        Me.Label2.Text = "From"
        '
        'dtptodate
        '
        Me.dtptodate.Location = New System.Drawing.Point(504, 65)
        Me.dtptodate.Name = "dtptodate"
        Me.dtptodate.Size = New System.Drawing.Size(253, 24)
        Me.dtptodate.TabIndex = 53
        '
        'dtpfromdate
        '
        Me.dtpfromdate.Location = New System.Drawing.Point(504, 12)
        Me.dtpfromdate.Name = "dtpfromdate"
        Me.dtpfromdate.Size = New System.Drawing.Size(253, 24)
        Me.dtpfromdate.TabIndex = 52
        Me.dtpfromdate.Value = New Date(2024, 1, 1, 0, 0, 0, 0)
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(62, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(82, 21)
        Me.Label1.TabIndex = 51
        Me.Label1.Text = "أمر شغل"
        '
        'txtworderid
        '
        Me.txtworderid.Location = New System.Drawing.Point(175, 14)
        Me.txtworderid.Name = "txtworderid"
        Me.txtworderid.Size = New System.Drawing.Size(214, 24)
        Me.txtworderid.TabIndex = 50
        '
        'btnview
        '
        Me.btnview.Location = New System.Drawing.Point(21, 174)
        Me.btnview.Name = "btnview"
        Me.btnview.Size = New System.Drawing.Size(79, 38)
        Me.btnview.TabIndex = 49
        Me.btnview.Text = "view"
        Me.btnview.UseVisualStyleBackColor = True
        '
        'lblUsername
        '
        Me.lblUsername.AutoSize = True
        Me.lblUsername.Location = New System.Drawing.Point(18, 17)
        Me.lblUsername.Name = "lblUsername"
        Me.lblUsername.Size = New System.Drawing.Size(35, 17)
        Me.lblUsername.TabIndex = 48
        Me.lblUsername.Text = "User"
        '
        'dgvresults
        '
        Me.dgvresults.AllowUserToAddRows = False
        Me.dgvresults.AllowUserToDeleteRows = False
        Me.dgvresults.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dgvresults.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvresults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvresults.Location = New System.Drawing.Point(13, 237)
        Me.dgvresults.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvresults.Name = "dgvresults"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ActiveCaption
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Tahoma", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvresults.RowHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.dgvresults.RowTemplate.Height = 26
        Me.dgvresults.Size = New System.Drawing.Size(1800, 665)
        Me.dgvresults.TabIndex = 47
        '
        'lblroll
        '
        Me.lblroll.AutoSize = True
        Me.lblroll.Location = New System.Drawing.Point(524, 216)
        Me.lblroll.Name = "lblroll"
        Me.lblroll.Size = New System.Drawing.Size(28, 17)
        Me.lblroll.TabIndex = 68
        Me.lblroll.Text = "رول"
        '
        'cmbProductName
        '
        Me.cmbProductName.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbProductName.FormattingEnabled = True
        Me.cmbProductName.Location = New System.Drawing.Point(933, 18)
        Me.cmbProductName.Name = "cmbProductName"
        Me.cmbProductName.Size = New System.Drawing.Size(203, 29)
        Me.cmbProductName.TabIndex = 69
        '
        'cmbColor
        '
        Me.cmbColor.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbColor.FormattingEnabled = True
        Me.cmbColor.Location = New System.Drawing.Point(933, 80)
        Me.cmbColor.Name = "cmbColor"
        Me.cmbColor.Size = New System.Drawing.Size(203, 29)
        Me.cmbColor.TabIndex = 70
        '
        'btnselectall
        '
        Me.btnselectall.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnselectall.Location = New System.Drawing.Point(967, 130)
        Me.btnselectall.Name = "btnselectall"
        Me.btnselectall.Size = New System.Drawing.Size(123, 34)
        Me.btnselectall.TabIndex = 71
        Me.btnselectall.Text = "Select"
        Me.btnselectall.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(832, 17)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(61, 21)
        Me.Label7.TabIndex = 72
        Me.Label7.Text = "الخامة"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(832, 88)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(51, 21)
        Me.Label8.TabIndex = 73
        Me.Label8.Text = "اللون"
        '
        'lblSumHeightWeight
        '
        Me.lblSumHeightWeight.AutoSize = True
        Me.lblSumHeightWeight.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSumHeightWeight.Location = New System.Drawing.Point(1361, 14)
        Me.lblSumHeightWeight.Name = "lblSumHeightWeight"
        Me.lblSumHeightWeight.Size = New System.Drawing.Size(58, 21)
        Me.lblSumHeightWeight.TabIndex = 74
        Me.lblSumHeightWeight.Text = "Label9"
        '
        'salesprintpackingform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1826, 936)
        Me.Controls.Add(Me.lblSumHeightWeight)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.btnselectall)
        Me.Controls.Add(Me.cmbColor)
        Me.Controls.Add(Me.cmbProductName)
        Me.Controls.Add(Me.lblroll)
        Me.Controls.Add(Me.lbltotalw)
        Me.Controls.Add(Me.lbltotalstockw)
        Me.Controls.Add(Me.btnprint)
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
        Me.Name = "salesprintpackingform"
        Me.Text = "salesprintpackingform"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.dgvresults, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lbltotalw As System.Windows.Forms.Label
    Friend WithEvents lbltotalstockw As System.Windows.Forms.Label
    Friend WithEvents btnprint As System.Windows.Forms.Button
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
    Friend WithEvents lblroll As System.Windows.Forms.Label
    Friend WithEvents cmbProductName As System.Windows.Forms.ComboBox
    Friend WithEvents cmbColor As System.Windows.Forms.ComboBox
    Friend WithEvents btnselectall As System.Windows.Forms.Button
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents lblSumHeightWeight As System.Windows.Forms.Label
End Class
