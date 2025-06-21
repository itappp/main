<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class reportlivebatchform
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
        Me.dgvDetails = New System.Windows.Forms.DataGridView()
        Me.btnsearch = New System.Windows.Forms.Button()
        Me.cmbKind = New System.Windows.Forms.ComboBox()
        Me.btnExportToExcel = New System.Windows.Forms.Button()
        CType(Me.dgvDetails, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvDetails
        '
        Me.dgvDetails.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dgvDetails.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDetails.Location = New System.Drawing.Point(12, 122)
        Me.dgvDetails.Name = "dgvDetails"
        Me.dgvDetails.RowHeadersWidth = 51
        Me.dgvDetails.RowTemplate.Height = 24
        Me.dgvDetails.Size = New System.Drawing.Size(1369, 690)
        Me.dgvDetails.TabIndex = 0
        '
        'btnsearch
        '
        Me.btnsearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnsearch.Location = New System.Drawing.Point(899, 26)
        Me.btnsearch.Name = "btnsearch"
        Me.btnsearch.Size = New System.Drawing.Size(144, 48)
        Me.btnsearch.TabIndex = 1
        Me.btnsearch.Text = "Search"
        Me.btnsearch.UseVisualStyleBackColor = True
        '
        'cmbKind
        '
        Me.cmbKind.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbKind.FormattingEnabled = True
        Me.cmbKind.Location = New System.Drawing.Point(395, 26)
        Me.cmbKind.Name = "cmbKind"
        Me.cmbKind.Size = New System.Drawing.Size(262, 28)
        Me.cmbKind.TabIndex = 2
        '
        'btnExportToExcel
        '
        Me.btnExportToExcel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExportToExcel.Location = New System.Drawing.Point(12, 44)
        Me.btnExportToExcel.Name = "btnExportToExcel"
        Me.btnExportToExcel.Size = New System.Drawing.Size(150, 58)
        Me.btnExportToExcel.TabIndex = 3
        Me.btnExportToExcel.Text = "Export To Excell"
        Me.btnExportToExcel.UseVisualStyleBackColor = True
        '
        'reportlivebatchform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1590, 811)
        Me.Controls.Add(Me.btnExportToExcel)
        Me.Controls.Add(Me.cmbKind)
        Me.Controls.Add(Me.btnsearch)
        Me.Controls.Add(Me.dgvDetails)
        Me.Name = "reportlivebatchform"
        Me.Text = "reportlivebatchform"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.dgvDetails, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents dgvDetails As DataGridView
    Friend WithEvents btnsearch As Button
    Friend WithEvents cmbKind As ComboBox
    Friend WithEvents btnExportToExcel As Button
End Class
