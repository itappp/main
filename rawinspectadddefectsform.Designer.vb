<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class rawinspectadddefectsform
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtdefect = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'btninsert
        '
        Me.btninsert.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btninsert.Location = New System.Drawing.Point(412, 347)
        Me.btninsert.Name = "btninsert"
        Me.btninsert.Size = New System.Drawing.Size(153, 44)
        Me.btninsert.TabIndex = 5
        Me.btninsert.Text = "تسجيل"
        Me.btninsert.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(453, 177)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(64, 24)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "العيب"
        '
        'txtdefect
        '
        Me.txtdefect.Location = New System.Drawing.Point(260, 236)
        Me.txtdefect.Name = "txtdefect"
        Me.txtdefect.Size = New System.Drawing.Size(436, 24)
        Me.txtdefect.TabIndex = 3
        '
        'rawinspectadddefectsform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(957, 568)
        Me.Controls.Add(Me.btninsert)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtdefect)
        Me.Name = "rawinspectadddefectsform"
        Me.Text = "rawinspectadddefectsform"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btninsert As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents txtdefect As TextBox
End Class
