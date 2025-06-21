<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class recievedchimicform
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
        Me.cmbCode = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtQty = New System.Windows.Forms.TextBox()
        Me.txtNumberUnit = New System.Windows.Forms.TextBox()
        Me.txtWeightUnit = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btnAddBalance = New System.Windows.Forms.Button()
        Me.cmbCodeType = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.dgv = New System.Windows.Forms.DataGridView()
        Me.cmbMovementType = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cmbDisributOrder = New System.Windows.Forms.ComboBox()
        CType(Me.dgv, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmbCode
        '
        Me.cmbCode.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbCode.FormattingEnabled = True
        Me.cmbCode.Location = New System.Drawing.Point(721, 272)
        Me.cmbCode.Name = "cmbCode"
        Me.cmbCode.Size = New System.Drawing.Size(302, 29)
        Me.cmbCode.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(811, 238)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(125, 21)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "كود الكيماويات"
        '
        'txtQty
        '
        Me.txtQty.Location = New System.Drawing.Point(286, 491)
        Me.txtQty.Name = "txtQty"
        Me.txtQty.Size = New System.Drawing.Size(145, 24)
        Me.txtQty.TabIndex = 2
        '
        'txtNumberUnit
        '
        Me.txtNumberUnit.Location = New System.Drawing.Point(550, 491)
        Me.txtNumberUnit.Name = "txtNumberUnit"
        Me.txtNumberUnit.Size = New System.Drawing.Size(145, 24)
        Me.txtNumberUnit.TabIndex = 3
        '
        'txtWeightUnit
        '
        Me.txtWeightUnit.Location = New System.Drawing.Point(791, 491)
        Me.txtWeightUnit.Name = "txtWeightUnit"
        Me.txtWeightUnit.Size = New System.Drawing.Size(145, 24)
        Me.txtWeightUnit.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(800, 449)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(98, 21)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "وزن الوحدة"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(570, 449)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(111, 21)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "عدد الوحدات"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(316, 449)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(60, 21)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "الكمية"
        '
        'btnAddBalance
        '
        Me.btnAddBalance.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAddBalance.Location = New System.Drawing.Point(563, 608)
        Me.btnAddBalance.Name = "btnAddBalance"
        Me.btnAddBalance.Size = New System.Drawing.Size(138, 45)
        Me.btnAddBalance.TabIndex = 8
        Me.btnAddBalance.Text = "تسجيل"
        Me.btnAddBalance.UseVisualStyleBackColor = True
        '
        'cmbCodeType
        '
        Me.cmbCodeType.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbCodeType.FormattingEnabled = True
        Me.cmbCodeType.Location = New System.Drawing.Point(296, 272)
        Me.cmbCodeType.Name = "cmbCodeType"
        Me.cmbCodeType.Size = New System.Drawing.Size(190, 29)
        Me.cmbCodeType.TabIndex = 9
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(334, 238)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(96, 21)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "نوع الصنف"
        '
        'dgv
        '
        Me.dgv.AllowUserToAddRows = False
        Me.dgv.AllowUserToDeleteRows = False
        Me.dgv.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dgv.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv.Location = New System.Drawing.Point(279, 7)
        Me.dgv.Name = "dgv"
        Me.dgv.RowHeadersWidth = 51
        Me.dgv.RowTemplate.Height = 26
        Me.dgv.Size = New System.Drawing.Size(658, 105)
        Me.dgv.TabIndex = 11
        '
        'cmbMovementType
        '
        Me.cmbMovementType.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbMovementType.FormattingEnabled = True
        Me.cmbMovementType.Location = New System.Drawing.Point(545, 200)
        Me.cmbMovementType.Name = "cmbMovementType"
        Me.cmbMovementType.Size = New System.Drawing.Size(190, 29)
        Me.cmbMovementType.TabIndex = 12
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(575, 163)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(96, 21)
        Me.Label6.TabIndex = 13
        Me.Label6.Text = "نوع الحركه"
        '
        'cmbDisributOrder
        '
        Me.cmbDisributOrder.FormattingEnabled = True
        Me.cmbDisributOrder.Location = New System.Drawing.Point(76, 200)
        Me.cmbDisributOrder.Name = "cmbDisributOrder"
        Me.cmbDisributOrder.Size = New System.Drawing.Size(218, 24)
        Me.cmbDisributOrder.TabIndex = 14
        Me.cmbDisributOrder.Visible = False
        '
        'recievedchimicform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1318, 796)
        Me.Controls.Add(Me.cmbDisributOrder)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.cmbMovementType)
        Me.Controls.Add(Me.dgv)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.cmbCodeType)
        Me.Controls.Add(Me.btnAddBalance)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtWeightUnit)
        Me.Controls.Add(Me.txtNumberUnit)
        Me.Controls.Add(Me.txtQty)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmbCode)
        Me.Name = "recievedchimicform"
        Me.Text = "recievedchimicform"
        CType(Me.dgv, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents cmbCode As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents txtQty As TextBox
    Friend WithEvents txtNumberUnit As TextBox
    Friend WithEvents txtWeightUnit As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents btnAddBalance As Button
    Friend WithEvents cmbCodeType As ComboBox
    Friend WithEvents Label5 As Label
    Friend WithEvents dgv As DataGridView
    Friend WithEvents cmbMovementType As ComboBox
    Friend WithEvents Label6 As Label
    Friend WithEvents cmbDisributOrder As ComboBox
End Class
