<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UpdateCodeOnPOForm
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
        Me.txtworderid = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lbloldcode = New System.Windows.Forms.Label()
        Me.txtreason = New System.Windows.Forms.TextBox()
        Me.btnsearch = New System.Windows.Forms.Button()
        Me.dgvLibCode = New System.Windows.Forms.DataGridView()
        Me.dgvoldcode = New System.Windows.Forms.DataGridView()
        Me.btnupdate = New System.Windows.Forms.Button()
        Me.cmbcodelib = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblidlibcode = New System.Windows.Forms.Label()
        Me.lblUsername = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        CType(Me.dgvLibCode, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvoldcode, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtworderid
        '
        Me.txtworderid.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtworderid.Location = New System.Drawing.Point(699, 12)
        Me.txtworderid.Name = "txtworderid"
        Me.txtworderid.Size = New System.Drawing.Size(206, 28)
        Me.txtworderid.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(599, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(94, 21)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Worder id"
        '
        'lbloldcode
        '
        Me.lbloldcode.AutoSize = True
        Me.lbloldcode.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbloldcode.Location = New System.Drawing.Point(1293, 101)
        Me.lbloldcode.Name = "lbloldcode"
        Me.lbloldcode.Size = New System.Drawing.Size(92, 21)
        Me.lbloldcode.TabIndex = 2
        Me.lbloldcode.Text = "Old Code "
        '
        'txtreason
        '
        Me.txtreason.Location = New System.Drawing.Point(563, 230)
        Me.txtreason.Multiline = True
        Me.txtreason.Name = "txtreason"
        Me.txtreason.Size = New System.Drawing.Size(482, 135)
        Me.txtreason.TabIndex = 3
        '
        'btnsearch
        '
        Me.btnsearch.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnsearch.Location = New System.Drawing.Point(761, 94)
        Me.btnsearch.Name = "btnsearch"
        Me.btnsearch.Size = New System.Drawing.Size(104, 33)
        Me.btnsearch.TabIndex = 4
        Me.btnsearch.Text = "Search"
        Me.btnsearch.UseVisualStyleBackColor = True
        '
        'dgvLibCode
        '
        Me.dgvLibCode.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dgvLibCode.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvLibCode.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvLibCode.Location = New System.Drawing.Point(12, 166)
        Me.dgvLibCode.Name = "dgvLibCode"
        Me.dgvLibCode.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.dgvLibCode.RowTemplate.Height = 26
        Me.dgvLibCode.Size = New System.Drawing.Size(545, 637)
        Me.dgvLibCode.TabIndex = 5
        '
        'dgvoldcode
        '
        Me.dgvoldcode.AllowUserToAddRows = False
        Me.dgvoldcode.AllowUserToDeleteRows = False
        Me.dgvoldcode.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dgvoldcode.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvoldcode.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvoldcode.Location = New System.Drawing.Point(1065, 150)
        Me.dgvoldcode.Name = "dgvoldcode"
        Me.dgvoldcode.ReadOnly = True
        Me.dgvoldcode.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.dgvoldcode.RowTemplate.Height = 26
        Me.dgvoldcode.Size = New System.Drawing.Size(509, 653)
        Me.dgvoldcode.TabIndex = 6
        '
        'btnupdate
        '
        Me.btnupdate.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnupdate.Location = New System.Drawing.Point(673, 526)
        Me.btnupdate.Name = "btnupdate"
        Me.btnupdate.Size = New System.Drawing.Size(232, 76)
        Me.btnupdate.TabIndex = 7
        Me.btnupdate.Text = "Update Code"
        Me.btnupdate.UseVisualStyleBackColor = True
        '
        'cmbcodelib
        '
        Me.cmbcodelib.FormattingEnabled = True
        Me.cmbcodelib.Location = New System.Drawing.Point(112, 101)
        Me.cmbcodelib.Name = "cmbcodelib"
        Me.cmbcodelib.Size = New System.Drawing.Size(257, 24)
        Me.cmbcodelib.TabIndex = 8
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(143, 60)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(111, 24)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "New Code"
        '
        'lblidlibcode
        '
        Me.lblidlibcode.AutoSize = True
        Me.lblidlibcode.Font = New System.Drawing.Font("Tahoma", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblidlibcode.Location = New System.Drawing.Point(217, 44)
        Me.lblidlibcode.Name = "lblidlibcode"
        Me.lblidlibcode.Size = New System.Drawing.Size(25, 17)
        Me.lblidlibcode.TabIndex = 9
        Me.lblidlibcode.Text = "ID"
        '
        'lblUsername
        '
        Me.lblUsername.AutoSize = True
        Me.lblUsername.Location = New System.Drawing.Point(13, 12)
        Me.lblUsername.Name = "lblUsername"
        Me.lblUsername.Size = New System.Drawing.Size(35, 17)
        Me.lblUsername.TabIndex = 10
        Me.lblUsername.Text = "User"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(748, 192)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(126, 24)
        Me.Label3.TabIndex = 11
        Me.Label3.Text = "سبب التغيير"
        '
        'UpdateCodeOnPOForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(1599, 831)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.lblUsername)
        Me.Controls.Add(Me.lblidlibcode)
        Me.Controls.Add(Me.cmbcodelib)
        Me.Controls.Add(Me.btnupdate)
        Me.Controls.Add(Me.dgvoldcode)
        Me.Controls.Add(Me.dgvLibCode)
        Me.Controls.Add(Me.btnsearch)
        Me.Controls.Add(Me.txtreason)
        Me.Controls.Add(Me.lbloldcode)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtworderid)
        Me.Name = "UpdateCodeOnPOForm"
        Me.Text = "Update Code Library On Worder"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.dgvLibCode, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvoldcode, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtworderid As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lbloldcode As System.Windows.Forms.Label
    Friend WithEvents txtreason As System.Windows.Forms.TextBox
    Friend WithEvents btnsearch As System.Windows.Forms.Button
    Friend WithEvents dgvLibCode As System.Windows.Forms.DataGridView
    Friend WithEvents dgvoldcode As System.Windows.Forms.DataGridView
    Friend WithEvents btnupdate As System.Windows.Forms.Button
    Friend WithEvents cmbcodelib As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblidlibcode As System.Windows.Forms.Label
    Friend WithEvents lblUsername As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
End Class
