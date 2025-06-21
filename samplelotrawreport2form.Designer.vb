<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class samplelotrawreport2form
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
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnExportToExcel = New System.Windows.Forms.Button()
        Me.dgvResults = New System.Windows.Forms.DataGridView()
        Me.dtpto = New System.Windows.Forms.DateTimePicker()
        Me.dtpfrom = New System.Windows.Forms.DateTimePicker()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.cmblot = New System.Windows.Forms.ComboBox()
        Me.cmbBatch = New System.Windows.Forms.ComboBox()
        CType(Me.dgvResults, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(60, 47)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(59, 21)
        Me.Label4.TabIndex = 21
        Me.Label4.Text = "Batch"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(60, 118)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(38, 21)
        Me.Label3.TabIndex = 20
        Me.Label3.Text = "Lot"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(617, 90)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(31, 21)
        Me.Label2.TabIndex = 19
        Me.Label2.Text = "To"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(599, 30)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(47, 21)
        Me.Label1.TabIndex = 18
        Me.Label1.Text = "Fom"
        '
        'btnExportToExcel
        '
        Me.btnExportToExcel.Location = New System.Drawing.Point(46, 183)
        Me.btnExportToExcel.Name = "btnExportToExcel"
        Me.btnExportToExcel.Size = New System.Drawing.Size(86, 23)
        Me.btnExportToExcel.TabIndex = 17
        Me.btnExportToExcel.Text = "Export"
        Me.btnExportToExcel.UseVisualStyleBackColor = True
        '
        'dgvResults
        '
        Me.dgvResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvResults.Location = New System.Drawing.Point(28, 249)
        Me.dgvResults.Name = "dgvResults"
        Me.dgvResults.RowHeadersWidth = 51
        Me.dgvResults.RowTemplate.Height = 26
        Me.dgvResults.Size = New System.Drawing.Size(1192, 449)
        Me.dgvResults.TabIndex = 16
        '
        'dtpto
        '
        Me.dtpto.Location = New System.Drawing.Point(686, 88)
        Me.dtpto.Name = "dtpto"
        Me.dtpto.Size = New System.Drawing.Size(228, 22)
        Me.dtpto.TabIndex = 15
        '
        'dtpfrom
        '
        Me.dtpfrom.Location = New System.Drawing.Point(686, 27)
        Me.dtpfrom.Name = "dtpfrom"
        Me.dtpfrom.Size = New System.Drawing.Size(228, 22)
        Me.dtpfrom.TabIndex = 14
        '
        'btnSearch
        '
        Me.btnSearch.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSearch.Location = New System.Drawing.Point(531, 183)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(111, 46)
        Me.btnSearch.TabIndex = 13
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'cmblot
        '
        Me.cmblot.FormattingEnabled = True
        Me.cmblot.Location = New System.Drawing.Point(175, 119)
        Me.cmblot.Name = "cmblot"
        Me.cmblot.Size = New System.Drawing.Size(167, 24)
        Me.cmblot.TabIndex = 12
        '
        'cmbBatch
        '
        Me.cmbBatch.FormattingEnabled = True
        Me.cmbBatch.Location = New System.Drawing.Point(175, 48)
        Me.cmbBatch.Name = "cmbBatch"
        Me.cmbBatch.Size = New System.Drawing.Size(167, 24)
        Me.cmbBatch.TabIndex = 11
        '
        'samplelotrawreport2form
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1571, 829)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnExportToExcel)
        Me.Controls.Add(Me.dgvResults)
        Me.Controls.Add(Me.dtpto)
        Me.Controls.Add(Me.dtpfrom)
        Me.Controls.Add(Me.btnSearch)
        Me.Controls.Add(Me.cmblot)
        Me.Controls.Add(Me.cmbBatch)
        Me.Name = "samplelotrawreport2form"
        Me.Text = "samplelotrawreport2form"
        CType(Me.dgvResults, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents btnExportToExcel As Button
    Friend WithEvents dgvResults As DataGridView
    Friend WithEvents dtpto As DateTimePicker
    Friend WithEvents dtpfrom As DateTimePicker
    Friend WithEvents btnSearch As Button
    Friend WithEvents cmblot As ComboBox
    Friend WithEvents cmbBatch As ComboBox
End Class
