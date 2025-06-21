<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class trackingform
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
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.cmbWorder = New System.Windows.Forms.ComboBox()
        Me.cmbCurrentMachine = New System.Windows.Forms.ComboBox()
        Me.cmbNextMachine = New System.Windows.Forms.ComboBox()
        Me.cmbclients = New System.Windows.Forms.ComboBox()
        Me.btnExportToExcel = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.chkShowAll = New System.Windows.Forms.CheckBox()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.BackgroundColor = System.Drawing.SystemColors.Control
        Me.DataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Location = New System.Drawing.Point(12, 228)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.RowHeadersWidth = 51
        Me.DataGridView1.RowTemplate.Height = 26
        Me.DataGridView1.Size = New System.Drawing.Size(1264, 539)
        Me.DataGridView1.TabIndex = 0
        '
        'btnRefresh
        '
        Me.btnRefresh.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRefresh.Location = New System.Drawing.Point(195, 102)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(129, 37)
        Me.btnRefresh.TabIndex = 1
        Me.btnRefresh.Text = "Refresh"
        Me.btnRefresh.UseVisualStyleBackColor = True
        '
        'cmbWorder
        '
        Me.cmbWorder.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbWorder.FormattingEnabled = True
        Me.cmbWorder.Location = New System.Drawing.Point(127, 37)
        Me.cmbWorder.Name = "cmbWorder"
        Me.cmbWorder.Size = New System.Drawing.Size(253, 29)
        Me.cmbWorder.TabIndex = 2
        '
        'cmbCurrentMachine
        '
        Me.cmbCurrentMachine.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbCurrentMachine.FormattingEnabled = True
        Me.cmbCurrentMachine.Location = New System.Drawing.Point(502, 64)
        Me.cmbCurrentMachine.Name = "cmbCurrentMachine"
        Me.cmbCurrentMachine.Size = New System.Drawing.Size(170, 29)
        Me.cmbCurrentMachine.TabIndex = 3
        '
        'cmbNextMachine
        '
        Me.cmbNextMachine.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbNextMachine.FormattingEnabled = True
        Me.cmbNextMachine.Location = New System.Drawing.Point(799, 64)
        Me.cmbNextMachine.Name = "cmbNextMachine"
        Me.cmbNextMachine.Size = New System.Drawing.Size(170, 29)
        Me.cmbNextMachine.TabIndex = 4
        '
        'cmbclients
        '
        Me.cmbclients.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbclients.FormattingEnabled = True
        Me.cmbclients.Location = New System.Drawing.Point(1087, 64)
        Me.cmbclients.Name = "cmbclients"
        Me.cmbclients.Size = New System.Drawing.Size(170, 29)
        Me.cmbclients.TabIndex = 5
        '
        'btnExportToExcel
        '
        Me.btnExportToExcel.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExportToExcel.Location = New System.Drawing.Point(12, 171)
        Me.btnExportToExcel.Name = "btnExportToExcel"
        Me.btnExportToExcel.Size = New System.Drawing.Size(123, 51)
        Me.btnExportToExcel.TabIndex = 6
        Me.btnExportToExcel.Text = "Export To Excell"
        Me.btnExportToExcel.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(29, 37)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(73, 21)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Worder"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(1123, 23)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(96, 21)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "كود العميل"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(845, 23)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(59, 21)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "التالى"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(542, 23)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(65, 21)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "الحالى"
        '
        'btnSearch
        '
        Me.btnSearch.Font = New System.Drawing.Font("Tahoma", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSearch.Location = New System.Drawing.Point(818, 134)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(118, 40)
        Me.btnSearch.TabIndex = 11
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'chkShowAll
        '
        Me.chkShowAll.AutoSize = True
        Me.chkShowAll.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkShowAll.Location = New System.Drawing.Point(390, 189)
        Me.chkShowAll.Name = "chkShowAll"
        Me.chkShowAll.Size = New System.Drawing.Size(290, 22)
        Me.chkShowAll.TabIndex = 12
        Me.chkShowAll.Text = "لأظهار الاوامر ذات تسجيل المرحله الاخيره "
        Me.chkShowAll.UseVisualStyleBackColor = True
        '
        'trackingform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1347, 868)
        Me.Controls.Add(Me.chkShowAll)
        Me.Controls.Add(Me.btnSearch)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnExportToExcel)
        Me.Controls.Add(Me.cmbclients)
        Me.Controls.Add(Me.cmbNextMachine)
        Me.Controls.Add(Me.cmbCurrentMachine)
        Me.Controls.Add(Me.cmbWorder)
        Me.Controls.Add(Me.btnRefresh)
        Me.Controls.Add(Me.DataGridView1)
        Me.Name = "trackingform"
        Me.Text = "trackingform"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents btnRefresh As Button
    Friend WithEvents cmbWorder As ComboBox
    Friend WithEvents cmbCurrentMachine As ComboBox
    Friend WithEvents cmbNextMachine As ComboBox
    Friend WithEvents cmbclients As ComboBox
    Friend WithEvents btnExportToExcel As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents btnSearch As Button
    Friend WithEvents chkShowAll As CheckBox
End Class
