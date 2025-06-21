<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class createbatchform
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
        Me.lblbatch = New System.Windows.Forms.Label()
        Me.cmbsupplier = New System.Windows.Forms.ComboBox()
        Me.cmbclient = New System.Windows.Forms.ComboBox()
        Me.cmbkindfabric = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblclient = New System.Windows.Forms.Label()
        Me.lblsup = New System.Windows.Forms.Label()
        Me.dgvdetailsbatch = New System.Windows.Forms.DataGridView()
        Me.btninsert = New System.Windows.Forms.Button()
        Me.btninsert2 = New System.Windows.Forms.Button()
        Me.lblstyle = New System.Windows.Forms.Label()
        Me.cmbstyle = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbservice = New System.Windows.Forms.ComboBox()
        Me.txtpo = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtmaterial = New System.Windows.Forms.TextBox()
        Me.lblUsername = New System.Windows.Forms.Label()
        CType(Me.dgvdetailsbatch, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblbatch
        '
        Me.lblbatch.AutoSize = True
        Me.lblbatch.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblbatch.Location = New System.Drawing.Point(418, 25)
        Me.lblbatch.Name = "lblbatch"
        Me.lblbatch.Size = New System.Drawing.Size(15, 21)
        Me.lblbatch.TabIndex = 0
        Me.lblbatch.Text = "."
        '
        'cmbsupplier
        '
        Me.cmbsupplier.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbsupplier.FormattingEnabled = True
        Me.cmbsupplier.Location = New System.Drawing.Point(998, 220)
        Me.cmbsupplier.Name = "cmbsupplier"
        Me.cmbsupplier.Size = New System.Drawing.Size(225, 32)
        Me.cmbsupplier.TabIndex = 1
        '
        'cmbclient
        '
        Me.cmbclient.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbclient.FormattingEnabled = True
        Me.cmbclient.Location = New System.Drawing.Point(84, 220)
        Me.cmbclient.Name = "cmbclient"
        Me.cmbclient.Size = New System.Drawing.Size(164, 32)
        Me.cmbclient.TabIndex = 2
        '
        'cmbkindfabric
        '
        Me.cmbkindfabric.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbkindfabric.FormattingEnabled = True
        Me.cmbkindfabric.Location = New System.Drawing.Point(998, 58)
        Me.cmbkindfabric.Name = "cmbkindfabric"
        Me.cmbkindfabric.Size = New System.Drawing.Size(164, 32)
        Me.cmbkindfabric.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(514, 25)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(107, 21)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "رقم الرسالة"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(1023, 16)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(105, 21)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "نوع القماش"
        '
        'lblclient
        '
        Me.lblclient.AutoSize = True
        Me.lblclient.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblclient.Location = New System.Drawing.Point(84, 188)
        Me.lblclient.Name = "lblclient"
        Me.lblclient.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblclient.Size = New System.Drawing.Size(54, 21)
        Me.lblclient.TabIndex = 6
        Me.lblclient.Text = "عميل"
        '
        'lblsup
        '
        Me.lblsup.AutoSize = True
        Me.lblsup.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblsup.Location = New System.Drawing.Point(1059, 178)
        Me.lblsup.Name = "lblsup"
        Me.lblsup.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblsup.Size = New System.Drawing.Size(47, 21)
        Me.lblsup.TabIndex = 7
        Me.lblsup.Text = "مورد"
        '
        'dgvdetailsbatch
        '
        Me.dgvdetailsbatch.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dgvdetailsbatch.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvdetailsbatch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvdetailsbatch.Location = New System.Drawing.Point(26, 330)
        Me.dgvdetailsbatch.Name = "dgvdetailsbatch"
        Me.dgvdetailsbatch.RowHeadersWidth = 51
        Me.dgvdetailsbatch.RowTemplate.Height = 26
        Me.dgvdetailsbatch.Size = New System.Drawing.Size(1269, 412)
        Me.dgvdetailsbatch.TabIndex = 8
        Me.dgvdetailsbatch.Visible = False
        '
        'btninsert
        '
        Me.btninsert.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btninsert.Location = New System.Drawing.Point(612, 287)
        Me.btninsert.Name = "btninsert"
        Me.btninsert.Size = New System.Drawing.Size(135, 37)
        Me.btninsert.TabIndex = 9
        Me.btninsert.Text = "تسجيل"
        Me.btninsert.UseVisualStyleBackColor = True
        '
        'btninsert2
        '
        Me.btninsert2.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btninsert2.Location = New System.Drawing.Point(50, 287)
        Me.btninsert2.Name = "btninsert2"
        Me.btninsert2.Size = New System.Drawing.Size(198, 37)
        Me.btninsert2.TabIndex = 10
        Me.btninsert2.Text = "تسجيل بيانات الرسالة"
        Me.btninsert2.UseVisualStyleBackColor = True
        Me.btninsert2.Visible = False
        '
        'lblstyle
        '
        Me.lblstyle.AutoSize = True
        Me.lblstyle.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblstyle.Location = New System.Drawing.Point(422, 121)
        Me.lblstyle.Name = "lblstyle"
        Me.lblstyle.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblstyle.Size = New System.Drawing.Size(61, 21)
        Me.lblstyle.TabIndex = 12
        Me.lblstyle.Text = "الخامة"
        '
        'cmbstyle
        '
        Me.cmbstyle.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbstyle.FormattingEnabled = True
        Me.cmbstyle.Location = New System.Drawing.Point(416, 145)
        Me.cmbstyle.Name = "cmbstyle"
        Me.cmbstyle.Size = New System.Drawing.Size(170, 32)
        Me.cmbstyle.TabIndex = 11
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(104, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(98, 21)
        Me.Label1.TabIndex = 14
        Me.Label1.Text = "نوع الخدمة"
        '
        'cmbservice
        '
        Me.cmbservice.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbservice.FormattingEnabled = True
        Me.cmbservice.Location = New System.Drawing.Point(26, 58)
        Me.cmbservice.Name = "cmbservice"
        Me.cmbservice.Size = New System.Drawing.Size(256, 32)
        Me.cmbservice.TabIndex = 13
        '
        'txtpo
        '
        Me.txtpo.Location = New System.Drawing.Point(634, 220)
        Me.txtpo.Name = "txtpo"
        Me.txtpo.Size = New System.Drawing.Size(164, 24)
        Me.txtpo.TabIndex = 15
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(684, 184)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(34, 21)
        Me.Label4.TabIndex = 16
        Me.Label4.Text = "PO"
        '
        'txtmaterial
        '
        Me.txtmaterial.Location = New System.Drawing.Point(26, 121)
        Me.txtmaterial.Multiline = True
        Me.txtmaterial.Name = "txtmaterial"
        Me.txtmaterial.Size = New System.Drawing.Size(319, 56)
        Me.txtmaterial.TabIndex = 17
        '
        'lblUsername
        '
        Me.lblUsername.AutoSize = True
        Me.lblUsername.Location = New System.Drawing.Point(796, 9)
        Me.lblUsername.Name = "lblUsername"
        Me.lblUsername.Size = New System.Drawing.Size(34, 17)
        Me.lblUsername.TabIndex = 18
        Me.lblUsername.Text = "user"
        '
        'createbatchform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1329, 767)
        Me.Controls.Add(Me.lblUsername)
        Me.Controls.Add(Me.txtmaterial)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtpo)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmbservice)
        Me.Controls.Add(Me.lblstyle)
        Me.Controls.Add(Me.cmbstyle)
        Me.Controls.Add(Me.btninsert2)
        Me.Controls.Add(Me.btninsert)
        Me.Controls.Add(Me.dgvdetailsbatch)
        Me.Controls.Add(Me.lblsup)
        Me.Controls.Add(Me.lblclient)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.cmbkindfabric)
        Me.Controls.Add(Me.cmbclient)
        Me.Controls.Add(Me.cmbsupplier)
        Me.Controls.Add(Me.lblbatch)
        Me.Name = "createbatchform"
        Me.Text = "createbatchform"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.dgvdetailsbatch, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblbatch As Label
    Friend WithEvents cmbsupplier As ComboBox
    Friend WithEvents cmbclient As ComboBox
    Friend WithEvents cmbkindfabric As ComboBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents lblclient As Label
    Friend WithEvents lblsup As Label
    Friend WithEvents dgvdetailsbatch As DataGridView
    Friend WithEvents btninsert As Button
    Friend WithEvents btninsert2 As Button
    Friend WithEvents lblstyle As Label
    Friend WithEvents cmbstyle As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents cmbservice As ComboBox
    Friend WithEvents txtpo As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents txtmaterial As TextBox
    Friend WithEvents lblUsername As Label
End Class
