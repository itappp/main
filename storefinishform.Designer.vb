<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class storefinishform
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.btnview = New System.Windows.Forms.Button()
        Me.lblUsername = New System.Windows.Forms.Label()
        Me.btnInsert = New System.Windows.Forms.Button()
        Me.lblcnd = New System.Windows.Forms.Label()
        Me.lblfirst = New System.Windows.Forms.Label()
        Me.lbltotalww = New System.Windows.Forms.Label()
        Me.lbltotalroll = New System.Windows.Forms.Label()
        Me.lbltotalh = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtcontractno = New System.Windows.Forms.TextBox()
        Me.dgvresults = New System.Windows.Forms.DataGridView()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.dtpToDate = New System.Windows.Forms.DateTimePicker()
        Me.dtpfromDate = New System.Windows.Forms.DateTimePicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtworderid = New System.Windows.Forms.TextBox()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.txtbarcode = New System.Windows.Forms.TextBox()
        Me.btnclear = New System.Windows.Forms.Button()
        Me.btnPrintAllRolls = New System.Windows.Forms.Button()
        CType(Me.dgvresults, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnview
        '
        Me.btnview.Location = New System.Drawing.Point(16, 110)
        Me.btnview.Name = "btnview"
        Me.btnview.Size = New System.Drawing.Size(81, 36)
        Me.btnview.TabIndex = 35
        Me.btnview.Text = "view"
        Me.btnview.UseVisualStyleBackColor = True
        '
        'lblUsername
        '
        Me.lblUsername.AutoSize = True
        Me.lblUsername.Location = New System.Drawing.Point(13, 6)
        Me.lblUsername.Name = "lblUsername"
        Me.lblUsername.Size = New System.Drawing.Size(36, 16)
        Me.lblUsername.TabIndex = 34
        Me.lblUsername.Text = "User"
        '
        'btnInsert
        '
        Me.btnInsert.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnInsert.Location = New System.Drawing.Point(13, 147)
        Me.btnInsert.Name = "btnInsert"
        Me.btnInsert.Size = New System.Drawing.Size(211, 48)
        Me.btnInsert.TabIndex = 33
        Me.btnInsert.Text = "تسجيل"
        Me.btnInsert.UseVisualStyleBackColor = True
        '
        'lblcnd
        '
        Me.lblcnd.AutoSize = True
        Me.lblcnd.Location = New System.Drawing.Point(1472, 130)
        Me.lblcnd.Name = "lblcnd"
        Me.lblcnd.Size = New System.Drawing.Size(56, 16)
        Me.lblcnd.TabIndex = 32
        Me.lblcnd.Text = "درجة تانية"
        '
        'lblfirst
        '
        Me.lblfirst.AutoSize = True
        Me.lblfirst.Location = New System.Drawing.Point(1470, 78)
        Me.lblfirst.Name = "lblfirst"
        Me.lblfirst.Size = New System.Drawing.Size(56, 16)
        Me.lblfirst.TabIndex = 31
        Me.lblfirst.Text = "درجة اولى"
        '
        'lbltotalww
        '
        Me.lbltotalww.AutoSize = True
        Me.lbltotalww.Font = New System.Drawing.Font("Tahoma", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbltotalww.Location = New System.Drawing.Point(983, 147)
        Me.lbltotalww.Name = "lbltotalww"
        Me.lbltotalww.Size = New System.Drawing.Size(79, 16)
        Me.lbltotalww.TabIndex = 30
        Me.lbltotalww.Text = "اجمالى الوزن"
        '
        'lbltotalroll
        '
        Me.lbltotalroll.AutoSize = True
        Me.lbltotalroll.Font = New System.Drawing.Font("Tahoma", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbltotalroll.Location = New System.Drawing.Point(982, 114)
        Me.lbltotalroll.Name = "lbltotalroll"
        Me.lbltotalroll.Size = New System.Drawing.Size(79, 16)
        Me.lbltotalroll.TabIndex = 29
        Me.lbltotalroll.Text = "اجمالى أتواب"
        '
        'lbltotalh
        '
        Me.lbltotalh.AutoSize = True
        Me.lbltotalh.Font = New System.Drawing.Font("Tahoma", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbltotalh.Location = New System.Drawing.Point(982, 78)
        Me.lbltotalh.Name = "lbltotalh"
        Me.lbltotalh.Size = New System.Drawing.Size(82, 16)
        Me.lbltotalh.TabIndex = 28
        Me.lbltotalh.Text = "اجمالى كميه "
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(22, 74)
        Me.Label4.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(98, 21)
        Me.Label4.TabIndex = 27
        Me.Label4.Text = "رقم التعاقد"
        '
        'txtcontractno
        '
        Me.txtcontractno.Location = New System.Drawing.Point(130, 78)
        Me.txtcontractno.Margin = New System.Windows.Forms.Padding(5, 4, 5, 4)
        Me.txtcontractno.Name = "txtcontractno"
        Me.txtcontractno.Size = New System.Drawing.Size(231, 22)
        Me.txtcontractno.TabIndex = 26
        '
        'dgvresults
        '
        Me.dgvresults.AllowDrop = True
        Me.dgvresults.AllowUserToAddRows = False
        Me.dgvresults.AllowUserToDeleteRows = False
        Me.dgvresults.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dgvresults.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvresults.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        Me.dgvresults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvresults.Location = New System.Drawing.Point(43, 202)
        Me.dgvresults.Margin = New System.Windows.Forms.Padding(5, 4, 5, 4)
        Me.dgvresults.Name = "dgvresults"
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ActiveCaption
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvresults.RowHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvresults.RowHeadersWidth = 51
        Me.dgvresults.RowTemplate.Height = 26
        Me.dgvresults.Size = New System.Drawing.Size(1656, 656)
        Me.dgvresults.TabIndex = 25
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(1264, 29)
        Me.Label3.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(31, 21)
        Me.Label3.TabIndex = 24
        Me.Label3.Text = "To"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(834, 31)
        Me.Label2.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(54, 21)
        Me.Label2.TabIndex = 23
        Me.Label2.Text = "From"
        '
        'dtpToDate
        '
        Me.dtpToDate.Location = New System.Drawing.Point(1305, 32)
        Me.dtpToDate.Margin = New System.Windows.Forms.Padding(5, 4, 5, 4)
        Me.dtpToDate.Name = "dtpToDate"
        Me.dtpToDate.Size = New System.Drawing.Size(292, 22)
        Me.dtpToDate.TabIndex = 22
        '
        'dtpfromDate
        '
        Me.dtpfromDate.Location = New System.Drawing.Point(898, 31)
        Me.dtpfromDate.Margin = New System.Windows.Forms.Padding(5, 4, 5, 4)
        Me.dtpfromDate.Name = "dtpfromDate"
        Me.dtpfromDate.Size = New System.Drawing.Size(292, 22)
        Me.dtpfromDate.TabIndex = 21
        Me.dtpfromDate.Value = New Date(2024, 1, 1, 0, 0, 0, 0)
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(27, 31)
        Me.Label1.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(93, 21)
        Me.Label1.TabIndex = 20
        Me.Label1.Text = "أمر الشغل"
        '
        'txtworderid
        '
        Me.txtworderid.Location = New System.Drawing.Point(130, 30)
        Me.txtworderid.Margin = New System.Windows.Forms.Padding(5, 4, 5, 4)
        Me.txtworderid.Name = "txtworderid"
        Me.txtworderid.Size = New System.Drawing.Size(231, 22)
        Me.txtworderid.TabIndex = 19
        '
        'btnSearch
        '
        Me.btnSearch.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSearch.Location = New System.Drawing.Point(699, 130)
        Me.btnSearch.Margin = New System.Windows.Forms.Padding(5, 4, 5, 4)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(165, 45)
        Me.btnSearch.TabIndex = 18
        Me.btnSearch.Text = "بحث"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'txtbarcode
        '
        Me.txtbarcode.Location = New System.Drawing.Point(445, 30)
        Me.txtbarcode.Margin = New System.Windows.Forms.Padding(5, 4, 5, 4)
        Me.txtbarcode.Name = "txtbarcode"
        Me.txtbarcode.Size = New System.Drawing.Size(238, 22)
        Me.txtbarcode.TabIndex = 36
        '
        'btnclear
        '
        Me.btnclear.Location = New System.Drawing.Point(524, 59)
        Me.btnclear.Name = "btnclear"
        Me.btnclear.Size = New System.Drawing.Size(81, 36)
        Me.btnclear.TabIndex = 37
        Me.btnclear.Text = "Clear"
        Me.btnclear.UseVisualStyleBackColor = True
        '
        'btnPrintAllRolls
        '
        Me.btnPrintAllRolls.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrintAllRolls.Location = New System.Drawing.Point(429, 118)
        Me.btnPrintAllRolls.Margin = New System.Windows.Forms.Padding(5, 4, 5, 4)
        Me.btnPrintAllRolls.Name = "btnPrintAllRolls"
        Me.btnPrintAllRolls.Size = New System.Drawing.Size(88, 45)
        Me.btnPrintAllRolls.TabIndex = 38
        Me.btnPrintAllRolls.Text = "طباعه"
        Me.btnPrintAllRolls.UseVisualStyleBackColor = True
        '
        'storefinishform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1818, 1036)
        Me.Controls.Add(Me.btnPrintAllRolls)
        Me.Controls.Add(Me.btnclear)
        Me.Controls.Add(Me.txtbarcode)
        Me.Controls.Add(Me.btnview)
        Me.Controls.Add(Me.lblUsername)
        Me.Controls.Add(Me.btnInsert)
        Me.Controls.Add(Me.lblcnd)
        Me.Controls.Add(Me.lblfirst)
        Me.Controls.Add(Me.lbltotalww)
        Me.Controls.Add(Me.lbltotalroll)
        Me.Controls.Add(Me.lbltotalh)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtcontractno)
        Me.Controls.Add(Me.dgvresults)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.dtpToDate)
        Me.Controls.Add(Me.dtpfromDate)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtworderid)
        Me.Controls.Add(Me.btnSearch)
        Me.Name = "storefinishform"
        Me.Text = "شاشة استلام المخزن"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.dgvresults, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnview As System.Windows.Forms.Button
    Friend WithEvents lblUsername As System.Windows.Forms.Label
    Friend WithEvents btnInsert As System.Windows.Forms.Button
    Friend WithEvents lblcnd As System.Windows.Forms.Label
    Friend WithEvents lblfirst As System.Windows.Forms.Label
    Friend WithEvents lbltotalww As System.Windows.Forms.Label
    Friend WithEvents lbltotalroll As System.Windows.Forms.Label
    Friend WithEvents lbltotalh As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtcontractno As System.Windows.Forms.TextBox
    Friend WithEvents dgvresults As System.Windows.Forms.DataGridView
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents dtpToDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpfromDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtworderid As System.Windows.Forms.TextBox
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents txtbarcode As TextBox
    Friend WithEvents btnclear As Button
    Friend WithEvents btnPrintAllRolls As Button
End Class
