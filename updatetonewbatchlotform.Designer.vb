<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class updatetonewbatchlotform
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
        Me.cmbBatchId = New System.Windows.Forms.ComboBox()
        Me.cmbLot = New System.Windows.Forms.ComboBox()
        Me.btnDuplicateBatch = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cmbChangeBatchTo = New System.Windows.Forms.ComboBox()
        Me.txtNewLot = New System.Windows.Forms.TextBox()
        Me.btnUpdateBatch = New System.Windows.Forms.Button()
        Me.chkUpdateOnly = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'cmbBatchId
        '
        Me.cmbBatchId.FormattingEnabled = True
        Me.cmbBatchId.Location = New System.Drawing.Point(289, 96)
        Me.cmbBatchId.Name = "cmbBatchId"
        Me.cmbBatchId.Size = New System.Drawing.Size(196, 24)
        Me.cmbBatchId.TabIndex = 0
        '
        'cmbLot
        '
        Me.cmbLot.FormattingEnabled = True
        Me.cmbLot.Location = New System.Drawing.Point(575, 96)
        Me.cmbLot.Name = "cmbLot"
        Me.cmbLot.Size = New System.Drawing.Size(193, 24)
        Me.cmbLot.TabIndex = 1
        '
        'btnDuplicateBatch
        '
        Me.btnDuplicateBatch.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDuplicateBatch.Location = New System.Drawing.Point(429, 177)
        Me.btnDuplicateBatch.Name = "btnDuplicateBatch"
        Me.btnDuplicateBatch.Size = New System.Drawing.Size(158, 33)
        Me.btnDuplicateBatch.TabIndex = 2
        Me.btnDuplicateBatch.Text = "انشاء وتحديث"
        Me.btnDuplicateBatch.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(312, 54)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(162, 21)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "رقم الرساله القديم"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(606, 54)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(144, 21)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "رقم اللوت القديم"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(589, 477)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(144, 21)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "رقم اللوت الجديد"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(290, 477)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(184, 21)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "تحويل الى رقم رساله"
        '
        'cmbChangeBatchTo
        '
        Me.cmbChangeBatchTo.FormattingEnabled = True
        Me.cmbChangeBatchTo.Location = New System.Drawing.Point(289, 519)
        Me.cmbChangeBatchTo.Name = "cmbChangeBatchTo"
        Me.cmbChangeBatchTo.Size = New System.Drawing.Size(196, 24)
        Me.cmbChangeBatchTo.TabIndex = 5
        '
        'txtNewLot
        '
        Me.txtNewLot.Location = New System.Drawing.Point(576, 519)
        Me.txtNewLot.Name = "txtNewLot"
        Me.txtNewLot.Size = New System.Drawing.Size(174, 24)
        Me.txtNewLot.TabIndex = 9
        '
        'btnUpdateBatch
        '
        Me.btnUpdateBatch.Font = New System.Drawing.Font("Tahoma", 13.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnUpdateBatch.Location = New System.Drawing.Point(926, 502)
        Me.btnUpdateBatch.Name = "btnUpdateBatch"
        Me.btnUpdateBatch.Size = New System.Drawing.Size(158, 49)
        Me.btnUpdateBatch.TabIndex = 10
        Me.btnUpdateBatch.Text = "تغير الى اللوت الجديد"
        Me.btnUpdateBatch.UseVisualStyleBackColor = True
        '
        'chkUpdateOnly
        '
        Me.chkUpdateOnly.AutoSize = True
        Me.chkUpdateOnly.Location = New System.Drawing.Point(508, 419)
        Me.chkUpdateOnly.Name = "chkUpdateOnly"
        Me.chkUpdateOnly.Size = New System.Drawing.Size(100, 21)
        Me.chkUpdateOnly.TabIndex = 11
        Me.chkUpdateOnly.Text = "CheckBox1"
        Me.chkUpdateOnly.UseVisualStyleBackColor = True
        '
        'updatetonewbatchlotform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1373, 674)
        Me.Controls.Add(Me.chkUpdateOnly)
        Me.Controls.Add(Me.btnUpdateBatch)
        Me.Controls.Add(Me.txtNewLot)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.cmbChangeBatchTo)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnDuplicateBatch)
        Me.Controls.Add(Me.cmbLot)
        Me.Controls.Add(Me.cmbBatchId)
        Me.Name = "updatetonewbatchlotform"
        Me.Text = "updatetonewbatchlotform"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents cmbBatchId As ComboBox
    Friend WithEvents cmbLot As ComboBox
    Friend WithEvents btnDuplicateBatch As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents cmbChangeBatchTo As ComboBox
    Friend WithEvents txtNewLot As TextBox
    Friend WithEvents btnUpdateBatch As Button
    Friend WithEvents chkUpdateOnly As CheckBox
End Class
