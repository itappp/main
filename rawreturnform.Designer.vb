<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class rawreturnform
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
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cmbLot = New System.Windows.Forms.ComboBox()
        Me.cmbbatch = New System.Windows.Forms.ComboBox()
        Me.txtReturnedWeight = New System.Windows.Forms.TextBox()
        Me.txtReturnedRolls = New System.Windows.Forms.TextBox()
        Me.btnReturn = New System.Windows.Forms.Button()
        Me.dgvStock = New System.Windows.Forms.DataGridView()
        Me.lblUsername = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtref = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        CType(Me.dgvStock, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(861, 45)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(36, 24)
        Me.Label4.TabIndex = 15
        Me.Label4.Text = "lot"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(287, 45)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(129, 24)
        Me.Label3.TabIndex = 14
        Me.Label3.Text = "رقم الرسالة "
        '
        'cmbLot
        '
        Me.cmbLot.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbLot.FormattingEnabled = True
        Me.cmbLot.Location = New System.Drawing.Point(757, 80)
        Me.cmbLot.Name = "cmbLot"
        Me.cmbLot.Size = New System.Drawing.Size(257, 32)
        Me.cmbLot.TabIndex = 13
        '
        'cmbbatch
        '
        Me.cmbbatch.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbbatch.FormattingEnabled = True
        Me.cmbbatch.Location = New System.Drawing.Point(217, 80)
        Me.cmbbatch.Name = "cmbbatch"
        Me.cmbbatch.Size = New System.Drawing.Size(257, 32)
        Me.cmbbatch.TabIndex = 12
        '
        'txtReturnedWeight
        '
        Me.txtReturnedWeight.Location = New System.Drawing.Point(501, 416)
        Me.txtReturnedWeight.Name = "txtReturnedWeight"
        Me.txtReturnedWeight.Size = New System.Drawing.Size(162, 24)
        Me.txtReturnedWeight.TabIndex = 16
        '
        'txtReturnedRolls
        '
        Me.txtReturnedRolls.Location = New System.Drawing.Point(865, 416)
        Me.txtReturnedRolls.Name = "txtReturnedRolls"
        Me.txtReturnedRolls.Size = New System.Drawing.Size(162, 24)
        Me.txtReturnedRolls.TabIndex = 17
        '
        'btnReturn
        '
        Me.btnReturn.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnReturn.Location = New System.Drawing.Point(661, 505)
        Me.btnReturn.Name = "btnReturn"
        Me.btnReturn.Size = New System.Drawing.Size(146, 52)
        Me.btnReturn.TabIndex = 18
        Me.btnReturn.Text = "تسجيل"
        Me.btnReturn.UseVisualStyleBackColor = True
        '
        'dgvStock
        '
        Me.dgvStock.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dgvStock.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvStock.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvStock.Location = New System.Drawing.Point(185, 172)
        Me.dgvStock.Name = "dgvStock"
        Me.dgvStock.RowHeadersWidth = 51
        Me.dgvStock.RowTemplate.Height = 26
        Me.dgvStock.Size = New System.Drawing.Size(808, 150)
        Me.dgvStock.TabIndex = 19
        '
        'lblUsername
        '
        Me.lblUsername.AutoSize = True
        Me.lblUsername.Location = New System.Drawing.Point(29, 9)
        Me.lblUsername.Name = "lblUsername"
        Me.lblUsername.Size = New System.Drawing.Size(34, 17)
        Me.lblUsername.TabIndex = 20
        Me.lblUsername.Text = "user"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(534, 375)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(94, 21)
        Me.Label1.TabIndex = 21
        Me.Label1.Text = "مرتجع وزن"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(888, 375)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(106, 21)
        Me.Label2.TabIndex = 22
        Me.Label2.Text = "مرتجع أتواب"
        '
        'txtref
        '
        Me.txtref.Location = New System.Drawing.Point(111, 416)
        Me.txtref.Name = "txtref"
        Me.txtref.Size = New System.Drawing.Size(177, 24)
        Me.txtref.TabIndex = 23
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(158, 375)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(91, 21)
        Me.Label5.TabIndex = 24
        Me.Label5.Text = "اذن مرتجع"
        '
        'rawreturnform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1252, 616)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtref)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblUsername)
        Me.Controls.Add(Me.dgvStock)
        Me.Controls.Add(Me.btnReturn)
        Me.Controls.Add(Me.txtReturnedRolls)
        Me.Controls.Add(Me.txtReturnedWeight)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.cmbLot)
        Me.Controls.Add(Me.cmbbatch)
        Me.Name = "rawreturnform"
        Me.Text = "rawreturnform"
        CType(Me.dgvStock, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents cmbLot As ComboBox
    Friend WithEvents cmbbatch As ComboBox
    Friend WithEvents txtReturnedWeight As TextBox
    Friend WithEvents txtReturnedRolls As TextBox
    Friend WithEvents btnReturn As Button
    Friend WithEvents dgvStock As DataGridView
    Friend WithEvents lblUsername As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents txtref As TextBox
    Friend WithEvents Label5 As Label
End Class
