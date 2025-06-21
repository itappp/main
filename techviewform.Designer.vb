<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class techviewform
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
        Me.dataGridView1 = New System.Windows.Forms.DataGridView()
        Me.txtWorderID = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnsearch = New System.Windows.Forms.Button()
        Me.btnExportExcel = New System.Windows.Forms.Button()
        Me.txtcode = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.comboBoxSearchOption = New System.Windows.Forms.ComboBox()
        Me.btnprint = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtcontractno = New System.Windows.Forms.TextBox()
        Me.btnprintworder = New System.Windows.Forms.Button()
        CType(Me.dataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
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
        Me.dataGridView1.Location = New System.Drawing.Point(16, 222)
        Me.dataGridView1.Name = "dataGridView1"
        Me.dataGridView1.RowHeadersWidth = 51
        DataGridViewCellStyle3.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Tahoma", 13.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black
        Me.dataGridView1.RowsDefaultCellStyle = DataGridViewCellStyle3
        Me.dataGridView1.RowTemplate.Height = 26
        Me.dataGridView1.Size = New System.Drawing.Size(1315, 602)
        Me.dataGridView1.TabIndex = 0
        '
        'txtWorderID
        '
        Me.txtWorderID.Location = New System.Drawing.Point(160, 35)
        Me.txtWorderID.Name = "txtWorderID"
        Me.txtWorderID.Size = New System.Drawing.Size(213, 28)
        Me.txtWorderID.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(62, 40)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(83, 24)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "worder"
        '
        'btnsearch
        '
        Me.btnsearch.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnsearch.Location = New System.Drawing.Point(576, 140)
        Me.btnsearch.Name = "btnsearch"
        Me.btnsearch.Size = New System.Drawing.Size(241, 59)
        Me.btnsearch.TabIndex = 4
        Me.btnsearch.Text = "Search"
        Me.btnsearch.UseVisualStyleBackColor = True
        '
        'btnExportExcel
        '
        Me.btnExportExcel.Location = New System.Drawing.Point(16, 150)
        Me.btnExportExcel.Name = "btnExportExcel"
        Me.btnExportExcel.Size = New System.Drawing.Size(150, 49)
        Me.btnExportExcel.TabIndex = 5
        Me.btnExportExcel.Text = "Export To Excell"
        Me.btnExportExcel.UseVisualStyleBackColor = True
        '
        'txtcode
        '
        Me.txtcode.Location = New System.Drawing.Point(946, 32)
        Me.txtcode.Name = "txtcode"
        Me.txtcode.Size = New System.Drawing.Size(178, 28)
        Me.txtcode.TabIndex = 6
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(876, 32)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(55, 24)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Code"
        '
        'comboBoxSearchOption
        '
        Me.comboBoxSearchOption.FormattingEnabled = True
        Me.comboBoxSearchOption.Items.AddRange(New Object() {"WorderID", "Code"})
        Me.comboBoxSearchOption.Location = New System.Drawing.Point(946, 140)
        Me.comboBoxSearchOption.Name = "comboBoxSearchOption"
        Me.comboBoxSearchOption.Size = New System.Drawing.Size(194, 29)
        Me.comboBoxSearchOption.TabIndex = 8
        '
        'btnprint
        '
        Me.btnprint.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnprint.Location = New System.Drawing.Point(230, 155)
        Me.btnprint.Name = "btnprint"
        Me.btnprint.Size = New System.Drawing.Size(124, 38)
        Me.btnprint.TabIndex = 9
        Me.btnprint.Text = "Print"
        Me.btnprint.UseVisualStyleBackColor = True
        Me.btnprint.Visible = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(496, 35)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(86, 21)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "رقم تعاقد"
        '
        'txtcontractno
        '
        Me.txtcontractno.Location = New System.Drawing.Point(588, 32)
        Me.txtcontractno.Name = "txtcontractno"
        Me.txtcontractno.Size = New System.Drawing.Size(169, 28)
        Me.txtcontractno.TabIndex = 11
        '
        'btnprintworder
        '
        Me.btnprintworder.Location = New System.Drawing.Point(397, 111)
        Me.btnprintworder.Name = "btnprintworder"
        Me.btnprintworder.Size = New System.Drawing.Size(75, 37)
        Me.btnprintworder.TabIndex = 12
        Me.btnprintworder.Text = "Button1"
        Me.btnprintworder.UseVisualStyleBackColor = True
        Me.btnprintworder.Visible = False
        '
        'techviewform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(1518, 836)
        Me.Controls.Add(Me.btnprintworder)
        Me.Controls.Add(Me.txtcontractno)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.btnprint)
        Me.Controls.Add(Me.comboBoxSearchOption)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtcode)
        Me.Controls.Add(Me.btnExportExcel)
        Me.Controls.Add(Me.btnsearch)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtWorderID)
        Me.Controls.Add(Me.dataGridView1)
        Me.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "techviewform"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Text = "techviewform"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.dataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents txtWorderID As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents DateTimePicker1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents btnsearch As System.Windows.Forms.Button
    Friend WithEvents btnExportExcel As System.Windows.Forms.Button
    Friend WithEvents txtcode As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents comboBoxSearchOption As System.Windows.Forms.ComboBox
    Friend WithEvents btnprint As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtcontractno As System.Windows.Forms.TextBox
    Friend WithEvents btnprintworder As Button
End Class
