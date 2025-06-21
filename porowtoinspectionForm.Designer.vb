<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class porowtoinspectionForm
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
        Me.cmbworder = New System.Windows.Forms.ComboBox()
        Me.lblbatchno = New System.Windows.Forms.Label()
        Me.lblcontractno = New System.Windows.Forms.Label()
        Me.lblusername = New System.Windows.Forms.Label()
        Me.lblqtym = New System.Windows.Forms.Label()
        Me.lblqtykg = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtkg = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btninsert = New System.Windows.Forms.Button()
        Me.lblstatus = New System.Windows.Forms.Label()
        Me.txtrolls = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'cmbworder
        '
        Me.cmbworder.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbworder.FormattingEnabled = True
        Me.cmbworder.Location = New System.Drawing.Point(474, 44)
        Me.cmbworder.Name = "cmbworder"
        Me.cmbworder.Size = New System.Drawing.Size(245, 29)
        Me.cmbworder.TabIndex = 0
        '
        'lblbatchno
        '
        Me.lblbatchno.AutoSize = True
        Me.lblbatchno.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblbatchno.Location = New System.Drawing.Point(767, 154)
        Me.lblbatchno.Name = "lblbatchno"
        Me.lblbatchno.Size = New System.Drawing.Size(58, 21)
        Me.lblbatchno.TabIndex = 1
        Me.lblbatchno.Text = "Label1"
        '
        'lblcontractno
        '
        Me.lblcontractno.AutoSize = True
        Me.lblcontractno.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblcontractno.Location = New System.Drawing.Point(986, 154)
        Me.lblcontractno.Name = "lblcontractno"
        Me.lblcontractno.Size = New System.Drawing.Size(58, 21)
        Me.lblcontractno.TabIndex = 2
        Me.lblcontractno.Text = "Label1"
        '
        'lblusername
        '
        Me.lblusername.AutoSize = True
        Me.lblusername.Location = New System.Drawing.Point(28, 9)
        Me.lblusername.Name = "lblusername"
        Me.lblusername.Size = New System.Drawing.Size(34, 17)
        Me.lblusername.TabIndex = 3
        Me.lblusername.Text = "user"
        '
        'lblqtym
        '
        Me.lblqtym.AutoSize = True
        Me.lblqtym.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblqtym.Location = New System.Drawing.Point(246, 154)
        Me.lblqtym.Name = "lblqtym"
        Me.lblqtym.Size = New System.Drawing.Size(58, 21)
        Me.lblqtym.TabIndex = 4
        Me.lblqtym.Text = "Label2"
        '
        'lblqtykg
        '
        Me.lblqtykg.AutoSize = True
        Me.lblqtykg.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblqtykg.Location = New System.Drawing.Point(504, 154)
        Me.lblqtykg.Name = "lblqtykg"
        Me.lblqtykg.Size = New System.Drawing.Size(58, 21)
        Me.lblqtykg.TabIndex = 5
        Me.lblqtykg.Text = "Label3"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(350, 44)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(82, 21)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "أمر شغل"
        '
        'txtkg
        '
        Me.txtkg.Location = New System.Drawing.Point(682, 279)
        Me.txtkg.Name = "txtkg"
        Me.txtkg.Size = New System.Drawing.Size(198, 24)
        Me.txtkg.TabIndex = 8
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(726, 236)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(101, 24)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "صرف وزن"
        '
        'btninsert
        '
        Me.btninsert.Font = New System.Drawing.Font("Tahoma", 13.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btninsert.Location = New System.Drawing.Point(541, 376)
        Me.btninsert.Name = "btninsert"
        Me.btninsert.Size = New System.Drawing.Size(140, 57)
        Me.btninsert.TabIndex = 11
        Me.btninsert.Text = "تسجيل"
        Me.btninsert.UseVisualStyleBackColor = True
        '
        'lblstatus
        '
        Me.lblstatus.AutoSize = True
        Me.lblstatus.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblstatus.Location = New System.Drawing.Point(117, 47)
        Me.lblstatus.Name = "lblstatus"
        Me.lblstatus.Size = New System.Drawing.Size(57, 21)
        Me.lblstatus.TabIndex = 12
        Me.lblstatus.Text = "الحالة"
        '
        'txtrolls
        '
        Me.txtrolls.Location = New System.Drawing.Point(295, 279)
        Me.txtrolls.Name = "txtrolls"
        Me.txtrolls.Size = New System.Drawing.Size(194, 24)
        Me.txtrolls.TabIndex = 13
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(331, 236)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(102, 24)
        Me.Label1.TabIndex = 14
        Me.Label1.Text = "عدد اتواب"
        '
        'porowtoinspectionForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1145, 510)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtrolls)
        Me.Controls.Add(Me.lblstatus)
        Me.Controls.Add(Me.btninsert)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtkg)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lblqtykg)
        Me.Controls.Add(Me.lblqtym)
        Me.Controls.Add(Me.lblusername)
        Me.Controls.Add(Me.lblcontractno)
        Me.Controls.Add(Me.lblbatchno)
        Me.Controls.Add(Me.cmbworder)
        Me.Name = "porowtoinspectionForm"
        Me.Text = "صرف القماش الخام للفحص"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmbworder As System.Windows.Forms.ComboBox
    Friend WithEvents lblbatchno As System.Windows.Forms.Label
    Friend WithEvents lblcontractno As System.Windows.Forms.Label
    Friend WithEvents lblusername As System.Windows.Forms.Label
    Friend WithEvents lblqtym As System.Windows.Forms.Label
    Friend WithEvents lblqtykg As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtkg As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btninsert As System.Windows.Forms.Button
    Friend WithEvents lblstatus As System.Windows.Forms.Label
    Friend WithEvents txtrolls As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
