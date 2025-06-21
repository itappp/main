<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class packingviewform
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
        Me.txtworderid = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.dgvResults = New System.Windows.Forms.DataGridView()
        Me.dtpfromdate = New System.Windows.Forms.DateTimePicker()
        Me.dtptodate = New System.Windows.Forms.DateTimePicker()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtContractNo = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.cmbref = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.btnprint = New System.Windows.Forms.Button()
        Me.txtrefprint = New System.Windows.Forms.TextBox()
        Me.btnprint2 = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.btnprintpacking = New System.Windows.Forms.Button()
        Me.btnprinttotal = New System.Windows.Forms.Button()
        CType(Me.dgvResults, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtworderid
        '
        Me.txtworderid.Location = New System.Drawing.Point(136, 22)
        Me.txtworderid.Margin = New System.Windows.Forms.Padding(4)
        Me.txtworderid.Name = "txtworderid"
        Me.txtworderid.Size = New System.Drawing.Size(216, 28)
        Me.txtworderid.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(23, 26)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(82, 21)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "أمر شغل"
        '
        'dgvResults
        '
        Me.dgvResults.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dgvResults.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvResults.Location = New System.Drawing.Point(15, 206)
        Me.dgvResults.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvResults.Name = "dgvResults"
        Me.dgvResults.RowHeadersWidth = 51
        Me.dgvResults.RowTemplate.Height = 26
        Me.dgvResults.Size = New System.Drawing.Size(1512, 572)
        Me.dgvResults.TabIndex = 2
        '
        'dtpfromdate
        '
        Me.dtpfromdate.Location = New System.Drawing.Point(638, 30)
        Me.dtpfromdate.Margin = New System.Windows.Forms.Padding(4)
        Me.dtpfromdate.Name = "dtpfromdate"
        Me.dtpfromdate.Size = New System.Drawing.Size(256, 28)
        Me.dtpfromdate.TabIndex = 3
        Me.dtpfromdate.Value = New Date(2024, 1, 1, 0, 0, 0, 0)
        '
        'dtptodate
        '
        Me.dtptodate.Location = New System.Drawing.Point(1093, 30)
        Me.dtptodate.Margin = New System.Windows.Forms.Padding(4)
        Me.dtptodate.Name = "dtptodate"
        Me.dtptodate.Size = New System.Drawing.Size(256, 28)
        Me.dtptodate.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(561, 26)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(54, 21)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "From"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(1045, 30)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(31, 21)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "To"
        '
        'txtContractNo
        '
        Me.txtContractNo.Location = New System.Drawing.Point(136, 91)
        Me.txtContractNo.Margin = New System.Windows.Forms.Padding(4)
        Me.txtContractNo.Name = "txtContractNo"
        Me.txtContractNo.Size = New System.Drawing.Size(216, 28)
        Me.txtContractNo.TabIndex = 7
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(10, 91)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(98, 21)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "رقم التعاقد"
        '
        'btnSearch
        '
        Me.btnSearch.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSearch.Location = New System.Drawing.Point(620, 134)
        Me.btnSearch.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(157, 43)
        Me.btnSearch.TabIndex = 9
        Me.btnSearch.Text = "بحث"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(1093, 91)
        Me.btnExport.Margin = New System.Windows.Forms.Padding(4)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(138, 64)
        Me.btnExport.TabIndex = 10
        Me.btnExport.Text = "Export To Excell"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'cmbref
        '
        Me.cmbref.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbref.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbref.FormattingEnabled = True
        Me.cmbref.Location = New System.Drawing.Point(136, 148)
        Me.cmbref.Name = "cmbref"
        Me.cmbref.Size = New System.Drawing.Size(216, 29)
        Me.cmbref.TabIndex = 11
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(30, 143)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(78, 24)
        Me.Label5.TabIndex = 12
        Me.Label5.Text = "اذن بيع"
        '
        'btnprint
        '
        Me.btnprint.Location = New System.Drawing.Point(468, 22)
        Me.btnprint.Name = "btnprint"
        Me.btnprint.Size = New System.Drawing.Size(80, 21)
        Me.btnprint.TabIndex = 13
        Me.btnprint.Text = "طباعه إجمالى"
        Me.btnprint.UseVisualStyleBackColor = True
        Me.btnprint.Visible = False
        '
        'txtrefprint
        '
        Me.txtrefprint.Location = New System.Drawing.Point(620, 84)
        Me.txtrefprint.Name = "txtrefprint"
        Me.txtrefprint.Size = New System.Drawing.Size(361, 28)
        Me.txtrefprint.TabIndex = 14
        '
        'btnprint2
        '
        Me.btnprint2.Location = New System.Drawing.Point(384, 12)
        Me.btnprint2.Name = "btnprint2"
        Me.btnprint2.Size = New System.Drawing.Size(78, 35)
        Me.btnprint2.TabIndex = 15
        Me.btnprint2.Text = "طباعه تفصيلى"
        Me.btnprint2.UseVisualStyleBackColor = True
        Me.btnprint2.Visible = False
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(480, 109)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(79, 28)
        Me.Button1.TabIndex = 15
        Me.Button1.Text = "Print 2"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'btnprintpacking
        '
        Me.btnprintpacking.Font = New System.Drawing.Font("Tahoma", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnprintpacking.Location = New System.Drawing.Point(439, 80)
        Me.btnprintpacking.Name = "btnprintpacking"
        Me.btnprintpacking.Size = New System.Drawing.Size(128, 35)
        Me.btnprintpacking.TabIndex = 16
        Me.btnprintpacking.Text = "طباعة الإذن"
        Me.btnprintpacking.UseVisualStyleBackColor = True
        '
        'btnprinttotal
        '
        Me.btnprinttotal.Font = New System.Drawing.Font("Tahoma", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnprinttotal.Location = New System.Drawing.Point(428, 134)
        Me.btnprinttotal.Name = "btnprinttotal"
        Me.btnprinttotal.Size = New System.Drawing.Size(139, 42)
        Me.btnprinttotal.TabIndex = 17
        Me.btnprinttotal.Text = "طباعة اجمالى"
        Me.btnprinttotal.UseVisualStyleBackColor = True
        '
        'packingviewform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1554, 900)
        Me.Controls.Add(Me.btnprinttotal)
        Me.Controls.Add(Me.btnprintpacking)
        Me.Controls.Add(Me.btnprint2)
        Me.Controls.Add(Me.txtrefprint)
        Me.Controls.Add(Me.btnprint)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.cmbref)
        Me.Controls.Add(Me.btnExport)
        Me.Controls.Add(Me.btnSearch)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtContractNo)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.dtptodate)
        Me.Controls.Add(Me.dtpfromdate)
        Me.Controls.Add(Me.dgvResults)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtworderid)
        Me.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "packingviewform"
        Me.Text = "تقرير الشحن"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.dgvResults, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtworderid As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents dgvResults As System.Windows.Forms.DataGridView
    Friend WithEvents dtpfromdate As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtptodate As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtContractNo As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents cmbref As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents btnprint As System.Windows.Forms.Button
    Friend WithEvents txtrefprint As System.Windows.Forms.TextBox
    Friend WithEvents btnprint2 As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents btnprintpacking As Button
    Friend WithEvents btnprinttotal As Button
End Class
