<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ContractViewForm
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
        Me.Contract = New System.Windows.Forms.Label()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.dgvContracts = New System.Windows.Forms.DataGridView()
        Me.dgvContractDetails = New System.Windows.Forms.DataGridView()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.btnExportToExcel = New System.Windows.Forms.Button()
        Me.lblUsername = New System.Windows.Forms.Label()
        CType(Me.dgvContracts, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvContractDetails, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Contract
        '
        Me.Contract.AutoSize = True
        Me.Contract.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Contract.Location = New System.Drawing.Point(262, 13)
        Me.Contract.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Contract.Name = "Contract"
        Me.Contract.Size = New System.Drawing.Size(83, 21)
        Me.Contract.TabIndex = 2
        Me.Contract.Text = "Contract"
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(578, 13)
        Me.btnSearch.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(187, 46)
        Me.btnSearch.TabIndex = 3
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'dgvContracts
        '
        Me.dgvContracts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvContracts.Location = New System.Drawing.Point(12, 199)
        Me.dgvContracts.Name = "dgvContracts"
        Me.dgvContracts.RowHeadersWidth = 51
        Me.dgvContracts.RowTemplate.Height = 26
        Me.dgvContracts.Size = New System.Drawing.Size(1139, 246)
        Me.dgvContracts.TabIndex = 0
        '
        'dgvContractDetails
        '
        Me.dgvContractDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvContractDetails.Location = New System.Drawing.Point(13, 201)
        Me.dgvContractDetails.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.dgvContractDetails.Name = "dgvContractDetails"
        Me.dgvContractDetails.RowHeadersWidth = 51
        Me.dgvContractDetails.RowTemplate.Height = 26
        Me.dgvContractDetails.Size = New System.Drawing.Size(1301, 281)
        Me.dgvContractDetails.TabIndex = 4
        '
        'DataGridView1
        '
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Location = New System.Drawing.Point(499, 281)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.RowHeadersWidth = 51
        Me.DataGridView1.RowTemplate.Height = 26
        Me.DataGridView1.Size = New System.Drawing.Size(240, 150)
        Me.DataGridView1.TabIndex = 4
        '
        'btnExportToExcel
        '
        Me.btnExportToExcel.Location = New System.Drawing.Point(942, 49)
        Me.btnExportToExcel.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnExportToExcel.Name = "btnExportToExcel"
        Me.btnExportToExcel.Size = New System.Drawing.Size(187, 46)
        Me.btnExportToExcel.TabIndex = 5
        Me.btnExportToExcel.Text = "Export To Excell "
        Me.btnExportToExcel.UseVisualStyleBackColor = True
        '
        'lblUsername
        '
        Me.lblUsername.AutoSize = True
        Me.lblUsername.Location = New System.Drawing.Point(13, 12)
        Me.lblUsername.Name = "lblUsername"
        Me.lblUsername.Size = New System.Drawing.Size(38, 18)
        Me.lblUsername.TabIndex = 6
        Me.lblUsername.Text = "User"
        '
        'ContractViewForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 18.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1329, 549)
        Me.Controls.Add(Me.lblUsername)
        Me.Controls.Add(Me.btnExportToExcel)
        Me.Controls.Add(Me.dgvContractDetails)
        Me.Controls.Add(Me.btnSearch)
        Me.Controls.Add(Me.Contract)
        Me.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Name = "ContractViewForm"
        Me.Text = "ContractViewForm"
        CType(Me.dgvContracts, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvContractDetails, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Contract As System.Windows.Forms.Label
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents dgvContracts As System.Windows.Forms.DataGridView
    Friend WithEvents dgvContractDetails As System.Windows.Forms.DataGridView
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents btnExportToExcel As System.Windows.Forms.Button
    Friend WithEvents lblUsername As System.Windows.Forms.Label
End Class
