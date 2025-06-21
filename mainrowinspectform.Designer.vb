<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class mainrowinspectform
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
        Me.btnprint2 = New System.Windows.Forms.Button()
        Me.lblUsername = New System.Windows.Forms.Label()
        Me.lbltotalpoints = New System.Windows.Forms.Label()
        Me.dataGridViewDetails = New System.Windows.Forms.DataGridView()
        Me.lbltotalw = New System.Windows.Forms.Label()
        Me.lbltotalm = New System.Windows.Forms.Label()
        Me.btnaddroll = New System.Windows.Forms.Button()
        Me.btnreport = New System.Windows.Forms.Button()
        Me.lblcolor = New System.Windows.Forms.Label()
        Me.lblclient = New System.Windows.Forms.Label()
        Me.lblqtym = New System.Windows.Forms.Label()
        Me.lblqtykg = New System.Windows.Forms.Label()
        Me.lblcontractno = New System.Windows.Forms.Label()
        Me.lblbatchno = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cmbworder = New System.Windows.Forms.ComboBox()
        Me.lblmaterial = New System.Windows.Forms.Label()
        CType(Me.dataGridViewDetails, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnprint2
        '
        Me.btnprint2.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnprint2.Location = New System.Drawing.Point(745, 288)
        Me.btnprint2.Name = "btnprint2"
        Me.btnprint2.Size = New System.Drawing.Size(147, 58)
        Me.btnprint2.TabIndex = 72
        Me.btnprint2.Text = "تقرير فحص الخام"
        Me.btnprint2.UseVisualStyleBackColor = True
        '
        'lblUsername
        '
        Me.lblUsername.AutoSize = True
        Me.lblUsername.Location = New System.Drawing.Point(2, 161)
        Me.lblUsername.Name = "lblUsername"
        Me.lblUsername.Size = New System.Drawing.Size(34, 17)
        Me.lblUsername.TabIndex = 71
        Me.lblUsername.Text = "user"
        '
        'lbltotalpoints
        '
        Me.lbltotalpoints.AutoSize = True
        Me.lbltotalpoints.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbltotalpoints.Location = New System.Drawing.Point(562, 388)
        Me.lbltotalpoints.Name = "lbltotalpoints"
        Me.lbltotalpoints.Size = New System.Drawing.Size(128, 24)
        Me.lbltotalpoints.TabIndex = 70
        Me.lbltotalpoints.Text = "Total Points"
        '
        'dataGridViewDetails
        '
        Me.dataGridViewDetails.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dataGridViewDetails.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dataGridViewDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dataGridViewDetails.Location = New System.Drawing.Point(58, 426)
        Me.dataGridViewDetails.Name = "dataGridViewDetails"
        Me.dataGridViewDetails.RowHeadersWidth = 51
        Me.dataGridViewDetails.RowTemplate.Height = 26
        Me.dataGridViewDetails.Size = New System.Drawing.Size(1223, 412)
        Me.dataGridViewDetails.TabIndex = 69
        '
        'lbltotalw
        '
        Me.lbltotalw.AutoSize = True
        Me.lbltotalw.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbltotalw.Location = New System.Drawing.Point(423, 222)
        Me.lbltotalw.Name = "lbltotalw"
        Me.lbltotalw.Size = New System.Drawing.Size(15, 21)
        Me.lbltotalw.TabIndex = 68
        Me.lbltotalw.Text = "."
        '
        'lbltotalm
        '
        Me.lbltotalm.AutoSize = True
        Me.lbltotalm.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbltotalm.Location = New System.Drawing.Point(685, 222)
        Me.lbltotalm.Name = "lbltotalm"
        Me.lbltotalm.Size = New System.Drawing.Size(15, 21)
        Me.lbltotalm.TabIndex = 67
        Me.lbltotalm.Text = "."
        '
        'btnaddroll
        '
        Me.btnaddroll.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnaddroll.Location = New System.Drawing.Point(733, 136)
        Me.btnaddroll.Name = "btnaddroll"
        Me.btnaddroll.Size = New System.Drawing.Size(159, 42)
        Me.btnaddroll.TabIndex = 66
        Me.btnaddroll.Text = "اضافه توب جديد"
        Me.btnaddroll.UseVisualStyleBackColor = True
        '
        'btnreport
        '
        Me.btnreport.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnreport.Location = New System.Drawing.Point(500, 288)
        Me.btnreport.Name = "btnreport"
        Me.btnreport.Size = New System.Drawing.Size(161, 58)
        Me.btnreport.TabIndex = 65
        Me.btnreport.Text = "تقرير تسليم مخزن"
        Me.btnreport.UseVisualStyleBackColor = True
        '
        'lblcolor
        '
        Me.lblcolor.AutoSize = True
        Me.lblcolor.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblcolor.Location = New System.Drawing.Point(244, 67)
        Me.lblcolor.Name = "lblcolor"
        Me.lblcolor.Size = New System.Drawing.Size(15, 21)
        Me.lblcolor.TabIndex = 60
        Me.lblcolor.Text = "."
        '
        'lblclient
        '
        Me.lblclient.AutoSize = True
        Me.lblclient.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblclient.Location = New System.Drawing.Point(248, 6)
        Me.lblclient.Name = "lblclient"
        Me.lblclient.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblclient.Size = New System.Drawing.Size(15, 21)
        Me.lblclient.TabIndex = 59
        Me.lblclient.Text = "."
        '
        'lblqtym
        '
        Me.lblqtym.AutoSize = True
        Me.lblqtym.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblqtym.Location = New System.Drawing.Point(765, 52)
        Me.lblqtym.Name = "lblqtym"
        Me.lblqtym.Size = New System.Drawing.Size(15, 21)
        Me.lblqtym.TabIndex = 58
        Me.lblqtym.Text = "."
        '
        'lblqtykg
        '
        Me.lblqtykg.AutoSize = True
        Me.lblqtykg.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblqtykg.Location = New System.Drawing.Point(514, 52)
        Me.lblqtykg.Name = "lblqtykg"
        Me.lblqtykg.Size = New System.Drawing.Size(15, 21)
        Me.lblqtykg.TabIndex = 57
        Me.lblqtykg.Text = "."
        '
        'lblcontractno
        '
        Me.lblcontractno.AutoSize = True
        Me.lblcontractno.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblcontractno.Location = New System.Drawing.Point(768, 6)
        Me.lblcontractno.Name = "lblcontractno"
        Me.lblcontractno.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblcontractno.Size = New System.Drawing.Size(15, 21)
        Me.lblcontractno.TabIndex = 56
        Me.lblcontractno.Text = "."
        '
        'lblbatchno
        '
        Me.lblbatchno.AutoSize = True
        Me.lblbatchno.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblbatchno.Location = New System.Drawing.Point(514, 3)
        Me.lblbatchno.Name = "lblbatchno"
        Me.lblbatchno.Size = New System.Drawing.Size(15, 21)
        Me.lblbatchno.TabIndex = 55
        Me.lblbatchno.Text = "."
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(496, 120)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(100, 24)
        Me.Label5.TabIndex = 54
        Me.Label5.Text = "أمر شغل "
        '
        'cmbworder
        '
        Me.cmbworder.Font = New System.Drawing.Font("Tahoma", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbworder.FormattingEnabled = True
        Me.cmbworder.Location = New System.Drawing.Point(447, 161)
        Me.cmbworder.Name = "cmbworder"
        Me.cmbworder.Size = New System.Drawing.Size(214, 30)
        Me.cmbworder.TabIndex = 53
        '
        'lblmaterial
        '
        Me.lblmaterial.AutoSize = True
        Me.lblmaterial.Location = New System.Drawing.Point(248, 120)
        Me.lblmaterial.Name = "lblmaterial"
        Me.lblmaterial.Size = New System.Drawing.Size(12, 17)
        Me.lblmaterial.TabIndex = 73
        Me.lblmaterial.Text = "."
        '
        'mainrowinspectform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1377, 987)
        Me.Controls.Add(Me.lblmaterial)
        Me.Controls.Add(Me.btnprint2)
        Me.Controls.Add(Me.lblUsername)
        Me.Controls.Add(Me.lbltotalpoints)
        Me.Controls.Add(Me.dataGridViewDetails)
        Me.Controls.Add(Me.lbltotalw)
        Me.Controls.Add(Me.lbltotalm)
        Me.Controls.Add(Me.btnaddroll)
        Me.Controls.Add(Me.btnreport)
        Me.Controls.Add(Me.lblcolor)
        Me.Controls.Add(Me.lblclient)
        Me.Controls.Add(Me.lblqtym)
        Me.Controls.Add(Me.lblqtykg)
        Me.Controls.Add(Me.lblcontractno)
        Me.Controls.Add(Me.lblbatchno)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.cmbworder)
        Me.Name = "mainrowinspectform"
        Me.Text = "mainrowinspectform"
        CType(Me.dataGridViewDetails, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnprint2 As Button
    Friend WithEvents lblUsername As Label
    Friend WithEvents lbltotalpoints As Label
    Friend WithEvents dataGridViewDetails As DataGridView
    Friend WithEvents lbltotalw As Label
    Friend WithEvents lbltotalm As Label
    Friend WithEvents btnaddroll As Button
    Friend WithEvents btnreport As Button
    Friend WithEvents lblcolor As Label
    Friend WithEvents lblclient As Label
    Friend WithEvents lblqtym As Label
    Friend WithEvents lblqtykg As Label
    Friend WithEvents lblcontractno As Label
    Friend WithEvents lblbatchno As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents cmbworder As ComboBox
    Friend WithEvents lblmaterial As Label
End Class
