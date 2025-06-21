<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class PrintWorkOrderForm
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
        Me.webBrowserPreview = New System.Windows.Forms.WebBrowser()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnPrint2 = New System.Windows.Forms.Button()
        Me.btnExportToExcel = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'cmbWorkOrder
        '
        Me.cmbWorkOrder.Font = New System.Drawing.Font("Tahoma", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbWorkOrder.FormattingEnabled = True
        Me.cmbWorkOrder.Location = New System.Drawing.Point(451, 57)
        Me.cmbWorkOrder.Name = "cmbWorkOrder"
        Me.cmbWorkOrder.Size = New System.Drawing.Size(217, 30)
        Me.cmbWorkOrder.TabIndex = 0
        '
        'webBrowserPreview
        '
        Me.webBrowserPreview.Location = New System.Drawing.Point(25, 131)
        Me.webBrowserPreview.MinimumSize = New System.Drawing.Size(20, 20)
        Me.webBrowserPreview.Name = "webBrowserPreview"
        Me.webBrowserPreview.Size = New System.Drawing.Size(1380, 773)
        Me.webBrowserPreview.TabIndex = 1
        '
        'btnPrint
        '
        Me.btnPrint.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrint.Location = New System.Drawing.Point(268, 47)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(120, 47)
        Me.btnPrint.TabIndex = 2
        Me.btnPrint.Text = "Search"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(518, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(82, 21)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "أمر شغل"
        '
        'btnPrint2
        '
        Me.btnPrint2.Location = New System.Drawing.Point(994, 47)
        Me.btnPrint2.Name = "btnPrint2"
        Me.btnPrint2.Size = New System.Drawing.Size(75, 23)
        Me.btnPrint2.TabIndex = 4
        Me.btnPrint2.Text = "Button1"
        Me.btnPrint2.UseVisualStyleBackColor = True
        Me.btnPrint2.Visible = False
        '
        'btnExportToExcel
        '
        Me.btnExportToExcel.Location = New System.Drawing.Point(780, 85)
        Me.btnExportToExcel.Name = "btnExportToExcel"
        Me.btnExportToExcel.Size = New System.Drawing.Size(75, 23)
        Me.btnExportToExcel.TabIndex = 5
        Me.btnExportToExcel.Text = "Button1"
        Me.btnExportToExcel.UseVisualStyleBackColor = True
        Me.btnExportToExcel.Visible = False
        '
        'PrintWorkOrderForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1445, 1011)
        Me.Controls.Add(Me.btnExportToExcel)
        Me.Controls.Add(Me.btnPrint2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnPrint)
        Me.Controls.Add(Me.webBrowserPreview)
        Me.Controls.Add(Me.cmbWorkOrder)
        Me.Name = "PrintWorkOrderForm"
        Me.Text = "printworkorderform"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents cmbWorkOrder As ComboBox
    Friend WithEvents webBrowserPreview As WebBrowser
    Friend WithEvents btnPrint As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents btnPrint2 As Button
    Friend WithEvents btnExportToExcel As Button
End Class
