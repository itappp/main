<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class AddChimicItemForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.txtCode = New System.Windows.Forms.TextBox()
        Me.txtProductName = New System.Windows.Forms.TextBox()
        Me.btnInsert = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmbCodeType = New System.Windows.Forms.ComboBox()
        Me.lblNextCode = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'txtCode
        '
        Me.txtCode.Location = New System.Drawing.Point(46, 197)
        Me.txtCode.Name = "txtCode"
        Me.txtCode.Size = New System.Drawing.Size(139, 24)
        Me.txtCode.TabIndex = 0
        '
        'txtProductName
        '
        Me.txtProductName.Location = New System.Drawing.Point(294, 197)
        Me.txtProductName.Name = "txtProductName"
        Me.txtProductName.Size = New System.Drawing.Size(758, 24)
        Me.txtProductName.TabIndex = 1
        '
        'btnInsert
        '
        Me.btnInsert.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnInsert.Location = New System.Drawing.Point(476, 345)
        Me.btnInsert.Name = "btnInsert"
        Me.btnInsert.Size = New System.Drawing.Size(152, 53)
        Me.btnInsert.TabIndex = 2
        Me.btnInsert.Text = "تسجيل"
        Me.btnInsert.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(85, 159)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(60, 24)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Code"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(596, 147)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(153, 24)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Product Name"
        '
        'cmbCodeType
        '
        Me.cmbCodeType.FormattingEnabled = True
        Me.cmbCodeType.Location = New System.Drawing.Point(485, 58)
        Me.cmbCodeType.Name = "cmbCodeType"
        Me.cmbCodeType.Size = New System.Drawing.Size(148, 24)
        Me.cmbCodeType.TabIndex = 5
        '
        'lblNextCode
        '
        Me.lblNextCode.AutoSize = True
        Me.lblNextCode.Location = New System.Drawing.Point(212, 58)
        Me.lblNextCode.Name = "lblNextCode"
        Me.lblNextCode.Size = New System.Drawing.Size(47, 17)
        Me.lblNextCode.TabIndex = 6
        Me.lblNextCode.Text = "Label3"
        '
        'AddChimicItemForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1186, 557)
        Me.Controls.Add(Me.lblNextCode)
        Me.Controls.Add(Me.cmbCodeType)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnInsert)
        Me.Controls.Add(Me.txtProductName)
        Me.Controls.Add(Me.txtCode)
        Me.Name = "AddChimicItemForm"
        Me.Text = "addchimicitemform"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtCode As TextBox
    Friend WithEvents txtProductName As TextBox
    Friend WithEvents btnInsert As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents cmbCodeType As ComboBox
    Friend WithEvents lblNextCode As Label
End Class
