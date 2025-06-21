<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class samplelotrawreportform
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
        Me.cmbBatch = New System.Windows.Forms.ComboBox()
        Me.cmblot = New System.Windows.Forms.ComboBox()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.dtpfrom = New System.Windows.Forms.DateTimePicker()
        Me.dtpto = New System.Windows.Forms.DateTimePicker()
        Me.dgvResults = New System.Windows.Forms.DataGridView()
        Me.btnExportToExcel = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        CType(Me.dgvResults, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmbBatch
        '
        Me.cmbBatch.FormattingEnabled = True
        Me.cmbBatch.Location = New System.Drawing.Point(141, 35)
        Me.cmbBatch.Name = "cmbBatch"
        Me.cmbBatch.Size = New System.Drawing.Size(147, 24)
        Me.cmbBatch.TabIndex = 0
        '
        'cmblot
        '
        Me.cmblot.FormattingEnabled = True
        Me.cmblot.Location = New System.Drawing.Point(141, 106)
        Me.cmblot.Name = "cmblot"
        Me.cmblot.Size = New System.Drawing.Size(147, 24)
        Me.cmblot.TabIndex = 1
        '
        'btnSearch
        '
        Me.btnSearch.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSearch.Location = New System.Drawing.Point(452, 170)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(97, 46)
        Me.btnSearch.TabIndex = 2
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'dtpfrom
        '
        Me.dtpfrom.Location = New System.Drawing.Point(588, 14)
        Me.dtpfrom.Name = "dtpfrom"
        Me.dtpfrom.Size = New System.Drawing.Size(200, 24)
        Me.dtpfrom.TabIndex = 3
        '
        'dtpto
        '
        Me.dtpto.Location = New System.Drawing.Point(588, 75)
        Me.dtpto.Name = "dtpto"
        Me.dtpto.Size = New System.Drawing.Size(200, 24)
        Me.dtpto.TabIndex = 4
        '
        'dgvResults
        '
        Me.dgvResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvResults.Location = New System.Drawing.Point(12, 236)
        Me.dgvResults.Name = "dgvResults"
        Me.dgvResults.RowHeadersWidth = 51
        Me.dgvResults.RowTemplate.Height = 26
        Me.dgvResults.Size = New System.Drawing.Size(1043, 449)
        Me.dgvResults.TabIndex = 5
        '
        'btnExportToExcel
        '
        Me.btnExportToExcel.Location = New System.Drawing.Point(28, 170)
        Me.btnExportToExcel.Name = "btnExportToExcel"
        Me.btnExportToExcel.Size = New System.Drawing.Size(75, 23)
        Me.btnExportToExcel.TabIndex = 6
        Me.btnExportToExcel.Text = "Export"
        Me.btnExportToExcel.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(512, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(47, 21)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Fom"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(528, 77)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(31, 21)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "To"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(40, 105)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(38, 21)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Lot"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(40, 34)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(59, 21)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "Batch"
        '
        'samplelotrawreportform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1192, 822)
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
        Me.Name = "samplelotrawreportform"
        Me.Text = "samplelotrawreportform"
        CType(Me.dgvResults, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents cmbBatch As ComboBox
    Friend WithEvents cmblot As ComboBox
    Friend WithEvents btnSearch As Button
    Friend WithEvents dtpfrom As DateTimePicker
    Friend WithEvents dtpto As DateTimePicker
    Friend WithEvents dgvResults As DataGridView
    Friend WithEvents btnExportToExcel As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
End Class
