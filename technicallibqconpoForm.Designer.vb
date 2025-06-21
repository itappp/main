<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class technicallibqconpoForm
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
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.cmbworder = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblcontractno = New System.Windows.Forms.Label()
        Me.lblbatchno = New System.Windows.Forms.Label()
        Me.lblkindcontract = New System.Windows.Forms.Label()
        Me.lblcontractid = New System.Windows.Forms.Label()
        Me.dgvsales = New System.Windows.Forms.DataGridView()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.dgvqc = New System.Windows.Forms.DataGridView()
        Me.dgvdefects = New System.Windows.Forms.DataGridView()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cmbcodelib = New System.Windows.Forms.ComboBox()
        Me.lbllibcode = New System.Windows.Forms.Label()
        Me.dgvLibCode = New System.Windows.Forms.DataGridView()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.btnupdate = New System.Windows.Forms.Button()
        Me.lblusername = New System.Windows.Forms.Label()
        Me.lblstatus = New System.Windows.Forms.Label()
        Me.dgvoldcode = New System.Windows.Forms.DataGridView()
        Me.lbloldcode = New System.Windows.Forms.Label()
        Me.txtnotes = New System.Windows.Forms.TextBox()
        Me.lblqty = New System.Windows.Forms.Label()
        Me.lblweightcalc = New System.Windows.Forms.Label()
        Me.cmbcodes = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        CType(Me.dgvsales, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvqc, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvdefects, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvLibCode, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvoldcode, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmbworder
        '
        Me.cmbworder.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbworder.FormattingEnabled = True
        Me.cmbworder.Location = New System.Drawing.Point(155, 17)
        Me.cmbworder.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbworder.Name = "cmbworder"
        Me.cmbworder.Size = New System.Drawing.Size(316, 32)
        Me.cmbworder.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(54, 17)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(93, 21)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "أمر الشغل"
        '
        'lblcontractno
        '
        Me.lblcontractno.AutoSize = True
        Me.lblcontractno.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblcontractno.Location = New System.Drawing.Point(1007, 17)
        Me.lblcontractno.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblcontractno.Name = "lblcontractno"
        Me.lblcontractno.Size = New System.Drawing.Size(0, 21)
        Me.lblcontractno.TabIndex = 2
        '
        'lblbatchno
        '
        Me.lblbatchno.AutoSize = True
        Me.lblbatchno.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblbatchno.Location = New System.Drawing.Point(767, 17)
        Me.lblbatchno.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblbatchno.Name = "lblbatchno"
        Me.lblbatchno.Size = New System.Drawing.Size(0, 21)
        Me.lblbatchno.TabIndex = 3
        '
        'lblkindcontract
        '
        Me.lblkindcontract.AutoSize = True
        Me.lblkindcontract.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblkindcontract.Location = New System.Drawing.Point(571, 17)
        Me.lblkindcontract.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblkindcontract.Name = "lblkindcontract"
        Me.lblkindcontract.Size = New System.Drawing.Size(0, 21)
        Me.lblkindcontract.TabIndex = 4
        '
        'lblcontractid
        '
        Me.lblcontractid.AutoSize = True
        Me.lblcontractid.Location = New System.Drawing.Point(1188, 17)
        Me.lblcontractid.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblcontractid.Name = "lblcontractid"
        Me.lblcontractid.Size = New System.Drawing.Size(0, 21)
        Me.lblcontractid.TabIndex = 5
        '
        'dgvsales
        '
        Me.dgvsales.AllowUserToAddRows = False
        Me.dgvsales.AllowUserToDeleteRows = False
        Me.dgvsales.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dgvsales.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvsales.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ActiveCaption
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvsales.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.dgvsales.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvsales.Location = New System.Drawing.Point(44, 95)
        Me.dgvsales.Name = "dgvsales"
        Me.dgvsales.RowHeadersWidth = 51
        Me.dgvsales.RowTemplate.Height = 26
        Me.dgvsales.Size = New System.Drawing.Size(1415, 88)
        Me.dgvsales.TabIndex = 6
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(571, 59)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(63, 24)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Sales"
        '
        'dgvqc
        '
        Me.dgvqc.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dgvqc.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvqc.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised
        Me.dgvqc.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvqc.Location = New System.Drawing.Point(31, 223)
        Me.dgvqc.Name = "dgvqc"
        Me.dgvqc.RowHeadersWidth = 51
        Me.dgvqc.RowTemplate.Height = 26
        Me.dgvqc.Size = New System.Drawing.Size(1437, 117)
        Me.dgvqc.TabIndex = 8
        '
        'dgvdefects
        '
        Me.dgvdefects.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dgvdefects.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvdefects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvdefects.Location = New System.Drawing.Point(726, 401)
        Me.dgvdefects.Name = "dgvdefects"
        Me.dgvdefects.RowHeadersWidth = 51
        Me.dgvdefects.RowTemplate.Height = 26
        Me.dgvdefects.Size = New System.Drawing.Size(874, 157)
        Me.dgvdefects.TabIndex = 9
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(571, 185)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(83, 24)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "QC LAB"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(1173, 374)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(86, 24)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "Defects"
        '
        'cmbcodelib
        '
        Me.cmbcodelib.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbcodelib.FormattingEnabled = True
        Me.cmbcodelib.Location = New System.Drawing.Point(121, 346)
        Me.cmbcodelib.Name = "cmbcodelib"
        Me.cmbcodelib.Size = New System.Drawing.Size(213, 32)
        Me.cmbcodelib.TabIndex = 12
        '
        'lbllibcode
        '
        Me.lbllibcode.AutoSize = True
        Me.lbllibcode.Location = New System.Drawing.Point(859, 352)
        Me.lbllibcode.Name = "lbllibcode"
        Me.lbllibcode.Size = New System.Drawing.Size(18, 21)
        Me.lbllibcode.TabIndex = 13
        Me.lbllibcode.Text = "L"
        '
        'dgvLibCode
        '
        Me.dgvLibCode.AllowUserToAddRows = False
        Me.dgvLibCode.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dgvLibCode.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvLibCode.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised
        Me.dgvLibCode.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvLibCode.Location = New System.Drawing.Point(12, 384)
        Me.dgvLibCode.Name = "dgvLibCode"
        Me.dgvLibCode.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.dgvLibCode.RowHeadersWidth = 51
        Me.dgvLibCode.RowTemplate.Height = 26
        Me.dgvLibCode.Size = New System.Drawing.Size(648, 475)
        Me.dgvLibCode.TabIndex = 14
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(15, 349)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(89, 21)
        Me.Label5.TabIndex = 15
        Me.Label5.Text = "كود المكتبه"
        '
        'btnupdate
        '
        Me.btnupdate.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnupdate.Location = New System.Drawing.Point(1244, 744)
        Me.btnupdate.Name = "btnupdate"
        Me.btnupdate.Size = New System.Drawing.Size(159, 54)
        Me.btnupdate.TabIndex = 16
        Me.btnupdate.Text = "تسجيل البيانات"
        Me.btnupdate.UseVisualStyleBackColor = True
        '
        'lblusername
        '
        Me.lblusername.AutoSize = True
        Me.lblusername.Location = New System.Drawing.Point(13, -3)
        Me.lblusername.Name = "lblusername"
        Me.lblusername.Size = New System.Drawing.Size(42, 21)
        Me.lblusername.TabIndex = 17
        Me.lblusername.Text = "user"
        '
        'lblstatus
        '
        Me.lblstatus.AutoSize = True
        Me.lblstatus.Location = New System.Drawing.Point(351, 59)
        Me.lblstatus.Name = "lblstatus"
        Me.lblstatus.Size = New System.Drawing.Size(51, 21)
        Me.lblstatus.TabIndex = 18
        Me.lblstatus.Text = "الحالة"
        '
        'dgvoldcode
        '
        Me.dgvoldcode.AllowUserToAddRows = False
        Me.dgvoldcode.AllowUserToDeleteRows = False
        Me.dgvoldcode.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dgvoldcode.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvoldcode.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvoldcode.Location = New System.Drawing.Point(786, 585)
        Me.dgvoldcode.Name = "dgvoldcode"
        Me.dgvoldcode.ReadOnly = True
        Me.dgvoldcode.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.dgvoldcode.RowHeadersWidth = 51
        Me.dgvoldcode.RowTemplate.Height = 26
        Me.dgvoldcode.Size = New System.Drawing.Size(432, 309)
        Me.dgvoldcode.TabIndex = 20
        '
        'lbloldcode
        '
        Me.lbloldcode.AutoSize = True
        Me.lbloldcode.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbloldcode.Location = New System.Drawing.Point(711, 561)
        Me.lbloldcode.Name = "lbloldcode"
        Me.lbloldcode.Size = New System.Drawing.Size(92, 21)
        Me.lbloldcode.TabIndex = 19
        Me.lbloldcode.Text = "Old Code "
        '
        'txtnotes
        '
        Me.txtnotes.Location = New System.Drawing.Point(1224, 600)
        Me.txtnotes.Multiline = True
        Me.txtnotes.Name = "txtnotes"
        Me.txtnotes.Size = New System.Drawing.Size(324, 118)
        Me.txtnotes.TabIndex = 21
        '
        'lblqty
        '
        Me.lblqty.AutoSize = True
        Me.lblqty.Location = New System.Drawing.Point(1353, 23)
        Me.lblqty.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblqty.Name = "lblqty"
        Me.lblqty.Size = New System.Drawing.Size(0, 21)
        Me.lblqty.TabIndex = 22
        '
        'lblweightcalc
        '
        Me.lblweightcalc.AutoSize = True
        Me.lblweightcalc.Location = New System.Drawing.Point(1048, 352)
        Me.lblweightcalc.Name = "lblweightcalc"
        Me.lblweightcalc.Size = New System.Drawing.Size(18, 21)
        Me.lblweightcalc.TabIndex = 23
        Me.lblweightcalc.Text = "L"
        '
        'cmbcodes
        '
        Me.cmbcodes.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbcodes.FormattingEnabled = True
        Me.cmbcodes.Location = New System.Drawing.Point(381, 346)
        Me.cmbcodes.Name = "cmbcodes"
        Me.cmbcodes.Size = New System.Drawing.Size(213, 32)
        Me.cmbcodes.TabIndex = 24
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(613, 352)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(39, 21)
        Me.Label6.TabIndex = 25
        Me.Label6.Text = "كود "
        '
        'technicallibqconpoForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1729, 913)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.cmbcodes)
        Me.Controls.Add(Me.lblweightcalc)
        Me.Controls.Add(Me.lblqty)
        Me.Controls.Add(Me.txtnotes)
        Me.Controls.Add(Me.dgvoldcode)
        Me.Controls.Add(Me.lbloldcode)
        Me.Controls.Add(Me.lblstatus)
        Me.Controls.Add(Me.lblusername)
        Me.Controls.Add(Me.btnupdate)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.dgvLibCode)
        Me.Controls.Add(Me.lbllibcode)
        Me.Controls.Add(Me.cmbcodelib)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.dgvdefects)
        Me.Controls.Add(Me.dgvqc)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.dgvsales)
        Me.Controls.Add(Me.lblcontractid)
        Me.Controls.Add(Me.lblkindcontract)
        Me.Controls.Add(Me.lblbatchno)
        Me.Controls.Add(Me.lblcontractno)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmbworder)
        Me.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "technicallibqconpoForm"
        Me.Text = "technicallibqconpoForm"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.dgvsales, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvqc, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvdefects, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvLibCode, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvoldcode, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmbworder As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblcontractno As System.Windows.Forms.Label
    Friend WithEvents lblbatchno As System.Windows.Forms.Label
    Friend WithEvents lblkindcontract As System.Windows.Forms.Label
    Friend WithEvents lblcontractid As System.Windows.Forms.Label
    Friend WithEvents dgvsales As System.Windows.Forms.DataGridView
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents dgvqc As System.Windows.Forms.DataGridView
    Friend WithEvents dgvdefects As System.Windows.Forms.DataGridView
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cmbcodelib As System.Windows.Forms.ComboBox
    Friend WithEvents lbllibcode As System.Windows.Forms.Label
    Friend WithEvents dgvLibCode As System.Windows.Forms.DataGridView
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents btnupdate As System.Windows.Forms.Button
    Friend WithEvents lblusername As System.Windows.Forms.Label
    Friend WithEvents lblstatus As System.Windows.Forms.Label
    Friend WithEvents dgvoldcode As System.Windows.Forms.DataGridView
    Friend WithEvents lbloldcode As System.Windows.Forms.Label
    Friend WithEvents txtnotes As TextBox
    Friend WithEvents lblqty As Label
    Friend WithEvents lblweightcalc As Label
    Friend WithEvents cmbcodes As ComboBox
    Friend WithEvents Label6 As Label
End Class
