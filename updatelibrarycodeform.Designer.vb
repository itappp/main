<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class UpdateLibraryCodeForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.cmbWorkOrder = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbNewLibraryCode = New System.Windows.Forms.ComboBox()
        Me.btnUpdate = New System.Windows.Forms.Button()
        Me.dgvNewLibraryStages = New System.Windows.Forms.DataGridView()
        Me.dgvOldLibraryStages = New System.Windows.Forms.DataGridView()
        Me.lblOldLibraryCode = New System.Windows.Forms.Label()
        Me.txtnotes = New System.Windows.Forms.TextBox()
        Me.lblusername = New System.Windows.Forms.Label()
        CType(Me.dgvNewLibraryStages, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvOldLibraryStages, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmbWorkOrder
        '
        Me.cmbWorkOrder.FormattingEnabled = True
        Me.cmbWorkOrder.Location = New System.Drawing.Point(347, 68)
        Me.cmbWorkOrder.Name = "cmbWorkOrder"
        Me.cmbWorkOrder.Size = New System.Drawing.Size(246, 24)
        Me.cmbWorkOrder.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(401, 27)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(108, 24)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "أمر الشغل"
        '
        'cmbNewLibraryCode
        '
        Me.cmbNewLibraryCode.FormattingEnabled = True
        Me.cmbNewLibraryCode.Location = New System.Drawing.Point(111, 153)
        Me.cmbNewLibraryCode.Name = "cmbNewLibraryCode"
        Me.cmbNewLibraryCode.Size = New System.Drawing.Size(246, 24)
        Me.cmbNewLibraryCode.TabIndex = 2
        '
        'btnUpdate
        '
        Me.btnUpdate.Location = New System.Drawing.Point(568, 455)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(122, 59)
        Me.btnUpdate.TabIndex = 5
        Me.btnUpdate.Text = "Button1"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'dgvNewLibraryStages
        '
        Me.dgvNewLibraryStages.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dgvNewLibraryStages.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvNewLibraryStages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvNewLibraryStages.Location = New System.Drawing.Point(12, 220)
        Me.dgvNewLibraryStages.Name = "dgvNewLibraryStages"
        Me.dgvNewLibraryStages.RowHeadersWidth = 51
        Me.dgvNewLibraryStages.RowTemplate.Height = 26
        Me.dgvNewLibraryStages.Size = New System.Drawing.Size(461, 620)
        Me.dgvNewLibraryStages.TabIndex = 6
        '
        'dgvOldLibraryStages
        '
        Me.dgvOldLibraryStages.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dgvOldLibraryStages.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvOldLibraryStages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvOldLibraryStages.Location = New System.Drawing.Point(737, 248)
        Me.dgvOldLibraryStages.Name = "dgvOldLibraryStages"
        Me.dgvOldLibraryStages.RowHeadersWidth = 51
        Me.dgvOldLibraryStages.RowTemplate.Height = 26
        Me.dgvOldLibraryStages.Size = New System.Drawing.Size(461, 620)
        Me.dgvOldLibraryStages.TabIndex = 7
        '
        'lblOldLibraryCode
        '
        Me.lblOldLibraryCode.AutoSize = True
        Me.lblOldLibraryCode.Location = New System.Drawing.Point(939, 206)
        Me.lblOldLibraryCode.Name = "lblOldLibraryCode"
        Me.lblOldLibraryCode.Size = New System.Drawing.Size(47, 17)
        Me.lblOldLibraryCode.TabIndex = 8
        Me.lblOldLibraryCode.Text = "Label2"
        '
        'txtnotes
        '
        Me.txtnotes.Location = New System.Drawing.Point(746, 40)
        Me.txtnotes.Multiline = True
        Me.txtnotes.Name = "txtnotes"
        Me.txtnotes.Size = New System.Drawing.Size(404, 91)
        Me.txtnotes.TabIndex = 9
        '
        'lblusername
        '
        Me.lblusername.AutoSize = True
        Me.lblusername.Location = New System.Drawing.Point(57, 9)
        Me.lblusername.Name = "lblusername"
        Me.lblusername.Size = New System.Drawing.Size(47, 17)
        Me.lblusername.TabIndex = 10
        Me.lblusername.Text = "Label2"
        '
        'UpdateLibraryCodeForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1242, 955)
        Me.Controls.Add(Me.lblusername)
        Me.Controls.Add(Me.txtnotes)
        Me.Controls.Add(Me.lblOldLibraryCode)
        Me.Controls.Add(Me.dgvOldLibraryStages)
        Me.Controls.Add(Me.dgvNewLibraryStages)
        Me.Controls.Add(Me.btnUpdate)
        Me.Controls.Add(Me.cmbNewLibraryCode)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmbWorkOrder)
        Me.Name = "UpdateLibraryCodeForm"
        Me.Text = "updatelibrarycodeform"
        CType(Me.dgvNewLibraryStages, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvOldLibraryStages, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents cmbWorkOrder As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents cmbNewLibraryCode As ComboBox
    Friend WithEvents btnUpdate As Button
    Friend WithEvents dgvNewLibraryStages As DataGridView
    Friend WithEvents dgvOldLibraryStages As DataGridView
    Friend WithEvents lblOldLibraryCode As Label
    Friend WithEvents txtnotes As TextBox
    Friend WithEvents lblusername As Label
End Class
