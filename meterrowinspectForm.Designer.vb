<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class meterrowinspectForm
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
        Me.lblstatus = New System.Windows.Forms.Label()
        Me.btninsert = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtm = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblqtykg = New System.Windows.Forms.Label()
        Me.lblqtym = New System.Windows.Forms.Label()
        Me.lblusername = New System.Windows.Forms.Label()
        Me.lblcontractno = New System.Windows.Forms.Label()
        Me.lblbatchno = New System.Windows.Forms.Label()
        Me.cmbworder = New System.Windows.Forms.ComboBox()
        Me.lblweightstore = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'lblstatus
        '
        Me.lblstatus.AutoSize = True
        Me.lblstatus.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblstatus.Location = New System.Drawing.Point(128, 58)
        Me.lblstatus.Name = "lblstatus"
        Me.lblstatus.Size = New System.Drawing.Size(57, 21)
        Me.lblstatus.TabIndex = 23
        Me.lblstatus.Text = "الحالة"
        '
        'btninsert
        '
        Me.btninsert.Font = New System.Drawing.Font("Tahoma", 13.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btninsert.Location = New System.Drawing.Point(552, 387)
        Me.btninsert.Name = "btninsert"
        Me.btninsert.Size = New System.Drawing.Size(140, 57)
        Me.btninsert.TabIndex = 22
        Me.btninsert.Text = "تسجيل"
        Me.btninsert.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(587, 262)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(40, 24)
        Me.Label4.TabIndex = 21
        Me.Label4.Text = "متر"
        '
        'txtm
        '
        Me.txtm.Location = New System.Drawing.Point(519, 304)
        Me.txtm.Name = "txtm"
        Me.txtm.Size = New System.Drawing.Size(198, 24)
        Me.txtm.TabIndex = 20
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(361, 55)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(82, 21)
        Me.Label2.TabIndex = 19
        Me.Label2.Text = "أمر شغل"
        '
        'lblqtykg
        '
        Me.lblqtykg.AutoSize = True
        Me.lblqtykg.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblqtykg.Location = New System.Drawing.Point(515, 165)
        Me.lblqtykg.Name = "lblqtykg"
        Me.lblqtykg.Size = New System.Drawing.Size(58, 21)
        Me.lblqtykg.TabIndex = 18
        Me.lblqtykg.Text = "Label3"
        '
        'lblqtym
        '
        Me.lblqtym.AutoSize = True
        Me.lblqtym.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblqtym.Location = New System.Drawing.Point(257, 165)
        Me.lblqtym.Name = "lblqtym"
        Me.lblqtym.Size = New System.Drawing.Size(58, 21)
        Me.lblqtym.TabIndex = 17
        Me.lblqtym.Text = "Label2"
        '
        'lblusername
        '
        Me.lblusername.AutoSize = True
        Me.lblusername.Location = New System.Drawing.Point(39, 20)
        Me.lblusername.Name = "lblusername"
        Me.lblusername.Size = New System.Drawing.Size(34, 17)
        Me.lblusername.TabIndex = 16
        Me.lblusername.Text = "user"
        '
        'lblcontractno
        '
        Me.lblcontractno.AutoSize = True
        Me.lblcontractno.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblcontractno.Location = New System.Drawing.Point(997, 165)
        Me.lblcontractno.Name = "lblcontractno"
        Me.lblcontractno.Size = New System.Drawing.Size(58, 21)
        Me.lblcontractno.TabIndex = 15
        Me.lblcontractno.Text = "Label1"
        '
        'lblbatchno
        '
        Me.lblbatchno.AutoSize = True
        Me.lblbatchno.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblbatchno.Location = New System.Drawing.Point(778, 165)
        Me.lblbatchno.Name = "lblbatchno"
        Me.lblbatchno.Size = New System.Drawing.Size(58, 21)
        Me.lblbatchno.TabIndex = 14
        Me.lblbatchno.Text = "Label1"
        '
        'cmbworder
        '
        Me.cmbworder.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbworder.FormattingEnabled = True
        Me.cmbworder.Location = New System.Drawing.Point(485, 55)
        Me.cmbworder.Name = "cmbworder"
        Me.cmbworder.Size = New System.Drawing.Size(245, 29)
        Me.cmbworder.TabIndex = 13
        '
        'lblweightstore
        '
        Me.lblweightstore.AutoSize = True
        Me.lblweightstore.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblweightstore.Location = New System.Drawing.Point(172, 300)
        Me.lblweightstore.Name = "lblweightstore"
        Me.lblweightstore.Size = New System.Drawing.Size(164, 24)
        Me.lblweightstore.TabIndex = 24
        Me.lblweightstore.Text = "وزن فعلى مخزن"
        '
        'meterrowinspectForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1149, 552)
        Me.Controls.Add(Me.lblweightstore)
        Me.Controls.Add(Me.lblstatus)
        Me.Controls.Add(Me.btninsert)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtm)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lblqtykg)
        Me.Controls.Add(Me.lblqtym)
        Me.Controls.Add(Me.lblusername)
        Me.Controls.Add(Me.lblcontractno)
        Me.Controls.Add(Me.lblbatchno)
        Me.Controls.Add(Me.cmbworder)
        Me.Name = "meterrowinspectForm"
        Me.Text = "meterrowinspectForm"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblstatus As System.Windows.Forms.Label
    Friend WithEvents btninsert As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtm As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblqtykg As System.Windows.Forms.Label
    Friend WithEvents lblqtym As System.Windows.Forms.Label
    Friend WithEvents lblusername As System.Windows.Forms.Label
    Friend WithEvents lblcontractno As System.Windows.Forms.Label
    Friend WithEvents lblbatchno As System.Windows.Forms.Label
    Friend WithEvents cmbworder As System.Windows.Forms.ComboBox
    Friend WithEvents lblweightstore As System.Windows.Forms.Label
End Class
