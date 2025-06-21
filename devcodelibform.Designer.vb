<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class devcodelibform
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
        Me.btninsert = New System.Windows.Forms.Button()
        Me.txtcodeall = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtcode = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtpublicname = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'btninsert
        '
        Me.btninsert.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btninsert.Location = New System.Drawing.Point(398, 426)
        Me.btninsert.Name = "btninsert"
        Me.btninsert.Size = New System.Drawing.Size(216, 76)
        Me.btninsert.TabIndex = 17
        Me.btninsert.Text = "Insert"
        Me.btninsert.UseVisualStyleBackColor = True
        '
        'txtcodeall
        '
        Me.txtcodeall.Location = New System.Drawing.Point(135, 184)
        Me.txtcodeall.Multiline = True
        Me.txtcodeall.Name = "txtcodeall"
        Me.txtcodeall.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtcodeall.Size = New System.Drawing.Size(924, 211)
        Me.txtcodeall.TabIndex = 16
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(554, 91)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(60, 24)
        Me.Label2.TabIndex = 15
        Me.Label2.Text = "Code"
        '
        'txtcode
        '
        Me.txtcode.Location = New System.Drawing.Point(645, 91)
        Me.txtcode.Multiline = True
        Me.txtcode.Name = "txtcode"
        Me.txtcode.Size = New System.Drawing.Size(202, 49)
        Me.txtcode.TabIndex = 14
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(114, 87)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 24)
        Me.Label1.TabIndex = 13
        Me.Label1.Text = "Public"
        '
        'txtpublicname
        '
        Me.txtpublicname.Location = New System.Drawing.Point(205, 87)
        Me.txtpublicname.Multiline = True
        Me.txtpublicname.Name = "txtpublicname"
        Me.txtpublicname.Size = New System.Drawing.Size(202, 49)
        Me.txtpublicname.TabIndex = 12
        '
        'devcodelibform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1172, 589)
        Me.Controls.Add(Me.btninsert)
        Me.Controls.Add(Me.txtcodeall)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtcode)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtpublicname)
        Me.Name = "devcodelibform"
        Me.Text = "devcodelibform"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btninsert As System.Windows.Forms.Button
    Friend WithEvents txtcodeall As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtcode As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtpublicname As System.Windows.Forms.TextBox
End Class
