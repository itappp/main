<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class rawstorerawform
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
        Me.dgvReport = New System.Windows.Forms.DataGridView()
        Me.btnSearchOther = New System.Windows.Forms.Button()
        Me.btnExportToExcel = New System.Windows.Forms.Button()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.dtpToDate = New System.Windows.Forms.DateTimePicker()
        Me.dtpFromDate = New System.Windows.Forms.DateTimePicker()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cmbBatchId = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cmbpo = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmbServiceType = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbStorePermission = New System.Windows.Forms.ComboBox()
        CType(Me.dgvReport, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvReport
        '
        Me.dgvReport.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dgvReport.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvReport.Location = New System.Drawing.Point(32, 289)
        Me.dgvReport.Name = "dgvReport"
        Me.dgvReport.RowHeadersWidth = 51
        Me.dgvReport.RowTemplate.Height = 26
        Me.dgvReport.Size = New System.Drawing.Size(1192, 497)
        Me.dgvReport.TabIndex = 0
        '
        'btnSearchOther
        '
        Me.btnSearchOther.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSearchOther.Location = New System.Drawing.Point(791, 229)
        Me.btnSearchOther.Name = "btnSearchOther"
        Me.btnSearchOther.Size = New System.Drawing.Size(174, 43)
        Me.btnSearchOther.TabIndex = 30
        Me.btnSearchOther.Text = "بحث صرف ومرتجع"
        Me.btnSearchOther.UseVisualStyleBackColor = True
        '
        'btnExportToExcel
        '
        Me.btnExportToExcel.Font = New System.Drawing.Font("Tahoma", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExportToExcel.Location = New System.Drawing.Point(78, 241)
        Me.btnExportToExcel.Name = "btnExportToExcel"
        Me.btnExportToExcel.Size = New System.Drawing.Size(126, 42)
        Me.btnExportToExcel.TabIndex = 29
        Me.btnExportToExcel.Text = "Export To Excell"
        Me.btnExportToExcel.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSearch.Location = New System.Drawing.Point(542, 229)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(154, 43)
        Me.btnSearch.TabIndex = 28
        Me.btnSearch.Text = "بحث اضافات"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(859, 127)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(24, 17)
        Me.Label6.TabIndex = 27
        Me.Label6.Text = "To"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(473, 127)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(40, 17)
        Me.Label5.TabIndex = 26
        Me.Label5.Text = "From"
        '
        'dtpToDate
        '
        Me.dtpToDate.Location = New System.Drawing.Point(901, 127)
        Me.dtpToDate.Name = "dtpToDate"
        Me.dtpToDate.Size = New System.Drawing.Size(200, 24)
        Me.dtpToDate.TabIndex = 25
        '
        'dtpFromDate
        '
        Me.dtpFromDate.Location = New System.Drawing.Point(542, 127)
        Me.dtpFromDate.Name = "dtpFromDate"
        Me.dtpFromDate.Size = New System.Drawing.Size(200, 24)
        Me.dtpFromDate.TabIndex = 24
        Me.dtpFromDate.Value = New Date(2025, 1, 1, 0, 0, 0, 0)
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(75, 38)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(43, 17)
        Me.Label4.TabIndex = 23
        Me.Label4.Text = "رساله"
        '
        'cmbBatchId
        '
        Me.cmbBatchId.FormattingEnabled = True
        Me.cmbBatchId.Location = New System.Drawing.Point(149, 32)
        Me.cmbBatchId.Name = "cmbBatchId"
        Me.cmbBatchId.Size = New System.Drawing.Size(194, 24)
        Me.cmbBatchId.TabIndex = 22
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(63, 79)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Label3.Size = New System.Drawing.Size(63, 17)
        Me.Label3.TabIndex = 21
        Me.Label3.Text = "رقم الPO"
        '
        'cmbpo
        '
        Me.cmbpo.FormattingEnabled = True
        Me.cmbpo.Location = New System.Drawing.Point(149, 76)
        Me.cmbpo.Name = "cmbpo"
        Me.cmbpo.Size = New System.Drawing.Size(194, 24)
        Me.cmbpo.TabIndex = 20
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(63, 127)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(70, 17)
        Me.Label2.TabIndex = 19
        Me.Label2.Text = "نوع الخدمة"
        '
        'cmbServiceType
        '
        Me.cmbServiceType.FormattingEnabled = True
        Me.cmbServiceType.Location = New System.Drawing.Point(149, 127)
        Me.cmbServiceType.Name = "cmbServiceType"
        Me.cmbServiceType.Size = New System.Drawing.Size(194, 24)
        Me.cmbServiceType.TabIndex = 18
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(61, 184)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 17)
        Me.Label1.TabIndex = 17
        Me.Label1.Text = "اذن الاضافة"
        '
        'cmbStorePermission
        '
        Me.cmbStorePermission.FormattingEnabled = True
        Me.cmbStorePermission.Location = New System.Drawing.Point(149, 181)
        Me.cmbStorePermission.Name = "cmbStorePermission"
        Me.cmbStorePermission.Size = New System.Drawing.Size(194, 24)
        Me.cmbStorePermission.TabIndex = 16
        '
        'rawstorerawform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1420, 798)
        Me.Controls.Add(Me.btnSearchOther)
        Me.Controls.Add(Me.btnExportToExcel)
        Me.Controls.Add(Me.btnSearch)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.dtpToDate)
        Me.Controls.Add(Me.dtpFromDate)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.cmbBatchId)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.cmbpo)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.cmbServiceType)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmbStorePermission)
        Me.Controls.Add(Me.dgvReport)
        Me.Name = "rawstorerawform"
        Me.Text = "rawstorerawform"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.dgvReport, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents dgvReport As DataGridView
    Friend WithEvents btnSearchOther As Button
    Friend WithEvents btnExportToExcel As Button
    Friend WithEvents btnSearch As Button
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents dtpToDate As DateTimePicker
    Friend WithEvents dtpFromDate As DateTimePicker
    Friend WithEvents Label4 As Label
    Friend WithEvents cmbBatchId As ComboBox
    Friend WithEvents Label3 As Label
    Friend WithEvents cmbpo As ComboBox
    Friend WithEvents Label2 As Label
    Friend WithEvents cmbServiceType As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents cmbStorePermission As ComboBox
End Class
