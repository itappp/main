<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class mainfinishinspectform
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
        Me.lblcolor = New System.Windows.Forms.Label()
        Me.lblclient = New System.Windows.Forms.Label()
        Me.lblqtym = New System.Windows.Forms.Label()
        Me.lblqtykg = New System.Windows.Forms.Label()
        Me.lblcontractno = New System.Windows.Forms.Label()
        Me.lblbatchno = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cmbworder = New System.Windows.Forms.ComboBox()
        Me.btnreport = New System.Windows.Forms.Button()
        Me.lblkg = New System.Windows.Forms.Label()
        Me.lblm = New System.Windows.Forms.Label()
        Me.cmbroll = New System.Windows.Forms.ComboBox()
        Me.btnprint = New System.Windows.Forms.Button()
        Me.btnaddroll = New System.Windows.Forms.Button()
        Me.lbltotalw = New System.Windows.Forms.Label()
        Me.lbltotalm = New System.Windows.Forms.Label()
        Me.dataGridViewDetails = New System.Windows.Forms.DataGridView()
        Me.lbltotalpoints = New System.Windows.Forms.Label()
        Me.lblUsername = New System.Windows.Forms.Label()
        Me.btnprint2 = New System.Windows.Forms.Button()
        Me.btnrollnd = New System.Windows.Forms.Button()
        Me.lblmaterial = New System.Windows.Forms.Label()
        Me.btnprintall = New System.Windows.Forms.Button()
        CType(Me.dataGridViewDetails, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblcolor
        '
        Me.lblcolor.AutoSize = True
        Me.lblcolor.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblcolor.Location = New System.Drawing.Point(291, 70)
        Me.lblcolor.Name = "lblcolor"
        Me.lblcolor.Size = New System.Drawing.Size(15, 21)
        Me.lblcolor.TabIndex = 39
        Me.lblcolor.Text = "."
        '
        'lblclient
        '
        Me.lblclient.AutoSize = True
        Me.lblclient.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblclient.Location = New System.Drawing.Point(296, 9)
        Me.lblclient.Name = "lblclient"
        Me.lblclient.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblclient.Size = New System.Drawing.Size(15, 21)
        Me.lblclient.TabIndex = 38
        Me.lblclient.Text = "."
        '
        'lblqtym
        '
        Me.lblqtym.AutoSize = True
        Me.lblqtym.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblqtym.Location = New System.Drawing.Point(887, 55)
        Me.lblqtym.Name = "lblqtym"
        Me.lblqtym.Size = New System.Drawing.Size(15, 21)
        Me.lblqtym.TabIndex = 37
        Me.lblqtym.Text = "."
        '
        'lblqtykg
        '
        Me.lblqtykg.AutoSize = True
        Me.lblqtykg.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblqtykg.Location = New System.Drawing.Point(600, 55)
        Me.lblqtykg.Name = "lblqtykg"
        Me.lblqtykg.Size = New System.Drawing.Size(15, 21)
        Me.lblqtykg.TabIndex = 36
        Me.lblqtykg.Text = "."
        '
        'lblcontractno
        '
        Me.lblcontractno.AutoSize = True
        Me.lblcontractno.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblcontractno.Location = New System.Drawing.Point(890, 9)
        Me.lblcontractno.Name = "lblcontractno"
        Me.lblcontractno.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblcontractno.Size = New System.Drawing.Size(15, 21)
        Me.lblcontractno.TabIndex = 35
        Me.lblcontractno.Text = "."
        '
        'lblbatchno
        '
        Me.lblbatchno.AutoSize = True
        Me.lblbatchno.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblbatchno.Location = New System.Drawing.Point(600, 6)
        Me.lblbatchno.Name = "lblbatchno"
        Me.lblbatchno.Size = New System.Drawing.Size(15, 21)
        Me.lblbatchno.TabIndex = 34
        Me.lblbatchno.Text = "."
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(579, 123)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(100, 24)
        Me.Label5.TabIndex = 33
        Me.Label5.Text = "أمر شغل "
        '
        'cmbworder
        '
        Me.cmbworder.Font = New System.Drawing.Font("Tahoma", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbworder.FormattingEnabled = True
        Me.cmbworder.Location = New System.Drawing.Point(523, 164)
        Me.cmbworder.Name = "cmbworder"
        Me.cmbworder.Size = New System.Drawing.Size(244, 30)
        Me.cmbworder.TabIndex = 32
        '
        'btnreport
        '
        Me.btnreport.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnreport.Location = New System.Drawing.Point(584, 291)
        Me.btnreport.Name = "btnreport"
        Me.btnreport.Size = New System.Drawing.Size(184, 58)
        Me.btnreport.TabIndex = 44
        Me.btnreport.Text = "تقرير تسليم مخزن"
        Me.btnreport.UseVisualStyleBackColor = True
        '
        'lblkg
        '
        Me.lblkg.AutoSize = True
        Me.lblkg.Location = New System.Drawing.Point(166, 332)
        Me.lblkg.Name = "lblkg"
        Me.lblkg.Size = New System.Drawing.Size(10, 16)
        Me.lblkg.TabIndex = 43
        Me.lblkg.Text = "."
        '
        'lblm
        '
        Me.lblm.AutoSize = True
        Me.lblm.Location = New System.Drawing.Point(166, 281)
        Me.lblm.Name = "lblm"
        Me.lblm.Size = New System.Drawing.Size(10, 16)
        Me.lblm.TabIndex = 42
        Me.lblm.Text = "."
        '
        'cmbroll
        '
        Me.cmbroll.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbroll.FormattingEnabled = True
        Me.cmbroll.Location = New System.Drawing.Point(311, 281)
        Me.cmbroll.Name = "cmbroll"
        Me.cmbroll.Size = New System.Drawing.Size(122, 29)
        Me.cmbroll.TabIndex = 41
        '
        'btnprint
        '
        Me.btnprint.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnprint.Location = New System.Drawing.Point(267, 320)
        Me.btnprint.Name = "btnprint"
        Me.btnprint.Size = New System.Drawing.Size(206, 39)
        Me.btnprint.TabIndex = 40
        Me.btnprint.Text = "طباعة بيانات رول"
        Me.btnprint.UseVisualStyleBackColor = True
        '
        'btnaddroll
        '
        Me.btnaddroll.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnaddroll.Location = New System.Drawing.Point(850, 139)
        Me.btnaddroll.Name = "btnaddroll"
        Me.btnaddroll.Size = New System.Drawing.Size(182, 42)
        Me.btnaddroll.TabIndex = 45
        Me.btnaddroll.Text = "اضافه توب جديد"
        Me.btnaddroll.UseVisualStyleBackColor = True
        '
        'lbltotalw
        '
        Me.lbltotalw.AutoSize = True
        Me.lbltotalw.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbltotalw.Location = New System.Drawing.Point(496, 225)
        Me.lbltotalw.Name = "lbltotalw"
        Me.lbltotalw.Size = New System.Drawing.Size(15, 21)
        Me.lbltotalw.TabIndex = 47
        Me.lbltotalw.Text = "."
        '
        'lbltotalm
        '
        Me.lbltotalm.AutoSize = True
        Me.lbltotalm.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbltotalm.Location = New System.Drawing.Point(795, 225)
        Me.lbltotalm.Name = "lbltotalm"
        Me.lbltotalm.Size = New System.Drawing.Size(15, 21)
        Me.lbltotalm.TabIndex = 46
        Me.lbltotalm.Text = "."
        '
        'dataGridViewDetails
        '
        Me.dataGridViewDetails.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dataGridViewDetails.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dataGridViewDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dataGridViewDetails.Location = New System.Drawing.Point(79, 429)
        Me.dataGridViewDetails.Name = "dataGridViewDetails"
        Me.dataGridViewDetails.RowHeadersWidth = 51
        Me.dataGridViewDetails.RowTemplate.Height = 26
        Me.dataGridViewDetails.Size = New System.Drawing.Size(1514, 412)
        Me.dataGridViewDetails.TabIndex = 48
        '
        'lbltotalpoints
        '
        Me.lbltotalpoints.AutoSize = True
        Me.lbltotalpoints.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbltotalpoints.Location = New System.Drawing.Point(655, 391)
        Me.lbltotalpoints.Name = "lbltotalpoints"
        Me.lbltotalpoints.Size = New System.Drawing.Size(128, 24)
        Me.lbltotalpoints.TabIndex = 49
        Me.lbltotalpoints.Text = "Total Points"
        '
        'lblUsername
        '
        Me.lblUsername.AutoSize = True
        Me.lblUsername.Location = New System.Drawing.Point(15, 164)
        Me.lblUsername.Name = "lblUsername"
        Me.lblUsername.Size = New System.Drawing.Size(33, 16)
        Me.lblUsername.TabIndex = 50
        Me.lblUsername.Text = "user"
        '
        'btnprint2
        '
        Me.btnprint2.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnprint2.Location = New System.Drawing.Point(864, 291)
        Me.btnprint2.Name = "btnprint2"
        Me.btnprint2.Size = New System.Drawing.Size(168, 58)
        Me.btnprint2.TabIndex = 51
        Me.btnprint2.Text = "تقرير الفحص النهائى"
        Me.btnprint2.UseVisualStyleBackColor = True
        '
        'btnrollnd
        '
        Me.btnrollnd.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnrollnd.Location = New System.Drawing.Point(1051, 240)
        Me.btnrollnd.Name = "btnrollnd"
        Me.btnrollnd.Size = New System.Drawing.Size(123, 36)
        Me.btnrollnd.TabIndex = 52
        Me.btnrollnd.Text = "توب وصلات"
        Me.btnrollnd.UseVisualStyleBackColor = True
        '
        'lblmaterial
        '
        Me.lblmaterial.AutoSize = True
        Me.lblmaterial.Location = New System.Drawing.Point(296, 139)
        Me.lblmaterial.Name = "lblmaterial"
        Me.lblmaterial.Size = New System.Drawing.Size(10, 16)
        Me.lblmaterial.TabIndex = 53
        Me.lblmaterial.Text = "."
        '
        'btnprintall
        '
        Me.btnprintall.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnprintall.Location = New System.Drawing.Point(1084, 361)
        Me.btnprintall.Name = "btnprintall"
        Me.btnprintall.Size = New System.Drawing.Size(117, 39)
        Me.btnprintall.TabIndex = 54
        Me.btnprintall.Text = "طباعة الكل"
        Me.btnprintall.UseVisualStyleBackColor = True
        '
        'mainfinishinspectform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1622, 863)
        Me.Controls.Add(Me.lblmaterial)
        Me.Controls.Add(Me.btnrollnd)
        Me.Controls.Add(Me.btnprint2)
        Me.Controls.Add(Me.btnprintall)
        Me.Controls.Add(Me.lblUsername)
        Me.Controls.Add(Me.lbltotalpoints)
        Me.Controls.Add(Me.dataGridViewDetails)
        Me.Controls.Add(Me.lbltotalw)
        Me.Controls.Add(Me.lbltotalm)
        Me.Controls.Add(Me.btnaddroll)
        Me.Controls.Add(Me.btnreport)
        Me.Controls.Add(Me.lblkg)
        Me.Controls.Add(Me.lblm)
        Me.Controls.Add(Me.cmbroll)
        Me.Controls.Add(Me.btnprint)
        Me.Controls.Add(Me.lblcolor)
        Me.Controls.Add(Me.lblclient)
        Me.Controls.Add(Me.lblqtym)
        Me.Controls.Add(Me.lblqtykg)
        Me.Controls.Add(Me.lblcontractno)
        Me.Controls.Add(Me.lblbatchno)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.cmbworder)
        Me.Name = "mainfinishinspectform"
        Me.Text = "الشاشه الرئيسيه للفحص المجهز"
        CType(Me.dataGridViewDetails, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblcolor As Label
    Friend WithEvents lblclient As Label
    Friend WithEvents lblqtym As Label
    Friend WithEvents lblqtykg As Label
    Friend WithEvents lblcontractno As Label
    Friend WithEvents lblbatchno As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents cmbworder As ComboBox
    Friend WithEvents btnreport As Button
    Friend WithEvents lblkg As Label
    Friend WithEvents lblm As Label
    Friend WithEvents cmbroll As ComboBox
    Friend WithEvents btnprint As Button
    Friend WithEvents btnaddroll As Button
    Friend WithEvents lbltotalw As Label
    Friend WithEvents lbltotalm As Label
    Friend WithEvents dataGridViewDetails As DataGridView
    Friend WithEvents lbltotalpoints As Label
    Friend WithEvents lblUsername As Label
    Friend WithEvents btnprint2 As Button
    Friend WithEvents btnrollnd As Button
    Friend WithEvents lblmaterial As Label
    Friend WithEvents btnprintall As Button
End Class
