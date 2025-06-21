<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class mainrowsampleform
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
        Me.cmbbatch = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmblot = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.dataGridViewDetails = New System.Windows.Forms.DataGridView()
        Me.btnprint2 = New System.Windows.Forms.Button()
        Me.btnaddroll = New System.Windows.Forms.Button()
        Me.btnreport = New System.Windows.Forms.Button()
        Me.lblUsername = New System.Windows.Forms.Label()
        Me.lbltotalpoints = New System.Windows.Forms.Label()
        Me.dataGridViewDetails2 = New System.Windows.Forms.DataGridView()
        Me.cmbStatus = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnSaveStatus = New System.Windows.Forms.Button()
        Me.btnadd = New System.Windows.Forms.Button()
        Me.dgvresult = New System.Windows.Forms.DataGridView()
        Me.lblTotalHeight = New System.Windows.Forms.Label()
        Me.dgvstatus = New System.Windows.Forms.DataGridView()
        Me.chkShowInitialData = New System.Windows.Forms.CheckBox()
        CType(Me.dataGridViewDetails, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dataGridViewDetails2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvresult, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvstatus, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmbbatch
        '
        Me.cmbbatch.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbbatch.FormattingEnabled = True
        Me.cmbbatch.Location = New System.Drawing.Point(371, 149)
        Me.cmbbatch.Name = "cmbbatch"
        Me.cmbbatch.Size = New System.Drawing.Size(227, 29)
        Me.cmbbatch.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(429, 112)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(82, 24)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "الرسالة"
        '
        'cmblot
        '
        Me.cmblot.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmblot.FormattingEnabled = True
        Me.cmblot.Location = New System.Drawing.Point(653, 149)
        Me.cmblot.Name = "cmblot"
        Me.cmblot.Size = New System.Drawing.Size(213, 29)
        Me.cmblot.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(722, 112)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(41, 24)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Lot"
        '
        'dataGridViewDetails
        '
        Me.dataGridViewDetails.AllowUserToAddRows = False
        Me.dataGridViewDetails.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dataGridViewDetails.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dataGridViewDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dataGridViewDetails.Location = New System.Drawing.Point(210, 12)
        Me.dataGridViewDetails.Name = "dataGridViewDetails"
        Me.dataGridViewDetails.ReadOnly = True
        Me.dataGridViewDetails.RowHeadersWidth = 51
        Me.dataGridViewDetails.RowTemplate.Height = 26
        Me.dataGridViewDetails.Size = New System.Drawing.Size(835, 97)
        Me.dataGridViewDetails.TabIndex = 4
        '
        'btnprint2
        '
        Me.btnprint2.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnprint2.Location = New System.Drawing.Point(554, 318)
        Me.btnprint2.Name = "btnprint2"
        Me.btnprint2.Size = New System.Drawing.Size(184, 36)
        Me.btnprint2.TabIndex = 75
        Me.btnprint2.Text = "تقرير فحص الخام"
        Me.btnprint2.UseVisualStyleBackColor = True
        '
        'btnaddroll
        '
        Me.btnaddroll.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnaddroll.Location = New System.Drawing.Point(882, 129)
        Me.btnaddroll.Name = "btnaddroll"
        Me.btnaddroll.Size = New System.Drawing.Size(107, 58)
        Me.btnaddroll.TabIndex = 74
        Me.btnaddroll.Text = "اضافه توب جديد"
        Me.btnaddroll.UseVisualStyleBackColor = True
        '
        'btnreport
        '
        Me.btnreport.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnreport.Location = New System.Drawing.Point(299, 315)
        Me.btnreport.Name = "btnreport"
        Me.btnreport.Size = New System.Drawing.Size(212, 37)
        Me.btnreport.TabIndex = 73
        Me.btnreport.Text = "تقرير تسليم مخزن"
        Me.btnreport.UseVisualStyleBackColor = True
        '
        'lblUsername
        '
        Me.lblUsername.AutoSize = True
        Me.lblUsername.Location = New System.Drawing.Point(14, 9)
        Me.lblUsername.Name = "lblUsername"
        Me.lblUsername.Size = New System.Drawing.Size(33, 16)
        Me.lblUsername.TabIndex = 76
        Me.lblUsername.Text = "user"
        '
        'lbltotalpoints
        '
        Me.lbltotalpoints.AutoSize = True
        Me.lbltotalpoints.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbltotalpoints.Location = New System.Drawing.Point(245, 355)
        Me.lbltotalpoints.Name = "lbltotalpoints"
        Me.lbltotalpoints.Size = New System.Drawing.Size(128, 24)
        Me.lbltotalpoints.TabIndex = 78
        Me.lbltotalpoints.Text = "Total Points"
        '
        'dataGridViewDetails2
        '
        Me.dataGridViewDetails2.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dataGridViewDetails2.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dataGridViewDetails2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dataGridViewDetails2.Location = New System.Drawing.Point(17, 394)
        Me.dataGridViewDetails2.Name = "dataGridViewDetails2"
        Me.dataGridViewDetails2.RowHeadersWidth = 51
        Me.dataGridViewDetails2.RowTemplate.Height = 26
        Me.dataGridViewDetails2.Size = New System.Drawing.Size(1422, 394)
        Me.dataGridViewDetails2.TabIndex = 77
        '
        'cmbStatus
        '
        Me.cmbStatus.FormattingEnabled = True
        Me.cmbStatus.Location = New System.Drawing.Point(32, 112)
        Me.cmbStatus.Name = "cmbStatus"
        Me.cmbStatus.Size = New System.Drawing.Size(138, 24)
        Me.cmbStatus.TabIndex = 80
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(67, 75)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(57, 21)
        Me.Label3.TabIndex = 81
        Me.Label3.Text = "الحالة"
        '
        'btnSaveStatus
        '
        Me.btnSaveStatus.Location = New System.Drawing.Point(32, 143)
        Me.btnSaveStatus.Name = "btnSaveStatus"
        Me.btnSaveStatus.Size = New System.Drawing.Size(119, 35)
        Me.btnSaveStatus.TabIndex = 82
        Me.btnSaveStatus.Text = "تسجيل الحالة"
        Me.btnSaveStatus.UseVisualStyleBackColor = True
        '
        'btnadd
        '
        Me.btnadd.Location = New System.Drawing.Point(47, 323)
        Me.btnadd.Name = "btnadd"
        Me.btnadd.Size = New System.Drawing.Size(86, 29)
        Me.btnadd.TabIndex = 83
        Me.btnadd.Text = "تسجيل"
        Me.btnadd.UseVisualStyleBackColor = True
        '
        'dgvresult
        '
        Me.dgvresult.AllowUserToAddRows = False
        Me.dgvresult.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dgvresult.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvresult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvresult.Location = New System.Drawing.Point(17, 204)
        Me.dgvresult.Name = "dgvresult"
        Me.dgvresult.ReadOnly = True
        Me.dgvresult.RowHeadersWidth = 51
        Me.dgvresult.RowTemplate.Height = 26
        Me.dgvresult.Size = New System.Drawing.Size(1063, 91)
        Me.dgvresult.TabIndex = 79
        '
        'lblTotalHeight
        '
        Me.lblTotalHeight.AutoSize = True
        Me.lblTotalHeight.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalHeight.Location = New System.Drawing.Point(722, 355)
        Me.lblTotalHeight.Name = "lblTotalHeight"
        Me.lblTotalHeight.Size = New System.Drawing.Size(77, 24)
        Me.lblTotalHeight.TabIndex = 84
        Me.lblTotalHeight.Text = "Label4"
        '
        'dgvstatus
        '
        Me.dgvstatus.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dgvstatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvstatus.Location = New System.Drawing.Point(1086, 12)
        Me.dgvstatus.Name = "dgvstatus"
        Me.dgvstatus.RowHeadersWidth = 51
        Me.dgvstatus.RowTemplate.Height = 24
        Me.dgvstatus.Size = New System.Drawing.Size(498, 367)
        Me.dgvstatus.TabIndex = 85
        '
        'chkShowInitialData
        '
        Me.chkShowInitialData.AutoSize = True
        Me.chkShowInitialData.Location = New System.Drawing.Point(948, 361)
        Me.chkShowInitialData.Name = "chkShowInitialData"
        Me.chkShowInitialData.Size = New System.Drawing.Size(18, 17)
        Me.chkShowInitialData.TabIndex = 86
        Me.chkShowInitialData.UseVisualStyleBackColor = True
        '
        'mainrowsampleform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1590, 881)
        Me.Controls.Add(Me.chkShowInitialData)
        Me.Controls.Add(Me.dgvstatus)
        Me.Controls.Add(Me.lblTotalHeight)
        Me.Controls.Add(Me.btnadd)
        Me.Controls.Add(Me.btnSaveStatus)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.cmbStatus)
        Me.Controls.Add(Me.dgvresult)
        Me.Controls.Add(Me.lbltotalpoints)
        Me.Controls.Add(Me.dataGridViewDetails2)
        Me.Controls.Add(Me.lblUsername)
        Me.Controls.Add(Me.btnprint2)
        Me.Controls.Add(Me.btnaddroll)
        Me.Controls.Add(Me.btnreport)
        Me.Controls.Add(Me.dataGridViewDetails)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.cmblot)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmbbatch)
        Me.Name = "mainrowsampleform"
        Me.Text = "mainrowsampleform"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.dataGridViewDetails, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dataGridViewDetails2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvresult, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvstatus, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents cmbbatch As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents cmblot As ComboBox
    Friend WithEvents Label2 As Label
    Friend WithEvents dataGridViewDetails As DataGridView
    Friend WithEvents btnprint2 As Button
    Friend WithEvents btnaddroll As Button
    Friend WithEvents btnreport As Button
    Friend WithEvents lblUsername As Label
    Friend WithEvents lbltotalpoints As Label
    Friend WithEvents dataGridViewDetails2 As DataGridView
    Friend WithEvents cmbStatus As ComboBox
    Friend WithEvents Label3 As Label
    Friend WithEvents btnSaveStatus As Button
    Friend WithEvents btnadd As Button
    Friend WithEvents dgvresult As DataGridView
    Friend WithEvents lblTotalHeight As Label
    Friend WithEvents dgvstatus As DataGridView
    Friend WithEvents chkShowInitialData As CheckBox
End Class
