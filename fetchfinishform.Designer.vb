<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fetchfinishform
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
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtdegree = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtnotes = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cmbworder = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtSpeed = New System.Windows.Forms.TextBox()
        Me.txtWidth = New System.Windows.Forms.TextBox()
        Me.txtWeight = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtHeight = New System.Windows.Forms.TextBox()
        Me.btnFetchData = New System.Windows.Forms.Button()
        Me.btnmenu = New System.Windows.Forms.Button()
        Me.checkboxdegree = New System.Windows.Forms.CheckBox()
        Me.btnFetchWeight = New System.Windows.Forms.Button()
        Me.lblmaterial = New System.Windows.Forms.Label()
        CType(Me.dataGridViewDefects, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblcolor
        '
        Me.lblcolor.AutoSize = True
        Me.lblcolor.Location = New System.Drawing.Point(309, 21)
        Me.lblcolor.Name = "lblcolor"
        Me.lblcolor.Size = New System.Drawing.Size(12, 17)
        Me.lblcolor.TabIndex = 62
        Me.lblcolor.Text = "."
        '
        'lblclient
        '
        Me.lblclient.AutoSize = True
        Me.lblclient.Location = New System.Drawing.Point(312, 67)
        Me.lblclient.Name = "lblclient"
        Me.lblclient.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblclient.Size = New System.Drawing.Size(12, 17)
        Me.lblclient.TabIndex = 60
        Me.lblclient.Text = "."
        '
        'lbltotalw
        '
        Me.lbltotalw.AutoSize = True
        Me.lbltotalw.Location = New System.Drawing.Point(528, 211)
        Me.lbltotalw.Name = "lbltotalw"
        Me.lbltotalw.Size = New System.Drawing.Size(12, 17)
        Me.lbltotalw.TabIndex = 55
        Me.lbltotalw.Text = "."
        '
        'lbltotalm
        '
        Me.lbltotalm.AutoSize = True
        Me.lbltotalm.Location = New System.Drawing.Point(726, 211)
        Me.lbltotalm.Name = "lbltotalm"
        Me.lbltotalm.Size = New System.Drawing.Size(12, 17)
        Me.lbltotalm.TabIndex = 54
        Me.lbltotalm.Text = "."
        '
        'lblqtym
        '
        Me.lblqtym.AutoSize = True
        Me.lblqtym.Location = New System.Drawing.Point(770, 67)
        Me.lblqtym.Name = "lblqtym"
        Me.lblqtym.Size = New System.Drawing.Size(12, 17)
        Me.lblqtym.TabIndex = 53
        Me.lblqtym.Text = "."
        '
        'lblqtykg
        '
        Me.lblqtykg.AutoSize = True
        Me.lblqtykg.Location = New System.Drawing.Point(539, 67)
        Me.lblqtykg.Name = "lblqtykg"
        Me.lblqtykg.Size = New System.Drawing.Size(12, 17)
        Me.lblqtykg.TabIndex = 52
        Me.lblqtykg.Text = "."
        '
        'lblcontractno
        '
        Me.lblcontractno.AutoSize = True
        Me.lblcontractno.Location = New System.Drawing.Point(773, 21)
        Me.lblcontractno.Name = "lblcontractno"
        Me.lblcontractno.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblcontractno.Size = New System.Drawing.Size(12, 17)
        Me.lblcontractno.TabIndex = 51
        Me.lblcontractno.Text = "."
        '
        'lblbatchno
        '
        Me.lblbatchno.AutoSize = True
        Me.lblbatchno.Location = New System.Drawing.Point(539, 18)
        Me.lblbatchno.Name = "lblbatchno"
        Me.lblbatchno.Size = New System.Drawing.Size(12, 17)
        Me.lblbatchno.TabIndex = 50
        Me.lblbatchno.Text = "."
        '
        'btninsert
        '
        Me.btninsert.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btninsert.Location = New System.Drawing.Point(975, 375)
        Me.btninsert.Name = "btninsert"
        Me.btninsert.Size = New System.Drawing.Size(155, 44)
        Me.btninsert.TabIndex = 49
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
        Me.dataGridViewDefects.Location = New System.Drawing.Point(45, 428)
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
        Me.dataGridViewDefects.TabIndex = 48
        '
        'lblUsername
        '
        Me.lblUsername.AutoSize = True
        Me.lblUsername.Location = New System.Drawing.Point(49, 23)
        Me.lblUsername.Name = "lblUsername"
        Me.lblUsername.Size = New System.Drawing.Size(34, 17)
        Me.lblUsername.TabIndex = 47
        Me.lblUsername.Text = "user"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(354, 171)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(100, 21)
        Me.Label7.TabIndex = 46
        Me.Label7.Text = "ترحيل تانيه"
        '
        'txtdegree
        '
        Me.txtdegree.Location = New System.Drawing.Point(339, 204)
        Me.txtdegree.Name = "txtdegree"
        Me.txtdegree.Size = New System.Drawing.Size(114, 24)
        Me.txtdegree.TabIndex = 45
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(944, 234)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(82, 21)
        Me.Label6.TabIndex = 44
        Me.Label6.Text = "ملاحظات"
        '
        'txtnotes
        '
        Me.txtnotes.Location = New System.Drawing.Point(845, 268)
        Me.txtnotes.Multiline = True
        Me.txtnotes.Name = "txtnotes"
        Me.txtnotes.Size = New System.Drawing.Size(285, 70)
        Me.txtnotes.TabIndex = 43
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(599, 120)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(87, 21)
        Me.Label5.TabIndex = 42
        Me.Label5.Text = "أمر شغل "
        '
        'cmbworder
        '
        Me.cmbworder.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbworder.FormattingEnabled = True
        Me.cmbworder.Location = New System.Drawing.Point(542, 160)
        Me.cmbworder.Name = "cmbworder"
        Me.cmbworder.Size = New System.Drawing.Size(214, 29)
        Me.cmbworder.TabIndex = 41
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(197, 327)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(72, 21)
        Me.Label4.TabIndex = 40
        Me.Label4.Text = "السرعه"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(205, 279)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(64, 21)
        Me.Label3.TabIndex = 39
        Me.Label3.Text = "العرض"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(217, 388)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(52, 21)
        Me.Label2.TabIndex = 38
        Me.Label2.Text = "الوزن"
        '
        'txtSpeed
        '
        Me.txtSpeed.Location = New System.Drawing.Point(63, 324)
        Me.txtSpeed.Name = "txtSpeed"
        Me.txtSpeed.Size = New System.Drawing.Size(86, 24)
        Me.txtSpeed.TabIndex = 37
        '
        'txtWidth
        '
        Me.txtWidth.Location = New System.Drawing.Point(63, 276)
        Me.txtWidth.Name = "txtWidth"
        Me.txtWidth.Size = New System.Drawing.Size(86, 24)
        Me.txtWidth.TabIndex = 36
        '
        'txtWeight
        '
        Me.txtWeight.Location = New System.Drawing.Point(63, 384)
        Me.txtWeight.Name = "txtWeight"
        Me.txtWeight.Size = New System.Drawing.Size(85, 24)
        Me.txtWeight.TabIndex = 35
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(217, 236)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(59, 21)
        Me.Label1.TabIndex = 34
        Me.Label1.Text = "الطول"
        '
        'txtHeight
        '
        Me.txtHeight.Location = New System.Drawing.Point(63, 233)
        Me.txtHeight.Name = "txtHeight"
        Me.txtHeight.Size = New System.Drawing.Size(85, 24)
        Me.txtHeight.TabIndex = 33
        '
        'btnFetchData
        '
        Me.btnFetchData.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFetchData.Location = New System.Drawing.Point(312, 268)
        Me.btnFetchData.Name = "btnFetchData"
        Me.btnFetchData.Size = New System.Drawing.Size(165, 63)
        Me.btnFetchData.TabIndex = 32
        Me.btnFetchData.Text = "بيانات المتر"
        Me.btnFetchData.UseVisualStyleBackColor = True
        '
        'btnmenu
        '
        Me.btnmenu.Location = New System.Drawing.Point(12, 110)
        Me.btnmenu.Name = "btnmenu"
        Me.btnmenu.Size = New System.Drawing.Size(74, 44)
        Me.btnmenu.TabIndex = 63
        Me.btnmenu.Text = "رجوع"
        Me.btnmenu.UseVisualStyleBackColor = True
        Me.btnmenu.Visible = False
        '
        'checkboxdegree
        '
        Me.checkboxdegree.AutoSize = True
        Me.checkboxdegree.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.checkboxdegree.Location = New System.Drawing.Point(581, 327)
        Me.checkboxdegree.Name = "checkboxdegree"
        Me.checkboxdegree.Size = New System.Drawing.Size(151, 28)
        Me.checkboxdegree.TabIndex = 64
        Me.checkboxdegree.Text = "توب درجه تانيه"
        Me.checkboxdegree.UseVisualStyleBackColor = True
        '
        'btnFetchWeight
        '
        Me.btnFetchWeight.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFetchWeight.Location = New System.Drawing.Point(315, 373)
        Me.btnFetchWeight.Name = "btnFetchWeight"
        Me.btnFetchWeight.Size = New System.Drawing.Size(162, 44)
        Me.btnFetchWeight.TabIndex = 65
        Me.btnFetchWeight.Text = "بيانات الوزن"
        Me.btnFetchWeight.UseVisualStyleBackColor = True
        '
        'lblmaterial
        '
        Me.lblmaterial.AutoSize = True
        Me.lblmaterial.Location = New System.Drawing.Point(312, 120)
        Me.lblmaterial.Name = "lblmaterial"
        Me.lblmaterial.Size = New System.Drawing.Size(12, 17)
        Me.lblmaterial.TabIndex = 66
        Me.lblmaterial.Text = "."
        '
        'fetchfinishform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1401, 891)
        Me.Controls.Add(Me.lblmaterial)
        Me.Controls.Add(Me.btnFetchWeight)
        Me.Controls.Add(Me.checkboxdegree)
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
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtdegree)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtnotes)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.cmbworder)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtSpeed)
        Me.Controls.Add(Me.txtWidth)
        Me.Controls.Add(Me.txtWeight)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtHeight)
        Me.Controls.Add(Me.btnFetchData)
        Me.Name = "fetchfinishform"
        Me.Text = "fetchfinishform"
        CType(Me.dataGridViewDefects, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

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
    Friend WithEvents Label7 As Label
    Friend WithEvents txtdegree As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents txtnotes As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents cmbworder As ComboBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents txtSpeed As TextBox
    Friend WithEvents txtWidth As TextBox
    Friend WithEvents txtWeight As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents txtHeight As TextBox
    Friend WithEvents btnFetchData As Button
    Friend WithEvents btnmenu As Button
    Friend WithEvents checkboxdegree As CheckBox
    Friend WithEvents btnFetchWeight As Button
    Friend WithEvents lblmaterial As Label
End Class
