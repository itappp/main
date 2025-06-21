<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class UpdateDeliveryDateForm
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
        Me.cmbworder = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.dtpDeliveryDate = New System.Windows.Forms.DateTimePicker()
        Me.btnupdate = New System.Windows.Forms.Button()
        Me.lblDeliveryDate = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'cmbworder
        '
        Me.cmbworder.FormattingEnabled = True
        Me.cmbworder.Location = New System.Drawing.Point(309, 73)
        Me.cmbworder.Name = "cmbworder"
        Me.cmbworder.Size = New System.Drawing.Size(284, 24)
        Me.cmbworder.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(382, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(108, 24)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "أمر الشغل"
        '
        'dtpDeliveryDate
        '
        Me.dtpDeliveryDate.Location = New System.Drawing.Point(362, 306)
        Me.dtpDeliveryDate.Name = "dtpDeliveryDate"
        Me.dtpDeliveryDate.Size = New System.Drawing.Size(200, 24)
        Me.dtpDeliveryDate.TabIndex = 2
        '
        'btnupdate
        '
        Me.btnupdate.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnupdate.Location = New System.Drawing.Point(408, 389)
        Me.btnupdate.Name = "btnupdate"
        Me.btnupdate.Size = New System.Drawing.Size(112, 46)
        Me.btnupdate.TabIndex = 3
        Me.btnupdate.Text = "تحديث"
        Me.btnupdate.UseVisualStyleBackColor = True
        '
        'lblDeliveryDate
        '
        Me.lblDeliveryDate.AutoSize = True
        Me.lblDeliveryDate.Location = New System.Drawing.Point(424, 188)
        Me.lblDeliveryDate.Name = "lblDeliveryDate"
        Me.lblDeliveryDate.Size = New System.Drawing.Size(47, 17)
        Me.lblDeliveryDate.TabIndex = 4
        Me.lblDeliveryDate.Text = "Label2"
        '
        'UpdateDeliveryDateForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(888, 594)
        Me.Controls.Add(Me.lblDeliveryDate)
        Me.Controls.Add(Me.btnupdate)
        Me.Controls.Add(Me.dtpDeliveryDate)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmbworder)
        Me.Name = "UpdateDeliveryDateForm"
        Me.Text = "updatedeliverydateform"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents cmbworder As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents dtpDeliveryDate As DateTimePicker
    Friend WithEvents btnupdate As Button
    Friend WithEvents lblDeliveryDate As Label
End Class
