<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class powerpiform
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
        Me.dgvResults = New System.Windows.Forms.DataGridView()
        Me.dtpDateFrom = New System.Windows.Forms.DateTimePicker()
        Me.dtpDateto = New System.Windows.Forms.DateTimePicker()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.cmbShift = New System.Windows.Forms.ComboBox()
        Me.btnGenerateReport = New System.Windows.Forms.Button()
        Me.dgvStoreFinishResults = New System.Windows.Forms.DataGridView()
        CType(Me.dgvResults, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvStoreFinishResults, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvResults
        '
        Me.dgvResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvResults.Location = New System.Drawing.Point(547, 196)
        Me.dgvResults.Name = "dgvResults"
        Me.dgvResults.RowHeadersWidth = 51
        Me.dgvResults.RowTemplate.Height = 24
        Me.dgvResults.Size = New System.Drawing.Size(668, 296)
        Me.dgvResults.TabIndex = 0
        '
        'dtpDateFrom
        '
        Me.dtpDateFrom.Location = New System.Drawing.Point(191, 44)
        Me.dtpDateFrom.Name = "dtpDateFrom"
        Me.dtpDateFrom.Size = New System.Drawing.Size(200, 22)
        Me.dtpDateFrom.TabIndex = 1
        '
        'dtpDateto
        '
        Me.dtpDateto.Location = New System.Drawing.Point(701, 44)
        Me.dtpDateto.Name = "dtpDateto"
        Me.dtpDateto.Size = New System.Drawing.Size(200, 22)
        Me.dtpDateto.TabIndex = 2
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(539, 88)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(75, 23)
        Me.btnSearch.TabIndex = 3
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'cmbShift
        '
        Me.cmbShift.FormattingEnabled = True
        Me.cmbShift.Location = New System.Drawing.Point(1069, 41)
        Me.cmbShift.Name = "cmbShift"
        Me.cmbShift.Size = New System.Drawing.Size(121, 24)
        Me.cmbShift.TabIndex = 4
        '
        'btnGenerateReport
        '
        Me.btnGenerateReport.Location = New System.Drawing.Point(968, 127)
        Me.btnGenerateReport.Name = "btnGenerateReport"
        Me.btnGenerateReport.Size = New System.Drawing.Size(75, 23)
        Me.btnGenerateReport.TabIndex = 5
        Me.btnGenerateReport.Text = "Search"
        Me.btnGenerateReport.UseVisualStyleBackColor = True
        '
        'dgvStoreFinishResults
        '
        Me.dgvStoreFinishResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvStoreFinishResults.Location = New System.Drawing.Point(63, 259)
        Me.dgvStoreFinishResults.Name = "dgvStoreFinishResults"
        Me.dgvStoreFinishResults.RowHeadersWidth = 51
        Me.dgvStoreFinishResults.RowTemplate.Height = 24
        Me.dgvStoreFinishResults.Size = New System.Drawing.Size(387, 233)
        Me.dgvStoreFinishResults.TabIndex = 6
        '
        'powerpiform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1279, 646)
        Me.Controls.Add(Me.dgvStoreFinishResults)
        Me.Controls.Add(Me.btnGenerateReport)
        Me.Controls.Add(Me.cmbShift)
        Me.Controls.Add(Me.btnSearch)
        Me.Controls.Add(Me.dtpDateto)
        Me.Controls.Add(Me.dtpDateFrom)
        Me.Controls.Add(Me.dgvResults)
        Me.Name = "powerpiform"
        Me.Text = "powerpiform"
        CType(Me.dgvResults, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvStoreFinishResults, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents dgvResults As DataGridView
    Friend WithEvents dtpDateFrom As DateTimePicker
    Friend WithEvents dtpDateto As DateTimePicker
    Friend WithEvents btnSearch As Button
    Friend WithEvents cmbShift As ComboBox
    Friend WithEvents btnGenerateReport As Button
    Friend WithEvents dgvStoreFinishResults As DataGridView
End Class
