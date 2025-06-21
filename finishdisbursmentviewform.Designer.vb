<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class finishdisbursmentviewform
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
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.txtrefprint = New System.Windows.Forms.TextBox()
        Me.btnprint = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cmbref = New System.Windows.Forms.ComboBox()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtContractNo = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.dtptodate = New System.Windows.Forms.DateTimePicker()
        Me.dtpfromdate = New System.Windows.Forms.DateTimePicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtworderid = New System.Windows.Forms.TextBox()
        Me.dgvResults = New System.Windows.Forms.DataGridView()
        CType(Me.dgvResults, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtrefprint
        '
        Me.txtrefprint.Location = New System.Drawing.Point(618, 75)
        Me.txtrefprint.Name = "txtrefprint"
        Me.txtrefprint.Size = New System.Drawing.Size(361, 24)
        Me.txtrefprint.TabIndex = 29
        '
        'btnprint
        '
        Me.btnprint.Location = New System.Drawing.Point(479, 74)
        Me.btnprint.Name = "btnprint"
        Me.btnprint.Size = New System.Drawing.Size(75, 28)
        Me.btnprint.TabIndex = 28
        Me.btnprint.Text = "Print"
        Me.btnprint.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(28, 134)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(78, 24)
        Me.Label5.TabIndex = 27
        Me.Label5.Text = "اذن بيع"
        '
        'cmbref
        '
        Me.cmbref.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbref.FormattingEnabled = True
        Me.cmbref.Location = New System.Drawing.Point(134, 134)
        Me.cmbref.Name = "cmbref"
        Me.cmbref.Size = New System.Drawing.Size(216, 29)
        Me.cmbref.TabIndex = 26
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(1091, 82)
        Me.btnExport.Margin = New System.Windows.Forms.Padding(4)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(138, 64)
        Me.btnExport.TabIndex = 25
        Me.btnExport.Text = "Export To Excell"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSearch.Location = New System.Drawing.Point(618, 125)
        Me.btnSearch.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(157, 43)
        Me.btnSearch.TabIndex = 24
        Me.btnSearch.Text = "بحث"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(8, 82)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(98, 21)
        Me.Label4.TabIndex = 23
        Me.Label4.Text = "رقم التعاقد"
        '
        'txtContractNo
        '
        Me.txtContractNo.Location = New System.Drawing.Point(134, 82)
        Me.txtContractNo.Margin = New System.Windows.Forms.Padding(4)
        Me.txtContractNo.Name = "txtContractNo"
        Me.txtContractNo.Size = New System.Drawing.Size(216, 24)
        Me.txtContractNo.TabIndex = 22
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(1043, 21)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(31, 21)
        Me.Label3.TabIndex = 21
        Me.Label3.Text = "To"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(559, 17)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(54, 21)
        Me.Label2.TabIndex = 20
        Me.Label2.Text = "From"
        '
        'dtptodate
        '
        Me.dtptodate.Location = New System.Drawing.Point(1091, 21)
        Me.dtptodate.Margin = New System.Windows.Forms.Padding(4)
        Me.dtptodate.Name = "dtptodate"
        Me.dtptodate.Size = New System.Drawing.Size(256, 24)
        Me.dtptodate.TabIndex = 19
        '
        'dtpfromdate
        '
        Me.dtpfromdate.Location = New System.Drawing.Point(636, 21)
        Me.dtpfromdate.Margin = New System.Windows.Forms.Padding(4)
        Me.dtpfromdate.Name = "dtpfromdate"
        Me.dtpfromdate.Size = New System.Drawing.Size(256, 24)
        Me.dtpfromdate.TabIndex = 18
        Me.dtpfromdate.Value = New Date(2024, 1, 1, 0, 0, 0, 0)
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(21, 17)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(82, 21)
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "أمر شغل"
        '
        'txtworderid
        '
        Me.txtworderid.Location = New System.Drawing.Point(134, 13)
        Me.txtworderid.Margin = New System.Windows.Forms.Padding(4)
        Me.txtworderid.Name = "txtworderid"
        Me.txtworderid.Size = New System.Drawing.Size(216, 24)
        Me.txtworderid.TabIndex = 15
        '
        'dgvResults
        '
        Me.dgvResults.AllowUserToAddRows = False
        Me.dgvResults.AllowUserToDeleteRows = False
        Me.dgvResults.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dgvResults.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvResults.DefaultCellStyle = DataGridViewCellStyle3
        Me.dgvResults.Location = New System.Drawing.Point(13, 210)
        Me.dgvResults.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvResults.Name = "dgvResults"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvResults.RowHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.dgvResults.RowTemplate.Height = 26
        Me.dgvResults.Size = New System.Drawing.Size(1742, 469)
        Me.dgvResults.TabIndex = 30
        '
        'finishdisbursmentviewform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1814, 887)
        Me.Controls.Add(Me.dgvResults)
        Me.Controls.Add(Me.txtrefprint)
        Me.Controls.Add(Me.btnprint)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.cmbref)
        Me.Controls.Add(Me.btnExport)
        Me.Controls.Add(Me.btnSearch)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtContractNo)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.dtptodate)
        Me.Controls.Add(Me.dtpfromdate)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtworderid)
        Me.Name = "finishdisbursmentviewform"
        Me.Text = "مراجعه اذون صرف المجهز والطباعه"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.dgvResults, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtrefprint As System.Windows.Forms.TextBox
    Friend WithEvents btnprint As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents cmbref As System.Windows.Forms.ComboBox
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtContractNo As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents dtptodate As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpfromdate As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtworderid As System.Windows.Forms.TextBox
    Friend WithEvents dgvResults As System.Windows.Forms.DataGridView
End Class
