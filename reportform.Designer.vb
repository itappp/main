<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class reportform
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
        Me.btnLoadReport = New System.Windows.Forms.Button()
        Me.btnExportExcel = New System.Windows.Forms.Button()
        Me.dtpTo = New System.Windows.Forms.DateTimePicker()
        Me.dtpFrom = New System.Windows.Forms.DateTimePicker()
        Me.txtWorderID = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        CType(Me.dgvReport, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvReport
        '
        Me.dgvReport.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dgvReport.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvReport.Location = New System.Drawing.Point(12, 145)
        Me.dgvReport.Name = "dgvReport"
        Me.dgvReport.RowHeadersWidth = 51
        Me.dgvReport.RowTemplate.Height = 26
        Me.dgvReport.Size = New System.Drawing.Size(1443, 493)
        Me.dgvReport.TabIndex = 0
        '
        'btnLoadReport
        '
        Me.btnLoadReport.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLoadReport.Location = New System.Drawing.Point(771, 80)
        Me.btnLoadReport.Name = "btnLoadReport"
        Me.btnLoadReport.Size = New System.Drawing.Size(179, 59)
        Me.btnLoadReport.TabIndex = 1
        Me.btnLoadReport.Text = "Search"
        Me.btnLoadReport.UseVisualStyleBackColor = True
        '
        'btnExportExcel
        '
        Me.btnExportExcel.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExportExcel.Location = New System.Drawing.Point(98, 79)
        Me.btnExportExcel.Name = "btnExportExcel"
        Me.btnExportExcel.Size = New System.Drawing.Size(161, 45)
        Me.btnExportExcel.TabIndex = 2
        Me.btnExportExcel.Text = "Export To Excell"
        Me.btnExportExcel.UseVisualStyleBackColor = True
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(986, 21)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Size = New System.Drawing.Size(136, 24)
        Me.dtpTo.TabIndex = 3
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(708, 21)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Size = New System.Drawing.Size(136, 24)
        Me.dtpFrom.TabIndex = 4
        '
        'txtWorderID
        '
        Me.txtWorderID.Location = New System.Drawing.Point(352, 21)
        Me.txtWorderID.Name = "txtWorderID"
        Me.txtWorderID.Size = New System.Drawing.Size(149, 24)
        Me.txtWorderID.TabIndex = 5
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(949, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(31, 21)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "To"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(648, 24)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(54, 21)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "From"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(245, 21)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(86, 24)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Worder"
        '
        'reportform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(1632, 712)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtWorderID)
        Me.Controls.Add(Me.dtpFrom)
        Me.Controls.Add(Me.dtpTo)
        Me.Controls.Add(Me.btnExportExcel)
        Me.Controls.Add(Me.btnLoadReport)
        Me.Controls.Add(Me.dgvReport)
        Me.Name = "reportform"
        Me.Text = "Report Production Wagdy Moamen"
        CType(Me.dgvReport, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dgvReport As System.Windows.Forms.DataGridView
    Friend WithEvents btnLoadReport As System.Windows.Forms.Button
    Friend WithEvents btnExportExcel As System.Windows.Forms.Button
    Friend WithEvents dtpTo As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpFrom As System.Windows.Forms.DateTimePicker
    Friend WithEvents txtWorderID As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
End Class
