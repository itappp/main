<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class rawdisbursementform
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
        Me.txtIssuedWeight = New System.Windows.Forms.TextBox()
        Me.txtIssuedRolls = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmbbatch = New System.Windows.Forms.ComboBox()
        Me.btninsert = New System.Windows.Forms.Button()
        Me.cmbLot = New System.Windows.Forms.ComboBox()
        Me.dgvStock = New System.Windows.Forms.DataGridView()
        Me.cmbworder = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.lblbatchno = New System.Windows.Forms.Label()
        Me.lblcontractno = New System.Windows.Forms.Label()
        Me.lblqtym = New System.Windows.Forms.Label()
        Me.lblqtykg = New System.Windows.Forms.Label()
        Me.lblUsername = New System.Windows.Forms.Label()
        Me.txtref = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        CType(Me.dgvStock, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtIssuedWeight
        '
        Me.txtIssuedWeight.Location = New System.Drawing.Point(579, 500)
        Me.txtIssuedWeight.Name = "txtIssuedWeight"
        Me.txtIssuedWeight.Size = New System.Drawing.Size(132, 23)
        Me.txtIssuedWeight.TabIndex = 0
        '
        'txtIssuedRolls
        '
        Me.txtIssuedRolls.Location = New System.Drawing.Point(841, 500)
        Me.txtIssuedRolls.Name = "txtIssuedRolls"
        Me.txtIssuedRolls.Size = New System.Drawing.Size(128, 23)
        Me.txtIssuedRolls.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(849, 469)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(102, 24)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "عدد اتواب"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(613, 469)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(59, 24)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "الوزن"
        '
        'cmbbatch
        '
        Me.cmbbatch.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbbatch.FormattingEnabled = True
        Me.cmbbatch.Location = New System.Drawing.Point(103, 249)
        Me.cmbbatch.Name = "cmbbatch"
        Me.cmbbatch.Size = New System.Drawing.Size(257, 32)
        Me.cmbbatch.TabIndex = 5
        '
        'btninsert
        '
        Me.btninsert.Font = New System.Drawing.Font("Tahoma", 13.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btninsert.Location = New System.Drawing.Point(514, 605)
        Me.btninsert.Name = "btninsert"
        Me.btninsert.Size = New System.Drawing.Size(105, 38)
        Me.btninsert.TabIndex = 6
        Me.btninsert.Text = "تسجيل"
        Me.btninsert.UseVisualStyleBackColor = True
        '
        'cmbLot
        '
        Me.cmbLot.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbLot.FormattingEnabled = True
        Me.cmbLot.Location = New System.Drawing.Point(795, 249)
        Me.cmbLot.Name = "cmbLot"
        Me.cmbLot.Size = New System.Drawing.Size(257, 32)
        Me.cmbLot.TabIndex = 7
        '
        'dgvStock
        '
        Me.dgvStock.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dgvStock.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvStock.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvStock.Location = New System.Drawing.Point(225, 298)
        Me.dgvStock.Name = "dgvStock"
        Me.dgvStock.RowHeadersWidth = 51
        Me.dgvStock.RowTemplate.Height = 26
        Me.dgvStock.Size = New System.Drawing.Size(807, 150)
        Me.dgvStock.TabIndex = 8
        '
        'cmbworder
        '
        Me.cmbworder.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbworder.FormattingEnabled = True
        Me.cmbworder.Location = New System.Drawing.Point(473, 180)
        Me.cmbworder.Name = "cmbworder"
        Me.cmbworder.Size = New System.Drawing.Size(171, 32)
        Me.cmbworder.TabIndex = 9
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(173, 214)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(129, 24)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "رقم الرسالة "
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(899, 214)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(36, 24)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "lot"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(519, 146)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(108, 24)
        Me.Label5.TabIndex = 12
        Me.Label5.Text = "أمر الشغل"
        '
        'lblbatchno
        '
        Me.lblbatchno.AutoSize = True
        Me.lblbatchno.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblbatchno.Location = New System.Drawing.Point(176, 47)
        Me.lblbatchno.Name = "lblbatchno"
        Me.lblbatchno.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblbatchno.Size = New System.Drawing.Size(60, 21)
        Me.lblbatchno.TabIndex = 13
        Me.lblbatchno.Text = "رسالة"
        '
        'lblcontractno
        '
        Me.lblcontractno.AutoSize = True
        Me.lblcontractno.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblcontractno.Location = New System.Drawing.Point(361, 47)
        Me.lblcontractno.Name = "lblcontractno"
        Me.lblcontractno.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblcontractno.Size = New System.Drawing.Size(86, 21)
        Me.lblcontractno.TabIndex = 14
        Me.lblcontractno.Text = "رقم تعاقد"
        '
        'lblqtym
        '
        Me.lblqtym.AutoSize = True
        Me.lblqtym.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblqtym.Location = New System.Drawing.Point(529, 47)
        Me.lblqtym.Name = "lblqtym"
        Me.lblqtym.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblqtym.Size = New System.Drawing.Size(79, 21)
        Me.lblqtym.TabIndex = 15
        Me.lblqtym.Text = "كمية متر"
        '
        'lblqtykg
        '
        Me.lblqtykg.AutoSize = True
        Me.lblqtykg.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblqtykg.Location = New System.Drawing.Point(762, 47)
        Me.lblqtykg.Name = "lblqtykg"
        Me.lblqtykg.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblqtykg.Size = New System.Drawing.Size(83, 21)
        Me.lblqtykg.TabIndex = 16
        Me.lblqtykg.Text = "كميه وزن"
        '
        'lblUsername
        '
        Me.lblUsername.AutoSize = True
        Me.lblUsername.Location = New System.Drawing.Point(12, 9)
        Me.lblUsername.Name = "lblUsername"
        Me.lblUsername.Size = New System.Drawing.Size(32, 16)
        Me.lblUsername.TabIndex = 19
        Me.lblUsername.Text = "user"
        '
        'txtref
        '
        Me.txtref.Location = New System.Drawing.Point(192, 500)
        Me.txtref.Name = "txtref"
        Me.txtref.Size = New System.Drawing.Size(181, 23)
        Me.txtref.TabIndex = 20
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(247, 469)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(98, 24)
        Me.Label6.TabIndex = 21
        Me.Label6.Text = "اذن صرف"
        '
        'rawdisbursementform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1224, 677)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtref)
        Me.Controls.Add(Me.lblUsername)
        Me.Controls.Add(Me.lblqtykg)
        Me.Controls.Add(Me.lblqtym)
        Me.Controls.Add(Me.lblcontractno)
        Me.Controls.Add(Me.lblbatchno)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.cmbworder)
        Me.Controls.Add(Me.dgvStock)
        Me.Controls.Add(Me.cmbLot)
        Me.Controls.Add(Me.btninsert)
        Me.Controls.Add(Me.cmbbatch)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtIssuedRolls)
        Me.Controls.Add(Me.txtIssuedWeight)
        Me.Font = New System.Drawing.Font("Tahoma", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "rawdisbursementform"
        Me.Text = "rawdisbursementform"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.dgvStock, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtIssuedWeight As TextBox
    Friend WithEvents txtIssuedRolls As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents cmbbatch As ComboBox
    Friend WithEvents btninsert As Button
    Friend WithEvents cmbLot As ComboBox
    Friend WithEvents dgvStock As DataGridView
    Friend WithEvents cmbworder As ComboBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents lblbatchno As Label
    Friend WithEvents lblcontractno As Label
    Friend WithEvents lblqtym As Label
    Friend WithEvents lblqtykg As Label
    Friend WithEvents lblUsername As Label
    Friend WithEvents txtref As TextBox
    Friend WithEvents Label6 As Label
End Class
