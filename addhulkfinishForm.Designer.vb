<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class addhulkfinishForm
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
        Me.txtm = New System.Windows.Forms.TextBox()
        Me.txtkg = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btninsert = New System.Windows.Forms.Button()
        Me.txtworderid = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblUsername = New System.Windows.Forms.Label()
        Me.btnRetrieve = New System.Windows.Forms.Button()
        Me.lblContractNo = New System.Windows.Forms.Label()
        Me.lblRefNo = New System.Windows.Forms.Label()
        Me.lblBatchNo = New System.Windows.Forms.Label()
        Me.lblClientCode = New System.Windows.Forms.Label()
        Me.lblColor = New System.Windows.Forms.Label()
        Me.lblProductName = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'txtm
        '
        Me.txtm.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtm.Location = New System.Drawing.Point(216, 293)
        Me.txtm.Name = "txtm"
        Me.txtm.Size = New System.Drawing.Size(146, 28)
        Me.txtm.TabIndex = 0
        '
        'txtkg
        '
        Me.txtkg.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtkg.Location = New System.Drawing.Point(475, 293)
        Me.txtkg.Name = "txtkg"
        Me.txtkg.Size = New System.Drawing.Size(146, 28)
        Me.txtkg.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(261, 252)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(40, 24)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "متر"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(526, 252)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(46, 24)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "وزن"
        '
        'btninsert
        '
        Me.btninsert.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btninsert.Location = New System.Drawing.Point(324, 391)
        Me.btninsert.Name = "btninsert"
        Me.btninsert.Size = New System.Drawing.Size(157, 56)
        Me.btninsert.TabIndex = 4
        Me.btninsert.Text = "تسجيل"
        Me.btninsert.UseVisualStyleBackColor = True
        '
        'txtworderid
        '
        Me.txtworderid.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtworderid.Location = New System.Drawing.Point(206, 67)
        Me.txtworderid.Name = "txtworderid"
        Me.txtworderid.Size = New System.Drawing.Size(173, 28)
        Me.txtworderid.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(247, 28)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(94, 24)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "أمر شغل"
        '
        'lblUsername
        '
        Me.lblUsername.AutoSize = True
        Me.lblUsername.Location = New System.Drawing.Point(52, 23)
        Me.lblUsername.Name = "lblUsername"
        Me.lblUsername.Size = New System.Drawing.Size(35, 17)
        Me.lblUsername.TabIndex = 7
        Me.lblUsername.Text = "User"
        '
        'btnRetrieve
        '
        Me.btnRetrieve.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRetrieve.Location = New System.Drawing.Point(234, 119)
        Me.btnRetrieve.Name = "btnRetrieve"
        Me.btnRetrieve.Size = New System.Drawing.Size(128, 45)
        Me.btnRetrieve.TabIndex = 8
        Me.btnRetrieve.Text = "Search"
        Me.btnRetrieve.UseVisualStyleBackColor = True
        '
        'lblContractNo
        '
        Me.lblContractNo.AutoSize = True
        Me.lblContractNo.Location = New System.Drawing.Point(516, 23)
        Me.lblContractNo.Name = "lblContractNo"
        Me.lblContractNo.Size = New System.Drawing.Size(47, 17)
        Me.lblContractNo.TabIndex = 9
        Me.lblContractNo.Text = "Label4"
        '
        'lblRefNo
        '
        Me.lblRefNo.AutoSize = True
        Me.lblRefNo.Location = New System.Drawing.Point(683, 23)
        Me.lblRefNo.Name = "lblRefNo"
        Me.lblRefNo.Size = New System.Drawing.Size(47, 17)
        Me.lblRefNo.TabIndex = 10
        Me.lblRefNo.Text = "Label5"
        '
        'lblBatchNo
        '
        Me.lblBatchNo.AutoSize = True
        Me.lblBatchNo.Location = New System.Drawing.Point(816, 23)
        Me.lblBatchNo.Name = "lblBatchNo"
        Me.lblBatchNo.Size = New System.Drawing.Size(47, 17)
        Me.lblBatchNo.TabIndex = 11
        Me.lblBatchNo.Text = "Label6"
        '
        'lblClientCode
        '
        Me.lblClientCode.AutoSize = True
        Me.lblClientCode.Location = New System.Drawing.Point(956, 23)
        Me.lblClientCode.Name = "lblClientCode"
        Me.lblClientCode.Size = New System.Drawing.Size(47, 17)
        Me.lblClientCode.TabIndex = 12
        Me.lblClientCode.Text = "Label7"
        '
        'lblColor
        '
        Me.lblColor.AutoSize = True
        Me.lblColor.Location = New System.Drawing.Point(494, 110)
        Me.lblColor.Name = "lblColor"
        Me.lblColor.Size = New System.Drawing.Size(47, 17)
        Me.lblColor.TabIndex = 13
        Me.lblColor.Text = "Label4"
        '
        'lblProductName
        '
        Me.lblProductName.AutoSize = True
        Me.lblProductName.Location = New System.Drawing.Point(668, 110)
        Me.lblProductName.Name = "lblProductName"
        Me.lblProductName.Size = New System.Drawing.Size(47, 17)
        Me.lblProductName.TabIndex = 14
        Me.lblProductName.Text = "Label5"
        '
        'addhulkfinishForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1159, 474)
        Me.Controls.Add(Me.lblProductName)
        Me.Controls.Add(Me.lblColor)
        Me.Controls.Add(Me.lblClientCode)
        Me.Controls.Add(Me.lblBatchNo)
        Me.Controls.Add(Me.lblRefNo)
        Me.Controls.Add(Me.lblContractNo)
        Me.Controls.Add(Me.btnRetrieve)
        Me.Controls.Add(Me.lblUsername)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtworderid)
        Me.Controls.Add(Me.btninsert)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtkg)
        Me.Controls.Add(Me.txtm)
        Me.Name = "addhulkfinishForm"
        Me.Text = "استلام هالك المجهز"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtm As System.Windows.Forms.TextBox
    Friend WithEvents txtkg As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btninsert As System.Windows.Forms.Button
    Friend WithEvents txtworderid As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lblUsername As System.Windows.Forms.Label
    Friend WithEvents btnRetrieve As System.Windows.Forms.Button
    Friend WithEvents lblContractNo As System.Windows.Forms.Label
    Friend WithEvents lblRefNo As System.Windows.Forms.Label
    Friend WithEvents lblBatchNo As System.Windows.Forms.Label
    Friend WithEvents lblClientCode As System.Windows.Forms.Label
    Friend WithEvents lblColor As System.Windows.Forms.Label
    Friend WithEvents lblProductName As System.Windows.Forms.Label
End Class
