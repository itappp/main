<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class reportstop
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
        Me.btnExportExcel = New System.Windows.Forms.Button()
        Me.btnLoadReport = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtWorderID = New System.Windows.Forms.TextBox()
        Me.dtpFrom = New System.Windows.Forms.DateTimePicker()
        Me.dtpTo = New System.Windows.Forms.DateTimePicker()
        CType(Me.dgvReport, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvReport
        '
        Me.dgvReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvReport.Location = New System.Drawing.Point(5, 163)
        Me.dgvReport.Name = "dgvReport"
        Me.dgvReport.RowTemplate.Height = 26
        Me.dgvReport.Size = New System.Drawing.Size(1111, 435)
        Me.dgvReport.TabIndex = 1
        '
        'btnExportExcel
        '
        Me.btnExportExcel.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExportExcel.Location = New System.Drawing.Point(66, 89)
        Me.btnExportExcel.Name = "btnExportExcel"
        Me.btnExportExcel.Size = New System.Drawing.Size(161, 45)
        Me.btnExportExcel.TabIndex = 3
        Me.btnExportExcel.Text = "Export To Excell"
        Me.btnExportExcel.UseVisualStyleBackColor = True
        '
        'btnLoadReport
        '
        Me.btnLoadReport.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLoadReport.Location = New System.Drawing.Point(485, 98)
        Me.btnLoadReport.Name = "btnLoadReport"
        Me.btnLoadReport.Size = New System.Drawing.Size(179, 59)
        Me.btnLoadReport.TabIndex = 4
        Me.btnLoadReport.Text = "Search"
        Me.btnLoadReport.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(131, 22)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(86, 24)
        Me.Label3.TabIndex = 14
        Me.Label3.Text = "Worder"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(534, 25)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(54, 21)
        Me.Label2.TabIndex = 13
        Me.Label2.Text = "From"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(835, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(31, 21)
        Me.Label1.TabIndex = 12
        Me.Label1.Text = "To"
        '
        'txtWorderID
        '
        Me.txtWorderID.Location = New System.Drawing.Point(238, 22)
        Me.txtWorderID.Name = "txtWorderID"
        Me.txtWorderID.Size = New System.Drawing.Size(149, 24)
        Me.txtWorderID.TabIndex = 11
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(594, 22)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Size = New System.Drawing.Size(136, 24)
        Me.dtpFrom.TabIndex = 10
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(872, 22)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Size = New System.Drawing.Size(136, 24)
        Me.dtpTo.TabIndex = 9
        '
        'reportstop
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1141, 625)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtWorderID)
        Me.Controls.Add(Me.dtpFrom)
        Me.Controls.Add(Me.dtpTo)
        Me.Controls.Add(Me.btnLoadReport)
        Me.Controls.Add(Me.btnExportExcel)
        Me.Controls.Add(Me.dgvReport)
        Me.Name = "reportstop"
        Me.Text = "reportstop"
        CType(Me.dgvReport, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dgvReport As System.Windows.Forms.DataGridView
    Friend WithEvents btnExportExcel As System.Windows.Forms.Button
    Friend WithEvents btnLoadReport As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtWorderID As System.Windows.Forms.TextBox
    Friend WithEvents dtpFrom As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpTo As System.Windows.Forms.DateTimePicker
End Class
