<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class accaddclientform
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
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.TextBoxCode = New System.Windows.Forms.TextBox()
        Me.TextBoxName = New System.Windows.Forms.TextBox()
        Me.TextBoxGroup = New System.Windows.Forms.TextBox()
        Me.ButtonAdd = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.LabelClientFCode = New System.Windows.Forms.Label()
        Me.LabelClientPCode = New System.Windows.Forms.Label()
        Me.LabelSupplierCode = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'ComboBox1
        '
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(409, 107)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(182, 24)
        Me.ComboBox1.TabIndex = 0
        '
        'TextBoxCode
        '
        Me.TextBoxCode.Location = New System.Drawing.Point(66, 234)
        Me.TextBoxCode.Name = "TextBoxCode"
        Me.TextBoxCode.Size = New System.Drawing.Size(157, 24)
        Me.TextBoxCode.TabIndex = 1
        '
        'TextBoxName
        '
        Me.TextBoxName.Location = New System.Drawing.Point(409, 234)
        Me.TextBoxName.Name = "TextBoxName"
        Me.TextBoxName.Size = New System.Drawing.Size(175, 24)
        Me.TextBoxName.TabIndex = 2
        '
        'TextBoxGroup
        '
        Me.TextBoxGroup.Location = New System.Drawing.Point(705, 234)
        Me.TextBoxGroup.Name = "TextBoxGroup"
        Me.TextBoxGroup.Size = New System.Drawing.Size(202, 24)
        Me.TextBoxGroup.TabIndex = 3
        '
        'ButtonAdd
        '
        Me.ButtonAdd.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAdd.Location = New System.Drawing.Point(409, 366)
        Me.ButtonAdd.Name = "ButtonAdd"
        Me.ButtonAdd.Size = New System.Drawing.Size(163, 53)
        Me.ButtonAdd.TabIndex = 4
        Me.ButtonAdd.Text = "تسجيل"
        Me.ButtonAdd.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(125, 195)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(43, 24)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "كود"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(482, 195)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(67, 24)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "الاسم"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(785, 195)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(65, 24)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "جروب"
        '
        'LabelClientFCode
        '
        Me.LabelClientFCode.AutoSize = True
        Me.LabelClientFCode.Location = New System.Drawing.Point(93, 24)
        Me.LabelClientFCode.Name = "LabelClientFCode"
        Me.LabelClientFCode.Size = New System.Drawing.Size(47, 17)
        Me.LabelClientFCode.TabIndex = 8
        Me.LabelClientFCode.Text = "Label4"
        '
        'LabelClientPCode
        '
        Me.LabelClientPCode.AutoSize = True
        Me.LabelClientPCode.Location = New System.Drawing.Point(393, 24)
        Me.LabelClientPCode.Name = "LabelClientPCode"
        Me.LabelClientPCode.Size = New System.Drawing.Size(47, 17)
        Me.LabelClientPCode.TabIndex = 9
        Me.LabelClientPCode.Text = "Label5"
        '
        'LabelSupplierCode
        '
        Me.LabelSupplierCode.AutoSize = True
        Me.LabelSupplierCode.Location = New System.Drawing.Point(789, 24)
        Me.LabelSupplierCode.Name = "LabelSupplierCode"
        Me.LabelSupplierCode.Size = New System.Drawing.Size(47, 17)
        Me.LabelSupplierCode.TabIndex = 10
        Me.LabelSupplierCode.Text = "Label6"
        '
        'accaddclientform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1062, 564)
        Me.Controls.Add(Me.LabelSupplierCode)
        Me.Controls.Add(Me.LabelClientPCode)
        Me.Controls.Add(Me.LabelClientFCode)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ButtonAdd)
        Me.Controls.Add(Me.TextBoxGroup)
        Me.Controls.Add(Me.TextBoxName)
        Me.Controls.Add(Me.TextBoxCode)
        Me.Controls.Add(Me.ComboBox1)
        Me.Name = "accaddclientform"
        Me.Text = "accaddclientform"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents TextBoxCode As TextBox
    Friend WithEvents TextBoxName As TextBox
    Friend WithEvents TextBoxGroup As TextBox
    Friend WithEvents ButtonAdd As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents LabelClientFCode As Label
    Friend WithEvents LabelClientPCode As Label
    Friend WithEvents LabelSupplierCode As Label
End Class
