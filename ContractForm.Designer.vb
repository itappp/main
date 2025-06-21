<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ContractForm
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
        Me.txtcontractno = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbcontracttype = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.dtpContract = New System.Windows.Forms.DateTimePicker()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cmbclientcode = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.btninsert = New System.Windows.Forms.Button()
        Me.dgvContractDetails = New System.Windows.Forms.DataGridView()
        Me.btnView = New System.Windows.Forms.Button()
        Me.lbllastcontractno = New System.Windows.Forms.Label()
        Me.lblUsername = New System.Windows.Forms.Label()
        Me.dgvqc = New System.Windows.Forms.DataGridView()
        Me.Label4 = New System.Windows.Forms.Label()
        CType(Me.dgvContractDetails, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvqc, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtcontractno
        '
        Me.txtcontractno.Location = New System.Drawing.Point(514, 47)
        Me.txtcontractno.Margin = New System.Windows.Forms.Padding(4)
        Me.txtcontractno.Multiline = True
        Me.txtcontractno.Name = "txtcontractno"
        Me.txtcontractno.Size = New System.Drawing.Size(193, 46)
        Me.txtcontractno.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(541, 11)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(116, 22)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Contract no"
        '
        'cmbcontracttype
        '
        Me.cmbcontracttype.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbcontracttype.FormattingEnabled = True
        Me.cmbcontracttype.Location = New System.Drawing.Point(993, 166)
        Me.cmbcontracttype.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbcontracttype.Name = "cmbcontracttype"
        Me.cmbcontracttype.Size = New System.Drawing.Size(201, 33)
        Me.cmbcontracttype.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(1213, 166)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(86, 22)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "نوع التعاقد"
        '
        'dtpContract
        '
        Me.dtpContract.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpContract.Location = New System.Drawing.Point(993, 230)
        Me.dtpContract.Margin = New System.Windows.Forms.Padding(4)
        Me.dtpContract.Name = "dtpContract"
        Me.dtpContract.Size = New System.Drawing.Size(201, 27)
        Me.dtpContract.TabIndex = 4
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(1213, 230)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(109, 22)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "تاريخ التسليم"
        '
        'cmbclientcode
        '
        Me.cmbclientcode.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbclientcode.FormattingEnabled = True
        Me.cmbclientcode.Location = New System.Drawing.Point(993, 104)
        Me.cmbclientcode.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbclientcode.Name = "cmbclientcode"
        Me.cmbclientcode.Size = New System.Drawing.Size(201, 28)
        Me.cmbclientcode.TabIndex = 9
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Tahoma", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(1205, 104)
        Me.Label5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(94, 22)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "كود العميـل"
        '
        'btninsert
        '
        Me.btninsert.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btninsert.Location = New System.Drawing.Point(676, 616)
        Me.btninsert.Margin = New System.Windows.Forms.Padding(4)
        Me.btninsert.Name = "btninsert"
        Me.btninsert.Size = New System.Drawing.Size(176, 70)
        Me.btninsert.TabIndex = 17
        Me.btninsert.Text = "تسجيل البيانات"
        Me.btninsert.UseVisualStyleBackColor = True
        '
        'dgvContractDetails
        '
        Me.dgvContractDetails.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dgvContractDetails.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvContractDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvContractDetails.Location = New System.Drawing.Point(32, 280)
        Me.dgvContractDetails.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvContractDetails.Name = "dgvContractDetails"
        Me.dgvContractDetails.RowHeadersWidth = 51
        Me.dgvContractDetails.RowTemplate.Height = 26
        Me.dgvContractDetails.Size = New System.Drawing.Size(1421, 241)
        Me.dgvContractDetails.TabIndex = 6
        '
        'btnView
        '
        Me.btnView.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnView.Location = New System.Drawing.Point(32, 68)
        Me.btnView.Name = "btnView"
        Me.btnView.Size = New System.Drawing.Size(97, 39)
        Me.btnView.TabIndex = 20
        Me.btnView.Text = "View"
        Me.btnView.UseVisualStyleBackColor = True
        '
        'lbllastcontractno
        '
        Me.lbllastcontractno.AutoSize = True
        Me.lbllastcontractno.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbllastcontractno.Location = New System.Drawing.Point(813, 25)
        Me.lbllastcontractno.Name = "lbllastcontractno"
        Me.lbllastcontractno.Size = New System.Drawing.Size(125, 20)
        Me.lbllastcontractno.TabIndex = 21
        Me.lbllastcontractno.Text = "رقم التعاقد السابق"
        '
        'lblUsername
        '
        Me.lblUsername.AutoSize = True
        Me.lblUsername.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUsername.Location = New System.Drawing.Point(44, 25)
        Me.lblUsername.Name = "lblUsername"
        Me.lblUsername.Size = New System.Drawing.Size(49, 20)
        Me.lblUsername.TabIndex = 22
        Me.lblUsername.Text = "User"
        '
        'dgvqc
        '
        Me.dgvqc.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dgvqc.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvqc.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvqc.Location = New System.Drawing.Point(24, 144)
        Me.dgvqc.Name = "dgvqc"
        Me.dgvqc.RowHeadersWidth = 51
        Me.dgvqc.RowTemplate.Height = 26
        Me.dgvqc.Size = New System.Drawing.Size(944, 129)
        Me.dgvqc.TabIndex = 23
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(294, 97)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(153, 25)
        Me.Label4.TabIndex = 24
        Me.Label4.Text = "بيانات معمل الجوده"
        '
        'ContractForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 18.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1479, 829)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.dgvqc)
        Me.Controls.Add(Me.lblUsername)
        Me.Controls.Add(Me.lbllastcontractno)
        Me.Controls.Add(Me.btnView)
        Me.Controls.Add(Me.btninsert)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.cmbclientcode)
        Me.Controls.Add(Me.dgvContractDetails)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.dtpContract)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.cmbcontracttype)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtcontractno)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "ContractForm"
        Me.Text = "ContractForm"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.dgvContractDetails, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvqc, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtcontractno As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbcontracttype As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents dtpContract As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cmbclientcode As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents btninsert As System.Windows.Forms.Button
    Friend WithEvents dgvContractDetails As System.Windows.Forms.DataGridView
    Friend WithEvents btnView As System.Windows.Forms.Button
    Friend WithEvents lbllastcontractno As System.Windows.Forms.Label
    Friend WithEvents lblUsername As System.Windows.Forms.Label
    Friend WithEvents dgvqc As DataGridView
    Friend WithEvents Label4 As Label
End Class
