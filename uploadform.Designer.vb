<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UploadForm
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
        Me.btnUpload = New System.Windows.Forms.Button()
        Me.lblFilePath = New System.Windows.Forms.Label()
        Me.cmbTables = New System.Windows.Forms.ComboBox()
        Me.flpColumns = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnsample = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnUpload
        '
        Me.btnUpload.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnUpload.Location = New System.Drawing.Point(332, 55)
        Me.btnUpload.Name = "btnUpload"
        Me.btnUpload.Size = New System.Drawing.Size(262, 85)
        Me.btnUpload.TabIndex = 0
        Me.btnUpload.Text = "Upload"
        Me.btnUpload.UseVisualStyleBackColor = True
        '
        'lblFilePath
        '
        Me.lblFilePath.AutoSize = True
        Me.lblFilePath.Location = New System.Drawing.Point(48, 23)
        Me.lblFilePath.Name = "lblFilePath"
        Me.lblFilePath.Size = New System.Drawing.Size(26, 17)
        Me.lblFilePath.TabIndex = 1
        Me.lblFilePath.Text = "File"
        '
        'cmbTables
        '
        Me.cmbTables.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbTables.FormattingEnabled = True
        Me.cmbTables.Location = New System.Drawing.Point(109, 21)
        Me.cmbTables.Name = "cmbTables"
        Me.cmbTables.Size = New System.Drawing.Size(208, 32)
        Me.cmbTables.TabIndex = 2
        '
        'flpColumns
        '
        Me.flpColumns.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.flpColumns.Location = New System.Drawing.Point(109, 172)
        Me.flpColumns.Name = "flpColumns"
        Me.flpColumns.Size = New System.Drawing.Size(801, 300)
        Me.flpColumns.TabIndex = 4
        '
        'btnsample
        '
        Me.btnsample.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnsample.Location = New System.Drawing.Point(832, 23)
        Me.btnsample.Name = "btnsample"
        Me.btnsample.Size = New System.Drawing.Size(183, 54)
        Me.btnsample.TabIndex = 5
        Me.btnsample.Text = "Sample Sheet"
        Me.btnsample.UseVisualStyleBackColor = True
        '
        'UploadForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1194, 633)
        Me.Controls.Add(Me.btnsample)
        Me.Controls.Add(Me.flpColumns)
        Me.Controls.Add(Me.cmbTables)
        Me.Controls.Add(Me.lblFilePath)
        Me.Controls.Add(Me.btnUpload)
        Me.Name = "UploadForm"
        Me.Text = "uploadform"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnUpload As System.Windows.Forms.Button
    Friend WithEvents lblFilePath As System.Windows.Forms.Label
    Friend WithEvents cmbTables As System.Windows.Forms.ComboBox
    Friend WithEvents flpColumns As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents btnsample As System.Windows.Forms.Button
End Class
