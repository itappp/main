<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class recievedrawform
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
        Me.lblstylecode = New System.Windows.Forms.Label()
        Me.cmbpo = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbbatch = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.dgvdetailsbatch = New System.Windows.Forms.DataGridView()
        Me.btninsert = New System.Windows.Forms.Button()
        Me.lblUsername = New System.Windows.Forms.Label()
        Me.dgvdetailsbatch1 = New System.Windows.Forms.DataGridView()
        Me.lblbatch = New System.Windows.Forms.Label()
        Me.lblclient = New System.Windows.Forms.Label()
        CType(Me.dgvdetailsbatch, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvdetailsbatch1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblstylecode
        '
        Me.lblstylecode.AutoSize = True
        Me.lblstylecode.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblstylecode.Location = New System.Drawing.Point(446, 9)
        Me.lblstylecode.Name = "lblstylecode"
        Me.lblstylecode.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblstylecode.Size = New System.Drawing.Size(84, 21)
        Me.lblstylecode.TabIndex = 3
        Me.lblstylecode.Text = "كود الخامه"
        '
        'cmbpo
        '
        Me.cmbpo.FormattingEnabled = True
        Me.cmbpo.Location = New System.Drawing.Point(199, 152)
        Me.cmbpo.Name = "cmbpo"
        Me.cmbpo.Size = New System.Drawing.Size(137, 24)
        Me.cmbpo.TabIndex = 4
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(143, 152)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(38, 24)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "PO"
        '
        'cmbbatch
        '
        Me.cmbbatch.FormattingEnabled = True
        Me.cmbbatch.Location = New System.Drawing.Point(674, 156)
        Me.cmbbatch.Name = "cmbbatch"
        Me.cmbbatch.Size = New System.Drawing.Size(169, 24)
        Me.cmbbatch.TabIndex = 6
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(557, 156)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(68, 24)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Batch"
        '
        'btnSearch
        '
        Me.btnSearch.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSearch.Location = New System.Drawing.Point(24, 197)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(130, 38)
        Me.btnSearch.TabIndex = 8
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'dgvdetailsbatch
        '
        Me.dgvdetailsbatch.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dgvdetailsbatch.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvdetailsbatch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvdetailsbatch.Location = New System.Drawing.Point(24, 241)
        Me.dgvdetailsbatch.Name = "dgvdetailsbatch"
        Me.dgvdetailsbatch.RowHeadersWidth = 51
        Me.dgvdetailsbatch.RowTemplate.Height = 26
        Me.dgvdetailsbatch.Size = New System.Drawing.Size(1188, 313)
        Me.dgvdetailsbatch.TabIndex = 9
        '
        'btninsert
        '
        Me.btninsert.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btninsert.Location = New System.Drawing.Point(1014, 197)
        Me.btninsert.Name = "btninsert"
        Me.btninsert.Size = New System.Drawing.Size(137, 38)
        Me.btninsert.TabIndex = 11
        Me.btninsert.Text = "تسجيل"
        Me.btninsert.UseVisualStyleBackColor = True
        '
        'lblUsername
        '
        Me.lblUsername.AutoSize = True
        Me.lblUsername.Location = New System.Drawing.Point(1117, 152)
        Me.lblUsername.Name = "lblUsername"
        Me.lblUsername.Size = New System.Drawing.Size(34, 17)
        Me.lblUsername.TabIndex = 19
        Me.lblUsername.Text = "user"
        '
        'dgvdetailsbatch1
        '
        Me.dgvdetailsbatch1.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dgvdetailsbatch1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvdetailsbatch1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvdetailsbatch1.Location = New System.Drawing.Point(12, 3)
        Me.dgvdetailsbatch1.Name = "dgvdetailsbatch1"
        Me.dgvdetailsbatch1.RowHeadersWidth = 51
        Me.dgvdetailsbatch1.RowTemplate.Height = 26
        Me.dgvdetailsbatch1.Size = New System.Drawing.Size(1187, 91)
        Me.dgvdetailsbatch1.TabIndex = 20
        '
        'lblbatch
        '
        Me.lblbatch.AutoSize = True
        Me.lblbatch.Location = New System.Drawing.Point(962, 108)
        Me.lblbatch.Name = "lblbatch"
        Me.lblbatch.Size = New System.Drawing.Size(47, 17)
        Me.lblbatch.TabIndex = 21
        Me.lblbatch.Text = "Label3"
        '
        'lblclient
        '
        Me.lblclient.AutoSize = True
        Me.lblclient.Location = New System.Drawing.Point(637, 108)
        Me.lblclient.Name = "lblclient"
        Me.lblclient.Size = New System.Drawing.Size(47, 17)
        Me.lblclient.TabIndex = 22
        Me.lblclient.Text = "Label3"
        '
        'recievedrawform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1224, 678)
        Me.Controls.Add(Me.lblclient)
        Me.Controls.Add(Me.lblbatch)
        Me.Controls.Add(Me.dgvdetailsbatch1)
        Me.Controls.Add(Me.lblUsername)
        Me.Controls.Add(Me.btninsert)
        Me.Controls.Add(Me.dgvdetailsbatch)
        Me.Controls.Add(Me.btnSearch)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.cmbbatch)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmbpo)
        Me.Controls.Add(Me.lblstylecode)
        Me.Name = "recievedrawform"
        Me.Text = "recievedrawform"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.dgvdetailsbatch, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvdetailsbatch1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblstylecode As Label
    Friend WithEvents cmbpo As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents cmbbatch As ComboBox
    Friend WithEvents Label2 As Label
    Friend WithEvents btnSearch As Button
    Friend WithEvents dgvdetailsbatch As DataGridView
    Friend WithEvents btninsert As Button
    Friend WithEvents lblUsername As Label
    Friend WithEvents dgvdetailsbatch1 As DataGridView
    Friend WithEvents lblbatch As Label
    Friend WithEvents lblclient As Label
End Class
