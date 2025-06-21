<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dentryform
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
        Me.btnInsert = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblcodelib = New System.Windows.Forms.Label()
        Me.lblkindcontract = New System.Windows.Forms.Label()
        Me.lblcontractid = New System.Windows.Forms.Label()
        Me.lblbatchno = New System.Windows.Forms.Label()
        Me.lblcontractno = New System.Windows.Forms.Label()
        Me.lblusername = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbworder = New System.Windows.Forms.ComboBox()
        Me.SuspendLayout()
        '
        'btnInsert
        '
        Me.btnInsert.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnInsert.Location = New System.Drawing.Point(193, 27)
        Me.btnInsert.Name = "btnInsert"
        Me.btnInsert.Size = New System.Drawing.Size(109, 32)
        Me.btnInsert.TabIndex = 47
        Me.btnInsert.Text = "تسجيل"
        Me.btnInsert.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.AutoScroll = True
        Me.Panel1.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel1.Location = New System.Drawing.Point(17, 65)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Panel1.Size = New System.Drawing.Size(1485, 766)
        Me.Panel1.TabIndex = 46
        '
        'lblcodelib
        '
        Me.lblcodelib.AutoSize = True
        Me.lblcodelib.Location = New System.Drawing.Point(898, 7)
        Me.lblcodelib.Name = "lblcodelib"
        Me.lblcodelib.Size = New System.Drawing.Size(47, 17)
        Me.lblcodelib.TabIndex = 45
        Me.lblcodelib.Text = "Label2"
        '
        'lblkindcontract
        '
        Me.lblkindcontract.AutoSize = True
        Me.lblkindcontract.Location = New System.Drawing.Point(1041, 6)
        Me.lblkindcontract.Name = "lblkindcontract"
        Me.lblkindcontract.Size = New System.Drawing.Size(47, 17)
        Me.lblkindcontract.TabIndex = 44
        Me.lblkindcontract.Text = "Label2"
        '
        'lblcontractid
        '
        Me.lblcontractid.AutoSize = True
        Me.lblcontractid.Location = New System.Drawing.Point(1495, 16)
        Me.lblcontractid.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblcontractid.Name = "lblcontractid"
        Me.lblcontractid.Size = New System.Drawing.Size(0, 17)
        Me.lblcontractid.TabIndex = 43
        Me.lblcontractid.Visible = False
        '
        'lblbatchno
        '
        Me.lblbatchno.AutoSize = True
        Me.lblbatchno.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblbatchno.Location = New System.Drawing.Point(1248, 12)
        Me.lblbatchno.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblbatchno.Name = "lblbatchno"
        Me.lblbatchno.Size = New System.Drawing.Size(0, 21)
        Me.lblbatchno.TabIndex = 42
        '
        'lblcontractno
        '
        Me.lblcontractno.AutoSize = True
        Me.lblcontractno.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblcontractno.Location = New System.Drawing.Point(1399, 12)
        Me.lblcontractno.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblcontractno.Name = "lblcontractno"
        Me.lblcontractno.Size = New System.Drawing.Size(0, 21)
        Me.lblcontractno.TabIndex = 41
        '
        'lblusername
        '
        Me.lblusername.AutoSize = True
        Me.lblusername.Location = New System.Drawing.Point(14, 12)
        Me.lblusername.Name = "lblusername"
        Me.lblusername.Size = New System.Drawing.Size(34, 17)
        Me.lblusername.TabIndex = 40
        Me.lblusername.Text = "user"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(377, 7)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(93, 21)
        Me.Label1.TabIndex = 39
        Me.Label1.Text = "أمر الشغل"
        '
        'cmbworder
        '
        Me.cmbworder.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbworder.FormattingEnabled = True
        Me.cmbworder.Location = New System.Drawing.Point(478, 7)
        Me.cmbworder.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbworder.Name = "cmbworder"
        Me.cmbworder.Size = New System.Drawing.Size(316, 32)
        Me.cmbworder.TabIndex = 38
        '
        'dentryform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1583, 706)
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
        Me.Name = "dentryform"
        Me.Text = "dentryform"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnInsert As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lblcodelib As System.Windows.Forms.Label
    Friend WithEvents lblkindcontract As System.Windows.Forms.Label
    Friend WithEvents lblcontractid As System.Windows.Forms.Label
    Friend WithEvents lblbatchno As System.Windows.Forms.Label
    Friend WithEvents lblcontractno As System.Windows.Forms.Label
    Friend WithEvents lblusername As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbworder As System.Windows.Forms.ComboBox
End Class
