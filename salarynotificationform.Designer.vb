<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class salarynotificationform
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.btnUpload = New System.Windows.Forms.Button()
        Me.btnSend = New System.Windows.Forms.Button()
        Me.dgvEmployees = New System.Windows.Forms.DataGridView()
        Me.openFileDialog = New System.Windows.Forms.OpenFileDialog()
        Me.btnDownloadSample = New System.Windows.Forms.Button()
        CType(Me.dgvEmployees, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnUpload
        '
        Me.btnUpload.Location = New System.Drawing.Point(53, 231)
        Me.btnUpload.Margin = New System.Windows.Forms.Padding(4)
        Me.btnUpload.Name = "btnUpload"
        Me.btnUpload.Size = New System.Drawing.Size(200, 37)
        Me.btnUpload.TabIndex = 0
        Me.btnUpload.Text = "Upload Excel File"
        Me.btnUpload.UseVisualStyleBackColor = True
        '
        'btnSend
        '
        Me.btnSend.Enabled = False
        Me.btnSend.Location = New System.Drawing.Point(279, 231)
        Me.btnSend.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSend.Name = "btnSend"
        Me.btnSend.Size = New System.Drawing.Size(200, 37)
        Me.btnSend.TabIndex = 1
        Me.btnSend.Text = "Send WhatsApp Messages"
        Me.btnSend.UseVisualStyleBackColor = True
        '
        'dgvEmployees
        '
        Me.dgvEmployees.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvEmployees.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvEmployees.Location = New System.Drawing.Point(27, 290)
        Me.dgvEmployees.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvEmployees.Name = "dgvEmployees"
        Me.dgvEmployees.RowHeadersWidth = 51
        Me.dgvEmployees.Size = New System.Drawing.Size(987, 387)
        Me.dgvEmployees.TabIndex = 2
        '
        'openFileDialog
        '
        Me.openFileDialog.Filter = "Excel Files|*.xls;*.xlsx"
        Me.openFileDialog.Title = "Select Employee Data File"
        '
        'btnDownloadSample
        '
        Me.btnDownloadSample.Location = New System.Drawing.Point(583, 231)
        Me.btnDownloadSample.Margin = New System.Windows.Forms.Padding(4)
        Me.btnDownloadSample.Name = "btnDownloadSample"
        Me.btnDownloadSample.Size = New System.Drawing.Size(133, 37)
        Me.btnDownloadSample.TabIndex = 4
        Me.btnDownloadSample.Text = "Sample"
        Me.btnDownloadSample.UseVisualStyleBackColor = True
        '
        'salarynotificationform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1067, 738)
        Me.Controls.Add(Me.btnDownloadSample)
        Me.Controls.Add(Me.dgvEmployees)
        Me.Controls.Add(Me.btnSend)
        Me.Controls.Add(Me.btnUpload)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "salarynotificationform"
        Me.Text = "HR Salary Notification"
        CType(Me.dgvEmployees, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents btnUpload As Button
    Friend WithEvents btnSend As Button
    Friend WithEvents dgvEmployees As DataGridView
    Friend WithEvents openFileDialog As OpenFileDialog
    Friend WithEvents btnDownloadSample As Button
End Class