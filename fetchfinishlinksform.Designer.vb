<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fetchfinishlinksform
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
        Me.btnFetchWeight = New System.Windows.Forms.Button()
        Me.btnmenu = New System.Windows.Forms.Button()
        Me.lblcolor = New System.Windows.Forms.Label()
        Me.lblclient = New System.Windows.Forms.Label()
        Me.lbltotalw = New System.Windows.Forms.Label()
        Me.lbltotalm = New System.Windows.Forms.Label()
        Me.lblqtym = New System.Windows.Forms.Label()
        Me.lblqtykg = New System.Windows.Forms.Label()
        Me.lblcontractno = New System.Windows.Forms.Label()
        Me.lblbatchno = New System.Windows.Forms.Label()
        Me.btninsert = New System.Windows.Forms.Button()
        Me.dataGridViewDefects = New System.Windows.Forms.DataGridView()
        Me.lblUsername = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtnotes = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cmbworder = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtWeight = New System.Windows.Forms.TextBox()
        CType(Me.dataGridViewDefects, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnFetchWeight
        '
        Me.btnFetchWeight.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFetchWeight.Location = New System.Drawing.Point(314, 268)
        Me.btnFetchWeight.Name = "btnFetchWeight"
        Me.btnFetchWeight.Size = New System.Drawing.Size(162, 44)
        Me.btnFetchWeight.TabIndex = 94
        Me.btnFetchWeight.Text = "بيانات الوزن"
        Me.btnFetchWeight.UseVisualStyleBackColor = True
        '
        'btnmenu
        '
        Me.btnmenu.Location = New System.Drawing.Point(14, 98)
        Me.btnmenu.Name = "btnmenu"
        Me.btnmenu.Size = New System.Drawing.Size(74, 44)
        Me.btnmenu.TabIndex = 92
        Me.btnmenu.Text = "رجوع"
        Me.btnmenu.UseVisualStyleBackColor = True
        Me.btnmenu.Visible = False
        '
        'lblcolor
        '
        Me.lblcolor.AutoSize = True
        Me.lblcolor.Location = New System.Drawing.Point(311, 9)
        Me.lblcolor.Name = "lblcolor"
        Me.lblcolor.Size = New System.Drawing.Size(12, 17)
        Me.lblcolor.TabIndex = 91
        Me.lblcolor.Text = "."
        '
        'lblclient
        '
        Me.lblclient.AutoSize = True
        Me.lblclient.Location = New System.Drawing.Point(314, 55)
        Me.lblclient.Name = "lblclient"
        Me.lblclient.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblclient.Size = New System.Drawing.Size(12, 17)
        Me.lblclient.TabIndex = 90
        Me.lblclient.Text = "."
        '
        'lbltotalw
        '
        Me.lbltotalw.AutoSize = True
        Me.lbltotalw.Location = New System.Drawing.Point(530, 199)
        Me.lbltotalw.Name = "lbltotalw"
        Me.lbltotalw.Size = New System.Drawing.Size(12, 17)
        Me.lbltotalw.TabIndex = 89
        Me.lbltotalw.Text = "."
        '
        'lbltotalm
        '
        Me.lbltotalm.AutoSize = True
        Me.lbltotalm.Location = New System.Drawing.Point(728, 199)
        Me.lbltotalm.Name = "lbltotalm"
        Me.lbltotalm.Size = New System.Drawing.Size(12, 17)
        Me.lbltotalm.TabIndex = 88
        Me.lbltotalm.Text = "."
        '
        'lblqtym
        '
        Me.lblqtym.AutoSize = True
        Me.lblqtym.Location = New System.Drawing.Point(772, 55)
        Me.lblqtym.Name = "lblqtym"
        Me.lblqtym.Size = New System.Drawing.Size(12, 17)
        Me.lblqtym.TabIndex = 87
        Me.lblqtym.Text = "."
        '
        'lblqtykg
        '
        Me.lblqtykg.AutoSize = True
        Me.lblqtykg.Location = New System.Drawing.Point(541, 55)
        Me.lblqtykg.Name = "lblqtykg"
        Me.lblqtykg.Size = New System.Drawing.Size(12, 17)
        Me.lblqtykg.TabIndex = 86
        Me.lblqtykg.Text = "."
        '
        'lblcontractno
        '
        Me.lblcontractno.AutoSize = True
        Me.lblcontractno.Location = New System.Drawing.Point(775, 9)
        Me.lblcontractno.Name = "lblcontractno"
        Me.lblcontractno.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblcontractno.Size = New System.Drawing.Size(12, 17)
        Me.lblcontractno.TabIndex = 85
        Me.lblcontractno.Text = "."
        '
        'lblbatchno
        '
        Me.lblbatchno.AutoSize = True
        Me.lblbatchno.Location = New System.Drawing.Point(541, 6)
        Me.lblbatchno.Name = "lblbatchno"
        Me.lblbatchno.Size = New System.Drawing.Size(12, 17)
        Me.lblbatchno.TabIndex = 84
        Me.lblbatchno.Text = "."
        '
        'btninsert
        '
        Me.btninsert.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btninsert.Location = New System.Drawing.Point(977, 363)
        Me.btninsert.Name = "btninsert"
        Me.btninsert.Size = New System.Drawing.Size(155, 44)
        Me.btninsert.TabIndex = 83
        Me.btninsert.Text = "تسجيل"
        Me.btninsert.UseVisualStyleBackColor = True
        '
        'dataGridViewDefects
        '
        Me.dataGridViewDefects.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dataGridViewDefects.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dataGridViewDefects.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dataGridViewDefects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dataGridViewDefects.DefaultCellStyle = DataGridViewCellStyle1
        Me.dataGridViewDefects.GridColor = System.Drawing.SystemColors.ActiveCaption
        Me.dataGridViewDefects.Location = New System.Drawing.Point(47, 416)
        Me.dataGridViewDefects.Name = "dataGridViewDefects"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ActiveCaption
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dataGridViewDefects.RowHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.dataGridViewDefects.RowHeadersVisible = False
        Me.dataGridViewDefects.RowHeadersWidth = 51
        Me.dataGridViewDefects.RowTemplate.Height = 26
        Me.dataGridViewDefects.Size = New System.Drawing.Size(1095, 451)
        Me.dataGridViewDefects.TabIndex = 82
        '
        'lblUsername
        '
        Me.lblUsername.AutoSize = True
        Me.lblUsername.Location = New System.Drawing.Point(51, 11)
        Me.lblUsername.Name = "lblUsername"
        Me.lblUsername.Size = New System.Drawing.Size(34, 17)
        Me.lblUsername.TabIndex = 81
        Me.lblUsername.Text = "user"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(946, 222)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(82, 21)
        Me.Label6.TabIndex = 78
        Me.Label6.Text = "ملاحظات"
        '
        'txtnotes
        '
        Me.txtnotes.Location = New System.Drawing.Point(847, 256)
        Me.txtnotes.Multiline = True
        Me.txtnotes.Name = "txtnotes"
        Me.txtnotes.Size = New System.Drawing.Size(285, 70)
        Me.txtnotes.TabIndex = 77
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(601, 108)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(87, 21)
        Me.Label5.TabIndex = 76
        Me.Label5.Text = "أمر شغل "
        '
        'cmbworder
        '
        Me.cmbworder.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbworder.FormattingEnabled = True
        Me.cmbworder.Location = New System.Drawing.Point(544, 148)
        Me.cmbworder.Name = "cmbworder"
        Me.cmbworder.Size = New System.Drawing.Size(214, 29)
        Me.cmbworder.TabIndex = 75
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(216, 283)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(52, 21)
        Me.Label2.TabIndex = 72
        Me.Label2.Text = "الوزن"
        '
        'txtWeight
        '
        Me.txtWeight.Location = New System.Drawing.Point(62, 279)
        Me.txtWeight.Name = "txtWeight"
        Me.txtWeight.Size = New System.Drawing.Size(85, 24)
        Me.txtWeight.TabIndex = 69
        '
        'fetchfinishlinksform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1113, 630)
        Me.Controls.Add(Me.btnFetchWeight)
        Me.Controls.Add(Me.btnmenu)
        Me.Controls.Add(Me.lblcolor)
        Me.Controls.Add(Me.lblclient)
        Me.Controls.Add(Me.lbltotalw)
        Me.Controls.Add(Me.lbltotalm)
        Me.Controls.Add(Me.lblqtym)
        Me.Controls.Add(Me.lblqtykg)
        Me.Controls.Add(Me.lblcontractno)
        Me.Controls.Add(Me.lblbatchno)
        Me.Controls.Add(Me.btninsert)
        Me.Controls.Add(Me.dataGridViewDefects)
        Me.Controls.Add(Me.lblUsername)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtnotes)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.cmbworder)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtWeight)
        Me.Name = "fetchfinishlinksform"
        Me.Text = "fetchfinishlinksform"
        CType(Me.dataGridViewDefects, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnFetchWeight As Button
    Friend WithEvents btnmenu As Button
    Friend WithEvents lblcolor As Label
    Friend WithEvents lblclient As Label
    Friend WithEvents lbltotalw As Label
    Friend WithEvents lbltotalm As Label
    Friend WithEvents lblqtym As Label
    Friend WithEvents lblqtykg As Label
    Friend WithEvents lblcontractno As Label
    Friend WithEvents lblbatchno As Label
    Friend WithEvents btninsert As Button
    Friend WithEvents dataGridViewDefects As DataGridView
    Friend WithEvents lblUsername As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents txtnotes As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents cmbworder As ComboBox
    Friend WithEvents Label2 As Label
    Friend WithEvents txtWeight As TextBox
End Class
