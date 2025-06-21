<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class planningForm
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
        Me.lblusername = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbworder = New System.Windows.Forms.ComboBox()
        Me.lblcontractid = New System.Windows.Forms.Label()
        Me.lblbatchno = New System.Windows.Forms.Label()
        Me.lblcontractno = New System.Windows.Forms.Label()
        Me.lblkindcontract = New System.Windows.Forms.Label()
        Me.lblcodelib = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnInsert = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lblusername
        '
        Me.lblusername.AutoSize = True
        Me.lblusername.Location = New System.Drawing.Point(20, 15)
        Me.lblusername.Name = "lblusername"
        Me.lblusername.Size = New System.Drawing.Size(34, 17)
        Me.lblusername.TabIndex = 20
        Me.lblusername.Text = "user"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(383, 10)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(93, 21)
        Me.Label1.TabIndex = 19
        Me.Label1.Text = "أمر الشغل"
        '
        'cmbworder
        '
        Me.cmbworder.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbworder.FormattingEnabled = True
        Me.cmbworder.Location = New System.Drawing.Point(484, 10)
        Me.cmbworder.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbworder.Name = "cmbworder"
        Me.cmbworder.Size = New System.Drawing.Size(316, 32)
        Me.cmbworder.TabIndex = 18
        '
        'lblcontractid
        '
        Me.lblcontractid.AutoSize = True
        Me.lblcontractid.Location = New System.Drawing.Point(1501, 19)
        Me.lblcontractid.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblcontractid.Name = "lblcontractid"
        Me.lblcontractid.Size = New System.Drawing.Size(0, 17)
        Me.lblcontractid.TabIndex = 23
        Me.lblcontractid.Visible = False
        '
        'lblbatchno
        '
        Me.lblbatchno.AutoSize = True
        Me.lblbatchno.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblbatchno.Location = New System.Drawing.Point(1254, 15)
        Me.lblbatchno.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblbatchno.Name = "lblbatchno"
        Me.lblbatchno.Size = New System.Drawing.Size(0, 21)
        Me.lblbatchno.TabIndex = 22
        '
        'lblcontractno
        '
        Me.lblcontractno.AutoSize = True
        Me.lblcontractno.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblcontractno.Location = New System.Drawing.Point(1405, 15)
        Me.lblcontractno.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblcontractno.Name = "lblcontractno"
        Me.lblcontractno.Size = New System.Drawing.Size(0, 21)
        Me.lblcontractno.TabIndex = 21
        '
        'lblkindcontract
        '
        Me.lblkindcontract.AutoSize = True
        Me.lblkindcontract.Location = New System.Drawing.Point(1047, 9)
        Me.lblkindcontract.Name = "lblkindcontract"
        Me.lblkindcontract.Size = New System.Drawing.Size(47, 17)
        Me.lblkindcontract.TabIndex = 24
        Me.lblkindcontract.Text = "Label2"
        '
        'lblcodelib
        '
        Me.lblcodelib.AutoSize = True
        Me.lblcodelib.Location = New System.Drawing.Point(904, 10)
        Me.lblcodelib.Name = "lblcodelib"
        Me.lblcodelib.Size = New System.Drawing.Size(47, 17)
        Me.lblcodelib.TabIndex = 25
        Me.lblcodelib.Text = "Label2"
        '
        'Panel1
        '
        Me.Panel1.AutoScroll = True
        Me.Panel1.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel1.Location = New System.Drawing.Point(23, 68)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Panel1.Size = New System.Drawing.Size(1485, 766)
        Me.Panel1.TabIndex = 26
        '
        'btnInsert
        '
        Me.btnInsert.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnInsert.Location = New System.Drawing.Point(199, 30)
        Me.btnInsert.Name = "btnInsert"
        Me.btnInsert.Size = New System.Drawing.Size(109, 32)
        Me.btnInsert.TabIndex = 27
        Me.btnInsert.Text = "تسجيل"
        Me.btnInsert.UseVisualStyleBackColor = True
        '
        'planningForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1567, 846)
        Me.Controls.Add(Me.btnInsert)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.lblcodelib)
        Me.Controls.Add(Me.lblkindcontract)
        Me.Controls.Add(Me.lblcontractid)
        Me.Controls.Add(Me.lblbatchno)
        Me.Controls.Add(Me.lblcontractno)
        Me.Controls.Add(Me.lblusername)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmbworder)
        Me.Name = "planningForm"
        Me.Text = "planningForm"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblusername As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbworder As System.Windows.Forms.ComboBox
    Friend WithEvents lblcontractid As System.Windows.Forms.Label
    Friend WithEvents lblbatchno As System.Windows.Forms.Label
    Friend WithEvents lblcontractno As System.Windows.Forms.Label
    Friend WithEvents lblkindcontract As System.Windows.Forms.Label
    Friend WithEvents lblcodelib As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnInsert As System.Windows.Forms.Button
End Class
