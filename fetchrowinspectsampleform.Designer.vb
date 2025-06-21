<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fetchrowinspectsampleform
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
        Me.lblUsername = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmbuser2 = New System.Windows.Forms.ComboBox()
        Me.btninsert = New System.Windows.Forms.Button()
        Me.dataGridViewDefects = New System.Windows.Forms.DataGridView()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtnotes = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtSpeed = New System.Windows.Forms.TextBox()
        Me.txtWidth = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtHeight = New System.Windows.Forms.TextBox()
        Me.btnFetchData = New System.Windows.Forms.Button()
        Me.dataGridViewDetails = New System.Windows.Forms.DataGridView()
        CType(Me.dataGridViewDefects, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dataGridViewDetails, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblUsername
        '
        Me.lblUsername.AutoSize = True
        Me.lblUsername.Location = New System.Drawing.Point(28, 20)
        Me.lblUsername.Name = "lblUsername"
        Me.lblUsername.Size = New System.Drawing.Size(34, 17)
        Me.lblUsername.TabIndex = 82
        Me.lblUsername.Text = "user"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(568, 114)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(114, 21)
        Me.Label2.TabIndex = 108
        Me.Label2.Text = "عامل مساعد"
        '
        'cmbuser2
        '
        Me.cmbuser2.Font = New System.Drawing.Font("Tahoma", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbuser2.FormattingEnabled = True
        Me.cmbuser2.Location = New System.Drawing.Point(516, 150)
        Me.cmbuser2.Name = "cmbuser2"
        Me.cmbuser2.Size = New System.Drawing.Size(209, 30)
        Me.cmbuser2.TabIndex = 107
        '
        'btninsert
        '
        Me.btninsert.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btninsert.Location = New System.Drawing.Point(961, 162)
        Me.btninsert.Name = "btninsert"
        Me.btninsert.Size = New System.Drawing.Size(155, 44)
        Me.btninsert.TabIndex = 106
        Me.btninsert.Text = "تسجيل"
        Me.btninsert.UseVisualStyleBackColor = True
        '
        'dataGridViewDefects
        '
        Me.dataGridViewDefects.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dataGridViewDefects.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dataGridViewDefects.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dataGridViewDefects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dataGridViewDefects.DefaultCellStyle = DataGridViewCellStyle3
        Me.dataGridViewDefects.GridColor = System.Drawing.SystemColors.ActiveCaption
        Me.dataGridViewDefects.Location = New System.Drawing.Point(31, 344)
        Me.dataGridViewDefects.Name = "dataGridViewDefects"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.ActiveCaption
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dataGridViewDefects.RowHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.dataGridViewDefects.RowHeadersVisible = False
        Me.dataGridViewDefects.RowHeadersWidth = 51
        Me.dataGridViewDefects.RowTemplate.Height = 26
        Me.dataGridViewDefects.Size = New System.Drawing.Size(1146, 451)
        Me.dataGridViewDefects.TabIndex = 105
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(634, 217)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(82, 21)
        Me.Label6.TabIndex = 104
        Me.Label6.Text = "ملاحظات"
        '
        'txtnotes
        '
        Me.txtnotes.Location = New System.Drawing.Point(535, 251)
        Me.txtnotes.Multiline = True
        Me.txtnotes.Name = "txtnotes"
        Me.txtnotes.Size = New System.Drawing.Size(311, 70)
        Me.txtnotes.TabIndex = 103
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(189, 288)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(72, 21)
        Me.Label4.TabIndex = 102
        Me.Label4.Text = "السرعه"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(197, 240)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(64, 21)
        Me.Label3.TabIndex = 101
        Me.Label3.Text = "العرض"
        '
        'txtSpeed
        '
        Me.txtSpeed.Location = New System.Drawing.Point(37, 289)
        Me.txtSpeed.Name = "txtSpeed"
        Me.txtSpeed.Size = New System.Drawing.Size(86, 24)
        Me.txtSpeed.TabIndex = 100
        '
        'txtWidth
        '
        Me.txtWidth.Location = New System.Drawing.Point(37, 241)
        Me.txtWidth.Name = "txtWidth"
        Me.txtWidth.Size = New System.Drawing.Size(86, 24)
        Me.txtWidth.TabIndex = 99
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(209, 197)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(59, 21)
        Me.Label1.TabIndex = 98
        Me.Label1.Text = "الطول"
        '
        'txtHeight
        '
        Me.txtHeight.Location = New System.Drawing.Point(37, 198)
        Me.txtHeight.Name = "txtHeight"
        Me.txtHeight.Size = New System.Drawing.Size(85, 24)
        Me.txtHeight.TabIndex = 97
        '
        'btnFetchData
        '
        Me.btnFetchData.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFetchData.Location = New System.Drawing.Point(304, 229)
        Me.btnFetchData.Name = "btnFetchData"
        Me.btnFetchData.Size = New System.Drawing.Size(165, 63)
        Me.btnFetchData.TabIndex = 96
        Me.btnFetchData.Text = "بيانات المتر"
        Me.btnFetchData.UseVisualStyleBackColor = True
        '
        'dataGridViewDetails
        '
        Me.dataGridViewDetails.AllowUserToAddRows = False
        Me.dataGridViewDetails.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dataGridViewDetails.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dataGridViewDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dataGridViewDetails.Location = New System.Drawing.Point(193, 20)
        Me.dataGridViewDetails.Name = "dataGridViewDetails"
        Me.dataGridViewDetails.ReadOnly = True
        Me.dataGridViewDetails.RowHeadersWidth = 51
        Me.dataGridViewDetails.RowTemplate.Height = 26
        Me.dataGridViewDetails.Size = New System.Drawing.Size(923, 80)
        Me.dataGridViewDetails.TabIndex = 111
        '
        'fetchrowinspectsampleform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1230, 938)
        Me.Controls.Add(Me.dataGridViewDetails)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.cmbuser2)
        Me.Controls.Add(Me.btninsert)
        Me.Controls.Add(Me.dataGridViewDefects)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtnotes)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtSpeed)
        Me.Controls.Add(Me.txtWidth)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtHeight)
        Me.Controls.Add(Me.btnFetchData)
        Me.Controls.Add(Me.lblUsername)
        Me.Name = "fetchrowinspectsampleform"
        Me.Text = "fetchrowinspectsampleform"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.dataGridViewDefects, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dataGridViewDetails, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblUsername As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents cmbuser2 As ComboBox
    Friend WithEvents btninsert As Button
    Friend WithEvents dataGridViewDefects As DataGridView
    Friend WithEvents Label6 As Label
    Friend WithEvents txtnotes As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents txtSpeed As TextBox
    Friend WithEvents txtWidth As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents txtHeight As TextBox
    Friend WithEvents btnFetchData As Button
    Friend WithEvents dataGridViewDetails As DataGridView
End Class
