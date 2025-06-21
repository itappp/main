<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class updateqtyworderform
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
        Me.dgvoldqty = New System.Windows.Forms.DataGridView()
        Me.txtReason = New System.Windows.Forms.TextBox()
        Me.txtworderid = New System.Windows.Forms.TextBox()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.btnupdate = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblUsername = New System.Windows.Forms.Label()
        Me.txtNewQtyM = New System.Windows.Forms.TextBox()
        Me.txtNewQtyKg = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        CType(Me.dgvoldqty, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvoldqty
        '
        Me.dgvoldqty.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dgvoldqty.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvoldqty.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvoldqty.Location = New System.Drawing.Point(586, 121)
        Me.dgvoldqty.Name = "dgvoldqty"
        Me.dgvoldqty.ReadOnly = True
        Me.dgvoldqty.RowTemplate.Height = 26
        Me.dgvoldqty.Size = New System.Drawing.Size(505, 137)
        Me.dgvoldqty.TabIndex = 0
        '
        'txtReason
        '
        Me.txtReason.Location = New System.Drawing.Point(586, 343)
        Me.txtReason.Multiline = True
        Me.txtReason.Name = "txtReason"
        Me.txtReason.Size = New System.Drawing.Size(419, 166)
        Me.txtReason.TabIndex = 1
        '
        'txtworderid
        '
        Me.txtworderid.Location = New System.Drawing.Point(153, 122)
        Me.txtworderid.Name = "txtworderid"
        Me.txtworderid.Size = New System.Drawing.Size(240, 24)
        Me.txtworderid.TabIndex = 2
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(199, 180)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(137, 48)
        Me.btnSearch.TabIndex = 3
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnupdate
        '
        Me.btnupdate.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnupdate.Location = New System.Drawing.Point(731, 650)
        Me.btnupdate.Name = "btnupdate"
        Me.btnupdate.Size = New System.Drawing.Size(137, 48)
        Me.btnupdate.TabIndex = 4
        Me.btnupdate.Text = "Update"
        Me.btnupdate.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(736, 293)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(64, 21)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "السبب"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(41, 121)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(82, 21)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "أمر شغل"
        '
        'lblUsername
        '
        Me.lblUsername.AutoSize = True
        Me.lblUsername.Location = New System.Drawing.Point(45, 13)
        Me.lblUsername.Name = "lblUsername"
        Me.lblUsername.Size = New System.Drawing.Size(34, 17)
        Me.lblUsername.TabIndex = 7
        Me.lblUsername.Text = "user"
        '
        'txtNewQtyM
        '
        Me.txtNewQtyM.Location = New System.Drawing.Point(532, 588)
        Me.txtNewQtyM.Name = "txtNewQtyM"
        Me.txtNewQtyM.Size = New System.Drawing.Size(133, 24)
        Me.txtNewQtyM.TabIndex = 8
        '
        'txtNewQtyKg
        '
        Me.txtNewQtyKg.Location = New System.Drawing.Point(920, 588)
        Me.txtNewQtyKg.Name = "txtNewQtyKg"
        Me.txtNewQtyKg.Size = New System.Drawing.Size(124, 24)
        Me.txtNewQtyKg.TabIndex = 9
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(575, 551)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(40, 24)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "متر"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(955, 551)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(46, 24)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "وزن"
        '
        'updateqtyworderform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1125, 842)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtNewQtyKg)
        Me.Controls.Add(Me.txtNewQtyM)
        Me.Controls.Add(Me.lblUsername)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnupdate)
        Me.Controls.Add(Me.btnSearch)
        Me.Controls.Add(Me.txtworderid)
        Me.Controls.Add(Me.txtReason)
        Me.Controls.Add(Me.dgvoldqty)
        Me.Name = "updateqtyworderform"
        Me.Text = "updateqtyworderform"
        CType(Me.dgvoldqty, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dgvoldqty As System.Windows.Forms.DataGridView
    Friend WithEvents txtReason As System.Windows.Forms.TextBox
    Friend WithEvents txtworderid As System.Windows.Forms.TextBox
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnupdate As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblUsername As System.Windows.Forms.Label
    Friend WithEvents txtNewQtyM As System.Windows.Forms.TextBox
    Friend WithEvents txtNewQtyKg As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
End Class
