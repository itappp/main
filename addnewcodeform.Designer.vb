<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class addnewcodeform
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
        Me.txtpublicname = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtcode = New System.Windows.Forms.TextBox()
        Me.btninsert = New System.Windows.Forms.Button()
        Me.cmbMachines = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cmbProcesses = New System.Windows.Forms.ComboBox()
        Me.btnAddProcess = New System.Windows.Forms.Button()
        Me.txtcodeall = New System.Windows.Forms.TextBox()
        Me.btnUndo = New System.Windows.Forms.Button()
        Me.txtProcessIds = New System.Windows.Forms.TextBox()
        Me.lblUsername = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'txtpublicname
        '
        Me.txtpublicname.Location = New System.Drawing.Point(705, 535)
        Me.txtpublicname.Multiline = True
        Me.txtpublicname.Name = "txtpublicname"
        Me.txtpublicname.Size = New System.Drawing.Size(431, 49)
        Me.txtpublicname.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(894, 495)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 24)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Public"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(397, 495)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(60, 24)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Code"
        '
        'txtcode
        '
        Me.txtcode.Location = New System.Drawing.Point(330, 535)
        Me.txtcode.Name = "txtcode"
        Me.txtcode.Size = New System.Drawing.Size(244, 24)
        Me.txtcode.TabIndex = 2
        '
        'btninsert
        '
        Me.btninsert.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btninsert.Location = New System.Drawing.Point(586, 673)
        Me.btninsert.Name = "btninsert"
        Me.btninsert.Size = New System.Drawing.Size(216, 76)
        Me.btninsert.TabIndex = 5
        Me.btninsert.Text = "Insert"
        Me.btninsert.UseVisualStyleBackColor = True
        '
        'cmbMachines
        '
        Me.cmbMachines.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbMachines.FormattingEnabled = True
        Me.cmbMachines.Location = New System.Drawing.Point(358, 180)
        Me.cmbMachines.Name = "cmbMachines"
        Me.cmbMachines.Size = New System.Drawing.Size(170, 32)
        Me.cmbMachines.TabIndex = 6
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(399, 129)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(95, 24)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Machine"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(725, 129)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(98, 24)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "Proccess"
        '
        'cmbProcesses
        '
        Me.cmbProcesses.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbProcesses.FormattingEnabled = True
        Me.cmbProcesses.Location = New System.Drawing.Point(606, 180)
        Me.cmbProcesses.Name = "cmbProcesses"
        Me.cmbProcesses.Size = New System.Drawing.Size(506, 32)
        Me.cmbProcesses.TabIndex = 8
        '
        'btnAddProcess
        '
        Me.btnAddProcess.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAddProcess.Location = New System.Drawing.Point(690, 273)
        Me.btnAddProcess.Name = "btnAddProcess"
        Me.btnAddProcess.Size = New System.Drawing.Size(112, 54)
        Me.btnAddProcess.TabIndex = 12
        Me.btnAddProcess.Text = "اضافه"
        Me.btnAddProcess.UseVisualStyleBackColor = True
        '
        'txtcodeall
        '
        Me.txtcodeall.Location = New System.Drawing.Point(32, 358)
        Me.txtcodeall.Multiline = True
        Me.txtcodeall.Name = "txtcodeall"
        Me.txtcodeall.ReadOnly = True
        Me.txtcodeall.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtcodeall.Size = New System.Drawing.Size(1224, 93)
        Me.txtcodeall.TabIndex = 4
        '
        'btnUndo
        '
        Me.btnUndo.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnUndo.Location = New System.Drawing.Point(113, 240)
        Me.btnUndo.Name = "btnUndo"
        Me.btnUndo.Size = New System.Drawing.Size(97, 32)
        Me.btnUndo.TabIndex = 13
        Me.btnUndo.Text = "تراجع"
        Me.btnUndo.UseVisualStyleBackColor = True
        '
        'txtProcessIds
        '
        Me.txtProcessIds.Location = New System.Drawing.Point(32, 52)
        Me.txtProcessIds.Multiline = True
        Me.txtProcessIds.Name = "txtProcessIds"
        Me.txtProcessIds.ReadOnly = True
        Me.txtProcessIds.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtProcessIds.Size = New System.Drawing.Size(425, 32)
        Me.txtProcessIds.TabIndex = 14
        '
        'lblUsername
        '
        Me.lblUsername.AutoSize = True
        Me.lblUsername.Location = New System.Drawing.Point(68, 13)
        Me.lblUsername.Name = "lblUsername"
        Me.lblUsername.Size = New System.Drawing.Size(34, 17)
        Me.lblUsername.TabIndex = 15
        Me.lblUsername.Text = "user"
        '
        'addnewcodeform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1305, 809)
        Me.Controls.Add(Me.lblUsername)
        Me.Controls.Add(Me.txtProcessIds)
        Me.Controls.Add(Me.btnUndo)
        Me.Controls.Add(Me.btnAddProcess)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.cmbProcesses)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.cmbMachines)
        Me.Controls.Add(Me.btninsert)
        Me.Controls.Add(Me.txtcodeall)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtcode)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtpublicname)
        Me.Name = "addnewcodeform"
        Me.Text = "addnewcodeform"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtpublicname As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtcode As System.Windows.Forms.TextBox
    Friend WithEvents btninsert As System.Windows.Forms.Button
    Friend WithEvents cmbMachines As ComboBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents cmbProcesses As ComboBox
    Friend WithEvents btnAddProcess As Button
    Friend WithEvents txtcodeall As TextBox
    Friend WithEvents btnUndo As Button
    Friend WithEvents txtProcessIds As TextBox
    Friend WithEvents lblUsername As Label
End Class
