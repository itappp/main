<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class storefinishreturn
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
        Me.dgvResults = New System.Windows.Forms.DataGridView()
        Me.btnReturn = New System.Windows.Forms.Button()
        Me.txtRefPacking = New System.Windows.Forms.TextBox()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblUsername = New System.Windows.Forms.Label()
        Me.txtWorkOrder = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        CType(Me.dgvResults, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvResults
        '
        Me.dgvResults.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dgvResults.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvResults.Location = New System.Drawing.Point(34, 166)
        Me.dgvResults.Name = "dgvResults"
        Me.dgvResults.RowHeadersWidth = 51
        Me.dgvResults.RowTemplate.Height = 26
        Me.dgvResults.Size = New System.Drawing.Size(1138, 477)
        Me.dgvResults.TabIndex = 0
        '
        'btnReturn
        '
        Me.btnReturn.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnReturn.Location = New System.Drawing.Point(884, 111)
        Me.btnReturn.Name = "btnReturn"
        Me.btnReturn.Size = New System.Drawing.Size(102, 39)
        Me.btnReturn.TabIndex = 1
        Me.btnReturn.Text = "تسجيل"
        Me.btnReturn.UseVisualStyleBackColor = True
        '
        'txtRefPacking
        '
        Me.txtRefPacking.Location = New System.Drawing.Point(150, 32)
        Me.txtRefPacking.Name = "txtRefPacking"
        Me.txtRefPacking.Size = New System.Drawing.Size(203, 24)
        Me.txtRefPacking.TabIndex = 2
        '
        'btnSearch
        '
        Me.btnSearch.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSearch.Location = New System.Drawing.Point(195, 97)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(107, 36)
        Me.btnSearch.TabIndex = 3
        Me.btnSearch.Text = "بحث"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(31, 32)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(113, 21)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "رقم اذن البيع"
        '
        'lblUsername
        '
        Me.lblUsername.AutoSize = True
        Me.lblUsername.Location = New System.Drawing.Point(912, 13)
        Me.lblUsername.Name = "lblUsername"
        Me.lblUsername.Size = New System.Drawing.Size(47, 17)
        Me.lblUsername.TabIndex = 5
        Me.lblUsername.Text = "Label2"
        '
        'txtWorkOrder
        '
        Me.txtWorkOrder.Location = New System.Drawing.Point(600, 32)
        Me.txtWorkOrder.Name = "txtWorkOrder"
        Me.txtWorkOrder.Size = New System.Drawing.Size(203, 24)
        Me.txtWorkOrder.TabIndex = 6
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(500, 32)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(94, 24)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "أمر شغل"
        '
        'storefinishreturn
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1204, 1045)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtWorkOrder)
        Me.Controls.Add(Me.lblUsername)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnSearch)
        Me.Controls.Add(Me.txtRefPacking)
        Me.Controls.Add(Me.btnReturn)
        Me.Controls.Add(Me.dgvResults)
        Me.Name = "storefinishreturn"
        Me.Text = "storefinishreturn"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.dgvResults, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents dgvResults As DataGridView
    Friend WithEvents btnReturn As Button
    Friend WithEvents txtRefPacking As TextBox
    Friend WithEvents btnSearch As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents lblUsername As Label
    Friend WithEvents txtWorkOrder As TextBox
    Friend WithEvents Label2 As Label
End Class
