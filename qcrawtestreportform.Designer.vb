<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class qcrawtestreportform
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
        Me.dgvqc = New System.Windows.Forms.DataGridView()
        Me.cmbbatch = New System.Windows.Forms.ComboBox()
        Me.btnsearch = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnExportToExcel = New System.Windows.Forms.Button()
        CType(Me.dgvqc, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvqc
        '
        Me.dgvqc.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dgvqc.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvqc.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvqc.Location = New System.Drawing.Point(12, 245)
        Me.dgvqc.Name = "dgvqc"
        Me.dgvqc.RowHeadersWidth = 51
        Me.dgvqc.RowTemplate.Height = 26
        Me.dgvqc.Size = New System.Drawing.Size(1377, 378)
        Me.dgvqc.TabIndex = 0
        '
        'cmbbatch
        '
        Me.cmbbatch.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbbatch.FormattingEnabled = True
        Me.cmbbatch.Location = New System.Drawing.Point(300, 96)
        Me.cmbbatch.Name = "cmbbatch"
        Me.cmbbatch.Size = New System.Drawing.Size(225, 32)
        Me.cmbbatch.TabIndex = 1
        '
        'btnsearch
        '
        Me.btnsearch.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnsearch.Location = New System.Drawing.Point(640, 177)
        Me.btnsearch.Name = "btnsearch"
        Me.btnsearch.Size = New System.Drawing.Size(102, 34)
        Me.btnsearch.TabIndex = 2
        Me.btnsearch.Text = "Search"
        Me.btnsearch.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(392, 41)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(58, 21)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "batch"
        '
        'btnExportToExcel
        '
        Me.btnExportToExcel.Location = New System.Drawing.Point(33, 188)
        Me.btnExportToExcel.Name = "btnExportToExcel"
        Me.btnExportToExcel.Size = New System.Drawing.Size(100, 35)
        Me.btnExportToExcel.TabIndex = 4
        Me.btnExportToExcel.Text = "Export To Excell"
        Me.btnExportToExcel.UseVisualStyleBackColor = True
        '
        'qcrawtestreportform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1435, 702)
        Me.Controls.Add(Me.btnExportToExcel)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnsearch)
        Me.Controls.Add(Me.cmbbatch)
        Me.Controls.Add(Me.dgvqc)
        Me.Name = "qcrawtestreportform"
        Me.Text = "qcrawtestreportform"
        CType(Me.dgvqc, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents dgvqc As DataGridView
    Friend WithEvents cmbbatch As ComboBox
    Friend WithEvents btnsearch As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents btnExportToExcel As Button
End Class
