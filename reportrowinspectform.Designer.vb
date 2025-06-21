<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class reportrowinspectform
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
        Me.btnsearch2 = New System.Windows.Forms.Button()
        Me.btnExportToExcel = New System.Windows.Forms.Button()
        Me.lblCountRolls = New System.Windows.Forms.Label()
        Me.lblTotalWeight = New System.Windows.Forms.Label()
        Me.lblTotalHeight = New System.Windows.Forms.Label()
        Me.cmbShift = New System.Windows.Forms.ComboBox()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.dgvResults = New System.Windows.Forms.DataGridView()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtBatch = New System.Windows.Forms.TextBox()
        Me.txtContract = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.dtpDateto = New System.Windows.Forms.DateTimePicker()
        Me.dtpDateFrom = New System.Windows.Forms.DateTimePicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtWorderId = New System.Windows.Forms.TextBox()
        CType(Me.dgvResults, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnsearch2
        '
        Me.btnsearch2.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnsearch2.Location = New System.Drawing.Point(730, 193)
        Me.btnsearch2.Name = "btnsearch2"
        Me.btnsearch2.Size = New System.Drawing.Size(132, 37)
        Me.btnsearch2.TabIndex = 35
        Me.btnsearch2.Text = "Search 2"
        Me.btnsearch2.UseVisualStyleBackColor = True
        '
        'btnExportToExcel
        '
        Me.btnExportToExcel.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExportToExcel.Location = New System.Drawing.Point(42, 227)
        Me.btnExportToExcel.Name = "btnExportToExcel"
        Me.btnExportToExcel.Size = New System.Drawing.Size(127, 55)
        Me.btnExportToExcel.TabIndex = 34
        Me.btnExportToExcel.Text = "Export Excell"
        Me.btnExportToExcel.UseVisualStyleBackColor = True
        '
        'lblCountRolls
        '
        Me.lblCountRolls.AutoSize = True
        Me.lblCountRolls.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCountRolls.Location = New System.Drawing.Point(995, 212)
        Me.lblCountRolls.Name = "lblCountRolls"
        Me.lblCountRolls.Size = New System.Drawing.Size(102, 21)
        Me.lblCountRolls.TabIndex = 33
        Me.lblCountRolls.Text = "اجمالى اتواب"
        '
        'lblTotalWeight
        '
        Me.lblTotalWeight.AutoSize = True
        Me.lblTotalWeight.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalWeight.Location = New System.Drawing.Point(995, 162)
        Me.lblTotalWeight.Name = "lblTotalWeight"
        Me.lblTotalWeight.Size = New System.Drawing.Size(91, 21)
        Me.lblTotalWeight.TabIndex = 32
        Me.lblTotalWeight.Text = "اجمالى وزن"
        '
        'lblTotalHeight
        '
        Me.lblTotalHeight.AutoSize = True
        Me.lblTotalHeight.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalHeight.Location = New System.Drawing.Point(995, 118)
        Me.lblTotalHeight.Name = "lblTotalHeight"
        Me.lblTotalHeight.Size = New System.Drawing.Size(89, 21)
        Me.lblTotalHeight.TabIndex = 31
        Me.lblTotalHeight.Text = "اجمالى متر"
        '
        'cmbShift
        '
        Me.cmbShift.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbShift.FormattingEnabled = True
        Me.cmbShift.Location = New System.Drawing.Point(919, 64)
        Me.cmbShift.Name = "cmbShift"
        Me.cmbShift.Size = New System.Drawing.Size(230, 29)
        Me.cmbShift.TabIndex = 30
        '
        'btnSearch
        '
        Me.btnSearch.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSearch.Location = New System.Drawing.Point(467, 184)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(182, 49)
        Me.btnSearch.TabIndex = 29
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'dgvResults
        '
        Me.dgvResults.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dgvResults.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvResults.Location = New System.Drawing.Point(33, 288)
        Me.dgvResults.Name = "dgvResults"
        Me.dgvResults.RowHeadersWidth = 51
        Me.dgvResults.RowTemplate.Height = 26
        Me.dgvResults.Size = New System.Drawing.Size(1445, 536)
        Me.dgvResults.TabIndex = 28
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(71, 87)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(98, 21)
        Me.Label5.TabIndex = 27
        Me.Label5.Text = "رقم التعاقد"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(75, 149)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(72, 21)
        Me.Label4.TabIndex = 26
        Me.Label4.Text = "الرسالة"
        '
        'txtBatch
        '
        Me.txtBatch.Location = New System.Drawing.Point(191, 149)
        Me.txtBatch.Name = "txtBatch"
        Me.txtBatch.Size = New System.Drawing.Size(190, 24)
        Me.txtBatch.TabIndex = 25
        '
        'txtContract
        '
        Me.txtContract.Location = New System.Drawing.Point(191, 88)
        Me.txtContract.Name = "txtContract"
        Me.txtContract.Size = New System.Drawing.Size(190, 24)
        Me.txtContract.TabIndex = 24
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(496, 101)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(76, 21)
        Me.Label3.TabIndex = 23
        Me.Label3.Text = "Date To"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(483, 38)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(99, 21)
        Me.Label2.TabIndex = 22
        Me.Label2.Text = "Date From"
        '
        'dtpDateto
        '
        Me.dtpDateto.Location = New System.Drawing.Point(588, 101)
        Me.dtpDateto.Name = "dtpDateto"
        Me.dtpDateto.Size = New System.Drawing.Size(200, 24)
        Me.dtpDateto.TabIndex = 21
        '
        'dtpDateFrom
        '
        Me.dtpDateFrom.Location = New System.Drawing.Point(588, 41)
        Me.dtpDateFrom.Name = "dtpDateFrom"
        Me.dtpDateFrom.Size = New System.Drawing.Size(200, 24)
        Me.dtpDateFrom.TabIndex = 20
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(71, 34)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(82, 21)
        Me.Label1.TabIndex = 19
        Me.Label1.Text = "امر شغل"
        '
        'txtWorderId
        '
        Me.txtWorderId.Location = New System.Drawing.Point(191, 35)
        Me.txtWorderId.Name = "txtWorderId"
        Me.txtWorderId.Size = New System.Drawing.Size(190, 24)
        Me.txtWorderId.TabIndex = 18
        '
        'reportrowinspectform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1305, 775)
        Me.Controls.Add(Me.btnsearch2)
        Me.Controls.Add(Me.btnExportToExcel)
        Me.Controls.Add(Me.lblCountRolls)
        Me.Controls.Add(Me.lblTotalWeight)
        Me.Controls.Add(Me.lblTotalHeight)
        Me.Controls.Add(Me.cmbShift)
        Me.Controls.Add(Me.btnSearch)
        Me.Controls.Add(Me.dgvResults)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtBatch)
        Me.Controls.Add(Me.txtContract)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.dtpDateto)
        Me.Controls.Add(Me.dtpDateFrom)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtWorderId)
        Me.Name = "reportrowinspectform"
        Me.Text = "reportrowinspectform"
        CType(Me.dgvResults, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnsearch2 As Button
    Friend WithEvents btnExportToExcel As Button
    Friend WithEvents lblCountRolls As Label
    Friend WithEvents lblTotalWeight As Label
    Friend WithEvents lblTotalHeight As Label
    Friend WithEvents cmbShift As ComboBox
    Friend WithEvents btnSearch As Button
    Friend WithEvents dgvResults As DataGridView
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents txtBatch As TextBox
    Friend WithEvents txtContract As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents dtpDateto As DateTimePicker
    Friend WithEvents dtpDateFrom As DateTimePicker
    Friend WithEvents Label1 As Label
    Friend WithEvents txtWorderId As TextBox
End Class
