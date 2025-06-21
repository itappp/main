<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class mainqcrawtestform
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
        Me.btnadd = New System.Windows.Forms.Button()
        Me.dgvresult = New System.Windows.Forms.DataGridView()
        Me.lblUsername = New System.Windows.Forms.Label()
        Me.dataGridViewDetails = New System.Windows.Forms.DataGridView()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmblot = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbbatch = New System.Windows.Forms.ComboBox()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.dtpFromDate = New System.Windows.Forms.DateTimePicker()
        Me.dtptoDate = New System.Windows.Forms.DateTimePicker()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btnExportToExcel = New System.Windows.Forms.Button()
        Me.chkShowInitialData = New System.Windows.Forms.CheckBox()
        CType(Me.dgvresult, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dataGridViewDetails, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnadd
        '
        Me.btnadd.Location = New System.Drawing.Point(119, 158)
        Me.btnadd.Name = "btnadd"
        Me.btnadd.Size = New System.Drawing.Size(86, 29)
        Me.btnadd.TabIndex = 91
        Me.btnadd.Text = "تسجيل"
        Me.btnadd.UseVisualStyleBackColor = True
        '
        'dgvresult
        '
        Me.dgvresult.AllowUserToAddRows = False
        Me.dgvresult.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dgvresult.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvresult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvresult.Location = New System.Drawing.Point(25, 323)
        Me.dgvresult.Name = "dgvresult"
        Me.dgvresult.ReadOnly = True
        Me.dgvresult.RowHeadersWidth = 51
        Me.dgvresult.RowTemplate.Height = 26
        Me.dgvresult.Size = New System.Drawing.Size(1450, 594)
        Me.dgvresult.TabIndex = 90
        '
        'lblUsername
        '
        Me.lblUsername.AutoSize = True
        Me.lblUsername.Location = New System.Drawing.Point(134, 20)
        Me.lblUsername.Name = "lblUsername"
        Me.lblUsername.Size = New System.Drawing.Size(33, 16)
        Me.lblUsername.TabIndex = 89
        Me.lblUsername.Text = "user"
        '
        'dataGridViewDetails
        '
        Me.dataGridViewDetails.AllowUserToAddRows = False
        Me.dataGridViewDetails.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dataGridViewDetails.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dataGridViewDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dataGridViewDetails.Location = New System.Drawing.Point(362, 20)
        Me.dataGridViewDetails.Name = "dataGridViewDetails"
        Me.dataGridViewDetails.ReadOnly = True
        Me.dataGridViewDetails.RowHeadersWidth = 51
        Me.dataGridViewDetails.RowTemplate.Height = 26
        Me.dataGridViewDetails.Size = New System.Drawing.Size(887, 80)
        Me.dataGridViewDetails.TabIndex = 88
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(984, 120)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(41, 24)
        Me.Label2.TabIndex = 87
        Me.Label2.Text = "Lot"
        '
        'cmblot
        '
        Me.cmblot.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmblot.FormattingEnabled = True
        Me.cmblot.Location = New System.Drawing.Point(915, 157)
        Me.cmblot.Name = "cmblot"
        Me.cmblot.Size = New System.Drawing.Size(213, 29)
        Me.cmblot.TabIndex = 86
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(581, 120)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(82, 24)
        Me.Label1.TabIndex = 85
        Me.Label1.Text = "الرسالة"
        '
        'cmbbatch
        '
        Me.cmbbatch.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbbatch.FormattingEnabled = True
        Me.cmbbatch.Location = New System.Drawing.Point(523, 157)
        Me.cmbbatch.Name = "cmbbatch"
        Me.cmbbatch.Size = New System.Drawing.Size(227, 29)
        Me.cmbbatch.TabIndex = 84
        '
        'btnSearch
        '
        Me.btnSearch.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSearch.Location = New System.Drawing.Point(1192, 254)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(127, 37)
        Me.btnSearch.TabIndex = 92
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'dtpFromDate
        '
        Me.dtpFromDate.Location = New System.Drawing.Point(307, 253)
        Me.dtpFromDate.Name = "dtpFromDate"
        Me.dtpFromDate.Size = New System.Drawing.Size(228, 22)
        Me.dtpFromDate.TabIndex = 93
        '
        'dtptoDate
        '
        Me.dtptoDate.Location = New System.Drawing.Point(753, 254)
        Me.dtptoDate.Name = "dtptoDate"
        Me.dtptoDate.Size = New System.Drawing.Size(228, 22)
        Me.dtptoDate.TabIndex = 94
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(219, 255)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(54, 21)
        Me.Label3.TabIndex = 95
        Me.Label3.Text = "From"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(685, 256)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(31, 21)
        Me.Label4.TabIndex = 96
        Me.Label4.Text = "To"
        '
        'btnExportToExcel
        '
        Me.btnExportToExcel.Location = New System.Drawing.Point(37, 254)
        Me.btnExportToExcel.Name = "btnExportToExcel"
        Me.btnExportToExcel.Size = New System.Drawing.Size(102, 37)
        Me.btnExportToExcel.TabIndex = 97
        Me.btnExportToExcel.Text = "export"
        Me.btnExportToExcel.UseVisualStyleBackColor = True
        '
        'chkShowInitialData
        '
        Me.chkShowInitialData.AutoSize = True
        Me.chkShowInitialData.Location = New System.Drawing.Point(1192, 207)
        Me.chkShowInitialData.Name = "chkShowInitialData"
        Me.chkShowInitialData.Size = New System.Drawing.Size(18, 17)
        Me.chkShowInitialData.TabIndex = 98
        Me.chkShowInitialData.UseVisualStyleBackColor = True
        '
        'mainqcrawtestform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1678, 953)
        Me.Controls.Add(Me.chkShowInitialData)
        Me.Controls.Add(Me.btnExportToExcel)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.dtptoDate)
        Me.Controls.Add(Me.dtpFromDate)
        Me.Controls.Add(Me.btnSearch)
        Me.Controls.Add(Me.btnadd)
        Me.Controls.Add(Me.dgvresult)
        Me.Controls.Add(Me.lblUsername)
        Me.Controls.Add(Me.dataGridViewDetails)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.cmblot)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmbbatch)
        Me.Name = "mainqcrawtestform"
        Me.Text = "mainqcrawtestform"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.dgvresult, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dataGridViewDetails, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnadd As Button
    Friend WithEvents dgvresult As DataGridView
    Friend WithEvents lblUsername As Label
    Friend WithEvents dataGridViewDetails As DataGridView
    Friend WithEvents Label2 As Label
    Friend WithEvents cmblot As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents cmbbatch As ComboBox
    Friend WithEvents btnSearch As Button
    Friend WithEvents dtpFromDate As DateTimePicker
    Friend WithEvents dtptoDate As DateTimePicker
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents btnExportToExcel As Button
    Friend WithEvents chkShowInitialData As CheckBox
End Class
