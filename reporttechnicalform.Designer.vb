<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class reporttechnicalform
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.comboBoxSearchOption = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtcode = New System.Windows.Forms.TextBox()
        Me.btnExportExcel = New System.Windows.Forms.Button()
        Me.btnsearch = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtWorderID = New System.Windows.Forms.TextBox()
        Me.dataGridView1 = New System.Windows.Forms.DataGridView()
        Me.txtcontractno = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        CType(Me.dataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'comboBoxSearchOption
        '
        Me.comboBoxSearchOption.FormattingEnabled = True
        Me.comboBoxSearchOption.Items.AddRange(New Object() {"WorderID", "Code"})
        Me.comboBoxSearchOption.Location = New System.Drawing.Point(919, 142)
        Me.comboBoxSearchOption.Name = "comboBoxSearchOption"
        Me.comboBoxSearchOption.Size = New System.Drawing.Size(194, 24)
        Me.comboBoxSearchOption.TabIndex = 17
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(830, 41)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(55, 24)
        Me.Label2.TabIndex = 16
        Me.Label2.Text = "Code"
        '
        'txtcode
        '
        Me.txtcode.Location = New System.Drawing.Point(902, 37)
        Me.txtcode.Name = "txtcode"
        Me.txtcode.Size = New System.Drawing.Size(235, 24)
        Me.txtcode.TabIndex = 15
        '
        'btnExportExcel
        '
        Me.btnExportExcel.Location = New System.Drawing.Point(52, 152)
        Me.btnExportExcel.Name = "btnExportExcel"
        Me.btnExportExcel.Size = New System.Drawing.Size(150, 49)
        Me.btnExportExcel.TabIndex = 14
        Me.btnExportExcel.Text = "Export To Excell"
        Me.btnExportExcel.UseVisualStyleBackColor = True
        '
        'btnsearch
        '
        Me.btnsearch.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnsearch.Location = New System.Drawing.Point(549, 142)
        Me.btnsearch.Name = "btnsearch"
        Me.btnsearch.Size = New System.Drawing.Size(241, 59)
        Me.btnsearch.TabIndex = 13
        Me.btnsearch.Text = "Search"
        Me.btnsearch.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(35, 42)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(83, 24)
        Me.Label1.TabIndex = 12
        Me.Label1.Text = "worder"
        '
        'txtWorderID
        '
        Me.txtWorderID.Location = New System.Drawing.Point(133, 37)
        Me.txtWorderID.Name = "txtWorderID"
        Me.txtWorderID.Size = New System.Drawing.Size(165, 24)
        Me.txtWorderID.TabIndex = 11
        '
        'dataGridView1
        '
        Me.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders
        Me.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ActiveCaption
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dataGridView1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dataGridView1.DefaultCellStyle = DataGridViewCellStyle2
        Me.dataGridView1.Location = New System.Drawing.Point(12, 224)
        Me.dataGridView1.Name = "dataGridView1"
        Me.dataGridView1.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        DataGridViewCellStyle3.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Tahoma", 13.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black
        Me.dataGridView1.RowsDefaultCellStyle = DataGridViewCellStyle3
        Me.dataGridView1.RowTemplate.Height = 26
        Me.dataGridView1.Size = New System.Drawing.Size(1523, 602)
        Me.dataGridView1.TabIndex = 10
        '
        'txtcontractno
        '
        Me.txtcontractno.Location = New System.Drawing.Point(549, 37)
        Me.txtcontractno.Name = "txtcontractno"
        Me.txtcontractno.Size = New System.Drawing.Size(115, 24)
        Me.txtcontractno.TabIndex = 18
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(436, 36)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(98, 21)
        Me.Label3.TabIndex = 19
        Me.Label3.Text = "رقم التعاقد"
        '
        'reporttechnicalform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1841, 876)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtcontractno)
        Me.Controls.Add(Me.comboBoxSearchOption)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtcode)
        Me.Controls.Add(Me.btnExportExcel)
        Me.Controls.Add(Me.btnsearch)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtWorderID)
        Me.Controls.Add(Me.dataGridView1)
        Me.Name = "reporttechnicalform"
        Me.Text = "تقرير المكتب الفنى"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.dataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents comboBoxSearchOption As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtcode As System.Windows.Forms.TextBox
    Friend WithEvents btnExportExcel As System.Windows.Forms.Button
    Friend WithEvents btnsearch As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtWorderID As System.Windows.Forms.TextBox
    Friend WithEvents dataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents txtcontractno As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
End Class
