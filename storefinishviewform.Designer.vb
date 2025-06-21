<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class storefinishviewform
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
        Me.btnExport = New System.Windows.Forms.Button()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtContractNo = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.dtptodate = New System.Windows.Forms.DateTimePicker()
        Me.dtpfromdate = New System.Windows.Forms.DateTimePicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtworderid = New System.Windows.Forms.TextBox()
        Me.cmbClient = New System.Windows.Forms.ComboBox()
        Me.lbltotalstock = New System.Windows.Forms.Label()
        Me.lbltotal = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtbatch = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cmbref = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.dgvResults = New System.Windows.Forms.DataGridView()
        CType(Me.dgvResults, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(591, 97)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(122, 55)
        Me.btnExport.TabIndex = 21
        Me.btnExport.Text = "Export To Excell"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSearch.Location = New System.Drawing.Point(772, 112)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(139, 37)
        Me.btnSearch.TabIndex = 20
        Me.btnSearch.Text = "بحث"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(7, 66)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(98, 21)
        Me.Label4.TabIndex = 19
        Me.Label4.Text = "رقم التعاقد"
        '
        'txtContractNo
        '
        Me.txtContractNo.Location = New System.Drawing.Point(119, 66)
        Me.txtContractNo.Name = "txtContractNo"
        Me.txtContractNo.Size = New System.Drawing.Size(193, 26)
        Me.txtContractNo.TabIndex = 18
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(920, 55)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(31, 21)
        Me.Label3.TabIndex = 17
        Me.Label3.Text = "To"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(894, 8)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(54, 21)
        Me.Label2.TabIndex = 16
        Me.Label2.Text = "From"
        '
        'dtptodate
        '
        Me.dtptodate.Location = New System.Drawing.Point(963, 55)
        Me.dtptodate.Name = "dtptodate"
        Me.dtptodate.Size = New System.Drawing.Size(228, 26)
        Me.dtptodate.TabIndex = 15
        '
        'dtpfromdate
        '
        Me.dtpfromdate.Location = New System.Drawing.Point(963, 12)
        Me.dtpfromdate.Name = "dtpfromdate"
        Me.dtpfromdate.Size = New System.Drawing.Size(228, 26)
        Me.dtpfromdate.TabIndex = 14
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(18, 11)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(82, 21)
        Me.Label1.TabIndex = 12
        Me.Label1.Text = "أمر شغل"
        '
        'txtworderid
        '
        Me.txtworderid.Location = New System.Drawing.Point(119, 8)
        Me.txtworderid.Name = "txtworderid"
        Me.txtworderid.Size = New System.Drawing.Size(193, 26)
        Me.txtworderid.TabIndex = 11
        '
        'cmbClient
        '
        Me.cmbClient.FormattingEnabled = True
        Me.cmbClient.Location = New System.Drawing.Point(535, 8)
        Me.cmbClient.Name = "cmbClient"
        Me.cmbClient.Size = New System.Drawing.Size(191, 26)
        Me.cmbClient.TabIndex = 22
        '
        'lbltotalstock
        '
        Me.lbltotalstock.AutoSize = True
        Me.lbltotalstock.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbltotalstock.Location = New System.Drawing.Point(1054, 84)
        Me.lbltotalstock.Name = "lbltotalstock"
        Me.lbltotalstock.Size = New System.Drawing.Size(113, 21)
        Me.lbltotalstock.TabIndex = 23
        Me.lbltotalstock.Text = "رصيد المخزن"
        '
        'lbltotal
        '
        Me.lbltotal.AutoSize = True
        Me.lbltotal.Font = New System.Drawing.Font("Tahoma", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbltotal.Location = New System.Drawing.Point(1077, 136)
        Me.lbltotal.Name = "lbltotal"
        Me.lbltotal.Size = New System.Drawing.Size(86, 17)
        Me.lbltotal.TabIndex = 24
        Me.lbltotal.Text = "رصيد العميل"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(418, 7)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(96, 21)
        Me.Label5.TabIndex = 25
        Me.Label5.Text = "كود العميل"
        '
        'txtbatch
        '
        Me.txtbatch.Location = New System.Drawing.Point(535, 63)
        Me.txtbatch.Name = "txtbatch"
        Me.txtbatch.Size = New System.Drawing.Size(178, 26)
        Me.txtbatch.TabIndex = 26
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(459, 65)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(60, 21)
        Me.Label6.TabIndex = 27
        Me.Label6.Text = "رسالة"
        '
        'cmbref
        '
        Me.cmbref.FormattingEnabled = True
        Me.cmbref.Location = New System.Drawing.Point(119, 112)
        Me.cmbref.Name = "cmbref"
        Me.cmbref.Size = New System.Drawing.Size(193, 26)
        Me.cmbref.TabIndex = 28
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(21, 110)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(80, 24)
        Me.Label7.TabIndex = 29
        Me.Label7.Text = "اذن البيع"
        '
        'dgvResults
        '
        Me.dgvResults.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dgvResults.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvResults.Location = New System.Drawing.Point(11, 186)
        Me.dgvResults.Name = "dgvResults"
        Me.dgvResults.RowTemplate.Height = 26
        Me.dgvResults.Size = New System.Drawing.Size(1263, 574)
        Me.dgvResults.TabIndex = 30
        '
        'storefinishviewform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 18.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1588, 1007)
        Me.Controls.Add(Me.dgvResults)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.cmbref)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtbatch)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.lbltotal)
        Me.Controls.Add(Me.lbltotalstock)
        Me.Controls.Add(Me.cmbClient)
        Me.Controls.Add(Me.btnExport)
        Me.Controls.Add(Me.btnSearch)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtContractNo)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.dtptodate)
        Me.Controls.Add(Me.dtpfromdate)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtworderid)
        Me.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "storefinishviewform"
        Me.Text = "تقرير مخزن المجهز"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.dgvResults, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtContractNo As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents dtptodate As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpfromdate As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtworderid As System.Windows.Forms.TextBox
    Friend WithEvents cmbClient As System.Windows.Forms.ComboBox
    Friend WithEvents lbltotalstock As System.Windows.Forms.Label
    Friend WithEvents lbltotal As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtbatch As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cmbref As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents dgvResults As System.Windows.Forms.DataGridView
End Class
