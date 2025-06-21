<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class rawstorereporform
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
        Me.cmbStorePermission = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbServiceType = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmbpo = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cmbBatchId = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.dtpFromDate = New System.Windows.Forms.DateTimePicker()
        Me.dtpToDate = New System.Windows.Forms.DateTimePicker()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.btnExportToExcel = New System.Windows.Forms.Button()
        Me.btnSearchOther = New System.Windows.Forms.Button()
        CType(Me.dgvReport, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvReport
        '
        Me.dgvReport.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dgvReport.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvReport.Location = New System.Drawing.Point(28, 314)
        Me.dgvReport.Name = "dgvReport"
        Me.dgvReport.RowHeadersWidth = 51
        Me.dgvReport.RowTemplate.Height = 26
        Me.dgvReport.Size = New System.Drawing.Size(1402, 517)
        Me.dgvReport.TabIndex = 0
        '
        'cmbStorePermission
        '
        Me.cmbStorePermission.FormattingEnabled = True
        Me.cmbStorePermission.Location = New System.Drawing.Point(116, 199)
        Me.cmbStorePermission.Name = "cmbStorePermission"
        Me.cmbStorePermission.Size = New System.Drawing.Size(194, 24)
        Me.cmbStorePermission.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(28, 202)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 17)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "اذن الاضافة"
        '
        'cmbServiceType
        '
        Me.cmbServiceType.FormattingEnabled = True
        Me.cmbServiceType.Location = New System.Drawing.Point(116, 145)
        Me.cmbServiceType.Name = "cmbServiceType"
        Me.cmbServiceType.Size = New System.Drawing.Size(194, 24)
        Me.cmbServiceType.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(30, 145)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(70, 17)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "نوع الخدمة"
        '
        'cmbpo
        '
        Me.cmbpo.FormattingEnabled = True
        Me.cmbpo.Location = New System.Drawing.Point(116, 94)
        Me.cmbpo.Name = "cmbpo"
        Me.cmbpo.Size = New System.Drawing.Size(194, 24)
        Me.cmbpo.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(30, 97)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Label3.Size = New System.Drawing.Size(63, 17)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "رقم الPO"
        '
        'cmbBatchId
        '
        Me.cmbBatchId.FormattingEnabled = True
        Me.cmbBatchId.Location = New System.Drawing.Point(116, 50)
        Me.cmbBatchId.Name = "cmbBatchId"
        Me.cmbBatchId.Size = New System.Drawing.Size(194, 24)
        Me.cmbBatchId.TabIndex = 7
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(42, 56)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(43, 17)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "رساله"
        '
        'dtpFromDate
        '
        Me.dtpFromDate.Location = New System.Drawing.Point(509, 145)
        Me.dtpFromDate.Name = "dtpFromDate"
        Me.dtpFromDate.Size = New System.Drawing.Size(200, 24)
        Me.dtpFromDate.TabIndex = 9
        Me.dtpFromDate.Value = New Date(2025, 1, 1, 0, 0, 0, 0)
        '
        'dtpToDate
        '
        Me.dtpToDate.Location = New System.Drawing.Point(868, 145)
        Me.dtpToDate.Name = "dtpToDate"
        Me.dtpToDate.Size = New System.Drawing.Size(200, 24)
        Me.dtpToDate.TabIndex = 10
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(440, 145)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(40, 17)
        Me.Label5.TabIndex = 11
        Me.Label5.Text = "From"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(826, 145)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(24, 17)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = "To"
        '
        'btnSearch
        '
        Me.btnSearch.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSearch.Location = New System.Drawing.Point(509, 247)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(154, 43)
        Me.btnSearch.TabIndex = 13
        Me.btnSearch.Text = "بحث اضافات"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnExportToExcel
        '
        Me.btnExportToExcel.Font = New System.Drawing.Font("Tahoma", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExportToExcel.Location = New System.Drawing.Point(45, 259)
        Me.btnExportToExcel.Name = "btnExportToExcel"
        Me.btnExportToExcel.Size = New System.Drawing.Size(126, 42)
        Me.btnExportToExcel.TabIndex = 14
        Me.btnExportToExcel.Text = "Export To Excell"
        Me.btnExportToExcel.UseVisualStyleBackColor = True
        '
        'btnSearchOther
        '
        Me.btnSearchOther.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSearchOther.Location = New System.Drawing.Point(758, 247)
        Me.btnSearchOther.Name = "btnSearchOther"
        Me.btnSearchOther.Size = New System.Drawing.Size(174, 43)
        Me.btnSearchOther.TabIndex = 15
        Me.btnSearchOther.Text = "بحث صرف ومرتجع"
        Me.btnSearchOther.UseVisualStyleBackColor = True
        '
        'rawstorereporform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1481, 843)
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
        Me.Name = "rawstorereporform"
        Me.Text = "rawstorereporform"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.dgvReport, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents dgvReport As DataGridView
    Friend WithEvents cmbStorePermission As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents cmbServiceType As ComboBox
    Friend WithEvents Label2 As Label
    Friend WithEvents cmbpo As ComboBox
    Friend WithEvents Label3 As Label
    Friend WithEvents cmbBatchId As ComboBox
    Friend WithEvents Label4 As Label
    Friend WithEvents dtpFromDate As DateTimePicker
    Friend WithEvents dtpToDate As DateTimePicker
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents btnSearch As Button
    Friend WithEvents btnExportToExcel As Button
    Friend WithEvents btnSearchOther As Button
End Class
