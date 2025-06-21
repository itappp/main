<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class finishDisbursementtform
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
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.btnSubmit = New System.Windows.Forms.Button()
        Me.btnsearch = New System.Windows.Forms.Button()
        Me.txtworderid = New System.Windows.Forms.TextBox()
        Me.SubmitButton = New System.Windows.Forms.Button()
        Me.cmbKindTrans = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Cmbproductname = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cmbclient = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cmbbatch = New System.Windows.Forms.ComboBox()
        Me.cmbtoorfrom = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtnotes = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.cmbcolor = New System.Windows.Forms.ComboBox()
        Me.lblusername = New System.Windows.Forms.Label()
        Me.btnView = New System.Windows.Forms.Button()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.BackgroundColor = System.Drawing.SystemColors.Control
        Me.DataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridView1.DefaultCellStyle = DataGridViewCellStyle1
        Me.DataGridView1.Location = New System.Drawing.Point(15, 291)
        Me.DataGridView1.Margin = New System.Windows.Forms.Padding(4)
        Me.DataGridView1.Name = "DataGridView1"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridView1.RowHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.DataGridView1.RowTemplate.Height = 26
        Me.DataGridView1.Size = New System.Drawing.Size(1347, 469)
        Me.DataGridView1.TabIndex = 0
        '
        'btnSubmit
        '
        Me.btnSubmit.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSubmit.Location = New System.Drawing.Point(964, 217)
        Me.btnSubmit.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSubmit.Name = "btnSubmit"
        Me.btnSubmit.Size = New System.Drawing.Size(141, 63)
        Me.btnSubmit.TabIndex = 1
        Me.btnSubmit.Text = "تسجيل"
        Me.btnSubmit.UseVisualStyleBackColor = True
        '
        'btnsearch
        '
        Me.btnsearch.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnsearch.Location = New System.Drawing.Point(602, 128)
        Me.btnsearch.Margin = New System.Windows.Forms.Padding(4)
        Me.btnsearch.Name = "btnsearch"
        Me.btnsearch.Size = New System.Drawing.Size(119, 39)
        Me.btnsearch.TabIndex = 2
        Me.btnsearch.Text = "بحث"
        Me.btnsearch.UseVisualStyleBackColor = True
        '
        'txtworderid
        '
        Me.txtworderid.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtworderid.Location = New System.Drawing.Point(79, 42)
        Me.txtworderid.Margin = New System.Windows.Forms.Padding(4)
        Me.txtworderid.Name = "txtworderid"
        Me.txtworderid.Size = New System.Drawing.Size(390, 32)
        Me.txtworderid.TabIndex = 3
        '
        'SubmitButton
        '
        Me.SubmitButton.Location = New System.Drawing.Point(459, 77)
        Me.SubmitButton.Name = "SubmitButton"
        Me.SubmitButton.Size = New System.Drawing.Size(75, 23)
        Me.SubmitButton.TabIndex = 1
        Me.SubmitButton.Text = "submit"
        Me.SubmitButton.UseVisualStyleBackColor = True
        '
        'cmbKindTrans
        '
        Me.cmbKindTrans.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbKindTrans.FormattingEnabled = True
        Me.cmbKindTrans.Location = New System.Drawing.Point(537, 42)
        Me.cmbKindTrans.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbKindTrans.Name = "cmbKindTrans"
        Me.cmbKindTrans.Size = New System.Drawing.Size(219, 32)
        Me.cmbKindTrans.TabIndex = 4
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(212, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(94, 24)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "أمر شغل"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(777, 42)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(110, 24)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "نوع الحركه"
        '
        'Cmbproductname
        '
        Me.Cmbproductname.AllowDrop = True
        Me.Cmbproductname.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.Cmbproductname.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.Cmbproductname.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Cmbproductname.FormattingEnabled = True
        Me.Cmbproductname.Location = New System.Drawing.Point(131, 86)
        Me.Cmbproductname.Name = "Cmbproductname"
        Me.Cmbproductname.Size = New System.Drawing.Size(234, 29)
        Me.Cmbproductname.TabIndex = 7
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(381, 86)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(94, 21)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "نوع الخامه"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(381, 146)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(54, 21)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "عميل"
        '
        'cmbclient
        '
        Me.cmbclient.AllowDrop = True
        Me.cmbclient.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbclient.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbclient.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbclient.FormattingEnabled = True
        Me.cmbclient.Location = New System.Drawing.Point(131, 146)
        Me.cmbclient.Name = "cmbclient"
        Me.cmbclient.Size = New System.Drawing.Size(234, 29)
        Me.cmbclient.TabIndex = 9
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(381, 195)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(60, 21)
        Me.Label5.TabIndex = 12
        Me.Label5.Text = "رسالة"
        '
        'cmbbatch
        '
        Me.cmbbatch.AllowDrop = True
        Me.cmbbatch.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbbatch.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbbatch.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbbatch.FormattingEnabled = True
        Me.cmbbatch.Location = New System.Drawing.Point(131, 195)
        Me.cmbbatch.Name = "cmbbatch"
        Me.cmbbatch.Size = New System.Drawing.Size(234, 29)
        Me.cmbbatch.TabIndex = 11
        '
        'cmbtoorfrom
        '
        Me.cmbtoorfrom.FormattingEnabled = True
        Me.cmbtoorfrom.Location = New System.Drawing.Point(893, 78)
        Me.cmbtoorfrom.Name = "cmbtoorfrom"
        Me.cmbtoorfrom.Size = New System.Drawing.Size(246, 29)
        Me.cmbtoorfrom.TabIndex = 13
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(1145, 71)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(123, 24)
        Me.Label6.TabIndex = 14
        Me.Label6.Text = "قسم أو عميل"
        '
        'txtnotes
        '
        Me.txtnotes.Location = New System.Drawing.Point(893, 124)
        Me.txtnotes.Multiline = True
        Me.txtnotes.Name = "txtnotes"
        Me.txtnotes.Size = New System.Drawing.Size(246, 61)
        Me.txtnotes.TabIndex = 15
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(1170, 145)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(73, 21)
        Me.Label7.TabIndex = 16
        Me.Label7.Text = "ملاحظات"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(381, 255)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(51, 21)
        Me.Label8.TabIndex = 18
        Me.Label8.Text = "اللون"
        '
        'cmbcolor
        '
        Me.cmbcolor.AllowDrop = True
        Me.cmbcolor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbcolor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbcolor.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbcolor.FormattingEnabled = True
        Me.cmbcolor.Location = New System.Drawing.Point(131, 255)
        Me.cmbcolor.Name = "cmbcolor"
        Me.cmbcolor.Size = New System.Drawing.Size(234, 29)
        Me.cmbcolor.TabIndex = 17
        '
        'lblusername
        '
        Me.lblusername.AutoSize = True
        Me.lblusername.Location = New System.Drawing.Point(845, 9)
        Me.lblusername.Name = "lblusername"
        Me.lblusername.Size = New System.Drawing.Size(42, 21)
        Me.lblusername.TabIndex = 19
        Me.lblusername.Text = "user"
        '
        'btnView
        '
        Me.btnView.Location = New System.Drawing.Point(12, 122)
        Me.btnView.Name = "btnView"
        Me.btnView.Size = New System.Drawing.Size(83, 45)
        Me.btnView.TabIndex = 20
        Me.btnView.Text = "View"
        Me.btnView.UseVisualStyleBackColor = True
        '
        'finishDisbursementtform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1451, 795)
        Me.Controls.Add(Me.btnView)
        Me.Controls.Add(Me.lblusername)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.cmbcolor)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtnotes)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.cmbtoorfrom)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.cmbbatch)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.cmbclient)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Cmbproductname)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmbKindTrans)
        Me.Controls.Add(Me.txtworderid)
        Me.Controls.Add(Me.btnsearch)
        Me.Controls.Add(Me.btnSubmit)
        Me.Controls.Add(Me.DataGridView1)
        Me.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "finishDisbursementtform"
        Me.Text = "شاشة الصرف والمرتجع  مخزن المجهز"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents btnSubmit As System.Windows.Forms.Button
    Friend WithEvents btnsearch As System.Windows.Forms.Button
    Friend WithEvents txtworderid As System.Windows.Forms.TextBox
    Friend WithEvents SubmitButton As System.Windows.Forms.Button
    Friend WithEvents cmbKindTrans As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Cmbproductname As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cmbclient As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents cmbbatch As System.Windows.Forms.ComboBox
    Friend WithEvents cmbtoorfrom As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtnotes As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents cmbcolor As System.Windows.Forms.ComboBox
    Friend WithEvents lblusername As System.Windows.Forms.Label
    Friend WithEvents btnView As System.Windows.Forms.Button
End Class
