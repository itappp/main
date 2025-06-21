<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class techdataform
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
        Me.cmbbatch = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbcontractno = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtbatch = New System.Windows.Forms.TextBox()
        Me.lblcontractid = New System.Windows.Forms.Label()
        Me.dataGridbatch1 = New System.Windows.Forms.DataGridView()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtworder = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtqtym = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtqtykg = New System.Windows.Forms.TextBox()
        Me.dtpDeliveryDate = New System.Windows.Forms.DateTimePicker()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.btnInsert = New System.Windows.Forms.Button()
        Me.lblusername = New System.Windows.Forms.Label()
        Me.lblgetlastworder = New System.Windows.Forms.Label()
        Me.txtstagefix = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtstagereason = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtfrom = New System.Windows.Forms.TextBox()
        Me.txtdefect = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.clbWorderIds = New System.Windows.Forms.CheckedListBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtrefno = New System.Windows.Forms.TextBox()
        CType(Me.dataGridbatch1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmbbatch
        '
        Me.cmbbatch.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbbatch.FormattingEnabled = True
        Me.cmbbatch.Location = New System.Drawing.Point(684, 24)
        Me.cmbbatch.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.cmbbatch.Name = "cmbbatch"
        Me.cmbbatch.Size = New System.Drawing.Size(208, 29)
        Me.cmbbatch.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(606, 27)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(59, 21)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Batch"
        '
        'cmbcontractno
        '
        Me.cmbcontractno.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbcontractno.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Me.cmbcontractno.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbcontractno.FormattingEnabled = True
        Me.cmbcontractno.Location = New System.Drawing.Point(302, 24)
        Me.cmbcontractno.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.cmbcontractno.Name = "cmbcontractno"
        Me.cmbcontractno.Size = New System.Drawing.Size(190, 29)
        Me.cmbcontractno.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(216, 27)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(83, 21)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Contract"
        '
        'txtbatch
        '
        Me.txtbatch.Location = New System.Drawing.Point(1226, 20)
        Me.txtbatch.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.txtbatch.Name = "txtbatch"
        Me.txtbatch.Size = New System.Drawing.Size(114, 23)
        Me.txtbatch.TabIndex = 4
        Me.txtbatch.Visible = False
        '
        'lblcontractid
        '
        Me.lblcontractid.AutoSize = True
        Me.lblcontractid.Location = New System.Drawing.Point(1426, 24)
        Me.lblcontractid.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblcontractid.Name = "lblcontractid"
        Me.lblcontractid.Size = New System.Drawing.Size(49, 16)
        Me.lblcontractid.TabIndex = 5
        Me.lblcontractid.Text = "Label3"
        Me.lblcontractid.Visible = False
        '
        'dataGridbatch1
        '
        Me.dataGridbatch1.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dataGridbatch1.BorderStyle = System.Windows.Forms.BorderStyle.None
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ActiveCaption
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Tahoma", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dataGridbatch1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dataGridbatch1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dataGridbatch1.EnableHeadersVisualStyles = False
        Me.dataGridbatch1.GridColor = System.Drawing.SystemColors.ActiveCaption
        Me.dataGridbatch1.Location = New System.Drawing.Point(116, 87)
        Me.dataGridbatch1.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.dataGridbatch1.Name = "dataGridbatch1"
        Me.dataGridbatch1.RowHeadersWidth = 51
        Me.dataGridbatch1.RowTemplate.Height = 26
        Me.dataGridbatch1.Size = New System.Drawing.Size(1116, 88)
        Me.dataGridbatch1.TabIndex = 7
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(680, 63)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(55, 21)
        Me.Label5.TabIndex = 11
        Me.Label5.Text = "Sales"
        '
        'txtworder
        '
        Me.txtworder.Location = New System.Drawing.Point(116, 372)
        Me.txtworder.Multiline = True
        Me.txtworder.Name = "txtworder"
        Me.txtworder.Size = New System.Drawing.Size(154, 38)
        Me.txtworder.TabIndex = 13
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(36, 373)
        Me.Label6.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(73, 21)
        Me.Label6.TabIndex = 14
        Me.Label6.Text = "Worder"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(467, 375)
        Me.Label7.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(76, 21)
        Me.Label7.TabIndex = 16
        Me.Label7.Text = "Qty (M)"
        '
        'txtqtym
        '
        Me.txtqtym.Location = New System.Drawing.Point(547, 374)
        Me.txtqtym.Multiline = True
        Me.txtqtym.Name = "txtqtym"
        Me.txtqtym.Size = New System.Drawing.Size(95, 37)
        Me.txtqtym.TabIndex = 15
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(707, 374)
        Me.Label8.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(86, 21)
        Me.Label8.TabIndex = 18
        Me.Label8.Text = "Qty (KG)"
        '
        'txtqtykg
        '
        Me.txtqtykg.Location = New System.Drawing.Point(813, 372)
        Me.txtqtykg.Multiline = True
        Me.txtqtykg.Name = "txtqtykg"
        Me.txtqtykg.Size = New System.Drawing.Size(95, 37)
        Me.txtqtykg.TabIndex = 17
        '
        'dtpDeliveryDate
        '
        Me.dtpDeliveryDate.Location = New System.Drawing.Point(659, 267)
        Me.dtpDeliveryDate.Name = "dtpDeliveryDate"
        Me.dtpDeliveryDate.Size = New System.Drawing.Size(178, 23)
        Me.dtpDeliveryDate.TabIndex = 19
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(501, 268)
        Me.Label9.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(123, 21)
        Me.Label9.TabIndex = 21
        Me.Label9.Text = "Delivery date"
        '
        'btnInsert
        '
        Me.btnInsert.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnInsert.Location = New System.Drawing.Point(610, 701)
        Me.btnInsert.Name = "btnInsert"
        Me.btnInsert.Size = New System.Drawing.Size(183, 60)
        Me.btnInsert.TabIndex = 24
        Me.btnInsert.Text = "تسجيل البيانات"
        Me.btnInsert.UseVisualStyleBackColor = True
        '
        'lblusername
        '
        Me.lblusername.AutoSize = True
        Me.lblusername.Location = New System.Drawing.Point(1166, 67)
        Me.lblusername.Name = "lblusername"
        Me.lblusername.Size = New System.Drawing.Size(36, 16)
        Me.lblusername.TabIndex = 26
        Me.lblusername.Text = "user"
        '
        'lblgetlastworder
        '
        Me.lblgetlastworder.AutoSize = True
        Me.lblgetlastworder.Location = New System.Drawing.Point(37, 473)
        Me.lblgetlastworder.Name = "lblgetlastworder"
        Me.lblgetlastworder.Size = New System.Drawing.Size(88, 16)
        Me.lblgetlastworder.TabIndex = 27
        Me.lblgetlastworder.Text = "Last Worder"
        '
        'txtstagefix
        '
        Me.txtstagefix.Location = New System.Drawing.Point(230, 561)
        Me.txtstagefix.Multiline = True
        Me.txtstagefix.Name = "txtstagefix"
        Me.txtstagefix.Size = New System.Drawing.Size(174, 63)
        Me.txtstagefix.TabIndex = 30
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(476, 528)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(87, 16)
        Me.Label10.TabIndex = 31
        Me.Label10.Text = "بسبب مرحله"
        '
        'txtstagereason
        '
        Me.txtstagereason.Location = New System.Drawing.Point(438, 561)
        Me.txtstagereason.Multiline = True
        Me.txtstagereason.Name = "txtstagereason"
        Me.txtstagereason.Size = New System.Drawing.Size(180, 63)
        Me.txtstagereason.TabIndex = 32
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(269, 528)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(87, 16)
        Me.Label11.TabIndex = 33
        Me.Label11.Text = "مرحله تصليح"
        '
        'txtfrom
        '
        Me.txtfrom.Location = New System.Drawing.Point(647, 561)
        Me.txtfrom.Multiline = True
        Me.txtfrom.Name = "txtfrom"
        Me.txtfrom.Size = New System.Drawing.Size(176, 63)
        Me.txtfrom.TabIndex = 34
        '
        'txtdefect
        '
        Me.txtdefect.Location = New System.Drawing.Point(849, 561)
        Me.txtdefect.Multiline = True
        Me.txtdefect.Name = "txtdefect"
        Me.txtdefect.Size = New System.Drawing.Size(169, 63)
        Me.txtdefect.TabIndex = 35
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(687, 528)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(71, 16)
        Me.Label12.TabIndex = 36
        Me.Label12.Text = "الرجوع من"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(899, 528)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(41, 16)
        Me.Label13.TabIndex = 37
        Me.Label13.Text = "العيب"
        '
        'clbWorderIds
        '
        Me.clbWorderIds.BackColor = System.Drawing.SystemColors.Control
        Me.clbWorderIds.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.clbWorderIds.FormattingEnabled = True
        Me.clbWorderIds.Location = New System.Drawing.Point(12, 219)
        Me.clbWorderIds.Name = "clbWorderIds"
        Me.clbWorderIds.Size = New System.Drawing.Size(182, 126)
        Me.clbWorderIds.TabIndex = 38
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(38, 541)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(78, 16)
        Me.Label3.TabIndex = 40
        Me.Label3.Text = "رقم الاخطار"
        '
        'txtrefno
        '
        Me.txtrefno.Location = New System.Drawing.Point(12, 572)
        Me.txtrefno.Name = "txtrefno"
        Me.txtrefno.Size = New System.Drawing.Size(174, 23)
        Me.txtrefno.TabIndex = 39
        '
        'techdataform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1372, 879)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtrefno)
        Me.Controls.Add(Me.clbWorderIds)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.txtdefect)
        Me.Controls.Add(Me.txtfrom)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.txtstagereason)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.txtstagefix)
        Me.Controls.Add(Me.lblgetlastworder)
        Me.Controls.Add(Me.lblusername)
        Me.Controls.Add(Me.btnInsert)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.dtpDeliveryDate)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.txtqtykg)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtqtym)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtworder)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.dataGridbatch1)
        Me.Controls.Add(Me.lblcontractid)
        Me.Controls.Add(Me.txtbatch)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.cmbcontractno)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmbbatch)
        Me.Font = New System.Drawing.Font("Tahoma", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Name = "techdataform"
        Me.Text = "techdataform"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.dataGridbatch1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmbbatch As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbcontractno As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtbatch As System.Windows.Forms.TextBox
    Friend WithEvents lblcontractid As System.Windows.Forms.Label
    Friend WithEvents dataGridbatch1 As System.Windows.Forms.DataGridView
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtworder As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtqtym As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtqtykg As System.Windows.Forms.TextBox
    Friend WithEvents dtpDeliveryDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents btnInsert As System.Windows.Forms.Button
    Friend WithEvents lblusername As System.Windows.Forms.Label
    Friend WithEvents lblgetlastworder As System.Windows.Forms.Label
    Friend WithEvents txtstagefix As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtstagereason As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtfrom As System.Windows.Forms.TextBox
    Friend WithEvents txtdefect As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents clbWorderIds As System.Windows.Forms.CheckedListBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtrefno As TextBox
End Class
